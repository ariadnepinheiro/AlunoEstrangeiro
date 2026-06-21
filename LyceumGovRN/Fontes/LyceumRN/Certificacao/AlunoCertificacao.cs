using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Certificacao.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Certificacao;
using System.ComponentModel;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Validation;
using Techne.Lyceum.RN.Servicos;
using Seeduc.Infra.Extensions;
using System.Text.RegularExpressions;

namespace Techne.Lyceum.RN.Certificacao
{
    public class AlunoCertificacao
    {
        public DataTable Listar()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  ALUNOCERTIFICACAOID,
                                            UNIDADEENSINO,
                                             NOME,
                                             NOMEMAE,
                                             MAENAODECLARADA,
                                             NOMEPAI,
                                             PAINAODECLARADO,
                                             DATANASCIMENTO,
                                             MUNICIPIONASCIMENTO,
                                             NACIONALIDADE,
                                             CPF,
                                             RGNUMERO,
                                             RGEMISSOR,
                                             RGUF,
                                             NOMECURSO,
                                             ATOAUTORIZA,
                                             DATAAUTORIZA,
                                             TOTALHORASAULA,
                                             TOTALHORASRELOGIO,
                                             DATACONCLUSAO,
                                             NUMEROLIVRO,
                                             FOLHALIVRO,
                                             LIVRO, 
                                             OBSERVACAO,
                                             USUARIOID,
                                             DATACADASTRO,
                                             DATAALTERACAO
                                        FROM CertificacaoEscolar.ALUNOCERTIFICACAO (NOLOCK)
                                        ORDER BY NOME ";

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public void Salva(Entidades.AlunoCertificacao alunoCertificacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Certificacao.AlunoDocumento rnAlunoDocumento = new AlunoDocumento();

            try
            {
                //Verifica se já possui frequenciaTurma
                int id = this.ObtemIdPor(contexto, alunoCertificacao.CPF);

                if (id > 0)
                {
                    alunoCertificacao.AlunoCertificacaoId = id;
                    this.Atualiza(contexto, alunoCertificacao);
                }
                else
                {
                    this.Insere(contexto, alunoCertificacao);
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

        public void Insere(DataContext contexto, Entidades.AlunoCertificacao alunoCertificacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO CertificacaoEscolar.ALUNOCERTIFICACAO 
                                                        (
		                                                 NOME,
		                                                 NOMEMAE,
                                                         MAENAODECLARADA,
		                                                 NOMEPAI,
		                                                 PAINAODECLARADO,
		                                                 DATANASCIMENTO,
		                                                 MUNICIPIONASCIMENTO,
		                                                 NACIONALIDADE,
		                                                 CPF,
		                                                 RGNUMERO,
		                                                 RGEMISSOR,
		                                                 RGUF,		                                                 
		                                                 USUARIOID,
                                                         DATACADASTRO,
		                                                 DATAALTERACAO)
                                            VALUES      (
		                                                 @NOME,
		                                                 @NOMEMAE,
                                                         @MAENAODECLARADA,
		                                                 @NOMEPAI,
		                                                 @PAINAODECLARADO,
		                                                 @DATANASCIMENTO,
		                                                 @MUNICIPIONASCIMENTO,
		                                                 @NACIONALIDADE,
		                                                 @CPF,
		                                                 @RGNUMERO,
		                                                 @RGEMISSOR,
		                                                 @RGUF,		                                                
		                                                 @USUARIOID,
                                                         @DATACADASTRO,
		                                                 @DATAALTERACAO)       
                                    SELECT IDENT_CURRENT('CertificacaoEscolar.ALUNOCERTIFICACAO')                                                  
                                            ";


            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, alunoCertificacao.Nome);
            contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, alunoCertificacao.NomeMae);
            contextQuery.Parameters.Add("@MAENAODECLARADA", SqlDbType.Bit, alunoCertificacao.chkNaoDeclarMae);
            contextQuery.Parameters.Add("@NOMEPAI", SqlDbType.VarChar, alunoCertificacao.NomePai);
            contextQuery.Parameters.Add("@PAINAODECLARADO", SqlDbType.Bit, alunoCertificacao.chkNaoDeclarPai);
            contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, alunoCertificacao.DataNascimento);
            contextQuery.Parameters.Add("@MUNICIPIONASCIMENTO", SqlDbType.VarChar, alunoCertificacao.MunicipioNascimento);
            contextQuery.Parameters.Add("@NACIONALIDADE", SqlDbType.VarChar, alunoCertificacao.Nacionalidade);
            contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, alunoCertificacao.CPF);
            contextQuery.Parameters.Add("@RGNUMERO", SqlDbType.VarChar, alunoCertificacao.RgNumero);
            contextQuery.Parameters.Add("@RGEMISSOR", SqlDbType.VarChar, alunoCertificacao.RgEmissor);
            contextQuery.Parameters.Add("@RGUF", SqlDbType.VarChar, alunoCertificacao.RgUf);
            contextQuery.Parameters.Add("@USUARIOID", alunoCertificacao.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            alunoCertificacao.AlunoCertificacaoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public int ObtemIdPor(DataContext contexto, string cpf)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT ALUNOCERTIFICACAOID
                                          FROM CertificacaoEscolar.ALUNOCERTIFICACAO (NOLOCK)
                                            WHERE CPF = @CPF ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);


                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ALUNOCERTIFICACAOID"]);
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

