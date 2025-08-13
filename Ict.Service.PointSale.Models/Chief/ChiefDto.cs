using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Chief
{
    /// <summary>
    /// DTO, представляющий данные руководителя точки продаж.
    /// </summary>
    public class ChiefDto
    {
        /// <summary>
        /// Идентификатор руководителя.
        /// </summary>
        public Guid ChiefId { get; set; }

        /// <summary>
        /// Дата назначения руководителя.
        /// </summary>
        public DateOnly? OpenDate { get; set; }

        /// <summary>
        /// Имя директора организации.
        /// </summary>
        public string? ChiefName { get; set; }

        /// <summary>
        /// Должность директора организации.
        /// </summary>
        public int? ChiefPositionId { get; set; }

        /// <summary>
        /// Индификатор точки продажи, к которой принадлежит шеф 
        /// </summary>
        public Guid? PointSaleId { get; set; }

        /// <summary>
        /// Дата создания записи о руководителе в бд. 
        /// </summary>
        public DateTime EntryDate { get; set; }
    }
}
