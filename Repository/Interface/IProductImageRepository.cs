using Models;

namespace Repository.Interface;

public interface IProductImageRepository
{
    Task<List<ProductImage>> GetAllProductImagesAsync();
    Task<List<ProductImage>> AddProductImagesAsync(List<ProductImage> productImages);
}