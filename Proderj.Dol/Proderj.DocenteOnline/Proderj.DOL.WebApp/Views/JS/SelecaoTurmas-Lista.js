/*************************
SELECAO DE TURMAS - LISTA - INICIO
*************************/

$(document).ready(function() {
	CarregaEventos();
})

function CarregaEventos() {

	//Click nas disciplinas que são inválidas para lançamento
    $('.tabela-padrao tbody tr').on('click', function () {
        var alerta = false;
        if ($(this).is('.frequencia-prova')) {
            return false;
        }
        if ($(this).is('[invalido-para-lancamento]')) {
            alerta = true;
            mensagemAlerta = '<%=Resources.Recurso.SelecaoTurmasLista_AlertaInvalidoParaLancamento%>';
        }

        PreparaFormularioPOST($(this));
        if (alerta)
            alert(mensagemAlerta);
        else {
            PreparaFormularioPOST($(this));
            $('#frm-selecao-turma').submit();
        }
    });

	$('.tabela-padrao').on('click', '.expansor', function() {
		ExpansaoContracao($(this));
	});
	
}

function PreparaFormularioPOST(jqTRClick) {

	//Verificar o Ano e Periodo de onde ocorreu o click
	var jqTRAnoPeriodo = jqTRClick.closest('.periodo');

	//Verificar Unidade de ensino de onde ocorreu o click
	var jqTRUnidadeEnsino = jqTRAnoPeriodo.closest('.unidade');

	var codigoUnidade	= jqTRUnidadeEnsino.data('codigo-unidade');
	var ano				= jqTRAnoPeriodo.data('ano');
	var periodo			= jqTRAnoPeriodo.data('periodo');

	var codigoCurso			= jqTRClick.data('codigo-curso');
	var codigoTurma			= jqTRClick.data('codigo-turma');
	var codigoDisciplina	= jqTRClick.data('codigo-disciplina');
	var codigoModalidade	= jqTRClick.data('codigo-modalidade');
	var tipoCurso			= jqTRClick.data('tipocurso');
	var serie				= jqTRClick.data('serie');

	$('#FST-CodigoCurso').val(codigoCurso);
	$('#FST-TipoCurso').val(tipoCurso);
	$('#FST-CodigoUnidade').val(codigoUnidade);
	$('#FST-Ano').val(ano);
	$('#FST-Periodo').val(periodo);
	$('#FST-Serie').val(serie);
	$('#FST-CodigoTurma').val(codigoTurma);
	$('#FST-CodigoDisciplina').val(codigoDisciplina);
	$('#FST-CodigoModalidade').val(codigoModalidade);
}

function ExpansaoContracao(jqExpansorClick) {

	//verificar se o conteudo abaixo do expansor está visivel
	var tipo = jqExpansorClick.data('tipo');

	//Se o tipo for periodo esconde a tabela senao escode a div de periodo
	var alvosParaAcao = jqExpansorClick.closest('.' + tipo).children('table:first, .periodo:first')

	//Verifica se os objetos estão atualmente escondidos ou nao
	var estahEscondido = alvosParaAcao.is(':hidden');

	if (estahEscondido) {
		alvosParaAcao.show();
		
		//Mudar imagem do expansor.
		jqExpansorClick.css('background-position', 'right');
	}
	else {
		alvosParaAcao.hide();
		
		//Mudar imagem do expansor.
		jqExpansorClick.css('background-position', 'left');
	}
RetiraBloqueio();
}

/*************************
SELECAO DE TURMAS - LISTA - FIM
*************************/
