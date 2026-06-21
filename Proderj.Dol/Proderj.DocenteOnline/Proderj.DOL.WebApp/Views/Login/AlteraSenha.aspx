<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.AlteraSenhaViewModel>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "login-alterasenha"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "login-alterasenha" }) %>"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="conteudo">
        <div class="painel-login">
            <div class="caixa-login">
                <div class="aviso">
                    <h3>
                        <img src="<%=Url.Content("~/Imagens/sel.png")%>" alt="Indicador"><%=Model.TituloDaPagina %>
                    </h3>
                </div>
                <%=Html.ValidationSummary("", new { @class = "login-erro"}) %>
                <form id="frm-login" method="post">
                <input type="hidden" name="matricula" value="<%=Model.Matricula %>" />
                <input type="hidden" name="idFuncional" value="<%=Model.IdFuncional %>" />
                <input type="hidden" name="vinculo" value="<%=Model.Vinculo %>" />
                <p>
                    <label>
                        ID/Vínculo:</label>
                    <label>
                        <%=Model.IdFuncional %>/<%=Model.Vinculo %></label>
                </p>
                <p>
                    <label for="SenhaAtual" tabindex="-1">
                        Senha Atual:</label>
                    <input type="password" name="SenhaAtual" id="SenhaAtual" maxlength="15" title="Senha Atual" />
                </p>
                <p>
                    <label for="SenhaNova" tabindex="-1">
                        Senha Nova:</label>
                    <input type="password" name="SenhaNova" id="SenhaNova" maxlength="15" title="Senha Nova" />
                </p>
                <p>
                    <label for="SenhaNovaConfirmacao" tabindex="-1">
                        Senha Nova (Confirmação):</label>
                    <input type="password" name="SenhaNovaConfirmacao" id="SenhaNovaConfirmacao" maxlength="15"
                        title="Senha Nova (Confirmação)" />
                </p>
                <div class="botoes">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <input type="image" id="bt-confirmar" src="<%=Url.Content("~/Imagens/bot_confirmar.png") %>" />
                            </td>
                            <%--<td>
                                <input type="image" id="bt-cancelar" src="<%=Url.Content("~/Imagens/bot_cancelar.png") %>" />
                            </td>--%>
                        </tr>
                    </table>
                </div>
                </form>
            </div>
        </div>
    </div>
</asp:Content>
