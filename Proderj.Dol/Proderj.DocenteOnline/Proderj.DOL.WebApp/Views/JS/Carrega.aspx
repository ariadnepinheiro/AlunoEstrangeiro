<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.String>" %>
<% Response.ContentType = "text/javascript"; 
if (Model == null || Model.ToLower().IndexOf("jquery") == -1)
{
	%><!-- #include file = "Geral.js" --><%
}

if (Model != null)
{
	switch (Model.ToLower())
	{
		case "jquery"						: { %><!-- #include file = "jquery-1.8.0.min.js" --><% break; }
		case "login-inicial"				: { %><!-- #include file = "Login-Inicial.js" --><% break; }
		case "selecaoturmas-lista"			: { %><!-- #include file = "SelecaoTurmas-Lista.js" --><% break; }
		case "login-confirmatermoaceite"	: { %><!-- #include file = "Login-ConfirmaTermoAceite.js" --><% break; }
		case "lancamentonotas-lista"		: { %><!-- #include file = "LancamentoNotas-Lista.js" --><% break; }
		case "ajuda-curriculominimo"		: { %><!-- #include file = "jquery-ui-1.8.24.ajuda.min.js" --><% break; }
		case "respostacurriculominimo-lista": { %><!-- #include file = "RespostaCurriculoMinimo-Lista.js" --><% break; }
		case "avaliacaocurriculominimo-lista": { %><!-- #include file = "AvaliacaoCurriculoMinimo-Lista.js" --><% break; }
		case "cadastroglp-inicial"			: { %><!-- #include file = "CadastroGLP-Inicial.js" -->
                                                  <!-- #include file = "DadosPessoais-Inicial.js" -->
												  <!-- #include file = "MaskTypes.js" -->
												  <!-- #include file = "jquery.meio.mask.js" -->
                                                  <!-- #include file = "jquery.inputmask.bundle.min.js" -->
												<% break; }
        case "dadospessoais-inicial"	    : { %><!-- #include file = "DadosPessoais-Inicial.js" -->
												  <!-- #include file = "MaskTypes.js" -->
												  <!-- #include file = "jquery.inputmask.bundle.min.js" -->
												<% break; }
		case "lancamentonotas-lista-jquery"	: { %>
			<!-- #include file = "jquery.fieldSelection.js" -->
			<!-- #include file = "jquery.countdown.min.js" -->
			<!-- #include file = "jquery.numeric.js" -->
		<% break; }
		case "protocolonota-lista" : {%><!-- #include file = "ProtocoloNota-Lista.js" --><% break; }
		case "login-apresentacao" : {%><!-- #include file = "Login-Apresentacao.js" --><% break; }
        case "login-alterasenha": {%><!-- #include file = "Login-AlteraSenha.js" --><% break; }
        case "dadosdocente-inicial": {%><!-- #include file = "DadosDocente-Inicial.js" --><% break; }
        case "master-bloqueio": {%><!-- #include file = "Master.js" --><% break; }
        case "protocolonota-paginacao" : {%><!-- #include file = "ProtocoloNota-Paginacao.js" --><% break; }
	}
}
%>