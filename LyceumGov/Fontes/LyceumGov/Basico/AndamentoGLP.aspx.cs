using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Web;
using System.Data;
using System.Web;

namespace Techne.Lyceum.Net.Basico
{

    [NavUrl("~/Basico/AndamentoGLP.aspx"),
      ControlText("AndamentoGLP"),
      Title("Andamento de Solicitações de GLP")]

    public partial class AndamentoGLP : TPage
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
            TituloGrid(grdAndamento, "Andamento de Solicitações");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    //quando só possui acesso a uma unidade física já vem com sua unidade de ensino e UA selecionadas
                    if (RN.UsuarioUnidadeFis.PossuiUmaUnidadeSo(HttpContext.Current.User.Identity.Name))
                    {
                        tseUnidade_Ensino.DBValue = RN.UnidadesAssociadas.ConsultarUnidadeAssociada(HttpContext.Current.User.Identity.Name);
                    }

                    ListItem item = ddlAno.Items.FindByValue(DateTime.Today.Year.ToString());
                    if (item != null)
                    {
                        ddlAno.SelectedValue = DateTime.Today.Year.ToString();
                        ddlMes.SelectedValue = DateTime.Today.Month.ToString();
                        if (!tseUnidade_Ensino.DBValue.IsNull && tseUnidade_Ensino.IsValidDBValue)
                        {
                            odsAndamento.Select();
                            odsAndamento.DataBind();
                            grdAndamento.DataBind();
                            grdAndamento.Visible = true;
                        }
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
            ControlaAcesso(grdAndamento);
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CarregarAno();
                    CarregarMes();
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
        private void CarregarMes()
        {
            for (int i = 1; i <= 12; i++)
            {
                ddlMes.Items.Add(i.ToString());
            }
            ListItem ls = new ListItem("< Selecione >", "");
            ddlMes.Items.Insert(0, ls);
        }

        protected void grdAndamento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAndamento);
        }


        public object Listar(DbObject unidade, string ano, string mes)
        {
            QueryTable dt = null;

            if (!unidade.IsNull && !string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(mes))
            {
                dt = RN.DocenteGLP.CarregarAndamentos(unidade.ToString(), Convert.ToInt32(ano), Convert.ToInt32(mes));
            }
            return dt;
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdAndamento.Visible = false;

                if (ddlAno.SelectedValue != "" && ddlMes.SelectedValue != "" && tseUnidade_Ensino.IsValidDBValue && !tseUnidade_Ensino.DBValue.IsNull)
                {
                    lblMensagem.Text = string.Empty;
                    odsAndamento.Select();
                    odsAndamento.DataBind();
                    grdAndamento.DataBind();
                    grdAndamento.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdAndamento.Visible = false;

                if (ddlAno.SelectedValue != "" && ddlMes.SelectedValue != "" && tseUnidade_Ensino.IsValidDBValue && !tseUnidade_Ensino.DBValue.IsNull)
                {
                    lblMensagem.Text = string.Empty;
                    odsAndamento.Select();
                    odsAndamento.DataBind();
                    grdAndamento.DataBind();
                    grdAndamento.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Ensino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                grdAndamento.Visible = false;

                if (ddlAno.SelectedValue != "" && tseUnidade_Ensino.IsValidDBValue && !tseUnidade_Ensino.DBValue.IsNull)
                {
                    lblMensagem.Text = string.Empty;
                    odsAndamento.Select();
                    odsAndamento.DataBind();
                    grdAndamento.DataBind();
                    grdAndamento.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAndamento_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
        }

        protected void grdAndamento_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string status = Convert.ToString(e.CellValue);
            if (status == "Cancelada" || status == "Reprovado" || status == "Expirado")
            {
                e.Cell.ForeColor = System.Drawing.Color.Red;
            }
            else if (status == "Aceita")
            {
                e.Cell.ForeColor = System.Drawing.Color.Green;
            }
        }

        protected void grdAndamento_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "segmento")
            {
                string valor = e.Value.ToString();
                if (!string.IsNullOrEmpty(valor))
                {
                    if (valor.Contains("DOC II"))
                    {
                        e.DisplayText = "Ensino Fundamental Anos Iniciais";
                    }
                    else if (valor.Contains("DOC I"))
                    {
                        e.DisplayText = "Ensino Fundamental Anos Finais / Ensino Médio";
                    }
                }
            }
        }
    }
}
