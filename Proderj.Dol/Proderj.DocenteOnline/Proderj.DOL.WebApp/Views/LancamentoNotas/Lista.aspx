<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.LancamentoNotasListaViewModel>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "lancamentonotas-lista"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "lancamentonotas-lista-jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "lancamentonotas-lista" }) %>"></script>
    <script type="text/javascript">
<% 
    if (Model.HabilitaLancamentoNotas) 
    { 
        //Redefine a função para nao fazer nada .. 
%>
        function DesabilitaLancamento() { }; 
<%  } 

    if (!Model.LancamentoPersistido) 
    { 
        //Redefine a função para nao fazer nada .. 
%>
	    function ConfirmaAberturaDeFilipetaLancamento() { };
<%  } 
    else { 
%>
        var gblCodigoFilipeta = '<%=Model.CodigoFilipeta %>';
<%  } %>
		var gblChaveImpressaoFilipeta = '<%=Model.QueryStringImpressaoFilipeta%>';

        function atualizaCampoMaterialEstudo() {
            var marcado = '';
            $('.material-estudo').each(function(index, el){
                var valor = $(el).prop('checked');
                if (valor) {
                    var valortarget = el.value;
                    marcado = marcado + (marcado==''?'':',') + valortarget ;
                }            
            });
            $('#materialEstudo').val(marcado);            
        }

        $(document).ready(function(){          
            $('.material-estudo').live('click', atualizaCampoMaterialEstudo);
            atualizaCampoMaterialEstudo();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
    <div id="confirmacao-fundo">
    </div>
    <div id="confirmacao">
        <p class="mensagem">
        </p>
        <div class="botoes">
            <input type="button" class="bt-sim" value="Sim" />
            <input type="button" class="bt-nao" value="Não" />
        </div>
    </div>
    <div id="turma-selecionada">
        <div class="bt-mudar-turma">
            <a href="<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "LancamentoNotas" }) %>">
                <img src="<%=Url.Content("~/Imagens/ico_mudarturma.png") %>" alt="Clique aqui para alterar a turma"
                    title="Clique aqui para alterar a turma" />
                Clique aqui para alterar a turma </a>
        </div>
        <div class="turma-selecionada-detalhe">
            <p>
                <label>
                    Unidade de Ensino:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.DescricaoUnidadeEnsino %></span>
            </p>
            <p>
                <label>
                    Ano:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.Ano %></span>
            </p>
            <p>
                <label>
                    Período:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.Periodo %></span>
            </p>
            <p>
                <label>
                    Turma:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.CodigoTurma %></span>
            </p>
            <p>
                <label>
                    Disciplina:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.DescricaoDisciplina %></span>
            </p>
            <p>
                <label>
                    Matrícula:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.MatriculaDocente %></span>
            </p>
                  <p>
                <label>
                    ID/Vínculo:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.IdFuncional %></span> / <%=Model.TurmaSelecionadaModelo.Vinculo %></span>
            </p>
            <p>
                <label>
                    Nome:</label>
                <span>
                    <%=Model.TurmaSelecionadaModelo.NomeDocente %></span>
            </p>
        </div>
    </div>
    <div class="conteudo">
        <ul id="tab-bimestres">
            <% 
                string classeBimestreSelecionado = string.Empty;
                string classeConsolidadoSelecionado = (Model.ExibeConsolidado) ? "class=\"selecionado\"" : String.Empty;

                foreach (var bimestre in Model.BimestresHabilitados)
                {
                    classeBimestreSelecionado = (bimestre.SubPeriodo == Model.BimestreSelecionado && !Model.ExibeConsolidado) ? "class=\"selecionado\"" : String.Empty;
            %>
            <li onclick="Bloqueio()" <%=classeBimestreSelecionado %> data-subperiodo="<%=bimestre.SubPeriodo %>">
                <%=bimestre.Descricao%></li>
            <% 
                }
            %>
            <%
                if (Model.Periodo == 0 && Model.Ano >= 2025)
                { 
            %>
            <li onclick="Bloqueio()" <%=classeConsolidadoSelecionado %> data-consolidado="S">CONSOLIDADO
                TRIMESTRAL</li>
            <% 
                }
                else
                {
            %>
            <li onclick="Bloqueio()" <%=classeConsolidadoSelecionado %> data-consolidado="S">CONSOLIDADO
                BIMESTRAL</li>
            <% 
                }
            %>
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
        <input type="hidden" id="FMB-CodigoFilipeta" name="CodigoFilipeta" value="<%=Model.CodigoFilipeta %>" />
        <input type="hidden" id="FMB-Subperiodo" name="Subperiodo" value="" />
        <input type="hidden" id="FMB-Justificativa-Reabertura" name="JustificativaReabertura" />
        <input type="hidden" id="FMB-IdVinculo" name="IdVinculo" value="<%=Model.TurmaSelecionadaModelo.IdFuncional%>/<%=Model.TurmaSelecionadaModelo.Vinculo%>" />
        <input type="hidden" id="FMB-ExibeConsolidado" name="ExibeConsolidado" />
        </form>
        <%
            if (Model.LancamentoPersistido)
            { 
        %>
        <div class="mensagem-sucesso">
            <%=Model.MensagemLancamentoPersistido%>
        </div>
        <%  }

            if (Model.HabilitaCurriculoMinimo)
            { 
        %>
        <div class="mensagem-curriculo-minimo <%=Model.DadosPreenchimentoCurriculoMinimo.StatusPreenchimento %>">
            <p>
                <%=Model.MensagemStatusCurriculoMinimo%></p>
            <% 
                if (Model.DadosPreenchimentoCurriculoMinimo.StatusPreenchimento !=
                    Proderj.DOL.Service.StatusPreenchimentoCurriculoMinimoPorTurmaEnum.Completo)
                { 
            %>
            <a id="link-curriculo-minimo" href="<%=Url.Action("Lista","RespostaCurriculoMinimo")%>">
                Clique e acesse o formulário</a>
            <% 
                } 
            %>
        </div>
        <%  }

            if (Model.MensagemSolicitacaoAlteracaoNotasExistente != null)
            { 
        %>
        <div class="mensagem-solicitacao-existente <%=Model.DadosPreenchimentoCurriculoMinimo.StatusPreenchimento %>">
            <p>
                <%=Model.MensagemSolicitacaoAlteracaoNotasExistente%></p>
        </div>
        <%  }

            if (Model.SolicitadoReaberturaLancamento)
            { 
        %>
        <div class="mensagem-sucesso">
            <%=Resources.Recurso.LancamentoNotasLista_SolicitacaoReaberturaLancamento%>
        </div>
        <%  }

            if (Model.ExisteBimestreAnteriorPendenteDeLancamento)
            { 
        %>
        <div class="mensagem">
            <strong>
                <%=Resources.Recurso.LancamentoNotasLista_MensagemBimestreAnteriorPendente%>
            </strong>
        </div>
        <%  }

            if (Model.HabilitaLancamentoNotas && ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] != null)
            { 
        %>
        <div class="mensagem">
            <%=String.Format(Resources.Recurso.LancamentoNotasLista_MensagemContagemRegressiva, ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"])%>
        </div>
        <div class="mensagem">
            <%=Resources.Recurso.LancamentoNotasLista_MensagemAlunosOrdemAlfabetica%>
        </div>
        <div class="mensagem">
            <strong>Tempo restante: <span id="contagem-regressiva"></span></strong>
        </div>
        <% 
            }

            if (Model.DadosConfiguracaoNotaDisciplina != null)
            { 
        %>
        <input type="hidden" id="dados-turma-disciplina" data-nota-maxima="<%=Model.DadosConfiguracaoNotaDisciplina.NotaMaxima %>"
            data-nota-casas-decimais="<%=Model.DadosConfiguracaoNotaDisciplina.CasasDecimais %>" />
        <%
            }


            if (Model.ExibeConsolidado)
            {
                    
        %>
        <div class="mensagem">
            <%=Resources.Recurso.LancamentoNotasLista_MensagemConsolidado%>
        </div>
        <input type="hidden" id="quantidade-bimestre" value="<%=Model.DadosConsolidados.TotalBimestresAtivos%>" />
        <table>
            <tr>
                <td>
                    <table class="tabela-frequencia-turma-consolidado ">
                        <caption>
                            Frequência da turma</caption>
                        <tr>
                            <td class="legenda">
                                Total aulas previstas:
                                <%=Html.TextBoxFor(m => m.DadosConsolidados.TotalAulasPrevistas, new Dictionary<string, object> { { "readonly", true }, { "disabled", true }, { "style", "background:White" } })%>
                            </td>
                        </tr>
                        <tr>
                            <td class="legenda">
                                Total aulas dadas:
                                <%=Html.TextBoxFor(m => m.DadosConsolidados.TotalAulasDadas, new Dictionary<string, object> { { "readonly", true }, { "disabled", true }, { "style", "background:White" } })%>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table class="tabela-media-turma-consolidado ">
                        <caption>
                            Média da turma</caption>
                        <tr>
                            <%
                foreach (KeyValuePair<short?, decimal?> mt in Model.DadosConsolidados.MediaTurma)
                {

                            %>
                            <td class="legenda">
                                <%= mt.Key%>º Bim/Tri:
                            </td>
                            <td>
                                <%=Html.TextBoxFor(m => m.DadosConsolidados.MediaTurma[mt.Key], new Dictionary<string, object> { { "readonly", true }, { "disabled", true }, { "style", "background:White" } })%>
                            </td>
                            <%
                    if (mt.Key == 2)
                    {
                            %>
                        </tr>
                        <tr>
                            <%
                    }
                }
                            %>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="tabela-padrao">
            <div class="tabela-cabecalho">
                <img class="aba-direita" src="<%=Url.Content("~/Imagens/tabela_cabecalho_abadir.gif")%>" />
                <div class="centro">
                    <img src="<%=Url.Content("~/Imagens/tabela_cabecalho_grid.png")%>" />
                </div>
                <div class="aba-esquerda">
                </div>
                <span>
                    <%=Resources.Recurso.LancamentoNotasLista_TabelaTituloConsolidado%>
                </span>
            </div>
            <table>
                <thead>
                    <tr>
                        <td>
                            Nome
                        </td>
                        <td>
                            Situação
                        </td>
                        <%
                    for (int i = 0; i < Model.DadosConsolidados.TotalBimestresAtivos; i++)
                    {
                        if (Model.DadosConsolidados.TotalBimestresAtivos == 3)
                        {
                        %>
                            <td class="centralizado-micro">
                                Nota Final
                                <%=(i + 1)%>º Tri
                            </td>
                            <td class="centralizado-micro">
                                Faltas
                                <%=(i + 1)%>º Tri
                            </td>
                            <%
                        }
                        else
                        {
                          %>
                            <td class="centralizado-micro">
                                Nota Final
                                <%=(i + 1)%>º Bim
                            </td>
                            <td class="centralizado-micro">
                                Faltas
                                <%=(i + 1)%>º Bim
                            </td>
                            <%
                        }
                    }
                        %>
                        <td class="centralizado-micro">
                            Notas Acum.
                        </td>
                        <td class="centralizado-micro">
                            Faltas Acum.
                        </td>
                        <td class="centralizado-micro">
                            % Freq. Acum.
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <%-- <%
                        foreach (var notasFaltas in Model.DadosConsolidados.NotasFrequenciasConsolidadas)
                        {
                          string classeZebrado = (contadorAluno % 2 == 0) ? String.Empty : " linha-alternada";
                          string classeItemConsolidado = String.Concat("class=\"", classeZebrado, classCancelado, '"');  
                    %>--%>
                    <% 
                int contadorAluno = -1;
                int contadorAlunoCancelado = 1000;
                int contadorAlunoMatriculado = -1;

                int linhaNavegacao = 0;
                foreach (var notasFaltas in Model.DadosConsolidados.NotasFrequenciasConsolidadas)
                {
                    string classeZebrado = (contadorAluno % 2 == 0) ? String.Empty : " linha-alternada";
                    string classCancelado = (notasFaltas.SituacaoMatriculado) ? String.Empty : " cancelado";
                    string dadosNavegacaoLinha = String.Empty;

                    contadorAluno++;
                    if (classCancelado != String.Empty)
                    {
                        classeZebrado = String.Empty;
                        contadorAlunoCancelado++;
                    }
                    else
                    {
                        contadorAlunoMatriculado++;
                        linhaNavegacao++;
                        dadosNavegacaoLinha = String.Concat("data-linha-navegacao=\"", linhaNavegacao, "\"");
                    }
                    string classeItem = String.Concat("class=\"", classeZebrado, classCancelado, '"');
                    %>
                    <tr <%=classeItem%>>
                        <td>
                            <%=notasFaltas.NomeAluno%>
                        </td>
                        <td class="situacao">
                            <%=(notasFaltas.SituacaoMatriculado) ? "Matriculado" : "Cancelado"%>
                        </td>
                        <%
                    for (int i = 0; i < Model.DadosConsolidados.TotalBimestresAtivos; i++)
                    {
                        %>
                        <td class="consolidado">
                            <input type="text" value="<%=notasFaltas.Notas[i]%>" class="input-nota-consolidado" />
                        </td>
                        <td class="consolidado">
                            <input type="text" value="<%=notasFaltas.Faltas[i]%>" />
                        </td>
                        <%
                    }
                        %>
                        <td class="consolidado">
                            <input type="text" value="<%=notasFaltas.NotasAcumuladas%>" class="input-nota-acumulada-consolidado" />
                        </td>
                        <td class="consolidado">
                            <input type="text" value="<%=notasFaltas.FaltasAcumuladas%>" />
                        </td>
                        <td class="consolidado">
                            <% 
                    string frequenciaAcumulada = notasFaltas.PercentualFrequenciaAcumulada == default(decimal?) ? string.Empty : Convert.ToString(notasFaltas.PercentualFrequenciaAcumulada) + "%";
                            %>
                            <input type="text" value="<%=frequenciaAcumulada%>" class="input-frequencia-acumulada-consolidado" />
                        </td>
                    </tr>
                    <%
                }
                    %>
                </tbody>
            </table>
            <div class="botoes-acoes">
                <button id="bt-imprimir-filipeta-consolidado">
                    <img src="<%=Url.Content("~/Imagens/ico_impressora.png") %>" alt="Imprimir consolidado"
                        title="Imprimir consolidado" />
                    Imprimir
                </button>
                <button value="Voltar" id="btnVoltar">
                    <img src="<%=Url.Content("~/Imagens/ico_voltar.png")%>" title="Voltar" alt="Voltar" />
                    Voltar
                </button>
            </div>
        </div>
        <%
            }
            else
            {
        %>
        <div class="mensagem">
            <strong style="font-size: 12px; color: #0353AB;">Para saber mais sobre recuperação de
                estudos:</strong>
            <asp:HyperLink ID="SaibaMais" Font-Size="12px" runat="server" Target="_blank" Text="Clique Aqui."
                NavigateUrl="~/Arquivos/SAIBA MAIS.pdf"></asp:HyperLink>
        </div>
        <div class="mensagemFrequencia">
            <strong style="font-size: 12px; color: #0353AB;">
                <%= Model.MensagemFrequenciaNotaFalta%></strong>
            <input class="inputTemFrequencia" type="hidden" value="<%= Model.DisciplinaFrequenciaNota.TemFrequencia %>" />
            <input class="inputTemNota" type="hidden" value="<%= Model.DisciplinaFrequenciaNota.TemNota %>" />
        </div>
        <%=Html.ValidationSummary(String.Format(Resources.Recurso.LancamentoNotasLista_MensagemSumario, Model.CodigoTurmaErro))%>
        <form id="frm-lancamento" method="post" action="<%=Url.Action("Salva","LancamentoNotas") %>">
        <%
                if (Model.DadosFrequenciaTurma != null)
                {    
        %>
        <%=Html.HiddenFor(m => m.DadosFrequenciaTurma.CodigoFrequencia)%>
        <table class="tabela-frequencia-turma">
            <caption>
                Frequência da turma</caption>
            <tr>
                <td class="legenda">
                    Aulas previstas:
                </td>
                <td>
                    <%=Html.TextBoxFor(m => m.DadosFrequenciaTurma.AulasPrevistas, new Dictionary<string, object> { { "maxlength", 3 }, { "id", "AulasPrevistas" } })%>
                </td>
            </tr>
            <tr>
                <td class="legenda">
                    Aulas dadas:
                </td>
                <td>
                    <%=Html.TextBoxFor(m => m.DadosFrequenciaTurma.AulasDadas, new Dictionary<string, object> { { "maxlength", 3 }, { "id", "AulasDadas" } })%>
                </td>
            </tr>
        </table>
        <table class="tabela-frequencia-turma">
            <caption>
                Material de Estudo Proposto</caption>
            <tr>
                <td class="legenda">
                </td>
                <td>
                    <input type="hidden" id="materialEstudo" name="materialEstudo" />
                    <table>
                        <tr>
                            <% for (int i = 0; i < Model.ListaMaterialEstudo.Count(); i++)
                               {
                            %>
                            <%--<%  if (Model.ListaTurmaMaterialEstudo.Where(x => x.MaterialEstudoId == Model.ListaMaterialEstudo[i].MaterialEstudoId).Count() == 1)--%>
                            <%  if (Model.ListaTurmaMaterialEstudo.Any(x => x.MaterialEstudoId == Model.ListaMaterialEstudo[i].MaterialEstudoId))
                                   {
                                        %>
                                        <td></td><td></td><td></td><td></td><td></td>
                                        <td>
                                            <input type="checkbox" class="material-estudo" id="modalidade-medio" data-modalidade-opcao="1"
                                                value="<%=Model.ListaMaterialEstudo[i].MaterialEstudoId %>" name="CodigoModalidade"
                                                checked="checked" />
                                            <label for="modalidade-medio">
                                                <%=Model.ListaMaterialEstudo[i].Descricao%></label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td></td><td></td><td></td><td></td><td></td>
                                        <%
                                   }
                                   else
                                   { %>
                                   <td></td><td></td><td></td><td></td><td></td>
                                        <td>
                                            <input type="checkbox" class="material-estudo" id="modalidade-medio" data-modalidade-opcao="1"
                                                value="<%=Model.ListaMaterialEstudo[i].MaterialEstudoId %>" name="CodigoModalidade" />
                                            <label for="modalidade-medio">
                                                <%=Model.ListaMaterialEstudo[i].Descricao%></label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td></td><td></td><td></td><td></td><td></td>
                                        <% 
                                    } %>
                            </div>
                         <% } %>
                </tr> </table>
    </td> </tr> </table>
    <%
                }
    %>
    <div class="tabela-padrao">
        <div class="tabela-cabecalho">
            <img class="aba-direita" src="<%=Url.Content("~/Imagens/tabela_cabecalho_abadir.gif")%>" />
            <div class="centro">
                <img src="<%=Url.Content("~/Imagens/tabela_cabecalho_grid.png")%>" /></div>
            <div class="aba-esquerda">
            </div>
            <span>
                <%=Resources.Recurso.LancamentoNotasLista_TabelaTitulo%></span>
        </div>
        <table>
            <thead>
                <tr>
                    <td>
                        Nome
                    </td>
                    <td>
                        Situação
                    </td>
                    <td class="centralizado-micro">
                        Nota
                    </td>
                    <td class="centralizado-micro">
                        Faltas
                    </td>
                    <td class="centralizado-pequeno" title="Recuperação paralela">
                        Recuperação de Estudos
                    </td>
                    <td class="centralizado-micro" title="Nota R.P">
                        Nota de recuperação de estudos
                    </td>
                    <td class="centralizado-micro" title="Conceito">
                        Nota final
                    </td>
                    <td class="centralizado-pequeno">
                        Sem avaliação
                    </td>
                    <td class="centralizado-pequeno">
                        Justificativa da não avaliação
                    </td>
                </tr>
            </thead>
            <tbody>
                <% 
                int contadorAluno = -1;
                int contadorAlunoCancelado = 1000;
                int contadorAlunoMatriculado = -1;

                int linhaNavegacao = 0;
                foreach (var item in Model.ListaItemLancamentoNotaFrequenciaAluno)
                {
                    string classeZebrado = (contadorAluno % 2 == 0) ? String.Empty : " linha-alternada";
                    string classCancelado = (item.SituacaoMatriculado) ? String.Empty : " cancelado";
                    string dadosNavegacaoLinha = String.Empty;

                    contadorAluno++;
                    if (classCancelado != String.Empty)
                    {
                        classeZebrado = String.Empty;
                        contadorAlunoCancelado++;
                    }
                    else
                    {
                        contadorAlunoMatriculado++;
                        linhaNavegacao++;
                        dadosNavegacaoLinha = String.Concat("data-linha-navegacao=\"", linhaNavegacao, "\"");
                    }
                    string classeItem = String.Concat("class=\"", classeZebrado, classCancelado, '"');
                %>
                <tr <%=classeItem%>>
                    <% 
                    if (item.SituacaoMatriculado)
                    { 
                    %>
                    <td>
                        <%=item.Nome%>
                        <input type="hidden" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].Codigo"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].Codigo %>" />
                        <input type="hidden" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].Nome"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].Nome %>" />
                        <input type="hidden" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].Id"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].Id %>" />
                    </td>
                    <td class="situacao">
                        <%
                        if (!String.IsNullOrEmpty(item.DescricaoSituacao.Trim()))
                        { 
                        %>
                        <div class="descricao-situacao">
                            <div>
                                <%=item.DescricaoSituacao%></div>
                            <img class="aba" src="<%=Url.Content("~/Imagens/aba_balao.png") %>" />
                        </div>
                        <%
                    } 
                        %>
                        <%=item.Situacao%>
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="1">
                        <input type="text" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].NotaProva"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].NotaProva %>"
                            class="input-nota" data-navegar="" />
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="2">
                        <input type="text" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].Faltas"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].Faltas %>"
                            class="input-faltas" data-navegar="" maxlength="3" />
                    </td>
                    <td class="centralizado-pequeno">
                        <input type="checkbox" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].RecuperacaoParalela"
                            value="true" class="input-recuperacao-paralela" <% if (Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].RecuperacaoParalela) { %>
                            checked="checked" <% } %> />
                        <input type="hidden" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].RecuperacaoParalela"
                            value="false" />
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="3">
                        <input type="text" class="inputnotarecuperacao" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].NotaRecuperacao"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].NotaRecuperacao %>"
                            data-navegar="" />
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="4">
                        <input type="text" readonly="readonly" disabled="disabled" class="inputconceito"
                            name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].Nota"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].Nota %>"
                            data-navegar="" />
                    </td>
                    <td class="centralizado-pequeno">
                        <input type="checkbox" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].SemAvaliacao"
                            value="true" class="input-sem-avaliacao" onfocus="DesabilitaFocus(this)" <% if(Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].SemAvaliacao) { %>
                            checked="checked" <% } %> />
                        <input type="hidden" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].SemAvaliacao"
                            value="false" />
                    </td>
                    <td>
                        <%=Html.DropDownList(
                                String.Concat("ListaItemLancamentoNotaFrequenciaAluno[", contadorAlunoMatriculado, "].CodigoJustificativa"),
                                new SelectList(
                                    Model.ListaItemJustificativa,
                                    "Codigo",
                                    "Descricao",
                                    Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].CodigoJustificativa
                                ),
                                "<Selecione>",
                                new
                                {
                                    @class = "inputLancamento100",
                                    @Title = Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].DescricaoJustificativa
                                }
                            )%>
                        <input type="hidden" class="inputLicenca" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].ExibeMensagemAfastamentoMedico"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].ExibeMensagemAfastamentoMedico %>" />
                    </td>
                    <%          }
                    else
                    { 
                    %>
                    <td>
                        <%=item.Nome%>
                    </td>
                    <td class="situacao">
                        <% 
                    if (!String.IsNullOrEmpty(item.DescricaoSituacao.Trim()))
                    { 
                        %>
                        <div class="descricao-situacao">
                            <div>
                                <%=item.DescricaoSituacao%></div>
                            <img class="aba" src="<%=Url.Content("~/Imagens/aba_balao.png") %>" />
                        </div>
                        <%
                                } 
                        %>
                        <%=item.Situacao%>
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="1">
                        <input type="text" value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].NotaProva %>"
                            class="input-nota" data-navegar="" />
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="2">
                        <input type="text" value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].Faltas %>"
                            class="input-faltas" data-navegar="" maxlength="3" />
                    </td>
                    <td class="centralizado-pequeno">
                        <input type="checkbox" value="true" class="input-recuperacao-paralela" <% if(Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].RecuperacaoParalela) { %>checked="checked"
                            <% } %> />
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="1">
                        <input type="text" class="inputnotarecuperacao" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].NotaRecuperacao"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].NotaRecuperacao %>"
                            data-navegar="" />
                    </td>
                    <td class="centralizado-micro" <%=dadosNavegacaoLinha %> data-coluna-navegacao="1">
                        <input type="text" class="inputconceito" name="ListaItemLancamentoNotaFrequenciaAluno[<%=contadorAlunoMatriculado %>].Nota"
                            value="<%=Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].Nota %>"
                            data-navegar="" />
                    </td>
                    <td class="centralizado-pequeno">
                        <input type="checkbox" value="true" class="input-sem-avaliacao" <% if(Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].SemAvaliacao) { %>checked="checked"
                            <% } %> />
                    </td>
                    <td>
                        <%=Html.DropDownList(
                                String.Concat("Cancelado", contadorAlunoCancelado),
                                new SelectList(
                                    Model.ListaItemJustificativa,
                                    "Codigo",
                                    "Descricao",
                                    Model.ListaItemLancamentoNotaFrequenciaAluno[contadorAluno].CodigoJustificativa
                                ),
                                "<Selecione>",
                                new { @class = "inputLancamento100" }
                            )%>
                    </td>
                    <%      
                }
                    %>
                </tr>
                <%
                }
                %>
            </tbody>
        </table>
        <div class="botoes-acoes">
            <button id="bt-imprimir-filipeta">
                <img src="<%=Url.Content("~/Imagens/ico_impressora.png") %>" alt="Imprimir filipeta"
                    title="Imprimir filipeta" />
                Imprimir filipeta
            </button>
            <%
                    

                if (Model.HabilitaSolicitacaoDeLancamento)
                { 
            %>
            <input type="button" id="bt-solicitar-reabertura" value="Solicitar reabertura do bimestre/trimestre para lançamento" />
            <div id="painel-justificativa">
                <label for="txa-justificativa-reabertura-bimestre">
                    Descreva sua justificativa para reabrir o lançamento de notas deste bimestre/trimestre:</label>
                <textarea name="JustificativaSolicitacaoReaberturaBimestre" id="txa-justificativa-reabertura-bimestre"></textarea>
                <small>Informe no máximo 250 caracteres</small>
                <div class="botoes-acoes">
                    <input type="button" id="bt-cancelar-justificativa" value="Cancelar" />
                    <input type="button" id="bt-confirmar-justificativa" value="Confirmar" />
                </div>
            </div>
            <% 
                }

                if (Model.HabilitaLancamentoNotas)
                { 
            %>
            <button value="Salvar" id="bt-salvar-lancamento" onclick="atualizaCampoMaterialEstudo(); Bloqueio()">
                <img src="<%=Url.Content("~/Imagens/ico_salvar.png")%>" title="Salvar" alt="Salvar" />
                Salvar
            </button>
            <button value="Cancelar" id="bt-cancelar-lancamento" onclick="atualizaCampoMaterialEstudo();">
                <img src="<%=Url.Content("~/Imagens/ico_voltar.png")%>" title="Cancelar" alt="Cancelar" />
                Cancelar
            </button>
            <% 
                }
                else
                { 
            %>
            <button value="Cancelar" id="bt-cancelar-lancamento">
                <img src="<%=Url.Content("~/Imagens/ico_voltar.png")%>" title="Cancelar" alt="Voltar" />
                Voltar
            </button>
            <% 
                }
            %>
        </div>
        <%
            } %>
    </div>
    </form> </div>
</asp:Content>
