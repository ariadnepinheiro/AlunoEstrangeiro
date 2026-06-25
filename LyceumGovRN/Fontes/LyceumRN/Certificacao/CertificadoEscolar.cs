using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.RN.Certificacao
{
    public class CertificadoEscolar
    {
        RN.Certificacao.TipoDocumentoCertifica rntipodocumentocertifica = new TipoDocumentoCertifica();
        RN.Certificacao.DocumentoCertificacao rnDocumentoCertificacao = new DocumentoCertificacao();
        RN.Certificacao.Util rnUtil = new Util();
        RN.Certificacao.DocumentoGerado rnDocumentoGerado = new DocumentoGerado();
        RN.Certificacao.SignWrappers rnAssinatura = new SignWrappers();

        public string GerarCertificadoDiplomaEscolarCertidao(List<String> Aluno, int TipoConclusao, string usuariologado, int tipoDocumentoSelecionado)
        {
            string corpo = string.Empty;
            string erro = corpo;
            string result = corpo;
            string html = corpo;

            foreach (var item in Aluno)
            {
                result = this.GerarCertificadoDiplomaEscolarCertidao(item, TipoConclusao, usuariologado, tipoDocumentoSelecionado);
                if (result.Substring(2, 6) != "<html>")
                {
                    erro += "<tr><td> " + item + "</td>";
                    erro += "<td>" + result + "</td></tr>";
                }
                else
                {
                    corpo += result;
                }
            }

            if (!string.IsNullOrEmpty(erro) & !string.IsNullOrEmpty(corpo))
            {
                html = corpo + MontarErroCertidaoEmLote(erro, tipoDocumentoSelecionado);
            }
            else
                if (!string.IsNullOrEmpty(erro))
                    html = MontarErroCertidaoEmLote(erro, tipoDocumentoSelecionado);
                else
                    html = corpo;

            return html;
        }

        public string GerarCertificadoDiplomaEscolarCertidao(String Aluno, int TipoConclusao, string usuariologado, int tipoDocumentoSelecionado)
        {
            DataTable dtdisciplinascursadas = null;
            DataTable dtdocumentocertificacao = null;
            DataTable dtlistarCursoUnidade = null;
            string numero, folhas, livro;
            int ano, semestre, documentoCertId, documentoID;
            string corpoCertificado = null;
            RN.Aluno rnAluno = new RN.Aluno();
            RN.DTOs.DadosFichaAluno dadosAluno = new Techne.Lyceum.RN.DTOs.DadosFichaAluno();
            Certificacao.Entidades.DocumentoGerado dadosDocumentoGerado = new Certificacao.Entidades.DocumentoGerado();
            string tipoCurso, turno, curso, tipo_curso, curriculo, serie, unidade_Ens, municipioEscola, modalidadeNivel, modalidade, UA, numerocertificadoAnt, numerocertificado, msgErro, obs, nivel;
            Boolean podeEmitirCertificado = false;
            string corpopronto = null;

            //Campos do certificado
            string identificacaoCompletadaUE, atodeCriacao, regional, nomeCompletodoConcluinte, nacionalidade, tipoDocumentodeIdentificacao, numeroDocumentodeIdentificacao, orgaoDocumentodeIdentificacao, ufDocumentodeIdentificacao, filiacaoMae, filiacaoPai, ufNaturalidade, dtNascimetnoCompletaporExtenso, identificacaodoCurso, cargaHorariaTotal, datadeConclusaoporExtenso, filiacao, eixo;

            string atodeAutorizacaodoCurso = string.Empty;

            try
            {
                //Busca as disciplinas cursadas pelo Aluno de acordo com o Tipo de Conclusão

                #region Busca Disciplinas do Aluno
                dtdisciplinascursadas = ListarDisciplinasCursadas(Aluno, TipoConclusao);

                if (dtdisciplinascursadas.Rows.Count == 0)
                    return msgErro = "Não foi possível obter as disciplinas cursadas.";

                turno = dtdisciplinascursadas.Rows[0]["turno"].ToString();
                curso = dtdisciplinascursadas.Rows[0]["curso"].ToString();
                tipo_curso = dtdisciplinascursadas.Rows[0]["tipo_curso"].ToString() == "" ? "" : ", na forma " + dtdisciplinascursadas.Rows[0]["tipo_curso"].ToString();

                curriculo = dtdisciplinascursadas.Rows[0]["curriculo"].ToString();
                serie = dtdisciplinascursadas.Rows[0]["serie"].ToString();
                unidade_Ens = dtdisciplinascursadas.Rows[0]["unidade_Ens"].ToString();
                UA = dtdisciplinascursadas.Rows[0]["UA"].ToString();
                // modalidadeNivel = dtdisciplinascursadas.Rows[0]["modalidadeNivel"].ToString();
                modalidadeNivel = dtdisciplinascursadas.Rows[0]["NOMECERTIFICACAO"].ToString();

                municipioEscola = RN.UnidadeEnsino.RetornaNomeMunicipio(unidade_Ens);
                if (!municipioEscola.IsNullOrEmptyOrWhiteSpace())
                {
                    municipioEscola = RN.Util.Utils.Capitaliza(municipioEscola);
                }

                nivel = dtdisciplinascursadas.Rows[0]["TIPO"].ToString();
                tipoCurso = dtdisciplinascursadas.Rows[0]["TIPO_CURSO"].ToString();
                modalidade = dtdisciplinascursadas.Rows[0]["modalidade"].ToString();
                ano = Convert.ToInt32(dtdisciplinascursadas.Rows[0]["ano"].ToString());
                semestre = Convert.ToInt32(dtdisciplinascursadas.Rows[0]["semestre"].ToString());

                #endregion

                //Verificar se a serie do curso permite a emissão de Certificado

                #region Verifica possibilidade de Emitir Certificado

                podeEmitirCertificado = EmiteCertificacao(turno, curso, curriculo, serie);

                if (!podeEmitirCertificado)
                    return msgErro = "Sua série " + serie + " não permite emitir o documento requisitado.";
                #endregion

                //Faz o somatório da Carga Horária das disciplnas

                #region Carga Horária

                //verificar o total de carga horária
                string cargaHoraria = String.Format("{0:0,0}", dtdisciplinascursadas.AsEnumerable().Sum(row => row.Field<decimal>("CARGA_HORARIA")));
                decimal chTotal = dtdisciplinascursadas.AsEnumerable().Sum(row => row.Field<decimal>("CARGA_HORARIA"));

                if (string.IsNullOrEmpty(cargaHoraria))
                    return msgErro = "Não foi possível calcular a carga horária.";

                #endregion

                //Buscar informações de onde o certificado foi registrado

                #region Dados de Registro do Certificado

                //tipo de conclusão
                //1	FUNDAMENTAL
                //2	MÉDIO
                //3	PROFISSIONALIZANTE

                //tipo Documento
                //1		Histórico Escolar
                //2		Certidão
                //3		Certificado Escolar
                //4		Diploma

                dtdocumentocertificacao = rnDocumentoCertificacao.Listar(Aluno, TipoConclusao, tipoDocumentoSelecionado);

                if (dtdocumentocertificacao.Rows.Count == 0)
                    return msgErro = "Não foi possível obter dados do livro para gerar o documento requisitado.";

                documentoCertId = Convert.ToInt32(dtdocumentocertificacao.Rows[0]["documentoCertId"]);

                documentoID = dtdocumentocertificacao.Rows[0]["DOCUMENTOID"] != null ? Convert.ToInt32(dtdocumentocertificacao.Rows[0]["DOCUMENTOID"]) : 0;//TIPO DO DOCUMENTO

                numero = dtdocumentocertificacao.Rows[0]["NUMERO"].ToString();

                folhas = dtdocumentocertificacao.Rows[0]["FOLHAS"].ToString();

                livro = dtdocumentocertificacao.Rows[0]["LIVRO"].ToString();

                obs = dtdocumentocertificacao.Rows[0]["OBSERVACAO"].ToString() != "" ? dtdocumentocertificacao.Rows[0]["OBSERVACAO"].ToString() : " - ";
                eixo = dtdocumentocertificacao.Rows[0]["EIXO"].ToString();

                #endregion

                //Buscar corpo do certificado, onde os dados serão subistituidos 

                #region Dados do Certificado

                corpoCertificado = rntipodocumentocertifica.ListarCorpopor(tipoDocumentoSelecionado);

                if (corpoCertificado == null)
                    return msgErro = "Não foi possível obter o texto para compor o documento solicitado.";

                #endregion

                //Busca os dados do aluno passando o parâmetro matrícula

                #region Dados do Aluno



                //202317940625910
                dadosAluno = rnAluno.ObtemFichaAlunoPor(Aluno);

                //essa informação vem da última escola que o aluno cursou "33013551"
                DataTable dtUnidade = UnidadeEnsinoSituacao.ListarAtoCriacaoeRegional(unidade_Ens);

                //Retirado para diploma e certificado
                if (tipoDocumentoSelecionado != 4 && tipoDocumentoSelecionado != 3)
                {
                    if (dtUnidade.Rows.Count == 0)
                        return msgErro = "Não foi possível obter as informações sobre a criação da Unidade de Ensino(Histórico da Unidade).";
                }

                identificacaoCompletadaUE = string.Empty;
                atodeCriacao = string.Empty;
                regional = string.Empty;

                if (dtUnidade.Rows.Count > 0)
                {
                    identificacaoCompletadaUE = dtUnidade.Rows[0]["NOME_COMP"].ToString();
                    atodeCriacao = dtUnidade.Rows[0]["NUMERO_ATO_OFICIAL"] != null ? dtUnidade.Rows[0]["NUMERO_ATO_OFICIAL"].ToString() : string.Empty;
                    regional = dtUnidade.Rows[0]["REGIONAL"] != null ? dtUnidade.Rows[0]["REGIONAL"].ToString() : string.Empty;
                }

                nomeCompletodoConcluinte = dadosAluno.NomeAluno.Trim();
                nacionalidade = dadosAluno.Nacionalidade.ToLower();

                if (string.IsNullOrEmpty(dadosAluno.TipoDocumento.ToString()) || string.IsNullOrEmpty(dadosAluno.NumeroDocumento.ToString()) || string.IsNullOrEmpty(dadosAluno.OrgaoEmissorDocumento.ToString()) || string.IsNullOrEmpty(dadosAluno.EstadoDocumento.ToString()))
                {
                    tipoDocumentodeIdentificacao = "NÃO INFORMADO";
                    numeroDocumentodeIdentificacao = "NÃO INFORMADO";
                    orgaoDocumentodeIdentificacao = "NÃO INFORMADO";
                    ufDocumentodeIdentificacao = "NÃO INFORMADO";
                }
                else
                {
                    tipoDocumentodeIdentificacao = dadosAluno.TipoDocumento + " nº " + dadosAluno.NumeroDocumento;
                    //numeroDocumentodeIdentificacao = dadosAluno.NumeroDocumento;
                    orgaoDocumentodeIdentificacao = dadosAluno.OrgaoEmissorDocumento;
                    ufDocumentodeIdentificacao = dadosAluno.EstadoDocumento;
                }

                filiacaoMae = RN.Util.Utils.Capitaliza(dadosAluno.NomeMae.ToString());
                filiacaoPai = RN.Util.Utils.Capitaliza(dadosAluno.NomePai.ToString());
                filiacao = string.Empty;

                if (!string.IsNullOrEmpty(filiacaoMae) && !string.IsNullOrEmpty(filiacaoPai))
                    filiacao = filiacaoMae + " e " + filiacaoPai;
                else
                {
                    if (string.IsNullOrEmpty(filiacaoPai))
                        filiacao = filiacaoMae;
                    else if (string.IsNullOrEmpty(filiacaoMae))
                        filiacao = filiacaoPai;
                }

                if (!string.IsNullOrEmpty(dadosAluno.Naturalidade) && dadosAluno.Naturalidade.Trim() != "/")
                {
                    // Brasileiro: município e UF já vêm no DadosFichaAluno
                    ufNaturalidade = RN.Util.Utils.Capitaliza(dadosAluno.Naturalidade + " / " + dadosAluno.UfNascimento);
                }
                else
                {
                    // Nascido no estrangeiro: busca na HD_MUNICIPIO_CERTIFICACAO
                    DataTable dtNatEst = rnAluno.ObtemNaturalidadeEstrangeiraPor(Aluno);

                    if (dtNatEst != null && dtNatEst.Rows.Count > 0)
                    {
                        string munExt = dtNatEst.Rows[0]["NOME_MUNICIPIO"].ToString();
                        string ufExt = dtNatEst.Rows[0]["NOME_ESTADO"].ToString();
                        string paisExt = dtNatEst.Rows[0]["NOME_PAIS"].ToString();

                        // Brasileiro nascido fora do Brasil: "Município - UF - País"
                        if ((!string.IsNullOrEmpty(nacionalidade) && nacionalidade.Trim().ToUpper() == "BRASILEIRA"))
                            ufNaturalidade = RN.Util.Utils.Capitaliza(munExt + " / " + ufExt + " - " + paisExt);

                        // Exclusivamente estrangeiro: somente Município + País
                        else
                            ufNaturalidade = RN.Util.Utils.Capitaliza(munExt + " / " + paisExt);
                    }
                    else
                    {
                        ufNaturalidade = "Não Informado";
                    }
                }

                dtNascimetnoCompletaporExtenso = rnUtil.DataporExtenso(dadosAluno.DataNascimento);

                #endregion

                //Busca os dados do Curso

                #region  Dados do Curso

                dtlistarCursoUnidade = ListarCursoUnidade(unidade_Ens, curso);
                string ato_curso = string.Empty;
                string dt_ato = string.Empty;
                if (dtlistarCursoUnidade.Rows.Count > 0)
                {
                    var ato = Convert.ToString(dtlistarCursoUnidade.Rows[0]["ato"]);

                    if (!ato.IsNullOrEmptyOrWhiteSpace())
                    {
                        //    return msgErro = "Não foi possível obter as informações da criação do curso. (Implantação de Cursos/Modalidade)";
                        ato = RN.Util.Utils.Capitaliza(dtlistarCursoUnidade.Rows[0]["ato"].ToString());
                    }
                    dt_ato = Convert.ToString(dtlistarCursoUnidade.Rows[0]["dt_do"]);
                }
                //if (string.IsNullOrEmpty(ato_curso) || string.IsNullOrEmpty(dt_ato))
                //    return msgErro = "Não foi possível obter as informações da criação do curso.";

                RN.HCursosConcl rnHCursosConcl = new HCursosConcl();

                DateTime? dtFim = rnHCursosConcl.ObtemDataMotivoConclusaoPor(Aluno);

                if (dtFim == null)
                {
                    dtFim = BuscaDataFim(ano, semestre);
                }

                #endregion

                // se for diploma e a conclusão for profissionalizante , adiciona a informação.... falta validar com o Léo

                #region Curso Profissionalizante e Diploma

                //tipo de conclusão
                //1	FUNDAMENTAL
                //2	MÉDIO
                //3	PROFISSIONALIZANTE

                //tipo Documento
                //1		Histórico Escolar
                //2		Certidão
                //3		Certificado Escolar
                //4		Diploma



                if ((tipoDocumentoSelecionado == 4) & (TipoConclusao == 3))

                    if (!string.IsNullOrEmpty(eixo))
                        identificacaodoCurso = modalidadeNivel + ", com eixo " + eixo + " " + tipo_curso;
                    else
                        identificacaodoCurso = modalidadeNivel + " " + tipo_curso;
                else
                    identificacaodoCurso = modalidadeNivel;

                string dataAto = string.Empty;

                if (!dt_ato.IsNullOrEmptyOrWhiteSpace())
                {
                    dataAto = rnUtil.DataporExtenso(DateTime.Parse(dt_ato));
                }

                if (!ato_curso.IsNullOrEmptyOrWhiteSpace() && !dataAto.IsNullOrEmptyOrWhiteSpace())
                {
                    atodeAutorizacaodoCurso = ato_curso + " de " + dataAto;
                }
                cargaHorariaTotal = cargaHoraria;
                string cargaHorariaRelogio = String.Format("{0:0,0}", ((chTotal * 50) / 60));
                datadeConclusaoporExtenso = dtFim != DateTime.MinValue ? rnUtil.DataporExtenso(dtFim.Value) : "NÃO INFORMADO";

                #endregion

                corpopronto = corpoCertificado.Replace("#UE", identificacaoCompletadaUE).Replace("#atoCriação", atodeCriacao).Replace("#nomecompleto", nomeCompletodoConcluinte).Replace("#nacionalidade", nacionalidade).Replace("#RG", tipoDocumentodeIdentificacao).Replace("#orgaoExpedidor", orgaoDocumentodeIdentificacao).Replace("#UFexpedicao", ufDocumentodeIdentificacao).Replace("#filiação", filiacao).Replace("#UFnatural", ufNaturalidade).Replace("#dataNacimentoExtenso", dtNascimetnoCompletaporExtenso).Replace("#modalidade", identificacaodoCurso).Replace("#atoAutorizacaoCurso", atodeAutorizacaodoCurso).Replace("#cargaHoraria", cargaHorariaTotal).Replace("#dataConclusaoExntenso", datadeConclusaoporExtenso).Replace("#nomeAluno", nomeCompletodoConcluinte).Replace("#horasRelogio", cargaHorariaRelogio).Replace("#numCertidao", numero).Replace("#folhaCertidao", folhas).Replace("#livroCertidao", livro);

                //se for DIPLOMA verifica a modalidade, se for CERTIDÃO verifica ano de conclusão

                #region Tipo Documento Selecionado

                switch (tipoDocumentoSelecionado)
                {
                    case 2: //2		Certidão
                        if (semestre != 0)
                            corpopronto = corpopronto.Replace("#anoLetivodeconclusao", semestre + "/" + ano);
                        else
                            corpopronto = corpopronto.Replace("#anoLetivodeconclusao", ano.ToString());

                        break;


                    case 4://4		Diploma

                        //verifica se o curso é da modalidade normal, caso seja acresenta as inf do diploma
                        if (modalidade == "NO9")
                            corpopronto = corpopronto.Replace("#normal", "Em especial o exercício do magistério na educação infantil e nos cinco primeiros anos do ensino fundamental.");
                        else
                            corpopronto = corpopronto.Replace("#normal", ".");

                        break;

                    default:
                        break;
                }

                #endregion

                //Gera o Número do Certificado

                #region Número do Certificado

                //tipo Documento
                //1		Histórico Escolar
                //2		Certidão
                //3		Certificado Escolar
                //4		Diploma

                // Verificar método
                numerocertificadoAnt = GerarNumeroCertificado(UA, unidade_Ens, dtFim.Value.Year.ToString(), documentoCertId, TipoConclusao);

                //tipo Validador
                //01    Declaração		
                //02    Certidão		
                //03    Certificado		
                //04    Diploma		
                //05    Histórico
                //06    Transferencia

                int sequencial = 0;
                numerocertificado = GerarCodigoValidador(unidade_Ens, curso, modalidade, nivel, tipo_curso, tipoDocumentoSelecionado, dtFim.Value.Year, documentoCertId, out sequencial);

                if (numerocertificado == null)
                    return msgErro = "Erro ao gerar o número do certificado.";

                #endregion

                //Monta a estrututura do Html para o PDF

                corpopronto = MontarHistoricoFundamental(corpopronto, identificacaoCompletadaUE, livro, folhas, numero, obs, numerocertificado, tipoDocumentoSelecionado, regional, municipioEscola);

                dadosDocumentoGerado.DOCUMENTOCERTID = documentoCertId;
                dadosDocumentoGerado.NUMEROGERADO = numerocertificadoAnt;
                dadosDocumentoGerado.UsuarioId = usuariologado;

                //Salva os dado do documento na tabela de documento gerado.
                //aqui será feito o insert ou update
                rnDocumentoGerado.Manter(dadosDocumentoGerado, unidade_Ens, sequencial, numerocertificado);
            }
            catch (Exception ex)
            {
                corpopronto = "Erro ao GerarCertificadoDiplomaEscolar" + ex.Message;
            }

            return corpopronto;
        }

        public string MontarHistoricoFundamental(string corpo, string NomeUe, string livro, string folha, string numero, string obs, string numerocertificado, int tipoDocumentoSelecionado, string regional, string municipio)
        {
            //string img1 = HttpContext.Current.Server.MapPath("Img/logoRJ.PNG");
            string img1 = HttpContext.Current.Server.MapPath("Img/Brasao.PNG");
            string img2 = HttpContext.Current.Server.MapPath("Img/Assinatura.PNG");
            string img3 = HttpContext.Current.Server.MapPath("Img/SELONACIONAL.jpg");

            string strtipoDocumentoSelecionado = string.Empty;
            string strEste = string.Empty;
            string strRegistrado = string.Empty;

            //if (tipoDocumentoSelecionado == 4)
            //    strtipoDocumentoSelecionado = "diploma";
            //else if (tipoDocumentoSelecionado == 3)
            //    strtipoDocumentoSelecionado = "certificado";

            switch (tipoDocumentoSelecionado)
            {
                case 2:
                    strtipoDocumentoSelecionado = "certidão";
                    strEste = "Esta";
                    strRegistrado = "registrada";
                    break;

                case 3:
                    strtipoDocumentoSelecionado = "certificado";
                    strEste = "Este";
                    strRegistrado = "registrado";
                    break;

                case 4:
                    strtipoDocumentoSelecionado = "diploma";
                    strEste = "Este";
                    strRegistrado = "registrado";
                    break;

                default:
                    break;
            }

            string html = string.Empty;
            try
            {
                html = string.Format(@"
<html>
<head>
      <meta charset='utf-8'></meta>
      <title>{10}</title>
	  
    </head>
<body>
<page size='A4'>

<div id='tudo'>
        <div id='cabecalho1' align='center'>
        <table  width='100%'>
        <tr>
            <td><img src='{16}'></img></td>
            <td align='right'><img src='{3}'></img></td>
        </tr>
        </table>        
        <p class='c'>Governo do Estado do Rio de Janeiro</p>
        <p class='c'>Secretaria de Estado de Educação </p>
        <p><b>{0}</b></p>
        </div>

        <div id='cabecalho2'>
        <p>{11}<br/>{10}</p>
        </div>
        <br/>
        <div id='corpo'>
        <p>{1}</p>
		<p><strong>OBSERVAÇÕES:</strong> {8}</p>
    </div>
    <div id='rodape'>

    <p class='x'>{17}, {2}.</p>
    <p>Confere</p>
    <p class='y'>Secretário Escolar</p>
    <p>Visto</p>
    <p class='y'>Direção da Unidade Escolar</p>
    

    <br/>


        <table border='1' style='align='center';'>
	        <tr>

                <td> 
                    <div id='assinatura2' align='center'>
                    	
	                <img src='{3}'></img>
                    <p class='c'>Governo do Estado do Rio de Janeiro</p>
                    <p class='c'>Secretaria de Estado de Educação </p>
                                <br/>
                    	
	                    <p>Coordenadoria Regional de Inspeção Escolar - Regional {13}, verificada a documentação escolar, declaro a regularidade dos estudos realizados, nos termos da legislação em vigor.</p> <br/><br/>
                    	
	                    <p align='center'>_______________________</p>
                    	<br/>
	                    <p align='center'>Nome do Prof. Inspetor Escolar e ID/Vínculo</p>
                    	
                    	
	                   </div>
                </td>
        </tr>
       </table>

</div>
</div>

</page>
</body>
</html>
", NomeUe, corpo, rnUtil.DataporExtenso(DateTime.Now), img1, img2, numero, folha, livro, obs, DateTime.Now.Year, numerocertificado, strtipoDocumentoSelecionado.ToUpper(), strtipoDocumentoSelecionado, regional, strEste, strRegistrado, img3, municipio);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return html;
        }

        private string MontarErroCertidaoEmLote(string corpo, int tipoDocumentoSelecionado)
        {
            string img1 = HttpContext.Current.Server.MapPath("Img/logoRJ.PNG");
            //  string img2 = HttpContext.Current.Server.MapPath("Img/Assinatura.PNG");
            string strtipoDocumentoSelecionado = string.Empty;

            if (tipoDocumentoSelecionado == 4)
                strtipoDocumentoSelecionado = "diploma";
            else if (tipoDocumentoSelecionado == 3)
                strtipoDocumentoSelecionado = "certificado";

            string html = string.Empty;
            try
            {
                html = string.Format(@"
<html>
<head>
      <meta charset='utf-8'></meta>
      <title>Erros Certidão em Lote</title>
	  
    </head>
<body>
<page size='A4'>
<div id='tudo'>
       
	   <div id='cabecalho1' align='center'>
			<img src='{2}'></img>
			
        </div>

        <br/>
                
<div id='erro' style='padding-left:61px'>
    <table>
        <caption>ERROS ENCONTRADOS</caption>
  <thead>
    <tr>
      <th></th>
      <th></th>
      </tr>
  </thead>
<tbody>
<tr> 
<td>ALUNOS</td>
<td>ERROS</td>
</tr>
		{0}
</tbody>
    </table>
 </div>

<br/>
<br/>
    
<br/>    
	
    </div>
    <div id='rodape'>

    <p class='x'>Rio de Janeiro,{1}.</p>
    
    </div>
    <br/>
   

</page>
</body>
</html>


", corpo, rnUtil.DataporExtenso(DateTime.Now), img1);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return html;
        }

        public DataTable ListarDisciplinasCursadas(string aluno, int tipoconclusaoid)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                                     SELECT  
		                                    TC.DESCRICAO +' '+  MDC.DESCRICAO  AS MODALIDADENIVEL,lue.SETOR as UA,
		                                    LC.TITULO AS NOMECERTIFICACAO,
		                                    LM.ALUNO,  
		                                    LT.SERIE,  
		                                    LT.TURMA,   
		                                    LT.TURNO, 
		                                    LT.CURSO,LC.MODALIDADE,LC.TIPO_CURSO,LC.TIPO,
		                                    LT.CURRICULO, 
		                                    LM.ANO,  
		                                    LM.SEMESTRE,  
		                                    LUE.UNIDADE_ENS,  
		                                    LUE.NOME_COMP AS ESCOLA,  
		                                    LUE.ENDERECO,
		                                    MU.NOME AS MUNICIPIO,
		                                    UPPER (TSFA.SITUACAO_FINAL) SITUACAO,
		                                    (ISNULL(LD.HORAS_ATIV, 0) + ISNULL(LD.HORAS_AULA, 0) + ISNULL(LD.HORAS_ESTAGIO, 0) + ISNULL(LD.HORAS_LAB, 0)) AS CARGA_HORARIA,  
		                                    ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) AS DISCIPLINA, LD.NOME_COMPL AS NOME_DISC
                                    FROM	LY_HISTMATRICULA LM (NOLOCK) 
		                                    JOIN	LY_ALUNO LA (NOLOCK) ON LM.ALUNO = LA.ALUNO 
						                                    AND LM.SITUACAO_HIST <> 'CANCELADO' 
						                                    and ISNULL(LM.MAIS_EDUCACAO, 'N') = 'N'
						                                    and ISNULL(LM.EDUC_ESPECIAL, 'N') = 'N'
						                                    and ISNULL( LM.CONCOMITANTE, 'N') = 'N'
						                                    and ISNULL( LM.OPTATIVAREFORCO, 'N') = 'N'
		                                    JOIN	TCE_SITUACAO_FINAL_ALUNO TSFA (NOLOCK) ON TSFA.ALUNO = LM.ALUNO 
						                                    AND TSFA.ANO = LM.ANO
						                                    AND TSFA.PERIODO = LM.SEMESTRE
						                                    AND	TSFA.TURMA = LM.TURMA
						                                    AND ISNULL(LM.DEPENDENCIA, 'N') = 'N'
						                                    and TSFA.situacao_final in ('Aprovado', 'Aprovado Com Dep','Promovido')
		                                    JOIN	LY_TURMA LT (NOLOCK) ON LM.ANO = LT.ANO 
						                                    AND LM.SEMESTRE = LT.SEMESTRE 
						                                    AND LM.TURMA = LT.TURMA 
						                                    AND LM.DISCIPLINA = LT.DISCIPLINA 
		                                    JOIN	LY_UNIDADE_ENSINO LUE (NOLOCK) ON LUE.UNIDADE_ENS = LT.UNIDADE_RESPONSAVEL 
		                                    JOIN    MUNICIPIO MU (NOLOCK) ON LUE.MUNICIPIO = MU.CODIGO
		                                    JOIN	LY_DISCIPLINA LD (NOLOCK) ON LD.DISCIPLINA = ISNULL(LT.DISCIPLINA_MULTIPLA, LT.DISCIPLINA) 
		                                    JOIN	LY_CURSO LC (NOLOCK) ON LC.CURSO = LT.CURSO 
		                                    JOIN	CertificacaoEscolar.TIPOCONCLUSAO_MODALIDADETIPO tm (nolock) on (tm.TIPO = LC.TIPO and tm.MODALIDADE = lc.MODALIDADE )
		                                    LEFT JOIN LY_MODALIDADE_CURSO MDC(NOLOCK) ON  (MDC.MODALIDADE=LC.MODALIDADE AND MDC.MODALIDADE=TM.MODALIDADE)
		                                    LEFT JOIN LY_TIPO_CURSO  TC ON ( TC.TIPO=LC.TIPO) 
                                    WHERE  LM.ALUNO = @ALUNO 
                                           AND TM.TIPOCONCLUSAOID = @TIPOCONCLUSAOID 
                                    ORDER  BY LM.ANO desc, case when LM.SEMESTRE = 0 then 3 else LM.SEMESTRE end desc, LM.SERIE desc 
                                       ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, tipoconclusaoid);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return dt;
        }

        public Boolean EmiteCertificacao(string turno, string curso, string curriculo, string serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            Boolean emite = false;
            DataTable dt = null;
            try
            {
                contextQuery.Command = @"SELECT EMITE_CERTIFICACAO  
                                        FROM LY_SERIE
                                        WHERE 
                                        --EMITE_CERTIFICACAO = 'S' AND
                                        TURNO = @TURNO
                                        AND CURSO = @CURSO
                                        AND CURRICULO = @CURRICULO
                                        AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@CURRICULO", SqlDbType.VarChar, curriculo);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.VarChar, serie);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    emite = dt.Rows[0]["EMITE_CERTIFICACAO"].ToString() == "S" ? true : false;
                }
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                //throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return emite;
        }

        public Boolean PossuiAutorizacao(string aluno, int tipoconclusaoid, int tipoDocumentoSelecionado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            Boolean emite = false;
            DataTable dt = null;
            try
            {
                contextQuery.Command = @"SELECT AUTORIZADO  
                                        FROM [CertificacaoEscolar].DOCUMENTOCERTIFICACAO
                                        WHERE 
                                        TIPOCONCLUSAOID=@TIPOCONCLUSAOID 
                                        AND DOCUMENTOID=@TIPODOCUMENTOID
                                        AND ALUNO=@ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@TIPOCONCLUSAOID", SqlDbType.Int, tipoconclusaoid);
                contextQuery.Parameters.Add("@TIPODOCUMENTOID", SqlDbType.Int, tipoDocumentoSelecionado);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    emite = dt.Rows[0]["AUTORIZADO"].ToString() == "True" ? true : false;
                }
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                //throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return emite;
        }

        public DateTime BuscaDataFim(int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DateTime DT_FIM_AULA = DateTime.MinValue;
            DataTable dt = null;
            try
            {
                contextQuery.Command = @" SELECT DT_FIM_AULA  
                                          FROM LY_PERIODO_LETIVO 
                                          WHERE ANO=@ano AND PERIODO=@periodo ";

                contextQuery.Parameters.Add("@ano", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@periodo", SqlDbType.Int, periodo);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    DT_FIM_AULA = DateTime.Parse(dt.Rows[0]["DT_FIM_AULA"].ToString());
                }
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }

            return DT_FIM_AULA;
        }

        public DataTable ListarCursoUnidade(string unidade_Ens, string curso)
        {
            if (!string.IsNullOrEmpty(unidade_Ens))
            {
                return UnidadeEnsinoCursos.Listar(unidade_Ens, curso);
            }
            return null;
        }

        public string GerarNumeroCertificado(string UA, string unidade_Ens, string anoConclusao, int documentoCertId, int tipoConclusao)
        {
            string nCertificado = null;
            DataTable dtDocumentogerado = null;

            string[] via_;
            int via;

            try
            {
                dtDocumentogerado = rnDocumentoGerado.Listar(documentoCertId);
                //tipo Documento
                //1		Histórico Escolar
                //2		Certidão
                //3		Certificado Escolar
                //4		Diploma

                if (tipoConclusao == 2)// Certificado Escolar, não precisa de nº de Vias
                {
                    nCertificado = UA + unidade_Ens + anoConclusao + "-" + documentoCertId;

                }
                else
                {
                    if (dtDocumentogerado.Rows.Count > 0)
                    {
                        via_ = dtDocumentogerado.Rows[0]["NUMEROGERADO"].ToString().Split('V');
                        via = Convert.ToInt32(via_[1]) + 1;
                    }
                    else
                    {
                        via = 1;
                    }
                    nCertificado = UA + unidade_Ens + anoConclusao + "-" + documentoCertId + "V" + via;
                }
            }
            catch (Exception ex)
            {
                nCertificado = null;
                throw new Exception("Erro ao gerar número do documento.");
            }
            return nCertificado;
        }

        public string GerarCodigoValidador(string unidade_Ens, string curso, string modalidade, string nivel, string tipo_curso, int tipoDocumento, int anoConclusao, int documentoCertId, out int sequencial)
        {
            RN.Certificacao.DocumentoCertificacao rnDocumentoCertificacao = new DocumentoCertificacao();
            sequencial = 0;
            string codigo = null;
            DataTable dtDocumentogerado = null;
            string via = string.Empty;
            int total = 0;

            try
            {
                dtDocumentogerado = rnDocumentoGerado.Listar(documentoCertId);

                if (dtDocumentogerado.Rows.Count > 0)
                {
                    total = (dtDocumentogerado.Rows.Count + 1);

                    if (total <= 9)
                    {
                        via = total.ToString().PadLeft(2, '0');
                    }
                }
                else
                {
                    via = "01";
                }

                var tipoDoc = tipoDocumento == 3 ? "03" : "04";

                string nivelModalidade = string.Empty;

                //ED. PROF          

                // INTEGRADA                 

                if (curso == "0313.32" ||
                    curso == "0602.33" ||
                    curso == "0605.32" ||
                    curso == "0602.32" ||
                    curso == "0906.32" ||
                    curso == "0603.32" ||
                    curso == "0903.32" ||
                    curso == "0604.32" ||
                    curso == "0504.32" ||
                    curso == "0705.32" ||
                    curso == "1204.32" ||
                    curso == "0401.32" ||
                    curso == "0401.33" ||
                    curso == "0002.46" ||
                    curso == "0002.47" ||
                    curso == "0300.32" ||
                    curso == "0300.34" ||
                    curso == "0002.48" ||
                    curso == "2024.31") //CURSOS ENVIADOS PELA AREA
                {
                    nivelModalidade = "EPI";
                }
                //Caso a educação profissional não seja integrada ela será SUBSEQUENTE ou CONCOMITANTE (dependendo da marcacao do aluno na turma)

                // CONCOMITANTE
                if (curso == "0002.89" ||
                    curso == "0002.90" ||
                    curso == "0002.59" ||
                    curso == "0002.50" ||
                    curso == "0002.62" ||
                    curso == "0002.49" ||
                    curso == "0002.88")
                {
                    nivelModalidade = "EPC";
                }
                // SUBSEQUENTE
                if (curso == "0106.33" ||
                    curso == "0100.33" ||
                    curso == "1204.33" ||
                    curso == "0002.39" ||
                    curso == "0500.33" ||
                    curso == "0116.33" ||
                    curso == "0309.33" ||
                    curso == "0313.33" ||
                    curso == "0305.33")
                {
                    nivelModalidade = "EPS";
                }

                if (nivelModalidade.IsNullOrEmptyOrWhiteSpace())
                {
                    //CURSO NORMAL
                    if (modalidade == "NO9")
                    {
                        nivelModalidade = "CN";
                    }
                    //REGULAR
                    else if (modalidade == "RE1" || modalidade == "PR1")
                    {
                        //ENS. FUND REGULAR
                        if (nivel == "1" || nivel == "2")
                        {
                            nivelModalidade = "EFR";
                        }

                        //ENS MEDIO REGULAR
                        if (nivel == "3")
                        {
                            nivelModalidade = "EMR";
                        }
                    }

                    //EJA
                    else if (modalidade == "ED2" || modalidade == "ED7")
                    {
                        //ENS FUNDAMENTAL EJA
                        if (nivel == "1" || nivel == "2" || nivel == "6")
                        {
                            nivelModalidade = "EFE";
                        }

                        //ENS MEDIO EJA
                        if (nivel == "3")
                        {
                            nivelModalidade = "EME";
                        }
                    }
                }

                //Busca sequencial
                sequencial = rnDocumentoCertificacao.ObtemSequencialPor(unidade_Ens, tipoDocumento, documentoCertId);
                string seguencialFormatado = sequencial.ToString().PadLeft(5, '0');

                codigo = tipoDoc + "." + nivelModalidade + ".01." + unidade_Ens + "." + seguencialFormatado + "." + anoConclusao.ToString().Substring(2, 2) + "." + via + "." + DateTime.Now.Year.ToString().Substring(2, 2);
            }
            catch (Exception ex)
            {
                codigo = null;
                throw new Exception("Erro ao gerar número do documento.");
            }
            return codigo;
        }

        public Boolean ValidarCertificado()
        {
            Boolean valido = false;
            try
            {

                System.Security.Cryptography.X509Certificates.X509Certificate2 certificado = null;

                //pede o certificado e senha.
                certificado = RN.Certificacao.AssinaturaDigitalA3.BuscarCertificado();

                if (certificado != null)
                {

                    byte[] byteArray = new byte[16 * 1024];
                    RSACryptoServiceProvider Key = new RSACryptoServiceProvider();
                    // Key = (System.Security.Cryptography.RSACryptoServiceProvider)certificado.PrivateKey;
                    var x = RN.Certificacao.SignWrappers.SignFile(certificado, byteArray);
                    valido = true;
                }
            }
            catch (Exception)
            {

            }

            return valido;
        }
    }
}
