<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="MotivoExigenciaCredDeb.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.MotivoRepOperacao" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
<asp:ObjectDataSource ID="odsTipoVeiculo" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.MotivoRepOperacao"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update"
        DeleteMethod="Delete" >
    <DeleteParameters>
        <asp:Parameter Name="MOTIVOREPROVACAOOPERACAOID" Type="Object" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="DESCRICAO" Type="Object" />
        <asp:Parameter Name="ATIVO" Type="Object" />
        <asp:Parameter Name="MOTIVOREPROVACAOOPERACAOID" Type="Object" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="DESCRICAO" Type="Object" />
        <asp:Parameter Name="ATIVO" Type="Object" />
    </InsertParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdTipoVeiculo" runat="server" DataSourceID="odsTipoVeiculo"
        KeyFieldName="MOTIVOREPROVACAOOPERACAOID" AutoGenerateColumns="false" ClientInstanceName="grdTipoVeiculo"
        OnInitNewRow="grdTipoVeiculo_InitNewRow" OnStartRowEditing="grdTipoVeiculo_StartRowEditing"
        OnRowInserting="grdTipoVeiculo_RowInserting" OnRowUpdating="grdTipoVeiculo_RowUpdating"
        OnRowDeleting="grdTipoVeiculo_RowDeleting" Width="700px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdTipoVeiculo.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="MOTIVOREPROVACAOOPERACAOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
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
