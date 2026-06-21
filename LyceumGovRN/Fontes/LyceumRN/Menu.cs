using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Techne.Data;
using System.Data;

namespace Techne.Lyceum.RN
{
  public class Menu
  {
    private MenuItemCollection _children=null;

    public Menu()
    {
      _children = new MenuItemCollection();
    }
    public MenuItemCollection Items
    {
      get { return _children; }
    }
  }

  public class MenuItem
  {
    private MenuItemCollection _children=null;

    public MenuItem()
    {
      _children = new MenuItemCollection();
    }
    public string Text{get;set;}
    public string Name { get; set; }
    public string NavigateUrl { get; set; }
    public bool BeginGroup { get; set; }
    public MenuItemCollection Items
    {
      get { return _children; }
    }
  }

  public class MenuItemCollection:System.Collections.CollectionBase
  {
    public int Add(MenuItem column)
    {
      return List.Add(column);
    }

    public MenuItem this[int index]
    {
      get { return (MenuItem)List[index]; }
      set
      {
        if (value == null) throw new ArgumentNullException();
        List[index] = value;
      }
    }
  }

    public class MenuHelper
    {
      public static Menu PrepareMenu(System.Web.HttpContext context, string[] sistemas)
      {
        QueryTable qt;
        string appName = "";
        Menu menu = new Menu();


        if (context == null)
          return menu;


        TechneHttpApplication tapp = context.ApplicationInstance as TechneHttpApplication;
        if (tapp != null)
          appName = tapp.ApplicationName;

        string user = context.User.Identity.Name;
        qt = RN.MenuHelper.CarregaMenu(user, appName, sistemas, null);


        ArrayList arrayListMenu = new ArrayList();

        arrayListMenu.Insert(0, "Básico");
        arrayListMenu.Insert(1, "Currículo");
        arrayListMenu.Insert(2, "Acadêmico");
        arrayListMenu.Insert(3, "Hades");

        foreach (SimpleRow r in qt.Rows)
        {
          if (!arrayListMenu.Contains(r["Menu"].ToString()))
            arrayListMenu.Add(r["Menu"].ToString());
        }
        menu.Items.Clear();

        for (int i = 0; i < arrayListMenu.Count; i++)
        {
          SimpleRow[] sr = qt.Select("Menu ='" + arrayListMenu[i].ToString() + "'", "Menu", DataViewRowState.OriginalRows);


          if (sr.Length > 0)
          {
            MenuItem subMenu = new MenuItem();
            subMenu.Text = arrayListMenu[i].ToString();
            menu.Items.Add(subMenu);

            foreach (SimpleRow linha in sr)
            {
              MenuItem item = new MenuItem();

              item.Text = linha["Nome"].ToString();
              item.Name = linha["Nome"].ToString();
              string sis = linha["sis"].ToString();
              string navUrl = linha["Link"].ToString();
              string sisUrl = linha["sisUrl"].ToString();
              if (sis != appName && !string.IsNullOrEmpty(sisUrl) && navUrl.Length > 0 && navUrl.Substring(0, 1) == "~")
                navUrl = sisUrl + navUrl.Substring(1);
              item.NavigateUrl = navUrl;
              subMenu.Items.Add(item);

              if (linha["Separador"].ToString() == "-" && subMenu.Items.Count > 0)
              {
                subMenu.Items[subMenu.Items.Count - 1].BeginGroup = true;
              }

            }
          }
        }

        return menu;
      }

