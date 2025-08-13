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
    /// Класс, описывающий руководителя торговой точки.
    /// </summary>
    [Index(nameof(PointSaleId), nameof(OpenDate))]
    public class Chief
    {
        /// <summary>
        /// Уникальный идентификатор руководителя.
        /// </summary>
        [Key]
        public Guid ChiefId { get; set; }

        /// <summary>
        /// Дата назначения руководителя.
        /// </summary>
        public DateOnly OpenDate { get; set; }

        /// <summary>
        /// Имя директора организации.
        /// </summary>
        public string? ChiefName { get; set; }

        /// <summary>
        /// Внешний ключ для таблицы ChiefPosition.
        /// </summary>
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с таблицей ChiefPosition.
        /// </summary>
        [ForeignKey("ChiefPositionId")]
        public virtual ChiefPosition ChiefPosition { get; set; } = null!;

        /// <summary>
        /// Идентификатор точки продаж, к которой привязан руководитель.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с таблицей точек продаж.
        /// </summary>
        [ForeignKey("PointSaleId")]
        public PointSaleEntity PointSale { get; set; } = null!;

        /// <summary>
        /// Подтверждение шефа.
        /// </summary>
        public bool IsAproved { get; set; }

        /// <summary>
        /// Дата внесения записи в базу данных.
        /// </summary>
        public DateTime EntryDate { get; set; }
    }
}
