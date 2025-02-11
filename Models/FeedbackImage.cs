namespace Models;

public class FeedbackImage
{
    public int FeedbackImgId { get; set; }

    public string? FeedbackImage1 { get; set; }

    public int FeedbackId { get; set; }

    public int? CreateUser { get; set; }

    public DateTime? CreateAt { get; set; }

    public int? UpdateUser { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? DeleteUser { get; set; }

    public DateTime? DeleteAt { get; set; }

    public bool? IsDelete { get; set; }

    public virtual Feedback Feedback { get; set; } = null!;
}