<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Velocidade.aspx.cs" Inherits="Techne.Lyceum.Net.Interconectividade.Velocidade" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
<asp:ObjectDataSource ID="odsVelocidade" runat="server" TypeName="Techne.Lyceum.Net.Interconectividade.Velocidade"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update"
        DeleteMethod="Delete"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsUnidadeVelocidade" TypeName="Techne.Lyceum.Net.Interconectividade.Velocidade"
        SelectMethod="ListaUnidade" runat="server"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdVelocidade" runat="server" DataSourceID="odsVelocidade"
        KeyFieldName="VELOCIDADEID" AutoGenerateColumns="false" ClientInstanceName="grdVelocidade"
        OnInitNewRow="grdVelocidade_InitNewRow" OnStartRowEditing="grdVelocidade_StartRowEditing"
        OnRowInserting="grdVelocidade_RowInserting" OnRowUpdating="grdVelocidade_RowUpdating"
        OnRowDeleting="grdVelocidade_RowDeleting" Width="500px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdVelocidade.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="VelocidadeID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="1" Caption="Unidade*" FieldName="UNIDADEVELOCIDADEID"
                Width="150px">
                <PropertiesComboBox TextField="DESCRICAO" ValueField="UNIDADEVELOCIDADEID" EnableSynchronization="False"
                    EnableIncrementalFiltering="True" DataSourceID="odsUnidadeVelocidade" ClientInstanceName="UNIDADEVELOCIDADEID">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Valor*" Name="Valor" VisibleIndex="2"
                FieldName="VALOR" Width="400px">
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