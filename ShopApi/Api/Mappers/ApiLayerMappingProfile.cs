using Api.Contracts.Requests;
using Api.Contracts.Responces;
using AutoMapper;
using DomainLayer.Entities;

namespace Api.Mappers
{
    public class ApiLayerMappingProfile : Profile
    {
        public ApiLayerMappingProfile()
        {
            CreateMap<ShopRequest, ShopEntity>();
            CreateMap<CreateShopRequest, ShopEntity>();
            CreateMap<ShopEntity, ShopResponce>();

            CreateMap<GeoCoordinateRequest, GeoCoordinateEntity>();
            CreateMap<GeoCoordinateEntity, GeoCoordinateResponce>()
                .ForMember(dest => dest.Shops, opt => opt.MapFrom(source => source.Shops));
        }
    }
}
