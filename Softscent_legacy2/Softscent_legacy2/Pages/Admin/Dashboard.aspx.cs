using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

public partial class Pages_Admin_Dashboard : System.Web.UI.Page
{
    public int TotalOrders = 0;
    public decimal TotalRevenue = 0;
    public int PendingOrders = 0;
    public List<Order> RecentOrders = new List<Order>();

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadStats();
        LoadRecentOrders();
    }

    private void LoadStats()
    {
        // Mock data logic or query DB if table exists and has data
        // For now, let's use DataHelper if table is populated
        try {
            DataTable dt = DataHelper.ExecuteQuery("SELECT COUNT(*) as Count, SUM(TotalAmount) as Revenue FROM Orders");
            if (dt.Rows.Count > 0)
            {
                TotalOrders = dt.Rows[0]["Count"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Count"]) : 0;
                TotalRevenue = dt.Rows[0]["Revenue"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["Revenue"]) : 0;
            }
        } catch { } // Table might be empty or not exist yet
    }

    private void LoadRecentOrders()
    {
        try {
            DataTable dt = DataHelper.ExecuteQuery("SELECT TOP 5 * FROM Orders ORDER BY OrderDate DESC");
            foreach (DataRow row in dt.Rows)
            {
                RecentOrders.Add(new Order
                {
                    Id = Convert.ToInt32(row["Id"]),
                    OrderDate = Convert.ToDateTime(row["OrderDate"]),
                    Status = row["Status"].ToString(),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"])
                });
            }
        } catch { }
    }
}
