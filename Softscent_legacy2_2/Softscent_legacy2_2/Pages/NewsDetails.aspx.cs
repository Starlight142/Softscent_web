using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

/// <summary>
/// Code-behind for the News Details page.
/// Fetches and displays a specific news article based on the ID passed in the query string.
/// </summary>
public partial class Pages_NewsDetails : System.Web.UI.Page
{
    /// <summary>
    /// The specific news article being viewed.
    /// </summary>
    public News CurrentNews;

    /// <summary>
    /// Handles the Page Load event.
    /// Verifies the order ID parameter and attempts to load the article details.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string idParams = Request.QueryString["id"];
            int id;
            if (!string.IsNullOrEmpty(idParams) && int.TryParse(idParams, out id))
            {
                LoadNewsDetail(id);
            }
        }
    }

    /// <summary>
    /// Loads the news article data from the database using the provided ID.
    /// </summary>
    /// <param name="id">The unique identifier of the news article.</param>
    private void LoadNewsDetail(int id)
    {
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM News WHERE Id = @Id", new Dictionary<string, object> { { "@Id", id } });
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            CurrentNews = new News
            {
                Id = Convert.ToInt32(row["Id"]),
                Title = row["Title"].ToString(),
                Content = row["Content"].ToString(),
                ImageUrl = row["ImageUrl"] != DBNull.Value ? row["ImageUrl"].ToString() : "",
                PublishedDate = Convert.ToDateTime(row["PublishedDate"])
            };
        }
    }

    /// <summary>
    /// Resolves article image URLs, falling back to a placeholder if none is provided.
    /// </summary>
    public string GetImageUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return "https://via.placeholder.com/800x600?text=Softscent";
        if (url.StartsWith("http") || url.StartsWith("/")) return url;
        return ResolveUrl("~/Images/" + url);
    }
}
