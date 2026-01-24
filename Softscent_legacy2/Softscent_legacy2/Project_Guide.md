# คู่มือการพัฒนาเว็บไซต์ Softscent (Project Developer Guide)

เอกสารนี้จัดทำขึ้นเพื่อให้เพื่อนร่วมทีมหรือนักพัฒนาที่มาสานต่อเข้าใจโครงสร้างและการทำงานของโปรเจกต์ Softscent Legacy

## 1. โครงสร้างไฟล์ที่สำคัญ (Important File Structure)

### ส่วนกลาง (Core & Shared)
*   **[MasterPage.master](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/MasterPage.master)**: ไฟล์เทมเพลตหลักของเว็บ (Navbar, Footer, โครงสร้าง CSS) แก้ไขเมนูที่นี่
*   **[Web.config](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/Web.config)**: การตั้งค่าระบบ (Configuration) รวมถึงการตั้งค่า Encoding (UTF-8) และ Connection String ของฐานข้อมูล
*   **[App_Code/DataHelper.cs](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/App_Code/DataHelper.cs)**: คลาสส่วนกลางสำหรับติดต่อฐานข้อมูล (CRUD operations)

### หน้าเว็บ (Main Pages - /Pages)
*   **[index.aspx](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/index.aspx)**: หน้าแรกของเว็บไซต์
*   **[Products.aspx](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/Pages/Products.aspx)**: รายการสินค้าทั้งหมด
*   **[Custom.aspx](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/Pages/Custom.aspx)**: หน้าปรุงยาดมสูตรเฉพาะ (Crafting system)
*   **[Cart.aspx](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/Pages/Cart.aspx)** & **[Checkout.aspx](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/Pages/Checkout.aspx)**: ระบบตะกร้าและการชำระเงิน
*   **[Orders.aspx](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/Pages/Orders.aspx)**: ประวัติการสั่งซื้อของผู้ใช้

## 2. ระบบการแปลภาษา (Localization System)

เนื่องจากระบบเดิมเป็นภาษาอังกฤษและข้อมูลบางส่วนมาจากฐานข้อมูล เราจึงใช้การแปล 3 รูปแบบ:

1.  **Direct Inline (ในไฟล์ .aspx)**: สำหรับข้อความคงที่ (Static text) ให้แก้ในไฟล์ HTML ได้เลย
2.  **Code Mapping (ในไฟล์ .aspx.cs)**: สำหรับข้อมูลที่ดึงมาจากฐานข้อมูล (เช่น ชื่อสินค้า, สรรพคุณสมุนไพร) ดูฟังก์ชัน `GetProductThaiName`, `GetHerbThaiBenefit` เป็นต้น
3.  **Language Helper**: ไฟล์ [App_Code/LangHelper.cs](file:///D:/ProjectHTML/Projectwelove/Softscent_legacy2/Softscent_legacy2/App_Code/LangHelper.cs) สำหรับเก็บข้อความส่วนกลางที่ใช้ร่วมกันในหน้าปรุงยาดม

## 3. การเชื่อมต่อฐานข้อมูล (Database Connection)

หากต้องการเปลี่ยนฐานข้อมูล ให้ไปที่ **Web.config** ในส่วน `<connectionStrings>`:
```xml
<add name="DefaultConnection" connectionString="Server=YOUR_SERVER;Database=SoftscentLegacy;User Id=YOUR_USER;Password=YOUR_PASSWORD;" />
```

---
*หมายเหตุ: โปรเจกต์นี้ใช้ ASP.NET Web Forms (.NET Framework 4.7.2)*
