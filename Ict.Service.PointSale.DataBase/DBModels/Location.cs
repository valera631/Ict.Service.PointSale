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
    /// Класс, представляющий сущность "Локации".
    /// </summary>
    [Index(nameof(PointSaleId), nameof(OpenDate))]
    public class Location
    {
        /// <summary>
        /// Уникальный идентификатор локации.
        /// </summary>
        [Key]
        public Guid LocationId { get; set; }

        /// <summary>
        /// Дата изменения данных.
        /// </summary>
        public DateOnly OpenDate { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string Address { get; set; } = null!;

        /// <summary>
        /// Географическая широта локации.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Географическая долгота локации.
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Внешний идентификатор адреса (из сервиса location).
        /// </summary>
        public string AddressId { get; set; } = string.Empty;


        /// <summary>
        /// Внешний ключ для таблицы PointSales.
        /// Указывает, к какой точке продаж относится данная локация.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с таблицей PointSales.
        /// Позволяет получить данные о связанной точке продаж.
        /// </summary>
        [ForeignKey("PointSaleId")]
        public virtual PointSaleEntity PointSale { get; set; } = null!;

        /// <summary>
        /// Подтверждение организации.
        /// </summary>
        public bool IsAproved { get; set; }

        /// <summary>
        /// Дата внесения записи в базу данных.
        /// </summary>
        public DateTime EntryDate { get; set; }

    }
}
