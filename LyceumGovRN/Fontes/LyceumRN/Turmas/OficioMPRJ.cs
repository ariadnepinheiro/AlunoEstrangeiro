using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Turmas
{
    public class OficioMPRJ
    {
        public Entidades.OficioMPRJ ObtemPor(int notificacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.OficioMPRJ oficio = new Entidades.OficioMPRJ();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                        FROM Turma.OFICIOMPRJ
                                        WHERE NOTIFICACAOID = @NOTIFICACAOID ";

                contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);

                oficio = contexto.TryToBindEntity<Entidades.OficioMPRJ>(contextQuery);

                return oficio;
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

        public ValidacaoDados Valida(Entidades.OficioMPRJ oficioMPRJ, string aluno, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            RN.Aluno rnAluno = new Aluno();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (oficioMPRJ == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (oficioMPRJ.OficioMPRJId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (oficioMPRJ.NotificacaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA NOTIFICAÇÃO é obrigatório.");
            }

            if (oficioMPRJ.Promotoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PROMOTORIA é obrigatório.");
            }

            if (oficioMPRJ.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório.");
            }

            if (oficioMPRJ.Municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICÍPIO é obrigatório.");
            }

            if (oficioMPRJ.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório.");
            }

            if (oficioMPRJ.Numero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO é obrigatório.");
            }

            if (oficioMPRJ.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }

            if (oficioMPRJ.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi cadastrada oficioMPRJ para aquela notificação
                    if (this.PossuiOutroOficioPor(contexto, oficioMPRJ.NotificacaoId, oficioMPRJ.OficioMPRJId))
                    {
                        mensagens.Add("Já foi cadastrada uma notificação para este ALUNO nesta DATA DE COMUNICAÇÃO.");
                    }

                    //Verifica se o aluno está como matricula em suspensao 
                    if (!rnAluno.EhAlunoMatriculaEmSuspensaoPor(contexto, aluno))
                    {
                        mensagens.Add("Este aluno não está com a situação Matrícula em Suspensão.");
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

        private bool PossuiOutroOficioPor(DataContext ctx, int notificacaoId, int oficioMPRJId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Turma.OFICIOMPRJ (NOLOCK)
                                    WHERE NOTIFICACAOID = @NOTIFICACAOID
										AND OFICIOMPRJID <> @OFICIOMPRJID ";

            contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);
            contextQuery.Parameters.Add("@OFICIOMPRJID", SqlDbType.Int, oficioMPRJId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.OficioMPRJ oficioMPRJ)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Turma.OFICIOMPRJ
                                               (NOTIFICACAOID
                                               ,PROMOTORIA
                                               ,CEP
                                               ,MUNICIPIO
                                               ,ENDERECO
                                               ,NUMERO
                                               ,BAIRRO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@NOTIFICACAOID, 
                                               @PROMOTORIA, 
                                               @CEP, 
                                               @MUNICIPIO, 
                                               @ENDERECO, 
                                               @NUMERO, 
                                               @BAIRRO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO) 
                                    
                                        SELECT IDENT_CURRENT('Turma.OFICIOMPRJ') ";


                contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, oficioMPRJ.NotificacaoId);
                contextQuery.Parameters.Add("@PROMOTORIA", SqlDbType.VarChar, oficioMPRJ.Promotoria);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, oficioMPRJ.Cep);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, oficioMPRJ.Municipio);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, oficioMPRJ.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, oficioMPRJ.Numero);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, oficioMPRJ.Bairro);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, oficioMPRJ.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                oficioMPRJ.OficioMPRJId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));

                
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

        public void Atualiza(Entidades.OficioMPRJ oficioMPRJ)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Turma.OFICIOMPRJ
                                           SET PROMOTORIA = @PROMOTORIA, 
                                              CEP = @CEP, 
                                              MUNICIPIO = @MUNICIPIO, 
                                              ENDERECO = @ENDERECO, 
                                              NUMERO = @NUMERO, 
                                              BAIRRO = @BAIRRO, 
                                              USUARIOID = @USUARIOID, 
                                              DATAALTERACAO = @DATAALTERACAO
                                        WHERE OFICIOMPRJID = @OFICIOMPRJID ";


                contextQuery.Parameters.Add("@OFICIOMPRJID", SqlDbType.Int, oficioMPRJ.OficioMPRJId);
                contextQuery.Parameters.Add("@PROMOTORIA", SqlDbType.VarChar, oficioMPRJ.Promotoria);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, oficioMPRJ.Cep);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, oficioMPRJ.Municipio);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, oficioMPRJ.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, oficioMPRJ.Numero);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, oficioMPRJ.Bairro);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, oficioMPRJ.UsuarioId);               
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
    }
}
