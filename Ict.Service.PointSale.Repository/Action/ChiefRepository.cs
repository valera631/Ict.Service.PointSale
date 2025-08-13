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
    /// Репозиторий для управления данными руководителей (Chief).
    /// </summary>
    public class ChiefRepository(PointSaleDbContext _pointSaleDbContext) : IChiefRepository
    {

        /// <summary>
        /// Асинхронно обновляет или добавляет запись о руководителе для указанной торговой точки.
        /// </summary>
        public async Task<OperationResult<bool>> UpdateAsync(ChiefUpdateDto chefUpdate)
        {
            var response = new OperationResult<bool>();
            try
            {
                var existingChef = await _pointSaleDbContext.Chiefs
                    .FirstOrDefaultAsync(c => c.PointSaleId == chefUpdate.PointSaleId &&
                                             c.OpenDate == chefUpdate.OpenDateChief);
                if (existingChef != null)
                {
                    existingChef.ChiefName = chefUpdate.ChiefName;
                    existingChef.ChiefPositionId = chefUpdate.ChiefPositionId;
                    _pointSaleDbContext.Chiefs.Update(existingChef);
                }
                else
                {
                    Chief? previousChef = null;

                    // Ищем предыдущего шеф-повара только если OpenDateChef указана.
                    if (chefUpdate.OpenDateChief.HasValue)
                    {
                        previousChef = await _pointSaleDbContext.Chiefs
                            .Where(c => c.PointSaleId == chefUpdate.PointSaleId &&
                                        c.ChiefPositionId == chefUpdate.ChiefPositionId && // Важно: для той же должности
                                        c.OpenDate < chefUpdate.OpenDateChief.Value) // Даты, которые раньше новой
                            .OrderByDescending(c => c.OpenDate) // Самая свежая предыдущая запись
                            .FirstOrDefaultAsync();
                    }

                    // Создаем новую запись о шеф-поваре.
                    var newChef = new Chief
                    {
                        ChiefId = Guid.NewGuid(),
                        PointSaleId = chefUpdate.PointSaleId,
                        ChiefPositionId = chefUpdate.ChiefPositionId,
                        ChiefName = chefUpdate.ChiefName,
                        OpenDate = chefUpdate.OpenDateChief ?? default,
                        EntryDate = DateTime.UtcNow,
                        IsAproved = false
                    };

                    await _pointSaleDbContext.Chiefs.AddAsync(newChef);
                }

                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Произошла ошибка при обновлении или создании данных шеф-повара: " + ex.Message;
                response.Data = false;
            }
            return response;
        }


    }
}
