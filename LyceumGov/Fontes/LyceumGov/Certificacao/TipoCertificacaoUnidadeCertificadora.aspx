<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="TipoCertificacaoUnidadeCertificadora.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.TipoCertificacaoUnidadeCertificadora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    
    
    <asp:ObjectDataSource ID="odsTipoCertificacao" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.TipoCertificacaoUnidadeCertificadora" SelectMethod="ListaTipoCertificacao">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsUnidadeCertificadora" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.TipoCertificacaoUnidadeCertificadora" SelectMethod="ListaUnidadeCertificadora">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsTipoCertificacaoUnidadeCertificadora" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.TipoCertificacaoUnidadeCertificadora" SelectMethod="Lista" InsertMethod="Insert" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    
    <dxwgv:ASPxGridView ID="grdTipoCertificacao" runat="server" DataSourceID="odsTipoCertificacaoUnidadeCertificadora"
        KeyFieldName="TIPOCERTIFICACAOID;UNIDADECERTIFICADORAID" AutoGenerateColumns="false" ClientInstanceName="grdTipoCertificacao"
        OnInitNewRow="grdTipoCertificacao_InitNewRow"
        OnRowInserting="grdTipoCertificacao_RowInserting"
        OnRowDeleting="grdTipoCertificacao_RowDeleting" 
        OnAfterPerformCallback="grdTipoCertificacao_OnAfterPerformCallback"
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
            
            <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Certificação" VisibleIndex="1" FieldName="TIPOCERTIFICACAOID" Width="200px">
                <PropertiesComboBox DataSourceID="odsTipoCertificacao" TextField="DESCRICAO" ValueField="TIPOCERTIFICACAOID">
                    <Items>
                        <dxe:ListEditItem />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            
            <dxwgv:GridViewDataComboBoxColumn Caption="Unidade Certificadora" VisibleIndex="2" FieldName="UNIDADECERTIFICADORAID" Width="200px">
                <PropertiesComboBox DataSourceID="odsUnidadeCertificadora" TextField="UNIDADE" ValueField="UNIDADECERTIFICADORAID">
                    <Items>
                        <dxe:ListEditItem />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            
            <%--<dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="TIPOCERTIFICACAOID" Visible="false"></dxwgv:GridViewDataTextColumn>
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
            </dxwgv:GridViewDataCheckColumn>--%>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
