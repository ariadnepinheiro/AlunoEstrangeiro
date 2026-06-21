<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TiposDependencia.aspx.cs"
    Inherits="Techne.Lyceum.Net.Basico.TiposDependencia" MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<asp:Content ID="conTiposDependencia" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsTiposDependencia" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_tipo_dependencia">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView ID="grdTiposDependencia" runat="server" 
        AutoGenerateColumns="False" ClientInstanceName="grdTiposDependencia"
        DataSourceID="tdsTiposDependencia" KeyFieldName="tipo_depend" OnCellEditorInitialize="grdTiposDependencia_CellEditorInitialize"
        Font-Names="Verdana" Font-Size="Small" OnInitNewRow="grdTiposDependencia_InitNewRow"
        OnStartRowEditing="grdTiposDependencia_StartRowEditing" OnAfterPerformCallback="grdTiposDependencia_AfterPerformCallback" Width="600px">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdTiposDependencia.AddNewRow();"
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
            <dxwgv:GridViewDataTextColumn Caption="Tipo*" HeaderStyle-Font-Bold="true" FieldName="tipo_depend" VisibleIndex="1" Width="150px">
                <PropertiesTextEdit MaxLength="20" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Tipo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome*" HeaderStyle-Font-Bold="true" FieldName="nome" VisibleIndex="2" Width="400px">
                <PropertiesTextEdit MaxLength="50" Width="400px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Nome." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
         <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
