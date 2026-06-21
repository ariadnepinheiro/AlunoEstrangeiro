using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.ComponentModel;

namespace Techne.Lyceum.RN
{
    public class HistoricoImportacao : RNBase
    {
        public enum StatusProcessamento
        {
            [Description("Em Execução")]
            EmExecucao = 1,
            [Description("Concluído")]
            Concluido = 2,
            [Description("Falha")]
            Falha = 3
        }

        public enum TipoEntradaSistemaEnum
        {
            ImportacaoCSV = 1,
            Digitado = 2,
            ImportacaoWebService = 3
        }

        public enum TipoImportacaoEnum
        {
            IdFuncionalSeplag = 1,
            ClassificadosProcessoSeletivoAlunoCeperj = 2
        }

        /// <summary>
        ///REF6. Query para Inclusão de Histórico de Importação com a situação como “Em Execução”
        /// </summary>
        /// <param name="historicoImportacao"></param>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public int Inserir(Entidades.HistoricoImportacao historicoImportacao, DataContext contexto)
        {
            int idHistoricoImportacao = 0;
            string strSQL = string.Empty;
            ContextQuery contextQuery = null;

            try
            {
                strSQL = @"INSERT INTO [LYCEUM].[dbo].[HISTORICOIMPORTACAO]
                                    (
                                        DATAIMPORTACAO,
                                        NOMEARQUIVO,
                                        STATUSPROCESSAMENTO,
                                        TIPOIMPORTACAOID,
                                        TIPOENTRADASISTEMAID,
                                        ARQUIVO,
                                        USUARIO
                                    )
                                VALUES  
                                    (
                                        @DATAIMPORTACAO,
                                        @NOMEARQUIVO,
                                        @STATUSPROCESSAMENTO,
                                        @TIPOIMPORTACAOID,
                                        @TIPOENTRADASISTEMAID,
                                        @ARQUIVO,
                                        @USUARIO
                                    );";

                contextQuery = new ContextQuery(strSQL);
                contextQuery.Parameters.Add("@DATAIMPORTACAO", historicoImportacao.DataImportacao);
                contextQuery.Parameters.Add("@NOMEARQUIVO", historicoImportacao.NomeArquivo);
                contextQuery.Parameters.Add("@STATUSPROCESSAMENTO", Convert.ToInt32(historicoImportacao.StatusProcessamento));
                contextQuery.Parameters.Add("@TIPOENTRADASISTEMAID", Convert.ToInt32(historicoImportacao.TipoEntradaSistemaId));
                contextQuery.Parameters.Add("@TIPOIMPORTACAOID", Convert.ToInt32(historicoImportacao.TipoImportacaoId));
                contextQuery.Parameters.Add("@ARQUIVO", historicoImportacao.Arquivo);
                contextQuery.Parameters.Add("@USUARIO", historicoImportacao.Usuario);
                contexto.ApplyModifications(contextQuery);

                //Retorna o último Id do Histórico da Importação
                strSQL = @"SELECT MAX(HISTORICOIMPORTACAOID) FROM [LYCEUM].[dbo].[HISTORICOIMPORTACAO]
                            WHERE DATAIMPORTACAO = @DATAIMPORTACAO
                              AND NOMEARQUIVO = @NOMEARQUIVO
                              AND TIPOENTRADASISTEMAID = @TIPOENTRADASISTEMAID
                              AND STATUSPROCESSAMENTO = @STATUSPROCESSAMENTO
                              AND USUARIO = @USUARIO";

                object id = contexto.GetReturnValue(new ContextQuery(strSQL, contextQuery.Parameters));

                if (id != null && !(id == DBNull.Value))
                    idHistoricoImportacao = Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador de Sistema repassando esta mensagem ou tente mais tarde. {0} Erro gerado {1}",
                    Environment.NewLine, Convert.ToString(ex.InnerException));
                throw new Exception(mensagem);
            }

            return idHistoricoImportacao;
        }

