using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class OrderDAO : SingletonBase<OrderDAO>
{
    public async Task<Order> CreateOrderAsync(Order order, List<OrderDetail> orderDetails)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Thêm order
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Gán OrderId cho các OrderDetail
            foreach (var detail in orderDetails) detail.OrderId = order.OrderId;
            // Thêm order details
            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();

            // Cập nhật số lượng sản phẩm trong kho
            foreach (var detail in orderDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= detail.Quantity;
                    _context.Products.Update(product);
                }
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return order;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Error creating order: {ex.Message}");
        }
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.Customer)
                .Include(o => o.Voucher)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && !o.IsDeleted.Value);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting order: {ex.Message}");
        }
    }

    public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        try
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.Voucher)
                .Where(o => o.CustomerId == customerId && !o.IsDeleted.Value)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting customer orders: {ex.Message}");
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                Console.WriteLine($"Order not found: {orderId}");
                return false;
            }

            order.Status = status;
            order.UpdatedAt = DateTime.Now;

            _context.Orders.Update(order);
            var result = await _context.SaveChangesAsync();

            Console.WriteLine($"Order {orderId} status updated to {status}. Rows affected: {result}");
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order status: {ex}");
            throw;
        }
    }

    public async Task<bool> CancelOrderAsync(int orderId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;

            // Cập nhật trạng thái đơn hàng
            order.Status = "cancelled";
            order.UpdatedAt = DateTime.Now;

            // Hoàn trả số lượng sản phẩm vào kho
            foreach (var detail in order.OrderDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product != null)
                {
                    product.StockQuantity += detail.Quantity;
                    _context.Products.Update(product);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Error cancelling order: {ex.Message}");
        }
    }

    public async Task<List<Order>> GetOrders()
    {
        List<Order> orders = new();
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            orders = await _context.Orders.ToListAsync();
            return orders;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Error getting orders: {e.Message}");
        }
    }

    public async Task<(List<Order>, int)> GetPaginationOrdersAsync(int page, int pageSize)
    {
        var orders = _context.Orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = _context.Orders.Count();
        return (orders, totalCount);
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
        try
        {
            var query = _context.Orders
                .Where(o => !o.IsDeleted.Value) // Loại bỏ đơn hàng đã bị xóa
                .AsQueryable();

            // Lọc theo mã khách hàng
            if (customerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == customerId);
            }

            // Lọc theo khoảng thời gian đặt hàng
            if (minOrderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= minOrderDate);
            }
            if (maxOrderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= maxOrderDate);
            }

            // Lọc theo trạng thái đơn hàng
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(o => o.Status == status);
            }

            // Lọc theo tổng giá trị đơn hàng
            if (minTotalPrice.HasValue)
            {
                query = query.Where(o => o.TotalPrice >= minTotalPrice);
            }
            if (maxTotalPrice.HasValue)
            {
                query = query.Where(o => o.TotalPrice <= maxTotalPrice);
            }

            // Sắp xếp theo tiêu chí
            switch (sortBy?.ToLower())
            {
                case "newest":
                    query = query.OrderByDescending(o => o.OrderDate);
                    break;
                case "oldest":
                    query = query.OrderBy(o => o.OrderDate);
                    break;
                case "highest_total":
                    query = query.OrderByDescending(o => o.TotalPrice);
                    break;
                case "lowest_total":
                    query = query.OrderBy(o => o.TotalPrice);
                    break;
                default:
                    query = query.OrderBy(o => o.OrderId);
                    break;
            }

            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error filtering orders: " + e.Message);
            throw;
        }
    }

}