using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.GestaoRede
{
    public class RegiaoFinanceiraMunicipio
    {
        public bool PossuiRegiaoFinanceiraCadastradaPor(DataContext ctx, int regiaoFinanceiraId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM GestaoRede.REGIAOFINANCEIRAMUNICIPIO (NOLOCK)
                                        WHERE REGIAOFINANCEIRAID = @REGIAOFINANCEIRAID ";

            contextQuery.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.Int, regiaoFinanceiraId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}