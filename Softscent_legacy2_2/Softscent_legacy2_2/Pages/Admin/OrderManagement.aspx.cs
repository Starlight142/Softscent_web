using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Admin_OrderManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Simple Admin Check (In real app, check Role)
        if (Session["User"] == null)
        {
            Response.Redirect("~/Pages/Login.aspx");
        }

        if (!IsPostBack)
        {
            LoadOrders();
        }
    }

    private void LoadOrders()
    {
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Orders ORDER BY OrderDate DESC");
        gvOrders.DataSource = dt;
        gvOrders.DataBind();
    }

    protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Pre-select the current status in dropdown
            string currentStatus = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddlStatus");
            if (ddl != null && ddl.Items.FindByValue(currentStatus) != null)
            {
                ddl.SelectedValue = currentStatus;
            }
        }
    }

    protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UpdateStatus")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvOrders.Rows[index];
            int orderId = Convert.ToInt32(gvOrders.DataKeys[index].Value);

            DropDownList ddl = (DropDownList)row.FindControl("ddlStatus");
            string newStatus = ddl.SelectedValue;

            UpdateOrderStatus(orderId, newStatus);
            LoadOrders();
        }
    }

    private void UpdateOrderStatus(int orderId, string status)
    {
        string query = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
        var parameters = new Dictionary<string, object>
        {
            { "@Status", status },
            { "@Id", orderId }
        };
        DataHelper.ExecuteNonQuery(query, parameters);
    }

    public string GetStatusColor(string status)
    {
        switch (status.ToLower())
        {
            case "completed":
            case "delivered":
                return "success";
            case "pending": return "warning";
            case "cancelled": return "danger";
            case "shipped":
            case "out_for_delivery":
                return "info";
            case "paid": return "primary";
            default: return "secondary";
        }
    }

    public string GetThaiStatus(string status)
    {
        switch (status.ToLower())
        {
            case "completed": return "สำเร็จ";
            case "pending": return "รอชำระเงิน";
            case "paid": return "ชำระเงินแล้ว";
            case "shipped": return "อยู่ระหว่างจัดส่ง";
            case "out_for_delivery": return "กำลังนำจ่าย";
            case "delivered": return "จัดส่งสำเร็จ";
            case "cancelled": return "ยกเลิก";
            default: return status;
        }
    }
}
