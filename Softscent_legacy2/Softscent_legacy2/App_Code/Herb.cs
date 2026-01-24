using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    public class Herb
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Benefit { get; set; }
    }
}
