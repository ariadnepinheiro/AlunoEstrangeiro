<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/ProcessoSeletivoMaster.Master" AutoEventWireup="true"
    CodeBehind="Relatorios.aspx.cs" Inherits="Techne.Lyceum.Net.Relatorio.Relatorios" %>

<asp:Content ID="ctRelatorioDiretor" ContentPlaceHolderID="cphFormulario" runat="server">
    <div>
        <asp:Label ID="lblMsg" runat="server" BackColor="#FFC0C0"></asp:Label>
    </div>
    <div>
        <rsweb:reportviewer id="rptViewer" runat="server" width="100%" font-names="Verdana"
            font-size="8pt" processingmode="Remote" borderstyle="None" 
            borderwidth="1px" ShowPrintButton="true" Height="900px" ShowExportControls="true" >
        </rsweb:reportviewer>
    </div>
</asp:Content>
