<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="InventarioExistenciasFisicasRegional.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.InventarioExistenciasFisicasRegional" %>

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

    <asp:Panel runat="server" ID="pnlTipoFiltro" GroupingText="Informe os dados para pesquisar as entradas"
        Width="800px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblFiltro" runat="server" Text="Filtros:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblTipoFiltro" runat="server" RepeatDirection="Horizontal"
                        Width="254px" AutoPostBack="true" OnSelectedIndexChanged="rblTipoFiltro_SelectedIndexChanged">
                        <asp:ListItem Text="Por Regional" Value="R"></asp:ListItem>
                        <asp:ListItem Text="Por Regional/Unidade" Value="U"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlFiltro" Width="800px" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRegional" runat="server" Text="Regional:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseRegional" runat="server" SqlOrder="descricao" SqlSelect="select distinct regional, descricao from (select distinct R.ID_REGIONAL as regional, R.REGIONAL as descricao from VW_UNIDADE_ENSINO_SITUACAO uuf
                                join LY_UNIDADE_ENSINO ue on uuf.UNIDADE_ENS = ue.UNIDADE_ENS
                                join TCE_REGIONAL R on R.ID_REGIONAL = ue.ID_REGIONAL) as tabela" GridWidth="600px"
                            ArgumentColumns="50" Columns="10" MaxLength="10" OnChanged="tseRegional_Changed"
                            DataType="Number" Key="regional">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="descricao" Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <asp:Panel runat="server" ID="pnlUnidade" Width="800px" Visible="false">
                        <td style="text-align: right; width: 200px">
                            <asp:Label Font-Names="Verdana" ID="lblUA" runat="server" Text="Unidade Administrativa:*"
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td colspan="3">
                            <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" Caption="" Key="setor"
                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome" SqlSelect=" SELECT uar.setor, nome, ua_atual, ua_antiga from VW_UNIDADE_ADMINISTRATIVA_REGIONAL uar join HADES..VW_SETOR s on uar.SETOR = s.SETOR "
                                SqlWhere=" regionalid = isnull( #tseRegional# ,0)" GridWidth="550px" OnChanged="tseUnidadeAdministrativa_Changed"
                                SqlOrder="setor">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />                                    
                                    <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="70%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </asp:Panel>
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
                    <td>
                        <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnPesquisar" runat="server" ImageUrl="~/Images/bot_buscar.png"
                            OnClick="btnPesquisar_Click" ValidationGroup="ConfirmarForm" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
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
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;
                    font-weight: bold;">
                    SUBSECRETARIA DE INFRAESTRUTURA E TECNOLOGIA
                </td>
            </tr>
            <tr align="center">
                <td bgcolor="#D8D8D8" style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px;
                    border-color: #000000; font-weight: bold; background: #D8D8D8;">
                    <label id="lblTitulo" runat="server">
                    </label>
                </td>
            </tr>
            <tr align="center">
                <td style="font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000;
                    font-weight: bold;">
                    (ANEXO IV - IN 29/2014)
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
