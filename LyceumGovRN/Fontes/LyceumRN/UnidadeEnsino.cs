using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class UnidadeEnsino : RNBase
    {
        public static QueryTable ConsultarGratificada(string unidadeEns)
        {
            string sql =
                @"SELECT DISTINCT l.funcao, 
                    lf.descricao, 
                    l.matricula,
                    p.nome_compl, 
                    p.fone, 
                    p.celular, 
                    p.E_MAIL_INTERNO as e_mail
                FROM LY_UNIDADE_ENSINO ue 
                INNER JOIN LY_LOTACAO l ON ue.UNIDADE_ENS = l.UNIDADE_ENS 
                INNER JOIN LY_PESSOA p ON p.PESSOA = l.PESSOA
                INNER JOIN LY_FUNCAO Lf ON l.funcao = Lf.funcao
                LEFT JOIN FUNCAO F ON F.FUNCAOID=LF.FUNCAO
                WHERE ue.UNIDADE_ENS = ? 
                AND FUNCAOBB = 'GRATIFICADA'
                AND (TIPOFUNCAOID IS NULL OR TIPOFUNCAOID NOT IN (1,2,3))
                AND l.DATA_NOMEACAO <= convert(date,GetDate()) 
                AND (l.DATA_DESATIVACAO is null OR convert(date,l.data_desativacao) > convert(date,GetDate()))
                ORDER BY DESCRICAO, NOME_COMPL";

            return Consultar(sql, unidadeEns);
        }

        /// <summary>
        /// Consulta dados da unidade de ensino.
        /// </summary>
        /// <param name="unidadeEnsino">unidade de ensino</param>
        /// <returns>linha com dados da unidade de ensino</returns>
        public static Ly_unidade_ensino.Row Consultar(String unidadeEnsino)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return Ly_unidade_ensino.QueryFirstRow(connection, "unidade_ens = ?", unidadeEnsino);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Consultar mnemonico da unidade de ensino.
        /// </summary>
        /// <param name="unidade_ens"></param>
        /// <returns></returns>
        public static QueryTable ConsultarUnidadeQT(string unidade_ens)
        {
            return Consultar("SELECT MNEMONICO FROM LY_UNIDADE_ENSINO WHERE UNIDADE_ENS = ? ORDER BY NOME_COMP", unidade_ens);
        }

        /// <summary>
        /// Consulta situação da unidade.
        /// </summary>
        /// <param name="unidadeEnsino">unidade de ensino</param>
        /// <returns>situação da unidade de ensino</returns>
        public static string ConsultarPorUnidade(string unidadeEnsino)
        {
            return ConsultarCampo("select nome_comp from VW_UNIDADE_ENSINO_SITUACAO where UNIDADE_ENS = ?", unidadeEnsino);
        }

        public DTOs.DadosDiretor ObtemDiretorPor(DataContext contexto, string censo)
        {
            return this.ObtemDiretorPor(contexto, censo, DateTime.Now.Date);
        }

        public DTOs.DadosDiretor ObtemDiretorPor(DataContext contexto, string censo, DateTime dataConsulta)
        {
            DTOs.DadosDiretor dadosDiretor = new DTOs.DadosDiretor();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT L.UNIDADE_ENS AS CENSO,
					                            P.NOME_COMPL AS NOME,
					                            P.RG_NUM AS RG,
					                            P.CPF,
					                            P.ENDERECO,
					                            P.END_NUM AS NUMERO,
					                            P.END_COMPL AS COMPLEMENTO,
					                            P.BAIRRO,
					                            M.NOME AS MUNICIPIO,
					                             P.E_MAIL_INTERNO AS EMAIL,
					                            ISNULL(P.CELULAR, P.FONE) AS TELEFONE,
					                            P.IDFUNCIONAL,
					                            L.MATRICULA,
												L.DATA_NOMEACAO_DO
                                            FROM LY_LOTACAO L (NOLOCK) 
					                            INNER JOIN LY_PESSOA P (NOLOCK) ON P.PESSOA = L.PESSOA
					                            LEFT JOIN MUNICIPIO M ON P.END_MUNICIPIO = M.CODIGO
                                            WHERE l.UNIDADE_ENS = @CENSO
					                            AND L.FUNCAO = 14
					                            AND L.DATA_NOMEACAO <= CONVERT(DATE, @DATACONSULTA) 
					                            AND (L.DATA_DESATIVACAO IS NULL OR CONVERT(DATE,L.DATA_DESATIVACAO) > CONVERT(DATE, @DATACONSULTA)) ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@DATACONSULTA", dataConsulta);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosDiretor.Censo = Convert.ToString(reader["CENSO"]);
                    dadosDiretor.Nome = Convert.ToString(reader["NOME"]);
                    dadosDiretor.Rg = Convert.ToString(reader["RG"]);
                    dadosDiretor.Cpf = Convert.ToString(reader["CPF"]);
                    dadosDiretor.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosDiretor.Numero = Convert.ToString(reader["NUMERO"]);
                    dadosDiretor.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                    dadosDiretor.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosDiretor.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosDiretor.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosDiretor.Email = Convert.ToString(reader["EMAIL"]);
                    dadosDiretor.IdFuncional = Convert.ToString(reader["IDFUNCIONAL"]);
                    dadosDiretor.Matricula = Convert.ToString(reader["MATRICULA"]);
                    dadosDiretor.DataNomeacaoDo = Convert.ToString(reader["DATA_NOMEACAO_DO"]).IsNullOrEmptyOrWhiteSpace() ? (DateTime?)null : Convert.ToDateTime(reader["DATA_NOMEACAO_DO"]);
                }

                return dadosDiretor;
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

        public PrestacaoContas.DTOs.DadosModalidadesEnsino ObtemDadosModalidadesEnsino(DataContext contexto, string censo, DateTime dataConsulta)
        {
            ContextQuery contextQuery = new ContextQuery();
            PrestacaoContas.DTOs.DadosModalidadesEnsino dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosModalidadesEnsino();
            SqlDataReader reader = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PrestacaoContas.MODALIDADEENSINO";
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATACONSULTA", SqlDbType.DateTime, dataConsulta);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.FundamentalModalidade = Convert.ToBoolean(reader["FUNDAMENTALMODALIDADE"]);
                    dados.FundamentalNumeroAlunos = Convert.ToInt32(reader["FUNDAMENTALNUMEROALUNOS"]);
                    dados.FundamentalNumeroTurnos = Convert.ToInt32(reader["FUNDAMENTALNUMEROTURNOS"]);
                    dados.FundamentalHorarioIntegral = Convert.ToBoolean(reader["FUNDAMENTALHORARIOINTEGRAL"]);

                    dados.MedioModalidade = Convert.ToBoolean(reader["MEDIOMODALIDADE"]);
                    dados.MedioNumeroAlunos = Convert.ToInt32(reader["MEDIONUMEROALUNOS"]);
                    dados.MedioNumeroTurnos = Convert.ToInt32(reader["MEDIONUMEROTURNOS"]);
                    dados.MedioHorarioIntegral = Convert.ToBoolean(reader["MEDIOHORARIOINTEGRAL"]);

                    dados.EjaModalidade = Convert.ToBoolean(reader["EJAMODALIDADE"]);
                    dados.EjaNumeroAlunos = Convert.ToInt32(reader["EJANUMEROALUNOS"]);
                    dados.EjaNumeroTurnos = Convert.ToInt32(reader["EJANUMEROTURNOS"]);
                    dados.EjaHorarioIntegral = Convert.ToBoolean(reader["EJAHORARIOINTEGRAL"]);

                    dados.EducacaoEspecialModalidade = Convert.ToBoolean(reader["EDUCACAOESPECIALMODALIDADE"]);
                    dados.EducacaoEspecialNumeroAlunos = Convert.ToInt32(reader["EDUCACAOESPECIALNUMEROALUNOS"]);
                    dados.EducacaoEspecialNumeroTurnos = Convert.ToInt32(reader["EDUCACAOESPECIALNUMEROTURNOS"]);
                    dados.EducacaoEspecialHorarioIntegral = Convert.ToBoolean(reader["EDUCACAOESPECIALHORARIOINTEGRAL"]);
                }

                return dados;
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

        public PrestacaoContas.DTOs.DadosFormulario1 ObtemDadosUnidadePor(DataContext contexto, string censo, DateTime dataConsulta)
        {
            PrestacaoContas.DTOs.DadosFormulario1 dadosFormulario1 = new PrestacaoContas.DTOs.DadosFormulario1();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 UE.NOME_COMP AS ESCOLA,
												UE.UNIDADE_ENS AS CENSO,
												UE.CGC AS CNPJ,
												UE.ENDERECO + ', ' + UE.END_NUM AS ENDERECO,
												UE.END_COMPL AS COMPLEMENTO,
												BA.DESCRICAO AS BAIRRO,
												M.NOME AS MUNICIPIO,
												UE.CEP,
												UE.FONE	AS TELEFONE,
                                                UE.E_MAIL AS EMAIL,
												R.REGIONAL,
												B.NOME AS BANCO,
											    A.NOME AS AGENCIA,
											    CC.CONTA
                                        FROM   LY_UNIDADE_ENSINO UE (NOLOCK)
                                               LEFT JOIN HADES..BAIRRO BA (NOLOCK)
													   ON UE.BAIRRO = BA.CODIGO
                                               INNER JOIN TCE_REGIONAL R (NOLOCK)
                                                       ON UE.ID_REGIONAL = R.ID_REGIONAL
                                               INNER JOIN MUNICIPIO M 
                                                       ON UE.MUNICIPIO = M.CODIGO
											   LEFT JOIN PrestacaoContas.CONTACORRENTE CC (NOLOCK)	
													   ON CC.CENSO = UE.UNIDADE_ENS
													   AND CC.DATAINICIO <= @DATACONSULTA
													   AND (CC.DATAFIM IS NULL OR CC.DATAFIM >= @DATACONSULTA)
											   LEFT JOIN HADES.DBO.BANCOS B (NOLOCK) 
					                                    ON CONVERT(INT, CC.BANCO) = CONVERT(INT, B.BANCO)
											    LEFT JOIN  HADES.DBO.AGENCIAS A (NOLOCK) 
					                                     ON CONVERT(INT, CC.AGENCIA) = CONVERT(INT, A.AGENCIA) 
                                                          AND CONVERT(INT, CC.BANCO) = CONVERT(INT, A.BANCO)
                                        WHERE  UE.UNIDADE_ENS = @CENSO 
										ORDER BY CC.DATAINICIO DESC ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATACONSULTA", SqlDbType.DateTime, dataConsulta);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosFormulario1.NomeAae = Convert.ToString(reader["ESCOLA"]);
                    dadosFormulario1.Censo = Convert.ToString(reader["CENSO"]);
                    dadosFormulario1.Cnpj = Convert.ToString(reader["CNPJ"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["CNPJ"]);
                    dadosFormulario1.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosFormulario1.Complemento = Convert.ToString(reader["COMPLEMENTO"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["COMPLEMENTO"]);
                    dadosFormulario1.Bairro = Convert.ToString(reader["BAIRRO"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["BAIRRO"]);
                    dadosFormulario1.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosFormulario1.Cep = Convert.ToString(reader["CEP"]);
                    dadosFormulario1.EmailInstituicional = Convert.ToString(reader["EMAIL"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["EMAIL"]);
                    dadosFormulario1.DiretoriaRegional = Convert.ToString(reader["REGIONAL"]);
                    dadosFormulario1.Banco = Convert.ToString(reader["BANCO"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["BANCO"]);
                    dadosFormulario1.Agencia = Convert.ToString(reader["AGENCIA"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["AGENCIA"]);
                    dadosFormulario1.ContaCorrente = Convert.ToString(reader["CONTA"]).IsNullOrEmptyOrWhiteSpace() ? " - " : Convert.ToString(reader["CONTA"]);

                    string telefone = Convert.ToString(reader["TELEFONE"]).RetirarMascaraTelefone();
                    dadosFormulario1.Ddd = Convert.ToString(reader["TELEFONE"]).IsNullOrEmptyOrWhiteSpace() ? " - " : telefone.Substring(0, 2);
                    dadosFormulario1.Telefone = Convert.ToString(reader["TELEFONE"]).IsNullOrEmptyOrWhiteSpace() ? " - " : telefone.Substring(2, telefone.Length - 2);
                }

                return dadosFormulario1;
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

        public PrestacaoContas.DTOs.DadosUnidadeAae ObtemDadosUnidadePor(DataContext contexto, string censo)
        {
            PrestacaoContas.DTOs.DadosUnidadeAae dadosUnidadeAae = new PrestacaoContas.DTOs.DadosUnidadeAae();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT UE.ID_REGIONAL		AS IDREGIONAL,
                                               R.REGIONAL,
                                               UE.ENDERECO,
                                               UE.END_NUM			AS NUMERO,
                                               UE.END_COMPL			AS COMPLEMENTO,
                                               BA.DESCRICAO         AS BAIRRO,
                                               M.NOME				AS MUNICIPIO,
                                               UE.FONE				AS TELEFONE,
                                               UE.E_MAIL			AS EMAIL,
                                               UE.UNIDADE_ENS		AS CENSO
                                        FROM   LY_UNIDADE_ENSINO UE (NOLOCK)
											   LEFT JOIN HADES..BAIRRO BA (NOLOCK)
													   ON UE.BAIRRO = BA.CODIGO
                                               INNER JOIN TCE_REGIONAL R (NOLOCK)
                                                       ON UE.ID_REGIONAL = R.ID_REGIONAL
                                               INNER JOIN MUNICIPIO M 
                                                       ON UE.MUNICIPIO = M.CODIGO
                                        WHERE  UE.UNIDADE_ENS = @CENSO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosUnidadeAae.IdRegional = Convert.ToInt32(reader["IDREGIONAL"]);
                    dadosUnidadeAae.Regional = Convert.ToString(reader["REGIONAL"]);
                    dadosUnidadeAae.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosUnidadeAae.Numero = Convert.ToString(reader["NUMERO"]);
                    dadosUnidadeAae.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                    dadosUnidadeAae.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dadosUnidadeAae.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosUnidadeAae.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosUnidadeAae.Email = Convert.ToString(reader["EMAIL"]);
                    dadosUnidadeAae.Censo = Convert.ToString(reader["CENSO"]);
                }

                return dadosUnidadeAae;
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

        public DTOs.UnidadeInformacoesGerais ObtemInformacoesGeraisPor(string censo)
        {
            DTOs.UnidadeInformacoesGerais entidade = new DTOs.UnidadeInformacoesGerais();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT UE.UNIDADE_ENS, 
                                            SIT_FUNCIONAMENTO, 
                                            UE.NOME_COMP, 
                                            UE.NOME_ABREV, 
                                            UE.ID_REGIONAL, 
                                            UE.NUCLEO, 
                                            UE.SETOR, 
                                            WS.UA_ATUAL,
                                            UE.CLASSIFICACAO, 
                                            IMOVEL_COMPARTILHADO, 
		                                    UF.CEP, 
                                            UF.MUNICIPIO, 
		                                    M.NOME AS NOMEMUNICIPIO,
		                                    M.UF_SIGLA AS UF,
                                            UF.ENDERECO, 
                                            UF.END_NUM, 
                                            UF.END_COMPL, 
                                            UF.BAIRRO, 
                                            BA.DESCRICAO AS DESCRICAOBAIRRO,
                                            DISTRITO,
                                            LATITUDE, 
                                            LONGITUDE,
                                            RE.REGIONAL,
                                            NC.DESCRICAO AS COORDENADORIA
                                    FROM   LY_UNIDADE_ENSINO UE (NOLOCK) 
		                                    INNER JOIN MUNICIPIO M 
				                                    ON UE.MUNICIPIO = M.CODIGO
                                            INNER JOIN LY_UNIDADES_ASSOCIADAS UA (NOLOCK) 
                                                    ON UE.UNIDADE_ENS = UA.UNIDADE_ENS 
                                            INNER JOIN LY_UNIDADE_FISICA UF (NOLOCK) 
                                                    ON UF.UNIDADE_FIS = UA.UNIDADE_FIS 
                                            LEFT JOIN HADES.dbo.BAIRRO BA (NOLOCK)
													ON BA.CODIGO = UF.BAIRRO
                                            LEFT JOIN LY_NUCLEO NC (NOLOCK)
													ON NC.NUCLEO = UE.NUCLEO
											LEFT JOIN TCE_REGIONAL RE (NOLOCK)
													ON RE.ID_REGIONAL = UE.ID_REGIONAL
                                            INNER JOIN HADES..VW_SETOR WS 
                                                    ON WS.SETOR = UE.SETOR
                                    WHERE  UE.UNIDADE_ENS = @UNIDADE_ENS  ";

                contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    entidade.Censo = Convert.ToString(reader["UNIDADE_ENS"]);
                    entidade.SituacaoFuncionamento = Convert.ToString(reader["SIT_FUNCIONAMENTO"]);
                    entidade.NomeUnidade = Convert.ToString(reader["NOME_COMP"]);
                    if (reader["ID_REGIONAL"] != DBNull.Value)
                    {
                        entidade.RegionalId = Convert.ToInt32(reader["ID_REGIONAL"]);
                        entidade.NomeRegional = Convert.ToString(reader["REGIONAL"]);
                    }
                    if (reader["NUCLEO"] != DBNull.Value)
                    {
                        entidade.NomeCoordenadoria = Convert.ToString(reader["COORDENADORIA"]);
                    }

                    entidade.Coordenadoria = Convert.ToString(reader["NUCLEO"]);
                    entidade.UnidadeAdministrativa = Convert.ToString(reader["UA_ATUAL"]);
                    entidade.Classificacao = Convert.ToString(reader["CLASSIFICACAO"]);
                    entidade.ImovelCompartilhado = Convert.ToString(reader["IMOVEL_COMPARTILHADO"]);
                    entidade.Cep = Convert.ToString(reader["CEP"]);
                    entidade.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    entidade.MunicipioDescricao = Convert.ToString(reader["NOMEMUNICIPIO"]);
                    entidade.UF = Convert.ToString(reader["UF"]);
                    entidade.Endereco = Convert.ToString(reader["ENDERECO"]);
                    entidade.EnderecoNumero = Convert.ToString(reader["END_NUM"]);
                    entidade.EnderecoComplemento = Convert.ToString(reader["END_COMPL"]);
                    entidade.EnderecoBairro = Convert.ToString(reader["BAIRRO"]);
                    entidade.EnderecoDescricaoBairro = Convert.ToString(reader["DESCRICAOBAIRRO"]);
                    entidade.Distrito = Convert.ToString(reader["DISTRITO"]);
                    entidade.Latitude = Convert.ToString(reader["LATITUDE"]);
                    entidade.Longitude = Convert.ToString(reader["LONGITUDE"]);
                }

                return entidade;
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

        public DataTable ObtemDadosEscolaPor(string censo)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"[GestaoRede].[SP_DADOS_ESCOLA]";
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                lista = contexto.GetDataTable(contextQuery);
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
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }

        public bool ExisteUnidadeEnsinoCadastradaPor(DataContext contexto, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   LY_UNIDADE_ENSINO (NOLOCK) 
                                      WHERE  UNIDADE_ENS = @CENSO ";

            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaInformacoesGeraisInsercao(DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            RN.UnidadeFisica rnUnidadeFisica = new RN.UnidadeFisica();
            List<string> mensagens = new List<string>();
            List<string> validacaoInformacoesGerais = new List<string>();
            DataContext contexto = null;
            string cep = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeInformacoesGerais == null)
            {
                return validacaoDados;
            }


            //Validacao dos campos da Aba Informações Gerais
            validacaoInformacoesGerais = this.ValidaInformacoesGerais(unidadeInformacoesGerais);
            if (validacaoInformacoesGerais.Count > 0)
            {
                mensagens.AddRange(validacaoInformacoesGerais);
            }

            //Validacao dos campos do Endereço
            if (unidadeInformacoesGerais.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório.");
            }
            else
            {
                cep = Utils.RetirarMascara(unidadeInformacoesGerais.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (unidadeInformacoesGerais.Municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICÍPIO é obrigatório.");
            }

            if (unidadeInformacoesGerais.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório.");
            }

            if (unidadeInformacoesGerais.EnderecoNumero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO é obrigatório.");
            }

            if (unidadeInformacoesGerais.EnderecoBairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o censo já existe
                    if (this.ExisteUnidadeEnsinoCadastradaPor(contexto, unidadeInformacoesGerais.Censo))
                    {
                        mensagens.Add("Já existe uma UNIDADE DE ENSINO cadastrada com este CÓDIGO DO CENSO.");
                    }

                    if (rnUnidadeFisica.ExisteUnidadeFisicaCadastradaPor(contexto, unidadeInformacoesGerais.Censo))
                    {
                        mensagens.Add("Já existe uma UNIDADE FÍSICA cadastrada com este CÓDIGO DO CENSO.");
                    }

                    //Verifica outra escola com mesma unidade Administrativa
                    string escola = this.ObtemOutraUnidadeEnsinoCadastradaPor(contexto, unidadeInformacoesGerais.UnidadeAdministrativa, unidadeInformacoesGerais.Censo);
                    if (!escola.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add(string.Format("A unidade {0} já está cadastrada com a UA informada. ", escola));
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

        public ValidacaoDados ValidaInformacoesGeraisAlteracao(DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            List<string> validacaoInformacoesGerais = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeInformacoesGerais == null)
            {
                return validacaoDados;
            }

            //Validacao dos campos da Aba Informações Gerais
            validacaoInformacoesGerais = this.ValidaInformacoesGerais(unidadeInformacoesGerais);
            if (validacaoInformacoesGerais.Count > 0)
            {
                mensagens.AddRange(validacaoInformacoesGerais);
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica outra escola com mesma unidade Administrativa
                    string escola = this.ObtemOutraUnidadeEnsinoCadastradaPor(contexto, unidadeInformacoesGerais.UnidadeAdministrativa, unidadeInformacoesGerais.Censo);
                    if (!escola.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add(string.Format("A unidade {0} já está cadastrada com a UA informada. ", escola));
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

        private string ObtemOutraUnidadeEnsinoCadastradaPor(DataContext contexto, string setor, string unidadeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 NOME_COMP
                                            FROM    DBO.LY_UNIDADE_ENSINO
                                            WHERE   SETOR = @SETOR
                                                    AND UNIDADE_ENS <> @UNIDADE_ENS ";

            contextQuery.Parameters.Add("@SETOR", TechneDbType.T_ALFASMALL, setor);
            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadeEnsino);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        private List<string> ValidaInformacoesGerais(DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            List<string> mensagens = new List<string>();
            decimal numeroDecimal;
            int numeroInteiro;

            if (unidadeInformacoesGerais.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CÓDIGO DO CENSO é obrigatório.");
            }
            else
            {
                if (unidadeInformacoesGerais.Censo.Length != 8)
                {
                    mensagens.Add("Campo CÓDIGO DO CENSO deve ser composto de 8 digitos.");
                }

                if (!int.TryParse(unidadeInformacoesGerais.Censo, out numeroInteiro))
                {
                    mensagens.Add("Campo CÓDIGO DO CENSO deve ser composto apenas por números.");
                }
            }

            if (unidadeInformacoesGerais.SituacaoFuncionamento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SITUAÇÃO DE FUNCIONAMENTO é obrigatório.");
            }

            if (unidadeInformacoesGerais.NomeUnidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA UNIDADE é obrigatório.");
            }

            if (unidadeInformacoesGerais.RegionalId <= 0)
            {
                mensagens.Add("Campo DIRETORIA REGIONAL é obrigatório.");
            }

            if (unidadeInformacoesGerais.Coordenadoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo COORDENADORIA é obrigatório.");
            }

            if (unidadeInformacoesGerais.UnidadeAdministrativa.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo U.A. é obrigatório.");
            }

            if (unidadeInformacoesGerais.ImovelCompartilhado.IsNullOrEmptyOrWhiteSpace()
                || (unidadeInformacoesGerais.ImovelCompartilhado != "N" && unidadeInformacoesGerais.ImovelCompartilhado != "S"))
            {
                mensagens.Add("Campo IMÓVEL COMPARTILHADO é obrigatório com os Valores N ou S.");
            }

            if (unidadeInformacoesGerais.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (unidadeInformacoesGerais.Latitude.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo LATITUDE é obrigatório.");
            }
            else
            {
                if (!decimal.TryParse(unidadeInformacoesGerais.Latitude.Replace(".", ","), out numeroDecimal))
                {
                    mensagens.Add("LATITUDE inválida.");
                }
            }

            if (unidadeInformacoesGerais.Longitude.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo LONGITUDE é obrigatório.");
            }
            else
            {
                if (!decimal.TryParse(unidadeInformacoesGerais.Longitude.Replace(".", ","), out numeroDecimal))
                {
                    mensagens.Add("LONGITUDE inválida.");
                }
            }

            return mensagens;
        }

        public void Insere(DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais, bool usuarioPrivilegiado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.UnidadeFisica rnUnidadeFisica = new UnidadeFisica();
            RN.Entidades.LyUnidadesAssociadas unidadesAssociadas = new LyUnidadesAssociadas();
            RN.UnidadesAssociadas rnUnidadesAssociadas = new UnidadesAssociadas();
            RN.UsuarioUnidadeFis rnUsuarioUnidadeFis = new UsuarioUnidadeFis();
            RN.GestaoRede.RegionalHistoricoUnidade rnRegionalHistoricoUnidade = new Techne.Lyceum.RN.GestaoRede.RegionalHistoricoUnidade();
            RN.GestaoRede.Entidades.RegionalHistoricoUnidade regionalHistoricoUnidade = new Techne.Lyceum.RN.GestaoRede.Entidades.RegionalHistoricoUnidade();

            try
            {
                //Insere unidade Fisica
                rnUnidadeFisica.InsereInformacoesGerais(contexto, unidadeInformacoesGerais);

                //Insere unidade Ensino
                this.InsereInformacoesGerais(contexto, unidadeInformacoesGerais);

                //Monta Unidades Associadas
                unidadesAssociadas.UnidadeEns = unidadeInformacoesGerais.Censo;
                unidadesAssociadas.UnidadeFis = unidadeInformacoesGerais.Censo;

                //Insere Unidades Associadas
                rnUnidadesAssociadas.Insere(contexto, unidadesAssociadas);

                //Monta Historico Regional
                regionalHistoricoUnidade.Censo = unidadeInformacoesGerais.Censo;
                regionalHistoricoUnidade.RegionalId = unidadeInformacoesGerais.RegionalId;
                regionalHistoricoUnidade.DataInicio = DateTime.Now.Date;
                regionalHistoricoUnidade.DataFim = null;
                regionalHistoricoUnidade.UsuarioId = unidadeInformacoesGerais.UsuarioResponsavel;

                //Insere Historico Regional
                rnRegionalHistoricoUnidade.Insere(contexto, regionalHistoricoUnidade);

                if (!usuarioPrivilegiado)
                {
                    //Insere acesso para o usuario
                    rnUsuarioUnidadeFis.Insere(contexto, unidadesAssociadas.UnidadeFis, unidadeInformacoesGerais.UsuarioResponsavel);

                    //Insere acesso para usuários COOPLA
                    contexto.ApplyModifications(new ContextQuery(
                        @"INSERT INTO LYCEUM..[LY_USUARIO_UNIDADE_FIS] ([USUARIO], [UNIDADE_FIS])
                          SELECT DISTINCT U.USUARIO, @UNIDADE_ENS
                          FROM HADES..HD_PADUSUARIO PU
                          INNER JOIN HADES..HD_USUARIO U ON PU.USUARIO = U.USUARIO
                          WHERE PADACES = @PADACES
                          AND NOT EXISTS (
                          SELECT * FROM LYCEUM..[LY_USUARIO_UNIDADE_FIS] UU
                          WHERE UU.USUARIO = U.USUARIO 
                          AND UU.UNIDADE_FIS = @UNIDADE_ENS)",
                        new ContextQueryParameter("@UNIDADE_ENS", unidadesAssociadas.UnidadeFis),
                        new ContextQueryParameter("@PADACES", "COOPLA")
                    ));
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

        private void InsereInformacoesGerais(DataContext contexto, DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_UNIDADE_ENSINO 
                                    (UNIDADE_ENS, 
                                     SIT_FUNCIONAMENTO, 
                                     NOME_COMP, 
                                     NOME_ABREV, 
                                     ID_REGIONAL, 
                                     NUCLEO, 
                                     SETOR, 
                                     CLASSIFICACAO, 
                                     CEP, 
                                     MUNICIPIO, 
                                     ENDERECO, 
                                     END_NUM, 
                                     END_COMPL, 
                                     BAIRRO, 
                                     MATRICULA, 
                                     DT_CADASTRO) 
                        VALUES      (@UNIDADE_ENS, 
                                     @SIT_FUNCIONAMENTO, 
                                     @NOME_COMP, 
                                     @NOME_ABREV, 
                                     @ID_REGIONAL, 
                                     @NUCLEO, 
                                     @SETOR, 
                                     @CLASSIFICACAO, 
                                     @CEP, 
                                     @MUNICIPIO, 
                                     @ENDERECO, 
                                     @END_NUM, 
                                     @END_COMPL, 
                                     @BAIRRO, 
                                     @MATRICULA, 
                                     @DT_CADASTRO) ";

            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Censo);
            contextQuery.Parameters.Add("@SIT_FUNCIONAMENTO", SqlDbType.VarChar, unidadeInformacoesGerais.SituacaoFuncionamento);
            contextQuery.Parameters.Add("@NOME_COMP", TechneDbType.T_ALFALARGE, unidadeInformacoesGerais.NomeUnidade);
            contextQuery.Parameters.Add("@NOME_ABREV", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.NomeUnidade);
            contextQuery.Parameters.Add("@ID_REGIONAL", SqlDbType.Int, unidadeInformacoesGerais.RegionalId);
            contextQuery.Parameters.Add("@NUCLEO", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Coordenadoria);
            contextQuery.Parameters.Add("@SETOR", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.UnidadeAdministrativa);
            contextQuery.Parameters.Add("@CLASSIFICACAO", SqlDbType.VarChar, unidadeInformacoesGerais.Classificacao);
            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, unidadeInformacoesGerais.Cep);
            contextQuery.Parameters.Add("@MUNICIPIO", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Municipio);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.Endereco);
            contextQuery.Parameters.Add("@END_NUM", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.EnderecoNumero);
            contextQuery.Parameters.Add("@END_COMPL", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.EnderecoComplemento);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.EnderecoBairro);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeInformacoesGerais.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DT_CADASTRO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaInformacoesGerais(DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.UnidadeFisica rnUnidadeFisica = new UnidadeFisica();
            RN.Entidades.LyUnidadesAssociadas unidadesAssociadas = new LyUnidadesAssociadas();
            RN.UnidadesAssociadas rnUnidadesAssociadas = new UnidadesAssociadas();
            RN.GestaoRede.RegionalHistoricoUnidade rnRegionalHistoricoUnidade = new Techne.Lyceum.RN.GestaoRede.RegionalHistoricoUnidade();
            RN.GestaoRede.Entidades.RegionalHistoricoUnidade regionalHistoricoUnidade = new Techne.Lyceum.RN.GestaoRede.Entidades.RegionalHistoricoUnidade();
            RN.GestaoRede.Entidades.RegionalHistoricoUnidade regionalBase;

            try
            {
                //Atualiza ImovelCompartilhado e latidude e longitude
                rnUnidadeFisica.AtualizaInformacoesGerais(contexto, unidadeInformacoesGerais);

                //Atualiza unidade Ensino
                this.AtualizaInformacoesGerais(contexto, unidadeInformacoesGerais);

                //Busca qual regional esta ativa para a unidade no historico
                regionalBase = rnRegionalHistoricoUnidade.ObtemRegionalAtivaPor(contexto, unidadeInformacoesGerais.Censo);

                //Atualiza vínculo para todos ao mudar para Pré-Ativa
                if (unidadeInformacoesGerais.SituacaoFuncionamento == "PreAtiva")
                {
                    contexto.ApplyModifications(new ContextQuery(
                        @"INSERT INTO LYCEUM..[LY_USUARIO_UNIDADE_FIS] ([USUARIO], [UNIDADE_FIS])
                          SELECT DISTINCT U.USUARIO, @UNIDADE_ENS
                          FROM HADES..HD_PADUSUARIO PU
                          INNER JOIN HADES..HD_USUARIO U ON PU.USUARIO = U.USUARIO
						  INNER JOIN [APOLLO]..SEEDUC_PADACES SP ON PU.PADACES = SP.PADACES
                          AND NOT EXISTS (
                              SELECT * FROM LYCEUM..[LY_USUARIO_UNIDADE_FIS] UU
                              WHERE UU.USUARIO = U.USUARIO 
                              AND UU.UNIDADE_FIS = @UNIDADE_ENS)",
                        new ContextQueryParameter("@UNIDADE_ENS", unidadeInformacoesGerais.Censo)
                    ));
                }

                if (regionalBase.RegionalId != unidadeInformacoesGerais.RegionalId)
                {
                    //Finaliza Historico atual
                    rnRegionalHistoricoUnidade.Finaliza(contexto, regionalBase.RegionalHistoricoUnidadeId, unidadeInformacoesGerais.UsuarioResponsavel, DateTime.Now.Date.AddDays(-1));

                    //Monta Novo Historico Regional
                    regionalHistoricoUnidade.Censo = unidadeInformacoesGerais.Censo;
                    regionalHistoricoUnidade.RegionalId = unidadeInformacoesGerais.RegionalId;
                    regionalHistoricoUnidade.DataInicio = DateTime.Now.Date;
                    regionalHistoricoUnidade.DataFim = null;
                    regionalHistoricoUnidade.UsuarioId = unidadeInformacoesGerais.UsuarioResponsavel;

                    //Insere Historico Regional
                    rnRegionalHistoricoUnidade.Insere(contexto, regionalHistoricoUnidade);
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

        private void AtualizaInformacoesGerais(DataContext contexto, DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.LY_UNIDADE_ENSINO 
                                SET    SIT_FUNCIONAMENTO = @SIT_FUNCIONAMENTO, 
                                       NOME_COMP = @NOME_COMP, 
                                       NOME_ABREV = @NOME_ABREV, 
                                       ID_REGIONAL = @ID_REGIONAL, 
                                       NUCLEO = @NUCLEO, 
                                       SETOR = @SETOR, 
                                       CLASSIFICACAO = @CLASSIFICACAO, 
                                       MATRICULA = @MATRICULA 
                                WHERE  UNIDADE_ENS = @UNIDADE_ENS ";

            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Censo);
            contextQuery.Parameters.Add("@SIT_FUNCIONAMENTO", SqlDbType.VarChar, unidadeInformacoesGerais.SituacaoFuncionamento);
            contextQuery.Parameters.Add("@NOME_COMP", TechneDbType.T_ALFALARGE, unidadeInformacoesGerais.NomeUnidade);
            contextQuery.Parameters.Add("@NOME_ABREV", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.NomeUnidade);
            contextQuery.Parameters.Add("@ID_REGIONAL", SqlDbType.Int, unidadeInformacoesGerais.RegionalId);
            contextQuery.Parameters.Add("@NUCLEO", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Coordenadoria);
            contextQuery.Parameters.Add("@SETOR", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.UnidadeAdministrativa);
            contextQuery.Parameters.Add("@CLASSIFICACAO", SqlDbType.VarChar, unidadeInformacoesGerais.Classificacao);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeInformacoesGerais.UsuarioResponsavel);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaCaracteristicasFisicas(DataContext contexto, DTOs.UnidadeCaracteristicasFisicas unidadeCaracteristicasFisicas)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.LY_UNIDADE_ENSINO
                                    SET   CEP = @CEP, 
                                           MUNICIPIO = @MUNICIPIO, 
                                           ENDERECO = @ENDERECO, 
                                           END_NUM = @END_NUM, 
                                           END_COMPL = @END_COMPL, 
                                           BAIRRO = @BAIRRO, 
	                                       DEPENDENCIA_ADM = @DEPENDENCIA_ADM,
	                                       E_MAIL = @E_MAIL,
	                                       CGC = @CGC,
	                                       EXTRACLASSE = @EXTRACLASSE,
	                                       FONE = @FONE,
	                                       TEL2 = @TEL2,
	                                       FAX = @FAX,
	                                       MATRICULA = @MATRICULA
                                    WHERE UNIDADE_ENS = @UNIDADE_ENS ";

            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadeCaracteristicasFisicas.UnidadeFisica);
            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, unidadeCaracteristicasFisicas.Cep);
            contextQuery.Parameters.Add("@MUNICIPIO", TechneDbType.T_CODIGO, unidadeCaracteristicasFisicas.Municipio);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.Endereco);
            contextQuery.Parameters.Add("@END_NUM", TechneDbType.T_ALFASMALL, unidadeCaracteristicasFisicas.EnderecoNumero);
            contextQuery.Parameters.Add("@END_COMPL", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.EnderecoComplemento);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.EnderecoBairro);
            contextQuery.Parameters.Add("@DEPENDENCIA_ADM", TechneDbType.T_ALFALARGE, unidadeCaracteristicasFisicas.DependenciaAdministrativa);
            contextQuery.Parameters.Add("@E_MAIL", SqlDbType.VarChar, unidadeCaracteristicasFisicas.Email);
            contextQuery.Parameters.Add("@CGC", TechneDbType.T_CGC, unidadeCaracteristicasFisicas.Cnpj);
            contextQuery.Parameters.Add("@EXTRACLASSE", SqlDbType.VarChar, unidadeCaracteristicasFisicas.Extraclasse);
            contextQuery.Parameters.Add("@FONE", TechneDbType.T_TELEFONE, unidadeCaracteristicasFisicas.Telefone1);
            contextQuery.Parameters.Add("@TEL2", SqlDbType.VarChar, unidadeCaracteristicasFisicas.Telefone2);
            contextQuery.Parameters.Add("@FAX", TechneDbType.T_TELEFONE, unidadeCaracteristicasFisicas.Fax);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeCaracteristicasFisicas.UsuarioResponsavel);

            contexto.ApplyModifications(contextQuery);
        }

        public static LyUnidadeEnsino Carregar(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT  *
                            FROM    dbo.LY_UNIDADE_ENSINO
                            WHERE   UNIDADE_ENS = @UNIDADE_ENS ");
                contextQuery.Parameters.Add("@UNIDADE_ENS", censo);

                return ctx.TryToBindEntity<LyUnidadeEnsino>(contextQuery);
            }
        }

        public static void AlterarSituacaoFuncionamento(DataContext ctx, string censo, string atoOficial)
        {
            var situacao = string.Empty;

            if (atoOficial == UnidadeEnsinoSituacao.Paralizacao)
            {
                situacao = "Paralisada";
            }
            else if (atoOficial == UnidadeEnsinoSituacao.Extincao)
            {
                situacao = "Extinta";
            }
            else if (atoOficial == UnidadeEnsinoSituacao.EmProcesso)
            {
                situacao = "EmProcesso";
            }
            else
            {
                situacao = "EmAtividade";
            }

            ctx.ApplyModifications(
                new ContextQuery(
                    @"UPDATE  dbo.LY_UNIDADE_ENSINO
                    SET     SIT_FUNCIONAMENTO = @SIT_FUNCIONAMENTO
                    WHERE   UNIDADE_ENS = @CENSO",
                    new ContextQueryParameter("@SIT_FUNCIONAMENTO", situacao),
                    new ContextQueryParameter("@CENSO", censo)));
        }

        public static DataTable Buscar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return null;
            }

            return Consultar(
                new ContextQuery(
                    string.Format(
                        @"SELECT TOP 5 UNIDADE_ENS AS 'censo',
                                UNIDADE_ENS + ' - ' + NOME_COMP AS 'nome'
                        FROM    ly_unidade_ensino
                        WHERE   UNIDADE_ENS + ' - ' + NOME_COMP LIKE '%{0}%'",
                        texto.Trim().Replace("'", "''"))));
        }

        public static string RetornaNomeUnidadeEnsino(string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT TOP 1 NOME_COMP
                            FROM    DBO.LY_UNIDADE_ENSINO
                            WHERE   UNIDADE_ENS = @UNIDADE_ENSINO ");

                contextQuery.Parameters.Add("@UNIDADE_ENSINO", censo);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public string RetornaMunicipioPor(DataContext contexto, string unidadeEnsino)
        {

            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT  MUNICIPIO
                        FROM    DBO.LY_UNIDADE_ENSINO 
                        WHERE   UNIDADE_ENS = @UNIDADE_ENS ";

            contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public static string RetornaMunicipio(string unidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  MUNICIPIO
                        FROM    DBO.LY_UNIDADE_ENSINO 
                        WHERE   UNIDADE_ENS = @UNIDADE_ENS ");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static string RetornaNomeMunicipio(string unidadeEnsino)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"  SELECT  m.NOME
                        FROM    DBO.LY_UNIDADE_ENSINO u
								inner join MUNICIPIO m on u.MUNICIPIO = m.CODIGO
                        WHERE   UNIDADE_ENS = @UNIDADE_ENS ");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static DataTable RetornaUnidadeAbsorvida(string pUnidadeOrigem)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery( //U.UNIDADE_ENS, UNIDADE_ENS + ' - ' + U.NOME_COMP AS NOME_COMP
                    @" 
                        SELECT DISTINCT UE.unidade_ens, UE.unidade_ens + ' - ' + UE.NOME_COMP AS NOME_COMP
                        FROM   ly_unidade_ensino UE (nolock)
                        WHERE  UE.unidade_ens = @UNIDADE_ENS 
                               AND NOT EXISTS (SELECT 1 
                                               FROM   serieabsorvida S (nolock)
                                               WHERE  S.unidadeensinoorigemid = UE.unidade_ens 
                                                      AND S.nivelabsorcaoid = 1) --Unidade completamente absorvida 
                        UNION 
                        SELECT DISTINCT UE.unidade_ens, UE.unidade_ens + ' - ' + UE.NOME_COMP AS NOME_COMP 
                        FROM   ly_unidade_ensino UE (nolock)
                               INNER JOIN serieabsorvida S (nolock)
                                       ON ( S.unidadeensinodestinoid = UE.unidade_ens ) 
                        WHERE  S.unidadeensinoorigemid = @UNIDADE_ENS 
                               --AND S.nivelabsorcaoid <> 1 Outros níveis de absorção ");

                contextQuery.Parameters.Add("@UNIDADE_ENS", pUnidadeOrigem);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable RetornaUnidadeEnsinoAlunosDestinoDasCompartilhadas()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @"
                                        SELECT distinct (U.unidade_ens) as unidade_ens, 
                                               U.nome_comp, --Dados Unidade Estadual de Destino    
                                               U.unidade_ens + ' / ' + U.nome_comp AS unidade_ensino_destino
                                               --C.censo_compartilhada, 
                                               --Código da Unidade compartilhada (origem municipal)   
                                               --C.nome                              AS NOMECOMPARTILHADA, 
                                               --Nome da Unidade compartilhada (origem)   
                                               --C.rede_ensino                       AS REDEENSINOCOMPARTILHADA 
                                        --Rede da Unidade compartilhada (origem)   
                                        FROM   vw_unidade_ensino_situacao U 
                                               INNER JOIN tce_compartilhada C 
                                                       ON ( U.unidade_ens = C.censo ) ";

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static DataTable RetornaUnidadeEnsinoAlunosOrigemDasCompartilhadas(string unidadeDestino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = String.Format(@"
                                        SELECT distinct (C.censo_compartilhada) as unidade_ens, 
	                                           C.censo_compartilhada + ' / ' + C.nome AS unidade_ensino_origem,
                                               --Código da Unidade compartilhada (origem municipal)   
                                               C.nome 
                                               --Nome da Unidade compartilhada (origem)   
                                               --C.rede_ensino                          AS REDEENSINOCOMPARTILHADA 
                                        --Rede da Unidade compartilhada (origem)   
                                        FROM   vw_unidade_ensino_situacao U 
                                               INNER JOIN tce_compartilhada C 
                                                       ON ( U.unidade_ens = C.censo ) 
                                        WHERE C.CENSO = '{0}'", unidadeDestino);

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        /// <summary>
        /// Retorna a rede de ensino por unidade de ensino de origem
        /// </summary>
        /// <param name="unidadeOrigem">unidade</param>
        /// <returns>rede ensino</returns>
        public static string RetornaRedeEnsinoPor(string unidadeOrigem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dtRedeEnsingo = null;
            string redeEnsino = string.Empty;

            try
            {
                contextQuery.Command = string.Format(@"
                                    SELECT TOP 1 C.rede_ensino AS REDEENSINOCOMPARTILHADA 
                                    FROM   vw_unidade_ensino_situacao U 
                                           INNER JOIN tce_compartilhada C 
                                                   ON ( U.unidade_ens = C.censo ) 
                                    WHERE  C.censo_compartilhada = '{0}'", unidadeOrigem);

                dtRedeEnsingo = ctx.GetDataTable(contextQuery);

                if (dtRedeEnsingo.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRedeEnsingo.Rows)
                    {
                        redeEnsino = row["REDEENSINOCOMPARTILHADA"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return redeEnsino;
        }

        public List<string> ObtemListaUnidadesEnsinoPor(FiltroRestricaoTerminalidade filtro)
        {
            List<string> listaUnidadesEnsino = new List<string>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string unidadeEnsino = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT  UNIDADE_ENS
                                            FROM    VW_UNIDADE_ENSINO_SITUACAO
                                            WHERE  SITUACAO = 'ESTADUAL' 
                                                    AND UNIDADE_ENS = ISNULL(@UNIDADE_ENS, UNIDADE_ENS)
                                                    AND ID_REGIONAL = ISNULL(@ID_REGIONAL, ID_REGIONAL)
                                                    AND MUNICIPIO = ISNULL(@MUNICIPIO, MUNICIPIO) ";

                contextQuery.Parameters.Add("@UNIDADE_ENS", filtro.PorUnidadeEnsino ? filtro.UnidadeEnsino : null);
                contextQuery.Parameters.Add("@ID_REGIONAL", filtro.PorRegional ? filtro.Regional : null);
                contextQuery.Parameters.Add("@MUNICIPIO", filtro.PorMunicipio ? filtro.Municipio : null);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    unidadeEnsino = Convert.ToString(reader["UNIDADE_ENS"]);
                    listaUnidadesEnsino.Add(unidadeEnsino);
                }

                return listaUnidadesEnsino;
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

        public DataRow ObtemPorUnidadeEns(int unidadeEns)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"
                                    SELECT lyue.unidade_ens, lyue.nome_comp, lyue.setor, lyue.cgc, lyue.situacao, lyue.nucleo, lyue.municipio, lyue.id_regional, r.REGIONAL, m.NOME as NOMEMUNICIPIO
                                    from VW_UNIDADE_ENSINO_SITUACAO lyue
                                    inner join TCE_REGIONAL r on lyue.ID_REGIONAL = r.ID_REGIONAL
                                    inner join MUNICIPIO m on lyue.MUNICIPIO = m.CODIGO
                                    WHERE unidade_ens = @UNIDADE_ENS
                                    ";

                contextQuery.Parameters.Add("@UNIDADE_ENS", SqlDbType.Int, unidadeEns);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        private string ObtemOutraUnidadeEnsinoCadastradaPor(DataContext contexto, string cep, string endereco, string numero, string unidadeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 NOME_COMP
                                            FROM    DBO.LY_UNIDADE_ENSINO
                                            WHERE   CEP = @CEP
                                                    AND ENDERECO = @ENDERECO
                                                    AND END_NUM = @END_NUM 
                                                    AND UNIDADE_ENS <> @UNIDADE_ENS  ";

            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, cep);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, endereco);
            contextQuery.Parameters.Add("@END_NUM", TechneDbType.T_ALFASMALL, numero);
            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadeEnsino);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public DTOs.UnidadeDadosPedagogicos ObtemDadosPedagogicosPor(string censo)
        {
            DTOs.UnidadeDadosPedagogicos entidade = new DTOs.UnidadeDadosPedagogicos();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.GestaoRede.UnidadeFisicaOrgaoColegiado rnUnidadeFisicaOrgaoColegiado = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaOrgaoColegiado();
            RN.GestaoRede.UnidadeFisicaMaterialPedagogico rnUnidadeFisicaMaterialPedagogico = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaMaterialPedagogico();

            try
            {
                //Busca caracteristicas fisicas
                entidade = this.ObtemDadosPedagogicosPor(contexto, censo);

                //Busca materiais pedagogicos
                entidade.MaterialPedagogico = rnUnidadeFisicaMaterialPedagogico.ListaMaterialPedagogicoPor(contexto, censo);

                //Busca orgãos colegioados                
                entidade.OrgaoColegiado = rnUnidadeFisicaOrgaoColegiado.ListaOrgaoColegiadoPor(contexto, censo);

                return entidade;
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

        private DTOs.UnidadeDadosPedagogicos ObtemDadosPedagogicosPor(DataContext contexto, string censo)
        {
            DTOs.UnidadeDadosPedagogicos entidade = new DTOs.UnidadeDadosPedagogicos();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT ESPACOEQUIPAMENTOENTORNO, 
                                                 ESPACOESCOLACOMUNIDADE,
		                                            POSSUIPAGINAWEB, 
		                                            PAGINAWEB, 
		                                            POSSUIPROJETOPEDAGOGICO, 
		                                            CUMPRIUPROJETOPEDAGOGICO, 
		                                            UE.UNIDADE_ENS, 
		                                            UF.UNIDADE_FIS,
                                            Educacaoambiental,
                                            ConteudoComponentes,
                                            Componentecurricular,
                                            EixoEstuturante,
                                            EmEventos,
                                            ProjetosTransversais,
                                            NOL 
                                            FROM   LY_UNIDADE_ENSINO UE (NOLOCK) 
                                                    INNER JOIN LY_UNIDADES_ASSOCIADAS UA (NOLOCK) 
                                                            ON UE.UNIDADE_ENS = UA.UNIDADE_ENS 
                                                    INNER JOIN LY_UNIDADE_FISICA UF (NOLOCK) 
                                                            ON UF.UNIDADE_FIS = UA.UNIDADE_FIS 
                                            WHERE  UE.UNIDADE_ENS = @UNIDADE_ENS ";

                contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    entidade.Censo = Convert.ToString(reader["UNIDADE_ENS"]);
                    entidade.EspacoEquipamentoEntorno = Convert.ToString(reader["ESPACOEQUIPAMENTOENTORNO"]);
                    entidade.PossuiPaginaWeb = Convert.ToString(reader["POSSUIPAGINAWEB"]);
                    entidade.PaginaWeb = Convert.ToString(reader["PAGINAWEB"]);
                    entidade.PossuiProjetoPedagogico = Convert.ToString(reader["POSSUIPROJETOPEDAGOGICO"]);
                    entidade.CumpriuProjetoPedagogico = Convert.ToString(reader["CUMPRIUPROJETOPEDAGOGICO"]);
                    entidade.EspacoEscolaComunidade = Convert.ToString(reader["ESPACOESCOLACOMUNIDADE"]);
                    entidade.Educacaoambiental = Convert.ToString(reader["Educacaoambiental"]);
                    entidade.ConteudoComponentes = Convert.ToString(reader["ConteudoComponentes"]);
                    entidade.Componentecurricular = Convert.ToString(reader["Componentecurricular"]);
                    entidade.EixoEstuturante = Convert.ToString(reader["EixoEstuturante"]);
                    entidade.EmEventos = Convert.ToString(reader["EmEventos"]);
                    entidade.ProjetosTransversais = Convert.ToString(reader["ProjetosTransversais"]);
                    entidade.NOL = Convert.ToString(reader["NOL"]);
                }

                return entidade;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados ValidaDadosPedagogicos(DTOs.UnidadeDadosPedagogicos unidadeDadosPedagogicos)
        {
            List<string> mensagens = new List<string>();
            string cep = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeDadosPedagogicos == null)
            {
                return validacaoDados;
            }

            if (unidadeDadosPedagogicos.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (unidadeDadosPedagogicos.MaterialPedagogico == null || unidadeDadosPedagogicos.MaterialPedagogico.Count == 0)
            {
                mensagens.Add("Campo INSTRUMENTOS, MATERIAIS SOCIOCULTURAIS E/OU PEDAGÓGICOS EM USO NA ESCOLA PARA O DESENVOLVIMENTO DE ATIVIDADES DE ENSINO APRENDIZAGEM é obrigatório.");
            }
            else
            {
                //1	Nenhuma das opções
                if (unidadeDadosPedagogicos.MaterialPedagogico.Contains(1) && unidadeDadosPedagogicos.MaterialPedagogico.Count > 1)
                {
                    mensagens.Add("Quando a opção NENHUMA DAS OPÇÕES estiver marcada não é possível acrescentar outra opção");
                }
            }

            if (unidadeDadosPedagogicos.OrgaoColegiado == null || unidadeDadosPedagogicos.OrgaoColegiado.Count == 0)
            {
                mensagens.Add("Campo ÓRGÃOS COLEGIADOS EM FUNCIONAMENTO NA ESCOLA é obrigatório.");
            }
            else
            {
                //6	- Não há órgãos colegiados em funcionamento
                if (unidadeDadosPedagogicos.OrgaoColegiado.Contains(6) && unidadeDadosPedagogicos.OrgaoColegiado.Count > 1)
                {
                    mensagens.Add("Quando a opção NÃO HÁ ÓRGÃOS COLEGIADOS EM FUNCIONAMENTO estiver marcada não é possível acrescentar outra opção");
                }
            }

            if (unidadeDadosPedagogicos.PossuiPaginaWeb.IsNullOrEmptyOrWhiteSpace()
             || (unidadeDadosPedagogicos.EspacoEquipamentoEntorno != "N" && unidadeDadosPedagogicos.EspacoEquipamentoEntorno != "S"))
            {
                mensagens.Add("Campo POSSUI SITE OU BLOG OU PÁGINA EM REDES SOCIAIS é obrigatório.");
            }
            else
            {
                if (unidadeDadosPedagogicos.PossuiPaginaWeb == "S")
                {
                    if (unidadeDadosPedagogicos.PaginaWeb.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo O LINK DO SITE OU BLOG OU PÁGINA EM REDES SOCIAIS é obrigatório, quando marcada como possui.");
                    }
                }
                else if (!unidadeDadosPedagogicos.PaginaWeb.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Não é possivél informar um link sem marcar a opção POSSUI SITE OU BLOG OU PÁGINA EM REDES SOCIAIS.");
                }
            }

            if (unidadeDadosPedagogicos.EspacoEquipamentoEntorno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USA ESPAÇOS E EQUIPAMENTOS DO ENTORNO ESCOLAR é obrigatório.");
            }

            if (unidadeDadosPedagogicos.PossuiProjetoPedagogico.IsNullOrEmptyOrWhiteSpace()
             || (unidadeDadosPedagogicos.PossuiProjetoPedagogico != "N" && unidadeDadosPedagogicos.PossuiProjetoPedagogico != "S"))
            {
                mensagens.Add("Campo POSSUI PROJETO POLÍTICO PEDAGÓGICO OU A PROPOSTA PEDAGÓGICA é obrigatório.");
            }
            else
            {
                if (unidadeDadosPedagogicos.PossuiProjetoPedagogico == "S")
                {
                    if (unidadeDadosPedagogicos.CumpriuProjetoPedagogico.IsNullOrEmptyOrWhiteSpace()
                        || (unidadeDadosPedagogicos.CumpriuProjetoPedagogico != "N" && unidadeDadosPedagogicos.CumpriuProjetoPedagogico != "S"))
                    {
                        mensagens.Add("Campo PROJETO POLÍTICO PEDAGÓGICO OU A PROPOSTA PEDAGÓGICA FOI ATUALZADO é obrigatório.");
                    }
                }
                else
                {
                    unidadeDadosPedagogicos.CumpriuProjetoPedagogico = "N";
                }
            }

            if (unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo COMPARTILHA ESPAÇOS PARA ATIVIDADES DE INTEGRAÇÃO ESCOLA-COMUNIDADE é obrigatório.");
            }

            if (unidadeDadosPedagogicos.EspacoEscolaComunidade.IsNullOrEmptyOrWhiteSpace()
              || (unidadeDadosPedagogicos.EspacoEscolaComunidade != "N" && unidadeDadosPedagogicos.EspacoEscolaComunidade != "S"))
            {
                mensagens.Add("Campo COMPARTILHA ESPAÇOS PARA ATIVIDADES DE INTEGRAÇÃO ESCOLA-COMUNIDADE é obrigatório.");
            }


            if (unidadeDadosPedagogicos.Educacaoambiental.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo A ESCOLA DESENVOLVE AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL é obrigatório.");
            }
            var aux = 1;
            if (unidadeDadosPedagogicos.Educacaoambiental == "S")
            {
                if (unidadeDadosPedagogicos.ConteudoComponentes == "S" ||
                  unidadeDadosPedagogicos.Componentecurricular == "S" ||
                  unidadeDadosPedagogicos.EixoEstuturante == "S" ||
                  unidadeDadosPedagogicos.EmEventos == "S" ||
                  unidadeDadosPedagogicos.ProjetosTransversais == "S" ||
                  unidadeDadosPedagogicos.NOL == "S"
                  )
                    aux = 0;
                else
                {
                    mensagens.Add("Para o caso da opção A ESCOLA DESENVOLVE AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL, esteja marcada com SIM, pelo menos uma das opções das AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL deve ser SIM");
                }
            }

            if (unidadeDadosPedagogicos.Educacaoambiental == "S")
                if (unidadeDadosPedagogicos.ConteudoComponentes.IsNullOrEmptyOrWhiteSpace() ||
                    unidadeDadosPedagogicos.EixoEstuturante.IsNullOrEmptyOrWhiteSpace() ||
                    unidadeDadosPedagogicos.Componentecurricular.IsNullOrEmptyOrWhiteSpace() ||
                    unidadeDadosPedagogicos.EmEventos.IsNullOrEmptyOrWhiteSpace() ||
                    unidadeDadosPedagogicos.ProjetosTransversais.IsNullOrEmptyOrWhiteSpace() ||
                    unidadeDadosPedagogicos.NOL.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("Para o caso da opção A ESCOLA DESENVOLVE AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL, esteja marcada com SIM, as demais opções do quadro AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL,devem estar preenchidas");


            if ((unidadeDadosPedagogicos.ConteudoComponentes == "S" ||
                  unidadeDadosPedagogicos.Componentecurricular == "S" ||
                  unidadeDadosPedagogicos.EixoEstuturante == "S" ||
                  unidadeDadosPedagogicos.EmEventos == "S" ||
                  unidadeDadosPedagogicos.ProjetosTransversais == "S" ||
                  unidadeDadosPedagogicos.NOL == "S") && unidadeDadosPedagogicos.Educacaoambiental == "N"
                  )
                mensagens.Add("Para o caso da opção A ESCOLA DESENVOLVE AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL, esteja marcada com NÂO, todas as opções da AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL deve ser NÃO");


            if (unidadeDadosPedagogicos.NOL == "S" && (
                unidadeDadosPedagogicos.ConteudoComponentes == "S" ||
                unidadeDadosPedagogicos.Componentecurricular == "S" ||
                  unidadeDadosPedagogicos.EixoEstuturante == "S" ||
                  unidadeDadosPedagogicos.EmEventos == "S" ||
                  unidadeDadosPedagogicos.ProjetosTransversais == "S"))
                mensagens.Add("Para o caso da opção NENHUMA DAS OPÇÕES ACIMA, esteja marcada com SIM, nenhuma das opções de AÇÕES NA ÁREA DE EDUCAÇÃO AMBIENTAL, exceto a primeira, deve ser SIM");

            if (unidadeDadosPedagogicos.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
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

        public void SalvaDadosPedagogicos(DTOs.UnidadeDadosPedagogicos unidadeDadosPedagogicos)
        {
            RN.UnidadeFisica rnUnidadeFisica = new UnidadeFisica();
            RN.GestaoRede.UnidadeFisicaOrgaoColegiado rnUnidadeFisicaOrgaoColegiado = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaOrgaoColegiado();
            RN.GestaoRede.UnidadeFisicaMaterialPedagogico rnUnidadeFisicaMaterialPedagogico = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaMaterialPedagogico();

            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza unidade Fisica
                AtualizaDadosPedagogicos(contexto, unidadeDadosPedagogicos);

                //Atualiza dados da unidade Ensino
                rnUnidadeFisica.AtualizaDadosPedagogicos(contexto, unidadeDadosPedagogicos);

                //Remove todas os orgão colegiados
                rnUnidadeFisicaOrgaoColegiado.RemovePorUnidade(contexto, unidadeDadosPedagogicos.Censo);

                //Remove todos os materiais pedagogicos
                rnUnidadeFisicaMaterialPedagogico.RemovePorUnidade(contexto, unidadeDadosPedagogicos.Censo);

                if (unidadeDadosPedagogicos.OrgaoColegiado != null && unidadeDadosPedagogicos.OrgaoColegiado.Count > 0)
                {
                    foreach (int idOrgaoColegiado in unidadeDadosPedagogicos.OrgaoColegiado)
                    {
                        //Inserir orgao colegiado
                        rnUnidadeFisicaOrgaoColegiado.Insere(contexto, unidadeDadosPedagogicos.Censo, idOrgaoColegiado, unidadeDadosPedagogicos.UsuarioResponsavel);
                    }
                }

                if (unidadeDadosPedagogicos.MaterialPedagogico != null && unidadeDadosPedagogicos.MaterialPedagogico.Count > 0)
                {
                    foreach (int idMaterialPedagogico in unidadeDadosPedagogicos.MaterialPedagogico)
                    {
                        //Inserir orgao colegiado
                        rnUnidadeFisicaMaterialPedagogico.Insere(contexto, unidadeDadosPedagogicos.Censo, idMaterialPedagogico, unidadeDadosPedagogicos.UsuarioResponsavel);
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

        private void AtualizaDadosPedagogicos(DataContext contexto, DTOs.UnidadeDadosPedagogicos unidadeDadosPedagogicos)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.LY_UNIDADE_ENSINO
                                SET    POSSUIPAGINAWEB = @POSSUIPAGINAWEB, 
                                       PAGINAWEB = @PAGINAWEB, 
                                       POSSUIPROJETOPEDAGOGICO = @POSSUIPROJETOPEDAGOGICO,
                                       CUMPRIUPROJETOPEDAGOGICO = @CUMPRIUPROJETOPEDAGOGICO,
                                       MATRICULA = @MATRICULA 
                                WHERE  UNIDADE_ENS = @UNIDADE_ENS ";

            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadeDadosPedagogicos.Censo);
            contextQuery.Parameters.Add("@POSSUIPAGINAWEB", SqlDbType.VarChar, unidadeDadosPedagogicos.PossuiPaginaWeb);
            contextQuery.Parameters.Add("@PAGINAWEB", SqlDbType.VarChar, unidadeDadosPedagogicos.PaginaWeb);
            contextQuery.Parameters.Add("@POSSUIPROJETOPEDAGOGICO", SqlDbType.VarChar, unidadeDadosPedagogicos.PossuiProjetoPedagogico);
            contextQuery.Parameters.Add("@CUMPRIUPROJETOPEDAGOGICO", SqlDbType.VarChar, unidadeDadosPedagogicos.CumpriuProjetoPedagogico);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeDadosPedagogicos.UsuarioResponsavel);

            contexto.ApplyModifications(contextQuery);
        }

        public List<LyUnidadeEnsino> Lista(int? ID_REGIONAL, string MUNICIPIO, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            List<LyUnidadeEnsino> lista = new List<LyUnidadeEnsino>();

            try
            {
                contextQuery.Command = @"
                select DISTINCT
                lyue.UNIDADE_ENS as UnidadeEns,
                lyue.NOME_COMP as NomeComp,
                lyue.NOME_ABREV as NomeAbrev,
                lyue.ENDERECO,
                lyue.END_NUM as EndNum,
                lyue.END_COMPL as EndCompl,
                lyue.BAIRRO,
                lyue.MUNICIPIO,
                lyue.CEP,
                lyue.CAIXA_POSTAL as CaixaPostal,
                lyue.FONE,
                lyue.FAX,
                lyue.CGC,
                lyue.E_MAIL as EMail,
                lyue.TURMAPREF,
                lyue.CCM,
                lyue.MNEMONICO,
                lyue.OUTRA_FACULDADE as OutraFaculdade,
                lyue.BANCO,
                lyue.AGENCIA,
                lyue.CONTA_BANCO as ContaBanco,
                lyue.TITULAR,
                lyue.WEB_SITE as WebSite,
                lyue.INEP_FACULDADE as InepFaculdade,
                lyue.INSCR_ESTADUAL as InscrEstadual,
                lyue.MARCA,
                lyue.GRUPO,
                lyue.STAMP_ATUALIZACAO as StampAtualizacao,
                lyue.NUCLEO,
                lyue.SETOR,
                lyue.TIPO,
                lyue.DEPENDENCIA_ADM as DependenciaAdm,
                lyue.CLASSIFICACAO,
                lyue.EXTRACLASSE,
                lyue.ESCOLA_ABERTA as EscolaAberta,
                lyue.SIT_FUNCIONAMENTO as SitFuncionamento,
                lyue.ID_REGIONAL as IdRegional,
                lyue.TEL2,
                lyue.MATRICULA,
                lyue.DT_CADASTRO as DtCadastro

               FROM	LYCEUM.DBO.LY_UNIDADE_ENSINO LYUE (NOLOCK)
                          JOIN	LYCEUM.DBO.TCE_REGIONAL TR (NOLOCK) ON TR.ID_REGIONAL = LYUE.ID_REGIONAL
                          JOIN	LYCEUM.DBO.LY_USUARIO_UNIDADE_FIS LUUF (NOLOCK) ON LUUF.UNIDADE_FIS = LYUE.UNIDADE_ENS
                          JOIN	LYCEUM.DBO.LY_UNIDADE_ENSINO_SITUACAO LUES ON LUES.UNIDADE_ENS = LYUE.UNIDADE_ENS
                          AND LUES.DT_SITUACAO = (
                          SELECT	MAX(DT_SITUACAO)
                          FROM	LYCEUM.DBO.LY_UNIDADE_ENSINO_SITUACAO L1
                          WHERE	L1.UNIDADE_ENS = LYUE.UNIDADE_ENS)
                          AND		(SIT_FUNCIONAMENTO = 'EmAtividade' OR 
                                     SIT_FUNCIONAMENTO = 'EmProcesso' OR 
                                     SIT_FUNCIONAMENTO = 'PreAtiva' OR 
                                     SIT_FUNCIONAMENTO IS NULL
                          )
                where(
                LUUF.USUARIO = ISNULL(@usuario,LUUF.USUARIO) OR LUUF.USUARIO IS NULL) 
                AND (lyue.ID_REGIONAL = @ID_REGIONAL or @ID_REGIONAL is null)
                and (lyue.MUNICIPIO = @MUNICIPIO or @MUNICIPIO is null)
                ";

                contextQuery.Parameters.Add("@ID_REGIONAL", ID_REGIONAL);
                contextQuery.Parameters.Add("@MUNICIPIO", MUNICIPIO);
                contextQuery.Parameters.Add("@usuario", usuario);

                lista = contexto
                    .GetDataTable(contextQuery)
                    .ToList<LyUnidadeEnsino>() ?? new List<LyUnidadeEnsino>();
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

            return lista;
        }

        public List<DadosMunicipio> ListaMunicipios(int? ID_REGIONAL, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            List<DadosMunicipio> lista = new List<DadosMunicipio>();

            try
            {
                contextQuery.Command = @"
                            SELECT DISTINCT M.CODIGO,
                                            M.NOME
                            FROM	LYCEUM.DBO.LY_UNIDADE_ENSINO LUE (NOLOCK)
                              JOIN	LYCEUM.DBO.MUNICIPIO M (NOLOCK) ON M.CODIGO = LUE.MUNICIPIO
                              JOIN	LYCEUM.DBO.TCE_REGIONAL TR (NOLOCK) ON LUE.ID_REGIONAL = TR.ID_REGIONAL
                              JOIN	LYCEUM.DBO.LY_USUARIO_UNIDADE_FIS LUUF (NOLOCK) ON LUUF.UNIDADE_FIS = LUE.UNIDADE_ENS
                            WHERE (LUUF.USUARIO = ISNULL(@usuario,LUUF.USUARIO) OR LUUF.USUARIO IS NULL) 
                                AND TR.ID_REGIONAL IS NOT NULL AND ( TR.ID_REGIONAL = @ID_REGIONAL
                                    OR @ID_REGIONAL IS NULL )  
                ";

                contextQuery.Parameters.Add("@ID_REGIONAL", ID_REGIONAL);
                contextQuery.Parameters.Add("@usuario", usuario);

                lista = contexto
                    .GetDataTable(contextQuery)
                    .ToList<DadosMunicipio>() ?? new List<DadosMunicipio>();
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

            return lista;
        }

        public List<DadosRegional> ListaRegionais(string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            List<DadosRegional> lista = new List<DadosRegional>();

            try
            {
                contextQuery.Command = @" 
                    SELECT DISTINCT TR.ID_REGIONAL,
                                    REGIONAL
                      FROM	LYCEUM.DBO.TCE_REGIONAL TR (NOLOCK)
                      JOIN	LYCEUM.DBO.LY_UNIDADE_ENSINO LUE (NOLOCK) ON LUE.ID_REGIONAL = TR.ID_REGIONAL
                      JOIN	LYCEUM.DBO.LY_USUARIO_UNIDADE_FIS LUUF (NOLOCK) ON LUUF.UNIDADE_FIS = LUE.UNIDADE_ENS
                      WHERE	(LUUF.USUARIO = ISNULL(@usuario,LUUF.USUARIO) OR LUUF.USUARIO IS NULL)
                    ORDER  BY TR.ID_REGIONAL  
                ";

                contextQuery.Parameters.Add("@usuario", usuario);

                lista = contexto
                    .GetDataTable(contextQuery)
                    .ToList<DadosRegional>() ?? new List<DadosRegional>();
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

            return lista;
        }

        public IList<Entidades.LyUnidadeEnsino> RetornaApenasAsUnidadesExistentes(IList<string> unidadesDeEnsino)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return RetornaApenasAsUnidadesExistentes(contexto, unidadesDeEnsino);
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

        public IList<Entidades.LyUnidadeEnsino> RetornaApenasAsUnidadesExistentes(DataContext contexto, IList<string> unidadesDeEnsino)
        {
            ContextQuery contextQuery = new ContextQuery();

            var stringUnidadesDeEnsino = unidadesDeEnsino
                    .Select(s => "'" + s.Trim() + "'")
                    .Aggregate((c, n) => c + "," + n);

            contextQuery.Command = @" SELECT * FROM LY_UNIDADE_ENSINO UE (NOLOCK) WHERE UE.UNIDADE_ENS in (" + stringUnidadesDeEnsino + ") ";

            return contexto.TryToBindEntities<Entidades.LyUnidadeEnsino>(contextQuery).ToList();
        }

        public string ObtemRegionalPor(string setor)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string regional = string.Empty;

            try
            {
                contextQuery.Command = @"  select DISTINCT  R.REGIONAL
                                             from HADES..HD_SETOR S
                                             INNER JOIN LY_UNIDADE_ENSINO UE ON UE.SETOR=S.SETOR                                            
                                             INNER JOIN TCE_REGIONAL R ON R.ID_REGIONAL = UE.ID_REGIONAL
                                             WHERE S.SETOR = @SETOR ";

                contextQuery.Parameters.Add("@SETOR", setor);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["REGIONAL"] != DBNull.Value)
                    {
                        regional = Convert.ToString(reader["REGIONAL"]);
                    }
                }

                return regional;
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