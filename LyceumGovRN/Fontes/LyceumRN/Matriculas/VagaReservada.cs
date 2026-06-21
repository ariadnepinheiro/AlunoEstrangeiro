using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class VagaReservada
    {
        public void Insere(DataContext contexto, Entidades.VagaReservada vagaReservada)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO MATRICULA.VAGARESERVADA 
                                            (ALUNO, 
                                             CONTROLEVAGAID, 
                                             DATAINICIO, 
                                             DATAFIM, 
                                             USUARIOID) 
                                VALUES      (@ALUNO, 
                                             @CONTROLEVAGAID, 
                                             @DATAINICIO, 
                                             @DATAFIM, 
                                             @USUARIOID)  ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, vagaReservada.Aluno);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, vagaReservada.ControleVagaId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, vagaReservada.DataInicio);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, vagaReservada.DataFim);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, vagaReservada.UsuarioId);

            contexto.ApplyModifications(contextQuery);     
        }

        public void Insere(Entidades.VagaReservada vagaReservada, List<ContextQuery> listaContextQuery)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO MATRICULA.VAGARESERVADA 
                                            (ALUNO, 
                                             CONTROLEVAGAID, 
                                             DATAINICIO, 
                                             DATAFIM, 
                                             USUARIOID) 
                                VALUES      (@ALUNO, 
                                             @CONTROLEVAGAID, 
                                             @DATAINICIO, 
                                             @DATAFIM, 
                                             @USUARIOID)  ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, vagaReservada.Aluno);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, vagaReservada.ControleVagaId);
            contextQuery.Parameters.Add("@DATAINICIO", SqlDbType.DateTime, vagaReservada.DataInicio);
            contextQuery.Parameters.Add("@DATAFIM", SqlDbType.DateTime, vagaReservada.DataFim);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, vagaReservada.UsuarioId);

            listaContextQuery.Add(contextQuery);
        }
    }
}
