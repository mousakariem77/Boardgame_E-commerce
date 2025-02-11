using Models;

namespace Xanh_Dau.DTO;

public class ProfileDTO
{
    public Customer Customer { get; set; }

    public Address Address { get; set; }

    public List<Address> ListAddresses { get; set; } = new();
}