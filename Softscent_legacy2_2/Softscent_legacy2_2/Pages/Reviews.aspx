<%@ Page Title="Reviews" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Reviews.aspx.cs" Inherits="Pages_Reviews" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
        <style>
            .review-card {
                background: rgba(255, 255, 255, 0.95);
                border-radius: 15px;
                padding: 1.5rem;
                margin-bottom: 2rem;
                box-shadow: 0 5px 20px rgba(0, 0, 0, 0.05);
                transition: transform 0.3s ease;
                position: relative;
                border: 1px solid rgba(0, 0, 0, 0.02);
                height: 100%;
                /* For uniform height in some layouts, though masonry ignores this */
            }

            .review-card:hover {
                transform: translateY(-5px);
                box-shadow: 0 15px 30px rgba(0, 0, 0, 0.08);
            }

            .quote-icon {
                position: absolute;
                top: 15px;
                right: 20px;
                font-size: 2rem;
                color: rgba(39, 174, 96, 0.1);
            }

            .reviewer-avatar-placeholder {
                width: 50px;
                height: 50px;
                background: linear-gradient(135deg, #e0e0e0 0%, #f5f5f5 100%);
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                font-size: 1.2rem;
                color: #888;
                margin-right: 15px;
            }

            .rating {
                color: #f1c40f;
                font-size: 0.9rem;
                margin: 0.5rem 0;
            }

            .review-text {
                color: #555;
                font-style: italic;
                margin-bottom: 1.5rem;
                line-height: 1.6;
            }

            .review-masonry {
                column-count: 3;
                column-gap: 2rem;
            }

            @media (max-width: 992px) {
                .review-masonry {
                    column-count: 2;
                }
            }

            @media (max-width: 768px) {
                .review-masonry {
                    column-count: 1;
                }
            }

            .review-item {
                break-inside: avoid;
                margin-bottom: 2rem;
            }

            /* Review Form */
            .review-form-card {
                background: #fff;
                border-radius: 15px;
                padding: 2rem;
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
                margin-bottom: 3rem;
                border-left: 5px solid var(--primary);
            }
        </style>
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-4">
            <div class="text-center mb-5">
                <h1 class="display-5 fw-bold mb-3" style="color: var(--secondary);">Voices of Softscent</h1>
                <p class="lead text-muted mx-auto mb-4" style="max-width: 600px;">
                    ความคิดเห็นจริงจากลูกค้าที่ใช้งานจริง
                </p>
            </div>

            <!-- Write Review Section -->
            <div class="row justify-content-center">
                <div class="col-md-8">
                    <div class="review-form-card fade-in-up">
                        <% if (Session["User"] !=null) { %>
                            <h4 class="mb-3 fw-bold"><i class="fas fa-pen-fancy me-2"></i>เขียนรีวิวของคุณ</h4>
                            <div class="row g-3">
                                <div class="col-md-4">
                                    <label class="form-label">คะแนนความพึงพอใจ</label>
                                    <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="5" Selected="True">⭐⭐⭐⭐⭐ (ดีมาก)</asp:ListItem>
                                        <asp:ListItem Value="4">⭐⭐⭐⭐ (ดี)</asp:ListItem>
                                        <asp:ListItem Value="3">⭐⭐⭐ (ปานกลาง)</asp:ListItem>
                                        <asp:ListItem Value="2">⭐⭐ (พอใช้)</asp:ListItem>
                                        <asp:ListItem Value="1">⭐ (ควรปรับปรุง)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-12">
                                    <label class="form-label">ความคิดเห็น</label>
                                    <asp:TextBox ID="txtReviewComment" runat="server" TextMode="MultiLine" Rows="3"
                                        CssClass="form-control" placeholder="บอกเล่าความประทับใจของคุณ...">
                                    </asp:TextBox>
                                </div>
                                <div class="col-12 text-end">
                                    <asp:Button ID="btnSubmitReview" runat="server" Text="ส่งรีวิว"
                                        OnClick="btnSubmitReview_Click" CssClass="btn btn-primary px-4" />
                                </div>
                            </div>
                            <% } else { %>
                                <div class="text-center py-2">
                                    <h5><i class="fas fa-sign-in-alt me-2"></i>เข้าสู่ระบบเพื่อเขียนรีวิว</h5>
                                    <p class="text-muted">แบ่งปันประสบการณ์ของคุณกับเราและเพื่อนๆ</p>
                                    <a href="/Pages/Login.aspx" class="btn btn-outline-primary">เข้าสู่ระบบ</a>
                                </div>
                                <% } %>
                    </div>
                </div>
            </div>

            <!-- Reviews List -->
            <% if (ReviewList.Count> 0) { %>
                <div class="review-masonry">
                    <% foreach (var item in ReviewList) { %>
                        <div class="review-item">
                            <div class="review-card">
                                <i class="fas fa-quote-right quote-icon"></i>
                                <p class="review-text">"<%= item.Comment %>"</p>
                                <div class="d-flex align-items-center">
                                    <div class="reviewer-avatar-placeholder">
                                        <%= item.ReviewerName.Substring(0,1).ToUpper() %>
                                    </div>
                                    <div>
                                        <h5 class="reviewer-name" style="font-size: 1rem;">
                                            <%= item.ReviewerName %>
                                        </h5>
                                        <div class="rating">
                                            <% for(int i=0; i<item.Rating; i++) { %> <i class="fas fa-star"></i>
                                                <% } %>
                                                    <% for(int i=item.Rating; i<5; i++) { %> <i
                                                            class="far fa-star text-muted" style="opacity:0.3"></i>
                                                        <% } %>
                                        </div>
                                        <small class="text-muted" style="font-size: 0.75rem;">
                                            <%= item.CreatedDate.ToString("dd MMM yyyy") %>
                                        </small>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <% } %>
                </div>
                <% } else { %>
                    <div class="text-center py-5">
                        <i class="far fa-comment-dots fa-3x text-muted mb-3"></i>
                        <p class="text-muted">ยังไม่มีรีวิว เป็นคนแรกที่รีวิวเลย!</p>
                    </div>
                    <% } %>
        </div>
    </asp:Content>