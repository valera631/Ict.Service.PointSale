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
    /// Сервис для управления локациями и адресами точек продаж.
    /// </summary>
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }


        /// <summary>
        /// Асинхронно обновляет адрес точки продаж.
        /// </summary>
        /// <param name="addressUpdateDto">DTO с данными для обновления адреса.</param>
        /// <returns>Результат операции с булевым значением, указывающим успешность обновления.</returns>
        public async Task<OperationResult<bool>> UpdateAddressAsync(AddressUpdateDto addressUpdateDto)
        {
            OperationResult<bool> response = new OperationResult<bool>();
            try
            {
                var updateResult = await _locationRepository.UpdateAddressAsync(addressUpdateDto);
                if (!updateResult.IsSuccess)
                {
                    response.ErrorMessage = "Не удалось обновить адрес.";
                    return response;
                }
                response.Data = updateResult.Data;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
    }
}
