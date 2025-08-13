using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{

    /// <summary>
    /// Модель, представляющая запрос данных по торговой точке на определённую дату.
    /// </summary>
    public class PointSaleDate
    {
        /// <summary>
        /// Дата на которую мы хотим получить информацию про торговую точку.
        /// </summary>
        public DateOnly? OpenDate { get; set; } 

        /// <summary>
        /// Уникальный идентификатор торговой точки.
        /// </summary>
        public Guid PointSaleId { get; set; }
    }
}
