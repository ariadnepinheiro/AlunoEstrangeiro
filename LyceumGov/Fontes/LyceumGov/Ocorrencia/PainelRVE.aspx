<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PainelRVE.aspx.cs" Inherits="Techne.Lyceum.Net.Ocorrencia.PainelRVE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
            text-align: justify;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div id="IdentificacaoSuperior">
        <%--<iframe id="frResultado" runat="server" width="80%" height="800px" frameborder="1"
            scrolling="auto" ></iframe>--%>
        <a target="_blank" href="https://app.powerbi.com/groups/me/apps/19970ee7-d42c-4f76-99d0-786d5ee15466/reports/">
            <img src="../Images/CapaRVE.png" alt="Link para painel RVE">
        </a>
    </div>
</asp:Content>
