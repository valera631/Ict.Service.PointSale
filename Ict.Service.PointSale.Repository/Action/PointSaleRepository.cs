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
    }
}
