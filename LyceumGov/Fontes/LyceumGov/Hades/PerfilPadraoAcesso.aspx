<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PerfilPadraoAcesso.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.PerfilPadraoAcesso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxwgv:ASPxGridView ID="grdPerfil" runat="server" DataSourceID="odsPerfil" ClientInstanceName="grdPadaces"
        KeyFieldName="ID_PERFIL" OnCellEditorInitialize="grdPerfil_CellEditorInitialize"
        EnableCallBacks="true" AutoGenerateColumns="False" Width="550px" OnInitNewRow="grdPerfil_InitNewRow"
        OnStartRowEditing="grdPerfil_StartRowEditing" OnAfterPerformCallback="grdPerfil_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" AllowFocusedRow="true"
            ProcessFocusedRowChangedOnServer="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPadaces.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
               <%-- <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>--%>
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
            <dxwgv:GridViewDataTextColumn FieldName="ID_PERFIL" ReadOnly="true" VisibleIndex="1"
                Caption="Perfil de Acesso*" HeaderStyle-Font-Bold="true" Width="50px">
                <PropertiesTextEdit>
                    <ReadOnlyStyle>
                        <Border BorderStyle="None"></Border>
                    </ReadOnlyStyle>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" ReadOnly="False" VisibleIndex="2"
                Caption="Nome*" HeaderStyle-Font-Bold="true" Width="300px">
                <PropertiesTextEdit MaxLength="500" Width="300">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Nome." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsPerfil" TypeName="Techne.Lyceum.Net.Hades.PerfilPadraoAcesso"
        runat="server" SelectMethod="Listar" OnDeleting="odsPerfil_Deleting" DeleteMethod="Delete"
        OnUpdating="odsPerfil_Updating" UpdateMethod="Update" OnInserting="odsPerfil_Inserting"
        InsertMethod="Insert"></asp:ObjectDataSource>
    <br />
    <br />
</asp:Content>
