using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Auto-fix session if user is logged in but role is missing
        if (Session["User"] != null && Session["Role"] == null)
        {
            string email = Session["User"].ToString();
            DataTable dt = DataHelper.ExecuteQuery("SELECT Id FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
            if (dt.Rows.Count > 0)
            {
                string userId = dt.Rows[0]["Id"].ToString();
                DataTable dtRole = DataHelper.ExecuteQuery(@"SELECT r.Name FROM Roles r 
                                                           JOIN UserRoles ur ON r.Id = ur.RoleId 
                                                           WHERE ur.UserId = @UserId AND r.Name = 'Admin'",
                                                           new Dictionary<string, object> { { "@UserId", userId } });
                Session["Role"] = (dtRole.Rows.Count > 0) ? "Admin" : "User";
            }
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("~/index.aspx");
    }
}
