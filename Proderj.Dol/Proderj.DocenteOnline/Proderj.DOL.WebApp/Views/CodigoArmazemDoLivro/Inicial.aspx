<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.CodigoArmazemDoLivroViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "codigo-armazem-do-livro"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "codigo-armazem-do-livro-inicial" }) %>"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
    
    <% if (!string.IsNullOrWhiteSpace(Model.Codigo)) { %>

    <div style="
        text-align: center;
        width: 90%;
        background-color: #ededed;
        display: inline-block;
        position: relative;
        margin: 5%;
        line-height: 50px;
        font-size: 20px;
        padding: 20px 0;
        border-radius: 15px;">
        O seu Voucher para acesso ao Armazém Do Livro é:<br />
        <span style="color: #f00;"><%= Model.Codigo%></span>
    </div>

    <% } else { %>

    <div style="
        text-align: center;
        width: 90%;
        background-color: #ededed;
        display: inline-block;
        position: relative;
        margin: 5%;
        line-height: 50px;
        font-size: 20px;
        padding: 20px 0;
        border-radius: 15px;">
        Você não possui um Voucher para acesso ao Armazém do Livro.<br />
    </div>

    <% } %>

</asp:Content>
