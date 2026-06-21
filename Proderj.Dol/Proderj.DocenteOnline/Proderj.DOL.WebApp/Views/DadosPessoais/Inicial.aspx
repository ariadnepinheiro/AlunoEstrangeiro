<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.DadosPessoaisInicialViewModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	<%=Model.TituloDaPagina %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "dadospessoais-inicial"}) %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "dadospessoais-inicial" }) %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPage("<%= Model.CabecalhoModelo.DocenteLogadoModelo.Matricula %>");
        });
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
	<% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
<div class="conteudo">
    <% Html.RenderPartial("DadosPessoais", Model.DadosPessoais); %>
</div>
</asp:Content>