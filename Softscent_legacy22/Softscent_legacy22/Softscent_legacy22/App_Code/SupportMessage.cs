using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents a support request or contact message sent by a user.
    /// </summary>
    public class SupportMessage
    {
        public int Id { get; set; }

        /// <summary>
        /// ID of the user who sent the message. Can be null if the user is a guest.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The topic or subject of the support request.
        /// </summary>
        [Required]
        public string Subject { get; set; }

        /// <summary>
        /// The full details of the user's inquiry or issue.
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// Response provided by the support administrator.
        /// </summary>
        public string AdminReply { get; set; }

        /// <summary>
        /// Indicates if the issue has been addressed and closed.
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// Date and time when the message was sent.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
