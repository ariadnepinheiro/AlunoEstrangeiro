using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
	[NavUrl("~/Academico/ProgramasSociais.aspx"),
	ControlText("ProgramasSociais"),
	Title("Programas Sociais"),]
	public partial class ProgramasSociais : TPage
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
			ControlaAcesso(grdProgramasSociais);
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdProgramasSociais, "Programas Sociais");
		}

		#region Métodos da Grid
		protected void grdProgramasSociais_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdProgramasSociais);
		}

		protected void grdProgramasSociais_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
		{
			grdProgramasSociais.Settings.ShowFilterRow = false;
		}

		protected void grdProgramasSociais_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
		{
			grdProgramasSociais.Settings.ShowFilterRow = false;
			string codigoAgencia = e.NewValues["agencia"].ToString().Trim();
			string codigoAgenciaOld = e.OldValues["agencia"] == null ? null : e.OldValues["agencia"].ToString().Trim();
			string codigoPrograma = e.NewValues["programa"].ToString().Trim();
			string codigoProgramaOld = e.OldValues["programa"] == null ? null : e.OldValues["programa"].ToString().Trim();
			if (grdProgramasSociais.IsNewRowEditing)
			{
				if (RN.ProgramasSociais.ExisteCodigoAgenciaPrograma(codigoAgencia, codigoPrograma))
					e.RowError = "Já existe um programa social cadastrado com esses códigos de agência e de programa.";
			}
			else if (grdProgramasSociais.IsEditing)
			{
				if (codigoAgencia != codigoAgenciaOld || codigoPrograma != codigoProgramaOld)
				{
					if (RN.ProgramasSociais.ExisteCodigoAgenciaPrograma(codigoAgencia, codigoPrograma))
						e.RowError = "Já existe um programa social cadastrado com esses códigos de agência e de programa.";
				}
			}
		}

		protected void grdProgramasSociais_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
			grdProgramasSociais.Settings.ShowFilterRow = false;
		}
		#endregion

        protected void grdProgramasSociais_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string codigoAgencia = e.Values["agencia"].ToString().Trim();
            string codigoPrograma = e.Values["programa"].ToString().Trim();

            //verifica o programa e agência já estão sendo utilizados em Unidade
            if (RN.ProgramasSociais.ExisteProgramaUnidade(codigoAgencia, codigoPrograma))
                throw new ApplicationException("Não é possível excluir o programa social.\nExiste unidade que o utiliza.");

        }
	}
}
