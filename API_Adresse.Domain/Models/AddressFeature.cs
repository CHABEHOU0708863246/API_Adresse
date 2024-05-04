using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_Adresse.Domain.Models
{
    public class AddressFeature
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("geometry")]
        public Geometry Geometry { get; set; }

        [BsonElement("properties")]
        public Address Properties { get; set; }
    }
}
