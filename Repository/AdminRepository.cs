using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class AdminRepository : IAdminRepository
{
    private readonly AdminDAO _adminDAO;

    public AdminRepository(AdminDAO adminDAO)
    {
        _adminDAO = adminDAO;
    }

    public async Task<List<Admin>> GetListAdminAsync()
    {
        return await _adminDAO.GetListAdminAsync();
    }

    public async Task ChangePassAsync(int id, string pass)
    {
        await _adminDAO.ChangePassAsync(id, pass);
    }

    public async Task<Admin> GetAdminByIdAsync(int id)
    {
        return await _adminDAO.GetAdminByIdAsync(id);
    }

    public async Task<Admin> FindAdminByEmailAsync(string email)
    {
        return await _adminDAO.FindAdminByEmailAsync(email);
    }

    public async Task<Admin> CreateAdminAsync(Admin admin)
    {
        return await _adminDAO.CreateAdminAsync(admin);
    }

    public async Task<Admin> UpdateAdminAsync(Admin admin)
    {
        return await _adminDAO.UpdateAdminAsync(admin);
    }

    public async Task DeleteAdminAsync(int adminId)
    {
        await _adminDAO.DeleteAdminAsync(adminId);
    }

    public async Task<Admin> GetAdminByUsernameAsync(string username)
    {
        return await _adminDAO.GetAdminByUsernameAsync(username);
    }

    public async Task<(List<Admin>, int)> GetPaginationAdminsAsync(int page, int pageSize)
    {
        return await _adminDAO.GetPaginationAdminsAsync(page, pageSize);
    }

    public async Task<List<Admin>> GetFilteredAdminsAsync(
        string searchTerm = null,
        string role = null,
        string status = null,
        DateTime? minLastLogin = null,
        DateTime? maxLastLogin = null,
        string sortBy = null)
    {
        return await _adminDAO.GetFilteredAdminsAsync(searchTerm, role, status, minLastLogin, maxLastLogin, sortBy);
    }

}