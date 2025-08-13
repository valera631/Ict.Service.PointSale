using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// Модель запроса для обновления название точки продажи.
    /// </summary>
    public class PointSaleNameUpdateRequest
    {

        /// <summary>
        /// Идентификатор точки продажи, которую нужно обновить.
        /// </summary>
        [Required]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Название точки продажи.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NamePointSale { get; set; } = string.Empty;

        /// <summary>
        /// Название точки продажи на английском языке.
        /// </summary>
        public string? EnglishNamePointSale { get; set; }

        /// <summary>
        /// Дата открытия точки продажи(активити).
        /// </summary>
        public DateOnly OpenDatePointSale { get; set; }
    }
}
