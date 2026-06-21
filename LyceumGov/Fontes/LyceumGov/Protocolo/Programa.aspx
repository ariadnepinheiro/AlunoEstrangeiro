<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Programa.aspx.cs" Inherits="Techne.Lyceum.Net.Protocolo.Programa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsPrograma" runat="server" TypeName="Techne.Lyceum.Net.Protocolo.Programa"
        SelectMethod="ListaPrograma" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTipoPrograma" runat="server" TypeName="Techne.Lyceum.Net.Protocolo.Programa"
        SelectMethod="ListaTipoPrograma"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPrograma" runat="server" DataSourceID="odsPrograma" KeyFieldName="PROGRAMAPROTOCOLOID"
        AutoGenerateColumns="false" ClientInstanceName="grdPrograma" OnInitNewRow="grdPrograma_InitNewRow"
        OnStartRowEditing="grdPrograma_StartRowEditing" OnRowInserting="grdPrograma_RowInserting"
        OnCellEditorInitialize="grdPrograma_CellEditorInitialize" OnRowUpdating="grdPrograma_RowUpdating"
        OnRowDeleting="grdPrograma_RowDeleting" OnAfterPerformCallback="grdPrograma_AfterPerformCallback"
        Width="500px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdPrograma.AddNewRow();" />
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
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="1" Caption="Tipo*" Name="ddlTipoProtocolo"
                FieldName="TIPOPROTOCOLOID" Width="200" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsTipoPrograma" TextField="DESCRICAO" ValueField="TIPOPROTOCOLOID"
                    ValueType="System.String">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="2" FieldName="PROGRAMAPROTOCOLOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="3"
                FieldName="DESCRICAO" Width="400px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="4"
                Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
