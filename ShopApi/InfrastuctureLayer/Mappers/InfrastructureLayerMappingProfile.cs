using AutoMapper;
using DomainLayer.Entities;
using InfrastuctureLayer.Data.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace InfrastuctureLayer.Mappers
{
    public class InfrastructureLayerMappingProfile : Profile
    {
        public InfrastructureLayerMappingProfile()
        {
            CreateMap<ShopEntity, ShopModel>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(
                    source => new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(source.Longitude, source.Latitude))));

            CreateMap<ShopModel, ShopEntity>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(source => source.Location.Coordinates.Y))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(source => source.Location.Coordinates.X));


            CreateMap<GeoCoordinateEntity, GeoCoordinateModel>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(
                    source => new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(source.Longitude, source.Latitude))))
                .ForMember(dest => dest.Shops, opt => opt.MapFrom(
                    source => source.Shops.Select(
                        shopEntity => new ShopModel()
                        {
                            Id = shopEntity.Id,
                            Name = shopEntity.Name,
                            Location = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(shopEntity.Longitude, shopEntity.Latitude)),
                        }
                        )));

            CreateMap<GeoCoordinateModel, GeoCoordinateEntity>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(source => source.Location.Coordinates.Y))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(source => source.Location.Coordinates.X))
                .ForMember(dest => dest.Shops, opt => opt.MapFrom(
                    source => source.Shops.Select(
                        shopModel => new ShopEntity() 
                        { 
                            Id = shopModel.Id, 
                            Name = shopModel.Name, 
                            Longitude = shopModel.Location.Coordinates.X, 
                            Latitude = shopModel.Location.Coordinates.Y,
                        }
                        )));
        }
    }
}
