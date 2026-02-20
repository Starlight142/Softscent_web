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
                    <div class="d-flex">
                        <input type="text" id="searchInput" class="form-control me-2"
                            placeholder="ค้นหาสินค้า (เช่น ลาเวนเดอร์)..." oninput="searchProducts()">
                        <button type="button" class="btn btn-primary" onclick="searchProducts()"><i
                                class="fas fa-search"></i></button>
                    </div>
                </div>
            </div>
            <div class="row" id="productList">
                <% foreach (var item in ProductList) { %>
                    <div class="col-md-4 mb-4 product-item"
                        data-search-text="<%= (item.Name + " " + item.Description).ToLower() %>">
                        <div class="card h-100">
                            <img src="/<%= item.ImageUrl ?? " https://placehold.co/400x300?text=Product" %>"
                            class="card-img-top" alt="<%= item.Name %>" style="height: 200px; object-fit: cover;">
                                <div class="card-body d-flex flex-column">
                                    <h5 class="card-title">
                                        <%= item.Name %>
                                    </h5>
                                    <p class="card-text text-muted">
                                        <%= item.Description %>
                                    </p>
                                    <div class="mt-auto d-flex justify-content-between align-items-center">
                                        <span class="h5 mb-0 text-primary">
                                            <%= item.Price.ToString("C") %>
                                        </span>
                                        <% if (item.StockQuantity> 0) { %>
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
                                                        <% } else { %>
                                                            <button class="btn btn-secondary"
                                                                disabled>สินค้าหมด</button>
                                                            <% } %>
                                    </div>
                                </div>
                        </div>
                    </div>
                    <% } %>

                        <div id="noResults" class="col-12 text-center" style="display: none;">
                            <p class="text-muted">ไม่พบสินค้าที่ค้นหา</p>
                        </div>

                        <% if (ProductList.Count==0) { %>
                            <div class="col-12 text-center">
                                <p class="text-muted">ขณะนี้ยังไม่มีสินค้าพร้อมจำหน่าย โปรดกลับมาตรวจสอบใหม่ในภายหลัง!
                                </p>
                            </div>
                            <% } %>
            </div>

            <script>
                function searchProducts() {
                    const input = document.getElementById('searchInput');
                    const filter = input.value.toLowerCase().trim();
                    const productList = document.getElementById('productList');
                    const products = productList.getElementsByClassName('product-item');
                    let visibleCount = 0;

                    for (let i = 0; i < products.length; i++) {
                        const searchText = products[i].getAttribute('data-search-text');
                        if (searchText.indexOf(filter) > -1) {
                            products[i].style.display = "";
                            visibleCount++;
                        } else {
                            products[i].style.display = "none";
                        }
                    }

                    const noResults = document.getElementById('noResults');
                    if (visibleCount === 0 && products.length > 0) {
                        noResults.style.display = "block";
                    } else {
                        noResults.style.display = "none";
                    }
                }
            </script>
        </div>
    </asp:Content>