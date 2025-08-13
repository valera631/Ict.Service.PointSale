using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    public class CategoriesUpdateDto
    {
        /// <summary>
        /// Уникальный идентификатор точки продажи, к которой относятся категории.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Список идентификаторов категорий, которые будут установлены для точки продажи.
        /// </summary>
        public List<int> CategoryIds { get; set; } = new();
    }
}
