<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/MasterPageCMS.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Pages_Admin_Dashboard" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="mb-5">
            <h2 class="fw-bold mb-1">Dashboard Overview</h2>
            <p class="text-muted">Welcome back, Admin. Here's what's happening with your store today.</p>
        </div>

        <div class="row g-4 mb-5">
            <!-- Total Orders -->
            <div class="col-md-4">
                <div class="stat-card primary">
                    <div class="stat-icon">
                        <i class="fas fa-shopping-bag"></i>
                    </div>
                    <div class="stat-value">
                        <%= TotalOrders %>
                    </div>
                    <div class="stat-label">Total Orders</div>
                    <div class="position-absolute top-0 end-0 p-3 opacity-25">
                        <i class="fas fa-shopping-bag fa-5x text-primary transform scale-150"></i>
                    </div>
                </div>
            </div>

            <!-- Revenue -->
            <div class="col-md-4">
                <div class="stat-card warning">
                    <div class="stat-icon">
                        <i class="fas fa-coins"></i>
                    </div>
                    <div class="stat-value">
                        <%= TotalRevenue.ToString("C") %>
                    </div>
                    <div class="stat-label">Total Earnings</div>
                    <div class="position-absolute top-0 end-0 p-3 opacity-25">
                        <i class="fas fa-wallet fa-5x text-warning transform scale-150"></i>
                    </div>
                </div>
            </div>

            <!-- Pending -->
            <div class="col-md-4">
                <div class="stat-card info">
                    <div class="stat-icon">
                        <i class="fas fa-clock"></i>
                    </div>
                    <div class="stat-value">
                        <%= PendingOrders %>
                    </div>
                    <div class="stat-label">Pending Orders</div>
                    <div class="position-absolute top-0 end-0 p-3 opacity-25">
                        <i class="fas fa-hourglass-half fa-5x text-info transform scale-150"></i>
                    </div>
                </div>
            </div>
        </div>

        <h4 class="fw-bold mb-4">Recent Transactions</h4>
        <div class="modern-table-card">
            <div class="table-responsive">
                <table class="table table-hover align-middle mb-0">
                    <thead class="bg-light">
                        <tr>
                            <th class="ps-4">Order ID</th>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Amount</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach (var order in RecentOrders) { %>
                            <tr>
                                <td class="ps-4 fw-bold">#<%= order.Id %>
                                </td>
                                <td class="text-muted">
                                    <i class="far fa-calendar-alt me-2"></i>
                                    <%= order.OrderDate.ToShortDateString() %>
                                </td>
                                <td>
                                    <% string badgeClass="completed" ; if(order.Status=="Pending" ) badgeClass="pending"
                                        ; else if(order.Status=="Cancelled" ) badgeClass="cancelled" ; %>
                                        <span class="status-badge <%= badgeClass %>">
                                            <%= order.Status %>
                                        </span>
                                </td>
                                <td class="fw-bold">
                                    <%= order.TotalAmount.ToString("C") %>
                                </td>
                                <td>
                                    <a href="#" class="btn btn-sm btn-light rounded-circle shadow-sm">
                                        <i class="fas fa-chevron-right"></i>
                                    </a>
                                </td>
                            </tr>
                            <% } %>
                    </tbody>
                </table>
            </div>
        </div>
    </asp:Content>