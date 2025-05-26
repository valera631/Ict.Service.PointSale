using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Photo;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    public interface IPhotoService 
    {
        /// <summary>
        /// Добавляет логотип к организации.
        /// </summary>
        Task<OperationResult<bool>> AddLogoAsync(PhotoContainerDto photoUploadDto);

        Task<OperationResult<PointSalePhotoDto>> GetLogoAsync(Guid pointSaleId);


        Task<OperationResult<List<PointSalePhotoDto>>> GetPreviewsAsync(Guid pointSaleId);

        /// <summary>
        /// Загружает фотографии в хранилище и связывает их с торговой точки.
        /// </summary>
        Task<OperationResult<bool>> AddPhotoAsync(PhotoContainerDto photoUploadDtos);
    }
}
