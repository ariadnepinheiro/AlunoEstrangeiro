<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FichaIndividualBem.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.FichaIndividualBem" %>

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

    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar o inventário individual"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 150px">
                    <asp:Label Font-Names="Verdana" ID="lblUA" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio">
                    </asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseUA" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetorCedente"
                        AutoPostBack="true" OnTextChanged="tseUA_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="Label1" SkinID="lblObrigatorio" runat="server"
                        Text="Patrimônio:*">
                    </asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseBem" runat="server" SqlOrder="numeroformatado" Caption=""
                        AutoPostBack="true" OnChanged="tseBem_Changed" Key="numeroformatado">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Nº Patrimônio" FieldName="numeroformatado" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    </label>
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
    <div id="divPrincipal" runat="server">
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
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;
                    font-weight: bold;">
                    <label id="lblSetor" runat="server" skinid="lblObrigatorio">
                    </label>
                </td>
            </tr>
        </table>
        <br />
        <table align="center" style="border-style: solid; border-width: 1px; width: 80%;
            border-color: #000000; border-collapse: collapse" border="1">
            <tr align="center">
                <td colspan="5" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    font-weight: bold; border-bottom-style: solid; border-bottom-width: 1px">
                    BENS PATRIMONIAIS - INVENTÁRIO INDIVIDUAL DE BEM PATRIMONIAL<br />
                    (ANEXO XV - IN 41/2017)
                </td>
            </tr>
            <tr>
                <td align="justify" style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000;
                    font-family: Tahoma, Geneva, sans-serif; font-size: 11px; height: 32px;" colspan="5">
                    <label id="lblOrgao" runat="server">
                    </label>
                </td>
            </tr>
            <tr>
                <td align="justify" colspan="5">
                    <table border="0">
                        <tr>
                            <td style="width: 194px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                 border-style: none none none none" >
                                Identificação:
                            </td>
                            <td style="width: 544px; height: 22px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                border-style: none none none none" colspan="4" >
                                <label id="lblIdentificacao" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 194px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                 border-style: none none none none" >
                                Nº de Inventário:
                            </td>
                            <td  style="width: 544px; height: 22px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                border-style: none none none none" colspan="4" >
                                <label id="lblInventario" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 194px; height: 22px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                border-style: none none none none" >
                                Código de Classificação:
                            </td>
                            <td  style="width: 544px; height: 22px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                                border-style: none none none none" colspan="4" >
                                <label id="lblClassificacao" runat="server">
                                </label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td style="border-width: 1px; border-color: #000000; border-style: solid; width: 194px;
                    font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    Data
                </td>
                <td style="border-width: 1px; border-color: #000000; border-style: solid solid solid none;
                    width: 544px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    Operação
                </td>
                <td style="border-width: 1px; border-color: #000000; border-style: solid solid solid none;
                    width: 544px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    Documento Hábil
                </td>
                <td style="border-width: 1px; border-color: #000000; border-style: solid solid solid none;
                    width: 544px;">
                    Histórico de Operação
                </td>
                <td style="border-width: 1px; border-color: #000000; border-style: solid solid solid none;
                    width: 544px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    Valor
                </td>
            </tr>
            <tr>
                <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #000000;
                    width: 194px; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;">
                    <label id="lblDataIncorporacao" runat="server">
                    </label>
                </td>
                <td style="border-right-style: solid; border-right-width: 1px; width: 544px; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px; border-right-color: #000000">
                    <label id="lblOperacao" runat="server">
                    </label>
                </td>
                <td style="border-right-style: solid; border-right-width: 1px; width: 544px; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px; border-right-color: #000000">
                    <label id="lblDocumento" runat="server">
                    </label>
                </td>
                <td style="border-right-style: solid; border-right-width: 1px; width: 544px; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px; border-right-color: #000000">
                    <label id="lblHistorico" runat="server">
                    </label>
                </td>
                <td style="border-right-style: solid; border-right-width: 1px; width: 544px; font-family: Tahoma, Geneva, sans-serif;
                    font-size: 11px; border-right-color: #000000">
                    <label id="lblValor" runat="server">
                    </label>
                </td>
            </tr>        
        </table>
        <br />
        <br />
        <br />
        <table align="center">
            <tr>
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;">
                    <label id="lblRodape" runat="server" skinid="lblObrigatorio">
                    </label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
