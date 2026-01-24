using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;
using Softscent.Models;
using System.Security.Claims;

namespace Softscent.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Challenge();

        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Pending");

        if (order == null)
        {
            order = new Order(); // Empty cart
        }

        return View(order);
    }

    public async Task<IActionResult> AddToCart(int productId, string? customConfig)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Challenge();

        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Pending");

        if (order == null)
        {
            order = new Order
            {
                UserId = userId,
                Status = "Pending",
                OrderDate = DateTime.Now
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        if (productId == -1 && !string.IsNullOrEmpty(customConfig))
        {
             // Custom Item
             // We need a dummy product or handle null product. 
             // My OrderDetail allows null Product? No, ProductId is int.
             // I'll assume we have a "Custom Blend" product with ID 1 seeded. 
             // If not, I'll create it on the fly or find it.
             
             var customProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Custom Inhaler Blend");
             if (customProduct == null) {
                 customProduct = new Product { Name = "Custom Inhaler Blend", Price = 59.00m, IsCustomizable = true };
                 _context.Products.Add(customProduct);
                 await _context.SaveChangesAsync();
             }
             
             var detail = new OrderDetail
             {
                 OrderId = order.Id,
                 ProductId = customProduct.Id,
                 Quantity = 1,
                 UnitPrice = customProduct.Price,
                 CustomConfiguration = customConfig
             };
             _context.OrderDetails.Add(detail);
        }
        else
        {
            // Standard Product
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                 var detail = order.OrderDetails.FirstOrDefault(d => d.ProductId == productId && d.CustomConfiguration == null);
                 if (detail != null)
                {
                    detail.Quantity++;
                }
                else
                {
                    detail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = 1,
                        UnitPrice = product.Price
                    };
                    _context.OrderDetails.Add(detail);
                }
            }
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Challenge();

        var user = await _userManager.FindByIdAsync(userId);
        
        var order = await _context.Orders
             .Include(o => o.OrderDetails)
             .ThenInclude(od => od.Product)
             .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Pending");
             
        if (order == null || !order.OrderDetails.Any())
        {
            return RedirectToAction(nameof(Index));
        }
        
        // Pre-fill address from user profile
        order.ShippingAddress = user?.Address ?? "";
        return View(order);
    }
    
    [HttpPost]
    public async Task<IActionResult> Checkout(Order orderDto) // Binding to Order model heavily simplified
    {
        // In real app, use DTO
        var userId = _userManager.GetUserId(User);
        var order = await _context.Orders
             .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Pending");
             
        if (order != null)
        {
            order.Status = "Completed";
            order.ShippingAddress = orderDto.ShippingAddress;
            order.PaymentMethod = orderDto.PaymentMethod;
            order.ShippingMethod = orderDto.ShippingMethod;
            
            // Set Payment Status based on method
            if (order.PaymentMethod == "Cash on Delivery")
            {
                order.PaymentStatus = "Pending";
            }
            else
            {
                // Simulate immediate payment for others
                order.PaymentStatus = "Paid";
            }

            order.OrderDate = DateTime.Now;
        }
        
        await _context.SaveChangesAsync();
        return View("OrderConfirmed");
    }
}
