using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Collections.Generic;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.Basico
{
    [
NavUrl("~/Basico/RestricaoAcessoUsuarios.aspx"),
ControlText("RestricaoAcessoUsuarios"),
Title("Restrição de Acesso por Usuários"),
]
    public partial class RestricaoAcessoUsuarios : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdUsuarioUnidade, "Unidades de Acesso Permitido do Usuário");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Permission.AllowDelete)
                btnRemoverTodasUnidades.Visible = false;
            if (!Permission.AllowInsert)
                btnAdicionarTodasUnidades.Visible = false;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdUsuarioUnidade);
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

        protected void tseUsuario_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (tseUsuario.IsValidDBValue && !tseUsuario.DBValue.IsNull)
            {
                btnAdicionarTodasUnidades.Visible = true;
                btnRemoverTodasUnidades.Visible = true;
                grdUsuarioUnidade.Visible = true;
                lblMensagem.Text = string.Empty;
            }
            else if (!tseUsuario.DBValue.IsNull)
            {
                btnAdicionarTodasUnidades.Visible = false;
                btnRemoverTodasUnidades.Visible = false;
                grdUsuarioUnidade.Visible = false;
                lblMensagem.Text = "Usuário não cadastrado.";
            }
            else
            {
                btnAdicionarTodasUnidades.Visible = false;
                btnRemoverTodasUnidades.Visible = false;
                grdUsuarioUnidade.Visible = false;
                lblMensagem.Text = "Favor consultar um usuário.";
            }
        }

        protected void grdUsuarioUnidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdUsuarioUnidade);
        }

        protected void grdUsuarioUnidade_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string usuario = Convert.ToString(e.GetListSourceFieldValue("usuario"));
                string faculdade = Convert.ToString(e.GetListSourceFieldValue("unidade_fis"));
                e.Value = usuario + "-" + faculdade;
            }
        }

        protected void grdUsuarioUnidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("usuario", e.Values["usuario"]);
            e.Keys.Add("unidade_fis", e.Values["unidade_fis"]);
        }

        protected void grdUsuarioUnidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["usuario"] = tseUsuario.DBValue.ToString();
        }

        protected void grdUsuarioUnidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("usuario", chaves[0]);
            e.Keys.Add("unidade_fis", chaves[1]);
        }

        protected void grdUsuarioUnidade_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdUsuarioUnidade.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "nome_comp03")
                    e.Editor.ReadOnly = false;

            }
            else if (grdUsuarioUnidade.IsEditing)
            {
                if ((e.Column.FieldName) == "nome_comp03")
                    e.Editor.ReadOnly = true;
            }
        }

        protected void grdUsuarioUnidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdUsuarioUnidade.Settings.ShowFilterRow = false;
        }

        protected void grdUsuarioUnidade_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            TSearchBox tseUnidadeFisica = (TSearchBox)grdUsuarioUnidade.FindEditFormTemplateControl("tseUnidadeFisica");

            if (tseUnidadeFisica != null)
            {
                if (tseUnidadeFisica.DBValue.IsNull)
                    e.RowError = "Favor selecionar uma para unidade física.";

                if (!tseUnidadeFisica.IsValidDBValue)
                    e.RowError = "Favor selecionar uma unidade física válida.";
            }
        }

        protected void btnAdicionarTodasUnidades_Click(object sender, EventArgs e)
        {
            if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
            {
                lblMensagem.Text = "Selecione um usuário válido.";
                return;
            }

            RetValue ret = RN.Usuarios.InserirAcessoTodasUnidadesFisicas(tseUsuario.DBValue.ToString());
            if (ret != null && !ret.Ok)
                lblMensagem.Text = "Não foi possível inserir todas as unidades físicas.<br/>Operação cancelada.";
            grdUsuarioUnidade.DataBind();
        }

        protected void btnRemoverTodasUnidades_Click(object sender, EventArgs e)
        {
            RN.Usuarios rnUsuarios = new Usuarios();

            try
            {
                if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
                {
                    lblMensagem.Text = "Selecione um usuário válido.";
                    return;
                }

                rnUsuarios.RemoveAcessoUnidadesFisicasPor(tseUsuario.DBValue.ToString());
                grdUsuarioUnidade.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnInserirRegional_Click(object sender, EventArgs e)
        {
            if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
            {
                lblMensagem.Text = "Selecione um usuário válido.";
                return;
            }

            if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
            {
                RetValue ret = RN.Usuarios.InserirAcessoTodasUnidadesFisicas(tseRegional.DBValue.ToString(), tseUsuario.DBValue.ToString());
                if (ret != null && !ret.Ok)
                    lblMensagem.Text = "Não foi possível incluir as unidades físicas da regional.<br/>Operação cancelada.";
                grdUsuarioUnidade.DataBind();
            }
            else
            {
                lblMensagem.Text = "Selecione uma Regional válida.";
            }
        }

        protected void btnRemoverRegional_Click(object sender, EventArgs e)
        {
            if (tseUsuario.DBValue.IsNull || !tseUsuario.IsValidDBValue)
            {
                lblMensagem.Text = "Selecione um usuário válido.";
                return;
            }

            if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
            {
                RetValue ret = RN.Usuarios.RemoverAcessoUnidadesFisicas(tseRegional.DBValue.ToString(), tseUsuario.DBValue.ToString());
                if (ret != null && !ret.Ok)
                    lblMensagem.Text = "Não foi possível incluir as unidades físicas da regional.<br/>Operação cancelada.";
                grdUsuarioUnidade.DataBind();
            }
            else
            {
                lblMensagem.Text = "Selecione uma regional válida.";
            }
        }
    }
}
