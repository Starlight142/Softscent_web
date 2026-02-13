using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Profile_Password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!IsPostBack)
        {
            LoadUserProfile();
        }
    }

    private void LoadUserProfile()
    {
        // For sidebar name display
        string userEmail = Session["User"].ToString();
        string query = "SELECT FullName FROM Users WHERE Email = @Email";
        var parameters = new Dictionary<string, object>
        {
            { "@Email", userEmail }
        };

        DataTable dt = DataHelper.ExecuteQuery(query, parameters);
        if (dt.Rows.Count > 0)
        {
            lblSidebarName.Text = dt.Rows[0]["FullName"] != DBNull.Value ? dt.Rows[0]["FullName"].ToString() : "ผู้ใช้งาน";
        }
        else
        {
            lblSidebarName.Text = "ผู้ใช้งาน";
        }
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        string currentPassword = txtCurrentPassword.Text;
        string newPassword = txtNewPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;

        if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
        {
            lblMessage.CssClass = "text-danger fw-bold";
            lblMessage.Text = "กรุณากรอกรหัสผ่านให้ครบถ้วน";
            return;
        }

        if (newPassword != confirmPassword)
        {
            lblMessage.CssClass = "text-danger fw-bold";
            lblMessage.Text = "รหัสผ่านใหม่ไม่ตรงกัน";
            return;
        }

        string userEmail = Session["User"].ToString();

        // Verify current password
        string query = "SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @Password";
        var parameters = new Dictionary<string, object>
        {
            { "@Email", userEmail },
            { "@Password", currentPassword }
        };

        DataTable dt = DataHelper.ExecuteQuery(query, parameters);
        if (dt.Rows.Count > 0)
        {
            // Update password
            string updateQuery = "UPDATE Users SET PasswordHash = @NewPassword WHERE Email = @Email";
            var updateParams = new Dictionary<string, object>
            {
                { "@NewPassword", newPassword },
                { "@Email", userEmail }
            };

            DataHelper.ExecuteNonQuery(updateQuery, updateParams);

            lblMessage.CssClass = "text-success fw-bold";
            lblMessage.Text = "เปลี่ยนรหัสผ่านสำเร็จ";
            
            // Clear fields
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
        }
        else
        {
            lblMessage.CssClass = "text-danger fw-bold";
            lblMessage.Text = "รหัสผ่านปัจจุบันไม่ถูกต้อง";
        }
    }
}
