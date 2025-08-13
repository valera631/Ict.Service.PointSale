using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models
{
    /// <summary>
    /// Модель для представления пагинированных данных.
    /// Содержит список элементов текущей страницы и метаданные пагинации.
    /// </summary>
    /// <typeparam name="T">Тип элементов в списке (например, Guid или OrganizationResultFull).</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Список элементов текущей страницы.
        /// Инициализируется пустым списком, чтобы избежать null-ошибок.
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();


        /// <summary>
        /// Общее количество элементов, соответствующих фильтру, во всех страницах.
        /// Используется для расчета общего количества страниц.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Номер текущей страницы (начинается с 1).
        /// Указывает, какая страница запрошена.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Количество элементов на одной странице.
        /// Определяет размер страницы (например, 9 для отображения 9 организаций).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Общее количество страниц.
        /// Вычисляется как потолок от деления TotalCount на PageSize.
        /// Если PageSize равен 0, возвращается 0 для предотвращения деления на ноль.
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

        /// <summary>
        /// Указывает, есть ли предыдущая страница.
        /// Возвращает true, если текущая страница больше 1.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Указывает, есть ли следующая страница.
        /// Возвращает true, если текущая страница меньше общего количества страниц.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
