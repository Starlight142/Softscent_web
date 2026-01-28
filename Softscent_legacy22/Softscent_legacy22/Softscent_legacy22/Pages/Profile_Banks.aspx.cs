using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Profile_Banks : System.Web.UI.Page
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
            LoadBanks();
        }
    }

    private void LoadUserProfile()
    {
        string userEmail = Session["User"].ToString();
        string query = "SELECT FullName FROM Users WHERE Email = @Email";
        DataTable dt = DataHelper.ExecuteQuery(query, new Dictionary<string, object> { { "@Email", userEmail } });
        if (dt.Rows.Count > 0)
        {
            lblSidebarName.Text = dt.Rows[0]["FullName"] != DBNull.Value ? dt.Rows[0]["FullName"].ToString() : "ผู้ใช้งาน";
        }
        else
        {
            lblSidebarName.Text = "ผู้ใช้งาน";
        }
    }

    private void LoadBanks()
    {
        string userId = GetCurrentUserId();
        string query = "SELECT * FROM UserBanks WHERE UserId = @UserId ORDER BY IsDefault DESC, Id DESC";
        DataTable dt = DataHelper.ExecuteQuery(query, new Dictionary<string, object> { { "@UserId", userId } });
        
        rptBanks.DataSource = dt;
        rptBanks.DataBind();

        lblEmpty.Visible = dt.Rows.Count == 0;
    }

    protected void btnSaveBank_Click(object sender, EventArgs e)
    {
        string userId = GetCurrentUserId();
        string bankName = ddlBankName.SelectedValue;
        string accountName = txtAccountName.Text.Trim();
        string accountNumber = txtAccountNumber.Text.Trim();
        bool isDefault = chkIsDefault.Checked;

        if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(accountNumber))
        {
            lblModalMsg.CssClass = "text-danger fw-bold";
            lblModalMsg.Text = "กรุณากรอกข้อมูลให้ครบถ้วน";
            return;
        }

        try
        {
            // If setting as default, unset others first
            if (isDefault)
            {
                DataHelper.ExecuteNonQuery("UPDATE UserBanks SET IsDefault = 0 WHERE UserId = @UserId", 
                    new Dictionary<string, object> { { "@UserId", userId } });
            }

            string query = @"INSERT INTO UserBanks (UserId, BankName, AccountName, AccountNumber, IsDefault) 
                             VALUES (@UserId, @BankName, @AccountName, @AccountNumber, @IsDefault)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", userId },
                { "@BankName", bankName },
                { "@AccountName", accountName },
                { "@AccountNumber", accountNumber },
                { "@IsDefault", isDefault }
            };

            DataHelper.ExecuteNonQuery(query, parameters);

            // Clear form and reload
            txtAccountName.Text = "";
            txtAccountNumber.Text = "";
            chkIsDefault.Checked = false;
            LoadBanks();
            
            // Note: In real WebForms, closing modal requires JS injection or UpdatePanel
             Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", 
                 "var myModalEl = document.getElementById('addBankModal'); var modal = bootstrap.Modal.getInstance(myModalEl) || new bootstrap.Modal(myModalEl); modal.hide();", true);
        }
        catch (Exception ex)
        {
            lblModalMsg.Text = "Error: " + ex.Message;
        }
    }

    protected void rptBanks_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string userId = GetCurrentUserId();
        int bankId = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Delete")
        {
            string query = "DELETE FROM UserBanks WHERE Id = @Id AND UserId = @UserId";
            DataHelper.ExecuteNonQuery(query, new Dictionary<string, object> 
            { 
                { "@Id", bankId },
                { "@UserId", userId }
            });
            LoadBanks();
        }
        else if (e.CommandName == "SetDefault")
        {
            // Unset all
            DataHelper.ExecuteNonQuery("UPDATE UserBanks SET IsDefault = 0 WHERE UserId = @UserId", 
                new Dictionary<string, object> { { "@UserId", userId } });
            
            // Set specific
            DataHelper.ExecuteNonQuery("UPDATE UserBanks SET IsDefault = 1 WHERE Id = @Id AND UserId = @UserId", 
                new Dictionary<string, object> 
                { 
                    { "@Id", bankId },
                    { "@UserId", userId }
                });
            LoadBanks();
        }
    }

    private string GetCurrentUserId()
    {
        if (Session["User"] == null) return null;
        string email = Session["User"].ToString();
        DataTable dt = DataHelper.ExecuteQuery("SELECT Id FROM Users WHERE Email = @Email", new Dictionary<string, object> { { "@Email", email } });
        if (dt.Rows.Count > 0) return dt.Rows[0]["Id"].ToString();
        return null;
    }
}
