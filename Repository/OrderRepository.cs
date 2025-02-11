using DataAccess.DAOs;
using Models;
using Repository.Interface;

namespace Repository;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDAO _orderDAO;

    public OrderRepository(OrderDAO orderDAO)
    {
        _orderDAO = orderDAO;
    }

    public async Task<Order> CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
    {
        return await _orderDAO.CreateOrderAsync(order, orderDetails);
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _orderDAO.GetOrderByIdAsync(orderId);
    }

    public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        return await _orderDAO.GetOrdersByCustomerIdAsync(customerId);
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        return await _orderDAO.UpdateOrderStatusAsync(orderId, status);
    }

    public async Task<bool> CancelOrderAsync(int orderId)
    {
        return await _orderDAO.CancelOrderAsync(orderId);
    }

    public async Task<List<Order>> GetOrders()
    {
        return await _orderDAO.GetOrders();
    }

    public async Task<(List<Order>, int)> GetPaginationOrdersAsync(int page, int pageSize)
    {
        return await _orderDAO.GetPaginationOrdersAsync(page, pageSize);
    }

    public async Task<List<Order>> GetFilteredOrdersAsync(
        int? customerId = null,
        DateTime? minOrderDate = null,
        DateTime? maxOrderDate = null,
        string status = null,
        decimal? minTotalPrice = null,
        decimal? maxTotalPrice = null,
        string sortBy = null)
    {
        return await _orderDAO.GetFilteredOrdersAsync(customerId, minOrderDate, maxOrderDate, status, minTotalPrice, maxTotalPrice, sortBy);
    }

}