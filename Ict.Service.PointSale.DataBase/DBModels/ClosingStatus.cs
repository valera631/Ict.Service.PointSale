using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Представляет статус закрытия точки продаж.
    /// </summary>
    public class ClosingStatus
    {
        /// <summary>
        /// Уникальный идентификатор статуса закрытия.
        /// </summary>
        [Key]
        public int ClosingStatusId { get; set; }

        /// <summary>
        /// Название статуса закрытия (например, "Закрыто", "В процессе закрытия").
        /// </summary>
        public string NameStatus { get; set; } = null!;


        /// <summary>
        /// Навигационное свойство для связи с таблицей точек продаж.
        /// </summary>
        public virtual ICollection<PointSaleEntity> PointSales { get; set; } = new List<PointSaleEntity>();
    }
}
