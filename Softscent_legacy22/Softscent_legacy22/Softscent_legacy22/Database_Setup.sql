-- =============================================
-- คำสั่งสำหรับการสร้างฐานข้อมูล SoftscentLegacy (Complete Schema)
-- ครอบคลุมทั้งระบบสมาชิก (Identity), สินค้า, การสั่งซื้อ และฟีเจอร์เสริม
-- =============================================

-- 1. ตารางระบบสมาชิก (ASP.NET Identity Infrastructure)
CREATE TABLE Roles (
    Id NVARCHAR(450) PRIMARY KEY,
    Name NVARCHAR(256),
    NormalizedName NVARCHAR(256),
    ConcurrencyStamp NVARCHAR(MAX)
);

CREATE TABLE Users (
    Id NVARCHAR(450) PRIMARY KEY,
    UserName NVARCHAR(256),
    NormalizedUserName NVARCHAR(256),
    Email NVARCHAR(256),
    NormalizedEmail NVARCHAR(256),
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX),
    SecurityStamp NVARCHAR(MAX),
    ConcurrencyStamp NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DATETIMEOFFSET,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL,
    -- คอลัมน์เพิ่มเติมของ Softscent
    FullName NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    City NVARCHAR(MAX),
    PostalCode NVARCHAR(MAX),
    Gender NVARCHAR(20),
    BirthDate DATETIME
);

CREATE TABLE UserRoles (
    UserId NVARCHAR(450) NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    RoleId NVARCHAR(450) NOT NULL REFERENCES Roles(Id) ON DELETE CASCADE,
    PRIMARY KEY (UserId, RoleId)
);

-- 2. ตารางสินค้าและส่วนผสม (Products & Herbs)
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    NameThai NVARCHAR(200),
    Description NVARCHAR(MAX),
    DescriptionThai NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    ImageUrl NVARCHAR(MAX),
    IsCustomizable BIT DEFAULT 0 -- 1 = ปรุงเองได้
);

CREATE TABLE Herbs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    Benefit NVARCHAR(MAX)
);

-- 3. ตารางระบบสั่งซื้อ (Orders & OrderDetails)
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES Users(Id),
    OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18, 2) NOT NULL,
    Status NVARCHAR(MAX) DEFAULT 'Pending',
    PaymentStatus NVARCHAR(MAX) DEFAULT 'Unpaid',
    ShippingAddress NVARCHAR(MAX),
    ShippingMethod NVARCHAR(MAX),
    PaymentMethod NVARCHAR(MAX)
);

CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(Id) ON DELETE CASCADE,
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    CustomConfiguration NVARCHAR(MAX) -- เก็บสูตรปรุงยาแบบกำหนดเอง
);

-- 4. ตารางฟีเจอร์อื่นๆ (News, Reviews, Support)
CREATE TABLE News (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(MAX) NOT NULL,
    Content NVARCHAR(MAX),
    ImageUrl NVARCHAR(MAX),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    IsPublished BIT DEFAULT 1
);

CREATE TABLE Reviews (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES Users(Id),
    Rating INT NOT NULL,
    Comment NVARCHAR(MAX),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE SupportMessages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) FOREIGN KEY REFERENCES Users(Id),
    Subject NVARCHAR(MAX) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    AdminReply NVARCHAR(MAX),
    IsResolved BIT DEFAULT 0,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- =============================================
-- ตัวอย่างข้อมูลเบื้องต้นสำหรับการทดสอบ (Optional)
-- =============================================
-- INSERT INTO Roles (Id, Name, NormalizedName) VALUES ('admin-role', 'Admin', 'ADMIN');
-- INSERT INTO Users (Id, Email, EmailConfirmed, PasswordHash, FullName) VALUES ('test-user', 'admin@softscent.com', 1, '...', 'Administrator');
-- INSERT INTO UserRoles (UserId, RoleId) VALUES ('test-user', 'admin-role');
