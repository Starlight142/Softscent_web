using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

public partial class Pages_Profile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!IsPostBack)
        {
            InitDateDropdowns();
            LoadProfile();
        }
    }

    private void InitDateDropdowns()
    {
        ddlDay.Items.Add(new System.Web.UI.WebControls.ListItem("Day", ""));
        for (int i = 1; i <= 31; i++) ddlDay.Items.Add(i.ToString());

        ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem("Month", ""));
        string[] months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
        for (int i = 0; i < 12; i++) ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(months[i], (i + 1).ToString()));

        ddlYear.Items.Add(new System.Web.UI.WebControls.ListItem("Year", ""));
        for (int i = DateTime.Now.Year; i >= 1900; i--) ddlYear.Items.Add(i.ToString());
    }

    private void LoadProfile()
    {
        string email = Session["User"].ToString();
        lblSidebarName.Text = email.Split('@')[0]; // Simple display name

        // Fetch User Data
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];

            lblUsername.Text = row["UserName"] != DBNull.Value ? row["UserName"].ToString() : email;
            txtName.Text = row["FullName"] != DBNull.Value ? row["FullName"].ToString() : "";
            lblDisplayEmail.Text = MaskEmail(email);

            // Try load new fields
            try
            {
                txtPhone.Text = row["PhoneNumber"] != DBNull.Value ? row["PhoneNumber"].ToString() : "";
            }
            catch { }

            try
            {
                string gender = row["Gender"] != DBNull.Value ? row["Gender"].ToString() : "";
                if (gender == "Male") rbMale.Checked = true;
                else if (gender == "Female") rbFemale.Checked = true;
                else if (gender == "Other") rbOther.Checked = true;
            }
            catch { }

            try
            {
                if (row["BirthDate"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(row["BirthDate"]);
                    ddlDay.SelectedValue = dob.Day.ToString();
                    ddlMonth.SelectedValue = dob.Month.ToString();
                    ddlYear.SelectedValue = dob.Year.ToString();
                }
            }
            catch { }
        }
    }

    private string MaskEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@")) return email;
        var parts = email.Split('@');
        if (parts[0].Length <= 2) return email;
        return parts[0].Substring(0, 2) + "****" + "@" + parts[1];
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string email = Session["User"].ToString();
        string fullName = txtName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string gender = rbMale.Checked ? "Male" : (rbFemale.Checked ? "Female" : (rbOther.Checked ? "Other" : ""));

        DateTime? dob = null;
        if (!string.IsNullOrEmpty(ddlDay.SelectedValue) && !string.IsNullOrEmpty(ddlMonth.SelectedValue) && !string.IsNullOrEmpty(ddlYear.SelectedValue))
        {
            try
            {
                dob = new DateTime(int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), int.Parse(ddlDay.SelectedValue));
            }
            catch { }
        }

        // Schema update string
        // Note: We are dropping Address/City/PostalCode from the query as they aren't in the new UI reference,
        // but we might want to keep them if I decided to keep the sidebar link active. 
        // For now, I only update what's in the form.

        string updateSql = @"UPDATE Users SET 
                                FullName = @FullName, 
                                PhoneNumber = @Phone,
                                Gender = @Gender,
                                BirthDate = @BirthDate
                             WHERE Email = @Email";

        var parameters = new Dictionary<string, object>
        {
            { "@FullName", fullName },
            { "@Phone", phone },
            { "@Gender", gender },
            { "@BirthDate", dob ?? (object)DBNull.Value },
            { "@Email", email }
        };

        try
        {
            DataHelper.ExecuteNonQuery(updateSql, parameters);
            lblMessage.Text = "Profile updated successfully!";
            lblMessage.CssClass = "text-success";
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error updating profile: " + ex.Message;
            lblMessage.CssClass = "text-danger";

            // Auto-fix attempt for new columns
            if (ex.Message.Contains("Invalid column name"))
            {
                lblMessage.Text += " Attempting to fix database schema...";
                try
                {
                    DataHelper.ExecuteNonQuery("ALTER TABLE Users ADD PhoneNumber NVARCHAR(50), Gender NVARCHAR(20), BirthDate DATETIME");
                    // Retry update
                    DataHelper.ExecuteNonQuery(updateSql, parameters);
                    lblMessage.Text = "Profile updated successfully! (Schema updated)";
                    lblMessage.CssClass = "text-success";
                }
                catch (Exception ex2)
                {
                    lblMessage.Text += " Schema fix failed: " + ex2.Message;
                }
            }
        }
    }
}
