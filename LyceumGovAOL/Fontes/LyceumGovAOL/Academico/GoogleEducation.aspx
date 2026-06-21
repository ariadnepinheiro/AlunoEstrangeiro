<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="GoogleEducation.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.GoogleEducation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">    
    <br />
    <asp:Label ID="lblMensagem" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
    <br />
    <table class="card">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Esta é sua conta para acessar o Classroom:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblEmail" runat="server" Style="text-decoration: underline; font-weight: bold;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label1" runat="server" Text="Link para acesso:"></asp:Label>
            </td>
            <td>                
                <asp:HyperLink  NavigateUrl="https://classroom.google.com/h" ID="HyperLink1" runat="server">Google Classroom</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
