<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.RespostaCurriculoMinimoListaViewModel>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "respostacurriculominimo-lista"}) %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "respostacurriculominimo-lista" }) %>"></script>
	
	<script type="text/javascript">
		var gblChaveImpressao = '<%=Model.QueryStringImpressao%>';
	</script>
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
	<%
	Response.Write(Html.ValidationSummary(Model.MensagemSumario));
	if (Model.ListaGruposDistintosOrdenados != null)
	{ %>
		<ul class="caixa-avaliacao">
			<li>
				<a href="<%=Url.Action("CurriculoMinimo", "Ajuda")%>" id="link-ajuda">
					<img src="<%=Url.Content("~/Imagens/ico_ajuda.gif") %>" alt="Perguntas frequêntes" title="Perguntas frequêntes" /> 
					Perguntas frequêntes
				</a>
			</li>
			<li>
				<a href="<%=Url.Action("Lista","AvaliacaoCurriculoMinimo") %>" id="link-avaliacao">
					<img src="<%=Url.Content("~/Imagens/ico_alerta.png") %>" alt="Avalie o Currículo mínimo"  alt="Avalie o Currículo mínimo" />
					Avalie o Currículo mínimo
				</a>
			</li>
		</ul>
	
		<h2><%=Model.TituloLancamentoResposta %></h2>
	<% } %>

	<form id="frm-lancamento" method="post" action="<%=Url.Action("Salva","RespostaCurriculoMinimo") %>">
			<% 
			
			if (Model.ListaGruposDistintosOrdenados != null)
			{ 
				int contadorRespostas = 0;
				foreach (var grupo in Model.ListaGruposDistintosOrdenados) { %>
						<table class="tabela-perguntas">
							<caption class="pergunta"><%=grupo.Descricao %></caption><% 
							foreach (var resposta in Model.ListaRespostasOrdenadasPor(grupo)) { %>
								<tr class="resposta">
									<td>
										<%=Html.Hidden("ListaResposta[" + contadorRespostas + "].Codigo", resposta.CodigoResposta)%>
										<%=Html.Hidden("ListaResposta[" + contadorRespostas + "].CodigoGrupo", resposta.CodigoGrupo)%>
										<%=Html.CheckBox("ListaResposta[" + contadorRespostas + "].Respondido", resposta.Respondido)%>								
									</td>
									<td class="descricao-resposta">
										<label for="ListaResposta_<%=contadorRespostas %>__Respondido"><%=resposta.DescricaoResposta %></label>
									</td>
								</tr>
								<%
								contadorRespostas++;
							} %>
						</table>
					<% 
				}
			%>


			<div class="botoes-acoes">
				<button id="bt-imprimir">
					<img src="<%=Url.Content("~/Imagens/ico_impressora.png") %>" alt="Imprimir" title="Imprimir" /> 
					Imprimir
				</button>

				<button value="Salvar" id="bt-salvar-lancamento">
					<img src="<%=Url.Content("~/Imagens/ico_salvar.png")%>" title="Salvar" alt="Salvar" />
					Salvar
				</button>

				<button value="Cancelar" id="bt-cancelar-lancamento">
					<img src="<%=Url.Content("~/Imagens/ico_voltar.png")%>" title="Cancelar" alt="Cancelar" />
					Cancelar
				</button>
			</div>
			<% } else {%>
				<div class="botoes-acoes">
					<button id="bt-cancelar-lancamento">
						<img src="<%=Url.Content("~/Imagens/ico_voltar.png")%>" title="Voltar" alt="Voltar" />
						Voltar
					</button>
				</div>
			<%} %>
		</form>
	
</div>
</asp:Content>

