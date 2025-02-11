using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.Interface;
using Xanh_Dau.DTO;
using Xanh_Dau.Services;

namespace Xanh_Dau.Controllers;

public class OrderController : Controller
{
    private readonly IAddressRepository _addressRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly TokenService _tokenService;

    public OrderController(IOrderRepository orderRepository, TokenService tokenService, ICartRepository cartRepository,
        ICustomerRepository customerRepository, IAddressRepository addressRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _customerRepository = customerRepository;
        // _voucherRepository = voucherRepository;
        _tokenService = tokenService;
        _addressRepository = addressRepository;
    }

    private int GetUserId()
    {
        var token = Request.Cookies["auth_token"];
        var userId = _tokenService.GetUserIdFromToken(token);
        return Convert.ToInt32(userId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDTO orderDTO, bool UseDefaultAddress)
    {
        try
        {
            var customerId = GetUserId();
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            var cart = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);
            var address = await _addressRepository.GetAddressByIdAsync(customerId);

            if (cart?.CartDetails == null || !cart.CartDetails.Any())
            {
                TempData["Error"] = "Giỏ hàng trống";
                return RedirectToAction("CartDetail", "Home");
            }

            // Validate selected items
            var selectedItemIds = !string.IsNullOrEmpty(orderDTO.SelectedItemIds)
                ? orderDTO.SelectedItemIds.Split(',').Select(int.Parse).ToList()
                : new List<int>();

            if (!selectedItemIds.Any())
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một sản phẩm để đặt hàng";
                return RedirectToAction("CartDetail", "Home");
            }

            var selectedCartDetails = cart.CartDetails
                .Where(cd => selectedItemIds.Contains(cd.CartDetailId) && !cd.IsDeleted.Value)
                .ToList();

            if (!selectedCartDetails.Any())
            {
                TempData["Error"] = "Không tìm thấy sản phẩm đã chọn";
                return RedirectToAction("CartDetail", "Home");
            }

            // Calculate total from selected items only
            var total = selectedCartDetails.Sum(cd => cd.Product.Price * cd.Quantity);

            // Create new order
            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Status = "pending",
                ShippingAddress = UseDefaultAddress ? address.ShipAddress : orderDTO.ShippingAddress,
                Receiver =
                    orderDTO.UseDefaultAddress ? address.Receiver : orderDTO.Receiver,
                ShipPhone = orderDTO.UseDefaultAddress ? address.ShipPhone : orderDTO.ShipPhone,
                VoucherId = null,
                Subtotal = 0,
                Total = total,
                TotalPrice = total,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };

            // Create order details from selected cart items
            var orderDetails = selectedCartDetails.Select(cd => new OrderDetail
            {
                ProductId = cd.ProductId,
                Quantity = cd.Quantity,
                Price = cd.Product.Price * cd.Quantity,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            }).ToList();

            // Save order and details
            await _orderRepository.CreateOrderAsync(order, orderDetails);

            // Remove selected items from cart
            foreach (var cartDetail in selectedCartDetails)
                await _cartRepository.RemoveCartDetailAsync(cartDetail.CartDetailId);

            TempData["Success"] = "Đặt hàng thành công";
            return RedirectToAction("MyOrders");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tạo đơn hàng: {ex.Message}";
            return RedirectToAction("CartDetail", "Home");
        }
    }


    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        try
        {
            var customerId = GetUserId();
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return View(orders);
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi khi tải danh sách đơn hàng: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> OrderDetail(int orderId)
    {
        try
        {
            var customerId = GetUserId();
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            // Verify order belongs to current customer
            if (order == null || order.CustomerId != customerId) return NotFound("Không tìm thấy đơn hàng");

            return View(order);
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi khi tải chi tiết đơn hàng: {ex.Message}");
        }
    }
}