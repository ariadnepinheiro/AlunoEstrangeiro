using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Patrimonio
{
    public class Transferencia
    {
        public DataTable ListaSituacao()
        {
            DataTable situacoes = new DataTable();
            situacoes.Columns.Add("SITUACAO");

            situacoes.Rows.Add(TransferenciaItem.Pendente);
            situacoes.Rows.Add(TransferenciaItem.Recusada);
            situacoes.Rows.Add(TransferenciaItem.Aceita);

            return situacoes;
        }

        public DataTable ListaTransferenciaOrigemPor(string setor, string situacao)
        {
            if (situacao == TransferenciaItem.Pendente)
            {
                //Se a transferencia estiver pendente pega dados atuais do item
                return ListaTransferenciaPendentePor(setor, null, true);
            }
            else
            {
                //Senão pega dados do historico da transferencia
                return ListaTransferenciaProcessadaPor(setor, situacao, true);
            }
        }

        public DataTable ListaTransferenciaDestinoPor(string setor, string situacao, int? transferenciaId)
        {
            if (situacao == TransferenciaItem.Pendente)
            {
                //Se a transferencia estiver pendente pega dados atuais do item
                return ListaTransferenciaPendentePor(setor, transferenciaId, false);
            }
            else
            {
                //Senão pega dados do historico da transferencia
                return ListaTransferenciaProcessadaPor(setor, situacao, false);
            }
        }

        private DataTable ListaTransferenciaPendentePor(string setor, int? transferenciaId, bool origem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@"  SELECT E.TRANSFERENCIAITEMID, 
                                E.TRANSFERENCIAID, 
                                E.BEMID,
                                REPLICATE('0',6 - LEN(M.NUMERO)) + CONVERT(VARCHAR(6), M.NUMERO) AS NUMERO, 
                                B.DESCRICAO AS BEM, 
                                C.CONTA,
                                C.DESCRICAO AS CLASSIFICACAO, 
                                E.SITUACAO, 
                                T.SETORDESTINO, 
                                SD.NOME     AS SETORDESTINODESCRICAO, 
                                T.SETORORIGEM, 
                                SO.NOME     AS SETORORIGEMDESCRICAO, 
                                E.JUSTIFICATIVA, 
                                CONVERT(VARCHAR,T.DATAANDAMENTO,103) AS DATAANDAMENTO,
                                CONVERT(VARCHAR,T.DATAMOVIMENTACAO,103) AS DATAMOVIMENTACAO,
								CONVERT(VARCHAR,T.DATASOLICITACAO,103) AS DATASOLICITACAO,
								ED.CONCEITO AS ESTADOCONSERVACAO,
                                I.MOEDAID,
								MO.SIGLA,
                                Patrimonio.FN_CALCULOVALORATUALIZADO(E.BEMID, GETDATE(), I.DATAINICIO, I.VALOR, CV.TAXAVALORRESIDUAL,CV.VIDAUTIL) AS VALOR
                        FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
                                INNER JOIN PATRIMONIO.TRANSFERENCIAITEM E (NOLOCK) 
                                        ON T.TRANSFERENCIAID = E.TRANSFERENCIAID 
                                INNER JOIN HADES.DBO.HD_SETOR SO (NOLOCK) 
                                        ON T.SETORORIGEM = SO.SETOR 
                                INNER JOIN HADES.DBO.HD_SETOR SD (NOLOCK) 
                                        ON T.SETORDESTINO = SD.SETOR 
                                INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
                                        ON E.BEMID = B.BEMID 
								INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                                       ON B.BEMID = I.BEMID 
                                          AND i.DATAINICIO <= GETDATE()
                                          AND ( i.DATAFIM IS NULL 
                                                 OR i.DATAFIM >= GETDATE() ) 
								INNER JOIN Patrimonio.MOEDA mo (NOLOCK) 
										ON MO.MOEDAID = I.MOEDAID	
								INNER JOIN PATRIMONIO.ESTADOCONSERVACAO ED  (NOLOCK)  
											ON ED.ESTADOCONSERVACAOID = I.ESTADOCONSERVACAOID
			                    INNER JOIN PATRIMONIO.MOVIMENTACAO M (NOLOCK) 
                                                           ON B.BEMID = M.BEMID 
                                                              AND ( M.DATAFIM IS NULL 
                                                                     OR m.DATAFIM >= GETDATE() ) 
                                INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                        ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
                                INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
                                        ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
                                        AND CV.DATAINICIO <= GETDATE()
                                        AND  ( CV.DATAFIM IS NULL 
                                              OR CV.DATAFIM >= GETDATE() ) 
                       WHERE E.SITUACAO = @SITUACAO	                
                ");

                //Verifica se a listagem será por origem ou destino
                if (origem)
                {
                    sql.Append(@" AND T.SETORORIGEM = @SETOR ");
                }
                else
                {
                    sql.Append(@" AND T.SETORDESTINO = @SETOR  ");
                }
                //Verifica se serão apenas as transferencias do lote
                if (transferenciaId != null)
                {
                    sql.Append(@" AND T.TRANSFERENCIAID = @TRANSFERENCIAID  ");
                    contextQuery.Parameters.Add("@TRANSFERENCIAID", SqlDbType.Int, transferenciaId);
                }

                sql.Append(@" ORDER  BY B.DESCRICAO ");

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);
                contextQuery.Parameters.Add("@SITUACAO", SqlDbType.VarChar, TransferenciaItem.Pendente);

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

        public bool VerificaSeTemTransferenciaPendentePor(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            var sql = string.Empty;

            try
            {
                sql = @"  
select case when exists 
(
    SELECT top 1 1
    FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
            INNER JOIN PATRIMONIO.TRANSFERENCIAITEM E (NOLOCK) 
                    ON T.TRANSFERENCIAID = E.TRANSFERENCIAID 
            INNER JOIN HADES.DBO.HD_SETOR SO (NOLOCK) 
                    ON T.SETORORIGEM = SO.SETOR 
            INNER JOIN HADES.DBO.HD_SETOR SD (NOLOCK) 
                    ON T.SETORDESTINO = SD.SETOR 
            INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
                    ON E.BEMID = B.BEMID 
		    INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                   ON B.BEMID = I.BEMID 
                      AND i.DATAINICIO <= GETDATE()
                      AND ( i.DATAFIM IS NULL 
                             OR i.DATAFIM >= GETDATE() ) 
		    INNER JOIN Patrimonio.MOEDA mo (NOLOCK) 
				    ON MO.MOEDAID = I.MOEDAID	
		    INNER JOIN PATRIMONIO.ESTADOCONSERVACAO ED  (NOLOCK)  
					    ON ED.ESTADOCONSERVACAOID = I.ESTADOCONSERVACAOID
            INNER JOIN PATRIMONIO.MOVIMENTACAO M (NOLOCK) 
                                       ON B.BEMID = M.BEMID 
                                          AND ( M.DATAFIM IS NULL 
                                                 OR m.DATAFIM >= GETDATE() ) 
            INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                    ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
            INNER JOIN Patrimonio.CLASSIFICACAOVIGENCIA CV (NOLOCK) 
                    ON B.CLASSIFICACAOID = CV.CLASSIFICACAOID
                    AND CV.DATAINICIO <= GETDATE()
                    AND  ( CV.DATAFIM IS NULL 
                          OR CV.DATAFIM >= GETDATE() ) 
    WHERE E.SITUACAO = 'Pendente'                
    and (T.SETORORIGEM = @SETOR or T.SETORDESTINO = @SETOR)
) 
then 1 else 0 end
                ";

                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

                contextQuery.Command = sql;

                return ctx.GetReturnValue<bool>(contextQuery);
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
        }

        private DataTable ListaTransferenciaProcessadaPor(string setor, string situacao, bool origem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT E.TRANSFERENCIAITEMID, 
                                    E.TRANSFERENCIAID, 
                                    REPLICATE('0',6 - LEN(e.NUMEROBEMORIGEM)) + CONVERT(VARCHAR(6), e.NUMEROBEMORIGEM) AS NUMERO, 
                                    B.DESCRICAO AS BEM, 
                                    B.CLASSIFICACAOID, 
                                    C.CONTA,
                                    C.DESCRICAO AS CLASSIFICACAO, 
                                    E.SITUACAO, 
                                    T.SETORDESTINO, 
                                    SD.NOME     AS SETORDESTINODESCRICAO, 
                                    T.SETORORIGEM, 
                                    SO.NOME     AS SETORORIGEMDESCRICAO,  
                                    E.JUSTIFICATIVA,
                                    CONVERT(VARCHAR,T.DATAANDAMENTO,103) AS DATAANDAMENTO,
                                    CONVERT(VARCHAR,T.DATAMOVIMENTACAO,103) AS DATAMOVIMENTACAO,
                                    CONVERT(VARCHAR,T.DATASOLICITACAO,103) AS DATASOLICITACAO,
									ED.CONCEITO AS ESTADOCONSERVACAO,
                                    E.MOEDAID,
                                    MO.SIGLA,
									E.VALOR		
                            FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
                                    INNER JOIN PATRIMONIO.TRANSFERENCIAITEM E (NOLOCK) 
                                            ON T.TRANSFERENCIAID = E.TRANSFERENCIAID 
									INNER JOIN Patrimonio.MOEDA MO (NOLOCK)
											ON MO.MOEDAID = E.MOEDAID
                                    INNER JOIN HADES.DBO.HD_SETOR SO (NOLOCK) 
                                            ON T.SETORORIGEM = SO.SETOR 
                                    INNER JOIN HADES.DBO.HD_SETOR SD (NOLOCK) 
                                            ON T.SETORDESTINO = SD.SETOR 
                                    INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
                                            ON E.BEMID = B.BEMID 
                                    INNER JOIN PATRIMONIO.BEMVALOR I (NOLOCK) 
                                            ON B.BEMID = I.BEMID 
                                                AND CONVERT(DATE, T.DATAMOVIMENTACAO) BETWEEN I.DATAINICIO AND ISNULL(I.DATAFIM, CONVERT(DATE, GETDATE())) 
									INNER JOIN PATRIMONIO.ESTADOCONSERVACAO ED  (NOLOCK)  
											ON ED.ESTADOCONSERVACAOID = I.ESTADOCONSERVACAOID
                                    INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                            ON B.CLASSIFICACAOID = C.CLASSIFICACAOID 
                            WHERE E.SITUACAO = @SITUACAO 
                                 ");

                //Verifica se a listagem será por origem ou destino
                if (origem)
                {
                    sql.Append(@" AND T.SETORORIGEM = @SETOR ");
                }
                else
                {
                    sql.Append(@" AND T.SETORDESTINO = @SETOR  ");
                }

                sql.Append(@" ORDER  BY B.DESCRICAO ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@SITUACAO", SqlDbType.VarChar, situacao);
                contextQuery.Parameters.Add("@SETOR", SqlDbType.VarChar, setor);

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

        public DataTable ListaLoteAvisoTransferenciaBensMoveisPor(string setorOrigem, string setorDestino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT T.TRANSFERENCIAID AS LOTE, 
                                            T.DATASOLICITACAO, 		
                                            T.SETORDESTINO, 
                                            SD.NOME AS SETORDESTINODESCRICAO, 
                                            T.SETORORIGEM, 
                                            SO.NOME AS SETORORIGEMDESCRICAO, 
                                            MAX(T.DATAANDAMENTO) AS DATAANDAMENTO, 
                                            COUNT(DISTINCT IA.TRANSFERENCIAITEMID) AS QUANTIDADEITENSACEITOS,
		                                    COUNT(DISTINCT IR.TRANSFERENCIAITEMID) AS QUANTIDADEITENSRECUSADOS,
		                                    COUNT(DISTINCT IP.TRANSFERENCIAITEMID) AS QUANTIDADEITENSPENDENTES 
                                    FROM   PATRIMONIO.TRANSFERENCIA T (NOLOCK) 
                                            LEFT JOIN PATRIMONIO.TRANSFERENCIAITEM IA (NOLOCK) 
                                                    ON T.TRANSFERENCIAID = IA.TRANSFERENCIAID 
					                                    AND IA.SITUACAO = 'Aceita'
		                                    LEFT JOIN PATRIMONIO.TRANSFERENCIAITEM IR (NOLOCK) 
                                                    ON T.TRANSFERENCIAID = IR.TRANSFERENCIAID 
					                                    AND IR.SITUACAO = 'Recusada'
		                                    LEFT JOIN PATRIMONIO.TRANSFERENCIAITEM IP (NOLOCK) 
                                                    ON T.TRANSFERENCIAID = IP.TRANSFERENCIAID 
					                                    AND IP.SITUACAO = 'Pendente'
                                            INNER JOIN HADES.DBO.HD_SETOR SO (NOLOCK) 
                                                    ON T.SETORORIGEM = SO.SETOR 
                                            INNER JOIN HADES.DBO.HD_SETOR SD (NOLOCK) 
                                                    ON T.SETORDESTINO = SD.SETOR 
                                    WHERE  T.SETORORIGEM = @SETORORIGEM 
                                            AND T.SETORDESTINO = @SETORDESTINO 
                                    GROUP BY T.TRANSFERENCIAID, 
                                                T.DATASOLICITACAO, 
                                                T.SETORDESTINO, 
                                                SD.NOME, 
                                                T.SETORORIGEM, 
                                                SO.NOME 
                                    ORDER BY T.DATASOLICITACAO DESC ";

                contextQuery.Parameters.Add("@SETORORIGEM", SqlDbType.VarChar, setorOrigem);
                contextQuery.Parameters.Add("@SETORDESTINO", SqlDbType.VarChar, setorDestino);

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

        public DataTable ObtemAvisoTransferenciaBensMoveisPor(int lote)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT C.CONTA, 
                                REPLICATE('0', 6 - LEN(I.NUMEROBEMORIGEM)) + CONVERT(VARCHAR(6), I.NUMEROBEMORIGEM) AS NUMEROBEMORIGEM, 
                                B.DESCRICAO, 
                                'unid' AS UNIDADEMEDIDA, 
                                1 AS QUANTIDADE, 
                                M.SIGLA + CONVERT(VARCHAR(100), I.VALOR) AS VALORCOMSIGLA, 
                                B.DOCUMENTOHABIL,
		                        T.DATASOLICITACAO
                        FROM   PATRIMONIO.TRANSFERENCIAITEM I (NOLOCK)
		                        INNER JOIN Patrimonio.TRANSFERENCIA T (NOLOCK)
				                        ON I.TRANSFERENCIAID = T.TRANSFERENCIAID 
                                INNER JOIN PATRIMONIO.MOEDA M (NOLOCK) 
                                        ON I.MOEDAID = M.MOEDAID 
                                INNER JOIN PATRIMONIO.BEM B (NOLOCK) 
                                        ON I.BEMID = B.BEMID 
                                INNER JOIN PATRIMONIO.CLASSIFICACAO C (NOLOCK) 
                                        ON C.CLASSIFICACAOID = B.CLASSIFICACAOID 
                        WHERE  SITUACAO = 'Aceita' 
                                AND T.TRANSFERENCIAID = @TRANSFERENCIAID ";

                contextQuery.Parameters.Add("@TRANSFERENCIAID", SqlDbType.Int, lote);

                dt = contexto.GetDataTable(contextQuery);

                if (dt.Rows.Count == 0)
                {
                    throw new Exception("ERRO: Este Lote não possui itens aceitos.");
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
                        mensagem = ex.Message;
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

            return dt;
        }

        public ValidacaoDados ValidaSolicitacao(Entidades.Transferencia transferencia, List<int> listaBensId)
        {
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();
            RN.Perfil rnPerfil = new Perfil();
            List<string> mensagens = new List<string>();
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new TransferenciaItem();
            string[] bem = new string[2]; 
            RN.Patrimonio.Bem rnBem = new Bem();
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (transferencia == null)
            {
                return validacaoDados;
            }

            transferencia.DataSolicitacao = DateTime.Now.Date;

            if (listaBensId.Count() == 0)
            {
                mensagens.Add("É obrigatorio adicionar ao menos 1 item.");
            }

            if (string.IsNullOrEmpty(transferencia.SetorOrigem))
            {
                mensagens.Add("O campo UNIDADE ADMINISTRATIVA DE ORIGEM é obrigatório.");
            }

            if (string.IsNullOrEmpty(transferencia.SetorDestino))
            {
                mensagens.Add("O campo UNIDADE ADMINISTRATIVA DE DESTINO é obrigatório.");
            }

            if (!string.IsNullOrEmpty(transferencia.SetorOrigem)
                && !string.IsNullOrEmpty(transferencia.SetorDestino)
                && transferencia.SetorOrigem == transferencia.SetorDestino)
            {
                mensagens.Add("As unidades administrativas de ORIGEM e DESTINO devem ser diferentes.");
            }

            if (transferencia.UsuarioSolicitanteId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO SOLICITANTE é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    foreach (int bemId in listaBensId)
                    {
                        //Verifica se já existe uma transferencia pendente para o item
                        if (rnTransferenciaItem.ExisteSolicitacaoPendentePor(contexto, bemId))
                        {
                            //Busca dados do item
                            bem = rnBem.ObtemNumeroDescricaoPor(contexto, bemId);

                            mensagens.Add(string.Format("Já existe uma solicitação de transferência pendente para o item: {0} - {1}.", bem[0], bem[1]));
                        }

                        //Verifica se o item ja tem baixa     
                        if (rnBem.PossuiBaixaPor(contexto, bemId))
                        {
                            //Busca dados do item
                            bem = rnBem.ObtemNumeroDescricaoPor(contexto, bemId);

                            mensagens.Add(string.Format("Não é possível transferir o item: {0} - {1}, pois ele já tem uma baixa cadastrada.", bem[0], bem[1]));
                        }
                        
                        //Verifica se o bem ao menos ficou 1 dia na ua
                        DateTime ultimaDataInicio = rnMovimentacao.ObtemInicioMovimentacaoAtivaPor(contexto, bemId);
                        if (ultimaDataInicio.Date >= transferencia.DataSolicitacao.Date)
                        {
                            //Busca dados do item
                            bem = rnBem.ObtemNumeroDescricaoPor(contexto, bemId);
                           
                            mensagens.Add(string.Format("Não é possível transferir o item: {0} - {1}, pois a data da transferência deve ser maior que a data de incorporação / última transferência.", bem[0], bem[1]));
                        }

                        /*
                        Data: 11/11/2022
                        http://10.11.69.140/ConexaoEducacao/Documentacao/2022/2022.0016 - Melhorias Patrimônio/6 - ETM/Chamado 17511 - Ato de Transferência dos Bens Móveis (ATBM)/ETM03_Patrimônio_Solicitacao_de_Transferencia.doc
                        RN1 - O sistema deve permitir que a solicitação de transferência de bens patrimoniais de qualquer data de aquisição e incorporação seja realizada a qualquer tempo entre as Unidades Administrativas.
                        */

                        //if (!rnPerfil.PossuiPerfilLiberacaoPatrimonioFinalizadoPor(transferencia.UsuarioSolicitanteId))
                        //{
                        //    if (!rnPeriodoLancamento.PossuiPeriodoLancamentoAbertoPor(ultimaDataInicio.Date.Year, transferencia.DataSolicitacao.Date))
                        //    {
                        //        mensagens.Add("A DATA DA TRANSFERÊNCIA está fora do intervalo permitido para lançamento.");
                        //    }
                        //}
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

        public ValidacaoDados ValidaSolicitacao(Entidades.Transferencia transferencia, int bemId)
        {
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new PeriodoLancamento();
            RN.Perfil rnPerfil = new Perfil();
            List<string> mensagens = new List<string>();
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new TransferenciaItem();
            string[] bem = new string[2];
            RN.Patrimonio.Bem rnBem = new Bem();
            RN.Patrimonio.Movimentacao rnMovimentacao = new Movimentacao();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (transferencia == null)
            {
                return validacaoDados;
            }

            transferencia.DataSolicitacao = DateTime.Now.Date;

            if (string.IsNullOrEmpty(transferencia.SetorOrigem))
            {
                mensagens.Add("O campo UNIDADE ADMINISTRATIVA DE ORIGEM é obrigatório.");
            }

            if (string.IsNullOrEmpty(transferencia.SetorDestino))
            {
                mensagens.Add("O campo UNIDADE ADMINISTRATIVA DE DESTINO é obrigatório.");
            }

            if (!string.IsNullOrEmpty(transferencia.SetorOrigem)
                && !string.IsNullOrEmpty(transferencia.SetorDestino)
                && transferencia.SetorOrigem == transferencia.SetorDestino)
            {
                mensagens.Add("As unidades administrativas de ORIGEM e DESTINO devem ser diferentes.");
            }

            if (transferencia.UsuarioSolicitanteId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO SOLICITANTE é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe uma transferencia pendente para o item
                    if (rnTransferenciaItem.ExisteSolicitacaoPendentePor(contexto, bemId))
                    {
                        //Busca dados do item
                        bem = rnBem.ObtemNumeroDescricaoPor(contexto, bemId);

                        mensagens.Add(string.Format("Já existe uma solicitação de transferência pendente para o item: {0} - {1}.", bem[0], bem[1]));
                    }

                    //Verifica se o item ja tem baixa     
                    if (rnBem.PossuiBaixaPor(contexto, bemId))
                    {
                        //Busca dados do item
                        bem = rnBem.ObtemNumeroDescricaoPor(contexto, bemId);

                        mensagens.Add(string.Format("Não é possível transferir o item: {0} - {1}, pois ele já tem uma baixa cadastrada.", bem[0], bem[1]));
                    }

                    //Verifica se o bem ao menos ficou 1 dia na ua
                    DateTime ultimaDataInicio = rnMovimentacao.ObtemInicioMovimentacaoAtivaPor(contexto, bemId);
                    if (ultimaDataInicio.Date >= transferencia.DataSolicitacao.Date)
                    {
                        //Busca dados do item
                        bem = rnBem.ObtemNumeroDescricaoPor(contexto, bemId);

                        mensagens.Add(string.Format("Não é possível transferir o item: {0} - {1}, pois a data da transferência deve ser maior que a data de incorporação / última transferência.", bem[0], bem[1]));
                    }

                    /*
                    Data: 11/11/2022
                    http://10.11.69.140/ConexaoEducacao/Documentacao/2022/2022.0016 - Melhorias Patrimônio/6 - ETM/Chamado 17511 - Ato de Transferência dos Bens Móveis (ATBM)/ETM03_Patrimônio_Solicitacao_de_Transferencia.doc
                    RN1 - O sistema deve permitir que a solicitação de transferência de bens patrimoniais de qualquer data de aquisição e incorporação seja realizada a qualquer tempo entre as Unidades Administrativas.
                    */

                    //if (!rnPerfil.PossuiPerfilLiberacaoPatrimonioFinalizadoPor(transferencia.UsuarioSolicitanteId))
                    //{
                    //    if (!rnPeriodoLancamento.PossuiPeriodoLancamentoAbertoPor(ultimaDataInicio.Date.Year, transferencia.DataSolicitacao.Date))
                    //    {
                    //        mensagens.Add("A DATA DA TRANSFERÊNCIA está fora do intervalo permitido para lançamento.");
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

        public void SolicitaTransferencia(Entidades.Transferencia transferencia, List<int> listaBensId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Patrimonio.TransferenciaItem rnTransferenciaItem = new TransferenciaItem();
            Patrimonio.Entidades.TransferenciaItem transferenciaItem = new Techne.Lyceum.RN.Patrimonio.Entidades.TransferenciaItem();
            try
            {
                //Insere Transferencia
                this.Insere(contexto, transferencia);

                //Alimenta entidade TransferenciaItem
                transferenciaItem.TransferenciaId = transferencia.TransferenciaId;
                transferenciaItem.Situacao = TransferenciaItem.Pendente;

                //Insere itens
                foreach (int bemId in listaBensId)
                {
                    transferenciaItem.BemId = bemId;

                    //Insere TransferenciaItem
                    rnTransferenciaItem.Insere(contexto, transferenciaItem);
                }
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

        private void Insere(DataContext contexto, Entidades.Transferencia transferencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Patrimonio.TRANSFERENCIA
                                                (SETORORIGEM, 
                                                 SETORDESTINO, 
                                                 USUARIOSOLICITANTEID,                                                  
                                                 DATASOLICITACAO) 
                                    VALUES      (@SETORORIGEM, 
                                                 @SETORDESTINO, 
                                                 @USUARIOSOLICITANTEID, 
                                                 @DATASOLICITACAO)
                                			
                                SELECT IDENT_CURRENT('Patrimonio.TRANSFERENCIA') ";

            contextQuery.Parameters.Add("@SETORORIGEM", SqlDbType.VarChar, transferencia.SetorOrigem);
            contextQuery.Parameters.Add("@SETORDESTINO", SqlDbType.VarChar, transferencia.SetorDestino);
            contextQuery.Parameters.Add("@USUARIOSOLICITANTEID", SqlDbType.VarChar, transferencia.UsuarioSolicitanteId);
            contextQuery.Parameters.Add("@DATASOLICITACAO", SqlDbType.DateTime, DateTime.Now);

            transferencia.TransferenciaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(DataContext contexto, int transferenciaId, string usuarioAndamento, DateTime dataAndamento, DateTime dataMovimentacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PATRIMONIO.TRANSFERENCIA
                                       SET USUARIOANDAMENTOID = @USUARIOANDAMENTOID,
                                          DATAANDAMENTO = @DATAANDAMENTO,
                                          DATAMOVIMENTACAO = @DATAMOVIMENTACAO
                                    WHERE TRANSFERENCIAID = @TRANSFERENCIAID ";

            contextQuery.Parameters.Add("@USUARIOANDAMENTOID", SqlDbType.VarChar, usuarioAndamento);
            contextQuery.Parameters.Add("@DATAANDAMENTO", SqlDbType.DateTime, dataAndamento);
            contextQuery.Parameters.Add("@DATAMOVIMENTACAO", SqlDbType.DateTime, dataMovimentacao.Date);
            contextQuery.Parameters.Add("@TRANSFERENCIAID", SqlDbType.Int, transferenciaId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}