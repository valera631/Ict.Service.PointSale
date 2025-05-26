using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Description
{
    public class DescriptionDto
    {
        /// <summary>
        /// Уникальный идентификатор описания.
        /// </summary>
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
        /// Внешний ключ для таблицы PointSale.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Время ввода данных в систему.
        /// </summary>
        public DateTime EntryDate { get; set; } = DateTime.Now;

    }
}
