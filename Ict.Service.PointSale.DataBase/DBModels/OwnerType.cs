using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Класс, представляющий тип владельца точки продаж.
    /// </summary>
    public class OwnerType
    {
        /// <summary>
        /// Уникальный идентификатор типа владельца.
        /// </summary>
        [Key]
        public int OwnerTypeId { get; set; }

        /// <summary>
        /// Название типа владельца (например, "Физическое лицо", "Юридическое лицо").
        /// </summary>
        public string NameType { get; set; } = null!;


        /// <summary>
        /// Навигационное свойство для связи с таблицей точек продаж.
        /// </summary>
        public virtual ICollection<PointSaleEntity> PointSales { get; set; } = new List<PointSaleEntity>();


    }
}
