using System;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class RetornoQuery : QueryBase<RetornoQuery>
    {
        private const string TABELA_RETORNO = "LYCEUM.CartaoEstudante.RETORNO";
        private const string VIEW_ULTIMO_RETORNO = "LYCEUM.CartaoEstudante.VW_RETORNO_ULTIMA_REMESSA";
        public static readonly string INSERT_RETORNO = "INSERT INTO " + TABELA_RETORNO +
            " (REMESSAID, OPERADORAID, IDBENEFICIARIO, DATAPROCESSAMENTO, SITUACAOPROCESSAMENTO) " +
            "VALUES " +
            " (@REMESSAID, @OPERADORAID, @IDBENEFICIARIO, @DATAPROCESSAMENTO, @SITUACAOPROCESSAMENTO)";

        RetornoQuery() { }

        public void Insere(Retorno retorno)
        {
            AplicarModificacoes(
                INSERT_RETORNO,
                retorno.RemessaId, retorno.OperadoraId, retorno.IdBeneficiario, retorno.DataProcessamento, retorno.SituacaoProcessamento
            );
        }

        public int ObtemUltimoRetornoID()
        {
            string SELECT_MAX_RETORNOID = "SELECT MAX(RETORNOID) FROM " + TABELA_RETORNO;

            int retorno = ObtemValorSimples<int>(SELECT_MAX_RETORNOID);

            return retorno;
        }

        public Retorno ObtemUltimoRetornoPor(int remessaId)
        {
            string SELECT_MAX_RETORNOID = " SELECT TOP 1 R.* FROM " + TABELA_RETORNO + " R " +
                                          " LEFT JOIN " + VIEW_ULTIMO_RETORNO + " VR ON VR.RETORNOID = R.RETORNOID " +
                                          " WHERE VR.REMESSAID = " + remessaId.ToString();

            Retorno retorno = ObtemPor<Retorno>(SELECT_MAX_RETORNOID);

            return retorno;
        }

        public bool PossuiRetornoPor(int remessaId, string situacaoProcessamento)
        {
            string SELECT_POSSUI_POR_REMESSA_SITUACAO = 
                "SELECT 1 FROM " + TABELA_RETORNO + 
                " WHERE REMESSAID = @REMESSAID AND SITUACAOPROCESSAMENTO = @SITUACAOPROCESSAMENTO";

            return Possui(SELECT_POSSUI_POR_REMESSA_SITUACAO, remessaId, situacaoProcessamento);
        }

        public Retorno ObtemPorId(int retornoId)
        {
            string SELECT_RETORNOID = "SELECT TOP 1 * FROM " + TABELA_RETORNO + " WHERE RETORNOID = " + retornoId.ToString();

            Retorno retorno = ObtemPor<Retorno>(SELECT_RETORNOID);

            return retorno;
        }
    }
}
