using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
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

        Task<OperationResult<PointSaleResultFullDto>> GetPointSaleAsync(PointSaleDate pointSaleDate);

        /// <summary>
        /// Добовляет связь между торговой точки и оператором.
        /// </summary>
        Task<OperationResult<Guid?>> LinkOperatorAsync(LinkOperatorDto linkOperatorRequest);

        /// <summary>
        /// Асинхронно удаляет связь между организацией и оператором.
        /// </summary>
        /// <param name="unlinkOperatorRequest"></param>
        /// <returns></returns>
        Task<OperationResult<bool>> UnlinkOperatorAsync(OperatorUnlinkDto unlinkOperatorRequest);


        Task<OperationResult<PaginatedResult<PointSaleResultFullDto>>> GetPointSalesByFilterAsync(PointSaleFilter pointSaleFilter);
    }
}
