using Ict.Provider.Service.File.Interface;
using Ict.Service.File.Models.File;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Photo;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ict.Service.PointSale.Core.Services
{
    /// <summary>
    /// Сервис для работы с фотографиями точек продаж.
    /// Отвечает за загрузку, получение и удаление логотипов и других фотографий.
    /// </summary>
    public class PhotoService : IPhotoService
    {
        private readonly IFileProvider _fileConnected;
        public readonly IPhotoRepository _photoRepository;
        private readonly string _parentId;




        /// <summary>
        /// Создаёт экземпляр сервиса для работы с фотографиями.
        /// </summary>
        /// <param name="fileConnected">Сервис для взаимодействия с файловым хранилищем.</param>
        /// <param name="photoRepository">Репозиторий для работы с фотографиями в базе данных.</param>
        public PhotoService(IFileProvider fileConnected, IConfiguration configuration, IPhotoRepository  photoRepository)
        {
            _fileConnected = fileConnected;
            _parentId = configuration["Service.Pointsale:PointsaleId"] ??
                 throw new ArgumentNullException(nameof(_parentId), "PointsaleId не найден");
            _photoRepository = photoRepository;
        }


        /// <summary>
        /// Асинхронно добавляет логотип для точки продаж.
        /// </summary>
        /// <param name="photoContainerDto">DTO с фотографиями и данными о точке продаж.</param>
        /// <returns>Результат операции с булевым значением успешности.</returns>
        public async Task<OperationResult<bool>> AddLogoAsync(PhotoContainerDto photoContainerDto)
        {
            OperationResult<bool> response = new();
            try
            {
                // Проверка на наличие фотографий
                if (photoContainerDto.Photos == null || !photoContainerDto.Photos.Any())
                {
                    response.ErrorMessage = "Список фотографий пуст.";
                    return response;
                }

                var fileElements = new List<FileElement>();

                // Обработка каждой фотографии
                foreach (var photo in photoContainerDto.Photos)
                {
                    // Проверка, является ли файл изображением
                    if (!photo.ContentType.StartsWith("image/"))
                    {
                        response.ErrorMessage = $"Файл {photo.FileName} не является изображением.";
                        return response;
                    }

                    var fileElement = new FileElement
                    {
                        FileId = photo.ImageId,
                        FileSize = photo.Images,
                        ContentType = photo.ContentType,
                        Name = photo.FileName,
                        FileParentId = photo.ParentId,
                        OwnerId = photoContainerDto.PointSaleId,
                        IsNewVersion = false,
                        ServiceId = new Guid(_parentId),
                    };

                    fileElements.Add(fileElement);
                }

                // Создание контейнера для файлов
                var fileContainer = new FileContainer
                {
                    Elements = fileElements
                };

                // Загрузка файлов через FileConnected
                var fileResponse = await _fileConnected.Create2(fileContainer);

                // Получение ID сохраненного файла
                var savedFileId = fileContainer.Elements
                    .FirstOrDefault(e => e.FileParentId == null)?.FileId;



                if (savedFileId == null)
                {
                    response.ErrorMessage = "Не удалось получить ID для всех загруженных файлов.";
                    return response;
                }
                var result = await _photoRepository.AddLogoAsync(photoContainerDto.PointSaleId, savedFileId.Value, photoContainerDto.OpenDateLogo);

                if (!result.IsSuccess)
                {
                    response.ErrorMessage = result.ErrorMessage;
                    return response;
                }

                response.Data = true;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }


        /// <summary>
        /// Асинхронно получает логотип точки продаж по её идентификатору.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор точки продаж.</param>
        /// <returns>Результат операции с DTO логотипа и его данными.</returns>
        public async Task<OperationResult<PointSalePhotoDto>> GetLogoAsync(Guid pointSaleId)
        {

            OperationResult<PointSalePhotoDto> response = new();
            try
            {
                
                var logoId = await _photoRepository.GetLogoAsync(pointSaleId);

                if (logoId.Data == null)
                {
                    response.ErrorMessage = logoId.ErrorMessage;
                    return response;
                }

                var logoRequest = new RequestFileDto { FileId = logoId.Data.LogoId, IsLastVersions = false };
                var logoResponse = await _fileConnected.Select(logoRequest);
                if (!logoResponse.IsSuccess || logoResponse.Result == null)
                {
                    response.ErrorMessage = logoResponse.ErrorMessage;
                    return response;
                }
                var pointSalePhotoDto = new PointSalePhotoDto
                {
                    PhotoId = logoResponse.Result.FileId ?? Guid.Empty,
                    PhotoName = logoResponse.Result.Name,
                    OpenDateLogo = logoId.Data.OpenDateLogo,
                    PhotoContentType = logoResponse.Result.ContentType,
                    IsMain = false,
                    PhotoData = logoResponse.Result.FileSize
                };
                response.Data = pointSalePhotoDto;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        /// <summary>
        /// Асинхронно добавляет фотографии к точке продаж.
        /// </summary>
        /// <param name="photoContainer">DTO с фотографиями и данными о точке продаж.</param>
        /// <returns>Результат операции с булевым значением успешности.</returns>
        public async Task<OperationResult<bool>> AddPhotoAsync(PhotoContainerDto photoContainer)
        {
            OperationResult<bool> response = new();
            try
            {
                // Проверка, что список фотографий не пуст
                if (photoContainer.Photos == null || !photoContainer.Photos.Any())
                {
                    response.ErrorMessage = "Список фотографий пуст.";
                    return response;
                }


                // Проверка, что OrganizationId указан
                if (photoContainer.PointSaleId == Guid.Empty)
                {
                    response.ErrorMessage = "Идентификатор организации не указан.";
                    return response;
                }

                var fileContainer = new FileContainer();

                foreach (var file in photoContainer.Photos)
                {
                    // Проверка, является ли файл изображением
                    if (!file.ContentType.StartsWith("image/"))
                    {
                        response.ErrorMessage = $"Файл {file.FileName} не является изображением.";
                        return response;
                    }

                    // Создание FileElement для каждого файла
                    var fileElement = new FileElement
                    {
                        FileId = file.ImageId,
                        FileSize = file.Images,
                        ContentType = file.ContentType,
                        Name = file.FileName,
                        FileParentId = file.ParentId,
                        OwnerId = photoContainer.PointSaleId,
                        IsNewVersion = false,
                        ServiceId = new Guid(_parentId)
                    };

                    // Добавление файла в контейнер
                    fileContainer.Elements.Add(fileElement);
                }

                // Загрузка файлов в хранилище
                var fileResponse = await _fileConnected.Create2(fileContainer);

                // Получение идентификаторов сохранённых файлов с FileParentId == null
                var savedFileIds = fileContainer.Elements
                    .Where(e => e.FileParentId == null)
                    .Select(e => e.FileId)
                    .ToList();

                // Проверка, есть ли файлы для связывания
                if (savedFileIds == null || !savedFileIds.Any())
                {
                    response.ErrorMessage = "Нет файлов с FileParentId равным null для связывания.";
                    return response;
                }

                var result = await _photoRepository.AddPhotoAsync(photoContainer.PointSaleId, savedFileIds);

                if (!result.IsSuccess)
                {
                    response.ErrorMessage = result.ErrorMessage;
                    return response;
                }

                response.Data = true;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        /// <summary>
        /// Асинхронно получает превью фотографий точки продаж.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор точки продаж.</param>
        /// <returns>Результат операции со списком DTO фотографий.</returns>
        public async Task<OperationResult<List<PointSalePhotoDto>>> GetPreviewsAsync(Guid pointSaleId)
        {
            OperationResult<List<PointSalePhotoDto>> response = new();

            try
            {
                var previewIds = await _photoRepository.GetPreviewsAsync(pointSaleId);

                if (!previewIds.IsSuccess)
                {
                    response.ErrorMessage = previewIds.ErrorMessage;
                    return response;
                }


                if (previewIds.Data == null || !previewIds.Data.Any())
                {
                    response.Data = new List<PointSalePhotoDto>();
                    return response;
                }

                var result = new List<PointSalePhotoDto>();
                foreach (var previewId in previewIds.Data)
                {
                    var previewRequest = new RequestFileDto { FileId = previewId, IsLastVersions = false };


                    var previewResponse = await _fileConnected.Select(previewRequest);
                    if (!previewResponse.IsSuccess || previewResponse.Result == null)
                    {
                        response.ErrorMessage = previewResponse.ErrorMessage;
                        return response;
                    }

                    var organizationPhotoDto = new PointSalePhotoDto
                    {
                        PhotoId = previewId,
                        PhotoName = previewResponse.Result.Name,
                        PhotoContentType = previewResponse.Result.ContentType,
                        IsMain = false,
                        PhotoData = previewResponse.Result.FileSize
                    };
                    result.Add(organizationPhotoDto);
                }
                response.Data = result;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        /// <summary>
        /// Асинхронно удаляет фотографии точки продаж.
        /// </summary>
        public async Task<OperationResult<List<Guid>>> DeletePhotoAsync(PhotosDeleteDto deletePhotosDto)
        {
            OperationResult<List<Guid>> response = new();
            try
            {
                // Проверяем входные данные
                if (deletePhotosDto == null || deletePhotosDto.PointSaleId == Guid.Empty || deletePhotosDto.PhotoIds == null || !deletePhotosDto.PhotoIds.Any())
                {
                    response.ErrorMessage = "Некорректные входные данные: организация или фотографии не указаны.";
                    return response;
                }

                var photos = await _photoRepository.GetPhotosByIdsAsync(
                   deletePhotosDto.PointSaleId,
                   deletePhotosDto.PhotoIds
                   );

                 // Удаляем файлы из хранилища
                List<Guid> deletedPhotoIds = new();
                foreach (var photoId in photos.Data)
                {
                    var deleteRequest = new RequestFileDelete { FileId = photoId, IsDeleteAllChild = true };
                    var deleteResponse = await _fileConnected.Delete(deleteRequest);
                    if (!deleteResponse.IsSuccess)
                    {
                        response.ErrorMessage = deleteResponse.ErrorMessage;
                        return response;
                    }
                    deletedPhotoIds.Add(photoId);
                }

                var deleteResult = await _photoRepository.DeletePhotosAsync(deletePhotosDto.PointSaleId,
                    deletedPhotoIds);

                if (!deleteResult.IsSuccess)
                {
                    response.ErrorMessage = deleteResult.ErrorMessage;
                    return response;
                }


                response.Data = deleteResult.Data;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }
    }
}
