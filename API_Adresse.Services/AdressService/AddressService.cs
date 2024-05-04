using API_Adresse.Domain.DTOs;
using API_Adresse.Domain.Models;
using MongoDB.Driver;
using System.Text.Json;

namespace API_Adresse.Services.AdressService
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;
        private readonly IMongoCollection<Address> _addressesCollection;

        public AddressService(HttpClient httpClient, IMongoDatabase database)
        {
            _httpClient = httpClient;
            _addressesCollection = database.GetCollection<Address>("addresses");
        }

        public async Task<List<AddressDTO>> GetAddressesAsync(string query)
        {
            var apiUrl = $"https://api-adresse.data.gouv.fr/search/?q={query}";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(jsonString);
            var features = jsonObject.RootElement.GetProperty("features");

            var addresses = new List<AddressDTO>();
            foreach (var feature in features.EnumerateArray())
            {
                var properties = feature.GetProperty("properties");
                var label = properties.GetProperty("label").GetString();
                var postcode = properties.GetProperty("postcode").GetString();
                var city = properties.GetProperty("city").GetString();

                addresses.Add(new AddressDTO
                {
                    Label = label,
                    Postcode = postcode,
                    City = city
                });

                // Enregistrer l'adresse dans MongoDB
                var addressModel = new Address
                {
                    Label = label,
                    Postcode = postcode,
                    City = city
                };
                await _addressesCollection.InsertOneAsync(addressModel);
            }

            return addresses;
        }
    }
}
