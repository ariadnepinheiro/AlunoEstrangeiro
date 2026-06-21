
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class TipoSolicitacaoQuery: QueryBase<TipoSolicitacaoQuery>
    {
        TipoSolicitacaoQuery() {
            NomeTabela = "Lyceum.CartaoEstudante.TipoSolicitacao";
        }
    }
}
