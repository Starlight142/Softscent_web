using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Softscent.Models;

/// <summary>
/// Code-behind for the Reviews page.
/// Displays user feedback and allows logged-in users to submit new reviews.
/// </summary>
public partial class Pages_Reviews : System.Web.UI.Page
{
    /// <summary>
    /// View model extending the base Review to include UI-specific fields like display name.
    /// </summary>
    public class ReviewViewModel : Review
    {
        public string ReviewerName { get; set; }
    }

    /// <summary>
    /// List of reviews to display on the page.
    /// </summary>
    public List<ReviewViewModel> ReviewList = new List<ReviewViewModel>();

    /// <summary>
    /// Handles the Page Load event.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadReviews();
        }
    }

    /// <summary>
    /// Fetches reviews from the database. 
    /// Includes a LEFT JOIN to the Users table to retrieve reviewer names.
    /// Handles missing columns gracefully as a fallback.
    /// </summary>
    private void LoadReviews()
    {
        string query = @"
            SELECT r.*, u.FullName 
            FROM Reviews r 
            LEFT JOIN Users u ON r.UserId = u.Id 
            ORDER BY r.CreatedDate DESC";

        DataTable dt;
        try
        {
            dt = DataHelper.ExecuteQuery(query);
        }
        catch
        {
            // Fallback if JOIN fails (database schema mismatch)
            query = "SELECT * FROM Reviews ORDER BY CreatedDate DESC";
            dt = DataHelper.ExecuteQuery(query);
        }

        ReviewList.Clear();
        foreach (DataRow row in dt.Rows)
        {
            ReviewViewModel review = new ReviewViewModel
            {
                Id = Convert.ToInt32(row["Id"]),
                Rating = Convert.ToInt32(row["Rating"]),
                Comment = row["Comment"].ToString(),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UserId = row["UserId"].ToString()
            };

            // Set the reviewer display name
            if (dt.Columns.Contains("FullName") && row["FullName"] != DBNull.Value)
            {
                review.ReviewerName = row["FullName"].ToString();
            }
            else
            {
                // Masking the User ID if no explicit name is found
                review.ReviewerName = "ลูกค้า (" + review.UserId.Substring(0, Math.Min(3, review.UserId.Length)) + "...)";
            }

            ReviewList.Add(review);
        }

        // Inject dummy data for display if list is empty
        if (ReviewList.Count == 0)
        {
            ReviewList.Add(new ReviewViewModel
            {
                Id = 0,
                Rating = 5,
                Comment = "สินค้าดีมากค่ะ หอมผ่อนคลายจริงๆ (Example Review)",
                CreatedDate = DateTime.Now,
                UserId = "0",
                ReviewerName = "Admin (Demo)"
            });
        }
    }

    /// <summary>
    /// Handles the "Submit Review" click event.
    /// Validates user session and inserts the review into the database.
    /// </summary>
    protected void btnSubmitReview_Click(object sender, EventArgs e)
    {
        if (Session["User"] != null)
        {
            string userEmail = Session["User"].ToString();
            string userId = GetUserIdFromEmail(userEmail);

            int rating = int.Parse(ddlRating.SelectedValue);
            string comment = txtReviewComment.Text.Trim();

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(comment))
            {
                string query = "INSERT INTO Reviews (UserId, Rating, Comment, CreatedDate) VALUES (@UserId, @Rating, @Comment, GETDATE())";
                var parameters = new Dictionary<string, object>
                {
                    { "@UserId", userId },
                    { "@Rating", rating },
                    { "@Comment", comment }
                };

                DataHelper.ExecuteNonQuery(query, parameters);

                // Reset form inputs and reload list
                txtReviewComment.Text = "";
                ddlRating.SelectedIndex = 4; // Default to 5 stars
                LoadReviews();
            }
        }
        else
        {
            Response.Redirect("/Pages/Login.aspx");
        }
    }

    /// <summary>
    /// Helper to resolve User ID string from an email.
    /// </summary>
    private string GetUserIdFromEmail(string email)
    {
        string query = "SELECT Id FROM Users WHERE Email = @Email";
        object result = DataHelper.ExecuteScalar(query, new Dictionary<string, object> { { "@Email", email } });
        return result != null ? result.ToString() : null;
    }
}
