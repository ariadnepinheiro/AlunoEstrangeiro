namespace Techne.Lyceum.Net
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DevExpress.Web.ASPxGridView;
    using DevExpress.Web.ASPxPopupControl;
    using DevExpress.Web.ASPxPanel;
    using System.Web.UI.HtmlControls;

    [Flags]
    public enum NavigationKey
    {
        Enter = 1, 

        Backspace = 2
    }

    public class TPage : Techne.TPage
    {
        private const string DefaultConnection = "Lyceum";

        private NavigationKey disabledNavigationKeys = NavigationKey.Enter | NavigationKey.Backspace;

        private NameValueCollection queryStringDecodificada;

        public TPage()
        {
            this.Connection = DefaultConnection;
            this.SwitchPostback = false;
        }

        public string Ano
        {
            get
            {
                return Convert.ToString(this.Session["txtAno"]);
            }
        }

        public override string ApplicationName
        {
            get
            {
                return "LyceumNet";
            }
        }

        public string CodUnidadeEnsino
        {
            get
            {
                return Convert.ToString(this.Session["txtCodUnidadeEns"]);
            }
        }

        [DefaultValue(DefaultConnection)]
        public override sealed string Connection
        {
            get
            {
                return base.Connection;
            }

            set
            {
                base.Connection = value;
            }
        }

        public string Curso
        {
            get
            {
                return Convert.ToString(this.Session["txtCurso"]);
            }
        }

        public bool DadosValidos
        {
            get
            {
                return new[]
                       {
                           this.MatriculaDocente, this.Disciplina, this.Turma, this.Ano, this.Periodo
                       }
                       .Where(i => string.IsNullOrEmpty(i))
                       .Count() == 0;
            }
        }

        [DefaultValue(NavigationKey.Enter | NavigationKey.Backspace)]
        public NavigationKey DisabledNavigationKeys
        {
            get
            {
                return this.disabledNavigationKeys;
            }

            set
            {
                this.disabledNavigationKeys = value;
            }
        }

        public string Disciplina
        {
            get
            {
                return Convert.ToString(this.Session["txtDisciplina"]);
            }
        }

        public string MatriculaDocente
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        public string Modalidade
        {
            get
            {
                return Convert.ToString(this.Session["txtModalidade"]);
            }
        }

        public string NomeDisciplina
        {
            get
            {
                return Convert.ToString(this.Session["txtNomeDisciplina"]);
            }
        }

        public string NomeDocente
        {
            get
            {
                return Convert.ToString(this.Session["txtNomeProfessor"]);
            }
        }

        public string NomeUnidadeEnsino
        {
            get
            {
                return Convert.ToString(this.Session["txtNomeUnidadeEns"]);
            }
        }

        public string NumFunc
        {
            get
            {
                return Convert.ToString(this.Session["txtNumFunc"]);
            }
        }

        public string Periodo
        {
            get
            {
                return Convert.ToString(this.Session["txtPeriodo"]);
            }
        }

        public NameValueCollection QueryStringDecodificada
        {
            get
            {
                if (this.queryStringDecodificada == null)
                {
                    var decod = new NameValueCollection();
                    var q = this.Request.QueryString["Chave"];

                    if (!string.IsNullOrEmpty(q))
                    {
                        try
                        {
                            var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                            var chave = Encoding.UTF8.GetString(decodedBytes);

                            if (!string.IsNullOrEmpty(chave))
                            {
                                var listaDados = chave.Split('&');

                                foreach (var dados in listaDados)
                                {
                                    var par = dados.Split('=');

                                    if (par.Length > 0
                                        && !string.IsNullOrEmpty(par[0]))
                                    {
                                        if (par.Length > 1
                                            && !string.IsNullOrEmpty(par[1]))
                                        {
                                            decod.Add(par[0], par[1]);
                                        }
                                        else
                                        {
                                            decod.Add(par[0], string.Empty);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    this.queryStringDecodificada = decod;
                }

                return this.queryStringDecodificada;
            }
        }

        public string Serie
        {
            get
            {
                return Convert.ToString(this.Session["txtSerie"]);
            }
        }

        public decimal Subperiodo
        {
            get
            {
                var subperiodo = this.ViewState["subperiodo"];

                return subperiodo == null ? -1.0m : (decimal)subperiodo;
            }

            set
            {
                this.ViewState["subperiodo"] = value;
            }
        }

        public string TipoCurso
        {
            get
            {
                return Convert.ToString(this.Session["txtTipoCurso"]);
            }
        }

        public string Turma
        {
            get
            {
                return Convert.ToString(this.Session["txtTurma"]);
            }
        }

        protected bool VerificarCompatibilidadeComIE = false;
        protected string MensagemCompatibilidadeIE; 

        public static string CodificaQueryString(IDictionary<string, string> pares)
        {
            var str = new StringBuilder();

            foreach (var key in pares.Keys)
            {
                if (str.Length > 0)
                {
                    str.Append("&");
                }

                str.Append(key + "=" + pares[key]);
            }

            var bytesToEncode = Encoding.UTF8.GetBytes(str.ToString());

            return "Chave=" + Convert.ToBase64String(bytesToEncode);
        }

        public static void RegisterDisabledNavigationKeys(Page page, NavigationKey disabledKeys)
        {
            var strScript = new StringBuilder();
            if (!page.ClientScript.IsStartupScriptRegistered(typeof(Page), "RegisterDisabledNavigationKeys"))
            {
                strScript.AppendLine("<script language=\"JavaScript\">");
                strScript.AppendLine("if(typeof disableNavigationKeys == 'function') {");
                strScript.AppendLine(" disableNavigationKeys(" + ((int)disabledKeys) + ");");
                strScript.AppendLine("}");
                strScript.AppendLine("</script>");
                page.ClientScript.RegisterStartupScript(typeof(Page), "RegisterDisabledNavigationKeys", strScript.ToString());
            }
        }

        public static void TituloGrid(ASPxGridView grid, string titulo)
        {
            if (grid == null)
            {
                return;
            }

            var tituloGrade = grid.SettingsText.Title;
            if (tituloGrade != string.Empty)
            {
                grid.SettingsText.Title = tituloGrade.Replace("|Tabela:|", titulo);
            }
        }

        public void PreencherDadoCombo(DropDownList drop, string dado)
        {
            // como preenche da QT
            if (dado == null)
            {
                drop.SelectedValue = string.Empty;
            }
            else
            {
                if (drop.Items.FindByValue(dado) == null)
                {
                    var dadoinvalido = "<" + dado + ">";
                    ListItem li;
                    if (drop.Items.Count > 0)
                    {
                        li = new ListItem(dadoinvalido, dado);
                    }
                    else
                    {
                        li = new ListItem(dado, dado);
                    }

                    drop.Items.Add(li);
                    drop.SelectedValue = dado;
                }
                else
                {
                    drop.SelectedValue = dado;
                }
            }
        }

        protected string CodificaQueryString(NameValueCollection pares)
        {
            var str = new StringBuilder();
            foreach (string key in pares.Keys)
            {
                if (str.Length > 0)
                {
                    str.Append("&");
                }

                str.Append(key + "=" + pares[key]);
            }

            var bytesToEncode = Encoding.UTF8.GetBytes(str.ToString());
            return "Chave=" + Convert.ToBase64String(bytesToEncode);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ClientScript.IsClientScriptBlockRegistered(typeof(TPage), "TPageLyceum"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(
                    typeof(TPage), 
                    "TPageLyceum", 
                    "<script language=\"JavaScript\" " + "src=\"" + TUtil.TranslateRelativeUrl("~/Scripts/TPageLyceum.js", this) + "\">" + "</script>\r\n");
            }

            RegisterDisabledNavigationKeys(this, this.DisabledNavigationKeys);
        }

        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            if (VerificarCompatibilidadeComIE)
            {
                ASPxPopupControl popup = (ASPxPopupControl)Master.FindControl("ppcMensagemCompatilidadeIE");

                if (!IsPostBack)
                {
                    if (popup != null)
                    {
                        Label lblMensagem = (Label)popup.FindControl("lblMensagem");

                        bool ehInternetExplorer = EhInternetExplorer();

                        float versaoIE = (float)(Request.Browser.MajorVersion + Request.Browser.MinorVersion);

                        if ((!ehInternetExplorer) ||
                            (ehInternetExplorer && versaoIE < 7.0))
                        {
                            lblMensagem.Text = MensagemCompatibilidadeIE;
                            popup.Enabled = true;
                            popup.ShowOnPageLoad = true;
                        }
                    }
                }
            } 
        }

        protected void OcultarPopupModal()
        {
            if (VerificarCompatibilidadeComIE)
            {
                ASPxPopupControl popup = (ASPxPopupControl)Master.FindControl("ppcMensagemCompatilidadeIE");

                if (popup != null)
                {
                    VerificarCompatibilidadeComIE = false;
                    popup.ShowOnPageLoad = false;
                }
            }
        }

        protected bool EhInternetExplorer()
        {
            return ((Request.Browser.Browser == "InternetExplorer" || Request.Browser.Browser == "IE") &&
                    (!Request.UserAgent.Contains("Chrome")));
        }
    }
}