using System.ComponentModel.DataAnnotations;

namespace Softscent.Models;

public class SupportMessage
{
    public int Id { get; set; }

    public string? UserId { get; set; }
    public AppUser? User { get; set; }

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;
    
    public string? AdminReply { get; set; }

    public bool IsResolved { get; set; } = false;

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
