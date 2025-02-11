namespace Models;

public class ApplyVoucher
{
    public int ApplyVoucherId { get; set; }

    public int VoucherId { get; set; }

    public int CustomerId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Voucher Voucher { get; set; } = null!;
}