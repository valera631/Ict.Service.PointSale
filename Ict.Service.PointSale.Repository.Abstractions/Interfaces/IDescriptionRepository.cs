using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Description;

namespace Ict.Service.PointSale.Repository.Abstractions.Interfaces
{
    public interface IDescriptionRepository
    {

        /// <summary>
        /// Изменяет описание организации в базе данных.
        /// </summary>
        /// <param name="descriptionChangeDto">DTO с данными для изменения описания.</param>
        /// <returns>Результат операции с уникальным идентификатором измененной организации.</returns>
        Task<OperationResult<Guid>> ChangeDescriptionAsync(DescriptionChangeDto descriptionChangeDto);


        Task<OperationResult<Guid>> UpdateDescriptionAsync(DescriptionChangeDto descriptionChange);
    }
}
