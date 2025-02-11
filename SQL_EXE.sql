CREATE DATABASE BAN_HANG
GO

USE BAN_HANG
GO

CREATE TABLE [Admin]
(
    [adminID] INT IDENTITY (1,1) PRIMARY KEY,       -- Mã quản trị viên
    [first_name] NVARCHAR(50),            -- Tên
    [last_name] NVARCHAR(50),             -- Họ
    [username] VARCHAR(50) UNIQUE,                 -- Tên đăng nhập (độc nhất)
    [password] VARCHAR(50),                        -- Mật khẩu
    [email] VARCHAR(100) UNIQUE,                   -- Email (độc nhất)
    [picture] TEXT,                                -- Ảnh đại diện
    [role] NVARCHAR(20) DEFAULT 'admin',            -- Vai trò
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [last_login] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian đăng nhập cuối
    [status] NVARCHAR(20) DEFAULT N'Hoạt động',         -- Trạng thái
    [last_password_change] DATETIME DEFAULT CURRENT_TIMESTAMP -- Thời gian đổi mật khẩu cuối
);
GO

CREATE TABLE [Customers]
(
    [customerID] INT IDENTITY (1,1) PRIMARY KEY,   -- Mã khách hàng
    [first_name] NVARCHAR(50),           -- Tên
    [last_name] NVARCHAR(50),            -- Họ
	[username] VARCHAR(50) UNIQUE,
    [gender] NVARCHAR(50),                       -- Giới tính
    [email] VARCHAR(100) UNIQUE,                  -- Email (độc nhất)
    [phone_number] VARCHAR(15),                   -- Số điện thoại
    [address] NTEXT,                               -- Địa chỉ
    [picture] TEXT,                               -- Ảnh đại diện
	[dob] DATE,
    [password] VARCHAR(50),                       -- Mật khẩu
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_by] INT,                           -- Người cập nhật
    [updated_at] DATETIME,                      -- Thời gian cập nhật
    [deleted_by] INT,                           -- Người xóa
    [deleted_at] DATETIME,                      -- Thời gian xóa
    [isDeleted] BIT DEFAULT 0,                        -- Trạng thái xóa (0: không, 1: đã xóa)
    [status] NVARCHAR(20) DEFAULT N'Hoạt động',        -- Trạng thái
    [last_login] DATETIME DEFAULT CURRENT_TIMESTAMP -- Thời gian đăng nhập cuối
);
GO

CREATE TABLE [Address]
(
    [addressID] INT IDENTITY (1,1) PRIMARY KEY,       -- Mã địa chỉ
    [customerID] INT,                       -- Mã khách hàng
    [receiver] NVARCHAR(250),               -- Tên người nhận
    [ship_Address] NVARCHAR(250),            -- Địa chỉ giao hàng
    [ship_Phone] CHAR(25),                   -- Số điện thoại nhận hàng
    [isDefault] BIT DEFAULT 0,                       -- Địa chỉ mặc định (0: không, 1: có)
    [created_by] INT,                       -- Người tạo
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_by] INT,                           -- Người cập nhật
    [updated_at] DATETIME,                      -- Thời gian cập nhật
    [deleted_by] INT,                           -- Người xóa
    [deleted_at] DATETIME,                      -- Thời gian xóa
    [isDeleted] BIT DEFAULT 0                        -- Trạng thái xóa (0: không, 1: đã xóa)
);
GO


CREATE TABLE [Categories]
(
    [categoryID] INT IDENTITY (1,1) PRIMARY KEY,   -- Mã danh mục
    [name] NVARCHAR(100),                -- Tên danh mục
    [description] NTEXT,                           -- Mô tả danh mục
    [parentID] INT,                          -- Mã danh mục cha (nếu có)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
	[created_by] INT,                                  -- Người tạo
    [updated_by] INT,                                  -- Người cập nhật
	[deleted_at] DATETIME,                        -- Thời gian xóa
    [is_deleted] BIT DEFAULT 0,                        -- Trạng thái xóa (logic delete)
    [deleted_by] INT,                                  -- Người xóa
    [status] NVARCHAR(20),        -- Trạng thái
    FOREIGN KEY ([parentID]) REFERENCES [Categories]([categoryID]), -- Khóa ngoại tự liên kết
	FOREIGN KEY ([created_by]) REFERENCES [Admin]([adminID]), -- Khóa ngoại đến bảng Admin
	FOREIGN KEY ([updated_by]) REFERENCES [Admin]([adminID]), -- Khóa ngoại đến bảng Admin
	FOREIGN KEY ([deleted_by]) REFERENCES [Admin]([adminID]) -- Khóa ngoại đến bảng Admin
);
GO


