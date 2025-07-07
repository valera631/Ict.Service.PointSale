using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.References
{
    public class CategoryItemResponse
    {
        /// <summary>
        /// id категории.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; set; } = null!;


        /// <summary>
        /// Путь к категории. 
        /// </summary>
        public string Path { get; set; } = null!;
    }
}
