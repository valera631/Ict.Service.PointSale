using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Photo;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с фотографиями торговых точек.
    /// </summary>
    public interface IPhotoRepository
    {
        /// <summary>
        /// Добавляет логотип к торговой точке по идентификатору точки продаж и идентификатору логотипа.
        /// </summary>
        Task<OperationResult<bool>> AddLogoAsync(Guid pointSaleId, Guid logoId, DateOnly OpenDateLogo);


        /// <summary>
        /// Получает актуальный логотип торговой точки по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<LogoDto>> GetLogoAsync(Guid pointSaleId);


        /// <summary>
        /// Получает список превью фотографий торговой точки по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<List<Guid>>> GetPreviewsAsync(Guid pointSaleId);


        /// <summary>
        /// Загружает фотографии в хранилище и связывает их с торговой точкой.
        /// </summary>
        Task<OperationResult<bool>> AddPhotoAsync(Guid pointSaleId, List<Guid?> photoId);

        /// <summary>
        /// Получает список фотографий торговой точки по идентификатору точки продаж и предпологаемых ее фотогофий(id).
        /// </summary>
        Task<OperationResult<List<Guid>>> GetPhotosByIdsAsync(Guid pointSaleId, List<Guid> photoIds);

        /// <summary>
        /// Удаляет фотографии торговой точки по идентификатору точки продаж и списку идентификаторов фотографий.
        /// </summary>
        Task<OperationResult<List<Guid>>> DeletePhotosAsync(Guid pointSaleId, List<Guid> photoIds);

        /// <summary>
        /// Получает идентификаторы логотипов торговой точки по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<List<Guid>>> GetLogoIdAsync(Guid pointSaleId);

    }
}
