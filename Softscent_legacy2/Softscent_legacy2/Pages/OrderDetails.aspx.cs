using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

/// <summary>
/// Code-behind for the Order Details page.
/// Displays specifically selected order information and its line items.
/// </summary>
public partial class Pages_OrderDetails : System.Web.UI.Page
{
    public int OrderId;
    public Order CurrentOrder;

    /// <summary>
    /// Handles the Page Load event.
    /// Redirects to login if session is invalid, or back to history if no order ID is provided.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out OrderId))
        {
            Response.Redirect("Orders.aspx");
            return;
        }

        LoadOrderDetails();
    }

    /// <summary>
    /// Retrieves order header and item details from the database for the current order.
    /// </summary>
    private void LoadOrderDetails()
    {
        string userEmail = Session["User"].ToString();
        string userId = GetUserIdFromEmail(userEmail);

        // 1. Fetch Order Basic Info
        string orderQuery = "SELECT * FROM Orders WHERE Id = @OrderId AND UserId = @UserId";
        var orderParams = new Dictionary<string, object>
        {
            { "@OrderId", OrderId },
            { "@UserId", userId }
        };

        DataTable dtOrder = DataHelper.ExecuteQuery(orderQuery, orderParams);
        if (dtOrder.Rows.Count > 0)
        {
            DataRow row = dtOrder.Rows[0];
            CurrentOrder = new Order
            {
                Id = Convert.ToInt32(row["Id"]),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                Status = row["Status"].ToString(),
                ShippingAddress = row["ShippingAddress"].ToString(),
                ShippingMethod = row["ShippingMethod"] != DBNull.Value ? row["ShippingMethod"].ToString() : "Standard",
                PaymentMethod = row["PaymentMethod"] != DBNull.Value ? row["PaymentMethod"].ToString() : "Direct Payment",
                PaymentStatus = row["PaymentStatus"] != DBNull.Value ? row["PaymentStatus"].ToString() : "Success"
            };

            // 2. Fetch Order Items with Product Names joined
            string itemsQuery = @"SELECT od.*, p.Name as ProductName 
                                 FROM OrderDetails od 
                                 JOIN Products p ON od.ProductId = p.Id 
                                 WHERE od.OrderId = @OrderId";
            
            DataTable dtItems = DataHelper.ExecuteQuery(itemsQuery, new Dictionary<string, object> { { "@OrderId", OrderId } });
            foreach (DataRow itemRow in dtItems.Rows)
            {
                CurrentOrder.OrderDetails.Add(new OrderDetail
                {
                    Id = Convert.ToInt32(itemRow["Id"]),
                    ProductId = Convert.ToInt32(itemRow["ProductId"]),
                    Quantity = Convert.ToInt32(itemRow["Quantity"]),
                    UnitPrice = Convert.ToDecimal(itemRow["UnitPrice"]),
                    CustomConfiguration = itemRow["CustomConfiguration"].ToString(),
                    ProductInfo = new Product { Name = itemRow["ProductName"].ToString() }
                });
            }
        }
    }

    /// <summary>
    /// Helper to translate user email to User ID string.
    /// </summary>
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
    /// Helper to provide a Bootstrap color class for a status.
    /// </summary>
    public string GetStatusColor(string status)
    {
        switch (status.ToLower())
        {
            case "completed": 
            case "delivered":
                return "success";
            case "pending": return "warning";
            case "cancelled": return "danger";
            case "shipped": 
            case "out_for_delivery":
                return "info";
            case "paid": return "primary";
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
            case "paid": return "ชำระเงินแล้ว";
            case "shipped": return "อยู่ระหว่างจัดส่ง";
            case "out_for_delivery": return "กำลังนำจ่าย";
            case "delivered": return "จัดส่งสำเร็จ";
            case "cancelled": return "ยกเลิก";
            default: return status;
        }
    }
}
