using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    /// <summary>
    /// Модель для обновления имени точки продаж.
    /// </summary>
    public class PointSaleNameUpdateDto
    {
        /// <summary>
        /// Идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Имя точки продаж.
        /// </summary>
        public string NamePointSale { get; set; } = string.Empty;

        /// <summary>
        /// Английское имя точки продаж.
        /// </summary>
        public string? EnglishNamePointSale { get; set; }

        /// <summary>
        /// Дата изменения имени точки продаж.
        /// </summary>
        public DateOnly OpenDatePointSale { get; set; }
    }
}
