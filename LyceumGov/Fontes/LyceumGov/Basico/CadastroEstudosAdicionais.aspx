<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="CadastroEstudosAdicionais.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CadastroEstudosAdicionais" Title="Cadastro de Estudos Adicionais" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <dxwgv:ASPxGridView ID="grdEstudosAdicionais" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdEstudosAdicionais"
        KeyFieldName="ESTUDOADICIONALID" DataSourceID="odsEstudosAdicionais" OnAfterPerformCallback="grdEstudosAdicionais_AfterPerformCallback"
        OnRowValidating="grdEstudosAdicionais_RowValidating" OnRowInserting="grdEstudosAdicionais_RowInserting"
        OnRowUpdating="grdEstudosAdicionais_RowUpdating" OnRowDeleting="grdEstudosAdicionais_RowDeleting">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdEstudosAdicionais.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ESTUDOADICIONALID" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Estudo Adicional *" FieldName="NOME" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsEstudosAdicionais" TypeName="Techne.Lyceum.Net.Basico.CadastroEstudosAdicionais"
        runat="server" SelectMethod="Listar"></asp:ObjectDataSource>
</asp:Content>
