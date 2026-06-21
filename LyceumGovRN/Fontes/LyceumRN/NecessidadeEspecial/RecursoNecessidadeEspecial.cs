using System;
using System.Collections.Generic;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Linq;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;
using System.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class RecursoNecessidadeEspecial
    {
        public DadosRecursoNecessidadeEspecial ObtemDadosRecursoNecessidadeEspecialPor(string cpf)
        {
            DadosRecursoNecessidadeEspecial recurso = new RN.DTOs.DadosRecursoNecessidadeEspecial();
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.Aluno rnAluno = new Aluno();
            RN.Docentes rnDocente = new Docentes();
            RN.VinculoLy rnVinculo = new VinculoLy();
            RN.Pessoa rnPessoa = new Pessoa();

            try
            {
                recurso = rnPessoa.ObtemDadosRecursoNecessidadeEspecialPor(ctx, cpf);

                if (recurso.PessoaId > 0)
                {
                    //Verifica se a pessoa é um aluno ativo
                    if (rnAluno.EhAlunoAtivoPor(ctx, recurso.PessoaId))
                    {
                        recurso.Bloqueado = true;
                    }

                    //Verifica se a pessoa é docente
                    if (rnDocente.EhDocentePor(ctx, recurso.PessoaId))
                    {
                        recurso.Bloqueado = true;
                    }

                    //Verifica se a pessoa é servidor
                    if (rnVinculo.EhServidorPor(ctx, recurso.PessoaId))
                    {
                        recurso.Bloqueado = true;
                    }
                }

                return recurso;
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

        public DadosRecursoNecessidadeEspecial ObtemDadosRecursoNecessidadeEspecialPor(int recursoId)
        {
            RN.DTOs.DadosRecursoNecessidadeEspecial recurso = new RN.DTOs.DadosRecursoNecessidadeEspecial();
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.Aluno rnAluno = new Aluno();
            RN.Docentes rnDocente = new Docentes();
            RN.VinculoLy rnVinculo = new VinculoLy();

            try
            {
                recurso = this.ObtemDadosRecursoNecessidadeEspecialPor(ctx, recursoId);

                //Verifica se a pessoa é um aluno ativo
                if (rnAluno.EhAlunoAtivoPor(ctx, recurso.PessoaId))
                {
                    recurso.Bloqueado = true;
                }

                //Verifica se a pessoa é docente
                if (rnDocente.EhDocentePor(ctx, recurso.PessoaId))
                {
                    recurso.Bloqueado = true;
                }

                //Verifica se a pessoa é servidor
                if (rnVinculo.EhServidorPor(ctx, recurso.PessoaId))
                {
                    recurso.Bloqueado = true;
                }

                //Obtem tipos cadastrados
                if (recurso.RecursoNecessidadeEspecialId > 0)
                {
                    recurso.TipoRecursoNecessidadeEspecial = this.ListaTipoRecursoNecessidadeEspecialPor(ctx, recurso.RecursoNecessidadeEspecialId);
                }

                return recurso;
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

        private DadosRecursoNecessidadeEspecial ObtemDadosRecursoNecessidadeEspecialPor(DataContext ctx, int recursoId)
        {
            DadosRecursoNecessidadeEspecial dadosPessoais = new DadosRecursoNecessidadeEspecial();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT E.RECURSONECESSIDADEESPECIALID,
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
                                                    P.CELULAR,
                                                    E.ATIVO ,
                                                    E.USUARIOID,
                                                    E.DATACADASTRO,
                                                    E.DATAALTERACAO
                                            FROM   LY_PESSOA P ( NOLOCK ) 
                                                    INNER JOIN [NecessidadeEspecial].[RECURSONECESSIDADEESPECIAL] E ( NOLOCK ) 
                                                            ON P.PESSOA = E.PESSOAID 
                                                    INNER JOIN MUNICIPIO M ( NOLOCK ) 
                                                            ON M.CODIGO = CONVERT(INT, P.END_MUNICIPIO) 
                                            WHERE  E.RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Decimal, recursoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosPessoais.RecursoNecessidadeEspecialId = Convert.ToInt32(reader["RECURSONECESSIDADEESPECIALID"]);
                    dadosPessoais.PessoaId = Convert.ToDecimal(reader["PESSOA"]);
                    dadosPessoais.Cpf = Convert.ToString(reader["CPF"]);
                    dadosPessoais.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dadosPessoais.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    dadosPessoais.Sexo = Convert.ToString(reader["SEXO"]);
                    dadosPessoais.EstadoCivl = Convert.ToString(reader["EST_CIVIL"]);
                    dadosPessoais.PaisEndereco = Convert.ToString(reader["END_PAIS"]);
                    dadosPessoais.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosPessoais.Numero = Convert.ToString(reader["END_NUM"]);
                    dadosPessoais.Complemento = Convert.ToString(reader["END_COMPL"]);
                    dadosPessoais.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosPessoais.Cep = Convert.ToString(reader["CEP"]);
                    dadosPessoais.Municipio = Convert.ToString(reader["END_MUNICIPIO"]);
                    dadosPessoais.Telefone = Convert.ToString(reader["FONE"]);
                    dadosPessoais.Email = Convert.ToString(reader["E_MAIL"]);
                    dadosPessoais.Celular = Convert.ToString(reader["CELULAR"]);
                    dadosPessoais.Ativo = Convert.ToBoolean(reader["ATIVO"]);
                    dadosPessoais.UsuarioId = Convert.ToString(reader["USUARIOID"]);
                    dadosPessoais.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    dadosPessoais.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return dadosPessoais;
        }

        private List<int> ListaTipoRecursoNecessidadeEspecialPor(DataContext ctx, int recursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<int> retorno = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT TIPORECURSONECESSIDADEESPECIALID
                                        FROM [NECESSIDADEESPECIAL].[RECURSONECESSIDADEESPECIAL_TIPORECURSONECESSIDADEESPECIAL] ( NOLOCK ) 
                                        WHERE RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno.Add(Convert.ToInt32(reader["TIPORECURSONECESSIDADEESPECIALID"]));
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public ValidacaoDados Valida(DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Pessoa rnPessoa = new Pessoa();
            CuidadorAluno rnCuidadorAluno = new CuidadorAluno();
            LedorAluno rnLedorAluno = new LedorAluno();
            InterpreteTurma rnInterpreteTurma = new InterpreteTurma();
            bool cuidadorAtivo = false;
            bool ledorAtivo = false;
            bool interpreteAtivo = false;
            int numero = 0;
            long cpf = 0;

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosRecursoNecessidadeEspecial == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }

                if (dadosRecursoNecessidadeEspecial.PessoaId == 0)
                {
                    mensagens.Add("Campo PESSOA é obrigatório.");
                }
            }

            if (dadosRecursoNecessidadeEspecial.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                if (dadosRecursoNecessidadeEspecial.Cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo CPF é de preenchimento obrigatório.");
                }
                else
                {
                    if (dadosRecursoNecessidadeEspecial.Cpf == "00000000000")
                    {
                        mensagens.Add("Este código representa as associações de empresas e não pode ser modificado.");
                    }
                    else
                    {
                        if ((dadosRecursoNecessidadeEspecial.Cpf.Length != 11) || !long.TryParse(dadosRecursoNecessidadeEspecial.Cpf, out cpf))
                        {
                            mensagens.Add("Campo CPF deve ser composto por 11 dígitos.");
                        }
                        else
                        {
                            if (!Utils.ValidarCpf(dadosRecursoNecessidadeEspecial.Cpf))
                            {
                                mensagens.Add("CPF inválido.");
                            }
                        }
                    }
                }

                if (dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial.Count == 0)
                {
                    mensagens.Add("A escolha de pelo menos um TIPO DE RECURSO é obrigatória.");
                }

                //Verificar se os dados podem ser alterados
                if (dadosRecursoNecessidadeEspecial.Bloqueado)
                {
                    if (dadosRecursoNecessidadeEspecial.PessoaId <= 0)
                    {
                        mensagens.Add("Campo PESSOA é obrigatório.");
                    }
                }
                else
                {
                    if (dadosRecursoNecessidadeEspecial.Nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NOME é obrigatório.");
                    }
                    else
                    {
                        //Verificar nome valido
                        int n = 0;
                        for (n = 0; n <= 9; n++)
                        {
                            if (dadosRecursoNecessidadeEspecial.Nome.IndexOf(n.ToString()) > 0)
                            {
                                mensagens.Add("Campo NOME não se pode ter números no nome.(" + n.ToString() + ").");
                            }
                        }

                        string[] vetorNome = dadosRecursoNecessidadeEspecial.Nome.Split(' ');

                        if (vetorNome.Length == 1)
                        {
                            mensagens.Add("Campo NOME não pode ser formado por apenas uma palavra.");
                        }

                        if (Utils.VerificaTriploCaracter(dadosRecursoNecessidadeEspecial.Nome))
                        {
                            mensagens.Add("Campo NOME não se pode ter três letras iguais consecutivas.");
                        }
                    }

                    if (dadosRecursoNecessidadeEspecial.DataNascimento == DateTime.MinValue || dadosRecursoNecessidadeEspecial.DataNascimento == null)
                    {
                        mensagens.Add("O campo DATA DE NASCIMENTO é de preenchimento obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.Sexo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo SEXO é de preenchimento obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.EstadoCivl.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo ESTADO CIVIL é de preenchimento obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.PaisEndereco.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("O campo PAÍS DE ENDEREÇO é de preenchimento obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.Cep.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo CEP é obrigatório.");
                    }
                    else if ((dadosRecursoNecessidadeEspecial.Cep.Length != 8) || !int.TryParse(dadosRecursoNecessidadeEspecial.Cep, out numero))
                    {
                        mensagens.Add("Campo CEP deve ser composto por 8 dígitos.");
                    }

                    if (dadosRecursoNecessidadeEspecial.Municipio.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo MUNICIPIO é obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.Endereco.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo ENDEREÇO é obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.Numero.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NÚMERO é obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.Bairro.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo BAIRRO é obrigatório.");
                    }

                    if (dadosRecursoNecessidadeEspecial.Telefone.IsNullOrEmptyOrWhiteSpace() && dadosRecursoNecessidadeEspecial.Celular.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("É obrigatório informar TELEFONE ou CELULAR.");
                    }
                    else
                    {
                        if (!dadosRecursoNecessidadeEspecial.Telefone.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (!Validacao.ValidaTelefoneComDDD(dadosRecursoNecessidadeEspecial.Telefone))
                            {
                                mensagens.Add("O campo TELEFONE é inválido.!");
                            }
                        }

                        if (!dadosRecursoNecessidadeEspecial.Celular.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (!Validacao.ValidaCelularComDDD(dadosRecursoNecessidadeEspecial.Celular))
                            {
                                mensagens.Add("O campo CELULAR é inválido.!");
                            }
                        }
                    }

                    if (!dadosRecursoNecessidadeEspecial.Email.IsNullOrEmptyOrWhiteSpace() && !RN.Validacao.ValidaEmail(dadosRecursoNecessidadeEspecial.Email))
                    {
                        mensagens.Add("Campo E-MAIL inválido.");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o cpf já foi cadastrado anteriormente como recruso necessidade especial
                    if (this.PossuiOutroCpfCadastradoPor(contexto, dadosRecursoNecessidadeEspecial.Cpf, dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId))
                    {
                        mensagens.Add("Este CPF já foi utilizado!");
                    }

                    if (dadosRecursoNecessidadeEspecial.PessoaId > 0)
                    {
                        //Verifica se o cpf é reslmente o cpf da pessoa
                        if (rnPessoa.ObtemCpfPor(contexto, dadosRecursoNecessidadeEspecial.PessoaId) != dadosRecursoNecessidadeEspecial.Cpf)
                        {
                            mensagens.Add("Este CPF não pertence a esta PESSOA.");
                        }

                        //Verifica se a pessoa já foi cadastrada anteriormente como recurso necessidade especial
                        if (this.PossuiOutraPessoaCadastradaPor(contexto, dadosRecursoNecessidadeEspecial.PessoaId, dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId))
                        {
                            mensagens.Add("Esta PESSOA já foi utilizada.");
                        }
                    }

                    //Verifica se eh alteração
                    if (!cadastro)
                    {
                        //Busca associações do recruso
                        cuidadorAtivo = rnCuidadorAluno.EhCuidadorAtivoPor(contexto, dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId);
                        ledorAtivo = rnLedorAluno.EhLedorAtivoPor(contexto, dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId);
                        interpreteAtivo = rnInterpreteTurma.EhInterpreteAtivoPor(contexto, dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId);

                        //Verifica se possui associação ativa como cuidador e desmarcou a opção
                        if (!dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial.Contains((int)TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador)
                            && cuidadorAtivo)
                        {
                            mensagens.Add("A opção CUIDADOR não pode ser desmarcada pois possui uma associação ativa.");
                        }

                        //Verifica se possui associação ativa como ledor e desmarcou a opção
                        if (!dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial.Contains((int)TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor)
                            && ledorAtivo)
                        {
                            mensagens.Add("A opção LEDOR não pode ser desmarcada pois possui associação ativa.");
                        }

                        //Verifica se possui associação ativa como interprete e desmarcou a opção
                        if (!dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial.Contains((int)TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete)
                            && interpreteAtivo)
                        {
                            mensagens.Add("A opção INTÉRPRETE DE LIBRAS não pode ser desmarcada pois possui associação ativa.");
                        }

                        if (!dadosRecursoNecessidadeEspecial.Ativo)
                        {
                            //Verifica se possui associação ativa como cuidador
                            if (cuidadorAtivo)
                            {
                                mensagens.Add("Este recurso não pode ser desativado pois possui associação como CUIDADOR ativa.");
                            }

                            //Verifica se possui associação ativa como ledor
                            if (ledorAtivo)
                            {
                                mensagens.Add("Este recurso não pode ser desativado pois possui uma associação como LEDOR ativa.");
                            }

                            //Verifica se possui associação ativa como interprete
                            if (interpreteAtivo)
                            {
                                mensagens.Add("Este recurso não pode ser desativado pois possui uma associação como INTÉRPRETE DE LIBRAS ativa.");
                            }
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

        private bool PossuiOutraPessoaCadastradaPor(DataContext ctx, decimal pessoa, int recursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   NecessidadeEspecial.RECURSONECESSIDADEESPECIAL (NOLOCK) 
                                        WHERE  PESSOAID = @PESSOAID
                                                AND RECURSONECESSIDADEESPECIALID <> @RECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);
                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);

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

        private bool PossuiOutroCpfCadastradoPor(DataContext ctx, string cpf, int recursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                            FROM   NecessidadeEspecial.RECURSONECESSIDADEESPECIAL T (NOLOCK) 
                                                    INNER JOIN LY_PESSOA P(NOLOCK) 
                                                            ON T.PESSOAID = p.PESSOA 
                                            WHERE  CPF = @CPF 
                                                    AND T.RECURSONECESSIDADEESPECIALID <> @RECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);

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

        public void Insere(DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Entidades.LyPessoa pessoa = new LyPessoa();
            Entidades.RecursoNecessidadeEspecial recurso = new Entidades.RecursoNecessidadeEspecial();

            try
            {
                if (!dadosRecursoNecessidadeEspecial.Bloqueado)
                {
                    //Monta Entidade Pessoa
                    pessoa.Nome_compl = dadosRecursoNecessidadeEspecial.Nome;
                    pessoa.Cpf = dadosRecursoNecessidadeEspecial.Cpf;
                    pessoa.Cep = dadosRecursoNecessidadeEspecial.Cep;
                    pessoa.End_municipio = dadosRecursoNecessidadeEspecial.Municipio;
                    pessoa.Endereco = dadosRecursoNecessidadeEspecial.Endereco;
                    pessoa.End_num = dadosRecursoNecessidadeEspecial.Numero;
                    pessoa.End_compl = dadosRecursoNecessidadeEspecial.Complemento;
                    pessoa.Bairro = dadosRecursoNecessidadeEspecial.Bairro;
                    pessoa.E_mail = dadosRecursoNecessidadeEspecial.Email;
                    pessoa.Fone = dadosRecursoNecessidadeEspecial.Telefone;
                    pessoa.Celular = dadosRecursoNecessidadeEspecial.Celular;
                    pessoa.Sexo = dadosRecursoNecessidadeEspecial.Sexo;
                    pessoa.Est_civil = dadosRecursoNecessidadeEspecial.EstadoCivl;
                    pessoa.End_pais = dadosRecursoNecessidadeEspecial.PaisEndereco;
                    pessoa.Dt_nasc = dadosRecursoNecessidadeEspecial.DataNascimento;
                    pessoa.UsuarioId = dadosRecursoNecessidadeEspecial.UsuarioId;

                    if (dadosRecursoNecessidadeEspecial.PessoaId <= 0)
                    {
                        rnPessoa.InserePessoaExterna(ctx, pessoa);
                        dadosRecursoNecessidadeEspecial.PessoaId = pessoa.Pessoa;
                    }
                    else
                    {
                        pessoa.Pessoa = dadosRecursoNecessidadeEspecial.PessoaId;
                        rnPessoa.AtualizaPessoaExterna(ctx, pessoa);
                    }
                }

                //Monta Entidade RECURSONECESSIDADEESPECIAL
                recurso.UsuarioId = dadosRecursoNecessidadeEspecial.UsuarioId;
                recurso.PessoaId = dadosRecursoNecessidadeEspecial.PessoaId;
                recurso.Ativo = dadosRecursoNecessidadeEspecial.Ativo;

                //Insere RECURSONECESSIDADEESPECIAL
                this.Insere(ctx, recurso);
                dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId = recurso.RecursoNecessidadeEspecialId;

                foreach (int tipoRecurso in dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial)
                {
                    //Insere tipo recurso
                    this.InsereTipoRecursoNecessidadeEspecial(ctx, recurso.RecursoNecessidadeEspecialId, tipoRecurso, recurso.UsuarioId);
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

        private void Insere(DataContext ctx, Entidades.RecursoNecessidadeEspecial recurso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO NecessidadeEspecial.RECURSONECESSIDADEESPECIAL 
                                ( 
                                        PESSOAID, 
                                        ATIVO, 
                                        USUARIOID, 
                                        DATACADASTRO, 
                                        DATAALTERACAO 
                                ) 
                                VALUES 
                                ( 
		                                @PESSOAID, 
                                        @ATIVO,
		                                @USUARIOID, 
                                        @DATACADASTRO, 
                                        @DATAALTERACAO 
                                )
                                			
                                SELECT IDENT_CURRENT('NecessidadeEspecial.RECURSONECESSIDADEESPECIAL') ";

            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, recurso.PessoaId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, recurso.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, recurso.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            recurso.RecursoNecessidadeEspecialId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
        }

        private void InsereTipoRecursoNecessidadeEspecial(DataContext ctx, int recursoId, int tipoRecursoNecessidadeEspecial, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO NecessidadeEspecial.RECURSONECESSIDADEESPECIAL_TIPORECURSONECESSIDADEESPECIAL 
                                                    (TIPORECURSONECESSIDADEESPECIALID, 
                                                     RECURSONECESSIDADEESPECIALID, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        VALUES      (@TIPORECURSONECESSIDADEESPECIALID, 
                                                     @RECURSONECESSIDADEESPECIALID, 
                                                     @USUARIOID, 
                                                     @DATACADASTRO, 
                                                     @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoNecessidadeEspecial);
            contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public void Atualiza(DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.Entidades.LyPessoa pessoa = new LyPessoa();
            RN.Pessoa rnPessoa = new Pessoa();
            Entidades.RecursoNecessidadeEspecial recurso = new Entidades.RecursoNecessidadeEspecial();
            List<int> tiposRecursosAtuais = new List<int>();
            List<int> tiposRecursosRemovidos = new List<int>();
            List<int> tiposRecursosAdicionados = new List<int>();

            try
            {
                //Monta Entidade recurso
                recurso.UsuarioId = dadosRecursoNecessidadeEspecial.UsuarioId;
                recurso.PessoaId = dadosRecursoNecessidadeEspecial.PessoaId;
                recurso.Ativo = dadosRecursoNecessidadeEspecial.Ativo;
                recurso.RecursoNecessidadeEspecialId = dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId;

                //Atualiza recurso
                this.Atualiza(ctx, recurso.RecursoNecessidadeEspecialId, recurso.UsuarioId, recurso.Ativo);

                //Verifica se dados da pessoa podem ser editados
                if (!dadosRecursoNecessidadeEspecial.Bloqueado)
                {
                    //Atualiza dados pessoa
                    pessoa.Pessoa = dadosRecursoNecessidadeEspecial.PessoaId;
                    pessoa.Nome_compl = dadosRecursoNecessidadeEspecial.Nome;
                    pessoa.Cpf = dadosRecursoNecessidadeEspecial.Cpf;
                    pessoa.Cep = dadosRecursoNecessidadeEspecial.Cep;
                    pessoa.End_municipio = dadosRecursoNecessidadeEspecial.Municipio;
                    pessoa.Endereco = dadosRecursoNecessidadeEspecial.Endereco;
                    pessoa.End_num = dadosRecursoNecessidadeEspecial.Numero;
                    pessoa.End_compl = dadosRecursoNecessidadeEspecial.Complemento;
                    pessoa.Bairro = dadosRecursoNecessidadeEspecial.Bairro;
                    pessoa.E_mail = dadosRecursoNecessidadeEspecial.Email;
                    pessoa.Fone = dadosRecursoNecessidadeEspecial.Telefone;
                    pessoa.Celular = dadosRecursoNecessidadeEspecial.Celular;
                    pessoa.Est_civil = dadosRecursoNecessidadeEspecial.EstadoCivl;
                    pessoa.Sexo = dadosRecursoNecessidadeEspecial.Sexo;
                    pessoa.End_pais = dadosRecursoNecessidadeEspecial.PaisEndereco;
                    pessoa.Dt_nasc = dadosRecursoNecessidadeEspecial.DataNascimento;
                    pessoa.UsuarioId = dadosRecursoNecessidadeEspecial.UsuarioId;

                    rnPessoa.AtualizaPessoaExterna(ctx, pessoa);
                }

                //Busca tipos recursos atuais
                tiposRecursosAtuais = this.ListaTipoRecursoNecessidadeEspecialPor(ctx, recurso.RecursoNecessidadeEspecialId);

                //Verificar quais recursos foram removidos
                foreach (int tipoRecurso in tiposRecursosAtuais)
                {
                    if (!dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial.Contains(tipoRecurso))
                    {
                        //Remove tipo recurso
                        this.RemoveTipoRecursoNecessidadeEspecial(ctx, recurso.RecursoNecessidadeEspecialId, tipoRecurso);
                    }
                }

                //Verifica quais recursos foram adicionados
                foreach (int tipoRecurso in dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial)
                {
                    if (!tiposRecursosAtuais.Contains(tipoRecurso))
                    {
                        //Insere tipo recurso
                        this.InsereTipoRecursoNecessidadeEspecial(ctx, recurso.RecursoNecessidadeEspecialId, tipoRecurso, recurso.UsuarioId);
                    }
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

        private void RemoveTipoRecursoNecessidadeEspecial(DataContext ctx, int recursoId, int tipoRecursoNecessidadeEspecial)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE FROM NecessidadeEspecial.RECURSONECESSIDADEESPECIAL_TIPORECURSONECESSIDADEESPECIAL 
                                        WHERE  RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
                                            AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID  ";

            contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoNecessidadeEspecial);
            contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);

            ctx.ApplyModifications(contextQuery);
        }

        public bool EhRecursoAtivoPor(DataContext contexto, int recursoNecessidadeEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL (NOLOCK) 
                                    WHERE  RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
                                            AND ATIVO = 1 ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoNecessidadeEspecialId);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool EhRecursoPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM   NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL (NOLOCK) 
                                    WHERE  PESSOAID = @PESSOA  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public ValidacaoDados ValidaDesativacao(int recursoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            CuidadorAluno rnCuidadorAluno = new CuidadorAluno();
            LedorAluno rnLedorAluno = new LedorAluno();
            InterpreteTurma rnInterpreteTurma = new InterpreteTurma();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (recursoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se recurso está ativo
                    if (!this.EhRecursoAtivoPor(contexto, recursoId))
                    {
                        mensagens.Add("Este recurso já se encontra desativado.");
                    }
                    else
                    {
                        //Verifica se possui associação ativa como cuidador
                        if (rnCuidadorAluno.EhCuidadorAtivoPor(contexto, recursoId))
                        {
                            mensagens.Add("Este recurso não pode ser desativado pois possui uma associação como CUIDADOR ativa.");
                        }

                        //Verifica se possui associação ativa como ledor
                        if (rnLedorAluno.EhLedorAtivoPor(contexto, recursoId))
                        {
                            mensagens.Add("Este recurso não pode ser desativado pois possui uma associação como LEDOR ativa.");
                        }

                        //Verifica se possui associação ativa como interprete
                        if (rnInterpreteTurma.EhInterpreteAtivoPor(contexto, recursoId))
                        {
                            mensagens.Add("Este recurso não pode ser desativado pois possui uma associação como INTÉRPRETE DE LIBRAS ativa.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Desativa(int recursoId, string usuarioResponsavel)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Atualiza(ctx, recursoId, usuarioResponsavel, false);
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

        private void Atualiza(DataContext ctx, int recursoId, string usuarioResponsavel, bool ativo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE NecessidadeEspecial.RECURSONECESSIDADEESPECIAL
                                        SET ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID";

            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, ativo);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);

            ctx.ApplyModifications(contextQuery);
        }

        public bool PossuiTipoCadastradoPor(DataContext ctx, int recursoId, int tipoRecursoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM NECESSIDADEESPECIAL.RECURSONECESSIDADEESPECIAL_TIPORECURSONECESSIDADEESPECIAL  (NOLOCK)
                                        WHERE  RECURSONECESSIDADEESPECIALID = @RECURSONECESSIDADEESPECIALID 
                                               AND TIPORECURSONECESSIDADEESPECIALID = @TIPORECURSONECESSIDADEESPECIALID ";

                contextQuery.Parameters.Add("@RECURSONECESSIDADEESPECIALID", SqlDbType.Int, recursoId);
                contextQuery.Parameters.Add("@TIPORECURSONECESSIDADEESPECIALID", SqlDbType.Int, tipoRecursoId);

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
    }
}