namespace Models;

public class BannerCategory
{
    public int BannerId { get; set; }

    public int CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt1 { get; set; }

    public DateTime? UpdatedAt1 { get; set; }

    public DateTime? DeletedAt1 { get; set; }

    public virtual Banner Banner { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}