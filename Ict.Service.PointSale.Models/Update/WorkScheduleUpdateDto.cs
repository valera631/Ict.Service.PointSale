using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Update
{
    /// <summary>
    /// DTO для обновления расписания работы торговой точки.
    /// </summary>
    public class WorkScheduleUpdateDto
    {
        /// <summary>
        /// Уникальный идентификатор торговой точки, для которой обновляется расписание.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Список рабочих дней и времени работы торговой точки.
        /// </summary>
        public List<PointSaleScheduleDto> WorkSchedule { get; set; } = new List<PointSaleScheduleDto>();
    }
}
