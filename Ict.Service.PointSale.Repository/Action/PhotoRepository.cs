using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.DataBase.DBModels;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.Repository.Action
{
    public class PhotoRepository : IPhotoRepository
    {

        private readonly PointSaleDbContext _pointSaleDbContext;

        public PhotoRepository(PointSaleDbContext pointSaleDbContext)
        {
            _pointSaleDbContext = pointSaleDbContext;
        }

        public async Task<OperationResult<bool>> AddLogoAsync(Guid pointSaleId, Guid logoId)
        {
            OperationResult<bool> response = new();

            try
            {
                var pointSale = await _pointSaleDbContext.PointSales
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
                    OpenDate = DateOnly.FromDateTime(DateTime.UtcNow),
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

        public async Task<OperationResult<bool>> AddPhotoAsync(Guid pointSaleId, List<Guid?> photoId)
        {

            OperationResult<bool> response = new();

            try
            {
                var pointSale = await _pointSaleDbContext.PointSales
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

        public async Task<OperationResult<Guid>> GetLogoAsync(Guid pointSaleId)
        {
            OperationResult<Guid> response = new();
            try
            {
                var pointSaleExist = await _pointSaleDbContext.PointSales
                    .AnyAsync(p => p.PointSaleId == pointSaleId);

                if (!pointSaleExist)
                {
                    response.ErrorMessage = "PointSale not found.";
                    return response;
                }

                var logoId = await _pointSaleDbContext.Logos
                    .Where(p => p.PointSaleId == pointSaleId)
                    .Select(p => p.LogoId)
                    .FirstOrDefaultAsync();

                response.Data = logoId;
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<OperationResult<List<Guid>>> GetPreviewsAsync(Guid pointSaleId)
        {
            OperationResult<List<Guid>> response = new();

            try
            {
                var pointSaleExist = _pointSaleDbContext.PointSales
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
