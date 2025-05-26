using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Description;

namespace Ict.Service.PointSale.Core.Abstractions.Interfaces
{
    public interface IDescriptionService
    {
        /// <summary>
        /// Асинхронно обновляет описание организации по идентификатору точки продаж.
        /// </summary>
        Task<OperationResult<Guid>> ChangeDescriptionAsync(DescriptionChangeDto descriptionChange);


        Task<OperationResult<Guid>> UpdateDescriptionAsync(DescriptionChangeDto descriptionChange);
    }
}
