using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class GoogleEducation
    {
        public Entidades.GoogleEducation ObtemPor(string aluno)
        {	
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                        FROM RecursosHumanos.GOOGLEEDUCATION (NOLOCK)
                                        WHERE ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                googleEducation = contexto.TryToBindEntity<Entidades.GoogleEducation>(contextQuery);

                return googleEducation;
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

        public Entidades.GoogleEducation ObtemPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                        FROM RecursosHumanos.GOOGLEEDUCATION (NOLOCK)
                                        WHERE PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

                googleEducation = contexto.TryToBindEntity<Entidades.GoogleEducation>(contextQuery);

                return googleEducation;
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

        public bool PossuiEmailGooglePor(DataContext ctx, decimal pessoa, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT   COUNT(*) 
                                            FROM   RECURSOSHUMANOS.GOOGLEEDUCATION ";

            if (!aluno.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Command += " WHERE  ALUNO = @ALUNO  ";
                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            }
            else
            {
                contextQuery.Command += " WHERE  PESSOA = @PESSOA  ";
                contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            }

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutroEmailPor(DataContext ctx, string email, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM RECURSOSHUMANOS.GOOGLEEDUCATION (NOLOCK)
                                WHERE EMAIL = @EMAIL
	                                AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@EMAIL", SqlDbType.VarChar, email);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string RetornaEmailPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 EMAIL 
                            FROM RECURSOSHUMANOS.GOOGLEEDUCATION (NOLOCK)
                            WHERE PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void Salva(DataContext ctx, RecursosHumanos.Entidades.GoogleEducation googleEducation)
        {
            bool existe = this.PossuiEmailGooglePor(ctx, googleEducation.Pessoa, googleEducation.Aluno);

            if (existe)
            {
                this.Atualiza(ctx, googleEducation);
            }
            else
            {
                if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                {
                    this.Insere(ctx, googleEducation);
                }
            }
        }

        public void Insere(DataContext ctx, RecursosHumanos.Entidades.GoogleEducation googleEducation)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO RECURSOSHUMANOS.GOOGLEEDUCATION 
                                        (PESSOA, 
                                         ALUNO, 
                                         EMAIL, 
                                         USUARIOID, 
                                         DATAALTERACAO,                                      
                                         DATACADASTRO) 
                                    VALUES      
	                                    (@PESSOA, 
                                        @ALUNO, 
                                        @EMAIL,   
                                        @USUARIOID, 
                                        @DATAALTERACAO,                                    
                                        @DATACADASTRO)  ";

                contextQuery.Parameters.Add("@PESSOA", googleEducation.Pessoa);
                contextQuery.Parameters.Add("@ALUNO", googleEducation.Aluno);
                contextQuery.Parameters.Add("@EMAIL", googleEducation.Email);
                contextQuery.Parameters.Add("@USUARIOID", googleEducation.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public void Atualiza(DataContext ctx, RecursosHumanos.Entidades.GoogleEducation googleEducation)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE RECURSOSHUMANOS.GOOGLEEDUCATION 
                                            SET   
                                                   EMAIL = @EMAIL,
                                                   USUARIOID = @USUARIOID,
                                                   DATAALTERACAO = @DATAALTERACAO
                                           ";

                if (!googleEducation.Aluno.IsNullOrEmptyOrWhiteSpace())
                {
                    contextQuery.Command += " WHERE  ALUNO = @ALUNO  ";
                    contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, googleEducation.Aluno);
                }
                else
                {
                    contextQuery.Command += " WHERE  PESSOA = @PESSOA  ";
                    contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, googleEducation.Pessoa);
                }


                contextQuery.Parameters.Add("@EMAIL", googleEducation.Email);
                contextQuery.Parameters.Add("@USUARIOID", googleEducation.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

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

        public void RemoveEmailFuncinario(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE RECURSOSHUMANOS.GOOGLEEDUCATION
                            WHERE PESSOA = @PESSOA
                                   AND ALUNO IS NULL ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemoveEmailAluno(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE RECURSOSHUMANOS.GOOGLEEDUCATION
                            WHERE PESSOA = @PESSOA
                                   AND ALUNO IS NOT NULL ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
 
            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ObtemAcessosPor(string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.GoogleEducation googleEducation = new Techne.Lyceum.RN.RecursosHumanos.Entidades.GoogleEducation();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command =
                                        @" 
                                             SELECT DISTINCT CONVERT(DATE,LOGINTIME) LOGINTIME
                                             FROM [GSUITE].[GSUITE].[GOOGLE_DIRECTORY_USERSLOGIN] UL
                                                INNER JOIN [GSUITE].[GSUITE].[GOOGLE_DIRECTORY_USERS] U
                                                                            ON U.id = UL.id
                                            WHERE   Year(LOGINTIME) = Year(Getdate())
                                                    AND U.[customSchemas_Aluno_Matricula] = @ALUNO
                                                    AND MONTH(LOGINTIME) >= 3
                                            ORDER BY LOGINTIME 
                                        ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                return contexto.GetDataTable(contextQuery);
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
    }
}
