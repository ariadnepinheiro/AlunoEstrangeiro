using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class PlanilhaOrcamentaria
    {
        public bool PossuiNaturezaDespesaPor(DataContext contexto, int naturezaDespesaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.PLANILHAORCAMENTARIA (NOLOCK)
                                    WHERE NATUREZADESPESAID = @NATUREZADESPESAID ";

            contextQuery.Parameters.Add("@NATUREZADESPESAID", SqlDbType.Int, naturezaDespesaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int PesquisaIDPlanilha(string PROCESSO)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            var retorno = 0;

            contextQuery.Command = @" SELECT PLANILHAORCAMENTARIAID
                                    FROM PrestacaoContas.PLANILHAORCAMENTARIA (NOLOCK)
                                    WHERE PROCESSO = @PROCESSO ";

            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, PROCESSO);

                dt = ctx.GetDataTable(contextQuery);

                if (dt.Rows.Count ==1)
                {
                    retorno = Convert.ToInt32(dt.Rows[0][0]);
                }


                return retorno;
        }  

        public DataTable CarregaNumProcesso(int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {

                contextQuery.Command = @" SELECT PLANILHAORCAMENTARIAID, PROCESSO
                                      FROM [PRESTACAOCONTAS].[PLANILHAORCAMENTARIA]
                                      WHERE ANO = @ANO ORDER BY PROCESSO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

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

        public DataTable ListaPlanilhaOrcamentariaPor(string planilhaOrcamentariaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {

                contextQuery.Command = @" SELECT po.*, 
                                                pt.PROGRAMATRABALHOID 
                                          FROM PRESTACAOCONTAS.PLANILHAORCAMENTARIA po
                                                join PrestacaoContas.PLANOTRABALHO pt on po.PLANOTRABALHOID = pt.PLANOTRABALHOID
                                          WHERE po.PROCESSO = @PLANILHAORCAMENTARIAID";

                contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.VarChar, planilhaOrcamentariaId);

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

        public void Insere(Entidades.PlanilhaOrcamentaria planilhaOrcamentaria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.PlanilhaOrcamentaria rnPlanilhaOrcamentaria = new RN.PrestacaoContas.PlanilhaOrcamentaria();

            try
            {
                //Inserre 
                this.Insere(contexto, planilhaOrcamentaria);

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

        private void Insere(DataContext contexto, Entidades.PlanilhaOrcamentaria planilhaOrcamentaria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO  [PrestacaoContas].[PLANILHAORCAMENTARIA]
                                           ( NATUREZADESPESAID
                                           ,PLANOTRABALHOID
                                           ,REGIAOFINANCEIRAID
                                           ,PROCESSO
                                           ,DESCRICAO
                                           ,USUARIOID  
                                           ,ANO)
                                     VALUES
	                                       ( @NATUREZADESPESAID
                                           ,@PLANOTRABALHOID
                                           ,@REGIAOFINANCEIRAID
                                           ,@PROCESSO
                                           ,@DESCRICAO
                                           ,@USUARIOID
                                           ,@ANO)

                         SELECT IDENT_CURRENT('PrestacaoContas.PLANILHAORCAMENTARIA') ";

            contextQuery.Parameters.Add("@NATUREZADESPESAID", SqlDbType.Int, planilhaOrcamentaria.NaturezaDespesaId);
            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.VarChar, planilhaOrcamentaria.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.VarChar, planilhaOrcamentaria.RegiaoFinanceiraId);
            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, planilhaOrcamentaria.Processo);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, planilhaOrcamentaria.Descricao);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, planilhaOrcamentaria.Ano);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, planilhaOrcamentaria.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

            planilhaOrcamentaria.PlanilhaOrcamentariaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados Valida(Entidades.PlanilhaOrcamentaria planilhaOrcamentaria, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (planilhaOrcamentaria == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            //if (!cadastro)
            //{
            //    if (mandatoAae.MandatoAaeId <= 0)
            //    {
            //        mensagens.Add("Campo Nome é obrigatório.");
            //    }
            //}

            if (String.IsNullOrEmpty(planilhaOrcamentaria.Processo))
            {
                mensagens.Add(" O campo obrigatório Num Processo não foi preenchido");
            }
           if (planilhaOrcamentaria.Processo=="0")
            {
                mensagens.Add(" O campo obrigatório Num Processo não foi preenchido");
            }
            if (String.IsNullOrEmpty(planilhaOrcamentaria.Descricao))
            {
                mensagens.Add(" O campo obrigatório Descrição não foi preenchido");
            }
            else if (planilhaOrcamentaria.Descricao.Length > 255)
            {
                mensagens.Add(" O campo Descrição deve conter no máximo 255 caracteres");
            }

            if (planilhaOrcamentaria.ProgramaTrabalhoId == 0)
            {
                mensagens.Add(" O campo obrigatório Programa Trabalho não foi preenchido");
            }

            if (planilhaOrcamentaria.PlanoTrabalhoId == 0)
            {
                mensagens.Add(" O campo obrigatório Projeto / Programa não foi preenchido");
            }

            if (planilhaOrcamentaria.NaturezaDespesaId == 0)
            {
                mensagens.Add(" O campo obrigatório Natureza Despesa não foi preenchido");
            }

            if (planilhaOrcamentaria.RegiaoFinanceiraId == 0)
            {
                mensagens.Add(" O campo obrigatório Regiao Financeira não foi preenchido");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiProcessoAssociadoPor(contexto, planilhaOrcamentaria.Processo,planilhaOrcamentaria.PlanilhaOrcamentariaId))
                    {
                        mensagens.Add("Este processo já consta associado a outra planilha orçamentária.");
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

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiProcessoDuplicado(contexto, planilhaOrcamentaria.Processo))
                    {
                        mensagens.Add("Este processo já consta como cadastrado.");
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

        public ValidacaoDados ValidaEdicao(Entidades.PlanilhaOrcamentaria planilhaOrcamentaria, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (planilhaOrcamentaria == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            //if (!cadastro)
            //{
            //    if (mandatoAae.MandatoAaeId <= 0)
            //    {
            //        mensagens.Add("Campo Nome é obrigatório.");
            //    }
            //}

            if (String.IsNullOrEmpty(planilhaOrcamentaria.Processo))
            {
                mensagens.Add(" O campo obrigatório Num Processo não foi preenchido");
            }
            if (planilhaOrcamentaria.Processo == "0")
            {
                mensagens.Add(" O campo obrigatório Num Processo não foi preenchido");
            }
            if (String.IsNullOrEmpty(planilhaOrcamentaria.Descricao))
            {
                mensagens.Add(" O campo obrigatório Descrição não foi preenchido");
            }
            else if (planilhaOrcamentaria.Descricao.Length > 255)
            {
                mensagens.Add(" O campo Descrição deve conter no máximo 255 caracteres");
            }

            if (planilhaOrcamentaria.ProgramaTrabalhoId == 0)
            {
                mensagens.Add(" O campo obrigatório Programa Trabalho não foi preenchido");
            }

            if (planilhaOrcamentaria.PlanoTrabalhoId == 0)
            {
                mensagens.Add(" O campo obrigatório Projeto / Programa não foi preenchido");
            }

            if (planilhaOrcamentaria.NaturezaDespesaId == 0)
            {
                mensagens.Add(" O campo obrigatório Natureza Despesa não foi preenchido");
            }

            if (planilhaOrcamentaria.RegiaoFinanceiraId == 0)
            {
                mensagens.Add(" O campo obrigatório Regiao Financeira não foi preenchido");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (this.PossuiProcessoAssociadoPor(contexto, planilhaOrcamentaria.Processo, planilhaOrcamentaria.PlanilhaOrcamentariaId))
                    {
                        mensagens.Add("Este processo já consta associado a outra planilha orçamentária.");
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


        public void Atualiza(Entidades.PlanilhaOrcamentaria planilhaOrcamentaria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();

            try
            {
                //Atualiza
                this.Atualiza(contexto, planilhaOrcamentaria);

                //Verifica se possui analise
                if (rnAnalisePlanilhaOrcamentaria.PossuiAnalisePor(contexto, planilhaOrcamentaria.PlanilhaOrcamentariaId))
                {
                    //Incluir analise anterior no historico
                    rnAnalisePlanilhaOrcamentaria.InsereHistorico(contexto, planilhaOrcamentaria.PlanilhaOrcamentariaId);

                    //Remove analise anterior
                    rnAnalisePlanilhaOrcamentaria.Remove(contexto, planilhaOrcamentaria.PlanilhaOrcamentariaId);
                }
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

        private void Atualiza(DataContext contexto, Entidades.PlanilhaOrcamentaria planilhaOrcamentaria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.PLANILHAORCAMENTARIA
                                       SET
									    NATUREZADESPESAID = @NATUREZADESPESAID
										,PLANOTRABALHOID = @PLANOTRABALHOID
										,REGIAOFINANCEIRAID = @REGIAOFINANCEIRAID
										,PROCESSO = @PROCESSO
										,DESCRICAO = @DESCRICAO
                                        ,ANO = @ANO
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                     WHERE PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentaria.PlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@NATUREZADESPESAID", SqlDbType.Int, planilhaOrcamentaria.NaturezaDespesaId);
            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planilhaOrcamentaria.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.Int, planilhaOrcamentaria.RegiaoFinanceiraId);
            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, planilhaOrcamentaria.Processo);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, planilhaOrcamentaria.Descricao);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, planilhaOrcamentaria.Ano);

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, planilhaOrcamentaria.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiPlanilhaOrcamentariaPor(int planoTrabalhoId, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM [PrestacaoContas].[PLANILHAORCAMENTARIA] A
                                    JOIN [PrestacaoContas].ITEMPLANILHAORCAMENTARIA B
                                       ON A.PLANILHAORCAMENTARIAID = B. PLANILHAORCAMENTARIAID
                                    JOIN [PrestacaoContas].LANCAMENTOREPASSE  C
                                       ON C.ITEMPLANILHAORCAMENTARIAID = B.ITEMPLANILHAORCAMENTARIAID
                                    WHERE A.PLANOTRABALHOID = @PLANOTRABALHOID
                                    AND C.CENSO = @CENSO
                                    ";

                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
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

            return existe;
        }

        private bool PossuiProcessoAssociadoPor(DataContext ctx, string processo, int planilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(1)  
                            FROM [PrestacaoContas].[PLANILHAORCAMENTARIA] 
                                    WHERE 
                                     PROCESSO = @PROCESSO
                                    AND PLANILHAORCAMENTARIAID <> @PLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);
            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, processo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }
        private bool PossuiProcessoDuplicado(DataContext ctx, string processo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(1)  
                            FROM [PrestacaoContas].[PLANILHAORCAMENTARIA] 
                                    WHERE 
                                     PROCESSO = @PROCESSO ";

            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, processo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

    }
}

