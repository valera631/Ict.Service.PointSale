using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Представляет точки продаж.
    /// </summary>
    public class PointSaleEntity
    {
        /// <summary>
        /// Уникальный идентификатор точек продаж.
        /// </summary>
        [Key]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Внешний ключ для таблицы CreationType.
        /// </summary>
        public int CreationTypeId { get; set; } // Внешний ключ

        /// <summary>
        /// Индификатор владельца точки продаж.
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// Индефикатор типа владельца точки продаж.
        /// </summary>
        public int? OwnerTypeId { get; set; } // Внешний ключ

        public string? OwnerName { get; set; }

        /// <summary>
        /// Внешний ключ для таблицы OrganizationTypeId .
        /// </summary>
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// Дата закрытия организации, если применимо.
        /// </summary>
        public DateOnly? ClosingDate { get; set; }

        /// <summary>
        /// Уникальный идентификатор статуса закрытия.
        /// </summary>
        public int? ClosingStatusId { get; set; }

        /// <summary>
        /// сколько раз обновлялась данная организация можно отследить.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Подтверждение организации.
        /// </summary>
        public bool IsAproved { get; set; }

        /// <summary>
        /// Дата внесения записи в базу данных.
        /// </summary>
        public DateTime EntryDate { get; set; }


        /// <summary>
        /// Навигационное свойство для связи с таблицей CreationType.
        /// Связывает филиал с типом его создания.
        /// </summary>
        [ForeignKey("CreationTypeId")]
        public virtual CreationType CreationType { get; set; } = null!;

        /// <summary>
        /// Навигационное свойство для связи с таблицей ClosingStatuses.
        /// </summary>
        [ForeignKey("ClosingStatusId")]
        public virtual ClosingStatus? ClosingStatus { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с таблицей GuidType.
        /// </summary>
        [ForeignKey("OrganizationTypeId")]
        public virtual OrganizationType OrganizationType { get; set; } = null!;


        [ForeignKey("OwnerTypeId")]
        public virtual OwnerType OwnerType { get; set; } = null!;




        /// <summary>
        /// Навигационное свойство для связи с фотографиями организации.
        /// </summary>
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

        /// <summary>
        /// Навигационное свойство для связи с таблицей логотипов.
        /// </summary>
        public virtual ICollection<Logo> Logos { get; set; } = new List<Logo>();

        /// <summary>
        /// Навигационное свойство для связи с таблицей описаний.
        /// </summary>
        public virtual ICollection<Description> Descriptions { get; set; } = new List<Description>();

        /// <summary>
        /// Навигационное свойство для связи с таблицей PointSaleActivity.
        /// </summary>
        public virtual ICollection<PointSaleActivity> PointSaleActivities { get; set; } = new List<PointSaleActivity>();

        /// <summary>
        /// Навигационное свойство для связи с таблицей Chief.
        /// </summary>
        public virtual ICollection<Chief> Chiefs { get; set; } = new List<Chief>();

        /// <summary>
        /// Навигационное свойство для связи с таблицей Location.
        /// </summary>
        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

        /// <summary>
        /// Навигационное свойство для связи с операторами.
        /// </summary>
        public virtual ICollection<Operator> Operators { get; set; } = new List<Operator>();
    }
}
