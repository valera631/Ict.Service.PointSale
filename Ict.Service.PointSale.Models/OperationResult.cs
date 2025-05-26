using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ict.Service.PointSale.Models
{
    /// <summary>
    /// Класс для представления результата операции с данными.
    /// </summary>
    /// <typeparam name="T">Тип данных, возвращаемых в результате операции.</typeparam>
    public class OperationResult<T>
    {
        /// <summary>
        /// Указывает, была ли операция успешной.
        /// </summary>
        public bool IsSuccess => ErrorMessage == null;

        /// <summary>
        /// Данные, возвращенные в результате операции (может быть null в случае ошибки).
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Сообщение об ошибке, если операция не удалась (может быть null, если ошибок нет).
        /// </summary>
        public string? ErrorMessage { get; set; }

    }
}
