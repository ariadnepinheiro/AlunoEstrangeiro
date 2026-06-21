<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="NaturezaDespesa.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.NaturezaDespesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsNaturezaDespesa" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.NaturezaDespesa"
        SelectMethod="Lista" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdNaturezaDespesa" runat="server" DataSourceID="odsNaturezaDespesa"
        KeyFieldName="NATUREZADESPESAID" AutoGenerateColumns="false" ClientInstanceName="grdNaturezaDespesa"
        OnInitNewRow="grdNaturezaDespesa_InitNewRow" OnStartRowEditing="grdNaturezaDespesa_StartRowEditing"
        OnRowUpdating="grdNaturezaDespesa_RowUpdating" OnRowDeleting="grdNaturezaDespesa_RowDeleting" 
        OnAfterPerformCallback="grdNaturezaDespesa_AfterPerformCallback" Width="500px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="NATUREZADESPESAID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
                FieldName="DESCRICAO" Width="400px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código Sefaz*" Name="CODIGOSEFAZ" VisibleIndex="2"
                FieldName="CODIGOSEFAZ" Width="400px" ReadOnly="true">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="3"
                Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
