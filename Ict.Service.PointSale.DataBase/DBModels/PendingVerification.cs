using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Класс, представляющий ожидающие проверки магазины.
    /// </summary>
    public class PendingVerification
    {
        /// <summary>
        /// Уникальный идентификатор ожидающей проверки магазины.
        /// </summary>
        [Key]
        public Guid PointSaleId { get; set; }
    }
}
