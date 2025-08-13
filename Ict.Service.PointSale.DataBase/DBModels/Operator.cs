using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Представляет оператора, связанного с одной или несколькими точками продаж.
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// Уникальный идентификатор оператора.
        /// </summary>
        [Key]
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с точкой продажи.
        /// </summary>
        public virtual ICollection<PointSaleEntity> PointsSale { get; set; } = new List<PointSaleEntity>();
    }
}
