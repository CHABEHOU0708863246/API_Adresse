using MongoDB.Bson.Serialization.Attributes;

namespace API_Adresse.Domain.Models
{
    public class AddressResponse
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("features")]
        public List<AddressFeature> Features { get; set; }

        [BsonElement("attribution")]
        public string Attribution { get; set; }

        [BsonElement("licence")]
        public string Licence { get; set; }

        [BsonElement("query")]
        public string Query { get; set; }

        [BsonElement("limit")]
        public int Limit { get; set; }
    }
}
