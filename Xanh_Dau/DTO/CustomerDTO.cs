using Models;

namespace Xanh_Dau.DTO;

public class CustomerDTO
{
    public Customer Customer { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public List<Customer> ListCustomer { get; set; } = new();
} 