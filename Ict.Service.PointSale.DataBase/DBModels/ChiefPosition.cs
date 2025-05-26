using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{

    /// <summary>
    /// Класс, описывающий должность руководителя.
    /// </summary>
    public class ChiefPosition
    {
        /// <summary>
        /// Уникальный идентификатор должности руководителя.
        /// </summary>
        [Key]
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Название должности руководителя.
        /// </summary>
        [Required]
        [MaxLength(100)] // Ограничение длины, если нужно
        public string PositionName { get; set; } = null!;

        /// <summary>
        /// Навигационное свойство для связи с таблицей Chief.
        /// </summary>
        public virtual ICollection<Chief> Chiefs { get; set; } = null!;
    }
}
