<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HistoricoNotasTurma.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.HistoricoNotasTurma" %>

<asp:Content ID="cNotasTurma" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function EndRequestHandler(sender, args) 
            AddEvents();
        }

        function BloquearCtrl() {
            if (event.keyCode == 17) {
                alert("Proibido utilizar o Ctrl neste campo");
            }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }

        function controlRelatedFields(field) {
            var txtConceitoId = $(field).attr("conceito");
            var txtNotaRecuperacaoId = $(field).attr("NotaRecuperacao");
            var txtNotaFinalId = $(field).attr("NotaFinal");
            var isChecked = $(field).is(':checked');
            var txtConceito = $("#" + txtConceitoId);
            var txtNotaRecuperacao = $("#" + txtNotaRecuperacaoId);
            var txtNotaFinal = $("#" + txtNotaFinalId);
            var sitMatricula = $(field).attr("sitMatricula");
            var cmbJustificativaId = $(field).attr("justificativa");
            var cmbJustificativa = $("#" + cmbJustificativaId);
            var chkRecuperacaoId = $(field).attr("recuperacao");
            var chkRecuperacao = $("#" + chkRecuperacaoId);

            if (sitMatricula == "APROVADO" || sitMatricula == "APROVADO CONSELHO" || sitMatricula == "REP FREQ" || sitMatricula == "REP NOTA") {
                if (isChecked) {

                    $(chkRecuperacao).prop('checked', false);
                    $(chkRecuperacao).prop('disabled', true);

                    $(txtConceito).val("");
                    $(txtConceito).attr("readonly", "readonly");
                    $(txtConceito).attr("disabled", true);
                    $(txtConceito).css("background-color", "Gainsboro");

                    $(txtNotaRecuperacao).val("");
                    $(txtNotaRecuperacao).attr("readonly", "readonly");
                    $(txtNotaRecuperacao).attr("disabled", true);
                    $(txtNotaRecuperacao).css("background-color", "Gainsboro");

                    $(txtNotaFinal).val("");
                    $(txtNotaFinal).attr("readonly", "readonly");
                    //$(txtNotaFinal).attr("disabled", true);
                    $(txtNotaFinal).css("background-color", "Gainsboro");

                    //$(cmbJustificativa).removeAttr("disabled");
                }
                else {
                    $(txtConceito).removeAttr("readonly");
                    $(txtConceito).removeAttr("disabled");
                    $(txtConceito).css("background-color", "");

                    $(txtNotaFinal).attr("readonly", "readonly");
                    //                    $(txtNotaFinal).attr("disabled", true);
                    $(txtNotaFinal).css("background-color", "Gainsboro");

                    $(cmbJustificativa).attr("disabled", true);
                    $(cmbJustificativa).val("Selecione");
                }
            }

            ControlaBloqCampos();

        }

        function controlRelatedFieldsRecuperacao(field) {
            var txtConceitoId = $(field).attr("conceito");
            var txtConceito = $("#" + txtConceitoId);
            var txtNotaRecuperacaoId = $(field).attr("NotaRecuperacao");
            var isChecked = $(field).is(':checked');
            var txtNotaRecuperacao = $("#" + txtNotaRecuperacaoId);
            var sitMatricula = $(field).attr("sitMatricula");

            if (sitMatricula == "APROVADO" || sitMatricula == "APROVADO CONSELHO" || sitMatricula == "REP FREQ" || sitMatricula == "REP NOTA") {
                if (isChecked) {

                    if ($(txtConceito).val() == "" || parseFloat($(txtConceito).val()) >= 5) {
                        $(field).prop('checked', false);
                        $(field).prop('disabled', true);
                        return;
                    }

                    $(txtNotaRecuperacao).removeAttr("readonly");
                    $(txtNotaRecuperacao).css("background-color", "");
                    $(txtNotaRecuperacao).removeAttr("disabled");

                }
                else {
                    $(txtNotaRecuperacao).val("");
                    $(txtNotaRecuperacao).attr("readonly", "readonly");
                    $(txtNotaRecuperacao).attr("disabled", true);
                    $(txtNotaRecuperacao).css("background-color", "Gainsboro");
                }
            }

        }

        function ControlaBloqCampos() {
            if ($('input[name$="hdnTemNota"]').val() == "N") {
                $(".input-nota").val("");
                $(".input-nota").attr("disabled", true);
                $(".input-nota").attr("readonly", "readonly");
                $(".input-nota").css("background-color", "Gainsboro");
                //----------------------------------------------------------
                $("input[id*='chkSemAvaliacao']").attr("disabled", true);
                $("input[id*='cmbJustificativa']").attr("disabled", true);
            }

            if ($('input[name$="hdnTemFreq"]').val() == "N" || $('input[name$="hdnFreq"]').val() == "") {
                $(".input-faltas").val("");
                $(".input-faltas").attr("disabled", true);
                $(".input-faltas").attr("readonly", "readonly");
                $(".input-faltas").css("background-color", "Gainsboro");
            }
        }

        $(document).ready(function() {
            AddEvents();
            ControlaBloqCampos();
        });

        // adiciona eventos nas células de notas da Grid para:
        // 1. navegação com direcionais do teclado
        // 2. validação dos caracteres digitados na célula
        function AddEvents() {
            $("input[id*='txtFrequencia']").numeric();

            $('#<%= this.txtAulasPrevistas.ClientID %>').numeric();
            $('#<%= this.txtAulasDadas.ClientID %>').numeric();

            $("input[id*='txtFrequencia']").bind('paste', function(e) {
                return false;
            });

            $("input[id*='txtFrequencia']").bind('keypress', function(e) {
                var keyCode = (e.which) ? e.which : event.keyCode
                return !(keyCode > 31 && (keyCode < 48 || keyCode > 57));
            });

            $("input[id*='chkSemAvaliacao']").click(function() {
                controlRelatedFields(this);
            });

            $("input[id*='chkSemAvaliacao']").each(function() {
                controlRelatedFields(this);
            });

            $("input[id*='chkRecuperacao']").click(function() {
                controlRelatedFieldsRecuperacao(this);
                CalculaNotaFinal(this);
            });

            $("input[id*='chkRecuperacao']").each(function() {
                controlRelatedFieldsRecuperacao(this);
                CalculaNotaFinal(this);
            });

            // registra eventos 'keydown' para controles que possuem attributo "navegar"
            $("#<%=grdMatriculas.ClientID %> *[navegar=true]").each(function(i) {
                var col = parseInt($("#" + this.id).attr("columnIndex"));
                var row = parseInt($("#" + this.id).attr("rowIndex"));

                $("#" + this.id).keydown(function(e) {
                    nav(e, row, col);
                });
            });

            // registra eventos nas células das notas que possuem atributo "validar=true" da Grid para validação numérica:
            // 1. evento 'keypress' que permite apenas números decimais positivos e 'SN'
            // 2. evento 'change' que valida o novo valor da célula
            // 3. evento 'blur' que adiciona o ',0' às células com números inteiros positivos
            // Obs.: As células com atributo "validar=true" correspondem às notas dos alunos com situação 'Matriculado'.
            $("#<%=grdMatriculas.ClientID %> *[validar=true]").each(function(i) {
                var notaCelula = this;
                ValidaCorrige(notaCelula);

                $(notaCelula).bind('keypress', function(e) {
                    var permiteCaracter = ValidaKeyPress(notaCelula, e);
                    e.stopImmediatePropagation();
                    return permiteCaracter;
                });

                $(notaCelula).bind('change', function(e) {
                    ValidaCorrige(notaCelula);
                    CalculaNotaFinal(notaCelula);
                    e.stopImmediatePropagation();
                });

                $(notaCelula).bind('blur', function(e) {
                    InsereVirgulaZero(notaCelula);
                    HabilitaRecParalelaConf(notaCelula);
                    CalculaNotaFinal(notaCelula);
                    e.stopImmediatePropagation();
                });

                $(notaCelula).bind('input paste', function(e) {
                    return false;
                }).bind('contextmenu', function(e) {
                    return false;
                });
            });

            // registra eventos nas células das notas de recuperacao que possuem atributo "validarRecuperacao=true" da Grid para validação numérica:
            // 1. evento 'keypress' que permite apenas números decimais positivos e 'SN'
            // 2. evento 'change' que valida o novo valor da célula
            // 3. evento 'blur' que adiciona o ',0' às células com números inteiros positivos
            // Obs.: As células com atributo "validarRecuperacao=true" correspondem às notas de recuperacao dos alunos com situação 'Matriculado'.
            $("#<%=grdMatriculas.ClientID %> *[validarRecuperacao=true]").each(function(i) {
                var notaCelula = this;
                ValidaCorrige(notaCelula);

                $(notaCelula).bind('keypress', function(e) {
                    var permiteCaracter = ValidaKeyPress(notaCelula, e);
                    e.stopImmediatePropagation();
                    return permiteCaracter;
                });

                $(notaCelula).bind('change', function(e) {
                    ValidaCorrige(notaCelula);
                    CalculaNotaFinal(notaCelula);
                    e.stopImmediatePropagation();
                });

                $(notaCelula).bind('blur', function(e) {
                    InsereVirgulaZero(notaCelula);
                    CalculaNotaFinal(notaCelula);
                    e.stopImmediatePropagation();
                });

                $(notaCelula).bind('input paste', function(e) {
                    return false;
                }).bind('contextmenu', function(e) {
                    return false;
                });
            });


        }
        // constantes e enumerações usadas para o controle de navegação das teclas pressinadas
        var KeyCode = { "Left": 37, "Up": 38, "Right": 39, "Down": 40, "Enter": 13 };
        var ControlType = { "button": {}, "checkbox": {}, "file": {}, "hidden": {}, "image": {},
            "password": {}, "radio": {}, "reset": {}, "select-one": {},
            "select-multiple": {}, "submit": {}, "text": {}, "textarea": {}
        };

        // função para navegação entre controles através das setas
        function nav(e, row, column) {
            var unicode = getKeyCode(e);
            var direcional = false;

            // atualiza índices row, column do próximo controle, conforme seta pressionada
            switch (unicode) {
                case KeyCode.Up:
                    row -= 1;
                    direcional = true;
                    break;

                case KeyCode.Down: row += 1;
                    direcional = true;
                    break;

                case KeyCode.Left: column -= 1;
                    direcional = true;
                    break;

                case KeyCode.Right: column += 1;
                    direcional = true;
                    break;

                case KeyCode.Enter: row += 1;
                    direcional = true;
                    break;
            }

            if (!direcional) {
                return;
            }

            // busca o próximo controle
            var ob = $("#<%=grdMatriculas.ClientID %> *[navegar=true][columnIndex=" + column + "][rowIndex=" + row + "]");
            // se não nulo, foca no controle ou avança para o próximo controle caso readOnly || disabled
            if (ob.length) {
                //verifica se controle é tipo text
                if (ob.attr("type") == "text") {
                    // se controle readOnly ou disabled, chama evento para avançar para próximo controle
                    if (ob.attr("disabled")
                        || ob.attr("readOnly")
                        && direcional) {
                        nav(e, row, column);
                        return;
                    }
                    // se controle enabled e !readonly, foca no controle
                    else {
                        ob.focus();
                    }
                }
                // se nulo, volta para início ou fim da linha ou coluna, conforme tecla pressionada
            } else {
                var coltemp = 0, rowtemp = 0;

                if (unicode == KeyCode.Up || unicode == KeyCode.Down || unicode == KeyCode.Enter)
                    coltemp = column;
                if (unicode == KeyCode.Left || unicode == KeyCode.Right)
                    rowtemp = row;

                switch (unicode) {
                    case KeyCode.Up:
                        if (row < 0)
                            nav(e, $("#<%=grdMatriculas.ClientID %> *[navegar=true][columnIndex=" + coltemp + "]:last").attr("rowIndex"), coltemp);
                        else
                            $("#<%=grdMatriculas.ClientID %> *[navegar=true][columnIndex=" + coltemp + "]:last").focus();
                        break;
                    case KeyCode.Down:
                    case KeyCode.Enter:
                        $("#<%=grdMatriculas.ClientID %> *[navegar=true][columnIndex=" + coltemp + "]:first").focus();
                        break;
                    case KeyCode.Left:
                        $("#<%=grdMatriculas.ClientID %> *[navegar=true][rowIndex=" + rowtemp + "]:last").focus();
                        break;
                    case KeyCode.Right:
                        $("#<%=grdMatriculas.ClientID %> *[navegar=true][rowIndex=" + rowtemp + "]:first").focus();
                        break;
                }
            }
        }

        // constantes e enumerações usadas para a validação dos caracteres digitados pelo usuário
        var Keys = { "Backspace": 8, "Tab": 9, "Left": 37, "Up": 38, "Right": 39, "Down": 40, "Del": 46, "End": 35, "Home": 36, "Shift": 16 };

        function ValidaKeyPress(jqObject, e) {

            $(jqObject).replaceSelection('', true);

            var keyCode = getKeyCode(e);
            var new_char = String.fromCharCode(keyCode);
            var old_value = $(jqObject).val().replace(",", ".");
            var notaMax = $(jqObject).attr("notaMax");
            var numCasasDec = $("#<%=hdnNCasasDec.ClientID %>").val();

            if (new_char == ",") {
                new_char = ".";
            }

            // Bloqueia inserção de mais de um separador decimal
            if (new_char == "." && old_value.indexOf('.') >= 0) {
                return (false);
            }

            if (new_char == "." && old_value.length == 0) {
                old_value = "0";
            }
            else if ("0123456789.".indexOf(new_char) >= 0) {
                var fut_value = old_value + new_char;

                // Valor máximo atingido
                if (parseFloat(fut_value) > parseFloat(notaMax.replace(",", "."))) {
                    return (false);
                }
                else if (parseFloat(fut_value) < parseFloat(parseFloat(0.5) * parseFloat(notaMax.replace(",", ".")))) {
                    $(jqObject).css({ "color": "Red" });
                }
                else {
                    $(jqObject).css({ "color": "Blue" });
                }

                // Número máximo de casas decimais atingido
                if (fut_value.indexOf('.') >= 0) {
                    if (fut_value.substring(fut_value.indexOf('.'), fut_value.length - 1).length > parseInt(numCasasDec)) return (false);
                }

                $(jqObject).val(fut_value.replace(".", ","));

                return (false);
            } else if (keyCode == Keys.Tab || keyCode == Keys.Home || keyCode == Keys.End || keyCode == Keys.Shift) {
                return (true);
            } else if (keyCode == Keys.Backspace || keyCode == Keys.Del) {
                return (true);
            } else {
                return (false);
            }
        }

        function ValidaCorrige(jqObject) {

            //POG
            try {
                var ok = $(jqObject);
            } catch (e) {
                return;
            }

            // obtém atributos da célula referente à nota do aluno
            var notaMax = parseFloat($(jqObject).attr("notaMax").replace(",", "."));
            var numCasasDec = document.getElementById("<%=hdnNCasasDec.ClientID %>") != null ? document.getElementById("<%=hdnNCasasDec.ClientID %>").value : 0;
            var notaCelula = $(jqObject).val().replace(",", ".");

            if ("sS".indexOf(notaCelula) >= 0)
                if ("0123456789.".indexOf(notaCelula) >= 0)
                $(jqObject).val("");
            else
                $(jqObject).val("SN");

            // 3. Número Máximo de Casas Decimais:
            // A partir da ocorrência de ponto na célula, verifica-se se o número de casas decimais permitido foi atingigo.
            // Caso positivo, os caracteres excedentes devem ser eliminados na célula.
            var ocorrenciaPonto = notaCelula.indexOf('.');
            var casasDecimaisCelula = notaCelula.substring(ocorrenciaPonto, notaCelula.length - 1).length;

            if (ocorrenciaPonto >= 0) {
                if (casasDecimaisCelula > parseInt(numCasasDec)) {
                    var comprimentoCelulaValido = notaCelula.length - casasDecimaisCelula + parseInt(numCasasDec);
                    $(jqObject).val(notaCelula.substring(0, comprimentoCelulaValido).replace(".", ","));
                }
            }

            var valor = parseFloat($(jqObject).val().replace(",", "."));

            $(jqObject).css({ "color": (valor >= notaMax / 2.0) ? "Blue" : "Red" });
        }

        function InsereVirgulaZero(jqObject) {

            var oval = $(jqObject).val().replace(",", ".");
            if (oval != "") {
                if (oval != null && oval.indexOf('.') < 0)
                    $(jqObject).val(oval + ",0");
                else if (oval.indexOf('.') == oval.length - 1) {
                    $(jqObject).val(oval.replace(".", ",") + "0");
                }
            }
        }

        function HabilitaRecParalela(jqObject) {

            var jqTRLancamento = $(jqObject).closest('tr');
            var jqChkRecParalela = jqTRLancamento.find('span.chkrecuperacaoparalela').find('input:checkbox');
            var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');


            if (parseFloat(jqObject.value) < 5) {
                jqChkRecParalela.prop('disabled', false);
                jqChkRecParalela.prop('checked', true);
                jqInputRecParalela.val('').prop('disabled', false);
                jqInputRecParalela.css('background-color', '#FFF');
            }
            else {
                jqChkRecParalela.prop('disabled', true);
                jqChkRecParalela.prop('checked', false);
                jqInputRecParalela.val('').prop('disabled', true);
                jqInputRecParalela.css('background-color', '#E0E0E0');
            }

        }

        function CalculaNotaFinal(jqObject) {

            var jqTRLancamento = $(jqObject).closest('tr');
            var jqInputNota = jqTRLancamento.find('input:text.input-nota');
            var jqInputNotaRecuperacao = jqTRLancamento.find('input:text.inputnotarecuperacao');
            var jqInputNotaFinalBimestre = jqTRLancamento.find('input:text.inputconceito');

            $(jqInputNotaFinalBimestre).val(jqInputNota.val());

            if (parseFloat(jqInputNota.val().replace(",", ".")) < parseFloat(jqInputNotaRecuperacao.val().replace(",", ".") == "" ? 0 : jqInputNotaRecuperacao.val().replace(",", "."))) {
                $(jqInputNotaFinalBimestre).val(jqInputNotaRecuperacao.val());
            }

            var notaMaxVer = parseFloat($(jqObject).attr("notaMax").replace(",", "."));

            var valor = parseFloat($(jqInputNotaFinalBimestre).val().replace(",", "."));

            $(jqInputNotaFinalBimestre).css({ "color": (valor >= notaMaxVer / 2.0) ? "blue" : "red" });


        }


        function HabilitaRecParalelaConf(jqObject) {

            if (parseFloat(jqObject.defaultValue) < 5 && parseFloat(jqObject.value) >= 5) {

                var decisao = confirm("A nota da recuperação de estudos será cancelada devido à alteração na nota do aluno. Confirma a alteração?");
                if (decisao) {
                    HabilitaRecParalela(jqObject);
                    jqObject.defaultValue = jqObject.value;
                }
                else {
                    jqObject.value = jqObject.defaultValue;
                    ValidaCorrige(jqObject);
                }
            }
            else if (parseFloat(jqObject.defaultValue) >= 5 && parseFloat(jqObject.value) < 5) {

                var jqTRLancamento = $(jqObject).closest('tr');
                var oChkRecuperacao = jqTRLancamento.find('span.chkrecuperacaoparalela').find('input:checkbox');
                var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');
                var jqInputFaltas = jqTRLancamento.find('input:text.input-faltas');

                var decisao = confirm("Prezado diretor, foram aplicadas estratégias de recuperação de estudo ao aluno? Em caso de dúvidas, para saber mais sobre recuperação de estudos clique no link abaixo das aulas dadas.");

                if (decisao) {

                    oChkRecuperacao.prop('checked', true);
                    oChkRecuperacao.prop('disabled', false);
                    jqInputRecParalela.val('').prop('disabled', false);
                    jqInputRecParalela.css('background-color', '#FFF');
                    jqInputRecParalela.removeAttr("readonly");
                    jqInputFaltas.focus();
                    HabilitaRecParalela(jqObject);
                    jqObject.defaultValue = jqObject.value;
                }
                else {
                    HabilitaRecParalela(jqObject);
                    jqInputRecParalela.val('').prop('disabled', true);
                    jqInputRecParalela.css('background-color', '#E0E0E0');
                    jqInputFaltas.focus();
                    oChkRecuperacao.prop('checked', false);
                    ValidaCorrige(jqObject);
                }
            }
            else if ($.trim(jqObject.defaultValue) == "" && parseFloat(jqObject.value) < 5) {
                var jqTRLancamento = $(jqObject).closest('tr');
                var oChkRecuperacao = jqTRLancamento.find('span.chkrecuperacaoparalela').find('input:checkbox');
                var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');
                var jqInputFaltas = jqTRLancamento.find('input:text.input-faltas');

                var decisao = confirm("Prezado diretor, foram aplicadas estratégias de recuperação de estudo ao aluno? Em caso de dúvidas, para saber mais sobre recuperação de estudos no link abaixo das aulas dadas.");

                if (decisao) {
                    oChkRecuperacao.prop('checked', true);
                    oChkRecuperacao.prop('disabled', false);
                    jqInputRecParalela.val('').prop('disabled', false);
                    jqInputRecParalela.css('background-color', '#FFF');
                    jqInputRecParalela.removeAttr("readonly");
                    jqInputFaltas.focus();
                    HabilitaRecParalela(jqObject);
                    jqObject.defaultValue = jqObject.value;
                }
                else {

                    HabilitaRecParalela(jqObject);
                    jqInputRecParalela.val('').prop('disabled', true);
                    jqInputRecParalela.css('background-color', '#E0E0E0');
                    jqInputFaltas.focus();
                    oChkRecuperacao.prop('checked', false);
                    ValidaCorrige(jqObject);
                }
            }
            else if ((parseFloat(jqObject.defaultValue) < 5 && parseFloat(jqObject.value) < 5) && (parseFloat(jqObject.defaultValue) != parseFloat(jqObject.value))) {

                var jqTRLancamento = $(jqObject).closest('tr');
                var oChkRecuperacao = jqTRLancamento.find('span.chkrecuperacaoparalela').find('input:checkbox');
                var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');
                var jqInputFaltas = jqTRLancamento.find('input:text.input-faltas');

                var decisao = confirm("Prezado diretor, foram aplicadas estratégias de recuperação de estudo ao aluno? Em caso de dúvidas, para saber mais sobre recuperação de estudos no link abaixo das aulas dadas.");

                if (decisao) {
                    oChkRecuperacao.prop('checked', true);
                    oChkRecuperacao.prop('disabled', false);
                    jqInputRecParalela.val('').prop('disabled', false);
                    jqInputRecParalela.css('background-color', '#FFF');
                    jqInputRecParalela.removeAttr("readonly");
                    jqInputFaltas.focus();
                    HabilitaRecParalela(jqObject);
                    jqObject.defaultValue = jqObject.value;
                }
                else {

                    HabilitaRecParalela(jqObject);
                    jqInputRecParalela.val('').prop('disabled', true);
                    jqInputRecParalela.css('background-color', '#E0E0E0');
                    jqInputFaltas.focus();
                    oChkRecuperacao.prop('checked', false);
                    ValidaCorrige(jqObject);
                }

            }
            else {

                HabilitaRecParalela(jqObject);
                jqObject.defaultValue = jqObject.value;
            }
        }

        // Trata as justificativas;
        function TrataJustificativa() {
            var jqInputLicenca = jqListaTRLancamentosMatriculados.find(".inputLicenca");
            var jqInputLancamento = jqListaTRLancamentosMatriculados.find(".inputLancamento100");
            var jqInputFaltas = jqListaTRLancamentosMatriculados.find(".input-faltas");
            var aulasDadas = $("#AulasDadas");

            var msg = "Declaro ter conhecimento do instrumento que formaliza o afastamento e solicitei à equipe pedagógica a aplicação residencial dos instrumentos de avaliação";
            var msgDependencia = "Declaro que solicitei à equipe pedagógica da unidade o contato com o aluno para que entregue o trabalho até o próximo bimestre/trimestre";

            for (var i = 0; i < jqInputLicenca.length; i++) {
                if (jqInputLicenca[i].value == "True") {
                    $(jqInputLancamento[i]).change(function() {
                        // Licenca.
                        if (this.value == "1") {
                            if (!confirm(msg)) {
                                this.value = "";
                            }
                        }

                        if (this.value != "")
                            this.title = $(this).find('option:selected').text();
                        else
                            this.title = "";
                    })
                }

                $(jqInputLancamento[i]).change({ input: jqInputFaltas[i] }, function(e) {

                    if (this.value == "2") {
                        var percentual = e.data.input.value / aulasDadas.val() * 100;
                        // Outros.
                        if (percentual >= 100 - FREQUENCIA_MINIMA) {
                            if (!confirm("Declaro que alertei a unidade escolar a respeito das faltas do aluno para análise e preparação da ficha de comunicação do aluno infrequente (FICAI)")) {
                                this.value = "";
                            }
                        } else {
                            this.value = "";
                            alert("O diretor deverá lançar valor da nota do(s) instrumento(s) aplicado(s) durante período em que o aluno esteve frequente");
                        }
                    }

                    if (this.value != "")
                        this.title = $(this).find('option:selected').text();
                    else
                        this.title = "";

                })

                $(jqInputLancamento[i]).change(function() {
                    //Dependencia
                    if (this.value == "0") {
                        if (!confirm(msgDependencia)) {
                            this.value = "";
                        }
                    }

                    if (this.value != "")
                        this.title = $(this).find('option:selected').text();
                    else
                        this.title = "";

                })
            }
        }

        function ValidaPreenchimentoTotal() {

            if ($('input[name$="hdnTemNota"]').val() == "S") {
                var notasEmBranco = $("#<%=grdMatriculas.ClientID %> input:text.input-nota").filter(
                function() {
                    var vazio = this.value == "";
                    var semAvaliacaoId = $(this).attr("semAvaliacao");
                    var semAvaliacao = $("#" + semAvaliacaoId).attr("checked");
                    var sitMatricula = $(this).attr("sitMatricula");

                    return vazio
                           && !semAvaliacao
                           && (sitMatricula == "APROVADO" || sitMatricula == "APROVADO CONSELHO" || sitMatricula == "REP FREQ" || sitMatricula == "REP NOTA");
                });

                var totalNotasEmBranco = notasEmBranco.length;
                var notasEmBrancoAlunos = "";

                if (totalNotasEmBranco > 0) {

                    for (var i = 0; i < notasEmBranco.length; i++) {
                        notasEmBrancoAlunos += "\n" + ($(notasEmBranco[i]).attr("nomeAluno"));
                    }

                    alert('O lançamento foi realizado de forma parcial (' + totalNotasEmBranco + ((totalNotasEmBranco == 1) ? ' nota pendente' : ' notas pendentes') + ').\nPara salvar o lançamento das notas, por favor, digite a nota de todos os alunos abaixo: \n' + notasEmBrancoAlunos);
                    return (false);

                }
            }

            if ($('input[name$="hdnTemFreq"]').val() == "S" && $('input[name$="hdnFreq"]').val() != "") {
                var faltasEmBranco = $("#<%=grdMatriculas.ClientID %> input[falta]:text").filter(
                    function() {
                        var vazio = this.value == "";
                        var sitMatricula = $(this).attr("sitMatricula");

                        return vazio
                               && (sitMatricula == "APROVADO" || sitMatricula == "APROVADO CONSELHO" || sitMatricula == "REP FREQ" || sitMatricula == "REP NOTA");
                    }
                );

                var totalFaltasEmBranco = faltasEmBranco.length;
                var faltasEmBrancoAlunos = "";

                if (totalFaltasEmBranco > 0) {

                    for (var i = 0; i < faltasEmBranco.length; i++) {
                        faltasEmBrancoAlunos += "\n" + ($(faltasEmBranco[i]).attr("nomeAluno"));
                    }

                    alert('O lançamento foi realizado de forma parcial (' + totalFaltasEmBranco +
                    ((totalFaltasEmBranco == 1) ? ' falta pendente' : ' faltas pendentes') + ').\nPara salvar o lançamento, por favor, digite a falta de todos os alunos abaixo:\n' + faltasEmBrancoAlunos);
                    return (false);
                }
            }

            var notasRecuperacaoEmBranco = $("#<%=grdMatriculas.ClientID %> input:text.inputnotarecuperacao").filter(
                function() {
                    var vazio = this.value == "";
                    var notaRecuperacaoId = $(this).attr("recuperacao");
                    var notaRecuperacao = $("#" + notaRecuperacaoId).attr("checked");
                    var sitMatricula = $(this).attr("sitMatricula");
                    return vazio
                           && notaRecuperacao
                           && (sitMatricula == "APROVADO" || sitMatricula == "APROVADO CONSELHO" || sitMatricula == "REP FREQ" || sitMatricula == "REP NOTA");
                }
            );

            var totalnotasRecuperacaoEmBranco = notasRecuperacaoEmBranco.length;
            var notasRecuperacaoEmBrancoAlunos = "";

            if (totalnotasRecuperacaoEmBranco > 0) {

                for (var i = 0; i < notasRecuperacaoEmBranco.length; i++) {
                    notasRecuperacaoEmBrancoAlunos += "\n" + ($(notasRecuperacaoEmBranco[i]).attr("nomeAluno"));
                }

                alert('O lançamento foi realizado de forma parcial (' + totalnotasRecuperacaoEmBranco +
                ((totalnotasRecuperacaoEmBranco == 1) ? ' nota de recuperação pendente' : ' notas de recuperação pendentes') + ').\nPara salvar o lançamento das notas, por favor, digite a nota de recuperação de todos os alunos abaixo:\n' + notasRecuperacaoEmBrancoAlunos);
                return (false);
            }

            return (true);
        }

        // cancela o lançamento de notas da turma selecionada e redireciona para o PopUp de seleção de turmas
        // Obs.: Todas as notas digitadas na turma anteriormente selecionada serão perdidas.
        function ConfirmaCancelarLancamentoNotas() {
            if (confirm("Esta operação cancela a digitação de notas referente à turma. Nenhuma nota lançada será salva. Deseja continuar?")) {
                return true;
            }
            return false;
        }

        //fieldSelection plugin: obter/substituir facilmente o texto selecionado
        (function() {
            var fieldSelection = {
                getSelection: function() {
                    var e = this.jquery ? this[0] : this;
                    return (

                    /* mozilla / dom 3.0 */
				('selectionStart' in e && function() {
				    var l = e.selectionEnd - e.selectionStart;
				    return { start: e.selectionStart, end: e.selectionEnd, length: l, text: e.value.substr(e.selectionStart, l) };
				}) ||

                    /* explorer */
				(document.selection && function() {
				    e.focus();
				    var r = document.selection.createRange();
				    if (r == null) {
				        return { start: 0, end: e.value.length, length: 0 }
				    }

				    var re = e.createTextRange();
				    var rc = re.duplicate();
				    re.moveToBookmark(r.getBookmark());
				    rc.setEndPoint('EndToStart', re);

				    return { start: rc.text.length, end: rc.text.length + r.text.length, length: r.text.length, text: r.text };
				}) ||
                    /* browser not supported */
				function() {
				    return { start: 0, end: e.value.length, length: 0 };
				}
			)();
                },
                replaceSelection: function() {
                    var e = this.jquery ? this[0] : this;
                    var text = arguments[0] || '';
                    return (
                    /* mozilla / dom 3.0 */
				('selectionStart' in e && function() {
				    e.value = e.value.substr(0, e.selectionStart) + text + e.value.substr(e.selectionEnd, e.value.length);
				    return this;
				}) ||
                    /* explorer */
				(document.selection && function() {
				    e.focus();
				    document.selection.createRange().text = text;
				    return this;
				}) ||
                    /* browser not supported */
				function() {
				    e.value += text;
				    return this;
				}
			)();
                }
            };
            jQuery.each(fieldSelection, function(i) { jQuery.fn[i] = this; });
        })();
		
    </script>

    <div style="visibility: hidden">
        <asp:HiddenField ID="hdnGradeID" runat="server" />
        <asp:HiddenField ID="hdnGrupoNota" runat="server" />
        <asp:HiddenField ID="hdnNCasasDec" runat="server" />
        <asp:HiddenField ID="hdnFreq" runat="server" />
        <asp:HiddenField ID="hdnSubperiodo" runat="server" />
        <asp:HiddenField ID="hdnTemNota" runat="server" Value="" />
        <asp:HiddenField ID="hdnTemFreq" runat="server" Value="" />
        <asp:HiddenField ID="hdnTotalSubPeriodo" runat="server" Value="" />
    </div>
    <asp:ScriptManagerProxy ID="scriptManager" runat="server" />
    <asp:Panel ID="pnlDadosTurma" GroupingText="Dados da turma" runat="server" Width="700px"
        Wrap="false" HorizontalAlign="Left">
        <table width="90%">
            <tr>
                <td align="right">
                    <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:" runat="server" />
                </td>
                <th colspan="3" align="left">
                    <asp:TextBox ID="tbUnidadeEnsino" ReadOnly="true" runat="server" Width="350px" />
                </th>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTurma" Text="Turma:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbTurma" ReadOnly="true" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="lblAno" Text="Ano:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbAno" runat="server" ReadOnly="true" />
                </td>
                <td align="right">
                    <asp:Label ID="lblPeriodo" Text="Período:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbPeriodo" runat="server" ReadOnly="true" Width="50px" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblEscolaridade" Text="Escolaridade:" runat="server" />
                </td>
                <th colspan="3" align="left">
                    <asp:TextBox ID="tbEscolaridade" ReadOnly="true" runat="server" Width="350px" />
                </th>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTurno" Text="Turno:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbTurno" ReadOnly="true" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="lblMatrizCurricular" Text="Matriz Curricular:" runat="server" />
                </td>
                <td align="right">
                    <asp:TextBox ID="tbMatrizCurricular" ReadOnly="true" runat="server" />
                </td>
                <td align="right" style="width: 85px">
                    <asp:Label ID="lblAnoEscolar" Text="Ano de Escolaridade:" runat="server" Width="100%" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbAnoEscolar" ReadOnly="true" runat="server" Width="50px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlDisciplina" runat="server" GroupingText="Informe os dados para pesquisar notas"
        Width="700px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblDisciplinas" Text="Disciplina:*" SkinID="lblObrigatorio" runat="server"
                        Width="120px" />
                </td>
                <td colspan="3" align="left">
                    <asp:DropDownList ID="cmbdisciplina" runat="server" AutoPostBack="True" DataTextField="nome_disc"
                        DataValueField="disciplina" OnSelectedIndexChanged="cmbdisciplina_SelectedIndexChanged"
                        AppendDataBoundItems="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDisc" runat="server" ControlToValidate="cmbdisciplina"
                        ErrorMessage="Disciplina: Preenchimento obrigatório." InitialValue="" ValidationGroup="buscar"><img 
                            alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPeriodoEscolar" Text="Período Letivo:" runat="server" Visible="false" />
                </td>
                <td align="left">
                </td>
                <td style="width: 25px" />
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnMensagens" runat="server" Width="800px">
        <table>
            <tr>
                <asp:Label ID="lblMensagem" EnableViewState="true" SkinID="lblMensagem" runat="server"
                    Width="800px" />
            </tr>
        </table>
    </asp:Panel>
    <dxtc:ASPxTabControl ID="tcSubperiodo" runat="server" Width="940px" SyncSelectionMode="None"
        Visible="true" TextField="descricao" NameField="subperiodo" OnTabClick="tcSubperiodo_TabClick"
        OnActiveTabChanging="tcSubperiodo_OnActiveTabChanging">
        <TabStyle Wrap="True" Width="105px">
        </TabStyle>
        <ClientSideEvents ActiveTabChanging="function(s, e) {
                e.processOnServer = true;	
	                }" />
    </dxtc:ASPxTabControl>
    <asp:Panel ID="pnlAulas" runat="server" Width="288px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAulasPrevistas" runat="server" SkinID="lblObrigatorio" Text="Aulas Previstas:*"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAulasPrevistas" runat="server" Text="" MaxLength="3" Width="41px"
                        onkeydown="return BloquearCtrl(event, this);" onmousedown="return desabilitaBotaoDireito(event, this);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAulasDadas" runat="server" SkinID="lblObrigatorio" Text="Aulas Dadas:*"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAulasDadas" runat="server" Text="" MaxLength="3" Width="40px"
                        onkeydown="return BloquearCtrl(event, this);" onmousedown="return desabilitaBotaoDireito(event, this);"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlConsolidado" runat="server" Width="500px" Visible="false">
        <table id="Table4" width="800px" runat="server">
            <tr>
                <td>
                    <br />
                    Prezado Professor,
                    <br />
                    <br />
                    A aba Consolidado Bimestral/Trimestral traz as informações da nota final e total de faltas
                    por aluno a cada bimestre/trimestre. Estão sendo indicados em vermelho os alunos com nota
                    final abaixo de 5.0, os alunos sem avaliação e os que possuem frequência menor do
                    que 75%. Esta sinalização facilitará a identificação dos casos que necessitam uma
                    maior atenção por parte do docente e da equipe diretiva da unidade escolar.
                    <br />
                    <br />
                    Para seu controle e acompanhamento será informado nas três últimas colunas o total
                    de notas e faltas acumuladas ao longo dos bimestres/trimestres e o percentual de frequência,
                    com base no número total de aulas dadas.
                    <br />
                    <br />
                    Se deseja imprimir, será aberto um documento com todas as informações disponíveis
                    nesta aba.
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Panel ID="pnlFrequenciaTurma" runat="server">
                        <fieldset style="height: 60px; width: 170px">
                            <legend style="font-size: 12px; color: #0353AB; font-weight: bold;">Frequência da Turma</legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio" Text="Total aulas previstas:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalAulasPrevistas" runat="server" Text="" Style="text-align: center;
                                            background-color: #E0E0E0" ReadOnly="true" Width="40px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="Total aulas dadas:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalAulasDadas" runat="server" Text="" Style="text-align: center;
                                            background-color: #E0E0E0" ReadOnly="true" Width="40px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlMediaTurma" runat="server">
                        <fieldset style="height: 60px;">
                            <legend style="font-size: 12px; color: #0353AB; font-weight: bold;">Média da Turma</legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPrimeiroBimestre" runat="server" SkinID="lblObrigatorio" Text="1ºBimestre/Trimestre:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrimeiroBimestre" runat="server" Style="text-align: center; background-color: #E0E0E0"
                                            Text="" ReadOnly="true" Width="40px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSegundoBimestre" runat="server" SkinID="lblObrigatorio" Text="2ºBimestre/Trimestre:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSegundoBimestre" runat="server" Text="" Style="text-align: center;
                                            background-color: #E0E0E0" ReadOnly="true" Width="40px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTerceiroBimestre" runat="server" SkinID="lblObrigatorio" Text="3ºBimestre/Trimestre:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTerceiroBimestre" runat="server" Text="" Style="text-align: center;
                                            background-color: #E0E0E0" ReadOnly="true" Width="40px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <%--<asp:Label ID="lblQuartoBimestre" runat="server" SkinID="lblObrigatorio" Text="4º Bimestre:"></asp:Label>--%>
                                    </td>
                                    <td>
                                        <%--<asp:TextBox ID="txtQuartoBimestre" runat="server" Text="" Style="text-align: center;
                                            background-color: #E0E0E0" ReadOnly="true" Width="40px"></asp:TextBox>--%>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlMaterialEstudo" runat="server" Width="800px" GroupingText="Material de Estudo Proposto"
        Visible="false">
        <table>
            <tr>
                <td>
                    <asp:CheckBoxList ID="cblMaterialEstudo" runat="server" RepeatDirection="Horizontal"
                        Width="100%">
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlNotasConsolidada" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdConsolidado" ClientInstanceName="grdConsolidado" Visible="true"
            KeyFieldName="aluno" runat="server" EnableCallBacks="false" OnHtmlRowCreated="grdConsolidado_HtmlRowCreated">
            <SettingsPager Mode="ShowAllRecords" />
            <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                AllowMultiSelection="false" AllowGroup="false" />
            <Columns />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsMatriculas" runat="server" TypeName="Techne.Lyceum.RN.HistMatricula"
        SelectMethod="ConsultaHistoricoMatriculaPor">
        <SelectParameters>
            <asp:ControlParameter ControlID="cmbdisciplina" PropertyName="SelectedValue" Name="disciplina" />
            <asp:ControlParameter ControlID="tbTurma" PropertyName="Text" Name="turma" />
            <asp:ControlParameter ControlID="tbAno" PropertyName="Text" Name="ano" />
            <asp:ControlParameter ControlID="tbPeriodo" PropertyName="Text" Name="semestre" />
            <asp:ControlParameter ControlID="tcSubperiodo" PropertyName="ActiveTab.Name" Name="subperiodo" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnMensagemHdn" runat="server" Width="700px" Visible="false">
        <table>
            <tr>
                <td>
                    <span id="dadosDocenteDisciplina" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="mensagem">
                        <strong style="font-size: 12px; color: #0353AB;">Para saber mais sobre recuperação de
                            estudos:</strong>
                        <asp:HyperLink ID="SaibaMais" Font-Size="12px" runat="server" Target="_blank" Text="Clique Aqui."
                            NavigateUrl="http://docenteonline.educacao.rj.gov.br/NOVODOCENTE/ARQUIVOS/SAIBA%20MAIS.PDF"></asp:HyperLink>
                        <br />
                        <div id="msg" runat="server" style="font-weight: bold; font-size: 12px; color: #0353AB;">
                            &nbsp;</div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlAviso" runat="server" Width="700px">
        <table>
            <tr>
                <td>
                    <span id="Span1" runat="server"><span style='color: Red'>Alterações realizadas nas notas
                        não interferem na situação final do aluno (Aprovações/Reprovações).<br />
                        Para realizar acertos na situação final do aluno, acesse a funcionalidade “Histórico
                        de Matrículas.<br />
                    </span></span>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlGridMatriculas" runat="server">
        <dxwgv:ASPxGridView ID="grdMatriculas" ClientInstanceName="grdMatriculas" KeyFieldName="aluno"
            DataSourceID="odsMatriculas" runat="server" EnableCallBacks="false" OnHtmlRowCreated="grdMatriculas_HtmlRowCreated">
            <SettingsPager Mode="ShowAllRecords" />
            <SettingsBehavior AllowDragDrop="false" ProcessSelectionChangedOnServer="false" AllowMultiSelection="false"
                AllowGroup="false" AllowSort="false" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Nº" FieldName="num_chamada" Name="num_chamada"
                    UnboundType="Decimal">
                    <CellStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome_compl" Name="nome_compl"
                    UnboundType="String">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Sit. Aluno" FieldName="sit_matricula" Name="sit_matricula"
                    UnboundType="String" Visible="true">
                    <CellStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Nota" FieldName="MÉDIA" Name="MÉDIA" UnboundType="String">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:TextBox ID="txtConceito" runat="server" CssClass="input-nota" Text='<%# Bind("MÉDIA") %>'
                            Style="text-align: center; width: 35px;" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Faltas" Name="Faltas" FieldName="faltas" UnboundType="Integer">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:TextBox ID="txtFrequencia" runat="server" CssClass="input-faltas" Text='<%# Bind("faltas") %>'
                            Style="text-align: center; width: 35px;" MaxLength="3" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="recuperacao_paralela" Caption="Recuperação de estudos"
                    Name="recuperacao_paralela" Width="100px">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkRecuperacao" CssClass="chkrecuperacaoparalela" runat="server"
                            Checked='<%# this.VerificarCheck(Eval("recuperacao_paralela")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Nota de recuperação de estudos" FieldName="NotaRecuperacao"
                    Name="NotaRecuperacao" UnboundType="String">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:TextBox ID="txtNotaRecuperacao" CssClass="inputnotarecuperacao" runat="server"
                            Text='<%# Bind("NotaRecuperacao") %>' Style="text-align: center; width: 35px"
                            Enabled="false" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Nota final do bimestre" FieldName="NotaFinal"
                    Name="NotaFinal" UnboundType="String">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:TextBox ID="txtNotaFinal" CssClass="inputconceito" runat="server" Style="text-align: center;
                            width: 35px" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="sem_avaliacao" Caption="Sem Avaliação" Name="sem_avaliacao">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <asp:CheckBox ID="chkSemAvaliacao" runat="server" Checked='<%# this.VerificarCheck(Eval("sem_avaliacao")) %>' />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Justificativa" FieldName="justificativa" Name="justificativa"
                    UnboundType="String">
                    <DataItemTemplate>
                        <asp:HiddenField runat="server" ID="hfJustificativa" Value='<%# Bind("justificativa") %>' />
                        <asp:DropDownList ID="cmbJustificativa" runat="server" DataSourceID="odsMotivoSemNotaId"
                            DataTextField="DESCR" DataValueField="ITEM" Width="200px">
                        </asp:DropDownList>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="aluno" Name="aluno" UnboundType="String"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ordem" FieldName="ORDEM" Name="ORDEM" UnboundType="String"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsMotivoSemNotaId" runat="server" TypeName="Techne.Lyceum.RN.DeclaracaoSemNota"
        SelectMethod="ListaMotivoSemNotaId"></asp:ObjectDataSource>
    <br />
    <br />
    <asp:Panel ID="botoes" runat="server">
        <table>
            <tr>
                <td align="left">
                    <asp:ImageButton ID="btnImprimirComp" runat="server" SkinID="Imprimir" ImageAlign="Right" />
                </td>
                <td align="left">
                    <asp:ImageButton ID="btnImprimirConsolidado" runat="server" SkinID="Imprimir" ImageAlign="Right"
                        Visible="false" />
                </td>
                <td align="left">
                    <asp:ImageButton ID="btnSalvar" OnClick="btnSalvar_Click" SkinID="BcSalva" runat="server"
                        Visible="true" OnClientClick="return ValidaPreenchimentoTotal();" />
                </td>
                <td align="left">
                    <asp:ImageButton ID="btnCancelar" runat="server" SkinID="Cancelar" OnClick="btnCancelarVoltar_Click"
                        OnClientClick="return ConfirmaCancelarLancamentoNotas();" Visible="false" />
                </td>
                <td align="left">
                    <asp:ImageButton ID="btnVoltar" runat="server" SkinID="Voltar" OnClick="btnCancelarVoltar_Click"
                        Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
