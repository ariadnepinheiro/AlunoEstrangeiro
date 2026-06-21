using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class UsuarioExterno : RNBase
    {
        #region Propriedades e Enum
        public enum TiposUsuarioExterno
        {
            [StringValue("Analisador Protocolo Prestação")]
            AnalisadorProtocolo = 1
        }
        #endregion

        public RN.DTOs.DadosExterno ObtemDadosExternoPor(string cpf)
        {
            RN.DTOs.DadosExterno dadosExterno = new RN.DTOs.DadosExterno();
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.Aluno rnAluno = new Aluno();
            RN.Docentes rnDocente = new Docentes();
            RN.VinculoLy rnVinculo = new VinculoLy();
            RN.Pessoa rnPessoa = new Pessoa();

            try
            {
                dadosExterno = rnPessoa.ObtemDadosExternoPor(ctx, cpf);

                if (dadosExterno.PessoaId > 0)
                {
                    //Verifica se a pessoa é um aluno ativo
                    if (rnAluno.EhAlunoAtivoPor(ctx, dadosExterno.PessoaId))
                    {
                        dadosExterno.Bloqueado = true;
                    }

                    //Verifica se a pessoa é docente
                    if (rnDocente.EhDocentePor(ctx, dadosExterno.PessoaId))
                    {
                        dadosExterno.Bloqueado = true;
                    }

                    //Verifica se a pessoa é servidor
                    if (rnVinculo.EhServidorPor(ctx, dadosExterno.PessoaId))
                    {
                        dadosExterno.Bloqueado = true;
                    }
                }

                return dadosExterno;
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

        public RN.DTOs.DadosExterno ObtemDadosExternoPor(int usuarioExternoId)
        {
            RN.DTOs.DadosExterno dadosExterno = new RN.DTOs.DadosExterno();
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.Aluno rnAluno = new Aluno();
            RN.Docentes rnDocente = new Docentes();
            RN.VinculoLy rnVinculo = new VinculoLy();

            try
            {
                dadosExterno = this.ObtemDadosExternoPor(ctx, usuarioExternoId);

                //Verifica se a pessoa é um aluno ativo
                if (rnAluno.EhAlunoAtivoPor(ctx, dadosExterno.PessoaId))
                {
                    dadosExterno.Bloqueado = true;
                }

                //Verifica se a pessoa é docente
                if (rnDocente.EhDocentePor(ctx, dadosExterno.PessoaId))
                {
                    dadosExterno.Bloqueado = true;
                }

                //Verifica se a pessoa é servidor
                if (rnVinculo.EhServidorPor(ctx, dadosExterno.PessoaId))
                {
                    dadosExterno.Bloqueado = true;
                }

                return dadosExterno;
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

        private RN.DTOs.DadosExterno ObtemDadosExternoPor(DataContext ctx, int usuarioExternoId)
        {
            RN.DTOs.DadosExterno dadosExterno = new RN.DTOs.DadosExterno();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT 
		                            U.USUARIOEXTERNOID,
                                    P.PESSOA, 
                                    P.CPF, 
                                    P.NOME_COMPL, 
                                    P.DT_NASC, 
                                    P.SEXO, 
                                    P.EST_CIVIL, 
                                    P.ENDERECO, 
                                    P.END_NUM, 
                                    P.END_COMPL, 
                                    P.BAIRRO, 
                                    P.CEP, 
                                    M.NOME AS MUNICIPIO, 
                                    P.END_MUNICIPIO,
                                    P.END_PAIS,
                                    P.FONE,
                                    P.E_MAIL,
                                    P.E_MAIL_INTERNO,
                                    P.CELULAR,
                                    u.ATIVO ,
                                    u.USUARIOID,
                                    u.DATACADASTRO,
                                    u.DATAALTERACAO,
									T.TIPOUSUARIOEXTERNOID,
									T.DESCRICAO AS TIPOUSUARIOEXTERNO
                            FROM   LY_PESSOA P ( NOLOCK ) 
		                            INNER JOIN RecursosHumanos.USUARIOEXTERNO U ( NOLOCK ) 
                                            ON P.PESSOA = U.PESSOAID
                                    LEFT JOIN RecursosHumanos.TIPOUSUARIOEXTERNO T ( NOLOCK ) 
				                            ON t.TIPOUSUARIOEXTERNOID = u.TIPOUSUARIOEXTERNOID
                                    INNER JOIN MUNICIPIO M ( NOLOCK ) 
                                            ON M.CODIGO = CONVERT(INT, P.END_MUNICIPIO) 
                            WHERE  U.USUARIOEXTERNOID = @USUARIOEXTERNOID  ";

                contextQuery.Parameters.Add("@USUARIOEXTERNOID", SqlDbType.Int, usuarioExternoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosExterno.UsuarioExternoId = Convert.ToInt32(reader["USUARIOEXTERNOID"]);
                    dadosExterno.TipoExternoId = Convert.ToInt32(reader["TIPOUSUARIOEXTERNOID"]);
                    dadosExterno.TipoExterno = Convert.ToString(reader["TIPOUSUARIOEXTERNO"]);
                    dadosExterno.PessoaId = Convert.ToDecimal(reader["PESSOA"]);
                    dadosExterno.Cpf = Convert.ToString(reader["CPF"]);
                    dadosExterno.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dadosExterno.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    dadosExterno.Sexo = Convert.ToString(reader["SEXO"]);
                    dadosExterno.EstadoCivl = Convert.ToString(reader["EST_CIVIL"]);
                    dadosExterno.PaisEndereco = Convert.ToString(reader["END_PAIS"]);
                    dadosExterno.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosExterno.Numero = Convert.ToString(reader["END_NUM"]);
                    dadosExterno.Complemento = Convert.ToString(reader["END_COMPL"]);
                    dadosExterno.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosExterno.Cep = Convert.ToString(reader["CEP"]);
                    dadosExterno.Municipio = Convert.ToString(reader["END_MUNICIPIO"]);
                    dadosExterno.Telefone = Convert.ToString(reader["FONE"]);
                    dadosExterno.Email = Convert.ToString(reader["E_MAIL_INTERNO"]);
                    dadosExterno.EmailAlternativo = Convert.ToString(reader["E_MAIL"]);
                    dadosExterno.Celular = Convert.ToString(reader["CELULAR"]);
                    dadosExterno.Ativo = Convert.ToBoolean(reader["ATIVO"]);
                    dadosExterno.UsuarioResponsavel = Convert.ToString(reader["USUARIOID"]);
                    dadosExterno.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    dadosExterno.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return dadosExterno;
        }

        public ValidacaoDados Valida(DadosExterno dadosExterno, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Pessoa rnPessoa = new Pessoa();
            int numero = 0;
            long cpf = 0;

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosExterno == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (dadosExterno.PessoaId == 0)
                {
                    mensagens.Add("Campo PESSOA é obrigatório.");
                }

                if (dadosExterno.UsuarioExternoId == 0)
                {
                    mensagens.Add("Campo USUARIO EXTERNO é obrigatório.");
                }
            }

            if (dadosExterno.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (dadosExterno.TipoExternoId == 0)
            {
                mensagens.Add("Campo TIPO EXTERNO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                if (dadosExterno.Cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo CPF é de preenchimento obrigatório.");
                }
                else if ((dadosExterno.Cpf.Length != 11) || !long.TryParse(dadosExterno.Cpf, out cpf))
                {
                    mensagens.Add("Campo CPF deve ser composto por 11 dígitos.");
                }
                else
                {
                    if (!Utils.ValidarCpf(dadosExterno.Cpf))
                    {
                        mensagens.Add("CPF inválido.");
                    }
                }

                //Verificar se os dados podem ser alterados
                if (dadosExterno.Bloqueado)
                {
                    if (dadosExterno.PessoaId <= 0)
                    {
                        mensagens.Add("Campo PESSOA é obrigatório.");
                    }
                }
                else
                {
                    if (dadosExterno.Nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NOME é obrigatório.");
                    }
                    else
                    {
                        //Verificar nome valido
                        int n = 0;
                        for (n = 0; n <= 9; n++)
                        {
                            if (dadosExterno.Nome.IndexOf(n.ToString()) > 0)
                            {
                                mensagens.Add("Campo NOME não se pode ter números no nome.(" + n.ToString() + ").");
                            }
                        }

                        string[] vetorNome = dadosExterno.Nome.Split(' ');

                        if (vetorNome.Length == 1)
                        {
                            mensagens.Add("Campo NOME não pode ser formado por apenas uma palavra.");
                        }

                        if (Utils.VerificaTriploCaracter(dadosExterno.Nome))
                        {
                            mensagens.Add("Campo NOME não se pode ter três letras iguais consecutivas.");
                        }
                    }

                    if (dadosExterno.DataNascimento == DateTime.MinValue || dadosExterno.DataNascimento == null)
                    {
                        mensagens.Add("O campo DATA DE NASCIMENTO é de preenchimento obrigatório.");
                    }

                    if (dadosExterno.Sexo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo SEXO é de preenchimento obrigatório.");
                    }

                    if (dadosExterno.PaisEndereco.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo PAÍS DE ENDEREÇO é de preenchimento obrigatório.");
                    }

                    if (dadosExterno.Cep.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo CEP é obrigatório.");
                    }
                    else if ((dadosExterno.Cep.Length != 8) || !int.TryParse(dadosExterno.Cep, out numero))
                    {
                        mensagens.Add("Campo CEP deve ser composto por 8 dígitos.");
                    }

                    if (dadosExterno.Municipio.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo MUNICIPIO é obrigatório.");
                    }

                    if (dadosExterno.Endereco.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo ENDEREÇO é obrigatório.");
                    }

                    if (dadosExterno.Numero.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NÚMERO é obrigatório.");
                    }

                    if (dadosExterno.Bairro.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo BAIRRO é obrigatório.");
                    }

                    if (dadosExterno.Telefone.IsNullOrEmptyOrWhiteSpace() && dadosExterno.Celular.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("É obrigatório informar TELEFONE ou CELULAR.");
                    }
                    else
                    {
                        if (!dadosExterno.Telefone.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (!Validacao.ValidaTelefoneComDDD(dadosExterno.Telefone))
                            {
                                mensagens.Add("O campo TELEFONE é inválido.!");
                            }
                        }

                        if (!dadosExterno.Celular.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (!Validacao.ValidaCelularComDDD(dadosExterno.Celular))
                            {
                                mensagens.Add("O campo CELULAR é inválido.!");
                            }
                        }
                    }

                    if (!dadosExterno.Email.IsNullOrEmptyOrWhiteSpace() && !RN.Validacao.ValidaEmail(dadosExterno.Email))
                    {
                        mensagens.Add("Campo E-MAIL INSTITUCIONAL inválido.");
                    }

                    if (!dadosExterno.EmailAlternativo.IsNullOrEmptyOrWhiteSpace() && !RN.Validacao.ValidaEmail(dadosExterno.EmailAlternativo))
                    {
                        mensagens.Add("Campo E-MAIL ALTERNATIVO inválido.");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o cpf já foi cadastrado anteriormente
                    if (this.PossuiOutroCpfCadastradoPor(contexto, dadosExterno.Cpf, dadosExterno.UsuarioExternoId))
                    {
                        mensagens.Add("Este CPF já foi utilizado!");
                    }

                    if (dadosExterno.PessoaId > 0)
                    {
                        //Verifica se o cpf é reslmente o cpf da pessoa
                        if (rnPessoa.ObtemCpfPor(contexto, dadosExterno.PessoaId) != dadosExterno.Cpf)
                        {
                            mensagens.Add("Este CPF não pertence a esta PESSOA.");
                        }

                        //Verifica se a pessoa já foi cadastrada anteriormente
                        if (this.PossuiOutraPessoaCadastradaPor(contexto, dadosExterno.PessoaId, dadosExterno.UsuarioExternoId))
                        {
                            mensagens.Add("Esta PESSOA já foi utilizada.");
                        }
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

        public void Insere(DadosExterno dadosExterno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Entidades.LyPessoa pessoa = new LyPessoa();
            RN.RecursosHumanos.Entidades.UsuarioExterno usuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.Entidades.UsuarioExterno();

            try
            {
                if (!dadosExterno.Bloqueado)
                {
                    //Monta Entidade PESSOA
                    pessoa.Nome_compl = dadosExterno.Nome;
                    pessoa.Cpf = dadosExterno.Cpf;
                    pessoa.Cep = dadosExterno.Cep;
                    pessoa.End_municipio = dadosExterno.Municipio;
                    pessoa.Endereco = dadosExterno.Endereco;
                    pessoa.End_num = dadosExterno.Numero;
                    pessoa.End_compl = dadosExterno.Complemento;
                    pessoa.Bairro = dadosExterno.Bairro;
                    pessoa.E_mail_interno = dadosExterno.Email;
                    pessoa.E_mail = dadosExterno.EmailAlternativo;
                    pessoa.Fone = dadosExterno.Telefone;
                    pessoa.Celular = dadosExterno.Celular;
                    pessoa.Sexo = dadosExterno.Sexo;
                    pessoa.Est_civil = dadosExterno.EstadoCivl;
                    pessoa.End_pais = dadosExterno.PaisEndereco;
                    pessoa.Dt_nasc = dadosExterno.DataNascimento;

                    //Insere ou Atualiza PESSOA
                    if (dadosExterno.PessoaId <= 0)
                    {
                        rnPessoa.InserePessoaExterna(ctx, pessoa);
                        dadosExterno.PessoaId = pessoa.Pessoa;
                    }
                    else
                    {
                        pessoa.Pessoa = dadosExterno.PessoaId;
                        rnPessoa.AtualizaPessoaExterna(ctx, pessoa);
                    }
                }

                //Monta Entidade USUARIOEXTERNO
                usuarioExterno.TipoUsuarioExternoId = dadosExterno.TipoExternoId;
                usuarioExterno.PessoaId = dadosExterno.PessoaId;
                usuarioExterno.Ativo = dadosExterno.Ativo;
                usuarioExterno.UsuarioId = dadosExterno.UsuarioResponsavel;

                //Insere USUARIOEXTERNO
                this.Insere(ctx, usuarioExterno);
                dadosExterno.UsuarioExternoId = usuarioExterno.UsuarioExternoId;
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

        public void Atualiza(DadosExterno dadosExterno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Entidades.LyPessoa pessoa = new LyPessoa();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.RecursosHumanos.Entidades.UsuarioExterno usuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.Entidades.UsuarioExterno();
            RN.Usuarios rnUsuario = new Usuarios();
            int grupoUsuario = 0;

            try
            {
                //Monta Entidade UsuarioExterno
                usuarioExterno.UsuarioExternoId = dadosExterno.UsuarioExternoId;
                usuarioExterno.Ativo = dadosExterno.Ativo;
                usuarioExterno.UsuarioId = dadosExterno.UsuarioResponsavel;
                usuarioExterno.TipoUsuarioExternoId = dadosExterno.TipoExternoId;

                //Atualiza UsuarioExterno
                this.Atualiza(ctx, usuarioExterno);

                //Verifica se dados da pessoa podem ser editados
                if (!dadosExterno.Bloqueado)
                {
                    //Atualiza dados pessoa
                    pessoa.Pessoa = dadosExterno.PessoaId;
                    pessoa.Nome_compl = dadosExterno.Nome;
                    pessoa.Cpf = dadosExterno.Cpf;
                    pessoa.Cep = dadosExterno.Cep;
                    pessoa.End_municipio = dadosExterno.Municipio;
                    pessoa.Endereco = dadosExterno.Endereco;
                    pessoa.End_num = dadosExterno.Numero;
                    pessoa.End_compl = dadosExterno.Complemento;
                    pessoa.Bairro = dadosExterno.Bairro;
                    pessoa.E_mail_interno = dadosExterno.Email;
                    pessoa.E_mail = dadosExterno.EmailAlternativo;
                    pessoa.Fone = dadosExterno.Telefone;
                    pessoa.Celular = dadosExterno.Celular;
                    pessoa.Est_civil = dadosExterno.EstadoCivl;
                    pessoa.Sexo = dadosExterno.Sexo;
                    pessoa.End_pais = dadosExterno.PaisEndereco;
                    pessoa.Dt_nasc = dadosExterno.DataNascimento;

                    rnPessoa.AtualizaPessoaExterna(ctx, pessoa);
                }

                //Verifica se usuario está sendo desativado
                if (!dadosExterno.Ativo)
                {
                    //Desativa usuario de sistema caso exista
                    grupoUsuario = dadosExterno.TipoExternoId;
                    rnUsuario.DesativaUsuarioExterno(ctx, dadosExterno.PessoaId, grupoUsuario);
                }
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

        public ValidacaoDados ValidaDesativacao(int usuarioExternoId, int tipoExternoId, decimal pessoaId, string usuarioResponsavel)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };            

            if (usuarioExternoId <= 0)
            {
                mensagens.Add("Campo USUARIO EXTERNO é obrigatório.");
            }

            if (tipoExternoId <= 0)
            {
                mensagens.Add("Campo TIPO USUARIO EXTERNO é obrigatório.");
            }

            if (pessoaId <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se recurso está ativo
                    if (!this.EhExternoAtivoPor(contexto, usuarioExternoId))
                    {
                        mensagens.Add("Este recurso externo já se encontra desativado.");
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

        private bool EhExternoAtivoPor(DataContext contexto, int externoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   Protocolo.ANALISADOR (NOLOCK) 
                                    WHERE  ANALISADORID = @ANALISADORID
                                            AND ATIVO = 1 ";

                contextQuery.Parameters.Add("@ANALISADORID", SqlDbType.Int, externoId);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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
        }

        public void Desativa(int usuarioExternoId, int tipoExternoId, decimal pessoaId, string usuarioResponsavel)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Usuarios rnUsuario = new Usuarios();
            RN.RecursosHumanos.Entidades.UsuarioExterno usuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.Entidades.UsuarioExterno();
            int grupoUsuario = 0;

            try
            {
                //Monta Entidade UsuarioExterno
                usuarioExterno.UsuarioExternoId = usuarioExternoId;
                usuarioExterno.Ativo = false;
                usuarioExterno.UsuarioId = usuarioResponsavel;
                usuarioExterno.TipoUsuarioExternoId = tipoExternoId;

                //Atualiza UsuarioExterno
                this.Atualiza(ctx, usuarioExterno);
 
                //Desativa usuario de sistema caso exista
                grupoUsuario = tipoExternoId;
                rnUsuario.DesativaUsuarioExterno(ctx, pessoaId, grupoUsuario);
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

        private bool PossuiOutroCpfCadastradoPor(DataContext ctx, string cpf, int usuarioExternoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM RECURSOSHUMANOS.USUARIOEXTERNO U ( NOLOCK ) 
                                               INNER JOIN LY_PESSOA P(NOLOCK) 
                                                       ON U.PESSOAID = P.PESSOA 
                                        WHERE  CPF = @CPF 
                                               AND U.USUARIOEXTERNOID <> @USUARIOEXTERNOID  ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
                contextQuery.Parameters.Add("@USUARIOEXTERNOID", SqlDbType.Int, usuarioExternoId);

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

        private bool PossuiOutraPessoaCadastradaPor(DataContext ctx, decimal pessoa, int usuarioExternoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM   RECURSOSHUMANOS.USUARIOEXTERNO ( NOLOCK ) 
                                WHERE  PESSOAID = @PESSOAID
                                    AND USUARIOEXTERNOID <> @USUARIOEXTERNOID ";

                contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);
                contextQuery.Parameters.Add("@USUARIOEXTERNOID", SqlDbType.Int, usuarioExternoId);

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

        public bool PossuiTipoUsuarioExternoPor(DataContext contexto, int tipoUsuarioExternoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM RecursosHumanos.USUARIOEXTERNO (NOLOCK)
                                    WHERE TIPOUSUARIOEXTERNOID = @TIPOUSUARIOEXTERNOID ";

            contextQuery.Parameters.Add("@TIPOUSUARIOEXTERNOID", tipoUsuarioExternoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhExternoPor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                                    FROM   RECURSOSHUMANOS.USUARIOEXTERNO 
                                    WHERE  PESSOAID = @PESSOAID "
            };

            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool EhAtivoPor(decimal pessoa, int tipoExternoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM RECURSOSHUMANOS.USUARIOEXTERNO (NOLOCK)
                                WHERE PESSOAID = @PESSOAID
                                AND TIPOUSUARIOEXTERNOID = @TIPOUSUARIOEXTERNOID
                                AND ATIVO = 1 ";

                contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);
                contextQuery.Parameters.Add("@TIPOUSUARIOEXTERNOID", SqlDbType.Int, tipoExternoId);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public void Insere(DataContext ctx, Entidades.UsuarioExterno usuarioExterno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.USUARIOEXTERNO
                                               (TIPOUSUARIOEXTERNOID
                                               ,PESSOAID
                                               ,ATIVO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@TIPOUSUARIOEXTERNOID
                                               ,@PESSOAID
                                               ,@ATIVO
                                               ,@USUARIOID
                                               ,@DATACADASTRO
                                               ,@DATAALTERACAO)
                                                                    			
                                SELECT IDENT_CURRENT('RecursosHumanos.USUARIOEXTERNO') ";

            contextQuery.Parameters.Add("@TIPOUSUARIOEXTERNOID", SqlDbType.Decimal, usuarioExterno.TipoUsuarioExternoId);
            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, usuarioExterno.PessoaId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, usuarioExterno.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioExterno.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            usuarioExterno.UsuarioExternoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
        }

        public void Atualiza(DataContext ctx, Entidades.UsuarioExterno usuarioExterno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE RecursosHumanos.USUARIOEXTERNO
                                        SET    ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO,
                                               TIPOUSUARIOEXTERNOID = @TIPOUSUARIOEXTERNOID
                                        WHERE  USUARIOEXTERNOID = @USUARIOEXTERNOID ";

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioExterno.UsuarioId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, usuarioExterno.Ativo);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@TIPOUSUARIOEXTERNOID", SqlDbType.Int, usuarioExterno.TipoUsuarioExternoId);
            contextQuery.Parameters.Add("@USUARIOEXTERNOID", SqlDbType.Int, usuarioExterno.UsuarioExternoId);

            ctx.ApplyModifications(contextQuery);
        }
    }
}
