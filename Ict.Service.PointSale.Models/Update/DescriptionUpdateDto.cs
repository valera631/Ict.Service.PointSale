using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    /// <summary>
    /// DTO для обновления описания точки продаж.
    /// </summary>
    public class DescriptionUpdateDto
    {
        /// <summary>
        /// Уникальный идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Текст описания торговой точки. Может быть пустым или null.
        /// </summary>
        public string? DescriptionText { get; set; }

        /// <summary>
        /// Дата, когда было обновлено описание.
        /// </summary>
        public DateOnly OpenDateDescription { get; set; }
    }
}
