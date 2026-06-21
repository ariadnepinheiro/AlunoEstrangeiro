using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Techne.Controls;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Techne.Lyceum.RN;
using System.Collections.Specialized;
using System.Collections;
using DevExpress.Web.ASPxTabControl;
using System.Reflection;
using System.Data;

namespace Techne.Lyceum.Net
{
    public enum NavigationKey
    {
        Enter=1,
        Backspace=2
    }

    public class TPage : Techne.TPage
    {
        private const string Connection_Def = "Lyceum";
        private NavigationKey _disabledNavigationKeys = NavigationKey.Enter | NavigationKey.Backspace;
        private NameValueCollection _queryStringDecodificada = null;
        protected bool UsaGoogleMapsAPI = false;
        protected readonly string UsuarioLogado;
        private readonly string transacao;

        public enum AcaoControle
        { 
            excluir,
            novo,
            editar
        }

        public TPage()
        {
            Connection = Connection_Def;
            this.SwitchPostback = false;
        }

         public TPage(Type transacao)
        {
            this.transacao = transacao.FullName;
            UsuarioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            Connection = Connection_Def;
            this.SwitchPostback = false;
        }

        [DefaultValue(Connection_Def),]
        public override string Connection
        {
            get { return base.Connection; }
            set { base.Connection = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ClientScript.IsClientScriptBlockRegistered(typeof(TPage), "TPageLyceum"))
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(TPage), "TPageLyceum",
                  "<script language=\"JavaScript\" " +
                          "src=\"" + TUtil.TranslateRelativeUrl("~/Scripts/TPageLyceum.js", this) + "\">" +
                  "</script>\r\n"
                );
            }

            RegisterDisabledNavigationKeys(this, this.DisabledNavigationKeys);

