using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softscent.Models;

public class Order
{
    public int Id { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled
    
    // Shipping Details (snapshot of user address or custom)
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingMethod { get; set; } = "Standard"; // Standard, Express
    public string PaymentMethod { get; set; } = "Credit Card"; 
    public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid

    public List<OrderDetail> OrderDetails { get; set; } = new();
}
