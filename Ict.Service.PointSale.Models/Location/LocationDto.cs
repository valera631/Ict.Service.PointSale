using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Location
{
    public class LocationDto
    {
        /// <summary>
        /// Уникальный идентификатор локации.
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Дата изменения данных.
        /// </summary>
        public DateOnly OpenDate { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string Address { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string AddressId { get; set; } = string.Empty;

        /// <summary>
        /// Внешний ключ для таблицы PointSales.
        /// Указывает, к какой точке продаж относится данная локация.
        /// </summary>
        public Guid? PointSaleId { get; set; }


        public DateTime EntryDate { get; set; }

    }
}
