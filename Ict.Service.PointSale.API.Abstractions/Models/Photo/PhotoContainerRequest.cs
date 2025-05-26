using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Photo
{
    public class PhotoContainerRequest
    {
        /// <summary>
        /// Идентификатор точки продаж, к которой принадлежит фотография.
        /// </summary>
        public Guid PointSaleId { get; set; }

        public List<PhotoItemRequest> Photos { get; set; } = new List<PhotoItemRequest>();
    }
}
