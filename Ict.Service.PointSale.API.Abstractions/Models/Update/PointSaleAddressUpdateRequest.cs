using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// Запрос на обновление адреса точки продаж.
    /// </summary>
    public class PointSaleAddressUpdateRequest
    {
        /// <summary>
        /// Идентификатор точки продаж.
        /// </summary>
        [Required]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Широта локации.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Долгота локации.
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Идентификатор адреса, который нужно обновить.
        /// </summary>
        public string AddressId { get; set; } = string.Empty;

        /// <summary>
        /// Дата открытия локации.
        /// </summary>
        public DateOnly? OpenDateLocation { get; set; }
    }
}
