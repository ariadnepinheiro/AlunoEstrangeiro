/*************************
LANCAMENTO DE NOTAS - LISTA - INICIO
*************************/

// constantes e enumerações usadas para a validação dos caracteres digitados pelo usuário
var gblTeclas = { "Backspace": 8, "Tab": 9, "Left": 37, "Up": 38, "Right": 39, "Down": 40, "Del": 46, "End": 35, "Home": 36, "Shift": 16, "Enter": 13 };

//Cache de objetos jquery das linhas de lancamentos matriculados
var jqListaTRLancamentosMatriculados = null
var gblAlteracaoDetectada = false;

var FREQUENCIA_MINIMA = 40;
var PERCENTUAL_FREQUENCIA = 75;

$(document).ready(function () {
    jqListaTRLancamentosMatriculados = $('.tabela-padrao table tbody tr:not(.cancelado)');
    DesabilitaLancamento();
    DesabilitaCancelados();
    TrataLancamentoSemAvaliacaoGeral();
    CarregaEventos();
    IniciaContagemRegressiva();
    ConfirmaAberturaDeFilipetaLancamento();
    OcultaMensagemCurriculoMinimoCompleto();
    DesabilitaNotaRecuperacao();
    HabilitaNotaRecuperacaoSeChecado();
    TrataJustificativa();
    DesabilitaFocus();
    InputNotaFinalFormata();
    DesabilitaNotaProvaEFalta();

    $("#btnVoltar").click(function () {
        window.history.back();
    });
});

function InputNotaFinalFormata() {

    var jqInputNota = jqListaTRLancamentosMatriculados.find('input:text.inputconceito');

    for (var i = 0; i < jqInputNota.length; i++) {
        var oval = $(jqInputNota[i]).val().replace(",", ".");

        if (oval.indexOf('.') == oval.length - 2) {
            $(jqInputNota[i]).val(oval.replace(".", ",") + "0");
        }
    }
}

function DesabilitaFocus() {
    $(':checkbox').each(function () {
        this.tabIndex = -1;
    });

    $('input:text.inputconceito').each(function () {

        this.tabIndex = -1;
    });
}



function OcultaMensagemCurriculoMinimoCompleto() {
    //Espera 3 segundos para ocultar a mensagem de curriculo minimo preenchido completo
    $('.mensagem-curriculo-minimo.Completo')
		.delay(3000)
		.fadeOut(1000);
}

//Desabilita nota recuperação
function DesabilitaNotaRecuperacao() {
    $('.tabela-padrao table').find('input:text.inputnotarecuperacao')
		.prop('disabled', true)
		.filter('input:text')
		.css('background-color', '#E0E0E0');
}

//Desabilita nota e falta de acordo com a frequencia e nota
function DesabilitaNotaProvaEFalta() {
    var jqInputTemNota = $('.mensagemFrequencia').find('input:hidden.inputTemNota');
    var jqInputTemFrequencia = $('.mensagemFrequencia').find('input:hidden.inputTemFrequencia');
    var jqInputNota = $('.tabela-padrao table').find('input:text.input-nota');
    var jqInputFalta = $('.tabela-padrao table').find('input:text.input-faltas');
    var jqInputNotaFinal = $('.tabela-padrao table').find('input:text.inputconceito');
    var jqInputSemAvaliacao = $('.tabela-padrao table').find('input:checkbox.input-sem-avaliacao');
    var jqInputRecuperacaoParalela = $('.tabela-padrao table').find('input:checkbox.input-recuperacao-paralela');
    var jqInputNotaRecuperacao = $('.tabela-padrao table').find('input:text.inputnotarecuperacao');
    var jqSelectJustificativa = $('.tabela-padrao table').find('.inputLancamento100');

    //Desabilita nota
    if ($(jqInputTemNota).val() == "N" && $(jqInputTemFrequencia).val() == "S") {
        jqInputNota.prop('disabled', true);
        jqInputNota.css('background-color', '#E0E0E0');
        jqInputNotaRecuperacao.prop('disabled', true);
        jqInputNotaRecuperacao.css('background-color', '#E0E0E0');
        jqInputSemAvaliacao.prop('checked', false);
        jqInputSemAvaliacao.val('').prop('disabled', true);
        jqInputSemAvaliacao.css('background-color', '#E0E0E0');
        jqInputRecuperacaoParalela.prop('checked', false);
        jqInputRecuperacaoParalela.val('').prop('disabled', true);
        jqInputRecuperacaoParalela.css('background-color', '#E0E0E0');
        for (var i = 0; i < jqSelectJustificativa.length; i++) {
            jqSelectJustificativa.get(i).selectedIndex = 0;
        }
        jqSelectJustificativa.prop('disabled', true);
        jqSelectJustificativa.css('background-color', '#E0E0E0');
        jqInputNotaFinal.prop('disabled', true);
        jqInputNotaFinal.css('background-color', '#E0E0E0');
    }
    //Desabilita falta, Aulas Previstas e Aulas Dadas.
    else if ($(jqInputTemNota).val() == "S" && $(jqInputTemFrequencia).val() == "N") {
        jqInputFalta.prop('disabled', true);
        jqInputFalta.css('background-color', '#E0E0E0');
        $('#AulasPrevistas, #AulasDadas')
		.prop('readonly', 'readonly')
		.css('background-color', '#E0E0E0')
        .val('0');
    } //Desabilita nota, falta, Aulas Previstas, Aulas Dadas e o botão salvar.
    else if ($(jqInputTemNota).val() == "N" && $(jqInputTemFrequencia).val() == "N") {
        jqInputNota.prop('disabled', true);
        jqInputNota.css('background-color', '#E0E0E0');
        jqInputFalta.prop('disabled', true);
        jqInputFalta.css('background-color', '#E0E0E0');
        jqInputSemAvaliacao.val('').prop('disabled', true);
        jqInputSemAvaliacao.css('background-color', '#E0E0E0');
        jqInputSemAvaliacao.prop('checked', false);
        $('#AulasPrevistas, #AulasDadas')
		.prop('readonly', 'readonly')
		.css('background-color', '#E0E0E0')
        .val('0');
        $('#bt-salvar-lancamento')
		.prop('disabled', true);
        $('#bt-imprimir-filipeta')
		.prop('disabled', true);
        jqInputRecuperacaoParalela.prop('checked', false);
        jqInputRecuperacaoParalela.val('').prop('disabled', true);
        jqInputRecuperacaoParalela.css('background-color', '#E0E0E0');
        jqInputNotaRecuperacao.prop('disabled', true);
        jqInputNotaRecuperacao.css('background-color', '#E0E0E0');
        for (var i = 0; i < jqSelectJustificativa.length; i++) {
            jqSelectJustificativa.get(i).selectedIndex = 0;
        }
        jqSelectJustificativa.prop('disabled', true);
        jqSelectJustificativa.css('background-color', '#E0E0E0');
        jqInputNotaFinal.prop('disabled', true);
        jqInputNotaFinal.css('background-color', '#E0E0E0');
    }
}

