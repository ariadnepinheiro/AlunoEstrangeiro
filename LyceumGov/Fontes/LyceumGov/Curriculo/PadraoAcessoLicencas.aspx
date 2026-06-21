<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadraoAcessoLicencas.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.PadraoAcessoLicencas" %>

<asp:Content ID="ctPadraoAcLic" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsPadraoAcessoLicensa" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_padaces_licenca"
        SqlOrder="motivo" SqlWhere="padaces = @padaces">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="ddlPadaces" Name="padaces" PropertyName="SelectedValue" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsPadaces" TypeName="Techne.Lyceum.RN.PadroesDeAcessos"
        SelectMethod="ConsultarPadaces" runat="server"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsMotivo" TypeName="Techne.Lyceum.RN.Licencas" SelectMethod="ListarLicencas"
        runat="server"></asp:ObjectDataSource>
    <table>
        <tr>
            <td align="right">
                <asp:Label ID="lblPadaces" runat="server" Text="Padrão de Acesso:*" SkinID="lblObrigatorio"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlPadaces" DataSourceID="odsPadaces" DataTextField="nomepadaces"
                    AutoPostBack="true" DataValueField="padaces" runat="server" Width="300px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdAcesso" runat="server" AutoGenerateColumns="False" DataSourceID="tdsPadraoAcessoLicensa"
        ClientInstanceName="grdAcesso" KeyFieldName="padaces;motivo" Width="500px" Font-Names="Verdana"
        Font-Size="Small" OnAfterPerformCallback="grdAcesso_AfterPerformCallback" OnRowInserting="grdAcesso_RowInserting"
        OnInitNewRow="grdAcesso_InitNewRow" OnStartRowEditing="grdAcesso_StartRowEditing">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="70px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdAcesso.AddNewRow();" alt="Novo" />
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
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Padrao" FieldName="padaces" HeaderStyle-Font-Bold="true"
                VisibleIndex="1" Width="200px" Visible="false">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Licença*" FieldName="motivo" VisibleIndex="1">
                <PropertiesComboBox DataSourceID="odsMotivo" MaxLength="2" ValueType="System.String"
                    ValueField="motivo" TextField="descricao" Width="300px">
                    <ValidationSettings Display="Dynamic">
                        <RequiredField ErrorText="Favor informar a licença." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True" />
            </dxwgv:GridViewDataComboBoxColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
