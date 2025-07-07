using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Models.Chief;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Models.Location;

namespace Ict.Service.PointSale.Models.PointSale
{
    public class PointSaleFullDto
    {
        public PointSaleDto PointSale { get; set; } = null!;

        public PointSaleActivityDto PointSaleActivity { get; set; } = null!;

        public DescriptionDto Description { get; set; }

        public ChiefDto Chief { get; set; }

        public LocationDto Location { get; set; }

        public List<PointSaleScheduleDto> PointSaleSchedules { get; set; } = new List<PointSaleScheduleDto>();

        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<string> AlternativeName { get; set; } = new List<string>();
    }
}
