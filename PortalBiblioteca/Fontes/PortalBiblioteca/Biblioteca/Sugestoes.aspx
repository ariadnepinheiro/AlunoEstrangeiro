<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PublicMaster.Master"
    AutoEventWireup="true" CodeBehind="Sugestoes.aspx.cs" Inherits="Techne.Lyceum.Net.Biblioteca.Sugestoes" %>

<asp:Content ID="conCompletarRegistro" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnSugestoes">
        <br />
        <br />
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblTexto" runat="server" Text="Registrar sugestão de aquisição.<br>Caso não tenha encontrado o material desejado, preencha o formulário abaixo para solicitar a sua aquisição."></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblTitulo" runat="server" Text="Título:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTitulo" runat="server" MaxLength="500" Width="600px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo"
                        InitialValue="" ErrorMessage="Título: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAutor" runat="server" Text="Autor(es):*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAutor" runat="server" MaxLength="500" Width="600px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTitulo"
                        InitialValue="" ErrorMessage="Autor(es): Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEditora" runat="server" Text="Editora: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEditora" runat="server" MaxLength="500" Width="600px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAno" runat="server" MaxLength="4" Width="80px" SkinID="numerico"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblObs" runat="server" Text="Observações:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtObs" runat="server" MaxLength="500" Width="600px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="btnEnviar" Text="Enviar" runat="server" OnClick="btnEnviar_Click"
                        ValidationGroup="SalvarForm" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
