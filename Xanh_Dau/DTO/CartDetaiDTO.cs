namespace Xanh_Dau.DTO;

public class CartDetaiDTO
{
    public List<CartDetailProductDTO>? CartItems { get; set; }

    public decimal TotalAmount { get; set; }
}