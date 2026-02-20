using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents a product available for sale in the shop.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier for the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Detailed description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unit price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Path or URL to the product image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Indicates if this product represents a customizable item (e.g., custom inhaler).
        /// </summary>
        public bool IsCustomizable { get; set; }

        /// <summary>
        /// Quantity of product in stock.
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Name of the product in Thai.
        /// </summary>
        public string NameThai { get; set; }

        /// <summary>
        /// Description of the product in Thai.
        /// </summary>
        public string DescriptionThai { get; set; }
    }
}
