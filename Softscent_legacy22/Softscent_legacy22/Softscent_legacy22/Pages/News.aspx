<%@ Page Title="News & Updates" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="News.aspx.cs" Inherits="Pages_News" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
            .news-card {
                border: none;
                border-radius: 12px;
                overflow: hidden;
                transition: all 0.3s ease;
                height: 100%;
                background: rgba(255, 255, 255, 0.9);
                box-shadow: 0 4px 10px rgba(0, 0, 0, 0.03);
                display: flex;
                flex-direction: column;
            }

            .news-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
            }

            .news-img {
                height: 220px;
                object-fit: cover;
                width: 100%;
            }

            .news-date {
                font-size: 0.85rem;
                color: #888;
                margin-bottom: 0.5rem;
            }

            .news-title {
                font-size: 1.25rem;
                font-weight: 600;
                color: #2c3e50;
                margin-bottom: 0.75rem;
                line-height: 1.4;
            }

            .news-excerpt {
                color: #666;
                font-size: 0.95rem;
                margin-bottom: 1.5rem;
                flex-grow: 1;
            }

            .content-box {
                padding: 1.5rem;
                display: flex;
                flex-direction: column;
                flex-grow: 1;
            }

            .read-more-btn {
                color: var(--primary);
                text-decoration: none;
                font-weight: 500;
                display: inline-flex;
                align-items: center;
                margin-top: auto;
            }

            .read-more-btn:hover {
                color: var(--primary-dark);
            }

            .read-more-btn i {
                margin-left: 5px;
                transition: margin-left 0.2s;
            }

            .read-more-btn:hover i {
                margin-left: 8px;
            }

            /* Modal Styles */
            .modal-glass {
                background: rgba(255, 255, 255, 0.95);
                backdrop-filter: blur(10px);
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-4">
            <div class="d-flex justify-content-between align-items-center mb-5 fade-in-up">
                <div class="text-start">
                    <h1 class="display-5 fw-bold mb-2" style="color: var(--secondary);">News & Updates</h1>
                    <p class="lead text-muted" style="max-width: 600px;">
                        ติดตามข่าวสาร กิจกรรม และเคล็ดลับการดูแลสุขภาพด้วยกลิ่นบำบัดจาก Softscent
                    </p>
                </div>
                <% if (Session["Role"] !=null && Session["Role"].ToString()=="Admin" ) { %>
                    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#addNewsModal">
                        <i class="fas fa-plus me-2"></i>เพิ่มข่าวสาร
                    </button>
                    <% } %>
            </div>

            <% if (NewsList.Count> 0) { %>
                <div class="row g-4">
                    <% foreach (var item in NewsList) { %>
                        <div class="col-md-6 col-lg-4">
                            <article class="news-card">
                                <img src="<%= GetImageUrl(item.ImageUrl) %>" alt="<%= item.Title %>" class="news-img" />
                                <div class="content-box">
                                    <div class="news-date"><i class="far fa-calendar-alt me-2"></i>
                                        <%= item.PublishedDate.ToString("dd MMM yyyy") %>
                                    </div>
                                    <h3 class="news-title">
                                        <%= item.Title %>
                                    </h3>
                                    <p class="news-excerpt">
                                        <%= item.Content.Length> 120 ? item.Content.Substring(0, 117) + "..." :
                                            item.Content %>
                                    </p>
                                    <a href="NewsDetails.aspx?id=<%= item.Id %>" class="read-more-btn">อ่านต่อ <i
                                            class="fas fa-arrow-right"></i></a>
                                </div>
                            </article>
                        </div>
                        <% } %>
                </div>
                <% } else { %>
                    <div class="text-center py-5">
                        <p class="text-muted">ยังไม่มีข่าวสารในขณะนี้</p>
                    </div>
                    <% } %>
        </div>

        <!-- Admin Add News Modal -->
        <div class="modal fade" id="addNewsModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content modal-glass">
                    <div class="modal-header border-0">
                        <h5 class="modal-title fw-bold">เพิ่มข่าวสารใหม่</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="mb-3">
                            <label class="form-label">หัวข้อข่าว</label>
                            <asp:TextBox ID="txtNewTitle" runat="server" CssClass="form-control"
                                placeholder="ใส่หัวข้อข่าว..."></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">รูปภาพ URL (Optional)</label>
                            <asp:TextBox ID="txtNewImage" runat="server" CssClass="form-control"
                                placeholder="https://..."></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">เนื้อหา</label>
                            <asp:TextBox ID="txtNewContent" runat="server" TextMode="MultiLine" Rows="5"
                                CssClass="form-control" placeholder="รายละเอียดเนื้อหา..."></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer border-0">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">ยกเลิก</button>
                        <asp:Button ID="btnAddNews" runat="server" Text="บันทึกข่าวสาร" OnClick="btnAddNews_Click"
                            CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>