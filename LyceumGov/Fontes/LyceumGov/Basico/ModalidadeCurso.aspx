<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalidadeCurso.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ModalidadeCurso"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<asp:Content ID="conSubnivel" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsSubnivel" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_modalidade_curso">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsSistemaAvaliacao" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_sistema_avaliacao">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView ID="grdSubnivel" runat="server" AutoGenerateColumns="False" DataSourceID="tdsSubnivel" ClientInstanceName="grdSubnivel"
        KeyFieldName="modalidade" OnCellEditorInitialize="grdSubnivel_CellEditorInitialize"
        Font-Names="Verdana" Font-Size="Small" OnInitNewRow="grdSubnivel_InitNewRow"
        OnStartRowEditing="grdSubnivel_StartRowEditing"
		OnAfterPerformCallback="grdSubnivel_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdSubnivel.AddNewRow();"
                            alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Visible="True" Text="Editar">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Visible="True" Text="Remover">
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
            <dxwgv:GridViewDataTextColumn Caption="Modalidade*" HeaderStyle-Font-Bold="true" FieldName="modalidade" VisibleIndex="1"
                Width="200px">
                <PropertiesTextEdit MaxLength="20">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Modalidade." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao" VisibleIndex="2"
                Width="430px">
                <PropertiesTextEdit MaxLength="100">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Sistema Avaliação" FieldName="sistema_avaliacao"
                VisibleIndex="3" Width="250px">
                <PropertiesComboBox DataSourceID="tdsSistemaAvaliacao" TextField="descricao" ValueField="sistema_avaliacao"
                    ValueType="System.String">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
        </Columns>
         <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>