CREATE TABLE [Products]
(
    [productID] INT IDENTITY (1,1) PRIMARY KEY,         -- Mã sản phẩm
    [name] NVARCHAR(100),                     -- Tên sản phẩm
    [description] TEXT,                           -- Mô tả sản phẩm
    [price] DECIMAL(10, 2),                   -- Giá sản phẩm
    [stock_quantity] INT,                     -- Số lượng tồn kho
    [categoryID] INT,                         -- Mã danh mục (khóa ngoại)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP,   -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP,   -- Thời gian cập nhật
    [created_by] INT,                                  -- Người tạo
    [updated_by] INT,                                  -- Người cập nhật
    [deleted_at] DATETIME,                        -- Thời gian xóa
    [is_deleted] BIT DEFAULT 0,                        -- Trạng thái xóa (logic delete)
    [deleted_by] INT,                                  -- Người xóa
    [status] NVARCHAR(20) DEFAULT N'Còn hàng',          -- Trạng thái (available, out of stock)
    FOREIGN KEY ([categoryID]) REFERENCES [Categories]([categoryID]), -- Khóa ngoại đến bảng Categories
	FOREIGN KEY ([created_by]) REFERENCES [Admin]([adminID]), -- Khóa ngoại đến bảng Admin
	FOREIGN KEY ([updated_by]) REFERENCES [Admin]([adminID]), -- Khóa ngoại đến bảng Admin
	FOREIGN KEY ([deleted_by]) REFERENCES [Admin]([adminID]) -- Khóa ngoại đến bảng Admin
);
GO


CREATE TABLE [Product_Images]
(
    [imageID] INT IDENTITY (1,1) PRIMARY KEY,         -- Mã hình ảnh
    [productID] INT,                         -- Mã sản phẩm (liên kết với Products)
    [image_url] TEXT,                        -- Đường dẫn hình ảnh
    [is_primary] BIT DEFAULT 0,                       -- Đánh dấu ảnh chính (0: không, 1: có)
    [arrange_order] INT DEFAULT 1,                    -- Thứ tự hiển thị hình ảnh
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP,  -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP,  -- Thời gian cập nhật
    [deleted_at] DATETIME,                       -- Thời gian xóa (logic delete)
    [is_deleted] BIT DEFAULT 0,                       -- Trạng thái xóa (logic delete)
    FOREIGN KEY ([productID]) REFERENCES [Products]([productID]) ON DELETE CASCADE ON UPDATE CASCADE -- Khóa ngoại đến Products
);
GO

CREATE TABLE [Carts]
(
    [cartID] INT IDENTITY (1,1) PRIMARY KEY,         -- Mã giỏ hàng
    [customerID] INT,                      -- Mã khách hàng (liên kết với Customers)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [deleted_at] DATETIME,                      -- Thời gian xóa (logic delete)
    [is_deleted] BIT DEFAULT 0,                      -- Trạng thái xóa (0: không, 1: đã xóa)
    FOREIGN KEY ([customerID]) REFERENCES [Customers]([customerID]) ON DELETE CASCADE -- Khóa ngoại đến bảng Customers
);
GO

CREATE TABLE [Cart_Details]
(
    [cartDetailID] INT IDENTITY (1,1) PRIMARY KEY,  -- Mã chi tiết giỏ hàng
    [cartID] INT,                          -- Mã giỏ hàng (liên kết với Carts)
    [productID] INT,                       -- Mã sản phẩm (liên kết với Products)
    [quantity] INT,                        -- Số lượng sản phẩm trong giỏ
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [deleted_at] DATETIME,                     -- Thời gian xóa (logic delete)
    [is_deleted] BIT DEFAULT 0,                     -- Trạng thái xóa (0: không, 1: đã xóa)
    FOREIGN KEY ([cartID]) REFERENCES [Carts]([cartID]) ON DELETE CASCADE, -- Khóa ngoại đến bảng Carts
    FOREIGN KEY ([productID]) REFERENCES [Products]([productID]) ON DELETE NO ACTION -- Khóa ngoại đến bảng Products
);
GO

