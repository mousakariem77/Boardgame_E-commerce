namespace Models;

public class Feedback
{
    public int FeedbackId { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public int? FeedbackRate { get; set; }

    public string? FeedbackComment { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? DeleteUser { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<FeedbackImage> FeedbackImages { get; set; } = new List<FeedbackImage>();

    public virtual Product Product { get; set; } = null!;
}