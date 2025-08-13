using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Photo
{

    /// <summary>
    /// DTO, представляющий контейнер с фотографиями, относящимися к точке продаж.
    /// </summary>
    public class PhotoContainerDto
    {
        /// <summary>
        /// Идентификатор точки продаж, к которой принадлежит фотография.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Дата открытия логотипа (или дата, связанная с фотографиями).
        /// </summary>
        public DateOnly OpenDateLogo { get; set; }

        /// <summary>
        /// Список фотографий.
        /// </summary>
        public List<PhotoItemDto> Photos { get; set; } = new List<PhotoItemDto>();
    }
}
