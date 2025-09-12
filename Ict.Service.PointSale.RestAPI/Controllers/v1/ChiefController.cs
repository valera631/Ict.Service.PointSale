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
    public class ChiefController : ControllerBase
    {
        private readonly IChiefService _chiefService;
        public ChiefController(IChiefService chiefService)
        {
            _chiefService = chiefService;
        }

        [HttpPut("Update")]
        public async Task<ApiResult<bool>> UpdateChief(PointSaleChiefUpdateRequest pointSaleChiefUpdateRequest)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                if (!ModelState.IsValid)
                {
                    operation.ErrorMessage = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                    return operation;
                }

                var chiefUpdateDto = new ChiefUpdateDto
                {
                    PointSaleId = pointSaleChiefUpdateRequest.PointSaleId,
                    ChiefPositionId = pointSaleChiefUpdateRequest.ChiefPositionId,
                    ChiefName = pointSaleChiefUpdateRequest.ChiefName,
                    OpenDateChief = pointSaleChiefUpdateRequest.OpenDateChief
                };

                var result = await _chiefService.UpdateChiefAsync(chiefUpdateDto);
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
