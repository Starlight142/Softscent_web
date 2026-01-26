<%@ Page Title="News Detail" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NewsDetails.aspx.cs" Inherits="Pages_NewsDetails" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <% if (CurrentNews !=null) { %>
                <div class="card shadow-lg border-0 rounded-3 overflow-hidden">
                    <img src="<%= GetImageUrl(CurrentNews.ImageUrl) %>" class="card-img-top"
                        alt="<%= CurrentNews.Title %>" style="max-height: 400px; object-fit: cover;">
                    <div class="card-body p-5">
                        <div class="mb-3 text-muted">
                            <i class="far fa-calendar-alt me-2"></i>
                            <%= CurrentNews.PublishedDate.ToString("dd MMMM yyyy") %>
                        </div>
                        <h1 class="card-title fw-bold mb-4">
                            <%= CurrentNews.Title %>
                        </h1>
                        <div class="card-text" style="font-size: 1.1rem; line-height: 1.8; white-space: pre-wrap;">
                            <%= CurrentNews.Content %>
                        </div>
                        <div class="mt-5 pt-3 border-top">
                            <a href="News.aspx" class="btn btn-outline-secondary"><i
                                    class="fas fa-arrow-left me-2"></i>Back to News</a>
                        </div>
                    </div>
                </div>
                <% } else { %>
                    <div class="alert alert-warning text-center">
                        <h4>News article not found.</h4>
                        <a href="News.aspx" class="btn btn-primary mt-3">Back to News</a>
                    </div>
                    <% } %>
        </div>
    </asp:Content>