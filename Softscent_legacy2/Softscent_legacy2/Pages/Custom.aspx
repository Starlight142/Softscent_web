<%@ Page Title="Create Custom Blend" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Custom.aspx.cs" Inherits="Pages_Custom" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
        <div class="container py-5">
            <div class="row align-items-center">
                <div class="col-lg-6">
                    <h1 class="mb-4 display-4">
                        <%= LangHelper.Get("CraftYourScent") %>
                    </h1>
                    <p class="lead mb-5">
                        <%= LangHelper.Get("CraftSubtitle") %>
                    </p>
                </div>
                <div class="col-lg-6">
                    <div class="card p-5 glass-nav">
                        <!-- Inputs will be submitted via the MasterPage form to the PostBackUrl -->
                        <input type="hidden" name="action" value="addCustom" />
                        <input type="hidden" name="productId" value="-1" />

                        <h3 class="mb-2">
                            <%= LangHelper.Get("SelectIngredients") %>
                        </h3>
                        <p class="text-muted small mb-4">
                            <%= LangHelper.Get("PremiumIngredients") %> (<%= HerbList.Count %>)
                        </p>

                        <div class="mb-4" style="max-height: 500px; overflow-y: auto; padding-right: 10px;">
                            <h6 class="text-primary fw-bold mb-3 border-bottom pb-2">
                                <%= LangHelper.Get("ClassicHerbs") %>
                            </h6>
                            <% foreach (var herb in HerbList.FindAll(h=> h.Id <= 6)) { %>
                                    <div class="form-check mb-3 custom-herb-item">
                                        <input class="form-check-input herb-checkbox" type="checkbox"
                                            value="<%= herb.Name %>" id="herb_<%= herb.Id %>"
                                            name="selectedHerbs_array">
                                        <label class="form-check-label d-flex align-items-center w-100"
                                            for="herb_<%= herb.Id %>">
                                            <div class="flex-grow-1">
                                                <div class="fw-bold">
                                                    <%= GetHerbThaiName(herb.Name) %>
                                                </div>
                                                <div class="text-muted small">
                                                    <%= !string.IsNullOrEmpty(GetHerbThaiBenefit(herb.Name)) ?
                                                        GetHerbThaiBenefit(herb.Name) : herb.Benefit %>
                                                </div>
                                            </div>
                                        </label>
                                    </div>
                                    <% } %>

                                        <h6 class="text-primary fw-bold mt-4 mb-3 border-bottom pb-2">
                                            <%= LangHelper.Get("ThaiHerbs") %>
                                        </h6>
                                        <% foreach (var herb in HerbList.FindAll(h=> h.Id > 6)) { %>
                                            <div class="form-check mb-3 custom-herb-item">
                                                <input class="form-check-input herb-checkbox" type="checkbox"
                                                    value="<%= herb.Name %>" id="herb_<%= herb.Id %>"
                                                    name="selectedHerbs_array">
                                                <label class="form-check-label d-flex align-items-center w-100"
                                                    for="herb_<%= herb.Id %>">
                                                    <div class="flex-grow-1">
                                                        <div class="fw-bold">
                                                            <%= GetHerbThaiName(herb.Name) %>
                                                        </div>
                                                        <div class="text-muted small text-success">
                                                            <%= !string.IsNullOrEmpty(GetHerbThaiBenefit(herb.Name)) ?
                                                                GetHerbThaiBenefit(herb.Name) : herb.Benefit %>
                                                        </div>
                                                    </div>
                                                </label>
                                            </div>
                                            <% } %>
                        </div>

                        <!-- Hidden input to store comma separated string, populated by JS -->
                        <input type="hidden" name="customConfig" id="selectedHerbsInput" />

                        <div class="d-grid mt-4">
                            <asp:Button runat="server" Text='<%# LangHelper.Get("CreateBlend") %>'
                                CssClass="btn btn-primary btn-lg" OnClientClick="prepareSubmission()"
                                PostBackUrl="~/Pages/Cart.aspx" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script>
            function prepareSubmission() {
                var checked = document.querySelectorAll('.herb-checkbox:checked');
                var values = Array.from(checked).map(c => c.value);
                document.getElementById('selectedHerbsInput').value = values.join(', ');
            }
        </script>
    </asp:Content>