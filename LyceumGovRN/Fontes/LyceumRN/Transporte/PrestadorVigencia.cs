using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class PrestadorVigencia
    {
        public DataTable ListaPor(int prestadorId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PV.PRESTADORVIGENCIAID, 
                                            PV.PRESTADORID, 
											RE.REGIONAL,
											M.NOME AS MUNICIPIODESCRICAO,
                                            PV.CENSO,
											UE.NOME_COMP AS ESCOLA,
                                            PV.DATAINICIO, 
                                            PV.DATAFIM, 
                                            PV.USUARIOID, 
                                            PV.DATACADASTRO, 
                                            PV.DATAALTERACAO
                                    FROM   [Transporte].[PRESTADORVIGENCIA] PV (NOLOCK)
									    INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                               ON PV.CENSO = UE.UNIDADE_ENS 
                                       INNER JOIN TCE_REGIONAL RE (NOLOCK) 
                                               ON UE.ID_REGIONAL = RE.ID_REGIONAL 
                                       INNER JOIN HADES.DBO.TCE_MUNICIPIO M (NOLOCK) 
                                               ON UE.MUNICIPIO = M.ID_MUNICIPIO 
                                    WHERE PRESTADORID = @PRESTADORID ";

                contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

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

        public ValidacaoDados Valida(Entidades.PrestadorVigencia prestadorVigencia, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            Pagamento rnPagamento = new Pagamento();
            RN.UsuarioUnidadeFis rnUsuarioUnidadeFis = new UsuarioUnidadeFis();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorVigencia == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (prestadorVigencia.PrestadorId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (prestadorVigencia.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (prestadorVigencia.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INICIO é obrigatório.");
            }

            if (prestadorVigencia.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }
            else
            {
                if (prestadorVigencia.DataInicio != DateTime.MinValue && prestadorVigencia.DataInicio.Date > prestadorVigencia.DataFim.Date)
                {
                    mensagens.Add("A DATA INICIO deve ser menor ou igual a DATA FIM.");
                }
            }

            if (prestadorVigencia.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se usuario tem permissão naquela escola
                    if (!rnUsuarioUnidadeFis.PossuiPermissaoPor(contexto, prestadorVigencia.UsuarioId, prestadorVigencia.Censo))
                    {
                        mensagens.Add("Você não possui permissão para alterações nesta escola.");
                    }

                    //Verifica se o período de vigencia está em intervalo de outro
                    if (this.PossuiOutraVigenciaPor(contexto, prestadorVigencia.PrestadorId, prestadorVigencia.Censo, prestadorVigencia.DataInicio, prestadorVigencia.DataFim, prestadorVigencia.PrestadorVigenciaId))
                    {
                        mensagens.Add("Já existe uma vigência cadastrada neste período para o prestador / escola.");
                    }

                    if (!cadastro)
                    {
                        //Verifica se teve pagamento no periodo
                        if (rnPagamento.PossuiPagamentoPeriodoPor(contexto, prestadorVigencia.PrestadorVigenciaId))
                        {
                            mensagens.Add("Está vigência não pode ser alterada pois possui pagamentos neste invervalo.");
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

        public bool PossuiPrestadorVigenciaPor(DataContext ctx, int prestadorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORVIGENCIA] (NOLOCK)
                                WHERE PRESTADORID = @PRESTADORID ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public List<DateTime> RetornaDiasSemVigenciaPor(DataContext contexto, int prestadorId, string censo, DateTime dataInicio, DateTime dataFim)
        {
            List<DateTime> diasSemVigencia= new List<DateTime>();

            for (DateTime i = dataInicio; i.Date <= dataFim.Date; i = i.AddDays(1))
            {
                DateTime data = i;

                //Verificar se possui vigencia
                if (!this.PossuiVigenciaAbertaPor(contexto, prestadorId, censo, data))
                {
                    diasSemVigencia.Add(data);
                }
            }

            return diasSemVigencia;
        }

        public bool PossuiVigenciaAbertaPor(DataContext ctx, int prestadorId, string censo, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Transporte].[PRESTADORVIGENCIA] (NOLOCK)
                                WHERE PRESTADORID = @PRESTADORID
                                        AND CENSO = @CENSO
										AND (@DATA BETWEEN DATAINICIO AND DATAFIM) ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOutraVigenciaPor(DataContext ctx, int prestadorId, string censo, DateTime dataInicio, DateTime dataFim, int prestadorVigenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM    [Transporte].[PRESTADORVIGENCIA]  ( NOLOCK )
                                            WHERE   PRESTADORID = @PRESTADORID
                                                    AND CENSO = @CENSO 
                                                    AND PRESTADORVIGENCIAID <> @PRESTADORVIGENCIAID                                                    
                                                    AND (
				                                            (@DATAINICIO <= CONVERT(DATE, DATAINICIO) AND @DATAFIM >= CONVERT(DATE, DATAFIM))
				                                            OR (@DATAINICIO BETWEEN  CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
			                                                OR (@DATAFIM BETWEEN CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
			                                             ) ";

                contextQuery.Parameters.Add("@PRESTADORVIGENCIAID", SqlDbType.Int, prestadorVigenciaId);
                contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim.Date);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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
        }

        public void Insere(Entidades.PrestadorVigencia prestadorVigencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(ctx, prestadorVigencia);
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

        public void Insere(DataContext contexto, Entidades.PrestadorVigencia prestadorVigencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO Transporte.PRESTADORVIGENCIA 
                                                        (PRESTADORID, 
                                                         CENSO,
                                                         DATAINICIO, 
                                                         DATAFIM, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@PRESTADORID, 
                                                         @CENSO,
                                                         @DATAINICIO, 
                                                         @DATAFIM, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorVigencia.PrestadorId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, prestadorVigencia.Censo);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, prestadorVigencia.DataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, prestadorVigencia.DataFim.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, prestadorVigencia.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(Entidades.PrestadorVigencia prestadorVigencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  UPDATE Transporte.PRESTADORVIGENCIA
                                           SET DATAINICIO = @DATAINICIO, 
                                              DATAFIM = @DATAFIM, 
                                              USUARIOID = @USUARIOID, 
                                              DATAALTERACAO = @DATAALTERACAO
                                         WHERE PRESTADORVIGENCIAID = @PRESTADORVIGENCIAID ";

                contextQuery.Parameters.Add("@PRESTADORVIGENCIAID", SqlDbType.Int, prestadorVigencia.PrestadorVigenciaId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, prestadorVigencia.DataInicio.Date);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, prestadorVigencia.DataFim.Date);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, prestadorVigencia.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int prestadorVigenciaId, string usuario, string censo)
        {
            List<string> mensagens = new List<string>();
            Pagamento rnPagamento = new Pagamento();
            RN.UsuarioUnidadeFis rnUsuarioUnidadeFis = new UsuarioUnidadeFis();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (prestadorVigenciaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se usuario tem permissão naquela escola
                    if (!rnUsuarioUnidadeFis.PossuiPermissaoPor(contexto, usuario, censo))
                    {
                        mensagens.Add("Você não possui permissão para alterações nesta escola.");
                    }

                    //Verifica se teve pagamento no periodo
                    if (rnPagamento.PossuiPagamentoPeriodoPor(contexto, prestadorVigenciaId))
                    {
                        mensagens.Add("Está vigência não pode ser excluida pois possui pagamentos neste invervalo.");
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

        public void Remove(int prestadorVigenciaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Transporte.PRESTADORVIGENCIA
                            WHERE  PRESTADORVIGENCIAID = @PRESTADORVIGENCIAID  ";

                contextQuery.Parameters.Add("@PRESTADORVIGENCIAID", SqlDbType.Int, prestadorVigenciaId);

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
