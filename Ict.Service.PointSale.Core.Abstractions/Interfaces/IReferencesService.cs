using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.References;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Сервис для работы с справочными данными точек продаж.
    /// </summary>
    public interface IReferencesService
    {
        /// <summary>
        /// Асинхронно получает все типы справочных данных.
        /// </summary>
        Task<OperationResult<PointSaleTypesDto>> GetAllTypesAsync();
    }
}
