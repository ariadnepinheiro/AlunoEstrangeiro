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
using System.Collections.Specialized;

namespace Techne.Lyceum.Net
{
    public enum NavigationKey
    {
        Enter = 1,
        Backspace = 2
    }

    public class TPage : Techne.TPage
    {
        private const string Connection_Def = "Lyceum";
        private NavigationKey _disabledNavigationKeys = NavigationKey.Enter | NavigationKey.Backspace;
        private NameValueCollection _queryStringDecodificada = null;

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

        [
          DefaultValue(Connection_Def),
        ]
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
        }

        [DefaultValue(NavigationKey.Enter | NavigationKey.Backspace)]
        public NavigationKey DisabledNavigationKeys
        {
            get { return _disabledNavigationKeys; }
            set { _disabledNavigationKeys = value; }
        }

        public override string ApplicationName
        {
            get
            {
                return "LyceumNet";
            }
        }

        public static void RegisterDisabledNavigationKeys(Page page, NavigationKey disabledKeys)
        {
            StringBuilder strScript = new StringBuilder();
            if (!page.ClientScript.IsStartupScriptRegistered(typeof(Page), "RegisterDisabledNavigationKeys"))
            {
                strScript.AppendLine("<script language=\"JavaScript\">");
                strScript.AppendLine("if(typeof disableNavigationKeys == 'function') {");
                strScript.AppendLine(" disableNavigationKeys(" + ((int)disabledKeys).ToString() + ");");
                strScript.AppendLine("}");
                strScript.AppendLine("</script>");
                page.ClientScript.RegisterStartupScript(typeof(Page), "RegisterDisabledNavigationKeys", strScript.ToString());
            }
        }

