using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Представляет тип создания точки продаж.
    /// </summary>
    public class CreationType
    {
        /// <summary>
        /// Уникальный идентификатор типа создания.
        /// </summary>
        [Key]
        public int CreationTypeId { get; set; }

        /// <summary>
        /// Название типа создания.
        /// </summary>
        public string CreationTypeName { get; set; } = null!;


        /// <summary>
        /// Навигационное свойство для связи с таблицей точек продаж.
        /// </summary>
        public virtual ICollection<PointSaleEntity> PointSales { get; set; } = new List<PointSaleEntity>();
    }
}
