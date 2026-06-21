<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Avaliacao.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.Avaliacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsTipoAvaliacao" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Avaliacao"
        runat="server" SelectMethod="ListaTipoAvaliacao" />
    <asp:ObjectDataSource ID="odsAvaliacao" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Avaliacao"
        runat="server" SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update"
        DeleteMethod="Delete" />
    <dxwgv:ASPxGridView ID="grdAvaliacao" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdAvaliacao" DataSourceID="odsAvaliacao" KeyFieldName="AVALIACAOID"
        OnAfterPerformCallback="grdAvaliacao_AfterPerformCallback" OnStartRowEditing="grdAvaliacao_StartRowEditing"
        OnInitNewRow="grdAvaliacao_InitNewRow" OnRowInserting="grdAvaliacao_RowInserting"
        OnRowUpdating="grdAvaliacao_RowUpdating" OnRowDeleting="grdAvaliacao_RowDeleting"
        Width="775px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="100px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdAvaliacao.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="AVALIACAOID" VisibleIndex="1"
                Visible="false" />
            <dxwgv:GridViewDataTextColumn Caption="Descrição" VisibleIndex="2" FieldName="DESCRICAO"
                Width="425px" />
            <dxwgv:GridViewDataTextColumn Caption="Ano" VisibleIndex="3" FieldName="ANO" Width="100px">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Campo ANO é obrigatório." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="<a href='/AvaliacaoExterna/TipoAvaliacao.aspx' title='Cadastro de Objetivos da Avaliação'>Objetivo</a>"
                FieldName="TIPOAVALIACAOID" VisibleIndex="4" Width="150px">
                <PropertiesComboBox DataSourceID="odsTipoAvaliacao" TextField="DESCRICAO" ValueField="TIPOAVALIACAOID"
                    Width="150px" ValueType="System.String" DropDownWidth="150px">
                    <Items>
                        <dxe:ListEditItem Text="" Value="" />
                    </Items>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Campo OBJETIVO é obrigatório." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo*" FieldName="ATIVO" VisibleIndex="5"
                Width="100px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
