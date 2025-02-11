using Models;

namespace Repository.Interface;

public interface IAddressRepository
{
    Task<List<Address>> GetAllAddressesAsync();

    Task<Address> GetAddressByIdAsync(long addressId);

    Task<Address> CreateAddressAsync(Address address);

    Task<Address> UpdateAddressAsync(Address address);

    Task DeleteAddressAsync(long addressId);
    Task<Address> GetDefaultAddressAsync(int customerId);
    Task<bool> ResetDefaultAddressesAsync(int customerId);
    Task SetDefaultAddress(int addressId, int customerId);
    Task<List<Address>> GetAddressesByCustomerIdAsync(int customerId);
}