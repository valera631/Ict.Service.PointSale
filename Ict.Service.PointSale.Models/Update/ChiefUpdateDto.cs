using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    /// <summary>
    /// Модель обновление руководителя точки продаж.
    /// </summary>
    public class ChiefUpdateDto
    {
        /// <summary>
        /// Идентификатор точке продаже.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Идентификатор руководителя точки продаж.
        /// </summary>
        public int ChiefPositionId { get; set; }

        /// <summary>
        /// Имя руководителя точки продаж.
        /// </summary>
        public string ChiefName { get; set; } = string.Empty;

        /// <summary>
        /// Дата назначения руководителя.
        /// </summary>
        public DateOnly? OpenDateChief { get; set; }
    }
}
