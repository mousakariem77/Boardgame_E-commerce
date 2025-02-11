namespace Models;

public class Voucher
{
    public int VoucherId { get; set; }

    public string VoucherCode { get; set; } = null!;

    public string VoucherName { get; set; } = null!;

    public string? VoucherType { get; set; }

    public int? VoucherDiscount { get; set; }

    public DateOnly? VoucherStartAt { get; set; }

    public DateOnly? VoucherEndAt { get; set; }

    public decimal? VoucherMax { get; set; }

    public int? VoucherQuantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ApplyVoucher> ApplyVouchers { get; set; } = new List<ApplyVoucher>();

    public virtual ICollection<BannerVoucher> BannerVouchers { get; set; } = new List<BannerVoucher>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<VoucherCondition> VoucherConditions { get; set; } = new List<VoucherCondition>();
}