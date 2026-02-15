<%@ Page Title="Product Management" Language="C#" MasterPageFile="~/MasterPageCMS.master" AutoEventWireup="true"
    CodeFile="ProductManagement.aspx.cs" Inherits="Pages_Admin_ProductManagement" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
            .product-img {
                width: 50px;
                height: 50px;
                object-fit: cover;
                border-radius: 8px;
            }

            .action-btn {
                width: 32px;
                height: 32px;
                padding: 0;
                display: inline-flex;
                align-items: center;
                justify-content: center;
                border-radius: 50%;
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div
            class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h1 class="h2">จัดการสินค้า (Product Management)</h1>
            <div class="btn-toolbar mb-2 mb-md-0">
                <button type="button" class="btn btn-primary" onclick="openModal('New')">
                    <i class="fas fa-plus me-2"></i>เพิ่มสินค้าใหม่
                </button>
            </div>
        </div>

        <div class="card shadow-sm border-0">
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvProducts" runat="server" CssClass="table table-hover align-middle"
                        AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvProducts_RowCommand"
                        GridLines="None" OnRowDeleting="gvProducts_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="#" />
                            <asp:TemplateField HeaderText="Image">
                                <ItemTemplate>
                                    <img src='<%# GetImageUrl(Eval("ImageUrl")) %>' class="product-img"
                                        onerror="this.src='https://placehold.co/50x50'" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="NameThai" HeaderText="Name (TH)" />
                            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="StockQuantity" HeaderText="Stock" />
                            <asp:TemplateField HeaderText="Customizable">
                                <ItemTemplate>
                                    <span
                                        class='badge bg-<%# Convert.ToBoolean(Eval("IsCustomizable")) ? "success" : "secondary" %>'>
                                        <%# Convert.ToBoolean(Eval("IsCustomizable")) ? "Yes" : "No" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditProduct"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-outline-primary btn-sm action-btn me-1">
                                        <i class="fas fa-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" CommandName="Delete"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-outline-danger btn-sm action-btn"
                                        OnClientClick="return confirm('Are you sure you want to delete this product?');">
                                        <i class="fas fa-trash"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center py-5">
                                <p class="text-muted">No products found.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Product Modal -->
        <div class="modal fade" id="productModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalTitle">Product Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfProductId" runat="server" />
                                <div class="row g-3">
                                    <div class="col-md-6">
                                        <label class="form-label">Name (English)</label>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"
                                            required="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label">Name (Thai)</label>
                                        <asp:TextBox ID="txtNameThai" runat="server" CssClass="form-control">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        <label class="form-label">Description (English)</label>
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                    <div class="col-md-12">
                                        <label class="form-label">Description (Thai)</label>
                                        <asp:TextBox ID="txtDescThai" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label">Price</label>
                                        <div class="input-group">
                                            <span class="input-group-text">฿</span>
                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control"
                                                TextMode="Number" Step="0.01" required="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label">Stock Quantity</label>
                                        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control"
                                            TextMode="Number" required="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label">Image URL</label>
                                        <asp:TextBox ID="txtImageUrl" runat="server" CssClass="form-control">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-12">
                                        <div class="form-check">
                                            <asp:CheckBox ID="chkCustomizable" runat="server"
                                                CssClass="form-check-input" />
                                            <label class="form-check-label" for="<%= chkCustomizable.ClientID %>">
                                                Allow Customization (Ingredients)
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="alert alert-danger mt-3 d-none" id="errorAlert" runat="server">
                                    Error saving product.
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-primary"
                            OnClick="btnSave_Click" />
                    </div>
                </div>
            </div>
        </div>

        <script>
            function openModal(mode) {
                if (mode === 'New') {
                    document.getElementById('modalTitle').innerText = 'Add New Product';
                    // Clear fields logic if needed or handle in code behind on "New" button click (server side)
                    // But here we used a client side button for New. ideally should be server side to clear fields.
                    // Converting "New" button to server side to make it easier to clear fields.
                    __doPostBack('<%= btnAddNew.UniqueID %>', '');
                } else {
                    document.getElementById('modalTitle').innerText = 'Edit Product';
                }
                var myModal = new bootstrap.Modal(document.getElementById('productModal'));
                myModal.show();
            }

            // Helper to show modal from server side
            function showModal() {
                var myModal = new bootstrap.Modal(document.getElementById('productModal'));
                myModal.show();
            }

            function hideModal() {
                var myModalEl = document.getElementById('productModal');
                var modal = bootstrap.Modal.getInstance(myModalEl);
                if (modal) {
                    modal.hide();
                }
                // Also remove backdrop manually if needed
                $('.modal-backdrop').remove();
            }
        </script>

        <!-- Hidden button for Add New to simplify clearing fields server-side -->
        <asp:Button ID="btnAddNew" runat="server" style="display:none" OnClick="btnAddNew_Click" />

    </asp:Content>