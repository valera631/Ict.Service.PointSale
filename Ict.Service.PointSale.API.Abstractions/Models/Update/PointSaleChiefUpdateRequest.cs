using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// Модель запроса для обновление руководителя точки продажи.
    /// </summary>
    public class PointSaleChiefUpdateRequest
    {
        /// <summary>
        /// Уникальный идентификатор точки продажи.
        /// </summary>
        [Required]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Идентификатор должности руководителя точки продажи.
        /// </summary>
        [Required]
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Имя руководителя точки продажи.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ChiefName { get; set; } = string.Empty;

        /// <summary>
        /// Дата назначения руководителя точки продажи.
        /// </summary>
        public DateOnly? OpenDateChief { get; set; }
    }
}
