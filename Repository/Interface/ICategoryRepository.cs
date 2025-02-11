using Models;

namespace Repository.Interface;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();

    Task<Category> GetCategoryByIdAsync(long categoryId);

    Task<Category> GetCategoryByNameAsync(string name);

    Task<Category> UpdateCategoryAsync(Category category);

    Task DeleteCategoryAsync(long categoryId);

    Task<Category> CreateCategoryAsync(Category category);
    Task<(List<Category>, int)> GetPaginationCategorysAsync(int page, int pageSize);
    Task<List<Category>> GetFilteredCategoriesAsync(
            string searchTerm = null,
            int? parentId = null,
            string status = null,
            DateTime? minCreatedAt = null,
            DateTime? maxCreatedAt = null,
            string sortBy = null);
}