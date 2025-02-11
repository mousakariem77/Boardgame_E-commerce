using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDAO _customerDAO;

    public CustomerRepository(CustomerDAO customerDAO)
    {
        _customerDAO = customerDAO;
    }

    public async Task<List<Customer>> GetListCustomerAsync()
    {
        return await _customerDAO.GetListCustomerAsync();
    }

    public async Task ChangePassAsync(int id, string pass)
    {
        await _customerDAO.ChangePassAsync(id, pass);
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        return await _customerDAO.GetCustomerByIdAsync(id);
    }

    public async Task<Customer> FindCustomerByEmailAsync(string email)
    {
        return await _customerDAO.FindCustomerByEmailAsync(email);
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        return await _customerDAO.CreateCustomerAsync(customer);
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        return await _customerDAO.UpdateCustomerAsync(customer);
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        await _customerDAO.DeleteCustomerAsync(customerId);
    }

    public async Task<Customer> GetCustomerByUsernameAsync(string username)
    {
        return await _customerDAO.GetCustomerByUsernameAsync(username);
    }

    public async Task<Customer> GetCustomerByEmailAsync(string email)
    {
        return await _customerDAO.GetCustomerByEmailAsync(email);
    }
 
    public async Task<(List<Customer>, int)> GetPaginationCustomersAsync(int page, int pageSize)
    {
        return await _customerDAO.GetPaginationCustomersAsync(page, pageSize);
    }

    public async Task<List<Customer>> GetFilteredCustomersAsync(
        string searchTerm = null,
        string status = null,
        DateTime? minCreatedAt = null,
        DateTime? maxCreatedAt = null,
        string sortBy = null)
    {
        return await _customerDAO.GetFilteredCustomersAsync(searchTerm, status, minCreatedAt, maxCreatedAt, sortBy);
    }

}