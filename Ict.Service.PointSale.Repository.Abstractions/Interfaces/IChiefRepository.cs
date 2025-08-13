using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Update;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с данными руководителя.
    /// </summary>
    public interface IChiefRepository
    {
        /// <summary>
        /// Асинхронно обновляет данные руководителя.
        /// </summary>
        Task<OperationResult<bool>> UpdateAsync(ChiefUpdateDto chiefUpdate);
    }
}
