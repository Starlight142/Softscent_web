using System;
using System.Collections.Generic;
using System.Data;

namespace Softscent.Data
{
    public static class MockDataStore
    {
        public static DataSet Database;

        static MockDataStore()
        {
            Database = new DataSet("SoftscentLegacy");
            InitializeTables();
            SeedData();
        }

        private static void InitializeTables()
        {
            // Users
            DataTable users = new DataTable("Users");
            users.Columns.Add("Id", typeof(string));
            users.Columns.Add("UserName", typeof(string));
            users.Columns.Add("NormalizedUserName", typeof(string));
            users.Columns.Add("Email", typeof(string));
            users.Columns.Add("NormalizedEmail", typeof(string));
            users.Columns.Add("EmailConfirmed", typeof(bool));
            users.Columns.Add("PasswordHash", typeof(string));
            users.Columns.Add("SecurityStamp", typeof(string));
            users.Columns.Add("ConcurrencyStamp", typeof(string));
            users.Columns.Add("PhoneNumber", typeof(string));
            users.Columns.Add("PhoneNumberConfirmed", typeof(bool));
            users.Columns.Add("TwoFactorEnabled", typeof(bool));
            users.Columns.Add("LockoutEnd", typeof(DateTimeOffset));
            users.Columns.Add("LockoutEnabled", typeof(bool));
            users.Columns.Add("AccessFailedCount", typeof(int));
            users.Columns.Add("FullName", typeof(string));
            users.Columns.Add("Address", typeof(string));
            users.Columns.Add("City", typeof(string));
            users.Columns.Add("PostalCode", typeof(string));
            users.Columns.Add("Gender", typeof(string));
            users.Columns.Add("BirthDate", typeof(DateTime));
            Database.Tables.Add(users);

            // Roles
            DataTable roles = new DataTable("Roles");
            roles.Columns.Add("Id", typeof(string));
            roles.Columns.Add("Name", typeof(string));
            roles.Columns.Add("NormalizedName", typeof(string));
            roles.Columns.Add("ConcurrencyStamp", typeof(string));
            Database.Tables.Add(roles);

            // UserRoles
            DataTable userRoles = new DataTable("UserRoles");
            userRoles.Columns.Add("UserId", typeof(string));
            userRoles.Columns.Add("RoleId", typeof(string));
            Database.Tables.Add(userRoles);

            // Products
            DataTable products = new DataTable("Products");
            products.Columns.Add("Id", typeof(int));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("NameThai", typeof(string));
            products.Columns.Add("Description", typeof(string));
            products.Columns.Add("DescriptionThai", typeof(string));
            products.Columns.Add("Price", typeof(decimal));
            products.Columns.Add("ImageUrl", typeof(string));
            products.Columns.Add("IsCustomizable", typeof(bool));
            products.PrimaryKey = new DataColumn[] { products.Columns["Id"] };
            products.Columns["Id"].AutoIncrement = true;
            products.Columns["Id"].AutoIncrementSeed = 1;
            Database.Tables.Add(products);

            // Herbs
            DataTable herbs = new DataTable("Herbs");
            herbs.Columns.Add("Id", typeof(int));
            herbs.Columns.Add("Name", typeof(string));
            herbs.Columns.Add("Description", typeof(string));
            herbs.Columns.Add("Price", typeof(decimal));
            herbs.Columns.Add("Benefit", typeof(string));
            herbs.PrimaryKey = new DataColumn[] { herbs.Columns["Id"] };
            herbs.Columns["Id"].AutoIncrement = true;
            herbs.Columns["Id"].AutoIncrementSeed = 1;
            Database.Tables.Add(herbs);

            // Orders
            DataTable orders = new DataTable("Orders");
            orders.Columns.Add("Id", typeof(int));
            orders.Columns.Add("UserId", typeof(string));
            orders.Columns.Add("OrderDate", typeof(DateTime));
            orders.Columns.Add("TotalAmount", typeof(decimal));
            orders.Columns.Add("Status", typeof(string));
            orders.Columns.Add("PaymentStatus", typeof(string));
            orders.Columns.Add("ShippingAddress", typeof(string));
            orders.Columns.Add("ShippingMethod", typeof(string));
            orders.Columns.Add("PaymentMethod", typeof(string));
            orders.PrimaryKey = new DataColumn[] { orders.Columns["Id"] };
            orders.Columns["Id"].AutoIncrement = true;
            orders.Columns["Id"].AutoIncrementSeed = 1;
            Database.Tables.Add(orders);

            // OrderDetails
            DataTable orderDetails = new DataTable("OrderDetails");
            orderDetails.Columns.Add("Id", typeof(int));
            orderDetails.Columns.Add("OrderId", typeof(int));
            orderDetails.Columns.Add("ProductId", typeof(int));
            orderDetails.Columns.Add("Quantity", typeof(int));
            orderDetails.Columns.Add("UnitPrice", typeof(decimal));
            orderDetails.Columns.Add("CustomConfiguration", typeof(string));
            orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["Id"] };
            orderDetails.Columns["Id"].AutoIncrement = true;
            orderDetails.Columns["Id"].AutoIncrementSeed = 1;
            Database.Tables.Add(orderDetails);

