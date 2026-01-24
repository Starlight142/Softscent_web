<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OrderDetails.aspx.cs" Inherits="Pages_OrderDetails" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
            .order-info-card {
                border-radius: 15px;
                overflow: hidden;
                border: none;
                box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
            }

            .status-badge {
                font-size: 0.9rem;
                padding: 8px 16px;
                border-radius: 50px;
            }

            .item-row {
                transition: background 0.2s;
            }

            .item-row:hover {
                background-color: #f8f9fa;
            }

            .detail-label {
                font-size: 0.8rem;
                text-transform: uppercase;
                letter-spacing: 1px;
                color: #888;
                font-weight: 600;
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h1 class="mb-0">Order #<%= OrderId %>
                </h1>
                <a href="Orders.aspx" class="btn btn-outline-secondary btn-sm">
                    <i class="fas fa-arrow-left me-2"></i>Back to My Orders
                </a>
            </div>

            <% if (CurrentOrder !=null) { %>
                <div class="row">
                    <div class="col-md-8">
                        <div class="card order-info-card mb-4">
                            <div class="card-header bg-white border-0 py-3">
                                <h5 class="mb-0 fw-bold"><i class="fas fa-shopping-bag me-2 text-primary"></i>Items
                                    Ordered</h5>
                            </div>
                            <div class="table-responsive">
                                <table class="table align-middle mb-0">
                                    <thead class="bg-light">
                                        <tr>
                                            <th class="border-0 px-4">Product</th>
                                            <th class="border-0">Price</th>
                                            <th class="border-0 text-center">Qty</th>
                                            <th class="border-0 text-end px-4">Subtotal</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <% foreach (var item in CurrentOrder.OrderDetails) { var
                                            prodName=item.ProductInfo !=null ? item.ProductInfo.Name : "Product #" +
                                            item.ProductId; %>
                                            <tr class="item-row">
                                                <td class="px-4 py-3">
                                                    <div class="fw-bold">
                                                        <%= prodName %>
                                                    </div>
                                                    <% if (!string.IsNullOrEmpty(item.CustomConfiguration)) { %>
                                                        <div class="text-muted small">
                                                            <i class="fas fa-magic me-1"></i>
                                                            <%= item.CustomConfiguration %>
                                                        </div>
                                                        <% } %>
                                                </td>
                                                <td>
                                                    <%= item.UnitPrice.ToString("C") %>
                                                </td>
                                                <td class="text-center">
                                                    <%= item.Quantity %>
                                                </td>
                                                <td class="text-end px-4 fw-bold text-primary">
                                                    <%= (item.UnitPrice * item.Quantity).ToString("C") %>
                                                </td>
                                            </tr>
                                            <% } %>
                                    </tbody>
                                </table>
                            </div>
                            <div class="card-footer bg-white border-0 py-4 px-4">
                                <div class="d-flex justify-content-between align-items-center">
                                    <h5 class="mb-0 text-secondary">Total Amount</h5>
                                    <h3 class="mb-0 fw-bold text-primary">
                                        <%= CurrentOrder.TotalAmount.ToString("C") %>
                                    </h3>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="card order-info-card mb-4">
                            <div class="card-body p-4">
                                <div class="mb-4">
                                    <div class="detail-label mb-2">Order Status</div>
                                    <span class="badge status-badge bg-<%= GetStatusColor(CurrentOrder.Status) %>">
                                        <%= CurrentOrder.Status %>
                                    </span>
                                </div>
                                <div class="mb-4">
                                    <div class="detail-label mb-2">Order Date</div>
                                    <div class="fw-bold">
                                        <%= CurrentOrder.OrderDate.ToString("MMMM dd, yyyy HH:mm") %>
                                    </div>
                                </div>
                                <hr />
                                <div class="mb-4">
                                    <div class="detail-label mb-2">Shipping Information</div>
                                    <div class="fw-bold">
                                        <%= CurrentOrder.ShippingMethod %> Delivery
                                    </div>
                                    <div class="text-muted small mt-1">
                                        <%= CurrentOrder.ShippingAddress %>
                                    </div>
                                </div>
                                <div class="mb-0">
                                    <div class="detail-label mb-2">Payment Details</div>
                                    <div class="fw-bold">
                                        <%= CurrentOrder.PaymentMethod %>
                                    </div>
                                    <div class="text-<%= CurrentOrder.PaymentStatus == " Paid" ? "success" : "warning"
                                        %> small fw-bold">
                                        <i class="fas fa-<%= CurrentOrder.PaymentStatus == " Paid" ? "check-circle"
                                            : "clock" %> me-1"></i>
                                        <%= CurrentOrder.PaymentStatus %>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <% } else { %>
                    <div class="alert alert-danger shadow-sm">
                        <i class="fas fa-exclamation-triangle me-2"></i>Order not found or you don't have permission to
                        view it.
                    </div>
                    <% } %>
        </div>
    </asp:Content>