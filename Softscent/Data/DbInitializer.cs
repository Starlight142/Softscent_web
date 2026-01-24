using Softscent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Softscent.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // context.Database.EnsureCreated();
        await context.Database.MigrateAsync();

        // Seed Roles
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Admin User
        var adminEmail = "admin@softscent.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Address = "Softscent HQ, Bangkok, Thailand",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Look for any products.
        if (context.Products.Any())
        {
            return;   // DB has been seeded
        }

        var products = new Product[]
        {
            new Product{Name="Peppermint Inhaler", NameThai="เปปเปอร์มิ้นท์ เฟรช", Price=5.00m, Description="Classic refreshing peppermint scent.", DescriptionThai="เย็นสดชื่นทันทีและช่วยบรรเทาอาการปวดหัว", IsCustomizable=false, ImageUrl="/images/products/peppermint.png"},
            new Product{Name="Lavender Sleep Inhaler", NameThai="ลาเวนเดอร์ สลีป", Price=6.50m, Description="Calming lavender for better sleep.", DescriptionThai="ผ่อนคลายและช่วยให้หลับสนิทยิ่งขึ้น", IsCustomizable=false, ImageUrl="/images/products/lavender.png"},
            new Product{Name="Citrus Energy Inhaler", NameThai="ส้ม ซิตรัส เอเนอร์จี้", Price=6.00m, Description="Zesty citrus to boost your energy.", DescriptionThai="หอมสดชื่นจากผิวส้ม ช่วยปลุกพลังระหว่างวัน", IsCustomizable=false, ImageUrl="/images/products/citrus.png"},
            new Product{Name="Traditional Thai Herbal Jar", NameThai="ยาดมกระปุกยาจีนโบราณ", Price=8.00m, Description="Authentic Thai herbal blend in a traditional jar. Contains clove, star anise, and camphor.", DescriptionThai="สูตรต้นตำรับจากสมุนไพรหมัก", IsCustomizable=false, ImageUrl="/images/products/thai_jar.png"},
            new Product{Name="Custom Inhaler Blend", NameThai="จัดยาดมเองตามใจชอบ", Price=10.00m, Description="Your unique blend of herbs.", DescriptionThai="สูตรพิเศษเลือกผสมเองเพื่อคุณ", IsCustomizable=true, ImageUrl="/images/products/thai_jar.png"}
        };
        context.Products.AddRange(products);

        var herbs = new Herb[]
        {
            new Herb{Name="Peppermint", NameThai="เปปเปอร์มิ้นท์", Price=0.50m, Benefit="Refreshing", BenefitThai="ช่วยให้เย็นสดชื่นทันที บรรเทาอาการคัดจมูก"},
            new Herb{Name="Eucalyptus", NameThai="ยูคาลิปตัส", Price=0.50m, Benefit="Clearing", BenefitThai="บรรเทาอาการหวัด คัดจมูก และฆ่าเชื้อในระบบทางเดินหายใจ"},
            new Herb{Name="Lavender", NameThai="ลาเวนเดอร์", Price=1.00m, Benefit="Calming", BenefitThai="ช่วยให้ผ่อนคลาย หลับสบาย บรรเทาความเครียด"},
            new Herb{Name="Lemongrass", NameThai="ตะไคร้หอม", Price=0.75m, Benefit="Energizing", BenefitThai="ช่วยให้จิตใจสงบ แก้ปวดศีรษะ"},
            new Herb{Name="Bergamot", NameThai="มะกรูด", Price=1.20m, Benefit="Uplifting", BenefitThai="ปรับสมดุลอารมณ์ ลดความกังวล"},
            new Herb{Name="Rosemary", NameThai="โรสแมรี่", Price=0.80m, Benefit="Focus", BenefitThai="ช่วยให้มีสมาธิและจดจำได้ดีขึ้น"},
            // Thai Traditional Herbs
            new Herb{Name="Borneol (Phimsen)", NameThai="พิมเสน", Price=1.50m, Benefit="Cooling & Respiratory Aid", BenefitThai="บำรุงหัวใจ แก้หน้ามืด และอาการวิงเวียน"},
            new Herb{Name="Camphor (Karaboon)", NameThai="การบูร", Price=1.00m, Benefit="Relieves Dizziness", BenefitThai="ทำให้หายใจสะดวก แก้หน้ามืดตาลาย"},
            new Herb{Name="Star Anise (Poy Kak)", NameThai="โป๊ยกั๊ก", Price=1.20m, Benefit="Warming Aroma", BenefitThai="กลิ่นหอมอุ่น ช่วยขับเสมหะและแก้ไอ"},
            new Herb{Name="Clove (Kan Phlu)", NameThai="กานพลู", Price=1.20m, Benefit="Spicy & Clearing", BenefitThai="กลิ่นหอมเผ็ดร้อน ช่วยแก้ท้องอืดและบรรเทาอาการปวด"},
            new Herb{Name="Cinnamon (Ob Choey)", NameThai="อบเชย", Price=1.00m, Benefit="Stimulates Circulation", BenefitThai="ช่วยให้เลือดลมไหลเวียนดี กระตุ้นความจำ"},
            new Herb{Name="Nutmeg (Look Jun)", NameThai="ลูกจันทน์", Price=1.50m, Benefit="Relaxing & Soothing", BenefitThai="กลิ่นหอมหวาน ช่วยให้นอนหลับง่ายและผ่อนคลาย"}
        };
        context.Herbs.AddRange(herbs);

        await context.SaveChangesAsync();
    }
}
