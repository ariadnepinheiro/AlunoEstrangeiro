using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class FornecedorAnalise
    {
        public DataTable ListaPor(int fornecedorId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" 
                SELECT  A.FORNECEDORANALISEID,
		                A.DATACADASTRO AS DATAANALISE,
		                A.FORNECEDORID,
		                CASE 
			                WHEN A.APROVADA = 1 THEN 'Aprovado'
			                ELSE 'Reprovado'
		                END SITUACAO,
		                (
			                select STRING_AGG(DESCRICAO, char(13) + char(10)) as DESCRICAO 
			                from PrestacaoContas.MOTIVOREPROVACAOFORNECEDOR mrf (nolock)
			                inner join PrestacaoContas.FORNECEDORANALISE__MOTIVOREPROVACAOFORNECEDOR famrf (nolock) on famrf.MOTIVOREPROVACAOFORNECEDORID = mrf.MOTIVOREPROVACAOFORNECEDORID
			                where famrf.FORNECEDORANALISEID = a.FORNECEDORANALISEID
		                ) as MOTIVOREPROVACAO
                FROM PrestacaoContas.FORNECEDORANALISE a (nolock)
                WHERE A.FORNECEDORID = @FORNECEDORID
                ORDER BY a.DATACADASTRO desc
                ";

                contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorId);

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

        public bool PossuiMotivoReprovacaoFornecedorPor(DataContext contexto, int motivoReprovacaoFornecedorId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PrestacaoContas.FORNECEDORANALISE (NOLOCK)
                                    WHERE MOTIVOREPROVACAOFORNECEDORID = @MOTIVOREPROVACAOFORNECEDORID ";

            contextQuery.Parameters.Add("@MOTIVOREPROVACAOFORNECEDORID", SqlDbType.Int, motivoReprovacaoFornecedorId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaAnalise(Entidades.FornecedorAnalise fornecedorAnalise)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();
            Fornecedor rnFornecedor = new Fornecedor();

            if (fornecedorAnalise.FornecedorId <= 0)
            {
                mensagens.Add("Campo FORNECEDOR é obrigatório.");
            }

            if (!fornecedorAnalise.Aprovada)
            {
                if (fornecedorAnalise.MotivoReprovacaoFornecedorId == null || fornecedorAnalise.MotivoReprovacaoFornecedorId < 0)
                {
                    mensagens.Add("Campo MOTIVO é obrigatório quando o fornecedor for reprovado.");
                }
            }

            if (fornecedorAnalise.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se foi enviado para analise
                    if (!rnFornecedor.EhEnviadoAnalisePor(contexto, fornecedorAnalise.FornecedorId))
                    {
                        mensagens.Add("Este fornecedor ainda nao foi enviado para análise.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public ValidacaoDados ValidaAnalise(Entidades.FornecedorAnalise fornecedorAnalise, int[] motivosReprovacaoFornecedorId)
        {
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();
            Fornecedor rnFornecedor = new Fornecedor();

            if (fornecedorAnalise.FornecedorId <= 0)
            {
                mensagens.Add("Campo FORNECEDOR é obrigatório.");
            }

            if (!fornecedorAnalise.Aprovada)
            {
                if (motivosReprovacaoFornecedorId == null || !motivosReprovacaoFornecedorId.Any())
                {
                    mensagens.Add("Campo MOTIVO é obrigatório quando o fornecedor for reprovado.");
                }

                if (motivosReprovacaoFornecedorId.Any(q => q <= 0))
                {
                    mensagens.Add("Há MOTIVO inválido selecionado.");
                }
            }

            if (fornecedorAnalise.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se foi enviado para analise
                    if (!rnFornecedor.EhEnviadoAnalisePor(contexto, fornecedorAnalise.FornecedorId))
                    {
                        mensagens.Add("Este fornecedor ainda nao foi enviado para análise.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Analisa(Entidades.FornecedorAnalise fornecedorAnalise)
        {
            DataContext contexto = null;
            Fornecedor rnFornecedor = new Fornecedor();

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                //Insere Analise
                this.Insere(contexto, fornecedorAnalise);

                //Atualiza situação fornecedor
                rnFornecedor.AtualizaSituacao(contexto, fornecedorAnalise.FornecedorId, fornecedorAnalise.Aprovada, fornecedorAnalise.UsuarioId);
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

        public void Analisa(Entidades.FornecedorAnalise fornecedorAnalise, int[] motivosReprovacaoFornecedorId)
        {
            DataContext contexto = null;
            Fornecedor rnFornecedor = new Fornecedor();

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                //Insere Analise
                this.InsereERetornaId(contexto, fornecedorAnalise);

                //Em caso de reprovação, insere os motivos de reprovação
                if (!fornecedorAnalise.Aprovada)
                    this.InsereMotivoReprovacao(contexto, fornecedorAnalise.FornecedorAnaliseId, motivosReprovacaoFornecedorId, fornecedorAnalise.UsuarioId);

                //Atualiza situação fornecedor
                rnFornecedor.AtualizaSituacao(contexto, fornecedorAnalise.FornecedorId, fornecedorAnalise.Aprovada, fornecedorAnalise.UsuarioId);
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

        private void Insere(DataContext contexto, Entidades.FornecedorAnalise fornecedorAnalise)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" 
            INSERT INTO PrestacaoContas.FORNECEDORANALISE
                                           (FORNECEDORID
                                           ,MOTIVOREPROVACAOFORNECEDORID
                                           ,APROVADA
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@FORNECEDORID, 
                                           @MOTIVOREPROVACAOFORNECEDORID,
                                           @APROVADA, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO ) 
            ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorAnalise.FornecedorId);
            contextQuery.Parameters.Add("@MOTIVOREPROVACAOFORNECEDORID", SqlDbType.Int, fornecedorAnalise.MotivoReprovacaoFornecedorId == null ? (object)DBNull.Value : fornecedorAnalise.MotivoReprovacaoFornecedorId);
            contextQuery.Parameters.Add("@APROVADA", SqlDbType.Bit, fornecedorAnalise.Aprovada);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorAnalise.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereERetornaId(DataContext contexto, Entidades.FornecedorAnalise fornecedorAnalise)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" 
            INSERT INTO PrestacaoContas.FORNECEDORANALISE
                                           (FORNECEDORID
                                           ,MOTIVOREPROVACAOFORNECEDORID
                                           ,APROVADA
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
            VALUES
                                           (@FORNECEDORID, 
                                           @MOTIVOREPROVACAOFORNECEDORID,
                                           @APROVADA, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO )

            select IDENT_CURRENT('PrestacaoContas.FORNECEDORANALISE')
            ";

            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, fornecedorAnalise.FornecedorId);
            contextQuery.Parameters.Add("@MOTIVOREPROVACAOFORNECEDORID", SqlDbType.Int, fornecedorAnalise.MotivoReprovacaoFornecedorId == null ? (object)DBNull.Value : fornecedorAnalise.MotivoReprovacaoFornecedorId);
            contextQuery.Parameters.Add("@APROVADA", SqlDbType.Bit, fornecedorAnalise.Aprovada);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, fornecedorAnalise.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            fornecedorAnalise.FornecedorAnaliseId = contexto.GetReturnValue<int>(contextQuery);
        }

        private void InsereMotivoReprovacao(DataContext contexto, int fornecedorAnaliseId, int[] motivosReprovacaoFornecedorId, string usuarioId)
        {
            var insereUmMotivo = new Action<int>(mrfid => 
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT INTO PrestacaoContas.FORNECEDORANALISE__MOTIVOREPROVACAOFORNECEDOR
                                           (FORNECEDORANALISEID
                                           ,MOTIVOREPROVACAOFORNECEDORID
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@FORNECEDORANALISEID, 
                                           @MOTIVOREPROVACAOFORNECEDORID, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO ) ";

                contextQuery.Parameters.Add("@FORNECEDORANALISEID", SqlDbType.Int, fornecedorAnaliseId);
                contextQuery.Parameters.Add("@MOTIVOREPROVACAOFORNECEDORID", SqlDbType.Int, mrfid);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
            });

            foreach (var id in motivosReprovacaoFornecedorId)
                insereUmMotivo(id);
        }
    }
}