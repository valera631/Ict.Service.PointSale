
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.References;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с справочными данными, таких как типы создания, статусы закрытия, типы организаций и другие справочники.
    /// </summary>
    public interface IReferencesRepository
    {
        /// <summary>
        /// Асинхронно получает список типов создания (Creation Types).
        /// </summary>
        /// <returns>Результат операции со списком элементов справочника.</returns>
        Task<OperationResult<List<LookupItemDto>>> GetCreationTypesAsync();

        /// <summary>
        /// Асинхронно получает список статусов закрытия (Closing Statuses).
        /// </summary>
        /// <returns>Результат операции со списком элементов справочника.</returns>
        Task<OperationResult<List<LookupItemDto>>> GetClosingStatusesAsync();

        /// <summary>
        /// Асинхронно получает список типов организаций.
        /// </summary>
        /// <returns>Результат операции со списком элементов справочника.</returns>
        Task<OperationResult<List<LookupItemDto>>> GetOrganizationTypesAsync();

        /// <summary>
        /// Асинхронно получает список типов должностей руководителей.
        /// </summary>
        /// <returns>Результат операции со списком элементов справочника.</returns>
        Task<OperationResult<List<LookupItemDto>>> GetChiefPositionTypesAsync();

        /// <summary>
        /// Асинхронно получает список типов владельцев.
        /// </summary>
        /// <returns>Результат операции со списком элементов справочника.</returns>
        Task<OperationResult<List<LookupItemDto>>> GetOwnerTypesAsync();

        /// <summary>
        /// Асинхронно получает список категорий.
        /// </summary>
        /// <returns>Результат операции со списком категорий.</returns>
        Task<OperationResult<List<CategoryItem>>> GetCategoriesAsync();
    }
}
