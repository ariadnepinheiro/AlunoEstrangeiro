<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.AjudaViewModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	<%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS") %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "ajuda" }) %>"></script>

	<script type="text/javascript">
		$(document).ready(function () {
			$('.cabecalho a.ico_sair').on('click', function () {
				if (opener) {
					window.close();
					return false;
				}
			})

		})
	</script>
	<style type="text/css">
	 h2 { font-size:14px; color:#000;}
	</style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
	<% Html.RenderPartial("Cabecalho",Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
<div class="conteudo">
	<h2>Protocolos de lançamento de notas gerados</h2>
	<p>Consultar os protocolos de lançamento de notas gerados.</p>

	<h3>Operação</h3>
	<p>O sistema permite a consulta dos protocolos gerados notas em um determinado período letivo.</p>

	<h4>Listagem de protocolos de lançamento</h4>
	<p>Escolha o período letivo desejado para o lançamento das notas. Caso nenhum período letivo seja selecionado, todos serão apresentados</p>

	<h4>Descrição dos campos</h4>
	<ul>
		<li>
			<h4>Grade: Protocolos gerados</h4>
			<ul>
				<li>Data: data de emissão do protocolo.</li>
				<li>Nº Protocolo: número identificador do protocolo.</li>
				<li>T: tipo de protocolo</li>
				<li>Ano: ano letivo do lançamento de notas</li>
				<li>P: período letivo do lançamento de notas</li>
				<li>B: bimestre do lançamento de notas</li>
				<li>Turma: código da turma</li>
				<li>Disciplina: nome da disciplina</li>
			</ul>
		</li>
	</ul>
</div>
</asp:Content>


