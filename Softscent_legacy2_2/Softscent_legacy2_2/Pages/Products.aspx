<%@ Page Title="คอลเลกชันของเรา" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Products.aspx.cs" Inherits="Pages_Products" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <div class="row mb-5 align-items-center">
                <div class="col-md-6">
                    <h1 class="text-start mb-0">คอลเลกชันสินค้าของเรา</h1>
                </div>
                <div class="col-md-6">
                    <form method="get" action="Products.aspx" class="d-flex">
                        <input type="text" name="q" class="form-control me-2"
                            placeholder="ค้นหาสินค้า (เช่น ลาเวนเดอร์)..." value="<%= Request.QueryString[" q"] %>">
                        <button type="submit" class="btn btn-primary"><i class="fas fa-search"></i></button>
                    </form>
                </div>
            </div>
            <div class="row">
                <% foreach (var item in ProductList) { %>
                    <div class="col-md-4 mb-4">
                        <div class="card h-100">
                            <img src="/<%= item.ImageUrl ?? " https://placehold.co/400x300?text=Product" %>"
                            class="card-img-top" alt="<%= item.Name %>" style="height: 200px; object-fit: cover;">
                                <div class="card-body d-flex flex-column">
                                    <h5 class="card-title">
                                        <%= GetProductThaiName(item.Name) %>
                                    </h5>
                                    <p class="card-text text-muted">
                                        <%= GetProductThaiDescription(item.Name, item.Description) %>
                                    </p>
                                    <div class="mt-auto d-flex justify-content-between align-items-center">
                                        <span class="h5 mb-0 text-primary">
                                            <%= item.Price.ToString("C") %>
                                        </span>
                                        <% if (item.IsCustomizable) { %>
                                            <a href="Custom.aspx?productId=<%= item.Id %>"
                                                class="btn btn-outline-primary">
                                                <i class="fas fa-magic me-2"></i> ปรุงสูตรเอง
                                            </a>
                                            <% } else { %>
                                                <a href="Cart.aspx?action=add&productId=<%= item.Id %>"
                                                    class="btn btn-primary">
                                                    <i class="fas fa-shopping-cart me-2"></i> เพิ่มลงตะกร้า
                                                </a>
                                                <% } %>
                                    </div>
                                </div>
                        </div>
                    </div>
                    <% } %>

                        <% if (ProductList.Count==0) { %>
                            <div class="col-12 text-center">
                                <p class="text-muted">ขณะนี้ยังไม่มีสินค้าพร้อมจำหน่าย โปรดกลับมาตรวจสอบใหม่ในภายหลัง!
                                </p>
                            </div>
                            <% } %>
            </div>
        </div>
    </asp:Content>