CREATE TABLE [Vouchers]
(
    [voucherID] INT IDENTITY (1,1) PRIMARY KEY,      -- Mã voucher
    [voucherCode] CHAR(6),                   -- Mã voucher (định dạng cố định)
    [voucher_Name] NVARCHAR(150),             -- Tên voucher
    [voucher_Type] NVARCHAR(150),                      -- Loại voucher (ví dụ: giảm giá theo phần trăm hoặc giá trị cố định)
    [voucher_Discount] INT DEFAULT 0,                  -- Mức giảm giá (theo phần trăm hoặc giá trị)
    [voucher_StartAt] DATE,                            -- Ngày bắt đầu hiệu lực
    [voucher_EndAt] DATE,                              -- Ngày kết thúc hiệu lực
    [voucher_Max] NUMERIC(9) DEFAULT 0,                -- Mức giảm giá tối đa có thể áp dụng
    [voucher_Quantity] INT,                            -- Số lượng voucher còn lại
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [deleted_at] DATETIME,                     -- Thời gian xóa (logic delete)
    [isDeleted] BIT DEFAULT 0                         -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)
);
GO

CREATE TABLE [Voucher_Conditions]
(
    [conditionID] INT IDENTITY (1,1) PRIMARY KEY,    -- Mã điều kiện
    [voucherID] INT,                         -- Mã voucher (liên kết với bảng Vouchers)
    [condition_Name] NVARCHAR(150),                    -- Tên điều kiện (ví dụ: "Đơn hàng trên 500.000 VND")
    [condition_Type] NVARCHAR(150),                    -- Loại điều kiện (ví dụ: giá trị đơn hàng, số lượng sản phẩm)
    [condition_Value] NVARCHAR(150),                   -- Giá trị điều kiện (ví dụ: số tiền tối thiểu của đơn hàng)
    [condition_MaxUsage] INT DEFAULT 0,                -- Số lần tối đa có thể áp dụng điều kiện này
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [deleted_at] DATETIME,                     -- Thời gian xóa (logic delete)
    [isDeleted] BIT DEFAULT 0,                        -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)

    FOREIGN KEY ([voucherID]) REFERENCES [Vouchers]([voucherID]) ON DELETE NO ACTION ON UPDATE NO ACTION -- Khóa ngoại liên kết với bảng Vouchers
);
GO

CREATE TABLE [Apply_Vouchers]
(
    [applyVoucherID] INT IDENTITY (1,1) PRIMARY KEY, -- Mã áp dụng voucher
    [voucherID] INT,                         -- Mã voucher (liên kết với bảng Vouchers)
    [customerID] INT,                        -- Mã khách hàng (liên kết với bảng Customers)
    [status] NVARCHAR(20) DEFAULT 'active',            -- Trạng thái voucher đã được sử dụng hay chưa
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [deleted_at] DATETIME,                     -- Thời gian xóa (logic delete)
    [isDeleted] BIT DEFAULT 0,                        -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)

    FOREIGN KEY ([voucherID]) REFERENCES [Vouchers]([voucherID]) ON DELETE NO ACTION ON UPDATE NO ACTION, -- Khóa ngoại đến bảng Vouchers
    FOREIGN KEY ([customerID]) REFERENCES [Customers]([customerID]) ON DELETE NO ACTION ON UPDATE NO ACTION -- Khóa ngoại đến bảng Customers
);
GO

CREATE TABLE [Banners]
(
    [bannerID] INT IDENTITY (1,1) PRIMARY KEY,            -- Mã banner
    [title] NVARCHAR(150),                               -- Tiêu đề banner
    [imageUrl] NVARCHAR(150),                            -- Đường dẫn hình ảnh của banner
    [targetUrl] NVARCHAR(150),                           -- URL đích mà banner dẫn đến
    [createdAt] DATETIME DEFAULT CURRENT_TIMESTAMP,       -- Thời gian tạo
    [updatedAt] DATETIME,                                -- Thời gian cập nhật
    [deletedAt] DATETIME,                                -- Thời gian xóa (logic delete)
    [isDeleted] BIT DEFAULT 0,                           -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [deleted_at] DATETIME NULL,                     -- Thời gian xóa (logic delete)
);
GO

