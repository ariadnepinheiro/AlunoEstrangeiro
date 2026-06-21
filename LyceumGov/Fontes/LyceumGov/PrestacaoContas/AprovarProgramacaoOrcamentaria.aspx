<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="AprovarProgramacaoOrcamentaria.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.AprovarProgramacaoOrcamentaria" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Import Namespace="Techne.Lyceum.RN.Util" %>
<%@ Import Namespace="System.Linq" %>
    
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
        function OnAcaoChanged(ddlAcao, ddlMotivo) {
            if (ddlAcao.GetValue() == "N") //Reprovar
            {
                ddlMotivo.SetEnabled(true);
            }
            else {
                ddlMotivo.SetText("");
                ddlMotivo.SetEnabled(false);
            }
        }
       
    </script>
  <asp:HiddenField ID="hdnNumProcesso" runat="server" />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Dados da Programação Orçamentária"
         
        Width="50%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Ano Referência:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAnoCadastro" runat="server" DataTextField="ano" DataValueField="ano"
                        OnSelectedIndexChanged="ddlAnoCadastro_SelectedIndexChanged" 
                        AutoPostBack="true" onload="ddlAnoCadastro_Load">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label10" runat="server" Font-Names="Verdana" Text="Num. Processo:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseNumProcesso" runat="server" Key="planilhaorcamentariaid"
                        Argument="PROCESSO" MaxLength="22" ArgumentColumns="50" Columns="10" AutoPostBack="true" 
                        SqlSelect="select descricao,planilhaorcamentariaid from PRESTACAOCONTAS.PLANILHAORCAMENTARIA"
                        OnChanged="tseNumProcesso_Changed" 
                        GridWidth="850px" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="id" FieldName="planilhaorcamentariaid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Num.Processo" FieldName="PROCESSO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
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
    <div class="divEditBlock" style="width: 950px;">
        <asp:Label runat="server" ID="lblBloco" Text="Analise de Programação Orçamentária"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsFornecedor" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnlObrigacoesFiscais" runat="server" Visible="false">
        <asp:ObjectDataSource ID="odsPlanilhaOrcamentaria" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarProgramacaoOrcamentaria"
            runat="server" SelectMethod="ListaDadosGridPor" UpdateMethod="Update">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlAnoCadastro" DefaultValue="" Name="ano"      PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="hdnNumProcesso" DefaultValue="" Name="processo" PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsItemPlanilha" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarProgramacaoOrcamentaria"
            runat="server" SelectMethod="ListaDadosGridPopupPor">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnProgramaOrcamentarioId" DefaultValue="" Name="planilhaOrcamentariaId"
                    PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsMotivo" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarProgramacaoOrcamentaria"
            SelectMethod="ListaMotivo"></asp:ObjectDataSource>
        <asp:HiddenField ID="hdnItemPlanilhaOrcamentariaId" runat="server" />
        <asp:HiddenField ID="hdnLancamentoRepasseId" runat="server" />
        <asp:HiddenField ID="hdnLancRepasseId" runat="server" />
        <asp:HiddenField ID="hdnWsRepasseDefazId" runat="server" />
        <asp:HiddenField ID="hdnItemplaORc" runat="server" />
        <asp:HiddenField ID="hdnProgramaOrcamentarioId" runat="server" />
 
        <asp:HiddenField ID="hdnAnalisePlanilhaOrcamentariaId" runat="server" />
        <dxwgv:ASPxGridView ClientInstanceName="grdPlanilhaOrcamentaria" ID="grdPlanilhaOrcamentaria"
            runat="server" Width="100%" DataSourceID="odsPlanilhaOrcamentaria" KeyFieldName="PLANILHAORCAMENTARIAID"
            OnCustomButtonCallback="grdPlanilhaOrcamentaria_CustomButtonCallback" EnableCallBacks="false"
            OnAfterPerformCallback="grdPlanilhaOrcamentaria_AfterPerformCallback" OnCommandButtonInitialize="grdPlanilhaOrcamentaria_CommandButtonInitialize"
            OnRowUpdating="grdPlanilhaOrcamentaria_RowUpdating">
            <settingsbehavior allowmultiselection="False" allowsort="False" />
            <settingscookies enabled="false" />
            <settingstext emptydatarow="Não existem dados." />
            <styles header-horizontalalign="Center" cell-horizontalalign="Center" cell-verticalalign="Middle" />
            <settingsediting mode="Inline" />
            <settingstext confirmdelete="Confirma a remoção?" emptydatarow="Não existem dados." />
            <settingsbehavior confirmdelete="true" />
            <columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                    <HeaderCaptionTemplate>
                    </HeaderCaptionTemplate>
                    <CancelButton Visible="true" Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                    <EditButton Text="Editar" Visible="True">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <UpdateButton Text="Salvar">
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Visualizar" ID="btnVizualizar" Visibility="AllDataRows"
                            Image-Url="~/img/bt_busca.png" Image-Height="15px" Image-AlternateText="Visualizar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ANALISEPLANILHAORCAMENTARIAID" FieldName="ANALISEPLANILHAORCAMENTARIAID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="PLANILHAORCAMENTARIAID" FieldName="PLANILHAORCAMENTARIAID"
                    VisibleIndex="2" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano Referência" ReadOnly="True" FieldName="ANO"
                    VisibleIndex="2" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Num. Processo" ReadOnly="True" FieldName="PROCESSO"
                    VisibleIndex="3" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Descrição" ReadOnly="True" FieldName="DESCRICAO"
                    VisibleIndex="4" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" Caption="Programa de Trabalho" FieldName="PROGRAMATRABALHOID"
                    VisibleIndex="5" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" Caption="PT" FieldName="PT"
                    VisibleIndex="6" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" Caption="Programa de Trabalho" FieldName="PROGRAMATRABALHO"
                    VisibleIndex="7" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" Caption="Projeto / Programa" FieldName="PLANOTRABALHOID"
                    VisibleIndex="8" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn ReadOnly="True" Caption="Natureza Despesa" FieldName="NATUREZADESPESAID"
                    VisibleIndex="9" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Região" ReadOnly="True" FieldName="REGIAOFINANCEIRAID"
                    VisibleIndex="10" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Valor Total" ReadOnly="True" FieldName="VALORTOTAL"
                    VisibleIndex="11" Visible="true">
                     <PropertiesTextEdit MaxLength="11" Width="150px" DisplayFormatString="c">
                                            <MaskSettings Mask="$&lt;0..9999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Ação" FieldName="ACAO" VisibleIndex="12"
                    Width="110px">
                    <PropertiesComboBox ValueType="System.String" Width="110px" ClientInstanceName="ddlAcao">
                        <Items>
                            <dxe:ListEditItem Text="" Value="SELECIONE" />
                            <dxe:ListEditItem Text="Aprovar" Value="S" />
                            <dxe:ListEditItem Text="Reprovar" Value="N" />
                        </Items>
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OnAcaoChanged(ddlAcao, ddlMotivo);}" />
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Motivo Reprovação" HeaderStyle-Font-Bold="true"
                    FieldName="MOTIVOREPROVACAO" VisibleIndex="13" Width="150px">
                    <PropertiesComboBox DataSourceID="odsMotivo" TextField="DESCRICAO" ValueField="MOTIVOREPROVACAOPLANILHAORCAMENTARIAID"
                        ValueType="System.String" ClientInstanceName="ddlMotivo">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
            </columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <br />
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="true"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="500px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Parcelas do Lancameto de Repasse">
        <headerstyle horizontalalign="Center" />
        <border bordercolor="Gainsboro" borderstyle="Solid" borderwidth="2px" />
        <contentstyle verticalalign="Top">
        </contentstyle>
        <sizegripimage height="12px" width="12px" />
        <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <contentcollection>
            <dxpc:PopupControlContentControl>
                <div align="center">
                    <dxwgv:ASPxGridView ClientInstanceName="grdItemPlanilha" ID="grdItemPlanilha" runat="server"
                        Width="100%" DataSourceID="odsItemPlanilha" KeyFieldName="ITEMPLANILHAORCAMENTARIAID"
                        EnableCallBacks="false">
                        <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                        <SettingsCookies Enabled="false" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" 
                            Cell-VerticalAlign="Middle" >
<Header HorizontalAlign="Center"></Header>

<Cell HorizontalAlign="Center" VerticalAlign="Middle"></Cell>
                        </styles>
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <SettingsBehavior ConfirmDelete="true" />
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Referência" FieldName="REFERENCIA" VisibleIndex="0"
                                Visible="true">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Fonte de Recurso" FieldName="FONTERECURSOID"
                                VisibleIndex="1" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                             <dxwgv:GridViewDataTextColumn Caption="Fonte de Recurso" FieldName="FONTERECURSO_DESCRICAO"
                                VisibleIndex="2" Visible="true">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALOR" VisibleIndex="2"
                                Visible="true">
                                <PropertiesTextEdit MaxLength="11" Width="150px" DisplayFormatString="c">
                                            

<MaskSettings Mask="$&lt;0..9999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        

</PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Estimado/Faturado" FieldName="ESTIMADOFATURADO"
                                VisibleIndex="4" Visible="true">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                    </dxwgv:ASPxGridView>
                    <asp:Label ID="lblTotal" runat="server" Text="Total Parcelas:"></asp:Label>
                    <asp:Label ID="lblTotalParcelas" runat="server" Text=""></asp:Label>
                </div>
            </dxpc:PopupControlContentControl>
        </contentcollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
