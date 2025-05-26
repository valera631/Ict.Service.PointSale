using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;

namespace Ict.Service.PointSale.Core.Services
{
    public class DescriptionService : IDescriptionService
    {

        private readonly IDescriptionRepository _descriptionRepository;
        public DescriptionService(IDescriptionRepository descriptionRepository)
        {
            _descriptionRepository = descriptionRepository;
        }


        public async Task<OperationResult<Guid>> ChangeDescriptionAsync(DescriptionChangeDto descriptionChange)
        {
            OperationResult<Guid> response = new();

            try
            {
                var result = await _descriptionRepository.ChangeDescriptionAsync(descriptionChange);

                if (!result.IsSuccess)
                {
                    response.ErrorMessage = result.ErrorMessage;
                    return response;
                }
                response.Data = result.Data;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<Guid>> UpdateDescriptionAsync(DescriptionChangeDto descriptionChange)
        {
            OperationResult<Guid> response = new();
            try
            {
                var result = await _descriptionRepository.UpdateDescriptionAsync(descriptionChange);
                if (!result.IsSuccess)
                {
                    response.ErrorMessage = result.ErrorMessage;
                    return response;
                }
                response.Data = result.Data;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }
    }
}
