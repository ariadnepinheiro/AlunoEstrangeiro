<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DocumentosRequeridos.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.DocumentosRequeridos" %>

<asp:Content ID="conDocumentosRequeridos" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsDocumentosRequeridos" runat="server" DataTableClassName="Techne.Lyceum.CR.LY_DOCUMENTOS_INGRESSO">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView ID="grdDocumentosRequeridos" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdDocumentosRequeridos" DataSourceID="tdsDocumentosRequeridos"
        KeyFieldName="doc" Font-Names="Verdana" Font-Size="Small" Width="600px" OnInitNewRow="grdDocumentosRequeridos_InitNewRow"
        OnStartRowEditing="grdDocumentosRequeridos_StartRowEditing" OnCellEditorInitialize="grdDocumentosRequeridos_CellEditorInitialize"
        OnAfterPerformCallback="grdDocumentosRequeridos_AfterPerformCallback" OnRowValidating="grdDocumentosRequeridos_RowValidating">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdDocumentosRequeridos.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="true">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="true">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton>
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="true">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código*" HeaderStyle-Font-Bold="true" FieldName="doc"
                VisibleIndex="1" Width="100">
                <PropertiesTextEdit MaxLength="20">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="nome"
                VisibleIndex="2" Width="300">
                <PropertiesTextEdit MaxLength="100">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a descrição do documento." IsRequired="true" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Bloqueia Pré-Matrícula" FieldName="bloqueia_pre_matr"
                VisibleIndex="3" Width="200px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueType="System.String" ValueUnchecked="N">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
