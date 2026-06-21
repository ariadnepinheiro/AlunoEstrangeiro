using System;
using Techne.Web;
using DevExpress.Web.ASPxClasses;

namespace Techne.Lyceum.Net.Hades
{
    [
        NavUrl("~/Hades/PadroesdeAcesso.aspx"),
        ControlText("PadroesdeAcesso"),
        Title("Padrões de Acesso"),
    ]
    public partial class PadroesdeAcesso : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
			TituloGrid(grdPadaces, "Padrões de Acesso");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    //int linha = ObterLinhaQueryString(decodedText);
                    //grdPadaces.FocusedRowIndex = linha;
                    //lblItemSelected.Text = "Padrão de Acesso selecionado: " + ObterPadacesQueryString(decodedText);
                    string padrao = ObterPadacesQueryString(decodedText);
                    grdPadaces.FocusedRowIndex = grdPadaces.FindVisibleIndexByKeyValue(padrao);
                    
                }
                else
                    grdPadaces.FocusedRowIndex = 0;
            }
        }

		protected void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdPadaces);
			btnTransacao.Visible = Permission.AllowUpdate;
			btnTransacao.Enabled = Permission.AllowUpdate;
		}

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            string padaces = ObtemPadaces();
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    //int linha = ObterLinhaQueryString(decodedText);
                    //grdPadaces.FocusedRowIndex = linha;

                    string padrao = ObterPadacesQueryString(decodedText);
                    grdPadaces.FocusedRowIndex = grdPadaces.FindVisibleIndexByKeyValue(padrao);

                    //lblItemSelected.Text = "Padrão de Acesso selecionado: " + ObterPadacesQueryString(decodedText);
                }
                else
                    grdPadaces.FocusedRowIndex = 0;
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


        private string ObtemPadaces()
        {
            //obtém o indice atual da seleção
            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            string padrao = (string)grdPadaces.GetRowValues(curPageSelection, new string[] { grdPadaces.KeyFieldName });

            return padrao;
        }


        protected void grdPadaces_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPadaces.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.Enabled = true;
            }
            else if (grdPadaces.IsEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.Enabled = false;
            }

        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdPadaces.PageIndex * grdPadaces.SettingsPager.PageSize;
            for (int i = 0; i < grdPadaces.VisibleRowCount; i++)
            {
                if (grdPadaces.FocusedRowIndex == startIndexOnPage + i)
                    return startIndexOnPage + i;
            }
            return -1;
        }

        protected void grdPadaces_FocusedRowChanged(object sender, EventArgs e)
        {
            //string padaces = ObtemPadaces();
            //lblItemSelected.Text = "Padrão de Acesso selecionado: " + padaces;
            grdPadaces.CancelEdit();
        }


        protected void btnTransacao_Click(object sender, EventArgs e)
        {
            string padaces = ObtemPadaces();
            int linha = GetSelectedRowOnTheCurrentPage();

            if (!string.IsNullOrEmpty(padaces) && linha != -1)
            {
                string queryString = MontarQueryString(padaces, linha);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                Response.Redirect("PadacesTransacoes.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            else
            {
                lblMensagem.Text = "Selecione um padrão de acesso.";
            }
        }

        protected void btnUsuarios_Click(object sender, EventArgs e)
        {
            string padaces = ObtemPadaces();
            int linha = GetSelectedRowOnTheCurrentPage();

            if (!string.IsNullOrEmpty(padaces) && linha != -1)
            {
                string queryString = MontarQueryString(padaces, linha);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                Response.Redirect("PadacesUsuarios.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            else
            {
                lblMensagem.Text = "Selecione um padrão de acesso.";
            }
        }

        protected void btnRelatorios_Click(object sender, EventArgs e)
        {
            string padaces = ObtemPadaces();
            int linha = GetSelectedRowOnTheCurrentPage();

            if (!string.IsNullOrEmpty(padaces) && linha != -1 )
            {
                string queryString = MontarQueryString(padaces, linha);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                Response.Redirect("PadacesRelatorios.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            else
            {
                lblMensagem.Text = "Selecione um padrão de acesso.";
            }
        }

        protected void grdPadaces_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPadaces.Settings.ShowFilterRow = false;
        }

        protected void grdPadaces_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPadaces.Settings.ShowFilterRow = false;
        }

        private string MontarQueryString(string padaces, int linha)
        {
            System.Text.StringBuilder queryString = new System.Text.StringBuilder();
            queryString.Append(String.Format("line={0}", linha));
            queryString.Append(String.Format("&value={0}", padaces));
            return queryString.ToString();
        }


        private int ObterLinhaQueryString(string queryString)
        {
            lblMensagem.Text = string.Empty;
            string[] listaDados = queryString.Split('&');
            int linha = -1;
            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("line") >= 0)
                   linha = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
            }
            return linha;
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



        protected void grdPadaces_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPadaces);
        }


    }
}
