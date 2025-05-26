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
        Task<OperationResult<bool>> AddLogoAsync(Guid pointSaleId, Guid logoId);

        Task<OperationResult<Guid>> GetLogoAsync(Guid pointSaleId);

        Task<OperationResult<List<Guid>>> GetPreviewsAsync(Guid pointSaleId);

        Task<OperationResult<bool>> AddPhotoAsync(Guid pointSaleId, List<Guid?> photoId);
    }
}
