using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Techne.Web.Hades
{
    internal class Transacao
    {
        internal static Dictionary<string, HdTransacao> GetTransacao(string sistema)
        {
            var dic = new Dictionary<string, HdTransacao>();
            DataTable dtTransacao = null;
            DataTable dtPadTrans = null;

            var sqlTransacao = "select t.SIS,t.TRANS,t.PAGINAWEB,s.URL as SISURL,t.publica,t.acessoanonimo " +
                               "from hd_transacao t (NOLOCK) " +
                               "left join HD_SISTEMA s (NOLOCK) on t.SIS=s.SIS " +
                               "where t.sis=? order by t.TRANS ";
            dtTransacao = HadesUtil.ExecuteQuery(sqlTransacao, new object[] { sistema });

            var sqlPadTrans = "select pt.SIS,pt.TRANS,pt.PADACES,pt.PODEALT,pt.PODECAD,pt.PODEREM from HD_PADTRANS pt (NOLOCK) " +
                              "where pt.SIS=? ";
            dtPadTrans = HadesUtil.ExecuteQuery(sqlPadTrans, new object[] { sistema });

            foreach (DataRow r in dtTransacao.Rows)
            {
                var trans = new HdTransacao();
                trans.Sistema = r["SIS"] as string;
                trans.Trans = r["TRANS"] as string;
                trans.Url = r["PAGINAWEB"] as string;
                trans.SisUrl = r["SISURL"] as string;

                var roles = new HashSet<string>();
                roles.Add("PRIVILEGIADO");
                if ("S".Equals(string.Empty + r["PUBLICA"], StringComparison.InvariantCultureIgnoreCase))
                {
                    roles.Add("*");
                }

                if ("S".Equals(string.Empty + r["ACESSOANONIMO"], StringComparison.InvariantCultureIgnoreCase))
                {
                    roles.Add("?");
                }

                var dvPadTrans = new DataView(dtPadTrans);
                dvPadTrans.RowFilter = "sis='" + trans.Sistema + "' and trans='" + trans.Trans + "'";
                foreach (DataRowView rr in dvPadTrans)
                {
                    var padaces = rr["PADACES"] as string;
                    if (!string.IsNullOrEmpty(padaces) && !roles.Contains(padaces))
                    {
                        roles.Add(padaces);
                    }
                }

                trans.Roles = roles.ToArray();

                if (!dic.ContainsKey(trans.Url.ToLower().Trim()))
                {
                    dic.Add(trans.Url.ToLower().Trim(), trans);
                }
            }

            return dic;
        }
    }
}