using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Photo
{
    public class PointSalePhotoDto
    {
        /// <summary>
        /// Идентификатор фотографии
        /// </summary>
        public Guid PhotoId { get; set; }

        /// <summary>
        /// Данные фотографии в формате base64
        /// </summary>
        public byte[] PhotoData { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Дата создания логотипа
        /// </summary>
        public DateOnly OpenDateLogo { get; set; }

        /// <summary>
        /// Имя файла фотографии
        /// </summary>
        public string PhotoName { get; set; } = string.Empty;


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
