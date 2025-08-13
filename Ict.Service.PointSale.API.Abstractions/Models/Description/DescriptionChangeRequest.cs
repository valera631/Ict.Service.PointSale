using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.Description
{

    /// <summary>
    /// Запрос на изменение описания торговой точки
    /// </summary>
    public class DescriptionChangeRequest
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
