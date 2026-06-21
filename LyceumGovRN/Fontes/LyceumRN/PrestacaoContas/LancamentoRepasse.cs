using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class LancamentoRepasse
    {
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

        public IList<Entidades.LancamentoRepasse> ListaLancamentoRepassePor(int itemPlanilhaOrcamentariaId, string[] censosDesconsiderados)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ListaLancamentoRepassePor(contexto, itemPlanilhaOrcamentariaId, censosDesconsiderados);
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

        public IList<Entidades.LancamentoRepasse> ListaLancamentoRepassePor(DataContext contexto, int itemPlanilhaOrcamentariaId, string[] censosDesconsiderados)
        {
            ContextQuery contextQuery = new ContextQuery();

            var sql = @" SELECT * FROM prestacaocontas.lancamentorepasse lr WHERE lr.itemplanilhaorcamentariaid = @ITEMPLANILHAORCAMENTARIAID ";

            if (censosDesconsiderados != null && censosDesconsiderados.Any())
            {
                censosDesconsiderados = censosDesconsiderados.Where(q => q.All(r => char.IsNumber(r)) && q.Length == 8).ToArray();
                censosDesconsiderados = censosDesconsiderados.Select(s => "'" + s.Trim() + "'").ToArray();
                censosDesconsiderados = censosDesconsiderados.Distinct().ToArray();
                if (censosDesconsiderados.Any())
                {
                    var censos = censosDesconsiderados.Aggregate((c, n) => c + "," + n);
                    sql = sql + " and lr.CENSO not in (" + censos + ")";
                }
            }

            contextQuery.Command = sql;

            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

            return contexto.TryToBindEntities<Entidades.LancamentoRepasse>(contextQuery).ToList();
        }

        public DataTable ListaItemPlanilhaOrcamentaria(int itemPlanilhaOrcamentariaId)
        {
            ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new ItemPlanilhaOrcamentaria();
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            DateTime data = rnItemPlanilhaOrcamentaria.ObtemUltimoDiaPor(itemPlanilhaOrcamentariaId);

            try
            {
                contextQuery.Command = @" SELECT LR.CENSO,
	                                               VUE.NOME_COMP AS ESCOLA,
                                                   LR.CONTACORRENTEID,
                                                   CC.AGENCIA,
                                                   CC.CONTA,
												   B.NOME AS BANCONOME,
                                                   LR.VALOR,
                                                   LR.NUMERONE,
                                                   LR.NUMERONL,
                                                   LR.NUMEROPD,
                                                   LR.NUMEROOB,
                                                   LR.NUMEROLISTAOB,
                                                   LR.STATUSLISTA,
                                                   LR.NUMEROPROCESSOREPASSE,
                                                   IP.REFERENCIA AS MES,
                                                   PO.ANO,
                                                   IP.FONTERECURSOID,
                                                   LR.NUMEROPROCESSOREPASSE,
                                                   LR.LANCAMENTOREPASSEID,
                                                   LR.WSREPASSESEFAZID,
                                                   LR.ITEMPLANILHAORCAMENTARIAID,
                                                   CASE
                                                     WHEN UI.UNIDADEENSINOIMPEDIDAID IS NULL THEN 'N'
                                                     ELSE 'S'
                                                   END           AS IMPEDIDA,
                                                   UI.DATAINICIO,
                                                   UI.DATAFIM,
                                                   IP.REFERENCIA AS MES,
                                                   CASE
                                                     WHEN AR.APROVADO = 0 THEN 'Reprovado'
                                                     WHEN AR.APROVADO = 1 THEN 'Aprovado'
                                                     ELSE NULL
                                                   END           AS ACAO,
                                                   MRL.DESCRICAO AS MOTIVOREPROVACAO,
                                                   LR.WSREPASSESEFAZID,
                                                   ip.VALOR
                                            FROM   prestacaocontas.lancamentorepasse lr
                                                   inner JOIN prestacaocontas.itemplanilhaorcamentaria ip
                                                     ON ip.itemplanilhaorcamentariaid = lr.itemplanilhaorcamentariaid
                                                   inner JOIN prestacaocontas.contacorrente cc
                                                     ON lr.contacorrenteid = cc.contacorrenteid
												   INNER JOIN HADES.DBO.BANCOS B (NOLOCK) 
					                                        ON CONVERT(INT, CC.BANCO) = CONVERT(INT, B.BANCO)
                                                   inner JOIN prestacaocontas.planilhaorcamentaria po
                                                     ON po.planilhaorcamentariaid = ip.planilhaorcamentariaid
                                                   LEFT JOIN prestacaocontas.unidadeensinoimpedida ui
                                                          ON ui.censo = lr.censo
                                                             AND @DATA BETWEEN UI.DATAINICIO AND UI.DATAFIM
                                                   LEFT JOIN prestacaocontas.analiserepasse ar
                                                          ON ar.lancamentorepasseid = lr.lancamentorepasseid
                                                   LEFT JOIN prestacaocontas.motivoreprovacaolancamentorepasse mrl
                                                          ON mrl.motivoreprovacaolancamentorepasseid =
                                                             ar.motivoreprovacaolancamentorepasseid
                                                   INNER JOIN LY_UNIDADE_ENSINO vue
                                                          ON vue.unidade_ens = lr.censo
                                            WHERE  lr.itemplanilhaorcamentariaid = @ITEMPLANILHAORCAMENTARIAID ";

                contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);
                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data);

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

        public DataTable ListaPor(int itemPlanilhaOrcamentariaId)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"select lr.CENSO
											,lyue.CGC AS CNPJ
											 ,lr.LANCAMENTOREPASSEID
                                            ,lyue.NOME_COMP AS ESCOLA
                                            ,lr.CONTACORRENTEID
                                            ,cc.BANCO
                                            ,cc.AGENCIA
                                            ,cc.CONTA
                                            ,lr.VALOR
                                            ,lr.NUMERONE
                                            ,lr.NUMERONL
                                            ,lr.NUMEROPD
                                            ,lr.NUMEROOB
                                            ,lr.NUMEROLISTAOB
                                            ,lr.STATUSLISTA
                                            ,lr.NUMEROPROCESSOREPASSE  
	                                        ,ip.REFERENCIA as MES 
                                            ,po.ANO
                                            ,po.REGIAOFINANCEIRAID 
                                            ,ip.FONTERECURSOID
                                           
                                            ,ar.ANALISEREPASSEID
                                            ,lr.WSREPASSESEFAZID
                                            ,lr.ITEMPLANILHAORCAMENTARIAID
                                            ,case when ui.UNIDADEENSINOIMPEDIDAID is null 
											     then 0 
											     else 1
											end as IMPEDIDA
											,ui.DATAINICIO, ui.DATAFIM
											,ar.APROVADO as ACAO
                                            ,mrl.DESCRICAO as MOTIVOREPROVACAO
                                            ,lr.WSREPASSESEFAZID
                                            ,mrl.MOTIVOREPROVACAOLANCAMENTOREPASSEID
	                                        from PrestacaoContas.LANCAMENTOREPASSE lr (nolock)
												inner join PrestacaoContas.ITEMPLANILHAORCAMENTARIA ip (nolock) on ip.ITEMPLANILHAORCAMENTARIAID = lr.ITEMPLANILHAORCAMENTARIAID
												inner join PrestacaoContas.CONTACORRENTE cc (nolock) on lr.CONTACORRENTEID = cc.CONTACORRENTEID 
												inner join PrestacaoContas.PLANILHAORCAMENTARIA po (nolock) on po.PLANILHAORCAMENTARIAID = ip.PLANILHAORCAMENTARIAID
												left join PrestacaoContas.UNIDADEENSINOIMPEDIDA ui (nolock) on ui.CENSO = lr.CENSO  
																	AND EOMONTH ( CONVERT(datetime, Convert(varchar, po.ANO) + '-' + Convert(varchar, ip.REFERENCIA) + '-1')  ) BETWEEN UI.DATAINICIO AND UI.DATAFIM
												left join PrestacaoContas.ANALISEREPASSE ar (nolock) on ar.LANCAMENTOREPASSEID = lr.LANCAMENTOREPASSEID
												left join PrestacaoContas.MOTIVOREPROVACAOLANCAMENTOREPASSE mrl (nolock) on mrl.MOTIVOREPROVACAOLANCAMENTOREPASSEID = ar.MOTIVOREPROVACAOLANCAMENTOREPASSEID
												left join LY_UNIDADE_ENSINO lyue (nolock) on lyue.UNIDADE_ENS = lr.CENSO
                                                LEFT JOIN TCE_REGIONAL RE ON RE.ID_REGIONAL = lyue.ID_REGIONAL
												LEFT JOIN MUNICIPIO MU ON MU.CODIGO = LYUE.MUNICIPIO
                                            where lr.ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID 
                                            ORDER BY RE.REGIONAL,MU.NOME,LYUE.NOME_COMP ";

                    contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

                    return ctx.GetDataTable(contextQuery);
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

        public DataTable ListaSomaValorLancRepasseItemPlanilhaOrc(int itemPlanilhaOrcamentariaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            DataTable dt = null;

            try
            {
                dt = this.ListaSomaValorLancRepasseItemPlanilhaOrc(ctx, itemPlanilhaOrcamentariaId);
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

        private DataTable ListaSomaValorLancRepasseItemPlanilhaOrc(DataContext ctx, int itemPlanilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @"  SELECT  IP.REFERENCIA AS MES,
                                            PO.ANO, 		
		                                    RF.DESCRICAO  AS REGIAOFINANCEIRA,
		                                    F.CODIGOSEFAZ,
		                                    FS.DESCRICAO AS FONTE,
		                                    IP.VALOR AS SOMAITEMPLAORC,  
		                                    SUM(LR.VALOR) AS SOMALANCREPASSE,
											MIN(lr.NUMEROPROCESSOREPASSE) as NUMEROPROCESSOREPASSE
                                    FROM PRESTACAOCONTAS.ITEMPLANILHAORCAMENTARIA IP
	                                    LEFT JOIN  PRESTACAOCONTAS.LANCAMENTOREPASSE LR ON IP.ITEMPLANILHAORCAMENTARIAID = LR.ITEMPLANILHAORCAMENTARIAID
	                                    INNER JOIN PRESTACAOCONTAS.PLANILHAORCAMENTARIA PO ON PO.PLANILHAORCAMENTARIAID = IP.PLANILHAORCAMENTARIAID
	                                    LEFT JOIN GESTAOREDE.REGIAOFINANCEIRA RF ON RF.REGIAOFINANCEIRAID = PO.REGIAOFINANCEIRAID
	                                    LEFT JOIN PRESTACAOCONTAS.FONTERECURSO F ON IP.FONTERECURSOID = F.FONTERECURSOID
	                                    LEFT JOIN PRESTACAOCONTAS.WSFONTERECURSOSEFAZ FS ON F.CODIGOSEFAZ = FS.CODIGOSEFAZ
                                    WHERE IP.ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID
                                    GROUP BY IP.VALOR, IP.REFERENCIA, PO.ANO, RF.DESCRICAO, F.CODIGOSEFAZ, FS.DESCRICAO ";

            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

            dt = ctx.GetDataTable(contextQuery);

            return dt;
        }

        public void Insere(DataContext contexto, Entidades.LancamentoRepasse lancamentoRepasse)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.LANCAMENTOREPASSE
                                           (CENSO
                                           ,CONTACORRENTEID
                                           ,VALOR
                                           ,ITEMPLANILHAORCAMENTARIAID
                                           ,USUARIOID  
                                           ,DATAALTERACAO                                                                          
                                           )
                                     VALUES
	                                       (@CENSO
                                           ,@CONTACORRENTEID
                                           ,@VALOR
                                           ,@ITEMPLANILHAORCAMENTARIAID  
                                           ,@USUARIOID    
                                           ,@DATAALTERACAO                                      
                                          )

                         SELECT IDENT_CURRENT('PrestacaoContas.LANCAMENTOREPASSE') ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, lancamentoRepasse.Censo);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, lancamentoRepasse.ContaCorrenteId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, lancamentoRepasse.Valor);
            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, lancamentoRepasse.ItemPlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, lancamentoRepasse.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            lancamentoRepasse.LancamentoRepasseId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados Valida(Entidades.LancamentoRepasse lancamentoRepasse, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            AnaliseRepasse rnAnaliseRepasse = new AnaliseRepasse();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (lancamentoRepasse == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (lancamentoRepasse.LancamentoRepasseId <= 0)
                {
                    mensagens.Add(" O campo obrigatório CODIGO não foi preenchido");
                }
            }

            if (lancamentoRepasse.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo obrigatório UNIDADE ENSINO não foi preenchido  ");
            }

            if (lancamentoRepasse.ContaCorrenteId <= 0)
            {
                mensagens.Add(" O campo obrigatório CONTA CORRENTE não foi preenchido");
            }

            if (lancamentoRepasse.Valor <= 0)
            {
                mensagens.Add("O campo obrigatório VALOR deve ser preenchido e não pode ser negativo nem zero");
            }

            if (lancamentoRepasse.ItemPlanilhaOrcamentariaId <= 0)
            {
                mensagens.Add("O campo obrigatório PARCELA DA PROGRAMAÇÃO ORÇAMENTÁRIA não foi preenchido");
            }

            if (lancamentoRepasse.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUARIO não foi preenchido");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    DataTable dadosSomaValoresLancRepItemPlaOrc = new DataTable();

                    //Busca totais da parcela
                    dadosSomaValoresLancRepItemPlaOrc = this.ListaSomaValorLancRepasseItemPlanilhaOrc(contexto, lancamentoRepasse.ItemPlanilhaOrcamentariaId);

                    if (dadosSomaValoresLancRepItemPlaOrc.Rows != null && dadosSomaValoresLancRepItemPlaOrc.Rows.Count > 0)
                    {
                        var lancRepasse = dadosSomaValoresLancRepItemPlaOrc.Rows[0]["SOMALANCREPASSE"] != DBNull.Value ? Convert.ToDecimal(dadosSomaValoresLancRepItemPlaOrc.Rows[0]["SOMALANCREPASSE"]) : 0;
                        var item = dadosSomaValoresLancRepItemPlaOrc.Rows[0]["SOMAITEMPLAORC"] != DBNull.Value ? Convert.ToDecimal(dadosSomaValoresLancRepItemPlaOrc.Rows[0]["SOMAITEMPLAORC"]) : 0;

                        var somaLancRepasse = lancRepasse + lancamentoRepasse.Valor;
                        var somaItemPlaOrc = item;

                        if (somaLancRepasse > somaItemPlaOrc)
                        {
                            mensagens.Add("Valor informado inválido ou negativo, ou a soma dos lançamentos de repasses informados excedeu o valor da parcela da Programação Orçamentária.");
                        }
                    }

                    if (PossuiLancamentoRepassePor(contexto, lancamentoRepasse.Censo, lancamentoRepasse.ItemPlanilhaOrcamentariaId, lancamentoRepasse.LancamentoRepasseId))
                    {
                         mensagens.Add("Já existe um repasse cadastrado para esta unidade escolar para esta parcela da PO.");
                    }

                    //Verifica se o numero do repasse ja existe para outra planilha orcamentaria
                    if (!lancamentoRepasse.NumeroProcessoRepasse.IsNullOrEmptyOrWhiteSpace() && 
                        this.PossuiOutroNumeroProcessoRepassePor(contexto, lancamentoRepasse.NumeroProcessoRepasse, lancamentoRepasse.ItemPlanilhaOrcamentariaId, lancamentoRepasse.Censo, lancamentoRepasse.LancamentoRepasseId))
                    {
                        mensagens.Add("Este NÚMERO já foi cadastrado para este CENSO / REFERENCIA.");
                    }

                    if (!cadastro)
                    {
                        //Verifica se o repasse já foi aprovado
                        if (rnAnaliseRepasse.EhAprovadoPor(contexto, lancamentoRepasse.LancamentoRepasseId))
                        {
                            mensagens.Add("Repasses que já tenha sido “aprovados” não poderão ser editados.");
                        }
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

        private bool PossuiOutroNumeroProcessoRepassePor(DataContext contexto, string numeroProcessoRepasse, int itemPlanilhaOrcamentariaId, string censo, int lancamentoRepasseId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" select COUNT(*)
                                        FROM PRESTACAOCONTAS.LANCAMENTOREPASSE LR
                                            INNER JOIN PRESTACAOCONTAS.ITEMPLANILHAORCAMENTARIA I ON LR.ITEMPLANILHAORCAMENTARIAID = I.ITEMPLANILHAORCAMENTARIAID
                                        WHERE NUMEROPROCESSOREPASSE = @NUMEROPROCESSOREPASSE
                                            AND I.REFERENCIA = (SELECT REFERENCIA 
				                                        FROM PRESTACAOCONTAS.ITEMPLANILHAORCAMENTARIA
				                                        WHERE ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID)
											AND CENSO = @CENSO
											and LANCAMENTOREPASSEID <> @LANCAMENTOREPASSEID ";

            contextQuery.Parameters.Add("@NUMEROPROCESSOREPASSE", SqlDbType.VarChar, numeroProcessoRepasse);
            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@LANCAMENTOREPASSEID", SqlDbType.Int, lancamentoRepasseId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaRemocao(int lancamentoRepasseId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            AnaliseRepasse rnAnaliseRepasse = new AnaliseRepasse();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (lancamentoRepasseId <= 0)
            {
                mensagens.Add(" O campo obrigatório CODIGO não foi preenchido");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o repasse já foi aprovado
                    if (rnAnaliseRepasse.EhAprovadoPor(contexto, lancamentoRepasseId))
                    {
                        mensagens.Add("Repasses que já tenha sido “aprovados” não poderão ser excluídos.");
                    }

                    //Verifica se o repasse já foi ntegrados com o processo de integração
                    if (rnAnaliseRepasse.EhRepasseIntegradoComSEFAZ(contexto, lancamentoRepasseId))
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

        public void Insere(Entidades.LancamentoRepasse lancamentoRepasse)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere dados
                this.Insere(contexto, lancamentoRepasse);

                //Atualiza todos os numeros de processos
                this.AtualizaNumeroProcessoRepasse(contexto, lancamentoRepasse);
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

        public void Atualiza(Entidades.LancamentoRepasse lancamentoRepasse)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza dados
                this.Atualiza(contexto, lancamentoRepasse);

                //Atualiza todos os numeros de processos
                this.AtualizaNumeroProcessoRepasse(contexto, lancamentoRepasse);
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

        private void Atualiza(DataContext contexto, Entidades.LancamentoRepasse lancamentoRepasse)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.LANCAMENTOREPASSE
                                       SET
									    CENSO = @CENSO,
										CONTACORRENTEID = @CONTACORRENTEID,
										VALOR = @VALOR,
                                        USUARIOID = @USUARIOID,
                                        DATAALTERACAO = @DATAALTERACAO
                                     WHERE LANCAMENTOREPASSEID = @LANCAMENTOREPASSEID ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, lancamentoRepasse.Censo);
            contextQuery.Parameters.Add("@CONTACORRENTEID", SqlDbType.Int, lancamentoRepasse.ContaCorrenteId);
            contextQuery.Parameters.Add("@VALOR", SqlDbType.Decimal, lancamentoRepasse.Valor);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, lancamentoRepasse.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@LANCAMENTOREPASSEID", SqlDbType.Int, lancamentoRepasse.LancamentoRepasseId);

            contexto.ApplyModifications(contextQuery);
        }

        private void AtualizaNumeroProcessoRepasse(DataContext contexto, Entidades.LancamentoRepasse lancamentoRepasse)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.LANCAMENTOREPASSE
                                       SET NUMEROPROCESSOREPASSE = @NUMEROPROCESSOREPASSE
                                     WHERE ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, lancamentoRepasse.ItemPlanilhaOrcamentariaId);
            contextQuery.Parameters.Add("@NUMEROPROCESSOREPASSE", SqlDbType.VarChar, lancamentoRepasse.NumeroProcessoRepasse);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(int lancamentorRepasseId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" DELETE FROM PrestacaoContas.LANCAMENTOREPASSE 
                                        WHERE  LANCAMENTOREPASSEID = @LANCAMENTOREPASSEID ";

                contextQuery.Parameters.Add("@LANCAMENTOREPASSEID", SqlDbType.Int, lancamentorRepasseId);

                contexto.ApplyModifications(contextQuery);
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

        public bool PossuiLancamentoRepassePor(DataContext contexto, string censo, int itemPlanilhaId, int lancamentoRepasseId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"SELECT COUNT(*) 
                        FROM   [PrestacaoContas].[LANCAMENTOREPASSE] 
                        WHERE  CENSO = @CENSO
                               AND ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID
                               AND LANCAMENTOREPASSEID <> @LANCAMENTOREPASSEID
                                ";


            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaId);
            contextQuery.Parameters.Add("@LANCAMENTOREPASSEID", SqlDbType.Int, lancamentoRepasseId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void ImportaArquivo(IList<Entidades.LancamentoRepasse> lancamentosRepasse, out decimal valorTotalImportado, out int totalItensImportados)
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

        private void ImportaArquivo(DataContext contexto, IList<Entidades.LancamentoRepasse> lancamentosRepasse, out decimal valorTotalImportado, out int totalItensImportados)
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

                InsereRelatorio(contexto, primeiroLancamentoDaLista.ItemPlanilhaOrcamentariaId, valorTotalImportado, totalItensImportados, primeiroLancamentoDaLista.UsuarioId);
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

        public IList<Entidades.LancamentoRepasse> RetornaApenasAsUnidadesExistentes(int itemPlanilhaOrcamentariaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return RetornaApenasAsUnidadesExistentes(contexto, itemPlanilhaOrcamentariaId);
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

        public IList<Entidades.LancamentoRepasse> RetornaApenasAsUnidadesExistentes(DataContext contexto, int itemPlanilhaOrcamentariaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * FROM [LYCEUM].[PrestacaoContas].[LANCAMENTOREPASSE] LR (NOLOCK) WHERE LR.ITEMPLANILHAORCAMENTARIAID = @ITEMPLANILHAORCAMENTARIAID ";

            contextQuery.Parameters.Add("@ITEMPLANILHAORCAMENTARIAID", SqlDbType.Int, itemPlanilhaOrcamentariaId);

            return contexto.TryToBindEntities<Entidades.LancamentoRepasse>(contextQuery).ToList();
        }
    }
}