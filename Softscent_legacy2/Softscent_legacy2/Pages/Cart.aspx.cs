using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Softscent.Models;

public partial class Pages_Cart : System.Web.UI.Page
{
    public Order CurrentOrder;

    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentOrder = GetCart();

        string action = Request.Params["action"];
        if (!IsPostBack && !string.IsNullOrEmpty(action))
        {
            if (action == "add")
            {
                int productId = Convert.ToInt32(Request.Params["productId"]);
                AddToCart(productId);
                Response.Redirect("Cart.aspx");
            }
            else if (action == "addCustom")
            {
                // Form post usually handled below, but if check param from query string (unlikely for post)
                // For POST, we check Request.Form
            }
            else if (action == "checkout")
            {
                // This is now handled by the dedicated Checkout.aspx page
                Response.Redirect("Checkout.aspx");
            }
        }

        if (Request.HttpMethod == "POST")
        {
            string formAction = Request.Form["action"];
            if (formAction == "addCustom")
            {
                string customConfig = Request.Form["customConfig"];
                AddCustomToCart(customConfig);
                Response.Redirect("Cart.aspx");
            }
        }
    }

    private Order GetCart()
    {
        if (Session["Cart"] == null)
        {
            Session["Cart"] = new Order();
        }
        Order cart = (Order)Session["Cart"];

        // Scrub cart of invalid ProductIds (e.g. old 999 fake ID)
        if (cart.OrderDetails.Count > 0)
        {
            var validIds = new List<int>();
            DataTable dt = DataHelper.ExecuteQuery("SELECT Id FROM Products");
            foreach (DataRow row in dt.Rows) validIds.Add(Convert.ToInt32(row["Id"]));

            cart.OrderDetails.RemoveAll(d => !validIds.Contains(d.ProductId));
        }

        return cart;
    }

    private void AddToCart(int productId)
    {
        // Fetch Product
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Products WHERE Id = @Id", new Dictionary<string, object> { { "@Id", productId } });
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            Product p = new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Price = Convert.ToDecimal(row["Price"])
            };

            // Check if exists
            var existing = CurrentOrder.OrderDetails.FirstOrDefault(d => d.ProductId == productId && string.IsNullOrEmpty(d.CustomConfiguration));
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                CurrentOrder.OrderDetails.Add(new OrderDetail
                {
                    ProductId = productId,
                    ProductInfo = p, // Keep reference for display
                    Quantity = 1,
                    UnitPrice = p.Price
                });
            }
        }
    }

    private void AddCustomToCart(string config)
    {
        // Fetch the real Custom Product from DB
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Products WHERE Name = 'Custom Inhaler Blend'");
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            Product p = new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                IsCustomizable = true
            };

            CurrentOrder.OrderDetails.Add(new OrderDetail
            {
                ProductId = p.Id,
                ProductInfo = p,
                Quantity = 1,
                UnitPrice = p.Price,
                CustomConfiguration = config
            });
        }
    }

    private void Checkout()
    {
        // 1. Get User
        string userId = "Guest";
        if (Session["User"] != null)
        {
            // Lookup ID from Email
            string email = Session["User"].ToString();
            DataTable dtUser = DataHelper.ExecuteQuery("SELECT Id FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
            if (dtUser.Rows.Count > 0)
            {
                userId = dtUser.Rows[0]["Id"].ToString();
            }
        }
        else
        {
            Response.Redirect("Login.aspx?returnUrl=Cart.aspx");
            return;
        }

        // 2. Insert Order (Included missing required columns: ShippingMethod, PaymentMethod, PaymentStatus)
        string insertOrder = @"INSERT INTO Orders (UserId, OrderDate, TotalAmount, Status, ShippingAddress, ShippingMethod, PaymentMethod, PaymentStatus) 
                               OUTPUT INSERTED.Id 
                               VALUES (@UserId, @OrderDate, @TotalAmount, @Status, @Address, @ShippingMethod, @PaymentMethod, @PaymentStatus)";

        decimal total = GetTotal();
        var orderParams = new Dictionary<string, object>
        {
            { "@UserId", userId },
            { "@OrderDate", DateTime.Now },
            { "@TotalAmount", total },
            { "@Status", "Pending" },
            { "@Address", "Store Pickup" },
            { "@ShippingMethod", "Standard" },
            { "@PaymentMethod", "Cash on Delivery" },
            { "@PaymentStatus", "Pending" }
        };

        object orderIdObj = DataHelper.ExecuteScalar(insertOrder, orderParams);
        int orderId = Convert.ToInt32(orderIdObj);

        // 3. Insert Order Details
        foreach (var item in CurrentOrder.OrderDetails)
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

        // 4. Clear Cart
        Session["Cart"] = null;
        Response.Write("<script>alert('Order Placed Successfully!'); window.location='../Pages/Orders.aspx';</script>");
        Response.End();
    }

    public decimal GetTotal()
    {
        if (CurrentOrder == null) return 0;
        decimal total = 0;
        foreach (var item in CurrentOrder.OrderDetails)
        {
            total += item.UnitPrice * item.Quantity;
        }
        return total;
    }
}
