using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Patrimonio
{
    [
    NavUrl("~/Patrimonio/PeriodoLancamento.aspx"),
     ControlText("PeriodoLancamento"),
     Title("Período Lançamento"),
   ]

    public partial class PeriodoLancamento : TPage
    {
        public object Lista()
        {
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new Techne.Lyceum.RN.Patrimonio.PeriodoLancamento();

            return rnPeriodoLancamento.Lista();

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodoLancamento, "Períodos Lançamento");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPeriodoLancamento);
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

        protected void grdPeriodoLancamento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPeriodoLancamento);
        }


        public void Insert(object ANO, object DATAINICIO, object DATAFIM) { }
        public void Update(object ANO, object DATAINICIO, object DATAFIM, object PERIODOLANCAMENTOID) { }
        public void Delete(object PERIODOLANCAMENTOID) { }

        protected void grdPeriodoLancamento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPeriodoLancamento.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoLancamento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPeriodoLancamento.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoLancamento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.PeriodoLancamento periodoLancamento = new Techne.Lyceum.RN.Patrimonio.Entidades.PeriodoLancamento();
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new Techne.Lyceum.RN.Patrimonio.PeriodoLancamento();

            periodoLancamento.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
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

            grdPeriodoLancamento.DataBind();

        }

        protected void grdPeriodoLancamento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.Entidades.PeriodoLancamento periodoLancamento = new Techne.Lyceum.RN.Patrimonio.Entidades.PeriodoLancamento();
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new Techne.Lyceum.RN.Patrimonio.PeriodoLancamento();

            periodoLancamento.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoLancamento.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodoLancamento.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodoLancamento.UsuarioId = User.Identity.Name;
            periodoLancamento.PeriodoLancamentoId = Convert.ToInt32(e.Keys["PERIODOLANCAMENTOID"]);


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

            grdPeriodoLancamento.DataBind();
        }

        protected void grdPeriodoLancamento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Patrimonio.PeriodoLancamento rnPeriodoLancamento = new Techne.Lyceum.RN.Patrimonio.PeriodoLancamento();
            int periodoLancamentoId = 0;

            periodoLancamentoId = Convert.ToInt32(e.Keys["PERIODOLANCAMENTOID"]);


            rnPeriodoLancamento.Remove(periodoLancamentoId);
            grdPeriodoLancamento.DataBind();

        }

        protected void grdPeriodoLancamento_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPeriodoLancamento.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PERIODOLANCAMENTOID")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdPeriodoLancamento.IsEditing)
            {
                if ((e.Column.FieldName) == "PERIODOLANCAMENTOID")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }    
    }
}
