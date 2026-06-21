<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ControleMensalGestor.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.ControleMensalGestor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        $(function() {
            $("#btnImprimir").click(function() {

                var nav = navigator.userAgent.toLowerCase();
                var printContent = document.getElementById("<%=divPrincipal.ClientID %>");
                var title = document.title;

                if (nav.indexOf("chrome") != -1) {
                    var frame1 = $('<iframe />');
                    frame1[0].name = "frame1";
                    frame1.css({ "position": "absolute", "top": "-1000000px" });
                    $("body").append(frame1);
                    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                    frameDoc.document.open();
                    frameDoc.document.write('<html><head><title>' + title + '</title>');
                    frameDoc.document.write('</head><body>');
                    frameDoc.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    frameDoc.document.write(printContent.innerHTML);
                    frameDoc.document.write('</body></html>');
                    frameDoc.document.close();
                    setTimeout(function() {
                        window.frames["frame1"].focus();
                        window.frames["frame1"].print();
                        frame1.remove();
                    }, 500);
                }
                else {

                    var windowUrl = 'about:blank';
                    var windowName = 'Impressão';
                    var printWindow = window.open(windowUrl, windowName, 'width=850,height=800');
                    printWindow.document.write('<html><head><title>' + title + '</title><link rel="stylesheet" type="text/css" href="../LyceumNet.css" media="print" ></head><body>');
                    printWindow.document.write(printContent.innerHTML);
                    printWindow.document.write('</body></html>');
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();
                }
            });
        });   
    </script>

    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar o controle mensal"
        Width="500px">
        <div>
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"
                            Font-Bold="true">                                   
                        </asp:Label>
                    </td>
                    <td width="20%">
                        <asp:DropDownList Height="20px" ID="ddlAno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                            DataTextField="ano" DataValueField="ano" SkinID="a">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblMes" runat="server" Text="Mês:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMes" Height="20px" runat="server" AutoPostBack="True" DataTextField="DESCRICAO"
                            DataValueField="CODIGO" OnSelectedIndexChanged="ddlMes_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="4">
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel runat="server" ID="pnlImprimir" Visible="false" Width="500px">
        <div>
            <table>
                <tr>
                    <td>
                        <input type="button" id="btnImprimir" style="background-image: url(../Images/bot_imprimir.png);
                            width: 100px; height: 27px; background-color: transparent!important;" />
                    </td>
                    <td>
                        <asp:ImageButton ID="btnExportarPDF" runat="server" ImageAlign="Right" ToolTip="Exportar para pdf"
                            OnClick="btnExportarPDF_Click" ImageUrl="~/Images/bot_PDF.png" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div id="divPrincipal" runat="server" visible="false">
        <table align="center">
            <tr align="center">
                <td align="center">
                    <img src="~/Images/logo_govrj.jpg" id="Image1" runat="server" />
                </td>
            </tr>
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;">
                    Secretaria de Estado de Educação
                </td>
            </tr>
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;
                    font-weight: bold;">
                    BENS PATRIMONIAIS - CONTROLE MENSAL DO GESTOR DE BENS MÓVEIS
                </td>
            </tr>
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;">
                    (Anexo II - IN 29/2014)
                </td>
            </tr>
            <tr align="center">
                <td style="font-weight: bold; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    border-color: #000000;">
                    <label id="lblReferencia" runat="server">
                    </label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <div runat="server" id="divControle">
        </div>
    </div>
</asp:Content>
