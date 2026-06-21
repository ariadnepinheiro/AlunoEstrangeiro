using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno
{
    public class Candidato : RNBase
    {
        public static DataTable ListaDadosInscricaoCandidato(int candidatoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dadosInscricao = null;

            try
            {
                contextQuery.Command = @"SELECT C.NOMECOMPLETO, 
	                                            C.DATANASCIMENTO, 
	                                            C.MUNICIPIONASCIMENTO, 
	                                            C.PAISNASCIMENTO, 
	                                            C.NACIONALIDADE,
	                                            C.NOMEPAI, 
	                                            C.NOMEMAE, 
	                                            C.SEXO, 
	                                            C.ESTADOCIVIL,
	                                            C.ETNIA,
	                                            C.CREDO,
	                                            C.NECESSIDADEESPECIAL,
	                                            C.ENDERECO,
	                                            C.ENDERECONUMERO,
	                                            C.ENDERECOCOMPLEMENTO,
	                                            C.ENDERECOBAIRRO,
	                                            C.ENDERECOMUNICIPIO,
	                                            C.ENDERECOPAIS,
	                                            C.ENDERECOCEP,
	                                            C.LOCALIZACAOZONARESIDENCIA,
	                                            C.TELEFONE,
	                                            C.CELULAR,
	                                            C.EMAIL,
	                                            C.RGTIPO,
	                                            C.RGNUMERO,
	                                            C.RGEMISSOR,
	                                            C.RGUF,
	                                            C.RGDATAEXPEDICAO,
	                                            C.RGCOMPLEMENTO,
	                                            C.CPF,
	                                            C.TIPOCERTIDAO,
	                                            C.CERTIDAONUMERO,
	                                            C.CERTIDAOFOLHA,
	                                            C.CERTIDAOLIVRO,
	                                            C.CERTIDAODATAEMISSAO,
	                                            C.CERTIDAOCARTORIOUF,
	                                            C.CERTIDAOCARTORIOEXPEDICAO,
	                                            C.CERTIDAONUMEROMATRICULA,
	                                            C.CARTORIOID,
	                                            C.MODELOCERTIDAOCIVIL,
	                                            C.RESPONSAVEL,
	                                            C.RESPONSAVELNOME,
	                                            C.RESPONSAVELCPF,
	                                            C.PAICPF,
	                                            C.MAECPF,
	                                            C.PAIFALECIDO,
	                                            C.MAEFALECIDA,
	                                            C.PAITELEFONE,
	                                            C.MAETELEFONE,
	                                            C.RESPONSAVELTELEFONE,
	                                            C.ALUNOID, 
	                                            C.REDEENSINOORIGEM, 
	                                            C.TIPOBOLSAPARTICULAR, 
	                                            car.codigo_municipio, 
	                                            M.UF_SIGLA
                                           FROM LYCEUM.PROCESSOSELETIVOALUNO.CANDIDATO C
                                           LEFT JOIN CARTORIO Car 
                                             ON car.cod_cartorio = c.CARTORIOID
                                           LEFT JOIN MUNICIPIO M ON C.ENDERECOMUNICIPIO = M.CODIGO
                                          WHERE C.CANDIDATOID = @CANDIDATOID";

                contextQuery.Parameters.Add("@CANDIDATOID ", candidatoId);
                dadosInscricao = ctx.GetDataTable(contextQuery);
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

            return dadosInscricao;
        }

        public static DataTable ListaDadosRedeEstadual(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dadosInscricao = null;

            try
            {
                contextQuery.Command = @"SELECT 
                                               P.[PESSOA],
                                               P.[NOME_COMPL] NOMECOMPLETO,
                                               P.[DT_NASC] DATANASCIMENTO,
                                               P.[MUNICIPIO_NASC] MUNICIPIONASCIMENTO,
                                               P.[PAIS_NASC] PAISNASCIMENTO,
                                               P.[NACIONALIDADE] NACIONALIDADE,
                                               P.[NOME_PAI] NOMEPAI,
                                               P.[NOME_MAE] NOMEMAE,
                                               P.[SEXO] SEXO,
                                               P.[EST_CIVIL] ESTADOCIVIL,
                                               P.[ETNIA] ETNIA,
                                               P.[CREDO] CREDO,
                                               P.[NECESSIDADE_ESPECIAL] NECESSIDADEESPECIAL,
                                               P.[ENDERECO] ENDERECO,
                                               P.[END_NUM] ENDERECONUMERO,
                                               P.[END_COMPL] ENDERECOCOMPLEMENTO,
                                               P.[BAIRRO] ENDERECOBAIRRO,
                                               P.[END_MUNICIPIO] ENDERECOMUNICIPIO,
                                               P.[END_PAIS] ENDERECOPAIS,
                                               P.[CEP] ENDERECOCEP,
                                               FP.[FL_FIELD_01] LOCALIZACAOZONARESIDENCIA,
                                               P.[FONE] TELEFONE,
                                               P.[CELULAR] CELULAR,
                                               P.[E_MAIL] EMAIL,
                                               P.[RG_TIPO] RGTIPO,
                                               P.[RG_NUM] RGNUMERO,
                                               P.[RG_EMISSOR] RGEMISSOR,
                                               P.[RG_UF] RGUF,
                                               P.[RG_DTEXP] RGDATAEXPEDICAO,
                                               FP.[FL_FIELD_07] RGCOMPLEMENTO,
                                               P.[CPF] CPF,
                                               FP.[FL_FIELD_02] TIPOCERTIDAO,
                                               P.[CERT_NASC_NUM] CERTIDAONUMERO,
                                               P.[CERT_NASC_FOLHA] CERTIDAOFOLHA,
                                               P.[CERT_NASC_LIVRO] CERTIDAOLIVRO,
                                               P.[CERT_NASC_EMISSAO] CERTIDAODATAEMISSAO,
                                               car.[UF] CERTIDAOCARTORIOUF,
                                               P.[CERT_NASC_CARTORIO_EXPED] CERTIDAOCARTORIOEXPEDICAO,
                                               P.[CERT_NUMERO_MATRICULA] CERTIDAONUMEROMATRICULA,
                                               P.[ID_CARTORIO] CARTORIOID,
                                               FP.[FL_FIELD_09] MODELOCERTIDAOCIVIL,
                                               P.[RESPONSAVEL] RESPONSAVEL,
                                               P.[RESP_NOME_COMPL] RESPONSAVELNOME,
                                               P.[RESP_CPF] RESPONSAVELCPF,
                                               P.[PAI_CPF] PAICPF,
                                               P.[MAE_CPF] MAECPF,
                                               P.[PAI_FALECIDO] PAIFALECIDO,
                                               P.[MAE_FALECIDA] MAEFALECIDA,
                                               P.[PAI_TELEFONE] PAITELEFONE,
                                               P.[MAE_TELEFONE] MAETELEFONE,
                                               P.[RESP_FONE] RESPONSAVELTELEFONE,
                                               A.[ALUNO] ALUNOID,
	                                           car.codigo_municipio,
	                                           m.UF_SIGLA
                                          FROM [LYCEUM].[DBO].[LY_PESSOA] P
                                             INNER JOIN [LYCEUM].[DBO].[LY_ALUNO] A
                                                ON P.[PESSOA] = A.[PESSOA]
                                             INNER JOIN [LYCEUM].[DBO].[LY_FL_PESSOA] FP
                                                ON P.[PESSOA] = FP.[PESSOA]
                                              LEFT JOIN CARTORIO Car 
                                              ON car.cod_cartorio = p.ID_CARTORIO
                                              LEFT JOIN MUNICIPIO M ON P.END_MUNICIPIO = M.CODIGO      
                                            WHERE A.[ALUNO] = @ALUNO";

                contextQuery.Parameters.Add("@ALUNO ", aluno);
                dadosInscricao = ctx.GetDataTable(contextQuery);
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

            return dadosInscricao;
        }

        public static int BuscaCandidadoID(RN.ProcessoSeletivoAluno.Entidades.Candidato candidato, DataContext ctx)
        {
            ContextQuery contextQuery = new ContextQuery();
            int candidatoId = 0;

            try
            {
                contextQuery.Command = @"SELECT CANDIDATOID 
	                                           FROM LYCEUM.ProcessoSeletivoAluno.CANDIDATO 
		                                      WHERE NOMECOMPLETO = @NOMECOMPLETO
		                                        AND DATANASCIMENTO = @DATANASCIMENTO
		                                        AND NOMEMAE = @NOMEMAE";

                contextQuery.Parameters.Add("@NOMECOMPLETO", candidato.NomeCompleto);
                contextQuery.Parameters.Add("@DATANASCIMENTO", candidato.DataNascimento);
                contextQuery.Parameters.Add("@NOMEMAE", candidato.NomeMãe);

                candidatoId = ctx.GetReturnValue<int>(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return candidatoId;
        }

        public static void SalvaDadosCandidato(RN.ProcessoSeletivoAluno.Entidades.Candidato candidato, List<RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato> listRecursoAplicacaoProvaCandidato, RN.ProcessoSeletivoAluno.Entidades.Inscricao inscricao, RN.ProcessoSeletivoAluno.Entidades.UnidadeEnsinoCursoTurnoInscricao unidadeEnsinoCursoTurnoInscricao, int agendaId, out int inscricaoId, out Int64 numeroInscricao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                using (DataContext ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    contextQuery.Command = @"INSERT INTO LYCEUM.ProcessoSeletivoAluno.CANDIDATO 
                                         (
		                                     NOMECOMPLETO,
                                             DATANASCIMENTO,
                                             MUNICIPIONASCIMENTO,
                                             PAISNASCIMENTO,
                                             NACIONALIDADE, 
                                             NOMEPAI,
                                             NOMEMAE,
                                             SEXO,
                                             ESTADOCIVIL,
                                             ETNIA,
                                             CREDO,
                                             NECESSIDADEESPECIAL,
                                             ENDERECO,
                                             ENDERECONUMERO,
                                             ENDERECOCOMPLEMENTO,
                                             ENDERECOBAIRRO,
                                             ENDERECOMUNICIPIO,
                                             ENDERECOPAIS,
                                             ENDERECOCEP,
                                             LOCALIZACAOZONARESIDENCIA,
                                             TELEFONE,
                                             CELULAR,
                                             EMAIL,
                                             RGTIPO,
                                             RGNUMERO,
                                             RGEMISSOR,
                                             RGUF,
                                             RGDATAEXPEDICAO,
                                             RGCOMPLEMENTO,
                                             CPF,
                                             TIPOCERTIDAO,
                                             CERTIDAONUMERO,
                                             CERTIDAOFOLHA,
                                             CERTIDAOLIVRO,
                                             CERTIDAODATAEMISSAO,
                                             CERTIDAOCARTORIOUF,
                                             CERTIDAOCARTORIOEXPEDICAO,
                                             CERTIDAONUMEROMATRICULA,
                                             CARTORIOID,
                                             MODELOCERTIDAOCIVIL,
                                             RESPONSAVEL,
                                             RESPONSAVELNOME,
                                             RESPONSAVELCPF, 
                                             REDEENSINOORIGEM,
                                             TIPOBOLSAPARTICULAR,
                                             ALUNOID,
                                             DATAALTERACAO,
                                             IP,
                                             DATACADASTRO,
                                             PAICPF,
                                             MAECPF,
                                             PAIFALECIDO,
                                             MAEFALECIDA,
                                             PAITELEFONE,
                                             MAETELEFONE, 
                                             RESPONSAVELTELEFONE
                                         ) VALUES (
                                             @NOMECOMPLETO,
                                             @DATANASCIMENTO,
                                             @MUNICIPIONASCIMENTO,
                                             @PAISNASCIMENTO, 
                                             @NACIONALIDADE, 
                                             @NOMEPAI,
                                             @NOMEMAE,
                                             @SEXO,
                                             @ESTADOCIVIL,
                                             @ETNIA,
                                             @CREDO,
                                             @NECESSIDADEESPECIAL,
                                             @ENDERECO,
                                             @ENDERECONUMERO,
                                             @ENDERECOCOMPLEMENTO,
                                             @ENDERECOBAIRRO,
                                             @ENDERECOMUNICIPIO,
                                             @ENDERECOPAIS,
                                             @ENDERECOCEP,
                                             @LOCALIZACAOZONARESIDENCIA,
                                             @TELEFONE,
                                             @CELULAR,
                                             @EMAIL,
                                             @RGTIPO,
                                             @RGNUMERO,
                                             @RGEMISSOR,
                                             @RGUF,
                                             @RGDATAEXPEDICAO,
                                             @RGCOMPLEMENTO,
                                             @CPF,
                                             @TIPOCERTIDAO,
                                             @CERTIDAONUMERO,
                                             @CERTIDAOFOLHA,
                                             @CERTIDAOLIVRO,
                                             @CERTIDAODATAEMISSAO,
                                             @CERTIDAOCARTORIOUF,
                                             @CERTIDAOCARTORIOEXPEDICAO,
                                             @CERTIDAONUMEROMATRICULA,
                                             @CARTORIOID,
                                             @MODELOCERTIDAOCIVIL,
                                             @RESPONSAVEL,
                                             @RESPONSAVELNOME,
                                             @RESPONSAVELCPF, 
                                             @REDEENSINOORIGEM,
                                             @TIPOBOLSAPARTICULAR,
                                             @ALUNOID,
                                             @DATAALTERACAO,
                                             @IP,
                                             @DATACADASTRO,
                                             @PAICPF,
                                             @MAECPF,
                                             @PAIFALECIDO,
                                             @MAEFALECIDA,
                                             @PAITELEFONE,
                                             @MAETELEFONE,
                                             @RESPONSAVELTELEFONE
                                         )";

                    contextQuery.Parameters.Add("@NOMECOMPLETO", candidato.NomeCompleto);
                    contextQuery.Parameters.Add("@DATANASCIMENTO", candidato.DataNascimento);
                    contextQuery.Parameters.Add("@MUNICIPIONASCIMENTO", candidato.MunicipioNascimento);
                    contextQuery.Parameters.Add("@PAISNASCIMENTO", candidato.PaisNascimento);
                    contextQuery.Parameters.Add("@NACIONALIDADE", candidato.Nacionalidade);
                    contextQuery.Parameters.Add("@NOMEPAI", candidato.NomePai);
                    contextQuery.Parameters.Add("@NOMEMAE", candidato.NomeMãe);
                    contextQuery.Parameters.Add("@SEXO", candidato.Sexo);
                    contextQuery.Parameters.Add("@ESTADOCIVIL", candidato.EstadoCivil);
                    contextQuery.Parameters.Add("@ETNIA", candidato.Etnia);
                    contextQuery.Parameters.Add("@CREDO", candidato.Credo);
                    contextQuery.Parameters.Add("@NECESSIDADEESPECIAL", candidato.NecessidadeEspecial);
                    contextQuery.Parameters.Add("@ENDERECO", candidato.Endereco);
                    contextQuery.Parameters.Add("@ENDERECONUMERO", candidato.EnderecoNumero);
                    contextQuery.Parameters.Add("@ENDERECOCOMPLEMENTO", candidato.EnderecoCompleto);
                    contextQuery.Parameters.Add("@ENDERECOBAIRRO", candidato.EnderecoBairro);
                    contextQuery.Parameters.Add("@ENDERECOMUNICIPIO", candidato.EnderecoMunicipio);
                    contextQuery.Parameters.Add("@ENDERECOPAIS", "1");
                    contextQuery.Parameters.Add("@ENDERECOCEP", candidato.EnderecoCep);
                    contextQuery.Parameters.Add("@LOCALIZACAOZONARESIDENCIA", candidato.LocalizacaoZonaResidencia);
                    contextQuery.Parameters.Add("@TELEFONE", candidato.Telefone);
                    contextQuery.Parameters.Add("@CELULAR", candidato.Celular);
                    contextQuery.Parameters.Add("@EMAIL", candidato.Email);
                    contextQuery.Parameters.Add("@RGTIPO", candidato.RGTipo);
                    contextQuery.Parameters.Add("@RGNUMERO", candidato.RGNumero);
                    contextQuery.Parameters.Add("@RGEMISSOR", candidato.RGEmissor);
                    contextQuery.Parameters.Add("@RGUF", candidato.RGUF);
                    contextQuery.Parameters.Add("@RGDATAEXPEDICAO", candidato.RGDataExpedida);
                    contextQuery.Parameters.Add("@RGCOMPLEMENTO", candidato.RGComplemento);
                    contextQuery.Parameters.Add("@CPF", candidato.CPF);
                    contextQuery.Parameters.Add("@TIPOCERTIDAO", candidato.TipoCertidao);
                    contextQuery.Parameters.Add("@CERTIDAONUMERO", candidato.CertidaoNumero);
                    contextQuery.Parameters.Add("@CERTIDAOFOLHA", candidato.CertidaoFolha);
                    contextQuery.Parameters.Add("@CERTIDAOLIVRO", candidato.CertidaoLivro);
                    contextQuery.Parameters.Add("@CERTIDAODATAEMISSAO", candidato.CertidaoDataEmissao);
                    contextQuery.Parameters.Add("@CERTIDAOCARTORIOUF", candidato.CertidaoCartorioUF);
                    contextQuery.Parameters.Add("@CERTIDAOCARTORIOEXPEDICAO", candidato.CertidaoCartorioExpedicao);
                    contextQuery.Parameters.Add("@CERTIDAONUMEROMATRICULA", candidato.CertidaoNumeroMatricula);
                    contextQuery.Parameters.Add("@CARTORIOID", candidato.CartorioID);
                    contextQuery.Parameters.Add("@MODELOCERTIDAOCIVIL", !string.IsNullOrEmpty(candidato.ModeloCertidaoCivil)
                                               ? candidato.ModeloCertidaoCivil
                                               : null);
                    contextQuery.Parameters.Add("@RESPONSAVEL", candidato.Responsavel);
                    contextQuery.Parameters.Add("@RESPONSAVELNOME", candidato.ResponsavelNome);
                    contextQuery.Parameters.Add("@RESPONSAVELCPF", candidato.ResponsavelCPF);
                    contextQuery.Parameters.Add("@REDEENSINOORIGEM", candidato.RedeEnsino);
                    contextQuery.Parameters.Add("@TIPOBOLSAPARTICULAR", candidato.TipoBolsaParticular);
                    contextQuery.Parameters.Add("@ALUNOID", candidato.AlunoID);
                    contextQuery.Parameters.Add("@DATAALTERACAO", candidato.DataAlteracao);
                    contextQuery.Parameters.Add("@IP", candidato.IP);
                    contextQuery.Parameters.Add("@DATACADASTRO", candidato.DataCadastro);
                    contextQuery.Parameters.Add("@PAICPF", candidato.PaiCPF);
                    contextQuery.Parameters.Add("@MAECPF", candidato.MaeCPF);
                    contextQuery.Parameters.Add("@PAIFALECIDO", candidato.PaiFalecido);
                    contextQuery.Parameters.Add("@MAEFALECIDA", candidato.MaeFalecida);
                    contextQuery.Parameters.Add("@PAITELEFONE", candidato.PaiTelefone);
                    contextQuery.Parameters.Add("@MAETELEFONE", candidato.MaeTelefone);
                    contextQuery.Parameters.Add("@RESPONSAVELTELEFONE", candidato.ResponsavelTelefone);

                    ctx.ApplyModifications(contextQuery);

                    int candidatoId = BuscaCandidadoID(candidato, ctx);

                    //Salva dados de Recursos Necessários para Aplicação de Prova
                    RN.ProcessoSeletivoAluno.RecursoAplicacaoProvaCandidato.SalvaRecursoAplicacaoProva(listRecursoAplicacaoProvaCandidato, candidatoId, ctx);

                    //Salva dados de Inscricao
                    inscricaoId = RN.ProcessoSeletivoAluno.Inscricao.SalvaInscricao(inscricao, agendaId, candidatoId, unidadeEnsinoCursoTurnoInscricao.UnidadeEnsinoId, out numeroInscricao, ctx);

                    //Salva dados Unidade de Ensino Curso Turno
                    RN.ProcessoSeletivoAluno.UnidadeEnsinoCursoTurnoInscricao.SalvaUnidadeEnsinoCursoTurnoInscricao(unidadeEnsinoCursoTurnoInscricao, inscricaoId, ctx);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static void AlteraDadosCandidato(RN.ProcessoSeletivoAluno.Entidades.Candidato candidato, List<RN.ProcessoSeletivoAluno.Entidades.RecursoAplicacaoProvaCandidato> listRecursoAplicacaoProvaCandidato, RN.ProcessoSeletivoAluno.Entidades.Inscricao inscricao, RN.ProcessoSeletivoAluno.Entidades.UnidadeEnsinoCursoTurnoInscricao unidadeEnsinoCursoTurnoInscricao, int agendaId, int candidatoId, ref int inscricaoId, ref Int64 numeroInscricao)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                using (DataContext ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    contextQuery.Command = @"UPDATE [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATO]
                                                SET [NOMECOMPLETO] = @NOMECOMPLETO
                                                   ,[DATANASCIMENTO] = @DATANASCIMENTO
                                                   ,[MUNICIPIONASCIMENTO] = @MUNICIPIONASCIMENTO
                                                   ,[PAISNASCIMENTO] = @PAISNASCIMENTO
                                                   ,[NACIONALIDADE] = @NACIONALIDADE
                                                   ,[NOMEPAI] = @NOMEPAI
                                                   ,[NOMEMAE] = @NOMEMAE
                                                   ,[SEXO] = @SEXO
                                                   ,[ESTADOCIVIL] = @ESTADOCIVIL
                                                   ,[ETNIA] = @ETNIA
                                                   ,[CREDO] = @CREDO
                                                   ,[NECESSIDADEESPECIAL] = @NECESSIDADEESPECIAL
                                                   ,[ENDERECO] = @ENDERECO
                                                   ,[ENDERECONUMERO] = @ENDERECONUMERO
                                                   ,[ENDERECOCOMPLEMENTO] = @ENDERECOCOMPLEMENTO
                                                   ,[ENDERECOBAIRRO] = @ENDERECOBAIRRO
                                                   ,[ENDERECOMUNICIPIO] = @ENDERECOMUNICIPIO
                                                   ,[ENDERECOPAIS] = @ENDERECOPAIS
                                                   ,[ENDERECOCEP] = @ENDERECOCEP
                                                   ,[LOCALIZACAOZONARESIDENCIA] = @LOCALIZACAOZONARESIDENCIA
                                                   ,[TELEFONE] = @TELEFONE
                                                   ,[CELULAR] = @CELULAR
                                                   ,[EMAIL] = @EMAIL
                                                   ,[RGTIPO] = @RGTIPO
                                                   ,[RGNUMERO] = @RGNUMERO
                                                   ,[RGEMISSOR] = @RGEMISSOR
                                                   ,[RGUF] = @RGUF
                                                   ,[RGDATAEXPEDICAO] = @RGDATAEXPEDICAO
                                                   ,[RGCOMPLEMENTO] = @RGCOMPLEMENTO
                                                   ,[CPF] = @CPF
                                                   ,[TIPOCERTIDAO] = @TIPOCERTIDAO
                                                   ,[CERTIDAONUMERO] = @CERTIDAONUMERO
                                                   ,[CERTIDAOFOLHA] = @CERTIDAOFOLHA
                                                   ,[CERTIDAOLIVRO] = @CERTIDAOLIVRO
                                                   ,[CERTIDAODATAEMISSAO] = @CERTIDAODATAEMISSAO
                                                   ,[CERTIDAOCARTORIOUF] = @CERTIDAOCARTORIOUF
                                                   ,[CERTIDAOCARTORIOEXPEDICAO] = @CERTIDAOCARTORIOEXPEDICAO
                                                   ,[CERTIDAONUMEROMATRICULA] = @CERTIDAONUMEROMATRICULA
                                                   ,[CARTORIOID] = @CARTORIOID
                                                   ,[MODELOCERTIDAOCIVIL] = @MODELOCERTIDAOCIVIL
                                                   ,[RESPONSAVEL] = @RESPONSAVEL
                                                   ,[RESPONSAVELNOME] = @RESPONSAVELNOME
                                                   ,[RESPONSAVELCPF] = @RESPONSAVELCPF
                                                   ,[REDEENSINOORIGEM] = @REDEENSINOORIGEM
                                                   ,[TIPOBOLSAPARTICULAR] = @TIPOBOLSAPARTICULAR
                                                   ,[ALUNOID] = @ALUNOID
                                                   ,[DATAALTERACAO] = @DATAALTERACAO
                                                   ,[IP] = @IP
                                                   ,[PAICPF] = @PAICPF
                                                   ,[MAECPF] = @MAECPF
                                                   ,[PAIFALECIDO] = @PAIFALECIDO
                                                   ,[MAEFALECIDA] = @MAEFALECIDA
                                                   ,[PAITELEFONE] = @PAITELEFONE
                                                   ,[MAETELEFONE] = @MAETELEFONE
                                                   ,[RESPONSAVELTELEFONE] = @RESPONSAVELTELEFONE
                                             WHERE CANDIDATOID = @CANDIDATOID ";

                    contextQuery.Parameters.Add("@NOMECOMPLETO", candidato.NomeCompleto);
                    contextQuery.Parameters.Add("@DATANASCIMENTO", candidato.DataNascimento);
                    contextQuery.Parameters.Add("@MUNICIPIONASCIMENTO", candidato.MunicipioNascimento);
                    contextQuery.Parameters.Add("@PAISNASCIMENTO", candidato.PaisNascimento);
                    contextQuery.Parameters.Add("@NACIONALIDADE", candidato.Nacionalidade);
                    contextQuery.Parameters.Add("@NOMEPAI", candidato.NomePai);
                    contextQuery.Parameters.Add("@NOMEMAE", candidato.NomeMãe);
                    contextQuery.Parameters.Add("@SEXO", candidato.Sexo);
                    contextQuery.Parameters.Add("@ESTADOCIVIL", candidato.EstadoCivil);
                    contextQuery.Parameters.Add("@ETNIA", candidato.Etnia);
                    contextQuery.Parameters.Add("@CREDO", candidato.Credo);
                    contextQuery.Parameters.Add("@NECESSIDADEESPECIAL", candidato.NecessidadeEspecial);
                    contextQuery.Parameters.Add("@ENDERECO", candidato.Endereco);
                    contextQuery.Parameters.Add("@ENDERECONUMERO", candidato.EnderecoNumero);
                    contextQuery.Parameters.Add("@ENDERECOCOMPLEMENTO", candidato.EnderecoCompleto);
                    contextQuery.Parameters.Add("@ENDERECOBAIRRO", candidato.EnderecoBairro);
                    contextQuery.Parameters.Add("@ENDERECOMUNICIPIO", candidato.EnderecoMunicipio);
                    contextQuery.Parameters.Add("@ENDERECOPAIS", "1");
                    contextQuery.Parameters.Add("@ENDERECOCEP", candidato.EnderecoCep);
                    contextQuery.Parameters.Add("@LOCALIZACAOZONARESIDENCIA", candidato.LocalizacaoZonaResidencia);
                    contextQuery.Parameters.Add("@TELEFONE", candidato.Telefone);
                    contextQuery.Parameters.Add("@CELULAR", candidato.Celular);
                    contextQuery.Parameters.Add("@EMAIL", candidato.Email);
                    contextQuery.Parameters.Add("@RGTIPO", candidato.RGTipo);
                    contextQuery.Parameters.Add("@RGNUMERO", candidato.RGNumero);
                    contextQuery.Parameters.Add("@RGEMISSOR", candidato.RGEmissor);
                    contextQuery.Parameters.Add("@RGUF", candidato.RGUF);
                    contextQuery.Parameters.Add("@RGDATAEXPEDICAO", candidato.RGDataExpedida);
                    contextQuery.Parameters.Add("@RGCOMPLEMENTO", candidato.RGComplemento);
                    contextQuery.Parameters.Add("@CPF", candidato.CPF);
                    contextQuery.Parameters.Add("@TIPOCERTIDAO", candidato.TipoCertidao);
                    contextQuery.Parameters.Add("@CERTIDAONUMERO", candidato.CertidaoNumero);
                    contextQuery.Parameters.Add("@CERTIDAOFOLHA", candidato.CertidaoFolha);
                    contextQuery.Parameters.Add("@CERTIDAOLIVRO", candidato.CertidaoLivro);
                    contextQuery.Parameters.Add("@CERTIDAODATAEMISSAO", candidato.CertidaoDataEmissao);
                    contextQuery.Parameters.Add("@CERTIDAOCARTORIOUF", candidato.CertidaoCartorioUF);
                    contextQuery.Parameters.Add("@CERTIDAOCARTORIOEXPEDICAO", candidato.CertidaoCartorioExpedicao);
                    contextQuery.Parameters.Add("@CERTIDAONUMEROMATRICULA", candidato.CertidaoNumeroMatricula);
                    contextQuery.Parameters.Add("@CARTORIOID", candidato.CartorioID);
                    contextQuery.Parameters.Add("@MODELOCERTIDAOCIVIL", !string.IsNullOrEmpty(candidato.ModeloCertidaoCivil)
                                               ? candidato.ModeloCertidaoCivil
                                               : null);
                    contextQuery.Parameters.Add("@RESPONSAVEL", candidato.Responsavel);
                    contextQuery.Parameters.Add("@RESPONSAVELNOME", candidato.ResponsavelNome);
                    contextQuery.Parameters.Add("@RESPONSAVELCPF", candidato.ResponsavelCPF);
                    contextQuery.Parameters.Add("@REDEENSINOORIGEM", candidato.RedeEnsino);
                    contextQuery.Parameters.Add("@TIPOBOLSAPARTICULAR", candidato.TipoBolsaParticular);
                    contextQuery.Parameters.Add("@ALUNOID", candidato.AlunoID);
                    contextQuery.Parameters.Add("@DATAALTERACAO", candidato.DataAlteracao);
                    contextQuery.Parameters.Add("@IP", candidato.IP);
                    contextQuery.Parameters.Add("@PAICPF", candidato.PaiCPF);
                    contextQuery.Parameters.Add("@MAECPF", candidato.MaeCPF);
                    contextQuery.Parameters.Add("@PAIFALECIDO", candidato.PaiFalecido);
                    contextQuery.Parameters.Add("@MAEFALECIDA", candidato.MaeFalecida);
                    contextQuery.Parameters.Add("@PAITELEFONE", candidato.PaiTelefone);
                    contextQuery.Parameters.Add("@MAETELEFONE", candidato.MaeTelefone);
                    contextQuery.Parameters.Add("@RESPONSAVELTELEFONE", candidato.ResponsavelTelefone);
                    contextQuery.Parameters.Add("@CANDIDATOID", candidatoId);

                    ctx.ApplyModifications(contextQuery);

                    //Salva dados de Recursos Necessários para Aplicação de Prova
                    RN.ProcessoSeletivoAluno.RecursoAplicacaoProvaCandidato.AlteraRecursoAplicacaoProva(listRecursoAplicacaoProvaCandidato, candidatoId, ctx);

                    //Verifica se o candidato já tem uma inscrição neste processo seletivo
                    if (inscricaoId != int.MinValue)
                    {
                        //Altera dados de Inscricao
                        RN.ProcessoSeletivoAluno.Inscricao.AlteraInscricao(inscricao, inscricaoId, ctx);
                        numeroInscricao = RN.ProcessoSeletivoAluno.Inscricao.BuscaNumeroInscricaoPorInscricaoId(inscricaoId, ctx);

                        //Altera dados Unidade de Ensino Curso Turno
                        RN.ProcessoSeletivoAluno.UnidadeEnsinoCursoTurnoInscricao.AlteraUnidadeEnsinoCursoTurnoInscricao(unidadeEnsinoCursoTurnoInscricao, inscricaoId, ctx);
                    }
                    else
                    {
                        //Salva dados de Inscrição
                        inscricaoId = RN.ProcessoSeletivoAluno.Inscricao.SalvaInscricao(inscricao, agendaId, candidatoId, unidadeEnsinoCursoTurnoInscricao.UnidadeEnsinoId, out numeroInscricao, ctx);

                        //Salva dados Unidade de Ensino Curso Turno
                        RN.ProcessoSeletivoAluno.UnidadeEnsinoCursoTurnoInscricao.SalvaUnidadeEnsinoCursoTurnoInscricao(unidadeEnsinoCursoTurnoInscricao, inscricaoId, ctx);
                    }
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }

        public static DataTable VerificaCandidatoExistente(string nomeCompleto, string nomeMae, DateTime dataNascimento, int agendaId)
        {
            DataTable dadosCandidato = null;

            try
            {
                using (DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"SELECT C.[CANDIDATOID], I.[INSCRICAOID], I.[NUMEROINSCRICAO], ps.AGENDAID
                                               FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[CANDIDATO] C
                                               LEFT JOIN ([LYCEUM].[PROCESSOSELETIVOALUNO].[INSCRICAO] I
                                              INNER JOIN [LYCEUM].[AGENDA].[PROCESSOSELETIVO] PS
	                                             ON I.[PROCESSOSELETIVOID] = PS.[PROCESSOSELETIVOID] and (PS.[AGENDAID] = @AGENDAID OR PS.[AGENDAID] IS NULL))
	                                             ON C.[CANDIDATOID] = I.[CANDIDATOID]
                                              WHERE C.[NOMECOMPLETO] = @NOMECOMPLETO
                                                AND C.[NOMEMAE] = @NOMEMAE
                                                AND C.[DATANASCIMENTO] = @DATANASCIMENTO ";

                    contextQuery.Parameters.Add("@NOMECOMPLETO", nomeCompleto);
                    contextQuery.Parameters.Add("@NOMEMAE ", nomeMae);
                    contextQuery.Parameters.Add("@DATANASCIMENTO ", dataNascimento);
                    contextQuery.Parameters.Add("@AGENDAID ", agendaId);

                    dadosCandidato = ctx.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return dadosCandidato;
        }

        public static DataTable VerificaCandidatoExistentePorNumeroInscricao(Int64 numeroInscricao, string nomeMae, DateTime dataNascimento, int agendaId)
        {
            DataTable dadosCandidato = null;

            try
            {
                using (DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"SELECT C.[CANDIDATOID], I.[INSCRICAOID]
                                               FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[INSCRICAO] I
                                              INNER JOIN [LYCEUM].[PROCESSOSELETIVOALUNO].[CANDIDATO] C
                                                 ON I.CANDIDATOID = C.CANDIDATOID
                                              INNER JOIN [LYCEUM].[AGENDA].[PROCESSOSELETIVO] PS
                                                 ON I.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                              WHERE I.[NUMEROINSCRICAO] = @NUMEROINSCRICAO
                                                AND C.[NOMEMAE] = @NOMEMAE
                                                AND C.[DATANASCIMENTO] = @DATANASCIMENTO
                                                AND PS.[AGENDAID] = @AGENDAID ";

                    contextQuery.Parameters.Add("@NUMEROINSCRICAO ", numeroInscricao);
                    contextQuery.Parameters.Add("@NOMEMAE ", nomeMae);
                    contextQuery.Parameters.Add("@DATANASCIMENTO ", dataNascimento);
                    contextQuery.Parameters.Add("@AGENDAID ", agendaId);

                    dadosCandidato = ctx.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return dadosCandidato;
        }

        public static bool VerificaDadosCandidatoJaExistente(int candidatoId, string nomeCompleto, string nomeMae, DateTime dataNascimento)
        {
            bool candidatoExistente = false;

            try
            {
                using (DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @"SELECT 1
                                               FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[CANDIDATO] C
                                              WHERE C.[NOMECOMPLETO] = @NOMECOMPLETO
                                                AND C.[NOMEMAE] = @NOMEMAE
                                                AND C.[DATANASCIMENTO] = @DATANASCIMENTO ";

                    if (candidatoId != int.MinValue)
                    {
                        contextQuery.Command += (" AND C.[CANDIDATOID] <> @CANDIDATOID");
                        contextQuery.Parameters.Add("@CANDIDATOID ", candidatoId);
                    }

                    contextQuery.Parameters.Add("@NOMECOMPLETO", nomeCompleto);
                    contextQuery.Parameters.Add("@NOMEMAE ", nomeMae);
                    contextQuery.Parameters.Add("@DATANASCIMENTO ", dataNascimento);

                    object retorno = ctx.GetReturnValue(contextQuery);

                    if (retorno != null)
                        candidatoExistente = (retorno.ToString() == "1");
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return candidatoExistente;
        }
    }
}
