using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/DiasNaoLetivo.aspx"), ControlText("Dias Não Letivo"), Title("Dias Não Letivo")]
    public partial class DiasNaoLetivo : TPage
    {
        public object Lista(object ano)
        {
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();

            if (ano != null)
            {
                if (!string.IsNullOrEmpty(ano.ToString()))
                {
                    return rnDiasNaoLetivos.ListaPor(Convert.ToInt32(ano.ToString()));
                }
            }
            return null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdDiasNaoLetivo, "Dias não letivos");

        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(grdDiasNaoLetivo);

        }

        protected void cmbAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimparTela();
                pnlGrid.Visible = false;
                pnlDados.Visible = false;
                if (!cmbAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    pnlDados.Visible = true;
                    pnlGrid.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblAbrangencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlMunicipio.Visible = false;
                tseMunicipio.ResetValue();
                dtData.Text = string.Empty;
                if (rblAbrangencia.SelectedValue == "M")
                {
                    pnlMunicipio.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;

                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void LimparTela()
        {
            rblAbrangencia.ClearSelection();
            tseMunicipio.ResetValue();
            dtData.Text = string.Empty;
            grdDiasNaoLetivo.DataBind();

        }

        private void CarregaAno()
        {
            cmbAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            cmbAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            cmbAno.DataBind();
            cmbAno.Items.Insert(0, item);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            RN.Matriculas.Entidades.DiasNaoLetivos diasNaoLetivos = new Techne.Lyceum.RN.Matriculas.Entidades.DiasNaoLetivos();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;
            try
            {
                diasNaoLetivos.Dia = !dtData.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtData.Text) : DateTime.MinValue;
                diasNaoLetivos.MunicipioId = !this.tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue ? tseMunicipio.DBValue.ToString() : null;
                diasNaoLetivos.UsuarioId = User.Identity.Name;

                validacao = rnDiasNaoLetivos.Valida(diasNaoLetivos, (rblAbrangencia.SelectedValue == "M" ? true : false));

                if (validacao.Valido)
                {
                    rnDiasNaoLetivos.Insere(diasNaoLetivos);

                    odsDiasNaoLetivo.Select();
                    grdDiasNaoLetivo.DataBind();
                    rblAbrangencia.ClearSelection();
                    dtData.Text = string.Empty;
                    tseMunicipio.ResetValue();
                    pnlMunicipio.Visible = false;

                    lblMensagem.Text = "Dia não letivo inserido com sucesso.";

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        public void Delete(object DIASNAOLETIVOSID) { }

        protected void grdDiasNaoLetivo_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDiasNaoLetivo);
        }

        protected void grdDiasNaoLetivo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDiasNaoLetivo.Settings.ShowFilterRow = false;
        }

        protected void grdDiasNaoLetivo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdDiasNaoLetivo.Settings.ShowFilterRow = false;
        }


        protected void grdDiasNaoLetivo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            int diasNaoLetivo = 0;
            DateTime data;
            data = e.Values["DIA"] != null ? Convert.ToDateTime(e.Values["DIA"]) : DateTime.MinValue;

            diasNaoLetivo = Convert.ToInt32(e.Keys["DIASNAOLETIVOSID"]);

            validacao = rnDiasNaoLetivos.ValidaRemocao(diasNaoLetivo, data);

            if (validacao.Valido)
            {
                rnDiasNaoLetivos.Remove(diasNaoLetivo);
                grdDiasNaoLetivo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
