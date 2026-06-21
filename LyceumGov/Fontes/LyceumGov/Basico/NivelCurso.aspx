<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="NivelCurso.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.NivelCurso" %>

<asp:Content ID="ctNivelCurso" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsNivelCurso" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_tipo_curso">
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsDetalhe" runat="server" TypeName="Techne.Lyceum.RN.NivelCurso"
        SelectMethod="ObterDetalhe">
    </asp:ObjectDataSource>
    <br />
    <dxwgv:ASPxGridView ID="grdTiposCurso" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdTiposCurso" DataSourceID="tdsNivelCurso" KeyFieldName="tipo"
        OnCellEditorInitialize="grdTiposCurso_CellEditorInitialize" OnInitNewRow="grdTiposCurso_InitNewRow"
        OnStartRowEditing="grdTiposCurso_StartRowEditing"
		OnAfterPerformCallback="grdTiposCurso_AfterPerformCallback">
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" >
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" 
                            onclick="grdTiposCurso.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <UpdateButton>
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn VisibleIndex="1" Caption="Nível*" FieldName="tipo" ReadOnly="True" 
                Width="150px" HeaderStyle-Font-Bold="true">
                <PropertiesTextEdit MaxLength="20">
                  <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="True" ErrorText="Favor informar o Nível"></RequiredField>
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Descrição*" FieldName="descricao" 
                Width="300px" HeaderStyle-Font-Bold="true">
                <PropertiesTextEdit MaxLength="100">
                  <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="True" ErrorText="Favor informar a Descrição"></RequiredField>
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn> 
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="3" Caption="Detalhe" Name="detalhe" FieldName="detalhe"
                Width="200" HeaderStyle-Font-Bold="true">
                <PropertiesComboBox DataSourceID="odsDetalhe" TextField="descricao" ValueField="item"
                    ValueType="System.String">
            </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
        </Columns>
        <Settings ShowFilterRow="True" />
    </dxwgv:ASPxGridView>
    <br />
</asp:Content>
