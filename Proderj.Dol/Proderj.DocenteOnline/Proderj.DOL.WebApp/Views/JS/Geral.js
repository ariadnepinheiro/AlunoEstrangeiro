/*****************
GERAL - INICIO
******************/

function AbrePopUpAjuda(oLink) {
	var url = oLink.href;
	var janela = window.open(oLink.href, 'popAjuda', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=780, height=600, resizable=yes');

	return false;
}

function ExibeErroNegocio(oJSON) {
	if (oJSON.ListaMensagem) {
		var msgErro = 'Ocorreram os seguintes erros:\n';
		$.each(oJSON.ListaMensagem, function () {
			msgErro += '\n- ' + this;
		});

		alert(msgErro);
	}
	else {
		alert(oJSON.Mensagem);
	}
}

//Verifica se existe JQuery
if (window.jQuery !== undefined) {
	$(document).ready(function () {
		//Link QHI Menu
		$('#link-QHI').on('click', function () {
		    window.open('<%=Url.Action("Relatorios","Api")%>', 'popQHI', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');
			return false;
		});

		//Link Ajuda Menu
		$('#menu-ajuda').on('click', function () {
			AbrePopUpAjuda(this);
			return false;
		});
	});
}
else {
	//Faz através de DOM (sem jQuery)
	var intervaloLinkQHI = setInterval(function () {
		var oLink = document.getElementById('link-QHI');

		//Link QHI Menu
		if (oLink) {
			oLink.onclick = function () {
				window.open('<%=Url.Content("~/Relatorios.aspx?relatorio=chdocenteonline&grupo=dol") %>', 'popQHI', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');
				return false;
			}

			clearInterval(intervaloLinkQHI);
			intervaloLinkQHI = null;
		}

		//Link Ajuda Menu
		var oLinkAjuda = document.getElementById('menu-ajuda')
		if (oLinkAjuda) {
			oLinkAjuda.onclick = function () {
				AbrePopUpAjuda(oLinkAjuda)
				return false;
			};
		}
	}, 1000);
}

/*****************
GERAL - FIM
******************/