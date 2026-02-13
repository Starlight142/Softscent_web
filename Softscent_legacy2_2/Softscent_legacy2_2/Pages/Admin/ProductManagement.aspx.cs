using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_ProductManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Simple Admin Check
        if (Session["User"] == null)
        {
            Response.Redirect("~/Pages/Login.aspx");
        }

        if (!IsPostBack)
        {
            LoadProducts();
        }
    }

    private void LoadProducts()
    {
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Products ORDER BY Id DESC");
        gvProducts.DataSource = dt;
        gvProducts.DataBind();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ClearModal();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
    }

    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditProduct")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            LoadProductDetails(id);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }
    }

    protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
        string query = "DELETE FROM Products WHERE Id = @Id";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };
        
        DataHelper.ExecuteNonQuery(query, parameters);
        LoadProducts();
    }

    private void LoadProductDetails(int id)
    {
        string query = "SELECT * FROM Products WHERE Id = @Id";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };

        DataTable dt = DataHelper.ExecuteQuery(query, parameters);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            hfProductId.Value = row["Id"].ToString();
            txtName.Text = row["Name"].ToString();
            txtNameThai.Text = row["NameThai"].ToString();
            txtDesc.Text = row["Description"].ToString();
            txtDescThai.Text = row["DescriptionThai"].ToString();
            txtPrice.Text = row["Price"].ToString();
            txtImageUrl.Text = row["ImageUrl"].ToString();
            chkCustomizable.Checked = row["IsCustomizable"] != DBNull.Value && Convert.ToBoolean(row["IsCustomizable"]);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            decimal price = 0;
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                // Simple validation feedback (in real app us validators)
                return;
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Name", txtName.Text },
                { "@NameThai", txtNameThai.Text },
                { "@Description", txtDesc.Text },
                { "@DescriptionThai", txtDescThai.Text },
                { "@Price", price },
                { "@ImageUrl", txtImageUrl.Text },
                { "@IsCustomizable", chkCustomizable.Checked }
            };

            if (string.IsNullOrEmpty(hfProductId.Value))
            {
                // Insert
                string query = @"INSERT INTO Products (Name, NameThai, Description, DescriptionThai, Price, ImageUrl, IsCustomizable) 
                               Values (@Name, @NameThai, @Description, @DescriptionThai, @Price, @ImageUrl, @IsCustomizable)";
                DataHelper.ExecuteNonQuery(query, parameters);
            }
            else
            {
                // Update
                parameters.Add("@Id", Convert.ToInt32(hfProductId.Value));
                string query = @"UPDATE Products SET 
                               Name = @Name, 
                               NameThai = @NameThai, 
                               Description = @Description, 
                               DescriptionThai = @DescriptionThai, 
                               Price = @Price, 
                               ImageUrl = @ImageUrl, 
                               IsCustomizable = @IsCustomizable
                               WHERE Id = @Id";
                DataHelper.ExecuteNonQuery(query, parameters);
            }

            // Hide modal and reload
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hide", "hideModal();", true);
            LoadProducts();
        }
        catch (Exception ex)
        {
            // Log error
            errorAlert.InnerText = ex.Message;
            errorAlert.Attributes.Remove("class");
            errorAlert.Attributes.Add("class", "alert alert-danger mt-3");
        }
    }

    private void ClearModal()
    {
        hfProductId.Value = "";
        txtName.Text = "";
        txtNameThai.Text = "";
        txtDesc.Text = "";
        txtDescThai.Text = "";
        txtPrice.Text = "";
        txtImageUrl.Text = "";
        chkCustomizable.Checked = false;
        errorAlert.Attributes.Add("class", "alert alert-danger mt-3 d-none");
    }
    protected string GetImageUrl(object urlObj)
    {
        if (urlObj == null || urlObj == DBNull.Value) return "https://placehold.co/50x50";
        string url = urlObj.ToString();
        if (string.IsNullOrEmpty(url)) return "https://placehold.co/50x50";
        if (url.StartsWith("http") || url.StartsWith("/")) return url;
        return "/" + url;
    }
}
