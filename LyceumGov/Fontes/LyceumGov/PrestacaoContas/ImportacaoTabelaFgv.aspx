<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ImportacaoTabelaFgv.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ImportacaoTabelaFgv" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informações da Unidade de Ensino"
        Width="600px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Região FGV:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegiaoFGV" runat="server" SqlOrder="descricao" SqlSelect="select datainicio,datafim FROM PrestacaoContas.REGIAOFGV"
                        Key="regiaofgvid" DataType="Number" GridWidth="600px" ArgumentColumns="30" Columns="2" MaxLength="30" OnChanged="tseRegiaoFGV_Changed" Argument="descricao">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Id" FieldName="regiaofgvid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label1" Style="margin-left: 23px;" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label5" Style="margin-left: 23px;" Text="Mês:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMes" runat="server" DataTextField="DESCRICAO" DataValueField="CODIGO">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Text="Selecione o arquivo a ser importado"></asp:Label>
                    <input id="arquivo" type="file" runat="server" name="oFile">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnImportar" runat="server" Text="Importar" OnClick="btnImportar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlGridCoordenadoria" runat="server" Visible="false">
    </asp:Panel>
</asp:Content>
