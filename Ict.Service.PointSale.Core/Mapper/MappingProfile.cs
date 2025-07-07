using AutoMapper;
using Ict.Service.PointSale.API.Abstractions.Models.PointSale;
using Ict.Service.PointSale.API.Abstractions.Models.References;
using Ict.Service.PointSale.DataBase.DBModels;
using Ict.Service.PointSale.Models;
using Ict.Service.PointSale.Models.Chief;
using Ict.Service.PointSale.Models.Description;
using Ict.Service.PointSale.Models.Location;
using Ict.Service.PointSale.Models.PointSale;
using Ict.Service.PointSale.Models.References;

namespace Ict.Service.PointSale.Core.Mapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<PointSaleCreateRequest, PointSaleFullDto>()
                .ForMember(dest => dest.PointSale, opt => opt.MapFrom(src => new PointSaleDto
                {
                    PointSaleId = Guid.NewGuid(),
                    OwnerId = src.OwnerId,
                    OwnerTypeId = src.OwnerTypeId,
                    CreationTypeId = src.CreationTypeId,
                    OrganizationTypeId = src.OrganizationTypeId,
                    EntryDate = DateTime.Now,
                    ClosingStatusId = null,
                    ClosingDate = null
                }))
                .ForMember(dest => dest.PointSaleActivity, opt => opt.MapFrom(src => new PointSaleActivityDto
                {
                    PointSaleActivityId = Guid.NewGuid(),
                    NamePointSale = src.NamePointSale,
                    EnglishNamePointSale = src.EnglishNamePointSale,
                    PointSaleId = Guid.Empty,
                    OpenDate = src.OpenDatePointSale,
                    Email = src.Email,
                    Phone = src.Phone,
                    EntryDate = DateTime.Now
                }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionText != null ? new DescriptionDto
                {
                    DescriptionId = Guid.NewGuid(),
                    DescriptionText = src.DescriptionText,
                    OpenDate = src.OpenDatePointSale,
                    PointSaleId = Guid.Empty,
                    EntryDate = DateTime.Now
                } : null))
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds))
                .ForMember(dest => dest.Chief, opt => opt.MapFrom(src => new ChiefDto
                {
                    ChiefId = Guid.NewGuid(),
                    OpenDate = src.OpenDateChif,
                    ChiefName = src.ChiefName ?? string.Empty,
                    ChiefPositionId = src.ChiefPositionId,
                    PointSaleId = Guid.Empty,
                    EntryDate = DateTime.Now
                }))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationDto
                {
                    LocationId = Guid.NewGuid(),
                    OpenDate = src.OpenDateLocation,
                    Latitude = src.Latitude,
                    Longitude = src.Longitude,
                    Address = src.Address,
                    AddressId = src.LocationId,
                    PointSaleId = Guid.Empty,
                    EntryDate = DateTime.Now
                }))
                .ForMember(dest => dest.PointSaleSchedules, opt => opt.MapFrom(src => src.WorkSchedule.Select(ws => new PointSaleScheduleDto
                {
                       PointSaleScheduleId = Guid.NewGuid(),
                       PointSaleId = Guid.Empty, // Будет заполнен позже
                       DayOfWeek = ws.DayOfWeek,
                       IsWorkingDay = ws.IsWorkingDay,
                       StartTime = ws.StartTime,
                       EndTime = ws.EndTime,
                       BreakStartTime = ws.BreakStartTime,
                       BreakEndTime = ws.BreakEndTime
                   }).ToList()));



            CreateMap<LookupItemDto, LookupItemResponse>();
            CreateMap<CategoryItem, CategoryItemResponse>();
            CreateMap<PointSaleTypesDto, PointSaleTypesResponse>();
            CreateMap<PointSaleScheduleDto, PointSaleSchedule>();


            CreateMap<PointSaleDto, PointSaleEntity>();
            CreateMap<DescriptionDto, Description>();
            CreateMap<ChiefDto, Chief>();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<PointSaleActivityDto, PointSaleActivity>();

            CreateMap<PointSaleResultFullDto, PointSaleResultFull>();
        }

    }
}
