<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="UnidadeEnsinoImpedida.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.UnidadeEnsinoImpedida" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlTipoFiltro" GroupingText="Informe os dados para pesquisar as unidades"
        Width="800px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblFiltro" runat="server" Text="Filtros:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblTipoFiltro" runat="server" RepeatDirection="Horizontal"
                        Width="254px" AutoPostBack="true" OnSelectedIndexChanged="rblTipoFiltro_SelectedIndexChanged">
                        <asp:ListItem Text="Todas" Value="T" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Por Unidade de Ensino" Value="U"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlFiltros" Width="800px" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblUnidade" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO "
                            GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <br />
    <br />
    <div class="divEditBlock" style="width: 800px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBlocoUnidade" Text="Unidades de Ensino Impedidas"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsUnidade" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel runat="server" ID="pnlNovoImpedimento" GroupingText="Informe os dados do impedimento"
        Width="800px" Visible="false">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label1" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseUnidadeImpedida" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO "
                        GridWidth="850px" OnChanged="tseUnidadeImpedida_Changed" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Motivo Impedimento:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlMotivoImpedimento" runat="server" DataTextField="DESCRICAO"
                        DataValueField="MOTIVOIMPEDIMENTOID">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label5" runat="server" Text="Data Início:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label6" runat="server" Text="Data Fim:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataFim" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtDataFim" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel runat="server" ID="pnlGrid" Width="800px">
        <asp:ObjectDataSource ID="odsUnidadeEnsinoImpedida" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.UnidadeEnsinoImpedida"
            SelectMethod="Lista" UpdateMethod="Update" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade"
                    PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdUnidadeEnsinoImpedida" runat="server" DataSourceID="odsUnidadeEnsinoImpedida"
            KeyFieldName="UNIDADEENSINOIMPEDIDAID" AutoGenerateColumns="false" ClientInstanceName="grdUnidadeEnsinoImpedida"
            OnStartRowEditing="grdUnidadeEnsinoImpedida_StartRowEditing" OnRowUpdating="grdUnidadeEnsinoImpedida_RowUpdating"
            OnRowDeleting="grdUnidadeEnsinoImpedida_RowDeleting" OnAfterPerformCallback="grdUnidadeEnsinoImpedida_AfterPerformCallback"
            Width="100%" OnCellEditorInitialize="grdUnidadeEnsinoImpedida_CellEditorInitialize">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="UNIDADEENSINOIMPEDIDAID"
                    Visible="false" Width="700px">
                    <PropertiesTextEdit MaxLength="200">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Regional*" Name="REGIONAL" VisibleIndex="2" FieldName="REGIONAL"
                    Width="200px" ReadOnly="true" >
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Escola*" Name="ESCOLA" VisibleIndex="4" FieldName="ESCOLA"
                    Width="300px" ReadOnly="true">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Censo*" Name="CENSO" VisibleIndex="3" FieldName="CENSO"
                    Width="150px" ReadOnly="true">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataTextColumn Caption="MOTIVOIMPEDIMENTOID" Name="MOTIVOIMPEDIMENTOID"
                    VisibleIndex="1" FieldName="MOTIVOIMPEDIMENTOID" Visible="false" Width="700px">
                    <PropertiesTextEdit MaxLength="200">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Motivo*" Name="MOTIVO" VisibleIndex="5" FieldName="MOTIVO"
                    Width="300px" ReadOnly="true">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Ínicio*" FieldName="DATAINICIO" VisibleIndex="6"
                    Width="150px">
                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="7"
                    Width="150px">
                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </PropertiesDateEdit>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