CREATE TABLE [Banner_Categories]
(
    [bannerID] INT,                        -- Mã banner (liên kết với Banners)
    [categoryID] INT,                      -- Mã danh mục (liên kết với Categories)
    [createdAt] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updatedAt] DATETIME,                           -- Thời gian cập nhật
    [deletedAt] DATETIME,                           -- Thời gian xóa (logic delete)
    [isDeleted] BIT DEFAULT 0,                      -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian cập nhật
    [deleted_at] DATETIME,                     -- Thời gian xóa (logic delete)
    PRIMARY KEY ([bannerID], [categoryID]),          -- Khóa chính

    FOREIGN KEY ([categoryID]) REFERENCES [Categories]([categoryID]) ON DELETE NO ACTION ON UPDATE NO ACTION, -- Khóa ngoại đến bảng Categories
    FOREIGN KEY ([bannerID]) REFERENCES [Banners]([bannerID]) ON DELETE NO ACTION ON UPDATE NO ACTION -- Khóa ngoại đến bảng Banners
);
GO

CREATE TABLE [Banner_Products]
(
    [bannerID] INT,                    -- Mã banner (liên kết với Banners)
    [productID] INT,                   -- Mã sản phẩm (liên kết với Products)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME,                        -- Thời gian cập nhật
    [deleted_at] DATETIME,                        -- Thời gian xóa (logic delete)
    [isDeleted] BIT DEFAULT 0,                   -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)
    [created_User] INT,                           -- Người tạo
    [updated_User] INT,                           -- Người cập nhật
    [deleted_User] INT                            -- Người xóa
    PRIMARY KEY ([bannerID], [productID]),        -- Khóa chính

    FOREIGN KEY ([bannerID]) REFERENCES [Banners]([bannerID]) ON DELETE NO ACTION ON UPDATE NO ACTION, -- Khóa ngoại đến bảng Banners
    FOREIGN KEY ([productID]) REFERENCES [Products]([productID]) ON DELETE NO ACTION ON UPDATE NO ACTION -- Khóa ngoại đến bảng Products
);
GO

CREATE TABLE [Banner_Vouchers]
(
    [bannerID] INT,                        -- Mã banner (liên kết với Banners)
    [voucherID] INT,                       -- Mã voucher (liên kết với Vouchers)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo
    [updated_at] DATETIME,                        -- Thời gian cập nhật
    [deleted_at] DATETIME,                        -- Thời gian xóa (logic delete)
    [isDeleted] BIT DEFAULT 0,                       -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)
    [created_User] INT,                           -- Người tạo
    [updated_User] INT,                           -- Người cập nhật
    [deleted_User] INT                            -- Người xóa
    PRIMARY KEY ([bannerID], [voucherID]),            -- Khóa chính

    FOREIGN KEY ([bannerID]) REFERENCES [Banners]([bannerID]) ON DELETE NO ACTION ON UPDATE NO ACTION, -- Khóa ngoại đến bảng Banners
    FOREIGN KEY ([voucherID]) REFERENCES [Vouchers]([voucherID]) ON DELETE NO ACTION ON UPDATE NO ACTION -- Khóa ngoại đến bảng Vouchers
);
GO

