<%@ Page Title="ประวัติการสั่งซื้อ" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Orders.aspx.cs" Inherits="Pages_Orders" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
            .orders-card {
                background: #ffffff;
                border-radius: 20px;
                box-shadow: 0 10px 40px rgba(0, 0, 0, 0.1);
                overflow: hidden;
                border: none;
            }

            .orders-table thead {
                background-color: #f8f9fa;
            }

            .orders-table th {
                font-weight: 700;
                text-transform: uppercase;
                font-size: 0.8rem;
                letter-spacing: 1px;
                color: #888;
                padding: 20px 25px;
                border: none;
            }

            .orders-table td {
                padding: 20px 25px;
                vertical-align: middle;
                border-bottom: 1px solid #f0f0f0;
            }

            .badge-status {
                padding: 8px 16px;
                border-radius: 50px;
                font-weight: 600;
                font-size: 0.75rem;
            }

            .btn-view-details {
                border-radius: 50px;
                padding: 8px 20px;
                font-weight: 600;
                font-size: 0.85rem;
                transition: all 0.3s;
            }

            .btn-view-details:hover {
                transform: translateY(-2px);
                box-shadow: 0 4px 10px rgba(var(--bs-primary-rgb), 0.2);
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <h1 class="mb-4">ประวัติการสั่งซื้อ</h1>

            <% if (OrderList.Count> 0) { %>
                <div class="orders-card mb-5">
                    <div class="table-responsive">
                        <table class="table table-hover orders-table mb-0">
                            <thead>
                                <tr>
                                    <th>รหัสคำสั่งซื้อ</th>
                                    <th>วันที่</th>
                                    <th>ยอดรวม</th>
                                    <th>สถานะ</th>
                                    <th class="text-end">การดำเนินการ</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% int counter=1; foreach (var order in OrderList) { %>
                                    <tr>
                                        <td class="fw-bold text-dark">#<%= counter++ %>
                                        </td>
                                        <td class="text-secondary">
                                            <%= order.OrderDate.ToString("MMMM dd, yyyy") %>
                                        </td>
                                        <td class="fw-bold text-primary h5 mb-0">
                                            <%= order.TotalAmount.ToString("C") %>
                                        </td>
                                        <td>
                                            <span class="badge badge-status bg-<%= GetStatusColor(order.Status) %>">
                                                <%= GetThaiStatus(order.Status) %>
                                            </span>
                                        </td>
                                        <td class="text-end">
                                            <a href="OrderDetails.aspx?id=<%= order.Id %>"
                                                class="btn btn-primary btn-view-details shadow-sm">
                                                <i class="fas fa-eye me-2"></i>ดูรายละเอียด
                                            </a>
                                        </td>
                                    </tr>
                                    <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
                <% } else { %>
                    <div class="text-center py-5">
                        <i class="fas fa-box-open fa-3x text-muted mb-3"></i>
                        <h3>ไม่พบประวัติการสั่งซื้อ</h3>
                        <p class="text-muted">คุณยังไม่ได้ทำการสั่งซื้อสินค้าใดๆ</p>
                        <a href="Products.aspx" class="btn btn-primary mt-3">เริ่มการสั่งซื้อ</a>
                    </div>
                    <% } %>
        </div>
    </asp:Content>