using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.CartaoEstudante
{
    public class Solicitacao
    {
        public bool ExistePor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @"  SELECT COUNT(*) 
                                FROM CartaoEstudante.SOLICITACAO (NOLOCK)
                                WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }
    }
}
