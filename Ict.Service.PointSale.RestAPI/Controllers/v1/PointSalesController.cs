using AutoMapper;
using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.PointSale;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
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
    }
}