<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Contrato.aspx.cs" Inherits="Techne.Lyceum.Net.Interconectividade.Contrato" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        $(document).ready(function() {
            AddEvents();
        });

        function AddEvents() {


        }

        function onKeyUpOrChange(evt) {
            var newItem = evt.GetValue();
        }
      
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa, ano e mês"
        Width="650px">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td style="width: 450px">
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect=" SELECT DISTINCT s.setor, nome, ue.UNIDADE_ENS, ua_atual, ua_antiga FROM VW_ZZCRO_UNIDADE_ADMINSTRATIVA S inner join HADES..VW_SETOR se on S.SETOR = se.SETOR left join LY_UNIDADE_ENSINO ue on S.SETOR = ue.SETOR "
                        SqlOrder="setor" ColumnName="setor" Caption="" MaxLength="6" DataType="Varchar"
                        OnChanged="tseUnidadeAdministrativa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />                            
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="UNIDADE_ENS" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBlocoContrato" Text="Contrato" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsUnidade" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnlContrato" runat="server" Visible="false">
        <dxtc:ASPxTabControl ID="tab" runat="server" Width="940px" ActiveTabIndex="0" OnTabClick="tab_TabClick"
            AutoPostBack="True" SyncSelectionMode="None" Visible="false">
            <TabStyle Wrap="True" Width="105px">
            </TabStyle>
            <Tabs>
                <dxtc:Tab Text="Dados Gerais" Name="tabDadosGerais">
                </dxtc:Tab>
                <dxtc:Tab Text="Circuito/Link" Name="tabCircuito">
                </dxtc:Tab>
            </Tabs>
        </dxtc:ASPxTabControl>
        <asp:ObjectDataSource ID="odsContrato" runat="server" TypeName="Techne.Lyceum.Net.Interconectividade.Contrato"
            SelectMethod="Lista" UpdateMethod="Update" DeleteMethod="Delete" InsertMethod="Insert">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUnidadeAdministrativa" Name="setor" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Panel ID="pntabDadosGerais" runat="server" Visible="false">
            <asp:Panel ID="pnlDadosContrato" runat="server" Visible="false">
                <br />
                <br />
                <table>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label9" runat="server" Text="Tipo Link:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ddlTipoLink" runat="server" TextField="DESCRICAO" ValueField="TIPOLINKID"
                                ClientInstanceName="ddlTipoLink" Width="100px">
                            </dxe:ASPxComboBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label1" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumero" runat="server" MaxLength="50" Width="220px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label2" runat="server" Text="Descrição:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDescricao" runat="server" MaxLength="500" TextMode="MultiLine"
                                Width="520px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label4" runat="server" Text="Data Contratação:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="dtContratacao" runat="server" MinDate="1901-01-01" Width="120px"
                                EnableDefaultAppearance="true" ClientInstanceName="dtContratacao" CalendarProperties-ClearButtonText="Limpar"
                                CalendarProperties-TodayButtonText="Hoje" ClientEnabled="True">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label3" runat="server" Text="Data Instalação:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="dtImplantacao" runat="server" MinDate="1901-01-01" Width="120px"
                                EnableDefaultAppearance="true" ClientInstanceName="dtImplantacao" CalendarProperties-ClearButtonText="Limpar"
                                CalendarProperties-TodayButtonText="Hoje" ClientEnabled="True">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label5" runat="server" Text="Data Término:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxDateEdit ID="dtTermino" runat="server" MinDate="1901-01-01" Width="120px"
                                EnableDefaultAppearance="true" ClientInstanceName="dtTermino" CalendarProperties-ClearButtonText="Limpar"
                                CalendarProperties-TodayButtonText="Hoje" ClientEnabled="True">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label6" runat="server" Text="Operadora:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ddlOperadora" runat="server" TextField="DESCRICAO" ValueField="OPERADORAID"
                                ClientInstanceName="ddlOperadora" Width="100px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table align="LEFT">
                    <tr>
                        <td>
                            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <dxwgv:ASPxGridView ID="grdContrato" runat="server" DataSourceID="odsContrato" KeyFieldName="compositekey" OnCellEditorInitialize="grdContrato_CellEditorInitialize"
                AutoGenerateColumns="false" ClientInstanceName="grdContrato" OnInitNewRow="grdContrato_InitNewRow"
                OnRowUpdating="grdContrato_RowUpdating" OnAfterPerformCallback="grdContrato_AfterPerformCallback"
                OnStartRowEditing="grdContrato_StartRowEditing" OnCustomUnboundColumnData="grdContrato_CustomUnboundColumnData"
                Width="1150px" OnCustomButtonCallback="grdContrato_CustomButtonCallback">
                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsEditing Mode="Inline" />
                <SettingsBehavior ConfirmDelete="False" />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                        <%--<CustomButtons>
                            <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditar" Visibility="AllDataRows"
                                Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>--%>
                        <CancelButton Visible="true" Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                        <EditButton Visible="True" Text="Editar">
                            <Image Url="../img/bt_editar.png" />
                        </EditButton>
                        <UpdateButton Visible="true" Text="Alterar">
                            <Image Url="../img/bt_salvar.png" />
                        </UpdateButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn Caption="compositekey" FieldName="compositekey" UnboundType="String"
                        Visible="False" VisibleIndex="5">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CONTRATOID" Name="CONTRATOID" VisibleIndex="1"
                        FieldName="CONTRATOID" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CONTRATOSETORID" Name="CONTRATOSETORID" VisibleIndex="1"
                        FieldName="CONTRATOSETORID" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CONTRATOOPERADORAID" Name="CONTRATOOPERADORAID"
                        VisibleIndex="2" FieldName="CONTRATOOPERADORAID" Visible="false">
                        <PropertiesTextEdit MaxLength="100">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Link*" FieldName="TIPOLINKID" VisibleIndex="1"
                        Width="150px" HeaderStyle-Font-Bold="true">
                        <PropertiesComboBox DataSourceID="odsTipoLink" MaxLength="20" TextField="DESCRICAO"
                            ValueField="TIPOLINKID" ValueType="System.String" Width="150px" EnableIncrementalFiltering="true">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Número*" Name="numero" VisibleIndex="2" FieldName="NUMERO"
                        Width="150px">
                        <PropertiesTextEdit MaxLength="100">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="DESCRICAO" VisibleIndex="2"
                        FieldName="DESCRICAO" Width="250px">
                        <PropertiesTextEdit MaxLength="100">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn Caption="Data Contratação*" FieldName="DATACONTRATACAO"
                        VisibleIndex="3" Width="130px">
                        <EditItemTemplate>
                            <table style="width: 110px">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtContratacao" runat="server" Width="90px" Enabled="true" Value='<%# Bind("DATACONTRATACAO") %>'
                                            EnableDefaultAppearance="true" ClientInstanceName="dtContratacao" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataDateColumn Caption="Data Instalação*" FieldName="DATAIMPLANTACAO"
                        VisibleIndex="4" Width="130px">
                        <EditItemTemplate>
                            <table style="width: 110px">
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtImplantacao" runat="server" Width="90px" Enabled="true" Value='<%# Bind("DATAIMPLANTACAO") %>'
                                            EnableDefaultAppearance="true" ClientInstanceName="dtImplantacao" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataDateColumn Caption="Data Término" FieldName="DATATERMINO" VisibleIndex="5"
                        Width="110px">
                        <EditItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtTermino" runat="server" Width="90px" Enabled="true" Value='<%# Bind("DATATERMINO") %>'
                                            EnableDefaultAppearance="true" ClientInstanceName="dtTermino" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Operadora*" FieldName="OPERADORAID" VisibleIndex="6"
                        Width="150px" HeaderStyle-Font-Bold="true">
                        <PropertiesComboBox DataSourceID="odsOperadora" MaxLength="20" TextField="DESCRICAO"
                            ValueField="OPERADORAID" ValueType="System.String" Width="150px" EnableIncrementalFiltering="true">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CNPJ Operadora" Name="CNPJOPERADORA" VisibleIndex="7"
                        FieldName="CNPJOPERADORA" Width="200px">                       
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="odsOperadora" runat="server" SelectMethod="ListaOperadoraAtiva"
                TypeName="Techne.Lyceum.RN.FiscalizacaoLink.Operadora"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsTipoLink" runat="server" SelectMethod="ListaAtivo" TypeName="Techne.Lyceum.RN.FiscalizacaoLink.TipoLink">
            </asp:ObjectDataSource>
            <asp:HiddenField ID="hdnContratoSetorId" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pntabCircuito" runat="server" Visible="false">
        </asp:Panel>
    </asp:Panel>
</asp:Content>
