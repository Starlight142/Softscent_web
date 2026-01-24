using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;
using Softscent.Models;

namespace Softscent.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SupportController : Controller
{
    private readonly ApplicationDbContext _context;

    public SupportController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var messages = await _context.SupportMessages
            .Include(m => m.User)
            .OrderByDescending(m => m.CreatedDate)
            .ToListAsync();
        return View(messages);
    }

    public async Task<IActionResult> Reply(int? id)
    {
        if (id == null) return NotFound();

        var message = await _context.SupportMessages
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (message == null) return NotFound();

        return View(message);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(int id, string reply)
    {
        var message = await _context.SupportMessages.FindAsync(id);
        if (message != null)
        {
            message.AdminReply = reply;
            message.IsResolved = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
