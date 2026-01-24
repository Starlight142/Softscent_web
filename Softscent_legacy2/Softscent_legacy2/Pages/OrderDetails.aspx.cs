using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

public partial class Pages_OrderDetails : System.Web.UI.Page
{
    public int OrderId;
    public Order CurrentOrder;

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

            // 2. Fetch Order Items
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

    private string GetUserIdFromEmail(string email)
    {
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
