using Ict.ApiProvider;
using Ict.ApiProvider.Services.IServices;
using Ict.Service.File.Models;
using Ict.Service.PointSale.Connector.File.Interface;


namespace Ict.Service.PointSale.Connector.File.Action
{
    public class ConnectionToService(IApiService apiService) : IConnectionToService
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
        public async Task<Response<TResponse>> GetData<TResponse, TRequest>(TRequest item, AppData.ApiType apiType, (string serviceUrl, string action) path)
        {
            var param = new ApiRequest<TRequest>()
            {
                ApiType = apiType,
                Url = $"{path.serviceUrl}/{path.action}",
                Data = item,
            };
            var result = await apiService.SendAsync<TResponse, TRequest>(param);


            Response<TResponse> response = new();

            if (result.Success) response.Result = result.Result;
            else response.ErrorMessage = result.ErrorMessage;

            return response;
        }

        /// <summary>
        /// Получение данных с сервера по пути
        /// </summary>
        /// <typeparam name="TResponse">класс ответа</typeparam>       
        /// <param name="apiType">Тип запроса</param>
        /// <param name="path">строка подключения</param>
        /// <returns></returns>
        public async Task<Response<TResponse>> GetData<TResponse>(AppData.ApiType apiType, (string serviceUrl, string action) path)
        {
            var param = new ApiRequest()
            {
                ApiType = apiType,
                Url = $"{path.serviceUrl}/{path.action}",
            };

            var result = await apiService.SendAsync<TResponse>(param);

            Response<TResponse> response = new();

            if (result.Success) response.Result = result.Result;
            else response.ErrorMessage = result.ErrorMessage;

            return response;
        }
    }
}
