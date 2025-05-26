using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    public class Photo
    {
        /// <summary>
        /// Идентификатор фотографии (первичный ключ).
        /// </summary>
        [Key]
        public Guid PhotoId { get; set; }

        /// <summary>
        /// Идентификатор точки продажи (внешний ключ, может быть null).
        /// </summary>
        public Guid PointSaleId { get; set; }


        /// <summary>
        /// Является ли фотография основной для организации.
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с точкой продажи.
        /// </summary>
        [ForeignKey("PointSaleId")]
        public virtual PointSaleEntity PointSale { get; set; } = null!;
    }
}
