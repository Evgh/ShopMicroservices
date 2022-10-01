using InfrastuctureLayer.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace InfrastuctureLayer.Data.Models
{
    public class ShopModel : BaseModel
    {
        public string? Name { get; set; }
        public GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string LocationId => GeoCoordinateHashHelper.CountGeoHash(Location.Coordinates.X, Location.Coordinates.Y);
    }
}
