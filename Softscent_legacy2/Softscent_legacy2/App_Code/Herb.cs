using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents an ingredient (herb) used in customizable inhalers.
    /// </summary>
    public class Herb
    {
        public int Id { get; set; }

        /// <summary>
        /// Name of the herb.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Description of the herb characteristics.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price per unit for this herb.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// URL or path to the herb image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Stated health or wellness benefit of the herb.
        /// </summary>
        public string Benefit { get; set; }
    }
}
