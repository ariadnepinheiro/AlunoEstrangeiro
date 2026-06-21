/*********
LOGIN - ALTERA SENHA
*/

$(document).ready(function () {
    var senhaAtual = document.getElementById('SenhaAtual');
    if (senhaAtual)
        senhaAtual.focus();

    $('#bt-confirmar').on('click', function () {
        $('#frm-login').submit();
    });

    $('#bt-cancelar').on('click', function () {
        $('#frm-login')
        .prop('method', 'GET')
        .prop('action', '<%= Url.Action("ConfirmaTermoAceite", "Login")%>')
        .submit();
    });
});

/****
LOGIN - ALTERA SENHA
****/