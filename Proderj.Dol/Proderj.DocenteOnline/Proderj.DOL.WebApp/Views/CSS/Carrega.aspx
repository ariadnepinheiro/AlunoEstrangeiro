<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.String>" %>
<% Response.ContentType = "text/css"; %>
<!-- #include file = "Geral.css" -->
<!-- #include file = "Cabecalho.css" -->
<!-- #include file = "Rodape.css" -->
<%
switch (Model)
{
	case "login-inicial"				: { %><!-- #include file = "Login-Inicial.css" --><% break; }
	case "login-apresentacao"			: { %><!-- #include file = "Login-Apresentacao.css" --><% break; }
	case "selecaoturmas-lista"			: { %><!-- #include file = "SelecaoTurmas-Lista.css" --><% break; }

	case "lancamentonotas-lista"		: { %><!-- #include file = "TurmaSelecionada.css" -->
											  <!-- #include file = "LancamentoNotas-Lista.css" --><% break; }

	case "respostacurriculominimo-lista": { %><!-- #include file = "TurmaSelecionada.css" -->
											  <!-- #include file = "RespostaCurriculoMinimo-Lista.css" --><% break; }

	case "avaliacaocurriculominimo-lista": {%><!-- #include file = "TurmaSelecionada.css" -->
											  <!-- #include file = "AvaliacaoCurriculoMinimo-Lista.css" --><% break; }

	case "login-confirmatermoaceite"	: { %><!-- #include file = "Login-ConfirmatermoAceite.css" --><% break; }
	case "cadastroglp-inicial"	: { %>
                                    <!-- #include file = "CadastroGLP-Inicial.css" -->
                                    <!-- #include file = "DadosPessoais-Inicial.css" -->
                                    <% break; }
    case "dadospessoais-inicial": {%><!-- #include file = "DadosPessoais-Inicial.css" --><% break; }
	case "shared-erroinesperado" : {%><!-- #include file = "Shared-ErroInesperado.css" --><% break; }
	case "protocolonota-lista" : {%><!-- #include file = "ProtocoloNota-Lista.css" --><% break; }
    case "login-alterasenha" : {%><!-- #include file = "Login-AlteraSenha.css" --><% break; }
    case "dadosdocente": {%><!-- #include file = "DadosDocente.css" --><% break; }
    case "acompanhamentoclassroom": {%><!-- #include file = "AcompanhamentoClassroom.css" --><% break; }
}
%>