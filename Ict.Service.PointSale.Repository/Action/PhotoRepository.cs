using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.DataBase.DBModels;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Photo;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.Repository.Action
{
    /// <summary>
    /// Репозиторий для работы с фотографиями и логотипами торговых точек.
    /// </summary>
    public class PhotoRepository : IPhotoRepository
    {

        private readonly PointSaleDbContext _pointSaleDbContext;

        public PhotoRepository(PointSaleDbContext pointSaleDbContext)
        {
            _pointSaleDbContext = pointSaleDbContext;
        }


        /// <summary>
        /// Асинхронно добавляет логотип к торговой точке.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор торговой точки.</param>
        /// <param name="logoId">Идентификатор логотипа (фото).</param>
        /// <param name="OpenDateLogo">Дата открытия/создания логотипа.</param>
        /// <returns>Результат операции с признаком успешности.</returns>
        public async Task<OperationResult<bool>> AddLogoAsync(Guid pointSaleId, Guid logoId, DateOnly OpenDateLogo)
        {
            OperationResult<bool> response = new();

            try
            {
                var pointSale = await _pointSaleDbContext.PointSaleEntities
                    .FirstOrDefaultAsync(p => p.PointSaleId == pointSaleId);

                if ( pointSale== null)
                {
                    response.ErrorMessage = "PointSale not found.";
                    return response;
                }
                var pointSaleLogo = new Logo
                {
                    PointSaleId = pointSaleId,
                    LogoId = logoId,
                    OpenDate = OpenDateLogo,
                    EntryDate = DateTime.UtcNow
                };

                await _pointSaleDbContext.Logos.AddAsync(pointSaleLogo);

                // Сохраняем изменения в базе данных
                await _pointSaleDbContext.SaveChangesAsync();

                response.Data = true;

            }
            catch (Exception ex)
            {

                response.ErrorMessage = $"Ошибка при добавлении логотипа: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// Асинхронно добавляет список фотографий к торговой точке.
        /// Первое фото автоматически назначается главным, если главного ещё нет.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор торговой точки.</param>
        /// <param name="photoId">Список идентификаторов фотографий для добавления.</param>
        /// <returns>Результат операции с признаком успешности.</returns>
        public async Task<OperationResult<bool>> AddPhotoAsync(Guid pointSaleId, List<Guid?> photoId)
        {

            OperationResult<bool> response = new();

            try
            {
                var pointSale = await _pointSaleDbContext.PointSaleEntities
                    .FirstOrDefaultAsync(p => p.PointSaleId == pointSaleId);

                if (pointSale == null) 
                {
                    response.ErrorMessage = "PointSale not found.";
                    return response;
                }

                var validFileIds = photoId.Where(id => id.HasValue).Select(id => id.Value).ToList();

                // Проверка наличия главного фото для организации
                bool hasMainPhoto = await _pointSaleDbContext.Photos
                    .AnyAsync(op => op.PointSaleId == pointSaleId && op.IsMain);

                // Обработка файлов
                for (int i = 0; i < validFileIds.Count; i++)
                {
                    var fileId = validFileIds[i];

                    // Первое фото становится главным, только если ещё нет главного фото
                    bool isMain = !hasMainPhoto && i == 0;

                    var organizationPhoto = new Photo
                    {
                        PhotoId = fileId,
                        PointSaleId = pointSaleId,
                        IsMain = isMain
                    };

                    await _pointSaleDbContext.Photos.AddAsync(organizationPhoto);

                    // Если установили главное фото, отмечаем, что оно теперь есть
                    if (isMain)
                    {
                        hasMainPhoto = true;
                    }
                }

                // Сохраняем изменения в базе данных
                await _pointSaleDbContext.SaveChangesAsync();
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
        /// Асинхронно удаляет указанные фотографии у торговой точки.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор торговой точки.</param>
        /// <param name="photoIds">Список идентификаторов фотографий для удаления.</param>
        /// <returns>Результат операции с списком удалённых идентификаторов.</returns>
        public async Task<OperationResult<List<Guid>>> DeletePhotosAsync(Guid pointSaleId, List<Guid> photoIds)
        {
            OperationResult<List<Guid>> response = new();
            try
            {
                // Проверка, что organizationId указан
                if (pointSaleId == Guid.Empty)
                {
                    response.ErrorMessage = "Идентификатор организации не указан.";
                    return response;
                }

                // Проверка, что список photoIds не пуст
                if (photoIds == null || !photoIds.Any())
                {
                    response.ErrorMessage = "Список идентификаторов фотографий пуст.";
                    return response;
                }

                var pointSaleExist = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == pointSaleId);

                if (!pointSaleExist)
                {
                    response.ErrorMessage = "PointSale not found.";
                    return response;
                }

                // Получаем фотографии, которые нужно удалить
                var photosToDelete = await _pointSaleDbContext.Photos
                    .Where(p => p.PointSaleId == pointSaleId && photoIds.Contains(p.PhotoId))
                    .ToListAsync();

                if (!photosToDelete.Any())
                {
                    response.ErrorMessage = "No photos found to delete.";
                    return response;
                }

                // Удаляем фотографии
                _pointSaleDbContext.Photos.RemoveRange(photosToDelete);


                // Сохраняем изменения
                await _pointSaleDbContext.SaveChangesAsync();

                response.Data = photosToDelete.Select(op => op.PhotoId).ToList();

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.Data = new List<Guid>();
            }
            return response;
        }


        /// <summary>
        /// Получает последний логотип торговой точки по дате открытия.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор торговой точки.</param>
        /// <returns>Результат операции с DTO логотипа.</returns>
        public async Task<OperationResult<LogoDto>> GetLogoAsync(Guid pointSaleId)
        {
            OperationResult<LogoDto> response = new();
            try
            {
                var pointSaleExist = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == pointSaleId);

                if (!pointSaleExist)
                {
                    response.ErrorMessage = "PointSale not found.";
                    return response;
                }

                var logoId = await _pointSaleDbContext.Logos
                  .Where(p => p.PointSaleId == pointSaleId)
                  .OrderByDescending(p => p.OpenDate) // Сортировка по дате создания
                  .Select(ol => new LogoDto
                  {
                     LogoId = ol.LogoId,
                     OpenDateLogo = ol.OpenDate
                 })
                 .FirstOrDefaultAsync();

                response.Data = logoId;
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Получает список всех идентификаторов логотипов, связанных с торговой точкой.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор торговой точки.</param>
        public async Task<OperationResult<List<Guid>>> GetLogoIdAsync(Guid pointSaleId)
        {
            OperationResult<List<Guid>> response = new();
            try
            {
                var pointSaleExist = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == pointSaleId);
                if (!pointSaleExist)
                    {
                    response.ErrorMessage = "Магазин не найден";
                    return response;
                }

                var logoIds = await _pointSaleDbContext.Logos
                    .Where(p => p.PointSaleId == pointSaleId)
                    .Select(p => p.LogoId)
                    .ToListAsync();

                response.Data = logoIds ?? new List<Guid>();

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.Data = new List<Guid>();
            }
            return response;
        }

        /// <summary>
        /// Получает список идентификаторов фотографий по списку id для заданной торговой точки.
        /// </summary>
        /// <param name="pointSaleId">Идентификатор торговой точки.</param>
        /// <param name="photoIds">Список идентификаторов фотографий.</param>
        /// <returns>Результат операции со списком найденных идентификаторов.</returns>
        public async Task<OperationResult<List<Guid>>> GetPhotosByIdsAsync(Guid pointSaleId, List<Guid> photoIds)
        {
            OperationResult<List<Guid>> response = new();
            try
            {

                // Проверка, что organizationId указан
                if (pointSaleId == Guid.Empty)
                {
                    response.ErrorMessage = "Идентификатор организации не указан.";
                    return response;
                }

                // Проверка, что список photoIds не пуст
                if (photoIds == null || !photoIds.Any())
                {
                    response.ErrorMessage = "Список идентификаторов фотографий пуст.";
                    return response;
                }


                var pointSaleExist = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == pointSaleId);


                if (!pointSaleExist)
                {
                    response.ErrorMessage = "PointSale not found.";
                    return response;
                }

                var photos = await _pointSaleDbContext.Photos
                    .Where(p => photoIds.Contains(p.PhotoId) &&
                           (p.PointSaleId == pointSaleId))
                    .Select(p => p.PhotoId)
                    .ToListAsync();


                response.Data = photos;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Получает все фотографии (preview) торговой точки.
        /// </summary>
        public async Task<OperationResult<List<Guid>>> GetPreviewsAsync(Guid pointSaleId)
        {
            OperationResult<List<Guid>> response = new();

            try
            {
                var pointSaleExist = _pointSaleDbContext.PointSaleEntities
                    .Any(p => p.PointSaleId == pointSaleId);

                if (!pointSaleExist)
                {
                    response.ErrorMessage = "PointSale not found.";
                    return response;
                }

                var previews = await _pointSaleDbContext.Photos
                    .Where(p => p.PointSaleId == pointSaleId)
                    .Select(p => p.PhotoId)
                    .ToListAsync();

                response.Data = previews;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
