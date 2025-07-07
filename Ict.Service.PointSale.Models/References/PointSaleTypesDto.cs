using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.References
{
    public class PointSaleTypesDto
    {
        public List<LookupItemDto> CreationTypes { get; set; } = new();
        public List<LookupItemDto> OrganizationTypes { get; set; } = new();
        public List<LookupItemDto> ChiefPositions { get; set; } = new();
        public List<LookupItemDto> OwnerTypes { get; set; } = new();


        public List<CategoryItem> Categories { get; set; } = new();
    }
}
