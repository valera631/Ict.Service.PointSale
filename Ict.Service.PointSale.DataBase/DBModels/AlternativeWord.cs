using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.DataBase.DBModels
{
    /// <summary>
    /// Класс, представляющий альтернативные слова для точек продаж.
    /// </summary>
    public class AlternativeWord
    {
        /// <summary>
        /// Уникальный идентификатор альтернативного слова.
        /// </summary>
        public Guid AlternativeWordId { get; set; }

        /// <summary>
        /// Идентификатор точки продаж, к которой относится это альтернативное слово.
        /// </summary>
        public Guid PointSaleId { get; set; }

        /// <summary>
        /// Имя альтернативного слова, которое может использоваться для поиска или идентификации точки продаж.
        /// </summary>
        public string AlternativeWordName { get; set; } = null!;


        /// <summary>
        /// Навигационное свойство для связи с точкой продаж.
        /// </summary>
        [ForeignKey("PointSaleId")]
        public PointSaleEntity PointSale { get; set; } 


    }
}
