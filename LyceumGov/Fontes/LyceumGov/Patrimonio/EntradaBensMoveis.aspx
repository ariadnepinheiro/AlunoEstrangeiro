<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="EntradaBensMoveis.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.EntradaBensMoveis" %>

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

    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar as entradas"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="lblUACedente" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <tweb:TSearch ID="tseUACedente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetorCedente"
                        AutoPostBack="true" OnTextChanged="tseUACedente_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label Font-Names="Verdana" ID="lblClassificacao" runat="server" Text="Classificação:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseClassificacao" runat="server" SqlSelect="SELECT CONTA, DESCRICAO,CLASSIFICACAOID FROM [LYCEUM].[Patrimonio].[CLASSIFICACAO]"
                        SqlOrder="CONTA" ColumnName="conta" Caption="" MaxLength="15" SqlWhere=" ativo=1"
                        DataType="Varchar" OnChanged="tseClassificacao_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Conta" FieldName="CONTA" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Mês Início do Período:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlMesInicial" runat="server" AutoPostBack="True" DataTextField="DESCRICAO"
                        DataValueField="CODIGO">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Mês Fim do Período:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMesFinal" runat="server" AutoPostBack="True" DataTextField="DESCRICAO"
                        DataValueField="CODIGO">
                    </asp:DropDownList>
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
                <td>
                    <img src="~/Images/logo_govrj.jpg" id="Image1" runat="server" />
                </td>
            </tr>
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;
                    font-weight: bold;">
                    GOVERNO DO ESTADO DO RIO DE JANEIRO
                </td>
            </tr>
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;
                    font-weight: bold;">
                    SECRETARIA DE ESTADO DE EDUCAÇÃO
                </td>
            </tr>
            <tr align="center">
                <td bgcolor="#D8D8D8" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    border-color: #000000; font-weight: bold; background: #D8D8D8;">
                    <label id="lblMeses" runat="server">
                    </label>
                </td>
            </tr>
           
        </table>
        <table align="center">
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    ( ) Inventário Anual
                </td>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    ( ) Inventário de Transferência de Responsabilidade
                </td>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    ( ) Inventário Especial
                </td>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    ( ) Inventário Rotativo
                </td>
            </tr>
        </table>
        <br />
        <table align="center" style="width: 80%;">
            <tr>
                <td align="justify" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    border-color: #000000; font-weight: bold;">
                    <label id="lblNomeSetor" runat="server">
                    </label>
                </td>
                <td align="right" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    border-color: #000000; font-weight: bold;">
                    <label id="lblSetor" runat="server">
                    </label>
                </td>
            </tr>
        </table>
        <br />
        <div runat="server" id="divControle">
        </div>
    </div>
</asp:Content>
