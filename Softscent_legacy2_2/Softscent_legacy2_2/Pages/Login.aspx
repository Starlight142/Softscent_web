<%@ Page Title="เข้าสู่ระบบ" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Pages_Login" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card p-4">
                        <h2 class="text-center mb-4">เข้าสู่ระบบ</h2>
                        <!-- form handled by Master Page -->
                        <div class="mb-3">
                            <label class="form-label">อีเมล</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                placeholder="example@email.com"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">รหัสผ่าน</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"
                                placeholder="กรอกรหัสผ่านที่นี่"></asp:TextBox>
                        </div>
                        <div class="d-grid">
                            <asp:Button ID="btnLogin" runat="server" Text="เข้าสู่ระบบ" CssClass="btn btn-primary"
                                OnClick="btnLogin_Click" />
                        </div>
                        <div class="mt-3 text-center">
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                        </div>
                        <!-- /form -->
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>