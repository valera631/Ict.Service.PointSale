using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.API.Abstractions.Models.References
{
    /// <summary>
    /// Модель ответа с типами для точек продаж.
    /// </summary>
    public class PointSaleTypesResponse
    {
        /// <summary>
        /// Список типов создания точек продаж.
        /// </summary>
        public List<LookupItemResponse> CreationTypes { get; set; } = new();

        /// <summary>
        /// Список типов точек продаж.
        /// </summary>
        public List<LookupItemResponse> OrganizationTypes { get; set; } = new();

        /// <summary>
        /// Список типов позиций руководителей.
        /// </summary>
        public List<LookupItemResponse> ChiefPositions { get; set; } = new();

        /// <summary>
        /// Список типов владельцев точек продаж.
        /// </summary>
        public List<LookupItemResponse> OwnerTypes { get; set; } = new();

        /// <summary>
        /// Список категорий точек продаж.
        /// </summary>
        public List<CategoryItemResponse> Categories { get; set; } = new();
    }
}
