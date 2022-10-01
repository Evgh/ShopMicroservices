using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InfrastuctureLayer.Data.Models
{
    public abstract class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}
