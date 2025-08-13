using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models.References
{
    /// <summary>
    /// DTO для типов точек продаж, содержащий списки справочных данных.
    /// </summary>
    public class PointSaleTypesDto
    {
        /// <summary>
        /// Список типов создания точки продаж.
        /// </summary>
        public List<LookupItemDto> CreationTypes { get; set; } = new();

        /// <summary>
        /// Список типов организаций.
        /// </summary>
        public List<LookupItemDto> OrganizationTypes { get; set; } = new();

        /// <summary>
        /// Список должностей руководителей.
        /// </summary>
        public List<LookupItemDto> ChiefPositions { get; set; } = new();

        /// <summary>
        /// Список типов собственников.
        /// </summary>
        public List<LookupItemDto> OwnerTypes { get; set; } = new();

        /// <summary>
        /// Список категорий точек продаж.
        /// </summary>
        public List<CategoryItem> Categories { get; set; } = new();
    }
}
