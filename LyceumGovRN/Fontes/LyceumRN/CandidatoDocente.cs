using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Linq;
using Techne.Lyceum.RN.RecursosHumanos.DTO;

namespace Techne.Lyceum.RN
{
    public class CandidatoDocente : RNBase
    {
        public static QueryTable ConsultarProcessoSeletivo(DbObject concurso, DbObject candidato)
        {
            //Usada pela tela CandidatoDocenteFicha e CandidatoDocente
            string sql = "SELECT CDS.DESCRICAO AS STATUS, " +
                         "(SELECT SUM(CTPONT.PONTUACAO) FROM " +
                         "((SELECT PONTUACAO " +
                         "FROM LY_CONCURSO_DOC_EXPERIENCIA CODE " +
                         "INNER JOIN LY_CANDIDATO_DOC_EXPERIENCIAS CADE ON " +
                         "CODE.CONCURSO = CADE.CONCURSO AND CODE.EXPERIENCIA = CADE.EXPERIENCIA " +
                         "WHERE CADE.CANDIDATO = CD.CANDIDATO AND CD.CONCURSO = CADE.CONCURSO) " +
                         "UNION ALL " +
                         "(SELECT PONTUACAO FROM LY_CONCURSO_DOC_TITULACOES CODT " +
                         "INNER JOIN LY_CANDIDATO_DOC_TITULACOES CADT ON " +
                         "CODT.CONCURSO = CADT.CONCURSO AND CODT.TITULACAO = CADT.TITULACAO " +
                         "WHERE CADT.CANDIDATO = CD.CANDIDATO AND CADT.CONCURSO = CD.CONCURSO)) CTPONT) " +
                         "SOMA_PONTUACAO, CD.DT_APRESENTACAO, CD.HORA_APRESENTACAO,CDC.DT_INICIO_CONTRATO,CDC.DT_FIM_CONTRATO " +
                         "FROM LY_CANDIDATO_DOCENTE CD WITH(NOLOCK) " +
                         "INNER JOIN LY_CANDIDATO_DOCENTE_STATUS CDS WITH(NOLOCK) ON CDS.STATUSID = CD.STATUS " +
                         "LEFT JOIN LY_CANDIDATO_DOC_CONTRATO CDC WITH(NOLOCK) ON " +
                         "CD.CANDIDATO = CDC.CANDIDATO AND CD.CONCURSO = CDC.CONCURSO " +
                         "where " +
                         "CD.CONCURSO = ? " +
                         "and CD.CANDIDATO = ?";

            return Consultar(sql, concurso, candidato);
        }

        public static QueryTable ConsultarTitulacao(DbObject concurso)
        {
            //Usada pela tela CandidatoDocenteFicha e CandidatoDocente
            string sql = "Select ct.titulacao, descricao, pontuacao, (DESCRICAO + ' - ' + CONVERT(VARCHAR,PONTUACAO) + ' PONTOS') AS NOME FROM LY_CONCURSO_TITULACAO ct join LY_CONCURSO_DOC_TITULACOES cdt on ct.titulacao = cdt.titulacao  where concurso = ?  ORDER BY PONTUACAO   ";

            return Consultar(sql, concurso.ToString());
        } 
       
        public static QueryTable ConsultarExperiencia(DbObject concurso)
        {
            //Usada pela tela CandidatoDocenteFicha e CandidatoDocente
            string sql = "Select ce.experiencia, descricao, pontuacao, (DESCRICAO + ' - ' + CONVERT(VARCHAR,PONTUACAO) + ' PONTOS') AS NOME FROM LY_CONCURSO_EXPERIENCIA ce join LY_CONCURSO_DOC_EXPERIENCIA cde on ce.experiencia = cde.experiencia where concurso = ? order by pontuacao ";

            return Consultar(sql, concurso.ToString());
        }

        public static QueryTable ConsultarExperienciaSeeduc(DbObject concurso)
        {
            //Usada pela tela CandidatoDocenteFicha e CandidatoDocente
            string sql = "Select ce.experiencia, descricao, pontuacao FROM LY_CONCURSO_EXPERIENCIA ce join LY_CONCURSO_DOC_EXPERIENCIA cde on ce.experiencia = cde.experiencia where concurso = ? and origem = 'S' order by pontuacao desc";

            return Consultar(sql, concurso.ToString());
        }

        public static QueryTable ConsultarExperienciaFora(DbObject concurso)
        {
            //Usada pela tela CandidatoDocenteFicha e CandidatoDocente
            string sql = "Select ce.experiencia, descricao, pontuacao FROM LY_CONCURSO_EXPERIENCIA ce join LY_CONCURSO_DOC_EXPERIENCIA cde on ce.experiencia = cde.experiencia where concurso = ? and origem = 'N' order by pontuacao desc";

            return Consultar(sql, concurso.ToString());
        }

        public static string GeraCandidato()
        {
            QueryTable qt = Consultar("select RIGHT(REPLICATE('0',8) + Convert(varchar, isnull(max(CONVERT(int,candidato)),0)+1),8) candidato  FROM Ly_candidato_docente");

            if (qt.Rows.Count > 0)
                return qt.Rows[0]["candidato"].ToString();
            else
                return "0001";
        }

        public static QueryTable ConsultarSituacaoAvaliacao()
        {
            //Usada pela tela CandidatoDocente
            string sql = "Select situacao, descricao FROM LY_CONCURSO_DOC_SIT_AVALIACAO";

            return Consultar(sql);
        }

        public static QueryTable ConsultarDisciplinas(DbObject concurso, DbObject municipio, string nucleo)
        {
            //Usada pela tela CandidatoDocenteFicha
            string sql = "SELECT DISTINCT gh.AGRUPAMENTO as agrupamento, gh.DESCRICAO as descricao " +
                         "FROM LY_GRUPO_HABILITACAO gh " +
                         "inner join LY_CONCURSO_DOC_HABILITACAO ldh on gh.AGRUPAMENTO = ldh.AGRUPAMENTO " +
                         "where ldh.CONCURSO = ? and ldh.MUNICIPIO_PROC = ? and  ldh.nucleo = ? AND GH.ATIVO = 'S' ";

            return Consultar(sql, concurso.ToString(), municipio, nucleo);
        }

        public static decimal ConsultarPontuacao(DbObject candidato, DbObject concurso)
        {
            string _sqlQuery = @" SELECT  SUM(ctpont.PONTUACAO)
                            FROM    ( (SELECT   pontuacao
                                       FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                JOIN LY_CANDIDATO_DOC_EXPERIENCIAS cade ON code.CONCURSO = cade.CONCURSO
                                                                                          AND code.EXPERIENCIA = cade.EXPERIENCIA
                                       WHERE    cade.CANDIDATO = ?
                                                AND cade.CONCURSO = ?)
                                      UNION ALL
                                      ( SELECT  pontuacao
                                        FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                JOIN LY_CANDIDATO_DOC_TITULACOES cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                                         AND codt.TITULACAO = cadt.TITULACAO
                                        WHERE   cadt.CANDIDATO = ?
                                                AND cadt.CONCURSO = ?
                                      )
                                    ) ctpont ";

            return ExecutarFuncaoDec(_sqlQuery, candidato, concurso, candidato, concurso);
        }

