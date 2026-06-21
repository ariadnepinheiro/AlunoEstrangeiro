<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.LoginApresentacaoViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TituloPagina"><%=Model.TituloDaPagina %></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeadPagina">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "login-apresentacao"}) %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "login-apresentacao" }) %>"></script>
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Cabecalho">
	<% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="Conteudo">
	<div class="conteudo">
		<div id="painel-apresentacao">
			<h3>Prezado(a) <%=Model.CabecalhoModelo.DocenteLogadoModelo.Nome %>,</h3>
			<p>
				O sistema Conexão Educação é uma importante ferramenta de gestão da Secretaria de
				Estado de Educação, que tem como principal objetivo apoiar os gestores das unidades
				escolares no desenvolvimento de suas atividades pedagógicas e no cumprimento de
				suas metas.
			</p>
			<p>
				Visando fortalecer o uso do Conexão, a Superintendência de Tecnologia da Informação
				vem desenvolvendo melhorias contínuas no sistema, de forma a aumentar a confiabilidade
				e a disponibilidade das informações nele inseridas, bem como seu desempenho.<br />
				Além das melhorias no Conexão Educação, outras ações de infraestrutura de tecnologia
				estão sendo realizadas, como a migração dos links MPLS nas escolas e a priorização
				nos atendimentos de suporte técnico.
			</p>
			<p>
				<strong>
					PARA GARANTIR MAIOR EFETIVIDADE NO USO DO CONEXÃO EDUCAÇÃO, 
					INFORMAMOS QUE O SISTEMA É HOMOLOGADO PARA USO ATRAVÉS DOS NAVEGADORES E SUAS VERSÕES MICROSOFT EDGE 12+, CHROME 21+ e FIREFOX 28+.
				</strong>
			</p>
			<p>
				<strong>
					O USO DO SISTEMA ATRAVÉS DE OUTROS NAVEGADORES PODE OCASIONAR ALGUM COMPORTAMENTO
					FORA DOS PADRÕES NORMAIS DE FUNCIONAMENTO, PREVIAMENTE HOMOLOGADOS. CASO ALGUM COMPORTAMENTO
					DESTE TIPO VENHA A OCORRER, FAVOR REALIZAR A OPERAÇÃO UTILIZANDO UM DOS NAVEGADORES
					HOMOLOGADOS. CASO O COMPORTAMENTO FORA DO NORMAL CONTINUE A OCORRER, FAVOR ENTRAR
					EM CONTATO CONOSCO ATRAVÉS DA CENTRAL DE RELACIONAMENTO.
				</strong>
			</p>
			<p>
				Agradecemos a sua constante colaboração para uma educação de qualidade!
			</p>
			
			<p>
				Superintendência de Tecnologia da Informação<br />
				Subsecretaria Executiva de Infraestrutura e Tecnologia
			</p>
			<input type="button" id="btContinuarApresentacao" value="Continuar"/>
			<div class="clear"></div>
			<p id="ajuda-menu">
				Selecione uma das opções de menu acima
				<%--<img id="seta-lancamento-nota" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_LancamentoDeNota %>" />
				<img id="seta-cadastramento-glp" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_CadastramentoParaGLP %>" />
				<img id="seta-consulta-qhi" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_ConsultaAlocacaoQHI %>" />
				<img id="seta-curriculo-minimo" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_CurriculoMinimo %>" />
				<img id="seta-protocolos" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_Protocolos %>" />
                <img id="seta-dados-docente" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_DadosDocente %>" />
				<img id="seta-codigo-armazem-do-livro" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_CodigoArmazemDoLivro %>" />
                <img id="seta-dados-pessoais" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_DadosPessoais %>" />
                <img id="seta-acompanhamento-classroom" src="<%=Url.Content("~/Imagens/apresentacao_ajuda.png") %>" title="<%= Resources.Recurso.Menu_AcompanhamentoClassroom %>" />--%>
			</p>
		</div>
	</div>
</asp:Content>
