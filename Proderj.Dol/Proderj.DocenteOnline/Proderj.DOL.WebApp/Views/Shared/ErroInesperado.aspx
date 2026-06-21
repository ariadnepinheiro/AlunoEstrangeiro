<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Exception>" %>
<%@ Import Namespace="Proderj.DOL.Service" %>
<%@ Import Namespace="Proderj.DOL.WebApp.Models" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	Erro Inesperado
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "shared-erroinesperado"}) %>" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server"><% 
		var modeloCabecalho = new CabecalhoViewModel
		                      	{
		                      		BotaoInicioHabilitado = true,
		                      		BotaoSairHabilitado = true
		                      	};

		if (HttpContext.Current.User is DTODocenteLogadoPrincipal)
		{
			modeloCabecalho.DocenteLogadoModelo = new DocenteLogadoBindModel((DTODocenteLogadoPrincipal) HttpContext.Current.User);
		}
		Html.RenderPartial("Cabecalho", modeloCabecalho);  %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
<div class="conteudo"  style="text-align: center;">
  
  
 <img runat="server" src="~/Imagens/mensagem-erro.png" style=" width:400px" />
<div class="painel-erro">

<p>Ocorreu um problema ao processar sua solicitação.
Por favor, tente novamente em alguns instantes. Caso o problema persista, entre em contato com o suporte informando o que você estava tentando realizar.</p>

</div>
</div>
</asp:Content>