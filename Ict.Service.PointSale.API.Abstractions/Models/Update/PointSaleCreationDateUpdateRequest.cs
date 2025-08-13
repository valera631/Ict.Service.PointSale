using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// Модель запроса для обновления даты создания точки продажи.
    /// </summary>
    public class PointSaleCreationDateUpdateRequest
    {
        /// <summary>
        /// Уникальный идентификатор точки продажи, для которой нужно обновить дату создания.
        /// </summary>
        [Required]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Дата создания точки продажи, которую нужно установить.
        /// </summary>
        [Required]
        public DateOnly CreationDatePointSale { get; set; }
    }
}
