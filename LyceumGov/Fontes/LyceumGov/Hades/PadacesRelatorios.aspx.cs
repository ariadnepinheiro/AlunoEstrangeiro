using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
	[NavUrl("~/Hades/PadacesRelatorios.aspx"),
	ControlText("Padrões de Acesso"),
	Title("Padrões de Acesso"),]

    public partial class PadacesRelatorios : TPage
    {
		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdRelatorios, "Padrões de Acesso - Relatórios");
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdRelatorios);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    lblInvisible.Text = decodedText;
                    lblPadaces.Text = ObterPadacesQueryString(decodedText);
                }
                else
                    Response.Redirect("PadroesdeAcesso.aspx");
            }
        }

        protected void grdRelatorios_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('|');

            e.Keys.Clear();
            e.Keys.Add("padaces", chaves[0]);
            e.Keys.Add("sis", chaves[1]);
            e.Keys.Add("gruporelat", chaves[2]);
            e.Keys.Add("relatorio", chaves[3]);
        }

        protected void grdRelatorios_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["padaces"] = lblPadaces.Text;
            e.NewValues["sis"] = "LyceumNet";
        }

        protected void grdRelatorios_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            //verificar existencia do padrão de acesso para poder inserir os relatórios
            if (!string.IsNullOrEmpty(Convert.ToString(e.NewValues["padaces"])))
            {
                string padaces = e.NewValues["padaces"].ToString();
                if (!string.IsNullOrEmpty(padaces))
                {
                    bool podeInserir = RN.PadroesDeAcessos.VerificarPadaces(padaces);
                    if (!podeInserir)
                    {
                        //AvisaErro();
                        ViewState["erro"] = "Verdadeiro";
                        e.RowError = "O padrão de acesso no qual se quer inserir um relatório foi excluido. Operação não realizada.";
                        grdRelatorios.CancelEdit();
                    }
                    else
                        ViewState["erro"] = "Falso";
                }
            }
        }

        protected void grdRelatorios_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('|');

            e.Keys.Clear();
            e.Keys.Add("padaces", chaves[0]);
            e.Keys.Add("sis", chaves[1]);
            e.Keys.Add("gruporelat", chaves[2]);
            e.Keys.Add("relatorio", chaves[3]);
        }

        protected void grdRelatorios_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {

        }

        protected void grdRelatorios_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string padaces = Convert.ToString(e.GetListSourceFieldValue("padaces"));
                string sis = Convert.ToString(e.GetListSourceFieldValue("sis"));
                string grupo = Convert.ToString(e.GetListSourceFieldValue("gruporelat"));
                string relatorio = Convert.ToString(e.GetListSourceFieldValue("relatorio"));
                e.Value = padaces + "|" + sis + "|" + grupo + "|" + relatorio;
            }
        }

        protected void grdRelatorios_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRelatorios.Settings.ShowFilterRow = false;
            e.NewValues["padaces"] = lblPadaces.Text;
        }

        protected void grdRelatorios_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdRelatorios.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "gruporelat")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "relatorio")
                    e.Editor.Enabled = true;
            }

            if (!grdRelatorios.IsEditing || e.Column.FieldName != "relatorio")
                return;

            DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
            combo.Callback += new CallbackEventHandlerBase(cmbRelat_OnCallback);
        }

        private void cmbRelat_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillComboRelat(source as DevExpress.Web.ASPxEditors.ASPxComboBox, e.Parameter);
        }

        protected void FillComboRelat(DevExpress.Web.ASPxEditors.ASPxComboBox cmbRelat, string grupoRelat)
        {
            if (string.IsNullOrEmpty(grupoRelat))
            { 
                tdsComboRelat.SqlWhere = "Hd_relatorio.gruporelat = ''"; 
                return; 
            }

            tdsComboRelat.SqlWhere = "Hd_relatorio.sis = 'LyceumNet' and Hd_relatorio.gruporelat = '" + RN.RNBase.MudarAspas(grupoRelat) + "'";

            cmbRelat.Items.Clear();
            cmbRelat.DataSource = tdsComboRelat.Select();
            cmbRelat.DataBind();

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            string padaces = lblInvisible.Text;
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(padaces);
            Response.Redirect("PadroesdeAcesso.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        private string ObterPadacesQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string padaces = string.Empty;
            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("value") >= 0)
                    padaces = dados.Substring(dados.LastIndexOf('=') + 1);
            }
            return padaces;
        }

        protected void grdRelatorios_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRelatorios);
        }

    }
}
