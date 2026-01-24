using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

public partial class Pages_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text; // In real app, hash this

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "Please enter email and password.";
            return;
        }

        try
        {
            // Check credentials against Users table
            string query = "SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @Password";
            var parameters = new Dictionary<string, object>
             {
                 { "@Email", email },
                 { "@Password", password }
             };

            DataTable dt = DataHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                // Store identity in session
                Session["User"] = email;
                Session["UserId"] = dt.Rows[0]["Id"].ToString();

                // Check if user has Admin role in database
                string roleQuery = @"SELECT r.Name FROM Roles r 
                                     JOIN UserRoles ur ON r.Id = ur.RoleId 
                                     WHERE ur.UserId = @UserId AND r.Name = 'Admin'";
                var roleParams = new Dictionary<string, object> { { "@UserId", dt.Rows[0]["Id"] } };
                DataTable dtRole = DataHelper.ExecuteQuery(roleQuery, roleParams);

                if (dtRole.Rows.Count > 0)
                {
                    Session["Role"] = "Admin";
                }
                else
                {
                    Session["Role"] = "User";
                }

                Response.Redirect("../index.aspx");
            }
            else
            {
                lblMessage.Text = "Invalid login attempt.";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error logging in: " + ex.Message;
        }
    }
}
