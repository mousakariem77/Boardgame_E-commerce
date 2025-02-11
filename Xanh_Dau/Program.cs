using System.Text;
using DataAccess;
using DataAccess.DAOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Interface;
using Xanh_Dau.Services;

var builder = WebApplication.CreateBuilder(args);

// Get the secret key from configuration
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(secretKey)) throw new Exception("No Key!");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Đọc config từ appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//DI
builder.Services.AddScoped<BanHangContext>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<FileService>();

//DataAccess
builder.Services.AddScoped<CustomerDAO>();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<ProductImageDAO>();
builder.Services.AddScoped<OrderDAO>();
builder.Services.AddScoped<AdminDAO>();
builder.Services.AddScoped<CategoryDAO>();
builder.Services.AddScoped<CartDAO>();
builder.Services.AddScoped<FeedbackDAO>();
builder.Services.AddScoped<AddressDAO>();


//Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

// Configure authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "MyAuthService",
            ValidAudience = "MyApp",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("CustomerOnly", policy =>
        policy.RequireRole("Customer"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) // Chỉ bật Swagger trong môi trường Dev
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Custom middlewares
app.UseMiddleware<JwtCookieMiddleware>();
app.UseMiddleware<UnauthorizedRedirectMiddleware>();



// Authentication & Authorization middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();