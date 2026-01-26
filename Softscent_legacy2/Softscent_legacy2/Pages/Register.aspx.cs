using System;
using System.Collections.Generic;
using System.Web.UI;

/// <summary>
/// Code-behind for the Registration page.
/// Handles new user account creation.
/// </summary>
public partial class Pages_Register : System.Web.UI.Page
{
    /// <summary>
    /// Handles Page Load event.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Handles the Register button click event.
    /// Validates redundant inputs, checks for existing users, and creates a new user record.
    /// </summary>
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string fullName = txtFullName.Text.Trim();
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;

        // Basic validation
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

        try
        {
            // Check if the email is already registered to avoid duplicates
            var checkUser = DataHelper.ExecuteQuery("SELECT * FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
            if (checkUser.Rows.Count > 0)
            {
                lblMessage.Text = "Email already registered.";
                return;
            }

            // Insert new user into the database with standard Identity columns
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
                { "@Password", password }, // Note: In a production app, password should be hashed
                { "@FullName", fullName }
            };

            DataHelper.ExecuteNonQuery(insertSql, parameters);

            // Successfully registered, set session and redirect to homepage
            Session["User"] = email;
            Response.Redirect("../index.aspx");
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error converting/registering: " + ex.Message;
        }
    }
}
