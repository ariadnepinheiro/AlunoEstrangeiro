<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Proderj.DOL.WebApp.Models.CabecalhoViewModel>" %>

<% if (Model.DocenteLogadoModelo != null) { %>

<style type="text/css">

.cabecalho{position:fixed;top:0px;left:0px;width:100%;min-height:60px;background:#ffffff;box-shadow:0 0.5rem 1rem rgb(0 0 0 / 5%), inset 0 -1px 0 rgb(0 0 0 / 15%);z-index:10000;}
.cabecalho .ico_sair{float:right;display:flex;margin:1px 5px 1px 5px;width:80px;height:auto;border:2px solid #1E6496;background:#ffffff;color:#1E6496;text-align:center;border-radius:5px;text-decoration:none;line-height:20px;font-weight:600;justify-content:space-around;align-items:center;}
.cabecalho .ico_sair:hover{background-position:-60px;cursor:pointer;border:2px solid #1E6496;background:#1E6496;color:#ffffff;}
.cabecalho .menu-topo{width:2000px;margin:0px;padding:0px;margin-left:5px;padding-top:5px;border-top:2px solid #dfe6ed;}
.cabecalho .menu-topo li a{display:block;height:26px;width:96px;padding:0px 1px;background:#1E6496;color:#FFF;text-decoration:none;font-size:11px;padding:3px 5px;border-radius:5px;font-family:'Open Sans',Tahoma, Geneva, sans-serif;word-break: break-word;text-align: center;line-height: 11px;vertical-align: middle;}
.cabecalho .menu-topo li a:hover{text-decoration:underline;}
.cabecalho .menu-topo li{list-style:none;float:left;margin-right:2px;}
.top_inf{position:fixed;width:100%;background:#1E6496;color:#ffffff;box-shadow:0 0.5rem 1rem rgb(0 0 0 / 5%), inset 0 -1px 0 rgb(0 0 0 / 15%);height:65px;font-size:small;font-family:verdana, arial, helvetica;z-index:9999;}
.top_outros{position:relative;height:65px;}
.top_logo_governo{background-image:url("<%=Url.Content("~/imagens/logo-educacao-branca.png")%>");background-size:cover;width:164px;height:50px;margin:7px auto;position:absolute;left:20px;}
.top_logo_gestao{background-image:url("<%=Url.Content("~/imagens/logo-docente.png")%>");background-size:cover;width:135px;height:30px;margin-top:7px;position:absolute;left:210px;}
.tit_pag{position:absolute;top:20px;left:210px;height:20px;width:775px;}
.tit_pag > h5{width:260px;}
.top_usuario_logado
{
    height: 50px;
    margin: 15px auto;
    position: absolute;
    left: 480px;
    right: 115px;
    color: #ffffff;
    white-space: nowrap;
    overflow: hidden;
}
.top_usuario_logado > span{display:block;}


#top_menu
{
    width: 2000px;
    margin: 3px 3px;
}

@media (max-width:984px){

}
/*! CSS Used from: https://desenvolvimento.educacao.rj.gov.br/NovoFrontEndConexao2/site.css */
.top_botoes{width:170px;height:50px;margin:7px auto;position:absolute;right:10px;}
.imagem-menu{display:inline-block;height:50px;width:50px;background-repeat:no-repeat;background-position:center;background-size:35px;border:1px solid transparent;}
.imagem-home{background-image:url("<%=Url.Content("~/Imagens/icone-site-04.png")%>");}
.imagem-ajuda{background-image:url("<%=Url.Content("~/Imagens/icone-site-05.png")%>");}
.imagem-sair{background-image:url("<%=Url.Content("~/Imagens/icone-site-06.png")%>");}
a:link{text-decoration:none;color:#0353ab;}
a:visited{text-decoration:none;color:#0353ab;}
a:hover{text-decoration:underline;color:#666;}
a:active{text-decoration:none;color:#0353ab;}
</style>

<div class="top_inf cabecalho">
	
    <div class="top_outros">
        <!-- LOGO -->
        <div class="top_logo_governo"></div>
        <div class="top_logo_gestao"></div>
		<div class="tit_pag">
			<!-- Sessão -->  
			<h5 align="left"><%=Model.TituloCabecalho %></h5>
       	</div>
       	<!-- Dados do usuário logado -->  
        <div class="top_usuario_logado">
            <span>
            <% if (Model.DocenteLogadoModelo != null) { %>
				<b>ID/Vínculo:</b> <%=Model.DocenteLogadoModelo.IdFuncional %> / <%=Model.DocenteLogadoModelo.Vinculo %>
			<% } %>
            </span>
            <span><%=Model.DataPorExtenso %></span>
        </div>

      
        <div class="top_botoes">
            <!-- Botões - Home -->   
            <a id="ctl00_hlInicio" title="Início" class="imagem-menu imagem-home" href="<%=Url.Action("Apresentacao","Login") %>"></a>
            <!-- Botões - Sair -->
            <a id="ctl00_lbSair" title="Logoff" class="imagem-menu imagem-sair" href="<%=Url.Action("Desloga", "Login") %>"></a>
        </div>

      
            
    </div>
        
    <!-- MENU PRINCIPAL -->
    <div id="top_menu">                        
        
        <% if (Model.DocenteLogadoModelo != null && Model.DocenteLogadoModelo.AceitouTermoDeAceite) { %>
        <!-- NAVBAR - MENU INTERNO -->
		<ul class="menu-topo">
    	    <li>
                <a onclick="Bloqueio()" href="<%=Url.Action("LancamentoFrequencia","Api")%>">Lançamento de Frequência</a>
    	    </li>
			<li>
				<a onclick="Bloqueio()" href="<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "LancamentoNotas" })%>"><%=Resources.Recurso.Menu_LancamentoDeNota%></a>
			</li>
			<li>
				<a onclick="Bloqueio()" href="<%=Url.Action("CadastroGLP","Api")%>">Cadastramento para GLP</a>
			</li>

			<li>
				<a id="link-QHI" href="#"><%=Resources.Recurso.Menu_ConsultaAlocacaoQHI%></a>
			</li>

		  <%--	<li>
				<a onclick="Bloqueio()" href="<%=Url.Action("Lista","SelecaoTurmas", new { nomeController = "RespostaCurriculoMinimo" })%>"><%=Resources.Recurso.Menu_CurriculoMinimo%></a>
			</li>--%>

			<li>
				<a onclick="Bloqueio()" href="<%=Url.Action("ProtocoloNota","Api")%>">Protocolos</a>
			</li>

            <li>
				<a onclick="Bloqueio()" href="<%=Url.Action("DadosDocente","Api")%>">Consulta ao meu cadastro</a>
			</li>

            <li>
				<a onclick="Bloqueio()" href="<%=Url.Action("DadosPessoais","Api")%>">Dados Pessoais</a>
			</li>

           <%-- <li>
				<a onclick="Bloqueio()" href="<%=Url.Action("Inicial","CodigoArmazemDoLivro")%>"><%=Resources.Recurso.Menu_CodigoArmazemDoLivro%></a>
			</li>--%>

           <%--  <li>
				<a onclick="Bloqueio()" href="<%=Url.Action("Inicial","AcompanhamentoClassroom")%>"><%=Resources.Recurso.Menu_AcompanhamentoClassroom%></a>
			</li>--%>
            <li>
				<a onclick="Bloqueio()" href="<%=Url.Action("Inicial", "ResultadoAvaliacao")%>"><%=Resources.Recurso.Menu_ResultadoAvaliacao%></a>
			</li>
             <li>
				<a onclick="Bloqueio()" href="<%=Url.Action("Inicial", "AnaliseRendimento")%>"><%=Resources.Recurso.Menu_AnaliseRendimento%></a>
			</li>

            <li>
				<a onclick="Bloqueio()" href="<%=Url.Action("AlteraSenha","Api")%>">Alterar Senha</a>
			</li>
		</ul>
        <% } %>
                             
    </div>

</div>

<% } else { %>

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
			    <span><img src="<%=Url.Content("~/Imagens/logo-docente-azul.png")%>" /></span>
		    </div>

            <!--  DATA E INFO DO USUÁRIO -->
		    <div class="saudacao" align="left">
			    <div id="div1"><%=Model.DataPorExtenso %></div>
		    </div>
        
             <!-- BOTÕES INICIO / AJUDA  / SAIR -->
            
            <div class="tit_pag">       
                <a class="icon-externo" href="https://www.seeduc.rj.gov.br/" target="_blank">Site Seeduc</a>     
            </div>

		    
       </div><!-- FIM menu responsivo-->
	</div><!-- FIM TOP-INFO -->
</div>

<div class="top_wrapper"></div>

<% } %>