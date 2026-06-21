<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalisarEvento.aspx.cs" MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.AnalisarEvento" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Import Namespace="Techne.Lyceum.RN.Util" %>
<%@ Import Namespace="System.Linq" %>

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
        
        .links-download 
        {
            display: inline-block; 
            margin: 5px;
            padding: 10px;
            border: 1px solid #bababa;
            background-color: aqua;
            border-radius: 15px;
        }
    </style>
    
    <script language="javascript">
        function ddlUnidadeMedida_SelectedIndexChanged(s, e) {
            console.log("callback do ddlUnidadeMedida_SelectedIndexChanged");
            
            var ncm = grdItensXml.GetEditor("NCM").GetValue();
            var unidadeMedidaId = grdItensXml.GetEditor("UNIDADEMEDIDAID").GetValue();
            
            if (ncm == undefined || ncm == null || ncm == "" || ncm.indexOf("&") !== -1)
                return;
                
            if (unidadeMedidaId == undefined || unidadeMedidaId == null || unidadeMedidaId == "")
                return;

            var param = {
                Operation: "ddlUnidadeMedida_SelectedIndexChanged",
                NCM: ncm,
                UnidadeMedidaId: unidadeMedidaId
            };
            
            console.log("NCM: " + ncm);
            console.log("UNIDADEMEDIDAID: " + unidadeMedidaId);
            
            grdItensXml.PerformCallback(JSON.stringify(param));
        }
        
        function ddlCodigoFgv_SelectedIndexChanged(s, e) {
            console.log("callback do ddlCodigoFgv_SelectedIndexChanged");

            var ncm = grdItensXml.GetEditor("NCM").GetValue();
            var unidadeMedidaId = grdItensXml.GetEditor("UNIDADEMEDIDAID").GetValue();
            var codigoFgvId = grdItensXml.GetEditor("CODIGO_FGV").GetValue();

            if (ncm == undefined || ncm == null || ncm == "" || ncm.indexOf("&") !== -1)
                return;

            if (unidadeMedidaId == undefined || unidadeMedidaId == null || unidadeMedidaId == "")
                return;

            if (codigoFgvId == undefined || codigoFgvId == null || codigoFgvId == "")
                return;
            
            var param = {
                Operation: "ddlCodigoFgv_SelectedIndexChanged",
                NCM: ncm,
                UnidadeMedidaId: unidadeMedidaId,
                CodigoFgvId: codigoFgvId
            };
            
            console.log("CodigoFgvId: " + codigoFgvId);
            
            grdItensXml.PerformCallback(JSON.stringify(param));
        }

        function btnAprovar_Click() {
            var aprovado = ($(".ExigenciaAprovada").filter((index, el) => $(el).text() == "False").length === 0);
            if (aprovado) {
                msg = "A despesa será validada. Tem certeza?";
            }
            else {
                msg = "A despesa será reprovada. Tem certeza?";
            }
            if (msg) {
                result = confirm(msg);
            }
            return result;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informações da Despesa" Width="800px">
        
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Projeto / Programa:</span>
                </td>
                <td width="600">
                    <asp:Label ID="lblPlanoTrabalho" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Unidade de Ensino:</span>
                </td>
                <td width="600">
                    <asp:Label ID="lblUnidadeEnsino" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Tipo da Despesa:</span>
                </td>
                <td width="600">
                    <asp:Label ID="lblTipoEvento" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Número da Despesa:</span>
                </td>
                <td width="600">
                    <asp:Label ID="lblNumeroEvento" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Finalidade:</span>
                </td>
                <td width="500">
                    <asp:Label ID="lblFinalidade" runat="server"></asp:Label>
                </td>
                <td width="100">
                    <asp:Button ID="btnAprovar" runat="server" Text="Finalizar" OnClick="btnAprovar_Click" OnClientClick="return btnAprovar_Click();" Visible="false" />
                    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
                </td>
            </tr>
        </table>
        
    </asp:Panel>
    
    <br />
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:HiddenField ID="hdnFinalidadeId" runat="server" />
    <asp:HiddenField ID="hdnEventoId" runat="server" />
    <asp:HiddenField ID="hdnCenso" runat="server" />
    <div class="divEditBlock" style="width: 950px;">
        <asp:Label runat="server" ID="lblBlocoAnalisarEvento" Text="Despesas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAnalisarEvento" runat="server" EnableClientScript="true" ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    
    <asp:PlaceHolder ID="plaAnalisarEvento" runat="server" Visible="false">
    
    <br />
    
    <asp:Panel ID="pnlDescricao" runat="server" GroupingText="Descrição" Width="853px">
        <table class="table-fixed" width="600">
            <tr>
                <td width="100" align="right" valign="top">Descrição:</td>
                <td width="500">
                    <asp:TextBox ID="txtDescricao" runat="server" Width="492px" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <br />
    
    <dxtc:ASPxPageControl ID="pcAnalisarEvento" runat="server" ActiveTabIndex="0" Width="850px" ClientInstanceName="pcAnalisarEvento">
        <TabPages>
            
            <%-- Despesa com NF-e --%>
            <dxtc:TabPage Name="tabDespesaComum" Text="Despesa com NF-e" ClientVisible="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:Panel ID="pnlInfoPagamento" runat="server" GroupingText="Informações de Pagamento" Width="825px">                        
                            <table class="table-fixed" width="700" id="tbChaveAcesso" runat="server">
                                <tr>
                                    <td width="100" align="right">
                                        Chave de Acesso: <span style="color: red">*</span>
                                    </td>
                                    <td width="600">
                                        <asp:TextBox ID="txtChaveAcesso" runat="server" Width="490px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">
                                        Número da Nota Fiscal: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:TextBox ID="txtNumeroNF" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="100" align="right">
                                        Valor da Nota Fiscal: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:TextBox ID="txtValorTotalNF" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td width="100">
                                        &nbsp;
                                    </td>
                                    <td width="100" align="right">
                                        Valor Pago NF: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:TextBox ID="txtValorPagoNF" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="100" align="right">
                                        Data da Nota Fiscal: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:TextBox ID="txtDataNF" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td width="100">
                                        &nbsp;
                                    </td>
                                    <td width="100" align="right">
                                        Data de Pagamento NF: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:TextBox ID="txtDataPagamentoNF" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">
                                        Nota Fiscal: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:LinkButton ID="lnkVisualizarNotaFiscal" runat="server" Text="Nota Fiscal Atestada" OnCommand="lnkVisualizarNotaFiscal_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">
                                        Comprovante de Pagamento: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:LinkButton ID="lnkVisualizarComprovantePgto" runat="server" Text="Comprovante de Pagamento" OnCommand="lnkVisualizarComprovantePgto_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="300" id="tbXML" runat="server">
                                <tr>
                                    <td width="100" align="right">
                                        XML: <span style="color: red">*</span>
                                    </td>
                                    <td width="200">
                                        <asp:LinkButton ID="lnkVisualizarXML" runat="server" Text="XML inserido" OnCommand="lnkVisualizarXML_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlInfoFornecedorVencedorOrcamento1" runat="server" GroupingText="Informações Fornecedor Vencedor Orçamento 1" Width="825px">
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="100" align="right">Fornecedor:</td>
                                    <td width="600">
                                        <asp:Label ID="lblFornecedor" runat="server" ReadOnly="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlPesquisaPrecos" runat="server" GroupingText="Informações de Pesquisa de Preços" Width="825px">
                            <table class="table-fixed" width="600">
                                <tr>
                                    <td width="100" align="right">Orçamento 1:</td>
                                    <td width="100"><asp:LinkButton ID="lnkVisualizarOrcamento1" runat="server" Text="Anexo" Visible="false" OnCommand="lnkVisualizarOrcamento_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton></td>
                                    
                                    <td width="100" align="right">Orçamento 2:</td>
                                    <td width="100"><asp:LinkButton ID="lnkVisualizarOrcamento2" runat="server" Text="Anexo" Visible="false" OnCommand="lnkVisualizarOrcamento_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton></td>
                                    
                                    <td width="100" align="right">Orçamento 3:</td>
                                    <td width="100"><asp:LinkButton ID="lnkVisualizarOrcamento3" runat="server" Text="Anexo" Visible="false" OnCommand="lnkVisualizarOrcamento_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton></td>
                                </tr>
                            </table>
                            <br />
                            <table class="table-fixed" width="600">
                                <tr>
                                    <td width="100" align="right" valign="top">Justifique:</td>
                                    <td width="500">
                                        <asp:TextBox ID="txtJustificativa" runat="server" TextMode="MultiLine" Width="492px" Height="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlInfoAdicionais" runat="server" GroupingText="Informações Adicionais" Width="825px">
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="100" align="right" valign="top">Observação:</td>
                                    <td width="600">
                                        <asp:TextBox ID="txtObservacao" runat="server" Width="490px" ReadOnly="true"></asp:TextBox>
                                        <p>
                                            Se valor de pagamento da NF for menor do que valor da NF, justifique no campo acima e anexar carta
                                            de desconto do fornecedor junto com comprovante de pagamento no mesmo documento.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="200">
                                <tr>
                                    <td width="100" align="right">
                                        Evidência:
                                    </td>
                                    <td width="100">
                                        <asp:LinkButton ID="lnkVisualizarEvidencia" runat="server" Text="Anexo" Visible="false" OnCommand="lnkVisualizarEvidencia_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
            <%-- Despesa com Demais Documentos Fiscais --%>
            <dxtc:TabPage Name="tabPequenaDespesaComComprovacao" Text="Despesa com Demais Documentos Fiscais" ClientVisible="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl7" runat="server">
                        <p style="color: red">
                            São despesas para as quais é necessária apresentação tanto do
                            documento fiscal atestado quanto do comprovante de pagamento.
                            O ateste do documento fiscal deve ser realizado por dois servidores
                            lotados na unidade de ensino.
                        </p>
                        <br />
                        <asp:Panel ID="pnlFornecedor_DCC" runat="server" GroupingText="Informações Fornecedor" Width="825px">
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="100" align="right">
                                        Fornecedor: <span style="color: red">*</span>
                                    </td>
                                    <td width="600">
                                        <asp:Label ID="lblFornecedor_DCC" runat="server" Style="display: block"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlInfoPagamento_DCC" runat="server" GroupingText="Informações de Pagamento" Width="825px">
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Forma de Pagamento: <span style="color: red">*</span>
                                    </td>
                                    <td width="550">
                                        <asp:DropDownList ID="ddlFormaPagamento_DCC" runat="server" Width="200px" Enabled="false">
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
                                        <asp:TextBox ID="txtNumeroNF_DCC" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Valor da Nota Fiscal: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:TextBox ID="txtValorNF_DCC" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td width="100">
                                        &nbsp;
                                    </td>
                                    <td width="150" align="right">
                                        Valor Pago NF: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:TextBox ID="txtValorPagoNF_DCC" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Data da Nota Fiscal: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:TextBox ID="txtDataNF_DCC" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td width="100">
                                        &nbsp;
                                    </td>
                                    <td width="150" align="right">
                                        Data de Pagamento NF: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:TextBox ID="txtDataPagamento_DCC" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="150" align="right">
                                        Nota Fiscal: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:LinkButton ID="lnkVisualizarNotaFiscal_DCC" runat="server" Text="Nota Fiscal Atestada" OnCommand="lnkVisualizarNotaFiscal_DCC_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="150" align="right">
                                        Comprovante de Pagamento: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:LinkButton ID="lnkVisualizarComprovantePgto_DCC" runat="server" Text="Comprovante de Pagamento" OnCommand="lnkVisualizarComprovantePgto_DCC_Command" OnClientClick="window.scrollTo(0, 0);"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
            <%-- Pequena Despesa --%>
            <dxtc:TabPage Name="tabPequenaDespesaSemComprovacao" Text="Pequena Despesa" ClientVisible="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl8" runat="server">
                        <p style="color: red">
                            São despesas de pequeno porte, para as quais não é necessária
                            ou não é possível a apresentação de documento fiscal ou de
                            comprovante de pagamento. É necessário apresentar justificativa
                            para a não apresentação da comprovação da despesa.
                        </p>
                        <br />
                        <asp:Panel ID="pnlDespesaSemComprovacao" runat="server" GroupingText="" Width="825px">
                            <asp:Panel ID="pnlFornecedor_DSC" runat="server" GroupingText="Informações Fornecedor" Width="125px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Fornecedor: <span style="color: red">*</span>
                                        </td>
                                        <td width="600">
                                            <asp:Label ID="lblFornecedor_DSC" runat="server" Style="display: block"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlInfoPagamento_DSC" runat="server" GroupingText="Informações de Pagamento" Width="825px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Forma de Pagamento: <span style="color: red">*</span>
                                        </td>
                                        <td width="550">
                                            <asp:DropDownList ID="ddlFormaPagamento_DSC" runat="server" Width="200px" Enabled="false">
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
                                            <asp:TextBox ID="txtValorPago_DSC" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="150" align="right">
                                            Data de Pagamento: <span style="color: red">*</span>
                                        </td>
                                        <td width="150">
                                            <asp:TextBox ID="txtDataPagamento_DSC" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="150" align="right">
                                            Justificativa: <span style="color: red">*</span>
                                        </td>
                                        <td width="550">
                                            <asp:TextBox ID="txtJustificativa_DSC" runat="server" Width="500px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
            <%-- Despesa Com Locomoção de Servidores --%>
            <dxtc:TabPage Name="tabPequenaDespesaComTranslado" Text="Despesa Com Locomoção de Servidores" ClientVisible="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl9" runat="server">
                        <p style="color: red">
                            São despesas realizadas para a locomoção de servidores lotados
                            na unidade, quando em atendimento a atividades requisitadas pela SEEDUC, como reuniões,
                            por exemplo. É necessário justificar a causa da despesa e informar quais servidores
                            foram atendidos.
                        </p>
                        <br />
                        <asp:Panel ID="pnlInfoTranslado_DCTS" runat="server" GroupingText="Informações do Translado" Width="825px">
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Modal de transporte: <span style="color: red">*</span>
                                    </td>
                                    <td width="550">
                                        <asp:DropDownList ID="ddlModalTransporte_DCTS" runat="server" DataTextField="DESCRICAO" DataValueField="TIPOTRANSPORTEID" Width="200px" AppendDataBoundItems="true" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Origem: <span style="color: red">*</span>
                                    </td>
                                    <td width="550">
                                        <asp:TextBox ID="txtOrigem_DCTS" runat="server" Width="540px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Destino: <span style="color: red">*</span>
                                    </td>
                                    <td width="550">
                                        <asp:TextBox ID="txtDestino_DCTS" runat="server" Width="540px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Valor Pago: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:TextBox ID="txtValorPago_DCTS" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td width="100">&nbsp;</td>
                                    <td width="150" align="right">
                                        Data de Pagamento: <span style="color: red">*</span>
                                    </td>
                                    <td width="150">
                                        <asp:TextBox ID="txtDataPagamento_DCTS" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-fixed" width="700">
                                <tr>
                                    <td width="150" align="right">
                                        Justificativa: <span style="color: red">*</span>
                                    </td>
                                    <td width="550">
                                        <asp:TextBox ID="txtJustificativa_DCTS" runat="server" Width="540px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlFornecedor_DCTS" runat="server" GroupingText="Servidores" Width="825px">
                            <dxwgv:ASPxGridView runat="server" ID="grdServidores_DCTS" ClientInstanceName="grdServidores_DCTS"
                                AutoGenerateColumns="False" EnableCallBacks="false" Width="800" KeyFieldName="IdFuncional">
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="false" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="PEQUENADESPESASERVIDORID" FieldName="PequenaDespesaServidorId" VisibleIndex="0" Width="100px" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="PESSOA" FieldName="Pessoa" VisibleIndex="0" Width="100px" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Id Funcional" FieldName="IdFuncional" VisibleIndex="1" Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NomeCompl" VisibleIndex="2" Width="200px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="Matricula" VisibleIndex="3" Width="100px">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
            <dxtc:TabPage Name="tabTransporte" Text="Transporte" ClientVisible="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                    
                        <asp:Panel ID="Panel4" runat="server" GroupingText="Informações de Transporte" Width="825px">
                        
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">Modal:</td>
                                    <td width="200">
                                        <asp:TextBox ID="txtTipoTransporte_T" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">Origem:</td>
                                    <td width="200">
                                        <asp:TextBox ID="txtOrigem_T" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">Destino:</td>
                                    <td width="200">
                                        <asp:TextBox ID="txtDestino_T" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">Valor:</td>
                                    <td width="200">
                                        <asp:TextBox ID="txtValor_T" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">Data Pagamento:</td>
                                    <td width="200">
                                        <asp:TextBox ID="txtDataPagamento_T" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            
                            <table class="table-fixed" width="300">
                                <tr>
                                    <td width="100" align="right">Justificativa:</td>
                                    <td width="200">
                                        <asp:TextBox ID="txtJustificativa_T" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        
                        </asp:Panel>
                    
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
            <dxtc:TabPage Name="tabItemNF" Text="Itens NF" ClientVisible="false">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl5" runat="server">
                    
                        <asp:ObjectDataSource ID="odsUnidadeMedida" runat="server" 
                            TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarEvento" 
                            SelectMethod="ListaUnidadeMedida">
                        </asp:ObjectDataSource>
                    
                        <asp:ObjectDataSource ID="odsItensXml" runat="server" 
                            TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarEvento" 
                            SelectMethod="ListaItensXml" UpdateMethod="UpdateItensXml">
                            <SelectParameters>
                    
                                <asp:ControlParameter ControlID="hdnEventoId" DefaultValue="" Name="eventoId" PropertyName="Value" />
                                <asp:ControlParameter ControlID="hdnCenso" DefaultValue="" Name="censo" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    
                        <dxwgv:ASPxGridView runat="server" ID="grdItensXml"
                            ClientInstanceName="grdItensXml"
                            AutoGenerateColumns="False"
                            EnableCallBacks="true" 
                            Width="1100"
                            DataSourceID="odsItensXml"
                            KeyFieldName="IMPORTACAOXMLEVENTOID" 
                            OnCellEditorInitialize="grdItensXml_CellEditorInitialize"
                            OnCommandButtonInitialize="grdItensXml_CommandButtonInitialize"
                            OnCustomCallback="grdItensXml_CustomCallback" OnRowUpdating="grdItensXml_RowUpdating">
                            
                            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                            
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Caption="" Width="50px">
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataColumn Caption="Item" FieldName="ITEM" VisibleIndex="1" Width="190px">
                                    <CellStyle HorizontalAlign="Left" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NCM" FieldName="NCM" VisibleIndex="2" Width="60px">
                                    <PropertiesTextEdit ClientInstanceName="txtNCM">
                                        <ClientSideEvents TextChanged="ddlUnidadeMedida_SelectedIndexChanged" />
                                    </PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Un. Medida" FieldName="UNIDADEMEDIDAID" VisibleIndex="3" Width="150px">
                                    <PropertiesComboBox DataSourceID="odsUnidadeMedida" TextField="DESCRICAO" ValueField="UNIDADEMEDIDAID" 
                                        ClientInstanceName="ddlUnidadeMedida" EnableSynchronization="False" EnableIncrementalFiltering="True">
                                        <ClientSideEvents SelectedIndexChanged="ddlUnidadeMedida_SelectedIndexChanged" />
                                    </PropertiesComboBox>            
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Código FGV" FieldName="CODIGO_FGV" VisibleIndex="4" Width="150px">
                                    <DataItemTemplate><%# Eval("CODIGOFGV") %></DataItemTemplate>
                                    <PropertiesComboBox ClientInstanceName="ddlCodigoFgv" TextField="DESCRICAO" ValueField="CODIGOFGV">
                                        <ClientSideEvents SelectedIndexChanged="ddlCodigoFgv_SelectedIndexChanged" />
                                    </PropertiesComboBox>
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataColumn Caption="Valor" FieldName="VALORUNITARIO" VisibleIndex="5" Width="100px">
                                    <EditItemTemplate><%# Eval("VALORUNITARIO") %></EditItemTemplate>
                                    <EditCellStyle CssClass="tab-cell"></EditCellStyle>
                                    <CellStyle HorizontalAlign="Right" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Valor FGV" FieldName="VALORFGV" VisibleIndex="6" Width="100px" ReadOnly="true">
                                    <PropertiesTextEdit ClientInstanceName="txtValorFgv"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Diferença" FieldName="DIFERENCA" VisibleIndex="7" Width="100px" ReadOnly="true">
                                    <PropertiesTextEdit ClientInstanceName="txtDiferenca"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="% Diferença" FieldName="PORCENTAGEMDIFERENCA" VisibleIndex="8" Width="100px" ReadOnly="true">
                                    <PropertiesTextEdit ClientInstanceName="txtPorcentagemDiferenca"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataColumn Caption="Itens Não Permitidos" FieldName="NAOPERMITIDO" VisibleIndex="9" Width="100px">
                                    <EditItemTemplate></EditItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet"></CellStyle>
                                    <EditCellStyle HorizontalAlign="Center" VerticalAlign="NotSet"></EditCellStyle>
                                </dxwgv:GridViewDataColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
            <dxtc:TabPage Name="tabExigencias" Text="Exigências">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl4" runat="server">
                    
                        <asp:ObjectDataSource ID="odsMotivoExigenciaEvento" runat="server"
                            TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarEvento" 
                            SelectMethod="ListaMotivoExigenciaEvento">
                        </asp:ObjectDataSource>
                    
                        <asp:ObjectDataSource ID="odsExigenciaEvento" runat="server" 
                            TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarEvento" 
                            SelectMethod="ListaExigenciaEvento"
                            InsertMethod="InsertExigenciaEvento"
                            UpdateMethod="UpdateExigenciaEvento"
                            DeleteMethod="DeleteExigenciaEvento">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnEventoId" DefaultValue="" Name="eventoId" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    
                        <dxwgv:ASPxGridView runat="server" ID="grdExigenciaEvento"
                            ClientInstanceName="grdExigenciaEvento"
                            AutoGenerateColumns="False"
                            EnableCallBacks="false" 
                            Width="800"
                            DataSourceID="odsExigenciaEvento"
                            KeyFieldName="EXIGENCIAEVENTOID"
                            OnCommandButtonInitialize="grdExigenciaEvento_CommandButtonInitialize"
                            OnRowInserting="grdExigenciaEvento_RowInserting"
                            OnRowUpdating="grdExigenciaEvento_RowUpdating"
                            OnRowDeleting="grdExigenciaEvento_RowDeleting"
                            OnCustomButtonInitialize="grdExigenciaEvento_CustomButtonInitialize"
                            OnCustomButtonCallback="grdExigenciaEvento_CustomButtonCallback">
                            
                            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                            
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Caption="" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <asp:ImageButton ID="btnNovaExigenciaEvento" runat="server" AlternateText="Novo" ImageUrl="~/img/bt_novo.png" OnClientClick="grdExigenciaEvento.AddNewRow(); return false;" OnLoad="btnNovaExigenciaEvento_Load" style="cursor: pointer" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnAprovarExigencia" Text="Aprovar Exigência" Visibility="Invisible">
                                            <Image Url="~/img/bt_ok.png" />
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnRejeitarExigencia" Text="Rejeitar Exigência" Visibility="Invisible">
                                            <Image Url="~/img/bt_desabilitar.png" />
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Exigência" FieldName="MOTIVOEXIGENCIAEVENTOID" VisibleIndex="1" Width="200px">
                                    <PropertiesComboBox DataSourceID="odsMotivoExigenciaEvento" TextField="DESCRICAO" ValueField="MOTIVOEXIGENCIAEVENTOID"></PropertiesComboBox>
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                                    </CellStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nota Explicativa" FieldName="NOTAEXPLICATIVA" VisibleIndex="2" Width="200px">
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" VisibleIndex="3" Width="200px">
                                    <EditItemTemplate><%# Eval("JUSTIFICATIVA") %></EditItemTemplate>
                                    <CellStyle HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </CellStyle>
                                    <EditCellStyle CssClass="tab-cell" HorizontalAlign="Justify" VerticalAlign="Middle">
                                    </EditCellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Visualizar" VisibleIndex="8" Width="50px">
                                    <DataItemTemplate>
                                        <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EXIGENCIAEVENTOID") + "," + Eval("TIPOARQUIVO") %>'
                                            OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                                            AlternateText="Visualizar Documento" Visible='<%# Eval("EXIGENCIAEVENTOID") != DBNull.Value && Eval("EXIGENCIAEVENTOARQUIVOID") != DBNull.Value %>'></asp:ImageButton>
                                    </DataItemTemplate>
                                    <EditItemTemplate></EditItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataColumn Caption="Aprovado" FieldName="APROVADO" VisibleIndex="6" Width="50px">
                                    <DataItemTemplate><%# Convert.ToBoolean(Eval("APROVADO")) ? "Sim" : "Não" %><span class="ExigenciaAprovada" style="display: none;"><%# Convert.ToBoolean(Eval("APROVADO")) %></span></DataItemTemplate>
                                    <EditItemTemplate></EditItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Rejeitado" FieldName="REJEITADO" VisibleIndex="7" Width="50px">
                                    <DataItemTemplate><%# Convert.ToBoolean(Eval("REJEITADO")) ? "Sim" : "Não" %><span class="ExigenciaRejeitada" style="display: none;"><%# Convert.ToBoolean(Eval("REJEITADO")) %></span></DataItemTemplate>
                                    <EditItemTemplate></EditItemTemplate>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="TIPOARQUIVO" Visible="false"></dxwgv:GridViewDataColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                                            
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            
        </TabPages>
    </dxtc:ASPxPageControl>
    
    <br />
    
    <asp:ObjectDataSource ID="odsArquivoEvento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarEvento" SelectMethod="ListaTodosOsArquivosDaDespesa">
        <SelectParameters>
            <asp:ControlParameter DbType="Int32" ControlID="hdnEventoId" PropertyName="Value" Name="eventoId" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
    <asp:Panel ID="pnlDownloadArquivos" runat="server" GroupingText="Todos os arquivos desta despesa" Width="853px">
        <asp:Repeater ID="repCarrossel" runat="server" DataSourceID="odsArquivoEvento">
            <ItemTemplate>
                <a href="FileCS.ashx?Tabela=<%# Eval("Tabela") %>&Id=<%# Eval("ArquivoId") %>&TipoArquivo=<%# Eval("TipoArquivo") %>" target="_blank" style="text-decoration: none;">
                    <div class="links-download">
                    <%# DataBinder.Eval(Container.DataItem, "Tabela").ToString()
                               .Replace("EventoNotaFiscalArquivo", "NOTA FISCAL")
                               .Replace("ComprovantePagamentoArquivo", "COMPROVANTE DE PAGAMENTO")
                               .Replace("OrcamentoArquivo", "ORÇAMENTO")
                               .Replace("EvidenciaArquivo", "EVIDÊNCIA")
                               .Replace("ExigenciaEventoArquivo2", "EXIGÊNCIA")
                            %>
                    </div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </asp:Panel>
    
    <br />

    <asp:Panel ID="pnlStatusAnalise" runat="server" GroupingText="Status da Análise" Width="853px">    
        <asp:Label ID="lblStatusEvento" runat="server" Text="" Style="font-weight: bold; font-size: 14px;" />
    </asp:Panel>
    
    </asp:PlaceHolder>
    
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
                
                <asp:ObjectDataSource ID="odsXML" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AnalisarEvento" SelectMethod="ListaXML">
                    <SelectParameters>
                       <asp:ControlParameter ControlID="hdnEventoId" DefaultValue="" Name="eventoId" PropertyName="Value" />
                                <asp:ControlParameter ControlID="hdnCenso" DefaultValue="" Name="censo" PropertyName="Value" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            
                <dxwgv:ASPxGridView runat="server" ID="grdXML"
                    ClientInstanceName="grdXML"
                    AutoGenerateColumns="False"
                    EnableCallBacks="false" 
                    Width="770"
                    DataSourceID="odsXML"
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
                        <dxwgv:GridViewDataTextColumn Caption="Un. Medida" FieldName="UNIDADEMEDIDA" VisibleIndex="3" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Cód. FGV" FieldName="CODIGOFGV" VisibleIndex="4" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Qtd." FieldName="QUANTIDADE" VisibleIndex="5" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Vr. Unitário" FieldName="VALORUNITARIO" VisibleIndex="6" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Vr. Pago" FieldName="VALORPAGO" VisibleIndex="7" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Vr. FGV" FieldName="VALORFGV" VisibleIndex="8" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Diferença" FieldName="DIFERENCA" VisibleIndex="9" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Dif. %" FieldName="PORCENTAGEMDIFERENCA" VisibleIndex="10" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Permitido" FieldName="NAOPERMITIDO" VisibleIndex="11" Width="70px">
                            <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                            </CellStyle>
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
                
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        
    </dxpc:ASPxPopupControl>
    
</asp:Content>
