using System.Collections.Generic;
using AutoMapper;
using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.PointSale;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
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
                pointSaleFullDto.Chief.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;
                pointSaleFullDto.Location.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;
                pointSaleFullDto.Description.PointSaleId = pointSaleFullDto.PointSale.PointSaleId;


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
                    Name = filterRequest.Name,
                    IsApproved = filterRequest.IsApproved,
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

    }
}