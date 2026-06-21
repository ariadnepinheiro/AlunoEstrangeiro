<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Responsaveis.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Responsaveis"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<asp:Content ID="conPapelPessoa" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsPapelPessoa" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_papel_pessoa">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView ID="grdPapelPessoa" runat="server" 
        AutoGenerateColumns="False" ClientInstanceName="grdPapelPessoa"
        DataSourceID="tdsPapelPessoa" KeyFieldName="papel" OnCellEditorInitialize="grdPapelPessoa_CellEditorInitialize"
        OnInitNewRow="grdPapelPessoa_InitNewRow" 
        OnStartRowEditing="grdPapelPessoa_StartRowEditing" 
        onrowinserting="grdPapelPessoa_RowInserting"
		OnAfterPerformCallback="grdPapelPessoa_AfterPerformCallback" 
        onrowdeleting="grdPapelPessoa_RowDeleting">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdPapelPessoa.AddNewRow();"
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
            <dxwgv:GridViewDataTextColumn Caption="Responsável*" HeaderStyle-Font-Bold="true" FieldName="papel" VisibleIndex="1" Width="200px">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Responsável." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao" Width="720px"
                VisibleIndex="2">
                <PropertiesTextEdit MaxLength="100" Width="720px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
         <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
