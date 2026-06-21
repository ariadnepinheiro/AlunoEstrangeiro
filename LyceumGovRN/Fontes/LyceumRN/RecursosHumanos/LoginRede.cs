using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class LoginRede
    {
        public DTOs.DadosLoginRedeEmail ObtemDadosLoginRedeEmailPor(decimal pessoa)
        {	
            DTOs.DadosLoginRedeEmail dados = new DTOs.DadosLoginRedeEmail();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT P.PESSOA, 
                                       CPF, 
                                       NOME_COMPL, 
                                       IDFUNCIONAL, 
                                       P.E_MAIL_INTERNO, 
									   p.E_MAIL,
                                       G.EMAIL AS EMAILGOOGLE, 
                                       LR.LOGINREDEID, 
                                       LR.LOGINREDE 
                                FROM   LY_PESSOA P 
                                       LEFT JOIN RECURSOSHUMANOS.GOOGLEEDUCATION G 
                                              ON P.PESSOA = G.PESSOA 
                                       LEFT JOIN RECURSOSHUMANOS.LOGINREDE LR 
                                              ON P.PESSOA = LR.PESSOA 
                                WHERE  P.PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    dados.LoginRedeId = reader["LOGINREDEID"] != DBNull.Value ? Convert.ToInt32(reader["LOGINREDEID"]) : 0;
                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    dados.IdFuncional = Convert.ToString(reader["IDFUNCIONAL"]);
                    dados.EmailOffice365 = Convert.ToString(reader["E_MAIL_INTERNO"]);
                    dados.EmailGoogleEducation = Convert.ToString(reader["EMAILGOOGLE"]);
                    dados.EmailAlternativo = Convert.ToString(reader["E_MAIL"]);
                    dados.LoginRede = Convert.ToString(reader["LOGINREDE"]);
                }

                return dados;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public ValidacaoDados ValidaDadosLoginRedeEmail(DTOs.DadosLoginRedeEmail dadosLoginRedeEmail)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            GoogleEducation rnGoogleEducation = new GoogleEducation();
            RN.Pessoa rnPessoa = new Pessoa();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosLoginRedeEmail == null)
            {
                return validacaoDados;
            }

            if (dadosLoginRedeEmail.Pessoa <= 0)
            {
                mensagens.Add("O PESSOA é obrigatório.");
            }

            if (dadosLoginRedeEmail.EmailOffice365.IsNullOrEmptyOrWhiteSpace()
                && dadosLoginRedeEmail.EmailGoogleEducation.IsNullOrEmptyOrWhiteSpace()
                && dadosLoginRedeEmail.EmailAlternativo.IsNullOrEmptyOrWhiteSpace()
                && dadosLoginRedeEmail.LoginRede.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("É obrigatório informar ao menos 1 dos campos.");
            }

            if (!dadosLoginRedeEmail.EmailOffice365.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosLoginRedeEmail.EmailOffice365.Split('@')[1].Trim() != "prof.educacao.rj.gov.br"
                    && dadosLoginRedeEmail.EmailOffice365.Split('@')[1].Trim() != "educacao.rj.gov.br")
                {
                    mensagens.Add("No campo E-MAIL OFFICE 365 serão aceitos apenas e-mails institucionais @educacao.rj.gov.br ou @prof.educacao.rj.gov.br");
                }
            }

            if (!dadosLoginRedeEmail.EmailGoogleEducation.IsNullOrEmptyOrWhiteSpace())
            {
                if ((dadosLoginRedeEmail.EmailGoogleEducation.Split('@')[1].Trim() != "prof.educa.rj.gov.br"
                      && dadosLoginRedeEmail.EmailGoogleEducation.Split('@')[1].Trim() != "educa.rj.gov.br"))
                {
                    mensagens.Add("No campo E-MAIL GOOGLE FOR EDUCATION serão aceitos apenas e-mails @educa.rj.gov.br ou @prof.educa.rj.gov.br");
                }
            }

            if (!dadosLoginRedeEmail.EmailAlternativo.IsNullOrEmptyOrWhiteSpace())
            {
                if (!RN.Validacao.ValidaEmail(dadosLoginRedeEmail.EmailAlternativo))
                {
                    mensagens.Add("E-MAIL ALTERNATIVO inválido");
                }
            }

            if (dadosLoginRedeEmail.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O USUARIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se EmailOffice365 é utilizada por outra pessoa
                    if (this.PossuiOutroLoginPor(contexto, dadosLoginRedeEmail.LoginRede, dadosLoginRedeEmail.Pessoa))
                    {
                        mensagens.Add("Este login já foi utilizado por outra pessoa.");
                    }

                    //Verifica se EmailGoogleEducation é utilizada por outra pessoa
                    if (rnGoogleEducation.PossuiOutroEmailPor(contexto, dadosLoginRedeEmail.EmailGoogleEducation, dadosLoginRedeEmail.Pessoa))
                    {
                        mensagens.Add("Este E-MAIL GOOGLE FOR EDUCATION já foi utilizado por outra pessoa.");
                    }

                    //Verifica se Email Internto é utilizado por outra pessoa
                    if (rnPessoa.PossuiOutroEmailInternoPor(contexto, dadosLoginRedeEmail.EmailOffice365, dadosLoginRedeEmail.Pessoa))
                    {
                        mensagens.Add("Este E-MAIL OFFICE 365 já foi utilizado por outra pessoa.");
                    }

                    //Verifica se Email é utilizado por outra pessoa
                    if (rnPessoa.PossuiOutroEmailPor(contexto, dadosLoginRedeEmail.EmailAlternativo, dadosLoginRedeEmail.Pessoa))
                    {
                        mensagens.Add("Este E-MAIL ALTERNATIVO já foi utilizado por outra pessoa.");
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

        private bool PossuiOutroLoginPor(DataContext ctx, string loginRede, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM RECURSOSHUMANOS.LOGINREDE (NOLOCK)
                                WHERE LOGINREDE = @LOGINREDE
	                                AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@LOGINREDE", SqlDbType.VarChar, loginRede);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private string RetornaLoginRedePor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 LOGINREDE 
                            FROM RECURSOSHUMANOS.LOGINREDE (nolock)
                            WHERE PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void SalvaDadosLoginRedeEmail(DTOs.DadosLoginRedeEmail dadosLoginRedeEmail)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            GoogleEducation rnGoogleEducation = new GoogleEducation();
            Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
            RN.Pessoa rnPessoa = new Pessoa();
            string loginRedeAtual = string.Empty;
            string emailGoogleAtual = string.Empty;
            string emailOffice365Atual = string.Empty; 
            string emailAlternativo = string.Empty;

            try
            {
                //Busca Login atual
                loginRedeAtual = this.RetornaLoginRedePor(contexto, dadosLoginRedeEmail.Pessoa);

                //Verifica se tem login
                if (!loginRedeAtual.IsNullOrEmptyOrWhiteSpace())
                {
                    //Verifca se foi retirado
                    if (dadosLoginRedeEmail.LoginRede.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Remove login rede
                        this.Remove(contexto, dadosLoginRedeEmail.Pessoa);
                    }
                    else
                    {
                        //Verifica se foi modificado
                        if (dadosLoginRedeEmail.LoginRede != loginRedeAtual)
                        {
                            //Atualiza login rede
                            this.Atualiza(contexto, dadosLoginRedeEmail);
                        }
                    }
                }
                else if (!dadosLoginRedeEmail.LoginRede.IsNullOrEmptyOrWhiteSpace())
                {
                    //Insere login rede
                    this.Insere(contexto, dadosLoginRedeEmail);
                }

                //Busca email google atual
                emailGoogleAtual = rnGoogleEducation.RetornaEmailPor(contexto, dadosLoginRedeEmail.Pessoa);

                //Verifica se tem email google
                if (!emailGoogleAtual.IsNullOrEmptyOrWhiteSpace())
                {
                    //Verifca se foi retirado
                    if (dadosLoginRedeEmail.EmailGoogleEducation.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Remove email google
                        rnGoogleEducation.RemoveEmailFuncinario(contexto, dadosLoginRedeEmail.Pessoa);
                    }
                    else
                    {
                        //VErifica se foi modificado
                        if (dadosLoginRedeEmail.EmailGoogleEducation != emailGoogleAtual)
                        {
                            //Monta Entidade 
                            googleEducation.Pessoa = dadosLoginRedeEmail.Pessoa;
                            googleEducation.Email = dadosLoginRedeEmail.EmailGoogleEducation;
                            googleEducation.UsuarioId = dadosLoginRedeEmail.UsuarioId;
                                
                            //Atualiza email google
                            rnGoogleEducation.Atualiza(contexto, googleEducation);
                        }
                    }
                }
                else if (!dadosLoginRedeEmail.EmailGoogleEducation.IsNullOrEmptyOrWhiteSpace())
                {
                    //Monta Entidade 
                    googleEducation.Pessoa = dadosLoginRedeEmail.Pessoa;
                    googleEducation.Email = dadosLoginRedeEmail.EmailGoogleEducation;
                    googleEducation.UsuarioId = dadosLoginRedeEmail.UsuarioId;

                    //Insere email google
                    rnGoogleEducation.Insere(contexto, googleEducation);
                }

                //Busca email interno atual
                emailOffice365Atual = rnPessoa.RetornaEmailInternoPor(contexto, dadosLoginRedeEmail.Pessoa);

                //Busca email Alternativo atual
                emailAlternativo = rnPessoa.RetornaEmailPor(contexto, dadosLoginRedeEmail.Pessoa);

                if (dadosLoginRedeEmail.EmailOffice365 != emailOffice365Atual || dadosLoginRedeEmail.EmailAlternativo != emailAlternativo)
                {
                    //Atualiza email interno e alternativo na pessoa
                    rnPessoa.AtualizaEmailInternoAlternativo(contexto, dadosLoginRedeEmail.Pessoa, dadosLoginRedeEmail.EmailOffice365, dadosLoginRedeEmail.EmailAlternativo, dadosLoginRedeEmail.UsuarioId);
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

        private void Insere(DataContext contexto, DTOs.DadosLoginRedeEmail dadosLoginRedeEmail)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.LOGINREDE
                                               (PESSOA
                                               ,LOGINREDE
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@PESSOA
                                               ,@LOGINREDE
                                               ,@USUARIOID
                                               ,@DATACADASTRO
                                               ,@DATAALTERACAO) ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, dadosLoginRedeEmail.Pessoa);
            contextQuery.Parameters.Add("@LOGINREDE", SqlDbType.VarChar, dadosLoginRedeEmail.LoginRede);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosLoginRedeEmail.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void Atualiza(DataContext contexto, DTOs.DadosLoginRedeEmail dadosLoginRedeEmail)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE RecursosHumanos.LOGINREDE
	                                    SET LOGINREDE = @LOGINREDE,
		                                    USUARIOID = @USUARIOID,
		                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, dadosLoginRedeEmail.Pessoa);
            contextQuery.Parameters.Add("@LOGINREDE", SqlDbType.VarChar, dadosLoginRedeEmail.LoginRede);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosLoginRedeEmail.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void Remove(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE RECURSOSHUMANOS.LOGINREDE
                            WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
