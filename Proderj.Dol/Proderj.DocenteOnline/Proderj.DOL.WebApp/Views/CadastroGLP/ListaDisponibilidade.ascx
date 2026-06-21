<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<Proderj.DOL.Service.DTOListaDISPONIBILIDADEGLP>>" %>
<% if (Model != null && Model.Count > 0) { %>
	<table style="table-layout: fixed; width: 600px;">
		<thead>
			<tr>
				<td style="width: 17px; text-align: center;">Ação</td>
                <td style="width: 70px; text-align: center;">Regional</td>
                <td style="width: 70px; text-align: center;">Município</td>
                <td style="width: 105px; text-align: center;">Unidade Escolar</td>
				<td style="width: 80px; text-align: center;">Disciplina</td>
                <td style="width: 90px; text-align: center;">Modalidade</td>
				<td style="width: 65px; text-align: center;">Dia da semana</td>
                <td style="width: 30px; text-align: center;">Turno</td>
			</tr>
		</thead>
		<tbody>
			<% foreach (var dtoDisponivel in Model) {  %>
				<tr>
					<td class="td-centro"><input type="image" src="<%=Url.Content("~/Imagens/ico_lixeira.png") %>" data-disponibilidade-glp-id="<%=dtoDisponivel.DISPONIBILIDADEGLPID %>" data-unidade-ens="<%=dtoDisponivel.UNIDADE_ENS %>" /></td>
                    <td><%=dtoDisponivel.REGIONAL %></td>
                    <td><%=dtoDisponivel.MUNICIPIO %></td>
                    <td><%=dtoDisponivel.UNIDADE_ESCOLAR %></td>
					<td><%=dtoDisponivel.DISCIPLINA %></td>
                    <td><%=dtoDisponivel.MODALIDADE %></td>
					<td><%=dtoDisponivel.DIASEMANA %></td>
                    <td><%=dtoDisponivel.TURNO %></td>
				</tr>
            <% } %>
		</tbody>
	</table>
<% } else { %>
	<p>Ainda não há disponibilidade cadastrada</p>
<% } %>