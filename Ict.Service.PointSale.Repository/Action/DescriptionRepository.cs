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
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.Repository.Action
{
    public class DescriptionRepository : IDescriptionRepository
    {

        private readonly PointSaleDbContext _pointSaleDbContext;
        private readonly IMapper _mapper;

        public DescriptionRepository(PointSaleDbContext pointSaleDbContext, IMapper mapper)
        {
            _pointSaleDbContext = pointSaleDbContext;
            _mapper = mapper;
        }


        public async Task<OperationResult<Guid>> ChangeDescriptionAsync(DescriptionChangeDto descriptionChangeDto)
        {
            OperationResult<Guid> response = new OperationResult<Guid>();

            try
            {
                var newDescription = new Description()
                {
                    DescriptionId = Guid.NewGuid(),
                    PointSaleId = descriptionChangeDto.PointSaleId,
                    DescriptionText = descriptionChangeDto.DescriptionText,
                    OpenDate = DateOnly.FromDateTime(DateTime.Now),
                    EntryDate = DateTime.Now
                };

                await _pointSaleDbContext.Descriptions.AddAsync(newDescription);

                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = newDescription.DescriptionId;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<Guid>> UpdateDescriptionAsync(DescriptionChangeDto descriptionChange)
        {
            OperationResult<Guid> response = new();
            try
            {
                // Find the most recent description for the organization
                var latestDescription = await _pointSaleDbContext.Descriptions
                    .Where(d => d.PointSaleId == descriptionChange.PointSaleId)
                    .OrderByDescending(d => d.EntryDate)
                    .FirstOrDefaultAsync();

                if (latestDescription == null)
                {
                    response.ErrorMessage = "No description found for the specified organization.";
                    return response;
                }

                latestDescription.DescriptionText = descriptionChange.DescriptionText;
                latestDescription.EntryDate = DateTime.Now;

                await _pointSaleDbContext.SaveChangesAsync();
                response.Data = latestDescription.DescriptionId;
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
