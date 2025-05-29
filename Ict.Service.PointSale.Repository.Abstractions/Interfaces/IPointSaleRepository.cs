using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
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
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<OperationResult<List<PointSaleResultFullDto>>> GetPointSaleByOwnerIdAsync(Guid ownerId);


        Task<OperationResult<Guid?>> AddOperatorToPointSaleAsync(LinkOperatorDto linkOperatorDto);

        Task<OperationResult<bool>> UnlinkOperatorAsync(OperatorUnlinkDto unlinkOperatorDto);

        Task<OperationResult<PointSaleResultFullDto>> GetPointSaleByIdAsync(Guid pointSaleId ,DateOnly? dateOnly);


    }
}
