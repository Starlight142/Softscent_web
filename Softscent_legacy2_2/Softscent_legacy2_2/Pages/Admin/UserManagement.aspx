<%@ Page Title="User Management" Language="C#" MasterPageFile="~/MasterPageCMS.master" AutoEventWireup="true"
    CodeFile="UserManagement.aspx.cs" Inherits="Pages_Admin_UserManagement" %>

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
            <h1 class="h2">จัดการผู้ใช้งาน (User Management)</h1>
            <div class="btn-toolbar mb-2 mb-md-0">
                <button type="button" class="btn btn-primary" onclick="openModal('New')">
                    <i class="fas fa-plus me-2"></i>เพิ่มผู้ใช้ใหม่
                </button>
            </div>
        </div>

        <div class="card shadow-sm border-0">
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvUsers" runat="server" CssClass="table table-hover align-middle"
                        AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvUsers_RowCommand" GridLines="None"
                        OnRowDeleting="gvUsers_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="#" />
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Username" HeaderText="Username" />
                            <asp:TemplateField HeaderText="Role">
                                <ItemTemplate>
                                    <span
                                        class='badge bg-<%# Eval("RoleName").ToString() == "Admin" ? "danger" : "info" %>'>
                                        <%# Eval("RoleName") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditUser"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-outline-primary btn-sm action-btn me-1">
                                        <i class="fas fa-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" CommandName="Delete"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-outline-danger btn-sm action-btn"
                                        OnClientClick="return confirm('Are you sure you want to delete this user?');">
                                        <i class="fas fa-trash"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center py-5">
                                <p class="text-muted">No users found.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- User Modal -->
        <div class="modal fade" id="userModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalTitle">User Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfUserId" runat="server" />
                                <div class="row g-3">
                                    <div class="col-12">
                                        <label class="form-label">Name</label>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"
                                            required="true">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-12">
                                        <label class="form-label">Email</label>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                            TextMode="Email" required="true"></asp:TextBox>
                                    </div>
                                    <div class="col-12">
                                        <label class="form-label">Username</label>
                                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-12">
                                        <label class="form-label">Password</label>
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                                            TextMode="Password"></asp:TextBox>
                                        <small class="text-muted" id="pwdHelp" runat="server">Leave blank to keep
                                            current password when editing.</small>
                                    </div>
                                    <div class="col-12">
                                        <div class="form-check">
                                            <asp:CheckBox ID="chkIsAdmin" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label" for="<%= chkIsAdmin.ClientID %>">
                                                Is Administrator
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="alert alert-danger mt-3 d-none" id="errorAlert" runat="server">
                                    Error saving user.
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
                    document.getElementById('modalTitle').innerText = 'Add New User';
                    __doPostBack('<%= btnAddNew.UniqueID %>', '');
                } else {
                    document.getElementById('modalTitle').innerText = 'Edit User';
                }
                var myModal = new bootstrap.Modal(document.getElementById('userModal'));
                myModal.show();
            }

            function showModal() {
                var myModal = new bootstrap.Modal(document.getElementById('userModal'));
                myModal.show();
            }

            function hideModal() {
                var myModalEl = document.getElementById('userModal');
                var modal = bootstrap.Modal.getInstance(myModalEl);
                if (modal) {
                    modal.hide();
                }
                $('.modal-backdrop').remove();
            }
        </script>

        <asp:Button ID="btnAddNew" runat="server" style="display:none" OnClick="btnAddNew_Click" />

    </asp:Content>