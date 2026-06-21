using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class AnalisePlanilhaOrcamentaria
    {
        public Entidades.AnalisePlanilhaOrcamentaria ObtemPor(int planilhaOrcamentariaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.AnalisePlanilhaOrcamentaria analise = new Entidades.AnalisePlanilhaOrcamentaria();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select top 1 * 
                                        from  prestacaocontas.ANALISEPLANILHAORCAMENTARIA 
						                where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID
										order by ANALISEPLANILHAORCAMENTARIAID desc ";

                contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);

                analise = contexto.TryToBindEntity<Entidades.AnalisePlanilhaOrcamentaria>(contextQuery);

                return analise;
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

        public bool PossuiMotivoReprovacaoPlanilhaOrcamentariaPor(DataContext contexto, int motivoReprovacaoPlanilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.ANALISEPLANILHAORCAMENTARIA (NOLOCK)
                                    WHERE MOTIVOREPROVACAOPLANILHAORCAMENTARIAID = @MOTIVOREPROVACAOPLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@MOTIVOREPROVACAOPLANILHAORCAMENTARIAID", SqlDbType.Int, motivoReprovacaoPlanilhaOrcamentariaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiAnalisePor(DataContext contexto, int planilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.ANALISEPLANILHAORCAMENTARIA (NOLOCK)
                                    WHERE PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

//        public bool PossuiProgramacaoOrcamentariaPor(int PlanilhaOrcamentariaId)
//        {
//            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
//            ContextQuery contextQuery = new ContextQuery();
//            bool existe = false;
//            DataTable dt = new DataTable();

//            contextQuery.Command = @" select APROVADA from  prestacaocontas.ANALISEPLANILHAORCAMENTARIA 
//						              where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

//            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, PlanilhaOrcamentariaId);

//            dt = contexto.GetDataTable(contextQuery);

//            if (dt != null)
//            {
//                if (dt.Rows != null && dt.Rows.Count > 0 )
//                {
//                    if (dt.Rows[0].ItemArray != null && dt.Rows[0].ItemArray.Count() > 0)
//                    {
//                        if ((bool)dt.Rows[0].ItemArray[0] == true)
//                        {
//                            existe = true;
//                        }
//                    }
//                }
//            }

//            //if (contexto.GetReturnValue<int>(contextQuery) > 0)
//            //{
//            //    existe = true;
//            //}

//            return existe;
//        }
        
//        public String RetornaMotivoPor(int PlanilhaOrcamentariaId)
//        {
//            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
//            ContextQuery contextQuery = new ContextQuery();
//            string retorno = "";
//            DataTable dt = new DataTable();

//            contextQuery.Command = @" select m.DESCRICAO from  prestacaocontas.ANALISEPLANILHAORCAMENTARIA a
//                                      join PrestacaoContas.MOTIVOREPROVACAOPLANILHAORCAMENTARIA m 
//                                      on a.MOTIVOREPROVACAOPLANILHAORCAMENTARIAID = m.MOTIVOREPROVACAOPLANILHAORCAMENTARIAID
//                                      where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID  ";

//            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, PlanilhaOrcamentariaId);

//            dt = contexto.GetDataTable(contextQuery);

//            if (dt != null)
//            {
//                if (dt.Rows != null && dt.Rows.Count > 0)
//                {
//                    if (dt.Rows[0].ItemArray != null && dt.Rows[0].ItemArray.Count() > 0)
//                    {
//                        if (!String.IsNullOrEmpty( dt.Rows[0].ItemArray[0].ToString() ))
//                        {
//                            retorno = dt.Rows[0].ItemArray[0].ToString();
//                        }
//                    }
//                }
//            }

//            return retorno;
//        }

        public DataTable ListaDadosGridPor(int ano, string processo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT   PO.PLANILHAORCAMENTARIAID,
                                                   PO.ANO,
                                                   PO.PROCESSO,
                                                   PO.DESCRICAO,
	                                               PT.DESCRICAO AS PLANOTRABALHOID,
                                                   (SELECT DESCRICAO
                                                    FROM   PRESTACAOCONTAS.NATUREZADESPESA
                                                    WHERE  NATUREZADESPESAID = PO.NATUREZADESPESAID) AS NATUREZADESPESAID,
                                                   (SELECT DESCRICAO
                                                    FROM   GESTAOREDE.REGIAOFINANCEIRA
                                                    WHERE  REGIAOFINANCEIRAID = PO.REGIAOFINANCEIRAID) AS REGIAOFINANCEIRAID,
                                                   (SELECT SUM(VALOR)
                                                    FROM   PRESTACAOCONTAS.ITEMPLANILHAORCAMENTARIA IPO
                                                    WHERE  IPO.PLANILHAORCAMENTARIAID = PO.PLANILHAORCAMENTARIAID) AS VALORTOTAL,
                                                   CASE
                                                     WHEN AP.APROVADA IS NULL THEN 'SELECIONE'
                                                     WHEN AP.APROVADA = 0 THEN 'N'
                                                     ELSE 'S'
                                                   END AS ACAO,
                                                   AP.MOTIVOREPROVACAOPLANILHAORCAMENTARIAID AS MOTIVOREPROVACAO,
                                                   AP.ANALISEPLANILHAORCAMENTARIAID,
                                                   PROT.PROGRAMATRABALHOID,
												   WS.PT,
												   WS.DESCRICAO AS PROGRAMATRABALHO
                                            FROM   PRESTACAOCONTAS.PLANILHAORCAMENTARIA PO
                                                   LEFT JOIN PRESTACAOCONTAS.ANALISEPLANILHAORCAMENTARIA AP
                                                          ON PO.PLANILHAORCAMENTARIAID = AP.PLANILHAORCAMENTARIAID
                                                   INNER JOIN PRESTACAOCONTAS.PLANOTRABALHO PT
                                                          ON PT.PLANOTRABALHOID = PO.PLANOTRABALHOID
                                                   INNER JOIN PRESTACAOCONTAS.PROGRAMATRABALHO PROT
                                                          ON PROT.PROGRAMATRABALHOID = PT.PROGRAMATRABALHOID
												   LEFT JOIN PRESTACAOCONTAS.WSPROGRAMASEFAZ WS
														  ON WS.WSPROGRAMASEFAZID = PROT.WSPROGRAMASEFAZID
                                            WHERE  PO.ANO = @ANO 
                                                   ");

                if (!processo.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" AND PO.PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ");
                    contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.VarChar, processo);
                }

                contextQuery.Command = sql.ToString();
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);                

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

        public DataTable ListaDadosGridPorAno(int ano)
        {
            return this.ListaDadosGridPor(ano, string.Empty);
        }

        public void Insere(Entidades.AnalisePlanilhaOrcamentaria analisePlanilhaOrcamentaria)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.ANALISEPLANILHAORCAMENTARIA
                                                        (PLANILHAORCAMENTARIAID, 
                                                         MOTIVOREPROVACAOPLANILHAORCAMENTARIAID, 
                                                         APROVADA, 
                                                         USUARIOID,														 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@PLANILHAORCAMENTARIAID, 
                                                         @MOTIVOREPROVACAOPLANILHAORCAMENTARIAID, 
														 @APROVADA,
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, analisePlanilhaOrcamentaria.PlanilhaOrcamentariaId);
                contextQuery.Parameters.Add("@MOTIVOREPROVACAOPLANILHAORCAMENTARIAID", SqlDbType.Int, analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId == null || analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId <= 0 ? (object)DBNull.Value : analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId);
                contextQuery.Parameters.Add("@APROVADA", SqlDbType.Bit, analisePlanilhaOrcamentaria.Aprovada);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, analisePlanilhaOrcamentaria.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public void InsereHistorico(DataContext contexto, int planilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.ANALISEPLANILHAORCAMENTARIAHISTORICO
                                           (ANALISEPLANILHAORCAMENTARIAID
                                           ,PLANILHAORCAMENTARIAID
                                           ,MOTIVOREPROVACAOPLANILHAORCAMENTARIAID
                                           ,APROVADA
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     select ANALISEPLANILHAORCAMENTARIAID
                                           ,PLANILHAORCAMENTARIAID
                                           ,MOTIVOREPROVACAOPLANILHAORCAMENTARIAID
                                           ,APROVADA
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO
	                                 from PrestacaoContas.ANALISEPLANILHAORCAMENTARIA
	                                 where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int planilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" delete PrestacaoContas.ANALISEPLANILHAORCAMENTARIA
	                                 where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);

            contexto.ApplyModifications(contextQuery);
        }

        public bool EhAprovadaPor(int planilhaOrcamentariaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.EhAprovadaPor(contexto, planilhaOrcamentariaId);
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

        private bool EhAprovadaPor(DataContext contexto, int planilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.ANALISEPLANILHAORCAMENTARIA (NOLOCK)
                                    WHERE PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID
                                          AND APROVADA = 1 ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Entidades.AnalisePlanilhaOrcamentaria analisePlanilhaOrcamentaria, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (analisePlanilhaOrcamentaria == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (analisePlanilhaOrcamentaria.AnalisePlanilhaOrcamentariaId <= 0)
                {
                    mensagens.Add(" O campo obrigatório CODIGO Não foi preenchido.");
                }
            }

            if (analisePlanilhaOrcamentaria.Ano <= 0)
            {
                mensagens.Add(" O campo obrigatório ANO Não foi preenchido.");
            }

            if (analisePlanilhaOrcamentaria.PlanilhaOrcamentariaId <= 0)
            {
                mensagens.Add(" O campo obrigatório PROGRAMAÇÃO ORÇAMENTÁRIA Não foi preenchido.");
            }

            if (!analisePlanilhaOrcamentaria.Aprovada &&
                (analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId == null || analisePlanilhaOrcamentaria.Ano <= 0))
            {
                mensagens.Add(" O campo obrigatório MOTIVO Não foi preenchido ");
            }

            if (analisePlanilhaOrcamentaria.Aprovada &&
                (analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId != null && analisePlanilhaOrcamentaria.Ano > 0))
            {
                mensagens.Add(" O campo obrigatório MOTIVO não pode ser preenchido quando a situação for aprovado.");
            }

            if(analisePlanilhaOrcamentaria.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add(" O campo obrigatório USUARIO Não foi preenchido ");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (analisePlanilhaOrcamentaria.Aprovada)
                    {
                        analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId = null;
                    }

                    //Verifica se a Programação Orçamentária já foi aprovada
                    if(this.EhAprovadaPor(contexto, analisePlanilhaOrcamentaria.PlanilhaOrcamentariaId))
                    {
                        mensagens.Add("Esta Programação Orçamentária já foi aprovada, e com isso não pode mais ser modificada.");
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

        public void Atualiza(Entidades.AnalisePlanilhaOrcamentaria analisePlanilhaOrcamentaria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.ANALISEPLANILHAORCAMENTARIA
                                       SET APROVADA = @APROVADA,
										    MOTIVOREPROVACAOPLANILHAORCAMENTARIAID = @MOTIVOREPROVACAOPLANILHAORCAMENTARIAID,										
                                            DATAALTERACAO = @DATAALTERACAO,
                                            USUARIOID = @USUARIOID
                                     WHERE ANALISEPLANILHAORCAMENTARIAID = @ANALISEPLANILHAORCAMENTARIAID ";

                contextQuery.Parameters.Add("@ANALISEPLANILHAORCAMENTARIAID", SqlDbType.Int, analisePlanilhaOrcamentaria.AnalisePlanilhaOrcamentariaId);
                contextQuery.Parameters.Add("@APROVADA", SqlDbType.Int, analisePlanilhaOrcamentaria.Aprovada);
                contextQuery.Parameters.Add("@MOTIVOREPROVACAOPLANILHAORCAMENTARIAID", SqlDbType.Int, analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId == null || analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId <= 0 ? (object)DBNull.Value : analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, analisePlanilhaOrcamentaria.UsuarioId);
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
    }
}
