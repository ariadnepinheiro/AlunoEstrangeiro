using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class EventoHistoricoExclusao
    {
        public void Insere(DataContext contexto, int eventoId, string motivoExclusao, string usuarioId, int quantidadeExigencias)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO PRESTACAOCONTAS.EVENTOHISTORICOEXCLUSAO 
                                                    (EVENTOID, 
                                                    DESCRICAO, 
                                                    PLANOTRABALHOID, 
                                                    FORNECEDORID, 
                                                    CENSO, 
                                                    JUSTIFICATIVAORCAMENTO, 
                                                    CHAVEACESSO, 
                                                    NUMERONOTAFISCAL, 
                                                    VALORNOTAFISCAL, 
                                                    DATANOTAFISCAL,
                                                    OBSERVACOES, 
                                                    EVIDENCIAS, 
                                                    DATAPAGAMENTO, 
                                                    VALORPAGAMENTO, 
                                                    NUMEROEVENTO, 
                                                    TIPODESPESA, 
                                                    APROVADO, 
                                                    DATAAPROVACAO, 
                                                    USUARIOID, 
                                                    USUARIOAPROVACAO, 
                                                    DATACADASTRO, 
                                                    DATAALTERACAO,
                                                    QUANTIDADEEXIGENCIAS, 
                                                    MOTIVOEXCLUSAO, 
                                                    USUARIOEXCLUSAO, 
                                                    DATAEXCLUSAO) 
                                        SELECT *,
                                                @QUANTIDADEEXIGENCIAS, 
                                                @MOTIVOEXCLUSAO, 
                                                @USUARIOEXCLUSAO, 
                                                @DATAEXCLUSAO
                                        FROM   LYCEUM.PRESTACAOCONTAS.EVENTO
                                        WHERE  EVENTOID = @EVENTOID  ";

            contextQuery.Parameters.Add("@EVENTOID", SqlDbType.Int, eventoId);
            contextQuery.Parameters.Add("@QUANTIDADEEXIGENCIAS", SqlDbType.Int, quantidadeExigencias);            
            contextQuery.Parameters.Add("@MOTIVOEXCLUSAO", SqlDbType.VarChar, motivoExclusao);
            contextQuery.Parameters.Add("@USUARIOEXCLUSAO", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAEXCLUSAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
