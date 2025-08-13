using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{

    /// <summary>
    /// Расширенная модель результата с полной информацией о точке продаж.
    /// </summary>
    public class PointSaleResultFullDto
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
        /// Дата начала деятельности.
        /// </summary>
        public DateOnly? OpenDateActivity { get; set; }

        /// <summary>
        /// Идентификатор типа владельца точки продаж.
        /// </summary>
        public int? OwnerTypeId { get; set; }

        /// <summary>
        /// Название типа владельца.
        /// </summary>
        public string? OwnerTypeName { get; set; }

        /// <summary>
        /// Идентификатор типа создания.
        /// </summary>
        public int? CreationTypeId { get; set; }

        /// <summary>
        /// Название типа создания.
        /// </summary>
        public string? CreationTypeName { get; set; }

        /// <summary>
        /// Тип организации
        /// </summary>
        public int? OrganizationTypeId { get; set; }

        /// <summary>
        /// Название типа организации
        /// </summary>
        public string? OrganizationTypeName { get; set; }

        /// <summary>
        /// Идентификатор статуса закрытия.
        /// </summary>
        public int? ClosingStatusId { get; set; }

        /// <summary>
        /// Название статуса закрытия.
        /// </summary>
        public string? ClosingStatusName { get; set; }

        /// <summary>
        /// Дата регистрации торговой точки.
        /// </summary>
        public DateOnly? CreationDatePointSale { get; set; }


        /// <summary>
        /// Идентификатор локации.
        /// </summary>
        public Guid? LocationId { get; set; }

        /// <summary>
        /// Дата открытия локации.
        /// </summary>
        public DateOnly? OpenDateLocation { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Географическая широта.
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// Географическая долгота.
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// Дата создания записи о локации.
        /// </summary>
        public DateTime? CreationDateLocation { get; set; }

        /// <summary>
        /// Подтверждение организации.
        /// </summary>
        public bool IsAproved { get; set; }

        /// <summary>
        /// Версия записи.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Дата создания записи в системе.
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Дата закрытия точки продаж.
        /// </summary>
        public DateOnly? ClosingDate { get; set; }

        /// <summary>
        /// Текстовое описание.
        /// </summary>
        public string? DescriptionText { get; set; }

        /// <summary>
        /// Идентификатор руководителя.
        /// </summary>
        public Guid ChiefId { get; set; }

        /// <summary>
        /// Дата назначения руководителя.
        /// </summary>
        public DateOnly? OpenDateChief { get; set; }

        /// <summary>
        /// Имя руководителя.
        /// </summary>
        public string ChiefName { get; set; } = "Руководитель не назначен";

        /// <summary>
        /// Идентификатор должности руководителя.
        /// </summary>
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Название должности руководителя.
        /// </summary>
        public string ChiefPositionName { get; set; } = "Должность не назначена";


        /// <summary>
        /// Расписание работы точки продаж.
        /// </summary>
        public List<PointSaleScheduleDto> Schedules { get; set; } = new List<PointSaleScheduleDto>();

        /// <summary>
        /// Список названий категорий точки продаж.
        /// </summary>
        public List<string> CategoryNames { get; set; } = new List<string>();

        /// <summary>
        /// Список идентификаторов операторов, связанных с точкой продаж.
        /// </summary>
        public List<Guid> OperatorIds { get; set; } = new List<Guid>(); // Добавляем для операторов
    }
}
