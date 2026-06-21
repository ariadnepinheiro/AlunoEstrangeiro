<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="AlteracaoSenha.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.AlteracaoSenha" %>

<asp:Content ID="conAlteracaoSenha" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <table>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblUnuario" runat="server" Text="Usuário: "></asp:Label>
            </td>
            <td colspan="3">
                <asp:Label ID="txtUsuario" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblNome" runat="server" Text="Nome: "></asp:Label>
            </td>
            <td colspan="3">
                <asp:Label ID="txtNome" runat="server" Text=" "></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblSenhaAntiga" runat="server" Text="Senha Antiga:* " SkinID="lblObrigatorio"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtSenhaAntiga" runat="server" MaxLength="30" Width="150" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvSenha1" runat="server" ControlToValidate="txtSenhaAntiga" Enabled="false"
                    InitialValue="" ErrorMessage="Senha Antiga: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblNovaSenha" runat="server" Text="Nova Senha:* " SkinID="lblObrigatorio"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtNovaSenha" runat="server" MaxLength="30" Width="150" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvSenha2" runat="server" ControlToValidate="txtNovaSenha"
                    InitialValue="" ErrorMessage="Nova Senha: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblConfirmaSenha" runat="server" Text="Confirmação Nova Senha:* "
                    SkinID="lblObrigatorio"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtConfirmaSenha" runat="server" MaxLength="30" Width="150" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvSenha3" runat="server" ControlToValidate="txtConfirmaSenha"
                    InitialValue="" ErrorMessage="Confirmação Nova Senha: Preenchimento obrigatório."
                    ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblDataAlteracao" runat="server" Text="Data de Alteração da Senha : "></asp:Label>
            </td>
            <td>
                <dxe:ASPxDateEdit ID="dtAlteracao" runat="server" ReadOnly="true">
                </dxe:ASPxDateEdit>
            </td>
        </tr>

        <tr>
            <td colspan="2" align="right">
                <asp:ImageButton ID="btConfirmar" runat="server" SkinID="Confirmar" OnClick="btConfirmar_Click"
                    ValidationGroup="SalvarForm" />
                <asp:ValidationSummary ID="vsAlteracaoSenha" runat="server" ShowMessageBox="true"
                    ValidationGroup="SalvarForm" ShowSummary="false" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblPrivilediado" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblHabilitado" runat="server" Text="" Visible="false"></asp:Label>
</asp:Content>
