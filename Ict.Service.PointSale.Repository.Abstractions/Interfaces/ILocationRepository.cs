using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Update;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с адресами точек продаж.
    /// </summary>
    public interface ILocationRepository
    {
        /// <summary>
        /// Асинхронно обновляет адрес точки продаж на основе предоставленных данных.
        /// </summary>
        Task<OperationResult<bool>> UpdateAddressAsync(AddressUpdateDto addressUpdate);
    }
}