        public Entidades.AlunoCertificacao ObtemDadosPor(string cpf)
        {
            Entidades.AlunoCertificacao alunoCertificacao = new Entidades.AlunoCertificacao();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT AC.ALUNOCERTIFICACAOID,
		                                       NOME,
		                                       NOMEMAE,
		                                       NOMEPAI,
		                                       MAENAODECLARADA,
		                                       PAINAODECLARADO,
		                                       DATANASCIMENTO,
		                                       MUNICIPIONASCIMENTO,
		                                       NACIONALIDADE,
		                                       CPF,
		                                       RGNUMERO,
		                                       RGEMISSOR,
		                                       RGUF	                                   
                                            FROM CertificacaoEscolar.ALUNOCERTIFICACAO AC(NOLOCK)
                                            WHERE CPF= @CPF
                                         ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    alunoCertificacao.AlunoCertificacaoId = Convert.ToInt32(reader["ALUNOCERTIFICACAOID"]);
                    alunoCertificacao.Nome = Convert.ToString(reader["NOME"]);
                    alunoCertificacao.NomeMae = Convert.ToString(reader["NOMEMAE"]);
                    alunoCertificacao.chkNaoDeclarMae = Convert.ToBoolean(reader["MAENAODECLARADA"]);
                    alunoCertificacao.NomePai = Convert.ToString(reader["NOMEPAI"]);
                    alunoCertificacao.chkNaoDeclarPai = Convert.ToBoolean(reader["PAINAODECLARADO"]);
                    alunoCertificacao.CPF = Convert.ToString(reader["CPF"]);
                    alunoCertificacao.MunicipioNascimento = Convert.ToString(reader["MUNICIPIONASCIMENTO"]);
                    alunoCertificacao.DataNascimento = reader["DATANASCIMENTO"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DATANASCIMENTO"]);
                    alunoCertificacao.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);
                    alunoCertificacao.RgNumero = Convert.ToString(reader["RGNUMERO"]);
                    alunoCertificacao.RgEmissor = Convert.ToString(reader["RGEMISSOR"]);
                    alunoCertificacao.RgUf = Convert.ToString(reader["RGUF"]);
                }

                return alunoCertificacao;
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

