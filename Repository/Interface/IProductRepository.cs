using Models;

namespace Repository.Interface;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();

    Task<Product> GetProductByIdAsync(long productId);

    Task<Product> CreateProductAsync(Product product);

    Task<Product> GetProductByNameAsync(string name);

    Task<Product> UpdateProductAsync(Product product);

    Task DeleteProductAsync(int productId);

    Task<(List<Product>, int)> GetPaginationProductsAsync(int page, int pageSize);

    Task<List<Product>> GetFilteredProductsAsync(
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string searchTerm = null,
        string sortBy = null); 
}