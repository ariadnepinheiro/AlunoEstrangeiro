using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.PrestacaoContas.DTO;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class Evento : RNBase
    {
        public enum TipoDespesa
        {
            [StringValue("Despesa com NF-e")]
            DespesaComum = 0,
            [StringValue("Despesa com Demais Documentos Fiscais")]
            DespesaDocumentosFiscais = 1,
            [StringValue("Pequena Despesa")]
            PequenaDespesaComComprovacao = 2,
            [StringValue("Despesa com Locomoção de Servidores")]
            PequenaDespesaComTransladoServidores = 3,
            [StringValue("Pequena Despesa sem comprovacao")]
            PequenaDespesaSemComprovacao = 4
        }

        public DTOs.DadosEvento ObtemDadosEventoPor(int eventoId)
        {
            OrcamentoArquivo rnOrcamentoArquivo = new OrcamentoArquivo();
            DTOs.DadosEvento eventoGeral = new DTOs.DadosEvento();
            DTOs.DadosEvento dadosEvento = new DTOs.DadosEvento();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            PequenaDespesaServidor rnPequenaDespesaServidor = new PequenaDespesaServidor();

            try
            {
                //Busca Tipo do evento e dados gerais
                eventoGeral = this.ObtemDadosEventoPor(contexto, eventoId);

                //Verifica tipo se é evento simples
                if (eventoGeral.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum || eventoGeral.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                {

                    //Alimenta dados de Evento Simples
                    dadosEvento = this.ObtemDadosEventoSimplesPor(contexto, eventoId);

                    dadosEvento.TipoDespesa = eventoGeral.TipoDespesa;

                    //Buscar Orcamentos
                    DataTable orcamentos = rnOrcamentoArquivo.ListaOrcamentoPor(contexto, eventoId);

                    if (orcamentos.Rows.Count > 0)
                    {
                        dadosEvento.Orcamento1Id = Convert.ToInt32(orcamentos.Rows[0]["ORCAMENTOARQUIVOID"]);
                        dadosEvento.Orcamento1Arquivo = (byte[])orcamentos.Rows[0]["ARQUIVO"];
                        dadosEvento.Orcamento1NomeArquivo = Convert.ToString(orcamentos.Rows[0]["NOMEARQUIVO"]);
                        dadosEvento.Orcamento1TipoArquivo = orcamentos.Rows[0]["TIPOARQUIVO"].ToString();

                        if (orcamentos.Rows.Count > 1)
                        {
                            dadosEvento.Orcamento2Id = Convert.ToInt32(orcamentos.Rows[1]["ORCAMENTOARQUIVOID"]);
                            dadosEvento.Orcamento2Arquivo = (byte[])orcamentos.Rows[1]["ARQUIVO"];
                            dadosEvento.Orcamento2NomeArquivo = Convert.ToString(orcamentos.Rows[1]["NOMEARQUIVO"]);
                            dadosEvento.Orcamento2TipoArquivo = orcamentos.Rows[1]["TIPOARQUIVO"].ToString();

                            if (orcamentos.Rows.Count > 2)
                            {
                                dadosEvento.Orcamento3Id = Convert.ToInt32(orcamentos.Rows[2]["ORCAMENTOARQUIVOID"]);
                                dadosEvento.Orcamento3Arquivo = (byte[])orcamentos.Rows[2]["ARQUIVO"];
                                dadosEvento.Orcamento3NomeArquivo = Convert.ToString(orcamentos.Rows[2]["NOMEARQUIVO"]);
                                dadosEvento.Orcamento3TipoArquivo = orcamentos.Rows[2]["TIPOARQUIVO"].ToString();
                            }
                        }
                    }
                }

                //Verifica tipo se é pequenas despesas com comprovacao
                if (eventoGeral.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao)
                {
                    dadosEvento = this.ObtemDadosEventoPequenasDespesasComComprovacaoPor(contexto, eventoId);
                }

                //Verifica tipo se é pequenas despesas sem comprovacao
                if (eventoGeral.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao)
                {
                    dadosEvento = this.ObtemDadosEventoPequenasDespesasSemComprovacaoPor(contexto, eventoId);
                }

                //Verifica tipo se é pequena despesa transporte
                if (eventoGeral.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores)
                {
                    dadosEvento = this.ObtemDadosEventoTransportePor(contexto, eventoId);
                    dadosEvento.Servidores = rnPequenaDespesaServidor.ObtemDadosPequenaDespesaServidorPorEventoId(contexto, eventoId).ToList();
                }

                //Alimenta dados gerais
                dadosEvento.PlanoTrabalhoId = eventoGeral.PlanoTrabalhoId;
                dadosEvento.Descricao = eventoGeral.Descricao;
                dadosEvento.Censo = eventoGeral.Censo;
                dadosEvento.EventoId = eventoId;
                dadosEvento.NumeroEvento = eventoGeral.NumeroEvento;
                dadosEvento.TipoDespesa = eventoGeral.TipoDespesa;
                dadosEvento.DescricaoTipoDespesa = eventoGeral.DescricaoTipoDespesa;
                dadosEvento.Aprovado = eventoGeral.Aprovado;
                dadosEvento.DataAprovacao = eventoGeral.DataAprovacao;
                dadosEvento.PlanoTrabalhoDescricao = eventoGeral.PlanoTrabalhoDescricao;
                dadosEvento.CensoNomeComp = eventoGeral.CensoNomeComp;
                dadosEvento.FinalidadeId = eventoGeral.FinalidadeId;
                dadosEvento.FinalidadeDescricao = eventoGeral.FinalidadeDescricao;
                dadosEvento.TodasExigenciasAprovadas = eventoGeral.TodasExigenciasAprovadas;

                return dadosEvento;
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

        public DTOs.DadosEvento ObtemDadosEventoPorChave(string chave)
        {
            OrcamentoArquivo rnOrcamentoArquivo = new OrcamentoArquivo();
            DTOs.DadosEvento eventoGeral = null;
            DTOs.DadosEvento dadosEvento = new DTOs.DadosEvento();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                eventoGeral = this.ObtemDadosEventoPorChave(contexto, chave);

                if (eventoGeral == null)
                    return null;

                if (eventoGeral.EventoId == 0)
                    return null;

                if (eventoGeral.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum ||
                    eventoGeral.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                {
                    dadosEvento = this.ObtemDadosEventoSimplesPor(contexto, eventoGeral.EventoId);

                    if (dadosEvento == null)
                        dadosEvento = new DTOs.DadosEvento();

                    DataTable orcamentos = rnOrcamentoArquivo.ListaOrcamentoPor(contexto, eventoGeral.EventoId);

                    if (orcamentos != null && orcamentos.Rows.Count > 0)
                    {
                        dadosEvento.Orcamento1Id = Convert.ToInt32(orcamentos.Rows[0]["ORCAMENTOARQUIVOID"]);
                        dadosEvento.Orcamento1Arquivo = (byte[])orcamentos.Rows[0]["ARQUIVO"];
                        dadosEvento.Orcamento1NomeArquivo = Convert.ToString(orcamentos.Rows[0]["NOMEARQUIVO"]);
                        dadosEvento.Orcamento1TipoArquivo = orcamentos.Rows[0]["TIPOARQUIVO"].ToString();

                        if (orcamentos.Rows.Count > 1)
                        {
                            dadosEvento.Orcamento2Id = Convert.ToInt32(orcamentos.Rows[1]["ORCAMENTOARQUIVOID"]);
                            dadosEvento.Orcamento2Arquivo = (byte[])orcamentos.Rows[1]["ARQUIVO"];
                            dadosEvento.Orcamento2NomeArquivo = Convert.ToString(orcamentos.Rows[1]["NOMEARQUIVO"]);
                            dadosEvento.Orcamento2TipoArquivo = orcamentos.Rows[1]["TIPOARQUIVO"].ToString();

                            if (orcamentos.Rows.Count > 2)
                            {
                                dadosEvento.Orcamento3Id = Convert.ToInt32(orcamentos.Rows[2]["ORCAMENTOARQUIVOID"]);
                                dadosEvento.Orcamento3Arquivo = (byte[])orcamentos.Rows[2]["ARQUIVO"];
                                dadosEvento.Orcamento3NomeArquivo = Convert.ToString(orcamentos.Rows[2]["NOMEARQUIVO"]);
                                dadosEvento.Orcamento3TipoArquivo = orcamentos.Rows[2]["TIPOARQUIVO"].ToString();
                            }
                        }
                    }
                }

                //Alimenta dados gerais
                dadosEvento.PlanoTrabalhoId = eventoGeral.PlanoTrabalhoId;
                dadosEvento.Descricao = eventoGeral.Descricao;
                dadosEvento.Censo = eventoGeral.Censo;
                dadosEvento.EventoId = eventoGeral.EventoId;
                dadosEvento.NumeroEvento = eventoGeral.NumeroEvento;
                dadosEvento.TipoDespesa = eventoGeral.TipoDespesa;
                dadosEvento.DescricaoTipoDespesa = eventoGeral.DescricaoTipoDespesa;
                dadosEvento.Aprovado = eventoGeral.Aprovado;
                dadosEvento.DataAprovacao = eventoGeral.DataAprovacao;
                dadosEvento.PlanoTrabalhoDescricao = eventoGeral.PlanoTrabalhoDescricao;
                dadosEvento.CensoNomeComp = eventoGeral.CensoNomeComp;
                dadosEvento.FinalidadeId = eventoGeral.FinalidadeId;
                dadosEvento.FinalidadeDescricao = eventoGeral.FinalidadeDescricao;
                dadosEvento.TodasExigenciasAprovadas = eventoGeral.TodasExigenciasAprovadas;
                return dadosEvento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar evento: " + ex.Message);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public int ObtemTipoEventoPor(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return ObtemTipoEventoPor(contexto, eventoId);
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

        public int ObtemTipoEventoPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                    SELECT  E.TIPODESPESA
                    FROM PrestacaoContas.EVENTO E (NOLOCK)
	                WHERE E.EVENTOID = @EVENTOID
                ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                return contexto.GetReturnValue<int>(contextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerificaPeriodoPagamento(string periodo, string data)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            try
            {
                return this.VerificaPeriodoPagamento(contexto, periodo, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool VerificaPeriodoPagamento(DataContext contexto, string periodo, string data)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            var retorno = false;

            try
            {
                contextQuery.Command = @"
                    SELECT  [PERIODOREFERENCIAID]
                                  ,[ANO]
                                  ,[MESINICIAL]
                                  ,[MESFINAL]
                              FROM [LYCEUM].[PrestacaoContas].[PERIODOREFERENCIA]
                              where [PERIODOREFERENCIAID]  = @PERIODOID
                ";

                contextQuery.Parameters.Add("@PERIODOID", SqlDbType.Int, periodo);
                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    var ANO = reader["ANO"].ToString();
                    var MESINICIAL = reader["MESINICIAL"].ToString();
                    var MESFINAL = reader["MESFINAL"].ToString();

                    //Pega ultimo dia do mes
                    var dia = DateTime.DaysInMonth(Convert.ToInt32(ANO), Convert.ToInt32(MESFINAL));

                    if (Convert.ToDateTime("01/" + MESINICIAL + "/" + ANO) <= (Convert.ToDateTime(data)) &&
                        Convert.ToDateTime(dia + "/" + MESFINAL + "/" + ANO) >= (Convert.ToDateTime(data)))
                        retorno = true;
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

        private string BuscaPeriodoReferencia(string mesano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            var retorno = "0";

            try
            {
                contextQuery.Command = @"
                              SELECT  [PERIODOREFERENCIAID]
                                  ,[ANO]
                                  ,[MESINICIAL]
                                  ,[MESFINAL]
                              FROM [LYCEUM].[PrestacaoContas].[PERIODOREFERENCIA]
							  where iif(mesinicial<10, concat(ano,'0', mesinicial),concat(ano,mesinicial)) < @MESANO and
							         iif(mesfinal<10, concat(ano,'0', mesfinal),concat(ano,mesfinal)) > @MESANO
                ";

                contextQuery.Parameters.Add("@MESANO", SqlDbType.VarChar, mesano);
                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = reader["PERIODOREFERENCIAID"].ToString();


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

        public bool TemItensNFPor(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return TemItensNFPor(contexto, eventoId);
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

        public bool TemItensNFPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                    SELECT case when count(0) > 0 then 1 else 0 end 
                    FROM PrestacaoContas.IMPORTACAOXMLEVENTO IXMLE (NOLOCK)
	                WHERE IXMLE.EVENTOID = @EVENTOID
                ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                return contexto.GetReturnValue<bool>(contextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ObtemPlanoTrabalhoPor(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT PLANOTRABALHOID
                                    FROM PrestacaoContas.EVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID  ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PLANOTRABALHOID"]);
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }

            return retorno;
        }

        private PrestacaoContas.DTOs.DadosEvento ObtemDadosEventoPor(DataContext contexto, int eventoId)
        {
            PrestacaoContas.DTOs.DadosEvento dadosEvento = new PrestacaoContas.DTOs.DadosEvento();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                    SELECT  E.EVENTOID, 
		                    E.NUMEROEVENTO,
		                    CASE 
                                WHEN E.TIPODESPESA = 0 THEN 'Despesa com NF-e'
                                WHEN E.TIPODESPESA = 1 THEN 'Despesa com Demais Documentos Fiscais'
			                    WHEN E.TIPODESPESA = 2 THEN 'Pequena Despesa'
                                WHEN E.TIPODESPESA = 4 THEN 'Pequena Despesa Sem Comprovação'
                                WHEN E.TIPODESPESA = 3 THEN 'Despesa com Locomoção de Servidores'
                                ELSE 'Tipo desconhecido: ' + cast(E.TIPODESPESA as varchar)
                            END TIPO,
                            E.TIPODESPESA,
		                    E.APROVADO,
		                    E.DATAAPROVACAO,
		                    E.DESCRICAO,
		                    E.PLANOTRABALHOID,
		                    PT.DESCRICAO as PLANOTRABALHO_DESCRICAO,
		                    E.CENSO,
		                    P.NOME_COMPL as NOME_USUARIO,
		                    E.USUARIOID as MATRICULA,
		                    LYUE.NOME_COMP as CENSO_NOME_COMP,
		                    PT.FINALIDADEID,
		                    F.DESCRICAO as FINALIDADE_DESCRICAO,
                            case when not exists (select top 1 1 from PrestacaoContas.EXIGENCIAEVENTO where EVENTOID = @EVENTOID and APROVADO <> 1) then 1 else 0 end as TODASEXIGENCIASAPROVADAS
                    FROM PrestacaoContas.EVENTO e (NOLOCK)
	                    LEFT JOIN PRESTACAOCONTAS.PEQUENADESPESA PD (NOLOCK) ON E.EVENTOID = PD.EVENTOID
	                    inner join PrestacaoContas.PLANOTRABALHO PT (nolock) on e.PLANOTRABALHOID = PT.PLANOTRABALHOID
	                    inner join PrestacaoContas.FINALIDADE F (nolock) on PT.FINALIDADEID = F.FINALIDADEID
	                    inner join LY_UNIDADE_ENSINO LYUE (nolock) on e.CENSO = LYUE.UNIDADE_ENS
	                    LEFT JOIN USUARIO U ON U.USUARIO = E.USUARIOID
	                    LEFT JOIN LY_PESSOA P ON P.PESSOA = U.PESSOA_USUARIO
                    WHERE E.EVENTOID = @EVENTOID
                ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosEvento.PlanoTrabalhoId = Convert.ToInt32(reader["PLANOTRABALHOID"]);
                    dadosEvento.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    dadosEvento.Censo = Convert.ToString(reader["CENSO"]);
                    dadosEvento.DescricaoTipoDespesa = Convert.ToString(reader["TIPO"]);
                    dadosEvento.EventoId = Convert.ToInt32(reader["EVENTOID"]);
                    dadosEvento.NumeroEvento = Convert.ToString(reader["NUMEROEVENTO"]);
                    dadosEvento.TipoDespesa = Convert.ToInt32(reader["TIPODESPESA"]);
                    dadosEvento.Aprovado = Convert.ToString(reader["APROVADO"]).IsNullOrEmptyOrWhiteSpace() ? null : (bool?)reader["APROVADO"];
                    dadosEvento.DataAprovacao = reader["DATAAPROVACAO"] != DBNull.Value ? Convert.ToDateTime(reader["DATAAPROVACAO"]) : (DateTime?)null;
                    dadosEvento.PlanoTrabalhoDescricao = Convert.ToString(reader["PLANOTRABALHO_DESCRICAO"]);
                    dadosEvento.CensoNomeComp = Convert.ToString(reader["CENSO_NOME_COMP"]);
                    dadosEvento.FinalidadeId = Convert.ToInt32(reader["FINALIDADEID"]);
                    dadosEvento.FinalidadeDescricao = Convert.ToString(reader["FINALIDADE_DESCRICAO"]);
                    dadosEvento.TodasExigenciasAprovadas = Convert.ToBoolean(reader["TODASEXIGENCIASAPROVADAS"]);
                    dadosEvento.NomeUsuario = Convert.ToString(reader["NOME_USUARIO"]);
                    dadosEvento.UsuarioId = Convert.ToString(reader["MATRICULA"]);
                }

                return dadosEvento;
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

        private PrestacaoContas.DTOs.DadosEvento ObtemDadosEventoPorChave(DataContext contexto, string chave)
        {
            PrestacaoContas.DTOs.DadosEvento dadosEvento = new PrestacaoContas.DTOs.DadosEvento();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"
                        SELECT  E.EVENTOID, 
		                    E.NUMEROEVENTO,
		                    CASE 
                                WHEN E.TIPODESPESA = 0 THEN 'Despesa com NF-e'
                                WHEN E.TIPODESPESA = 1 THEN 'Despesa com Demais Documentos Fiscais'
			                    WHEN E.TIPODESPESA = 2 THEN 'Pequena Despesa'
                                WHEN E.TIPODESPESA = 4 THEN 'Pequena Despesa Sem Comprovação'
                                WHEN E.TIPODESPESA = 3 THEN 'Despesa com Locomoção de Servidores'
                                ELSE 'Tipo desconhecido: ' + cast(E.TIPODESPESA as varchar)
                            END TIPO,
                            E.TIPODESPESA,
                            E.CHAVEACESSO,
		                    E.APROVADO,
		                    E.DATAAPROVACAO,
		                    E.DESCRICAO,
		                    E.PLANOTRABALHOID,
		                    PT.DESCRICAO as PLANOTRABALHO_DESCRICAO,
		                    E.CENSO,
		                    P.NOME_COMPL as NOME_USUARIO,
		                    E.USUARIOID as MATRICULA,
		                    LYUE.NOME_COMP as CENSO_NOME_COMP,
		                    PT.FINALIDADEID,
		                    F.DESCRICAO as FINALIDADE_DESCRICAO,
                            case when not exists (select top 1 1 from PrestacaoContas.EXIGENCIAEVENTO where CHAVEACESSO = @CHAVEDEACESSO and APROVADO <> 1) then 1 else 0 end as TODASEXIGENCIASAPROVADAS
                    FROM PrestacaoContas.EVENTO e (NOLOCK)
	                    LEFT JOIN PRESTACAOCONTAS.PEQUENADESPESA PD (NOLOCK) ON E.EVENTOID = PD.EVENTOID
	                    inner join PrestacaoContas.PLANOTRABALHO PT (nolock) on e.PLANOTRABALHOID = PT.PLANOTRABALHOID
	                    inner join PrestacaoContas.FINALIDADE F (nolock) on PT.FINALIDADEID = F.FINALIDADEID
	                    inner join LY_UNIDADE_ENSINO LYUE (nolock) on e.CENSO = LYUE.UNIDADE_ENS
	                    LEFT JOIN USUARIO U ON U.USUARIO = E.USUARIOID
	                    LEFT JOIN LY_PESSOA P ON P.PESSOA = U.PESSOA_USUARIO
                    WHERE E.CHAVEACESSO LIKE '%' + @CHAVEDEACESSO + '%'
                ";

                contextQuery.Parameters.Add("@CHAVEDEACESSO", SqlDbType.VarChar, chave.Trim());

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosEvento.PlanoTrabalhoId = Convert.ToInt32(reader["PLANOTRABALHOID"]);
                    dadosEvento.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    dadosEvento.Censo = Convert.ToString(reader["CENSO"]);
                    dadosEvento.DescricaoTipoDespesa = Convert.ToString(reader["TIPO"]);
                    dadosEvento.EventoId = Convert.ToInt32(reader["EVENTOID"]);
                    dadosEvento.NumeroEvento = Convert.ToString(reader["NUMEROEVENTO"]);
                    dadosEvento.TipoDespesa = Convert.ToInt32(reader["TIPODESPESA"]);
                    dadosEvento.Aprovado = Convert.ToString(reader["APROVADO"]).IsNullOrEmptyOrWhiteSpace() ? null : (bool?)reader["APROVADO"];
                    dadosEvento.DataAprovacao = reader["DATAAPROVACAO"] != DBNull.Value ? Convert.ToDateTime(reader["DATAAPROVACAO"]) : (DateTime?)null;
                    dadosEvento.PlanoTrabalhoDescricao = Convert.ToString(reader["PLANOTRABALHO_DESCRICAO"]);
                    dadosEvento.CensoNomeComp = Convert.ToString(reader["CENSO_NOME_COMP"]);
                    dadosEvento.FinalidadeId = Convert.ToInt32(reader["FINALIDADEID"]);
                    dadosEvento.FinalidadeDescricao = Convert.ToString(reader["FINALIDADE_DESCRICAO"]);
                    dadosEvento.TodasExigenciasAprovadas = Convert.ToBoolean(reader["TODASEXIGENCIASAPROVADAS"]);
                    dadosEvento.NomeUsuario = Convert.ToString(reader["NOME_USUARIO"]);
                    dadosEvento.UsuarioId = Convert.ToString(reader["MATRICULA"]);
                }

                return dadosEvento;
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

        private PrestacaoContas.DTOs.DadosEvento ObtemDadosEventoSimplesPor(DataContext contexto, int eventoId)
        {
            PrestacaoContas.DTOs.DadosEvento dadosEvento = new PrestacaoContas.DTOs.DadosEvento();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT  E.EVENTOID,  
		                                            E.FORNECEDORID,
		                                            E.JUSTIFICATIVAORCAMENTO,
		                                            E.CHAVEACESSO, 
		                                            E.TIPODESPESA,
		                                            E.NUMERONOTAFISCAL,
		                                            E.VALORNOTAFISCAL,
		                                            E.VALORPAGAMENTO,
		                                            E.DATANOTAFISCAL,
		                                            E.DATAPAGAMENTO,
		                                            E.USUARIOID AS MATRICULA,
                                                    P.NOME_COMPL AS NOME_USUARIO,
		                                            NF.EVENTONOTAFISCALARQUIVOID, 
		                                            NF.TIPOARQUIVO AS NOTAFISCALTIPOARQUIVO,
		                                            NF.ARQUIVO AS NOTAFISCALARQUIVO,
		                                            NF.NOMEARQUIVO AS NOTAFISCALNOMEARQUIVO,
		                                            C.COMPROVANTEPAGAMENTOARQUIVOID,
		                                            C.ARQUIVO AS COMPROVANTEPAGAMENTOARQUIVO,
		                                            C.NOMEARQUIVO AS COMPROVANTEPAGAMENTONOMEARQUIVO, 
		                                            C.TIPOARQUIVO AS COMPROVANTEPAGAMENTOTIPOARQUIVO,
		                                            EE.EVIDENCIAARQUIVOID,
		                                            EE.ARQUIVO AS EVIDENCIAARQUIVO,
		                                            EE.NOMEARQUIVO AS EVIDENCIANOMEARQUIVO, 
		                                            EE.TIPOARQUIVO AS EVIDENCIATIPOARQUIVO,
		                                            E.OBSERVACOES,
		                                            E.EVIDENCIAS,
													CASE 
														WHEN X.IMPORTACAOXMLEVENTOID IS NOT NULL THEN 1
														ELSE 0
													END POSSUIXMLIMPORTADO
                                            FROM PrestacaoContas.EVENTO e (NOLOCK)
	                                            INNER JOIN PrestacaoContas.EVENTONOTAFISCALARQUIVO NF (NOLOCK) 
			                                            ON E.EVENTOID = NF.EVENTOID
	                                            INNER JOIN PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO c (NOLOCK) 
			                                            ON E.EVENTOID = C.EVENTOID
												LEFT JOIN PrestacaoContas.IMPORTACAOXMLEVENTO X (NOLOCK)
														ON E.EVENTOID = X.EVENTOID
	                                            LEFT JOIN PrestacaoContas.EVIDENCIAARQUIVO EE (NOLOCK)
			                                            ON E.EVENTOID = EE.EVENTOID
                                              	LEFT JOIN USUARIO U 
                                                        ON U.USUARIO = E.USUARIOID
	                                            LEFT JOIN LY_PESSOA P 
                                                        ON P.PESSOA = U.PESSOA_USUARIO
                                            WHERE E.EVENTOID = @EVENTOID 
                                                  AND E.TIPODESPESA IN (0,1) ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosEvento.JustificativaOrcamento = Convert.ToString(reader["JUSTIFICATIVAORCAMENTO"]);
                    dadosEvento.ChaveAcesso = Convert.ToString(reader["CHAVEACESSO"]);
                    dadosEvento.TipoDespesa = Convert.ToInt32(reader["TIPODESPESA"]);
                    dadosEvento.NumeroNotaFiscal = Convert.ToString(reader["NUMERONOTAFISCAL"]);
                    dadosEvento.ValorNotaFiscal = Convert.ToDecimal(reader["VALORNOTAFISCAL"]);
                    dadosEvento.DataNotaFiscal = reader["DATANOTAFISCAL"] != DBNull.Value ? Convert.ToDateTime(reader["DATANOTAFISCAL"]) : (DateTime?)null;
                    dadosEvento.FornecedorId = Convert.ToInt32(reader["FORNECEDORID"]);
                    dadosEvento.ValorPagamento = Convert.ToDecimal(reader["VALORPAGAMENTO"]);
                    dadosEvento.DataPagamento = reader["DATAPAGAMENTO"] != DBNull.Value ? Convert.ToDateTime(reader["DATAPAGAMENTO"]) : (DateTime?)null;
                    dadosEvento.NotaFiscalArquivoId = Convert.ToInt32(reader["EVENTONOTAFISCALARQUIVOID"]);
                    dadosEvento.NotaFiscalArquivo = (byte[])reader["NOTAFISCALARQUIVO"];
                    dadosEvento.NotaFiscalNomeArquivo = Convert.ToString(reader["NOTAFISCALNOMEARQUIVO"]);
                    dadosEvento.NotaFiscalTipoArquivo = Convert.ToString(reader["NOTAFISCALTIPOARQUIVO"]);
                    dadosEvento.ComprovantePagamentoArquivoId = Convert.ToInt32(reader["COMPROVANTEPAGAMENTOARQUIVOID"]);
                    dadosEvento.ComprovantePagamentoArquivo = (byte[])reader["COMPROVANTEPAGAMENTOARQUIVO"];
                    dadosEvento.ComprovantePagamentoNomeArquivo = Convert.ToString(reader["COMPROVANTEPAGAMENTONOMEARQUIVO"]);
                    dadosEvento.ComprovantePagamentoTipoArquivo = Convert.ToString(reader["COMPROVANTEPAGAMENTOTIPOARQUIVO"]);
                    dadosEvento.EvidenciaArquivoId = reader["EVIDENCIAARQUIVOID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["EVIDENCIAARQUIVOID"]) : null;
                    dadosEvento.EvidenciaArquivo = reader["EVIDENCIAARQUIVO"] != DBNull.Value ? (byte[])reader["EVIDENCIAARQUIVO"] : new byte[] { };
                    dadosEvento.EvidenciaNomeArquivo = Convert.ToString(reader["EVIDENCIANOMEARQUIVO"]);
                    dadosEvento.EvidenciaTipoArquivo = Convert.ToString(reader["EVIDENCIATIPOARQUIVO"]);
                    dadosEvento.Observacoes = Convert.ToString(reader["OBSERVACOES"]);
                    dadosEvento.Evidencias = Convert.ToString(reader["EVIDENCIAS"]);
                    dadosEvento.PossuiXmlImportado = Convert.ToBoolean(reader["POSSUIXMLIMPORTADO"]);
                    dadosEvento.NomeUsuario = Convert.ToString(reader["NOME_USUARIO"]);
                    dadosEvento.UsuarioId = Convert.ToString(reader["MATRICULA"]);
                }

                return dadosEvento;
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

        private PrestacaoContas.DTOs.DadosEvento ObtemDadosEventoPequenasDespesasComComprovacaoPor(DataContext contexto, int eventoId)
        {
            PrestacaoContas.DTOs.DadosEvento dadosEvento = new PrestacaoContas.DTOs.DadosEvento();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT  E.EVENTOID, 
		                                            E.TIPODESPESA,
		                                            E.FORNECEDORID,
		                                            E.NUMERONOTAFISCAL,
		                                            E.VALORNOTAFISCAL,
                                                    E.DATANOTAFISCAL,
		                                            E.VALORPAGAMENTO,
		                                            E.DATAPAGAMENTO,
		                                            NF.EVENTONOTAFISCALARQUIVOID, 
		                                            NF.TIPOARQUIVO AS NOTAFISCALTIPOARQUIVO,
		                                            NF.ARQUIVO AS NOTAFISCALARQUIVO,
		                                            NF.NOMEARQUIVO AS NOTAFISCALNOMEARQUIVO,
                                                    CP.COMPROVANTEPAGAMENTOARQUIVOID, 
		                                            CP.TIPOARQUIVO AS COMPROVANTEPAGAMENTOTIPOARQUIVO,
		                                            CP.ARQUIVO AS COMPROVANTEPAGAMENTOARQUIVO,
		                                            CP.NOMEARQUIVO AS COMPROVANTEPAGAMENTONOMEARQUIVO,
		                                            PD.FORMAPAGAMENTO
                                            from PrestacaoContas.EVENTO e (NOLOCK)
	                                            INNER JOIN PRESTACAOCONTAS.PEQUENADESPESA PD (NOLOCK)
			                                            ON E.EVENTOID = PD.EVENTOID
	                                            INNER JOIN PrestacaoContas.EVENTONOTAFISCALARQUIVO NF (NOLOCK) 
			                                            ON E.EVENTOID = NF.EVENTOID
                                                LEFT JOIN PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO CP (NOLOCK)
                                                        ON E.EVENTOID = CP.EVENTOID
                                            WHERE E.EVENTOID = @EVENTOID
	                                              and PD.TIPODESPESA = 'PEQUENADESPESA'
                                                  and E.TIPODESPESA = 2 ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosEvento.FornecedorId = reader["FORNECEDORID"] != DBNull.Value ? Convert.ToInt32(reader["FORNECEDORID"]) : (int?)null;
                    dadosEvento.TipoDespesa = Convert.ToInt32(reader["TIPODESPESA"]);
                    dadosEvento.NumeroNotaFiscal = reader["NUMERONOTAFISCAL"] != DBNull.Value ? Convert.ToString(reader["NUMERONOTAFISCAL"]) : null;
                    dadosEvento.ValorNotaFiscal = reader["VALORNOTAFISCAL"] != DBNull.Value ? Convert.ToDecimal(reader["VALORNOTAFISCAL"]) : (decimal?)null;
                    dadosEvento.DataNotaFiscal = reader["DATANOTAFISCAL"] != DBNull.Value ? Convert.ToDateTime(reader["DATANOTAFISCAL"]) : (DateTime?)null;
                    dadosEvento.ValorPagamento = Convert.ToDecimal(reader["VALORPAGAMENTO"]);
                    dadosEvento.DataPagamento = reader["DATAPAGAMENTO"] != DBNull.Value ? Convert.ToDateTime(reader["DATAPAGAMENTO"]) : (DateTime?)null;
                    dadosEvento.NotaFiscalArquivoId = reader["EVENTONOTAFISCALARQUIVOID"] != DBNull.Value ? Convert.ToInt32(reader["EVENTONOTAFISCALARQUIVOID"]) : (int?)null;
                    dadosEvento.NotaFiscalArquivo = (byte[])reader["NOTAFISCALARQUIVO"];
                    dadosEvento.NotaFiscalNomeArquivo = reader["NOTAFISCALNOMEARQUIVO"] != DBNull.Value ? Convert.ToString(reader["NOTAFISCALNOMEARQUIVO"]) : null;
                    dadosEvento.NotaFiscalTipoArquivo = reader["NOTAFISCALTIPOARQUIVO"] != DBNull.Value ? Convert.ToString(reader["NOTAFISCALTIPOARQUIVO"]) : null;
                    dadosEvento.ComprovantePagamentoArquivoId = reader["COMPROVANTEPAGAMENTOARQUIVOID"] != DBNull.Value ? Convert.ToInt32(reader["COMPROVANTEPAGAMENTOARQUIVOID"]) : (int?)null;

                    if (reader["COMPROVANTEPAGAMENTOARQUIVO"] != DBNull.Value)
                    {
                        dadosEvento.ComprovantePagamentoArquivo = (byte[])reader["COMPROVANTEPAGAMENTOARQUIVO"];
                    }
                    dadosEvento.ComprovantePagamentoNomeArquivo = reader["COMPROVANTEPAGAMENTONOMEARQUIVO"] != DBNull.Value ? Convert.ToString(reader["COMPROVANTEPAGAMENTONOMEARQUIVO"]) : null;
                    dadosEvento.ComprovantePagamentoTipoArquivo = reader["COMPROVANTEPAGAMENTOTIPOARQUIVO"] != DBNull.Value ? Convert.ToString(reader["COMPROVANTEPAGAMENTOTIPOARQUIVO"]) : null;
                    dadosEvento.FormaPagamento = reader["FORMAPAGAMENTO"] != DBNull.Value ? Convert.ToString(reader["FORMAPAGAMENTO"]) : null;
                }

                return dadosEvento;
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

        private PrestacaoContas.DTOs.DadosEvento ObtemDadosEventoPequenasDespesasSemComprovacaoPor(DataContext contexto, int eventoId)
        {
            PrestacaoContas.DTOs.DadosEvento dadosEvento = new PrestacaoContas.DTOs.DadosEvento();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT  E.EVENTOID,  
		                                            E.TIPODESPESA,
		                                            E.FORNECEDORID,
		                                            E.VALORPAGAMENTO,
		                                            E.DATAPAGAMENTO,
                                                    PD.JUSTIFICATIVA,
		                                            PD.FORMAPAGAMENTO
                                            from PrestacaoContas.EVENTO e (NOLOCK)
	                                            INNER JOIN PRESTACAOCONTAS.PEQUENADESPESA PD (NOLOCK)
			                                            ON E.EVENTOID = PD.EVENTOID
                                            WHERE E.EVENTOID = @EVENTOID
	                                              and PD.TIPODESPESA = 'SEMCOMPROVACAO'
                                                  and E.TIPODESPESA = 2 ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosEvento.FornecedorId = Convert.ToInt32(reader["FORNECEDORID"]);
                    dadosEvento.TipoDespesa = Convert.ToInt32(reader["TIPODESPESA"]);
                    dadosEvento.ValorPagamento = Convert.ToDecimal(reader["VALORPAGAMENTO"]);
                    dadosEvento.DataPagamento = Convert.ToDateTime(reader["DATAPAGAMENTO"]);
                    dadosEvento.Justificativa = Convert.ToString(reader["JUSTIFICATIVA"]);
                    dadosEvento.FormaPagamento = Convert.ToString(reader["FORMAPAGAMENTO"]);
                }

                return dadosEvento;
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

        private PrestacaoContas.DTOs.DadosEvento ObtemDadosEventoTransportePor(DataContext contexto, int eventoId)
        {
            PrestacaoContas.DTOs.DadosEvento dadosEvento = new PrestacaoContas.DTOs.DadosEvento();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT  E.EVENTOID, 
		                                            E.TIPODESPESA,
		                                            E.VALORPAGAMENTO,		
		                                            PD.TIPOTRANSPORTEID,
		                                            PD.ORIGEM,
		                                            PD.DESTINO,
                                                    E.DATAPAGAMENTO,
		                                            PD.JUSTIFICATIVA
                                            from PrestacaoContas.EVENTO e (NOLOCK)
	                                            INNER JOIN PRESTACAOCONTAS.PEQUENADESPESA PD (NOLOCK)
			                                            ON E.EVENTOID = PD.EVENTOID
	                                            --INNER JOIN PrestacaoContas.EVENTONOTAFISCALARQUIVO NF (NOLOCK) 
			                                    --        ON E.EVENTOID = NF.EVENTOID
                                            WHERE E.EVENTOID = @EVENTOID
	                                              and PD.TIPODESPESA = 'TRANSPORTE'
	                                              and E.TIPODESPESA = 3 ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosEvento.ValorPagamento = reader["VALORPAGAMENTO"] != DBNull.Value ? Convert.ToDecimal(reader["VALORPAGAMENTO"]) : 0;
                    dadosEvento.TipoDespesa = reader["TIPODESPESA"] != DBNull.Value ? Convert.ToInt32(reader["TIPODESPESA"]) : 0;
                    dadosEvento.TipoTransporteId = reader["TIPOTRANSPORTEID"] != DBNull.Value ? Convert.ToInt32(reader["TIPOTRANSPORTEID"]) : 0;
                    dadosEvento.Origem = reader["ORIGEM"] != DBNull.Value ? Convert.ToString(reader["ORIGEM"]) : "";
                    dadosEvento.Destino = reader["DESTINO"] != DBNull.Value ? Convert.ToString(reader["DESTINO"]) : "";
                    dadosEvento.DataPagamento = reader["DATAPAGAMENTO"] != DBNull.Value ? Convert.ToDateTime(reader["DATAPAGAMENTO"]) : (DateTime?)null;
                    dadosEvento.Justificativa = reader["JUSTIFICATIVA"] != DBNull.Value ? Convert.ToString(reader["JUSTIFICATIVA"]) : "";
                }

                return dadosEvento;
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

        public ValidacaoDados Valida(DTOs.DadosEvento dados, DadosArquivoXml arquivoXml, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            List<DadosXmlItem> itensNotaFiscal = new List<DadosXmlItem>();
            PlanoTrabalho rnPlanoTrabalho = new PlanoTrabalho();
            Fornecedor rnFornecedor = new Fornecedor();
            string cnpjChave = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dados == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (dados.EventoId <= 0)
                {
                    mensagens.Add("Campo DESPESA é obrigatório para alteraçao.");
                }
            }

            if (dados.PlanoTrabalhoId <= 0)
            {
                mensagens.Add("Campo PROJETO / PROGRAMA é obrigatório.");
            }

            if (dados.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UNIDADE DE ENSINO é obrigatório.");
            }

            if (dados.Descricao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DESCRIÇÃO DA DESPESA é obrigatório.");
            }
            else if (dados.Descricao.Length > 500)
            {
                mensagens.Add("Campo DESCRIÇÃO DA DESPESA deve conter no máximo 500 caracteres.");
            }

            if (dados.TipoDespesa < 0)
            {
                mensagens.Add("Campo TIPO DA DESPESA é obrigatório.");
            }
            else
            {
                if (dados.TipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum
                    && dados.TipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais
                    && dados.TipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao
                    && dados.TipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores)
                {
                    mensagens.Add("Campo TIPO DO DESPESA é inválido.");
                }
                else
                {
                    //Verifica tipo se é evento simples
                    if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum
                        || dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                    {
                        if (dados.Orcamento1Arquivo == null || dados.Orcamento1Arquivo.Count() <= 0)
                        {
                            mensagens.Add("É obrigatório anexar o orçamento 1.");
                        }
                        else
                        {
                            if (dados.Orcamento1NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo NOME ARQUIVO DO ORÇAMENTO 1 é obrigatório.");
                            }

                            if (dados.Orcamento1TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo TIPO ARQUIVO DO ORÇAMENTO 1 é obrigatório.");
                            }
                            else
                            {
                                //Apenas aceitar pdf
                                if (dados.Orcamento1TipoArquivo.ToUpper() != "APPLICATION/PDF")
                                {
                                    mensagens.Add("ORÇAMENTO 1 - Apenas serão aceitos arquivos dos tipos .pdf .");
                                }
                            }

                            if (dados.Orcamento2Arquivo != null && dados.Orcamento1Arquivo.Count() > 0)
                            {
                                if (dados.Orcamento2NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo NOME ARQUIVO DO ORÇAMENTO 2 é obrigatório.");
                                }

                                if (dados.Orcamento2TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo TIPO ARQUIVO DO ORÇAMENTO 2 é obrigatório.");
                                }
                                else
                                {
                                    //Apenas aceitar pdf
                                    if (dados.Orcamento2TipoArquivo.ToUpper() != "APPLICATION/PDF")
                                    {
                                        mensagens.Add("ORÇAMENTO 2 - Apenas serão aceitos arquivos dos tipos .pdf .");
                                    }
                                }
                            }

                            if (dados.Orcamento3Arquivo != null && dados.Orcamento1Arquivo.Count() > 0)
                            {
                                if (dados.Orcamento3NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo NOME ARQUIVO DO ORÇAMENTO 3 é obrigatório.");
                                }

                                if (dados.Orcamento3TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo TIPO ARQUIVO DO ORÇAMENTO 3 é obrigatório.");
                                }
                                else
                                {
                                    //Apenas aceitar pdf
                                    if (dados.Orcamento3TipoArquivo.ToUpper() != "APPLICATION/PDF")
                                    {
                                        mensagens.Add("ORÇAMENTO 3 - Apenas serão aceitos arquivos dos tipos .pdf .");
                                    }
                                }
                            }

                            if ((dados.Orcamento2Arquivo == null || dados.Orcamento2Arquivo.Count() <= 0
                                || dados.Orcamento3Arquivo == null || dados.Orcamento3Arquivo.Count() <= 0)
                                && dados.JustificativaOrcamento.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo JUSTIFICATIVA é obrigatório quando não são anexados 3 orçamentos.");
                            }

                            if (!dados.JustificativaOrcamento.IsNullOrEmptyOrWhiteSpace() && dados.JustificativaOrcamento.Length > 500)
                            {
                                mensagens.Add("Campo JUSTIFICATIVA deve conter no máximo 500 caracteres.");
                            }
                        }

                        if (dados.FornecedorId == null || dados.FornecedorId <= 0)
                        {
                            mensagens.Add("Campo FORNECEDOR é obrigatório.");
                        }

                        if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum)
                        {
                            if (dados.ChaveAcesso.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("Campo CHAVE DE ACESSO é obrigatório.");
                            }
                            else
                            {
                                if (!dados.ChaveAcesso.EhNumerico())
                                {
                                    mensagens.Add("Campo CHAVE DE ACESSO precisa ser numérico.");
                                }

                                if (dados.ChaveAcesso.Length != 44)
                                {
                                    mensagens.Add("Campo CHAVE DE ACESSO precisa ter 44 dígitos.");
                                }

                                //AAMM – Ano e Mês de emissão da NF-e
                                if (dados.ChaveAcesso.Substring(2, 4) != dados.DataNotaFiscal.Value.ToString("yyMM"))
                                {
                                    mensagens.Add("O ANO/MÊS informado na CHAVE DE ACESSO não conferem.");
                                }
                                cnpjChave = dados.ChaveAcesso.Substring(6, 14);

                                //Validação retirada a pedido do chamado 26722, pq MEI nao tem cnpj na nota, Ver outra solução marcando MEI no fornecedor pra nao validar apenas no MEI
                                //if (!Techne.Lyceum.RN.Validacao.ValidaCnpj(dados.ChaveAcesso.Substring(6, 14)))
                                //{
                                //    mensagens.Add("O CNPJ/CPF informado na CHAVE DE ACESSO é inválido.");
                                //}

                                if (!dados.ChaveAcesso.ValidaChaveNotaFiscal())
                                {
                                    mensagens.Add("CHAVE DE ACESSO inválida conforme cálculo do dígito verificador.");
                                }
                            }
                        }

                        if (dados.NumeroNotaFiscal.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo NÚMERO DA NOTA FISCAL é obrigatório.");
                        }

                        if (dados.ValorNotaFiscal == null || dados.ValorNotaFiscal <= 0)
                        {
                            mensagens.Add("Campo VALOR DA NOTA FISCAL é obrigatório.");
                        }

                        if (dados.DataNotaFiscal == null || dados.DataNotaFiscal == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA DA NOTA FISCAL é obrigatório.");
                        }
                        else
                        {

                            DateTime dataNF;
                            if (!DateTime.TryParse(dados.DataNotaFiscal.ToString(), out dataNF))
                            {
                                mensagens.Add("Campo DATA DA NOTA FISCAL inválida.");
                            }
                        }

                        if (dados.ValorPagamento <= 0)
                        {
                            mensagens.Add("Campo VALOR PAGAMENTO é obrigatório.");
                        }
                        else if (dados.ValorNotaFiscal != null || dados.ValorNotaFiscal > 0)
                        {
                            //O campo observação será obrigatório de ser preenchido em casos em que o valor informado de pagamento de nota fiscal for menor do que o valor informado para nota fiscal
                            if (dados.ValorPagamento != dados.ValorNotaFiscal && dados.Observacoes.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add("O campo OBSERVAÇÃO é obrigatório quando o VALOR PAGAMENTO for diferente do VALOR DA NOTA FISCAL.");

                            }
                        }

                        if (dados.DataPagamento == null || dados.DataPagamento == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA PAGAMENTO é obrigatório.");
                        }
                        else
                        {

                            DateTime dataPagamento;
                            if (!DateTime.TryParse(dados.DataPagamento.ToString(), out dataPagamento))
                            {
                                mensagens.Add("Campo DATA PAGAMENTO inválida.");
                            }
                        }

                        if (dados.NotaFiscalArquivo == null || dados.NotaFiscalArquivo.Count() <= 0)
                        {
                            mensagens.Add("Campo NOTA FISCAL é obrigatório.");
                        }

                        if (dados.DataNotaFiscal == null || dados.DataNotaFiscal == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA DA NOTA FISCAL é obrigatório.");
                        }
                        else
                        {

                            DateTime datanf;
                            if (!DateTime.TryParse(dados.DataNotaFiscal.ToString(), out datanf))
                            {
                                mensagens.Add("Campo DATA DA NOTA FISCAL inválida.");
                            }
                        }

                        //Apenas aceitar pdf e imagem 
                        if (dados.NotaFiscalArquivo != null
                            && dados.NotaFiscalTipoArquivo.ToUpper() != "IMAGE/JPEG"
                            && dados.NotaFiscalTipoArquivo.ToUpper() != "APPLICATION/PDF")
                        {
                            mensagens.Add("NOTA FISCAL - Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                        }

                        if (dados.ComprovantePagamentoArquivo == null || dados.ComprovantePagamentoArquivo.Count() <= 0)
                        {
                            mensagens.Add("Campo COMPROVANTE PAGAMENTO é obrigatório.");
                        }

                        //Apenas aceitar pdf e imagem 
                        if (dados.ComprovantePagamentoArquivo != null
                            && dados.ComprovantePagamentoTipoArquivo.ToUpper() != "IMAGE/JPEG"
                            && dados.ComprovantePagamentoTipoArquivo.ToUpper() != "APPLICATION/PDF")
                        {
                            mensagens.Add("COMPROVANTE DE PAGAMENTO - Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                        }

                        if (!dados.Observacoes.IsNullOrEmptyOrWhiteSpace() && dados.Observacoes.Length > 1000)
                        {
                            mensagens.Add("Campo OBSERVAÇÕES deve conter no máximo 1000 caracteres.");
                        }

                        if (!dados.Evidencias.IsNullOrEmptyOrWhiteSpace() && dados.Evidencias.Length > 500)
                        {
                            mensagens.Add("Campo EVIDÊNCIAS deve conter no máximo 500 caracteres.");
                        }

                        if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum || dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                        {

                            //Verifica se tem arquivo XML
                            if (arquivoXml != null && arquivoXml.ArquivoXml != null)
                            {
                                if (arquivoXml.NomeArquivo.IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo NOME ARQUIVO XML é obrigatório.");
                                }

                                if (arquivoXml.TipoArquivo.IsNullOrEmptyOrWhiteSpace())
                                {
                                    mensagens.Add("Campo TIPO ARQUIVO XML é obrigatório.");
                                }
                                else
                                {
                                    if (arquivoXml.TipoArquivo.ToUpper() != "TEXT/XML")
                                    {
                                        mensagens.Add("XML - Apenas serão aceitos arquivos do tipo .xml.");
                                    }
                                    else
                                    {
                                        Stream inputStream2 = arquivoXml.ArquivoXml;
                                        XmlDocument arquivo = new XmlDocument();

                                        try
                                        {
                                            arquivo.Load(inputStream2);

                                            XmlNodeList infNFe_prod = arquivo.GetElementsByTagName("prod");

                                            if (infNFe_prod.Count <= 0)
                                            {
                                                mensagens.Add("XML Inválido: Não foram encontrados produtos.");
                                            }
                                            else
                                            {
                                                int contador = 1;
                                                foreach (XmlElement nodo1 in infNFe_prod)
                                                {
                                                    DadosXmlItem item = new DadosXmlItem();

                                                    //ITEMS OBRIGATORIOS NA NOTA
                                                    if (nodo1.GetElementsByTagName("xProd")[0] == null)
                                                    {
                                                        mensagens.Add(string.Format("XML Inválido: Não foi encontrado DESCRIÇÃO (xProd) no produto {0}.", contador.ToString()));
                                                    }
                                                    else
                                                    {
                                                        string xProd = nodo1.GetElementsByTagName("xProd")[0].InnerText;

                                                        if (xProd.IsNullOrEmptyOrWhiteSpace())
                                                        {
                                                            mensagens.Add(string.Format("XML Inválido: Campo DESCRIÇÃO (xProd) no produto {0} está vazio.", contador.ToString()));
                                                        }
                                                        else
                                                        {
                                                            item.xProd = xProd;
                                                        }
                                                    }

                                                    if (nodo1.GetElementsByTagName("qCom")[0] == null)
                                                    {
                                                        mensagens.Add(string.Format("XML Inválido: Não foi encontrado QUANTIDADE (qCom) no produto {0}.", contador.ToString()));
                                                    }
                                                    else
                                                    {
                                                        string qCom = nodo1.GetElementsByTagName("qCom")[0].InnerText;

                                                        if (qCom.IsNullOrEmptyOrWhiteSpace())
                                                        {
                                                            mensagens.Add(string.Format("XML Inválido: Campo QUANTIDADE (qCom) no produto {0} está vazio.", contador.ToString()));
                                                        }
                                                        else
                                                        {
                                                            item.qCom = Convert.ToDecimal(qCom, CultureInfo.InvariantCulture);
                                                        }
                                                    }

                                                    if (nodo1.GetElementsByTagName("vUnCom")[0] == null)
                                                    {
                                                        mensagens.Add(string.Format("XML Inválido: Não foi encontrado VALOR UNITÁRIO (vUnCom) no produto {0}.", contador.ToString()));
                                                    }
                                                    else
                                                    {
                                                        string vUnCom = nodo1.GetElementsByTagName("vUnCom")[0].InnerText;

                                                        if (vUnCom.IsNullOrEmptyOrWhiteSpace())
                                                        {
                                                            mensagens.Add(string.Format("XML Inválido: Campo VALOR UNITÁRIO (vUnCom) no produto {0} está vazio.", contador.ToString()));
                                                        }
                                                        else
                                                        {
                                                            item.vUnCom = Convert.ToDecimal(vUnCom, CultureInfo.InvariantCulture);
                                                        }
                                                    }

                                                    //ITEMS NÃO OBRIGATORIOS NA NOTA
                                                    if (nodo1.GetElementsByTagName("cProd")[0] != null
                                                        && !nodo1.GetElementsByTagName("cProd")[0].InnerText.IsNullOrEmptyOrWhiteSpace())
                                                    {
                                                        item.cProd = nodo1.GetElementsByTagName("cProd")[0].InnerText;
                                                    }

                                                    if (nodo1.GetElementsByTagName("cEAN")[0] != null
                                                        && !nodo1.GetElementsByTagName("cEAN")[0].InnerText.IsNullOrEmptyOrWhiteSpace())
                                                    {
                                                        item.cEAN = nodo1.GetElementsByTagName("cEAN")[0].InnerText;
                                                    }

                                                    if (nodo1.GetElementsByTagName("NCM")[0] != null
                                                        && !nodo1.GetElementsByTagName("NCM")[0].InnerText.IsNullOrEmptyOrWhiteSpace())
                                                    {
                                                        item.NCM = nodo1.GetElementsByTagName("NCM")[0].InnerText;
                                                    }

                                                    itensNotaFiscal.Add(item);
                                                    contador++;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            mensagens.Add("Erro ao carregar o XML. Provavelmente está mal-formado ou corrompido. Obtenha um novo XML e tente novamente." + Environment.NewLine + "Descrição do erro do XML: " + ex.Message);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Verifica se a despesá de de merenda
                                if (dados.FinalidadeId == 2)
                                {
                                    //Verifica se está configurado para ignorar algumas regras de despesas e analise
                                    var retiradaValidacaoDespesa = Convert.ToBoolean(ConfigurationManager.AppSettings["RetiradaValidacaoDespesa"] ?? "false");

                                    if (retiradaValidacaoDespesa)
                                    {
                                        //Caso a validação esteja retirada e nao tiver xml, criar um linha generica para a nota fiscal
                                        DadosXmlItem item = new DadosXmlItem();
                                        item.xProd = "ITEM DE REPRESENTAÇÃO DA NF";//DESCRICAO                                        
                                        item.qCom = 1;//QUANTIDADE                                        
                                        item.vUnCom = Convert.ToDecimal(dados.ValorNotaFiscal);//VALORUNITARIO,
                                        itensNotaFiscal.Add(item);
                                    }
                                    else
                                    {
                                        mensagens.Add("É obrigatório anexar o XML da despesa.");
                                    }
                                }
                            }
                        }
                    }


                    //Verifica tipo se é pequenas despesas com comprovacao
                    if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao)
                    {
                        if (dados.FormaPagamento.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo FORMA DE PAGAMENTO é obrigatório.");
                        }

                        if (dados.FornecedorId == null || dados.FornecedorId <= 0)
                        {
                            mensagens.Add("Campo FORNECEDOR é obrigatório.");
                        }

                        if (dados.NumeroNotaFiscal.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo NÚMERO DA NOTA FISCAL é obrigatório.");
                        }

                        if (dados.ValorNotaFiscal == null || dados.ValorNotaFiscal <= 0)
                        {
                            mensagens.Add("Campo VALOR DA NOTA FISCAL é obrigatório.");
                        }

                        if (dados.ValorPagamento <= 0)
                        {
                            mensagens.Add("Campo VALOR PAGAMENTO é obrigatório.");
                        }
                        else if (dados.ValorNotaFiscal != null || dados.ValorNotaFiscal > 0)
                        {
                            //Em eventos simples o Valor de Pagamento da Nota Fiscal não poderá ser maior do que o valor da nota fiscal. 
                            if (dados.ValorPagamento != dados.ValorNotaFiscal)
                            {
                                mensagens.Add("Campo VALOR PAGAMENTO deve ser igual ao VALOR DA NOTA FISCAL.");
                            }
                        }

                        if (dados.DataPagamento == null || dados.DataPagamento == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA PAGAMENTO é obrigatório.");
                        }
                        else
                        {

                            DateTime dataPagamento;
                            if (!DateTime.TryParse(dados.DataPagamento.ToString(), out dataPagamento))
                            {
                                mensagens.Add("Campo DATA PAGAMENTO inválida.");
                            }
                        }

                        if (dados.NotaFiscalArquivo == null || dados.NotaFiscalArquivo.Count() <= 0)
                        {
                            mensagens.Add("Campo DOCUMENTO FISCAL é obrigatório.");
                        }

                        //Apenas aceitar pdf e imagem 
                        if (dados.NotaFiscalArquivo != null
                            && dados.NotaFiscalTipoArquivo.ToUpper() != "IMAGE/JPEG"
                            && dados.NotaFiscalTipoArquivo.ToUpper() != "APPLICATION/PDF")
                        {
                            mensagens.Add("NOTA FISCAL - Apenas serão aceitos arquivos dos tipos .jpeg e .pdf .");
                        }
                    }

                    //Verifica tipo se é pequenas despesas Sem comprovacao
                    if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao)
                    {
                        if (dados.FornecedorId == null || dados.FornecedorId <= 0)
                        {
                            mensagens.Add("Campo FORNECEDOR é obrigatório.");
                        }

                        if (dados.FormaPagamento.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo FORMA DE PAGAMENTO é obrigatório.");
                        }

                        if (dados.ValorPagamento <= 0)
                        {
                            mensagens.Add("Campo VALOR PAGAMENTO é obrigatório.");
                        }

                        if (dados.DataPagamento == null || dados.DataPagamento == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA PAGAMENTO é obrigatório.");
                        }
                        else
                        {
                            DateTime dataPagamento;
                            if (!DateTime.TryParse(dados.DataPagamento.ToString(), out dataPagamento))
                            {
                                mensagens.Add("Campo DATA PAGAMENTO inválida.");
                            }
                        }

                        if (dados.Justificativa.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo JUSTIFICATIVA é obrigatório.");
                        }
                    }

                    //Verifica tipo se é transporte
                    if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores)
                    {
                        if (dados.TipoTransporteId <= 0)
                        {
                            mensagens.Add("Campo MODAL é obrigatório.");
                        }

                        if (dados.Origem.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo ORIGEM é obrigatório.");
                        }

                        if (dados.Destino.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo DESTINO é obrigatório.");
                        }

                        if (dados.ValorPagamento <= 0)
                        {
                            mensagens.Add("Campo VALOR PAGAMENTO é obrigatório.");
                        }

                        if (dados.DataPagamento == null || dados.DataPagamento == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA PAGAMENTO é obrigatório.");
                        }
                        else
                        {
                            DateTime dataPagamento;
                            if (!DateTime.TryParse(dados.DataPagamento.ToString(), out dataPagamento))
                            {
                                mensagens.Add("Campo DATA PAGAMENTO inválida.");
                            }
                        }

                        if (dados.Justificativa.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo JUSTIFICATIVA é obrigatório.");
                        }
                        else if (dados.Justificativa.Length > 500)
                        {
                            mensagens.Add("Campo JUSTIFICATIVA deve conter no máximo 500 caracteres.");
                        }

                        if (!dados.Servidores.Any())
                        {
                            mensagens.Add("É necessário adicionar pelo menos 1 servidor.");
                        }
                    }

                    if ((dados.DataPagamento != null || dados.DataPagamento != DateTime.MinValue) && (dados.DataNotaFiscal != null || dados.DataNotaFiscal != DateTime.MinValue))
                    {
                        DateTime dataPagamento, dataNF;
                        if ((DateTime.TryParse(dados.DataPagamento.ToString(), out dataPagamento)) && (DateTime.TryParse(dados.DataNotaFiscal.ToString(), out dataNF)))
                        //A data de pagamento da NF deverá ser maior ou igual, que a data informada da NF Fiscal
                        {
                            if (dados.DataPagamento < dados.DataNotaFiscal)
                            {
                                mensagens.Add("A data de pagamento da NF deverá ser maior ou igual, que a data informada da Nota Fiscal.");
                            }
                        }
                    }
                }
            }

            if (dados.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Somente nos eventos em que o plano, cuja a finalidade for merenda (FINALIDADEID = 2) serão obrigatório ocorrer a importação do XML.
                    if (
                        dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum &&
                        rnPlanoTrabalho.EhPlanoTrabalhoMerenda(contexto, dados.PlanoTrabalhoId) &&
                        itensNotaFiscal.Count == 0
                    )
                    {
                        //Verifica se está configurado para ignorar algumas regras de despesas e analise
                        var retiradaValidacaoDespesa = Convert.ToBoolean(ConfigurationManager.AppSettings["RetiradaValidacaoDespesa"] ?? "false");

                        if (retiradaValidacaoDespesa)
                        {
                            //Caso a validação esteja retirada e nao tiver xml, criar um linha generica para a nota fiscal
                            DadosXmlItem item = new DadosXmlItem();
                            item.xProd = "ITEM DE REPRESENTAÇÃO DA NF";//DESCRICAO                                        
                            item.qCom = 1;//QUANTIDADE                                        
                            item.vUnCom = Convert.ToDecimal(dados.ValorNotaFiscal);//VALORUNITARIO,
                            itensNotaFiscal.Add(item);
                        }
                        else
                        {

                            mensagens.Add("Campo ARQUIVO XML é obrigatório para o Projeto / Programa de Merenda.");
                        }
                    }

                    //RETIRADA A REGRA POR SOLICITAÇÃO DO GT NO CHAMADO 25472
                    //Verficia se FORNECEDOR está FINALIZADO 
                    //if (dados.FornecedorId != null && dados.FornecedorId > 0 &&
                    //    !rnFornecedor.EhFornecedorFinalizado(contexto, Convert.ToInt32(dados.FornecedorId)))
                    //{
                    //    mensagens.Add("O Fornecedor não pode ser utilizado pois não está aprovado.");
                    //}

                    //O sistema não deve permitir duplicidade de chaves de Notas Fiscais
                    if (!dados.ChaveAcesso.IsNullOrEmptyOrWhiteSpace() && this.PossuiOutraChaveAcessoPor(contexto, dados.ChaveAcesso, dados.EventoId))
                    {
                        mensagens.Add("Esta CHAVE DE ACESSO já foi utilizada por outra despesa.");
                    }
                    else
                    {
                        //Validação retirada a pedido do chamado 26722, pq MEI nao tem cnpj na nota, Ver outra solução marcando MEI no fornecedor pra nao validar apenas no MEI
                        ////CNPJ – CNPJ do emitente;                        
                        //if (!cnpjChave.IsNullOrEmptyOrWhiteSpace() && dados.FornecedorId != null && dados.FornecedorId > 0)
                        //{
                        //    //Busca cnpj do fornecedor
                        //    string cnpjFornecedor = rnFornecedor.ObtemCNPJPor(contexto, Convert.ToInt32(dados.FornecedorId));

                        //    //Verifica se eh o mesmo cnpj
                        //    if (!cnpjFornecedor.IsNullOrEmptyOrWhiteSpace() && cnpjFornecedor != cnpjChave)
                        //    {
                        //        mensagens.Add("O CNPJ da CHAVE DE ACESSO não confere com o CNPJ do fornecedor.");
                        //    }
                        //}
                    }

                    //Verifica se o evento já foi enviado para analise
                    if (this.PossuiNumeroEventoPor(contexto, dados.EventoId))
                    {
                        mensagens.Add("Esta despesa não pode ser editado pois já foi enviado para análise.");
                    }

                    //Verifica se p tipo de despesa precisa de marcação: Pequena despesa Pequena Despesa - 2, Despesa com Locomoção de Servidores - 3, Pequena Despesa sem comprovacao - 4
                    if (dados.TipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum  //0 
                        && dados.TipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais //1
                        && !rnPlanoTrabalho.PermitePequenaDespesa(contexto, dados.PlanoTrabalhoId))
                    {
                        mensagens.Add("A despesa não pode do tipo DEMAIS DESPESAS pois o Projeto / Programa selecionado não permite Pequenas Despesas.");
                    }

                    //Atualiza com dtos de itens da nota
                    arquivoXml.itensXml = itensNotaFiscal;
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

        public ValidacaoDados ValidaDatas(int eventoId, int tipoDespesa, DateTime? dataNotaFiscal, DateTime dataPagamento, int periodoPrestacaoId, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (eventoId <= 0)
            {
                mensagens.Add("Campo DESPESA é obrigatório para alteraçao.");
            }

            if (periodoPrestacaoId <= 0)
            {
                mensagens.Add("Campo PERÍODO é obrigatório para alteraçao.");
            }

            //Data do limite 
            if (dataPagamento == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA PAGAMENTO é obrigatório.");
            }

            //Data do pagamento é obrigatorio para todos
            if (dataPagamento == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA PAGAMENTO é obrigatório.");
            }
            else
            {
                DateTime dataPag;
                if (!DateTime.TryParse(dataPagamento.ToString(), out dataPag))
                {
                    mensagens.Add("Campo DATA PAGAMENTO inválida.");
                }
            }

            if (tipoDespesa < 0)
            {
                mensagens.Add("Campo TIPO DA DESPESA é obrigatório.");
            }
            else
            {
                if (tipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum
                    && tipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais
                    && tipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao
                    && tipoDespesa != (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores)
                {
                    mensagens.Add("Campo TIPO DO DESPESA é inválido.");
                }
                else
                {
                    //Verifica tipo se é evento simples ou com documentos fiscais (data NF obrigatoria)
                    if (tipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum
                        || tipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                    {
                        if (dataNotaFiscal == null || dataNotaFiscal == DateTime.MinValue)
                        {
                            mensagens.Add("Campo DATA DA NOTA FISCAL é obrigatório.");
                        }
                        else
                        {
                            DateTime dataNF;
                            if (!DateTime.TryParse(dataNotaFiscal.ToString(), out dataNF))
                            {
                                mensagens.Add("Campo DATA DA NOTA FISCAL inválida.");
                            }
                        }
                    }

                    if (dataPagamento != null && (dataNotaFiscal != null || dataNotaFiscal != DateTime.MinValue))
                    {
                        DateTime dataPag, dataNF;
                        if ((DateTime.TryParse(dataPagamento.ToString(), out dataPag)) && (DateTime.TryParse(dataNotaFiscal.ToString(), out dataNF)))
                        //A data de pagamento da NF deverá ser maior ou igual, que a data informada da NF Fiscal
                        {
                            if (dataPagamento < dataNotaFiscal)
                            {
                                mensagens.Add("A data de pagamento da NF deverá ser maior ou igual, que a data informada da Nota Fiscal.");
                            }
                        }
                    }
                }
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verificar se é com exigencia aberta de despesa em analise                   
                    if (!rnExigenciaEvento.PossuiEventoExigenciaAbertaPor(contexto, eventoId))
                    {
                        mensagens.Add("Esta Despesa não pode ser editada pois não possui exigências em aberto.");
                    }
                    else if (!this.VerificaPeriodoPagamento(contexto, periodoPrestacaoId.ToString(), dataPagamento.ToString()))
                    {
                        mensagens.Add("A DATA DE PAGAMENTO está fora do Período da despesa.");
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

        public void Insere(DTOs.DadosEvento dados, DadosArquivoXml arquivoXml)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Entidades.OrcamentoArquivo orcamentoArquivo;
            Entidades.EvidenciaArquivo evidenciaArquivo;
            OrcamentoArquivo rnOrcamentoArquivo = new OrcamentoArquivo();
            EvidenciaArquivo rnEvidenciaArquivo = new EvidenciaArquivo();
            EventoNotaFiscalArquivo rnEventoNotaFiscalArquivo = new EventoNotaFiscalArquivo();
            Entidades.EventoNotaFiscalArquivo notaFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.EventoNotaFiscalArquivo();
            ComprovantePagamentoArquivo rnComprovantePagamentoArquivo = new ComprovantePagamentoArquivo();
            Entidades.ComprovantePagamentoArquivo comprovantePagamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ComprovantePagamentoArquivo();
            PequenaDespesa rnPequenaDespesa = new PequenaDespesa();
            Entidades.ImportacaoXmlEvento importacaoXmlEvento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ImportacaoXmlEvento();
            ImportacaoXmlEvento rnImportacaoXmlEvento = new ImportacaoXmlEvento();
            PequenaDespesaServidor rnPequenaDespesaServidor = new PequenaDespesaServidor();
            Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor dadosServidor = new DadosPequenaDespesaServidor();
            ProdutoServico rnProdutoServico = new ProdutoServico();

            try
            {
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum || dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                {
                    //Insere Evento
                    this.InsereEventoSimples(contexto, dados);

                    //Monta orçamento 1
                    orcamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OrcamentoArquivo();
                    orcamentoArquivo.EventoId = dados.EventoId;
                    orcamentoArquivo.Arquivo = dados.Orcamento1Arquivo;
                    orcamentoArquivo.NomeArquivo = dados.Orcamento1NomeArquivo;
                    orcamentoArquivo.TipoArquivo = dados.Orcamento1TipoArquivo;
                    orcamentoArquivo.UsuarioId = dados.UsuarioId;

                    //Insere Orcamento 1
                    rnOrcamentoArquivo.Insere(contexto, orcamentoArquivo);
                    rnOrcamentoArquivo.InsereAuditoria(contexto, orcamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);

                    if (dados.Orcamento2Arquivo != null && dados.Orcamento2Arquivo.Count() > 0)
                    {
                        //Monta orçamento 2
                        orcamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OrcamentoArquivo();
                        orcamentoArquivo.EventoId = dados.EventoId;
                        orcamentoArquivo.Arquivo = dados.Orcamento2Arquivo;
                        orcamentoArquivo.NomeArquivo = dados.Orcamento2NomeArquivo;
                        orcamentoArquivo.TipoArquivo = dados.Orcamento2TipoArquivo;
                        orcamentoArquivo.UsuarioId = dados.UsuarioId;

                        //Insere Orcamento 2
                        rnOrcamentoArquivo.Insere(contexto, orcamentoArquivo);
                        rnOrcamentoArquivo.InsereAuditoria(contexto, orcamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                    }

                    if (dados.Orcamento3Arquivo != null && dados.Orcamento3Arquivo.Count() > 0)
                    {
                        //Monta orçamento 3
                        orcamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OrcamentoArquivo();
                        orcamentoArquivo.EventoId = dados.EventoId;
                        orcamentoArquivo.Arquivo = dados.Orcamento3Arquivo;
                        orcamentoArquivo.NomeArquivo = dados.Orcamento3NomeArquivo;
                        orcamentoArquivo.TipoArquivo = dados.Orcamento3TipoArquivo;
                        orcamentoArquivo.UsuarioId = dados.UsuarioId;

                        //Insere Orcamento 3
                        rnOrcamentoArquivo.Insere(contexto, orcamentoArquivo);
                        rnOrcamentoArquivo.InsereAuditoria(contexto, orcamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                    }

                    //Monta Nota Fiscal
                    notaFiscalArquivo.EventoId = dados.EventoId;
                    notaFiscalArquivo.Arquivo = dados.NotaFiscalArquivo;
                    notaFiscalArquivo.NomeArquivo = dados.NotaFiscalNomeArquivo;
                    notaFiscalArquivo.TipoArquivo = dados.NotaFiscalTipoArquivo;
                    notaFiscalArquivo.UsuarioId = dados.UsuarioId;

                    //Insere Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.Insere(contexto, notaFiscalArquivo);

                    //Auditoria Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.InsereAuditoria(contexto, notaFiscalArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);

                    //Monta Comprovante Pagamento
                    comprovantePagamentoArquivo.EventoId = dados.EventoId;
                    comprovantePagamentoArquivo.Arquivo = dados.ComprovantePagamentoArquivo;
                    comprovantePagamentoArquivo.NomeArquivo = dados.ComprovantePagamentoNomeArquivo;
                    comprovantePagamentoArquivo.TipoArquivo = dados.ComprovantePagamentoTipoArquivo;
                    comprovantePagamentoArquivo.UsuarioId = dados.UsuarioId;

                    //Insere Comprovante Pagamento
                    rnComprovantePagamentoArquivo.Insere(contexto, comprovantePagamentoArquivo);

                    //Auditoria Comprovante Pagamento
                    rnComprovantePagamentoArquivo.InsereAuditoria(contexto, comprovantePagamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);

                    if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum || dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                    {
                        //Insere itens do XML
                        foreach (DadosXmlItem item in arquivoXml.itensXml)
                        {
                            //Monta IMPORTACAOXMLEVENTO
                            importacaoXmlEvento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ImportacaoXmlEvento();
                            importacaoXmlEvento.EventoId = dados.EventoId;
                            importacaoXmlEvento.NumeroItem = item.cEAN;
                            importacaoXmlEvento.Descricao = item.xProd;
                            importacaoXmlEvento.Ncm = item.NCM;
                            importacaoXmlEvento.Quantidade = Convert.ToInt32(item.qCom);
                            importacaoXmlEvento.ValorUnitario = item.vUnCom;
                            importacaoXmlEvento.UsuarioId = dados.UsuarioId;
                            importacaoXmlEvento.ProdutoServicoId = rnProdutoServico.ObtemProdutoServicoIdPor(contexto, item.NCM);

                            //Insere IMPORTACAOXMLEVENTO
                            rnImportacaoXmlEvento.Insere(contexto, importacaoXmlEvento);
                        }
                    }

                    if (dados.EvidenciaArquivo != null && dados.EvidenciaArquivo.Count() > 0)
                    {
                        //Monta evidencia
                        evidenciaArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.EvidenciaArquivo();
                        evidenciaArquivo.EventoId = dados.EventoId;
                        evidenciaArquivo.Arquivo = dados.EvidenciaArquivo;
                        evidenciaArquivo.NomeArquivo = dados.EvidenciaNomeArquivo;
                        evidenciaArquivo.TipoArquivo = dados.EvidenciaTipoArquivo;
                        evidenciaArquivo.UsuarioId = dados.UsuarioId;

                        //Insere Orcamento 3
                        rnEvidenciaArquivo.Insere(contexto, evidenciaArquivo);
                        rnEvidenciaArquivo.InsereAuditoria(contexto, evidenciaArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                    }
                }

                //Verifica tipo se é pequenas despesas com comprovacao
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao)
                {
                    //Insere Evento
                    this.InsereEventoPequenasDespesasComComprovacao(contexto, dados);

                    //Insere pequenas Despesas
                    rnPequenaDespesa.InsereEventoPequenasDespesasComComprovacao(contexto, dados);

                    //Monta Nota Fiscal
                    notaFiscalArquivo.EventoId = dados.EventoId;
                    notaFiscalArquivo.Arquivo = dados.NotaFiscalArquivo;
                    notaFiscalArquivo.NomeArquivo = dados.NotaFiscalNomeArquivo;
                    notaFiscalArquivo.TipoArquivo = dados.NotaFiscalTipoArquivo;
                    notaFiscalArquivo.UsuarioId = dados.UsuarioId;

                    //Insere Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.Insere(contexto, notaFiscalArquivo);

                    //Auditoria Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.InsereAuditoria(contexto, notaFiscalArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);

                    if (!dados.ComprovantePagamentoNomeArquivo.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Monta Comprovante Pagamento
                        comprovantePagamentoArquivo.EventoId = dados.EventoId;
                        comprovantePagamentoArquivo.Arquivo = dados.ComprovantePagamentoArquivo;
                        comprovantePagamentoArquivo.NomeArquivo = dados.ComprovantePagamentoNomeArquivo;
                        comprovantePagamentoArquivo.TipoArquivo = dados.ComprovantePagamentoTipoArquivo;
                        comprovantePagamentoArquivo.UsuarioId = dados.UsuarioId;

                        //Insere Arquivo Comprovante Pagamento
                        rnComprovantePagamentoArquivo.Insere(contexto, comprovantePagamentoArquivo);

                        //Auditoria Arquivo Comprovante Pagamento
                        rnComprovantePagamentoArquivo.InsereAuditoria(contexto, comprovantePagamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                    }
                }

                //Verifica tipo se é pequenas despesas sem comprovacao
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao)
                {
                    //Insere Evento
                    this.InsereEventoPequenasDespesasSemComprovacao(contexto, dados);

                    //Insere pequenas Despesas
                    rnPequenaDespesa.InsereEventoPequenasDespesasSemComprovacao(contexto, dados);
                }

                //Verifica tipo se é transporte
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores)
                {
                    //Insere Evento
                    this.InsereEventoTransporte(contexto, dados);

                    //Insere pequenas Despesas
                    rnPequenaDespesa.InsereEventoTransporte(contexto, dados);

                    if (dados.Servidores.Count > 0)
                    {

                        foreach (var item in dados.Servidores)
                        {
                            item.PequenaDespesaId = dados.PequenaDespesaId;
                        }

                        rnPequenaDespesaServidor.Salva(contexto, dados.Servidores);
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

        public void Atualiza(DTOs.DadosEvento dados, DadosArquivoXml arquivoXml)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            Entidades.OrcamentoArquivo orcamentoArquivo;
            OrcamentoArquivo rnOrcamentoArquivo = new OrcamentoArquivo();
            EventoNotaFiscalArquivo rnEventoNotaFiscalArquivo = new EventoNotaFiscalArquivo();
            Entidades.EventoNotaFiscalArquivo notaFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.EventoNotaFiscalArquivo();
            ComprovantePagamentoArquivo rnComprovantePagamentoArquivo = new ComprovantePagamentoArquivo();
            Entidades.ComprovantePagamentoArquivo comprovantePagamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ComprovantePagamentoArquivo();
            PequenaDespesa rnPequenaDespesa = new PequenaDespesa();
            Entidades.ImportacaoXmlEvento importacaoXmlEvento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ImportacaoXmlEvento();
            ImportacaoXmlEvento rnImportacaoXmlEvento = new ImportacaoXmlEvento();
            PequenaDespesaServidor rnPequenaDespesaServidor = new PequenaDespesaServidor();
            Entidades.EvidenciaArquivo evidenciaArquivo;
            EvidenciaArquivo rnEvidenciaArquivo = new EvidenciaArquivo();

            try
            {
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum || dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais)
                {
                    //Atualiza Evento
                    this.AtualizaEventoSimples(contexto, dados);

                    //Insere auditoria orcamentos anteriores
                    rnOrcamentoArquivo.InsereAuditoria(contexto, dados.EventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);
                    rnEvidenciaArquivo.InsereAuditoria(contexto, dados.EventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                    //Remove os orcamentos anteriores
                    rnOrcamentoArquivo.Remove(contexto, dados.EventoId);
                    rnEvidenciaArquivo.Remove(contexto, dados.EventoId);

                    //Insere novos orçamentos
                    //orçamento 1
                    orcamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OrcamentoArquivo();
                    orcamentoArquivo.EventoId = dados.EventoId;
                    orcamentoArquivo.Arquivo = dados.Orcamento1Arquivo;
                    orcamentoArquivo.NomeArquivo = dados.Orcamento1NomeArquivo;
                    orcamentoArquivo.TipoArquivo = dados.Orcamento1TipoArquivo;
                    orcamentoArquivo.UsuarioId = dados.UsuarioId;

                    //Orcamento 1
                    rnOrcamentoArquivo.Insere(contexto, orcamentoArquivo);
                    rnOrcamentoArquivo.InsereAuditoria(contexto, orcamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);

                    if (dados.Orcamento2Arquivo != null && dados.Orcamento2Arquivo.Count() > 0)
                    {
                        //Monta orçamento 2
                        orcamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OrcamentoArquivo();
                        orcamentoArquivo.EventoId = dados.EventoId;
                        orcamentoArquivo.Arquivo = dados.Orcamento2Arquivo;
                        orcamentoArquivo.NomeArquivo = dados.Orcamento2NomeArquivo;
                        orcamentoArquivo.TipoArquivo = dados.Orcamento2TipoArquivo;
                        orcamentoArquivo.UsuarioId = dados.UsuarioId;

                        //Orcamento 2
                        rnOrcamentoArquivo.Insere(contexto, orcamentoArquivo);
                        rnOrcamentoArquivo.InsereAuditoria(contexto, orcamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                    }

                    if (dados.Orcamento3Arquivo != null && dados.Orcamento3Arquivo.Count() > 0)
                    {
                        //Monta orçamento 3
                        orcamentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OrcamentoArquivo();
                        orcamentoArquivo.EventoId = dados.EventoId;
                        orcamentoArquivo.Arquivo = dados.Orcamento3Arquivo;
                        orcamentoArquivo.NomeArquivo = dados.Orcamento3NomeArquivo;
                        orcamentoArquivo.TipoArquivo = dados.Orcamento3TipoArquivo;
                        orcamentoArquivo.UsuarioId = dados.UsuarioId;

                        //Orcamento 3
                        rnOrcamentoArquivo.Insere(contexto, orcamentoArquivo);
                        rnOrcamentoArquivo.InsereAuditoria(contexto, orcamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                    }

                    //Monta Nota Fiscal
                    notaFiscalArquivo.EventoId = dados.EventoId;
                    notaFiscalArquivo.Arquivo = dados.NotaFiscalArquivo;
                    notaFiscalArquivo.NomeArquivo = dados.NotaFiscalNomeArquivo;
                    notaFiscalArquivo.TipoArquivo = dados.NotaFiscalTipoArquivo;
                    notaFiscalArquivo.UsuarioId = dados.UsuarioId;

                    //Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.Atualiza(contexto, notaFiscalArquivo);

                    //Auditoria Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.InsereAuditoria(contexto, notaFiscalArquivo, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);

                    //Monta Comprovante Pagamento
                    comprovantePagamentoArquivo.EventoId = dados.EventoId;
                    comprovantePagamentoArquivo.Arquivo = dados.ComprovantePagamentoArquivo;
                    comprovantePagamentoArquivo.NomeArquivo = dados.ComprovantePagamentoNomeArquivo;
                    comprovantePagamentoArquivo.TipoArquivo = dados.ComprovantePagamentoTipoArquivo;
                    comprovantePagamentoArquivo.UsuarioId = dados.UsuarioId;

                    //Comprovante Pagamento
                    rnComprovantePagamentoArquivo.Atualiza(contexto, comprovantePagamentoArquivo);

                    //Auditoria Comprovante Pagamento
                    rnComprovantePagamentoArquivo.InsereAuditoria(contexto, comprovantePagamentoArquivo, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);

                    if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum)
                    {

                        if (arquivoXml != null && arquivoXml.itensXml.Any())
                        {
                            //Deletar itens do XML anterior
                            rnImportacaoXmlEvento.RemoveTodos(contexto, dados.EventoId);

                            //Insere itens do XML
                            foreach (DadosXmlItem item in arquivoXml.itensXml)
                            {
                                //Monta IMPORTACAOXMLEVENTO
                                importacaoXmlEvento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ImportacaoXmlEvento();
                                importacaoXmlEvento.EventoId = dados.EventoId;
                                importacaoXmlEvento.NumeroItem = item.cEAN;
                                importacaoXmlEvento.Descricao = item.xProd;
                                importacaoXmlEvento.Ncm = item.NCM;
                                importacaoXmlEvento.Quantidade = Convert.ToInt32(item.qCom);
                                importacaoXmlEvento.ValorUnitario = item.vUnCom;
                                importacaoXmlEvento.UsuarioId = dados.UsuarioId;

                                //Insere IMPORTACAOXMLEVENTO
                                rnImportacaoXmlEvento.Insere(contexto, importacaoXmlEvento);
                            }
                        }
                    }

                    if (dados.EvidenciaArquivo != null && dados.EvidenciaArquivo.Count() > 0)
                    {
                        //Monta evidencia
                        evidenciaArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.EvidenciaArquivo();
                        evidenciaArquivo.EventoId = dados.EventoId;
                        evidenciaArquivo.Arquivo = dados.EvidenciaArquivo;
                        evidenciaArquivo.NomeArquivo = dados.EvidenciaNomeArquivo;
                        evidenciaArquivo.TipoArquivo = dados.EvidenciaTipoArquivo;
                        evidenciaArquivo.UsuarioId = dados.UsuarioId;

                        //Insere evidencia
                        rnEvidenciaArquivo.Insere(contexto, evidenciaArquivo);
                        rnEvidenciaArquivo.InsereAuditoria(contexto, evidenciaArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);
                    }
                }

                //Verifica tipo se é pequenas despesas com comprovacao
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao)
                {
                    //Evento
                    this.AtualizaEventoPequenasDespesasComComprovacao(contexto, dados);

                    //pequenas Despesas
                    rnPequenaDespesa.AtualizaEventoPequenasDespesasComComprovacao(contexto, dados);

                    //Monta Nota Fiscal
                    notaFiscalArquivo.EventoId = dados.EventoId;
                    notaFiscalArquivo.Arquivo = dados.NotaFiscalArquivo;
                    notaFiscalArquivo.NomeArquivo = dados.NotaFiscalNomeArquivo;
                    notaFiscalArquivo.TipoArquivo = dados.NotaFiscalTipoArquivo;
                    notaFiscalArquivo.UsuarioId = dados.UsuarioId;

                    //Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.Atualiza(contexto, notaFiscalArquivo);

                    //Auditoria Arquivo Nota Fiscal
                    rnEventoNotaFiscalArquivo.InsereAuditoria(contexto, notaFiscalArquivo, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);

                    if (!dados.ComprovantePagamentoNomeArquivo.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Monta Comprovante Pagamento
                        comprovantePagamentoArquivo.EventoId = dados.EventoId;
                        comprovantePagamentoArquivo.Arquivo = dados.ComprovantePagamentoArquivo;
                        comprovantePagamentoArquivo.NomeArquivo = dados.ComprovantePagamentoNomeArquivo;
                        comprovantePagamentoArquivo.TipoArquivo = dados.ComprovantePagamentoTipoArquivo;
                        comprovantePagamentoArquivo.UsuarioId = dados.UsuarioId;

                        if (rnComprovantePagamentoArquivo.ExisteComprovanteArquivoPor(contexto, dados.EventoId))
                        {
                            //Comprovante Pagamento
                            rnComprovantePagamentoArquivo.Atualiza(contexto, comprovantePagamentoArquivo);
                            //Auditoria Comprovante Pagamento
                            rnComprovantePagamentoArquivo.InsereAuditoria(contexto, comprovantePagamentoArquivo, "ALTERADO", System.Web.HttpContext.Current.Request.UserHostName);

                        }
                        else
                        {
                            rnComprovantePagamentoArquivo.Insere(contexto, comprovantePagamentoArquivo);
                            //Auditoria Comprovante Pagamento
                            rnComprovantePagamentoArquivo.InsereAuditoria(contexto, comprovantePagamentoArquivo, "CADASTRADO", System.Web.HttpContext.Current.Request.UserHostName);

                        }

                    }
                }

                //Verifica tipo se é pequenas despesas sem comprovacao
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao)
                {
                    //Evento
                    this.AtualizaEventoPequenasDespesasSemComprovacao(contexto, dados);

                    //pequenas Despesas
                    rnPequenaDespesa.AtualizaEventoPequenasDespesasSemComprovacao(contexto, dados);
                }

                //Verifica tipo se é transporte
                if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores)
                {
                    //Evento
                    this.AtualizaEventoTransporte(contexto, dados);

                    //pequenas Despesas
                    rnPequenaDespesa.AtualizaEventoTransporte(contexto, dados);

                    //servidores
                    rnPequenaDespesaServidor.Salva(contexto, dados.Servidores);
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

        private void InsereEventoSimples(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.EVENTO
                                                   (DESCRICAO
                                                   ,PLANOTRABALHOID
                                                   ,FORNECEDORID
                                                   ,CENSO
                                                   ,JUSTIFICATIVAORCAMENTO
                                                   ,CHAVEACESSO
                                                   ,NUMERONOTAFISCAL
                                                   ,VALORNOTAFISCAL
                                                   ,DATANOTAFISCAL
                                                   ,OBSERVACOES
                                                   ,EVIDENCIAS
                                                   ,DATAPAGAMENTO
                                                   ,VALORPAGAMENTO
                                                   ,TIPODESPESA
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO)
                                             VALUES
                                                   (@DESCRICAO,
                                                   @PLANOTRABALHOID, 
                                                   @FORNECEDORID, 
                                                   @CENSO, 
                                                   @JUSTIFICATIVAORCAMENTO, 
                                                   @CHAVEACESSO, 
                                                   @NUMERONOTAFISCAL, 
                                                   @VALORNOTAFISCAL,
                                                   @DATANOTAFISCAL, 
                                                   @OBSERVACOES, 
                                                   @EVIDENCIAS, 
                                                   @DATAPAGAMENTO, 
                                                   @VALORPAGAMENTO, 
                                                   @TIPODESPESA,
                                                   @USUARIOID, 
                                                   @DATACADASTRO, 
                                                   @DATAALTERACAO)

                        select IDENT_CURRENT('PrestacaoContas.EVENTO')
            ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, dados.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, dados.FornecedorId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dados.Censo);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@JUSTIFICATIVAORCAMENTO", SqlDbType.VarChar, dados.JustificativaOrcamento);
            contextQuery.Parameters.Add("@CHAVEACESSO", SqlDbType.VarChar, dados.ChaveAcesso);
            contextQuery.Parameters.Add("@NUMERONOTAFISCAL", SqlDbType.VarChar, dados.NumeroNotaFiscal);
            contextQuery.Parameters.Add("@VALORNOTAFISCAL", SqlDbType.Decimal, dados.ValorNotaFiscal);
            contextQuery.Parameters.Add("@DATANOTAFISCAL", SqlDbType.DateTime, dados.DataNotaFiscal);
            contextQuery.Parameters.Add("@OBSERVACOES", SqlDbType.VarChar, dados.Observacoes);
            contextQuery.Parameters.Add("@EVIDENCIAS", SqlDbType.VarChar, dados.Evidencias);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.Int, dados.TipoDespesa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dados.EventoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void AtualizaEventoSimples(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE PrestacaoContas.EVENTO
	                                    SET FORNECEDORID = @FORNECEDORID,
                                            DESCRICAO = @DESCRICAO,
		                                    JUSTIFICATIVAORCAMENTO = @JUSTIFICATIVAORCAMENTO, 
                                            CHAVEACESSO = @CHAVEACESSO,
                                            NUMERONOTAFISCAL = @NUMERONOTAFISCAL, 
                                            VALORNOTAFISCAL = @VALORNOTAFISCAL,
                                            DATANOTAFISCAL = @DATANOTAFISCAL, 
                                            OBSERVACOES = @OBSERVACOES, 
                                            EVIDENCIAS = @EVIDENCIAS, 
                                            DATAPAGAMENTO = @DATAPAGAMENTO, 
                                            VALORPAGAMENTO = @VALORPAGAMENTO, 
                                            USUARIOID = @USUARIOID, 
                                            DATAALTERACAO = @DATAALTERACAO
                                    WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, dados.FornecedorId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@JUSTIFICATIVAORCAMENTO", SqlDbType.VarChar, dados.JustificativaOrcamento);
            contextQuery.Parameters.Add("@CHAVEACESSO", SqlDbType.VarChar, dados.ChaveAcesso);
            contextQuery.Parameters.Add("@NUMERONOTAFISCAL", SqlDbType.VarChar, dados.NumeroNotaFiscal);
            contextQuery.Parameters.Add("@VALORNOTAFISCAL", SqlDbType.Decimal, dados.ValorNotaFiscal);
            contextQuery.Parameters.Add("@DATANOTAFISCAL", SqlDbType.DateTime, dados.DataNotaFiscal);
            contextQuery.Parameters.Add("@OBSERVACOES", SqlDbType.VarChar, dados.Observacoes);
            contextQuery.Parameters.Add("@EVIDENCIAS", SqlDbType.VarChar, dados.Evidencias);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereEventoPequenasDespesasComComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.EVENTO
                                                   (PLANOTRABALHOID
                                                   ,FORNECEDORID
                                                   ,CENSO
                                                   ,DESCRICAO
                                                   ,NUMERONOTAFISCAL
                                                   ,DATANOTAFISCAL
                                                   ,VALORNOTAFISCAL
                                                   ,DATAPAGAMENTO
                                                   ,VALORPAGAMENTO
                                                   ,TIPODESPESA
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO)
                                             VALUES
                                                   (@PLANOTRABALHOID, 
                                                   @FORNECEDORID, 
                                                   @CENSO, 
                                                   @DESCRICAO,
                                                   @NUMERONOTAFISCAL, 
                                                   @DATANOTAFISCAL,
                                                   @VALORNOTAFISCAL, 
                                                   @DATAPAGAMENTO, 
                                                   @VALORPAGAMENTO,
                                                   @TIPODESPESA,
                                                   @USUARIOID, 
                                                   @DATACADASTRO, 
                                                   @DATAALTERACAO)

                         SELECT IDENT_CURRENT('PrestacaoContas.EVENTO') ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, dados.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, dados.FornecedorId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dados.Censo);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@NUMERONOTAFISCAL", SqlDbType.VarChar, dados.NumeroNotaFiscal);
            contextQuery.Parameters.Add("@VALORNOTAFISCAL", SqlDbType.Decimal, dados.ValorNotaFiscal);
            contextQuery.Parameters.Add("@DATANOTAFISCAL", SqlDbType.DateTime, dados.DataNotaFiscal);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.Int, (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dados.EventoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void InsereEventoPequenasDespesasSemComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.EVENTO
                                                   (PLANOTRABALHOID
                                                   ,FORNECEDORID
                                                   ,CENSO
                                                   ,DESCRICAO
                                                   ,NUMERONOTAFISCAL
                                                   ,VALORNOTAFISCAL
                                                   ,DATAPAGAMENTO
                                                   ,VALORPAGAMENTO
                                                   ,TIPODESPESA
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO)
                                             VALUES
                                                   (@PLANOTRABALHOID, 
                                                   @FORNECEDORID, 
                                                   @CENSO, 
                                                   @DESCRICAO,
                                                   @NUMERONOTAFISCAL, 
                                                   @VALORNOTAFISCAL, 
                                                   @DATAPAGAMENTO, 
                                                   @VALORPAGAMENTO,
                                                   @TIPODESPESA,
                                                   @USUARIOID, 
                                                   @DATACADASTRO, 
                                                   @DATAALTERACAO)

                         SELECT IDENT_CURRENT('PrestacaoContas.EVENTO') ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, dados.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, dados.FornecedorId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dados.Censo);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@NUMERONOTAFISCAL", SqlDbType.VarChar, dados.NumeroNotaFiscal);
            contextQuery.Parameters.Add("@VALORNOTAFISCAL", SqlDbType.Decimal, dados.ValorNotaFiscal);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.Int, (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dados.EventoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void AtualizaEventoPequenasDespesasComComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTO
                                    SET FORNECEDORID = @FORNECEDORID,
                                        DESCRICAO = @DESCRICAO, 
	                                    NUMERONOTAFISCAL = @NUMERONOTAFISCAL, 
                                        DATANOTAFISCAL = @DATANOTAFISCAL,
	                                    VALORNOTAFISCAL = @VALORNOTAFISCAL, 
	                                    DATAPAGAMENTO = @DATAPAGAMENTO, 
	                                    VALORPAGAMENTO = @VALORPAGAMENTO,
	                                    USUARIOID = @USUARIOID, 
	                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, dados.FornecedorId);
            contextQuery.Parameters.Add("@NUMERONOTAFISCAL", SqlDbType.VarChar, dados.NumeroNotaFiscal);
            contextQuery.Parameters.Add("@DATANOTAFISCAL", SqlDbType.DateTime, dados.DataNotaFiscal);
            contextQuery.Parameters.Add("@VALORNOTAFISCAL", SqlDbType.Decimal, dados.ValorNotaFiscal);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void AtualizaEventoPequenasDespesasSemComprovacao(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTO
                                    SET FORNECEDORID = @FORNECEDORID,
                                        DESCRICAO = @DESCRICAO, 
	                                    DATAPAGAMENTO = @DATAPAGAMENTO, 
	                                    VALORPAGAMENTO = @VALORPAGAMENTO,
	                                    USUARIOID = @USUARIOID, 
	                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@FORNECEDORID", SqlDbType.Int, dados.FornecedorId);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void InsereEventoTransporte(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO PrestacaoContas.EVENTO
                                               (PLANOTRABALHOID
                                               ,CENSO
                                               ,DESCRICAO
                                               ,VALORPAGAMENTO
                                               ,DATAPAGAMENTO
                                               ,TIPODESPESA
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@PLANOTRABALHOID, 
                                               @CENSO, 
                                               @DESCRICAO,
                                               @VALORPAGAMENTO,
                                               @DATAPAGAMENTO,
                                               @TIPODESPESA,
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO)

                         SELECT IDENT_CURRENT('PrestacaoContas.EVENTO') ";

            contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, dados.PlanoTrabalhoId);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, dados.Censo);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.Int, (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            dados.EventoId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void AtualizaEventoTransporte(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTO
                                        SET VALORPAGAMENTO = @VALORPAGAMENTO,
                                            DESCRICAO = @DESCRICAO,
                                            DATAPAGAMENTO = @DATAPAGAMENTO, 
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE EVENTOID  = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, dados.Descricao);
            contextQuery.Parameters.Add("@VALORPAGAMENTO", SqlDbType.Decimal, dados.ValorPagamento);
            contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dados.DataPagamento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaDatas(int eventoId, DateTime? dataNotaFiscal, DateTime? dataPagamento, string usuarioId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  UPDATE PrestacaoContas.EVENTO
	                                    SET DATANOTAFISCAL = @DATANOTAFISCAL, 
                                            DATAPAGAMENTO = @DATAPAGAMENTO, 
                                            USUARIOID = @USUARIOID, 
                                            DATAALTERACAO = @DATAALTERACAO
                                    WHERE EVENTOID = @EVENTOID ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
                contextQuery.Parameters.Add("@DATAPAGAMENTO", SqlDbType.DateTime, dataPagamento);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                if (dataNotaFiscal == null || dataNotaFiscal == DateTime.MinValue)
                {
                    contextQuery.Parameters.Add("@DATANOTAFISCAL", SqlDbType.DateTime, DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@DATANOTAFISCAL", SqlDbType.DateTime, dataNotaFiscal);
                }

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

        public Entidades.Evento ObtemPor(DataContext contexto, int eventoId)
        {
            Entidades.Evento evento = new PrestacaoContas.Entidades.Evento();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                                      FROM PRESTACAOCONTAS.EVENTO (NOLOCK) 
                                      WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            evento = contexto.TryToBindEntity<Entidades.Evento>(contextQuery);

            return evento;
        }

        public List<DTOs.DadosDespesa> ObtemDadosDespesaPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int finalidadeId)
        {
            List<DTOs.DadosDespesa> dados = new List<DTOs.DadosDespesa>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  E.EVENTOID, 
			                                e.NUMEROEVENTO,
			                                E.FORNECEDORID,
			                                [PrestacaoContas].[fn_ObterRazaoSocialDoFornecedorNaDtPgtoDaDespesa](e.EVENTOID) as NOME_FORNECEDOR,	
			                                E.NUMERONOTAFISCAL,
			                                E.VALORPAGAMENTO, 
			                                E.PLANOTRABALHOID, 
			                                E.DATAPAGAMENTO
	                                from PrestacaoContas.EVENTO e
		                                INNER JOIN PRESTACAOCONTAS.PLANOTRABALHO PT 
				                                ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
	                                WHERE E.APROVADO = 1
		                                AND CENSO = @CENSO
		                                AND PT.FINALIDADEID = @FINALIDADEID
		                                AND DATAPAGAMENTO >= @DATAINICIO 
		                                AND DATAPAGAMENTO <= @DATAFIM 
                                    ORDER BY E.DATAPAGAMENTO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalidadeId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DTOs.DadosDespesa item = new DadosDespesa();

                    item.Evento = Convert.ToString(reader["NUMEROEVENTO"]);
                    item.FornecedorBeneficiario = Convert.ToString(reader["NOME_FORNECEDOR"]);
                    item.DocumentoFiscal = Convert.ToString(reader["NUMERONOTAFISCAL"]);
                    item.Valor = Convert.ToDecimal(reader["VALORPAGAMENTO"]).ToString("c");
                    item.ValorCalculo = Convert.ToDecimal(reader["VALORPAGAMENTO"]);
                    item.DataPagamento = Convert.ToDateTime(reader["DATAPAGAMENTO"]);

                    dados.Add(item);
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

        public List<DTOs.DadosDespesa> ObtemDadosDespesaPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int finalidadeId, string pt)
        {
            List<DTOs.DadosDespesa> dados = new List<DTOs.DadosDespesa>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  E.EVENTOID, 
			                                e.NUMEROEVENTO,
			                                E.FORNECEDORID,
			                                [PrestacaoContas].[fn_ObterRazaoSocialDoFornecedorNaDtPgtoDaDespesa](e.EVENTOID) as NOME_FORNECEDOR,
			                                E.NUMERONOTAFISCAL,
			                                E.VALORPAGAMENTO, 
			                                E.PLANOTRABALHOID, 
			                                E.DATAPAGAMENTO
	                                from PrestacaoContas.EVENTO e
		                                INNER JOIN PRESTACAOCONTAS.PLANOTRABALHO PT 
				                                ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
										inner join PrestacaoContas.PROGRAMATRABALHO po 
												on po.PROGRAMATRABALHOID = PT.PROGRAMATRABALHOID
										inner join PrestacaoContas.WSPROGRAMASEFAZ pws 
												on po.WSPROGRAMASEFAZID = pws.WSPROGRAMASEFAZID
	                                WHERE E.APROVADO = 1
		                                AND CENSO = @CENSO
		                                AND PT.FINALIDADEID = @FINALIDADEID
										AND PWS.PT = @PT
		                                AND DATAPAGAMENTO >= @DATAINICIO 
		                                AND DATAPAGAMENTO <= @DATAFIM 
                                    ORDER BY E.DATAPAGAMENTO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@FINALIDADEID", SqlDbType.Int, finalidadeId);
                contextQuery.Parameters.Add("@PT", SqlDbType.VarChar, pt);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DTOs.DadosDespesa item = new DadosDespesa();

                    item.Evento = Convert.ToString(reader["NUMEROEVENTO"]);
                    item.FornecedorBeneficiario = Convert.ToString(reader["NOME_FORNECEDOR"]);
                    item.DocumentoFiscal = Convert.ToString(reader["NUMERONOTAFISCAL"]);
                    item.Valor = Convert.ToDecimal(reader["VALORPAGAMENTO"]).ToString("c");
                    item.ValorCalculo = Convert.ToDecimal(reader["VALORPAGAMENTO"]);
                    item.DataPagamento = Convert.ToDateTime(reader["DATAPAGAMENTO"]);

                    dados.Add(item);
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

        public List<DTOs.DadosDespesa> ObtemDadosDespesaProgramaPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int programaTrabalhoId)
        {
            List<DTOs.DadosDespesa> dados = new List<DTOs.DadosDespesa>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  E.EVENTOID, 
			                                e.NUMEROEVENTO,
			                                E.FORNECEDORID,
			                                [PrestacaoContas].[fn_ObterRazaoSocialDoFornecedorNaDtPgtoDaDespesa](e.EVENTOID) as NOME_FORNECEDOR,
			                                E.NUMERONOTAFISCAL,
			                                E.VALORPAGAMENTO, 
			                                E.PLANOTRABALHOID, 
			                                E.DATAPAGAMENTO
	                                from PrestacaoContas.EVENTO e
		                                INNER JOIN PRESTACAOCONTAS.PLANOTRABALHO PT 
				                                ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
	                                WHERE E.APROVADO = 1
		                                AND CENSO = @CENSO
		                                AND PT.PROGRAMATRABALHOID = @PROGRAMATRABALHOID
		                                AND DATAPAGAMENTO >= @DATAINICIO 
		                                AND DATAPAGAMENTO <= @DATAFIM 
                                    ORDER BY E.DATAPAGAMENTO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@PROGRAMATRABALHOID", SqlDbType.Int, programaTrabalhoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DTOs.DadosDespesa item = new DadosDespesa();

                    item.Evento = Convert.ToString(reader["NUMEROEVENTO"]);
                    item.FornecedorBeneficiario = Convert.ToString(reader["NOME_FORNECEDOR"]);
                    item.DocumentoFiscal = Convert.ToString(reader["NUMERONOTAFISCAL"]);
                    item.Valor = Convert.ToDecimal(reader["VALORPAGAMENTO"]).ToString("c");
                    item.ValorCalculo = Convert.ToDecimal(reader["VALORPAGAMENTO"]);
                    item.DataPagamento = Convert.ToDateTime(reader["DATAPAGAMENTO"]);

                    dados.Add(item);
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

        public List<DTOs.DadosDespesa> ObtemDadosDespesaProjetoPor(DataContext contexto, string censo, DateTime dataInicio, DateTime dataFim, int planoTrabalhoId)
        {
            List<DTOs.DadosDespesa> dados = new List<DTOs.DadosDespesa>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  E.EVENTOID, 
			                                e.NUMEROEVENTO,
			                                E.FORNECEDORID,
			                                [PrestacaoContas].[fn_ObterRazaoSocialDoFornecedorNaDtPgtoDaDespesa](e.EVENTOID) as NOME_FORNECEDOR,
			                                E.NUMERONOTAFISCAL,
			                                E.VALORPAGAMENTO, 
			                                E.PLANOTRABALHOID, 
			                                E.DATAPAGAMENTO
	                                from PrestacaoContas.EVENTO e
		                                INNER JOIN PRESTACAOCONTAS.PLANOTRABALHO PT 
				                                ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
	                                WHERE E.APROVADO = 1
		                                AND CENSO = @CENSO
		                                AND E.PLANOTRABALHOID = @PLANOTRABALHOID
		                                AND DATAPAGAMENTO >= @DATAINICIO 
		                                AND DATAPAGAMENTO <= @DATAFIM 
                                    ORDER BY E.DATAPAGAMENTO ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DTOs.DadosDespesa item = new DadosDespesa();

                    item.Evento = Convert.ToString(reader["NUMEROEVENTO"]);
                    item.FornecedorBeneficiario = Convert.ToString(reader["NOME_FORNECEDOR"]);
                    item.DocumentoFiscal = Convert.ToString(reader["NUMERONOTAFISCAL"]);
                    item.Valor = Convert.ToDecimal(reader["VALORPAGAMENTO"]).ToString("c");
                    item.ValorCalculo = Convert.ToDecimal(reader["VALORPAGAMENTO"]);
                    item.DataPagamento = Convert.ToDateTime(reader["DATAPAGAMENTO"]);

                    dados.Add(item);
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

        public ValidacaoDados ValidaEnvioAnalise(DTOs.DadosEvento dados)
        {
            PlanoTrabalho rnPlanoTrabalho = new PlanoTrabalho();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Finalidade rnFinalidade = new Finalidade();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dados == null)
            {
                return validacaoDados;
            }

            if (dados.EventoId <= 0)
            {
                mensagens.Add("Campo DESPESA é obrigatório.");
            }

            if (dados.TipoDespesa < 0)
            {
                mensagens.Add("Campo TIPO DA DESPESA é obrigatório.");
            }

            if (dados.PlanoTrabalhoId <= 0)
            {
                mensagens.Add("Campo PROJETO / PROGRAMA é obrigatório.");
            }

            if (dados.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se o evento já foi enviado para analise
                    if (this.PossuiNumeroEventoPor(contexto, dados.EventoId))
                    {
                        mensagens.Add("Esta DESPESA já foi enviado para análise.");
                    }
                    else
                    {
                        string siglaEvento;

                        //Busca finalidade de acordo com planho de trabalho
                        int finalidadeId = rnPlanoTrabalho.ObtemFinalidadePor(contexto, dados.PlanoTrabalhoId);

                        //Caso o evento seja do tipo Pequenas Despesas ou Transporte a sigla do evento será sempre PD (Pequenas despesas), 
                        if (dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao
                            || dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao
                            || dados.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores)
                        {
                            siglaEvento = "PD";
                        }
                        else //caso contrario seja verificada a sigla de finalidade  
                        {
                            //Buca Sigla da Finalidade
                            siglaEvento = rnFinalidade.ObtemSiglaEventoPor(finalidadeId);
                        }

                        if (siglaEvento.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Não foi encontrado uma SIGLA DE EVENTO PARA A FINALIDADE ESPECIFICADA.");
                        }
                        else
                        {
                            //Composição do Numero do Evento (Tabela EVENTO campo NUMEROEVENTO)
                            //ANO Corrente do Sistema + Sigla do Evento + Sequencial (com 10 dígitos, no caso preencher com zero para formar os 10 dígitos)
                            //Ex: Ano corrente é 2021, o Finalidade do Plano selecionado é de merenda  (FINALIDADEID = 2) e o EVENTOID criado é 6.
                            //Logo o numero do evento será: 2021ME0000000006

                            //Monta inicio da sigla completa
                            dados.NumeroEvento = string.Format("{0}{1}{2}", DateTime.Now.Year.ToString(), siglaEvento, dados.EventoId.ToString().PadLeft(10, '0'));
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

        private bool PossuiNumeroEventoPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID
                                          AND NUMEROEVENTO IS NOT NULL ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiXmlGeradoInternamente(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS QTDE
                                            FROM PRESTACAOCONTAS.IMPORTACAOXMLEVENTO
                                            WHERE DESCRICAO = 'ITEM DE REPRESENTAÇÃO DA NF'
	                                            AND EVENTOID = @EVENTOID ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

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

        private bool PossuiOutraChaveAcessoPor(DataContext contexto, string chaveAcesso, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EVENTO (NOLOCK)
                                    WHERE CHAVEACESSO = @CHAVEACESSO
                                          AND EVENTOID <> @EVENTOID
                                          AND (APROVADO IS NULL OR APROVADO = 1) ";

            contextQuery.Parameters.Add("@CHAVEACESSO", SqlDbType.VarChar, chaveAcesso);
            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void EnviaAnalise(DTOs.DadosEvento dados)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza numero evento
                this.AtualizaNumeroEvento(contexto, dados);
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

        private void AtualizaNumeroEvento(DataContext contexto, DTOs.DadosEvento dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTO
                                        SET NUMEROEVENTO = @NUMEROEVENTO,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE EVENTOID  = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, dados.EventoId);
            contextQuery.Parameters.Add("@NUMEROEVENTO", SqlDbType.VarChar, dados.NumeroEvento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaRemocao(int eventoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (eventoId <= 0)
            {
                mensagens.Add("Campo DESPESA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se já possui já foi enviado para analise (tem numero evento)
                    if (this.PossuiNumeroEventoPor(contexto, eventoId))
                    {
                        mensagens.Add("Esta DESPESA não pode ser removida pois já foi enviada para análise.");
                    }

                    //Verifica se existe exigencias
                    if (rnExigenciaEvento.PossuiEventoPor(contexto, eventoId))
                    {
                        mensagens.Add("Esta DESPESA não pode ser removida pois já possui exigências.");
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

        public bool VerificaEnvioSEI(int eventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            contextQuery.Command = @"begin
                                        declare @mesano varchar(6);
                                        declare @censo varchar(10);
                                        declare @idperiodoreferencia int;

                                        --select top(100) *  FROM [LYCEUM].[PrestacaoContas].[EVENTO]

                                        SELECT @mesano =  CONVERT(nvarchar(6), DATAPAGAMENTO, 112) , @censo = CENSO
                                          FROM [LYCEUM].[PrestacaoContas].[EVENTO]
                                        where EVENTOID = @EVENTOID

                                        SELECT  @idperiodoreferencia = [PERIODOREFERENCIAID]     
                                        FROM [LYCEUM].[PrestacaoContas].[PERIODOREFERENCIA]
                                        where iif(mesinicial<10, concat(ano,'0', mesinicial),concat(ano,mesinicial)) <= @mesano and
                                              iif(mesfinal<10, concat(ano,'0', mesfinal),concat(ano,mesfinal)) >= @mesano

                                        SELECT count([IMPORTACAOSEIID]) as existe
                                                                             FROM [LYCEUM].[PrestacaoContas].[IMPORTACAOSEI]
                                                                             where [CENSO] = @censo and
		                                                                            [PERIODOREFERENCIAID] = @idperiodoreferencia
                                    end";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["existe"]) != 0)
                    return true;
                else
                    return false;
            }

            return true;
        }
        public bool VerificaEnvioSEIPorUnidade(string censo, int idperiodoreferencia)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;

            contextQuery.Command = @"SELECT count([IMPORTACAOSEIID]) as existe
                                                                             FROM [LYCEUM].[PrestacaoContas].[IMPORTACAOSEI]
                                                                             where [CENSO] = @CENSO and
		                                                                           [PERIODOREFERENCIAID] = @PERIODOREFERENCIAID
                                        ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@PERIODOREFERENCIAID", SqlDbType.Int, idperiodoreferencia);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                if (Convert.ToInt32(reader["existe"]) != 0)
                    return true;
                else
                    return false;
            }

            return true;
        }
        public ValidacaoDados ValidaRemocaoTotal(int eventoId, string motivoExclusao, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            RN.Perfil rnPerfil = new Perfil();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (eventoId <= 0)
            {
                mensagens.Add("Campo DESPESA é obrigatório.");
            }

            if (motivoExclusao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MOTIVO DA EXCLUSÃO é obrigatório.");
            }
            else if (motivoExclusao.Length > 5000)
            {
                mensagens.Add("O campo MOTIVO DA EXCLUSÃO deve ter no máximo 5000 caracteres.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUÁRIO RESPONSÁVEL é obrigatório.");
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

        public void AnalisaEvento(DataContext contexto, int eventoId, bool aprovado, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTO
                                        SET APROVADO = @APROVADO,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE EVENTOID  = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
            contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, aprovado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void RemoveEvento(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" insert into PrestacaoContas.EVENTO_EXCLUIDO
                                            SELECT *, GETDATE() FROM PrestacaoContas.EVENTO
                                            WHERE EVENTOID = @EVENTOID

                                      DELETE PrestacaoContas.EVENTO
                                            WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(int eventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            PequenaDespesa rnPequenaDespesa = new PequenaDespesa();
            PequenaDespesaServidor rnPequenaDespesaServidor = new PequenaDespesaServidor();
            EventoNotaFiscalArquivo rnEventoNotaFiscalArquivo = new EventoNotaFiscalArquivo();
            ComprovantePagamentoArquivo rnComprovantePagamentoArquivo = new ComprovantePagamentoArquivo();
            OrcamentoArquivo rnOrcamentoArquivo = new OrcamentoArquivo();
            EvidenciaArquivo rnEvidenciaArquivo = new EvidenciaArquivo();
            ImportacaoXmlEvento rnImportacaoXmlEvento = new ImportacaoXmlEvento();

            try
            {
                //Remove todos os servidores associados à pequena despesa
                rnPequenaDespesaServidor.RemoveTodosPorEvento(contexto, eventoId);

                //Remove pequenas despesas
                rnPequenaDespesa.Remove(contexto, eventoId);

                //Auditoria Arquivo Nota Fiscal
                rnEventoNotaFiscalArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove Arquivo Nota fiscal
                rnEventoNotaFiscalArquivo.Remove(contexto, eventoId);

                //Auditoria Arquivo Comprovante pagamento
                rnComprovantePagamentoArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove comprovante pagamento
                rnComprovantePagamentoArquivo.Remove(contexto, eventoId);

                //Insere auditoria orcamentos
                rnOrcamentoArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove os orcamentos
                rnOrcamentoArquivo.Remove(contexto, eventoId);

                //Remove itens do XML
                rnImportacaoXmlEvento.RemoveTodos(contexto, eventoId);

                //Auditoria Arquivo Evidencia
                rnEvidenciaArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove Arquivo Evidencia
                rnEvidenciaArquivo.Remove(contexto, eventoId);

                //Remove o evento
                RemoveEvento(contexto, eventoId);
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

        public void RemoveTotal(int eventoId, string motivoExclusao, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            PequenaDespesa rnPequenaDespesa = new PequenaDespesa();
            PequenaDespesaServidor rnPequenaDespesaServidor = new PequenaDespesaServidor();
            EventoNotaFiscalArquivo rnEventoNotaFiscalArquivo = new EventoNotaFiscalArquivo();
            ComprovantePagamentoArquivo rnComprovantePagamentoArquivo = new ComprovantePagamentoArquivo();
            OrcamentoArquivo rnOrcamentoArquivo = new OrcamentoArquivo();
            EvidenciaArquivo rnEvidenciaArquivo = new EvidenciaArquivo();
            ImportacaoXmlEvento rnImportacaoXmlEvento = new ImportacaoXmlEvento();
            ExigenciaEventoArquivo rnExigenciaEventoArquivo = new ExigenciaEventoArquivo();
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();
            EventoCredito rnEventoCredito = new EventoCredito();
            EventoHistoricoExclusao rnEventoHistoricoExclusao = new EventoHistoricoExclusao();

            try
            {
                //Busca quantidade de exigencias
                int quantidadeExigencias = rnExigenciaEvento.ObtemquantidadeExigenciasPor(contexto, eventoId);

                //Adiciona despesa no historico
                rnEventoHistoricoExclusao.Insere(contexto, eventoId, motivoExclusao, usuarioId, quantidadeExigencias);

                //Remove todos os servidores associados à pequena despesa
                rnPequenaDespesaServidor.RemoveTodosPorEvento(contexto, eventoId);

                //Remove pequenas despesas
                rnPequenaDespesa.Remove(contexto, eventoId);

                //Auditoria Arquivo Nota Fiscal
                rnEventoNotaFiscalArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove Arquivo Nota fiscal
                rnEventoNotaFiscalArquivo.Remove(contexto, eventoId);

                //Auditoria Arquivo Comprovante pagamento
                rnComprovantePagamentoArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove comprovante pagamento
                rnComprovantePagamentoArquivo.Remove(contexto, eventoId);

                //Insere auditoria orcamentos
                rnOrcamentoArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove os orcamentos
                rnOrcamentoArquivo.Remove(contexto, eventoId);

                //Remove itens do XML
                rnImportacaoXmlEvento.RemoveTodos(contexto, eventoId);

                //Auditoria Arquivo Evidencia
                rnEvidenciaArquivo.InsereAuditoria(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove creditos das exigendas da despesa
                rnEventoCredito.RemovePorEvento(contexto, eventoId);

                //Auditoria Arquivo Exigencia
                rnExigenciaEventoArquivo.InsereAuditoriaPorEvento(contexto, eventoId, "REMOVIDO", System.Web.HttpContext.Current.Request.UserHostName);

                //Remove Arquivo Exigencia
                rnExigenciaEventoArquivo.RemovePorEvento(contexto, eventoId);

                //Remove Exigencia
                rnExigenciaEvento.Remove(contexto, eventoId);

                //Remove Arquivo Evidencia
                rnEvidenciaArquivo.Remove(contexto, eventoId);

                //Remove o evento
                RemoveEvento(contexto, eventoId);
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

        public bool EhEventoAprovadoPor(DataContext contexto, int eventoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM PrestacaoContas.EVENTO (NOLOCK)
                                    WHERE EVENTOID = @EVENTOID
                                          AND APROVADO = 1 ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaEventoParaAnalisePor(DateTime dataInicio, DateTime dataFim, string censo, int? planoTrabalhoId, int? tipoDespesa, int? eventoId, string situacao, string usuario)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT E.EVENTOID,
                                    E.PLANOTRABALHOID,
                                    E.CENSO,
                                    UE.NOME_COMP AS ESCOLA,
                                    E.NUMEROEVENTO,
                                    F.DESCRICAO AS FINALIDADE,
                                    CASE 
                                        WHEN E.TIPODESPESA = 0 THEN 'Despesa com NF-e'
                                        WHEN E.TIPODESPESA = 1 THEN 'Despesa com Demais Documentos Fiscais'
			                            WHEN E.TIPODESPESA = 2 THEN 'Pequena Despesa'
                                        WHEN E.TIPODESPESA = 4 THEN 'Pequena Despesa Sem Comprovação'
                                        WHEN E.TIPODESPESA = 3 THEN 'Despesa com Locomoção de Servidores'
                                        ELSE 'Tipo desconhecido: ' + cast(E.TIPODESPESA as varchar)
                                    END TIPODESPESA,
                                    E.TIPODESPESA AS TIPOEVENTO,
                                    E.DATACADASTRO AS DATAEVENTO,
                                    E.DATANOTAFISCAL,
                                    E.DATAPAGAMENTO,
                                    E.VALORPAGAMENTO,
                                    APROVADO,
									CASE
										WHEN E.NUMEROEVENTO IS NULL THEN 'Aberto'
										WHEN E.APROVADO IS NULL THEN 'Enviado para Análise'
										WHEN E.APROVADO = 1 THEN 'Validado'
										WHEN E.APROVADO = 0 THEN 'Reprovado'
									END SITUACAO,
									(SELECT COUNT(1) 
											FROM PrestacaoContas.EXIGENCIAEVENTO EX (NOLOCK)
                                            WHERE E.EVENTOID = EX.EVENTOID) as TOTALEXIGENCIAS,
	                                (SELECT COUNT(1) 
											FROM PrestacaoContas.EXIGENCIAEVENTO EX (NOLOCK)
                                            WHERE E.EVENTOID = EX.EVENTOID
											AND CUMPRIDA = 1) as TOTALEXIGENCIASCUMPRIDA
                            INTO #RESULTADO
                            from PrestacaoContas.EVENTO E (NOLOCK)
                                    INNER JOIN PrestacaoContas.PLANOTRABALHO PT (NOLOCK)
                                            ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
                                    INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
                                            ON E.CENSO = UE.UNIDADE_ENS
                                    INNER JOIN LY_USUARIO_UNIDADE_FIS UF ON UF.UNIDADE_FIS = UE.UNIDADE_ENS AND UF.USUARIO=@USUARIO
                                    INNER JOIN PrestacaoContas.FINALIDADE F (NOLOCK)
                                            ON PT.FINALIDADEID = F.FINALIDADEID
                                    LEFT JOIN PrestacaoContas.PEQUENADESPESA PD (NOLOCK)
                                            ON E.EVENTOID = PD.EVENTOID	       
                            WHERE E.DATAPAGAMENTO BETWEEN  @DATAINICIO AND @DATAFIM 
                                    AND E.NUMEROEVENTO IS NOT NULL 


                            SELECT * FROM #RESULTADO
                            WHERE NUMEROEVENTO IS NOT NULL 
                            ");


                if (!censo.IsNullOrEmptyOrWhiteSpace())
                {
                    sql.Append(@" 
                                      AND CENSO = @CENSO ");
                    contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                }

                if (planoTrabalhoId != null && planoTrabalhoId > 0)
                {
                    sql.Append(@" 
                                      AND PLANOTRABALHOID = @PLANOTRABALHOID ");
                    contextQuery.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, planoTrabalhoId == null ? (object)DBNull.Value : planoTrabalhoId);
                }

                if (tipoDespesa != null && tipoDespesa >= 0)
                {
                    sql.Append(@" 
                                      AND TIPOEVENTO = @TIPODESPESA ");
                    contextQuery.Parameters.Add("@TIPODESPESA", SqlDbType.Int, tipoDespesa == null ? (object)DBNull.Value : tipoDespesa);
                }

                if (eventoId != null && eventoId > 0)
                {
                    sql.Append(@"                                              
                                      AND EVENTOID = @EVENTOID ");
                    contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId == null ? (object)DBNull.Value : eventoId);
                }

                if (!situacao.IsNullOrEmptyOrWhiteSpace() && situacao != "Todos")
                {
                    if (situacao == "Enviado para Análise")
                    {
                        sql.Append(@" AND APROVADO IS NULL ");
                    }
                    else if (situacao == "Com Exigências")
                    {
                        sql.Append(@" AND APROVADO IS NULL
									  AND EVENTOID IN (SELECT EVENTOID
											FROM PRESTACAOCONTAS.EXIGENCIAEVENTO (NOLOCK)) ");
                    }
                    else if (situacao == "Cumprida")
                    {
                        sql.Append(@" AND APROVADO IS NULL
									  AND TOTALEXIGENCIAS > 0
                                      AND (TOTALEXIGENCIAS = TOTALEXIGENCIASCUMPRIDA)   ");

                    }
                    else if (situacao == "Validado")
                    {
                        sql.Append(@" AND APROVADO = 1 ");
                    }
                    else //Reprovado
                    {
                        sql.Append(@" AND APROVADO = 0 ");
                    }
                }

                sql.Append(@" ORDER BY EVENTOID DESC ");

                contextQuery.Command = sql.ToString();
                contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, dataFim);
                contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);


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

        public DataTable ListaEventoParaAnalisePor(string censo, DateTime? inicio, DateTime? fim, bool? mostrarSomenteComTodasExigenciasAnalisadas)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery contextQuery = new ContextQuery();
                DataTable dt = null;
                string sql = string.Empty;

                sql = @"
                SELECT 
                    E.EVENTOID,
                    E.PLANOTRABALHOID,
                    PT.FINALIDADEID,
                    E.CENSO,
                    UE.NOME_COMP AS ESCOLA,
                    E.NUMEROEVENTO,
                    F.DESCRICAO AS FINALIDADE,
                    CASE 
                        WHEN E.TIPODESPESA = 0 THEN 'Despesa com NF-e'
                        WHEN E.TIPODESPESA = 1 THEN 'Despesa com Demais Documentos Fiscais'
		                WHEN E.TIPODESPESA = 2 THEN 'Pequena Despesa'
                        WHEN E.TIPODESPESA = 4 THEN 'Pequena Despesa Sem Comprovação'
                        WHEN E.TIPODESPESA = 3 THEN 'Despesa com Locomoção de Servidores'
                        ELSE 'Tipo desconhecido: ' + cast(E.TIPODESPESA as varchar)
                    END TIPODESPESA,
                    E.TIPODESPESA AS TIPOEVENTO,
                    E.DATACADASTRO AS DATAEVENTO,
                    E.DATANOTAFISCAL,
                    E.DATAPAGAMENTO,
                    E.VALORPAGAMENTO,
	                CASE
		                WHEN E.NUMEROEVENTO IS NULL THEN 'Aberto'
		                WHEN E.APROVADO IS NULL THEN 'Enviado para Análise'
		                WHEN E.APROVADO = 1 THEN 'Validado'
		                WHEN E.APROVADO = 0 THEN 'Reprovado'
	                END SITUACAO,
	                (SELECT COUNT(1) 
			                FROM PrestacaoContas.EXIGENCIAEVENTO EX (NOLOCK)
                            WHERE E.EVENTOID = EX.EVENTOID) as TOTALEXIGENCIAS,
	                (select count(1)
			                from PrestacaoContas.EXIGENCIAEVENTO EX (nolock)
			                where E.EVENTOID = EX.EVENTOID
			                and (EX.APROVADO = 1 or EX.REJEITADO = 1)
	                ) as TOTALEXIGENCIASANALISADAS,
	                (select count(1)
			                from PrestacaoContas.EXIGENCIAEVENTO EX (nolock)
			                where E.EVENTOID = EX.EVENTOID
			                and (EX.REJEITADO = 1)
	                ) as TOTALEXIGENCIASREJEITADAS

                from 
                PrestacaoContas.EVENTO E (NOLOCK)
                INNER JOIN PrestacaoContas.PLANOTRABALHO PT (NOLOCK) ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID
                INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON E.CENSO = UE.UNIDADE_ENS
                INNER JOIN PrestacaoContas.FINALIDADE F (NOLOCK) ON PT.FINALIDADEID = F.FINALIDADEID
                LEFT JOIN PrestacaoContas.PEQUENADESPESA PD (NOLOCK) ON E.EVENTOID = PD.EVENTOID	       

                WHERE
                E.CENSO = @CENSO
                AND E.NUMEROEVENTO IS NOT NULL
                and E.APROVADO is null
                and (@INICIO is null or E.DATAPAGAMENTO >= @INICIO)
                and (@FIM is null or E.DATAPAGAMENTO <= @FIM)
                and (isnull(@MOSTRARSOMENTECOMTODASEXIGENCIASANALISADAS, 0) = 0 or not exists (
	                select top 1 1 from PrestacaoContas.EXIGENCIAEVENTO ex (nolock)
	                where ex.EVENTOID = e.EVENTOID
	                and ex.REJEITADO = 0
	                and ex.APROVADO = 0
                ))

                ORDER BY 
                E.DATAPAGAMENTO DESC
                ";

                contextQuery.Command = sql;

                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);
                contextQuery.Parameters.Add("@INICIO", SqlDbType.DateTime, (object)inicio ?? DBNull.Value);
                contextQuery.Parameters.Add("@FIM", SqlDbType.DateTime, (object)fim ?? DBNull.Value);
                contextQuery.Parameters.Add("@MOSTRARSOMENTECOMTODASEXIGENCIASANALISADAS", SqlDbType.Bit, (object)mostrarSomenteComTodasExigenciasAnalisadas ?? DBNull.Value);

                return contexto.GetDataTable(contextQuery);
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

        public ValidacaoDados ValidaFinalizacao(int eventoId, int finalidadeId, string usuarioId, bool desconsiderarExigenciaSemAnalise)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ImportacaoXmlEvento rnImportacaoXmlEvento = new ImportacaoXmlEvento();
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (eventoId <= 0)
            {
                mensagens.Add("Campo CÓDIGO DA EXIGÊNCIA é obrigatório.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (finalidadeId == 2) //Merenda
                    {
                        //Verifica se está configurado para ignorar algumas regras de despesas e analise
                        var retiradaValidacaoDespesa = Convert.ToBoolean(ConfigurationManager.AppSettings["RetiradaValidacaoDespesa"] ?? "false");

                        //Verifica se possui algum item da NF não analisado
                        if (!retiradaValidacaoDespesa && rnImportacaoXmlEvento.PossuiItensNaoAnalisadosPor(contexto, eventoId))
                        {
                            mensagens.Add("Esta despesa não pode ser finalizada pois possui itens da Nota Fiscal sem análise.");
                        }
                    }

                    //se for considerar as exigências sem análise...
                    if (!desconsiderarExigenciaSemAnalise)
                    {
                        //Verifica se possui alguma exigencia não analisado
                        if (rnExigenciaEvento.PossuiExigenciaSemAnalisePor(contexto, eventoId))
                        {
                            mensagens.Add("Esta despesa não pode ser finalizada pois possui exigência sem análise.");
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

        public void Finaliza(int eventoId, string usuarioId)
        {
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool aprovado = false;

            try
            {
                //Verifica se possui alguma exigencia não aprovada                            
                if (rnExigenciaEvento.PossuiExigenciaAbertaPor(contexto, eventoId))
                {
                    //Reprova
                    aprovado = false;
                }
                else
                {
                    aprovado = true;
                }

                this.Finaliza(contexto, aprovado, eventoId, usuarioId);
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

        public void FinalizaForcandoAprovacao(int eventoId, string usuarioId)
        {
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            bool aprovado = false;

            try
            {
                this.Finaliza(contexto, true, eventoId, usuarioId);
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

        private void Finaliza(DataContext ctx, bool aprova, int eventoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE PrestacaoContas.EVENTO
                                           SET APROVADO = @APROVADO, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO
                                         WHERE EVENTOID = @EVENTOID ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
            contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, aprova);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public List<DadosEventoArquivo> ListaTodosOsArquivosDaDespesa(int eventoId)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                return ListaTodosOsArquivosDaDespesa(ctx, eventoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private List<DadosEventoArquivo> ListaTodosOsArquivosDaDespesa(DataContext ctx, int eventoId)
        {
            List<DadosEventoArquivo> dados = new List<DadosEventoArquivo>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  
                select 'EventoNotaFiscalArquivo' as TABELA, EVENTONOTAFISCALARQUIVOID as ARQUIVOID, @EVENTOID as EVENTOID, CHAVEARQUIVO, ARQUIVO, TIPOARQUIVO, NOMEARQUIVO, null as DESCRICAO, null as JUSTIFICATIVA, USUARIOID, DATACADASTRO, DATAALTERACAO 
                from PrestacaoContas.EVENTONOTAFISCALARQUIVO enfa (nolock) where EVENTOID = @EVENTOID
                union
                select 'ComprovantePagamentoArquivo' as TABELA, COMPROVANTEPAGAMENTOARQUIVOID as ARQUIVOID, @EVENTOID as EVENTOID, CHAVEARQUIVO, ARQUIVO, TIPOARQUIVO, NOMEARQUIVO, null as DESCRICAO, null as JUSTIFICATIVA, USUARIOID, DATACADASTRO, DATAALTERACAO 
                from PrestacaoContas.COMPROVANTEPAGAMENTOARQUIVO cpa (nolock) where EVENTOID = @EVENTOID
                union
                select 'OrcamentoArquivo' as TABELA, ORCAMENTOARQUIVOID as ARQUIVOID, @EVENTOID as EVENTOID, CHAVEARQUIVO, ARQUIVO, TIPOARQUIVO, NOMEARQUIVO, null as DESCRICAO, null as JUSTIFICATIVA, USUARIOID, DATACADASTRO, DATAALTERACAO 
                from PrestacaoContas.ORCAMENTOARQUIVO oa (nolock) where EVENTOID = @EVENTOID
                union
                select 'EvidenciaArquivo' as TABELA, EVIDENCIAARQUIVOID as ARQUIVOID, @EVENTOID as EVENTOID, CHAVEARQUIVO, ARQUIVO, TIPOARQUIVO, NOMEARQUIVO, null as DESCRICAO, null as JUSTIFICATIVA, USUARIOID, DATACADASTRO, DATAALTERACAO 
                from PrestacaoContas.EVIDENCIAARQUIVO oa (nolock) where EVENTOID = @EVENTOID
                union
                select 'ExigenciaEventoArquivo2' as TABELA, EXIGENCIAEVENTOARQUIVOID as ARQUIVOID, @EVENTOID as EVENTOID, CHAVEARQUIVO, ARQUIVO, TIPOARQUIVO, NOMEARQUIVO, mee.DESCRICAO, ee.JUSTIFICATIVA, eea.USUARIOID, eea.DATACADASTRO, eea.DATAALTERACAO 
                from PrestacaoContas.EXIGENCIAEVENTOARQUIVO eea (nolock)
                inner join PrestacaoContas.EXIGENCIAEVENTO ee (nolock) on ee.EXIGENCIAEVENTOID = eea.EXIGENCIAEVENTOID
                inner join PrestacaoContas.MOTIVOEXIGENCIAEVENTO mee (nolock) on mee.MOTIVOEXIGENCIAEVENTOID = ee.MOTIVOEXIGENCIAEVENTOID
                where eea.EXIGENCIAEVENTOID in (select EXIGENCIAEVENTOID from PrestacaoContas.EXIGENCIAEVENTO ee (nolock) where EVENTOID = @EVENTOID)
                ";

                contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosEventoArquivo item = new DadosEventoArquivo();

                    item.Tabela = Convert.ToString(reader["TABELA"]);
                    item.ArquivoId = Convert.ToInt32(reader["ARQUIVOID"]);
                    item.EventoId = Convert.ToInt32(reader["EVENTOID"]);
                    item.ChaveArquivo = Convert.ToString(reader["CHAVEARQUIVO"]);
                    item.Arquivo = (byte[])reader["ARQUIVO"];
                    item.TipoArquivo = Convert.ToString(reader["TIPOARQUIVO"]);
                    item.NomeArquivo = Convert.ToString(reader["NOMEARQUIVO"]);
                    item.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    item.Justificativa = Convert.ToString(reader["JUSTIFICATIVA"]);
                    item.UsuarioId = Convert.ToString(reader["USUARIOID"]);
                    item.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    item.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);

                    dados.Add(item);
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
    }
}