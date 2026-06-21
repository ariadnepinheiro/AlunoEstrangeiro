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
using System.Web.UI.WebControls;
using System.Collections;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
	[
	NavUrl("~/ProcessoSeletivo/RelatoriosContratos.aspx"),
	Title("Relatório de Contratos Anteriores")
	]
	public partial class RelatoriosContratos : TPage
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
		protected void Page_Load(object sender, EventArgs e)
		{
		}
		#endregion

		#region Eventos
		protected void ddlAnoBusca_SelectedIndexChanged(object sender, EventArgs args)
		{
			if (ddlAnoBusca.SelectedValue.Equals("2008"))
			{
				ddlContratosBusca.Items.Clear();
				ddlContratosBusca.Items.Add(new ListItem("Nenhum", string.Empty, true));
                ddlContratosBusca.Items.Add(new ListItem("E21.10002.2008 - Minuta Contratação Temporária - Docente I_formatado", "E21.10002.2008.Docente.I_formatado", true));
                ddlContratosBusca.Items.Add(new ListItem("E21.10002.2008 - Minuta Contratação Temporária_FBM - Docente I - fracionado_formatado", "E21.10002.2008.Docente.I_fracionado_formatado", true));
                ddlContratosBusca.Items.Add(new ListItem("E21.10002.2008 - Minuta de Contratação Temporária - Docente II_formatado", "E21.10002.2008.Docente.II_formatado", true));
			}
			if (ddlAnoBusca.SelectedValue.Equals("2009"))
			{
				ddlContratosBusca.Items.Clear();
				ddlContratosBusca.Items.Add(new ListItem("Nenhum", string.Empty, true));
                ddlContratosBusca.Items.Add(new ListItem("Minuta Contratação Temporária - Docente I", "2009.Minuta.Docente_I", true));
                ddlContratosBusca.Items.Add(new ListItem("Minuta Contratação Temporária - Docente I - fracionado", "2009.Minuta.Docente.I_fracionado", true));
                ddlContratosBusca.Items.Add(new ListItem("Minuta - Termo Contratual Docente II", "2009.Minuta.Docente.II", true));
                ddlContratosBusca.Items.Add(new ListItem("Termo Aditivo Prorrogação Docente II 09", "TERMO ADITIVO PRORROGAÇÃO DOC II 09  (ALTERAÇÃO ASJUR)", true));
			}            
			if (ddlAnoBusca.SelectedValue.Equals(string.Empty))
			{
				ddlContratosBusca.Items.Clear();
				ddlContratosBusca.Items.Add(new ListItem("Nenhum", string.Empty, true));
			}
			tseConcursoBusca.ResetValue();
			tseConcursoBusca.SqlWhere = "ano = '" + ddlAnoBusca.SelectedValue + "'";
			tseConcursoBusca.DataBind();
			tseCandidatoBusca.ResetValue();
		}

		protected void btnImprimir_Click(object sender, EventArgs args)
		{
            if (!string.IsNullOrEmpty(ddlContratosBusca.SelectedValue))
            {
                Dictionary<string,string> dict = new Dictionary<string,string>();

                dict.Add("xml", ddlContratosBusca.SelectedValue);
                dict.Add("p1", tseConcursoBusca.DBValue.ToString());
                dict.Add("p2", tseCandidatoBusca.DBValue.ToString());
                string chave = CodificaQueryString(dict);

                Response.Redirect("~/Relatorio/RelatorioXML.aspx?" + chave);
            }
            else
                lblMensagem.Text = "Selecione um contrato.";
		}
		#endregion
	}
}



