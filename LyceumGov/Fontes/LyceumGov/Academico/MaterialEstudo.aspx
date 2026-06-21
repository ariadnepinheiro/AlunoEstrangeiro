<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialEstudo.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MaterialEstudo" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="MaterialEstudo.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MaterialEstudo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

<asp:ObjectDataSource ID="odsMaterialEstudo" runat="server" TypeName="Techne.Lyceum.Net.Academico.MaterialEstudo"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update"
        DeleteMethod="Delete"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdMaterialEstudo" runat="server" DataSourceID="odsMaterialEstudo"
        KeyFieldName="MATERIALESTUDOID" AutoGenerateColumns="false" ClientInstanceName="grdMaterialEstudo"
        OnInitNewRow="grdMaterialEstudo_InitNewRow" OnStartRowEditing="grdMaterialEstudo_StartRowEditing"
        OnRowInserting="grdMaterialEstudo_RowInserting" OnRowUpdating="grdMaterialEstudo_RowUpdating"
        OnRowDeleting="grdMaterialEstudo_RowDeleting" Width="700px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovo" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdMaterialEstudo.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="MATERIALESTUDOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
                FieldName="DESCRICAO" Width="400px">
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
