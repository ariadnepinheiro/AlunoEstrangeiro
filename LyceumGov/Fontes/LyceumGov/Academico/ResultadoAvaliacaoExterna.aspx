<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ResultadoAvaliacaoExterna.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ResultadoAvaliacaoExterna" %>

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
        <iframe id="frResultado" runat="server" width="80%" height="800px" frameborder="1"
            scrolling="auto" ></iframe>
    </div>
</asp:Content>
