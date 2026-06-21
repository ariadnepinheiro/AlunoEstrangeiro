<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadacesPerfil.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.PadacesPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnPadraoAcesso" runat="server" GroupingText="Informe o padrão de acesso"
        Width="640px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPadaces" runat="server" Text="Padrão de Acesso:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePadrao" runat="server" Caption="" SqlSelect="SELECT padaces, nomepadaces from padaces"
                        ArgumentColumns="60" Columns="20" MaxLength="14" GridWidth="600px" SqlOrder="nomepadaces">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="padaces" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nomepadaces" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPerfil" runat="server" Text="Perfil de Acesso:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePerfil" runat="server" Caption="" SqlSelect="SELECT id_perfil,descricao from TCE_PERFIL"
                        Key="id_perfil" DataType="Number" ArgumentColumns="60" Columns="20" MaxLength="14"
                        GridWidth="600px" SqlOrder="id_perfil" Connection="Hades">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_perfil" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        OnClick="btnSalvar_Click" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnGrid" runat="server">
            <dxwgv:ASPxGridView ID="grdPadraoPerfil" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdPadraoPerfil" KeyFieldName="ID_PADACES_PERFIL" DataSourceID="odsPadraoPerfil"
                OnStartRowEditing="grdPadraoPerfil_StartRowEditing" OnAfterPerformCallback="grdPadraoPerfil_AfterPerformCallback">
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                <SettingsEditing Mode="EditForm" />
                <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                        <CancelButton Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="id_padaces_perfil" ReadOnly="False" VisibleIndex="1"
                        Caption="id_padaces_perfil" HeaderStyle-Font-Bold="true" Width="150px" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="ID_Perfil" VisibleIndex="2" Visible="false"
                        Caption="Perfil*" HeaderStyle-Font-Bold="true" Width="380px">
                        <PropertiesTextEdit MaxLength="50" Width="380px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="NOME" ReadOnly="False" VisibleIndex="3"
                        Caption="Padrão de Acesso*" HeaderStyle-Font-Bold="true" Width="380px">
                        <PropertiesTextEdit MaxLength="50" Width="380px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor informar o Padrão de Acesso." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" ReadOnly="False" VisibleIndex="3"
                        Caption="Perfil*" HeaderStyle-Font-Bold="true" Width="380px">
                        <PropertiesTextEdit MaxLength="50" Width="380px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor informar o Nome." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Templates>
                    <EditForm>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <div style="padding: 4px 4px 3px 4px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPerfil" runat="server" Text="Função:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <tweb:TSearchBox ID="tsePerfil" runat="server" Argument="descricao" ArgumentColumns="70"
                                                FollowContainerMode="false" MaxLength="20" AutoPostBack="false" Columns="10"
                                                DataType="VarChar" Key="id_perfil" Value='<%# Bind("id_perfil") %>' SqlOrder="descricao"
                                                SqlSelect="SELECT id_perfil, descricao FROM TCE_PERFIL">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="id_perfil" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
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
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="odsPadraoPerfil" TypeName="Techne.Lyceum.Net.Hades.PadacesPerfil"
                runat="server" SelectMethod="Listar" OnDeleting="odsPadraoPerfil_Deleting" DeleteMethod="Delete">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tsePadrao" Name="padaces" PropertyName="DBValue" />
                    <asp:ControlParameter ControlID="tsePerfil" Name="id_perfil" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
</asp:Content>
