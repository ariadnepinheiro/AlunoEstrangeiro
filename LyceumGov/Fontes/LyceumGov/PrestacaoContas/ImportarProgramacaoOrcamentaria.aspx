<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ImportarProgramacaoOrcamentaria.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ImportarProgramacaoOrcamentaria" %>

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
                <td>
                    <asp:Label runat="server" ID="Label1" Style="margin-left: 23px;" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
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
