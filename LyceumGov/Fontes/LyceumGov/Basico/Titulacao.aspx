<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Titulacao.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Titulacao"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<asp:Content ID="conTitulacao" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsTitulacao" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_titulacao">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView ID="grdTitulacao" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdTitulacao" DataSourceID="tdsTitulacao" KeyFieldName="titulacao"
        OnCellEditorInitialize="grdTitulacao_CellEditorInitialize" Font-Names="Verdana"
        Font-Size="Small" OnInitNewRow="grdTitulacao_InitNewRow" 
        OnStartRowEditing="grdTitulacao_StartRowEditing" 
        onrowinserting="grdTitulacao_RowInserting"
        OnRowValidating="grdTitulacao_RowValidating"
		OnAfterPerformCallback="grdTitulacao_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdTitulacao.AddNewRow();"
                            alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="Titulação*" HeaderStyle-Font-Bold="true" FieldName="titulacao" VisibleIndex="1" Width="200px">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Titulação." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao" VisibleIndex="2"
                Width="720px">
                <PropertiesTextEdit MaxLength="100" Width="720px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Grau" FieldName="nr_grau" VisibleIndex="3"
                UnboundType="Integer" Width="50px">
                <PropertiesTextEdit MaxLength="3" Width="50px">
                    <ValidationSettings ErrorText="">
                        <RegularExpression ErrorText="A coluna Grau só aceita valores numéricos e inteiros." ValidationExpression="\d{0,3}" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
         <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
