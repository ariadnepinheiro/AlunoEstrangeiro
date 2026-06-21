using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [
    NavUrl("~/Basico/PeriodoFrequenciaAluno.aspx"),
     ControlText("PeriodoFrequenciaAluno"),
     Title("Período Frequência Aluno"),
   ]

    public partial class PeriodoFrequenciaAluno : TPage
    {
        public object Lista()
        {
            RN.Turmas.PeriodoFrequenciaAluno rnPeriodoLancamento = new Techne.Lyceum.RN.Turmas.PeriodoFrequenciaAluno();

            return rnPeriodoLancamento.Lista();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodo, "Períodos Lançamento Frequência Aluno");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPeriodo);
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

        protected void grdPeriodo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPeriodo);
        }


        public void Insert(object ANO, object MES, object DATAINICIO, object DATAFIM) { }
        public void Update(object ANO, object MES, object DATAINICIO, object DATAFIM, object PERIODOFREQUENCIAALUNOID) { }
        public void Delete(object PERIODOFREQUENCIAALUNOID) { }

        protected void grdPeriodo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPeriodo.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPeriodo.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.PeriodoFrequenciaAluno periodoLancamento = new Techne.Lyceum.RN.Turmas.Entidades.PeriodoFrequenciaAluno();
            RN.Turmas.PeriodoFrequenciaAluno rnPeriodoLancamento = new Techne.Lyceum.RN.Turmas.PeriodoFrequenciaAluno();

            periodoLancamento.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoLancamento.Mes = e.NewValues["MES"] != null ? Convert.ToInt32(e.NewValues["MES"]) : -1;
            periodoLancamento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodoLancamento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodoLancamento.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoLancamento.Valida(periodoLancamento, true);

            if (validacao.Valido)
            {
                rnPeriodoLancamento.Insere(periodoLancamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodo.DataBind();

        }

        protected void grdPeriodo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.PeriodoFrequenciaAluno periodoLancamento = new Techne.Lyceum.RN.Turmas.Entidades.PeriodoFrequenciaAluno();
            RN.Turmas.PeriodoFrequenciaAluno rnPeriodoLancamento = new Techne.Lyceum.RN.Turmas.PeriodoFrequenciaAluno();

            periodoLancamento.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoLancamento.Mes = e.NewValues["MES"] != null ? Convert.ToInt32(e.NewValues["MES"]) : -1;
            periodoLancamento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodoLancamento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodoLancamento.UsuarioId = User.Identity.Name;
            periodoLancamento.PeriodoFrequenciaAlunoId = Convert.ToInt32(e.Keys["PERIODOFREQUENCIAALUNOID"]);


            validacao = rnPeriodoLancamento.Valida(periodoLancamento, true);

            if (validacao.Valido)
            {
                rnPeriodoLancamento.Atualiza(periodoLancamento);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodo.DataBind();
        }

        protected void grdPeriodo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.PeriodoFrequenciaAluno rnPeriodoLancamento = new Techne.Lyceum.RN.Turmas.PeriodoFrequenciaAluno();
            int PeriodoFrequenciaAlunoId = 0;

            PeriodoFrequenciaAlunoId = Convert.ToInt32(e.Keys["PERIODOFREQUENCIAALUNOID"]);


            rnPeriodoLancamento.Remove(PeriodoFrequenciaAlunoId);
            grdPeriodo.DataBind();

        }

        protected void grdPeriodo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPeriodo.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PeriodoFrequenciaAlunoID")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdPeriodo.IsEditing)
            {
                if ((e.Column.FieldName) == "PeriodoFrequenciaAlunoID")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "MES")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }    
    }
}
