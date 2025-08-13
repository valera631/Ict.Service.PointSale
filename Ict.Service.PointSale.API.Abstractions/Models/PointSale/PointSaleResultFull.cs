using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.API.Abstractions.Models.Schedule;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Модель полного результата точки продаж.
    /// </summary>
    public class PointSaleResultFull
    {
        /// <summary>
        /// Уникальный идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Идентификатор владельца.
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// Имя владельца точки продаж.
        /// </summary>
        public string? OwnerName { get; set; }

        /// <summary>
        /// Название торговойточки-владельца.
        /// </summary>
        public string PointSaleName { get; set; } = string.Empty;

        /// <summary>
        /// Время регистрации
        /// </summary>
        public DateOnly? CreationDatePointSale { get; set; }

        /// <summary>
        /// Дата открытия точки продажи(активити).
        /// </summary>
        public DateOnly? OpenDateActivity { get; set; }

        /// <summary>
        /// Идентификатор типа владельца точки продаж.
        /// </summary>
        public int? OwnerTypeId { get; set; }

        /// <summary>
        /// Тип владельца точки продаж.
        /// </summary>
        public string? OwnerTypeName { get; set; }

        /// <summary>
        /// Идентификатор типа создания точки продаж.
        /// </summary>
        public int? CreationTypeId { get; set; }

        /// <summary>
        /// Название типа создания точки продаж.
        /// </summary>
        public string? CreationTypeName { get; set; }

        /// <summary>
        /// Тип организации точки продаж.
        /// </summary>
        public int? OrganizationTypeId { get; set; }

        /// <summary>
        /// Название типа организации точки продаж.
        /// </summary>
        public string? OrganizationTypeName { get; set; }

        /// <summary>
        /// Тип закрытие точки продаж.
        /// </summary>
        public int? ClosingStatusId { get; set; }

        /// <summary>
        /// Название типа закрытия точки продаж.
        /// </summary>
        public string? ClosingStatusName { get; set; }

        /// <summary>
        /// Индификатор локации точки продаж.
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Дата открытия локации точки продаж.
        /// </summary>
        public DateOnly? OpenDateLocation { get; set; }

        /// <summary>
        /// Адрес точки продаж.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Широта точки продаж (для геолокации).
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// Долгота точки продаж (для геолокации).
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// Дата создания локации точки продаж.
        /// </summary>
        public DateTime? CreationDateLocation { get; set; }

        /// <summary>
        /// Флаг, указывающий, что точка продаж одобрена.
        /// </summary>
        public bool IsAproved { get; set; }

        /// <summary>
        /// Версия точки продаж.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Дата создания точки продаж в системе.
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Дата закрытия точки продаж (если применимо).
        /// </summary>
        public DateOnly? ClosingDate { get; set; }

        /// <summary>
        /// Описание точки продаж.
        /// </summary>
        public string? DescriptionText { get; set; }

        /// <summary>
        /// Идентификатор шефа организации.
        /// </summary>
        public Guid ChiefId { get; set; }

        /// <summary>
        /// Дата назначения шефа организации.
        /// </summary>
        public DateOnly? OpenDateChief { get; set; }

        /// <summary>
        /// Имя шефа организации.
        /// </summary>
        public string ChiefName { get; set; } = "Руководитель не назначен";

        /// <summary>
        /// Идентификатор должности шефа организации.
        /// </summary>
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Название должности шефа организации.
        /// </summary>
        public string ChiefPositionName { get; set; } = "Должность не назначена";

        /// <summary>
        /// График работы торговой точки.
        /// </summary>
        public List<WorkScheduleRequest> Schedules { get; set; } = new List<WorkScheduleRequest>();

        /// <summary>
        /// Список категорий, к которым принадлежит точка продаж.
        /// </summary>
        public List<string> CategoryNames { get; set; } = new List<string>();

        /// <summary>
        /// Операторы организации
        /// </summary>
        public List<Guid> OperatorIds { get; set; } = new List<Guid>(); // Добавляем для операторов
    }
}
