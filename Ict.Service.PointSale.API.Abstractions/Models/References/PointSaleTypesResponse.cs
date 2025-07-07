using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.References
{
    public class PointSaleTypesResponse
    {
        public List<LookupItemResponse> CreationTypes { get; set; } = new();
        public List<LookupItemResponse> OrganizationTypes { get; set; } = new();
        public List<LookupItemResponse> ChiefPositions { get; set; } = new();
        public List<LookupItemResponse> OwnerTypes { get; set; } = new();


        public List<CategoryItemResponse> Categories { get; set; } = new();
    }
}
