<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="TiposTransportes.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.TiposTransportes" %>

<asp:Content ID="conTiposDisciplina" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsTiposTransporte" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_tipo_Transporte">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView ID="grdTiposTransporte" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdTiposTransporte" DataSourceID="tdsTiposTransporte" KeyFieldName="tipo"
        OnCellEditorInitialize="grdTiposTransporte_CellEditorInitialize" Font-Names="Verdana"
        Font-Size="Small" OnInitNewRow="grdTiposTransporte_InitNewRow" OnStartRowEditing="grdTiposTransporte_StartRowEditing"
        OnAfterPerformCallback="grdTiposTransporte_AfterPerformCallback" Width="600px">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdTiposTransporte.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="Tipo*" HeaderStyle-Font-Bold="true" FieldName="tipo"
                VisibleIndex="1" Width="150px">
                <PropertiesTextEdit MaxLength="20" Width="150px">
                    <ClientSideEvents KeyPress="function (s, e){ SomentePermitirCodigo(s, e.htmlEvent); }" />
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RegularExpression ErrorText="Tipo não permite caracteres especiais, acentos e espaços."
                            ValidationExpression="^[A-Za-z0-9]*$" />
                        <RequiredField ErrorText="Favor informar o Tipo." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                VisibleIndex="2" Width="400px">
                <PropertiesTextEdit MaxLength="100" Width="400px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
