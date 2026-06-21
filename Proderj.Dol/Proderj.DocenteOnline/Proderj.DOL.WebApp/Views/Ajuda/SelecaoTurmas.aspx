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
	<h2>Seleção de Turmas</h2>
	<p>Lista as turmas que o Docente tem vínculo para seleção</p>

	<h3>Operação</h3>
	<p>O sistema permite a listagem das turmas para efetuar operações de lançamento de notas e lançamento de Currículo mínimo</p>

	<h4>Listagem de turmas e disciplinas do docente</h4>
	<p>Todos as turmas disponíveis são carregadas junto com a página.</p>

	<h4>Selecionando uma turma</h4>
	<p>
		Para selecionar uma turma, basta clicar com o mouse em cima da linha que representa turma.
	</p>

	<h4>Descrição dos campos</h4>
	<ul>
		<li>
			<h4>Grade: Turmas e disciplinas disponíveis</h4>
			<ul>
				<li>Turma: Código da turma.</li>
				<li>Disciplina: Nome da disciplina.</li>
				<li>Status dos bimestres - Liberado: Bimestre liberado par ao lançamento de notas</li>
				<li>Status dos bimestres - Aguardando: Bimestre aguardando liberação de solicitação para lançamento de notas</li>
				<li>Status dos bimestres - Bloqueado: Bimestres bloqueados para lançamento de notas</li>
			</ul>
		</li>
	</ul>

</div>
</asp:Content>


