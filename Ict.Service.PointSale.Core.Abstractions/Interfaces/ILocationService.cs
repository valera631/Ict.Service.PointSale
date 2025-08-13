using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Update;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Сервис для работы с адресом точек продаж.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Асинхронно обновляет адрес точки продаж на основе предоставленных данных.
        /// </summary>
        Task<OperationResult<bool>> UpdateAddressAsync(AddressUpdateDto addressUpdateDto);
    }
}