            // News
            DataTable news = new DataTable("News");
            news.Columns.Add("Id", typeof(int));
            news.Columns.Add("Title", typeof(string));
            news.Columns.Add("Content", typeof(string));
            news.Columns.Add("ImageUrl", typeof(string));
            news.Columns.Add("CreatedDate", typeof(DateTime));
            news.Columns.Add("IsPublished", typeof(bool));
            news.PrimaryKey = new DataColumn[] { news.Columns["Id"] };
            news.Columns["Id"].AutoIncrement = true;
            news.Columns["Id"].AutoIncrementSeed = 1;
            Database.Tables.Add(news);

            // Reviews
            DataTable reviews = new DataTable("Reviews");
            reviews.Columns.Add("Id", typeof(int));
            reviews.Columns.Add("ProductId", typeof(int));
            reviews.Columns.Add("UserId", typeof(string));
            reviews.Columns.Add("Rating", typeof(int));
            reviews.Columns.Add("Comment", typeof(string));
            reviews.Columns.Add("CreatedDate", typeof(DateTime));
            reviews.PrimaryKey = new DataColumn[] { reviews.Columns["Id"] };
            reviews.Columns["Id"].AutoIncrement = true;
            reviews.Columns["Id"].AutoIncrementSeed = 1;
            Database.Tables.Add(reviews);
            
             // SupportMessages
            DataTable support = new DataTable("SupportMessages");
            support.Columns.Add("Id", typeof(int));
            support.Columns.Add("UserId", typeof(string));
            support.Columns.Add("Subject", typeof(string));
            support.Columns.Add("Message", typeof(string));
            support.Columns.Add("AdminReply", typeof(string));
            support.Columns.Add("IsResolved", typeof(bool));
            support.Columns.Add("CreatedDate", typeof(DateTime));
            support.PrimaryKey = new DataColumn[] { support.Columns["Id"] };
            support.Columns["Id"].AutoIncrement = true;
            support.Columns["Id"].AutoIncrementSeed = 1;
            Database.Tables.Add(support);
        }

        private static void SeedData()
        {
            // Seed Users
            var users = Database.Tables["Users"];
            users.Rows.Add(
                "1", "admin@softscent.com", "ADMIN@SOFTSCENT.COM", "admin@softscent.com", "ADMIN@SOFTSCENT.COM", 
                true, "1234", "securitystamp", "concurrencystamp", "1234567890", true, false, null, false, 0, 
                "Admin User", "123 Admin St", "Bangkok", "10110", "Other", DateTime.Now
            );

             // Seed Roles
            var roles = Database.Tables["Roles"];
            roles.Rows.Add("1", "Admin", "ADMIN", "stamp");
            roles.Rows.Add("2", "User", "USER", "stamp");

            // Seed UserRoles
            var userRoles = Database.Tables["UserRoles"];
            userRoles.Rows.Add("1", "1"); // Admin has Admin role

            // Seed Products
            var products = Database.Tables["Products"];
            products.Rows.Add(null, "Traditional Thai Jar", "ยาดมสมุนไพรแบบกระปุก", "Original Thai herbal inhaler made from fermented herbs.", "สูตรต้นตำรับจากสมุนไพรหมัก กลิ่นหอมเอกลักษณ์ไทย", 150.00, "Images/jar.png", false);
            products.Rows.Add(null, "Peppermint Fresh", "เปปเปอร์มิ้นท์ เฟรช", "Instant freshness and headache relief.", "เย็นสดชื่นทันที ช่วยให้ตื่นตัวและแก้ปวดหัว", 120.00, "Images/peppermint.png", false);
            products.Rows.Add(null, "Lavender Sleep", "ลาเวนเดอร์ สลีป", "Relaxing scent for a better sleep.", "กลิ่นหอมผ่อนคลาย ช่วยให้หลับสนิทตลอดคืน", 180.00, "Images/lavender.png", false);
            products.Rows.Add(null, "Custom Blend Inhaler", "ยาดมผสมเอง", "Create your own scent.", "ปรุงกลิ่นที่คุณต้องการได้ด้วยตัวเอง", 250.00, "Images/custom.png", true);

            // Seed Herbs
             var herbs = Database.Tables["Herbs"];
            herbs.Rows.Add(null, "Menthol", "Cooling sensation", 10.00, "Refreshes and clears nasal passages");
            herbs.Rows.Add(null, "Eucalyptus", "Strong aroma", 15.00, "Helps with breathing");
            herbs.Rows.Add(null, "Cinnamon", "Warm and spicy", 20.00, "Stimulates blood flow");
        }
    }
}
