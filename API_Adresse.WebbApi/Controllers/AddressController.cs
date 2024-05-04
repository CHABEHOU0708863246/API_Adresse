using API_Adresse.Domain.DTOs;
using API_Adresse.Services.AdressService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IO.Compression;

namespace API_Adresse.WebbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressService addressService, IMemoryCache cache, ILogger<AddressController> logger)
        {
            _addressService = addressService;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet("search/{query}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<ActionResult<List<AddressDTO>>> SearchAddresses(string query)
        {
            try
            {
                var cachedResult = await _cache.GetOrCreateAsync($"address_search_{query}", async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                    var addresses = await _addressService.GetAddressesAsync(query);

                    var json = JsonConvert.SerializeObject(addresses);
                    _logger.LogInformation($"Response JSON: {json}");

                    return addresses;
                });

                return Ok(cachedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error searching addresses: {ex.Message}");
            }
        }

        [HttpGet("geocode/{address}")]
        public async Task<ActionResult<AddressDTO>> GeocodeAddress(string address)
        {
            try
            {
                var result = await _addressService.GeocodeAddressAsync(address);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Address not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error geocoding address: {ex.Message}");
            }
        }

        [HttpGet("stream")]
        public async Task<IActionResult> StreamAddresses()
        {
            try
            {
                var addresses = await _addressService.GetAddressesAsync("Paris");

                var stream = new MemoryStream();
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    var entry = archive.CreateEntry("addresses.txt");
                    using (var entryStream = entry.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        foreach (var address in addresses)
                        {
                            await streamWriter.WriteLineAsync($"{address.Label}, {address.Postcode}, {address.City}");
                        }
                    }
                }

                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/octet-stream", "addresses.zip");
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError($"Error generating zip file: {ex.Message}");
                return StatusCode(500, $"Error generating zip file: {ex.Message}");
            }
        }
    }
}
