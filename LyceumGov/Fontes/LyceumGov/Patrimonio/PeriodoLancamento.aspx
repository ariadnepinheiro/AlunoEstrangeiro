<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PeriodoLancamento.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.PeriodoLancamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsPeriodoLancamento" runat="server" TypeName="Techne.Lyceum.Net.Patrimonio.PeriodoLancamento"
        SelectMethod="Lista" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAno" runat="server" SelectMethod="ListaAnoPatrimonio"
        TypeName="Techne.Lyceum.RN.PeriodoLetivo"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPeriodoLancamento" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdPeriodoLancamento" DataSourceID="odsPeriodoLancamento"
        KeyFieldName="PERIODOLANCAMENTOID" OnRowDeleting="grdPeriodoLancamento_RowDeleting"
        OnRowUpdating="grdPeriodoLancamento_RowUpdating" Font-Names="Verdana" Font-Size="Small"
        OnCellEditorInitialize="grdPeriodoLancamento_CellEditorInitialize" OnInitNewRow="grdPeriodoLancamento_InitNewRow"
        OnStartRowEditing="grdPeriodoLancamento_StartRowEditing" OnRowInserting="grdPeriodoLancamento_RowInserting"
        OnAfterPerformCallback="grdPeriodoLancamento_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPeriodoLancamento.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="PERIODOLANCAMENTOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano*" FieldName="ANO" VisibleIndex="3"
                Width="200px">
                <PropertiesComboBox ValueType="System.String" DataSourceID="odsAno" TextField="ANO"
                    ClientInstanceName="cmbANO" ValueField="ANO" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar uma situação." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Início Período*" HeaderStyle-Font-Bold="true"
                FieldName="DATAINICIO" VisibleIndex="5" Width="200px">
                <PropertiesDateEdit Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o início do período." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Término Período*" HeaderStyle-Font-Bold="true"
                FieldName="DATAFIM" VisibleIndex="6" Width="200px">
                <PropertiesDateEdit Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o término do período." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
