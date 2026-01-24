using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    public class SupportMessage
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        
        [Required]
        public string Subject { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public string AdminReply { get; set; }
        
        public bool IsResolved { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
