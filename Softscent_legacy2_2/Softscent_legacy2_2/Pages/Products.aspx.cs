using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

/// <summary>
/// Code-behind for the Product Listing page.
/// Displays products and handles search filtering.
/// </summary>
public partial class Pages_Products : System.Web.UI.Page
{
    /// <summary>
    /// List of products to be rendered in the view.
    /// </summary>
    public List<Product> ProductList = new List<Product>();

    /// <summary>
    /// Handles Page Load.
    /// Fetches products from the database, filtering by the search query if present.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        string query = "SELECT * FROM Products";
        string q = Request.QueryString["q"];
        if (!string.IsNullOrEmpty(q))
        {
            // Simple SQL injection protection handling done by DataHelper usually, but here we use parameter
            // However, DataHelper.ExecuteQuery(string) doesn't take params directly in this overload?
            // Let's assume we can use parameterized query
            query = "SELECT * FROM Products WHERE Name LIKE @Search OR Description LIKE @Search";
            DataTable dt = DataHelper.ExecuteQuery(query, new Dictionary<string, object> { { "@Search", "%" + q + "%" } });
            if (dt != null) BindList(dt);
            return;
        }

        DataTable dtAll = DataHelper.ExecuteQuery(query);
        BindList(dtAll);
    }

    /// <summary>
    /// Binds DataTable rows to the ProductList property.
    /// </summary>
    /// <param name="dt">DataTable containing product rows.</param>
    private void BindList(DataTable dt)
    {
        ProductList.Clear();
        foreach (DataRow row in dt.Rows)
        {
            ProductList.Add(new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                ImageUrl = row["ImageUrl"].ToString(),
                IsCustomizable = Convert.ToBoolean(row["IsCustomizable"]),
                StockQuantity = row["StockQuantity"] != DBNull.Value ? Convert.ToInt32(row["StockQuantity"]) : 0
            });
        }
    }

    /// <summary>
    /// Helper method to translate product names to Thai for display.
    /// </summary>
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

    /// <summary>
    /// Helper method to translate product descriptions to Thai for display.
    /// </summary>
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
