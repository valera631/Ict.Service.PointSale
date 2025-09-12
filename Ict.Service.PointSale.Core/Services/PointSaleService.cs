using Ict.Provider.Service.File.Interface;
using Ict.Service.File.Models.File;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Core.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Models.Update;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;

namespace Ict.Service.PointSale.Core.Services
{

    /// <summary>
    /// Сервис для управления точками продаж.
    /// </summary>
    public class PointSaleService : IPointSaleService
    {

        private readonly IPointSaleRepository _pointSaleRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IFileProvider _fileConnected;
        private readonly IPointSaleSearch _pointSaleSearch;

        /// <summary>
        /// Создаёт новый экземпляр сервиса точек продаж с заданными зависимостями.
        /// </summary>
        /// <param name="pointSaleRepository">Репозиторий для доступа к данным точек продаж.</param>
        /// <param name="pointSaleSearch">Сервис для поиска и фильтрации точек продаж.</param>
        /// <param name="photoRepository">Репозиторий для работы с фотографиями точек продаж.</param>
        /// <param name="fileConnected">Сервис для взаимодействия с файловым хранилищем.</param>
        public PointSaleService(IPointSaleRepository pointSaleRepository, IPointSaleSearch pointSaleSearch, IPhotoRepository photoRepository, IFileProvider fileConnected)
        {
            _pointSaleRepository = pointSaleRepository;
            _pointSaleSearch = pointSaleSearch;
            _photoRepository = photoRepository;
            _fileConnected = fileConnected;
        }


        /// <summary>
        /// Асинхронно создает новую точку продаж.
        /// </summary>
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
                if (!result.IsSuccess)
                {
                    response.ErrorMessage = result.ErrorMessage ?? "Не удалось создать точку продаж.";
                    return response;
                }
                response.Data = pointSaleFullDto.PointSale.PointSaleId;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }


