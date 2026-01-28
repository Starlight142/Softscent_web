-- Add tables for User Banks and Addresses

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserBanks]') AND type in (N'U'))
BEGIN
    CREATE TABLE UserBanks (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId NVARCHAR(450) NOT NULL,
        BankName NVARCHAR(100),
        AccountName NVARCHAR(200),
        AccountNumber NVARCHAR(50),
        IsDefault BIT DEFAULT 0,
        CreatedDate DATETIME2 DEFAULT GETDATE()
        -- Note: Foreign key constraint logic handled in application or assume Users table exists
        -- CONSTRAINT FK_UserBanks_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserAddresses]') AND type in (N'U'))
BEGIN
    CREATE TABLE UserAddresses (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId NVARCHAR(450) NOT NULL,
        FullName NVARCHAR(200),
        PhoneNumber NVARCHAR(50),
        AddressLine NVARCHAR(MAX),
        Province NVARCHAR(100),
        PostalCode NVARCHAR(20),
        IsDefault BIT DEFAULT 0,
        CreatedDate DATETIME2 DEFAULT GETDATE()
        -- CONSTRAINT FK_UserAddresses_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END
