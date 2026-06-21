using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.Net.InspecaoEscolar
{
    [NavUrl("~/InspecaoEscolar/RelatorioReplicacaoCampanha.aspx"),
    ControlText("Relatório de Replicação de Campanha"),
    Title("Relatório de Replicação de Campanha"),]

    public partial class RelatorioReplicacaoCampanha : TPage
    {
        private readonly RN.InspecaoEscolar.Campanha CampanhaRN;
        private readonly RN.InspecaoEscolar.Grupo GrupoRN;
        private readonly RN.InspecaoEscolar.Assunto AssuntoRN;


        public RelatorioReplicacaoCampanha()
        {
            CampanhaRN = new Techne.Lyceum.RN.InspecaoEscolar.Campanha();
            GrupoRN = new Techne.Lyceum.RN.InspecaoEscolar.Grupo();
            AssuntoRN = new Techne.Lyceum.RN.InspecaoEscolar.Assunto();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnReplicar, AcaoControle.editar);
            ControlaAcesso(btnCancelar, AcaoControle.editar);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    this.ListarAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void LimpaDdl(DropDownList[] ddl)
        {
            foreach (var nomeddl in ddl)
            {
                nomeddl.ClearSelection();
                nomeddl.DataSource = null;
                nomeddl.DataBind();
                nomeddl.Items.Insert(0, new ListItem("Selecione", "-1"));
            }

        }

        public void ListarAno()
        {
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", "-1"));
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ano foi escolhido, logo limpa demais campos
            DropDownList[] dropdown = new DropDownList[] { ddlSemestre };
            ddlSemestre.ClearSelection();
            txtObjetivo.Text = string.Empty;
            txtProcedimento.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            rblExibeInspecaoEscolar.ClearSelection();
        }

        protected void tseCampanhaOrigem_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                if (!this.tseCampanhaOrigem.DBValue.IsNull)
                {
                    if (this.tseCampanhaOrigem.IsValidDBValue)
                    {
                        return;
                    }
                    else
                    {
                        this.lblMensagem.Text = "campanha não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "campanha não cadastrada.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnReplicar_Click(object sender, EventArgs e)
        {
            int campanhaIdOrigem = -1;
            ValidacaoDados validacao = new ValidacaoDados();

            RN.InspecaoEscolar.Entidades.Campanha campanhaDados = new RN.InspecaoEscolar.Entidades.Campanha();
            RN.InspecaoEscolar.Replicacao rnReplicacao = new Techne.Lyceum.RN.InspecaoEscolar.Replicacao();
            bool replicou = false;

            try
            {
                lblMensagem.Text = string.Empty;
                campanhaIdOrigem = (tseCampanhaOrigem.Value == null) ? -1 : Convert.ToInt32(tseCampanhaOrigem.Value.ToString());

                //preenche a entidade
                campanhaDados.Ano = ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlAno.SelectedValue);
                campanhaDados.Semestre = ddlSemestre.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(ddlSemestre.SelectedValue);
                campanhaDados.Objetivo = txtObjetivo.Text.Trim().ToUpper();
                campanhaDados.Procedimento = txtProcedimento.Text.Trim().ToUpper();
                campanhaDados.ExibeInspecaoEscolar = !rblExibeInspecaoEscolar.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToBoolean(rblExibeInspecaoEscolar.SelectedValue) : (bool?)null;
                campanhaDados.Titulo = txtTitulo.Text.Trim().ToUpper();
                campanhaDados.UsuarioId = User.Identity.Name;
                campanhaDados.CampanhaId = 0;

                validacao = rnReplicacao.ValidaReplicacao(campanhaDados, campanhaIdOrigem);

                if (validacao.Valido)
                {
                    replicou = rnReplicacao.replicar(campanhaDados, campanhaIdOrigem);
                    if (replicou)
                    {
                        lblMensagem.Text = "replicação da campanha " + txtTitulo.Text.Trim().ToUpper() + " realizada com sucesso.";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('replicação realizada com sucesso.');", true);
                        this.LimparTela();
                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparTela()
        {
            tseCampanhaOrigem.ResetValue();
            ddlAno.ClearSelection();
            ddlSemestre.ClearSelection();
            txtObjetivo.Text = string.Empty;
            txtProcedimento.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            rblExibeInspecaoEscolar.ClearSelection();
            HiddenID.Value = null;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.LimparTela();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
