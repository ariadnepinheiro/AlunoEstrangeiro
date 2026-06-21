using System;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.UnidadeEnsinoCompartilhada
{
    public class Aluno_UnidadeEnsinoCompartilhada : RNBase
    {
        public static void InsereAluno_UnidadeEnsinoCompartilhada(Entidades.Aluno_UnidadeEnsinoCompartilhada alunoUnidade, DataContext context)
        {
            if ((string.IsNullOrEmpty(alunoUnidade.AlunoId)) || (string.IsNullOrEmpty(alunoUnidade.UnidadeEnsinoCompartilhadaId)))
            {
                return;
            }

            ContextQuery contextQuery = new ContextQuery
            {
                Command = string.Format(@"
                    INSERT INTO [Matricula].[aluno_unidadeensinocompartilhada] 
                                ([alunoid], 
                                 [unidadeensinocompartilhadaid]) 
                    VALUES      ('{0}', 
                                 '{1}') "
                , alunoUnidade.AlunoId
                , alunoUnidade.UnidadeEnsinoCompartilhadaId)
            };

            context.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE  FROM Matricula.ALUNO_UNIDADEENSINOCOMPARTILHADA
                                      WHERE   ALUNOID = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contexto.ApplyModifications(contextQuery);
        }


        public bool PossuiAlunoUnidadeCompartilhadaPor(DataContext contexto, int id)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                 FROM    Matricula.ALUNO_UNIDADEENSINOCOMPARTILHADA 
                    WHERE   UNIDADEENSINOCOMPARTILHADAID = @ID_COMPARTILHADA ";

            contextQuery.Parameters.Add("@ID_COMPARTILHADA", id);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}