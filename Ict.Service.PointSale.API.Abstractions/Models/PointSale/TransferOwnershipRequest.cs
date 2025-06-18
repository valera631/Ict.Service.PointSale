using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Модель запроса для передачи прав собственности на точку продаж.
    /// </summary>
    public class TransferOwnershipRequest
    {
        /// <summary>
        /// Идентификатор точки продаж, права на которую передаются.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Идентификатор нового владельца, которому передаются права на точку продаж.
        /// </summary>
        public Guid NewOwnerId { get; set; }

        /// <summary>
        /// Идентификатор типа владельца точки продаж, которому передаются права.
        /// </summary>
        public int OwnerTypeId { get; set; }

        /// <summary>
        /// Имя нового владельца точки продаж, которому передаются права.
        /// </summary>
        public string OwnerName { get; set; } = null!;
    }
}
