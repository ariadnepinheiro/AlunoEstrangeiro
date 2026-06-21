using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.Protocolo
{
    public class ProtocoloPrestacao
    {
        public DataTable ListaProtocoloComUltimaSituacaoPor(int regionalId, string unidadeEnsinoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable resultado = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@"   --CRIA TABELA
                                SELECT P.PROTOCOLOPRESTACAOID, 
                                       CONVERT(DATETIME, NULL) AS DATASITUACAO, 
	                                   CONVERT(INT, NULL) AS ANALISEID, 
	                                   CONVERT(VARCHAR(100), NULL) AS SITUACAO, 
                                       CONVERT(INT, NULL) AS SITUACAOPROTOCOLOID,
                                       CONVERT(VARCHAR(500), NULL) AS RESPONSAVEL 
                                INTO #ULTIMASITUACAO
                                FROM   PROTOCOLO.PROTOCOLOPRESTACAO P (NOLOCK) 
                         ");

                if (unidadeEnsinoId.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@"   WHERE REGIONALID = @REGIONALID
                                        AND UNIDADEENSINOID IS NULL ");

                    contextQuery.Parameters.Add("@REGIONALID", regionalId);
                }
                else
                {
                    sql.Append(@" WHERE REGIONALID IS NULL
                                        AND UNIDADEENSINOID = @UNIDADEENSINOID ");

                    contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsinoId);
                }

                sql.Append(@" 
                            --ATUALIZA DATA
                            UPDATE MD
	                            SET DATASITUACAO = (SELECT MAX(A.DATASITUACAO)
						                            FROM PROTOCOLO.ANALISE A 
						                            WHERE MD.PROTOCOLOPRESTACAOID = A.PROTOCOLOPRESTACAOID)
                            FROM #ULTIMASITUACAO MD 

                            --ATUALIZA ID
                            UPDATE MD
	                            SET ANALISEID = (SELECT MAX(A.ANALISEID)
					                            FROM PROTOCOLO.ANALISE A 
					                            WHERE MD.DATASITUACAO = A.DATASITUACAO AND MD.PROTOCOLOPRESTACAOID = A.PROTOCOLOPRESTACAOID)
                            FROM #ULTIMASITUACAO MD 

                            SELECT DISTINCT P.PROTOCOLOPRESTACAOID, 
                                    ANO, 
                                    SEMESTRE,
                                    TEMPORALIDADE, 
                                    UNIDADEENSINOID, 
                                    REGIONALID, 
                                    PROCESSO, 
                                    P.PROGRAMAPROTOCOLOID, 
                                    NUMEROFOLHAS, 
                                    P.TIPOPROTOCOLOID,
                                    T.DESCRICAO AS TIPO, 
                                    PP.DESCRICAO AS PROGRAMA, 
                                    P.OBSERVACAO, 
                                    DATAPROCESSO, 
                                    ISNULL(S.DESCRICAO, 'Aguardando Análise') AS SITUACAO, 
                                    U.NOME AS NOMEUSUARIOSISTEMA,
									A.USUARIOANALISADOR,
		                            A.USUARIOREVISOR,
		                            CONVERT(VARCHAR(500), NULL) AS NOMEUSUARIOANALISADOR,
		                            CONVERT(VARCHAR(500), NULL) AS NOMEUSUARIOREVISOR,
                                    A.SITUACAOPROTOCOLOID,
                                    P.USUARIOID,
                                    P.DATACADASTRO,
                                    P.DATAALTERACAO
                            INTO #RESULTADO
                            FROM   #ULTIMASITUACAO US 
                                   INNER JOIN PROTOCOLO.PROTOCOLOPRESTACAO P (NOLOCK) 
                                           ON US.PROTOCOLOPRESTACAOID = P.PROTOCOLOPRESTACAOID 
                                   LEFT JOIN PROTOCOLO.ANALISE A (NOLOCK) 
                                          ON US.ANALISEID = A.ANALISEID 
                                   INNER JOIN PROTOCOLO.TIPOPROTOCOLO T (NOLOCK) 
                                           ON P.TIPOPROTOCOLOID = T.TIPOPROTOCOLOID 
                                   LEFT JOIN PROTOCOLO.PROGRAMAPROTOCOLO PP (NOLOCK) 
                                          ON P.PROGRAMAPROTOCOLOID = PP.PROGRAMAPROTOCOLOID 
                                   LEFT JOIN PROTOCOLO.SITUACAOPROTOCOLO S (NOLOCK) 
                                          ON A.SITUACAOPROTOCOLOID = S.SITUACAOPROTOCOLOID 
                                   LEFT JOIN HADES..HD_USUARIO U (NOLOCK) 
                                          ON A.USUARIOSISTEMA = U.USUARIO 
								   
                            UPDATE P
                            SET NOMEUSUARIOANALISADOR = PE.NOME_COMPL
                            FROM  #RESULTADO P
                            INNER JOIN  LY_LOTACAO FA (NOLOCK) 
		                            ON P.USUARIOANALISADOR = FA.MATRICULA
                            INNER JOIN  LY_PESSOA PE (NOLOCK)
		                            ON FA.PESSOA = PE.PESSOA		
                            WHERE P.USUARIOANALISADOR IS NOT NULL

                            UPDATE P
                            SET NOMEUSUARIOREVISOR = PE.NOME_COMPL
                            FROM  #RESULTADO P
                            INNER JOIN  LY_LOTACAO FR (NOLOCK) 
		                            ON P.USUARIOREVISOR = FR.MATRICULA
                            INNER JOIN  LY_PESSOA PE (NOLOCK)
		                            ON FR.PESSOA = PE.PESSOA	
                            WHERE P.USUARIOREVISOR IS NOT NULL

                            SELECT * 
                            FROM #RESULTADO
                            ORDER  BY ANO DESC, 
                                        TIPO, 
                                        PROGRAMA

                            DROP TABLE #ULTIMASITUACAO
                            DROP TABLE #RESULTADO ");

                contextQuery.Command = sql.ToString();

                resultado = contexto.GetDataTable(contextQuery);
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

            return resultado;
        }

        public DataTable ListaConsultaProtocoloPor(int coordenadoriaId, int regionalId, string unidadeEnsinoId, int situacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable resultado = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                if (coordenadoriaId > 0)
                {
                    sql.Append(@"  SELECT P.PROTOLOCOPRESTACAO_COORDENADORIAID, 
                                        ANO, 
                                        SEMESTRE, 
                                        CASE 
											WHEN SEMESTRE = 0 THEN 'Anual'
											WHEN SEMESTRE = 1 THEN 'Semestral'
											WHEN SEMESTRE = 2 THEN 'Semestral'
											else null
                                        END TEMPORALIDADE,
                                        p.NUCLEO, 
		                                N.DESCRICAO AS COORDENADORIA,
                                        PROCESSO, 
                                        P.PROGRAMAPROTOCOLOID, 
                                        NUMEROFOLHAS, 
                                        P.TIPOPROTOCOLOID,
                                        T.DESCRICAO AS TIPO, 
                                        PP.DESCRICAO AS PROGRAMA, 
                                        P.OBSERVACAO, 
                                        DATAPROCESSO, 
                                        S.DESCRICAO AS SITUACAO, 
		                                P.DETALHE,
		                                p.DATAEXIGENCIA,
		                                p.DATATOMADACONTAS,
		                                p.SITUACAOTOMADACONTAS,
                                        p.DATAAPROVACAO,
                                        P.ANALIADOR,
                                        P.DATACADASTRO,
                                        P.DATAALTERACAO
                                FROM PROTOCOLO.PROTOLOCOPRESTACAO_COORDENADORIA P (NOLOCK) 
                                    INNER JOIN LY_NUCLEO N  (NOLOCK) 
                                            ON P.NUCLEO = N.NUCLEO 
                                    INNER JOIN PROTOCOLO.TIPOPROTOCOLO T (NOLOCK) 
                                            ON P.TIPOPROTOCOLOID = T.TIPOPROTOCOLOID 
                                    LEFT JOIN PROTOCOLO.PROGRAMAPROTOCOLO PP (NOLOCK) 
                                            ON P.PROGRAMAPROTOCOLOID = PP.PROGRAMAPROTOCOLOID 
                                    INNER JOIN PROTOCOLO.SITUACAOPROTOCOLO S (NOLOCK) 
                                            ON p.SITUACAOPROTOCOLOID = S.SITUACAOPROTOCOLOID 
                                WHERE P.NUCLEO = @COORDENADORIAID
                                ORDER  BY P.ANO DESC, 
                                        T.DESCRICAO, 
                                        PP.DESCRICAO ");

                    contextQuery.Parameters.Add("@COORDENADORIAID", coordenadoriaId);
                }
                else
                {
                    sql.Append(@"   --CRIA TABELA
                                SELECT P.PROTOCOLOPRESTACAOID, 
                                       CONVERT(DATETIME, NULL) AS DATASITUACAO, 
	                                   CONVERT(INT, NULL) AS ANALISEID, 
	                                   CONVERT(VARCHAR(100), NULL) AS SITUACAO, 
                                       CONVERT(INT, NULL) AS SITUACAOPROTOCOLOID,
                                       CONVERT(VARCHAR(500), NULL) AS RESPONSAVEL 
                                INTO #ULTIMASITUACAO
                                FROM   PROTOCOLO.PROTOCOLOPRESTACAO P (NOLOCK) 
                         ");

                    if (!unidadeEnsinoId.IsNullOrEmptyOrWhiteSpace())
                    {
                        sql.Append(@" WHERE REGIONALID IS NULL
                                        AND UNIDADEENSINOID = @UNIDADEENSINOID ");

                        contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsinoId);
                    }
                    else if (regionalId > 0)
                    {
                        sql.Append(@"   WHERE REGIONALID = @REGIONALID
                                        AND UNIDADEENSINOID IS NULL ");

                        contextQuery.Parameters.Add("@REGIONALID", regionalId);
                    }

                    sql.Append(@" 
                            --ATUALIZA DATA
                            UPDATE MD
	                            SET DATASITUACAO = (SELECT MAX(A.DATASITUACAO)
						                            FROM PROTOCOLO.ANALISE A 
						                            WHERE MD.PROTOCOLOPRESTACAOID = A.PROTOCOLOPRESTACAOID)
                            FROM #ULTIMASITUACAO MD 

                            --ATUALIZA ID
                            UPDATE MD
	                            SET ANALISEID = (SELECT MAX(A.ANALISEID)
					                            FROM PROTOCOLO.ANALISE A 
					                            WHERE MD.DATASITUACAO = A.DATASITUACAO AND MD.PROTOCOLOPRESTACAOID = A.PROTOCOLOPRESTACAOID)
                            FROM #ULTIMASITUACAO MD 

                           SELECT DISTINCT P.PROTOCOLOPRESTACAOID, 
                                    ANO, 
                                    SEMESTRE, 
                                    TEMPORALIDADE,
                                    UNIDADEENSINOID, 
		                            UE.NOME_COMP AS ESCOLA,
		                            R.REGIONAL,
                                    REGIONALID, 
                                    PROCESSO, 
                                    P.PROGRAMAPROTOCOLOID, 
                                    NUMEROFOLHAS, 
                                    P.TIPOPROTOCOLOID,
                                    T.DESCRICAO AS TIPO, 
                                    PP.DESCRICAO AS PROGRAMA, 
                                    P.OBSERVACAO, 
                                    DATAPROCESSO, 
                                    ISNULL(S.DESCRICAO, 'Aguardando Análise') AS SITUACAO, 
									U.NOME AS NOMEUSUARIOSISTEMA,
									FA.NOME_COMPL AS NOMEUSUARIOANALISADOR,
									FR.NOME_COMPL AS NOMEUSUARIOREVISOR,
                                    A.SITUACAOPROTOCOLOID,
                                    P.USUARIOID,
                                    P.DATACADASTRO,
                                    P.DATAALTERACAO
                            FROM   #ULTIMASITUACAO US 
                                    INNER JOIN PROTOCOLO.PROTOCOLOPRESTACAO P (NOLOCK) 
                                            ON US.PROTOCOLOPRESTACAOID = P.PROTOCOLOPRESTACAOID 		
		                            LEFT JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
				                            ON P.UNIDADEENSINOID = UE.UNIDADE_ENS
		                            LEFT JOIN TCE_REGIONAL R (NOLOCK) 
				                            ON ISNULL(P.REGIONALID, UE.ID_REGIONAL) = R.ID_REGIONAL
                                    LEFT JOIN PROTOCOLO.ANALISE A (NOLOCK) 
                                            ON US.ANALISEID = A.ANALISEID 
                                    INNER JOIN PROTOCOLO.TIPOPROTOCOLO T (NOLOCK) 
                                            ON P.TIPOPROTOCOLOID = T.TIPOPROTOCOLOID 
                                    LEFT JOIN PROTOCOLO.PROGRAMAPROTOCOLO PP (NOLOCK) 
                                            ON P.PROGRAMAPROTOCOLOID = PP.PROGRAMAPROTOCOLOID 
                                    LEFT JOIN PROTOCOLO.SITUACAOPROTOCOLO S (NOLOCK) 
                                            ON A.SITUACAOPROTOCOLOID = S.SITUACAOPROTOCOLOID 
                                    LEFT JOIN HADES..HD_USUARIO U (NOLOCK) 
                                            ON A.USUARIOSISTEMA = U.USUARIO 
									LEFT JOIN  DBO.VW_FUNCIONARIOS FA (NOLOCK) 
										  ON A.USUARIOANALISADOR = FA.MATRICULA
									LEFT JOIN  DBO.VW_FUNCIONARIOS FR (NOLOCK) 
										  ON A.USUARIOREVISOR = FR.MATRICULA 
                            ");

                    if (situacaoId > 0)
                    {
                        sql.Append(@"  WHERE ISNULL(A.SITUACAOPROTOCOLOID, 2) = @SITUACAOPROTOCOLOID 
                                    ");

                        contextQuery.Parameters.Add("@SITUACAOPROTOCOLOID", situacaoId);
                    }

                    sql.Append(@"  ORDER  BY P.ANO DESC, 
                                      P.REGIONALID,
                                      P.UNIDADEENSINOID,
                                      T.DESCRICAO, 
                                      PP.DESCRICAO 

                            DROP TABLE #ULTIMASITUACAO ");
                }

                contextQuery.Command = sql.ToString();

                resultado = contexto.GetDataTable(contextQuery);
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

            return resultado;
        }

        public DTOs.DadosAnaliseProtocolo ObtemDadosAnaliseProtocoloPor(int protocoloId)
        {
            RN.Protocolo.Analise rnAnalise = new Analise();
            DTOs.DadosAnaliseProtocolo dados = new DTOs.DadosAnaliseProtocolo();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT P.PROTOCOLOPRESTACAOID, 
                                               P.ANO, 
                                               P.TEMPORALIDADE,
                                               P.SEMESTRE, 
                                               P.REGIONALID, 
                                               R.REGIONAL, 
                                               P.UNIDADEENSINOID, 
                                               UE.NOME_COMP AS UNIDADEENSINO, 
                                               P.PROCESSO, 
                                               P.NUMEROFOLHAS, 
                                               P.TIPOPROTOCOLOID, 
                                               P.OBSERVACAO,
                                               T.DESCRICAO  AS TIPO, 
                                               P.PROGRAMAPROTOCOLOID, 
                                               PR.DESCRICAO  AS PROGRAMAPROTOCOLO,
                                               P.DATAPROCESSO,
											   UE.CGC AS CNPJ 
                                        FROM   PROTOCOLO.PROTOCOLOPRESTACAO P (NOLOCK) 
                                               LEFT JOIN TCE_REGIONAL R (NOLOCK) 
                                                       ON R.ID_REGIONAL = P.REGIONALID 
                                               INNER JOIN PROTOCOLO.TIPOPROTOCOLO T 
                                                       ON P.TIPOPROTOCOLOID = T.TIPOPROTOCOLOID 
                                               LEFT JOIN PROTOCOLO.PROGRAMAPROTOCOLO PR 
                                                      ON P.PROGRAMAPROTOCOLOID = PR.PROGRAMAPROTOCOLOID 
                                               LEFT JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                                      ON P.UNIDADEENSINOID = UE.UNIDADE_ENS 
                                        WHERE  P.PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID  ";

                contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.ProtocoloPrestacaoId = Convert.ToInt32(reader["PROTOCOLOPRESTACAOID"]);
                    dados.Ano = Convert.ToInt32(reader["ANO"]);
                    dados.Semestre = reader["SEMESTRE"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SEMESTRE"]) : (int?)null;
                    dados.Temporalidade = reader["TEMPORALIDADE"] != DBNull.Value ? Convert.ToString(reader["TEMPORALIDADE"]) : string.Empty;
                    dados.UnidadeEnsinoId = reader["UNIDADEENSINOID"] != DBNull.Value ? Convert.ToString(reader["UNIDADEENSINOID"]) : string.Empty;
                    dados.UnidadeEnsino = reader["UNIDADEENSINO"] != DBNull.Value ? Convert.ToString(reader["UNIDADEENSINO"]) : string.Empty;
                    dados.RegionalId = reader["REGIONALID"] != DBNull.Value ? Convert.ToInt32(reader["REGIONALID"]) : 0;
                    dados.Regional = Convert.ToString(reader["REGIONAL"]);
                    dados.Processo = Convert.ToString(reader["PROCESSO"]);
                    if (reader["NUMEROFOLHAS"] != DBNull.Value)
                    {
                        dados.NumeroFolhas = Convert.ToInt32(reader["NUMEROFOLHAS"]);
                    }
                    dados.TipoProtocoloId = Convert.ToInt32(reader["TIPOPROTOCOLOID"]);
                    dados.TipoProtocolo = Convert.ToString(reader["TIPO"]);
                    dados.ProgramaProtocoloId = reader["PROGRAMAPROTOCOLOID"] != DBNull.Value ? Convert.ToInt32(reader["PROGRAMAPROTOCOLOID"]) : 0;
                    dados.ProgramaProtocolo = reader["PROGRAMAPROTOCOLO"] != DBNull.Value ? Convert.ToString(reader["PROGRAMAPROTOCOLO"]) : string.Empty;
                    dados.Observacao = reader["OBSERVACAO"] != DBNull.Value ? Convert.ToString(reader["OBSERVACAO"]) : string.Empty;
                    dados.DataProcesso = Convert.ToDateTime(reader["DATAPROCESSO"]);
                    dados.Cnpj = reader["CNPJ"] != DBNull.Value ? Convert.ToString(reader["CNPJ"]) : string.Empty;
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

        public Entidades.ProtocoloPrestacao ObtemPor(DataContext contexto, int protocoloId)
        {
            Entidades.ProtocoloPrestacao protocoloPrestacao = new Entidades.ProtocoloPrestacao();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                    FROM PROTOCOLO.PROTOCOLOPRESTACAO (NOLOCK)
                                    WHERE PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID ";

            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloId);

            protocoloPrestacao = contexto.TryToBindEntity<Entidades.ProtocoloPrestacao>(contextQuery);

            return protocoloPrestacao;
        }

        public ValidacaoDados Valida(Entidades.ProtocoloPrestacao protocoloPrestacao, bool cadastro, string inicialProcesso)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (protocoloPrestacao == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (protocoloPrestacao.ProtocoloPrestacaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }
            }

            if ((protocoloPrestacao.RegionalId == null || protocoloPrestacao.RegionalId <= 0) && protocoloPrestacao.UnidadeEnsinoId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("É obrigatório escolher uma REGIONAL ou uma UNIDADE DE ENSINO.");
            }
            else
            {
                if (!protocoloPrestacao.UnidadeEnsinoId.IsNullOrEmptyOrWhiteSpace())
                {
                    protocoloPrestacao.RegionalId = null;

                    if (protocoloPrestacao.UnidadeEnsinoId.Length != 8)
                    {
                        mensagens.Add("Campo UNIDADE ENSINO deve conter 8 caracteres.");
                    }
                }
                else
                {
                    protocoloPrestacao.UnidadeEnsinoId = null;
                }
            }

            if (protocoloPrestacao.TipoProtocoloId <= 0)
            {
                mensagens.Add("Campo TIPO DE PRESTAÇÃO é obrigatório.");
            }

            if (protocoloPrestacao.ProgramaProtocoloId == null || protocoloPrestacao.ProgramaProtocoloId <= 0)
            {
                mensagens.Add("Campo PROGRAMA é obrigatório.");
            }

            if (inicialProcesso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PREFIXO do NÚMERO DO PROCESSO é obrigatório.");
            }

            if (protocoloPrestacao.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }
            else
            {
                if (protocoloPrestacao.Ano < 1900)
                {
                    mensagens.Add("Campo ANO deve ser menor que 1900.");
                }

                if (protocoloPrestacao.Ano > DateTime.Now.Year)
                {
                    mensagens.Add("Campo ANO não pode ser maior que o ano atual.");
                }
            }

            if (protocoloPrestacao.Temporalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TEMPORALIDADE é obrigatório.");
            }
            else if (protocoloPrestacao.Temporalidade != "Anual" && protocoloPrestacao.Temporalidade != "Semestral" && protocoloPrestacao.Temporalidade != "Trimestral")
            {
                mensagens.Add("Campo TEMPORALIDADE apenas pode conter os valores 'Anual', 'Semestral' e 'Trimestral'.");
            }

            if (protocoloPrestacao.Processo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO PROCESSO é obrigatório.");
            }
            else
            {
                if (protocoloPrestacao.Processo.Length > 50)
                {
                    mensagens.Add("Campo NÚMERO DO PROCESSO deve conter no máximo 50 caracteres.");
                }
            }

            if (protocoloPrestacao.DataProcesso == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DO PROCESSO é obrigatório.");
            }
            else if (protocoloPrestacao.DataProcesso > DateTime.Now)
            {
                mensagens.Add("Campo DATA DO PROCESSO não pode ser maior que a data atual.");
            }

            if (inicialProcesso != "SEI-")
            {
                if (protocoloPrestacao.NumeroFolhas <= 0)
                {
                    mensagens.Add("Campo NÚMERO DE FOLHAS é obrigatório.");
                }
            }

            if (protocoloPrestacao.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se existe o mesmo numero cadastrado para outra prestacao
                    if (this.PossuiOutroNumeroProcessoCadastradoPor(contexto, protocoloPrestacao.RegionalId, protocoloPrestacao.UnidadeEnsinoId, protocoloPrestacao.Processo, protocoloPrestacao.ProtocoloPrestacaoId))
                    {
                        mensagens.Add("Já existe outra prestação cadastrada com este NÚMERO DE PROCESSO.");
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

        private bool PossuiOutroNumeroProcessoCadastradoPor(DataContext contexto, int? regionalId, string unidadeEnsinoId, string processo, int protocoloPrestacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            StringBuilder sql = new StringBuilder();

            sql.Append(@" SELECT COUNT(*) 
                                    FROM PROTOCOLO.PROTOCOLOPRESTACAO (NOLOCK)
                                    WHERE PROCESSO = @PROCESSO
	                                    AND PROTOCOLOPRESTACAOID <> @PROTOCOLOPRESTACAOID ");

            if (unidadeEnsinoId.IsNullOrEmptyOrWhiteSpace())
            {
                sql.Append(@"  AND REGIONALID = @REGIONALID
                               AND UNIDADEENSINOID IS NULL ");

                contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, regionalId);
            }
            else
            {
                sql.Append(@" AND REGIONALID IS NULL
                              AND UNIDADEENSINOID = @UNIDADEENSINOID ");

                contextQuery.Parameters.Add("@UNIDADEENSINOID", SqlDbType.VarChar, unidadeEnsinoId);
            }

            contextQuery.Command = sql.ToString();
            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, processo);
            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloPrestacaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Insere(Entidades.ProtocoloPrestacao protocoloPrestacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere Protocolo Prestacao
                this.Insere(contexto, protocoloPrestacao);
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

        private void Insere(DataContext contexto, Entidades.ProtocoloPrestacao protocoloPrestacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Protocolo.PROTOCOLOPRESTACAO 
                                            (ANO, 
                                             TEMPORALIDADE, 
                                             UNIDADEENSINOID,                                             
                                             REGIONALID, 
                                             PROCESSO, 
                                             NUMEROFOLHAS,                                                                           
                                             TIPOPROTOCOLOID, 
                                             PROGRAMAPROTOCOLOID,
                                             DATAPROCESSO,                                            
                                             USUARIOID, 
                                             USUARIOCADASTROID,
                                             DATACADASTRO, 
                                             DATAALTERACAO) 
                                VALUES      (@ANO, 
                                             @TEMPORALIDADE, 
                                             @UNIDADEENSINOID,                                             
                                             @REGIONALID, 
                                             @PROCESSO, 
                                             @NUMEROFOLHAS,                                                                           
                                             @TIPOPROTOCOLOID, 
                                             @PROGRAMAPROTOCOLOID,
                                             @DATAPROCESSO, 
                                             @USUARIOID,
                                             @USUARIOCADASTROID,
                                             @DATACADASTRO, 
                                             @DATAALTERACAO)
                                			
                                SELECT IDENT_CURRENT('Protocolo.PROTOCOLOPRESTACAO') ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, protocoloPrestacao.Ano);
            contextQuery.Parameters.Add("@TEMPORALIDADE", SqlDbType.VarChar, protocoloPrestacao.Temporalidade);
            contextQuery.Parameters.Add("@UNIDADEENSINOID", SqlDbType.VarChar, protocoloPrestacao.UnidadeEnsinoId);
            contextQuery.Parameters.Add("@REGIONALID", SqlDbType.Int, protocoloPrestacao.RegionalId);
            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, protocoloPrestacao.Processo);
            contextQuery.Parameters.Add("@NUMEROFOLHAS", SqlDbType.Int, protocoloPrestacao.NumeroFolhas);
            contextQuery.Parameters.Add("@TIPOPROTOCOLOID", SqlDbType.Int, protocoloPrestacao.TipoProtocoloId);
            contextQuery.Parameters.Add("@PROGRAMAPROTOCOLOID", SqlDbType.Int, protocoloPrestacao.ProgramaProtocoloId);
            contextQuery.Parameters.Add("@DATAPROCESSO", SqlDbType.Date, protocoloPrestacao.DataProcesso.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, protocoloPrestacao.UsuarioId);
            contextQuery.Parameters.Add("@USUARIOCADASTROID", SqlDbType.VarChar, protocoloPrestacao.UsuarioCadastroId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            protocoloPrestacao.ProtocoloPrestacaoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public void Atualiza(Entidades.ProtocoloPrestacao protocoloPrestacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza Protocolo Prestacao
                this.Atualiza(contexto, protocoloPrestacao);
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

        private void Atualiza(DataContext contexto, Entidades.ProtocoloPrestacao protocoloPrestacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Protocolo.PROTOCOLOPRESTACAO	 
                                SET PROCESSO = @PROCESSO,
                                    TEMPORALIDADE = @TEMPORALIDADE,
                                    ANO = @ANO,                               
                                    NUMEROFOLHAS = @NUMEROFOLHAS, 
                                    TIPOPROTOCOLOID = @TIPOPROTOCOLOID, 
                                    PROGRAMAPROTOCOLOID = @PROGRAMAPROTOCOLOID, 
                                    DATAPROCESSO = @DATAPROCESSO,                                    
                                    USUARIOID = @USUARIOID, 
                                    DATAALTERACAO = @DATAALTERACAO 
                                WHERE PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID ";

            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloPrestacao.ProtocoloPrestacaoId);
            contextQuery.Parameters.Add("@PROCESSO", SqlDbType.VarChar, protocoloPrestacao.Processo);
            contextQuery.Parameters.Add("@TEMPORALIDADE", SqlDbType.VarChar, protocoloPrestacao.Temporalidade);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, protocoloPrestacao.Ano);
            contextQuery.Parameters.Add("@NUMEROFOLHAS", SqlDbType.Int, protocoloPrestacao.NumeroFolhas);
            contextQuery.Parameters.Add("@TIPOPROTOCOLOID", SqlDbType.Int, protocoloPrestacao.TipoProtocoloId);
            contextQuery.Parameters.Add("@PROGRAMAPROTOCOLOID", SqlDbType.Int, protocoloPrestacao.ProgramaProtocoloId);
            contextQuery.Parameters.Add("@DATAPROCESSO", SqlDbType.Date, protocoloPrestacao.DataProcesso.Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, protocoloPrestacao.UsuarioId);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaDadosAnalise(DataContext contexto, int? numeroFolhas, int protocoloPrestacaoId, string observacao, string usuarioResponsavel)
        {
            StringBuilder sql = new StringBuilder();
            ContextQuery contextQuery = new ContextQuery();

            sql.Append(@" UPDATE Protocolo.PROTOCOLOPRESTACAO	 
                                        SET NUMEROFOLHAS = @NUMEROFOLHAS,
                                            OBSERVACAO = @OBSERVACAO,
                                            USUARIOID = @USUARIOID, 
                                            DATAALTERACAO = @DATAALTERACAO 
                                        WHERE PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID 
                            ");

            if (observacao.IsNullOrEmptyOrWhiteSpace())
            {
                sql.Append(@" AND (NUMEROFOLHAS <> @NUMEROFOLHAS
                                OR OBSERVACAO IS NOT NULL) ");
            }
            else
            {
                sql.Append(@" AND (NUMEROFOLHAS <> @NUMEROFOLHAS
                                OR OBSERVACAO <> @OBSERVACAO 
                                OR OBSERVACAO IS NULL) ");
            }

            contextQuery.Command = sql.ToString();

            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloPrestacaoId);
            contextQuery.Parameters.Add("@NUMEROFOLHAS", SqlDbType.Int, numeroFolhas);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, observacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAAlTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int protocoloPrestacaoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Protocolo.Analise rnAnalise = new Analise();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (protocoloPrestacaoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnAnalise.PossuiAnalisePor(contexto, protocoloPrestacaoId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois já foi analisado.");
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

        public void RemoveCoordenador(int protocoloPrestacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Protocolo.Analise rnAnalise = new Analise();
            try
            {
                if (rnAnalise.PossuiAnalisePor(contexto, protocoloPrestacaoId))
                {
                    rnAnalise.RemoveAnalise(contexto, protocoloPrestacaoId);
                }
                //Remove Protocolo Prestacao
                this.Remove(contexto, protocoloPrestacaoId);
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

        public void Remove(int protocoloPrestacaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Remove Protocolo Prestacao
                this.Remove(contexto, protocoloPrestacaoId);
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

        private void Remove(DataContext contexto, int protocoloPrestacaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Protocolo.PROTOCOLOPRESTACAO
                                        WHERE PROTOCOLOPRESTACAOID = @PROTOCOLOPRESTACAOID ";

            contextQuery.Parameters.Add("@PROTOCOLOPRESTACAOID", SqlDbType.Int, protocoloPrestacaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiTipoProtocoloPor(DataContext contexto, int tipoProtocoloId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Protocolo.PROTOCOLOPRESTACAO (NOLOCK)
                                    WHERE TIPOPROTOCOLOID = @TIPOPROTOCOLOID ";

            contextQuery.Parameters.Add("@TIPOPROTOCOLOID", tipoProtocoloId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiProgramaProtocoloPor(DataContext contexto, int programaProtocoloId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM PROTOCOLO.PROTOCOLOPRESTACAO (NOLOCK)
                                    WHERE PROGRAMAPROTOCOLOID = @PROGRAMAPROTOCOLOID ";

            contextQuery.Parameters.Add("@PROGRAMAPROTOCOLOID", programaProtocoloId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}