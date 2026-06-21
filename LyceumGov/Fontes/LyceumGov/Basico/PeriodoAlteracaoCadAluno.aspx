<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PeriodoAlteracaoCadAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.PeriodoAlteracaoCadAluno" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsPeriodoAlteracao" runat="server" TypeName="Techne.Lyceum.Net.Basico.PeriodoAlteracaoCadAluno"
        SelectMethod="Lista" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAno" runat="server" SelectMethod="ListaAnoPeriodoAlteracaoAluno"
        TypeName="Techne.Lyceum.RN.PeriodoLetivo"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPeriodo" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdPeriodo"
        DataSourceID="odsPeriodoAlteracao" KeyFieldName="PERIODOALTERACAOALUNOID" OnRowDeleting="grdPeriodo_RowDeleting"
        OnRowUpdating="grdPeriodo_RowUpdating" Font-Names="Verdana" Font-Size="Small"
        OnCellEditorInitialize="grdPeriodo_CellEditorInitialize" OnInitNewRow="grdPeriodo_InitNewRow"
        OnStartRowEditing="grdPeriodo_StartRowEditing" OnRowInserting="grdPeriodo_RowInserting"
        OnAfterPerformCallback="grdPeriodo_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPeriodo.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="PERIODOALTERACAOALUNOID" VisibleIndex="1"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano" FieldName="ANO" VisibleIndex="1"
                Width="80px">
                <PropertiesComboBox ValueType="System.String" DataSourceID="odsAno" TextField="ANO"
                    ClientInstanceName="cmbANO" ValueField="ANO" Width="80px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar uma ano." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Início"
                HeaderStyle-Font-Bold="true" FieldName="DATAINICIO" VisibleIndex="2" Width="200px">
                <PropertiesDateEdit Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o inicio do período." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>                
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Fim"
                HeaderStyle-Font-Bold="true" FieldName="DATAFIM" VisibleIndex="3" Width="200px">
                <PropertiesDateEdit Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o fim do período." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Responsável" Name="RESPONSAVELID" VisibleIndex="4"
                FieldName="USUARIOID" Width="700px">
                <PropertiesTextEdit MaxLength="150">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
