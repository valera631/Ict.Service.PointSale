using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    /// <summary>
    /// DTO для отвязки оператора от точки продаж.
    /// </summary>
    public class OperatorUnlinkDto
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
