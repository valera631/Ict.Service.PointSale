using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.Description;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models.Description;
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

        /// <summary>
        /// Обновляет описание организации.
        /// </summary>
        /// <param name="descriptionChangeRequest"></param>
        /// <returns></returns>
        [HttpPost("Change")]
        public async Task<ApiResult<Guid>> ChangeDescription(DescriptionChangeRequest descriptionChangeRequest)
        {
            var operation = ApiResult.CreateResult<Guid>();
            try
            {
                var descriptionChange = new DescriptionChangeDto()
                {
                    PointSaleId = descriptionChangeRequest.PointSaleId,
                    DescriptionText = descriptionChangeRequest.DescriptionText
                };

                var result = await _descriptionService.ChangeDescriptionAsync(descriptionChange);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;

            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;

        }


        [HttpPut("Update")]
        public async Task<ApiResult<Guid>> UpdateDescription(DescriptionChangeRequest descriptionChange)
        {
            var operation = ApiResult.CreateResult<Guid>();
            try
            {
                var descriptionChangeDto = new DescriptionChangeDto()
                {
                    PointSaleId = descriptionChange.PointSaleId,
                    DescriptionText = descriptionChange.DescriptionText
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
