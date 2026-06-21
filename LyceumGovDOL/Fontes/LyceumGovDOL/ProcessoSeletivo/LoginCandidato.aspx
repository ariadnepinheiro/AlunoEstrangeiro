<%@ Page Language="c#" CodeBehind="LoginCandidato.aspx.cs" AutoEventWireup="True"
    MasterPageFile="~/Modulos/ProcessoSeletivoMaster.Master" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.LoginCandidato" %>

<asp:Content ContentPlaceHolderID="cphFormulario" ID="Content1" runat="server">
    <asp:Panel ID="PanelLogin" runat="server" Width="98%">
        <div class="login">
            <h3>
                &nbsp;<img src="../Images/sel.png" alt="" width="18" height="15" align="absmiddle" />&nbsp;Efetue
                seu login:</h3>
            <div class="log_int">
                <strong>
                    <asp:Label ID="Label1" TabIndex="-1" runat="server">Usuário:</asp:Label></strong><br />
                <asp:TextBox ID="cUsuario" runat="server" CssClass="log_form"></asp:TextBox>
                <br />
                <br />
                <strong>
                    <asp:Label ID="Label2" TabIndex="-1" runat="server">Senha:</asp:Label></strong><br />
                <asp:TextBox ID="cSenha" runat="server" ToolTip="Senha" TextMode="Password" CssClass="log_form"
                    MaxLength="15"></asp:TextBox>
                <br />
                <p align="right">
                    <asp:ImageButton ID="bt_Entrar" runat="server" ImageUrl="~/Images/bot_entrar.png"
                        OnClick="bt_Entrar_Click"></asp:ImageButton>
                    <br />
                    <br />
                    <asp:Label ID="lblErroSenha" runat="server" SkinID="lblMensagem"></asp:Label>
                    <asp:Label ID="cMsg" TabIndex="-1" runat="server" Font-Bold="True" CssClass="MsgError"></asp:Label>
                </p>
            </div>
        </div>
    </asp:Panel>
    <asp:Label ID="lblMsgAlteracao" runat="server" CssClass="azul1"></asp:Label>
    <asp:Panel ID="PanelPassword" runat="server" Visible="False">
        <div class="login">
            <h3>
                &nbsp;<img src="../Images/sel.png" alt="" width="18" height="15" align="absmiddle" />&nbsp;Alteração
                de Senha:</h3>
            <div class="log_int">
                <strong>
                    <asp:Label ID="lbltitUsuario2" runat="server">Usu&aacute;rio:</asp:Label></strong><br />
                <asp:Label ID="lblUsuarioSenha" runat="server"></asp:Label><br>
                <br />
                <strong>
                    <asp:Label ID="lblSenhaAtual" runat="server">Senha Atual:</asp:Label></strong><br />
                <asp:TextBox ID="txtSenhaAtual" runat="server" TextMode="Password" CssClass="log_form"
                    MaxLength="15"></asp:TextBox><br />
                <br />
                <strong>
                    <asp:Label ID="Label4" TabIndex="-1" runat="server">Senha:</asp:Label></strong><br />
                <asp:TextBox ID="txtSenhaNova1" runat="server" ToolTip="Senha" TextMode="Password"
                    MaxLength="15" CssClass="log_form"></asp:TextBox><br />
                <br />
                <strong>
                    <asp:Label ID="lblSenhaNova2" runat="server">Senha Nova (Confirme):</asp:Label></strong><br />
                <asp:TextBox ID="txtSenhaNova2" runat="server" TextMode="Password" CssClass="log_form"
                    MaxLength="15"></asp:TextBox><br />
                <asp:ImageButton ID="ib_ok" runat="server" ImageUrl="~/Images/bot_confirmar.png"
                    OnClick="ib_ok_Click"></asp:ImageButton>
                <asp:ImageButton ID="ib_cancelar" runat="server" ImageUrl="~/Images/bot_cancelar.png"
                    OnClick="ib_cancelar_Click"></asp:ImageButton>
                <br />
                <br />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
