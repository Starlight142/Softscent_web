using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Softscent.Models;

/// <summary>
/// Code-behind logic for the Shopping Cart page.
/// Handles cart operations like adding, updating, and removing items.
/// </summary>
public partial class Pages_Cart : System.Web.UI.Page
{
    /// <summary>
    /// Holds the current session's order (cart) object.
    /// </summary>
    public Order CurrentOrder;

    /// <summary>
    /// Handles the Page_Load event. 
    /// Retrieves the current cart and processes action parameters (add/remove/update) from the URL or Form.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentOrder = GetCart();

        string action = Request.Params["action"];
        if (!IsPostBack && !string.IsNullOrEmpty(action))
        {
            int productId = 0;
            if (Request.Params["productId"] != null) int.TryParse(Request.Params["productId"], out productId);

            if (action == "add")
            {
                AddToCart(productId);
                Response.Redirect("Cart.aspx");
            }
            else if (action == "remove")
            {
                RemoveFromCart(productId);
                Response.Redirect("Cart.aspx");
            }
            else if (action == "update")
            {
                int qty = 1;
                if (Request.Params["qty"] != null) int.TryParse(Request.Params["qty"], out qty);
                UpdateCart(productId, qty);
                Response.Redirect("Cart.aspx");
            }
            else if (action == "addCustom")
            {
                // Form post usually handled below, but if check param from query string (unlikely for post)
                // For POST, we check Request.Form
            }
            else if (action == "checkout")
            {
                // checkout logic is handled by Checkout.aspx
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

    /// <summary>
    /// Initializes or retrieves the shopping cart from the Session.
    /// Also validates product IDs against the database to ensure integrity.
    /// </summary>
    /// <returns>The Order object representing the cart.</returns>
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

    /// <summary>
    /// Adds a product to the cart by ID. If item exists, increments quantity.
    /// </summary>
    private void AddToCart(int productId)
    {
        // Fetch Product
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Products WHERE Id = @Id", new Dictionary<string, object> { { "@Id", productId } });
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            int stock = row["StockQuantity"] != DBNull.Value ? Convert.ToInt32(row["StockQuantity"]) : 0;

            Product p = new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                StockQuantity = stock
            };

            // Check if exists
            var existing = CurrentOrder.OrderDetails.FirstOrDefault(d => d.ProductId == productId && string.IsNullOrEmpty(d.CustomConfiguration));
            if (existing != null)
            {
                if (existing.Quantity < stock)
                {
                    existing.Quantity++;
                }
            }
            else
            {
                if (stock > 0)
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
    }

    /// <summary>
    /// Adds a custom-configured product (e.g., blend) to the cart.
    /// </summary>
    private void AddCustomToCart(string config)
    {
        // Fetch the real Custom Product from DB
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Products WHERE Name = 'Custom Inhaler Blend'");
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            int stock = row["StockQuantity"] != DBNull.Value ? Convert.ToInt32(row["StockQuantity"]) : 0;
            if (stock <= 0) return; // Out of stock

            Product p = new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                IsCustomizable = true,
                StockQuantity = stock
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

    /// <summary>
    /// Removes an item from the cart by Product ID.
    /// </summary>
    private void RemoveFromCart(int productId)
    {
        CurrentOrder.OrderDetails.RemoveAll(d => d.ProductId == productId);
    }

    /// <summary>
    /// Updates the quantity of a cart item.
    /// </summary>
    /// <summary>
    /// Updates the quantity of a cart item with stock validation.
    /// </summary>
    private void UpdateCart(int productId, int qty)
    {
        var item = CurrentOrder.OrderDetails.FirstOrDefault(d => d.ProductId == productId);
        if (item != null)
        {
            if (qty > 0)
            {
                // Check stock
                object stockObj = DataHelper.ExecuteScalar("SELECT StockQuantity FROM Products WHERE Id = @Id", new Dictionary<string, object> { { "@Id", productId } });
                int stock = stockObj != null && stockObj != DBNull.Value ? Convert.ToInt32(stockObj) : 0;

                if (qty <= stock)
                {
                    item.Quantity = qty;
                }
                else
                {
                    item.Quantity = stock; // Max out at available stock
                }
            }
            else
            {
                CurrentOrder.OrderDetails.Remove(item);
            }
        }
    }

    /// <summary>
    /// Calculates the total total price of items in the cart.
    /// </summary>
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

    // Checkout method removed/commented as separate Checkout.aspx handles it now.
}
