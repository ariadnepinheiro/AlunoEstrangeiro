<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Grupo.aspx.cs" Inherits="Techne.Lyceum.Net.InspecaoEscolar.Grupo" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsCampanha" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Grupo"
        SelectMethod="ListaCampanha"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupo" runat="server" TypeName="Techne.Lyceum.Net.InspecaoEscolar.Grupo"
        InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete" SelectMethod="ListaGrupo">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdGrupo" runat="server" DataSourceID="odsGrupo" KeyFieldName="GRUPOID"
        AutoGenerateColumns="false" ClientInstanceName="grdGrupo" OnInitNewRow="grdGrupo_InitNewRow"
        OnStartRowEditing="grdGrupo_StartRowEditing" OnRowInserting="grdGrupo_RowInserting"
        OnCellEditorInitialize="grdGrupo_CellEditorInitialize" OnRowUpdating="grdGrupo_RowUpdating"
        OnRowDeleting="grdGrupo_RowDeleting">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsPager PageSize="15" />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdGrupo.AddNewRow();" />
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
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="1" Caption="CAMPANHA*" Name="ddlCAMPANHA"
                FieldName="CAMPANHAID" Width="800" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsCampanha" TextField="CAMPANHA" ValueField="CAMPANHAID"
                    ValueType="System.String">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="2" FieldName="GRUPOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="500">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="3"
                FieldName="DESCRICAO" Width="1500px">
                <PropertiesTextEdit MaxLength="500">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ordem*" Name="ORDEM" VisibleIndex="4" FieldName="ORDEM"
                UnboundType="Integer">
                <PropertiesTextEdit MaxLength="100">
                    <ValidationSettings ErrorText="">
                        <RegularExpression ErrorText="A ordem só aceita valores numéricos e inteiros." ValidationExpression="\d+" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <br />
</asp:Content>
