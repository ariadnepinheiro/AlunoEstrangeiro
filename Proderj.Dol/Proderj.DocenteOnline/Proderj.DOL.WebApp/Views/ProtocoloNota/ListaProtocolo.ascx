<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<Proderj.DOL.Service.DTOProtocoloNotaComData>>" %>
<script type="text/javascript" src="<%=Url.Action("Carrega", "JS", new { funcao = "protocolonota-paginacao" }) %>"></script>
<% if (Model != null && Model.Count > 0)
   { %>
<table>
    <thead>
        <tr>
            <td>
                Data
            </td>
            <td>
                Nº Protocolo
            </td>
            <td title="Tipo" class="tipo">
                T
            </td>
            <td>
                Ano
            </td>
            <td title="Período" class="periodo">
                P
            </td>
            <td title="Bimestre" class="bimestre">
                B
            </td>
            <td>
                Turma
            </td>
            <td class="disciplina">
                Disciplina
            </td>
        </tr>
    </thead>
    <tbody>
        <%
       var contador = -1;
       var numeroPagina = 0;
       var rnModel = new Proderj.DOL.WebApp.Models.ProtocoloNota.ProtocoloNotaListaViewModel();
       rnModel.RegistrosPorPagina = 10;
       foreach (var dtoProtocolo in Model)
       {
           contador++;
           if (contador % rnModel.RegistrosPorPagina == 0)
               numeroPagina++;
                
        %>
        <tr class="pagina pagina-<%=numeroPagina %>">
            <td class="data">
                <%=dtoProtocolo.DataCadastro %>
            </td>
            <td class="protocolo">
                <%=dtoProtocolo.Codigo %>
            </td>
            <td>
                <%=dtoProtocolo.Tipo %>
            </td>
            <td>
                <%=dtoProtocolo.Ano %>
            </td>
            <td>
                <%=dtoProtocolo.Periodo %>
            </td>
            <td>
                <%=dtoProtocolo.SubPeriodo %>
            </td>
            <td>
                <%=dtoProtocolo.CodigoTurma %>
            </td>
            <td class="disciplina">
                <%=dtoProtocolo.NomeDisciplina %>
            </td>
        </tr>
        <% 
            } %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="8">
                <span>Total de
                    <%=Model.Count %>
                    itens</span>
                <% if (numeroPagina > 1)
                   {
                %>
                <%=Html.DropDownList("ProtocoloNota", rnModel.ListaPaginasAte(numeroPagina, 1), new { id = "cmbMudarPagina" })%>
                <%} %>
            </td>
        </tr>
    </tfoot>
</table>
<% } else { %>
	<p>Nenhum registro encontrado para o filtro informado.</p>
<% } %>