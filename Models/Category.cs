namespace Models;

public class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<BannerCategory> BannerCategories { get; set; } = new List<BannerCategory>();

    public virtual Admin? CreatedByNavigation { get; set; }

    public virtual Admin? DeletedByNavigation { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Admin? UpdatedByNavigation { get; set; }
}