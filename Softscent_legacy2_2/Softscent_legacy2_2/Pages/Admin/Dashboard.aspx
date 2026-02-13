<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/MasterPageCMS.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Pages_Admin_Dashboard" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div
            class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h1 class="h2">Dashboard</h1>
        </div>

        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card text-white bg-primary mb-3">
                    <div class="card-header">Total Orders</div>
                    <div class="card-body">
                        <h5 class="card-title">
                            <%= TotalOrders %>
                        </h5>
                        <p class="card-text">All time orders.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card text-white bg-success mb-3">
                    <div class="card-header">Total Revenue</div>
                    <div class="card-body">
                        <h5 class="card-title">
                            <%= TotalRevenue.ToString("C") %>
                        </h5>
                        <p class="card-text">Lifetime earnings.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card text-white bg-info mb-3">
                    <div class="card-header">Pending Orders</div>
                    <div class="card-body">
                        <h5 class="card-title">
                            <%= PendingOrders %>
                        </h5>
                        <p class="card-text">Orders waiting for process.</p>
                    </div>
                </div>
            </div>
        </div>

        <div class="card shadow-sm">
            <div class="card-body">
                <h3 class="card-title mb-3">Recent Orders</h3>
                <div class="table-responsive">
                    <table class="table table-striped table-sm mb-0">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Date</th>
                                <th>Status</th>
                                <th>Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var order in RecentOrders) { %>
                                <tr>
                                    <td>
                                        <%= order.Id %>
                                    </td>
                                    <td>
                                        <%= order.OrderDate.ToShortDateString() %>
                                    </td>
                                    <td>
                                        <%= order.Status %>
                                    </td>
                                    <td>
                                        <%= order.TotalAmount.ToString("C") %>
                                    </td>
                                </tr>
                                <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </asp:Content>