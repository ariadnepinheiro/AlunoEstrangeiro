using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using Techne.Controls;
using Techne.Web;
using Techne.Data;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/SubPeriodoLetivo.aspx"),
      ControlText("SubPeriodoLetivo"),
      Title("Período Letivo"),
    ]

    public partial class SubPeriodoLetivo : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSubPeriodoLetivo, "Períodos Letivos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSubPeriodoLetivo);
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

        protected void grdSubPeriodoLetivo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSubPeriodoLetivo);
        }

        protected void grdSubPeriodoLetivo_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string[] anoperiodo = Convert.ToString(e.GetListSourceFieldValue("anoperiodo")).Split('-');
                string subPeriodo = Convert.ToString(e.GetListSourceFieldValue("subperiodo"));
                e.Value = anoperiodo[0].Trim() + "-" + anoperiodo[1].Trim() + "-" + subPeriodo;
            }
            if (e.Column.FieldName == "anoperiodo")
            {
                string ano = Convert.ToString(e.GetListSourceFieldValue("ano"));
                string periodo = Convert.ToString(e.GetListSourceFieldValue("periodo"));
                e.Value = ano + " - " + periodo;
            }
        }

        protected void grdSubPeriodoLetivo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("ano", chaves[0]);
            e.Keys.Add("periodo", chaves[1]);
            e.Keys.Add("subperiodo", chaves[2]);

            string[] anoperiodo = e.NewValues["anoperiodo"].ToString().Split('-');
            e.NewValues.Add("ano", anoperiodo[0].Trim());
            e.NewValues.Add("periodo", anoperiodo[1].Trim());
            e.NewValues.Remove("anoperiodo");
        }

        protected void grdSubPeriodoLetivo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            string[] anoperiodo = e.Values["anoperiodo"].ToString().Split('-');

            e.Keys.Add("ano", anoperiodo[0].Trim());
            e.Keys.Add("periodo", anoperiodo[1].Trim());
            e.Keys.Add("subperiodo", e.Values["subperiodo"]);
        }

        protected void grdSubPeriodoLetivo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdSubPeriodoLetivo.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ano")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "periodo")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "subperiodo")
                    e.Editor.Enabled = true;

            }
            else if (grdSubPeriodoLetivo.IsEditing)
            {
                if ((e.Column.FieldName) == "ano")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "periodo")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "subperiodo")
                    e.Editor.Enabled = false;

            }

            // filtrando uma combo da grid pela outra
            if (!grdSubPeriodoLetivo.IsEditing || e.Column.FieldName != "periodo")
                return;
            DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
            combo.Callback += new CallbackEventHandlerBase(cmbPeriodo_OnCallback);

        }

        protected void FillPeriodoCombo(DevExpress.Web.ASPxEditors.ASPxComboBox cmb, string ano)
        {
            if (string.IsNullOrEmpty(ano)) return;

            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();

            cmb.Items.Clear();

            cmb.DataSource = rnPeriodoLetivo.ListaPeriodosletivosPor(Convert.ToInt32(ano));
            cmb.DataBind();
        }

        private void cmbPeriodo_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillPeriodoCombo(source as DevExpress.Web.ASPxEditors.ASPxComboBox, e.Parameter);
        }


        protected void grdSubPeriodoLetivo_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //if (!string.IsNullOrEmpty(e.NewValues["dias_letivos"].ToString()))
            //{
            //    if (e.NewValues["dias_letivos"].ToString() == " " || e.NewValues["dias_letivos"].ToString() == "  " || e.NewValues["dias_letivos"].ToString() == "   ")
            //    {
            //        e.NewValues["dias_letivos"] = "";
            //    }
            //}
            string[] anoperiodo = e.NewValues["anoperiodo"].ToString().Split('-');

            Techne.Lyceum.RN.RetValue valorRetorno = null;
            if (!e.IsNewRow)
                valorRetorno = RN.PeriodoLetivo.ValidarDatas(Convert.ToDateTime(e.NewValues["dt_inicio"]), Convert.ToDateTime(e.NewValues["dt_fim"]), e.Keys["CompositeKey"].ToString().Split('-')[0], e.Keys["CompositeKey"].ToString().Split('-')[1]);
            else
                valorRetorno = RN.PeriodoLetivo.ValidarDatas(Convert.ToDateTime(e.NewValues["dt_inicio"]), Convert.ToDateTime(e.NewValues["dt_fim"]), anoperiodo[0].Trim(), anoperiodo[1].Trim());

            if (valorRetorno != null)
            {
                if (!valorRetorno.Ok)
                    e.RowError = valorRetorno.Errors.ToString();
            }
            if (Convert.ToDateTime(e.NewValues["dt_inicio"]) > Convert.ToDateTime(e.NewValues["dt_lancamento"]))
            {
                e.RowError = "A data de lançamento do subperíodo não pode ser menor que a data de início do subperíodo.";
                return;
            }
            if (Convert.ToDateTime(e.NewValues["dt_inicio"]) > Convert.ToDateTime(e.NewValues["dt_curriculo_minimo"]))
            {
                e.RowError = "A data de lançamento do currículo mínimo não pode ser menor que a data de início do subperíodo.";
                return;
            }
        }

        protected void grdSubPeriodoLetivo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSubPeriodoLetivo.Settings.ShowFilterRow = false;
        }

        protected void grdSubPeriodoLetivo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdSubPeriodoLetivo.Settings.ShowFilterRow = false;
        }

        protected void grdSubPeriodoLetivo_BeforeGetCallbackResult(object sender, EventArgs e)
        {

        }

        protected void grdSubPeriodoLetivo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //string[] anoperiodo = e.NewValues["anoperiodo"].ToString().Split('-');
            //e.NewValues.Add("ano", anoperiodo[0].Trim());
            //e.NewValues.Add("periodo", anoperiodo[1].Trim());
            //e.NewValues.Remove("anoperiodo");
        }

        protected void odsSubPeriodoLetivo_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();
            RN.SubperiodoLetivo rnSubperiodoLetivo = new Techne.Lyceum.RN.SubperiodoLetivo();
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RetValue retorno = null;
            List<string> mensagens = new List<string>();

            CR.Ly_subperiodo_letivo dtSubPeriodo = new Techne.Lyceum.CR.Ly_subperiodo_letivo();

            Techne.Lyceum.CR.Ly_subperiodo_letivo.Row dadosSubPeriodo = dtSubPeriodo.NewRow();

            dadosSubPeriodo.Ano = Convert.ToDecimal(e.InputParameters["ano"]);
            dadosSubPeriodo.Periodo = Convert.ToDecimal(e.InputParameters["periodo"]);
            dadosSubPeriodo.Subperiodo = Convert.ToDecimal(e.InputParameters["subperiodo"]);
            dadosSubPeriodo.Descricao = e.InputParameters["descricao"].ToString();
            dadosSubPeriodo.Dt_inicio = Convert.ToDateTime(e.InputParameters["dt_inicio"]);
            dadosSubPeriodo.Dt_fim = Convert.ToDateTime(e.InputParameters["dt_fim"]);
            dadosSubPeriodo.Dias_letivos = Convert.ToInt32(e.InputParameters["dias_letivos"]);
            dadosSubPeriodo.Dt_lancamento = Convert.ToDateTime(e.InputParameters["DT_LANCAMENTO"]);
            dadosSubPeriodo.Dt_curriculo_minimo = Convert.ToDateTime(e.InputParameters["DT_CURRICULO_MINIMO"]);
            dadosSubPeriodo.Dt_limite_frequencia = Convert.ToDateTime(e.InputParameters["DT_LIMITE_FREQUENCIA"]);

            dtSubPeriodo.Rows.Add(dadosSubPeriodo);

            if (dadosSubPeriodo.Ano > 0 && dadosSubPeriodo.Periodo >= 0)
            {
                if (dadosSubPeriodo.Dt_inicio > DateTime.MinValue && dadosSubPeriodo.Dt_fim > DateTime.MinValue)
                {

                    if (rnSubperiodoLetivo.PossuiOutroSubPeriodoIntercaladoPor(Convert.ToInt32(dadosSubPeriodo.Ano), Convert.ToInt32(dadosSubPeriodo.Periodo), Convert.ToInt32(dadosSubPeriodo.Subperiodo), Convert.ToDateTime(dadosSubPeriodo.Dt_inicio), Convert.ToDateTime(dadosSubPeriodo.Dt_fim)))
                    {

                        mensagens.Add("DATA DE INÍCIO e FIM não podem intercalar com outro período.");
                    }
                }
                else
                {
                    mensagens.Add("Os campos DATA INÍCIO e DATA FIM é de preenchimento obrigatório");
                }
            }
            else
            {
                mensagens.Add("Os campos ANO E PERÍODO é de preenchimento obrigatório");

            }

            if (mensagens.Count == 0)
            {
                retorno = RN.PeriodoLetivo.AlterarSubPeriodo(dtSubPeriodo);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                        throw new Exception(retorno.Errors.ToString());
                }

            }
            else
            {
                validacao.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);

                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));

            }
        }

        protected void odsSubPeriodoLetivo_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();
            RN.SubperiodoLetivo rnSubperiodoLetivo = new Techne.Lyceum.RN.SubperiodoLetivo();
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RetValue retorno = null;
            List<string> mensagens = new List<string>();

            CR.Ly_subperiodo_letivo dtSubPeriodo = new Techne.Lyceum.CR.Ly_subperiodo_letivo();

            Techne.Lyceum.CR.Ly_subperiodo_letivo.Row dadosSubPeriodo = dtSubPeriodo.NewRow();
            string[] anoperiodo = e.InputParameters["anoperiodo"].ToString().Split('-');

            dadosSubPeriodo.Ano = Convert.ToDecimal(anoperiodo[0].Trim());
            dadosSubPeriodo.Periodo = Convert.ToDecimal(anoperiodo[1].Trim());
            dadosSubPeriodo.Subperiodo = Convert.ToDecimal(e.InputParameters["subperiodo"]);
            dadosSubPeriodo.Descricao = e.InputParameters["descricao"].ToString();
            dadosSubPeriodo.Dt_inicio = Convert.ToDateTime(e.InputParameters["dt_inicio"]);
            dadosSubPeriodo.Dt_fim = Convert.ToDateTime(e.InputParameters["dt_fim"]);
            dadosSubPeriodo.Dias_letivos = Convert.ToInt32(e.InputParameters["dias_letivos"]);
            dadosSubPeriodo.Dt_lancamento = Convert.ToDateTime(e.InputParameters["DT_LANCAMENTO"]);
            dadosSubPeriodo.Dt_curriculo_minimo = Convert.ToDateTime(e.InputParameters["DT_CURRICULO_MINIMO"]);
            dadosSubPeriodo.Dt_limite_frequencia = Convert.ToDateTime(e.InputParameters["DT_LIMITE_FREQUENCIA"]);

            dtSubPeriodo.Rows.Add(dadosSubPeriodo);

            if (dadosSubPeriodo.Ano > 0 && dadosSubPeriodo.Periodo >= 0)
            {
                int qtde = rnPeriodoLetivo.ObtemQtdeSubPeriodo(Convert.ToInt32(anoperiodo[0].Trim()), Convert.ToInt32(anoperiodo[1].Trim()));

                int total = rnPeriodoLetivo.ObtemTotalSubPeriodo(Convert.ToInt32(anoperiodo[0].Trim()), Convert.ToInt32(anoperiodo[1].Trim()));

                if (!((total + 1) <= qtde))
                {
                    mensagens.Add("Não é possível criar um novo Periodo Letivo pois ultrapassa a quantidade de ciclos avaliativos(" + qtde + ")");
                }

                if (dadosSubPeriodo.Dt_inicio > DateTime.MinValue && dadosSubPeriodo.Dt_fim > DateTime.MinValue)
                {

                    if (rnSubperiodoLetivo.PossuiOutroSubPeriodoIntercaladoPor(Convert.ToInt32(anoperiodo[0].Trim()), Convert.ToInt32(anoperiodo[1].Trim()), Convert.ToInt32(dadosSubPeriodo.Subperiodo), Convert.ToDateTime(dadosSubPeriodo.Dt_inicio), Convert.ToDateTime(dadosSubPeriodo.Dt_fim)))
                    {

                        mensagens.Add("DATA DE INÍCIO e FIM não podem intercalar com outro período.");
                    }
                }
                else
                {
                    mensagens.Add("Os campos DATA INÍCIO e DATA FIM é de preenchimento obrigatório");
                }
            }
            else
            {
                mensagens.Add("Os campos ANO E PERÍODO é de preenchimento obrigatório");

            }

            if (mensagens.Count == 0)
            {
                retorno = RN.PeriodoLetivo.IncluirSubPeriodo(dtSubPeriodo);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                        throw new Exception(retorno.Errors.ToString());
                }
            }
            else
            {
                validacao.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);

                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void odsSubPeriodoLetivo_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();
            RN.SubperiodoLetivo rnSubperiodoLetivo = new Techne.Lyceum.RN.SubperiodoLetivo();
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RetValue retorno = null;
            List<string> mensagens = new List<string>();

            CR.Ly_subperiodo_letivo dtSubPeriodo = new Techne.Lyceum.CR.Ly_subperiodo_letivo();

            Techne.Lyceum.CR.Ly_subperiodo_letivo.Row dadosSubPeriodo = dtSubPeriodo.NewRow();

            dadosSubPeriodo.Ano = Convert.ToDecimal(e.InputParameters["ano"]);
            dadosSubPeriodo.Periodo = Convert.ToDecimal(e.InputParameters["periodo"]);
            dadosSubPeriodo.Subperiodo = Convert.ToDecimal(e.InputParameters["subperiodo"]);

            dtSubPeriodo.Rows.Add(dadosSubPeriodo);

            if (rnSubperiodoLetivo.PossuiCompetenciaHabilidadePor(Convert.ToInt32(dadosSubPeriodo.Ano), Convert.ToInt32(dadosSubPeriodo.Periodo), Convert.ToInt32(dadosSubPeriodo.Subperiodo)))
            {

                mensagens.Add("Este periodo letivo não pode ser excluído pois possui COMPETÊNCIAS/HABILIDADES vinculado.");
            }
            if (rnSubperiodoLetivo.PossuiGrupoCompetenciaHabilidadePor(Convert.ToInt32(dadosSubPeriodo.Ano), Convert.ToInt32(dadosSubPeriodo.Periodo), Convert.ToInt32(dadosSubPeriodo.Subperiodo)))
            {

                mensagens.Add("Este periodo letivo não pode ser excluído pois possui GRUPO DE COMPETÊNCIAS/HABILIDADEs vinculado.");
            }
            if (rnSubperiodoLetivo.PossuiMaterialEstudoPor(Convert.ToInt32(dadosSubPeriodo.Ano), Convert.ToInt32(dadosSubPeriodo.Periodo), Convert.ToInt32(dadosSubPeriodo.Subperiodo)))
            {

                mensagens.Add("Este periodo letivo não pode ser excluído pois possui MATERIAL DE ESTUDO vinculado.");
            }

            if (mensagens.Count == 0)
            {

                retorno = RN.PeriodoLetivo.ExcluirSubPeriodo(dtSubPeriodo);

                if (retorno != null)
                {
                    if (!retorno.Ok)
                        throw new Exception(retorno.Errors.ToString());
                }
            }
            else
            {
                e.Cancel = true;
                validacao.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
                
                throw new Exception(validacao.Mensagem);             
            }
        }
    }
}
