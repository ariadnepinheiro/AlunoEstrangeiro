<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="TesteHorario.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.TesteHorario" %>

<asp:Content ID="conTeste" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <br />
    <asp:Label ID="lblGrade" runat="server" Text="Entre com ID da Grade:    "></asp:Label>
    <asp:TextBox ID="txtTurma" runat="server"></asp:TextBox>
    <asp:Button ID="botao" runat="server" Text="Enviar ->" OnClick="Botao_Click" />
</asp:Content>
