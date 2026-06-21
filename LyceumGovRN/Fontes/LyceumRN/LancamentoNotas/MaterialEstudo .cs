using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;

namespace Techne.Lyceum.RN.LancamentoNotas
{
    public class MaterialEstudo : RNBase
    {
        public DataTable ListaAtivoPor()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"   
                                            SELECT  MATERIALESTUDOID, 
		                                            DESCRICAO
                                            FROM LancamentoNotas.MATERIALESTUDO (NOLOCK)
                                                 WHERE ATIVO = 1
                                            ORDER BY DESCRICAO  ";

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

        public DataTable Lista()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  MATERIALESTUDOID, 
		                                        DESCRICAO, 		                                     
		                                        ATIVO, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM LancamentoNotas.MATERIALESTUDO (NOLOCK)
                                        ORDER BY DESCRICAO ";

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

        public ValidacaoDados Valida(Entidades.MaterialEstudo materialEstudo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (materialEstudo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (materialEstudo.MaterialEstudoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (materialEstudo.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO é obrigatório.");
            }

            if (materialEstudo.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    // Verifica se já existe a descrição cadastrada
                    if (this.PossuiOutraDescricaoCadastradaPor(contexto, materialEstudo.Descricao, materialEstudo.MaterialEstudoId))
                    {
                        mensagens.Add("Esta DESCRIÇÃO já foi utilizada.");
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

        private bool PossuiOutraDescricaoCadastradaPor(DataContext ctx, string descricao, int MaterialEstudoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM LancamentoNotas.MATERIALESTUDO (NOLOCK)
                                WHERE DESCRICAO = @DESCRICAO
	                                AND MATERIALESTUDOID <> @MATERIALESTUDOID ";

            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, descricao);
            contextQuery.Parameters.Add("@MATERIALESTUDOID", SqlDbType.Int, MaterialEstudoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.MaterialEstudo MaterialEstudo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO LancamentoNotas.MATERIALESTUDO
                                                        (DESCRICAO, 
                                                         ATIVO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (@DESCRICAO, 
                                                         @ATIVO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, MaterialEstudo.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, MaterialEstudo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, MaterialEstudo.UsuarioId);
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

        public void Atualiza(Entidades.MaterialEstudo MaterialEstudo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LancamentoNotas.MATERIALESTUDO
                                        SET    DESCRICAO = @DESCRICAO, 
                                               ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  MATERIALESTUDOID = @MATERIALESTUDOID ";

                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, MaterialEstudo.Descricao);
                contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, MaterialEstudo.Ativo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, MaterialEstudo.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@MATERIALESTUDOID", SqlDbType.Int, MaterialEstudo.MaterialEstudoId);

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

        public ValidacaoDados ValidaRemocao(int materialEstudoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.LancamentoNotas.TurmaMaterialEstudo rnTurmaMaterialEstudo = new TurmaMaterialEstudo();
            //RN.Matriculas.OpcaoInscricaoHist rnOpcaoInscricaoHist = new OpcaoInscricaoHist();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (materialEstudoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica 
                    if (rnTurmaMaterialEstudo.PossuiMaterialEstudoPor(contexto, materialEstudoId))
                    {
                        mensagens.Add("Este material não pode ser excluído.");
                    }

                    ////Verifica se motivo ja foi utilizado em algum contato
                    //if (rnOpcaoInscricaoHist.PossuiMotivoRetornoPor(contexto, motivoRetornoId))
                    //{
                    //    mensagens.Add("Este motivo não pode ser excluído, pois já foi utilizado para um contato.");
                    //}
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

        public void Remove(int materialEstudoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE LancamentoNotas.MATERIALESTUDO
                            WHERE  MATERIALESTUDOID = @MATERIALESTUDOID  ";

                contextQuery.Parameters.Add("@MATERIALESTUDOID", SqlDbType.Int, materialEstudoId);

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
