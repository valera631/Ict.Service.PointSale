using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Модель запроса для подверждения точки продажи.
    /// </summary>
    public class PoinrSaleIsApprovedRequest
    {
        /// <summary>
        /// Идентификатор точки продажи.
        /// </summary>
        public Guid PointSaleId { get; set; }
    }
}
