using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;
using Softscent.Models;

namespace Softscent.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class NewsController : Controller
{
    private readonly ApplicationDbContext _context;

    public NewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/News
    public async Task<IActionResult> Index()
    {
        return View(await _context.News.OrderByDescending(n => n.PublishedDate).ToListAsync());
    }

    // GET: Admin/News/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/News/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Content,ImageUrl")] News news)
    {
        if (ModelState.IsValid)
        {
            news.PublishedDate = DateTime.Now;
            _context.Add(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(news);
    }
    
    // GET: Admin/News/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var news = await _context.News
            .FirstOrDefaultAsync(m => m.Id == id);
        if (news == null) return NotFound();

        return View(news);
    }

    // POST: Admin/News/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var news = await _context.News.FindAsync(id);
        if (news != null)
        {
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
