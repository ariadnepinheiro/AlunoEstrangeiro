<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AlteraSenhaUsuario.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.AlteraSenhaUsuario" %>

<asp:Content ID="ctAlterarSenhaMat" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por usuário"
        Height="51px" Width="600px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBusca" runat="server" Text="Usuário:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUsuario" runat="server" Argument="nomeusuario" Caption=""
                        Key="usuario" SqlOrder="usuario" SqlSelect="SELECT usuario, nomeusuario, p.cpf, matricula, p.e_mail_interno FROM usuario u left join LY_PESSOA p on u.PESSOA_USUARIO = p.PESSOA"
                        OnChanged="tseUsuario_Changed" MaxLength="15">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Usuário" FieldName="usuario" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomeusuario" Width="55%" />
                            <tweb:TSearchBoxColumn Caption="E-mail" FieldName="e_mail_interno" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnSenha" runat="server" GroupingText="Nova Senha" Width="600px" Visible="false">
        <table>
            <tr>
            <td align="right">
                Usuário:
            </td>
            <td>
                <asp:Label ID="lblUsuario" runat="server" MaxLength="12"></asp:Label>
            </td>
            </tr>
            <tr>
                <td align="right">
                    Nome:
                </td>
                <td>
                    <asp:Label ID="lblNome" runat="server" MaxLength="12"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    CPF:
                </td>
                <td>
                    <asp:Label ID="lblCPF" runat="server" MaxLength="12"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Email:
                </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server" MaxLength="12"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Último acesso:
                </td>
                <td>
                    <asp:Label ID="lblUltimoAcesso" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Último reset de senha:
                </td>
                <td>
                    <asp:Label ID="lblUltimoReset" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td align="right">
                </td>
                <td>                    
                   <dxe:ASPxButton ID="btnResetarCpf" runat="server" Text="Resetar Senha para CPF" OnClick="btnResetarCpf_Click"></dxe:ASPxButton>
                </td>
            </tr>  
            <tr>
                <td align="right">
                </td>
                <td>&nbsp;
                </td>
            </tr>   
            <tr>
                <td align="right">
                    <asp:Label ID="lblSenha" runat="server" Text="Senha:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSenha" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtSenha" runat="server" ControlToValidate="txtSenha"
                        InitialValue="" ErrorMessage="Senha: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>  
            <tr>
                <td align="right">
                    <asp:Label ID="lblConfirmarSenha" runat="server" Text="Confirmar Senha:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmarSenha" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtConfirmarSenha" runat="server" ControlToValidate="txtConfirmarSenha"
                        InitialValue="" ErrorMessage="Confirmar Senha: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
                <td>                    
                   <dxe:ASPxButton ID="btnResetar" runat="server" Text="Resetar Senha" OnClick="btnResetar_Click"></dxe:ASPxButton>
                </td>
            </tr>           
        </table>
    </asp:Panel>
</asp:Content>
