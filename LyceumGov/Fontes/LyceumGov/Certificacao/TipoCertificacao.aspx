<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="TipoCertificacao.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.TipoCertificacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsTipoCertificacao" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.TipoCertificacao"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdTipoCertificacao" runat="server" DataSourceID="odsTipoCertificacao"
        KeyFieldName="TIPOCERTIFICACAOID" AutoGenerateColumns="false" ClientInstanceName="grdTipoCertificacao"
        OnInitNewRow="grdTipoCertificacao_InitNewRow" OnStartRowEditing="grdTipoCertificacao_StartRowEditing"
        OnRowInserting="grdTipoCertificacao_RowInserting" OnRowUpdating="grdTipoCertificacao_RowUpdating"
        OnRowDeleting="grdTipoCertificacao_RowDeleting" OnAfterPerformCallback="grdTipoCertificacao_OnAfterPerformCallback"
        Width="600px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdTipoCertificacao.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="TIPOCERTIFICACAOID" Visible="false"></dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataMemoColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2" FieldName="DESCRICAO" Width="250px">
                <PropertiesMemoEdit Rows="5" Columns="20">
                </PropertiesMemoEdit>
            </dxwgv:GridViewDataMemoColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Permite Polo?*" FieldName="PERMITEPOLO" VisibleIndex="3" Width="50px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1" ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Permite CEJA?*" FieldName="PERMITECEJA" VisibleIndex="4" Width="50px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1" ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Permite Transparência?*" FieldName="PERMITETRANSPARENCIA" VisibleIndex="5" Width="50px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1" ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Etapa de Ensino*" Name="ETAPAENSINO" VisibleIndex="6" FieldName="ETAPAENSINO" Width="100px">
                <PropertiesComboBox ValueType="System.String" ClientInstanceName="ddlEtapaEnsino" EnableSynchronization="False" EnableIncrementalFiltering="True" >
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Campo ETAPA DE ENSINO é obrigatório." IsRequired="True" />
                    </ValidationSettings>
                    <Items>
                        <dxe:ListEditItem Text="FUNDAMENTAL" Value="FUNDAMENTAL" />
                        <dxe:ListEditItem Text="MÉDIO" Value="MEDIO" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="7" Width="50px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1" ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