        public ValidacaoDados Validar(Entidades.AlunoCertificacao alunoCertificacao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Aluno rnAluno = new Aluno();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\s{2,}");
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (alunoCertificacao == null)
            {
                return validacaoDados;
            }

            //Tipo para documentos sempre RG
            if (alunoCertificacao.CPF == null)
            {
                mensagens.Add("Campo CPF é obrigatório.");
            }
            else
            {
                alunoCertificacao.CPF = alunoCertificacao.CPF.RetirarMascaraCPF();
                if (!Validacao.ValidaCpf(alunoCertificacao.CPF))
                {
                    mensagens.Add("O CPF informado é inválido.");
                }
            }

            if (alunoCertificacao.DataNascimento == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE NASCIMENTO é obrigatório.");
            }
            else
            {
                int idade = Utils.CalcularIdade(alunoCertificacao.DataNascimento);

                if (idade > 110)
                {
                    mensagens.Add("A idade não pode ser superior a 110 anos. Favor verificar a Data de Nascimento.");
                }
            }

            if (alunoCertificacao.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else
            {
                alunoCertificacao.Nome = alunoCertificacao.Nome.RetiraEspacosDuplos();

                /// Validaçoes de nome do aluno 
                if (alunoCertificacao.Nome.Length < 5)
                {
                    mensagens.Add("Campo NOME DO ALUNO deve conter pelo menos cinco letras!");
                }

                if (!string.IsNullOrEmpty(alunoCertificacao.Nome)
                    && !Validacao.SomenteLetras(alunoCertificacao.Nome))
                {
                    mensagens.Add("Campo NOME DO ALUNO não pode conter números.");
                }

                var palavras = alunoCertificacao.Nome.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(alunoCertificacao.Nome, 3);
                var nomeInvalido = Seeduc.Infra.Validation.TextValidator.HasForbiddenWords(alunoCertificacao.Nome, new Techne.Lyceum.RN.Servicos.PalavrasProibidasEmNomes());
                if (Validacao.contemNumeros(alunoCertificacao.Nome) == false)
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor, informar nome e sobrenome no campo NOME.");
                    }

                    if (contemRepeticao)
                    {
                        mensagens.Add("Campo NOME possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("Campo NOME possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }

                regex = new Regex(@"\s{2,}");
                string NomeCompl = regex.Replace(alunoCertificacao.Nome.Trim().ToUpper(), " ");
                var contemApostrofeRep = Validacao.substitueApostrofe(NomeCompl);
                if (contemApostrofeRep)
                {
                    mensagens.Add("Campo NOME DO ALUNO possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (alunoCertificacao.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }
            else
            {
                alunoCertificacao.NomeMae = alunoCertificacao.NomeMae.RetiraEspacosDuplos();

                //Validaçoes de nome da mae
                var palavras = alunoCertificacao.NomeMae.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(alunoCertificacao.NomeMae, 3);
                var nomeInvalido = Seeduc.Infra.Validation.TextValidator.HasForbiddenWords(alunoCertificacao.NomeMae, new Techne.Lyceum.RN.Servicos.PalavrasProibidasEmNomes());
                if (!Validacao.contemNumeros(alunoCertificacao.NomeMae))
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor, informar nome e sobrenome no campo NOME DA MÃE.");
                    }

                    if (contemRepeticao)
                    {
                        mensagens.Add("Campo NOME DA MÃE possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("Campo NOME DA MÃE possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }
                else if (!Validacao.SomenteLetras(alunoCertificacao.NomeMae))
                {
                    mensagens.Add("Campo NOME DA MÃE não pode conter números.");
                }

                string NomeMae = regex.Replace(alunoCertificacao.NomeMae.Trim().ToUpper(), " ");
                var contemApostrofeRepMae = Validacao.substitueApostrofe(NomeMae);
                if (contemApostrofeRepMae)
                {
                    mensagens.Add("Campo NOME DA MÃE possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (alunoCertificacao.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO PAI é obrigatório.");
            }
            else
            {
                alunoCertificacao.NomePai = alunoCertificacao.NomePai.RetiraEspacosDuplos();

                //Validaçoes de nome do pai
                var palavras = alunoCertificacao.NomePai.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(alunoCertificacao.NomePai, 3);
                var nomeInvalido = Seeduc.Infra.Validation.TextValidator.HasForbiddenWords(alunoCertificacao.NomePai, new Techne.Lyceum.RN.Servicos.PalavrasProibidasEmNomes());
                if (!Validacao.contemNumeros(alunoCertificacao.NomePai))
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor, informar nome e sobrenome no campo NOME DO PAI.");
                    }

                    if (contemRepeticao)
                    {
                        mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("Campo NOME DO PAI possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }
                else if (!Validacao.SomenteLetras(alunoCertificacao.NomePai))
                {
                    mensagens.Add("Campo NOME DO PAI não pode conter números.");
                }

                string NomePai = regex.Replace(alunoCertificacao.NomePai.Trim().ToUpper(), " ");
                var contemApostrofeRepPai = Validacao.substitueApostrofe(NomePai);
                if (contemApostrofeRepPai)
                {
                    mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (alunoCertificacao.RgNumero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO RG é obrigatório.");
            }
            else
            {
                var rg = Utils.RetirarMascara(alunoCertificacao.RgNumero);

                if (rg.Length < 5)
                {
                    mensagens.Add("O NÚMERO DO RG deve conter no mínimo cinco dígitos!");
                }

                if (alunoCertificacao.RgEmissor == "DETRAN" && alunoCertificacao.RgUf == "RJ")
                {
                    if (!Validacao.ValidaNumerosInteirosPositivos(alunoCertificacao.RgNumero))
                    {
                        mensagens.Add("O NÚMERO DO RG DETRAN deve conter só números inteiros.");
                    }
                }
            }

            if (alunoCertificacao.RgEmissor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ORGÃO EMISSOR DO RG é obrigatório.");
            }

            if (alunoCertificacao.RgUf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UF DO RG é obrigatório.");
            }

            if (alunoCertificacao.Nacionalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NACIONALIDADE é obrigatório.");
            }

            if (alunoCertificacao.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO DE NASCIMENTO é obrigatório.");
            }

            if (alunoCertificacao.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida se o cpf é de um aluno
                    if (rnAluno.PossuiAlunoPor(contexto, alunoCertificacao.CPF))
                    {
                        mensagens.Add("Para alunos já cadastrados no conexão educação deve ser utilizada a tela 'Certificação Escolar -> Certificado/Diploma Escolar'.");
                    }                    

                    //Valida Nome, mae e data de nascimento existente para outra pessoa
                    if (PossuiOutroCPFCertificacaoPor(contexto, alunoCertificacao.CPF, alunoCertificacao.AlunoCertificacaoId))
                    {
                        mensagens.Add("Existe outro cadastro com este CPF.");
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

        public bool PossuiOutroCPFCertificacaoPor(DataContext ctx, string cpf, int id)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool temCPF = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM CertificacaoEscolar.ALUNOCERTIFICACAO (NOLOCK)
                                      WHERE CPF = @CPF 
                                            and ALUNOCERTIFICACAOID <> @ALUNOCERTIFICACAOID ";

            contextQuery.Parameters.Add("@CPF", cpf);
            contextQuery.Parameters.Add("@ALUNOCERTIFICACAOID", id);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                temCPF = true;
            }

            return temCPF;
        }

        public void Atualiza(DataContext contexto, Entidades.AlunoCertificacao alunoCertificacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE CertificacaoEscolar.ALUNOCERTIFICACAO 
                                        SET  
		                                       NOME = @NOME, 
		                                       NOMEMAE = @NOMEMAE,
                                               MAENAODECLARADA = @MAENAODECLARADA, 
		                                       NOMEPAI = @NOMEPAI, 
		                                       PAINAODECLARADO = @PAINAODECLARADO, 
		                                       DATANASCIMENTO = @DATANASCIMENTO, 
		                                       MUNICIPIONASCIMENTO = @MUNICIPIONASCIMENTO, 
		                                       NACIONALIDADE = @NACIONALIDADE, 
		                                       CPF = @CPF, 
		                                       RGNUMERO = @RGNUMERO, 
		                                       RGEMISSOR = @RGEMISSOR, 
		                                       RGUF = @RGUF, 		                                      
		                                       USUARIOID = @USUARIOID, 
		                                       DATAALTERACAO = @DATAALTERACAO
                                        WHERE  ALUNOCERTIFICACAOID = @ALUNOCERTIFICACAOID  ";

            contextQuery.Parameters.Add("@ALUNOCERTIFICACAOID", alunoCertificacao.AlunoCertificacaoId);
            contextQuery.Parameters.Add("@NOME", alunoCertificacao.Nome);
            contextQuery.Parameters.Add("@NOMEMAE", alunoCertificacao.NomeMae);
            contextQuery.Parameters.Add("@MAENAODECLARADA", alunoCertificacao.chkNaoDeclarMae);
            contextQuery.Parameters.Add("@NOMEPAI", alunoCertificacao.NomePai);
            contextQuery.Parameters.Add("@PAINAODECLARADO", alunoCertificacao.chkNaoDeclarPai);
            contextQuery.Parameters.Add("DATANASCIMENTO", alunoCertificacao.DataNascimento);
            contextQuery.Parameters.Add("@MUNICIPIONASCIMENTO", alunoCertificacao.MunicipioNascimento);
            contextQuery.Parameters.Add("@NACIONALIDADE", alunoCertificacao.Nacionalidade);
            contextQuery.Parameters.Add("@CPF", alunoCertificacao.CPF);
            contextQuery.Parameters.Add("@RGNUMERO", alunoCertificacao.RgNumero);
            contextQuery.Parameters.Add("@RGEMISSOR", alunoCertificacao.RgEmissor);
            contextQuery.Parameters.Add("@RGUF", alunoCertificacao.RgUf);
            contextQuery.Parameters.Add("@USUARIOID", alunoCertificacao.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);

        }
    }
}