using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Photo
{

    /// <summary>
    /// Класс, представляющий результат операции с фотографией торговой точки.
    /// </summary>
    public class PointSalePhotoResult
    {

        /// <summary>
        /// Идентификатор фотографии
        /// </summary>
        public Guid PhotoId { get; set; }

        /// <summary>
        /// Данные фотографии в формате base64
        /// </summary>
        public string PhotoData { get; set; } = string.Empty;

        /// <summary>
        /// Имя файла фотографии
        /// </summary>
        public string PhotoName { get; set; } = string.Empty;

        /// <summary>
        /// Дата создания логотипа
        /// </summary>
        public DateOnly OpenDateLogo { get; set; }

        /// <summary>
        /// Флаг главной фотографии
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        /// Тип контента
        /// </summary>
        public string PhotoContentType { get; set; } = string.Empty;

    }
}
