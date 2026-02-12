using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Softscent.Models;

/// <summary>
/// Code-behind for the News Listing page.
/// Displays news articles and allows admins to post new ones.
/// </summary>
public partial class Pages_News : System.Web.UI.Page
{
    /// <summary>
    /// List of news objects to display on the page.
    /// </summary>
    public List<News> NewsList = new List<News>();

    /// <summary>
    /// Handles Page Load. Fetches news articles on first load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadNews();
        }
    }

    /// <summary>
    /// Fetches all news from database ordered by publish date.
    /// </summary>
    private void LoadNews()
    {
        string query = "SELECT * FROM News ORDER BY PublishedDate DESC";
        DataTable dt = DataHelper.ExecuteQuery(query);

        NewsList.Clear();
        foreach (DataRow row in dt.Rows)
        {
            NewsList.Add(new News
            {
                Id = Convert.ToInt32(row["Id"]),
                Title = row["Title"].ToString(),
                Content = row["Content"].ToString(),
                ImageUrl = row["ImageUrl"] != DBNull.Value ? row["ImageUrl"].ToString() : "",
                PublishedDate = Convert.ToDateTime(row["PublishedDate"])
            });
        }
    }

    /// <summary>
    /// Resolves partial or relative image URLs to browser-friendly absolute paths.
    /// </summary>
    public string GetImageUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return "https://via.placeholder.com/800x600?text=Softscent";
        }

        // If it's an external URL, return as is
        if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        // If it starts with ~, it is already a root-relative path formatted for ASP.NET
        if (url.StartsWith("~"))
        {
            return ResolveUrl(url);
        }

        // If it is just a filename (e.g. "image.png"), assume it is in the Images folder
        if (!url.Contains("/") && !url.Contains("\\"))
        {
            return ResolveUrl("~/Images/" + url);
        }

        // If it is a relative path like "Images/products/foo.png", we need to anchor it to root with ~/
        // otherwise it will be relative to the current page (Pages folder) which is wrong.
        if (!url.StartsWith("/"))
        {
            return ResolveUrl("~/" + url);
        }

        // If it starts with /, return as is or resolve it just in case
        return ResolveUrl(url);
    }

    /// <summary>
    /// Handles posting a new news article (Admin only).
    /// </summary>
    protected void btnAddNews_Click(object sender, EventArgs e)
    {
        if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
        {
            string title = txtNewTitle.Text.Trim();
            string content = txtNewContent.Text.Trim();
            string imageUrl = txtNewImage.Text.Trim();

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                string query = "INSERT INTO News (Title, Content, ImageUrl, PublishedDate) VALUES (@Title, @Content, @ImageUrl, GETDATE())";
                var parameters = new Dictionary<string, object>
                {
                    { "@Title", title },
                    { "@Content", content },
                    { "@ImageUrl", imageUrl }
                };

                DataHelper.ExecuteNonQuery(query, parameters);

                // Clear inputs and reload
                txtNewTitle.Text = "";
                txtNewContent.Text = "";
                txtNewImage.Text = "";

                LoadNews();
            }
        }
    }
}
