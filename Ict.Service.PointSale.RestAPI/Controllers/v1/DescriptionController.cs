using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.Description;
using Ict.Service.PointSale.API.Abstractions.Models.Update;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Models.Update;
using Microsoft.AspNetCore.Mvc;

namespace Ict.Service.PointSale.RestAPI.Controllers.v1
{

    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public class DescriptionController : ControllerBase
    {
        private readonly IDescriptionService _descriptionService;
        public DescriptionController(IDescriptionService descriptionService)
        {
            _descriptionService = descriptionService;
        }

       

        [HttpPut("Update")]
        public async Task<ApiResult<bool>> UpdateDescription(PointSaleDescriptionUpdateRequest descriptionChange)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var descriptionChangeDto = new DescriptionUpdateDto()
                {
                    PointSaleId = descriptionChange.PointSaleId,
                    DescriptionText = descriptionChange.DescriptionText,
                    OpenDateDescription = descriptionChange.OpenDateDescription
                };

                var result = await _descriptionService.UpdateDescriptionAsync(descriptionChangeDto);
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
