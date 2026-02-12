using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents a product review from a customer.
    /// </summary>
    public class Review
    {
        public int Id { get; set; }

        /// <summary>
        /// ID of the product being reviewed.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ID of the user who wrote the review.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Rating score (usually 1 to 5 stars).
        /// </summary>
        [Range(1, 5)]
        public int Rating { get; set; }

        /// <summary>
        /// Textual feedback left by the user.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Date and time when the review was submitted.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
