using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Models.Update;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с точками продаж.
    /// </summary>
    public interface IPointSaleRepository
    {
        /// <summary>
        /// Асинхронно создает новую точку продаж на основе предоставленных данных.
        /// </summary>
        /// <param name="pointSaleDto">Объект DTO, содержащий полные данные для создания точки продаж.</param>
        /// <returns>Результат операции с подтверждением успешного создания (true) или ошибки (false).</returns>
        Task<OperationResult<bool>> CreatePointSale(PointSaleFullDto pointSaleDto);

        /// <summary>
        /// Асинхронно получает инденфикаторы точек продаж по идентификатору владельца.
        /// </summary>
        Task<OperationResult<List<PointSaleResultFullDto>>> GetPointSaleByOwnerIdAsync(Guid ownerId);
      
        /// <summary>
        /// Асинхронно получает точки продаж по идентификатору оператора.
        /// </summary>
        Task<OperationResult<List<PointSaleResultFullDto>>> GetByOperatorAsync(Guid operatorId);

        /// <summary>
        /// Добовляет связь между торговой точкой и оператором.
        /// </summary>
        Task<OperationResult<Guid?>> AddOperatorToPointSaleAsync(LinkOperatorDto linkOperatorDto);

        /// <summary>
        /// Асинхронно удаляет связь между торговой точкой и оператором.
        /// </summary>
        Task<OperationResult<bool>> UnlinkOperatorAsync(OperatorUnlinkDto unlinkOperatorDto);

        /// <summary>
        /// Асинхронно получает точки продаж.
        /// </summary>
        Task<OperationResult<PointSaleResultFullDto>> GetPointSaleByIdAsync(Guid pointSaleId ,DateOnly? dateOnly);

        /// <summary>
        /// Асинхронно получает точку продаж в виде модели PointSaleFullDto.
        /// </summary>
        Task<OperationResult<PointSaleFullDto>> GetPointSaleDtoAsync(Guid pointSaleId, DateOnly? dateOnly);


        /// <summary>
        /// Получает индентификаторы точек продаж по фильтру.
        /// </summary>
        Task<OperationResult<PaginatedResult<Guid>>> GetFilteredPointsSaleAsync(PointSaleFilter filter);

        /// <summary>
        /// Асинхронно передает право собственности на точку продаж другому владельцу.
        /// </summary>
        Task<OperationResult<bool>> TransferOwnershipAsync(TransferOwnershipDto transferOwnershipDto);

        /// <summary>
        /// Асинхронно получает количество точек продаж
        /// </summary>
        Task<OperationResult<int>> GetCountPointSalesAsync();

        /// <summary>
        /// Асинхронно получает список счетчиков (PointSaleCounts) для заданных идентификаторов владельцев.
        /// </summary>
        Task<OperationResult<List<PointSaleCounts>>> GetCountsByOwnersIdAsync(List<Guid> ownerIds);

        /// <summary>
        /// Асинхронно обновляет название точки продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdatePointSaleNameAsync(PointSaleNameUpdateDto pointSaleNameUpdateDto);

        /// <summary>
        /// Асинхронно обновляет дату открытия точки продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdateCreationDateAsync(CreationDateUpdateDto pointSaleOpenDateUpdateDto);

        /// <summary>
        /// Асинхронно обновляет рабочий график точки продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdateWorkScheduleAsync(WorkScheduleUpdateDto workScheduleUpdate);

        /// <summary>
        /// Асинхронно обновляет категории в точке продаж.
        /// </summary>
        Task<OperationResult<bool>> UpdateCategoriesAsync(CategoriesUpdateDto categoriesUpdate);

        /// <summary>
        /// Асинхронно удаляет владельца точки продаж.
        /// </summary>
        Task<OperationResult<bool>> DeleteOwnerAsync(DeleteOwnerDto deleteOwnerDto);

        /// <summary>
        /// Добавляет индентификатор точеки продажи для верификации и возвращает id администраторов, которые должны подтвердить верификацию.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task<OperationResult<List<Guid>>> SubmitVerificationAsync(Guid pointSaleId);

        /// <summary>
        /// Асинхронно подтверждает точку продажи как верифицированную.
        /// </summary>
        Task<OperationResult<bool>> ConfirmIsApprovedAsync(PoinrSaleIsApprovedDto poinrSaleIsApprovedDto);

        /// <summary>
        /// Асинхронно удаляет точку продаж по её идентификатору.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор точки продаж, которую нужно удалить.</param>
        /// <returns>Результат операции с булевым значением, указывающим успешность удаления.</returns>
        Task<OperationResult<bool>> DeletePointSaleAsync(Guid pointSaleId);

        /// <summary>
        /// Асинхронно закрывает точку продаж на основании данных DTO.
        /// </summary>
        /// <param name="pointSaleCloseDto">Объект с данными для закрытия точки продаж.</param>
        /// <returns>Результат операции с булевым значением, указывающим успешность закрытия.</returns>
        Task<OperationResult<bool>> ClosePointSaleAsync(PointSaleCloseDto pointSaleCloseDto);

    }
}
