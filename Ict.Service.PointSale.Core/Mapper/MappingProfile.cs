using AutoMapper;
using Ict.Service.PointSale.API.Abstractions.Models.PointSale;
using Ict.Service.PointSale.API.Abstractions.Models.References;
using Ict.Service.PointSale.API.Abstractions.Models.Schedule;
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
                    OwnerName = src.OwnerName,
                    CreationDate = src.CreationDatePointSale,
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
                    EntryDate = DateTime.Now
                }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.DescriptionText) ? new DescriptionDto
                {
                    DescriptionId = Guid.NewGuid(),
                    DescriptionText = src.DescriptionText,
                    OpenDate = src.OpenDateDescription,
                    PointSaleId = Guid.Empty,
                    EntryDate = DateTime.Now
                } : null))
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds))

               .ForMember(dest => dest.Chief, opt => opt.MapFrom(src =>
                src.ChiefName != null && src.ChiefPositionId != null
                    ? new ChiefDto
                    {
                    ChiefId = Guid.NewGuid(),
                    OpenDate = src.OpenDateChief,
                    ChiefName = src.ChiefName ?? string.Empty,
                    ChiefPositionId = src.ChiefPositionId,
                    PointSaleId = Guid.Empty,
                    EntryDate = DateTime.Now
                    }
                    : null))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => 
                src.Latitude != null && src.Longitude != null && src.Address != null
                ? new LocationDto
                { LocationId = Guid.NewGuid(),
                    OpenDate = src.OpenDateLocation,
                    Latitude = src.Latitude, 
                    Longitude = src.Longitude, 
                    Address = src.Address, 
                    AddressId = src.LocationId, 
                    PointSaleId = Guid.Empty,
                    EntryDate = DateTime.Now 
                } 
                : null))
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

            CreateMap<PointSaleFullInfoRequest, PointSaleFullDto>()
          .ForMember(dest => dest.PointSale, opt => opt.MapFrom(src => new PointSaleDto
          {
              // For updating, PointSaleId comes from the request
              PointSaleId = src.PointSaleId,
              OwnerId = src.OwnerId,
              OwnerTypeId = src.OwnerTypeId,
              OwnerName = src.OwnerName, // Added OwnerName from request
              CreationTypeId = src.CreationTypeId,
              OrganizationTypeId = src.OrganizationTypeId,
              EntryDate = DateTime.Now, // Or DateTime.UtcNow for consistency
              ClosingStatusId = null, // Not provided in request, assuming null for update or ignore
              ClosingDate = null // Not provided in request, assuming null for update or ignore
          }))
          .ForMember(dest => dest.PointSaleActivity, opt => opt.MapFrom(src => new PointSaleActivityDto
          {
              // For updating, PointSaleActivityId comes from the request
              PointSaleActivityId = src.PointSaleActivityId,
              NamePointSale = src.NamePointSale,
              EnglishNamePointSale = src.EnglishNamePointSale,
              PointSaleId = src.PointSaleId, // PointSaleId is now known from the request
              OpenDate = src.OpenDatePointSale,
              EntryDate = DateTime.Now // Or DateTime.UtcNow
          }))
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionText != null ? new DescriptionDto
          {
              // For updating, DescriptionId comes from the request
              DescriptionId = src.DescriptionId,
              DescriptionText = src.DescriptionText,
              OpenDate = src.OpenDatePointSale,
              PointSaleId = src.PointSaleId, // PointSaleId is now known from the request
              EntryDate = DateTime.Now // Or DateTime.UtcNow
          } : null))
.ForMember(dest => dest.Chief, opt => opt.MapFrom(src =>
    src.ChiefId != Guid.Empty ? new ChiefDto
    {
        ChiefId = src.ChiefId,
        OpenDate = src.OpenDateChif,
        ChiefName = src.ChiefName ?? string.Empty,
        ChiefPositionId = src.ChiefPositionId,
        PointSaleId = src.PointSaleId,
        EntryDate = DateTime.Now
    } : null))
.ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
    !string.IsNullOrEmpty(src.LocationId) ? new LocationDto
    {
        LocationId = Guid.Parse(src.LocationId),
        OpenDate = src.OpenDateLocation,
        Latitude = src.Latitude,
        Longitude = src.Longitude,
        Address = src.Address,
        AddressId = src.LocationId ?? string.Empty,
        PointSaleId = src.PointSaleId,
        EntryDate = DateTime.Now
    } : null))
          .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds))
          .ForMember(dest => dest.AlternativeName, opt => opt.MapFrom(src => src.AlternativeName)) // Added AlternativeName
          .ForMember(dest => dest.PointSaleSchedules, opt => opt.MapFrom(src => src.WorkSchedule.Select(ws => new PointSaleScheduleDto
          {
              PointSaleScheduleId = Guid.NewGuid(), // Generate new for each schedule item
              PointSaleId = src.PointSaleId, // PointSaleId is now directly available from the request
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


            CreateMap<PointSaleScheduleDto, WorkScheduleRequest>();

            CreateMap<PointSaleResultFullDto, PointSaleResultFull>()
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules ?? new List<PointSaleScheduleDto>()));




            CreateMap<PointSaleFullDto, PointSaleFullInfoRequest>()
            // Map properties from PointSaleDto
            .ForMember(dest => dest.PointSaleId, opt => opt.MapFrom(src => src.PointSale.PointSaleId))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.PointSale.OwnerId))
            .ForMember(dest => dest.OwnerTypeId, opt => opt.MapFrom(src => src.PointSale.OwnerTypeId))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.PointSale.OwnerName))
            .ForMember(dest => dest.CreationTypeId, opt => opt.MapFrom(src => src.PointSale.CreationTypeId))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.PointSale.CreationDate))
            .ForMember(dest => dest.OrganizationTypeId, opt => opt.MapFrom(src => src.PointSale.OrganizationTypeId))
            // Map properties from PointSaleActivityDto
            .ForMember(dest => dest.PointSaleActivityId, opt => opt.MapFrom(src => src.PointSaleActivity.PointSaleActivityId))
            .ForMember(dest => dest.NamePointSale, opt => opt.MapFrom(src => src.PointSaleActivity.NamePointSale))
            .ForMember(dest => dest.EnglishNamePointSale, opt => opt.MapFrom(src => src.PointSaleActivity.EnglishNamePointSale))
            .ForMember(dest => dest.OpenDatePointSale, opt => opt.MapFrom(src => src.PointSaleActivity.OpenDate))
            // Map properties from DescriptionDto
            .ForMember(dest => dest.DescriptionId, opt => opt.MapFrom(src => src.Description != null ? src.Description.DescriptionId : Guid.Empty))
            .ForMember(dest => dest.DescriptionText, opt => opt.MapFrom(src => src.Description != null ? src.Description.DescriptionText : null))
            // Map properties from ChiefDto
            .ForMember(dest => dest.ChiefId, opt => opt.MapFrom(src => src.Chief != null ? src.Chief.ChiefId : Guid.Empty))
            .ForMember(dest => dest.ChiefName, opt => opt.MapFrom(src => src.Chief != null ? src.Chief.ChiefName : null))
            .ForMember(dest => dest.ChiefPositionId, opt => opt.MapFrom(src => src.Chief != null ? src.Chief.ChiefPositionId : 0))
            .ForMember(dest => dest.OpenDateChif, opt => opt.MapFrom(src => src.Chief != null ? src.Chief.OpenDate : default(DateOnly)))
            // Map properties from LocationDto
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationId : Guid.Empty))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location != null ? src.Location.Address : null))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location != null ? src.Location.Latitude : 0f))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location != null ? src.Location.Longitude : 0f))
            .ForMember(dest => dest.OpenDateLocation, opt => opt.MapFrom(src => src.Location != null ? src.Location.OpenDate : default(DateOnly)))
            // Map lists
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds))
            .ForMember(dest => dest.AlternativeName, opt => opt.MapFrom(src => src.AlternativeName))
            .ForMember(dest => dest.WorkSchedule, opt => opt.MapFrom(src => src.PointSaleSchedules));

            // Map PointSaleScheduleDto to WorkScheduleRequest
            CreateMap<PointSaleScheduleDto, WorkScheduleRequest>();

        }

    }
}
