using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.API.Abstractions.Models.Schedule;

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

        /// <summary>
        /// Имя владельца точки продаж.
        /// </summary>
        public string? OwnerName { get; set; } 

        /// <summary>
        /// Индификатор типа создания.
        /// </summary>
        public int? CreationTypeId { get; set; }


        /// <summary>
        /// Время регистрации
        /// </summary>
        public DateOnly? CreationDatePointSale { get; set; }

        /// <summary>
        /// Индификатор типа организации.
        /// </summary>
        public int? OrganizationTypeId { get; set; }

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
        public DateOnly? OpenDatePointSale { get; set; }

        /// <summary>
        /// Дата назначения руководителя.
        /// </summary>
        public DateOnly? OpenDateChief { get; set; }



        /// <summary>
        /// Дата добовление описание.
        /// </summary>
        public DateOnly? OpenDateDescription { get; set; }

        /// <summary>
        /// Имя руководителя организации.
        /// </summary>
        public string? ChiefName { get; set; }

        /// <summary>
        /// Описание точки продаж.
        /// </summary>
        public string? DescriptionText { get; set; }

        /// <summary>
        /// Индификатор должности руководителя организации.
        /// </summary>
        public int? ChiefPositionId { get; set; }

        /// <summary>
        /// Дата изменения данных.
        /// </summary>
        public DateOnly? OpenDateLocation { get; set; }

        /// <summary>
        /// Адрес локации.
        /// </summary>
        public string? Address { get; set; } = null!;

        /// <summary>
        /// Широта и долгота точки продаж.
        /// </summary>
        public float? Latitude { get; set; }

        public float? Longitude { get; set; }

        /// <summary>
        /// Идентификатор локации.
        /// </summary>
        public string? LocationId { get; set; } = string.Empty;

        /// <summary>
        /// Список альтернативных названий точки продаж.
        /// </summary>
        public List<string> AlternativeName { get; set; } = new List<string>();

        /// <summary>
        /// Список идентификаторов категорий, к которым принадлежит точка продаж.
        /// </summary>
        public List<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// График работы точки продаж.
        /// </summary>
        public List<WorkScheduleRequest> WorkSchedule { get; set; } = new List<WorkScheduleRequest>();
    }
}
