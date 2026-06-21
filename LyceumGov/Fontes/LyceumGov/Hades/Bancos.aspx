<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Bancos.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Bancos" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
<techne:TTableDataSource ID="tdsBanco" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_banco"
        SqlOrder="nome">
    </techne:TTableDataSource>
<dxwgv:ASPxGridView ID="grdBanco" runat="server" AutoGenerateColumns="False" DataSourceID="tdsBanco"
        ClientInstanceName="grdBanco" KeyFieldName="banco"
        Font-Names="Verdana" Font-Size="Small"
        OnAfterPerformCallback="grdBanco_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdBanco.AddNewRow();"
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
            <dxwgv:GridViewDataTextColumn Caption="Número*" HeaderStyle-Font-Bold="true"
                FieldName="banco" VisibleIndex="1" Width="100px">
                <PropertiesTextEdit MaxLength="3" Width="90%">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o número do banco." IsRequired="True" />
                    </ValidationSettings>
                    <MaskSettings Mask="###" ErrorText="Este campo só permite números."/> 
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome*" HeaderStyle-Font-Bold="true"
                FieldName="nome" VisibleIndex="2" Width="355px">
                <PropertiesTextEdit MaxLength="100" Width="90%">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o nome." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Máscara Agência*" HeaderStyle-Font-Bold="true"
                FieldName="maskag" VisibleIndex="3" Width="55px" Visible="false">
                <PropertiesTextEdit MaxLength="7" Width="90%">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Máscara Conta*" HeaderStyle-Font-Bold="true"
                FieldName="maskconta" VisibleIndex="4" Width="105px" Visible="false">
                <PropertiesTextEdit MaxLength="15" Width="90%">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
