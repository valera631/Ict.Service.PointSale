using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.References
{
    public class CategoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Path { get; set; } = null!;
    }
}
