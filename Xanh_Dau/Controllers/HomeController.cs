using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.Interface;
using Xanh_Dau.DTO;
using Xanh_Dau.Services;

namespace Xanh_Dau.Controllers;

[Authorize] // Default authentication requirement
public class HomeController : Controller
{
    private readonly ICartRepository _cartRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly FileService _fileService;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly TokenService _tokenService;

    public HomeController(IProductRepository productRepository, FileService fileService, ICartRepository cartRepository,
        TokenService tokenService, IOrderRepository orderRepository, IFeedbackRepository feedbackRepository,
        ICategoryRepository categoryRepository, IProductImageRepository __productImageRepository)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _tokenService = tokenService;
        _orderRepository = orderRepository;
        _feedbackRepository = feedbackRepository;
        _fileService = fileService;
        _categoryRepository = categoryRepository;
        _productImageRepository = __productImageRepository;
    }

    private int getUserId()
    {
        var token = Request.Cookies["auth_token"];

        var userId = _tokenService.GetUserIdFromToken(token);

        return Convert.ToInt32(userId);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var homeDTO = new HomeDTO();
        homeDTO.ListProduct = await _productRepository.GetAllProductsAsync();
        foreach (var product in homeDTO.ListProduct)
        {
            var (reviewCount, averageRating) = await GetProductReviewStats(product.ProductId);
            product.ReviewCount = reviewCount;
            product.AverageRating = averageRating;
        }

        return View(homeDTO);
    }

    public IActionResult About()
    {
        return View();
    }

    private async Task<(int reviewCount, double averageRating)> GetProductReviewStats(int productId)
    {
        var feedbacks = await _feedbackRepository.GetProductFeedbacksAsync(productId);

        if (feedbacks == null || !feedbacks.Any())
            return (0, 0);

        var reviewCount = feedbacks.Count;
        // Explicitly convert feedback_Rate to double before averaging
        var averageRating = feedbacks.Average(f => (double)f.FeedbackRate);

        return (reviewCount, averageRating);
    }

    public async Task<IActionResult> Shop(
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string searchTerm = null,
        string sortBy = null)
    {
        var products = await _productRepository.GetFilteredProductsAsync(
            categoryId, minPrice, maxPrice, searchTerm, sortBy);

        // Calculate review statistics for each product
        foreach (var product in products)
        {
            var (reviewCount, averageRating) = await GetProductReviewStats(product.ProductId);
            product.ReviewCount = reviewCount;
            product.AverageRating = averageRating;
        }

        var shopDTO = new HomeDTO
        {
            Categories = await _categoryRepository.GetAllCategoriesAsync(),
            ListProduct = products
        };

        // Store filter parameters for the view
        ViewBag.CurrentCategoryId = categoryId;
        ViewBag.CurrentMinPrice = minPrice;
        ViewBag.CurrentMaxPrice = maxPrice;
        ViewBag.CurrentSearchTerm = searchTerm;
        ViewBag.CurrentSortBy = sortBy;

        return View(shopDTO);
    }


    public async Task<IActionResult> ShopDetail(int productId)
    {
        var shopDetailDTO = new ShopDetailDTO
        {
            Product = await _productRepository.GetProductByIdAsync(productId),
            Feedbacks = await _feedbackRepository.GetProductFeedbacksAsync(productId),
            ListProductImages = await _productImageRepository.GetAllProductImagesAsync() // Add this line
        };
        return View(shopDetailDTO);
    }

    public IActionResult Blog()
    {
        return View();
    }

    public IActionResult Event()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult FAQs()
    {
        return View();
    }

    public async Task<IActionResult> CartDetail()
    {
        var customerId = getUserId(); // Lấy ID người dùng
        var cart = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);

        // Kiểm tra xem giỏ hàng có tồn tại không
        if (cart == null)
            return View(new CartDetaiDTO { CartItems = new List<CartDetailProductDTO>(), TotalAmount = 0 });

        var cartItems = cart.CartDetails
            .Where(cd => !cd.IsDeleted.Value)
            .Select(cd => new CartDetailProductDTO
            {
                CartDetailID = cd.CartDetailId,
                CartID = cd.CartId,
                ProductID = cd.ProductId,
                Quantity = cd.Quantity,
                Product = cd.Product
            }).ToList();

        var viewModel = new CartDetaiDTO
        {
            CartItems = cartItems,
            TotalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity)
        };

        return View(viewModel);
    }


    public async Task<IActionResult> OrderSuccess(int orderId)
    {
        try
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound();
            return View(order);
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi khi tải thông tin đơn hàng: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddFeedback(CreateFeedbackDTO model)
    {
        try
        {
            var userId = getUserId();
            if (userId == 0)
            {
                TempData["Error"] = "Vui lòng đăng nhập để đánh giá sản phẩm";
                return RedirectToAction("ShopDetail", new { productId = model.ProductId });
            }

            var feedback = new Feedback
            {
                ProductId = model.ProductId,
                CustomerId = userId,
                FeedbackRate = model.Rating,
                FeedbackComment = model.Comment,
                CreateAt = DateTime.Now,
                IsDelete = false
            };

            // Create feedback
            feedback = await _feedbackRepository.CreateFeedbackAsync(feedback);

            // Handle images
            if (model.Images?.Length > 0)
            {
                var feedbackImages = new List<FeedbackImage>();
                foreach (var image in model.Images)
                {
                    // Use the new FileService to upload image to Azure Blob Storage
                    var imageUrl = await _fileService.UploadImageAsync(image);
                    feedbackImages.Add(new FeedbackImage
                    {
                        FeedbackId = feedback.FeedbackId,
                        FeedbackImage1 = imageUrl, // Store the URL returned from Azure Blob Storage
                        CreateAt = DateTime.Now,
                        IsDelete = false
                    });
                }

                await _feedbackRepository.AddFeedbackImagesAsync(feedbackImages);
            }

            TempData["Success"] = "Cảm ơn bạn đã đánh giá sản phẩm!";
            return RedirectToAction("ShopDetail", new { productId = model.ProductId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Có lỗi xảy ra khi gửi đánh giá";
            return RedirectToAction("ShopDetail", new { productId = model.ProductId });
        }
    }

    // Thêm action để xóa feedback
    [HttpPost]
    public async Task<IActionResult> DeleteFeedback(int feedbackId, int productId)
    {
        var userId = getUserId();
        if (userId == 0) return Json(new { success = false, message = "Unauthorized" });

        var success = await _feedbackRepository.DeleteFeedbackAsync(feedbackId, userId);
        return Json(new { success });
    }
}