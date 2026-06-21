using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class Pagamento
    {
        public bool PossuiPagamentoPeriodoPor(DataContext contexto, int PrestadorVigenciaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT COUNT(*)
                                    FROM TRANSPORTE.PRESTADORVIGENCIA V (NOLOCK)
                                          INNER JOIN TRANSPORTE.ROTATRAJETO R (NOLOCK) 
	                                                  ON  V.PRESTADORID = R.PRESTADORID
                                          INNER JOIN TRANSPORTE.PAGAMENTOROTA PGR (NOLOCK) 
	                                                  ON R.ROTAID = PGR.ROTAID
										  INNER JOIN TRANSPORTE.PAGAMENTO PG (NOLOCK) 
	                                                  ON PGR.PAGAMENTOID = PG.PAGAMENTOID
                                    WHERE PRESTADORVIGENCIAID = @PRESTADORVIGENCIAID
                                           AND (
													(V.DATAINICIO <= CONVERT(DATE, PG.DATAINICIO) AND V.DATAFIM >= CONVERT(DATE, PG.DATAFIM))
													OR (V.DATAINICIO BETWEEN  CONVERT(DATE, PG.DATAINICIO) AND  CONVERT(DATE, PG.DATAFIM))
													OR (V.DATAFIM BETWEEN CONVERT(DATE, PG.DATAINICIO) AND  CONVERT(DATE, PG.DATAFIM))
			                                    ) ";

            contextQuery.Parameters.Add("@PRESTADORVIGENCIAID", SqlDbType.Int, PrestadorVigenciaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiPagamentoPeriodoPor(DataContext contexto, int rotaId, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                    FROM TRANSPORTE.PAGAMENTO PG (NOLOCK)
									      INNER JOIN TRANSPORTE.PAGAMENTOROTA PGR (NOLOCK) ON PG.PAGAMENTOID = PGR.PAGAMENTOID
                                    WHERE ROTAID = @ROTAID
                                          AND (
	                                                (@DATAINICIO <= CONVERT(DATE, DATAINICIO) AND @DATAFIM >= CONVERT(DATE, DATAFIM))
	                                                OR (@DATAINICIO BETWEEN  CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
                                                    OR (@DATAFIM BETWEEN CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
                                                 ) ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim.Date);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiPagamentoPeriodoPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                    FROM TRANSPORTE.PAGAMENTO PG (NOLOCK)
									      INNER JOIN TRANSPORTE.PAGAMENTOROTA PGR (NOLOCK) ON PG.PAGAMENTOID = PGR.PAGAMENTOID
                                    WHERE CENSO = @CENSO
                                          AND (
	                                                (@DATAINICIO <= CONVERT(DATE, DATAINICIO) AND @DATAFIM >= CONVERT(DATE, DATAFIM))
	                                                OR (@DATAINICIO BETWEEN  CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
                                                    OR (@DATAFIM BETWEEN CONVERT(DATE, DATAINICIO) AND  CONVERT(DATE, DATAFIM))
                                                 ) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim.Date);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT PG.PAGAMENTOID,
									           PG.CENSO,
									           PG.QUANTIDADEDIAS,
									           PG.VALORTOTAL,
      								           PG.DATAINICIO,
									           PG.DATAFIM,
									           PG.DATACADASTRO AS DATAGERACAOPAGAMENTO
                                        FROM  Transporte.PAGAMENTO PG  (NOLOCK) 
                                        WHERE CENSO = @CENSO
								        ORDER BY DATAINICIO DESC ";

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

        public DataTable ListaPagamentoRotaPor(string censo, string municipio, DateTime dataInicio, DateTime dataFim, int quandidadeDiasLetivos)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            RotaAluno rnRotaAluno = new RotaAluno();
            Veiculo rnVeiculo = new Veiculo();
            Condutor rnCondutor = new Condutor();
            Prestador rnPrestador = new Prestador();
            PrestadorVigencia rnPrestadorVigencia = new PrestadorVigencia();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            CondutorBloqueio rnCondutorBloqueio = new CondutorBloqueio();
            VeiculoBloqueio rnVeiculoBloqueio = new VeiculoBloqueio();
            PrestadorBloqueio rnPrestadorBloqueio = new PrestadorBloqueio();
            PagamentoRota rnPagamentoRota = new PagamentoRota();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT R.ROTAID, 
                                                'LIBERADO'                                   AS SITUACAO, 
                                                R.CENSO + '-' + CONVERT(VARCHAR(3), R.ORDEM) AS CODIGO, 
                                                T.DESCRICAO                                  AS TURNO, 
                                                CP.DESCRICAO                                 AS TIPO, 
                                                IDA.ROTATRAJETOID                            AS ROTATRAJETOIDIDA, 
                                                CIDA.DESCRICAO                               AS TIPOCONTRATACAODESCRICAIDA, 
                                                0                                            AS QUANTIDADEALUNOIDA, 
                                                IDA.VALORROTA                                AS VALORROTAIDA, 
                                                IDA.QUANTIDADEKM                             AS QUANTIDADEKMIDA, 
                                                VOL.ROTATRAJETOID                            AS ROTATRAJETOIDVOLTA, 
                                                CVOL.DESCRICAO                               AS TIPOCONTRATACAODESCRICAVOLTA, 
                                                CIDA.DESCRICAO + ' - ' + CVOL.DESCRICAO      AS TIPOCONTRATACAO, 
                                                0                                            AS QUANTIDADEALUNOVOLTA, 
                                                VOL.VALORROTA                                AS VALORROTAVOLTA, 
                                                VOL.QUANTIDADEKM                             AS QUANTIDADEKMVOLTA, 
                                                CONVERT(DECIMAL(10, 2), 0)                   AS VALORCALCULADOIDA, 
                                                CONVERT(DECIMAL(10, 2), 0)                   AS VALORCALCULADOVOLTA, 
                                                CONVERT(DECIMAL(10, 2), NULL)                AS DESCONTO, 
                                                CONVERT(DECIMAL(10, 2), 0)                   AS VALORCALCULADOTOTAL, 
                                                CONVERT(INTEGER, NULL)                       AS PAGAMENTOID, 
                                                CONVERT(DATETIME, NULL)                      AS DATAINICIO, 
                                                CONVERT(DATETIME, NULL)                      AS DATAFIM, 
                                                0                                            AS QUANTIDADEDIAS, 
                                                0                                            AS QUANTIDADEDIASVOLTA, 
                                                0                                            AS QUANTIDADEDIASIDA, 
                                                ''                                           AS MOTIVO, 
                                                IDA.TIPOCONTRATACAOID                        AS TIPOCONTRATACAOIDIDA, 
                                                VOL.TIPOCONTRATACAOID                        AS TIPOCONTRATACAOIDVOLTA, 
                                                IDA.VEICULOID                                AS VEICULOIDIDA, 
                                                VOL.VEICULOID                                AS VEICULOIDVOLTA, 
                                                IDA.CONDUTORID                               AS CONDUTORIDIDA, 
                                                VOL.CONDUTORID                               AS CONDUTORIDVOLTA, 
                                                IDA.PRESTADORID                              AS PRESTADORIDIDA, 
                                                VOL.PRESTADORID                              AS PRESTADORIDVOLTA 
                                FROM   [TRANSPORTE].[ROTA] R (NOLOCK) 
                                       INNER JOIN TRANSPORTE.TIPOCALCULOPAGAMENTO CP (NOLOCK) 
                                               ON CP.TIPOCALCULOPAGAMENTOID = R.TIPOCALCULOPAGAMENTOID 
                                       INNER JOIN LY_TURNO T (NOLOCK) 
                                               ON R.TURNO = T.TURNO 
                                       INNER JOIN [TRANSPORTE].[ROTATRAJETO] IDA (NOLOCK) 
                                               ON R.ROTAID = IDA.ROTAID 
                                                  AND IDA.IDA = 1 
                                       INNER JOIN [TRANSPORTE].[ROTATRAJETO] VOL (NOLOCK) 
                                               ON R.ROTAID = VOL.ROTAID 
                                                  AND VOL.IDA = 0 
                                       INNER JOIN [TRANSPORTE].TIPOCONTRATACAO CIDA (NOLOCK) 
                                               ON CIDA.TIPOCONTRATACAOID = IDA.TIPOCONTRATACAOID 
                                       INNER JOIN [TRANSPORTE].TIPOCONTRATACAO CVOL (NOLOCK) 
                                               ON CVOL.TIPOCONTRATACAOID = VOL.TIPOCONTRATACAOID 
                                WHERE  CENSO = @CENSO 
                                       AND R.ATIVO = 1 
                                       AND APROVADO = 1 
                                ORDER  BY SITUACAO  ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                dt = contexto.GetDataTable(contextQuery);

                foreach (DataRow item in dt.Rows)
                {
                    List<string> bloqueios = new List<string>();
                    List<string> desativados = new List<string>();
                    List<DateTime> diasBloqueadosVeiculoIda = new List<DateTime>();
                    List<DateTime> diasBloqueadosVeiculoVolta = new List<DateTime>();
                    List<DateTime> diasBloqueadosCondutorIda = new List<DateTime>();
                    List<DateTime> diasBloqueadosCondutorVolta = new List<DateTime>();
                    List<DateTime> diasBloqueadosPrestadorIda = new List<DateTime>();
                    List<DateTime> diasBloqueadosPrestadorVolta = new List<DateTime>();
                    List<DateTime> diasSemVigenciaIda = new List<DateTime>();
                    List<DateTime> diasSemVigenciaVolta = new List<DateTime>();
                    List<DateTime> diasBloqueadosIda = new List<DateTime>();
                    List<DateTime> diasBloqueadosVolta = new List<DateTime>();
                    List<DateTime> diasLetivos = new List<DateTime>();
                    int quandidadeDiasIda = 0;
                    int quandidadeDiasVolta = 0;

                    //Verifica se ja tem quantidade de alunos
                    item["QUANTIDADEALUNOIDA"] = rnRotaAluno.ObtemAlunosAtivosPor(Convert.ToInt32(item["ROTATRAJETOIDIDA"]), dataFim);
                    item["QUANTIDADEALUNOVOLTA"] = rnRotaAluno.ObtemAlunosAtivosPor(Convert.ToInt32(item["ROTATRAJETOIDVOLTA"]), dataFim);

                    item["QUANTIDADEDIAS"] = quandidadeDiasLetivos;

                    //Verifica desativados
                    if (!rnVeiculo.EhAtivoPor(contexto, Convert.ToInt32(item["VEICULOIDIDA"]))
                        || !rnVeiculo.EhAtivoPor(contexto, Convert.ToInt32(item["VEICULOIDVOLTA"])))
                    {
                        desativados.Add("VEICULO");
                    }

                    if (!rnCondutor.EhAtivoPor(contexto, Convert.ToInt32(item["CONDUTORIDIDA"]))
                        || !rnCondutor.EhAtivoPor(contexto, Convert.ToInt32(item["CONDUTORIDVOLTA"])))
                    {
                        desativados.Add("CONDUTOR");
                    }

                    if (!rnPrestador.EhAtivoPor(contexto, Convert.ToInt32(item["PRESTADORIDIDA"]))
                        || !rnPrestador.EhAtivoPor(contexto, Convert.ToInt32(item["PRESTADORIDVOLTA"])))
                    {
                        desativados.Add("PRESTADOR");
                    }

                    //Verifica se está ativo
                    if (desativados.Count > 0)
                    {
                        //Caso esteja desativado não havera dias para pagamento
                        item["SITUACAO"] = string.Format("{0} DESATIVADO(s)", desativados.Aggregate((x, y) => x + ", " + y));
                        item["QUANTIDADEDIASIDA"] = 0;
                        item["QUANTIDADEDIASVOLTA"] = 0;
                    }
                    else
                    {
                        //Verificar os bloqueios de veiculos
                        diasBloqueadosVeiculoIda.AddRange(rnVeiculoBloqueio.RetornaDiasBloqueiosPor(contexto, Convert.ToInt32(item["VEICULOIDIDA"]), dataInicio, dataFim));
                        diasBloqueadosVeiculoVolta.AddRange(rnVeiculoBloqueio.RetornaDiasBloqueiosPor(contexto, Convert.ToInt32(item["VEICULOIDVOLTA"]), dataInicio, dataFim));

                        //Verificar os bloqueios de condutor
                        diasBloqueadosCondutorIda.AddRange(rnCondutorBloqueio.RetornaDiasBloqueiosPor(contexto, Convert.ToInt32(item["CONDUTORIDIDA"]), dataInicio, dataFim));
                        diasBloqueadosCondutorVolta.AddRange(rnCondutorBloqueio.RetornaDiasBloqueiosPor(contexto, Convert.ToInt32(item["CONDUTORIDVOLTA"]), dataInicio, dataFim));

                        //Verificar os bloqueios de prestador
                        diasBloqueadosPrestadorIda.AddRange(rnPrestadorBloqueio.RetornaDiasBloqueiosPor(contexto, Convert.ToInt32(item["PRESTADORIDIDA"]), dataInicio, dataFim));
                        diasBloqueadosPrestadorVolta.AddRange(rnPrestadorBloqueio.RetornaDiasBloqueiosPor(contexto, Convert.ToInt32(item["PRESTADORIDVOLTA"]), dataInicio, dataFim));

                        //Verificar vigencia do prestador
                        diasSemVigenciaIda.AddRange(rnPrestadorVigencia.RetornaDiasSemVigenciaPor(contexto, Convert.ToInt32(item["PRESTADORIDIDA"]), censo, dataInicio, dataFim));
                        diasSemVigenciaVolta.AddRange(rnPrestadorVigencia.RetornaDiasSemVigenciaPor(contexto, Convert.ToInt32(item["PRESTADORIDVOLTA"]), censo, dataInicio, dataFim));

                        if (diasBloqueadosVeiculoIda.Count > 0 || diasBloqueadosVeiculoVolta.Count > 0)
                        {
                            bloqueios.Add("VEICULO");
                        }

                        if (diasBloqueadosCondutorIda.Count > 0 || diasBloqueadosCondutorVolta.Count > 0)
                        {
                            bloqueios.Add("CONDUTOR");
                        }

                        if (diasBloqueadosPrestadorIda.Count > 0 || diasBloqueadosPrestadorVolta.Count > 0)
                        {
                            bloqueios.Add("PRESTADOR");
                        }

                        if (diasSemVigenciaIda.Count > 0 || diasSemVigenciaVolta.Count > 0)
                        {
                            bloqueios.Add("PRESTADOR SEM VIGÊNCIA");
                        }

                        //Verifica se possui algum bloqueio
                        if (bloqueios.Count > 0)
                        {
                            item["SITUACAO"] = string.Format("BLOQUEIO(s): {0}", bloqueios.Aggregate((x, y) => x + ", " + y));

                            //Busca dias letivos
                            diasLetivos = rnDiasNaoLetivos.RetornaDiasLetivosPor(contexto, municipio, dataInicio, dataFim);

                            //Busca todos os bloqueios de ida
                            diasBloqueadosIda.AddRange(diasBloqueadosVeiculoIda);
                            diasBloqueadosIda.AddRange(diasBloqueadosCondutorIda);
                            diasBloqueadosIda.AddRange(diasBloqueadosPrestadorIda);
                            diasBloqueadosIda.AddRange(diasSemVigenciaIda);

                            if (diasBloqueadosIda.Count > 0)
                            {
                                //Calcula dias da rota
                                foreach (DateTime dia in diasLetivos)
                                {
                                    if (!diasBloqueadosIda.Contains(dia))
                                    {
                                        quandidadeDiasIda++;
                                    }
                                }
                            }
                            else
                            {
                                quandidadeDiasIda = quandidadeDiasLetivos;
                            }

                            //Verifica dias uteis para cada rota ida
                            diasBloqueadosVolta.AddRange(diasBloqueadosVeiculoVolta);
                            diasBloqueadosVolta.AddRange(diasBloqueadosCondutorVolta);
                            diasBloqueadosVolta.AddRange(diasBloqueadosPrestadorVolta);
                            diasBloqueadosVolta.AddRange(diasSemVigenciaVolta);

                            if (diasBloqueadosVolta.Count > 0)
                            {
                                //Calcula dias da rota
                                foreach (DateTime dia in diasLetivos)
                                {
                                    if (!diasBloqueadosVolta.Contains(dia))
                                    {
                                        quandidadeDiasVolta++;
                                    }
                                }
                            }
                            else
                            {
                                quandidadeDiasVolta = quandidadeDiasLetivos;
                            }
                        }
                        else
                        {
                            //Caso não possua serão as mesmas quantidades de dias letivos
                            quandidadeDiasIda = quandidadeDiasLetivos;
                            quandidadeDiasVolta = quandidadeDiasLetivos;
                        }
                    }

                    item["QUANTIDADEDIASIDA"] = quandidadeDiasIda;
                    item["QUANTIDADEDIASVOLTA"] = quandidadeDiasVolta;

                    //Busca dados de ida
                    int tipoContratacaoIda = Convert.ToInt32(item["TIPOCONTRATACAOIDIDA"]);
                    decimal valorRotaIda = Convert.ToDecimal(item["VALORROTAIDA"]);
                    decimal quantidadeKmIda = Convert.ToDecimal(item["QUANTIDADEKMIDA"]);
                    int quantidadeAlunoIda = Convert.ToInt32(item["QUANTIDADEALUNOIDA"]);

                    //Calcula valor ida
                    decimal valorCalcudadoIda = rnPagamentoRota.CalculaValor(tipoContratacaoIda, valorRotaIda, quantidadeKmIda, quandidadeDiasIda);
                    item["VALORCALCULADOIDA"] = valorCalcudadoIda;

                    //Busca dados de volta
                    int tipoContratacaoVolta = Convert.ToInt32(item["TIPOCONTRATACAOIDVOLTA"]);
                    decimal valorRotaVolta = Convert.ToDecimal(item["VALORROTAVOLTA"]);
                    decimal quantidadeKmVolta = Convert.ToDecimal(item["QUANTIDADEKMVOLTA"]);
                    int quantidadeAlunoVolta = Convert.ToInt32(item["QUANTIDADEALUNOVOLTA"]);

                    //Calcula valor volta
                    decimal valorCalcudadoVolta = rnPagamentoRota.CalculaValor(tipoContratacaoVolta, valorRotaVolta, quantidadeKmVolta, quandidadeDiasVolta);
                    item["VALORCALCULADOVOLTA"] = valorCalcudadoVolta;

                    //Calcula valor TOTAL
                    item["VALORCALCULADOTOTAL"] = valorCalcudadoIda + valorCalcudadoVolta;
                }
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

        public DataTable ListaPagamentoRotaPor(int pagamentoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            PagamentoRota rnPagamentoRota = new PagamentoRota();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT R.ROTAID, 
                                       'PAGAMENTO GERADO'                           AS SITUACAO, 
                                       R.CENSO + '-' + CONVERT(VARCHAR(3), R.ORDEM) AS CODIGO, 
                                       T.DESCRICAO                                  AS TURNO, 
                                       CP.DESCRICAO                                 AS TIPO, 
                                       IDA.ROTATRAJETOID                            AS ROTATRAJETOIDIDA, 
                                       CIDA.DESCRICAO                               AS TIPOCONTRATACAODESCRICAIDA, 
                                       QUANTIDADEALUNOIDA, 
                                       VALORROTAIDA, 
                                       QUANTIDADEKMIDA, 
                                       VOL.ROTATRAJETOID                            AS ROTATRAJETOIDVOLTA, 
                                       CVOL.DESCRICAO                               AS TIPOCONTRATACAODESCRICAVOLTA, 
									   CIDA.DESCRICAO + ' - ' + CVOL.DESCRICAO      AS TIPOCONTRATACAO,
                                       QUANTIDADEALUNOVOLTA, 
                                       VALORROTAVOLTA, 
                                       QUANTIDADEKMVOLTA, 
                                       CONVERT(DECIMAL(10, 2), 0)                   AS VALORCALCULADOIDA, 
                                       CONVERT(DECIMAL(10, 2), 0)                   AS VALORCALCULADOVOLTA, 
                                       DESCONTO                                     AS DESCONTO, 
                                       CONVERT(DECIMAL(10, 2), 0)                   AS VALORCALCULADOTOTAL, 
                                       PG.PAGAMENTOID, 
                                       PG.DATAINICIO, 
                                       PG.DATAFIM, 
                                       PG.QUANTIDADEDIAS,
                                       PGR.QUANTIDADEDIASVOLTA,
                                       PGR.QUANTIDADEDIASIDA,                                       
                                       SPG.SITUACAOPAGAMENTOID                      AS MOTIVO, 
                                       IDA.TIPOCONTRATACAOID                        AS TIPOCONTRATACAOIDIDA, 
                                       VOL.TIPOCONTRATACAOID                        AS TIPOCONTRATACAOIDVOLTA, 
                                       IDA.VEICULOID                                AS VEICULOIDIDA, 
                                       VOL.VEICULOID                                AS VEICULOIDVOLTA, 
                                       IDA.CONDUTORID                               AS CONDUTORIDIDA, 
                                       VOL.CONDUTORID                               AS CONDUTORIDVOLTA, 
                                       IDA.PRESTADORID                              AS PRESTADORIDIDA, 
                                       VOL.PRESTADORID                              AS PRESTADORIDVOLTA 
                                FROM   TRANSPORTE.PAGAMENTO PG (NOLOCK) 
                                       INNER JOIN TRANSPORTE.PAGAMENTOROTA PGR (NOLOCK) 
                                               ON PGR.PAGAMENTOID = PG.PAGAMENTOID 
                                       INNER JOIN [TRANSPORTE].[ROTA] R (NOLOCK) 
                                               ON PGR.ROTAID = R.ROTAID 
                                       INNER JOIN TRANSPORTE.TIPOCALCULOPAGAMENTO CP (NOLOCK) 
                                               ON CP.TIPOCALCULOPAGAMENTOID = R.TIPOCALCULOPAGAMENTOID 
                                       INNER JOIN LY_TURNO T (NOLOCK) 
                                               ON R.TURNO = T.TURNO 
                                       INNER JOIN [TRANSPORTE].[ROTATRAJETO] IDA (NOLOCK) 
                                               ON R.ROTAID = IDA.ROTAID 
                                                  AND IDA.IDA = 1 
                                       INNER JOIN [TRANSPORTE].[ROTATRAJETO] VOL (NOLOCK) 
                                               ON R.ROTAID = VOL.ROTAID 
                                                  AND VOL.IDA = 0 
                                       INNER JOIN [TRANSPORTE].TIPOCONTRATACAO CIDA (NOLOCK) 
                                               ON CIDA.TIPOCONTRATACAOID = IDA.TIPOCONTRATACAOID 
                                       INNER JOIN [TRANSPORTE].TIPOCONTRATACAO CVOL (NOLOCK) 
                                               ON CVOL.TIPOCONTRATACAOID = VOL.TIPOCONTRATACAOID 
                                       INNER JOIN TRANSPORTE.SITUACAOPAGAMENTO SPG (NOLOCK) 
                                               ON SPG.SITUACAOPAGAMENTOID = PGR.SITUACAOPAGAMENTOID 
                                WHERE PG.PAGAMENTOID = @PAGAMENTOID 
                                ORDER  BY R.ORDEM ";

                contextQuery.Parameters.Add("@PAGAMENTOID", SqlDbType.Int, pagamentoId);

                dt = contexto.GetDataTable(contextQuery);

                foreach (DataRow item in dt.Rows)
                {
                    int quandidadeDiasLetivos = Convert.ToInt32(item["QUANTIDADEDIAS"]);

                    //Busca dados de ida
                    int tipoContratacaoIda = Convert.ToInt32(item["TIPOCONTRATACAOIDIDA"]);
                    decimal valorRotaIda = Convert.ToDecimal(item["VALORROTAIDA"]);
                    decimal quantidadeKmIda = Convert.ToDecimal(item["QUANTIDADEKMIDA"]);
                    int quantidadeAlunoIda = Convert.ToInt32(item["QUANTIDADEALUNOIDA"]);

                    //Calcula valor ida
                    decimal valorCalcudadoIda = rnPagamentoRota.CalculaValor(tipoContratacaoIda, valorRotaIda, quantidadeKmIda, quandidadeDiasLetivos);
                    item["VALORCALCULADOIDA"] = valorCalcudadoIda;

                    //Busca dados de volta
                    int tipoContratacaoVolta = Convert.ToInt32(item["TIPOCONTRATACAOIDVOLTA"]);
                    decimal valorRotaVolta = Convert.ToDecimal(item["VALORROTAVOLTA"]);
                    decimal quantidadeKmVolta = Convert.ToDecimal(item["QUANTIDADEKMVOLTA"]);
                    int quantidadeAlunoVolta = Convert.ToInt32(item["QUANTIDADEALUNOVOLTA"]);

                    //Calcula valor volta
                    decimal valorCalcudadoVolta = rnPagamentoRota.CalculaValor(tipoContratacaoVolta, valorRotaVolta, quantidadeKmVolta, quandidadeDiasLetivos);
                    item["VALORCALCULADOVOLTA"] = valorCalcudadoVolta;

                    //Calcula valor TOTAL
                    item["VALORCALCULADOTOTAL"] = valorCalcudadoIda + valorCalcudadoVolta;
                }
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

        public Entidades.Pagamento ObtemPor(int pagamentoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            Entidades.Pagamento pagamento = new Entidades.Pagamento();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                            FROM TRANSPORTE.PAGAMENTO (NOLOCK)
                                            WHERE PAGAMENTOID = @PAGAMENTOID ";

                contextQuery.Parameters.Add("@PAGAMENTOID", SqlDbType.Int, pagamentoId);

                pagamento = contexto.TryToBindEntity<Entidades.Pagamento>(contextQuery);

                return pagamento;
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

        public ValidacaoDados ValidaGeracaoPagamento(Entidades.Pagamento pagamento, List<Entidades.PagamentoRota> listaPagamentosRota)
        {
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            RotaTrajeto rnRotaTrajeto = new RotaTrajeto();
            Rota rnRota = new Rota();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pagamento == null)
            {
                return validacaoDados;
            }

            if (listaPagamentosRota == null)
            {
                return validacaoDados;
            }

            if (listaPagamentosRota.Count <= 0)
            {
                mensagens.Add("Não existem pagamentos para serem gerados.");
            }

            if (pagamento.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (pagamento.ValorTotal < 0)
            {
                mensagens.Add("Campo VALOR TOTAL é deve ser maior ou igual a 0.");
            }

            if (pagamento.QuantidadeDias < 0)
            {
                mensagens.Add("Campo QUANDIDADE DE DIAS é obrigatório.");
            }

            if (pagamento.DataInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INICIO é obrigatório.");
            }

            if (pagamento.DataFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }
            else
            {
                if (pagamento.DataInicio != DateTime.MinValue)
                {
                    if (pagamento.DataInicio.Date > pagamento.DataFim.Date)
                    {
                        mensagens.Add("A DATA INICIO do pagamento deve ser menor ou igual a DATA FIM.");
                    }

                    if (pagamento.DataInicio.Month != pagamento.DataFim.Month)
                    {
                        mensagens.Add("A DATA INICIO e a DATA FIM do pagamento devem ser do mesmo mês.");
                    }
                }
            }

            if (pagamento.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            //Percorre todos as rotas de pagamentos
            foreach (Entidades.PagamentoRota pagamentoRota in listaPagamentosRota)
            {
                if (pagamentoRota.RotaId <= 0)
                {
                    mensagens.Add("Campo CODIGO da ROTA é obrigatório.");
                }
                else
                {
                    //Busca codigo de idencificação da rota
                    string codigo = rnRota.ObtemCodigoPor(pagamentoRota.RotaId);

                    //Verifica se a rota está repetida
                    if (listaPagamentosRota.Where(x => x.RotaId == pagamentoRota.RotaId).Count() > 1)
                    {
                        mensagens.Add(string.Format("A rota {0} está repetida.", codigo));
                    }

                    if (pagamentoRota.SituacaoPagamentoId <= 0)
                    {
                        mensagens.Add(string.Format("Campo MOTIVO da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.QuantidadeDiasIda < 0)
                    {
                        mensagens.Add(string.Format("Campo QUANDIDADE DE DIAS DA IDA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.QuantidadeDiasVolta < 0)
                    {
                        mensagens.Add(string.Format("Campo QUANDIDADE DE DIAS DA VOLTA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.QuantidadeAlunoIda < 0)
                    {
                        mensagens.Add(string.Format("Campo QUANTIDADE DE ALUNO DA IDA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.QuantidadeAlunoVolta < 0)
                    {
                        mensagens.Add(string.Format("Campo QUANTIDADE DE ALUNO DA VOLTA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.QuantidadeKmIda <= 0)
                    {
                        mensagens.Add(string.Format("Campo QUANTIDADE DE KM DA IDA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.QuantidadeKmVolta <= 0)
                    {
                        mensagens.Add(string.Format("Campo QUANTIDADE DE KM DA VOLTA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.ValorRotaIda < 0)
                    {
                        mensagens.Add(string.Format("Campo VALOR ROTA DA IDA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.ValorRotaVolta < 0)
                    {
                        mensagens.Add(string.Format("Campo VALOR ROTA DA VOLTA da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.Desconto < 0)
                    {
                        mensagens.Add(string.Format("Campo DESCONTO da rota {0} é obrigatório.", codigo));
                    }

                    if (pagamentoRota.ValorTotal < 0)
                    {
                        mensagens.Add(string.Format("Campo TOTAL A PAGAR da rota {0} deve ser positivo.", codigo));
                    }
                }
            }

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                //Verifica se já existe pagamento no periodo
                if (this.PossuiPagamentoPeriodoPor(contexto, pagamento.Censo, pagamento.DataInicio, pagamento.DataFim))
                {
                    mensagens.Add("Já existe um pagamento cadastrado neste periodo para esta escola.");
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

        public void GerarPagamento(Entidades.Pagamento pagamento, List<Entidades.PagamentoRota> listaPagamentosRota)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            PagamentoRota rnPagamentoRota = new PagamentoRota();

            try
            {
                //Insere pagamento
                this.Insere(contexto, pagamento);

                foreach (Entidades.PagamentoRota pagamentoRota in listaPagamentosRota)
                {
                    //Atualiza com id gerado
                    pagamentoRota.PagamentoId = pagamento.PagamentoId;
                    pagamentoRota.UsuarioId = pagamento.UsuarioId;

                    //Insere rota do pagamento
                    rnPagamentoRota.Insere(contexto, pagamentoRota);
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

        private void Insere(DataContext contexto, Entidades.Pagamento pagamento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO TRANSPORTE.PAGAMENTO 
                                                (CENSO, 
                                                 QUANTIDADEDIAS, 
                                                 VALORTOTAL, 
                                                 DATAINICIO, 
                                                 DATAFIM, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@CENSO, 
                                                 @QUANTIDADEDIAS, 
                                                 @VALORTOTAL, 
                                                 @DATAINICIO, 
                                                 @DATAFIM, 
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO) 

                                    SELECT IDENT_CURRENT('Transporte.PAGAMENTO') ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, pagamento.Censo);
            contextQuery.Parameters.Add("@QUANTIDADEDIAS", SqlDbType.Int, pagamento.QuantidadeDias);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, pagamento.DataInicio.Date);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, pagamento.DataFim.Date);
            contextQuery.Parameters.Add("@VALORTOTAL", SqlDbType.Decimal, pagamento.ValorTotal);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, pagamento.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            pagamento.PagamentoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }
    }
}
