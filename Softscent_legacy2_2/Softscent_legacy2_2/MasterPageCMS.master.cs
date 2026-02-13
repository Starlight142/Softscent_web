using System;
using System.Web.UI;

public partial class MasterPageCMS : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
        {
            Response.Redirect("~/Pages/Login.aspx");
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/Pages/Login.aspx");
    }
}
