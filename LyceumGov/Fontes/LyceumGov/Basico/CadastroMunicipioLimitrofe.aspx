<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CadastroMunicipioLimitrofe.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CadastroMunicipioLimitrofe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Município de Origem:" Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUFOrigem" runat="server" Text="UF:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUFOrigem" runat="server" AutoPostBack="True" DataTextField="uf_sigla"
                        DataValueField="uf_sigla" OnSelectedIndexChanged="ddlUFOrigem_SelectedIndexChanged"
                        AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblMunicipioOrigem" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct ID_MUNICIPIO, nome, uf from hades.dbo.TCE_MUNICIPIO m "
                        GridWidth="600px" ArgumentColumns="50" Columns="10" MaxLength="10" OnChanged="tseMunicipio_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="ID_MUNICIPIO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlLimitrofe" runat="server" GroupingText="Município Limítrofes:"
        Width="800px" Visible="False">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUFLimitrofe" runat="server" Text="UF:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUFLimitrofe" runat="server" AutoPostBack="True" DataTextField="uf_sigla"
                        DataValueField="uf_sigla" OnSelectedIndexChanged="ddlUFLimitrofe_SelectedIndexChanged"
                        AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblMunicipioLimitrofe" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipioLimitrofe" runat="server" SqlOrder="nome" SqlSelect=" select distinct ID_MUNICIPIO, nome, uf from hades.dbo.TCE_MUNICIPIO m "
                        GridWidth="600px" ArgumentColumns="50" Columns="10" OnChanged="tseMunicipioLimitrofe_Changed"
                        MaxLength="10">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="ID_MUNICIPIO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        OnClick="btnSalvar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <dxwgv:ASPxGridView ID="grdMunicipioLimitrofe" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grdMunicipioLimitrofe" DataSourceID="odsMunicipioLimitrofe"
                        KeyFieldName="ID_MUNICIPIO_LIMITROFE">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <DeleteButton Text="Remover" Visible="True">
                                    <Image Url="~/img/bt_exclui2.png" />
                                </DeleteButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_MUNICIPIO_LIMITROFE"
                                ReadOnly="true" VisibleIndex="1">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código Origem" FieldName="COD_ORIGEM" ReadOnly="true"
                                VisibleIndex="2">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Município Origem" FieldName="MUNICIPIO_ORIGEM"
                                ReadOnly="true" VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="UF Origem" FieldName="UF_ORIGEM" ReadOnly="true"
                                VisibleIndex="4">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código Município Limítrofe" FieldName="CODIGO_MUNICIPIO_LIMITROFE"
                                ReadOnly="true" VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="UF" FieldName="UF" ReadOnly="true" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Município Limítrofe" FieldName="MUNICIPIO_LIMITROFE"
                                ReadOnly="true" VisibleIndex="7">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:ObjectDataSource ID="odsMunicipioLimitrofe" TypeName="Techne.Lyceum.Net.Basico.CadastroMunicipioLimitrofe"
        runat="server" SelectMethod="Listar" OnDeleting="odsMunicipioLimitrofe_Deleting"
        DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseMunicipio" DefaultValue="" Name="ID_MUNICIPIO" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
</asp:Content>
