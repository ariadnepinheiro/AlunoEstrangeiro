<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="ManutencaoCep.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ManutencaoCep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

<asp:HiddenField ID="hdnIdRegional" runat="server"/>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Pesquisar CEP"
        Width="780px">
        <table>
            <tr>
                <td style="text-align: right;width: 27%;">
                    <asp:Label ID="tseLabel" runat="server" Text="Município" style="font-size:11px"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox 
                        ID="tseMunicipio" 
                        runat="server" 
                        Caption="" 
                        MaxLength="20" 
                        SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                        SqlOrder="nome" 
                        ArgumentColumns="30" 
                        Columns="10"
                        GridWidth="600px"
                        OnChanged="tseMunicipio_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <div class="divEditBlock" style="width: 742px;">
        <asp:ImageButton ID="novo" runat="server" SkinID="BcNovo" AlternateText="Adicionar CEP" OnClick="novo_Click" Visible="true" />
        <asp:ImageButton ID="cancel" runat="server" SkinID="BcCancelar" AlternateText="Cancelar" OnClick="cancel_Click" Visible="false" />
        <asp:Label runat="server" ID="Label1" Text="Manutenção de CEP" SkinID="BcTitulo" />
    </div>
    
    <asp:Panel ID="pnlInformacoes" runat="server" GroupingText="Inserir novo CEP:"
        Width="780px" Visible="false">
        <table style="width:100%">
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblMunicipio" runat="server" Text="Municipio:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMunicipio"  runat="server" Width="208px" DataTextField="NOME" DataValueField="CODIGO" AutoPostBack="true" style="height:17px" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>            
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCep" runat="server" Text="Cep:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCep" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblLogradouro" runat="server" Text="Logradouro:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLogradouro" runat="server" MaxLength="50" Width="200px"/>
                </td>  
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblBairro" runat="server" Text="Bairro:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBairro"  runat="server" Width="208px" DataTextField="DESCRICAO" DataValueField="DESCRICAO" style="height:17px">
                    </asp:DropDownList>
                </td>
            </tr>    
            <tr>
                <td colspan="2" style="text-align: right; width:100%;">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" Visible="false"/>
                </td>
            </tr>
            
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Label 
                        ID="lblMensagem" 
                        runat="server" 
                        EnableViewState="false"
                        Font-Bold="true">
                    </asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    
    <table>
            <tr>
                <td>
                    <asp:Panel ID="pnGrid" runat="server">

                        <dxwgv:ASPxGridView 
                            ID="grdCep" 
                            runat="server"
                            AutoGenerateColumns="False"
                            ClientInstanceName="grdRegional"
                            KeyFieldName="ID_LOGRADOURO"
                            OnRowDeleting="grdCep_RowDeleting">

                            <SettingsText EmptyDataRow="Não existem dados." />

                            <Columns>
                            
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                <DeleteButton Text="Remover" Visible="True">
                                    <Image Url="~/img/bt_exclui2.png" />
                                </DeleteButton>
                            </dxwgv:GridViewCommandColumn>

                                <dxwgv:GridViewDataTextColumn 
                                    Caption="ID LOGRADOURO" 
                                    FieldName="ID_LOGRADOURO" 
                                    Visible="false"/>

                                <dxwgv:GridViewDataTextColumn 
                                    Caption="MUNCÍPIO" 
                                    FieldName="MUNICIPIO" />

                                <dxwgv:GridViewDataSpinEditColumn 
                                    Caption="CEP" 
                                    FieldName="CEP" />

                                <dxwgv:GridViewDataTextColumn 
                                    Caption="LOGRADOURO" 
                                    FieldName="NOME" />

                                <dxwgv:GridViewDataTextColumn 
                                    Caption="BAIRRO" 
                                    FieldName="BAIRRO" />

                            </Columns>

                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />

                        </dxwgv:ASPxGridView>

                    </asp:Panel>
                </td>
            </tr>
        </table>

</asp:Content>
