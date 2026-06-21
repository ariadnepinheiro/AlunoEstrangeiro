using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class Operacao
    {
        public Entidades.Operacao ListaOperacaoPor(string periodoreferencia, string planodetrabalho, string censo)
        {
            Operacao rnOperacao = new Operacao();
            Entidades.Operacao OperacaoGeral = new Entidades.Operacao();
            // Entidades.Operacao dadosOperacao = new Entidades.Operacao();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            // PequenaDespesaServidor rnPequenaDespesaServidor = new PequenaDespesaServidor();

            try
            {
                //Busca Tipo do evento e dados gerais
                OperacaoGeral = this.ListaOperacaoPor(contexto, periodoreferencia, planodetrabalho, censo);

                //Alimenta dados gerais
                //     dadosOperacao.PlanoTrabalhoId = OperacaoGeral.PlanoTrabalhoId;
                //    dadosOperacao.Justificativa   = OperacaoGeral.Justificativa;

                return OperacaoGeral;
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
        public DataTable ListaDadosGridPor(string UnidadeEnsino, string PlanoTrabalho, string PeriodoPrestacaoContas, int status)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" Begin

                                drop table if exists  #temp;
                                SELECT iif(lr.STATUS = 3 , lr.codoperacao ,STR(lr.operacaoid)) as operacaoid,
                                iif (lr.tipooperacao ='C' , 'CRÉDITO','DÉBITO') as tipooperacao,
                                p.descricao as planotrabalho,
                                iif (lr.dataanalise is null , lr.dataalteracao,lr.dataanalise) as dataanalise,
                                lr.VALOR,e.OPERACAOEXIGENCIAID,e.APROVADO,
                                  iif (lr.STATUS = 0, 'Lançado pela AAE',
                                  iif (lr.STATUS = 1, 'Enviado para Análise',
                                  iif (lr.STATUS = 2, 'Devolvido para Revisão',
                                  iif (lr.STATUS = 3, 'Aprovado',
                                  iif (lr.STATUS = 4, 'Reprovado', 'NI'))))) as status ,
                                lr.JUSTIFICATIVA into #temp
                                FROM prestacaocontas.Operacao lr 
                                inner join  PrestacaoContas.PLANOTRABALHO p on p.PLANOTRABALHOID = lr.PLANOTRABALHOID
                                left join PrestacaoContas.OPERACAOEXIGENCIA e on lr.OPERACAOID = e.OPERACAOID 
                                WHERE lr.tipooperacao ='C' and lr.STATUS = @STATUS and 
                                      lr.PLANOTRABALHOID = @PLANOTRABALHOID ");

                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" and lr.CENSO = @CENSO   ");
                }
                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" and lr.PERIODOREFERENCIAID = @PERIODOREFERENCIAID    ");
                }

                sql.Append(@" select * from #temp
                                where (OPERACAOEXIGENCIAID is null and APROVADO is null)
                                or (OPERACAOEXIGENCIAID is not null and APROVADO=1)

                                End");


                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.VarChar, PlanoTrabalho);
                contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, status);

                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, UnidadeEnsino);
                }

                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.VarChar, PeriodoPrestacaoContas);
                }
                contextQuery.Command = sql.ToString();


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

        public void Aprovaemlote(string UnidadeEnsino, string PlanoTrabalho, string PeriodoPrestacaoContas)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            var planotrabalho = "";
            var periodoprestacao = "";

            try
            {

                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                    planotrabalho = @" and CENSO = @CENSO   ";

                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                    periodoprestacao = @" and PERIODOREFERENCIAID = @PERIODOREFERENCIAID    ";

                contextQuery.Command = @" UPDATE  prestacaocontas.Operacao                                         
                                        SET  STATUS = 3, 
                                        codoperacao= concat(year(GETDATE()), 'C' , REPLACE(STR(prestacaocontas.Operacao.operacaoid, 6), SPACE(1), '0'))  , 
                                        DATAALTERACAO = @DATAALTERACAO,
                                        USUARIOID = @USUARIOID
                                        FROM prestacaocontas.Operacao
                                        left join PrestacaoContas.OPERACAOEXIGENCIA e on prestacaocontas.Operacao.OPERACAOID = e.OPERACAOID   
                                        WHERE
                                                ( prestacaocontas.Operacao.STATUS = 1) AND
                                                ((OPERACAOEXIGENCIAID is null and APROVADO is null)
                                                  or (OPERACAOEXIGENCIAID is not null and APROVADO=1))
												AND PLANOTRABALHOID = @PLANOTRABALHOID  " + planotrabalho + periodoprestacao;

                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, UnidadeEnsino);
                }
                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.VarChar, PeriodoPrestacaoContas);
                }

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.VarChar, PlanoTrabalho);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, System.Web.HttpContext.Current.User.Identity.Name);

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
        private DataTable SomadordeOperacoesCredito(DataContext ctx, string UnidadeEnsino, string PlanoTrabalho, string PeriodoPrestacaoContas, string status)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" Begin

                                drop table if exists  #temp;
                               SELECT   p.descricao as planotrabalho,e.OPERACAOEXIGENCIAID,e.APROVADO, 
                                        ISNULL(sum(lr.VALOR), 0.00)  as tot into #temp
                               FROM prestacaocontas.Operacao lr 
                               inner join  PrestacaoContas.PLANOTRABALHO p on p.PLANOTRABALHOID = lr.PLANOTRABALHOID
                               left join PrestacaoContas.OPERACAOEXIGENCIA e on lr.OPERACAOID = e.OPERACAOID 
                               WHERE lr.tipooperacao ='C' and lr.STATUS = @STATUS and 
                                      lr.PLANOTRABALHOID = @PLANOTRABALHOID ");
                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@"  and lr.CENSO = @CENSO ");
                }

                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" and lr.PERIODOREFERENCIAID = @PERIODOREFERENCIAID    ");
                }

                sql.Append(@" group by  p.descricao,e.OPERACAOEXIGENCIAID,e.APROVADO order by 1   ");

                sql.Append(@" select planotrabalho, ISNULL(sum(tot), 0.00)  as total from #temp
                                where (OPERACAOEXIGENCIAID is null and APROVADO is null)
                                or (OPERACAOEXIGENCIAID is not null and APROVADO=1)
                                group by planotrabalho

                                End");

                contextQuery.Command = sql.ToString();
                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.VarChar, PlanoTrabalho);

                contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, status);

                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, UnidadeEnsino);
                }

                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.VarChar, PeriodoPrestacaoContas);
                }



                dt = ctx.GetDataTable(contextQuery);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable SomadordeOperacoesCredito(string UnidadeEnsino, string PlanoTrabalho, string PeriodoPrestacaoContas, string status)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return SomadordeOperacoesCredito(ctx, UnidadeEnsino, PlanoTrabalho, PeriodoPrestacaoContas, status);
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
        }
        private int TotaldeOperacoesCredito(DataContext ctx, string UnidadeEnsino, string PlanoTrabalho, string PeriodoPrestacaoContas, string status)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int id = 0;
            StringBuilder sql = new StringBuilder();

            try
            {

                sql.Append(@" Begin

                                drop table if exists  #temp;
                                 SELECT   e.OPERACAOEXIGENCIAID,e.APROVADO, 
                                       lr.operacaoid  into #temp
                               FROM prestacaocontas.Operacao lr 
                               inner join  PrestacaoContas.PLANOTRABALHO p on p.PLANOTRABALHOID = lr.PLANOTRABALHOID
                               left join PrestacaoContas.OPERACAOEXIGENCIA e on lr.OPERACAOID = e.OPERACAOID 
                               WHERE lr.tipooperacao ='C' and lr.STATUS = @STATUS and 
                                     lr.PLANOTRABALHOID = @PLANOTRABALHOID ");
                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" and lr.CENSO = @CENSO ");
                }
                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" and lr.PERIODOREFERENCIAID = @PERIODOREFERENCIAID    ");
                }

                sql.Append(@" select count(operacaoid) as total from #temp
                                where (OPERACAOEXIGENCIAID is null and APROVADO is null)
                                or (OPERACAOEXIGENCIAID is not null and APROVADO=1)

                                End");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.VarChar, PlanoTrabalho);
                contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, status);

                if (!UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, UnidadeEnsino);
                }
                if (!PeriodoPrestacaoContas.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.VarChar, PeriodoPrestacaoContas);
                }

                reader = ctx.GetDataReader(contextQuery);
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["total"]);
                }

                return id; return id;
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
        public int TotaldeOperacoesCredito(string UnidadeEnsino, string PlanoTrabalho, string PeriodoPrestacaoContas, string status)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return TotaldeOperacoesCredito(ctx, UnidadeEnsino, PlanoTrabalho, PeriodoPrestacaoContas, status);
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
        }
        private PrestacaoContas.Entidades.Operacao ListaOperacaoPor(DataContext contexto, string periodoreferencia, string planodetrabalho, string censo)
        {
            PrestacaoContas.Entidades.Operacao retOperacao = new PrestacaoContas.Entidades.Operacao();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @"  SELECT * FROM prestacaocontas.Operacao lr 
                                             WHERE lr.PERIODOREFERENCIAID = @PERIODOREFERENCIAID  and
                                                    lr.PLANOTRABALHOID = @PLANOTRABALHOID  and
                                                    lr.CENSO = @CENSO  ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoreferencia);
                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planodetrabalho);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retOperacao.PlanoTrabalhoId = Convert.ToInt32(reader["PLANOTRABALHOID"]);
                    retOperacao.Justificativa = Convert.ToString(reader["JUSTIFICATIVA"]);
                }


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

            return retOperacao;
        }


        private bool ExisteOperacaoPor(DataContext ctx, int periodoreferencia, int planodetrabalho, string censo, string operacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.OPERACAO (NOLOCK) 
                                      where PERIODOREFERENCIAID = @PERIODOREFERENCIAID and
                                            PLANOTRABALHOID = @PLANOTRABALHOID and
                                            CENSO = @CENSO and
                                            TIPOOPERACAO = @TIPOOPERACAO  ";

            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoreferencia);
            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planodetrabalho);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@TIPOOPERACAO", SqlDbType.VarChar, operacao);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
        public bool ExisteOperacaoPor(int periodoreferencia, int planodetrabalho, string censo, string operacao)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return ExisteOperacaoPor(ctx, periodoreferencia, planodetrabalho, censo, operacao);
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
        }

        public DataTable ObtemOperacaoPor(int eventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery query = new ContextQuery();
                query.Command = @"
                    select * from [LYCEUM].[PrestacaoContas].[OPERACAO]
                    where OPERACAOID = @OPERACAOID    ";

                query.Parameters.Add("@OPERACAOID", SqlDbType.Int, eventoId);

                return ctx.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Entidades.Operacao ObtemPor(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ObtemPor(contexto, eventoId);
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
        public Entidades.Operacao ObtemPor(string eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ObtemPor(contexto, eventoId);
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
        public bool PossuiExigenciasNaoAnalisadas(int extratoBancarioId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return PossuiExigenciasNaoAnalisadas(ctx, extratoBancarioId);
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
        }
        public void AtualizaStatusReprovado(int operacaoId, int? status, string motivo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery query = new ContextQuery();

                    query.Command = @"
                         update PrestacaoContas.OPERACAO
                         set STATUS =  @STATUS
                             ,MOTIVOREPROVACAOOPERACAOID =  @MOTIVOREPROVACAOOPERACAOID
                             ,USUARIOID = @USUARIOID
                             ,DATAALTERACAO = @DATAALTERACAO
                        where OPERACAOID = @OPERACAOID
                    ";

                    query.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);
                    query.Parameters.Add("@STATUS", SqlDbType.Int, (object)status ?? DBNull.Value);
                    query.Parameters.Add("@MOTIVOREPROVACAOOPERACAOID", SqlDbType.Int, (object)motivo ?? DBNull.Value);
                    query.Parameters.Add("@USUARIOID", SqlDbType.VarChar, System.Web.HttpContext.Current.User.Identity.Name);
                    query.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    ctx.ApplyModifications(query);
                }
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
        }

        public void AtualizaStatus(int operacaoId, int? status)
        {
            try
            {
                var complemento = "";
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery query = new ContextQuery();
                    if (status == 3)
                    {
                        var ano = DateTime.Now.Year;

                        var codoperacao = ano.ToString() + RetornaTipoOperacao(operacaoId) + operacaoId.ToString().PadLeft(6, '0');

                        query.Command = @"
                         update PrestacaoContas.OPERACAO
                         set STATUS =  @STATUS
                             ,USUARIOID = @USUARIOID
                             ,DATAALTERACAO = @DATAALTERACAO
                             ,CODOPERACAO =  @CODOPERACAO 
                        where OPERACAOID = @OPERACAOID ";
                        query.Parameters.Add("@CODOPERACAO", SqlDbType.VarChar, codoperacao.ToString());
                        query.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);
                        query.Parameters.Add("@STATUS", SqlDbType.Int, (object)status ?? DBNull.Value);
                        query.Parameters.Add("@USUARIOID", SqlDbType.VarChar, System.Web.HttpContext.Current.User.Identity.Name);
                        query.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                        ctx.ApplyModifications(query);
                    }
                    else
                    {
                        query.Command = @"
                         update PrestacaoContas.OPERACAO
                         set STATUS =  @STATUS
                             ,USUARIOID = @USUARIOID
                             ,DATAALTERACAO = @DATAALTERACAO
                        where OPERACAOID = @OPERACAOID
                    ";
                        query.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);
                        query.Parameters.Add("@STATUS", SqlDbType.Int, (object)status ?? DBNull.Value);
                        query.Parameters.Add("@USUARIOID", SqlDbType.VarChar, System.Web.HttpContext.Current.User.Identity.Name);
                        query.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                        ctx.ApplyModifications(query);
                    }



                }
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
        }

        public void ExcluiOperacao(int operacaoId, string motivo)
        {
            OperacaoExclusao rnOperacaoExclusao = new OperacaoExclusao();
            OperacaoDocumentos rnOperacaoDocumentos = new OperacaoDocumentos();
            OperacaoExigencia rnOperacaoExigencia = new OperacaoExigencia();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //obtem usuario
                string usuarioid = System.Web.HttpContext.Current.User.Identity.Name;

                //insere operação no historico
                rnOperacaoExclusao.InsereOperacaoHistorico(contexto, operacaoId, motivo, usuarioid);

                //insere documentos operação no historico
                rnOperacaoExclusao.InsereOperacaoDocumentosHistorico(contexto, operacaoId);

                //insere exigencias operação no historico
                rnOperacaoExclusao.InsereOperacaoExigenciaHistorico(contexto, operacaoId);

                //Insere auditoria de documento 
                rnOperacaoDocumentos.InsereAuditoriaPorOperacao(contexto, operacaoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName, usuarioid);

                //deleta documentos
                rnOperacaoDocumentos.RemoveDocumentos(contexto, operacaoId);

                //deleta exgencias
                rnOperacaoExigencia.RemoveExigencia(contexto, operacaoId);

                //deleta operacao
                this.Remove(contexto, operacaoId);
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

        public bool VerificaEnvioSEI(int ocorrenciaId)
        {

            var operacao = ObtemOperacaoPor(ocorrenciaId);

            var row = operacao.Rows[0];
            var periodoreferencia = Convert.ToInt32(row["PERIODOREFERENCIAID"]);
            var censo = Convert.ToString(row["CENSO"]);



            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            contextQuery.Command = @"SELECT count([IMPORTACAOSEIID]) as existe
                                     FROM [LYCEUM].[PrestacaoContas].[IMPORTACAOSEI]
                                     where [CENSO] = @CENSO and
		                                    [PERIODOREFERENCIAID] = @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoreferencia);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["existe"]) != 0)
                    return true;
                else
                    return false;
            }

            return true;
        }

        public bool VerificaEnvioSEI(int periodoreferencia, string censo)
        {

            //    var operacao = ObtemOperacaoPor(ocorrenciaId);

            //   var row = operacao.Rows[0];
            //   var periodoreferencia = Convert.ToInt32(row["PERIODOREFERENCIAID"]);
            //  var censo = Convert.ToString(row["CENSO"]);



            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            contextQuery.Command = @"SELECT count([IMPORTACAOSEIID]) as existe
                                     FROM [LYCEUM].[PrestacaoContas].[IMPORTACAOSEI]
                                     where [CENSO] = @CENSO and
		                                    [PERIODOREFERENCIAID] = @PERIODOREFERENCIAID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, periodoreferencia);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["existe"]) != 0)
                    return true;
                else
                    return false;
            }

            return true;
        }

        public string RetornaStatus(int operacaoId)
        {
            SqlDataReader reader = null;
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                var id = "";
                contextQuery.Command = @" SELECT status 
                                      FROM   PrestacaoContas.OPERACAO (NOLOCK) 
                                      WHERE  OPERACAOID = @OPERACAOID  ";

                contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    id = reader["status"].ToString();
                }

                return id;
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
        public string RetornaStatusReprovado(int operacaoId)
        {
            SqlDataReader reader = null;
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                var id = "";
                contextQuery.Command = @" SELECT m.DESCRICAO
                                         FROM PrestacaoContas.OPERACAO o
                                         inner join PrestacaoContas.MOTIVOREPROVACAOOPERACAO m on o.MOTIVOREPROVACAOOPERACAOID = m.MOTIVOREPROVACAOOPERACAOID
                                         where status = 4 
                                         and  o.OPERACAOID = @OPERACAOID ";

                contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    id = reader["DESCRICAO"].ToString();
                }

                return id;
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
        public string RetornaTipoOperacao(int operacaoId)
        {
            SqlDataReader reader = null;
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                var id = "";
                contextQuery.Command = @" SELECT TIPOOPERACAO 
                                      FROM   PrestacaoContas.OPERACAO (NOLOCK) 
                                      WHERE  OPERACAOID = @OPERACAOID  ";

                contextQuery.Parameters.Add("@OPERACAOID", SqlDbType.Int, operacaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    id = reader["TIPOOPERACAO"].ToString();
                }

                return id;
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
        public void AtualizaStatusExigencia(int extratoBancarioId, int? status)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    ContextQuery query = new ContextQuery();

                    query.Command = @"
                        update PrestacaoContas.OPERACAOEXIGENCIA set
                            STATUS = @STATUS
                            ,USUARIOID = @USUARIOID
                            ,DATAALTERACAO = @DATAALTERACAO
                        where OPERACAOID = @EXTRATOBANCARIOID
                    ";

                    query.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);
                    query.Parameters.Add("@STATUS", SqlDbType.Int, (object)status ?? DBNull.Value);
                    query.Parameters.Add("@USUARIOID", SqlDbType.VarChar, System.Web.HttpContext.Current.User.Identity.Name);
                    query.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                    ctx.ApplyModifications(query);
                }
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
        }

        public bool EstaComJustificativasEmBranco(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.OPERACAOEXIGENCIA (NOLOCK) 
                                      WHERE  OPERACAOID = @EXTRATOBANCARIOID 
                                      AND isnull(JUSTIFICATIVA, '') = '' ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
        public bool PossuiExigenciasNaoAnalisadas(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.OPERACAOEXIGENCIA (NOLOCK) 
                                      WHERE  OPERACAOID = @EXTRATOBANCARIOID 
                                      AND APROVADO IS NULL ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool PossuiExigenciasReprovadas(int extratoBancarioId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return PossuiExigenciasReprovadas(ctx, extratoBancarioId);
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
        }

        public bool PossuiExigenciasReprovadas(DataContext ctx, int extratoBancarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   PrestacaoContas.OPERACAOEXIGENCIA (NOLOCK) 
                                      WHERE  OPERACAOID = @EXTRATOBANCARIOID 
                                      AND APROVADO = 0 ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancarioId);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
        public Entidades.Operacao ObtemPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select * from PrestacaoContas.OPERACAO (nolock)                    
                    where OPERACAOID = @EVENTOID ";
                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
                return contexto.TryToBindEntity<Entidades.Operacao>(contextQuery);
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
        }
        public Entidades.Operacao ObtemPor(DataContext contexto, string eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select * from PrestacaoContas.OPERACAO (nolock)                    
                    where CODOPERACAO = @EVENTOID ";
                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.VarChar, eventoId);
                return contexto.TryToBindEntity<Entidades.Operacao>(contextQuery);
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
        }
        public int Insere(DataContext contexto, Entidades.Operacao Operacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.Operacao
                                           (PERIODOREFERENCIAID
                                            ,PLANOTRABALHOID
                                            ,CENSO
                                            ,TIPOOPERACAO
                                            ,VALOR
                                            ,JUSTIFICATIVA
                                            ,STATUS
                                            ,USUARIOID
                                            ,DATACADASTRO
                                           )
                                     VALUES
	                                       (@PERIODOREFERENCIAID
                                            ,@PLANOTRABALHOID
                                            ,@CENSO
                                            ,@TIPOOPERACAO
                                            ,@VALOR
                                            ,@JUSTIFICATIVA
                                            ,@STATUS
                                            ,@USUARIOID
                                            ,@DATACADASTRO                                 
                                          )

                         SELECT IDENT_CURRENT('PrestacaoContas.Operacao') ";

            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, Operacao.PeriodoReferenciaId);
            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, Operacao.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, Operacao.Censo);
            contextQuery.Parameters.Add("@TIPOOPERACAO", SqlDbType.VarChar, Operacao.TipoOperacao);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, Operacao.Valor);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, Operacao.Justificativa);
            contextQuery.Parameters.Add("@STATUS", SqlDbType.Int, Operacao.Status);
            // contextQuery.Parameters.Add("@DATAANALISE", SqlDbType.DateTime, Operacao.DataAnalise);
            //  contextQuery.Parameters.Add("@MOTIVOREPROVACAOOPERACAOID", SqlDbType.Int, Operacao.MotivoReprovacaoOperacaoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, Operacao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            return Operacao.OperacaoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados Valida(Entidades.Operacao Operacao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            AnaliseRepasse rnAnaliseRepasse = new AnaliseRepasse();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (Operacao == null)
            {
                return validacaoDados;
            }


            if (Operacao.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo obrigatório UNIDADE ENSINO não foi preenchido  ");
            }

            if (Operacao.Valor <= 0)
            {
                mensagens.Add("O campo obrigatório VALOR deve ser preenchido e não pode ser negativo nem zero");
            }
            // if (Operacao.OperacaoId == 0)
            //    {
            //    if (VerificaEnvioSEI(Convert.ToInt32(Operacao.PeriodoReferenciaId), Operacao.Censo))
            //        mensagens.Add("A operação não pode ser cadastrada/alterada, pois o Formulário SEI já foi gerado.");
            //    }
            //  else
            //     if (VerificaEnvioSEI(Convert.ToInt32(Operacao.OperacaoId)))
            //         mensagens.Add("Formulário não pode ser adicionado/alterado, pois já foi enviado ao SEI.");

            if (Operacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUARIO não foi preenchido");
            }
            if (Operacao.TipoOperacao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo OPERAÇÃO não foi preenchido");
            }
            if (string.IsNullOrEmpty(Operacao.Valor.ToString()))
            {
                mensagens.Add("O campo VALOR não foi preenchido");
            }
            if (Convert.ToDecimal(Operacao.Valor.ToString()) == 0)
            {
                mensagens.Add("O campo VALOR não foi preenchido");
            }

            if (Operacao.Justificativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo JUSTIFICATIVA não foi preenchido");
            }

            if (Operacao.Justificativa.Length > 500)
            {
                mensagens.Add("O campo JUSTIFICATIVA está com mais caracteres que o permitido");
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


        public ValidacaoDados ValidaRemocao(int OperacaoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            AnaliseRepasse rnAnaliseRepasse = new AnaliseRepasse();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (OperacaoId <= 0)
            {
                mensagens.Add(" O campo obrigatório CODIGO não foi preenchido");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o repasse já foi aprovado
                    if (rnAnaliseRepasse.EhAprovadoPor(contexto, OperacaoId))
                    {
                        mensagens.Add("Repasses que já tenha sido “aprovados” não poderão ser excluídos.");
                    }

                    //Verifica se o repasse já foi ntegrados com o processo de integração
                    if (rnAnaliseRepasse.EhRepasseIntegradoComSEFAZ(contexto, OperacaoId))
                    {
                        mensagens.Add("Este repasse não pode ser mais excluido pois já foi faturado");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public int Insere(Entidades.Operacao Operacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            int valor = 0;
            try
            {
                //Insere dados
                valor = this.Insere(contexto, Operacao);

                return valor;
                //Atualiza todos os numeros de processos
                //    this.AtualizaNumeroProcessoRepasse(contexto, Operacao);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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
            return valor;
        }

        public void Atualiza(Entidades.Operacao Operacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza dados
                this.Atualiza(contexto, Operacao);

                //Atualiza todos os numeros de processos
                //   this.AtualizaNumeroProcessoRepasse(contexto, Operacao);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    if (Convert.ToString(ex.Message).Contains("ERRO:"))
                    {
                        mensagem = ex.Message.Replace("ERRO: ", string.Empty);
                    }
                    else
                    {
                        mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine,
                            Convert.ToString(ex.Message));
                    }
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

        private void Atualiza(DataContext contexto, Entidades.Operacao Operacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.Operacao
                                       SET
										JUSTIFICATIVA = @JUSTIFICATIVA,
										VALOR = @VALOR,
                                        USUARIOID = @USUARIOID,
                                        DATAALTERACAO = @DATAALTERACAO
                                     WHERE OperacaoID = @OperacaoID ";

            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, Operacao.Justificativa);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, Operacao.Valor);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, Operacao.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@OperacaoID", SqlDbType.Int, Operacao.OperacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(int operacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Remove(contexto, operacaoId);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
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
        }

        private void Remove(DataContext contexto, int operacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FROM PrestacaoContas.Operacao 
                                        WHERE  OperacaoID = @OperacaoID ";

            contextQuery.Parameters.Add("@OperacaoID", SqlDbType.Int, operacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiOperacaoPor(DataContext contexto, string censo, int itemPlanilhaId, int OperacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[Operacao] 
                        WHERE  CENSO = @CENSO
                               AND ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID
                               AND OperacaoID <> @OperacaoID
                                ";


            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaId);
            contextQuery.Parameters.Add("@OperacaoID", SqlDbType.Int, OperacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void ImportaArquivo(IList<Entidades.Operacao> lancamentosRepasse, out decimal valorTotalImportado, out int totalItensImportados)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                valorTotalImportado = (decimal)0;
                totalItensImportados = 0;

                ImportaArquivo(contexto, lancamentosRepasse, out valorTotalImportado, out totalItensImportados);
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
        }

        private void ImportaArquivo(DataContext contexto, IList<Entidades.Operacao> lancamentosRepasse, out decimal valorTotalImportado, out int totalItensImportados)
        {
            valorTotalImportado = (decimal)0;
            totalItensImportados = 0;

            var primeiroLancamentoDaLista = lancamentosRepasse.FirstOrDefault();
            if (primeiroLancamentoDaLista == null)
                return;

            try
            {
                foreach (var lr in lancamentosRepasse)
                {
                    valorTotalImportado += lr.Valor;
                    totalItensImportados++;
                    Insere(contexto, lr);
                }

                //   InsereRelatorio(contexto, primeiroLancamentoDaLista.ItemPlanilhaOrcamentariaId, valorTotalImportado, totalItensImportados, primeiroLancamentoDaLista.UsuarioId);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    msgErro += Environment.NewLine + ex.Message;
                }

                throw new Exception(msgErro);
            }
        }

        private void InsereRelatorio(DataContext contexto, int itemPlanilhaOrcamentariaId, decimal valorTotalImportado, int totalItensImportados, string usuario)
        {
            //insere o relatorio orcamentaria
            ContextQuery contextQueryIPO = new ContextQuery();
            contextQueryIPO.Command = @"insert into [LYCEUM].[PrestacaoContas].[IMPORTACAOREPASSE] (ITEMPLANILHAORCAMENTARIAID, VALORTOTALIMPORTADO  ,TOTALITENSIMPORTADOS , USUARIOIMPORTACAO) values
                                                                                                       (@ITEMPLANILHAORCAMENTARIAID, @VALORTOTALIMPORTADO ,@TOTALITENSIMPORTADOS, @USUARIO )";
            contextQueryIPO.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);
            contextQueryIPO.Parameters.Add("@VALORTOTALIMPORTADO", SqlDbType.Decimal, valorTotalImportado);
            contextQueryIPO.Parameters.Add("@TOTALITENSIMPORTADOS", SqlDbType.Int, totalItensImportados);
            contextQueryIPO.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);

            contexto.ApplyModifications(contextQueryIPO);
        }
    }
}