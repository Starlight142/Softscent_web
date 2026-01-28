-- Run this script in your SQL Server database to create the necessary tables

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[News]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[News] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Title] NVARCHAR(255) NOT NULL,
        [Content] NVARCHAR(MAX) NOT NULL,
        [ImageUrl] NVARCHAR(MAX) NULL,
        [PublishedDate] DATETIME DEFAULT GETDATE()
    );
    
    -- Insert some sample news
    INSERT INTO [dbo].[News] (Title, Content, ImageUrl, PublishedDate) VALUES 
    (N'เปิดตัวคอลเลคชั่นใหม่ "Sense of Nature"', N'ค้นพบพลังแห่งธรรมชาติกับยาดมสมุนไพรสูตรใหม่ที่ผสานกลิ่นอายของป่าเขาและสายน้ำ ช่วยให้คุณผ่อนคลายได้ทุกที่ทุกเวลา...', N'https://images.unsplash.com/photo-1602166242292-93a13e5d9b05?auto=format&fit=crop&q=80&w=800', GETDATE()),
    (N'5 ประโยชน์ของการดมกลิ่นบำบัดที่คุณอาจไม่รู้', N'การดมกลิ่นหอมไม่ใช่แค่เรื่องของความสดชื่น แต่ยังมีผลต่อสมองและอารมณ์ของคุณโดยตรง เรียนรู้วิธีใช้ประโยชน์จากกลิ่น...', N'https://images.unsplash.com/photo-1512413914633-b5043f4041ea?auto=format&fit=crop&q=80&w=800', DATEADD(day, -5, GETDATE())),
    (N'Workshop: ปรุงกลิ่นยาดมในแบบของคุณ', N'เชิญชวนผู้ที่สนใจเข้าร่วมกิจกรรม Workshop สุดพิเศษ ที่จะพาคุณไปเรียนรู้ศาสตร์แห่งการปรุงกลิ่นและทำยาดมสูตรเฉพาะตัว...', N'https://images.unsplash.com/photo-1532413992378-f169ac26fff0?auto=format&fit=crop&q=80&w=800', DATEADD(day, -10, GETDATE()));
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reviews]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Reviews] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ProductId] INT NOT NULL DEFAULT 0,
        [UserId] NVARCHAR(128) NOT NULL, -- Assuming matches Users table
        [Rating] INT NOT NULL,
        [Comment] NVARCHAR(MAX) NULL,
        [CreatedDate] DATETIME DEFAULT GETDATE()
    );
    
    -- Insert some sample reviews (You might need valid UserIds if you have foreign keys, but this is loose)
    -- This part is commented out to avoid FK errors if your Users table is empty or different
    /* 
    INSERT INTO [dbo].[Reviews] (ProductId, UserId, Rating, Comment, CreatedDate) VALUES 
    (0, 'test_user_id', 5, N'สินค้าดีมาก ชอบมากค่ะ', GETDATE()); 
    */
END