        /// <summary>
        /// Асинхронно получает точки продаж по идентификатору оператора.
        /// </summary>
        public async Task<OperationResult<List<PointSaleResultFullDto>>> GetByOperatorAsync(Guid operatorId)
        {
            OperationResult<List<PointSaleResultFullDto>> response = new();

            try
            {
                var pointSaleResult = await _pointSaleRepository.GetByOperatorAsync(operatorId);

                if (!pointSaleResult.IsSuccess || pointSaleResult.Data == null)
                {
                    response.ErrorMessage = pointSaleResult.ErrorMessage ?? "Failed to retrieve point of sale data.";
                    return response;
                }

                response.Data = pointSaleResult.Data;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }


        /// <summary>
        /// Асинхронно получает количество точек продаж.
        /// </summary>
        public async Task<OperationResult<int>> GetCountPointSalesAsync()
        {
            OperationResult<int> response = new();
            try
            {
                var dateResult = await _pointSaleRepository.GetCountPointSalesAsync();
                if (!dateResult.IsSuccess)
                {
                    response.ErrorMessage = dateResult.ErrorMessage ?? "Не удалось получить количество точек продаж.";
                    return response;
                }

                response.Data = dateResult.Data;
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
                return response;
            }

            return response;
        }

        /// <summary>
        /// Асинхронно получает количество точек продаж по идентификаторам владельцев.
        /// </summary>
        public async Task<OperationResult<List<PointSaleCounts>>> GetCountsByOwnersIdAsync(List<Guid> ownerIds)
        {
            OperationResult<List<PointSaleCounts>> response = new();
            try
            {
                if (ownerIds == null || !ownerIds.Any())
                {
                    response.Data = new List<PointSaleCounts>();
                    return response;
                }
                var countsResult = await _pointSaleRepository.GetCountsByOwnersIdAsync(ownerIds);

                response.Data = countsResult.Data;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }


        /// <summary>
        /// Асинхронно получает точку продаж по дате и идентификатору.
        /// </summary>
        /// <param name="pointSaleDate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Асинхронно получает полную информацию о точке продаж по дате и идентификатору.
        /// </summary>
        public async Task<OperationResult<PointSaleFullDto>> GetPointSaleDtoAsync(PointSaleDate pointSaleDate)
        {
            OperationResult<PointSaleFullDto> response = new();
            try
            {
                var pointSaleDto = await _pointSaleRepository.GetPointSaleDtoAsync(pointSaleDate.PointSaleId, pointSaleDate.OpenDate);
          

                if (pointSaleDto.Data == null)
                {
                    response.ErrorMessage = "Торговая точка не найдена.";
                    return response;
                }

                response.Data = pointSaleDto.Data;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }


        /// <summary>
        /// Асинхронно получает точки продаж по фильтру.
        /// </summary>
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


        /// <summary>
        /// Асинхронно добавляет связь между торговой точкой и оператором.
        /// </summary>
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


        /// <summary>
        /// Асинхронно передает право собственности на точку продаж другому владельцу.
        /// </summary>
        public async Task<OperationResult<bool>> TransferOwnershipAsync(TransferOwnershipDto transferOwnershipDto)
        {
            OperationResult<bool> response = new();
            try
            {
                var transferResult = await _pointSaleRepository.TransferOwnershipAsync(transferOwnershipDto);
                if (!transferResult.IsSuccess)
                {
                    response.ErrorMessage = transferResult.ErrorMessage ?? "Не удалось передать право собственности на точку продаж.";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        /// <summary>
        /// Асинхронно удаляет связь между организацией и оператором.
        /// </summary>
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
            var pointSaleOpenDate = pointSale.PointSale.CreationDate;

            if (pointSale?.PointSale?.CreationDate == null)
            {
                response.Data = true;
                return response;
            }

            if (pointSale.Chief != null && pointSale.Chief.OpenDate != null && pointSale.Chief.OpenDate < pointSaleOpenDate)
            {
                response.ErrorMessage = "Дата создания руководителя не может быть раньше даты создания торговой точки.";
                return response;
            }

            if (pointSale.Location != null && pointSale.Location.OpenDate != null && pointSale.Location.OpenDate < pointSaleOpenDate)
            {
                response.ErrorMessage = "Дата создания локации не может быть раньше даты создания торговой точки.";
                return response;
            }

            response.Data = true;
            return response;

        }


        /// <summary>
        /// Асинхронно обновляет имя точки продаж.
        /// </summary>
        public async Task<OperationResult<bool>> UpdatePointSaleNameAsync(PointSaleNameUpdateDto pointSaleNameUpdateDto)
        {
            OperationResult<bool> response = new();
            try
            {
                var updateResult = await _pointSaleRepository.UpdatePointSaleNameAsync(pointSaleNameUpdateDto);
                if (!updateResult.IsSuccess)
                {
                    response.ErrorMessage = updateResult.ErrorMessage ?? "Не удалось обновить имя точки продаж.";
                    return response;
                }

                response.Data = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

        }
        /// <summary>
        /// Асинхронно обновляет дату создания (открытия) точки продаж.
        /// </summary>
        public async Task<OperationResult<bool>> UpdateCreationDateAsync(CreationDateUpdateDto pointSaleOpenDateUpdateDto)
        {
            OperationResult<bool> response = new();
            try
            {
                var updateResult = await _pointSaleRepository.UpdateCreationDateAsync(pointSaleOpenDateUpdateDto);
                if (!updateResult.IsSuccess)
                {
                    response.ErrorMessage = updateResult.ErrorMessage ?? "Не удалось обновить дату открытия точки продаж.";
                    return response;
                }
                response.Data = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            
        }

        /// <summary>
        /// Асинхронно обновляет расписание работы точки продаж.
        /// </summary>
        public async Task<OperationResult<bool>> UpdateWorkScheduleAsync(WorkScheduleUpdateDto pointSaleWorkScheduleUpdateDto)
        {
            OperationResult<bool> response = new();
            try
            {
                var updateResult = await _pointSaleRepository.UpdateWorkScheduleAsync(pointSaleWorkScheduleUpdateDto);
                if (!updateResult.IsSuccess)
                {
                    response.ErrorMessage = updateResult.ErrorMessage ?? "Не удалось обновить расписание работы точки продаж.";
                    return response;
                }
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// Асинхронно обновляет категории точки продаж.
        /// </summary>
        public async Task<OperationResult<bool>> UpdateCategoriesAsync(CategoriesUpdateDto categoriesUpdate)
        {
            OperationResult<bool> response = new();
            try
            {
                var updateResult = await _pointSaleRepository.UpdateCategoriesAsync(categoriesUpdate);
                if (!updateResult.IsSuccess)
                {
                    response.ErrorMessage = updateResult.ErrorMessage ?? "Не удалось обновить категории точки продаж.";
                    return response;
                }
                response.Data = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }


        /// <summary>
        /// Асинхронно удаляет владельца точки продаж.
        /// </summary>
        public async Task<OperationResult<bool>> DeleteOwnerAsync(DeleteOwnerDto deleteOwner)
        {
            OperationResult<bool> response = new();
            try
            {
                var deleteResult = await _pointSaleRepository.DeleteOwnerAsync(deleteOwner);
                if (!deleteResult.IsSuccess)
                {
                    response.ErrorMessage = deleteResult.ErrorMessage ?? "Не удалось удалить владельца точки продаж.";
                    return response;
                }
                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        /// <summary>
        /// Отправляет запрос на верификацию точки продаж.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор точки продаж.</param>
        /// <returns>Результат операции со списком идентификаторов админов которые должны подвердить верификацию.</returns>
        public async Task<OperationResult<List<Guid>>> SubmitVerificationAsync(Guid pointSaleId)
        {
            OperationResult<List<Guid>> response = new();
            try
            {
                var verificationResult = await _pointSaleRepository.SubmitVerificationAsync(pointSaleId);

                if (!verificationResult.IsSuccess)
                {
                    response.ErrorMessage = verificationResult.ErrorMessage ?? "Не удалось отправить запрос на верификацию точки продаж.";
                    return response;
                }
                response.Data = verificationResult.Data;
                return response;


            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }


        /// <summary>
        /// Подтверждает, что точка продаж прошла верификацию и она проверина.
        /// </summary>
        /// <param name="poinrSaleIsApproved">DTO с данными подтверждения.</param>
        public async Task<OperationResult<bool>> ConfirmIsApprovedAsync(PoinrSaleIsApprovedDto poinrSaleIsApproved)
        {
            OperationResult<bool> response = new();
            try
            {
                var updateResult = await _pointSaleRepository.ConfirmIsApprovedAsync(poinrSaleIsApproved);
                if (!updateResult.IsSuccess)
                {
                    response.ErrorMessage = updateResult.ErrorMessage ?? "Не удалось подтвердить точку продаж как верифицированную.";
                    return response;
                }
                response.Data = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }


        /// <summary>
        /// Асинхронно удаляет точку продаж вместе с её фотографиями и логотипом.
        /// </summary>
        public async Task<OperationResult<bool>> DeletePointSaleAsync(Guid pointSaleId)
        {
            OperationResult<bool> response = new();
            try
            {    // Получаем id фотографий для удаления
                var photosIdResult = await _photoRepository.GetPreviewsAsync(pointSaleId);
                if(!photosIdResult.IsSuccess || photosIdResult.Data == null)
                {                    
                    response.ErrorMessage = photosIdResult.ErrorMessage ?? "Не удалось получить фотографии точки продаж.";
                    return response;
                }

                List<Guid> deletedPhotoIds = new();
                // Удаляем каждую фотографию из файлового хранилища
                foreach (var photoId in photosIdResult.Data)
                {
                    var deleteRequest = new RequestFileDelete { FileId = photoId, IsDeleteAllChild = true };
                    var deletePhotoResult = await _fileConnected.Delete(deleteRequest);
                    if (!deletePhotoResult.IsSuccess)
                    {
                        response.ErrorMessage = deletePhotoResult.ErrorMessage ?? "Не удалось удалить фотографию точки продаж.";
                        return response;
                    }
                    deletedPhotoIds.Add(photoId);
                }
                // Удаляем записи фотографий из базы
                var deletePhotosResult = await _photoRepository.DeletePhotosAsync(pointSaleId, deletedPhotoIds);

                // Получаем ID логотипов для удаления
                var logoIdResult = await _photoRepository.GetLogoIdAsync(pointSaleId);
                if (!logoIdResult.IsSuccess || logoIdResult.Data == null)
                {
                    response.ErrorMessage = logoIdResult.ErrorMessage ?? "Не удалось получить логотип точки продаж.";
                    return response;
                }

                List<Guid> deletedLogoIds = new();
                // Удаляем логотипы из файлового хранилища
                foreach (var logoId in logoIdResult.Data)
                {
                    var deleteLogoRequest = new RequestFileDelete { FileId = logoId, IsDeleteAllChild = true };
                    var deleteLogoResponse = await _fileConnected.Delete(deleteLogoRequest);
                    if (!deleteLogoResponse.IsSuccess)
                    {
                        response.ErrorMessage = deleteLogoResponse.ErrorMessage;
                        return response;
                    }
                    deletedLogoIds.Add(logoId);
                }
                // Удаляем записи логотипов из базы
                var deleteLogoResult = await _photoRepository.DeletePhotosAsync(pointSaleId, deletedLogoIds);

                // Удаляем саму точку продаж из базы
                var deleteResult = await _pointSaleRepository.DeletePointSaleAsync(pointSaleId);
                if (!deleteResult.IsSuccess)
                {
                    response.ErrorMessage = deleteResult.ErrorMessage ?? "Не удалось удалить точку продаж.";
                    return response;
                }
                response.Data = deleteResult.Data;

                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }


        /// <summary>
        /// Асинхронно закрывает точку продаж.
        /// </summary>
        public async Task<OperationResult<bool>> ClosePointSaleAsync(PointSaleCloseDto pointSaleCloseDto)
        {
            OperationResult<bool> response = new();
            try
            {
                var closeResult = await _pointSaleRepository.ClosePointSaleAsync(pointSaleCloseDto);
                if (!closeResult.IsSuccess)
                {
                    response.ErrorMessage = closeResult.ErrorMessage ?? "Не удалось закрыть точку продаж.";
                    return response;
                }
                response.Data = true;
                return response;


            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
    }

}
