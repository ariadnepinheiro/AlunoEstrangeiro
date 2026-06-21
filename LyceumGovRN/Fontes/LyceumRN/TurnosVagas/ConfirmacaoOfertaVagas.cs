using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class ConfirmacaoOfertaVagas
    {
        public void Insere(DataContext contexto, Entidades.ConfirmacaoOfertaVagas confirmacaoOfertaVagas)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO TurnosVagas.CONFIRMACAOOFERTAVAGAS
                                           (CONFIRMACAOOFERTAID
                                           ,CURSO
                                           ,SERIE
                                           ,VAGASMANHA
                                           ,VAGASTARDE
                                           ,VAGASNOITE
                                           ,VAGASINTEGRAL
                                           ,QUANTIDADEOPTANTES)
                                     VALUES
                                           (@CONFIRMACAOOFERTAID, 
                                           @CURSO, 
                                           @SERIE, 
                                           @VAGASMANHA, 
                                           @VAGASTARDE, 
                                           @VAGASNOITE, 
                                           @VAGASINTEGRAL, 
                                           @QUANTIDADEOPTANTES) ";

            contextQuery.Parameters.Add("@CONFIRMACAOOFERTAID", SqlDbType.Int, confirmacaoOfertaVagas.ConfirmacaoOfertaId);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, confirmacaoOfertaVagas.Curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, confirmacaoOfertaVagas.Serie);
            contextQuery.Parameters.Add("@VAGASMANHA", SqlDbType.Int, confirmacaoOfertaVagas.VagasManha);
            contextQuery.Parameters.Add("@VAGASTARDE", SqlDbType.Int, confirmacaoOfertaVagas.VagasTarde);
            contextQuery.Parameters.Add("@VAGASNOITE", SqlDbType.Int, confirmacaoOfertaVagas.VagasNoite);
            contextQuery.Parameters.Add("@VAGASINTEGRAL", SqlDbType.Int, confirmacaoOfertaVagas.VagasIntegral);
            contextQuery.Parameters.Add("@QUANTIDADEOPTANTES", SqlDbType.Int, confirmacaoOfertaVagas.QuantidadeOptantes);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemoveTodos(DataContext contexto, int confirmacaoOfertaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE TurnosVagas.CONFIRMACAOOFERTAVAGAS
                                      WHERE CONFIRMACAOOFERTAID = @CONFIRMACAOOFERTAID ";

            contextQuery.Parameters.Add("@CONFIRMACAOOFERTAID", SqlDbType.Int, confirmacaoOfertaId);        

            contexto.ApplyModifications(contextQuery);
        }
    }
}
