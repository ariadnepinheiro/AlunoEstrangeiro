<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConsultaCartao.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ConsultaCartao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <table>
 <tr>
            <td>
               <asp:Label ID="Label1" runat="server" 
                    Text="Consulta Cartão Estudante/Gratuidade" 
                    Style="font-size: small; "></asp:Label>
            </td>
        </tr>
</table>
<br/>
    <table class="style1">
        <tr>
            <td>
               <asp:Label ID="Label2" runat="server" 
                    Text="Situação - emissão cartão do estudante/gratuidade" 
                    Style="font-weight: 700; font-size: x-small; text-decoration: underline;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSituacao" runat="server" SkinID="lblMensagem"
                    Style="font-weight: 700; font-size: small"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
</asp:Content>
