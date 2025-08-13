using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    /// <summary>
    /// DTO для связи оператора с точкой продаж.
    /// </summary>
    public class LinkOperatorDto
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
