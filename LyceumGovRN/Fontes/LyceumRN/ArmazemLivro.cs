namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.HadesLyc.CR;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using Techne.Lyceum.RN.DTOs;
    using System.Data.SqlClient;

    public class ArmazemLivro : RNBase
    {
        public class DadosArmazemLivro
        {
            public string Nome { get; set; }
            public string Cpf { get; set; }
            public string Categoria { get; set; }
            public string IdFuncional { get; set; }
            public string CodigoAcesso { get; set; }
        }

        public bool EhDiretor(DataContext ctx, string cpf)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool diretor = false;

            contextQuery.Command = @"SELECT COUNT(0) FROM ARMAZEM_LIVRO_2019 WHERE CATEGORIA = 'DIRETOR' and CPF = @CPF";

            contextQuery.Parameters.Add("@CPF", cpf);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                diretor = true;
            }

            return diretor;
        }

        public bool EhDiretor(string cpf)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool diretor = false;

            try
            {
                diretor = this.EhDiretor(ctx, cpf);
                return diretor;
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

        public DataTable ObtemCodigosPor(string cpf)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select CATEGORIA, CODIGO_ACESSO from ARMAZEM_LIVRO_2019 where CPF = @CPF";
                contextQuery.Parameters.Add("@CPF", cpf);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public DadosArmazemLivro ObtemDadosArmazemLivroPor(DataContext contexto, string cpf, string categoria)
        {
            DadosArmazemLivro dados = new DadosArmazemLivro();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT CODIGO_ACESSO, 
		                                        NOME_COMPLETO, 
		                                        A.CPF, 
		                                        P.IDFUNCIONAL,
		                                        A.CATEGORIA
                                        FROM [DBO].[ARMAZEM_LIVRO_2019] A
	                                        LEFT JOIN LY_PESSOA P ON A.PESSOA = P.PESSOA
                                        WHERE A.CPF = @CPF
                                            AND CATEGORIA = @CATEGORIA ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
                contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, categoria);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {                    
                    dados.CodigoAcesso = Convert.ToString(reader["CODIGO_ACESSO"]);
                    dados.Nome = Convert.ToString(reader["NOME_COMPLETO"]);
                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.IdFuncional = Convert.ToString(reader["IDFUNCIONAL"]);
                    dados.Categoria = Convert.ToString(reader["CATEGORIA"]);
                }

                return dados;
            }            
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }                
            }
        }

        public void EnviaCodigo(string cpf, string email, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            DadosArmazemLivro dadosArmazem = new DadosArmazemLivro();
            RN.DTOs.DadosEmail dadosEmail = new DadosEmail();
            bool erroValidacao = false;

            try
            {
                if (cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    erroValidacao = true;
                    throw new Exception("Este cpf não informado.");
                }

                //Busca Codigo Servidor e demais dados
                dadosArmazem = this.ObtemDadosArmazemLivroPor(contexto, cpf, "SERVIDOR");

                if (dadosArmazem.CodigoAcesso.IsNullOrEmptyOrWhiteSpace())
                {
                    erroValidacao = true;
                    throw new Exception("Este cpf não possui Voucher como SERVIDOR / PROFESSOR.");
                }

                //Atualiza Campos tabela
                this.AtualizaEnvioCodigo(contexto, cpf, email, usuario, "SERVIDOR");                     

                //Monta email                
                dadosEmail.Destinatario = email;
                dadosEmail.Remetente = System.Configuration.ConfigurationManager.AppSettings["EmailSistema"].ToString();
                dadosEmail.Assunto = "Voucher Armazém do Livro";
                dadosEmail.Texto = string.Format(@"{0}{1}
                                            <br />Seu Voucher para acesso ao Armazém do Livro é: <b>{2}</b>.
                                            <br />
                                            <br />Seeduc", 
                                                         dadosArmazem.Nome,
                                                         dadosArmazem.IdFuncional.IsNullOrEmptyOrWhiteSpace() || dadosArmazem.IdFuncional == "0" ? "," : " - Id Funcional: " + dadosArmazem.IdFuncional + ",",
                                                         dadosArmazem.CodigoAcesso);

                //Envia Email
                RN.Util.Email.Envia(dadosEmail);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();

                if (!erroValidacao && 
                    !Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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

        private void AtualizaEnvioCodigo(DataContext contexto, string cpf, string email, string usuario, string categoria)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.ARMAZEM_LIVRO_2019
                                    SET EMAIL_ENVIO_CODIGO = @EMAIL_ENVIO_CODIGO,
	                                    USUARIO_ENVIO_CODIGO = @USUARIO_ENVIO_CODIGO,
	                                    DATA_ENVIO_CODIGO = @DATA_ENVIO_CODIGO
                                    WHERE CPF = @CPF
	                                    AND CATEGORIA = @CATEGORIA ";

            contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
            contextQuery.Parameters.Add("@CATEGORIA", SqlDbType.VarChar, categoria);
            contextQuery.Parameters.Add("@EMAIL_ENVIO_CODIGO", SqlDbType.VarChar, email);
            contextQuery.Parameters.Add("@USUARIO_ENVIO_CODIGO", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATA_ENVIO_CODIGO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}