using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.DataBase.DBModels;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Models.Update;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.Repository.Action
{

    /// <summary>
    /// Репозиторий для управления описаниями торговых точек.
    /// </summary>
    public class DescriptionRepository : IDescriptionRepository
    {

        private readonly PointSaleDbContext _pointSaleDbContext;

        public DescriptionRepository(PointSaleDbContext pointSaleDbContext)
        {
            _pointSaleDbContext = pointSaleDbContext;
        }



        /// <summary>
        /// Асинхронно обновляет или создаёт описание торговой точки.
        /// </summary>
        public async Task<OperationResult<bool>> UpdateDescriptionAsync(DescriptionUpdateDto descriptionUpdate)
        {
            OperationResult<bool> response = new();
            try
            {
                // 1. Проверяем существование торговой точки.
                var pointSaleExists = await _pointSaleDbContext.PointSaleEntities
                    .AnyAsync(p => p.PointSaleId == descriptionUpdate.PointSaleId);

                if (!pointSaleExists)
                {
                    response.ErrorMessage = "Торговая точка с указанным PointSaleId не найдена.";
                    return response;
                }

                // 2. Попытаться найти существующую запись описания.
                var existingDescription = await _pointSaleDbContext.Descriptions
                    .FirstOrDefaultAsync(d => d.PointSaleId == descriptionUpdate.PointSaleId &&
                                             d.OpenDate == descriptionUpdate.OpenDateDescription);

                if (existingDescription != null)
                {
                    // 3. Если запись найдена, обновляем её.
                    existingDescription.DescriptionText = descriptionUpdate.DescriptionText;
                    _pointSaleDbContext.Descriptions.Update(existingDescription);
                }
                else
                {
                    // 4. Если запись не найдена, создаём новую.
                    Description? previousDescription = null;
                        previousDescription = await _pointSaleDbContext.Descriptions
                            .Where(d => d.PointSaleId == descriptionUpdate.PointSaleId &&
                                        d.OpenDate < descriptionUpdate.OpenDateDescription)
                            .OrderByDescending(d => d.OpenDate)
                            .FirstOrDefaultAsync();
                    

                    // Создаем новую запись описания.
                    var newDescription = new Description
                    {
                        DescriptionId = Guid.NewGuid(),
                        PointSaleId = descriptionUpdate.PointSaleId,
                        DescriptionText = descriptionUpdate.DescriptionText,
                        OpenDate = descriptionUpdate.OpenDateDescription,
                        EntryDate = DateTime.UtcNow,
                       
                    };

                    await _pointSaleDbContext.Descriptions.AddAsync(newDescription);
                }

                // 5. Сохраняем все изменения в базе данных.
                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Произошла ошибка при обновлении описания торговой точки: " + ex.Message;
                response.Data = false;
            }

            return response;
        }
    }
}
