using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.References
{
    /// <summary>
    /// Представляет элемент категории.
    /// </summary>
    public class CategoryItem
    {
        /// <summary>
        /// Уникальный идентификатор элемента категории.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Путь, связанный с категорией.
        /// </summary>
        public string Path { get; set; } = null!;
    }
}
