using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class TrilhaAprendizagemAluno
    {
        public bool PossuiTrilhaAprendizagemEscolaPor(DataContext ctx, string censo, string curso, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[TRILHAAPRENDIZAGEM_ALUNO] (NOLOCK)
                                WHERE CENSO = @CENSO
                                    AND CURSOTRILHA = @CURSOTRILHA
                                    AND ANOOFERTA = @ANO ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@CURSOTRILHA", SqlDbType.VarChar, curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
