using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Таблица расписания работы торговой точки.
    /// </summary>
    public class PointSaleSchedule
    {
        [Key]
        public Guid PointSaleScheduleId {get; set; }
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// День недели, где 0 - воскресенье, 1 - понедельник и т.д.
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Флаг, указывающий, является ли день рабочим.
        /// </summary>
        public bool IsWorkingDay { get; set; }

        /// <summary>
        /// Время начала и окончания рабочего дня
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Время окончания рабочего дня
        /// </summary>
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// Время начала и окончания перерыва в работе торговой точки.
        /// </summary>
        public TimeSpan? BreakStartTime { get; set; }
        public TimeSpan? BreakEndTime { get; set; }

        [ForeignKey("PointSaleId")]
        public PointSaleEntity PointSale { get; set; }


    }
}
