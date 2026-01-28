<%@ Page Title="สมัครสมาชิก" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Register.aspx.cs" Inherits="Pages_Register" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card p-4">
                        <h2 class="text-center mb-4">สมัครสมาชิก</h2>
                        <!-- form handled by Master Page -->
                        <div class="mb-3">
                            <label class="form-label">ชื่อ-นามสกุล</label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"
                                placeholder="สมชาย ใจดี">
                            </asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">อีเมล</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                placeholder="example@email.com"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">รหัสผ่าน</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"
                                placeholder="อย่างน้อย 6 ตัวอักษร"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">ยืนยันรหัสผ่าน</label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control"
                                TextMode="Password" placeholder="กรอกรหัสผ่านอีกครั้ง"></asp:TextBox>
                        </div>
                        <div class="d-grid">
                            <asp:Button ID="btnRegister" runat="server" Text="ยืนยันการสมัครสมาชิก"
                                CssClass="btn btn-success" OnClick="btnRegister_Click" />
                        </div>
                        <div class="mt-3 text-center">
                            <p>มีบัญชีอยู่แล้ว? <a href="Login.aspx">เข้าสู่ระบบที่นี่</a></p>
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                        </div>
                        <!-- /form -->
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>