/*****************
PROTOCOLONOTA - PAGINACAO - INICIO
******************/

    $('#cmbMudarPagina').on('change', function () {
        var pagina = this.value;
        $('.tabela-padrao tbody tr.pagina:visible').hide();
        $('.tabela-padrao tbody tr.pagina-' + pagina).fadeIn(100);
    });


