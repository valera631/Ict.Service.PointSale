using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.API.Abstractions.Models.Schedule;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Модель запроса для получения полной информации о точке продажи.
    /// </summary>
    public class PointSaleFullInfoRequest
    {
        /// <summary>
        /// Уникальный идентификатор точки продажи.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Уникальный идентификатор активности точки продажи.
        /// </summary>
        public Guid PointSaleActivityId { get; set; }

        /// <summary>
        /// Уникальный идентификатор описания точки продажи.
        /// </summary>
        public Guid DescriptionId { get; set; }

        /// <summary>
        /// Уникальный идентификатор руководителя точки продажи.
        /// </summary>
        public Guid ChiefId { get; set; }

        /// <summary>
        /// Уникальный идентификатор местоположения точки продажи.
        /// </summary>
        public string? LocationId { get; set; }

        /// <summary>
        /// Уникальный идентификатор владельца точки продажи.
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// Идентификатор типа владельца точки продажи.
        /// </summary>
        public int? OwnerTypeId { get; set; }

        /// <summary>
        /// Имя владельца точки продажи.
        /// </summary>
        public string? OwnerName { get; set; }

        /// <summary>
        /// Идентификатор типа создания точки продажи.
        /// </summary>
        public int CreationTypeId { get; set; }

        /// <summary>
        /// Дата регистрации точки продажи.
        /// </summary>
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// Название точки продажи.
        /// </summary>
        public string NamePointSale { get; set; } = null!;

        /// <summary>
        /// Название точки продажи на английском языке.
        /// </summary>
        public string? EnglishNamePointSale { get; set; }

        /// <summary>
        /// Дата создания точки продажи.
        /// </summary>
        public DateOnly? CreationDate { get; set; }

        /// <summary>
        /// Дата открытия описание точки продажи.
        /// </summary>
        public DateOnly? OpenDateDescription { get; set; }

        /// <summary>
        /// Дата открытия точки продажи(активити).
        /// </summary>
        public DateOnly OpenDatePointSale { get; set; }

        /// <summary>
        /// Дата назначения руководителя точки продажи.
        /// </summary>
        public DateOnly? OpenDateChif { get; set; }

        /// <summary>
        /// Имя руководителя точки продажи.
        /// </summary>
        public string? ChiefName { get; set; }

        /// <summary>
        /// Описание точки продажи.
        /// </summary>
        public string? DescriptionText { get; set; }

        /// <summary>
        /// Идентификатор позиции руководителя точки продажи.
        /// </summary>
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Дата открытия местоположения точки продажи.
        /// </summary>
        public DateOnly? OpenDateLocation { get; set; }

        /// <summary>
        /// Адрес точки продажи.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Широта точки продажи.
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// Долгота точки продажи.
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// Список альтернативных названий точки продажи.
        /// </summary>
        public List<string> AlternativeName { get; set; } = new List<string>();

        /// <summary>
        /// Список идентификаторов категорий, к которым принадлежит точка продажи.
        /// </summary>
        public List<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// График работы точки продажи.
        /// </summary>
        public List<WorkScheduleRequest> WorkSchedule { get; set; } = new List<WorkScheduleRequest>();
    }
}
