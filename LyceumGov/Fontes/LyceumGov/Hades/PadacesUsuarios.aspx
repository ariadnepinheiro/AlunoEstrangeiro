<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadacesUsuarios.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.PadacesUsuarios" %>

<asp:Content ID="conpadacesUsuarios" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblPadaceselecionado" runat="server" Text="Padrão de Acesso:"></asp:Label>
    <asp:Label ID="lblPadaces" runat="server"></asp:Label>
    <asp:Label ID="lblInvisible" runat="server" Visible="false"></asp:Label>
    <techne:TTableDataSource ID="tdsPadUsuario" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padusuario"
        SqlWhere="hd_padusuario.padaces = @padaces">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="lblPadaces" Name="padaces" PropertyName="Text" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsUsuario" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_usuario"
        SqlOrder="nome">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView runat="server" ID="grdPadUsuario" DataSourceID="tdsPadUsuario"
        ClientInstanceName="grdTipoDisciplina" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
        OnCellEditorInitialize="grdPadUsuario_CellEditorInitialize" OnCustomUnboundColumnData="grdPadUsuario_CustomUnboundColumnData"
        OnRowDeleting="grdPadUsuario_RowDeleting" OnRowInserting="grdPadUsuario_RowInserting"
        OnRowValidating="grdPadUsuario_RowValidating" OnInitNewRow="grdPadUsuario_InitNewRow"
        OnRowUpdating="grdPadUsuario_RowUpdating" Width="411px" OnAfterPerformCallback="grdPadUsuario_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="conUsuarios" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    Usuário:
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseUsuario" runat="server" SqlSelect="SELECT DISTINCT usuario, nomeusuario FROM usuario"
                                        SqlOrder="nomeusuario" Argument="nomeusuario" Caption="" MaxLength="15" Value='<%# Bind("usuario") %>'
                                        AutoPostBack="false">
                                        <gridcolumns>
                                            <tweb:TSearchBoxColumn Caption="Usuário" FieldName="usuario" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomeusuario" Width="70%" />
                                        </gridcolumns>
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
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdTipoDisciplina.AddNewRow();" alt="Novo" />
                    </div>
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
            <dxwgv:GridViewDataTextColumn FieldName="padaces" ReadOnly="True" VisibleIndex="1"
                Caption="Padrão de Acesso*" HeaderStyle-Font-Bold="true" Visible="True" Width="100px">
                <PropertiesTextEdit MaxLength="14" Width="100px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Padrão de Acesso." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Usuário*" HeaderStyle-Font-Bold="true" FieldName="usuario"
                VisibleIndex="2" Visible="false" Width="150px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" HeaderStyle-Font-Bold="true" FieldName="nome02"
                VisibleIndex="3" Visible="true" Width="400px" ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataColumn FieldName="CompositeKey" VisibleIndex="4" UnboundType="String"
                Visible="False">
            </dxwgv:GridViewDataColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <br />
    <br />
    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
</asp:Content>
