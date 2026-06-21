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
     NavUrl("~/Basico/PeriodoAlteracaoCadAluno.aspx"),
     ControlText("PeriodoAlteracaoCadAluno"),
     Title("Período Alteracao Cad Aluno"),
    ]

    public partial class PeriodoAlteracaoCadAluno : TPage
    {
        public object Lista()
        {
            RN.RecursosHumanos.PeriodoAlteracaoAluno rnPeriodoAlteracao = new Techne.Lyceum.RN.RecursosHumanos.PeriodoAlteracaoAluno();
            return rnPeriodoAlteracao.Lista();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodo, "Períodos Alteração dos Dados Cadastrais do Aluno");
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

        public void Insert(object ANO, object DATAINICIO, object DATAFIM, object USUARIOID)
        {
        }

        public void Update(object ANO, object DATAINICIO, object DATAFIM, object USUARIOID, object PERIODOALTERACAOALUNOID)
        {
        }

        public void Delete(object PERIODOALTERACAOALUNOID)
        {
        }

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
            RN.RecursosHumanos.Entidades.PeriodoAlteracaoAluno periodoAlteracao = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PeriodoAlteracaoAluno();
            RN.RecursosHumanos.PeriodoAlteracaoAluno rnPeriodoAlteracao = new Techne.Lyceum.RN.RecursosHumanos.PeriodoAlteracaoAluno();

            periodoAlteracao.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoAlteracao.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodoAlteracao.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodoAlteracao.UsuarioId = User.Identity.Name;

            validacao = rnPeriodoAlteracao.Valida(periodoAlteracao, true);

            if (validacao.Valido)
            {
                rnPeriodoAlteracao.Insere(periodoAlteracao);
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
            RN.RecursosHumanos.Entidades.PeriodoAlteracaoAluno periodoAlteracao = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PeriodoAlteracaoAluno();
            RN.RecursosHumanos.PeriodoAlteracaoAluno rnPeriodoAlteracao = new Techne.Lyceum.RN.RecursosHumanos.PeriodoAlteracaoAluno();

            periodoAlteracao.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            periodoAlteracao.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            periodoAlteracao.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            periodoAlteracao.UsuarioId = User.Identity.Name;
            periodoAlteracao.PeriodoAlteracaoAlunoId = Convert.ToInt32(e.Keys["PERIODOALTERACAOALUNOID"]);

            validacao = rnPeriodoAlteracao.Valida(periodoAlteracao, false);

            if (validacao.Valido)
            {
                rnPeriodoAlteracao.Atualiza(periodoAlteracao);
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
            RN.RecursosHumanos.PeriodoAlteracaoAluno rnPeriodoAlteracao = new Techne.Lyceum.RN.RecursosHumanos.PeriodoAlteracaoAluno();
            int PeriodoAlteracaoAlunoId = 0;

            PeriodoAlteracaoAlunoId = Convert.ToInt32(e.Keys["PERIODOALTERACAOALUNOID"]);

            rnPeriodoAlteracao.Remove(PeriodoAlteracaoAlunoId);
            grdPeriodo.DataBind();
        }

        protected void grdPeriodo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
           
            if (grdPeriodo.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PERIODOALTERACAOALUNOID")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "USUARIOID")
                {
                    e.Editor.ReadOnly = true;
                }
            }
            else if (grdPeriodo.IsEditing)
            {
                if ((e.Column.FieldName) == "PERIODOALTERACAOALUNOID")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "DATAINICIO")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "USUARIOID")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }
    }
}
