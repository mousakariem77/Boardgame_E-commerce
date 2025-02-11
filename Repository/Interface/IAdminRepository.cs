using Models;

namespace Repository.Interface;

public interface IAdminRepository
{
    Task<List<Admin>> GetListAdminAsync();

    Task ChangePassAsync(int id, string pass);

    Task<Admin> GetAdminByIdAsync(int id);

    Task<Admin> FindAdminByEmailAsync(string email);

    Task<Admin> CreateAdminAsync(Admin admin);

    Task<Admin> UpdateAdminAsync(Admin admin);

    Task DeleteAdminAsync(int adminId);

    Task<Admin> GetAdminByUsernameAsync(string username);
    Task<(List<Admin>, int)> GetPaginationAdminsAsync(int page, int pageSize);
    Task<List<Admin>> GetFilteredAdminsAsync(
        string searchTerm = null,
        string role = null,
        string status = null,
        DateTime? minLastLogin = null,
        DateTime? maxLastLogin = null,
        string sortBy = null);

}