using System;
using System.Collections.Generic;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class DocumentoAluno: RNBase
    {
        public static DataTable Listar(string aluno, decimal ano, decimal periodo)
        {
            var contextQuery = new ContextQuery
                        (@"   SELECT    ID_DOCUMENTO_ALUNO,
                                D.ID_DOCUMENTO,
                                ALUNO,
                                DT_ENTREGA,
                                DA.DT_CADASTRO,
                                DA.DT_ALTERACAO,
                                DA.MATRICULA,
                                D.DESCRICAO,
                                D.OBRIGATORIO,
                                DATEADD(DAY,D.PRAZO,GETDATE()) AS PRAZO,
                                CASE WHEN DT_ENTREGA IS NULL THEN '0'
                                ELSE '1' END ENTREGA
                      FROM    TCE_DOCUMENTO   D
                                LEFT JOIN dbo.TCE_DOCUMENTO_ALUNO DA ON DA.ID_DOCUMENTO = D.ID_DOCUMENTO AND ALUNO = @ALUNO 
                                WHERE ANO = @ANO 
                                AND PERIODO = @PERIODO "
                            );
            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            return Consultar(contextQuery);
        }

        public static void SalvarDocumentos(ICollection<TceDocumentoAluno> documentosAlunos, string aluno)
        {
            if (string.IsNullOrEmpty(aluno))
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //Deletar os antigos
                    Remover(aluno, ctx);

                    // Inserir os novas
                    if (documentosAlunos != null)
                    {
                        foreach (var documentoAluno in documentosAlunos)
                        {
                            Inserir(documentoAluno, ctx);
                        }
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        private static void Inserir(TceDocumentoAluno documentoAluno, DataContext ctx)
        {

            var contextQuery = new ContextQuery(
               @"INSERT INTO dbo.TCE_DOCUMENTO_ALUNO ( ID_DOCUMENTO, ALUNO, DT_ENTREGA,
                                                           MATRICULA)
                    VALUES  ( @ID_DOCUMENTO, @ALUNO, @DT_ENTREGA, @MATRICULA)");

            contextQuery.Parameters.Add("@ID_DOCUMENTO", documentoAluno.IdDocumento);
            contextQuery.Parameters.Add("@ALUNO", documentoAluno.Aluno);
            contextQuery.Parameters.Add("@DT_ENTREGA", documentoAluno.DtEntrega);
            contextQuery.Parameters.Add("@MATRICULA", documentoAluno.Matricula);

            ctx.ApplyModifications(contextQuery);

        }
    
        private static void Remover(string aluno, DataContext context)
        {
            var contextQuery = new ContextQuery(
               @"DELETE  FROM TCE_DOCUMENTO_ALUNO
                    WHERE   ALUNO = @ALUNO");

            contextQuery.Parameters.Add("@ALUNO", aluno);

            context.ApplyModifications(contextQuery);

        }
    }
}
