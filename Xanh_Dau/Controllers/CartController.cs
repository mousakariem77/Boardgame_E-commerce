using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.Interface;
using Xanh_Dau.Services;

namespace Xanh_Dau.Controllers;

public class CartController : Controller
{
    private readonly ICartRepository _cartRepository;
    private readonly TokenService _tokenService;

    public CartController(ICartRepository cartRepository, TokenService tokenService)
    {
        _cartRepository = cartRepository;
        _tokenService = tokenService;
    }

    private int getUserId()
    {
        var token = Request.Cookies["auth_token"];

        var userId = _tokenService.GetUserIdFromToken(token);

        return Convert.ToInt32(userId);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        try
        {
            var customerId = getUserId();

            var cart = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);
            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                };
                cart = await _cartRepository.AddCartAsync(cart);
            }

            var cartDetail = await _cartRepository.GetCartDetailByCartAndProductAsync(cart.CartId, productId);
            if (cartDetail != null)
            {
                cartDetail.Quantity += quantity;
                cartDetail.UpdatedAt = DateTime.Now;
                await _cartRepository.UpdateCartDetailAsync(cartDetail);
            }
            else
            {
                var newCartDetail = new CartDetail
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                };
                await _cartRepository.AddCartDetailAsync(newCartDetail);
            }

            TempData["Success"] = "Sản phẩm đã được thêm vào giỏ hàng!";
            return RedirectToAction("ShopDetail", "Home", new { productId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Có lỗi xảy ra khi thêm vào giỏ hàng.";
            return RedirectToAction("ShopDetail", "Home", new { productId });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartDetailId, int quantity)
    {
        try
        {
            if (quantity <= 0)
            {
                await _cartRepository.RemoveCartDetailAsync(cartDetailId);
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng";
            }
            else
            {
                var cartDetail = await _cartRepository.GetCartDetailByIdAsync(cartDetailId);
                if (cartDetail != null)
                {
                    cartDetail.Quantity = quantity;
                    cartDetail.UpdatedAt = DateTime.Now;

                    if (!await _cartRepository.UpdateCartDetailAsync(cartDetail))
                        TempData["Error"] = "Số lượng vượt quá số lượng trong kho";
                }
            }
        }
        catch (Exception)
        {
            TempData["Error"] = "Có lỗi xảy ra khi cập nhật giỏ hàng";
        }

        return RedirectToAction("CartDetail", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSelection(int cartDetailId, bool isSelected)
    {
        try
        {
            var customerId = getUserId();
            await _cartRepository.UpdateSelectionAsync(cartDetailId, customerId, isSelected);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddToCartAndCheckout(int productId, int quantity)
    {
        var result = await AddToCart(productId, quantity);
        if (result is RedirectToActionResult redirect &&
            redirect.ActionName == "ShopDetail") return redirect; // Trả về trang detail nếu có lỗi
        return RedirectToAction("CartDetail", "Home"); // Chuyển đến trang thanh toán
    }
}