using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Результат подсчета количества точек продаж владельца.
    /// </summary>
    public class PointSaleCountResult
    {
        /// <summary>
        /// Идентификатор владельца точек продаж.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Количество точек продаж, принадлежащих владельцу.
        /// </summary>
        public int PointSaleCount { get; set; }
    }
}
