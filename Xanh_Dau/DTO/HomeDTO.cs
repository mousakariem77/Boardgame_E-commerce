using Models;

namespace Xanh_Dau.DTO;

public class HomeDTO
{
    public List<Product> ListProduct { get; set; } = new();
    public List<Category> Categories { get; set; }
}