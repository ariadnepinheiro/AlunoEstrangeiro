<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.AvaliacaoCurriculoMinimoListaViewModel>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "avaliacaocurriculominimo-lista"}) %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "avaliacaocurriculominimo-lista" }) %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	<%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
	<% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">

<div id="turma-selecionada">
	<div class="bt-mudar-turma">
		<a href="<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "RespostaCurriculoMinimo"}) %>"> 
			<img src="<%=Url.Content("~/Imagens/ico_mudarturma.png") %>" 
				alt="Clique aqui para alterar a turma" 
				title="Clique aqui para alterar a turma" />
			Clique aqui para alterar a turma
		</a>
	</div>	
	<div class="turma-selecionada-detalhe">
		<p>
			<label>Unidade de Ensino:</label>
			<span><%=Model.TurmaSelecionadaModelo.DescricaoUnidadeEnsino %></span>
		</p>
		<p>
			<label>Ano:</label>
			<span><%=Model.TurmaSelecionadaModelo.Ano %></span>
		</p>
		<p>
			<label>Período:</label>
			<span><%=Model.TurmaSelecionadaModelo.Periodo %></span>
		</p>
		<p>
			<label>Turma:</label>
			<span><%=Model.TurmaSelecionadaModelo.CodigoTurma %></span>
		</p>
		<p>
			<label>Disciplina:</label>
			<span><%=Model.TurmaSelecionadaModelo.DescricaoDisciplina %></span>
		</p>
		<p>
			<label>Matrícula:</label>
			<span><%=Model.TurmaSelecionadaModelo.MatriculaDocente %></span>
		</p>
		              <p>
                <label>
                    ID/Vínculo:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.IdFuncional %></span> / <%=Model.TurmaSelecionadaModelo.Vinculo %></span>
            </p>
		<p>
			<label>Nome:</label>
			<span><%=Model.TurmaSelecionadaModelo.NomeDocente %></span>
		</p>
	</div>
</div>

<div class="conteudo">

    <ul id="tab-bimestres"><% 
		foreach (var bimestre in Model.BimestresHabilitadosGeral) {
			string classeBimestre = (bimestre.SubPeriodo == Model.BimestreSelecionado) ? "class=\"selecionado\"" : String.Empty;
			if (!Model.BimestreAtivo(bimestre.SubPeriodo))
				classeBimestre = "class=\"inativo\"";
			%>
			<li <%=classeBimestre %> data-subperiodo="<%=bimestre.SubPeriodo %>"><%=bimestre.Descricao %></li><% 
		} %>
    </ul>

	<form id="frm-postback" method="post">
		<input type="hidden" id="FMB-CodigoCurso" name="CodigoCurso" value="<%=Model.CodigoCurso %>" />
		<input type="hidden" id="FMB-TipoCurso" name="TipoCurso" value="<%=Model.TipoCurso %>" />
		<input type="hidden" id="FMB-CodigoUnidade" name="CodigoUnidadeEnsino" value="<%=Model.CodigoUnidadeEnsino %>" />
		<input type="hidden" id="FMB-Ano" name="Ano" value="<%=Model.Ano %>" />
		<input type="hidden" id="FMB-Periodo" name="Periodo" value="<%=Model.Periodo%>" />
		<input type="hidden" id="FMB-Serie" name="Serie" value="<%=Model.Serie %>" />
		<input type="hidden" id="FMB-CodigoTurma" name="CodigoTurma" value="<%=Model.CodigoTurma %>" />
		<input type="hidden" id="FMB-CodigoDisciplina" name="CodigoDisciplina" value="<%=Model.CodigoDisciplina %>" />
		<input type="hidden" id="FMB-CodigoModalidade" name="CodigoModalidade" value="<%=Model.CodigoModalidade %>" />
		<input type="hidden" id="FMB-Subperiodo" name="Subperiodo" value="" />
	</form>
	
	<% 
	if (Model.LancamentoPersistido) { %>
		<div class="mensagem-sucesso">
			<%=Model.MensagemLancamentoPersistido%>
		</div><% 
	} %>

	<%=Html.ValidationSummary(Model.MensagemSumario)%>
	<h2><%=Model.TituloLista %></h2>
    <form id="frm-lancamento" method="post" action="<%=Url.Action("Salva","AvaliacaoCurriculoMinimo") %>">
		<ol class="perguntas">
			<%
			int contadorAvaliacao = -1;
            if (Model.DadosAvaliacoesEJustificativas != null)
            {
			    foreach (var avaliacao in Model.DadosAvaliacoesEJustificativas.ListaAvaliacaoCurriculoMinimo) {
				    contadorAvaliacao++; %>
				    <li>
					    <h3><%=avaliacao.DescricaoAvaliacao %></h3>
					    <input type="hidden" name="ListaAvaliacao[<%=contadorAvaliacao%>].Codigo" value="<%=avaliacao.IdAvaliacaoCurriculoMinimo %>" />
					    <ol class="respostas">
						    <li>
							    <input type="radio" id="ListaAvaliacaoSim[<%=contadorAvaliacao%>]" name="ListaAvaliacao[<%=contadorAvaliacao%>].Resposta" value="Sim" <% if (avaliacao.EhAvaliadoPositivamente.HasValue && avaliacao.EhAvaliadoPositivamente.Value) { %>checked="checked"<% } %> />
							    <label for="ListaAvaliacaoSim[<%=contadorAvaliacao%>]">Sim</label>
						    </li>
						    <li>
							    <input type="radio" id="ListaAvaliacaoNao[<%=contadorAvaliacao%>]" name="ListaAvaliacao[<%=contadorAvaliacao%>].Resposta" value="Nao" <% if (avaliacao.EhAvaliadoPositivamente.HasValue && !avaliacao.EhAvaliadoPositivamente.Value) { %>checked="checked"<% } %> />
							    <label for="ListaAvaliacaoNao[<%=contadorAvaliacao%>]">Não</label>
						    </li>
					    </ol>
				    </li><% 
			    }
            } %>
		</ol>

		<div class="justificativa">
			<label for="txaJustiticativa"><%=Resources.Recurso.AvaliacaoCurriculoMinimoLista_LegendaJustificativa %></label>
			<textarea id="txaJustificativa" name="DescricaoJustificativa"><%=Model.DadosAvaliacoesEJustificativas.DescricaoJustificativa %></textarea>
		</div>

		<div class="botoes-acoes">

			<button value="Salvar" id="bt-salvar-lancamento">
				<img src="<%=Url.Content("~/Imagens/ico_salvar.png")%>" title="Salvar" alt="Salvar" />
				Salvar
			</button>

			<button value="Cancelar" id="bt-cancelar-lancamento">
				<img src="<%=Url.Content("~/Imagens/ico_voltar.png")%>" title="Cancelar" alt="Cancelar" />
				Cancelar
			</button>
		</div>
	</form>
</div>
</asp:Content>