            if (UsaGoogleMapsAPI)
                RegisterGoogleMapsAPI(base.Page);
        }

        [DefaultValue(NavigationKey.Enter | NavigationKey.Backspace)]
        public NavigationKey DisabledNavigationKeys
        {
            get { return _disabledNavigationKeys; }
            set { _disabledNavigationKeys = value; }
        }

        public static void RegisterDisabledNavigationKeys(Page page,NavigationKey disabledKeys)
        {
            StringBuilder strScript = new StringBuilder();
            if (!page.ClientScript.IsStartupScriptRegistered(typeof(Page),"RegisterDisabledNavigationKeys"))
            {
                strScript.AppendLine("<script language=\"JavaScript\">");
                strScript.AppendLine("if(typeof disableNavigationKeys == 'function') {");
                strScript.AppendLine(" disableNavigationKeys(" + ((int)disabledKeys).ToString()+");");
                strScript.AppendLine("}");
                strScript.AppendLine("</script>");
                page.ClientScript.RegisterStartupScript(typeof(Page), "RegisterDisabledNavigationKeys", strScript.ToString());
            }
        }

        //Verifica todos os controles das páginas aspx
        private void ListControlCollections()
        {
            ArrayList controlList = new ArrayList();
            AddControls(Page.Controls, controlList);            
        }

        private void AddControls(ControlCollection page, ArrayList controlList)
        {
            foreach (Control c in page)
            {
                if (c.ID != null)
                {
                    controlList.Add(c.ID);
                }

                if (c.HasControls())
                {
                    AddControls(c.Controls, controlList);
                }
            }
        }

        public void HabilitaCamposPeloPerfil(TabPage tela, string pagina)
        {
            //Verifica o tipo de perfil logado no sistema  
            var dtPerfil = Perfil.ListarComponentesDoPerfil(User.Identity.Name, pagina);
            String stPerfis = User.Identity.Name;
            Session["Perfil"] = stPerfis;
            for (int i = 0; i < dtPerfil.Rows.Count; i++)
            {
                Control c = FindControl(dtPerfil.Rows[i]["COMPONENTE"].ToString());

                if (tela.Controls.Contains(c))
                {   
                    Type t = c.GetType();
                    t.GetProperty("Enabled").SetValue(c, (Boolean)dtPerfil.Rows[i]["HABILITAR"], null);

                    if (t == typeof(TextBox))
                    {
                        ((TextBox)c).ReadOnly = (!(Boolean)dtPerfil.Rows[i]["HABILITAR"]);
                    }
                    else if (t == typeof(Techne.Web.TSearchBox))
                    {
                        ((Techne.Web.TSearchBox)c).Mode = ControlMode.Edit;
                    }
                    else if (t == typeof(Techne.Web.TSearch))
                    {
                        ((Techne.Web.TSearch)c).ShowButton = (Boolean)dtPerfil.Rows[i]["HABILITAR"];
                        ((Techne.Web.TSearch)c).ReadOnly = (!(Boolean)dtPerfil.Rows[i]["HABILITAR"]);
                    }
                }
            }
        }

        public void ControlaAcesso(Control control)
        {
            if (control is ASPxGridView)
            {
                ASPxGridView gv = (ASPxGridView)control;

                foreach (GridViewColumn col in gv.Columns)
                {
                    if (col is GridViewCommandColumn)
                    {
                        if (((GridViewCommandColumn)col).EditButton.Visible)
                            ((GridViewCommandColumn)col).EditButton.Visible = Permission.AllowUpdate;
                        if (((GridViewCommandColumn)col).DeleteButton.Visible)
                            ((GridViewCommandColumn)col).DeleteButton.Visible = Permission.AllowDelete;
                        if (((GridViewCommandColumn)col).NewButton.Visible)
                            ((GridViewCommandColumn)col).NewButton.Visible = Permission.AllowInsert;
                        break;
                    }
                    //Isto corrige a posição dos botões na grid para o Firefox.
                    col.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }

                if (gv != null)
                {
                    HtmlImage img = (HtmlImage)gv.FindHeaderTemplateControl(gv.Columns[""], "btnNovoGrid");

                    if (img != null)
                    {
                        img.Visible = Permission.AllowInsert;
                    }
                }
            }
        }

        protected void ControlaAcesso(ASPxGridView grid, AcaoControle ac, string customButtons)
        {
            foreach (GridViewColumn col in grid.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (ac == AcaoControle.excluir)
                    {
                        if (((GridViewCommandColumn)col).CustomButtons[customButtons] != null && ((GridViewCommandColumn)col).CustomButtons[customButtons].Visibility == GridViewCustomButtonVisibility.AllDataRows)
                            ((GridViewCommandColumn)col).CustomButtons[customButtons].Visibility = Permission.AllowDelete ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                    }
                    else if (ac == AcaoControle.novo)
                    {
                        if (((GridViewCommandColumn)col).CustomButtons[customButtons] != null && ((GridViewCommandColumn)col).CustomButtons[customButtons].Visibility == GridViewCustomButtonVisibility.AllDataRows)
                            ((GridViewCommandColumn)col).CustomButtons[customButtons].Visibility = Permission.AllowInsert ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                    }
                    else if (ac == AcaoControle.editar)
                    {
                        if (((GridViewCommandColumn)col).CustomButtons[customButtons] != null && ((GridViewCommandColumn)col).CustomButtons[customButtons].Visibility == GridViewCustomButtonVisibility.AllDataRows)
                            ((GridViewCommandColumn)col).CustomButtons[customButtons].Visibility = Permission.AllowUpdate ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;
                    }                  
                }
            }
        }

        private void gridPrestacaoContas_PreRender(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            HtmlImage img = null;

            //busca acesso permitido
            string acessoID = "acesso_" + grid.UniqueID;
            bool acessoPermitido = this.ViewState[acessoID] is bool ? (bool)this.ViewState[acessoID] : true;

            //Busca configuração original da grid e salva no viewstate da página
            bool[] botoesVisiveis = null;
            string viewStateID = "botoes_" + grid.UniqueID;
            if (this.ViewState[viewStateID] is bool[])
            {
                botoesVisiveis = (bool[])this.ViewState[viewStateID];
            }
            else
            {
                botoesVisiveis = new bool[] { false, false, false, false };
                foreach (GridViewColumn col in grid.Columns)
                {
                    if (col is GridViewCommandColumn)
                    {
                        if (((GridViewCommandColumn)col).EditButton.Visible)
                            botoesVisiveis[0] = true;
                        if (((GridViewCommandColumn)col).DeleteButton.Visible)
                            botoesVisiveis[1] = true;
                        if (((GridViewCommandColumn)col).NewButton.Visible)
                            botoesVisiveis[2] = true;
                        break;
                    }
                }
                img = (HtmlImage)grid.FindHeaderTemplateControl(grid.Columns[""], "btnNovoGrid");

                if (img != null)
                {
                    botoesVisiveis[3] = true;
                }

                this.ViewState[viewStateID] = botoesVisiveis;
            }
            //habilita/desabilita comandos
            foreach (GridViewColumn col in grid.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    GridViewCommandColumn cmdCol = (GridViewCommandColumn)col;
                    cmdCol.EditButton.Visible = botoesVisiveis[0] && Permission.AllowUpdate && acessoPermitido;
                    cmdCol.DeleteButton.Visible = botoesVisiveis[1] && Permission.AllowDelete && acessoPermitido;
                    cmdCol.NewButton.Visible = botoesVisiveis[2] && Permission.AllowInsert && acessoPermitido;
                    break;
                }
                //Isto corrige a posição dos botões na grid para o Firefox.
                col.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            }

            img = (HtmlImage)grid.FindHeaderTemplateControl(grid.Columns[""], "btnNovoGrid");

            if (img != null)
            {
                img.Visible = botoesVisiveis[3] && Permission.AllowInsert && acessoPermitido;
            }
        }        

        /// <summary>
        /// Método que recebe os 3 controles para restringir a visibilidade
        /// </summary>
        /// <param name="excluir">Controle de excluir</param>
        /// <param name="novo">Controle de novo</param>
        /// <param name="editar">Controle de editar</param>
        public void ControlaAcesso(Control control, AcaoControle ac)
        {
            if (ac == AcaoControle.excluir)
            {
                if (control.Visible)
                    control.Visible = Permission.AllowDelete;
            }
            else if (ac == AcaoControle.novo)
            {
                if (control.Visible)
                    control.Visible = Permission.AllowInsert;
            }
            else if (ac == AcaoControle.editar)
            {
                if (control.Visible)
                    control.Visible = Permission.AllowUpdate;
            }
        }

		public static void TituloGrid(ASPxGridView grid, string titulo)
		{
            if (grid == null)
                return;
			string tituloGrade = grid.SettingsText.Title;
			if (tituloGrade != string.Empty) grid.SettingsText.Title = tituloGrade.Replace("|Tabela:|", titulo);
		}

        public NameValueCollection QueryStringDecodificada
        {
            get
            {
                if (_queryStringDecodificada == null)
                {
                    NameValueCollection decod = new NameValueCollection();

                    string q=Request.QueryString["Chave"];
                    if (!string.IsNullOrEmpty(q))
                    {
                        try
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                            string chave = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            if (!string.IsNullOrEmpty(chave))
                            {
                                string[] listaDados = chave.Split('&');

                                foreach (string dados in listaDados)
                                {
                                    string[] par = dados.Split('=');
                                    if (par.Length > 0 && !string.IsNullOrEmpty(par[0]))
                                    {
                                        if (par.Length > 1 && !string.IsNullOrEmpty(par[1]))
                                            decod.Add(par[0], par[1]);
                                        else
                                            decod.Add(par[0], "");
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                    _queryStringDecodificada = decod;
                }
                return _queryStringDecodificada;
            }
        }

        protected string CodificaQueryString(NameValueCollection pares)
        {
            StringBuilder str=new StringBuilder();
            foreach(string key in pares.Keys)
            {
                if(str.Length>0)
                    str.Append("&");
                str.Append(key + "=" + pares[key]);
            }
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(str.ToString());
            return("Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        public static string CodificaQueryString(IDictionary<string,string> pares)
        {
            StringBuilder str = new StringBuilder();
            foreach (string key in pares.Keys)
            {
                if (str.Length > 0)
                    str.Append("&");
                str.Append(key + "=" + pares[key]);
            }
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(str.ToString());
            return ("Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        public static bool IsUrlAuthorized(string url)
        {
            return TechneAuthorization.IsUrlAuthorized(Techne.TUtil.TranslateRelativeUrl(url));
        }

        public void PreencherDadoCombo(DropDownList drop, string dado) // como preenche da QT
        {
            if (dado == null)
                drop.SelectedValue = "";
            else
            {
                if (drop.Items.FindByValue(dado) == null)
                {
                    string dadoinvalido = "<" + dado + ">";
                    ListItem li;
                    if (drop.Items.Count > 0) li = new ListItem(dadoinvalido, dado);
                    else li = new ListItem(dado, dado);
                    if (li.Value != "NULL")
                    {
                        drop.Items.Add(li);
                        drop.SelectedValue = dado;
                    }                  
                }
                else
                {
                    drop.SelectedValue = dado;
                }
            }
        }
        private void RegisterGoogleMapsAPI(Page page)
        {
            StringBuilder mapsScript = new StringBuilder();

            if (!page.ClientScript.IsStartupScriptRegistered(typeof(Page), "RegisterGoogleMapsAPI"))
            {
                //Busca chave
                string key = System.Configuration.ConfigurationSettings.AppSettings["GoogleMapsKey.conexao.educacao.rj.gov.br"];
                mapsScript.Append(string.Format(@"<script src='https://maps.googleapis.com/maps/api/js?key={0}'></script>", key));
                mapsScript.Append("<script src='../Scripts/mapLibrary.js' type='text/javascript'></script>");

                page.ClientScript.RegisterStartupScript(typeof(Page), "RegisterGoogleMapsAPI", mapsScript.ToString());
            }
        }
    }
}
