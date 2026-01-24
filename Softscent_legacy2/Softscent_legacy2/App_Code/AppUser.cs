using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    public class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
