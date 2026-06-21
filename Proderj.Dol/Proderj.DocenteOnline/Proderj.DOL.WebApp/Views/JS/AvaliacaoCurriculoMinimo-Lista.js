/*************************
AVALIACAO CURRICULO MINIMO - LISTA - INICIO
*************************/
var gblAlteracaoDetectada = false;

$(document).ready(function () {
	CarregaEventos();
});

function CarregaEventos() {

	$('.conteudo input, .conteudo select, .conteudo textarea').on('keydown.detectaAlteracao', function () {
		gblAlteracaoDetectada = true;
		$('.conteudo input, .conteudo select, .conteudo textarea').off('keydown.detectaAlteracao');
	});

	$('.conteudo input:radio').on('change.detectaAlteracao', function () {
		gblAlteracaoDetectada = true;
		$('.conteudo input:radio').off('change.detectaAlteracao');
	});


	//Trata as possíveis saídas da interface
	$('#bt-cancelar-lancamento') //Cancelar
	.add('#turma-selecionada .bt-mudar-turma a') //Mudar turma
	.add('#tab-bimestres li:not(.inativo)') //Tabs de bimestres
	.add('body > .cabecalho a') //Menu superior
		.on('click', function () {
			var confirmado = true;
			if (gblAlteracaoDetectada) {

				confirmado = confirm('<%= Resources.Recurso.AvaliacaoCurriculoMinimoLista_MensagemSairSemSalvar %>');
			}

			if (confirmado && $(this).is('#bt-cancelar-lancamento')) {
				this.form.action = '<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "RespostaCurriculoMinimo" })%>';
				this.form.submit() //Necessário forçar para IE7 ou anterior
			}

			//Inputs do tipo button, já disparam o form-submit se nao ocorrer um "return false";
			return confirmado;
		});

	$('#tab-bimestres').on('click', 'li', function () {
		if ($(this).hasClass('inativo')) {
			alert('<%=Resources.Recurso.AvaliacaoCurriculoMinimoLista_BimestreNaoConfiguradoParaLancamentoDeCurriculoMinimo %>');
		}
		else {
			var subperiodo = $(this).data('subperiodo');
			var jqFormulario = $('#frm-postback');
			jqFormulario.prop('action', '<%=Url.Action("Lista","AvaliacaoCurriculoMinimo") %>');
			jqFormulario.prop('target', '');

			$('#FMB-Subperiodo').val(subperiodo);
			jqFormulario.submit();
		}
	});

	$('#bt-salvar-lancamento').on('click', function () {
		
		//Move os dados do formulario de postback para o foprmulario de envio de nota.
		var sHTMInputs = $('#frm-postback').html();
		$('#frm-postback').empty() //Limpa o form antigo para nao ter duplicação de ids
		$('#frm-lancamento').prepend(sHTMInputs);

		var subperiodo = ObtemSubperiodoSelecionado();
		$('#FMB-Subperiodo').val(subperiodo);

		$('#frm-lancamento').submit(); //Necessário forçar para IE7 ou anterior
		return true; //submit do form
	});
}

function ObtemSubperiodoSelecionado() {
	var subperiodo = $('#tab-bimestres li.selecionado').data('subperiodo');
	return subperiodo;
}


/*************************
AVALIACAO CURRICULO MINIMO - LISTA - FIM
*************************/
