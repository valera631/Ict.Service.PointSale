using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.References
{
    /// <summary>
    /// DTO для элемента справочника (lookup).
    /// </summary>
    public class LookupItemDto
    {
        /// <summary>
        /// Уникальный идентификатор элемента.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование элемента.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
