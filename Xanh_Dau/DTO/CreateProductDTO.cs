using Models;

namespace Xanh_Dau.DTO;

public class CreateProductDTO
{
    public Product Product { get; set; }

    public List<Category> ListCategories { get; set; } = new();

    public int? CategoryId { get; set; }
}