function HabilitaNotaRecuperacaoSeChecado() {

    var jqInputNota = jqListaTRLancamentosMatriculados.find('input:text.inputnotarecuperacao');
    var oChkRecuperacao = jqListaTRLancamentosMatriculados.find('input:checkbox.input-recuperacao-paralela');

    for (var i = 0; i < oChkRecuperacao.length; i++) {
        if (oChkRecuperacao[i].checked) {
            jqInputNota[i].disabled = false;
            $(jqInputNota[i]).css('background-color', '#FFF');

        }
    }
}

// adiciona eventos nas células de notas da Grid para:
// 1. navegação com direcionais do teclado
// 2. validação dos caracteres digitados na célula

function CarregaEventos() {

    $('.conteudo input, .conteudo select').on('keydown.detectaAlteracao', function () {
        gblAlteracaoDetectada = true;
        $('.conteudo input, .conteudo select').off('keydown.detectaAlteracao');
    });

    $('#tab-bimestres').on('click', 'li', function () {
        var subperiodo = $(this).data('subperiodo');
        var consolidado = ($(this).data('consolidado') == "S");
        var jqFormulario = $('#frm-postback');

        $('#FMB-ExibeConsolidado').val(consolidado);
        $('#FMB-Subperiodo').val(subperiodo);

        jqFormulario.prop('action', '<%=Url.Action("Lista","LancamentoNotas") %>');
        jqFormulario.prop('target', '');
        jqFormulario.submit();
    });

    $('.tabela-padrao table tbody tr.cancelado td.situacao').on('click', function () {
        $('.tabela-padrao table tbody .descricao-situacao:visible').hide();

        jqBalao = $(this).find('.descricao-situacao').show();
        if (jqBalao.data('timer')) {
            clearTimeout(jqBalao.data('timer'));
        }
        jqBalao.data('timer', setTimeout(function () { jqBalao.fadeOut(500); }, 4000));
    });

    $('.tabela-padrao table tbody .descricao-situacao').on('mouseleave', function () {
        if ($(this).data('timer')) { clearTimeout(jqBalao.data('timer')); }
        $(this).fadeOut(500);
    });

    $('#bt-imprimir-filipeta').on('click', function () {
        var subperiodo = ObtemSubperiodoSelecionado();

        var jqFormulario = $('#frm-postback');
        jqFormulario.prop('action', '<%=Url.Content("~/Relatorios.aspx?relatorio=Filipeta&grupo=dol")%>&Chave=' + gblChaveImpressaoFilipeta);
        jqFormulario.prop('target', 'popFilipeta');

        var janela = window.open('<%=Url.Action("GeraFilipeta","LancamentoNotas") %>', 'popFilipeta', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');

        $('#FMB-Subperiodo').val(subperiodo);
        jqFormulario.submit();

        return false;
    });
    $('#bt-imprimir-filipeta-consolidado').on('click', function () {

        var jqFormulario = $('#frm-postback');
        jqFormulario.prop('action', '<%=Url.Content("~/Relatorios.aspx?relatorio=FilipetaConsolidada&grupo=dol")%>&Chave=' + gblChaveImpressaoFilipeta);
        jqFormulario.prop('target', 'popFilipeta');

        var janela = window.open('<%=Url.Action("GeraFilipeta","LancamentoNotas") %>', 'popFilipeta', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');

        jqFormulario.submit();

        return false;
    });
    $('#bt-solicitar-reabertura').on('click', function () {
        $(this).hide();
        $('#painel-justificativa').show();
        $("html, body").animate({ scrollTop: $(document).height() }, "slow");
    });

    $('#bt-solicitar-reabertura').on('click', function () {
        $(this).hide();
        $('#painel-justificativa').show();
    });

    $('#bt-cancelar-justificativa').on('click', function () {
        $('#txa-justificativa-reabertura-bimestre').val('');
        $('#bt-solicitar-reabertura').show();
        $('#painel-justificativa').hide();
    });

    $('#bt-confirmar-justificativa').on('click', function () {
        var textoJustificativa = $.trim($('#txa-justificativa-reabertura-bimestre').val());
        //Validar texto...
        var bErro = false;

        if (textoJustificativa.length < 5 || textoJustificativa > 250) {
            bErro = true;
            alert("O campo justificativa é obrigatório e precisa ter de 5 a 250 caracteres!");
            RetiraBloqueio();
        }

        if (!bErro) {
            var subperiodo = ObtemSubperiodoSelecionado();

            $('#FMB-Justificativa-Reabertura').val(textoJustificativa);
            $('#FMB-Subperiodo').val(subperiodo);
            var jqFormulario = $('#frm-postback');
            jqFormulario.prop('action', '<%=Url.Action("SolicitaReaberturaBimestre","LancamentoNotas") %>');
            jqFormulario.submit();
        }
    });

    //Bloqueia paste nos campos input
    $('.tabela-padrao table tbody input:text').bind('paste', function (e) {
        return false;
    });

    $('#bt-salvar-lancamento').on('click', function () {
        //Inputs do tipo button, já disparam o form-submit se nao ocorrer um "return false";

        var oRetorno = ValidaPreenchimentoTotal();
        if (oRetorno.erro) {
            alert(oRetorno.mensagemAuxiliar);
            RetiraBloqueio();
            return false;
        }

        //Preparando para envio:

        //Move os dados do formulario de postback para o foprmulario de envio de nota.
        var sHTMInputs = $('#frm-postback').html();
        $('#frm-postback').empty() //Limpa o form antigo para nao ter duplicação de ids
        $('#frm-lancamento').prepend(sHTMInputs);

        var subperiodo = ObtemSubperiodoSelecionado();
        $('#FMB-Subperiodo').val(subperiodo);

        $('#frm-lancamento').submit();  //Necessário forçar para IE7 ou anterior

        return true; //submit do form
    });

    //Trata as possíveis saídas da interface
    $('#bt-cancelar-lancamento') //Cancelar
	.add('#turma-selecionada .bt-mudar-turma a') //Mudar turma
	.add('#tab-bimestres li') //Tabs de bimestres
	.add('body > .cabecalho a') //Menu superior
		.on('click', function () {
		    var confirmado = true;
		    if (gblAlteracaoDetectada) {

		        confirmado = confirm('<%= Resources.Recurso.LancamentoNotasLista_MensagemSairSemSalvar %>');
		        RetiraBloqueio();
		    }

		    if (confirmado && $(this).is('#bt-cancelar-lancamento')) {
		        this.form.action = '<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "LancamentoNotas" })%>';
		        this.form.submit(); //Necessário forçar para IE7 ou anterior
		    }

		    //Inputs do tipo button, já disparam o form-submit se nao ocorrer um "return false";
		    return confirmado;
		});

    //Permite apenas numericos nos campos de frequencia da turma e faltas
    $('#AulasPrevistas, #AulasDadas').numeric();
    jqListaTRLancamentosMatriculados.find('input:text.input-faltas').numeric();

    jqListaTRLancamentosMatriculados.find('input:checkbox.input-sem-avaliacao').on('click', function () {
        TrataLancamentoSemAvaliacao(this);
    });

    jqListaTRLancamentosMatriculados.find('input:checkbox.input-recuperacao-paralela').on('click', function () {
        TrataLancamentoSemAvaliacaoRecuperacao(this);
    });

    // Permite navegar pelas notas e faltas usando setas direcionais
    jqListaTRLancamentosMatriculados.find('[data-navegar]:enabled').on('keydown', function (e) {
        if ($(this).prop('readonly') == false) {
            NavegaProximo(e, $(this));
        }
    });

    jqInputNotas = jqListaTRLancamentosMatriculados.find('input:text.input-nota-consolidado').each(function () {

        var notaCelula = this;
        CalculaNotaFinalConsolidado(notaCelula);

    });
    jqInputNotas = jqListaTRLancamentosMatriculados.find('input:text.input-nota-acumulada-consolidado').each(function () {

        var notaCelula = this;
        CalculaNotaFrequenciaAcumuladaConsolidado(notaCelula);

    });
    // registra eventos nas células das notas que possuem atributo para validação numérica:
    // 1. evento 'keypress' que permite apenas números decimais positivos e 'SN'
    // 2. evento 'change' que valida o novo valor da célula
    // 3. evento 'blur' que adiciona o ',0' às células com números inteiros positivos
    jqInputNotas = jqListaTRLancamentosMatriculados.find('input:text.input-nota').each(function () {
        var notaCelula = this;

        ValidaCorrige(notaCelula);
        HabilitaRecParalela(notaCelula);

        //Trava eventos se o campo estiver somente leitura
        if ($(notaCelula).prop('readonly') == false) {

            $(notaCelula)
				.on('keypress', function (e) {
				    var permiteCaracter = ValidaKeyPress(notaCelula, e);
				    e.stopImmediatePropagation();
				    return permiteCaracter;
				})
				.on('change', function (e) {
				    ValidaCorrige(notaCelula);
				    e.stopImmediatePropagation();
				})
				.on('blur', function (e) {
				    InsereVirgulaZero(notaCelula);
				    HabilitaRecParalelaConf(notaCelula);
				    CalculaNotaFinal(notaCelula);
				    e.stopImmediatePropagation();
				})
				.on('input paste', function (e) {
				    return false;
				})
				.on('contextmenu', function (e) {
				    return false;
				});
        }
    });

    // registra eventos nas células das notas que possuem atributo para validação numérica:
    // 1. evento 'keypress' que permite apenas números decimais positivos e 'SN'
    // 2. evento 'change' que valida o novo valor da célula
    // 3. evento 'blur' que adiciona o ',0' às células com números inteiros positivos
    jqInputNotasRecuperacao = jqListaTRLancamentosMatriculados.find('input:text.inputnotarecuperacao').each(function () {
        var notaCelula = this;

        ValidaCorrige(notaCelula);

        //Trava eventos se o campo estiver somente leitura
        if ($(notaCelula).prop('readonly') == false) {

            $(notaCelula)
				    .on('keypress', function (e) {
				        var permiteCaracter = ValidaKeyPress(notaCelula, e);
				        e.stopImmediatePropagation();
				        return permiteCaracter;
				    })
				    .on('change', function (e) {
				        ValidaCorrige(notaCelula);
				        e.stopImmediatePropagation();
				    })
				    .on('blur', function (e) {
				        InsereVirgulaZero(notaCelula);
				        CalculaNotaFinal(notaCelula);
				        e.stopImmediatePropagation();
				    })
				    .on('input paste', function (e) {
				        return false;
				    })
				    .on('contextmenu', function (e) {
				        return false;
				    });
        }
    });

    // registra eventos nas células das notas que possuem atributo para validação numérica:
    // 1. evento 'keypress' que permite apenas números decimais positivos e 'SN'
    // 2. evento 'change' que valida o novo valor da célula
    // 3. evento 'blur' que adiciona o ',0' às células com números inteiros positivos
    jqInputNotasRecuperacao = jqListaTRLancamentosMatriculados.find('input:text.inputconceito').each(function () {
        var notaCelula = this;

        ValidaCorrige(notaCelula);

        //Trava eventos se o campo estiver somente leitura
        if ($(notaCelula).prop('readonly') == false) {

            $(notaCelula)
				        .on('keypress', function (e) {
				            var permiteCaracter = ValidaKeyPress(notaCelula, e);
				            e.stopImmediatePropagation();
				            return permiteCaracter;
				        })
				        .on('change', function (e) {
				            ValidaCorrige(notaCelula);
				            e.stopImmediatePropagation();
				        })
				        .on('blur', function (e) {
				            InsereVirgulaZero(notaCelula);
				            e.stopImmediatePropagation();
				        })
				        .on('input paste', function (e) {
				            return false;
				        })
				        .on('contextmenu', function (e) {
				            return false;
				        });
        }
    });

    $('#link-curriculo-minimo').on('click', function () {
        var subperiodo = ObtemSubperiodoSelecionado();
        var jqFormulario = $('#frm-postback');
        jqFormulario.prop('action', '<%=Url.Action("Lista","RespostaCurriculoMinimo") %>');
        jqFormulario.prop('target', '');

        $('#FMB-Subperiodo').val(subperiodo);
        jqFormulario.submit();

        return false;
    });
}

function CalculaNotaFinal(jqObject) {
    var jqTRLancamento = $(jqObject).closest('tr');
    var jqInputNota = jqTRLancamento.find('input:text.input-nota');
    var jqInputNotaRecuperacao = jqTRLancamento.find('input:text.inputnotarecuperacao');
    var jqInputNotaFinalBimestre = jqTRLancamento.find('input:text.inputconceito');
    var NotaRecuperacao = parseFloat($(jqInputNotaRecuperacao).val().replace(",", "."));
    var Nota = parseFloat($(jqInputNota).val().replace(",", "."));

    $(jqInputNotaFinalBimestre).val(jqInputNota.val());
    if (Nota < NotaRecuperacao) {
        $(jqInputNotaFinalBimestre).val(jqInputNotaRecuperacao.val());
    }

    jqDadosTurmaDisciplina = $('#dados-turma-disciplina');
    var notaMax = jqDadosTurmaDisciplina.data('nota-maxima').toString();
    var valor = parseFloat($(jqInputNotaFinalBimestre).val().replace(",", "."));
    $(jqInputNotaFinalBimestre).css({ "color": (valor >= notaMax / 2.0) ? "blue" : "red" });
    if (parseFloat(jqInputNota.val()) > notaMax) {
        alert("A nota não pode ser maior que " + notaMax);
        RetiraBloqueio();
        jqInputNota.val('').prop('text', '');
        jqInputNotaFinalBimestre.val('').prop('text', '');
    }
    if (parseFloat(jqInputNotaRecuperacao.val()) > notaMax) {
        alert("A nota não pode ser maior que " + notaMax);
        RetiraBloqueio();
        jqInputNotaRecuperacao.val('').prop('text', '');
        jqInputNotaFinalBimestre.val('').prop('text', '');
    }
}

function CalculaNotaFinalConsolidado(jqObject) {

    var jqTDLancamento = $(jqObject).closest('td');
    var jqInputNota = jqTDLancamento.find('input:text.input-nota-consolidado');
    var Nota = parseFloat($(jqInputNota).val().replace(",", "."));

    jqDadosTurmaDisciplina = $('#dados-turma-disciplina');
    var notaMax = jqDadosTurmaDisciplina.data('nota-maxima').toString();

    var valor = parseFloat($(jqInputNota).val().replace(",", "."));
    $(jqInputNota).css({ "color": (valor >= notaMax / 2.0) ? "blue" : "red" });
}


function CalculaNotaFrequenciaAcumuladaConsolidado(jqObject) {

    var jqTRLancamento = $(jqObject).closest('tr');
    var jqInputNota = jqTRLancamento.find('input:text.input-nota-acumulada-consolidado');
    var jqInputFrequencia = jqTRLancamento.find('input:text.input-frequencia-acumulada-consolidado');

    var Nota = parseFloat($(jqInputNota).val().replace(",", "."));
    var Frequencia = parseFloat($(jqInputFrequencia).val().replace(",", "."));

    var QtdeBimestre = $('#quantidade-bimestre').val();

    jqDadosTurmaDisciplina = $('#dados-turma-disciplina');
    var notaMax = jqDadosTurmaDisciplina.data('nota-maxima').toString();

    $(jqInputNota).css({ "color": (Nota >= (notaMax * QtdeBimestre) / 2.0) ? "blue" : "red" });

    $(jqInputFrequencia).css({ "color": (Frequencia >= PERCENTUAL_FREQUENCIA) ? "blue" : "red" });

}


function DesabilitaLancamento() {
    //Desabilita todos os inputs e combos da tabela de lançamentos
    $('#AulasPrevistas, #AulasDadas')
		.prop('readonly', 'readonly')
		.css('background-color', '#E0E0E0');

    $('.tabela-padrao table').find('input:checkbox, input:text, select')
		.prop('disabled', true)
		.filter('input:text')
		.prop('disabled', false)
		.prop('readonly', 'readonly')
		.css('background-color', '#E0E0E0');
    return true;
}

function DesabilitaCancelados() {
    //Desabilita todos os inputs de alunos não elegíveis para lançamento na tabela
    $('.tabela-padrao table tbody tr.cancelado').find('input, select')
		.prop('disabled', true)
		.filter('input:text')
		.css('background-color', '#E0E0E0');
}

function ObtemSubperiodoSelecionado() {
    var subperiodo = $('#tab-bimestres li.selecionado').data('subperiodo');
    return subperiodo;
}


function TrataLancamentoSemAvaliacaoGeral() {
    jqListaTRLancamentosMatriculados.find('input:checkbox.input-sem-avaliacao:enabled').each(function () {
        TrataLancamentoSemAvaliacao(this);
    });
}

function TrataLancamentoSemAvaliacao(oChkSemAvaliacao) {
    //Nao implementado
    var jqTRLancamento = $(oChkSemAvaliacao).closest('tr');

    var jqInputNota = jqTRLancamento.find('input:text.input-nota');
    var jqInputConceito = jqTRLancamento.find('input:text.inputconceito');
    var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');
    var jqCmbJustificativa = jqTRLancamento.find('select');
    var jqChkRecParalela = jqTRLancamento.find('input:checkbox.input-recuperacao-paralela');

    if (oChkSemAvaliacao.checked) {
        jqInputNota.val('').prop('disabled', true);
        jqInputNota.css('background-color', '#E0E0E0');
        jqInputConceito.val('').prop('disabled', true);
        jqInputConceito.css('background-color', '#E0E0E0');
        jqInputRecParalela.prop('disabled', true);
        jqInputRecParalela.css('background-color', '#E0E0E0');
        jqCmbJustificativa.prop('disabled', false);
        jqChkRecParalela.prop('checked', false);
        jqChkRecParalela.prop('disabled', true);
    }
    else {
        jqInputNota.prop('disabled', false);
        jqInputNota.css('background-color', '');
        jqInputConceito.prop('disabled', false);
        jqInputConceito.css('background-color', '');
        jqInputRecParalela.prop('disabled', true);
        jqInputRecParalela.css('background-color', '#E0E0E0');
        jqCmbJustificativa.get(0).selectedIndex = 0;
        jqCmbJustificativa.prop('title', '');
        jqCmbJustificativa.prop('disabled', true);
        jqChkRecParalela.prop('disabled', false);
    }

}


// Trata as justificativas;
function TrataJustificativa() {
    var jqInputLicenca = jqListaTRLancamentosMatriculados.find(".inputLicenca");
    var jqInputLancamento = jqListaTRLancamentosMatriculados.find(".inputLancamento100");
    var jqInputFaltas = jqListaTRLancamentosMatriculados.find(".input-faltas");
    var aulasDadas = $("#AulasDadas");

    var msg = "Declaro ter conhecimento do instrumento que formaliza o afastamento e solicitei à equipe pedagógica a aplicação residencial dos instrumentos de avaliação";
    var msgDependencia = "Declaro que solicitei à equipe pedagógica da unidade o contato com o aluno para que entregue o trabalho até o próximo bimestre";

    for (var i = 0; i < jqInputLicenca.length; i++) {
        if (jqInputLicenca[i].value == "True") {
            $(jqInputLancamento[i]).change(function () {
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

        $(jqInputLancamento[i]).change({ input: jqInputFaltas[i] }, function (e) {

            if (this.value == "2") {
                var percentual = e.data.input.value / aulasDadas.val() * 100;
                // Outros.
                if (percentual >= 100 - FREQUENCIA_MINIMA) {
                    if (!confirm("Declaro que alertei a unidade escolar a respeito das faltas do aluno para análise e preparação da ficha de comunicação do aluno infrequente (FICAI)")) {
                        this.value = "";
                    }
                } else {
                    this.value = "";
                    alert("O professor deverá lançar valor da nota do(s) instrumento(s) aplicado(s) durante período em que o aluno esteve frequente");
                    RetiraBloqueio();
                }
            }

            if (this.value != "")
                this.title = $(this).find('option:selected').text();
            else
                this.title = "";

        })

        $(jqInputLancamento[i]).change(function () {
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

function TrataLancamentoSemAvaliacaoRecuperacao(oChkSemAvaliacao) {
    //Nao implementado
    var jqTRLancamento = $(oChkSemAvaliacao).closest('tr');

    var jqInputNota = jqTRLancamento.find('input:text.inputnotarecuperacao');

    if (oChkSemAvaliacao.checked) {
        jqInputNota.prop('disabled', false);
        jqInputNota.css('background-color', '');
    }
    else {
        jqInputNota.val('').prop('disabled', true);
        jqInputNota.css('background-color', '#E0E0E0');
    }

}

function ValidaPreenchimentoTotal() {
    var mensagemAuxiliar = '';
    var bErro = false;

    //Procura se pelo menos um material estudo foi preenchido
    var materialEstudoMarcado = $('#modalidade-medio.material-estudo').is(':checked');
    if (materialEstudoMarcado == false) {
        mensagemAuxiliar += '\n Preencha material estudo. \n\n ';
        bErro = true;
    }

    //Procura por itens de preenchimento inválido
    var ListaAlunoNotas = jqListaTRLancamentosMatriculados.filter(function () {
        var semAvaliacao = $(this).find('input:checkbox.input-sem-avaliacao').is(':checked');
        var comAvaliacao = !semAvaliacao;
        var valorNotaVazio = false;
        var itemInvalido = false;

        if (comAvaliacao) {
            valorNotaVazio = $.trim($(this).find('input:text.input-nota').val()).length == 0;
            if (valorNotaVazio) {
                itemInvalido = true;
            }
        }
        return itemInvalido;
    });


    var notasEmBranco = ListaAlunoNotas.length;
    var notasEmBrancoAlunos = "";

    for (var i = 0; i < ListaAlunoNotas.length; i++) {
        notasEmBrancoAlunos += "\n" + ($('input:eq(1)', $(ListaAlunoNotas[i])).val());
    }



    //Procura por itens de preenchimento inválido recuperação
    var notasEmBrancoRecuperacao = jqListaTRLancamentosMatriculados.filter(function () {
        var semAvaliacaoRecuperacao = $(this).find('input:checkbox.input-recuperacao-paralela').is(':checked');
        var comAvaliacaoRecuperacao = !semAvaliacaoRecuperacao;
        var valorNotaVazioRecuperacao = false;
        var itemInvalidoRecuperacao = false;

        if (comAvaliacaoRecuperacao) {
            valorNotaVazioRecuperacao = $.trim($(this).find('input:text.inputnotarecuperacao').val()).length == 0;
            if (valorNotaVazioRecuperacao) {
                itemInvalidoRecuperacao = true;
            }
        }
        return itemInvalidoRecuperacao;
    }).length;

    var ListaAlunoFaltas = jqListaTRLancamentosMatriculados.filter(function () {
        var itemInvalido = false;
        var valorFaltasVazio = $.trim($(this).find('input:text.input-faltas').val()).length == 0;
        if (valorFaltasVazio) {
            itemInvalido = true;
        }
        return itemInvalido;
    });


    var faltasEmBranco = ListaAlunoFaltas.length;
    var faltasEmBrancoAlunos = "";

    for (var i = 0; i < ListaAlunoFaltas.length; i++) {
        faltasEmBrancoAlunos += "\n" + ($('input:eq(1)', $(ListaAlunoFaltas[i])).val());
    }

    var jqInputTemNota = $('.mensagemFrequencia').find('input:hidden.inputTemNota');
    var jqInputTemFrequencia = $('.mensagemFrequencia').find('input:hidden.inputTemFrequencia');

    if ((notasEmBranco > 0 || faltasEmBranco > 0) && $(jqInputTemNota).val() == "S" && $(jqInputTemFrequencia).val() == "S") {
        mensagemAuxiliar += '\nO lançamento foi realizado de forma parcial. Preencha os dados dos alunos indicados abaixo, caso contrário perderá todos os dados lançados. Após o preenchimento das pendências, salve os dados novamente.\n\n ';
    }

    if (notasEmBranco > 0 && $(jqInputTemNota).val() == "S") {
        mensagemAuxiliar += '\n' + notasEmBranco;
        mensagemAuxiliar += (notasEmBranco == 1) ? ' nota pendente' : ' notas pendentes';
        mensagemAuxiliar += notasEmBrancoAlunos + '\n';

        bErro = true;
    }

    if (faltasEmBranco > 0 && $(jqInputTemFrequencia).val() == "S") {
        mensagemAuxiliar += '\n' + faltasEmBranco;
        mensagemAuxiliar += (faltasEmBranco == 1) ? ' falta pendente' : ' faltas pendentes';
        mensagemAuxiliar += faltasEmBrancoAlunos;

        bErro = true;
    }

    var boxes = $('input:checkbox.input-recuperacao-paralela:checked');

    var ListaAlunoRecuperacaoParalelas = jqListaTRLancamentosMatriculados.filter(function () {
        var itemInvalido = false;
        var CheckRecuperacaoParalela = $(this).find('input:checkbox.input-recuperacao-paralela').is(':checked');
        if (CheckRecuperacaoParalela) {
            itemInvalido = true;
        }
        return itemInvalido;
    });

    var validarCheckRecuperacaoParalela = ListaAlunoRecuperacaoParalelas.length;

    var paralelaEmBrancoAlunos = "";
    var totalParalelaEmBranco = 0;

    for (var i = 0; i < ListaAlunoRecuperacaoParalelas.length; i++) {
        if ($.trim($('input:text.inputnotarecuperacao', $(ListaAlunoRecuperacaoParalelas[i])).val()).length == 0) {
            paralelaEmBrancoAlunos += "\n" + ($('input:eq(1)', $(ListaAlunoRecuperacaoParalelas[i])).val());
            totalParalelaEmBranco += 1;
        }
    }

    var validarTextRecuperacaoParalela = jqListaTRLancamentosMatriculados.filter(function () {
        var itemInvalido = false;
        var TextRecuperacaoParalela = $.trim($(this).find('input:text.inputnotarecuperacao').val()).length == 0;
        if (TextRecuperacaoParalela) {
            itemInvalido = true;
        }
        return itemInvalido;
    }).length;

    if (validarCheckRecuperacaoParalela > 0 && totalParalelaEmBranco > 0) {
        mensagemAuxiliar += '\n\n\n“O campo “Nota de recuperação de estudos” é de preenchimento obrigatório quando o campo recuperação de estudos estiver marcado. Segue abaixo relação de aluno(s) com pendência de lançamento(s) da nota de recuperação de estudos:\n ';
        mensagemAuxiliar += '\n' + totalParalelaEmBranco;
        mensagemAuxiliar += (totalParalelaEmBranco == 1) ? ' Nota Recuperação pendente' : ' Notas Recuperação pendentes';
        mensagemAuxiliar += paralelaEmBrancoAlunos;
        bErro = true;
    }

    return {
        mensagemAuxiliar: mensagemAuxiliar,
        erro: bErro
    };
}

// função para navegação entre controles através das setas
function NavegaProximo(e, jqInput) {
    var linhaAtual = jqInput.closest('td').data('linha-navegacao');
    var colunaAtual = jqInput.closest('td').data('coluna-navegacao');
    var linhaNova = linhaAtual;
    var colunaNova = colunaAtual;

    var unicode = ObtemTecla(e);
    var direcional = false;

    switch (unicode) {
        case gblTeclas.Up:
        case gblTeclas.Down:
        case gblTeclas.Left:
        case gblTeclas.Right:
        case gblTeclas.Enter:
            direcional = true;
            break;
    }

    // atualiza índices row, column do próximo controle, conforme seta pressionada
    switch (unicode) {
        case gblTeclas.Up: linhaNova = linhaAtual - 1; break;
        case gblTeclas.Enter: linhaNova = linhaAtual + 1; break;
        case gblTeclas.Down: linhaNova = linhaAtual + 1; break;
        case gblTeclas.Left: colunaNova = colunaAtual - 1; break;
        case gblTeclas.Right: colunaNova = colunaAtual + 1; break;
    }

    if (!direcional) {
        return;
    }

    // busca o próximo controle
    var jqProxControle = jqListaTRLancamentosMatriculados.find('td[data-linha-navegacao="' + linhaNova + '"][data-coluna-navegacao="' + colunaNova + '"]').find('input:text')

    // se não nulo, foca no controle ou avança para o próximo controle caso readOnly || disabled
    if (jqProxControle.length) {

        //verifica se controle é tipo text
        if (jqProxControle.is('input:text')) {
            // se controle readOnly ou disabled, chama evento para avançar para próximo controle
            if (jqProxControle.prop('disabled') ||
				jqProxControle.prop('readonly') &&
				direcional) {
                NavegaProximo(e, jqProxControle);
                return;
            }
            // se controle enabled e !readonly, foca no controle
            else {
                jqProxControle.focus();
            }
        }
        // se nulo, volta para início ou fim da linha ou coluna, conforme tecla pressionada
    }
    else {
        var colunaTemp = 0, linhaTemp = 0;

        if (unicode == gblTeclas.Up ||
			unicode == gblTeclas.Down ||
			unicode == gblTeclas.Enter) {
            colunaTemp = colunaAtual;
        }

        if (unicode == gblTeclas.Left ||
			unicode == gblTeclas.Right) {
            linhaTemp = linhaAtual;
        }

        switch (unicode) {
            case gblTeclas.Up:
                {
                    jqListaTRLancamentosMatriculados
					.find('td[data-coluna-navegacao="' + colunaTemp + '"]')
					.last()
					.find('input:enabled')
					.focus();

                    $("html, body").animate({ scrollTop: $(document).height() }, "slow");

                    break;
                }
            case gblTeclas.Down:
            case gblTeclas.Enter:
                {
                    jqListaTRLancamentosMatriculados
					.find('td[data-coluna-navegacao="' + colunaTemp + '"]')
					.first()
					.find('input:enabled')
					.focus();

                    $("html, body").animate({ scrollTop: 0 }, "slow");

                    break;
                }
            case gblTeclas.Left:
                {
                    var alvo = $(jqInput)
					.closest('tr')
					.find('td[data-linha-navegacao="' + linhaTemp + '"]')
					.last()
					.find('input:enabled')
					.focus();

                    if (alvo.length) {
                        alvo.focus();
                    }

                    break;
                }
            case gblTeclas.Right:
                {
                    var alvo = $(jqInput)
					.closest('tr')
					.find('td[data-linha-navegacao="' + linhaTemp + '"]')
					.first()
					.find('input:enabled')

                    if (alvo.length) {
                        alvo.focus();
                    }
                    break;
                }
        }
    }
}

function ValidaKeyPress(jqObject, e) {
    var keyCode = ObtemTecla(e);

    if (keyCode != gblTeclas.Tab)
        $(jqObject).replaceSelection('', true);

    var new_char = String.fromCharCode(keyCode);
    var old_value = $(jqObject).val().replace(",", ".");

    jqDadosTurmaDisciplina = $('#dados-turma-disciplina');
    var notaMax = jqDadosTurmaDisciplina.data('nota-maxima').toString();
    var numCasasDec = jqDadosTurmaDisciplina.data('nota-casas-decimais').toString();

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
    } else if (keyCode == gblTeclas.Tab ||
				keyCode == gblTeclas.Home ||
				keyCode == gblTeclas.End ||
				keyCode == gblTeclas.Shift ||
				keyCode == gblTeclas.Backspace ||
				keyCode == gblTeclas.Del) {
        return (true);
    } else {
        return (false);
    }
}

function HabilitaRecParalela(jqObject) {
    var jqTRLancamento = $(jqObject).closest('tr');
    var jqChkRecParalela = jqTRLancamento.find('input:checkbox.input-recuperacao-paralela');
    var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');

    if (parseFloat(jqObject.value) < 5) {
        jqChkRecParalela.prop('disabled', false);
    }
    else {
        jqChkRecParalela.prop('disabled', true);
        jqChkRecParalela.prop('checked', false);
        jqInputRecParalela.prop('disabled', true);
        jqInputRecParalela.css('background-color', '#E0E0E0');
    }

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
        var decisao = confirm("Prezado professor, foram aplicadas estratégias de recuperação de estudo ao aluno? Em caso de dúvidas, ver informações através do link “Para saber mais sobre recuperação de estudos: clique aqui”.");
        if (decisao) {
            var jqTRLancamento = $(jqObject).closest('tr');
            var oChkRecuperacao = jqTRLancamento.find('input:checkbox.input-recuperacao-paralela');
            var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');
            var jqInputFaltas = jqTRLancamento.find('input:text.input-faltas');
            oChkRecuperacao.prop('checked', true);
            jqInputRecParalela.val('').prop('disabled', false);
            jqInputRecParalela.css('background-color', '#FFF');
            jqInputFaltas.focus();

            HabilitaRecParalela(jqObject);
            jqObject.defaultValue = jqObject.value;
        }
        else {
            HabilitaRecParalela(jqObject);
            ValidaCorrige(jqObject);
        }
    }
    else if ($.trim(jqObject.defaultValue) == "" && parseFloat(jqObject.value) < 5) {
        var decisao = confirm("Prezado professor, foram aplicadas estratégias de recuperação de estudo ao aluno? Em caso de dúvidas, ver informações através do link “Para saber mais sobre recuperação de estudos: clique aqui”.");
        if (decisao) {
            var jqTRLancamento = $(jqObject).closest('tr');
            var oChkRecuperacao = jqTRLancamento.find('input:checkbox.input-recuperacao-paralela');
            var jqInputRecParalela = jqTRLancamento.find('input:text.inputnotarecuperacao');
            var jqInputFaltas = jqTRLancamento.find('input:text.input-faltas');
            oChkRecuperacao.prop('checked', true);
            jqInputRecParalela.val('').prop('disabled', false);
            jqInputRecParalela.css('background-color', '#FFF');
            jqInputFaltas.focus();

            HabilitaRecParalela(jqObject);
            jqObject.defaultValue = jqObject.value;
        }
        else {
            HabilitaRecParalela(jqObject);
            ValidaCorrige(jqObject);
        }
    }
    else {
        HabilitaRecParalela(jqObject);
        jqObject.defaultValue = jqObject.value;
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
    jqDadosTurmaDisciplina = $('#dados-turma-disciplina');
    var notaMax = jqDadosTurmaDisciplina.data('nota-maxima').toString();
    var numCasasDec = jqDadosTurmaDisciplina.data('nota-casas-decimais').toString();


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

$(jqObject).css({ "color": (valor >= notaMax / 2.0) ? "blue" : "red" });

var jqInputNotaFinalBimestreformatada = $('.tabela-padrao table').find('input:text.inputconceito');
jqInputNotaFinalBimestreformatada = jqInputNotaFinalBimestreformatada.val();
var valorFormatado = $(jqObject).val();
}

function InsereVirgulaZero(jqObject) {
    var oval = $(jqObject).val().replace(",", ".");
    if (oval != "") {
        if (oval.indexOf('.') == 0) {
            oval = "0" + oval;
        }
        if (oval != null && oval.indexOf('.') < 0)
            $(jqObject).val(oval + ",00");
        else if (oval.indexOf('.') == oval.length - 1) {
            $(jqObject).val(oval.replace(".", ",") + "00");
        }
        else if (oval.indexOf('.') == oval.length - 2) {
            $(jqObject).val(oval.replace(".", ",") + "0");
        }
        else if (oval.indexOf('.') == oval.length - 3) {
            $(jqObject).val(oval.replace(".", ","));
        }
    }
}

function ObtemTecla(a) {
    var b;
    if (a.keyCode != "undefined" && a.keyCode != "0") {
        b = a.keyCode
    }
    else {
        if (a.which != "undefined" && a.which != "0") {
            b = a.which
        }
        else {
            if (a.charCode != "undefined" && a.charCode != "0") {
                b = a.charCode
            }
            else {
                return 0
            }
        }
    }
    return b;
}

function IniciaContagemRegressiva() {
    var tempoMaximo = parseInt('<%= ConfigurationManager.AppSettings["TempoMaximoLancamentoNotas"] ?? "-1" %>');
    var date = new Date();

    if (tempoMaximo < 0) {
        return;
    }

    date.setMinutes(date.getMinutes() + tempoMaximo);

    if ($('#contagem-regressiva').length) {

        $('#contagem-regressiva').countdown({
            until: date,
            compact: true,
            layout: '{hnn}{sep}{mnn}{sep}{snn}',
            onExpiry: function () {
                alert('<%=Resources.Recurso.LancamentoNotasLista_MensagemContagemRegressivaExpirada%>');

                window.location.href = '<%=Url.Action("Desloga","Login")%>';
            }
        });
    }
}

function ConfirmaAberturaDeFilipetaLancamento() {
    var mensagemConfirmacao = '<%=Resources.Recurso.LancamentoNotasLista_MensagemConfirmaImpressaoFilipeta%>'.replace('{0}', gblCodigoFilipeta);

    $('#confirmacao-fundo').show();
    $('#confirmacao').show().find('.mensagem').html(mensagemConfirmacao);

    $('#confirmacao .bt-sim').click(function () {
        window.open('<%=Url.Content("~/Relatorios.aspx?relatorio=Filipeta&grupo=dol")%>&Chave=' + gblChaveImpressaoFilipeta, 'popFilipeta', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');
        $('#confirmacao-fundo').hide();
        $('#confirmacao').hide();
    })

    $('#confirmacao .bt-nao').click(function () {
        $('#confirmacao-fundo').hide();
        $('#confirmacao').hide();
    })
}




/*************************
LANCAMENTO DE NOTAS - LISTA - FIM
*************************/
