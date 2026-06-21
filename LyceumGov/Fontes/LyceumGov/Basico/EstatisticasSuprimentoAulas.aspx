<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="EstatisticasSuprimentoAulas.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.EstatisticasSuprimentoAulas" %>

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
        <a target="_blank" href="https://app.powerbi.com/groups/me/apps/64ea3362-99e8-48ac-b830-3d104f24a53a/reports/d7a86e6d-d192-44d8-b500-c381b5ff9c3a/ReportSection3f1c883a5620d66ee9b4?experience=power-bi">
            <img src="../Images/capa_suprimento_aulas.png" alt="Link para Suprimento de Aulas">
        </a>
    </div>
</asp:Content>
