using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Core.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;

namespace Ict.Service.PointSale.Core.Services
{
    public class PointSaleSearch : IPointSaleSearch
    {
        private readonly IPointSaleRepository _pointSaleRepository;

        public PointSaleSearch(IPointSaleRepository pointSaleRepository)
        {
            _pointSaleRepository = pointSaleRepository;
        }

        public async Task<OperationResult<PaginatedResult<Guid>>> GetFilteredPointsSaleAsync(PointSaleFilter filter)
        {

            return await _pointSaleRepository.GetFilteredPointsSaleAsync(filter);
        }
    }
}
