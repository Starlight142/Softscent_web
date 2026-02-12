<%@ Page Title="Manage Orders" Language="C#" MasterPageFile="~/MasterPageCMS.master" AutoEventWireup="true"
    CodeFile="OrderManagement.aspx.cs" Inherits="Pages_Admin_OrderManagement" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
            .status-select {
                min-width: 140px;
                border-radius: 20px;
                border: 1px solid #dee2e6;
                padding: 5px 10px;
                font-size: 0.9rem;
            }

            .btn-update {
                border-radius: 20px;
                padding: 5px 15px;
                font-size: 0.9rem;
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div
            class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h1 class="h2">จัดการคำสั่งซื้อ (Order Management)</h1>
        </div>

        <div class="card shadow-sm border-0">
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvOrders" runat="server" CssClass="table table-hover align-middle"
                        AutoGenerateColumns="False" OnRowCommand="gvOrders_RowCommand"
                        OnRowDataBound="gvOrders_RowDataBound" DataKeyNames="Id" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="#" ItemStyle-Font-Bold="true" />
                            <asp:BoundField DataField="OrderDate" HeaderText="วันที่สั่งซื้อ"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="UserId" HeaderText="User ID" />
                            <asp:BoundField DataField="TotalAmount" HeaderText="ยอดรวม" DataFormatString="{0:C}"
                                ItemStyle-CssClass="fw-bold text-primary" />

                            <asp:TemplateField HeaderText="ที่อยู่จัดส่ง">
                                <ItemTemplate>
                                    <div style="max-width: 200px; font-size: 0.85rem;" class="text-truncate"
                                        title='<%# Eval("ShippingAddress") %>'>
                                        <%# Eval("ShippingAddress") %>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="สถานะปัจจุบัน">
                                <ItemTemplate>
                                    <span class='badge bg-<%# GetStatusColor(Eval("Status").ToString()) %>'>
                                        <%# GetThaiStatus(Eval("Status").ToString()) %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="อัพเดทสถานะ">
                                <ItemTemplate>
                                    <div class="d-flex gap-2">
                                        <asp:DropDownList ID="ddlStatus" runat="server"
                                            CssClass="form-select status-select">
                                            <asp:ListItem Value="Pending">รอชำระเงิน</asp:ListItem>
                                            <asp:ListItem Value="Paid">ชำระเงินแล้ว</asp:ListItem>
                                            <asp:ListItem Value="Shipped">อยู่ระหว่างจัดส่ง</asp:ListItem>
                                            <asp:ListItem Value="Out_For_Delivery">กำลังนำจ่าย</asp:ListItem>
                                            <asp:ListItem Value="Delivered">จัดส่งสำเร็จ</asp:ListItem>
                                            <asp:ListItem Value="Cancelled">ยกเลิก</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnUpdate" runat="server" Text="บันทึก"
                                            CommandName="UpdateStatus" CommandArgument='<%# Container.DataItemIndex %>'
                                            CssClass="btn btn-primary btn-update" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center py-5">
                                <p class="text-muted">ไม่พบข้อมูลคำสั่งซื้อ</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Content>