<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SitePublic.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.LoginInicialViewModel>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
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
                    Efetue seu login no 
                    <br>
                    Docente Online
                </h3>
               
               <%=Html.ValidationSummary("", new { @class = "login-erro"}) %>
                <form id="frm-login" action="<%=Url.Action("Loga","Login") %>" method="post">
                <p>
                    <label for="LoginMatricula" tabindex="-1">ID Funcional/Vínculo:</label>
                    <input type="text" name="IdFuncional" id="LoginMatricula" class="login-forms" title="ID Funcional/Vínculo" value="<%=Model.LoginEntraModelo.IdFuncional %>"
                        maxlength="15" />
                </p>
                <p>
                    <label for="LoginSenha" tabindex="-1">Senha do Docente On-Line:</label>
                    <input name="Senha" id="LoginSenha" class="login-forms" type="password" maxlength="15" title="Senha" autocomplete="off" />
                </p>
                <p>
                    <img id="ImagemCaptcha" alt="ImagemCaptcha" src="<%=Url.Action("GetCaptcha","Login") %>" /><br />
                    <%--<%=Html.ActionLink("Trocar imagem", "TrocaImagem", Model.LoginEntraModelo)%> --%>
                    <%--<a href="<%=Url.Action("TrocaImagem","Login")%>">Trocar imagem</a> --%>
                    <a href="#" class="troca-captcha" onclick="TrocaImagem()">Trocar imagem</a>
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
                    <a href="<%=Url.Action("RedefineSenha") %>">Esqueci minha Senha</a><br />                
                    <a href="<%=Url.Action("PedidoUsuario") %>">Esqueci meu ID Funcional/Vínculo</a>
                </div>

                <%--<p class="esqueci-senha">
				    Se você esqueceu sua senha, .
                </p>--%>
            </div>
        </div>
    </div>
    <% /*A chamada do script está no final da página para nao precisar 
    * carregar o jquery só pra detectar o "document.ready" na pagina inicial 
    * e ganhar uns KBs no tamanho desta página*/ %>
    <script language="javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "login-inicial" }) %>"></script>
</asp:Content>
