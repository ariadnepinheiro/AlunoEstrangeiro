using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.ContratoTemporario;

namespace Techne.Lyceum.RN
{
    public class LiberacaoCandidatoDocente
    {
        public void InserirLiberacaoCandidato(DataContext ctx, string concurso, string candidato, string nome, string cpf, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO DBO.TCE_LIBERACAO_CANDIDATO_DOCENTE 
                                                    (CONCURSO, 
                                                     CANDIDATO, 
                                                     NOME, 
                                                     CPF, 
                                                     MATRICULA) 
                                        VALUES      (@CONCURSO, 
                                                     @CANDIDATO, 
                                                     @NOME, 
                                                     @CPF, 
                                                     @MATRICULA)  ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);
                contextQuery.Parameters.Add("@NOME", nome);
                contextQuery.Parameters.Add("@CPF", cpf);
                contextQuery.Parameters.Add("@MATRICULA", usuario);

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
        }

        public ValidacaoDados Valida(string concurso, string candidato)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            CandidatoDocContrato rnCandidatoDocContrato = new CandidatoDocContrato();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (string.IsNullOrEmpty(candidato))
            {
                mensagens.Add("Favor selecionar um candidato.");
            }

            if (string.IsNullOrEmpty(concurso))
            {
                mensagens.Add("Favor selecionar um concurso.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a candidato já foi liberado anteriormente
                    if (this.ExisteLiberacaoPor(contexto, concurso, candidato))
                    {
                        mensagens.Add("Este candidado já foi liberado.");
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

        private bool ExisteLiberacaoPor(DataContext ctx, string concurso, string candidato)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   TCE_LIBERACAO_CANDIDATO_DOCENTE (NOLOCK) 
                                        WHERE  CONCURSO = @CONCURSO 
                                               AND CANDIDATO = @CANDIDATO ";

                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@CANDIDATO", candidato);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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
    }
}