using Microsoft.AspNetCore.Identity;

namespace Softscent.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
}
