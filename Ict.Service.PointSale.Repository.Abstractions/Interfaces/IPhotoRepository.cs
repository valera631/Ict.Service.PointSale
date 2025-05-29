using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    public interface IPhotoRepository
    {
        /// <summary>
        /// Добавляет логотип к торговой точке по идентификатору точки продаж и идентификатору логотипа.
        /// </summary>
        Task<OperationResult<bool>> AddLogoAsync(Guid pointSaleId, Guid logoId);


        /// <summary>
        /// Получает логотип торговой точки по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<Guid>> GetLogoAsync(Guid pointSaleId);


        /// <summary>
        /// Получает список превью фотографий торговой точки по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<List<Guid>>> GetPreviewsAsync(Guid pointSaleId);


        /// <summary>
        /// Загружает фотографии в хранилище и связывает их с торговой точкой.
        /// </summary>
        Task<OperationResult<bool>> AddPhotoAsync(Guid pointSaleId, List<Guid?> photoId);


        Task<OperationResult<List<Guid>>> GetPhotosByIdsAsync(Guid pointSaleId, List<Guid> photoIds);


        Task<OperationResult<List<Guid>>> DeletePhotosAsync(Guid pointSaleId, List<Guid> photoIds);

    }
}
