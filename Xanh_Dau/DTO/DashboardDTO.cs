using Models;

namespace Xanh_Dau.DTO;

public class DashboardDTO
{
    public List<Product> ListProducts { get; set; } = new();
    public List<Customer> ListCustomers { get; set; } = new();
    public List<Order> ListOrders { get; set; } = new();

    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
}