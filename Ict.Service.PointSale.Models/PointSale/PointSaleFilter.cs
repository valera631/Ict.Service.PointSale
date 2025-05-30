using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.PointSale
{
    public class PointSaleFilter
    {

        /// <summary>
        /// Имя точки для фильтрации.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Указывает, нужно ли отфильтровать только подтверждённые точки.
        /// true — только подтверждённые,
        /// false — только неподтверждённые,
        /// null — не учитывать подтверждение.
        /// </summary>
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Указывает, нужно ли отфильтровать точки, у которых есть оператор.
        /// true — только с оператором,
        /// false — только без оператора,
        /// null — не учитывать наличие оператора.
        /// </summary>
        public bool? HasOperator { get; set; }

        /// <summary>
        /// Указывает, нужно ли отфильтровать точки, у которых есть филиалы.
        /// true — только с филиалами,
        /// false — только без филиалов,
        /// null — не учитывать наличие филиалов.
        /// </summary>
        public bool? HasNoBranches { get; set; }

        /// <summary>
        /// Страница для пагинации.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Количество элементов на странице для пагинации.
        /// </summary>
        public int PageSize { get; set; } = 9;
    }
}
