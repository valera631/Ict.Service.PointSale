using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.References
{
    /// <summary>
    /// Модель элемента справочника.
    /// </summary>
    public class LookupItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
