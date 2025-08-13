using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Models.Update;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Сервис для работы с описанием торговой точки.
    /// </summary>
    public interface IDescriptionService
    {
        /// <summary>
        /// Асинхронно обновляет описание торговой точки.
        /// </summary>
        Task<OperationResult<bool>> UpdateDescriptionAsync(DescriptionUpdateDto descriptionChange);
    }
}
