using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_Adresse.Domain.Models
{
    public class Address
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("label")]
        public string Label { get; set; }

        [BsonElement("score")]
        public double Score { get; set; }

        [BsonElement("housenumber")]
        public string HouseNumber { get; set; }

        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("locality")]
        public string Locality { get; set; }

        [BsonElement("municipality")]
        public string Municipality { get; set; }

        [BsonElement("postcode")]
        public string Postcode { get; set; }

        [BsonElement("citycode")]
        public string CityCode { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("district")]
        public string District { get; set; }

        [BsonElement("oldcitycode")]
        public string OldCityCode { get; set; }

        [BsonElement("oldcity")]
        public string OldCity { get; set; }

        [BsonElement("context")]
        public string Context { get; set; }

        [BsonElement("x")]
        public double X { get; set; }

        [BsonElement("y")]
        public double Y { get; set; }

        [BsonElement("importance")]
        public double Importance { get; set; }
    }
}
