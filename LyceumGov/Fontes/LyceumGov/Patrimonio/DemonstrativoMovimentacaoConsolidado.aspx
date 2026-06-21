<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DemonstrativoMovimentacaoConsolidado.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.DemonstrativoMovimentacaoConsolidado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="../Scripts/themes/RelatorioPatrimonio.css" rel="stylesheet" type="text/css" />
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
                    frameDoc.document.write('<link href="../Scripts/themes/RelatorioPatrimonio.css" rel="stylesheet" type="text/css" />');
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
                    var printWindow = window.open(windowUrl, windowName, 'width=1850,height=800');

                    printWindow.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    printWindow.document.write('<link href="../Scripts/themes/RelatorioPatrimonio.css" rel="stylesheet" type="text/css" />');
                    printWindow.document.write(printContent.innerHTML);
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();
                }
            });
        });   
    </script>

    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar as movimentações"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Data Início do Período:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Data Fim do Período:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataFim" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtDataFim" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <asp:ImageButton ID="btnPesquisar" runat="server" ImageUrl="~/Images/bot_buscar.png"
                        OnClick="btnPesquisar_Click" ValidationGroup="ConfirmarForm" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlImprimir" Visible="false" Width="500px">
        <div>
            <table>
                <tr>
                    <td>
                        <input type="button" id="btnImprimir" style="background-image: url(../Images/bot_imprimir.png);
                            width: 100px; height: 27px; background-color: transparent!important;" />
                    </td>
                    <td>
                        <asp:ImageButton ID="btnExportarPDF" runat="server" ImageAlign="Right" ToolTip="Export"
                            OnClick="btnExportarPDF_Click" ImageUrl="~/Images/bot_PDF.png" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div id="divPrincipal" runat="server" visible="false">
        <table align="center">
            <tr align="center">
                <td align="center">
                    <img src="~/Images/logo_govrj.jpg" id="Image1" runat="server" />
                </td>
            </tr>
            <tr align="center">
                <td align="center" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    border-color: #000000; font-weight: bold;">
                    GOVERNO DO ESTADO DO RIO DE JANEIRO
                </td>
            </tr>
            <tr align="center">
                <td align="center" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    border-color: #000000; font-weight: bold;">
                    SECRETARIA DE ESTADO DE EDUCAÇÃO
                </td>
            </tr>
        </table>
        <br />
        <div runat="server" id="divControle">
        </div>
    </div>
</asp:Content>
