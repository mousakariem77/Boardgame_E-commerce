using Models;

namespace Xanh_Dau.DTO;

public class ShopDetailDTO
{
    public Product Product { get; set; }
    public List<Feedback> Feedbacks { get; set; }
    public List<ProductImage> ListProductImages { get; set; }
}