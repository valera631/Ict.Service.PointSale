using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;

namespace Ict.Service.PointSale.Core.Interfaces
{
    public interface IPointSaleSearch
    {
        Task<OperationResult<PaginatedResult<Guid>>> GetFilteredPointsSaleAsync(PointSaleFilter filter);
    }
}
