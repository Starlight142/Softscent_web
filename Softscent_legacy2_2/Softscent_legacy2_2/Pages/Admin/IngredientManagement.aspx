<%@ Page Title="Ingredient Management" Language="C#" MasterPageFile="~/MasterPageCMS.master" AutoEventWireup="true"
    CodeFile="IngredientManagement.aspx.cs" Inherits="Pages_Admin_IngredientManagement" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
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
            <h1 class="h2">จัดการส่วนผสม (Ingredients / Custom)</h1>
            <div class="btn-toolbar mb-2 mb-md-0">
                <button type="button" class="btn btn-success" onclick="openModal('New')">
                    <i class="fas fa-plus me-2"></i>เพิ่มส่วนผสมใหม่
                </button>
            </div>
        </div>

        <div class="card shadow-sm border-0">
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvIngredients" runat="server" CssClass="table table-hover align-middle"
                        AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvIngredients_RowCommand"
                        GridLines="None" OnRowDeleting="gvIngredients_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="#" />
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="StockQuantity" HeaderText="Stock" />
                            <asp:TemplateField HeaderText="Benefit / Description">
                                <ItemTemplate>
                                    <div class="text-muted small" style="max-width: 300px;">
                                        <%# Eval("Benefit") %>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditIngredient"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-outline-primary btn-sm action-btn me-1">
                                        <i class="fas fa-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" CommandName="Delete"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-outline-danger btn-sm action-btn"
                                        OnClientClick="return confirm('Are you sure you want to delete this ingredient?');">
                                        <i class="fas fa-trash"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center py-5">
                                <p class="text-muted">No ingredients found.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Ingredient Modal -->
        <div class="modal fade" id="ingredientModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalTitle">Ingredient Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfIngredientId" runat="server" />
                                <div class="mb-3">
                                    <label class="form-label">Name</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required="true"
                                        placeholder="e.g. Lavender"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Price (+)</label>
                                    <div class="input-group">
                                        <span class="input-group-text">฿</span>
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control"
                                            TextMode="Number" Step="0.01" required="true"></asp:TextBox>
                                    </div>
                                    <div class="form-text">Additional cost for this ingredient (usually 0 if included in
                                        base price).</div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Stock Quantity</label>
                                    <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number"
                                        required="true"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Benefit / Description</label>
                                    <asp:TextBox ID="txtBenefit" runat="server" CssClass="form-control"
                                        TextMode="MultiLine" Rows="3" placeholder="Explain the benefit in Thai">
                                    </asp:TextBox>
                                </div>

                                <div class="alert alert-danger mt-3 d-none" id="errorAlert" runat="server">
                                    Error saving ingredient.
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
                    document.getElementById('modalTitle').innerText = 'Add New Ingredient';
                    __doPostBack('<%= btnAddNew.UniqueID %>', '');
                } else {
                    document.getElementById('modalTitle').innerText = 'Edit Ingredient';
                }
                var myModal = new bootstrap.Modal(document.getElementById('ingredientModal'));
                myModal.show();
            }

            function showModal() {
                var myModal = new bootstrap.Modal(document.getElementById('ingredientModal'));
                myModal.show();
            }

            function hideModal() {
                var myModalEl = document.getElementById('ingredientModal');
                var modal = bootstrap.Modal.getInstance(myModalEl);
                if (modal) {
                    modal.hide();
                }
                $('.modal-backdrop').remove();
            }
        </script>

        <asp:Button ID="btnAddNew" runat="server" style="display:none" OnClick="btnAddNew_Click" />

    </asp:Content>