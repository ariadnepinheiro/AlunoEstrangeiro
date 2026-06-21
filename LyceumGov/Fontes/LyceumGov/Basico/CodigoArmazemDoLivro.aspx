<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CodigoArmazemDoLivro.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CodigoArmazemDoLivro" %>

<asp:Content ID="ctAlterarSenhaMat" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por CPF"
        Height="51px" Width="600px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBusca" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>                    
                       <tweb:TSearch ID="tseUsuario" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryArmazemLivro"
                        AutoPostBack="true" OnTextChanged="tseUsuario_Changed" >
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    
    <% if (!string.IsNullOrEmpty(lblMensagem.Text)) { %>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <% } %>
    
    <% if (!string.IsNullOrEmpty(CodigoAcesso)) { %>
    <asp:Panel ID="pnCodigo" runat="server" GroupingText="Seu código" Width="600px" Visible="false">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCodigo" runat="server" Text="Código: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <%= CodigoAcesso %>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <% } %>
    <asp:Panel ID="pnEmail" runat="server" GroupingText="E-Mail do destinatário que receberá o código" Width="600px" Visible="false">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblEmail" runat="server" Text="E-Mail:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail"
                        InitialValue="" ErrorMessage="E-Mail: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                    <asp:Button runat="server" ID="btnConfirmar" ValidationGroup="SalvarForm" OnClick="btnConfirmar_Click" Text="Enviar" />
                </td>
            </tr>
            <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
                ValidationGroup="SalvarForm" ShowSummary="false" />
        </table>
    </asp:Panel>
</asp:Content>
