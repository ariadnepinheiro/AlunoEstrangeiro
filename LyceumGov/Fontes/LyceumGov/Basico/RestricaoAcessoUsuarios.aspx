<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RestricaoAcessoUsuarios.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.RestricaoAcessoUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphFormulario" runat="server">
    <div>
        <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por usuário"
            Width="600px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblUsuarioTSearch" runat="server" Text="Usuário:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUsuario" runat="server" SqlSelect="SELECT DISTINCT usuario, nomeusuario FROM usuario"
                            SqlOrder="usuario" Argument="nomeusuario" Caption="" MaxLength="15" OnChanged="tseUsuario_Changed">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Usuário" FieldName="usuario" Width="30%" />
                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomeusuario" Width="70%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <techne:TTableDataSource ID="tdsUsuarioUnidade" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_usuario_unidade_fis"
        SqlWhere="usuario = @usuario">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="tseUsuario" DefaultValue="" Name="usuario" PropertyName="DBValue" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsUnidadeFisica" runat="server" DataTableClassName="Techne.Lyceum.CR.Vw_zzcro_unidade_fisica">
    </techne:TTableDataSource>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Button ID="btnAdicionarTodasUnidades" Text="Incluir todas Unidades" Width="160px"
        runat="server" OnClick="btnAdicionarTodasUnidades_Click" Visible="false" OnClientClick="javascript: return confirm('Todas as Unidades Físicas cadastradas serão associadas ao usuário.\nClique em OK para continuar.');" />
    <asp:Button ID="btnRemoverTodasUnidades" Text="Remover todas Unidades" Width="160px"
        runat="server" OnClick="btnRemoverTodasUnidades_Click" Visible="false" OnClientClick="javascript: return confirm('Todas as associações de Unidades Físicas relacionadas ao usuário serão removidas.\nClique em OK para continuar.');" />
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdUsuarioUnidade" runat="server" AutoGenerateColumns="False"
        Width="500px" ClientInstanceName="grdUsuarioUnidade" DataSourceID="tdsUsuarioUnidade"
        KeyFieldName="CompositeKey" OnCustomUnboundColumnData="grdUsuarioUnidade_CustomUnboundColumnData"
        OnRowDeleting="grdUsuarioUnidade_RowDeleting" OnCellEditorInitialize="grdUsuarioUnidade_CellEditorInitialize"
        OnRowInserting="grdUsuarioUnidade_RowInserting" OnRowUpdating="grdUsuarioUnidade_RowUpdating"
        OnInitNewRow="grdUsuarioUnidade_InitNewRow" OnRowValidating="grdUsuarioUnidade_RowValidating"
        OnAfterPerformCallback="grdUsuarioUnidade_AfterPerformCallback" Visible="false">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="ContentControl1" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblUnidadeFisica" runat="server" Text="Unidade Física:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseUnidadeFisica" runat="server" Argument="nome_comp" ArgumentColumns="30"
                                        FollowContainerMode="false" MaxLength="20" AutoPostBack="false" Columns="10"
                                        DataType="VarChar" Key="unidade_fis" Value='<%# Bind("unidade_fis") %>' SqlOrder="nome_comp"
                                        SqlSelect="SELECT unidade_fis, nome_comp FROM Ly_Unidade_Fisica">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_fis" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome_comp" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                        </table>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                </dxw:ContentControl>
                </div>
            </EditForm>
        </Templates>
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                        onclick="grdUsuarioUnidade.AddNewRow();" alt="Novo" />
                </HeaderCaptionTemplate>
                <EditButton Text="Editar">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton>
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="usuario" ReadOnly="True"
                VisibleIndex="1" Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="unidade_fis" FieldName="unidade_fis" VisibleIndex="1"
                Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade Física" FieldName="nome_comp03" VisibleIndex="2">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a unidade física." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                Visible="False" VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:Panel ID="pnRegional" runat="server" GroupingText="Selecione a Regional para inserir unidades físicas por Regional"
        Width="600px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblRegional" runat="server" Text="Regional:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" 
                        SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnInserirRegional" Text="Incluir" Width="160px" runat="server" OnClick="btnInserirRegional_Click"
            Visible="true" OnClientClick="javascript: return confirm('Todas as Unidades Físicas relacionadas a Regional selecionada serão associadas ao usuário.\nClique em OK para continuar.');" />
        <asp:Button ID="btnRemoverRegional" Text="Remover" Width="160px" runat="server" OnClick="btnRemoverRegional_Click"
            Visible="true" OnClientClick="javascript: return confirm('Todas as associações de Unidades Físicas do usuário relacionadas a Regional selecionada serão removidas.\nClique em OK para continuar.');" />
    </asp:Panel>
</asp:Content>
