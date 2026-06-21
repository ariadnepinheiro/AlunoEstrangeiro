<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="RelatorioXML.aspx.cs" Inherits="Techne.Lyceum.Net.Relatorio.RelatorioXML" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Relatório</title>
</head>
<body approot='<%=Page.ResolveClientUrl("~/") %>'>
    
<script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/jquery-1.7.1.min.js") %>'></script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/jquery.jqGrid.js") %>'></script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/js/jqModal.js") %>'></script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/jquery.maskedinput-1.2.2.min.js") %>'></script>

    <script type="text/javascript" src='<%=Page.ResolveClientUrl("~/Scripts/MaskTypes.js") %>'></script>
    
    <form id="frmMain" runat="server">
    <script type="text/javascript">
<!--
        function printPartOfPage(elementId) {
            var printContent = document.getElementById(elementId);
            var windowUrl = 'about:blank';
            var windowName = 'Impressão';
            var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }
// -->
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div id="dvParametros" runat="server"></div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="bt_Voltar"/>
            <asp:Button ID="btnCarregar" runat="server" Text="Exibir Relatório" OnClick="bt_Click" />
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" OnClientClick="printPartOfPage('dvTexto')" />
            <div id="dvTexto" runat="server"></div>
            <%--<asp:Label ID="lblTexto" runat="server"></asp:Label>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    </form>
    </body>
</html>

