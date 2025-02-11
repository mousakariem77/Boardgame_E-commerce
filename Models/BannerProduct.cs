namespace Models;

public class BannerProduct
{
    public int BannerId { get; set; }

    public int ProductId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedUser { get; set; }

    public int? UpdatedUser { get; set; }

    public int? DeletedUser { get; set; }

    public virtual Banner Banner { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}