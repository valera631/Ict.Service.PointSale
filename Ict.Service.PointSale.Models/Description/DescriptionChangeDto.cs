using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.Description
{
    public class DescriptionChangeDto
    {
        /// <summary>
        /// Уникальный идентификатор торговой точки
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Новый текст описания
        /// </summary>
        public string? DescriptionText { get; set; }
    }
}
