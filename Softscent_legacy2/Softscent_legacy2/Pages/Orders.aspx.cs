using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

public partial class Pages_Orders : System.Web.UI.Page
{
    public List<Order> OrderList = new List<Order>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        LoadOrders();
    }

    private void LoadOrders()
    {
        string userEmail = Session["User"].ToString();
        // Assuming UserId in Orders table is the email for simple legacy auth or we need to look up ID
        // The previous cart logic used "Guest".
        // Let's assume for this feature, we query by UserId column, but since our current simple login uses Email in Session...
        // We probably need to fetch the User's ID from the Users table first, OR if we modified Checkout to save Email as UserId.

        // Let's check how Checkout was implemented in Cart.aspx.cs
        // Wait, Checkout in Cart.aspx.cs used "@UserId" = "Guest".
        // We need to fix Checkout to use the logged-in user!

        // For now, let's look up the User's ID from the email
        string userId = GetUserIdFromEmail(userEmail);

        string query = "SELECT * FROM Orders WHERE UserId = @UserId ORDER BY OrderDate ASC";
        var parameters = new Dictionary<string, object>
         {
             { "@UserId", userId }
         };

        DataTable dt = DataHelper.ExecuteQuery(query, parameters);
        foreach (DataRow row in dt.Rows)
        {
            OrderList.Add(new Order
            {
                Id = Convert.ToInt32(row["Id"]),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                Status = row["Status"].ToString()
            });
        }
    }

    private string GetUserIdFromEmail(string email)
    {
        // We might simply return the email if UserId column holds strings (which is common in Identity UserIds)
        // But let's check Users table ID.
        // Actually, ASP.NET Identity UsertId is a Guid string (NVARCHAR).

        DataTable dt = DataHelper.ExecuteQuery("SELECT Id FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["Id"].ToString();
        }
        return "Guest";
    }

    public string GetStatusColor(string status)
    {
        switch (status.ToLower())
        {
            case "completed": return "success";
            case "pending": return "warning";
            case "cancelled": return "danger";
            default: return "secondary";
        }
    }
}
