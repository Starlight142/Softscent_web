using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        // public AppUser User { get; set; } // Simplify for ADO.NET
        
        public DateTime OrderDate { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; } // Pending, Completed, Cancelled
        
        public string ShippingAddress { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; } 
        public string PaymentStatus { get; set; }
        
        public List<OrderDetail> OrderDetails { get; set; }

        public Order() {
            OrderDetails = new List<OrderDetail>();
            Status = "Pending";
        }
    }
}
