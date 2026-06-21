<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SitePublic.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.RedefineSenhaViewModel>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    Redefinição de Senha
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "login-inicial"}) %>" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
    <div class="conteudo">
        <div class="painel-login">
            <div class="caixa-login">
                <h3>
                  <img src="<%=Url.Content("~/Imagens/icone-professor.png")%>" alt="">
                    <span class="icone-card-login"></span>
                    Recupere a sua senha do 
                    <br>
                    Docente Online
                </h3>
                <%=Html.ValidationSummary("", new { @class = "login-erro"}) %>
                <form id="frm-login" action="<%=Url.Action("RedefineSenha") %>" method="post">
                <p>
                    <label for="LoginMatricula" tabindex="-1">ID Funcional/Vínculo:</label>
                    <input type="text" name="IdFuncional" id="LoginMatricula" class="login-forms" title="IdFuncional" value="<%=Model.IdFuncional %>"
                        maxlength="20" />
                </p>
                <p>&nbsp;</p>
                <p>
                    <img id="ImagemCaptcha" alt="ImagemCaptcha" src="<%=Url.Action("GetCaptcha","Login") %>" /><br />
                    <a href="<%= Url.Action("RedefineSenha") %>" class="troca-captcha">Trocar imagem</a>
                </p>
                <p>
                    <label for="Codigo" tabindex="-1">Digite o código da imagem acima:</label>
                </p>
                <p>
                    <input type="text" name="Codigo" id="Codigo" class="login-forms" title="Codigo" maxlength="6" />
                </p>
                <p>
                    <input type="image" id="btLoginEntrar" onclick="Bloqueio()" src="<%=Url.Content("~/Imagens/btn_EntrarBranco.png") %>" />
                </p>
                </form>

                <div class="redefinir-senha">
                    <a href="<%=Url.Action("Inicial") %>">Retornar para Login</a>
                </div>
            </div>
        </div>
    </div>
    <% /*A chamada do script está no final da página para nao precisar 
    * carregar o jquery só pra detectar o "document.ready" na pagina inicial 
    * e ganhar uns KBs no tamanho desta página*/ %>
    <script language="javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "login-inicial" }) %>"></script>
</asp:Content>
