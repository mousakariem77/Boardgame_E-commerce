using Models;

namespace Xanh_Dau.DTO;

public class ProductDTO
{
    public Category Category { get; set; }
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int ReviewCount { get; set; }
    public double AverageRating { get; set; }
    public List<Product> ListProducts { get; set; } = new();
    public List<Category> ListCategories { get; set; } = new();
    public List<ProductImage> ListProductImages { get; set; } = new();
}