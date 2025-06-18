using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{

    /// <summary>
    /// Модель запроса на создание точки продаж.
    /// </summary>
    public class PointSaleCreateRequest
    {
        /// <summary>
        /// Индификатор владельца.
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// Индефикатор типа владельца точки продаж.
        /// </summary>
        public int? OwnerTypeId { get; set; }

        public string? OwnerName { get; set; } 

        /// <summary>
        /// Индификатор типа создания.
        /// </summary>
        public int CreationTypeId { get; set; }

        /// <summary>
        /// Индификатор типа организации.
        /// </summary>
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// Название точки продаж.
        /// </summary>
        public string NamePointSale { get; set; } = null!;

        /// <summary>
        /// Название магазина на английском.
        /// </summary>
        public string? EnglishNamePointSale { get; set; }

        /// <summary>
        /// Дата открытия точки продажи.
        /// </summary>
        public DateOnly OpenDatePointSale { get; set; }

        /// <summary>
        /// Электронная почта организации.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Контактный телефон.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Дата назначения руководителя.
        /// </summary>
        public DateOnly OpenDateChif { get; set; }

        /// <summary>
        /// Имя руководителя организации.
        /// </summary>
        public string? ChiefName { get; set; }

        public string? DescriptionText { get; set; }

        /// <summary>
        /// Индификатор должности руководителя организации.
        /// </summary>
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Дата изменения данных.
        /// </summary>
        public DateOnly OpenDateLocation { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string Address { get; set; } = null!;

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string LocationId { get; set; } = string.Empty;
    }
}
