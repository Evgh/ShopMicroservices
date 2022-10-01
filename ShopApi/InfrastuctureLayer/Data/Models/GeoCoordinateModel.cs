using InfrastuctureLayer.Helpers;
using MongoDB.Driver.GeoJsonObjectModel;

namespace InfrastuctureLayer.Data.Models
{
    public class GeoCoordinateModel : BaseModel
    {
        public override string Id { get => GeoCoordinateHashHelper.CountGeoHash(Location.Coordinates.X, Location.Coordinates.Y); }

        public GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }

        public List<ShopModel> Shops { get; set; }
    }
}
