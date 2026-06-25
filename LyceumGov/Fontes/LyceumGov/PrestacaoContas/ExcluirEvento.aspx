<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcluirEvento.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.ExcluirEvento" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="FornecedorPopup.ascx" TagName="FornecedorPopup" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateEditWithoutBorder
        {
            width: 100%;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
            padding-bottom: 1px;
        }
        .TSearchButton
        {
            border-width: 0px !important;
            vertical-align: top !important;
            position: relative;
            top: -1px;
        }
        .table-fixed
        {
            table-layout: fixed;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:HiddenField ID="hidModoTela" runat="server" />
    <asp:HiddenField ID="hidPequenaDespesaId" runat="server" />
    <asp:HiddenField ID="hidPermitePequenaDespesa" runat="server" />
    <asp:HiddenField ID="hdnPodeExcluir" runat="server" />
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informações da Despesa" Width="800px">
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Unidade de Ensino: <span style="color: red">*</span></span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="11%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Projeto / Programa: <span style="color: red">*</span></span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="VarChar" AutoPostBack="false"
                        SqlSelect=" select pt.descricao, FINALIDADE,FINALIDADEID, pt.IDENTIFICADOR from  PrestacaoContas.VW_PLANOTRABALHO PT "
                        OnChanged="tsePlanoTrabalho_Changed" >
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                            <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Despesa:</span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tseEvento" runat="server" Caption="" Key="EVENTOID" Argument="DESCRICAO"
                        MaxLength="20" ArgumentColumns="50" Columns="10" GridWidth="850px" DataType="VarChar"
                        SqlSelect=" SELECT ESCOLA, NUMEROEVENTO, FINALIDADE, DATAPAGAMENTO, DATAPAGAMENTOFORMATADA, PLANOTRABALHOID, NUMERONOTAFISCAL FROM PRESTACAOCONTAS.VW_EVENTO "
                        OnChanged="tseEvento_Changed" Enabled="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="EVENTOID" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Número" FieldName="NUMEROEVENTO" Width="15%" />                            
                            <tweb:TSearchBoxColumn Caption="Nota Fiscal" FieldName="NUMERONOTAFISCAL" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Escola" FieldName="ESCOLA" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Dt.Pagamento" FieldName="DATAPAGAMENTOFORMATADA"
                                Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Código Plano" FieldName="PLANOTRABALHOID" Width="5%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:HiddenField ID="hdnEventoId" runat="server" />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" Visible="false"
            OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" Visible="false" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" Visible="false"
            OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" Visible="false"
            OnClick="btnSalvar_Click" />
        <asp:ImageButton ID="btnDeletar" runat="server" SkinID="BcDeletar" Visible="false"
            OnClick="btnDeletar_Click" OnClientClick="return confirm('A despesa será excluída. Confirma?');" />
        <asp:Label runat="server" ID="lblBlocoCadastrarEvento" Text="Despesas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsCadastrarEvento" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:PlaceHolder ID="plaCadastrarEvento" runat="server" Visible="false">
        <br />
        <asp:Panel ID="pnlDespesa" runat="server" GroupingText="Tipo de Despesa *" Width="853px"
            Style="color: Blue">
            <table class="table-fixed" width="800">
                <tr>
                    <td width="600">
                        <asp:RadioButtonList ID="rblFiltroTipoEvento" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblFiltroTipoEvento_SelectedIndexChanged">
                            <asp:ListItem Text="Despesas com documentos fiscais" Value="0" />
                            <asp:ListItem Text="Demais Despesas" Value="1" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlTipoDocumentoFiscal" runat="server" GroupingText="Tipo de Documento Fiscal *"
            Width="853px" Style="color: Blue">
            <table class="table-fixed" width="800">
                <tr>
                    <td width="600">
                        <asp:RadioButtonList ID="rblDocumentoFiscal" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rblDocumentoFiscal_SelectedIndexChanged">
                            <asp:ListItem Text="NF-e" Value="0" />
                            <asp:ListItem Text="Demais Documentos Fiscais" Value="1" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlTipoPequenaDespesa" runat="server" GroupingText="Tipo de Pequena Despesa *"
            Width="853px" Style="color: Blue">
            <table class="table-fixed" width="800">
                <tr>
                    <td width="600">
                        <asp:RadioButtonList ID="rbFiltroTipoPequenaDespesa" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="rbFiltroTipoPequenaDespesa_SelectedIndexChanged">
                            <asp:ListItem Text="Pequena Despesa" Value="0" />
                            <asp:ListItem Text="Despesa Com Locomoção de Servidores " Value="2" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlDescricao" runat="server" GroupingText="Descrição *" Width="853px"
            Style="color: Blue">
            <table class="table-fixed" width="600">
                <tr>
                    <td width="500">
                        <asp:TextBox ID="txtDescricao" runat="server" Width="492px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <dxtc:ASPxPageControl ID="pcCadastrarEvento" runat="server" ActiveTabIndex="0" Width="850px">
            <TabPages>
                <%-- Despesas com NF-e --%>
                <dxtc:TabPage Name="tabDespesaComum" Text="Despesas com NF-e ou Demais Documentos Fiscais"
                    ClientVisible="false">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl6" runat="server">
                            <br />
                            <asp:Panel ID="pnlInfoFornecedorVencedorOrcamento1" runat="server" GroupingText="Informações Fornecedor Vencedor Orçamento 1"
                                Width="825px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Fornecedor: <span style="color: red">*</span>
                                        </td>
                                        <td width="600">
                                            <tweb:TSearchBox ID="tseFornecedor" runat="server" Key="IDFORNECEDOR" Argument="RAZAOSOCIAL"
                                                MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" SqlSelect="SELECT CNPJ, INSCRICAOESTADUAL, INSCRICAOMUNICIPAL, DATACADASTRO, SITUACAO from [PrestacaoContas].[VW_TSEARCH_FORNECEDOR]"
                                                GridWidth="850px" SqlOrder="CNPJ" DataType="VarChar" OnChanged="tseFornecedor_Changed">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="IDFORNECEDOR" Width="5%" />
                                                    <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="CNPJ" Width="15%" />
                                                    <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="RAZAOSOCIAL" Width="30%" />
                                                    <tweb:TSearchBoxColumn Caption="Inscrição Estadual" FieldName="INSCRICAOESTADUAL"
                                                        Width="15%" />
                                                    <tweb:TSearchBoxColumn Caption="Inscrição Municipal" FieldName="INSCRICAOMUNICIPAL"
                                                        Width="15%" />
                                                    <tweb:TSearchBoxColumn Caption="Situação" FieldName="SITUACAO" Width="25%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                            <asp:Label ID="lblFornecedor" runat="server" Style="display: block"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlPesquisaPrecos" runat="server" GroupingText="Informações de Pesquisa de Preços"
                                Width="825px">
                                <table width="600">
                                    <tr>
                                        <td width="100" align="right">
                                            Orçamento 1: <span style="color: red">*</span>
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarOrcamento1" runat="server" href="javascript: void(0)" onclick="filOrcamento1.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="70">
                                            <asp:LinkButton ID="lnkVisualizarOrcamento1" runat="server" Text="Anexo" Visible="false"
                                                OnCommand="lnkVisualizarOrcamento_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanOrcamentoAnexado1"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" align="right">
                                            Orçamento 2:
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarOrcamento2" runat="server" href="javascript: void(0)" onclick="filOrcamento2.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="70">
                                            <asp:LinkButton ID="lnkVisualizarOrcamento2" runat="server" Text="Anexo" Visible="false"
                                                OnCommand="lnkVisualizarOrcamento_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanOrcamentoAnexado2"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" align="right">
                                            Orçamento 3:
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarOrcamento3" runat="server" href="javascript: void(0)" onclick="filOrcamento3.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="70">
                                            <asp:LinkButton ID="lnkVisualizarOrcamento3" runat="server" Text="Anexo" Visible="false"
                                                OnCommand="lnkVisualizarOrcamento_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanOrcamentoAnexado3"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100" align="right" valign="top">
                                            Justifique:
                                        </td>
                                        <td width="500" colspan="2">
                                            <asp:TextBox ID="txtJustificativa" runat="server" TextMode="MultiLine" Width="492px"
                                                Height="80px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlInfoPagamento" runat="server" GroupingText="Informações de Pagamento"
                                Width="825px">
                                <table class="table-fixed" width="700" id="tbChaveAcesso" runat="server">
                                    <tr>
                                        <td width="100" align="right">
                                            Chave de Acesso: <span style="color: red">*</span>
                                        </td>
                                        <td width="600">
                                            <asp:TextBox ID="txtChaveAcesso" runat="server" Width="490px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="100" align="right">
                                            Número da Nota Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtNumeroNF" runat="server" Width="490px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Valor da Nota Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtValorTotalNF" runat="server" Width="190px" CssClass="numeric"
                                                OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                        </td>
                                        <td width="100">
                                            &nbsp;
                                        </td>
                                        <td width="100" align="right">
                                            Valor Pago NF: <span style="color: red">*</span>
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtValorPagoNF" runat="server" Width="190px" CssClass="numeric"
                                                OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Data da Nota Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtDataNF" runat="server" Width="190px" CssClass="date"></asp:TextBox>
                                        </td>
                                        <td width="100">
                                            &nbsp;
                                        </td>
                                        <td width="100" align="right">
                                            Data de Pagamento NF: <span style="color: red">*</span>
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtDataPagamentoNF" runat="server" Width="190px" CssClass="date"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="100" align="right">
                                            Nota Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarNotaFiscal" runat="server" href="javascript: void(0)" onclick="$(() => { filNotaFiscal.click(); });">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="170">
                                            <asp:LinkButton ID="lnkVisualizarNotaFiscal" runat="server" Text="Nota Fiscal Atestada"
                                                OnCommand="lnkVisualizarNotaFiscal_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanNotaFiscalAnexada"></span>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="100" align="right">
                                            Comprovante de Pagamento: <span style="color: red">*</span>
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarComprovantePgto" runat="server" href="javascript: void(0)" onclick="filComprovantePgto.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="170">
                                            <asp:LinkButton ID="lnkVisualizarComprovantePgto" runat="server" Text="Comprovante de Pagamento"
                                                OnCommand="lnkVisualizarComprovantePgto_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanComprovantePgtoAnexado"></span>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300" id="tbXML" runat="server">
                                    <tr>
                                        <td width="100" align="right">
                                            XML: <span style="color: red">*</span>
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarXML" runat="server" href="javascript: void(0)" onclick="filXML.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="170">
                                            <asp:LinkButton ID="lnkVisualizarXML" runat="server" Text="XML inserido" OnCommand="lnkVisualizarXML_Command"
                                                OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanXMLAnexado"></span>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlInfoAdicionais" runat="server" GroupingText="Informações Adicionais"
                                Width="825px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right" valign="top">
                                            Observação:
                                        </td>
                                        <td width="600">
                                            <asp:TextBox ID="txtObservacao" runat="server" Width="490px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="200">
                                    <tr>
                                        <td width="100" align="right">
                                            Evidência:
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarEvidencia" runat="server" href="javascript: void(0)" onclick="filEvidencia.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="70">
                                            <asp:LinkButton ID="lnkVisualizarEvidencia" runat="server" Text="Anexo" Visible="false"
                                                OnCommand="lnkVisualizarEvidencia_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanEvidenciaAnexada"></span>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div style="display: none;">
                                <asp:FileUpload ID="filOrcamento1" runat="server" />
                                <asp:FileUpload ID="filOrcamento2" runat="server" />
                                <asp:FileUpload ID="filOrcamento3" runat="server" />
                                <asp:FileUpload ID="filNotaFiscal" runat="server" />
                                <asp:FileUpload ID="filComprovantePgto" runat="server" />
                                <asp:FileUpload ID="filXML" runat="server" />
                                <asp:FileUpload ID="filEvidencia" runat="server" />
                                <asp:HiddenField ID="hidOrcamento1" runat="server" />
                                <asp:HiddenField ID="hidOrcamento2" runat="server" />
                                <asp:HiddenField ID="hidOrcamento3" runat="server" />
                                <asp:HiddenField ID="hidNotaFiscal" runat="server" />
                                <asp:HiddenField ID="hidComprovantePgto" runat="server" />
                                <asp:HiddenField ID="hidXML" runat="server" />
                                <asp:HiddenField ID="hidEvidencia" runat="server" />
                            </div>

                            <script language="javascript">

                                var filOrcamento1, filOrcamento2, filOrcamento3, filNotaFiscal, filComprovantePgto, filXML, filEvidencia;
                                var hidXML;

                                $(document).ready(function() {

                                    filOrcamento1 = $("#<%= filOrcamento1.ClientID %>");
                                    filOrcamento2 = $("#<%= filOrcamento2.ClientID %>");
                                    filOrcamento3 = $("#<%= filOrcamento3.ClientID %>");
                                    filNotaFiscal = $("#<%= filNotaFiscal.ClientID %>");
                                    filComprovantePgto = $("#<%= filComprovantePgto.ClientID %>");
                                    filXML = $("#<%= filXML.ClientID %>");
                                    filEvidencia = $("#<%= filEvidencia.ClientID %>");

                                    hidOrcamento1 = $("#<%= hidOrcamento1.ClientID %>");
                                    hidOrcamento2 = $("#<%= hidOrcamento2.ClientID %>");
                                    hidOrcamento3 = $("#<%= hidOrcamento3.ClientID %>");
                                    hidNotaFiscal = $("#<%= hidNotaFiscal.ClientID %>");
                                    hidComprovantePgto = $("#<%= hidComprovantePgto.ClientID %>");
                                    hidXML = $("#<%= hidXML.ClientID %>");
                                    hidEvidencia = $("#<%= hidEvidencia.ClientID %>");

                                    filOrcamento1.attr("data-url", "<%= hidOrcamento1.ClientID %>").attr("accept", "application/pdf");
                                    filOrcamento2.attr("data-url", "<%= hidOrcamento2.ClientID %>").attr("accept", "application/pdf");
                                    filOrcamento3.attr("data-url", "<%= hidOrcamento3.ClientID %>").attr("accept", "application/pdf");
                                    filNotaFiscal.attr("data-url", "<%= hidNotaFiscal.ClientID %>").attr("accept", "application/pdf");
                                    filComprovantePgto.attr("data-url", "<%= hidComprovantePgto.ClientID %>").attr("accept", "application/pdf");
                                    filXML.attr("data-url", hidXML.attr("id")).attr("accept", "text/xml");
                                    filEvidencia.attr("data-url", "<%= hidEvidencia.ClientID %>").attr("accept", "application/pdf");

                                    hidOrcamento1.attr("data-msg", "spanOrcamentoAnexado1");
                                    hidOrcamento2.attr("data-msg", "spanOrcamentoAnexado2");
                                    hidOrcamento3.attr("data-msg", "spanOrcamentoAnexado3");
                                    hidNotaFiscal.attr("data-msg", "spanNotaFiscalAnexada");
                                    hidComprovantePgto.attr("data-msg", "spanComprovantePgtoAnexado");
                                    hidXML.attr("data-msg", "spanXMLAnexado");
                                    hidEvidencia.attr("data-msg", "spanEvidenciaAnexada");

                                    $(filOrcamento1).on("change", fileUploadChange);
                                    $(filOrcamento2).on("change", fileUploadChange);
                                    $(filOrcamento3).on("change", fileUploadChange);
                                    $(filNotaFiscal).on("change", fileUploadChange);
                                    $(filComprovantePgto).on("change", fileUploadChange);
                                    $(filXML).on("change", fileUploadChange);
                                    $(filEvidencia).on("change", fileUploadChange);

                                    if (["InserirNovaDespesaComum", "EditarDespesaComumExistente"].includes($("#ctl00_cphFormulario_hidModoTela").val())) {
                                        $(hidOrcamento1).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                        $(hidOrcamento2).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                        $(hidOrcamento3).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                        $(hidNotaFiscal).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                        $(hidComprovantePgto).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                        $(hidXML).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                        $(hidEvidencia).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                    }
                                });
                            
                            </script>

                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <%-- Despesa com Demais Documentos Fiscais --%>
                <dxtc:TabPage Name="tabPequenaDespesaComComprovacao" Text="Pequena Despesa" ClientVisible="false">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl7" runat="server">
                            <p style="color: red">
                                São despesas de pequeno porte, para as quais é necessária apresentação do documento
                                fiscal, sendo facultativa a apresentação do comprovante de pagamento.
                            </p>
                            <br />
                            <asp:Panel ID="pnlFornecedor_DCC" runat="server" GroupingText="Informações Fornecedor"
                                Width="825px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Fornecedor: <span style="color: red">*</span>
                                        </td>
                                        <td width="600">
                                            <tweb:TSearchBox ID="tseFornecedor_DCC" runat="server" SqlOrder="cnpj" SqlSelect="
                                            SELECT cnpj, inscricaoestadual, inscricaomunicipal, DATACADASTRO, SITUACAO 
                                            FROM [LYCEUM].PrestacaoContas.VW_TSEARCH_FORNECEDOR" MaxLength="20" ArgumentColumns="50"
                                                Columns="10" AutoPostBack="true" EnableViewState="true" Key="IDFORNECEDOR" DataType="VarChar"
                                                Argument="razaosocial" OnChanged="tseFornecedor_Changed">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="IDFORNECEDOR" Width="5%" />
                                                    <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cnpj" Width="15%" />
                                                    <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="razaosocial" Width="30%" />
                                                    <tweb:TSearchBoxColumn Caption="Inscrição Estadual" FieldName="inscricaoestadual"
                                                        Width="15%" />
                                                    <tweb:TSearchBoxColumn Caption="Inscrição Municipal" FieldName="inscricaomunicipal"
                                                        Width="15%" />
                                                    <tweb:TSearchBoxColumn Caption="Situação" FieldName="SITUACAO" Width="20%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                            <asp:Label ID="lblFornecedor_DCC" runat="server" Style="display: block"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlInfoPagamento_DCC" runat="server" GroupingText="Informações de Pagamento"
                                Width="825px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Forma de Pagamento: <span style="color: red">*</span>
                                        </td>
                                        <td width="550">
                                            <asp:DropDownList ID="ddlFormaPagamento_DCC" runat="server" Width="200px">
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>Em Espécie</asp:ListItem>
                                                <asp:ListItem>Débito/Transferência</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="150" align="right">
                                            Número da Nota Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtNumeroNF_DCC" runat="server" Width="490px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Valor da Nota Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtValorNF_DCC" runat="server" Width="100px" CssClass="numeric"
                                                OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                        </td>
                                        <td width="100">
                                            &nbsp;
                                        </td>
                                        <td width="150" align="right">
                                            Valor Pago NF: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtValorPagoNF_DCC" runat="server" Width="100px" CssClass="numeric"
                                                OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Data da Nota Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtDataNF_DCC" runat="server" Width="100px" CssClass="date"></asp:TextBox>
                                        </td>
                                        <td width="100">
                                            &nbsp;
                                        </td>
                                        <td width="150" align="right">
                                            Data de Pagamento NF: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtDataPagamento_DCC" runat="server" Width="100px" CssClass="date"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="150" align="right">
                                            Documento Fiscal: <span style="color: red">*</span>
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarNotaFiscal_DCC" runat="server" href="javascript: void(0)" onclick="filNotaFiscal_DCC.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="120">
                                            <asp:LinkButton ID="lnkVisualizarNotaFiscal_DCC" runat="server" Text="Documento Fiscal Atestado"
                                                OnCommand="lnkVisualizarNotaFiscal_DCC_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanNotaFiscalDCCAnexada"></span>
                                        </td>
                                    </tr>
                                </table>
                                <div style="display: none;">
                                    <asp:FileUpload ID="filNotaFiscal_DCC" runat="server" />
                                    <asp:HiddenField ID="hidNotaFiscal_DCC" runat="server" />
                                </div>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="150" align="right">
                                            Comprovante de Pagamento:
                                        </td>
                                        <td width="30">
                                            <a id="btnAnexarComprovantePgto_DCC" runat="server" href="javascript: void(0)" onclick="filComprovantePgto_DCC.click()">
                                                <img src="../img/upload.png" alt="" /></a>
                                        </td>
                                        <td width="120">
                                            <asp:LinkButton ID="lnkVisualizarComprovantePgto_DCC" runat="server" Text="Comprovante de Pagamento"
                                                OnCommand="lnkVisualizarComprovantePgto_DCC_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                            <span id="spanComprovantePgtoDCCAnexado"></span>
                                        </td>
                                    </tr>
                                </table>
                                <div style="display: none;">
                                    <asp:FileUpload ID="filComprovantePgto_DCC" runat="server" />
                                    <asp:HiddenField ID="hidComprovantePgto_DCC" runat="server" />
                                </div>

                                <script language="javascript">

                                    var filNotaFiscal_DCC, filComprovantePgto_DCC;

                                    $(document).ready(function() {

                                        filNotaFiscal_DCC = $("#<%= filNotaFiscal_DCC.ClientID %>");
                                        filComprovantePgto_DCC = $("#<%= filComprovantePgto_DCC.ClientID %>");

                                        hidNotaFiscal_DCC = $("#<%= hidNotaFiscal_DCC.ClientID %>");
                                        hidComprovantePgto_DCC = $("#<%= hidComprovantePgto_DCC.ClientID %>");

                                        filNotaFiscal_DCC.attr("data-url", "<%= hidNotaFiscal_DCC.ClientID %>").attr("accept", "application/pdf");
                                        filComprovantePgto_DCC.attr("data-url", "<%= hidComprovantePgto_DCC.ClientID %>").attr("accept", "application/pdf");

                                        hidNotaFiscal_DCC.attr("data-msg", "spanNotaFiscalDCCAnexada");
                                        hidComprovantePgto_DCC.attr("data-msg", "spanComprovantePgtoDCCAnexado");

                                        $(filNotaFiscal_DCC).on("change", fileUploadChange);
                                        $(filComprovantePgto_DCC).on("change", fileUploadChange);

                                        if (["InserirNovaPequenaDespesaComComprovacao", "EditarPequenaDespesaComComprovacaoExistente"].includes($("#ctl00_cphFormulario_hidModoTela").val())) {
                                            $(hidNotaFiscal_DCC).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                            $(hidComprovantePgto_DCC).on("valor-alterado", hiddenChange).trigger("valor-alterado");
                                        }
                                    });
                                    
                                </script>

                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <%-- Pequena Despesa --%>
                <dxtc:TabPage Name="tabPequenaDespesaSemComprovacao" Text="Pequena Despesa sem comprovação"
                    ClientVisible="false">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl8" runat="server">
                            <asp:Panel ID="pnlDespesaSemComprovacao" runat="server" GroupingText="" Width="825px">
                                <p style="color: red">
                                    São despesas de pequeno porte, para as quais não é necessária ou não é possível
                                    a apresentação de documento fiscal ou de comprovante de pagamento. É necessário
                                    apresentar justificativa para a não apresentação da comprovação da despesa
                                </p>
                                <br />
                                <asp:Panel ID="pnlFornecedor_DSC" runat="server" GroupingText="Informações Fornecedor"
                                    Width="125px">
                                    <table class="table-fixed" width="700">
                                        <tr>
                                            <td width="100" align="right">
                                                Fornecedor: <span style="color: red">*</span>
                                            </td>
                                            <td width="600">
                                                <tweb:TSearchBox ID="tseFornecedor_DSC" runat="server" SqlOrder="cnpj" SqlSelect="
                                            SELECT cnpj, razaosocial,inscricaoestadual, inscricaomunicipal, DATACADASTRO, SITUACAO 
                                            FROM [LYCEUM].[PrestacaoContas].[VW_TSEARCH_FORNECEDOR] f
                                            " MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true" EnableViewState="true"
                                                    Key="IDFORNECEDOR" DataType="Number" Argument="razaosocial" OnChanged="tseFornecedor_Changed">
                                                    <GridColumns>
                                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="IDFORNECEDOR" Width="5%" />
                                                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cnpj" Width="15%" />
                                                        <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="razaosocial" Width="30%" />
                                                        <tweb:TSearchBoxColumn Caption="Inscrição Estadual" FieldName="inscricaoestadual"
                                                            Width="15%" />
                                                        <tweb:TSearchBoxColumn Caption="Inscrição Municipal" FieldName="inscricaomunicipal"
                                                            Width="15%" />
                                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="SITUACAO" Width="20%" />
                                                        <tweb:TSearchBoxColumn Caption="Data cadastro" FieldName="DATACADASTRO" Width="15%"
                                                            Visible="false" />
                                                    </GridColumns>
                                                </tweb:TSearchBox>
                                                <asp:Label ID="lblFornecedor_DSC" runat="server" Style="display: block"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlInfoPagamento_DSC" runat="server" GroupingText="Informações de Pagamento"
                                    Width="825px">
                                    <table class="table-fixed" width="700">
                                        <tr>
                                            <td width="150" align="right">
                                                Forma de Pagamento: <span style="color: red">*</span>
                                            </td>
                                            <td width="550">
                                                <asp:DropDownList ID="ddlFormaPagamento_DSC" runat="server" Width="200px">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem>Em Espécie</asp:ListItem>
                                                    <asp:ListItem>Débito/Transferência</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table-fixed" width="300">
                                        <tr>
                                            <td width="150" align="right">
                                                Valor pago: <span style="color: red">*</span>
                                            </td>
                                            <td width="150">
                                                <asp:TextBox ID="txtValorPago_DSC" runat="server" Width="100px" OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table-fixed" width="300">
                                        <tr>
                                            <td width="150" align="right">
                                                Data de Pagamento: <span style="color: red">*</span>
                                            </td>
                                            <td width="150">
                                                <asp:TextBox ID="txtDataPagamento_DSC" runat="server" Width="100px" CssClass="date"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="table-fixed" width="700">
                                        <tr>
                                            <td width="150" align="right">
                                                Justificativa: <span style="color: red">*</span>
                                            </td>
                                            <td width="550">
                                                <asp:TextBox ID="txtJustificativa_DSC" runat="server" Width="500px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <%-- Despesa Com Locomoção de Servidores --%>
                <dxtc:TabPage Name="tabPequenaDespesaComTranslado" Text="Despesa Com Locomoção de Servidores"
                    ClientVisible="false">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl9" runat="server">
                            <p style="color: red">
                                São despesas realizadas para a locomoção de servidores lotados na unidade, quando
                                em atendimento a atividades requisitadas pela SEEDUC, como reuniões, por exemplo.
                                É necessário justificar a causa da despesa e informar quais servidores foram atendidos.
                            </p>
                            <br />
                            <asp:Panel ID="pnlInfoTranslado_DCTS" runat="server" GroupingText="Informações da Locomoção"
                                Width="825px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Modal de transporte: <span style="color: red">*</span>
                                        </td>
                                        <td width="550">
                                            <asp:DropDownList ID="ddlModalTransporte_DCTS" runat="server" DataTextField="DESCRICAO"
                                                DataValueField="TIPOTRANSPORTEID" Width="200px" AppendDataBoundItems="true" />
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Origem: <span style="color: red">*</span>
                                        </td>
                                        <td width="550">
                                            <asp:TextBox ID="txtOrigem_DCTS" runat="server" Width="540px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Destino: <span style="color: red">*</span>
                                        </td>
                                        <td width="550">
                                            <asp:TextBox ID="txtDestino_DCTS" runat="server" Width="540px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Valor Pago: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtValorPago_DCTS" runat="server" Width="100px" OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                        </td>
                                        <td width="100">
                                            &nbsp;
                                        </td>
                                        <td width="150" align="right">
                                            Data de Pagamento: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtDataPagamento_DCTS" runat="server" Width="100px" CssClass="date"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Justificativa: <span style="color: red">*</span>
                                        </td>
                                        <td width="550">
                                            <asp:TextBox ID="txtJustificativa_DCTS" runat="server" Width="540px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlFornecedor_DCTS" runat="server" GroupingText="Servidores" Width="825px">
                                <asp:PlaceHolder ID="plaAdicionarServidor_DCTS" runat="server" Visible="false">
                                    <p style="color: red">
                                        É obrigatório identificar os servidores lotados na unidade que tenham utilizado
                                        o transporte com este recurso(mínimo um servidor).
                                    </p>
                                    <table class="table-fixed" width="700">
                                        <tr>
                                            <td width="150" align="right">
                                                Servidor: <span style="color: red">*</span>
                                            </td>
                                            <td width="450">
                                                <tweb:TSearch ID="tseAdicionarServidor_DCTS" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryMatriculaComIdFuncional"
                                                    AutoPostBack="false" Width="400px" />
                                            </td>
                                            <td width="100">
                                                <asp:Button ID="btnAdicionarServidor_DCTS" runat="server" Width="90px" Text="Adicionar"
                                                    OnClick="btnAdicionarServidor_DCTS_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:PlaceHolder>
                                <dxwgv:ASPxGridView runat="server" ID="grdServidores_DCTS" ClientInstanceName="grdServidores_DCTS"
                                    AutoGenerateColumns="False" EnableCallBacks="false" Width="800" KeyFieldName="IdFuncional"
                                    OnRowDeleting="grdServidores_DCTS_RowDeleting" OnCommandButtonInitialize="grdServidores_DCTS_CommandButtonInitialize">
                                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                    <SettingsEditing Mode="Inline" />
                                    <SettingsText EmptyDataRow="Não existem dados." />
                                    <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="100px">
                                            <DeleteButton Visible="True" Text="Remover">
                                                <Image Url="../img/bt_exclui2.png" />
                                            </DeleteButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PEQUENADESPESASERVIDORID" FieldName="PequenaDespesaServidorId"
                                            VisibleIndex="0" Width="100px" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PESSOA" FieldName="Pessoa" VisibleIndex="0"
                                            Width="100px" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Id Funcional" FieldName="IdFuncional" VisibleIndex="1"
                                            Width="100px">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NomeCompl" VisibleIndex="2"
                                            Width="200px">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="Matricula" VisibleIndex="3"
                                            Width="100px">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <%-- Exigências --%>
                <dxtc:TabPage Name="tabExigencias" Text="Exigências">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl4" runat="server">
                            <asp:ObjectDataSource ID="odsExigenciaEvento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.CadastrarEvento"
                                SelectMethod="ListaExigenciaEvento" UpdateMethod="UpdateExigenciaEvento">
                                <SelectParameters>
                                    <asp:ControlParameter Name="eventoId" ControlID="tseEvento" PropertyName="Value"
                                        Type="Int32" DefaultValue="1" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView runat="server" ID="grdExigenciaEvento" ClientInstanceName="grdExigenciaEvento"
                                AutoGenerateColumns="False" EnableCallBacks="false" Width="850" DataSourceID="odsExigenciaEvento"
                                KeyFieldName="EXIGENCIAEVENTOID">
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="Motivo" FieldName="MOTIVO" VisibleIndex="1"
                                        Width="200px">
                                        <EditItemTemplate>
                                            <%# Eval("MOTIVO")%></EditItemTemplate>
                                        <EditCellStyle CssClass="tab-cell">
                                        </EditCellStyle>
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nota Explicativa" FieldName="NOTAEXPLICATIVA"
                                        VisibleIndex="2" Width="200px">
                                        <EditItemTemplate>
                                            <%# Eval("NOTAEXPLICATIVA")%></EditItemTemplate>
                                        <EditCellStyle CssClass="tab-cell">
                                        </EditCellStyle>
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" VisibleIndex="3"
                                        Width="200px">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Valor Ressar cimento" FieldName="VALORRESSARCIMENTO"
                                        VisibleIndex="4" Width="50px">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Data Ressar cimento" FieldName="DATARESSARCIMENTO"
                                        VisibleIndex="5" Width="50px">
                                        <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Anexo" Name="btnDetalhes" VisibleIndex="6"
                                        Width="50px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EXIGENCIAEVENTOID") %>'
                                                OnCommand="btnDetalhes_Command" ImageUrl="~/img/upload.png" Height="15px" AlternateText="Importar Arquivo"
                                                Visible='<%# Eval("EXIGENCIAEVENTOID") != DBNull.Value %>'></asp:ImageButton>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Visua lizar" VisibleIndex="7" Width="50px">
                                        <DataItemTemplate>
                                            <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EXIGENCIAEVENTOID") + "," + Eval("TIPOARQUIVO") %>'
                                                OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                                OnClientClick="window.scrollTo(0, 0);" AlternateText="Visualizar Documento" Visible='<%# Eval("EXIGENCIAEVENTOID") != DBNull.Value && Eval("EXIGENCIAEVENTOARQUIVOID") != DBNull.Value %>'>
                                            </asp:ImageButton>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Aprovado" FieldName="APROVADO" VisibleIndex="8"
                                        Width="50px">
                                        <DataItemTemplate>
                                            <%# Convert.ToBoolean(Eval("APROVADO")) ? "Sim" : "Não" %></DataItemTemplate>
                                        <EditItemTemplate>
                                            <%# Convert.ToBoolean(Eval("APROVADO")) ? "Sim" : "Não" %></EditItemTemplate>
                                        <EditCellStyle CssClass="tab-cell" HorizontalAlign="Center" VerticalAlign="Middle">
                                        </EditCellStyle>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Rejeitado" FieldName="REJEITADO" VisibleIndex="8"
                                        Width="50px">
                                        <DataItemTemplate>
                                            <%# Convert.ToBoolean(Eval("REJEITADO")) ? "Sim" : "Não" %></DataItemTemplate>
                                        <EditItemTemplate>
                                            <%# Convert.ToBoolean(Eval("REJEITADO")) ? "Sim" : "Não" %></EditItemTemplate>
                                        <EditCellStyle CssClass="tab-cell" HorizontalAlign="Center" VerticalAlign="Middle">
                                        </EditCellStyle>
                                        <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                            <p>
                                <span style="color: red">*</span> Exigência de taxas bancárias não reembolsadas
                                no período e valores constantes no extrato que não possuam evento de saída
                            </p>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <%-- Itens da Nota Fiscal --%>
                <dxtc:TabPage Name="tabItensNf" Text="Itens NF" ClientVisible="true">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl5" runat="server">
                            <asp:Panel ID="Panel5" runat="server" GroupingText="Informações da Nota Fiscal" Width="825px">
                                <asp:ObjectDataSource ID="odsItensNf" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.CadastrarEvento"
                                    SelectMethod="ListaXML">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="eventoId" ControlID="tseEvento" PropertyName="Value"
                                            Type="Int32" DefaultValue="0" />
                                        <asp:ControlParameter Name="censo" ControlID="tseUnidadeEnsino" PropertyName="Value"
                                            Type="String" DefaultValue="" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <dxwgv:ASPxGridView runat="server" ID="grdItensNf" ClientInstanceName="grdItensNf"
                                    AutoGenerateColumns="False" EnableCallBacks="false" Width="770" DataSourceID="odsItensNf"
                                    KeyFieldName="IMPORTACAOXMLEVENTOID">
                                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                    <SettingsEditing Mode="Inline" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                    <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                                    <Columns>
                                        <dxwgv:GridViewDataColumn Caption="Item" FieldName="ITEM" VisibleIndex="1" Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="NCM" FieldName="NCM" VisibleIndex="2" Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Un. Medida" FieldName="UNIDADEMEDIDA" VisibleIndex="3"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CODIGOFGV" VisibleIndex="4"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Qtd." FieldName="QUANTIDADE" VisibleIndex="5"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Vr. Unitário" FieldName="VALORUNITARIO" VisibleIndex="6"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Vr. Pago" FieldName="VALORPAGO" VisibleIndex="7"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALORFGV" VisibleIndex="8"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Diferença" FieldName="DIFERENCA" VisibleIndex="9"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Dif. %" FieldName="PORCENTAGEMDIFERENCA" VisibleIndex="10"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Permitido" FieldName="NAOPERMITIDO" VisibleIndex="11"
                                            Width="70px">
                                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <%-- Itens da Nota Fiscal --%>
                <dxtc:TabPage Name="tabExcluir" Text="Excluir Despesa" ClientVisible="false">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <table>
                                <tr>
                                    <td width="100" align="right" valign="top">
                                        Motivo da exclusão:
                                    </td>
                                    <td width="500" colspan="2">
                                        <asp:TextBox ID="txtJustificativaExclusao" runat="server" TextMode="MultiLine" Width="492px"
                                            Height="80px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td width="100">
                                        <asp:Button ID="btnExcluirDespesa" runat="server" Text="Excluir Despesa" OnClick="btnExcluirDespesa_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
        <br />
        <asp:Panel ID="pnlStatusAnalise" runat="server" GroupingText="Status da Análise"
            Width="853px">
            <asp:Button ID="btnEnviarEvento" runat="server" Text="Enviar Despesa" OnClick="btnEnviarEvento_Click"
                Visible="false" />
            <asp:Label ID="lblStatusEvento" runat="server" Text="" Visible="false" Style="font-weight: bold;
                font-size: 14px;" />
        </asp:Panel>
    </asp:PlaceHolder>
    <asp:HiddenField ID="hdnExigenciaEventoId" runat="server" />
    <asp:HiddenField ID="hdnId" runat="server" />
    <dxpc:ASPxPopupControl ID="pucConfirmarArquivo" ClientInstanceName="pucConfirmarArquivo"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
        ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
        Width="580px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
        HeaderText="Upload de Arquivos">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,18000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:Label ID="lblMensagemImportarArquivo" runat="server" SkinID="lblMensagem"></asp:Label>
                <table id="Table1" runat="server" width="100%">
                    <tr>
                        <td>
                            Documento:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Justificativa:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtJustificativaExigencia" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblValorRessarcimento" runat="server" Text="Valor Ressarcimento*:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtValorRessarcimento" runat="server" Width="140px" OnKeyPress="return(moeda(this,'.',',',event))" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDataRessarcimento" runat="server" Text="Data Ressarcimento*:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxDateEdit ID="dtDataRessarcimento" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                                CalendarProperties-TodayButtonText="Hoje" Width="140px">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnImportar" runat="server" Text="Importar" OnClick="btnImportar_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
        ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Arquivos">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <dxe:ASPxBinaryImage ID="bimgArquivo" Width="350px" Height="350px" runat="server"
                    Visible="false" StoreContentBytesInViewState="True" AlternateText="sem foto"
                    ClientInstanceName="bimgArquivo">
                    <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                </dxe:ASPxBinaryImage>
                <asp:Literal ID="ltEmbed" runat="server" Visible="false" />
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucVisualizarXML" ClientInstanceName="pucVisualizarXML"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
        ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Arquivos">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:ObjectDataSource ID="odsXML" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.CadastrarEvento"
                    SelectMethod="ListaXML">
                    <SelectParameters>
                        <asp:ControlParameter Name="eventoId" ControlID="tseEvento" PropertyName="Value"
                            Type="Int32" DefaultValue="0" />
                        <asp:ControlParameter Name="censo" ControlID="tseUnidadeEnsino" PropertyName="Value"
                            Type="String" DefaultValue="" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <dxwgv:ASPxGridView runat="server" ID="grdXML" ClientInstanceName="grdXML" AutoGenerateColumns="False"
                    EnableCallBacks="false" Width="770" DataSourceID="odsXML" KeyFieldName="IMPORTACAOXMLEVENTOID">
                    <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewDataColumn Caption="Item" FieldName="ITEM" VisibleIndex="1" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataColumn Caption="NCM" FieldName="NCM" VisibleIndex="2" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Un. Medida" FieldName="UNIDADEMEDIDA" VisibleIndex="3"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CODIGOFGV" VisibleIndex="4"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Qtd." FieldName="QUANTIDADE" VisibleIndex="5"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Vr. Unitário" FieldName="VALORUNITARIO" VisibleIndex="6"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Vr. Pago" FieldName="VALORPAGO" VisibleIndex="7"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALORFGV" VisibleIndex="8"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Diferença" FieldName="DIFERENCA" VisibleIndex="9"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Dif. %" FieldName="PORCENTAGEMDIFERENCA" VisibleIndex="10"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Permitido" FieldName="NAOPERMITIDO" VisibleIndex="11"
                            Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarxcluir" ClientInstanceName="pucConfirmarxcluir"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Confirma exclusão da despesa?
                            <br />
                            Essa ação não poderá ser desfeita!
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>

    <script language="javascript">
                                
        function fileUploadChange (evt, mimeType) {

            let MAX_FILE_SIZE = 4194304; //tamanho máx. foto ou pdf: 4MB

            let self = $(this);
            let files = $(this).prop("files");
            let accept = $(this).attr("accept").split(",");
    		
            if (files.length > 0) {
                let arquivo = $(this).prop("files")[0];
                let reader = new FileReader();
    			
                reader.onload = function (e) {

                    let result = e.target.result;

                    if (e.loaded > MAX_FILE_SIZE) {
                        alert("Não são permitidos arquivos acima de 4MB");
                        return;
                    }

                    if (accept.indexOf(arquivo.type) == -1) {
                        alert("Não é permitido arquivo que não seja \"" + accept.join("\",\"") + "\"");
                        return;
                    }

                    let dataUrlParts = result.split(";");
                    dataUrlParts.splice(1, 0, "name:" + arquivo.name);
                    result = dataUrlParts.join(";");

                    console.log(self.attr("data-url"));

                    $("#" + self.attr("data-url")).val(result);
                    $("#" + self.attr("data-url")).attr("data-filename", arquivo.name);
                    $("#" + self.attr("data-url")).trigger("valor-alterado");
                }

                reader.readAsDataURL(arquivo);
            }

            $(this).prop("value", "");

        };
        
        function hiddenChange (evt) {
            let hid = $(evt.target);
            let msg = $("#" + hid.attr("data-msg"));
            
            let dataUrl = hid.val();
            if (dataUrl == "")
                return;
            
            let dataSize = atob(dataUrl.split(",")[1]).length;
            let dataType = dataUrl.split(";")[0].split(":")[1];
            let dataName = dataUrl.split(";")[1].split(":")[1];
            
           // $(\"#" + msg.attr("id") + "\").html(\"\");
            msg.html("[" + dataName + "]<br /><a href='javascript: $(\"#" + hid.attr("id") + "\").val(\"\"); LimpaSpan(\"" + msg.attr("id") + "\") '>Excluir</a>");
                       
        };
        
         function LimpaSpan(msg)
         {
            document.getElementById(msg).innerHTML = "";    
         }     ;                 
    </script>

    <script language="javascript">
        var tsePlanoTrabalho, tseUnidadeEnsino, tseEvento;
        
        $(document).ready(function() {
            tseUnidadeEnsino = $("#<%= tseUnidadeEnsino.ClientID %>");
            tseUnidadeEnsinoGrid = $("#<%= tseUnidadeEnsino.ClientID %>_grid");
            tsePlanoTrabalho = $("#<%= tsePlanoTrabalho.ClientID %>");
            tsePlanoTrabalhoGrid = $("#<%= tsePlanoTrabalho.ClientID %>_grid");
            tseEvento = $("#<%= tseEvento.ClientID %>");
            tseEventoGrid = $("#<%= tseEvento.ClientID %>_grid");

            tseUnidadeEnsino.on("change", filtrarEvento);
            tseUnidadeEnsinoGrid.on("click", filtrarEvento);
            tsePlanoTrabalho.on("change", filtrarEvento);
            tsePlanoTrabalhoGrid.on("click", filtrarEvento);
            
            $(".numeric").numeric({ decimal: ",", decimalPlaces: 2 });

            $(".date").each(function() {
                $(this).mask("99/99/9999");
            });
            
            $("#<%= txtChaveAcesso.ClientID %>").mask("99999999999999999999999999999999999999999999", { placeholder: '' })
        });

        function filtrarEvento(lock) {
            try {
                if (lock != "lock") {
                    window.setTimeout(() => filtrarEvento("lock"), 100);
                    return;
                }
                
                console.log("verificando filtragem...");
                
                var tseEventoDisabled = tseEvento.attr("disabled") == "disabled";
                    
                if (tseEventoDisabled && (tseUnidadeEnsino.val() == undefined || tseUnidadeEnsino.val() == null || tseUnidadeEnsino.val() == ""))
                    return;
                
                if (tseEventoDisabled && (tsePlanoTrabalho.val() == undefined || tsePlanoTrabalho.val() == null || tsePlanoTrabalho.val() == ""))
                    return;
                
                console.log("filtragem ok, filtrando...");
                    
                __doPostBack("FiltraEvento", null);
            }
            catch (ex) {
            }
        }
        
        function clearInputFile(f){
            if(f.value){
                try{
                    f.value = ''; //for IE11, latest Chrome/Firefox/Opera...
                }catch(err){ }
                if(f.value){ //for IE5 ~ IE10
                    var form = document.createElement('form'),
                        parentNode = f.parentNode, ref = f.nextSibling;
                    form.appendChild(f);
                    form.reset();
                    parentNode.insertBefore(f,ref);
                }
            }
            
            if ($(f).attr("data-msg")) {
                var msg = $("#" + $(f).attr("data-msg"));
                msg.html("");
            }
        }
        
        function MascaraMoeda(objTextBox, SeparadorMilesimo, SeparadorDecimal, e, Tamanho) {

            var sep = 0;
            var key = '';
            var i = j = 0;
            var len = len2 = 0;
            var strCheck = '0123456789';
            var aux = aux2 = '';
            var whichCode = (window.Event) ? e.which : e.keyCode;
            if (whichCode == 13) return true;
            if (whichCode == 8) return true;

            key = String.fromCharCode(whichCode); // Valor para o código da Chave
            if (strCheck.indexOf(key) == -1) return false; // Chave inválida
            if (Tamanho < objTextBox.value.length) return false; // Tamanho
            len = objTextBox.value.length;

            for (i = 0; i < len; i++)
                if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != SeparadorDecimal)) break;
            aux = '';
            for (; i < len; i++)
                if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
            aux += key;
            len = aux.length;
            if (len == 0) objTextBox.value = '';
            if (len == 1) objTextBox.value = '0' + SeparadorDecimal + '0' + aux;
            if (len == 2) objTextBox.value = '0' + SeparadorDecimal + aux;
            if (len > 2) {
                aux2 = '';
                for (j = 0, i = len - 3; i >= 0; i--) {
                    if (j == 3) {
                        aux2 += SeparadorMilesimo;
                        j = 0;
                    }
                    aux2 += aux.charAt(i);
                    j++;
                }
                objTextBox.value = '';
                len2 = aux2.length;
                for (i = len2 - 1; i >= 0; i--)
                    objTextBox.value += aux2.charAt(i);
                objTextBox.value += SeparadorDecimal + aux.substr(len - 2, len);
            }
            return false;
        }
        
        function moeda(a, e, r, t) {

            var n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
            if (13 == o || 8 == o)
                return !0;
            if (n = String.fromCharCode(o), -1 == "0123456789".indexOf(n))
                return !1;
            for (u = a.value.length, h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++)
                ;
            for (l = ""; h < u; h++)
-1 != "0123456789".indexOf(a.value.charAt(h)) && (l += a.value.charAt(h));
            if (l += n,
    0 == (u = l.length) && (a.value = ""),
    1 == u && (a.value = "0" + r + "0" + l),
    2 == u && (a.value = "0" + r + l),
    u > 2) {
                for (ajd2 = "",
        j = 0,
        h = u - 3; h >= 0; h--)
                    3 == j && (ajd2 += e,
            j = 0),
            ajd2 += l.charAt(h),
            j++;
                for (a.value = "",
        tamanho2 = ajd2.length,
        h = tamanho2 - 1; h >= 0; h--)
                    a.value += ajd2.charAt(h);
                a.value += r + l.substr(u - 2, u)
            }
            return !1
        }
        
    </script>

    <uc1:FornecedorPopup ID="FornecedorPopup_DC" runat="server" />
</asp:Content>
