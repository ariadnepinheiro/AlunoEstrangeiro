using System;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Web;


namespace Techne.Lyceum.Net.Curriculo
{

    [NavUrl("~/Curriculo/ValorGLP.aspx"),
      ControlText("Valor GLP"),
      Title("Valor da GLP")]

    public partial class ValorGLP : TPage
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
            TituloGrid(grdValores, "Valores GLP");
        }

        protected void Page_Load(object sender, EventArgs e)
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

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdValores);
        }

        private void CarregarAno()
        {
            QueryTable qt = RN.PeriodoLetivo.ConsultarAno();
            ddlAno.DataSource = qt;
            ddlAno.DataBind();
            ListItem ls = new ListItem("< Selecione >", "");
            ddlAno.Items.Insert(0, ls);
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdValores.CancelEdit();

                if (ddlAno.SelectedValue != "")
                {
                    grdValores.Visible = true;
                }
                else
                {
                    grdValores.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        public object Listar(string ano)
        {
            DataTable dt = null;

            if (!string.IsNullOrEmpty(ano))
            {
                dt = RN.DocenteGLP.CarregarValoresGLP(Convert.ToInt32(ano));
            }
            return dt;
        }

        protected void grdValores_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            e.NewValues["funcao"] = e.OldValues["funcao"];

            if (grdValores.IsEditing)
            {
                RN.RetValue retorno = null;

                CR.Ly_valor_glp dtValorGLP = new Techne.Lyceum.CR.Ly_valor_glp();
                CR.Ly_valor_glp dtValorGLPDel = new Techne.Lyceum.CR.Ly_valor_glp();

                for (int i = 1; i < 13; i++)
                {
                    if (e.NewValues["m" + i.ToString()] != null && e.NewValues["m" + i.ToString()].ToString() != string.Empty && Convert.ToInt32(e.NewValues["m" + i.ToString()]) != 0)
                    {
                        CR.Ly_valor_glp.Row row = dtValorGLP.NewRow();

                        row.Id_valor_glp = i;
                        row.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
                        row.Mes = i;
                        row.Valor = Convert.ToDecimal(e.NewValues["m" + i.ToString()]);

                        if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - Atividade Complementar")
                        {
                            row.Funcao = "108";
                            row.Tipo = "Atividade";
                        }
                        else if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - Disciplina Integrada")
                        {
                            row.Funcao = "108";
                            row.Tipo = "Disciplina";
                        }
                        else if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - 40 - Atividade Complementar")
                        {
                            row.Funcao = "109";
                            row.Tipo = "Atividade";
                        }
                        else if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - 40 - Disciplina Integrada")
                        {
                            row.Funcao = "109";
                            row.Tipo = "Disciplina";
                        }
                        else
                        {
                            row.Funcao = Convert.ToString(e.NewValues["funcao"]);
                        }

                        dtValorGLP.Rows.Add(row);
                    }
                    else if (e.OldValues["m" + i.ToString()] != null && e.OldValues["m" + i.ToString()].ToString() != string.Empty)
                    {
                        CR.Ly_valor_glp.Row row = dtValorGLPDel.NewRow();

                        row.Id_valor_glp = i; //só p/ p dt, depois troca
                        row.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
                        row.Mes = i;
                        row.Valor = Convert.ToDecimal(e.NewValues["m" + i.ToString()]);

                        if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - Atividade Complementar")
                        {
                            row.Funcao = "108";
                            row.Tipo = "Atividade";
                        }
                        else if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - Disciplina Integrada")
                        {
                            row.Funcao = "108";
                            row.Tipo = "Disciplina";
                        }
                        else if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - 40 - Atividade Complementar")
                        {
                            row.Funcao = "109";
                            row.Tipo = "Atividade";
                        }
                        else if (Convert.ToString(e.NewValues["funcao"]) == "DOC II - 40 - Disciplina Integrada")
                        {
                            row.Funcao = "109";
                            row.Tipo = "Disciplina";
                        }
                        else
                        {
                            row.Funcao = Convert.ToString(e.NewValues["funcao"]);
                        }
                        dtValorGLPDel.Rows.Add(row);
                    }
                }

                if (dtValorGLP.Rows != null && dtValorGLP.Rows.Count > 0)
                {
                    retorno = RN.DocenteGLP.AtualizaValorGLP(dtValorGLP);
                }

                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        e.RowError = "Erro ao atualizar. " + retorno.Errors.ToString();
                        return;
                    }

                }

                if (dtValorGLPDel.Rows != null && dtValorGLPDel.Rows.Count > 0)
                {
                    retorno = RN.DocenteGLP.ExcluirValorGLP(dtValorGLPDel);
                }

                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        e.RowError = "Erro ao excluir. " + retorno.Errors.ToString();
                        return;
                    }
                }
            }
        }

        protected void grdValores_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdValores);
        }

        protected void odsValores_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {           

        }

        public void Update(String funcao, decimal m1, decimal m2, decimal m3, decimal m4, decimal m5, decimal m6, decimal m7, decimal m8, decimal m9, decimal m10, decimal m11, decimal m12) { }

        protected void grdValores_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            int mesAtual = DateTime.Now.Month;
            int ano = Convert.ToInt32(ddlAno.SelectedValue);
            int anoAtual = DateTime.Now.Year;

            int field = 0;
            int.TryParse(e.Column.FieldName.Remove(0, 1), out field);

            if (field < mesAtual && field != 0 && ano == anoAtual || ano < anoAtual)
                e.Editor.ReadOnly = true;

            if (e.Column.FieldName == "funcao")
            {
                e.Editor.ReadOnly = true;
            }

        }
    }
}




