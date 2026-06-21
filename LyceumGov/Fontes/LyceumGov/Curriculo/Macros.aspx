<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Macros.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.Macros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnAbas" runat="server" Width="800px">
        <dxtc:ASPxPageControl ID="pcTermo" runat="server" ActiveTabIndex="0" Width="800px" OnTabClick="pcTermo_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Macrocampos">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão:"
                                Width="800px">
                                <table>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label ID="lblNome" runat="server" Text="Nome:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNome" runat="server" Width="268px" MaxLength="100"></asp:TextBox>
                                            
                                            <asp:RequiredFieldValidator ErrorMessage="Nome: Preenchimento obrigatório." 
                                            ID="rqrNome" runat="server" ControlToValidate="txtNome" InitialValue="" 
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblObrigatoria" Text="Obrigatorio:" SkinID="lblObrigatorio" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:CheckBox ID="chkObrigatoria" runat="server" />
                                        </td>
                                    </tr>                                   
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                                OnClick="btnSalvar_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnGrid" runat="server">
                                <dxwgv:ASPxGridView ID="grdMacro" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdMacro"
                                    KeyFieldName="ID_MACRO_CAMPOS" DataSourceID="odsMacro" OnCellEditorInitialize="grdMacro_CellEditorInitialize"
                                    OnStartRowEditing="grdMacro_StartRowEditing" OnRowValidating="grdMacro_RowValidating" onsh
                                    OnAfterPerformCallback="grdMacro_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                            <EditButton Text="Editar" Visible="True">
                                                <Image Url="~/img/bt_editar.png" />
                                            </EditButton>
                                            <DeleteButton Text="Remover" Visible="True">
                                                <Image Url="~/img/bt_exclui2.png" />
                                            </DeleteButton>
                                            <CancelButton Text="Cancelar">
                                                <Image Url="~/img/bt_cancelar.png" />
                                            </CancelButton>
                                            <UpdateButton Text="Salvar">
                                                <Image Url="~/img/bt_salvar.png" />
                                            </UpdateButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_MACRO_CAMPOS" Visible="false"
                                            VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataCheckColumn Caption="Obrigatória" FieldName="OBRIGATORIO" VisibleIndex="3"
                                            Width="110px">
                                            <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N">
                                            </PropertiesCheckEdit>
                                        </dxwgv:GridViewDataCheckColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="MATRICULA" VisibleIndex="4">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsEditing Mode="Inline" />
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="odsMacro" TypeName="Techne.Lyceum.Net.Curriculo.Macros"
        runat="server" SelectMethod="Listar" OnDeleting="odsMacro_Deleting" UpdateMethod="Update"
        DeleteMethod="Delete" OnUpdating="odsMacro_Updating"></asp:ObjectDataSource>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
</asp:Content>
