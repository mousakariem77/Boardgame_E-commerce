namespace Models;

public class VoucherCondition
{
    public int ConditionId { get; set; }

    public int VoucherId { get; set; }

    public string? ConditionName { get; set; }

    public string? ConditionType { get; set; }

    public string? ConditionValue { get; set; }

    public int? ConditionMaxUsage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Voucher Voucher { get; set; } = null!;
}