using Microsoft.AspNetCore.Mvc;
using Models;
using Repository.Interface;
using Xanh_Dau.DTO;
using Xanh_Dau.Services;

namespace Xanh_Dau.Controllers;

public class AuthController : Controller
{
    private readonly IAddressRepository _addressRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly FileService _fileService;
    private readonly TokenService _tokenService;

    public AuthController(ICustomerRepository customerRepository, TokenService tokenService, FileService fileService,
        IAddressRepository addressRepository)
    {
        _customerRepository = customerRepository;
        _tokenService = tokenService;
        _fileService = fileService;
        _addressRepository = addressRepository;
    }

    private int getUserId()
    {
        var token = Request.Cookies["auth_token"];

        var userId = _tokenService.GetUserIdFromToken(token);

        return Convert.ToInt32(userId);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _customerRepository.GetCustomerByEmailAsync(email);

        if (user == null || user.Password != password)
        {
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // Create token for the user
        var tokenString = await _tokenService.CreateTokenAsync(Convert.ToInt32(user.CustomerId));

        Console.WriteLine(tokenString);

        Response.Cookies.Append("auth_token", tokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.Now.AddHours(24),
            SameSite = SameSiteMode.Strict
        });

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> setDefaultAddess(int addessId, int customerId)
    {
        await _addressRepository.SetDefaultAddress(addessId, customerId);
        return RedirectToAction("Profile");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Customer customer)
    {
        if (!ModelState.IsValid) return View(customer);

        var cus = await _customerRepository.CreateCustomerAsync(customer);

        if (cus != null) return RedirectToAction("Login", "Auth");

        return View(customer);
    }

    public IActionResult TOS() //Terms of Service
    {
        return View();
    }

    public async Task<IActionResult> Profile()
    {
        var addresses = await _addressRepository.GetAllAddressesAsync();
        var profile = new ProfileDTO();
        profile.Customer = await _customerRepository.GetCustomerByIdAsync(getUserId());
        profile.ListAddresses = addresses.Where(c => c.CustomerId == getUserId()).ToList();
        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(ProfileDTO profileDto)
    {
        try
        {
            profileDto.Address.CustomerId = getUserId();
            var address = await _addressRepository.CreateAddressAsync(profileDto.Address);

            return RedirectToAction("Profile");
        }
        catch (Exception e)
        {
            Console.WriteLine("error creating address");
            return RedirectToAction("Profile");
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        // Delete auth_token cookie
        Response.Cookies.Delete("auth_token");

        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
    {
        // Lấy thông tin user
        var user = await _customerRepository.GetCustomerByIdAsync(getUserId());

        // Kiểm tra mật khẩu hiện tại
        if (oldPassword != user.Password)
        {
            TempData["wrongPass"] = "Mật khẩu hiện tại không đúng!";
            TempData["ActiveTab"] = "change-password"; // Kích hoạt tab "Đổi mật khẩu"
            return RedirectToAction("Profile");
        }

        // Kiểm tra xác nhận mật khẩu
        if (newPassword != confirmPassword)
        {
            TempData["wrongPass"] = "Mật khẩu mới và xác nhận không khớp!";
            TempData["ActiveTab"] = "change-password"; // Kích hoạt tab "Đổi mật khẩu"
            return RedirectToAction("Profile");
        }

        // Đổi mật khẩu
        await _customerRepository.ChangePassAsync(user.CustomerId, newPassword);

        // Thông báo thành công
        TempData["SuccessMessage"] = true;
        return RedirectToAction("Profile");
    }

    [HttpPost]
    [Route("auth/UpdateAvatar")]
    public async Task<IActionResult> UpdateAvatar(IFormFile avatar)
    {
        if (avatar == null || avatar.Length == 0)
            return Json(new { success = false, message = "File ảnh không hợp lệ." });

        try
        {
            // Kiểm tra loại file
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(avatar.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return Json(new { success = false, message = "Chỉ hỗ trợ các định dạng JPG, JPEG, PNG, GIF." });

            var avatarUrl = await _fileService.UploadImageAsync(avatar);
            var user = await _customerRepository.GetCustomerByIdAsync(getUserId());
            user.Picture = avatarUrl;

            _customerRepository.UpdateCustomerAsync(user);
            return Json(new { success = true, avatarUrl });
        }
        catch (Exception ex)
        {
            // Xử lý lỗi
            return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(ProfileDTO profile, string gender)
    {
        try
        {
            var user = await _customerRepository.GetCustomerByIdAsync(getUserId());
            user.FirstName = profile.Customer.FirstName;
            user.LastName = profile.Customer.LastName;
            user.Email = profile.Customer.Email;
            user.PhoneNumber = profile.Customer.PhoneNumber;
            user.Gender = profile.Customer.Gender;
            user.Dob = profile.Customer.Dob;
            user.UpdatedAt = DateTime.Now;
            user.Gender = gender;

            var updated = await _customerRepository.UpdateCustomerAsync(user);
            if (updated == null)
            {
                TempData["updateMess"] = "Cập nhật không thành công!";
                TempData["UpdateProfile"] = "profile-modal"; // Kích hoạt tab "Đổi mật khẩu"
                return RedirectToAction("Profile");
            }


            // Thông báo thành công
            TempData["updateDone"] = true;
            return RedirectToAction("Profile");
        }
        catch (Exception e)
        {
            TempData["updateMess"] = "Lỗi !";
            TempData["UpdateProfile"] = "profile-modal"; // Kích hoạt tab "Đổi mật khẩu"
            return RedirectToAction("Profile");
        }
    }
}