using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class ProductImageDAO : SingletonBase<ProductImageDAO>
{
    public async Task<List<ProductImage>> GetAllProductImagesAsync()
    {
        return await _context.ProductImages.ToListAsync();
    }

    public async Task<List<ProductImage>> AddProductImagesAsync(List<ProductImage> productImages)
    {
        if (productImages == null || !productImages.Any())
            throw new ArgumentException("Product images cannot be null or empty.", nameof(productImages));

        await _context.ProductImages.AddRangeAsync(productImages);
        await _context.SaveChangesAsync();
        return productImages;
    }
}