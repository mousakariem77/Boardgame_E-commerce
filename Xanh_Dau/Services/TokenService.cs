using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Xanh_Dau.Services;

public class TokenService
{
    private readonly IAdminRepository _adminRepository;
    private readonly string _audience;
    private readonly ICustomerRepository _customerRepository;
    private readonly string _issuer;
    private readonly string _secretKey;

    public TokenService(ICustomerRepository customerRepository, IConfiguration configuration,
        IAdminRepository adminRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
        _secretKey = configuration["JwtSettings:SecretKey"] ?? throw new ArgumentNullException("JwtSettings:SecretKey");
        _issuer = configuration["JwtSettings:Issuer"] ?? "MyAuthService";
        _audience = configuration["JwtSettings:Audience"] ?? "MyApp";
    }

    public async Task<string> CreateTokenAsync(int id)
    {
        var user = await _customerRepository.GetCustomerByIdAsync(id);
        if (user == null) return null;
        var picture = user.Picture ?? "/img/avatar.jpg";


        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.CustomerId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, "Customer"), // Thêm role khách hàng
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("UserName", user.FirstName + " " + user.LastName),
            new("Picture", picture)

        };

        return GenerateToken(claims);
    }

    public async Task<string> CreateTokenAdminAsync(int id)
    {
        var user = await _adminRepository.GetAdminByIdAsync(id);
        if (user == null) return null;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.AdminId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, "Admin"), // Thêm role admin
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("UserName", user.LastName + " " + user.FirstName)
        };
        return GenerateToken(claims);
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public int? GetUserIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId)) return userId;
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public string GetUserImageUrl(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var imageUrl = jwtToken.Claims.FirstOrDefault(c => c.Type == "imageUrl")?.Value;
        return imageUrl;
    }
}