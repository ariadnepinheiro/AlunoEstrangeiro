<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Proderj.DOL.WebApp.Models.DadosPessoaisViewModel>" %>

<form id="frmDadosPessoais" style="width: 600px;">
	<h3>Prezado(a) Professor(a),<br />Verifique e atualize os dados cadastrados no quadro abaixo:</h3>
		
    <input type="hidden" id="pessoa" />
    <input type="hidden" id="matricula" />

    <table id="table-dados-pessoais">
        <tr>
            <td>País:*</td>
            <td><select id="pais" style="width: 229px;"></select></td>
        </tr>

        <tr>
            <td>CEP:*</td>
            <td><input type="text" id="cep" style="width: 80px;" maxlength="9" /></td>
        </tr>

        <tr>
            <td>Endereço:*</td>
            <td colspan="3"><input type="text" id="endereco" maxlength="50" style="width: 100%;" /></td>
        </tr>

        <tr>
            <td>Nº:*</td>
            <td><input type="text" id="numero" maxlength="15" style="width: 50px;" /></td>

            <td>Complemento:</td>
            <td><input type="text" id="complemento" maxlength="50" style="width: 100%;" /></td>
        </tr>

        <tr>
            <td>Bairro:*</td>
            <td><input type="text" id="bairro" maxlength="50" style="width: 122px;" /></td>
        </tr>

        <tr>
            <td>Município:*</td>
            <td>
                <input type="text" id="municipio-id" maxlength="20" style="width: 50px;" />
                <select id="municipio" style="width: 166px;"></select>
            </td>

            <td>Estado:*</td>
            <td><select id="uf" style="width: 50px;"></select></td>
        </tr>

        <tr>
            <td>Localização/Zona de Residência:*</td>
            <td colspan="3">
                <label><input type="radio" id="localizacao-zona-rural" name="localizacao-zona" value="Rural" />Rural</label>&nbsp;&nbsp;&nbsp;
                <label><input type="radio" id="localizacao-zona-urbana" name="localizacao-zona" value="Urbana" />Urbana</label>
            </td>
        </tr>

        <tr>
            <td>Localização Diferenciada:*</td>
            <td colspan="3">
                <label><input type="checkbox" id="localizacao-diferenciada-na" />Não se aplica</label>&nbsp;&nbsp;
                <label><input type="checkbox" id="localizacao-diferenciada-assentamento" />Área de assentamento</label>&nbsp;&nbsp;
                <label><input type="checkbox" id="localizacao-diferenciada-quilombos" />Área remanescente de quilombos</label>&nbsp;&nbsp;
                <label><input type="checkbox" id="localizacao-diferenciada-indigena" />Terra indígena</label>
            </td>
        </tr>

        <tr>
            <td>Telefone Fixo:</td>
            <td><input type="text" id="telefone" maxlength="30" style="width: 122px;" /></td>

            <td>Celular:</td>
            <td><input type="text" id="celular" maxlength="30" style="width: 100%;" /></td>
        </tr>

        <tr>
            <td>E-mail institucional:</td>
            <td><input type="text" id="email-institucional" maxlength="100" style="width: 220px; background-color: #ededed;" readonly="readonly" /></td>
        </tr>

        <tr>
            <td>E-mail educa:</td>
            <td><input type="text" id="email-educa" maxlength="100" style="width: 220px; background-color: #ededed;" readonly="readonly" /></td>
        </tr>

        <tr>
            <td>E-mail alternativo:</td>
            <td><input type="text" id="email" maxlength="100" style="width: 220px;" /></td>
        </tr>

        <tr><td>&nbsp;</td></tr>

    </table>

	<div class="botoes-acao">
		<input type="button" id="btAlterarDocente" value="Aplicar alteração"/>
	</div>

    <div id="msg-erro-dados-pessoais"></div>

    <div class="clear"></div>
</form>

<div style="margin-top: 30px; font-size: 12px; font-weight: bold;">Dados do Docente:</div>