        /// <summary>
        /// REF8. Query para Atualização do Registro de Histórico de Importação com a situação como “Falha”
        /// </summary>
        /// <param name="idHistoricoImportacao"></param>
        /// <param name="arquivoAtualizado"></param>
        /// <param name="statusProcessamentoAtualizado"></param>
        /// <param name="contexto"></param>
        public void Atualizar(int idHistoricoImportacao, byte[] arquivoAtualizado, StatusProcessamento statusProcessamentoAtualizado, DataContext contexto)
        {
            string strSQL = string.Empty;
            ContextQuery contextQuery = null;

            try
            {
                strSQL = @"UPDATE [LYCEUM].[dbo].[HISTORICOIMPORTACAO] 
                              SET STATUSPROCESSAMENTO = @STATUSPROCESSAMENTO
                                , ARQUIVO = @ARQUIVO
                            WHERE HISTORICOIMPORTACAOID = @HISTORICOIMPORTACAOID ";

                contextQuery = new ContextQuery(strSQL);
                contextQuery.Parameters.Add("@STATUSPROCESSAMENTO", Convert.ToInt32(statusProcessamentoAtualizado));
                contextQuery.Parameters.Add("@ARQUIVO", arquivoAtualizado);
                contextQuery.Parameters.Add("@HISTORICOIMPORTACAOID", idHistoricoImportacao);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador de Sistema repassando esta mensagem ou tente mais tarde. {0} Erro gerado {1}",
                    Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        /// <summary>
        /// REF9. Query para Atualização do Registro de Histórico de Importação com a situação como “Concluído”
        /// </summary>
        /// <param name="idHistoricoImportacao"></param>
        /// <param name="arquivoAtualizado"></param>
        /// <param name="statusProcessamentoAtualizado"></param>
        /// <param name="TotalImportados"></param>
        /// <param name="contexto"></param>
        public void AtualizarImportacaoConcluida(int idHistoricoImportacao, byte[] arquivoAtualizado, StatusProcessamento statusProcessamentoAtualizado, int TotalImportados, DataContext contexto)
        {
            string strSQL = string.Empty;
            ContextQuery contextQuery = null;

            try
            {
                strSQL = @"
                            UPDATE [LYCEUM].[dbo].[HISTORICOIMPORTACAO]
                               SET STATUSPROCESSAMENTO = @STATUSPROCESSAMENTO
                                 , ARQUIVO = @ARQUIVO
                                 , TOTALREGISTROIMPORTADO = @TOTALREGISTROIMPORTADO
                            WHERE HISTORICOIMPORTACAOID = @HISTORICOIMPORTACAOID ";

                contextQuery = new ContextQuery(strSQL);
                contextQuery.Parameters.Add("@STATUSPROCESSAMENTO", Convert.ToInt32(statusProcessamentoAtualizado));
                contextQuery.Parameters.Add("@ARQUIVO", arquivoAtualizado);
                contextQuery.Parameters.Add("@TOTALREGISTROIMPORTADO", TotalImportados);
                contextQuery.Parameters.Add("@HISTORICOIMPORTACAOID", idHistoricoImportacao);

                contexto.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador de Sistema repassando esta mensagem ou tente mais tarde. {0} Erro gerado {1}",
                    Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        public DataTable Listar()
        {
            string strSQL = string.Empty;
            DataContext contexto = null;
            ContextQuery contextQuery = null;
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                strSQL = @"SELECT H.[HISTORICOIMPORTACAOID],
                                  H.[DATAIMPORTACAO],
                                  H.[NOMEARQUIVO],
                                  H.[STATUSPROCESSAMENTO],
                                  H.[TIPOENTRADASISTEMAID]
                             FROM [LYCEUM].[dbo].[HISTORICOIMPORTACAO] H WITH (NOLOCK) 
                         ORDER BY H.[DATAIMPORTACAO] DESC ";

                contextQuery = new ContextQuery(strSQL);

                dt = Consultar(contextQuery);
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        public Techne.Lyceum.RN.Entidades.HistoricoImportacao Consultar(int idHistoricoImportacao)
        {
            RN.Entidades.HistoricoImportacao historicoImportacao = new RN.Entidades.HistoricoImportacao();
            string strSQL = string.Empty;

            DataContext contexto = null;
            ContextQuery contextQuery = null;
            DataTable dtArquivoHistorico = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                strSQL = @" SELECT H.[HISTORICOIMPORTACAOID],
                                   H.[DATAIMPORTACAO],
                                   H.[NOMEARQUIVO],
                                   H.[STATUSPROCESSAMENTO],
                                   H.[TIPOIMPORTACAOID],
                                   H.[TIPOENTRADASISTEMAID],
                                   H.[ARQUIVO],
                                   H.[TOTALREGISTROIMPORTADO]
                              FROM [LYCEUM].[dbo].[HISTORICOIMPORTACAO] H
                             WHERE H.[HISTORICOIMPORTACAOID] = @HISTORICOIMPORTACAOID ";

                contextQuery = new ContextQuery(strSQL);
                contextQuery.Parameters.Add("@HISTORICOIMPORTACAOID", idHistoricoImportacao);
                dtArquivoHistorico = Consultar(contextQuery);

                if (dtArquivoHistorico.Rows.Count > 0)
                {
                    historicoImportacao.HistoricoImportacaoId = Convert.ToInt32(dtArquivoHistorico.Rows[0]["HISTORICOIMPORTACAOID"]);
                    historicoImportacao.DataImportacao = Convert.ToDateTime(dtArquivoHistorico.Rows[0]["DATAIMPORTACAO"]);
                    historicoImportacao.NomeArquivo = (string)dtArquivoHistorico.Rows[0]["NOMEARQUIVO"];
                    historicoImportacao.StatusProcessamento = (StatusProcessamento)Convert.ToInt32(dtArquivoHistorico.Rows[0]["STATUSPROCESSAMENTO"]);
                    historicoImportacao.TipoImportacaoId = (TipoImportacaoEnum)Convert.ToInt32(dtArquivoHistorico.Rows[0]["TIPOIMPORTACAOID"]);
                    historicoImportacao.TipoEntradaSistemaId = (TipoEntradaSistemaEnum)Convert.ToInt32(dtArquivoHistorico.Rows[0]["TIPOENTRADASISTEMAID"]);
                    if (!(dtArquivoHistorico.Rows[0]["TOTALREGISTROIMPORTADO"] == DBNull.Value ))
                        historicoImportacao.TotalRegistroImportado = Convert.ToInt32(dtArquivoHistorico.Rows[0]["TOTALREGISTROIMPORTADO"]);
                    historicoImportacao.Arquivo = dtArquivoHistorico.Rows[0]["ARQUIVO"] as byte[];
                }
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                    Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return historicoImportacao;
        }

        public Techne.Lyceum.RN.Entidades.HistoricoImportacao AdicionaItemHistorico(string nomeArquivo, byte[] bytesArquivoZipado, StatusProcessamento statusProcessamento, TipoEntradaSistemaEnum tipoEntradaSistema, TipoImportacaoEnum tipoImportacao, string usuario)
        {
            //Insere registro de histórico
            RN.Entidades.HistoricoImportacao historicoImportacao = new RN.Entidades.HistoricoImportacao();
            historicoImportacao.Arquivo = bytesArquivoZipado;
            historicoImportacao.DataImportacao = DateTime.Now;
            historicoImportacao.NomeArquivo = nomeArquivo;
            historicoImportacao.StatusProcessamento = statusProcessamento;
            historicoImportacao.TipoImportacaoId = tipoImportacao;
            historicoImportacao.TipoEntradaSistemaId = tipoEntradaSistema;
            historicoImportacao.Usuario = usuario;
            return historicoImportacao;
        }
    }
}
