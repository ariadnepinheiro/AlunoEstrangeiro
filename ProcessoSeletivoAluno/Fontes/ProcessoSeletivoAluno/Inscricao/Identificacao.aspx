<%@ Page Language="C#" MasterPageFile="~/Modulos/PublicMaster.Master" AutoEventWireup="true"
    CodeBehind="Identificacao.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivoAluno.Identificacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OpenPopup(ctrlid) {

            window.open("L2List.aspx?ctrlid=" + ctrlid, "List", "left = 300, top=150,scrollbars=no,resizable=no,width=400,height=280");

            return false;
        }

        $(document).ready(function() {
            $("#<%= this.txtAluno.ClientID %>").live("cut copy paste", function(e) {
                e.preventDefault();
            });
        });

        $(document).ready(function() {
            $("#<%= this.txtNomeMae.ClientID %>").live("cut copy paste", function(e) {
                e.preventDefault();
            });
        });

        function TextToUpper(str) {
            str.value = trim(str.value.toUpperCase().trim());
            return (str);
        }

        function trim(str) {
            return str.replace(/^\s+|\s+$/g, "");
        }

        function isNumberKey(evt) {
            var RB1 = document.getElementById("<%=rbTipoDadosInscricao.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var isChecked = false;
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (radio[1].checked) {
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
            else {
                return validaLetra(charCode);
            }
        }

        function validaLetra(charCode) {
            if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)
             || (charCode >= 192 && charCode <= 255) || (charCode == 32) || (charCode == 39) || (charCode == 8)) {
                return true;
            }
            else {
                return false;
            }
        }

        function Sotexto(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            return validaLetra(charCode);
        }
        
        function soNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
            {
                 return false;
            }

            return true;
        }
    </script>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <div class="login">
        <h3>
            <img src="../Images/sel.png" alt="" width="18" height="15" align="middle" />&nbsp;Identificação
            Processo Seletivo Aluno:</h3>
        <div class="log_int">
            <asp:UpdatePanel ID="UpdateCandidatoInscricao" runat="server">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2">
                                <h3>
                                    Informe seus dados</h3>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rbTipoDadosInscricao" runat="server" RepeatDirection="Horizontal"
                                    Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="rbTipoDadosInscricao_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">Nome candidato</asp:ListItem>
                                    <asp:ListItem Value="1">Número Inscrição</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtAluno" runat="server" Height="20px" Width="400px" Style="text-transform: uppercase;"
                                    onkeypress="return isNumberKey(event)" onblur="TextToUpper(this);" MaxLength="100"
                                    AutoComplete="Off"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label3" runat="server" Text="Nome Mãe" SkinID="lblObrigatorio" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtNomeMae" runat="server" Height="20px" Width="400px" Style="text-transform: uppercase;"
                                    onkeypress="return Sotexto(event)" onblur="TextToUpper(this);" AutoComplete="Off"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblDataNasc" runat="server" SkinID="lblObrigatorio" Text="Data de Nascimento:"
                                    Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" MinDate="1901-01-01" Width="140px"
                                    Height="20px" Font-Size="12px" ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje" Paddings-Padding="1px">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="2">
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td align="center" width="400px">
                        <asp:Image ID="imgChave" runat="server" ImageUrl="GeraChaveSeguranca.aspx" />
                        <asp:LinkButton ID="btnAtualizaImagemCaptcha" Text="Trocar Imagem" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblChave" runat="server" SkinID="lblObrigatorio" Text="Digite o código da imagem acima:"></asp:Label>
                        <asp:TextBox ID="txtChave" Visible="true" runat="server" onkeypress="return soNumero(event)"
                            MaxLength="6" Width="50"></asp:TextBox>
                    </td>
                </tr>               
                <tr>
                    <td align="right">
                        <asp:ImageButton ID="BtProsseguir" runat="server" ImageUrl="~/Images/bot_prosseguir.png"
                            OnClick="BtProsseguir_Click"></asp:ImageButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" runat="server" CssClass="overlay">
                <asp:Panel ID="Panel2" runat="server" CssClass="loader">
                    <asp:Image ID="Image1" runat="server" AlternateText="Updating..." Height="48" ImageUrl="~/Images/updateProgress.gif"
                        Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
