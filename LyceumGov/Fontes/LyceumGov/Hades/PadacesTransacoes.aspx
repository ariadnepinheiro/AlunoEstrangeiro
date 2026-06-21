<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadacesTransacoes.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.PadacesTransacoes" %>

<asp:Content ID="conpadacesUsuarios" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ScriptManagerProxy ID="manager" runat="server"/>
        
    <script type="text/javascript">
        function DisparaCliqueBotao() {
            __doPostBack("<%= dummy.ClientID %>", "");
        }
    </script>

    <asp:Label ID="lblInvisible" runat="server" Visible="false"></asp:Label>

    <asp:UpdatePanel ID="uppTree" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTexto" runat="server" Font-Bold="true"></asp:Label>
            <asp:Label ID="lblPadaces" runat="server" Font-Bold="true"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <br />
            <br />
            <table>
                <tr style="width: 100%">
                    <td style="width: 30%; vertical-align: top">
                        <asp:TreeView ID="treeMenu" runat="server" ForeColor="#0066FF" ImageSet="Arrows"
                            CollapseImageToolTip="Recolher {0}" ExpandImageToolTip="Expandir {0}" OnSelectedNodeChanged="treeMenu_SelectedNodeChanged"
                            EnableClientScript="true" EnableViewState="true">
                            <NodeStyle HorizontalPadding="10px" />
                            <SelectedNodeStyle CssClass="TreeViewSelected" />
                        </asp:TreeView>
                        <br />
                        <br />
                        <br />
                        <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
                        <asp:Button ID="btnAlterar" runat="server" Text="Alterar" OnClick="btnAlterar_Click" />
                    </td>
                    <td style="width: 70%; vertical-align: top">
                        <table>
                            <tr>
                                <td>
                                    <asp:TreeView ID="treePaginas" runat="server" ForeColor="#0066FF" ImageSet="Arrows"
                                        Visible="false" CollapseImageToolTip="Recolher {0}" ExpandImageToolTip="Expandir {0}"
                                        EnableClientScript="true" OnTreeNodeCheckChanged="treePaginas_TreeNodeCheckChanged"
                                        EnableViewState="true" ShowCheckBoxes="All">
                                        <NodeStyle HorizontalPadding="10px" />
                                        <SelectedNodeStyle CssClass="TreeViewSelected" />
                                    </asp:TreeView>
                                    <br />
                                    <br />
                                    <center>
                                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" /></center>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Button runat="server" ID="dummy" OnClick="dummy_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
</asp:Content>
