<%@ Page Title="ธนาคารและบัตร" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Profile_Banks.aspx.cs" Inherits="Pages_Profile_Banks" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
            .profile-container {
                background: linear-gradient(135deg, #fdfbfb 0%, #ebedee 100%);
                border-radius: 20px;
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.05);
                overflow: hidden;
            }

            .profile-sidebar {
                background: rgba(255, 255, 255, 0.7);
                backdrop-filter: blur(10px);
                border-right: 1px solid rgba(0, 0, 0, 0.05);
                padding: 30px;
            }

            .profile-content {
                background: #fff;
                padding: 40px;
            }

            .profile-menu-item {
                display: flex;
                align-items: center;
                padding: 12px 15px;
                color: #555;
                text-decoration: none;
                border-radius: 10px;
                margin-bottom: 5px;
                transition: all 0.3s ease;
                font-weight: 500;
            }

            .profile-menu-item i {
                width: 25px;
                margin-right: 10px;
                font-size: 1.1rem;
            }

            .profile-menu-item:hover {
                background: rgba(var(--bs-primary-rgb), 0.1);
                color: var(--bs-primary);
                transform: translateX(5px);
            }

            .profile-menu-item.active {
                background: var(--bs-primary);
                color: #fff;
                box-shadow: 0 4px 15px rgba(var(--bs-primary-rgb), 0.3);
            }

            .form-section-title {
                font-weight: 700;
                font-size: 1.25rem;
                margin-bottom: 5px;
                color: #333;
            }

            .form-section-subtitle {
                color: #888;
                font-size: 0.9rem;
                margin-bottom: 30px;
            }

            .bank-card {
                border: 1px solid #eee;
                border-radius: 15px;
                padding: 20px;
                margin-bottom: 20px;
                transition: all 0.3s;
                position: relative;
                background: linear-gradient(135deg, #ffffff 0%, #f9f9f9 100%);
            }

            .bank-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 10px 25px rgba(0, 0, 0, 0.08);
                border-color: transparent;
            }

            .bank-logo {
                width: 50px;
                height: 50px;
                border-radius: 10px;
                object-fit: contain;
                padding: 5px;
                background: #fff;
                box-shadow: 0 2px 5px rgba(0, 0, 0, 0.05);
            }

            .btn-add-card {
                border: 2px dashed #ddd;
                border-radius: 15px;
                display: flex;
                align-items: center;
                justify-content: center;
                height: 100px;
                width: 100%;
                background: #fafafa;
                color: #888;
                transition: all 0.3s;
                cursor: pointer;
            }

            .btn-add-card:hover {
                border-color: var(--bs-primary);
                color: var(--bs-primary);
                background: rgba(var(--bs-primary-rgb), 0.02);
            }

            .avatar-section {
                text-align: center;
                margin-bottom: 30px;
            }

            .avatar-img {
                width: 60px;
                height: 60px;
                border-radius: 50%;
                object-fit: cover;
                border: 3px solid #fff;
                box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
            }

            .bank-details {
                display: flex;
                align-items: center;
                gap: 15px;
            }

            .bank-name {
                font-weight: 700;
                color: #333;
            }

            .account-number {
                font-family: monospace;
                font-size: 1.1rem;
                color: #555;
                letter-spacing: 1px;
            }

            .badge-default {
                background-color: #e3f2fd;
                color: #0d6efd;
                font-size: 0.75rem;
                padding: 5px 10px;
                border-radius: 20px;
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <div class="profile-container row g-0">
                <!-- Sidebar -->
                <div class="col-md-3 profile-sidebar">
                    <div class="avatar-section">
                        <img src="/Images/default-avatar.png" onerror="this.src='https://via.placeholder.com/60'"
                            class="avatar-img mb-3" alt="Avatar">
                        <div class="fw-bold h5 mb-0">
                            <asp:Label ID="lblSidebarName" runat="server"></asp:Label>
                        </div>
                        <small class="text-muted"><i
                                class="fas fa-certificate text-primary me-1"></i>บัญชีที่ยืนยันแล้ว</small>
                    </div>

                    <div class="mt-4">
                        <div class="text-muted small fw-bold mb-3 ps-2 text-uppercase letter-spacing-1">บัญชีของฉัน
                        </div>
                        <a href="Profile.aspx" class="profile-menu-item">
                            <i class="fas fa-user-circle"></i>ข้อมูลส่วนตัว
                        </a>
                        <a href="Profile_Banks.aspx" class="profile-menu-item active">
                            <i class="fas fa-university"></i>ธนาคารและบัตร
                        </a>
                        <a href="Profile_Addresses.aspx" class="profile-menu-item">
                            <i class="fas fa-map-marker-alt"></i>ที่อยู่
                        </a>
                        <a href="Profile_Password.aspx" class="profile-menu-item">
                            <i class="fas fa-shield-alt"></i>เปลี่ยนรหัสผ่าน
                        </a>

                        <div class="text-muted small fw-bold mt-4 mb-3 ps-2 text-uppercase letter-spacing-1">
                            การซื้อของฉัน</div>
                        <a href="Orders.aspx" class="profile-menu-item">
                            <i class="fas fa-shopping-bag"></i>ประวัติการสั่งซื้อ
                        </a>
                    </div>
                </div>

                <!-- Main Content -->
                <div class="col-md-9 profile-content">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <div>
                            <div class="form-section-title">บัญชีธนาคารและบัตร</div>
                            <div class="form-section-subtitle mb-0">
                                จัดการช่องทางการชำระเงินของคุณ
                            </div>
                        </div>
                        <button type="button" class="btn btn-primary btn-sm rounded-pill px-3" data-bs-toggle="modal"
                            data-bs-target="#addBankModal">
                            <i class="fas fa-plus me-1"></i> เพิ่มบัญชี
                        </button>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <asp:Repeater ID="rptBanks" runat="server" OnItemCommand="rptBanks_ItemCommand">
                                <ItemTemplate>
                                    <!-- Bank Card -->
                                    <div class="bank-card">
                                        <div class="d-flex justify-content-between align-items-center">
                                            <div class="bank-details">
                                                <div
                                                    class="bank-logo d-flex align-items-center justify-content-center text-primary h3 mb-0">
                                                    <i class="fas fa-university"></i>
                                                </div>
                                                <div>
                                                    <div class="bank-name">
                                                        <%# Eval("BankName") %>
                                                            <span class="badge-default ms-2"
                                                                visible='<%# Convert.ToBoolean(Eval("IsDefault")) %>'>เริ่มต้น</span>
                                                    </div>
                                                    <div class="account-number">
                                                        <%# Eval("AccountNumber") %>
                                                    </div>
                                                    <div class="small text-muted">
                                                        <%# Eval("AccountName") %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="dropdown">
                                                <button class="btn btn-light btn-sm rounded-circle" type="button"
                                                    data-bs-toggle="dropdown">
                                                    <i class="fas fa-ellipsis-v"></i>
                                                </button>
                                                <ul class="dropdown-menu dropdown-menu-end border-0 shadow">
                                                    <li>
                                                        <asp:LinkButton ID="btnSetDefault" runat="server"
                                                            CommandName="SetDefault" CommandArgument='<%# Eval("Id") %>'
                                                            CssClass="dropdown-item">ตั้งเป็นค่าเริ่มต้น
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnDelete" runat="server"
                                                            CommandName="Delete" CommandArgument='<%# Eval("Id") %>'
                                                            CssClass="dropdown-item text-danger"
                                                            OnClientClick="return confirm('คุณต้องการลบบัญชีนี้ใช่หรือไม่?');">
                                                            ลบ</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Label ID="lblEmpty" runat="server" Visible="false">
                                <div class="text-center py-4 text-muted">
                                    <i class="fas fa-wallet fa-3x mb-3 opacity-50"></i>
                                    <p>ยังไม่มีบัญชีธนาคารหรือบัตรเครดิต</p>
                                </div>
                            </asp:Label>

                            <!-- Add New Button -->
                            <div class="btn-add-card" data-bs-toggle="modal" data-bs-target="#addBankModal">
                                <div class="text-center">
                                    <i class="fas fa-plus-circle fa-2x mb-2 d-block"></i>
                                    <span>เพิ่มบัญชีธนาคาร / บัตรเครดิต</span>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Add Bank Modal -->
        <div class="modal fade" id="addBankModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content border-0 shadow-lg" style="border-radius: 20px;">
                    <div class="modal-header border-0 pb-0">
                        <h5 class="modal-title fw-bold">เพิ่มบัญชีธนาคาร</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body p-4">
                        <asp:Label ID="lblModalMsg" runat="server" CssClass="d-block mb-3"></asp:Label>

                        <div class="mb-3">
                            <label class="form-label text-muted small fw-bold">ชื่อธนาคาร</label>
                            <asp:DropDownList ID="ddlBankName" runat="server" CssClass="form-select modern-input">
                                <asp:ListItem Value="กสิกรไทย">กสิกรไทย (KBANK)</asp:ListItem>
                                <asp:ListItem Value="ไทยพาณิชย์">ไทยพาณิชย์ (SCB)</asp:ListItem>
                                <asp:ListItem Value="กรุงเทพ">กรุงเทพ (BBL)</asp:ListItem>
                                <asp:ListItem Value="กรุงไทย">กรุงไทย (KTB)</asp:ListItem>
                                <asp:ListItem Value="กรุงศรีอยุธยา">กรุงศรีอยุธยา (BAY)</asp:ListItem>
                                <asp:ListItem Value="ทหารไทยธนชาต">ทหารไทยธนชาต (TTB)</asp:ListItem>
                                <asp:ListItem Value="ออมสิน">ออมสิน (GSB)</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label class="form-label text-muted small fw-bold">ชื่อบัญชี</label>
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control modern-input"
                                placeholder="ระบุชื่อเจ้าของบัญชี"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label class="form-label text-muted small fw-bold">เลขที่บัญชี</label>
                            <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control modern-input"
                                placeholder="XXX-X-XXXXX-X"></asp:TextBox>
                        </div>

                        <div class="form-check">
                            <asp:CheckBox ID="chkIsDefault" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="chkIsDefault">ตั้งเป็นบัญชีหลัก</label>
                        </div>

                        <div class="mt-4 d-grid">
                            <asp:Button ID="btnSaveBank" runat="server" Text="บันทึกข้อมูล"
                                CssClass="btn btn-primary py-2 rounded-pill fw-bold" OnClick="btnSaveBank_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>