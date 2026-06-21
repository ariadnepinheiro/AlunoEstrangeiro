using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Seeduc.Infra.Data;
using Seeduc.Infra.Extensions;
using Seeduc.Infra.Validation;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Servicos;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class Aluno : RNBase
    {
        [Serializable]
        public class DadosAluno
        {
            public string Aluno { get; set; }

            public string Nome_compl { get; set; }

            public string NomeMae { get; set; }

            public DateTime? DataNascimento { get; set; }

            public string Cpf { get; set; }

            public string Curso { get; set; }

            public string NomeCurso { get; set; }

            public string Turno { get; set; }

            public string Serie { get; set; }

            public string UnidadeResponsavel { get; set; }

            public string NomeUnidade { get; set; }

            public string MnemonicoUnidadeResponsavel { get; set; }

            public string Grade { get; set; }

            public string Curriculo { get; set; }

            public string Faculdade { get; set; }

            public string Nucleo { get; set; }

            public string Municipio { get; set; }

            public string Grade_id { get; set; }

            public string SitAluno { get; set; }
        }

        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM LY_ALUNO
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public List<string> ListaMatriculasPor(DataContext ctx, int pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> retorno = new List<string>();

            try
            {
                contextQuery.Command = @" SELECT ALUNO
                                            FROM LY_ALUNO (NOLOCK)
                                            WHERE PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno.Add(Convert.ToString(reader["ALUNO"]));
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

        public int RetornaQuantiadeAlunosPor(DataContext contexto, decimal pessoa)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QUANTIDADE 
                                            FROM LY_ALUNO 
                                            WHERE PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int RetornaQuantiadeAlunosAtivosPor(DataContext contexto, string censo)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QUANTIDADE 
                                          FROM LY_ALUNO 
                                          WHERE SIT_ALUNO = 'Ativo'
										        AND UNIDADE_ENSINO = @CENSO ";

                contextQuery.Parameters.Add("@CENSO", censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static SimpleRow ConsultarDadosAluno(string aluno)
        {
            return Consultar(@"Select top 1 a.curso, c.nome nomecurso, a.turno, t.descricao nometurno, a.curriculo, a.serie, 
                                s.descricao nomeserie, a.unidade_ensino, ue.nome_comp nomeunidade
                                from LY_ALUNO a
                                inner join LY_CURSO c on c.CURSO = a.CURSO
                                inner join LY_TURNO t on t.TURNO = a.TURNO
                                inner join LY_SERIE s on s.SERIE = a.SERIE and s.curso = a.curso and s.turno = a.turno and s.curriculo = a.curriculo
                                inner join LY_UNIDADE_ENSINO ue on ue.UNIDADE_ENS = a.UNIDADE_ENSINO
                                where 
                                ALUNO = ?", aluno).Rows[0];
        }

        public RN.Aluno.DadosAluno ObtemDadosAluno(string aluno)
        {
            RN.Aluno.DadosAluno dadosAluno = new RN.Aluno.DadosAluno();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                dadosAluno = this.ObtemDadosAluno(ctx, aluno);
                return dadosAluno;
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

        public RN.Aluno.DadosAluno ObtemDadosAluno(DataContext ctx, string aluno)
        {
            RN.Aluno.DadosAluno dadosAluno = new RN.Aluno.DadosAluno();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  A.ALUNO ,
                                    A.CURSO ,
                                    A.TURNO ,
                                    A.CURRICULO ,
                                    A.SERIE ,
                                    A.UNIDADE_ENSINO,
                                    A.SIT_ALUNO,
                                    P.NOME_COMPL,
									P.NOME_MAE,
									P.DT_NASC,
                                    P.CPF
                            FROM    LY_ALUNO A
                            INNER JOIN LY_PESSOA P ON P.PESSOA=A.PESSOA
                            WHERE   ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosAluno.Aluno = Convert.ToString(reader["ALUNO"]);
                    dadosAluno.UnidadeResponsavel = Convert.ToString(reader["UNIDADE_ENSINO"]);
                    dadosAluno.Curso = Convert.ToString(reader["CURSO"]);
                    dadosAluno.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    dadosAluno.Turno = Convert.ToString(reader["TURNO"]);
                    dadosAluno.Serie = Convert.ToString(reader["SERIE"]);
                    dadosAluno.SitAluno = Convert.ToString(reader["SIT_ALUNO"]);
                    dadosAluno.Nome_compl = Convert.ToString(reader["NOME_COMPL"]);
                    dadosAluno.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dadosAluno.Cpf = Convert.ToString(reader["CPF"]);

                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dadosAluno.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                }

                return dadosAluno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public RN.DTOs.DadosAlunoPessoa ObtemDadosAlunoPessoaPor(string aluno)
        {
            RN.DTOs.DadosAlunoPessoa dadosPessoa = new DadosAlunoPessoa();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT A.ALUNO, 
                                               P.NOME_COMPL, 
                                               P.DT_NASC, 
                                               P.NOME_MAE, 
                                               A.UNIDADE_ENSINO, 
                                               A.SIT_ALUNO,  
                                               P.CPF
                                        FROM   LY_ALUNO A (NOLOCK) 
                                               INNER JOIN LY_PESSOA P (NOLOCK) 
                                                       ON A.PESSOA = P.PESSOA 
                                        WHERE  A.ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosPessoa.Aluno = Convert.ToString(reader["ALUNO"]);
                    dadosPessoa.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    dadosPessoa.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dadosPessoa.UnidadeEnsino = Convert.ToString(reader["UNIDADE_ENSINO"]);
                    dadosPessoa.SitAluno = Convert.ToString(reader["SIT_ALUNO"]);
                    dadosPessoa.Cpf = Convert.ToString(reader["CPF"]);

                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dadosPessoa.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                }

                return dadosPessoa;
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

        public DadosFichaAluno ObtemFichaAlunoPor(string aluno)
        {
            DadosFichaAluno dados = new DadosFichaAluno();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.PESSOA ,
							A.ALUNO ,
                            p.NOME_COMPL ,
                            P.DT_NASC ,
                            P.SEXO ,
                            P.QT_FILHOS ,
                            P.TIPO_SANGUINEO ,
                            P.ETNIA ,
                            P.EST_CIVIL ,
                            PA.NOME AS PAIS_NASC ,
                            UPPER(NA.NOME) AS NACIONALIDADE ,
                            MNASC.UF_SIGLA AS UF_NASC ,
                            MNASC.NOME AS MUNICIPIO_NASC ,
                            P.CREDO ,
							P.NECESSIDADEESPECIALID ,
                            NEC.DESCRICAO AS NECESSIDADE_ESPECIAL,
							P.NOME_MAE ,
                            P.MAE_FALECIDA ,
                            P.MAE_CPF ,
                            P.NOME_PAI ,
                            P.PAI_FALECIDO ,
                            P.PAI_CPF ,
							P.RESPONSAVEL ,
                            P.RESP_NOME_COMPL ,
                            P.RESP_CPF ,
                            MAE_TELEFONE ,
                            PAI_TELEFONE ,
                            RESP_FONE ,
							P.ENDERECO ,
                            P.END_NUM ,
                            P.END_COMPL ,
                            P.BAIRRO ,
                            MEND.NOME AS END_MUNICIPIO ,
                            MEND.UF_SIGLA AS END_UF ,
                            P.CEP ,
                            FLP.FL_FIELD_01 AS LOCALIZACAO ,  
                            P.FONE ,
                            P.CELULAR ,
                            A.E_MAIL_INTERNO ,
							P.CPF ,
                            P.RG_TIPO ,
                            P.RG_NUM ,
                            FLP.FL_FIELD_07 AS COMPLETMENTO_RG ,
                            P.RG_UF ,
                            P.RG_EMISSOR ,
                            P.RG_DTEXP ,
                            P.ID_CENSO ,
                            FLP.FL_FIELD_08 AS NIS ,
                            FLP.FL_FIELD_02 AS TIPO_CERTIDAO ,
                            FLP.FL_FIELD_09 AS CERTIDAO_CIVIL ,
                            C.UF AS UF_CARTORIO ,
                            C.MUNICIPIO AS MUNICIPIO_CARTORIO ,
                            C.NOME_CARTORIO ,
                            P.CERT_NASC_LIVRO ,
                            P.CERT_NASC_FOLHA ,
                            P.CERT_NASC_NUM ,
                            P.CERT_NASC_EMISSAO ,
                            P.CERT_NUMERO_MATRICULA,
                            FLP.FL_FIELD_23 AS QUILOMBOLA_INDIGENA
                    FROM   LY_ALUNO A ( NOLOCK ) 
                            INNER JOIN DBO.LY_PESSOA P ( NOLOCK ) ON A.PESSOA = P.PESSOA
                            LEFT JOIN DBO.LY_FL_PESSOA FLP ( NOLOCK ) ON P.PESSOA = FLP.PESSOA
                            LEFT JOIN MUNICIPIO MNASC ( NOLOCK ) ON P.MUNICIPIO_NASC = MNASC.CODIGO
                            LEFT JOIN HADES.DBO.HD_PAIS PA ( NOLOCK ) ON P.PAIS_NASC = PA.PAIS
                            LEFT JOIN MUNICIPIO MEND ( NOLOCK ) ON P.END_MUNICIPIO = MEND.CODIGO
                            LEFT JOIN CARTORIO C ( NOLOCK ) ON C.COD_CARTORIO = P.ID_CARTORIO
                            LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID	
                            LEFT JOIN NACIONALIDADE NA ON NA.NACIONALIDADE =P.NACIONALIDADE								
                    WHERE   A.ALUNO = @ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                reader = ctx.GetDataReader(contextQuery);

                if (reader.Read())
                {
                    //Preenche dados pessoais
                    dados.Pessoa = Convert.ToInt32(reader["PESSOA"]);
                    dados.AlunoMatricula = Convert.ToString(reader["ALUNO"]);
                    dados.NomeAluno = Convert.ToString(reader["NOME_COMPL"]);
                    dados.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);
                    if (reader["QT_FILHOS"] != DBNull.Value)
                    {
                        dados.QuantidadeFilhos = Convert.ToInt32(reader["QT_FILHOS"]);
                    }
                    dados.TipoSanguineo = Convert.ToString(reader["TIPO_SANGUINEO"]);
                    dados.Etnia = Convert.ToString(reader["ETNIA"]);
                    dados.EstadoCivil = Convert.ToString(reader["EST_CIVIL"]);
                    dados.PaisNascimento = Convert.ToString(reader["PAIS_NASC"]);
                    dados.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);
                    dados.UfNascimento = Convert.ToString(reader["UF_NASC"]);
                    dados.Naturalidade = Convert.ToString(reader["MUNICIPIO_NASC"]);
                    dados.Credo = Convert.ToString(reader["CREDO"]);
                    dados.DescendenciaFamiliar = Convert.ToString(reader["QUILOMBOLA_INDIGENA"]);
                    dados.NecessidadeEspecial = Convert.ToString(reader["NECESSIDADE_ESPECIAL"]);

                    //Preenche filiacao
                    dados.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dados.FalecidaMae = Convert.ToString(reader["MAE_FALECIDA"]);
                    dados.CPFMae = Convert.ToString(reader["MAE_CPF"]);
                    dados.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dados.FalecidoPai = Convert.ToString(reader["PAI_FALECIDO"]);
                    dados.CPFPai = Convert.ToString(reader["PAI_CPF"]);
                    dados.ResponsavelLegal = Convert.ToString(reader["RESPONSAVEL"]);
                    if (dados.ResponsavelLegal.ToUpper() == "OUTROS;")
                    {
                        dados.NomeOutros = Convert.ToString(reader["RESP_NOME_COMPL"]);
                        dados.CpfOutros = Convert.ToString(reader["RESP_CPF"]);
                        dados.TelResponsavel = Convert.ToString(reader["RESP_FONE"]);
                    }
                    dados.TelMae = Convert.ToString(reader["MAE_TELEFONE"]);
                    dados.TelPai = Convert.ToString(reader["PAI_TELEFONE"]);

                    //Preenche endereço
                    dados.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dados.NumeroEndereco = Convert.ToString(reader["END_NUM"]);
                    dados.ComplementoEndereco = Convert.ToString(reader["END_COMPL"]);
                    dados.BairroEndereco = Convert.ToString(reader["BAIRRO"]);
                    dados.MunicipioEndereco = Convert.ToString(reader["END_MUNICIPIO"]);
                    dados.EstadoEndereco = Convert.ToString(reader["END_UF"]);
                    dados.CepEndereco = Convert.ToString(reader["CEP"]);
                    dados.LocalizacaoEndereco = Convert.ToString(reader["LOCALIZACAO"]);

                    //Preenche contato
                    dados.Telefone = Convert.ToString(reader["FONE"]);
                    dados.Celular = Convert.ToString(reader["CELULAR"]);
                    dados.Email = Convert.ToString(reader["E_MAIL_INTERNO"]);

                    //Preenche Documentos
                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.TipoDocumento = Convert.ToString(reader["RG_TIPO"]);
                    dados.NumeroDocumento = Convert.ToString(reader["RG_NUM"]);
                    dados.ComplementoIdentidade = Convert.ToString(reader["COMPLETMENTO_RG"]);
                    dados.EstadoDocumento = Convert.ToString(reader["RG_UF"]);
                    dados.OrgaoEmissorDocumento = Convert.ToString(reader["RG_EMISSOR"]);
                    if (reader["RG_DTEXP"] != DBNull.Value)
                    {
                        dados.DataExpedicaoDocumento = Convert.ToDateTime(reader["RG_DTEXP"]);
                    }

                    //Preenche Outras Informacoes
                    dados.Inep = Convert.ToString(reader["ID_CENSO"]);
                    dados.Nis = Convert.ToString(reader["NIS"]);

                    //Preenche Certidao Civil
                    dados.TipoCertidao = Convert.ToString(reader["TIPO_CERTIDAO"]);
                    dados.CertidaoCivil = Convert.ToString(reader["CERTIDAO_CIVIL"]);
                    dados.UfCartorio = Convert.ToString(reader["UF_CARTORIO"]);
                    dados.MunicipioCartorio = Convert.ToString(reader["MUNICIPIO_CARTORIO"]);
                    dados.NomeCartorio = Convert.ToString(reader["NOME_CARTORIO"]);
                    dados.Livro = Convert.ToString(reader["CERT_NASC_LIVRO"]);
                    dados.Folha = Convert.ToString(reader["CERT_NASC_FOLHA"]);
                    dados.NumeroTermo = Convert.ToString(reader["CERT_NASC_NUM"]);
                    if (reader["CERT_NASC_EMISSAO"] != DBNull.Value)
                    {
                        dados.DataEmissaoCertidao = Convert.ToDateTime(reader["CERT_NASC_EMISSAO"]);
                    }
                    dados.MatriculaCertidao = Convert.ToString(reader["CERT_NUMERO_MATRICULA"]);
                }

                //Buscar Foto do aluno
                dados.Foto = FotoPessoa.Carregar(Convert.ToInt32(dados.Pessoa));

                //Ajusta descriçoes e formataçoes necessárias
                if (dados.Sexo == "F")
                {
                    dados.Sexo = "Feminino";
                }
                else if (dados.Sexo == "M")
                {
                    dados.Sexo = "Masculino";
                }

                if (dados.DescendenciaFamiliar == "S")
                {
                    dados.DescendenciaFamiliar = "Sim";
                }
                else if (dados.DescendenciaFamiliar == "N")
                {
                    dados.DescendenciaFamiliar = "Não";
                }

                if (dados.NomeMae != "NÃO DECLARADO")
                {
                    if (dados.FalecidaMae == "S")
                    {
                        dados.FalecidaMae = "Sim";
                    }
                    else if (dados.FalecidaMae == "N")
                    {
                        dados.FalecidaMae = "Não";
                    }
                }
                else
                {
                    dados.FalecidaMae = string.Empty;
                }

                if (dados.NomePai != "NÃO DECLARADO")
                {
                    if (dados.FalecidoPai == "S")
                    {
                        dados.FalecidoPai = "Sim";
                    }
                    else if (dados.FalecidoPai == "N")
                    {
                        dados.FalecidoPai = "Não";
                    }
                }
                else
                {
                    dados.FalecidoPai = string.Empty;
                }

                if (dados.Credo == "Naodeclarado")
                {
                    dados.Credo = "Não declarado";
                }

                if (!string.IsNullOrEmpty(dados.ResponsavelLegal))
                {
                    dados.ResponsavelLegal = dados.ResponsavelLegal.Replace(';', ' ');
                }
                dados.CPFMae = dados.CPFMae.AplicarMascaraCPF();
                dados.CPFPai = dados.CPFPai.AplicarMascaraCPF();
                dados.Cpf = dados.Cpf.AplicarMascaraCPF();

                return dados;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public DataTable ObtemDadosAlunoParaMatriculaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT A.ALUNO, 
                       A.UNIDADE_ENSINO, 
                       C.NOME       AS NOMECURSO, 
                       C.CURSO, 
                       T.DESCRICAO  AS NOMETURNO, 
                       T.TURNO, 
                       A.CURRICULO, 
                       A.SERIE, 
                       S.DESCRICAO  AS NOMESERIE, 
                       UE.NOME_COMP AS NOMEFACULDADE, 
                       N.REGIONAL  AS REGIONAL, 
                       UE.ID_REGIONAL, 
                       A.UNIDADE_FISICA, 
                       UF.NOME_COMP AS NOMEUNIDADEFISICA 
                FROM   LY_ALUNO A (NOLOCK) 
                       INNER JOIN LY_CURSO C (NOLOCK) 
                               ON A.CURSO = C.CURSO 
                       LEFT JOIN LY_UNIDADE_FISICA UF (NOLOCK) 
                              ON A.UNIDADE_FISICA = UF.UNIDADE_FIS 
                       INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                               ON A.UNIDADE_ENSINO = UE.UNIDADE_ENS 
                       INNER JOIN LY_TURNO (NOLOCK) T 
                               ON A.TURNO = T.TURNO 
                       INNER JOIN LY_SERIE (NOLOCK) S 
                               ON C.CURSO = S.CURSO 
                                  AND A.CURRICULO = S.CURRICULO 
                                  AND T.TURNO = S.TURNO 
                                  AND A.SERIE = S.SERIE 
                       LEFT JOIN TCE_REGIONAL (NOLOCK) N 
                              ON N.ID_REGIONAL = UE.ID_REGIONAL 
                WHERE  A.ALUNO = @ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

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

        public static string ConsultarNome(string aluno)
        {
            return ConsultarCampo("select top 1 nome_compl from ly_aluno a inner join ly_pessoa p on p.pessoa= a.pessoa where aluno = ?", aluno);
        }

        public static string ConsultarCenso(string aluno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT TOP 1
                                CENSO
                        FROM    TCE_CONFIRMACAO_MATRICULA (NOLOCK)
                        WHERE   ANO = YEAR(GETDATE())
                                AND ALUNO = @ALUNO
                                AND ( STATUS IS NULL
                                      OR STATUS = 'Confirmado'
                                     )
                        ORDER BY ID_CONFIRMACAO_MATRICULA DESC ");

                contextQuery.Parameters.Add("@ALUNO", aluno);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static QueryTable ConsultarAlunos(string aluno, string coordenadoria, string unidadeEns, string curso, string turno, string serie, string curriculo)
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT  
                        VW_ZZCRO_ALUNO.ALUNO aluno,  
                        VW_ZZCRO_ALUNO.NOME_COMPL nome_compl,  
                        LY_CURSO.CURSO curso,  
                        LY_CURSO.NOME nome_curso,  
                        LY_TURNO.TURNO turno,  
                        LY_TURNO.DESCRICAO descricao_turno,  
                        LY_SERIE.SERIE serie,  
                        LY_SERIE.DESCRICAO descricao_serie,  
                        LY_UNIDADE_FISICA.UNIDADE_FIS unidade_fisica,  
                        LY_UNIDADE_FISICA.NOME_COMP nome_unidadefisica,  
                        LY_UNIDADE_ENSINO.UNIDADE_ENS unidade_ensino,  
                        LY_UNIDADE_ENSINO.NOME_COMP nome_unidadeensino  
                        FROM VW_ZZCRO_ALUNO INNER JOIN LY_SERIE   
                        ON VW_ZZCRO_ALUNO.CURSO = LY_SERIE.CURSO   
                        AND VW_ZZCRO_ALUNO.TURNO = LY_SERIE.TURNO   
                        AND VW_ZZCRO_ALUNO.CURRICULO = LY_SERIE.CURRICULO   
                        AND VW_ZZCRO_ALUNO.SERIE = LY_SERIE.SERIE   
                        INNER JOIN LY_CURSO   
                        ON ly_curso.curso = VW_ZZCRO_ALUNO.curso   
                        INNER JOIN ly_unidade_ensino   
                        ON ly_unidade_ensino.unidade_ens = VW_ZZCRO_ALUNO.unidade_ensino  
                        INNER JOIN LY_TURNO   
                        ON VW_ZZCRO_ALUNO.TURNO = LY_TURNO.TURNO   
                        LEFT JOIN LY_NUCLEO   
                        ON ly_unidade_ensino.NUCLEO = LY_NUCLEO.NUCLEO   
                        LEFT JOIN LY_UNIDADE_FISICA   
                        ON VW_ZZCRO_ALUNO.UNIDADE_FISICA = LY_UNIDADE_FISICA.UNIDADE_FIS   
                        WHERE ");

            if (aluno != null)
            {
                sql.Append(String.Format("aluno = '{0}'", aluno));
            }
            else
            {
                if (curso != string.Empty && unidadeEns != string.Empty && turno != string.Empty && serie != string.Empty)
                {
                    if (curriculo != string.Empty)
                    {
                        if (coordenadoria != string.Empty)
                            sql.Append(String.Format("ly_nucleo.nucleo = '{0}' and unidade_ens = '{1}' and VW_ZZCRO_ALUNO.curso = '{2}' and VW_ZZCRO_ALUNO.turno = '{3}'and VW_ZZCRO_ALUNO.curriculo = '{4}' and VW_ZZCRO_ALUNO.serie = {5}", coordenadoria, unidadeEns, curso, turno, curriculo, serie));
                        else
                            sql.Append(String.Format("unidade_ens = '{0}' and VW_ZZCRO_ALUNO.curso = '{1}' and VW_ZZCRO_ALUNO.turno = '{2}'and VW_ZZCRO_ALUNO.curriculo = '{3}' and VW_ZZCRO_ALUNO.serie = {4}", unidadeEns, curso, turno, curriculo, serie));
                    }
                    else
                    {
                        if (coordenadoria != string.Empty)
                            sql.Append(String.Format("ly_nucleo.nucleo = '{0}' and unidade_ens = '{1}' and VW_ZZCRO_ALUNO.curso = '{2}' and VW_ZZCRO_ALUNO.turno = '{3}' and VW_ZZCRO_ALUNO.serie = {4}", coordenadoria, unidadeEns, curso, turno, serie));
                        else
                            sql.Append(String.Format("unidade_ens = '{0}' and VW_ZZCRO_ALUNO.curso = '{1}' and VW_ZZCRO_ALUNO.turno = '{2}' and VW_ZZCRO_ALUNO.serie = {3}", unidadeEns, curso, turno, serie));
                    }

                }
                else
                {
                    sql.Append("aluno = ''");
                }
            }

            return Consultar(sql.ToString());
        }

        public static QueryTable ConsultarMeusAlunos(string coordenadoria, string unidadeEns, string curso, string turno, string serie, string turma)
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT DISTINCT 
				a.ALUNO aluno,  
				PE.NOME_COMPL nome_compl,  
				a.CURSO curso,  
				c.NOME nome_curso,  
				a.TURNO turno,  
				t.DESCRICAO descricao_turno,  
				a.SERIE serie,  
				s.DESCRICAO descricao_serie,  
				a.UNIDADE_FISICA unidade_fisica,  
				uf.NOME_COMP nome_unidadefisica,  
				a.UNIDADE_ENSINO unidade_ensino,  
				ue.NOME_COMP nome_unidadeensino  
				FROM VW_ZZCRO_ALUNO a 
                INNER JOIN LY_PESSOA PE ON A.PESSOA =PE.PESSOA
				INNER JOIN LY_SERIE s ON a.CURSO = s.CURSO   
					AND a.TURNO = s.TURNO   
					AND a.CURRICULO = s.CURRICULO   
					AND a.SERIE = s.SERIE   
				INNER JOIN LY_CURSO c ON c.curso = a.curso
				INNER JOIN ly_unidade_ensino ue ON ue.unidade_ens = a.unidade_ensino  
				INNER JOIN LY_TURNO t ON a.TURNO = t.TURNO    
				LEFT JOIN LY_NUCLEO n ON ue.NUCLEO = n.NUCLEO   
				LEFT JOIN LY_UNIDADE_FISICA uf ON a.UNIDADE_FISICA = uf.UNIDADE_FIS 
				LEFT JOIN LY_MATGRADE matgrade on a.ALUNO = matgrade.ALUNO 
				LEFT JOIN LY_GRADE_SERIE gradeserie on matgrade.GRADE_ID = gradeserie.GRADE_ID ");
            if (!String.IsNullOrEmpty(coordenadoria) ||
                !String.IsNullOrEmpty(unidadeEns) ||
                !String.IsNullOrEmpty(curso) ||
                !String.IsNullOrEmpty(turno) ||
                !String.IsNullOrEmpty(serie) ||
                !String.IsNullOrEmpty(turma))
            {
                sql.Append(" WHERE ");
                if (!String.IsNullOrEmpty(coordenadoria))
                    sql.Append(" ue.NUCLEO = '" + coordenadoria + "' ");
                if (!String.IsNullOrEmpty(coordenadoria) && !String.IsNullOrEmpty(unidadeEns))
                    sql.Append(" AND a.UNIDADE_ENSINO = '" + unidadeEns + "' ");
                else if (!String.IsNullOrEmpty(unidadeEns))
                    sql.Append(" a.UNIDADE_ENSINO = '" + unidadeEns + "' ");
                if ((!String.IsNullOrEmpty(coordenadoria) || !String.IsNullOrEmpty(unidadeEns)) &&
                    !String.IsNullOrEmpty(curso))
                    sql.Append(" AND a.CURSO = '" + curso + "' ");
                else if (!String.IsNullOrEmpty(curso))
                    sql.Append(" a.CURSO = '" + curso + "' ");
                if ((!String.IsNullOrEmpty(coordenadoria) || !String.IsNullOrEmpty(unidadeEns) || !String.IsNullOrEmpty(curso)) &&
                    !String.IsNullOrEmpty(turno))
                    sql.Append(" AND a.TURNO = '" + turno + "' ");
                else if (!String.IsNullOrEmpty(turno))
                    sql.Append(" a.TURNO = '" + turno + "' ");
                if ((!String.IsNullOrEmpty(coordenadoria) || !String.IsNullOrEmpty(unidadeEns) || !String.IsNullOrEmpty(curso) || !String.IsNullOrEmpty(turno)) &&
                    !String.IsNullOrEmpty(serie))
                    sql.Append(" AND a.SERIE = '" + serie + "' ");
                else if (!String.IsNullOrEmpty(serie))
                    sql.Append(" a.SERIE = '" + serie + "' ");
                if ((!String.IsNullOrEmpty(coordenadoria) || !String.IsNullOrEmpty(unidadeEns) || !String.IsNullOrEmpty(curso) || !String.IsNullOrEmpty(turno) || !String.IsNullOrEmpty(serie)) &&
                    !String.IsNullOrEmpty(turma))
                    sql.Append(" AND gradeserie.GRADE_ID = '" + turma + "' ");
                else if (!String.IsNullOrEmpty(turma))
                    sql.Append(" gradeserie.GRADE_ID = '" + turma + "' ");
            }
            return Consultar(sql.ToString());
        }

        public static Ly_aluno.Row ConsultarAluno(string aluno)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                Ly_aluno.Row dadosAluno = Ly_aluno.QueryFirstRow(connection, "aluno = ?", aluno);
                return dadosAluno;
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable ConsultarMatriculaPrincipalPorTurma(string turma, string unidadeResponsavel, decimal ano, decimal semestre, string curso, string turno, string curriculo, decimal serie)
        {
            var contextQuery =
                new ContextQuery(
                    @" SELECT distinct m.ALUNO ALUNO1,  P.NOME_COMPL NOME_COMPL1,  
                        CASE WHEN m.DEPENDENCIA = 'S' THEN m.SIT_MATRICULA + ' - Dependência'
                             ELSE m.SIT_MATRICULA
                        END SIT_MATRICULA,                        
                        '' NOVATURMA,
                        CASE WHEN m.DEPENDENCIA = 'S' THEN 'S'
                             ELSE 'N'
                        END DEPENDENCIA
                FROM    LY_ALUNO A
                        INNER JOIN dbo.LY_MATRICULA M ON a.ALUNO = m.ALUNO
                        INNER JOIN ly_turma t ON m.turma = t.TURMA
                                                 AND m.DISCIPLINA = t.DISCIPLINA
                                                 AND m.ANO = t.ano
                                                 AND m.SEMESTRE = t.SEMESTRE
                        INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA
                WHERE m.TURMA = @TURMA
                    AND t.FACULDADE = @FACULDADE
                    AND m.ANO = @ANO
                    AND m.SEMESTRE = @SEMESTRE
                    AND t.CURSO = @CURSO
                    AND t.TURNO = @TURNO
                    AND t.CURRICULO = @CURRICULO
                    AND t.SERIE = @SERIE 
                    AND ISNULL(T.ELETIVA, 'N') = 'N'
                ORDER BY DEPENDENCIA, NOME_COMPL ");

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@FACULDADE", unidadeResponsavel);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@SERIE", serie);

            return Consultar(contextQuery);
        }

        public static DataTable ConsultarMatriculaEletivaPorTurma(string turma, string unidadeResponsavel, decimal ano, decimal semestre, string curso, string turno, string curriculo, decimal serie)
        {
            var contextQuery =
                new ContextQuery(
                    @" SELECT distinct M.ALUNO AS ALUNO1,  
					   P.NOME_COMPL AS NOME_COMPL1,  
                       m.SIT_MATRICULA,
					   CONVERT(char(1), NULL) AS ELETIVA1,
					   CONVERT(char(1), NULL) AS ELETIVA2,
					   CONVERT(char(1), NULL) AS ELETIVA3
                INTO #ALUNOSELETIVAS     
                FROM    LY_ALUNO A
                        INNER JOIN dbo.LY_MATRICULA M ON a.ALUNO = m.ALUNO
                        INNER JOIN ly_turma t ON m.turma = t.TURMA
                                                 AND m.DISCIPLINA = t.DISCIPLINA
                                                 AND m.ANO = t.ano
                                                 AND m.SEMESTRE = t.SEMESTRE
                        INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA					
                WHERE m.TURMA = @TURMA
					AND t.FACULDADE = @FACULDADE
					AND m.ANO = @ANO
					AND m.SEMESTRE = @SEMESTRE
					AND t.CURSO = @CURSO
					AND t.TURNO = @TURNO
					AND t.CURRICULO = @CURRICULO
					AND t.SERIE = @SERIE 
                    AND ISNULL(T.ELETIVA, 'N') = 'S'
					AND SIT_MATRICULA = 'Matriculado'                
				
				UPDATE #ALUNOSELETIVAS
				SET ELETIVA1 = CASE 
								WHEN (SELECT COUNT(*)
									FROM dbo.LY_MATRICULA M  
										INNER JOIN LY_TURMA T 
												ON M.TURMA = T.TURMA
												AND M.DISCIPLINA = T.DISCIPLINA
												AND M.ANO = T.ANO
												AND M.SEMESTRE = T.SEMESTRE
										INNER JOIN  LY_DISCIPLINA D (NOLOCK) 
													ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA											
									WHERE D.GRUPO = 1
											AND M.SIT_MATRICULA = 'Matriculado'
											AND M.ALUNO = #ALUNOSELETIVAS.ALUNO1
											AND m.TURMA = @TURMA
											AND t.FACULDADE = @FACULDADE
											AND m.ANO = @ANO
											AND m.SEMESTRE = @SEMESTRE
												) > 0 THEN 'X'
													ELSE ''
												END
				
				UPDATE #ALUNOSELETIVAS
				SET ELETIVA2 = CASE 
								WHEN (SELECT COUNT(*)
									FROM dbo.LY_MATRICULA M  
										INNER JOIN LY_TURMA T 
												ON M.TURMA = T.TURMA
												AND M.DISCIPLINA = T.DISCIPLINA
												AND M.ANO = T.ANO
												AND M.SEMESTRE = T.SEMESTRE
										INNER JOIN  LY_DISCIPLINA D (NOLOCK) 
													ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA											
									WHERE D.GRUPO = 2
											AND M.SIT_MATRICULA = 'Matriculado'
											AND M.ALUNO = #ALUNOSELETIVAS.ALUNO1
											AND m.TURMA = @TURMA
											AND t.FACULDADE = @FACULDADE
											AND m.ANO = @ANO
											AND m.SEMESTRE = @SEMESTRE
												) > 0 THEN 'X'
													ELSE ''
												END

				UPDATE #ALUNOSELETIVAS
				SET ELETIVA3 = CASE 
								WHEN (SELECT COUNT(*)
									FROM dbo.LY_MATRICULA M  
										INNER JOIN LY_TURMA T 
												ON M.TURMA = T.TURMA
												AND M.DISCIPLINA = T.DISCIPLINA
												AND M.ANO = T.ANO
												AND M.SEMESTRE = T.SEMESTRE
										INNER JOIN  LY_DISCIPLINA D (NOLOCK) 
													ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA											
									WHERE D.GRUPO = 3
											AND M.SIT_MATRICULA = 'Matriculado'
											AND M.ALUNO = #ALUNOSELETIVAS.ALUNO1
											AND m.TURMA = @TURMA
											AND t.FACULDADE = @FACULDADE
											AND m.ANO = @ANO
											AND m.SEMESTRE = @SEMESTRE
												) > 0 THEN 'X'
													ELSE ''
												END
					
					SELECT *
					FROM #ALUNOSELETIVAS
					ORDER BY NOME_COMPL1 ");

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@FACULDADE", unidadeResponsavel);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@SERIE", serie);

            return Consultar(contextQuery);
        }

        public static DataTable ConsultarMatriculaPrincipalPorTurmaHistorico(string turma, string unidadeResponsavel, decimal ano, decimal semestre, string curso, string turno, string curriculo, decimal serie)
        {
            var contextQuery =
                new ContextQuery(
                    @" SELECT DISTINCT
                            m.ALUNO ALUNO1,  P.NOME_COMPL NOME_COMPL1,                             
                            CASE WHEN m.DEPENDENCIA = 'S' THEN s.SITUACAO_FINAL + ' - Dependência'
                                 ELSE s.SITUACAO_FINAL
                            END SIT_MATRICULA,
                            '' NOVATURMA,
                            CASE WHEN m.DEPENDENCIA = 'S' THEN 'S'
                                 ELSE 'N'
                            END DEPENDENCIA
                    FROM    LY_ALUNO A
                            INNER JOIN dbo.LY_HISTMATRICULA M ON a.ALUNO = m.ALUNO
                            INNER JOIN ly_turma t ON m.turma = t.TURMA
                                                     AND m.DISCIPLINA = t.DISCIPLINA
                                                     AND m.ANO = t.ano
                                                     AND m.SEMESTRE = t.SEMESTRE
                            LEFT JOIN dbo.TCE_SITUACAO_FINAL_ALUNO s ON m.ALUNO = s.ALUNO
                                                                        AND m.ANO = s.ANO
                                                                        AND m.SEMESTRE = s.PERIODO
                            INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA
                WHERE m.TURMA = @TURMA
                    AND t.FACULDADE = @FACULDADE
                    AND m.ANO = @ANO
                    AND m.SEMESTRE = @SEMESTRE
                    AND t.CURSO = @CURSO
                    AND t.TURNO = @TURNO
                    AND t.CURRICULO = @CURRICULO
                    AND t.SERIE = @SERIE  
				    AND ISNULL(T.ELETIVA, 'N') = 'N'
                ORDER BY DEPENDENCIA, NOME_COMPL ");

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@FACULDADE", unidadeResponsavel);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@SERIE", serie);

            return Consultar(contextQuery);
        }

        public static DataTable ConsultarMatriculaEletivaPorTurmaHistorico(string turma, string unidadeResponsavel, decimal ano, decimal semestre, string curso, string turno, string curriculo, decimal serie)
        {
            var contextQuery =
                new ContextQuery(
                    @" SELECT distinct M.ALUNO AS ALUNO1,  
					   P.NOME_COMPL AS NOME_COMPL1,  
                       M.SITUACAO_HIST AS SIT_MATRICULA,
					   CONVERT(char(1), NULL) AS ELETIVA1,
					   CONVERT(char(1), NULL) AS ELETIVA2,
					   CONVERT(char(1), NULL) AS ELETIVA3
                INTO #ALUNOSELETIVAS     
                FROM    LY_ALUNO A
                        INNER JOIN dbo.LY_HISTMATRICULA M ON a.ALUNO = m.ALUNO
                        INNER JOIN ly_turma t ON m.turma = t.TURMA
                                                 AND m.DISCIPLINA = t.DISCIPLINA
                                                 AND m.ANO = t.ano
                                                 AND m.SEMESTRE = t.SEMESTRE
                        INNER JOIN LY_PESSOA P ON P.PESSOA = A.PESSOA					
                WHERE m.TURMA = @TURMA
					AND t.FACULDADE = @FACULDADE
					AND m.ANO = @ANO
					AND m.SEMESTRE = @SEMESTRE
					AND t.CURSO = @CURSO
					AND t.TURNO = @TURNO
					AND t.CURRICULO = @CURRICULO
					AND t.SERIE = @SERIE 
                    AND ISNULL(T.ELETIVA, 'N') = 'S'
					AND SITUACAO_HIST <> 'Cancelado'                
				
				UPDATE #ALUNOSELETIVAS
				SET ELETIVA1 = CASE 
								WHEN (SELECT COUNT(*)
									FROM dbo.LY_HISTMATRICULA M  
										INNER JOIN LY_TURMA T 
												ON M.TURMA = T.TURMA
												AND M.DISCIPLINA = T.DISCIPLINA
												AND M.ANO = T.ANO
												AND M.SEMESTRE = T.SEMESTRE
										INNER JOIN  LY_DISCIPLINA D (NOLOCK) 
													ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA											
									WHERE D.GRUPO = 1
											AND SITUACAO_HIST <> 'Cancelado'
											AND M.ALUNO = #ALUNOSELETIVAS.ALUNO1
											AND m.TURMA = @TURMA
											AND t.FACULDADE = @FACULDADE
											AND m.ANO = @ANO
											AND m.SEMESTRE = @SEMESTRE
												) > 0 THEN 'X'
													ELSE ''
												END
				
				UPDATE #ALUNOSELETIVAS
				SET ELETIVA2 = CASE 
								WHEN (SELECT COUNT(*)
									FROM dbo.LY_HISTMATRICULA M  
										INNER JOIN LY_TURMA T 
												ON M.TURMA = T.TURMA
												AND M.DISCIPLINA = T.DISCIPLINA
												AND M.ANO = T.ANO
												AND M.SEMESTRE = T.SEMESTRE
										INNER JOIN  LY_DISCIPLINA D (NOLOCK) 
													ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA											
									WHERE D.GRUPO = 2
											AND SITUACAO_HIST <> 'Cancelado'
											AND M.ALUNO = #ALUNOSELETIVAS.ALUNO1
											AND m.TURMA = @TURMA
											AND t.FACULDADE = @FACULDADE
											AND m.ANO = @ANO
											AND m.SEMESTRE = @SEMESTRE
												) > 0 THEN 'X'
													ELSE ''
												END

				UPDATE #ALUNOSELETIVAS
				SET ELETIVA3 = CASE 
								WHEN (SELECT COUNT(*)
									FROM dbo.LY_HISTMATRICULA M  
										INNER JOIN LY_TURMA T 
												ON M.TURMA = T.TURMA
												AND M.DISCIPLINA = T.DISCIPLINA
												AND M.ANO = T.ANO
												AND M.SEMESTRE = T.SEMESTRE
										INNER JOIN  LY_DISCIPLINA D (NOLOCK) 
													ON ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA) = D.DISCIPLINA											
									WHERE D.GRUPO = 3
											AND SITUACAO_HIST <> 'Cancelado'
											AND M.ALUNO = #ALUNOSELETIVAS.ALUNO1
											AND m.TURMA = @TURMA
											AND t.FACULDADE = @FACULDADE
											AND m.ANO = @ANO
											AND m.SEMESTRE = @SEMESTRE
												) > 0 THEN 'X'
													ELSE ''
												END
					
					SELECT *
					FROM #ALUNOSELETIVAS	
					ORDER BY NOME_COMPL1 ");

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@FACULDADE", unidadeResponsavel);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@SERIE", serie);

            return Consultar(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(string aluno, decimal pessoa, string curso)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Matricula rnMatricula = new Matricula();
            RN.HCursosConcl rnHCursosConcl = new HCursosConcl();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.CartaoEstudante.Solicitacao rnSolicitacao = new Techne.Lyceum.RN.CartaoEstudante.Solicitacao();
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.Transferencia rnTransferencia = new Transferencia();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnMatricula.PossuiMatriculaPor(contexto, aluno))
                    {
                        mensagens.Add("Não é possível excluir aluno pois ele possui registro de matrícula.");
                    }

                    if (rnHCursosConcl.ExistePor(contexto, aluno))
                    {
                        mensagens.Add("Não é possível excluir aluno pois ele possui registro de encerramento.");
                    }

                    if (rnAvaliacaoNapes.PossuiAvaliacaoPor(contexto, aluno))
                    {
                        mensagens.Add("Não é possível excluir aluno pois ele possui registro de avaliação do napes.");
                    }

                    //Verfiica se alguma confirmação já foi confirmada
                    if (rnConfirmacaoMatricula.ExisteConfirmacaoConfirmadaPor(contexto, aluno))
                    {
                        mensagens.Add("Não é possível excluir aluno pois ele possui confirmação de matricula confirmada.");
                    }

                    if (rnSolicitacao.ExistePor(contexto, aluno))
                    {
                        mensagens.Add("Não é possível excluir aluno pois ele possui solicitação de cartão.");
                    }

                    if (rnSolicitacao.ExistePor(contexto, aluno))
                    {
                        mensagens.Add("Não é possível excluir aluno pois ele possui solicitação de cartão.");
                    }

                    if (rnTransferencia.ExistePor(contexto, aluno))
                    {
                        mensagens.Add("Não é possível excluir aluno pois ele possui registro de transferencia.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Remove(string aluno, decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RN.UnidadeEnsinoCompartilhada.Aluno_UnidadeEnsinoCompartilhada rnAluno_UnidadeEnsinoCompartilhada = new Techne.Lyceum.RN.UnidadeEnsinoCompartilhada.Aluno_UnidadeEnsinoCompartilhada();
            RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial rnAlunoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.VinculoLy rnVinculo = new VinculoLy();
            RN.Docentes rnDocentes = new Docentes();
            RN.NecessidadeEspecial.RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.RecursoNecessidadeEspecial();

            try
            {
                //Remove confirmacao
                rnConfirmacaoMatricula.Remove(contexto, aluno);

                //Remove ALUNO_UNIDADEENSINOCOMPARTILHADA
                rnAluno_UnidadeEnsinoCompartilhada.Remove(contexto, aluno);

                //Remove NECESSIDADEESPECIAL.ALUNORECURSONECESSIDADEESPECIAL
                rnAlunoRecursoNecessidadeEspecial.Remove(contexto, aluno);

                //Remove todas as declaracao do aluno
                RN.DeclaracaoAusencia.RemoverPorAluno(aluno, contexto);

                //Remove ALUNO
                this.Remove(contexto, aluno);

                //Remove recursos para aplicacao de prova
                PessoaRecursoAplicacaoProva.RemoverPessoaRecursoAplicacaoProva(Convert.ToInt32(pessoa), contexto);

                decimal qtdalunos = this.RetornaQuantiadeAlunosPor(contexto, pessoa);
                bool ehDocente = rnDocentes.EhDocentePor(contexto, pessoa);
                bool ehServidor = rnVinculo.EhServidorPor(contexto, pessoa);
                bool enRecurso = rnRecursoNecessidadeEspecial.EhRecursoPor(contexto, pessoa);

                //Verifica se pessoa está associada a outro registro antes de excluir
                if (qtdalunos == 1
                    && !ehDocente
                    && !ehServidor
                    && !enRecurso)
                {
                    //Remove FOTO
                    FotoPessoa.Remover(pessoa, contexto);

                    //Remove FIELDS
                    FlPessoa.Remover(pessoa, contexto);

                    //Remove PESSOA
                    rnPessoa.Remove(contexto, pessoa);
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

        public static string ValidaDadosParaCartao(string aluno, string pessoa)
        {
            var mensagem = new StringBuilder();

            // Verificar enturmação
            if (!Matricula.ExisteDadoMatricula(aluno))
            {
                mensagem.AppendLine("O aluno precisa estar matriculado para solicitar carteirinha.");
            }

            // Verificar endereço, data de nascimento, enturmação
            if (!Pessoa.VerificaMaeCadastrada(pessoa))
            {
                mensagem.AppendLine("Campo nome da mãe não informado.");
            }

            if (Servico.ValidaDadosAluno(aluno))
            {
                mensagem.AppendLine("Campos obrigatórios não foram informados para o aluno. <br>Verifique: nome, data de nascimento, endereço (rua, número, bairro, CEP, município).");
            }

            // Verificar foto
            var mensagemFoto = FotoPessoa.ValidaFoto(decimal.Parse(pessoa));

            if (!string.IsNullOrEmpty(mensagemFoto))
            {
                mensagem.AppendLine("Problema com a foto do aluno: " + mensagemFoto);
            }

            return mensagem.ToString();
        }

        public static AlunoAutenticado Autenticar(string aluno)
        {
            if (string.IsNullOrEmpty(aluno))
            {
                return null;
            }

            var contextQuery = new ContextQuery(
                @"SELECT  a.ALUNO,
                         P.NOME_COMPL,
                        p.DT_NASC,
                        p.PESSOA,
                        ISNULL(fl.FL_FIELD_06, '') AS TELEFONE
                FROM    ly_aluno a
                        INNER JOIN LY_PESSOA p ON a.PESSOA = p.PESSOA
                        LEFT JOIN dbo.LY_FL_PESSOA fl ON p.PESSOA = fl.PESSOA
                WHERE   a.ALUNO = @ALUNO");

            contextQuery.Parameters.Add("@ALUNO", aluno);

            var result = Consultar(contextQuery);

            if (result.Rows.Count != 1)
            {
                return null;
            }

            var autenticacaoAluno = new AlunoAutenticado
            {
                Matricula = Convert.ToString(result.Rows[0]["ALUNO"]),
                Pessoa = Convert.ToDecimal(result.Rows[0]["PESSOA"]),
                Nome = Convert.ToString(result.Rows[0]["NOME_COMPL"]),
                TelefoneResponsavel = Convert.ToString(result.Rows[0]["TELEFONE"])
            };

            var dataNascimento = result.Rows[0]["DT_NASC"];

            if (dataNascimento != null
                && dataNascimento != DBNull.Value)
            {
                autenticacaoAluno.DataNascimento = Convert.ToDateTime(dataNascimento);
            }

            return autenticacaoAluno;
        }

        public static DataTable RetornaAnoPeriodoMatricula(string aluno)
        {
            var contextQuery = new ContextQuery
             (@"select distinct ano,semestre from LY_MATRICULA 
                        WHERE sit_matricula <> 'Cancelado' 
                        AND ALUNO = @ALUNO");

            contextQuery.Parameters.Add("@ALUNO", aluno);

            return Consultar(contextQuery);
        }

        public void GeraCodigoAluno(DataContext context, LyAluno aluno)
        {
            string matricula;
            SqlDataReader reader = default(SqlDataReader);

            try
            {
                using (context = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    context.CommandTimeout = 100;
                    do
                    {
                        if (reader != null)
                            reader.Close();
                        var contextQuery = new ContextQuery("a_Gera_Codigo_Aluno");
                        contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                        contextQuery.Parameters.Add("@p_unidade_ens", TechneDbType.T_CODIGO, aluno.UnidadeEnsino);
                        contextQuery.Parameters.Add("@p_ano", TechneDbType.T_ANO, aluno.AnoIngresso);

                        reader = context.GetDataReader(contextQuery);
                        reader.Read();

                    } while (VerificaMatricula(Convert.ToString(reader["aluno"])));

                    matricula = Convert.ToString(reader["aluno"]);
                    reader.Close();
                }

                aluno.Aluno = matricula;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                context.Dispose();
            }

            //var contextQuery = new ContextQuery("a_Gera_Codigo_Aluno");
            //contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
            //contextQuery.Parameters.Add("@p_unidade_ens", TechneDbType.T_CODIGO, aluno.UnidadeEnsino);
            //contextQuery.Parameters.Add("@p_ano", TechneDbType.T_ANO, aluno.AnoIngresso);

            //using (var reader = context.GetDataReader(contextQuery))
            //{
            //    if (reader.Read())
            //    {
            //        var erro = Convert.ToString(reader["erro"]);
            //        if (string.IsNullOrEmpty(erro))
            //        {
            //            aluno.Aluno = Convert.ToString(reader["aluno"]);
            //        }
            //        else
            //        {
            //            throw new Exception(erro);
            //        }
            //    }
            //}
        }

        private static bool VerificaMatricula(string matricula)
        {
            try
            {
                return (!String.IsNullOrEmpty(RNBase.ConsultarCampo("SELECT top(1) * FROM dbo.LY_ALUNO WHERE ALUNO = ? ", matricula)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int RetornaPessoa(string aluno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  A.PESSOA
                        FROM    LY_ALUNO A
                        WHERE   ALUNO = @ALUNO ");

                contextQuery.Parameters.Add("@ALUNO", aluno);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public decimal ObtemPessoaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT  A.PESSOA
                        FROM    LY_ALUNO A
                        WHERE   ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["PESSOA"] != DBNull.Value ? Convert.ToDecimal(reader["PESSOA"]) : 0;
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

        public DataTable ObtemNaturalidadeEstrangeiraPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
                                SELECT MC.NOME_MUNICIPIO,
                                       MC.NOME_ESTADO,
                                       MC.NOME_PAIS
                                FROM LY_ALUNO A (NOLOCK)
                                JOIN LY_PESSOA P (NOLOCK)
                                    ON A.PESSOA = P.PESSOA
                                JOIN HADES.DBO.HD_MUNICIPIO_CERTIFICACAO MC (NOLOCK)
                                    ON P.MUNICIPIO_NASC = CAST(MC.ID_MUNICIPIO AS VARCHAR(20))
                                WHERE A.ALUNO = @ALUNO";
                
                contextQuery.Parameters.Add("ALUNO", SqlDbType.VarChar, aluno);
                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                throw new Exception(ex.Message);
            }
            finally
            {
                ctx.Dispose();
            }
            return dt;
        }

        public string ObtemSituacaoAlunoPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            string situacaoAluno = string.Empty;

            contextQuery.Command = @" SELECT SIT_ALUNO
                            FROM    LY_ALUNO
                            WHERE   ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            situacaoAluno = ctx.GetReturnValue<string>(contextQuery);

            return situacaoAluno;
        }

        public bool PossuiNecessidadeEspecialPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            string necessidadeEspecial = string.Empty;
            bool possui = false;

            try
            {
                possui = this.PossuiNecessidadeEspecialPor(ctx, aluno);
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
            finally
            {
                ctx.Dispose();
            }
        }

        public bool PossuiNecessidadeEspecialPor(DataContext ctx, string aluno)
        {
            string necessidadeEspecial = string.Empty;
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
                 {
                     Command = @" SELECT  COUNT(*)
                            FROM    LY_ALUNO A
                                    INNER JOIN DBO.LY_PESSOA P ON A.PESSOA = P.PESSOA
                                    LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID
                            WHERE   ALUNO = @ALUNO 
                                    AND (P.NECESSIDADEESPECIALID IS NOT NULL AND P.NECESSIDADEESPECIALID <> 30)"
                 };

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void InsereAlunoPreCadastro(DataContext contexto, int preCadastroAlunoId, string aluno, string curriculo, string usuarioResponsavel, int controlevagaid)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_ALUNO 
                                                (ALUNO, 
                                                 CURSO, 
                                                 TURNO, 
                                                 CURRICULO, 
                                                 SERIE, 
                                                 ANO_INGRESSO, 
                                                 SEM_INGRESSO, 
                                                 SIT_ALUNO, 
                                                 UNIDADE_FISICA, 
                                                 E_MAIL_INTERNO, 
                                                 PESSOA, 
                                                 UNIDADE_ENSINO, 
                                                 NUMINSCRICAO, 
                                                 REDE_ENSINO_ORIGEM,
                                                 USUARIO, 
                                                 DT_CADASTRO, 
                                                 DATAALTERACAO,
                                                 TIPO_INGRESSO) 
                                    SELECT @ALUNO ,
                                           CV.CURSO, 
                                           CV.TURNO, 
                                           @CURRICULO, 
                                           CV.SERIE, 
                                           CV.ANO, 
                                           CV.PERIODO, 
                                           @SITALUNO ,
                                           CV.CENSO, 
                                           PC.EMAIL, 
                                           PC.PESSOAID, 
                                           CV.CENSO, 
                                           I.NUMEROINSCRICAO, 
                                           ISNULL(I.REDEORIGEMINSCRICAO, PC.REDEORIGEM) as REDEORIGEM,
                                           @USUARIO, 
                                           @DT_CADASTRO, 
                                           @DATAALTERACAO,
                                           'Outros'
                                    FROM   MATRICULA.OPCAOINSCRICAO O ( NOLOCK) 
                                           INNER JOIN TCE_CONTROLE_VAGA CV ( NOLOCK) 
                                                   ON O.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA 
                                           INNER JOIN MATRICULA.INSCRICAOALUNO I ( NOLOCK) 
                                                   ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID 
                                           INNER JOIN MATRICULA.PRECADASTROALUNO PC ( NOLOCK) 
                                                   ON I.PRECADASTROALUNOID = PC.PRECADASTROALUNOID
                                   WHERE PC.PRECADASTROALUNOID = @PRECADASTROALUNOID 
                                   AND O.CONTROLEVAGAID = @CONTROLEVAGAID";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, curriculo);
            contextQuery.Parameters.Add("@SITALUNO", TechneDbType.T_SIT_ALUNO, "Ativo");
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DT_CADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controlevagaid);

            contexto.ApplyModifications(contextQuery);
        }

        public void InsereEncaminhamentoEspecial(DataContext contexto, DTOs.DadosEncaminhamentoEspecial dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_ALUNO 
                                                (ALUNO, 
                                                 CURSO, 
                                                 TURNO, 
                                                 CURRICULO, 
                                                 SERIE, 
                                                 ANO_INGRESSO, 
                                                 SEM_INGRESSO, 
                                                 SIT_ALUNO, 
                                                 UNIDADE_FISICA, 
                                                 E_MAIL_INTERNO, 
                                                 PESSOA, 
                                                 UNIDADE_ENSINO, 
                                                 USUARIO, 
                                                 DT_CADASTRO, 
                                                 DATAALTERACAO,
                                                 TIPO_INGRESSO) 
                                    VALUES( @ALUNO ,
                                           @CURSO, 
                                           @TURNO, 
                                           @CURRICULO, 
                                           @SERIE, 
                                           @ANO, 
                                           @PERIODO, 
                                           @SITALUNO ,
                                           @CENSO, 
                                           NULL, 
                                           @PESSOAID, 
                                           @CENSO, 
                                           @USUARIO, 
                                           @DT_CADASTRO, 
                                           @DATAALTERACAO,
                                           'Outros') ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, dados.PessoaAluno.Aluno);
            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, dados.Curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, dados.Turno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, dados.Curriculo);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, dados.Serie);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, dados.Ano);
            contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, dados.Periodo);
            contextQuery.Parameters.Add("@SITALUNO", TechneDbType.T_SIT_ALUNO, "Ativo");
            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dados.Censo);
            contextQuery.Parameters.Add("@PESSOAID", TechneDbType.T_NUMERO, dados.Pessoa);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, dados.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DT_CADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaEncaminhamentoEspecial(DataContext contexto, DadosEncaminhamentoEspecial dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_ALUNO
									SET CURSO = @CURSO, 
                                                 TURNO = @TURNO, 
                                                 CURRICULO = @CURRICULO, 
                                                 SERIE = @SERIE,                                                   
                                                 SIT_ALUNO = @SITALUNO, 
                                                 UNIDADE_FISICA = @CENSO, 
                                                 UNIDADE_ENSINO = @CENSO, 
                                                 USUARIO = @USUARIO, 
                                                 STAMP_ATUALIZACAO = @DATAALTERACAO,
                                                 DATAALTERACAO = @DATAALTERACAO  
									WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, dados.PessoaAluno.Aluno);
            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, dados.Curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, dados.Turno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, dados.Curriculo);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, dados.Serie);
            contextQuery.Parameters.Add("@SITALUNO", TechneDbType.T_SIT_ALUNO, "Ativo");
            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dados.Censo);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, dados.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaAlunoDuplicidade(DataContext contexto, DadosDuplicidadeAluno dadosDuplicidadeAluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_ALUNO
									SET CURSO = @CURSO, 
                                                 TURNO = @TURNO, 
                                                 CURRICULO = @CURRICULO, 
                                                 SERIE = @SERIE,                                                   
                                                 SIT_ALUNO = @SITALUNO, 
                                                 UNIDADE_FISICA = @CENSO, 
                                                 UNIDADE_ENSINO = @CENSO, 
                                                 USUARIO = @USUARIO, 
                                                 STAMP_ATUALIZACAO = @DATAALTERACAO,
                                                 DATAALTERACAO = @DATAALTERACAO  
									WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, dadosDuplicidadeAluno.MatriculaCorreta);
            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, dadosDuplicidadeAluno.Curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, dadosDuplicidadeAluno.Turno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, dadosDuplicidadeAluno.Curriculo);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, dadosDuplicidadeAluno.Serie);
            contextQuery.Parameters.Add("@SITALUNO", TechneDbType.T_SIT_ALUNO, "Ativo");
            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, dadosDuplicidadeAluno.Censo);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, dadosDuplicidadeAluno.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaEscola(DataContext contexto, string aluno, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_ALUNO
									SET UNIDADE_FISICA = @CENSO, 
                                        UNIDADE_ENSINO = @CENSO 
									WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaDadosEnturmacao(DataContext contexto, string aluno, string curriculo, string usuarioResponsavel, int controleVagaId, int opcapInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE A
									        SET  CURSO = t.CURSO, 
                                                 TURNO = t.TURNO, 
                                                 CURRICULO = @CURRICULO, 
                                                 SERIE = t.SERIE, 
                                                 USUARIO = @USUARIO, 
                                                 STAMP_ATUALIZACAO = @DATAALTERACAO,
                                                 DATAALTERACAO = @DATAALTERACAO
								FROM (SELECT CV.CURSO, 
													CV.TURNO,                                                 
													CV.SERIE,  
													CV.CENSO, 
													PC.EMAIL,
													ISNULL(I.REDEORIGEMINSCRICAO, PC.REDEORIGEM) as REDEORIGEM,
													I.NUMEROINSCRICAO
									FROM   MATRICULA.OPCAOINSCRICAO O ( NOLOCK) 
											INNER JOIN TCE_CONTROLE_VAGA CV ( NOLOCK) 
													ON O.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA 
											INNER JOIN MATRICULA.INSCRICAOALUNO I ( NOLOCK) 
													ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID 
											INNER JOIN MATRICULA.PRECADASTROALUNO PC ( NOLOCK) 
													ON I.PRECADASTROALUNOID = PC.PRECADASTROALUNOID
									WHERE O.OPCAOINSCRICAOID = @OPCAOINSCRICAOID
									        AND O.CONTROLEVAGAID = @CONTROLEVAGAID ) as T
					            INNER JOIN LY_ALUNO A on a.ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, curriculo);
            contextQuery.Parameters.Add("@SITALUNO", TechneDbType.T_SIT_ALUNO, "Ativo");
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controleVagaId);
            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcapInscricaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaAlunoPreCadastro(DataContext contexto, string aluno, string curriculo, string usuarioResponsavel, int controleVagaId, int opcapInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE A
									        SET  SIT_ALUNO = @SITALUNO, 
                                                 E_MAIL_INTERNO = t.EMAIL, 
												 NUMINSCRICAO = t.NUMEROINSCRICAO,
                                                 USUARIO = @USUARIO, 
                                                 STAMP_ATUALIZACAO = @DATAALTERACAO,
                                                 DATAALTERACAO = @DATAALTERACAO
								FROM (SELECT CV.CURSO, 
													CV.TURNO,                                                 
													CV.SERIE,  
													CV.CENSO, 
													PC.EMAIL,
													ISNULL(I.REDEORIGEMINSCRICAO, PC.REDEORIGEM) as REDEORIGEM,
													I.NUMEROINSCRICAO
									FROM   MATRICULA.OPCAOINSCRICAO O ( NOLOCK) 
											INNER JOIN TCE_CONTROLE_VAGA CV ( NOLOCK) 
													ON O.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA 
											INNER JOIN MATRICULA.INSCRICAOALUNO I ( NOLOCK) 
													ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID 
											INNER JOIN MATRICULA.PRECADASTROALUNO PC ( NOLOCK) 
													ON I.PRECADASTROALUNOID = PC.PRECADASTROALUNOID
									WHERE O.OPCAOINSCRICAOID = @OPCAOINSCRICAOID
									        AND O.CONTROLEVAGAID = @CONTROLEVAGAID ) as T
					            INNER JOIN LY_ALUNO A on a.ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, curriculo);
            contextQuery.Parameters.Add("@SITALUNO", TechneDbType.T_SIT_ALUNO, "Ativo");
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controleVagaId);
            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcapInscricaoId);

            contexto.ApplyModifications(contextQuery);
        }

        private void Insere(DataContext context, LyAluno aluno)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                this.GeraCodigoAluno(context, aluno);
                var contextQuery = new ContextQuery
                {
                    Command = @" INSERT INTO DBO.LY_ALUNO 
                            ( 
                                        ALUNO, 
                                        CURSO, 
                                        TURNO, 
                                        CURRICULO, 
                                        SERIE, 
                                        TIPO_INGRESSO, 
                                        ANO_INGRESSO, 
                                        SEM_INGRESSO, 
                                        SIT_ALUNO, 
                                        UNIDADE_FISICA, 
                                        E_MAIL_INTERNO, 
                                        PESSOA, 
                                        OUTRA_FACULDADE, 
                                        UNIDADE_ENSINO, 
                                        NUMINSCRICAO, 
                                        TIPO_ENSINO_PROFISSIONALIZANTE, 
                                        REDE_ENSINO_ORIGEM, 
                                        TEMPO_AFASTAMENTO_REDE,
			                            DATAATUALIZACAOEMAILINTERNO,
			                            DT_CADASTRO,
			                            USUARIO,
			                            DATAALTERACAO
                            ) 
                            VALUES 
                            ( 
                                        @ALUNO, 
                                        @CURSO, 
                                        @TURNO, 
                                        @CURRICULO, 
                                        @SERIE, 
                                        @TIPO_INGRESSO, 
                                        @ANO_INGRESSO, 
                                        @SEM_INGRESSO, 
                                        @SIT_ALUNO, 
                                        @UNIDADE_FISICA, 
                                        @E_MAIL_INTERNO, 
                                        @PESSOA, 
                                        @OUTRA_FACULDADE, 
                                        @UNIDADE_ENSINO, 
                                        @NUMINSCRICAO, 
                                        @TIPO_ENSINO_PROFISSIONALIZANTE, 
                                        @REDE_ENSINO_ORIGEM, 
                                        @TEMPO_AFASTAMENTO_REDE, 
                                        @DATAATUALIZACAOEMAILINTERNO,
			                            @DT_CADASTRO,
			                            @USUARIO,
			                            @DATAALTERACAO 
                            ) "
                };

                contextQuery.Parameters.Add("@ALUNO ", aluno.Aluno);
                contextQuery.Parameters.Add("@CURSO", aluno.Curso);
                contextQuery.Parameters.Add("@TURNO", aluno.Turno);
                contextQuery.Parameters.Add("@CURRICULO", aluno.Curriculo);
                contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, aluno.Serie);
                contextQuery.Parameters.Add("@TIPO_INGRESSO ", aluno.TipoIngresso);
                contextQuery.Parameters.Add("@ANO_INGRESSO", TechneDbType.T_ANO, aluno.AnoIngresso);
                contextQuery.Parameters.Add("@SEM_INGRESSO", TechneDbType.T_SEMESTRE2, aluno.SemIngresso);
                contextQuery.Parameters.Add("@SIT_ALUNO", aluno.SitAluno);
                contextQuery.Parameters.Add("@UNIDADE_FISICA", aluno.UnidadeFisica);
                contextQuery.Parameters.Add("@E_MAIL_INTERNO", aluno.EMailInterno);
                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, aluno.Pessoa);
                contextQuery.Parameters.Add("@OUTRA_FACULDADE", aluno.OutraFaculdade);
                contextQuery.Parameters.Add("@UNIDADE_ENSINO", aluno.UnidadeEnsino);
                contextQuery.Parameters.Add("@NUMINSCRICAO", aluno.Numinscricao);
                contextQuery.Parameters.Add("@TIPO_ENSINO_PROFISSIONALIZANTE", aluno.TipoEnsinoProfissionalizante);
                contextQuery.Parameters.Add("@REDE_ENSINO_ORIGEM", aluno.RedeEnsinoOrigem);
                contextQuery.Parameters.Add("@TEMPO_AFASTAMENTO_REDE", aluno.TempoAfastamentoRede);
                contextQuery.Parameters.Add("@DATAATUALIZACAOEMAILINTERNO", aluno.DataAtualizacaoEmailInterno);
                contextQuery.Parameters.Add("@DT_CADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@USUARIO", aluno.Usuario);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

                context.ApplyModifications(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
                throw;
            }
        }

        public static LyAluno Carregar(string idAluno)
        {
            var aluno = new LyAluno();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT * FROM LY_ALUNO
                            WHERE ALUNO = @ID "
                };

                contextQuery.Parameters.Add("@ID", idAluno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        aluno.Aluno = idAluno;
                        aluno.Curso = Convert.ToString(reader["CURSO"]);
                        aluno.Turno = Convert.ToString(reader["TURNO"]);
                        aluno.Curriculo = Convert.ToString(reader["CURRICULO"]);
                        aluno.Serie = Convert.ToDecimal(reader["SERIE"]);
                        aluno.TipoIngresso = Convert.ToString(reader["TIPO_INGRESSO"]);
                        aluno.AnoIngresso = Convert.ToDecimal(reader["ANO_INGRESSO"]);
                        aluno.SemIngresso = Convert.ToDecimal(reader["SEM_INGRESSO"]);
                        aluno.SitAluno = Convert.ToString(reader["SIT_ALUNO"]);
                        aluno.UnidadeFisica = Convert.ToString(reader["UNIDADE_FISICA"]);
                        aluno.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                        aluno.OutraFaculdade = Convert.ToString(reader["OUTRA_FACULDADE"]);
                        aluno.UnidadeEnsino = Convert.ToString(reader["UNIDADE_ENSINO"]);
                        aluno.Numinscricao = Convert.ToString(reader["NUMINSCRICAO"]);
                        aluno.TipoEnsinoProfissionalizante = Convert.ToString(reader["TIPO_ENSINO_PROFISSIONALIZANTE"]);
                        aluno.RedeEnsinoOrigem = Convert.ToString(reader["REDE_ENSINO_ORIGEM"]);
                        aluno.EMailInterno = Convert.ToString(reader["E_MAIL_INTERNO"]);

                        if (reader["DT_CADASTRO"] != DBNull.Value)
                        {
                            aluno.DtCadastro = Convert.ToDateTime(reader["DT_CADASTRO"]);
                        }

                        if (reader["TEMPO_AFASTAMENTO_REDE"] != DBNull.Value)
                        {
                            aluno.TempoAfastamentoRede = Convert.ToInt32(reader["TEMPO_AFASTAMENTO_REDE"]);
                        }

                        if (reader["DATAATUALIZACAOEMAILINTERNO"] != DBNull.Value)
                        {
                            aluno.DataAtualizacaoEmailInterno = Convert.ToDateTime(reader["DataAtualizacaoEmailInterno"]);
                        }

                        if (reader["DATA_EM_SUSPENSAO"] != DBNull.Value)
                        {
                            aluno.DataEmSuspensao = Convert.ToDateTime(reader["DATA_EM_SUSPENSAO"]);
                        }

                        if (reader["SUSPENSO"] != DBNull.Value)
                        {
                            aluno.Suspenso = Convert.ToBoolean(reader["SUSPENSO"]);
                        }
                    }
                }
                return aluno;
            }
        }

        public void SalvaTransporte(LyFlPessoa flPessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.FlPessoa rnFlPessoa = new FlPessoa();

            try
            {
                // verifica se a pessoa esta dentro da tabela LY_FL_PESSOA
                if (rnFlPessoa.ExistePor(contexto, flPessoa.Pessoa))
                {
                    //Alterar flPessoa
                    FlPessoa.AlterarTransporte(flPessoa, contexto);
                }
                else
                {
                    if (flPessoa.Pessoa != 0)
                    {    //inseri flPessoa
                        FlPessoa.InserirTransporte(flPessoa, contexto);
                    }
                    else
                    {
                        throw new Exception("Código da Pessoa não localizado para cadastrar um Field.");
                    }
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

        public void Atualiza(LyAluno aluno, LyPessoa pessoa, LyFlPessoa flPessoa, LyFotoPessoa foto, List<RN.Entidades.DeclaracaoAusencia> declaracoesAusencia, List<RN.Entidades.PessoaRecursoAplicacaoProva> listPessoaRecursoAplicacaoProva, List<int> listRecursoNecessidadeEspecial, RN.RecursosHumanos.Entidades.GoogleEducation googleEducation, bool compartilhada, string unidadeDestino, string unidadeOrigem, List<int> listaTranstornoAprendizagem)
        {
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            RN.Compartilhada rnCompartilhada = new Compartilhada();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial rnAlunoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial();
            RN.PessoaTranstornoAprendizagem rnPessoaTranstornoAprendizagem = new PessoaTranstornoAprendizagem();
            Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();

            try
            {
                //Alterar pessoa
                rnPessoa.AtualizaPessoaAluno(contexto, pessoa);

                // verifica se a pessoa esta dentro da tabela LY_FL_PESSOA
                flPessoa.Pessoa = pessoa.Pessoa;
                if (rnFlPessoa.ExistePor(contexto, pessoa.Pessoa))
                {
                    //Alterar flPessoa
                    FlPessoa.Alterar(flPessoa, contexto);
                }
                else
                {
                    if (flPessoa.Pessoa != 0)
                    {    //inseri flPessoa
                        FlPessoa.Inserir(flPessoa, contexto);
                    }
                    else
                    {
                        throw new Exception("Código da Pessoa não localizado para cadastrar um Field.");
                    }
                }

                //Alterar aluno
                this.Atualiza(contexto, aluno);

                //Insere dados Compartilhada
                if (compartilhada)
                {
                    //Busca unidada compartilhada
                    string unidadeCompartilhada = rnCompartilhada.RetornaUnidadeCompartilhada(contexto, unidadeDestino, unidadeOrigem);

                    RN.UnidadeEnsinoCompartilhada.Entidades.Aluno_UnidadeEnsinoCompartilhada alunoUnidadeCompartilhada = new RN.UnidadeEnsinoCompartilhada.Entidades.Aluno_UnidadeEnsinoCompartilhada();

                    alunoUnidadeCompartilhada.AlunoId = aluno.Aluno;
                    alunoUnidadeCompartilhada.UnidadeEnsinoCompartilhadaId = unidadeCompartilhada;

                    RN.UnidadeEnsinoCompartilhada.Aluno_UnidadeEnsinoCompartilhada.InsereAluno_UnidadeEnsinoCompartilhada(alunoUnidadeCompartilhada, contexto);
                }

                if (foto.Foto != null)
                {
                    foto.Pessoa = pessoa.Pessoa;

                    //verificar se já tem uma foto para esta pessoa
                    if (FotoPessoa.VerificarFoto(pessoa.Pessoa))
                    {
                        FotoPessoa.Alterar(foto, contexto);
                    }
                    else
                    {
                        FotoPessoa.Inserir(foto, contexto);
                    }
                }

                //Inserir dados na tabela de Declaracao de ausencia
                //1º Remove todas as declaracao anteriores
                RN.DeclaracaoAusencia.RemoverPorAluno(aluno.Aluno, contexto);

                foreach (var declaracaoAusencia in declaracoesAusencia)
                {
                    //2º Insere as declarações necessárias
                    declaracaoAusencia.AlunoId = aluno.Aluno;
                    declaracaoAusencia.Matricula = aluno.Usuario;
                    RN.DeclaracaoAusencia.Inserir(declaracaoAusencia, contexto);
                }

                //Recurso Aplicação Prova - Necessidade Especial
                RN.PessoaRecursoAplicacaoProva.InserirPessoaRecursoAplicacaoProva(listPessoaRecursoAplicacaoProva, Convert.ToInt32(aluno.Pessoa), contexto);

                //Altera Recursos Necessidade Especial
                rnAlunoRecursoNecessidadeEspecial.AtualizaLista(contexto, listRecursoNecessidadeEspecial, aluno.Aluno, aluno.Usuario);

                //Verifica de tem email google
                if (googleEducation != null && !googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                {
                    googleEducation.Aluno = aluno.Aluno;
                    googleEducation.Pessoa = aluno.Pessoa.Value;
                    googleEducation.UsuarioId = aluno.Usuario;
                    rnGoogleEducation.Salva(contexto, googleEducation);
                }

                //Transtornos de aprendizagem
                rnPessoaTranstornoAprendizagem.AtualizaLista(contexto, listaTranstornoAprendizagem, Convert.ToDecimal(aluno.Pessoa), aluno.Usuario);
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

        public void AtualizaSuspensao(DataContext contexto, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_ALUNO 
                                        SET     SIT_ALUNO = 'Suspenso',
		                                        DATAALTERACAO = GETDATE()
                                        FROM LY_ALUNO A 
	                                        INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = A.ALUNO
                                        where HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";

            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            contexto.ApplyModifications(contextQuery);
        }

        private void Atualiza(DataContext context, LyAluno aluno)
        {
            var contextQuery = new ContextQuery
            {
                Command = @"UPDATE  LY_ALUNO
                            SET                                    
                                    E_MAIL_INTERNO = @E_MAIL_INTERNO,
                                    TIPO_INGRESSO = @TIPO_INGRESSO,                                    
                                    NUMINSCRICAO = @NUMINSCRICAO,
                                    REDE_ENSINO_ORIGEM = @REDE_ENSINO_ORIGEM,
                                    TIPO_ENSINO_PROFISSIONALIZANTE = @TIPO_ENSINO_PROFISSIONALIZANTE,                                    
                                    DATAATUALIZACAOEMAILINTERNO = @DATAATUALIZACAOEMAILINTERNO,
		                            USUARIO = @USUARIO,
		                            DATAALTERACAO = @DATAALTERACAO
                            WHERE   ALUNO = @ALUNO"
            };

            contextQuery.Parameters.Add("@ALUNO ", aluno.Aluno);
            contextQuery.Parameters.Add("@TIPO_INGRESSO ", aluno.TipoIngresso);
            contextQuery.Parameters.Add("@NUMINSCRICAO", aluno.Numinscricao);
            contextQuery.Parameters.Add("@REDE_ENSINO_ORIGEM", aluno.RedeEnsinoOrigem);
            contextQuery.Parameters.Add("@TIPO_ENSINO_PROFISSIONALIZANTE", aluno.TipoEnsinoProfissionalizante);
            contextQuery.Parameters.Add("@E_MAIL_INTERNO", aluno.EMailInterno);
            contextQuery.Parameters.Add("@DATAATUALIZACAOEMAILINTERNO", aluno.DataAtualizacaoEmailInterno);
            contextQuery.Parameters.Add("@USUARIO", aluno.Usuario);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            context.ApplyModifications(contextQuery);
        }

        public void AtualizaDataEmail(DataContext context, string aluno, DateTime dataAtualizacaoEmail, string usuarioId)
        {
            var contextQuery = new ContextQuery
            {
                Command = @"UPDATE  LY_ALUNO
                            SET     DATAATUALIZACAOEMAILINTERNO = @DATAATUALIZACAOEMAILINTERNO,
		                            USUARIO = @USUARIO,
		                            DATAALTERACAO = @DATAALTERACAO
                            WHERE   ALUNO = @ALUNO"
            };

            contextQuery.Parameters.Add("@ALUNO ", aluno);
            contextQuery.Parameters.Add("@DATAATUALIZACAOEMAILINTERNO", dataAtualizacaoEmail);
            contextQuery.Parameters.Add("@USUARIO", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            context.ApplyModifications(contextQuery);
        }

        public ValidacaoDados Valida(LyAluno aluno, LyPessoa pessoa, LyFlPessoa flPessoa, LyFotoPessoa foto, List<RN.Entidades.DeclaracaoAusencia> declaracoesAusencia, List<RN.Entidades.PessoaRecursoAplicacaoProva> listPessoaRecursoAplicacaoProva, List<int> listRecursoNecessidadeEspecial, bool cadastro, bool mudouCurso, bool compartilhada, string unidadeDestino, string unidadeOrigem, string SemLocalizacaoDiferenciada, RN.RecursosHumanos.Entidades.GoogleEducation googleEducation, List<int> listaTranstornoAprendizagem)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Compartilhada rnCompartilhada = new Compartilhada();
            RN.EventoGeral rnEventoGeral = new EventoGeral();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Validacao rnValidacao = new Validacao();
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.NecessidadeEspecial();
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
            RN.RecursosHumanos.NomeSemValidacao rnNomeSemValidacao = new Techne.Lyceum.RN.RecursosHumanos.NomeSemValidacao();
            RestricaoIdadeSerie rnRestricaoIdadeSerie = new RestricaoIdadeSerie();
            TceRestricaoIdadeSerie restricao = new TceRestricaoIdadeSerie();
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            string cpf = string.Empty;
            Regex regex = new Regex(@"\s{2,}");
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int idade = 0;
            decimal numeroDecimalLat;
            decimal numeroDecimalLong;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aluno == null || pessoa == null || flPessoa == null)
            {
                return validacaoDados;
            }

            if (cadastro)
            {
                aluno.Aluno = string.Empty;
                aluno.Pessoa = 0;
                pessoa.Pessoa = 0;
            }
            else
            {
                if (aluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo ALUNO é obrigatório.");
                }

                if (pessoa.Pessoa <= 0)
                {
                    mensagens.Add("Campo PESSOA é obrigatório.");
                }
                else
                {
                    aluno.Pessoa = Convert.ToDecimal(pessoa.Pessoa);
                }
            }

            if (aluno.Usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (compartilhada)
            {
                if (unidadeDestino.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo UNIDADE DESTINO é obrigatório.");
                }
                else
                {
                    if (unidadeDestino != aluno.UnidadeEnsino)
                    {
                        mensagens.Add("A unidade de ensino do aluno de compartilhada deve ser igual a unidade de destino da compartilhada.");
                    }
                }

                if (unidadeOrigem.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo UNIDADE ORIGEM é obrigatório.");
                }
            }

            //validação da foto
            if (foto.Foto != null)
            {
                var ms = new MemoryStream(foto.Foto);
                var imagem = System.Drawing.Image.FromStream(ms);

                if (!System.Drawing.Imaging.ImageFormat.Jpeg.Equals(imagem.RawFormat))
                {
                    mensagens.Add("Formato da FOTO Inválido!");
                }

                //Verifica tamanho do arquivo - Todas as fotos devem ter entre 8 e 32Kb
                int tamanhoByte = Buffer.ByteLength(foto.Foto);
                if (tamanhoByte < 8192 || tamanhoByte > 32768)
                {
                    mensagens.Add("A foto devem ter tamanho entre 8 e 32Kb.");
                }

                // As fotos precisam ser quadradas, com no máximo 400px de largura.
                int width = imagem.Width;
                int height = imagem.Height;
                if (width != height)
                {
                    mensagens.Add("A foto precisa ser quadrada.");
                }
                else if (width > 400)
                {
                    mensagens.Add("A foto precisa ter no máximo 400px de largura.");
                }
            }

            // valida campos preenchidos da aba dados pessoais

            if (pessoa.Nome_compl.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO ALUNO é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(pessoa.Pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Nome);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != pessoa.Nome_compl.ToUpper())
                {
                    /// Validaçoes de nome do aluno 
                    if (pessoa.Nome_compl.Length < 5)
                    {
                        mensagens.Add("Campo NOME DO ALUNO deve conter pelo menos cinco letras!");
                    }

                    if (!string.IsNullOrEmpty(pessoa.Nome_compl)
                        && !Validacao.SomenteLetras(pessoa.Nome_compl))
                    {
                        mensagens.Add("Campo NOME DO ALUNO não pode conter números.");
                    }

                    var palavras = pessoa.Nome_compl.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(pessoa.Nome_compl, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(pessoa.Nome_compl, new PalavrasProibidasEmNomes());
                    if (Validacao.contemNumeros(pessoa.Nome_compl) == false)
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME.");
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
                    string NomeCompl = regex.Replace(pessoa.Nome_compl.Trim().ToUpper(), " ");
                    var contemApostrofeRep = Validacao.substitueApostrofe(NomeCompl);
                    if (contemApostrofeRep)
                    {
                        mensagens.Add("Campo NOME DO ALUNO possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }
            }

            if (pessoa.Dt_nasc == null || pessoa.Dt_nasc == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE NASCIMENTO é obrigatório.");
            }
            else
            {
                idade = Utils.CalcularIdade(Convert.ToDateTime(pessoa.Dt_nasc));

                if (idade > 110)
                {
                    mensagens.Add("A idade não pode ser superior a 110 anos. Favor verificar a Data de Nascimento.");
                }
            }

            if (pessoa.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SEXO é obrigatório.");
            }

            if (string.IsNullOrEmpty(pessoa.Etnia) || pessoa.Etnia == "<Nenhum>")
            {
                mensagens.Add("Campo ETNIA é obrigatório!");
            }
            else
            {
                if (pessoa.Etnia == "Índigena")
                {
                    if (flPessoa.FlField21.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo POVO ÍNDIGENA é obrigatório quando o campo COR/RAÇA for Índigena.");
                    }
                }
                else
                {
                    //Verifica se foi informado o codigo do povo indigena e se a cor/raça não é indigena
                    if (!flPessoa.FlField21.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Apenas pode ser informado o campo POVO ÍNDIGENA quando o campo COR/RAÇA for Índigena.");
                    }
                }
            }

            if (pessoa.NecessidadeEspecialId == null || pessoa.NecessidadeEspecialId <= 0)
            {
                mensagens.Add("Campo NECESSIDADE ESPECIAL é obrigatório.");
            }
            else
            {
                if (pessoa.NecessidadeEspecialId != 30) //"Não possui."
                {
                    if (!aluno.DeclaroNecessidadeEspecial)
                    {
                        mensagens.Add("Se o Aluno possuir necessidade Especial é necessário que a informação de declaração esteja marcada.");
                    }

                    //Verifica se marcou opção nenhum
                    if (!aluno.NenhumRecursoAplicacaoProva)
                    {
                        //Senão cobra um recurso
                        if (listPessoaRecursoAplicacaoProva == null || listPessoaRecursoAplicacaoProva.Count == 0)
                        {
                            mensagens.Add("É Obrigatório informar ao menos 1 recurso para aplicação de prova.");
                        }
                    }
                }
            }

            if (flPessoa.FlField22.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TRANSTORNO(S) QUE IMPACTA(M) O DESENVOLVIMENTO DA APRENDIZAGEM é obrigatório.");
            }
            else if (flPessoa.FlField22 == "S") //Verifica se marcou sim
            {
                //Cobra ao menos 1 transtorno
                if (listaTranstornoAprendizagem == null || listaTranstornoAprendizagem.Count == 0)
                {
                    mensagens.Add("É Obrigatório marcar ao menos 1 TRANSTORNO(S) QUE IMPACTA(M) O DESENVOLVIMENTO DA APRENDIZAGEM.");
                }
            }

            if (flPessoa.FlField23.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo COMUNIDADE QUILOMBOLA OU INDÍGENA é obrigatório.");
            }

            if (pessoa.Est_civil.IsNullOrEmptyOrWhiteSpace() || pessoa.Est_civil == "<Nenhum>")
            {
                mensagens.Add("Campo ESTADO CIVIL é obrigatório.");
            }

            if (pessoa.Pais_nasc.IsNullOrEmptyOrWhiteSpace() || pessoa.Pais_nasc == "<Nenhum>")
            {
                mensagens.Add("Campo PAIS DE NASCIMENTO é obrigatório.");
            }

            if (pessoa.Nacionalidade.IsNullOrEmptyOrWhiteSpace() || pessoa.Nacionalidade == "<Nenhum>")
            {
                mensagens.Add("Campo NACIONALIDADE é obrigatório.");
            }
            else
            {
                if (pessoa.Nacionalidade == "BRASILEIRA")
                {
                    //Apenas validar Naturalidade caso o Pais de nascimento seja o brasil
                    if (pessoa.Municipio_nasc.IsNullOrEmptyOrWhiteSpace() || pessoa.Municipio_nasc == "<Nenhum>")
                    {
                        mensagens.Add("Campo NATURALIDADE é obrigatório.");
                    }
                }

                if (pessoa.Pais_nasc.IsNullOrEmptyOrWhiteSpace() || pessoa.Pais_nasc == "<Nenhum>")
                {
                    if (pessoa.Pais_nasc == "1"
                        && pessoa.Nacionalidade != "BRASILEIRA")
                    {
                        mensagens.Add("Campo PAÍS DE NASCIMENTO não pode ser Brasil com a NACIONALIDADE diferente de brasileira.");
                    }

                    if (pessoa.Pais_nasc != "1"
                        && pessoa.Nacionalidade == "BRASILEIRA")
                    {
                        mensagens.Add("Campo PAÍS DE NASCIMENTO não pode ser diferente de Brasil e a NACIONALIDADE ser brasileira.");
                    }
                }
            }

            if (pessoa.Credo.IsNullOrEmptyOrWhiteSpace() || pessoa.Credo == "<Nenhum>")
            {
                mensagens.Add("Campo CREDO é obrigatório.");
            }

            if (pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(pessoa.Pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Mae);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != pessoa.NomeMae.ToUpper())
                {
                    /// Validaçoes de nome da mae

                    if (pessoa.NomeMae == "NÃO DECLARADA" && !aluno.DeclaroAusenciaMae)
                    {
                        mensagens.Add("Se o Nome da Mãe for Não Declarada é necessário que a informação de declaração esteja marcada.");
                    }

                    var palavras = pessoa.NomeMae.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(pessoa.NomeMae, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(pessoa.NomeMae, new PalavrasProibidasEmNomes());
                    if (!Validacao.contemNumeros(pessoa.NomeMae))
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DA MÃE.");
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
                    else if (!Validacao.SomenteLetras(pessoa.NomeMae))
                    {
                        mensagens.Add("Campo NOME DA MÃE não pode conter números.");
                    }

                    string NomeMae = regex.Replace(pessoa.NomeMae.Trim().ToUpper(), " ");
                    var contemApostrofeRepMae = Validacao.substitueApostrofe(NomeMae);
                    if (contemApostrofeRepMae)
                    {
                        mensagens.Add("Campo NOME DO MÃE possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }

                if (!string.IsNullOrEmpty(pessoa.MaeCpf))
                {
                    cpf = Utils.RetirarMascara(pessoa.MaeCpf);

                    if (!Validacao.ValidaCpf(cpf))
                    {
                        mensagens.Add("O CPF informado da Mãe não é válido!");
                    }
                }
            }

            if (pessoa.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO PAI é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(pessoa.Pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Pai);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != pessoa.NomePai.ToUpper())
                {
                    /// Validações de nome do pai
                    if (pessoa.NomePai == "NÃO DECLARADO" && !aluno.DeclaroAusenciaPai)
                    {
                        mensagens.Add("Se o Nome do Pai for Não Declarado é necessário que a informação de declaração esteja marcada.");
                    }

                    var palavras = pessoa.NomePai.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(pessoa.NomePai, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(pessoa.NomePai, new PalavrasProibidasEmNomes());
                    if (Validacao.contemNumeros(pessoa.NomePai) == false)
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DO PAI.");
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
                    else if (!string.IsNullOrEmpty(pessoa.NomePai) && !Validacao.SomenteLetras(pessoa.NomePai))
                    {
                        mensagens.Add("Campo NOME DO PAI não pode conter números.");
                    }

                    string NomePai = regex.Replace(pessoa.NomePai.Trim().ToUpper(), " ");
                    var contemApostrofeRepPai = Validacao.substitueApostrofe(NomePai);
                    if (contemApostrofeRepPai)
                    {
                        mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }

                if (!string.IsNullOrEmpty(pessoa.PaiCpf))
                {
                    cpf = Utils.RetirarMascara(pessoa.PaiCpf);

                    if (!Validacao.ValidaCpf(cpf))
                    {
                        mensagens.Add("O CPF informado do Pai não é válido!");
                    }
                }
            }

            if (!string.IsNullOrEmpty(pessoa.NomeMae) && !string.IsNullOrEmpty(pessoa.NomePai))
            {
                if (pessoa.NomeMae == pessoa.NomePai)
                {
                    mensagens.Add("Campo NOME DO PAI não pode ser idêntico ao NOME DA MÃE.");
                }
            }

            if (pessoa.Responsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo RESPONSÁVEL LEGAL é obrigatório.");
            }
            else
            {
                string[] tipo_resp = pessoa.Responsavel.Split(';');

                if (tipo_resp.Count() > 2)
                {
                    mensagens.Add("Somente poderá ter no máximo duas opções de Responsável Legal.");
                }

                if (pessoa.Responsavel.Contains("Próprio Aluno"))
                {
                    if (idade < 18)
                    {
                        mensagens.Add("Para o aluno ser Responsável é necessário ter mais que 18 anos.");
                    }
                }

                if (pessoa.Responsavel.Contains("Pai"))
                {
                    if (pessoa.NomePai.IsNullOrEmptyOrWhiteSpace() || pessoa.NomePai == "NÃO DECLARADO")
                    {
                        mensagens.Add("Campo Nome do Pai é de preenchimento obrigatório  e deve ser diferente de não declarado, quando escolhido como responsável.");
                    }
                }

                if (pessoa.Responsavel.Contains("Mãe"))
                {
                    if (pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace() || pessoa.NomeMae == "NÃO DECLARADA")
                    {
                        mensagens.Add("Campo Nome do Mãe é de preenchimento obrigatório e deve ser diferente de não declarada, quando escolhida como responsável.");
                    }
                }

                if (pessoa.Responsavel.Contains("Outros"))
                {
                    if (string.IsNullOrEmpty(pessoa.RespNomeCompl))
                    {
                        mensagens.Add("Campo Nome do Responsável Legal é de preenchimento obrigatório.");
                    }
                    if (pessoa.RespNomeCompl == pessoa.NomeMae || pessoa.RespNomeCompl == pessoa.NomePai)
                    {
                        mensagens.Add("Campo Nome do Responsável Legal não pode ser igual ao Nome da(o) Mãe/Pai.");
                    }

                    var palavras = pessoa.RespNomeCompl.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(pessoa.RespNomeCompl, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(pessoa.RespNomeCompl, new PalavrasProibidasEmNomes())
                                       || TextValidator.HasNumbers(pessoa.RespNomeCompl);

                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor, informar nome e sobrenome no campo NOME DO RESPONSÁVEL.");
                    }

                    if (contemRepeticao)
                    {
                        mensagens.Add("Campo NOME DO RESPONSÁVEL possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("Campo NOME DO RESPONSÁVEL possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }
            }

            if (pessoa.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório!");
            }
            else
            {
                var cep = Utils.RetirarMascara(pessoa.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (pessoa.End_municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO é obrigatório.");
            }

            if (pessoa.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório!");
            }
            else
            {
                if (pessoa.Endereco.Length > 50)
                {
                    mensagens.Add("Campo ENDEREÇO deve conter no máximo 50 caracteres!");
                }
            }

            if (pessoa.End_num.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO ENDEREÇO é obrigatório.");
            }

            if (pessoa.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }
            else
            {
                if (!Validacao.Bairro(pessoa.Bairro))
                {
                    mensagens.Add("Campo BAIRRO é inválido!");
                }
            }

            if (flPessoa.FlField01.IsNullOrEmptyOrWhiteSpace() || flPessoa.FlField01 == "<Nenhum>")
            {
                mensagens.Add("Campo LOCALIZAÇÃO/ZONA DE RESIDÊNCIA é obrigatório.");
            }

            //if (pessoa.Latitude.IsNullOrEmptyOrWhiteSpace())
            //{
            //    mensagens.Add("Campo LATITUDE é obrigatório.");
            //}
            //else
            if (!pessoa.Latitude.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(pessoa.Latitude.Replace(".", ","), out numeroDecimalLat))
                {
                    mensagens.Add("LATITUDE inválida.");
                }
                else
                {
                    var lat = Math.Abs(numeroDecimalLat);
                    if (lat < 20 || lat > 24)
                    {
                        mensagens.Add("Latitude fora do limite permitido. Favor verificar.");
                    }
                }
            }

            //if (pessoa.Longitude.IsNullOrEmptyOrWhiteSpace())
            //{
            //    mensagens.Add("Campo LONGITUDE é obrigatório.");
            //}
            //else
            if (!pessoa.Longitude.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(pessoa.Longitude.Replace(".", ","), out numeroDecimalLong))
                {
                    mensagens.Add("LONGITUDE inválida.");
                }
                else
                {
                    var longi = Math.Abs(numeroDecimalLong);
                    if (longi < 40 || longi > 45)
                    {
                        mensagens.Add("Longitude fora do limite permitido. Favor verificar.");
                    }
                }
            }

            if (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.AreaQuilombos != "N" && pessoa.AreaQuilombos != "S"))
            {
                mensagens.Add("O campo AREA DE QUILOMBOS é obrigatório com os Valores N ou S.");
            }

            if (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.TerraIndigena != "N" && pessoa.TerraIndigena != "S"))
            {
                mensagens.Add("O campo TERRA INDIGENA é obrigatório com os Valores N ou S.");
            }

            if (pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.AreaAssentamento != "N" && pessoa.AreaAssentamento != "S"))
            {
                mensagens.Add("O campo AREA DE ASSENTAMENTO é obrigatório com os Valores N ou S.");
            }

            if (SemLocalizacaoDiferenciada.IsNullOrEmptyOrWhiteSpace()
                || (SemLocalizacaoDiferenciada != "N" && SemLocalizacaoDiferenciada != "S"))
            {
                mensagens.Add("O campo SEM LOCALIZAÇÃO DIFERENCIADA é obrigatório com os Valores N ou S.");
            }
            else
            {
                if (SemLocalizacaoDiferenciada == "N")
                {
                    if ((pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaAssentamento == "N")
                        && (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() || pessoa.TerraIndigena == "N")
                        && (pessoa.AreaTradicional.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaTradicional == "N")
                        && (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaQuilombos == "N"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA é obrigatório.");
                    }
                }
                else
                {
                    if ((!pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaAssentamento == "S")
                        || (!pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() && pessoa.TerraIndigena == "S")
                        || (!pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaQuilombos == "S"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA não pode possuir outra marcação quando NÃO SE APLICA estiver selecionado.");
                    }
                }
            }

            if (!pessoa.Fone.IsNullOrEmptyOrWhiteSpace())
            {
                var telefone = Utils.RetirarMascara(pessoa.Fone);

                if (!Validacao.ValidaTelefoneComDDD(telefone) && !Validacao.ValidaCelularComDDD(telefone))
                {
                    mensagens.Add("Campo TELEFONE OU CELULAR é inválido.");
                }
            }

            if (!pessoa.Celular.IsNullOrEmptyOrWhiteSpace())
            {
                var celular = Utils.RetirarMascara(pessoa.Celular);

                if (!Validacao.ValidaCelularComDDD(celular))
                {
                    mensagens.Add("Campo CELULAR é inválido.!");
                }
            }

            if (!pessoa.E_mail.IsNullOrEmptyOrWhiteSpace())
            {
                aluno.EMailInterno = pessoa.E_mail;

                if (!Validacao.Email(pessoa.E_mail))
                {
                    mensagens.Add("Campo E-MAIL está em um formato incorreto!");
                }

                if (pessoa.E_mail.Contains("educacao.rj.gov.br"))
                {
                    mensagens.Add("Campo E-mail não pode ser do dominio educacao.rj.gov.br ou prof.educacao.rj.gov.br");
                }

                //Verifica se trocou o email
                if (aluno.EMailAnterior != pessoa.E_mail)
                {
                    if (aluno.EMailConfirmacao.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo E-mail deverá ser confirmado antes de salvar o cadastro.");
                    }

                    if (pessoa.E_mail != aluno.EMailConfirmacao)
                    {
                        mensagens.Add("Campo E-mail de Confirmação não é igual ao preenchido no campo E-mail.");
                    }
                }
            }

            if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
            {
                if (!googleEducation.Email.Contains("aluno.educa.rj.gov.br"))
                {
                    mensagens.Add("Campo E-mail Google for Education serão aceitos apenas e-mails do dominínio aluno.educa.rj.gov.br");
                }
            }
            if (!pessoa.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                cpf = Utils.RetirarMascara(pessoa.Cpf);

                if (!Validacao.ValidaCpf(cpf))
                {
                    mensagens.Add("O CPF informado não é válido!");
                }
                else
                {
                    string cpfMae = Utils.RetirarMascara(pessoa.MaeCpf);
                    string cpfPai = Utils.RetirarMascara(pessoa.PaiCpf);
                    string cpfOutros = Utils.RetirarMascara(pessoa.RespCpf);

                    if (cpf == cpfMae)
                    {
                        mensagens.Add("O CPF informado não pode ser igual ao cpf da mãe");
                    }

                    if (cpf == cpfPai)
                    {
                        mensagens.Add("O CPF informado não pode ser igual ao cpf do pai");
                    }

                    if (pessoa.Responsavel.Contains("Outros") && cpf == cpfOutros)
                    {
                        mensagens.Add("O CPF informado não pode ser igual ao cpf do responsável");
                    }
                }
            }

            #region Validações dos Campos de Documento
            bool documentoValido, iniciouMensagem, maisDeUmCampo;
            documentoValido = true;
            iniciouMensagem = maisDeUmCampo = false;
            System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
            System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
            mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");

            if (!pessoa.Rg_tipo.IsNullOrEmptyOrWhiteSpace() && pessoa.Rg_tipo != "<Nenhum>")
            {
                if (Convert.ToString(pessoa.Rg_num).IsNullOrEmptyOrWhiteSpace())
                {
                    documentoValido = false;
                    iniciouMensagem = true;
                    camposDocumento.Append("Número ");
                }
                else
                {
                    var rg = Utils.RetirarMascara(pessoa.Rg_num);

                    if (rg.Length < 5)
                    {
                        mensagens.Add("O NÚMERO DO DOCUMENTO deve conter no mínimo cinco dígitos!");
                    }

                    if (pessoa.Rg_tipo == "RG" && pessoa.Rg_emissor == "DETRAN" && pessoa.Rg_uf == "RJ")
                    {
                        if (!Validacao.ValidaNumerosInteirosPositivos(pessoa.Rg_num))
                        {
                            mensagens.Add("O número de documento do RG Detran deve conter só números inteiros.");
                        }
                    }
                }

                if (pessoa.Rg_tipo == "RG")
                {
                    if (pessoa.Rg_emissor != "CGPI/DIREX/DPF" && Convert.ToString(pessoa.Rg_uf).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Estado ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Estado ");
                        }
                    }

                    if (Convert.ToString(pessoa.Rg_emissor).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }

                    if (pessoa.Rg_dtexp == null)
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Data de Expedição ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Data de Expedição ");
                        }
                    }

                    if (pessoa.Rg_dtexp != null && pessoa.Rg_dtexp != DateTime.MinValue)
                    {
                        if (Convert.ToDateTime(pessoa.Dt_nasc).Date >= Convert.ToDateTime(pessoa.Rg_dtexp).Date)
                        {
                            mensagens.Add("A Data de expedição da identidade tem que ser maior que a data de nascimento.");
                        }
                    }
                }
                else
                {
                    if (pessoa.Rg_tipo == "PASSAPORTE" && pessoa.Nacionalidade == "BRASILEIRA")
                    {
                        mensagens.Add("Para alunos de Nacionalidade 'BRASILEIRA' o documento obrigatório é o RG.");
                    }
                }
            }

            if (pessoa.Rg_emissor == "CGPI/DIREX/DPF" && pessoa.Nacionalidade == "BRASILEIRA")
            {
                mensagens.Add("Para alunos de Nacionalidade 'BRASILEIRA' o Órgão Emissor não pode ser 'CGPI/DIREX/DPF'.");
            }

            if (!documentoValido)
            {
                if (maisDeUmCampo)
                {
                    mensagemDocumento.Append("<br>Campos Necessários: ");
                }
                else
                {
                    mensagemDocumento.Append("<br>Campo Necessário: ");
                }

                mensagemDocumento.Append(camposDocumento);
                mensagens.Add(mensagemDocumento.ToString());
            }

            #endregion

            if (!flPessoa.FlField08.IsNullOrEmptyOrWhiteSpace())
            {
                // Verifica se NIS é numero
                long resultado;
                if (!long.TryParse(flPessoa.FlField08, out resultado))
                {
                    mensagens.Add("O Nº Identificação Social (NIS) deve ser um número.");
                }
            }

            if (flPessoa.FlField02.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Necessário selecionar o tipo de certidão civil.");
            }
            else
            {
                if (flPessoa.FlField02 == "Nenhum")
                {
                    RN.Entidades.DeclaracaoAusencia declaracao = declaracoesAusencia.Where(x => x.TipoDeclaracaoAusenciaId == Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoCertidaoCivil)).First();
                    if (declaracao == null
                        || declaracao.TipoDeclaracaoAusenciaId <= 0
                        || declaracao.Motivo.IsNullOrEmptyOrWhiteSpace()
                        || !aluno.DeclaroCertidaoCivil)
                    {
                        mensagens.Add("É necessário preencher a Declaração/Motivo da Certidão Civil.");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(flPessoa.FlField09))
                    {
                        mensagens.Add("Necessário selecionar a certidão civil.");
                    }

                    if (flPessoa.FlField02 == "Nascimento")
                    {
                        if (string.IsNullOrEmpty(flPessoa.FlField09)
                            && ((!string.IsNullOrEmpty(pessoa.CertNascCartorioExped))
                                || !string.IsNullOrEmpty(flPessoa.FlField02)
                                || !string.IsNullOrEmpty(pessoa.CertNascFolha)
                                || !string.IsNullOrEmpty(pessoa.CertNascLivro)
                                || !string.IsNullOrEmpty(pessoa.CertNascNum)
                                || !string.IsNullOrEmpty(pessoa.CertNumeroMatricula)))
                        {
                            mensagens.Add("É necessário preencher os dados referentes a Certidão Civil");
                        }
                    }


                    if (!string.IsNullOrEmpty(flPessoa.FlField09) ||
                        !string.IsNullOrEmpty(flPessoa.FlField02))
                    {
                        if (flPessoa.FlField09 == "Modelo Novo" &&
                            string.IsNullOrEmpty(pessoa.CertNumeroMatricula))
                        {
                            mensagens.Add("É necessário preencher os dados referentes a Certidão Civil Modelo Novo.");
                        }
                        else if ((flPessoa.FlField09 == "Modelo Antigo") &&
                                 (string.IsNullOrEmpty(pessoa.CertNascCartorioExped)
                                  || pessoa.IdCartorio == null || pessoa.IdCartorio <= 0
                                  || string.IsNullOrEmpty(pessoa.CertNascFolha)
                                  || string.IsNullOrEmpty(pessoa.CertNascLivro)
                                  || string.IsNullOrEmpty(pessoa.CertNascNum)
                                 ))
                        {
                            mensagens.Add("É necessário preencher os dados referentes a Certidão Civil Modelo Antigo");
                        }

                        if (flPessoa.FlField09 == "Modelo Novo" &&
                           !string.IsNullOrEmpty(pessoa.CertNumeroMatricula))
                        {
                            if (!Validacao.ValidaNumerosInteirosPositivos(pessoa.CertNumeroMatricula))
                            {
                                mensagens.Add("O número de matrícula da Certidão Modelo Novo deve conter só números inteiros.");
                            }

                            if (pessoa.CertNumeroMatricula.Length != 32)
                            {
                                mensagens.Add("O número de matrícula da Certidão Modelo Novo deve ter 32 dígitos.");
                            }
                        }
                    }

                    if (pessoa.Dt_nasc != null && pessoa.CertNascEmissao != null)
                    {
                        if (Convert.ToDateTime(pessoa.Dt_nasc) > Convert.ToDateTime(pessoa.CertNascEmissao))
                        {
                            mensagens.Add("A data de Emissão da Certidão não pode ser inferior a data de nascimento.");
                        }
                    }

                    if (pessoa.Dt_nasc != null && pessoa.Rg_dtexp != null && pessoa.Rg_dtexp < pessoa.Dt_nasc)
                    {
                        mensagens.Add("A DATA DE EXPEDIÇÃO do documento de indentificação deve ser maior que a data de nascimento!");
                    }

                    if (pessoa.Dt_nasc != null && pessoa.CertNascEmissao != null && pessoa.CertNascEmissao < pessoa.Dt_nasc)
                    {
                        mensagens.Add("A DATA DE EMISSÃO da certidão de nascimento deve ser maior que a data de nascimento!");
                    }
                }
            }

            // valida campos preenchidos da aba dados escolares

            if (aluno.SitAluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SITUAÇÃO DO ALUNO é obrigatório!");
            }

            if (aluno.AnoIngresso <= 0)
            {
                mensagens.Add("Campo ANO INGRESSO é obrigatório.");
            }

            if (aluno.SemIngresso < 0)
            {
                mensagens.Add("Campo PERÍODO INGRESSO é obrigatório.");
            }

            if (aluno.TipoIngresso.IsNullOrEmptyOrWhiteSpace() || aluno.TipoIngresso == "<Nenhum>")
            {
                mensagens.Add("Campo TIPO INGRESSO é obrigatório.");
            }
            else
            {
                if (aluno.TipoIngresso == "Afastado" && (aluno.TempoAfastamentoRede == null || aluno.TempoAfastamentoRede == 0))
                {
                    mensagens.Add("Campo TEMPO DE AFASTAMENTO é obrigatório para o tipo de ingresso 'Afastado'!");
                }
            }

            if (aluno.RedeEnsinoOrigem.IsNullOrEmptyOrWhiteSpace() || aluno.RedeEnsinoOrigem == "Selecione")
            {
                mensagens.Add("Campo REDE DE ENSINO ORIGEM é obrigatório!");
            }

            if (aluno.UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }
            else
            {
                aluno.UnidadeFisica = aluno.UnidadeEnsino;

                if (aluno.MunicipioEscola.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo MUNICIPIO ESCOLA é obrigatorio.");
                }

                if (pessoa.End_municipio != aluno.MunicipioEscola)
                {
                    var limitrofe = MunicipioLimitrofe.Listar(aluno.MunicipioEscola);
                    var achou = false;

                    foreach (DataRow item in limitrofe.Rows)
                    {
                        if (item["CODIGO_MUNICIPIO_LIMITROFE"].ToString() == pessoa.End_municipio)
                        {
                            achou = true;
                            break;
                        }
                    }

                    if (!achou)
                    {
                        mensagens.Add("O Município/UF informado no endereço do aluno nao condiz com o Município da escola ou de entorno. Favor verificar se o endereço do aluno está correto.");
                    }
                }
            }

            if (aluno.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }
            else
            {
                var tipo_curso = Curso.ConsultarTipoProfCurso(aluno.Curso);

                if (tipo_curso == "Especial")
                {
                    if (pessoa.NecessidadeEspecialId == 30) //"NÃO POSSUI"
                    {
                        mensagens.Add("Para escolher um curso de educação especial, informar o tipo de necessidade especial do aluno(a) na aba 'Dados Pessoais'.");
                    }
                }
                else if (tipo_curso == "Concomitante/Subsequente")
                {
                    if (string.IsNullOrEmpty(aluno.TipoEnsinoProfissionalizante))
                    {
                        mensagens.Add("Para escolher um curso Concomitante/Subsequente, informar o Tipo Ens. Profissionalizante do aluno(a) na aba 'Dados Escolares'.");
                    }
                }
            }

            if (aluno.Turno.IsNullOrEmptyOrWhiteSpace() || aluno.Turno == "<Nenhum>")
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (aluno.Curriculo.IsNullOrEmptyOrWhiteSpace() || aluno.Curriculo == "<Nenhum>")
            {
                mensagens.Add("O campo MATRIZ CURRICULAR é obrigatório!");
            }

            if (aluno.Serie < 0)
            {
                mensagens.Add("Campo SÉRIE é obrigatório.");
            }

            if (flPessoa.FlField03.IsNullOrEmptyOrWhiteSpace() || flPessoa.FlField03 == "<Nenhum>")
            {
                mensagens.Add("Campo RECEBE ESCOLARIZAÇÃO EM OUTRO ESPAÇO é obrigatório.");
            }


            if (flPessoa.FlField04 == "S")
            {
                if (pessoa.E_mail.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Se o Aluno utiliza transporte, é necessário o preenchimento do campo E-MAIL.");
                }
                else
                {
                    if (aluno.EMailAnterior != pessoa.E_mail)
                    {
                        if (aluno.EMailConfirmacao.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo E-MAIL CONFIRMAÇÃO é obrigatório.");
                        }

                        if (pessoa.E_mail != aluno.EMailConfirmacao)
                        {
                            mensagens.Add("O E-mail de Confirmação não é igual ao preenchido no campo E-mail.");
                        }
                    }
                }
            }

            foreach (var declaracaoAusencia in declaracoesAusencia)
            {
                if (declaracaoAusencia.TipoDeclaracaoAusenciaId != 0)
                {
                    if (declaracaoAusencia.TipoDeclaracaoAusenciaId == 0)
                    {
                        mensagens.Add("Campo Tipo de Declaração é obrigatório.");
                    }

                    if (declaracaoAusencia.TipoDeclaracaoAusenciaId == 3 && string.IsNullOrEmpty(declaracaoAusencia.Motivo))
                    {
                        mensagens.Add("Campo Motivo é obrigatório para Certidão Civil.");
                    }

                    if (!string.IsNullOrEmpty(declaracaoAusencia.Motivo))
                    {
                        if (declaracaoAusencia.Motivo.Length < 10)
                        {
                            mensagens.Add("Campo MOTIVO deve ter mais 10 caracteres.");
                        }

                        if (declaracaoAusencia.Motivo.Length > 200)
                        {
                            mensagens.Add("Campo MOTIVO é obrigatório com o máximo de 200 caracteres!");
                        }

                        regex = new Regex(@"(\w)\1\1+");

                        if (regex.IsMatch(declaracaoAusencia.Motivo))
                        {
                            mensagens.Add("Campo MOTIVO não deve ter mais de 2 letras consecutivas repetidas.");
                        }
                    }

                    if (string.IsNullOrEmpty(declaracaoAusencia.Matricula)
                            || (!string.IsNullOrEmpty(declaracaoAusencia.Matricula)
                                && declaracaoAusencia.Matricula.Length > 12))
                    {
                        mensagens.Add("Campo MATRICULA é obrigatório com o máximo de 12 caracteres!");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca pessoa caso existe com mesmo nome/data de nascimento/nome da mãe
                    decimal pessoaBase = rnPessoa.ObtemPessoaPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc));

                    //Verifica se é cadastro
                    if (cadastro)
                    {
                        //Utiliza pessoa em caso de cadastro
                        pessoa.Pessoa = pessoaBase;
                        aluno.Pessoa = pessoaBase;

                        if (rnEventoGeral.ExisteBloqueioCadastroAlunoAtivoPor(contexto, aluno.Curso))
                        {
                            mensagens.Add("Não é possível matricular o aluno neste curso, pois ele está bloqueado para Matriculas. Favor verificar na Agenda de Escolaridades.");
                            return validacaoDados;
                        }

                        if (this.PossuiOutroAlunoPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc), aluno.Aluno ?? string.Empty))
                        {
                            mensagens.Add("Já existe outro aluno com mesmo nome/data de nascimento/nome da mãe cadastrado.");
                        }

                        //Verificar se tem vaga no curso / serie / turno / ano / semestre
                        int vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(contexto,
                           aluno.UnidadeEnsino,
                            Convert.ToInt32(aluno.AnoIngresso),
                            Convert.ToInt32(aluno.SemIngresso),
                            Convert.ToInt32(aluno.Serie),
                            aluno.Curso,
                            aluno.Turno);

                        int vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(contexto,
                            aluno.UnidadeEnsino,
                            Convert.ToInt32(aluno.AnoIngresso),
                            Convert.ToInt32(aluno.SemIngresso),
                            Convert.ToInt32(aluno.Serie),
                            aluno.Curso,
                            aluno.Turno);

                        if (vagasLiberadas <= vagasUtilizadas)
                        {
                            mensagens.Add("Não será possível realizar o cadastro do aluno, pois não existem vagas disponíveis para a serie ou modalidade ou turno pretendidos!");
                        }
                    }
                    else
                    {
                        //Caso seja alteração

                        if (pessoaBase != 0 && pessoa.Pessoa != pessoaBase)
                        {
                            mensagens.Add("Já existe outra pessoa com mesmo nome/data de nascimento/nome da mãe cadastrada.");
                        }
                        else
                        {
                            //Caso o aluno editado esteja ativo, verificar ser existe outro ativo
                            if (aluno.SitAluno == "Ativo")
                            {
                                if (this.PossuiOutroAlunoAtivoPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc), aluno.Aluno ?? string.Empty))
                                {
                                    mensagens.Add("Já existe outro aluno ativo com mesmo nome/data de nascimento/nome da mãe cadastrado.");
                                }
                            }
                        }

                        if (mudouCurso)
                        {
                            // não permitir alterar para curso com matrículas abertas
                            if (rnEventoGeral.ExisteBloqueioCadastroAlunoAtivoPor(contexto, aluno.Curso))
                            {
                                mensagens.Add("Não é possível alterar curso para curso com matrículas abertas.");
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(pessoa.Id_censo))
                    {
                        if (!RN.Pessoa.VerificarInep(pessoa.Id_censo, pessoa.Pessoa))
                        {
                            mensagens.Add("O Número de Identificação no INEP já está sendo usado por outra pessoa.");
                        }
                    }

                    if (!pessoa.Cpf.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Verifica se existe outro aluno com o mesmo cpf
                        string retorno = this.PossuiOutroCPFPor(contexto, pessoa.Cpf, aluno.Aluno ?? string.Empty);
                        if (!retorno.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add(string.Format("O aluno {0} já foi cadastrado com este CPF.", retorno));
                        }
                        else
                        {
                            if (rnPessoa.PossuiCPFPor(contexto, pessoa.Cpf, pessoa.Pessoa))
                            {
                                mensagens.Add("CPF já existente cadastrado para um docente / servidor.");
                            }
                        }
                    }

                    restricao = rnRestricaoIdadeSerie.CarregaRestricaoPor(contexto, aluno.Curso, Convert.ToInt32(aluno.Serie));

                    //Verifica se o aluno possui necessidade especial
                    if (pessoa.NecessidadeEspecialId == 30)//"Não possui."
                    {
                        //Verifica se possiu avaliacao napes positiva
                        if (rnAvaliacaoNapes.PossuiAvaliacaoPositivaPor(contexto, aluno.Aluno))
                        {
                            mensagens.Add("Este aluno possui Avaliação NAPES positiva, com isso é necessário informar uma necessidade especial.");
                        }

                        //Para Alunos sem necessidades Especiais Verificar restrição de idade minima e maxima
                        if (idade < restricao.IdadeMinima || idade > restricao.IdadeMaxima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado é permitido cadastrar alunos entre {0} e {1} anos. Favor verificar a DATA DE NASCIMENTO!",
                                restricao.IdadeMinima,
                                restricao.IdadeMaxima));
                        }
                    }
                    else
                    {
                        //Verifica se possiu avaliacao napes positiva para tipos de recurso não permitidos para a necessaidade especial
                        if (rnAvaliacaoNapes.PossuiOutraAvaliacaoPositivaPor(contexto, aluno.Aluno, Convert.ToInt32(pessoa.NecessidadeEspecialId)))
                        {
                            mensagens.Add("Este aluno possui Avaliação NAPES positiva para apoios não permitidos à necessidade especial informada, favor verificar.");
                        }

                        //Para Alunos com necessidades Especiais Verificar restrição de idade minima
                        if (idade < restricao.IdadeMinima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado não é permitido cadastrar alunos com necessidade especial com menos de {0} anos. Favor verificar a DATA DE NASCIMENTO!",
                                 restricao.IdadeMinima));
                        }
                    }

                    //Verifica se aluno permite abreviação
                    if (this.ExisteAbreviacao(contexto, aluno.Aluno ?? string.Empty))
                    {
                        var nomes = pessoa.Nome_compl.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        for (var i = 0; i < nomes.Length; i++)
                        {
                            var nome = nomes[i];

                            if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                            {
                                nome = nome.Remove(1);
                            }

                            if ((nome.Length == 1 && !rnValidacao.ehAbreviacaoValida(nome) && (string.Compare(nome, "e", true) != 0)) ||
                                ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomes.Length - 1)))
                            {
                                mensagens.Add("Não é possível utilizar abreviações no nome do aluno.");
                            }
                        }

                        var nomesMae = pessoa.NomeMae.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        for (var i = 0; i < nomesMae.Length; i++)
                        {
                            var nome = nomesMae[i];

                            if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                            {
                                nome = nome.Remove(1);
                            }
                            if ((nome.Length == 1 && !rnValidacao.ehAbreviacaoValida(nome) && (string.Compare(nome, "e", true) != 0)) ||
                                ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomesMae.Length - 1)))
                            {
                                mensagens.Add("Não é possível utilizar abreviações no nome da mãe.");
                            }
                        }

                        var nomesPai = pessoa.NomePai.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        for (var i = 0; i < nomesPai.Length; i++)
                        {
                            var nome = nomesPai[i];

                            if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                            {
                                nome = nome.Remove(1);
                            }
                            if ((nome.Length == 1 && (string.Compare(nome, "e", true) != 0)) ||
                                ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomesPai.Length - 1)))
                            {
                                mensagens.Add("Não é possível utilizar abreviações no nome do pai.");
                            }
                        }
                    }

                    if (compartilhada)
                    {
                        //Verifica se existe compartilhada
                        if (!rnCompartilhada.PossuiUnidadeCompartilhada(contexto, unidadeDestino, unidadeOrigem))
                        {
                            mensagens.Add("Não foi encontrado registro de unidade compartilhada para a unidade do aluno");
                        }
                    }

                    if (listRecursoNecessidadeEspecial != null && listRecursoNecessidadeEspecial.Count > 0)
                    {
                        if (pessoa.NecessidadeEspecialId != null)
                        {
                            foreach (int tipoRecurso in listRecursoNecessidadeEspecial)
                            {
                                if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador)
                                {
                                    //Verifica se aluno possui necessidade especial que necessite de cuidador
                                    if (!rnNecessidadeEspecial.NecessitaCuidadorPor(contexto, Convert.ToInt32(pessoa.NecessidadeEspecialId)))
                                    {
                                        mensagens.Add("A necessidade especial do aluno não pode ser associada a um Cuidador");
                                    }
                                }
                                else if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor)
                                {
                                    //Verifica se aluno possui necessidade especial que necessite de Ledor
                                    if (!rnNecessidadeEspecial.NecessitaLedorPor(contexto, Convert.ToInt32(pessoa.NecessidadeEspecialId)))
                                    {
                                        mensagens.Add("A necessidade especial do aluno não pode ser associada a um Ledor");
                                    }
                                }
                                else if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete)
                                {
                                    //Verifica se aluno possui necessidade especial que necessite de Intérprete
                                    if (!rnNecessidadeEspecial.NecessitaInterpretePor(contexto, Convert.ToInt32(pessoa.NecessidadeEspecialId)))
                                    {
                                        mensagens.Add("A necessidade especial do aluno não pode ser associada a um Intérprete");
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
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public ValidacaoDados ValidaTransporte(int necessidadeEspecialId, string email, LyFlPessoa flPessoa)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (flPessoa == null)
            {
                return validacaoDados;
            }

            if (flPessoa.Pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (flPessoa.FlField04.IsNullOrEmptyOrWhiteSpace() || flPessoa.FlField04 == "<Nenhum>")
            {
                mensagens.Add("Campo UTILIZA TRANSPORTE é obrigatório.");
            }
            else
            {
                if (flPessoa.FlField04 == "S")
                {
                    if (email.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Se o Aluno utiliza transporte, é necessário o preenchimento do campo E-MAIL.");
                    }
                    else
                    {
                        if (flPessoa.FlField10.IsNullOrEmptyOrWhiteSpace() || flPessoa.FlField10 == "<Nenhum>")
                        {
                            mensagens.Add("Necessário selecionar o campo 'PODER PÚBLICO RESPONSÁVEL PELO TRANSPORTE ESCOLAR'.");
                        }
                    }

                    if (string.IsNullOrEmpty(flPessoa.FlField05))
                    {
                        mensagens.Add("Necessário selecionar pelo menos um modal.");
                    }
                    else
                    {
                        string[] modaisSelecionados = flPessoa.FlField05.Split(';');

                        // Transporte Rural (5)
                        if (modaisSelecionados.Contains("5"))
                        {
                            if (flPessoa.FlField11.IsNullOrEmptyOrWhiteSpace() && flPessoa.FlField12.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo Rodoviário/Aquaviário é de preenchimento obrigatório.");
                            }
                        }

                        // Operadora Ônibus (2)
                        if (modaisSelecionados.Contains("2"))
                        {
                            if (flPessoa.FlField20.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo Operadora é de preenchimento obrigatório.");
                            }
                        }

                        // Carro com Acessibilidade (6)
                        if (modaisSelecionados.Contains("6"))
                        {
                            if (necessidadeEspecialId == 30) // 30 = não possui necessidade especial
                            {
                                mensagens.Add("Somente poderá ser selecionado a opção CARRO COM ACESSIBILIDADE (CADEIRANTE) caso o aluno possua Necessidade Especial.");
                            }
                        }
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

        public void Insere(LyAluno aluno, LyPessoa pessoa, LyFlPessoa flPessoa, LyFotoPessoa foto, List<RN.Entidades.DeclaracaoAusencia> declaracoesAusencia, List<RN.Entidades.PessoaRecursoAplicacaoProva> listPessoaRecursoAplicacaoProva, List<int> listRecursoNecessidadeEspecial, bool compartilhada, string unidadeDestino, string unidadeOrigem, List<int> listaTranstornoAprendizagem)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Compartilhada rnCompartilhada = new Compartilhada();
            RN.PessoaTranstornoAprendizagem rnPessoaTranstornoAprendizagem = new PessoaTranstornoAprendizagem();
            RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial rnAlunoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial();
            Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();

            try
            {
                if (pessoa.Pessoa == 0)
                {
                    //Inserir pessoa
                    rnPessoa.Insere(contexto, pessoa);

                    //insere foto
                    if (foto.Foto != null)
                    {
                        foto.Pessoa = pessoa.Pessoa;
                        if (foto.Pessoa != 0)
                        {
                            FotoPessoa.Inserir(foto, contexto);
                        }
                        else
                        {
                            throw new Exception("Código da Pessoa não localizado para cadastrar uma Foto.");
                        }
                    }
                }
                else
                {
                    //Alterar pessoa (já existente)
                    rnPessoa.AtualizaPessoaAluno(contexto, pessoa);

                    if (foto.Foto != null)
                    {
                        foto.Pessoa = pessoa.Pessoa;
                        //verificar se já tem uma foto para esta pessoa
                        if (FotoPessoa.VerificarFoto(pessoa.Pessoa))
                        {
                            FotoPessoa.Alterar(foto, contexto);
                        }
                        else
                        {
                            if (foto.Pessoa != 0)
                            {
                                FotoPessoa.Inserir(foto, contexto);
                            }
                            else
                            {
                                throw new Exception("Código da Pessoa não localizado para cadastrar uma Foto.");
                            }
                        }
                    }
                }

                flPessoa.Pessoa = pessoa.Pessoa;
                if (rnFlPessoa.ExistePor(contexto, pessoa.Pessoa))
                {
                    //up dados na flPessoa                    
                    FlPessoa.Alterar(flPessoa, contexto);
                }
                else
                {
                    //inserir dados na flPessoa
                    if (flPessoa.Pessoa != 0)
                    {    //inseri flPessoa
                        FlPessoa.Inserir(flPessoa, contexto);
                    }
                    else
                    {
                        throw new Exception("Código da Pessoa não localizado para cadastrar um Field.");
                    }
                }

                //inserir aluno
                aluno.Pessoa = pessoa.Pessoa;
                this.Insere(contexto, aluno);

                //Insere dados Compartilhada
                if (compartilhada)
                {
                    //Busca unidada compartilhada
                    string unidadeCompartilhada = rnCompartilhada.RetornaUnidadeCompartilhada(contexto, unidadeDestino, unidadeOrigem);

                    RN.UnidadeEnsinoCompartilhada.Entidades.Aluno_UnidadeEnsinoCompartilhada alunoUnidadeCompartilhada = new RN.UnidadeEnsinoCompartilhada.Entidades.Aluno_UnidadeEnsinoCompartilhada();

                    alunoUnidadeCompartilhada.AlunoId = aluno.Aluno;
                    alunoUnidadeCompartilhada.UnidadeEnsinoCompartilhadaId = unidadeCompartilhada;

                    RN.UnidadeEnsinoCompartilhada.Aluno_UnidadeEnsinoCompartilhada.InsereAluno_UnidadeEnsinoCompartilhada(alunoUnidadeCompartilhada, contexto);
                }

                //Inserir dados na tabela de Declaracao de ausencia
                //1º Remove todas as declaracao anteriores
                RN.DeclaracaoAusencia.RemoverPorAluno(aluno.Aluno, contexto);

                foreach (var declaracaoAusencia in declaracoesAusencia)
                {
                    //2º Insere as declarações necessárias
                    declaracaoAusencia.AlunoId = aluno.Aluno;
                    declaracaoAusencia.Matricula = aluno.Usuario;
                    RN.DeclaracaoAusencia.Inserir(declaracaoAusencia, contexto);
                }

                //Recurso Aplicação Prova - Necessidade Especial
                RN.PessoaRecursoAplicacaoProva.InserirPessoaRecursoAplicacaoProva(listPessoaRecursoAplicacaoProva, Convert.ToInt32(aluno.Pessoa), contexto);

                //Insere Recursos Necessidade Especial
                rnAlunoRecursoNecessidadeEspecial.InsereLista(contexto, listRecursoNecessidadeEspecial, aluno.Aluno, aluno.Usuario);

                //Insere Transtorno Aprendizagem
                rnPessoaTranstornoAprendizagem.InsereLista(contexto, listaTranstornoAprendizagem, Convert.ToDecimal(aluno.Pessoa), aluno.Usuario);

                TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula
                {
                    Aluno = aluno.Aluno,
                    Censo = aluno.UnidadeEnsino,
                    Ano = aluno.AnoIngresso,
                    Periodo = aluno.SemIngresso,
                    Curso = aluno.Curso,
                    Turno = aluno.Turno,
                    Curriculo = aluno.Curriculo,
                    Serie = aluno.Serie,
                    EnsinoReligioso = false,
                    LinguaEstrangeiraFacultativa = false,
                    ProjetoAutonomia = false,
                    Matricula = aluno.Usuario,
                    TipoVagaOcupada = "VN"
                };

                ConfirmacaoMatricula.Inserir(confirmacao, contexto);
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

        public string PossuiOutroCPFPor(DataContext contexto, string cpf, string aluno)
        {
            cpf = Utils.RetirarMascara(cpf);

            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 ALUNO
                                      FROM   LY_ALUNO A (NOLOCK) 
                                             INNER JOIN LY_PESSOA P (NOLOCK) 
                                                       ON A.PESSOA = P.PESSOA 
                                      WHERE  P.CPF = @CPF
                                             AND P.PESSOA <> (SELECT PESSOA FROM LY_ALUNO A2 WHERE A2.ALUNO = @ALUNO) ";

            contextQuery.Parameters.Add("@CPF", cpf);
            contextQuery.Parameters.Add("@ALUNO", aluno);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public bool PossuiOutroCPFAtivoPor(string cpf, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiOutroCPFAtivoPor(ctx, cpf, aluno);
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
            finally
            {
                ctx.Dispose();
            }
        }

        public bool PossuiAlunoPor(DataContext ctx, string cpf)
        {
            cpf = Utils.RetirarMascara(cpf);

            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   LY_ALUNO A (NOLOCK) 
                                             INNER JOIN LY_PESSOA P (NOLOCK) 
                                                       ON A.PESSOA = P.PESSOA   
                                      INNER JOIN TCE_SITUACAO_FINAL_ALUNO TS ON A.ALUNO = TS.ALUNO AND TS.ANO >= 2014
                                      WHERE  P.CPF = @CPF  
                                        AND NOT EXISTS (SELECT * FROM  LY_H_CURSOS_CONCL CC  INNER JOIN LY_ALUNO A2 ON A2.ALUNO=CC.ALUNO where A2.PESSOA=A.PESSOA AND MOTIVO = 'CONCLUSAO' AND ANO_ENCERRAMENTO < 2014)";

            contextQuery.Parameters.Add("@CPF", cpf);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public bool PossuiOutroCPFAtivoPor(DataContext ctx, string cpf, string aluno)
        {
            cpf = Utils.RetirarMascara(cpf);

            ContextQuery contextQuery = new ContextQuery();
            bool temCPF = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   LY_ALUNO A (NOLOCK) 
                                             INNER JOIN LY_PESSOA P (NOLOCK) 
                                                       ON A.PESSOA = P.PESSOA 
                                      WHERE  P.CPF = @CPF 
                                             AND A.SIT_ALUNO = @SIT_ALUNO
                                             AND A.ALUNO <> @ALUNO ";

            contextQuery.Parameters.Add("@CPF", cpf);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@SIT_ALUNO", "Ativo");

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                temCPF = true;
            }

            return temCPF;
        }

        private bool PossuiOutroAlunoPor(DataContext ctx, string nome, string nomeMae, DateTime dataNascimento, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   LY_ALUNO A (NOLOCK) 
                                    INNER JOIN LY_PESSOA P (NOLOCK) 
                                            ON A.PESSOA = P.PESSOA 
                            WHERE P.NOME_COMPL = @NOME
	                            AND CONVERT(DATE, P.DT_NASC) = @DATANASCIMENTO 
	                            AND P.NOME_MAE = @NOMEMAE 
                                AND A.ALUNO <> @ALUNO ";

            contextQuery.Parameters.Add("@NOME", nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", dataNascimento.Date);
            contextQuery.Parameters.Add("@NOMEMAE", nomeMae);
            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutroAlunoAtivoPor(DataContext ctx, string nome, string nomeMae, DateTime dataNascimento, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM   LY_ALUNO A (NOLOCK) 
                                    INNER JOIN LY_PESSOA P (NOLOCK) 
                                            ON A.PESSOA = P.PESSOA 
                            WHERE P.NOME_COMPL = @NOME
	                            AND CONVERT(DATE, P.DT_NASC) = @DATANASCIMENTO 
	                            AND P.NOME_MAE = @NOMEMAE
                                AND A.SIT_ALUNO = @SIT_ALUNO 
                                AND A.ALUNO <> @ALUNO ";

            contextQuery.Parameters.Add("@NOME", nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", dataNascimento.Date);
            contextQuery.Parameters.Add("@NOMEMAE", nomeMae);
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@SIT_ALUNO", "Ativo");

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutroAlunoPor(DataContext ctx, string nome, string nomeMae, DateTime dataNascimento, List<string> alunos)
        {
            string matriculas = alunos.Aggregate((x, y) => x + ", " + y);

            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = string.Format(@" SELECT COUNT(*) 
                            FROM   LY_ALUNO A (NOLOCK) 
                                    INNER JOIN LY_PESSOA P (NOLOCK) 
                                            ON A.PESSOA = P.PESSOA 
                            WHERE P.NOME_COMPL = @NOME
	                            AND CONVERT(DATE, P.DT_NASC) = @DATANASCIMENTO 
	                            AND P.NOME_MAE = @NOMEMAE
                                AND A.SIT_ALUNO = @SIT_ALUNO 
                                AND A.ALUNO NOT IN ({0}) ", matriculas);

            contextQuery.Parameters.Add("@NOME", nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", dataNascimento.Date);
            contextQuery.Parameters.Add("@NOMEMAE", nomeMae);
            contextQuery.Parameters.Add("@SIT_ALUNO", "Ativo");

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutroAlunoAtivoPor(string nome, string nomeMae, DateTime dataNascimento, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiOutroAlunoAtivoPor(ctx, nome, nomeMae, dataNascimento, aluno);
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
            finally
            {
                ctx.Dispose();
            }
        }

        private void Remove(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE  FROM DBO.LY_ALUNO
                                      WHERE   ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contexto.ApplyModifications(contextQuery);
        }

        public static ValidacaoDados ValidarDadosCartao(LyAluno aluno)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aluno == null)
            {
                return validacaoDados;
            }

            var flPessoa = FlPessoa.Carregar(Convert.ToInt32(aluno.Pessoa));
            var pessoa = Pessoa.Carregar(Convert.ToInt32(aluno.Pessoa));
            var foto = FotoPessoa.Carregar(Convert.ToInt32(aluno.Pessoa));

            if (flPessoa.FlField04 != "S")
            {
                mensagens.Add("No seu registo não consta pedido de gratuidade. Favor comparecer à secretaria de sua unidade escolar e requisitar alteração no cadastro!");
            }

            var erroCadastro = false;

            if (string.IsNullOrEmpty(pessoa.Nome_compl) || (pessoa.Dt_nasc == DateTime.MinValue)
                           || (pessoa.Dt_nasc == null) || (pessoa.Dt_nasc == DateTime.MinValue)
                           || (pessoa.Dt_nasc == null) || string.IsNullOrEmpty(pessoa.NomeMae)
                           || string.IsNullOrEmpty(pessoa.Cep) || string.IsNullOrEmpty(pessoa.End_municipio)
                           || string.IsNullOrEmpty(pessoa.Bairro))
            {
                erroCadastro = true;
                mensagens.Add("Existem pendências cadastrais!");
                mensagens.Add("Seus dados ainda não foram enviados para emissão do cartão porque seu cadastro escolar está imcompleto. Compareça à secretaria da sua unidade escolar com seus documentos e comprovante de residência!");
            }

            if (foto.Foto == null)
            {
                if (!erroCadastro)
                {
                    mensagens.Add("Existem pendências cadastrais!");
                    erroCadastro = true;
                }

                mensagens.Add(@"Seus dados ainda não foram enviados para emissão do cartão porque não existe no sistema o registro de sua foto. Compareça à secretaria de sua unidade escolar para tirá-la!");
            }

            if (!Matricula.ExisteDadoMatricula(aluno.Aluno))
            {
                if (!erroCadastro)
                {
                    mensagens.Add("Existem pendências cadastrais!");
                }

                mensagens.Add(@"Seus dados ainda não foram enviados para emissão do cartão, pois você ainda não foi lotado em umas das turmas de sua unidade escolar. Compareça à secretaria de sua unidade escolar para pedir a regularização da situação!");
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

        public static bool ExisteUtilizaTransporte(string aluno)
        {
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();
                return ExisteUtilizaTransporte(contexto, aluno);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;

                if (contexto != null)
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
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        public static bool ExisteUtilizaTransporte(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool utiliza = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                      FROM    dbo.LY_ALUNO A
                                              INNER JOIN dbo.LY_FL_PESSOA FP ON A.PESSOA = FP.PESSOA
                                      WHERE   ALUNO = @ALUNO
                                              AND FL_FIELD_04 = 'S' ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                utiliza = true;
            }

            return utiliza;
        }

        public void RetiraUsoGratuidade(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE  FP
                            SET     FL_FIELD_04 = 'N' ,
                                    FL_FIELD_05 = NULL
                            FROM    dbo.LY_ALUNO A
                                    INNER JOIN dbo.LY_FL_PESSOA FP ON A.PESSOA = FP.PESSOA
                            WHERE   ALUNO = @ALUNO
                                    AND FL_FIELD_04 = 'S' ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);

            contexto.ApplyModifications(contextQuery);
        }

        public static bool ExisteAbreviacao(string aluno)
        {
            RN.Aluno rnAluno = new Aluno();
            DataContext contexto = null;
            bool retorno = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                retorno = rnAluno.ExisteAbreviacao(contexto, aluno);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return retorno;
        }

        public bool ExisteAbreviacao(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                            FROM TCE_ABREVIACAO_ALUNO (NOLOCK)
                            WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static string RetornaMunicipio(string aluno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  P.END_MUNICIPIO
                        FROM    LY_ALUNO A
                                INNER JOIN DBO.LY_PESSOA P ON A.PESSOA = P.PESSOA
                        WHERE   ALUNO = @ALUNO ");

                contextQuery.Parameters.Add("@ALUNO", aluno);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static string RetornaAluno(decimal idPessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"  SELECT TOP 1 A.ALUNO
                             FROM   LY_ALUNO A
                             WHERE  A.PESSOA = @PESSOA ");

                contextQuery.Parameters.Add("@PESSOA", idPessoa);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static DataTable ListarAlunoNovo(string aluno, string nome, string mae, DateTime dataNascimento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int cont = 0;

            try
            {

                if (string.IsNullOrEmpty(nome) && string.IsNullOrEmpty(mae) && (dataNascimento == DateTime.MinValue))
                    return null;

                if (!string.IsNullOrEmpty(nome)) cont = cont + 1;
                if (!string.IsNullOrEmpty(mae)) cont = cont + 1;
                if (dataNascimento != DateTime.MinValue) cont = cont + 1;

                if (cont < 2)
                    return null;

                var validacao = Aluno.ValidarBuscaNovoAluno(aluno, nome, mae, dataNascimento);

                if (!validacao.Valido)
                    return null;

                ContextQuery contextQuery = new ContextQuery
                {
                    Command =
                        @"SELECT DISTINCT
                           A.ALUNO,
                           P.NOME_COMPL, 
                           P.RG_NUM,
                           P.CPF, 
                           P.NOME_MAE, 
                           P.NOME_PAI, 
                           P.DT_NASC, 
                           A.NUMINSCRICAO,
                           A.UNIDADE_ENSINO,
                           A.SIT_ALUNO,
						   u.nome_comp
                    FROM   LY_ALUNO A WITH ( NOLOCK )
                            INNER JOIN LY_PESSOA P WITH ( NOLOCK ) ON P.PESSOA = A.PESSOA
                         LEFT OUTER JOIN LY_H_CURSOS_CONCL C ON A.ALUNO = C.ALUNO
                         inner join LY_Unidade_fisica u on a.unidade_fisica = u.unidade_fis
                    WHERE (MOTIVO IS NULL OR MOTIVO NOT IN ('DUPLIC_SIS','DUPLICIDADE'))  "
                };

                if (!string.IsNullOrEmpty(nome))
                {
                    if (nome.IndexOf("'") >= 0)
                        nome = nome.Replace("'", "''");

                    contextQuery.Command += String.Format(" AND P.nome_compl like '{0}%'", nome);
                }
                if (!string.IsNullOrEmpty(mae))
                {
                    if (mae.IndexOf("'") >= 0)
                        mae = mae.Replace("'", "''");

                    contextQuery.Command += String.Format(" AND p.nome_mae like '{0}%'", mae);
                }
                if (dataNascimento != null && dataNascimento != DateTime.MinValue)
                {
                    contextQuery.Command += " AND CONVERT(DATE,p.DT_NASC) =  Convert(date,@data,103)";
                    contextQuery.Parameters.Add("@data", dataNascimento);
                }
                return ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static DataTable ListaAlunoNovoTransf(string aluno)
        {
            if (string.IsNullOrEmpty(aluno))
                return null;

            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            try
            {

                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"SELECT DISTINCT 
                                      a.aluno,
                                      a.numinscricao,
                                      p.nome_pai AS pai,
                                      p.nome_mae AS mae,
                                      p.pessoa,
                                      p.nome_compl AS nome,
                                      p.rg_num,
                                      p.cpf,
                                      a.sit_aluno,
                                      a.unidade_ensino,
                                      UE.NOME_COMP AS escola,
									  UE.MUNICIPIO,
									  M.NOME AS NOME_MUNICIPIO,
                                      CONVERT(VARCHAR(10), p.DT_NASC, 103) AS dt_nascimento,
                                      p.necessidadeespecialid
                              FROM    LY_ALUNO a WITH (NOLOCK)
                                      INNER JOIN LY_PESSOA p WITH (NOLOCK)
                                      ON p.PESSOA = a.PESSOA
                                      INNER JOIN LY_UNIDADE_ENSINO UE 
                                      ON UE.UNIDADE_ENS=A.UNIDADE_ENSINO
                                     INNER JOIN dbo.MUNICIPIO M 
                                      ON M.CODIGO=UE.MUNICIPIO
                              where NOT EXISTS ( SELECT 1
                                                 FROM   dbo.LY_H_CURSOS_CONCL hc WITH ( NOLOCK )
                                                 WHERE  hc.ALUNO = a.ALUNO
                                                        AND DT_REABERTURA IS NULL
                                                        AND MOTIVO IN ( 'CONCLUSAO', 'OBITO' ) )
                                    AND a.ALUNO = @ALUNO"
                };
                contextQuery.Parameters.Add("@ALUNO", aluno);

                return ctx.GetDataTable(contextQuery);

            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static DataTable ListaAlunoNovoTransfUnidadeEnsinoDestino(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
                return null;

            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            try
            {

                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"select distinct ue.UNIDADE_ENS,ue.NOME_COMP,uas.UNIDADE_FIS 
                                    from LY_UNIDADE_ENSINO ue 
                                    inner join SETOR s on ue.SETOR = s.SETOR 
                                    inner join LY_UNIDADES_ASSOCIADAS uas on uas.UNIDADE_ENS = ue.UNIDADE_ENS 
                                    inner join LY_USUARIO_UNIDADE_FIS uuf on uuf.UNIDADE_FIS = uas.UNIDADE_FIS
                                    where uuf.USUARIO = @USUARIO"
                };
                contextQuery.Parameters.Add("@USUARIO", usuario);

                return ctx.GetDataTable(contextQuery);

            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static DataTable ListarIrmaos(string NomeMae, string NomePai, string NomeIrmao, DateTime DataNascimento, string Matricula, bool MaeNaoDeclar, bool PaiNaoDeclar, decimal Pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {

                if (MaeNaoDeclar && PaiNaoDeclar)
                {
                    return null;
                }
                if (NomeMae == "DESCONHECIDO" && NomePai == "DESCONHECIDO")
                {
                    return null;
                }

                if (string.IsNullOrEmpty(NomeMae)
                   && (NomePai == "NÃO DECLARADO" || NomePai.Contains("NÃO INFORMADO") || NomePai == "DESCONHECIDO"))
                {
                    return null;
                }
                if (string.IsNullOrEmpty(NomePai)
                   && (NomeMae == "NÃO DECLARADO" || NomeMae == ("DESCONHECIDO") || NomeMae == "NÃO DECLARADA")
                    || NomeMae == ("DESCONHECIDA") || NomeMae.Contains("NÃO INFORMADA"))
                {
                    return null;
                }

                var contextQuery = new ContextQuery


                {
                    Command =
                        @"SELECT A.ALUNO,P.DT_NASC,P.NOME_COMPL,P.NOME_MAE,P.NOME_PAI,
                           CERT_NASC_FOLHA,CERT_NASC_LIVRO,
                           CERT_NASC_NUM,CERT_NUMERO_MATRICULA,A.UNIDADE_ENSINO,
                            UE.NOME_COMP, A.UNIDADE_ENSINO + ' - ' + UE.NOME_COMP AS ESCOLA
                           FROM LY_ALUNO A
                            INNER JOIN dbo.LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                           INNER JOIN LY_PESSOA P
                           ON A.PESSOA = P.PESSOA
                           WHERE "
                };
                if (!string.IsNullOrEmpty(NomeMae) && NomeMae != "NÃO DECLARADA" && NomeMae != "NÃO DECLARADO" &&
                    NomeMae != "DESCONHECIDA" && NomeMae != "MÃE NÃO INFORMADA" && NomeMae != "DESCONHECIDO" &&
                    !string.IsNullOrEmpty(NomePai) && NomePai != "NÃO DECLARADO"
                    && NomePai != "DESCONHECIDO" && NomePai != "PAI NÃO INFORMADO")
                {
                    contextQuery.Command += "(";
                }


                if (!string.IsNullOrEmpty(NomeMae) && NomeMae != "NÃO DECLARADA" && NomeMae != "NÃO DECLARADO"
                    && NomeMae != "DESCONHECIDA" && NomeMae != "MÃE NÃO INFORMADA" && NomeMae != "DESCONHECIDO")
                {
                    if (NomeMae.IndexOf("'") > 0)
                        NomeMae = NomeMae.Replace("'", "''");

                    contextQuery.Command += String.Format(" P.NOME_MAE like '{0}%'", NomeMae);
                }
                //não apresenta quando a mãe é não declarada ou não declarado 
                //else
                //{
                //    contextQuery.Command += " P.DT_NASC <>  Convert(datetime,@data,103) AND ";
                //    contextQuery.Parameters.Add("@data", DataNascimento);

                //}
                if (!string.IsNullOrEmpty(NomeMae) && NomeMae != "NÃO DECLARADA" && NomeMae != "NÃO DECLARADO" &&
                    NomeMae != "DESCONHECIDA" && NomeMae != "MÃE NÃO INFORMADA" && NomeMae != "DESCONHECIDO" &&
                     !string.IsNullOrEmpty(NomePai) && NomePai != "NÃO DECLARADO"
                     && NomePai != "DESCONHECIDO" && NomePai != "PAI NÃO INFORMADO")
                {
                    contextQuery.Command += " OR ";
                }


                if (!string.IsNullOrEmpty(NomePai) && NomePai != "NÃO DECLARADO" && NomePai != "DESCONHECIDO"
                    && NomePai != "PAI NÃO INFORMADO")
                {
                    if (NomePai.IndexOf("'") > 0)
                        NomePai = NomePai.Replace("'", "''");

                    contextQuery.Command += String.Format(" P.NOME_PAI like '{0}%'", NomePai);

                    if (NomeMae != "NÃO DECLARADA" && NomeMae != "NÃO DECLARADO"
                        && NomeMae != "DESCONHECIDA" && NomeMae != "MÃE NÃO INFORMADA" && NomeMae != "DESCONHECIDO"
                        && !string.IsNullOrEmpty(NomeMae))
                    {
                        contextQuery.Command += ")";
                    }
                }

                //não apresenta quando a pai é não declarado 
                //if ((string.IsNullOrEmpty(NomePai) || NomePai == "NÃO DECLARADO" || NomePai == "DESCONHECIDO"
                //    || NomePai == "PAI NÃO INFORMADO") && !string.IsNullOrEmpty(NomeMae))
                //{
                //    contextQuery.Command += " AND P.DT_NASC <>  Convert(datetime,@data,103)";
                //    contextQuery.Parameters.Add("@data", DataNascimento);
                //}

                if (!contextQuery.Command.EndsWith("WHERE ") && (!contextQuery.Command.EndsWith("AND ")))
                    contextQuery.Command += "  AND";

                if (!string.IsNullOrEmpty(Matricula))
                {
                    contextQuery.Command += String.Format(" A.ALUNO <> '{0}'", Matricula);
                }

                contextQuery.Command += String.Format(" AND  P.PESSOA NOT IN (SELECT PARENTEID FROM RELACAOPESSOA WHERE PESSOAID = {0}", Pessoa);
                contextQuery.Command += ")";
                contextQuery.Command += " AND A.SIT_ALUNO = 'Ativo'";

                if (NomeIrmao.IndexOf("'") > 0)
                    NomeIrmao = NomeIrmao.Replace("'", "''");

                contextQuery.Command += String.Format(" AND P.NOME_COMPL like '{0}%'", NomeIrmao);
                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ConsultarFiliacaoAluno(decimal pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =

                new ContextQuery(@" SELECT NOME_MAE,NOME_PAI  
                         FROM LY_PESSOA 
                         WHERE PESSOA = @PESSOA");
                {
                    contextQuery.Parameters.Add("@PESSOA", pessoa);
                }
                return Consultar(contextQuery);
            }
        }

        public static DataTable ListarIrmaosPessoa(string NomeMae, string NomePai, decimal Pessoa, decimal Parentesco)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =


                        @"SELECT PARENTESCOID =
                              CASE PARENTESCOID
                                 WHEN 1 THEN 'Irmão '
                                 WHEN 2 THEN 'Irmão Gêmeo'
                              END,A.ALUNO,P.DT_NASC,P.NOME_COMPL,P.NOME_MAE,P.NOME_PAI,
                           CERT_NASC_FOLHA,CERT_NASC_LIVRO,
                           CERT_NASC_NUM,CERT_NUMERO_MATRICULA,A.UNIDADE_ENSINO,UE.NOME_COMP,
                            A.UNIDADE_ENSINO + ' - ' + UE.NOME_COMP AS ESCOLA,A.PESSOA
                           FROM LY_ALUNO A
                           INNER JOIN dbo.LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                           INNER JOIN LY_PESSOA P ON A.PESSOA = P.PESSOA INNER JOIN RELACAOPESSOA R"
                };

                contextQuery.Command += " ON R.PARENTEID = P.PESSOA";
                contextQuery.Command += String.Format(" WHERE R.PESSOAID = {0}", Pessoa);
                contextQuery.Command += String.Format(" AND A.SIT_ALUNO = 'Ativo'");

                return ctx.GetDataTable(contextQuery);

            };


        }

        public static DataTable ListarIrmaosPessoaParente(string NomeMae, string NomePai, decimal Pessoa, decimal Parentesco)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =


                        @"SELECT PARENTESCOID =
                              CASE PARENTESCOID
                                 WHEN 1 THEN 'Irmão '
                                 WHEN 2 THEN 'Irmão Gêmeo'
                              END,A.ALUNO,P.DT_NASC,P.NOME_COMPL,P.NOME_MAE,P.NOME_PAI,
                           CERT_NASC_FOLHA,CERT_NASC_LIVRO,
                           CERT_NASC_NUM,CERT_NUMERO_MATRICULA,A.UNIDADE_ENSINO,
                            UE.NOME_COMP,A.UNIDADE_ENSINO + ' - ' + UE.NOME_COMP AS ESCOLA,A.PESSOA
                           FROM LY_ALUNO A
                           INNER JOIN dbo.LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                           INNER JOIN LY_PESSOA P ON A.PESSOA = P.PESSOA INNER JOIN RELACAOPESSOA R"
                };

                contextQuery.Command += String.Format(" ON R.PARENTEID = {0}", Parentesco);

                contextQuery.Command += " WHERE ";


                contextQuery.Command += String.Format(" P.NOME_MAE like '%{0}%'", NomeMae);

                contextQuery.Command += String.Format(" AND P.NOME_PAI like '%{0}%'", NomePai);

                contextQuery.Command += String.Format(" AND A.PESSOA IN (SELECT R.PARENTEID from RELACAOPESSOA where R.PARENTEID = {0}", Parentesco);

                contextQuery.Command += ")";
                return ctx.GetDataTable(contextQuery);

            };


        }

        public static ValidacaoDados ValidarBuscaNovoAluno(string aluno, string nomeCompl, string nomeMae, DateTime dtNasc)
        {
            RN.RecursosHumanos.NomeSemValidacao rnNomeSemValidacao = new Techne.Lyceum.RN.RecursosHumanos.NomeSemValidacao();
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados { Valido = false };
            decimal pessoa = 0;

            if (!aluno.IsNullOrEmptyOrWhiteSpace() && aluno != "0")
            {
                pessoa = Aluno.RetornaPessoa(aluno);
            }

            if (string.IsNullOrEmpty(nomeCompl) && string.IsNullOrEmpty(nomeMae) && dtNasc == null)
            {
                return validacaoDados;
            }

            if (dtNasc != DateTime.MinValue && dtNasc > DateTime.Now.Date)
            {
                mensagens.Add("O campo DATA DE NASCIMENTO não pode ser maior que a data atual.");
            }

            if ((!string.IsNullOrEmpty(nomeCompl) && !string.IsNullOrEmpty(nomeMae)) && nomeCompl == nomeMae)
            {
                mensagens.Add("O campo NOME DO ALUNO não pode ser igual ao NOME DA MÃE.");
            }

            if (!string.IsNullOrEmpty(nomeCompl) && nomeCompl.Length < 5)
            {
                mensagens.Add("O campo NOME DO ALUNO é obrigatório, e deve conter pelo menos cinco letras!");
            }

            if (!string.IsNullOrEmpty(nomeCompl) && !Validacao.SomenteLetras(nomeCompl))
            {
                mensagens.Add("O campo NOME DO ALUNO não pode conter números.");
            }

            /// nome do aluno
            if (!string.IsNullOrEmpty(nomeCompl))
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Nome);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != nomeCompl.ToUpper())
                {
                    var palavras = nomeCompl.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(nomeCompl, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(nomeCompl, new PalavrasProibidasEmNomes());

                    if (Validacao.contemNumeros(nomeCompl) == false)
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, nome e sobrenome no campo NOME DO ALUNO.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("O campo NOME DO ALUNO possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("O campo NOME DO ALUNO possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }
                    else if (!string.IsNullOrEmpty(nomeCompl) && !Validacao.SomenteLetras(nomeCompl))
                    {
                        mensagens.Add("O campo NOME DO ALUNO não pode conter números.");
                    }
                }
            }

            /// nome da mae
            if (!string.IsNullOrEmpty(nomeMae))
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Mae);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != nomeMae.ToUpper())
                {
                    var palavras = nomeMae.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(nomeMae, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(nomeMae, new PalavrasProibidasEmNomes());

                    if (Validacao.contemNumeros(nomeCompl) == false)
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DA MÃE.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("O campo NOME DA MÃE possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("O campo NOME DA MÃE possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }
                    else if (!string.IsNullOrEmpty(nomeMae) && !Validacao.SomenteLetras(nomeMae))
                    {
                        mensagens.Add("O campo NOME DA MÃE não pode conter números.");
                    }
                }
            }

            // ----------------------------------------
            // VERIFICA SE ALUNO JA ESTA CADASTRADO
            // NA TABELA TCE_ABREVIACAO_ALUNO
            // ----------------------------------------

            RN.Pessoa rnPessoa = new Pessoa();
            RN.Validacao rnValidacao = new Validacao();

            if (!ExisteAbreviacao(aluno))
            {
                // nome do aluno
                if (!string.IsNullOrEmpty(nomeCompl))
                {
                    var nomes = nomeCompl.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    for (var i = 0; i < nomes.Length; i++)
                    {
                        var nome = nomes[i];

                        if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                        {
                            nome = nome.Remove(1);
                        }

                        if ((nome.Length == 1 && !rnValidacao.ehAbreviacaoValida(nome) && (string.Compare(nome, "e", true) != 0)) ||
                            ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomes.Length - 1)))
                        {
                            mensagens.Add("Não é possível utilizar abreviações no NOME DO ALUNO.");


                        }
                    }
                }
                // nome da mae
                if (!string.IsNullOrEmpty(nomeMae))
                {
                    var nomesMae = nomeMae.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    for (var i = 0; i < nomesMae.Length; i++)
                    {
                        var nome = nomesMae[i];

                        if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                        {
                            nome = nome.Remove(1);
                        }

                        if ((nome.Length == 1 && !rnValidacao.ehAbreviacaoValida(nome) && (string.Compare(nome, "e", true) != 0)) ||
                            ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomesMae.Length - 1)))
                        {
                            mensagens.Add("Não é possível utilizar abreviações no NOME DA MÃE.");
                        }
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

        public static DataTable ListaAlunoAtivoReabertura(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command =
                        @"DECLARE @NOME_ALUNO VARCHAR(100) ,
                                @NOME_MAE VARCHAR(100) ,
                                @DATA_NASCIMENTO DATETIME


                            SELECT  @NOME_ALUNO = P.NOME_COMPL ,
                                    @DATA_NASCIMENTO = DT_NASC ,
                                    @NOME_MAE = NOME_MAE
                            FROM    lY_aluno A
                                    INNER JOIN dbo.LY_PESSOA P ON A.PESSOA = P.PESSOA
                            WHERE   ALUNO = @ALUNO


                            SELECT  *
                            FROM    lY_aluno A
                                    INNER JOIN dbo.LY_PESSOA P ON A.PESSOA = P.PESSOA
                            WHERE   P.NOME_COMPL = @NOME_ALUNO
                                    AND DT_NASC = @DATA_NASCIMENTO
                                    AND NOME_MAE = @NOME_MAE
                                    AND SIT_ALUNO = 'Ativo' "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);

                return ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool EhAlunoAtivo(decimal pessoa)
        {
            bool alunoAtivo = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            object obj = new Object();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1
                            LY_PESSOA.PESSOA
                    FROM    LY_ALUNO
                            INNER JOIN dbo.LY_PESSOA ON LY_ALUNO.PESSOA = dbo.LY_PESSOA.PESSOA
                    WHERE   LY_PESSOA.PESSOA = @PESSOA
                            AND SIT_ALUNO = 'Ativo'
                    ORDER BY LY_PESSOA.STAMP_ATUALIZACAO DESC "
                };

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    alunoAtivo = true;
                }

                return alunoAtivo;
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

        public bool EhAlunoAtivoPor(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                    FROM    LY_ALUNO A
                                            INNER JOIN DBO.LY_PESSOA P ON A.PESSOA = P.PESSOA
                                    WHERE   P.PESSOA = @PESSOA
                                            AND SIT_ALUNO = 'ATIVO' ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhAlunoAtivoPor(string aluno)
        {
            bool alunoAtivo = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                alunoAtivo = this.EhAlunoAtivoPor(ctx, aluno);
                return alunoAtivo;
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

        public bool EhalunoRegularConcluinte(string aluno)
        {
            bool ehMatriculaRegularConcluinte;

            ContextQuery contextQuery = new ContextQuery(
                     @" SELECT COUNT(1)
                        FROM LY_ALUNO A (NOLOCK)
	                        INNER JOIN DBO.LY_SERIE S (NOLOCK) ON A.CURRICULO = S.CURRICULO
                                           AND A.CURSO = S.CURSO
                                           AND A.SERIE = S.SERIE
                                           AND A.TURNO = S.TURNO
	                        INNER JOIN LY_CURSO C (NOLOCK) ON C.CURSO = A.CURSO
                        WHERE ALUNO = @ALUNO
                            AND ((ANO_SERIE_CONCLUINTE = 'S' AND TIPO IN ( 3, 4 ) OR (A.SERIE = 9))) ");

            contextQuery.Parameters.Add("@ALUNO", aluno);

            ehMatriculaRegularConcluinte = (ExecutarFuncao<int>(contextQuery) > 0);

            return ehMatriculaRegularConcluinte;
        }

        public bool EhAlunoAtivoPor(DataContext ctx, string aluno)
        {
            bool alunoAtivo = false;
            object obj = new Object();

            ContextQuery contextQuery = new ContextQuery
                           {
                               Command = @" SELECT TOP 1 1
                                FROM    LY_ALUNO
                                WHERE   ALUNO = @ALUNO
                                        AND SIT_ALUNO = 'Ativo' "
                           };

            contextQuery.Parameters.Add("@ALUNO", aluno);

            obj = ctx.GetReturnValue(contextQuery);

            if (obj != null)
            {
                alunoAtivo = true;
            }

            return alunoAtivo;
        }

        public void AtivaAtualizaAlunoPor(DataContext contexto, string curso, string turno, string curriculo, int serie, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_ALUNO 
                            SET SIT_ALUNO = 'Ativo', 
                                CURSO = @CURSO ,
                                TURNO = @TURNO ,
                                CURRICULO = @CURRICULO ,
                                SERIE  = @SERIE
                            WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);
            contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, turno);
            contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, curriculo);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtivaMatriculaEmSuspensao(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_ALUNO
                                    SET     SUSPENSO = 0,
                                            DATA_EM_SUSPENSAO = NULL
                                    WHERE   ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            contexto.ApplyModifications(contextQuery);
        }

        public bool EhAlunoMatriculaEmSuspensaoPor(DataContext ctx, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
					FROM    LY_ALUNO
                    WHERE   ALUNO = @ALUNO
                            AND SIT_ALUNO = 'Ativo'
							AND SUSPENSO = 1 ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void AtualizaPessoa(DataContext ctx, decimal pessoaCorreta, decimal pessoaErrada, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE  LY_ALUNO
                            SET     PESSOA = @PESSOACORRETA ,
                                    SIT_ALUNO = 'Cancelado',
                                    USUARIO = @USUARIO,
                                    DATAALTERACAO = @DATAALTERACAO
                            WHERE   PESSOA = @PESSOAERRADA ";

            contextQuery.Parameters.Add("@PESSOACORRETA", pessoaCorreta);
            contextQuery.Parameters.Add("@USUARIO", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
            contextQuery.Parameters.Add("@PESSOAERRADA", pessoaErrada);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaMatriculaFechamento(DataContext ctx, LyAluno aluno)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE  LY_ALUNO
                                SET     CURSO = @CURSO ,
                                        TURNO = @TURNO ,
                                        CURRICULO = @CURRICULO ,
                                        SERIE = @SERIE
                                WHERE   ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@CURSO", aluno.Curso);
                contextQuery.Parameters.Add("@TURNO", aluno.Turno);
                contextQuery.Parameters.Add("@CURRICULO", aluno.Curriculo);
                contextQuery.Parameters.Add("@SERIE", aluno.Serie);
                contextQuery.Parameters.Add("@ALUNO", aluno.Aluno);

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

        public void AtualizaSituacao(DataContext ctx, string situacao, string aluno)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE  LY_ALUNO
                                SET     SIT_ALUNO = @SIT_ALUNO
                                WHERE   ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@SIT_ALUNO", situacao);
                contextQuery.Parameters.Add("@ALUNO", aluno);

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

        public string ObtemAlunoComDuplicidadeDeEmailPor(string aluno, string email)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery query = new ContextQuery();
            string SELECT_EMAIL_DUPLICIDADE = @"SELECT TOP 1 ALUNO                                                    
                                                FROM
                                                    LY_ALUNO
                                                WHERE
                                                    SIT_ALUNO = 'ATIVO' 
                                                    AND ALUNO <> @ALUNO
                                                    AND E_MAIL_INTERNO IS NOT NULL
                                                    AND E_MAIL_INTERNO <> ''
                                                    AND E_MAIL_INTERNO = @EMAIL";
            string matricula = string.Empty;

            try
            {
                query.Command = SELECT_EMAIL_DUPLICIDADE;

                query.Parameters.Add("@ALUNO", aluno);
                query.Parameters.Add("@EMAIL", email);

                matricula = ctx.GetReturnValue<string>(query);
            }
            catch (Exception ex)
            {
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

            return matricula;
        }

        public RN.DTOs.DadosAlunoNotificacao ObtemDadosAlunoNotificacaoPor(string aluno)
        {
            RN.DTOs.DadosAlunoNotificacao dados = new RN.DTOs.DadosAlunoNotificacao();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT P.PESSOA,
	                                            ALUNO,
	                                            P.NOME_COMPL,
	                                            PRE_NOME_SOCIAL,
	                                            P.DT_NASC,
	                                            P.NOME_MAE,
	                                            P.MAE_FALECIDA,
	                                            P.MAE_CPF,
	                                            P.MAE_TELEFONE,
	                                            P.NOME_PAI,
	                                            P.PAI_FALECIDO,
	                                            P.PAI_CPF,
	                                            P.PAI_TELEFONE,
	                                            P.RESPONSAVEL,
	                                            P.RESP_NOME_COMPL,
	                                            P.RESP_CPF,
	                                            P.RESP_FONE,
	                                            P.CEP,
	                                            P.END_MUNICIPIO,
	                                            M.NOME AS MUNICIPIO,
	                                            M.UF,
	                                            P.ENDERECO,
	                                            P.END_NUM,
	                                            P.END_COMPL,
	                                            P.BAIRRO,
	                                            UE.ID_REGIONAL,
	                                            R.REGIONAL,
	                                            UE.UNIDADE_ENS,
                                                C.TIPO AS NIVEL,
                                                C.MODALIDADE AS MODALIDADE,
	                                            TC.DESCRICAO AS NOME_NIVEL,
	                                            MC.DESCRICAO AS NOME_MODALIDADE,
	                                            C.CURSO,
                                                A.TURNO,
	                                            TU.DESCRICAO AS DESC_TURNO,
	                                            A.SERIE,
	                                            A.CURRICULO,
	                                            A.SIT_ALUNO,
	                                            CASE 
		                                            WHEN A.SIT_ALUNO = 'ATIVO' AND ISNULL(A.SUSPENSO,0) = 1 THEN 'MATRÍCULA EM SUSPENSÃO'
		                                            ELSE A.SIT_ALUNO
	                                            END SITUACAO
                                            FROM LY_PESSOA P
                                                INNER JOIN LY_ALUNO A ON P.PESSOA = A.PESSOA
	                                            INNER JOIN LY_UNIDADE_ENSINO UE ON A.UNIDADE_ENSINO = UE.UNIDADE_ENS
	                                            INNER JOIN TCE_REGIONAL R ON UE.ID_REGIONAL = R.ID_REGIONAL
	                                            INNER JOIN LY_CURSO C ON A.CURSO = C.CURSO
	                                            INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE
	                                            INNER JOIN LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
	                                            INNER JOIN LY_TURNO TU ON A.TURNO = TU.TURNO
                                                LEFT JOIN HADES..TCE_MUNICIPIO m ON p.End_municipio = m.ID_MUNICIPIO
                                            WHERE A.ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    dados.Aluno = Convert.ToString(reader["ALUNO"]);
                    dados.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    dados.NomeSocial = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                    dados.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    dados.Idade = Utils.CalcularIdade(Convert.ToDateTime(dados.DataNascimento));

                    if (Convert.ToString(reader["NOME_MAE"]) != "NÃO DECLARADO")
                    {
                        dados.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                        dados.MaeNaoDeclarada = false;
                    }
                    else
                    {
                        dados.MaeNaoDeclarada = true;
                    }

                    if (Convert.ToString(reader["MAE_FALECIDA"]) == "S")
                    {
                        dados.MaeFalecida = true;
                    }
                    else
                    {
                        dados.MaeFalecida = false;
                    }

                    dados.MaeCpf = Convert.ToString(reader["MAE_CPF"]);
                    dados.MaeTelefone = Convert.ToString(reader["MAE_TELEFONE"]);

                    if (Convert.ToString(reader["NOME_PAI"]) != "NÃO DECLARADO")
                    {
                        dados.NomePai = Convert.ToString(reader["NOME_PAI"]);
                        dados.PaiNaoDeclarado = false;
                    }
                    else
                    {
                        dados.PaiNaoDeclarado = true;
                    }

                    if (Convert.ToString(reader["PAI_FALECIDO"]) == "S")
                    {
                        dados.PaiFalecido = true;
                    }
                    else
                    {
                        dados.PaiFalecido = false;
                    }

                    dados.PaiCpf = Convert.ToString(reader["PAI_CPF"]);
                    dados.PaiTelefone = Convert.ToString(reader["PAI_TELEFONE"]);
                    dados.ResponsavelLegal = Convert.ToString(reader["RESPONSAVEL"]);
                    dados.ResponsavelNome = Convert.ToString(reader["RESP_NOME_COMPL"]);
                    dados.ResponsavelCpf = Convert.ToString(reader["RESP_CPF"]);
                    dados.ResponsavelTelefone = Convert.ToString(reader["RESP_FONE"]);
                    dados.Cep = Convert.ToString(reader["CEP"]);
                    dados.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dados.CodMunicipio = Convert.ToString(reader["END_MUNICIPIO"]);
                    dados.Estado = Convert.ToString(reader["UF"]);
                    dados.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dados.Numero = Convert.ToString(reader["END_NUM"]);
                    dados.Complemento = Convert.ToString(reader["END_COMPL"]);
                    dados.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dados.Regional = Convert.ToString(reader["ID_REGIONAL"]);
                    dados.Censo = Convert.ToString(reader["UNIDADE_ENS"]);
                    dados.Nível = Convert.ToString(reader["NIVEL"]);
                    dados.Modalidade = Convert.ToString(reader["MODALIDADE"]);
                    dados.Curso = Convert.ToString(reader["CURSO"]);
                    dados.Turno = Convert.ToString(reader["TURNO"]);
                    dados.Serie = Convert.ToString(reader["SERIE"]);
                    dados.Situacao = Convert.ToString(reader["SITUACAO"]);
                    dados.Curriculo = Convert.ToString(reader["CURRICULO"]);

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

        internal void AtualizaDadosTransferencia(LyAluno lyAluno, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"UPDATE  LY_ALUNO
                                     SET     CURSO = @CURSO,
                                             TURNO = @TURNO,
                                             CURRICULO = @CURRICULO,
                                             SERIE = @SERIE,
                                             UNIDADE_ENSINO = @UNIDADE_ENSINO,
                                             UNIDADE_FISICA = @UNIDADE_FISICA,
                                             TIPO_ENSINO_PROFISSIONALIZANTE = @TIPO_ENSINO_PROFISSIONALIZANTE,
                                             SIT_ALUNO = @SIT_ALUNO
                                     WHERE   ALUNO = @ALUNO";


            contextQuery.Parameters.Add("@CURSO", lyAluno.Curso);
            contextQuery.Parameters.Add("@TURNO", lyAluno.Turno);
            contextQuery.Parameters.Add("@CURRICULO", lyAluno.Curriculo);
            contextQuery.Parameters.Add("@SERIE", lyAluno.Serie);
            contextQuery.Parameters.Add("@UNIDADE_ENSINO", lyAluno.UnidadeEnsino);
            contextQuery.Parameters.Add("@UNIDADE_FISICA", lyAluno.UnidadeFisica);
            contextQuery.Parameters.Add("@TIPO_ENSINO_PROFISSIONALIZANTE", lyAluno.TipoEnsinoProfissionalizante);
            contextQuery.Parameters.Add("@ALUNO", lyAluno.Aluno);
            contextQuery.Parameters.Add("@SIT_ALUNO", "Ativo");

            listaContextQuery.Add(contextQuery);
        }

        public bool EhAlunoPor(decimal pessoa)
        {
            bool aluno = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                aluno = this.EhAlunoPor(ctx, pessoa);
                return aluno;
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

        public bool EhAlunoPor(DataContext ctx, decimal pessoa)
        {
            bool aluno = false;
            object obj = new Object();

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT TOP 1 1
                                FROM    LY_ALUNO
                                WHERE   PESSOA = @PESSOA
                                        "
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            obj = ctx.GetReturnValue(contextQuery);

            if (obj != null)
            {
                aluno = true;
            }

            return aluno;
        }

        public DataTable ObtemDadosAlunoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT 
                                       A.ALUNO                   AS ALUNO, 
                                       A.CURSO                   AS CURSO, 
                                       C.NOME                    AS NOME_CURSO, 
                                       ISNULL(C.COBRAN_DISC, '') COBRAN_DISC, 
                                       A.TURNO                   AS TURNO, 
                                       T.DESCRICAO               AS NOME_TURNO, 
                                       A.CURRICULO               AS CURRICULO, 
                                       A.SERIE                   AS SERIE, 
                                       S.DESCRICAO               AS NOME_SERIE, 
                                       A.SIT_ALUNO               AS SIT_ALUNO, 
                                       A.ANO_INGRESSO            AS ANO_INGRESSO, 
                                       A.SEM_INGRESSO            AS SEM_INGRESSO, 
                                       A.UNIDADE_ENSINO          AS UNI_ENSINO, 
                                       U.NOME_COMP               AS NOME_UNI_ENSINO, 
                                       A.UNIDADE_FISICA          AS UNI_FISICA, 
                                       UF.NOME_COMP              AS NOME_UNI_FISICA 
                                FROM   LY_ALUNO A 
                                       JOIN LY_CURSO C 
                                         ON A.CURSO = C.CURSO 
                                       JOIN LY_TURNO T 
                                         ON A.TURNO = T.TURNO 
                                       JOIN LY_SERIE S 
                                         ON A.CURSO = S.CURSO 
                                            AND A.TURNO = S.TURNO 
                                            AND A.CURRICULO = S.CURRICULO 
                                            AND A.SERIE = S.SERIE 
                                       JOIN LY_UNIDADE_ENSINO U 
                                         ON A.UNIDADE_ENSINO = U.UNIDADE_ENS 
                                       JOIN LY_UNIDADE_FISICA UF 
                                         ON A.UNIDADE_FISICA = UF.UNIDADE_FIS 
                                WHERE  A.ALUNO =@ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

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

        public bool EhAlunoEmMunicipioBilhetagemEletronicaPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(*)
                                    FROM   LY_ALUNO A
                                           INNER JOIN LY_UNIDADE_ENSINO UE
                                                   ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                           INNER JOIN CARTAOESTUDANTE.MUNICIPIOBILHETAGEM MB
                                                   ON MB.MUNICIPIOID = UE.MUNICIPIO
                                    WHERE  ALUNO = @ALUNO   "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool EhAlunoSemNecessidadeEspecialPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(*)
                                    FROM   LY_ALUNO A
                                           INNER JOIN LY_PESSOA P
                                                   ON P.PESSOA=A.PESSOA
                                    WHERE  ALUNO = @ALUNO   
                                           AND P.NECESSIDADEESPECIALID = 30"
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public void CancelaAlunoPessoaDuplicadaPor(DataContext ctx, string aluno, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE  LY_ALUNO
                            SET     SIT_ALUNO = 'Cancelado',
                                    USUARIO = @USUARIO,
                                    DATAALTERACAO = @DATAALTERACAO
                            WHERE   ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@USUARIO", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public string ObtemAlunoMaisPessoaAtivaPor(decimal pessoa, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            string matricula = string.Empty;

            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"SELECT  a.ALUNO
                            FROM   ly_aluno A
                            INNER JOIN     Matricula.PESSOAALUNO P ON P.PESSOAID=A.PESSOA AND p.ALUNO = @ALUNO
                            WHERE A.PESSOA = @PESSOA   
                            AND SIT_ALUNO = 'Ativo'                       
                            AND a.ALUNO <> @ALUNO ";
            try
            {
                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                matricula = ctx.GetReturnValue<string>(contextQuery);
            }
            catch (Exception ex)
            {
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

            return matricula;
        }

        public bool ParticipouMatriculaFacilPor(string aluno)
        {
            Seeduc.Infra.Data.DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool retorno = false;

            try
            {
                retorno = this.ParticipouMatriculaFacilPor(contexto, aluno);
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

        public bool ParticipouMatriculaFacilPor(Seeduc.Infra.Data.DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT count(*)
                                        FROM   TCE_CONFIRMACAO_MATRICULA
                                        WHERE ALUNO = @ALUNO
                                        AND MATRICULAFACIL = 1
                                           ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DadosEncaminhamentoEspecial ObtemDadosEncaminhamentoEspecialPor(string nome, string nomeMae, DateTime dataNascimento)
        {
            DadosEncaminhamentoEspecial dados = new DadosEncaminhamentoEspecial();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.Matriculas.PreCadastroAluno rnPreCadastroAluno = new Techne.Lyceum.RN.Matriculas.PreCadastroAluno();
            RN.Pessoa rnPessoa = new Pessoa();

            try
            {
                //Busca dados pela pessoa
                dados = rnPessoa.ObtemDadosEncaminhamentoEspecialPor(contexto, nome, nomeMae, dataNascimento);

                if (dados.Pessoa == null || dados.Pessoa <= 0)
                {
                    //Busca no precadastro
                    dados = rnPreCadastroAluno.ObtemDadosEncaminhamentoEspecialPor(contexto, nome, nomeMae, dataNascimento);
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

        public ValidacaoDados ValidaEncaminhamentoEspecial(DadosEncaminhamentoEspecial dados)
        {
            List<string> mensagens = new List<string>();
            Regex regex = new Regex(@"\s{2,}");
            DataContext contexto = null;
            RN.Curriculo rnCurriculo = new Curriculo();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Turma rnTurma = new Turma();
            RN.Matricula rnMatricula = new Matricula();
            RN.Matriculas.PessoaAluno rnPessoaAluno = new Techne.Lyceum.RN.Matriculas.PessoaAluno();
            RestricaoIdadeSerie rnRestricaoIdadeSerie = new RestricaoIdadeSerie();
            TceRestricaoIdadeSerie restricao = new TceRestricaoIdadeSerie();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Entidades.TceControleVaga controleVaga = new TceControleVaga();
            decimal pessoaBase = 0;
            int idade = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dados == null)
            {
                return validacaoDados;
            }

            if (dados.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else
            {
                /// Validaçoes de nome do aluno 
                if (dados.Nome.Length < 5)
                {
                    mensagens.Add("Campo NOME deve conter pelo menos cinco letras!");
                }

                if (!string.IsNullOrEmpty(dados.Nome)
                    && !Validacao.SomenteLetras(dados.Nome))
                {
                    mensagens.Add("Campo NOME não pode conter números.");
                }

                var palavras = dados.Nome.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(dados.Nome, 3);
                var nomeInvalido = TextValidator.HasForbiddenWords(dados.Nome, new PalavrasProibidasEmNomes());
                if (Validacao.contemNumeros(dados.Nome) == false)
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME.");
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
                string NomeCompl = regex.Replace(dados.Nome.Trim().ToUpper(), " ");
                var contemApostrofeRep = Validacao.substitueApostrofe(NomeCompl);
                if (contemApostrofeRep)
                {
                    mensagens.Add("Campo NOME DO ALUNO possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (dados.DataNascimento == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE NASCIMENTO é obrigatório.");
            }
            else
            {
                idade = Utils.CalcularIdade(Convert.ToDateTime(dados.DataNascimento));

                if (idade > 110)
                {
                    mensagens.Add("A idade não pode ser superior a 110 anos. Favor verificar a Data de Nascimento.");
                }
            }

            if (dados.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SEXO é obrigatório.");
            }

            if (dados.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }
            else
            {
                /// Validaçoes de nome da mae

                if (dados.NomeMae == "NÃO DECLARADA" && !dados.MaeNãoDeclarada)
                {
                    mensagens.Add("Se o Nome da Mãe for Não Declarada é necessário que a informação de declaração esteja marcada.");
                }

                var palavras = dados.NomeMae.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(dados.NomeMae, 3);
                var nomeInvalido = TextValidator.HasForbiddenWords(dados.NomeMae, new PalavrasProibidasEmNomes());
                if (!Validacao.contemNumeros(dados.NomeMae))
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DA MÃE.");
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
                else if (!Validacao.SomenteLetras(dados.NomeMae))
                {
                    mensagens.Add("Campo NOME DA MÃE não pode conter números.");
                }

                string NomeMae = regex.Replace(dados.NomeMae.Trim().ToUpper(), " ");
                var contemApostrofeRepMae = Validacao.substitueApostrofe(NomeMae);
                if (contemApostrofeRepMae)
                {
                    mensagens.Add("Campo NOME DO MÃE possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (!dados.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                /// Validações de nome do pai

                if (dados.NomePai == "NÃO DECLARADO" && !dados.PaiNãoDeclarado)
                {
                    mensagens.Add("Se o Nome do Pai for Não Declarado é necessário que a informação de declaração esteja marcada.");
                }

                var palavras = dados.NomePai.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(dados.NomePai, 3);
                var nomeInvalido = TextValidator.HasForbiddenWords(dados.NomePai, new PalavrasProibidasEmNomes());
                if (Validacao.contemNumeros(dados.NomePai) == false)
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DO PAI.");
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
                else if (!string.IsNullOrEmpty(dados.NomePai) && !Validacao.SomenteLetras(dados.NomePai))
                {
                    mensagens.Add("Campo NOME DO PAI não pode conter números.");
                }

                string NomePai = regex.Replace(dados.NomePai.Trim().ToUpper(), " ");
                var contemApostrofeRepPai = Validacao.substitueApostrofe(NomePai);
                if (contemApostrofeRepPai)
                {
                    mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (!string.IsNullOrEmpty(dados.NomeMae) && !string.IsNullOrEmpty(dados.NomePai))
            {
                if (dados.NomeMae == dados.NomePai)
                {
                    mensagens.Add("Campo NOME DO PAI não pode ser idêntico ao NOME DA MÃE.");
                }
            }

            if (dados.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório!");
            }
            else
            {
                var cep = Utils.RetirarMascara(dados.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (dados.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório!");
            }
            else
            {
                if (dados.Endereco.Length > 50)
                {
                    mensagens.Add("Campo ENDEREÇO deve conter no máximo 50 caracteres!");
                }
            }

            if (dados.NumeroEndereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO ENDEREÇO é obrigatório.");
            }

            if (dados.MunicipioEndereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO é obrigatório.");
            }

            if (dados.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }
            else
            {
                if (!Validacao.Bairro(dados.Bairro))
                {
                    mensagens.Add("Campo BAIRRO é inválido!");
                }
            }

            if (!dados.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                string cpf = Utils.RetirarMascara(dados.Cpf);

                if (!Validacao.ValidaCpf(cpf))
                {
                    mensagens.Add("O CPF informado não é válido!");
                }
            }

            if (dados.NecessidadeEspecialId == null || dados.NecessidadeEspecialId <= 0)
            {
                mensagens.Add("Campo NECESSIDADE ESPECIAL é obrigatório.");
            }

            if (dados.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (dados.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (dados.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (dados.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (dados.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (dados.Serie < 0)
            {
                mensagens.Add("Campo SERIE é obrigatório.");
            }

            if (dados.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (dados.MotivoEncaminhamentoEspecial <= 0)
            {
                mensagens.Add("Campo MOTIVO DO ENCAMINHAMENTO ESPECIAL é obrigatório.");
            }

            if (!dados.Observacao.IsNullOrEmptyOrWhiteSpace())
            {
                if (dados.Observacao.Length > 500)
                {
                    mensagens.Add("Campo OBSERVAÇÃO deve conter no máximo 500 caracteres.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca curriculo
                    dados.Curriculo = rnCurriculo.ObtemPrimeiroCurriculoAtivoPor(contexto, dados.Curso, dados.Turno, dados.Serie, dados.Ano, dados.Periodo);
                    if (dados.Curriculo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Não foi encontrada MATRIZ CURRICULAR para este ano / periodo / curso / turno / serie.");
                    }

                    //Busca controle Vaga
                    controleVaga = rnControleVaga.ObtemPor(contexto, dados.Ano, dados.Periodo, dados.Censo, dados.Curso, dados.Turno, dados.Serie);
                    if (controleVaga.IdControleVaga <= 0)
                    {
                        mensagens.Add("Não foram criadas vagas para este ano / periodo / censo / curso / turno / serie.");
                    }
                    else
                    {
                        dados.ControleVagaId = controleVaga.IdControleVaga;
                    }

                    if (dados.Pessoa == null || dados.Pessoa <= 0)
                    {
                        //Verifica se existe pessoa com msm nome / mae / nascimento
                        pessoaBase = rnPessoa.ObtemPessoaPor(contexto, dados.Nome, dados.NomeMae, dados.DataNascimento);

                        if (pessoaBase > 0)
                        {
                            //Caso encontre atualiza pessoa
                            dados.Pessoa = pessoaBase;
                        }
                    }

                    if (dados.Pessoa != null || dados.Pessoa > 0)
                    {
                        //Caso exista pessoa busca dados de matricula
                        dados.PessoaAluno = rnPessoaAluno.ObtemPor(contexto, Convert.ToDecimal(dados.Pessoa));
                    }

                    if (!dados.Cpf.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (rnPessoa.PossuiCPFPor(contexto, dados.Cpf, dados.Pessoa == null ? 0 : Convert.ToDecimal(dados.Pessoa)))
                        {
                            mensagens.Add("Já existe uma pessoa cadastrada com este CPF.");
                        }
                    }
                    else
                    {
                        dados.Cpf = null;
                    }

                    if (!dados.PessoaAluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Verifica se aluno já possui uma enturmação ativa para os dados escolhidos
                        if (rnMatricula.PossuiMatriculaPrincipalAtivaPor(contexto, dados.PessoaAluno.Aluno, dados.Ano, dados.Periodo, dados.Censo, dados.Curso, dados.Turno, dados.Serie))
                        {
                            mensagens.Add("Este aluno já está enturmado nesta escola / ano / periodo / curso / turno / serie.");
                        }

                        if (dados.PessoaAluno.EscolaAluno != dados.Censo)
                        {
                            //Busca turma com vaga
                            dados.Turma = rnTurma.ObtemPrimeiraTurmaComVagaPor(contexto, dados.Ano, dados.Periodo, dados.Censo, dados.Turno, dados.Curso, dados.Serie);

                            if (dados.Turma.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Não existe turma com vagas disponíveis para a transferência do aluno.");
                            }
                        }
                    }

                    restricao = rnRestricaoIdadeSerie.CarregaRestricaoPor(contexto, dados.Curso, dados.Serie);

                    //Verifica se o aluno possui necessidade especial
                    if (dados.NecessidadeEspecialId == 30)//"Não possui."
                    {
                        //Para Alunos sem necessidades Especiais Verificar restrição de idade minima e maxima
                        if (idade < restricao.IdadeMinima || idade > restricao.IdadeMaxima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado é permitido cadastrar alunos entre {0} e {1} anos. Favor verificar a DATA DE NASCIMENTO!",
                                restricao.IdadeMinima,
                                restricao.IdadeMaxima));
                        }
                    }
                    else
                    {
                        //Para Alunos com necessidades Especiais Verificar restrição de idade minima
                        if (idade < restricao.IdadeMinima)
                        {
                            mensagens.Add(string.Format("Para o curso selecionado não é permitido cadastrar alunos com necessidade especial com menos de {0} anos. Favor verificar a DATA DE NASCIMENTO!",
                                 restricao.IdadeMinima));
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

        public ValidacaoDados ValidaAtualizacaoCpf(string aluno, string cpf, string usuarioId, decimal pessoa)
        {
            Pessoa rnPessoa = new Pessoa();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (cpf.IsNullOrEmptyOrWhiteSpace())
            {
                cpf = null;
            }
            else
            {
                cpf = Utils.RetirarMascara(cpf);

                if (!Validacao.ValidaCpf(cpf))
                {
                    mensagens.Add("O CPF informado não é válido!");
                }
            }

            if (pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    if (!cpf.IsNullOrEmptyOrWhiteSpace())
                    {
                        contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                        //Verifica se existe outro aluno com o mesmo cpf
                        string retorno = this.PossuiOutroCPFPor(contexto, cpf, aluno);
                        if (!retorno.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add(string.Format("O aluno {0} já foi cadastrado com este CPF.", retorno));
                        }
                        else
                        {
                            //Verifica se existe outra pessoa (docente / servidor) com o mesmo cpf
                            if (rnPessoa.PossuiOutroCPFPor(contexto, cpf, pessoa))
                            {
                                mensagens.Add("CPF já existente cadastrado para um docente / servidor.");
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

        public void AtualizaCpf(string cpf, string usuarioId, decimal pessoa)
        {
            cpf = Utils.RetirarMascara(cpf);

            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_PESSOA 
                        SET  CPF = @CPF,
                             STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO, 
                             USUARIOID = @USUARIOID, 
                             DATAALTERACAO = @DATAALTERACAO 
                    WHERE  PESSOA = @PESSOA  ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
                contextQuery.Parameters.Add("@CPF", cpf.IsNullOrEmptyOrWhiteSpace() ? null : cpf);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@USUARIOID", usuarioId);
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

        public ValidacaoDados ValidaImpressaoFicha(string aluno, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se todos os campos obrigatorios do aluno foram preenchidos
                    if (this.PossuiCamposObrigatoriosVaziosPor(contexto, aluno))
                    {
                        mensagens.Add("Para impressão da ficha é necessário que TODOS os campos obrigatorios estejam preenchidos.");
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

        private bool PossuiCamposObrigatoriosVaziosPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM   LY_ALUNO A (NOLOCK) 
                                           INNER JOIN LY_PESSOA P (NOLOCK) 
                                                   ON A.PESSOA = P.PESSOA 
                                           INNER JOIN LY_FL_PESSOA FL (NOLOCK) 
                                                   ON P.PESSOA = FL.PESSOA 
                                    WHERE  ALUNO = @ALUNO 
                                           AND SIT_ALUNO = 'Ativo' 
                                           AND ( NOME_COMPL IS NULL 
                                                  OR DT_NASC IS NULL 
                                                  OR SEXO IS NULL 
                                                  OR ETNIA IS NULL 
                                                  OR NECESSIDADEESPECIALID IS NULL 
                                                  OR EST_CIVIL IS NULL 
                                                  OR PAIS_NASC IS NULL 
                                                  OR NACIONALIDADE IS NULL 
                                                  OR ( NACIONALIDADE = 'BRASILEIRA' AND MUNICIPIO_NASC IS NULL ) 
                                                  OR CREDO IS NULL 
                                                  OR NOME_MAE IS NULL 
                                                  OR NOME_PAI IS NULL 
                                                  OR RESPONSAVEL IS NULL 
                                                  OR CEP IS NULL 
                                                  OR END_MUNICIPIO IS NULL 
                                                  OR ENDERECO IS NULL 
                                                  OR END_NUM IS NULL 
                                                  OR BAIRRO IS NULL 
                                                  OR FL.FL_FIELD_01 IS NULL -- ZONA RESIDENCIAL 
                                                  OR FL_FIELD_02 IS NULL -- TIPO CERTIDAO 
                                                  OR ANO_INGRESSO IS NULL 
                                                  OR SEM_INGRESSO IS NULL 
                                                  OR TIPO_INGRESSO IS NULL 
                                                  OR REDE_ENSINO_ORIGEM IS NULL 
                                                  OR FL_FIELD_04 IS NULL -- UTILIZA TRANSPORTE 
                                                  OR FL_FIELD_10 IS NULL -- PODER RESPONSAVEL 
                                                  OR FL_FIELD_03 IS NULL -- RECEBE ESCOLARIZAÇÃO EM OUTRO ESPAÇO
                                                )  ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void InsereEncaminhamentoEspecial(DadosEncaminhamentoEspecial dados)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Matriculas.EncaminhamentoEspecial rnEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.EncaminhamentoEspecial();
            RN.HCursosConcl rnHCursosConcl = new HCursosConcl();
            RN.Transferencia rnTransferencia = new Transferencia();
            RN.Matriculas.InscricaoAluno rnInscricaoAluno = new RN.Matriculas.InscricaoAluno();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Matriculas.ContatoOpcaoInscricaoHist rnContatoOpcaoInscricaoHist = new RN.Matriculas.ContatoOpcaoInscricaoHist();
            RN.Matriculas.ContatoOpcaoInscricao rnContatoOpcaoInscricao = new RN.Matriculas.ContatoOpcaoInscricao();
            RN.Matriculas.PreCadastroAluno rnPreCadastroAluno = new RN.Matriculas.PreCadastroAluno();
            RN.Matriculas.OpcaoInscricaoHist rnOpcaoInscricaoHist = new RN.Matriculas.OpcaoInscricaoHist();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Matricula rnMatricula = new Matricula();
            RN.Matgrade rnMatgrade = new Matgrade();
            RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            string tipoVaga = string.Empty;
            bool confirmado = false;
            int? motivoRejeicaoInscricaoId = null;
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            RN.Matriculas.Entidades.OpcaoInscricao opcaoInscricao = new Techne.Lyceum.RN.Matriculas.Entidades.OpcaoInscricao();

            try
            {
                if (!dados.Cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    //Busca a pessoa por CPF
                    string cpf = dados.Cpf.RetirarMascaraCPF();
                    decimal pessoaCPF = rnPessoa.ObtemPessoaPor(contexto, cpf);

                    //Verifica se a pessoa do cpf é diferente do instrito
                    if (pessoaCPF > 0 && pessoaCPF != dados.Pessoa)
                    {
                        //Limpa o CPF                               
                        dados.Cpf = null;
                    }
                }

                if (dados.Pessoa == null || dados.Pessoa <= 0)
                {
                    //Insere Pessoa
                    decimal pessoa;

                    //Verifica se existe precadastro
                    if (dados.PreCadastroAlunoId != null && dados.PreCadastroAlunoId > 0)
                    {
                        //Insere Pessoa
                        rnPessoa.InsereEncaminhamentoEspecial(contexto, Convert.ToInt32(dados.PreCadastroAlunoId), dados, out pessoa);

                        //Atualiza pessoa gerada no precadastro
                        rnPreCadastroAluno.AtualizaPessoa(contexto, Convert.ToInt32(dados.PreCadastroAlunoId), pessoa, dados.UsuarioResponsavel);

                        //insere FlPessoa
                        Entidades.LyFlPessoa flPessoa = new LyFlPessoa();
                        rnFlPessoa.InsereFlPessoaPreCadastro(contexto, Convert.ToInt32(dados.PreCadastroAlunoId), flPessoa);
                    }
                    else
                    {
                        //insere pessoa
                        rnPessoa.InsereEncaminhamentoEspecial(contexto, dados, out pessoa);
                    }

                    dados.Pessoa = pessoa;
                }
                else
                {
                    //Verifica se existe precadastro
                    if (dados.PreCadastroAlunoId != null && dados.PreCadastroAlunoId > 0)
                    {
                        //Atualiza pessoa encontrada no precadastro
                        rnPreCadastroAluno.AtualizaPessoa(contexto, Convert.ToInt32(dados.PreCadastroAlunoId), Convert.ToDecimal(dados.Pessoa), dados.UsuarioResponsavel);
                    }

                    //Atualiza Pessoa
                    rnPessoa.AtualizaEncaminhamentoEspecial(contexto, dados);

                    //Verifica se já existe matricula para a pessoa
                    if (!dados.PessoaAluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Verifica se aluno está ativo
                        if (dados.PessoaAluno.SituacaoAluno != "Ativo")
                        {
                            //Reabre aluno
                            rnHCursosConcl.ReabreAlunoPreCadastro(contexto, dados.PessoaAluno.Aluno, dados.Ano, dados.Periodo);
                        }
                        else
                        {
                            //cancela matricula
                            rnMatricula.CancelaMatriculaPor(contexto, dados.PessoaAluno.Aluno, dados.UsuarioResponsavel);

                            //Cancela matgrade
                            rnMatgrade.CancelaMatgradePor(contexto, dados.PessoaAluno.Aluno);

                            //Cancela Renovaçoes
                            rnRenovacao.CancelaRenovacaoPor(contexto, dados.PessoaAluno.Aluno, dados.UsuarioResponsavel);
                        }

                        //Verifica se aluno é de outra escola
                        if (dados.PessoaAluno.EscolaAluno != dados.Censo)
                        {
                            //Cria transferencia para a turma, e enturma o aluno
                            rnTransferencia.GeraTransferenciaEnturmaEncaminhamentoEspecial(contexto, dados);
                        }
                    }
                }

                if (dados.PessoaAluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                {
                    //Gera matricula para o novo aluno
                    LyAluno aluno = new LyAluno();
                    aluno.UnidadeEnsino = dados.Censo;
                    aluno.AnoIngresso = dados.Ano;
                    this.GeraCodigoAluno(contexto, aluno);
                    dados.PessoaAluno.Aluno = aluno.Aluno;

                    //Cria aluno
                    this.InsereEncaminhamentoEspecial(contexto, dados);
                }
                else
                {
                    //Atualiza Aluno
                    this.AtualizaEncaminhamentoEspecial(contexto, dados);

                    //Cancela Confirmações
                    rnConfirmacaoMatricula.CancelaPossiveisConfirmacaoMatriculaPor(contexto, dados.PessoaAluno.Aluno, dados.Ano, dados.Periodo, dados.UsuarioResponsavel);
                }

                tipoVaga = "VU";

                //Cria confirmacao confirmada
                int confirmacaoId;
                rnConfirmacaoMatricula.InsereEncaminhamentoEspecial(contexto, dados, tipoVaga, out confirmacaoId);

                //Insere na tabela de controle
                rnEncaminhamentoEspecial.Insere(contexto, dados.MotivoEncaminhamentoEspecial, dados.Observacao, confirmacaoId, dados.ControleVagaId, dados.PessoaAluno.Aluno, dados.UsuarioResponsavel);

                //Inclui uma vaga vn
                rnControleVaga.IncrementaVagaNova(contexto, dados.ControleVagaId);

                //Verifica se existe precadastro
                if (dados.PreCadastroAlunoId != null && dados.PreCadastroAlunoId > 0)
                {
                    //Busca uma opção igual para aluno
                    opcaoInscricao = rnOpcaoInscricao.ObtemPor(contexto, dados.ControleVagaId, confirmacaoId);
                    if (opcaoInscricao.OpcaoInscricaoId > 0)
                    {
                        //Coloca dados para opção aceita
                        confirmado = true;
                        motivoRejeicaoInscricaoId = null;

                        if (opcaoInscricao.Fase == 1)
                        {
                            rnInscricaoAluno.ConfirmaFase1(contexto, opcaoInscricao.InscricaoAlunoId, true);
                        }
                        else
                        {
                            rnInscricaoAluno.ConfirmaFase2(contexto, opcaoInscricao.InscricaoAlunoId, true);
                        }

                        //Insere opção no historico
                        rnOpcaoInscricaoHist.Insere(contexto, opcaoInscricao.OpcaoInscricaoId, confirmado, motivoRejeicaoInscricaoId);

                        //Leva todos os contatos da opção para historio
                        rnContatoOpcaoInscricaoHist.Insere(contexto, opcaoInscricao.OpcaoInscricaoId);

                        //Remover todos os contatos da opção 
                        rnContatoOpcaoInscricao.Remove(contexto, opcaoInscricao.OpcaoInscricaoId);

                        //Remover opção 
                        rnOpcaoInscricao.Remove(contexto, opcaoInscricao.OpcaoInscricaoId);
                    }
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

        public DadosFichaAluno ObtemFichaAlunoPorPessoa(string pessoa)
        {
            DadosFichaAluno dados = new DadosFichaAluno();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.PESSOA ,
							A.ALUNO ,
                            p.NOME_COMPL ,
                            P.DT_NASC ,
                            P.SEXO ,
                            P.QT_FILHOS ,
                            P.TIPO_SANGUINEO ,
                            P.ETNIA ,
                            P.EST_CIVIL ,
                            PA.NOME AS PAIS_NASC ,
                            UPPER(NA.NOME) AS NACIONALIDADE ,
                            MNASC.UF AS UF_NASC ,
                            MNASC.NOME AS MUNICIPIO_NASC ,
                            P.CREDO ,
							P.NECESSIDADEESPECIALID ,
                            NEC.DESCRICAO AS NECESSIDADE_ESPECIAL,
							P.NOME_MAE ,
                            P.MAE_FALECIDA ,
                            P.MAE_CPF ,
                            P.NOME_PAI ,
                            P.PAI_FALECIDO ,
                            P.PAI_CPF ,
							P.RESPONSAVEL ,
                            P.RESP_NOME_COMPL ,
                            P.RESP_CPF ,
                            MAE_TELEFONE ,
                            PAI_TELEFONE ,
                            RESP_FONE ,
							P.ENDERECO ,
                            P.END_NUM ,
                            P.END_COMPL ,
                            P.BAIRRO ,
                            MEND.NOME AS END_MUNICIPIO ,
                            MEND.UF AS END_UF ,
                            P.CEP ,
                            FLP.FL_FIELD_01 AS LOCALIZACAO ,  
                            P.FONE ,
                            P.CELULAR ,
                            A.E_MAIL_INTERNO ,
							P.CPF ,
                            P.RG_TIPO ,
                            P.RG_NUM ,
                            FLP.FL_FIELD_07 AS COMPLETMENTO_RG ,
                            P.RG_UF ,
                            P.RG_EMISSOR ,
                            P.RG_DTEXP ,
                            P.ID_CENSO ,
                            FLP.FL_FIELD_08 AS NIS ,
                            FLP.FL_FIELD_02 AS TIPO_CERTIDAO ,
                            FLP.FL_FIELD_09 AS CERTIDAO_CIVIL ,
                            C.UF AS UF_CARTORIO ,
                            C.MUNICIPIO AS MUNICIPIO_CARTORIO ,
                            C.NOME_CARTORIO ,
                            P.CERT_NASC_LIVRO ,
                            P.CERT_NASC_FOLHA ,
                            P.CERT_NASC_NUM ,
                            P.CERT_NASC_EMISSAO ,
                            P.CERT_NUMERO_MATRICULA ,
                            FLP.FL_FIELD_23 AS QUILOMBOLA_INDIGENA
                    FROM   LY_ALUNO A ( NOLOCK ) 
                            INNER JOIN DBO.LY_PESSOA P ( NOLOCK ) ON A.PESSOA = P.PESSOA
                            LEFT JOIN DBO.LY_FL_PESSOA FLP ( NOLOCK ) ON P.PESSOA = FLP.PESSOA
                            LEFT JOIN HADES.DBO.HD_MUNICIPIO MNASC ( NOLOCK ) ON P.MUNICIPIO_NASC = MNASC.MUNICIPIO
                            LEFT JOIN HADES.DBO.HD_PAIS PA ( NOLOCK ) ON P.PAIS_NASC = PA.PAIS
                            LEFT JOIN HADES.DBO.HD_MUNICIPIO MEND ( NOLOCK ) ON P.END_MUNICIPIO = MEND.MUNICIPIO
                            LEFT JOIN CARTORIO C ( NOLOCK ) ON C.COD_CARTORIO = P.ID_CARTORIO
                            LEFT JOIN HADES.dbo.NECESSIDADEESPECIAL NEC ON NEC.NECESSIDADEESPECIALID=P.NECESSIDADEESPECIALID
                            LEFT JOIN NACIONALIDADE NA ON NA.NACIONALIDADE = P.NACIONALIDADE    					
                    WHERE   A.PESSOA = @PESSOA  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = ctx.GetDataReader(contextQuery);

                if (reader.Read())
                {
                    //Preenche dados pessoais
                    dados.Pessoa = Convert.ToInt32(reader["PESSOA"]);
                    dados.AlunoMatricula = Convert.ToString(reader["ALUNO"]);
                    dados.NomeAluno = Convert.ToString(reader["NOME_COMPL"]);
                    dados.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);
                    if (reader["QT_FILHOS"] != DBNull.Value)
                    {
                        dados.QuantidadeFilhos = Convert.ToInt32(reader["QT_FILHOS"]);
                    }
                    dados.TipoSanguineo = Convert.ToString(reader["TIPO_SANGUINEO"]);
                    dados.Etnia = Convert.ToString(reader["ETNIA"]);
                    dados.EstadoCivil = Convert.ToString(reader["EST_CIVIL"]);
                    dados.PaisNascimento = Convert.ToString(reader["PAIS_NASC"]);
                    dados.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);
                    dados.UfNascimento = Convert.ToString(reader["UF_NASC"]);
                    dados.Naturalidade = Convert.ToString(reader["MUNICIPIO_NASC"]);
                    dados.Credo = Convert.ToString(reader["CREDO"]);
                    dados.DescendenciaFamiliar = Convert.ToString(reader["QUILOMBOLA_INDIGENA"]);
                    dados.NecessidadeEspecial = Convert.ToString(reader["NECESSIDADE_ESPECIAL"]);

                    //Preenche filiacao
                    dados.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dados.FalecidaMae = Convert.ToString(reader["MAE_FALECIDA"]);
                    dados.CPFMae = Convert.ToString(reader["MAE_CPF"]);
                    dados.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dados.FalecidoPai = Convert.ToString(reader["PAI_FALECIDO"]);
                    dados.CPFPai = Convert.ToString(reader["PAI_CPF"]);
                    dados.ResponsavelLegal = Convert.ToString(reader["RESPONSAVEL"]);
                    if (dados.ResponsavelLegal.ToUpper() == "OUTROS;")
                    {
                        dados.NomeOutros = Convert.ToString(reader["RESP_NOME_COMPL"]);
                        dados.CpfOutros = Convert.ToString(reader["RESP_CPF"]);
                        dados.TelResponsavel = Convert.ToString(reader["RESP_FONE"]);
                    }
                    dados.TelMae = Convert.ToString(reader["MAE_TELEFONE"]);
                    dados.TelPai = Convert.ToString(reader["PAI_TELEFONE"]);

                    //Preenche endereço
                    dados.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dados.NumeroEndereco = Convert.ToString(reader["END_NUM"]);
                    dados.ComplementoEndereco = Convert.ToString(reader["END_COMPL"]);
                    dados.BairroEndereco = Convert.ToString(reader["BAIRRO"]);
                    dados.MunicipioEndereco = Convert.ToString(reader["END_MUNICIPIO"]);
                    dados.EstadoEndereco = Convert.ToString(reader["END_UF"]);
                    dados.CepEndereco = Convert.ToString(reader["CEP"]);
                    dados.LocalizacaoEndereco = Convert.ToString(reader["LOCALIZACAO"]);

                    //Preenche contato
                    dados.Telefone = Convert.ToString(reader["FONE"]);
                    dados.Celular = Convert.ToString(reader["CELULAR"]);
                    dados.Email = Convert.ToString(reader["E_MAIL_INTERNO"]);

                    //Preenche Documentos
                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.TipoDocumento = Convert.ToString(reader["RG_TIPO"]);
                    dados.NumeroDocumento = Convert.ToString(reader["RG_NUM"]);
                    dados.ComplementoIdentidade = Convert.ToString(reader["COMPLETMENTO_RG"]);
                    dados.EstadoDocumento = Convert.ToString(reader["RG_UF"]);
                    dados.OrgaoEmissorDocumento = Convert.ToString(reader["RG_EMISSOR"]);
                    if (reader["RG_DTEXP"] != DBNull.Value)
                    {
                        dados.DataExpedicaoDocumento = Convert.ToDateTime(reader["RG_DTEXP"]);
                    }

                    //Preenche Outras Informacoes
                    dados.Inep = Convert.ToString(reader["ID_CENSO"]);
                    dados.Nis = Convert.ToString(reader["NIS"]);

                    //Preenche Certidao Civil
                    dados.TipoCertidao = Convert.ToString(reader["TIPO_CERTIDAO"]);
                    dados.CertidaoCivil = Convert.ToString(reader["CERTIDAO_CIVIL"]);
                    dados.UfCartorio = Convert.ToString(reader["UF_CARTORIO"]);
                    dados.MunicipioCartorio = Convert.ToString(reader["MUNICIPIO_CARTORIO"]);
                    dados.NomeCartorio = Convert.ToString(reader["NOME_CARTORIO"]);
                    dados.Livro = Convert.ToString(reader["CERT_NASC_LIVRO"]);
                    dados.Folha = Convert.ToString(reader["CERT_NASC_FOLHA"]);
                    dados.NumeroTermo = Convert.ToString(reader["CERT_NASC_NUM"]);
                    if (reader["CERT_NASC_EMISSAO"] != DBNull.Value)
                    {
                        dados.DataEmissaoCertidao = Convert.ToDateTime(reader["CERT_NASC_EMISSAO"]);
                    }
                    dados.MatriculaCertidao = Convert.ToString(reader["CERT_NUMERO_MATRICULA"]);
                }

                //Buscar Foto do aluno
                dados.Foto = FotoPessoa.Carregar(Convert.ToInt32(dados.Pessoa));

                //Ajusta descriçoes e formataçoes necessárias
                if (dados.Sexo == "F")
                {
                    dados.Sexo = "Feminino";
                }
                else if (dados.Sexo == "M")
                {
                    dados.Sexo = "Masculino";
                }

                if (dados.DescendenciaFamiliar == "S")
                {
                    dados.DescendenciaFamiliar = "Sim";
                }
                else if (dados.DescendenciaFamiliar == "N")
                {
                    dados.DescendenciaFamiliar = "Não";
                }

                if (dados.NomeMae != "NÃO DECLARADO")
                {
                    if (dados.FalecidaMae == "S")
                    {
                        dados.FalecidaMae = "Sim";
                    }
                    else if (dados.FalecidaMae == "N")
                    {
                        dados.FalecidaMae = "Não";
                    }
                }
                else
                {
                    dados.FalecidaMae = string.Empty;
                }

                if (dados.NomePai != "NÃO DECLARADO")
                {
                    if (dados.FalecidoPai == "S")
                    {
                        dados.FalecidoPai = "Sim";
                    }
                    else if (dados.FalecidoPai == "N")
                    {
                        dados.FalecidoPai = "Não";
                    }
                }
                else
                {
                    dados.FalecidoPai = string.Empty;
                }

                if (dados.Credo == "Naodeclarado")
                {
                    dados.Credo = "Não declarado";
                }

                if (!string.IsNullOrEmpty(dados.ResponsavelLegal))
                {
                    dados.ResponsavelLegal = dados.ResponsavelLegal.Replace(';', ' ');
                }
                dados.CPFMae = dados.CPFMae.AplicarMascaraCPF();
                dados.CPFPai = dados.CPFPai.AplicarMascaraCPF();
                dados.Cpf = dados.Cpf.AplicarMascaraCPF();

                return dados;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

    }
}