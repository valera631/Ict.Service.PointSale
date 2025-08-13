using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Photo;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Сервис для работы с фотографиями торговых точек.
    /// </summary>
    public interface IPhotoService 
    {
        /// <summary>
        /// Добавляет логотип к организации.
        /// </summary>
        Task<OperationResult<bool>> AddLogoAsync(PhotoContainerDto photoUploadDto);

        /// <summary>
        /// Получает логотип торговой точки по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<PointSalePhotoDto>> GetLogoAsync(Guid pointSaleId);

        /// <summary>
        /// Получает список превью фотографий торговой точки по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<List<PointSalePhotoDto>>> GetPreviewsAsync(Guid pointSaleId);

        /// <summary>
        /// Загружает фотографии в хранилище и связывает их с торговой точки.
        /// </summary>
        Task<OperationResult<bool>> AddPhotoAsync(PhotoContainerDto photoUploadDtos);

        /// <summary>
        /// Удаляет фотографии по идентификаторам.
        /// </summary>
        Task<OperationResult<List<Guid>>> DeletePhotoAsync(PhotosDeleteDto deletePhotosDto);
    }
}
