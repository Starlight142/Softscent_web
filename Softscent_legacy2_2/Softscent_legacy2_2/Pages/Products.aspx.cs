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
        // Ensure database schema has necessary translation columns
        try { DataHelper.EnsureTranslationColumns(); } catch { /* Ignore permissions issues */ }

        // Fetch all products to allow for client-side real-time filtering
        // This is efficient for small-to-medium datasets and provides the best UX (instant feedback)
        string query = "SELECT * FROM Products";
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
            var p = new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                // Store base values
                Price = Convert.ToDecimal(row["Price"]),
                ImageUrl = row["ImageUrl"].ToString(),
                IsCustomizable = Convert.ToBoolean(row["IsCustomizable"]),
                StockQuantity = row["StockQuantity"] != DBNull.Value ? Convert.ToInt32(row["StockQuantity"]) : 0,

                // Retrieve Thai translations if available
                NameThai = row.Table.Columns.Contains("NameThai") ? row["NameThai"].ToString() : null,
                DescriptionThai = row.Table.Columns.Contains("DescriptionThai") ? row["DescriptionThai"].ToString() : null
            };

            // Set Display Properties (Prioritize Thai)
            p.Name = !string.IsNullOrEmpty(p.NameThai) ? p.NameThai : row["Name"].ToString();
            p.Description = !string.IsNullOrEmpty(p.DescriptionThai) ? p.DescriptionThai : row["Description"].ToString();

            ProductList.Add(p);
        }
    }
}
