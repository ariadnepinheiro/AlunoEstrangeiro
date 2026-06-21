using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class PeriodoConfirmacao
    {
        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  PERIODOCONFIRMACAOID, 
		                                        ANO, 	
                                                PERIODO,
                                                DATAINICIO,
                                                DATAFIM,
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM Pedagogico.PERIODOCONFIRMACAO (NOLOCK)
                                        ORDER BY ANO DESC, PERIODO DESC ";

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

        public ValidacaoDados Valida(Entidades.PeriodoConfirmacao periodoConfirmacao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PeriodoConfirmacaoCurso rnPeriodoConfirmacaoCurso = new PeriodoConfirmacaoCurso();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoConfirmacao == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (periodoConfirmacao.PeriodoConfirmacaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (periodoConfirmacao.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodoConfirmacao.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (periodoConfirmacao.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }

            if (periodoConfirmacao.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }
            else if (periodoConfirmacao.DataInicio != DateTime.MinValue)
            {
                if (periodoConfirmacao.DataInicio > periodoConfirmacao.DataFim)
                {
                    mensagens.Add("A DATA INÍCIO não pode ser inferior a DATA FIM.");
                }
            }

            if (periodoConfirmacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já existe o ano / periodo cadastrado
                    if (this.PossuiOutroAnoPeriodoCadastradoPor(contexto, periodoConfirmacao.Ano, periodoConfirmacao.Periodo, periodoConfirmacao.PeriodoConfirmacaoId))
                    {
                        mensagens.Add("Este ANO/PERIODO já foi cadastrado.");
                    }

                    //verifica se é alteração
                    if (!cadastro)
                    {
                        //Verifica se já tem curso cadastrado
                        if (rnPeriodoConfirmacaoCurso.PossuiPeriodoConfirmacaoPor(contexto, periodoConfirmacao.PeriodoConfirmacaoId))
                        {
                            //Busca data Inicio
                            var periodoBase = this.ObtemPor(contexto, periodoConfirmacao.PeriodoConfirmacaoId);

                            //Verifica se está trocando data inicio
                            if (periodoBase.DataInicio != periodoBase.DataInicio)
                            {
                                mensagens.Add("A DATA INÍCIO não pode ser alterada pois este ANO/PERIODO já tem cursos cadastrados.");
                            }
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

        public bool ObtemPeriodoAberto()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.PeriodoConfirmacao periodoConfirmacao = new Entidades.PeriodoConfirmacao();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                          FROM Pedagogico.PERIODOCONFIRMACAO
                                          WHERE GETDATE() BETWEEN DATAINICIO AND DATAFIM ";

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public Entidades.PeriodoConfirmacao ObtemPor(DataContext contexto, int ano, int periodo)
        {
            Entidades.PeriodoConfirmacao periodoConfirmacao = new Entidades.PeriodoConfirmacao();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                        FROM Pedagogico.PERIODOCONFIRMACAO
                                        WHERE ANO = @ANO
	                                        AND PERIODO = @PERIODO ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

            periodoConfirmacao = contexto.TryToBindEntity<Entidades.PeriodoConfirmacao>(contextQuery);

            return periodoConfirmacao;
        }

        private Entidades.PeriodoConfirmacao ObtemPor(DataContext contexto, int periodoConfirmacaoId)
        {
            Entidades.PeriodoConfirmacao periodoConfirmacao = new Techne.Lyceum.RN.Pedagogico.Entidades.PeriodoConfirmacao();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                FROM Pedagogico.PERIODOCONFIRMACAOCURSO
                                WHERE PERIODOCONFIRMACAOID = @PERIODOCONFIRMACAOID ";

            contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacaoId);

            periodoConfirmacao = contexto.TryToBindEntity<Entidades.PeriodoConfirmacao>(contextQuery);

            return periodoConfirmacao;
        }

        private bool PossuiOutroAnoPeriodoCadastradoPor(DataContext ctx, int ano, int periodo, int periodoConfirmacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM Pedagogico.PERIODOCONFIRMACAO (NOLOCK)
                                WHERE ANO = @ANO
                                    AND PERIODO = @PERIODO
	                                AND PERIODOCONFIRMACAOID <> @PERIODOCONFIRMACAOID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacaoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.PeriodoConfirmacao periodoConfirmacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Pedagogico.PERIODOCONFIRMACAO
                                                        (ANO, 
                                                         PERIODO,
                                                         DATAINICIO,
                                                         DATAFIM,
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@ANO, 
                                                         @PERIODO,
                                                         @DATAINICIO,
                                                         @DATAFIM,
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, periodoConfirmacao.Ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodoConfirmacao.Periodo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoConfirmacao.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoConfirmacao.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoConfirmacao.UsuarioId);
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

        public void Atualiza(Entidades.PeriodoConfirmacao periodoConfirmacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Pedagogico.PERIODOCONFIRMACAO
                                        SET    DATAINICIO = @DATAINICIO,
                                               DATAFIM = @DATAFIM,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PERIODOCONFIRMACAOID = @PERIODOCONFIRMACAOID ";

                contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacao.PeriodoConfirmacaoId);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, periodoConfirmacao.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, periodoConfirmacao.DataFim);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoConfirmacao.UsuarioId);
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

        public ValidacaoDados ValidaRemocao(int periodoConfirmacaoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PeriodoConfirmacaoCurso rnPeriodoConfirmacaoCurso = new PeriodoConfirmacaoCurso();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoConfirmacaoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se motivo ja foi utilizado
                    if (rnPeriodoConfirmacaoCurso.PossuiPeriodoConfirmacaoPor(contexto, periodoConfirmacaoId))
                    {
                        mensagens.Add("Este período não pode ser excluído, pois tem cursos cadastrados.");
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

        public void Remove(int periodoConfirmacaoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Pedagogico.PERIODOCONFIRMACAO
                            WHERE  PERIODOCONFIRMACAOID = @PERIODOCONFIRMACAOID  ";

                contextQuery.Parameters.Add("@PERIODOCONFIRMACAOID", SqlDbType.Int, periodoConfirmacaoId);

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
