using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;
using Softscent.Models;

namespace Softscent.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ReviewsController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Reviews/Create?productId=5
    public async Task<IActionResult> Create(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) return NotFound();

        ViewBag.ProductName = product.Name;
        
        var review = new Review
        {
            ProductId = productId
        };
        return View(review);
    }

    // POST: Reviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,Rating,Comment")] Review review)
    {
        var userId = _userManager.GetUserId(User);
        review.UserId = userId;
        review.CreatedDate = DateTime.Now;

        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            
            // Redirect to Order History or Product Page
            return RedirectToAction("Index", "Orders");
        }
        
        // Reload product name if error
        var product = await _context.Products.FindAsync(review.ProductId);
        ViewBag.ProductName = product?.Name;
        
        return View(review);
    }
}
