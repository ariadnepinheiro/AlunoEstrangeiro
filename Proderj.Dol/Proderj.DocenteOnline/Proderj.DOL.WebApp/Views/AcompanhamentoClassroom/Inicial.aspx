<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.AcompanhamentoClassroomViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "acompanhamentoclassroom"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "AcompanhamentoClassroom-Inicial" }) %>"></script>
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

    <div class="tabela-padrao">
    
        <% if (!string.IsNullOrWhiteSpace(Model.DadosGerais.EmailGoogle)) { %>
        
        <div class="topo-padrao">
            <table class="tabela-dados-docente">
                <thead>
                    <tr>
                        <td colspan="2">DADOS DOCENTE</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="campo-email-titulo">E-mail:</td>
                        <td class="campo-email-valor"><%=Model.DadosGerais.EmailGoogle%></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="topo-padrao">
            
            <p class="titulo-tabela">Turmas</p>

            <table class="tabela-turmas">
                <thead>
                    <tr>
                        <td class="campo-unidadeescolar-titulo">Unidade Escolar</td>
                        <td class="campo-turma-titulo">Turma</td>
                    </tr>
                </thead>
                <tbody>
                    <% if (Model.Turmas.Any()) { %>
                    <% foreach (var turma in Model.Turmas) { %>
                    <tr>
                        <td class="campo-unidadeescolar-valor"><%= turma.UnidadeEscolar %></td>
                        <td class="campo-turma-valor"><%= turma.Turma %></td>
                    </tr>
                    <% } %>
                    <% } else { %>
                    <tr><td colspan="2">Sem turmas cadastradas</td></tr>
                    <% } %>
                </tbody>            
            </table>
        </div>

        <div class="topo-padrao">

            <p class="titulo-tabela">Últimos Acessos</p>

            <table class="tabela-ultimosacessos">
                <thead>
                    <tr>
                        <td class="campo-datalogin-titulo">Data Login</td>
                    </tr>
                </thead>
                <tbody>
                    <% if (Model.Turmas.Any()) { %>
                    <% foreach (var ultimoAcesso in Model.UltimosAcessos) { %>
                    <tr>
                        <td align="center" class="campo-datalogin-valor"><%= ultimoAcesso.ToString("dd/MM/yyyy") %></td>
                    </tr>
                    <% } %>
                    <% } else { %>
                    <tr><td>Sem acessos registrados</td></tr>
                    <% } %>
                </tbody>      
            </table>
        </div>        

        <% } else { %>

        <div class="topo-padrao">
        
        <div style="
            text-align: center;
            width: 90%;
            background-color: #ededed;
            display: inline-block;
            position: relative;
            margin: 5%;
            line-height: 50px;
            font-size: 18px;
            padding: 20px 0;
            border-radius: 15px;">
            Docente não possui histórico de acesso ao Google Classroom.<br />
        </div>
        
        </div>

        <% } %>
    </div>

</asp:Content>
