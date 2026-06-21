using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Hades
{

    [
        NavUrl("~/Hades/PadacesPerfil.aspx"),
         ControlText("PadacesPerfil"),
         Title("Perfil para Padrão de Acesso"),
    ]
    public partial class PadacesPerfil : TPage
    {
        public object Listar(object padaces, object id_perfil)
        {
            if (!string.IsNullOrEmpty(padaces.ToString())  && string.IsNullOrEmpty(id_perfil.ToString()))
                return RN.PadacesPerfil.ListarPorPadraoAcesso(padaces.ToString());
            else if (string.IsNullOrEmpty(padaces.ToString()) && !string.IsNullOrEmpty(id_perfil.ToString()))
                return RN.PadacesPerfil.ListarPorPerfil(int.Parse(id_perfil.ToString()));
            else
                return RN.PadacesPerfil.Listar(padaces.ToString());
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdPadraoPerfil, "Perfil para Padrão de Acesso");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            { }

        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var mensagem = string.Empty;

                var padperfil = new TcePadacesPerfil
                {
                    IdPerfil = string.IsNullOrEmpty(tsePerfil.DBValue.ToString()) ? 0 : Convert.ToInt32(tsePerfil.DBValue.ToString()),
                    Matricula = User.Identity.Name,
                    Padaces = tsePadrao.DBValue.ToString()
                };

                var validacao = RN.PadacesPerfil.Validar(padperfil);

                if (validacao.Valido)
                {
                    RN.PadacesPerfil.Inserir(padperfil);
                    mensagem = "Perfil do Padrão de Acesso incluído com sucesso.";

                    var script = @"alert('" + mensagem + @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                    this.LimparCampos();

                    this.odsPadraoPerfil.Select();
                    this.odsPadraoPerfil.DataBind();
                    this.grdPadraoPerfil.DataBind();
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            this.tsePerfil.ResetValue();
            this.tsePadrao.ResetValue();
        }
        protected void grdPadraoPerfil_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPadraoPerfil);
        }
        protected void grdPadraoPerfil_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPadraoPerfil.Settings.ShowFilterRow = false;

        }

        protected void grdPadraoPerfil_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPadraoPerfil.Settings.ShowFilterRow = false;
        }
        public void Delete(object ID_PADACES_PERFIL)
        {
        }

        protected void odsPadraoPerfil_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_PADACES_PERFIL"].ToString();

            var validacao = RN.PadacesPerfil.ValidarRemover(int.Parse(id));

            if (validacao.Valido)
            {
                RN.PadacesPerfil.Remover(int.Parse(id));
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

    }
}
