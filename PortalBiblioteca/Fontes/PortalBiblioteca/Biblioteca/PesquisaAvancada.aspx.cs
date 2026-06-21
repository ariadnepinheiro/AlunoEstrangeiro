using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using Techne.Data;
using Techne.Lyceum.Net.Modulos;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    

    public class FiltrosPesquisa
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editora { get; set; }
        public string Assunto { get; set; }
        public object Biblioteca { get; set; }
        public bool TodasBibliotecas { get; set; }

        public FiltrosPesquisa()
        {
        }

    }

    [NavUrl("~/Biblioteca/PesquisaAvancada.aspx"),
    ControlText("PesquisaAvancada"),
    Title("Pesquisa Avançada"),]

    public partial class PesquisaAvancada : TPage
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
            if (IsPostBack)
                CarregarGridAvancada();
        }

        private void CarregarGridAvancada()
        {
            FiltrosPesquisa filtros = new FiltrosPesquisa();

            if (tseBiblioteca.IsValidDBValue && !tseBiblioteca.DBValue.IsNull && !chkTodosLocais.Checked)
                filtros.Biblioteca = tseBiblioteca.DBValue;

            filtros.Titulo = filtros.Editora = filtros.Assunto = filtros.Autor = string.Empty;
            ValidaFiltros(filtros, ddlFiltro1.SelectedValue, txtFiltro1.Text);
            ValidaFiltros(filtros, ddlFiltro2.SelectedValue, txtFiltro2.Text);
            ValidaFiltros(filtros, ddlFiltro3.SelectedValue, txtFiltro3.Text);

            List<string> lista_tipos = new List<string>();
            foreach (Control item in tipos.Controls)
            {
                if (item.ID != null)
                {
                    CheckBox chk = (CheckBox)item;
                    if (chk != null && chk.Checked)
                        lista_tipos.Add(item.ID);
                }
            }

            grdBusca.DataSource = RN.Biblioteca.PesquisaAvancadaLivro(lista_tipos, chkDisponiveis.Checked, filtros.Biblioteca, filtros.Titulo, filtros.Editora, filtros.Autor, filtros.Assunto);
            grdBusca.DataBind();
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

            //carregar tipos de materiais na div
            QueryTable qt = RN.Biblioteca.ConsultarTipos();
            if (qt != null)
            {
                for (int i = 0; i < qt.Rows.Count; i++)
                {
                    ASPxBinaryImage img = new ASPxBinaryImage();
                    img.ID = qt.Rows[i]["ID"].ToString();
                    //img.ContentBytes = Convert.FromBase64String(qt.Rows[i]["imagem"].ToString();
                    CheckBox chk = new CheckBox();
                    chk.ID = qt.Rows[i]["ID"].ToString();
                    chk.Text = qt.Rows[i]["SIGLA"].ToString() + "    ";
                    //tipos.Controls.Add(img);
                    tipos.Controls.Add(chk);

                }
            }
            else
            {
                Label lbl = new Label();
                lbl.Text = "Não existem tipos cadastrados.";
                tipos.Controls.Add(lbl);
            }
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

        protected void btnBuscarAvanc_Click(object sender, EventArgs e)
        {
            CarregarGridAvancada();
        }

        private void ValidaFiltros(FiltrosPesquisa filtros, string tipo, string valor)
        {
            switch (tipo)
            {
                default:
                    break;
                case "titulo":
                    filtros.Titulo = RetornaParametro(valor, filtros.Titulo);
                    break;
                case "editora":
                    filtros.Editora = RetornaParametro(valor, filtros.Editora);
                    break;
                case "autor":
                    filtros.Autor = RetornaParametro(valor, filtros.Autor);
                    break;
                case "assunto":
                    filtros.Assunto = RetornaParametro(valor, filtros.Assunto);
                    break;
            }
        }

        private string RetornaParametro(string valor, string valorFinal)
        {
            if (valorFinal == string.Empty)
                valorFinal = valor;
            else
                valorFinal = valor + "|" + valorFinal;

            return valorFinal;
        }

        protected void grdBusca_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "navurl")
            {
                string id = Convert.ToString(e.GetListSourceFieldValue("id"));
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("ID", id);
                e.Value = TPage.CodificaQueryString(dic);
            }
        }


    }
}
