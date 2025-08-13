using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Schedule
{
    /// <summary>
    /// Модель запроса для расписания.
    /// </summary>
    public class WorkScheduleRequest
    {
        /// <summary>
        /// Идентификатор рабочего расписания.
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Флаг, указывающий, является ли день рабочим.
        /// </summary>
        public bool IsWorkingDay { get; set; }

        /// <summary>
        /// Время начала и окончания рабочего дня, а также время начала и окончания перерыва.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Время окончания рабочего дня.
        /// </summary>
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// Время начала и окончания перерыва в рабочем дне.
        /// </summary>
        public TimeSpan? BreakStartTime { get; set; }
        public TimeSpan? BreakEndTime { get; set; }
    }
}
