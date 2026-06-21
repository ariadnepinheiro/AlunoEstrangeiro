using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Turmas
{
    public class OficioConselho
    {
        public Entidades.OficioConselho ObtemPor(int notificacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.OficioConselho oficio = new Entidades.OficioConselho();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                        FROM Turma.OFICIOCONSELHO
                                        WHERE NOTIFICACAOID = @NOTIFICACAOID ";

                contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);

                oficio = contexto.TryToBindEntity<Entidades.OficioConselho>(contextQuery);

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

        public ValidacaoDados Valida(Entidades.OficioConselho oficioConselho, string aluno, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            RN.Aluno rnAluno = new Aluno();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (oficioConselho == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (oficioConselho.OficioConselhoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (oficioConselho.NotificacaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA NOTIFICAÇÃO é obrigatório.");
            }

            if (oficioConselho.Conselho.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CONSELHO é obrigatório.");
            }

            if (oficioConselho.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório.");
            }

            if (oficioConselho.Municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICÍPIO é obrigatório.");
            }

            if (oficioConselho.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório.");
            }

            if (oficioConselho.Numero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO é obrigatório.");
            }

            if (oficioConselho.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }

            if (oficioConselho.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi cadastrada oficioConselho para aquela notificação
                    if (this.PossuiOutroOficioPor(contexto, oficioConselho.NotificacaoId, oficioConselho.OficioConselhoId))
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

        private bool PossuiOutroOficioPor(DataContext ctx, int notificacaoId, int oficioConselhoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Turma.OFICIOCONSELHO (NOLOCK)
                                    WHERE NOTIFICACAOID = @NOTIFICACAOID
										AND OFICIOCONSELHOID <> @OFICIOCONSELHOID ";

            contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, notificacaoId);
            contextQuery.Parameters.Add("@OFICIOCONSELHOID", SqlDbType.Int, oficioConselhoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.OficioConselho oficioConselho)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO Turma.OFICIOCONSELHO
                                               (NOTIFICACAOID
                                               ,CONSELHO
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
                                               @CONSELHO, 
                                               @CEP, 
                                               @MUNICIPIO, 
                                               @ENDERECO, 
                                               @NUMERO, 
                                               @BAIRRO, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO)
                                            

                                    SELECT IDENT_CURRENT('Turma.OFICIOCONSELHO') ";


                contextQuery.Parameters.Add("@NOTIFICACAOID", SqlDbType.Int, oficioConselho.NotificacaoId);
                contextQuery.Parameters.Add("@CONSELHO", SqlDbType.VarChar, oficioConselho.Conselho);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, oficioConselho.Cep);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, oficioConselho.Municipio);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, oficioConselho.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, oficioConselho.Numero);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, oficioConselho.Bairro);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, oficioConselho.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                oficioConselho.OficioConselhoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.OficioConselho oficioConselho)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE Turma.OFICIOCONSELHO
                                           SET CONSELHO = @CONSELHO, 
                                              CEP = @CEP, 
                                              MUNICIPIO = @MUNICIPIO, 
                                              ENDERECO = @ENDERECO, 
                                              NUMERO = @NUMERO, 
                                              BAIRRO = @BAIRRO, 
                                              USUARIOID = @USUARIOID, 
                                              DATAALTERACAO = @DATAALTERACAO
                                        WHERE OFICIOCONSELHOID = @OFICIOCONSELHOID ";


                contextQuery.Parameters.Add("@OFICIOCONSELHOID", SqlDbType.Int, oficioConselho.OficioConselhoId);
                contextQuery.Parameters.Add("@CONSELHO", SqlDbType.VarChar, oficioConselho.Conselho);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, oficioConselho.Cep);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, oficioConselho.Municipio);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, oficioConselho.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, oficioConselho.Numero);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, oficioConselho.Bairro);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, oficioConselho.UsuarioId);
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
