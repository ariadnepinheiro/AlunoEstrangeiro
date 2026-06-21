using System;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/VerbaPorCompetencia.aspx"),
  ControlText("VerbaPorCompetencia"),
  Title("Verba por Competência")]

    public partial class VerbaPorCompetencia : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdVerba, "Verbas por Competência");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ListItem item = ddlAno.Items.FindByValue(DateTime.Today.Year.ToString());
                    if (item != null)
                    {
                        ddlAno.SelectedValue = DateTime.Today.Year.ToString();
                        grdVerba.Visible = true;
                        odsVerba.Select();
                        odsVerba.DataBind();
                        grdVerba.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdVerba);
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CarregarAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarAno()
        {
            QueryTable qt = RN.PeriodoLetivo.ConsultarAno();
            ddlAno.DataSource = qt;
            ddlAno.DataBind();
            ListItem ls = new ListItem("< Selecione >", "");
            ddlAno.Items.Insert(0, ls);
        }


        protected void grdVerba_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdVerba);
        }


        public object Listar(string ano)
        {
            DataTable dt = null;

            if (!string.IsNullOrEmpty(ano))
            {
                dt = RN.DocenteGLP.CarregarVerbaCompetencia(Convert.ToInt32(ano));
            }
            return dt;
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdVerba.CancelEdit();

                if (ddlAno.SelectedValue != "")
                {
                    grdVerba.Visible = true;
                    odsVerba.Select();
                    odsVerba.DataBind();
                    grdVerba.DataBind();
                }
                else
                {
                    grdVerba.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void Update(Object mes, Object valor, Object aceita, Object alocada, Object saldo) { }

        protected void odsVerba_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.RetValue retorno = null;
            Ly_verba_glp_ano_mes.Row row = new Ly_verba_glp_ano_mes().NewRow();

            row.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
            row.Mes = Convert.ToDecimal(e.InputParameters["mes"]);
            row.Valor = Convert.ToDecimal(e.InputParameters["valor"]);
            decimal aceita = Convert.ToDecimal(e.InputParameters["aceita"]);

            retorno = RN.DocenteGLP.AtualizaVerbaCompetencia(row, aceita);

            if (retorno != null)
            {
                if (!retorno.Ok)
                    throw new Exception(retorno.Errors.ToString());
            }
        }

        protected void grdVerba_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (ddlAno.SelectedValue != "")
            {
                if (e.ButtonType.ToString() == "Edit" && Convert.ToUInt16(ddlAno.SelectedValue) < DateTime.Today.Year)
                {
                    e.Visible = false;
                }
                else if (e.ButtonType.ToString() == "Edit" && Convert.ToUInt16(ddlAno.SelectedValue) == DateTime.Today.Year)
                {
                    int mes = Convert.ToInt16(grdVerba.GetRowValues(e.VisibleIndex, "mes"));
                    if (mes < DateTime.Today.Month)
                        e.Visible = false;
                }
            }
        }

        protected void grdVerba_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["valor"] != null)
            {
                decimal total = Convert.ToDecimal(e.NewValues["valor"]);
                decimal aceita = Convert.ToDecimal(e.NewValues["aceita"]);
                if (total <= 0)
                {
                    e.RowError = "Valor deve ser maior que zero.";
                }

                if (total < aceita)
                {
                    e.RowError = "O Valor Inicial não pode ser menor que a GLP Aceita ";
                }

                QueryTable qt = RN.Basico.ConsultaItemTabelaValDescr("Verba Maxima");
                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                    {
                        decimal verba_maxima = Convert.ToDecimal(qt.Rows[0]["descr"]);
                        if (total > verba_maxima)
                            e.RowError = "Valor não pode ultrapassar R$" + verba_maxima + ".";
                    }
                }
            }
        }

    }
}

