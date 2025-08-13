using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Модель запроса для отвязки оператора от точки продаж.
    /// </summary>
    public class OperatorUnlinkRequest
    {
        /// <summary>
        /// Идентификатор точки продаж.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Идентификатор оператора.
        /// </summary>
        public Guid OperatorId { get; set; }
    }
}
