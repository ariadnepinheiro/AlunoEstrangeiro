using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class Concurso : RNBase
    {
        //private static readonly string queryConsultaFuncaoConcurso = @"SELECT [CONCURSO],[FUNCAOID] FROM [LYCEUM].[dbo].[LY_CONCURSO_DOCENTE] WHERE [CONCURSO] = @CONCURSO";
        private static readonly string queryConsultaFuncaoConcurso = @"SELECT F.FUNCAOID, F.DESCRICAO FROM FUNCAO F INNER JOIN LY_CONCURSO_DOCENTE C ON (F.FUNCAOID = C.FUNCAOID) WHERE CONCURSO = @CONCURSO";

        public static string QueryConsultaFuncaoConcurso
        {
            get { return Concurso.queryConsultaFuncaoConcurso; }
        }


        private static readonly string queryConsultaConcurso = @"SELECT [CONCURSO],[DESCRICAO],[ANO],[SEMESTRE],[STATUS], TIPO,[DT_INICIO],[DT_FIM],[DT_INI_INSCR]
      ,[DT_FIM_INSCR],[DT_INI_CONSULTA],[DT_FIM_CONSULTA],[DT_INI_CONVOCACAO],[DT_FIM_CONVOCACAO],[DT_INI_INGRESSO],[DT_FIM_INGRESSO],[NR_DIGITOS_CODIGO],[QT_DIAS_CONVOCACAO]
      ,[QT_DIAS_REPROVACAO],[NR_RESOLUCAO],[PONTUACAO_MINIMA],[PORTARIA],[DT_PUBLICACAO_DO],[RESTRICAOANOS],[OBSERVACAO],[FUNCAOID], INDIGENA
       FROM [LYCEUM].[dbo].[LY_CONCURSO_DOCENTE]
       WHERE CONCURSO = @CONCURSO ;";

        public static string QueryConsultaConcurso
        {
            get { return Concurso.queryConsultaConcurso; }
        }



        private readonly string queryDisciplinasHabilitadas = @"select agrupamento, descricao from ly_grupo_habilitacao where ativo = 'S' order by descricao";

        public string QueryDisciplinasHabilitadas
        {
            get { return queryDisciplinasHabilitadas; }
        }

        private readonly string queryDisciplinasHabilitadasFiltradas = @"select agrupamento, descricao from ly_grupo_habilitacao where ATIVO = 'S' and AGRUPAMENTO not in (select agrupamento from LY_CONCURSO_DOC_HABILITACAO where CONCURSO=@CONCURSO and REGIONALID=@REGIONALID and MUNICIPIO_PROC=@MUNICIPIO ) order by descricao";

		public string QueryDisciplinasHabilitadasFiltradas
		{
			get { return queryDisciplinasHabilitadasFiltradas; }
		}

        public static RetValue Incluir(Ly_concurso_docente dtConcurso)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (dtConcurso != null)
                {
                    if (dtConcurso.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtConcurso);

                        Ly_concurso_docente.Row.Insert(connection, dtConcurso.Rows[0].Concurso, dtConcurso.Rows[0].Descricao, Convert.ToDecimal(dtConcurso.Rows[0].Ano), Convert.ToDecimal(dtConcurso.Rows[0].Semestre), dtConcurso.Rows[0].Status, dtConcurso.Rows[0].Tipo, Convert.ToDateTime(dtConcurso.Rows[0].Dt_inicio), Convert.ToDateTime(dtConcurso.Rows[0].Dt_fim), Convert.ToDecimal(dtConcurso.Rows[0].Nr_digitos_codigo), Convert.ToDecimal(dtConcurso.Rows[0].Pontuacao_minima), colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Processo seletivo cadastrado com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue Alterar(Ly_concurso_docente dtConcurso)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                if (dtConcurso != null)
                {
                    if (dtConcurso.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtConcurso);

                        Ly_concurso_docente.Row.Update(connection, dtConcurso.Rows[0].Concurso, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Processo seletivo alterado com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public void AlteraFuncaoConcurso(string concurso, string funcaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE [LY_CONCURSO_DOCENTE] 
                                            SET [FUNCAOID] = @FUNCAOID 
                                        WHERE [CONCURSO] = @CONCURSO ";

                contextQuery.Parameters.Add("@FUNCAOID", funcaoId);
                contextQuery.Parameters.Add("@CONCURSO", concurso);

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
            finally
            {
                ctx.Dispose();
            }
        }

        public static RetValue Excluir(string concurso)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                object[] parametros = new object[] { concurso };
                Ly_concurso_docente dtConcurso = Ly_concurso_docente.Query(connection, "concurso = ?", parametros);

                if (dtConcurso != null)
                {
                    if (dtConcurso.Rows != null)
                    {
                        foreach (Ly_concurso_docente.Row linha in dtConcurso.Rows)
                        {
                            linha.Delete();
                        }

                        dtConcurso.Put(Config.CreateWritableConnection());
                        retorno = VerificarErro(dtConcurso);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Processo seletivo removido com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public DataTable ConsultarConcurso(string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = QueryConsultaConcurso;

                contextQuery.Parameters.Add("@CONCURSO", concurso);

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


        public static QueryTable ConsultarStatus()
        {
            QueryTable qt = null;
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            connection.Open();
            string sql = "SELECT ITEM, DESCR FROM HD_TABELAITEM WHERE TABELA = 'StatusConcursoDoc'";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }


            return qt;
        }

        /// <summary>
        /// Este Método Executa a query e Preenche a entidade na tabela hpGalerias com Entidades.E_hpGalerias como Parametro
        /// </summary>
        /// <param name="Entidade">Entidades.E_hpGalerias</param>
        /// <returns>DataTable</returns>
        public DataTable RetornaDisciplinasHabilitadas()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = QueryDisciplinasHabilitadas;
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

        public DataTable RetornaDisciplinasHabilitadasPor(string strConcurso, string strRegional, string strMunicipio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = QueryDisciplinasHabilitadasFiltradas;

                contextQuery.Parameters.Add("@CONCURSO", strConcurso);
                contextQuery.Parameters.Add("@REGIONALID", strRegional);
                contextQuery.Parameters.Add("@MUNICIPIO", strMunicipio);

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

        public DataTable RetornaFuncaoConcuso(string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = QueryConsultaFuncaoConcurso;

                contextQuery.Parameters.Add("@CONCURSO", concurso);

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

		public int RetornarCategoriasPor(string strConcurso)
		{
			int ordem = 0;
			DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
			SqlDataReader reader = null;
			ContextQuery contextQuery = new ContextQuery();

			try
			{
				contextQuery.Command = @" SELECT COUNT(CDCD.CONCURSO_DOCENTE__CATEGORIA_DOCENTEID) as QTD
										FROM ContratoTemporario.CONCURSO_DOCENTE__CATEGORIA_DOCENTE CDCD
										WHERE CDCD.CONCURSOID = @CONCURSO";

				contextQuery.Parameters.Add("@CONCURSO", strConcurso);

				reader = ctx.GetDataReader(contextQuery);

				while (reader.Read())
				{
					ordem = Convert.ToInt32(reader["QTD"]);
				}

				return ordem;
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

        public bool ExisteDocumentoNecesssarioPor(string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command =
                @" 
                    SELECT   COUNT(0) 
                    FROM   RECURSOSHUMANOS.TIPODOCUMENTOCONCURSO 					
                    WHERE  CONCURSO = @CONCURSO 
                ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);


                return ctx.GetReturnValue<int>(contextQuery) > 0;
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
