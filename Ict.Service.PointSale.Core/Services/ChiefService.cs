using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Update;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;

namespace Ict.Service.PointSale.Core.Services
{
    /// <summary>
    /// Сервис для управления данными руководителей.
    /// Реализует бизнес-логику обновления информации о руководителях.
    /// </summary>
    public class ChiefService : IChiefService
    {
        private readonly IChiefRepository _chiefRepository;

        public ChiefService(IChiefRepository chiefRepository)
        {
            _chiefRepository = chiefRepository;
            
        }

        /// <summary>
        /// Асинхронно обновляет информацию о руководителе.
        /// </summary>
        /// <param name="chiefUpdate">DTO с данными для обновления руководителя.</param>
        /// <returns>Результат операции с булевым значением, указывающим успешность обновления.</returns>
        public async Task<OperationResult<bool>> UpdateChiefAsync(ChiefUpdateDto chiefUpdate)
        {
            OperationResult<bool> response = new OperationResult<bool>();
            try
            {
                var updateResult = await _chiefRepository.UpdateAsync(chiefUpdate);

                if (!updateResult.IsSuccess)
                {
                    response.ErrorMessage = "Не удалось обновить данные руководителя.";
                    return response;
                }

                response.Data = updateResult.Data;

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
