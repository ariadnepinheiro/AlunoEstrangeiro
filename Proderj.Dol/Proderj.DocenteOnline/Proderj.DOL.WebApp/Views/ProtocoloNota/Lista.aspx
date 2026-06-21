<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.ProtocoloNota.ProtocoloNotaListaViewModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "protocolonota-lista"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "protocolonota-lista" }) %>"></script>
    <script type="text/javascript">

	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Conteudo" runat="server" >
    <div id="geral" class="conteudo" runat="server">
    <form id="teste">
        <div class="tabela-padrao">
            <div class="tabela-cabecalho">
                <img class="aba-direita" src="<%=Url.Content("~/Imagens/tabela_cabecalho_abadir.gif")%>" />
                <div class="centro">
                    <img src="<%=Url.Content("~/Imagens/tabela_cabecalho_grid.png")%>" /></div>
                <div class="aba-esquerda">
                </div>
                <span>
                    <%=Model.TituloDaPagina %></span>
            </div>
            <table>
                <tr>
                    <td>
                        <label>
                            Ano:</label>
                    </td>
                    <td >
                        <%=Html.DropDownList("Ano", new SelectList(Model.ListaAno, "Ano", "Ano"), "-- Selecione um ano --", new { @class = "combo-media", id = "cmbAno" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Período:</label>
                    </td>
                    <td>
                        <select name="Periodo" class="combo-media" disabled="disabled" id="cmbPeriodo" onchange="Bloqueio()">
                            <option>-- Selecione um período --</option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        </form>
        <div class="tabela-padrao">
            <div class="tabela-cabecalho">
                <img class="aba-direita" src="<%=Url.Content("~/Imagens/tabela_cabecalho_abadir.gif")%>" />
                <div class="centro">
                    <img src="<%=Url.Content("~/Imagens/tabela_cabecalho_grid.png")%>" /></div>
                <div class="aba-esquerda">
                </div>
                <span>Protocolos</span>
            </div>
            <div id="tabela-protocolo" >
            </div>
        </div>
    </div>
</asp:Content>
