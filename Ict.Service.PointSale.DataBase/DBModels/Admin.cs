using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Сущность администратора.
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// Уникальный идентификатор администратора.
        /// </summary>
        [Key]
        public Guid AdminId { get; set; }

    }
}
