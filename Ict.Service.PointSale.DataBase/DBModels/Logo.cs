using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.DataBase.DBModels
{

    /// <summary>
    /// Класс, представляющий связи торговых точек и их логотипов.
    /// </summary>

    [Index(nameof(OpenDate))]
    [Index(nameof(PointSaleId))]
    public class Logo
    {
        /// <summary>
        /// Уникальный идентификатор логотипа (внешний ключ к таблице Photos).
        /// </summary>
        [Key]
        public Guid LogoId { get; set; }

        /// <summary>
        /// Идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Дата изменения данных.
        /// </summary>
        public DateOnly OpenDate { get; set; }

        /// <summary>
        /// Дата создания записи.
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Навигационное свойство для таблицы PointSale.
        /// </summary>
        [ForeignKey("PointSaleId")]
        public virtual PointSaleEntity PointSale { get; set; } = null!;
    }
}
