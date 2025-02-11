using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class ProductImageRepository : IProductImageRepository
{
    private readonly ProductImageDAO _productImageDAO;

    public ProductImageRepository(ProductImageDAO productImageDAO)
    {
        _productImageDAO = productImageDAO ?? throw new ArgumentNullException(nameof(productImageDAO));
    }

    public async Task<List<ProductImage>> GetAllProductImagesAsync()
    {
        return await _productImageDAO.GetAllProductImagesAsync();
    }

    public async Task<List<ProductImage>> AddProductImagesAsync(List<ProductImage> productImages)
    {
        return await _productImageDAO.AddProductImagesAsync(productImages);
    }
}