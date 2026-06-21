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
	<h2>Lançamento de notas e frequência</h2>
	<p>Consultar, cadastrar e alterar as notas do aluno.</p>

	<h3>Operação</h3>
	<p>O sistema permite a consulta, o lançamento e a alteração das notas em um determinado período letivo.</p>

	<h4>Escolher Período Letivo</h4>
	<p>Escolha o período letivo desejado para o lançamento das notas. Caso nenhum período letivo seja selecionado, todos serão apresentados</p>

	<h4>Cadastrar/Alterar notas</h4>
	<p>
		Selecione o aluno e a avaliação desejados e preencha na célula correspondente a nota do aluno.

		Caso uma avaliação tenha nota máxima cadastrada, não será possível inserir valores superiores a nota máxima

		Caso uma avaliação possua fórmula cadastrada, não será possível preencher as notas.

		Após lançar todas as notas desejadas, clique no botão  para salvar as alterações.
	</p>

	<h4>Descrição dos campos</h4>
	<ul>
		<li>
			<h4>Grade:  Lançamento de Notas</h4>
			<ul>
				<li>Nome: nome do aluno.</li>
				<li>Sit. Matrícula: situação da matrícula do aluno.</li>
				<li>Nota: indica a nota do aluno (conceito)</li>
				<li>Faltas: indica o número de faltas do aluno no período</li>
				<li>Recuperação paralela: indica se o aluno está em recuperação paralela</li>
				<li>Sem avaliação: indica que o aluno não pôde ser avaliado</li>
				<li>Justificativa da não avaliação: indica o possivel motivo para a impossibilidade da avaliação</li>
			</ul>
		</li>
	</ul>
</div>
</asp:Content>


