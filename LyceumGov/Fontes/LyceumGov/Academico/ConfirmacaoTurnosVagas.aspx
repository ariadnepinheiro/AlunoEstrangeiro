<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConfirmacaoTurnosVagas.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ConfirmacaoTurnosVagas" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
        }
    </style>

    <script src="../Scripts/DevExpressProderj.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro">
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlAno" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlResultadoTurnos" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlResultadoVagas" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlFaixaVariacao" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPerfil" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlModSegCurso" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlSerie" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlTurno" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtFaixaInicial" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtFaixaFinal" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="rblTurnosAnalisados" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="rblVagasAnalisadas" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="tseUnidadeResponsavel" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="tseMunicipio" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="tseRegional" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="btnSalvarParcialTurnos" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnFinalizarTurnos" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grdConfTurnos" EventName="HtmlRowCreated" />
            </Triggers>
            <ContentTemplate>

                <script type="text/javascript">
                    $(document).ready(function() {
                        $("#<%= this.txtFaixaInicial.ClientID %>").numeric();
                        $("#<%= this.txtFaixaFinal.ClientID %>").numeric();

                    });
                    function BloquearCtrl() {
                        if (event.keyCode == 17)
                        { alert("Proibido utilizar o Ctrl neste campo"); }
                    }

                    function desabilitaBotaoDireito() {
                        if (event.button == 2) {
                            alert("Proibido utilizar o botao direito neste campo");
                        }
                    }

                    function onlyNumbers() {
                        if (event.keyCode < 48 || event.keyCode > 57) {

                            event.keyCode = 0;
                        };
                    }

                    function countOnes(text) {
                        var occurrences = text.split("1");

                        return occurrences.length - 1;
                    }
                    function blocTexto(campo, qtde) {
                        var quant = qtde;
                        var valor = $.trim($(campo).val());
                        var total = valor.length;

                        if (total > quant) {
                            $(campo).val(valor.substr(0, quant));
                        }
                    }
                    $(document).ready(function() {
                        $('#<%=txtFaixaInicial.ClientID %>').bind('keyup', function() { blocTexto(this, 5); });
                        $('#<%=txtFaixaFinal.ClientID %>').bind('keyup', function() { blocTexto(this, 5); });
                    });
                    function abrirPopupTurnosFinalizar() {
                        window.setTimeout(function() {
                            pucConfirmarTurnos.Show();
                        }, 1000);
                    }
                    function abrirPopupTurnosParcial() {
                        window.setTimeout(function() {
                            pucParcialTurnos.Show();
                        }, 1000);
                    }
                    function abrirPopupNovaTurma() {
                        window.setTimeout(function() {
                            pcTurmaNova.Show();
                        }, 1000);
                    }
                    function fecharPopupNovaTurma() {
                        window.setTimeout(function() {
                            pcTurmaNova.Hide();
                        }, 1000);
                    }
                    function abrirPopupNovaTurmaComDados() {
                        window.setTimeout(function() {
                            pucConfirmaTurmaNovaComDados.Show();
                        }, 1000);
                    }
                    function fecharPopupNovaTurmaComDados() {
                        window.setTimeout(function() {
                            pucConfirmaTurmaNovaComDados.Hide();
                        }, 1000);
                    }
                    function abrirPopupVagasOfertadas() {
                        window.setTimeout(function() {

                            pcVagasOfertadas.Show();
                            pcConfirmacaoValidacaoVagas.Hide();
                        }, 1000);
                    }

                    function fecharPopupVagasOfertadas() {
                        window.setTimeout(function() {
                            pcVagasOfertadas.Hide();
                        }, 1000);
                    }

                    function abrirPopupVagas() {
                        window.setTimeout(function() {
                            pcVagasDataFim.Show();
                        }, 1000);
                    }

                    function fecharPopupVagas() {
                        window.setTimeout(function() {
                            pcVagasDataFim.Hide();
                        }, 1000);
                    }

                    function abrirPopupConfirmacaoValidacaoVagas() {
                        window.setTimeout(function() {
                            pcConfirmacaoValidacaoVagas.Show();
                        }, 1000);
                    }

                    function fecharPopupConfirmacaoValidacaoVagas() {
                        window.setTimeout(function() {
                            pcConfirmacaoValidacaoVagas.Hide();
                        }, 1000);
                    }

                    function OnlyNumericEntry(e) {

                        var charCode = (e.which) ? e.which : event.keyCode
                        if (charCode > 31 && (charCode < 48 || charCode > 57))
                            return false;
                        return true;
                    }

                    function controlRelatedFieldsTurnos(field) {
                        var valorAntigo = $("#" + $(field).attr("valorAntigo")).val();
                        var valorNovo = $("#" + $(field).attr("valorNovo")).val();
                        var txtJustificativa = $("#" + $(field).attr("justificativa"));
                        var perfilResponsavel = $("#" + $(field).attr("PerfilResponsavel")).val();
                        var finalizado = $("#" + $(field).attr("finalizado")).val();
                        var encerrado = $("#" + $(field).attr("encerrado")).val();
                        var perfil = $("#<%=hdnPerfil.ClientID %>").val();
                        var chkIntegral = $("#" + $(field).attr("integral"));
                        var chkAmpliado = $("#" + $(field).attr("ampliado"));
                        var linha = field.id.split('_')[3].substring(4, field.Length);
                        var coluna = field.id.split('_')[4];
                        var nomeCheckbox = field.id.split('_')[5]; //chkIntegral ou chkAmpliado
                        var idInicial = field.id.split('_')[0] + "_" + field.id.split('_')[1] + "_" + field.id.split('_')[2] + "_" + field.id.split('_')[3].substring(0, 4);
                        var colunaNovaIntegral = null;
                        colunaNovaIntegral = parseInt(coluna) + 1;
                        var ampliadoCompleto = idInicial + linha + "_" + colunaNovaIntegral + "_chkAmpliado";
                        var colunaNovaAmpliado = null;
                        colunaNovaAmpliado = parseInt(coluna) - 1;
                        var integralCompleto = idInicial + linha + "_" + colunaNovaAmpliado + "_chkIntegral";

                        if (encerrado == 'True') {
                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").attr("disabled", true);
                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").attr("disabled", true);
                            $(txtJustificativa).attr("readonly", "readonly");
                            $(txtJustificativa).attr("disabled", true);
                            $(txtJustificativa).css("background-color", "Gainsboro");
                        }
                        else {
                            if (valorAntigo != valorNovo) {
                                if (perfil == 'SUPED' || perfil == 'SUPLAN' || perfil == 'DIESP') {

                                    if (((perfil == 'SUPED' && perfilResponsavel == 'SUPLAN') || (perfil == 'SUPLAN' && perfilResponsavel == 'SUPED'))) {

                                        $(txtJustificativa).attr("readonly", "readonly");
                                        $(txtJustificativa).attr("disabled", true);
                                        $(txtJustificativa).css("background-color", "Gainsboro");
                                    }
                                    else {
                                        $(txtJustificativa).removeAttr("readonly");
                                        $(txtJustificativa).removeAttr("disabled");
                                        $(txtJustificativa).css("background-color", "");
                                    }
                                }
                                else if (perfil == 'DIRETOR_UE') {

                                    if (finalizado == 'True') {
                                        $(txtJustificativa).attr("readonly", "readonly");
                                        $(txtJustificativa).attr("disabled", true);
                                        $(txtJustificativa).css("background-color", "Gainsboro");
                                    } else {
                                        $(txtJustificativa).removeAttr("readonly");
                                        $(txtJustificativa).removeAttr("disabled");
                                        $(txtJustificativa).css("background-color", "");
                                    }
                                }
                                else {
                                    $(txtJustificativa).removeAttr("readonly");
                                    $(txtJustificativa).removeAttr("disabled");
                                    $(txtJustificativa).css("background-color", "");
                                }
                            }
                            else {
                                $(txtJustificativa).attr("readonly", "readonly");
                                $(txtJustificativa).attr("disabled", true);
                                $(txtJustificativa).css("background-color", "Gainsboro");
                                $(txtJustificativa).val("");
                            }

                            if (perfil == 'SUPED' || perfil == 'SUPLAN' || perfil == 'DIESP') {

                                if ((perfil == 'SUPED' && perfilResponsavel == 'SUPED') || (perfil == 'SUPLAN' && perfilResponsavel == 'SUPLAN') || (perfil == 'DIESP')) {

                                    if (chkAmpliado.is(":checked")) {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").attr("disabled", true);
                                    }
                                    else {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").removeAttr("disabled", true);
                                    }

                                    if (chkIntegral.is(":checked")) {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").attr("disabled", true);
                                    }
                                    else {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").removeAttr("disabled", true);
                                    }
                                }
                                else {
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").attr("disabled", true);
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").attr("disabled", true);
                                }
                            }
                            else if (perfil == 'DIRETOR_UE') {

                                if (finalizado == 'True') {

                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").attr("disabled", true);
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").attr("disabled", true);
                                }
                                else {

                                    if (chkAmpliado.is(":checked")) {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").attr("disabled", true);
                                    }
                                    else {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").removeAttr("disabled", true);
                                    }

                                    if (chkIntegral.is(":checked")) {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").attr("disabled", true);
                                    }
                                    else {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").removeAttr("disabled", true);
                                    }
                                }
                            }
                            else if (perfil == 'privilegiado') {

                                if (chkAmpliado.is(":checked")) {
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").attr("disabled", true);
                                }
                                else {
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompleto + "']").removeAttr("disabled", true);
                                }

                                if (chkIntegral.is(":checked")) {
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").attr("disabled", true);
                                }
                                else {
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompleto + "']").removeAttr("disabled", true);
                                }
                            }
                        }
                    }

                    function controlRelatedFieldsTurnosNovo(fieldNovo) {
                        var valorAntigoNovo = $("#" + $(fieldNovo).attr("valorAntigoNovo")).val();
                        var valorNovoNovo = $("#" + $(fieldNovo).attr("valorNovoNovo")).val();
                        var txtJustificativaNovo = $("#" + $(fieldNovo).attr("justificativaNovo"));
                        var perfilResponsavelNovo = $("#" + $(fieldNovo).attr("PerfilResponsavel")).val();
                        var finalizadoNovo = $("#" + $(fieldNovo).attr("finalizado")).val();
                        var encerradoNovo = $("#" + $(fieldNovo).attr("encerrado")).val();
                        var perfilNovo = $("#<%=hdnPerfil.ClientID %>").val();
                        var linhaNovo = fieldNovo.id.split('_')[3].substring(4, fieldNovo.Length);
                        var colunaNovo = fieldNovo.id.split('_')[4];
                        var nomeCheckboxNovo = fieldNovo.id.split('_')[5]; //chkIntegralNovo ou chkAmpliadoNovo
                        var idInicialNovo = fieldNovo.id.split('_')[0] + "_" + fieldNovo.id.split('_')[1] + "_" + fieldNovo.id.split('_')[2] + "_" + fieldNovo.id.split('_')[3].substring(0, 4);
                        var colunaNovaIntegralNovo = null;
                        colunaNovaIntegralNovo = parseInt(colunaNovo) + 1;
                        var ampliadoCompletoNovo = idInicialNovo + linhaNovo + "_" + colunaNovaIntegralNovo + "_chkAmpliadoNovo";
                        var colunaNovaAmpliado = null;
                        colunaNovaAmpliadoNovo = parseInt(colunaNovo) - 1;
                        var integralCompletoNovo = idInicialNovo + linhaNovo + "_" + colunaNovaAmpliadoNovo + "_chkIntegralNovo";

                        if (encerradoNovo == 'True') {
                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").attr("disabled", true);
                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").attr("disabled", true);
                            $(txtJustificativaNovo).attr("readonly", "readonly");
                            $(txtJustificativaNovo).attr("disabled", true);
                            $(txtJustificativaNovo).css("background-color", "Gainsboro");
                        }
                        else {
                            if (valorAntigoNovo != valorNovoNovo) {
                                if (perfilNovo == 'SUPED' || perfilNovo == 'SUPLAN' || perfilNovo == 'DIESP') {

                                    if (((perfilNovo == 'SUPED' && perfilResponsavelNovo == 'SUPLAN') || (perfilNovo == 'SUPLAN' && perfilResponsavelNovo == 'SUPED'))) {

                                        $(txtJustificativaNovo).attr("readonly", "readonly");
                                        $(txtJustificativaNovo).attr("disabled", true);
                                        $(txtJustificativaNovo).css("background-color", "Gainsboro");
                                    }
                                    else {
                                        $(txtJustificativaNovo).removeAttr("readonly");
                                        $(txtJustificativaNovo).removeAttr("disabled");
                                        $(txtJustificativaNovo).css("background-color", "");
                                    }
                                } else if (perfilNovo == 'DIRETOR_UE') {
                                    if (finalizadoNovo == 'True') {
                                        $(txtJustificativaNovo).attr("readonly", "readonly");
                                        $(txtJustificativaNovo).attr("disabled", true);
                                        $(txtJustificativaNovo).css("background-color", "Gainsboro");
                                    } else {
                                        $(txtJustificativaNovo).removeAttr("readonly");
                                        $(txtJustificativaNovo).removeAttr("disabled");
                                        $(txtJustificativaNovo).css("background-color", "");
                                    }
                                }
                                else {
                                    $(txtJustificativaNovo).removeAttr("readonly");
                                    $(txtJustificativaNovo).removeAttr("disabled");
                                    $(txtJustificativaNovo).css("background-color", "");
                                }
                            }
                            else {
                                $(txtJustificativaNovo).attr("readonly", "readonly");
                                $(txtJustificativaNovo).attr("disabled", true);
                                $(txtJustificativaNovo).css("background-color", "Gainsboro");
                                $(txtJustificativaNovo).val("");
                            }

                            if (perfilNovo == 'SUPED' || perfilNovo == 'SUPLAN' || perfilNovo == 'DIESP') {
                                if ((perfilNovo == 'SUPED' && perfilResponsavelNovo == 'SUPED') || (perfilNovo == 'SUPLAN' && perfilResponsavelNovo == 'SUPLAN') || (perfilNovo == 'DIESP')) {
                                    if (fieldNovo.id.split('_')[5] == "chkAmpliadoNovo") {
                                        if ($(fieldNovo).is(":checked")) {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").attr("disabled", true);
                                        }
                                        else {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").removeAttr("disabled", true);
                                        }
                                    }
                                    else if (fieldNovo.id.split('_')[5] == "chkIntegralNovo") {
                                        if ($(fieldNovo).is(":checked")) {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").attr("disabled", true);
                                        }
                                        else {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").removeAttr("disabled", true);
                                        }
                                    }
                                }
                                else {
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").attr("disabled", true);
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").attr("disabled", true);
                                }
                            }
                            else if (perfilNovo == 'DIRETOR_UE') {

                                if (finalizadoNovo == 'True') {
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").attr("disabled", true);
                                    $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").attr("disabled", true);
                                }
                                else {
                                    if (fieldNovo.id.split('_')[5] == "chkAmpliadoNovo") {
                                        if ($(fieldNovo).is(":checked")) {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").attr("disabled", true);
                                        }
                                        else {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").removeAttr("disabled", true);
                                        }
                                    }
                                    else if (fieldNovo.id.split('_')[5] == "chkIntegralNovo") {
                                        if ($(fieldNovo).is(":checked")) {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").attr("disabled", true);
                                        }
                                        else {
                                            $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").removeAttr("disabled", true);
                                        }
                                    }
                                }
                            }

                            else if (perfilNovo == 'privilegiado') {
                                if (fieldNovo.id.split('_')[5] == "chkAmpliadoNovo") {
                                    if ($(fieldNovo).is(":checked")) {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").attr("disabled", true);
                                    }
                                    else {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + integralCompletoNovo + "']").removeAttr("disabled", true);
                                    }
                                }
                                else if (fieldNovo.id.split('_')[5] == "chkIntegralNovo") {
                                    if ($(fieldNovo).is(":checked")) {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").attr("disabled", true);
                                    }
                                    else {
                                        $("#<%=grdConfTurnos.ClientID %> input[id$='" + ampliadoCompletoNovo + "']").removeAttr("disabled", true);
                                    }
                                }
                            }
                        }
                    }

                    function changeOptions(position, fieldcO) {
                        var perfilcO = $("#<%=hdnPerfil.ClientID %>").val();
                        var isChecked = $(fieldcO).is(':checked');
                        var hdnValorNovo = $("#" + $(fieldcO).attr("valorNovo"));
                        var valorAntigocO = $("#" + $(fieldcO).attr("valorAntigo")).val();
                        var valorNovocO = $(hdnValorNovo).val();

                        valorNovocO = valorNovocO.substring(0, position) + (isChecked ? "1" : "0") + valorNovocO.substring(position + 1);

                        if (isChecked) {
                            var quantidadeAntiga = countOnes(valorAntigocO);
                            var quantidadeNova = countOnes(valorNovocO);
                        }

                        hdnValorNovo.val(valorNovocO);
                    }


                    function changeOptionsTurnosNovo(position, fieldNovo) {

                        var perfilcOn = $("#<%=hdnPerfil.ClientID %>").val();
                        var isCheckedcOn = $(fieldNovo).is(':checked');
                        var hdnValorNovocOn = $("#" + $(fieldNovo).attr("valorNovoNovo"));
                        var valorAntigocOn = $("#" + $(fieldNovo).attr("valorAntigoNovo")).val();
                        var valorNovocOn = $(hdnValorNovocOn).val();

                        valorNovocOn = valorNovocOn.substring(0, position) + (isCheckedcOn ? "1" : "0") + valorNovocOn.substring(position + 1);

                        if (isCheckedcOn) {
                            var quantidadeAntiga = countOnes(valorAntigocOn);
                            var quantidadeNova = countOnes(valorNovocOn);
                        }

                        hdnValorNovocOn.val(valorNovocOn);
                    }

                    $(document).ready(function() {

                        $("input[id*='chkManhaNovo']").click(function() {
                            changeOptionsTurnosNovo(0, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkTardeNovo']").click(function() {
                            changeOptionsTurnosNovo(1, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkNoiteNovo']").click(function() {
                            changeOptionsTurnosNovo(2, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkIntegralNovo']").click(function() {
                            changeOptionsTurnosNovo(3, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkAmpliadoNovo']").click(function() {
                            changeOptionsTurnosNovo(4, this);
                            controlRelatedFieldsTurnosNovo(this);

                        });
                        $("input[id*='chkManha']").click(function() {
                            changeOptions(0, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkTarde']").click(function() {
                            changeOptions(1, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkNoite']").click(function() {
                            changeOptions(2, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkIntegral']").click(function() {
                            changeOptions(3, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkAmpliado']").click(function() {
                            changeOptions(4, this);
                            controlRelatedFieldsTurnos(this);

                        });

                        $("input[id*='chk']").each(function() {
                            controlRelatedFieldsTurnos(this);
                            controlRelatedFieldsTurnosNovo(this);
                        });
                    });

                    var prm = Sys.WebForms.PageRequestManager.getInstance();

                    prm.add_endRequest(function() {
                        $("input[id*='chkManhaNovo']").click(function() {
                            changeOptionsTurnosNovo(0, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkTardeNovo']").click(function() {
                            changeOptionsTurnosNovo(1, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkNoiteNovo']").click(function() {
                            changeOptionsTurnosNovo(2, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkIntegralNovo']").click(function() {
                            changeOptionsTurnosNovo(3, this);
                            controlRelatedFieldsTurnosNovo(this);
                        });

                        $("input[id*='chkAmpliadoNovo']").click(function() {
                            changeOptionsTurnosNovo(4, this);
                            controlRelatedFieldsTurnosNovo(this);

                        });
                        $("input[id*='chkManha']").click(function() {
                            changeOptions(0, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkTarde']").click(function() {
                            changeOptions(1, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkNoite']").click(function() {
                            changeOptions(2, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkIntegral']").click(function() {
                            changeOptions(3, this);
                            controlRelatedFieldsTurnos(this);
                        });

                        $("input[id*='chkAmpliado']").click(function() {
                            changeOptions(4, this);
                            controlRelatedFieldsTurnos(this);

                        });

                        $("input[id*='chk']").each(function() {
                            controlRelatedFieldsTurnos(this);
                            controlRelatedFieldsTurnosNovo(this);
                        });
                    });

                    function informaSalvarVagasParcial() {

                        alert("Para evitar a perda de informações, a configuração atual foi salva parcialmente com sucesso.");
                        return true;
                    }

                    /*
                    DevExpress Workaround:
                    https://www.devexpress.com/Support/Center/Question/Details/T555320/a-popup-is-not-shown-or-is-shown-at-an-incorrect-position-in-chrome-61-and-newer-versions
                    A popup is not shown or is shown at an incorrect position in Chrome 61 and newer versions
                    
                    Nas versões recentes do Chrome, o popup não estava aparecendo centralizado na tela visível do usuário. Ao invés disso, estava aparecendo centralizado como
                    se o scrollTop estivesse zerado.
                    
                    As funções abaixo corrigem o scrollTop para o que o componente do DevExpress realmente espera receber.
                    */
                    window.onload = function() {
                    

                        function _aspxGetDocumentScrollTop() {
                            return document.documentElement.scrollTop || document.body.scrollTop
                        }
                        if (window._aspxGetDocumentScrollTop) {
                            window._aspxGetDocumentScrollTop = _aspxGetDocumentScrollTop;
                        }
                        /* Begin -> Correct ScrollLeft */
                        function _aspxGetDocumentScrollLeft() {
                            return document.documentElement.scrollLeft || document.body.scrollLeft
                        }
                        if (window._aspxGetDocumentScrollLeft) {
                            window._aspxGetDocumentScrollLeft = _aspxGetDocumentScrollLeft;
                        }
                        /* End -> Correct ScrollLeft */
                    }
                    
                </script>

                <asp:Panel runat="server" ID="pnFiltro" GroupingText="Informe os dados para pesquisar a confirmação de turnos e vagas">
                    <div>
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                                        Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                                        AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="pnlAnalise" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label6" runat="server" Text="Perfil:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlPerfil" runat="server" DataTextField="DESCRICAO" DataValueField="PERFILID"
                                                        Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPerfil_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label5" runat="server" Text="Turnos Analisados:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblTurnosAnalisados" runat="server" AutoPostBack="true"
                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="rblTurnosAnalisados_SelectedIndexChanged">
                                                        <asp:ListItem Text="Não" Value="Nao" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                                                        <asp:ListItem Text="Todos" Value="Todos"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblFiltroResultadoTurnos" Visible="false" runat="server" Text="Resultado:"></asp:Label>
                                                    <asp:DropDownList ID="ddlResultadoTurnos" Visible="false" runat="server" AppendDataBoundItems="True"
                                                        AutoPostBack="true" DataValueField="Descricao" DataTextField="Descricao" OnSelectedIndexChanged="ddlResultadoTurnos_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label7" runat="server" Text="Vagas Analisadas:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblVagasAnalisadas" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="rblVagasAnalisadas_SelectedIndexChanged">
                                                        <asp:ListItem Text="Não" Value="Nao" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                                                        <asp:ListItem Text="Todos" Value="Todos"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblFiltroResultadoVagas" Visible="false" runat="server" Text="Resultado:"></asp:Label>
                                                    <asp:DropDownList ID="ddlResultadoVagas" Visible="false" runat="server" AppendDataBoundItems="True"
                                                        AutoPostBack="true" DataValueField="Descricao" DataTextField="Descricao" OnSelectedIndexChanged="ddlResultadoVagas_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:Panel ID="pnlFaixaVariacao" Visible="false" runat="server">
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblFaixaVariacao" runat="server" Text="Faixa de Variação de Vagas:"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        De<asp:TextBox ID="txtFaixaInicial" runat="server" Text="" MaxLength="5" onkeypress="return OnlyNumericEntry(event)"
                                                            OnTextChanged="txtFaixaInicial_TextChanged" AutoPostBack="true"></asp:TextBox>%
                                                        até
                                                        <asp:TextBox ID="txtFaixaFinal" runat="server" Text="" MaxLength="5" onkeypress="return OnlyNumericEntry(event)"
                                                            OnTextChanged="txtFaixaFinal_TextChanged" AutoPostBack="true"></asp:TextBox>%
                                                        <asp:DropDownList ID="ddlFaixaVariacao" runat="server" AppendDataBoundItems="True"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlFaixaVariacao_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Text="Para Mais ou Para Menos" Value="Ambos"></asp:ListItem>
                                                            <asp:ListItem Text="Para Mais" Value="Para Mais"></asp:ListItem>
                                                            <asp:ListItem Text="Para Menos" Value="Para Menos"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label8" runat="server" Text="Modalidade/Segmento/Curso:"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <dxe:ASPxComboBox ID="ddlModSegCurso" runat="server" AutoPostBack="true" DataSourceID="odsModalidade"
                                                        ValueType="System.String" TextFormatString="{0} | {1} | {3}" Width="480px" ValueField="CURSO"
                                                        DropDownWidth="900px" ClientInstanceName="ddlModSegCurso" OnSelectedIndexChanged="ddlModSegCurso_SelectedIndexChanged"
                                                        EnableIncrementalFiltering="true">
                                                        <Columns>
                                                            <dxe:ListBoxColumn Caption="MODALIDADE" FieldName="MODALIDADE" Width="25%" />
                                                            <dxe:ListBoxColumn Caption="NIVEL" FieldName="SEGMENTO" Width="25%" />
                                                            <dxe:ListBoxColumn Caption="CODIGO CURSO" FieldName="CURSO" Width="10%" />
                                                            <dxe:ListBoxColumn Caption="CURSO" FieldName="NOME_CURSO" Width="40%" />
                                                        </Columns>
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label9" runat="server" Text="Série/Ano Escolar:"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" DataValueField="serie"
                                                        AutoPostBack="True" AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="Label10" runat="server" Text="Turno:"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlTurno" runat="server" DataTextField="DESCRICAO" DataValueField="TURNO"
                                                        AutoPostBack="True" AppendDataBoundItems="true" Enabled="false" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; ">
                                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                                        SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                        DataType="Number">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; ">
                                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="600px"
                                        ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; ">
                                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearch ID="tseUnidadeResponsavel" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryUnidadeEnsinoTurnosEVagas" 
                                    AutoPostBack="true" OnTextChanged="tseUnidadeResponsavel_Changed">
                                     <QueryParameters>
                                            <asp:ControlParameter Name="PeriodoAnalise" ControlID="pnlAnalise" PropertyName="Visible" />
                                            <asp:ControlParameter Name="Ano" ControlID="ddlAno" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="Perfil" ControlID="ddlPerfil" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="TurnosAnalisados" ControlID="rblTurnosAnalisados" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="ResultadoTurnos" ControlID="ddlResultadoTurnos" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="VagasAnalisadas" ControlID="rblVagasAnalisadas" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="ResultadoVagas" ControlID="ddlResultadoVagas" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="FaixaInicial" ControlID="txtFaixaInicial" PropertyName="Text" />
                                            <asp:ControlParameter Name="FaixaFinal" ControlID="txtFaixaFinal" PropertyName="Text" />
                                            <asp:ControlParameter Name="FaixaVariacao" ControlID="ddlFaixaVariacao" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="ModSegCurso" ControlID="ddlModSegCurso" PropertyName="Value" />
                                            <asp:ControlParameter Name="Serie" ControlID="ddlSerie" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="Turno" ControlID="ddlTurno" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="Regional" ControlID="tseRegional" PropertyName="DBValue" />
                                            <asp:ControlParameter Name="Municipio" ControlID="tseMunicipio" PropertyName="DBValue" />
                                            <asp:ControlParameter Name="PerfilUsuarioLogado" ControlID="hdnPerfil" PropertyName="Value" /> 
                                        </QueryParameters>
                                    </tweb:TSearch>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                                        ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                                        ValidationGroup="ConfirmarForm">
                            <img alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" />
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <br />
                <div style="font-family: arial; color: red; font-size: 12px; font-weight: bold;"
                    id="divLinkHistorico" runat="server" visible="false">
                    Já existe histórico de lançamento para esta unidade, para visualizá-lo
                    <asp:LinkButton ID="hplLink" Font-Size="12px" Font-Bold="true" OnClick="hplLink_Click"
                        runat="server">clique aqui</asp:LinkButton>
                </div>
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                <br />
                <asp:Panel runat="server" ID="pnTurnos" Visible="false" GroupingText="Confirmação de Turnos">
                    <br />
                    <asp:Label ID="lblMensagemFinalizarTurno" runat="server" SkinID="lblMensagem"></asp:Label>
                    <br />
                     <br />
                    <asp:Label ID="lblInconsistenciaSala" runat="server" SkinID="lblMensagem"></asp:Label>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:HiddenField runat="server" ID="hdnPerfil" />
                                <asp:Panel ID="pnGridTurnos" runat="server">
                                    <dxwgv:ASPxGridView ID="grdConfTurnos" runat="server" AutoGenerateColumns="False"
                                        Visible="False" ClientInstanceName="grdConfTurnos" DataSourceID="odsConfirmacaoTurnos"
                                        KeyFieldName="IdAgendaConfTurnoVaga" OnHtmlRowCreated="grdConfTurnos_HtmlRowCreated"
                                        SettingsBehavior-AllowDragDrop="false" SettingsBehavior-AllowSort="False">
                                        <SettingsPager Mode="ShowAllRecords" />
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="IdAgendaConfTurnoVaga"
                                                VisibleIndex="0" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="AgendaId" FieldName="AgendaId" VisibleIndex="1"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="Periodo" VisibleIndex="2"
                                                ReadOnly="true" Width="50">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Manhã Código" FieldName="ManhaCodigo" VisibleIndex="3"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Tarde Código" FieldName="TardeCodigo" VisibleIndex="4"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Noite Código" FieldName="NoiteCodigo" VisibleIndex="5"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Integral Código" FieldName="IntegralCodigo"
                                                VisibleIndex="6" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Ampliado Código" FieldName="AmpliadoCodigo"
                                                VisibleIndex="7" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Manhã Código" FieldName="ManhaNovoCodigo"
                                                VisibleIndex="8" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Tarde Código" FieldName="TardeNovoCodigo"
                                                VisibleIndex="9" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Noite Código" FieldName="NoiteNovoCodigo"
                                                VisibleIndex="10" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Integral Código" FieldName="IntegralNovoCodigo"
                                                VisibleIndex="11" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Ampliado Código" FieldName="AmpliadoNovoCodigo"
                                                VisibleIndex="12" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="CodModalidade" FieldName="CodigoModalidade"
                                                VisibleIndex="13" Visible="false" ReadOnly="true" Width="200">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="CodTipo" FieldName="CodigoTipo" VisibleIndex="14"
                                                Visible="false" ReadOnly="true" Width="200">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="Modalidade" VisibleIndex="15"
                                                Visible="true" ReadOnly="true" Width="200">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="Curso" VisibleIndex="16"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="NomeCurso" VisibleIndex="17"
                                                Visible="true" ReadOnly="true" Width="200">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="Serie" VisibleIndex="18"
                                                ReadOnly="true" Width="50">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="DescricaoSerie" VisibleIndex="19"
                                                ReadOnly="true" Width="50" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataColumn Caption="Manhã" FieldName="Manha" VisibleIndex="20" Name="Manha"
                                                Width="50">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkManha" runat="server" Checked='<%# Bind("Manha") %>'></asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Tarde" FieldName="Tarde" VisibleIndex="21" Name="Tarde"
                                                Width="50">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkTarde" runat="server" Checked='<%# Bind("Tarde") %>'></asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Noite" FieldName="Noite" VisibleIndex="22" Name="Noite"
                                                Width="50">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkNoite" runat="server" Checked='<%# Bind("Noite") %>'></asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Integral" FieldName="Integral" VisibleIndex="23"
                                                Name="Integral" Width="50">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkIntegral" runat="server" Checked='<%# Bind("Integral") %>'>
                                                    </asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Ampliado" FieldName="Ampliado" VisibleIndex="24"
                                                Name="Ampliado" Width="50">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkAmpliado" runat="server" Checked='<%# Bind("Ampliado")  %>'>
                                                    </asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Justificativa" FieldName="Justificativa" VisibleIndex="25"
                                                Name="Justificativa" Width="200">
                                                <DataItemTemplate>
                                                    <asp:TextBox ID="txtJustificativa" runat="server" Width="200" Text='<%# Bind("Justificativa") %>'></asp:TextBox>
                                                    <asp:HiddenField ID="hdnValorAntigo" runat="server" Value='<%# Bind("TurnosIniciais") %>' />
                                                    <asp:HiddenField ID="hdnValorNovo" runat="server" Value='<%# Bind("Turnos") %>' />
                                                    <asp:HiddenField ID="hdnPerfilResponsavel" runat="server" Value='<%# Bind("PerfilResponsavel") %>' />
                                                    <asp:HiddenField ID="hdnFinalizado" runat="server" Value='<%# Bind("Finalizado") %>' />
                                                    <asp:HiddenField ID="hdnEncerrado" runat="server" Value='<%# Bind("Encerrado") %>' />
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Manhã" FieldName="ManhaNovo" VisibleIndex="26"
                                                Name="ManhaNovo">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkManhaNovo" runat="server" Checked='<%# Bind("ManhaNovo") %>'>
                                                    </asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Tarde" FieldName="TardeNovo" VisibleIndex="27"
                                                Name="TardeNovo">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkTardeNovo" runat="server" Checked='<%# Bind("TardeNovo") %>'>
                                                    </asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Noite" FieldName="NoiteNovo" VisibleIndex="28"
                                                Name="NoiteNovo">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkNoiteNovo" runat="server" Checked='<%# Bind("NoiteNovo") %>'>
                                                    </asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Integral" FieldName="IntegralNovo" VisibleIndex="29"
                                                Name="IntegralNovo">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkIntegralNovo" runat="server" Checked='<%# Bind("IntegralNovo") %>'>
                                                    </asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Ampliado" FieldName="AmpliadoNovo" VisibleIndex="30"
                                                Name="AmpliadoNovo">
                                                <DataItemTemplate>
                                                    <asp:CheckBox ID="chkAmpliadoNovo" runat="server" Checked='<%# Bind("AmpliadoNovo") %>'>
                                                    </asp:CheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Justificativa" FieldName="JustificativaNovo" VisibleIndex="31"
                                                Name="JustificativaNovo" Width="200">
                                                <DataItemTemplate>
                                                    <asp:TextBox ID="txtJustificativaNovo" runat="server" Width="200" Text='<%# Bind("JustificativaNovo") %>'></asp:TextBox>
                                                    <asp:HiddenField ID="hdnValorAntigoNovo" runat="server" Value='<%# Bind("TurnosIniciais") %>' />
                                                    <asp:HiddenField ID="hdnValorNovoNovo" runat="server" Value='<%# Bind("TurnosNovo") %>' />
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Perfil" FieldName="PerfilResponsavel" Name="PerfilResponsavel"
                                                VisibleIndex="32" UnboundType="String" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Finalizado" FieldName="Finalizado" Name="Finalizado"
                                                VisibleIndex="33" UnboundType="String" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Encerrado" FieldName="Encerrado" Name="Encerrado"
                                                VisibleIndex="34" UnboundType="String" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="TurnosListaInicial" FieldName="TurnosListaInicial"
                                                VisibleIndex="35" Name="TurnosListaInicial" UnboundType="String" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                        </Columns>
                                        <Templates>
                                            <TitlePanel>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td colspan="3">
                                                            &nbsp; &nbsp; &nbsp;
                                                        </td>
                                                        <td colspan="6" align="center">
                                                            Turnos Matriculas Continuidade
                                                        </td>
                                                        <td colspan="6" align="center">
                                                            Turnos Matriculas Novas
                                                        </td>
                                                    </tr>
                                                </table>
                                            </TitlePanel>
                                        </Templates>
                                        <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                                    </dxwgv:ASPxGridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel ID="pnlAnaliseTurnos" runat="server" Visible="false">
                        <table id="tbAnalise" runat="server">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="label50" Text="Análise Turnos SUPED - Validação:" SkinID="lblobrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblAnaliseTurnosGeralSUPED" Visible="false"> </asp:Label>
                                    <asp:Label runat="server" ID="lblAnaliseTurnosSUPED0" Visible="false" Text="Periodo: 0"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPEDPeriodo0" Visible="false"> </asp:Label>
                                    <asp:DropDownList ID="ddlResultadoTurnosSUPEDPeriodo0" Visible="false" runat="server"
                                        DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:Label runat="server" ID="lblAnaliseTurnosSUPED1" Visible="false" Text="Periodo: 1"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPEDPeriodo1" Visible="false"> </asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblAnaliseTurnosSUPED2" Text="Periodo: 2" Visible="false"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPEDPeriodo2" Visible="false"> </asp:Label>
                                    <asp:DropDownList ID="ddlResultadoTurnosSUPEDPeriodo2" Visible="false" runat="server"
                                        DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <br />
                                    <asp:Label runat="server" ID="label51" Text="Análise Turnos SUPLAN - Validação:"
                                        SkinID="lblobrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblAnaliseTurnosGeralSUPLAN" Visible="false"> </asp:Label>
                                    <asp:Label runat="server" ID="lblAnaliseTurnosSUPLAN0" Visible="false" Text="Periodo: 0"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPLANPeriodo0" Visible="false"> </asp:Label>
                                    <asp:DropDownList ID="ddlResultadoTurnosSUPLANPeriodo0" Visible="false" runat="server"
                                        DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:Label runat="server" ID="lblAnaliseTurnosSUPLAN1" Visible="false" Text="Periodo: 1"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPLANPeriodo1" Visible="false"> </asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblAnaliseTurnosSUPLAN2" Text="Periodo: 2" Visible="false"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPLANPeriodo2" Visible="false"> </asp:Label>
                                    <asp:DropDownList ID="ddlResultadoTurnosSUPLANPeriodo2" Visible="false" runat="server"
                                        DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <br />
                                    <asp:Label runat="server" ID="label52" Text="Análise Turnos DIESP - Validação:" SkinID="lblobrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblAnaliseTurnosGeralDIESP" Visible="false"> </asp:Label>
                                    <asp:Label runat="server" ID="lblAnaliseTurnosDIESP0" Visible="false" Text="Periodo: 0"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosDIESPPeriodo0" Visible="false"> </asp:Label>
                                    <asp:DropDownList ID="ddlResultadoTurnosDIESPPeriodo0" Visible="false" runat="server"
                                        DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:Label runat="server" ID="lblAnaliseTurnosDIESP1" Visible="false" Text="Periodo: 1"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosDIESPPeriodo1" Visible="false"> </asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblAnaliseTurnosDIESP2" Text="Periodo: 2" Visible="false"> </asp:Label>
                                    <asp:Label runat="server" ID="lblDescAnaliseTurnosDIESPPeriodo2" Visible="false"> </asp:Label>
                                    <asp:DropDownList ID="ddlResultadoTurnosDIESPPeriodo2" Visible="false" runat="server"
                                        DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Button ID="btnSalvarAnaliseTurnos" runat="server" OnClick="btnSalvarAnaliseTurnos_Click"
                        Text="Salvar Análise Turnos" Visible="false" />
                    <asp:Button ID="btnSalvarParcialTurnos" runat="server" OnClick="btnAbrirPopupTurnosParcial_Click"
                        Text="Salvar Parcialmente Turnos" Visible="false" />
                    <asp:Button ID="btnFinalizarTurnos" runat="server" OnClick="btnAbrirPopupTurnosFinalizar_Click"
                        Text="Finalizar Turnos" Visible="false" />
                    <asp:HiddenField ID="hdnPodeAnalisarTurno" runat="server" />
                </asp:Panel>
                <br />
                <asp:Label ID="lblMensagemVagas" runat="server" SkinID="lblMensagem"></asp:Label>
                <br />
                <asp:UpdatePanel ID="updatePanel7" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gridVagas" />
                        <asp:AsyncPostBackTrigger ControlID="gridSalas" />
                        <asp:AsyncPostBackTrigger ControlID="btnSalvarParcialVagas" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSalvarDefinitivoVagas" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnGridVagas" GroupingText="Confirmação de Vagas" Visible="false">
                            <br />
                            <asp:Label ID="lblMensagemFinalizarVagas" runat="server" SkinID="lblMensagem"></asp:Label>
                            <br />
                            <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gridSalas" />
                                </Triggers>
                                <ContentTemplate>
                                    <dxwgv:ASPxGridView ID="gridVagas" Width="90%" runat="server" AutoGenerateColumns="False"
                                        ClientInstanceName="gridVagas" KeyFieldName="ID" DataSourceID="odsConfVagas"
                                        SettingsPager-PageSize="200" OnHtmlRowCreated="gridVagas_onHtmlRowCreated" SettingsBehavior-AllowDragDrop="false"
                                        SettingsBehavior-AllowSort="False">
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="1" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="3%" Caption="Código" FieldName="IDAgenda" VisibleIndex="2"
                                                Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="3%" Caption="AgendaID Evento" FieldName="AgendaID"
                                                VisibleIndex="3" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="3%" Caption="SerieEntrada" FieldName="SerieEntrada"
                                                VisibleIndex="4" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="3%" Caption="Editavel" FieldName="Editavel"
                                                VisibleIndex="4" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="3%" Caption="Finalizado" FieldName="Finalizado"
                                                VisibleIndex="5" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="3%" Caption="Encerrado" FieldName="Encerrado"
                                                VisibleIndex="6" Visible="false">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="3%" Caption="Período" FieldName="Periodo" VisibleIndex="7"
                                                Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Width="8%" Caption="Modalidade" FieldName="Modalidade"
                                                VisibleIndex="8" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Curso" Width="5%" FieldName="Curso" VisibleIndex="9"
                                                Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Curso" Width="5%" FieldName="NomeCurso" VisibleIndex="10"
                                                Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Série" Width="3%" FieldName="Serie" VisibleIndex="11"
                                                Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Série" Width="4%" FieldName="DescricaoSerie"
                                                VisibleIndex="12" Visible="false" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantContSeeduc"
                                                VisibleIndex="13" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Nova" Width="4%" FieldName="QuantNovaSeeduc"
                                                VisibleIndex="14" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantCont" VisibleIndex="15"
                                                Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Nova" Width="3%" FieldName="QuantNovas" VisibleIndex="16"
                                                Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Taxa de Aprovação" Width="5%" FieldName="TaxaAprovacao"
                                                VisibleIndex="17" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Taxa de Reprovação" Width="5%" FieldName="TaxaReprovacao"
                                                VisibleIndex="18" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Justificativa Nova" Width="30%" FieldName="JustificativaNova"
                                                VisibleIndex="19" Visible="true" ReadOnly="false">
                                                <DataItemTemplate>
                                                    <asp:TextBox ID="txtJFNova" runat="server" Width="95%" Text='<%# Bind("JustificativaNova") %>'
                                                        OnTextChanged="txtJFNova_OnTextChanged" MaxLength="500"></asp:TextBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantContVagasUtilizadas"
                                                VisibleIndex="20" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Nova" Width="4%" FieldName="QuantNovasVagasUtilizadas"
                                                VisibleIndex="21" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantContSaldo"
                                                VisibleIndex="22" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Nova" Width="4%" FieldName="QuantNovaSaldo"
                                                VisibleIndex="23" Visible="true" ReadOnly="true">
                                            </dxwgv:GridViewDataTextColumn>
                                        </Columns>
                                        <Templates>
                                            <TitlePanel>
                                                <asp:Label ID="lb1" runat="server" Width="22%"></asp:Label>
                                                <%-- <asp:Label ID="lb2" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                                                    BorderColor="#6ca6ea" Width="17%">&nbsp;&nbsp;&nbsp;&nbsp;Proposta SEEDUC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Proposta U.E.</asp:Label>--%>
                                                <asp:Label ID="lb2" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                                                    BorderColor="#6ca6ea" Width="9%">Proposta SEEDUC</asp:Label>
                                                <asp:Label ID="lb3" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                                                    BorderColor="#6ca6ea" Width="7%">Proposta U.E.</asp:Label>
                                                <asp:Label ID="lb4" runat="server" Width="41%"></asp:Label>
                                                <%-- <asp:Label ID="lb5" runat="server" BorderStyle="Solid" BorderColor="#6ca6ea" Width="18%">&nbsp;&nbsp;&nbsp;&nbsp;Vagas Utilizadas&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Saldo de Vagas</asp:Label>--%>
                                                <asp:Label ID="lb5" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                                                    BorderColor="#6ca6ea" Width="8%">Vagas Utilizadas</asp:Label>
                                                <asp:Label ID="lb6" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                                                    BorderColor="#6ca6ea" Width="8%">Saldo de Vagas</asp:Label>
                                            </TitlePanel>
                                        </Templates>
                                        <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                                    </dxwgv:ASPxGridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Panel runat="server" ID="pnTurmas" BorderWidth="0">
                                <div class="mensagem">
                                    <strong style="font-size: 12px; color: #0353AB;">Para instruções:</strong>
                                    <asp:HyperLink ID="hpLeiaMais" Font-Size="12px" runat="server" Target="_blank" Text="Leia Mais."
                                        NavigateUrl="http://docenteonline.educacao.rj.gov.br/Arquivos/SAIBA%20MAIS.pdf"></asp:HyperLink>
                                    <br />
                                    <div id="msg" runat="server" style="font-weight: bold; font-size: 12px; color: #0353AB;">
                                        &nbsp;</div>
                                </div>
                                <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <br />
                                        <dxwgv:ASPxGridView ID="gridSalas" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridSalas"
                                            OnHtmlRowCreated="gridSalas_onHtmlRowCreated" Width="100%" SettingsPager-PageSize="200">
                                            <Columns>
                                                <dxwgv:GridViewDataTextColumn Caption="Salas de Aula / Capacidade" Width="12%" FieldName="SalaCapacidade"
                                                    VisibleIndex="0" Visible="true" ReadOnly="true">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataColumn Caption="Manhã" Name="Manha" VisibleIndex="1">
                                                    <DataItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hdnIDManha" runat="server" />
                                                                    <asp:HiddenField ID="hdnEditavelManha" runat="server" />
                                                                    <asp:HiddenField ID="hdnTurmaFilhaManha" runat="server" />
                                                                    <asp:Label ID="lblTurma_M" runat="server" Text="Turma" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVCM_M" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVN_M" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTurmas_M" OnDataBound="ddlTurmas_OnDataBound" AutoPostBack="true"
                                                                        DataValueField="Turma" DataTextField="DescricaoTurma" runat="server" OnSelectedIndexChanged="ddlTurmas_SelectedIndexChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVC_M" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVC_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVN_M" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVN_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DataItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataColumn Caption="Tarde" Name="Tarde" VisibleIndex="2">
                                                    <DataItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hdnIDTarde" runat="server" />
                                                                    <asp:HiddenField ID="hdnEditavelTarde" runat="server" />
                                                                    <asp:HiddenField ID="hdnTurmaFilhaTarde" runat="server" />
                                                                    <asp:Label ID="lblTurma_T" runat="server" Text="Turma" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVCM_T" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVN_T" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTurmas_T" OnDataBound="ddlTurmas_OnDataBound" AutoPostBack="true"
                                                                        DataValueField="Turma" DataTextField="DescricaoTurma" runat="server" OnSelectedIndexChanged="ddlTurmas_SelectedIndexChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVC_T" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVC_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVN_T" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVN_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DataItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataColumn Caption="Noite" Name="Noite" VisibleIndex="3">
                                                    <DataItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hdnIDNoite" runat="server" />
                                                                    <asp:HiddenField ID="hdnTurmaFilhaNoite" runat="server" />
                                                                    <asp:HiddenField ID="hdnEditavelNoite" runat="server" />
                                                                    <asp:Label ID="lblTurma_N" runat="server" Text="Turma" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVCM_N" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVN_N" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTurmas_N" OnDataBound="ddlTurmas_OnDataBound" AutoPostBack="true"
                                                                        DataValueField="Turma" DataTextField="DescricaoTurma" runat="server" OnSelectedIndexChanged="ddlTurmas_SelectedIndexChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVC_N" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVC_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVN_N" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVN_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DataItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataColumn Caption="Ampliado" Name="Ampliado" VisibleIndex="4">
                                                    <DataItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hdnIDAmpliado" runat="server" />
                                                                    <asp:HiddenField ID="hdnEditavelAmpliado" runat="server" />
                                                                    <asp:HiddenField ID="hdnTurmaFilhaAmpliado" runat="server" />
                                                                    <asp:Label ID="lblTurma_A" runat="server" Text="Turma" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVCM_A" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVN_A" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTurmas_A" OnDataBound="ddlTurmas_OnDataBound" AutoPostBack="true"
                                                                        DataValueField="Turma" DataTextField="DescricaoTurma" runat="server" OnSelectedIndexChanged="ddlTurmas_SelectedIndexChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVC_A" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVC_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVN_A" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVN_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DataItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataColumn Caption="Integral" Name="Integral" VisibleIndex="5">
                                                    <DataItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hdnIDIntegral" runat="server" />
                                                                    <asp:HiddenField ID="hdnEditavelIntegral" runat="server" />
                                                                    <asp:HiddenField ID="hdnTurmaFilhaIntegral" runat="server" />
                                                                    <asp:Label ID="lblTurma_I" runat="server" Text="Turma" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVCM_I" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblVN_I" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTurmas_I" OnDataBound="ddlTurmas_OnDataBound" AutoPostBack="true"
                                                                        DataValueField="Turma" DataTextField="DescricaoTurma" runat="server" OnSelectedIndexChanged="ddlTurmas_SelectedIndexChanged" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVC_I" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVC_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVN_I" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                                                        MaxLength="3" Enabled="false" Width="20" OnTextChanged="txtVN_OnTextChanged"
                                                                        onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DataItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                            </Columns>
                                            <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                                        </dxwgv:ASPxGridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:Label ID="lblMensagemVagasBottom" runat="server" SkinID="lblMensagem"></asp:Label>
                                <br />
                                <asp:Button ID="btnSalvarAnaliseVagas" runat="server" OnClick="btnSalvarAnaliseVagas_Click"
                                    Text="Salvar Análise Vagas" Visible="false" />
                                <asp:Button ID="btnSalvarParcialVagas" OnClick="btnSalvarParcialVagas_OnClick" runat="server"
                                    Text="Salvar Vagas Parcialmente" Width="170px" />
                                <asp:Button ID="btnSalvarDefinitivoVagas" OnClick="btnSalvarDefinitivoVagas_OnClick"
                                    runat="server" Text="Finalizar Vagas" Width="150px" />
                                <asp:Button ID="btnExcluirTurmasProvisorias" runat="server" OnClick="btnExcluirTurmasProvisorias_Click"
                                    OnClientClick="return informaSalvarVagasParcial();" Text="Excluir Turmas" Visible="true" />
                                <asp:HiddenField ID="hdnPodeAnalisarVaga" runat="server" />
                                <br />
                                <br />
                                <br />
                                <asp:Panel ID="pnlAnaliseVagas" runat="server" Visible="false">
                                    <table id="Table6" runat="server">
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" ID="label11" Text="Análise Vagas SUPED - Validação:" SkinID="lblobrigatorio"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblAnaliseVagasGeralSUPED" Visible="false"> </asp:Label>
                                                <asp:Label runat="server" ID="lblAnaliseVagasSUPED0" Visible="false" Text="Periodo: 0"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasSUPEDPeriodo0" Visible="false"> </asp:Label>
                                                <asp:DropDownList ID="ddlResultadoVagasSUPEDPeriodo0" Visible="false" runat="server"
                                                    DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label runat="server" ID="lblAnaliseVagasSUPED1" Visible="false" Text="Periodo: 1"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasSUPEDPeriodo1" Visible="false"> </asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="lblAnaliseVagasSUPED2" Text="Periodo: 2" Visible="false"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasSUPEDPeriodo2" Visible="false"> </asp:Label>
                                                <asp:DropDownList ID="ddlResultadoVagasSUPEDPeriodo2" Visible="false" runat="server"
                                                    DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <br />
                                                <asp:Label runat="server" ID="label12" Text="Análise Vagas SUPLAN - Validação:" SkinID="lblobrigatorio"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblAnaliseVagasGeralSUPLAN" Visible="false"> </asp:Label>
                                                <asp:Label runat="server" ID="lblAnaliseVagasSUPLAN0" Visible="false" Text="Periodo: 0"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasSUPLANPeriodo0" Visible="false"> </asp:Label>
                                                <asp:DropDownList ID="ddlResultadoVagasSUPLANPeriodo0" Visible="false" runat="server"
                                                    DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label runat="server" ID="lblAnaliseVagasSUPLAN1" Visible="false" Text="Periodo: 1"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasSUPLANPeriodo1" Visible="false"> </asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="lblAnaliseVagasSUPLAN2" Text="Periodo: 2" Visible="false"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasSUPLANPeriodo2" Visible="false"> </asp:Label>
                                                <asp:DropDownList ID="ddlResultadoVagasSUPLANPeriodo2" Visible="false" runat="server"
                                                    DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <br />
                                                <asp:Label runat="server" ID="label13" Text="Análise Vagas DIESP - Validação:" SkinID="lblobrigatorio"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" ID="lblAnaliseVagasGeralDIESP" Visible="false"> </asp:Label>
                                                <asp:Label runat="server" ID="lblAnaliseVagasDIESP0" Visible="false" Text="Periodo: 0"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasDIESPPeriodo0" Visible="false"> </asp:Label>
                                                <asp:DropDownList ID="ddlResultadoVagasDIESPPeriodo0" Visible="false" runat="server"
                                                    DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label runat="server" ID="lblAnaliseVagasDIESP1" Visible="false" Text="Periodo: 1"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasDIESPPeriodo1" Visible="false"> </asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="lblAnaliseVagasDIESP2" Text="Periodo: 2" Visible="false"> </asp:Label>
                                                <asp:Label runat="server" ID="lblDescAnaliseVagasDIESPPeriodo2" Visible="false"> </asp:Label>
                                                <asp:DropDownList ID="ddlResultadoVagasDIESPPeriodo2" Visible="false" runat="server"
                                                    DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <%--Popups--%>
    <dxpc:ASPxPopupControl ID="pucConfirmarTurnos" ClientInstanceName="pucConfirmarTurnos"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
        Width="880px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
        HeaderText="Turnos Ofertados para Confirmação de Matrícula">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:UpdatePanel ID="updatePanel9" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConfirmarTurnos" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarTurnos" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <table id="Table1" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMensagemPopup" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="tbTurnos" runat="server">
                                        <tr>
                                            <td id="Td28" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px" rowspan="2">
                                                Periodo
                                            </td>
                                            <td id="Td29" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px" rowspan="2">
                                                Modalidade/Curso/Série
                                            </td>
                                            <td style="color: #000000; background-color: #99CCFF; border-style: solid; border-width: 1px"
                                                colspan="5">
                                                Vagas Ofertadas para Matrícula Fácil
                                            </td>
                                            <td style="color: #000000; background-color: #99CCFF; border-style: solid; border-width: 1px"
                                                colspan="5">
                                                Vagas para Absorção de Alunos da Própria Unidade
                                            </td>
                                        </tr>
                                        <tr id="Tr5" runat="server">
                                            <td id="Td30" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Manhã
                                            </td>
                                            <td id="Td31" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Tarde
                                            </td>
                                            <td id="Td32" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Noite
                                            </td>
                                            <td id="Td33" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Ampliado
                                            </td>
                                            <td id="Td34" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Integral
                                            </td>
                                            <td id="Td35" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Manhã
                                            </td>
                                            <td id="Td36" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Tarde
                                            </td>
                                            <td id="Td37" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Noite
                                            </td>
                                            <td id="Td38" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Ampliado
                                            </td>
                                            <td id="Td39" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                border-width: 1px">
                                                Integral
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmar" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btnConfirmarTurnos" runat="server" Text="Confirmar" OnClick="btnFinalizarTurnos_Click"
                                        OnClientClick="pucConfirmarTurnos.Hide(); return true;" />
                                    <asp:Button ID="btnCancelarTurnos" runat="server" Text="Retornar para os Turnos"
                                        OnClientClick="pucConfirmarTurnos.Hide(); return false;" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucParcialTurnos" ClientInstanceName="pucParcialTurnos"
        runat="server" Modal="True" ShowCloseButton="False" ShowSizeGrip="False" EnableAnimation="False"
        Width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
        HeaderText="Importante!">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,13000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:UpdatePanel ID="updatePanel8" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPopupParcialTurnos" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <table style="width: 372px">
                            <tr>
                                <td>
                                    <span class="style1">A oferta ainda não foi concluída, é necessário retornar ao sistema
                                        para finalizá-la até o dia</span>
                                    <asp:Label ID="lblPopupMensagemParcial" runat="server" Style="font-size: small" ForeColor="Red"></asp:Label>
                                    .
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btnPopupParcialTurnos" runat="server" Text="Confirmar" OnClientClick="pucParcialTurnos.Hide(); return false;" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <CloseButtonImage Height="16px" Width="17px" />
        <HeaderStyle HorizontalAlign="Center" />
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pcVagasDataFim" runat="server" Modal="True" Width="350"
        Height="150" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" ClientInstanceName="pcVagasDataFim" HeaderStyle-HorizontalAlign="Center"
        HeaderText="Importante!" AllowDragging="True" EnableAnimation="False" EnableViewState="False"
        ShowPageScrollbarWhenModal="true">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <dxp:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="btnConfirmar">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent1" runat="server">
                            <asp:UpdatePanel ID="updatePanel5" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarVagasPopup" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="center">
                                                <span class="style1">A oferta ainda não foi concluída, é necessário retornar ao sistema
                                                    para finalizá-la até o dia</span>
                                                <asp:Label ID="lblMensagemDataFimVagas" runat="server" SkinID="lblMensagem" />
                                                <br />
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnConfirmarVagasPopup" OnClick="btnConfirmarVagasPopup_OnClick"
                                                    runat="server" Text="Confirmar" Width="130px" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pcTurmaNova" runat="server" Modal="True" Width="300" Height="200"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        ClientInstanceName="pcTurmaNova" HeaderStyle-HorizontalAlign="Center" HeaderText="Turma Nova"
        AllowDragging="True" EnableAnimation="False" AutoUpdatePosition="True">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <dxp:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnSalvarTurmaNova">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent2" runat="server">
                            <asp:UpdatePanel ID="updatePanel4" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaPeriodo" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaModalidade" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaCurso" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaSerie" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaPeriodo" EventName="DataBound" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaModalidade" EventName="DataBound" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaCurso" EventName="DataBound" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTurmaNovaSerie" EventName="DataBound" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSalvarTurmaNova" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr style="height: 30px; vertical-align: middle;">
                                            <td align="right">
                                                <asp:Label ID="Label2" runat="server" Text="Período: " />
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTurmaNovaPeriodo" runat="server" OnSelectedIndexChanged="ddlTurmaNovaPeriodo_SelectedIndexChanged"
                                                    DataTextField="periodo" DataValueField="periodo" Width="140px" AutoPostBack="True"
                                                    OnDataBound="ddlTurmaNovaPeriodo_DataBound" EnableViewState="true" />
                                            </td>
                                        </tr>
                                        <tr style="height: 30px; vertical-align: middle;">
                                            <td align="right">
                                                <asp:Label ID="lblTurmaNovaModalidade" runat="server" Text="Modalidade: " />
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTurmaNovaModalidade" runat="server" OnSelectedIndexChanged="ddlTurmaNovaModalidade_SelectedIndexChanged"
                                                    DataTextField="descricao" DataValueField="modalidade" Width="140px" AutoPostBack="True"
                                                    OnDataBound="ddlTurmaNovaModalidade_DataBound" EnableViewState="true" />
                                            </td>
                                        </tr>
                                        <caption>
                                            <br />
                                            <tr style="height: 30px; vertical-align: middle;">
                                                <td align="right">
                                                    <asp:Label ID="lblTurmaNovaCurso" runat="server" Text="Curso: " />
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlTurmaNovaCurso" runat="server" AutoPostBack="True" OnDataBound="ddlTurmaNovaCurso_DataBound"
                                                        OnSelectedIndexChanged="ddlTurmaNovaCurso_SelectedIndexChanged" DataTextField="nome"
                                                        DataValueField="curso" Width="180px" />
                                                </td>
                                            </tr>
                                            <caption>
                                                <br />
                                                <tr style="height: 30px; vertical-align: middle;">
                                                    <td align="right">
                                                        <asp:Label ID="lblTurmaNovaSerie" runat="server" Text="Serie: " />
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlTurmaNovaSerie" runat="server" AutoPostBack="True" OnDataBound="ddlTurmaNovaSerie_DataBound"
                                                            CausesValidation="true" DataTextField="serie" DataValueField="serieprefixo" Width="140px"
                                                            OnSelectedIndexChanged="ddlTurmaNovaSerie_SelectedIndexChanged" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 30px; vertical-align: middle;">
                                                    <td align="right">
                                                        <asp:Label ID="lblTurmaNovaTurma" runat="server" Text="Turma: " />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblTurmaNovaSeriePrefixo" runat="server" />
                                                        <asp:TextBox ID="txtTurmaNova" runat="server" Width="50px" MaxLength="2" onkeypress="return OnlyNumericEntry(event)" />
                                                        <asp:Label ID="lblTurmaNovaSetor" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" style="height: 30px; vertical-align: bottom;">
                                                        <asp:Label ID="lblTurmaNovaMensagem" runat="server" SkinID="lblMensagem" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" style="height: 30px; vertical-align: bottom;">
                                                        <dxe:ASPxButton ID="btnSalvarTurmaNova" runat="server" OnClick="btnSalvarTurmaNova_OnClick"
                                                            Text="Salvar Turma" Width="130px" />
                                                    </td>
                                                </tr>
                                            </caption>
                                        </caption>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pcVagasOfertadas" runat="server" Modal="True" Width="800"
        Height="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" ClientInstanceName="pcVagasOfertadas" HeaderStyle-HorizontalAlign="Center"
        HeaderText="Vagas Ofertadas" AllowDragging="True" EnableAnimation="False" EnableViewState="False"
        ShowPageScrollbarWhenModal="true">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,16000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <dxp:ASPxPanel ID="ASPxPanel3" runat="server" DefaultButton="btnConfirmar">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent3" runat="server">
                            <asp:UpdatePanel ID="updatePanel6" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarVagasOfertadas" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <div style="width: 650; height: 400px; border: solid thin black; overflow-y: scroll;">
                                        <table id="Table2" runat="server" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table id="tbVagas" runat="server">
                                                        <tr>
                                                            <td id="Td40" runat="server" class="style7" style="color: #000000; background-color: #99CCFF;
                                                                border-style: solid; border-width: 1px" align="center" rowspan="2">
                                                                Período
                                                            </td>
                                                            <td id="Td41" runat="server" class="style7" style="color: #000000; background-color: #99CCFF;
                                                                border-style: solid; border-width: 1px" align="center" rowspan="2">
                                                                Modalidade/Curso/Série
                                                            </td>
                                                            <td align="center" class="style6" style="color: #000000; background-color: #99CCFF;
                                                                border-style: solid; border-width: 1px" colspan="5">
                                                                Vagas Ofertadas para Matrícula Fácil
                                                            </td>
                                                            <td align="center" class="style6" style="color: #000000; background-color: #99CCFF;
                                                                border-style: solid; border-width: 1px" colspan="5">
                                                                Vagas para absorção de alunos da própria unidade
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr6" runat="server">
                                                            <td id="Td42" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Manhã
                                                            </td>
                                                            <td id="Td43" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Tarde
                                                            </td>
                                                            <td id="Td44" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Noite
                                                            </td>
                                                            <td id="Td45" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Ampliado
                                                            </td>
                                                            <td id="Td46" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Integral
                                                            </td>
                                                            <td id="Td47" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Manhã
                                                            </td>
                                                            <td id="Td48" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Tarde
                                                            </td>
                                                            <td id="Td49" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Noite
                                                            </td>
                                                            <td id="Td50" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Ampliado
                                                            </td>
                                                            <td id="Td51" runat="server" style="color: #000000; background-color: #99CCFF; border-style: solid;
                                                                border-width: 1px" align="center">
                                                                Integral
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table runat="server" width="100%">
                                        <tr>
                                            <td style="height: 30px; vertical-align: bottom; text-align: left">
                                                <dxe:ASPxButton ID="btnConfirmarVagasOfertadas" runat="server" OnClick="btnConfirmarVagasOfertadas_OnClick"
                                                    Text="Confirmar" Width="130px" />
                                            </td>
                                            <td style="height: 30px; vertical-align: bottom; text-align: right">
                                                <dxe:ASPxButton ID="btnRetornarVagasOfertadas" runat="server" OnClick="btnRetornarVagasOfertadas_OnClick"
                                                    Text="Retornar para Vagas" Width="130px" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pcConfirmacaoValidacaoVagas" runat="server" Modal="True"
        Width="800px" Height="400px" ShowPageScrollbarWhenModal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" ClientInstanceName="pcConfirmacaoValidacaoVagas"
        HeaderStyle-HorizontalAlign="Center" HeaderText="Confirmação de Vagas" AllowDragging="True"
        EnableAnimation="False" EnableViewState="False">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,17000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <dxp:ASPxPanel ID="ASPxPanel4" runat="server" DefaultButton="btnConfirmarValidacaoVagas">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent4" runat="server">
                            <asp:UpdatePanel ID="updatePanel10" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:HiddenField runat="server" ID="hdnOrigem" />
                                    <div style="width: 650; height: 400px; border: solid thin black; overflow-y: scroll;">
                                        <table id="Table3" width="100%" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMensagemValidacaoVagas" Visible="true" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table id="Table4" width="100%" runat="server">
                                        <tr style="text-align: center">
                                            <td>
                                                <dxe:ASPxButton ID="btnConfirmarValidacaoVagas" runat="server" OnClick="btnConfirmarValidacaoVagas_OnClick"
                                                    Text="Confirmar" />
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnCancelarValidacaoVagas" runat="server" OnClick="btnCancelarValidacaoVagas_OnClick"
                                                    Text="Cancelar" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxp:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucConfirmaTurmaNovaComDados" ClientInstanceName="pucConfirmaTurmaNovaComDados"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
        Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/"
        HeaderText="Confirma criar nova turma">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,18000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:UpdatePanel ID="updatePanel12" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConfirmaTurmaNovaComDados" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnNaoConfirmaTurmaNovaComDados" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <table id="Table5" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Deseja criar turma para a mesma modalidade/curso?"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnConfirmaTurmaNovaComDados" runat="server" Text="Sim" OnClick="btnConfirmaTurmaNovaComDados_Click"
                                        OnClientClick="pucConfirmaTurmaNovaComDados.Hide(); return true;" />
                                    <asp:Button ID="btnNaoConfirmaTurmaNovaComDados" runat="server" Text="Não" OnClick="btnNaoConfirmaTurmaNovaComDados_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <%-- Ods --%>
    <asp:ObjectDataSource ID="odsConfirmacaoTurnos" TypeName="Techne.Lyceum.Net.Academico.ConfirmacaoTurnosVagas"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsConfVagas" TypeName="Techne.Lyceum.Net.Academico.ConfirmacaoTurnosVagas"
        runat="server" SelectMethod="ListarVagas"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsModalidade" runat="server" TypeName="Techne.Lyceum.RN.Curso"
        SelectMethod="listaModalidadeSegmentoCursoPor">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" PropertyName="SelectedValue" Name="ano" />
            <asp:ControlParameter ControlID="ddlPerfil" PropertyName="SelectedValue" Name="perfilId" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%--<asp:ObjectDataSource ID="odsVagasPopup" TypeName="Techne.Lyceum.Net.Academico.ConfirmacaoTurnosVagas"
        runat="server" SelectMethod="ListarVagasPopup">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>--%>
</asp:Content>
