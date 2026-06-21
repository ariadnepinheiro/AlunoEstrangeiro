using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class DocenteCandidatoTitulacao : RNBase
    {
        public static decimal ConsultarPontuacaoTitulacaoCandidato(string candidato, string concurso)
        {
            return ExecutarFuncaoDec("SELECT CT.PONTUACAO as pontuacao FROM [RECURSOSHUMANOS].[DOCENTECANDIDATOTITULACAO] DT INNER JOIN [DBO].[LY_CONCURSO_DOC_TITULACOES] CT ON DT.CONCURSO = CT.CONCURSO AND DT.TITULACAO = CT.TITULACAO INNER JOIN [LY_CONCURSO_TITULACAO] T ON CT.TITULACAO = T.TITULACAO WHERE  DT.CONCURSO =  ? AND DOCENTECANDIDATOID =?", concurso, candidato);
        }

        public void Insere(DataContext contexto, Entidades.DocenteCandidatoTitulacao docenteCandidatoTitulacao)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  INSERT INTO RecursosHumanos.DOCENTECANDIDATOTITULACAO
                                           (DOCENTECANDIDATOID
                                           ,CONCURSO
                                           ,TITULACAO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@DOCENTECANDIDATOID
                                           ,@CONCURSO
                                           ,@TITULACAO
                                           ,@USUARIOID
                                           ,@DATACADASTRO
                                           ,@DATAALTERACAO) ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoTitulacao.DocenteCandidatoId);
            contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, docenteCandidatoTitulacao.Concurso);
            contextQuery.Parameters.Add("@TITULACAO", SqlDbType.VarChar, docenteCandidatoTitulacao.Titulacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, docenteCandidatoTitulacao.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(DataContext contexto, int docenteCandidatoId, string concurso, string titulacao, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  UPDATE RecursosHumanos.DOCENTECANDIDATOTITULACAO
                                             SET TITULACAO = @TITULACAO,
                                                 USUARIOID = @USUARIOID,
                                                 DATAALTERACAO = @DATAALTERACAO
                                            WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID
                                                 AND CONCURSO = @CONCURSO ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);
            contextQuery.Parameters.Add("@TITULACAO", SqlDbType.VarChar, titulacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, int docenteCandidatoId, string concurso)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  DELETE RecursosHumanos.DOCENTECANDIDATOTITULACAO                                            
                                            WHERE DOCENTECANDIDATOID = @DOCENTECANDIDATOID
                                                 AND CONCURSO = @CONCURSO ";

            contextQuery.Parameters.Add("@DOCENTECANDIDATOID", SqlDbType.Int, docenteCandidatoId);
            contextQuery.Parameters.Add("@CONCURSO", SqlDbType.VarChar, concurso);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
