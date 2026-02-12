using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents a customer order header.
    /// </summary>
    public class Order
    {
        public int Id { get; set; }

        /// <summary>
        /// ID of the user who placed the order. Linked to the Users table.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The date and time the order was created.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Total grand total for the order including all items.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Current state of the order: Pending, Completed, or Cancelled.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Address where the goods will be shipped (or store pickup location).
        /// </summary>
        public string ShippingAddress { get; set; }

        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        /// <summary>
        /// List of individual lines (items) included in this order.
        /// </summary>
        public List<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            Status = "Pending";
        }
    }
}
