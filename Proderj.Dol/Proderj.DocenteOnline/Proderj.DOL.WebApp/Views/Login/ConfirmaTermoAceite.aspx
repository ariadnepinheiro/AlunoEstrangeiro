<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.ConfirmaTermoAceiteViewModel>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "login-confirmatermoaceite"}) %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "login-confirmatermoaceite" }) %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	<%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
	<% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
	<div class="conteudo">
		<h2 class="titulo-pagina"><%=Model.TituloDaPagina %></h2>
	
		<iframe src="<%=Url.Content("~/Arquivos/Termos/" + Model.Arquivo) %>"></iframe>

		<form id="frm-termo-aceite" method="post">
			<%=Html.HiddenFor(m => m.Ano) %>
			<%=Html.HiddenFor(m => m.Codigo) %>
			<%=Html.Hidden("AceitouTermo",true)%>
			<div class="botoes">
				<input type="button" id="bt-concordo" value="Declaro que li e aceito o termo de compromisso" />
				<input type="button" id="bt-nao-concordo" value="Não concordo" />
			</div>
		</form>
	</div>
</asp:Content>