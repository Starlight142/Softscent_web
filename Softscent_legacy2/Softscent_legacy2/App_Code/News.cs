using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    public class News
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public string ImageUrl { get; set; }
        
        public DateTime PublishedDate { get; set; }
    }
}
