using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class ProductDAO : SingletonBase<ProductDAO>
{
    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _context.Products.Include(p => p.ProductImages).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("loi list" + e.Message);
            throw;
        }
    }

    public async Task<Product> GetProductByIdAsync(long productId)
    {
        return await _context.Products.FindAsync((int)productId);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> GetProductByNameAsync(string name)
    {
        try
        {
            return await _context.Products.FirstOrDefaultAsync(
                p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving product by name: " + ex.Message);
            return null;
        }
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.ProductId);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.UpdatedAt = DateTime.Now;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.UpdatedBy = product.UpdatedBy;
            existingProduct.Status = product.Status;
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        return null;
    }

    public async Task DeleteProductAsync(int productId)
    {
        if (productId <= 0) throw new ArgumentException("Invalid product ID.", nameof(productId));

        try
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            throw;
        }
    } 


    public async Task<(List<Product>, int)> GetPaginationProductsAsync(int page, int pageSize)
    {
        var products = _context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = _context.Products.Count();
        return (products, totalCount);
    }


    public async Task<List<Product>> GetFilteredProductsAsync(
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string searchTerm = null,
        string sortBy = null)
    {
        try
        {
            var query = _context.Products
                .Where(p => !p.IsDeleted.Value)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .AsQueryable();

            // Apply category filter
            if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId);

            // Apply price range filter
            if (minPrice.HasValue) query = query.Where(p => p.Price >= minPrice);
            if (maxPrice.HasValue) query = query.Where(p => p.Price <= maxPrice);

            // Apply search term
            if (!string.IsNullOrWhiteSpace(searchTerm)) query = query.Where(p => p.Name.Contains(searchTerm));

            // Apply sorting
            switch (sortBy?.ToLower())
            {
                case "bestselling":
                    query = query.OrderByDescending(p =>
                        _context.OrderDetails.Count(od => od.ProductId == p.ProductId));
                    break;
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error filtering products: " + e.Message);
            throw;
        }
    }
}