using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
using DevExpress.Web.ASPxGridView.Rendering;
using System.Text;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/TurmasProvisorias.aspx")
    , ControlText("TurmasProvisorias")
    , Title("Turmas Provisórias")]
    public partial class TurmasProvisorias : TPage
    {
        #region Propriedades

        string ano, censo, tipoEvento = string.Empty;

        #endregion

        public object ListaTurma(object UnidadeEnsino, object Ano, object TipoEvento)
        {
            CtvTurmaProvisoria rnCtvTurmaProvisoria = new CtvTurmaProvisoria();

            if ((UnidadeEnsino != null) && Ano != null && TipoEvento != null)
            {
                return rnCtvTurmaProvisoria.ListaTurmaProvisoriaPor(UnidadeEnsino.ToString(), Ano.ToString(), TipoEvento.ToString());
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;                

                if (!Page.IsPostBack)
                {
                    hdnTipoEvento.Value = string.Empty;
                    hdnAno.Value = string.Empty;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                        ObterDadosQueryString(decodedText);
                    
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("Ano=") >= 0)
                {
                    ano = dados.Substring(dados.LastIndexOf('=') + 1);
                    hdnAno.Value = ano;

                }
                else if (dados.IndexOf("UnidadeEnsino=") >= 0)
                {
                    censo = dados.Substring(dados.LastIndexOf('=') + 1);
                    tseUnidadesFisicas.Value = censo;
                }
                else if (dados.IndexOf("TipoEvento=") >= 0)
                {
                    tipoEvento = dados.Substring(dados.LastIndexOf('=') + 1);
                    hdnTipoEvento.Value = tipoEvento;
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdTurmasProvisorias, "Turmas Provisórias");
        }

        protected void grdTurmasProvisorias_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTurmasProvisorias);
        }

        protected void grdTurmasProvisorias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTurmasProvisorias.Settings.ShowFilterRow = false;
        }

        protected void odsTurmasProvisorias_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.InputParameters["ID"].ToString());
                string mensagem = string.Empty;
                DataTable dtSalas = null;
                CtvTurmaProvisoria rnCtvTurmaProvisoria = new CtvTurmaProvisoria();

                dtSalas = rnCtvTurmaProvisoria.ListaSalaAssociadaPor(id);

                if (dtSalas.Rows.Count > 0)
                {
                    mensagem = "Não foi possível realizar a exclusão. Turma provisória " + dtSalas.Rows[0]["TURMA"].ToString() + " já associada à sala " + dtSalas.Rows[0]["SALA"].ToString() + ". Caso deseje excluir, antes remova a associação existente.";
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    ObterDadosQueryString(decodedText);

                    ClientScript.RegisterStartupScript(this.GetType(), "TurmaProvisoria", "document.onload = alert('" + mensagem.ToString() + "');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "TurmaProvisoria", "document.onload = alert('Turma Provisória excluída com sucesso.');", true);

                    RN.CtvTurmaProvisoria.RemoveTurmaProvisoriaPor(id);                   
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void Delete(object ID)
        {
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
            {
                Response.Redirect("~/Academico/ConfirmacaoTurnosVagas.aspx?Chave=" + Request.QueryString["Chave"], false);
            }
        }
    }
}
