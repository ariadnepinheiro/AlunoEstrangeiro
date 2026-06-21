using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class EncaminhamentoEspecial
    {
        public void Insere(DataContext contexto, int motivoEncaminhamentoEspecialId, string observacao, int confirmacaoMatriculaId, int controleVagaId, string aluno, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.ENCAMINHAMENTOESPECIAL 
                                                (CONFIRMACAOMATRICULAID, 
                                                 CONTROLEVAGAID, 
                                                 MOTIVOENCAMINHAMENTOESPECIALID,
                                                 ALUNO, 
                                                 OBSERVACAO,
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    VALUES      (@CONFIRMACAOMATRICULAID, 
                                                 @CONTROLEVAGAID, 
                                                 @MOTIVOENCAMINHAMENTOESPECIALID,
                                                 @ALUNO, 
                                                 @OBSERVACAO,
                                                 @USUARIOID, 
                                                 @DATACADASTRO, 
                                                 @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@CONFIRMACAOMATRICULAID", SqlDbType.Int, confirmacaoMatriculaId);
            contextQuery.Parameters.Add("@CONTROLEVAGAID", SqlDbType.Int, controleVagaId);
            contextQuery.Parameters.Add("@MOTIVOENCAMINHAMENTOESPECIALID", SqlDbType.Int, motivoEncaminhamentoEspecialId);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@OBSERVACAO", SqlDbType.VarChar, observacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiMotivoPor(DataContext contexto, int motivoEncaminhamentoEspecialId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM MATRICULA.ENCAMINHAMENTOESPECIAL (NOLOCK)
                                    WHERE MOTIVOENCAMINHAMENTOESPECIALID = @MOTIVOENCAMINHAMENTOESPECIALID ";

            contextQuery.Parameters.Add("@MOTIVOENCAMINHAMENTOESPECIALID", SqlDbType.Int, motivoEncaminhamentoEspecialId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
