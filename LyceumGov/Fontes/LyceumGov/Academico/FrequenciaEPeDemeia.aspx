<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FrequenciaEPeDemeia.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.FrequenciaEPeDemeia" %>

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
    <a target="_blank" href="https://app.powerbi.com/groups/me/apps/8f5db9b8-c68d-46ad-91dc-8df972171813/reports/37aaaa2e-7203-4deb-a15c-34806dd4a752/ReportSectionb59d51d585a920a5d8eb?ctid=0c2829c9-41fa-4885-b057-a327fa5f37d4&experience=power-bi">
        <img src="../Images/PainelFrequencia.png" alt="Link para Frequência e Pé-de-Meia" />
    </a>
</asp:Content>
