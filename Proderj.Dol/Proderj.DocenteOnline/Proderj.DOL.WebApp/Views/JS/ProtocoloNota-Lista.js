/*****************
PROTOCOLONOTA - LISTA - INICIO
******************/
$(document).ready(function () {
    CarregaEventos();
});

function CarregaEventos() {

    $('#cmbMudarPagina').trigger('change');  //Forçar a primeira página aparecer para IE7 ou anterior

    $('#cmbAno').on('change', function () {
        //Limpar combos dependentes
        var ano = this.value;
        jqComboPeriodo = $('#cmbPeriodo');
        $('#tabela-protocolo').html('');
        if (this.selectedIndex == 0) {
            jqComboPeriodo
				.empty()
				.append('<option>-- Selecione um ano -- </option>')
				.prop('disabled', true)

				.val('')
				.css('background-color', '#EBEBE4')
				.prop('disabled', true);


        }
        else {
            //Carregamento dinâmico dos valores das combos.
            var oCombo = jqComboPeriodo.get(0);
            oCombo.options[0].text = 'Carregando...'

            $.ajax({
                url: '<%=Url.Action("ListaPeriodo","ProtocoloNota")%>',
                data: { ano: ano },
                type: 'POST',
                success: function (oJSON) {
                    if (oJSON.Sucesso) {
                        jqComboPeriodo.empty();
                        var oOption = document.createElement('OPTION');
                        oOption.text = '-- Selecione um periodo --';
                        oOption.value = '';

                        jqComboPeriodo
							.append(oOption)
							.prop('disabled', false)

							.val('')
							.css('background-color', '#FFF')
							.prop('disabled', false);


                        $.each(oJSON.Combo, function () {
                            var oOption = document.createElement('OPTION');
                            oOption.value = this.Codigo;
                            oOption.text = this.Descricao;
                            jqComboPeriodo.append(oOption);
                        });
                    }
                },
                error: function (oXHTR) {
                    oCombo.options[0].text = 'Ocorreu um erro inesperado... '
                }
            });
        }
    });


    $('#cmbPeriodo').on('change', function () {
        var periodo = this.value;
        var ano = $('#cmbAno').val();

        jqComboPeriodo = $('#cmbPeriodo');
        if (this.selectedIndex == 0) {
            $('#tabela-protocolo').html('');
            RetiraBloqueio();
        }
        else {            
            $('#tabela-protocolo').fadeTo(0, 0.5);
            $.ajax({
                method: "POST",
                url: '<%=Url.Action("ListaProtocolo", "ProtocoloNota")%>',
                dataType:"html",
                cache: false,
                data: { ano: ano, periodo: periodo },
                
                success: function (sHTML) {
                    $('#tabela-protocolo').html(sHTML);
                },
                error: function () {
                    alert('Erro inesperado ao listar protocolo');
                },
                complete: function () {
                    $('#tabela-protocolo').fadeTo(0, 1);
                    RetiraBloqueio();
                }

            })
        }
    });


}

/*****************
PROTOCOLONOTA - LISTA - FIM
******************/