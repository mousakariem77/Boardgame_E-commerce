using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess;

public partial class BanHangContext : DbContext
{
    public BanHangContext()
    {
    }

    public BanHangContext(DbContextOptions<BanHangContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<ApplyVoucher> ApplyVouchers { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<BannerCategory> BannerCategories { get; set; }

    public virtual DbSet<BannerProduct> BannerProducts { get; set; }

    public virtual DbSet<BannerVoucher> BannerVouchers { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<FeedbackImage> FeedbackImages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherCondition> VoucherConditions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(
            "Server=LAPTOP-DT9JEAJM\\SQLEXPRESS;Database=BAN_HANG;User=sa;Password=12345;encrypt=true; trustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__26A1118D2646B5FB");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId).HasColumnName("addressID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("isDefault");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.Receiver)
                .HasMaxLength(250)
                .HasColumnName("receiver");
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(250)
                .HasColumnName("ship_Address");
            entity.Property(e => e.ShipPhone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ship_Phone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__AD05008633172144");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Email, "UQ__Admin__AB6E61643D9F034B").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Admin__F3DBC572AEBA3497").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("adminID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastLogin)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.LastPasswordChange)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_password_change");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Picture)
                .HasColumnType("text")
                .HasColumnName("picture");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("admin")
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<ApplyVoucher>(entity =>
        {
            entity.HasKey(e => e.ApplyVoucherId).HasName("PK__Apply_Vo__614340338ADBD69D");

            entity.ToTable("Apply_Vouchers");

            entity.Property(e => e.ApplyVoucherId).HasColumnName("applyVoucherID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VoucherId).HasColumnName("voucherID");

            entity.HasOne(d => d.Customer).WithMany(p => p.ApplyVouchers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Apply_Vou__custo__18EBB532");

            entity.HasOne(d => d.Voucher).WithMany(p => p.ApplyVouchers)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Apply_Vou__vouch__17F790F9");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banners__BD58D393EE61EA5B");

            entity.Property(e => e.BannerId).HasColumnName("bannerID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedAt1)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.DeletedAt1)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(150)
                .HasColumnName("imageUrl");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.TargetUrl)
                .HasMaxLength(150)
                .HasColumnName("targetUrl");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedAt1)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<BannerCategory>(entity =>
        {
            entity.HasKey(e => new { e.BannerId, e.CategoryId }).HasName("PK__Banner_C__2F647C8CCF6672A3");

            entity.ToTable("Banner_Categories");

            entity.Property(e => e.BannerId).HasColumnName("bannerID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedAt1)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.DeletedAt1)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedAt1)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Banner).WithMany(p => p.BannerCategories)
                .HasForeignKey(d => d.BannerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Banner_Ca__banne__2645B050");

            entity.HasOne(d => d.Category).WithMany(p => p.BannerCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Banner_Ca__categ__25518C17");
        });

        modelBuilder.Entity<BannerProduct>(entity =>
        {
            entity.HasKey(e => new { e.BannerId, e.ProductId }).HasName("PK__Banner_P__0F89DE87EB1B24F1");

            entity.ToTable("Banner_Products");

            entity.Property(e => e.BannerId).HasColumnName("bannerID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedUser).HasColumnName("created_User");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedUser).HasColumnName("deleted_User");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser).HasColumnName("updated_User");

            entity.HasOne(d => d.Banner).WithMany(p => p.BannerProducts)
                .HasForeignKey(d => d.BannerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Banner_Pr__banne__2B0A656D");

            entity.HasOne(d => d.Product).WithMany(p => p.BannerProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Banner_Pr__produ__2BFE89A6");
        });

        modelBuilder.Entity<BannerVoucher>(entity =>
        {
            entity.HasKey(e => new { e.BannerId, e.VoucherId }).HasName("PK__Banner_V__320BEB0B9DD4C4E2");

            entity.ToTable("Banner_Vouchers");

            entity.Property(e => e.BannerId).HasColumnName("bannerID");
            entity.Property(e => e.VoucherId).HasColumnName("voucherID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedUser).HasColumnName("created_User");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedUser).HasColumnName("deleted_User");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedUser).HasColumnName("updated_User");

            entity.HasOne(d => d.Banner).WithMany(p => p.BannerVouchers)
                .HasForeignKey(d => d.BannerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Banner_Vo__banne__30C33EC3");

            entity.HasOne(d => d.Voucher).WithMany(p => p.BannerVouchers)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Banner_Vo__vouch__31B762FC");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__415B03D82CB5BE7D");

            entity.Property(e => e.CartId).HasColumnName("cartID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Carts__customerI__7D439ABD");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasKey(e => e.CartDetailId).HasName("PK__Cart_Det__3DD393962BF2DB17");

            entity.ToTable("Cart_Details");

            entity.Property(e => e.CartDetailId).HasColumnName("cartDetailID");
            entity.Property(e => e.CartId).HasColumnName("cartID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__Cart_Deta__cartI__02FC7413");

            entity.HasOne(d => d.Product).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_Deta__produ__03F0984C");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__23CAF1F8312C20A2");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parentID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Categorie__creat__6477ECF3");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CategoryDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__Categorie__delet__66603565");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Categorie__paren__6383C8BA");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Categorie__updat__656C112C");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__B611CB9DA5E395F0");

            entity.HasIndex(e => e.Email, "UQ__Customer__AB6E6164F266C333").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Customer__F3DBC572CD96392C").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.Address)
                .HasColumnType("ntext")
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastLogin)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Picture)
                .HasColumnType("text")
                .HasColumnName("picture");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC42CAA4C19");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.CreateBy).HasColumnName("create_by");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.DeleteAt)
                .HasColumnType("datetime")
                .HasColumnName("delete_at");
            entity.Property(e => e.DeleteUser).HasColumnName("delete_User");
            entity.Property(e => e.FeedbackComment)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("feedback_Comment");
            entity.Property(e => e.FeedbackRate).HasColumnName("feedback_Rate");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("isDelete");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Feedback__custom__4F47C5E3");

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Feedback__produc__4E53A1AA");
        });

        modelBuilder.Entity<FeedbackImage>(entity =>
        {
            entity.HasKey(e => e.FeedbackImgId).HasName("PK__Feedback__A722B68E6185EBB5");

            entity.ToTable("Feedback_Image");

            entity.Property(e => e.FeedbackImgId).HasColumnName("feedbackImgID");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.CreateUser).HasColumnName("create_User");
            entity.Property(e => e.DeleteAt)
                .HasColumnType("datetime")
                .HasColumnName("delete_at");
            entity.Property(e => e.DeleteUser).HasColumnName("delete_User");
            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.FeedbackImage1)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("feedback_Image");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("isDelete");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.UpdateUser).HasColumnName("update_User");

            entity.HasOne(d => d.Feedback).WithMany(p => p.FeedbackImages)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__Feedback___feedb__540C7B00");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__0809337D664542F1");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasDefaultValue(0)
                .HasColumnName("order_status");
            entity.Property(e => e.Receiver)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("receiver");
            entity.Property(e => e.ShipPhone)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("ship_phone");
            entity.Property(e => e.ShipperPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("shipper_phone");
            entity.Property(e => e.ShippingAddress)
                .HasColumnType("text")
                .HasColumnName("shipping_address");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.Subtotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VoucherId).HasColumnName("voucherID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__customer__3C34F16F");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__Orders__voucherI__3D2915A8");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__Order_De__E4FEDE2A8C28CF95");

            entity.ToTable("Order_Details");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Order_Det__order__42E1EEFE");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_Det__produ__43D61337");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__Payment__082E8AE355B880C0");

            entity.ToTable("Payment");

            entity.Property(e => e.PayId).HasColumnName("payID");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.IsDelete)
                .HasDefaultValue(false)
                .HasColumnName("isDelete");
            entity.Property(e => e.IsSuccess).HasColumnName("isSuccess");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.PayName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("payName");
            entity.Property(e => e.PayTxnRef)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("payTxnRef");
            entity.Property(e => e.PayType)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("payType");
            entity.Property(e => e.PaymentUrl)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("payment_URL");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__custome__498EEC8D");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__orderID__489AC854");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__2D10D14AAB569489");

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("available")
                .HasColumnName("status");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__catego__6D0D32F4");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Products__create__6E01572D");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ProductDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__Products__delete__6FE99F9F");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Products__update__6EF57B66");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Product___336E9B75B469BDEB");

            entity.ToTable("Product_Images");

            entity.Property(e => e.ImageId).HasColumnName("imageID");
            entity.Property(e => e.ArrangeOrder)
                .HasDefaultValue(1)
                .HasColumnName("arrange_order");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.ImageUrl)
                .HasColumnType("text")
                .HasColumnName("image_url");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsPrimary)
                .HasDefaultValue(false)
                .HasColumnName("is_primary");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Product_I__produ__778AC167");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Vouchers__F53389896F6ADBAD");

            entity.Property(e => e.VoucherId).HasColumnName("voucherID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VoucherCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("voucherCode");
            entity.Property(e => e.VoucherDiscount)
                .HasDefaultValue(0)
                .HasColumnName("voucher_Discount");
            entity.Property(e => e.VoucherEndAt).HasColumnName("voucher_EndAt");
            entity.Property(e => e.VoucherMax)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("voucher_Max");
            entity.Property(e => e.VoucherName)
                .HasMaxLength(150)
                .HasColumnName("voucher_Name");
            entity.Property(e => e.VoucherQuantity).HasColumnName("voucher_Quantity");
            entity.Property(e => e.VoucherStartAt).HasColumnName("voucher_StartAt");
            entity.Property(e => e.VoucherType)
                .HasMaxLength(150)
                .HasColumnName("voucher_Type");
        });

        modelBuilder.Entity<VoucherCondition>(entity =>
        {
            entity.HasKey(e => e.ConditionId).HasName("PK__Voucher___A297579C0C4E1BDB");

            entity.ToTable("Voucher_Conditions");

            entity.Property(e => e.ConditionId).HasColumnName("conditionID");
            entity.Property(e => e.ConditionMaxUsage)
                .HasDefaultValue(0)
                .HasColumnName("condition_MaxUsage");
            entity.Property(e => e.ConditionName)
                .HasMaxLength(150)
                .HasColumnName("condition_Name");
            entity.Property(e => e.ConditionType)
                .HasMaxLength(150)
                .HasColumnName("condition_Type");
            entity.Property(e => e.ConditionValue)
                .HasMaxLength(150)
                .HasColumnName("condition_Value");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VoucherId).HasColumnName("voucherID");

            entity.HasOne(d => d.Voucher).WithMany(p => p.VoucherConditions)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Voucher_C__vouch__114A936A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}