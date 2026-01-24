using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;

namespace Softscent.Controllers;

public class NewsController : Controller
{
    private readonly ApplicationDbContext _context;

    public NewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.News.OrderByDescending(n => n.PublishedDate).ToListAsync());
    }
}
