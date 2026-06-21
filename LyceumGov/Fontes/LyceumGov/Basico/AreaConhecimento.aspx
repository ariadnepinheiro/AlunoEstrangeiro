<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AreaConhecimento.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AreaConhecimento" %>

<asp:Content ID="conAreaConhecimento" ContentPlaceHolderID="cphFormulario" runat="server">
<asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:ObjectDataSource ID="odsAreaConhecimento" TypeName="Techne.Lyceum.Net.Basico.AreaConhecimento"
        runat="server" SelectMethod="Listar" OnDeleting="odsAreaConhecimento_Deleting"
        DeleteMethod="DeleteArea" OnUpdating="odsAreaConhecimento_Updating" UpdateMethod="AlterArea"
        OnInserting="odsAreaConhecimento_Insert" InsertMethod="InsertArea">
        <%--<SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
        </SelectParameters>--%>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdAreaConhecimento" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdAreaConhecimento" DataSourceID="odsAreaConhecimento" KeyFieldName="AREACONHECIMENTOID"
        Font-Names="Verdana" Font-Size="Small" Width="60%" OnInitNewRow="grdAreaConhecimento_InitNewRow"
        OnStartRowEditing="grdAreaConhecimento_StartRowEditing" OnCellEditorInitialize="grdAreaConhecimento_CellEditorInitialize"
        OnAfterPerformCallback="grdAreaConhecimento_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdAreaConhecimento.AddNewRow();" alt="Novo" />
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
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="true">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Área de Conhecimento*" HeaderStyle-Font-Bold="true"
                FieldName="DESCRICAO" VisibleIndex="2" Width="60%">
                <PropertiesTextEdit MaxLength="50" Width="90%">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a área de conhecimento." IsRequired="true" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Styles>
            <CommandColumn Wrap="False">
            </CommandColumn>
        </Styles>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    
</asp:Content>
