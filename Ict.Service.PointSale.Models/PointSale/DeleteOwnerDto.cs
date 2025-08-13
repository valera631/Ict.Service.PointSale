using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    /// <summary>
    /// Модель для удаления владельца точки продаж.
    /// </summary>
    public class DeleteOwnerDto
    {
        /// <summary>
        /// Инденфикатор владельца которога необходимо удалить.
        /// </summary>
        public Guid OwnerId { get; set; }
    }
}
