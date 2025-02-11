using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class CartRepository : ICartRepository
{
    private readonly CartDAO _cartDAO;

    public CartRepository()
    {
        _cartDAO = CartDAO.Instance;
    }

    public async Task<Cart> GetCartItemsByCustomerIdAsync(int customerId)
    {
        return await _cartDAO.GetCartItemsByCustomerIdAsync(customerId);
    }

    public async Task<Cart> AddCartAsync(Cart cart)
    {
        return await _cartDAO.AddCartAsync(cart);
    }

    public async Task<CartDetail> GetCartDetailByCartAndProductAsync(int cartId, int productId)
    {
        return await _cartDAO.GetCartDetailByCartAndProductAsync(cartId, productId);
    }

    public async Task<bool> AddCartDetailAsync(CartDetail cartDetail)
    {
        return await _cartDAO.AddCartDetailAsync(cartDetail);
    }

    public async Task<bool> UpdateCartDetailAsync(CartDetail cartDetail)
    {
        return await _cartDAO.UpdateCartDetailAsync(cartDetail);
    }

    public async Task RemoveFromCartAsync(int cartDetailId)
    {
        await _cartDAO.RemoveFromCartAsync(cartDetailId);
    }

    public async Task<CartDetail> GetCartDetailByIdAsync(int cartDetailId)
    {
        return await _cartDAO.GetCartDetailByIdAsync(cartDetailId);
    }

    public async Task<bool> RemoveCartDetailAsync(int cartDetailId)
    {
        var cartDetail = await GetCartDetailByIdAsync(cartDetailId);
        if (cartDetail == null) return false;

        cartDetail.IsDeleted = true;
        cartDetail.DeletedAt = DateTime.Now;
        return await UpdateCartDetailAsync(cartDetail);
    }

    public async Task<bool> UpdateSelectionAsync(int cartDetailId, int customerId, bool isSelected)
    {
        return await _cartDAO.UpdateSelectionAsync(cartDetailId, customerId, isSelected);
    }

    public async Task<bool> RemoveSelectedItemsAsync(int cartId, IEnumerable<int> selectedDetailIds)
    {
        return await _cartDAO.RemoveSelectedItemsAsync(cartId, selectedDetailIds);
    }
}