      private static QueryTable CarregaMenu(string user, string sistema, string[] outrosSistemas, string[] modulos)
      {
        bool bFirst = true;
        List<DbObject> values = new List<DbObject>();

        System.Text.StringBuilder strSistemas = new StringBuilder();
        List<DbObject> sisValues = new List<DbObject>();
        System.Text.StringBuilder strModulos = new StringBuilder();
        List<DbObject> modValues = new List<DbObject>();

        //filtro de sistemas
        List<string> sistemas = new List<string>();
        if (!string.IsNullOrEmpty(sistema))
          sistemas.Add(sistema);
        if (outrosSistemas != null)
          sistemas.AddRange(outrosSistemas);
        if (sistemas.Count>0)
        {
          strSistemas.Append(" AND (");
          bFirst = true;
          foreach (string sis in sistemas)
          {
            if (bFirst)
              bFirst = false;
            else
              strSistemas.Append(" OR ");
            strSistemas.Append("htr.SIS = ? ");
            sisValues.Add(sis);
          }
          strSistemas.Append(") ");
        }

        //filtro de modulos
        if (modulos != null)
        {
          strModulos.Append(" AND (");
          bFirst = true;
          foreach (string modulo in modulos)
          {
            if (bFirst)
              bFirst = false;
            else
              strModulos.Append(" OR ");
            strModulos.Append("htr.Modulo = ? ");
            modValues.Add(modulo);
          }
          strModulos.Append(") ");
        }
        else
        {
          strModulos.Append(" AND (htr.Modulo is not null) ");
        }

        TConnection con = new TConnection(Techne.HadesLyc.Config.ConnectionString);

        System.Text.StringBuilder strQuery = new StringBuilder();
        strQuery.Append("SELECT " +
                         "PaginaWeb as Link, " +
                         "htr.Modulo as Menu, " +
                         "htr.itemMenu as Nome, " +
                         "isnull(htr.nome,'') Separador, " +
                         "sist.url as sisUrl, sist.sis as sis, " +
                         "htr.Form " +
                        "from " +
                        "usuario usu " +
                        "inner join padusuario pus on pus.usuario = usu.usuario " +
                        "inner join transpadaces trp on pus.padaces = trp.padaces " +
                        "inner join hd_transacao htr on trp.sis = htr.sis and trp.trans = htr.trans " +
                        "inner join hd_sistema sist on sist.sis = htr.sis " +
                        "where usu.usuario = ? ");
        values.Add(user);
        //sistemas
        strQuery.Append(strSistemas.ToString());
        values.AddRange(sisValues);
        //modulos
        strQuery.Append(strModulos.ToString());
        values.AddRange(modValues);

        strQuery.Append("UNION ");
        strQuery.Append("SELECT " +
                        " paginaWeb as Link, " +
                        " htr.Modulo as Menu, " +
                        " htr.itemMenu as Nome, " +
                        " isnull(htr.nome,'') Separador, " +
                        " sist.url as sisUrl, sist.sis as sis, " +
                        " htr.Form " +
                        " from " +
                        " hd_transacao htr " +
                        " inner join hd_sistema sist on sist.sis = htr.sis " +
                        " where " + 
                        " ? = isnull((SELECT usuario FROM usuario WHERE usuario = ? AND PRIVIL = 'S'),'') ");
        values.Add(user);
        values.Add(user);
        //sistemas
        strQuery.Append(strSistemas.ToString());
        values.AddRange(sisValues);
        //modulos
        strQuery.Append(strModulos.ToString());
        values.AddRange(modValues);

        strQuery.Append(" ORDER by Menu, Form ");

        QueryTable qt = new QueryTable(strQuery.ToString());
        DbObject[] dbobj = values.ToArray();
        qt.Query(con, dbobj);
        return qt;
      }


      private static QueryTable CarregaMenu(string user)
        {
            TConnection con = new TConnection(Techne.HadesLyc.Config.ConnectionString);

            QueryTable qt = new QueryTable("SELECT " + 
                             "PaginaWeb as Link, " +
                             "htr.Modulo as Menu, " +
                             "htr.itemMenu as Nome, " +
                             "isnull(htr.nome,'') Separador, " +
                             "htr.Form " +
                            "from " +
                            "usuario usu " +
                            "inner join padusuario pus on pus.usuario = usu.usuario " +
                            "inner join transpadaces trp on pus.padaces = trp.padaces " +
                            "inner join hd_transacao htr on trp.sis = htr.sis and trp.trans = htr.trans " +
                            "where " +
                            " (htr.Modulo = 'Básico' OR  htr.Modulo = 'Acadêmico' OR  htr.Modulo = 'Currículo' OR  htr.Modulo = 'Hades') " +
                            " AND usu.usuario = ? " +
                            " AND htr.SIS = 'LyceumNet' " +

                            "UNION " +

                            "SELECT " +
                            " paginaWeb as Link, " +
                            " htr.Modulo as Menu, " +
                            " htr.itemMenu as Nome, " +
                            " isnull(htr.nome,'') Separador, " +
                            " htr.Form " +
                            "from " +
                            " hd_transacao htr " +
                            "where " +
                            " (htr.Modulo = 'Básico' OR  htr.Modulo = 'Acadêmico' OR  htr.Modulo = 'Currículo' OR  htr.Modulo = 'Hades') " +
                            " AND ? = isnull((SELECT usuario FROM usuario WHERE usuario = ? AND PRIVIL = 'S'),'') " +
                            " AND SIS = 'LyceumNet' " + 
                            " ORDER by Menu, Form ");

            DbObject[] dbobj = new DbObject[]{user, user, user};
            qt.Query(con,dbobj);
            return qt;
        }
    }
}
