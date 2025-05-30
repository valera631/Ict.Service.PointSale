using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Core.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;

namespace Ict.Service.PointSale.Core.Services
{
    public class PointSaleService : IPointSaleService
    {

        private readonly IPointSaleRepository _pointSaleRepository;
        private readonly IPointSaleSearch _pointSaleSearch;

        public PointSaleService(IPointSaleRepository pointSaleRepository, IPointSaleSearch pointSaleSearch)
        {
            _pointSaleRepository = pointSaleRepository;
            _pointSaleSearch = pointSaleSearch;
        }

        public async Task<OperationResult<Guid>> CreatePointSaleAsync(PointSaleFullDto pointSaleFullDto)
        {

            OperationResult<Guid> response = new();

            try
            {
                // Валидация данных 
                var validationResult = await ValidateCreationDates(pointSaleFullDto);

                if (!validationResult.IsSuccess)
                {
                    response.ErrorMessage = validationResult.ErrorMessage;
                    return response;
                }

                var result = await _pointSaleRepository.CreatePointSale(pointSaleFullDto);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<PointSaleResultFullDto>> GetPointSaleAsync(PointSaleDate pointSaleDate)
        {
            OperationResult<PointSaleResultFullDto> response = new();
            try
            {
                var result = await _pointSaleRepository.GetPointSaleByIdAsync(pointSaleDate.PointSaleId, pointSaleDate.OpenDate);
                if (result.Data == null)
                {
                    return response;
                }
                response.Data = result.Data;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Асинхронно получает точку продаж по идентификатору владельца.
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<OperationResult<List<PointSaleResultFullDto>>> GetPointSaleByOwnerIdAsync(Guid ownerId)
        {
            var response = new OperationResult<List<PointSaleResultFullDto>>();
            try
            {
                var result = await _pointSaleRepository.GetPointSaleByOwnerIdAsync(ownerId);

                if (result.Data == null)
                {
                    return response;
                }

                response.Data = result.Data;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<OperationResult<PaginatedResult<PointSaleResultFullDto>>> GetPointSalesByFilterAsync(PointSaleFilter filter)
        {
            var response = new OperationResult<PaginatedResult<PointSaleResultFullDto>>();
            var result = new PaginatedResult<PointSaleResultFullDto>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
            try
            {
                var pointSaleIdsResult = await _pointSaleSearch.GetFilteredPointsSaleAsync(filter);


                if (!pointSaleIdsResult.IsSuccess)
                {
                    response.ErrorMessage = pointSaleIdsResult.ErrorMessage ?? "Ошибка при получении данных организаций.";
                    return response;
                }

                // Если нет организаций, возвращаем успешный результат с пустым списком
                if (pointSaleIdsResult.Data?.Items == null || !pointSaleIdsResult.Data.Items.Any())
                {
                    result.Items = new List<PointSaleResultFullDto>();
                    response.Data = result;
                    return response;
                }

                result.TotalCount = pointSaleIdsResult.Data.TotalCount;

                // Список для хранения результатов по каждой организации
                var pointSaleResults = new List<PointSaleResultFullDto>();

                foreach (var pointSaleId in pointSaleIdsResult.Data.Items)
                {
                    // Получаем полную информацию о точке продаж по идентификатору
                    var pointSaleResult = await _pointSaleRepository.GetPointSaleByIdAsync(pointSaleId, null);
                    if (pointSaleResult.IsSuccess && pointSaleResult.Data != null)
                    {
                        pointSaleResults.Add(pointSaleResult.Data);
                    }
                }

                result.Items = pointSaleResults;
                response.Data = result;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

        }

        public async Task<OperationResult<Guid?>> LinkOperatorAsync(LinkOperatorDto linkOperatorRequest)
        {
            OperationResult<Guid?> response = new();
            try
            {

                var addResult = await _pointSaleRepository.AddOperatorToPointSaleAsync(linkOperatorRequest);

                if (!addResult.IsSuccess)
                {
                    response.ErrorMessage = "Не удалось добавить связь: организация или оператор не найдены.";
                    return response;
                }

                response.Data = addResult.Data;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<bool>> UnlinkOperatorAsync(OperatorUnlinkDto unlinkOperatorRequest)
        {

            OperationResult<bool> response = new();
            try
            {
                var removeResult = await _pointSaleRepository.UnlinkOperatorAsync(unlinkOperatorRequest);

                if (!removeResult.IsSuccess)
                {
                    response.ErrorMessage = "Не удалось удалить связь: организация или оператор не найдены.";
                    return response;
                }

                response.Data = removeResult.Data;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
           return response;
        }



        /// <summary>
        /// Асинхронно проверяет корректность дат создания точки продаж относительно руководителя, местоположения и организации.
        /// </summary>
        /// <param name="pointSale">Объект DTO точки продаж для валидации.</param>
        /// <returns>Результат операции с подтверждением корректности (true) или сообщением об ошибке.</returns>
        private async Task<OperationResult<bool>> ValidateCreationDates(PointSaleFullDto pointSale)
        {
            var response = new OperationResult<bool>();
            var pointSaleOpenDate = pointSale.PointSaleActivity.OpenDate;

            if (pointSale.Chief.OpenDate < pointSaleOpenDate)
            {
                response.ErrorMessage = "Дата создания руководителя не может быть раньше даты создания торговой точки.";
                return response;
            }

            if (pointSale.Location.OpenDate < pointSaleOpenDate)
            {
                response.ErrorMessage = "Дата создания локации не может быть раньше даты создания торговой точки.";
                return response;
            }

            response.Data = true;
            return response;
        }
    }

}
