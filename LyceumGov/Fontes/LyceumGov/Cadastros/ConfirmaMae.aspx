<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConfirmaMae.aspx.cs" Inherits="Techne.Lyceum.Net.Cadastros.ConfirmaMae" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }     
     
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnMaeInscricaoId" runat="server" />
    <div class="divEditBlock" style="width: 90%;">
        <asp:ImageButton ID="btnVoltar" runat="server" ImageAlign="Right" SkinID="Voltar"
            OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Confirmação de Candidato" SkinID="BcTitulo" />
    </div>
    <div id="divPrincipal" runat="server" visible="false">
        <br />
        <asp:Panel ID="Panel1" runat="server" GroupingText="Dados do Candidato" Width="60%">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="Unidade:" SkinID="lblObrigatorio"></asp:Label>
                    </td>                   
                    <td>
                        <asp:Label ID="lblUnidade" runat="server" SkinID="lblObrigatorio"></asp:Label>
                        <asp:HiddenField ID="hdnCenso" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label Font-Names="Verdana" ID="Label3" SkinID="lblObrigatorio" runat="server"
                            Text="Nome:*"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNome" runat="server" ReadOnly="true" Enabled="false" Width="600px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label Font-Names="Verdana" ID="Label4" SkinID="lblObrigatorio" runat="server"
                            Text="CPF:*"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCPF" runat="server" ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="pnlConfirmação" runat="server" GroupingText="Confirmação" Width="20%">
            <table>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblConfirmacao" runat="server" RepeatDirection="Horizontal"
                            onchange="Bloqueio()" AutoPostBack="true" Width="201px" OnSelectedIndexChanged="rblConfirmacao_SelectedIndexChanged">
                            <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="Nao"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Panel ID="pnlMotivo" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Motivo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMotivo" runat="server" DataTextField="DESCRICAO" DataValueField="MAE_MOTIVONAOHABILITADOID"
                                            AppendDataBoundItems="true" Width="201px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlData" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Data Início:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                            ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar Confirmação"
                        OnClick="btnSalvar_Click" Visible="false" OnClientClick="Bloqueio();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
