using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Photo
{
    public class PhotosDeleteRequest
    {
        /// <summary>
        /// Идентификаторы фотографий для удаления.
        /// </summary>
        public List<Guid> ImageIds { get; set; } = new List<Guid>();
        /// <summary>
        /// Идентификатор точки продаж, к которой относятся фотографии.
        /// </summary>
        public Guid PointSaleId { get; set; }
    }
}
