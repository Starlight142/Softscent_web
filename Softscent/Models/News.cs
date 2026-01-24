using System.ComponentModel.DataAnnotations;

namespace Softscent.Models;

public class News
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public DateTime PublishedDate { get; set; } = DateTime.Now;
}
