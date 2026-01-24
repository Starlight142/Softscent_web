using System;
using System.Collections.Generic;
using System.Web.UI;

public partial class Pages_Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string fullName = txtFullName.Text.Trim();
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;

        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "All fields are required.";
            return;
        }

        if (password != confirmPassword)
        {
            lblMessage.Text = "Passwords do not match.";
            return;
        }

        // Logic to save user to DB
        // For this legacy conversion, we'll implement a simple SQL Insert using DataHelper
        // Assuming a Users table exists or generic Identity table.
        // For simplicity/compatibility with "AppUser", we might use "Users" table.

        try
        {
            // Simple check if user exists
            var checkUser = DataHelper.ExecuteQuery("SELECT * FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
            if (checkUser.Rows.Count > 0)
            {
                lblMessage.Text = "Email already registered.";
                return;
            }

            // Insert User
            // Insert User with required Identity columns
            string insertSql = @"
                INSERT INTO Users (
                    Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
                    PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, 
                    TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FullName
                ) VALUES (
                    NEWID(), @Email, UPPER(@Email), @Email, UPPER(@Email), 1, 
                    @Password, NEWID(), NEWID(), 0, 
                    0, 1, 0, @FullName
                )";
            var parameters = new Dictionary<string, object>
            {
                { "@Email", email },
                { "@Password", password },
                { "@FullName", fullName }
            };

            DataHelper.ExecuteNonQuery(insertSql, parameters);

            // Auto login or redirect
            Session["User"] = email; // Simple Session Auth
            Response.Redirect("../index.aspx");
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error converting/registering: " + ex.Message;
        }
    }
}
