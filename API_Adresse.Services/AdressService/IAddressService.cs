using API_Adresse.Domain.DTOs;

namespace API_Adresse.Services.AdressService
{
    public interface IAddressService
    {
        Task<List<AddressDTO>> GetAddressesAsync(string query);
    }
}