CREATE TABLE [Orders]
(
    [orderID] INT IDENTITY (1,1) PRIMARY KEY,        -- Mã đơn hàng
    [customerID] INT,                        -- Mã khách hàng (liên kết với Customers)
    [order_date] DATETIME DEFAULT CURRENT_TIMESTAMP,  -- Ngày đặt hàng
    [status] NVARCHAR(20) DEFAULT 'pending',           -- Trạng thái đơn hàng (pending, completed, cancelled)
    [total_price] DECIMAL(10, 2),            -- Tổng giá trị đơn hàng
    [shipping_address] NTEXT,                 -- Địa chỉ giao hàng
    [receiver] VARCHAR(150),                          -- Tên người nhận
    [ship_phone] VARCHAR(25),                         -- Số điện thoại người nhận
    [shipper_phone] CHAR(20),                         -- Số điện thoại nhân viên giao hàng
    [voucherID] INT,                                  -- Mã voucher (nếu có)
    [subtotal] DECIMAL(10, 2) DEFAULT 0,              -- Tổng giá trị trước giảm giá (nếu có)
    [total] DECIMAL(10, 2) DEFAULT 0,                 -- Tổng giá trị sau giảm giá
    [order_status] INT DEFAULT 0,                     -- Trạng thái đơn hàng (mã trạng thái)
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP,  -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP,  -- Thời gian cập nhật
    [deleted_at] DATETIME,                       -- Thời gian xóa (logic delete)
    [is_deleted] BIT DEFAULT 0,                       -- Trạng thái xóa (0: không, 1: đã xóa)
    
    FOREIGN KEY ([customerID]) REFERENCES [Customers]([customerID]) ON DELETE CASCADE,  -- Khóa ngoại đến bảng Customers
    FOREIGN KEY ([voucherID]) REFERENCES [Vouchers]([voucherID]) ON DELETE NO ACTION ON UPDATE NO ACTION,  -- Khóa ngoại đến bảng Vouchers (nếu có)
);
GO

CREATE TABLE [Order_Details]
(
    [orderDetailID] INT IDENTITY (1,1) PRIMARY KEY,    -- Mã chi tiết đơn hàng
    [orderID] INT,                             -- Mã đơn hàng (liên kết với Orders)
    [productID] INT,                           -- Mã sản phẩm (liên kết với Products)
    [quantity] INT,                            -- Số lượng sản phẩm trong đơn hàng
    [price] DECIMAL(10, 2),                    -- Giá sản phẩm tại thời điểm đặt hàng
    [created_at] DATETIME DEFAULT CURRENT_TIMESTAMP,    -- Thời gian tạo
    [updated_at] DATETIME DEFAULT CURRENT_TIMESTAMP,    -- Thời gian cập nhật
    [deleted_at] DATETIME NULL,                         -- Thời gian xóa (logic delete)
    [is_deleted] BIT DEFAULT 0,                         -- Trạng thái xóa (0: không, 1: đã xóa)

    FOREIGN KEY ([orderID]) REFERENCES [Orders]([orderID]) ON DELETE CASCADE ON UPDATE CASCADE,  -- Khóa ngoại đến bảng Orders
    FOREIGN KEY ([productID]) REFERENCES [Products]([productID]) ON DELETE NO ACTION ON UPDATE NO ACTION -- Khóa ngoại đến bảng Products
);
GO

CREATE TABLE [Payment] (
    [payID] INT IDENTITY(1,1) PRIMARY KEY,         -- Mã thanh toán
    [payName] VARCHAR(250),                         -- Tên phương thức thanh toán
    [payType] VARCHAR(250),                         -- Loại phương thức thanh toán
    [payTxnRef] VARCHAR(100),                       -- Mã giao dịch
    [orderID] INT,                           -- Mã đơn hàng (liên kết với bảng Orders)
    [isSuccess] INT,                                -- Trạng thái thành công (1: thành công, 0: thất bại)
    [customerID] INT,                           -- Mã khách hàng (liên kết với bảng Customer)
    [create_at] DATETIME DEFAULT CURRENT_TIMESTAMP,  -- Thời gian tạo
    [isDelete] BIT DEFAULT 0,                       -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)
    [payment_URL] VARCHAR(2048),                     -- URL liên kết thanh toán

    FOREIGN KEY ([orderID]) REFERENCES [Orders]([orderID]) ON DELETE NO ACTION ON UPDATE NO ACTION, -- Khóa ngoại đến bảng Orders
    FOREIGN KEY ([customerID]) REFERENCES [Customers]([customerID]) ON DELETE NO ACTION ON UPDATE NO ACTION,-- Khóa ngoại đến bảng Customer
);
GO

CREATE TABLE Contact (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Subject NVARCHAR(255) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);


