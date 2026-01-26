using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

/// <summary>
/// Code-behind for the Order History page.
/// Displays a list of previous orders for the logged-in user.
/// </summary>
public partial class Pages_Orders : System.Web.UI.Page
{
    /// <summary>
    /// List of orders retrieved for display.
    /// </summary>
    public List<Order> OrderList = new List<Order>();

    /// <summary>
    /// Handles the Page Load event. 
    /// Verifies the user is logged in before attempting to load their order history.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        LoadOrders();
    }

    /// <summary>
    /// Queries the database for all orders belonging to the current user.
    /// </summary>
    private void LoadOrders()
    {
        string userEmail = Session["User"].ToString();

        // Fetch the unique User ID associated with the email in the Session
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

    /// <summary>
    /// Helper to look up a User ID in the database based on their email address.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>The string representation of the User ID, or 'Guest' if not found.</returns>
    private string GetUserIdFromEmail(string email)
    {
        DataTable dt = DataHelper.ExecuteQuery("SELECT Id FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["Id"].ToString();
        }
        return "Guest";
    }

    /// <summary>
    /// Determines the Bootstrap color class based on the order status.
    /// </summary>
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

    /// <summary>
    /// Translates the order status string to Thai for display.
    /// </summary>
    public string GetThaiStatus(string status)
    {
        switch (status.ToLower())
        {
            case "completed": return "สำเร็จ";
            case "pending": return "รอชำระเงิน";
            case "cancelled": return "ยกเลิก";
            default: return status;
        }
    }
}
