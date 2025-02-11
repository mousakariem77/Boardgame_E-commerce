using Models;

namespace Xanh_Dau.DTO;

public class AdminDTO
{
    public Admin Admin { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public List<Admin> ListAdmin { get; set; } = new();
} 