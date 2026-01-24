using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softscent.Data;
using Softscent.Models;

namespace Softscent.Controllers;

[Authorize]
public class SupportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public SupportController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var messages = await _context.SupportMessages
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.CreatedDate)
            .ToListAsync();
        return View(messages);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Subject,Message")] SupportMessage supportMessage)
    {
        if (ModelState.IsValid)
        {
            supportMessage.UserId = _userManager.GetUserId(User);
            supportMessage.CreatedDate = DateTime.Now;
            _context.Add(supportMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(supportMessage);
    }
}
