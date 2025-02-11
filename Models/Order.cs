namespace Models;

public class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public decimal TotalPrice { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string? Receiver { get; set; }

    public string? ShipPhone { get; set; }

    public string? ShipperPhone { get; set; }

    public int? VoucherId { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Total { get; set; }

    public int? OrderStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Voucher? Voucher { get; set; }
}