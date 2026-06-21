using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Ocorrencias
{
    public class OcorrenciaTratamento
    {
        public bool PossuiTratamentoPor(DataContext contexto, int tratamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Ocorrencias.OCORRENCIATRATAMENTO (NOLOCK)
                                    WHERE TRATAMENTOID = @TRATAMENTOID ";

            contextQuery.Parameters.Add("@TRATAMENTOID", SqlDbType.Int, tratamentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ObtemListaPor(int ocorrenciaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            DataTable dt = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT OCORRENCIATRATAMENTOID, OT.TRATAMENTOID, T.DESCRICAO
                                        FROM Ocorrencias.OCORRENCIATRATAMENTO OT
											INNER JOIN Ocorrencias.TRATAMENTO T ON T.TRATAMENTOID = OT.TRATAMENTOID
                                        WHERE OCORRENCIAID = @OCORRENCIAID ";

                contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);

                dt = contexto.GetDataTable(contextQuery);

                return dt;
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

        public ValidacaoDados Valida(int ocorrenciaId, int tratamentoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ocorrenciaId <= 0)
            {
                mensagens.Add("Campo OCORRENCIA é obrigatório.");
            }

            if (tratamentoId <= 0)
            {
                mensagens.Add("Campo TRATAMENTO é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();                   

                    //Verifica se já existe a ordem cadastrada
                    if (this.PossuiOcorrenciaTratamentoCadastradaPor(contexto, ocorrenciaId, tratamentoId))
                    {
                        mensagens.Add("Este tratamento já foi adicionado para esta ocorrência.");
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

        private bool PossuiOcorrenciaTratamentoCadastradaPor(DataContext ctx, int ocorrenciaId, int tratamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM Ocorrencias.OCORRENCIATRATAMENTO (NOLOCK)
                                WHERE OCORRENCIAID = @OCORRENCIAID
	                                AND TRATAMENTOID = @TRATAMENTOID ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);
            contextQuery.Parameters.Add("@TRATAMENTOID", SqlDbType.Int, tratamentoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(int ocorrenciaId, int tratamentoId, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(ctx, ocorrenciaId, tratamentoId, usuarioId);
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

        public void Insere(DataContext contexto, int ocorrenciaId, int tratamentoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Ocorrencias.OCORRENCIATRATAMENTO
                                               (OCORRENCIAID
                                               ,TRATAMENTOID
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@OCORRENCIAID, 
                                               @TRATAMENTOID, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@OCORRENCIAID", SqlDbType.Int, ocorrenciaId);
            contextQuery.Parameters.Add("@TRATAMENTOID", SqlDbType.Int, tratamentoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(int ocorrenciaTratamentoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE Ocorrencias.OCORRENCIATRATAMENTO
                            WHERE  OCORRENCIATRATAMENTOID = @OCORRENCIATRATAMENTOID
                                    ";

                contextQuery.Parameters.Add("@OCORRENCIATRATAMENTOID", SqlDbType.Int, ocorrenciaTratamentoId);

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
