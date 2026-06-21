<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DocumentosNecessariosFornecedor.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.DocumentosNecessariosFornecedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsDocumentosNecessariosFornecedor" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.DocumentosNecessariosFornecedor"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdDocumentosNecessariosFornecedor" runat="server" DataSourceID="odsDocumentosNecessariosFornecedor"
        KeyFieldName="DOCUMENTOSNECESSARIOSFORNECEDORID" AutoGenerateColumns="false"
        ClientInstanceName="grdDocumentosNecessariosFornecedor" OnInitNewRow="grdDocumentosNecessariosFornecedor_InitNewRow"
        OnStartRowEditing="grdDocumentosNecessariosFornecedor_StartRowEditing" OnRowInserting="grdDocumentosNecessariosFornecedor_RowInserting"
        OnRowUpdating="grdDocumentosNecessariosFornecedor_RowUpdating" OnRowDeleting="grdDocumentosNecessariosFornecedor_RowDeleting"
        OnAfterPerformCallback="grdDocumentosNecessariosFornecedor_AfterPerformCallback"
        Width="80%">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdDocumentosNecessariosFornecedor.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="DOCUMENTOSNECESSARIOSFORNECEDORID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
                FieldName="DESCRICAO" Width="400px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Periodicidade*" FieldName="PERIODICIDADE"
                VisibleIndex="3" Width="250">
                <PropertiesComboBox>
                    <Items>
                        <dxe:ListEditItem Text="Sem Periodicidade" Value="0" />
                        <dxe:ListEditItem Text="Mensal" Value="1" />
                        <dxe:ListEditItem Text="Bimestral" Value="2" />
                        <dxe:ListEditItem Text="Trimestral" Value="3" />
                        <dxe:ListEditItem Text="Semestral" Value="6" />
                        <dxe:ListEditItem Text="Anual" Value="12" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Fornecedor*" FieldName="TIPO"
                VisibleIndex="4" Width="250">
                <PropertiesComboBox>
                    <Items>
                        <dxe:ListEditItem Text="Pessoa Física" Value="Pessoa Física" />
                        <dxe:ListEditItem Text="Pessoa Jurídica" Value="Pessoa Jurídica" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Ínicio*" FieldName="DATAINICIO" VisibleIndex="5"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="6"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="7"
                Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
