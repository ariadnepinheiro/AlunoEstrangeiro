using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Techne.Web;
using System.Configuration;
using Techne.Lyceum.RN;
using System.Collections.Specialized;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using Techne.Data;
using System.Drawing;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
	[
	NavUrl("~/ProcessoSeletivo/ListaCandidatos.aspx"),
	Title("Lista de Candidatos")
	]
	public partial class ListaCandidatos : TPage
	{
		#region Código Padrão Techne
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
		public static string GetUrl()
		{
			#region Código gerado Techne
			return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
			#endregion
		}
		#endregion

		#region Eventos da Página
		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdListaCandidatos, "Lista de Candidatos");
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!tseConcursoBusca.IsValidDBValue && tseConcursoBusca.DBValue.IsNull)
				pnListaCandidatos.Visible = false;
			if (tseConcursoBusca.IsValidDBValue && !tseConcursoBusca.DBValue.IsNull &&
				tseCoordenadoriaBusca.IsValidDBValue && !tseCoordenadoriaBusca.DBValue.IsNull &&
				cmbCargoBusca.SelectedItem != null &&
				tseDisciplinaBusca.IsValidDBValue && !tseDisciplinaBusca.DBValue.IsNull)
			{
				IDictionary<string, string> pares = new Dictionary<string, string>();
				pares.Add("concurso", tseConcursoBusca.DBValue.ToString());
				pares.Add("nucleo", tseCoordenadoriaBusca.DBValue.ToString());
				pares.Add("categoria", cmbCargoBusca.SelectedItem.Value.ToString());
				pares.Add("disciplina", tseDisciplinaBusca.DBValue.ToString());
				if (tseNomeCandidatoBusca.IsValidDBValue && !tseNomeCandidatoBusca.DBValue.IsNull)
				{
					pares.Add("candidato", string.Empty);
					btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rscandidatos&grp=dol&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
				}
				else
				{
					pares.Add("candidato", tseNomeCandidatoBusca.DBValue.ToString());
					btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rscandidatos&grp=dol&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
				}
			}
		}

		protected void Page_PreRenderComplete(object sender, EventArgs e)
		{
		}
		#endregion

		#region Eventos
		protected void tseConcursoBusca_Changed(object sender, EventArgs args)
		{
			if (tseConcursoBusca.IsValidDBValue && !tseConcursoBusca.DBValue.IsNull)
			{
				tseCoordenadoriaBusca.Enabled = true;
				tseCoordenadoriaBusca.ResetValue();
				cmbCargoBusca.Enabled = false;
				cmbCargoBusca.Items.Clear();
				cmbCargoBusca.SelectedIndex = -1;
				tseDisciplinaBusca.ResetValue();
				tseDisciplinaBusca.SqlWhere = string.Empty;
				tseDisciplinaBusca.DataBind();
				tseNomeCandidatoBusca.Mode = ControlMode.View;
				tseNomeCandidatoBusca.ResetValue();
				tseNomeCandidatoBusca.SqlWhere = string.Empty;
				tseNomeCandidatoBusca.DataBind();
			}
			else
			{
				tseCoordenadoriaBusca.Enabled = false;
				cmbCargoBusca.Enabled = false;
				cmbCargoBusca.Items.Clear();
				cmbCargoBusca.SelectedIndex = -1;
				tseDisciplinaBusca.Mode = ControlMode.View;
				tseDisciplinaBusca.ResetValue();
				tseDisciplinaBusca.SqlWhere = string.Empty;
				tseDisciplinaBusca.DataBind();
				tseNomeCandidatoBusca.Mode = ControlMode.View;
				tseNomeCandidatoBusca.ResetValue();
				tseNomeCandidatoBusca.SqlWhere = string.Empty;
				tseNomeCandidatoBusca.DataBind();
			}
			lblMensagem.Text = string.Empty;
			btnImprimir.Visible = false;
			pnListaCandidatos.Visible = false;
		}

		protected void cmbCargoBusca_Changed(object sender, EventArgs args)
		{
			if (!cmbCargoBusca.SelectedItem.Value.Equals(string.Empty))
			{
				tseDisciplinaBusca.ResetValue();
				tseDisciplinaBusca.Mode = ControlMode.Edit;
				tseDisciplinaBusca.SqlWhere = "ldh.Concurso = '" + tseConcursoBusca.DBValue.ToString() +
					"' and ldh.nucleo = '" + tseCoordenadoriaBusca.DBValue.ToString() +
					"' and ldh.categoria = '" + cmbCargoBusca.SelectedItem.Value.ToString() + "'";
				tseDisciplinaBusca.DataBind();
				tseNomeCandidatoBusca.ResetValue();
				tseNomeCandidatoBusca.Mode = ControlMode.Edit;
				tseNomeCandidatoBusca.SqlWhere = "concurso = '" + tseConcursoBusca.DBValue.ToString() +
					"' and nucleo = '" + tseCoordenadoriaBusca.DBValue.ToString() +
					"' and categoria = '" + cmbCargoBusca.SelectedItem.Value.ToString() + "'";
				tseNomeCandidatoBusca.DataBind();
			}
			lblMensagem.Text = string.Empty;
			btnImprimir.Visible = false;
			pnListaCandidatos.Visible = false;
		}

		protected void tseDisciplinaBusca_Changed(object sender, EventArgs args)
		{
			pnListaCandidatos.Visible = false;
			if (tseConcursoBusca.IsValidDBValue && !tseConcursoBusca.DBValue.IsNull &&
				tseCoordenadoriaBusca.IsValidDBValue && !tseCoordenadoriaBusca.DBValue.IsNull &&
				!cmbCargoBusca.SelectedItem.Value.Equals(string.Empty) &&
				tseDisciplinaBusca.IsValidDBValue && !tseDisciplinaBusca.DBValue.IsNull)
			{
				tseNomeCandidatoBusca.ResetValue();
				tseNomeCandidatoBusca.Mode = ControlMode.Edit;
				tseNomeCandidatoBusca.SqlWhere = "concurso = '" + tseConcursoBusca.DBValue.ToString() +
					"' and nucleo = '" + tseCoordenadoriaBusca.DBValue.ToString() +
					"' and categoria = '" + cmbCargoBusca.SelectedItem.Value.ToString() +
					"' and agrupamento_ingresso = '" + tseDisciplinaBusca.DBValue.ToString() + "'";
				tseNomeCandidatoBusca.DataBind();
				lblMensagem.Text = string.Empty;
				btnImprimir.Visible = true;
				pnListaCandidatos.Visible = true;
			}
			else
			{
				lblMensagem.Text = "Favor selecionar processo seletivo, coordenadoria, função e disciplina válidos.";
				btnImprimir.Visible = false;
				pnListaCandidatos.Visible = false;
				return;
			}
		}
		#endregion

		#region Grid Lista de Candidatos
		protected void grdListaCandidatos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
		}

		protected void grdListaCandidatos_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
		{
			if (e.Column.FieldName.Equals("funcao"))
			{
				if (e.Value.Equals(RN.ProcessoSeletivo.DOCI_VALUE))
					e.DisplayText = RN.ProcessoSeletivo.DOCI_TEXT;
				if (e.Value.Equals(RN.ProcessoSeletivo.DOCII_VALUE))
					e.DisplayText = RN.ProcessoSeletivo.DOCII_TEXT;
			}
			if (e.Column.FieldName.Equals("situacao"))
			{
				RN.ProcessoSeletivo.StatusPublico status = RN.ProcessoSeletivo.ConverteStatusCandidatoParaStatusCandidatoPublico(((RN.ProcessoSeletivo.Status)Enum.Parse(typeof(RN.ProcessoSeletivo.Status), Convert.ToString(e.Value))));
				if (status.Equals(RN.ProcessoSeletivo.StatusPublico.Aguardando))
					e.DisplayText = "Aguardando";
				if (status.Equals(RN.ProcessoSeletivo.StatusPublico.Aprovado))
					e.DisplayText = "Aprovado";
				if (status.Equals(RN.ProcessoSeletivo.StatusPublico.Reprovado))
					e.DisplayText = "Reprovado";
			}
		}

		protected void grdListaCandidatos_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if (e.DataColumn.FieldName.Equals("situacao"))
			{
				RN.ProcessoSeletivo.StatusPublico status = RN.ProcessoSeletivo.ConverteStatusCandidatoParaStatusCandidatoPublico(((RN.ProcessoSeletivo.Status)Enum.Parse(typeof(RN.ProcessoSeletivo.Status), Convert.ToString(e.CellValue))));
				if (status.Equals(RN.ProcessoSeletivo.StatusPublico.Aguardando))
					e.Cell.ForeColor = Color.Gray;
				if (status.Equals(RN.ProcessoSeletivo.StatusPublico.Aprovado))
					e.Cell.ForeColor = Color.Green;
				if (status.Equals(RN.ProcessoSeletivo.StatusPublico.Reprovado))
					e.Cell.ForeColor = Color.Red;
			}
		}
		#endregion
	}
}

