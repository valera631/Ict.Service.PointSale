using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.PointSale
{
    /// <summary>
    /// Модель запроса для удаления владельца точки продаж.
    /// </summary>
    public class DeleteOwnerRequest
    {
        /// <summary>
        /// Инденфикатор владельца которога необходимо удалить.
        /// </summary>
        public Guid OwnerId { get; set; }
    }
}
