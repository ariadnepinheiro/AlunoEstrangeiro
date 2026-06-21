using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Basico
{

    [NavUrl("~/Basico/PeriodoLetivo.aspx"),
      ControlText("Periodo Letivo"),
      Title("Ano Letivo")]
    public partial class PeriodoLetivo : TPage
    {
        public object Lista()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            return rnPeriodoLetivo.Lista();
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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPeriodoLetivo, "Anos Letivos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPeriodoLetivo);
        }

        protected void grdPeriodoLetivo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPeriodoLetivo);
        }

        protected void grdPeriodoLetivo_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string ano = Convert.ToString(e.GetListSourceFieldValue("ano"));
                string periodo = Convert.ToString(e.GetListSourceFieldValue("periodo"));
                e.Value = ano + "-" + periodo;
            }
        }

        protected void grdPeriodoLetivo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPeriodoLetivo.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ano")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "periodo")
                    e.Editor.Enabled = true;
            }
            else if (grdPeriodoLetivo.IsEditing)
            {
                if ((e.Column.FieldName) == "ano")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "periodo")
                    e.Editor.Enabled = false;

                if (e.Column.FieldName == "dt_fim")
                {
                    if (!PossuiTurma((DevExpress.Web.ASPxGridView.ASPxGridView)sender, e))
                    {
                        e.Editor.ToolTip = String.Empty;
                        e.Editor.ClientEnabled = true;
                    }
                    else
                    {
                        e.Editor.ClientEnabled = false;
                        e.Editor.ToolTip = "Não é possível alterar este campo, pois existe turma para este ano/período.";
                    }
                }

            }

        }
        private bool PossuiTurma(DevExpress.Web.ASPxGridView.ASPxGridView grid, ASPxGridViewEditorEventArgs e)
        {
            RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();

            decimal ano = Convert.ToDecimal(grid.GetRowValues(e.VisibleIndex, "ano"));
            decimal periodo = Convert.ToDecimal(grid.GetRowValues(e.VisibleIndex, "periodo"));

            return rnTurma.PossuiTurmaAbertaPor(ano, periodo);
        }

        protected void grdPeriodoLetivo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPeriodoLetivo.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoLetivo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPeriodoLetivo.Settings.ShowFilterRow = false;
        }

        protected void grdPeriodoLetivo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Entidades.LyPeriodoLetivo periodo = new Techne.Lyceum.RN.Entidades.LyPeriodoLetivo();
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            periodo.Ano = e.NewValues["ano"] != null ? Convert.ToDecimal(e.NewValues["ano"]) : -1;
            periodo.Periodo = e.NewValues["periodo"] != null ? Convert.ToDecimal(e.NewValues["periodo"]) : -1;
            periodo.DtInicio = e.NewValues["dt_inicio"] != null ? Convert.ToDateTime(e.NewValues["dt_inicio"]) : (DateTime?)null;
            periodo.DtFim = e.NewValues["dt_fim"] != null ? Convert.ToDateTime(e.NewValues["dt_fim"]) : (DateTime?)null;
            periodo.DtInicioAula = e.NewValues["dt_inicio_aula"] != null ? Convert.ToDateTime(e.NewValues["dt_inicio_aula"]) : (DateTime?)null;
            periodo.DtFimAula = e.NewValues["dt_fim_aula"] != null ? Convert.ToDateTime(e.NewValues["dt_fim_aula"]) : (DateTime?)null;
            periodo.DataInicioDocente = e.NewValues["data_inicio_docente"] != null ? Convert.ToDateTime(e.NewValues["data_inicio_docente"]) : (DateTime?)null;
            periodo.DataFimDocente = e.NewValues["data_fim_docente"] != null ? Convert.ToDateTime(e.NewValues["data_fim_docente"]) : (DateTime?)null;
            periodo.DataInicioIndicacaoEletiva = e.NewValues["data_inicio_indicacao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_inicio_indicacao_eletiva"]) : (DateTime?)null;
            periodo.DataFimIndicacaoEletiva = e.NewValues["data_fim_indicacao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_fim_indicacao_eletiva"]) : (DateTime?)null;
            periodo.DataInicioDistribuicaoEletiva = e.NewValues["data_inicio_distribuicao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_inicio_distribuicao_eletiva"]) : (DateTime?)null;
            periodo.DataFimDistribuicaoEletiva = e.NewValues["data_fim_distribuicao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_fim_distribuicao_eletiva"]) : (DateTime?)null;
            periodo.Descricao = e.NewValues["descricao"] != null ? e.NewValues["descricao"].ToString().Trim().ToUpper() : null;
            periodo.PerAno = e.NewValues["per_ano"] != null ? Convert.ToDecimal(e.NewValues["per_ano"]) : (decimal?)null;
            periodo.PerPeriodo = e.NewValues["per_periodo"] != null ? Convert.ToDecimal(e.NewValues["per_periodo"]) : (decimal?)null;
            periodo.QtdeSubperiodo = e.NewValues["qtde_subperiodo"] != null ? Convert.ToInt32(e.NewValues["qtde_subperiodo"]) : -1;
            periodo.UsuarioId = User.Identity.Name;


            validacao = rnPeriodoLetivo.Valida(periodo, true);

            if (validacao.Valido)
            {
                rnPeriodoLetivo.Insere(periodo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoLetivo.DataBind();

        }

        protected void grdPeriodoLetivo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Entidades.LyPeriodoLetivo periodo = new Techne.Lyceum.RN.Entidades.LyPeriodoLetivo();
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');

            periodo.Ano = chaves[0] != null ? Convert.ToDecimal(chaves[0]) : -1;
            periodo.Periodo = chaves[1] != null ? Convert.ToDecimal(chaves[1]) : -1;
            periodo.DtInicio = e.NewValues["dt_inicio"] != null ? Convert.ToDateTime(e.NewValues["dt_inicio"]) : (DateTime?)null;
            periodo.DtFim = e.NewValues["dt_fim"] != null ? Convert.ToDateTime(e.NewValues["dt_fim"]) : (DateTime?)null;
            periodo.DtInicioAula = e.NewValues["dt_inicio_aula"] != null ? Convert.ToDateTime(e.NewValues["dt_inicio_aula"]) : (DateTime?)null;
            periodo.DtFimAula = e.NewValues["dt_fim_aula"] != null ? Convert.ToDateTime(e.NewValues["dt_fim_aula"]) : (DateTime?)null;
            periodo.DataInicioDocente = e.NewValues["data_inicio_docente"] != null ? Convert.ToDateTime(e.NewValues["data_inicio_docente"]) : (DateTime?)null;
            periodo.DataFimDocente = e.NewValues["data_fim_docente"] != null ? Convert.ToDateTime(e.NewValues["data_fim_docente"]) : (DateTime?)null;
            periodo.DataInicioIndicacaoEletiva = e.NewValues["data_inicio_indicacao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_inicio_indicacao_eletiva"]) : (DateTime?)null;
            periodo.DataFimIndicacaoEletiva = e.NewValues["data_fim_indicacao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_fim_indicacao_eletiva"]) : (DateTime?)null;
            periodo.DataInicioDistribuicaoEletiva = e.NewValues["data_inicio_distribuicao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_inicio_distribuicao_eletiva"]) : (DateTime?)null;
            periodo.DataFimDistribuicaoEletiva = e.NewValues["data_fim_distribuicao_eletiva"] != null ? Convert.ToDateTime(e.NewValues["data_fim_distribuicao_eletiva"]) : (DateTime?)null;
            periodo.Descricao = e.NewValues["descricao"] != null ? e.NewValues["descricao"].ToString().Trim().ToUpper() : null;
            periodo.PerAno = e.NewValues["per_ano"] != null ? Convert.ToDecimal(e.NewValues["per_ano"]) : (decimal?)null;
            periodo.PerPeriodo = e.NewValues["per_periodo"] != null ? Convert.ToDecimal(e.NewValues["per_periodo"]) : (decimal?)null;
            periodo.QtdeSubperiodo = e.NewValues["qtde_subperiodo"] != null ? Convert.ToInt32(e.NewValues["qtde_subperiodo"]) : -1;
            periodo.UsuarioId = User.Identity.Name;


            validacao = rnPeriodoLetivo.Valida(periodo, false);

            if (validacao.Valido)
            {
                rnPeriodoLetivo.Atualiza(periodo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdPeriodoLetivo.DataBind();
        }

        protected void grdPeriodoLetivo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');

            validacao = rnPeriodoLetivo.ValidaRemocao(Convert.ToDecimal(chaves[0]), Convert.ToDecimal(chaves[1]));

            if (validacao.Valido)
            {
                rnPeriodoLetivo.Remove(Convert.ToDecimal(chaves[0]), Convert.ToDecimal(chaves[1]));
                grdPeriodoLetivo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void Update(object dt_inicio, object dt_fim, object dt_inicio_aula, object dt_fim_aula, object data_inicio_docente, object data_fim_docente, object data_inicio_indicacao_eletiva, object data_fim_indicacao_eletiva, object data_inicio_distribuicao_eletiva, object data_fim_distribuicao_eletiva, object descricao, object per_ano, object per_periodo, object qtde_subperiodo, object CompositeKey)
        { }

        public void Insert(object ano,object periodo,object dt_inicio, object dt_fim, object dt_inicio_aula, object dt_fim_aula, object data_inicio_docente, object data_fim_docente, object data_inicio_indicacao_eletiva, object data_fim_indicacao_eletiva, object data_inicio_distribuicao_eletiva, object data_fim_distribuicao_eletiva, object descricao, object per_ano, object per_periodo,object qtde_subperiodo )
        { }

        public void Delete(object compositeKey)
        { }
    }
}
