<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadroesdeAcesso.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.PadroesdeAcesso" %>

<asp:Content ID="conPadroesDeAcesso" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsPadaces" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padaces">
    </techne:TTableDataSource>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblSelecione" runat="server" Text="Selecione um padrão de acesso na tabela abaixo:"></asp:Label>
    <br />
    <dxwgv:ASPxGridView ID="grdPadaces" runat="server" DataSourceID="tdsPadaces" ClientInstanceName="grdPadaces"
        KeyFieldName="padaces" OnCellEditorInitialize="grdPadaces_CellEditorInitialize"
        EnableCallBacks="true" AutoGenerateColumns="False" Width="850px" OnInitNewRow="grdPadaces_InitNewRow"
        OnStartRowEditing="grdPadaces_StartRowEditing" 
        OnFocusedRowChanged="grdPadaces_FocusedRowChanged" 
        onafterperformcallback="grdPadaces_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" AllowFocusedRow="true"
            ProcessFocusedRowChangedOnServer="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdPadaces.AddNewRow();"
                            alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
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
            <dxwgv:GridViewDataTextColumn FieldName="padaces" ReadOnly="False" VisibleIndex="1"
                Caption="Padrão de Acesso*" HeaderStyle-Font-Bold="true" Width="150px">
                <PropertiesTextEdit MaxLength="14" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Padrão de Acesso." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="nome" ReadOnly="False" VisibleIndex="2"
                Caption="Nome*" HeaderStyle-Font-Bold="true" Width="380px">
                <PropertiesTextEdit MaxLength="50" Width="380px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Nome." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <br />
    <br />
    <%--    <asp:Label ID="lblItemSelected" runat="server"></asp:Label>--%>
        <asp:Label ID="lblSelecione2" runat="server" Text="Selecione uma opção:"></asp:Label>
    <br />
    <br />
    <asp:Button ID="btnTransacao" runat="server" Text="Transações" OnClick="btnTransacao_Click" />
    <asp:Button ID="btnUsuarios" runat="server" Text="Usuários" OnClick="btnUsuarios_Click" />
    <asp:Button ID="btnRelatorios" runat="server" Text="Relatórios" OnClick="btnRelatorios_Click" />
</asp:Content>
