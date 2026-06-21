using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class AplicacaoFinanceira
    {
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

        public bool PossuiProgramacaoOrcamentariaPor(int PlanilhaOrcamentariaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            DataTable dt = new DataTable();

            contextQuery.Command = @" select APROVADA from  prestacaocontas.ANALISEPLANILHAORCAMENTARIA 
						              where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, PlanilhaOrcamentariaId);

            dt = contexto.GetDataTable(contextQuery);

            if (dt != null)
            {
                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray != null && dt.Rows[0].ItemArray.Count() > 0)
                    {
                        if ((bool)dt.Rows[0].ItemArray[0] == true)
                        {
                            existe = true;
                        }
                    }
                }
            }

            return existe;
        }


        public String RetornaMotivoPor(int PlanilhaOrcamentariaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            string retorno = "";
            DataTable dt = new DataTable();

            contextQuery.Command = @" select m.DESCRICAO from  prestacaocontas.ANALISEPLANILHAORCAMENTARIA a
                                      join PrestacaoContas.MOTIVOREPROVACAOPLANILHAORCAMENTARIA m 
                                      on a.MOTIVOREPROVACAOPLANILHAORCAMENTARIAID = m.MOTIVOREPROVACAOPLANILHAORCAMENTARIAID
                                      where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID  ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, PlanilhaOrcamentariaId);

            dt = contexto.GetDataTable(contextQuery);

            if (dt != null)
            {
                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray != null && dt.Rows[0].ItemArray.Count() > 0)
                    {
                        if (!String.IsNullOrEmpty(dt.Rows[0].ItemArray[0].ToString()))
                        {
                            retorno = dt.Rows[0].ItemArray[0].ToString();
                        }
                    }
                }
            }

            return retorno;
        }

        //public DataTable ListaDados(String censo, String processo)
        public DataTable ListaDados(object censo)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 	SELECT
                                                      eb.CENSO,
                                                      UE.NOME_COMP AS UNIDADEENSINO,
                                                      eb.ANO,
                                                      eb.MES,
                                                      af.VALOR,
                                                      af.JUSTIFICATIVA,
                                                      eb.EXTRATOBANCARIOID,
                                                      af.APLICACAOFINANCEIRAID,
                                                      ar.TIPOARQUIVO
                                                    FROM PrestacaoContas.APLICACAOFINANCEIRA af
                                                    LEFT JOIN PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO ar
                                                      ON ar.APLICACAOFINANCEIRAID = af.APLICACAOFINANCEIRAID
                                                    LEFT JOIN PrestacaoContas.EXTRATOBANCARIO eb
                                                      ON eb.EXTRATOBANCARIOID = af.EXTRATOBANCARIOID
                                                    INNER JOIN LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = EB.CENSO
                                                    WHERE eb.CENSO = @CENSO
                                             ";


                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);


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

        public DataTable ListaDadosPor(string censo, int extratoBancario)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 	SELECT
                                              eb.CENSO,
                                              UE.NOME_COMP AS UNIDADEENSINO,
                                              eb.ANO,
                                              eb.MES,
                                              af.VALOR,
                                              af.JUSTIFICATIVA,
                                              eb.EXTRATOBANCARIOID,
                                              af.APLICACAOFINANCEIRAID,
                                              ar.TIPOARQUIVO,
                                              ar.APLICACAOFINANCEIRACOMPROVANTEARQUIVOID
                                            FROM PrestacaoContas.APLICACAOFINANCEIRA af
                                            LEFT JOIN PrestacaoContas.APLICACAOFINANCEIRACOMPROVANTEARQUIVO ar
                                              ON ar.APLICACAOFINANCEIRAID = af.APLICACAOFINANCEIRAID
                                            LEFT JOIN PrestacaoContas.EXTRATOBANCARIO eb
                                              ON eb.EXTRATOBANCARIOID = af.EXTRATOBANCARIOID
                                            INNER JOIN LY_UNIDADE_ENSINO UE
                                              ON UE.UNIDADE_ENS = EB.CENSO
                                            WHERE eb.CENSO = @CENSO
                                            AND eb.EXTRATOBANCARIOID = @EXTRATOBANCARIOID ";


                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, extratoBancario);

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

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" select po.PLANILHAORCAMENTARIAID, po.ANO , po.PROCESSO, po.DESCRICAO, (select DESCRICAO from PrestacaoContas.PLANOTRABALHO where PLANOTRABALHOID = po.PLANOTRABALHOID) as PLANOTRABALHOID  
                                        , (select DESCRICAO from PrestacaoContas.NATUREZADESPESA where NATUREZADESPESAID = po.NATUREZADESPESAID)
	                                        as NATUREZADESPESAID , (select DESCRICAO from GestaoRede.REGIAOFINANCEIRA where REGIAOFINANCEIRAID = po.REGIAOFINANCEIRAID) as REGIAOFINANCEIRAID
                                          
										  -- ui.UNIDADEENSINOIMPEDIDAID as IMPEDIDA 
										 -- case when ui.UNIDADEENSINOIMPEDIDAID is null 
											--then 'N' 
											--else 'S'
											--end as IMPEDIDA
										  --, ipo.VALOR, 
										  , ( select sum(valor) from PrestacaoContas.ITEMPLANILHAORCAMENTARIA ipo where 
										  ipo.PLANILHAORCAMENTARIAID = po.PLANILHAORCAMENTARIAID ) as VALORTOTAL,
										  --ar.APROVADO as ACAO 
										  
                                          --ap.APROVADA as ACAO 
                                             case when ap.APROVADA is null 
                                            then 'SELECIONE' 
                                            when ap.APROVADA = 0
											then 'N' 
											else 'S'
											end as ACAO
										  , ap.MOTIVOREPROVACAOPLANILHAORCAMENTARIAID as MOTIVOREPROVACAO 
                                          , ap.ANALISEPLANILHAORCAMENTARIAID
                                          , prot.PROGRAMATRABALHOID
                                          from PrestacaoContas.PLANILHAORCAMENTARIA po
                                          left join PrestacaoContas.ANALISEPLANILHAORCAMENTARIA ap 
                                          on po.PLANILHAORCAMENTARIAID = ap.PLANILHAORCAMENTARIAID
                                          left join PrestacaoContas.PLANOTRABALHO pt 
                                          on pt.PROGRAMATRABALHOID = po.PLANOTRABALHOID
	                                      left join PrestacaoContas.PROGRAMATRABALHO prot 
                                          on prot.PROGRAMATRABALHOID = pt.PROGRAMATRABALHOID

                                          WHERE po.ANO = @ANO ";



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

        public ValidacaoDados Valida(Entidades.AplicacaoFinanceira aplicacaoFinanceira, RN.PrestacaoContas.Entidades.AplicacaoFinanceiraComprovanteArquivo aplicacaoFinanceiraArquivo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ImportacaoSei rnImportacaoSei = new ImportacaoSei();
            ExtratoBancario rnExtratoBancario = new ExtratoBancario();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aplicacaoFinanceira == null)
            {
                return validacaoDados;
            }

            if (String.IsNullOrEmpty(aplicacaoFinanceira.Censo))
            {
                mensagens.Add(" O campo obrigatório Unidade Ensino não foi preenchido");
            }

            if (aplicacaoFinanceira.ExtratoBancarioId == null || aplicacaoFinanceira.ExtratoBancarioId == 0)
            {
                mensagens.Add(" O campo obrigatório Extrato Bancario não foi preenchido");
            }

            if (aplicacaoFinanceira.Valor <= 0)
            {
                mensagens.Add(" O campo VALOR APLICAÇÃO é de preenchimento obrigatório.");
            }

            if (String.IsNullOrEmpty(aplicacaoFinanceira.Justificativa))
            {
                mensagens.Add(" O campo obrigatório Justificativa não foi preenchido");
            }

            if (aplicacaoFinanceiraArquivo.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ANEXO DO COMPROVANTE é de preenchimento obrigatório.");
            }
            else
            {

                if (aplicacaoFinanceiraArquivo.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
                }
                else
                { 
                    //Apenas aceitar pdf e imagem 
                    if (aplicacaoFinanceiraArquivo.TipoArquivo.ToUpper() != "IMAGE/JPEG"
                        && aplicacaoFinanceiraArquivo.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                    {
                        mensagens.Add("Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                    }
                }
            }

            if (aplicacaoFinanceira.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca extrato bancario
                     var extratoBancario = rnExtratoBancario.ObtemExtratoBancario(contexto, aplicacaoFinanceira.ExtratoBancarioId);                    

                     var row = extratoBancario.Rows[0];

                     int ano = row["ANO"] != DBNull.Value ? (int)Convert.ToInt32(row["ANO"]) : 0;
                     int mes = row["MES"] != DBNull.Value ? (int)Convert.ToInt32(row["MES"]) : 0;

                    //Verifica se ja foi para o SEI
                     if (rnImportacaoSei.PossuiImportacaoSeiPor(contexto, ano, mes, aplicacaoFinanceira.Censo))
                    {
                        mensagens.Add("Esta aplicação não pode ser incluída, pois já foi gerado o documento para o SEI.");
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

        public void Insere(Entidades.AplicacaoFinanceira aplicacaoFinanceira, RN.PrestacaoContas.Entidades.AplicacaoFinanceiraComprovanteArquivo aplicacaoFinanceiraArquivo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo rnAplicacaoFinanceiraComprovanteArquivo = new Techne.Lyceum.RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo();

            try
            {
                this.Insere(ctx, aplicacaoFinanceira);

                //Insere arquivo
                aplicacaoFinanceiraArquivo.AplicacaoFinanceiraId = aplicacaoFinanceira.AplicacaoFinanceiraId;
                rnAplicacaoFinanceiraComprovanteArquivo.Insere(ctx, aplicacaoFinanceiraArquivo);

                //Insere auditoria arquivo
                rnAplicacaoFinanceiraComprovanteArquivo.InsereAuditoria(ctx, aplicacaoFinanceiraArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        private void Insere(DataContext ctx, Entidades.AplicacaoFinanceira aplicacaoFinanceira)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PrestacaoContas.APLICACAOFINANCEIRA
                                                        (EXTRATOBANCARIOID
                                                         ,VALOR 
                                                         ,JUSTIFICATIVA 
                                                         ,USUARIOID														 
                                                         ,DATACADASTRO 
                                                         ,DATAALTERACAO
                                                         ,DATALANCAMENTO) 
                                            VALUES      (@EXTRATOBANCARIOID, 
                                                         @VALOR, 
														 @JUSTIFICATIVA,
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO
                                                        ,@DATALANCAMENTO)

                                                        SELECT IDENT_CURRENT('PrestacaoContas.APLICACAOFINANCEIRA') ";

            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, aplicacaoFinanceira.ExtratoBancarioId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, aplicacaoFinanceira.Valor);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, aplicacaoFinanceira.Justificativa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, aplicacaoFinanceira.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATALANCAMENTO", SqlDbType.DateTime, DateTime.Now);

            aplicacaoFinanceira.AplicacaoFinanceiraId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
        }

        //public ValidacaoDados Valida(Entidades.AnalisePlanilhaOrcamentaria analisePlanilhaOrcamentaria)
        //{
        //    List<string> mensagens = new List<string>();
        //    DataContext contexto = null;
        //    ValidacaoDados validacaoDados = new ValidacaoDados
        //    {
        //        Valido = false
        //    };

        //    if (analisePlanilhaOrcamentaria == null)
        //    {
        //        return validacaoDados;
        //    }

        //    if (analisePlanilhaOrcamentaria.Ano == null || analisePlanilhaOrcamentaria.Ano == 0)
        //    {
        //        mensagens.Add(" O campo obrigatório Ano Não foi preenchido ");
        //    }


        //    if (mensagens.Count == 0)
        //    {
        //        try
        //        {

        //        }
        //        catch (Exception ex)
        //        {
        //            if (contexto != null)
        //            {
        //                contexto.Abandon();
        //            }
        //            throw new Exception(ex.Message);
        //        }
        //        finally
        //        {
        //            if (contexto != null)
        //            {
        //                contexto.Dispose();
        //            }
        //        }
        //    }

        //    if (mensagens.Count > 0)
        //    {
        //        validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
        //    }
        //    else
        //    {
        //        validacaoDados.Valido = true;
        //    }

        //    return validacaoDados;
        //}

        public void Atualiza(Entidades.AplicacaoFinanceira aplicacaoFinanceira, RN.PrestacaoContas.Entidades.AplicacaoFinanceiraComprovanteArquivo aplicacaoFinanceiraArquivo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo rnAplicacaoFinanceiraComprovanteArquivo = new Techne.Lyceum.RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo();

            try
            {
                this.Atualiza(ctx, aplicacaoFinanceira);

                //Atualiza arquivo
                rnAplicacaoFinanceiraComprovanteArquivo.Atualiza(ctx, aplicacaoFinanceiraArquivo);

                //Insere auditoria arquivo
                rnAplicacaoFinanceiraComprovanteArquivo.InsereAuditoria(ctx, aplicacaoFinanceiraArquivo, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        private void Atualiza(DataContext contexto, Entidades.AplicacaoFinanceira aplicacaoFinanceira)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.APLICACAOFINANCEIRA
                                       SET
									   EXTRATOBANCARIOID = @EXTRATOBANCARIOID, 
                                       VALOR = @VALOR, 
									   JUSTIFICATIVA = @JUSTIFICATIVA, 
                                       USUARIOID = @USUARIOID, 
                                       DATAALTERACAO = @DATAALTERACAO
                                     WHERE APLICACAOFINANCEIRAID = @APLICACAOFINANCEIRAID ";

            contextQuery.Parameters.Add("@APLICACAOFINANCEIRAID", SqlDbType.Int, aplicacaoFinanceira.AplicacaoFinanceiraId);
            contextQuery.Parameters.Add("@EXTRATOBANCARIOID", SqlDbType.Int, aplicacaoFinanceira.ExtratoBancarioId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, aplicacaoFinanceira.Valor);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, aplicacaoFinanceira.Justificativa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, aplicacaoFinanceira.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int aplicacaoFinanceiraId, string usuarioId, int ano, int mes, string censo)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ImportacaoSei rnImportacaoSei = new ImportacaoSei();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aplicacaoFinanceiraId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (mes <= 0)
            {
                mensagens.Add("Campo MÊS é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi para o SEI
                    if (rnImportacaoSei.PossuiImportacaoSeiPor(contexto, ano, mes, censo))
                    {
                        mensagens.Add("Esta aplicação não pode ser excluída, pois já foi gerado o documento para o SEI.");
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

        public void Remove(int aplicacaoFinanceiraId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo rnAplicacaoFinanceiraComprovanteArquivo = new Techne.Lyceum.RN.PrestacaoContas.AplicacaoFinanceiraComprovanteArquivo();

            try
            {
                //Insere auditoria arquivo
                rnAplicacaoFinanceiraComprovanteArquivo.InsereAuditoria(ctx, aplicacaoFinanceiraId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName, usuarioId);

                //Remove arquivo
                rnAplicacaoFinanceiraComprovanteArquivo.Remove(ctx, aplicacaoFinanceiraId);

                this.Remove(ctx, aplicacaoFinanceiraId);
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


        private void Remove(DataContext ctx, int aplicacaoFinanceiraId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PrestacaoContas.APLICACAOFINANCEIRA
                            WHERE  APLICACAOFINANCEIRAID = @APLICACAOFINANCEIRAID  ";

            contextQuery.Parameters.Add("@APLICACAOFINANCEIRAID", SqlDbType.Int, aplicacaoFinanceiraId);

            ctx.ApplyModifications(contextQuery);
        }
    }
}
