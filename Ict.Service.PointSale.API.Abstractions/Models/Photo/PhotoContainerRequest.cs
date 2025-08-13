using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Photo
{
    /// <summary>
    /// Запрос на загрузку фотографий для торговой точки.
    /// </summary>
    public class PhotoContainerRequest
    {
        /// <summary>
        /// Идентификатор точки продаж, к которой принадлежит фотография.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Дата открытия точки продаж, для которой загружается логотип.
        /// </summary>
        public DateOnly OpenDateLogo { get; set; }

        /// <summary>
        /// Список фотографий, которые необходимо загрузить.
        /// </summary>
        public List<PhotoItemRequest> Photos { get; set; } = new List<PhotoItemRequest>();
    }
}
