using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class AdminDAO : SingletonBase<AdminDAO>
{
    public async Task<List<Admin>> GetListAdminAsync()
    {
        try
        {
            var admins = await _context.Admins.ToListAsync();

            return admins.Count > 0 ? admins : new List<Admin>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error list admin! {ex.Message}");
            return new List<Admin>();
        }
    }

    public async Task ChangePassAsync(int id, string pass)
    {
        try
        {
            var admin = await GetAdminByIdAsync(id);

            if (admin == null) return;

            admin.Password = pass;

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"error change pass! {e.Message}");
        }
    }

    public async Task<Admin> GetAdminByIdAsync(int id)
    {
        try
        {
            return await _context.Admins.SingleOrDefaultAsync(a => a.AdminId == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error get admin by id! {ex.Message}");
            return null;
        }
    }

    public async Task<Admin> FindAdminByEmailAsync(string email)
    {
        try
        {
            return await _context.Admins.SingleOrDefaultAsync(a => a.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error find admin by email! {e.Message}");
            return null;
        }
    }


    public async Task<Admin> CreateAdminAsync(Admin admin)
    {
        try
        {
            if (admin == null) return null;

            var existingAdmin = await GetAdminByIdAsync(Convert.ToInt32(admin.AdminId));

            if (existingAdmin != null) return null;

            await _context.Admins.AddAsync(admin);

            await _context.SaveChangesAsync();

            return admin;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error create admin! {ex.Message}");
            return null;
        }
    }

    public async Task<Admin> UpdateAdminAsync(Admin admin)
    {
        if (admin == null) return null;

        try
        {
            var existingAdmin = await _context.Admins.FindAsync(admin.AdminId);

            if (existingAdmin == null) return null;

            existingAdmin.FirstName = admin.FirstName;
            existingAdmin.LastName = admin.LastName;
            existingAdmin.Username = admin.Username;
            existingAdmin.Password = admin.Password;
            existingAdmin.Email = admin.Email;
            existingAdmin.Picture = admin.Picture;
            existingAdmin.Role = admin.Role;
            existingAdmin.Status = admin.Status;
            existingAdmin.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return existingAdmin;
        }
        catch (Exception e)
        {
            Console.WriteLine($"error update admin! {e.Message}");
            return null;
        }
    }

    public async Task DeleteAdminAsync(int adminId)
    {
        try
        {
            var admin = await GetAdminByIdAsync(adminId);

            if (admin == null) return;

            _context.Admins.Remove(admin);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error delete admin! {ex.Message}");
        }
    }

    public async Task<Admin> GetAdminByUsernameAsync(string username)
    {
        try
        {
            return await _context.Admins.SingleOrDefaultAsync(a => a.Username.Equals(username));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error get admin name! {ex.Message}");
            return null;
        }
    }

    public async Task<(List<Admin>, int)> GetPaginationAdminsAsync(int page, int pageSize)
    {
        var admins = _context.Admins.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = _context.Admins.Count();
        return (admins, totalCount);
    }

    public async Task<List<Admin>> GetFilteredAdminsAsync(
        string searchTerm = null,
        string role = null,
        string status = null,
        DateTime? minLastLogin = null,
        DateTime? maxLastLogin = null,
        string sortBy = null)
    {
        try
        {
            var query = _context.Admins.AsQueryable();

            // Lọc theo từ khóa tìm kiếm (tên, username, email)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => 
                    a.FirstName.Contains(searchTerm) || 
                    a.LastName.Contains(searchTerm) || 
                    a.Username.Contains(searchTerm) || 
                    a.Email.Contains(searchTerm));
            }

            // Lọc theo vai trò
            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(a => a.Role == role);
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            // Lọc theo khoảng thời gian đăng nhập gần nhất
            if (minLastLogin.HasValue)
            {
                query = query.Where(a => a.LastLogin >= minLastLogin);
            }
            if (maxLastLogin.HasValue)
            {
                query = query.Where(a => a.LastLogin <= maxLastLogin);
            }

            // Sắp xếp theo tiêu chí
            switch (sortBy?.ToLower())
            {
                case "newest":
                    query = query.OrderByDescending(a => a.CreatedAt);
                    break;
                case "oldest":
                    query = query.OrderBy(a => a.CreatedAt);
                    break;
                case "recent_login":
                    query = query.OrderByDescending(a => a.LastLogin);
                    break;
                case "role":
                    query = query.OrderBy(a => a.Role);
                    break;
                default:
                    query = query.OrderBy(a => a.FirstName);
                    break;
            }

            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error filtering admins: " + e.Message);
            throw;
        }
    }

}