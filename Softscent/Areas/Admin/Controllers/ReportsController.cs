using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;
using Softscent.Models;

namespace Softscent.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string period = "daily")
    {
        var orders = _context.Orders
            .Include(o => o.OrderDetails)
            .Where(o => o.Status != "Cancelled");

        DateTime startDate;
        DateTime endDate = DateTime.Now;

        switch (period.ToLower())
        {
            case "monthly":
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                ViewBag.Period = "Monthly";
                break;
            case "yearly":
                startDate = new DateTime(DateTime.Now.Year, 1, 1);
                ViewBag.Period = "Yearly";
                break;
            case "daily":
            default:
                startDate = DateTime.Today;
                ViewBag.Period = "Daily";
                break;
        }

        var filteredOrders = await orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate.AddDays(1))
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        ViewBag.TotalSales = filteredOrders.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice));
        ViewBag.TotalOrders = filteredOrders.Count;
        ViewBag.CurrentPeriod = period;

        return View(filteredOrders);
    }
}
