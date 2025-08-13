using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// Модель запроса для обновления категорий точек продаж.
    /// </summary>
    public class PointSaleCategoriesUpdateRequest
    {
        /// <summary>
        /// Идентификатор точки продажи, категории которой нужно обновить.
        /// </summary>
        [Required]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Список идентификаторов категорий, которые нужно установить для точки продажи.
        /// </summary>
        public List<int> CategoryIds { get; set; } = new();
    }
}
