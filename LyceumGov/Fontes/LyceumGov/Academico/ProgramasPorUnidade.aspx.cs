using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/ProgramasPorUnidade.aspx"),
    ControlText("ProgramasPorUnidade"),
    Title("Programas Sociais por Unidade de Ensino"),]
    public partial class ProgramasPorUnidade : TPage
    {
        #region Código Padrão Techne
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_RenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProgramasUnidade);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProgramasUnidade, "Programas Por Unidade");
        }

		#region Métodos da Grid
        protected void grdProgramasUnidade_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if ((e.Column.FieldName) == "dt_inicio")
            {
                (e.Editor as ASPxDateEdit).MaxDate = DateTime.Now;
            }

            // filtrando uma combo da grid pela outra
            if (!grdProgramasUnidade.IsEditing || e.Column.FieldName != "programa")
                return;
            ASPxComboBox combo = e.Editor as ASPxComboBox;
            combo.Callback += new CallbackEventHandlerBase(cmbPrograma_OnCallback);
        }

        private void cmbPrograma_OnCallback(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox cmbPrograma = (source as ASPxComboBox);

            cmbPrograma.Items.Clear();
            cmbPrograma.DataSource = RN.ProgramasUnidade.ConsultarProgramas(e.Parameter);
            cmbPrograma.DataBind();
        }

        protected void grdProgramasUnidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProgramasUnidade.Settings.ShowFilterRow = false;
        }

        protected void grdProgramasUnidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdProgramasUnidade.Settings.ShowFilterRow = false;
        }

        protected void grdProgramasUnidade_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
			grdProgramasUnidade.Settings.ShowFilterRow = false;
            if (grdProgramasUnidade.IsNewRowEditing)
            {
                if (RN.ProgramasUnidade.ExisteUnidadeEnsinoPrograma(tseUnidadeEns.Value.ToString(), e.NewValues["agencia"], e.NewValues["programa"], e.NewValues["tipo_beneficio"], e.NewValues["ano_validade"]))
                {
                    e.RowError = "Já existe um programa social com esses dados para esta unidade de ensino.";
                }
            }
            if (grdProgramasUnidade.IsEditing)
            {
                if (RN.ProgramasUnidade.ExisteUnidadeEnsinoPrograma(tseUnidadeEns.Value.ToString(), e.NewValues["agencia"], e.NewValues["programa"], e.NewValues["tipo_beneficio"], e.NewValues["ano_validade"], e.Keys["id_unidade_ensino_programas"]))
                {
					e.RowError = "Já existe um programa social com esses dados para esta unidade de ensino.";
                }
            }

            DateTime dataInicio = Convert.ToDateTime(e.NewValues["dt_inicio"]);
            DateTime dataFinal = Convert.ToDateTime(e.NewValues["dt_fim"]);

            if (dataFinal < dataInicio)
            {
                e.RowError = "A data final deve ser maior que a data de início.";
            }
        }

        protected void grdProgramasUnidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["unidade_ens"] = tseUnidadeEns.Value;
        }

        protected void grdProgramasUnidade_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProgramasUnidade);
        }
		#endregion

        protected void tseUnidadeEns_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (tseUnidadeEns.IsValidDBValue && !tseUnidadeEns.DBValue.IsNull)
                grdProgramasUnidade.Visible = true;
            else
                grdProgramasUnidade.Visible = false;
        }

        protected void grdProgramasUnidade_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if(e.Column.FieldName == "programa")
                e.DisplayText = RN.ProgramasUnidade.ConsultarDescricaoPrograma(e.Value.ToString());
        }
    }
}
