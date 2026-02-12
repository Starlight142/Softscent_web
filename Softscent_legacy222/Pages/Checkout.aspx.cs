using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Softscent.Models;

/// <summary>
/// Code-behind for the Checkout page.
/// Handles the final order placement process.
/// </summary>
public partial class Pages_Checkout : System.Web.UI.Page
{
    public List<OrderDetail> OrderDetails = new List<OrderDetail>();
    public decimal TotalAmount = 0;
    public int CartCount = 0;
    public string UserAddress = "";

    /// <summary>
    /// Handles Page Load.
    /// Validates user session, cart existence, and pre-fills user address if available.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("Login.aspx?returnUrl=Checkout.aspx");
            return;
        }

        Order cart = Session["Cart"] as Order;
        if (cart == null || cart.OrderDetails.Count == 0)
        {
            Response.Redirect("Cart.aspx");
            return;
        }

        OrderDetails = cart.OrderDetails;
        CartCount = OrderDetails.Count;
        TotalAmount = OrderDetails.Sum(x => x.UnitPrice * x.Quantity);

        if (!IsPostBack)
        {
            // Load User Address
            string email = Session["User"].ToString();
            DataTable dtUser = DataHelper.ExecuteQuery("SELECT Address FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
            if (dtUser.Rows.Count > 0)
            {
                UserAddress = dtUser.Rows[0]["Address"].ToString();
            }
        }
    }

    /// <summary>
    /// Handles the "Complete Order" button click.
    /// Creates the order header and details in the database, then clears the cart.
    /// </summary>
    protected void btnCompleteOrder_Click(object sender, EventArgs e)
    {
        if (Session["User"] == null) return;

        string email = Session["User"].ToString();
        DataTable dtUser = DataHelper.ExecuteQuery("SELECT Id FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
        if (dtUser.Rows.Count == 0) return;

        string userId = dtUser.Rows[0]["Id"].ToString();
        Order cart = Session["Cart"] as Order;

        // Get form values
        string address = Request.Form["shippingAddress"];
        string shipMethod = Request.Form["shippingMethod"];
        string payMethod = Request.Form["paymentMethod"];

        // 1. Insert Order
        string insertOrder = @"INSERT INTO Orders (UserId, OrderDate, TotalAmount, Status, ShippingAddress, ShippingMethod, PaymentMethod, PaymentStatus) 
                               OUTPUT INSERTED.Id 
                               VALUES (@UserId, @OrderDate, @TotalAmount, @Status, @Address, @ShippingMethod, @PaymentMethod, @PaymentStatus)";

        var orderParams = new Dictionary<string, object>
        {
            { "@UserId", userId },
            { "@OrderDate", DateTime.Now },
            { "@TotalAmount", TotalAmount },
            { "@Status", "Completed" },
            { "@Address", address },
            { "@ShippingMethod", shipMethod },
            { "@PaymentMethod", payMethod },
            { "@PaymentStatus", payMethod == "Cash on Delivery" ? "Pending" : "Paid" }
        };

        object orderIdObj = DataHelper.ExecuteScalar(insertOrder, orderParams);
        int orderId = Convert.ToInt32(orderIdObj);

        // 2. Insert Order Details
        foreach (var item in cart.OrderDetails)
        {
            string insertDetail = "INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice, CustomConfiguration) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice, @CustomConfig)";
            var detailParams = new Dictionary<string, object>
             {
                 { "@OrderId", orderId },
                 { "@ProductId", item.ProductId },
                 { "@Quantity", item.Quantity },
                 { "@UnitPrice", item.UnitPrice },
                 { "@CustomConfig", item.CustomConfiguration ?? (object)DBNull.Value }
             };
            DataHelper.ExecuteNonQuery(insertDetail, detailParams);
        }

        // 3. Clear Cart
        Session["Cart"] = null;
        Response.Write("<script>alert('Order Placed Successfully!'); window.location='Orders.aspx';</script>");
        Response.End();
    }
}
