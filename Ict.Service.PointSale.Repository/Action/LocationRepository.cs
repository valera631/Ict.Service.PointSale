using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.DataBase.DBModels;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Update;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.Repository.Action
{
    /// <summary>
    /// Репозиторий для управления локациями торговых точек.
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        private readonly PointSaleDbContext _pointSaleDbContext;
        public LocationRepository(PointSaleDbContext pointSaleDbContext)
        {
            _pointSaleDbContext = pointSaleDbContext;
        }


        /// <summary>
        /// Асинхронно обновляет или создаёт адрес (локацию) торговой точки.
        /// </summary>
        /// <param name="addressUpdate">DTO с данными для обновления адреса.</param>
        /// <returns>Результат операции с признаком успешности.</returns>
        public async Task<OperationResult<bool>> UpdateAddressAsync(AddressUpdateDto addressUpdate)
        {
            OperationResult<bool> response = new();
            try
            {
                // 1. Проверяем существование торговой точки.
                var pointSaleExists = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == addressUpdate.PointSaleId);

                if (!pointSaleExists)
                {
                    response.ErrorMessage = "Торговая точка с указанным PointSaleId не найдена.";
                    return response;
                }

                if (!addressUpdate.OpenDateLocation.HasValue)
                {
                    response.ErrorMessage = "OpenDateLocation для адреса должен быть указан.";
                    return response;
                }

                // 2. Попытаться найти существующую запись Location, совпадающую по PointSaleId и OpenDateLocation.
                var existingLocation = await _pointSaleDbContext.Locations
                    .FirstOrDefaultAsync(l => l.PointSaleId == addressUpdate.PointSaleId &&
                                             l.OpenDate == addressUpdate.OpenDateLocation.Value);

                Location locationToSave;

                if (existingLocation != null)
                {
                    // 3. Если запись найдена, обновляем её.
                    locationToSave = existingLocation;
                    locationToSave.Address = addressUpdate.Address;
                    locationToSave.AddressId = addressUpdate.AddressId;
                    locationToSave.Latitude = addressUpdate.Latitude;
                    locationToSave.Longitude = addressUpdate.Longitude;
                    locationToSave.EntryDate = DateTime.UtcNow; // Обновляем дату последнего изменения
                    _pointSaleDbContext.Locations.Update(locationToSave);
                }
                else
                {
                    // 4. Если запись не найдена, создаем новую.
                    Location? previousLocation = await _pointSaleDbContext.Locations
                        .Where(l => l.PointSaleId == addressUpdate.PointSaleId)
                        .OrderByDescending(l => l.OpenDate) // Ищем самую свежую запись по дате открытия
                        .FirstOrDefaultAsync();

                    locationToSave = new Location
                    {
                        LocationId = Guid.NewGuid(),
                        PointSaleId = addressUpdate.PointSaleId,
                        EntryDate = DateTime.UtcNow,
                        OpenDate = addressUpdate.OpenDateLocation.Value,
                        Address = addressUpdate.Address,
                        AddressId = addressUpdate.AddressId,
                        Latitude = addressUpdate.Latitude,
                        Longitude = addressUpdate.Longitude
                    };

                    await _pointSaleDbContext.Locations.AddAsync(locationToSave);
                }

                // 5. Сохраняем все изменения.
                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Произошла ошибка при обновлении адреса торговой точки: " + ex.Message;
                response.Data = false;
            }
            return response;
        }
    }
}
