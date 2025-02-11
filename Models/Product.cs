using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải là số dương.")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải là số không âm.")]
    public int StockQuantity { get; set; }

    [Required] public int CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    [Required] public string? Status { get; set; }

    public virtual ICollection<BannerProduct> BannerProducts { get; set; } = new List<BannerProduct>();

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Category Category { get; set; } = null!;

    public virtual Admin? CreatedByNavigation { get; set; }

    public virtual Admin? DeletedByNavigation { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual Admin? UpdatedByNavigation { get; set; }

    [NotMapped] public int ReviewCount { get; set; }

    [NotMapped] public double AverageRating { get; set; }
}