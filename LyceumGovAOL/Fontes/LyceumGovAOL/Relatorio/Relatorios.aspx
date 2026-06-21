<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Relatorios.aspx.cs" Inherits="Techne.Lyceum.Net.Relatorio.Relatorios" %>

<asp:Content ID="ctRelatorioDiretor" ContentPlaceHolderID="cphFormulario" runat="server">
    <div>
        <asp:Label ID="lblMsg" runat="server" BackColor="#FFC0C0"></asp:Label>
    </div>
    <div style="width: 100%; height: 100%">
        <rsweb:ReportViewer ID="rptViewer" runat="server" Height="550px" Width="100%" Font-Names="Verdana"
            Font-Size="8pt" ProcessingMode="Remote" BorderStyle="None" BorderWidth="1px">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
