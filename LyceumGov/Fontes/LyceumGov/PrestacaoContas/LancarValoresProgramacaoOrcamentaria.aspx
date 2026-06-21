<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="LancarValoresProgramacaoOrcamentaria.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.LancarValoresProgramacaoOrcamentaria" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 60px;
        }
    </style>
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
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Dados da Programação Orçamentária"
        Width="617px">
        <table style="width: 600px">
            <tr>
                <td align="left">
                    <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Programação Orçamentária:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseProgramacaoOrcamentaria" runat="server" MaxLength="22" DataType="Number"
                        Argument="NOME" OnChanged="tseProgramacaoOrcamentaria_Changed" SqlSelect="select PLANILHAORCAMENTARIAID,DESCRICAO,PROCESSO  from  PrestacaoContas.VW_PLANILHAORCAMENTARIA"
                        SqlWhere=" aprovada = 1 ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANILHAORCAMENTARIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="40%" />
                            <tweb:TSearchBoxColumn Caption="Processo" FieldName="PROCESSO" Width="40%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Parcela da Programação Orçamentária:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseItemPlanilha" runat="server" Key="ITEMPLANILHAORCAMENTARIAID"
                        Argument="ANO_MES" SqlWhere=" PLANILHAORCAMENTARIAID = #tseProgramacaoOrcamentaria# "
                        MaxLength="22" OnChanged="tseItemPlanilha_Changed" DataType="Number" SqlSelect="select PLANILHA,ANO,MES,PLANILHAORCAMENTARIAID from  PrestacaoContas.VW_ITEMPLANILHAORCAMENTARIA ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="ITEMPLANILHAORCAMENTARIAID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="PLANILHA" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ANO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Mês Referência" FieldName="MES" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td>
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:Label runat="server" ID="lblBloco" Text="Lançamentos de Repasses das Parcelas da Programação Orçamentária"
            SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlGrid" runat="server">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Text="Mês/Ano Referência:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblMesAnoReferenciaResult" SkinID="lblObrigatorio" runat="server"
                        Font-Names="Verdana" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Text="Região Financeira:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblRegiaoFinanceiraResult" SkinID="lblObrigatorio" runat="server"
                        Font-Names="Verdana" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Text="Fonte de Recursos:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblFonteRecursosResult" SkinID="lblObrigatorio" runat="server" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblNumeroProcessoTexto" runat="server" Font-Names="Verdana" Text="Num. Processo Repasse:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblNumeroProcesso" SkinID="lblObrigatorio" runat="server" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlNovo" Visible="false" runat="server" GroupingText="Informe os dados do repasse da parcela da Programação Orçamentária"
            Width="70%">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label9" runat="server" Font-Names="Verdana" Text="Num. Processo Repasse:"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtNumProcessoRepasse" runat="server" Font-Names="Verdana"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblUnidade" SkinID="lblObrigatorio" runat="server" Text="Unidade Ensino*:"></asp:Label>
                    </td>
                    <td colspan="2">
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO "
                            GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblConta" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                            Text="Conta Corrente*:"></asp:Label>
                    </td>
                    <td colspan="2">
                        <tweb:TSearchBox ID="tseContaCorrente" runat="server" Key="CONTACORRENTEID" MaxLength="9"
                            Argument="CONTA" DataType="Number" SqlSelect=" select CC.REGIONALID, CENSO, NOMEBANCO, AGENCIA from PrestacaoContas.VW_CONTACORRENTE cc  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="CONTACORRENTEID" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Conta" FieldName="CONTA" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Banco" FieldName="NOMEBANCO" Width="40%" />
                                <tweb:TSearchBoxColumn Caption="Agência" FieldName="AGENCIA" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label14" runat="server" SkinID="lblObrigatorio" Text="Valor*: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtValor" runat="server" Width="80px" OnKeyPress="return(moeda(this,'.',',',event))" />
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
                        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalva_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ObjectDataSource ID="odsItemPlanilhaOrcamentaria" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.LancarValoresProgramacaoOrcamentaria"
            SelectMethod="ListaItemPlanilhaOrcamentaria">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseItemPlanilha" DefaultValue="" Name="itemPlanilhaOrcamentariaId"
                    PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="hdnLancamentoRepasseId" runat="server" />
        <dxwgv:ASPxGridView ClientInstanceName="grdItemPlanilhaOrcamentaria" ID="grdItemPlanilhaOrcamentaria"
            runat="server" Width="100%" DataSourceID="odsItemPlanilhaOrcamentaria" KeyFieldName="ITEMPLANILHAORCAMENTARIAID"
            OnCustomButtonCallback="grdItemPlanilhaOrcamentaria_CustomButtonCallback" EnableCallBacks="false"
            OnCustomButtonInitialize="grdItemPlanilhaOrcamentaria_CustomButtonInitialize">
            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
            <SettingsCookies Enabled="false" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <asp:ImageButton ID="btnNovo" runat="server" EnableViewState="false" OnCommand="btnNovo_Command"
                                ImageUrl="~/img/bt_novo.png" AlternateText="Novo"></asp:ImageButton>
                        </div>
                    </HeaderCaptionTemplate>
                    <CancelButton Visible="true" Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarCustom" Visibility="AllDataRows"
                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Deletar" ID="btnDeletar" Visibility="AllDataRows"
                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Deletar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="LANCAMENTOREPASSEID" FieldName="LANCAMENTOREPASSEID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="WSREPASSESEFAZID" FieldName="WSREPASSESEFAZID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="ITEMPLANILHAORCAMENTARIAID" FieldName="ITEMPLANILHAORCAMENTARIAID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="CONTACORRENTEID" FieldName="CONTACORRENTEID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="1"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" VisibleIndex="2"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Banco" FieldName="BANCONOME" VisibleIndex="3"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Agência" FieldName="AGENCIA" VisibleIndex="4"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Conta Corrente" FieldName="CONTA" VisibleIndex="5"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALOR" VisibleIndex="6"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="NE" FieldName="NUMERONE" VisibleIndex="7"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="NL" FieldName="NUMERONL" VisibleIndex="8"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="PD" FieldName="NUMEROPD" VisibleIndex="9"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Número OB" FieldName="NUMEROOB" VisibleIndex="10"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="OB Lista" FieldName="NUMEROLISTAOB" VisibleIndex="11"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="" VisibleIndex="15" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Num. Precesso Repasse" FieldName="NUMEROPROCESSOREPASSE"
                    VisibleIndex="13" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Impedida" FieldName="IMPEDIDA" Name="IMPEDIDA"
                    VisibleIndex="14">
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkEnsinoReligioso" runat="server" Checked='<%# this.VerificaCheck(Eval("IMPEDIDA")) %>'
                            Enabled="false" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="ACAO" VisibleIndex="15"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Motivo Rep." FieldName="MOTIVOREPROVACAO"
                    VisibleIndex="16" Visible="true">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <asp:Label ID="Label2" runat="server" Text="Valor total lançado para repasse:"></asp:Label>
        <asp:Label ID="lblTotalLancRepasse" SkinID="lblObrigatorio" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="Label7" runat="server" Text="Valor total da parcela da Programação Orçamentária:"></asp:Label>
        <asp:Label ID="lblTotalItemPlaOrc" SkinID="lblObrigatorio" runat="server" Text=""></asp:Label>
    </asp:Panel>
    <br />
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Deseja executar a operação de exclusão do lançamento de repasse da parcela da Programação Orçamentária?">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <div align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnExcluir" runat="server" Text="Sim" OnClick="btnExcluir_Click"
                                    OnClientClick="popup.Hide(); return true;" />
                            </td>
                            <td>
                                <asp:Button ID="btnNao" runat="server" Text="Não" OnClientClick="popup.Hide();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
