using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// модель для закрытие точки продаж.
    /// </summary>
    public class PointSaleCloseRequest
    {
        /// <summary>
        /// Идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Дата закрытия точки продаж.
        /// </summary>
        public DateOnly CloseDate { get; set; }

        /// <summary>
        /// Идентификатор статуса закрытия точки продаж.
        /// </summary>
        public int ClosingStatusId { get; set; }
    }
}
