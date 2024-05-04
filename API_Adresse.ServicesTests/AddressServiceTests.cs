using API_Adresse.Domain.DTOs;
using API_Adresse.Domain.Models;
using API_Adresse.Services.AdressService;
using MongoDB.Driver;
using Moq;
using Moq.Protected;

namespace API_Adresse.Tests
{
    [TestClass]
    public class AddressServiceTests
    {
        private readonly HttpClient _mockHttpClient;
        private readonly Mock<IMongoDatabase> _mockDatabase;

        public AddressServiceTests()
        {
            _mockHttpClient = new HttpClient();
            _mockDatabase = new Mock<IMongoDatabase>();
        }

        [TestMethod]
        public async Task GetAddressesAsync_ShouldRetrieveAndStoreAddresses()
        {
            // Arrange
            var expectedAddresses = new List<AddressDTO>
            {
                new AddressDTO { Label = "Address 1", Postcode = "12345", City = "City 1" },
                new AddressDTO { Label = "Address 2", Postcode = "67890", City = "City 2" }
            };

            var mockResponseContent = new StringContent("{\"features\": [{\"properties\": {\"label\": \"Address 1\", \"postcode\": \"12345\", \"city\": \"City 1\"}}, {\"properties\": {\"label\": \"Address 2\", \"postcode\": \"67890\", \"city\": \"City 2\"}}]}");
            _mockHttpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");

            var mockResponse = new HttpResponseMessage();
            mockResponse.Content = mockResponseContent;
            mockResponse.StatusCode = System.Net.HttpStatusCode.OK;

            var mockHttpHandler = new Mock<HttpMessageHandler>();
            mockHttpHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            var httpClient = new HttpClient(mockHttpHandler.Object);

            var mockCollection = MockCollection();
            _mockDatabase.Setup(x => x.GetCollection<Address>("addresses", null)).Returns(mockCollection);

            var addressService = new AddressService(httpClient, _mockDatabase.Object);

            // Act
            var result = await addressService.GetAddressesAsync("query");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAddresses.Count, result.Count);
        }

        private IMongoCollection<Address> MockCollection()
        {
            var addresses = new List<Address>();
            var mockCollection = new Mock<IMongoCollection<Address>>();

            mockCollection.Setup(x => x.InsertOneAsync(It.IsAny<Address>(), null, default)).Callback<Address, InsertOneOptions, System.Threading.CancellationToken>((a, b, c) => addresses.Add(a));

            return mockCollection.Object;
        }
    }
}
