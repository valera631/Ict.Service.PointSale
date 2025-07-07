using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.References;
using Ict.Service.PointSale.Repository.Abstractions.Interfaces;

namespace Ict.Service.PointSale.Core.Services
{
    public class ReferencesService : IReferencesService
    {

        private readonly IReferencesRepository _referencesRepository;

        public ReferencesService(IReferencesRepository referencesRepository)
        {
            _referencesRepository = referencesRepository;
        }

        public async Task<OperationResult<PointSaleTypesDto>> GetAllTypesAsync()
        {
            OperationResult<PointSaleTypesDto> response = new();
            try
            {
                var result = new PointSaleTypesDto();

                // Получение типов создания
                var creationTypesResult = await _referencesRepository.GetCreationTypesAsync();
                if (creationTypesResult.Data == null)
                {
                    response.ErrorMessage = creationTypesResult.ErrorMessage ?? "Ошибка при получении типов создания";
                    return response;
                }
                result.CreationTypes = creationTypesResult.Data ?? new List<LookupItemDto>();

                // Получение типов организаций
                var organizationTypesResult = await _referencesRepository.GetOrganizationTypesAsync();
                if (organizationTypesResult.Data == null)
                {
                    response.ErrorMessage = organizationTypesResult.ErrorMessage ?? "Ошибка при получении типов организаций";
                    return response;
                }
                result.OrganizationTypes = organizationTypesResult.Data ?? new List<LookupItemDto>();


                // Получение типов должностей руководителей
                var chiefPositionTypesResult = await _referencesRepository.GetChiefPositionTypesAsync();
                if (chiefPositionTypesResult.Data == null)
                {
                    response.ErrorMessage = chiefPositionTypesResult.ErrorMessage ?? "Ошибка при получении типов должностей руководителей";
                    return response;
                }
                result.ChiefPositions = chiefPositionTypesResult.Data ?? new List<LookupItemDto>();

                // Получение типов владельцев
                var ownerTypesResult = await _referencesRepository.GetOwnerTypesAsync();
                if (ownerTypesResult.Data == null)
                {
                    response.ErrorMessage = ownerTypesResult.ErrorMessage ?? "Ошибка при получении типов владельцев";
                    return response;
                }
                result.OwnerTypes = ownerTypesResult.Data ?? new List<LookupItemDto>();


                var categoryResult = await _referencesRepository.GetCategoriesAsync();
                if (categoryResult.Data == null)
                {
                    response.ErrorMessage = categoryResult.ErrorMessage ?? "Ошибка при получении категорий";
                    return response;
                }
                result.Categories = categoryResult.Data ?? new List<CategoryItem>();

                response.Data = result;
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
