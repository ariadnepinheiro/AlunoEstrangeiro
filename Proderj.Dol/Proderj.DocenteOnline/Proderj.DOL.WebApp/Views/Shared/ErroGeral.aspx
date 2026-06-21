<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Proderj.DOL.WebApp.Models" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	Erro Inesperado
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "erroinesperado"}) %>" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server"><% 
		var modeloCabecalho = new CabecalhoViewModel
		                      	{
		                      		BotaoInicioHabilitado = true,
		                      		BotaoSairHabilitado = true
		                      	};

		Html.RenderPartial("Cabecalho", modeloCabecalho);  %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
<div class="conteudo">
<h3>Ocorreu um erro inesperado.</h3>
<h3><% 
if (Response.Cookies["MensagemErroGeral"] != null) {%>
	<%=Response.Cookies["MensagemErroGeral"].Value%><%
	Response.Cookies.Remove("MensagemErroGeral");
}
%></h3>
		
<p>
<h3>Stack trace: </h3>
<pre>
<% if (Response.Cookies["StackErroGeral"] != null)
   {%>
<%=Response.Cookies["StackErroGeral"].Value%><%
	   Response.Cookies.Remove("StackErroGeral");
} %>
</pre>
</p>
</div>
</asp:Content>
