<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/PublicMaster.Master"
    CodeBehind="Edital.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivoAluno.Edital" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 904px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        $(document).ready(function() {
            $('div').bind('scroll', chk_scroll);
        });

        function chk_scroll(e) {
            var elem = $(e.currentTarget);
            if (elem[0].scrollHeight - elem.scrollTop() <= elem.outerHeight()) {
                $('table.geral').find('#ckbEdital').prop('disabled', false);
            }
        }

        function setaChkEdital() {
            var seChecado = $('#ctl00_cphFormulario_SeChecado');
            var check = $('table.geral').find('#ckbEdital').attr('checked') ? "True" : "False";
            seChecado.val(check);
        }
        
    </script>

    <table class="geral" style="width: 850px">
        <tr id="trSemProcessoSeletivoPeriodoInscricao" visible="false" runat="server">
            <td>
                <div id="div1" style="border: solid 1px #dd3c10; background-color: #ffebe8; font-family: Verdana;
                    color: #FF0000; font-weight: bold; text-align: center; padding: 100px;">
                    <asp:Label ID="lblErroSemProcessoSeletivoPeriodoInscricao" style="font-size: small" runat="server"
                        Text="N&atildeo existe nenhum processo seletivo dispon&iacutevel para inscriç&atildeo no momento."></asp:Label>
                </div>
            </td>
        </tr>
        <tr id="trEdital" runat="server">
            <td>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="font-family: Verdana; color: #FF0000; font-size: small; font-weight: bold"
                            class="style1">
                            <asp:Label ID="lblMensagem" runat="server" Text="É necessário ler todo o Edital para continuar."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <div id="divEdital" style="overflow: auto; width: 650px; height: 200px; border: solid 1px;
                                padding: 100px" runat="server">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div id="divControle" style="width: 850px">
                                <b>
                                    <input id="ckbEdital" type="checkbox" name="Edital" disabled="disabled" onclick="setaChkEdital();" />
                                    <span>Li e estou de acordo com todos os termos, normas e condições estabelecidas no
                                        presente EDITAL DE PROCESSO SELETIVO destinado à classificação de candidatos ao
                                        ingresso na 1ª série do Ensino Médio, nos colégios estaduais participantes.</span>
                                    <input type="hidden" runat="server" value="false" name="SeChecado" id="SeChecado" /></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:UpdatePanel ID="UpdateEdital" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnEdital" runat="server" Text="Avançar" OnClick="btnEdital_Click"
                                        Width="100px" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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
