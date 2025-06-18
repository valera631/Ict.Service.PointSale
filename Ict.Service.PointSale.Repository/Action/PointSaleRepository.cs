using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.DataBase.DBModels;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
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

        public async Task<OperationResult<Guid?>> AddOperatorToPointSaleAsync(LinkOperatorDto linkOperatorDto)
        {
            OperationResult<Guid?> response = new();
            try
            {
                var pointSale =  _pointSaleDbContext.PointSales
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

                var location = _mapper.Map<Location>(pointSaleDto.Location);


                await _pointSaleDbContext.PointSales.AddAsync(pointSale);
                await _pointSaleDbContext.PointSaleActivities.AddAsync(pointSaleActivity);
                await _pointSaleDbContext.Locations.AddAsync(location);

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

                if (!string.IsNullOrEmpty(filter.Name))
                {
                    query = query.Where(activity => EF.Functions.Like(activity.NamePointSale, $"%{filter.Name}%"));
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

        public async Task<OperationResult<List<PointSaleResultFullDto>>> GetByOperatorAsync(Guid operatorId)
        {
            OperationResult<List<PointSaleResultFullDto>> response = new();

            try
            {
                var targetDate = DateOnly.FromDateTime(DateTime.Now);
                var pointSaleDtos = new List<PointSaleResultFullDto>();

                // Fetch point sales where operatorId is in Operators
                var pointSales = await _pointSaleDbContext.PointSales
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

                // Collect IDs for reference data
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

                // Build dictionaries for reference data
                var creationTypeDict = creationTypes.ToDictionary(ct => ct.CreationTypeId, ct => ct.CreationTypeName);
                var organizationTypeDict = organizationTypes.ToDictionary(ot => ot.OrganizationTypeId, ot => ot.NameType);
                var closingStatusDict = closingStatuses.ToDictionary(cs => cs.ClosingStatusId, cs => cs.NameStatus);
                var ownerTypeDict = ownerTypes.ToDictionary(ot => ot.OwnerTypeId, ot => ot.NameType);
                var chiefPositionDict = chiefPositions.ToDictionary(p => p.ChiefPositionId, p => p.PositionName);

                // Transform point sales into DTOs
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
                        Email = ps.PointSaleActivity?.Email,
                        Phone = ps.PointSaleActivity?.Phone,

                        // Location info
                        LocationId = ps.Location?.LocationId ?? Guid.Empty,
                        OpenDateLocation = ps.Location?.OpenDate ?? default,
                        Address = ps.Location?.Address ?? string.Empty,
                        CreationDateLocation = ps.Location?.EntryDate ?? default,

                        // Types and statuses
                        CreationTypeId = ps.PointSaleEntity.CreationTypeId,
                        CreationTypeName = creationTypeDict.GetValueOrDefault(ps.PointSaleEntity.CreationTypeId, string.Empty),
                        OrganizationTypeId = ps.PointSaleEntity.OrganizationTypeId,
                        OrganizationTypeName = organizationTypeDict.GetValueOrDefault(ps.PointSaleEntity.OrganizationTypeId, string.Empty),
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

        public async Task<OperationResult<PointSaleResultFullDto>> GetPointSaleByIdAsync(Guid pointSaleId, DateOnly? targetDate)
        {
            var response = new OperationResult<PointSaleResultFullDto>();
            try
            {

                var effectiveDate = targetDate ?? DateOnly.FromDateTime(DateTime.Now); // Текущая дата

                var pointSale = await _pointSaleDbContext.PointSales
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
                            .ToList()
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
                 .Where(ct => ct.CreationTypeId == pointSale.PointSaleEntity.CreationTypeId)
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
                    Email = pointSale.PointSaleActivity.Email,
                    Phone = pointSale.PointSaleActivity.Phone,


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
                    DescriptionText = pointSale.Description?.DescriptionText ?? "No description",
                    OperatorIds = pointSale.Operators,

                    // Информация о руководителе
                    ChiefId = pointSale.Chief?.ChiefId ?? Guid.Empty,
                    OpenDateChief = pointSale.Chief?.OpenDate,
                    ChiefName = pointSale.Chief?.ChiefName ?? "No chief assigned",
                    ChiefPositionId = pointSale.Chief?.ChiefPositionId ?? 0,
                    ChiefPositionName = references?.ChiefPositionName ?? "No position assigned"
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
                var pointSales = await _pointSaleDbContext.PointSales
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
                            .FirstOrDefault()
                    })
                    .AsNoTracking()
                    .ToListAsync();

                if (pointSales == null || !pointSales.Any())
                {
                    response.ErrorMessage = $"No active point sales found for owner ID {ownerId} on date {targetDate}.";
                    return response;
                }


                // Получаем ссылочные данные для всех точек продаж
                var pointSaleIds = pointSales.Select(x => x.PointSaleEntity.PointSaleId).ToList();
                var references = await _pointSaleDbContext.CreationTypes
                    .AsSplitQuery()
                    .Where(ct => pointSales.Select(ps => ps.PointSaleEntity.CreationTypeId).Contains(ct.CreationTypeId))
                    .SelectMany(ct => _pointSaleDbContext.PointSales
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
                        ChiefName = ps.Chief?.ChiefName ?? "No chief assigned",
                        ChiefPositionId = ps.Chief?.ChiefPositionId ?? 0,
                        ChiefPositionName = refData?.ChiefPositionName ?? "No position assigned"
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
                var pointSale = _pointSaleDbContext.PointSales
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
                var entity = await _pointSaleDbContext.PointSales
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
                var count = await _pointSaleDbContext.PointSales
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
                var pointSaleCounts = await _pointSaleDbContext.PointSales
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
    }
}