        public static RetValue Excluir(Ly_candidato_docente.Row dadosCandidatoDocente)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                Ly_candidato_docente.Row.Delete(connection, dadosCandidatoDocente.Concurso, dadosCandidatoDocente.Candidato);
                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                retorno = new RetValue(true, "Registro excluído com sucesso.", null);
            }

            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static Ly_candidato_docente.Row Consultar(Ly_candidato_docente.Row dadosCandidatoDocente)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            Ly_candidato_docente.Row candidatoDocente = new Ly_candidato_docente().NewRow();
            try
            {
                candidatoDocente = Ly_candidato_docente.QueryFirstRow(connection, "candidato = ? and concurso = ?", dadosCandidatoDocente.Candidato, dadosCandidatoDocente.Concurso);
            }
            finally
            {
                connection.Close();
            }
            return candidatoDocente;
        }
        
        public static decimal ConsultarPontuacaoTitulacao(string titulacao, string concurso)
        {
            return ExecutarFuncaoDec("select pontuacao from LY_CONCURSO_DOC_TITULACOES where CONCURSO = ? and TITULACAO = ?", concurso, titulacao);
        }
        public static decimal ConsultarPontuacaoExperiencia(string titulacao, string concurso)
        {
            return ExecutarFuncaoDec("select pontuacao from LY_CONCURSO_DOC_EXPERIENCIA where CONCURSO = ? and EXPERIENCIA = ?", concurso, titulacao);
        }
       
        /// <summary>
        /// Verifica se existe Concurso válido
        /// <returns></returns>
        public static bool ExisteConcursoAtivo()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            try
            {
                string sql = @"SELECT 1 from ly_concurso_docente where CONVERT(datetime,CONVERT(varchar(10),DT_INI_INSCR,102),102) <= CONVERT(datetime,CONVERT(varchar(10),getdate(),102),102) and CONVERT(datetime,CONVERT(varchar(10),DT_FIM_INSCR,102),102) >= CONVERT(datetime,CONVERT(varchar(10),getdate(),102),102)";
                qt = new QueryTable(sql);
                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }
            return qt.Rows.Count > 0;
        }

        /// <summary>
        /// Verifica se existe Concurso não expirado
        /// <returns></returns>
        public static bool ExisteConcursoConsulta(string concurso)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            try
            {
                string sql = @"SELECT 1 from ly_concurso_docente where concurso = ? and CONVERT(datetime,CONVERT(varchar(10),DT_INI_CONSULTA,102),102) <= CONVERT(datetime,CONVERT(varchar(10),getdate(),102),102) and CONVERT(datetime,CONVERT(varchar(10),DT_FIM_CONSULTA,102),102) >= CONVERT(datetime,CONVERT(varchar(10),getdate(),102),102)";
                qt = new QueryTable(sql);
                qt.Query(connection, concurso);
            }
            finally
            {
                connection.Close();
            }
            return qt.Rows.Count > 0;
        }

        /// <summary>
        /// Verifica se o candidato é concursado ou não a partir do CPF.
        /// DEFINIÇÃO: Concursado é um docente registrado no sistema que não foi demitido.
        /// </summary>
        /// <param name="cpf">CPF do candidato</param>
        /// <returns></returns>
        public static bool CandidatoFuncionarioConcursado(string cpf)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool ehFuncionarioConcursado = false;

            if (string.IsNullOrEmpty(cpf))
            {
                return ehFuncionarioConcursado;
            }
            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM    LY_VINCULO V
                                            INNER JOIN DBO.LY_PESSOA P ON P.PESSOA = V.PESSOA
                                    WHERE   CPF = @CPF
                                            AND DATA_DESATIVACAO IS NULL
                                            AND NOT EXISTS ( SELECT 1
                                                             FROM   LY_LICENCA_PESSOA LP
                                                                    INNER JOIN dbo.LY_LICENCAS l ON LP.MOTIVO = l.MOTIVO
                                                             WHERE  V.PESSOA = LP.PESSOA
                                                                    AND l.PARTICIPACONTRATOTEMPORARIO = 'S' ) ";

                contextQuery.Parameters.Add("@CPF", cpf);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    ehFuncionarioConcursado = true;
                }

                return ehFuncionarioConcursado;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        /// <summary>
        /// Verifica se o candidato é concursado ou não a partir do CPF.
        /// DEFINIÇÃO: Concursado é um docente registrado no sistema que não foi demitido.
        /// </summary>
        /// <param name="cpf">CPF do candidato</param>
        /// <returns></returns>
        public static bool CandidatoDocenteConcursado(string cpf)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool ehDocenteConcursado = false;

            if (string.IsNullOrEmpty(cpf))
            {
                return ehDocenteConcursado;
            }
            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    LY_DOCENTE D
                                        INNER JOIN LY_PESSOA PE ON PE.PESSOA = D.PESSOA
                                        WHERE   PE.CPF = @CPF
                                                AND D.DT_DEMISSAO IS NULL
                                                AND D.VOLUNTARIO <> 'S'
                                                AND REGIMECONTRATACAOID IN (1,2)
                                                AND NOT EXISTS ( SELECT 1
                                                                 FROM   LY_LICENCA_DOCENTE dr
                                                                        INNER JOIN dbo.LY_LICENCAS l ON dr.MOTIVO = l.MOTIVO
                                                                 WHERE  D.NUM_FUNC = dr.NUM_FUNC
                                                                        AND l.PARTICIPACONTRATOTEMPORARIO = 'S' ) ";

                contextQuery.Parameters.Add("@CPF", cpf);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    ehDocenteConcursado = true;
                }

                return ehDocenteConcursado;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        /// <summary>
        /// Verifica se o candidato é docente em concurso temporário menos um ano da data atual a partir do CPF.
        /// </summary>
        /// <param name="cpf">CPF do candidato</param>
        /// <returns></returns>
        public static bool CandidatoConcursadoTemporario(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return true;
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            try
            {
                string sql = @"SELECT 1 FROM LY_DOCENTE D (NOLOCK) 
                                INNER JOIN LY_PESSOA P (NOLOCK) ON D.PESSOA = P.PESSOA
                                WHERE 1 = 1 
                                AND P.CPF = ? 
                                AND D.REGIMECONTRATACAOID = ?
                                AND D.VOLUNTARIO <> 'S' 
                                AND ( D.DT_DEMISSAO is null OR DATEDIFF(day,D.DT_DEMISSAO,GETDATE()) < 365)";

                qt = new QueryTable(sql);
                qt.Query(connection, cpf, (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario);
            }
            //RETIRADO EM 06/04 THAIS
            //                    CONVERT(datetime,CONVERT(varchar(10),DT_ADMISSAO,102),102) >= CONVERT(datetime,CONVERT(varchar(4),DATEPART(YEAR,getdate())-1,102) + '-'
            //					+ CONVERT(varchar(2),DATEPART(MONTH,getdate()),102) + '-'
            //					+ CONVERT(varchar(2),DATEPART(DAY,getdate()),102),102)";
            finally
            {
                connection.Close();
            }
            if (qt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Verifica se existe CPF inscrito no concurso.
        /// Obs.: Método utilizado na inserção do registro.
        /// </summary>
        /// <param name="concurso">Concurso</param>
        /// <param name="cpf">CPF do candidato</param>
        /// <returns></returns>
        public static bool ExisteCPFConcurso(string concurso, string cpf)
        {
            if (string.IsNullOrEmpty(concurso) || string.IsNullOrEmpty(cpf))
                return false;
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            try
            {
                string sql = @"SELECT 1 FROM ly_candidato_docente 
								WHERE concurso = ? and cpf = ? ";
                qt = new QueryTable(sql);
                qt.Query(connection, concurso, cpf);
            }
            finally
            {
                connection.Close();
            }
            return qt.Rows.Count > 0;
        }

        /// <summary>
        /// Verifica se existe CPF inscrito no concurso.
        /// Obs.: Método utilizado na edição do registro.
        /// </summary>
        /// <param name="concurso">Concurso</param>
        /// <param name="candidato">Identificador do candidato</param>
        /// <param name="cpf">CPF do candidato</param>
        /// <returns></returns>
        public static bool ExisteCPFConcurso(string concurso, string candidato, string cpf)
        {
            if (string.IsNullOrEmpty(concurso) || string.IsNullOrEmpty(candidato) || string.IsNullOrEmpty(cpf))
                return false;
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            try
            {
                string sql = @"SELECT 1 FROM ly_candidato_docente 
								WHERE concurso = ? and candidato <> ? and cpf = ? ";
                qt = new QueryTable(sql);
                qt.Query(connection, concurso, candidato, cpf);
            }
            finally
            {
                connection.Close();
            }
            return qt.Rows.Count > 0;
        }

        /// <summary>
        /// Verifica se o período de inscrição de um Concurso está encerrado ou não a partir de seu identificador.
        /// </summary>
        /// <param name="concurso">Identificador do Concurso</param>
        /// <returns></returns>
        public static bool PeriodoInscricaoEncerrado(string concurso)
        {
            if (string.IsNullOrEmpty(concurso))
                return false;
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            try
            {
                string sql = @"SELECT 1 FROM ly_concurso_docente WHERE concurso = ? 
					and CONVERT(datetime,CONVERT(varchar(10),DT_FIM_INSCR,102),102) < CONVERT(datetime,CONVERT(varchar(10),getdate(),102),102)";
                qt = new QueryTable(sql);
                qt.Query(connection, concurso);
            }
            finally
            {
                connection.Close();
            }
            return qt.Rows.Count > 0;
        }

        public static string ConsultaCandidato(string usuario)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            string candidato;
            try
            {
                candidato = Convert.ToString(TCommand.ExecuteScalar(connection, "SELECT candidato + '|' + concurso as candcon  FROM ly_candidato_docente WHERE usulogin = ?", usuario));
            }
            finally
            {
                connection.Close();
            }
            return candidato;
        }

        public static RetValue Finalizar(string concurso, string candidato)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();
            var sql = @"UPDATE  dbo.LY_CANDIDATO_DOCENTE
                        SET     FINALIZADO = 'S'
                        WHERE   CONCURSO = ?
                                AND CANDIDATO = ?";

            try
            {
                connection.Open(true);

                TCommand.ExecuteNonQuery(
                                        connection,
                                        sql,
                                        concurso,
                                        candidato);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();

                    return retorno;
                }

                retorno = new RetValue(true, "Candidatura finalizada com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();

                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public bool EhMatriculaDocente(string concurso, string candidato)
        {
            bool existeMatriculaDocente;

            ContextQuery contextQuery = new ContextQuery(
                     @"SELECT  COUNT(*)
                        FROM    LY_DOCENTE
                        WHERE   CONCURSO = @CONCURSO
                                AND CANDIDATO = @CANDIDATO
                                 ");

            contextQuery.Parameters.Add("@CONCURSO", concurso);
            contextQuery.Parameters.Add("@CANDIDATO", candidato);

            existeMatriculaDocente = (ExecutarFuncao<int>(contextQuery) > 0);

            return existeMatriculaDocente;
        }

        public LyCandidatoDocente ObtemCandidoDocentePor(string concurso, string candidato)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            LyCandidatoDocente candidatoDocente = new LyCandidatoDocente();

            try
            {
                candidatoDocente = this.ObtemCandidoDocentePor(contexto, concurso, candidato);
                return candidatoDocente;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        private LyCandidatoDocente ObtemCandidoDocentePor(DataContext contexto, string concurso, string candidato)
        {
            LyCandidatoDocente candidatoDocente = new LyCandidatoDocente();
            ContextQuery contextQuery = new ContextQuery();


            contextQuery.Command = @" SELECT *
                            FROM   LY_CANDIDATO_DOCENTE (NOLOCK) 
                            WHERE  CONCURSO = @CONCURSO 
                                   AND CANDIDATO = @CANDIDATO ";

            contextQuery.Parameters.Add("@CONCURSO", TechneDbType.T_CODIGO, concurso);
            contextQuery.Parameters.Add("@CANDIDATO", TechneDbType.T_CODIGO, candidato);

            candidatoDocente = contexto.TryToBindEntity<LyCandidatoDocente>(contextQuery);

            return candidatoDocente;
        }

        public static LyCandidatoDocente ListarDadosCandidatoDocente(string strConcurso, string strCandidato)
        {
            LyCandidatoDocente entidade = new LyCandidatoDocente();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"SELECT CONCURSO,CANDIDATO,NOME,DT_NASC,SEXO,ISNULL(NEC.DESCRICAO,'Nenhum')AS NECESSIDADE_ESPECIAL,LY_CANDIDATO_DOCENTE.NECESSIDADEESPECIALID
                                                ,NOME_MAE,NOME_PAI,ESTADO_CIVIL,PAIS_NASC,NACIONALIDADE,MUNICIPIO_NASC,MUNICIPIO_PROC
                                                ,END_PAIS,CEP,END_MUNICIPIO,ENDERECO,END_NUM,END_COMPL,BAIRRO,FONE,CELULAR,E_MAIL,RG_TIPO
                                                ,RG_NUM,RG_EMISSOR,RG_UF,RG_DTEXP,CPF,PIS_PASEP,CPROF_NUM,CPROF_SERIE,CPROF_UF,CPROF_DTEXP
                                                ,NUCLEO,STATUS,GI_Seplag,ETNIAID,COTAIDINSCRICAO,COTAIDCONVOCACAO,IDFUNCIONAL,REGIONALID
												,AGRUPAMENTO_INGRESSO
										FROM LY_CANDIDATO_DOCENTE (NOLOCK) 
                                        LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=LY_CANDIDATO_DOCENTE.NECESSIDADEESPECIALID
                                        WHERE CONCURSO = @CONCURSO AND CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    entidade.Concurso = reader["CONCURSO"].ToString();
                    entidade.Candidato = reader["CANDIDATO"].ToString();
                    entidade.Nome = reader["NOME"].ToString();

                    if (reader["DT_NASC"] != DBNull.Value)
                        entidade.Dt_nasc = Convert.ToDateTime(reader["DT_NASC"]);

                    entidade.Sexo = reader["SEXO"].ToString();
                    if (reader["NECESSIDADEESPECIALID"] != DBNull.Value)
                    {
                        entidade.NecessidadeEspecialId = Convert.ToInt32(reader["NECESSIDADEESPECIALID"]);
                    }
                    entidade.Nome_mae = reader["NOME_MAE"].ToString();
                    entidade.Nome_pai = reader["NOME_PAI"].ToString();
                    entidade.Estado_civil = reader["ESTADO_CIVIL"].ToString();
                    entidade.Pais_nasc = reader["PAIS_NASC"].ToString();
                    entidade.Nacionalidade = reader["NACIONALIDADE"].ToString();
                    entidade.Municipio_nasc = reader["MUNICIPIO_NASC"].ToString();
                    entidade.End_pais = reader["END_PAIS"].ToString();
                    entidade.Cep = reader["CEP"].ToString();
                    entidade.End_municipio = reader["END_MUNICIPIO"].ToString();
                    entidade.Endereco = reader["ENDERECO"].ToString();
                    entidade.End_num = reader["END_NUM"].ToString();
                    entidade.End_compl = reader["END_COMPL"].ToString();
                    entidade.Bairro = reader["BAIRRO"].ToString();
                    entidade.Fone = reader["FONE"].ToString();
                    entidade.Celular = reader["CELULAR"].ToString();
                    entidade.E_mail = reader["E_MAIL"].ToString();
                    entidade.Rg_tipo = reader["RG_TIPO"].ToString();
                    entidade.Rg_num = reader["RG_NUM"].ToString();
                    entidade.Rg_emissor = reader["RG_EMISSOR"].ToString();
                    entidade.Rg_uf = reader["RG_UF"].ToString();

                    if (reader["RG_DTEXP"] != DBNull.Value)
                        entidade.Rg_dtexp = Convert.ToDateTime(reader["RG_DTEXP"]);

                    entidade.Cpf = reader["CPF"].ToString();
                    entidade.Pis_pasep = reader["PIS_PASEP"].ToString();
                    entidade.Cprof_num = reader["CPROF_NUM"].ToString();
                    entidade.Cprof_serie = reader["CPROF_SERIE"].ToString();
                    entidade.Cprof_uf = reader["CPROF_UF"].ToString();

                    if (reader["CPROF_DTEXP"] != DBNull.Value)
                        entidade.Cprof_dtexp = Convert.ToDateTime(reader["CPROF_DTEXP"]);

                    entidade.Nucleo = reader["NUCLEO"].ToString();
                    entidade.Status = Convert.ToInt32(reader["STATUS"]);
                    entidade.Municipio_proc = reader["MUNICIPIO_PROC"].ToString();
                    entidade.GI_Seplag = reader["GI_Seplag"].ToString();
                    entidade.EtniaId = reader["ETNIAID"] != DBNull.Value ? Convert.ToInt32(reader["ETNIAID"]) : 0;
                    entidade.CotaIdInscricao = reader["COTAIDINSCRICAO"] != DBNull.Value ? Convert.ToInt32(reader["COTAIDINSCRICAO"]) : 0;
                    entidade.CotaIdConvocacao = reader["COTAIDCONVOCACAO"] != DBNull.Value ? Convert.ToInt32(reader["COTAIDCONVOCACAO"]) : 0;
                    entidade.IdFuncional = reader["IDFUNCIONAL"] != DBNull.Value ? Convert.ToInt32(reader["IDFUNCIONAL"]) : 0;
                    entidade.RegionalId = reader["REGIONALID"] != DBNull.Value ? Convert.ToInt32(reader["REGIONALID"]) : 0;
                    entidade.Agrupamento_ingresso = reader["AGRUPAMENTO_INGRESSO"] != DBNull.Value ? reader["AGRUPAMENTO_INGRESSO"].ToString() : string.Empty;
                }

                return entidade;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public static bool PossuiStatusAguardandoOuAguardandoAvaliacaoCGP(string processoSeletivo, string inscricaoDocente)
        {
            DataContext contexto = null;
            bool possui = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                possui = PossuiStatusAguardandoOuAguardandoAvaliacaoCGP(contexto, processoSeletivo, inscricaoDocente);
                return possui;
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
        }

        public static bool PossuiStatusAguardandoOuAguardandoAvaliacaoCGP(DataContext ctx, string processoSeletivo, string inscricaoDocente)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                //Status: 1- "Aguardando" / 23 - "Aguardando avaliação CGP"
                contextQuery.Command = @" SELECT COUNT(1)
                                        FROM LY_CANDIDATO_DOCENTE
                                        WHERE CONCURSO = @CONCURSO
                                            AND CANDIDATO = @CANDIDATO
                                            AND STATUS IN (1, 23) ";

                contextQuery.Parameters.Add("@CONCURSO", processoSeletivo);
                contextQuery.Parameters.Add("@CANDIDATO", inscricaoDocente);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public static RetValue ValidaFichaDeInscricao(string processoSeletivo, string inscricaoDocente)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();
            var sql = @"UPDATE LY_CANDIDATO_DOCENTE 
                           SET STATUS = 1,
                               DATAVALIDACAOINSCRICAO = GETDATE()
                         WHERE CONCURSO = ? 
                           AND CANDIDATO = ?";
            try
            {
                connection.Open(true);

                TCommand.ExecuteNonQuery(connection, sql, processoSeletivo, inscricaoDocente);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                retorno = new RetValue(true, "Validação efetuada.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static int RetornaStatusCandidato(string strConcurso, string strCandidato)
        {
            return (ExecutarFuncao("SELECT STATUS FROM LY_CANDIDATO_DOCENTE WITH (NOLOCK) where CONCURSO = ? and CANDIDATO = ?", strConcurso, strCandidato));
        }

        public static DataTable ListarStatusCandidatoDocentePor(int enumStatus)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dtStatus = null;

            try
            {
                if (enumStatus.Equals(Convert.ToInt32(ProcessoSeletivo.Status.Faltoso)) ||
                    enumStatus.Equals(Convert.ToInt32(ProcessoSeletivo.Status.AguardandoCGP)) ||
                    enumStatus.Equals(Convert.ToInt32(ProcessoSeletivo.Status.Aguardando)) ||
                    enumStatus.Equals(Convert.ToInt32(ProcessoSeletivo.Status.Inabilitado)) ||
                    enumStatus.Equals(Convert.ToInt32(ProcessoSeletivo.Status.Desistente))) 
                {
                    contextQuery.Command = @" SELECT CDS.STATUSID, CDS.DESCRICAO
										FROM LY_CANDIDATO_DOCENTE_STATUS CDS
										WHERE CDS.STATUSID IN (5,21,22,1)
										ORDER BY CDS.DESCRICAO ";
                }
                else if (enumStatus.Equals(Convert.ToInt32(ProcessoSeletivo.Status.Convocado))) 
                {
                    contextQuery.Command = @" SELECT CDS.STATUSID, CDS.DESCRICAO
											FROM LY_CANDIDATO_DOCENTE_STATUS CDS
											WHERE CDS.STATUSID IN (1,2,5,21,22)
											ORDER BY CDS.DESCRICAO ";
                }

                dtStatus = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dtStatus;
        }

        public void AtualizaCandidatoDocente(DataContext ctx, LyCandidatoDocente candidato)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_CANDIDATO_DOCENTE 
                                        SET    NOME = @NOME, 
                                               DT_NASC = @DT_NASC, 
                                               SEXO = @SEXO, 
                                               ETNIAID = @ETNIAID, 
                                               NOME_MAE = @NOME_MAE, 
                                               NOME_PAI = @NOME_PAI, 
                                               NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID, 
                                               ESTADO_CIVIL = @ESTADO_CIVIL, 
                                               PAIS_NASC = @PAIS_NASC, 
                                               NACIONALIDADE = @NACIONALIDADE, 
                                               MUNICIPIO_NASC = @MUNICIPIO_NASC, 
                                               END_PAIS = @END_PAIS, 
                                               CEP = @CEP, 
                                               END_MUNICIPIO = @END_MUNICIPIO, 
                                               ENDERECO = @ENDERECO, 
                                               END_NUM = @END_NUM, 
                                               END_COMPL = @END_COMPL, 
                                               BAIRRO = @BAIRRO, 
                                               FONE = @FONE, 
                                               CELULAR = @CELULAR, 
                                               E_MAIL = @E_MAIL, 
                                               CPF = @CPF, 
                                               RG_TIPO = @RG_TIPO, 
                                               RG_NUM = @RG_NUM, 
                                               RG_UF = @RG_UF, 
                                               RG_EMISSOR = @RG_EMISSOR, 
                                               RG_DTEXP = @RG_DTEXP, 
                                               PIS_PASEP = @PIS_PASEP, 
                                               CPROF_NUM = @CPROF_NUM, 
                                               CPROF_SERIE = @CPROF_SERIE, 
                                               CPROF_DTEXP = @CPROF_DTEXP, 
                                               CPROF_UF = @CPROF_UF, 
                                               CATEGORIA = @CATEGORIA, 
                                               DT_PROPOSTA = @DT_PROPOSTA,
                                               CARGA_HORARIA= @CARGA_HORARIA,
											   IDFUNCIONAL = @IDFUNCIONAL
										WHERE CANDIDATO = @CANDIDATO 
                                               AND CONCURSO = @CONCURSO ";

                contextQuery.Parameters.Add("@NOME", candidato.Nome);
                contextQuery.Parameters.Add("@DT_NASC", candidato.Dt_nasc);
                contextQuery.Parameters.Add("@SEXO", candidato.Sexo);
                contextQuery.Parameters.Add("@ETNIAID", candidato.EtniaId);
                contextQuery.Parameters.Add("@NOME_MAE", candidato.Nome_mae);
                contextQuery.Parameters.Add("@NOME_PAI", candidato.Nome_pai);
                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", candidato.NecessidadeEspecialId);
                contextQuery.Parameters.Add("@ESTADO_CIVIL", candidato.Estado_civil);
                contextQuery.Parameters.Add("@PAIS_NASC", candidato.Pais_nasc);
                contextQuery.Parameters.Add("@NACIONALIDADE", candidato.Nacionalidade);
                contextQuery.Parameters.Add("@MUNICIPIO_NASC", candidato.Municipio_nasc);
                contextQuery.Parameters.Add("@END_PAIS", candidato.End_pais);
                contextQuery.Parameters.Add("@CEP", candidato.Cep);
                contextQuery.Parameters.Add("@END_MUNICIPIO", candidato.End_municipio);
                contextQuery.Parameters.Add("@ENDERECO", candidato.Endereco);
                contextQuery.Parameters.Add("@END_NUM", candidato.End_num);
                contextQuery.Parameters.Add("@END_COMPL ", candidato.End_compl);
                contextQuery.Parameters.Add("@BAIRRO", candidato.Bairro);
                contextQuery.Parameters.Add("@FONE", candidato.Fone);
                contextQuery.Parameters.Add("@CELULAR", candidato.Celular);
                contextQuery.Parameters.Add("@E_MAIL", candidato.E_mail);
                contextQuery.Parameters.Add("@CPF", candidato.Cpf);
                contextQuery.Parameters.Add("@RG_TIPO", candidato.Rg_tipo);
                contextQuery.Parameters.Add("@RG_NUM", candidato.Rg_num);
                contextQuery.Parameters.Add("@RG_UF", candidato.Rg_uf);
                contextQuery.Parameters.Add("@RG_EMISSOR", candidato.Rg_emissor);
                contextQuery.Parameters.Add("@RG_DTEXP", candidato.Rg_dtexp);
                contextQuery.Parameters.Add("@PIS_PASEP", candidato.Pis_pasep);
                contextQuery.Parameters.Add("@CPROF_NUM", candidato.Cprof_num);
                contextQuery.Parameters.Add("@CPROF_SERIE", candidato.Cprof_serie);
                contextQuery.Parameters.Add("@CPROF_DTEXP", candidato.Cprof_dtexp);
                contextQuery.Parameters.Add("@CPROF_UF", candidato.Cprof_uf);
                contextQuery.Parameters.Add("@CATEGORIA", candidato.Categoria);
                contextQuery.Parameters.Add("@DT_PROPOSTA", candidato.Dt_proposta);
                contextQuery.Parameters.Add("@CARGA_HORARIA", candidato.Carga_Horaria);
                contextQuery.Parameters.Add("@IDFUNCIONAL", candidato.IdFuncional);
                contextQuery.Parameters.Add("@CANDIDATO", candidato.Candidato);
                contextQuery.Parameters.Add("@CONCURSO", candidato.Concurso);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public void AtualizaCandidatoDocenteProposta(DataContext ctx, LyCandidatoDocente candidato)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_CANDIDATO_DOCENTE 
                                        SET   PIS_PASEP = @PIS_PASEP,
                                              CATEGORIA = @CATEGORIA,
                                              STATUS = @STATUS,
                                              DT_PROPOSTA = @DT_PROPOSTA,
                                              CARGA_HORARIA= @CARGA_HORARIA,
											  IDFUNCIONAL = @IDFUNCIONAL,
                                              CPROF_NUM = @CPROF_NUM, 
                                              CPROF_SERIE = @CPROF_SERIE, 
                                              CPROF_DTEXP = @CPROF_DTEXP, 
                                              CPROF_UF = @CPROF_UF                                               
										WHERE CANDIDATO = @CANDIDATO 
                                               AND CONCURSO = @CONCURSO ";

                contextQuery.Parameters.Add("@CANDIDATO", candidato.Candidato);
                contextQuery.Parameters.Add("@CONCURSO", candidato.Concurso);
                contextQuery.Parameters.Add("@PIS_PASEP", candidato.Pis_pasep);
                contextQuery.Parameters.Add("@CATEGORIA", candidato.Categoria);
                contextQuery.Parameters.Add("@STATUS", candidato.Status);
                contextQuery.Parameters.Add("@DT_PROPOSTA", candidato.Dt_proposta);
                contextQuery.Parameters.Add("@CARGA_HORARIA", candidato.Carga_Horaria);
                contextQuery.Parameters.Add("@IDFUNCIONAL", candidato.IdFuncional);
                contextQuery.Parameters.Add("@CPROF_NUM", candidato.Cprof_num);
                contextQuery.Parameters.Add("@CPROF_SERIE", candidato.Cprof_serie);
                contextQuery.Parameters.Add("@CPROF_DTEXP", candidato.Cprof_dtexp);
                contextQuery.Parameters.Add("@CPROF_UF", candidato.Cprof_uf);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public void AtualizaCandidatoDocenteContrato(DataContext ctx, string strConcurso, string strCandidato, DateTime dtDataUltimoExercicio, string status)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_CANDIDATO_DOC_CONTRATO
										SET DT_FIM_CONTRATO = @DT_FIM_CONTRATO, STATUS = @STATUS
										WHERE CANDIDATO = @CANDIDATO AND CONCURSO = @CONCURSO";

                contextQuery.Parameters.Add("@DT_FIM_CONTRATO", dtDataUltimoExercicio);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);
                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@STATUS", status);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public void AtualizaCandidatoDocenteStatus(DataContext ctx, string strObservacaoStatus, string strCandidato, string strConcurso, int status)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_CANDIDATO_DOCENTE
										SET STATUS = @STATUS, 
                                            STATUS_OBS = @STATUS_OBS 
										WHERE CANDIDATO = @CANDIDATO
										AND CONCURSO = @CONCURSO";

                contextQuery.Parameters.Add("@STATUS_OBS", strObservacaoStatus);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);
                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@STATUS", status);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public void IncluirCandidatoDocenteSolicitacoes(DataContext ctx, string strConcurso, string strCandidato, DateTime dtDataUltimoExercicio)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO LY_CANDIDATO_DOC_SOLICITACOES
										(CONCURSO,CANDIDATO,DATA,STATUS,TIPO)
										VALUES 
										(@CONCURSO,@CANDIDATO,@DATA,'Aprovado','Solicitação de Rescisão de Contrato Temporário Aprovada') ";

                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);
                contextQuery.Parameters.Add("@DATA", dtDataUltimoExercicio);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public void IncluirCandidatoDocenteSolicitacoes(DataContext ctx, string strCandidato, string strConcurso, string strCargaHoraria, string strCargaNova, string strTipo)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"INSERT INTO LY_CANDIDATO_DOC_SOLICITACOES
										(CANDIDATO,CONCURSO,CARGA_HORARIA_ATUAL,CARGA_HORARIA_NOVA,DATA,TIPO,STATUS)
										VALUES 
										(@CANDIDATO, @CONCURSO, @CARGA_HORARIA_ATUAL, @CARGA_HORARIA_NOVA, GETDATE(),@TIPO,'Aprovado') ";

                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);
                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@CARGA_HORARIA_ATUAL", strCargaHoraria);
                contextQuery.Parameters.Add("@CARGA_HORARIA_NOVA", strCargaNova);
                contextQuery.Parameters.Add("@TIPO", strTipo);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public void AtualizaCargaHoraria(DataContext ctx, string strConcurso, string strCandidato, string strCargaHoraria)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_CANDIDATO_DOCENTE
										SET CARGA_HORARIA = @CARGA_HORARIA
										WHERE CONCURSO = @CONCURSO AND CANDIDATO = @CANDIDATO";

                contextQuery.Parameters.Add("@CARGA_HORARIA", strCargaHoraria);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);
                contextQuery.Parameters.Add("@CONCURSO", strConcurso);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }        

        public DataTable ObtemDadosCandidato(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CANDIDATO, 
                                               CONCURSO, 
                                               ISNULL((SELECT TOP 1 1 
                                                       FROM   LY_CANDIDATO_DOC_TITULACOES DTIT 
                                                       WHERE  DTIT.CANDIDATO = CD.CANDIDATO 
                                                              AND DTIT.CONCURSO = CD.CONCURSO), 0) AS POSSUI_TITULACAO, 
                                               ISNULL((SELECT TOP 1 1 
                                                       FROM   LY_CANDIDATO_DOC_EXPERIENCIAS DEXP 
                                                       WHERE  DEXP.CANDIDATO = CD.CANDIDATO 
                                                              AND DEXP.CONCURSO = CD.CONCURSO), 0) AS POSSUI_EXPERIENCIA 
                                               , 
                                               ISNULL((SELECT TOP 1 1 
                                                       FROM 
                                               [LYCEUM].[CONTRATOTEMPORARIO].[CANDIDATODOCENTE_GRUPOHABILITACAO] 
                                               HAB 
                                                       WHERE  HAB.CANDIDATO = CD.CANDIDATO 
                                                              AND HAB.CONCURSO = CD.CONCURSO), 0)  AS POSSUI_HABILITACAO 
                                        FROM   LY_CANDIDATO_DOCENTE CD WITH(NOLOCK) 
                                        WHERE  CD.CONCURSO = @CONCURSO
                                               AND CD.CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public bool PossuiCandidatoInscrito(string cpf, string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(1) FROM ly_candidato_docente 
								WHERE CPF = @CPF and CONCURSO = @CONCURSO "
                };

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CPF", cpf);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public int Insere(LyCandidatoDocente dadosCandidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret = int.MinValue;

            try
            {
                contextQuery.Command = @"INSERT INTO LYCEUM.dbo.LY_CANDIDATO_DOCENTE 
                                                               (CONCURSO
                                                               ,CANDIDATO
                                                               ,NOME
                                                               ,DT_NASC
                                                               ,SEXO
                                                               ,NECESSIDADEESPECIALID
                                                               ,NOME_MAE
                                                               ,NOME_PAI
                                                               ,ESTADO_CIVIL
                                                               ,PAIS_NASC
                                                               ,NACIONALIDADE
                                                               ,MUNICIPIO_NASC
															   ,MUNICIPIO_PROC
                                                               ,END_PAIS
                                                               ,CEP
                                                               ,END_MUNICIPIO
                                                               ,ENDERECO
                                                               ,END_NUM
                                                               ,END_COMPL
                                                               ,BAIRRO
                                                               ,FONE
                                                               ,CELULAR
                                                               ,E_MAIL
                                                               ,RG_TIPO
                                                               ,RG_NUM
                                                               ,RG_EMISSOR
                                                               ,RG_UF
                                                               ,RG_DTEXP
                                                               ,CPF
                                                               ,PIS_PASEP
                                                               ,CPROF_NUM
                                                               ,CPROF_SERIE
                                                               ,CPROF_UF
                                                               ,CPROF_DTEXP
                                                               ,REGIONALID
                                                               ,STATUS
                                                               ,ETNIAID
                                                               ,COTAIDINSCRICAO)
                                                         VALUES
                                                               (@CONCURSO
                                                               ,@CANDIDATO
                                                               ,@NOME
                                                               ,@DT_NASC 
                                                               ,@SEXO
                                                               ,@NECESSIDADEESPECIALID
                                                               ,@NOME_MAE
                                                               ,@NOME_PAI
                                                               ,@ESTADO_CIVIL
                                                               ,@PAIS_NASC
                                                               ,@NACIONALIDADE
                                                               ,@MUNICIPIO_NASC
															   ,@MUNICIPIO_PROC
                                                               ,@END_PAIS
                                                               ,@CEP
                                                               ,@END_MUNICIPIO
                                                               ,@ENDERECO
                                                               ,@END_NUM
                                                               ,@END_COMPL
                                                               ,@BAIRRO
                                                               ,@FONE
                                                               ,@CELULAR
                                                               ,@E_MAIL
                                                               ,@RG_TIPO
                                                               ,@RG_NUM
                                                               ,@RG_EMISSOR
                                                               ,@RG_UF
                                                               ,@RG_DTEXP
                                                               ,@CPF
                                                               ,@PIS_PASEP
                                                               ,@CPROF_NUM
                                                               ,@CPROF_SERIE
                                                               ,@CPROF_UF
                                                               ,@CPROF_DTEXP
                                                               ,@REGIONALID
                                                               ,@STATUS
                                                               ,@ETNIAID
                                                               ,@COTAIDINSCRICAO) ";

                contextQuery.Parameters.Add("@CONCURSO", dadosCandidato.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", dadosCandidato.Candidato);
                contextQuery.Parameters.Add("@NOME", dadosCandidato.Nome);
                contextQuery.Parameters.Add("@DT_NASC", Convert.ToDateTime(dadosCandidato.Dt_nasc));
                contextQuery.Parameters.Add("@SEXO", dadosCandidato.Sexo);
                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", dadosCandidato.NecessidadeEspecialId);
                contextQuery.Parameters.Add("@NOME_MAE", dadosCandidato.Nome_mae);
                contextQuery.Parameters.Add("@NOME_PAI", dadosCandidato.Nome_pai);
                contextQuery.Parameters.Add("@ESTADO_CIVIL", dadosCandidato.Estado_civil);
                contextQuery.Parameters.Add("@PAIS_NASC", dadosCandidato.Pais_nasc);
                contextQuery.Parameters.Add("@NACIONALIDADE", dadosCandidato.Nacionalidade);
                contextQuery.Parameters.Add("@MUNICIPIO_NASC", dadosCandidato.Municipio_nasc);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", dadosCandidato.Municipio_proc);
                contextQuery.Parameters.Add("@END_PAIS", dadosCandidato.End_pais);
                contextQuery.Parameters.Add("@CEP", dadosCandidato.Cep);
                contextQuery.Parameters.Add("@END_MUNICIPIO", dadosCandidato.End_municipio);
                contextQuery.Parameters.Add("@ENDERECO", dadosCandidato.Endereco);
                contextQuery.Parameters.Add("@END_NUM", dadosCandidato.End_num);
                contextQuery.Parameters.Add("@END_COMPL", dadosCandidato.End_compl);
                contextQuery.Parameters.Add("@BAIRRO", dadosCandidato.Bairro);
                contextQuery.Parameters.Add("@FONE", dadosCandidato.Fone);
                contextQuery.Parameters.Add("@CELULAR", dadosCandidato.Celular);
                contextQuery.Parameters.Add("@E_MAIL", dadosCandidato.E_mail);
                contextQuery.Parameters.Add("@RG_TIPO", dadosCandidato.Rg_tipo);
                contextQuery.Parameters.Add("@RG_NUM", dadosCandidato.Rg_num);
                contextQuery.Parameters.Add("@RG_EMISSOR", dadosCandidato.Rg_emissor);

                contextQuery.Parameters.Add("@CPF", dadosCandidato.Cpf);
                contextQuery.Parameters.Add("@PIS_PASEP", dadosCandidato.Pis_pasep);

                if (dadosCandidato.Rg_uf == null || dadosCandidato.Rg_uf.ToString() == string.Empty)
                {
                    contextQuery.Parameters.Add("@RG_UF", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@RG_UF", dadosCandidato.Rg_uf);
                }

                if (dadosCandidato.Rg_dtexp == null || dadosCandidato.Rg_dtexp.ToString() == string.Empty)
                {
                    contextQuery.Parameters.Add("@RG_DTEXP", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@RG_DTEXP", dadosCandidato.Rg_dtexp);
                }

                if (dadosCandidato.Cprof_num == null || dadosCandidato.Cprof_num.ToString() == string.Empty)
                {
                    contextQuery.Parameters.Add("@CPROF_NUM", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@CPROF_NUM", dadosCandidato.Cprof_num);
                }

                if (dadosCandidato.Cprof_serie == null || dadosCandidato.Cprof_serie.ToString() == string.Empty)
                {
                    contextQuery.Parameters.Add("@CPROF_SERIE", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@CPROF_SERIE", dadosCandidato.Cprof_serie);
                }

                if (dadosCandidato.Cprof_uf == null || dadosCandidato.Cprof_uf.ToString() == string.Empty)
                {
                    contextQuery.Parameters.Add("@CPROF_UF", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@CPROF_UF", dadosCandidato.Cprof_uf);
                }

                if (dadosCandidato.Cprof_dtexp == null || dadosCandidato.Cprof_dtexp.ToString() == string.Empty)
                {
                    contextQuery.Parameters.Add("@CPROF_DTEXP", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@CPROF_DTEXP", dadosCandidato.Cprof_dtexp);
                }

                //contextQuery.Parameters.Add("@CATEGORIA", dadosCandidato.Categoria);
                contextQuery.Parameters.Add("@REGIONALID", dadosCandidato.RegionalId);
                contextQuery.Parameters.Add("@STATUS", Convert.ToInt32(dadosCandidato.Status));
                contextQuery.Parameters.Add("@ETNIAID", dadosCandidato.EtniaId);
                contextQuery.Parameters.Add("@COTAIDINSCRICAO", dadosCandidato.CotaIdInscricao);

                ret = ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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

            return ret;
        }

        public int AlterarCandidatoDocente(LyCandidatoDocente dadosCandidato, string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret = int.MinValue;

            try
            {
                contextQuery.Command = @"UPDATE LYCEUM.dbo.LY_CANDIDATO_DOCENTE 
																SET CONCURSO = @CONCURSO,
                                                                CANDIDATO = @CANDIDATO,
																NOME = @NOME,
																DT_NASC = @DT_NASC,
																SEXO = @SEXO,
                                                                NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID,
                                                                NOME_MAE = @NOME_MAE,
                                                                NOME_PAI = @NOME_PAI,
                                                                ESTADO_CIVIL = @ESTADO_CIVIL, 
                                                                PAIS_NASC = @PAIS_NASC,
                                                                NACIONALIDADE = @NACIONALIDADE,
                                                                MUNICIPIO_NASC = @MUNICIPIO_NASC,
																MUNICIPIO_PROC = @MUNICIPIO_PROC,
                                                                END_PAIS = @END_PAIS,
                                                                CEP = @CEP, 
                                                                END_MUNICIPIO = @END_MUNICIPIO,
                                                                ENDERECO = @ENDERECO,
                                                                END_NUM = @END_NUM,
                                                                END_COMPL = @END_COMPL,
                                                                BAIRRO = @BAIRRO,
                                                                FONE = @FONE,
                                                                CELULAR = @CELULAR,
                                                                E_MAIL = @E_MAIL,
                                                                RG_TIPO = @RG_TIPO,
                                                                RG_NUM = @RG_NUM,
                                                                RG_EMISSOR = @RG_EMISSOR,
                                                                RG_UF = @RG_UF,
                                                                RG_DTEXP =@RG_DTEXP,
                                                                CPF = @CPF,
                                                                PIS_PASEP = @PIS_PASEP,
                                                                CPROF_NUM = @CPROF_NUM,
                                                                CPROF_SERIE = @CPROF_SERIE,
                                                                CPROF_UF = @CPROF_UF,
                                                                CPROF_DTEXP = @CPROF_DTEXP,
                                                                REGIONALID = @REGIONALID,
                                                                ETNIAID = @ETNIAID,
                                                                COTAIDINSCRICAO = @COTAIDINSCRICAO
															    WHERE   
															    CONCURSO = @CONCURSO
																AND CANDIDATO = @CANDIDATO";

                contextQuery.Parameters.Add("@CONCURSO", dadosCandidato.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", dadosCandidato.Candidato);
                contextQuery.Parameters.Add("@NOME", dadosCandidato.Nome);
                contextQuery.Parameters.Add("@DT_NASC", dadosCandidato.Dt_nasc);
                contextQuery.Parameters.Add("@SEXO", dadosCandidato.Sexo);
                contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", dadosCandidato.NecessidadeEspecialId);
                contextQuery.Parameters.Add("@NOME_MAE", dadosCandidato.Nome_mae);
                contextQuery.Parameters.Add("@NOME_PAI", dadosCandidato.Nome_pai);
                contextQuery.Parameters.Add("@ESTADO_CIVIL", dadosCandidato.Estado_civil);
                contextQuery.Parameters.Add("@PAIS_NASC", dadosCandidato.Pais_nasc);
                contextQuery.Parameters.Add("@NACIONALIDADE", dadosCandidato.Nacionalidade);
                contextQuery.Parameters.Add("@MUNICIPIO_NASC", dadosCandidato.Municipio_nasc);
                contextQuery.Parameters.Add("@MUNICIPIO_PROC", dadosCandidato.Municipio_proc);
                contextQuery.Parameters.Add("@END_PAIS", dadosCandidato.End_pais);
                contextQuery.Parameters.Add("@CEP", dadosCandidato.Cep);
                contextQuery.Parameters.Add("@END_MUNICIPIO", dadosCandidato.End_municipio);
                contextQuery.Parameters.Add("@ENDERECO", dadosCandidato.Endereco);
                contextQuery.Parameters.Add("@END_NUM", dadosCandidato.End_num);
                contextQuery.Parameters.Add("@END_COMPL", dadosCandidato.End_compl);
                contextQuery.Parameters.Add("@BAIRRO", dadosCandidato.Bairro);
                contextQuery.Parameters.Add("@FONE", dadosCandidato.Fone);
                contextQuery.Parameters.Add("@CELULAR", dadosCandidato.Celular);
                contextQuery.Parameters.Add("@E_MAIL", dadosCandidato.E_mail);
                contextQuery.Parameters.Add("@RG_TIPO", dadosCandidato.Rg_tipo);
                contextQuery.Parameters.Add("@RG_NUM", dadosCandidato.Rg_num);
                contextQuery.Parameters.Add("@RG_EMISSOR", dadosCandidato.Rg_emissor);
                contextQuery.Parameters.Add("@CPF", dadosCandidato.Cpf);
                contextQuery.Parameters.Add("@PIS_PASEP", dadosCandidato.Pis_pasep);
                contextQuery.Parameters.Add("@CPROF_NUM", dadosCandidato.Cprof_num);
                contextQuery.Parameters.Add("@CPROF_SERIE", dadosCandidato.Cprof_serie);
                contextQuery.Parameters.Add("@REGIONALID", dadosCandidato.RegionalId);
                contextQuery.Parameters.Add("@ETNIAID", dadosCandidato.EtniaId);
                contextQuery.Parameters.Add("@COTAIDINSCRICAO", dadosCandidato.CotaIdInscricao);

                if (dadosCandidato.Cprof_uf == null || dadosCandidato.Cprof_uf.ToString() == string.Empty)
                { contextQuery.Parameters.Add("@CPROF_UF", DBNull.Value); }
                else
                { contextQuery.Parameters.Add("@CPROF_UF", dadosCandidato.Cprof_uf); }

                if (dadosCandidato.Cprof_dtexp == null || dadosCandidato.Cprof_dtexp.ToString() == string.Empty)
                { contextQuery.Parameters.Add("@CPROF_DTEXP", DBNull.Value); }
                else
                { contextQuery.Parameters.Add("@CPROF_DTEXP", dadosCandidato.Cprof_dtexp); }

                if (dadosCandidato.Rg_dtexp == null || dadosCandidato.Rg_dtexp.ToString() == string.Empty)
                { contextQuery.Parameters.Add("@RG_DTEXP", DBNull.Value); }
                else
                {
                    contextQuery.Parameters.Add("@RG_DTEXP", dadosCandidato.Rg_dtexp);
                }

                if (dadosCandidato.Rg_uf == null || dadosCandidato.Rg_uf.ToString() == string.Empty)
                { contextQuery.Parameters.Add("@RG_UF", DBNull.Value); }
                else
                {
                    contextQuery.Parameters.Add("@RG_UF", dadosCandidato.Rg_uf);
                }

                ret = ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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

            return ret;
        }

        public DataTable ConsultarCandidatoDocente(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT *
                                       ,ISNULL(NEC.DESCRICAO,'Nenhum')AS NECESSIDADE_ESPECIAL
                                       
                                       FROM LYCEUM.dbo.LY_CANDIDATO_DOCENTE (NOLOCK)
                                       LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=LY_CANDIDATO_DOCENTE.NECESSIDADEESPECIALID
									   WHERE CONCURSO = @CONCURSO AND CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

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

        public bool EhContratadoCandidatoDocente(string concurso, string candidato)
        {
            bool retorno = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"SELECT TOP 1 D.MATRICULA 
                                                                FROM LYCEUM.dbo.LY_LOTACAO L
                                                                    INNER JOIN LYCEUM.dbo.LY_DOCENTE D ON
                                                                        D.PESSOA = L.PESSOA
                                                                        AND D.MATRICULA = L.MATRICULA
                                                                    INNER JOIN LYCEUM.dbo.LY_CANDIDATO_DOCENTE CD ON
                                                                        CD.CONCURSO = D.CONCURSO AND
                                                                        CD.CANDIDATO = D.CANDIDATO
                                                                WHERE
                                                                    CD.CONCURSO = @CONCURSO AND
                                                                    CD.CANDIDATO = @CANDIDATO";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = true;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public string ConsultarCandidatoPorCPFeNasc(string concurso, string cpf, DateTime dtNasc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @"SELECT CD.CANDIDATO
                                                                              FROM LY_CANDIDATO_DOCENTE CD WITH (NOLOCK)
                                                                              WHERE CD.CONCURSO = @CONCURSO AND 
                                                                                  CD.CPF = @CPF AND 
                                                                                  CONVERT(DATE,CD.DT_NASC) = CONVERT(DATE,@DT_NASC) ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CPF", cpf);
                contextQuery.Parameters.Add("@DT_NASC", dtNasc);

                resultado = ctx.GetReturnValue<string>(contextQuery);

                return resultado;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
        }

        public DataTable ObtemHabilitacaoCandidatoPor(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT CONCURSO, CANDIDATO,CD.AGRUPAMENTO,DESCRICAO,HABILITADO
                                        FROM   CONTRATOTEMPORARIO.CANDIDATODOCENTE_GRUPOHABILITACAO CD WITH(NOLOCK) 
                                        INNER JOIN LY_GRUPO_HABILITACAO GH ON GH.AGRUPAMENTO = CD.AGRUPAMENTO
                                        WHERE  CD.CONCURSO = @CONCURSO
                                               AND CD.CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public bool PodeInscricaoIndigena(string cpf)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(1) FROM LY_DOCENTE D (NOLOCK) INNER JOIN LY_PESSOA P  (NOLOCK) ON D.PESSOA = P.PESSOA
                                WHERE CPF = @CPF
                                AND REGIMECONTRATACAOID = @REGIMECONTRATACAOID
                                AND VOLUNTARIO <> 'S' 
                                AND ( DT_DEMISSAO is null OR DATEDIFF(day,DT_DEMISSAO,GETDATE()) < 30) "
                };

                contextQuery.Parameters.Add("@REGIMECONTRATACAOID", (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario);
                contextQuery.Parameters.Add("@CPF", cpf);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public DataTable ObtemDadosDocenteCandidatoPor(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT CD.CANDIDATO, 
                                               CD.CONCURSO, 
                                               D.MATRICULA,
                                               PE.IDFUNCIONAL,
                                               D.VINCULO
                                        FROM   LY_CANDIDATO_DOCENTE CD WITH(NOLOCK)
                                        LEFT JOIN LY_DOCENTE D ON CD.CANDIDATO=D.CANDIDATO AND CD.CONCURSO = D.CONCURSO
                                        LEFT JOIN LY_PESSOA PE ON PE.PESSOA = D.PESSOA
                                        WHERE  CD.CONCURSO = @CONCURSO
                                               AND CD.CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return dt;
        }

        public ValidacaoDados ValidaProposta(LyCandidatoDocente candidato, LyPessoa pessoa, LyDocente docente, RecursosHumanos.Entidades.Acumulacao acumulacao, LyLotacao lotacao, RecursosHumanos.Entidades.PessoaDadosBancarios dadosBancarios, LyGrupoHabilitacaoDoc grupoHabilitacaoDoc)
        {
            List<string> mensagens = new List<string>();
            RN.Docentes rnDocente = new Docentes();
            RN.VinculoLy rnVinculo = new VinculoLy();
            RN.Pessoa rnPessoa = new Pessoa();
            DataContext contexto = null;
            DateTime milnov = new DateTime(1899, 12, 31);
            LyCandidatoDocente candidatoBase = new LyCandidatoDocente();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            if (candidato == null || pessoa == null || lotacao == null || dadosBancarios == null || grupoHabilitacaoDoc == null)
            {
                return validacaoDados;
            }

            if (pessoa.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUARIOID é obrigatório.");
            }

            if (candidato.Concurso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CONCURSO é obrigatório.");
            }

            if (candidato.Candidato.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CANDIDATO é obrigatório.");
            }

            if (!string.IsNullOrEmpty(pessoa.Cep))
            {
                pessoa.Cep = pessoa.Cep.RetirarCaracteres();
                if (pessoa.Cep.Length < 8)
                {
                    mensagens.Add("CEP inválido. <br>O CEP deve ter 8 números.");
                }
            }
            if (!string.IsNullOrEmpty(pessoa.Nome_compl))
            {
                if (pessoa.Nome_compl.Length < 5)
                {
                    mensagens.Add("Nome deve conter pelo menos cinco letras.");
                }
                if (!Validacao.Validou(pessoa.Nome_compl, Validacao.Tipo.nome))
                {
                    mensagens.Add("Nome inválido.<br>O nome deve ter apenas letras.");
                }
            }

            if (string.IsNullOrEmpty(pessoa.Cpf))
            {
                mensagens.Add("O campo CPF é obrigatório.");
            }
            else
            {
                pessoa.Cpf = pessoa.Cpf.RetirarMascaraCPF();
                if (!Validacao.ValidaCpf(pessoa.Cpf))
                {
                    mensagens.Add("O CPF informado é inválido.");
                }
            }

            if (pessoa.Dt_nasc == null || pessoa.Dt_nasc == DateTime.MinValue)
            {
                mensagens.Add("Data de nascimento inválida.<br>Preenchimento obrigatório.");
            }
            else
            {
                if (!Validacao.ValidouData(pessoa.Dt_nasc, Validacao.Tipo.data))
                {
                    mensagens.Add("Data de nascimento inválida.<br>A data de nascimento deve ser maior que 1900 e não pode ser maior que a data de hoje.");
                }

                if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Rg_dtexp))
                {
                    mensagens.Add("Data expedição documento/nascimento inválidas.<br>A data de expedição do documento de indentificação deve ser maior que a data de nascimento.");
                }

                if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Cprof_dtexp))
                {
                    mensagens.Add("Data expedição carteira profissional/nascimento inválidas.<br>A data de expedição da carteira profissional deve ser maior que a data de nascimento.");
                }

                if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.CertNascEmissao))
                {
                    mensagens.Add("Data de emissão da certidão/nascimento inválidas.<br>A data de emissão da certidão de nascimento deve ser maior que a data de nascimento.");
                }

                if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Teleitor_dtexp))
                {
                    mensagens.Add("Data de emissão do título de eleitor/nascimento inválidas.<br>A data de emissão do título de eleitor deve ser maior que a data de nascimento.");
                }

                if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Alist_dtexp))
                {
                    mensagens.Add("Data de emissão do alistamento militar/nascimento inválidas.<br>A data de emissão do alistamento militar deve ser maior que a data de nascimento.");
                }

                if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Cr_dtexp))
                {
                    mensagens.Add("Data de emissão do certificado de reservista/nascimento inválidas.<br>A data de emissão do certificado de reservista deve ser maior que a data de nascimento.");
                }
            }

            if (string.IsNullOrEmpty(Convert.ToString(pessoa.Municipio_nasc)))
            {
                mensagens.Add("Município de nascimento inválido.<br>Preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(pessoa.Nacionalidade.ToString()))
            {
                mensagens.Add("Nacionalidade inválida.<br>Preenchimento obrigatório.");
            }

            if (!string.IsNullOrEmpty(pessoa.E_mail) && !Validacao.Validou(pessoa.E_mail, Validacao.Tipo.email))
            {
                mensagens.Add("Email externo inválido.<br>O e-mail está em um formato incorreto.");
            }

            if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace()
              && !(pessoa.E_mail_interno.Split('@')[1].Trim() == "prof.educacao.rj.gov.br"
              || pessoa.E_mail_interno.Split('@')[1].Trim() == "educacao.rj.gov.br"))
            {
                mensagens.Add("No campo E-MAIL INTERNO serão aceitos apenas e-mails institucionais @educacao.rj.gov.br ou @prof.educacao.rj.gov.br");
            }

            if (!string.IsNullOrEmpty(pessoa.Celular))
            {
                pessoa.Celular = pessoa.Celular.RetirarMascaraTelefone();
                bool celularOk = RN.Validacao.ValidaCelularComDDD(pessoa.Celular);
                if (celularOk != true)
                {
                    mensagens.Add("Celular inválido.");
                }
            }
            if (!string.IsNullOrEmpty(pessoa.Fone))
            {
                pessoa.Fone = pessoa.Fone.RetirarMascaraTelefone();
                bool telefoneOk = RN.Validacao.ValidaTelefoneComDDD(pessoa.Fone);
                if (telefoneOk != true)
                {
                    mensagens.Add("Telefone inválido.");
                }
            }

            if (!string.IsNullOrEmpty(pessoa.Rg_num))
            {
                pessoa.Rg_num = pessoa.Rg_num.RetirarMascaraRG();
                if (pessoa.Rg_num.Length < 5 || pessoa.Rg_num.Length > 15)
                {
                    mensagens.Add("O número do documento deve conter no mínimo cinco dígitos e no máximo quinze caracteres.");
                }
            }

            if (!string.IsNullOrEmpty(pessoa.Bairro))
            {
                bool bairroOk = RN.Validacao.ValidaBairro(pessoa.Bairro);
                if (bairroOk != true)
                {
                    mensagens.Add("Bairro inválido.");
                }
            }            

            if (pessoa.IdFuncional == null || pessoa.IdFuncional <= 0)
            {
                mensagens.Add("Identidade Funcional é campo obrigatório.");
            }

            if (docente.Vinculo <= 0)
            {
                mensagens.Add("Vínculo é campo obrigatório.");
            }
           
            if (!pessoa.Pispasep.IsNullOrEmptyOrWhiteSpace())
            {
                long resultado;
                if (!long.TryParse(pessoa.Pispasep, out resultado))
                {
                    mensagens.Add("O PIS/PASEP deve ser composto apenas por numeros.");
                }
            }

            if (pessoa.Cprof_num.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Número da carteira profissional: Preenchimento obrigatório.");
            }
            else
            {
                if (!Validacao.Validou(pessoa.Cprof_num, Validacao.Tipo.numerico))
                {
                    mensagens.Add("Número da carteira profissional inválido.<br>O número da carteira profissional deve ter somente números.");
                }
            }

            if (pessoa.Cprof_serie.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Série da carteira profissional: Preenchimento obrigatório.");
            }

            if (pessoa.Cprof_dtexp == null || pessoa.Cprof_dtexp == DateTime.MinValue)
            {
                mensagens.Add("Data de expedição da carteira profissional: Preenchimento obrigatório.");
            }
            else if (pessoa.Cprof_dtexp > DateTime.Now.Date)
            {
                mensagens.Add("Data de expedição da carteira profissional: Deve ser uma data menor ou igual a data de hoje.");
            }

            if (pessoa.Cprof_uf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Estado emissor da carteira profissional: Preenchimento obrigatório.");
            }

            if (lotacao.DataNomeacao != null)
            {
                if (lotacao.DataNomeacao < milnov)
                    mensagens.Add("Data da Nomeação não pode ser menor que 1900.");
            }

            //Verificar campos da tabela LY_GRUPO_HABILITACAO_DOC
            if (grupoHabilitacaoDoc.Agrupamento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo DISCIPLINA é de preenchimento obrigatório.");
            }

            if (grupoHabilitacaoDoc.Provisorio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo PROVISÓRIO é de preenchimento obrigatório.");
            }

            if (grupoHabilitacaoDoc.AgrupamentoIngresso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo AGRUPAMENTO INGRESSO é de preenchimento obrigatório.");
            }

            //Valida campos da acumulacao
            if (docente.Acumulacao < 0)
            {
                mensagens.Add("O campo ACUMULAÇÃO é de preenchimento obrigatório.");
            }
            else if (docente.Acumulacao == (int)RN.Docentes.PossuiAcumulacao.Sim)
            {
                if (acumulacao == null)
                {
                    mensagens.Add("Os dados da ACUMULAÇÃO são de preenchimento obrigatório.");
                }
                else
                {
                    if (acumulacao.Orgao.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo ÓRGÃO é de preenchimento obrigatório.");
                    }
                    else if (acumulacao.Orgao.Length > 120)
                    {
                        mensagens.Add("O campo ÓRGÃO deve ter no máximo 120 caracteres.");
                    }

                    if (acumulacao.MatriculaOrgao.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo MATRICULA ÓRGÃO é de preenchimento obrigatório.");
                    }
                    else
                    {
                        if (acumulacao.MatriculaOrgao.Length > 20)
                        {
                            mensagens.Add("O campo MATRICULA ÓRGÃO deve ter no máximo 20 caracteres.");
                        }

                        decimal matriculaOrgao;
                        if (!decimal.TryParse(acumulacao.MatriculaOrgao, out matriculaOrgao))
                        {
                            mensagens.Add("O campo MATRICULA ÓRGÃO deve conter apenas números.");
                        }
                    }

                    if (acumulacao.NumeroProcesso.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo NÚMERO DO PROCESSO é de preenchimento obrigatório.");
                    }
                    else if (acumulacao.NumeroProcesso.Length > 25)
                    {
                        mensagens.Add("O campo NÚMERO DO PROCESSO deve ter no máximo 25 caracteres.");
                    }
                }
            }

            if (lotacao.UnidadeEns.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Unidade de Ensino é campo obrigatório.");
            }

            if (docente.Regime_trabalho.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Carga horária é campo obrigatório.");
            }

            if (dadosBancarios.Banco <= 0)
            {
                mensagens.Add("Banco é campo obrigatório.");
            }

            if (dadosBancarios.Agencia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Agência é campo obrigatório.");
            }

            if (dadosBancarios.ContaBanco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Conta é campo obrigatório.");
            }

            if (docente.Dt_admissao == null || docente.Dt_admissao == DateTime.MinValue)
            {
                mensagens.Add("Data de Admissão é campo obrigatório.");
            }
            else
            {
                if (docente.Dt_admissao > DateTime.Now)
                {
                    mensagens.Add("Data de admissão: a data de admissão deve ser uma data menor ou igual à data atual.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    //Verifica se o docente não possui matricula
                    if (docente.Matricula.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Para docentes sem matricula, utilizar id/Vinculo
                        docente.Matricula = string.Format("{0}/{1}", pessoa.IdFuncional, docente.Vinculo);
                        lotacao.Matricula = docente.Matricula;
                    }


                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida CPF existente
                    var cpf = pessoa.Cpf.RetirarMascaraCPF();

                    //Caso não existe pessoa Buscar
                    if (pessoa.Pessoa == 0)
                    {
                        //Busca pessoa com mesmo  cpf
                        decimal pessoaCPF = rnPessoa.ObtemPessoaPor(contexto, cpf);
                        if (pessoaCPF > 0)
                        {
                            pessoa.Pessoa = pessoaCPF;
                        }
                    }

                    if (rnPessoa.PossuiOutroCPFPor(contexto, pessoa.Cpf, pessoa.Pessoa))
                    {
                        mensagens.Add("CPF já existente para outra pessoa.");
                    }

                    //Valida matricula
                    if (rnDocente.PossuiOutraMatriculaPor(contexto, docente.Matricula, pessoa.Pessoa))
                    {
                        mensagens.Add("Número de matrícula já cadastrado para outro docente.");
                    }

                    if (rnVinculo.PossuiOutraMatriculaPor(contexto, docente.Matricula, pessoa.Pessoa))
                    {
                        mensagens.Add("Número de matrícula já cadastrado para outro servidor.");
                    }

                    if (pessoa.IdFuncional != null && pessoa.IdFuncional > 0)
                    {
                        //Valida ID FUNCIONAL existente                      
                        if (rnPessoa.PossuiOutroIdFuncionalPor(contexto, Convert.ToInt32(pessoa.IdFuncional), pessoa.Pessoa))
                        {
                            mensagens.Add("Já existe uma pessoa cadastrada com esse número de ID FUNCIONAL.");
                        }
                    }
                    if (docente.Vinculo != null && docente.Vinculo > 0)
                    {
                        //Valida se o vinculo já foi utilizado para esta pessoa
                        if (rnDocente.PossuiOutroVinculoPor(contexto, pessoa.Pessoa, Convert.ToInt32(docente.Vinculo), docente.Matricula) ||
                            rnVinculo.PossuiOutroVinculoPor(contexto, pessoa.Pessoa, Convert.ToInt32(docente.Vinculo), docente.Matricula))
                        {
                            mensagens.Add("Este VINCULO já se encontra cadastrado para esta pessoa.");
                        }
                    }

                    //Busca dados atuais do candidato na base
                    candidatoBase = this.ObtemCandidoDocentePor(contexto, candidato.Concurso, candidato.Candidato);
                    if (Convert.ToDateTime(docente.Dt_admissao).Date < Convert.ToDateTime(candidatoBase.DataApresentacao).Date)
                    {
                        mensagens.Add(string.Format("Data de admissão: a data de admissão deve ser uma data maior ou igual à data de apresentação {0}", Convert.ToDateTime(candidatoBase.DataApresentacao).ToString("dd/MM/yyyy")));
                    }

                    if (!RN.ExtraClasse.ValidaLotacao(connection, docente.Matricula, lotacao.UnidadeEns, lotacao.Funcao, Convert.ToDateTime(lotacao.DataNomeacao).Year, lotacao.DataNomeacao))
                    {
                        mensagens.Add("Não existe carência dessa função na Unidade Escolar.");
                    }
                    RetValue retorno = VerificarErro(connection.GetErrors());
                    #region Verifica Erro
                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                        {
                            connection.Rollback();
                            mensagens.Add("Erro na Validação de Lotacao.");
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    connection.Rollback();
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                    connection.Close();
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void GeraProposta(LyCandidatoDocente candidato, LyPessoa pessoa, LyDocente docente, RecursosHumanos.Entidades.Acumulacao acumulacao, LyLotacao lotacao, RecursosHumanos.Entidades.PessoaDadosBancarios dadosBancarios, LyGrupoHabilitacaoDoc grupoHabilitacaoDoc)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool cadastroPessoa = (pessoa.Pessoa == 0);
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Docentes rnDocente = new Docentes();
            RN.Lotacao rnLotacao = new Lotacao();
            RN.RecursosHumanos.Acumulacao rnAcumulacao = new Techne.Lyceum.RN.RecursosHumanos.Acumulacao();
            RN.RecursosHumanos.PessoaDadosBancarios rnPessoaDadosBancarios = new Techne.Lyceum.RN.RecursosHumanos.PessoaDadosBancarios();
            RN.RecursosHumanos.Entidades.PessoaDadosBancarios dadosBancariosBase = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PessoaDadosBancarios();
            RN.CandidatoDocContrato rnCandidatoDocContrato = new Techne.Lyceum.RN.CandidatoDocContrato();
            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new GrupoHabilitacaoDoc();
            LyCandidatoDocContrato candidatoDocContrato = new LyCandidatoDocContrato();

            try
            {
                if (cadastroPessoa)
                {
                    rnPessoa.Insere(contexto, pessoa);
                }
                else
                {
                    rnPessoa.AtualizaPessoaCandidato(contexto, pessoa);
                }

                docente.Pessoa = pessoa.Pessoa;
                docente.Usuario = pessoa.UsuarioId;

                rnDocente.Insere(contexto, docente);

                if (dadosBancarios != null && !dadosBancarios.ContaBanco.IsNullOrEmptyOrWhiteSpace())
                {
                    dadosBancarios.PessoaId = pessoa.Pessoa;
                    dadosBancarios.UsuarioId = pessoa.UsuarioId;

                    if (cadastroPessoa)
                    {
                        rnPessoaDadosBancarios.Insere(contexto, dadosBancarios);
                    }
                    else
                    {
                        //Verifica se a pessoa já possui dados bancarios cadastrados
                        dadosBancariosBase = rnPessoaDadosBancarios.ObtemAtivoPor(contexto, pessoa.Pessoa);
                        if (dadosBancariosBase == null || dadosBancariosBase.PessoaDadosBancariosId <= 0)
                        {
                            //Caso não possua inser
                            rnPessoaDadosBancarios.Insere(contexto, dadosBancarios);
                        }
                        else if (dadosBancariosBase.Agencia != dadosBancarios.Agencia
                            || dadosBancariosBase.Banco != dadosBancarios.Banco
                            || dadosBancariosBase.ContaBanco != dadosBancarios.ContaBanco)
                        {
                            //Caso possua e não sejam os mesmos, desativa o anterior e insere o novo
                            rnPessoaDadosBancarios.Desativa(contexto, pessoa.Pessoa, pessoa.UsuarioId);
                            rnPessoaDadosBancarios.Insere(contexto, dadosBancarios);
                        }
                    }
                }

                if (lotacao != null && !lotacao.UnidadeEns.IsNullOrEmptyOrWhiteSpace())
                {
                    lotacao.Pessoa = pessoa.Pessoa;
                    lotacao.Usuario = pessoa.UsuarioId;
                    lotacao.DataDesativacao = null;
                    lotacao.DataNomeacaoDo = null;
                    lotacao.DataDesativacaoDo = null;
                    lotacao.DataDesativacao = null;
                    lotacao.AtoOficial = null;
                    lotacao.RespDocumentacao = null;
                    lotacao.DtInicioReadaptacao = null;
                    lotacao.DtFimReadaptacao = null;
                    lotacao.Readaptado = "N";

                    rnLotacao.Insere(contexto, lotacao);
                }

                this.AtualizaCandidatoDocenteProposta(contexto, candidato);

                if (docente.Acumulacao == (int)RN.Docentes.PossuiAcumulacao.Sim)
                {
                    acumulacao.DocenteId = docente.Num_func;
                    acumulacao.UsuarioId = pessoa.UsuarioId;
                    rnAcumulacao.Salva(contexto, acumulacao);
                }

                candidatoDocContrato.Candidato = candidato.Candidato;
                candidatoDocContrato.Concurso = candidato.Concurso;
                candidatoDocContrato.Status = "Admitido";
                candidatoDocContrato.DtInicioContrato = Convert.ToDateTime(candidato.Dt_proposta);
                candidatoDocContrato.DtFimContrato = null;

                if (rnCandidatoDocContrato.PossuiCandidatoDocContratoPor(candidatoDocContrato.Candidato, candidatoDocContrato.Concurso))
                {
                    rnCandidatoDocContrato.AtualizaDadosProposta(contexto, candidatoDocContrato);
                }
                else
                {
                    rnCandidatoDocContrato.Insere(contexto, candidatoDocContrato);
                }

                if (grupoHabilitacaoDoc != null && !grupoHabilitacaoDoc.Agrupamento.IsNullOrEmptyOrWhiteSpace())
                {
                    grupoHabilitacaoDoc.NumFunc = docente.Num_func;
                    rnGrupoHabilitacaoDoc.Insere(contexto, grupoHabilitacaoDoc);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        public void AtualizaSituacaoCandidato(string candidato, string concurso, int situacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.AtualizaCandidatoDocenteStatus(contexto, null, candidato, concurso, situacao);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        public static QueryTable obtemHabilitacaoProcessoPor(DbObject concurso, DbObject municipio, DbObject regional)
        {
            //Usada pela tela CandidatoDocenteFicha
            string sql = "SELECT DISTINCT gh.AGRUPAMENTO as agrupamento, gh.DESCRICAO as descricao " +
                         "FROM LY_GRUPO_HABILITACAO gh " +
                         "inner join LY_CONCURSO_DOC_HABILITACAO ldh on gh.AGRUPAMENTO = ldh.AGRUPAMENTO " +
                         "where ldh.CONCURSO = ? and ldh.MUNICIPIO_PROC = ? and  ldh.REGIONALID = ? AND GH.ATIVO = 'S' ";

            return Consultar(sql, concurso.ToString(), municipio, regional);
        }
    }
}