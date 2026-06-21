using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.RN.PrestacaoContas
{

    public class UnidadeEnsinoImpedida
    {
        public bool PossuiUnidadeEnsinoImpedidaPor(DataContext contexto, int motivoImpedimentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.UNIDADEENSINOIMPEDIDA (NOLOCK)
                                    WHERE MOTIVOIMPEDIMENTOID = @MOTIVOIMPEDIMENTOID ";

            contextQuery.Parameters.Add("@MOTIVOIMPEDIMENTOID", SqlDbType.Int, motivoImpedimentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public Entidades.UnidadeEnsinoImpedida ObtemImpedimentoAtivoPor(DataContext contexto, string censo)
        {
            Entidades.UnidadeEnsinoImpedida unidadeEnsinoImpedida = new Entidades.UnidadeEnsinoImpedida();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                    FROM	PrestacaoContas.UNIDADEENSINOIMPEDIDA 
                                    WHERE	CENSO = @CENSO
		                                    AND DATAINICIO <= GETDATE()
		                                    AND (DATAFIM IS NULL OR DATAFIM >= GETDATE()) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            unidadeEnsinoImpedida = contexto.TryToBindEntity<Entidades.UnidadeEnsinoImpedida>(contextQuery);

            return unidadeEnsinoImpedida;
        }

        public Entidades.UnidadeEnsinoImpedida ObtemImpedimentoAtivo(DataContext contexto, string censo)
        {
            Entidades.UnidadeEnsinoImpedida unidadeEnsinoImpedida = new Entidades.UnidadeEnsinoImpedida();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                    FROM	PrestacaoContas.UNIDADEENSINOIMPEDIDA 
                                    WHERE	CENSO = @CENSO ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            unidadeEnsinoImpedida = contexto.TryToBindEntity<Entidades.UnidadeEnsinoImpedida>(contextQuery);

            return unidadeEnsinoImpedida;
        }

        public DataTable ListaPor(string censo)
        {
            DataTable dt = null;

            try
            {
                //Verifica se a busca nao eh por escola
                if (censo.IsNullOrEmptyOrWhiteSpace())
                {
                    //CAso o opção seja todas, buscar apenas a ultima de cada escola
                    dt = this.ListaUltimas();
                }
                else
                {
                    //Em busca por escola trazer todas da escola
                    dt = this.ListaTodasPor(censo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        private DataTable ListaTodasPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT I.UNIDADEENSINOIMPEDIDAID,
	                                           R.REGIONAL,
                                               UE.NOME_COMP AS ESCOLA,
                                               I.CENSO,
	                                           I.MOTIVOIMPEDIMENTOID,
                                               M.DESCRICAO  AS MOTIVO,
                                               i.DATAINICIO,
                                               i.DATAFIM
                                        FROM   PRESTACAOCONTAS.UNIDADEENSINOIMPEDIDA I (NOLOCK)
                                               INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
                                                       ON I.CENSO = UE.UNIDADE_ENS
                                               INNER JOIN TCE_REGIONAL R (NOLOCK)
                                                       ON UE.ID_REGIONAL = R.ID_REGIONAL
                                               INNER JOIN PRESTACAOCONTAS.MOTIVOIMPEDIMENTO M (NOLOCK)
                                                       ON I.MOTIVOIMPEDIMENTOID = M.MOTIVOIMPEDIMENTOID
                                        WHERE  I.CENSO = @CENSO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        private DataTable ListaUltimas()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT CENSO, 
                                        (SELECT TOP 1 UNIDADEENSINOIMPEDIDAID 
	                                        FROM PRESTACAOCONTAS.UNIDADEENSINOIMPEDIDA I2 
	                                        WHERE I2.CENSO = I.CENSO
	                                        ORDER BY I2.DATAINICIO DESC) AS ULTIMA
                                        INTO #ULTIMAS
                                        FROM PRESTACAOCONTAS.UNIDADEENSINOIMPEDIDA I
                                        GROUP BY CENSO

                                        SELECT I.UNIDADEENSINOIMPEDIDAID,
	                                           R.REGIONAL,
                                               UE.NOME_COMP AS ESCOLA,
                                               I.CENSO,
	                                           I.MOTIVOIMPEDIMENTOID,
                                               M.DESCRICAO  AS MOTIVO,
                                               I.DATAINICIO,
                                               I.DATAFIM
                                        FROM   #ULTIMAS U 
											   INNER JOIN PRESTACAOCONTAS.UNIDADEENSINOIMPEDIDA I (NOLOCK) ON U.ULTIMA = I.UNIDADEENSINOIMPEDIDAID
                                               INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
                                                       ON I.CENSO = UE.UNIDADE_ENS
                                               INNER JOIN TCE_REGIONAL R (NOLOCK)
                                                       ON UE.ID_REGIONAL = R.ID_REGIONAL
                                               INNER JOIN PRESTACAOCONTAS.MOTIVOIMPEDIMENTO M (NOLOCK)
                                                       ON I.MOTIVOIMPEDIMENTOID = M.MOTIVOIMPEDIMENTOID  

                                        DROP TABLE #ULTIMAS ";

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

        public ValidacaoDados Valida(Entidades.UnidadeEnsinoImpedida unidadeEnsinoImpedida, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeEnsinoImpedida == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (unidadeEnsinoImpedida.UnidadeEnsinoImpedidaId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (unidadeEnsinoImpedida.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ENSINO é obrigatório.");
            }

            if (unidadeEnsinoImpedida.MotivoImpedimentoId <= 0)
            {
                mensagens.Add("Campo MOTIVO DO IMPEDIMENTO é obrigatório.");
            }

            if (unidadeEnsinoImpedida.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }
            else if (unidadeEnsinoImpedida.DataFim != null && unidadeEnsinoImpedida.DataFim != DateTime.MinValue)
            {
                if (unidadeEnsinoImpedida.DataInicio > unidadeEnsinoImpedida.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser superior a DATA FIM.");
                }

                //Não é possível alterar uma unidade de ensino em que a data final é menor que a data atual
                if (unidadeEnsinoImpedida.DataFim > DateTime.Now.Date)
                {
                    mensagens.Add("A DATA FIM não pode ser superior a data atual.");
                }
            }

            if (unidadeEnsinoImpedida.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida sobreposição de datas
                    //Verifica se a data de inicio está intercalada com outro
                    if (this.PossuiDataInicioEmOutroIntervaloPor(contexto, unidadeEnsinoImpedida.Censo, unidadeEnsinoImpedida.DataInicio, unidadeEnsinoImpedida.UnidadeEnsinoImpedidaId))
                    {
                        mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outra conta corrente dessa escola ou regional.");
                    }

                    //Verifica se não possui data de fim
                    if (unidadeEnsinoImpedida.DataFim != null && unidadeEnsinoImpedida.DataFim > DateTime.MinValue)
                    {
                        //Verifica se a data de inicio está intercalada com outro
                        if (this.PossuiDataFimEmOutroIntervaloPor(contexto, unidadeEnsinoImpedida.Censo, Convert.ToDateTime(unidadeEnsinoImpedida.DataFim), unidadeEnsinoImpedida.UnidadeEnsinoImpedidaId))
                        {
                            mensagens.Add("DATA FIM não pode estar dentro do intervalo de outra conta corrente dessa escola ou regional.");
                        }

                        //Verifica se as datas de inicio e de fim estão intercalada com outro
                        if (this.PossuiOutraIntercaladaPor(contexto, unidadeEnsinoImpedida.Censo, unidadeEnsinoImpedida.DataInicio, Convert.ToDateTime(unidadeEnsinoImpedida.DataFim), unidadeEnsinoImpedida.UnidadeEnsinoImpedidaId))
                        {
                            mensagens.Add("DATA INÍCIO E FIM não podem intercalar com outra conta corrente desta escola ou regional.");
                        }
                    }

                    if (!cadastro && this.PossuiDataFimPor(contexto, unidadeEnsinoImpedida.UnidadeEnsinoImpedidaId))
                    {
                        //Serão permitidos apenas editar unidades de ensinos impedidas com a data final nula (não informada)
                        mensagens.Add("Não é permitido editar impedimentos que já possuem DATA FIM.");
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

        private bool PossuiOutraIntercaladaPor(DataContext ctx, string censo, DateTime dataInicio, DateTime dataFim, int unidadeEnsinoImpedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   PrestacaoContas.UNIDADEENSINOIMPEDIDA  (NOLOCK)
                            WHERE CENSO = @CENSO                                                                         
                                    AND UNIDADEENSINOIMPEDIDAID <> @UNIDADEENSINOIMPEDIDAID
                                    AND @DATAINICIO <= CONVERT(DATE, DATAINICIO) 
                                    AND @DATAFIM >= CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE()))) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@UNIDADEENSINOIMPEDIDAID", SqlDbType.Int, unidadeEnsinoImpedidaId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.Date, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.Date, dataFim.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataInicioEmOutroIntervaloPor(DataContext ctx, string censo, DateTime data, int unidadeEnsinoImpedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*)
 
                                FROM  [PrestacaoContas].[UNIDADEENSINOIMPEDIDA]  (NOLOCK)
                                WHERE CENSO = @CENSO                                                                           
                                    AND UNIDADEENSINOIMPEDIDAID <> @UNIDADEENSINOIMPEDIDAID
	                                AND @DATA BETWEEN DATAINICIO AND 
			                                CONVERT(DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())) ) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@UNIDADEENSINOIMPEDIDAID", SqlDbType.Int, unidadeEnsinoImpedidaId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataFimPor(DataContext ctx, int unidadeEnsinoImpedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   [PRESTACAOCONTAS].[UNIDADEENSINOIMPEDIDA] 
                        WHERE  UNIDADEENSINOIMPEDIDAID = @UNIDADEENSINOIMPEDIDAID
                               AND DATAFIM IS NOT NULL ";

            contextQuery.Parameters.Add("@UNIDADEENSINOIMPEDIDAID", SqlDbType.Int, unidadeEnsinoImpedidaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiDataFimEmOutroIntervaloPor(DataContext ctx, string censo, DateTime data, int unidadeEnsinoImpedidaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[UNIDADEENSINOIMPEDIDA] 
                        WHERE  CENSO = @CENSO 
                                AND UNIDADEENSINOIMPEDIDAID <> @UNIDADEENSINOIMPEDIDAID
                                AND @DATA BETWEEN 
                                    CONVERT(DATE, CONVERT(DATETIME, DATAINICIO) + 1) AND CONVERT( 
                                    DATE, CONVERT(DATETIME, ISNULL(DATAFIM, GETDATE())))  ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@UNIDADEENSINOIMPEDIDAID", SqlDbType.Int, unidadeEnsinoImpedidaId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.Date, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void Insere(Entidades.UnidadeEnsinoImpedida unidadeEnsinoImpedida)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.UNIDADEENSINOIMPEDIDA
                                               (MOTIVOIMPEDIMENTOID
                                               ,CENSO
                                               ,DATAINICIO
                                               ,DATAFIM
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@MOTIVOIMPEDIMENTOID, 
                                               @CENSO, 
                                               @DATAINICIO, 
                                               @DATAFIM, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@MOTIVOIMPEDIMENTOID", SqlDbType.Int, unidadeEnsinoImpedida.MotivoImpedimentoId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, unidadeEnsinoImpedida.Censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, unidadeEnsinoImpedida.DataInicio);

                if (unidadeEnsinoImpedida.DataFim == null || unidadeEnsinoImpedida.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, unidadeEnsinoImpedida.DataFim);
                }
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeEnsinoImpedida.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public void Atualiza(Entidades.UnidadeEnsinoImpedida unidadeEnsinoImpedida)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE [PRESTACAOCONTAS].[UNIDADEENSINOIMPEDIDA]
                                        SET DATAFIM = @DATAFIM,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE UNIDADEENSINOIMPEDIDAID = @UNIDADEENSINOIMPEDIDAID ";

                contextQuery.Parameters.Add("@UNIDADEENSINOIMPEDIDAID", SqlDbType.Int, unidadeEnsinoImpedida.UnidadeEnsinoImpedidaId);

                if (unidadeEnsinoImpedida.DataFim == null || unidadeEnsinoImpedida.DataFim == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, unidadeEnsinoImpedida.DataFim);
                }

                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, unidadeEnsinoImpedida.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int unidadeEnsinoImpedidaId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeEnsinoImpedidaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
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

        public void Remove(int unidadeEnsinoImpedidaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE [PRESTACAOCONTAS].[UNIDADEENSINOIMPEDIDA]

                                        WHERE UNIDADEENSINOIMPEDIDAID = @UNIDADEENSINOIMPEDIDAID ";

                contextQuery.Parameters.Add("@UNIDADEENSINOIMPEDIDAID", SqlDbType.Int, unidadeEnsinoImpedidaId);

                contexto.ApplyModifications(contextQuery);
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
    }
}
