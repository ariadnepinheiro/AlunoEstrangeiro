<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Patrimonio.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.Patrimonio" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        $(document).ready(function() {
            $("input.dinheiro")
                 .maskMoney({
                     decimal: ",",
                     thousands: "."
                 })
        });

        function OnlyNumericEntry(e) {

            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function mostrarResultado(box, num_max, spContador) {
            var contagem_carac = box.length;
            if (contagem_carac != 0) {
                document.getElementById(spContador).innerHTML = contagem_carac + " caracteres digitados";
                if (contagem_carac == 1) {
                    document.getElementById(spContador).innerHTML = contagem_carac + " caracter digitado";
                }
                if (contagem_carac >= num_max) {
                    document.getElementById(spContador).innerHTML = "Limite de caracteres excedido!";
                }
            } else {
                document.getElementById(spContador).innerHTML = "Limite de " + num_max + " caracteres";
            }
        }
        function contarCaracteres(box, valor, spContador, campoMult) {

            var conta = valor - box.length;
            document.getElementById(spContador).innerHTML = "Você ainda pode digitar " + conta + " caracteres";
            if (box.length >= valor) {
                document.getElementById(spContador).innerHTML = "Limite excedido.";
                campoMult.value = campoMult.value.substr(0, valor);
            }
        }
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }

        function MascaraMoeda(objTextBox, SeparadorMilesimo, SeparadorDecimal, e, Tamanho) {

            var sep = 0;
            var key = '';
            var i = j = 0;
            var len = len2 = 0;
            var strCheck = '0123456789';
            var aux = aux2 = '';
            var whichCode = (window.Event) ? e.which : e.keyCode;
            if (whichCode == 13) return true;
            if (whichCode == 8) return true;

            key = String.fromCharCode(whichCode); // Valor para o código da Chave
            if (strCheck.indexOf(key) == -1) return false; // Chave inválida
            if (Tamanho < objTextBox.value.length) return false; // Tamanho
            len = objTextBox.value.length;

            for (i = 0; i < len; i++)
                if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != SeparadorDecimal)) break;
            aux = '';
            for (; i < len; i++)
                if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
            aux += key;
            len = aux.length;
            if (len == 0) objTextBox.value = '';
            if (len == 1) objTextBox.value = '0' + SeparadorDecimal + '0' + aux;
            if (len == 2) objTextBox.value = '0' + SeparadorDecimal + aux;
            if (len > 2) {
                aux2 = '';
                for (j = 0, i = len - 3; i >= 0; i--) {
                    if (j == 3) {
                        aux2 += SeparadorMilesimo;
                        j = 0;
                    }
                    aux2 += aux.charAt(i);
                    j++;
                }
                objTextBox.value = '';
                len2 = aux2.length;
                for (i = len2 - 1; i >= 0; i--)
                    objTextBox.value += aux2.charAt(i);
                objTextBox.value += SeparadorDecimal + aux.substr(len - 2, len);
            }
            return false;
        }

        function moeda(a, e, r, t) {

            var n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
            if (13 == o || 8 == o)
                return !0;
            if (n = String.fromCharCode(o),
    -1 == "0123456789".indexOf(n))
                return !1;
            for (u = a.value.length,
    h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++)
                ;
            for (l = ""; h < u; h++)
-1 != "0123456789".indexOf(a.value.charAt(h)) && (l += a.value.charAt(h));
            if (l += n,
    0 == (u = l.length) && (a.value = ""),
    1 == u && (a.value = "0" + r + "0" + l),
    2 == u && (a.value = "0" + r + l),
    u > 2) {
                for (ajd2 = "",
        j = 0,
        h = u - 3; h >= 0; h--)
                    3 == j && (ajd2 += e,
            j = 0),
            ajd2 += l.charAt(h),
            j++;
                for (a.value = "",
        tamanho2 = ajd2.length,
        h = tamanho2 - 1; h >= 0; h--)
                    a.value += ajd2.charAt(h);
                a.value += r + l.substr(u - 2, u)
            }
            return !1
        }

        function ConfirmaExecucao() {
            var txtQtdeReplicacao = $("#<%= this.txtQtdeReplicacao.ClientID %>");
            var hdnbemId = $("#<%= this.hdnbemId.ClientID %>");
            if (hdnbemId.val() == 0) {
                return confirm("Este processo criará " + $(txtQtdeReplicacao).val() + " bem(ns). Deseja continuar?");
            }
            else {
                return confirm("Este processo atualizará o bem. Deseja continuar?");
            }

        }
    </script>

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:HiddenField ID="hdnbemId" runat="server" />
    <asp:HiddenField ID="hdnSetor" runat="server" />
    <asp:HiddenField ID="hdnMoedaId" runat="server" />
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Localização do Patrimônio:"
        Width="900px">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label Font-Names="Verdana" ID="lblTextoUA" runat="server" Text="Unidade Administrativa:"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblUA" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    -
                    <asp:Label Font-Names="Verdana" ID="lblNomeUA" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="Voltar" ImageAlign="Right"
            OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" OnClientClick="return ConfirmaExecucao();Bloqueio()" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Patrimônio" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsPatrimonio" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label Font-Names="Verdana" ID="lblMensagem" SkinID="lblMensagem" runat="server"
        Font-Bold="true"></asp:Label>
    <br />
    <asp:Panel ID="pnlDadosPatrimonio" runat="server" GroupingText="Dados do Patrimônio:"
        Width="900px">
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rblOperacao" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="tseClassificacao" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="chkEfetuarBaixa" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlMotivoBaixa" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnEfetuarBaixa" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSalvarReavaliacao" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="rblBemInservivel" EventName="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblNumero" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                Text="Número:*"></asp:Label>
                            <asp:Label ID="lblQtdeReplicacao" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                Text="Quantidade:*"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNumero" runat="server" Width="80px" ReadOnly="true" Enabled="false" />
                            <asp:TextBox ID="txtQtdeReplicacao" runat="server" MaxLength="3" onkeypress="return OnlyNumericEntry(event)"
                                Width="35px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Operação:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblOperacao" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblOperacao_SelectedIndexChanged">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="lblClassificacao" SkinID="lblObrigatorio" runat="server"
                                Text="Classificação:*"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseClassificacao" runat="server" SqlSelect="SELECT CONTA, DESCRICAO,CLASSIFICACAOID FROM [LYCEUM].[Patrimonio].[CLASSIFICACAO]"
                                SqlOrder="CONTA" ColumnName="conta" Caption="" MaxLength="15" SqlWhere=" ativo=1 "
                                DataType="Varchar" OnChanged="tseClassificacao_Changed">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Conta" FieldName="CONTA" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Nome" FieldName="DESCRICAO" Width="80%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="Label2" SkinID="lblObrigatorio" runat="server"
                                Text="Descrição:*"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescricao" runat="server" MaxLength="1000" TextMode="MultiLine"
                                name="txtDescricao" Height="75px" Width="600px" onkeyup="mostrarResultado(this.value,1000,'spContadorDescricao');contarCaracteres(this.value,500,'spContadorDescricao',this)" />
                            <br />
                            <span id="spContadorDescricao" style="font-family: Georgia;">Limite de 1000 caracteres</span><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="lblEstadoConservacao" SkinID="lblObrigatorio"
                                runat="server" Text="Estado de Conservação:*"></asp:Label>
                            <asp:Label Font-Names="Verdana" ID="lblDataAquisicao" SkinID="lblObrigatorio" runat="server"
                                Text="Data da Aquisição:*"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEstadoConservacao" runat="server" DataTextField="conceito"
                                DataValueField="estadoconservacaoid">
                            </asp:DropDownList>
                            <dxe:ASPxDateEdit ID="dtDataAquisicao" runat="server" Width="100px" Enabled="true"
                                EnableDefaultAppearance="true" ClientInstanceName="dtDataAquisicao" CalendarProperties-ClearButtonText="Limpar"
                                CalendarProperties-TodayButtonText="Hoje">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label Font-Names="Verdana" ID="lblDataIncorporacao" SkinID="lblObrigatorio"
                                runat="server" Text="Data da Incorporação:*"></asp:Label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="dtDataIncorporacao" runat="server" Width="100px" Enabled="true"
                                EnableDefaultAppearance="true" ClientInstanceName="dtDataIncorporacao" CalendarProperties-ClearButtonText="Limpar"
                                CalendarProperties-TodayButtonText="Hoje">
                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                </CalendarProperties>
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="lblVidaUtil" SkinID="lblObrigatorio" runat="server"
                                Text="Período de Vida Útil:*"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriodoVidaUtil" runat="server" DataTextField="conceito"
                                DataValueField="QUANTIDADEANOS">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label Font-Names="Verdana" ID="lblPeriodoUtilizacaoFutura" SkinID="lblObrigatorio"
                                runat="server" Text="Período de Utilização Futura:*"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriodoUtilizacaoFutura" runat="server" DataTextField="conceito"
                                DataValueField="QUANTIDADEANOS">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblValor" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                Text="Valor de Mercado:*"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValor" runat="server" Width="80px" OnKeyPress="return(moeda(this,'.',',',event))" />
                        </td>
                        <td>
                            <asp:Label ID="Label17" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                Text="Moeda:*"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMoeda" runat="server" ReadOnly="true" Enabled="false" Width="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                Text="Documento Hábil:*"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDocumentoHabil" runat="server" Height="75px" MaxLength="500"
                                onkeyup="mostrarResultado(this.value,500,'spDocumentoHabil');contarCaracteres(this.value,500,'spDocumentoHabil',this)"
                                TextMode="MultiLine" Width="600px" />
                            <br />
                            <span id="spDocumentoHabil" style="font-family: Georgia;">Limite de 500 caracteres</span><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="7">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlHistorico" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label Font-Names="Verdana" ID="Label10" runat="server" Text="Histórico da Operação:"></asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtHistoricoOperacao" runat="server" MaxLength="500" TextMode="MultiLine"
                                    onkeyup="mostrarResultado(this.value,500,'spHistorico');contarCaracteres(this.value,500,'spHistorico',this)"
                                    Height="75px" Width="600px" />
                                <br />
                                <span id="spHistorico" style="font-family: Georgia;">Limite de 500 caracteres</span><br />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:CheckBox runat="server" ID="chkEfetuarBaixa" Text="Efetuar Baixa" AutoPostBack="true"
                                OnCheckedChanged="chkEfetuarBaixa_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlBaixa" runat="server" GroupingText="Dados da Baixa:" Width="700px">
                    <asp:Label Font-Names="Verdana" ID="lblMensagemBaixa" SkinID="lblMensagem" runat="server"
                        Font-Bold="true"></asp:Label>
                    <table>
                        <tr>
                            <td>
                                <asp:Label Font-Names="Verdana" ID="Label12" SkinID="lblObrigatorio" runat="server"
                                    Text="Motivo:*"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMotivoBaixa" runat="server" DataTextField="descricao" DataValueField="motivobaixaid"
                                    OnSelectedIndexChanged="ddlMotivoBaixa_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Processo*:" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProcessoPrefixo" runat="server">
                                    <asp:ListItem Text="Selecione" Value="">
                                    </asp:ListItem>
                                    <asp:ListItem Text="E-03/" Value="E-03/">
                                    </asp:ListItem>
                                    <asp:ListItem Text="SEI-" Value="SEI-">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtProcesso" runat="server" MaxLength="20" Width="109px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label Font-Names="Verdana" ID="Label5" SkinID="lblObrigatorio" runat="server"
                                    Text="Data da Baixa:*"></asp:Label>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtDataBaixa" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                                    ClientInstanceName="dtDataBaixa" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Font-Names="Verdana" ID="Label16" runat="server" Text="Observação:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtObservacao" runat="server" MaxLength="500" TextMode="MultiLine"
                                    Height="75px" Width="500px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Font-Names="Verdana" ID="lblBoletim" SkinID="lblObrigatorio" runat="server"
                                    Text="Boletim de Ocorrência:*"></asp:Label>
                                <asp:Label Font-Names="Verdana" ID="lblCNPJ" SkinID="lblObrigatorio" runat="server"
                                    Text="CNPJ:*"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoletimOcorrencia" runat="server" MaxLength="100" />
                                <asp:TextBox ID="txtCNPJ" runat="server" MaxLength="18" onkeypress="formataCNPJ(this,event)" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 2%">
                                <asp:Label Font-Names="Verdana" ID="lblPrefeitura" SkinID="lblObrigatorio" runat="server"
                                    Text="Prefeitura/<br />Instituição:*"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrefeituraInstituicao" runat="server" MaxLength="200" TextMode="MultiLine"
                                    Height="51px" Width="254px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnEfetuarBaixa" runat="server" Text="Salvar" OnClick="btnEfetuarBaixa_Click"
                                    OnClientClick="return confirm('Tem certeza que confirma a baixa do bem?');" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlReavaliacao" runat="server" GroupingText="Reavaliação:" Width="700px">
                    <asp:ObjectDataSource ID="odsReavaliacao" runat="server" TypeName="Techne.Lyceum.Net.Patrimonio.Patrimonio"
                        SelectMethod="Lista">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdnbemId" Name="bemId" PropertyName="Value" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:Label Font-Names="Verdana" ID="lblMensagemReavaliacao" SkinID="lblMensagem"
                        runat="server" Font-Bold="true"></asp:Label>
                    <asp:Panel ID="pnlDadosReavaliacao" runat="server" Width="680px" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBemInservivel" runat="server" Text="Bem Inservível?*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblBemInservivel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblBemInservivel_SelectedIndexChanged"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlBemInservivel" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Processo*:" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPrefixoProcessoReavaliacao" runat="server">
                                                        <asp:ListItem Text="Selecione" Value="">
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="E-03/" Value="E-03/">
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="SEI-" Value="SEI-">
                                                        </asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtProcessoReavaliacao" runat="server" MaxLength="20" Width="109px"
                                                        onkeypress="return OnlyNumericEntry(event)" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlBemNaoInservivel" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label Font-Names="Verdana" ID="Label18" SkinID="lblObrigatorio" runat="server"
                                                        Text="Estado de Conservação:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEstadoConservacaoNaoInservivel" runat="server" DataTextField="conceito"
                                                        DataValueField="estadoconservacaoid">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label Font-Names="Verdana" ID="Label19" SkinID="lblObrigatorio" runat="server"
                                                        Text="Vida Adicional:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlVidaUtilNaoInservivel" runat="server" DataTextField="CONCEITO"
                                                        DataValueField="QUANTIDADEANOS">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label21" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                                        Text="Valor de Mercado:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorNaoInservivel" runat="server" Width="80px" OnKeyPress="return(moeda(this,'.',',',event))" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnSalvarReavaliacao" runat="server" Text="Salvar" OnClick="btnSalvarReavaliacao_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <dxwgv:ASPxGridView ID="grdReavaliacao" runat="server" AutoGenerateColumns="False"
                                    ClientInstanceName="grdReavaliacao" DataSourceID="odsReavaliacao" KeyFieldName="REAVALIACAOID"
                                    Width="650px">
                                    <SettingsText EmptyDataRow="Não existem dados." />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                            <HeaderCaptionTemplate>
                                                <div style="text-align: center" id="dvteste">
                                                    <input type="image" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                        title="Novo" onserverclick="HabilitaPnlReavaliacao" runat="server" />
                                                </div>
                                            </HeaderCaptionTemplate>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="REAVALIACAOID" ReadOnly="true"
                                            Visible="false" VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Inservível" FieldName="INSERVIVEL" ReadOnly="true"
                                            VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Estado Conservação" FieldName="ESTADOCONSERVACAO"
                                            ReadOnly="true" VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="VIDAADICIONAL" Caption="Vida Adicional"
                                            VisibleIndex="4">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Valor Mercado" FieldName="VALORMERCADO" VisibleIndex="3"
                                            Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Valor Mercado" FieldName="VALORMERCADOCOMSIGLA"
                                            ReadOnly="true" VisibleIndex="6">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="VALORGERADO" ReadOnly="true" VisibleIndex="7"
                                            Caption="Valor " Width="20px" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Valor Reavaliação" FieldName="VALORGERADOCOMSIGLA"
                                            VisibleIndex="8">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataDateColumn Caption="Data Reavaliação" FieldName="DATAREAVALIACAO"
                                            VisibleIndex="9" Width="250px">
                                        </dxwgv:GridViewDataDateColumn>
                                    </Columns>
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                </dxwgv:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
