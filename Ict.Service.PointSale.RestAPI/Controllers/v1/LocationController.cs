using Asp.Versioning;
using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.Update;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models.Update;
using Microsoft.AspNetCore.Mvc;

namespace Ict.Service.PointSale.RestAPI.Controllers.v1
{

    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Метод обновления адреса точки продаж
        /// </summary>
        [HttpPut("UpdateAddress")]
        public async Task<ApiResult<bool>> UpdateAddress(PointSaleAddressUpdateRequest request)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                if (!ModelState.IsValid)
                {
                    operation.ErrorMessage = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                    return operation;
                }
                var addressUpdateDto = new AddressUpdateDto
                {
                    PointSaleId = request.PointSaleId,
                    AddressId = request.AddressId,
                    Address = request.Address,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    OpenDateLocation = request.OpenDateLocation,
                };
                var result = await _locationService.UpdateAddressAsync(addressUpdateDto);
                operation.Result = result.Data;
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
