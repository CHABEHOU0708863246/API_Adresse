using MongoDB.Bson.Serialization.Attributes;

namespace API_Adresse.Domain.Models
{
    public class Geometry
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("coordinates")]
        public List<double> Coordinates { get; set; }
    }
}
