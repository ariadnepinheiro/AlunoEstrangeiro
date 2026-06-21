using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class NivelCurso : RNBase
    {
        public static QueryTable ObterDetalhe()
        {
            TConnectionWritable tconnw = Config.CreateWritableConnection();
            QueryTable qtDetalhe = null;

            try
            {
                tconnw.Open(true);

                qtDetalhe = new QueryTable(
                    " SELECT ITEM, " +
                        " DESCR AS DESCRICAO " +
                    " FROM ITEMTABELA " +
                    " WHERE TAB = 'DetalheTipoCurso' " +
                    " ORDER BY DESCR ");
                qtDetalhe.Query(tconnw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tconnw.Close();
            }
            return qtDetalhe;
        }
    }
}
