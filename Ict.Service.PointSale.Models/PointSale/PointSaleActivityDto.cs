using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    public class PointSaleActivityDto
    {
        /// <summary>
        /// Уникальный идентификатор деятельности точки продаж.
        /// </summary>
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
        public DateOnly OpenDate { get; set; }


        /// <summary>
        /// Электронная почта организации.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Контактный телефон.
        /// </summary>
        public string? Phone { get; set; }


        public DateTime EntryDate { get; set; }
    }
}
