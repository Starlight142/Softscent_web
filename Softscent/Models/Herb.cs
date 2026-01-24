using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softscent.Models;

public class Herb
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? NameThai { get; set; }

    public string? Description { get; set; }

    public string? DescriptionThai { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    // For mixing logic, maybe properties?
    // e.g., benefit type: "Relaxing", "Refreshing"
    public string? Benefit { get; set; }

    public string? BenefitThai { get; set; }
}
