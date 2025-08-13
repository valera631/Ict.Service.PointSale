using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// Модель запроса для обновления описания точки продажи.
    /// </summary>
    public class PointSaleDescriptionUpdateRequest
    {
        /// <summary>
        /// Уникальный идентификатор точки продажи.
        /// </summary>
        [Required]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Текст описания точки продажи.
        /// </summary>
        public string? DescriptionText { get; set; }

        /// <summary>
        /// Дата открытия точки продажи, которая будет использоваться в описании.
        /// </summary>
        public DateOnly OpenDateDescription { get; set; }
    }
}
