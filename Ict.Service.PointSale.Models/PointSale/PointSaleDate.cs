using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    public class PointSaleDate
    {
        /// <summary>
        /// Дата на которую мы хотим получить информацию про торговую точку.
        /// </summary>
        public DateOnly? OpenDate { get; set; } = null;

        /// <summary>
        /// Уникальный идентификатор торговой точки.
        /// </summary>
        public Guid PointSaleId { get; set; }
    }
}
