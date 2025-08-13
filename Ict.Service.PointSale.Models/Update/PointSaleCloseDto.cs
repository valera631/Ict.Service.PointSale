using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    /// <summary>
    /// Модель для закрытия точки продаж.
    /// </summary>
    public class PointSaleCloseDto
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
