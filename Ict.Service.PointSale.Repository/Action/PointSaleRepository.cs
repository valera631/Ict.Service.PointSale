using AutoMapper;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.DataBase.DBModels;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Chief;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Models.Location;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Models.Update;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.Repository.Action
{
    public class PointSaleRepository : IPointSaleRepository
    {

        private readonly PointSaleDbContext _pointSaleDbContext;
        private readonly IMapper _mapper;

        public PointSaleRepository(PointSaleDbContext pointSaleDbContext, IMapper mapper)
        {
            _pointSaleDbContext = pointSaleDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Асинхронно добавляет связь между торговой точкой и оператором.
        /// </summary>
        public async Task<OperationResult<Guid?>> AddOperatorToPointSaleAsync(LinkOperatorDto linkOperatorDto)
        {
            OperationResult<Guid?> response = new();
            try
            {
                var pointSale =  _pointSaleDbContext.PointSaleEntities
                    .Include(o => o.Operators)
                    .FirstOrDefault(o => o.PointSaleId == linkOperatorDto.PointSaleId);

                if (pointSale == null)
                {
                    response.ErrorMessage = $"Point sale with ID {linkOperatorDto.PointSaleId} not found.";
                    return response;
                }

                // Проверяем, есть ли уже оператор с таким ID в точке продаж
                if (pointSale.Operators.Any(o => o.OperatorId == linkOperatorDto.OperatorId))
                {
                    response.ErrorMessage = $"Operator with ID {linkOperatorDto.OperatorId} is already linked to point sale {linkOperatorDto.PointSaleId}.";
                    return response;
                }

                // Проверяем, существует ли оператор
                var operatorEntity = await _pointSaleDbContext.Operators
                    .FirstOrDefaultAsync(op => op.OperatorId == linkOperatorDto.OperatorId);

                // Если оператора нет, создаём его
                if (operatorEntity == null)
                {
                    operatorEntity = new Operator
                    {
                        OperatorId = linkOperatorDto.OperatorId
                        // Другие обязательные поля Operator, если есть, нужно заполнить
                    };
                    _pointSaleDbContext.Operators.Add(operatorEntity);
                }

                // Добавляем оператора в коллекцию
                pointSale.Operators.Add(operatorEntity);


                // Сохраняем изменения
                await _pointSaleDbContext.SaveChangesAsync();

                response.Data = linkOperatorDto.OperatorId;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// Асинхронно создает новую точку продаж на основе предоставленных данных.
        /// </summary>
        public async Task<OperationResult<bool>> CreatePointSale(PointSaleFullDto pointSaleDto)
        {
            OperationResult<bool> response = new();
            using var transaction = _pointSaleDbContext.Database.BeginTransaction();
            try
            {
                var pointSale = _mapper.Map<PointSaleEntity>(pointSaleDto.PointSale);
                var pointSaleActivity = _mapper.Map<PointSaleActivity>(pointSaleDto.PointSaleActivity);


                if (pointSaleDto.Chief != null)
                {
                    var chief = _mapper.Map<Chief>(pointSaleDto.Chief);
                    _pointSaleDbContext.Chiefs.Add(chief);
                }

                if (pointSaleDto.Description != null)
                {
                    var description = _mapper.Map<Description>(pointSaleDto.Description);
                    _pointSaleDbContext.Descriptions.Add(description);
                }

                if(pointSaleDto.PointSaleSchedules != null && pointSaleDto.PointSaleSchedules.Any())
                {
                    var schedules = _mapper.Map<List<PointSaleSchedule>>(pointSaleDto.PointSaleSchedules);
                    _pointSaleDbContext.PointSaleSchedules.AddRange(schedules);
                }


                if(pointSaleDto.Location != null)
                {
                    var location = _mapper.Map<Location>(pointSaleDto.Location);
                    _pointSaleDbContext.Locations.Add(location);
                }

                if (pointSaleDto.AlternativeName != null && pointSaleDto.AlternativeName.Any())
                {
                    var alternativeWords = pointSaleDto.AlternativeName
                        .Select(name => new AlternativeWord
                        {
                            AlternativeWordId = Guid.NewGuid(),
                            PointSaleId = pointSale.PointSaleId,
                            AlternativeWordName = name,
                        })
                        .ToList();
                    _pointSaleDbContext.AlternativeWords.AddRange(alternativeWords);
                }

                await _pointSaleDbContext.PointSaleEntities.AddAsync(pointSale);
                await _pointSaleDbContext.PointSaleActivities.AddAsync(pointSaleActivity);

                if ( pointSaleDto.CategoryIds != null && pointSaleDto.CategoryIds.Any())
                {
                    // Optionally validate that CategoryIds exist
                    var validCategoryIds = await _pointSaleDbContext.CategoryPointSales
                        .Where(c => pointSaleDto.CategoryIds.Contains(c.CategoryId) && c.IsEnabled)
                        .Select(c => c.CategoryId)
                        .ToListAsync();

                    if (validCategoryIds.Count != pointSaleDto.CategoryIds.Count)
                    {
                        response.ErrorMessage = "Один или несколько указанных идентификаторов категорий недействительны или неактивны.";
                        await transaction.RollbackAsync();             
                        return response;
                    }


                    pointSale.CategoryPointSales = await _pointSaleDbContext.CategoryPointSales
                       .Where(c => pointSaleDto.CategoryIds.Contains(c.CategoryId))
                       .ToListAsync();
                }

                await _pointSaleDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.ErrorMessage = ex.Message;
            }
            return response;
        }


        /// <summary>
        /// Асинхронно получает идентификаторы точек продаж по фильтру.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<OperationResult<PaginatedResult<Guid>>> GetFilteredPointsSaleAsync(PointSaleFilter filter)
        {
            var response = new OperationResult<PaginatedResult<Guid>>();
            var result = new PaginatedResult<Guid>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
            try
            {
                IQueryable<PointSaleActivity> query = _pointSaleDbContext.PointSaleActivities;

                if (!string.IsNullOrEmpty(filter.QueryString))
                {
                    query = query.Where(activity => EF.Functions.Like(activity.NamePointSale, $"%{filter.QueryString}%"));
                }

                var pointQuery = query.Select(activity => activity.PointSales);

                if (filter.IsApproved.HasValue)
                {
                    pointQuery = pointQuery.Where(point => point.IsAproved == filter.IsApproved.Value);
                }

                if (filter.HasOperator.HasValue)
                {
                    pointQuery = pointQuery.Where(entity =>
                        filter.HasOperator.Value
                            ? entity.Operators.Any()
                            : !entity.Operators.Any());
                }

                if (filter.OperatorId.HasValue)
                {
                    pointQuery = pointQuery.Where(entity =>
                        entity.Operators.Any(op => op.OperatorId == filter.OperatorId.Value));
                }

                    result.TotalCount = await pointQuery
               .Distinct()
               .CountAsync();


                var pointSaleIds = await pointQuery
                   .Select(entity => entity.PointSaleId)
                   .Distinct()
                   .Skip((filter.PageNumber - 1) * filter.PageSize)
                   .Take(filter.PageSize)
                   .ToListAsync();

                if(!pointSaleIds.Any())
                {
                    result.Items = new List<Guid>();
                    response.Data = result;
                    return response;
                }

                result.Items = pointSaleIds;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Асинхронно получает точки продаж по идентификатору владельца.
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public async Task<OperationResult<List<PointSaleResultFullDto>>> GetByOperatorAsync(Guid operatorId)
        {
            OperationResult<List<PointSaleResultFullDto>> response = new();

            try
            {
                var targetDate = DateOnly.FromDateTime(DateTime.Now);
                var pointSaleDtos = new List<PointSaleResultFullDto>();


                var pointSales = await _pointSaleDbContext.PointSaleEntities
                    .Where(x => x.Operators.Any(op => op.OperatorId == operatorId))
                    .Select(x => new
                    {
                        PointSaleEntity = x,
                        PointSaleActivity = x.PointSaleActivities
                            .Where(a => a.OpenDate <= targetDate)
                            .OrderByDescending(a => a.OpenDate)
                            .FirstOrDefault(),
                        Location = x.Locations
                            .Where(l => l.OpenDate <= targetDate)
                            .OrderByDescending(l => l.OpenDate)
                            .FirstOrDefault(),
                        Description = x.Descriptions
                            .Where(d => d.OpenDate <= targetDate)
                            .OrderByDescending(d => d.EntryDate)
                            .FirstOrDefault(),
                        Chief = x.Chiefs
                            .Where(c => c.OpenDate <= targetDate)
                            .OrderByDescending(c => c.OpenDate)
                            .FirstOrDefault(),
                        Operators = x.Operators
                            .Select(op => op.OperatorId)
                            .ToList()
                    })
                    .AsNoTracking()
                    .AsSplitQuery()
                    .ToListAsync();

                if (!pointSales.Any())
                {
                    response.Data = pointSaleDtos; // Empty list
                    return response;
                }

                var pointSaleIds = pointSales.Select(ps => ps.PointSaleEntity.PointSaleId).ToList();
                var creationTypeIds = pointSales.Select(ps => ps.PointSaleEntity.CreationTypeId).Distinct().ToList();
                var organizationTypeIds = pointSales.Select(ps => ps.PointSaleEntity.OrganizationTypeId).Distinct().ToList();
                var closingStatusIds = pointSales.Select(ps => ps.PointSaleEntity.ClosingStatusId).OfType<int>().Distinct().ToList();
                var ownerTypeIds = pointSales.Select(ps => ps.PointSaleEntity.OwnerTypeId).Distinct().ToList();
                var chiefPositionIds = pointSales.Where(ps => ps.Chief != null).Select(ps => ps.Chief.ChiefPositionId).Distinct().ToList();

                // Fetch reference data
                var creationTypes = await _pointSaleDbContext.CreationTypes
                    .Where(ct => creationTypeIds.Contains(ct.CreationTypeId))
                    .Select(ct => new { ct.CreationTypeId, ct.CreationTypeName })
                    .AsNoTracking()
                    .ToListAsync();

                var organizationTypes = await _pointSaleDbContext.OrganizationTypes
                    .Where(ot => organizationTypeIds.Contains(ot.OrganizationTypeId))
                    .Select(ot => new { ot.OrganizationTypeId, ot.NameType })
                    .AsNoTracking()
                    .ToListAsync();

                var closingStatuses = await _pointSaleDbContext.ClosingStatuses
                    .Where(cs => closingStatusIds.Contains(cs.ClosingStatusId))
                    .Select(cs => new { cs.ClosingStatusId, cs.NameStatus })
                    .AsNoTracking()
                    .ToListAsync();

                var ownerTypes = await _pointSaleDbContext.OwnerTypes
                    .Where(ot => ownerTypeIds.Contains(ot.OwnerTypeId))
                    .Select(ot => new { ot.OwnerTypeId, ot.NameType })
                    .AsNoTracking()
                    .ToListAsync();

                var chiefPositions = await _pointSaleDbContext.ChiefPositions
                    .Where(p => chiefPositionIds.Contains(p.ChiefPositionId))
                    .Select(p => new { p.ChiefPositionId, p.PositionName })
                    .AsNoTracking()
                    .ToListAsync();

              
                var creationTypeDict = creationTypes.ToDictionary(ct => ct.CreationTypeId, ct => ct.CreationTypeName);
                var organizationTypeDict = organizationTypes.ToDictionary(ot => ot.OrganizationTypeId, ot => ot.NameType);
                var closingStatusDict = closingStatuses.ToDictionary(cs => cs.ClosingStatusId, cs => cs.NameStatus);
                var ownerTypeDict = ownerTypes.ToDictionary(ot => ot.OwnerTypeId, ot => ot.NameType);
                var chiefPositionDict = chiefPositions.ToDictionary(p => p.ChiefPositionId, p => p.PositionName);


                foreach (var ps in pointSales)
                {
                    var pointSaleDto = new PointSaleResultFullDto
                    {
                        // Main properties
                        PointSaleId = ps.PointSaleEntity.PointSaleId,
                        PointSaleName = ps.PointSaleActivity != null ? ps.PointSaleActivity.NamePointSale ?? "No name" : "No name",
                        OwnerId = ps.PointSaleEntity.OwnerId,
                        OwnerTypeId = ps.PointSaleEntity.OwnerTypeId,
                        OwnerTypeName = ps.PointSaleEntity.OwnerTypeId.HasValue
                                ? ownerTypeDict.GetValueOrDefault(ps.PointSaleEntity.OwnerTypeId.Value, string.Empty)
                                : string.Empty,
                        OpenDateActivity = ps.PointSaleActivity?.OpenDate ?? default,
                        CreationDatePointSale = ps.PointSaleEntity.CreationDate,
                        // Location info
                        LocationId = ps.Location?.LocationId ?? Guid.Empty,
                        OpenDateLocation = ps.Location?.OpenDate ?? default,
                        Address = ps.Location?.Address ?? string.Empty,
                        CreationDateLocation = ps.Location?.EntryDate ?? default,

                        // Types and statuses
                        CreationTypeId = ps.PointSaleEntity.CreationTypeId,
                        CreationTypeName = ps.PointSaleEntity.CreationTypeId.HasValue
                            ? creationTypeDict.GetValueOrDefault(ps.PointSaleEntity.CreationTypeId.Value, null)
                            : null,
                        OrganizationTypeId = ps.PointSaleEntity.OrganizationTypeId,
                        OrganizationTypeName = ps.PointSaleEntity.OrganizationTypeId.HasValue
                            ? organizationTypeDict.GetValueOrDefault(ps.PointSaleEntity.OrganizationTypeId.Value, string.Empty)
                            : null,
                        ClosingStatusId = ps.PointSaleEntity.ClosingStatusId,
                        ClosingStatusName = ps.PointSaleEntity.ClosingStatusId.HasValue
                            ? closingStatusDict.GetValueOrDefault(ps.PointSaleEntity.ClosingStatusId.Value, string.Empty)
                            : string.Empty,

                        // Metadata
                        IsAproved = ps.PointSaleEntity.IsAproved,
                        Version = ps.PointSaleEntity.Version,
                        EntryDate = ps.PointSaleEntity.EntryDate,
                        ClosingDate = ps.PointSaleEntity.ClosingDate,
                        DescriptionText = ps.Description?.DescriptionText ?? "No description",
                        OperatorIds = ps.Operators,

                        // Chief info
                        ChiefId = ps.Chief?.ChiefId ?? Guid.Empty,
                        OpenDateChief = ps.Chief?.OpenDate ?? default,
                        ChiefName = ps.Chief?.ChiefName ?? "No chief assigned",
                        ChiefPositionId = ps.Chief?.ChiefPositionId ?? 0,
                        ChiefPositionName = ps.Chief != null && ps.Chief.ChiefPositionId != 0
                            ? chiefPositionDict.GetValueOrDefault(ps.Chief.ChiefPositionId, "No position assigned")
                            : "No position assigned"
                    };

                    pointSaleDtos.Add(pointSaleDto);
                }

                response.Data = pointSaleDtos;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error retrieving point sales for operator ID {operatorId}: {ex.Message}";
                return response;
            }

            return response;
        }

        /// <summary>
        /// Получает полную DTO-модель торговой точки с деталями на указанную дату или на текущую дату, если дата не указана.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор торговой точки.</param>
        /// <param name="dateOnly">Дата, на которую нужно получить информацию (необязательно).</param>
        /// <returns>Результат операции с полной DTO торговой точки.</returns>
        public async Task<OperationResult<PointSaleFullDto>> GetPointSaleDtoAsync(Guid pointSaleId, DateOnly? dateOnly)
        {
            var response = new OperationResult<PointSaleFullDto>();
            try
            {
                // Вычисляем эффективную дату — переданную или текущую
                var effectiveDate = dateOnly ?? DateOnly.FromDateTime(DateTime.Now);

                // Загружаем основную информацию о торговой точке и связанные данные по effectiveDate
                var pointSaleData = await _pointSaleDbContext.PointSaleEntities
                    .Where(x => x.PointSaleId == pointSaleId &&
                                x.PointSaleActivities.Any(a => a.OpenDate <= effectiveDate))
                    .Select(x => new
                    {
                        PointSaleEntity = x,
                        PointSaleActivity = x.PointSaleActivities
                            .Where(a => a.OpenDate <= effectiveDate)
                            .OrderByDescending(a => a.OpenDate)
                            .FirstOrDefault(),
                        Location = x.Locations
                            .Where(l => l.OpenDate <= effectiveDate)
                            .OrderByDescending(l => l.OpenDate)
                            .FirstOrDefault(),
                        Description = x.Descriptions
                            .Where(d => d.OpenDate <= effectiveDate)
                            .OrderByDescending(d => d.OpenDate) // Assuming EntryDate is used for description versioning
                            .FirstOrDefault(),
                        Chief = x.Chiefs
                            .Where(c => c.OpenDate <= effectiveDate)
                            .OrderByDescending(c => c.OpenDate)
                            .FirstOrDefault(),

                        AlternativeNames = x.AlternativeWords.Select(an => an.AlternativeWordName).ToList(),
                        CategoryIds = x.CategoryPointSales.Select(c => c.CategoryId).ToList(),
                        PointSaleScheduleIds = x.PointSaleSchedules.Select(s => s.PointSaleScheduleId).ToList()
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (pointSaleData == null)
                {
                    response.ErrorMessage = $"Point sale with ID {pointSaleId} not found or no active activities on date {effectiveDate}.";
                    return response;
                }

                var references = await _pointSaleDbContext.CreationTypes
                    .AsSplitQuery()
                    .Where(ct => ct.CreationTypeId == pointSaleData.PointSaleEntity.CreationTypeId)
                    .Select(ct => new
                    {
                        ct.CreationTypeName,
                        OrganizationTypeName = _pointSaleDbContext.OrganizationTypes
                            .Where(ot => ot.OrganizationTypeId == pointSaleData.PointSaleEntity.OrganizationTypeId)
                            .Select(ot => ot.NameType)
                            .FirstOrDefault(),
                        ClosingStatusName = pointSaleData.PointSaleEntity.ClosingStatusId.HasValue
                            ? _pointSaleDbContext.ClosingStatuses
                                .Where(cs => cs.ClosingStatusId == pointSaleData.PointSaleEntity.ClosingStatusId)
                                .Select(cs => cs.NameStatus)
                                .FirstOrDefault()
                            : null,
                        OwnerTypeName = _pointSaleDbContext.OwnerTypes
                            .Where(ot => ot.OwnerTypeId == pointSaleData.PointSaleEntity.OwnerTypeId)
                            .Select(ot => ot.NameType)
                            .FirstOrDefault(),
                        ChiefPositionName = pointSaleData.Chief != null && pointSaleData.Chief.ChiefPositionId != 0
                            ? _pointSaleDbContext.ChiefPositions
                                .Where(p => p.ChiefPositionId == pointSaleData.Chief.ChiefPositionId)
                                .Select(p => p.PositionName)
                                .FirstOrDefault()
                            : null
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                var pointSaleSchedules = await _pointSaleDbContext.PointSaleSchedules
                    .Where(s => pointSaleData.PointSaleScheduleIds.Contains(s.PointSaleScheduleId))
                    .AsNoTracking()
                    .ToListAsync();

                response.Data = new PointSaleFullDto
                {
                    PointSale = new PointSaleDto
                    {
                        PointSaleId = pointSaleData.PointSaleEntity.PointSaleId,
                        OwnerId = pointSaleData.PointSaleEntity.OwnerId,
                        OwnerTypeId = pointSaleData.PointSaleEntity.OwnerTypeId,
                        OwnerName = pointSaleData.PointSaleEntity.OwnerName ?? string.Empty,
                        CreationTypeId = pointSaleData.PointSaleEntity.CreationTypeId,
                        OrganizationTypeId = pointSaleData.PointSaleEntity.OrganizationTypeId,
                        EntryDate = pointSaleData.PointSaleEntity.EntryDate,
                        ClosingStatusId = pointSaleData.PointSaleEntity.ClosingStatusId,
                        CreationDate = pointSaleData.PointSaleEntity.CreationDate,
                        ClosingDate = pointSaleData.PointSaleEntity.ClosingDate
                    },
                    PointSaleActivity = pointSaleData.PointSaleActivity != null
                        ? new PointSaleActivityDto
                        {
                            PointSaleActivityId = pointSaleData.PointSaleActivity.PointSaleActivityId,
                            NamePointSale = pointSaleData.PointSaleActivity.NamePointSale ?? "No name",
                            EnglishNamePointSale = pointSaleData.PointSaleActivity.EnglishNamePointSale,
                            PointSaleId = pointSaleData.PointSaleActivity.PointSaleId,
                            OpenDate = pointSaleData.PointSaleActivity.OpenDate, // Or some other appropriate default
                            EntryDate = pointSaleData.PointSaleActivity.EntryDate
                        }
                        : new PointSaleActivityDto { NamePointSale = "No name", PointSaleActivityId = Guid.Empty, PointSaleId = pointSaleId, OpenDate = effectiveDate, EntryDate = DateTime.Now }, // Fallback for activity
                    Description = pointSaleData.Description != null
                        ? new DescriptionDto
                        {
                            DescriptionId = pointSaleData.Description.DescriptionId,
                            DescriptionText = pointSaleData.Description.DescriptionText ?? string.Empty,
                            OpenDate = pointSaleData.Description.OpenDate,
                            PointSaleId = pointSaleData.Description.PointSaleId,
                            EntryDate = pointSaleData.Description.EntryDate
                        }
                        : null,
                    Chief = pointSaleData.Chief != null
                        ? new ChiefDto
                        {
                            ChiefId = pointSaleData.Chief.ChiefId,
                            OpenDate = pointSaleData.Chief.OpenDate,
                            ChiefName = pointSaleData.Chief.ChiefName ?? string.Empty,
                            ChiefPositionId = pointSaleData.Chief.ChiefPositionId,
                            PointSaleId = pointSaleData.Chief.PointSaleId,
                            EntryDate = pointSaleData.Chief.EntryDate
                        }
                        : null,
                    Location = pointSaleData.Location != null
                        ? new LocationDto
                        {
                            LocationId = pointSaleData.Location.LocationId,
                            OpenDate = pointSaleData.Location.OpenDate,
                            Address = pointSaleData.Location.Address ?? string.Empty,
                            Latitude = pointSaleData.Location.Latitude,
                            Longitude = pointSaleData.Location.Longitude,
                            AddressId = pointSaleData.Location.AddressId,
                            PointSaleId = pointSaleData.Location.PointSaleId,
                            EntryDate = pointSaleData.Location.EntryDate
                        }
                        : null,
                    PointSaleSchedules = pointSaleSchedules.Select(s => new PointSaleScheduleDto
                    {
                        PointSaleScheduleId = s.PointSaleScheduleId,
                        PointSaleId = s.PointSaleId,
                        DayOfWeek = s.DayOfWeek,
                        IsWorkingDay = s.IsWorkingDay,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        BreakStartTime = s.BreakStartTime,
                        BreakEndTime = s.BreakEndTime
                    }).ToList(),
                    CategoryIds = pointSaleData.CategoryIds, // PointSaleFullDto has CategoryIds, but your query gives CategoryNames. Adjust as needed.
                    AlternativeName = pointSaleData.AlternativeNames // This field is not populated by your query.
                };


                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error retrieving point sale DTO with ID {pointSaleId}: {ex.Message}";
                return response;
            }
        }

        public async Task<OperationResult<PointSaleResultFullDto>> GetPointSaleByIdAsync(Guid pointSaleId, DateOnly? targetDate)
        {
            var response = new OperationResult<PointSaleResultFullDto>();
            try
            {

                var effectiveDate = targetDate ?? DateOnly.FromDateTime(DateTime.Now); // Текущая дата

                var pointSale = await _pointSaleDbContext.PointSaleEntities
                    .Where(x => x.PointSaleId == pointSaleId &&
                                x.PointSaleActivities.Any(a => a.OpenDate <= effectiveDate))
                    .Select(x => new
                    {
                        PointSaleEntity = x,
                        PointSaleActivity = x.PointSaleActivities
                            .Where(a => a.OpenDate <= effectiveDate)
                            .OrderByDescending(a => a.OpenDate)
                            .FirstOrDefault(),
                        Location = x.Locations
                            .Where(l => l.OpenDate <= effectiveDate)
                            .OrderByDescending(l => l.OpenDate)
                            .FirstOrDefault(),
                        Description = x.Descriptions
                            .Where(d => d.OpenDate <= effectiveDate)
                            .OrderByDescending(d => d.EntryDate)
                            .FirstOrDefault(),
                        Chief = x.Chiefs
                            .Where(c => c.OpenDate <= effectiveDate)
                            .OrderByDescending(c => c.OpenDate)
                            .FirstOrDefault(),
                        Operators = x.Operators // Добавляем операторов
                            .Select(op => op.OperatorId)
                            .ToList(),
                        CategoryNames = x.CategoryPointSales.Select(c => c.Name).ToList(),

                        PointSaleScheduleIds = x.PointSaleSchedules.Select(s => s.PointSaleScheduleId).ToList()
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (pointSale == null)
                {
                    response.ErrorMessage = $"Point sale with ID {pointSaleId} not found or no active activities on date {effectiveDate}.";
                    return response;
                }

                var references = await _pointSaleDbContext.CreationTypes
                 .AsSplitQuery()
                 .Select(ct => new
                {
                    ct.CreationTypeName,
                    OrganizationTypeName = _pointSaleDbContext.OrganizationTypes
                        .Where(ot => ot.OrganizationTypeId == pointSale.PointSaleEntity.OrganizationTypeId)
                        .Select(ot => ot.NameType)
                        .FirstOrDefault(),
                    ClosingStatusName = pointSale.PointSaleEntity.ClosingStatusId.HasValue
                        ? _pointSaleDbContext.ClosingStatuses
                        .Where(cs => cs.ClosingStatusId == pointSale.PointSaleEntity.ClosingStatusId)
                        .Select(cs => cs.NameStatus)
                        .FirstOrDefault()
                     : null,
                     OwnerTypeName = _pointSaleDbContext.OwnerTypes
                        .Where(ot => ot.OwnerTypeId == pointSale.PointSaleEntity.OwnerTypeId)
                        .Select(ot => ot.NameType)
                        .FirstOrDefault(),

                     ChiefPositionName = pointSale.Chief != null && pointSale.Chief.ChiefPositionId != 0
                       ? _pointSaleDbContext.ChiefPositions
                         .Where(p => p.ChiefPositionId == pointSale.Chief.ChiefPositionId)
                         .Select(p => p.PositionName)
                         .FirstOrDefault()
                      : null
                })
               .AsNoTracking()
               .FirstOrDefaultAsync();

                var pointSaleSchedules = await _pointSaleDbContext.PointSaleSchedules
                    .Where(s => pointSale.PointSaleScheduleIds.Contains(s.PointSaleScheduleId))
                    .AsNoTracking()
                    .ToListAsync();

                var pointSaleResult = new PointSaleResultFullDto
                {
                    // Основные характеристики
                    PointSaleId = pointSale.PointSaleEntity.PointSaleId,
                    PointSaleName = pointSale.PointSaleActivity != null ? pointSale.PointSaleActivity.NamePointSale ?? "No name" : "No name",
                    OwnerId = pointSale.PointSaleEntity.OwnerId,
                    OwnerName = pointSale.PointSaleEntity.OwnerName ?? string.Empty, 
                    OwnerTypeId = pointSale.PointSaleEntity.OwnerTypeId,
                    OwnerTypeName = references?.OwnerTypeName ?? string.Empty,
                    OpenDateActivity = pointSale.PointSaleActivity.OpenDate,
                    CreationDatePointSale = pointSale.PointSaleEntity.CreationDate,

                    // Информация о локации
                    LocationId = pointSale.Location?.LocationId ?? Guid.Empty,
                    OpenDateLocation = pointSale.Location?.OpenDate,
                    Address = pointSale.Location?.Address ?? string.Empty,
                    CreationDateLocation = pointSale.Location?.EntryDate,
                    Longitude = pointSale.Location?.Longitude ?? 0f,
                    Latitude = pointSale.Location?.Latitude ?? 0f,

                    // Типы и статусы
                    CreationTypeId = pointSale.PointSaleEntity.CreationTypeId,
                    CreationTypeName = references?.CreationTypeName ?? string.Empty,
                    OrganizationTypeId = pointSale.PointSaleEntity.OrganizationTypeId,
                    OrganizationTypeName = references?.OrganizationTypeName ?? string.Empty,
                    ClosingStatusId = pointSale.PointSaleEntity.ClosingStatusId,
                    ClosingStatusName = references?.ClosingStatusName ?? string.Empty,

                    // Метаданные
                    IsAproved = pointSale.PointSaleEntity.IsAproved,
                    Version = pointSale.PointSaleEntity.Version,
                    EntryDate = pointSale.PointSaleEntity.EntryDate,
                    ClosingDate = pointSale.PointSaleEntity.ClosingDate,
                    DescriptionText = pointSale.Description?.DescriptionText ?? string.Empty,
                    OperatorIds = pointSale.Operators,

                    // Информация о руководителе
                    ChiefId = pointSale.Chief?.ChiefId ?? Guid.Empty,
                    OpenDateChief = pointSale.Chief?.OpenDate,
                    ChiefName = pointSale.Chief?.ChiefName ?? string.Empty,
                    ChiefPositionId = pointSale.Chief?.ChiefPositionId ?? 0,
                    ChiefPositionName = references?.ChiefPositionName ?? string.Empty,


                     CategoryNames = pointSale.CategoryNames,

                    Schedules = pointSaleSchedules.Select(s => new PointSaleScheduleDto // Assuming you have an OrganizationScheduleDto
                    {
                        PointSaleScheduleId = s.PointSaleScheduleId,
                        PointSaleId = s.PointSaleId,
                        DayOfWeek = s.DayOfWeek,
                        IsWorkingDay = s.IsWorkingDay,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        BreakStartTime = s.BreakStartTime,
                        BreakEndTime = s.BreakEndTime
                    }).ToList()
                };

                response.Data = pointSaleResult;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error retrieving point sale with ID {pointSaleId}: {ex.Message}";
                return response;
            }
        }



        public async Task<OperationResult<List<PointSaleResultFullDto>>> GetPointSaleByOwnerIdAsync(Guid ownerId)
        {
            OperationResult<List<PointSaleResultFullDto>> response = new();
            try
            {
                var targetDate = DateOnly.FromDateTime(DateTime.Now);

                // Основной запрос: получаем все точки продаж по OwnerId
                var pointSales = await _pointSaleDbContext.PointSaleEntities
                    .Where(x => x.OwnerId == ownerId &&
                                x.PointSaleActivities.Any(a => a.OpenDate <= targetDate))
                    .Select(x => new
                    {
                        PointSaleEntity = x,
                        PointSaleActivity = x.PointSaleActivities
                            .Where(a => a.OpenDate <= targetDate)
                            .OrderByDescending(a => a.OpenDate)
                            .FirstOrDefault(),
                        Location = x.Locations
                            .Where(l => l.OpenDate <= targetDate)
                            .OrderByDescending(l => l.OpenDate)
                            .FirstOrDefault(),
                        Description = x.Descriptions
                            .Where(d => d.OpenDate <= targetDate)
                            .OrderByDescending(d => d.EntryDate)
                            .FirstOrDefault(),
                        Chief = x.Chiefs
                            .Where(c => c.OpenDate <= targetDate)
                            .OrderByDescending(c => c.OpenDate)
                            .FirstOrDefault(),

                        PointSaleScheduleIds = x.PointSaleSchedules.Select(s => s.PointSaleScheduleId).ToList()
                    })
                    .AsNoTracking()
                    .ToListAsync();

                if (pointSales == null || !pointSales.Any())
                {
                    response.ErrorMessage = $"No active point sales found for owner ID {ownerId} on date {targetDate}.";
                    return response;
                }

                var pointIds = pointSales.Select(x => x.PointSaleEntity.PointSaleId).ToList();

                var pointSaleSchedules = await _pointSaleDbContext.PointSaleSchedules
                    .Where(s => pointIds.Contains(s.PointSaleId))
                    .AsNoTracking()
                    .ToListAsync();


                // Получаем ссылочные данные для всех точек продаж
                var pointSaleIds = pointSales.Select(x => x.PointSaleEntity.PointSaleId).ToList();


                var references = await _pointSaleDbContext.CreationTypes
                    .AsSplitQuery()
                    .Where(ct => pointSales.Select(ps => ps.PointSaleEntity.CreationTypeId).Contains(ct.CreationTypeId))
                    .SelectMany(ct => _pointSaleDbContext.PointSaleEntities
                        .Where(ps => ps.CreationTypeId == ct.CreationTypeId && pointSaleIds.Contains(ps.PointSaleId))
                        .Select(ps => new
                        {
                            PointSaleId = ps.PointSaleId,
                            CreationTypeName = ct.CreationTypeName,
                            OrganizationTypeName = _pointSaleDbContext.OrganizationTypes
                                .Where(ot => ot.OrganizationTypeId == ps.OrganizationTypeId)
                                .Select(ot => ot.NameType)
                                .FirstOrDefault(),
                            OwnerTypeName = _pointSaleDbContext.OwnerTypes
                                .Where(ot => ot.OwnerTypeId == ps.OwnerTypeId)
                                .Select(ot => ot.NameType)
                                .FirstOrDefault(),
                            ClosingStatusName = ps.ClosingStatusId.HasValue
                                ? _pointSaleDbContext.ClosingStatuses
                                    .Where(cs => cs.ClosingStatusId == ps.ClosingStatusId)
                                    .Select(cs => cs.NameStatus)
                                    .FirstOrDefault()
                                : null,

                            ChiefPositionName = ps.Chiefs.Any(c => c.OpenDate <= targetDate)
                                ? _pointSaleDbContext.ChiefPositions
                                    .Where(p => p.ChiefPositionId == ps.Chiefs
                                        .Where(c => c.OpenDate <= targetDate)
                                        .OrderByDescending(c => c.OpenDate)
                                        .FirstOrDefault().ChiefPositionId)
                                    .Select(p => p.PositionName)
                                    .FirstOrDefault()
                                : null,
                        }))
                    .AsNoTracking()
                    .ToListAsync();


                // Маппинг в PointSaleResultFullDto
                var pointSaleResults = pointSales.Select(ps =>
                {
                    var refData = references.FirstOrDefault(r => r.PointSaleId == ps.PointSaleEntity.PointSaleId);
                    return new PointSaleResultFullDto
                    {
                        // Основные характеристики
                        PointSaleId = ps.PointSaleEntity.PointSaleId,
                        OwnerId = ps.PointSaleEntity.OwnerId,
                        OwnerTypeId = ps.PointSaleEntity.OwnerTypeId,
                        OwnerTypeName = refData?.OwnerTypeName ?? string.Empty,
                        PointSaleName = ps.PointSaleActivity.NamePointSale,
                        CreationDatePointSale = ps.PointSaleEntity.CreationDate,

                        // Информация о локации
                        LocationId = ps.Location?.LocationId ?? Guid.Empty,
                        OpenDateLocation = ps.Location?.OpenDate,
                        Address = ps.Location?.Address ?? string.Empty,
                        CreationDateLocation = ps.Location?.EntryDate,

                        // Типы и статусы
                        CreationTypeId = ps.PointSaleEntity.CreationTypeId,
                        CreationTypeName = refData?.CreationTypeName ?? string.Empty,
                        OrganizationTypeId = ps.PointSaleEntity.OrganizationTypeId,
                        OrganizationTypeName = refData?.OrganizationTypeName ?? string.Empty,
                        ClosingStatusId = ps.PointSaleEntity.ClosingStatusId,
                        ClosingStatusName = refData?.ClosingStatusName ?? string.Empty,

                        // Метаданные
                        IsAproved = ps.PointSaleEntity.IsAproved,
                        Version = ps.PointSaleEntity.Version,
                        EntryDate = ps.PointSaleEntity.EntryDate,
                        ClosingDate = ps.PointSaleEntity.ClosingDate,
                        DescriptionText = ps.Description?.DescriptionText ?? "No description",

                        // Информация о руководителе
                        ChiefId = ps.Chief?.ChiefId ?? Guid.Empty,
                        OpenDateChief = ps.Chief?.OpenDate,
                        ChiefName = ps.Chief?.ChiefName ?? "Не указан",
                        ChiefPositionId = ps.Chief?.ChiefPositionId ?? 0,
                        ChiefPositionName = refData?.ChiefPositionName ?? "No position assigned",


                        Schedules = pointSaleSchedules
                         .Where(s => s.PointSaleId == ps.PointSaleEntity.PointSaleId)
                         .Select(s => new PointSaleScheduleDto
                             {
                                 PointSaleScheduleId = s.PointSaleScheduleId,
                                 PointSaleId = s.PointSaleId,
                                 DayOfWeek = s.DayOfWeek,
                                 IsWorkingDay = s.IsWorkingDay,
                                 StartTime = s.StartTime,
                                 EndTime = s.EndTime,
                                 BreakStartTime = s.BreakStartTime,
                                 BreakEndTime = s.BreakEndTime
                            }).ToList(),

                            };
                      }).ToList();

                response.Data = pointSaleResults;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error retrieving point sales for owner ID {ownerId}: {ex.Message}";
            }

            return response;
        }

        public async Task<OperationResult<bool>> UnlinkOperatorAsync(OperatorUnlinkDto unlinkOperatorDto)
        {
            OperationResult<bool> response = new();

            try
            {
                var pointSale = _pointSaleDbContext.PointSaleEntities
                    .Include(o => o.Operators)
                    .FirstOrDefault(o => o.PointSaleId == unlinkOperatorDto.PointSaleId);

                if (pointSale == null)
                {
                    response.ErrorMessage = $"Point sale with ID {unlinkOperatorDto.PointSaleId} not found.";
                    return response;
                }

                var operatorEntity = pointSale.Operators
                    .FirstOrDefault(o => o.OperatorId == unlinkOperatorDto.OperatorId);

                if (operatorEntity == null)
                {
                    response.ErrorMessage = $"Operator with ID {unlinkOperatorDto.OperatorId} not found in point sale {unlinkOperatorDto.PointSaleId}.";
                    return response;
                }

                pointSale.Operators.Remove(operatorEntity);

                await _pointSaleDbContext.SaveChangesAsync();

                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

        }

        public async Task<OperationResult<bool>> TransferOwnershipAsync(TransferOwnershipDto transferOwnershipDto)
        {
            OperationResult<bool> response = new();
            try
            {
                // Поиск сущности в базе данных
                var entity = await _pointSaleDbContext.PointSaleEntities
                    .FirstOrDefaultAsync(e => e.PointSaleId == transferOwnershipDto.PointSaleId);

                if (entity == null)
                {
                    response.ErrorMessage = $"Point sale with ID {transferOwnershipDto.PointSaleId} not found.";
                    return response;
                }

                // Проверка, что новый владелец отличается от текущего
                if (entity.OwnerId == transferOwnershipDto.NewOwnerId)
                {
                    response.ErrorMessage = "New owner is the same as the current owner.";
                    return response;
                }

                // Обновление владельца
                entity.OwnerId = transferOwnershipDto.NewOwnerId;
                entity.OwnerTypeId = transferOwnershipDto.OwnerTypeId;
                entity.OwnerName = transferOwnershipDto.OwnerName;


                // Сохранение изменений в базе данных
                await _pointSaleDbContext.SaveChangesAsync();

                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

        }


        /// <summary>
        /// Асинхронно получает количество точек продаж.
        /// </summary>
        public async Task<OperationResult<int>> GetCountPointSalesAsync()
        {
            OperationResult<int> response = new();
            try
            {
                var count = await _pointSaleDbContext.PointSaleEntities
                    .CountAsync();

                response.Data = count;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error retrieving point sales count: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<OperationResult<List<PointSaleCounts>>> GetCountsByOwnersIdAsync(List<Guid> ownerIds)
        {
            OperationResult<List<PointSaleCounts>> response = new();
            try
            {
                var pointSaleCounts = await _pointSaleDbContext.PointSaleEntities
              .Where(ps => ps.OwnerId.HasValue && ownerIds.Contains(ps.OwnerId.Value))
              .GroupBy(ps => ps.OwnerId)
              .Select(g => new PointSaleCounts
              {
                  OwnerId = g.Key!.Value,
                  PointSaleCount = g.Count()
              })
              .ToListAsync();

                // Добавляем ownerIds с нулевым количеством
                var result = ownerIds.Select(ownerId => pointSaleCounts
                    .FirstOrDefault(psc => psc.OwnerId == ownerId)
                    ?? new PointSaleCounts
                    {
                        OwnerId = ownerId,
                        PointSaleCount = 0
                    })
                    .ToList();
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Error retrieving point sale counts by owners: {ex.Message}";
                return response;
            }

            return response;
        }

        //public async Task<OperationResult<bool>> UpdatePointSaleAsync(PointSaleFullDto pointSaleFullDto)
        //{
        //    OperationResult<bool> response = new();

        //    // Используем транзакцию для атомарности операции
        //    using (var transaction = await _pointSaleDbContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            // Флаг для определения необходимости сброса IsApproved
        //            bool shouldResetApproval = false;

        //            // Находим существующую точку продаж
        //            var pointSale = await _pointSaleDbContext.PointSales
        //                .FirstOrDefaultAsync(ps => ps.PointSaleId == pointSaleFullDto.PointSale.PointSaleId);

        //            if (pointSale == null)
        //            {
        //                await transaction.RollbackAsync();
        //                response.ErrorMessage = "Точка продаж не найдена";
        //                return response;
        //            }

        //            // Проверяем критичные изменения для сброса IsApproved
        //            if (pointSale.CreationTypeId != pointSaleFullDto.PointSale.CreationTypeId ||
        //                pointSale.OrganizationTypeId != pointSaleFullDto.PointSale.OrganizationTypeId ||
        //                pointSale.OwnerId != pointSaleFullDto.PointSale.OwnerId ||
        //                pointSale.OwnerTypeId != pointSaleFullDto.PointSale.OwnerTypeId)
        //            {
        //                shouldResetApproval = true;
        //            }

        //            // Обновляем основную информацию точки продаж
        //            pointSale.OwnerId = pointSaleFullDto.PointSale.OwnerId;
        //            pointSale.OwnerTypeId = pointSaleFullDto.PointSale.OwnerTypeId;
        //            pointSale.OwnerName = pointSaleFullDto.PointSale.OwnerName;
        //            pointSale.CreationTypeId = pointSaleFullDto.PointSale.CreationTypeId;
        //            pointSale.OrganizationTypeId = pointSaleFullDto.PointSale.OrganizationTypeId;
        //            pointSale.ClosingStatusId = pointSaleFullDto.PointSale.ClosingStatusId;
        //            pointSale.ClosingDate = pointSaleFullDto.PointSale.ClosingDate;

        //            // Находим и обновляем активность точки продаж
        //            var pointSaleActivity = await _pointSaleDbContext.PointSaleActivities
        //                .FirstOrDefaultAsync(psa => psa.PointSaleActivityId == pointSaleFullDto.PointSaleActivity.PointSaleActivityId);

        //            if (pointSaleActivity == null)
        //            {
        //                await transaction.RollbackAsync();
        //                response.ErrorMessage = "Активность точки продаж не найдена";
        //                return response;
        //            }

        //            // Проверяем критичные изменения в активности
        //            if (pointSaleActivity.NamePointSale != pointSaleFullDto.PointSaleActivity.NamePointSale ||
        //                pointSaleActivity.OpenDate != pointSaleFullDto.PointSaleActivity.OpenDate)
        //            {
        //                shouldResetApproval = true;
        //            }

        //            pointSaleActivity.NamePointSale = pointSaleFullDto.PointSaleActivity.NamePointSale;
        //            pointSaleActivity.EnglishNamePointSale = pointSaleFullDto.PointSaleActivity.EnglishNamePointSale;
        //            pointSaleActivity.OpenDate = pointSaleFullDto.PointSaleActivity.OpenDate;

        //            // --- 3. Update/Delete Description ---
        //            var description = await _pointSaleDbContext.Descriptions
        //                .FirstOrDefaultAsync(d => d.PointSaleId == pointSaleFullDto.PointSale.PointSaleId);

        //            if (pointSaleFullDto.Description != null)
        //            {
        //                if (description != null)
        //                {
        //                    description.DescriptionText = pointSaleFullDto.Description.DescriptionText;
        //                    description.OpenDate = pointSaleFullDto.Description.OpenDate;
        //                }
        //                else
        //                {
        //                    var newDescription = new Description
        //                    {
        //                        DescriptionId = pointSaleFullDto.Description.DescriptionId == Guid.Empty ? Guid.NewGuid() : pointSaleFullDto.Description.DescriptionId,
        //                        DescriptionText = pointSaleFullDto.Description.DescriptionText,
        //                        OpenDate = pointSaleFullDto.Description.OpenDate,
        //                        PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                        EntryDate = DateTime.Now
        //                    };
        //                    await _pointSaleDbContext.Descriptions.AddAsync(newDescription);
        //                }
        //            }
        //            else // If description in DTO is null, but exists in DB, delete it
        //            {
        //                if (description != null)
        //                {
        //                    _pointSaleDbContext.Descriptions.Remove(description);
        //                }
        //            }

        //            // --- 4. Update/Delete Chief ---
        //            var chief = await _pointSaleDbContext.Chiefs
        //                .FirstOrDefaultAsync(c => c.PointSaleId == pointSaleFullDto.PointSale.PointSaleId);

        //            if (pointSaleFullDto.Chief != null && pointSaleFullDto.Chief.ChiefName != null && pointSaleFullDto.Chief.ChiefPositionId != null)
        //            {
        //                if (chief != null)
        //                {
        //                    if (chief.ChiefName != pointSaleFullDto.Chief.ChiefName ||
        //                        chief.ChiefPositionId != pointSaleFullDto.Chief.ChiefPositionId)
        //                    {
        //                        shouldResetApproval = true;
        //                    }

        //                    chief.ChiefName = pointSaleFullDto.Chief.ChiefName;
        //                    chief.ChiefPositionId = pointSaleFullDto.Chief.ChiefPositionId;
        //                    chief.OpenDate = pointSaleFullDto.Chief.OpenDate;
        //                }
        //                else
        //                {
        //                    var newChief = new Chief
        //                    {
        //                        ChiefId = pointSaleFullDto.Chief.ChiefId == Guid.Empty ? Guid.NewGuid() : pointSaleFullDto.Chief.ChiefId,
        //                        OpenDate = pointSaleFullDto.Chief.OpenDate,
        //                        ChiefName = pointSaleFullDto.Chief.ChiefName,
        //                        ChiefPositionId = pointSaleFullDto.Chief.ChiefPositionId,
        //                        PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                        EntryDate = DateTime.Now
        //                    };
        //                    await _pointSaleDbContext.Chiefs.AddAsync(newChief);
        //                }
        //            }
        //            else // If chief in DTO is null, but exists in DB, delete it
        //            {
        //                if (chief != null)
        //                {
        //                    _pointSaleDbContext.Chiefs.Remove(chief);
        //                }
        //            }

        //            // --- 5. Update/Delete Location ---
        //            var location = await _pointSaleDbContext.Locations
        //                .FirstOrDefaultAsync(l => l.PointSaleId == pointSaleFullDto.PointSale.PointSaleId);

        //            if (pointSaleFullDto.Location != null)
        //            {
        //                if (location != null)
        //                {
        //                    location.Address = pointSaleFullDto.Location.Address;
        //                    location.Latitude = pointSaleFullDto.Location.Latitude;
        //                    location.Longitude = pointSaleFullDto.Location.Longitude;
        //                    location.AddressId = pointSaleFullDto.Location.AddressId;
        //                    location.OpenDate = pointSaleFullDto.Location.OpenDate;
        //                }
        //                else
        //                {
        //                    var newLocation = new Location
        //                    {
        //                        LocationId = pointSaleFullDto.Location.LocationId == Guid.Empty ? Guid.NewGuid() : pointSaleFullDto.Location.LocationId,
        //                        OpenDate = pointSaleFullDto.Location.OpenDate,
        //                        Address = pointSaleFullDto.Location.Address,
        //                        Latitude = pointSaleFullDto.Location.Latitude,
        //                        Longitude = pointSaleFullDto.Location.Longitude,
        //                        AddressId = pointSaleFullDto.Location.AddressId,
        //                        PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                        EntryDate = DateTime.Now
        //                    };
        //                    await _pointSaleDbContext.Locations.AddAsync(newLocation);
        //                }
        //            }
        //            else // If location in DTO is null, but exists in DB, delete it
        //            {
        //                if (location != null)
        //                {
        //                    _pointSaleDbContext.Locations.Remove(location);
        //                }
        //            }

        //            // --- 6. Update PointSaleSchedules (Remove existing, Add new) ---
        //            if (pointSaleFullDto.PointSaleSchedules?.Any() == true)
        //            {
        //                var existingSchedules = await _pointSaleDbContext.PointSaleSchedules
        //                    .Where(s => s.PointSaleId == pointSaleFullDto.PointSale.PointSaleId)
        //                    .ToListAsync();

        //                _pointSaleDbContext.PointSaleSchedules.RemoveRange(existingSchedules);

        //                var newSchedules = pointSaleFullDto.PointSaleSchedules.Select(pss => new PointSaleSchedule
        //                {
        //                    PointSaleScheduleId = Guid.NewGuid(),
        //                    PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                    DayOfWeek = pss.DayOfWeek,
        //                    IsWorkingDay = pss.IsWorkingDay,
        //                    StartTime = pss.StartTime,
        //                    EndTime = pss.EndTime,
        //                    BreakStartTime = pss.BreakStartTime,
        //                    BreakEndTime = pss.BreakEndTime
        //                }).ToList();

        //                await _pointSaleDbContext.PointSaleSchedules.AddRangeAsync(newSchedules);
        //            }
        //            else
        //            {
        //                var existingSchedules = await _pointSaleDbContext.PointSaleSchedules
        //                    .Where(s => s.PointSaleId == pointSaleFullDto.PointSale.PointSaleId)
        //                    .ToListAsync();

        //                if (existingSchedules.Any())
        //                {
        //                    _pointSaleDbContext.PointSaleSchedules.RemoveRange(existingSchedules);
        //                }
        //            }

        //            // --- 7. Update CategoryIds (Sync) ---
        //            var pointSaleToUpdateForCategories = await _pointSaleDbContext.PointSales
        //                .Include(ps => ps.CategoryPointSales) // Eagerly load the related categories
        //                .FirstOrDefaultAsync(ps => ps.PointSaleId == pointSaleFullDto.PointSale.PointSaleId);

        //            if (pointSaleToUpdateForCategories == null)
        //            {
        //                await transaction.RollbackAsync();
        //                response.ErrorMessage = "Точка продаж не найдена для обновления категорий.";
        //                return response;
        //            }

        //            // Get current CategoryIds linked to the point sale
        //            var currentCategoryIds = pointSaleToUpdateForCategories.CategoryPointSales.Select(cps => cps.CategoryId).ToList();

        //            // Find categories to add
        //            var categoriesToAddIds = pointSaleFullDto.CategoryIds.Except(currentCategoryIds).ToList();
        //            // Find categories to remove
        //            var categoriesToRemoveIds = currentCategoryIds.Except(pointSaleFullDto.CategoryIds).ToList();

        //            // Remove categories
        //            foreach (var categoryIdToRemove in categoriesToRemoveIds)
        //            {
        //                var categoryToRemove = pointSaleToUpdateForCategories.CategoryPointSales.FirstOrDefault(cps => cps.CategoryId == categoryIdToRemove);
        //                if (categoryToRemove != null)
        //                {
        //                    pointSaleToUpdateForCategories.CategoryPointSales.Remove(categoryToRemove);
        //                }
        //            }

        //            // Add new categories
        //            if (categoriesToAddIds.Any())
        //            {
        //                var categoriesToAdd = await _pointSaleDbContext.CategoryPointSales // Assuming CategoryPointSales is your DbSet for categories
        //                    .Where(cps => categoriesToAddIds.Contains(cps.CategoryId))
        //                    .ToListAsync();

        //                foreach (var category in categoriesToAdd)
        //                {
        //                    pointSaleToUpdateForCategories.CategoryPointSales.Add(category);
        //                }
        //            }

        //            // --- 8. Update AlternativeName ---
        //            var existingAlternativeNames = await _pointSaleDbContext.AlternativeWords
        //                .Where(aw => aw.PointSaleId == pointSaleFullDto.PointSale.PointSaleId)
        //                .ToListAsync();

        //            var currentNames = existingAlternativeNames.Select(x => x.AlternativeWordName).ToList();
        //            var namesToAdd = pointSaleFullDto.AlternativeName.Except(currentNames).ToList();
        //            var namesToRemove = currentNames.Except(pointSaleFullDto.AlternativeName).ToList();

        //            if (namesToRemove.Any())
        //            {
        //                var altNamesToRemove = existingAlternativeNames
        //                    .Where(aw => namesToRemove.Contains(aw.AlternativeWordName))
        //                    .ToList();
        //                _pointSaleDbContext.AlternativeWords.RemoveRange(altNamesToRemove);
        //            }

        //            if (namesToAdd.Any())
        //            {
        //                var newAltNames = namesToAdd.Select(name => new AlternativeWord
        //                {
        //                    AlternativeWordId = Guid.NewGuid(),
        //                    PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                    AlternativeWordName = name
        //                }).ToList();
        //                await _pointSaleDbContext.AlternativeWords.AddRangeAsync(newAltNames);
        //            }

        //            // Сбрасываем IsApproved если были критичные изменения
        //            if (shouldResetApproval)
        //            {
        //                pointSale.IsAproved = false;
        //            }

        //            // Обновляем основные сущности
        //            _pointSaleDbContext.PointSales.Update(pointSale);
        //            _pointSaleDbContext.PointSaleActivities.Update(pointSaleActivity);

        //            // Сохраняем изменения
        //            await _pointSaleDbContext.SaveChangesAsync();

        //            // Фиксируем транзакцию
        //            await transaction.CommitAsync();

        //            response.Data = true;

        //            return response;
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            await transaction.RollbackAsync();
        //            response.ErrorMessage = $"Ошибка обновления: {ex.InnerException?.Message ?? ex.Message}";
        //            return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            response.ErrorMessage = $"Неожиданная ошибка: {ex.Message}";
        //            return response;
        //        }
        //    }
        //}

        //public async Task<OperationResult<bool>> ChangePointSaleAsync(PointSaleFullDto pointSaleFullDto)
        //{
        //    OperationResult<bool> response = new();

        //    // Using a transaction for atomicity
        //    using (var transaction = await _pointSaleDbContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            // Flag to determine if IsApproved needs to be reset
        //            bool shouldResetApproval = false;

        //            // --- 1. Handle PointSale Entity Update (ALWAYS IN-PLACE) ---
        //            // The main PointSale entity itself is always updated in place.
        //            // Creating a new PointSaleId would mean creating a new point of sale.
        //            var pointSale = await _pointSaleDbContext.PointSales
        //                .FirstOrDefaultAsync(ps => ps.PointSaleId == pointSaleFullDto.PointSale.PointSaleId);

        //            if (pointSale == null)
        //            {
        //                await transaction.RollbackAsync();
        //                response.ErrorMessage = "Торговая точка не найдена.";
        //                return response;
        //            }

        //            // Check for critical changes to reset IsApproved for PointSale
        //            if (pointSale.CreationTypeId != pointSaleFullDto.PointSale.CreationTypeId ||
        //                pointSale.OrganizationTypeId != pointSaleFullDto.PointSale.OrganizationTypeId ||
        //                pointSale.OwnerId != pointSaleFullDto.PointSale.OwnerId ||
        //                pointSale.OwnerTypeId != pointSaleFullDto.PointSale.OwnerTypeId)
        //            {
        //                shouldResetApproval = true;
        //            }

        //            // Update the main point sale information in place
        //            pointSale.OwnerId = pointSaleFullDto.PointSale.OwnerId;
        //            pointSale.OwnerTypeId = pointSaleFullDto.PointSale.OwnerTypeId;
        //            pointSale.OwnerName = pointSaleFullDto.PointSale.OwnerName;
        //            pointSale.CreationTypeId = pointSaleFullDto.PointSale.CreationTypeId;
        //            pointSale.OrganizationTypeId = pointSaleFullDto.PointSale.OrganizationTypeId;
        //            pointSale.ClosingStatusId = pointSaleFullDto.PointSale.ClosingStatusId;
        //            pointSale.ClosingDate = pointSaleFullDto.PointSale.ClosingDate;
        //            pointSale.EntryDate = DateTime.Now; // Update EntryDate for the main point sale itself

        //            // --- 2. Create NEW PointSale Activity Record if DTO has data (NEVER DELETE OLD) ---
        //            if (pointSaleFullDto.PointSaleActivity != null)
        //            {
        //                var newPointSaleActivity = new PointSaleActivity
        //                {
        //                    PointSaleActivityId = Guid.NewGuid(), // Always create a new ID
        //                    PointSaleId = pointSaleFullDto.PointSale.PointSaleId, // Link to the main point sale
        //                    NamePointSale = pointSaleFullDto.PointSaleActivity.NamePointSale,
        //                    EnglishNamePointSale = pointSaleFullDto.PointSaleActivity.EnglishNamePointSale,
        //                    OpenDate = pointSaleFullDto.PointSaleActivity.OpenDate,
        //                    EntryDate = DateTime.Now // Set current timestamp
        //                };
        //                await _pointSaleDbContext.PointSaleActivities.AddAsync(newPointSaleActivity);
        //                shouldResetApproval = true; // Adding a new activity is a change
        //            }

        //            // --- 3. Create New Description Record if DTO has data (NEVER DELETE OLD) ---
        //            if (pointSaleFullDto.Description != null)
        //            {
        //                var newDescription = new DataBase.DBModels.Description
        //                {
        //                    DescriptionId = Guid.NewGuid(), // Always create a new ID
        //                    DescriptionText = pointSaleFullDto.Description.DescriptionText,
        //                    OpenDate = pointSaleFullDto.Description.OpenDate,
        //                    PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                    EntryDate = DateTime.Now // Set current timestamp
        //                };
        //                await _pointSaleDbContext.Descriptions.AddAsync(newDescription);
        //                shouldResetApproval = true; // Adding a new description is a change
        //            }

        //            // --- 4. Create New Chief Record if DTO has data (NEVER DELETE OLD) ---
        //            if (pointSaleFullDto.Chief != null && pointSaleFullDto.Chief.ChiefName != null)
        //            {
        //                var newChief = new Chief
        //                {
        //                    ChiefId = Guid.NewGuid(), // Always create a new ID
        //                    OpenDate = pointSaleFullDto.Chief.OpenDate,
        //                    ChiefName = pointSaleFullDto.Chief.ChiefName,
        //                    ChiefPositionId = pointSaleFullDto.Chief.ChiefPositionId,
        //                    PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                    EntryDate = DateTime.Now // Set current timestamp
        //                };
        //                await _pointSaleDbContext.Chiefs.AddAsync(newChief);
        //                shouldResetApproval = true; // Adding a new chief is a change
        //            }

        //            // --- 5. Create New Location Record if DTO has data (NEVER DELETE OLD) ---
        //            if (pointSaleFullDto.Location != null)
        //            {
        //                var newLocation = new Location
        //                {
        //                    LocationId = Guid.NewGuid(), // Always create a new ID
        //                    OpenDate = pointSaleFullDto.Location.OpenDate,
        //                    Address = pointSaleFullDto.Location.Address,
        //                    Latitude = pointSaleFullDto.Location.Latitude,
        //                    Longitude = pointSaleFullDto.Location.Longitude,
        //                    AddressId = pointSaleFullDto.Location.AddressId,
        //                    PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                    EntryDate = DateTime.Now // Set current timestamp
        //                };
        //                await _pointSaleDbContext.Locations.AddAsync(newLocation);
        //                shouldResetApproval = true; // Adding a new location is a change
        //            }

        //            // --- 6. Update PointSaleSchedule (REPLACE CURRENT SET) ---
        //            // For collections like PointSaleSchedule, "new record" implies replacing the entire current schedule set.
        //            var existingSchedules = await _pointSaleDbContext.PointSaleSchedules
        //                .Where(s => s.PointSaleId == pointSaleFullDto.PointSale.PointSaleId)
        //                .ToListAsync();

        //            if (existingSchedules.Any())
        //            {
        //                _pointSaleDbContext.PointSaleSchedules.RemoveRange(existingSchedules);
        //                shouldResetApproval = true;
        //            }

        //            if (pointSaleFullDto.PointSaleSchedules?.Any() == true)
        //            {
        //                var newSchedules = pointSaleFullDto.PointSaleSchedules.Select(ps => new PointSaleSchedule
        //                {
        //                    PointSaleScheduleId = Guid.NewGuid(),
        //                    PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                    DayOfWeek = ps.DayOfWeek,
        //                    IsWorkingDay = ps.IsWorkingDay,
        //                    StartTime = ps.StartTime,
        //                    EndTime = ps.EndTime,
        //                    BreakStartTime = ps.BreakStartTime,
        //                    BreakEndTime = ps.BreakEndTime
        //                }).ToList();

        //                await _pointSaleDbContext.PointSaleSchedules.AddRangeAsync(newSchedules);
        //                shouldResetApproval = true;
        //            }

        //            // --- 7. Update CategoryIds (REPLACE CURRENT SET of relationships) ---
        //            // For many-to-many relationships, "new record" implies updating the current set.
        //            var pointSaleToUpdateForCategories = await _pointSaleDbContext.PointSales
        //               .Include(ps => ps.CategoryPointSales)
        //               .FirstOrDefaultAsync(ps => ps.PointSaleId == pointSaleFullDto.PointSale.PointSaleId);

        //            if (pointSaleToUpdateForCategories == null)
        //            {
        //                await transaction.RollbackAsync();
        //                response.ErrorMessage = "Торговая точка не найдена для обновления категорий.";
        //                return response;
        //            }

        //            var currentCategoryIds = pointSaleToUpdateForCategories.CategoryPointSales.Select(c => c.CategoryId).ToList();
        //            var categoriesToAddIds = pointSaleFullDto.CategoryIds.Except(currentCategoryIds).ToList();
        //            var categoriesToRemoveIds = currentCategoryIds.Except(pointSaleFullDto.CategoryIds).ToList();

        //            if (categoriesToRemoveIds.Any() || categoriesToAddIds.Any())
        //            {
        //                shouldResetApproval = true;
        //            }

        //            // Remove old relationships
        //            foreach (var categoryIdToRemove in categoriesToRemoveIds)
        //            {
        //                var categoryToRemove = pointSaleToUpdateForCategories.CategoryPointSales.FirstOrDefault(c => c.CategoryId == categoryIdToRemove);
        //                if (categoryToRemove != null)
        //                {
        //                    pointSaleToUpdateForCategories.CategoryPointSales.Remove(categoryToRemove);
        //                }
        //            }

        //            // Add new relationships
        //            if (categoriesToAddIds.Any())
        //            {
        //                var categoriesToAdd = await _pointSaleDbContext.CategoryPointSales
        //                    .Where(c => categoriesToAddIds.Contains(c.CategoryId))
        //                    .ToListAsync();

        //                foreach (var category in categoriesToAdd)
        //                {
        //                    pointSaleToUpdateForCategories.CategoryPointSales.Add(category);
        //                }
        //            }

        //            // --- 8. Update AlternativeName (ADD NEW RECORDS, DO NOT REPLACE) ---
        //            if (pointSaleFullDto.AlternativeName?.Any() == true)
        //            {
        //                // Get existing alternative names for this point sale
        //                var existingAlternativeWords = await _pointSaleDbContext.AlternativeWords
        //                    .Where(aw => aw.PointSaleId == pointSaleFullDto.PointSale.PointSaleId)
        //                    .Select(aw => aw.AlternativeWordName)
        //                    .ToListAsync();

        //                // Find new alternative names that are not already present
        //                var newAlternativeNamesToCreate = pointSaleFullDto.AlternativeName
        //                    .Where(name => !existingAlternativeWords.Contains(name))
        //                    .ToList();

        //                if (newAlternativeNamesToCreate.Any())
        //                {
        //                    var alternativeWordsToAdd = newAlternativeNamesToCreate.Select(name => new AlternativeWord
        //                    {
        //                        AlternativeWordId = Guid.NewGuid(),
        //                        PointSaleId = pointSaleFullDto.PointSale.PointSaleId,
        //                        AlternativeWordName = name
        //                    }).ToList();

        //                    await _pointSaleDbContext.AlternativeWords.AddRangeAsync(alternativeWordsToAdd);
        //                    shouldResetApproval = true; // Adding new alternative names is a change
        //                }
        //            }

        //            // Reset IsApproved if any critical changes were detected
        //            if (shouldResetApproval)
        //            {
        //                pointSale.IsAproved = false;
        //            }

        //            // Save all changes (updates and additions)
        //            await _pointSaleDbContext.SaveChangesAsync();

        //            // Commit the transaction
        //            await transaction.CommitAsync();

        //            response.Data = true;

        //            return response;
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            await transaction.RollbackAsync();
        //            response.ErrorMessage = $"Ошибка обновления базы данных: {ex.InnerException?.Message ?? ex.Message}";
        //            return response;
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            response.ErrorMessage = $"Неожиданная ошибка: {ex.Message}";
        //            return response;
        //        }
        //    }
        //}

        public async Task<OperationResult<bool>> UpdatePointSaleNameAsync(PointSaleNameUpdateDto pointSaleNameUpdateDto)
        { 
            OperationResult<bool> response = new();
            try
            {
                var pointSaleActivityToUpdate = await _pointSaleDbContext.PointSaleActivities
            .FirstOrDefaultAsync(p => p.PointSaleId == pointSaleNameUpdateDto.PointSaleId &&
                                    p.OpenDate == pointSaleNameUpdateDto.OpenDatePointSale);


                if (pointSaleActivityToUpdate != null)
                {
                    pointSaleActivityToUpdate.NamePointSale = pointSaleNameUpdateDto.NamePointSale;
                    pointSaleActivityToUpdate.EnglishNamePointSale = pointSaleNameUpdateDto.EnglishNamePointSale;
                    _pointSaleDbContext.PointSaleActivities.Update(pointSaleActivityToUpdate);
                }

                else
                {
                    PointSaleActivity? previousActivity = null;

                    // Ищем предыдущую запись только если новая дата (OpenDatePointSale) указана.
                        previousActivity = await _pointSaleDbContext.PointSaleActivities
                            .Where(p => p.PointSaleId == pointSaleNameUpdateDto.PointSaleId &&
                                        p.OpenDate < pointSaleNameUpdateDto.OpenDatePointSale)
                            .OrderByDescending(p => p.OpenDate)
                            .FirstOrDefaultAsync();
                    

                    // Создаем новую запись активности торговой точки.
                    var newPointSaleActivity = new PointSaleActivity
                    {
                        PointSaleId = pointSaleNameUpdateDto.PointSaleId,
                        NamePointSale = pointSaleNameUpdateDto.NamePointSale,
                        OpenDate = pointSaleNameUpdateDto.OpenDatePointSale,
                        EntryDate = DateTime.UtcNow, // Дата и время добавления записи в БД.
                        EnglishNamePointSale = pointSaleNameUpdateDto.EnglishNamePointSale
                    };

                    await _pointSaleDbContext.PointSaleActivities.AddAsync(newPointSaleActivity);
                }

                // Сохраняем все изменения (обновление или добавление) в базе данных.
                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Произошла ошибка при обновлении или создании названия торговой точки: " + ex.Message;
                response.Data = false;
            }

            return response;
        }

        public async Task<OperationResult<bool>> UpdateCreationDateAsync(CreationDateUpdateDto creationDateUpdate)
        {
            OperationResult<bool> response = new();
            using var transaction = await _pointSaleDbContext.Database.BeginTransactionAsync();
            try
            {
                var pointSaleoUpdate = await _pointSaleDbContext.PointSaleEntities
                    .Include(o => o.PointSaleActivities)
                    .Include(o => o.Chiefs)
                    .Include(o => o.Locations)
                    .Include(o => o.Descriptions)
                    .Include(o => o.Logos)
                    .FirstOrDefaultAsync(p => p.PointSaleId == creationDateUpdate.PointSaleId);


                if (pointSaleoUpdate == null)
                {
                    response.ErrorMessage = "Организация не найдена";
                    return response;
                }

                pointSaleoUpdate.CreationDate = creationDateUpdate.CreationDatePointSale;


                // Проверяем и обновляем OrganizationActivities
                if (pointSaleoUpdate.PointSaleActivities != null && pointSaleoUpdate.PointSaleActivities.Any())
                {
                    // Находим запись
                    var oldestActivity = pointSaleoUpdate.PointSaleActivities
                        .OrderBy(a => a.OpenDate)
                        .FirstOrDefault();

                    if (oldestActivity != null)
                    {
                        // Создаем список для удаления
                        var activitiesToRemove = new List<PointSaleActivity>();

                        // Если самая первая запись не равна новой дате, обновляем её
                        if (oldestActivity.OpenDate != pointSaleoUpdate.CreationDate)
                        {
                            oldestActivity.OpenDate = pointSaleoUpdate.CreationDate.Value;
                        }

                        // Собираем все остальные записи, которые нужно удалить
                        var otherActivities = pointSaleoUpdate.PointSaleActivities
                            .Where(a => a.PointSaleActivityId != oldestActivity.PointSaleActivityId && a.OpenDate < creationDateUpdate.CreationDatePointSale)
                            .ToList();

                        // Удаляем "висящие" записи
                        if (otherActivities.Any())
                        {
                            _pointSaleDbContext.PointSaleActivities.RemoveRange(otherActivities);
                        }
                    }
                }



                // Проверяем и обновляем Chiefs
                if (pointSaleoUpdate.Chiefs != null && pointSaleoUpdate.Chiefs.Any())
                {
                    // Находим самую старую запись
                    var oldestChief = pointSaleoUpdate.Chiefs
                        .OrderBy(c => c.OpenDate)
                        .FirstOrDefault();

                    if (oldestChief != null)
                    {
                        // Создаем список для удаления
                        var chiefsToRemove = new List<Chief>();

                        // Если самая старая запись не равна новой дате, обновляем её
                        if (oldestChief.OpenDate != creationDateUpdate.CreationDatePointSale)
                        {
                            oldestChief.OpenDate = creationDateUpdate.CreationDatePointSale;
                        }

                        // Собираем все остальные записи, которые нужно удалить
                        var otherChiefs = pointSaleoUpdate.Chiefs
                            .Where(c => c.ChiefId != oldestChief.ChiefId && c.OpenDate < creationDateUpdate.CreationDatePointSale)
                            .ToList();

                        // Удаляем "висящие" записи
                        if (otherChiefs.Any())
                        {
                            _pointSaleDbContext.Chiefs.RemoveRange(otherChiefs);
                        }
                    }
                }

                // Проверяем и обновляем OrganizationDescriptions
                if (pointSaleoUpdate.Descriptions != null && pointSaleoUpdate.Descriptions.Any())
                {
                    // Находим самую старую запись с описанием
                    var oldestDescription = pointSaleoUpdate.Descriptions
                        .OrderBy(d => d.OpenDate)
                        .FirstOrDefault();

                    if (oldestDescription != null)
                    {
                        // Создаем список для удаления
                        var descriptionsToRemove = new List<Description>();

                        // Обновляем дату у самой старой записи
                        if (oldestDescription.OpenDate != creationDateUpdate.CreationDatePointSale)
                        {
                            oldestDescription.OpenDate = creationDateUpdate.CreationDatePointSale;
                        }

                        // Собираем все остальные записи, которые нужно удалить
                        var otherDescriptions = pointSaleoUpdate.Descriptions
                            .Where(d => d.DescriptionId != oldestDescription.DescriptionId && d.OpenDate < creationDateUpdate.CreationDatePointSale)
                            .ToList();

                        // Удаляем "висящие" записи
                        if (otherDescriptions.Any())
                        {
                            _pointSaleDbContext.Descriptions.RemoveRange(otherDescriptions);
                        }
                    }
                }

                _pointSaleDbContext.PointSaleEntities.Update(pointSaleoUpdate);
                await _pointSaleDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.ErrorMessage = "Произошла ошибка при обновлении даты открытия торговой точки: " + ex.Message;
                response.Data = false;
            }
            return response;
        }

        public async Task<OperationResult<bool>> UpdateWorkScheduleAsync(WorkScheduleUpdateDto workScheduleUpdateDto)
        {
            OperationResult<bool> response = new();
            try
            {
                // 1. Проверяем существование торговой точки.
                var pointSaleExists = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == workScheduleUpdateDto.PointSaleId);

                if (!pointSaleExists)
                {
                    response.ErrorMessage = "Торговая точка с указанным PointSaleId не найдена.";
                    return response;
                }

                // 2. Загружаем текущее расписание для данной торговой точки из базы данных.
                var pointSaleToUpdate = await _pointSaleDbContext.PointSaleEntities
                    .Include(p => p.PointSaleSchedules) // Явно загружаем расписание
                    .FirstOrDefaultAsync(p => p.PointSaleId == workScheduleUpdateDto.PointSaleId);

                if (pointSaleToUpdate == null)
                {
                    response.ErrorMessage = "Не удалось загрузить данные торговой точки для обновления расписания.";
                    return response;
                }

                // 3. Удаляем все существующие записи расписания для торговой точки.
                if (pointSaleToUpdate.PointSaleSchedules.Any())
                {
                    _pointSaleDbContext.PointSaleSchedules.RemoveRange(pointSaleToUpdate.PointSaleSchedules);
                }

                // 4. Добавляем новые записи расписания из DTO.
                if (workScheduleUpdateDto.WorkSchedule.Any())
                {
                    var newScheduleEntities = workScheduleUpdateDto.WorkSchedule.Select(dto => new PointSaleSchedule
                    {
                        PointSaleScheduleId = Guid.NewGuid(), // Новый ID для каждой записи
                        PointSaleId = workScheduleUpdateDto.PointSaleId,
                        DayOfWeek = dto.DayOfWeek,
                        IsWorkingDay = dto.IsWorkingDay,
                        StartTime = dto.StartTime,
                        EndTime = dto.EndTime,
                        BreakStartTime = dto.BreakStartTime,
                        BreakEndTime = dto.BreakEndTime
                    }).ToList();

                    await _pointSaleDbContext.PointSaleSchedules.AddRangeAsync(newScheduleEntities);
                }

                // 5. Сохраняем все изменения в базе данных.
                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = true; // Указываем успешное выполнение

            }
            catch (Exception ex)
            {

                response.ErrorMessage = "Произошла ошибка при обновлении расписания работы торговой точки: " + ex.Message;
                response.Data = false;
            }
            return response;
        }

        public async Task<OperationResult<bool>> UpdateCategoriesAsync(CategoriesUpdateDto categoriesUpdateDto)
        {
            OperationResult<bool> response = new();
            try
            {
                // 1. Проверяем существование торговой точки.
                var pointSaleExists = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == categoriesUpdateDto.PointSaleId);

                if (!pointSaleExists)
                {
                    response.ErrorMessage = "Торговая точка с указанным PointSaleId не найдена.";
                    return response;
                }

                // 2. Загружаем торговую точку с ее текущими категориями.
                var pointSaleToUpdate = await _pointSaleDbContext.PointSaleEntities
                    .Include(p => p.CategoryPointSales) // Явно загружаем коллекцию связанных категорий
                    .FirstOrDefaultAsync(p => p.PointSaleId == categoriesUpdateDto.PointSaleId);

                if (pointSaleToUpdate == null)
                {
                    response.ErrorMessage = "Не удалось загрузить данные торговой точки для обновления категорий.";
                    return response;
                }

                // 3. Определяем текущие CategoryId, связанные с торговой точкой.
                var currentCategoryIdsInDb = pointSaleToUpdate.CategoryPointSales.Select(c => c.CategoryId).ToHashSet();

                // 4. Определяем, какие CategoryId нужно добавить и какие удалить.
                var categoryIdsToAdd = categoriesUpdateDto.CategoryIds.Except(currentCategoryIdsInDb).ToList();
                var categoryIdsToRemove = currentCategoryIdsInDb.Except(categoriesUpdateDto.CategoryIds).ToList();

                // 5. Удаляем ненужные связи.
                if (categoryIdsToRemove.Any())
                {
                    var categoriesToRemove = pointSaleToUpdate.CategoryPointSales
                        .Where(c => categoryIdsToRemove.Contains(c.CategoryId))
                        .ToList();

                    foreach (var category in categoriesToRemove)
                    {
                        pointSaleToUpdate.CategoryPointSales.Remove(category);
                    }
                }

                // 6. Добавляем новые связи.
                if (categoryIdsToAdd.Any())
                {
                    var categoriesToLink = await _pointSaleDbContext.CategoryPointSales
                        .Where(c => categoryIdsToAdd.Contains(c.CategoryId))
                        .ToListAsync();

                    if (categoriesToLink.Count != categoryIdsToAdd.Count)
                    {
                        response.ErrorMessage = "Один или несколько указанных CategoryId не существуют.";
                        return response;
                    }

                    foreach (var category in categoriesToLink)
                    {
                        pointSaleToUpdate.CategoryPointSales.Add(category);
                    }
                }

                // 7. Сохраняем все изменения в базе данных.
                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Произошла ошибка при обновлении категорий торговой точки: " + ex.Message;
                response.Data = false;
            }
            return response;
        }

        public async Task<OperationResult<bool>> DeleteOwnerAsync(DeleteOwnerDto deleteOwnerDto)
        {
            OperationResult<bool> response = new();
            using var transaction = await _pointSaleDbContext.Database.BeginTransactionAsync();
            try
            {
                var pointSales = await _pointSaleDbContext.PointSaleEntities
                 .Where(ps => ps.OwnerId == deleteOwnerDto.OwnerId)
                 .Include(ps => ps.PointSaleSchedules)
                 .Include(ps => ps.Photos)
                 .Include(ps => ps.Logos)
                 .Include(ps => ps.AlternativeWords)
                 .Include(ps => ps.Descriptions)
                 .Include(ps => ps.PointSaleActivities)
                 .Include(ps => ps.Chiefs)
                 .Include(ps => ps.Locations)
                 .Include(ps => ps.CategoryPointSales)
                 .ToListAsync();

                if (!pointSales.Any())
                {
                    response.ErrorMessage = "No point of sale found for the specified owner.";
                    return response;
                }

                foreach (var pointSale in pointSales)
                {
                    // Удаляем все связанные сущности
                    _pointSaleDbContext.PointSaleSchedules.RemoveRange(pointSale.PointSaleSchedules);
                    _pointSaleDbContext.Photos.RemoveRange(pointSale.Photos);
                    _pointSaleDbContext.Logos.RemoveRange(pointSale.Logos);
                    _pointSaleDbContext.AlternativeWords.RemoveRange(pointSale.AlternativeWords);
                    _pointSaleDbContext.Descriptions.RemoveRange(pointSale.Descriptions);
                    _pointSaleDbContext.PointSaleActivities.RemoveRange(pointSale.PointSaleActivities);
                    _pointSaleDbContext.Chiefs.RemoveRange(pointSale.Chiefs);
                    _pointSaleDbContext.Locations.RemoveRange(pointSale.Locations);
                    _pointSaleDbContext.CategoryPointSales.RemoveRange(pointSale.CategoryPointSales);
                    // Удаляем саму точку продаж
                    _pointSaleDbContext.PointSaleEntities.Remove(pointSale);
                }

                await _pointSaleDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Data = true;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.ErrorMessage = "An error occurred while deleting the owner: " + ex.Message;
                response.Data = false;
            }
            return response;
        }

        public async Task<OperationResult<List<Guid>>> SubmitVerificationAsync(Guid pointSaleId)
        {
            OperationResult<List<Guid>> response = new();
            try
            {
                var pointSale = await _pointSaleDbContext.PointSaleEntities
                    .FirstOrDefaultAsync(ps => ps.PointSaleId == pointSaleId);
                if (pointSale == null)
                {
                    response.ErrorMessage = "Point sale not found for the specified owner.";
                    return response;
                }

                var alreadySubmitted = await _pointSaleDbContext.PendingVerifications
                    .AnyAsync(pv => pv.PointSaleId == pointSaleId);

                if (alreadySubmitted)
                {
                    response.ErrorMessage = "Заявка уже была отправлена";
                    return response;
                }

                await _pointSaleDbContext.PendingVerifications.AddAsync(new PendingVerification
                {
                    PointSaleId = pointSaleId
                });

                await _pointSaleDbContext.SaveChangesAsync();

                // Получаем список всех админов (пример)
                var adminIds = await _pointSaleDbContext.Admins
                    .Select(a => a.AdminId) // подставь нужное поле
                    .ToListAsync();


                response.Data = adminIds;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Внутренняя ошибка";
                return response;
            }
        }

        public async Task<OperationResult<bool>> ConfirmIsApprovedAsync(PoinrSaleIsApprovedDto poinrSaleIsApprovedDto)
        {
            OperationResult<bool> response = new();
            using var transaction = await _pointSaleDbContext.Database.BeginTransactionAsync();
            try
            {
                var pointSale = await _pointSaleDbContext.PointSaleEntities
                    .FirstOrDefaultAsync(ps => ps.PointSaleId == poinrSaleIsApprovedDto.PointSaleId);

                if (pointSale == null)
                {
                    response.ErrorMessage = "Point sale not found.";
                    return response;
                }

                pointSale.IsAproved = true;
                _pointSaleDbContext.PointSaleEntities.Update(pointSale);

                var pendingVerification = await _pointSaleDbContext.PendingVerifications
                  .FirstOrDefaultAsync(p => p.PointSaleId == poinrSaleIsApprovedDto.PointSaleId);

                if (pendingVerification != null)
                {
                    _pointSaleDbContext.PendingVerifications.Remove(pendingVerification);
                }

                await _pointSaleDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                response.Data = true;
                return response;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.ErrorMessage = "Произошла ошибка при подтверждении магазина: " + ex.Message;
                return response;
            }
        }

        public async Task<OperationResult<bool>> DeletePointSaleAsync(Guid pointSaleId)
        {
            OperationResult<bool> response = new();
            using var transaction = await _pointSaleDbContext.Database.BeginTransactionAsync();
            try
            {
                var pointSales = await _pointSaleDbContext.PointSaleEntities
                 .Where(ps => ps.PointSaleId == pointSaleId)
                 .Include(ps => ps.PointSaleSchedules)
                 .Include(ps => ps.Photos)
                 .Include(ps => ps.Logos)
                 .Include(ps => ps.AlternativeWords)
                 .Include(ps => ps.Descriptions)
                 .Include(ps => ps.PointSaleActivities)
                 .Include(ps => ps.Chiefs)
                 .Include(ps => ps.Locations)
                 .Include(ps => ps.CategoryPointSales)
                 .ToListAsync();

                if (!pointSales.Any())
                {
                    response.ErrorMessage = "No point of sale found for the specified owner.";
                    return response;
                }

                foreach (var pointSale in pointSales)
                {
                    // Удаляем все связанные сущности
                    _pointSaleDbContext.PointSaleSchedules.RemoveRange(pointSale.PointSaleSchedules);
                    _pointSaleDbContext.Photos.RemoveRange(pointSale.Photos);
                    _pointSaleDbContext.Logos.RemoveRange(pointSale.Logos);
                    _pointSaleDbContext.AlternativeWords.RemoveRange(pointSale.AlternativeWords);
                    _pointSaleDbContext.Descriptions.RemoveRange(pointSale.Descriptions);
                    _pointSaleDbContext.PointSaleActivities.RemoveRange(pointSale.PointSaleActivities);
                    _pointSaleDbContext.Chiefs.RemoveRange(pointSale.Chiefs);
                    _pointSaleDbContext.Locations.RemoveRange(pointSale.Locations);
                    _pointSaleDbContext.CategoryPointSales.RemoveRange(pointSale.CategoryPointSales);
                    // Удаляем саму точку продаж
                    _pointSaleDbContext.PointSaleEntities.Remove(pointSale);
                }

                await _pointSaleDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Data = true;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.ErrorMessage = "An error occurred while deleting the owner: " + ex.Message;
                response.Data = false;
            }
            return response;
        }

        public async Task<OperationResult<bool>> ClosePointSaleAsync(PointSaleCloseDto pointSaleCloseDto)
        {
            OperationResult<bool> response = new();
            try
            {
                var pointSale = await _pointSaleDbContext.PointSaleEntities
                    .Include(ps => ps.PointSaleActivities)
                    .Include(ps => ps.Operators)
                    .Include(ps => ps.Chiefs)
                    .Include(ps => ps.Locations)
                    .Include(ps => ps.Descriptions)
                    .Include(ps => ps.Logos)
                    .FirstOrDefaultAsync(ps => ps.PointSaleId == pointSaleCloseDto.PointSaleId);

                if (pointSale == null)
                {
                    response.ErrorMessage = "Магазин не найден";
                    return response;
                }

                if (!pointSale.CreationDate.HasValue)
                {
                    response.ErrorMessage = "Дата создания магазина не указана";
                    return response;
                }

                if (pointSaleCloseDto.CloseDate < pointSale.CreationDate)
                {
                    response.ErrorMessage = "Дата закрытия не может быть раньше даты создания магазина";
                    return response;
                }

                // Проверяем, что дата закрытия не раньше самой новой даты назначения руководителя
                if (pointSale.Chiefs != null && pointSale.Chiefs.Any())
                {
                    var latestChiefOpenDate = pointSale.Chiefs.Max(c => c.OpenDate);

                    if (pointSaleCloseDto.CloseDate < latestChiefOpenDate)
                    {
                        response.ErrorMessage = $"Дата закрытия не может быть раньше даты назначения последнего руководителя ({latestChiefOpenDate:dd.MM.yyyy})";
                        return response;
                    }
                }

                // Проверяем даты в активностях организации
                if (pointSale.PointSaleActivities != null && pointSale.PointSaleActivities.Any())
                {
                    var latestActivityDate = pointSale.PointSaleActivities.Max(a => a.OpenDate);
                    if (pointSaleCloseDto.CloseDate < latestActivityDate)
                    {
                        response.ErrorMessage = $"Дата закрытия не может быть раньше даты последней активности ({latestActivityDate:dd.MM.yyyy})";
                        return response;
                    }
                }


                // Проверяем даты в логотипах
                if (pointSale.Logos != null && pointSale.Logos.Any())
                {
                    var latestLogoDate = pointSale.Logos.Max(l => l.OpenDate);
                    if (pointSaleCloseDto.CloseDate < latestLogoDate)
                    {
                        response.ErrorMessage = $"Дата закрытия не может быть раньше даты последнего логотипа ({latestLogoDate:dd.MM.yyyy})";
                        return response;
                    }
                }


                // Проверяем даты в описаниях
                if (pointSale.Descriptions != null && pointSale.Descriptions.Any())
                {
                    var latestDescriptionDate = pointSale.Descriptions.Max(d => d.OpenDate);
                    if (pointSaleCloseDto.CloseDate < latestDescriptionDate)
                    {
                        response.ErrorMessage = $"Дата закрытия не может быть раньше даты последнего описания ({latestDescriptionDate:dd.MM.yyyy})";
                        return response;
                    }
                }


                // Проверяем даты в описаниях
                if (pointSale.Locations != null && pointSale.Locations.Any())
                {
                    var latestDescriptionDate = pointSale.Locations.Max(d => d.OpenDate);
                    if (pointSaleCloseDto.CloseDate < latestDescriptionDate)
                    {
                        response.ErrorMessage = $"Дата закрытия не может быть раньше даты последнего адреса ({latestDescriptionDate:dd.MM.yyyy})";
                        return response;
                    }
                }

                pointSale.ClosingDate = pointSaleCloseDto.CloseDate;
                pointSale.ClosingStatusId = pointSaleCloseDto.ClosingStatusId;

                _pointSaleDbContext.PointSaleEntities.Update(pointSale);

                if(pointSale.Operators != null && pointSale.Operators.Any())
                {
                    _pointSaleDbContext.Operators.RemoveRange(pointSale.Operators);
                }

                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Внутренняя ошибка";
                return response;
            }
        }
    }
    
}
