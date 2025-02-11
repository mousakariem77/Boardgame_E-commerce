using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class ProductRepository : IProductRepository
{
    private readonly ProductDAO _productDAO;

    public ProductRepository(ProductDAO productDAO)
    {
        _productDAO = productDAO;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productDAO.GetAllProductsAsync();
    }

    public async Task<Product> GetProductByIdAsync(long productId)
    {
        return await _productDAO.GetProductByIdAsync(productId);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        return await _productDAO.CreateProductAsync(product);
    }

    public async Task<Product> GetProductByNameAsync(string name)
    {
        return await _productDAO.GetProductByNameAsync(name);
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        return await _productDAO.UpdateProductAsync(product);
    }

    public async Task DeleteProductAsync(int productId)
    {
        await _productDAO.DeleteProductAsync(productId);
    }

    public async Task<(List<Product>, int)> GetPaginationProductsAsync(int page, int pageSize)
    {
        return await _productDAO.GetPaginationProductsAsync(page, pageSize);
    }
 
    public async Task<List<Product>> GetFilteredProductsAsync(
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string searchTerm = null,
        string sortBy = null)
    {
        return await _productDAO.GetFilteredProductsAsync(categoryId, minPrice, maxPrice, searchTerm, sortBy);
    }
}