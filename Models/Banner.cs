namespace Models;

public class Banner
{
    public int BannerId { get; set; }

    public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    public string? TargetUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt1 { get; set; }

    public DateTime? UpdatedAt1 { get; set; }

    public DateTime? DeletedAt1 { get; set; }

    public virtual ICollection<BannerCategory> BannerCategories { get; set; } = new List<BannerCategory>();

    public virtual ICollection<BannerProduct> BannerProducts { get; set; } = new List<BannerProduct>();

    public virtual ICollection<BannerVoucher> BannerVouchers { get; set; } = new List<BannerVoucher>();
}