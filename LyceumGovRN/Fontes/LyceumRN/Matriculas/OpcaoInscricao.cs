using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.Matriculas
{
    public class OpcaoInscricao
    {
        public DataTable ListaConvocadosPor(string censo, int ano, int periodo, int fase)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.Append(@" SELECT I.INSCRICAOALUNOID, 
                                               PC.PRECADASTROALUNOID,
                                               OP.OPCAOINSCRICAOID,
                                               OP.CONTROLEVAGAID,
                                               I.NUMEROINSCRICAO, 
                                               PC.NOME, 
                                               PC.DATANASCIMENTO, 
                                               PC.NOMEMAE, 
                                               PC.CPF, 
                                               CV.CURSO, 
                                               C.NOME AS NOMECURSO, 
                                               CV.TURNO, 
                                               CV.SERIE,
	                                           OP.DATACONVOCACAO,
	                                           OP.PRAZORESPOSTA,
                                               CV.CENSO
                                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                               INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                                       ON I.PRECADASTROALUNOID = PC.PRECADASTROALUNOID 
                                               INNER JOIN MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
                                                       ON I.INSCRICAOALUNOID = OP.INSCRICAOALUNOID 
                                               INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
                                               INNER JOIN LY_CURSO C (NOLOCK) 
                                                       ON C.CURSO = CV.CURSO 
                                        WHERE  I.ANO = @ANO 
                                               AND CV.PERIODO = @PERIODO 
                                               AND OP.DATACONVOCACAO IS NOT NULL 
                                               AND OP.DATACONVOCACAO <= GETDATE()
                                               AND CV.CENSO = @CENSO
                                               AND OP.FASE = @FASE 
                                               ");

                //Verifica se é a primeira fase
                if (fase == 1)
                {
                    //Para primeira fase apenas buscar os alocados
                    sql.Append(@" AND I.ALOCADOFASE1 = 1 ");
                }
                else
                {
                    //Para segunda fase verifica campo que indica vaga concorrente
                    sql.Append(@" AND OP.VAGACONCORRENTE = 1 --ETAPA SEM PROCESSO DE ALOCACAO ");
                }

                sql.Append(@" ORDER  BY I.NUMEROINSCRICAO ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Decimal, periodo);
                contextQuery.Parameters.Add("@FASE", SqlDbType.Int, fase);

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

        public List<DadosConvocados> ListaConvocadosPor(DataContext contexto, int ano, int fase)
        {
            List<DadosConvocados> lista = new List<DadosConvocados>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT I.INSCRICAOALUNOID, 
                                               OP.OPCAOINSCRICAOID,
											   PC.EMAIL,
											   UE.NOME_COMP AS ESCOLA,
											   UE.MUNICIPIO,
                                               PC.NOME, 
											   TC.DESCRICAO AS SEGMENTO,
                                               CV.SERIE,
	                                           OP.DATACONVOCACAO,
	                                           OP.DATARETORNO,
	                                           OP.PRAZORESPOSTA,
                                               CV.CENSO
                                        FROM   MATRICULA.INSCRICAOALUNO I 
                                               INNER JOIN MATRICULA.PRECADASTROALUNO PC 
                                                       ON I.PRECADASTROALUNOID = PC.PRECADASTROALUNOID 
                                               INNER JOIN MATRICULA.OPCAOINSCRICAO OP  
                                                       ON I.INSCRICAOALUNOID = OP.INSCRICAOALUNOID 
                                               INNER JOIN TCE_CONTROLE_VAGA CV 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
                                               INNER JOIN LY_CURSO C  
                                                       ON C.CURSO = CV.CURSO 
											   INNER JOIN LY_TIPO_CURSO TC 
													   ON TC.TIPO = C.TIPO
											   INNER JOIN LY_UNIDADE_ENSINO UE 
													   ON UE.UNIDADE_ENS = CV.CENSO
                                        WHERE  I.ANO = @ANO
											   AND OP.DATACONVOCACAO IS NOT NULL
											   AND OP.FASE = @FASE ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@FASE", SqlDbType.Int, fase);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosConvocados convocado = new DadosConvocados();

                    convocado.OpcaoInscricaoId = Convert.ToInt32(reader["OPCAOINSCRICAOID"]);
                    convocado.InscricaoAlunoId = Convert.ToInt32(reader["INSCRICAOALUNOID"]);
                    convocado.Email = Convert.ToString(reader["EMAIL"]);
                    convocado.Nome = Convert.ToString(reader["NOME"]);
                    convocado.Serie = Convert.ToInt32(reader["SERIE"]);
                    convocado.Segmento = Convert.ToString(reader["SEGMENTO"]);
                    convocado.Escola = Convert.ToString(reader["ESCOLA"]);
                    convocado.MunicipioEscola = Convert.ToString(reader["MUNICIPIO"]);
                    convocado.PrazoFinal = Convert.ToDateTime(reader["PRAZORESPOSTA"]);
                    convocado.DataConvocacao = Convert.ToDateTime(reader["DATACONVOCACAO"]);

                    if (reader["DATARETORNO"] != DBNull.Value)
                    {
                        convocado.DataRetorno = Convert.ToDateTime(reader["DATARETORNO"]);
                    }

                    lista.Add(convocado);
                }

                return lista;
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

        public DataTable ListaNaoConfirmadosPor(string censo, int ano, int periodo, int fase)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.Append(@" SELECT I.INSCRICAOALUNOID, 
                                               PC.PRECADASTROALUNOID,
                                               OP.OPCAOINSCRICAOID,
                                               OP.CONTROLEVAGAID,
                                               I.NUMEROINSCRICAO, 
                                               PC.NOME, 
                                               PC.DATANASCIMENTO, 
                                               PC.NOMEMAE, 
                                               PC.CPF, 
                                               CV.CURSO, 
                                               C.NOME AS NOMECURSO, 
                                               CV.TURNO, 
                                               CV.SERIE,
	                                           OP.DATACONVOCACAO,
	                                           OP.DATASITUACAO,
											   M.DESCRICAO AS MOTIVONAOCONFIRMACAO,
                                               CV.CENSO,
                                               NULL AS MOTIVORETORNOID
                                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                               INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                                       ON I.PRECADASTROALUNOID = PC.PRECADASTROALUNOID 
                                               INNER JOIN MATRICULA.OPCAOINSCRICAOHIST OP (NOLOCK) 
                                                       ON I.INSCRICAOALUNOID = OP.INSCRICAOALUNOID 
                                               INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
                                               INNER JOIN LY_CURSO C (NOLOCK) 
                                                       ON C.CURSO = CV.CURSO
											   INNER JOIN Matricula.MOTIVOREJEICAOINSCRICAO M (NOLOCK)
													   ON M.MOTIVOREJEICAOINSCRICAOID = OP.MOTIVOREJEICAOINSCRICAOID
                                        WHERE  I.ANO = @ANO 
                                               AND CV.PERIODO = @PERIODO 
                                               AND OP.DATACONVOCACAO IS NOT NULL 
                                               AND OP.CONFIRMADO = 0
											   AND (M.TIPO = 1 OR M.MOTIVOREJEICAOINSCRICAOID = 20)--Apenas motivos da tela de confirmação ou que cairam por fim do prazo
                                               AND CV.CENSO = @CENSO
                                               AND OP.FASE = @FASE 
                                               ");

                //Verifica se é a primeira fase
                if (fase == 1)
                {
                    //Para primeira fase apenas buscar os alocados
                    sql.Append(@" AND I.ALOCADOFASE1 = 1 ");
                }
                else
                {
                    //Para segunda fase verifica campo que indica vaga concorrente
                    sql.Append(@" AND OP.VAGACONCORRENTE = 1 --ETAPA SEM PROCESSO DE ALOCACAO ");
                }

                sql.Append(@" ORDER  BY I.NUMEROINSCRICAO ");

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Decimal, periodo);
                contextQuery.Parameters.Add("@FASE", SqlDbType.Int, fase);

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

        public DadosConfirmacaoCandidato ObtemDadosConfirmacaoProximoFilaPor(DataContext contexto, int ano, int periodo, string censo, string curso, int serie, string turno)
        {
            DadosConfirmacaoCandidato dados = new DadosConfirmacaoCandidato();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader dataReader = null;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 PC.PRECADASTROALUNOID, 
							   I.INSCRICAOALUNOID, 
							   I.NUMEROINSCRICAO,
                               I.IRMAOMATRICULA,
                               I.IRMAOINSCRICAO,
                               II.NUMEROINSCRICAO AS IRMAONUMEROINSCRICAO,
							   I.ALUNO,
							   O.OPCAOINSCRICAOID,
                               PESSOAID, 
                               PC.NOME, 
							   CV.CENSO,
							   UE.NOME_COMP AS ESCOLA,
							   CV.ANO,
							   CV.PERIODO,
							   MO.DESCRICAO AS MODALIDADE,
							   CU.MODALIDADE AS MODALIDADECODIGO,
							   CU.TIPO AS SEGMENTOCODIGO,
							   TC.DESCRICAO AS SEGMENTO,
							   CV.CURSO,
							   CU.NOME AS CURSODESCRICAO,
							   CV.SERIE,
							   CV.TURNO,
							   O.CONTROLEVAGAID,
                               PC.EMAIL,
                               PC.DATANASCIMENTO,
                               PC.CPF,
                               PC.NOMEMAE,
                               UE.MUNICIPIO,
                               PC.CELULAR
                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
							   INNER JOIN MATRICULA.OPCAOINSCRICAO O (NOLOCK)
									  ON I.INSCRICAOALUNOID = O.INSCRICAOALUNOID
                               LEFT JOIN MATRICULA.INSCRICAOALUNO II (NOLOCK)
                                      ON I.IRMAOINSCRICAO = II.INSCRICAOALUNOID
                               INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID
							   INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK)
									  ON O.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA	
							   INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
									ON CV.CENSO = UE.UNIDADE_ENS
							   INNER JOIN LY_CURSO CU (NOLOCK)
									ON CV.CURSO = CU.CURSO
							   INNER JOIN LY_MODALIDADE_CURSO MO (NOLOCK)
									ON CU.MODALIDADE = MO.MODALIDADE 
							   INNER JOIN LY_TIPO_CURSO TC (NOLOCK)
									ON TC.TIPO = CU.TIPO	
                        WHERE  CV.ANO = @ANO 
                               AND CV.PERIODO = @PERIODO 
                               AND CV.CURSO = @CURSO 
                               AND CV.CENSO = @CENSO 
                               AND CV.SERIE = @SERIE
                               AND TURNO =  @TURNO 
                               AND O.DATACONVOCACAO IS NULL 
                               AND O.TIPOFILAID IS NOT NULL  
							   AND O.VAGACONCORRENTE = 1 --ETAPA SEM PROCESSO DE ALOCACAO 
                        ORDER  BY O.DATACADASTRO, O.OPCAOINSCRICAOID  ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    dados.OpcaoInscricaoId = Convert.ToInt32(dataReader["OPCAOINSCRICAOID"]);
                    dados.InscricaoAlunoId = Convert.ToInt32(dataReader["INSCRICAOALUNOID"]);
                    dados.NumeroInscricao = Convert.ToInt32(dataReader["NUMEROINSCRICAO"]);
                    dados.IrmaoMatricula = dataReader["IRMAOMATRICULA"] == DBNull.Value ? (string)null : Convert.ToString(dataReader["IRMAOMATRICULA"]);
                    dados.IrmaoIdInscricao = dataReader["IRMAOINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["IRMAOINSCRICAO"]);
                    dados.IrmaoNumeroInscricao = dataReader["IRMAONUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["IRMAONUMEROINSCRICAO"]);
                    dados.Aluno = Convert.ToString(dataReader["ALUNO"]);
                    dados.PreCadastroAlunoId = Convert.ToInt32(dataReader["PRECADASTROALUNOID"]);
                    dados.Pessoa = dataReader["PESSOAID"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataReader["PESSOAID"]);
                    dados.Nome = Convert.ToString(dataReader["NOME"]);
                    dados.Telefone = Convert.ToString(dataReader["CELULAR"]);
                    dados.NomeMae = Convert.ToString(dataReader["NOMEMAE"]);
                    dados.DataNascimento = Convert.ToDateTime(dataReader["DATANASCIMENTO"]);
                    dados.Censo = Convert.ToString(dataReader["CENSO"]);
                    dados.Escola = Convert.ToString(dataReader["ESCOLA"]);
                    dados.Ano = Convert.ToInt32(dataReader["ANO"]);
                    dados.Periodo = Convert.ToInt32(dataReader["PERIODO"]);
                    dados.Modalidade = Convert.ToString(dataReader["MODALIDADE"]);
                    dados.Segmento = Convert.ToString(dataReader["SEGMENTO"]);
                    dados.Curso = Convert.ToString(dataReader["CURSO"]);
                    dados.CursoDescricao = Convert.ToString(dataReader["CURSODESCRICAO"]);
                    dados.Serie = Convert.ToInt32(dataReader["SERIE"]);
                    dados.Turno = Convert.ToString(dataReader["TURNO"]);
                    dados.ControleVagaId = Convert.ToInt32(dataReader["CONTROLEVAGAID"]);
                    dados.Email = Convert.ToString(dataReader["EMAIL"]);
                    dados.Cpf = Convert.ToString(dataReader["CPF"]);
                    dados.ModalidadeCodigo = Convert.ToString(dataReader["MODALIDADECODIGO"]);
                    dados.SegmentoCodigo = Convert.ToString(dataReader["SEGMENTOCODIGO"]);
                    dados.Municipio = Convert.ToString(dataReader["MUNICIPIO"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
            return dados;
        }

        public DadosConfirmacaoCandidato ObtemDadosConfirmacaoCandidatoPor(int opcaoInscricaoId)
        {
            DadosConfirmacaoCandidato dados = new DadosConfirmacaoCandidato();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            PreCadastroAlunoFoto rnPreCadastroAlunoFoto = new PreCadastroAlunoFoto();
            Entidades.PreCadastroAlunoFoto preCadastroAlunoFoto = new Techne.Lyceum.RN.Matriculas.Entidades.PreCadastroAlunoFoto();
            InscricaoAluno rnInscricaoAluno = new InscricaoAluno();

            try
            {
                dados = this.ObtemDadosConfirmacaoCandidatoPor(contexto, opcaoInscricaoId);

                //Verifica se a inscrição informou a matricula do irmao
                if (!dados.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
                {
                    //Busca dados do irmao
                    dados.DadosIrmao = rnInscricaoAluno.ObtemDadosIrmaoPor(contexto, dados.IrmaoMatricula);
                }
                else
                {
                    dados.IrmaoMatricula = null;

                    //Verifica se a inscrição informou a inscrição do irmao
                    if (dados.IrmaoIdInscricao != null && dados.IrmaoIdInscricao > 0)
                    {
                        //Busca dados do irmao
                        dados.DadosIrmao = rnInscricaoAluno.ObtemDadosIrmaoPor(contexto, Convert.ToInt32(dados.IrmaoIdInscricao));
                    }
                    else
                    {
                        dados.IrmaoIdInscricao = null;
                    }
                }

                //Busca dados da foto
                preCadastroAlunoFoto = rnPreCadastroAlunoFoto.ObtemPor(contexto, dados.PreCadastroAlunoId);
                dados.FotoArquivo = preCadastroAlunoFoto.Arquivo;
                dados.FotoTipoArquivo = preCadastroAlunoFoto.TipoArquivo;
                dados.FotoNomeArquivo = preCadastroAlunoFoto.NomeArquivo;
                if (dados.FotoArquivo != null && dados.FotoArquivo.Length > 0)
                {
                    dados.FotoSrc = string.Format("data:{0};name:{1};base64,{2}", dados.FotoTipoArquivo, dados.FotoNomeArquivo, Convert.ToBase64String(dados.FotoArquivo));
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

        private DadosConfirmacaoCandidato ObtemDadosConfirmacaoCandidatoPor(DataContext contexto, int opcaoInscricaoId)
        {
            DadosConfirmacaoCandidato dados = new DadosConfirmacaoCandidato();
            SqlDataReader dataReader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PC.PRECADASTROALUNOID, 
							   I.INSCRICAOALUNOID, 
							   I.NUMEROINSCRICAO,
                               I.IRMAOMATRICULA,
                               I.IRMAOINSCRICAO,
                               II.NUMEROINSCRICAO AS IRMAONUMEROINSCRICAO,
                               I.ALUNO,
							   O.OPCAOINSCRICAOID,
							   O.FASE,
                               PESSOAID, 
                               PC.NOME, 
							   CV.CENSO,
							   UE.NOME_COMP AS ESCOLA,
							   CV.ANO,
							   CV.PERIODO,
							   MO.DESCRICAO AS MODALIDADE,
							   CU.MODALIDADE AS MODALIDADECODIGO,
							   CU.TIPO AS SEGMENTOCODIGO,
							   TC.DESCRICAO AS SEGMENTO,
							   CV.CURSO,
							   CU.NOME AS CURSODESCRICAO,
							   CV.SERIE,
							   CV.TURNO,
							   O.CONTROLEVAGAID,
                               PC.EMAIL,
                               PC.DATANASCIMENTO,
                               PC.CPF,
                               PC.NOMEMAE,
                               PC.NOMEPAI,
                               UE.MUNICIPIO,
                               PC.CELULAR
                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
							   INNER JOIN MATRICULA.OPCAOINSCRICAO O (NOLOCK)
									  ON I.INSCRICAOALUNOID = O.INSCRICAOALUNOID
                               INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID
                               LEFT JOIN MATRICULA.INSCRICAOALUNO II (NOLOCK)
                                      ON I.IRMAOINSCRICAO = II.INSCRICAOALUNOID
							   INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK)
									  ON O.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA	
							   INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
									ON CV.CENSO = UE. UNIDADE_ENS
							   INNER JOIN LY_CURSO CU (NOLOCK)
									ON CV.CURSO = CU.CURSO
							   INNER JOIN LY_MODALIDADE_CURSO MO (NOLOCK)
									ON CU.MODALIDADE = MO.MODALIDADE 
							   INNER JOIN LY_TIPO_CURSO TC (NOLOCK)
									ON TC.TIPO = CU.TIPO	
                        WHERE O.OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

                contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    dados.Fase = Convert.ToInt32(dataReader["FASE"]);
                    dados.OpcaoInscricaoId = Convert.ToInt32(dataReader["OPCAOINSCRICAOID"]);
                    dados.InscricaoAlunoId = Convert.ToInt32(dataReader["INSCRICAOALUNOID"]);
                    dados.NumeroInscricao = Convert.ToInt32(dataReader["NUMEROINSCRICAO"]);
                    dados.IrmaoMatricula = dataReader["IRMAOMATRICULA"] == DBNull.Value ? (string)null : Convert.ToString(dataReader["IRMAOMATRICULA"]);
                    dados.IrmaoIdInscricao = dataReader["IRMAOINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["IRMAOINSCRICAO"]);
                    dados.IrmaoNumeroInscricao = dataReader["IRMAONUMEROINSCRICAO"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataReader["IRMAONUMEROINSCRICAO"]);
                    dados.Aluno = Convert.ToString(dataReader["ALUNO"]);
                    dados.PreCadastroAlunoId = Convert.ToInt32(dataReader["PRECADASTROALUNOID"]);
                    dados.Pessoa = dataReader["PESSOAID"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataReader["PESSOAID"]);
                    dados.Nome = Convert.ToString(dataReader["NOME"]);
                    dados.Telefone = Convert.ToString(dataReader["CELULAR"]);
                    dados.NomeMae = Convert.ToString(dataReader["NOMEMAE"]);
                    dados.NomePai = Convert.ToString(dataReader["NOMEPAI"]);
                    dados.DeclaroAusenciaMae = dados.NomeMae == "NÃO DECLARADA" ? true : false;
                    dados.DeclaroAusenciaPai = dados.NomePai == "NÃO DECLARADO" ? true : false;
                    dados.DataNascimento = Convert.ToDateTime(dataReader["DATANASCIMENTO"]);
                    dados.Censo = Convert.ToString(dataReader["CENSO"]);
                    dados.Escola = Convert.ToString(dataReader["ESCOLA"]);
                    dados.Ano = Convert.ToInt32(dataReader["ANO"]);
                    dados.Periodo = Convert.ToInt32(dataReader["PERIODO"]);
                    dados.Modalidade = Convert.ToString(dataReader["MODALIDADE"]);
                    dados.Segmento = Convert.ToString(dataReader["SEGMENTO"]);
                    dados.Curso = Convert.ToString(dataReader["CURSO"]);
                    dados.CursoDescricao = Convert.ToString(dataReader["CURSODESCRICAO"]);
                    dados.Serie = Convert.ToInt32(dataReader["SERIE"]);
                    dados.Turno = Convert.ToString(dataReader["TURNO"]);
                    dados.ControleVagaId = Convert.ToInt32(dataReader["CONTROLEVAGAID"]);
                    dados.Email = Convert.ToString(dataReader["EMAIL"]);
                    dados.Cpf = Convert.ToString(dataReader["CPF"]);
                    dados.ModalidadeCodigo = Convert.ToString(dataReader["MODALIDADECODIGO"]);
                    dados.SegmentoCodigo = Convert.ToString(dataReader["SEGMENTOCODIGO"]);
                    dados.Municipio = Convert.ToString(dataReader["MUNICIPIO"]);
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        public Matriculas.Entidades.OpcaoInscricao ObtemPor(DataContext contexto, int controleVagaId, int preCadastroAlunoId)
        {
            Matriculas.Entidades.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.Entidades.OpcaoInscricao();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT DISTINCT O.* 
                                                FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                       INNER JOIN MATRICULA.OPCAOINSCRICAO O (NOLOCK) 
                                                               ON I.INSCRICAOALUNOID = O.INSCRICAOALUNOID 
                                                WHERE  CONTROLEVAGAID = @CONTROLEVAGAID 
                                                       AND I.PRECADASTROALUNOID = @PRECADASTROALUNOID  ";

            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controleVagaId);
            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);

            opcao = contexto.TryToBindEntity<Matriculas.Entidades.OpcaoInscricao>(contextQuery);

            return opcao;
        }

        public bool ObtemVagaConcorrentePor(DataContext contexto, int opcaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT VAGACONCORRENTE
							FROM MATRICULA.OPCAOINSCRICAO (NOLOCK)
							WHERE OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

                contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToBoolean(reader["VAGACONCORRENTE"]);
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

        public void AtualizaConvocacao(DataContext contexto, int opcaoInscricaoId, DateTime dataConvocacao, string usuarioResponsavel, out DateTime prazoFinalRetorno, string municipio)
        {
            ContextQuery contextQuery = new ContextQuery();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new DiasNaoLetivos();
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = new List<Entidades.DiasNaoLetivos>();
            DateTime prazoFinal = dataConvocacao.Date;
            prazoFinalRetorno = DateTime.Now.Date;
            int diasUteis = 0;
            int prazoResposta = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PrazoResposta"]);

            //Busca Dias nao letivos
            diasNaoLetivos = rnDiasNaoLetivos.ListaPor(contexto, dataConvocacao.Year, municipio);

            //Calcula prazo final
            while (diasUteis < prazoResposta)
            {
                prazoFinal = prazoFinal.AddDays(1);

                //Verificar se não é sabado (6) ou domingo (0)
                if ((int)prazoFinal.DayOfWeek != 0 && (int)prazoFinal.DayOfWeek != 6)
                {
                    //Verifica se eh um dia nao letivo
                    if (diasNaoLetivos.Where(x => x.Dia.Date == prazoFinal.Date).Count() == 0)
                    {
                        diasUteis++;
                    }
                }
            }

            prazoFinalRetorno = prazoFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            contextQuery.Command = @"   UPDATE Matricula.OPCAOINSCRICAO
                                        SET DATACONVOCACAO = @DATACONVOCACAO,
                                            PRAZORESPOSTA = @PRAZORESPOSTA,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);
            contextQuery.Parameters.Add("@DATACONVOCACAO", SqlDbType.DateTime, dataConvocacao);
            contextQuery.Parameters.Add("@PRAZORESPOSTA", SqlDbType.DateTime, prazoFinalRetorno);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPrazoResposta(DataContext contexto, DadosProrrogacaoPrazoConfirmacao dadosProrrogacaoPrazoConfirmacao, string municipio, DateTime prazoResposta, DateTime novoPrazoResposta)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            sql.Append(@"  UPDATE OP
                                SET PRAZORESPOSTA = @NOVOPRAZORESPOSTA,
                                    USUARIOID = @USUARIOID,
                                    DATAALTERACAO = @DATAALTERACAO
                            FROM   MATRICULA.OPCAOINSCRICAO  OP  
                                   INNER JOIN TCE_CONTROLE_VAGA CV 
                                           ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
								   INNER JOIN LY_UNIDADE_ENSINO UE 
										   ON UE.UNIDADE_ENS = CV.CENSO
                            WHERE  DATACONVOCACAO IS NOT NULL 
                                    AND OP.PRAZORESPOSTA = @PRAZORESPOSTA 
                                    AND UE.MUNICIPIO = @MUNICIPIO ");

            if (dadosProrrogacaoPrazoConfirmacao.PorRegional)
            {
                sql.Append(" AND UE.ID_REGIONAL = @ID_REGIONAL");
                contextQuery.Parameters.Add("@ID_REGIONAL", SqlDbType.Int, dadosProrrogacaoPrazoConfirmacao.Regional);
            }

            if (dadosProrrogacaoPrazoConfirmacao.PorUnidadeEnsino)
            {
                sql.Append(" AND CV.CENSO = @CENSO");
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dadosProrrogacaoPrazoConfirmacao.UnidadeEnsino);
            }

            contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);
            contextQuery.Parameters.Add("@PRAZORESPOSTA", SqlDbType.DateTime, prazoResposta);
            contextQuery.Parameters.Add("@NOVOPRAZORESPOSTA", SqlDbType.DateTime, novoPrazoResposta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosProrrogacaoPrazoConfirmacao.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contextQuery.Command = sql.ToString();

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPrazoResposta(DataContext contexto, int opcaoInscricaoId, DateTime prazoResposta, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE Matricula.OPCAOINSCRICAO
                                        SET PRAZORESPOSTA = @PRAZORESPOSTA,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);
            contextQuery.Parameters.Add("@PRAZORESPOSTA", SqlDbType.DateTime, prazoResposta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public DateTime RetornaPrazoResposta(DataContext contexto, DateTime dataConvocacao, string municipio)
        {
            ContextQuery contextQuery = new ContextQuery();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new DiasNaoLetivos();
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = new List<Entidades.DiasNaoLetivos>();
            DateTime prazoFinal = dataConvocacao.Date;

            int diasUteis = 0;
            int prazoResposta = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PrazoResposta"]);
            DateTime prazoFinalRetorno = DateTime.Now.Date;

            //Busca Dias nao letivos
            diasNaoLetivos = rnDiasNaoLetivos.ListaPor(contexto, dataConvocacao.Year, municipio);

            //Calcula prazo final
            while (diasUteis < prazoResposta)
            {
                prazoFinal = prazoFinal.AddDays(1);

                //Verificar se não é sabado (6) ou domingo (0)
                if ((int)prazoFinal.DayOfWeek != 0 && (int)prazoFinal.DayOfWeek != 6)
                {
                    //Verifica se eh um dia nao letivo
                    if (diasNaoLetivos.Where(x => x.Dia.Date == prazoFinal.Date).Count() == 0)
                    {
                        diasUteis++;
                    }
                }
            }

            prazoFinalRetorno = prazoFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            return prazoFinalRetorno;
        }

        public Entidades.OpcaoInscricao ObtemProximoFilaPor(DataContext contexto, int ano, int periodo, string censo, string curso, int serie, string turno)
        {
            Entidades.OpcaoInscricao opcaoInscricao = new Entidades.OpcaoInscricao();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT TOP 1 OP.* 
                                        FROM   TCE_CONTROLE_VAGA CV (NOLOCK) 
                                               INNER JOIN MATRICULA.OPCAOINSCRICAO OP (NOLOCK) 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
                                        WHERE  ANO = @ANO 
                                               AND PERIODO = @PERIODO 
                                               AND CURSO = @CURSO 
                                               AND CENSO = @CENSO
                                               AND SERIE = @SERIE 
                                               AND TURNO = @TURNO
                                               AND DATACONVOCACAO IS NULL   
											   AND OP.TIPOFILAID IS NOT NULL 
											   AND OP.VAGACONCORRENTE = 1 --ETAPA SEM PROCESSO DE ALOCACAO 
                                        ORDER  BY OP.DATACADASTRO, OP.OPCAOINSCRICAOID";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

            opcaoInscricao = contexto.TryToBindEntity<Entidades.OpcaoInscricao>(contextQuery);

            return opcaoInscricao;
        }

        public void AtualizaSuspensao(DataContext contexto, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" --SEPARA AS OPÇOES DE INSCRIÇÃO (15)
                                    SELECT DISTINCT O.*
                                    INTO #OPCOES
                                    FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
		                                    INNER JOIN MATRICULA.OPCAOINSCRICAO O (NOLOCK) 
				                                    ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID 
		                                    INNER JOIN Matricula.PRECADASTROALUNO P ON P.PRECADASTROALUNOID=I.PRECADASTROALUNOID
		                                    INNER JOIN Matricula.PESSOAALUNO PA ON PA.PESSOAID = P.PESSOAID
		                                    INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = pa.ALUNO
                                    WHERE  DATACONVOCACAO IS NULL
	                                    AND TIPOFILAID IS NOT NULL
	                                    and HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID 


                                    --JOGA AS OPÇOES PARA O HISTORICO
                                    INSERT INTO Matricula.OPCAOINSCRICAOHIST 
                                                (INSCRICAOALUNOID, 
                                                    OPCAOINSCRICAOID, 
                                                    CONTROLEVAGAID, 
                                                    TIPOFILAID, 
                                                    DATACONVOCACAO, 
                                                    PRAZORESPOSTA,
                                                    CONFIRMADO, 
                                                    DATASITUACAO, 
                                                    TIPOCANDIDATOID,
                                                    MOTIVOREJEICAOINSCRICAOID, 
                                                    FASE, 
                                                    VAGACONCORRENTE, 
                                                    MOTIVORETORNOID,
                                                    DATARETORNO,
                                                    USUARIOID, 
                                                    DATACADASTRO, 
                                                    DATAALTERACAO) 
                                    SELECT O.INSCRICAOALUNOID, 
                                            OPCAOINSCRICAOID, 
                                            CONTROLEVAGAID, 
                                            TIPOFILAID, 
                                            DATACONVOCACAO, 
                                            PRAZORESPOSTA,
                                            0, 
                                            GETDATE(), 
                                            TIPOCANDIDATOID,
                                            29, 
                                            FASE,
                                            VAGACONCORRENTE, 
                                            MOTIVORETORNOID,
                                            DATARETORNO,
                                            O.USUARIOID, 
                                            O.DATACADASTRO, 
                                            GETDATE()
                                    FROM   #OPCOES O

                                    --JOGA OS CONTATOS PARA O HISTORICO
                                     INSERT INTO Matricula.CONTATOOPCAOINSCRICAOHIST 
                                                (OPCAOINSCRICAOHISTID, 
                                                    DATACONTATO, 
                                                    CONTATO, 
                                                    ACEITO, 
                                                    MOTIVOREJEICAOINSCRICAOID, 
                                                    OBSERVACAO, 
                                                    USUARIOID, 
                                                    DATACADASTRO, 
                                                    DATAALTERACAO) 
                                    select OH.OPCAOINSCRICAOHISTID, 
                                                    DATACONTATO, 
                                                    CONTATO, 
                                                    ACEITO, 
                                                    co.MOTIVOREJEICAOINSCRICAOID, 
                                                    OBSERVACAO, 
                                                    CO.USUARIOID, 
                                                    CO.DATACADASTRO, 
                                                    CO.DATAALTERACAO 
                                    from Matricula.CONTATOOPCAOINSCRICAO co (NOLOCK)
	                                    INNER JOIN MATRICULA.OPCAOINSCRICAOHIST OH (NOLOCK) ON CO.OPCAOINSCRICAOID = OH.OPCAOINSCRICAOID
	                                    INNER JOIN #OPCOES O ON O.OPCAOINSCRICAOID = OH.OPCAOINSCRICAOID                                    								  

                                    --DELETA OS CONTATOS
                                    DELETE Matricula.CONTATOOPCAOINSCRICAO 
                                    WHERE OPCAOINSCRICAOID IN (SELECT OPCAOINSCRICAOID FROM #OPCOES)

                                    --DELETA AS OPÇOES
                                    DELETE Matricula.OPCAOINSCRICAO 
                                    WHERE OPCAOINSCRICAOID IN (SELECT OPCAOINSCRICAOID FROM #OPCOES) ";

            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int opcaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Matricula.OPCAOINSCRICAO 
                                      WHERE OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemoveRetiradaFila(DataContext contexto, int ano, int periodo, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE O
                                    FROM   MATRICULA.OPCAOINSCRICAO O	
		                                    INNER JOIN TCE_CONTROLE_VAGA CV ON O.CONTROLEVAGAID = ID_CONTROLE_VAGA
                                    WHERE  CV.ANO = @ANO
	                                       AND CV.PERIODO = @PERIODO 
	                                       AND cv.CURSO = @CURSO
	                                       AND O.DATACONVOCACAO IS NULL ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemoveRetiradaFila(DataContext contexto, int controleVagaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE MATRICULA.OPCAOINSCRICAO
                                    WHERE CONTROLEVAGAID = @CONTROLEVAGAID
	                                       AND DATACONVOCACAO IS NULL ";

            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controleVagaId);

            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListaDatasConvocados()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT CONVERT(VARCHAR(20), PRAZORESPOSTA, 103) AS PRAZORESPOSTA, OP.PRAZORESPOSTA, 
											   COUNT(*) AS CONVOCADOS
                                        FROM   MATRICULA.OPCAOINSCRICAO OP
                                        WHERE  DATACONVOCACAO IS NOT NULL 
										GROUP BY  CONVERT(VARCHAR(20), PRAZORESPOSTA, 103), OP.PRAZORESPOSTA
										ORDER  BY OP.PRAZORESPOSTA ";

                lista = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return lista;
        }

        public DataTable ListaDatasConvocadosPorEscola(string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT CONVERT(VARCHAR(20), PRAZORESPOSTA, 103) AS PRAZORESPOSTA, OP.PRAZORESPOSTA, 
											   COUNT(*) AS CONVOCADOS
                                        FROM   MATRICULA.OPCAOINSCRICAO  OP  
                                               INNER JOIN TCE_CONTROLE_VAGA CV 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
                                        WHERE  DATACONVOCACAO IS NOT NULL 
												AND CV.CENSO = @CENSO
										GROUP BY  CONVERT(VARCHAR(20), PRAZORESPOSTA, 103), OP.PRAZORESPOSTA
										ORDER  BY op.PRAZORESPOSTA ";

                contextQuery.Parameters.Add("@CENSO", censo);

                lista = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return lista;
        }

        public DataTable ListaDatasConvocadosPorMunicipio(string municipio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT CONVERT(VARCHAR(20), PRAZORESPOSTA, 103) AS PRAZORESPOSTA, OP.PRAZORESPOSTA, 
											   COUNT(*) AS CONVOCADOS
                                        FROM   MATRICULA.OPCAOINSCRICAO  OP  
                                               INNER JOIN TCE_CONTROLE_VAGA CV 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
											   INNER JOIN LY_UNIDADE_ENSINO UE 
													   ON UE.UNIDADE_ENS = CV.CENSO
                                        WHERE  DATACONVOCACAO IS NOT NULL 
												AND UE.MUNICIPIO = @MUNICIPIO
										GROUP BY  CONVERT(VARCHAR(20), PRAZORESPOSTA, 103), OP.PRAZORESPOSTA
										ORDER  BY OP.PRAZORESPOSTA ";

                contextQuery.Parameters.Add("@MUNICIPIO", municipio);

                lista = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return lista;
        }

        public DataTable ListaDatasConvocadosPorRegional(int idRegional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contextQuery.Command = @" SELECT CONVERT(VARCHAR(20), PRAZORESPOSTA, 103) AS PRAZORESPOSTA, OP.PRAZORESPOSTA, 
											   COUNT(*) AS CONVOCADOS
                                        FROM   MATRICULA.OPCAOINSCRICAO  OP  
                                               INNER JOIN TCE_CONTROLE_VAGA CV 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
											   INNER JOIN LY_UNIDADE_ENSINO UE 
													   ON UE.UNIDADE_ENS = CV.CENSO
                                        WHERE  DATACONVOCACAO IS NOT NULL 
												AND UE.ID_REGIONAL = @ID_REGIONAL
										GROUP BY  CONVERT(VARCHAR(20), PRAZORESPOSTA, 103), OP.PRAZORESPOSTA
										ORDER  BY OP.PRAZORESPOSTA ";

                contextQuery.Parameters.Add("@ID_REGIONAL", idRegional);

                lista = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return lista;
        }

        public ValidacaoDados ValidaProrrogacaoPrazo(DadosProrrogacaoPrazoConfirmacao dadosProrrogacaoPrazoConfirmacao)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosProrrogacaoPrazoConfirmacao == null)
            {
                return validacaoDados;
            }

            if (!dadosProrrogacaoPrazoConfirmacao.Todos && !dadosProrrogacaoPrazoConfirmacao.PorUnidadeEnsino
                && !dadosProrrogacaoPrazoConfirmacao.PorMunicipio && !dadosProrrogacaoPrazoConfirmacao.PorRegional)
            {
                mensagens.Add("Escolha um TIPO para prorrogação de prazo.");
            }

            if (dadosProrrogacaoPrazoConfirmacao.PorUnidadeEnsino && dadosProrrogacaoPrazoConfirmacao.UnidadeEnsino.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (dadosProrrogacaoPrazoConfirmacao.PorMunicipio && dadosProrrogacaoPrazoConfirmacao.Municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICÍPIO é obrigatório.");
            }

            if (dadosProrrogacaoPrazoConfirmacao.PorRegional && dadosProrrogacaoPrazoConfirmacao.Regional <= 0)
            {
                mensagens.Add("Campo REGIONAL é obrigatório.");
            }

            if (!RN.Validacao.ValidaNumerosInteirosPositivos(dadosProrrogacaoPrazoConfirmacao.Dias.ToString()))
            {
                mensagens.Add("Campo DIAS ÚTEIS DE PRORROGAÇÃO é obrigatório e só permite números inteiros e positivos.");
            }

            if (dadosProrrogacaoPrazoConfirmacao.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
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

        public void ProrrogaPrazo(DadosProrrogacaoPrazoConfirmacao dadosProrrogacaoPrazoConfirmacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new DiasNaoLetivos();
            List<DadosOpcaoConvocada> convocados = new List<DadosOpcaoConvocada>();

            try
            {
                //Busca datas por municipio conforme filtro
                List<DadosPrazoRepostaTroca> datasMunicipio = this.ListaDadosPrazoRepostaTrocaPor(contexto, dadosProrrogacaoPrazoConfirmacao);

                //Calcula novo prazo para cada data por municipio
                foreach (DadosPrazoRepostaTroca item in datasMunicipio.OrderByDescending(x => x.PrazoReposta))
                {
                    //Calcula novo prazo
                    DateTime novoPrazoResposta = rnDiasNaoLetivos.RetornaProximaDataFinalPor(contexto, item.PrazoReposta, dadosProrrogacaoPrazoConfirmacao.Dias, item.Municipio);
                    item.NovoPrazoReposta = novoPrazoResposta;

                    //Atualiza todas os prazo de resposta do municipio de acordo com filtro
                    this.AtualizaPrazoResposta(contexto, dadosProrrogacaoPrazoConfirmacao, item.Municipio, item.PrazoReposta, novoPrazoResposta);
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

        public List<DadosOpcaoConvocada> ListaDadosOpcaoConvocadaPor(DataContext contexto, DadosProrrogacaoPrazoConfirmacao dadosProrrogacaoPrazoConfirmacao)
        {
            List<DadosOpcaoConvocada> lista = new List<DadosOpcaoConvocada>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int? regional = null;
            string municipio = null;
            string censo = null;

            try
            {
                regional = dadosProrrogacaoPrazoConfirmacao.PorRegional ? dadosProrrogacaoPrazoConfirmacao.Regional : (int?)null;
                municipio = dadosProrrogacaoPrazoConfirmacao.PorMunicipio ? dadosProrrogacaoPrazoConfirmacao.Municipio : null;
                censo = dadosProrrogacaoPrazoConfirmacao.PorUnidadeEnsino ? dadosProrrogacaoPrazoConfirmacao.UnidadeEnsino : null;

                contextQuery.Command = @" SELECT DISTINCT OP.OPCAOINSCRICAOID,
											   OP.INSCRICAOALUNOID,
											   OP.CONTROLEVAGAID,
											   UE.MUNICIPIO,
											   OP.PRAZORESPOSTA 
                                        FROM   MATRICULA.OPCAOINSCRICAO  OP  
                                               INNER JOIN TCE_CONTROLE_VAGA CV 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
											   INNER JOIN LY_UNIDADE_ENSINO UE 
													   ON UE.UNIDADE_ENS = CV.CENSO
                                        WHERE  DATACONVOCACAO IS NOT NULL 
												AND UE.ID_REGIONAL = isnull(@ID_REGIONAL, UE.ID_REGIONAL)
												AND UE.MUNICIPIO = isnull(@MUNICIPIO, UE.MUNICIPIO)
												AND CV.CENSO = isnull(@CENSO, CV.CENSO) ";

                contextQuery.Parameters.Add("@ID_REGIONAL", SqlDbType.Int, regional);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosOpcaoConvocada dadosOpcaoConvocada = new DadosOpcaoConvocada();
                    dadosOpcaoConvocada.OpcaoInscricaoId = Convert.ToInt32(reader["OPCAOINSCRICAOID"]);
                    dadosOpcaoConvocada.InscricaoAlunoId = Convert.ToInt32(reader["INSCRICAOALUNOID"]);
                    dadosOpcaoConvocada.ControleVagaId = Convert.ToInt32(reader["CONTROLEVAGAID"]);
                    dadosOpcaoConvocada.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosOpcaoConvocada.PrazoReposta = Convert.ToDateTime(reader["PRAZORESPOSTA"]);

                    lista.Add(dadosOpcaoConvocada);
                }

                return lista;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DadosPrazoRepostaTroca> ListaDadosPrazoRepostaTrocaPor(DataContext contexto, DadosProrrogacaoPrazoConfirmacao dadosProrrogacaoPrazoConfirmacao)
        {
            List<DadosPrazoRepostaTroca> lista = new List<DadosPrazoRepostaTroca>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int? regional = null;
            string municipio = null;
            string censo = null;

            try
            {
                regional = dadosProrrogacaoPrazoConfirmacao.PorRegional ? dadosProrrogacaoPrazoConfirmacao.Regional : (int?)null;
                municipio = dadosProrrogacaoPrazoConfirmacao.PorMunicipio ? dadosProrrogacaoPrazoConfirmacao.Municipio : null;
                censo = dadosProrrogacaoPrazoConfirmacao.PorUnidadeEnsino ? dadosProrrogacaoPrazoConfirmacao.UnidadeEnsino : null;

                contextQuery.Command = @" SELECT DISTINCT UE.MUNICIPIO,
											   OP.PRAZORESPOSTA 
                                        FROM   MATRICULA.OPCAOINSCRICAO  OP  
                                               INNER JOIN TCE_CONTROLE_VAGA CV 
                                                       ON CV.ID_CONTROLE_VAGA = OP.CONTROLEVAGAID 
											   INNER JOIN LY_UNIDADE_ENSINO UE 
													   ON UE.UNIDADE_ENS = CV.CENSO
                                        WHERE  DATACONVOCACAO IS NOT NULL 
												AND UE.ID_REGIONAL = isnull(@ID_REGIONAL, UE.ID_REGIONAL)
												AND UE.MUNICIPIO = isnull(@MUNICIPIO, UE.MUNICIPIO)
												AND CV.CENSO = isnull(@CENSO, CV.CENSO) ";

                contextQuery.Parameters.Add("@ID_REGIONAL", SqlDbType.Int, regional);
                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosPrazoRepostaTroca dadosPrazoRepostaTroca = new DadosPrazoRepostaTroca();
                    dadosPrazoRepostaTroca.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    dadosPrazoRepostaTroca.PrazoReposta = Convert.ToDateTime(reader["PRAZORESPOSTA"]);

                    lista.Add(dadosPrazoRepostaTroca);
                }

                return lista;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados ValidaConfirmacao(DadosConfirmacaoCandidato dadosConfirmacaoCandidato, LyFlPessoa flPessoa, LyFotoPessoa fotoPessoa)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PessoaAluno rnPessoaAluno = new PessoaAluno();
            InscricaoAluno rnInscricaoAluno = new InscricaoAluno();
            PreCadastroAlunoFoto rnPreCadastroAlunoFoto = new PreCadastroAlunoFoto();
            PreCadastroAluno rnPreCadastroAluno = new PreCadastroAluno();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Curriculo rnCurriculo = new Curriculo();
            RN.Turma rnTurma = new Turma();
            RN.Matricula rnMatricula = new Matricula();
            RN.Aluno rnAluno = new Aluno();
            decimal pessoaBase = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosConfirmacaoCandidato == null)
            {
                return validacaoDados;
            }

            if (dadosConfirmacaoCandidato.InscricaoAlunoId <= 0)
            {
                mensagens.Add("Campo INSCRIÇÃO é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.Fase <= 0)
            {
                mensagens.Add("Campo FASE é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.OpcaoInscricaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA OPÇÃO é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.PreCadastroAlunoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DO PRE CADASTRO é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.ControleVagaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DO CONTROLE DE VAGAS é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.Confirma == null)
            {
                mensagens.Add("Campo CONFIRMAR MATRICULA é obrigatório.");
            }
            else
            {
                if (Convert.ToBoolean(dadosConfirmacaoCandidato.Confirma))
                {
                    dadosConfirmacaoCandidato.MotivoRejeicaoInscricaoId = null;

                    if (flPessoa.FlField04.IsNullOrEmptyOrWhiteSpace() || flPessoa.FlField04 == "<Nenhum>")
                    {
                        mensagens.Add("Campo UTILIZA TRANSPORTE é obrigatório.");
                    }
                    else
                    {
                        if (flPessoa.FlField04 == "S")
                        {
                            if (flPessoa.FlField10.IsNullOrEmptyOrWhiteSpace() || flPessoa.FlField10 == "<Nenhum>")
                            {
                                mensagens.Add("Necessário selecionar o campo 'PODER PÚBLICO RESPONSÁVEL PELO TRANSPORTE ESCOLAR'.");
                            }

                            if (string.IsNullOrEmpty(flPessoa.FlField05))
                            {
                                mensagens.Add("Necessário selecionar pelo menos um modal.");
                            }
                            else
                            {
                                if (flPessoa.FlField05.Contains("5")) //Transporte Rural
                                {
                                    if (flPessoa.FlField11.IsNullOrEmptyOrWhiteSpace() && flPessoa.FlField12.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        mensagens.Add("Campo Rodoviário/Aquaviário é de preenchimento obrigatório.");
                                    }

                                    string[] modais = flPessoa.FlField05.Split(';');
                                    if (modais.Count() > 4)
                                    {
                                        mensagens.Add("Somente poderá ter no máximo três opções de Transporte Rodoviário/Aquaviário.");
                                    }
                                }

                                if (flPessoa.FlField05.Contains("2")) //Operadora de Ônibus
                                {
                                    if (flPessoa.FlField20.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        mensagens.Add("Campo Operadora é de preenchimento obrigatório.");
                                    }

                                    else
                                    {
                                        string[] modais = flPessoa.FlField20.Split(';');
                                        if (modais.Count() > 3)
                                        {
                                            mensagens.Add("Somente poderá ter no máximo duas opções de Transporte por Ônibus.");

                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (flPessoa.FlField23.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo COMUNIDADE QUILOMBOLA OU INDÍGENA é obrigatório.");
                    }
                }
                else
                {
                    if (dadosConfirmacaoCandidato.MotivoRejeicaoInscricaoId == null || dadosConfirmacaoCandidato.MotivoRejeicaoInscricaoId <= 0)
                    {
                        mensagens.Add("Campo MOTIVO é obrigatório.");
                    }
                }
            }

            if (dadosConfirmacaoCandidato.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.Confirma != null && Convert.ToBoolean(dadosConfirmacaoCandidato.Confirma))
            {
                if (dadosConfirmacaoCandidato.Curriculo.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Não foi encontrada MATRIZ CURRICULAR para este ano / periodo / curso / turno / serie.");
                }
            }

            if (dadosConfirmacaoCandidato.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.DataNascimento == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE NASCIMENTO é obrigatório.");
            }

            if (dadosConfirmacaoCandidato.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se inscrição ainda nao foi confirmada
                    if (!this.EhConvocadoPor(contexto, dadosConfirmacaoCandidato.OpcaoInscricaoId))
                    {
                        mensagens.Add("Este candidato não está mais pendente de confirmação.");
                    }

                    if (Convert.ToBoolean(dadosConfirmacaoCandidato.Confirma))
                    {
                        //Valida model
                        if (flPessoa.FlField04 == "S" && flPessoa.FlField05.Contains("6")) //PNE
                        {
                            //Verifica se cancidado possui necessidade especial
                            if (!rnPreCadastroAluno.PossuiNecessidadeEspecialPor(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId))
                            {
                                mensagens.Add("Somente poderá ser selecionado a opção PCD caso o aluno possuia necessidade Especial.");
                            }
                        }

                        //Busca pessoa da Inscrição
                        if (dadosConfirmacaoCandidato.Pessoa == null || dadosConfirmacaoCandidato.Pessoa <= 0)
                        {
                            dadosConfirmacaoCandidato.Pessoa = rnInscricaoAluno.ObtemPessoaPor(contexto, dadosConfirmacaoCandidato.InscricaoAlunoId);
                        }

                        if (dadosConfirmacaoCandidato.Pessoa == null || dadosConfirmacaoCandidato.Pessoa <= 0)
                        {
                            //Verifica se existe pessoa com msm nome / mae / nascimento
                            pessoaBase = rnPessoa.ObtemPessoaPor(contexto, dadosConfirmacaoCandidato.Nome, dadosConfirmacaoCandidato.NomeMae, Convert.ToDateTime(dadosConfirmacaoCandidato.DataNascimento));

                            if (pessoaBase > 0)
                            {
                                //Caso encontre atualiza pessoa
                                dadosConfirmacaoCandidato.Pessoa = pessoaBase;
                            }
                        }

                        //Busca dados de matricula
                        dadosConfirmacaoCandidato.PessoaAluno = rnPessoaAluno.ObtemPor(contexto, Convert.ToDecimal(dadosConfirmacaoCandidato.Pessoa));

                        //Busca turma com vaga
                        dadosConfirmacaoCandidato.Turma = rnTurma.ObtemPrimeiraTurmaComVagaPor(contexto, dadosConfirmacaoCandidato.ControleVagaId);

                        if (!dadosConfirmacaoCandidato.PessoaAluno.Aluno.IsNullOrEmptyOrWhiteSpace())
                        {
                            //Busca situação do aluno
                            dadosConfirmacaoCandidato.SitAluno = rnAluno.ObtemSituacaoAlunoPor(contexto, dadosConfirmacaoCandidato.PessoaAluno.Aluno);

                            //Busca dados da matricula atual do aluno
                            var dadosEnturmacaoAluno = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, dadosConfirmacaoCandidato.PessoaAluno.Aluno);

                            //Verifica se a matricual atual do aluno é de um ano/periodo anterior
                            bool possuiMatriculaAnoAnterior = !dadosEnturmacaoAluno.Aluno.IsNullOrEmptyOrWhiteSpace()
                                   && (dadosEnturmacaoAluno.Ano < dadosConfirmacaoCandidato.Ano
                                   || (dadosEnturmacaoAluno.Ano == dadosConfirmacaoCandidato.Ano && dadosEnturmacaoAluno.Periodo < dadosConfirmacaoCandidato.Periodo));

                            if (dadosConfirmacaoCandidato.PessoaAluno.EscolaAluno != dadosConfirmacaoCandidato.Censo && !possuiMatriculaAnoAnterior)
                            {
                                if (dadosConfirmacaoCandidato.Turma.IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Não será possível realizar a confirmação, pois o Número Máximo de Alunos a Enturmar foi excedido para o Ano/Período/Curso/Série/Turno informado. Solicitar o ajuste junto (a) ao CGI de sua Regional.");
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

        public ValidacaoDados ValidaRetornoNaoConfirmado(DadosRetornoOpcaoNaoConfirmada dadosRetornoOpcaoNaoConfirmada)
        {
            List<string> mensagens = new List<string>();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Matriculas.InscricaoAluno rnInscricaoAluno = new InscricaoAluno();
            RN.Entidades.TceControleVaga controleVaga = new TceControleVaga();
            DataContext contexto = null;
            int vagasUtilizadas = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosRetornoOpcaoNaoConfirmada == null)
            {
                return validacaoDados;
            }

            if (dadosRetornoOpcaoNaoConfirmada.InscricaoAlunoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA INSCRICAO é obrigatório.");
            }

            if (dadosRetornoOpcaoNaoConfirmada.OpcaoInscricaoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA OPÇÃO é obrigatório.");
            }

            if (dadosRetornoOpcaoNaoConfirmada.Fase <= 0)
            {
                mensagens.Add("Campo FASE é obrigatório.");
            }

            if (dadosRetornoOpcaoNaoConfirmada.ControleVagaId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DO CONTROLE DE VAGAS é obrigatório.");
            }

            if (dadosRetornoOpcaoNaoConfirmada.MotivoRetornoId <= 0)
            {
                mensagens.Add("Campo MOTIVO é obrigatório.");
            }

            if (dadosRetornoOpcaoNaoConfirmada.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o candidato possui outra opcao convocada
                    if (this.PossuiOpcaoConvocadaPor(contexto, dadosRetornoOpcaoNaoConfirmada.InscricaoAlunoId))
                    {
                        mensagens.Add("Este candidato possui outra opção convocada.");
                    }
                    else
                    {
                        //Verifica se o candidato possui a mesma opção
                        if (this.PossuiOpcaoPor(contexto, dadosRetornoOpcaoNaoConfirmada.InscricaoAlunoId, dadosRetornoOpcaoNaoConfirmada.ControleVagaId))
                        {
                            mensagens.Add("Este candidato já possui esta opção ativa (ou na fila).");
                        }
                    }

                    //Busca dados da opção 
                    controleVaga = rnControleVaga.ObtemPor(contexto, dadosRetornoOpcaoNaoConfirmada.ControleVagaId);

                    //Busca vagas utilizadas
                    vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(contexto,
                               controleVaga.Censo,
                               controleVaga.Ano,
                               controleVaga.Periodo,
                               controleVaga.Serie,
                               controleVaga.Curso,
                               controleVaga.Turno);

                    //Verifica se existe vaga para volta a opçao
                    if (controleVaga.VagasLiberadas <= vagasUtilizadas)
                    {
                        mensagens.Add("Não existem vagas disponíveis para os dados da opção do aluno.");
                    }

                    //Verifica se é da fase 1
                    if (dadosRetornoOpcaoNaoConfirmada.Fase == 1)
                    {
                        //Verifica se é a opçcao alocada pelo processo
                        if (!rnInscricaoAluno.EhOpcaoAlocadaFase1Por(contexto, dadosRetornoOpcaoNaoConfirmada.InscricaoAlunoId, dadosRetornoOpcaoNaoConfirmada.OpcaoInscricaoId))
                        {
                            mensagens.Add("Esta opção não pode retornar para a fase 1 pois o aluno foi alocado em outra opcao.");
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

        public void RetornaNaoConfirmado(DadosRetornoOpcaoNaoConfirmada dadosRetornoOpcaoNaoConfirmada)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new DiasNaoLetivos();
            RN.Matriculas.InscricaoAluno rnInscricaoAluno = new InscricaoAluno();
            RN.Matriculas.OpcaoInscricaoHist rnOpcaoInscricaoHist = new OpcaoInscricaoHist();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            ICollection<Entidades.DiasNaoLetivos> diasNaoLetivos = new List<Entidades.DiasNaoLetivos>();
            DateTime prazoFinal = DateTime.Now.Date;
            int diasUteis = 0;
            int prazoResposta = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PrazoResposta"]);
            string municipio;
            int novaOpcaoInscricaoId;

            try
            {
                //Busca municipio da opcao
                municipio = rnControleVaga.ObtemMunicipioPor(contexto, dadosRetornoOpcaoNaoConfirmada.ControleVagaId);

                //Busca Dias nao letivos
                diasNaoLetivos = rnDiasNaoLetivos.ListaPor(contexto, prazoFinal.Year, municipio);

                //Calcula prazo final
                while (diasUteis < prazoResposta)
                {
                    prazoFinal = prazoFinal.AddDays(1);

                    //Verificar se não é sabado (6) ou domingo (0)
                    if ((int)prazoFinal.DayOfWeek != 0 && (int)prazoFinal.DayOfWeek != 6)
                    {
                        //Verifica se eh um dia nao letivo
                        if (diasNaoLetivos.Where(x => x.Dia.Date == prazoFinal.Date).Count() == 0)
                        {
                            diasUteis++;
                        }
                    }
                }

                prazoFinal = prazoFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

                //Volta para tabela opcao a opção do historio
                this.Retorna(contexto, dadosRetornoOpcaoNaoConfirmada, prazoFinal, out novaOpcaoInscricaoId);

                //Verifica se é da fase 1
                if (dadosRetornoOpcaoNaoConfirmada.Fase == 1)
                {
                    //Caso seja fase 1, atualiza a opcao alocado com o novo id gerado
                    rnInscricaoAluno.AtualizaOpcaoConvocadaFase1(contexto, dadosRetornoOpcaoNaoConfirmada.InscricaoAlunoId, novaOpcaoInscricaoId);
                }

                //Remover opção do historico
                rnOpcaoInscricaoHist.Remove(contexto, dadosRetornoOpcaoNaoConfirmada.OpcaoInscricaoId);
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

        private void Retorna(DataContext contexto, DadosRetornoOpcaoNaoConfirmada dadosRetornoOpcaoNaoConfirmada, DateTime novoPrazoResposta, out int novaOpcaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.OPCAOINSCRICAO
                                                (INSCRICAOALUNOID, 
												CONTROLEVAGAID, 
												TIPOFILAID, 
												DATACONVOCACAO, 
												PRAZORESPOSTA, 
												USUARIOID, 
												DATACADASTRO, 
												DATAALTERACAO, 
												TIPOCANDIDATOID, 
												FASE, 
												VAGACONCORRENTE, 
												MOTIVORETORNOID, 
												DATARETORNO) 
                                    SELECT INSCRICAOALUNOID, 
										   CONTROLEVAGAID, 
										   TIPOFILAID, 
										   DATACONVOCACAO, 
										   @NOVOPRAZORESPOSTA, 
										   @USUARIOID, 
										   DATACADASTRO, 
										   DATAALTERACAO, 
										   TIPOCANDIDATOID, 
										   FASE, 
										   VAGACONCORRENTE, 
										   @MOTIVORETORNOID, 
										   @DATARETORNO 
                                    FROM   Matricula.OPCAOINSCRICAOHIST 
                                    WHERE  OPCAOINSCRICAOID = @OPCAOINSCRICAOID 

                                    SELECT OPCAOINSCRICAOID
                                    FROM MATRICULA.OPCAOINSCRICAO
                                    WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID
	                                    AND CONTROLEVAGAID = @CONTROLEVAGAID
	                                    AND FASE = @FASE ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, dadosRetornoOpcaoNaoConfirmada.OpcaoInscricaoId);
            contextQuery.Parameters.Add("@NOVOPRAZORESPOSTA", SqlDbType.DateTime, novoPrazoResposta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dadosRetornoOpcaoNaoConfirmada.UsuarioId);
            contextQuery.Parameters.Add("@DATARETORNO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@MOTIVORETORNOID", SqlDbType.Int, dadosRetornoOpcaoNaoConfirmada.MotivoRetornoId);
            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, dadosRetornoOpcaoNaoConfirmada.InscricaoAlunoId);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, dadosRetornoOpcaoNaoConfirmada.ControleVagaId);
            contextQuery.Parameters.Add("@FASE", SqlDbType.Int, dadosRetornoOpcaoNaoConfirmada.Fase);

            novaOpcaoInscricaoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private bool EhConvocadoPor(DataContext contexto, int opcaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   MATRICULA.OPCAOINSCRICAO (NOLOCK)
                                        WHERE  OPCAOINSCRICAOID = @OPCAOINSCRICAOID
											   AND DATACONVOCACAO IS NOT NULL ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Confirma(DadosConfirmacaoCandidato dadosConfirmacaoCandidato, LyFlPessoa flPessoa, LyFotoPessoa foto, out string matriculaAluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.HCursosConcl rnHCursosConcl = new HCursosConcl();
            RN.Transferencia rnTransferencia = new Transferencia();
            RN.Matriculas.ContatoOpcaoInscricaoHist rnContatoOpcaoInscricaoHist = new ContatoOpcaoInscricaoHist();
            RN.Matriculas.PessoaAluno rnPessoaAluno = new PessoaAluno();
            RN.Matriculas.ContatoOpcaoInscricao rnContatoOpcaoInscricao = new ContatoOpcaoInscricao();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            RN.Matriculas.InscricaoAluno rnInscricaoAluno = new InscricaoAluno();
            RN.Matriculas.VagaReservada rnVagaReservada = new VagaReservada();
            Entidades.VagaReservada vagaReservada = new Techne.Lyceum.RN.Matriculas.Entidades.VagaReservada();
            PreCadastroAluno rnPreCadastroAluno = new PreCadastroAluno();
            OpcaoInscricaoHist rnOpcaoInscricaoHist = new OpcaoInscricaoHist();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            DadosEnturmacaoAluno dadosEnturmacaoAluno = new DadosEnturmacaoAluno();
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Matricula rnMatricula = new Matricula();
            RN.Matgrade rnMatgrade = new Matgrade();
            RN.Aluno rnAluno = new Aluno();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RN.RelacaoPessoa rnRelacaoPessoa = new RelacaoPessoa();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            string tipoVaga = string.Empty;
            bool confirmado = false;
            bool validacao = false;
            matriculaAluno = string.Empty;
            int controleVagaId = 0;
            string municipio = string.Empty;
            int? motivoRejeicaoInscricaoId = null;
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            bool possuiMatriculaAnoAnterior = false;

            try
            {
                //Verifica se o aluno digitou a matricula no momento da inscrição                            
                string matriculaConexao = rnInscricaoAluno.ObtemAlunoPor(contexto, dadosConfirmacaoCandidato.InscricaoAlunoId);
                if (!dadosConfirmacaoCandidato.PessoaAluno.Aluno.IsNullOrEmptyOrWhiteSpace() || !matriculaConexao.IsNullOrEmptyOrWhiteSpace())
                {
                    //Verifica se a matricula digitada não é a principal
                    if (!matriculaConexao.IsNullOrEmptyOrWhiteSpace() && matriculaConexao != dadosConfirmacaoCandidato.PessoaAluno.Aluno)
                    {
                        //Busca pessoa da matricula digitada 
                        decimal pessoaMatriculaConexao = rnAluno.ObtemPessoaPor(contexto, matriculaConexao);

                        //Verifica se a pessoa da matricula digitada, confere com a pessoa encontrada
                        if (pessoaMatriculaConexao != dadosConfirmacaoCandidato.PessoaAluno.PessoaId)
                        {
                            //Atualiza pessoa do precadastro para pessoa do conexao
                            rnPreCadastroAluno.AtualizaPessoa(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, pessoaMatriculaConexao, dadosConfirmacaoCandidato.UsuarioResponsavel);

                            //Atualiza pessoa nos DTOs
                            dadosConfirmacaoCandidato.Pessoa = pessoaMatriculaConexao;
                            dadosConfirmacaoCandidato.PessoaAluno.PessoaId = pessoaMatriculaConexao;
                            if (flPessoa != null)
                            {
                                flPessoa.Pessoa = pessoaMatriculaConexao;
                            }
                            if (foto != null)
                            {
                                foto.Pessoa = pessoaMatriculaConexao;
                            }
                        }
                    }
                }

                if (Convert.ToBoolean(dadosConfirmacaoCandidato.Confirma))
                {
                    if (!dadosConfirmacaoCandidato.Cpf.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Busca a pessoa por CPF
                        string cpf = dadosConfirmacaoCandidato.Cpf.RetirarMascaraCPF();
                        decimal pessoaCPF = rnPessoa.ObtemPessoaPor(contexto, cpf);

                        //Verifica se a pessoa do cpf é diferente do instrito
                        if (pessoaCPF > 0 && pessoaCPF != dadosConfirmacaoCandidato.Pessoa)
                        {
                            //Limpa o CPF                               
                            dadosConfirmacaoCandidato.Cpf = null;
                        }
                    }
                    else
                    {
                        dadosConfirmacaoCandidato.Cpf = null;
                    }

                    if (dadosConfirmacaoCandidato.Pessoa == null || dadosConfirmacaoCandidato.Pessoa <= 0)
                    {
                        //Insere Pessoa
                        decimal pessoa;
                        rnPessoa.InserePessoaPreCadastro(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, dadosConfirmacaoCandidato.Nome, dadosConfirmacaoCandidato.NomeMae, dadosConfirmacaoCandidato.DataNascimento, dadosConfirmacaoCandidato.UsuarioResponsavel, dadosConfirmacaoCandidato.Cpf, out pessoa);
                        dadosConfirmacaoCandidato.Pessoa = pessoa;

                        //insere foto
                        if (foto.Foto != null)
                        {
                            foto.Pessoa = pessoa;
                            if (foto.Pessoa != 0)
                            {
                                FotoPessoa.Inserir(foto, contexto);
                            }
                            else
                            {
                                throw new Exception("Código da Pessoa não localizado para cadastrar uma Foto.");
                            }
                        }

                        //Atualiza pessoa gerada no precadastro
                        rnPreCadastroAluno.AtualizaPessoa(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, pessoa, dadosConfirmacaoCandidato.UsuarioResponsavel);

                        //insere FlPessoa
                        rnFlPessoa.InsereFlPessoaPreCadastro(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, flPessoa);
                    }
                    else
                    {
                        //Atualiza pessoa encontrada no precadastro
                        rnPreCadastroAluno.AtualizaPessoa(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, Convert.ToDecimal(dadosConfirmacaoCandidato.Pessoa), dadosConfirmacaoCandidato.UsuarioResponsavel);

                        //Atualiza Pessoa
                        rnPessoa.AtualizaPessoaPreCadastro(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, dadosConfirmacaoCandidato.UsuarioResponsavel, dadosConfirmacaoCandidato.Cpf);

                        //Salva foto
                        if (foto.Foto != null)
                        {
                            foto.Pessoa = dadosConfirmacaoCandidato.Pessoa;
                            //verificar se já tem uma foto para esta pessoa
                            if (FotoPessoa.VerificarFoto(Convert.ToDecimal(dadosConfirmacaoCandidato.Pessoa)))
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

                        //insere ou atualiza flpessoa
                        if (rnFlPessoa.ExistePor(contexto, Convert.ToDecimal(dadosConfirmacaoCandidato.Pessoa)))
                        {
                            rnFlPessoa.AtualizaFlPessoaPreCadastro(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, flPessoa);
                        }
                        else
                        {
                            rnFlPessoa.InsereFlPessoaPreCadastro(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, flPessoa);
                        }



                        //Verifica se já existe matricula para a pessoa
                        if (!dadosConfirmacaoCandidato.PessoaAluno.Aluno.IsNullOrEmptyOrWhiteSpace() || !matriculaConexao.IsNullOrEmptyOrWhiteSpace())
                        {
                            //Verifica se a matricula digitada não é a principal
                            if (!matriculaConexao.IsNullOrEmptyOrWhiteSpace() && matriculaConexao != dadosConfirmacaoCandidato.PessoaAluno.Aluno)
                            {
                                //Busca pessoa da matricula digitada 
                                decimal pessoaMatriculaConexao = rnAluno.ObtemPessoaPor(contexto, matriculaConexao);

                                ////Retirada a validação a pedido do Chamado - ID 78946
                                ////Verifica se a pessoa da matricula digitada, confere com a pessoa encontrada
                                //if (pessoaMatriculaConexao != dadosConfirmacaoCandidato.PessoaAluno.PessoaId)
                                //{                                   
                                //    //validacao = true;
                                //    //throw new Exception("PESSOA NÃO CONFERE");    
                                //}

                                //Atualiza matricula digitada pelo aluno como principal para a pessoa
                                rnPessoaAluno.AtualizaAluno(contexto, dadosConfirmacaoCandidato.PessoaAluno.PessoaId, matriculaConexao, dadosConfirmacaoCandidato.UsuarioResponsavel);

                                dadosConfirmacaoCandidato.PessoaAluno.Aluno = matriculaConexao;
                            }

                            //Alimenta matricula do aluno
                            matriculaAluno = dadosConfirmacaoCandidato.PessoaAluno.Aluno;

                            //Reabre aluno
                            rnHCursosConcl.ReabreAlunoPreCadastro(contexto, matriculaAluno, dadosConfirmacaoCandidato.Ano, dadosConfirmacaoCandidato.Periodo);

                            //Verifica se aluno está ativo
                            if (dadosConfirmacaoCandidato.SitAluno == "Ativo")
                            {
                                //Busca dados da matricula atual do aluno
                                dadosEnturmacaoAluno = rnMatricula.ObtemMatriculaPrincipalAtivaPor(contexto, matriculaAluno);

                                //Verifica se a confirmaçao do aluno é para o periodo 2 e se o aluno possui enturmação ativa no periodo 1
                                if (dadosConfirmacaoCandidato.Periodo == 2 && dadosEnturmacaoAluno.Periodo == 1)
                                {
                                    validacao = true;
                                    //NÃO PERMITIR CONFIRMAR O CANDIDATO PARA O PERÍODO 2 ENQUANTO TIVER ENTURMACAO ATIVA NO PERÍODO 1
                                    throw new Exception("Este candidato não pode ser confirmado no periodo 2 pois sua enturmação ativa no periodo 1.");
                                }

                                //Verifica se o aluno tem dados da enturmacao
                                if (dadosEnturmacaoAluno.IdControleVaga > 0)
                                {
                                    controleVagaId = dadosEnturmacaoAluno.IdControleVaga;
                                    municipio = dadosEnturmacaoAluno.MunicipioEscola;
                                }
                                else
                                {
                                    //Caso não tenha busca dados da confirmação
                                    controleVagaId = rnConfirmacaoMatricula.ObtemControleVagaIdConfirmadoPor(contexto, matriculaAluno, dadosConfirmacaoCandidato.Ano, dadosConfirmacaoCandidato.Periodo, out municipio);
                                }

                                //Verifica se a matricual atual do aluno é de um ano/periodo anterior
                                possuiMatriculaAnoAnterior = !dadosEnturmacaoAluno.Aluno.IsNullOrEmptyOrWhiteSpace()
                                    && (dadosEnturmacaoAluno.Ano < dadosConfirmacaoCandidato.Ano
                                    || (dadosEnturmacaoAluno.Ano == dadosConfirmacaoCandidato.Ano
                                    && dadosEnturmacaoAluno.Periodo != 0
                                    && dadosConfirmacaoCandidato.Periodo != 0
                                    && dadosEnturmacaoAluno.Periodo < dadosConfirmacaoCandidato.Periodo));

                                //Apenas cancela caso o aluno não tenha uma matricula ativa no ano / periodo anterior
                                if (!possuiMatriculaAnoAnterior)
                                {
                                    //cancela matricula
                                    rnMatricula.CancelaMatriculaPor(contexto, matriculaAluno, dadosConfirmacaoCandidato.UsuarioResponsavel);

                                    //Cancela matgrade
                                    rnMatgrade.CancelaMatgradePor(contexto, matriculaAluno);

                                    ////Cancela Renovaçoes
                                    //rnRenovacao.CancelaRenovacaoPor(contexto, matriculaAluno, dadosConfirmacaoCandidato.UsuarioResponsavel);
                                }
                            }

                            //Verifica se aluno é de outra escola e não possue matricula no ano / periodo anterior
                            if (dadosConfirmacaoCandidato.PessoaAluno.EscolaAluno != dadosConfirmacaoCandidato.Censo && !possuiMatriculaAnoAnterior)
                            {
                                if (dadosConfirmacaoCandidato.Turma.IsNullOrEmptyOrWhiteSpace())
                                {
                                    validacao = true;
                                    throw new Exception("Não existe turma com vaga para efetuar a transferência. Favor verificar!");
                                }
                                else
                                {
                                    //Cria transferencia para a turma, e enturma o aluno
                                    rnTransferencia.GeraTransferenciaEnturmaAlunoPreMatricula(contexto, matriculaAluno, dadosConfirmacaoCandidato.UsuarioResponsavel, dadosConfirmacaoCandidato.OpcaoInscricaoId, dadosConfirmacaoCandidato.Turma, dadosConfirmacaoCandidato.Curriculo, dadosConfirmacaoCandidato.EnsinoReligioso, dadosConfirmacaoCandidato.LinguaEstrangeiraFacultativa);

                                    //Verifica se a matricula é no msm ano / possiveis periodos
                                    if (dadosEnturmacaoAluno.Ano == dadosConfirmacaoCandidato.Ano
                                        && (dadosEnturmacaoAluno.Periodo == dadosConfirmacaoCandidato.Periodo
                                        || dadosEnturmacaoAluno.Periodo == 0
                                        || dadosConfirmacaoCandidato.Periodo == 0))
                                    {
                                        //Monta Vaga reservada                              
                                        vagaReservada.Aluno = matriculaAluno;
                                        vagaReservada.ControleVagaId = controleVagaId;
                                        vagaReservada.DataInicio = DateTime.Now;
                                        vagaReservada.DataFim = rnDiasNaoLetivos.RetornaDataFinalPor(contexto, DateTime.Now, 1, municipio);
                                        vagaReservada.UsuarioId = dadosConfirmacaoCandidato.UsuarioResponsavel;

                                        if (vagaReservada.ControleVagaId > 0)
                                        {
                                            //Colocar a vaga com dados de origem da tabela de reserva 24 horas
                                            rnVagaReservada.Insere(contexto, vagaReservada);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Verifica se possui irmao
                    if (!dadosConfirmacaoCandidato.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace() ||
                        (dadosConfirmacaoCandidato.IrmaoIdInscricao != null && dadosConfirmacaoCandidato.IrmaoIdInscricao > 0))
                    {
                        decimal pessoaIrmao = 0;

                        //Busca pessoa do irmao
                        if (!dadosConfirmacaoCandidato.IrmaoMatricula.IsNullOrEmptyOrWhiteSpace())
                        {
                            pessoaIrmao = rnAluno.ObtemPessoaPor(contexto, dadosConfirmacaoCandidato.IrmaoMatricula);
                        }
                        else if (dadosConfirmacaoCandidato.IrmaoIdInscricao != null && dadosConfirmacaoCandidato.IrmaoIdInscricao > 0)
                        {
                            //Busca dados da inscrição do irmao
                            var candidatoIrmao = rnInscricaoAluno.ObtemDadosCandidatoPor(contexto, Convert.ToInt32(dadosConfirmacaoCandidato.IrmaoIdInscricao));

                            //Caso a incrição possua pessoa, usar a pessoa
                            if (candidatoIrmao.Pessoa != null && candidatoIrmao.Pessoa > 0)
                            {
                                pessoaIrmao = Convert.ToDecimal(candidatoIrmao.Pessoa);
                            }
                            else
                            {
                                //Caso o não possuia pessoa buscar por CPF
                                if (!candidatoIrmao.Cpf.IsNullOrEmptyOrWhiteSpace())
                                {
                                    pessoaIrmao = rnPessoa.ObtemPessoaPor(contexto, candidatoIrmao.Cpf);
                                }

                                //Caso não encontre por cpf, busca com nome + mae + data nascimento
                                if (pessoaIrmao == 0)
                                {
                                    pessoaIrmao = rnPessoa.ObtemPessoaPor(contexto, candidatoIrmao.Nome, candidatoIrmao.NomeMae, Convert.ToDateTime(candidatoIrmao.DataNascimento));
                                }
                            }
                        }

                        //Caso a pessoa seja encontrada Cria relação pessoa com irmao
                        int parentescoId = 1; //Irmão
                        if (pessoaIrmao > 0)
                        {
                            rnRelacaoPessoa.Insere(contexto, Convert.ToDecimal(dadosConfirmacaoCandidato.Pessoa), pessoaIrmao, parentescoId);
                        }
                    }

                    if (matriculaAluno.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Gera matricula para o novo aluno
                        LyAluno aluno = new LyAluno();
                        aluno.UnidadeEnsino = dadosConfirmacaoCandidato.Censo;
                        aluno.AnoIngresso = dadosConfirmacaoCandidato.Ano;
                        rnAluno.GeraCodigoAluno(contexto, aluno);
                        matriculaAluno = aluno.Aluno;

                        //Cria aluno
                        rnAluno.InsereAlunoPreCadastro(contexto, dadosConfirmacaoCandidato.PreCadastroAlunoId, matriculaAluno, dadosConfirmacaoCandidato.Curriculo, dadosConfirmacaoCandidato.UsuarioResponsavel, dadosConfirmacaoCandidato.ControleVagaId);
                    }
                    else
                    {
                        //Atualiza Aluno
                        rnAluno.AtualizaAlunoPreCadastro(contexto, matriculaAluno, dadosConfirmacaoCandidato.Curriculo, dadosConfirmacaoCandidato.UsuarioResponsavel, dadosConfirmacaoCandidato.ControleVagaId, dadosConfirmacaoCandidato.OpcaoInscricaoId);

                        //Cancela Confirmações
                        rnConfirmacaoMatricula.CancelaPossiveisConfirmacaoMatriculaPor(contexto, matriculaAluno, dadosConfirmacaoCandidato.Ano, dadosConfirmacaoCandidato.Periodo, dadosConfirmacaoCandidato.UsuarioResponsavel);
                    }

                    tipoVaga = "VU";

                    //criar confirmacao confirmada
                    rnConfirmacaoMatricula.InsereAlunoPreCadastro(contexto, dadosConfirmacaoCandidato.ControleVagaId, matriculaAluno, dadosConfirmacaoCandidato.Curriculo, tipoVaga, dadosConfirmacaoCandidato.EnsinoReligioso, dadosConfirmacaoCandidato.LinguaEstrangeiraFacultativa, dadosConfirmacaoCandidato.UsuarioResponsavel);

                    //Enturma aluno na primeira turma com vaga (caso exista)
                    if (!dadosConfirmacaoCandidato.Turma.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Apenas enturma caso o aluno não tenha uma matricula ativa no eno / periodo anterior
                        if (!possuiMatriculaAnoAnterior)
                        {
                            //Bsuca dados da vaga
                            TceControleVaga vaga = rnControleVaga.ObtemPor(contexto, dadosConfirmacaoCandidato.ControleVagaId);

                            //Enturma Aluno
                            rnMatricula.EnturmaAluno(contexto, matriculaAluno, vaga.Ano, vaga.Periodo, dadosConfirmacaoCandidato.Turma, vaga.Curso, dadosConfirmacaoCandidato.Curriculo, dadosConfirmacaoCandidato.Serie, dadosConfirmacaoCandidato.UsuarioResponsavel, dadosEnturmacaoAluno);

                            //Atuliza dados da enturmação
                            rnAluno.AtualizaDadosEnturmacao(contexto, matriculaAluno, dadosConfirmacaoCandidato.Curriculo, dadosConfirmacaoCandidato.UsuarioResponsavel, dadosConfirmacaoCandidato.ControleVagaId, dadosConfirmacaoCandidato.OpcaoInscricaoId);
                        }
                    }

                    //Coloca dados para opção aceita
                    confirmado = true;
                    motivoRejeicaoInscricaoId = null;
                }
                else
                {
                    //Coloca dados para opção não aceita
                    confirmado = false;
                    motivoRejeicaoInscricaoId = dadosConfirmacaoCandidato.MotivoRejeicaoInscricaoId;
                }

                //Atualiza coluna para confirmado na tabela de inscrição
                if (dadosConfirmacaoCandidato.Fase == 1)
                {
                    rnInscricaoAluno.ConfirmaFase1(contexto, dadosConfirmacaoCandidato.InscricaoAlunoId, confirmado);
                }
                else
                {
                    rnInscricaoAluno.ConfirmaFase2(contexto, dadosConfirmacaoCandidato.InscricaoAlunoId, confirmado);
                }

                //Insere opção no historico
                rnOpcaoInscricaoHist.Insere(contexto, dadosConfirmacaoCandidato.OpcaoInscricaoId, confirmado, motivoRejeicaoInscricaoId);

                //Remover opção 
                this.Remove(contexto, dadosConfirmacaoCandidato.OpcaoInscricaoId);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!validacao &&
                    !Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
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

        public DataTable ListaAlocacaoFase1Por(int inscricaoAlunoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT  A.ID_OPCAOESCOLA as OPCAOINSCRICAOID,
		                                          CV.ID_CONTROLE_VAGA as CONTROLEVAGAID,
                                                  DATA_INCLUSAO as DATACADASTRO, 
		                                          CV.CENSO,
		                                          UE.NOME_COMP AS ESCOLA,
		                                          CV.CURSO,
		                                          CV.TURNO,
		                                          CV.SERIE,
		                                          C.NOME AS DESCRICAOCURSO,		
		                                          TU.DESCRICAO AS DESCRICAOTURNO,
		                                          T.DESCRICAO AS DESCRICAOTIPO,
		                                          M.DESCRICAO + ' - ' + C.NOME AS DESCRICAOMODALIDADE,
		                                          A.NUM_OPCAO AS OPCAO,
		                                          CASE
			                                            WHEN I.OPCAOINSCRICAOIDALOCADA = A.ID_OPCAOESCOLA THEN 'Alocado'
			                                            ELSE 'Não Alocado'
		                                          END SITUACAO
                                            FROM MATRICULA.INSCRICAOALUNO I
	                                            INNER JOIN ALOCACAO.MATR.OPCOES_SEEDUC A ON A.ID_INSCRICAO = I.NUMEROINSCRICAO
	                                            LEFT JOIN MATRICULA.OPCAOINSCRICAO OI ON A.ID_OPCAOESCOLA = OI.OPCAOINSCRICAOID
	                                            LEFT JOIN MATRICULA.OPCAOINSCRICAOHIST OHI ON A.ID_OPCAOESCOLA = OHI.OPCAOINSCRICAOID		
	                                            INNER JOIN TCE_CONTROLE_VAGA CV ON ISNULL(OI.CONTROLEVAGAID, OHI.CONTROLEVAGAID) = CV.ID_CONTROLE_VAGA
	                                            INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON CV.CENSO = UE.UNIDADE_ENS
	                                            INNER JOIN LY_CURSO C (NOLOCK) ON CV.CURSO = C.CURSO
	                                            INNER JOIN LY_TIPO_CURSO T (NOLOCK) ON C.TIPO = T.TIPO
	                                            INNER JOIN LY_TURNO TU (NOLOCK) ON CV.TURNO = TU.TURNO 
	                                            INNER JOIN LY_MODALIDADE_CURSO M (NOLOCK) ON C.MODALIDADE = M.MODALIDADE
                                            WHERE I.INSCRICAOALUNOID = @INSCRICAOALUNOID
                                            ORDER BY A.NUM_OPCAO ";

                contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

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

        public DataTable ListaPor(int inscricaoAlunoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"  SELECT  O.OPCAOINSCRICAOID,
								                O.CONTROLEVAGAID,
                                                O.DATACADASTRO, 
		                                        CV.CENSO,
		                                        UE.NOME_COMP AS ESCOLA,
		                                        CV.CURSO,
		                                        CV.TURNO,
		                                        CV.SERIE,
		                                        C.NOME AS DESCRICAOCURSO,		
		                                        TU.DESCRICAO AS DESCRICAOTURNO,
		                                        T.DESCRICAO AS DESCRICAOTIPO,
		                                        M.DESCRICAO + ' - ' + C.NOME AS DESCRICAOMODALIDADE,
		                                        IDENTITY(int, 1,1) AS OPCAO,
								                O.TIPOFILAID,
                                                TF.PRIORIDADE,
		                                        CONVERT(INT, NULL) AS POSICAOFILA,
								                CASE
								                 WHEN O.DATACONVOCACAO IS NULL THEN 'Pendente'
								                 ELSE 'Convocado'
								                END SITUACAO
						                INTO #LISTA
                                        FROM	Matricula.OPCAOINSCRICAO O (NOLOCK)
                                                LEFT JOIN  MATRICULA.TIPOFILA TF (NOLOCK) ON O.TIPOFILAID = TF.TIPOFILAID
		                                        INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON O.CONTROLEVAGAID = CV.ID_CONTROLE_VAGA
		                                        INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON CV.CENSO = UE.UNIDADE_ENS
		                                        INNER JOIN LY_CURSO C (NOLOCK) ON CV.CURSO = C.CURSO
		                                        INNER JOIN LY_TIPO_CURSO T (NOLOCK) ON C.TIPO = T.TIPO
		                                        INNER JOIN LY_TURNO TU (NOLOCK) ON CV.TURNO = TU.TURNO 
		                                        INNER JOIN LY_MODALIDADE_CURSO M (NOLOCK) ON C.MODALIDADE = M.MODALIDADE
                                        WHERE	INSCRICAOALUNOID = @INSCRICAOALUNOID
						                ORDER BY O.DATACADASTRO, O.OPCAOINSCRICAOID

							                UPDATE #LISTA
							                SET POSICAOFILA =  ISNULL((SELECT COUNT(*) QTDE_INSCRITOS 
										                   FROM   MATRICULA.OPCAOINSCRICAO OI (NOLOCK)
                                                                  INNER JOIN  MATRICULA.TIPOFILA TF (NOLOCK) ON OI.TIPOFILAID = TF.TIPOFILAID
										                   WHERE  OI.CONTROLEVAGAID = #LISTA.CONTROLEVAGAID 
												                  AND TF.PRIORIDADE = #LISTA.PRIORIDADE
												                  AND OI.DATACADASTRO <= #LISTA.DATACADASTRO
												                  AND OI.OPCAOINSCRICAOID <= #LISTA.OPCAOINSCRICAOID
								                                  AND OI.DATACONVOCACAO IS NULL
										                  ), 0) 
							                WHERE TIPOFILAID IS NOT NULL  

						                SELECT DISTINCT i.OPCAOINSCRICAOID,
											i.CONTROLEVAGAID,
											i.DATACADASTRO,
											i.CENSO,
											i.ESCOLA,
											i.CURSO,
											i.TURNO,
											i.SERIE,
											i.DESCRICAOCURSO,
											i.DESCRICAOTURNO,
											i.DESCRICAOTIPO,
											i.DESCRICAOMODALIDADE,
											isnull(A.num_opcao, i.OPCAO) as OPCAO,
											i.TIPOFILAID,
											i.PRIORIDADE,
											i.POSICAOFILA,
											i.SITUACAO
										FROM #LISTA i
											LEFT JOIN ALOCACAO.MATR.OPCOES_SEEDUC A 
												ON A.ID_OPCAOESCOLA = i.OPCAOINSCRICAOID
						                ORDER BY OPCAO

						                DROP TABLE #LISTA ";


                contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

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

        public bool PossuiMotivoRetornoPor(DataContext contexto, int motivoRetornoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM MATRICULA.OPCAOINSCRICAO (NOLOCK)
                                    WHERE MOTIVOREJEICAOINSCRICAOID = @MOTIVORETORNOID ";

            contextQuery.Parameters.Add("@MOTIVORETORNOID", SqlDbType.Int, motivoRetornoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOpcaoPor(DataContext contexto, int inscricaoAlunoId, int controleVagaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1)
                                    FROM Matricula.OPCAOINSCRICAO (NOLOCK)
                                    WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID
	                                      AND CONTROLEVAGAID = @CONTROLEVAGAID ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controleVagaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiOpcaoConvocadaPor(DataContext contexto, int inscricaoAlunoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1)
                                    FROM Matricula.OPCAOINSCRICAO (NOLOCK)
                                    WHERE INSCRICAOALUNOID = @INSCRICAOALUNOID
	                                      AND DATACONVOCACAO IS NOT NULL ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiConvocacaoPendentePor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM   MATRICULA.OPCAOINSCRICAO O (NOLOCK)
                                                   INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK)
                                                           ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID
                                                   INNER JOIN MATRICULA.PRECADASTROALUNO PC (NOLOCK)
                                                           ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID
                                                   INNER JOIN MATRICULA.PESSOAALUNO PA (NOLOCK)
                                                           ON PA.PESSOAID = PC.PESSOAID
                                            WHERE  DATACONVOCACAO IS NOT NULL
                                                   AND PA.ALUNO = @ALUNO  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);


                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public int ObtemOpcaoIdPor(DataContext contexto, string matriculaAluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT DISTINCT O.OPCAOINSCRICAOID
                                        FROM   MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                               INNER JOIN MATRICULA.OPCAOINSCRICAO O (NOLOCK) 
                                                      ON O.INSCRICAOALUNOID = I.INSCRICAOALUNOID 
                                               INNER JOIN Matricula.PRECADASTROALUNO P ON P.PRECADASTROALUNOID=I.PRECADASTROALUNOID
                                               INNER JOIN Matricula.PESSOAALUNO PA ON PA.PESSOAID = P.PESSOAID
                                        WHERE  PA.ALUNO = @ALUNO 
                                            AND DATACONVOCACAO IS NULL
                                            AND TIPOFILAID IS NOT NULL";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, matriculaAluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["OPCAOINSCRICAOID"] != DBNull.Value ? Convert.ToInt32(reader["OPCAOINSCRICAOID"]) : 0;
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
    }
}
