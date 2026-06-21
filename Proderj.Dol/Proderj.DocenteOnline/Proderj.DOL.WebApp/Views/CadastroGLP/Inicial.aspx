<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Proderj.DOL.WebApp.Models.CadastroGLPInicialViewModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
	<%=Model.TituloDaPagina %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
	<link rel="stylesheet" href="<%=Url.Action("Carrega","CSS", new { funcao = "cadastroglp-inicial"}) %>" />
    <script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "jquery" }) %>"></script>
	<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "cadastroglp-inicial" }) %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPage("<%= Model.CabecalhoModelo.DocenteLogadoModelo.Matricula %>");
        });
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Cabecalho" runat="server">
	<% Html.RenderPartial("Cabecalho", Model.CabecalhoModelo); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="server">
<div class="conteudo">
    
    <% Html.RenderPartial("DadosPessoais", Model.DadosPessoais); %>

    <br /><br /><br />

    <% using (Html.BeginForm("", "", FormMethod.Post, new { id="frmInclusaoDisponibilidade" })) {  %>
        <%= Html.AntiForgeryToken() %>
		
        <h3>Inclusão de disponibilidade</h3>
		<table id="table-inclusao">
            
            <tr>
				<td><label class="titulo-inline">Regional:*</label></td>
				<td>
					<input class="codigo-combo" type="text" maxlength="2" id="txtRegional" />
                    <select class="combo-media" id="cmbRegional" name="CodigoRegional">
                        <option value="">-- Selecione aqui --</option>
                    </select>
				</td>
			</tr>		

			<tr>
				<td><label class="titulo-inline">Município:*</label></td>
				<td>
					<input class="codigo-combo" type="text" maxlength="8" id="txtMunicipio" />
					<select class="combo-media" id="cmbMunicipio" name="CodigoMunicipio">
						<option value="">-- Selecione uma regional --</option>
					</select>
				</td>
			</tr>		

			<tr>
				<td><label class="titulo-inline">Unidade Escolar:*</label></td>
				<td>
					<input class="codigo-combo" type="text" maxlength="8" id="txtUE" />
					<select class="combo-media" id="cmbUE" name="CodigoUE">
						<option value="">-- Selecione um município --</option>
					</select>
				</td>
			</tr>

			<tr>
				<td><label class="titulo-inline">Disciplina:*</label></td>
				<td>
					<input class="codigo-combo" type="text" maxlength="4" id="txtDisciplina" />
					<select class="combo-media" id="cmbDisciplina" name="CodigoDisciplina">
                        <option value="">-- Selecione aqui --</option>
                    </select>
				</td>
			</tr>

            <tr>
                <td><label class="titulo-inline">Modalidade:*</label></td>
                <td>
                    Opção 1:&nbsp;&nbsp;<input type="checkbox" id="modalidade-medio" data-modalidade-opcao="1" value="Médio" name="CodigoModalidade" /><label for="modalidade-medio">Médio</label>&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" id="modalidade-fundamental-finais" name="CodigoModalidade" data-modalidade-opcao="1" value="Ensino Fundamental Anos Finais" /><label for="modalidade-fundamental-finais">Ensino Fundamental Anos Finais</label><br />
                    Opção 2:&nbsp;&nbsp;<input type="checkbox" id="modalidade-fundamental-iniciais" data-modalidade-opcao="2" value="Ensino Fundamental Anos Iniciais" name="CodigoModalidade" /><label for="modalidade-fundamental-iniciais">Ensino Fundamental Anos Iniciais</label>
                </td>
            </tr>

			<tr>
				<td><label class="titulo-inline">Dia da semana:*</label></td>
				<td>
					<input type="checkbox" id="dia-segunda" name="CodigoDiaSemana" value="2" /><label for="dia-segunda">Segunda</label>&nbsp;&nbsp;&nbsp;
                    <input type="checkbox" id="dia-terca" name="CodigoDiaSemana" value="3" /><label for="dia-terca">Terça</label>&nbsp;&nbsp;&nbsp;
                    <input type="checkbox" id="dia-quarta" name="CodigoDiaSemana" value="4" /><label for="dia-quarta">Quarta</label>&nbsp;&nbsp;&nbsp;
                    <input type="checkbox" id="dia-quinta" name="CodigoDiaSemana" value="5" /><label for="dia-quinta">Quinta</label>&nbsp;&nbsp;&nbsp;
                    <input type="checkbox" id="dia-sexta" name="CodigoDiaSemana" value="6" /><label for="dia-sexta">Sexta</label>&nbsp;&nbsp;&nbsp;
                    <input type="checkbox" id="dia-sabado" name="CodigoDiaSemana" value="7" /><label for="dia-sabado">Sábado</label>&nbsp;&nbsp;&nbsp;
				</td>
			</tr>

			<tr>
				<td><label class="titulo-inline">Turno:*</label></td>
				<td>
					<input type="checkbox" id="turno-manha" name="CodigoTurno" value="M" /><label for="turno-manha">Manhã</label>&nbsp;&nbsp;&nbsp;
                    <input type="checkbox" id="turno-tarde" name="CodigoTurno" value="T" /><label for="turno-tarde">Tarde</label>&nbsp;&nbsp;&nbsp;
                    <input type="checkbox" id="turno-noite" name="CodigoTurno" value="N" /><label for="turno-noite">Noite</label>&nbsp;&nbsp;&nbsp;
				</td>
			</tr>
		</table>

        <div class="botoes-acao">
			<input id="btIncluirDisponibilidade" type="button" value="Incluir" />
		</div>

        <div id="msg-erro" style="width: 100%; padding: 10px 0; display: none;"></div>

		<div class="clear"></div>

    <% }; %>

    <br /><br />
    <div id="msg-erro-grid" style="width: 100%; padding: 10px 0; display: none;"></div>
    <span style="color: #f00;">Solicitamos que a disponibilidade cadastrada seja excluída caso não tenha mais interesse em atuar com GLP.</span>
    <br /><br />

    <div class="tabela-padrao">
		<div class="tabela-cabecalho">
			<img class="aba-direita" src="<%=Url.Content("~/Imagens/tabela_cabecalho_abadir.gif")%>"/>
			<div class="centro">
				<img src="<%=Url.Content("~/Imagens/tabela_cabecalho_grid.png")%>"/></div>
			<div class="aba-esquerda"></div>
			<span>Horários disponíveis</span>
		</div>
        <div id="tabela-disponibilidade"></div>
	</div>
</div>
</asp:Content>

