<%@ Page Title="ชำระเงิน" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Checkout.aspx.cs" Inherits="Pages_Checkout" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <div class="row">
                <div class="col-md-8">
                    <h2 class="mb-4">การจัดส่งและการชำระเงิน</h2>
                    <div class="card p-4 shadow-sm border-0">
                        <!-- Standard HTML Form for checkout submission -->
                        <div class="mb-4">
                            <h4 class="mb-3">ที่อยู่จัดส่ง</h4>
                            <div class="mb-3">
                                <label for="shippingAddress" class="form-label text-muted small fw-bold">ที่อยู่</label>
                                <textarea id="shippingAddress" name="shippingAddress" class="form-control" rows="3"
                                    required
                                    placeholder="กรอกที่อยู่สำหรับจัดส่งสินค้าแบบเต็ม..."><%= UserAddress %></textarea>
                            </div>

                            <h4 class="mb-3 mt-4">วิธีการจัดส่ง</h4>
                            <div class="mb-3">
                                <select name="shippingMethod" class="form-select">
                                    <option value="Standard">การส่งแบบธรรมดา (3-5 วัน) - ฟรี</option>
                                    <option value="Express">การส่งแบบด่วน (1-2 วัน) - +$5.00</option>
                                </select>
                            </div>

                            <h4 class="mb-3 mt-4">วิธีการชำระเงิน</h4>
                            <div class="payment-options">
                                <div class="form-check mb-2 p-3 border rounded shadow-sm custom-radio-card">
                                    <input class="form-check-input ms-0 me-3" type="radio" name="paymentMethod"
                                        id="credit" value="Credit Card" checked>
                                    <label class="form-check-label w-100" for="credit">
                                        <i class="fas fa-credit-card me-2 text-primary"></i>บัตรเครดิต
                                    </label>
                                </div>
                                <div class="form-check mb-2 p-3 border rounded shadow-sm custom-radio-card">
                                    <input class="form-check-input ms-0 me-3" type="radio" name="paymentMethod"
                                        id="promptpay" value="PromptPay">
                                    <label class="form-check-label w-100" for="promptpay">
                                        <i class="fas fa-qrcode me-2 text-primary"></i>PromptPay (QR Code)
                                    </label>
                                </div>
                                <div class="form-check mb-4 p-3 border rounded shadow-sm custom-radio-card">
                                    <input class="form-check-input ms-0 me-3" type="radio" name="paymentMethod" id="cod"
                                        value="Cash on Delivery">
                                    <label class="form-check-label w-100" for="cod">
                                        <i class="fas fa-money-bill-wave me-2 text-primary"></i>เก็บเงินปลายทาง
                                    </label>
                                </div>
                            </div>

                            <div class="d-grid mt-5">
                                <asp:Button runat="server" ID="btnCompleteOrder" Text="สั่งซื้อสินค้า"
                                    CssClass="btn btn-primary btn-lg py-3 fw-bold" OnClick="btnCompleteOrder_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <h4 class="d-flex justify-content-between align-items-center mb-3">
                        <span class="text-primary fw-bold">ตะกร้าของคุณ</span>
                        <span class="badge bg-primary rounded-pill">
                            <%= CartCount %>
                        </span>
                    </h4>
                    <ul class="list-group mb-3 shadow-sm border-0">
                        <% foreach(var item in OrderDetails) { %>
                            <li class="list-group-item d-flex justify-content-between lh-sm py-3">
                                <div>
                                    <h6 class="my-0 fw-bold">
                                        <%= item.ProductInfo.Name %>
                                    </h6>
                                    <small class="text-muted">Qty: <%= item.Quantity %></small>
                                    <% if (!string.IsNullOrEmpty(item.CustomConfiguration)) { %>
                                        <div class="text-muted x-small mt-1" style="font-size: 0.75rem;">สูตรผสม: <%=
                                                item.CustomConfiguration %>
                                        </div>
                                        <% } %>
                                </div>
                                <span class="text-muted fw-bold">
                                    <%= (item.UnitPrice * item.Quantity).ToString("C") %>
                                </span>
                            </li>
                            <% } %>
                                <li class="list-group-item d-flex justify-content-between bg-light py-3">
                                    <span class="fw-bold">ยอดรวม (USD)</span>
                                    <strong class="text-primary h5 mb-0">
                                        <%= TotalAmount.ToString("C") %>
                                    </strong>
                                </li>
                    </ul>
                </div>
            </div>
        </div>

        <style>
            .custom-radio-card {
                cursor: pointer;
                transition: all 0.2s;
            }

            .custom-radio-card:hover {
                border-color: var(--primary);
                background-color: #f8fff9;
            }

            .form-check-input:checked+.form-check-label {
                color: var(--primary);
                font-weight: bold;
            }
        </style>
    </asp:Content>