using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class LogImpressaoFichaMatricula
    {
        public static void Inserir(RN.Entidades.LogImpressaoFichaMatricula logImpressaoFichaMatricula)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"INSERT INTO LOGIMPRESSAOFICHAMATRICULA
                        (
                            ALUNOID, 
                            CONFIRMACAOMATRICULAID, 
                            MATRICULA
                        )
                        VALUES
                        (
                            @ALUNOID, 
                            @CONFIRMACAOMATRICULAID, 
                            @MATRICULA
                        ) "
                    };

                    contextQuery.Parameters.Add("@ALUNOID", logImpressaoFichaMatricula.AlunoId);
                    contextQuery.Parameters.Add("@CONFIRMACAOMATRICULAID", logImpressaoFichaMatricula.ConfirmacaoMatriculaId);
                    contextQuery.Parameters.Add("@MATRICULA", logImpressaoFichaMatricula.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }
    }
}
