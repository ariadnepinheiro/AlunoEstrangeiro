<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.DadosDocenteViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "acompanhamentoclassroom"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "dadosdocente-inicial" }) %>"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
    <div>
        <div class="cabecalho-impressao">
            <table>
                <tr>
                    <td><img src="<%=Url.Content("~/Imagens/logo.gif")%>" class="logo-impressao"></td>
                    <td><%=Model.TituloDaPagina %></td>
                </tr>
            </table>
        </div>
    </div>
    
     <% if (Model.EhPeriodoCampanhaLancamentoNotas && Model.BloqueiaDadosDocentesEmCampanha)
        { %>
        
            <div class="tabela-padrao">
                <p class="mensagem-inicial">
                    <%= ViewData["MensagemBloqueioCampanha"].ToString() %>
                </p>
            </div>

    <%  } else { %>
    
            <div class="tabela-padrao">
            <p class="mensagem-inicial">
                <%= Model.MensagemInicial %>
            </p>
        </div>

            <div class="tabela-padrao">
        <div class="topo-padrao">
            <table>
                <thead>
                    <tr>
                        <td colspan="2">
                            DADOS PESSOAIS
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            Nome Completo:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Nome%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nome Social:
                        </td>
                        <td>
                            <%=Model.DadosGerais.NomeSocial%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Data de Nascimento:
                        </td>
                        <td>
                            <%=Model.DadosGerais.DataNasc%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cor/Raça:
                        </td>
                        <td>
                            <%=Model.DadosGerais.CorRaca%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sexo:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Sexo%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Necessidade Especial:
                        </td>
                        <td>
                            <%=Model.DadosGerais.NecessidadeEspecial%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Estado Civil:
                        </td>
                        <td>
                            <%=Model.DadosGerais.EstadoCivil%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            País de Nascimento:
                        </td>
                        <td>
                            <%=Model.DadosGerais.PaisNasc%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nacionalidade:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Nacionalidade%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Naturalidade:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Naturalidade%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Estado:
                        </td>
                        <td>
                            <%=Model.DadosGerais.UFNascimento%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="topo-padrao">
            <table>
                <thead>
                    <tr>
                        <td colspan="2">
                            ENDEREÇO
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            País:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Pais%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            CEP:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Cep%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Município:
                        </td>
                        <td>
                            <%=Model.DadosGerais.EndMunicipio%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Estado:
                        </td>
                        <td>
                            <%=Model.DadosGerais.UFEndereco%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Endereço:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Endereco%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Número:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Numero%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Complemento:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Complemento%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bairro:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Bairro%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Localização/Zona de Residência:
                        </td>
                        <td>
                            <%=Model.DadosGerais.ZonaResidencial%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Telefone Fixo:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Telefone%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Celular:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Celular%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            E-mail Institucional:
                        </td>
                        <td>
                            <%=Model.DadosGerais.EmailInterno%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            E-mail Educa:
                        </td>
                        <td>
                            <%=Model.DadosGerais.EmailGoogle%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            E-mail Alternativo:
                        </td>
                        <td>
                            <%=Model.DadosGerais.Email%>
                        </td>
                    </tr>
                </tbody>            
            </table>
        </div>
        <div class="topo-padrao">
            <table>
                <thead>
                    <tr>
                        <td colspan="2">
                            DOCUMENTO/IDENTIDADE
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            RG Tipo:
                        </td>
                        <td>
                            <%=Model.DadosGerais.RGTipo%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Número:
                        </td>
                        <td>
                            <%=Model.DadosGerais.RGNumero%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Estado:
                        </td>
                        <td>
                            <%=Model.DadosGerais.RGUF%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Órgão Emissor:
                        </td>
                        <td>
                            <%=Model.DadosGerais.RGEmissor%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Data de Expedição:
                        </td>
                        <td>
                            <%=Model.DadosGerais.RGDtExp%>
                        </td>
                    </tr>      
                </tbody>      
            </table>
        </div>
        <div class="topo-padrao">
            <table>
                <thead>
                    <tr>
                        <td colspan="2">
                            DOCUMENTOS/CPF
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            CPF:
                        </td>
                        <td>
                            <%=Model.DadosGerais.CPF%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    
        <%
            int qtd = 0;
            IDictionary<string, string> camposFormacao = new Dictionary<string, string>();

            camposFormacao.Add("Escolaridade", "Escolaridade");
            camposFormacao.Add("SituacaoCurso", "Situação do Curso");
            camposFormacao.Add("AreaCurso", "Área do curso");
            camposFormacao.Add("Curso", "Curso");
            camposFormacao.Add("FormacaoComplementacaoPedagogica", "Formação/Complementação Pedagógica");
            camposFormacao.Add("AnoInicio", "Ano de Inicio");
            camposFormacao.Add("AnoConclusao", "Ano de Conclusão");
            camposFormacao.Add("CodInstituicao", "Código Instituição");
            camposFormacao.Add("Instituicao", "Nome da Instituição");
            camposFormacao.Add("DocComprobatorio", "Documentos Comprobatórios");

            if (Model.Graduacoes.Count > 0)
            {
                foreach (var graduacao in Model.Graduacoes)
                {
                    qtd++;
                    
        %>
                    <div class="topo-padrao div-graduacao">
                        <table>
                            <thead>
                                <tr>
                                    <td colspan="2">
                                        FORMAÇÃO PESSOAL/GRADUAÇÃO <%= qtd%>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            <%
                    foreach (var campo in camposFormacao)
                    {                                
                                %> 
                                    <tr>
                                        <td><%= campo.Value%>:</td>
                                        <td><%= graduacao.GetType().GetProperty(campo.Key).GetValue(graduacao, null)%></td>
                                    </tr>
                            <%  } %>
                            </tbody>
                        </table>
                    </div>
        <%
                }
            }

            if (Model.PosGraduacoes.Count > 0)
            {
                qtd = 0;

                foreach (var posGraduacao in Model.PosGraduacoes)
                {
                    qtd++;
                    
        %>
                    <div class="topo-padrao div-pos-graduacao">
                        <table>
                            <thead>
                                <tr>
                                    <td colspan="2">
                                        FORMAÇÃO PESSOAL/PÓS-GRADUAÇÃO <%= qtd%>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            <%
                    foreach (var campo in camposFormacao)
                    {                                
                             %> 
                                    <tr>
                                        <td><%= campo.Value%>:</td>
                                        <td><%= posGraduacao.GetType().GetProperty(campo.Key).GetValue(posGraduacao, null)%></td>
                                    </tr>
                            <%  } %>
                            </tbody>
                        </table>
                    </div>
         <%     
                }
            }

            if (Model.Capacitacoes.Count > 0)
            {
                IDictionary<string, string> camposCapacitacao = new Dictionary<string, string>();

                camposCapacitacao.Add("OferecidoSEEDUC", "Oferecido SEEDUC");
                camposCapacitacao.Add("Capacitacao", "Curso/Capacitação");
                camposCapacitacao.Add("TipoCurso", "Tipo do Curso");
                camposCapacitacao.Add("AreaConhecimento", "Área de Conhecimento");
                camposCapacitacao.Add("NomeInstituicao", "Nome da Instituição");
                camposCapacitacao.Add("CargaHoraria", "Carga Horária");
                camposCapacitacao.Add("DataConclusao", "Data Conclusão");

                qtd = 0;

                foreach (var capacitacao in Model.Capacitacoes)
                {
                    qtd++;
                    
        %>
                    <div class="topo-padrao div-capacitacao">
                        <table>
                            <thead>
                                <tr>
                                    <td colspan="2">
                                        CAPACITAÇÃO <%= qtd%>
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            <%
                    foreach (var campo in camposCapacitacao)
                    {                                
                             %> 
                                    <tr>
                                        <td><%= campo.Value%>:</td>
                                        <%
                        var objVal = capacitacao.GetType().GetProperty(campo.Key).GetValue(capacitacao, null);
                                        %>
                                        <td><%= (campo.Key == "DataConclusao") ? ((DateTime)objVal).ToString("dd/MM/yyyy") : objVal%></td>
                                    </tr>
                            <%  } %>
                            </tbody>
                        </table>
                    </div>
         <%     
                }
            } 
         %>

        <div class="div-assinatura">
            <hr />
            <label class="lbl-assinatura"><%= Model.DadosGerais.Nome%></label>
        </div>

        <div class="topo-padrao div-bt-imprimir">
            <button id="bt-imprimir">
                <img src="<%= Url.Content("~/Imagens/ico_impressora.png") %>" alt="Imprimir"
                    title="Imprimir" />
                Imprimir
            </button>
        </div>

        <div class="div-dados-impressao">
            <label id="lbl-data-hora-impressao"></label>
        </div>
    </div>

    <%  } %>
</asp:Content>
