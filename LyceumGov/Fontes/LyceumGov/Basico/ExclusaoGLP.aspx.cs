using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/ExclusaoGLP.aspx"),
      ControlText("ExclusaoGLP"),
      Title("Exclusão de Pedido GLP"),
    ]

    public partial class ExclusaoGLP : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDocenteFuncaoGLP, "Pedidos Aceitos de GLP");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDocenteFuncaoGLP);
            ControlaAcesso(grdDocenteFuncaoGLP, AcaoControle.excluir, "btnExcluir");
        }

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

        protected void grdDocenteFuncaoGLP_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDocenteFuncaoGLP);
        }

        public object Listar(DbObject idregional, DbObject unidade_ensino)
        {
            QueryTable qt = null;

            if (!idregional.IsNull && !unidade_ensino.IsNull)
            {
                qt = RN.DocenteGLP.ConsultarExclusaoDocenteGLP(idregional.ToString(), unidade_ensino.ToString());
            }
            return qt;
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                tseUnidade_Ensino.ResetValue();
                if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
                {
                    if (tseUnidade_Ensino.IsValidDBValue && !tseUnidade_Ensino.DBValue.IsNull)
                        grdDocenteFuncaoGLP.Visible = true;
                    lblMensagem.Text = string.Empty;
                }
                else if (!tseRegional.DBValue.IsNull)
                {
                    grdDocenteFuncaoGLP.Visible = false;
                    lblMensagem.Text = "Regional não cadastrada.";
                }
                else
                {
                    grdDocenteFuncaoGLP.Visible = false;
                    lblMensagem.Text = "Favor consultar uma Regional.";
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
                if (tseUnidade_Ensino.IsValidDBValue && !tseUnidade_Ensino.DBValue.IsNull)
                {
                    tseRegional.DBValue = tseUnidade_Ensino["id_regional"];
                    if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
                        grdDocenteFuncaoGLP.Visible = true;
                    lblMensagem.Text = string.Empty;
                }
                else if (!tseRegional.DBValue.IsNull)
                {
                    grdDocenteFuncaoGLP.Visible = false;
                    lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                }
                else
                {
                    grdDocenteFuncaoGLP.Visible = false;
                    lblMensagem.Text = "Favor consultar uma Unidade de Ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDocenteFuncaoGLP_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }

        protected void grdDocenteFuncaoGLP_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            Techne.Lyceum.CR.Ly_docente_funcao_glp.Row row = new Techne.Lyceum.CR.Ly_docente_funcao_glp().NewRow();
            row.Id_docente_funcao_glp = Convert.ToDecimal(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "ID_DOCENTE_FUNCAO_GLP"));
            row.Status = "Cancelada";
            row.Glp_usada = 0;
            row.Data = DateTime.Today;
            row.Glp_solicitada = Convert.ToDecimal(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "glp_solicitada"));
            row.Glp_cancelada = row.Glp_solicitada;
            row.Matricula = Convert.ToString(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "MATRICULA"));
            row.Agrupamento = Convert.ToString(grdDocenteFuncaoGLP.GetRowValues(e.VisibleIndex, "DISCIPLINA"));

            RetValue retorno = null;

            if (e.ButtonID == "btnExcluir")
            {
                try
                {
                    retorno = RN.DocenteGLP.ReprovarGLPAceita(row);
                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                        {
                            throw new Exception(retorno.Errors.ToString());
                        }
                        else
                        {
                            Session["Mensagem"] = retorno.Message;
                            odsDocenteFuncaoGLP.Select();
                            odsDocenteFuncaoGLP.DataBind();
                            grdDocenteFuncaoGLP.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Session["Mensagem"] = ex.Message;
                }
            }
        }
    }
}

