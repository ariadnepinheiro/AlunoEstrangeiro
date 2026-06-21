/*************************
DADOS DO DOCENTE - INICIO
*************************/

$(function () {
    $("#bt-imprimir").click(function () {
        $.ajax({
            url: "DadosDocente/ObtemDataHoraParaImpressao",
            dataType: "text",
            type: "POST",
            success: function (data) {
                $("#lbl-data-hora-impressao").text("Data e hora: " + data);

                print();
            }
        });
    });
});

/*************************
DADOS DO DOCENTE - FIM
*************************/