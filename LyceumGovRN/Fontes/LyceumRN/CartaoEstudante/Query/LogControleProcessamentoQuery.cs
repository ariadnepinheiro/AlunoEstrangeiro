using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class LogControleProcessamentoQuery : QueryBase<LogControleProcessamentoQuery>
    {
        private const string TABELA_LOG_CONTROLE_PROCESSAMENTO = "Lyceum.CartaoEstudante.LogControleProcessamento";
        public static readonly string INSERT_LOG_CONTROLE_PROCESSAMENTO = "INSERT INTO " + TABELA_LOG_CONTROLE_PROCESSAMENTO +
            " (CONTROLEPROCESSAMENTOID, DATAPROCESSAMENTO, " +
            "  REGISTROINICIAL, REGISTROFINAL, QUANTIDADEREGISTROSRETORNADOS, ULTIMOREGISTROPROCESSADO, SITUACAOPROCESSAMENTO) " +
            "VALUES " +
            " (@CONTROLEPROCESSAMENTOID, @DATAPROCESSAMENTO, " +
            "  @REGISTROINICIAL, @REGISTROFINAL, @QUANTIDADEREGISTROSRETORNADOS, @ULTIMOREGISTROPROCESSADO, @SITUACAOPROCESSAMENTO)";

        public static readonly string INSERT_LOG_ERRO_CONTROLE_PROCESSAMENTO = "INSERT INTO " + TABELA_LOG_CONTROLE_PROCESSAMENTO +
        " (CONTROLEPROCESSAMENTOID, DATAPROCESSAMENTO, SITUACAOPROCESSAMENTO) " + "VALUES " + " (@CONTROLEPROCESSAMENTOID, GETDATE(), 'N')";


        LogControleProcessamentoQuery() {
            NomeTabela = TABELA_LOG_CONTROLE_PROCESSAMENTO;
        }

        public void Insere(LogControleProcessamento logControleProcessamento)
        {
            AplicarModificacoes(
                INSERT_LOG_CONTROLE_PROCESSAMENTO,
                logControleProcessamento.ControleProcessamentoId, logControleProcessamento.DataProcessamento,
                logControleProcessamento.RegistroInicial, logControleProcessamento.RegistroFinal,
                logControleProcessamento.QuantidadeRegistrosRetornados, logControleProcessamento.UltimoRegistroProcessado,
                logControleProcessamento.SituacaoProcessamento
            );
        }

        public void InsereErro(LogControleProcessamento logControleProcessamento)
        {
            AplicarModificacoes(INSERT_LOG_ERRO_CONTROLE_PROCESSAMENTO, logControleProcessamento.ControleProcessamentoId);
        }

        public void Remove(ControleProcessamento controleProcessamento)
		{
            string DELETE_CONTROLE_PROCESSAMENTO = "DELETE FROM " + NomeTabela + " WHERE CONTROLEPROCESSAMENTOID = @CONTROLEPROCESSAMENTOID";

            AplicarModificacoes(DELETE_CONTROLE_PROCESSAMENTO,
                controleProcessamento.ControleProcessamentoId
            );
		}
    }
}
