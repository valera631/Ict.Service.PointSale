using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Photo
{
    public class PhotoContainerDto
    {
        /// <summary>
        /// Идентификатор точки продаж, к которой принадлежит фотография.
        /// </summary>
        public Guid PointSaleId { get; set; }

        public List<PhotoItemDto> Photos { get; set; } = new List<PhotoItemDto>();
    }
}
