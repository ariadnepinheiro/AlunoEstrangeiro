<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Proderj.DOL.WebApp.Models.CabecalhoViewModel>" %>
<div class="top_inf cabecalho">
	<div class="top">

        <!-- LOGO DO RJ -->
		<div class="top_logo" align="center">
			<img src="<%=Url.Content("~/Imagens/logo-educacao.png")%>" alt="Governo do Rio de Janeiro - Sec de Estado de Educação" style="border-width:0px;">
		</div>

        <!-- Icone menu responsivo -->
        <input type="checkbox" id="bt_menu">
        <label for="bt_menu">&#9776;</label>

        <!-- MENU RESPONSIVO -->
        <div class="menu-responsivo">
            <!-- LOGO CONEXÃO + DOL -->
		    <div class="tit_sist">
			    <%--<img border="0" src="<%=Url.Content("~/Imagens/logo_CEG.png")%>" style="border-width:0px;">--%>
                <span><h2>Docente</h2><p>Online teste!!!</p></span>
                <span class="marca-pagina"><p>Página:</p><h1 align="left"><%=Model.TituloCabecalho %></h1></span>
		    </div>

            <!--  DATA E INFO DO USUÁRIO -->
		    <div class="saudacao" align="left">
			    <% if (Model.DocenteLogadoModelo != null) { %>
				    ID/Vínculo: <%=Model.DocenteLogadoModelo.IdFuncional %> / <%=Model.DocenteLogadoModelo.Vinculo %>
			    <% } %>
			    <div id="divHoraData"><%=Model.DataPorExtenso %></div>
		    </div>
        
             <!-- BOTÕES INICIO / AJUDA  / SAIR -->
		    <div class="botoes-menu-superior">
			    <% if (Model.BotaoSairHabilitado) { %>
				    <a title="Sair" class="botao ico_sair" href="<%=Url.Action("Desloga", "Login") %>"><span class="icon-sair"></span> Sair</a>
			    <% }
			    if (Model.BotaoAjudaHabilitado) { %>
				    <a title="Ajuda" class="botao ico_info" id="menu-ajuda" href="<%=Model.LinkAjuda %>"><span class="icon-ajuda"></span>Ajuda</a>
			    <% }
			    if (Model.BotaoInicioHabilitado) { %>
				    <a title="Apresentação do sistema" class="botao ico_inicio" href="<%=Url.Action("Apresentacao","Login") %>"><span class="icon-home"></span>Início</a>
			    <% } %>
		    </div>

            <% 
		    if (Model.DocenteLogadoModelo != null && Model.DocenteLogadoModelo.AceitouTermoDeAceite) { %>
                <!-- NAVBAR - MENU INTERNO -->
			    <ul class="menu-topo">
				    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="https://localhost:7244/LancamentoFrequencia">Lançamento de Frequência</a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>
				    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "LancamentoNotas" })%>"><%=Resources.Recurso.Menu_LancamentoDeNota%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>
				    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Inicial","CadastroGLP")%>"><%=Resources.Recurso.Menu_CadastramentoParaGLP%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>

				    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a id="link-QHI" href="#"><%=Resources.Recurso.Menu_ConsultaAlocacaoQHI%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>

				    <%--<li>
					    <span class="menu-aba-esquerda"></span>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "RespostaCurriculoMinimo" })%>"><%=Resources.Recurso.Menu_CurriculoMinimo%></a>
					    <span class="menu-aba-direita"></span>
				    </li>--%>

				    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Lista","ProtocoloNota")%>"><%=Resources.Recurso.Menu_Protocolos%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>

                    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Inicial","DadosDocente")%>"><%=Resources.Recurso.Menu_DadosDocente%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>

                    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Inicial","DadosPessoais")%>"><%=Resources.Recurso.Menu_DadosPessoais%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>

                  <%--   <li>
					   <span class="menu-aba-esquerda"></span>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Inicial","CodigoArmazemDoLivro")%>"><%=Resources.Recurso.Menu_CodigoArmazemDoLivro%></a>
					   <span class="menu-aba-direita"></span>
				    </li>--%>

                    <%-- <li>
					   <span class="menu-aba-esquerda"></span>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Inicial","AcompanhamentoClassroom")%>"><%=Resources.Recurso.Menu_AcompanhamentoClassroom%></a>
					   <span class="menu-aba-direita"></span>
				    </li>--%>
                     <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Inicial","ResultadoAvaliacao")%>"><%=Resources.Recurso.Menu_ResultadoAvaliacao%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>
                    <li>
					    <%--<span class="menu-aba-esquerda"></span>--%>
					    <a onclick="Bloqueio()" href="<%=Url.Action("Inicial","AnaliseRendimento")%>"><%=Resources.Recurso.Menu_AnaliseRendimento%></a>
					    <%--<span class="menu-aba-direita"></span>--%>
				    </li>
			    </ul><% 
		    } %>
       </div><!-- FIM menu responsivo-->
	</div><!-- FIM TOP-INFO -->
</div>