using System;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/GrupoHabilitacaoDisciplina.aspx"),
      ControlText("GrupoHabilitaçãoDisciplina"),
      Title("Grupos de Habilitações das Disciplinas"),
    ]

    public partial class GrupoHabilitacaoDisciplina : TPage
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
            TituloGrid(grdGrupoHabilitacaoDisciplina, "Disciplinas do Grupo");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxGridView.ASPxGridViewTemplateReplacement Update = (DevExpress.Web.ASPxGridView.ASPxGridViewTemplateReplacement)grdGrupoHabilitacaoDisciplina.FindEditFormTemplateControl("repUpdate");
            if (Update != null)
                Update.Visible = false;
            if (tseAgrupamento.IsValidDBValue && tseAgrupamento.Value != null)
                grdGrupoHabilitacaoDisciplina.Visible = true;
            else
                grdGrupoHabilitacaoDisciplina.Visible = false;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdGrupoHabilitacaoDisciplina);
        }

        protected void grdGrupoHabilitacaoDisciplina_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdGrupoHabilitacaoDisciplina);
        }

        protected void grdGrupoHabilitacaoDisciplina_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["agrupamento"] = tseAgrupamento.DBValue.ToString();
        }

        protected void tseAgrupamento_Changed(object sender, ChangedEventArgs args)
        {
            if (tseAgrupamento.IsValidDBValue && !tseAgrupamento.DBValue.IsNull)
            {
                grdGrupoHabilitacaoDisciplina.Visible = true;
                lblMensagem.Text = string.Empty;
            }
            else if (!tseAgrupamento.DBValue.IsNull)
            {
                grdGrupoHabilitacaoDisciplina.Visible = false;
                lblMensagem.Text = "Grupo não cadastrado.";
            }
            else
            {
                grdGrupoHabilitacaoDisciplina.Visible = false;
                lblMensagem.Text = "Favor consultar um grupo.";
            }
        }

        protected void grdGrupoHabilitacaoDisciplina_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string agrupamento = tseAgrupamento.DBValue.ToString();
                string disciplina = Convert.ToString(e.GetListSourceFieldValue("disciplina"));
                e.Value = agrupamento + "-" + disciplina;
            }
        }

        protected void grdGrupoHabilitacaoDisciplina_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("agrupamento", e.Values["agrupamento"]);
            e.Keys.Add("disciplina", e.Values["disciplina"]);
        }

        protected void grdGrupoHabilitacaoDisciplina_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("agrupamento", chaves[0]);
            e.Keys.Add("disciplina", chaves[1]);
        }

        protected void grdGrupoHabilitacaoDisciplina_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdGrupoHabilitacaoDisciplina.Settings.ShowFilterRow = false;
        }

        protected void grdGrupoHabilitacaoDisciplina_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdGrupoHabilitacaoDisciplina.Settings.ShowFilterRow = false;
        }

        protected void grdGrupoHabilitacaoDisciplina_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            TSearchBox tseDisciplina = (TSearchBox)grdGrupoHabilitacaoDisciplina.FindEditFormTemplateControl("tsbDisciplina");
            if (tseDisciplina != null)
            {
                if (tseDisciplina.DBValue.IsNull || !tseDisciplina.IsValidDBValue)
                {
                    e.RowError = "Favor selecionar um valor válido para disciplina.";
                    return;
                }
                else
                {
                    //valida TIPO
                    string tipo_disciplina = RN.Disciplina.ConsultarTipoDisciplina(tseDisciplina.DBValue.ToString());
                    string tipo_grupo = RN.Disciplina.ConsultarTipoGrupo(tseAgrupamento.DBValue.ToString());
                    if (tipo_disciplina != tipo_grupo)
                    {
                        e.RowError = "Disciplina do tipo " + tipo_disciplina + " não pode fazer parte do grupo de tipo " + tipo_grupo + ".";
                        return;
                    }
                }
            }

        }

        protected void tsbDisciplina_Changed(object sender, ChangedEventArgs args)
        {
            if (!((TSearchBox)sender).DBValue.IsNull && ((TSearchBox)sender).IsValidDBValue)
            {
                DevExpress.Web.ASPxGridView.ASPxGridViewTemplateReplacement Update = (DevExpress.Web.ASPxGridView.ASPxGridViewTemplateReplacement)grdGrupoHabilitacaoDisciplina.FindEditFormTemplateControl("repUpdate");
                Update.Visible = true;
            }
            else
            {
                DevExpress.Web.ASPxGridView.ASPxGridViewTemplateReplacement Update = (DevExpress.Web.ASPxGridView.ASPxGridViewTemplateReplacement)grdGrupoHabilitacaoDisciplina.FindEditFormTemplateControl("repUpdate");
                Update.Visible = false;
            }
        }
    }
}
