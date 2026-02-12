<%@ Page Title="ที่อยู่" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Profile_Addresses.aspx.cs" Inherits="Pages_Profile_Addresses" %>

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

            .address-card {
                border: 1px solid #eee;
                border-radius: 15px;
                padding: 20px;
                margin-bottom: 20px;
                transition: all 0.3s;
                position: relative;
                background: #fff;
            }

            .address-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 10px 25px rgba(0, 0, 0, 0.08);
                border-color: transparent;
            }

            .address-type-badge {
                font-size: 0.75rem;
                padding: 4px 10px;
                border-radius: 20px;
                background-color: #e3f2fd;
                color: #0d6efd;
                font-weight: 600;
            }

            .address-name {
                font-weight: 700;
                color: #333;
                margin-bottom: 5px;
            }

            .address-details {
                color: #666;
                line-height: 1.6;
            }

            .btn-add-address {
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

            .btn-add-address:hover {
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
                        <a href="Profile_Banks.aspx" class="profile-menu-item">
                            <i class="fas fa-university"></i>ธนาคารและบัตร
                        </a>
                        <a href="Profile_Addresses.aspx" class="profile-menu-item active">
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
                            <div class="form-section-title">ที่อยู่ของฉัน</div>
                            <div class="form-section-subtitle mb-0">
                                จัดการที่อยู่สำหรับการจัดส่งสินค้า
                            </div>
                        </div>
                        <button type="button" class="btn btn-primary btn-sm rounded-pill px-3" data-bs-toggle="modal"
                            data-bs-target="#addAddressModal">
                            <i class="fas fa-plus me-1"></i> เพิ่มที่อยู่
                        </button>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <asp:Repeater ID="rptAddresses" runat="server" OnItemCommand="rptAddresses_ItemCommand">
                                <ItemTemplate>
                                    <!-- Address Card -->
                                    <div class="address-card">
                                        <div class="d-flex justify-content-between">
                                            <div>
                                                <div class="address-name">
                                                    <%# Eval("FullName") %> <span class="ms-2 text-muted fw-normal">|
                                                            <%# Eval("PhoneNumber") %></span>
                                                        <span class="badge status-badge bg-primary ms-2"
                                                            style="font-size: 0.7rem;"
                                                            visible='<%# Convert.ToBoolean(Eval("IsDefault")) %>'>ค่าเริ่มต้น</span>
                                                </div>
                                                <div class="address-details mt-2">
                                                    <%# Eval("AddressLine") %><br>
                                                        <%# Eval("Province") %>
                                                            <%# Eval("PostalCode") %>
                                                </div>
                                            </div>
                                            <div class="text-end">
                                                <div class="dropdown">
                                                    <button class="btn btn-light btn-sm rounded-circle" type="button"
                                                        data-bs-toggle="dropdown">
                                                        <i class="fas fa-ellipsis-v"></i>
                                                    </button>
                                                    <ul class="dropdown-menu dropdown-menu-end border-0 shadow">
                                                        <li>
                                                            <asp:LinkButton ID="btnSetDefault" runat="server"
                                                                CommandName="SetDefault"
                                                                CommandArgument='<%# Eval("Id") %>'
                                                                CssClass="dropdown-item">ตั้งเป็นค่าเริ่มต้น
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="btnDelete" runat="server"
                                                                CommandName="Delete" CommandArgument='<%# Eval("Id") %>'
                                                                CssClass="dropdown-item text-danger"
                                                                OnClientClick="return confirm('คุณต้องการลบที่อยู่นี้ใช่หรือไม่?');">
                                                                ลบ</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Label ID="lblEmpty" runat="server" Visible="false">
                                <div class="text-center py-4 text-muted">
                                    <i class="fas fa-map-marked-alt fa-3x mb-3 opacity-50"></i>
                                    <p>ยังไม่มีข้อมูลที่อยู่จัดส่ง</p>
                                </div>
                            </asp:Label>

                            <!-- Add New Button -->
                            <div class="btn-add-address" data-bs-toggle="modal" data-bs-target="#addAddressModal">
                                <div class="text-center">
                                    <i class="fas fa-plus-circle fa-2x mb-2 d-block"></i>
                                    <span>เพิ่มที่อยู่ใหม่</span>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Add Address Modal -->
        <div class="modal fade" id="addAddressModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content border-0 shadow-lg" style="border-radius: 20px;">
                    <div class="modal-header border-0 pb-0">
                        <h5 class="modal-title fw-bold">เพิ่มที่อยู่ใหม่</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body p-4">
                        <asp:Label ID="lblModalMsg" runat="server" CssClass="d-block mb-3"></asp:Label>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label text-muted small fw-bold">ชื่อ-นามสกุล</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control modern-input"
                                    placeholder="ชื่อผู้รับ"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label text-muted small fw-bold">เบอร์โทรศัพท์</label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control modern-input"
                                    placeholder="08X-XXX-XXXX"></asp:TextBox>
                            </div>
                        </div>

                        <div class="mb-3">
                            <label class="form-label text-muted small fw-bold">ที่อยู่ (บ้านเลขที่, ซอย, ถนน)</label>
                            <asp:TextBox ID="txtAddressLine" runat="server" CssClass="form-control modern-input"
                                TextMode="MultiLine" Rows="2" placeholder="รายละเอียดที่อยู่..."></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label text-muted small fw-bold">จังหวัด / เขต</label>
                                <asp:TextBox ID="txtProvince" runat="server" CssClass="form-control modern-input"
                                    placeholder="เช่น กรุงเทพมหานคร"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label text-muted small fw-bold">รหัสไปรษณีย์</label>
                                <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control modern-input"
                                    placeholder="xxxxx"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-check">
                            <asp:CheckBox ID="chkIsDefaultAddress" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="chkIsDefaultAddress">ตั้งเป็นที่อยู่เริ่มต้น</label>
                        </div>

                        <div class="mt-4 d-grid">
                            <asp:Button ID="btnSaveAddress" runat="server" Text="บันทึกข้อมูล"
                                CssClass="btn btn-primary py-2 rounded-pill fw-bold" OnClick="btnSaveAddress_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>