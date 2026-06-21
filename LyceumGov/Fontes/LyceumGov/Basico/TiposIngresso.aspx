<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="TiposIngresso.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.TiposIngresso" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsTiposIngresso" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_tipo_ingresso">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView ID="grdTiposIngresso" runat="server" 
        AutoGenerateColumns="False" ClientInstanceName="grdTiposIngresso"
        DataSourceID="tdsTiposIngresso" KeyFieldName="tipo_ingresso" OnCellEditorInitialize="grdTiposIngresso_CellEditorInitialize"
        OnRowDeleting="grdTiposIngresso_RowDeleting" OnRowUpdating="grdTiposIngresso_RowUpdating"
        Font-Names="Verdana" Font-Size="Small" OnInitNewRow="grdTiposIngresso_InitNewRow"
        OnStartRowEditing="grdTiposIngresso_StartRowEditing" 
        onrowinserting="grdTiposIngresso_RowInserting"
		OnAfterPerformCallback="grdTiposIngresso_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
        <SettingsEditing Mode="Inline"></SettingsEditing>
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados.">
        </SettingsText>
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdTiposIngresso.AddNewRow();"
                            alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
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
            <dxwgv:GridViewDataTextColumn VisibleIndex="1" Caption="Tipo de Ingresso*" HeaderStyle-Font-Bold="true" FieldName="tipo_ingresso"
                ReadOnly="false" Width="200px">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="True" ErrorText="Favor informar o Tipo de Ingresso">
                        </RequiredField>
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                ReadOnly="false" Width="720px">
                <PropertiesTextEdit MaxLength="100" Width="720px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="True" ErrorText="Favor informar a Descrição"></RequiredField>
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
         <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
