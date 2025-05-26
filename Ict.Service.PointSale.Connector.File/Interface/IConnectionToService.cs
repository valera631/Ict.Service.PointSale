using Ict.ApiProvider;
using Ict.Service.File.Models;

namespace Ict.Service.PointSale.Connector.File.Interface
{
    public interface IConnectionToService
    {
        /// <summary>
        /// Получение данных с сервера по запросу и пути
        /// </summary>
        /// <typeparam name="TResponse">класс ответа</typeparam>
        /// <typeparam name="TRequest">класс запроса</typeparam>
        /// <param name="item"> параметры запроса</param>
        /// <param name="apiType">Тип запроса</param>
        /// <param name="path">строка подключения</param>
        /// <returns></returns>
        Task<Response<TResponse>> GetData<TResponse, TRequest>(TRequest item, AppData.ApiType apiType, (string serviceUrl, string action) path);

        /// <summary>
        /// Получение данных с сервера по пути
        /// </summary>
        /// <typeparam name="TResponse">класс ответа</typeparam>       
        /// <param name="apiType">Тип запроса</param>
        /// <param name="path">строка подключения</param>
        /// <returns></returns>
        Task<Response<TResponse>> GetData<TResponse>(AppData.ApiType apiType, (string serviceUrl, string action) path);
    }
}
