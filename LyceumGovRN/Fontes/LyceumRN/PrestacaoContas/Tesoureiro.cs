using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class Tesoureiro
    {
        public Entidades.Tesoureiro ObtemPor(int tesoureiroId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Tesoureiro tesoureiro = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Tesoureiro();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                            FROM PrestacaoContas.TESOUREIRO (NOLOCK)
                                            WHERE TESOUREIROID = @TESOUREIROID ";

                contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.Int, tesoureiroId);

                tesoureiro = contexto.TryToBindEntity<Entidades.Tesoureiro>(contextQuery);

                return tesoureiro;
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

        public ValidacaoDados Valida(Entidades.Tesoureiro tesoureiro, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int numero = 0;
            long cpf = 0;

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tesoureiro == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (tesoureiro.TesoureiroId == 0)
                {
                    mensagens.Add("Campo CODIGO é obrigatório.");
                }
            }

            if (tesoureiro.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else
            {
                //Verificar nome valido
                int n = 0;
                for (n = 0; n <= 9; n++)
                {
                    if (tesoureiro.Nome.IndexOf(n.ToString()) > 0)
                    {
                        mensagens.Add("Campo NOME não se pode ter números no nome.(" + n.ToString() + ").");
                    }
                }

                string[] vetorNome = tesoureiro.Nome.Split(' ');

                if (vetorNome.Length == 1)
                {
                    mensagens.Add("Campo NOME não pode ser formado por apenas uma palavra.");
                }

                if (Utils.VerificaTriploCaracter(tesoureiro.Nome))
                {
                    mensagens.Add("Campo NOME não se pode ter três letras iguais consecutivas.");
                }
            }

            if (tesoureiro.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CPF é obrigatório.");
            }
            else
            {
                if (!long.TryParse(tesoureiro.Cpf, out cpf) || (tesoureiro.Cpf.Length != 11))
                {
                    mensagens.Add("Campo CPF deve ser composto por 11 dígitos.");
                }
                else
                {
                    if (!Utils.ValidarCpf(tesoureiro.Cpf))
                    {
                        mensagens.Add("CPF inválido.");
                    }
                }
            }
            if (tesoureiro.Rg.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("campo RG é obrigatório.");
            }
            if (tesoureiro.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório.");
            }
            else if ((tesoureiro.Cep.Length != 8) || !int.TryParse(tesoureiro.Cep, out numero))
            {
                mensagens.Add("Campo CEP deve ser composto por 8 dígitos.");
            }

            if (tesoureiro.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório.");
            }

            if (tesoureiro.Numero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO é obrigatório.");
            }

            if (tesoureiro.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }

            if (tesoureiro.MunicipioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO é obrigatório.");
            }

            if (tesoureiro.Telefone.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TELEFONE é obrigatório.");
            }
            else if (!Validacao.ValidaTelefoneComDDD(tesoureiro.Telefone) && !Validacao.ValidaCelularComDDD(tesoureiro.Telefone))
            {                
                mensagens.Add("O campo TELEFONE é inválido.!");
            }

            if (tesoureiro.Email.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo E-MAIL é obrigatório.");
            }
            else if (!RN.Validacao.ValidaEmail(tesoureiro.Email))
            {
                mensagens.Add("Campo E-MAIL inválido.");
            }

            if (tesoureiro.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o cpf já foi cadastrado anteriormente
                    if (this.PossuiOutroCpfCadastradoPor(contexto, tesoureiro.Cpf, tesoureiro.TesoureiroId))
                    {
                        mensagens.Add("Este CPF já foi cadastrado como tesoureiro!");
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

        private bool PossuiOutroCpfCadastradoPor(DataContext ctx, string cpf, int tesoureiroId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   PrestacaoContas.TESOUREIRO (NOLOCK) 
                                        WHERE  CPF = @CPF 
                                               AND TESOUREIROID <> @TESOUREIROID  ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
                contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.Int, tesoureiroId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public void Insere(Entidades.Tesoureiro tesoureiro)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO PrestacaoContas.TESOUREIRO
                                               (NOME
                                               ,CPF
                                               ,RG
                                               ,ENDERECO
                                               ,NUMERO
                                               ,COMPLEMENTO
                                               ,BAIRRO
                                               ,MUNICIPIOID
                                               ,CEP
                                               ,EMAIL
                                               ,TELEFONE
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@NOME, 
                                               @CPF, 
                                               @RG, 
                                               @ENDERECO, 
                                               @NUMERO,
                                               @COMPLEMENTO, 
                                               @BAIRRO, 
                                               @MUNICIPIOID, 
                                               @CEP,
                                               @EMAIL, 
                                               @TELEFONE, 
                                               @USUARIOID, 
                                               @DATACADASTRO,
                                               @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, tesoureiro.Nome);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, tesoureiro.Cpf);
                contextQuery.Parameters.Add("@RG", SqlDbType.VarChar, tesoureiro.Rg);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, tesoureiro.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, tesoureiro.Numero);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, tesoureiro.Bairro);
                contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, tesoureiro.MunicipioId);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, tesoureiro.Cep);
                contextQuery.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar, tesoureiro.Complemento);
                contextQuery.Parameters.Add("@EMAIL", SqlDbType.VarChar, tesoureiro.Email);
                contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, tesoureiro.Telefone);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tesoureiro.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now); 

                contexto.ApplyModifications(contextQuery);
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

        public void Atualiza(Entidades.Tesoureiro tesoureiro)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE PrestacaoContas.TESOUREIRO
                                           SET NOME = @NOME, 
                                              CPF = @CPF, 
                                              RG = @RG, 
                                              ENDERECO = @ENDERECO, 
                                              NUMERO = @NUMERO, 
                                              COMPLEMENTO = @COMPLEMENTO, 
                                              BAIRRO = @BAIRRO, 
                                              MUNICIPIOID = @MUNICIPIOID, 
                                              CEP = @CEP, 
                                              EMAIL = @EMAIL, 
                                              TELEFONE = @TELEFONE, 
                                              USUARIOID = @USUARIOID, 
                                              DATAALTERACAO = @DATAALTERACAO
                                         WHERE TESOUREIROID = @TESOUREIROID ";

                contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.Int, tesoureiro.TesoureiroId);
                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, tesoureiro.Nome);
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, tesoureiro.Cpf);
                contextQuery.Parameters.Add("@RG", SqlDbType.VarChar, tesoureiro.Rg);
                contextQuery.Parameters.Add("@ENDERECO", SqlDbType.VarChar, tesoureiro.Endereco);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.VarChar, tesoureiro.Numero);
                contextQuery.Parameters.Add("@BAIRRO", SqlDbType.VarChar, tesoureiro.Bairro);
                contextQuery.Parameters.Add("@MUNICIPIOID", SqlDbType.VarChar, tesoureiro.MunicipioId);
                contextQuery.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar, tesoureiro.Complemento);
                contextQuery.Parameters.Add("@CEP", SqlDbType.VarChar, tesoureiro.Cep);
                contextQuery.Parameters.Add("@EMAIL", SqlDbType.VarChar, tesoureiro.Email);
                contextQuery.Parameters.Add("@TELEFONE", SqlDbType.VarChar, tesoureiro.Telefone);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, tesoureiro.UsuarioId);               
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int tesoureiroId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            MandatoAae rnMandatoAae = new MandatoAae();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (tesoureiroId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi associado a uma aae
                    if (rnMandatoAae.PossuiTesoureiroPor(contexto, tesoureiroId))
                    {
                        mensagens.Add("Este tesoureiro não pode ser removido foi vinculado a uma Associação de Apoio à Escola.");
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

        public void Remove(int tesoureiroId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE PrestacaoContas.TESOUREIRO
                                         WHERE TESOUREIROID = @TESOUREIROID ";

                contextQuery.Parameters.Add("@TESOUREIROID", SqlDbType.Int, tesoureiroId);
   
                contexto.ApplyModifications(contextQuery);
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

        public Entidades.Tesoureiro ObtemTesoureiroPor(string CPF)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Tesoureiro tesoureiro = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Tesoureiro();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT *
                                            FROM PrestacaoContas.TESOUREIRO (NOLOCK)
                                            WHERE CPF = @CPF ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, CPF);

                tesoureiro = contexto.TryToBindEntity<Entidades.Tesoureiro>(contextQuery);

                return tesoureiro;
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
