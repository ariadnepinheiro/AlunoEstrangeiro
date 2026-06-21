<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="CadastrarProgramacaoOrcamentaria.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.CadastrarProgramacaoOrcamentaria" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 60px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(s) {
            if (s.cpAtualizar != undefined) {
                if (s.cpAtualizar != null) {
                    $("#<%= this.lblTotalParcelas.ClientID %>").text(s.cpAtualizar);
                    s.cpAtualizar = null;
                }
            }
        }
    </script>

    <style>
        .cursorImagem
        {
            cursor: pointer;
        }
        .txtInput
        {
            background-color: White;
            font-family: Verdana;
            font-size: smaller;
        }
    </style>

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informações da Programação Orçamentária"
        Width="50%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Ano Referência:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="dpdAnoRef" runat="server" DataTextField="ano" DataValueField="ano"
                        OnSelectedIndexChanged="dpdAnoRef_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label10" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Número Processo:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseProcesso" runat="server" Argument="DESCRICAO" Key="IDENTIFICADOR"
                        MaxLength="22" GridWidth="950px" SqlSelect="SELECT PLANILHAORCAMENTARIAID, PLANILHAORCAMENTARIA,DESCRICAOCOMPLETA,descsefaz,NATUREZADESPESA FROM [PrestacaoContas].VW_PROCESSOPLANILHAORCAMENTARIA">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Número Processo" FieldName="IDENTIFICADOR" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição Completa da Despesa" FieldName="DESCRICAOCOMPLETA"
                                Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Programa de Trabalho" FieldName="descsefaz" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Projeto/Programa" FieldName="PLANILHAORCAMENTARIA"
                                Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Natureza da Despesa" FieldName="NATUREZADESPESA"
                                Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Id" FieldName="PLANILHAORCAMENTARIAID" Width="0%"
                                Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 70%;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" />
        <asp:ValidationSummary ID="vsFornecedor" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnProgamacaoOrcamentaria" runat="server">
        <asp:ObjectDataSource ID="odsFonteRecurso" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.CadastrarProgramacaoOrcamentaria"
            SelectMethod="ListaFonteRecurso"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsItemPlanilhaOrcamentaria" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.CadastrarProgramacaoOrcamentaria"
            SelectMethod="ListaItemPlanilhaOrcamentaria" InsertMethod="Insert" DeleteMethod="Delete"
            UpdateMethod="Update">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseProcesso" PropertyName="DBValue" Name="planilhaOrcamentariaId" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="hdnItemPlanilhaOrcamentariaId" runat="server" />
        <dxtc:ASPxPageControl ID="pcPlanilhaOrcamentaria" runat="server" ActiveTabIndex="0"
            OnTabClick="pcPlanilhaOrcamentaria_TabClick" Width="70%">
            <TabPages>
                <dxtc:TabPage Name="tabGeral" Text="Programação Orçamentária">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblStatus" runat="server" Font-Names="Verdana" Text="Status:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStatusRetorno" runat="server" Font-Names="Verdana" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMotivo" runat="server" Font-Names="Verdana" Text="Motivo:"></asp:Label>
                                        <asp:Label ID="lblMotivoRetorno" runat="server" Font-Names="Verdana" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lbl" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana" Text="Número Processo*:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNumProcesso" runat="server" MaxLength="22" Width="140px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDescricaoComplDespesa" SkinID="lblObrigatorio" runat="server" Text="Descricao Completa Despesa*:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDescricaoComplDespesa" runat="server" Width="600px" MaxLength="255" />
                                        <asp:TextBox ID="PlanilhaOrcamentariaId" Visible="false" runat="server" Width="600px"
                                            MaxLength="255" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblProgramaTrabalho" SkinID="lblObrigatorio" runat="server" Text="Programa de Trabalho*:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <tweb:TSearchBox ID="tseProgramaTrabalho" runat="server" Argument="descricao" Key="programatrabalhoid"
                                            MaxLength="9" DataType="VarChar" SqlSelect="select usuarioid from prestacaocontas.vw_ts_programatrabalho "
                                            SqlWhere=" ATIVO = 1">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="programatrabalhoid" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="90%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblPlanoTrabalho" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                                            Text="Projeto / Programa*:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                                            MaxLength="9" DataType="VarChar" SqlSelect=" select pt.descricao,pt.IDENTIFICADOR from  PrestacaoContas.VW_PLANOTRABALHO PT "
                                            SqlWhere=" pt.PROGRAMATRABALHOID = #tseProgramaTrabalho# ">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                                                <tweb:TSearchBoxColumn Caption="Identificador" FieldName="IDENTIFICADOR" Width="90%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblEmail" runat="server"
                                            SkinID="lblObrigatorio" Text="Natureza de despesa*:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <tweb:TSearchBox ID="tseNaturezaDespeza" runat="server" Argument="DESCRICAO" Key="NATUREZADESPESAID"
                                            MaxLength="9" DataType="VarChar" SqlSelect=" SELECT  CODIGOSEFAZ FROM PrestacaoContas.VW_NATUREZADESPESA (NOLOCK)"
                                            SqlWhere=" ATIVO = 1">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="NATUREZADESPESAID" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="70%" />
                                                <tweb:TSearchBoxColumn Caption="Código Sefaz" FieldName="CODIGOSEFAZ" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblRegiaoFinanceira" SkinID="lblObrigatorio" runat="server" Text="Região Financeira*: "></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlRegiaoFinanceira" runat="server" DataTextField="DESCRICAO"
                                            DataValueField="REGIAOFINANCEIRAID" AutoPostBack="true" Width="600px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label14" SkinID="lblObrigatorio" runat="server" Text="Ano*: "></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlAnoCadastro" runat="server" DataTextField="ano" DataValueField="ano"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="tabParcela" Text="Parcelas do Programa" Visible="true">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl4" runat="server">
                            <dxwgv:ASPxGridView ID="grdItemPlanilhaOrcamentaria" ClientInstanceName="grdItemPlanilhaOrcamentaria"
                                runat="server" Width="100%" DataSourceID="odsItemPlanilhaOrcamentaria" KeyFieldName="ITEMPLANILHAORCAMENTARIAID"
                                OnRowDeleting="grdItemPlanilhaOrcamentaria_RowDeleting" OnRowUpdating="grdItemPlanilhaOrcamentaria_RowUpdating"
                                OnRowUpdated="grdItemPlanilhaOrcamentaria_OnRowUpdated" OnRowInserted="grdItemPlanilhaOrcamentaria_OnRowInserted"
                                OnRowDeleted="grdItemPlanilhaOrcamentaria_OnRowDeleted" OnRowInserting="grdItemPlanilhaOrcamentaria_RowInserting"
                                OnCommandButtonInitialize="grdItemPlanilhaOrcamentaria_CommandButtonInitialize"
                                EnableCallBacks="true">
                                <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center">
                                                <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                    onclick="grdItemPlanilhaOrcamentaria.AddNewRow();" />
                                            </div>
                                        </HeaderCaptionTemplate>
                                        <CancelButton Visible="true" Text="Cancelar">
                                            <Image Url="~/img/bt_cancelar.png" />
                                        </CancelButton>
                                        <EditButton Visible="True" Text="Editar">
                                            <Image Url="../img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Visible="True" Text="Remover">
                                            <Image Url="../img/bt_exclui2.png" />
                                        </DeleteButton>
                                        <ClearFilterButton Text="Limpar" Visible="True">
                                            <Image Url="~/img/bt_limpa.png" />
                                        </ClearFilterButton>
                                        <UpdateButton Visible="true" Text="Alterar">
                                            <Image Url="../img/bt_salvar.png" />
                                        </UpdateButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ITEMPLANILHAORCAMENTARIAID" FieldName="ITEMPLANILHAORCAMENTARIAID"
                                        VisibleIndex="1" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="PLANILHAORCAMENTARIAID" FieldName="PLANILHAORCAMENTARIAID"
                                        VisibleIndex="2" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Referência" FieldName="REFERENCIA" VisibleIndex="3"
                                        Width="110px">
                                        <PropertiesComboBox ValueType="System.String" Width="110px">
                                            <Items>
                                                <dxe:ListEditItem Text="Janeiro" Value="1" />
                                                <dxe:ListEditItem Text="Fevereiro" Value="2" />
                                                <dxe:ListEditItem Text="Março" Value="3" />
                                                <dxe:ListEditItem Text="Abril" Value="4" />
                                                <dxe:ListEditItem Text="Maio" Value="5" />
                                                <dxe:ListEditItem Text="Junho" Value="6" />
                                                <dxe:ListEditItem Text="Julho" Value="7" />
                                                <dxe:ListEditItem Text="Agosto" Value="8" />
                                                <dxe:ListEditItem Text="Setembro" Value="9" />
                                                <dxe:ListEditItem Text="Outubro" Value="10" />
                                                <dxe:ListEditItem Text="Novembro" Value="11" />
                                                <dxe:ListEditItem Text="Dezembro" Value="12" />
                                            </Items>
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a referência." IsRequired="true" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Fonte de Recurso*" HeaderStyle-Font-Bold="true"
                                        FieldName="FONTERECURSOID" VisibleIndex="4" Width="150px">
                                        <PropertiesComboBox DataSourceID="odsFonteRecurso" TextField="DESCRICAOCOMPLETA"
                                            ValueField="FONTERECURSOID" ValueType="System.String">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar Fonte Recurso ." IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALOR" VisibleIndex="5">
                                        <PropertiesTextEdit MaxLength="11" Width="150px" DisplayFormatString="c">
                                            <MaskSettings Mask="$&lt;0..9999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Estimado/Faturado" FieldName="RETORNOREFERENCIA"
                                        VisibleIndex="6" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Estimado/Faturado" FieldName="DESCRICAORETORNOREFERENCIA"
                                        VisibleIndex="6" ReadOnly="true">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                            <asp:Label ID="Label2" runat="server" Text="Total Parcelas:" Style="font-family: Verdana;
                                font-size: Small;"></asp:Label>
                            <asp:Label ID="lblTotalParcelas" runat="server" Style="font-family: Verdana; font-size: Small;"></asp:Label>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
</asp:Content>
