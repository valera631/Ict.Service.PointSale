using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    /// <summary>
    /// Модель, содержащая информацию о количестве точек продаж для конкретного владельца.
    /// </summary>
    public class PointSaleCounts
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
