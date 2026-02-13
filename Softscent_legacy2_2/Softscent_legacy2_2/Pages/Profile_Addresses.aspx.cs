using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Profile_Addresses : System.Web.UI.Page
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
            LoadAddresses();
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

    private void LoadAddresses()
    {
        string userId = GetCurrentUserId();
        string query = "SELECT * FROM UserAddresses WHERE UserId = @UserId ORDER BY IsDefault DESC, Id DESC";
        DataTable dt = DataHelper.ExecuteQuery(query, new Dictionary<string, object> { { "@UserId", userId } });
        
        rptAddresses.DataSource = dt;
        rptAddresses.DataBind();

        lblEmpty.Visible = dt.Rows.Count == 0;
    }

    protected void btnSaveAddress_Click(object sender, EventArgs e)
    {
        string userId = GetCurrentUserId();
        string fullName = txtName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string addressLine = txtAddressLine.Text.Trim();
        string province = txtProvince.Text.Trim();
        string postalCode = txtPostalCode.Text.Trim();
        bool isDefault = chkIsDefaultAddress.Checked;

        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(addressLine) || string.IsNullOrEmpty(province))
        {
            lblModalMsg.CssClass = "text-danger fw-bold";
            lblModalMsg.Text = "กรุณากรอกข้อมูลสำคัญให้ครบถ้วน";
            return;
        }

        try
        {
            // If setting as default, unset others first
            if (isDefault)
            {
                DataHelper.ExecuteNonQuery("UPDATE UserAddresses SET IsDefault = 0 WHERE UserId = @UserId", 
                    new Dictionary<string, object> { { "@UserId", userId } });
            }

            string query = @"INSERT INTO UserAddresses (UserId, FullName, PhoneNumber, AddressLine, Province, PostalCode, IsDefault) 
                             VALUES (@UserId, @FullName, @PhoneNumber, @AddressLine, @Province, @PostalCode, @IsDefault)";
            
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", userId },
                { "@FullName", fullName },
                { "@PhoneNumber", phone },
                { "@AddressLine", addressLine },
                { "@Province", province },
                { "@PostalCode", postalCode },
                { "@IsDefault", isDefault }
            };

            DataHelper.ExecuteNonQuery(query, parameters);

            // Clear form and reload
            txtName.Text = "";
            txtPhone.Text = "";
            txtAddressLine.Text = "";
            txtProvince.Text = "";
            txtPostalCode.Text = "";
            chkIsDefaultAddress.Checked = false;
            LoadAddresses();
            
             Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseModal", 
                 "var myModalEl = document.getElementById('addAddressModal'); var modal = bootstrap.Modal.getInstance(myModalEl) || new bootstrap.Modal(myModalEl); modal.hide();", true);
        }
        catch (Exception ex)
        {
            lblModalMsg.Text = "Error: " + ex.Message;
        }
    }

    protected void rptAddresses_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string userId = GetCurrentUserId();
        int addressId = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Delete")
        {
            string query = "DELETE FROM UserAddresses WHERE Id = @Id AND UserId = @UserId";
            DataHelper.ExecuteNonQuery(query, new Dictionary<string, object> 
            { 
                { "@Id", addressId },
                { "@UserId", userId }
            });
            LoadAddresses();
        }
        else if (e.CommandName == "SetDefault")
        {
            // Unset all
            DataHelper.ExecuteNonQuery("UPDATE UserAddresses SET IsDefault = 0 WHERE UserId = @UserId", 
                new Dictionary<string, object> { { "@UserId", userId } });
            
            // Set specific
            DataHelper.ExecuteNonQuery("UPDATE UserAddresses SET IsDefault = 1 WHERE Id = @Id AND UserId = @UserId", 
                new Dictionary<string, object> 
                { 
                    { "@Id", addressId },
                    { "@UserId", userId }
                });
            LoadAddresses();
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
