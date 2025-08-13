using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    /// <summary>
    /// DTO для обновления даты создания точки продаж.
    /// </summary>
    public class CreationDateUpdateDto
    {
        /// <summary>
        /// Уникальный идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Дата создания точки продаж.
        /// </summary>
        public DateOnly CreationDatePointSale { get; set; }
    }
}
