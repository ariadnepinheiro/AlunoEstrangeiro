<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.AjudaViewModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	<%=Model.TituloDaPagina %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS") %>" />
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "ajuda" }) %>"></script>

	<script type="text/javascript">
		$(document).ready(function () {
			$('#indice a').on('click', function () {
				sHash = $(this).data('resposta');
				setTimeout(function () {
					var jqPergunta = $('a[name="Resposta' + sHash + '"]');
					var liPergunta = jqPergunta.closest('li');

					liPergunta.css({ 'color': '#F00', 'border-top': '2px solid #F00', 'border-bottom': '2px solid #F00' }).animate({ 'color': '#000', 'border-top-color': '#FFF', 'border-bottom-color': '#FFF' }, 5000);
					liPergunta.find('p').css({ 'color': '#F00' }).animate({ 'color': '#538ED5' }, 5000);

					$('html, body').animate({ scrollTop: jqPergunta.position().top - 110 }, 'slow');

				}, 100)
			});

			$('.cabecalho a.ico_sair').on('click', function () {
				if (opener) {
					window.close();
					return false;
				}
			})

		})
	</script>
	<style type="text/css">
		.conteudo h2 {
			background-color: #8DB4E3;
			color:#000;
			font-size:12px;
			text-align:center;
			padding:5px;
		}

		#indice, #lista-respostas {
			padding:0px;
			margin-left: 40px;
		}

		#indice li {
			margin: 5px 0px;
			font-size:12px;
		}
		#indice li a {
			color:#333;
			text-decoration:none;
		}
		
		#indice li a:hover {
			color:#27F;
			text-decoration:underline;
		}

		#lista-respostas li h3 {
			font-size:12px;
		}

		#lista-respostas li {
			font-size:12px;
		}

		#lista-respostas li p {
			color:#135EB5;
			line-height:18px;
			margin:10px 0px 25px 0px;
		}

		#lista-respostas a.topo {
			float:right;
			margin-left:20px;
			font-size:11px;
			text-decoration:none;
			color:#F91;
		}

		#lista-respostas a.topo:hover {
			text-decoration:underline;
		}
	</style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
	<% Html.RenderPartial("Cabecalho",Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
<div class="conteudo">
	<h2>Perguntas Frequentes sobre Currículo Mínimo</h2>
	<ol id="indice">
		<li><a href="#Resposta1" data-resposta="1">Qual é a proposta do Currículo Mínimo?</a></li>
		<li><a href="#Resposta2" data-resposta="2">Quais disciplinas possuem Currículo Mínimo?</a></li>
		<li><a href="#Resposta3" data-resposta="3">Como tenho acesso ao Currículo Mínimo?</a></li>
		<li><a href="#Resposta4" data-resposta="4">Há Currículo Mínimo para a Educação de Jovens e Adultos (EJA)?</a></li>
		<li><a href="#Resposta5" data-resposta="5">Há Currículo Mínimo para o Curso Normal em Nível Médio?</a></li>
		<li><a href="#Resposta6" data-resposta="6">O Currículo Mínimo se aplica ao Ensino Regular Noturno?</a></li>
		<li><a href="#Resposta7" data-resposta="7">Quando haverá Currículo Mínimo para as outras disciplinas e/ou modalidades de ensino?</a></li>
		<li><a href="#Resposta8" data-resposta="8">Os professores das disciplinas que não possuem Currículo Mínimo terão que preencher alguma declaração?</a></li>
		<li><a href="#Resposta9" data-resposta="9">Haverá revisão do Currículo Mínimo?</a></li>
		<li><a href="#Resposta10" data-resposta="10">Qual é a relação entre Currículo Mínimo e o SAERJINHO?</a></li>
		<li><a href="#Resposta11" data-resposta="11">Posso abordar outros temas/conteúdos que não estão no Currículo Mínimo com meus alunos?</a></li>
	</ol>  

	<h2>Respostas</h2>
	<ol id="lista-respostas">
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta1">Qual é a proposta do Currículo Mínimo?</a></h3>
			<p>
				O Currículo Mínimo é um documento que estabelece um padrão básico, com aquilo que é essencial e que deve ser ensinado-aprendido
				em cada segmento e cada disciplina da rede estadual. Ele define as competências e habilidades que não podem deixar de ser
				desenvolvidas nas aulas da rede estadual. É desenvolvido de acordo com a carga horária da disciplina na matriz curricular e com os
				principais conceitos científicos de cada área de conhecimento. Além disso, está em consonância com a legislação nacional e as avaliações
				externas a que nossos alunos são submetidos.
			</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta2">Quais disciplinas possuem Currículo Mínimo?</a></h3>
			<p>As disciplinas que já possuem Currículo Mínimo são: Língua Portuguesa, Matemática, História, Geografia, Sociologia e Filosofia. Todavia, esses currículos foram pensados para a modalidade de Ensino Regular.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta3">Como tenho acesso ao Currículo Mínimo?</a></h3>
			<p>O Currículo Mínimo está disponível em dois formatos: versão online, nos nossos portais eletrônicos (www.rj.gov.br/web/seeduc e www.conexaoprofessor.rj.gov.br), e versão impressa, nas escolas de Ensino Regular da rede.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta4">Há Currículo Mínimo para a Educação de Jovens e Adultos (EJA)?</a></h3>
			<p>Não. Por enquanto, continuam vigorando, para a Educação de Jovens e Adultos (EJA), as Reorientações Curriculares de 2006 - disponíveis nos portais eletrônicos da rede (www.rj.gov.br/web/seeduc e www.conexaoprofessor.rj.gov.br). Todavia, ainda este ano, será elaborado o Currículo Mínimo para todas as disciplinas da Educação de Jovens e Adultos.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta5">Há Currículo Mínimo para o Curso Normal em Nível Médio?</a></h3>
			<p>Não. Por enquanto, continuam vigorando, para o Curso Normal em Nível Médio, as Reorientações Curriculares de 2006 - disponíveis nos portais eletrônicos da rede (www.rj.gov.br/web/seeduc e www.conexaoprofessor.rj.gov.br). Todavia, ainda este ano, será elaborado o Currículo Mínimo para todas as disciplinas de Base Nacional Comum do Curso Normal em Nível Médio.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta6">O Currículo Mínimo se aplica ao Ensino Regular Noturno?</a></h3>
			<p>Sim. Os professores do Ensino Regular Noturno também devem adequar suas aulas de modo a contemplar os itens propostos pelo Currículo Mínimo, considerados essenciais para uma formação ideal mínima do alunado.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta7">Quando haverá Currículo Mínimo para as outras disciplinas e/ou modalidades de ensino?</a></h3>
			<p>Ainda este ano, será elaborado o Currículo Mínimo para as outras disciplinas de Base Nacional Comum do Ensino Regular e para todas as disciplinas das seguintes modalidades: Educação de Jovens e Adultos (EJA) e Curso Normal em Nível Médio.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta8">Os professores das disciplinas que não possuem Currículo Mínimo terão que preencher alguma declaração?</a></h3>
			<p>Não. Apenas deverão declarar o cumprimento do Currículo Mínimo, para fins de avaliação da aplicabilidade desse documento, os professores das disciplinas já contempladas (Língua Portuguesa, Matemática, História, Geografia, Sociologia e Filosofia). Os professores das outras disciplinas somente terão que fazer esse lançamento no ano letivo de 2012, quando já haverá Currículo Mínimo para todas as disciplinas de Base Nacional Comum em todas as modalidades de ensino.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta9">Haverá revisão do Currículo Mínimo?</a></h3>
			<p>Sim. O Currículo Mínimo foi projetado para estar em contínua construção, com a participação dos professores da rede, em diálogo permanente com a SEEDUC e as equipes de elaboração. Por isso, todos os Currículos Mínimos que já estão sendo implementados nas escolas serão aprimorados. Após intenso debate - por meio do envio de e-mails e da realização de encontros regionais abertos a professores e alunos -, as equipes elaboradoras apresentarão uma nova edição do Currículo Mínimo, mais enxuta e compatível com as diversas realidades encontradas em nossa rede.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta10">Qual é a relação entre Currículo Mínimo e o SAERJINHO?</a></h3>
			<p>O SAERJINHO possui uma matriz de referência própria, disponível em dois formatos: versão online, nos nossos portais eletrônicos (www.rj.gov.br/web/seeduc e www.conexaoprofessor.rj.gov.br), e versão impressa, nas escolas que oferecem Ensino Regular e Curso Normal em Nível Médio. Porém, essa matriz avalia alguns tópicos do Currículo Mínimo, além de um conjunto de habilidades - já avaliadas pela Prova Brasil e pelo SAERJ - essenciais à leitura e à resolução de problemas, que os alunos devem acumular ao longo da sua formação.</p>
		</li>
		<li>
			<a class="topo" href="#" title="Voltar ao topo da página">Voltar ao topo</a>
			<h3><a name="Resposta11">Posso abordar outros temas/conteúdos que não estão no Currículo Mínimo com meus alunos?</a></h3>
			<p>Sim. Como o Currículo se propõe a ser mínimo, o ideal é que o professor trabalhe outros temas/conteúdos além daqueles propostos pelo Currículo Mínimo.</p>
		</li>
	</ol>
</div>
</asp:Content>


