using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.Interface;
using Xanh_Dau.DTO;
using Xanh_Dau.Services;

namespace Xanh_Dau.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly FileService _fileService;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductImageRepository _productImageRepository;
    private readonly IProductRepository _productRepository;
    private readonly TokenService _tokenService;

    public AdminController(
        IAdminRepository adminRepository,
        TokenService tokenService,
        ICustomerRepository customerRepository,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProductImageRepository productImageRepository,
        IOrderRepository orderRepository,
        FileService fileService)
    {
        _adminRepository = adminRepository;
        _tokenService = tokenService;
        _customerRepository = customerRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _productImageRepository = productImageRepository;
        _orderRepository = orderRepository;
        _fileService = fileService;
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
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string username, string password)
    {
        var user = await _adminRepository.GetAdminByUsernameAsync(username);

        if (user == null || user.Password != password)
        {
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // Create token for the user
        var tokenString = await _tokenService.CreateTokenAdminAsync(Convert.ToInt32(user.AdminId));

        Console.WriteLine(tokenString);

        Response.Cookies.Append("auth_token", tokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.Now.AddHours(24),
            SameSite = SameSiteMode.Strict
        });

        return RedirectToAction("DashBoard", "Admin");
    }

    public async Task<IActionResult> Dashboard()
    {
        var products = await _productRepository.GetAllProductsAsync();
        var customers = await _customerRepository.GetListCustomerAsync();
        var orders = await _orderRepository.GetOrders();

        var totalRevenue = orders
            .Where(o => o.Status == "completed")
            .Sum(o => o.TotalPrice);

        var totalOrders = orders
            .Count(o => o.Status != "cancelled");

        var dashboardDTO = new DashboardDTO
        {
            ListProducts = products,
            ListCustomers = customers,
            ListOrders = orders,
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders
        };

        return View(dashboardDTO);
    }

    [HttpGet]
    public async Task<IActionResult> Customer(int page = 1, int pageSize = 6)
    {
        var (customers, totalCount) = await _customerRepository.GetPaginationCustomersAsync(page, pageSize);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var customerDto = new CustomerDTO
        {
            ListCustomer = customers,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        return View(customerDto);
    }

    public async Task<IActionResult> CustomerDetail(int customerId)
    {
        var customerDTO = new CustomerDTO();
        customerDTO.Customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        return View(customerDTO);
    }

    public async Task<IActionResult> Manage(int page = 1, int pageSize = 6)
    {
        var (admins, totalCount) = await _adminRepository.GetPaginationAdminsAsync(page, pageSize);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var adminsDto = new AdminDTO
        {
            ListAdmin = admins,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        // var staff = admins.Where(c => c.Role == "staff").ToList();

        return View(adminsDto);
    }

    [HttpPost]
    [Route("CreateAdmin")]
    public async Task<IActionResult> CreateAdmin(Admin admin, IFormFile avatar)
    {
        System.Console.WriteLine(1);
        if (!ModelState.IsValid){
        System.Console.WriteLine(2);
            return RedirectToAction("Manage", "Admin");
        }
        

        if (avatar == null || avatar.Length == 0)
        {
            TempData["Error"] = "File ảnh không hợp lệ.";
            System.Console.WriteLine(3);
            return RedirectToAction("Manage", "Admin");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        if (!allowedExtensions.Contains(Path.GetExtension(avatar.FileName).ToLower()))
        {
            TempData["Error"] = "Chỉ hỗ trợ định dạng JPG, JPEG, PNG, GIF.";
            System.Console.WriteLine(4);
            return RedirectToAction("Manage", "Admin");
        }

        try
        {
            admin.CreatedAt = DateTime.Now;
            System.Console.WriteLine(admin.FirstName);
            System.Console.WriteLine(admin.LastName);
            System.Console.WriteLine(admin.Email);
            System.Console.WriteLine(admin.Username);
            System.Console.WriteLine(admin.Password);
            System.Console.WriteLine(admin.Picture);
            System.Console.WriteLine(admin.Role);
            System.Console.WriteLine(admin.Status);


            var newAdmin = await _adminRepository.CreateAdminAsync(admin);
            if (newAdmin == null)
            {
                TempData["Error"] = "Không thể tạo tài khoản.";
                System.Console.WriteLine(5);
                return RedirectToAction("Manage", "Admin");
            }

            newAdmin.Picture = await _fileService.UploadImageAsync(avatar);
            await _adminRepository.UpdateAdminAsync(newAdmin);

            TempData["Success"] = "Tạo tài khoản thành công!";
            return RedirectToAction("Manage", "Admin");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi: {ex.Message}";
            System.Console.WriteLine(6);
            return RedirectToAction("Manage", "Admin");
        }
    }

    public async Task<IActionResult> ManageDetail(int adminId)
    {
        var adminDTO = new AdminDTO();
        adminDTO.Admin = await _adminRepository.GetAdminByIdAsync(adminId);
        return View(adminDTO);
    }

    [HttpGet]
    public async Task<IActionResult> Product(int page = 1, int pageSize = 6)
    {
        var (products, totalCount) = await _productRepository.GetPaginationProductsAsync(page, pageSize);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var productDto = new ProductDTO
        {
            ListProducts = products,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        return View(productDto);
    }


    public async Task<IActionResult> ProductDetail(int productId)
    {
        var productDTO = new ProductDTO();
        productDTO.Product = await _productRepository.GetProductByIdAsync(productId);
        productDTO.ListProductImages = await _productImageRepository.GetAllProductImagesAsync();
        return View(productDTO);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        var productDTO = new ProductDTO();
        var category = await _categoryRepository.GetAllCategoriesAsync();
        productDTO.ListCategories = category;
        return View(productDTO);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductDTO productDto, List<IFormFile> images)
    {
        if (productDto == null)
        {
            Console.WriteLine("ProductDto is null");
            return View(productDto);
        }

        try
        {
            // Gắn thông tin tạo sản phẩm
            productDto.Product.CreatedAt = DateTime.Now;
            productDto.Product.CreatedBy = getUserId();

            // Lưu sản phẩm vào cơ sở dữ liệu
            var createdProduct = await _productRepository.CreateProductAsync(productDto.Product);

            if (createdProduct != null && images != null && images.Any())
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var productImages = new List<ProductImage>();

                foreach (var image in images)
                {
                    if (image == null || image.Length == 0)
                        continue;

                    var fileExtension = Path.GetExtension(image.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                        continue;

                    // Upload hình ảnh và lấy URL
                    var imageUrl = await _fileService.UploadImageAsync(image);

                    // Tạo đối tượng ProductImage
                    productImages.Add(new ProductImage
                    {
                        ProductId = createdProduct.ProductId,
                        ImageUrl = imageUrl,
                        IsPrimary = productImages.Count == 0, // Đặt ảnh đầu tiên là ảnh chính
                        ArrangeOrder = productImages.Count + 1 // Sắp xếp theo thứ tự tải lên
                    });
                }

                // Lưu danh sách hình ảnh vào cơ sở dữ liệu
                if (productImages.Any()) await _productImageRepository.AddProductImagesAsync(productImages);
            }

            return RedirectToAction("Product", "Admin");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
            return View(productDto);
        }
    }

    // [HttpPost]
    // public async Task<IActionResult> UpdateProduct(ProductDTO product, int productId)
    // {
    //     try
    //     {
    //         // Tìm sản phẩm cần cập nhật
    //         var existingProduct = await _productRepository.GetProductByIdAsync(productId);
    //         if (existingProduct == null)
    //         {
    //             TempData["updateMess"] = "Không tìm thấy sản phẩm!";
    //             return RedirectToAction("ProductList");
    //         }

    //         // Cập nhật thông tin sản phẩm
    //         existingProduct.Name = product.Product.Name;
    //         existingProduct.Description = product.Product.Description;
    //         existingProduct.Price = product.Product.Price;
    //         existingProduct.StockQuantity = product.Product.StockQuantity;
    //         existingProduct.CategoryId = product.Product.CategoryId;
    //         existingProduct.UpdatedAt = DateTime.Now;
    //         existingProduct.UpdatedBy = getUserId(); // Lấy ID của Admin hiện tại

    //         var updated = await _productRepository.UpdateProductAsync(existingProduct);
    //         if (updated != null)
    //         {
    //             TempData["updateMess"] = "Cập nhật sản phẩm không thành công!";
    //             return RedirectToAction("ProductList");
    //         }

    //         TempData["updateDone"] = "Cập nhật sản phẩm thành công!";
    //         return RedirectToAction("ProductList");
    //     }
    //     catch (Exception ex)
    //     {
    //         TempData["updateMess"] = "Đã xảy ra lỗi: " + ex.Message;
    //         return RedirectToAction("ProductList");
    //     }
    // }


    [HttpGet]
    public async Task<IActionResult> Category(int page = 1, int pageSize = 6)
    {
        var (categories, totalCount) = await _categoryRepository.GetPaginationCategorysAsync(page, pageSize);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var categoryDto = new CategoryDTO
        {
            ListCategories = categories,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        return View(categoryDto);
    }

    public async Task<IActionResult> CategoryDetail(int categoryId)
    {
        var categoryDTO = new CategoryDTO();
        categoryDTO.Category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        return View(categoryDTO);
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productRepository.DeleteProductAsync(id);
            return RedirectToAction("Product", "Admin");
        }
        catch (Exception e)
        {
            Console.WriteLine("error delete product! " + e.Message);
            return RedirectToAction("Product");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Category(CategoryDTO categoryDto)
    {
        var category = await _categoryRepository.CreateCategoryAsync(categoryDto.Category);
        return RedirectToAction("Category", "Admin");
    }

    public async Task<IActionResult> Order()
    {
        var adminOrderDTO = new AdminOrderDTO();
        adminOrderDTO.ListOrder = await _orderRepository.GetOrders();
        adminOrderDTO.ListCustomers = await _customerRepository.GetListCustomerAsync();
        return View(adminOrderDTO);
    }

    public IActionResult Analyst()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        // Delete auth_token cookie
        Response.Cookies.Delete("auth_token");

        return RedirectToAction("Index");
    }


    private bool IsValidStatus(string status)
    {
        var validStatuses = new[] { "pending", "processed", "completed", "cancelled" };
        return validStatuses.Contains(status.ToLower());
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDTO model)
    {
        try
        {
            var success = await _orderRepository.UpdateOrderStatusAsync(model.OrderId, model.Status);
            if (success) return Json(new { success = true });
            return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}