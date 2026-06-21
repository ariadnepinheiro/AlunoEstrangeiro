<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Relatorios.aspx.cs" Inherits="Techne.Lyceum.Net.Relatorio.Relatorios" %>

<asp:Content ID="ctRelatorioDiretor" ContentPlaceHolderID="cphFormulario" runat="server">
    <div>
        <asp:Label ID="lblMsg" runat="server" BackColor="#FFC0C0"></asp:Label>
    </div>
    <div style="width: 100%; height: 100%">
        <rsweb:reportviewer id="rptViewer" runat="server" width="100%" font-names="Verdana"
            font-size="8pt" processingmode="Remote" borderstyle="None" borderwidth="1px">
        </rsweb:reportviewer>
    </div>
</asp:Content>
