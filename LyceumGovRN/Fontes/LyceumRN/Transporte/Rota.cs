using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class Rota
    {
        public bool PossuiTipoCalculoPagamentoPor(DataContext contexto, int tipoCalculoPagamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.ROTA (NOLOCK)
                                    WHERE TIPOCALCULOPAGAMENTOID = @TIPOCALCULOPAGAMENTOID ";

            contextQuery.Parameters.Add("@TIPOCALCULOPAGAMENTOID", SqlDbType.Int, tipoCalculoPagamentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiCondutorOutraRotaPor(DataContext contexto, int condutorId, string turno, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.ROTA R (NOLOCK)
										 INNER JOIN Transporte.ROTATRAJETO rt (NOLOCK) 
												ON R.ROTAID = RT.ROTAID
                                    WHERE R.ATIVO = 1
										  AND CONDUTORID = @CONDUTORID
										  AND TURNO = @TURNO
										  AND R.ROTAID <> @ROTAID ";

            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiVeiculoOutraRotaPor(DataContext contexto, int veiculoId, string turno, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.ROTA R (NOLOCK)
										 INNER JOIN Transporte.ROTATRAJETO rt (NOLOCK) 
												ON R.ROTAID = RT.ROTAID
                                    WHERE R.ATIVO = 1
										  AND VEICULOID = @VEICULOID
										  AND TURNO = @TURNO
										  AND R.ROTAID <> @ROTAID ";

            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool PossuiCondutorVeiculoPrestadorPor(DataContext contexto, int rotaId, bool ida)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.ROTA R (NOLOCK)
										 INNER JOIN Transporte.ROTATRAJETO rt (NOLOCK) 
												ON R.ROTAID = RT.ROTAID
                                    WHERE R.ROTAID = @ROTAID
										  AND VEICULOID IS NOT NULL
										  AND CONDUTORID IS NOT NULL
										  AND PRESTADORID IS NOT NULL
										  AND IDA = @IDA ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@IDA", SqlDbType.Bit, ida);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public RN.DTOs.DadosRota ObtemDadosRotaPor(int rotaId)
        {
            RN.DTOs.DadosRota dados = new DadosRota();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader dataReader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT R.ROTAID, 
                                       UE.ID_REGIONAL,
                                       RE.REGIONAL,
                                       UE.MUNICIPIO,
                                       M.NOME AS MUNICIPIODESCRICAO, 
                                       R.CENSO, 
                                       RM.REGIAOFINANCEIRAID, 
                                       RF.DESCRICAO AS REGIAOFINANCEIRADESCRICAO, 
                                       UE.CGC, 
                                       R.TURNO, 
                                       R.TIPOCALCULOPAGAMENTOID, 
									   CP.DESCRICAO as TIPOCALCULOPAGAMENTO, 
                                       R.ATIVO, 
                                       R.APROVADO,
                                       R.ORDEM,
                                       R.USUARIOID, 
                                       R.DATACADASTRO, 
                                       R.DATAALTERACAO,
                                       IDA.ROTATRAJETOID AS ROTATRAJETOIDIDA,
									   IDA.PRESTADORID AS PRESTADORIDIDA, 
                                       IDA.CONDUTORID AS CONDUTORIDIDA, 
                                       IDA.VEICULOID AS VEICULOIDIDA, 
                                       IDA.TIPOCONTRATACAOID AS TIPOCONTRATACAOIDIDA, 
									   CIDA.DESCRICAO AS TIPOCONTRATACAODESCRICAIDA,
                                       IDA.VALORROTA AS VALORROTAIDA, 
									   IDA.QUANTIDADEKM AS QUANTIDADEKMIDA,
                                       IDA.TEMPO AS TEMPOIDA,
									   VOL.ROTATRAJETOID AS ROTATRAJETOIDVOLTA,
									   VOL.PRESTADORID AS PRESTADORIDVOLTA, 
                                       VOL.CONDUTORID AS CONDUTORIDVOLTA, 
                                       VOL.VEICULOID AS VEICULOIDVOLTA, 
                                       VOL.TIPOCONTRATACAOID AS TIPOCONTRATACAOIDVOLTA, 
									   CVOL.DESCRICAO AS  TIPOCONTRATACAODESCRICAVOLTA,
                                       VOL.VALORROTA AS VALORROTAVOLTA, 
									   VOL.QUANTIDADEKM AS QUANTIDADEKMVOLTA,
                                       VOL.TEMPO AS TEMPOVOLTA
                                FROM   [TRANSPORTE].[ROTA] R (NOLOCK)
									   INNER JOIN Transporte.TIPOCALCULOPAGAMENTO CP (NOLOCK) 
                                               ON CP.TIPOCALCULOPAGAMENTOID = r.TIPOCALCULOPAGAMENTOID
                                       INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                               ON R.CENSO = UE.UNIDADE_ENS 
                                       INNER JOIN TCE_REGIONAL RE (NOLOCK) 
                                               ON UE.ID_REGIONAL = RE.ID_REGIONAL 
                                       INNER JOIN HADES.DBO.TCE_MUNICIPIO M (NOLOCK) 
                                               ON UE.MUNICIPIO = M.ID_MUNICIPIO 
                                       INNER JOIN GESTAOREDE.REGIAOFINANCEIRAMUNICIPIO RM (NOLOCK) 
                                               ON M.ID_MUNICIPIO = RM.MUNICIPIOID 
                                       INNER JOIN GESTAOREDE.REGIAOFINANCEIRA RF (NOLOCK) 
                                               ON RM.REGIAOFINANCEIRAID = RF.REGIAOFINANCEIRAID 
									   INNER JOIN [Transporte].[ROTATRAJETO] IDA (NOLOCK) 
									           ON R.ROTAID = IDA.ROTAID AND IDA.IDA = 1
									   INNER JOIN [Transporte].[ROTATRAJETO] VOL (NOLOCK) 
									           ON R.ROTAID = VOL.ROTAID AND VOL.IDA = 0
									   INNER JOIN [Transporte].TIPOCONTRATACAO CIDA (NOLOCK) 
									           ON CIDA.TIPOCONTRATACAOID = IDA.TIPOCONTRATACAOID
									   INNER JOIN [Transporte].TIPOCONTRATACAO CVOL (NOLOCK) 
									           ON CVOL.TIPOCONTRATACAOID = VOL.TIPOCONTRATACAOID
                                WHERE  R.ROTAID = @ROTAID ";

                contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    dados.RotaId = Convert.ToInt32(dataReader["ROTAID"]);
                    dados.Situacao = Convert.ToBoolean(dataReader["APROVADO"]) ? "Aprovado" : "Aguardando aprovação";
                    dados.RegionalDescricao = Convert.ToString(dataReader["REGIONAL"]);
                    dados.Regional = Convert.ToInt32(dataReader["ID_REGIONAL"]);
                    dados.Municipio = Convert.ToString(dataReader["MUNICIPIO"]);
                    dados.MunicipioDescricao = Convert.ToString(dataReader["MUNICIPIODESCRICAO"]);
                    dados.Censo = Convert.ToString(dataReader["CENSO"]);
                    dados.RegiaoFinanceiraId = Convert.ToInt32(dataReader["REGIAOFINANCEIRAID"]);
                    dados.RegiaoFinanceiraDescricao = Convert.ToString(dataReader["REGIAOFINANCEIRADESCRICAO"]);
                    dados.Cnpj = Convert.ToString(dataReader["CGC"]);
                    dados.Turno = Convert.ToString(dataReader["TURNO"]);
                    dados.Ordem = Convert.ToInt32(dataReader["ORDEM"]);
                    dados.Codigo = string.Format("{0}-{1}", dados.Censo, Convert.ToString(dados.Ordem));
                    dados.TipoCalculoPagamentoId = Convert.ToInt32(dataReader["TIPOCALCULOPAGAMENTOID"]);
                    dados.TipoCalculoPagamento = Convert.ToString(dataReader["TIPOCALCULOPAGAMENTO"]);
                    dados.Ativo = Convert.ToBoolean(dataReader["ATIVO"]);
                    dados.UsuarioResponsavel = Convert.ToString(dataReader["USUARIOID"]);

                    //Dados da Ida
                    dados.RotaTrajetoIdIda = Convert.ToInt32(dataReader["ROTATRAJETOIDIDA"]);
                    dados.PrestadorIdIda = dataReader["PRESTADORIDIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["PRESTADORIDIDA"]);
                    dados.CondutorIdIda = dataReader["CONDUTORIDIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["CONDUTORIDIDA"]);
                    dados.VeiculoIdIda = dataReader["VEICULOIDIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["VEICULOIDIDA"]);
                    dados.TipoContratacaoIdIda = Convert.ToInt32(dataReader["TIPOCONTRATACAOIDIDA"]);
                    dados.TipoContratacaoDescricaoIda = Convert.ToString(dataReader["TIPOCONTRATACAODESCRICAIDA"]);
                    dados.ValorRotaIda = Convert.ToDecimal(dataReader["VALORROTAIDA"]);
                    dados.QuantidadeKmIda = Convert.ToDecimal(dataReader["QUANTIDADEKMIDA"]);
                    dados.TempoIda = dataReader["TEMPOIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["TEMPOIDA"]);

                    //Dados da volta
                    dados.RotaTrajetoIdVolta = Convert.ToInt32(dataReader["ROTATRAJETOIDVOLTA"]);
                    dados.PrestadorIdVolta = dataReader["PRESTADORIDVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["PRESTADORIDVOLTA"]);
                    dados.CondutorIdVolta = dataReader["CONDUTORIDVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["CONDUTORIDVOLTA"]);
                    dados.VeiculoIdVolta = dataReader["VEICULOIDVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["VEICULOIDVOLTA"]);
                    dados.TipoContratacaoIdVolta = Convert.ToInt32(dataReader["TIPOCONTRATACAOIDVOLTA"]);
                    dados.TipoContratacaoDescricaoVolta = Convert.ToString(dataReader["TIPOCONTRATACAODESCRICAVOLTA"]);
                    dados.ValorRotaVolta = Convert.ToDecimal(dataReader["VALORROTAVOLTA"]);
                    dados.QuantidadeKmVolta = Convert.ToDecimal(dataReader["QUANTIDADEKMVOLTA"]);
                    dados.TempoVolta = dataReader["TEMPOVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["TEMPOVOLTA"]);
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
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                contexto.Dispose();
            }
        }

        public RN.DTOs.DadosRota ObtemDadosRotaPagamentoPor(int pagamentoId)
        {
            RN.DTOs.DadosRota dados = new DadosRota();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader dataReader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PR.ROTAID, 
                                       UE.ID_REGIONAL,
                                       RE.REGIONAL,
                                       UE.MUNICIPIO,
                                       M.NOME AS MUNICIPIODESCRICAO, 
                                       p.CENSO, 
                                       RM.REGIAOFINANCEIRAID, 
                                       RF.DESCRICAO AS REGIAOFINANCEIRADESCRICAO, 
                                       UE.CGC, 
                                       R.TURNO, 
                                       R.TIPOCALCULOPAGAMENTOID, 
									   CP.DESCRICAO as TIPOCALCULOPAGAMENTO, 
                                       R.ATIVO, 
                                       R.APROVADO,
                                       R.ORDEM,
                                       R.USUARIOID, 
                                       R.DATACADASTRO, 
                                       R.DATAALTERACAO,
                                       IDA.ROTATRAJETOID AS ROTATRAJETOIDIDA,
									   IDA.PRESTADORID AS PRESTADORIDIDA, 
                                       IDA.CONDUTORID AS CONDUTORIDIDA, 
                                       IDA.VEICULOID AS VEICULOIDIDA, 
                                       IDA.TIPOCONTRATACAOID AS TIPOCONTRATACAOIDIDA, 
									   CIDA.DESCRICAO AS TIPOCONTRATACAODESCRICAIDA,
                                       PR.VALORROTAIDA, 
									   PR.QUANTIDADEKMIDA,
									   PR.QUANTIDADEALUNOIDA,
                                       IDA.TEMPO as TEMPOIDA,
									   VOL.ROTATRAJETOID AS ROTATRAJETOIDVOLTA,
									   VOL.PRESTADORID AS PRESTADORIDVOLTA, 
                                       VOL.CONDUTORID AS CONDUTORIDVOLTA, 
                                       VOL.VEICULOID AS VEICULOIDVOLTA, 
                                       VOL.TIPOCONTRATACAOID AS TIPOCONTRATACAOIDVOLTA, 
									   CVOL.DESCRICAO AS  TIPOCONTRATACAODESCRICAVOLTA,
                                       PR.VALORROTAVOLTA, 
									   PR.QUANTIDADEKMVOLTA,
									   PR.QUANTIDADEALUNOVOLTA,
                                       VOL.TEMPO AS TEMPOVOLTA
                                FROM   [TRANSPORTE].PAGAMENTO P (NOLOCK)
                                       INNER JOIN  [TRANSPORTE].PAGAMENTOROTA PR (NOLOCK)
                                               ON P.PAGAMENTOID = PR.PAGAMENTOID
									   INNER JOIN [TRANSPORTE].[ROTA] R (NOLOCK)
                                               ON PR.ROTAID = R.ROTAID
									   INNER JOIN Transporte.TIPOCALCULOPAGAMENTO CP (NOLOCK) 
                                               ON CP.TIPOCALCULOPAGAMENTOID = r.TIPOCALCULOPAGAMENTOID
                                       INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) 
                                               ON P.CENSO = UE.UNIDADE_ENS 
                                       INNER JOIN TCE_REGIONAL RE (NOLOCK) 
                                               ON UE.ID_REGIONAL = RE.ID_REGIONAL 
                                       INNER JOIN HADES.DBO.TCE_MUNICIPIO M (NOLOCK) 
                                               ON UE.MUNICIPIO = M.ID_MUNICIPIO 
                                       INNER JOIN GESTAOREDE.REGIAOFINANCEIRAMUNICIPIO RM (NOLOCK) 
                                               ON M.ID_MUNICIPIO = RM.MUNICIPIOID 
                                       INNER JOIN GESTAOREDE.REGIAOFINANCEIRA RF (NOLOCK) 
                                               ON RM.REGIAOFINANCEIRAID = RF.REGIAOFINANCEIRAID 
									   INNER JOIN [Transporte].[ROTATRAJETO] IDA (NOLOCK) 
									           ON PR.ROTAID = IDA.ROTAID AND IDA.IDA = 1
									   INNER JOIN [Transporte].[ROTATRAJETO] VOL (NOLOCK) 
									           ON PR.ROTAID = VOL.ROTAID AND VOL.IDA = 0
									   INNER JOIN [Transporte].TIPOCONTRATACAO CIDA (NOLOCK) 
									           ON CIDA.TIPOCONTRATACAOID = IDA.TIPOCONTRATACAOID
									   INNER JOIN [Transporte].TIPOCONTRATACAO CVOL (NOLOCK) 
									           ON CVOL.TIPOCONTRATACAOID = VOL.TIPOCONTRATACAOID
                                WHERE  P.PAGAMENTOID = @PAGAMENTOID ";

                contextQuery.Parameters.Add("@PAGAMENTOID", SqlDbType.Int, pagamentoId);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    dados.RotaId = Convert.ToInt32(dataReader["ROTAID"]);
                    dados.Situacao = Convert.ToBoolean(dataReader["APROVADO"]) ? "Aprovado" : "Aguardando aprovação";
                    dados.RegionalDescricao = Convert.ToString(dataReader["REGIONAL"]);
                    dados.Regional = Convert.ToInt32(dataReader["ID_REGIONAL"]);
                    dados.Municipio = Convert.ToString(dataReader["MUNICIPIO"]);
                    dados.MunicipioDescricao = Convert.ToString(dataReader["MUNICIPIODESCRICAO"]);
                    dados.Censo = Convert.ToString(dataReader["CENSO"]);
                    dados.RegiaoFinanceiraId = Convert.ToInt32(dataReader["REGIAOFINANCEIRAID"]);
                    dados.RegiaoFinanceiraDescricao = Convert.ToString(dataReader["REGIAOFINANCEIRADESCRICAO"]);
                    dados.Cnpj = Convert.ToString(dataReader["CGC"]);
                    dados.Turno = Convert.ToString(dataReader["TURNO"]);
                    dados.Ordem = Convert.ToInt32(dataReader["ORDEM"]);
                    dados.Codigo = string.Format("{0}-{1}", dados.Censo, Convert.ToString(dados.Ordem));
                    dados.TipoCalculoPagamentoId = Convert.ToInt32(dataReader["TIPOCALCULOPAGAMENTOID"]);
                    dados.TipoCalculoPagamento = Convert.ToString(dataReader["TIPOCALCULOPAGAMENTO"]);
                    dados.Ativo = Convert.ToBoolean(dataReader["ATIVO"]);
                    dados.UsuarioResponsavel = Convert.ToString(dataReader["USUARIOID"]);

                    //Dados da Ida
                    dados.RotaTrajetoIdIda = Convert.ToInt32(dataReader["ROTATRAJETOIDIDA"]);
                    dados.PrestadorIdIda = dataReader["PRESTADORIDIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["PRESTADORIDIDA"]);
                    dados.CondutorIdIda = dataReader["CONDUTORIDIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["CONDUTORIDIDA"]);
                    dados.VeiculoIdIda = dataReader["VEICULOIDIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["VEICULOIDIDA"]);
                    dados.TipoContratacaoIdIda = Convert.ToInt32(dataReader["TIPOCONTRATACAOIDIDA"]);
                    dados.TipoContratacaoDescricaoIda = Convert.ToString(dataReader["TIPOCONTRATACAODESCRICAIDA"]);
                    dados.ValorRotaIda = Convert.ToDecimal(dataReader["VALORROTAIDA"]);
                    dados.QuantidadeKmIda = Convert.ToDecimal(dataReader["QUANTIDADEKMIDA"]);
                    dados.QuantidadeAlunosIda = Convert.ToInt32(dataReader["QUANTIDADEALUNOIDA"]);
                    dados.TempoIda = dataReader["TEMPOIDA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["TEMPOIDA"]);

                    //Dados da volta
                    dados.RotaTrajetoIdVolta = Convert.ToInt32(dataReader["ROTATRAJETOIDVOLTA"]);
                    dados.PrestadorIdVolta = dataReader["PRESTADORIDVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["PRESTADORIDVOLTA"]);
                    dados.CondutorIdVolta = dataReader["CONDUTORIDVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["CONDUTORIDVOLTA"]);
                    dados.VeiculoIdVolta = dataReader["VEICULOIDVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["VEICULOIDVOLTA"]);
                    dados.TipoContratacaoIdVolta = Convert.ToInt32(dataReader["TIPOCONTRATACAOIDVOLTA"]);
                    dados.TipoContratacaoDescricaoVolta = Convert.ToString(dataReader["TIPOCONTRATACAODESCRICAVOLTA"]);
                    dados.ValorRotaVolta = Convert.ToDecimal(dataReader["VALORROTAVOLTA"]);
                    dados.QuantidadeKmVolta = Convert.ToDecimal(dataReader["QUANTIDADEKMVOLTA"]);
                    dados.QuantidadeAlunosVolta = Convert.ToInt32(dataReader["QUANTIDADEALUNOVOLTA"]);
                    dados.TempoVolta = dataReader["TEMPOVOLTA"] == DBNull.Value ? 0 : Convert.ToInt32(dataReader["TEMPOVOLTA"]);
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
                if (dataReader != null)
                {
                    dataReader.Close();
                }
                contexto.Dispose();
            }
        }

        public DataTable ListaAtivaPor(string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT R.ROTAID, 
									   R.CENSO, 
									   R.ORDEM,
									   R.CENSO + '-' + CONVERT(VARCHAR(3), R.ORDEM) AS CODIGO,
                                       T.DESCRICAO AS TURNO, 
                                       CP.DESCRICAO AS TIPOCALCULOPAGAMENTO, 
                                       R.ATIVO, 
                                       CASE
											WHEN R.APROVADO = 1 THEN 'APROVADO'
											ELSE 'AGUARDANDO APROVAÇÃO'
									   END SITUACAO,
									   R.DATALIMITEEDICAO,
									   R.DATALIMITEEDICAOALUNO
                                FROM   [TRANSPORTE].[ROTA] R (NOLOCK)									   
										INNER JOIN TRANSPORTE.TIPOCALCULOPAGAMENTO CP (NOLOCK)
											   ON CP.TIPOCALCULOPAGAMENTOID = R.TIPOCALCULOPAGAMENTOID
										INNER JOIN LY_TURNO T (NOLOCK)
											   ON R.TURNO = T.TURNO
                                WHERE R.ATIVO = 1
									  AND R.CENSO = @CENSO
								ORDER BY ORDEM DESC ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public ValidacaoDados ValidaInsercao(DadosRotaCadastro dadosRota)
        {
            List<string> mensagens = new List<string>();
            decimal numeroDecimalLat;
            decimal numeroDecimalLong;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosRota == null)
            {
                return validacaoDados;
            }

            //Dados Rota
            if (dadosRota.TipoCalculoPagamentoId <= 0)
            {
                mensagens.Add("Campo TIPO DE CALCULO DE PAGAMENTO é obrigatório.");
            }

            if (dadosRota.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ESCOLA é obrigatório.");
            }

            if (dadosRota.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (dadosRota.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            //Dados Trajeto Ida
            if (dadosRota.TipoContratacaoIdIda <= 0)
            {
                mensagens.Add("Campo TIPO DE CONTRATACAO do trajeto de ida é obrigatório.");
            }

            if (dadosRota.ValorRotaIda <= 0)
            {
                mensagens.Add("Campo VALOR do trajeto de ida é obrigatório.");
            }

            if (dadosRota.QuantidadeKmIda <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE KM do trajeto de ida é obrigatório.");
            }

            //Dados primeiro ponto Ida
            if (dadosRota.PrimeiroCepIda.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP do primeiro ponto de ida é obrigatório.");
            }
            else
            {
                dadosRota.PrimeiroCepIda = Utils.RetirarMascara(dadosRota.PrimeiroCepIda);

                if (!Validacao.ValidarCEP(dadosRota.PrimeiroCepIda))
                {
                    mensagens.Add("CEP do primeiro ponto de ida inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (dadosRota.PrimeiroLogradouroIda.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO do primeiro ponto de ida é obrigatório.");
            }
            else
            {
                if (dadosRota.PrimeiroLogradouroIda.Length > 50)
                {
                    mensagens.Add("Campo LOGRADOURO do primeiro ponto de ida deve conter no máximo 50 caracteres.");
                }
            }

            if (dadosRota.PrimeiroNumeroIda.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO do primeiro ponto de ida é obrigatório.");
            }

            if (dadosRota.PrimeiroBairroIda.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO do primeiro ponto de ida é obrigatório.");
            }
            else
            {
                if (!Validacao.Bairro(dadosRota.PrimeiroBairroIda))
                {
                    mensagens.Add("Campo BAIRRO do primeiro ponto de ida é inválido!");
                }
            }

            if (dadosRota.PrimeiroMunicipioIda.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO do primeiro ponto de ida é obrigatório.");
            }

            if (!dadosRota.PrimeiroLatitudeIda.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(dadosRota.PrimeiroLatitudeIda.Replace(".", ","), out numeroDecimalLat))
                {
                    mensagens.Add("LATITUDE do primeiro ponto de ida inválida.");
                }
                else
                {
                    var lat = Math.Abs(numeroDecimalLat);
                    if (lat < 20 || lat > 24)
                    {
                        mensagens.Add("Latitude do primeiro ponto de ida fora do limite permitido. Favor verificar.");
                    }
                }
            }

            if (!dadosRota.PrimeiroLongitudeIda.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(dadosRota.PrimeiroLongitudeIda.Replace(".", ","), out numeroDecimalLong))
                {
                    mensagens.Add("LONGITUDE do primeiro ponto de ida inválida.");
                }
                else
                {
                    var longi = Math.Abs(numeroDecimalLong);
                    if (longi < 40 || longi > 45)
                    {
                        mensagens.Add("Longitude do primeiro ponto de ida fora do limite permitido. Favor verificar.");
                    }
                }
            }

            //Dados Trajeto Volta
            if (dadosRota.TipoContratacaoIdVolta <= 0)
            {
                mensagens.Add("Campo TIPO DE CONTRATACAO do trajeto de volta é obrigatório.");
            }

            if (dadosRota.ValorRotaVolta <= 0)
            {
                mensagens.Add("Campo VALOR do trajeto de volta é obrigatório.");
            }

            if (dadosRota.QuantidadeKmVolta <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE KM do trajeto de volta é obrigatório.");
            }

            //Dados primeiro ponto Volta
            if (dadosRota.PrimeiroCepVolta.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP do primeiro ponto de Volta é obrigatório.");
            }
            else
            {
                dadosRota.PrimeiroCepVolta = Utils.RetirarMascara(dadosRota.PrimeiroCepVolta);

                if (!Validacao.ValidarCEP(dadosRota.PrimeiroCepVolta))
                {
                    mensagens.Add("CEP do primeiro ponto de Volta inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (dadosRota.PrimeiroLogradouroVolta.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO do primeiro ponto de Volta é obrigatório.");
            }
            else
            {
                if (dadosRota.PrimeiroLogradouroVolta.Length > 50)
                {
                    mensagens.Add("Campo LOGRADOURO do primeiro ponto de Volta deve conter no máximo 50 caracteres.");
                }
            }

            if (dadosRota.PrimeiroNumeroVolta.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO do primeiro ponto de Volta é obrigatório.");
            }

            if (dadosRota.PrimeiroBairroVolta.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO do primeiro ponto de Volta é obrigatório.");
            }
            else
            {
                if (!Validacao.Bairro(dadosRota.PrimeiroBairroVolta))
                {
                    mensagens.Add("Campo BAIRRO do primeiro ponto de Volta é inválido!");
                }
            }

            if (dadosRota.PrimeiroMunicipioVolta.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO do primeiro ponto de Volta é obrigatório.");
            }

            if (!dadosRota.PrimeiroLatitudeVolta.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(dadosRota.PrimeiroLatitudeVolta.Replace(".", ","), out numeroDecimalLat))
                {
                    mensagens.Add("LATITUDE do primeiro ponto de Volta inválida.");
                }
                else
                {
                    var lat = Math.Abs(numeroDecimalLat);
                    if (lat < 20 || lat > 24)
                    {
                        mensagens.Add("Latitude do primeiro ponto de Volta fora do limite permitido. Favor verificar.");
                    }
                }
            }

            if (!dadosRota.PrimeiroLongitudeVolta.IsNullOrEmptyOrWhiteSpace())
            {
                if (!decimal.TryParse(dadosRota.PrimeiroLongitudeVolta.Replace(".", ","), out numeroDecimalLong))
                {
                    mensagens.Add("LONGITUDE do primeiro ponto de Volta inválVolta.");
                }
                else
                {
                    var longi = Math.Abs(numeroDecimalLong);
                    if (longi < 40 || longi > 45)
                    {
                        mensagens.Add("Longitude do primeiro ponto de Volta fora do limite permitido. Favor verificar.");
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

        private bool PossuiOutroCadastroPor(DataContext ctx, string censo, int prestadorId, int condutorId, int veiculoId, string turno, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   Transporte.ROTA r (NOLOCK) 
											   INNER JOIN TRANSPORTE.ROTATRAJETO RT (NOLOCK) 
													ON R.ROTAID = RT.ROTAID
                                        WHERE  CENSO = @CENSO 
                                               AND PRESTADORID = @PRESTADORID 
                                               AND CONDUTORID = @CONDUTORID 
                                               AND VEICULOID = @VEICULOID 
                                               AND TURNO = @TURNO
                                               AND R.ROTAID <> @ROTAID
											   AND R.ATIVO = 1
											   AND RT.ATIVO = 1 ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PRESTADORID", SqlDbType.Int, prestadorId);
            contextQuery.Parameters.Add("@CONDUTORID", SqlDbType.Int, condutorId);
            contextQuery.Parameters.Add("@VEICULOID", SqlDbType.Int, veiculoId);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PodeEditarPor(DataContext ctx, int rotaId)
        {
            //Pode editar enquanto não houver aprovação ou até a data limite
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   Transporte.ROTA r (NOLOCK) 
											   INNER JOIN TRANSPORTE.ROTATRAJETO RT (NOLOCK) 
													ON R.ROTAID = RT.ROTAID
                                        WHERE   R.ROTAID = @ROTAID
												AND (R.APROVADO IS NULL 
														OR (R.DATALIMITEEDICAO IS NULL OR DATALIMITEEDICAO >= @DATA)) ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, DateTime.Now.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                pode = true;
            }

            return pode;
        }

        private bool EhAtivaPor(DataContext ctx, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   Transporte.ROTA (NOLOCK)
                                        WHERE   ROTAID = @ROTAID
												AND ATIVO = 1 ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                pode = true;
            }

            return pode;
        }

        public bool EhAtivaPor(int rotaId)
        {
            bool ativo = false;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                ativo = this.EhAtivaPor(ctx, rotaId);
                return ativo;
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

        public bool EhAprovadaPor(DataContext ctx, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   Transporte.ROTA (NOLOCK)
                                        WHERE   ROTAID = @ROTAID
												AND APROVADO = 1 ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                pode = true;
            }

            return pode;
        }

        public bool PodeEditarAlunoPor(DataContext ctx, int rotaId)
        {
            //Pode editar enquanto não houver aprovação ou até a data limite
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   Transporte.ROTA r (NOLOCK) 
											   INNER JOIN TRANSPORTE.ROTATRAJETO RT (NOLOCK) 
													ON R.ROTAID = RT.ROTAID
                                        WHERE   R.ROTAID = @ROTAID
												AND (R.APROVADO IS NULL 
														OR (R.DATALIMITEEDICAOALUNO IS NULL OR DATALIMITEEDICAOALUNO >= @DATA)) ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, DateTime.Now.Date);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                pode = true;
            }

            return pode;
        }

        public void Insere(DadosRotaCadastro dadosRota)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Entidades.Rota rota = new Techne.Lyceum.RN.Transporte.Entidades.Rota();
            Entidades.RotaTrajeto rotaTrajetoIda = new Techne.Lyceum.RN.Transporte.Entidades.RotaTrajeto();
            Entidades.RotaTrajeto rotaTrajetoVolta = new Techne.Lyceum.RN.Transporte.Entidades.RotaTrajeto();
            Entidades.PontoEmbarque pontoEmbarqueIda = new Techne.Lyceum.RN.Transporte.Entidades.PontoEmbarque();
            Entidades.PontoEmbarque pontoEmbarqueVolta = new Techne.Lyceum.RN.Transporte.Entidades.PontoEmbarque();
            PontoEmbarque rnPontoEmbarque = new PontoEmbarque();
            RotaTrajeto rnRotaTrajeto = new RotaTrajeto();

            try
            {
                //Monta Rota
                rota.TipoCalculoPagamentoId = dadosRota.TipoCalculoPagamentoId;
                rota.Censo = dadosRota.Censo;
                rota.Turno = dadosRota.Turno;
                rota.Aprovado = false;
                rota.Ativo = dadosRota.Ativo;
                rota.UsuarioId = dadosRota.UsuarioResponsavel;

                //Buscar maior ordem + 1 para o censo
                rota.Ordem = this.ObtemProximaOrdemPor(contexto, rota.Censo);

                //Insere Rota
                this.Insere(contexto, rota);

                //Atualiza id gerado e codigo
                dadosRota.RotaId = rota.RotaId;
                dadosRota.Codigo = string.Format("{0}-{1}", rota.Censo, Convert.ToString(rota.Ordem));

                //Monta Trajeto Ida
                rotaTrajetoIda.Ida = true;
                rotaTrajetoIda.RotaId = dadosRota.RotaId;
                rotaTrajetoIda.TipoContratacaoId = dadosRota.TipoContratacaoIdIda;
                rotaTrajetoIda.ValorRota = dadosRota.ValorRotaIda;
                rotaTrajetoIda.QuantidadeKm = dadosRota.QuantidadeKmIda;
                rotaTrajetoIda.Tempo = dadosRota.TempoIda;
                rotaTrajetoIda.Ativo = dadosRota.Ativo;
                rotaTrajetoIda.UsuarioId = dadosRota.UsuarioResponsavel;
                rotaTrajetoIda.PrestadorId = null; //Inicia vazio
                rotaTrajetoIda.CondutorId = null;
                rotaTrajetoIda.VeiculoId = null;

                //Insere Trajeto Ida
                rnRotaTrajeto.Insere(contexto, rotaTrajetoIda);

                //Atualiza id gerado
                dadosRota.RotaTrajetoIdIda = rotaTrajetoIda.RotaTrajetoId;

                //Monta Primeiro Ponto Embarque Ida
                pontoEmbarqueIda.RotaTrajetoId = dadosRota.RotaTrajetoIdIda;
                pontoEmbarqueIda.Primeiro = true;
                pontoEmbarqueIda.Cep = dadosRota.PrimeiroCepIda;
                pontoEmbarqueIda.Logradouro = dadosRota.PrimeiroLogradouroIda;
                pontoEmbarqueIda.Numero = dadosRota.PrimeiroNumeroIda;
                pontoEmbarqueIda.Bairro = dadosRota.PrimeiroBairroIda;
                pontoEmbarqueIda.Municipio = dadosRota.PrimeiroMunicipioIda;
                pontoEmbarqueIda.Latitude = dadosRota.PrimeiroLatitudeIda;
                pontoEmbarqueIda.Longitude = dadosRota.PrimeiroLongitudeIda;
                pontoEmbarqueIda.UsuarioId = dadosRota.UsuarioResponsavel;

                //Insere Primeiro ponto de embarque Ida
                rnPontoEmbarque.Insere(contexto, pontoEmbarqueIda);

                //Monta Trajeto Volta
                rotaTrajetoVolta.Ida = false;
                rotaTrajetoVolta.RotaId = dadosRota.RotaId;
                rotaTrajetoVolta.TipoContratacaoId = dadosRota.TipoContratacaoIdVolta;
                rotaTrajetoVolta.ValorRota = dadosRota.ValorRotaVolta;
                rotaTrajetoVolta.QuantidadeKm = dadosRota.QuantidadeKmVolta;
                rotaTrajetoVolta.Tempo = dadosRota.TempoVolta;
                rotaTrajetoVolta.Ativo = dadosRota.Ativo;
                rotaTrajetoVolta.UsuarioId = dadosRota.UsuarioResponsavel;
                rotaTrajetoVolta.PrestadorId = null; //Inicia vazio
                rotaTrajetoVolta.CondutorId = null;
                rotaTrajetoVolta.VeiculoId = null;

                //Insere Trajeto Volta
                rnRotaTrajeto.Insere(contexto, rotaTrajetoVolta);

                //Atualiza id gerado
                dadosRota.RotaTrajetoIdVolta = rotaTrajetoVolta.RotaTrajetoId;

                //Monta Primeiro Ponto Embarque Volta
                pontoEmbarqueVolta.RotaTrajetoId = dadosRota.RotaTrajetoIdVolta;
                pontoEmbarqueVolta.Primeiro = true;
                pontoEmbarqueVolta.Cep = dadosRota.PrimeiroCepVolta;
                pontoEmbarqueVolta.Logradouro = dadosRota.PrimeiroLogradouroVolta;
                pontoEmbarqueVolta.Numero = dadosRota.PrimeiroNumeroVolta;
                pontoEmbarqueVolta.Bairro = dadosRota.PrimeiroBairroVolta;
                pontoEmbarqueVolta.Municipio = dadosRota.PrimeiroMunicipioVolta;
                pontoEmbarqueVolta.Latitude = dadosRota.PrimeiroLatitudeVolta;
                pontoEmbarqueVolta.Longitude = dadosRota.PrimeiroLongitudeVolta;
                pontoEmbarqueVolta.UsuarioId = dadosRota.UsuarioResponsavel;

                //Insere Primeiro ponto de embarque Volta
                rnPontoEmbarque.Insere(contexto, pontoEmbarqueVolta);
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

        private int ObtemProximaOrdemPor(DataContext contexto, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int ordem = 0;

            try
            {
                contextQuery.Command = @" SELECT MAX(ORDEM) AS ORDEM
                                        FROM    Transporte.ROTA (NOLOCK)
                                        WHERE   CENSO = @CENSO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ordem = reader["ORDEM"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ORDEM"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return ordem + 1;
        }

        public string ObtemCodigoPor(int rotaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT CENSO + '-' + CONVERT(VARCHAR(3), ORDEM) AS CODIGO 
                                            FROM Transporte.ROTA (NOLOCK)
                                            WHERE ROTAID = @ROTAID ";

                contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
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

        public string ObtemMunicipioPor(DataContext contexto, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT UE.MUNICIPIO
                                            FROM Transporte.ROTA r (NOLOCK)
												INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
														ON R.CENSO = UE.UNIDADE_ENS
                                            WHERE ROTAID = @ROTAID ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        private void Insere(DataContext contexto, Entidades.Rota rota)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Transporte.ROTA
                                            (TIPOCALCULOPAGAMENTOID, 
                                             CENSO, 
                                             TURNO, 
                                             ORDEM, 
                                             APROVADO,
                                             ATIVO, 
                                             USUARIOID, 
                                             DATACADASTRO, 
                                             DATAALTERACAO) 
                                VALUES      (@TIPOCALCULOPAGAMENTOID, 
                                             @CENSO, 
                                             @TURNO, 
                                             @ORDEM,
                                             @APROVADO,
                                             @ATIVO, 
                                             @USUARIOID, 
                                             @DATACADASTRO, 
                                             @DATAALTERACAO) 

                                SELECT IDENT_CURRENT('Transporte.ROTA') ";

            contextQuery.Parameters.Add("@TIPOCALCULOPAGAMENTOID", SqlDbType.Int, rota.TipoCalculoPagamentoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, rota.Censo);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, rota.Turno);
            contextQuery.Parameters.Add("@ORDEM", SqlDbType.Int, rota.Ordem);
            contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, rota.Aprovado);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, rota.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, rota.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            rota.RotaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        public ValidacaoDados ValidaAtualizacao(DadosRota dadosRota)
        {
            List<string> mensagens = new List<string>();
            int quantidadeAlunoIda;
            int quantidadeAlunoVolta;
            int quantidadeAssentosIda;
            int quantidadeAssentosIVolta;
            DataContext contexto = null;
            Prestador rnPrestador = new Prestador();
            PrestadorBloqueio rnPrestadorBloqueio = new PrestadorBloqueio();
            PrestadorVigencia rnPrestadorVigencia = new PrestadorVigencia();
            Condutor rnCondutor = new Condutor();
            CondutorBloqueio rnCondutorBloqueio = new CondutorBloqueio();
            Veiculo rnVeiculo = new Veiculo();
            VeiculoBloqueio rnVeiculoBloqueio = new VeiculoBloqueio();
            RotaAluno rnRotaAluno = new RotaAluno();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosRota == null)
            {
                return validacaoDados;
            }

            //Dados Rota
            if (dadosRota.RotaId <= 0)
            {
                mensagens.Add("Campo CODIGO da ROTA é obrigatório.");
            }

            if (dadosRota.TipoCalculoPagamentoId <= 0)
            {
                mensagens.Add("Campo TIPO DE CALCULO DE PAGAMENTO é obrigatório.");
            }

            if (dadosRota.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ESCOLA é obrigatório.");
            }

            if (dadosRota.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (dadosRota.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            //Dados Trajeto Ida
            if (dadosRota.RotaTrajetoIdIda <= 0)
            {
                mensagens.Add("Campo CODIGO DO TRAJETO DE IDA é obrigatório.");
            }

            if (dadosRota.TipoContratacaoIdIda <= 0)
            {
                mensagens.Add("Campo TIPO DE CONTRATACAO do trajeto de ida é obrigatório.");
            }

            if (dadosRota.ValorRotaIda <= 0)
            {
                mensagens.Add("Campo VALOR do trajeto de ida é obrigatório.");
            }

            if (dadosRota.QuantidadeKmIda <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE KM do trajeto de ida é obrigatório.");
            }

            //Dados Trajeto Volta
            if (dadosRota.RotaTrajetoIdVolta <= 0)
            {
                mensagens.Add("Campo CODIGO DO TRAJETO DE VOLTA é obrigatório.");
            }

            if (dadosRota.TipoContratacaoIdVolta <= 0)
            {
                mensagens.Add("Campo TIPO DE CONTRATACAO do trajeto de volta é obrigatório.");
            }

            if (dadosRota.ValorRotaVolta <= 0)
            {
                mensagens.Add("Campo VALOR do trajeto de volta é obrigatório.");
            }

            if (dadosRota.QuantidadeKmVolta <= 0)
            {
                mensagens.Add("Campo QUANTIDADE DE KM do trajeto de volta é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se a rota pode ser alterada
                    if (!this.PodeEditarPor(contexto, dadosRota.RotaId))
                    {
                        mensagens.Add("A rota não pode ser alterada, pois o periodo de carência para edição terminou.");
                    }
                    else
                    {
                        //Verifca se existe a associação entre prestador / condutor / veiculo
                        if (!rnVeiculo.PossuiPrestadorCondutorVeiculoPor(contexto, dadosRota.PrestadorIdIda, dadosRota.CondutorIdIda, dadosRota.VeiculoIdIda, true))
                        {
                            mensagens.Add("O PRESTADOR / CONDUTOR / VEICULO de ida não possui associação ativa.");
                        }
                        if (!rnVeiculo.PossuiPrestadorCondutorVeiculoPor(contexto, dadosRota.PrestadorIdVolta, dadosRota.CondutorIdVolta, dadosRota.VeiculoIdVolta, true))
                        {
                            mensagens.Add("O PRESTADOR / CONDUTOR / VEICULO de volta não possui associação ativa.");
                        }


                        //Verifica se já existe a msm rota para o censo / prestador / condutor / veiculo / turno de ida
                        if (this.PossuiOutroCadastroPor(contexto, dadosRota.Censo, dadosRota.PrestadorIdIda, dadosRota.CondutorIdIda, dadosRota.VeiculoIdIda, dadosRota.Turno, dadosRota.RotaId))
                        {
                            mensagens.Add("Já existe uma rota cadastrada para o CENSO / PRESTADOR / CONDUTOR / VEICULO / TURNO de ida.");
                        }

                        //Verifica se já existe a msm rota para o censo / prestador / condutor / veiculo / turno de Volta
                        if (this.PossuiOutroCadastroPor(contexto, dadosRota.Censo, dadosRota.PrestadorIdVolta, dadosRota.CondutorIdVolta, dadosRota.VeiculoIdVolta, dadosRota.Turno, dadosRota.RotaId))
                        {
                            mensagens.Add("Já existe uma rota cadastrada para o CENSO / PRESTADOR / CONDUTOR / VEICULO / TURNO de volta.");
                        }

                        //Ao ser bloqueado ou desabilitado o prestador não poderá ser associado a uma rota
                        if (!rnPrestador.EhAtivoPor(contexto, dadosRota.PrestadorIdIda))
                        {
                            mensagens.Add("Este PRESTADOR de ida não pode ser associdado a rota pois não está ativo.");
                        }
                        else if (rnPrestadorBloqueio.PossuiBloqueioAbertoPor(contexto, dadosRota.PrestadorIdIda, DateTime.Now))
                        {
                            mensagens.Add("Este PRESTADOR de ida se encontra bloqueado.");
                        }
                        if (!rnPrestador.EhAtivoPor(contexto, dadosRota.PrestadorIdVolta))
                        {
                            mensagens.Add("Este PRESTADOR de volta não pode ser associdado a rota pois não está ativo.");
                        }
                        else if (rnPrestadorBloqueio.PossuiBloqueioAbertoPor(contexto, dadosRota.PrestadorIdVolta, DateTime.Now))
                        {
                            mensagens.Add("Este PRESTADOR de volta se encontra bloqueado.");
                        }

                        //Ao ser bloqueado ou desabilitado o condutor não poderá ser associado a uma rota                    
                        if (!rnCondutor.EhAtivoPor(contexto, dadosRota.CondutorIdIda))
                        {
                            mensagens.Add("Este CONDUTOR de ida não pode ser associdado a rota pois não está ativo.");
                        }
                        else if (rnCondutorBloqueio.PossuiBloqueioAbertoPor(contexto, dadosRota.CondutorIdIda, DateTime.Now))
                        {
                            mensagens.Add("Este CONDUTOR de ida se encontra bloqueado.");
                        }
                        if (!rnCondutor.EhAtivoPor(contexto, dadosRota.CondutorIdIda))
                        {
                            mensagens.Add("Este CONDUTOR de volta não pode ser associdado a rota pois não está ativo.");
                        }
                        else if (rnCondutorBloqueio.PossuiBloqueioAbertoPor(contexto, dadosRota.CondutorIdIda, DateTime.Now))
                        {
                            mensagens.Add("Este CONDUTOR de volta se encontra bloqueado.");
                        }

                        //Ao ser bloqueado ou desabilitado o veículo não poderá ser associado a uma rota
                        if (!rnVeiculo.EhAtivoPor(contexto, dadosRota.VeiculoIdIda))
                        {
                            mensagens.Add("Este VEÍCULO de ida não pode ser associdado a rota pois não está ativo.");
                        }
                        else if (rnVeiculoBloqueio.PossuiBloqueioAbertoPor(contexto, dadosRota.VeiculoIdIda, DateTime.Now))
                        {
                            mensagens.Add("Este VEÍCULO de ida se encontra bloqueado.");
                        }
                        if (!rnVeiculo.EhAtivoPor(contexto, dadosRota.VeiculoIdVolta))
                        {
                            mensagens.Add("Este VEÍCULO de volta não pode ser associdado a rota pois não está ativo.");
                        }
                        else if (rnVeiculoBloqueio.PossuiBloqueioAbertoPor(contexto, dadosRota.VeiculoIdVolta, DateTime.Now))
                        {
                            mensagens.Add("Este VEÍCULO de volta se encontra bloqueado.");
                        }

                        //Busca Alunos trajeto Ida
                        quantidadeAlunoIda = rnRotaAluno.ObtemAlunosAtivosPor(contexto, dadosRota.RotaTrajetoIdIda, DateTime.Now.Date);
                        //Para adicionar veiculo, condutor e prestado é necessaio ter aluno
                        if (quantidadeAlunoIda <= 0)
                        {
                            mensagens.Add("Para atualizar o trajeto de Ida necessario que existam alunos associados.");
                        }
                        else
                        {
                            //Busca Quantidade Assentos Ida
                            quantidadeAssentosIda = rnVeiculo.ObtemQuantidadeAssentosPor(contexto, dadosRota.VeiculoIdIda);

                            //Veiculo tem q ter quantidade de acentos maior ou igual a quantidade de aluno + o motorista
                            if (quantidadeAssentosIda < quantidadeAlunoIda + 1)
                            {
                                mensagens.Add("Veiculo de Ida precisa ter a quantidade de assentos maior ou igual a quantidade de aluno.");
                            }
                        }

                        //Bsuca Aluno trajeto Volta
                        quantidadeAlunoVolta = rnRotaAluno.ObtemAlunosAtivosPor(contexto, dadosRota.RotaTrajetoIdVolta, DateTime.Now.Date);
                        //Para adicionar veiculo, condutor e prestado é necessaio ter aluno
                        if (quantidadeAlunoVolta <= 0)
                        {
                            mensagens.Add("Para atualizar o trajeto de Volta é necessario que existam alunos associados.");
                        }
                        else
                        {
                            //Busca Quantidade Assentos Volta
                            quantidadeAssentosIVolta = rnVeiculo.ObtemQuantidadeAssentosPor(contexto, dadosRota.VeiculoIdVolta);

                            //Veiculo tem q ter quantidade de acentos maior ou igual a quantidade de aluno + o motorista
                            if (quantidadeAssentosIVolta < quantidadeAlunoVolta + 1)
                            {
                                mensagens.Add("Veiculo de Volta precisa ter a quantidade de assentos maior ou igual a quantidade de aluno.");
                            }
                        }

                        //Verifica se prestador tem vigencia para a escola
                        if (!rnPrestadorVigencia.PossuiVigenciaAbertaPor(contexto, dadosRota.PrestadorIdIda, dadosRota.Censo, DateTime.Now.Date))
                        {
                            mensagens.Add("O PRESTADOR de ida não pode ser associado a rota pois não possui vigência atual para a escola.");
                        }
                        if (!rnPrestadorVigencia.PossuiVigenciaAbertaPor(contexto, dadosRota.PrestadorIdVolta, dadosRota.Censo, DateTime.Now.Date))
                        {
                            mensagens.Add("O PRESTADOR de volta não pode ser associado a rota pois não possui vigência atual para a escola.");
                        }

                        if (dadosRota.Turno != "I") //Condutores e veiculos de rotas Integral podem estar em outras outras
                        {
                            //Verificar se veiculo está no msm turno em outra rota ativa
                            if (this.PossuiVeiculoOutraRotaPor(contexto, dadosRota.VeiculoIdIda, dadosRota.Turno, dadosRota.RotaId))
                            {
                                mensagens.Add("O VEICULO de ida não pode ser associado a rota pois já possui associação em outra rota no mesmo turno.");
                            }
                            if (this.PossuiVeiculoOutraRotaPor(contexto, dadosRota.VeiculoIdVolta, dadosRota.Turno, dadosRota.RotaId))
                            {
                                mensagens.Add("O VEICULO de volta não pode ser associado a rota pois já possui associação em outra rota no mesmo turno.");
                            }

                            //Verificar se condutor está no msm turno em outra rota ativa
                            if (this.PossuiCondutorOutraRotaPor(contexto, dadosRota.CondutorIdIda, dadosRota.Turno, dadosRota.RotaId))
                            {
                                mensagens.Add("O CONDUTOR de ida não pode ser associado a rota pois já possui associação em outra rota no mesmo turno.");
                            }
                            if (this.PossuiCondutorOutraRotaPor(contexto, dadosRota.CondutorIdVolta, dadosRota.Turno, dadosRota.RotaId))
                            {
                                mensagens.Add("O CONDUTOR de volta não pode ser associado a rota pois já possui associação em outra rota no mesmo turno.");
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

        public void Atualiza(DadosRota dadosRota)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Entidades.Rota rota = new Techne.Lyceum.RN.Transporte.Entidades.Rota();
            Entidades.RotaTrajeto rotaTrajetoIda = new Techne.Lyceum.RN.Transporte.Entidades.RotaTrajeto();
            Entidades.RotaTrajeto rotaTrajetoVolta = new Techne.Lyceum.RN.Transporte.Entidades.RotaTrajeto();
            RotaTrajeto rnRotaTrajeto = new RotaTrajeto();

            try
            {
                //Monta Rota
                rota.TipoCalculoPagamentoId = dadosRota.TipoCalculoPagamentoId;
                rota.RotaId = dadosRota.RotaId;
                rota.Censo = dadosRota.Censo;
                rota.Turno = dadosRota.Turno;
                rota.Ordem = dadosRota.Ordem;
                rota.UsuarioId = dadosRota.UsuarioResponsavel;
                rota.Ativo = dadosRota.Ativo;

                //Atualiza Rota
                this.Atualiza(contexto, rota);

                //Monta Trajeto Ida
                rotaTrajetoIda.Ida = true;
                rotaTrajetoIda.RotaTrajetoId = dadosRota.RotaTrajetoIdIda;
                rotaTrajetoIda.RotaId = dadosRota.RotaId;
                rotaTrajetoIda.TipoContratacaoId = dadosRota.TipoContratacaoIdIda;
                rotaTrajetoIda.ValorRota = dadosRota.ValorRotaIda;
                rotaTrajetoIda.QuantidadeKm = dadosRota.QuantidadeKmIda;
                rotaTrajetoIda.Tempo = dadosRota.TempoIda;
                rotaTrajetoIda.UsuarioId = dadosRota.UsuarioResponsavel;
                rotaTrajetoIda.PrestadorId = dadosRota.PrestadorIdIda;
                rotaTrajetoIda.CondutorId = dadosRota.CondutorIdIda;
                rotaTrajetoIda.VeiculoId = dadosRota.VeiculoIdIda;
                rotaTrajetoIda.Ativo = dadosRota.Ativo;

                //Atualiza Trajeto Ida
                rnRotaTrajeto.Atualiza(contexto, rotaTrajetoIda);

                //Monta Trajeto Volta
                rotaTrajetoVolta.Ida = false;
                rotaTrajetoVolta.RotaTrajetoId = dadosRota.RotaTrajetoIdVolta;
                rotaTrajetoVolta.RotaId = dadosRota.RotaId;
                rotaTrajetoVolta.TipoContratacaoId = dadosRota.TipoContratacaoIdVolta;
                rotaTrajetoVolta.ValorRota = dadosRota.ValorRotaVolta;
                rotaTrajetoVolta.QuantidadeKm = dadosRota.QuantidadeKmVolta;
                rotaTrajetoVolta.Tempo = dadosRota.TempoVolta;
                rotaTrajetoVolta.UsuarioId = dadosRota.UsuarioResponsavel;
                rotaTrajetoVolta.PrestadorId = dadosRota.PrestadorIdVolta;
                rotaTrajetoVolta.CondutorId = dadosRota.CondutorIdVolta;
                rotaTrajetoVolta.VeiculoId = dadosRota.VeiculoIdVolta;
                rotaTrajetoVolta.Ativo = dadosRota.Ativo;

                //Atualiza Trajeto Volta
                rnRotaTrajeto.Atualiza(contexto, rotaTrajetoVolta);
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

        private void Atualiza(DataContext contexto, Entidades.Rota rota)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Transporte.ROTA
                                        SET    ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  ROTAID = @ROTAID ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rota.RotaId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, rota.Ativo);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, rota.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaDesativacao(int rotaId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Transporte.VeiculoBloqueio rnVeiculoBloqueio = new VeiculoBloqueio();
            RN.Transporte.PrestadorBloqueio rnPrestadorBloqueio = new PrestadorBloqueio();
            RN.Transporte.CondutorBloqueio rnCondutorBloqueio = new CondutorBloqueio();
            RN.Transporte.RotaAluno rnRotaAluno = new RotaAluno();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja está desativado
                    if (!this.EhAtivaPor(contexto, rotaId))
                    {
                        mensagens.Add("Está rota já está desativada.");
                    }

                    //Verifica se tem aluno
                    if (rnRotaAluno.PossuiAlunoAtivoPor(contexto, rotaId))
                    {
                        mensagens.Add("Registro não pode ser desabilitado, pois existem alunos com associação ativa nesta rota.");
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

        public void Desativa(int rotaId, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RotaTrajeto rnRotaTrajeto = new RotaTrajeto();

            try
            {
                //Desativa Trajeto Rota
                rnRotaTrajeto.Desativa(contexto, rotaId, usuarioId);

                //Desativa Rota
                this.Desativa(contexto, rotaId, usuarioId);
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

        private void Desativa(DataContext contexto, int rotaId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Transporte.ROTA
                                        SET    ATIVO = @ATIVO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  ROTAID = @ROTAID ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
            contextQuery.Parameters.Add("@ATIVO", SqlDbType.Bit, false);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int rotaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Transporte.PontoEmbarque rnPontoEmbarque = new PontoEmbarque();
            RN.Transporte.RotaAluno rnRotaAluno = new RotaAluno();
            RN.Transporte.PagamentoRota rnPagamentoRota = new PagamentoRota();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado
                    if (rnPontoEmbarque.PossuiRotaSecundariaPor(contexto, rotaId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois existem pontos de embarque associados a essa rota.");
                    }

                    if (rnRotaAluno.PossuiAlunoPor(contexto, rotaId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois existem alunos associados a essa rota.");
                    }

                    if (rnPagamentoRota.PossuiRotaPor(contexto, rotaId))
                    {
                        mensagens.Add("Registro não pode ser excluído, pois existem pagamentos associados a essa rota.");
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

        public void Remove(int rotaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RotaTrajeto rnRotaTrajeto = new RotaTrajeto();
            PontoEmbarque rnPontoEmbarque = new PontoEmbarque();
            try
            {
                //Deleta primeiro ponto da rota de ida e volta
                rnPontoEmbarque.RemovePrimeiro(contexto, rotaId);

                //Delete trajeto de ida e volta
                rnRotaTrajeto.Remove(contexto, rotaId);

                //Deleta rota
                this.Remove(contexto, rotaId);
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

        private void Remove(DataContext contexto, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Transporte.ROTA
                                      WHERE  ROTAID = @ROTAID";


            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaAprovacao(int rotaId, string usuario)
        {
            RotaAluno rnRotaAluno = new RotaAluno();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi aprovado
                    if (this.EhAprovadaPor(contexto, rotaId))
                    {
                        mensagens.Add("Esta rota já foi aprovada");
                    }

                    //Verifica se está ativa
                    if (!this.EhAtivaPor(contexto, rotaId))
                    {
                        mensagens.Add("Esta rota não pode ser aprovada pois está desativada.");
                    }

                    //Verifica se tem prestador contudor e veiculo na Ida e Volta
                    if (!this.PossuiCondutorVeiculoPrestadorPor(contexto, rotaId, true))
                    {
                        mensagens.Add("Esta rota não pode ser aprovada pois não possui PRESTADOR / CONTUDOR / VEICULO associado ao trajeto de ida.");
                    }
                    if (!this.PossuiCondutorVeiculoPrestadorPor(contexto, rotaId, false))
                    {
                        mensagens.Add("Esta rota não pode ser aprovada pois não possui PRESTADOR / CONTUDOR / VEICULO associado ao trajeto de volta.");
                    }

                    //Verifica se tem aluno na Ida e Volta                    
                    if (!rnRotaAluno.PossuiAlunoAtivoPor(contexto, rotaId, true))
                    {
                        mensagens.Add("Esta rota não pode ser aprovada pois não possui ALUNO associado ao trajeto de ida.");
                    }
                    if (!rnRotaAluno.PossuiAlunoAtivoPor(contexto, rotaId, false))
                    {
                        mensagens.Add("Esta rota não pode ser aprovada pois não possui ALUNO associado ao trajeto de volta.");
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

        public void Aprova(int rotaId, string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();

            try
            {
                string municipio = this.ObtemMunicipioPor(ctx, rotaId);
                DateTime prazoFinal = rnDiasNaoLetivos.RetornaDataFinalPor(ctx, DateTime.Now, 7, municipio);

                contextQuery.Command = @" UPDATE Transporte.ROTA
                                            SET APROVADO = 1,
                                                DATALIMITEEDICAO = @DATALIMITEEDICAO,
                                                DATALIMITEEDICAOALUNO = @DATALIMITEEDICAOALUNO,
                                                USUARIOAPROVACAOID = @USUARIOAPROVACAOID,
                                                DATAALTERACAO = @DATAALTERACAO
                                            WHERE ROTAID = @ROTAID ";

                contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
                contextQuery.Parameters.Add("@DATALIMITEEDICAO", SqlDbType.DateTime, prazoFinal.Date);
                contextQuery.Parameters.Add("@DATALIMITEEDICAOALUNO", SqlDbType.DateTime, prazoFinal.Date);
                contextQuery.Parameters.Add("@USUARIOAPROVACAOID", SqlDbType.VarChar, usuario);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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

        public ValidacaoDados ValidaReaberturaEdicao(int rotaId, string usuario)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (rotaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (usuario.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já foi aprovado
                    if (!this.EhAprovadaPor(contexto, rotaId))
                    {
                        mensagens.Add("Esta rota ainda não foi aprovada.");
                    }

                    //Verifica se está ativa
                    if (!this.EhAtivaPor(contexto, rotaId))
                    {
                        mensagens.Add("Está rota está desativada.");
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

        public void ReabreEdicaoAluno(int rotaId, string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();

            try
            {
                string municipio = this.ObtemMunicipioPor(ctx, rotaId);
                DateTime prazoFinal = rnDiasNaoLetivos.RetornaDataFinalPor(ctx, DateTime.Now, 7, municipio);

                contextQuery.Command = @" UPDATE Transporte.ROTA
                                            SET DATALIMITEEDICAOALUNO = @DATALIMITEEDICAOALUNO,
                                                USUARIOAPROVACAOID = @USUARIOAPROVACAOID,
                                                DATAALTERACAO = @DATAALTERACAO
                                            WHERE ROTAID = @ROTAID ";

                contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);
                contextQuery.Parameters.Add("@DATALIMITEEDICAOALUNO", SqlDbType.DateTime, prazoFinal.Date);
                contextQuery.Parameters.Add("@USUARIOAPROVACAOID", SqlDbType.VarChar, usuario);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

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
    }
}
