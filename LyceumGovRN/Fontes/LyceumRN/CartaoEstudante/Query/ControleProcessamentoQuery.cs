using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class ControleProcessamentoQuery : QueryBase<ControleProcessamentoQuery>
    {
        private const string TABELA_CONTROLE_PROCESSAMENTO = "Lyceum.CartaoEstudante.ControleProcessamento";
        public static readonly string INSERT_CONTROLE_PROCESSAMENTO = "INSERT INTO " + TABELA_CONTROLE_PROCESSAMENTO +
            " (NOMEPROCESSO, DATAAGENDA, FREQUENCIA, DATAINICIOMOVIMENTO, DATAFIMMOVIMENTO, USUARIO) " +
            "VALUES " +
            " (@NOMEPROCESSO, @DATAAGENDA, @FREQUENCIA, @DATAINICIOMOVIMENTO, @DATAFIMMOVIMENTO, @USUARIO)";

        ControleProcessamentoQuery() {
            NomeTabela = TABELA_CONTROLE_PROCESSAMENTO;
        }

        public ControleProcessamento ObtemUltimaAtualizacaoPor(string nomeProcesso)
        {
            string SELECT_DATAS =
                "SELECT DATAINCLUSAO, MAX(DATAINICIOMOVIMENTO) DATAINICIOMOVIMENTO, MAX(DATAFIMMOVIMENTO) DATAFIMMOVIMENTO " +
                "FROM " + NomeTabela + " " +
                "WHERE " +
                "   NOMEPROCESSO = @NOMEPROCESSO " +
                "GROUP BY " +
                "	DATAINCLUSAO " +
                "HAVING " +
                "	DATAINCLUSAO = MAX(DATAINCLUSAO)";

            ControleProcessamento controleProcessamento = ObtemPor<ControleProcessamento>(
                SELECT_DATAS, 
                nomeProcesso
            );

            return controleProcessamento;
        }

        public void Insere(ControleProcessamento controleProcessamento)
		{
            AplicarModificacoes(INSERT_CONTROLE_PROCESSAMENTO,
                controleProcessamento.NomeProcesso, controleProcessamento.DataAgenda, controleProcessamento.Frequencia,
                controleProcessamento.DataInicioMovimento, controleProcessamento.DataFimMovimento, controleProcessamento.Usuario
            );
		}

        private int ObtemMaxID()
        {
            string SELECT_MAX_ID = "SELECT MAX(CONTROLEPROCESSAMENTOID) FROM " + NomeTabela;

            int retorno = ObtemValorSimples<int>(SELECT_MAX_ID);

            return retorno;
        }

        public ControleProcessamento ObtemPorID(int controleProcessamentoID)
		{
            string SELECT_CONTROLE_PROCESSAMENTO = "SELECT * FROM " + NomeTabela + " WHERE CONTROLEPROCESSAMENTOID = @CONTROLEPROCESSAMENTOID";
            return ObtemPor<ControleProcessamento>(SELECT_CONTROLE_PROCESSAMENTO, controleProcessamentoID);
		}

        public ControleProcessamento ObtemPor(string nomeProcesso)
        {
            string SELECT_CONTROLE_PROCESSAMENTO = "SELECT top 1 * FROM " + NomeTabela + " WHERE NOMEPROCESSO = @NOMEPROCESSO";
            return ObtemPor<ControleProcessamento>(SELECT_CONTROLE_PROCESSAMENTO, nomeProcesso);
        }

        public ControleProcessamento ObtemPor(string nomeProcesso, DateTime dataAgenda)
        {
            string SELECT_CONTROLE_PROCESSAMENTO = "SELECT * FROM " + NomeTabela + " WHERE NOMEPROCESSO = @NOMEPROCESSO AND DATAAGENDA = @DATAAGENDA";
            return ObtemPor<ControleProcessamento>(SELECT_CONTROLE_PROCESSAMENTO, nomeProcesso, dataAgenda);
        }

        public void Insere(ControleProcessamento controleProcessamento, LogControleProcessamento logControleProcessamento)
		{
            ContextQuery controleProcessamentoCtx = new ContextQuery(INSERT_CONTROLE_PROCESSAMENTO);
            ContextQuery logControleProcessamentoCtx = new ContextQuery(LogControleProcessamentoQuery.INSERT_LOG_CONTROLE_PROCESSAMENTO);
            DataContext dataContext = DataContextBuilder.FromLyceum.UsingLock();
            LogControleProcessamentoQuery logControleProcessamentoQuery = LogControleProcessamentoQuery.Instancia;

            try
            {
                VerificaEAtribuiParametros(
                    controleProcessamentoCtx.Command,
                    new object[] { 
                        controleProcessamento.NomeProcesso, controleProcessamento.DataAgenda, controleProcessamento.Frequencia, 
                        controleProcessamento.DataInicioMovimento, controleProcessamento.DataFimMovimento, controleProcessamento.Usuario
                    },
                    controleProcessamentoCtx
                );

                dataContext.ApplyModifications(controleProcessamentoCtx);

                int ultimoID = ObtemMaxID();
                ControleProcessamento controleProcessamentoInserido = ObtemPorID(ultimoID);

                if (controleProcessamentoInserido.ControleProcessamentoId > 0)
                {
                    VerificaEAtribuiParametros(
                        logControleProcessamentoCtx.Command,
                        new object[] {                     
                            controleProcessamentoInserido.ControleProcessamentoId, logControleProcessamento.DataProcessamento, 
                            logControleProcessamento.RegistroInicial, logControleProcessamento.RegistroFinal, logControleProcessamento.QuantidadeRegistrosRetornados,
                            logControleProcessamento.UltimoRegistroProcessado, logControleProcessamento.SituacaoProcessamento
                        },
                        logControleProcessamentoCtx
                    );

                    dataContext.ApplyModifications(logControleProcessamentoCtx);
                }
            }
            catch (Exception e)
            {
                dataContext.Abandon();
                throw e;
            }
            finally
            {
                dataContext.Dispose();
            }
		}

        public bool PodeProcessar(string nomeProcesso, TimeSpan horarioProcessamento)
        {
            string SELECT_HORARIO_VALIDO = "SELECT 1 FROM " + NomeTabela + @" 
                                            WHERE NOMEPROCESSO = @NOMEPROCESSO 
                                            AND convert(time, @HORARIOPROCESSAMENTO) BETWEEN convert(time,DATAINICIOMOVIMENTO) AND convert(time,DATAFIMMOVIMENTO)";
            return Possui(SELECT_HORARIO_VALIDO, nomeProcesso, horarioProcessamento);   
        }
    }
}
