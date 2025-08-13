using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Модель запроса для получения информации о торговой точке на определенную дату.
    /// </summary>
    public class PointSaleDateRequest
    {
        /// <summary>
        /// Дата на которую мы хотим получить информацию про торговую точку.
        /// </summary>
        public DateTime? OpenDate { get; set; } = null;

        /// <summary>
        /// Уникальный идентификатор торговой точки.
        /// </summary>
        public Guid PointSaleId { get; set; }
    }
}
