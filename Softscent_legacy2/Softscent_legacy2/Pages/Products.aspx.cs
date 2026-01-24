using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

public partial class Pages_Products : System.Web.UI.Page
{
    public List<Product> ProductList = new List<Product>();

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Products");
        foreach (DataRow row in dt.Rows)
        {
            ProductList.Add(new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                ImageUrl = row["ImageUrl"].ToString(),
                IsCustomizable = Convert.ToBoolean(row["IsCustomizable"])
            });
        }
    }
    public string GetProductThaiName(string name)
    {
        string n = name.ToLower();
        if (n.Contains("traditional thai jar")) return "ยาดมสมุนไพรแบบกระปุก";
        if (n.Contains("peppermint fresh")) return "เปปเปอร์มิ้นท์ เฟรช";
        if (n.Contains("lavender sleep")) return "ลาเวนเดอร์ สลีป";
        if (n.Contains("citrus energy")) return "ซิทรัส เอนเนอร์จี";
        if (n.Contains("eucalyptus clear")) return "ยูคาลิปตัส เคลียร์";
        if (n.Contains("lemongrass zen")) return "ตะไคร้หอม เซน";
        return name;
    }

    public string GetProductThaiDescription(string name, string description)
    {
        string n = name.ToLower();
        if (n.Contains("traditional thai jar")) return "สูตรต้นตำรับจากสมุนไพรหมัก กลิ่นหอมเอกลักษณ์ไทย";
        if (n.Contains("peppermint fresh")) return "เย็นสดชื่นทันที ช่วยให้ตื่นตัวและแก้ปวดหัว";
        if (n.Contains("lavender sleep")) return "กลิ่นหอมผ่อนคลาย ช่วยให้หลับสนิทตลอดคืน";
        if (n.Contains("citrus energy")) return "เติมพลังให้ร่างกายด้วยกลิ่นส้มสดชื่น";
        if (n.Contains("eucalyptus clear")) return "ช่วยให้หายใจโล่ง แก้คัดจมูกอย่างได้ผล";
        if (n.Contains("lemongrass zen")) return "สัมผัสความผ่อนคลายเหมือนอยู่ในสปา";
        return description;
    }
}
