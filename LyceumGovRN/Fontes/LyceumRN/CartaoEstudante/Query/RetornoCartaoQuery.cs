using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class RetornoCartaoQuery: QueryBase<RetornoCartaoQuery>
    {
        private const string TABELA_RETORNO_CARTAO = "LYCEUM.CartaoEstudante.RETORNOCARTAO";

        RetornoCartaoQuery() { }

        public void InsereRetornoCartao(RetornoCartao retornoCartao)
        {
            string INSERT_RETORNO_CARTAO = "INSERT INTO " + TABELA_RETORNO_CARTAO + " (" +
                "TIPOSITUACAOCARTAOID, OPERADORAID, TIPOCANCELAMENTOID, ALUNO, NUMEROCHIP, " +
                "NUMEROCARTAO, NUMEROLOTE, DATAIMPRESSAO, DATAUTILIZACAO, IDBENEFICIARIO, " +
                "LOCALIMPRESSAO, DATAENTREGALOTE, DATACONFIRMACAOENTREGA) " +
                "VALUES (" +
                "@TIPOSITUACAOCARTAOID, @OPERADORAID, @TIPOCANCELAMENTOID, @ALUNO, @NUMEROCHIP, " +
                "@NUMEROCARTAO, @NUMEROLOTE, @DATAIMPRESSAO, @DATAUTILIZACAO, @IDBENEFICIARIO, " +
                "@LOCALIMPRESSAO, @DATAENTREGALOTE, @DATACONFIRMACAOENTREGA)";

            AplicarModificacoes(INSERT_RETORNO_CARTAO,
                retornoCartao.TipoSituacaoCartaoId, retornoCartao.OperadoraID, retornoCartao.TipoCancelamentoId,
                retornoCartao.Aluno, retornoCartao.NumeroChip, retornoCartao.NumeroCartao, retornoCartao.NumeroLote,
                retornoCartao.DataImpressao, retornoCartao.DataUtilizacao, retornoCartao.IdBeneficiario,
                retornoCartao.LocalImpressao, retornoCartao.DataEntregaLote, retornoCartao.DataConfirmacaoEntrega
            );
        }
    }
}
