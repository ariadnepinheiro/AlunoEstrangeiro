<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="UsuarioUnidadeCertificadora.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.UsuarioUnidadeCertificadora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="divEditBlock" style="width: 700px;">
                <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
                <asp:ImageButton ID="btnCancelar" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
                <asp:Label runat="server" ID="lblBloco" Text="Usuário Unidade Certificadora" SkinID="BcTitulo" />
                <asp:ValidationSummary ID="vsUnidade" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
                    ShowSummary="false" />
            </div>
            <div>
                <asp:Panel ID="pnUsuarioUnidade" runat="server" GroupingText="Faça uma busca por usuário"
                    Width="600px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblUsuarioTSearch" runat="server" Text="Usuário:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseUsuario" runat="server" SqlSelect="SELECT DISTINCT usuario, nomeusuario FROM usuario"
                                    SqlOrder="usuario" Argument="nomeusuario" Caption="" MaxLength="15" OnChanged="tseUsuario_Changed">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Usuário" FieldName="usuario" Width="30%" />
                                        <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomeusuario" Width="70%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBuscaUnidade" runat="server" Text="Unidade Certificadora:*" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseUnidade" runat="server" Key="UNIDADECERTIFICADORAID" Argument="DESCRICAO"
                                    SqlSelect="SELECT TIPO FROM [CertificacaoEscolar].[UNIDADECERTIFICADORA] UC "
                                    SqlOrder="DESCRICAO" MaxLength="20" OnChanged="tseUnidade_Changed" DataType="Number">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="UNIDADECERTIFICADORAID" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade" FieldName="DESCRICAO" Width="50%" />
                                        <tweb:TSearchBoxColumn Caption="Tipo" FieldName="TIPO" Width="30%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="btnIncluir" Text="Incluir" Width="160px" runat="server" OnClick="btnIncluir_Click" />
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnNovo" />
            <asp:PostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="btnIncluir" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:ObjectDataSource ID="odsUsuarioUnidade" runat="server" TypeName="Techne.Lyceum.Net.Certificacao.UsuarioUnidadeCertificadora"
        SelectMethod="Lista" DeleteMethod="Delete"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdUsuarioUnidade" runat="server" AutoGenerateColumns="False"
        Width="500px" ClientInstanceName="grdUsuarioUnidade" DataSourceID="odsUsuarioUnidade"
        KeyFieldName="USUARIOUNIDADECERTIFICADORAID" OnRowDeleting="grdUsuarioUnidade_RowDeleting"
        OnAfterPerformCallback="grdUsuarioUnidade_OnAfterPerformCallback" Visible="true">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Codigo" FieldName="USUARIOUNIDADECERTIFICADORAID"
                ReadOnly="True" VisibleIndex="1" Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade Certificadora" FieldName="UNIDADECERTIFICADORA"
                VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIO" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome Usuário" FieldName="NOME" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
