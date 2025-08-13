using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Photo
{
    /// <summary>
    /// DTO для передачи данных об удалении фотографий, связанных с точкой продаж.
    /// </summary>
    public class PhotosDeleteDto
    {
        /// <summary>
        /// Идентификатор точки продаж, к которой принадлежат фотографии.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Список идентификаторов фотографий для удаления
        /// </summary>
        public List<Guid> PhotoIds { get; set; } = new List<Guid>();
    }
}
