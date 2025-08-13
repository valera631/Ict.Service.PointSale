using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.API.Abstractions.Models.Schedule;

namespace Ict.Service.PointSale.API.Abstractions.Models.Update
{
    /// <summary>
    /// Модель запроса для обновления расписания работы точки продажи.
    /// </summary>
    public class PointSaleWorkScheduleUpdateRequest
    {
        /// <summary>
        /// Уникальный идентификатор точки продажи, для которой обновляется расписание.
        /// </summary>
        [Required]
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Список расписаний работы, которые необходимо обновить для точки продажи.
        /// </summary>
        [Required]
        public List<WorkScheduleRequest> WorkSchedule { get; set; } = new();
    }
}
