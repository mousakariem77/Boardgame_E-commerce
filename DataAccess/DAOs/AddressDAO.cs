using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class AddressDAO : SingletonBase<AddressDAO>
{
    public async Task<List<Address>> GetAllAddressesAsync()
    {
        return await _context.Addresses.ToListAsync();
    }

    public async Task<Address> GetAddressByIdAsync(long addressId)
    {
        return await _context.Addresses.Where(a => a.IsDefault == true).FirstOrDefaultAsync();
    }

    public async Task<Address> CreateAddressAsync(Address address)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));


        var hasDefaultAddress = await _context.Addresses
            .Where(a => a.CustomerId == address.CustomerId && (bool)a.IsDefault)
            .AnyAsync();

        address.IsDefault = !hasDefaultAddress;

        address.CreatedAt = DateTime.Now;
        address.CreatedBy = address.CustomerId;
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
        return address;
    }

    public async Task<Address> UpdateAddressAsync(Address address)
    {
        try
        {
            // Tìm địa chỉ hiện tại
            var existingAddress = await _context.Addresses.FindAsync(address.AddressId);

            if (existingAddress == null)
            {
                // Nếu không tìm thấy địa chỉ, trả về null
                Console.WriteLine($"Address with ID {address.AddressId} not found.");
                return null;
            }

            // Cập nhật các trường cần thiết
            existingAddress.CustomerId = address.CustomerId; // Mã khách hàng
            existingAddress.Receiver = address.Receiver; // Tên người nhận
            existingAddress.ShipAddress = address.ShipAddress; // Địa chỉ giao hàng
            existingAddress.ShipPhone = address.ShipPhone; // Số điện thoại người nhận
            existingAddress.IsDefault = address.IsDefault; // Địa chỉ mặc định
            existingAddress.UpdatedBy = address.UpdatedBy; // Người cập nhật
            existingAddress.UpdatedAt = DateTime.Now; // Thời gian cập nhật

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingAddress;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating address: {ex.Message}");
            return null;
        }
    }


    public async Task DeleteAddressAsync(long addressId)
    {
        var address = await _context.Addresses.FindAsync(addressId);
        if (address != null)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<Address> GetDefaultAddressAsync(int customerId)
    {
        try
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.CustomerId == customerId && a.IsDefault.Value && !a.IsDeleted.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting default address: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> ResetDefaultAddressesAsync(int customerId)
    {
        try
        {
            var addresses = await _context.Addresses
                .Where(a => a.CustomerId == customerId && !a.IsDeleted.Value)
                .ToListAsync();

            foreach (var address in addresses)
            {
                address.IsDefault = false;
                address.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resetting default addresses: {ex.Message}");
            return false;
        }
    }

    public async Task SetDefaultAddress(int addressId, int customerId)
    {
        try
        {
            var addresses = await _context.Addresses
                .Where(a => a.CustomerId == customerId && (bool)!a.IsDeleted)
                .ToListAsync();

            foreach (var address in addresses)
            {
                address.IsDefault = (address.AddressId == addressId);
                address.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting default address: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Address>> GetAddressesByCustomerIdAsync(int customerId)
    {
        try
        {
            var addresses = await _context.Addresses
                .Where(a => a.CustomerId == customerId && !a.IsDeleted.Value)
                .ToListAsync();

            return addresses ?? new List<Address>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting addresses: {ex.Message}");
            return new List<Address>();
        }
    }
}