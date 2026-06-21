using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Cadastros.DTOs;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Cadastros
{
    public class MaeInscricao
    {
        public DataTable ListaInscricaoParaAnalisePor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT distinct
                                               I.MAE_INSCRICAOID,
	                                           I.CPF,
	                                           I.NOME,
	                                           I.DATANASCIMENTO,
	                                           I.EMAIL,
	                                           I.CELULAR,
	                                           I.FIXOCELULAR,
	                                           CASE 
			                                        WHEN I.EXPERIENCIATRABALHO = 1 THEN 'Sim'
			                                        ELSE 'Não'
		                                        END EXPERIENCIATRABALHO,
		                                         CASE 
			                                        WHEN I.HABILITADO IS NULL THEN 'Não Analisado'
			                                        WHEN I.HABILITADO = 1 THEN 'Sim'
			                                        ELSE 'Não'
		                                        END HABILITADO,
	                                           MO.DESCRICAO AS MOTIVO
                                        FROM Cadastros.MAE_INSCRICAO I (NOLOCK)
	                                        INNER JOIN Cadastros.MAE_INSCRICAOALUNO IA (nolock) on I.MAE_INSCRICAOID = Ia.MAE_INSCRICAOID
	                                        INNER JOIN LY_ALUNO A (nolock) on IA.ALUNO = A.ALUNO
	                                        LEFT JOIN Cadastros.MAE_MOTIVONAOHABILITADO MO (NOLOCK) ON I.MAE_MOTIVONAOHABILITADOID = MO.MAE_MOTIVONAOHABILITADOID
                                        WHERE A.UNIDADE_ENSINO = @CENSO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public DataTable ListaInscricaoPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"
                select distinct
                I.MAE_INSCRICAOID
                , I.NOME
                , I.CPF
                , I.NUMERORG
                , A.UNIDADE_ENSINO
                , ARQ.MAE_FORMULARIOBANCOARQUIVOID
                , ARQ.ARQUIVO
                , ARQ.CHAVEARQUIVO
                , ARQ.NOMEARQUIVO
                , ARQ.TIPOARQUIVO

                FROM Cadastros.MAE_INSCRICAO I (NOLOCK)
                INNER JOIN Cadastros.MAE_INSCRICAOALUNO IA (nolock) on I.MAE_INSCRICAOID = IA.MAE_INSCRICAOID
                INNER JOIN LY_ALUNO A (nolock) on IA.ALUNO = A.ALUNO
                LEFT JOIN Cadastros.MAE_FORMULARIOBANCOARQUIVO ARQ (nolock) on I.MAE_INSCRICAOID = ARQ.MAE_INSCRICAOID
                
                WHERE A.UNIDADE_ENSINO = @CENSO 
                ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public bool PossuiMotivoNaoHabilitadoPor(DataContext contexto, int maeMotivoNaoHabilitadoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM Cadastros.MAE_INSCRICAO (NOLOCK)
                                        WHERE MAE_MOTIVONAOHABILITADOID = @MAE_MOTIVONAOHABILITADOID ";

            contextQuery.Parameters.Add("@MAE_MOTIVONAOHABILITADOID", SqlDbType.Int, maeMotivoNaoHabilitadoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public MaeDadosInscricao ObtemDadosInscricaoPor(string cpf)
        {
            MaeDadosInscricao dados = new MaeDadosInscricao();
            MaeLotacao rnMaeLotacao = new MaeLotacao();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  I.MAE_INSCRICAOID AS MAEINSCRICAOID ,
									CPF ,
									I.NOME ,
									DATANASCIMENTO ,
									SEXO ,
									EMAIL ,
									CELULAR ,
									FIXOCELULAR ,
									NUMERORG ,
									ORGAORG ,
									UFRG ,
									E.MAE_ESCOLARIDADEID AS ESCOLARIDADEID ,
									E.DESCRICAO AS ESCOLARIDADEDESCRICAO ,
									I.CEP ,
									ENDERECO ,
									NUMEROENDERECO AS NUMERO ,
									COMPLEMENTOENDERECO AS COMPLEMENTO ,
									BAIRROID ,
									B.DESCRICAO AS BAIRRODESCRICAO,
									MUNICIPIOENDERECO AS MUNICIPIOID ,
									M.NOME AS MUNICIPIODESCRICAO,
									M.UF_SIGLA AS UF,
									TOMOUVACINA ,
									DOSEUNICA ,
									DATAVACINA1 ,
									DATAVACINA2 ,
									DESEMPREGADO ,
									SEGURODESEMPREGO   ,
									RENDASUPERIOR ,
									CARGAHORARIALIVRE ,
									EXPERIENCIATRABALHO ,
									AR.MAE_INSCRICAOARQUIVOID,
									AR.TIPOARQUIVO,
									I.HABILITADO,
									I.MAE_MOTIVONAOHABILITADOID AS MAEMOTIVONAOHABILITADOID,
									MO.DESCRICAO AS MAEMOTIVONAOHABILITADODESCRICAO,
                                    MANHA,
                                    TARDE
							FROM CADASTROS.MAE_INSCRICAO I
									INNER JOIN CADASTROS.MAE_ESCOLARIDADE E ON I.MAE_ESCOLARIDADEID = E.MAE_ESCOLARIDADEID
									LEFT JOIN CADASTROS.MAE_INSCRICAOARQUIVO AR ON I.MAE_INSCRICAOID = AR.MAE_INSCRICAOID
									INNER JOIN MUNICIPIO M ON I.MUNICIPIOENDERECO = M.CODIGO
									INNER JOIN HADES.DBO.BAIRRO B ON I.BAIRROID = B.CODIGO
									LEFT JOIN CADASTROS.MAE_MOTIVONAOHABILITADO MO ON I.MAE_MOTIVONAOHABILITADOID = MO.MAE_MOTIVONAOHABILITADOID
							WHERE CPF = @CPF ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.MaeInscricaoId = Convert.ToInt32(reader["MAEINSCRICAOID"]);
                    dados.CPF = Convert.ToString(reader["CPF"]);
                    dados.Nome = Convert.ToString(reader["NOME"]);
                    dados.DataNascimento = Convert.ToDateTime(reader["DATANASCIMENTO"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);
                    dados.Email = Convert.ToString(reader["EMAIL"]);
                    dados.Celular = Convert.ToString(reader["CELULAR"]);
                    dados.FixoCelular = Convert.ToString(reader["FIXOCELULAR"]);
                    dados.NumeroRG = Convert.ToString(reader["NUMERORG"]);
                    dados.OrgaoRG = Convert.ToString(reader["ORGAORG"]);
                    dados.UfRG = Convert.ToString(reader["UFRG"]);
                    dados.EscolaridadeId = Convert.ToInt32(reader["ESCOLARIDADEID"]);
                    dados.EscolaridadeDescricao = Convert.ToString(reader["ESCOLARIDADEDESCRICAO"]);
                    dados.CEP = Convert.ToString(reader["CEP"]);
                    dados.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dados.Numero = Convert.ToString(reader["NUMERO"]);
                    dados.Complemento = Convert.ToString(reader["COMPLEMENTO"]);
                    dados.BairroId = Convert.ToInt32(reader["BAIRROID"]);
                    dados.BairroDescricao = Convert.ToString(reader["BAIRRODESCRICAO"]);
                    dados.MunicipioId = Convert.ToString(reader["MUNICIPIOID"]);
                    dados.MunicipioDescricao = Convert.ToString(reader["MUNICIPIODESCRICAO"]);
                    dados.UF = Convert.ToString(reader["UF"]);
                    dados.TomouVacina = Convert.ToBoolean(reader["TOMOUVACINA"]);
                    dados.DoseUnica = Convert.ToBoolean(reader["DOSEUNICA"]);
                    dados.DataVacina1 = Convert.ToDateTime(reader["DATAVACINA1"]);

                    if (reader["DATAVACINA2"] != DBNull.Value)
                    {
                        dados.DataVacina2 = Convert.ToDateTime(reader["DATAVACINA2"]);
                    }

                    dados.Desempregado = Convert.ToBoolean(reader["DESEMPREGADO"]);
                    dados.SeguroDesemprego = Convert.ToBoolean(reader["SEGURODESEMPREGO"]);
                    dados.RendaSuperior = Convert.ToBoolean(reader["RENDASUPERIOR"]);
                    dados.CargaHorariaLivre = Convert.ToBoolean(reader["CARGAHORARIALIVRE"]);
                    dados.ExperienciaTrabalho = Convert.ToBoolean(reader["EXPERIENCIATRABALHO"]);
                    //dados.MaeInscricaoArquivoId = Convert.ToInt32(reader["MAE_INSCRICAOARQUIVOID"]);
                    //dados.TipoArquivoCurriculo = Convert.ToString(reader["TIPOARQUIVO"]);

                    if (reader["HABILITADO"] != DBNull.Value)
                    {
                        dados.Habilitado = Convert.ToBoolean(reader["HABILITADO"]);

                        if (reader["MAEMOTIVONAOHABILITADOID"] != DBNull.Value)
                        {
                            dados.MaeMotivoNaoHabilitadoId = Convert.ToInt32(reader["MAEMOTIVONAOHABILITADOID"]);
                            dados.MaeMotivoNaoHabilitadoDescricao = Convert.ToString(reader["MAEMOTIVONAOHABILITADODESCRICAO"]);
                        }
                    }
                    else
                    {
                        dados.Habilitado = null;
                        dados.MaeMotivoNaoHabilitadoId = null;
                        dados.MaeMotivoNaoHabilitadoDescricao = null;
                    }
                    dados.Manha = Convert.ToBoolean(reader["MANHA"]);
                    dados.Tarde = Convert.ToBoolean(reader["TARDE"]);
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

        public bool EhHabilitadoPor(DataContext contexto, int maeInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM Cadastros.MAE_INSCRICAO (NOLOCK)
                                        WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID
                                              AND HABILITADO = 1 ";

            contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeInscricaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados ValidaAnalise(MaeDadosAnalise dados)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            MaeLotacao rnMaeLotacao = new MaeLotacao();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dados.MaeInscricaoId <= 0)
            {
                mensagens.Add("Campo INSCRIÇÃO é obrigatório.");
            }

            //Verifica se foi habilitado
            if (dados.Habilitado)
            {
                dados.MaeMotivoNaoHabilitadoId = null;

                if (dados.DataInicio == DateTime.MinValue)
                {
                    mensagens.Add("Campo DATA INÍCIO é obrigatório.");
                }

                if (dados.Censo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo ESCOLA é obrigatório.");
                }
            }
            else
            {
                dados.DataInicio = null;
                dados.Censo = null;

                if (dados.MaeMotivoNaoHabilitadoId <= 0)
                {
                    mensagens.Add("Campo MOTIVO é obrigatório.");
                }
            }

            if (dados.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi analisado
                    if (this.EhHabilitadoPor(contexto, dados.MaeInscricaoId))
                    {
                        mensagens.Add("Esta analise não pode ser alterada pois já foi habilitada.");
                    }

                    //Verifica se tem lotacao aberta em outro censo
                    if (rnMaeLotacao.PossuiOutraLotacaoAbertaPor(contexto, dados.MaeInscricaoId, dados.Censo))
                    {
                        mensagens.Add("Esta analise não pode ser alterada pois o inscrito possui uma lotação aberta para outra escola.");
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

        public void Analise(MaeDadosAnalise dados)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            MaeLotacao rnMaeLotacao = new MaeLotacao();

            try
            {
                //Atualiza dados da analise 
                this.AtualizaDadosAnalise(contexto, dados);

                //Verifica se é hablitado para gerar lotacao
                if (dados.Habilitado)
                {
                    //Verifica se ja tem lotacao para a escola
                    if (rnMaeLotacao.PossuiLotacaoPor(contexto, dados.MaeInscricaoId, dados.Censo))
                    {
                        rnMaeLotacao.Atualiza(contexto, dados);
                    }
                    else
                    {
                        rnMaeLotacao.Insere(contexto, dados);
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

        private void AtualizaDadosAnalise(DataContext contexto, MaeDadosAnalise dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Cadastros.MAE_INSCRICAO
	                                    SET HABILITADO = @HABILITADO,
		                                    MAE_MOTIVONAOHABILITADOID = @MAE_MOTIVONAOHABILITADOID,
		                                    USUARIOID = @USUARIOID,
		                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID ";

            contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, dados.MaeInscricaoId);
            contextQuery.Parameters.Add("@HABILITADO", SqlDbType.Int, dados.Habilitado);
            contextQuery.Parameters.Add("@MAE_MOTIVONAOHABILITADOID", SqlDbType.Int, dados.MaeMotivoNaoHabilitadoId == null || dados.MaeMotivoNaoHabilitadoId <= 0 ? (object)DBNull.Value : dados.MaeMotivoNaoHabilitadoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public byte[] ObtemArquivoCurriculoPor(int maeInscricaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            byte[] arquivo = null;

            try
            {
                contextQuery.Command = @" SELECT ARQUIVO, 
	                                               TIPOARQUIVO, 
                                                   NOMEARQUIVO 
                                            FROM  Cadastros.MAE_INSCRICAOARQUIVO (NOLOCK) 
											WHERE MAE_INSCRICAOID = @MAE_INSCRICAOID ";

                contextQuery.Parameters.Add("@MAE_INSCRICAOID", SqlDbType.Int, maeInscricaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    arquivo = (byte[])reader["ARQUIVO"];
                }

                return arquivo;
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
    }
}
