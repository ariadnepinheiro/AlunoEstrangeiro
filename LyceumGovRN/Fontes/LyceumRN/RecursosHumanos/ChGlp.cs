using System;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.RecursosHumanos.Entidades;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using System.Linq;
using Techne.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class ChGlp
    {
        public DataTable Lista(int? AGRUPAMENTOCARGOSID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 
                    CH.[CH_GLPID],
                    CH.[NR_MATRICULAS],
                    CH.[AGRUPAMENTOCARGOSID],
                    CH.[CH_GRUPO],
                    CH.[FUNCAO],
                    LYF1.[FUNCAO] + ' - ' + LYF1.[DESCRICAO] AS [FUNCAO_DESCRICAO],
                    CH.[AGRUPAMENTOCARGOSID_2],
                    CH.[CH_GRUPO_2],
                    CH.[FUNCAO_2],
                    LYF2.[FUNCAO] + ' - ' + LYF2.[DESCRICAO] AS [FUNCAO_DESCRICAO_2],
                    CH.[CH_GRUPO] + isnull(CH.[CH_GRUPO_2], 0) AS [CH_TOTAL_GRUPO],
                    CH.[CH_SEMANAL_TOTAL],
                    CH.[CH_GLP],
                    CH.[USUARIOID],
                    CH.[DATACADASTRO],
                    CH.[DATAALTERACAO]

                    FROM
                    [RecursosHumanos].[CH_GLP] CH (NOLOCK)
                    INNER JOIN LY_FUNCAO LYF1 (NOLOCK) ON LYF1.FUNCAO = CH.FUNCAO
                    LEFT JOIN LY_FUNCAO LYF2 (NOLOCK) ON LYF2.FUNCAO = CH.FUNCAO_2 ";

                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", AGRUPAMENTOCARGOSID);

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

        public Entidades.ChGlp ObtemPor(int chGlpId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.ChGlp chGlp = new Entidades.ChGlp();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT   * 
                                            FROM   RECURSOSHUMANOS.CH_GLP 
                                            WHERE  CH_GLPID = @CH_GLPID  ";

                contextQuery.Parameters.Add("@CH_GLPID", chGlpId);

                chGlp = ctx.TryToBindEntity<Entidades.ChGlp>(contextQuery);

                return chGlp;
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

        public bool ExistePor(Entidades.ChGlp chGlp)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command =
                @" 
                    SELECT   count(0) 
                    FROM   RECURSOSHUMANOS.CH_GLP 
                    WHERE  (isnull(AGRUPAMENTOCARGOSID, 0) = isnull(@AGRUPAMENTOCARGOSID, 0) and isnull(FUNCAO, '') = isnull(@FUNCAO, '') and isnull(AGRUPAMENTOCARGOSID_2, 0) = isnull(@AGRUPAMENTOCARGOSID_2, 0) and isnull(FUNCAO_2, '') = isnull(@FUNCAO_2, '')) or
                           (isnull(AGRUPAMENTOCARGOSID_2, 0) = isnull(@AGRUPAMENTOCARGOSID, 0) and isnull(FUNCAO_2, '') = isnull(@FUNCAO, '') and isnull(AGRUPAMENTOCARGOSID, 0) = isnull(@AGRUPAMENTOCARGOSID_2, 0) and isnull(FUNCAO, '') = isnull(@FUNCAO_2, ''))
                ";

                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", chGlp.AgrupamentoCargosId);
                contextQuery.Parameters.Add("@FUNCAO", chGlp.Funcao);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID_2", chGlp.AgrupamentoCargosId2);
                contextQuery.Parameters.Add("@FUNCAO_2", chGlp.Funcao2);

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

        public Entidades.ChGlp ObtemPor(DataContext contexto, int numeroMatriculas, int agrupamentoCargos, string funcao, int agrupamentoCargos2, string funcao2)
        {
            Entidades.ChGlp chGlp = new Entidades.ChGlp();
            ContextQuery contextQuery = new ContextQuery();

            if (numeroMatriculas == 1)
            {
                contextQuery.Command = @"  SELECT TOP 1 * 
                                FROM   RecursosHumanos.CH_GLP (NOLOCK)
                                WHERE  NR_MATRICULAS = 1
										AND AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID
										AND FUNCAO = @FUNCAO
										AND AGRUPAMENTOCARGOSID_2 IS NULL
										AND FUNCAO_2 IS NULL ";

                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargos);
                contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, funcao);
            }
            else
            {
                contextQuery.Command = @" SELECT TOP 1 * 
                                FROM   RecursosHumanos.CH_GLP (NOLOCK)
                                WHERE  NR_MATRICULAS = 2
                                       AND ( AGRUPAMENTOCARGOSID = ISNULL(@AGRUPAMENTOCARGOSID, 0) 
                                         AND FUNCAO = ISNULL(@FUNCAO, '') 
                                         AND ISNULL(AGRUPAMENTOCARGOSID_2, 0) = ISNULL(@AGRUPAMENTOCARGOSID_2, 0) 
                                         AND ISNULL(FUNCAO_2, '') = ISNULL(@FUNCAO_2, '') ) 
                                        OR ( ISNULL(AGRUPAMENTOCARGOSID_2, 0) = ISNULL(@AGRUPAMENTOCARGOSID, 0) 
                                             AND ISNULL(FUNCAO_2, '') = ISNULL(@FUNCAO, '') 
                                             AND AGRUPAMENTOCARGOSID = ISNULL(@AGRUPAMENTOCARGOSID_2, 0) 
                                             AND FUNCAO = ISNULL(@FUNCAO_2, '') )  ";

                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", SqlDbType.Int, agrupamentoCargos);
                contextQuery.Parameters.Add("@FUNCAO", SqlDbType.VarChar, funcao);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID_2", SqlDbType.Int, agrupamentoCargos2);
                contextQuery.Parameters.Add("@FUNCAO_2", SqlDbType.VarChar, funcao2);
            }

            chGlp = contexto.TryToBindEntity<Entidades.ChGlp>(contextQuery);

            return chGlp;
        }

        public void Insere(Entidades.ChGlp chGlp)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RECURSOSHUMANOS.CH_GLP 
                                        (NR_MATRICULAS, 
                                         AGRUPAMENTOCARGOSID, 
                                         CH_GRUPO,
                                         FUNCAO,
                                         AGRUPAMENTOCARGOSID_2,
                                         CH_GRUPO_2,
                                         FUNCAO_2,
                                         CH_SEMANAL_TOTAL,
                                         CH_GLP,
                                         USUARIOID, 
                                         DATACADASTRO, 
                                         DATAALTERACAO) 
                                    VALUES      
	                                    (@NR_MATRICULAS, 
                                        @AGRUPAMENTOCARGOSID, 
                                        @CH_GRUPO, 
                                        @FUNCAO, 
                                        @AGRUPAMENTOCARGOSID_2,
                                        @CH_GRUPO_2,
                                        @FUNCAO_2,
                                        @CH_SEMANAL_TOTAL,
                                        @CH_GLP,
                                        @USUARIOID, 
                                        @DATACADASTRO, 
                                        @DATAALTERACAO);

                                    SELECT IDENT_CURRENT('RECURSOSHUMANOS.CH_GLP')
                ";

                contextQuery.Parameters.Add("@NR_MATRICULAS", chGlp.NrMatriculas);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", chGlp.AgrupamentoCargosId);
                contextQuery.Parameters.Add("@CH_GRUPO", chGlp.ChGrupo);
                contextQuery.Parameters.Add("@FUNCAO", chGlp.Funcao);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID_2", chGlp.AgrupamentoCargosId2);
                contextQuery.Parameters.Add("@CH_GRUPO_2", chGlp.ChGrupo2);
                contextQuery.Parameters.Add("@FUNCAO_2", chGlp.Funcao2);
                contextQuery.Parameters.Add("@CH_SEMANAL_TOTAL", chGlp.ChSemanalTotal);
                contextQuery.Parameters.Add("@CH_GLP", chGlp.Ch_Glp);
                contextQuery.Parameters.Add("@USUARIOID", chGlp.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

                chGlp.ChGlpId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.ChGlp chGlp)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RECURSOSHUMANOS.CH_GLP 
                                            SET    NR_MATRICULAS = @NR_MATRICULAS, 
                                                   AGRUPAMENTOCARGOSID = @AGRUPAMENTOCARGOSID, 
                                                   CH_GRUPO = @CH_GRUPO,
                                                   FUNCAO = @FUNCAO,
                                                   AGRUPAMENTOCARGOSID_2 = @AGRUPAMENTOCARGOSID_2,
                                                   CH_GRUPO_2 = @CH_GRUPO_2,
                                                   FUNCAO_2 = @FUNCAO_2,
                                                   CH_SEMANAL_TOTAL = @CH_SEMANAL_TOTAL,
                                                   CH_GLP = @CH_GLP,
                                                   USUARIOID = @USUARIOID,
                                                   DATAALTERACAO = @DATAALTERACAO
                                           WHERE  CH_GLPID = @CH_GLPID  ";

                contextQuery.Parameters.Add("@CH_GLPID", chGlp.ChGlpId);
                contextQuery.Parameters.Add("@NR_MATRICULAS", chGlp.NrMatriculas);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID", chGlp.AgrupamentoCargosId);
                contextQuery.Parameters.Add("@CH_GRUPO", chGlp.ChGrupo);
                contextQuery.Parameters.Add("@FUNCAO", chGlp.Funcao);
                contextQuery.Parameters.Add("@AGRUPAMENTOCARGOSID_2", chGlp.AgrupamentoCargosId2);
                contextQuery.Parameters.Add("@CH_GRUPO_2", chGlp.ChGrupo2);
                contextQuery.Parameters.Add("@FUNCAO_2", chGlp.Funcao2);
                contextQuery.Parameters.Add("@CH_SEMANAL_TOTAL", chGlp.ChSemanalTotal);
                contextQuery.Parameters.Add("@CH_GLP", chGlp.Ch_Glp);
                contextQuery.Parameters.Add("@USUARIOID", chGlp.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public ValidacaoDados ValidaRemocao(int chGlpId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            DocenteGLP rnDocenteGLP = new DocenteGLP();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (chGlpId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se existe dcoente com GLP aceita para grupo de categoria / função de suas matriculas a partir de 2020 (ano de implementação da regra)
                    if (rnDocenteGLP.PossuiAgrupamentoCargosPor(contexto, chGlpId))
                    {
                        mensagens.Add("Registro não pode ser excluído pois existem pedidos de GLP aceitos para grupo de categoria{s} / função{s}.");
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

        public void Remove(decimal chGlpId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE RECURSOSHUMANOS.CH_GLP 
                                        WHERE  CH_GLPID = @CH_GLPID  ";

                contextQuery.Parameters.Add("@CH_GLPID", chGlpId);

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

        public static int ObtemCH_GLP(TConnection connection, int ch_GLPId)
        {
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, "select CH_GLP from [RecursosHumanos].[CH_GLP] (NOLOCK) WHERE CH_GLPID = ?", ch_GLPId); ;
            if (!valorConsulta.IsNull)
                return (int)valorConsulta;
            else
                return 0;
        }

        public DataTable ListaExcel()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT 
                                            CH.[NR_MATRICULAS] AS 'QTDE. MATRÍCULAS',
					                        GRUPO1.DESCRICAO AS 'GRUPO',
					                        CH.[CH_GRUPO]AS 'CH GRUPO',
                                            LYF1.[FUNCAO] + ' - ' + LYF1.[DESCRICAO] AS 'FUNÇÃO DA MATRÍCULA',
					                        GRUPO2.DESCRICAO AS 'GRUPO (2)',
                                            CH.[CH_GRUPO_2] AS 'CH GRUPO (2)',
                                            LYF2.[FUNCAO] + ' - ' + LYF2.[DESCRICAO] AS 'FUNÇÃO DA MATRÍCULA (2)',
                                            CH.[CH_GRUPO] + isnull(CH.[CH_GRUPO_2], 0) AS 'TOTAL DO GRUPO',
                                            CH.[CH_GLP] AS 'CH GLP',
                                            CH.[CH_SEMANAL_TOTAL] AS 'CH FINAL COM A GLP'

                                            FROM
                                            [RecursosHumanos].[CH_GLP] CH (NOLOCK)
                                            INNER JOIN LY_FUNCAO LYF1 (NOLOCK) ON LYF1.FUNCAO = CH.FUNCAO
                                            LEFT JOIN LY_FUNCAO LYF2 (NOLOCK) ON LYF2.FUNCAO = CH.FUNCAO_2
					                        LEFT JOIN [RecursosHumanos].[AGRUPAMENTOCARGOS] GRUPO1 ON CH.AGRUPAMENTOCARGOSID = GRUPO1.AGRUPAMENTOCARGOSID
					                        LEFT JOIN [RecursosHumanos].[AGRUPAMENTOCARGOS] GRUPO2 ON CH.AGRUPAMENTOCARGOSID_2 = GRUPO2.AGRUPAMENTOCARGOSID ";

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
    }
}
