using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    /// <summary>
    /// Модель для подтверждения точки продажи.
    /// </summary>
    public class PoinrSaleIsApprovedDto
    {
        /// <summary>
        /// Идентификатор точки продажи.
        /// </summary>
        public Guid PointSaleId { get; set; }

    }
}
