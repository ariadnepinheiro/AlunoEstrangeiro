using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using Seeduc.Infra.Data;
using Seeduc.Infra.Extensions;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Matriculas
{
    public class InscricaoAluno : RNBase
    {
        public enum TipoCandidato
        {
            //[StringValue("Não Alocado")]
            //NaoAlocado = 1, //Retirado, foi utilizado apenas em 2019
            [StringValue("Novo ou Inativo")]
            Novo = 2,
            [StringValue("Transferido")]
            Transferido = 3
        }

        public DadosCandidato ObtemDadosCandidatoPor(int inscricaoAlunoId)
        {
            OpcaoInscricao rnOpcaoInscricao = new OpcaoInscricao();
            DadosCandidato dados = new DadosCandidato();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                dados = this.ObtemDadosCandidatoPor(contexto, inscricaoAlunoId);

                //Verifica se a inscrição informou a matricula do irmao
                if (!dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
                {
                    //Busca dados do irmao
                    dados.DadosIrmao = this.ObtemDadosIrmaoPor(contexto, dados.IrmaoMatricula);
                }
                else
                {
                    dados.IrmaoMatricula = null;

                    //Verifica se a inscrição informou a inscrição do irmao
                    if (dados.IrmaoIdInscricao != null && dados.IrmaoIdInscricao > 0)
                    {
                        //Busca dados do irmao
                        dados.DadosIrmao = this.ObtemDadosIrmaoPor(contexto, Convert.ToInt32(dados.IrmaoIdInscricao));
                    }
                    else
                    {
                        dados.IrmaoIdInscricao = null;
                    }
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
                contexto.Dispose();
            }
        }

        public DadosCandidato ObtemDadosCandidatoPor(string nome, string nomeMae, DateTime? dataNascimento, int ano, int periodo, string NumInscricao, string nomePai, string cpfResponsavel, bool fonetizar)
        {
            OpcaoInscricao rnOpcaoInscricao = new OpcaoInscricao();
            DadosCandidato dados = new DadosCandidato();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                // false = Overload para retrocompatibilidade - VS 2008 não suporta parâmetros opcionais
                dados = this.ObtemDadosCandidatoPor(contexto, nome, nomeMae, dataNascimento, ano, periodo, NumInscricao, nomePai, cpfResponsavel, fonetizar);

                //Verifica se a inscrição informou a matricula do irmao
                if (!dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace()) 
                {
                    //Busca dados do irmao
                    dados.DadosIrmao = this.ObtemDadosIrmaoPor(contexto, dados.IrmaoMatricula);
                }
                else
                {
                    dados.IrmaoMatricula = null;

                    //Verifica se a inscrição informou a inscrição do irmao
                    if (dados.IrmaoIdInscricao != null && dados.IrmaoIdInscricao > 0)
                    {
                        //Busca dados do irmao
                        dados.DadosIrmao = this.ObtemDadosIrmaoPor(contexto, Convert.ToInt32(dados.IrmaoIdInscricao));
                    }
                    else
                    {
                        dados.IrmaoIdInscricao = null;
                    }
                }
                return dados;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();

                //Verifica se o erro é pq foram encontrados mais de uma opção 
                if (Convert.ToString(ex.Message) != "Para o filtro selecionado foi localizada mais de uma inscrição. Favor escolher outros campos no filtro." 
                    && !Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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

        public DadosCandidato ObtemDadosCandidatoPor(DateTime? dataNascimento, int ano, string NumInscricao, string cpf)
        {
            OpcaoInscricao rnOpcaoInscricao = new OpcaoInscricao();
            DadosCandidato dados = new DadosCandidato();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                dados = this.ObtemDadosCandidatoPor(contexto, dataNascimento, ano, NumInscricao, cpf);

                //Verifica se a inscrição informou a matricula do irmao
                if (!dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
                {
                    //Busca dados do irmao
                    dados.DadosIrmao = this.ObtemDadosIrmaoPor(contexto, dados.IrmaoMatricula);
                }
                else
                {
                    dados.IrmaoMatricula = null;

                    //Verifica se a inscrição informou a inscrição do irmao
                    if (dados.IrmaoIdInscricao != null && dados.IrmaoIdInscricao > 0)
                    {
                        //Busca dados do irmao
                        dados.DadosIrmao = this.ObtemDadosIrmaoPor(contexto, Convert.ToInt32(dados.IrmaoIdInscricao));
                    }
                    else
                    {
                        dados.IrmaoIdInscricao = null;
                    }
                }

                return dados;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();

                //Verifica se o erro é pq foram encontrados mais de uma opção 
                if (Convert.ToString(ex.Message) != "Para o filtro selecionado foi localizada mais de uma inscrição. Favor escolher outros campos no filtro."
                    && !Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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

        public IEnumerable<DadosCandidato> ObtemDadosCandidatoPor(string nome, string nomeMae, DateTime? dataNascimento)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ObtemDadosCandidatoPor(contexto, nome, nomeMae, dataNascimento);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();

                //Verifica se o erro é pq foram encontrados mais de uma opção 
                if (Convert.ToString(ex.Message) != "Para o filtro selecionado foi localizada mais de uma inscrição. Favor escolher outros campos no filtro."
                    && !Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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

        public DadosCandidato ObtemDadosCandidatoPor(DataContext contexto, int inscricaoAlunoId)
        {
            DadosCandidato dados = new DadosCandidato();
            SqlDataReader dataReader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PC.PRECADASTROALUNOID, 
                               PESSOAID, 
                               PC.NOME, 
                               EMAIL, 
                               DATANASCIMENTO, 
                               RESPONSAVEL, 
                               NOMEMAE, 
                               MAECPF, 
                               NOMEPAI, 
                               PAICPF, 
                               RESPONSAVELNOME, 
                               RESPONSAVELFONE, 
                               RESPONSAVELCPF, 
                               CPF, 
                               SEXO, 
                               ESTADOCIVIL, 
                               NACIONALIDADE, 
                               MUNICIPIONASCIMENTO, 
							   MUNASC.UF AS UFNASCIMENTO,
                               CELULAR, 
                               FIXOCELULAR, 
                               CEP, 
                               ENDERECO, 
                               NUMEROENDERECO, 
                               COMPLEMENTOENDERECO, 
                               BAIRRO, 
                               MUNICIPIOENDERECO, 
							   MUEND.UF AS UFENDERECO,
                               MUEND.NOME AS DESCRICAOMUNICIPIOENDERECO,
							   MUNASC.NOME AS DESCRICAOMUNICIPIONASCIMENTO,
                               MATRICULACERTIDAO, 
                               TERMOCERTIDAO, 
                               FOLHACERTIDAO, 
                               LIVROCERTIDAO, 
                               PC.NECESSIDADEESPECIALID, 
							   NE.DESCRICAO AS DESCRICAONECESSIDADEESPECIAL,
                               MODELOCERTIDAO, 
                               TIPOCERTIDAO, 
                               PC.USUARIOID, 
                               PC.DATACADASTRO, 
                               PC.DATAALTERACAO, 
                               I.INSCRICAOALUNOID, 
							   I.NUMEROINSCRICAO,
                               I.IRMAOMATRICULA,
                               I.IRMAOINSCRICAO,
                               II.NUMEROINSCRICAO AS IRMAONUMEROINSCRICAO,
							   I.ANO,
							   PC.RESPONSAVELNUMERORG,
							   PC.RESPOSANVELEMISSORRG,
							   PC.RESPOSANVELUFRG,
							   PC.NUMERORG,
							   PC.ORGAORG,
							   PC.UFRG,
							   ISNULL(I.REDEORIGEMINSCRICAO, PC.REDEORIGEM) AS REDEORIGEM
                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                               INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUNASC(NOLOCK) 
                                      ON PC.MUNICIPIONASCIMENTO = MUNASC.ID_MUNICIPIO  
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUEND(NOLOCK) 
                                      ON PC.MUNICIPIOENDERECO = MUEND.ID_MUNICIPIO  
                               LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL ne
									  ON NE.NECESSIDADEESPECIALID = PC.NECESSIDADEESPECIALID
                               LEFT JOIN MATRICULA.INSCRICAOALUNO II (NOLOCK)
                                      ON I.IRMAOINSCRICAO = II.INSCRICAOALUNOID
                        WHERE I.INSCRICAOALUNOID = @INSCRICAOALUNOID ";

                contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    dados.InscricaoAlunoId = Convert.ToInt32(dataReader["INSCRICAOALUNOID"]);
                    dados.PreCadastroAlunoId = Convert.ToInt32(dataReader["PRECADASTROALUNOID"]);
                    dados.Pessoa = dataReader["PESSOAID"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataReader["PESSOAID"]);
                    dados.Nome = Convert.ToString(dataReader["NOME"]);
                    dados.Email = Convert.ToString(dataReader["EMAIL"]);
                    dados.Celular = Convert.ToString(dataReader["CELULAR"]);
                    dados.FixoCelular = Convert.ToString(dataReader["FIXOCELULAR"]);
                    dados.DataNascimento = dataReader["DATANASCIMENTO"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["DATANASCIMENTO"]);
                    dados.Responsavel = Convert.ToString(dataReader["RESPONSAVEL"]);
                    dados.NomeMae = Convert.ToString(dataReader["NOMEMAE"]);
                    dados.MaeCpf = Convert.ToString(dataReader["MAECPF"]);
                    dados.NomePai = Convert.ToString(dataReader["NOMEPAI"]);
                    dados.PaiCpf = Convert.ToString(dataReader["PAICPF"]);
                    dados.ResponsavelNome = Convert.ToString(dataReader["RESPONSAVELNOME"]);
                    dados.ResponsavelFone = Convert.ToString(dataReader["RESPONSAVELFONE"]);
                    dados.ResponsavelCpf = Convert.ToString(dataReader["RESPONSAVELCPF"]);
                    dados.Cpf = Convert.ToString(dataReader["CPF"]);
                    dados.Sexo = Convert.ToString(dataReader["SEXO"]);
                    dados.EstadoCivil = Convert.ToString(dataReader["ESTADOCIVIL"]);
                    dados.RedeOrigem = Convert.ToString(dataReader["REDEORIGEM"]);
                    dados.Nacionalidade = Convert.ToString(dataReader["NACIONALIDADE"]);
                    dados.MunicipioNascimento = Convert.ToString(dataReader["MUNICIPIONASCIMENTO"]);
                    dados.UfNascimento = Convert.ToString(dataReader["UFNASCIMENTO"]);
                    dados.UfEndereco = Convert.ToString(dataReader["UFENDERECO"]);
                    dados.Cep = Convert.ToString(dataReader["CEP"]);
                    dados.Endereco = Convert.ToString(dataReader["ENDERECO"]);
                    dados.NumeroEndereco = Convert.ToString(dataReader["NUMEROENDERECO"]);
                    dados.ComplementoEndereco = Convert.ToString(dataReader["COMPLEMENTOENDERECO"]);
                    dados.MunicipioEndereco = Convert.ToString(dataReader["MUNICIPIOENDERECO"]);
                    dados.DescricaoMunicipioEndereco = Convert.ToString(dataReader["DESCRICAOMUNICIPIOENDERECO"]);
                    dados.DescricaoMunicipioNascimento = Convert.ToString(dataReader["DESCRICAOMUNICIPIONASCIMENTO"]);
                    dados.DescricaoNecessidadeEspecial = Convert.ToString(dataReader["DESCRICAONECESSIDADEESPECIAL"]);
                    dados.Bairro = Convert.ToString(dataReader["BAIRRO"]);
                    dados.ModeloCertidao = Convert.ToString(dataReader["MODELOCERTIDAO"]);
                    dados.TipoCertidao = Convert.ToString(dataReader["TIPOCERTIDAO"]);
                    dados.MatriculaCertidao = Convert.ToString(dataReader["MATRICULACERTIDAO"]);
                    dados.TermoCertidao = Convert.ToString(dataReader["TERMOCERTIDAO"]);
                    dados.FolhaCertidao = Convert.ToString(dataReader["FOLHACERTIDAO"]);
                    dados.LivroCertidao = Convert.ToString(dataReader["LIVROCERTIDAO"]);
                    dados.NecessidadeEspecialId = dataReader["NECESSIDADEESPECIALID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["NECESSIDADEESPECIALID"]);
                    dados.Responsavel = !dados.Responsavel.IsNullOrEmptyOrWhiteSpace() ? dados.Responsavel.Replace(";", string.Empty) : string.Empty;
                    dados.DeclaroAusenciaMae = dados.NomeMae == "NÃO DECLARADA" ? true : false;
                    dados.DeclaroAusenciaPai = dados.NomePai == "NÃO DECLARADO" ? true : false;
                    dados.ResponsavelNumeroRG = Convert.ToString(dataReader["RESPONSAVELNUMERORG"]);
                    dados.ResposanvelEmissorRG = Convert.ToString(dataReader["RESPOSANVELEMISSORRG"]);
                    dados.ResposanvelUFRG = Convert.ToString(dataReader["RESPOSANVELUFRG"]);
                    dados.NumeroRG = Convert.ToString(dataReader["NUMERORG"]);
                    dados.OrgaoRG = Convert.ToString(dataReader["ORGAORG"]);
                    dados.UFRG = Convert.ToString(dataReader["UFRG"]);
                    dados.Ano = Convert.ToInt32(dataReader["ANO"]);
                    dados.NumeroInscricao = dataReader["NUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["NUMEROINSCRICAO"]);
                    dados.IrmaoMatricula = dataReader["IRMAOMATRICULA"] == DBNull.Value ? (string)null : Convert.ToString(dataReader["IRMAOMATRICULA"]);
                    dados.IrmaoIdInscricao = dataReader["IRMAOINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["IRMAOINSCRICAO"]);
                    dados.IrmaoNumeroInscricao = dataReader["IRMAONUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["IRMAONUMEROINSCRICAO"]);
                }

                return dados;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        private DadosCandidato ObtemDadosCandidatoPor(DataContext contexto, string nome, string nomeMae, DateTime? dataNascimento, int ano, int periodo, string NumInscricao, string nomePai, string cpfResponsavel, bool fonetizar)
        {
            DadosCandidato dados = new DadosCandidato();
            ContextQuery contextQuery = new ContextQuery();
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DataTable dt = null;

            contextQuery.Command = @" SELECT PC.PRECADASTROALUNOID, 
                               PESSOAID, 
                               PC.NOME, 
                               EMAIL, 
                               DATANASCIMENTO, 
                               RESPONSAVEL, 
                               NOMEMAE, 
                               MAECPF, 
                               NOMEPAI, 
                               PAICPF, 
                               RESPONSAVELNOME, 
                               RESPONSAVELFONE, 
                               RESPONSAVELCPF, 
                               CPF, 
                               SEXO, 
                               ESTADOCIVIL, 
                               NACIONALIDADE, 
                               MUNICIPIONASCIMENTO, 
							   MUNASC.UF AS UFNASCIMENTO,
                               CELULAR, 
                               FIXOCELULAR, 
                               CEP, 
                               ENDERECO, 
                               NUMEROENDERECO, 
                               COMPLEMENTOENDERECO, 
                               BAIRRO, 
                               MUNICIPIOENDERECO, 
							   MUEND.UF AS UFENDERECO,
                               MUEND.NOME AS DESCRICAOMUNICIPIOENDERECO,
							   MUNASC.NOME AS DESCRICAOMUNICIPIONASCIMENTO,
                               MATRICULACERTIDAO, 
                               TERMOCERTIDAO, 
                               FOLHACERTIDAO, 
                               LIVROCERTIDAO, 
                               PC.NECESSIDADEESPECIALID, 
							   NE.DESCRICAO AS DESCRICAONECESSIDADEESPECIAL,
                               MODELOCERTIDAO, 
                               TIPOCERTIDAO, 
                               PC.USUARIOID, 
                               PC.DATACADASTRO, 
                               PC.DATAALTERACAO, 
                               I.INSCRICAOALUNOID, 
                               I.NUMEROINSCRICAO,
                               I.IRMAOMATRICULA,
                               I.IRMAOINSCRICAO,
                               II.NUMEROINSCRICAO AS IRMAONUMEROINSCRICAO,
							   I.ANO,							 
							   PC.RESPONSAVELNUMERORG,
							   PC.RESPOSANVELEMISSORRG,
							   PC.RESPOSANVELUFRG,
							   PC.NUMERORG,
							   PC.ORGAORG,
							   PC.UFRG,
							   ISNULL(I.REDEORIGEMINSCRICAO, PC.REDEORIGEM) as REDEORIGEM
                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                               INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID
                               LEFT JOIN MATRICULA.INSCRICAOALUNO II (NOLOCK)
                                      ON I.IRMAOINSCRICAO = II.INSCRICAOALUNOID
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUNASC(NOLOCK) 
                                      ON PC.MUNICIPIONASCIMENTO = MUNASC.ID_MUNICIPIO  
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUEND(NOLOCK) 
                                      ON PC.MUNICIPIOENDERECO = MUEND.ID_MUNICIPIO  
                               LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL ne
									  ON NE.NECESSIDADEESPECIALID = PC.NECESSIDADEESPECIALID
                       WHERE  I.ANO = @ANO ";

            if (periodo != -1)
            {
                contextQuery.Command += @" AND EXISTS (SELECT TOP 1 1 
									FROM TCE_CONTROLE_VAGA CV (NOLOCK)
										LEFT JOIN MATRICULA.OPCAOINSCRICAO O (NOLOCK) 
											ON CV.ID_CONTROLE_VAGA = O.CONTROLEVAGAID
										LEFT JOIN MATRICULA.OPCAOINSCRICAOHIST H (NOLOCK) 
											ON CV.ID_CONTROLE_VAGA = H.CONTROLEVAGAID
									WHERE CV.ANO = @ANO
										AND CV.PERIODO = @PERIODO
										AND (O.INSCRICAOALUNOID = I.INSCRICAOALUNOID OR H.INSCRICAOALUNOID = I.INSCRICAOALUNOID)) ";

                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            }
            if (!nome.IsNullOrEmptyOrWhiteSpace())
            {
                if (fonetizar)
                {
                    contextQuery.Command += @" AND PC.PRECADASTROALUNOID IN (SELECT REGISTROID FROM dbo.FN_BUSCAFONETICA('PRECADASTROALUNO','NOME',@NOME)) ";
                    contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
                }
                else
                {
                    contextQuery.Command += @" AND PC.NOME = @NOME ";
                    contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
                }
            }

            if (!nomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                if (fonetizar)
                {
                    contextQuery.Command += @" AND PC.PRECADASTROALUNOID IN (SELECT REGISTROID FROM dbo.FN_BUSCAFONETICA('PRECADASTROALUNO','NOMEMAE',@NOMEMAE)) ";
                    contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, nomeMae);
                }
                else
                {
                    contextQuery.Command += @" AND PC.NOMEMAE = @NOMEMAE ";
                    contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, nomeMae);
                }
            }
            if (!nomePai.IsNullOrEmptyOrWhiteSpace())
            {
                if (fonetizar)
                {
                    contextQuery.Command += @" AND PC.PRECADASTROALUNOID IN (SELECT REGISTROID FROM dbo.FN_BUSCAFONETICA('PRECADASTROALUNO','NOMEPAI',@NOMEPAI)) ";
                    contextQuery.Parameters.Add("@NOMEPAI", SqlDbType.VarChar, nomePai);
                }
                else
                {
                    contextQuery.Command += @" AND PC.NOMEPAI = @NOMEPAI ";
                    contextQuery.Parameters.Add("@NOMEPAI", SqlDbType.VarChar, nomePai);
                }
            }
            if (dataNascimento.HasValue)
            {
                contextQuery.Command += @" AND PC.DATANASCIMENTO = @DT_NASC ";
                contextQuery.Parameters.Add("@DT_NASC", SqlDbType.DateTime, dataNascimento);
            }
            if (!NumInscricao.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Command += @" AND I.NUMEROINSCRICAO = @NUMEROINSCRICAO ";
                contextQuery.Parameters.Add("@NUMEROINSCRICAO", SqlDbType.VarChar, NumInscricao);
            }
            if (!cpfResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Command += @" AND PC.RESPONSAVELCPF = @RESPONSAVELCPF ";
                contextQuery.Parameters.Add("@RESPONSAVELCPF", SqlDbType.VarChar, cpfResponsavel);
            }
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

            dt = contexto.GetDataTable(contextQuery);

            if (dt.Rows.Count == 1)
            {
                dados.InscricaoAlunoId = Convert.ToInt32(dt.Rows[0]["INSCRICAOALUNOID"]);
                dados.PreCadastroAlunoId = Convert.ToInt32(dt.Rows[0]["PRECADASTROALUNOID"]);
                dados.Pessoa = dt.Rows[0]["PESSOAID"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dt.Rows[0]["PESSOAID"]);
                dados.Nome = Convert.ToString(dt.Rows[0]["NOME"]);
                dados.Email = Convert.ToString(dt.Rows[0]["EMAIL"]);
                dados.Celular = Convert.ToString(dt.Rows[0]["CELULAR"]);
                dados.FixoCelular = Convert.ToString(dt.Rows[0]["FIXOCELULAR"]);
                dados.DataNascimento = dt.Rows[0]["DATANASCIMENTO"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dt.Rows[0]["DATANASCIMENTO"]);
                dados.Responsavel = Convert.ToString(dt.Rows[0]["RESPONSAVEL"]);
                dados.NomeMae = Convert.ToString(dt.Rows[0]["NOMEMAE"]);
                dados.MaeCpf = Convert.ToString(dt.Rows[0]["MAECPF"]);
                dados.NomePai = Convert.ToString(dt.Rows[0]["NOMEPAI"]);
                dados.PaiCpf = Convert.ToString(dt.Rows[0]["PAICPF"]);
                dados.ResponsavelNome = Convert.ToString(dt.Rows[0]["RESPONSAVELNOME"]);
                dados.ResponsavelFone = Convert.ToString(dt.Rows[0]["RESPONSAVELFONE"]);
                dados.ResponsavelCpf = Convert.ToString(dt.Rows[0]["RESPONSAVELCPF"]);
                dados.Cpf = Convert.ToString(dt.Rows[0]["CPF"]);
                dados.Sexo = Convert.ToString(dt.Rows[0]["SEXO"]);
                dados.EstadoCivil = Convert.ToString(dt.Rows[0]["ESTADOCIVIL"]);
                dados.RedeOrigem = Convert.ToString(dt.Rows[0]["REDEORIGEM"]);
                dados.Nacionalidade = Convert.ToString(dt.Rows[0]["NACIONALIDADE"]);
                dados.MunicipioNascimento = Convert.ToString(dt.Rows[0]["MUNICIPIONASCIMENTO"]);
                dados.UfNascimento = Convert.ToString(dt.Rows[0]["UFNASCIMENTO"]);
                dados.UfEndereco = Convert.ToString(dt.Rows[0]["UFENDERECO"]);
                dados.Cep = Convert.ToString(dt.Rows[0]["CEP"]);
                dados.Endereco = Convert.ToString(dt.Rows[0]["ENDERECO"]);
                dados.NumeroEndereco = Convert.ToString(dt.Rows[0]["NUMEROENDERECO"]);
                dados.ComplementoEndereco = Convert.ToString(dt.Rows[0]["COMPLEMENTOENDERECO"]);
                dados.MunicipioEndereco = Convert.ToString(dt.Rows[0]["MUNICIPIOENDERECO"]);
                dados.DescricaoMunicipioEndereco = Convert.ToString(dt.Rows[0]["DESCRICAOMUNICIPIOENDERECO"]);
                dados.DescricaoMunicipioNascimento = Convert.ToString(dt.Rows[0]["DESCRICAOMUNICIPIONASCIMENTO"]);
                dados.DescricaoNecessidadeEspecial = Convert.ToString(dt.Rows[0]["DESCRICAONECESSIDADEESPECIAL"]);
                dados.Bairro = Convert.ToString(dt.Rows[0]["BAIRRO"]);
                dados.ModeloCertidao = Convert.ToString(dt.Rows[0]["MODELOCERTIDAO"]);
                dados.TipoCertidao = Convert.ToString(dt.Rows[0]["TIPOCERTIDAO"]);
                dados.MatriculaCertidao = Convert.ToString(dt.Rows[0]["MATRICULACERTIDAO"]);
                dados.TermoCertidao = Convert.ToString(dt.Rows[0]["TERMOCERTIDAO"]);
                dados.FolhaCertidao = Convert.ToString(dt.Rows[0]["FOLHACERTIDAO"]);
                dados.LivroCertidao = Convert.ToString(dt.Rows[0]["LIVROCERTIDAO"]);
                dados.NecessidadeEspecialId = dt.Rows[0]["NECESSIDADEESPECIALID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["NECESSIDADEESPECIALID"]);
                dados.Responsavel = !dados.Responsavel.IsNullOrEmptyOrWhiteSpace() ? dados.Responsavel.Replace(";", string.Empty) : string.Empty;
                dados.DeclaroAusenciaMae = dados.NomeMae == "NÃO DECLARADA" ? true : false;
                dados.DeclaroAusenciaPai = dados.NomePai == "NÃO DECLARADO" ? true : false;
                dados.ResponsavelNumeroRG = Convert.ToString(dt.Rows[0]["RESPONSAVELNUMERORG"]);
                dados.ResposanvelEmissorRG = Convert.ToString(dt.Rows[0]["RESPOSANVELEMISSORRG"]);
                dados.ResposanvelUFRG = Convert.ToString(dt.Rows[0]["RESPOSANVELUFRG"]);
                dados.NumeroRG = Convert.ToString(dt.Rows[0]["NUMERORG"]);
                dados.OrgaoRG = Convert.ToString(dt.Rows[0]["ORGAORG"]);
                dados.UFRG = Convert.ToString(dt.Rows[0]["UFRG"]);
                dados.Ano = Convert.ToInt32(dt.Rows[0]["ANO"]);
                dados.NumeroInscricao = dt.Rows[0]["NUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["NUMEROINSCRICAO"]);
                dados.IrmaoMatricula = dt.Rows[0]["IRMAOMATRICULA"] == DBNull.Value ? (string)null : Convert.ToString(dt.Rows[0]["IRMAOMATRICULA"]);
                dados.IrmaoIdInscricao = dt.Rows[0]["IRMAOINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["IRMAOINSCRICAO"]);
                dados.IrmaoNumeroInscricao = dt.Rows[0]["IRMAONUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["IRMAONUMEROINSCRICAO"]);
            }
            else
            {
                if (dt.Rows.Count > 1)
                {
                    throw new Exception("Para o filtro selecionado foi localizada mais de uma inscrição. Favor escolher outros campos no filtro.");
                }
            }

            return dados;
        }

        private DadosCandidato ObtemDadosCandidatoPor(DataContext contexto, DateTime? dataNascimento, int ano, string NumInscricao, string cpf)
        {
            DadosCandidato dados = new DadosCandidato();
            ContextQuery contextQuery = new ContextQuery();
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DataTable dt = null;

            contextQuery.Command = @" SELECT PC.PRECADASTROALUNOID, 
                               PESSOAID, 
                               PC.NOME, 
                               EMAIL, 
                               DATANASCIMENTO, 
                               RESPONSAVEL, 
                               NOMEMAE, 
                               MAECPF, 
                               NOMEPAI, 
                               PAICPF, 
                               RESPONSAVELNOME, 
                               RESPONSAVELFONE, 
                               RESPONSAVELCPF, 
                               CPF, 
                               SEXO, 
                               ESTADOCIVIL, 
                               NACIONALIDADE, 
                               MUNICIPIONASCIMENTO, 
							   MUNASC.UF AS UFNASCIMENTO,
                               CELULAR, 
                               FIXOCELULAR, 
                               CEP, 
                               ENDERECO, 
                               NUMEROENDERECO, 
                               COMPLEMENTOENDERECO, 
                               BAIRRO, 
                               MUNICIPIOENDERECO, 
							   MUEND.UF AS UFENDERECO,
                               MUEND.NOME AS DESCRICAOMUNICIPIOENDERECO,
							   MUNASC.NOME AS DESCRICAOMUNICIPIONASCIMENTO,
                               MATRICULACERTIDAO, 
                               TERMOCERTIDAO, 
                               FOLHACERTIDAO, 
                               LIVROCERTIDAO, 
                               PC.NECESSIDADEESPECIALID, 
							   NE.DESCRICAO AS DESCRICAONECESSIDADEESPECIAL,
                               MODELOCERTIDAO, 
                               TIPOCERTIDAO, 
                               PC.USUARIOID, 
                               PC.DATACADASTRO, 
                               PC.DATAALTERACAO, 
                               I.INSCRICAOALUNOID, 
                               I.NUMEROINSCRICAO,
                               I.IRMAOMATRICULA,
                               I.IRMAOINSCRICAO,
                               II.NUMEROINSCRICAO AS IRMAONUMEROINSCRICAO,
							   I.ANO,
                               I.ALUNO,							 
							   PC.RESPONSAVELNUMERORG,
							   PC.RESPOSANVELEMISSORRG,
							   PC.RESPOSANVELUFRG,
							   PC.NUMERORG,
							   PC.ORGAORG,
							   PC.UFRG,
							   ISNULL(I.REDEORIGEMINSCRICAO, PC.REDEORIGEM) as REDEORIGEM
                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                               INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID
                               LEFT JOIN MATRICULA.INSCRICAOALUNO II (NOLOCK)
                                      ON I.IRMAOINSCRICAO = II.INSCRICAOALUNOID
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUNASC(NOLOCK) 
                                      ON PC.MUNICIPIONASCIMENTO = MUNASC.ID_MUNICIPIO  
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUEND(NOLOCK) 
                                      ON PC.MUNICIPIOENDERECO = MUEND.ID_MUNICIPIO  
                               LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL ne
									  ON NE.NECESSIDADEESPECIALID = PC.NECESSIDADEESPECIALID
                       WHERE  I.ANO = @ANO ";

            if (dataNascimento.HasValue)
            {
                contextQuery.Command += @" AND PC.DATANASCIMENTO = @DT_NASC ";
                contextQuery.Parameters.Add("@DT_NASC", SqlDbType.DateTime, dataNascimento);
            }
            if (!NumInscricao.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Command += @" AND I.NUMEROINSCRICAO = @NUMEROINSCRICAO ";
                contextQuery.Parameters.Add("@NUMEROINSCRICAO", SqlDbType.VarChar, NumInscricao);
            }
            if (!cpf.IsNullOrEmptyOrWhiteSpace())
            {
                contextQuery.Command += @" AND PC.CPF = @CPF ";
                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
            }
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

            dt = contexto.GetDataTable(contextQuery);

            if (dt.Rows.Count == 1)
            {
                dados.InscricaoAlunoId = Convert.ToInt32(dt.Rows[0]["INSCRICAOALUNOID"]);
                dados.PreCadastroAlunoId = Convert.ToInt32(dt.Rows[0]["PRECADASTROALUNOID"]);
                dados.Aluno = Convert.ToString(dt.Rows[0]["ALUNO"]);
                dados.Pessoa = dt.Rows[0]["PESSOAID"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dt.Rows[0]["PESSOAID"]);
                dados.Nome = Convert.ToString(dt.Rows[0]["NOME"]);
                dados.Email = Convert.ToString(dt.Rows[0]["EMAIL"]);
                dados.Celular = Convert.ToString(dt.Rows[0]["CELULAR"]);
                dados.FixoCelular = Convert.ToString(dt.Rows[0]["FIXOCELULAR"]);
                dados.DataNascimento = dt.Rows[0]["DATANASCIMENTO"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dt.Rows[0]["DATANASCIMENTO"]);
                dados.Responsavel = Convert.ToString(dt.Rows[0]["RESPONSAVEL"]);
                dados.NomeMae = Convert.ToString(dt.Rows[0]["NOMEMAE"]);
                dados.MaeCpf = Convert.ToString(dt.Rows[0]["MAECPF"]);
                dados.NomePai = Convert.ToString(dt.Rows[0]["NOMEPAI"]);
                dados.PaiCpf = Convert.ToString(dt.Rows[0]["PAICPF"]);
                dados.ResponsavelNome = Convert.ToString(dt.Rows[0]["RESPONSAVELNOME"]);
                dados.ResponsavelFone = Convert.ToString(dt.Rows[0]["RESPONSAVELFONE"]);
                dados.ResponsavelCpf = Convert.ToString(dt.Rows[0]["RESPONSAVELCPF"]);
                dados.Cpf = Convert.ToString(dt.Rows[0]["CPF"]);
                dados.Sexo = Convert.ToString(dt.Rows[0]["SEXO"]);
                dados.EstadoCivil = Convert.ToString(dt.Rows[0]["ESTADOCIVIL"]);
                dados.RedeOrigem = Convert.ToString(dt.Rows[0]["REDEORIGEM"]);
                dados.Nacionalidade = Convert.ToString(dt.Rows[0]["NACIONALIDADE"]);
                dados.MunicipioNascimento = Convert.ToString(dt.Rows[0]["MUNICIPIONASCIMENTO"]);
                dados.UfNascimento = Convert.ToString(dt.Rows[0]["UFNASCIMENTO"]);
                dados.UfEndereco = Convert.ToString(dt.Rows[0]["UFENDERECO"]);
                dados.Cep = Convert.ToString(dt.Rows[0]["CEP"]);
                dados.Endereco = Convert.ToString(dt.Rows[0]["ENDERECO"]);
                dados.NumeroEndereco = Convert.ToString(dt.Rows[0]["NUMEROENDERECO"]);
                dados.ComplementoEndereco = Convert.ToString(dt.Rows[0]["COMPLEMENTOENDERECO"]);
                dados.MunicipioEndereco = Convert.ToString(dt.Rows[0]["MUNICIPIOENDERECO"]);
                dados.DescricaoMunicipioEndereco = Convert.ToString(dt.Rows[0]["DESCRICAOMUNICIPIOENDERECO"]);
                dados.DescricaoMunicipioNascimento = Convert.ToString(dt.Rows[0]["DESCRICAOMUNICIPIONASCIMENTO"]);
                dados.DescricaoNecessidadeEspecial = Convert.ToString(dt.Rows[0]["DESCRICAONECESSIDADEESPECIAL"]);
                dados.Bairro = Convert.ToString(dt.Rows[0]["BAIRRO"]);
                dados.ModeloCertidao = Convert.ToString(dt.Rows[0]["MODELOCERTIDAO"]);
                dados.TipoCertidao = Convert.ToString(dt.Rows[0]["TIPOCERTIDAO"]);
                dados.MatriculaCertidao = Convert.ToString(dt.Rows[0]["MATRICULACERTIDAO"]);
                dados.TermoCertidao = Convert.ToString(dt.Rows[0]["TERMOCERTIDAO"]);
                dados.FolhaCertidao = Convert.ToString(dt.Rows[0]["FOLHACERTIDAO"]);
                dados.LivroCertidao = Convert.ToString(dt.Rows[0]["LIVROCERTIDAO"]);
                dados.NecessidadeEspecialId = dt.Rows[0]["NECESSIDADEESPECIALID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["NECESSIDADEESPECIALID"]);
                dados.Responsavel = !dados.Responsavel.IsNullOrEmptyOrWhiteSpace() ? dados.Responsavel.Replace(";", string.Empty) : string.Empty;
                dados.DeclaroAusenciaMae = dados.NomeMae == "NÃO DECLARADA" ? true : false;
                dados.DeclaroAusenciaPai = dados.NomePai == "NÃO DECLARADO" ? true : false;
                dados.ResponsavelNumeroRG = Convert.ToString(dt.Rows[0]["RESPONSAVELNUMERORG"]);
                dados.ResposanvelEmissorRG = Convert.ToString(dt.Rows[0]["RESPOSANVELEMISSORRG"]);
                dados.ResposanvelUFRG = Convert.ToString(dt.Rows[0]["RESPOSANVELUFRG"]);
                dados.NumeroRG = Convert.ToString(dt.Rows[0]["NUMERORG"]);
                dados.OrgaoRG = Convert.ToString(dt.Rows[0]["ORGAORG"]);
                dados.UFRG = Convert.ToString(dt.Rows[0]["UFRG"]);
                dados.Ano = Convert.ToInt32(dt.Rows[0]["ANO"]);
                dados.NumeroInscricao = dt.Rows[0]["NUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["NUMEROINSCRICAO"]);
                dados.IrmaoMatricula = dt.Rows[0]["IRMAOMATRICULA"] == DBNull.Value ? (string)null : Convert.ToString(dt.Rows[0]["IRMAOMATRICULA"]);
                dados.IrmaoIdInscricao = dt.Rows[0]["IRMAOINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["IRMAOINSCRICAO"]);
                dados.IrmaoNumeroInscricao = dt.Rows[0]["IRMAONUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dt.Rows[0]["IRMAONUMEROINSCRICAO"]);
            }
            else
            {
                if (dt.Rows.Count > 1)
                {
                    throw new Exception("Para o filtro selecionado foi localizada mais de uma inscrição. Favor escolher outros campos no filtro.");
                }
            }

            return dados;
        }

        private IList<DadosCandidato> ObtemDadosCandidatoPor(DataContext contexto, string nome, string nomeMae, DateTime? dataNascimento)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT PC.PRECADASTROALUNOID, 
                               PESSOAID, 
                               PC.NOME, 
                               EMAIL, 
                               DATANASCIMENTO, 
                               RESPONSAVEL, 
                               NOMEMAE, 
                               MAECPF, 
                               NOMEPAI, 
                               PAICPF, 
                               RESPONSAVELNOME, 
                               RESPONSAVELFONE, 
                               RESPONSAVELCPF, 
                               CPF, 
                               SEXO, 
                               ESTADOCIVIL, 
                               NACIONALIDADE, 
                               MUNICIPIONASCIMENTO, 
							   MUNASC.UF AS UFNASCIMENTO,
                               CELULAR, 
                               FIXOCELULAR, 
                               CEP, 
                               ENDERECO, 
                               NUMEROENDERECO, 
                               COMPLEMENTOENDERECO, 
                               BAIRRO, 
                               MUNICIPIOENDERECO, 
							   MUEND.UF AS UFENDERECO,
                               MUEND.NOME AS DESCRICAOMUNICIPIOENDERECO,
							   MUNASC.NOME AS DESCRICAOMUNICIPIONASCIMENTO,
                               MATRICULACERTIDAO, 
                               TERMOCERTIDAO, 
                               FOLHACERTIDAO, 
                               LIVROCERTIDAO, 
                               PC.NECESSIDADEESPECIALID, 
							   NE.DESCRICAO AS DESCRICAONECESSIDADEESPECIAL,
                               MODELOCERTIDAO, 
                               TIPOCERTIDAO, 
                               PC.USUARIOID, 
                               PC.DATACADASTRO, 
                               PC.DATAALTERACAO, 
                               I.INSCRICAOALUNOID, 
                               I.NUMEROINSCRICAO,
                               I.IRMAOMATRICULA,
                               I.IRMAOINSCRICAO,
                               II.NUMEROINSCRICAO AS IRMAONUMEROINSCRICAO,
							   I.ANO,
                               I.ALUNO,							 
							   PC.RESPONSAVELNUMERORG,
							   PC.RESPOSANVELEMISSORRG,
							   PC.RESPOSANVELUFRG,
							   PC.NUMERORG,
							   PC.ORGAORG,
							   PC.UFRG,
							   ISNULL(I.REDEORIGEMINSCRICAO, PC.REDEORIGEM) as REDEORIGEM
                        FROM   MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                               LEFT JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID
                               LEFT JOIN MATRICULA.INSCRICAOALUNO II (NOLOCK)
                                      ON I.IRMAOINSCRICAO = II.INSCRICAOALUNOID
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUNASC(NOLOCK) 
                                      ON PC.MUNICIPIONASCIMENTO = MUNASC.ID_MUNICIPIO  
							   LEFT JOIN HADES.dbo.TCE_MUNICIPIO MUEND(NOLOCK) 
                                      ON PC.MUNICIPIOENDERECO = MUEND.ID_MUNICIPIO  
                               LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL ne
									  ON NE.NECESSIDADEESPECIALID = PC.NECESSIDADEESPECIALID
                        WHERE
                        PC.NOME = @NOME
                        AND PC.NOMEMAE = @NOMEMAE
                        AND PC.DATANASCIMENTO = @DATANASCIMENTO
            ";

            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, nomeMae);
            contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, dataNascimento);

            dt = contexto.GetDataTable(contextQuery);

            IList<DadosCandidato> dados = new List<DadosCandidato>();
            foreach(DataRow row in dt.Rows)
            {
                var d = new DadosCandidato();

                d.InscricaoAlunoId = row["INSCRICAOALUNOID"] == DBNull.Value ? 0 : Convert.ToInt32(row["INSCRICAOALUNOID"]);
                d.PreCadastroAlunoId = Convert.ToInt32(row["PRECADASTROALUNOID"]);
                d.Aluno = Convert.ToString(row["ALUNO"]);
                d.Pessoa = row["PESSOAID"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row["PESSOAID"]);
                d.Nome = Convert.ToString(row["NOME"]);
                d.Email = Convert.ToString(row["EMAIL"]);
                d.Celular = Convert.ToString(row["CELULAR"]);
                d.FixoCelular = Convert.ToString(row["FIXOCELULAR"]);
                d.DataNascimento = row["DATANASCIMENTO"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["DATANASCIMENTO"]);
                d.Responsavel = Convert.ToString(row["RESPONSAVEL"]);
                d.NomeMae = Convert.ToString(row["NOMEMAE"]);
                d.MaeCpf = Convert.ToString(row["MAECPF"]);
                d.NomePai = Convert.ToString(row["NOMEPAI"]);
                d.PaiCpf = Convert.ToString(row["PAICPF"]);
                d.ResponsavelNome = Convert.ToString(row["RESPONSAVELNOME"]);
                d.ResponsavelFone = Convert.ToString(row["RESPONSAVELFONE"]);
                d.ResponsavelCpf = Convert.ToString(row["RESPONSAVELCPF"]);
                d.Cpf = Convert.ToString(row["CPF"]);
                d.Sexo = Convert.ToString(row["SEXO"]);
                d.EstadoCivil = Convert.ToString(row["ESTADOCIVIL"]);
                d.RedeOrigem = Convert.ToString(row["REDEORIGEM"]);
                d.Nacionalidade = Convert.ToString(row["NACIONALIDADE"]);
                d.MunicipioNascimento = Convert.ToString(row["MUNICIPIONASCIMENTO"]);
                d.UfNascimento = Convert.ToString(row["UFNASCIMENTO"]);
                d.UfEndereco = Convert.ToString(row["UFENDERECO"]);
                d.Cep = Convert.ToString(row["CEP"]);
                d.Endereco = Convert.ToString(row["ENDERECO"]);
                d.NumeroEndereco = Convert.ToString(row["NUMEROENDERECO"]);
                d.ComplementoEndereco = Convert.ToString(row["COMPLEMENTOENDERECO"]);
                d.MunicipioEndereco = Convert.ToString(row["MUNICIPIOENDERECO"]);
                d.DescricaoMunicipioEndereco = Convert.ToString(row["DESCRICAOMUNICIPIOENDERECO"]);
                d.DescricaoMunicipioNascimento = Convert.ToString(row["DESCRICAOMUNICIPIONASCIMENTO"]);
                d.DescricaoNecessidadeEspecial = Convert.ToString(row["DESCRICAONECESSIDADEESPECIAL"]);
                d.Bairro = Convert.ToString(row["BAIRRO"]);
                d.ModeloCertidao = Convert.ToString(row["MODELOCERTIDAO"]);
                d.TipoCertidao = Convert.ToString(row["TIPOCERTIDAO"]);
                d.MatriculaCertidao = Convert.ToString(row["MATRICULACERTIDAO"]);
                d.TermoCertidao = Convert.ToString(row["TERMOCERTIDAO"]);
                d.FolhaCertidao = Convert.ToString(row["FOLHACERTIDAO"]);
                d.LivroCertidao = Convert.ToString(row["LIVROCERTIDAO"]);
                d.NecessidadeEspecialId = row["NECESSIDADEESPECIALID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["NECESSIDADEESPECIALID"]);
                d.Responsavel = !d.Responsavel.IsNullOrEmptyOrWhiteSpace() ? d.Responsavel.Replace(";", string.Empty) : string.Empty;
                d.DeclaroAusenciaMae = d.NomeMae == "NÃO DECLARADA" ? true : false;
                d.DeclaroAusenciaPai = d.NomePai == "NÃO DECLARADO" ? true : false;
                d.ResponsavelNumeroRG = Convert.ToString(row["RESPONSAVELNUMERORG"]);
                d.ResposanvelEmissorRG = Convert.ToString(row["RESPOSANVELEMISSORRG"]);
                d.ResposanvelUFRG = Convert.ToString(row["RESPOSANVELUFRG"]);
                d.NumeroRG = Convert.ToString(row["NUMERORG"]);
                d.OrgaoRG = Convert.ToString(row["ORGAORG"]);
                d.UFRG = Convert.ToString(row["UFRG"]);
                d.Ano = row["ANO"] == DBNull.Value ? 0 : Convert.ToInt32(row["ANO"]);
                d.NumeroInscricao = row["NUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["NUMEROINSCRICAO"]);
                d.IrmaoMatricula = row["IRMAOMATRICULA"] == DBNull.Value ? (string)null : Convert.ToString(row["IRMAOMATRICULA"]);
                d.IrmaoIdInscricao = row["IRMAOINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["IRMAOINSCRICAO"]);
                d.IrmaoNumeroInscricao = row["IRMAONUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["IRMAONUMEROINSCRICAO"]);

                dados.Add(d);
            }

            return dados;
        }

        public DadosIrmao ObtemDadosIrmaoPor(DataContext contexto, string matriculaConexao)
        {
            DadosIrmao dados = new DadosIrmao();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT A.ALUNO,
                               P.NOME_COMPL, 
                               P.DT_NASC,
							   A.UNIDADE_ENSINO,
							   UE.NOME_COMP,
							   A.SERIE,
							   C.NOME AS NOMECURSO,
							   T.DESCRICAO AS NOMETURNO
                        FROM   LY_ALUNO A (NOLOCK)
                               INNER JOIN LY_PESSOA P (NOLOCK) ON P.PESSOA = A.PESSOA
							   INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) on A.UNIDADE_ENSINO = UE.UNIDADE_ENS
							   INNER JOIN LY_CURSO C (NOLOCK) ON A.CURSO = C.CURSO
							   INNER JOIN LY_TURNO T (NOLOCK) ON A.TURNO = T.TURNO
                        WHERE A.ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", matriculaConexao);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.MatriculaConexao = Convert.ToString(reader["ALUNO"]);
                    dados.NomeCompl = Convert.ToString(reader["NOME_COMPL"]);
                    dados.DataNascimento = reader["DT_NASC"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DT_NASC"]);
                    dados.CensoEscolaAtual = Convert.ToString(reader["UNIDADE_ENSINO"]);
                    dados.EscolaAtual = Convert.ToString(reader["NOME_COMP"]);
                    dados.SerieAtual = Convert.ToInt32(reader["SERIE"]);
                    dados.CursoDescricaoAtual = Convert.ToString(reader["NOMECURSO"]);
                    dados.TurnoDescricaoAtual = Convert.ToString(reader["NOMETURNO"]);
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

        public DadosIrmao ObtemDadosIrmaoPor(DataContext contexto, int inscricaoAlunoId)
        {
            DadosIrmao dados = new DadosIrmao();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT I.ALUNO,
                               PC.NOME, 
                               DATANASCIMENTO, 
                               CASE
                                    WHEN I.USUARIO IS NULL THEN PC.USUARIOID
                                    ELSE I.USUARIO
                               END USUARIOID, 
                               PC.DATACADASTRO, 
                               PC.DATAALTERACAO, 
                               I.INSCRICAOALUNOID, 
							   I.NUMEROINSCRICAO
                        FROM   MATRICULA.PRECADASTROALUNO PC (NOLOCK) 							   
                               INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID									
                               LEFT JOIN Matricula.PESSOAALUNO PA (NOLOCK) 	
							          ON PC.PESSOAID = PA.PESSOAID
							   LEFT JOIN LY_ALUNO A (NOLOCK) 	
							          ON ISNULL(I.ALUNO, PA.ALUNO) = A.ALUNO
							   LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NE (NOLOCK)
									  ON PC.NECESSIDADEESPECIALID = NE.NECESSIDADEESPECIALID
							   LEFT JOIN HADES.DBO.TCE_MUNICIPIO MUNASC(NOLOCK) 
                                      ON PC.MUNICIPIONASCIMENTO = MUNASC.ID_MUNICIPIO  
							   INNER JOIN HADES.DBO.TCE_MUNICIPIO MUEND(NOLOCK) 
                                      ON PC.MUNICIPIOENDERECO = MUEND.ID_MUNICIPIO							   
                        WHERE I.INSCRICAOALUNOID = @INSCRICAOALUNOID ";

                contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.MatriculaConexao = Convert.ToString(reader["ALUNO"]);
                    dados.InscricaoAlunoId = reader["INSCRICAOALUNOID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["INSCRICAOALUNOID"]);
                    dados.NumeroInscricao = reader["NUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["NUMEROINSCRICAO"]);
                    dados.NomeCompl = Convert.ToString(reader["NOME"]);
                    dados.DataNascimento = reader["DATANASCIMENTO"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DATANASCIMENTO"]);
                    dados.UsuarioResponsavel = Convert.ToString(reader["USUARIOID"]);
                    dados.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    dados.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
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

        public int ObtemPessoaPor(DataContext contexto, int inscricaoAlunoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT PC.PESSOAID
                                        FROM   MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                               INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                        WHERE  INSCRICAOALUNOID = @INSCRICAOALUNOID ";

                contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["PESSOAID"] != DBNull.Value ? Convert.ToInt32(reader["PESSOAID"]) : 0;
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

        public string ObtemAlunoPor(DataContext contexto, int inscricaoAlunoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT ALUNO
                                    FROM Matricula.INSCRICAOALUNO (NOLOCK)
                                    WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public bool PossuiInscricaoPor(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                        FROM   MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                               INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                        WHERE  PC.PESSOAID = @PESSOAID ";

            contextQuery.Parameters.Add("@PESSOAID", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiInscricaoPor(DataContext ctx, int inscricaoAlunoId)
        {
            var contextQuery = new ContextQuery();
            contextQuery.Command = @" select case when exists (select top 1 1 from Matricula.INSCRICAOALUNO i (nolock) where i.INSCRICAOALUNOID = @INSCRICAOALUNOID) then 1 else 0 end ";
            contextQuery.Parameters.Add("@INSCRICAOALUNOID", inscricaoAlunoId);
            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool PossuiInscricaoPor(DataContext ctx, string nome, string nomeMae, DateTime dataNascimento, int inscricaoAlunoId, int ano)
        {
            var contextQuery = new ContextQuery();
            contextQuery.Command = @" 
            select case when exists (
                select top 1 1 
	            from Matricula.PRECADASTROALUNO pc (nolock)
	            inner join Matricula.INSCRICAOALUNO i (nolock) on i.PRECADASTROALUNOID = pc.PRECADASTROALUNOID
	            where 
	            trim(pc.NOME) = trim(@NOME)
	            and trim(pc.NOMEMAE) = trim(@NOMEMAE)
	            and pc.DATANASCIMENTO = @DATANASCIMENTO
	            and i.ANO = @ANO
	            and pc.PRECADASTROALUNOID <> (select PRECADASTROALUNOID from Matricula.INSCRICAOALUNO where INSCRICAOALUNOID = @INSCRICAOALUNOID)
            ) then 1 else 0 end 
            ";
            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, nomeMae);
            contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, dataNascimento);
            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }

        public bool EhOpcaoAlocadaFase1Por(DataContext ctx, int inscricaoAlunoId, int opcaoAlocada)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(1)
                                        FROM   MATRICULA.INSCRICAOALUNO (NOLOCK) 
                                        WHERE  INSCRICAOALUNOID = @INSCRICAOALUNOID
											   AND OPCAOINSCRICAOIDALOCADA = @OPCAOINSCRICAOIDALOCADA ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);
            contextQuery.Parameters.Add("@OPCAOINSCRICAOIDALOCADA", SqlDbType.Int, opcaoAlocada);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void ConfirmaFase1(DataContext contexto, int inscricaoAlunoId, bool confirma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Matricula.INSCRICAOALUNO 
                                           SET CONFIRMADOFASE1 = @CONFIRMADOFASE1,
                                               DATAALTERACAO = @DATAALTERACAO
                                      WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID ";

            contextQuery.Parameters.Add("@CONFIRMADOFASE1", SqlDbType.Bit, confirma);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void ConfirmaFase2(DataContext contexto, int inscricaoAlunoId, bool confirma)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Matricula.INSCRICAOALUNO 
                                           SET CONFIRMADOFASE2 = @CONFIRMADOFASE2,
                                               DATAALTERACAO = @DATAALTERACAO
                                      WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID ";

            contextQuery.Parameters.Add("@CONFIRMADOFASE2", SqlDbType.Bit, confirma);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void AlocaFase2(DataContext contexto, int inscricaoAlunoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Matricula.INSCRICAOALUNO 
                                           SET ALOCADOFASE2 = @ALOCADOFASE2,
                                               DATAALTERACAO = @DATAALTERACAO
                                      WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID ";

            contextQuery.Parameters.Add("@ALOCADOFASE2", SqlDbType.Bit, true);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaOpcaoConvocadaFase1(DataContext contexto, int inscricaoAlunoId, int opcaoAlocada)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Matricula.INSCRICAOALUNO 
                                           SET OPCAOINSCRICAOIDALOCADA = @OPCAOALOCADA
                                      WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);
            contextQuery.Parameters.Add("@OPCAOALOCADA", SqlDbType.Int, opcaoAlocada);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaAlteraDadosInscricao(int inscricaoAlunoId, int ano, string nome, string nomeMae, DateTime dataNascimento, string redeOrigemInscricao, int necessidadeEspecialId, string aluno)
        {
            var rnPreCadastroAluno = new RN.Matriculas.PreCadastroAluno();
            var validacaoDados = new ValidacaoDados();
            var mensagens = new List<string>();

            //validação do campo NOME
            if (nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO CANDIDATO é obrigatório.");
            }
            else
            {
                /// Validaçoes de nome do CANDIDATO 
                if (nome.Length < 5)
                {
                    mensagens.Add("Campo NOME DO CANDIDATO deve conter pelo menos cinco letras.");
                }

                if (!nome.IsNullOrEmptyOrWhiteSpace()
                    && !Utils.SomenteLetras(nome))
                {
                    mensagens.Add("Campo NOME DO CANDIDATO não pode conter números.");
                }

                var palavras = nome.CountWords();
                if (Utils.ContemNumeros(nome) == false)
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor, informar nome e sobrenome no campo NOME.");
                    }
                }
            }

            //validação do campo NOME DA MÃE
            if (nomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }
            else
            {
                /// Validaçoes de nome do CANDIDATO 
                if (nomeMae.Length < 5)
                {
                    mensagens.Add("Campo NOME DA MÃE deve conter pelo menos cinco letras.");
                }

                if (!nomeMae.IsNullOrEmptyOrWhiteSpace()
                    && !Utils.SomenteLetras(nomeMae))
                {
                    mensagens.Add("Campo NOME DA MÃE não pode conter números.");
                }

                var palavras = nomeMae.CountWords();
                if (Utils.ContemNumeros(nomeMae) == false)
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor, informar nome e sobrenome no campo NOME DA MÃE.");
                    }
                }
            }

            //validação do campo DATA DE NASCIMENTO
            if (dataNascimento < SqlDateTime.MinValue.Value)
                mensagens.Add("A DATA DE NASCIMENTO informada é inválida ou não está preenchida");
            if (dataNascimento >= DateTime.Now)
                mensagens.Add("A DATA DE NASCIMENTO informada é inválida.");

            //validação do campo REDE DE ORIGEM
            if (!new string[] { "Estadual", "Federal", "Municipal", "Particular", "Afastado" }.Contains(redeOrigemInscricao))
                mensagens.Add("Por favor informar uma REDE DE ORIGEM válida.");

            //se for informada matrícula do aluno, então a rede de origem tem que ser ESTADUAL
            if (!aluno.IsNullOrEmptyOrWhiteSpace() && redeOrigemInscricao != "Estadual")
                mensagens.Add("Para alunos com matrícula ativa informada, a rede de origem precisa ser ESTADUAL.");

            if (mensagens.Count == 0)
            {
                DataContext contexto = null;

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //verificar se existe algum pré-cadastro que atenda o conjunto NOME, NOMEMAE e DATANASCIMENTO que tenha inscrição com o ANO igual ao informado na tela
                    if (PossuiInscricaoPor(contexto, nome, nomeMae, dataNascimento, inscricaoAlunoId, ano))
                        mensagens.Add("Já existe um cadastro com o mesmo conjunto de dados NOME, NOME DA MÃE, DATA DE NASCIMENTO e ANO informado.");

                    //Calcula idade
                    //Verifica se a data atual é maior que a data base da matricula  
                    var rnAgenda = new RN.Matriculas.Agenda();
                    var dataBaseMatricula = rnAgenda.RetornaDataBaseMatricula(contexto, ano);
                    var idade = 0;

                    if (dataBaseMatricula == DateTime.MinValue)
                        mensagens.Add("Não foi possível obter a DATA BASE da matrícula por conta de não ter fases abertas no ano selecionado.");
                    else
                    {
                        if (dataBaseMatricula > DateTime.Now)
                        {
                            idade = Utils.CalcularIdadePorData(Convert.ToDateTime(dataNascimento), dataBaseMatricula);
                        }
                        else
                        {
                            idade = Utils.CalcularIdade(Convert.ToDateTime(dataNascimento));
                        }

                        //verificar se existe restrição de idade por série/etapa de ensino
                        var rnOpcaoInscricao = new RN.Matriculas.OpcaoInscricao();
                        var rnRestricaoIdadeSerie = new RN.Matriculas.RestricaoIdadeSerie();
                        var opcoes = rnOpcaoInscricao.ListaPor(inscricaoAlunoId);
                        foreach (DataRow o in opcoes.Rows)
                        {
                            var controleVagaId = Convert.ToInt32(o["CONTROLEVAGAID"]);
                            var descricaoModalidade = o["DESCRICAOMODALIDADE"];
                            var eRestricaoIdadeSerie = rnRestricaoIdadeSerie.ObtemPor(contexto, controleVagaId);

                            //Verifica se o candidato possui necessidade especial
                            if (necessidadeEspecialId == 30) //"Não possui."
                            {
                                //Para candidatos sem necessidades Especiais Verificar restrição de idade minima e maxima
                                if (idade < eRestricaoIdadeSerie.IdadeMinima || idade > eRestricaoIdadeSerie.IdadeMaxima)
                                    mensagens.Add(string.Format("Para curso {0} é permitido candidatos entre {1} e {2} anos.", descricaoModalidade, eRestricaoIdadeSerie.IdadeMinima, eRestricaoIdadeSerie.IdadeMaxima));
                            }
                            else
                            {
                                //Para candidatos com necessidades Especiais Verificar restrição de idade minima
                                if (idade < eRestricaoIdadeSerie.IdadeMinima)
                                {
                                    mensagens.Add(string.Format("Para o curso {0} não é permitido cadastrar candidatos com necessidade especial com menos de {1} anos.", descricaoModalidade, eRestricaoIdadeSerie.IdadeMinima));
                                }
                            }
                        }

                        //verificar se a matrícula informada eh ativa
                        if (!aluno.IsNullOrEmptyOrWhiteSpace())
                        {
                            var rnAluno = new RN.Aluno();
                            if (!rnAluno.EhAlunoAtivoPor(aluno))
                            {
                                mensagens.Add("A matrícula informada não está ATIVA.");
                            }
                            else
                            {
                                //verificar se a matrícula informada bate com os dados NOME, NOME DA MÃE e DATA DE NASCIMENTO
                                if (!aluno.IsNullOrEmptyOrWhiteSpace())
                                {
                                    var pessoa = rnPreCadastroAluno.ObtemPessoaPor(contexto, aluno);

                                    if (pessoa.Nome_compl == null)
                                        mensagens.Add("A matrícula especificada não existe.");
                                    else
                                    {
                                        if (pessoa.Nome_compl.ToUpper().RetiraEspacosDuplos().RetiraCaracteresEspeciais() != nome.ToUpper().RetiraEspacosDuplos().RetiraCaracteresEspeciais())
                                            mensagens.Add("Nome e/ou matrícula incorreto(s).");

                                        if (pessoa.NomeMae.ToUpper().RetiraEspacosDuplos().RetiraCaracteresEspeciais() != nomeMae.ToUpper().RetiraEspacosDuplos().RetiraCaracteresEspeciais())
                                            mensagens.Add("Nome da mãe e/ou matrícula incorreto(s).");

                                        if ((pessoa.Dt_nasc ?? DateTime.MinValue).Date != dataNascimento.Date)
                                            mensagens.Add("Data de nascimento e/ou matrícula incorreto(s).");
                                    }
                                }
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void AlteraDadosInscricao(int inscricaoAlunoId, string nome, string nomeMae, DateTime dataNascimento, string redeOrigemInscricao, string aluno, string usuarioId)
        {
            DataContext contexto = null;
            PreCadastroAluno rnPreCadastroAluno = new RN.Matriculas.PreCadastroAluno();
            PreCadastroAlunoFoto rnPreCadastroAlunoFoto = new RN.Matriculas.PreCadastroAlunoFoto();

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                rnPreCadastroAluno.MoveInscricoesDePreCadastroAntigoParaPreCadastroNovo(contexto, inscricaoAlunoId, nome, nomeMae, dataNascimento, usuarioId);
                rnPreCadastroAlunoFoto.RemovePreCadastroFotoSemInscricao(contexto, nome, nomeMae, dataNascimento);
                rnPreCadastroAluno.RemovePreCadastroSemInscricao(contexto, nome, nomeMae, dataNascimento);
                AlteraDadosInscricao(contexto, inscricaoAlunoId, nome, nomeMae, dataNascimento, redeOrigemInscricao, aluno, usuarioId);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }        
        
        public void AlteraDadosInscricao(DataContext contexto, int inscricaoAlunoId, string nome, string nomeMae, DateTime dataNascimento, string redeOrigemInscricao, string aluno, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"

            declare @PRECADASTROALUNOID int = (select PRECADASTROALUNOID from Matricula.INSCRICAOALUNO I (nolock) where I.INSCRICAOALUNOID = @INSCRICAOID)

            UPDATE MATRICULA.PRECADASTROALUNO
            SET NOME = @NOME_COMPL, 
                DATANASCIMENTO = @DT_NASC, 
                NOMEMAE = @NOME_MAE,
                USUARIOID = @USUARIOID, 
                DATAALTERACAO = @DATAALTERACAO	
            WHERE PRECADASTROALUNOID = @PRECADASTROALUNOID

            update Matricula.INSCRICAOALUNO
            set REDEORIGEMINSCRICAO = @REDEORIGEMINSCRICAO,
                ALUNO = @ALUNO,
                DATAALTERACAO = @DATAALTERACAO
            WHERE INSCRICAOALUNOID = @INSCRICAOID

            ";

            contextQuery.Parameters.Add("@INSCRICAOID", SqlDbType.Int, inscricaoAlunoId);
            contextQuery.Parameters.Add("@NOME_COMPL", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOME_MAE", SqlDbType.VarChar, nomeMae);
            contextQuery.Parameters.Add("@DT_NASC", SqlDbType.DateTime, dataNascimento);
            contextQuery.Parameters.Add("@REDEORIGEMINSCRICAO", SqlDbType.VarChar, redeOrigemInscricao);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, (object)aluno ?? DBNull.Value);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void MoveInscricaoParaNovoPreCadastro(DataContext contexto, int inscricaoAlunoId, int novoPreCadastroAlunoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"
            update Matricula.INSCRICAOALUNO
            set PRECADASTROALUNOID = @NOVOPRECADASTROALUNOID
                ,USUARIOID = @USUARIOID
                ,DATAALTERACAO = @DATAALTERACAO
            WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID
            ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);
            contextQuery.Parameters.Add("@NOVOPRECADASROALUNOID", SqlDbType.Int, novoPreCadastroAlunoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