        public void ControlaAcesso(Control control)
        {
            //if (control is ASPxGridView)
            //{
            //    ASPxGridView gv = (ASPxGridView)control;

            //    foreach (GridViewColumn col in gv.Columns)
            //    {

            //        if (col is GridViewCommandColumn)
            //        {
            //            if (((GridViewCommandColumn)col).EditButton.Visible)
            //                ((GridViewCommandColumn)col).EditButton.Visible = Permission.AllowUpdate;
            //            if (((GridViewCommandColumn)col).DeleteButton.Visible)
            //                ((GridViewCommandColumn)col).DeleteButton.Visible = Permission.AllowDelete;
            //            if (((GridViewCommandColumn)col).NewButton.Visible)
            //                ((GridViewCommandColumn)col).NewButton.Visible = Permission.AllowInsert;
            //            break;
            //        }
            //        //Isto corrige a posição dos botões na grid para o Firefox.
            //        col.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            //    }

            //    if (gv != null)
            //    {
            //        HtmlImage img = (HtmlImage)gv.FindHeaderTemplateControl(gv.Columns[""], "btnNovoGrid");

            //        if (img != null)
            //        {
            //            img.Visible = Permission.AllowInsert;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Método que recebe o controle e restringe o acesso a ele caso seja necessário
        /// </summary>
        /// <param name="control">Controle a ser manipulado</param>
        /// <param name="ForceReadOnly">Desabilita edição do controle</param>
        public void ControlaAcesso(Control control, bool ForceReadOnly)
        {
            //    if (control is ASPxGridView)
            //    {
            //        ASPxGridView gv = (ASPxGridView)control;

            //        //Salva configuração original da grid
            //        bool[] botoesVisiveis = null;
            //        string viewStateID="acesso_" + gv.UniqueID;
            //        if (this.ViewState[viewStateID] is bool[])
            //        {
            //            botoesVisiveis=(bool[])this.ViewState[viewStateID];
            //        }
            //        else
            //        {
            //            botoesVisiveis = new bool[] {false,false,false};
            //            foreach (GridViewColumn col in gv.Columns)
            //            {
            //                if (col is GridViewCommandColumn)
            //                {
            //                    if (((GridViewCommandColumn)col).EditButton.Visible)
            //                        botoesVisiveis[0] = true;
            //                    if (((GridViewCommandColumn)col).DeleteButton.Visible)
            //                        botoesVisiveis[1] = true;
            //                    if (((GridViewCommandColumn)col).NewButton.Visible)
            //                        botoesVisiveis[2] = true;
            //                    break;
            //                }
            //            }
            //            HtmlImage img = (HtmlImage)gv.FindHeaderTemplateControl(gv.Columns[""], "btnNovoGrid");

            //            if (img != null)
            //            {
            //                botoesVisiveis[2] = true;
            //            }

            //            this.ViewState[viewStateID] = botoesVisiveis;
            //        }

            //        foreach (GridViewColumn col in gv.Columns)
            //        {
            //            if (col is GridViewCommandColumn)
            //            {
            //                GridViewCommandColumn cmdCol = (GridViewCommandColumn)col;
            //                cmdCol.EditButton.Visible = botoesVisiveis[0] && Permission.AllowUpdate && !ForceReadOnly;
            //                cmdCol.DeleteButton.Visible = botoesVisiveis[1] && Permission.AllowDelete && !ForceReadOnly;
            //                cmdCol.NewButton.Visible = botoesVisiveis[2] && Permission.AllowInsert && !ForceReadOnly;
            //                break;
            //            }
            //            //Isto corrige a posição dos botões na grid para o Firefox.
            //            col.CellStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            //        }

            //        if (gv != null)
            //        {
            //            HtmlImage img = (HtmlImage)gv.FindHeaderTemplateControl(gv.Columns[""], "btnNovoGrid");

            //            if (img != null)
            //            {
            //                img.Visible = botoesVisiveis[2] && Permission.AllowInsert && !ForceReadOnly;
            //            }
            //        }
            //    }
        }

        /// <summary>
        /// Método que recebe os 3 controles para restringir a visibilidade
        /// </summary>
        /// <param name="excluir">Controle de excluir</param>
        /// <param name="novo">Controle de novo</param>
        /// <param name="editar">Controle de editar</param>
        public void ControlaAcesso(Control control, AcaoControle ac)
        {
            //    if (ac == AcaoControle.excluir)
            //    {
            //        if (control.Visible)
            //            control.Visible = Permission.AllowDelete;
            //    }
            //    else if (ac == AcaoControle.novo)
            //    {
            //        if (control.Visible)
            //            control.Visible = Permission.AllowInsert;
            //    }
            //    else if (ac == AcaoControle.editar)
            //    {
            //        if (control.Visible)
            //            control.Visible = Permission.AllowUpdate;
            //    }
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

                    string q = Request.QueryString["Chave"];
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

        public static string CodificaQueryString(IDictionary<string, string> pares)
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
                    drop.Items.Add(li);
                    drop.SelectedValue = dado;
                }
                else
                {
                    drop.SelectedValue = dado;
                }
            }
        }

        #region Session DOL
        public String NomeUnidadeEnsino
        {
            get { return Convert.ToString(Session["txtNomeUnidadeEns"]); }
            //set { Session["txtNomeUnidadeEns"] = value; }
        }
        public String CodUnidadeEnsino
        {
            get { return Convert.ToString(Session["txtCodUnidadeEns"]); }
            //set { Session["txtCodUnidadeEns"] = value; }
        }
        public String Ano
        {
            get { return Convert.ToString(Session["txtAno"]); }
            //set { Session["txtAno"] = value; }
        }
        public String Turma
        {
            get { return Convert.ToString(Session["txtTurma"]); }
            //set { Session["txtTurma"] = value; }
        }
        public String Periodo
        {
            get { return Convert.ToString(Session["txtPeriodo"]); }
            //set { Session["txtPeriodo"] = value; }
        }
        public String NomeDisciplina
        {
            get { return Convert.ToString(Session["txtNomeDisciplina"]); }
            //set { Session["txtNomeDisciplina"] = value; }
        }
        public String Disciplina
        {
            get { return Convert.ToString(Session["txtDisciplina"]); }
            //set { Session["txtDisciplina"] = value; }
        }

        public String Grade_id
        {
            get { return Convert.ToString(Session["txtGrade_id"]); }
        }

        public String MatriculaDocente
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }
        public Boolean DadosValidos
        {
            get
            {
                return new String[] { MatriculaDocente, Disciplina, Turma, Ano, Periodo }.Where(i => String.IsNullOrEmpty(i)).Count() == 0;
            }
        }
        #endregion
    }
}
