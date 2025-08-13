using System.Collections.Generic;
using AutoMapper;
using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.PointSale;
using Ict.Service.PointSale.API.Abstractions.Models.Update;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Models.Update;
using Microsoft.AspNetCore.Mvc;

namespace Ict.Service.PointSale.RestAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class PointSalesController : ControllerBase
    {
        private readonly IPointSaleService _pointSaleService;
        private readonly IMapper _mapper;

        public PointSalesController(IPointSaleService pointSaleService, IMapper mapper)
        {
            _pointSaleService = pointSaleService;
            _mapper = mapper;
        }


        /// <summary>
        /// Создает новую точку продаж.
        /// </summary>
        [HttpPost("Create")]
        public async Task<ApiResult<Guid>> CreatePointSale(PointSaleCreateRequest request)
        {
            var operation = ApiResult.CreateResult<Guid>();

            try
            {

                var pointSaleFullDto = _mapper.Map<PointSaleFullDto>(request);

                // Заполнение PointSaleId во всех связанных объектах
                pointSaleFullDto.PointSaleActivity.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;

                if (pointSaleFullDto.Chief != null)
                    pointSaleFullDto.Chief.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;

                if (pointSaleFullDto.Location != null)
                    pointSaleFullDto.Location.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;

                if (pointSaleFullDto.Description != null)
                    pointSaleFullDto.Description.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;


                // Заполнение OrganizationId для расписания работы
                foreach (var schedule in pointSaleFullDto.PointSaleSchedules)
                {
                    schedule.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;
                }

                var result = await _pointSaleService.CreatePointSaleAsync(pointSaleFullDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        [HttpGet("GetByOwnerId/{ownerId}")]
        public async Task<ApiResult<List<PointSaleResultFull>>> GetByOwnerId(Guid ownerId)
        {
            var operation = ApiResult.CreateResult<List<PointSaleResultFull>>();
            try
            {
                var result = await _pointSaleService.GetPointSaleByOwnerIdAsync(ownerId);

                if (result.Data != null)
                {
                    operation.Result = _mapper.Map<List<PointSaleResultFull>>(result.Data);
                }

                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        [HttpGet("GetByOperator/{operatorId}")]
        public async Task<ApiResult<List<PointSaleResultFull>>> GetByOperator(Guid operatorId)
        {
            var operation = ApiResult.CreateResult<List<PointSaleResultFull>>();
            try
            {
                var result = await _pointSaleService.GetByOperatorAsync(operatorId);
                if (result.Data != null)
                {
                    operation.Result = _mapper.Map<List<PointSaleResultFull>>(result.Data);
                }
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        [HttpGet("Get")]
        public async Task<ApiResult<PointSaleResultFull>> GetPointSale([FromBody] PointSaleDateRequest pointSaleDateRequest)
        {
            var operation = ApiResult.CreateResult<PointSaleResultFull>();
            try
            {
                var pointSaleDate = new PointSaleDate
                {
                    OpenDate = pointSaleDateRequest.OpenDate.HasValue
                       ? DateOnly.FromDateTime(pointSaleDateRequest.OpenDate.Value)
                       : DateOnly.FromDateTime(DateTime.UtcNow),
                    PointSaleId = pointSaleDateRequest.PointSaleId
                };


                var result = await _pointSaleService.GetPointSaleAsync(pointSaleDate);

                if (result.Data != null)
                {
                    operation.Result = _mapper.Map<PointSaleResultFull>(result.Data);
                }
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;

        }

        [HttpPost("ConfirmIsApproved")]
        public async Task<ApiResult<bool>> ConfirmPointSaleIsApproved(PoinrSaleIsApprovedRequest request)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var organizationIsApprovedConfirm = new PoinrSaleIsApprovedDto
                {
                    PointSaleId = request.PointSaleId
                };

                var result = await _pointSaleService.ConfirmIsApprovedAsync(organizationIsApprovedConfirm);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = $"Ошибка при подтверждении организации: {ex.Message}";
            }
            return operation;
        }

        [HttpPost("Closing")]
        public async Task<ApiResult<bool>> ClosePointSale(PointSaleCloseRequest request)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var pointSaleClose = new PointSaleCloseDto
                {
                    PointSaleId = request.PointSaleId,
                    CloseDate = request.CloseDate,
                    ClosingStatusId = request.ClosingStatusId

                };

                var result = await _pointSaleService.ClosePointSaleAsync(pointSaleClose);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = $"Ошибка при закрытии организации: {ex.Message}";
            }
            return operation;
        }


        [HttpPost("SubmitVerification/{pointSaleId}")]
        public async Task<ApiResult<List<Guid>>> SubmitVerification(Guid pointSaleId)
        {
            var operation = ApiResult.CreateResult<List<Guid>>();
            try
            {
                var result = await _pointSaleService.SubmitVerificationAsync(pointSaleId);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        [HttpGet("InfoUpdate")]
        public async Task<ApiResult<PointSaleFullInfoRequest>> GetPointSaleInfoUpdate(PointSaleDateRequest pointSaleDateRequest)
        {
            var operation = ApiResult.CreateResult<PointSaleFullInfoRequest>();
            try
            {
                var pointSale = new PointSaleDate
                {
                    OpenDate = pointSaleDateRequest.OpenDate.HasValue
                       ? DateOnly.FromDateTime(pointSaleDateRequest.OpenDate.Value)
                       : DateOnly.FromDateTime(DateTime.UtcNow),
                    PointSaleId = pointSaleDateRequest.PointSaleId

                };

                var result = await _pointSaleService.GetPointSaleDtoAsync(pointSale);
                if (result.Data != null)
                {
                    var organizationResult = _mapper.Map<PointSaleFullInfoRequest>(result.Data);

                    operation.Result = organizationResult;
                }
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }

            return operation;
        }



        /// <summary>
        /// Получает список точек пордаж по фильтру.
        /// </summary>
        [HttpGet("GetByFilter")]
        public async Task<ApiResult<PaginatedResult<PointSaleResultFull>>> GetByFilter(PointSaleFilterRequest filterRequest)
        {
            var operation = ApiResult.CreateResult<PaginatedResult<PointSaleResultFull>>();
            try
            {

                var filter = new PointSaleFilter
                {
                    QueryString = filterRequest.QueryString,
                    IsApproved = filterRequest.IsApproved,
                    OperatorId = filterRequest.OperatorId,
                    HasOperator = filterRequest.HasOperator,
                    HasNoBranches = filterRequest.HasNoBranches,
                    PageNumber = filterRequest.PageNumber,
                    PageSize = filterRequest.PageSize
                };

                var results = await _pointSaleService.GetPointSalesByFilterAsync(filter);
                if (!results.IsSuccess)
                {
                    operation.ErrorMessage = results.ErrorMessage;
                    return operation;
                }
                else
                {
                    if (results.Data != null)
                    {
                        var paginatedResult = new PaginatedResult<PointSaleResultFull>
                        {
                            Items = _mapper.Map<List<PointSaleResultFull>>(results.Data.Items),
                            PageNumber = results.Data.PageNumber,
                            PageSize = results.Data.PageSize,
                            TotalCount = results.Data.TotalCount
                        };

                        operation.Result = paginatedResult;
                    }
                }
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        [HttpPut("UpdateName")]
        public async Task<ApiResult<bool>> UpdatePointSaleName(PointSaleNameUpdateRequest request)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var pointSaleNameUpdateDto = new PointSaleNameUpdateDto
                {
                    PointSaleId = request.PointSaleId,
                    NamePointSale = request.NamePointSale,
                    EnglishNamePointSale = request.EnglishNamePointSale,
                    OpenDatePointSale = request.OpenDatePointSale
                };

                var result = await _pointSaleService.UpdatePointSaleNameAsync(pointSaleNameUpdateDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpPut("UpdateCreationDate")]
        public async Task<ApiResult<bool>> UpdatePointSaleCreationDate(PointSaleCreationDateUpdateRequest request)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var creationDateUpdateDto = new CreationDateUpdateDto
                {
                    PointSaleId = request.PointSaleId,
                    CreationDatePointSale = request.CreationDatePointSale
                };
                var result = await _pointSaleService.UpdateCreationDateAsync(creationDateUpdateDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpPut("UpdateWorkSchedule")]
        public async Task<ApiResult<bool>> UpdateWorkSchedule(PointSaleWorkScheduleUpdateRequest request)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var workScheduleUpdateDto = new WorkScheduleUpdateDto
                {
                    PointSaleId = request.PointSaleId,
                    WorkSchedule = request.WorkSchedule.Select(schedule => new PointSaleScheduleDto
                    {
                        PointSaleId = request.PointSaleId,
                        DayOfWeek = schedule.DayOfWeek,
                        IsWorkingDay = schedule.IsWorkingDay,
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        BreakStartTime = schedule.BreakStartTime,
                        BreakEndTime = schedule.BreakEndTime
                    }).ToList()
                };
                var result = await _pointSaleService.UpdateWorkScheduleAsync(workScheduleUpdateDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpPut("UpdateCategories")]
        public async Task<ApiResult<bool>> UpdateCategories(PointSaleCategoriesUpdateRequest request)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var categoriesUpdateDto = new CategoriesUpdateDto
                {
                    PointSaleId = request.PointSaleId,
                    CategoryIds = request.CategoryIds
                };
                var result = await _pointSaleService.UpdateCategoriesAsync(categoriesUpdateDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        [HttpGet("Count")]
        public async Task<ApiResult<int>> GetCountPointSales()
        {
            var operation = ApiResult.CreateResult<int>();
            try
            {
                var result = await _pointSaleService.GetCountPointSalesAsync();
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpGet("CountsByOwnersId")]
        public async Task<ApiResult<List<PointSaleCountResult>>> GetCountsByOwnersId(List<Guid> ownerIds)
        {
            var operation = ApiResult.CreateResult<List<PointSaleCountResult>>();
            try
            {
                var result = await _pointSaleService.GetCountsByOwnersIdAsync(ownerIds);

                if (result.IsSuccess && result.Data != null)
                {
                    operation.Result = result.Data.Select(count => new PointSaleCountResult
                    {
                        OwnerId = count.OwnerId,
                        PointSaleCount = count.PointSaleCount
                    }).ToList();
                }
                else
                {
                    operation.ErrorMessage = result.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        /// <summary>
        /// Создает связь между оператором и торговой точки.
        /// </summary>
        [HttpPost("linkOperator")]
        public async Task<ApiResult<Guid?>> LinkOperator(LinkOperatorRequest request)
        {
            var operation = ApiResult.CreateResult<Guid?>();
            try
            {
                var pointSaleFullDto = new LinkOperatorDto
                {
                    PointSaleId = request.PointSaleId,
                    OperatorId = request.OperatorId
                };

                var result = await _pointSaleService.LinkOperatorAsync(pointSaleFullDto);

                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        /// <summary>
        /// Удаляет связь между оператором и организацией.
        /// </summary>
        [HttpDelete("unlinkOperator")]
        public async Task<ApiResult<bool>> UnlinkOperatorFromOrganization(OperatorUnlinkRequest unlinkOperatorRequest)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var pointSaleFullDto = new OperatorUnlinkDto
                {
                    PointSaleId = unlinkOperatorRequest.PointSaleId,
                    OperatorId = unlinkOperatorRequest.OperatorId
                };
                var result = await _pointSaleService.UnlinkOperatorAsync(pointSaleFullDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;

            }
            catch (Exception ex)
            {

                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpPost("TransferOwnership")]
        public async Task<ApiResult<bool>> TransferPointSaleOwnership(TransferOwnershipRequest transferOwnershipRequest)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var transferOwnershipDto = new TransferOwnershipDto
                {
                    PointSaleId = transferOwnershipRequest.PointSaleId,
                    NewOwnerId = transferOwnershipRequest.NewOwnerId,
                    OwnerTypeId = transferOwnershipRequest.OwnerTypeId,
                    OwnerName = transferOwnershipRequest.OwnerName
                };

                var result = await _pointSaleService.TransferOwnershipAsync(transferOwnershipDto);

            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpDelete("Owner")]
        public async Task<ApiResult<bool>> DeleteOwnerFromPointSale(DeleteOwnerRequest deleteOwnerRequest)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var deleteOwnerDto = new DeleteOwnerDto
                {
                    OwnerId = deleteOwnerRequest.OwnerId
                };
                var result = await _pointSaleService.DeleteOwnerAsync(deleteOwnerDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpDelete("Delete/{pointSaleId}")]
        public async Task<ApiResult<bool>> DeletePointSale(Guid pointSaleId)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {

                var result = await _pointSaleService.DeletePointSaleAsync(pointSaleId);

                operation.Result = true; // Предположим, что удаление прошло успешно
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;

        }
    }
}