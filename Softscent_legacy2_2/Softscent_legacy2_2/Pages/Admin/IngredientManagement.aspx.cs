using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_IngredientManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Ensure Database Schema is up to date
        try { DataHelper.EnsureHerbStockColumn(); } catch { }

        // Simple Admin Check
        if (Session["User"] == null)
        {
            Response.Redirect("~/Pages/Login.aspx");
        }

        if (!IsPostBack)
        {
            LoadIngredients();
        }
    }

    private void LoadIngredients()
    {
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Herbs ORDER BY Id ASC");
        gvIngredients.DataSource = dt;
        gvIngredients.DataBind();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ClearModal();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
    }

    protected void gvIngredients_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditIngredient")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            LoadIngredientDetails(id);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }
    }

    protected void gvIngredients_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int id = Convert.ToInt32(gvIngredients.DataKeys[e.RowIndex].Value);
        string query = "DELETE FROM Herbs WHERE Id = @Id";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };
        
        DataHelper.ExecuteNonQuery(query, parameters);
        LoadIngredients();
    }

    private void LoadIngredientDetails(int id)
    {
        string query = "SELECT * FROM Herbs WHERE Id = @Id";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@Id", id }
        };

        DataTable dt = DataHelper.ExecuteQuery(query, parameters);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            hfIngredientId.Value = row["Id"].ToString();
            txtName.Text = row["Name"].ToString();
            txtPrice.Text = row["Price"].ToString();
            txtStock.Text = row["StockQuantity"] != DBNull.Value ? row["StockQuantity"].ToString() : "0";
            txtBenefit.Text = row["Benefit"] != DBNull.Value ? row["Benefit"].ToString() : "";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            decimal price = 0;
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                return;
            }

            int stock = 0;
            if (!int.TryParse(txtStock.Text, out stock)) stock = 0;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Name", txtName.Text },
                { "@Price", price },
                { "@StockQuantity", stock },
                { "@Benefit", txtBenefit.Text }
            };

            if (string.IsNullOrEmpty(hfIngredientId.Value))
            {
                // Insert
                string query = @"INSERT INTO Herbs (Name, Price, StockQuantity, Benefit) 
                               Values (@Name, @Price, @StockQuantity, @Benefit)";
                DataHelper.ExecuteNonQuery(query, parameters);
            }
            else
            {
                // Update
                parameters.Add("@Id", Convert.ToInt32(hfIngredientId.Value));
                string query = @"UPDATE Herbs SET 
                               Name = @Name, 
                               Price = @Price, 
                               StockQuantity = @StockQuantity,
                               Benefit = @Benefit
                               WHERE Id = @Id";
                DataHelper.ExecuteNonQuery(query, parameters);
            }

            // Hide modal and reload
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Hide", "hideModal();", true);
            LoadIngredients();
        }
        catch (Exception ex)
        {
            errorAlert.InnerText = ex.Message;
            errorAlert.Attributes.Remove("class");
            errorAlert.Attributes.Add("class", "alert alert-danger mt-3");
        }
    }

    private void ClearModal()
    {
        hfIngredientId.Value = "";
        txtName.Text = "";
        txtPrice.Text = "0.00";
        txtStock.Text = "0";
        txtBenefit.Text = "";
        errorAlert.Attributes.Add("class", "alert alert-danger mt-3 d-none");
    }
}
