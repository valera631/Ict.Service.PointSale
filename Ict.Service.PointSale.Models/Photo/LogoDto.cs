using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Photo
{
    /// <summary>
    /// Модель для представления логотипа точки продаж.
    /// </summary>
    public class LogoDto
    {
        /// <summary>
        /// Уникальный идентификатор логотипа.
        /// </summary>
        public Guid LogoId { get; set; }

        /// <summary>
        /// ДАта создания логотипа.
        /// </summary>
        public DateOnly OpenDateLogo { get; set; }
    }
}
