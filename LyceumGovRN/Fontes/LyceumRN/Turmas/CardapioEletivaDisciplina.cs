using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.Turmas
{
    public class CardapioEletivaDisciplina
    {
        public List<Entidades.CardapioEletivaDisciplina> ObtemListaPor(DataContext contexto, int cardapioEletivaId)
        {
            ICollection<Entidades.CardapioEletivaDisciplina> lista = new List<Entidades.CardapioEletivaDisciplina>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT CARDAPIOELETIVADISCIPLINAID, 
		                                        CARDAPIOELETIVAID, 
		                                        TURNO, 
		                                        DISCIPLINA, 
		                                        USUARIOID, 
		                                        DATACADASTRO, 
		                                        DATAALTERACAO
                                        FROM Turma.CARDAPIOELETIVADISCIPLINA
                                        WHERE CARDAPIOELETIVAID = @CARDAPIOELETIVAID ";

            contextQuery.Parameters.Add("@CARDAPIOELETIVAID", SqlDbType.Int, cardapioEletivaId);

            lista = contexto.TryToBindEntities<Entidades.CardapioEletivaDisciplina>(contextQuery);

            return lista.ToList();
        }

        public void Insere(DataContext contexto, int cardapioEletivaId, string turno, string disciplina, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Turma.CARDAPIOELETIVADISCIPLINA
                                           (CARDAPIOELETIVAID
                                           ,TURNO
                                           ,DISCIPLINA
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@CARDAPIOELETIVAID, 
                                           @TURNO, 
                                           @DISCIPLINA, 
                                           @USUARIOID,
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@CARDAPIOELETIVAID", SqlDbType.Int, cardapioEletivaId);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemoveTodas(DataContext contexto, int cardapioEletivaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Turma.CARDAPIOELETIVADISCIPLINA
                                      WHERE CARDAPIOELETIVAID = @CARDAPIOELETIVAID ";

            contextQuery.Parameters.Add("@CARDAPIOELETIVAID", SqlDbType.Int, cardapioEletivaId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
