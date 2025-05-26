using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    public class PointSaleResultFullDto
    {
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Идентификатор владельца.
        /// </summary>
        public Guid OwnerId { get; set; }


        /// <summary>
        /// Название торговойточки-владельца.
        /// </summary>
        public string PointSaleName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateOnly? OpenDateActivity { get; set; }

        /// <summary>
        /// Идентификатор типа владельца точки продаж.
        /// </summary>
        public int OwnerTypeId { get; set; }

        public string OwnerTypeName { get; set; } = string.Empty;

        // Типы и статусы
        public int CreationTypeId { get; set; }

        public string CreationTypeName { get; set; } = string.Empty;

        public int OrganizationTypeId { get; set; }

        public string OrganizationTypeName { get; set; } = string.Empty;

        public int? ClosingStatusId { get; set; }

        public string ClosingStatusName { get; set; } = string.Empty;


        // Информация о локации
        public Guid LocationId { get; set; }

        public DateOnly? OpenDateLocation { get; set; }

        public string Address { get; set; } = string.Empty;

        public DateTime? CreationDateLocation { get; set; }

        // Метаданные
        public bool IsAproved { get; set; }

        public int Version { get; set; }

        public DateTime EntryDate { get; set; }

        public DateOnly? ClosingDate { get; set; }

        public string? DescriptionText { get; set; }

        // Информация о руководителе (Chief)
        public Guid ChiefId { get; set; }

        public DateOnly? OpenDateChief { get; set; }

        public string ChiefName { get; set; } = "Руководитель не назначен";

        public int ChiefPositionId { get; set; }

        public string ChiefPositionName { get; set; } = "Должность не назначена";
    }
}
