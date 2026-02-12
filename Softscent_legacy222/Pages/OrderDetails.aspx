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

            /* Timeline CSS */
            .timeline-track {
                position: relative;
                margin: 40px 0 60px 0;
                height: 4px;
                background: #e9ecef;
                border-radius: 4px;
            }

            .timeline-step {
                position: absolute;
                top: 42px;
                transform: translate(-50%, -50%);
                width: 40px;
                height: 40px;
                background: #fff;
                border: 2px solid #e9ecef;
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                color: #adb5bd;
                font-size: 1rem;
                z-index: 10;
                transition: all 0.3s;
            }

            .timeline-step.active {
                border-color: var(--bs-primary);
                background: var(--bs-primary);
                color: #fff;
                box-shadow: 0 0 0 4px rgba(var(--bs-primary-rgb), 0.2);
            }

            .timeline-label {
                position: absolute;
                top: 80px;
                width: 150px;
                text-align: center;
                transform: translateX(-50%);
                font-size: 0.8rem;
                font-weight: 600;
                color: #adb5bd;
                white-space: nowrap;
            }

            .timeline-label.text-start-custom {
                transform: translateX(0);
                text-align: left;
            }

            .timeline-label.text-end-custom {
                transform: translateX(-100%);
                text-align: right;
            }

            .timeline-step.active+.timeline-label {
                color: var(--bs-primary);
            }

            .timeline-progress {
                position: absolute;
                top: 0;
                left: 0;
                height: 100%;
                background: var(--bs-primary);
                border-radius: 4px;
                transition: width 0.5s ease;
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

            <!-- Tracking Timeline -->
            <% string status=CurrentOrder !=null ? CurrentOrder.Status.ToLower() : "" ; int progress=0; if
                (status=="pending" ) progress=10; else if (status=="paid" ) progress=35; else if (status=="shipped" ||
                status=="out_for_delivery" ) progress=65; else if (status=="delivered" || status=="completed" )
                progress=100; else if (status=="cancelled" ) progress=0; %>
                <% if (status !="cancelled" ) { %>
                    <div class="card border-0 shadow-sm rounded-4 mb-4 p-4">
                        <div class="position-relative mx-4">
                            <div class="timeline-track">
                                <div class="timeline-progress" style="width: <%= progress %>%;"></div>
                            </div>

                            <!-- Step 1: Pending -->
                            <div class="timeline-step <%= progress >= 10 ? " active" : "" %>" style="left: 0%;">
                                <i class="fas fa-file-invoice"></i>
                            </div>
                            <div class="timeline-label text-start-custom" style="left: 0%;">ได้รับคำสั่งซื้อ</div>

                            <!-- Step 2: Paid -->
                            <div class="timeline-step <%= progress >= 35 ? " active" : "" %>" style="left: 33%;">
                                <i class="fas fa-money-bill-wave"></i>
                            </div>
                            <div class="timeline-label" style="left: 33%;">ชำระเงินแล้ว</div>

                            <!-- Step 3: Shipped -->
                            <div class="timeline-step <%= progress >= 65 ? " active" : "" %>" style="left: 66%;">
                                <i class="fas fa-shipping-fast"></i>
                            </div>
                            <div class="timeline-label" style="left: 66%;">อยู่ระหว่างจัดส่ง</div>

                            <!-- Step 4: Delivered -->
                            <div class="timeline-step <%= progress >= 100 ? " active" : "" %>" style="left: 100%;">
                                <i class="fas fa-check-circle"></i>
                            </div>
                            <div class="timeline-label text-end-custom" style="left: 100%;">จัดส่งสำเร็จ</div>
                        </div>
                    </div>
                    <% } else { %>
                        <div class="alert alert-danger rounded-4 mb-4 text-center">
                            <i class="fas fa-times-circle me-2"></i>คำสั่งซื้อนี้ถูกยกเลิก (Order Cancelled)
                        </div>
                        <% } %>

                            <% if (CurrentOrder !=null) { %>
                                <div class="row">
                                    <div class="col-md-8">
                                        <div class="card order-info-card mb-4">
                                            <div class="card-header bg-white border-0 py-3">
                                                <h5 class="mb-0 fw-bold"><i
                                                        class="fas fa-shopping-bag me-2 text-primary"></i>Items
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
                                                            prodName=item.ProductInfo !=null ? item.ProductInfo.Name
                                                            : "Product #" + item.ProductId; %>
                                                            <tr class="item-row">
                                                                <td class="px-4 py-3">
                                                                    <div class="fw-bold">
                                                                        <%= prodName %>
                                                                    </div>
                                                                    <% if
                                                                        (!string.IsNullOrEmpty(item.CustomConfiguration))
                                                                        { %>
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
                                                                    <%= (item.UnitPrice * item.Quantity).ToString("C")
                                                                        %>
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
                                                    <span
                                                        class="badge status-badge bg-<%= GetStatusColor(CurrentOrder.Status) %>">
                                                        <%= GetThaiStatus(CurrentOrder.Status) %>
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
                                                    <div class="text-<%= CurrentOrder.PaymentStatus == " Paid"
                                                        ? "success" : "warning" %> small fw-bold">
                                                        <i class="fas fa-<%= CurrentOrder.PaymentStatus == " Paid"
                                                            ? "check-circle" : "clock" %> me-1"></i>
                                                        <%= CurrentOrder.PaymentStatus %>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <% } else { %>
                                    <div class="alert alert-danger shadow-sm">
                                        <i class="fas fa-exclamation-triangle me-2"></i>Order not found or you don't
                                        have permission to
                                        view it.
                                    </div>
                                    <% } %>
        </div>
    </asp:Content>