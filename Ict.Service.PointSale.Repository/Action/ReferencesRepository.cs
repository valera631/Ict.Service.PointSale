using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.DataBase;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.References;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.Repository.Action
{
    public class ReferencesRepository : IReferencesRepository
    {
        private readonly PointSaleDbContext _pointSaleDbContext;

        public ReferencesRepository(PointSaleDbContext pointSaleDbContext)
        {
            _pointSaleDbContext = pointSaleDbContext;
        }

        public async Task<OperationResult<List<CategoryItem>>> GetCategoriesAsync()
        {
            OperationResult<List<CategoryItem>> response = new();
            try
            {
                var categories = _pointSaleDbContext.CategoryPointSales
                    .Select(x => new CategoryItem
                    {
                        Id = x.CategoryId,
                        Name = x.Name,
                        Path = x.Path
                    }).ToListAsync();

                response.Data = categories.Result;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<List<LookupItemDto>>> GetChiefPositionTypesAsync()
        {
            OperationResult<List<LookupItemDto>> response = new();
            try
            {
                var chiefPositionTypes = await _pointSaleDbContext.ChiefPositions
                    .Select(x => new LookupItemDto
                    {
                        Id = x.ChiefPositionId,
                        Name = x.PositionName
                    }).ToListAsync();
                response.Data = chiefPositionTypes;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<List<LookupItemDto>>> GetClosingStatusesAsync()
        {
            OperationResult<List<LookupItemDto>> response = new();
            try
            {
                var closingStatuses = await _pointSaleDbContext.ClosingStatuses
                    .Select(x => new LookupItemDto
                    {
                        Id = x.ClosingStatusId,
                        Name = x.NameStatus
                    }).ToListAsync();
                response.Data = closingStatuses;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<List<LookupItemDto>>> GetCreationTypesAsync()
        {
            OperationResult<List<LookupItemDto>> response = new();
            try
            {
                var creationTypes = await _pointSaleDbContext.CreationTypes
                    .Select(x => new LookupItemDto
                    {
                        Id = x.CreationTypeId,
                        Name = x.CreationTypeName
                    }).ToListAsync();
                response.Data = creationTypes;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }


        public async Task<OperationResult<List<LookupItemDto>>> GetOrganizationTypesAsync()
        {
            OperationResult<List<LookupItemDto>> response = new();
            try
            {
                var organizationTypes = await _pointSaleDbContext.OrganizationTypes
                    .Select(x => new LookupItemDto
                    {
                        Id = x.OrganizationTypeId,
                        Name = x.NameType
                    }).ToListAsync();
                response.Data = organizationTypes;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<OperationResult<List<LookupItemDto>>> GetOwnerTypesAsync()
        {
            OperationResult<List<LookupItemDto>> response = new();
            try
            {
                var ownerTypes = await _pointSaleDbContext.OwnerTypes
                    .Select(x => new LookupItemDto
                    {
                        Id = x.OwnerTypeId,
                        Name = x.NameType
                    }).ToListAsync();
                response.Data = ownerTypes;
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
