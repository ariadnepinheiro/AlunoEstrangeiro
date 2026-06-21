<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SitePublic.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.SenhaRedefinidaViewModel>" %>

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
                    Redefinição de Senha:
                </h3>
                
                <p style="text-align: center; font-weight: bold;">
                    <%= Model.Mensagem %>
                </p>
                <p>&nbsp;</p>

                <div class="redefinir-senha">
                    <a href="<%=Url.Action("Inicial") %>">Retornar para Login</a><br />
                    <br />
                    <a href="<%=Url.Action("RedefineSenha") %>">Enviar novamente</a><br />
                    <br />
                    <p style="text-align: center; width: 100%; color: #FF0000; ForeColor="Red" font-size: 14px;">
                    Caso não tenha recebido a senha temporária, verifique a caixa de spam.
                    <br />
                    Se mesmo assim não encontrar, entre em contato através do site <a href="https://suporteti.educacao.rj.gov.br"
                        target="_blank">suporteti.educacao.rj.gov.br</a>
                </p>
                </div>
            </div>
        </div>
    </div>
    <% /*A chamada do script está no final da página para nao precisar 
    * carregar o jquery só pra detectar o "document.ready" na pagina inicial 
    * e ganhar uns KBs no tamanho desta página*/ %>
    <script language="javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "login-inicial" }) %>"></script>
</asp:Content>