CREATE TABLE [Feedback] (
    [feedbackID] INT IDENTITY(1,1) PRIMARY KEY,     -- Mã phản hồi
    [productID] INT,                            -- Mã sản phẩm (liên kết với bảng Product)
    [customerID] INT,                            -- Mã khách hàng (liên kết với bảng Customer)
    [feedback_Rate] INT,                              -- Đánh giá của khách hàng (thường là 1-5)
    [feedback_Comment] VARCHAR(250),                  -- Bình luận của khách hàng
    [create_by] INT,                                -- Người tạo
    [create_at] DATETIME DEFAULT CURRENT_TIMESTAMP,   -- Thời gian tạo
    [update_by] INT,                                -- Người cập nhật
    [update_at] DATETIME,                             -- Thời gian cập nhật
    [delete_User] INT,                                -- Người xóa
    [delete_at] DATETIME,                             -- Thời gian xóa
    [isDelete] BIT DEFAULT 0,                        -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)

    FOREIGN KEY ([productID]) REFERENCES [Products]([productID]) ON DELETE CASCADE ON UPDATE CASCADE, -- Khóa ngoại đến bảng Product
    FOREIGN KEY ([customerID]) REFERENCES [Customers]([customerID]) ON DELETE CASCADE ON UPDATE CASCADE, -- Khóa ngoại đến bảng Customer
);
GO

CREATE TABLE [Feedback_Image] (
    [feedbackImgID] INT IDENTITY(1,1) PRIMARY KEY,  -- Mã hình ảnh phản hồi
    [feedback_Image] VARCHAR(250),                    -- Đường dẫn đến hình ảnh phản hồi
    [feedbackID] INT,                        -- Mã phản hồi (liên kết với bảng Feedback)
    [create_User] INT,                                -- Người tạo
    [create_at] DATETIME DEFAULT CURRENT_TIMESTAMP,   -- Thời gian tạo
    [update_User] INT,                                -- Người cập nhật
    [update_at] DATETIME,                             -- Thời gian cập nhật
    [delete_User] INT,                                -- Người xóa
    [delete_at] DATETIME,                             -- Thời gian xóa
    [isDelete] BIT DEFAULT 0,                        -- Trạng thái xóa (0: chưa xóa, 1: đã xóa)
    FOREIGN KEY ([feedbackID]) REFERENCES [Feedback]([feedbackID]) ON DELETE CASCADE ON UPDATE CASCADE, -- Khóa ngoại đến bảng Feedback
);
GO

INSERT INTO [Admin] ([first_name], [last_name], [username], [password], [email], [picture], [role], [status])
VALUES 
(
    N'Trung', N'Nguyễn', 'admin_trung', 'password123', 'admin_trung@example.com', 'https://example.com/profile-picture.png', 'super_admin', N'Hoạt động'
);

-- Chèn dữ liệu vào bảng Categories cho trang web boardgame Xanh Đấu
INSERT INTO [Categories] ([name], [description], [parentID], [created_by], [updated_by], [status])
VALUES
    (N'Bản chơi', N'Danh mục chứa các loại bảng chơi cho game Xanh Đấu', NULL, 1, 1, N'Đang bán'), -- Danh mục chính
    (N'Phụ kiện', N'Danh mục chứa các phụ kiện như quân cờ, xúc xắc, quân bài cho game Xanh Đấu', NULL, 1, 1, N'Đang bán'), -- Danh mục chính
    (N'Bộ bài', N'Danh mục chứa các bộ bài sử dụng trong game Xanh Đấu', NULL, 1, 1, N'Đang bán'), -- Danh mục chính
    (N'Phiên bản Xuân', N'Danh mục con của game Xanh Đấu, dành cho phiên bản Xuân', 1, 1, 1, N'Đang bán'), -- Danh mục con của "Bảng chơi"
    (N'Phiên bản Hạ', N'Danh mục con của game Xanh Đấu, dành cho phiên bản Hạ', 1, 1, 1, N'Đang bán'), -- Danh mục con của "Bảng chơi"
    (N'Phiên bản Thu', N'Danh mục con của game Xanh Đấu, dành cho phiên bản Thu', 1, 1, 1, N'Đang bán'), -- Danh mục con của "Bảng chơi"
    (N'Phiên bản Đông', N'Danh mục con của game Xanh Đấu, dành cho phiên bản Đông', 1, 1, 1, N'Đang bán'); -- Danh mục con của "Bảng chơi"




SELECT * FROM Customers
SELECT * FROM Products
SELECT * FROM Categories
SELECT * FROM Product_Images
Select * from Orders
SELECT * FROM Admin
Select * from Address