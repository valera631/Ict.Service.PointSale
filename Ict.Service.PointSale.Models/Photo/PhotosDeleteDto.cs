using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Photo
{
    public class PhotosDeleteDto
    {

        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Список идентификаторов фотографий для удаления
        /// </summary>
        public List<Guid> PhotoIds { get; set; } = new List<Guid>();
    }
}
