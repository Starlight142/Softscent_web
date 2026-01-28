using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents a user account in the system, mapped to the ASP.NET Identity Users table.
    /// </summary>
    public class AppUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// The hashed representation of the user's password.
        /// </summary>
        public string PasswordHash { get; set; }

        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        /// <summary>
        /// Indicates if the user has verified their email address.
        /// </summary>
        public bool EmailConfirmed { get; set; }
    }
}
