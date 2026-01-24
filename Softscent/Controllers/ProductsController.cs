using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;
using Softscent.Models;

namespace Softscent.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }

    // GET: Products/Custom
    public async Task<IActionResult> Custom()
    {
        var herbs = await _context.Herbs.ToListAsync();
        return View(herbs);
    }

    // POST: Products/AddToCartCustom
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCartCustom(string selectedHerbs)
    {
        // Simple logic: Create a dummy product or just add to order directly
        // Ideally we should redirect to CartController.AddToCart
        // But let's handle logic here for simplicity then redirect
        
        // This part requires Auth logic to get current user order
        // I will implement a CartService or logic in CartController.
        // For now, let's redirect to Cart with params? No, better to POST to Cart.
        
        return RedirectToAction("AddToCart", "Cart", new { productId = -1, customConfig = selectedHerbs });
    }
}
