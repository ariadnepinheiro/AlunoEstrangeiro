/****
LOGIN - CONFIRMA TERMO ACEITE - INICIO
****/

$(document).ready(function () {
	$('#bt-nao-concordo').on('click', function () {
		$('#frm-termo-aceite')
		.prop('action', '<%=Url.Action("Desloga","Login")%>')
		.submit();
	});

	$('#bt-concordo').on('click', function () {
		$('#frm-termo-aceite').submit();
	});

});

/****
LOGIN - CONFIRMA TERMO ACEITE - FIM
****/
