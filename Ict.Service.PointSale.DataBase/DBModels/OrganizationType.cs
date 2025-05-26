using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    public class OrganizationType
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        [Key]
        public int OrganizationTypeId { get; set; }

        /// <summary>
        /// тип организации.
        /// </summary>
        public string NameType { get; set; } = null!;


        /// <summary>
        /// Навигационное свойство для связи с таблицей точек продаж.
        /// </summary>
        public virtual ICollection<PointSaleEntity> PointSales { get; set; } = new List<PointSaleEntity>();
    }
}
