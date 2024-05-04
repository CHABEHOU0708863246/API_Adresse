namespace API_Adresse.Domain.DTOs
{
    public class AddressDTO
    {
        public string Label { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
