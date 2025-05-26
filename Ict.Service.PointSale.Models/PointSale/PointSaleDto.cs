using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    public class PointSaleDto
    {
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Индификатор владельца.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Индефикатор типа владельца точки продаж.
        /// </summary>
        public int OwnerTypeId { get; set; }

        /// <summary>
        /// Внешний ключ для таблицы CreationType.
        /// </summary>
        public int CreationTypeId { get; set; }


        /// <summary>
        /// Внешний ключ для таблицы GuidType.
        /// </summary>
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// Дата внесения записи в базу данных.
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Уникальный идентификатор статуса закрытия.
        /// </summary>
        public int? ClosingStatusId { get; set; }

        /// <summary>
        /// Дата закрытия организации, если применимо.
        /// </summary>
        public DateOnly? ClosingDate { get; set; }



    }
}
