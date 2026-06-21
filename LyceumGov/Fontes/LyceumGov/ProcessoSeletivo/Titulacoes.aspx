<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Titulacoes.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.Titulacoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <techne:TTableDataSource ID="tdsTitulacoes" runat="server" DataTableClassName="Techne.Lyceum.CR.LY_CONCURSO_TITULACAO">
    </techne:TTableDataSource>
        <dxwgv:ASPxGridView ID="grdTitulacoes" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdTitulacoes" DataSourceID="tdsTitulacoes" KeyFieldName="titulacao"
            Font-Names="Verdana" Font-Size="Small" Width="800px" OnInitNewRow="grdTitulacoes_InitNewRow"
            OnStartRowEditing="grdTitulacoes_StartRowEditing" 
            oncelleditorinitialize="grdTitulacoes_CellEditorInitialize" 
        onafterperformcallback="grdTitulacoes_AfterPerformCallback">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdTitulacoes.AddNewRow();"
                                alt="Novo" />
                        </div>
                    </HeaderCaptionTemplate>
                    <EditButton Text="Editar" Visible="true">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <DeleteButton Text="Remover" Visible="true">
                        <Image Url="~/img/bt_exclui2.png" />
                    </DeleteButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <UpdateButton>
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <ClearFilterButton Text="Limpar" Visible="true">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Titulação" FieldName="titulacao" VisibleIndex="1" Width="150">
                <PropertiesTextEdit MaxLength="20"></PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="descricao" VisibleIndex="2" Width="550">
                <PropertiesTextEdit MaxLength="500"></PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
</asp:Content>
