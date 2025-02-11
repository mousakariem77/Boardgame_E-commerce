using Models;

namespace Xanh_Dau.DTO;

public class CategoryDTO
{
    public Category Category { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    
    public List<Category> ListCategories { get; set; } = new();
}