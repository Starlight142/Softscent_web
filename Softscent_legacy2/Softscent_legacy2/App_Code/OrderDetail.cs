using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents a specific line item within a customer order.
    /// </summary>
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        /// <summary>
        /// Reference to the Product object for easier property access in UI.
        /// </summary>
        public Product ProductInfo { get; set; }

        /// <summary>
        /// Number of units purchased for this item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price per unit at the time of purchase.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// JSON or comma-separated string containing specific customizations (for custom inhalers).
        /// </summary>
        public string CustomConfiguration { get; set; }
    }
}
