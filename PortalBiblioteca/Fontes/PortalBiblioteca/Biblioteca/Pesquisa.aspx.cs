using System;
using System.Collections.Generic;
using Techne.Lyceum.Net.Modulos;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    [NavUrl("~/Biblioteca/Pesquisa.aspx"),
    ControlText("Pesquisa"),
    Title("Pesquisa"),]

    public partial class Pesquisa : TPage
    {
        #region Código gerado Techne
        public static string GetUrl()
        {
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    string busca = Request.QueryString["Chave"];

                    if (string.IsNullOrEmpty(busca))
                    {
                        lblMensagem.Text = "As chaves digitadas para pesquisa estão em branco.";
                        return;
                    }
                    if (busca.Length <= 2)
                    {
                        lblMensagem.Text = "A chaves de pesquisa devem ter mais de 2 dígitos.";
                        return;
                    }
                    hddBusca.Value = busca;
                    Session["busca"] = busca;
                }

                odsPesquisa.Select();
                odsPesquisa.DataBind();
                grdBusca.DataBind();
            }
        }

        const string GridCustomPageSizeName = "gridCustomPageSize";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session[GridCustomPageSizeName] != null)
            {
                grdBusca.SettingsPager.PageSize = (int)Session[GridCustomPageSizeName];
            }
            PublicMaster mp = (PublicMaster)Master;
            mp.habilitaLoading = true;
        }

        protected void grdBusca_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            int newPageSize;
            if (!int.TryParse(e.Parameters, out newPageSize)) return;
            grdBusca.SettingsPager.PageSize = newPageSize;
            Session[GridCustomPageSizeName] = newPageSize;
            grdBusca.DataBind();
        }

        protected string WriteSelectedIndex(int pageSize)
        {
            return pageSize == grdBusca.SettingsPager.PageSize ? "selected='selected'" : string.Empty;
        }

        protected string GetShowingOnPage()
        {
            int pageSize = grdBusca.SettingsPager.PageSize;
            int startIndex = grdBusca.PageIndex * pageSize + 1;
            int endIndex = (grdBusca.PageIndex + 1) * pageSize;
            if (endIndex > grdBusca.VisibleRowCount)
            {
                endIndex = grdBusca.VisibleRowCount;
            }
            return string.Format("Mostrando {0}-{1} de {2}", startIndex, endIndex, grdBusca.VisibleRowCount);
        }

        public object Listar(string busca)
        {
            Techne.Data.QueryTable qt = null;

            if (!string.IsNullOrEmpty(busca))
            {
                qt = RN.Biblioteca.PesquisaLivro(busca);
            }
            return qt;
        }

        protected void grdBusca_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "navurl")
            {
                string id = Convert.ToString(e.GetListSourceFieldValue("id"));
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("id", id);
                e.Value = TPage.CodificaQueryString(dic);
            }
        }



    }
}












