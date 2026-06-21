<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.SelecaoTurmasListaViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	<%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "selecaoturmas-lista"}) %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "selecaoturmas-lista" }) %>"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
	<% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
<div class="conteudo">
    <h2 class="titulo-pagina"><%=Model.TituloDaPagina %></h2>
    
    <p class="mensagem-lancamento"><%=Model.MensagemInicialTela %></p>
    <%=Html.ValidationSummary(Model.MensagemSumario)%>

    <div class="tabela-padrao">
		<div class="tabela-cabecalho">
			<img class="aba-direita" src="<%=Url.Content("~/Imagens/tabela_cabecalho_abadir.gif")%>"/>
			<div class="centro">
				<img src="<%=Url.Content("~/Imagens/tabela_cabecalho_grid.png")%>"/></div>
			<div class="aba-esquerda"></div>
			<span><%=Resources.Recurso.SelecaoTurmasLista_TabelaTitulo%></span>
		</div>
		<div class="tabela-legenda">
			<ul>
                <li><span class="caixinha-cor legenda-azul"></span> Disciplina não deverá possuir <br />lançamento de notas e faltas</li>
				<li><span class="caixinha-cor legenda-vermelho"></span> Notas pendentes</li>
				<li><span class="caixinha-cor legenda-verde"></span> Notas lançadas</li>
			</ul>
		</div>	<%
		foreach (string unidadeDeEnsino in Model.ListaSelecaoTurma.Select(dto => dto.UnidadeEnsino).Distinct())
		{
			var nomeUnidadeEnsino = Model.ListaSelecaoTurma.FirstOrDefault(dto => dto.UnidadeEnsino == unidadeDeEnsino).NomeCompletoUnidadeEnsino; %>
			<div class="unidade" data-codigo-unidade="<%=unidadeDeEnsino%>">
				<h2>
					<span class="expansor" data-tipo="unidade"></span>
					Unidade: <%=nomeUnidadeEnsino%>
				</h2><%
				IEnumerable <string> listaDeAnosEPeriodosPorUnidadeDeEnsino = Model.ListaSelecaoTurma
																			.Where(dto => dto.UnidadeEnsino == unidadeDeEnsino)
																			.Select(dto => String.Concat(dto.Ano, '/', dto.Semestre))
																			.Distinct();

			foreach (string anoPeriodo in listaDeAnosEPeriodosPorUnidadeDeEnsino)
			{
				string[] vetorAnoPeriodo = anoPeriodo.Split('/');
				string ano = vetorAnoPeriodo[0];
				string periodo = vetorAnoPeriodo[1];
			
				%>
			
				<div class="periodo" data-periodo="<%=periodo%>" data-ano="<%=ano%>" >
					<h2>
						<span class="expansor" data-tipo="periodo"></span>
						Período: <%=anoPeriodo %>
					</h2><%
					var listaDTOSelecaoTurmaPorAnoPeridoEUnidade = Model.ListaSelecaoTurma
																			.Where(dto =>	dto.UnidadeEnsino == unidadeDeEnsino &&
																							String.Concat(dto.Ano, '/', dto.Semestre) == anoPeriodo)
																			.ToList(); %>
					<table>
						<thead  >
							<tr>
								<td rowspan="2">Turma</td>
								<td rowspan="2">Disciplina</td>
                                <%
                                if (periodo == "0" && Convert.ToInt32(ano) >= 2025)
                                { 
                                %>
                                    <td colspan="3">Status dos trimestres para lançamento</td>
                                <% 
                                    }
                                    else
                                    {
                                %>
                                    <td colspan="3">Status dos bimestres para lançamento</td>
                                <% 
                                    }
                                %>								
							</tr>
							<tr>
								<td class="td-status" title="Liberado para lançamento de notas e frequências" >Liberado</td>
								<td class="td-status" title="Aguardando aprovação da solicitação de reabertura" >Aguardando</td>
								<td class="td-status" title="Bloqueados para lançamento de notas e frequências" >Bloqueados</td>
							</tr>
						</thead>
						<tbody onclick="Bloqueio()"><% 
							bool pintarLinha = false;
							foreach (var dtoSelecaoTurma in listaDTOSelecaoTurmaPorAnoPeridoEUnidade)
							{
								string atributoPossuiNotaPendente = String.Empty;
								string atributoInvalidoParaLancamento = String.Empty;
								string classePossuiNotaPendente = String.Empty;
								string classeLinha = String.Empty;
							    string classeRegistroProvaFrequencia = String.Empty;
                                
								if (pintarLinha)
								{
									classeLinha = " linha-alternada";
								}
								pintarLinha = !pintarLinha;
							
								if (dtoSelecaoTurma.PossuiNotasPendentes || dtoSelecaoTurma.PossuiFaltasPendentes)
								{
									atributoPossuiNotaPendente = " possui-nota-pendente=\"\"";
									classePossuiNotaPendente = " possui-nota-pendente";
								}

								if (!dtoSelecaoTurma.ValidoParaLancamento)
								{
									atributoInvalidoParaLancamento = " invalido-para-lancamento=\"\"";
								}

                                if (!dtoSelecaoTurma.RegistroProvaFrequencia)
                                {
                                    classeRegistroProvaFrequencia = " frequencia-prova";
                                }
								%>
								<tr class="<%=String.Concat(classePossuiNotaPendente, classeLinha)%> <%=classeRegistroProvaFrequencia%>" <%=atributoPossuiNotaPendente%> <%=atributoInvalidoParaLancamento%>
									data-codigo-turma="<%=dtoSelecaoTurma.Turma %>"
									data-codigo-disciplina="<%=dtoSelecaoTurma.Disciplina %>"
									data-codigo-curso="<%=dtoSelecaoTurma.Curso%>"
									data-codigo-modalidade="<%=dtoSelecaoTurma.Modalidade%>"
									data-tipocurso="<%=dtoSelecaoTurma.Tipo %>"
									data-serie="<%=dtoSelecaoTurma.Serie %>"
								>
									<td><%=dtoSelecaoTurma.Turma %></td>
									<td><%=dtoSelecaoTurma.NomeCompletoDisciplina %></td>
                                    <%
                                if (!dtoSelecaoTurma.RegistroProvaFrequencia)
                                {
                                 %>
									<td class="td-status"></td>
									<td class="td-status"></td>
									<td class="td-status"><%=dtoSelecaoTurma.StatusLancamentoLiberado + dtoSelecaoTurma.StatusLancamentoAguardando + dtoSelecaoTurma.StatusLancamentoBloqueado%></td>

								</tr><%   
                                }
                                else
                                {
                                    %>
									<td class="td-status"><%=dtoSelecaoTurma.StatusLancamentoLiberado%></td>
									<td class="td-status"><%=dtoSelecaoTurma.StatusLancamentoAguardando%></td>
									<td class="td-status"><%=dtoSelecaoTurma.StatusLancamentoBloqueado%></td>

								</tr><%
                                }
							} %>
						</tbody>
					</table>
				</div><%										
			}
			%></div><%
		}

	%>
	</div>
	<form id="frm-selecao-turma" method="post" action="<%=Model.AcaoPost%>"  >
		<input type="hidden" id="FST-CodigoCurso" name="CodigoCurso" />
		<input type="hidden" id="FST-TipoCurso" name="TipoCurso" />
		<input type="hidden" id="FST-CodigoUnidade" name="CodigoUnidadeEnsino" />
		<input type="hidden" id="FST-Ano" name="Ano" />
		<input type="hidden" id="FST-Periodo" name="Periodo" />
		<input type="hidden" id="FST-Serie" name="Serie" />
		<input type="hidden" id="FST-CodigoTurma" name="CodigoTurma" />
		<input type="hidden" id="FST-CodigoDisciplina" name="CodigoDisciplina" />
		<input type="hidden" id="FST-CodigoModalidade" name="CodigoModalidade" />
	</form>
</div>
</asp:Content>


