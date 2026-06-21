using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ExigenciaEvento
    {
        public int ObtemquantidadeExigenciasPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1) AS QTDE
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QTDE"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public bool PossuiEventoPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiExigenciaAbertaPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID
											AND APROVADO = 0 ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiEventoExigenciaAbertaPor(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiEventoExigenciaAbertaPor(contexto, eventoId);
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

        public bool PossuiEventoExigenciaAbertaPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EVENTO E (NOLOCK)
										INNER JOIN PrestacaoContas.EXIGENCIAEVENTO EX (NOLOCK) ON E.EVENTOID = EX.EVENTOID
                                    WHERE E.EVENTOID = @EVENTOID
											AND EX.APROVADO = 0
											AND E.APROVADO IS NULL ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiMotivoExigenciaEventoPor(DataContext contexto, int motivoExigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE MOTIVOEXIGENCIAEVENTOID = @MOTIVOEXIGENCIAEVENTOID ";

            contextQuery.Parameters.Add("@MOTIVOEXIGENCIAEVENTOID", SqlDbType.Int, motivoExigenciaEventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaEventosReprovadosExigenciasPor(DateTime dataInicio, DateTime dataFim, string censo, int? finalizadeId, int? eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT    EX.EXIGENCIAEVENTOID,
		                                            E.EVENTOID,
		                                            E.CENSO,
		                                            UE.NOME_COMP AS ESCOLA,
		                                            E.NUMEROEVENTO,
		                                            F.DESCRICAO AS FINALIDADE,
		                                            CASE 
                                                       WHEN E.TIPODESPESA = 0 THEN 'Despesa Comum'
                                                       WHEN E.TIPODESPESA = 1 THEN 'Pequena Despesa Com comprovação'
										               WHEN E.TIPODESPESA = 2 THEN 'Pequena Despesa Sem comprovação'
                                                       WHEN E.TIPODESPESA = 3 THEN 'Pequena Despesa com Translado de Servidores'
                                                           
                                                    END TIPODESPESA,
		                                            E.DATACADASTRO AS DATAEVENTO,
		                                            E.DATANOTAFISCAL,
		                                            E.DATAPAGAMENTO,
		                                            E.VALORPAGAMENTO,
		                                            M.DESCRICAO AS MOTIVO,
		                                            EX.REJEITADO,
		                                            EX.CUMPRIDA
                                            from [PrestacaoContas].[EVENTO] E
		                                            INNER JOIN [PRESTACAOCONTAS].[PLANOTRABALHO] PT (NOLOCK)
				                                            ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
		                                            INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
				                                            ON E.CENSO = UE.UNIDADE_ENS
		                                            INNER JOIN PRESTACAOCONTAS.FINALIDADE F (NOLOCK)
				                                            ON PT.FINALIDADEID = F.FINALIDADEID
		                                            LEFT JOIN PRESTACAOCONTAS.PEQUENADESPESA PD (NOLOCK)
				                                            ON E.EVENTOID = PD.EVENTOID
		                                            INNER JOIN PRESTACAOCONTAS.EXIGENCIAEVENTO EX (NOLOCK)
				                                            ON E.EVENTOID = EX.EVENTOID
		                                            INNER JOIN PRESTACAOCONTAS.MOTIVOEXIGENCIAEVENTO M (NOLOCK)
				                                            ON EX.MOTIVOEXIGENCIAEVENTOID = M.MOTIVOEXIGENCIAEVENTOID
                                            WHERE E.APROVADO = 0 --COM EXIGENCIAS
	                                              AND ISNULL(EX.APROVADO, 0) = 0 --EXIGENCIAS 
	                                              AND E.DATAPAGAMENTO BETWEEN  @DATAINICIO AND @DATAFIM
	                                              AND E.CENSO = @CENSO
	                                              AND PT.FINALIDADEID = ISNULL(@FINALIDADEID, PT.FINALIDADEID)	                                              
	                                              AND E.EVENTOID = ISNULL(@EVENTOID, E.EVENTOID) ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalizadeId == null ? (object)DBNull.Value : finalizadeId);
                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId == null ? (object)DBNull.Value : eventoId);

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

        public DataTable ListaTodosEventosExigenciasReprovadasPor(DateTime dataInicio, DateTime dataFim, string censo, int? finalizadeId, int? eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT    EX.EXIGENCIAEVENTOID,
		                                            E.EVENTOID,
		                                            E.CENSO,
		                                            UE.NOME_COMP AS ESCOLA,
		                                            E.NUMEROEVENTO,
		                                            F.DESCRICAO AS FINALIDADE,
		                                            CASE 
                                                       WHEN E.TIPODESPESA = 0 THEN 'Despesa Comum'
                                                       WHEN E.TIPODESPESA = 1 THEN 'Pequena Despesa Com comprovação'
										               WHEN E.TIPODESPESA = 2 THEN 'Pequena Despesa Sem comprovação'
                                                       WHEN E.TIPODESPESA = 3 THEN 'Pequena Despesa com Translado de Servidores'
                                                           
                                                    END TIPODESPESA,
		                                            E.DATACADASTRO AS DATAEVENTO,
		                                            E.DATANOTAFISCAL,
		                                            E.DATAPAGAMENTO,
		                                            E.VALORPAGAMENTO,
		                                            M.DESCRICAO AS MOTIVO,
		                                            EX.REJEITADO,
		                                            EX.CUMPRIDA
                                            from [PrestacaoContas].[EVENTO] E
		                                            INNER JOIN [PRESTACAOCONTAS].[PLANOTRABALHO] PT (NOLOCK)
				                                            ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
		                                            INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
				                                            ON E.CENSO = UE.UNIDADE_ENS
		                                            INNER JOIN PRESTACAOCONTAS.FINALIDADE F (NOLOCK)
				                                            ON PT.FINALIDADEID = F.FINALIDADEID
		                                            LEFT JOIN PRESTACAOCONTAS.PEQUENADESPESA PD (NOLOCK)
				                                            ON E.EVENTOID = PD.EVENTOID
		                                            INNER JOIN PRESTACAOCONTAS.EXIGENCIAEVENTO EX (NOLOCK)
				                                            ON E.EVENTOID = EX.EVENTOID
		                                            INNER JOIN PRESTACAOCONTAS.MOTIVOEXIGENCIAEVENTO M (NOLOCK)
				                                            ON EX.MOTIVOEXIGENCIAEVENTOID = M.MOTIVOEXIGENCIAEVENTOID
                                            WHERE (E.APROVADO IS NULL OR E.APROVADO = 1) --SOMENTE DESPESAS APROVADAS OU PENDENTES
                                                  AND ISNULL(EX.APROVADO, 0) = 0 --EXIGENCIAS 
	                                              AND E.DATAPAGAMENTO BETWEEN  @DATAINICIO AND @DATAFIM
	                                              AND E.CENSO = @CENSO
	                                              AND PT.FINALIDADEID = ISNULL(@FINALIDADEID, PT.FINALIDADEID)	                                              
	                                              AND E.EVENTOID = ISNULL(@EVENTOID, E.EVENTOID) ";

                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalizadeId == null ? (object)DBNull.Value : finalizadeId);
                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId == null ? (object)DBNull.Value : eventoId);

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

        public DataTable ListaExigenciasPor(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  EX.EXIGENCIAEVENTOID,
		                                          EX.EVENTOID,
                                                  EX.MOTIVOEXIGENCIAEVENTOID,
		                                          M.DESCRICAO AS MOTIVO,
		                                          EX.NOTAEXPLICATIVA,
		                                          EX.JUSTIFICATIVA,
		                                          EX.APROVADO,
		                                          A.EXIGENCIAEVENTOARQUIVOID,
		                                          A.TIPOARQUIVO,
                                                  A.ARQUIVO,
												  EC.VALOR AS VALORRESSARCIMENTO,
												  EC.DATAEVENTO AS DATARESSARCIMENTO,
		                                          EX.REJEITADO,
		                                          EX.CUMPRIDA
                                            FROM [PRESTACAOCONTAS].EXIGENCIAEVENTO EX (NOLOCK)
		                                            INNER JOIN PRESTACAOCONTAS.MOTIVOEXIGENCIAEVENTO M (NOLOCK)
				                                            ON EX.MOTIVOEXIGENCIAEVENTOID = M.MOTIVOEXIGENCIAEVENTOID
		                                            LEFT JOIN [PRESTACAOCONTAS].[EXIGENCIAEVENTOARQUIVO] A (NOLOCK)
				                                            ON EX.EXIGENCIAEVENTOID = A.EXIGENCIAEVENTOID
													LEFT JOIN PrestacaoContas.EVENTOCREDITO EC (NOLOCK)
															ON EX.EXIGENCIAEVENTOID = EC.EXIGENCIAEVENTOID
                                            WHERE EX.EVENTOID = @EVENTOID ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

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

        private bool EhExigenciaCadastradaPor(DataContext contexto, int eventoId, int motivoExigenciaEventoId, string notaExplicativa, Date dataCadastro)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE MOTIVOEXIGENCIAEVENTOID = @MOTIVOEXIGENCIAEVENTOID
										AND EVENTOID = @EVENTOID
										AND NOTAEXPLICATIVA = @NOTAEXPLICATIVA
										AND CONVERT(DATE, DATACADASTRO) = CONVERT(DATE, @DATACADASTRO)";

            contextQuery.Parameters.Add("@MOTIVOEXIGENCIAEVENTOID", SqlDbType.Int, motivoExigenciaEventoId);
            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
            contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, notaExplicativa);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.Date, dataCadastro);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool EhExigenciaAprovadoPor(DataContext contexto, int exigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID
                                          AND APROVADO = 1 ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool EhExigenciaCorrigidaPor(DataContext contexto, int exigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID
                                          AND CUMPRIDA = 1 ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool EhExigenciaRejeitadaPor(DataContext contexto, int exigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID
                                          AND REJEITADO = 1 ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaCorrigeExigencia(string censo, int planoTrabalhoId, string justificativa, decimal? valorRessarcimento, DateTime? dataRessarcimento, Entidades.ExigenciaEventoArquivo exigenciaEventoArquivo)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE ENSINO é obrigatório.");
            }

            if (justificativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo JUSTIFICATIVA é obrigatório.");
            }
            else if (justificativa.Length > 500)
            {
                mensagens.Add("Campo JUSTIFICATIVA deve conter no máximo 500 caracteres.");
            }

            if (exigenciaEventoArquivo.ExigenciaEventoId <= 0)
            {
                mensagens.Add("Campo CODIGO DA EXIGÊNCIA é obrigatório.");
            }

            if (exigenciaEventoArquivo == null)
            {
                mensagens.Add("Campo ARQUIVO é obrigatório.");
            }
            else
            {
                if (exigenciaEventoArquivo.Arquivo == null || exigenciaEventoArquivo.Arquivo.Count() <= 0)
                {
                    mensagens.Add("Campo ARQUIVO é obrigatório.");
                }

                if (exigenciaEventoArquivo.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo TIPO ARQUIVO é obrigatório.");
                }
                else
                {
                    //Apenas aceitar pdf e imagem 
                    if (exigenciaEventoArquivo.TipoArquivo.ToUpper() != "IMAGE/JPEG"
                        && exigenciaEventoArquivo.TipoArquivo.ToUpper() != "APPLICATION/PDF")
                    {
                        mensagens.Add("Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                    }
                }

                if (exigenciaEventoArquivo.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo NOME ARQUIVO é obrigatório.");
                }
            }

            if (exigenciaEventoArquivo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Somente exigências que não foram aprovadas poderão ser editadas
                    if (this.EhExigenciaAprovadoPor(contexto, exigenciaEventoArquivo.ExigenciaEventoId))
                    {
                        mensagens.Add("Esta exigência não pode ser alterada pois já foi aprovada.");
                    }

                    //Verifica se a exigencia precisa de ressarcimento
                    if (this.NecessitaRessarcimentoPor(contexto, exigenciaEventoArquivo.ExigenciaEventoId))
                    {
                        //Para exigencias que necessitem de ressarcimento é necessario informar Valor e Data de Credito
                        if (valorRessarcimento == null || valorRessarcimento <= 0)
                        {
                            mensagens.Add("Campo VALOR DO RESSARCIMENTO é obrigatório.");
                        }

                        if (dataRessarcimento == null || dataRessarcimento == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA DO RESSARCIMENTO é obrigatório.");
                        }
                    }
                    else
                    {
                        //Caso contrario não pode informar Valor e Data de Credito
                        if (valorRessarcimento != null && valorRessarcimento > 0)
                        {
                            mensagens.Add("Campo VALOR DO RESSARCIMENTO não pode ser informado para esta exigência.");
                        }

                        if (dataRessarcimento != null && dataRessarcimento != DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA DO RESSARCIMENTO não pode ser informado para esta exigência.");
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

        public bool NecessitaRessarcimentoPor(int exigenciaEventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.NecessitaRessarcimentoPor(contexto, exigenciaEventoId);
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

        private bool NecessitaRessarcimentoPor(DataContext ctx, int exigenciaEventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM PrestacaoContas.EXIGENCIAEVENTO e (nolock)
									inner join PrestacaoContas.MOTIVOEXIGENCIAEVENTO m (NOLOCK) 
											on e.MOTIVOEXIGENCIAEVENTOID = m.MOTIVOEXIGENCIAEVENTOID
                                WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID
	                                AND RESSARCIMENTO = 1 ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void CorrigeExigencia(string censo, int planoTrabalhoId, string justificativa, decimal? valorRessarcimento, DateTime? dataRessarcimento, Entidades.ExigenciaEventoArquivo exigenciaEventoArquivo)
        {
            ExigenciaEventoArquivo rnExigenciaEventoArquivo = new ExigenciaEventoArquivo();
            EventoCredito rnEventoCredito = new EventoCredito();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Entidades.EventoCredito eventoCredito = new Techne.Lyceum.RN.PrestacaoContas.Entidades.EventoCredito();

            try
            {
                //Atualizar campos da exigencia
                this.CorrigeExigencia(contexto, justificativa, exigenciaEventoArquivo);

                //Verifica se é ressarcimento (Campo Valor informado)
                if (valorRessarcimento != null && valorRessarcimento > 0)
                {
                    //Para exigencias que necessitem de ressarcimento (informado Valor e Data de Credito) é necessario criar o evento de credito

                    //Monta Numero evento de credito
                    eventoCredito.ExigenciaEventoId = exigenciaEventoArquivo.ExigenciaEventoId;
                    eventoCredito.PlanoTrabalhoId = planoTrabalhoId;
                    eventoCredito.Censo = censo;
                    eventoCredito.Valor = Convert.ToDecimal(valorRessarcimento);
                    eventoCredito.DataEvento = Convert.ToDateTime(dataRessarcimento);
                    eventoCredito.UsuarioId = exigenciaEventoArquivo.UsuarioId;

                    //Verifica se já existe um evento de credito para a exigencia
                    if (rnEventoCredito.PossuiCreditoExigenciaEventoPor(contexto, exigenciaEventoArquivo.ExigenciaEventoId))
                    {
                        rnEventoCredito.Atualiza(contexto, eventoCredito);
                    }
                    else
                    {
                        //Insere evento de credito
                        rnEventoCredito.Insere(contexto, eventoCredito);

                        // A sigla do evento deve sempre ser OE (Outras Entradas)
                        string siglaEventoCredito = "OE";

                        //Composição do Numero do Evento de crédito (Tabela EVENTOCREDITO campo NUMEROEVENTO)
                        //Sigla “EC” + ANO Corrente do Sistema + Sigla do Evento de Crédito + Sequencial (com 10 dígitos, no caso preencher com zero para formar os 10 dígitos)
                        //Monta inicio da sigla sem o Sequencial com 10 digitos
                        eventoCredito.NumeroEvento = string.Format("EC{0}{1}", DateTime.Now.Year.ToString(), siglaEventoCredito);

                        //Ex: Ano corrente é 2021 e o EVENTOID criado é 6.
                        //Logo o numero do evento será: EC2021OE0000000006
                        eventoCredito.NumeroEvento = eventoCredito.NumeroEvento + eventoCredito.EventoCreditoId.ToString().PadLeft(10, '0');

                        //Atualiza numero 
                        rnEventoCredito.AtualizaNumeroEvento(contexto, eventoCredito.EventoCreditoId, eventoCredito.NumeroEvento);
                    }
                }

                //Anexar arquivo
                if (rnExigenciaEventoArquivo.PossuiExigenciaEventoPor(contexto, exigenciaEventoArquivo.ExigenciaEventoId))
                {
                    //Insere arquivo
                    rnExigenciaEventoArquivo.Atualiza(contexto, exigenciaEventoArquivo);

                    //Insere auditoria
                    rnExigenciaEventoArquivo.InsereAuditoria(contexto, exigenciaEventoArquivo, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);
                }
                else
                {
                    //Insere arquivo
                    rnExigenciaEventoArquivo.Insere(contexto, exigenciaEventoArquivo);

                    //Insere auditoria
                    rnExigenciaEventoArquivo.InsereAuditoria(contexto, exigenciaEventoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
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

        private void CorrigeExigencia(DataContext contexto, string justificativa, Entidades.ExigenciaEventoArquivo exigenciaEventoArquivo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EXIGENCIAEVENTO
                                               SET JUSTIFICATIVA = @JUSTIFICATIVA,
                                                   CUMPRIDA = @CUMPRIDA,
                                                   USUARIOID = @USUARIOID,
                                                   DATAALTERACAO = @DATAALTERACAO,
                                                   REJEITADO = @REJEITADO
                                             WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

            contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoArquivo.ExigenciaEventoId);
            contextQuery.Parameters.Add("@JUSTIFICATIVA", SqlDbType.VarChar, justificativa);
            contextQuery.Parameters.Add("@REJEITADO", SqlDbType.Bit, false); //Ao reponder novamente a exigencia, o campo rejeitado volta ao normal
            contextQuery.Parameters.Add("@CUMPRIDA", SqlDbType.Bit, true); //Ao reponder a exigencia, ela passa a ser cumprida
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaEventoArquivo.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados Valida(Entidades.ExigenciaEvento exigenciaEvento, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Evento rnEvento = new Evento();
            EventoCredito rnEventoCredito = new EventoCredito();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (!cadastro)
            {
                if (exigenciaEvento.ExigenciaEventoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório para alteraçao.");
                }
            }

            if (exigenciaEvento.EventoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA DESPESA é obrigatório.");
            }

            if (exigenciaEvento.MotivoExigenciaEventoId <= 0)
            {
                mensagens.Add("Campo TIPO DE EXIGÊNCIA é obrigatório.");
            }

            if (exigenciaEvento.NotaExplicativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOTA EXPLICATIVA é obrigatório.");
            }

            if (exigenciaEvento.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                    exigenciaEvento.Aprovado = false;
                    exigenciaEvento.Rejeitado = false;

                    //Somente eventos não foram aprovadas poderão ter exigencias incluidas / editadas
                    if (rnEvento.EhEventoAprovadoPor(contexto, exigenciaEvento.EventoId))
                    {
                        mensagens.Add("Esta exigência não pode ser incluída / alterada pois a despesa já foi validada.");
                    }

                    //Verifica se é alteraçao
                    if (cadastro)
                    { 
                        if (this.EhExigenciaCadastradaPor(contexto, exigenciaEvento.EventoId, exigenciaEvento.MotivoExigenciaEventoId, exigenciaEvento.NotaExplicativa, DateTime.Now))
                        {
                            mensagens.Add("Esta exigência já foi cadastrada para essa despesa / data.");
                        }
                    }
                    else
                    {
                        //Somente exigências que não foram aprovadas poderão ser editadas
                        if (this.EhExigenciaAprovadoPor(contexto, exigenciaEvento.ExigenciaEventoId))
                        {
                            mensagens.Add("Esta exigência não pode ser alterada pois já foi aprovada.");
                        }

                        //Verifica se a exigencia já teve valor de ressarcimento lançado
                        if (rnEventoCredito.PossuiCreditoExigenciaEventoPor(contexto, exigenciaEvento.ExigenciaEventoId))
                        {
                            mensagens.Add("Esta exigência não pode ser alterada pois já foi realizado um ressarcimento de valor.");
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

        public void Insere(Entidades.ExigenciaEvento exigenciaEvento)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Evento rnEvento = new Evento();

            try
            {
                //Insere Exigencia
                this.Insere(contexto, exigenciaEvento);
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

        private void Insere(DataContext contexto, Entidades.ExigenciaEvento exigenciaEvento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.EXIGENCIAEVENTO
                                                   (MOTIVOEXIGENCIAEVENTOID
                                                   ,EVENTOID
                                                   ,NOTAEXPLICATIVA
                                                   ,APROVADO
                                                   ,REJEITADO
                                                   ,CUMPRIDA
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO)
                                             VALUES
                                                   (@MOTIVOEXIGENCIAEVENTOID,
                                                   @EVENTOID, 
                                                   @NOTAEXPLICATIVA, 
                                                   @APROVADO,
                                                   0, 
                                                   0,
                                                   @USUARIOID, 
                                                   @DATACADASTRO, 
                                                   @DATAALTERACAO)  ";

            contextQuery.Parameters.Add("@MOTIVOEXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEvento.MotivoExigenciaEventoId);
            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, exigenciaEvento.EventoId);
            contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, exigenciaEvento.NotaExplicativa);
            contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, exigenciaEvento.Aprovado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaEvento.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(Entidades.ExigenciaEvento exigenciaEvento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.EXIGENCIAEVENTO
                                           SET MOTIVOEXIGENCIAEVENTOID = @MOTIVOEXIGENCIAEVENTOID,
                                               NOTAEXPLICATIVA = @NOTAEXPLICATIVA, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO
                                         WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEvento.ExigenciaEventoId);
                contextQuery.Parameters.Add("@MOTIVOEXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEvento.MotivoExigenciaEventoId);
                contextQuery.Parameters.Add("@NOTAEXPLICATIVA", SqlDbType.VarChar, exigenciaEvento.NotaExplicativa);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, exigenciaEvento.UsuarioId);
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

        public ValidacaoDados ValidaAprovacao(int exigenciaEventoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Evento rnEvento = new Evento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (exigenciaEventoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA EXIGÊNCIA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a exigencia foi aprovada
                    if (EhExigenciaAprovadoPor(contexto, exigenciaEventoId))
                    {
                        mensagens.Add("Esta exigência já foi aprovada.");
                    }

                    //Verifica se o evento foi corrigido
                    if (!EhExigenciaCorrigidaPor(contexto, exigenciaEventoId))
                    {
                        mensagens.Add("Esta exigência não pode ser aprovada pois ainda não respondida / corrigida.");
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

        public void Aprova(int exigenciaEventoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.EXIGENCIAEVENTO
                                           SET APROVADO = @APROVADO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO
                                         WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);
                contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, true);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
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

        public ValidacaoDados ValidaRejeicao(int exigenciaEventoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Evento rnEvento = new Evento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (exigenciaEventoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA EXIGÊNCIA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a exigencia foi corrigido
                    if (!EhExigenciaCorrigidaPor(contexto, exigenciaEventoId))
                    {
                        mensagens.Add("Esta exigência não pode ser aprovada pois ainda não respondida / corrigida.");
                    }

                    //Verifica se a exigencia foi aprovada
                    if (EhExigenciaAprovadoPor(contexto, exigenciaEventoId))
                    {
                        mensagens.Add("Esta exigência não pode ser pois rejeitada pois já foi aprovada.");
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

        public void Rejeita(int exigenciaEventoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.EXIGENCIAEVENTO
                                           SET APROVADO = @APROVADO, 
                                               USUARIOID = @USUARIOID, 
                                               REJEITADO = @REJEITADO,
                                               CUMPRIDA = @CUMPRIDA,
                                               DATAALTERACAO = @DATAALTERACAO
                                         WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);
                contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, false);
                contextQuery.Parameters.Add("@REJEITADO", SqlDbType.Bit, true);
                contextQuery.Parameters.Add("@CUMPRIDA", SqlDbType.Bit, false);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
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

        public bool PossuiExigenciaSemAnalisePor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" 	SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID
										 AND APROVADO = 0 AND REJEITADO = 0 ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiExigenciaRejeitadaPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" 	SELECT COUNT(1) 
                                    FROM PrestacaoContas.EXIGENCIAEVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID
										 AND APROVADO = 0 AND REJEITADO = 1 ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Remove(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  insert into PrestacaoContas.[EXIGENCIAEVENTO_EXCLUIDO]
                                         SELECT [EXIGENCIAEVENTOID]
                                              ,[MOTIVOEXIGENCIAEVENTOID]
                                              ,[EVENTOID]
                                              ,[NOTAEXPLICATIVA]
                                              ,[JUSTIFICATIVA]
                                              ,[APROVADO]
                                              ,[REJEITADO]
                                              ,[USUARIOID]
                                              ,[DATACADASTRO]
                                              ,[DATAALTERACAO]
                                              ,GETDATE()
                                              ,[CUMPRIDA] 
                                        FROM PrestacaoContas.EXIGENCIAEVENTO                                    
                                        WHERE EVENTOID = @EVENTOID 

                                        DELETE PrestacaoContas.EXIGENCIAEVENTO                                    
                                        WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(int exigenciaEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT INTO PrestacaoContas.[EXIGENCIAEVENTO_EXCLUIDO]
                                        SELECT [EXIGENCIAEVENTOID]
                                              ,[MOTIVOEXIGENCIAEVENTOID]
                                              ,[EVENTOID]
                                              ,[NOTAEXPLICATIVA]
                                              ,[JUSTIFICATIVA]
                                              ,[APROVADO]
                                              ,[REJEITADO]
                                              ,[USUARIOID]
                                              ,[DATACADASTRO]
                                              ,[DATAALTERACAO]
                                              ,GETDATE()
                                              ,[CUMPRIDA] 
                                         FROM PrestacaoContas.EXIGENCIAEVENTO                                    
                                        WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID

                                        DELETE PrestacaoContas.EXIGENCIAEVENTO                                    
                                        WHERE EXIGENCIAEVENTOID = @EXIGENCIAEVENTOID ";

                contextQuery.Parameters.Add("@EXIGENCIAEVENTOID", SqlDbType.Int, exigenciaEventoId);

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

    }
}