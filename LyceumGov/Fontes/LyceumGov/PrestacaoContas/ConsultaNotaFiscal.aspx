<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="ConsultaNotaFiscal.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ConsultaNotaFiscal" %>
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
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para pesquisa" Width="800px">
    
        <%-- Pesquisa --%>
        <table style="width:100%" >
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblChave" runat="server" Text="Chave de acesso:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <input id="txtChave" runat="server" style="width:80%" class="txtInput"/>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="2" style="text-align: right;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
                    </td>
                </tr>
            </table>
           
    </asp:Panel>

        
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
        
            <div class="divEditBlock" style="width: 950px;">
                <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" Visible="false"
                    OnClick="btnCancel_Click" />
                <asp:Label runat="server" ID="lblBlocoCadastrarEvento" Text="Consulta - Nota Fiscal" SkinID="BcTitulo" />
            </div>
    
    <asp:PlaceHolder ID="plaConsultaNotaFiscal" runat="server" Visible="false">
        <%-- Despesas --%>
        <asp:Panel ID="pnlDespesa" runat="server" GroupingText="Tipo de Despesa *" Width="853px"
            Style="color: Blue">
            <table class="table-fixed" width="800">
                    <tr>
                        <td width="600">
                            <asp:RadioButtonList ID="rblFiltroTipoEvento" runat="server" AutoPostBack="true" Enabled=false> 
                                <asp:ListItem Text="Despesas com documentos fiscais" Value="0" />
                                <asp:ListItem Text="Demais Despesas" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
            </table>
        </asp:Panel>
        
        <br />
        
        <%-- Tipo Documento --%>
        <asp:Panel ID="pnlTipoDocumentoFiscal" runat="server" GroupingText="Tipo de Documento Fiscal *"
            Width="853px" Style="color: Blue">
            <table class="table-fixed" width="800">
                    <tr>
                        <td width="600">
                            <asp:RadioButtonList ID="rblDocumentoFiscal" runat="server" Enabled=false>
                                <asp:ListItem Text="NF-e" Value="0" />
                                <asp:ListItem Text="Demais Documentos Fiscais" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
            </table>
        </asp:Panel>
        
        <br />
        
        <%-- Descrição --%>
        <asp:Panel ID="pnlDescricao" runat="server" GroupingText="Descrição *" Width="853px"
            Style="color: Blue">
            <table class="table-fixed" width="600">
                <tr>
                    <td width="500">
                        <asp:TextBox ID="txtDescricao" runat="server" Width="492px" ReadOnly=true></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        
        <br />
        
        <dxtc:ASPxPageControl ID="pcCadastrarEvento" runat="server" ActiveTabIndex="0" Width="850px">
            <TabPages>
                <%-- Despesas com NF-e --%>
                <dxtc:TabPage Name="tabDespesaComum" Text="Despesas com NF-e ou Demais Documentos Fiscais">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl6" runat="server">
                            <br />
                            <asp:Panel ID="pnlInfoFornecedorVencedorOrcamento1" runat="server" GroupingText="Informações Fornecedor Vencedor Orçamento 1"
                                Width="825px">
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Fornecedor:
                                        </td>
                                        <td width="600">
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
                                            Orçamento 1:
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
                                                Height="80px" style="resize:none" ReadOnly=true></asp:TextBox>
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
                                            Chave de Acesso:
                                        </td>
                                        <td width="600">
                                            <asp:TextBox ID="txtChaveAcesso" runat="server" Width="490px" ReadOnly=true></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="100" align="right">
                                            Número da Nota Fiscal:
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtNumeroNF" runat="server" Width="490px" ReadOnly=true></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Valor da Nota Fiscal:
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtValorTotalNF" runat="server" Width="190px" CssClass="numeric" ReadOnly=true
                                                OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                        </td>
                                        <td width="100">
                                            &nbsp;
                                        </td>
                                        <td width="100" align="right">
                                            Valor Pago NF:
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtValorPagoNF" runat="server" Width="190px" CssClass="numeric" ReadOnly=true
                                                OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="700">
                                    <tr>
                                        <td width="100" align="right">
                                            Data da Nota Fiscal:
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtDataNF" runat="server" Width="190px" CssClass="date" ReadOnly=true></asp:TextBox>
                                        </td>
                                        <td width="100">
                                            &nbsp;
                                        </td>
                                        <td width="100" align="right">
                                            Data de Pagamento NF:
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtDataPagamentoNF" runat="server" Width="190px" CssClass="date" ReadOnly=true></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="100" align="right">
                                            Nota Fiscal:
                                        </td>
                                        <td width="170">
                                            <asp:LinkButton ID="lnkVisualizarNotaFiscal" runat="server" Text="Nota Fiscal Atestada"
                                                OnCommand="lnkVisualizarNotaFiscal_Command" OnClientClick="window.scrollTo(0, 0);" Visible=false></asp:LinkButton>
                                            <span id="spanNotaFiscalAnexada"></span>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300">
                                    <tr>
                                        <td width="100" align="right">
                                            Comprovante de Pagamento:
                                        </td>
                                        <td width="170">
                                            <asp:LinkButton ID="lnkVisualizarComprovantePgto" runat="server" Text="Comprovante de Pagamento"
                                                OnCommand="lnkVisualizarComprovantePgto_Command" OnClientClick="window.scrollTo(0, 0);" Visible=false></asp:LinkButton>
                                            <span id="spanComprovantePgtoAnexado"></span>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="300" id="tbXML" runat="server">
                                    <tr>
                                        <td width="100" align="right">
                                            XML:
                                        </td>
                                        <td width="170">
                                            <asp:LinkButton ID="lnkVisualizarXML" runat="server" Text="XML inserido" 
                                                OnCommand="lnkVisualizarXML_Command" OnClientClick="window.scrollTo(0, 0);" Visible=false></asp:LinkButton>
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
                                            <asp:TextBox ID="txtObservacao" runat="server" Width="490px" ReadOnly=true></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-fixed" width="200">
                                    <tr>
                                        <td width="100" align="right">
                                            Evidência:
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
                <%-- Exigências --%>
                <dxtc:TabPage Name="tabExigencias" Text="Exigências">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl4" runat="server">
                            <asp:ObjectDataSource ID="odsExigenciaEvento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.ConsultaNotaFiscal"
                                SelectMethod="ListaExigenciaEvento" UpdateMethod="UpdateExigenciaEvento">
                                <SelectParameters>
                                    <asp:ControlParameter 
                                        Name="chave"
                                        ControlID="txtChave"
                                        PropertyName="Value"
                                        Type="String" />
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
                                Exigência de taxas bancárias não reembolsadas
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
                                <asp:ObjectDataSource ID="odsItensNf" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.ConsultaNotaFiscal"
                                    SelectMethod="ListaXML">
                                    <SelectParameters>
                                        <asp:ControlParameter 
                                            Name="chave"
                                            ControlID="txtChave"
                                            PropertyName="Value"
                                            Type="String" />
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
                                        <dxwgv:GridViewDataTextColumn Caption="Cód. FGV" FieldName="CODIGOFGV" VisibleIndex="4"
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
                                        <dxwgv:GridViewDataTextColumn Caption="Vr. FGV" FieldName="VALORFGV" VisibleIndex="8"
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
            </TabPages>
        </dxtc:ASPxPageControl>
        <br />
        
        <%-- Status Analise  --%>
        <asp:Panel ID="pnlStatusAnalise" runat="server" GroupingText="Status da Análise"
            Width="853px">
            <asp:Label ID="lblStatusEvento" runat="server" Text="" Visible="false" Style="font-weight: bold;
                font-size: 14px;" />
        </asp:Panel>
    </asp:PlaceHolder>
       
    <%-- Popup visualizar arquivo --%> 
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
    
    <%-- Popup visualizar xml --%>
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
                <dxwgv:ASPxGridView runat="server" ID="grdXML" ClientInstanceName="grdXML" AutoGenerateColumns="False"
                    EnableCallBacks="false" Width="770" KeyFieldName="IMPORTACAOXMLEVENTOID" OnDataBinding="grdXML_DataBinding">
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
                        <dxwgv:GridViewDataTextColumn Caption="Cód. FGV" FieldName="CODIGOFGV" VisibleIndex="4"
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
                        <dxwgv:GridViewDataTextColumn Caption="Vr. FGV" FieldName="VALORFGV" VisibleIndex="8"
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
            
            let dataSize = atob(dataUrl.split(";")[2].split(",")[1]).length;
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

</asp:Content>