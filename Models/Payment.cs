namespace Models;

public class Payment
{
    public int PayId { get; set; }

    public string? PayName { get; set; }

    public string? PayType { get; set; }

    public string? PayTxnRef { get; set; }

    public int OrderId { get; set; }

    public int? IsSuccess { get; set; }

    public int CustomerId { get; set; }

    public DateTime? CreateAt { get; set; }

    public bool? IsDelete { get; set; }

    public string? PaymentUrl { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}