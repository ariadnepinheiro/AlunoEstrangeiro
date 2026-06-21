using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Techne.Web.Hades
{
    public class Menu
    {
        internal static HdMenu GetMenu(string menuName)
        {
            var mnu = new HdMenu();
            DataTable dtMenuItem = null;
            DataTable dtPadTrans = null;
            DataTable dtMenu = null;

            // busca menu
            dtMenu = HadesUtil.ExecuteQuery("Select menu,sis,descricao,tipo from hd_menu (NOLOCK) where menu=?", new object[] { menuName });

// se não encontrar, sai com menu vazio
            if (dtMenu == null || dtMenu.Rows.Count == 0)
            {
                mnu.NomeMenu = menuName;
                mnu.Sistema = string.Empty;
                mnu.Descricao = string.Empty;
                return mnu;
            }

            mnu.NomeMenu = dtMenu.Rows[0]["menu"].ToString();
            mnu.Sistema = dtMenu.Rows[0]["sis"].ToString();
            mnu.Descricao = dtMenu.Rows[0]["descricao"].ToString();

            var sqlMenuItem = "select m.MENU,m.MENUITEM,m.MENUITEMPAI,m.SIS,m.TRANS,m.ORDEM,m.TEXTO,s.URL as SISURL,t.PAGINAWEB, m.IMAGEM " +
                              "from hd_menuitem m (NOLOCK) " +
                              "left join hd_transacao t (NOLOCK) on m.Sis=t.sis and m.trans=t.trans " +
                              "left join HD_SISTEMA s (NOLOCK) on t.SIS=s.SIS " +
                              "where menu=? order by isnull(m.MENUITEMPAI,-1),m.ORDEM";
            try
            {
                dtMenuItem = HadesUtil.ExecuteQuery(sqlMenuItem, new object[] { menuName });
            }
            catch
            {
                dtMenuItem = null;
            }

            if (dtMenuItem == null)
            {
                sqlMenuItem = "select m.MENU,m.MENUITEM,m.MENUITEMPAI,m.SIS,m.TRANS,m.ORDEM,m.TEXTO,s.URL as SISURL,t.PAGINAWEB, null as IMAGEM " +
                              "from hd_menuitem m (NOLOCK) " +
                              "left join hd_transacao t (NOLOCK) on m.Sis=t.sis and m.trans=t.trans " +
                              "left join HD_SISTEMA s (NOLOCK) on t.SIS=s.SIS " +
                              "where menu=? order by isnull(m.MENUITEMPAI,-1),m.ORDEM";
                dtMenuItem = HadesUtil.ExecuteQuery(sqlMenuItem, new object[] { menuName });
            }

            var sqlPadTrans = "select pt.SIS,pt.TRANS,pt.PADACES,pt.PODEALT,pt.PODECAD,pt.PODEREM from HD_PADTRANS pt (NOLOCK) " +
                              "inner join hd_menuitem m (NOLOCK) on m.Sis=pt.sis and m.trans=pt.trans " +
                              "where m.MENU=? ";
            dtPadTrans = HadesUtil.ExecuteQuery(sqlPadTrans, new object[] { menuName });

            foreach (DataRow r in dtMenuItem.Rows)
            {
                var mi = new HdMenuItem();
                var id = r["MENUITEM"] as decimal?;
                var parentid = r["MENUITEMPAI"] as decimal?;
                var itemMenuName = string.Empty + r["MENU"];
                mi.ID = itemMenuName + (id == null ? string.Empty : ":" + id);
                mi.ParentID = itemMenuName + (parentid == null ? null : ":" + parentid);
                mi.Sistema = r["SIS"] as string;
                mi.Trans = r["TRANS"] as string;
                mi.Texto = r["TEXTO"] as string;
                mi.Url = r["PAGINAWEB"] as string;
                mi.SisUrl = r["SISURL"] as string;
                mi.ImageUrl = r["IMAGEM"] as string;

                var roles = new HashSet<string>();
                roles.Add("PRIVILEGIADO");
                GetRoles(roles, itemMenuName, id, dtMenuItem, dtPadTrans);
                mi.Roles = roles.ToArray();

                mnu.Items.Add(mi);
            }

            return mnu;
        }

        private static void GetRoles(HashSet<string> roles, string menuName, decimal? menuID, DataTable dtMenuItem, DataTable dtPadTrans)
        {
            if (menuID == null)
            {
                return;
            }

            var dvMenuItem = new DataView(dtMenuItem);
            dvMenuItem.RowFilter = "Menu='" + menuName + "' and menuitem=" + menuID;
            if (dvMenuItem.Count == 0)
            {
                return;
            }

            var sis = dvMenuItem[0]["sis"] as string;
            var trans = dvMenuItem[0]["trans"] as string;
            var padaces = string.Empty;

            if (!string.IsNullOrEmpty(sis) && !string.IsNullOrEmpty(trans))
            {
                var dvPadTrans = new DataView(dtPadTrans);
                dvPadTrans.RowFilter = "sis='" + sis + "' and trans='" + trans + "'";
                foreach (DataRowView rr in dvPadTrans)
                {
                    padaces = rr["PADACES"] as string;
                    if (!string.IsNullOrEmpty(padaces) && !roles.Contains(padaces))
                    {
                        roles.Add(padaces);
                    }
                }
            }
            else
            {
// se o item de menu não tem url associada, o padrão de acesso dele é a união dos padrões de acesso dos filhos (e netos)
                dvMenuItem.RowFilter = "Menu='" + menuName + "' and MENUITEMPAI=" + menuID;
                foreach (DataRowView mnu in dvMenuItem)
                {
                    var childID = mnu["MENUITEM"] is decimal ? (decimal?)(decimal)mnu["MENUITEM"] : null;
                    GetRoles(roles, menuName, childID, dtMenuItem, dtPadTrans);
                }
            }
        }
    }
}