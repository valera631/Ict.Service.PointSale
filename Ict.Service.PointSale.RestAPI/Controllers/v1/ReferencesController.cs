using System.ComponentModel.Design;
using Asp.Versioning;
using AutoMapper;
using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.References;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ict.Service.PointSale.RestAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReferencesController : ControllerBase
    {
        private readonly IReferencesService _referencesService;
        private readonly IMapper _mapper;


        public ReferencesController(IReferencesService referencesService, IMapper mapper)
        {
            _referencesService = referencesService;
            _mapper = mapper;
        }


        [HttpGet("GetAllTypes")]
        public async Task<ApiResult<PointSaleTypesResponse>> GetAllTypes()
        {
            var operation = new ApiResult<PointSaleTypesResponse>();

            try
            {
                var response = await _referencesService.GetAllTypesAsync();

                operation.Result = _mapper.Map<PointSaleTypesResponse>(response.Data);
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }

            return operation;
        }



    }
}
