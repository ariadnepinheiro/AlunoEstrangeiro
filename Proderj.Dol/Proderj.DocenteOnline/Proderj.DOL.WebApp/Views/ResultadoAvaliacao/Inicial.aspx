<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.ResultadoAvaliacaoViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <%=Model.TituloDaPagina %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "codigo-armazem-do-livro"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "codigo-armazem-do-livro-inicial" }) %>"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
    <% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
    <div  style="
        text-align: center;
        width: 90%;
        background-color: #ededed;
        display: inline-block;
        position: relative;
        margin: 5%;
        line-height: 50px;
        font-size: 20px;
        padding: 20px 0;
        border-radius: 15px;">
        SERVIÇO INDISPONÍVEL NO MOMENTO
        <%-- <iframe id="frResultado" runat="server" width="80%" height="800px" frameborder="1" src="https://app.powerbi.com/view?r=eyJrIjoiZGZiYzBmNzctYzQyYS00ZTY4LWE5NzYtNTVhZGMzNDQ3ODE5IiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9"
            scrolling="auto"></iframe>--%>
    </div>
</asp:Content>
