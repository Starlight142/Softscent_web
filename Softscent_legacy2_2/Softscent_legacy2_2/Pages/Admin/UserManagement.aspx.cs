using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_UserManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Security Check
        if (Session["User"] == null || Session["Role"] == null || Session["Role"].ToString() != "Admin")
        {
            Response.Redirect("~/Pages/Login.aspx");
        }

        if (!IsPostBack)
        {
            LoadUsers();
        }
    }

    private void LoadUsers()
    {
        string query = @"
            SELECT u.Id, u.FullName as Name, u.Email, u.UserName as Username,
            COALESCE(r.Name, 'User') as RoleName
            FROM Users u
            LEFT JOIN UserRoles ur ON u.Id = ur.UserId
            LEFT JOIN Roles r ON ur.RoleId = r.Id
            ORDER BY u.Id DESC";

        DataTable dt = DataHelper.ExecuteQuery(query);
        gvUsers.DataSource = dt;
        gvUsers.DataBind();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ClearModal();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
    }

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditUser")
        {
            string id = e.CommandArgument.ToString();
            LoadUserDetails(id);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }
    }

    protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = gvUsers.DataKeys[e.RowIndex].Value.ToString();

        // Prevent self-deletion
        if (Session["UserId"] != null && id == Session["UserId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Cannot delete your own account.');", true);
            return;
        }

        string deleteRoles = "DELETE FROM UserRoles WHERE UserId = @Id";
        DataHelper.ExecuteNonQuery(deleteRoles, new Dictionary<string, object> { { "@Id", id } });

        string deleteUser = "DELETE FROM Users WHERE Id = @Id";
        DataHelper.ExecuteNonQuery(deleteUser, new Dictionary<string, object> { { "@Id", id } });

        LoadUsers();
    }

    private void LoadUserDetails(string id)
    {
        string query = @"SELECT * FROM Users WHERE Id = @Id";
        DataTable dt = DataHelper.ExecuteQuery(query, new Dictionary<string, object> { { "@Id", id } });

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            hfUserId.Value = row["Id"].ToString();
            txtName.Text = row["FullName"] != DBNull.Value ? row["FullName"].ToString() : "";
            txtEmail.Text = row["Email"].ToString();
            txtUsername.Text = row["UserName"] != DBNull.Value ? row["UserName"].ToString() : "";
            txtPassword.Text = ""; // Don't show hash

            // Check role
            string roleQuery = "SELECT RoleId FROM UserRoles WHERE UserId = @Id";
            DataTable dtRole = DataHelper.ExecuteQuery(roleQuery, new Dictionary<string, object> { { "@Id", id } });

            // Assuming RoleId 1 is Admin, or query Role Name. Let's lookup ID for 'Admin' role
            // Ideally we query role name.
            string checkAdmin = @"SELECT r.Name FROM UserRoles ur JOIN Roles r ON ur.RoleId = r.Id WHERE ur.UserId = @Id AND r.Name = 'Admin'";
            DataTable dtAdmin = DataHelper.ExecuteQuery(checkAdmin, new Dictionary<string, object> { { "@Id", id } });
            chkIsAdmin.Checked = dtAdmin.Rows.Count > 0;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text; // Plain text input

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@FullName", name },
                { "@Email", email },
                { "@UserName", (object)username ?? DBNull.Value }
            };

            string userId = "";
            if (string.IsNullOrEmpty(hfUserId.Value))
            {
                // Insert
                // Require password for new user
                if (string.IsNullOrEmpty(password))
                {
                    ShowError("Password is required for new users.");
                    return;
                }

                string hash = password; // Using plain text to match existing system based on Login.aspx.cs
                parameters.Add("@PasswordHash", hash);

                // For GUIDs, database usually generates NewId() default, or we let scope_identity work if it was identity int..
                // Since it is GUID, we won't get SCOPE_IDENTITY easily if it's auto-generated GUID.
                // Safest to fetch by email.
                string query = @"INSERT INTO Users (FullName, Email, UserName, PasswordHash) 
                               VALUES (@FullName, @Email, @UserName, @PasswordHash)";
                
                DataHelper.ExecuteNonQuery(query, parameters);
                
                DataTable dtId = DataHelper.ExecuteQuery("SELECT Id FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
                if (dtId.Rows.Count > 0) userId = dtId.Rows[0]["Id"].ToString();
            }
            else
            {
                // Update
                userId = hfUserId.Value;
                parameters.Add("@Id", userId);

                string query = "";
                if (!string.IsNullOrEmpty(password))
                {
                     parameters.Add("@PasswordHash", password);
                     query = "UPDATE Users SET FullName=@FullName, Email=@Email, UserName=@UserName, PasswordHash=@PasswordHash WHERE Id=@Id";
                }
                else
                {
                     query = "UPDATE Users SET FullName=@FullName, Email=@Email, UserName=@UserName WHERE Id=@Id";
                }

                DataHelper.ExecuteNonQuery(query, parameters);
            }

            // Update Role
            // First remove existing Admin role
            // We need to know Admin Role ID. Let's find it.
            DataTable dtRole = DataHelper.ExecuteQuery("SELECT Id FROM Roles WHERE Name = 'Admin'");
            int adminRoleId = 1; // Default fallback
            if (dtRole.Rows.Count > 0) adminRoleId = Convert.ToInt32(dtRole.Rows[0]["Id"]);

            // Remove admin role linkage
            DataHelper.ExecuteNonQuery("DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId", 
                new Dictionary<string, object> { { "@UserId", userId }, { "@RoleId", adminRoleId } });

            if (chkIsAdmin.Checked)
            {
                DataHelper.ExecuteNonQuery("INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)",
                     new Dictionary<string, object> { { "@UserId", userId }, { "@RoleId", adminRoleId } });
            }

            // Hide modal and reload
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hide", "hideModal();", true);
            LoadUsers();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ClearModal()
    {
        hfUserId.Value = "";
        txtName.Text = "";
        txtEmail.Text = "";
        txtUsername.Text = "";
        txtPassword.Text = "";
        chkIsAdmin.Checked = false;
        errorAlert.Attributes.Add("class", "alert alert-danger mt-3 d-none");
    }

    private void ShowError(string msg)
    {
        errorAlert.InnerText = msg;
        errorAlert.Attributes.Remove("class");
        errorAlert.Attributes.Add("class", "alert alert-danger mt-3");
    }
}
