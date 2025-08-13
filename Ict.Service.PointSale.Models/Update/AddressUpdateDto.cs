using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    /// <summary>
    /// DTO для обновления адреса точки продаж.
    /// </summary>
    public class AddressUpdateDto
    {
        /// <summary>
        /// Уникальный идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Географическая широта.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Географическая долгота.
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Идентификатор адреса.
        /// </summary>
        public string AddressId { get; set; } = string.Empty;

        /// <summary>
        /// Дата открытия локации.
        /// </summary>
        public DateOnly? OpenDateLocation { get; set; }
    }
}