<table id="dados-docente" rules="all">
    <tr>
        <td style="width: 166px;">NOME</td>
        <td id="docente-nome1" style="width: 129px;"></td>
        <td style="width: 166px;">NOME</td>
        <td id="docente-nome2" style="width: 129px;"></td>
    </tr>
        <tr>
        <td style="width: 166px;">NOME SOCIAL</td>
        <td id="docente-nomesocial1" style="width: 129px;"></td>
        <td style="width: 166px;">NOME SOCIAL</td>
        <td id="docente-nomesocial2" style="width: 129px;"></td>
    </tr>
    <tr>
        <td>CPF</td>
        <td id="docente-cpf1"></td>
        <td>CPF</td>
        <td id="docente-cpf2"></td>
    </tr>
    <tr>
        <td>MATRÍCULA</td>
        <td id="docente-matricula1"></td>
        <td>MATRÍCULA</td>
        <td id="docente-matricula2"></td>
    </tr>
	    <tr>
        <td>ID/VÍNCULO</td>
        <td id="docente-idvinculo1"></td>
        <td>ID/VÍNCULO</td>
        <td id="docente-idvinculo2"></td>
    </tr>   
    <tr>
        <td>SITUAÇÃO</td>
        <td id="docente-situacao1"></td>
        <td>SITUAÇÃO</td>
        <td id="docente-situacao2"></td>
    </tr>
    <tr>
        <td>FUNÇÃO</td>
        <td id="docente-funcao1"></td>
        <td>FUNÇÃO</td>
        <td id="docente-funcao2"></td>
    </tr>
    <tr>
        <td>DISCIPLINA DE INGRESSO / APROVEITAMENTO</td>
        <td id="docente-disciplina-ingresso1"></td>
        <td>DISCIPLINA DE INGRESSO / APROVEITAMENTO</td>
        <td id="docente-disciplina-ingresso2"></td>
    </tr>
    <tr>
        <td>CARGO</td>
        <td id="docente-cargo1"></td>
        <td>CARGO</td>
        <td id="docente-cargo2"></td>
    </tr>
    <tr>
        <td>CARGA H. DE REGÊNCIA</td>
        <td id="docente-ch-regencia1"></td>
        <td>CARGA H. DE REGÊNCIA</td>
        <td id="docente-ch-regencia2"></td>
    </tr>
    <tr>
        <td>CARGA H. EM TURMA</td>
        <td id="docente-hor-tur1"></td>
        <td>CARGA H. EM TURMA</td>
        <td id="docente-hor-tur2"></td>
    </tr>
    <tr>
        <td>CARGA H. NORMAL</td>
        <td id="docente-tol-normal1"></td>
        <td>CARGA H. NORMAL</td>
        <td id="docente-tol-normal2"></td>
    </tr>
    <tr>
        <td>CARGA H. GLP</td>
        <td id="docente-tol-glp1"></td>
        <td>CARGA H. GLP</td>
        <td id="docente-tol-glp2"></td>
    </tr>
    <tr>
        <td>REGIONAL</td>
        <td id="docente-regional1"></td>
        <td>REGIONAL</td>
        <td id="docente-regional2"></td>
    </tr>
    <tr>
        <td>MUNICÍPIO</td>
        <td id="docente-municipio1"></td>
        <td>MUNICÍPIO</td>
        <td id="docente-municipio2"></td>
    </tr>
    <tr>
        <td>UA DE LOTAÇÃO</td>
        <td id="docente-ua-lotacao1"></td>
        <td>UA DE LOTAÇÃO</td>
        <td id="docente-ua-lotacao2"></td>
    </tr>
    <tr>
        <td>UNIDADE ADMINISTRATIVA</td>
        <td id="docente-unidade-adm1"></td>
        <td>UNIDADE ADMINISTRATIVA</td>
        <td id="docente-unidade-adm2"></td>
    </tr>
</table>