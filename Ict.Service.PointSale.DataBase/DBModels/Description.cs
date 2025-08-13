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
    [Index(nameof(PointSaleId), nameof(OpenDate))]
    public class Description
    {
        /// <summary>
        /// Уникальный идентификатор описания.
        /// </summary>
        [Key]
        public Guid DescriptionId { get; set; }

        /// <summary>
        /// Описание организации.
        /// </summary>
        public string? DescriptionText { get; set; }

        /// <summary>
        /// Дата изменения данных.
        /// </summary>
        public DateOnly OpenDate { get; set; }

        /// <summary>
        /// Идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; } 


        /// <summary>
        /// Дата создания записи.
        /// </summary>
        public DateTime EntryDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Навигационное свойство для таблицы PointSale.
        /// </summary>
        [ForeignKey("PointSaleId")]
        public virtual PointSaleEntity PointSale { get; set; } = null!;
    }
}
