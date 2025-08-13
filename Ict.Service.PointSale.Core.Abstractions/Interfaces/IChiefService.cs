using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Update;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Интерфейс для управления данными руководителя торговой точки.
    /// </summary>
    public interface IChiefService
    {
        /// <summary>
        /// Асинхронно обновляет данные существующего руководителя.
        /// </summary>
        /// <param name="chiefUpdate">Объект, содержащий обновленные данные руководителя.</param>
        Task<OperationResult<bool>> UpdateChiefAsync(ChiefUpdateDto chiefUpdate);
    }
}
