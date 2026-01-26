<%@ Page Title="ตะกร้าของคุณ" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Cart.aspx.cs" Inherits="Pages_Cart" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <h1 class="mb-4">ตะกร้าสินค้า</h1>

            <% if (CurrentOrder !=null && CurrentOrder.OrderDetails.Count> 0) { %>
                <div class="card p-4 shadow-sm border-0">
                    <div class="table-responsive">
                        <table class="table align-middle">
                            <thead>
                                <tr>
                                    <th class="border-0">สินค้า</th>
                                    <th class="border-0">ราคา</th>
                                    <th class="border-0">จำนวน</th>
                                    <th class="border-0 text-end">ยอดรวม</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% foreach (var item in CurrentOrder.OrderDetails) { var prodName=item.ProductInfo
                                    !=null ? item.ProductInfo.Name : "Unknown Product" ; %>
                                    <tr>
                                        <td class="py-3">
                                            <div class="fw-bold">
                                                <%= prodName %>
                                            </div>
                                            <% if (!string.IsNullOrEmpty(item.CustomConfiguration)) { %>
                                                <small class="text-muted">สูตรที่ปรุง: <%= item.CustomConfiguration %>
                                                </small>
                                                <% } %>
                                        </td>
                                        <td>
                                            <%= item.UnitPrice.ToString("C") %>
                                        </td>
                                        <div class="input-group input-group-sm" style="width: 120px;">
                                            <button type="button" class="btn btn-outline-secondary"
                                                onclick="updateQty('<%= item.ProductId %>', <%= item.Quantity - 1 %>)">-</button>
                                            <input type="text" class="form-control text-center"
                                                value="<%= item.Quantity %>" readonly>
                                            <button type="button" class="btn btn-outline-secondary"
                                                onclick="updateQty('<%= item.ProductId %>', <%= item.Quantity + 1 %>)">+</button>
                                        </div>
                                        </td>
                                        <td class="text-end fw-bold">
                                            <%= (item.UnitPrice * item.Quantity).ToString("C") %>
                                                <a href="Cart.aspx?action=remove&productId=<%= item.ProductId %>"
                                                    class="btn btn-sm btn-outline-danger ms-3"><i
                                                        class="fas fa-trash"></i></a>
                                        </td>
                                    </tr>
                                    <% } %>
                            </tbody>
                        </table>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mt-4 pt-3 border-top">
                        <div class="h4 mb-0 fw-bold text-secondary">ยอดรวมสุทธิ</div>
                        <div class="h3 mb-0 fw-bold text-primary">
                            <%= GetTotal().ToString("C") %>
                        </div>
                    </div>
                </div>

                <div class="text-end mt-4">
                    <a href="Checkout.aspx" class="btn btn-primary btn-lg px-5 py-3 fw-bold shadow-sm">ดำเนินการชำระเงิน
                        <i class="fas fa-arrow-right ms-2"></i></a>
                </div>

                <script>
                    function updateQty(offerId, newQty) {
                        if (newQty < 1) {
                            window.location.href = 'Cart.aspx?action=remove&productId=' + offerId;
                            return;
                        }
                        window.location.href = 'Cart.aspx?action=update&productId=' + offerId + '&qty=' + newQty;
                    }
                </script>
                <% } else { %>
                    <div class="text-center py-5">
                        <i class="fas fa-shopping-basket fa-3x text-muted mb-3"></i>
                        <h3>ตะกร้าของคุณยังว่างอยู่</h3>
                        <a href="Products.aspx" class="btn btn-outline-primary mt-3">เลือกซื้อสินค้า</a>
                    </div>
                    <% } %>
        </div>
    </asp:Content>