using System.Collections.Generic;
using System.Data;

namespace Techne.Web.Hades
{
    internal class Padaces
    {
        internal static string[] ListaPadaces()
        {
            var listPadaces = new List<string>();

            var sql = "select padaces from hd_padaces (NOLOCK) order by upper(padaces)";
            var dt = HadesUtil.ExecuteQuery(sql, new object[] { });
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["PADACES"] as string))
                    {
                        listPadaces.Add(dr["PADACES"] as string);
                    }
                }
            }

            return listPadaces.ToArray();
        }

        internal static bool PadacesValido(string padaces)
        {
            var sql = "select padaces from hd_padaces (NOLOCK) where pacades=?";
            var dt = HadesUtil.ExecuteQuery(sql, new object[] { padaces });
            return dt != null && dt.Rows.Count > 0;
        }
    }
}