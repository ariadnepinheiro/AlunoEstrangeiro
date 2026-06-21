using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{

	[NavUrl("~/Academico/DocumentosRequeridos.aspx"),
	ControlText("Documentos Requeridos"),
	Title("Documentos Requeridos"),]


	public partial class DocumentosRequeridos : TPage
	{
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

		protected void Page_Load(object sender, EventArgs e)
		{
			TituloGrid(grdDocumentosRequeridos, "Documentos Requeridos");
		}

		protected void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdDocumentosRequeridos);
		}

		#region Métodos da Grid
		protected void grdDocumentosRequeridos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdDocumentosRequeridos);
		}

		protected void grdDocumentosRequeridos_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
		{
			if (grdDocumentosRequeridos.IsNewRowEditing || grdDocumentosRequeridos.IsEditing)
			{
				if (e.Column.FieldName == "doc")
				{
					e.Editor.ReadOnly = true;
				}

                if (grdDocumentosRequeridos.IsNewRowEditing)
                {
                    if (e.Column.FieldName == "bloqueia_pre_matr")
                    {
                        e.Editor.Value = "N";
                    }
                }
			}
		}

		protected void grdDocumentosRequeridos_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
		{
            e.NewValues["doc"] = RN.DocumentosRequeridos.GeraNumeroDocumento();
			grdDocumentosRequeridos.Settings.ShowFilterRow = false;
		}

		protected void grdDocumentosRequeridos_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
		{
			string descricao = e.NewValues["nome"].ToString().Trim();
            string doc = e.NewValues["doc"].ToString().Trim();
			if (RN.DocumentosRequeridos.ExisteDescricao(descricao, doc))
				e.RowError = "Já existe um documento cadastrado com essa descrição.";
		}

		protected void grdDocumentosRequeridos_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
			grdDocumentosRequeridos.Settings.ShowFilterRow = false;
		}
		#endregion
	}
}
