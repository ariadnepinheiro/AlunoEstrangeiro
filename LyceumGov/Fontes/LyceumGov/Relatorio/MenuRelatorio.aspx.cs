using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Data;

namespace Techne.Lyceum.Net.Relatorio
{
	[
		NavUrl("~/Relatorio/MenuRelatorio.aspx"),
		ControlText("MenuRelatorio"),
		Title("Consultas/Relatório")
	]

	public partial class MenuRelatorio : TPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				tree.Nodes.Clear();
				CarregarGrupos();
				tree.CollapseAll();                
			}
		}

		private void CarregarGrupos()
		{
			QueryTable qtGrp = RN.Relatorio.GrupoRelatorio(HttpContext.Current.User.Identity.Name);
            QueryTable qtRel = RN.Relatorio.Relatorios(HttpContext.Current.User.Identity.Name);

			for (int i = 0; i < qtGrp.Rows.Count; i++)
			{
				TreeNode masterNode = new TreeNode(qtGrp.Rows[i]["descricao"].ToString(), qtGrp.Rows[i]["gruporelat"].ToString());

				tree.Nodes.Add(masterNode);

				SimpleRow[] linhas = qtRel.Select("gruporelat = '" + RN.RNBase.MudarAspas(qtGrp.Rows[i]["gruporelat"].ToString()) + "'", "descricao");

				if (linhas.Length > 0)
				{
					for (int j = 0; j < linhas.Length; j++)
					{
						TreeNode node = new TreeNode(linhas[j]["descricao"].ToString(), linhas[j]["relatorio"].ToString());
						node.NavigateUrl = "PageViewer.aspx?report=" + node.Value + "&grp=" + masterNode.Value;
						node.Target = "_blank";
						masterNode.ChildNodes.Add(node);
					}
				}
			}
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

		protected void tree_SelectedNodeChanged(object sender, EventArgs e)
		{
			//if (tree.SelectedNode.ChildNodes.Count == 0)
			//{
			//    if (tree.SelectedNode.Parent != null)
			//        Response.Redirect("PageViewer.aspx?report=" + tree.SelectedNode.Value + "&grp=" + tree.SelectedNode.Parent.Value);
			//}
			//else
			//{
			//    tree.SelectedNode.Expand();
			//}
		}

        protected void tree_SelectedNodeChanged1(object sender, EventArgs e)
        {
            if (tree.SelectedNode.ChildNodes.Count == 0)
            {
                //
            }
            else
            {
                if (tree.SelectedNode.Expanded != null && (bool)tree.SelectedNode.Expanded)
                    tree.SelectedNode.Collapse();
                else
                    tree.SelectedNode.Expand();
            }
        }

        protected void btnExpand_Click(object sender, EventArgs e)
        {
            tree.ExpandAll();
        }

        protected void btnCollapse_Click(object sender, EventArgs e)
        {
            tree.CollapseAll();
        }
	}
}
