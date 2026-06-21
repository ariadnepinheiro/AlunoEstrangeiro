using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class DocenteCandidatoExperiencia : RNBase
    {
        public static decimal ConsultarPontuacaoExperienciaCandidato(string candidato, string concurso)
        {
            return ExecutarFuncaoDec(" SELECT CT.PONTUACAO as pontuacao FROM [RECURSOSHUMANOS].[DOCENTECANDIDATOEXPERIENCIA] DT INNER JOIN [DBO].[LY_CONCURSO_DOC_EXPERIENCIA] CT ON DT.CONCURSO = CT.CONCURSO AND DT.EXPERIENCIA = CT.EXPERIENCIA INNER JOIN [LY_CONCURSO_EXPERIENCIA] T ON CT.EXPERIENCIA = T.EXPERIENCIA where DT.concurso = ? and dt.DOCENTECANDIDATOID=?", concurso, candidato);
        }

        public void Insere(DataContext contexto, Entidades.DocenteCandidatoExperiencia docenteCandidatoExperiencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA
                                           (DOCENTECANDIDATOID
                                           ,CONCURSO
                                           ,EXPERIENCIA
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@DOCENTECANDIDATOID
                                           ,@CONCURSO
                                           ,@EXPERIENCIA
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO) ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoExperiencia.DocenteCandidatoId);
            contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, docenteCandidatoExperiencia.Concurso);
            contextQuery.Parameters.Add("@EXPERIENCIA", SqlDbType.VarChar, docenteCandidatoExperiencia.Experiencia);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, docenteCandidatoExperiencia.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(DataContext contexto, int docenteCandidatoId, string concurso, string experiencia, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA
                                             SET EXPERIENCIA = @EXPERIENCIA,
                                                 USUARIOID = @USUARIOID,
                                                 DATAALTERACAO = @DATAALTERACAO
                                            WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID
                                                 AND CONCURSO = @CONCURSO ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);
            contextQuery.Parameters.Add("@EXPERIENCIA", SqlDbType.VarChar, experiencia);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int docenteCandidatoId, string concurso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  DELETE RecursosHumanos.DOCENTECANDIDATOEXPERIENCIA                                           
                                            WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID
                                                 AND CONCURSO = @CONCURSO ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);
         

            contexto.ApplyModifications(contextQuery);
        }
    }
}
