<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ResetaSenhaUsuarios.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.ResetaSenhaUsuarios" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <asp:Label ID="lblMensagem" SkinID="lblMensagem" runat="server"></asp:Label>
    <br />
    <dxtc:ASPxPageControl ID="pcResetar" runat="server" ClientInstanceName="pcResetar"
        ActiveTabIndex="2" Height="200px" Width="850px">
        <TabPages>           
            <dxtc:TabPage Text="Docente online">
                <ContentCollection>
                    <dxw:ContentControl ID="conDocenteOnline" runat="server">
                        <asp:Panel ID="pnDocenteOnline" runat="server" GroupingText="Selecione o docente para resetar a senha">
                            <br />
                            <table>
                                <tr>
                                    <td align="right">
                                        Id/Vínculo:
                                    </td>
                                    <td>
                                        <tweb:TSearch ID="tseDocente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocenteResetSenha"
                                            AutoPostBack="true" OnChanged="tseDocente_Changed" ValidateText="true">
                                        </tweb:TSearch>
                                    </td>
                                </tr>
                                <tr id="trNomeDocente" runat="server" visible="false">
                                    <td align="right">
                                        Nome:
                                    </td>
                                    <td>
                                        <asp:Literal ID="lNomeDocente" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr id="trCPFDocente" runat="server" visible="false">
                                    <td align="right">
                                        CPF:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCPFDocente" runat="server" MaxLength="12"></asp:Label>                                       
                                    </td>
                                </tr> 
                                <tr ID="trEmailDocente" runat="server" visible="false">
                                    <td align="right">
                                        Email:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmailDocente" runat="server" MaxLength="12"></asp:Label>                                       
                                    </td>
                                </tr>                                 
                                <tr id="trUltimoAcesso" runat="server" visible="false">
                                    <td align="right">
                                        Último acesso:
                                    </td>
                                    <td>                     
                                        <asp:Label ID="lblUltimoAcesso" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                 <tr id="trUltimoReset" runat="server" visible="false">
                                    <td align="right">
                                        Último reset de senha:
                                    </td>
                                    <td>                     
                                        <asp:Label ID="lblUltimoReset" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <dxe:ASPxButton ID="btnResetarDocente" runat="server" Text="Resetar Senha" OnClick="btnResetarDocente_Click">
                        </dxe:ASPxButton>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Portal Biblioteca" Visible="false">
                <ContentCollection>
                    <dxw:ContentControl ID="conPortalBiblioteca" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
