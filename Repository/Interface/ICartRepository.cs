using Models;

namespace Repository.Interface;

public interface ICartRepository
{
    Task<Cart> GetCartItemsByCustomerIdAsync(int customerId);
    Task<Cart> AddCartAsync(Cart cart);
    Task<CartDetail> GetCartDetailByCartAndProductAsync(int cartId, int productId);
    Task<bool> AddCartDetailAsync(CartDetail cartDetail);
    Task<bool> UpdateCartDetailAsync(CartDetail cartDetail);
    Task RemoveFromCartAsync(int cartDetailId);
    Task<CartDetail> GetCartDetailByIdAsync(int cartDetailId);
    Task<bool> RemoveCartDetailAsync(int cartDetailId);
    Task<bool> RemoveSelectedItemsAsync(int cartId, IEnumerable<int> selectedDetailIds);
    Task<bool> UpdateSelectionAsync(int cartDetailId, int customerId, bool isSelected);
}