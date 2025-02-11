using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class CategoryDAO : SingletonBase<CategoryDAO>
{
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        try
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted.Value)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error getting categories: " + e.Message);
            throw;
        }
    }

    public async Task<Category> GetCategoryByIdAsync(long categoryId)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId && !c.IsDeleted.Value);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        if (category == null) throw new ArgumentNullException(nameof(category));
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> GetCategoryByNameAsync(string name)
    {
        try
        {
            return await _context.Categories.FirstOrDefaultAsync(
                c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving category by name: " + ex.Message);
            return null;
        }
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
        if (existingCategory != null)
        {
            existingCategory.Name = category.Name; // Cập nhật các thuộc tính khác nếu cần
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        return null;
    }

    public async Task DeleteCategoryAsync(long categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(List<Category>, int)> GetPaginationCategorysAsync(int page, int pageSize)
    {
        var categories = _context.Categories.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = _context.Categories.Count();
        return (categories, totalCount);
    }

    public async Task<List<Category>> GetFilteredCategoriesAsync(
        string searchTerm = null,
        int? parentId = null,
        string status = null,
        DateTime? minCreatedAt = null,
        DateTime? maxCreatedAt = null,
        string sortBy = null)
    {
        try
        {
            var query = _context.Categories
                .Where(c => c.IsDeleted == false || c.IsDeleted == null) // Sửa lỗi nullable bool
                .AsQueryable();

            // Lọc theo từ khóa tìm kiếm (tên hoặc mô tả)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => 
                    c.Name.Contains(searchTerm) || 
                    c.Description.Contains(searchTerm));
            }

            // Lọc theo danh mục cha
            if (parentId.HasValue)
            {
                query = query.Where(c => c.ParentId == parentId);
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(c => c.Status == status);
            }

            // Lọc theo khoảng thời gian tạo danh mục
            if (minCreatedAt.HasValue)
            {
                query = query.Where(c => c.CreatedAt >= minCreatedAt);
            }
            if (maxCreatedAt.HasValue)
            {
                query = query.Where(c => c.CreatedAt <= maxCreatedAt);
            }

            // Sắp xếp theo tiêu chí
            switch (sortBy?.ToLower())
            {
                case "newest":
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;
                case "oldest":
                    query = query.OrderBy(c => c.CreatedAt);
                    break;
                case "name_asc":
                    query = query.OrderBy(c => c.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(c => c.Name);
                    break;
                default:
                    query = query.OrderBy(c => c.Name);
                    break;
            }

            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error filtering categories: " + e.Message);
            throw;
        }
    }

}