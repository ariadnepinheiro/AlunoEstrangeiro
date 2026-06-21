/*********
LOGIN - INICIAL - INICIO

Garante sempre que a pagina carregar o foco virá na matrícula e o 
usuário nao precisará forçosamente clicar com o mouse no campo de texto para digitar ou apertar o enter para logar 
caso o navegador tenha lembrete.*/

var loginMatricula = document.getElementById('LoginMatricula');
if (loginMatricula)
	loginMatricula.focus();

//TODO: ver se existe forma melhor de fazer
function TrocaImagem() {
    var form = document.getElementById("frm-login");
    form.action = '<%=Url.Action("TrocaImagem","Login")%>';
    form.submit();
}
/****
LOGIN - INICIAL - FIM
****/