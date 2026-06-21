/*********
LOGIN - INICIAL - INICIO
*********/
$(document).ready(function () {
	CarregaEventos();
});

function CarregaEventos() {

	$('input:button').on('click', function () {
		$(this).hide();
		$('#painel-apresentacao p').hide();
		$('#ajuda-menu').show();
		$('#ajuda-menu img').fadeIn(1000);
		setTimeout(function () {
			$('#ajuda-menu img').fadeTo(1000, 0.3);
			setInterval(function () {
				$('#ajuda-menu img').fadeTo(1000, 0.3);
			},2000)
		},1000);
		setInterval(function () {
			$('#ajuda-menu img').fadeTo(1000, 1);
		}, 2000);

		$('html, body').scrollTop(0);
	});
}
/****
LOGIN - INICIAL - FIM
****/