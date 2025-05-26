using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Photo
{
    public class PhotoItemRequest
    {
        /// <summary>
        /// Уникальный идентификатор фотографии.
        /// </summary>
        public Guid ImageId { get; set; }
        /// <summary>
        /// Массив байтов, представляющий изображение.
        /// </summary>
        public byte[] Images { get; set; } = null!;

        /// <summary>
        /// Имя файла изображения.
        /// </summary>
        public string FileName { get; set; } = null!;

        /// <summary>
        /// Тип содержимого изображения (например, "image/jpeg").
        /// </summary>
        public string ContentType { get; set; } = null!;

        /// <summary>
        /// Идентификатор родительского элемента, если применимо.
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
