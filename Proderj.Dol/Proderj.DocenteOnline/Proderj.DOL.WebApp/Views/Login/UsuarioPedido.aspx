<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SitePublic.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.UsuarioPedidoViewModel>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    Esqueci meu ID Funcional/Vínculo
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
                    Esqueci meu ID Funcional/Vínculo:
                </h3>
                
                <p style="text-align: center; font-weight: bold;">
                    <%= Model.Mensagem %>
                </p>
                <p>&nbsp;</p>

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
