using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    public class PointSaleActivity
    {

        /// <summary>
        /// Уникальный идентификатор деятельности точки продаж.
        /// </summary>
        [Key]
        public Guid PointSaleActivityId { get; set; }

        /// <summary>
        /// Название точки продаж.
        /// </summary>
        public string NamePointSale { get; set; } = null!;

        /// <summary>
        /// Название магазина на английском.
        /// </summary>
        public string? EnglishNamePointSale { get; set; }

        /// <summary>
        /// Внешний ключ для связи с таблицей PointSales.
        /// Указывает, к какой точке продаж относится данная деятельность.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Дата изменения данных о деятельности.
        /// </summary>
        public DateOnly? OpenDate { get; set; }

        /// <summary>
        /// Электронная почта организации.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Контактный телефон.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Дата внесения записи в базу данных.
        /// </summary>
        public DateTime EntryDate { get; set; }


        /// <summary>
        /// Навигационное свойство для связи с таблицей PointSales.
        /// Позволяет получить данные о точке продаж, связанной с этой деятельностью.
        /// </summary>
        [ForeignKey("PointSaleId")]
        public virtual PointSaleEntity PointSales { get; set; } = null!;
    }
}
