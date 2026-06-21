<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AvisoTransferenciaBens.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.AvisoTransferenciaBens" %>

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
                    var printWindow = window.open(windowUrl, windowName, 'width=1850,height=800');

                    printWindow.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    printWindow.document.write(printContent.innerHTML);
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();
                }
            });
        });   
    </script>

    <asp:Panel runat="server" ID="pnlImprimir" Visible="false" Width="500px">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:ImageButton ID="btnCancel" runat="server" SkinID="Voltar" OnClick="btnCancel_Click" />
                    </td>
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
    <div id="divPrincipal" runat="server">
        <table align="center">
            <tr >
                <td colspan="2" align="center">
                    <img src="~/Images/logo_govrj.jpg" id="Image1" runat="server" />
                </td>
            </tr>
            <tr >
                <td align="center" colspan="2" style="font-weight: bold; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px;">
                    GOVERNO DO ESTADO DO RIO DE JANEIRO
                </td>
            </tr>
            <tr >
                <td align="center" colspan="2" style="font-weight: bold; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px;">
                    SECRETARIA DE ESTADO DE EDUCAÇÃO
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="width: 212px;">
                    &nbsp;
                </td>
            </tr>
            <tr >
                <td align="center" colspan="2" style="font-weight: bold; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    AVISO DE TRANSFERÊNCIA DE BENS MÓVEIS                
                    <label id="lblNumero" runat="server">
                        Nº
                    </label>
                    <label id="lblLote" runat="server">
                    </label>/
                    <label id="lblAno" runat="server">
                    </label>
                </td>
            </tr>
        </table>
        <br />
        <table align="center" style="width: 913px">
            <tr>
                <td align="justify" style="width: 140px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    font-weight: bold">
                    <label id="lblCedente" runat="server">
                        UNIDADE CEDENTE:
                    </label>
                </td>
                <td style="width: 626px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    font-weight: bold">
                    <label id="lblUnidadeCedente" runat="server">
                    </label>
                </td>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; font-weight: bold">
                    <label id="lblUg" runat="server">
                        UG.:
                    </label>
                </td>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; font-weight: bold">
                    <label id="lblUACedente" runat="server">
                    </label>
                </td>
            </tr>
            <tr>
                <td align="justify" style="width: 140px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    font-weight: bold">
                    <label id="Label2" runat="server">
                        UNIDADE DESTINATÁRIA:</label>
                </td>
                <td style="width: 626px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    font-weight: bold">
                    <label id="lblUnidadeDestino" runat="server">
                    </label>
                </td>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; font-weight: bold">
                    <label id="Label4" runat="server">
                        UG.:
                    </label>
                </td>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; font-weight: bold">
                    <label id="lblUADestino" runat="server">
                    </label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <div runat="server" id="divControle">
        </div>
        <br />
        <table align="center" style="width: 921px">
            <tr>
                <td style="width: 82px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    <label id="Label5" runat="server">
                        CEDIDO EM</label>
                </td>
                <td style="width: 119px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    <label id="lblDataSolicitacao" runat="server">
                    </label>
                </td>
                <td style="width: 408px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;"
                    align="center">
                    <label id="Label6" runat="server">
                        RESPONSÁVEL PELOS BENS PATRIMONIAIS</label>
                </td>
                <td align="center">
                    &nbsp;
                </td>
                <td align="center" style="width: 236px; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px;">
                    <label id="lblVisto" runat="server">
                        Visto:</label>
                </td>
            </tr>
            <tr>
                <td style="width: 82px;">
                    &nbsp;
                </td>
                <td style="width: 119px;">
                    &nbsp;
                </td>
                <td style="width: 408px;">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="width: 236px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 82px;">
                    &nbsp;
                </td>
                <td style="width: 119px;">
                    &nbsp;
                </td>
                <td>
                    <hr width="100%" color="#000000" size="1px" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <hr width="100%" color="#000000" size="1px" />
                </td>
            </tr>
            <tr>
                <td style="width: 82px;">
                    &nbsp;
                </td>
                <td style="width: 119px;">
                    &nbsp;
                </td>
                <td style="width: 408px;" align="center">
                    &nbsp;
                </td>
                <td align="center">
                    &nbsp;
                </td>
                <td align="center" style="width: 236px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 82px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    <label id="Label8" runat="server">
                        RECEBIDO EM:
                    </label>
                </td>
                <td style="width: 119px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; /&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    /&nbsp;
                </td>
                <td style="width: 408px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;"
                    align="center">
                    <label id="Label7" runat="server">
                        RESPONSÁVEL PELOS BENS PATRIMONIAIS</label>
                </td>
                <td align="center">
                    &nbsp;
                </td>
                <td align="center" style="width: 236px; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px;">
                    <label id="lblVisto0" runat="server">
                        Visto:</label>
                </td>
            </tr>
            <tr>
                <td style="width: 82px;">
                    &nbsp;
                </td>
                <td style="width: 119px;">
                    &nbsp;
                </td>
                <td style="width: 408px;">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="width: 236px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 82px;">
                    &nbsp;
                </td>
                <td style="width: 119px;">
                    &nbsp;
                </td>
                <td>
                    <hr width="100%" color="#000000" size="1px" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <hr width="100%" color="#000000" size="1px" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
