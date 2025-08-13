using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Models.Update;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;

namespace Ict.Service.PointSale.Core.Services
{

    /// <summary>
    /// Сервис для управления описаниями точек продаж.
    /// Реализует бизнес-логику обновления описаний.
    /// </summary>
    public class DescriptionService : IDescriptionService
    {

        private readonly IDescriptionRepository _descriptionRepository;
        public DescriptionService(IDescriptionRepository descriptionRepository)
        {
            _descriptionRepository = descriptionRepository;
        }




        /// <summary>
        /// Асинхронно обновляет описание точки продаж.
        /// </summary>
        /// <param name="descriptionUpdateDto">DTO с данными для обновления описания.</param>
        public async Task<OperationResult<bool>> UpdateDescriptionAsync(DescriptionUpdateDto descriptionUpdateDto)
        {
            OperationResult<bool> response = new();
            try
            {
                var result = await _descriptionRepository.UpdateDescriptionAsync(descriptionUpdateDto);
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
