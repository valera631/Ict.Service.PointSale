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
    /// <summary>
    /// Полная информация о точке продаж.
    /// </summary>
    public class PointSaleFullDto
    {
        /// <summary>
        /// Основная информация о точке продаж.
        /// </summary>
        public PointSaleDto PointSale { get; set; } = null!;

        /// <summary>
        /// Информация об активности точки продаж.
        /// </summary>
        public PointSaleActivityDto PointSaleActivity { get; set; } = null!;

        /// <summary>
        /// Описание точки продаж.
        /// </summary>
        public DescriptionDto? Description { get; set; }

        /// <summary>
        /// Информация о руководителе точки продаж.
        /// </summary>
        public ChiefDto? Chief { get; set; }

        /// <summary>
        /// Адрес и геолокация точки продаж.
        /// </summary>
        public LocationDto? Location { get; set; }

        /// <summary>
        /// Расписание работы точки продаж.
        /// </summary>
        public List<PointSaleScheduleDto> PointSaleSchedules { get; set; } = new List<PointSaleScheduleDto>();

        /// <summary>
        /// Идентификаторы категорий, к которым относится точка продаж.
        /// </summary>
        public List<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// Альтернативные названия точки продаж.
        /// </summary>
        public List<string> AlternativeName { get; set; } = new List<string>();
    }
}
