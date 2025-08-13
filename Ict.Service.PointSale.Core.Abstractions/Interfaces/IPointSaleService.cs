using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Models.Update;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Сервис для работы с точками продаж.
    /// </summary>
    public interface IPointSaleService
    {
        /// <summary>
        /// Создает новую точку продаж.
        /// </summary>
        Task<OperationResult<Guid>> CreatePointSaleAsync(PointSaleFullDto pointSaleFullDto);

        /// <summary>
        /// Асинхронно получает точку продаж по идентификатору владельца.
        /// </summary>
        Task<OperationResult<List<PointSaleResultFullDto>>> GetPointSaleByOwnerIdAsync(Guid ownerId);

        /// <summary>
        /// Асинхронно получает точку продаж по идентификатору оператора.
        /// </summary>
        Task<OperationResult<List<PointSaleResultFullDto>>> GetByOperatorAsync(Guid operatorId);

        /// <summary>
        /// Асинхронно получает точку продаж по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<PointSaleFullDto>> GetPointSaleDtoAsync(PointSaleDate pointSaleDate);

        /// <summary>
        /// Асинхронно получает полную информацию о точке продаж по индентификатору и дате.
        /// </summary>
        Task<OperationResult<PointSaleResultFullDto>> GetPointSaleAsync(PointSaleDate pointSaleDate);


        /// <summary>
        /// Добовляет связь между торговой точки и оператором.
        /// </summary>
        Task<OperationResult<Guid?>> LinkOperatorAsync(LinkOperatorDto linkOperatorRequest);

        /// <summary>
        /// Асинхронно удаляет связь между организацией и оператором.
        /// </summary>
        Task<OperationResult<bool>> UnlinkOperatorAsync(OperatorUnlinkDto unlinkOperatorRequest);


        /// <summary>
        /// Асинхронно получает точеки продаж по фильтру.
        /// </summary>
        Task<OperationResult<PaginatedResult<PointSaleResultFullDto>>> GetPointSalesByFilterAsync(PointSaleFilter pointSaleFilter);

        /// <summary>
        /// Асинхронно получает количество точек продаж.
        /// </summary>
        Task<OperationResult<int>> GetCountPointSalesAsync();

        /// <summary>
        /// Асинхронно передает право собственности на точку продаж другому владельцу.
        /// </summary>
        Task<OperationResult<bool>> TransferOwnershipAsync(TransferOwnershipDto transferOwnershipDto);

        /// <summary>
        /// Асинхронно получает количество точек продаж по идентификаторам владельцев.
        /// </summary>
        Task<OperationResult<List<PointSaleCounts>>> GetCountsByOwnersIdAsync(List<Guid> ownerIds);

        /// <summary>
        /// Обновляет имя точки продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdatePointSaleNameAsync(PointSaleNameUpdateDto pointSaleNameUpdateDto);


        /// <summary>
        /// Обновляет дату открытия точки продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdateCreationDateAsync(CreationDateUpdateDto pointSaleOpenDateUpdateDto);


        /// <summary>
        /// Обновляет рабочий график точки продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdateWorkScheduleAsync(WorkScheduleUpdateDto pointSaleWorkScheduleUpdateDto);

        /// <summary>
        /// Обновляет категории точек продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdateCategoriesAsync(CategoriesUpdateDto categoriesUpdate);

        /// <summary>
        /// Удаляет владельца точки продаж.
        /// </summary>
        Task<OperationResult<bool>> DeleteOwnerAsync(DeleteOwnerDto deleteOwner);

        /// <summary>
        /// Асинхронно удаляет точку продаж по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<bool>> DeletePointSaleAsync(Guid pointSaleId);

        /// <summary>
        /// Асинхронно отправляет запрос на верификацию точки продаж.
        /// </summary>
        Task<OperationResult<List<Guid>>> SubmitVerificationAsync(Guid pointSaleId);

        /// <summary>
        /// Асинхронно подтверждает точку продаж.
        /// </summary>
        Task<OperationResult<bool>> ConfirmIsApprovedAsync(PoinrSaleIsApprovedDto poinrSaleIsApproved);

        /// <summary>
        /// Закрывает точку продаж.
        /// </summary>
        Task<OperationResult<bool>> ClosePointSaleAsync(PointSaleCloseDto pointSaleCloseDto);
    }
}
