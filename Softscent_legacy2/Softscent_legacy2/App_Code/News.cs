using System;
using System.ComponentModel.DataAnnotations;

namespace Softscent.Models
{
    /// <summary>
    /// Represents a news post or blog entry.
    /// </summary>
    public class News
    {
        public int Id { get; set; }

        /// <summary>
        /// Headline of the news article.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Full body text of the article.
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Main image for the news article.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Date and time when the article was posted.
        /// </summary>
        public DateTime PublishedDate { get; set; }
    }
}
