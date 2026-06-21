using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using System.Globalization;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ItemPlanilhaOrcamentaria
    {
        public DateTime ObtemUltimoDiaPor(int itemPlanilhaOrcamentariaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int ano;
            int mes;
            DateTime dataFinal = new DateTime();

            try
            {
                contextQuery.Command = @"   SELECT PO.ANO,
	                                               I.REFERENCIA
                                            FROM PRESTACAOCONTAS.PLANILHAORCAMENTARIA PO
                                                INNER JOIN PRESTACAOCONTAS.ITEMPLANILHAORCAMENTARIA I ON PO.PLANILHAORCAMENTARIAID = I.PLANILHAORCAMENTARIAID
                                            WHERE I.ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID ";

                contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ano = Convert.ToInt32(reader["ANO"]);
                    mes = Convert.ToInt32(reader["REFERENCIA"]);
                    dataFinal = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
                }

                return dataFinal;
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

        public decimal? ObtemValorPor(int itemPlanilhaOrcamentariaId)
        {
            try
            {
                ContextQuery query = new ContextQuery();

                query.Command = @"
                    select 
                    ipo.VALOR

                    from PrestacaoContas.ITEMPLANILHAORCAMENTARIA ipo
                    inner join PrestacaoContas.PLANILHAORCAMENTARIA po on po.PLANILHAORCAMENTARIAID = ipo.PLANILHAORCAMENTARIAID
                    
                    where 
                    ipo.ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID
                ";

                query.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

                using (DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return ctx.GetReturnValue<decimal?>(query);
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

        public DataTable ObtemPor(int itemPlanilhaOrcamentariaId)
        {
            try
            {
                ContextQuery query = new ContextQuery();

                query.Command = @"
                    select 
                    ipo.ITEMPLANILHAORCAMENTARIAID 
                    ,ipo.PLANILHAORCAMENTARIAID
                    ,ipo.FONTERECURSOID
                    ,ipo.REFERENCIA
                    ,ipo.RETORNOREFERENCIA
                    ,ipo.USUARIOID
                    ,ipo.DATACADASTRO
                    ,ipo.DATAALTERACAO
                    ,po.NATUREZADESPESAID
                    ,po.PLANOTRABALHOID
                    ,po.REGIAOFINANCEIRAID
                    ,po.ANO
                    ,po.REGIAOFINANCEIRAID
                    ,po.PROCESSO
                    ,po.DESCRICAO
                    ,po.USUARIOID as PLANILHAORCAMENTARIA_USUARIOID
                    ,po.DATACADASTRO as PLANILHAORCAMENTARIA_DATACADASTRO
                    ,po.DATAALTERACAO as PLANILHAORCAMENTARIA_DATAALTERACAO
                    ,rf.DESCRICAO as REGIAOFINANCEIRA_DESCRICAO
                    ,fr.CODIGOSEFAZ
                    ,wsfrs.DESCRICAO as FONTERECURSO_DESCRICAO
                    ,fr.DATAINICIO
                    ,fr.DATAFIM

                    from PrestacaoContas.ITEMPLANILHAORCAMENTARIA ipo
                    inner join PrestacaoContas.PLANILHAORCAMENTARIA po on po.PLANILHAORCAMENTARIAID = ipo.PLANILHAORCAMENTARIAID
                    inner join GestaoRede.REGIAOFINANCEIRA rf on rf.REGIAOFINANCEIRAID = po.REGIAOFINANCEIRAID
                    inner join PrestacaoContas.FONTERECURSO fr on fr.FONTERECURSOID = ipo.FONTERECURSOID
                    inner join PrestacaoContas.WSFONTERECURSOSEFAZ wsfrs on wsfrs.CODIGOSEFAZ = fr.CODIGOSEFAZ
                    
                    where 
                    ipo.ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID
                ";

                query.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

                using (DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                    return ctx.GetDataTable(query);
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

        private bool PossuiOutraDescricaoCadastradaPor(int planilhaOrcamentariaId, int referencia, int itemPlanilhaOrcamentariaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" select count(*) from PrestacaoContas.ITEMPLANILHAORCAMENTARIA where PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID
						 and REFERENCIA = @REFERENCIA and ITEMPLANILHAORCAMENTARIAID <> @ITEMPLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);
            contextQuery.Parameters.Add("@REFERENCIA", SqlDbType.Int, referencia);
            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaItemPlanilhaOrcamentaria(string planilhaOrcamentariaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {

                contextQuery.Command = @" SELECT I.ITEMPLANILHAORCAMENTARIAID, 
                                                I.PLANILHAORCAMENTARIAID, 
                                                I.FONTERECURSOID, 
                                                I.REFERENCIA, 
                                                I.VALOR, 
                                                I.RETORNOREFERENCIA ,
                                                CASE 
	                                                WHEN I.RETORNOREFERENCIA = 'E' THEN 'ESTIMADO'
	                                                WHEN I.RETORNOREFERENCIA = 'F' THEN 'FATURADO'
                                                END DESCRICAORETORNOREFERENCIA
                                        FROM prestacaocontas.ITEMPLANILHAORCAMENTARIA  I
										INNER JOIN PrestacaoContas.PLANILHAORCAMENTARIA P ON P.PLANILHAORCAMENTARIAID = I.PLANILHAORCAMENTARIAID
                                        WHERE P.PROCESSO = @PLANILHAORCAMENTARIAID ";

                contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Char, planilhaOrcamentariaId);

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

        public decimal ListaSomaValorItemPlanilhaOrcamentaria(int planilhaOrcamentariaId)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            decimal valorSoma = 0;

            try
            {
                contextQuery.Command = @" SELECT SUM(VALOR) 
                                            FROM prestacaocontas.ITEMPLANILHAORCAMENTARIA 
                                            WHERE PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

                contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0].ItemArray != null && dt.Rows[0].ItemArray.Count() > 0)
                        {
                            if (dt.Rows[0].ItemArray[0] != DBNull.Value)
                            {
                                valorSoma = (decimal)dt.Rows[0].ItemArray[0];
                            }
                        }
                    }
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

            return valorSoma;
        }
        public int PesquisaAnoPrestacao(string PERIODOREFERENCIAID)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            decimal valorSoma = 0;
            var ano =0;
            try
            {
                contextQuery.Command = @"   select distinct ano
                    from prestacaocontas.vw_periodoreferencia
	                where PERIODOREFERENCIAID =  @PERIODOREFERENCIAID ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PERIODOREFERENCIAID);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    ano = Convert.ToInt32(dt.Rows[0][0]);
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

            return ano;
        }

        public string retornamesinicioefimprestacao(string PERIODOREFERENCIAID)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            var mesinicial = "";
            var mesfinal = "";
            var retorno = "";
            try
            {
                contextQuery.Command = @"   Begin
                                    declare @MESINICIAL varchar(6);
                                    declare @MESFINAL varchar(6); 

                                     select distinct  @MESINICIAL = CASE MESINICIAL
                                        WHEN 'Janeiro' THEN concat(ANO,'01')
                                        WHEN 'Fevereiro' THEN concat(ANO,'02')
                                        WHEN 'Março' THEN concat(ANO,'03')
	                                    WHEN 'Abril' THEN concat(ANO,'04')
	                                    WHEN 'Maio' THEN concat(ANO,'05')
	                                    WHEN 'Junho' THEN concat(ANO,'06')
	                                    WHEN 'Julho' THEN concat(ANO,'07')
	                                    WHEN 'Agosto' THEN concat(ANO,'08')
	                                    WHEN 'Setembro' THEN concat(ANO,'09')
	                                    WHEN 'Outubro' THEN concat(ANO,'10')
	                                    WHEN 'Novembro' THEN concat(ANO,'11')
	                                    WHEN 'Dezembro' THEN concat(ANO,'12')
                                        ELSE '0'
                                    END
                                      from prestacaocontas.vw_periodoreferencia
	                                    where PERIODOREFERENCIAID =  @PERIODOREFERENCIAID 

	                                    select distinct  @MESfinal = CASE MESfinal
                                        WHEN 'Janeiro' THEN concat(ANO,'01')
                                        WHEN 'Fevereiro' THEN concat(ANO,'02')
                                        WHEN 'Março' THEN concat(ANO,'03')
	                                    WHEN 'Abril' THEN concat(ANO,'04')
	                                    WHEN 'Maio' THEN concat(ANO,'05')
	                                    WHEN 'Junho' THEN concat(ANO,'06')
	                                    WHEN 'Julho' THEN concat(ANO,'07')
	                                    WHEN 'Agosto' THEN concat(ANO,'08')
	                                    WHEN 'Setembro' THEN concat(ANO,'09')
	                                    WHEN 'Outubro' THEN concat(ANO,'10')
	                                    WHEN 'Novembro' THEN concat(ANO,'11')
	                                    WHEN 'Dezembro' THEN concat(ANO,'12')
                                        ELSE '0'
                                    END
                                      from prestacaocontas.vw_periodoreferencia
	                                    where PERIODOREFERENCIAID =  @PERIODOREFERENCIAID ;
                                    	
                                    SELECT @MESINICIAL,@MESFINAL;
                                    End ";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PERIODOREFERENCIAID);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    mesinicial = dt.Rows[0][0].ToString();
                    mesfinal = dt.Rows[0][1].ToString();
                    retorno = " (MESANO >= " + mesinicial + " AND MESANO <=" + mesfinal + ") ";
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

            return retorno;
        }

        public DataTable ListaDadosGridAprovarProOrcPor(int planilhaOrcamentariaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  IPO.REFERENCIA, 
		                                        IPO.FONTERECURSOID, 
												FR.CODIGOSEFAZ + ' - ' + WSFRS.DESCRICAO AS FONTERECURSO_DESCRICAO,
		                                        IPO.VALOR, 
		                                        CASE	
			                                        WHEN RETORNOREFERENCIA = 'E' THEN 'Estimado'
			                                        WHEN RETORNOREFERENCIA = 'F' THEN 'Faturada'
		                                        END ESTIMADOFATURADO
                                        FROM PrestacaoContas.ITEMPLANILHAORCAMENTARIA ipo
											left join PrestacaoContas.FONTERECURSO  fr on fr.FONTERECURSOID = ipo.FONTERECURSOID
											left join PrestacaoContas.WSFONTERECURSOSEFAZ wsfrs on wsfrs.CODIGOSEFAZ = fr.CODIGOSEFAZ
                                        WHERE PLANILHAORCAMENTARIAID = @PLANILHAORCAMENTARIAID ";

                contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, planilhaOrcamentariaId);

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

        public ValidacaoDados ValidaRemocao(int itemPlanilhaOrcamentariaId, int planilhaOrcamentariaId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (itemPlanilhaOrcamentariaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (planilhaOrcamentariaId <= 0)
            {
                mensagens.Add("Campo NÚMERO PROCESSO é obrigatório.");
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

        public void Remove(int itemPlanilhaOrcamentariaId, int planilhaOrcamentariaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();

            try
            {
                //Remove
                this.Remove(ctx, itemPlanilhaOrcamentariaId);

                //Verifica se possui analise
                if (rnAnalisePlanilhaOrcamentaria.PossuiAnalisePor(ctx, planilhaOrcamentariaId))
                {
                    //Incluir analise anterior no historico
                    rnAnalisePlanilhaOrcamentaria.InsereHistorico(ctx, planilhaOrcamentariaId);

                    //Remove analise anterior
                    rnAnalisePlanilhaOrcamentaria.Remove(ctx, planilhaOrcamentariaId);
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

        private void Remove(DataContext ctx, int itemPlanilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PrestacaoContas.ITEMPLANILHAORCAMENTARIA
                            WHERE  ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID  ";

            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

            ctx.ApplyModifications(contextQuery);
        }

        public ValidacaoDados Valida(Entidades.ItemPlanilhaOrcamentaria itemPlanilhaOrcamentaria, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (itemPlanilhaOrcamentaria == null)
            {
                return validacaoDados;
            }

            if (itemPlanilhaOrcamentaria.Referencia == 0)
            {
                mensagens.Add(" O campo obrigatório Referência não foi preenchido");
            }

            if (itemPlanilhaOrcamentaria.FonteRecursoId == 0)
            {
                mensagens.Add(" O campo obrigatório Fonte Recurso não foi preenchido");
            }

            if (itemPlanilhaOrcamentaria.Valor <= 0)
            {
                mensagens.Add(" O campo obrigatório Valor não foi preenchido ");
            }
            else
            {
                if (itemPlanilhaOrcamentaria.Valor <= Convert.ToDecimal("0,01"))
                {
                    mensagens.Add(" O valor informado não pode ser inferior a R$0,01 ");
                }

                int casasDecimais = (itemPlanilhaOrcamentaria.Valor.ToString(CultureInfo.InvariantCulture)).Split('.')[1].Length;
                int inteiro = (itemPlanilhaOrcamentaria.Valor.ToString(CultureInfo.InvariantCulture)).Split('.')[0].Length;

                if (inteiro > 8)
                {
                    mensagens.Add(" O valor informado não pode ser maior que 99999999,99 ");
                }

                if (casasDecimais > 2)
                {
                    mensagens.Add(" O valor informado não pode ter mais de 2 números decimais ");
                }
            }






            if (PossuiOutraDescricaoCadastradaPor(itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId, itemPlanilhaOrcamentaria.Referencia, itemPlanilhaOrcamentaria.ItemPlanilhaOrcamentariaId))
            {
                mensagens.Add(" Não é possível informar mais de uma parcela para um mesmo mês de referência. ");
            }

            if (mensagens.Count == 0)
            {
                try
                {

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

        public void Atualiza(Entidades.ItemPlanilhaOrcamentaria itemPlanilhaOrcamentaria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();

            try
            {
                //Atualiza 
                this.Atualiza(contexto, itemPlanilhaOrcamentaria);

                //Verifica se possui analise
                if (rnAnalisePlanilhaOrcamentaria.PossuiAnalisePor(contexto, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId))
                {
                    //Incluir analise anterior no historico
                    rnAnalisePlanilhaOrcamentaria.InsereHistorico(contexto, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId);

                    //Remove analise anterior
                    rnAnalisePlanilhaOrcamentaria.Remove(contexto, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId);
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

        private void Atualiza(DataContext contexto, Entidades.ItemPlanilhaOrcamentaria itemPlanilhaOrcamentaria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.ITEMPLANILHAORCAMENTARIA
                                       SET
									    REFERENCIA = @REFERENCIA
										,FONTERECURSOID = @FONTERECURSOID
										,VALOR = @VALOR										
                                        ,USUARIOID = @USUARIOID
                                        ,DATAALTERACAO = @DATAALTERACAO
                                     WHERE ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentaria.ItemPlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@REFERENCIA", SqlDbType.Int, itemPlanilhaOrcamentaria.Referencia);
            contextQuery.Parameters.Add("@FONTERECURSOID", SqlDbType.Int, itemPlanilhaOrcamentaria.FonteRecursoId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, itemPlanilhaOrcamentaria.Valor);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, itemPlanilhaOrcamentaria.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(Entidades.ItemPlanilhaOrcamentaria itemPlanilhaOrcamentaria)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();

            try
            {
                //Insere 
                this.Insere(contexto, itemPlanilhaOrcamentaria);

                //Verifica se possui analise
                if (rnAnalisePlanilhaOrcamentaria.PossuiAnalisePor(contexto, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId))
                {
                    //Incluir analise anterior no historico
                    rnAnalisePlanilhaOrcamentaria.InsereHistorico(contexto, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId);

                    //Remove analise anterior
                    rnAnalisePlanilhaOrcamentaria.Remove(contexto, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId);
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

        private void Insere(DataContext contexto, Entidades.ItemPlanilhaOrcamentaria itemPlanilhaOrcamentaria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO  [PrestacaoContas].[ITEMPLANILHAORCAMENTARIA]
                                           (REFERENCIA 
                                        ,PLANILHAORCAMENTARIAID
										,FONTERECURSOID 
										,VALOR 
										,RETORNOREFERENCIA 
                                        ,USUARIOID 
                                        ,DATAALTERACAO )

                                     VALUES
	                                       ( @REFERENCIA
                                            ,@PLANILHAORCAMENTARIAID
                                           ,@FONTERECURSOID
                                           ,@VALOR
                                           ,@RETORNOREFERENCIA                                           
                                           ,@USUARIOID
                                           --,DATACADASTRO
                                           ,@DATAALTERACAO
                                           )

                         SELECT IDENT_CURRENT('PrestacaoContas.ITEMPLANILHAORCAMENTARIA') ";

            contextQuery.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentaria.PlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentaria.ItemPlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@REFERENCIA", SqlDbType.Int, itemPlanilhaOrcamentaria.Referencia);
            contextQuery.Parameters.Add("@FONTERECURSOID", SqlDbType.Int, itemPlanilhaOrcamentaria.FonteRecursoId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, itemPlanilhaOrcamentaria.Valor);
            contextQuery.Parameters.Add("@RETORNOREFERENCIA", SqlDbType.VarChar, itemPlanilhaOrcamentaria.RetornoReferencia);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, itemPlanilhaOrcamentaria.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            itemPlanilhaOrcamentaria.ItemPlanilhaOrcamentariaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }



        public string ObtemIntervaloPagamento(string PERIODOREFERENCIAID)
        {

            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            DateTime mesinicial;
            DateTime mesfinal;
            var retorno = "";
            try
            {
                contextQuery.Command = @"  DECLARE @ANO  INT
                                            DECLARE @MESINICIO  INT
                                            DECLARE @MESFIM INT
                                            DECLARE @DATAINICIO DATETIME
                                            DECLARE @DATAFIM DATETIME

                                    SELECT @ANO = ANO, @MESINICIO = MESINICIAL, @MESFIM = MESFINAL
	                                FROM PrestacaoContas.PERIODOREFERENCIA
	                                WHERE PERIODOREFERENCIAID = @PERIODOREFERENCIAID

	                                SET @DATAINICIO = CONVERT(datetime, Convert(varchar, @ANO) + '-' + Convert(varchar, @MESINICIO) + '-1')

	                                DECLARE @DATE DATETIME = CONVERT(datetime, Convert(varchar, @ANO) + '-' + Convert(varchar, @MESFIM) + '-1') 
	                                SELECT @DATAFIM = EOMONTH ( @date )  
	                                SET DATEFORMAT DMY
	                                select CONCAT('(DATAPAGAMENTO  >= ''' ,  CONVERT(DATE, @DATAINICIO) , ''' AND DATAPAGAMENTO  <=  ''' ,  CONVERT(DATE,@DATAFIM) , ''')')
";

                contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, PERIODOREFERENCIAID);

                dt = ctx.GetDataTable(contextQuery);

                if (dt != null)
                {
                    retorno = dt.Rows[0][0].ToString();
                    //mesinicial = Convert.ToDateTime(dt.Rows[0][0]);
                    //mesfinal = Convert.ToDateTime(dt.Rows[0][1]);
                    //retorno = " (DATAPAGAMENTO  >= '" + mesinicial + "' AND DATAPAGAMENTO  <= '" + mesfinal + "') ";
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

            return retorno;
        }
    }
}
