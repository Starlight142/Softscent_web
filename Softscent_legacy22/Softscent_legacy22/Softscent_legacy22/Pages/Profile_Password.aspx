<%@ Page Title="เปลี่ยนรหัสผ่าน" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Profile_Password.aspx.cs" Inherits="Pages_Profile_Password" %>

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

            .form-label-custom {
                color: #777;
                font-size: 0.85rem;
                font-weight: 600;
                text-transform: uppercase;
                letter-spacing: 0.5px;
                margin-bottom: 8px;
            }

            .modern-input {
                border: 2px solid #f0f0f0;
                border-radius: 12px;
                padding: 12px 15px;
                transition: all 0.3s;
                background: #fcfcfc;
            }

            .modern-input:focus {
                border-color: var(--bs-primary);
                background: #fff;
                box-shadow: 0 0 0 4px rgba(var(--bs-primary-rgb), 0.1);
            }

            .btn-save-profile {
                background: linear-gradient(45deg, var(--bs-primary), #4facfe);
                border: none;
                border-radius: 12px;
                padding: 12px 40px;
                font-weight: 700;
                color: #fff;
                box-shadow: 0 4px 15px rgba(var(--bs-primary-rgb), 0.3);
                transition: all 0.3s;
            }

            .btn-save-profile:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(var(--bs-primary-rgb), 0.4);
                color: #fff;
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
                        <a href="Profile_Addresses.aspx" class="profile-menu-item">
                            <i class="fas fa-map-marker-alt"></i>ที่อยู่
                        </a>
                        <a href="Profile_Password.aspx" class="profile-menu-item active">
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
                    <div class="form-section-title">เปลี่ยนรหัสผ่าน</div>
                    <div class="form-section-subtitle">
                        เพื่อความปลอดภัยของบัญชีของคุณ กรุณาใชรหัสผ่านที่คาดเดายาก
                    </div>

                    <div class="row mt-4">
                        <div class="col-lg-8">
                            <div class="mb-4">
                                <label class="form-label-custom">รหัสผ่านปัจจุบัน</label>
                                <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control modern-input"
                                    TextMode="Password" placeholder="ระบุรหัสผ่านปัจจุบัน"></asp:TextBox>
                            </div>

                            <div class="mb-4">
                                <label class="form-label-custom">รหัสผ่านใหม่</label>
                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control modern-input"
                                    TextMode="Password" placeholder="ระบุรหัสผ่านใหม่"></asp:TextBox>
                            </div>

                            <div class="mb-5">
                                <label class="form-label-custom">ยืนยันรหัสผ่านใหม่</label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control modern-input"
                                    TextMode="Password" placeholder="ระบุรหัสผ่านใหม่อีกครั้ง"></asp:TextBox>
                            </div>

                            <div class="pt-3 border-top">
                                <asp:Button ID="btnChangePassword" runat="server" Text="ยืนยันการเปลี่ยนรหัสผ่าน"
                                    CssClass="btn btn-save-profile" OnClick="btnChangePassword_Click" />
                                <div class="mt-3">
                                    <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>