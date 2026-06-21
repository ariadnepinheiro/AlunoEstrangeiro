using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
	[NavUrl("~/Hades/PadacesUsuarios.aspx"),
	ControlText("Padrões de Acesso"),
	Title("Padrões de Acesso"),]

    public partial class PadacesUsuarios : TPage
    {
		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdPadUsuario, "Padrões de Acesso - Usuários");
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPadUsuario);
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

        protected void grdPadUsuario_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPadUsuario.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "usuario")
                    e.Editor.Enabled = true;
            }
            else if (grdPadUsuario.IsEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "usuario")
                    e.Editor.Enabled = false;
            }
        }

        protected void grdPadUsuario_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string padaces = Convert.ToString(e.GetListSourceFieldValue("padaces"));
                string usuario = Convert.ToString(e.GetListSourceFieldValue("usuario"));

                e.Value = padaces + "-" + usuario;
            }
        }

        protected void grdPadUsuario_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("padaces", e.Values["padaces"]);
            e.Keys.Add("usuario", e.Values["usuario"]);
        }

        protected void grdPadUsuario_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("usuario", chaves[0]);
            e.Keys.Add("sis", chaves[1]);
            e.NewValues["sis"] = "LyceumNet";

        }

        protected void grdPadUsuario_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["padaces"] = lblPadaces.Text;
        }

        protected void grdPadUsuario_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

            //verificar existencia do padrão de acesso para poder inserir os usuários
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
                        e.RowError = "O padrão de acesso no qual se quer inserir um usuário foi excluido. Operação não realizada.";
                        grdPadUsuario.CancelEdit();
                    }
                    else
                        ViewState["erro"] = "Falso";
                }
            }
        }


        protected void grdPadUsuario_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPadUsuario.Settings.ShowFilterRow = false;
            e.NewValues["padaces"] = lblPadaces.Text;
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

        protected void grdPadUsuario_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPadUsuario);
        }
    }
}
