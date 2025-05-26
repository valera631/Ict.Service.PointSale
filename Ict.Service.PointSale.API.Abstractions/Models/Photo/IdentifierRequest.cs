using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Photo
{
    public class IdentifierRequest
    {
        /// <summary>
        /// Идентификатор точки продажи (взаимоисключающий с OrganizationId).
        /// </summary>
        public Guid PointSaleId { get; set; }
    }
}
