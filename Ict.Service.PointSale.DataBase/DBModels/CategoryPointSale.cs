using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    public class CategoryPointSale
    {
        /// <summary>
        /// Уникальный идентификатор категории
        /// </summary>
        [Key]
        public int CategoryId { get; set; }


        /// <summary>
        /// Идентификатор родительской категории.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Флаг активности категории.
        /// </summary>
        public bool IsEnabled { get; set; }


        public string Path { get; set; } = null!;


        /// <summary>
        /// Навигационное свойство для связи многие-ко-многим с организациями.
        /// EF Core автоматически создаст промежуточную таблицу.
        /// </summary>
        public virtual ICollection<PointSaleEntity> Organizations { get; set; } = new List<PointSaleEntity>();
    }
}
