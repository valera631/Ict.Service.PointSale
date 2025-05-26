
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.References;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    public interface IReferencesRepository
    {
        Task<OperationResult<List<LookupItemDto>>> GetCreationTypesAsync();
        Task<OperationResult<List<LookupItemDto>>> GetClosingStatusesAsync();
        Task<OperationResult<List<LookupItemDto>>> GetOrganizationTypesAsync();
        Task<OperationResult<List<LookupItemDto>>> GetChiefPositionTypesAsync();
        Task<OperationResult<List<LookupItemDto>>> GetOwnerTypesAsync();
    }
}
