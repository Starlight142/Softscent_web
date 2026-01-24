using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Softscent.Data;

namespace Softscent.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Simple dashboard stats
        ViewBag.TotalOrders = _context.Orders.Count();
        ViewBag.PendingOrders = _context.Orders.Count(o => o.Status == "Pending");
        ViewBag.TotalSales = _context.OrderDetails.Sum(od => od.Quantity * od.UnitPrice);
        
        // Recent Orders
        var recentOrders = _context.Orders.OrderByDescending(o => o.OrderDate).Take(5).ToList();
        
        return View(recentOrders);
    }
}
