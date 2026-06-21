using System;
using System.Collections.Generic;
using System.Text;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using System.Data;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Linq;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class Funcao : RNBase
    {
        public static string SelecionarFuncao(string funcao)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                string sql = " SELECT TOP 1 DESCRICAO FROM LY_FUNCAO WHERE FUNCAO = ? ";
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, funcao);

                if (!valorConsulta.IsNull)
                    return (string)valorConsulta;
            }
            finally
            {
                connection.Close();
            }

            return string.Empty;
        }

        public static QueryTable PreencherComboFuncao(string usuario)
        {
            if (RN.Usuarios.UsuarioPrivilegiado(usuario))
            {
                return Consultar(@"select funcao, descricao from ly_funcao order by descricao");
            }
            else
            {
                return Consultar(@"select f.funcao, f.descricao from ly_funcao f 
                        inner join LY_PADACES_FUNCAO pf on f.funcao = pf.funcao
                        inner join PADUSUARIO pu on  pu.PADACES = pf.PADACES
                        where USUARIO = ? order by f.descricao", usuario);
            }
        }

        public static bool ExisteDescricao(string descricao)
        {
            string sql = "select 1 from ly_funcao where descricao = ?";
            int retorno = ExecutarFuncao(sql, descricao);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static bool ExisteDescricaoAlteracao(string descricao, string funcao)
        {
            string sql = "select 1 from ly_funcao where descricao = ? and funcao <> ?";
            int retorno = ExecutarFuncao(sql, descricao, funcao);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static String[] ConsultaPorDescricao(string descricao)
        {
            if (!ExisteDescricao(descricao))
                return new String[] { };

            TConnection connection = Config.CreateConnection();
            connection.Open();

            Ly_funcao rows = CR.Ly_funcao.Query(connection, "descricao = ?", descricao);
            List<String> funcoes = new List<String>();
            if (rows != null)
                foreach (Ly_funcao.Row row in rows.Rows)
                    funcoes.Add(row.Funcao);
            connection.Close();
            return funcoes.ToArray();
        }
        /// <summary>
        /// Verifica se a função da lotação ativa do docente permite realização de GLP.
        /// </summary>
        /// <param name="connection">Conexão.</param>
        /// <param name="funcao">Código da função.</param>
        /// <returns>Permite GLP se LY_FUNCAO.CAMPO_07 = 'S'</returns>
        public static bool PermiteGLP(TConnection connection, String funcao)
        {
            if (String.IsNullOrEmpty(funcao))
                return false;

            try
            {
                DbObject retorno = TCommand.ExecuteScalar(connection, "SELECT campo_07 FROM ly_funcao WHERE funcao = ?", funcao);
                return Convert.ToString(retorno) == "S";
            }
            catch
            {
                return false;
            }
        }

        public LyFuncao ObtemPor(DataContext contexto, string codigoFuncao)
        {	
            LyFuncao funcao = new LyFuncao();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  SELECT * 
                                         FROM [DBO].[LY_FUNCAO] (NOLOCK)
                                         WHERE FUNCAO = @CODIGOFUNCAO ";

            contextQuery.Parameters.Add("@CODIGOFUNCAO", TechneDbType.T_CODIGO, codigoFuncao);

            funcao = contexto.TryToBindEntity<LyFuncao>(contextQuery);

            return funcao;
        }

        public bool PermiteGlpPor(DataContext contexto, string funcao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1)
                                        FROM LY_FUNCAO (NOLOCK)
                                        WHERE FUNCAO = @FUNCAO
	                                        AND CAMPO_07 = 'S' ";

            contextQuery.Parameters.Add("@FUNCAO", TechneDbType.T_CODIGO, funcao);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }


        /// <summary>
        /// Verifica se a função é compatível com Ensino Médio (DOC I).
        /// Campo LY_FUNCAO.CAMPO_08.
        /// </summary>
        /// <param name="connection">Conexão.</param>
        /// <param name="funcao">Código da função.</param>
        /// <returns>Resultado da verificação. TRUE:compatível; FALSE:incompatível.</returns>
        public static bool VerificaFuncaoDOCI(TConnection connection, string funcao)
        {
            try
            {
                DbObject retorno = TCommand.ExecuteScalar(connection, @"
                    SELECT CAMPO_08 FROM LY_FUNCAO WHERE FUNCAO = ?", funcao);
                return Convert.ToString(retorno) == "S";
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Verifica se a função é compatível com Ensino Fundamental (DOC II).
        /// Campo LY_FUNCAO.CAMPO_09.
        /// </summary>
        /// <param name="connection">Conexão.</param>
        /// <param name="funcao">Código da função.</param>
        /// <returns>Resultado da verificação. TRUE:compatível; FALSE:incompatível.</returns>
        public static bool VerificaFuncaoDOCII(TConnection connection, string funcao)
        {
            try
            {
                DbObject retorno = TCommand.ExecuteScalar(connection, @"
                    SELECT CAMPO_09 FROM LY_FUNCAO WHERE FUNCAO = ?", funcao);
                return Convert.ToString(retorno) == "S";
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Verifica se a função possui alguma lotação ativa cujo docente possui solicitação de GLP ativa.
        /// </summary>
        /// <param name="funcao">Código da função.</param>
        /// <returns>TRUE: Possui solicitação; FALSE: não possui solicitação.</returns>
        public static bool PossuiLotacaoComSolicitacaoGLP(String funcao)
        {
            TConnection conn = Config.CreateConnection();
            conn.Open();

            try
            {
                DbObject possuiLotacaoSomSolicGLP = TCommand.ExecuteScalar(conn, @"
                    DECLARE @funcao T_CODIGO = ?

                    SELECT CASE WHEN EXISTS
                    (
	                    SELECT	TOP 1 1
	                    FROM	ly_docente_funcao_glp dfg (NOLOCK)
	                    WHERE
		                    dfg.status = 'Aceita' AND
		                    -- ACEITA E DENTRO DO PRAZO
		                    EXISTS (
			                    SELECT	TOP 1 1
			                    FROM	ly_docente_funcao_glp_detalhe dd (NOLOCK)
			                    WHERE	dd.id_docente_funcao_glp = dfg.id_docente_funcao_glp AND
					                    dd.status = 'Aceita' AND
					                    (dfg.prazo IS NULL OR CONVERT(DATE,DATEADD(DAY, dfg.prazo, dd.data)) >= CONVERT(DATE,GETDATE()))
		                    ) AND			
		                    -- MATRICULA POSSUI LOTAÇÃO ATIVA
		                    EXISTS (
			                    SELECT	TOP 1 1 
			                    FROM	ly_lotacao l (NOLOCK)
			                    WHERE	l.matricula = dfg.matricula AND				
					                    l.data_nomeacao <= CONVERT(DATE,GETDATE()) AND
					                    (l.data_desativacao IS NULL OR CONVERT(DATE,l.data_desativacao) > CONVERT(DATE,GETDATE())) AND
					                    l.funcao = @funcao
		                    )
                    ) THEN 'S' ELSE 'N' END PODE_ALTERAR", funcao);

                return Convert.ToString(possuiLotacaoSomSolicGLP) == "S";
            }
            catch { }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public static QueryTable ObterFuncaoRegente()
        {
            TConnectionWritable tconnw = Config.CreateWritableConnection();
            QueryTable qtFuncaoRegente = null;

            try
            {
                tconnw.Open(true);

                qtFuncaoRegente = new QueryTable(
                    " SELECT TOP 1 FUNCAO, " +
                        " DESCRICAO " +
                    " FROM LY_FUNCAO " +
                    " WHERE CAMPO_01 = 'S' " +
                    " ORDER BY DESCRICAO ");
                qtFuncaoRegente.Query(tconnw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tconnw.Close();
            }
            return qtFuncaoRegente; ;
        }

        public static QueryTable ObterFuncaoDoc()
        {
            return Consultar(" SELECT funcao, descricao FROM LY_FUNCAO WHERE CAMPO_01 = 'S'");
        }

        public DataTable RetornaFuncao()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @"SELECT F.FUNCAOID AS CODIGO, (LF.DESCRICAO + ' - ' + F.DESCRICAO) AS DESCRICAO
                                FROM FUNCAO F
	                            INNER JOIN LY_FUNCAO LF
		                        ON (F.FUNCAOID = LF.FUNCAO)
                                WHERE ISNULL(LF.CAMPO_10, 'N') = 'S'";


                dt = ctx.GetDataTable(contextQuery);

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

            return dt;
        }

        public DataTable RetornaFuncao(string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @"SELECT F.FUNCAOID CODIGO, F.DESCRICAO 
                                FROM FUNCAO F INNER JOIN LY_CONCURSO_DOCENTE C ON (F.FUNCAOID = C.FUNCAOID)
                                WHERE CONCURSO = @CONCURSO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);


                dt = ctx.GetDataTable(contextQuery);

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

            return dt;
        }

        public DataTable RetornaFuncao(string concurso, string candidato)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @"SELECT F.FUNCAOID, F.DESCRICAO 
                                FROM FUNCAO F 
                                    INNER JOIN LY_CATEGORIA_DOCENTE CT ON (F.FUNCAOID = CT.FUNCAO)
	                                INNER JOIN LY_CANDIDATO_DOCENTE CD ON (CT.CATEGORIA = CD.CATEGORIA)
                                WHERE 
                                    CONCURSO = @CONCURSO AND 
                                    CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                dt = ctx.GetDataTable(contextQuery);

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

            return dt;
        }

        public static DataTable RetornaCategoria(string funcao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @"SELECT 
                                LYF.DESCRICAO AS CODIGO
                                FROM FUNCAO F 
                                INNER JOIN LY_FUNCAO LYF ON LYF.FUNCAO = F.FUNCAOID
                                WHERE F.FUNCAOID = @FUNCAOID";

                contextQuery.Parameters.Add("@FUNCAOID", funcao);


                dt = ctx.GetDataTable(contextQuery);

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

            return dt;
        }

        public DataTable ListaFuncao()
        {
            DataTable funcao = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT  F.DESCRICAO as CODIGO,
                                                        LYF.DESCRICAO
                                                FROM    FUNCAO F
                                                        INNER JOIN LY_FUNCAO LYF ON LYF.FUNCAO = F.FUNCAOID
                                                WHERE   F.FUNCAOID IN ( 106, 108 )  ";


                funcao = Consultar(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            return funcao;
        }

        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT FUNCAO, 
                                               DESCRICAO
                                        FROM   LY_FUNCAO 
                                        ORDER BY DESCRICAO ";

                lista = ctx.GetDataTable(contextQuery);
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

            return lista;
        }

        public DataTable ListaExcel()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT DESCRICAO,
	FUNCAOBB AS TIPO,
	CAMPO_01 AS REGENTE,
	CAMPO_02 AS 'FUNÇÃO ESTRA-CLASSE?',
	CAMPO_03 AS 'LIBERA GLP NA 2 MATRICULA?',
	CAMPO_04 AS 'DIRETOR?',
	CAMPO_05 AS 'SECRETÁRIO?',
	CAMPO_06 AS 'DESALOCA AULAS?',
	CAMPO_07 AS 'PERMITE GLP?',
	CAMPO_08 AS 'COMPATÍVEL COM ENSINO MÉDIO - DOC I?',
	CAMPO_09 AS 'COMPATÍVEL COM ENSINO FUNDAMENTAL - DOC II?',
	CAMPO_10 AS 'PERMITE CONTRATO TEMPORÁRIO?',
	SEMCARGAHORARIAEFETIVA AS 'SEM CH EFETIVA?',
	ATIVO
	                                           
 FROM   LY_FUNCAO 
 ORDER BY DESCRICAO ";

                lista = ctx.GetDataTable(contextQuery);
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

            return lista;
        }

        // public DataTable ListaExcel() == listar todas funcoes sem trazer o id, na vdd a query ja nao busca o id

        
        
        public static string RetornaFuncaoPorConcurso(string strConcurso)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                string sql = " SELECT FUNCAOID FROM LY_CONCURSO_DOCENTE WHERE CONCURSO = ? ";
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, strConcurso);

                if (!valorConsulta.IsNull)
                    return (string)valorConsulta;
            }
            finally
            {
                connection.Close();
            }

            return string.Empty;
        }

        public static string RetornaDescricaoFuncaoPorConcurso(string strConcurso)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                string sql = "SELECT F.DESCRICAO FROM FUNCAO F INNER JOIN LY_CONCURSO_DOCENTE C ON (F.FUNCAOID = C.FUNCAOID) WHERE CONCURSO = ? ";
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, strConcurso);

                if (!valorConsulta.IsNull)
                    return (string)valorConsulta;
            }
            finally
            {
                connection.Close();
            }

            return string.Empty;
        }

        public bool PossuiTipoFuncaoAtivaPor(DataContext ctx, int funcaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   DBO.FUNCAO F ( NOLOCK ) 
                                        WHERE  FUNCAOID = @FUNCAOID 
                                               AND TIPOFUNCAOID <> 4  ";

                contextQuery.Parameters.Add("@FUNCAOID", funcaoId);

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

        public bool PossuiTipoFuncaoInativaPor(DataContext ctx, int funcaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   DBO.FUNCAO F ( NOLOCK ) 
                                        WHERE  FUNCAOID = @FUNCAOID 
                                               AND TIPOFUNCAOID = 4  ";

                contextQuery.Parameters.Add("@FUNCAOID", funcaoId);

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

        public ValidacaoDados Valida(Entidades.Funcao funcao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (funcao == null)
            {
                return validacaoDados;
            }

            if (funcao.FuncaoId <= 0)
            {
                mensagens.Add("Campo FUNCAO é obrigatório.");
            }

            if (string.IsNullOrEmpty(funcao.Descricao))
            {
                mensagens.Add("Campo DESCRIÇÃO FUNCAO é obrigatório.");
            }

            if (funcao.TipoFuncaoId <= 0)
            {
                mensagens.Add("Campo TIPO FUNCAO é obrigatório.");
            }
            else if (funcao.TipoFuncaoId == 4)
            {
                mensagens.Add("Campo TIPO FUNCAO não pode ser igual a 4.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a função já está associada a um tipo diferente de 4	
                    if (this.PossuiTipoFuncaoAtivaPor(contexto, funcao.FuncaoId))
                    {
                        mensagens.Add("Está função já possui associação.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public ValidacaoDados ValidaRemocaoTipoFuncao(Entidades.Funcao funcao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.TipoFuncao rnTipoFuncao = new TipoFuncao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (funcao == null)
            {
                return validacaoDados;
            }

            if (funcao.FuncaoId <= 0)
            {
                mensagens.Add("Campo FUNCAO é obrigatório.");
            }

            if (funcao.TipoFuncaoId <= 0)
            {
                mensagens.Add("Campo TIPO FUNCAO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnTipoFuncao.PossuiTipoFuncaoComAssociacaoAtivaPor(contexto, funcao.TipoFuncaoId, funcao.FuncaoId))
                    {
                        mensagens.Add("Registro não pode ser excluído por possuir informação associada.");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Insere(Entidades.Funcao funcao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            bool exiteTipoFuncao = false;
            try
            {
                //Verifica se a funcao já existe na tabela FUNCAO com TIPOFUNCAOID igual a 4
                exiteTipoFuncao = this.PossuiTipoFuncaoInativaPor(ctx, funcao.FuncaoId);

                if (exiteTipoFuncao)
                {
                    this.AtualizaTipoFuncao(ctx, funcao.FuncaoId, funcao.TipoFuncaoId);
                }
                else
                {
                    this.Insere(ctx, funcao);
                }
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

        private void Insere(DataContext ctx, Entidades.Funcao funcao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO FUNCAO 
                                                    (FUNCAOID, 
                                                     DESCRICAO, 
                                                     TIPOFUNCAOID) 
                                        VALUES      (@FUNCAOID, 
                                                     @DESCRICAO, 
                                                     @TIPOFUNCAOID)  ";

                contextQuery.Parameters.Add("@FUNCAOID", funcao.FuncaoId);
                contextQuery.Parameters.Add("@DESCRICAO", funcao.Descricao);
                contextQuery.Parameters.Add("@TIPOFUNCAOID", funcao.TipoFuncaoId);

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

        private void AtualizaTipoFuncao(DataContext ctx, int funcaoId, int tipoFuncaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE FUNCAO 
                                            SET    TIPOFUNCAOID = @TIPOFUNCAOID 
                                            WHERE  FUNCAOID = @FUNCAOID  ";

                contextQuery.Parameters.Add("@FUNCAOID", funcaoId);
                contextQuery.Parameters.Add("@TIPOFUNCAOID", tipoFuncaoId);

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

        public void RemoveTipoFuncao(Entidades.Funcao funcao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            int tipoFuncaoid = (int)RN.TipoFuncao.EnumTipoFuncao.Outros;
            try
            {
                //Na exclusão de tipo função, gravar na tabela FUNCAO, no campo TIPOFUNCAOID o valor “4” - Outros (exclusão lógica)
                this.AtualizaTipoFuncao(ctx, funcao.FuncaoId, tipoFuncaoid);
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

        public bool EhFuncaoSemCHEfetiva(string funcao)
        {
            bool funcaoRegenteSemCHEfetiva = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            object obj = new Object();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1
                            SEMCARGAHORARIAEFETIVA
                    FROM    LY_FUNCAO                          
                    WHERE   FUNCAO = @FUNCAO
                        AND ISNULL(SEMCARGAHORARIAEFETIVA, 'N') = 'S'"
                };

                contextQuery.Parameters.Add("@FUNCAO", funcao);

                obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    funcaoRegenteSemCHEfetiva = true;
                }

                return funcaoRegenteSemCHEfetiva;
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

        //Verifica se a funcao deve desalocar aulas (campo_06 = S)
        public bool VerificaDesalocacaoAulasPor(DataContext ctx, string funcao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool desalocaAulas = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   LY_FUNCAO 
                                        WHERE  CAMPO_06 = 'S' 
                                               AND FUNCAO = @FUNCAO  ";

            contextQuery.Parameters.Add("@FUNCAO", funcao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                desalocaAulas = true;
            }

            return desalocaAulas;
        }

        public bool EhFuncaoRegente(DataContext ctx, string funcao)
        {
            SqlDataReader reader = null;
            string regente = string.Empty;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  CAMPO_01
                                FROM    dbo.LY_FUNCAO
                                WHERE   FUNCAO = @FUNCAO "
                };

                contextQuery.Parameters.Add("@FUNCAO", funcao);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    regente = Convert.ToString(reader["CAMPO_01"]);
                }

                if (regente == "S")
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool EhFuncaoDiretor(string funcao)
        {
            bool funcaoDiretor = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                funcaoDiretor = this.EhFuncaoDiretor(ctx, funcao);
                return funcaoDiretor;
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

        public bool EhFuncaoDiretor(DataContext ctx, string funcao)
        {
            bool funcaoDiretor = false;
            object obj = new Object();

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*)
                    FROM    LY_FUNCAO                          
                    WHERE   FUNCAO = @FUNCAO
                        AND CAMPO_04='S'"
            };

            contextQuery.Parameters.Add("@FUNCAO", funcao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                funcaoDiretor = true;
            }

            return funcaoDiretor;
        }

        public bool EhFuncaoSecretario(string funcao)
        {
            bool funcaoSecretario = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                funcaoSecretario = this.EhFuncaoSecretario(ctx, funcao);
                return funcaoSecretario;
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

        public bool EhFuncaoSecretario(DataContext ctx, string funcao)
        {
            bool funcaoSecretario = false;
            object obj = new Object();

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*)
                    FROM    LY_FUNCAO                          
                    WHERE   FUNCAO = @FUNCAO
                    AND CAMPO_05='S'"
            };

            contextQuery.Parameters.Add("@FUNCAO", funcao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                funcaoSecretario = true;
            }

            return funcaoSecretario;
        }

        public string ObtemDescricaoPor(string funcao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string descricao = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT DESCRICAO 
                            FROM   LY_FUNCAO (NOLOCK)
                            WHERE  FUNCAO = @FUNCAO ";

                contextQuery.Parameters.Add("@FUNCAO", funcao);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["DESCRICAO"] != DBNull.Value)
                    {
                        descricao = reader["DESCRICAO"].ToString();
                    }
                }

                return descricao;
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
    }
}