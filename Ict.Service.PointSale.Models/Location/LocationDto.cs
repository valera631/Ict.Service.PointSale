using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Location
{
    /// <summary>
    /// DTO, представляющий данные о локации точки продаж.
    /// </summary>
    public class LocationDto
    {
        /// <summary>
        /// Уникальный идентификатор локации.
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Дата изменения данных.
        /// </summary>
        public DateOnly? OpenDate { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Географическая широта.
        /// Может быть null, если координаты не заданы.
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// Географическая долгота.
        /// Может быть null, если координаты не заданы.
        /// </summary>
        public float? Longitude { get; set; }


        /// <summary>
        /// Идентификатор адреса в сервисе локации.
        /// </summary>
        public string? AddressId { get; set; } = string.Empty;

        /// <summary>
        /// Внешний ключ для таблицы PointSales.
        /// Указывает, к какой точке продаж относится данная локация.
        /// </summary>
        public Guid? PointSaleId { get; set; }

        /// <summary>
        /// Дата внесения записи в базу данных.
        /// </summary>
        public DateTime EntryDate { get; set; }

    }
}
