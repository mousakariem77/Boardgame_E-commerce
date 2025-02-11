using Models;

namespace Xanh_Dau.DTO;

public class CartDetailProductDTO
{
    public int CartDetailID { get; set; }
    public int CartID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
    public bool IsSelected { get; set; }
}