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
	<h2>Disponibilidade de GLP</h2>
	<p>Consultar e cadastrar disponibilidade de GLP</p>

	<h3>Operação</h3>
	<p>O sistema permite a consulta e o lançamento de horários disponíveis do docente.</p>

	<h4>Consulta disponibilidade de GLP</h4>
	<p>Todos os horários disponíveis cadastrados do docente são carregados junto com a página.</p>

	<h4>Cadastro disponibilidade de GLP</h4>
	<p>
		Para efetuar o cadastro de nova disponibilidade, é necessário informar selecionar uma regional, um município, uma disciplina, o dia da semana e o horário disponível de início e fim. 
		Por fim clique no botão 'Incluir'.
	</p>

	<h4>Alterando telefone</h4>
	<p>
		Para alterar o telefone do docente, vá ao campo Telefone e entre com o novo número. 
		Para salvar a alteração clique no botão 'Aplicar alteração'.
	</p>

	<h4>Descrição dos campos</h4>
	<ul>
		<li>
			<h4>Grade: Horários Disponíveis Cadastrados</h4>
			<ul>
				<li>Coordenadoria: Nome da regional.</li>
				<li>Município: Nome do município.</li>
				<li>Disciplina: Disciplina em disponibilidade de GLP.</li>
				<li>Dia da Semana: Dia da semana.</li>
				<li>Hora Disponível Início: Hora disponível inicial.</li>
				<li>Hora Disponível Fim: Hora disponível final.</li>	
				<li>Botão remover: Remove a disponibilidade</li>
			</ul>
		</li>
	</ul>

</div>
</asp:Content>


