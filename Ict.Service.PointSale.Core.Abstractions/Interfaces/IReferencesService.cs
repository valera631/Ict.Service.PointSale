using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.References;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    public interface IReferencesService
    {
        Task<OperationResult<PointSaleTypesDto>> GetAllTypesAsync();
    }
}
