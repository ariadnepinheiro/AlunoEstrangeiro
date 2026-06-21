using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using Techne.Library;
using Techne.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class DocenteCandidato : RNBase
    {
        public enum EnumSituacao
        {
            [StringValue("Inscrito")]
            Inscrito = 1,
            [StringValue("Convocado")]
            Convocado = 2,
            [StringValue("Aprovado")]
            Aprovado = 3,
            [StringValue("Faltoso")]
            Faltoso = 4,
            [StringValue("Desistente")]
            Desistente = 5,
            [StringValue("Desclassificado")]
            Desclassificado = 6,
            [StringValue("Em processo")]
            EmProcesso = 7,
            [StringValue("Migração Concluída")]
            MigracaoConcluida = 8
        }

        public DataTable ObtemPor(string docenteCandidatoId, string concurso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT IDVINCULO,
	                    D.NUM_FUNC as NUMFUNC,
	                    D.PESSOA,
                        D.MATRICULA,
	                    DC.CONCURSO,
	                    DC.DOCENTECANDIDATOID,
                        D.CATEGORIA,
                        DC.DOCENTECANDIDATOID as Candidato,
						REPLICATE('0',6 - LEN(DC.DOCENTECANDIDATOID)) + CONVERT(VARCHAR(6), DC.DOCENTECANDIDATOID) AS NUMEROINSCRICAO, 
	                    DC.DISCIPLINAINGRESSO AS DISCIPLINAINGRESSO,
	                    GH.DESCRICAO AS DISCIPLINAINGRESSODESCRICAO,
						DC.ID_REGIONAL AS REGIONALID,  
                        CASE
                            WHEN DC.SEDE IS NOT NULL THEN 'SEDE'
							ELSE N.REGIONAL
						END REGIONALDESCRICAO,
                        DC.MUNICIPIO AS MUNICIPIOCODIGO,
                        CASE
                            WHEN DC.SEDE IS NOT NULL THEN 'RIO DE JANEIRO'
							WHEN DC.SEDE IS NULL THEN MUE.NOME
							ELSE NULL
						END MUNICIPIODESCRICAO,						
						DC.SEDE AS SEDE, 
	                    F.NOME_COMPL AS NOME,
	                    F.DT_NASC AS Dt_nasc,
	                    F.EST_CIVIL AS Estado_civil,
	                    F.NOME_MAE AS Nome_mae,
                        F.NOME_PAI AS Nome_pai,
	                    F.MUNICIPIO_NASC AS Municipio_nasc,
	                    MNASC.NOME AS NATURALIDADEDESCRICAO,
	                    MNASC.UF_SIGLA AS uf_sigla,
	                    FL.FL_FIELD_13 AS NUMERODEPENDENTES,
	                    F.RG_NUM AS Rg_num,
	                    F.RG_EMISSOR AS Rg_emissor,
	                    F.RG_DTEXP AS Rg_dtexp,
	                    F.RG_UF AS Rg_uf,
                        F.CPF AS Cpf,
	                    F.PISPASEP as Pis_pasep, 
	                    F.TELEITOR_NUM AS TITULOELEITOR, 
                        F.TELEITOR_SECAO AS TITULOELEITORZONA, 
                        F.TELEITOR_ZONA AS TITULOELEITORSECAO, 
	                    FL.FL_FIELD_18 AS TITULOELEITORUF,
	                    FL.FL_FIELD_14 AS CNH,
	                    FL.FL_FIELD_15 AS CNHCATEGORIA,
	                    FL.FL_FIELD_16 AS CNHVALIDADE,
	                    FL.FL_FIELD_17 AS CNHUF,
	                    F.CPROF_NUM AS Cprof_num, 
                        F.CPROF_SERIE AS Cprof_serie,
                        F.CPROF_DTEXP as Cprof_dtexp,
                        F.CPROF_UF AS Cprof_uf,
	                    F.CR_NUM AS CERTIFICADORESERVISTA,
	                    F.CR_SERIE AS CERTIFICADORESERVISTASERIE, 
	                    FL.FL_FIELD_19 AS CERTIFICADORESERVISTAUF,
	                    F.ENDERECO, 
	                    F.END_NUM AS End_num, 
	                    F.END_COMPL AS End_compl, 
	                    F.BAIRRO AS Bairro, 
                        M.UF_SIGLA AS ENDERECOUF, 
	                    F.END_MUNICIPIO AS End_municipio,
	                    F.CEP, 
	                    REPLACE(F.FONE, ' ', '') AS Fone,
	                    REPLACE(F.CELULAR, ' ', '') AS Celular,
	                    F.E_MAIL AS E_mail,
                        F.E_MAIL_INTERNO AS EMAILINSTITUCIONAL,
	                    DC.ACUMULACAO,
						CASE	
							WHEN DC.FUNCAODIRETOR = 1 THEN 'SIM'
							WHEN DC.FUNCAODIRETOR = 0 THEN 'NÃO'
							ELSE ''
						END FUNCAODIRETORDESCRICAO,
	                    DC.QTDEANOSGLP AS QUANTIDADEANOSGLP,
	                    DE.EXPERIENCIA AS EXPERIENCIA,
	                    DT.TITULACAO AS TITULACAO,
                        DC.UTILIZARUBRICA,
	                    DC.USUARIOID,
	                    DC.DATACADASTRO,
	                    DC.DATAALTERACAO,
						f.SEXO,
                        F.NECESSIDADEESPECIALID,
                        f.PAIS_NASC,
                        f.NACIONALIDADE,
						f.RG_TIPO,
                        f.ETNIA as ETNIAID,
                        DC.SITUACAO,
                        DC.DATACONVOCACAO,
                        Convert(varchar(10),DC.DATAAPRESENTACAO,103) AS DTAPRESENTACAO,
                        Convert(varchar(10),DC.DATAAPRESENTACAO,108)  AS HORAAPRESENTACAO,
						DC.FUNCAODIRETOR,
                        DC.DATAMIGRACAOPUBLICACAO
                    from RecursosHumanos.DOCENTECANDIDATO DC
						iNNER JOIN LY_DOCENTE D (NOLOCK)  on dc.NUM_FUNC = d.NUM_FUNC
						INNER JOIN VW_FUNCIONARIOS F ON F.MATRICULA = D.MATRICULA
						LEFT JOIN DBO.LY_GRUPO_HABILITACAO GH WITH ( NOLOCK ) 
                                ON DC.DISCIPLINAINGRESSO = GH.AGRUPAMENTO
						LEFT JOIN LY_FL_PESSOA FL (NOLOCK) ON D.PESSOA = FL.PESSOA
						LEFT JOIN MUNICIPIO MNASC ON F.MUNICIPIO_NASC = MNASC.CODIGO
	                    LEFT JOIN MUNICIPIO M WITH ( NOLOCK ) ON M.CODIGO = CONVERT(INT, F.END_MUNICIPIO) 
						LEFT JOIN MUNICIPIO MUE WITH ( NOLOCK ) ON MUE.CODIGO = DC.MUNICIPIO   
						LEFT JOIN TCE_REGIONAL N ON N.ID_REGIONAL = DC.ID_REGIONAL
	                    LEFT JOIN RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA DE ON DC.DOCENTECANDIDATOID = DE.DOCENTECANDIDATOID
						LEFT JOIN RecursosHumanos.DOCENTECANDIDATOTITULACAO DT ON DC.DOCENTECANDIDATOID = DT.DOCENTECANDIDATOID
                    WHERE DC.DOCENTECANDIDATOID = @DOCENTECANDIDATOID 
                        AND DC.CONCURSO = @CONCURSO ";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);

                // reader = contexto.GetDataReader(contextQuery);

                dt = ctx.GetDataTable(contextQuery);

                /*       while (reader.Read())
                       {
                           //dados de identificação
                           dadosDocenteCandidato.IdVinculo = Convert.ToString(reader["IDVINCULO"]);
                           dadosDocenteCandidato.NumFunc = Convert.ToInt32(reader["NUMFUNC"]);
                           dadosDocenteCandidato.Pessoa = Convert.ToInt32(reader["PESSOA"]);
                           dadosDocenteCandidato.Concurso = Convert.ToString(reader["CONCURSO"]);
                           dadosDocenteCandidato.DocenteCandidatoId = Convert.ToInt32(reader["DOCENTECANDIDATOID"]);
                           dadosDocenteCandidato.NumeroInscricao = Convert.ToString(reader["NUMEROINSCRICAO"]);

                           //regional de origem
                           dadosDocenteCandidato.RegionalId = reader["REGIONALID"] != DBNull.Value ? Convert.ToInt32(reader["REGIONALID"]) : (int?)null;
                           dadosDocenteCandidato.RegionalDescricao = Convert.ToString(reader["REGIONALDESCRICAO"]);
                           dadosDocenteCandidato.MunicipioCodigo = Convert.ToString(reader["MUNICIPIOCODIGO"]);
                           dadosDocenteCandidato.MunicipioDescricao = Convert.ToString(reader["MUNICIPIODESCRICAO"]);
                           dadosDocenteCandidato.Sede = Convert.ToString(reader["SEDE"]);
                           dadosDocenteCandidato.DisciplinaIngresso = Convert.ToString(reader["DISCIPLINAINGRESSO"]);
                           dadosDocenteCandidato.DisciplinaIngressoDescricao = Convert.ToString(reader["DISCIPLINAINGRESSODESCRICAO"]);

                           //dados do professor
                           dadosDocenteCandidato.Nome = Convert.ToString(reader["NOME"]);
                           dadosDocenteCandidato.DataNascimento = Convert.ToDateTime(reader["DATANASCIMENTO"]);
                           dadosDocenteCandidato.EstadoCivil = Convert.ToString(reader["ESTADOCIVIL"]);
                           dadosDocenteCandidato.NomeMae = Convert.ToString(reader["NOMEMAE"]);
                           dadosDocenteCandidato.NomePai = Convert.ToString(reader["NOMEPAI"]);
                           dadosDocenteCandidato.NaturalidadeId = Convert.ToString(reader["NATURALIDADEID"]);
                           dadosDocenteCandidato.NaturalidadeDescricao = Convert.ToString(reader["NATURALIDADEDESCRICAO"]);
                           dadosDocenteCandidato.NaturalidadeUf = Convert.ToString(reader["NATURALIDADEUF"]);
                           dadosDocenteCandidato.NumeroDependentes = Convert.ToInt32(reader["NUMERODEPENDENTES"]);

                           //documentos pessoais
                           dadosDocenteCandidato.Rg = Convert.ToString(reader["RG"]);
                           dadosDocenteCandidato.RgOrgao = Convert.ToString(reader["RGORGAO"]);
                           dadosDocenteCandidato.RgDataExpedicao = Convert.ToDateTime(reader["RGDATAEXPEDICAO"]);
                           dadosDocenteCandidato.RgUf = Convert.ToString(reader["RGUF"]);
                           dadosDocenteCandidato.Cpf = Convert.ToString(reader["CPF"]);
                           dadosDocenteCandidato.PisPasep = Convert.ToString(reader["PISPASEP"]);
                           dadosDocenteCandidato.TituloEleitor = Convert.ToString(reader["TITULOELEITOR"]);
                           dadosDocenteCandidato.TituloEleitorZona = Convert.ToString(reader["TITULOELEITORZONA"]);
                           dadosDocenteCandidato.TituloEleitorSecao = Convert.ToString(reader["TITULOELEITORSECAO"]);
                           dadosDocenteCandidato.TituloEleitorUf = Convert.ToString(reader["TITULOELEITORUF"]);
                           dadosDocenteCandidato.Cnh = Convert.ToString(reader["CNH"]);
                           dadosDocenteCandidato.CnhCategoria = Convert.ToString(reader["CNHCATEGORIA"]);
                           dadosDocenteCandidato.CnhValidade = reader["CNHVALIDADE"] != DBNull.Value ? Convert.ToDateTime(reader["CNHVALIDADE"]) : (DateTime?)null;
                           dadosDocenteCandidato.CnhUf = Convert.ToString(reader["CNHUF"]);
                           dadosDocenteCandidato.CarteiraTrabalho = Convert.ToString(reader["CARTEIRATRABALHO"]);
                           dadosDocenteCandidato.CarteiraTrabalhoSerie = Convert.ToString(reader["CARTEIRATRABALHOSERIE"]);
                           dadosDocenteCandidato.CarteiraTrabalhoUf = Convert.ToString(reader["CARTEIRATRABALHOUF"]);
                           dadosDocenteCandidato.CertificadoReservista = Convert.ToString(reader["CERTIFICADORESERVISTA"]);
                           dadosDocenteCandidato.CertificadoReservistaSerie = Convert.ToString(reader["CERTIFICADORESERVISTASERIE"]);
                           dadosDocenteCandidato.CertificadoReservistaUf = Convert.ToString(reader["CERTIFICADORESERVISTAUF"]);

                           //endereço
                           dadosDocenteCandidato.Cep = Convert.ToString(reader["CEP"]);
                           dadosDocenteCandidato.Endereco = Convert.ToString(reader["ENDERECO"]);
                           dadosDocenteCandidato.EnderecoNumero = Convert.ToString(reader["ENDERECONUMERO"]);
                           dadosDocenteCandidato.EnderecoComplemento = Convert.ToString(reader["ENDERECOCOMPLEMENTO"]);
                           dadosDocenteCandidato.EnderecoBairro = Convert.ToString(reader["ENDERECOBAIRRO"]);
                           dadosDocenteCandidato.EnderecoMunicipio = Convert.ToString(reader["ENDERECOMUNICIPIO"]);
                           dadosDocenteCandidato.EnderecoUf = Convert.ToString(reader["ENDERECOUF"]);

                           //contato
                           dadosDocenteCandidato.Telefone = Convert.ToString(reader["TELEFONE"]);
                           dadosDocenteCandidato.Celular = Convert.ToString(reader["CELULAR"]);
                           dadosDocenteCandidato.EmailPessoal = Convert.ToString(reader["EMAILPESSOAL"]);
                           dadosDocenteCandidato.EmailInstitucional = Convert.ToString(reader["EMAILINSTITUCIONAL"]);

                           //outros
                           dadosDocenteCandidato.Acumulacao = Convert.ToBoolean(reader["ACUMULACAO"]);
                           dadosDocenteCandidato.QuantidadeAnosGlp = Convert.ToInt32(reader["QUANTIDADEANOSGLP"]);
                           dadosDocenteCandidato.Experiencia = Convert.ToString(reader["EXPERIENCIA"]);
                           dadosDocenteCandidato.Titulacao = Convert.ToString(reader["TITULACAO"]);
                           dadosDocenteCandidato.UtilizaRubrica = Convert.ToBoolean(reader["UTILIZARUBRICA"]);
                           dadosDocenteCandidato.UsuarioId = Convert.ToString(reader["USUARIOID"]);                    
                       }

                       reader.Close();*/

                return dt;
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

        public void LimparConvocacao(string docenteCandidatoId, string concurso, string situacao, string observacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  
                UPDATE RecursosHumanos.DOCENTECANDIDATO
                SET 
                    SITUACAO = @SITUACAO,
                    DATAAPRESENTACAO = NULL,
                    USUARIOCONVOCACAOID = NULL,
                    DATACONVOCACAO = NULL,
                    DATACONVOCACAOPUBLICACAO = NULL,
                    DATAANALISE = NULL,
                    USUARIOANALISEID = NULL,
                    OBSERVACAO = @OBSERVACAO
                WHERE 
                    DOCENTECANDIDATOID = @DOCENTECANDIDATOID
                    AND CONCURSO = @CONCURSO
                ";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOID", docenteCandidatoId);
                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@SITUACAO", situacao);
                contextQuery.Parameters.Add("@OBSERVACAO", observacao);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                throw new Exception(ex.Message);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public DTO.DadosInscricaoMigracao ObtemDadosInscricaoMigracaoPor(string idVinculo, string concurso, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            DTO.DadosInscricaoMigracao dadosInscricaoMigracao = new DTO.DadosInscricaoMigracao();
            RN.RecursosHumanos.DocenteCandidatoGLP rnDocenteCandidatoGLP = new DocenteCandidatoGLP();

            try
            {
                dadosInscricaoMigracao = ObtemDadosInscricaoMigracaoPor(contexto, idVinculo, concurso, ano);

                if (dadosInscricaoMigracao.DocenteCandidatoId != null)
                {
                    dadosInscricaoMigracao.AnosGLP = rnDocenteCandidatoGLP.ListaAnosGLPPor(contexto, dadosInscricaoMigracao.DocenteCandidatoId.Value);
                }

                dadosInscricaoMigracao.ParticipouMigracaoAnterior = PossuiMigracaoAnteriorEfetivada(contexto, dadosInscricaoMigracao.Pessoa, ano);

                return dadosInscricaoMigracao;
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


        public DTO.DadosInscricaoMigracao ObtemDadosInscricaoMigracaoPor( DataContext contexto, string idVinculo, string concurso, int ano)
        {
           
            ContextQuery contextQuery = new ContextQuery();
            DTO.DadosInscricaoMigracao dadosInscricaoMigracao = new DTO.DadosInscricaoMigracao();
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @"  SELECT DISTINCT IDVINCULO,
	                    D.NUM_FUNC as NUMFUNC,
	                    D.PESSOA,
	                    @CONCURSO as CONCURSO,
	                    DC.DOCENTECANDIDATOID,
                        DC.SITUACAO,
	                    CASE
								WHEN DC.SITUACAO = 1 THEN 'Inscrito'
								WHEN DC.SITUACAO = 2 THEN 'Convocado'
								WHEN DC.SITUACAO = 3 THEN 'Aprovado'
								WHEN DC.SITUACAO = 4 THEN 'Faltoso'
								WHEN DC.SITUACAO = 5 THEN 'Desistente'
								WHEN DC.SITUACAO = 6 THEN 'Desclassificado'
                                WHEN DC.SITUACAO = 7 THEN 'Em processo'
								WHEN DC.SITUACAO = 8 THEN 'Migração Concluída'
                                ELSE ''
							END DESCRICAOSITUACAO,
                        CASE
                            WHEN DC.DOCENTECANDIDATOID IS NOT NULL THEN REPLICATE('0',6 - LEN(DC.DOCENTECANDIDATOID)) + CONVERT(VARCHAR(6), DC.DOCENTECANDIDATOID)
                            ELSE ''
						END NUMEROINSCRICAO, 
						GH.AGRUPAMENTO AS DISCIPLINAINGRESSO,
	                    GH.DESCRICAO AS DISCIPLINAINGRESSODESCRICAO,
						N.ID_REGIONAL AS REGIONALID, 
                        CASE
                            WHEN SEDE.DESCRICAO IS NOT NULL THEN 'SEDE'
							ELSE N.REGIONAL
						END REGIONALDESCRICAO,
                        CASE   
							WHEN DC.MUNICIPIO IS NOT NULL THEN DC.MUNICIPIO                        
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 1 THEN '00006950'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 2 THEN '00006881'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 3 THEN '00006993'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 4 THEN '00006989'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 5 THEN '00007043'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 6 THEN '00007043'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 7 THEN '00007043'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 8 THEN '00007043'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 9 THEN '00007067'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 10 THEN '00007012'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 11 THEN '00006912'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 12 THEN '00007069'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 13 THEN '00006994'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 14 THEN '00007113'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 15 THEN '00007121'
							ELSE UE.MUNICIPIO
						END AS MUNICIPIOCODIGO,				
						CASE
                            WHEN SEDE.DESCRICAO IS NOT NULL THEN 'RIO DE JANEIRO'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NOT NULL THEN MUE.NOME
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 1 THEN 'ITAPERUNA'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 2 THEN 'CAMPOS DOS GOYTACAZES'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 3 THEN 'NOVA FRIBURGO'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 4 THEN 'NITERÓI'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 5 THEN 'RIO DE JANEIRO'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 6 THEN 'RIO DE JANEIRO'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 7 THEN 'RIO DE JANEIRO'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 8 THEN 'RIO DE JANEIRO'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 9 THEN 'SÃO GONÇALO'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 10 THEN 'PETRÓPOLIS'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 11 THEN 'DUQUE DE CAXIAS'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 12 THEN 'SÃO JOÃO DE MERITI'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 13 THEN 'NOVA IGUAÇU'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 14 THEN 'VASSOURAS'
							WHEN SEDE.DESCRICAO IS NULL AND UE.MUNICIPIO IS NULL AND N.ID_REGIONAL = 15 THEN 'VOLTA REDONDA'
							ELSE NULL
						END MUNICIPIODESCRICAO,
						SEDE.DESCRICAO AS SEDE,
	                    ISNULL(DC.NOME,F.NOME_COMPL) AS NOME,
	                    ISNULL(DC.DATANASCIMENTO, F.DT_NASC) AS DATANASCIMENTO,
	                    ISNULL(DC.ESTADOCIVIL, F.EST_CIVIL) AS ESTADOCIVIL,
	                    ISNULL(DC.NOMEMAE,F.NOME_MAE) AS NOMEMAE,
                        ISNULL(DC.NOMEPAI,F.NOME_PAI) AS NOMEPAI,
	                    ISNULL(DC.NATURALIDADEID,F.MUNICIPIO_NASC) AS NATURALIDADEID,
	                    MNASC.NOME AS NATURALIDADEDESCRICAO,
	                    MNASC.UF_SIGLA AS NATURALIDADEUF,
	                    ISNULL(DC.NUMERODEPENDENTES, FL.FL_FIELD_13) AS NUMERODEPENDENTES,
	                    ISNULL(DC.RG, F.RG_NUM) AS RG,
	                    UPPER(ISNULL(DC.RGORGAO, F.RG_EMISSOR)) AS RGORGAO,
	                    ISNULL(DC.RGDATAEXPEDICAO, F.RG_DTEXP) AS RGDATAEXPEDICAO,
	                    ISNULL(DC.RGUF, F.RG_UF) AS RGUF,
                        F.CPF AS CPF,
	                    ISNULL(DC.PISPASEP, F.PISPASEP) AS PISPASEP, 
	                    ISNULL(dc.TITULOELEITOR,F.TELEITOR_NUM) AS TITULOELEITOR,
                        ISNULL(dc.TITULOELEITORZONA,F.TELEITOR_ZONA) AS TITULOELEITORZONA,                         
                        ISNULL(DC.TITULOELEITORSECAO, F.TELEITOR_SECAO)  AS TITULOELEITORSECAO, 
	                    ISNULL(DC.TITULOELEITORUF, FL.FL_FIELD_18) AS TITULOELEITORUF,
	                    ISNULL(DC.CNH, FL.FL_FIELD_14) AS CNH,
	                    ISNULL(DC.CNHCATEGORIA, FL.FL_FIELD_15) AS CNHCATEGORIA,
	                    ISNULL(DC.CNHVALIDADE, FL.FL_FIELD_16) AS CNHVALIDADE,
	                    ISNULL(DC.CNHUF, FL.FL_FIELD_17) AS CNHUF,
	                    ISNULL(DC.CARTEIRATRABALHO, F.CPROF_NUM) AS CARTEIRATRABALHO, 
                        ISNULL(DC.CARTEIRATRABALHOSERIE, F.CPROF_SERIE) AS CARTEIRATRABALHOSERIE,
                        ISNULL(DC.CARTEIRATRABALHOUF, F.CPROF_UF) AS CARTEIRATRABALHOUF,
	                    ISNULL(DC.CERTIFICADORESERVISTA, F.CR_NUM) AS CERTIFICADORESERVISTA,
	                    ISNULL(DC.CERTIFICADORESERVISTASERIE, F.CR_SERIE) AS CERTIFICADORESERVISTASERIE, 
	                    ISNULL(DC.CERTIFICADORESERVISTAUF, FL.FL_FIELD_19) AS CERTIFICADORESERVISTAUF,
	                    ISNULL(DC.ENDERECO, F.ENDERECO) AS ENDERECO, 
	                    ISNULL(DC.ENDERECONUMERO, F.END_NUM) AS ENDERECONUMERO, 
	                    ISNULL(DC.ENDERECOCOMPLEMENTO, F.END_COMPL) AS ENDERECOCOMPLEMENTO, 
	                    UPPER(ISNULL(DC.ENDERECOBAIRRO, F.BAIRRO)) AS ENDERECOBAIRRO, 
                        M.UF AS ENDERECOUF, 
	                    ISNULL(DC.ENDERECOMUNICIPIO, F.END_MUNICIPIO) AS ENDERECOMUNICIPIO,
						M.NOME AS ENDMUNICIPIODESCRICAO,
	                    ISNULL(DC.CEP, F.CEP) AS CEP, 
	                    ISNULL(DC.TELEFONE, REPLACE(F.FONE, ' ', '')) AS TELEFONE,
	                    ISNULL(DC.CELULAR, REPLACE(F.CELULAR, ' ', '')) AS CELULAR,
	                    ISNULL(DC.EMAILPESSOAL, F.E_MAIL) AS EMAILPESSOAL,
                        ISNULL(DC.EMAILINSTITUCIONAL, F.E_MAIL_INTERNO) AS EMAILINSTITUCIONAL, 

	                    DC.ACUMULACAO,
	                    DC.FUNCAODIRETOR,
	                    DC.QTDEANOSGLP AS QUANTIDADEANOSGLP,
	                    DE.EXPERIENCIA AS EXPERIENCIA,
	                    DT.TITULACAO AS TITULACAO,
                        DC.UTILIZARUBRICA,
	                    DC.USUARIOID,
	                    DC.DATACADASTRO,
	                    DC.DATAALTERACAO,
                        DC.DATAMIGRACAOPUBLICACAO,
                        DC.DATACONVOCACAOPUBLICACAO
                    from VW_FUNCIONARIOS F
						LEFT JOIN RecursosHumanos.DOCENTECANDIDATO DC ON F.NUM_FUNC = DC.NUM_FUNC
									                    AND DC.CONCURSO = @CONCURSO	
						LEFT JOIN LY_UNIDADE_ENSINO UE ON F.UNIDADE_ENS = UE.UNIDADE_ENS 
						LEFT JOIN MUNICIPIO MUE WITH ( NOLOCK ) ON MUE.CODIGO = CONVERT(INT, ISNULL(DC.MUNICIPIO, UE.MUNICIPIO))                         
						LEFT JOIN APOLLO..REGIONAIS_UAS RUA (NOLOCK) ON F.SETOR = RUA.SETOR
						LEFT JOIN TCE_REGIONAL N ON N.ID_REGIONAL = ISNULL(ISNULL(DC.ID_REGIONAL, UE.ID_REGIONAL), RUA.ID_REGIONAL) 
						LEFT JOIN APOLLO..SEEDUC_UAS SEDE ( NOLOCK ) ON F.SETOR = SEDE.SETOR
	                    INNER JOIN LY_DOCENTE D (NOLOCK) ON F.MATRICULA = D.MATRICULA
									                    AND F.PESSOA = D.PESSOA
	                    LEFT JOIN LY_FL_PESSOA FL (NOLOCK) ON D.PESSOA = FL.PESSOA
	                    LEFT JOIN MUNICIPIO MNASC ON F.MUNICIPIO_NASC = MNASC.CODIGO
	                    LEFT JOIN HADES..HD_MUNICIPIO M WITH ( NOLOCK ) ON M.MUNICIPIO = F.END_MUNICIPIO 	                   
	                    LEFT JOIN RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA DE (NOLOCK) ON DC.DOCENTECANDIDATOID = DE.DOCENTECANDIDATOID	
	                    LEFT JOIN RecursosHumanos.DOCENTECANDIDATOTITULACAO DT (NOLOCK) ON DC.DOCENTECANDIDATOID = DT.DOCENTECANDIDATOID
                        INNER JOIN LY_GRUPO_HABILITACAO_DOC GHD WITH ( NOLOCK ) 
                                ON GHD.NUM_FUNC = D.NUM_FUNC 
                                    AND GHD.AGRUPAMENTO_INGRESSO = 'S' 
                                    AND GHD.PROVISORIO = 'N'
                        INNER JOIN LY_GRUPO_HABILITACAO_DISC DIS WITH ( NOLOCK ) 
                                ON GHD.AGRUPAMENTO = DIS.AGRUPAMENTO 
                        INNER JOIN DBO.LY_GRUPO_HABILITACAO GH WITH ( NOLOCK ) 
                                ON GHD.AGRUPAMENTO = GH.AGRUPAMENTO
                    WHERE IDVINCULO = @IDVINCULO
	                    AND (f.DATA_DESATIVACAO IS NULL OR f.DATA_DESATIVACAO >= CONVERT(DATE,GETDATE()))
                        AND f.DATA_NOMEACAO <= CONVERT(DATE,GETDATE()) ";

                contextQuery.Parameters.Add("@IDVINCULO", SqlDbType.VarChar, idVinculo);
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    //dados de identificação
                    dadosInscricaoMigracao.IdVinculo = Convert.ToString(reader["IDVINCULO"]);
                    dadosInscricaoMigracao.NumFunc = Convert.ToInt32(reader["NUMFUNC"]);
                    dadosInscricaoMigracao.Pessoa = Convert.ToInt32(reader["PESSOA"]);
                    dadosInscricaoMigracao.Concurso = Convert.ToString(reader["CONCURSO"]);
                    dadosInscricaoMigracao.DocenteCandidatoId = reader["DOCENTECANDIDATOID"] != DBNull.Value ? Convert.ToInt32(reader["DOCENTECANDIDATOID"]) : (int?)null;
                    dadosInscricaoMigracao.NumeroInscricao = Convert.ToString(reader["NUMEROINSCRICAO"]);
                    dadosInscricaoMigracao.DescricaoSituacao = Convert.ToString(reader["DESCRICAOSITUACAO"]);
                    dadosInscricaoMigracao.Situacao = Convert.ToString(reader["SITUACAO"]);

                    if (reader["DATAMIGRACAOPUBLICACAO"] != DBNull.Value)
                        dadosInscricaoMigracao.DataMigracao = Convert.ToDateTime(reader["DATAMIGRACAOPUBLICACAO"]);

                    if (reader["DATACONVOCACAOPUBLICACAO"] != DBNull.Value)
                        dadosInscricaoMigracao.DataConvocacao = Convert.ToDateTime(reader["DATACONVOCACAOPUBLICACAO"]);

                    //regional de origem
                    dadosInscricaoMigracao.RegionalId = reader["REGIONALID"] != DBNull.Value ? Convert.ToInt32(reader["REGIONALID"]) : (int?)null;
                    dadosInscricaoMigracao.RegionalDescricao = Convert.ToString(reader["REGIONALDESCRICAO"]);
                    dadosInscricaoMigracao.MunicipioCodigo = Convert.ToString(reader["MUNICIPIOCODIGO"]);
                    dadosInscricaoMigracao.MunicipioDescricao = Convert.ToString(reader["MUNICIPIODESCRICAO"]);
                    dadosInscricaoMigracao.Sede = Convert.ToString(reader["SEDE"]);
                    dadosInscricaoMigracao.DisciplinaIngresso = Convert.ToString(reader["DISCIPLINAINGRESSO"]);
                    dadosInscricaoMigracao.DisciplinaIngressoDescricao = Convert.ToString(reader["DISCIPLINAINGRESSODESCRICAO"]);

                    //dados do professor
                    dadosInscricaoMigracao.Nome = Convert.ToString(reader["NOME"]);
                    dadosInscricaoMigracao.DataNascimento = Convert.ToDateTime(reader["DATANASCIMENTO"]);
                    dadosInscricaoMigracao.EstadoCivil = Convert.ToString(reader["ESTADOCIVIL"]);
                    dadosInscricaoMigracao.NomeMae = Convert.ToString(reader["NOMEMAE"]);
                    dadosInscricaoMigracao.NomePai = Convert.ToString(reader["NOMEPAI"]);
                    dadosInscricaoMigracao.NaturalidadeId = Convert.ToString(reader["NATURALIDADEID"]);
                    dadosInscricaoMigracao.NaturalidadeDescricao = Convert.ToString(reader["NATURALIDADEDESCRICAO"]);
                    dadosInscricaoMigracao.NaturalidadeUf = Convert.ToString(reader["NATURALIDADEUF"]);
                    dadosInscricaoMigracao.NumeroDependentes = reader["NUMERODEPENDENTES"] != DBNull.Value ? Convert.ToInt32(reader["NUMERODEPENDENTES"]) : (int?)null;

                    //documentos pessoais
                    dadosInscricaoMigracao.Rg = Convert.ToString(reader["RG"]);
                    dadosInscricaoMigracao.RgOrgao = Convert.ToString(reader["RGORGAO"]);
                    dadosInscricaoMigracao.RgDataExpedicao = Convert.ToDateTime(reader["RGDATAEXPEDICAO"]);
                    dadosInscricaoMigracao.RgUf = Convert.ToString(reader["RGUF"]);
                    dadosInscricaoMigracao.Cpf = Convert.ToString(reader["CPF"]);
                    dadosInscricaoMigracao.PisPasep = Convert.ToString(reader["PISPASEP"]);
                    dadosInscricaoMigracao.TituloEleitor = Convert.ToString(reader["TITULOELEITOR"]);
                    dadosInscricaoMigracao.TituloEleitorZona = Convert.ToString(reader["TITULOELEITORZONA"]);
                    dadosInscricaoMigracao.TituloEleitorSecao = Convert.ToString(reader["TITULOELEITORSECAO"]);
                    dadosInscricaoMigracao.TituloEleitorUf = Convert.ToString(reader["TITULOELEITORUF"]);
                    dadosInscricaoMigracao.Cnh = Convert.ToString(reader["CNH"]);
                    dadosInscricaoMigracao.CnhCategoria = Convert.ToString(reader["CNHCATEGORIA"]);
                    dadosInscricaoMigracao.CnhValidade = reader["CNHVALIDADE"] != DBNull.Value ? Convert.ToDateTime(reader["CNHVALIDADE"]) : (DateTime?)null;
                    dadosInscricaoMigracao.CnhUf = Convert.ToString(reader["CNHUF"]);
                    dadosInscricaoMigracao.CarteiraTrabalho = Convert.ToString(reader["CARTEIRATRABALHO"]);
                    dadosInscricaoMigracao.CarteiraTrabalhoSerie = Convert.ToString(reader["CARTEIRATRABALHOSERIE"]);
                    dadosInscricaoMigracao.CarteiraTrabalhoUf = Convert.ToString(reader["CARTEIRATRABALHOUF"]);
                    dadosInscricaoMigracao.CertificadoReservista = Convert.ToString(reader["CERTIFICADORESERVISTA"]);
                    dadosInscricaoMigracao.CertificadoReservistaSerie = Convert.ToString(reader["CERTIFICADORESERVISTASERIE"]);
                    dadosInscricaoMigracao.CertificadoReservistaUf = Convert.ToString(reader["CERTIFICADORESERVISTAUF"]);

                    //endereço
                    dadosInscricaoMigracao.Cep = Convert.ToString(reader["CEP"]);
                    dadosInscricaoMigracao.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dadosInscricaoMigracao.EnderecoNumero = Convert.ToString(reader["ENDERECONUMERO"]);
                    dadosInscricaoMigracao.EnderecoComplemento = Convert.ToString(reader["ENDERECOCOMPLEMENTO"]);
                    dadosInscricaoMigracao.EnderecoBairro = Convert.ToString(reader["ENDERECOBAIRRO"]);
                    dadosInscricaoMigracao.EnderecoMunicipio = Convert.ToString(reader["ENDERECOMUNICIPIO"]);
                    dadosInscricaoMigracao.EndMunicipioDescricao = Convert.ToString(reader["ENDMUNICIPIODESCRICAO"]);
                    dadosInscricaoMigracao.EnderecoUf = Convert.ToString(reader["ENDERECOUF"]);

                    //contato
                    dadosInscricaoMigracao.Telefone = Convert.ToString(reader["TELEFONE"]);
                    dadosInscricaoMigracao.Celular = Convert.ToString(reader["CELULAR"]);
                    dadosInscricaoMigracao.EmailPessoal = Convert.ToString(reader["EMAILPESSOAL"]);
                    dadosInscricaoMigracao.EmailInstitucional = Convert.ToString(reader["EMAILINSTITUCIONAL"]);

                    //outros
                    dadosInscricaoMigracao.Acumulacao = reader["ACUMULACAO"] != DBNull.Value ? Convert.ToBoolean(reader["ACUMULACAO"]) : (bool?)null;
                    dadosInscricaoMigracao.FuncaoDiretor = reader["FUNCAODIRETOR"] != DBNull.Value ? Convert.ToBoolean(reader["FUNCAODIRETOR"]) : (bool?)null;
                    dadosInscricaoMigracao.QuantidadeAnosGlp = reader["QUANTIDADEANOSGLP"] != DBNull.Value ? Convert.ToInt32(reader["QUANTIDADEANOSGLP"]) : (int?)null;
                    dadosInscricaoMigracao.Experiencia = Convert.ToString(reader["EXPERIENCIA"]);
                    dadosInscricaoMigracao.Titulacao = Convert.ToString(reader["TITULACAO"]);
                    dadosInscricaoMigracao.UtilizaRubrica = reader["UTILIZARUBRICA"] != DBNull.Value ? Convert.ToBoolean(reader["UTILIZARUBRICA"]) : (bool?)null;                 
                    dadosInscricaoMigracao.UsuarioId = Convert.ToString(reader["USUARIOID"]);

                }

                reader.Close();

                return dadosInscricaoMigracao;
            }
          
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
              
            }
        }

        public ValidacaoDados Valida(DTO.DadosInscricaoMigracao dadosInscricaoMigracao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (!cadastro)
            {
                if (dadosInscricaoMigracao.DocenteCandidatoId == null || dadosInscricaoMigracao.DocenteCandidatoId <= 0)
                {
                    mensagens.Add("Campo DOCENTE é obrigatório.");
                }
            }

            if (dadosInscricaoMigracao.Pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (dadosInscricaoMigracao.NumFunc <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (dadosInscricaoMigracao.Concurso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CONCURSO é obrigatório.");
            }

            if (dadosInscricaoMigracao.EmailPessoal.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo E_MAIL PESSOAL é obrigatório.");
            }
            else
            {
                if (dadosInscricaoMigracao.EmailPessoal.Length > 100)
                {
                    mensagens.Add("O campo E-MAIL só pode ter até 100 caracteres.");
                }

                if (!Validacao.Email(dadosInscricaoMigracao.EmailPessoal))
                {
                    mensagens.Add("O campo E-MAIL está em um formato incorreto.");
                }
            }

            if (dadosInscricaoMigracao.NumeroDependentes == null || dadosInscricaoMigracao.NumeroDependentes < 0)
            {
                mensagens.Add("Campo NUMERO DE DEPENDENTES é obrigatório.");
            }

            if (dadosInscricaoMigracao.Sede.IsNullOrEmptyOrWhiteSpace())
            {

                if (dadosInscricaoMigracao.MunicipioCodigo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo MUNICIPIO é obrigatório.");
                }
            }

            if (dadosInscricaoMigracao.RegionalId == null || dadosInscricaoMigracao.RegionalId <= 0)
            {
                mensagens.Add("Campo REGIONAL é obrigatório.");
            }

            if (dadosInscricaoMigracao.QuantidadeAnosGlp == null || dadosInscricaoMigracao.QuantidadeAnosGlp < 0)
            {
                mensagens.Add("Campo QUANTIDADE ANOS GLP é obrigatório.");
            }

            if (dadosInscricaoMigracao.Acumulacao == null)
            {
                mensagens.Add("Campo ACUMULAÇÃO é obrigatório.");
            }

            if (dadosInscricaoMigracao.Concurso == "2024-Migracao") //Saiu em 2025
            {
                if (dadosInscricaoMigracao.FuncaoDiretor == null)
                {
                    mensagens.Add("Campo FUNÇÃO DIRETOR é obrigatório.");
                }
            }
            else
            {
                dadosInscricaoMigracao.FuncaoDiretor = false;
            }

            if (dadosInscricaoMigracao.UtilizaRubrica == null)
            {
                mensagens.Add("Campo UTILIZA RUBRICA é obrigatório.");
            }

            if (dadosInscricaoMigracao.DisciplinaIngresso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DISCIPLINA INGRESSO é obrigatório.");
            }

            if (dadosInscricaoMigracao.Experiencia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo EXPERIÊNCIA é obrigatório.");
            }

            if (dadosInscricaoMigracao.Titulacao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TITULAÇÃO é obrigatório.");
            }

            if (dadosInscricaoMigracao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca situação do docente
                    int situacao = this.ObtemSituacaoPor(contexto, Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId));

                    if (situacao > 0)
                    {
                        // Verifica se o candidato esta com situação convocado
                        if (situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Inscrito)
                        {
                            mensagens.Add("Este CANDIDATO já foi convocado e/ou Analisado.");
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

        public void Insere(DTO.DadosInscricaoMigracao dadosInscricaoMigracao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RN.RecursosHumanos.DocenteCandidatoExperiencia rnDocenteCandidatoExperiencia = new DocenteCandidatoExperiencia();
            RN.RecursosHumanos.DocenteCandidatoTitulacao rnDocenteCandidatoTitulacao = new DocenteCandidatoTitulacao();
            RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new DocenteCandidatoArquivo();
            RN.RecursosHumanos.DocenteCandidatoGLP rnDocenteCandidatoGLP = new DocenteCandidatoGLP();
            RN.RecursosHumanos.Entidades.DocenteCandidatoGLP docenteGLP = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteCandidatoGLP();

            try
            {
                //Atualiza email pessoal
                rnPessoa.AtualizaEmailPessoal(contexto, dadosInscricaoMigracao.Pessoa, dadosInscricaoMigracao.EmailPessoal, dadosInscricaoMigracao.UsuarioId);

                //Atualizar numero de dependentes
                rnFlPessoa.AtualizaNumeroDependentes(contexto, Convert.ToInt32(dadosInscricaoMigracao.Pessoa), Convert.ToInt32(dadosInscricaoMigracao.NumeroDependentes));

                //Insere CandidatoDocente
                this.Insere(contexto, dadosInscricaoMigracao);

                //Monta Experiencia
                Entidades.DocenteCandidatoExperiencia docenteCandidatoExperiencia = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteCandidatoExperiencia();
                docenteCandidatoExperiencia.DocenteCandidatoId = Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId);
                docenteCandidatoExperiencia.Concurso = dadosInscricaoMigracao.Concurso;
                docenteCandidatoExperiencia.Experiencia = dadosInscricaoMigracao.Experiencia;
                docenteCandidatoExperiencia.UsuarioId = dadosInscricaoMigracao.UsuarioId;

                //Insere Experiencia
                rnDocenteCandidatoExperiencia.Insere(contexto, docenteCandidatoExperiencia);

                //Monta Titulacao
                Entidades.DocenteCandidatoTitulacao docenteCandidatoTitulacao = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteCandidatoTitulacao();
                docenteCandidatoTitulacao.DocenteCandidatoId = Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId);
                docenteCandidatoTitulacao.Concurso = dadosInscricaoMigracao.Concurso;
                docenteCandidatoTitulacao.Titulacao = dadosInscricaoMigracao.Titulacao;
                docenteCandidatoTitulacao.UsuarioId = dadosInscricaoMigracao.UsuarioId;

                //Insere Titulacao
                rnDocenteCandidatoTitulacao.Insere(contexto, docenteCandidatoTitulacao);

                if (dadosInscricaoMigracao.AnosGLP != null)
                {
                    if (dadosInscricaoMigracao.AnosGLP.Count > 0)
                    {

                        foreach (var item in dadosInscricaoMigracao.AnosGLP)
                        {
                            docenteGLP = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteCandidatoGLP();
                            docenteGLP.Ano = item;
                            docenteGLP.DocenteCandidatoId = Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId);
                            docenteGLP.UsuarioId = dadosInscricaoMigracao.UsuarioId;

                            rnDocenteCandidatoGLP.Insere(contexto, docenteGLP);
                        }
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

        private void Insere(DataContext contexto, DTO.DadosInscricaoMigracao dadosInscricaoMigracao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.DOCENTECANDIDATO
                                           (PESSOA
                                           ,NUM_FUNC
                                           ,CONCURSO
                                           ,QTDEANOSGLP
                                           ,ACUMULACAO
                                           ,FUNCAODIRETOR
                                           ,UTILIZARUBRICA
                                           ,ID_REGIONAL
                                           ,MUNICIPIO
                                           ,SEDE
                                           ,DISCIPLINAINGRESSO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@PESSOA
                                           ,@NUM_FUNC
                                           ,@CONCURSO
                                           ,@QTDEANOSGLP
                                           ,@ACUMULACAO
                                           ,@FUNCAODIRETOR
                                           ,@UTILIZARUBRICA
                                           ,@ID_REGIONAL
                                           ,@MUNICIPIO
                                           ,@SEDE
                                           ,@DISCIPLINAINGRESSO
                                           ,@USUARIOID 
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO)

                                      SELECT IDENT_CURRENT('RecursosHumanos.DOCENTECANDIDATO') ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Int, dadosInscricaoMigracao.Pessoa);
            contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Int, dadosInscricaoMigracao.NumFunc);
            contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, dadosInscricaoMigracao.Concurso);
            contextQuery.Parameters.Add("@QTDEANOSGLP", SqlDbType.Int, dadosInscricaoMigracao.QuantidadeAnosGlp);
            contextQuery.Parameters.Add("@ACUMULACAO", SqlDbType.Bit, dadosInscricaoMigracao.Acumulacao);
            contextQuery.Parameters.Add("@FUNCAODIRETOR", SqlDbType.Bit, dadosInscricaoMigracao.FuncaoDiretor);
            contextQuery.Parameters.Add("@UTILIZARUBRICA", SqlDbType.Bit, dadosInscricaoMigracao.UtilizaRubrica);
            contextQuery.Parameters.Add("@ID_REGIONAL", SqlDbType.Int, dadosInscricaoMigracao.RegionalId);
            contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, dadosInscricaoMigracao.MunicipioCodigo);
            contextQuery.Parameters.Add("@SEDE", SqlDbType.VarChar, dadosInscricaoMigracao.Sede.IsNullOrEmptyOrWhiteSpace() ? DBNull.Value : (object)dadosInscricaoMigracao.Sede);
            contextQuery.Parameters.Add("@DISCIPLINAINGRESSO", SqlDbType.VarChar, dadosInscricaoMigracao.DisciplinaIngresso);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosInscricaoMigracao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            //obter id gerado
            dadosInscricaoMigracao.DocenteCandidatoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(DTO.DadosInscricaoMigracao dadosInscricaoMigracao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RN.RecursosHumanos.DocenteCandidatoExperiencia rnDocenteCandidatoExperiencia = new DocenteCandidatoExperiencia();
            RN.RecursosHumanos.DocenteCandidatoTitulacao rnDocenteCandidatoTitulacao = new DocenteCandidatoTitulacao();
            RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new DocenteCandidatoArquivo();
            RN.RecursosHumanos.DocenteCandidatoGLP rnDocenteCandidatoGLP = new DocenteCandidatoGLP();            
            RN.RecursosHumanos.Entidades.DocenteCandidatoGLP docenteGLP = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteCandidatoGLP();

            try
            {
                //Atualiza email pessoal
                rnPessoa.AtualizaEmailPessoal(contexto, dadosInscricaoMigracao.Pessoa, dadosInscricaoMigracao.EmailPessoal, dadosInscricaoMigracao.UsuarioId);

                //Atualizar numero de dependentes
                rnFlPessoa.AtualizaNumeroDependentes(contexto, Convert.ToInt32(dadosInscricaoMigracao.Pessoa), Convert.ToInt32(dadosInscricaoMigracao.NumeroDependentes));

                //Atualiza CandidatoDocente
                this.Atualiza(contexto, dadosInscricaoMigracao);

                //Atualiza Experiencia
                rnDocenteCandidatoExperiencia.Atualiza(contexto, Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId), dadosInscricaoMigracao.Concurso, dadosInscricaoMigracao.Experiencia, dadosInscricaoMigracao.UsuarioId);

                //Atualiza Titulacao
                rnDocenteCandidatoTitulacao.Atualiza(contexto, Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId), dadosInscricaoMigracao.Concurso, dadosInscricaoMigracao.Titulacao, dadosInscricaoMigracao.UsuarioId);

                rnDocenteCandidatoGLP.Remove(contexto, Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId));

                if (dadosInscricaoMigracao.AnosGLP != null)
                {
                    if (dadosInscricaoMigracao.AnosGLP.Count > 0)
                    {
                        foreach (var item in dadosInscricaoMigracao.AnosGLP)
                        {
                            docenteGLP = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteCandidatoGLP();
                            docenteGLP.Ano = item;
                            docenteGLP.DocenteCandidatoId = Convert.ToInt32(dadosInscricaoMigracao.DocenteCandidatoId);
                            docenteGLP.UsuarioId = dadosInscricaoMigracao.UsuarioId;

                            rnDocenteCandidatoGLP.Insere(contexto, docenteGLP);
                        }
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

        private void Atualiza(DataContext contexto, DTO.DadosInscricaoMigracao dadosInscricaoMigracao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE RecursosHumanos.DOCENTECANDIDATO set
                                               QTDEANOSGLP = @QTDEANOSGLP,
                                               ACUMULACAO = @ACUMULACAO,
                                               FUNCAODIRETOR = @FUNCAODIRETOR,
                                               UTILIZARUBRICA = @UTILIZARUBRICA,  
                                               ID_REGIONAL = @ID_REGIONAL,
                                               USUARIOID = @USUARIOID,     
                                               DATAALTERACAO = @DATAALTERACAO
                                        WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

            contextQuery.Parameters.Add("@QTDEANOSGLP", SqlDbType.Int, dadosInscricaoMigracao.QuantidadeAnosGlp);
            contextQuery.Parameters.Add("@ID_REGIONAL", SqlDbType.Int, dadosInscricaoMigracao.RegionalId);
            contextQuery.Parameters.Add("@ACUMULACAO", SqlDbType.Bit, dadosInscricaoMigracao.Acumulacao);
            contextQuery.Parameters.Add("@FUNCAODIRETOR", SqlDbType.Bit, dadosInscricaoMigracao.FuncaoDiretor);
            contextQuery.Parameters.Add("@UTILIZARUBRICA", SqlDbType.Bit, dadosInscricaoMigracao.UtilizaRubrica);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosInscricaoMigracao.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, dadosInscricaoMigracao.DocenteCandidatoId);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int docenteCandidatoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docenteCandidatoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca situação do docente
                    int situacao = this.ObtemSituacaoPor(contexto, Convert.ToInt32(docenteCandidatoId));

                    // Verifica se o candidato esta com situação convocado
                    if (situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Inscrito)
                    {
                        mensagens.Add("Este CANDIDATO já foi convocado e/ou Analisado.");
                    }
                }
                catch (Exception ex)
                {
                    if (ctx != null)
                    {
                        ctx.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (ctx != null)
                    {
                        ctx.Dispose();
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

        public void Remove(int docenteCandidatoId, string concurso, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            RN.RecursosHumanos.DocenteCandidatoExperiencia rnDocenteCandidatoExperiencia = new DocenteCandidatoExperiencia();
            RN.RecursosHumanos.DocenteCandidatoTitulacao rnDocenteCandidatoTitulacao = new DocenteCandidatoTitulacao();
            RN.RecursosHumanos.DocenteCandidatoArquivo rnDocenteCandidatoArquivo = new DocenteCandidatoArquivo();

            try
            {
                //Insere auditorio arquivos anteriores
                rnDocenteCandidatoArquivo.InsereAuditoriaPorCandidatoDocente(contexto, docenteCandidatoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName, usuarioId);

                //Remove arquivos anteriors
                rnDocenteCandidatoArquivo.RemovePorCandidatoDocente(contexto, docenteCandidatoId);

                rnDocenteCandidatoExperiencia.Remove(contexto, docenteCandidatoId, concurso);

                rnDocenteCandidatoTitulacao.Remove(contexto, docenteCandidatoId, concurso);

                Remove(contexto, docenteCandidatoId);
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

        private void Remove(DataContext contexto, int docenteCandidatoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  DELETE RecursosHumanos.DOCENTECANDIDATO                                               
                                        WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaAnalise(int docenteCandidatoId, int situacao, string usuarioAnalise)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docenteCandidatoId <= 0)
            {
                mensagens.Add("Campo CANDIDATO é obrigatório.");
            }

            if (situacao <= 0)
            {
                mensagens.Add("Campo SITUAÇÃO é obrigatório.");
            }
            else
            {
                //Verifica se é uma situação Valida para analise
                if (situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Aprovado
                    && situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Desclassificado
                    && situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Faltoso
                     && situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.EmProcesso
                    && situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Desistente)
                {
                    mensagens.Add("Campo SITUAÇÃO é inválido.");
                }
            }

            if (usuarioAnalise.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    var situacaoAtual = this.ObtemSituacaoPor(contexto, docenteCandidatoId);
                    // Verifica se o candidato esta com situação convocado
                    if (situacaoAtual == (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Inscrito ||
                        situacaoAtual == (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.MigracaoConcluida)
                    {
                        mensagens.Add("Este CANDIDATO ainda não foi convocado ou já teve a migração concluída.");
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

        public void AnaliseCandidato(string strConcurso, string strCandidato, string strSituacao, string strUsuario)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE RecursosHumanos.DOCENTECANDIDATO
										SET SITUACAO = @SITUACAO,
                                            DATAANALISE = getdate(),
                                            USUARIOANALISEID = @USUARIOANALISEID
										WHERE CONCURSO = @CONCURSO AND DOCENTECANDIDATOID = @CANDIDATO";

                contextQuery.Parameters.Add("@SITUACAO", strSituacao);
                contextQuery.Parameters.Add("@USUARIOANALISEID", strUsuario);
                contextQuery.Parameters.Add("@CANDIDATO", strCandidato);
                contextQuery.Parameters.Add("@CONCURSO", strConcurso);

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
            finally
            {
                ctx.Dispose();
            }
        }

        public int ObtemSituacaoPor(DataContext contexto, int docenteCandidatoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT SITUACAO
                                          FROM   [RecursosHumanos].[DOCENTECANDIDATO]
                                          WHERE  DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

                contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["SITUACAO"]);
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

        public int ObtemInscricoesParaConvocacaoPor(string disciplina, string concurso, string municipio, string regional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            int total;

            try
            {
                contextQuery.Command = @"SELECT COUNT(1)
                                                FROM   [RecursosHumanos].[DOCENTECANDIDATO] DC
                                                WHERE  DC.CONCURSO = @CONCURSO
                                                       AND ISNULL(DC.MUNICIPIO, @MUNICIPIO) = @MUNICIPIO
                                                       AND DC.ID_REGIONAL = @REGIONALID
													   AND DC.DISCIPLINAINGRESSO = @AGRUPAMENTO
													   AND SITUACAO = @SITUACAO ";

                contextQuery.Parameters.Add("@AGRUPAMENTO", disciplina);
                contextQuery.Parameters.Add("@CONCURSO", concurso);
                contextQuery.Parameters.Add("@MUNICIPIO", municipio);
                contextQuery.Parameters.Add("@REGIONALID", regional);
                contextQuery.Parameters.Add("@SITUACAO", (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Inscrito);

                total = ctx.GetReturnValue<int>(contextQuery);
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

            return total;
        }

        public ValidacaoDados ValidaSelecaoConvocacao(string quantidade, DateTime dataPublicacaoConvocacao, DateTime dataApresentacao, string concurso, string disciplina, string municipio, string regional, out DataTable inscritos)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            inscritos = new DataTable();

            if (municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo Município é de preenchimento obrigatório.");
            }

            if (concurso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo Concurso é de preenchimento obrigatório.");
            }

            if (disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo Disciplina é de preenchimento obrigatório.");
            }

            if (regional.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo Regional é de preenchimento obrigatório.");
            }

            if (quantidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo Quantidade é de preenchimento obrigatório.");
            }
            else
            {
                int qtde = 0;
                if (!Int32.TryParse(quantidade, out qtde))
                {
                    mensagens.Add("Quantidade inválida.");
                }

                if (Convert.ToInt32(quantidade) <= 0)
                {
                    mensagens.Add("Quantidade deve ser maior que zero.");
                }
            }

            if (dataApresentacao == DateTime.MinValue)
            {
                mensagens.Add("Horário de apresentação é de preenchimento obrigatório.");
            }
            else
            {
                //valida horário e data maior que corrente
                if (dataApresentacao <= DateTime.Now)
                {
                    mensagens.Add("Data e horário não podem ser menores que a data e horário correntes.");
                }

                if (!RN.ProcessoSeletivo.ValidaDataApresentacao(concurso, dataApresentacao))
                {
                    mensagens.Add("Data de apresentação não pode ser menor que 48 horas após a data da convocação, desconsiderados sábados e domingos.");
                }
            }

            if (regional.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("É necessário escolher uma regional");
            }

            if (dataPublicacaoConvocacao == DateTime.MinValue)
            {
                mensagens.Add("É necessário escolher a data de publicação da convocação.");
            }
            else
            {
                if (dataPublicacaoConvocacao > DateTime.Now)
                {
                    mensagens.Add("A Data da publicação da convocação não pode ser maior que a data atual.");
                }
            }

            if (mensagens.Count == 0)
            {
                inscritos = this.ListaInscritosParaConvocacaoPor(quantidade, concurso, disciplina, municipio, regional);

                // Verifica se tem candidato para convocar
                if (inscritos == null || inscritos.Rows.Count == 0)
                {
                    mensagens.Add("Não existem candidatos para serem selecionados.");
                }
                else
                {
                    int disponivel = inscritos.Rows.Count;

                    if (disponivel <= 0)
                    {
                        mensagens.Add("Não há inscrições disponíveis para o processo seletivo.");
                    }

                    if (Convert.ToInt32(quantidade) > disponivel)
                    {
                        mensagens.Add("Quantidade não pode ser maior que o número de inscrições disponíveis.");
                    }

                    if (Convert.ToInt32(quantidade) < 0)
                    {
                        mensagens.Add("Quantidade deve ser um número inteiro maior que zero.");
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

        public DataTable ListaInscritosParaConvocacaoPor(string qtd, string concurso, string disciplina, string municipio, string regional)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT TOP ( @QTD )
                            DC.DOCENTECANDIDATOID AS DOCENTECANDIDATOID, 
							REPLICATE('0',6 - LEN(DC.DOCENTECANDIDATOID)) + CONVERT(VARCHAR(6), DC.DOCENTECANDIDATOID) AS CANDIDATO,
                            P.NOME_COMPL AS NOME ,
                            P.DT_NASC AS DATANASC , 
                            DC.SITUACAO ,
							P.E_MAIL_INTERNO ,
							P.E_MAIL,
                            ( SELECT    Convert(decimal (10, 2), SUM(ctpont.PONTUACAO))										
                              FROM      ( (SELECT   pontuacao
                                           FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                    INNER JOIN RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA cade ON code.CONCURSO = cade.CONCURSO
                                                                                  AND code.EXPERIENCIA = cade.EXPERIENCIA
                                           WHERE    CADE.DOCENTECANDIDATOID = DC.DOCENTECANDIDATOID )
                                          UNION ALL
                                          ( SELECT  pontuacao
                                            FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                    JOIN RecursosHumanos.DOCENTECANDIDATOTITULACAO cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                                  AND codt.TITULACAO = cadt.TITULACAO
                                            WHERE   cadt.DOCENTECANDIDATOID = DC.DOCENTECANDIDATOID
                                          )
                                        ) ctpont 
                            ) 
							+ (CASE
								WHEN DC.FUNCAODIRETOR = 1 THEN 1 --FUNCAO DE DIRETOR CONTA 1 PONTO
								ELSE 0
							END)
							+ (DC.QTDEANOSGLP * 0.5) 

                            + ( CASE 
									 WHEN (select COUNT(*) from  RecursosHumanos.DOCENTECANDIDATO 	D2
									 INNER JOIN LY_CONCURSO_DOCENTE DO ON DO.CONCURSO=D2.CONCURSO 						
									 where  SITUACAO IN (8,7,2,3) AND ANO < (SELECT ANO FROM LY_CONCURSO_DOCENTE WHERE CONCURSO = @CONCURSO) 
									 AND DC.PESSOA = D2.PESSOA) = 0 THEN 2 ELSE 0 END --2 PONTOS QUEM NAO PARTICIPOU DE OUTRAS MIGRAÇOES
							)	

                            AS PONTUACAO --0,5 PONTOS POR ANO DE GLP
                    FROM    RecursosHumanos.DOCENTECANDIDATO DC
							iNNER JOIN LY_DOCENTE D (NOLOCK) ON DC.NUM_FUNC = D.NUM_FUNC
							iNNER JOIN LY_PESSOA P (NOLOCK) ON D.PESSOA = P.PESSOA
                    WHERE   DC.CONCURSO = @CONCURSO
                            AND DC.DISCIPLINAINGRESSO = @DISCIPLINA
                            AND DC.SITUACAO = @SITUACAO
                            AND ISNULL(DC.MUNICIPIO, @MUNICIPIO) = @MUNICIPIO
                            AND DC.ID_REGIONAL= @REGIONAL 
                    ORDER BY PONTUACAO DESC, DT_NASC ASC ";

                contextQuery.Parameters.Add("@QTD", SqlDbType.Int, qtd);
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);
                contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Int, regional);
                contextQuery.Parameters.Add("@SITUACAO", SqlDbType.Int, (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Inscrito);

                dt = contexto.GetDataTable(contextQuery);
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

            return dt;
        }

        public void Convoca(DataTable convocar, DateTime dataPublicacaoConvocacao, DateTime dataApresentacao, string usuarioResponsavel)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                List<string> ids = new List<string>();

                foreach (DataRow linha in convocar.Rows)
                {
                    ids.Add(Convert.ToString(linha["DOCENTECANDIDATOID"]));
                }

                contextQuery.Command = string.Format(@" UPDATE [RECURSOSHUMANOS].[DOCENTECANDIDATO]
                                        SET SITUACAO = @SITUACAO,
                                            DATACONVOCACAOPUBLICACAO = @DATACONVOCACAOPUBLICACAO,
                                            DATACONVOCACAO = @DATACONVOCACAO,
                                            DATAAPRESENTACAO = @DATAAPRESENTACAO,
                                            USUARIOCONVOCACAOID = @USUARIOCONVOCACAOID
                                        WHERE DOCENTECANDIDATOID IN ({0}) ", ids.Aggregate((x, y) => x + "," + y));

                contextQuery.Parameters.Add("@SITUACAO", SqlDbType.Int, (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Convocado);
                contextQuery.Parameters.Add("@DATACONVOCACAOPUBLICACAO", SqlDbType.DateTime, dataPublicacaoConvocacao);
                contextQuery.Parameters.Add("@DATACONVOCACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAAPRESENTACAO", SqlDbType.DateTime, dataApresentacao);
                contextQuery.Parameters.Add("@USUARIOCONVOCACAOID", SqlDbType.Int, usuarioResponsavel);

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

        public DataTable ListaAprovadosPor(string qtd, string concurso, string disciplina, DateTime dataApresentacao, string municipio, string regional)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT TOP ( @QTD )
                            DC.DOCENTECANDIDATOID AS DOCENTECANDIDATOID, 
							REPLICATE('0',6 - LEN(DC.DOCENTECANDIDATOID)) + CONVERT(VARCHAR(6), DC.DOCENTECANDIDATOID) AS CANDIDATO,
                            P.NOME_COMPL AS NOME ,
                            P.DT_NASC AS DATANASC , 
                            DC.SITUACAO ,
							P.E_MAIL_INTERNO ,
							P.E_MAIL,
                            ( SELECT    Convert(decimal (10, 2), SUM(ctpont.PONTUACAO))										
                              FROM      ( (SELECT   pontuacao
                                           FROM     LY_CONCURSO_DOC_EXPERIENCIA code
                                                    INNER JOIN RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA cade ON code.CONCURSO = cade.CONCURSO
                                                                                  AND code.EXPERIENCIA = cade.EXPERIENCIA
                                           WHERE    CADE.DOCENTECANDIDATOID = DC.DOCENTECANDIDATOID )
                                          UNION ALL
                                          ( SELECT  pontuacao
                                            FROM    LY_CONCURSO_DOC_TITULACOES codt
                                                    JOIN RecursosHumanos.DOCENTECANDIDATOTITULACAO cadt ON codt.CONCURSO = cadt.CONCURSO
                                                                                  AND codt.TITULACAO = cadt.TITULACAO
                                            WHERE   cadt.DOCENTECANDIDATOID = DC.DOCENTECANDIDATOID
                                          )
                                        ) ctpont 
                            ) 
							+ (CASE
								WHEN DC.FUNCAODIRETOR = 1 THEN 1 --FUNCAO DE DIRETOR CONTA 1 PONTO
								ELSE 0
							END)
							+ (DC.QTDEANOSGLP * 0.5) 
                            + ( CASE 
									 WHEN (select COUNT(*) from  RecursosHumanos.DOCENTECANDIDATO 	D2
									 INNER JOIN LY_CONCURSO_DOCENTE DO ON DO.CONCURSO=D2.CONCURSO 						
									 where  SITUACAO IN (8,7,2,3) AND ANO < (SELECT ANO FROM LY_CONCURSO_DOCENTE WHERE CONCURSO = @CONCURSO) 
									 AND DC.PESSOA = D2.PESSOA) = 0 THEN 2 ELSE 0 END --2 PONTOS QUEM NAO PARTICIPOU DE OUTRAS MIGRAÇOES
							)	
                            AS PONTUACAO --0,5 PONTOS POR ANO DE GLP
                    FROM    RecursosHumanos.DOCENTECANDIDATO DC
							iNNER JOIN LY_DOCENTE D (NOLOCK) ON DC.NUM_FUNC = D.NUM_FUNC
							iNNER JOIN LY_PESSOA P (NOLOCK) ON D.PESSOA = P.PESSOA
                    WHERE   DC.CONCURSO = @CONCURSO
                            AND DC.DISCIPLINAINGRESSO = @DISCIPLINA
                            AND DC.SITUACAO = @SITUACAO
                            AND ISNULL(DC.MUNICIPIO, @MUNICIPIO) = @MUNICIPIO
                            AND DC.ID_REGIONAL= @REGIONAL 
							AND DATAAPRESENTACAO = @DATAAPRESENTACAO
                    ORDER BY PONTUACAO DESC, DT_NASC ASC ";

                contextQuery.Parameters.Add("@QTD", SqlDbType.Int, qtd);
                contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);
                contextQuery.Parameters.Add("@REGIONAL", SqlDbType.Int, regional);
                contextQuery.Parameters.Add("@DATAAPRESENTACAO", SqlDbType.DateTime, dataApresentacao);
                contextQuery.Parameters.Add("@SITUACAO", SqlDbType.Int, (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Convocado);

                dt = contexto.GetDataTable(contextQuery);
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

            return dt;
        }

        public ValidacaoDados ValidaAlocacao(DTO.DadosAlocacaoMigracao dadosAlocacaoMigracao,string concurso)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            TConnection connection = null;
            RN.Lotacao rnLotacao = new Lotacao();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RecursosHumanos.Entidades.ChGlp chGlp = new RecursosHumanos.Entidades.ChGlp();
            RecursosHumanos.ChGlp rnChGlp = new Techne.Lyceum.RN.RecursosHumanos.ChGlp();
            int numeroMatriculas = 0;
            int agrupamentoCargo = 0;
            CategoriaDocente rnCategoriaDocente = new CategoriaDocente();
            int agrupamentoCargoSegundaMatricula = 0;
            Docentes rnDocentes = new Docentes();
            string categoriaSegundaMatricula = string.Empty;
            string segundaMatricula = string.Empty;
            DocenteGLP rnDocenteGLP = new DocenteGLP();
            RN.RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new AgrupamentoCargos();

            int glpMatricula = 0;
            int glpSegundaMatricula = 0;
            RN.Entidades.LyDocenteFuncaoGlp docenteFuncaoGlp = new Techne.Lyceum.RN.Entidades.LyDocenteFuncaoGlp();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosAlocacaoMigracao.DocenteCandidatoId <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (dadosAlocacaoMigracao.Pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (dadosAlocacaoMigracao.NumFunc <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (dadosAlocacaoMigracao.MatriculaDocente.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MATRICULA é obrigatório.");
            }

            if (dadosAlocacaoMigracao.CategoriaAnterior.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOVA CATEGORIA ANTERIOR é obrigatório.");
            }

            if (dadosAlocacaoMigracao.Categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CARGO é obrigatório.");
            }          

            if (dadosAlocacaoMigracao.Funcao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo FUNÇÃO é obrigatório.");
            }

            if (dadosAlocacaoMigracao.DataConvocacaoDO == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA D.O. CONVOCAÇAO é obrigatória.");
            }
            else
            {
                if (dadosAlocacaoMigracao.DataConvocacaoDO > DateTime.Now.Date)
                {
                    mensagens.Add("O campo DATA D.O. CONVOCAÇAO não pode ser maior que a data atual.");
                }
            }

            if (!dadosAlocacaoMigracao.Observacao.IsNullOrEmptyOrWhiteSpace())
            {
                if (dadosAlocacaoMigracao.Observacao.Length > 500)
                {
                    mensagens.Add("Campo OBSERVAÇÃO não pode ser maior que 500 caracteres.");
                }
            }

            if (dadosAlocacaoMigracao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (dadosAlocacaoMigracao.Aulas == null || dadosAlocacaoMigracao.Aulas.Count() == 0)
            {
                mensagens.Add("AULAS PARA ALOCAÇÃO não informadas.");
            }
            else
            {
                //Não permitir que sejam selecionados mais de 8 tempos de aula para alocação
                int chTotal = dadosAlocacaoMigracao.Aulas.Count();
                if (chTotal > 8)
                {
                    mensagens.Add("São permitidos no máximos 8 tempos para alocação.");
                }
                else if (chTotal < 8)
                {
                    mensagens.Add("Precisa ser informado 8 tempos para alocação.");
                }
                else if (dadosAlocacaoMigracao.Aulas.Count() != dadosAlocacaoMigracao.Aulas.Distinct().Count())
                {
                    mensagens.Add("Existem tempos repetidos.");
                }

                foreach (DTO.DadosTurmaAlocacao item in dadosAlocacaoMigracao.Aulas)
                {
                    if (item.Ano <= 0)
                    {
                        mensagens.Add("Campo ANO é obrigatório.");
                    }

                    if (item.Semestre < 0)
                    {
                        mensagens.Add("Campo SEMESTRE é obrigatório.");
                    }

                    if (item.Turma.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TURMA é obrigatório.");
                    }

                    if (item.Turno.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TURNO é obrigatório.");
                    }

                    if (item.Disciplina.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo DISCIPLINA é obrigatório.");
                    }

                    if (item.Faculdade.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo FACULDADE é obrigatório.");
                    }

                    if (item.TipoAula.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TIPO AULA é obrigatório.");
                    }

                    if (item.DiaSemana <= 0)
                    {
                        mensagens.Add("Campo DIASEMANA é obrigatório.");
                    }

                    if (item.Aula <= 0)
                    {
                        mensagens.Add("Campo AULA é obrigatório.");
                    }

                    if (item.NumFuncAnterior <= 0)
                    {
                        mensagens.Add("Campo CODIGO DOCENTE ANTERIOR é obrigatório.");
                    }

                    if (item.DataInicio == DateTime.MinValue)
                    {
                        mensagens.Add("Campo DATA INICIO é obrigatório.");
                    }

                    if (item.DataFim == DateTime.MinValue)
                    {
                        mensagens.Add("Campo DATA FIM é obrigatório.");
                    }

                    if (item.HoraInicio == DateTime.MinValue)
                    {
                        mensagens.Add("Campo HORA INICIO é obrigatório.");
                    }

                    if (item.HoraFim == DateTime.MinValue)
                    {
                        mensagens.Add("Campo HORA FIM é obrigatório.");
                    }

                    if (item.DadosHoraAula == null || item.DadosHoraAula.Aula == null || item.DadosHoraAula.Aula <= 0)
                    {
                        mensagens.Add("Campo DADOS HORA AULA é obrigatório.");
                    }

                    if (item.DadosTurma == null || item.DadosTurma.Turma.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo DADOS TURMA é obrigatório.");
                    }

                    if (item.NomeDisciplina.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo NOME DISCIPLINA é obrigatório.");
                    }
                }
            }
            RN.Funcao rnFuncao = new Funcao();

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca situação do docente
                    int situacao = this.ObtemSituacaoPor(contexto, dadosAlocacaoMigracao.DocenteCandidatoId);

                    // Verifica se o candidato esta com situação convocado
                    if (situacao == (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.MigracaoConcluida)
                    {
                        mensagens.Add("Este CANDIDATO já teve sua migração concluida.");
                    }
                    else if (situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Aprovado)
                    {
                        mensagens.Add("Este CANDIDATO ainda não foi aprovado.");
                    }

                    if (PossuiMigracaoAnteriorPor(contexto, dadosAlocacaoMigracao.NumFunc, concurso))
                    {
                        mensagens.Add("Este CANDIDATO não pode ser migrado pois já teve uma migração concluída.");
                    }

                    //Verifica se é regente (esta em aula)
                    if (!rnAulaDocente.ExisteAulaAlocadaPor(contexto, dadosAlocacaoMigracao.NumFunc, DateTime.Now))
                    {
                        mensagens.Add("Este Docente não possui aulas ativas.");
                    }


                    //Verifica se é regente (esta em aula)
                    if (!rnAulaDocente.ExisteAulaAlocadaPor(contexto, dadosAlocacaoMigracao.NumFunc, DateTime.Now))
                    {
                        mensagens.Add("Este Docente não possui aulas ativas.");
                    }

                    //Alocação não salva, pois há conflito de horários com as aulas já alocadas na(s) turma(s) xxxxxxx.
                    connection = Config.CreateConnection();
                    connection.Open();
                    foreach (DTO.DadosTurmaAlocacao item in dadosAlocacaoMigracao.Aulas)
                    {
                        //realiza validação entre os novos horario
                        int mesmoHorario = dadosAlocacaoMigracao.Aulas.Where(x => x.Aula != item.Aula && x.DiaSemana == item.DiaSemana &&
                            ((item.HoraInicio == x.HoraInicio) || (item.HoraInicio > x.HoraInicio && item.HoraInicio < x.HoraFim) || (item.HoraFim > x.HoraInicio && item.HoraFim < x.HoraFim))).Count();

                        if (mesmoHorario > 0)
                        {
                            var strHoraIni = new DateTime(1899, 12, 30, item.HoraInicio.Hour, item.HoraInicio.Minute, item.HoraInicio.Second);
                            var strHoraFim = new DateTime(1899, 12, 30, item.HoraFim.Hour, item.HoraFim.Minute, item.HoraFim.Second);


                            mensagens.Add(string.Format("Conflito de horário na {0} na disciplina {1} e no horário de {2} a {3} ", item.DiaSemanaDescricao, item.NomeDisciplina, String.Format("{0:HH:mm}", strHoraIni), String.Format("{0:HH:mm}", strHoraFim)));
                        }


                        string numFunc2Matricula = rnLotacao.ObtemNumFuncSegundaMatriculaAtivaPor(contexto, dadosAlocacaoMigracao.MatriculaDocente, dadosAlocacaoMigracao.Pessoa);
                        decimal numFunc2 = numFunc2Matricula.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToDecimal(numFunc2Matricula);

                        //1ª MATRICULA
                        //Realiza a validação do horário com aulas já salvas
                        ErrorList erroListValidacao = RN.Turma.ValidarHorarioMigracao(connection, item.DadosHoraAula, item.DadosTurma, dadosAlocacaoMigracao.NumFunc, numFunc2, dadosAlocacaoMigracao.MatriculaDocente, item.NomeDisciplina);
                        if (erroListValidacao != null && erroListValidacao["ERRO_VALIDACAO"].Length > 0)
                        {
                            mensagens.Add(erroListValidacao.ToString());
                        }

                        //Valida 2ª matricula
                        if (numFunc2 > 0)
                        {
                            erroListValidacao = RN.Turma.ValidarHorarioMigracao(connection, item.DadosHoraAula, item.DadosTurma, numFunc2, dadosAlocacaoMigracao.NumFunc, dadosAlocacaoMigracao.MatriculaDocente, item.NomeDisciplina);
                            if (erroListValidacao != null && erroListValidacao["ERRO_VALIDACAO"].Length > 0)
                            {
                                mensagens.Add(erroListValidacao.ToString());
                            }
                        }
                    }

                    //VALIDA TOTAL GLP

                    LyLotacao lotacaoSegundaMatricula = new LyLotacao();
                    //Busca quantidade de matrículas do docente
                    numeroMatriculas = rnLotacao.ObtemNumeroMatriculaAtivaPor(contexto, dadosAlocacaoMigracao.Pessoa);

                    agrupamentoCargo = rnCategoriaDocente.ObtemAgrupamentoCargoPor(contexto, dadosAlocacaoMigracao.Categoria);

                    bool permiteFuncao = false;
                    bool ehDocente;
                    VinculoLy rnVinculoLy = new VinculoLy();

                    if (numeroMatriculas == 2)
                    {
                        //Buscar Segunda matricula do servidor
                        segundaMatricula = rnLotacao.ObtemSegundaMatriculaAtivaPor(contexto, dadosAlocacaoMigracao.MatriculaDocente, dadosAlocacaoMigracao.Pessoa);

                        ehDocente = rnDocentes.EhMatriculaDocentePor(contexto, segundaMatricula);

                        if (ehDocente)
                        {
                            categoriaSegundaMatricula = rnDocentes.ObtemCategoriaPor(contexto, segundaMatricula);

                        }
                        else
                        {
                            categoriaSegundaMatricula = rnVinculoLy.ObtemCategoriaPor(contexto, segundaMatricula);
                        }
                        agrupamentoCargoSegundaMatricula = rnCategoriaDocente.ObtemAgrupamentoCargoPor(contexto, categoriaSegundaMatricula);


                        //Busca lotação ativa da segunda matricula
                        lotacaoSegundaMatricula = rnLotacao.ObtemLotacaoAtivaPor(contexto, segundaMatricula);

                        var funcao = rnFuncao.ObtemPor(contexto, lotacaoSegundaMatricula.Funcao);
                        if (!funcao.FuncaoBB.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (funcao.FuncaoBB.ToUpper() == "COMISSIONADA" || funcao.FuncaoBB.ToUpper() == "GRATIFICADA" || funcao.Campo07 == "N")
                            {
                                permiteFuncao = true;
                            }
                        }
                    }
                    int? ch2 = 0;
                    int? ch = rnAgrupamentoCargos.ObtemCargaHorariaPor(agrupamentoCargo);

                    if (agrupamentoCargoSegundaMatricula > 0)
                    {
                        ch2 = rnAgrupamentoCargos.ObtemCargaHorariaPor(agrupamentoCargoSegundaMatricula);
                    }

                    if ((ch + ch2) >= 70)
                    {
                        permiteFuncao = true;
                    }

                    //Busca quantidade de GLP permitida para as categorias + funçoes das matriculas do docente
                    chGlp = rnChGlp.ObtemPor(contexto, numeroMatriculas, agrupamentoCargo, dadosAlocacaoMigracao.Funcao, agrupamentoCargoSegundaMatricula, lotacaoSegundaMatricula.Funcao);


                    if (chGlp.ChGlpId == 0 && !permiteFuncao)
                    {
                        mensagens.Add("Migração não pode ser feita pois não existe carga horaria de glp cadastrada para o grupo de categoria / função de suas matriculas.");
                    }
                    else
                    {
                        glpMatricula = rnAulaDocente.ObtemQuantidadeAulasGLPsAtivasDocentePor(contexto, dadosAlocacaoMigracao.MatriculaDocente);

                        if (!segundaMatricula.IsNullOrEmptyOrWhiteSpace())
                        {
                            glpSegundaMatricula = rnAulaDocente.ObtemQuantidadeAulasGLPsAtivasDocentePor(contexto, segundaMatricula);
                        }

                        //Valida qtd máxima de pedidos de GPL
                        if (chGlp.Ch_Glp == 0 && !permiteFuncao)
                        {
                            mensagens.Add("Migração não pode ser feita, pois a carga horária para o grupo de categoria/função de suas matrículas já atingiu o limite de 65h.");
                        }
                        else
                        {
                            //Atualiza id encontrado
                            docenteFuncaoGlp.ChGlpId = chGlp.ChGlpId;


                            if ((glpMatricula + glpSegundaMatricula) > chGlp.Ch_Glp)
                            {
                                mensagens.Add("Migração não pode ser feita, pois ultrapassou o total de GLP permitida.");
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

                    if (connection != null)
                    {
                        connection.Close();
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

        public void Aloca(DTO.DadosAlocacaoMigracao dadosAlocacaoMigracao)
        {
            AulasDesalocadas rnAulasDesalocadas = new AulasDesalocadas();
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();
            RN.Docentes rnDocente = new Docentes();
            RN.Lotacao rnLotacao = new Lotacao();
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            RN.Turma rnTurma = new Turma();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                foreach (DTO.DadosTurmaAlocacao item in dadosAlocacaoMigracao.Aulas)
                {
                    //Verifica se a aula continua para o msm docente
                    string numFunc = rnAulaDocente.RetornaDocenteEmAulaPor(contexto, item.Turma, item.Ano, item.Semestre, item.Turno, item.Faculdade, item.Disciplina, item.DiaSemana, item.Aula);
                    if (numFunc != item.NumFuncAnterior.ToString())
                    {
                        throw new Exception("ERRO: As AULAS PARA ALOCAÇÃO estão desatualizadas, favor realizar novamente a escolha.");
                    }

                    //Guardar aulas anteriores na tabela de controle
                    rnAulasDesalocadas.Insere(contexto, item, dadosAlocacaoMigracao.DocenteCandidatoId, dadosAlocacaoMigracao.UsuarioId);

                    DateTime dataFimAula = DateTime.Now.Date;

                    //Verifica se a data atual é anterior a data de inicio da turma
                    DateTime dataInicioTurma = rnTurma.ObtemDataInicioPor(contexto, item.Ano, item.Semestre, item.Turma);
                    if (dataFimAula.Date < dataInicioTurma.Date)
                    {
                        //Caso o data atual sejá menor que a data inicio da turma, usar data inicio da turma
                        dataFimAula = dataInicioTurma.Date;
                    }

                    //desaloca aulas anteriores
                    rnAulaDocente.DesalocaAulas(contexto, item.NumFuncAnterior, item.Turno, item.Faculdade, item.DiaSemana, item.Aula, item.Disciplina, item.Turma, item.Ano, item.Semestre, item.DataInicio, dataFimAula);

                    //desaloca aulas glp anteriores
                    rnAulaDocenteTipo.DesalocaAulasTipo(contexto, item.NumFuncAnterior, item.Turno, item.Faculdade, item.DiaSemana, item.Aula, item.Disciplina, item.Turma, item.Ano, item.Semestre, item.DataInicio, dataFimAula);

                    //Atualiza glps usadas
                    rnDocenteGLP.AtualizaGlpUsadaPor(contexto, item.NumFuncAnterior, dataFimAula);

                    //Monta aula que será inserida
                    RN.Entidades.LyAulaDocente aulaDocente = new Techne.Lyceum.RN.Entidades.LyAulaDocente();
                    aulaDocente.NumFunc = dadosAlocacaoMigracao.NumFunc;
                    aulaDocente.Turno = item.Turno;
                    aulaDocente.Faculdade = item.Faculdade;
                    aulaDocente.DiaSemana = item.DiaSemana;
                    aulaDocente.Aula = item.Aula;
                    aulaDocente.Disciplina = item.Disciplina;
                    aulaDocente.Turma = item.Turma;
                    aulaDocente.Ano = item.Ano;
                    aulaDocente.Semestre = item.Semestre;
                    aulaDocente.Tipo = null;
                    aulaDocente.DataInicio = dataFimAula;
                    aulaDocente.DataFim = item.DataFim;
                    aulaDocente.TipoDocente = item.TipoDocente;

                    //Aloca aulas novas
                    rnAulaDocente.Insere(contexto, aulaDocente);
                }

                //Troca de cargo
                rnDocente.AtualizaCargo(contexto, dadosAlocacaoMigracao.NumFunc, dadosAlocacaoMigracao.Categoria, dadosAlocacaoMigracao.UsuarioId);

                //Busca dados da lotação atual
                RN.Entidades.LyLotacao lotacaoBanco = rnLotacao.ObtemLotacaoAtivaPor(contexto, dadosAlocacaoMigracao.MatriculaDocente);

                //Monta lotação que será inserida 
                RN.Entidades.LyLotacao lotacao = new Techne.Lyceum.RN.Entidades.LyLotacao();
                lotacao.Ordem = rnLotacao.ObtemProximaOrdemPor(contexto, dadosAlocacaoMigracao.MatriculaDocente);
                lotacao.Pessoa = dadosAlocacaoMigracao.Pessoa;
                lotacao.Matricula = dadosAlocacaoMigracao.MatriculaDocente;
                lotacao.Funcao = dadosAlocacaoMigracao.Funcao;
                lotacao.DataNomeacao = DateTime.Now.Date;
                lotacao.UnidadeEns = lotacaoBanco.UnidadeEns;
                lotacao.UnidadeFis = lotacaoBanco.UnidadeFis;
                lotacao.Setor = lotacaoBanco.Setor;
                lotacao.Categoria = dadosAlocacaoMigracao.Categoria;
                lotacao.Nucleo = lotacaoBanco.Nucleo;
                lotacao.Usuario = dadosAlocacaoMigracao.UsuarioId;
                lotacao.DataAtualizacao = DateTime.Now;
                lotacao.DataNomeacaoDo = null;
                lotacao.DataDesativacaoDo = null;
                lotacao.AtoOficial = null;
                lotacao.RespDocumentacao = null;
                lotacao.Readaptado = null;

                //Finaliza lotacao anterior - Data da desativação da lotação anterior, sera a data da nomeação - 1
                rnLotacao.DesativaLotacaoMigracao(contexto, Convert.ToDateTime(lotacao.DataNomeacao).AddDays(-1), dadosAlocacaoMigracao.UsuarioId, dadosAlocacaoMigracao.MatriculaDocente);

                //Insere nova lotacao
                rnLotacao.Insere(contexto, lotacao);

                //Atualiza situaçao para final
                this.AtualizaSituacaoComObservacao(contexto, dadosAlocacaoMigracao.DocenteCandidatoId, (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.MigracaoConcluida, dadosAlocacaoMigracao.UsuarioId, dadosAlocacaoMigracao.DataConvocacaoDO,dadosAlocacaoMigracao.Observacao);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.")
                    && !Convert.ToString(ex.Message).Contains("ERRO: "))
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

        public void AtualizaCargo(int docenteCandidatoId, int numFunc, string categoria, string usuarioId, string matricula, DateTime dataConvocacaoDO)
        {
            RN.Docentes rnDocente = new Docentes();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Lotacao rnLotacao = new Lotacao();

            try
            {
                //Atualiza cargo
                rnDocente.AtualizaCargo(contexto, numFunc, categoria, usuarioId);

                //Busca dados da lotação atual
                RN.Entidades.LyLotacao lotacaoBanco = rnLotacao.ObtemLotacaoAtivaPor(contexto, matricula);

                //Monta lotação que será inserida 
                RN.Entidades.LyLotacao lotacao = new Techne.Lyceum.RN.Entidades.LyLotacao();
                lotacao.Ordem = rnLotacao.ObtemProximaOrdemPor(contexto, matricula);
                lotacao.Pessoa = lotacaoBanco.Pessoa;
                lotacao.Matricula = matricula;
                lotacao.Funcao = lotacaoBanco.Funcao;
                lotacao.DataNomeacao = DateTime.Now.Date;
                lotacao.UnidadeEns = lotacaoBanco.UnidadeEns;
                lotacao.UnidadeFis = lotacaoBanco.UnidadeFis;
                lotacao.Setor = lotacaoBanco.Setor;
                lotacao.Categoria = categoria;
                lotacao.Nucleo = lotacaoBanco.Nucleo;
                lotacao.Usuario = usuarioId;
                lotacao.DataAtualizacao = DateTime.Now;
                lotacao.DataNomeacaoDo = null;
                lotacao.DataDesativacaoDo = null;
                lotacao.AtoOficial = null;
                lotacao.RespDocumentacao = null;
                lotacao.Readaptado = null;

                //Finaliza lotacao anterior - Data da desativação da lotação anterior, sera a data da nomeação - 1
                rnLotacao.DesativaLotacao(contexto, Convert.ToDateTime(lotacao.DataNomeacao).AddDays(-1), usuarioId, matricula);

                //Insere nova lotacao
                rnLotacao.Insere(contexto, lotacao);

                //Atualiza situaçao para final
                this.AtualizaSituacao(contexto, docenteCandidatoId, (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.MigracaoConcluida, usuarioId, dataConvocacaoDO);
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

        public ValidacaoDados ValidaAtualizaCargo(int docenteCandidatoId, int numFunc, string categoria, string usuarioId, bool NaoRegenteComGLP, string matricula, DateTime dataConvocacaoDO, string concurso, int pessoa)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (docenteCandidatoId <= 0)
            {
                mensagens.Add("Campo CANDIDATO é obrigatório.");
            }

            if (numFunc <= 0)
            {
                mensagens.Add("Campo DOCENTE é obrigatório.");
            }

            if (categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CATEGORIA é obrigatório.");
            }

            if (matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MATRICULA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSÁVEL é obrigatório.");
            }

            if (dataConvocacaoDO == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA CONVOCAÇAO D.O. é obrigatória.");
            }
            else
            {
                if (dataConvocacaoDO > DateTime.Now.Date)
                {
                    mensagens.Add("O campo DATA D.O. CONVOCAÇAO não pode ser maior que a data atual.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca situação do docente
                    int situacao = this.ObtemSituacaoPor(contexto, docenteCandidatoId);

                    // Verifica se o candidato esta com situação convocado
                    if (situacao == (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.MigracaoConcluida)
                    {
                        mensagens.Add("Este CANDIDATO já teve sua migração concluida.");
                    }
                    else if (situacao != (int)RN.RecursosHumanos.DocenteCandidato.EnumSituacao.Aprovado)
                    {
                        mensagens.Add("Este CANDIDATO ainda não foi aprovado.");
                    }

                    if (PossuiMigracaoAnteriorPor(contexto, numFunc, concurso))
                    {
                        mensagens.Add("Este CANDIDATO não pode ser migrado pois já teve uma migração concluída.");
                    }

                    //Verifica se é regente (esta em aula)
                    if (!NaoRegenteComGLP)
                    {
                        if (rnAulaDocente.ExisteAulaAlocadaMigracaoPor(contexto, numFunc, DateTime.Now))
                        {
                            mensagens.Add("Este Docente possui aulas ativas e com isso precisa realizar a Migração com alocação das aulas.");
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

        private void AtualizaSituacao(DataContext contexto, int docenteCandidatoId, int situacao, string usuarioId, DateTime dataConvocacaoDO)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE [RECURSOSHUMANOS].[DOCENTECANDIDATO]
                                        SET SITUACAO = @SITUACAO,
                                            DATAALTERACAO = @DATAALTERACAO,
                                            USUARIOID = @USUARIOID,
                                            DATAMIGRACAOPUBLICACAO = @DATAMIGRACAOPUBLICACAO                                            
                                        WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

            contextQuery.Parameters.Add("@SITUACAO", SqlDbType.Int, situacao);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            contextQuery.Parameters.Add("@DATAMIGRACAOPUBLICACAO", SqlDbType.DateTime, dataConvocacaoDO);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaSituacaoComObservacao(DataContext contexto, int docenteCandidatoId, int situacao, string usuarioId, DateTime? dataConvocacaoDO, string observacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE [RECURSOSHUMANOS].[DOCENTECANDIDATO]
                                        SET SITUACAO = @SITUACAO,
                                            DATAALTERACAO = @DATAALTERACAO,
                                            USUARIOID = @USUARIOID,
                                            DATAMIGRACAOPUBLICACAO = @DATAMIGRACAOPUBLICACAO,
                                            OBSERVACAO = @OBSERVACAO
                                        WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

            contextQuery.Parameters.Add("@SITUACAO", SqlDbType.Int, situacao);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            contextQuery.Parameters.Add(
                "@DATAMIGRACAOPUBLICACAO",
                SqlDbType.DateTime,
                dataConvocacaoDO.HasValue ? (object)dataConvocacaoDO.Value : DBNull.Value
            );

            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, observacao);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaData(int docenteCandidatoId, string usuarioId, DateTime dataConvocacaoDO)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE [RECURSOSHUMANOS].[DOCENTECANDIDATO]
                                        SET
                                            DATAALTERACAO = @DATAALTERACAO,
                                            USUARIOID = @USUARIOID,
                                            DATAMIGRACAOPUBLICACAO = @DATAMIGRACAOPUBLICACAO
                                        WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID ";

                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
                contextQuery.Parameters.Add("@DATAMIGRACAOPUBLICACAO", SqlDbType.DateTime, dataConvocacaoDO);

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

        private bool PossuiMigracaoAnteriorPor(DataContext ctx, decimal numFunc, string concurso)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                                FROM   [RECURSOSHUMANOS].[DOCENTECANDIDATO]
                                     
                                WHERE  NUM_FUNC = @NUMFUNC  
                                       AND CONCURSO <> @CONCURSO
                                       AND SITUACAO = 8"
            };

            contextQuery.Parameters.Add("@NUMFUNC", numFunc);
            contextQuery.Parameters.Add("@CONCURSO", concurso);


            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        private bool PossuiMigracaoAnteriorEfetivada(DataContext ctx, decimal pessoa, int ano)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @"   SELECT COUNT(1)
                                  FROM RecursosHumanos.DOCENTECANDIDATO D
                                     INNER JOIN LY_CONCURSO_DOCENTE C ON D.CONCURSO = C.CONCURSO
                                  WHERE SITUACAO IN (2, 3, 7, 8) -- Convocado = 2, Aprovado = 3, EmProcesso = 7, MigracaoConcluida = 8
                                     AND PESSOA = @PESSOA
                                     AND C.ANO < @ANO"
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@ANO", ano);


            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }
    }
}