using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class AnaliseContaCorrente
    {
        public bool PossuiContaCorrentePor(DataContext contexto, int contaCorrenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.ANALISECONTACORRENTE (NOLOCK)
                                    WHERE CONTACORRENTEID = @CONTACORRENTEID ";

            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMotivoReprovacaoContaCorrentePor(DataContext contexto, int MotivoReprovacaoContaCorrenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.ANALISECONTACORRENTE (NOLOCK)
                                    WHERE MOTIVOREPROVACAOCONTACORRENTEID = @MOTIVOREPROVACAOCONTACORRENTEID ";

            contextQuery.Parameters.Add("@MOTIVOREPROVACAOCONTACORRENTEID", SqlDbType.Int, MotivoReprovacaoContaCorrenteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutraContaCorrentePor(DataContext contexto, int contaCorrenteId, int analiseContaCorrenteId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" select count(*) from PrestacaoContas.ANALISECONTACORRENTE where CONTACORRENTEID = @CONTACORRENTEID
                                      and ANALISECONTACORRENTEID <> @ANALISECONTACORRENTEID ";

            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, contaCorrenteId);
            contextQuery.Parameters.Add("@ANALISECONTACORRENTEID", SqlDbType.Int, analiseContaCorrenteId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaAnaliseContaCorrentePor(string situacao, string tipo, string usuarioId)
        {
            DataTable dt = null;

            if (tipo.ToUpper() == "U")//Unidade Ensino
            {
                dt = this.ListaAnaliseContaCorrenteUnidadePor(situacao, usuarioId);
            }
            else if (tipo.ToUpper() == "R")//Regional
            {
                dt = this.ListaAnaliseContaCorrenteRegionalPor(situacao, usuarioId);
            }

            return dt;
        }

        private DataTable ListaAnaliseContaCorrenteUnidadePor(string situacao, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT
                              lyu.NOME_COMP,
                              cc.*,
                              tcr.REGIONAL,
                              acc.analisecontacorrenteid,
                              mrc.DESCRICAO AS MOTIVOREPROVACAOCONTACORRENTEID,
                              acc.USUARIOAPROVACAOID,
                              acc.DATAAPROVACAO,
                              CASE
                                WHEN acc.aprovado IS NULL THEN 'Pendente'
                                WHEN acc.aprovado = 1 THEN 'Aprovado'
                                WHEN acc.aprovado = 0 THEN 'Reprovado'
                              END situacao,
                              'U' as tipo,
                              acc.aprovado
                            FROM [PrestacaoContas].[CONTACORRENTE] cc
                            INNER JOIN LY_UNIDADE_ENSINO lyu
                              ON lyu.UNIDADE_ENS = cc.CENSO
                            INNER JOIN LY_USUARIO_UNIDADE_FIS UU 
	                            ON UU.UNIDADE_FIS = cc.CENSO
	                            AND UU.USUARIO = @USUARIO
                            inner JOIN TCE_REGIONAL tcr
                              ON tcr.ID_REGIONAL = lyu.ID_REGIONAL
                            LEFT JOIN PrestacaoContas.ANALISECONTACORRENTE acc
                              ON cc.CONTACORRENTEID = acc.CONTACORRENTEID
                            LEFT JOIN PrestacaoContas.MOTIVOREPROVACAOCONTACORRENTE mrc
                              ON mrc.MOTIVOREPROVACAOCONTACORRENTEID = acc.MOTIVOREPROVACAOCONTACORRENTEID ");

                if (situacao.ToUpper() == "APROVADO")
                {
                    sql.Append(@"
                            WHERE ACC.APROVADO = 1 
                            ");
                }
                else if (situacao.ToUpper() == "REPROVADO")
                {
                    sql.Append(@"
                            where acc.aprovado = 0 
                            ");
                }
                else //"PENDENTE"
                {
                    sql.Append(@"
                            where acc.aprovado IS NULL 
                            ");
                }

                sql.Append("ORDER BY REGIONAL, NOME_COMP");

                contextQuery.Command = sql.ToString();
                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioId);

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

        private DataTable ListaAnaliseContaCorrenteRegionalPor(string situacao, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT
                              NULL AS NOME_COMP,
                              cc.*,
                              tcr.REGIONAL,
                              acc.analisecontacorrenteid,
                              mrc.DESCRICAO AS MOTIVOREPROVACAOCONTACORRENTEID,
                              acc.USUARIOAPROVACAOID,
                              acc.DATAAPROVACAO,
                              CASE
                                WHEN acc.aprovado IS NULL THEN 'Pendente'
                                WHEN acc.aprovado = 1 THEN 'Aprovado'
                                WHEN acc.aprovado = 0 THEN 'Reprovado'
                              END situacao,
                              'R' as tipo,
                              acc.aprovado
                            FROM [PRESTACAOCONTAS].[CONTACORRENTE] CC
                            INNER JOIN TCE_REGIONAL TCR
                              ON TCR.ID_REGIONAL = CC.REGIONALID
                            INNER JOIN LY_UNIDADE_ENSINO LYU
                              ON LYU.ID_REGIONAL = TCR.ID_REGIONAL
                            INNER JOIN LY_USUARIO_UNIDADE_FIS UU 
	                            ON UU.UNIDADE_FIS = LYU.UNIDADE_ENS
	                            AND UU.USUARIO = @USUARIO
                            LEFT JOIN PRESTACAOCONTAS.ANALISECONTACORRENTE ACC
                              ON CC.CONTACORRENTEID = ACC.CONTACORRENTEID
                            LEFT JOIN PRESTACAOCONTAS.MOTIVOREPROVACAOCONTACORRENTE MRC
                              ON MRC.MOTIVOREPROVACAOCONTACORRENTEID = ACC.MOTIVOREPROVACAOCONTACORRENTEID ");

                if (situacao.ToUpper() == "APROVADO")
                {
                    sql.Append(@"
                            WHERE ACC.APROVADO = 1 
                            ");
                }
                else if (situacao.ToUpper() == "REPROVADO")
                {
                    sql.Append(@"
                            where acc.aprovado = 0 
                            ");
                }
                else //"PENDENTE"
                {
                    sql.Append(@"
                            where acc.aprovado IS NULL 
                            ");
                }

                sql.Append("ORDER BY REGIONAL, NOME_COMP");

                contextQuery.Command = sql.ToString();
                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioId);

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

        public void Atualiza(Entidades.AnaliseContaCorrente analiseContaCorrente)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {

                contextQuery.Command = @" UPDATE PrestacaoContas.ANALISECONTACORRENTE 
										   SET APROVADO = @APROVADO,                                                         
                                               MOTIVOREPROVACAOCONTACORRENTEID = @MOTIVOREPROVACAOCONTACORRENTEID
                                           WHERE  CONTACORRENTEID = @CONTACORRENTEID
                                 ";

                contextQuery.Parameters.Add("@APROVADO", SqlDbType.Int, analiseContaCorrente.Aprovado);
                contextQuery.Parameters.Add("@MOTIVOREPROVACAOCONTACORRENTEID", SqlDbType.VarChar, analiseContaCorrente.MotivoReprovacaoContaCorrenteId);
                contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, analiseContaCorrente.ContaCorrenteId);
                //contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                //contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        //public void Insere(DataContext contexto, Entidades.AnaliseContaCorrente AnaliseContaCorrente)
        public void Insere(Entidades.AnaliseContaCorrente AnaliseContaCorrente)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {

                contextQuery.Command = @" INSERT INTO PrestacaoContas.ANALISECONTACORRENTE 
                                            (CONTACORRENTEID,
                                                   MOTIVOREPROVACAOCONTACORRENTEID,
                                                   APROVADO,
                                                   USUARIOAPROVACAOID,
                                                   DATAAPROVACAO,
                                                   DATAALTERACAO
												                                                                                                       
) 
                                VALUES      (@CONTACORRENTEID,
                                                 @MOTIVOREPROVACAOCONTACORRENTEID,
                                                  @APROVADO,
                                                   @USUARIOAPROVACAOID,
                                                   @DATAAPROVACAO,
                                                   @DATAALTERACAO
                                                )
                                			
                                SELECT IDENT_CURRENT('PrestacaoContas.ANALISECONTACORRENTE') ";

                contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, AnaliseContaCorrente.ContaCorrenteId);
                contextQuery.Parameters.Add("@MOTIVOREPROVACAOCONTACORRENTEID", SqlDbType.Int, AnaliseContaCorrente.MotivoReprovacaoContaCorrenteId);
                contextQuery.Parameters.Add("@APROVADO", SqlDbType.Int, AnaliseContaCorrente.Aprovado);
                contextQuery.Parameters.Add("@USUARIOAPROVACAOID", SqlDbType.VarChar, AnaliseContaCorrente.UsuarioAprovacaoId);
                contextQuery.Parameters.Add("@DATAAPROVACAO", SqlDbType.DateTime, DateTime.Now.Date);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now.Date);

                //contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, "a");
                //contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                //contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                AnaliseContaCorrente.AnaliseContaCorrenteId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));

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

        public ValidacaoDados Valida(Entidades.AnaliseContaCorrente AnaliseContaCorrente, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (AnaliseContaCorrente == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            //if (!cadastro)
            //{
            //    if (AnaliseContaCorrente.ContaCorrenteId <= 0)
            //    {
            //        mensagens.Add("Campo Conta Corrente é obrigatório.");
            //    }
            //}



            if (AnaliseContaCorrente.Aprovado == null)
            {
                mensagens.Add("Campo Aprovado é obrigatório.");
            }


            if (AnaliseContaCorrente.Aprovado != true)
            {
                if (AnaliseContaCorrente.MotivoReprovacaoContaCorrenteId <= 0)
                {
                    mensagens.Add("Motivo de reprovação obrigatório para contas correntes com situação reprovadas");
                }
            }





            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                    //validacaoDados.Valido = true;

                    if (PossuiOutraContaCorrentePor(contexto, AnaliseContaCorrente.ContaCorrenteId, AnaliseContaCorrente.AnaliseContaCorrenteId))
                    {
                        mensagens.Add("Já existe aprovação para esta conta corrente ");
                    }

                    //// Verifica se já existe a descricao cadastrada
                    //if (this.PossuiOutroCadastradoPor(contexto, contaCorrente.Conta, contaCorrente.Banco, contaCorrente.Agencia))
                    //{
                    //    mensagens.Add("Já existe uma conta corrente cadastrada com este banco / conta / agência.");
                    //}

                    ////Verifica se a data de inicio está intercalada com outro
                    //if (this.PossuiDataInicioEmOutroIntervaloPor(contexto, contaCorrente.Censo, contaCorrente.RegionalId, contaCorrente.DataInicio, contaCorrente.ContaCorrenteId))
                    //{
                    //    mensagens.Add("DATA INÍCIO não pode estar dentro do intervalo de outra conta corrente dessa escola ou regional.");
                    //}

                    ////Verifica se não possui data de fim
                    //if (contaCorrente.DataFim != null && contaCorrente.DataFim > DateTime.MinValue)
                    //{
                    //    //Verifica se a data de inicio está intercalada com outro
                    //    if (this.PossuiDataFimEmOutroIntervaloPor(contexto, contaCorrente.Censo, contaCorrente.RegionalId, Convert.ToDateTime(contaCorrente.DataFim), contaCorrente.ContaCorrenteId))
                    //    {
                    //        mensagens.Add("DATA FIM não pode estar dentro do intervalo de outra conta corrente dessa escola ou regional.");
                    //    }

                    //    //Verifica se as datas de inicio e de fim estão intercalada com outro
                    //    if (this.PossuiOutraIntercaladaPor(contexto, contaCorrente.Censo, contaCorrente.RegionalId, contaCorrente.DataInicio, Convert.ToDateTime(contaCorrente.DataFim), contaCorrente.ContaCorrenteId))
                    //    {
                    //        mensagens.Add("DATA INÍCIO E FIM não podem  intercalar com outra conta corrente desta escola ou regional.");
                    //    }
                    //}
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
    }
}
