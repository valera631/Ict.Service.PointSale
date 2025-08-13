using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;

namespace Ict.Service.PointSale.Core.Interfaces
{
    /// <summary>
    /// Интерфейс для поиска и фильтрации точек продаж.
    /// </summary>
    public interface IPointSaleSearch
    {
        /// <summary>
        /// Асинхронно получает идентификаторы точек продаж, отфильтрованных по заданным критериям.
        /// </summary>
        /// <param name="filter">Объект с параметрами фильтрации точек продаж.</param>
        /// <returns>Результат операции с пагинированным списком идентификаторов точек продаж.</returns>
        Task<OperationResult<PaginatedResult<Guid>>> GetFilteredPointsSaleAsync(PointSaleFilter filter);
    }
}
