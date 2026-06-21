using System;
using System.Data;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class Docentes : RNBase
    {
        public enum PossuiAcumulacao
        {
            [StringValue("Não")]
            Nao = 0,
            [StringValue("Sim")]
            Sim = 1,
            [StringValue("Não Informado")]
            NaoInformado = 2
        }

        public DadosVoluntario ObtemDadosVoluntariolPor(string cpf)
        {
            DadosVoluntario voluntario = new RN.DTOs.DadosVoluntario();
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.Aluno rnAluno = new Aluno();
            RN.Pessoa rnPessoa = new Pessoa();

            try
            {
                voluntario = rnPessoa.ObtemDadosVoluntarioPor(ctx, cpf);

                if (voluntario.PessoaId > 0)
                {
                    //Verifica se a pessoa é um aluno ativo
                    if (rnAluno.EhAlunoAtivoPor(ctx, voluntario.PessoaId))
                    {
                        voluntario.Bloqueado = true;
                    }
                }

                return voluntario;
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

        private decimal GeraDocentePor(DataContext ctx)
        {
            //gera código de docente a partir do último cadastrado no banco.
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = 0;

            contextQuery.Command = @" SELECT ISNULL(MAX(NUM_FUNC),0) + 1  AS ORDEM
                                      FROM LY_DOCENTE (NOLOCK) ";

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                retorno = Convert.ToInt32(reader["ORDEM"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return retorno;
        }

        public bool PossuiCategoriaPor(DataContext contexto, string categoria)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM LY_DOCENTE
                                    WHERE CATEGORIA = @CATEGORIA ";

            contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, categoria);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutraMatriculaPor(DataContext ctx, string matricula, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_DOCENTE 
                                        WHERE MATRICULA = @MATRICULA
	                                        AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public static string ObterMatricula(TConnection connection, decimal num_func)
        {
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, "select matricula from ly_docente where num_func = ?", num_func);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }

        public string ObtemMatriculaPor(decimal num_func)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT MATRICULA
                                            FROM LY_DOCENTE (NOLOCK)
                                            WHERE NUM_FUNC = @NUM_FUNC ";

                contextQuery.Parameters.Add("@NUM_FUNC", num_func);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public string ObtemIdVinculoMatriculaPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT IDVINCULO_MATRICULA
                                            FROM VW_FUNCIONARIOS
                                            WHERE MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public string ObtemMatriculaPorIdVinculo(string IdVinculo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT MATRICULA
                                            FROM VW_FUNCIONARIOS
                                            WHERE IDVINCULO_MATRICULA = @IDVINCULO ";

                contextQuery.Parameters.Add("@IDVINCULO", IdVinculo);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public decimal ObtemPessoaPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            decimal retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT PESSOA
                                            FROM LY_DOCENTE (NOLOCK)
                                            WHERE MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["PESSOA"]);
                }

                return retorno;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public bool AlteradoDocenteOnlinePor(decimal pessoa, out  DateTime dataAlteracao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            dataAlteracao = DateTime.MinValue;

            try
            {
                contextQuery.Command = @" SELECT P.DATAALTERACAO 
                                    FROM   LY_PESSOA P (NOLOCK) 
                                           INNER JOIN LY_DOCENTE D (NOLOCK) 
                                                   ON P.PESSOA = D.PESSOA 
                                    WHERE  P.USUARIOID = D.MATRICULA 
                                           AND P.PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = true;
                    dataAlteracao = reader["DATAALTERACAO"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DATAALTERACAO"]);
                }

                return retorno;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public string ObtemMatriculaPor(DataContext contexto, int idFuncional, string censo, DateTime dataConsulta)
        {
            string matricula = string.Empty;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT ISNULL(L.MATRICULA, D.MATRICULA) AS MATRICULA
                                            FROM LY_PESSOA P
                                            INNER JOIN LY_DOCENTE D ON P.PESSOA = D.PESSOA
                                            LEFT JOIN LY_LOTACAO L ON P.PESSOA = L.PESSOA
			                                            and l.DATA_NOMEACAO <= CONVERT(DATE, @DATACONSULTA) 
			                                            AND (L.DATA_DESATIVACAO IS NULL OR CONVERT(DATE,L.DATA_DESATIVACAO) > CONVERT(DATE, @DATACONSULTA)) 
			                                            and l.UNIDADE_ENS = @CENSO
                                            where IDFUNCIONAL = @IDFUNCIONAL
                                            order by d.DT_ADMISSAO ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATACONSULTA", dataConsulta);
                contextQuery.Parameters.Add("@IDFUNCIONAL", idFuncional);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    matricula = Convert.ToString(reader["MATRICULA"]);
                }

                return matricula;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int ObtemNumFuncPor(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int numFunc = 0;

            try
            {
                numFunc = this.ObtemNumFuncPor(ctx, matricula);
                return numFunc;
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

        public int ObtemNumFuncPor(DataContext contexto, string matricula)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT NUM_FUNC 
                                        FROM LY_DOCENTE (NOLOCK) 
                                     WHERE MATRICULA = @MATRICULA  ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["NUM_FUNC"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public string ObtemNomeDocentePorNumFunc(decimal numFunc)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT P.NOME_COMPL
                                          FROM LY_DOCENTE D (NOLOCK) 
	                                            INNER JOIN LY_PESSOA P (NOLOCK) 
			                                            ON D.PESSOA = P.PESSOA
                                            WHERE D.NUM_FUNC = @NUM_FUNC ";

                contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMFUNC, numFunc);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public static QueryTable ConsultarMatriculaDocente(TConnection connection, decimal num_func)
        {
            string sql = @"
                DECLARE @pessoa T_CODIGO
                SELECT @pessoa = pessoa FROM ly_docente (NOLOCK) WHERE num_func = ?

                SELECT  d.num_func, d.matricula, d.pessoa, l.ordem, f.funcao, d.categoria, d.regime_trabalho
                FROM    ly_docente d (NOLOCK) INNER JOIN
                        ly_lotacao l (NOLOCK) ON 
                            l.matricula = d.matricula AND
                            l.data_nomeacao <= CONVERT(DATE,GETDATE()) AND 
                            (l.data_desativacao IS NULL OR CONVERT(DATE,l.data_desativacao) > CONVERT(DATE,GETDATE())) INNER JOIN
                        ly_funcao f (NOLOCK) ON 
                            f.funcao = l.funcao --AND
                           -- f.campo_01 = 'S'
                WHERE d.pessoa = @pessoa";
            return RNBase.Consultar(connection, sql, num_func);
        }

        public static QueryTable ConsultarDadosDocente(string docente)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = " select d.NUM_FUNC, d.MATRICULA, p.CPF as cpf, " +
                         " case	 " +
                         "    when (select distinct 1 as 'aula'  " +
                         "        from LY_AULA_DOCENTE ad WITH ( NOLOCK )  " +
                         "        join LY_TURMA t WITH ( NOLOCK ) on  " +
                         "        ad.ANO = t.ANO  " +
                         "        AND ad.SEMESTRE = t.SEMESTRE  " +
                         "        and ad.DISCIPLINA = t.DISCIPLINA  " +
                         "        and ad.FACULDADE = t.FACULDADE  " +
                         "        and ad.TURMA = t.TURMA  " +
                         "        and ad.TURNO = t.TURNO   " +
                         "        and ad.DATA_FIM = t.DT_FIM  " +
                         "        join LY_CURSO c WITH ( NOLOCK ) on c.CURSO=t.CURSO  " +
                         "        join LY_DOCENTE d WITH ( NOLOCK ) on d.NUM_FUNC = ad.NUM_FUNC  " +
                         "        where ad.NUM_FUNC = ?  " +
                         "        and t.SIT_TURMA = 'Aberta'  " +
                         "        and ad.DATA_FIM >= convert(date,GETDATE())   " +
                         "        and t.ANO = YEAR(GETDATE())) = 1 then 'SIM'  " +
                         "    else 'NÃO' " +
                         "    END em_aula, " +
                         " f.DESCRICAO as funcao, " +
                         " gh.DESCRICAO as disciplina, d.CATEGORIA as cargo, f.FUNCAO as codFuncao " +
                         " from LY_DOCENTE d WITH ( NOLOCK ) join LY_PESSOA p WITH ( NOLOCK ) " +
                         " on d.PESSOA = p.PESSOA " +
                         " join LY_LOTACAO l WITH ( NOLOCK ) " +
                         " on  d.MATRICULA = l.matricula " +
                         " join LY_FUNCAO f  WITH ( NOLOCK ) " +
                         " on l.FUNCAO = f.FUNCAO " +
                         " left join LY_GRUPO_HABILITACAO_DOC ghd WITH ( NOLOCK ) " +
                         " on  d.NUM_FUNC = ghd.NUM_FUNC " +
                         " and ghd.AGRUPAMENTO_INGRESSO = 'S' and ghd.PROVISORIO='N' " +
                         " left join LY_GRUPO_HABILITACAO gh WITH ( NOLOCK ) " +
                         " on ghd.AGRUPAMENTO = gh.AGRUPAMENTO " +
                         " where l.DATA_NOMEACAO <= convert(date,GetDate()) AND (l.DATA_DESATIVACAO is null OR convert(date,l.data_desativacao) > convert(date,GetDate()))" +
                         " and d.NUM_FUNC = ?";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, docente, docente);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static QueryTable ConsultarHorarios(string grade_id, string docente)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = " select do.MATRICULA as matricula, PE.NOME_COMPL as nome, a.DIA_SEMANA as dia, h.HORAINI_AULA as horainicial, " +
                    "h.HORAFIM_AULA as horafinal, d.NOME as disciplina, a.DATA_INICIO as datainicial, a.DATA_FIM as datafinal " +
                    "from LY_GRADE_SERIE gs join LY_GRADE_TURMA gt " +
                            "on gs.GRADE_ID = gt.GRADE_ID " +
                       "join LY_AULA_DOCENTE a  " +
                            "on  gt.DISCIPLINA = a.DISCIPLINA " +
                            "and gt.TURMA = a.TURMA " +
                            "and gt.ANO = a.ANO " +
                            "and gt.SEMESTRE = a.SEMESTRE " +
                       "join LY_HOR_AULA h " +
                            "on  a.TURNO = h.TURNO " +
                            "and a.FACULDADE = h.FACULDADE " +
                            "and a.DIA_SEMANA = h.DIA_SEMANA " +
                            "and a.AULA = h.AULA " +
                            "and a.DISCIPLINA = h.DISCIPLINA " +
                            "and a.TURMA = h.TURMA " +
                            "and a.ANO = h.ANO " +
                            "and a.SEMESTRE = h.SEMESTRE " +
                       "join LY_DISCIPLINA d " +
                            "on a.DISCIPLINA = d.DISCIPLINA " +
                      " join LY_DOCENTE do " +
                           " on a.NUM_FUNC = do.NUM_FUNC " +
                     " join LY_PESSOA PE " +
                           " on PE.PESSOA = DO.PESSOA " +
                    "where gs.GRADE_ID = ? " +
                      "and do.NUM_FUNC = ? " +
                    "order by do.MATRICULA, PE.NOME_COMPL, a.DIA_SEMANA, h.HORAINI_AULA, d.NOME, a.DATA_INICIO";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, grade_id, docente);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public string ObtemFuncaoCategoriaPor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 FUNCAO 
                            FROM LY_CATEGORIA_DOCENTE CD 
                            INNER JOIN LY_DOCENTE D
								ON D.CATEGORIA = CD.CATEGORIA
                            WHERE MATRICULA = @MATRICULA ";

            contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_CODIGO, matricula);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemCategoriaPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemCategoriaPor(contexto, matricula);
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

        public string ObtemCategoriaPor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT CATEGORIA 
                            FROM LY_DOCENTE (NOLOCK)
                            WHERE MATRICULA = @MATRICULA ";

            contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_CODIGO, matricula);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public bool EhMatriculaContratoPor(DataContext contexto, string numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                                FROM   LY_DOCENTE 
                                                WHERE  NUM_FUNC = @NUM_FUNC 
                                                       AND REGIMECONTRATACAOID = @REGIMECONTRATACAOID ";

            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
            contextQuery.Parameters.Add("@REGIMECONTRATACAOID", (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool MatriculaContrato(string numFunc)
        {
            if (string.IsNullOrEmpty(numFunc))
                return false;
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            try
            {
                string sql = @"SELECT 1 FROM ly_docente WHERE num_func = ? 
					and REGIMECONTRATACAOID = ?";

                //<>'CONTRATO TEMPORÁRIO'";
                qt = new QueryTable(sql);
                qt.Query(connection, numFunc, (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario);
            }
            finally
            {
                connection.Close();
            }
            return qt.Rows.Count > 0;
        }

        public void Insere(LyDocente docente, LyPessoa pessoa, RecursosHumanos.Entidades.Acumulacao acumulacao, LyGrupoHabilitacaoDoc grupoHabilitacaoDoc, LyLotacao lotacao, string zonaResidencial, string povoIndigenaId)
        {
            Pessoa rnPessoa = new Pessoa();
            RecursosHumanos.Acumulacao rnAcumulacao = new Techne.Lyceum.RN.RecursosHumanos.Acumulacao();
            RN.Lotacao rnLotacao = new Lotacao();
            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new GrupoHabilitacaoDoc();
            RN.FlPessoa rnFlPessoa = new FlPessoa();

            bool cadastroPessoa = (pessoa.Pessoa == 0);

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    if (cadastroPessoa)
                    {
                        rnPessoa.Insere(context, pessoa);
                    }
                    else
                    {
                        rnPessoa.AtualizaPessoaDocente(context, pessoa);
                    }

                    docente.Pessoa = pessoa.Pessoa;
                    docente.Usuario = pessoa.UsuarioId;

                    this.Insere(context, docente);

                    if (acumulacao != null && !acumulacao.MatriculaOrgao.IsNullOrEmptyOrWhiteSpace())
                    {
                        acumulacao.DocenteId = docente.Num_func;
                        acumulacao.UsuarioId = pessoa.UsuarioId;
                        rnAcumulacao.Insere(context, acumulacao);
                    }

                    if (grupoHabilitacaoDoc != null && !grupoHabilitacaoDoc.Agrupamento.IsNullOrEmptyOrWhiteSpace())
                    {
                        grupoHabilitacaoDoc.NumFunc = docente.Num_func;
                        rnGrupoHabilitacaoDoc.Insere(context, grupoHabilitacaoDoc);
                    }

                    if (lotacao != null && !lotacao.UnidadeEns.IsNullOrEmptyOrWhiteSpace())
                    {
                        lotacao.Pessoa = pessoa.Pessoa;
                        lotacao.DataNomeacaoDo = null;
                        lotacao.DataDesativacaoDo = null;
                        lotacao.DataDesativacao = null;
                        lotacao.AtoOficial = null;
                        lotacao.RespDocumentacao = null;
                        lotacao.DtInicioReadaptacao = null;
                        lotacao.DtFimReadaptacao = null;
                        lotacao.Usuario = pessoa.UsuarioId;
                        rnLotacao.Insere(context, lotacao);
                    }

                    if (cadastroPessoa || !rnFlPessoa.ExistePor(context, pessoa.Pessoa))
                    {     
                        //Caso a pessoa não exista ou o flpessoa não exista Insere
                        rnFlPessoa.InsereZonaResidencialPovoIndigena(context, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                    }
                    else
                    {
                        //Caso contrario, Atualiza Zona Residencial (FL_PESSOA - FL_FIELD_01)
                        rnFlPessoa.AtualizaZonaResidencialPovoIndigena(context, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                    }

                }
                catch (Exception)
                {
                    context.Abandon();

                    throw;
                }
            }
        }

        public int Insere(DataContext ctx, LyDocente docente)
        {
            try
            {
                var id = this.GeraDocentePor(ctx);

                if (PossuiDocenteCadastrado(id))
                {
                    id = this.GeraDocentePor(ctx);
                }

                docente.Num_func = id;

                var contextQuery = new ContextQuery
                {
                    Command =
                        @" INSERT INTO LY_DOCENTE 
                                    (PESSOA, 
                                     NUM_FUNC, 
                                     CATEGORIA, 
                                     DT_ADMISSAO, 
                                     DT_DEMISSAO, 
                                     MATRICULA, 
                                     SENHA_ALTERADA, 
                                     ANO_INGRESSO, 
                                     CONCURSO, 
                                     CANDIDATO, 
                                     VOLUNTARIO, 
                                     REGIMECONTRATACAOID, 
                                     SENHA_DOL, 
                                     REGIME_TRABALHO, 
                                     ACUMULACAO,
                                     VINCULO,
                                     USUARIO,
                                     DATACADASTRO,
                                     DATAALTERACAO) 
                        VALUES      (@PESSOA, 
                                     @NUM_FUNC, 
                                     @CATEGORIA, 
                                     @DT_ADMISSAO,  
                                     @DT_DEMISSAO, 
                                     @MATRICULA, 
                                     @SENHA_ALTERADA, 
                                     @ANO_INGRESSO, 
                                     @CONCURSO, 
                                     @CANDIDATO, 
                                     @VOLUNTARIO, 
                                     @REGIMECONTRATACAOID, 
                                     @SENHA_DOL, 
                                     @REGIME_TRABALHO, 
                                     @ACUMULACAO,
                                     @VINCULO,
                                     @USUARIO,
                                     @DATACADASTRO,
                                     @DATAALTERACAO)  "
                };

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, docente.Pessoa);
                contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMFUNC, docente.Num_func);
                contextQuery.Parameters.Add("@CATEGORIA", docente.Categoria);
                contextQuery.Parameters.Add("@DT_ADMISSAO", docente.Dt_admissao);
                contextQuery.Parameters.Add("@DT_DEMISSAO", docente.Dt_demissao);
                contextQuery.Parameters.Add("@MATRICULA", docente.Matricula);
                contextQuery.Parameters.Add("@SENHA_ALTERADA", "S");
                contextQuery.Parameters.Add("@ANO_INGRESSO", TechneDbType.T_ANO, docente.Ano_ingresso);
                contextQuery.Parameters.Add("@CONCURSO", docente.Concurso);
                contextQuery.Parameters.Add("@CANDIDATO", docente.Candidato);
                contextQuery.Parameters.Add("@VOLUNTARIO", docente.Voluntario == "S" ? docente.Voluntario : "N");
                contextQuery.Parameters.Add("@REGIMECONTRATACAOID", docente.RegimeContratacaoId);
                contextQuery.Parameters.Add("@SENHA_DOL", docente.Senha_dol);
                contextQuery.Parameters.Add("@REGIME_TRABALHO", docente.Regime_trabalho);
                contextQuery.Parameters.Add("@ACUMULACAO", docente.Acumulacao);
                contextQuery.Parameters.Add("@VINCULO", docente.Vinculo);
                contextQuery.Parameters.Add("@USUARIO", docente.Usuario);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

                return ctx.ApplyModifications(contextQuery);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AtualizaVoluntario(LyDocente docente, LyPessoa pessoa, string zonaResidencial)
        {
            FlPessoa rnFlPessoa = new FlPessoa();
            Pessoa rnPessoa = new Pessoa();

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    rnPessoa.AtualizaPessoaVoluntario(context, pessoa);

                    if (!rnFlPessoa.ExistePor(context, pessoa.Pessoa))
                    {
                        //Caso a pessoa não exista ou o flpessoa não exista Insere
                        rnFlPessoa.InsereZonaResidencial(context, pessoa.Pessoa, zonaResidencial);
                    }
                    else
                    {
                        //Caso contrario, Atualiza Zona Residencial (FL_PESSOA - FL_FIELD_01)
                        rnFlPessoa.AtualizaZonaResidencial(context, pessoa.Pessoa, zonaResidencial);
                    }

                    docente.Usuario = pessoa.UsuarioId;
                    Atualiza(docente, context);
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public void Atualiza(LyDocente docente, LyPessoa pessoa, RecursosHumanos.Entidades.Acumulacao acumulacao, LyLotacao lotacao, LyCandidatoDocente candidato, string zonaResidencial, RecursosHumanos.Entidades.GoogleEducation googleEducation, string povoIndigenaId)
        {
            Pessoa rnPessoa = new Pessoa();
            CandidatoDocente rnCandidatoDocente = new CandidatoDocente();
            RecursosHumanos.Acumulacao rnAcumulacao = new Techne.Lyceum.RN.RecursosHumanos.Acumulacao();
            RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RN.Lotacao rnLotacao = new Lotacao();

            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    rnPessoa.AtualizaPessoaDocente(context, pessoa);
                    Atualiza(docente, context);

                    if (lotacao != null)
                    {
                        if (rnLotacao.PossuiLotacaoAtivaPor(docente.Matricula))
                        {
                            rnLotacao.AlteraCategoria(context, lotacao.Categoria, lotacao.Usuario, lotacao.Matricula);
                        }
                    }

                    if (candidato != null)
                    {
                        rnCandidatoDocente.AtualizaCandidatoDocente(context, candidato);
                    }

                    if (acumulacao != null)
                    {
                        acumulacao.DocenteId = docente.Num_func;
                        acumulacao.UsuarioId = pessoa.UsuarioId;
                        rnAcumulacao.Salva(context, acumulacao);
                    }

                    if (!rnFlPessoa.ExistePor(context, pessoa.Pessoa))
                    {
                        //Caso a pessoa não exista ou o flpessoa não exista Insere
                        rnFlPessoa.InsereZonaResidencialPovoIndigena(context, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                    }
                    else
                    {
                        //Caso contrario, Atualiza Zona Residencial (FL_PESSOA - FL_FIELD_01)
                        rnFlPessoa.AtualizaZonaResidencialPovoIndigena(context, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                    }

                    //Verifica de tem email google
                    if (googleEducation != null && !googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                    {
                        googleEducation.Pessoa = docente.Pessoa;
                        googleEducation.UsuarioId = docente.Usuario;
                        rnGoogleEducation.Salva(context, googleEducation);
                    }
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public LyDocente CarregaPor(int numFunc)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            LyDocente docente = new LyDocente();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  *
                        FROM    LY_DOCENTE WITH ( NOLOCK )
                        WHERE   NUM_FUNC = @NUM_FUNC  "
                };

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

                docente = ctx.TryToBindEntity<LyDocente>(contextQuery);
                return docente;
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

        public static LyDocente Carregar(int numfunc)
        {
            try
            {
                LyDocente Doc = new LyDocente();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = "SELECT * FROM LY_DOCENTE WHERE NUM_FUNC = @NUM_FUNC "
                    };
                    contextQuery.Parameters.Add("@NUM_FUNC", numfunc);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            Doc.Num_func = (decimal)reader["Num_func"];
                            Doc.Categoria = reader["Categoria"].ToString();
                            Doc.Dt_admissao = reader["Dt_admissao"].ToString() == string.Empty ? (DateTime?)null : DateTime.Parse(reader["Dt_admissao"].ToString());
                            Doc.Pessoa = reader["Pessoa"] != DBNull.Value ? (decimal)reader["Pessoa"] : 0;
                            Doc.Dt_demissao = reader["Dt_demissao"].ToString() == string.Empty ? (DateTime?)null : DateTime.Parse(reader["Dt_demissao"].ToString());
                            Doc.Matricula = reader["Matricula"].ToString();
                            Doc.Senha_alterada = reader["Senha_alterada"].ToString();
                            Doc.Ano_ingresso = reader["Ano_ingresso"].ToString() != string.Empty ? (decimal)reader["Ano_ingresso"] : (decimal?)null;
                            Doc.Concurso = reader["Concurso"].ToString();
                            Doc.Candidato = reader["Candidato"].ToString();
                            Doc.RegimeContratacaoId = reader["regimecontratacaoid"] != DBNull.Value ? Convert.ToInt32(reader["regimecontratacaoid"]) : (int?)null;
                            Doc.Acumulacao = Convert.ToInt32(reader["Acumulacao"]);
                        }
                    }
                    return Doc;

                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        private void Atualiza(LyDocente docente, DataContext ctx)
        {
            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" UPDATE LY_DOCENTE SET  
                                CATEGORIA = @CATEGORIA,
                                DT_ADMISSAO = @DT_ADMISSAO,                               
                                PESSOA = @PESSOA,                           
                                DT_DEMISSAO = @DT_DEMISSAO,
                                MATRICULA = @MATRICULA,
                                ANO_INGRESSO = @ANO_INGRESSO,                                
                                CONCURSO = @CONCURSO,
                                CANDIDATO = @CANDIDATO,     
                                REGIMECONTRATACAOID = @REGIMECONTRATACAOID,
                                REGIME_TRABALHO = @REGIME_TRABALHO,
                                ACUMULACAO = @ACUMULACAO,
                                VINCULO = @VINCULO,
                                USUARIO = @USUARIO,
                                DATAALTERACAO = @DATAALTERACAO
                          WHERE NUM_FUNC = @NUM_FUNC "
            };

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, docente.Pessoa);
            contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMFUNC, docente.Num_func);
            contextQuery.Parameters.Add("@CATEGORIA", docente.Categoria);
            contextQuery.Parameters.Add("@DT_ADMISSAO", TechneDbType.T_DATA, docente.Dt_admissao);
            contextQuery.Parameters.Add("@DT_DEMISSAO", TechneDbType.T_DATA, docente.Dt_demissao);
            contextQuery.Parameters.Add("@MATRICULA", docente.Matricula);
            contextQuery.Parameters.Add("@ANO_INGRESSO", TechneDbType.T_ANO, docente.Ano_ingresso);
            contextQuery.Parameters.Add("@CONCURSO", docente.Concurso);
            contextQuery.Parameters.Add("@CANDIDATO", docente.Candidato);
            contextQuery.Parameters.Add("@REGIMECONTRATACAOID", docente.RegimeContratacaoId);
            contextQuery.Parameters.Add("@REGIME_TRABALHO", docente.Regime_trabalho);
            contextQuery.Parameters.Add("@ACUMULACAO", docente.Acumulacao);
            contextQuery.Parameters.Add("@VINCULO", docente.Vinculo);
            contextQuery.Parameters.Add("@USUARIO", docente.Usuario);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaCargo(DataContext ctx, int numFunc, string categoria, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" UPDATE LY_DOCENTE SET  
                                CATEGORIA = @CATEGORIA,                               
                                USUARIO = @USUARIO,
                                DATAALTERACAO = @DATAALTERACAO
                          WHERE NUM_FUNC = @NUM_FUNC "
            };

            contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMFUNC, numFunc);
            contextQuery.Parameters.Add("@CATEGORIA", categoria);
            contextQuery.Parameters.Add("@USUARIO", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public static DataTable VerificaDocenteAtivo(string idVinculo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {

                var contextQuery = new ContextQuery(
                    @"SELECT  LO.FUNCAO
                    FROM   LY_DOCENTE D 
                    INNER JOIN LY_PESSOA PE ON PE.PESSOA = D.PESSOA
                    INNER JOIN LY_LOTACAO LO (NOLOCK) 
                           ON D.MATRICULA = LO.MATRICULA
                    WHERE (LO.DATA_DESATIVACAO IS NULL 
                           OR CONVERT(DATE,LO.DATA_DESATIVACAO) > CONVERT(DATE,GETDATE()))
                           AND D.VINCULO = @VINCULO
                           AND PE.IDFUNCIONAL = @IDFUNCIONAL
                    AND NOT EXISTS (SELECT 1 FROM  LY_LICENCA_DOCENTE L WITH(NOLOCK)  
                        WHERE D.NUM_FUNC = L.NUM_FUNC  
                        AND  MOTIVO <> '43'   
                        AND (DTFIM IS NULL OR DTFIM >= CONVERT(DATE, CURRENT_TIMESTAMP)))");

                contextQuery.Parameters.Add("@IDFUNCIONAL", idVinculo.Split('/')[0]);
                contextQuery.Parameters.Add("@VINCULO", idVinculo.Split('/')[1]);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool EhVoluntarioPor(decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.EhVoluntarioPor(ctx, pessoa);
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

        public bool EhVoluntarioPor(DataContext ctx, string cpf)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                                    FROM   LY_DOCENTE D (NOLOCK)
                                           INNER JOIN LY_PESSOA P (NOLOCK) ON D.PESSOA = P.PESSOA
                                    WHERE  P.CPF = @CPF 
                                        AND D.VOLUNTARIO = 'S' "
            };

            contextQuery.Parameters.Add("@CPF", cpf);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool EhVoluntarioPor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                                    FROM   LY_DOCENTE D (NOLOCK)
                                           INNER JOIN LY_PESSOA P (NOLOCK) ON D.PESSOA = P.PESSOA
                                    WHERE  P.PESSOA = @PESSOA 
                                        AND D.VOLUNTARIO = 'S' "
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool EhDocentePor(decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.EhDocentePor(ctx, pessoa);

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

        public bool EhDocentePor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
             {
                 Command = @" SELECT COUNT(*) 
                                    FROM   LY_DOCENTE 
                                    WHERE  PESSOA = @PESSOA "
             };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool EhDocenteAtivoPor(decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.EhDocenteAtivoPor(ctx, pessoa);
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

        private bool EhDocenteAtivoPor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
              {
                  Command = @" SELECT COUNT(*) 
                                    FROM   LY_DOCENTE 
                                    WHERE  PESSOA = @PESSOA 
                                           AND ( DT_DEMISSAO IS NULL 
                                                  OR DT_DEMISSAO > GETDATE() )  "
              };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiLotacaoAtivaPor(decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiLotacaoAtivaPor(ctx, pessoa);
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

        private bool PossuiLotacaoAtivaPor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
                 {
                     Command = @" SELECT COUNT(*) 
                                    FROM   LY_LOTACAO L ( NOLOCK ) 
                                           INNER JOIN LY_DOCENTE D ( NOLOCK ) 
                                                   ON L.MATRICULA = D.MATRICULA 
                                    WHERE  L.PESSOA = @PESSOA 
                                           AND ( L.DATA_DESATIVACAO IS NULL 
                                                  OR L.DATA_DESATIVACAO >= GETDATE() )  "
                 };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public DataTable ObtemMatriculaIdVinculoPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT D.MATRICULA, 
                                                D.MATRICULA + ';' + ISNULL(CONVERT(VARCHAR(20),P.IDFUNCIONAL),'') + ';' + ISNULL(CONVERT(VARCHAR(20),D.VINCULO),'') as MATRICULADESC, 
		                                        P.IDFUNCIONAL,
		                                        D.VINCULO,
		                                        ISNULL((CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,D.VINCULO)), D.MATRICULA) IDVINCULO
                                        FROM LY_DOCENTE D
		                                        INNER JOIN LY_PESSOA P ON D.PESSOA = P.PESSOA
                                        WHERE D.PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return dt;
        }

        public bool PossuiLicencaDefinitivaPor(decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiLicencaDefinitivaPor(ctx, pessoa);
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

        private bool PossuiLicencaDefinitivaPor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(*) 
                                FROM   LY_LICENCA_DOCENTE LD 
                                       INNER JOIN LY_LICENCAS L 
                                               ON LD.MOTIVO = L.MOTIVO 
                                       INNER JOIN LY_DOCENTE D 
                                               ON D.NUM_FUNC = LD.NUM_FUNC 
                                WHERE  L.POSSUI_DTFIM = 'N' 
                                       AND D.PESSOA = @PESSOA  "
                };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private string ObtemMatriculaFicticiaPor(DataContext contexto)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 MATRICULA 
                    FROM   TCE_MATRICULA_VOLUNTARIO V 
                    WHERE  UTILIZADA = 0 
                           AND NOT EXISTS(SELECT * 
                                          FROM   LY_DOCENTE D 
                                          WHERE  D.MATRICULA = V.MATRICULA) 
                           AND NOT EXISTS(SELECT * 
                                          FROM   LY_VINCULO VI 
                                          WHERE  VI.MATRICULA = V.MATRICULA) 
                    ORDER  BY MATRICULA  ";

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        private void InutilizarMatriculaFicticia(DataContext ctx, string matricula, string user)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE [TCE_MATRICULA_VOLUNTARIO] 
                                    SET    UTILIZADA = 1, 
                                           DT_UTILIZACAO = GETDATE(), 
                                           USUARIO = @USUARIO 
                                    WHERE  MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);
                contextQuery.Parameters.Add("@USUARIO", user);

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

        private static bool PossuiDocenteCadastrado(decimal numFunc)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"SELECT    *
                              FROM      LY_DOCENTE
                              WHERE     NUM_FUNC = @NUM_FUNC"
                };
                contextQuery.Parameters.Add("@NUM_FUNC", TechneDbType.T_NUMERO, numFunc);

                object obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static int ConsultarDocentePorCategoria(string strCategoria)
        {
            StringBuilder stb = new StringBuilder("select COUNT(*) from LY_DOCENTE ");
            stb.Append(" where CATEGORIA = ? ");

            var qtd = ExecutarFuncao(stb.ToString(), strCategoria);

            return qtd;
        }

        public void AtualizaDemissaoDocente(DataContext ctx, DateTime dtDataDemissao, string strNumFunc)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_DOCENTE
										SET DT_DEMISSAO = @DT_DEMISSAO
										WHERE NUM_FUNC = @NUM_FUNC ";

                contextQuery.Parameters.Add("@DT_DEMISSAO", dtDataDemissao);
                contextQuery.Parameters.Add("@NUM_FUNC", strNumFunc);

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

        public static DataTable ConsultarDadosDocente(string strConcurso, string strCandidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable consultaDadosDocente = null;

            try
            {
                contextQuery.Command = @" select * from LY_DOCENTE where CONCURSO=@CONCURSO and CANDIDATO=@CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);

                consultaDadosDocente = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return consultaDadosDocente;
        }

        public DadosTrocaMatriculaDocente ObtemDadosTrocaMatriculaDocentePor(string matricula)
        {
            DadosTrocaMatriculaDocente dados = new DadosTrocaMatriculaDocente();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT d.NUM_FUNC ,
                                    d.PESSOA ,
									p.IDFUNCIONAL,
									d.VINCULO,
                                    d.MATRICULA ,
                                    p.NOME_COMPL ,
                                    p.DT_NASC ,
                                    p.CPF ,
                                    p.SEXO,
                                    ISNULL((CONVERT(VARCHAR,P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR,D.VINCULO)),D.MATRICULA) IDVINCULO_MATRICULA,
									REGIME_TRABALHO
                            FROM    LY_DOCENTE d ( NOLOCK )
                                    INNER JOIN dbo.LY_PESSOA p ( NOLOCK ) ON d.PESSOA = p.PESSOA
                            WHERE   d.MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.NumFunc = Convert.ToInt32(reader["NUM_FUNC"]); ;
                    dados.Pessoa = Convert.ToInt32(reader["PESSOA"]);

                    if (reader["IDFUNCIONAL"] != DBNull.Value)
                    {
                        dados.IdFuncional = Convert.ToInt32(reader["IDFUNCIONAL"]);
                    }

                    if (reader["VINCULO"] != DBNull.Value)
                    {
                        dados.Vinculo = Convert.ToInt32(reader["VINCULO"]);
                    }

                    dados.Matricula = Convert.ToString(reader["MATRICULA"]);
                    dados.NomeCompl = Convert.ToString(reader["NOME_COMPL"]);

                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dados.DtNasc = Convert.ToDateTime(reader["DT_NASC"]);
                    }

                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);

                    if (reader["REGIME_TRABALHO"] != DBNull.Value && !Convert.ToString(reader["REGIME_TRABALHO"]).IsNullOrEmptyOrWhiteSpace())
                    {
                        dados.RegimeTrabalho = Convert.ToDecimal(reader["REGIME_TRABALHO"]);
                    }

                    dados.IdVinculoMatricula = Convert.ToString(reader["IDVINCULO_MATRICULA"]);
                    
                }

                return dados;
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

        public string ObtemSegundaMatriculaAtivaPor(DataContext contexto, string matricula, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 D.MATRICULA AS SEGUNDAMATRICULA 
                                    FROM   LY_DOCENTE D (NOLOCK) 
                                           INNER JOIN LY_LOTACAO L (NOLOCK) 
                                                   ON D.MATRICULA = L.MATRICULA 
                                    WHERE  D.PESSOA = @PESSOA 
                                           AND D.MATRICULA <> @MATRICULA
                                           AND ( L.DATA_DESATIVACAO IS NULL 
                                                  OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()) )  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }


        public ValidacaoDados ValidaTrocaMatricula(Entidades.LogAtualizacaoMatricula logAtualizacaoMatricula, int pessoa)
        {
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.VinculoLy rnVinculo = new VinculoLy();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int aulasAlocadas = 0;
            int glpsAlocadas = 0;
            int matricula = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (logAtualizacaoMatricula == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(logAtualizacaoMatricula.MatriculaAnterior) && logAtualizacaoMatricula.IdFuncionalAnterior == null)
            {
                mensagens.Add("Campo ID FUNCIONAL OU MATRICULA ATUAL é obrigatório.");
            }
            else
            {
                if (string.IsNullOrEmpty(logAtualizacaoMatricula.MatriculaAnterior))
                {
                    logAtualizacaoMatricula.MatriculaAnterior = string.Format("{0}/{1}", logAtualizacaoMatricula.IdFuncionalAnterior, logAtualizacaoMatricula.VinculoAnterior);
                }

                if (logAtualizacaoMatricula.IdFuncionalNovo == logAtualizacaoMatricula.IdFuncionalAnterior
                    && logAtualizacaoMatricula.VinculoNovo == logAtualizacaoMatricula.VinculoAnterior
                    && logAtualizacaoMatricula.MatriculaNova == logAtualizacaoMatricula.MatriculaAnterior)
                {
                    mensagens.Add("Campo ID FUNCIONAL OU VINCULO OU MATRÍCULA NOVA deve ser diferente do ATUAL.");
                }

                if (logAtualizacaoMatricula.MatriculaNova == "0")
                {
                    mensagens.Add("Campo MATRICULA NOVA deve ser diferente de 0.");
                }
            }

            if (string.IsNullOrEmpty(logAtualizacaoMatricula.MatriculaNova) && logAtualizacaoMatricula.IdFuncionalNovo == null)
            {
                mensagens.Add("Campo ID FUNCIONAL OU MATRICULA NOVA é obrigatório.");
            }
            else if (string.IsNullOrEmpty(logAtualizacaoMatricula.MatriculaNova))
            {
                logAtualizacaoMatricula.MatriculaNova = string.Format("{0}/{1}", logAtualizacaoMatricula.IdFuncionalNovo, logAtualizacaoMatricula.VinculoNovo);
            }

            if (logAtualizacaoMatricula.IdFuncionalNovo != null && logAtualizacaoMatricula.VinculoNovo == null)
            {
                mensagens.Add("Campo VINCULO NOVO é obrigatório quando o ID FUNCIONAL NOVO for informado.");
            }

            if (logAtualizacaoMatricula.DocenteId <= 0)
            {
                mensagens.Add("O NUM_FUNC é obrigatório.");
            }

            if (pessoa <= 0)
            {
                mensagens.Add("O PESSOA é obrigatório.");
            }

            if (string.IsNullOrEmpty(logAtualizacaoMatricula.UsuarioId))
            {
                mensagens.Add("O USUÁRIO RESPONSAVEL é obrigatório.");
            }


            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (logAtualizacaoMatricula.VinculoNovo <= 0)
                    {
                        mensagens.Add("O VINCULO NOVO não pode ser 0");
                    }


                    if (string.IsNullOrEmpty(logAtualizacaoMatricula.MatriculaNova) || logAtualizacaoMatricula.IdFuncionalNovo == null)
                    {
                        mensagens.Add("Campo ID/VINCULO é obrigatório.");
                    }

                    //Verifica aulas do docente
                    aulasAlocadas = rnAulaDocente.ObtemQuantidadeAulasAtivasDocentePor(contexto, logAtualizacaoMatricula.MatriculaAnterior);
                    if (aulasAlocadas > 0)
                    {
                        mensagens.Add(string.Format("Id/Vinculo ou Matrícula {0} possui {1} aulas alocadas e não pode ser atualizada.", logAtualizacaoMatricula.MatriculaAnterior, aulasAlocadas));
                    }

                    //Verifica glps do docente
                    glpsAlocadas = rnAulaDocente.ObtemQuantidadeGlpsAtivasDocentePor(contexto, logAtualizacaoMatricula.MatriculaAnterior);
                    if (glpsAlocadas > 0)
                    {
                        mensagens.Add(string.Format("Id/Vinculo ou Matrícula {0} possui {1} glps ativas e não pode ser atualizada.", logAtualizacaoMatricula.MatriculaAnterior, glpsAlocadas));
                    }

                    if (logAtualizacaoMatricula.IdFuncionalNovo != null && logAtualizacaoMatricula.IdFuncionalAnterior != logAtualizacaoMatricula.IdFuncionalNovo)
                    {
                        //Verifica se existe o Id para otura pessoa
                        if (rnPessoa.PossuiOutroIdFuncionalPor(contexto, Convert.ToInt32(logAtualizacaoMatricula.IdFuncionalNovo), pessoa))
                        {
                            mensagens.Add("Já existe uma pessoa cadastrada com esse número de ID FUNCIONAL.");
                        }
                    }

                    if (logAtualizacaoMatricula.VinculoNovo != null && logAtualizacaoMatricula.VinculoAnterior != logAtualizacaoMatricula.VinculoNovo)
                    {
                        //Verifica se já existe o vinculo para este docente
                        if (this.PossuiOutroVinculoPor(contexto, pessoa, Convert.ToInt32(logAtualizacaoMatricula.VinculoNovo), logAtualizacaoMatricula.DocenteId) ||
                            rnVinculo.PossuiOutroVinculoPor(contexto, pessoa, Convert.ToInt32(logAtualizacaoMatricula.VinculoNovo), logAtualizacaoMatricula.MatriculaAnterior))
                        {
                            mensagens.Add("Este VINCULO já se encontra cadastrado para esta pessoa.");
                        }
                    }

                    if (logAtualizacaoMatricula.MatriculaAnterior != logAtualizacaoMatricula.MatriculaNova)
                    {
                        //Verifica se matricula é utilizada por outro docente
                        matricula = this.ObtemQuantidadeDocentePor(contexto, logAtualizacaoMatricula.MatriculaNova);
                        if (matricula > 0)
                        {
                            mensagens.Add(string.Format("Id/Vinculo ou Matrícula {0} está associada a outro docente. Atualização não permitida.", logAtualizacaoMatricula.MatriculaNova));
                        }
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
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

        private int ObtemQuantidadeDocentePor(DataContext ctx, string matricula)
        {
            int quantidadeDocentes = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  COUNT(MATRICULA) CONTADOR
                                FROM    LY_DOCENTE
                                WHERE   MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidadeDocentes = Convert.ToInt32(reader["CONTADOR"]);
                }

                return quantidadeDocentes;
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
            }
        }

        public void TrocaMatricula(Entidades.LogAtualizacaoMatricula logAtualizacaoMatricula, int pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.LogAtualizacaoMatricula rnLogAtualizacaoMatricula = new LogAtualizacaoMatricula();
            RN.Lotacao rnLotacao = new Lotacao();
            RN.Pessoa rnPessoa = new Pessoa();

            try
            {
                if (logAtualizacaoMatricula.MatriculaAnterior != logAtualizacaoMatricula.MatriculaNova
                    || logAtualizacaoMatricula.VinculoAnterior != logAtualizacaoMatricula.VinculoNovo)
                {
                    //Altera tabela docente
                    this.AlteraMatriculaDocente(ctx, logAtualizacaoMatricula);
                }

                if (logAtualizacaoMatricula.MatriculaAnterior != logAtualizacaoMatricula.MatriculaNova)
                {
                    //Altera tabela lotaçao
                    rnLotacao.AlteraMatriculaLotacao(ctx, logAtualizacaoMatricula);
                }

                if (logAtualizacaoMatricula.IdFuncionalAnterior != logAtualizacaoMatricula.IdFuncionalNovo)
                {
                    //Altera Id
                    rnPessoa.AlteraIdFuncional(pessoa, logAtualizacaoMatricula.UsuarioId, logAtualizacaoMatricula.IdFuncionalNovo);
                }

                //insere log
                rnLogAtualizacaoMatricula.Insere(ctx, logAtualizacaoMatricula);
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

        private void AlteraMatriculaDocente(DataContext ctx, Entidades.LogAtualizacaoMatricula logAtualizacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  LY_DOCENTE
                                    SET     MATRICULA = @MATRICULANOVA,
                                            VINCULO = @VINCULONOVO
                                    WHERE   NUM_FUNC = @DOCENTEID
                                            AND MATRICULA = @MATRICULAANTERIOR ";

                contextQuery.Parameters.Add("@DOCENTEID", logAtualizacaoMatricula.DocenteId);
                contextQuery.Parameters.Add("@MATRICULAANTERIOR", logAtualizacaoMatricula.MatriculaAnterior);
                contextQuery.Parameters.Add("@MATRICULANOVA", logAtualizacaoMatricula.MatriculaNova);
                contextQuery.Parameters.Add("@VINCULONOVO", logAtualizacaoMatricula.VinculoNovo);

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

        public DTOs.DadosPessoaisAAGEMediador ObtemDadosPessoaisAAGEMediadorPor(decimal numFunc)
        {
            DTOs.DadosPessoaisAAGEMediador dadosPessoais = new DTOs.DadosPessoaisAAGEMediador();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  D.NUM_FUNC ,
                                            D.MATRICULA ,
                                            P.CPF ,
                                            P.NOME_COMPL ,
                                            P.DT_NASC ,
                                            P.SEXO ,
                                            P.EST_CIVIL ,
                                            P.ENDERECO ,
                                            P.END_NUM ,
                                            P.END_COMPL ,
                                            P.BAIRRO ,
                                            P.CEP ,
                                            M.NOME AS MUNICIPIO ,
                                            P.FONE
                                    FROM    LY_DOCENTE D WITH ( NOLOCK )
                                            INNER JOIN LY_PESSOA P WITH ( NOLOCK ) ON D.PESSOA = P.PESSOA
                                            INNER JOIN MUNICIPIO M WITH ( NOLOCK ) ON M.CODIGO = CONVERT(INT, P.END_MUNICIPIO)
                                    WHERE   D.NUM_FUNC = @NUM_FUNC ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosPessoais.DocenteId = Convert.ToDecimal(reader["NUM_FUNC"]);
                    dadosPessoais.Matricula = Convert.ToString(reader["MATRICULA"]);
                    dadosPessoais.Cpf = Convert.ToString(reader["CPF"]);
                    dadosPessoais.NomeCompleto = Convert.ToString(reader["NOME_COMPL"]);
                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dadosPessoais.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    dadosPessoais.Sexo = Convert.ToString(reader["SEXO"]);
                    dadosPessoais.EstadoCivl = Convert.ToString(reader["EST_CIVIL"]);
                    dadosPessoais.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosPessoais.Numero = Convert.ToString(reader["END_NUM"]);
                    dadosPessoais.Complemento = Convert.ToString(reader["END_COMPL"]);
                    dadosPessoais.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosPessoais.Cep = Convert.ToString(reader["CEP"]);
                    dadosPessoais.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosPessoais.Telefone = Convert.ToString(reader["FONE"]);
                }

                return dadosPessoais;
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

        public bool PossuiOutroVinculoPor(DataContext ctx, decimal pessoa, int vinculo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_DOCENTE (NOLOCK)
                                        WHERE PESSOA = @PESSOA
	                                        AND VINCULO = @VINCULO ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@VINCULO", vinculo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public ValidacaoDados Valida(LyPessoa pessoa, LyDocente docente, RecursosHumanos.Entidades.Acumulacao acumulacao, LyGrupoHabilitacaoDoc grupoHabilitacaoDoc, LyLotacao lotacao, bool dadosIngresso, LyCandidatoDocente candidato, string zonaResidencial, bool cadastro, bool possuiId, string SemLocalizacaoDiferenciada, RecursosHumanos.Entidades.GoogleEducation googleEducation, string povoIndigenaId)
        {
            List<string> mensagens = new List<string>();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Lotacao rnLotacao = new Lotacao();
            RN.VinculoLy rnVinculo = new VinculoLy();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docente == null || pessoa == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (pessoa.Pessoa <= 0)
                {
                    mensagens.Add("O campo PESSOA é de preenchimento obrigatório.");
                }

                if (docente.Num_func <= 0)
                {
                    mensagens.Add("O campo NUM_FUNC é de preenchimento obrigatório.");
                }
            }

            //Atualiza campos equivalentes do candidato 
            if (candidato != null)
            {
                candidato.Nome = pessoa.Nome_compl;
                candidato.Dt_nasc = pessoa.Dt_nasc;
                candidato.Sexo = pessoa.Sexo;
                candidato.Estado_civil = pessoa.Est_civil;
                candidato.NecessidadeEspecialId = pessoa.NecessidadeEspecialId;
                candidato.Pais_nasc = pessoa.Pais_nasc;
                candidato.Nacionalidade = pessoa.Nacionalidade;
                candidato.Municipio_nasc = pessoa.Municipio_nasc;
                candidato.Nome_mae = pessoa.NomeMae;
                candidato.Nome_pai = pessoa.NomePai;
                candidato.End_pais = pessoa.End_pais;
                candidato.Cep = pessoa.Cep;
                candidato.End_municipio = pessoa.End_municipio;
                candidato.Endereco = pessoa.Endereco;
                candidato.End_num = pessoa.End_num;
                candidato.End_compl = pessoa.End_compl;
                candidato.Bairro = pessoa.Bairro;
                candidato.Rg_tipo = pessoa.Rg_tipo;
                candidato.Rg_num = pessoa.Rg_num;
                candidato.Rg_uf = pessoa.Rg_uf;
                candidato.Rg_emissor = pessoa.Rg_emissor;
                candidato.Rg_dtexp = pessoa.Rg_dtexp;
                candidato.Cpf = pessoa.Cpf;
                candidato.Pis_pasep = pessoa.Pispasep;
                candidato.E_mail = pessoa.E_mail;
                candidato.Categoria = docente.Categoria;
                candidato.Candidato = docente.Candidato;
                candidato.Concurso = docente.Concurso;
                candidato.Dt_proposta = docente.Dt_admissao;
                candidato.Fone = pessoa.Fone.RetirarMascaraTelefone();
                candidato.Celular = pessoa.Celular.RetirarMascaraTelefone();
                candidato.Cprof_num = pessoa.Cprof_num;
                candidato.Cprof_serie = pessoa.Cprof_serie;
                candidato.Cprof_dtexp = pessoa.Cprof_dtexp;
                candidato.Cprof_uf = pessoa.Cprof_uf;
                candidato.IdFuncional = pessoa.IdFuncional;
            }

            if (pessoa.Nome_compl.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME é de preenchimento obrigatório.");
            }
            else
            {
                //Verificar nome valido
                int n = 0;
                for (n = 0; n <= 9; n++)
                {
                    if (pessoa.Nome_compl.IndexOf(n.ToString()) > 0)
                    {
                        mensagens.Add("Nome Completo: Não se pode ter números no nome.(" + n.ToString() + ").");
                    }
                }

                string[] vetorNome = pessoa.Nome_compl.Split(' ');

                if (vetorNome.Length == 1)
                {
                    mensagens.Add("Nome Completo: O Nome não pode ser formado por apenas uma palavra.");
                }

                if (Utils.VerificaTriploCaracter(pessoa.Nome_compl))
                {
                    mensagens.Add("Nome Completo: Não se pode ter três letras iguais consecutivas no nome.");
                }
            }

            if (pessoa.Dt_nasc == DateTime.MinValue || pessoa.Dt_nasc == null)
            {
                mensagens.Add("O campo DATA DE NASCIMENTO é de preenchimento obrigatório.");
            }
            else
            {
                //Valida idade de acordo com o regime de contratacao
                if (docente.RegimeContratacaoId != null)
                {
                    int idade = Utils.CalcularIdade(Convert.ToDateTime(pessoa.Dt_nasc));

                    //Verifica se o regime de contratação é 'Contrato Temporário'
                    bool ehContratoTemporario = docente.RegimeContratacaoId == (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario;

                    if (ehContratoTemporario)
                    {
                        //Para 'Contrato Temporário' o docente deve ter idade entre 18 e 80
                        if (idade < 18 || idade > 80)
                        {
                            mensagens.Add("Idade: Apenas docentes com idades entre 18 e 80 anos poderão ser incluidos.");
                        }
                    }
                    else
                    {
                        //Para outros regimes de contratação, o docente deve ter idade entre 18 e 70
                        if (idade < 18 || idade > 75)
                        {
                            mensagens.Add("Idade: Apenas docentes com idades entre 18 e 75 anos poderão ser incluidos.");
                        }
                    }
                }
            }
            if (pessoa.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo SEXO é de preenchimento obrigatório.");
            }
            if (pessoa.Etnia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo COR/RAÇA é de preenchimento obrigatório.");
            }
            else
            {
                //Verifica se foi informado o codigo do povo indigena e se a cor/raça é indigena
                if (!povoIndigenaId.IsNullOrEmptyOrWhiteSpace() && pessoa.Etnia != "Índigena")
                {
                    mensagens.Add("Apenas pode ser informado o campo POVO ÍNDIGENA quando o campo COR/RAÇA for Índigena.");
                }

                if (pessoa.Etnia == "Índigena" && povoIndigenaId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo POVO INDÍGENA é obrigatório!");
                }

                if (candidato != null)
                {
                    ///Busca o id da etnia
                    RN.Etnia rnEtnia = new Etnia();
                    candidato.EtniaId = rnEtnia.ObtemEtniaIdPor(pessoa.Etnia);
                }
            }

            if (pessoa.Est_civil.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ESTADO CIVIL é de preenchimento obrigatório.");
            }
            if (pessoa.NecessidadeEspecialId == null || pessoa.NecessidadeEspecialId <= 0)
            {
                mensagens.Add("O campo NECESSIDADE ESPECIAL é de preenchimento obrigatório.");
            }

            if (pessoa.Pais_nasc.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo PAÍS DE NASCIMENTO é de preenchimento obrigatório.");
            }
            if (pessoa.Nacionalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NACIONALIDADE é de preenchimento obrigatório.");
            }
            else
            {
                if (pessoa.Pais_nasc == "1" && pessoa.Nacionalidade != "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser Brasil com a NACIONALIDADE diferente de brasileira.");
                }

                if (pessoa.Pais_nasc != "1" && pessoa.Nacionalidade == "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser diferente de Brasil e a NACIONALIDADE ser brasileira.");
                }

                if (pessoa.Nacionalidade == "BRASILEIRA")
                {
                    if (string.IsNullOrEmpty(pessoa.Municipio_nasc) || pessoa.Municipio_nasc == "<Nenhum>")
                    {
                        mensagens.Add("O campo NATURALIDADE é de preenchimento obrigatório.");
                    }
                }
            }

            if (pessoa.Municipio_nasc.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NATURALIDADE é de preenchimento obrigatório.");
            }
            if (pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME DA MÃE é de preenchimento obrigatório.");
            }
            if (pessoa.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME DO PAI é de preenchimento obrigatório.");
            }
            if (pessoa.End_pais.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo PAÍS DE ENDEREÇO é de preenchimento obrigatório.");
            }
            if (pessoa.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CEP é de preenchimento obrigatório.");
            }
            if (pessoa.End_municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MUNICIPIO é de preenchimento obrigatório.");
            }
            if (pessoa.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ENDEREÇO DA PESSOA é de preenchimento obrigatório.");
            }
            if (pessoa.End_num.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NÚMERO DO ENDEREÇO é de preenchimento obrigatório.");
            }
            if (pessoa.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo BAIRRO é de preenchimento obrigatório.");
            }

            if (zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O  campo LOCALIZAÇÃO é de preenchimento obrigatório.");
            }

            if (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.AreaQuilombos != "N" && pessoa.AreaQuilombos != "S"))
            {
                mensagens.Add("O campo AREA DE QUILOMBOS é obrigatório com os Valores N ou S.");
            }

            if (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.TerraIndigena != "N" && pessoa.TerraIndigena != "S"))
            {
                mensagens.Add("O campo TERRA INDIGENA é obrigatório com os Valores N ou S.");
            }

            if (pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.AreaAssentamento != "N" && pessoa.AreaAssentamento != "S"))
            {
                mensagens.Add("O campo AREA DE ASSENTAMENTO é obrigatório com os Valores N ou S.");
            }

            if (SemLocalizacaoDiferenciada.IsNullOrEmptyOrWhiteSpace()
                || (SemLocalizacaoDiferenciada != "N" && SemLocalizacaoDiferenciada != "S"))
            {
                mensagens.Add("O campo SEM LOCALIZAÇÃO DIFERENCIADA é obrigatório com os Valores N ou S.");
            }
            else
            {
                if (SemLocalizacaoDiferenciada == "N")
                {
                    if ((pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaAssentamento == "N")
                        && (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() || pessoa.TerraIndigena == "N")
                        && (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaQuilombos == "N"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA é obrigatório.");
                    }
                }
                else
                {
                    if ((!pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaAssentamento == "S")
                        || (!pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() && pessoa.TerraIndigena == "S")
                        || (!pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaQuilombos == "S"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA não pode possuir outra marcação quando NÃO SE APLICA estiver selecionado.");
                    }
                }
            }

            if (pessoa.Rg_tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TIPO DE DOCUMENTO é de preenchimento obrigatório.");
            }
            else
            {
                #region Validações dos Campos de Documento
                bool documentoValido, iniciouMensagem, maisDeUmCampo;
                documentoValido = true;
                iniciouMensagem = maisDeUmCampo = false;
                System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
                System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
                mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");

                if (pessoa.Rg_tipo == "RG")
                {
                    if (Convert.ToString(pessoa.Rg_num).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        iniciouMensagem = true;
                        camposDocumento.Append("Número ");
                    }

                    if (Convert.ToString(pessoa.Rg_uf).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Estado ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Estado ");
                        }
                    }

                    if (Convert.ToString(pessoa.Rg_emissor).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }

                    if (pessoa.Rg_dtexp == null)
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Data de Expedição ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Data de Expedição ");
                        }
                    }
                }
                else
                {
                    if (Convert.ToString(pessoa.Rg_num).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        iniciouMensagem = true;
                        camposDocumento.Append("Número ");
                    }
                    if (Convert.ToString(pessoa.Rg_uf).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }
                }

                if (!documentoValido)
                {
                    if (maisDeUmCampo)
                    {
                        mensagemDocumento.Append("<br>Campos Necessários: ");
                    }
                    else
                    {
                        mensagemDocumento.Append("<br>Campo Necessário: ");
                    }

                    mensagemDocumento.Append(camposDocumento);
                    mensagens.Add(mensagemDocumento.ToString());
                }

                #endregion
            }

            if (pessoa.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CPF é de preenchimento obrigatório.");
            }
            else
            {
                if (!Utils.ValidarCpf(pessoa.Cpf))
                {
                    mensagens.Add("CPF inválido.");
                }
            }

            if (pessoa.Pispasep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O  campo PIS é de preenchimento obrigatório.");
            }

            if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace()
              && !(pessoa.E_mail_interno.Split('@')[1].Trim() == "prof.educacao.rj.gov.br"
              || pessoa.E_mail_interno.Split('@')[1].Trim() == "educacao.rj.gov.br"))
            {
                mensagens.Add("No campo E-MAIL OFFICE 365 serão aceitos apenas e-mails institucionais @educacao.rj.gov.br ou @prof.educacao.rj.gov.br");
            }

            if (!pessoa.E_mail.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.Email(pessoa.E_mail))
                {
                    mensagens.Add("O campo E-MAIL ALTERNATIVO está em um formato incorreto!");
                }
            }

            if (dadosIngresso)
            {
                //Verifica se é cadastro ou se o campo não possui id não foi informado
                if (cadastro || possuiId || docente.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    if (pessoa.IdFuncional == null || pessoa.IdFuncional <= 0)
                    {
                        mensagens.Add("O campo ID FUNCIONAL é de preenchimento obrigatório.");
                    }

                    if (docente.Vinculo == null || docente.Vinculo <= 0)
                    {
                        mensagens.Add("O campo VINCULO é de preenchimento obrigatório.");
                    }
                }
                else
                {
                    pessoa.IdFuncional = 0;
                    docente.Vinculo = null;
                }

                if (!docente.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    if (docente.Matricula == "00000000"
                            || docente.Matricula == "11111111"
                            || docente.Matricula == "22222222"
                            || docente.Matricula == "44444444"
                            || docente.Matricula == "55555551"
                            || docente.Matricula == "55555555"
                            || docente.Matricula == "66666666"
                            || docente.Matricula == "77777777"
                            || docente.Matricula == "88888888"
                            || docente.Matricula == "99999999")
                    {
                        mensagens.Add("Este número de matrícula é reservado.");
                    }
                }

                if (docente.Categoria.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo CARGO é de preenchimento obrigatório.");
                }

                if (docente.RegimeContratacaoId == null)
                {
                    mensagens.Add("O campo REGIME DE CONTRATAÇÃO é de preenchimento obrigatório.");
                }
                else
                {
                    //Verifica se o regime de contratação é 'Contrato Temporário'
                    bool ehContratoTemporario = docente.RegimeContratacaoId == (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario;

                    //Apenas permitir tipo 'Contrato Temporário' para docentes com candidato e concurso
                    if (ehContratoTemporario && (docente.Candidato.IsNullOrEmptyOrWhiteSpace() || docente.Concurso.IsNullOrEmptyOrWhiteSpace()))
                    {
                        mensagens.Add("O campo REGIME DE CONTRATAÇÃO apenas pode ser do tipo 'Contrato Temporário' caso o docente possua CANDIDATO e CONCURSO.");
                    }
                }
                if (docente.Dt_admissao == null)
                {
                    mensagens.Add("O  campo DATA DE ADMISSÃO é de preenchimento obrigatório.");
                }

                if (docente.Acumulacao != (int)RN.Docentes.PossuiAcumulacao.Sim && docente.Acumulacao != (int)RN.Docentes.PossuiAcumulacao.Nao)
                {
                    mensagens.Add("O campo ACUMULAÇÃO é de preenchimento obrigatório.");
                }

                if (acumulacao != null && docente.Acumulacao == (int)RN.Docentes.PossuiAcumulacao.Sim)
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
                        else
                        {
                            decimal matricula;
                            if (!decimal.TryParse(acumulacao.MatriculaOrgao, out matricula))
                            {
                                mensagens.Add("O campo MATRICULA ÓRGÃO deve conter apenas números.");
                            }

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

                if (candidato != null)
                {
                    if (candidato.Carga_Horaria < 0)
                    {
                        mensagens.Add("O campo CARGA HORÁRIA é de preenchimento obrigatório.");
                    }
                    else
                    {
                        docente.Regime_trabalho = candidato.Carga_Horaria.ToString();
                    }

                    if (candidato.Aulas_Alocadas < 0)
                    {
                        mensagens.Add("Aulas alocadas não encontradas.");
                    }
                    else
                    {
                        if (candidato.Carga_Horaria < candidato.Aulas_Alocadas)
                        {
                            mensagens.Add("A carga horária não pode ser menor que a carga alocada para o docente.");
                        }
                    }
                }

                if (grupoHabilitacaoDoc != null)
                {
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
                }

                if (lotacao != null)
                {
                    //Valida campos da lotacao                    
                    if (lotacao.Ordem <= 0)
                    {
                        mensagens.Add("O campo ORDEM é de preenchimento obrigatório.");
                    }

                    if (lotacao.Funcao.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo FUNCÃO é de preenchimento obrigatório.");
                    }

                    if (docente.Regime_trabalho.IsNullOrEmptyOrWhiteSpace() || docente.Regime_trabalho == "-1")
                    {
                        mensagens.Add("O campo CARGA HORÁRIA é de preenchimento obrigatório.");
                    }
                    else
                    {
                        int resultado;
                        if (!Int32.TryParse(docente.Regime_trabalho, out resultado))
                        {
                            mensagens.Add("O campo CARGA HORÁRIA deve ser um número inteiro.");
                        }
                    }
                    if (lotacao.DataNomeacao == DateTime.MinValue)
                    {
                        mensagens.Add("O campo DATA DA NOMEAÇÃO é de preenchimento obrigatório.");
                    }

                    if (lotacao.UnidadeEns.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo UNIDADE ENSINO é de preenchimento obrigatório.");
                    }

                    if (lotacao.Readaptado.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo READAPTADO é de preenchimento obrigatório.");
                    }
                }

                if (docente.Ano_ingresso != null && docente.Ano_ingresso > 0)
                {
                    if (docente.Ano_ingresso < 1900)
                    {
                        mensagens.Add("O ano de ingresso não pode ser inferior a 1900.");
                    }
                    else if (docente.Ano_ingresso > DateTime.Now.Year)
                    {
                        mensagens.Add("O ano de ingresso não pode ser superior ao ano atual.");
                    }
                }
            }

            if (pessoa.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O USUÁRIO RESPONSÁVEL é de preenchimento obrigatório.");
            }

            if (!cadastro)
            {
                if (googleEducation != null)
                {
                    if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace()
                     && !(googleEducation.Email.Split('@')[1].Trim() == "prof.educa.rj.gov.br"
                     || googleEducation.Email.Split('@')[1].Trim() == "educa.rj.gov.br"))
                    {
                        mensagens.Add("No campo E-MAIL GOOGLE FOR EDUCATION serão aceitos apenas e-mails @educa.rj.gov.br ou @prof.educa.rj.gov.br");
                    }
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
                        //Valida CPF existente
                        if (rnPessoa.PossuiOutroCPFPor(contexto, pessoa.Cpf, pessoa.Pessoa))
                        {
                            mensagens.Add("CPF já existente. Favor utilizar a busca através da Pessoa .");
                        }
                        else
                        {
                            //Valida Nome, mae e data de nascimento existente 
                            if (rnPessoa.PossuiOutroNomeMaeDataNascimentoPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc), pessoa.Pessoa))
                            {
                                mensagens.Add("Nome/data de nascimento/nome da mãe já existente. Favor utilizar a busca através da Pessoa .");
                            }
                        }
                    }
                    else
                    {
                        if (rnPessoa.PossuiOutroCPFPor(contexto, pessoa.Cpf, pessoa.Pessoa))
                        {
                            mensagens.Add("CPF já existente.");
                        }
                        else
                        {
                            //Valida Nome, mae e data de nascimento existente 
                            if (rnPessoa.PossuiOutroNomeMaeDataNascimentoPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc), pessoa.Pessoa))
                            {
                                mensagens.Add("Nome/data de nascimento/nome da mãe já existente.");
                            }
                        }
                    }

                    if (!cadastro)
                    {
                        //Valida matricula
                        if (this.PossuiOutraMatriculaPor(contexto, docente.Matricula, pessoa.Pessoa))
                        {
                            mensagens.Add("Número de ID/VINCULO ou MATRÍCULA já cadastrado para outro docente.");
                        }

                        if (rnVinculo.PossuiOutraMatriculaPor(contexto, docente.Matricula, pessoa.Pessoa))
                        {
                            mensagens.Add("Número de ID/VINCULO ou MATRÍCULA já cadastrado para outro servidor.");
                        }
                    }
                    else
                    {
                        if (this.PossuiMatriculaPor(contexto, docente.Matricula))
                        {
                            mensagens.Add("Número de ID/VINCULO ou MATRÍCULA já cadastrado.");
                        }
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
                        if (this.PossuiOutroVinculoPor(contexto, pessoa.Pessoa, Convert.ToInt32(docente.Vinculo), docente.Matricula) ||
                            rnVinculo.PossuiOutroVinculoPor(contexto, pessoa.Pessoa, Convert.ToInt32(docente.Vinculo), docente.Matricula))
                        {
                            mensagens.Add("Este VINCULO já se encontra cadastrado para esta pessoa.");
                        }
                    }

                    //Validar datas de lotação
                    var dt = RN.Lotacao.ConsultaDatas(docente.Matricula);
                    if (dt.Rows.Count > 0)
                    {
                        var dataNomeacao = !string.IsNullOrEmpty(dt.Rows[0]["data_nomeacao"].ToString()) ? dt.Rows[0]["data_nomeacao"] : null;
                        var dataDesativacao = !string.IsNullOrEmpty(dt.Rows[0]["DATA_DESATIVACAO"].ToString()) ? dt.Rows[0]["DATA_DESATIVACAO"] : null;

                        if (dataNomeacao != null)
                        {
                            if (docente.Dt_admissao != null)
                            {
                                if (Convert.ToDateTime(dataNomeacao).Date < Convert.ToDateTime(docente.Dt_admissao).Date)
                                {
                                    mensagens.Add("Dados Lotação: A data de admissão não pode ser maior que a menor data de nomeação cadastrada (" + Convert.ToDateTime(dataNomeacao).Date.ToShortDateString() + ").");
                                }
                            }
                            else
                            {
                                if (dataNomeacao != null)
                                {
                                    mensagens.Add("Dados Lotação: Data de Admissão deve ser preenchida, pois existe lotação.");
                                }
                            }
                        }

                        if (dataDesativacao != null)
                        {
                            if (!string.IsNullOrEmpty(dataDesativacao.ToString()))
                            {
                                if (docente.Dt_demissao != null)
                                {
                                    if (Convert.ToDateTime(dataDesativacao).Date > Convert.ToDateTime(docente.Dt_demissao).Date)
                                    {
                                        mensagens.Add("Dados Lotação: A data de demissão não pode ser menor que a maior data de desativação cadastrada (" + Convert.ToDateTime(dataDesativacao).Date.ToShortDateString() + ").");
                                    }
                                }
                            }
                        }
                    }
                    else if (lotacao != null)
                    {
                        if (docente.Dt_admissao != null)
                        {
                            if (lotacao.DataNomeacao.Date < Convert.ToDateTime(docente.Dt_admissao).Date)
                            {
                                mensagens.Add("Dados Lotação: A data de admissão não pode ser maior que a data de nomeação (" + lotacao.DataNomeacao.Date.ToShortDateString() + ").");
                            }
                        }
                        else
                        {
                            mensagens.Add("Dados Lotação: Data de Admissão deve ser preenchida.");
                        }

                        if (!string.IsNullOrEmpty(lotacao.Matricula) && lotacao.Ordem > 0 && lotacao.Pessoa > 0)
                        {
                            if (rnLotacao.PossuiLotacaoPor(lotacao.Matricula, lotacao.Pessoa, lotacao.Ordem))
                            {
                                mensagens.Add("Lotação já cadastrada para essa matrícula.");
                            }
                        }
                    }

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

        public ValidacaoDados ValidaVoluntario(LyPessoa pessoa, LyDocente docente, string zonaResidencial, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.VinculoLy rnVinculo = new VinculoLy();
            DataContext contexto = null;
            long resultado;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docente == null || pessoa == null)
            {
                return validacaoDados;
            }

            if (pessoa.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUÁRIO RESPONSÁVEL é de preenchimento obrigatório.");
            }

            if (pessoa.Nome_compl.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME é de preenchimento obrigatório.");
            }
            else
            {
                //Verificar nome valido
                int n = 0;
                for (n = 0; n <= 9; n++)
                {
                    if (pessoa.Nome_compl.IndexOf(n.ToString()) > 0)
                    {
                        mensagens.Add("Nome Completo: Não se pode ter números no nome.(" + n.ToString() + ").");
                    }
                }

                string[] vetorNome = pessoa.Nome_compl.Split(' ');

                if (vetorNome.Length == 1)
                {
                    mensagens.Add("Nome Completo: O Nome não pode ser formado por apenas uma palavra.");
                }

                if (Utils.VerificaTriploCaracter(pessoa.Nome_compl))
                {
                    mensagens.Add("Nome Completo: Não se pode ter três letras iguais consecutivas no nome.");
                }
            }

            if (pessoa.Dt_nasc == DateTime.MinValue || pessoa.Dt_nasc == null)
            {
                mensagens.Add("O campo DATA DE NASCIMENTO é de preenchimento obrigatório.");
            }

            if (pessoa.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo SEXO é de preenchimento obrigatório.");
            }

            if (pessoa.Etnia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ETNIA é de preenchimento obrigatório.");
            }

            if (pessoa.NecessidadeEspecialId == null)
            {
                mensagens.Add("O campo NECESSIDADE ESPECIAL é de preenchimento obrigatório.");
            }

            if (pessoa.Est_civil.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ESTADO CIVIL é de preenchimento obrigatório.");
            }

            if (pessoa.Pais_nasc.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo PAÍS DE NASCIMENTO é de preenchimento obrigatório.");
            }
            if (pessoa.Nacionalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NACIONALIDADE é de preenchimento obrigatório.");
            }
            else
            {
                if (pessoa.Pais_nasc == "1" && pessoa.Nacionalidade != "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser Brasil com a NACIONALIDADE diferente de brasileira.");
                }

                if (pessoa.Pais_nasc != "1" && pessoa.Nacionalidade == "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser diferente de Brasil e a NACIONALIDADE ser brasileira.");
                }

                if (pessoa.Nacionalidade == "BRASILEIRA")
                {
                    if (string.IsNullOrEmpty(pessoa.Municipio_nasc) || pessoa.Municipio_nasc == "<Nenhum>")
                    {
                        mensagens.Add("O campo NATURALIDADE é de preenchimento obrigatório.");
                    }
                }
            }

            if (pessoa.Municipio_nasc.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NATURALIDADE é de preenchimento obrigatório.");
            }

            if (pessoa.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CEP é de preenchimento obrigatório.");
            }
            if (pessoa.End_municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MUNICIPIO é de preenchimento obrigatório.");
            }

            if (pessoa.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ENDEREÇO é de preenchimento obrigatório.");
            }

            if (pessoa.End_num.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NÚMERO DO ENDEREÇO é de preenchimento obrigatório.");
            }

            if (pessoa.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo BAIRRO é de preenchimento obrigatório.");
            }

            if (zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O  campo LOCALIZAÇÃO é de preenchimento obrigatório.");
            }

            if (!pessoa.Fone.IsNullOrEmptyOrWhiteSpace())
            {
                if (long.TryParse(pessoa.Fone.RetirarMascaraTelefone(), out resultado))
                {
                    if (pessoa.Fone.RetirarMascaraTelefone().Length == 10)
                    {
                        pessoa.Fone = string.Format("{0:(00)0000-0000}", resultado);
                    }
                    if (pessoa.Fone.RetirarMascaraTelefone().Length == 11)
                    {
                        pessoa.Fone = string.Format("{0:(00)00000-0000}", resultado);
                    }
                }
                else
                {
                    mensagens.Add("Telefone inválido.");
                }
            }

            if (!pessoa.Celular.IsNullOrEmptyOrWhiteSpace())
            {
                if (long.TryParse(pessoa.Celular.RetirarMascaraTelefone(), out resultado))
                {
                    if (pessoa.Celular.RetirarMascaraTelefone().Length == 10)
                    {
                        pessoa.Celular = string.Format("{0:(00)0000-0000}", resultado);
                    }
                    if (pessoa.Celular.RetirarMascaraTelefone().Length == 11)
                    {
                        pessoa.Celular = string.Format("{0:(00)00000-0000}", resultado);
                    }
                }
                else
                {
                    mensagens.Add("Celular inválido.");
                }
            }

            if (!pessoa.E_mail.IsNullOrEmptyOrWhiteSpace() && !Validacao.Email(pessoa.E_mail))
            {
                mensagens.Add("O campo E-MAIL está em um formato incorreto.");
            }

            if (pessoa.Rg_tipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo TIPO DE DOCUMENTO é de preenchimento obrigatório.");
            }
            else
            {
                #region Validações dos Campos de Documento
                bool documentoValido, iniciouMensagem, maisDeUmCampo;
                documentoValido = true;
                iniciouMensagem = maisDeUmCampo = false;
                System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
                System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
                mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");

                if (pessoa.Rg_tipo == "RG")
                {
                    if (Convert.ToString(pessoa.Rg_num).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        iniciouMensagem = true;
                        camposDocumento.Append("Número ");
                    }

                    if (Convert.ToString(pessoa.Rg_uf).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Estado ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Estado ");
                        }
                    }

                    if (Convert.ToString(pessoa.Rg_emissor).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }

                    if (pessoa.Rg_dtexp == null)
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Data de Expedição ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Data de Expedição ");
                        }
                    }
                }
                else
                {
                    if (Convert.ToString(pessoa.Rg_num).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        iniciouMensagem = true;
                        camposDocumento.Append("Número ");
                    }
                    if (Convert.ToString(pessoa.Rg_uf).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }
                }

                if (!documentoValido)
                {
                    if (maisDeUmCampo)
                    {
                        mensagemDocumento.Append("<br>Campos Necessários: ");
                    }
                    else
                    {
                        mensagemDocumento.Append("<br>Campo Necessário: ");
                    }

                    mensagemDocumento.Append(camposDocumento);
                    mensagens.Add(mensagemDocumento.ToString());
                }

                #endregion
            }

            if (pessoa.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CPF é de preenchimento obrigatório.");
            }
            else
            {
                if (!Utils.ValidarCpf(pessoa.Cpf))
                {
                    mensagens.Add("CPF inválido.");
                }
            }

            if (docente.Categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo FUNÇÃO é de preenchimento obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida CPF existente
                    var cpf = pessoa.Cpf.RetirarMascaraCPF();

                    //Caso não existe pessoa tentar Buscar
                    if (pessoa.Pessoa == 0)
                    {
                        decimal pessoaCPF = rnPessoa.ObtemPessoaPor(contexto, cpf);
                        if (pessoaCPF > 0)
                        {
                            pessoa.Pessoa = pessoaCPF;
                        }
                    }

                    if (pessoa.Pessoa != 0 && cadastro)
                    {
                        //Verifica se é um docente
                        if (this.EhDocentePor(contexto, pessoa.Pessoa))
                        {
                            if (this.EhVoluntarioPor(contexto, pessoa.Pessoa))
                            {
                                mensagens.Add("Número de CPF já cadastrado como voluntário.");
                            }
                            else
                            {
                                //Verifica se é docente com afastamento definitivo
                                if (this.PossuiLicencaDefinitivaPor(contexto, pessoa.Pessoa))
                                {
                                    mensagens.Add("O Voluntário não pode ser um docente com afastamento definitivo.");
                                }

                                //Verifica se é um docente ativo                        
                                if (this.EhDocenteAtivoPor(contexto, pessoa.Pessoa))
                                {
                                    mensagens.Add("O Voluntário não pode ser um docente ativo (sem data de demissão).");
                                }
                            }

                            //Verifica se tem lotação ativa                       
                            if (this.PossuiLotacaoAtivaPor(contexto, pessoa.Pessoa))
                            {
                                mensagens.Add("O Voluntário não pode ter uma lotação ativa.");
                            }
                        }

                        //Verifica se a pessoa é um servidor
                        if (rnVinculo.PossuiVinculoServidorPor(contexto, pessoa.Pessoa))
                        {
                            mensagens.Add("O Voluntário não pode ser servidor.");
                        }
                    }

                    if (cadastro)
                    {
                        docente.Matricula = this.ObtemMatriculaFicticiaPor(contexto);

                        if (docente.Matricula.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Não foi encontrada uma matricula valida para o voluntários.");
                        }

                        if (this.EhVoluntarioPor(contexto, pessoa.Cpf))
                        {
                            mensagens.Add("Número de CPF já cadastrado como voluntário.");
                        }
                    }

                    //Valida matricula
                    if (this.PossuiOutraMatriculaPor(contexto, docente.Matricula, pessoa.Pessoa))
                    {
                        mensagens.Add("Número de matrícula já cadastrado.");
                    }

                    if (rnVinculo.PossuiOutraMatriculaPor(contexto, docente.Matricula, pessoa.Pessoa))
                    {
                        mensagens.Add("Número de matrícula já cadastrado para outro servidor.");
                    }
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

        public void InsereVoluntario(LyPessoa pessoa, LyDocente docente, string zonaResidencial)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Pessoa rnPessoa = new Pessoa();
            GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new GrupoHabilitacaoDoc();
            LyGrupoHabilitacaoDoc grupoHabilitacaoDoc = new LyGrupoHabilitacaoDoc();
            bool cadastroPessoa = pessoa.Pessoa == 0;
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            try
            {
                if (cadastroPessoa)
                {
                    rnPessoa.Insere(contexto, pessoa);
                }
                else
                {
                    rnPessoa.AtualizaPessoaVoluntario(contexto, pessoa);
                }

                docente.Pessoa = pessoa.Pessoa;
                docente.Usuario = pessoa.UsuarioId;
                this.Insere(contexto, docente);

                if (cadastroPessoa || !rnFlPessoa.ExistePor(contexto, pessoa.Pessoa))
                {
                    //Caso a pessoa não exista ou o flpessoa não exista Insere
                    rnFlPessoa.InsereZonaResidencial(contexto, pessoa.Pessoa, zonaResidencial);
                }
                else
                {
                    //Caso contrario, Atualiza Zona Residencial (FL_PESSOA - FL_FIELD_01)
                    rnFlPessoa.AtualizaZonaResidencial(contexto, pessoa.Pessoa, zonaResidencial);
                }

                this.InutilizarMatriculaFicticia(contexto, docente.Matricula, pessoa.UsuarioId);

                if (docente.Categoria == "REG MAIS EDUCACAO")
                {
                    //Cria Habilitacao para Mais Educacao
                    grupoHabilitacaoDoc.NumFunc = this.ObtemNumFuncPor(contexto, docente.Matricula);
                    grupoHabilitacaoDoc.Agrupamento = "MaisEdu";
                    grupoHabilitacaoDoc.Provisorio = "N";
                    grupoHabilitacaoDoc.DtLimite = null;
                    grupoHabilitacaoDoc.StampAtualizacao = DateTime.Now;
                    grupoHabilitacaoDoc.Campo01 = "S";
                    grupoHabilitacaoDoc.Campo02 = "N";
                    grupoHabilitacaoDoc.AgrupamentoIngresso = "N";

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

        public int ObtemQuantidadeMatriculaPor(string cpf)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            int nrMatriculas = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(D.MATRICULA) 
                                FROM   LY_DOCENTE D (NOLOCK) 
		                                INNER JOIN LY_PESSOA P (NOLOCK)
				                                ON D.PESSOA = P.PESSOA
                                        INNER JOIN LY_LOTACAO L (NOLOCK) 
                                                ON D.PESSOA = L.PESSOA 
                                                    AND D.MATRICULA = L.MATRICULA 
                                WHERE  P.CPF = @CPF
                                        AND L.DATA_NOMEACAO <= CONVERT(DATE, GETDATE()) 
                                        AND ( L.DATA_DESATIVACAO IS NULL 
                                                OR CONVERT(DATE, L.DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()) )
                                "
                };
                contextQuery.Parameters.Add("@CPF", cpf);

                nrMatriculas = contexto.GetReturnValue(contextQuery) == null ? 0 : contexto.GetReturnValue<int>(contextQuery);

                return nrMatriculas;
            }

            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public LyDocente Carregar(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            LyDocente docente = new LyDocente();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  *
                        FROM    LY_DOCENTE WITH ( NOLOCK )
                        WHERE   MATRICULA = @MATRICULA  "
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                docente = ctx.TryToBindEntity<LyDocente>(contextQuery);
                return docente;
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

        public DTOs.FichaImplantacaoDocente ObtemDadosFichaDocentePor(decimal numFunc)
        {
            DTOs.FichaImplantacaoDocente dadosDocente = new DTOs.FichaImplantacaoDocente();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT D.NUM_FUNC, 
                                                        D.MATRICULA, 
                                                        P.NOME_COMPL, 
                                                        L.SETOR, 
                                                        M2.NOME           AS MUNICIPIO_LOTACAO, 
                                                        CASE 
                                                          WHEN AC.MATRICULAORGAO IS NULL THEN 'NÃO' 
                                                          ELSE 'SIM' 
                                                        END               AS ACUMULACAO, 
                                                        AC.MATRICULAORGAO AS MATRICULA_ACUMULACAO, 
                                                        AC.NUMEROPROCESSO AS NUMEROPROCESSO_ACUMULACAO, 
                                                        AC.ORGAO          AS ORGAO_ACUMULACAO, 
                                                        GH.DESCRICAO      AS INGRESSO, 
                                                        D.DT_ADMISSAO, 
                                                        P.CPF, 
                                                        P.DT_NASC, 
                                                        P.SEXO, 
                                                        P.ETNIA, 
                                                        P.EST_CIVIL, 
                                                        P.NACIONALIDADE, 
                                                        M3.NOME           AS NATURALIDADE, 
                                                        P.NOME_MAE, 
                                                        P.NOME_PAI, 
                                                        P.ENDERECO, 
                                                        P.END_NUM, 
                                                        P.END_COMPL, 
                                                        P.BAIRRO, 
                                                        M.UF_SIGLA        AS ESTADO_ENDERECO, 
                                                        P.CEP, 
                                                        M.NOME            AS MUNICIPIO, 
                                                        P.RG_NUM          AS IDENTIDADE, 
                                                        P.RG_EMISSOR      AS IDENTIDADE_ORGAO, 
                                                        P.RG_DTEXP        AS IDENTIDADE_DATAEXP, 
                                                        P.RG_UF           AS IDENTIDADE_UF, 
                                                        P.RG_TIPO, 
                                                        P.TELEITOR_NUM    AS TITULO_NUMERO, 
                                                        P.TELEITOR_SECAO  AS TITULO_SESSAO, 
                                                        P.TELEITOR_ZONA   AS TITULO_ZONA, 
                                                        P.CR_NUM          AS CERTIFICADO, 
                                                        P.CR_CAT          AS CERTIFICADO_CATEGORIA, 
                                                        P.CR_SERIE        AS CERTIFICADO_SERIE, 
                                                        P.PISPASEP, 
                                                        P.CPROF_NUM       AS CTPS, 
                                                        P.CPROF_SERIE     AS CTPS_SERIE,
                                                        D.ANO_INGRESSO AS ANO_INGRESSO ,
                                                        P.IDFUNCIONAL ,
                                                        D.VINCULO
                                        FROM   LY_DOCENTE D WITH ( NOLOCK ) 
                                               INNER JOIN LY_PESSOA P WITH ( NOLOCK ) 
                                                       ON D.PESSOA = P.PESSOA 
                                               INNER JOIN MUNICIPIO M WITH ( NOLOCK ) 
                                                       ON M.CODIGO = CONVERT(INT, P.END_MUNICIPIO) 
                                               INNER JOIN LY_LOTACAO L WITH ( NOLOCK ) 
                                                       ON L.MATRICULA = D.MATRICULA 
                                               INNER JOIN LY_UNIDADE_ENSINO UE WITH ( NOLOCK ) 
                                                       ON UE.UNIDADE_ENS = L.UNIDADE_ENS 
                                               INNER JOIN MUNICIPIO M2 WITH ( NOLOCK ) 
                                                       ON M2.CODIGO = UE.MUNICIPIO 
                                               INNER JOIN LY_GRUPO_HABILITACAO_DOC GHD WITH ( NOLOCK ) 
                                                       ON GHD.NUM_FUNC = D.NUM_FUNC 
                                                          AND GHD.AGRUPAMENTO_INGRESSO = 'S' 
                                                          AND GHD.PROVISORIO = 'N'
                                               INNER JOIN LY_GRUPO_HABILITACAO_DISC DIS WITH ( NOLOCK ) 
                                                       ON GHD.AGRUPAMENTO = DIS.AGRUPAMENTO 
                                               INNER JOIN DBO.LY_GRUPO_HABILITACAO GH WITH ( NOLOCK ) 
                                                       ON GHD.AGRUPAMENTO = GH.AGRUPAMENTO 
                                               LEFT JOIN RECURSOSHUMANOS.ACUMULACAO AC WITH ( NOLOCK ) 
                                                      ON AC.DOCENTEID = D.NUM_FUNC 
                                               INNER JOIN MUNICIPIO M3 WITH ( NOLOCK ) 
                                                       ON M3.CODIGO = CONVERT(INT, P.MUNICIPIO_NASC) 
                                        WHERE  D.NUM_FUNC = @NUM_FUNC
                                               AND L.ORDEM = 1 ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosDocente.DocenteId = Convert.ToDecimal(reader["NUM_FUNC"]);
                    dadosDocente.Matricula = Convert.ToString(reader["MATRICULA"]);
                    dadosDocente.Cpf = Convert.ToString(reader["CPF"]);
                    dadosDocente.NomeCompleto = Convert.ToString(reader["NOME_COMPL"]);
                    dadosDocente.UnidadeAdministrativa = Convert.ToString(reader["SETOR"]);
                    dadosDocente.MunicipioLotacao = Convert.ToString(reader["MUNICIPIO_LOTACAO"]);
                    if (reader["DT_ADMISSAO"] != DBNull.Value)
                    {
                        dadosDocente.DataAdmissao = Convert.ToDateTime(reader["DT_ADMISSAO"]);
                    }
                    if (reader["MATRICULA_ACUMULACAO"] != DBNull.Value)
                    {
                        dadosDocente.MatriculaAcumulucao = Convert.ToString(reader["MATRICULA_ACUMULACAO"]);
                        dadosDocente.OrgaoAcumulucao = Convert.ToString(reader["ORGAO_ACUMULACAO"]);
                        dadosDocente.ProcessoAcumulucao = Convert.ToString(reader["NUMEROPROCESSO_ACUMULACAO"]);
                    }
                    dadosDocente.DisciplinaIngresso = Convert.ToString(reader["INGRESSO"]);
                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dadosDocente.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    dadosDocente.Sexo = Convert.ToString(reader["SEXO"]);
                    dadosDocente.Raca = Convert.ToString(reader["ETNIA"]);
                    dadosDocente.EstadoCivil = Convert.ToString(reader["EST_CIVIL"]);
                    dadosDocente.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);
                    dadosDocente.Naturalidade = Convert.ToString(reader["NATURALIDADE"]);
                    dadosDocente.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dadosDocente.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dadosDocente.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosDocente.Numero = Convert.ToString(reader["END_NUM"]);
                    dadosDocente.Complemento = Convert.ToString(reader["END_COMPL"]);
                    dadosDocente.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosDocente.Cep = Convert.ToString(reader["CEP"]);
                    dadosDocente.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosDocente.Estado = Convert.ToString(reader["ESTADO_ENDERECO"]);
                    dadosDocente.Identidade = Convert.ToString(reader["IDENTIDADE"]);
                    dadosDocente.OrgaoIdentidade = Convert.ToString(reader["IDENTIDADE_ORGAO"]);
                    if (reader["IDENTIDADE_DATAEXP"] != DBNull.Value)
                    {
                        dadosDocente.DataExpedicao = Convert.ToDateTime(reader["IDENTIDADE_DATAEXP"]);
                    }
                    dadosDocente.UFIdentidade = Convert.ToString(reader["IDENTIDADE_UF"]);
                    dadosDocente.TituloEleitor = Convert.ToString(reader["TITULO_NUMERO"]);
                    dadosDocente.ZonaTitulo = Convert.ToString(reader["TITULO_ZONA"]);
                    dadosDocente.SecaoTitulo = Convert.ToString(reader["TITULO_SESSAO"]);
                    dadosDocente.Certificado = Convert.ToString(reader["CERTIFICADO"]);
                    dadosDocente.CategoriaCertificado = Convert.ToString(reader["CERTIFICADO_CATEGORIA"]);
                    dadosDocente.SerieCertificado = Convert.ToString(reader["CERTIFICADO_SERIE"]);
                    dadosDocente.Pis = Convert.ToString(reader["PISPASEP"]);
                    dadosDocente.Ctps = Convert.ToString(reader["CTPS"]);
                    dadosDocente.SerieCtps = Convert.ToString(reader["CTPS_SERIE"]);
                    dadosDocente.AnoConcurso = Convert.ToString(reader["ANO_INGRESSO"]);

                    if (reader["IDFUNCIONAL"] != DBNull.Value)
                    {
                        dadosDocente.IdFuncional = Convert.ToInt32(reader["IDFUNCIONAL"]);
                    }
                    if (reader["VINCULO"] != DBNull.Value)
                    {
                        dadosDocente.Vinculo = Convert.ToInt32(reader["VINCULO"]);
                    }

                }

                return dadosDocente;
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

        public void AtualizaCargaHorariaPor(DataContext ctx, string strConcurso, string strCandidato, string strCargaHoraria)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_DOCENTE
                                        SET REGIME_TRABALHO = @CARGA_HORARIA
                                        WHERE CONCURSO = @CONCURSO 
                                        AND CANDIDATO = @CANDIDATO ";

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

        public decimal? ObtemRegimeTrabalhoPor(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            decimal? retorno = null;

            try
            {
                contextQuery.Command = @" SELECT REGIME_TRABALHO
                                            FROM LY_DOCENTE (NOLOCK)
                                            WHERE MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["REGIME_TRABALHO"] != DBNull.Value)
                    {
                        retorno = Convert.ToDecimal(reader["REGIME_TRABALHO"]);
                    }
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

        public bool EhUsadoPor(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_DOCENTE 
                                        WHERE MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
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
                ctx.Dispose();
            }
        }

        public DTOs.DadosDocente ObtemDadosDocentePor(decimal numFunc)
        {
            DTOs.DadosDocente dadosDocente = new DTOs.DadosDocente();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT D.NUM_FUNC,                                                         
                                                        D.MATRICULA, 
                                                        P.PESSOA,
                                                        P.NOME_COMPL, 
                                                        D.ACUMULACAO           AS ACUMULACAO, 
                                                        AC.MATRICULAORGAO AS MATRICULA_ACUMULACAO, 
                                                        AC.NUMEROPROCESSO AS NUMEROPROCESSO_ACUMULACAO, 
                                                        AC.ORGAO          AS ORGAO_ACUMULACAO, 
                                                        D.DT_ADMISSAO, 
                                                        D.DT_DEMISSAO, 
                                                        D.CONCURSO,
                                                        D.CANDIDATO,
                                                        D.VOLUNTARIO,
                                                        D.REGIMECONTRATACAOID,
                                                        D.REGIME_TRABALHO,
                                                        P.CPF, 
                                                        P.DT_NASC, 
                                                        P.SEXO, 
                                                        P.ETNIA, 
                                                        P.E_MAIL_INTERNO AS EMAIL,
                                                        P.EST_CIVIL, 
                                                        P.NACIONALIDADE, 
                                                        M3.NOME           AS NATURALIDADE, 
                                                        P.NOME_MAE, 
                                                        P.NOME_PAI, 
                                                        P.ENDERECO, 
                                                        P.END_NUM, 
                                                        P.END_COMPL, 
                                                        P.BAIRRO, 
                                                        M.UF_SIGLA        AS ESTADO_ENDERECO, 
                                                        P.CEP, 
                                                        M.NOME            AS MUNICIPIO, 
                                                        P.FONE AS TELEFONE,
                                                        p.CELULAR,
                                                        P.RG_NUM          AS IDENTIDADE, 
                                                        P.RG_EMISSOR      AS IDENTIDADE_ORGAO, 
                                                        P.RG_DTEXP        AS IDENTIDADE_DATAEXP, 
                                                        P.RG_UF           AS IDENTIDADE_UF, 
                                                        P.RG_TIPO         AS IDENTIDADE_TIPO, 
                                                        P.TELEITOR_NUM    AS TITULO_NUMERO, 
                                                        P.TELEITOR_SECAO  AS TITULO_SESSAO, 
                                                        P.TELEITOR_ZONA   AS TITULO_ZONA, 
                                                        P.CR_NUM          AS CERTIFICADO, 
                                                        P.CR_CAT          AS CERTIFICADO_CATEGORIA, 
                                                        P.CR_SERIE        AS CERTIFICADO_SERIE, 
                                                        P.PISPASEP, 
                                                        P.CPROF_NUM       AS CTPS, 
                                                        P.CPROF_SERIE     AS CTPS_SERIE,
                                                        P.CPROF_DTEXP     AS CTPS_DATA,
                                                        P.CPROF_UF        AS CTPS_UF,
                                                        D.ANO_INGRESSO AS ANO_INGRESSO  ,
														D.CATEGORIA,
														FL.FL_FIELD_01 AS ZONARESIDENCIAL,
                                                        DB.BANCO,
                                                        DB.CONTABANCO,
                                                        DB.AGENCIA,
                                                        D.SENHA_DOL,
                                                        D.SENHA_ALTERADA,
														D.VINCULO,
														P.IDFUNCIONAL
                                         FROM   LY_DOCENTE D WITH ( NOLOCK ) 
                                               INNER JOIN LY_PESSOA P WITH ( NOLOCK ) 
                                                       ON D.PESSOA = P.PESSOA 
                                               LEFT JOIN LY_FL_PESSOA FL WITH ( NOLOCK ) 
                                                       ON FL.PESSOA = P.PESSOA 
											   LEFT JOIN RecursosHumanos.PESSOADADOSBANCARIOS DB
													   ON DB.PESSOAID = P.PESSOA 
                                                       AND DB.ATIVO = 1	
                                               LEFT JOIN MUNICIPIO M WITH ( NOLOCK ) 
                                                       ON M.CODIGO = P.END_MUNICIPIO
                                               LEFT JOIN RECURSOSHUMANOS.ACUMULACAO AC WITH ( NOLOCK ) 
                                                      ON AC.DOCENTEID = D.NUM_FUNC 
                                               LEFT JOIN MUNICIPIO M3 WITH ( NOLOCK ) 
                                                       ON M3.CODIGO = P.MUNICIPIO_NASC
                                        WHERE  D.NUM_FUNC = @NUM_FUNC
                                               ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosDocente.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    dadosDocente.DocenteId = Convert.ToDecimal(reader["NUM_FUNC"]);
                    dadosDocente.Matricula = Convert.ToString(reader["MATRICULA"]);

                    dadosDocente.Cpf = Convert.ToString(reader["CPF"]);
                    dadosDocente.NomeCompleto = Convert.ToString(reader["NOME_COMPL"]);
                    dadosDocente.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosDocente.Celular = Convert.ToString(reader["CELULAR"]);
                    if (reader["DT_ADMISSAO"] != DBNull.Value)
                    {
                        dadosDocente.DataAdmissao = Convert.ToDateTime(reader["DT_ADMISSAO"]);
                    }
                    if (reader["DT_DEMISSAO"] != DBNull.Value)
                    {
                        dadosDocente.DataDemissao = Convert.ToDateTime(reader["DT_DEMISSAO"]);
                    }
                    if (reader["MATRICULA_ACUMULACAO"] != DBNull.Value)
                    {
                        dadosDocente.Acumulacao = Convert.ToInt32(reader["ACUMULACAO"]);
                        dadosDocente.MatriculaAcumulacao = Convert.ToString(reader["MATRICULA_ACUMULACAO"]);
                        dadosDocente.OrgaoAcumulacao = Convert.ToString(reader["ORGAO_ACUMULACAO"]);
                        dadosDocente.ProcessoAcumulacao = Convert.ToString(reader["NUMEROPROCESSO_ACUMULACAO"]);
                    }

                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dadosDocente.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    dadosDocente.Sexo = Convert.ToString(reader["SEXO"]);
                    dadosDocente.Etnia = Convert.ToString(reader["ETNIA"]);
                    dadosDocente.Email = Convert.ToString(reader["EMAIL"]);
                    dadosDocente.EstadoCivil = Convert.ToString(reader["EST_CIVIL"]);
                    dadosDocente.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);
                    dadosDocente.Naturalidade = Convert.ToString(reader["NATURALIDADE"]);
                    dadosDocente.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dadosDocente.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dadosDocente.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosDocente.Numero = Convert.ToString(reader["END_NUM"]);
                    dadosDocente.Complemento = Convert.ToString(reader["END_COMPL"]);
                    dadosDocente.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosDocente.Cep = Convert.ToString(reader["CEP"]);
                    dadosDocente.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosDocente.Estado = Convert.ToString(reader["ESTADO_ENDERECO"]);
                    dadosDocente.Identidade = Convert.ToString(reader["IDENTIDADE"]);
                    dadosDocente.OrgaoIdentidade = Convert.ToString(reader["IDENTIDADE_ORGAO"]);
                    if (reader["IDENTIDADE_DATAEXP"] != DBNull.Value)
                    {
                        dadosDocente.DataExpedicao = Convert.ToDateTime(reader["IDENTIDADE_DATAEXP"]);
                    }
                    dadosDocente.UFIdentidade = Convert.ToString(reader["IDENTIDADE_UF"]);
                    dadosDocente.TipoIdentidade = Convert.ToString(reader["IDENTIDADE_TIPO"]);
                    dadosDocente.TituloEleitor = Convert.ToString(reader["TITULO_NUMERO"]);
                    dadosDocente.ZonaTitulo = Convert.ToString(reader["TITULO_ZONA"]);
                    dadosDocente.SecaoTitulo = Convert.ToString(reader["TITULO_SESSAO"]);
                    dadosDocente.Certificado = Convert.ToString(reader["CERTIFICADO"]);
                    dadosDocente.CategoriaCertificado = Convert.ToString(reader["CERTIFICADO_CATEGORIA"]);
                    dadosDocente.SerieCertificado = Convert.ToString(reader["CERTIFICADO_SERIE"]);
                    dadosDocente.Pis = Convert.ToString(reader["PISPASEP"]);
                    dadosDocente.Ctps = Convert.ToString(reader["CTPS"]);
                    dadosDocente.CtpsSerie = Convert.ToString(reader["CTPS_SERIE"]);
                    if (reader["CTPS_DATA"] != DBNull.Value)
                    {
                        dadosDocente.CtpsData = Convert.ToDateTime(reader["CTPS_DATA"]);
                    }
                    dadosDocente.CtpsUF = Convert.ToString(reader["CTPS_UF"]);

                    dadosDocente.AnoConcurso = Convert.ToString(reader["ANO_INGRESSO"]);
                    dadosDocente.Categoria = Convert.ToString(reader["CATEGORIA"]);
                    dadosDocente.ZonaResidencial = Convert.ToString(reader["ZONARESIDENCIAL"]);

                    dadosDocente.Concurso = Convert.ToString(reader["CONCURSO"]);
                    dadosDocente.Candidato = Convert.ToString(reader["CANDIDATO"]);
                    dadosDocente.Voluntario = Convert.ToString(reader["VOLUNTARIO"]);
                    if (reader["REGIMECONTRATACAOID"] != DBNull.Value)
                    {
                        dadosDocente.RegimeContratacaoId = Convert.ToInt32(reader["REGIMECONTRATACAOID"]);
                    }
                    dadosDocente.RegimeTrabalho = Convert.ToString(reader["REGIME_TRABALHO"]);
                    if (reader["BANCO"] != DBNull.Value)
                    {
                        dadosDocente.Banco = Convert.ToInt32(reader["BANCO"]);
                    }
                    dadosDocente.Agencia = Convert.ToString(reader["AGENCIA"]);
                    dadosDocente.Conta = Convert.ToString(reader["CONTABANCO"]);
                    dadosDocente.SenhaAlterada = Convert.ToString(reader["SENHA_ALTERADA"]);
                    dadosDocente.SenhaDol = Convert.ToString(reader["SENHA_DOL"]);

                    if (reader["IDFUNCIONAL"] != DBNull.Value)
                    {
                        dadosDocente.IdFuncional = Convert.ToInt32(reader["IDFUNCIONAL"]);
                    }
                    else
                    {
                        dadosDocente.IdFuncional = null;
                    }

                    if (reader["VINCULO"] != DBNull.Value)
                    {
                        dadosDocente.Vinculo = Convert.ToInt32(reader["VINCULO"]);
                    }
                    else
                    {
                        dadosDocente.Vinculo = null;
                    }
                }

                return dadosDocente;
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

        public bool PossuiMatriculaPor(DataContext ctx, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_DOCENTE 
                                        WHERE MATRICULA = @MATRICULA
	                                        ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool EhMatriculaDocentePor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                                FROM   LY_DOCENTE 
                                                WHERE  MATRICULA = @MATRICULA 
                                                       ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutroVinculoPor(DataContext ctx, decimal pessoa, int vinculo, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_DOCENTE (NOLOCK)
                                        WHERE PESSOA = @PESSOA
	                                        AND VINCULO = @VINCULO 
                                                AND MATRICULA <> @MATRICULA";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@VINCULO", vinculo);
            contextQuery.Parameters.Add("@MATRICULA", matricula);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiOutroVinculoPor(DataContext ctx, decimal pessoa, int vinculo, decimal numFunc)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_DOCENTE (NOLOCK)
                                        WHERE PESSOA = @PESSOA
	                                        AND VINCULO = @VINCULO 
                                            AND NUM_FUNC <> @NUM_FUNC ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@VINCULO", vinculo);
            contextQuery.Parameters.Add("@NUM_FUNC", numFunc);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public string ObtemMatriculaPor(string idVinculo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT MATRICULA
                                            FROM LY_DOCENTE D (NOLOCK)
                                            INNER JOIN LY_PESSOA P ON D.PESSOA = P.PESSOA
                                            WHERE IDFUNCIONAL = @IDFUNCIONAL
                                            AND VINCULO = @VINCULO";

                contextQuery.Parameters.Add("@IDFUNCIONAL", idVinculo.Split('/')[0]);
                contextQuery.Parameters.Add("@VINCULO", idVinculo.Split('/')[1]);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public DataTable ObtemDadosDocenteMigracaoPor(int ano, string idVinculo)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"REL_CH_SERV_ANO_RDL";
                contextQuery.Parameters.Add("@IDVINCULO", idVinculo);
                contextQuery.Parameters.Add("@ANO", ano);

                lista = contexto.GetDataTable(contextQuery);
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
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }

        public bool EhMatriculaDocentePor(string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_DOCENTE 
                                        WHERE MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
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
                ctx.Dispose();
            }
        }

    }
}