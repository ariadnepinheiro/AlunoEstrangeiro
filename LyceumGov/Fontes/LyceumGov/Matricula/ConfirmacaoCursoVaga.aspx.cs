using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/ConfirmacaoCursoVaga.aspx"),
    ControlText("ConfirmacaoCursoVaga"),
    Title("Confirmação Vagas Continuidade Após Escolha dos Itinerários Formativos")]
    public partial class ConfirmacaoCursoVaga : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                lblMensagemFinalizacao.Text = string.Empty;
                RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
                ControlaAcesso(btnSalvar, AcaoControle.novo);
                ControlaAcesso(btnFinalizar, AcaoControle.novo);

                if (!this.Page.IsPostBack)
                {
                    lblMensagemPeriodo.Text = string.Empty;
                    CarregaAno();
                    btnFinalizar.Visible = false;
                    btnSalvar.Visible = false;
                    pnGrid.Visible = false;
                    pnlResumo.Visible = false;

                    //Verifica periodo
                    DateTime dtInicio = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataInicioConfirmacaoOfertaEscola"]);
                    DateTime dtFim = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataFimConfirmacaoOfertaEscola"]);

                    lblInicio.Text = dtInicio.ToString("dd/MM/yyyy");
                    lblFim.Text = dtFim.ToString("dd/MM/yyyy");

                    if (DateTime.Now < dtInicio || DateTime.Now > dtFim)
                    {
                        btnSalvar.Visible = false;
                        btnFinalizar.Visible = false;
                        pnGrid.Enabled = false;
                        pnlResumo.Visible = true;
                        lblMensagemPeriodo.Text = "O período para a realização da ação de " + dtInicio.ToString("dd/MM/yyyy") + " a " + dtFim.ToString("dd/MM/yyyy") + " .";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                btnFinalizar.Visible = false;
                btnSalvar.Visible = false;
                pnGrid.Visible = false;
                pnlResumo.Visible = false;
                lblMensagemFinalizacao.Text = string.Empty;


                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipio.ResetValue();


                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();

                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                btnFinalizar.Visible = false;
                btnSalvar.Visible = false;
                pnGrid.Visible = false;
                pnlResumo.Visible = false;
                lblMensagemFinalizacao.Text = string.Empty;

                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
                tseUnidadeResponsavel.ResetValue();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            try
            {
                btnFinalizar.Visible = false;
                btnSalvar.Visible = false;
                pnGrid.Visible = false;
                pnlResumo.Visible = false;
                lblMensagemFinalizacao.Text = string.Empty;

                DataTable dtTurma = new DataTable();
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (Convert.ToString(this.tseUnidadeResponsavel["unidade_ens"]) != string.Empty)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];

                            CriaCursoVaga();
                            ManterTurma();
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }
                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Coordenadoria = string.Empty;
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }


                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";


                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Coordenadoria = string.Empty;
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }


                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;


            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseMunicipio.ResetValue();
                tseRegional.ResetValue();
                tseUnidadeResponsavel.ResetValue();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        public void CriaCursoVaga()
        {
            try
            {
                if (ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() || tseUnidadeResponsavel.DBValue.IsNull || !tseUnidadeResponsavel.IsValidDBValue)
                {
                    lblMensagem.Text = "Favor selecione Ano e Unidade de Ensino.";
                    return;
                }

                txtMatEJAM.Text = "-";
                txtMatEJAT.Text = "-";
                txtMatEJAN.Text = "-";
                txtMatEJAI.Text = "-";
                txtMatEnsMedioM.Text = "-";
                txtMatEnsMedioT.Text = "-";
                txtMatEnsMedioN.Text = "-";
                txtMatEnsMedioI.Text = "-";
                txtTotal.Text = "-";
                txtPorEnsMedio.Text = "-";
                txtOptEJA.Text = "-";
                txtPorEJA.Text = "-";
                txtOptEnsMedio.Text = "-";
                txtTotalEJA.Text = "-";


                RN.TurnosVagas.ConfirmacaoOferta rnConfirmacaoVagasOferta = new Techne.Lyceum.RN.TurnosVagas.ConfirmacaoOferta();
                List<RN.DTOs.DadosConfirmacaoVagasOferta> lista = new List<Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta>();

                lista = rnConfirmacaoVagasOferta.ObtemDadosConfirmacaoVagasOferta(Convert.ToInt32(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString());

                rpModalidade.DataSource = lista;
                rpModalidade.DataBind();

                if (lista.Count > 0)
                {
                    btnFinalizar.Visible = true;
                    btnSalvar.Visible = true;
                    pnGrid.Visible = true;
                    pnlResumo.Visible = true;
                }
                else
                {
                    btnFinalizar.Visible = false;
                    btnSalvar.Visible = false;
                    pnGrid.Visible = false;
                    lblMensagem.Text = "A unidade escolar não fez escolhas dos itinerários / trilhas.";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private void ManterTurma()
        {
            try
            {
                if (ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() || tseUnidadeResponsavel.DBValue.IsNull || !tseUnidadeResponsavel.IsValidDBValue)
                {
                    lblMensagem.Text = "Favor selecione Ano e Unidade de Ensino.";
                    return;
                }

                RN.TurnosVagas.ConfirmacaoOferta rnConfirmacaoVagasOferta = new Techne.Lyceum.RN.TurnosVagas.ConfirmacaoOferta();
                List<RN.DTOs.DadosConfirmacaoVagasOferta> lsModalidadeCurso = new List<Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta>();
                List<RN.DTOs.DadosConfirmacaoVagasOfertaCurso> lsCurso = new List<RN.DTOs.DadosConfirmacaoVagasOfertaCurso>();
                decimal optantes = 0;
                int lancVagas = 0;

                lsModalidadeCurso = rnConfirmacaoVagasOferta.ObtemDadosConfirmacaoVagasOferta(Convert.ToInt32(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString());

                if (lsModalidadeCurso.Count > 0)
                {
                    if (lsModalidadeCurso.FirstOrDefault().Finalizado == true)
                    {
                        btnFinalizar.Visible = false;
                        btnSalvar.Visible = false;
                        pnGrid.Enabled = false;

                        lblMensagemFinalizacao.Text = string.Format(@"A confirmação foi finalizada: {0} às {1} por {2} - {3}.",
                                lsModalidadeCurso.FirstOrDefault().Data.ToShortDateString(), lsModalidadeCurso.FirstOrDefault().Data.ToShortTimeString(), lsModalidadeCurso.FirstOrDefault().UsuarioId, lsModalidadeCurso.FirstOrDefault().UsuarioNome);
                    }

                    txtMatEJAM.Text = "-";
                    txtMatEJAT.Text = "-";
                    txtMatEJAN.Text = "-";
                    txtMatEJAI.Text = "-";
                    txtMatEnsMedioM.Text = "-";
                    txtMatEnsMedioT.Text = "-";
                    txtMatEnsMedioN.Text = "-";
                    txtMatEnsMedioI.Text = "-";
                    txtTotal.Text = "-";
                    txtTotalEJA.Text = "-";

                    foreach (RepeaterItem item in rpModalidade.Items)
                    {
                        Repeater rpVagas = (Repeater)item.FindControl("rpVagas");

                        Label lblModalidade = (Label)item.FindControl("lblModalidade");
                        Label lblPorcentagem = (Label)item.FindControl("lblPorcentagem");

                        HiddenField hdnCursoReferencia = (HiddenField)item.FindControl("hdnCursoReferencia");
                        HiddenField hdnSerieReferencia = (HiddenField)item.FindControl("hdnSerieReferencia");

                        if (lsModalidadeCurso.Count > 0)
                        {
                            hdnCursoReferencia.Value = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).CursoReferencia;
                            hdnSerieReferencia.Value = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).SerieReferencia.ToString();

                            lsCurso = lsModalidadeCurso.Find(x => x.ModalidadeCurso == lblModalidade.Text).Ofertas.ToList();

                            if (lblModalidade.Text == "Ensino Médio")
                            {
                                txtMatEnsMedioM.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosManha.ToString();
                                txtMatEnsMedioT.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosTarde.ToString();
                                txtMatEnsMedioN.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosNoite.ToString();
                                txtMatEnsMedioI.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosIntegral.ToString();
                                txtTotal.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).QuantidadeMatriculados.ToString();

                                optantes = lsModalidadeCurso.Find(x => x.ModalidadeCurso == lblModalidade.Text).Ofertas.Sum(x => x.QuantidadeOptantes);
                                txtOptEnsMedio.Text = optantes.ToString();
                                if (lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).QuantidadeMatriculados > 0)
                                {
                                    txtPorEnsMedio.Text = ((optantes / Convert.ToDecimal(txtTotal.Text)) * 100).ToString("0.00");

                                    decimal soma = (lsCurso.Sum(x => x.VagasManha) + lsCurso.Sum(x => x.VagasTarde) + lsCurso.Sum(x => x.VagasNoite) + lsCurso.Sum(x => x.VagasIntegral));

                                    lblPorcentagem.Text = ((soma / Convert.ToDecimal(txtTotal.Text)) * 100).ToString("0.00");

                                }
                                else
                                {
                                    txtPorEnsMedio.Text = "0.00";
                                    lblPorcentagem.Text = "0.00";
                                }

                            }

                            if (lblModalidade.Text == "Educação de Jovens e Adultos")
                            {
                                txtMatEJAM.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosManha.ToString();
                                txtMatEJAT.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosTarde.ToString();
                                txtMatEJAN.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosNoite.ToString();
                                txtMatEJAI.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).MatriculadosIntegral.ToString();
                                txtTotalEJA.Text = lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).QuantidadeMatriculados.ToString();
                                optantes = lsModalidadeCurso.Find(x => x.ModalidadeCurso == lblModalidade.Text).Ofertas.Sum(x => x.QuantidadeOptantes);

                                txtOptEJA.Text = optantes.ToString();

                                if (lsModalidadeCurso.FirstOrDefault(x => x.ModalidadeCurso == lblModalidade.Text).QuantidadeMatriculados > 0)
                                {
                                    txtPorEJA.Text = ((optantes / Convert.ToDecimal(txtTotalEJA.Text)) * 100).ToString("0.00");
                                    decimal soma = (lsCurso.Sum(x => x.VagasManha) + lsCurso.Sum(x => x.VagasTarde) + lsCurso.Sum(x => x.VagasNoite) + lsCurso.Sum(x => x.VagasIntegral));

                                    lblPorcentagem.Text = ((soma / Convert.ToDecimal(txtTotalEJA.Text)) * 100).ToString("0.00");
                                }
                                else
                                {
                                    txtPorEJA.Text = "0.00";
                                    lblPorcentagem.Text = "0.00";
                                }
                            }


                            rpVagas.DataSource = lsCurso;
                            rpVagas.DataBind();
                            lancVagas += lsCurso.Count;                            
                        }
                    }
                }

                if (lancVagas == 0)
                {
                    btnFinalizar.Visible = false;
                    btnSalvar.Visible = false;
                    pnGrid.Visible = false;
                    lblMensagem.Text = "A unidade escolar não possui Oferta de Itinerários para o ano.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Salva(false);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private bool Salva(bool finaliza)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                ValidacaoDados validacao = new ValidacaoDados();
                RN.TurnosVagas.ConfirmacaoOferta rnConfirmacaoVagasOferta = new Techne.Lyceum.RN.TurnosVagas.ConfirmacaoOferta();
                List<RN.DTOs.DadosConfirmacaoVagasOferta> lsVagas = new List<Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta>();
                RN.DTOs.DadosConfirmacaoVagasOferta dados = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta();
                RN.DTOs.DadosConfirmacaoVagasOfertaCurso oferta = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOfertaCurso();
                List<RN.DTOs.DadosConfirmacaoVagasOfertaCurso> lsCurso = new List<RN.DTOs.DadosConfirmacaoVagasOfertaCurso>();


                foreach (RepeaterItem itemM in rpModalidade.Items)
                {
                    dados = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta();
                    Repeater rpVagas = (Repeater)itemM.FindControl("rpVagas");
                    Label lblModalidade = (Label)itemM.FindControl("lblModalidade");
                    Label lblQtde = (Label)itemM.FindControl("lblQtde");
                    HiddenField hdnCursoReferencia = (HiddenField)itemM.FindControl("hdnCursoReferencia");
                    HiddenField hdnSerieReferencia = (HiddenField)itemM.FindControl("hdnSerieReferencia");

                    dados.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                    dados.Censo = (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                    dados.Periodo = lblModalidade.Text == "Ensino Médio" ? 0 : 1;
                    dados.Finalizado = finaliza;
                    if (lblModalidade.Text == "Ensino Médio")
                    {
                        dados.MatriculadosManha = !txtMatEnsMedioM.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEnsMedioM.Text) : -1;
                        dados.MatriculadosTarde = !txtMatEnsMedioT.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEnsMedioT.Text) : -1;
                        dados.MatriculadosNoite = !txtMatEnsMedioN.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEnsMedioN.Text) : -1;
                        dados.MatriculadosIntegral = !txtMatEnsMedioI.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEnsMedioI.Text) : -1;
                    }
                    else
                    {
                        dados.MatriculadosManha = !txtMatEJAM.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEJAM.Text) : -1;
                        dados.MatriculadosTarde = !txtMatEJAT.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEJAT.Text) : -1;
                        dados.MatriculadosNoite = !txtMatEJAN.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEJAN.Text) : -1;
                        dados.MatriculadosIntegral = !txtMatEJAI.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMatEJAI.Text) : -1;

                    }


                    dados.ModalidadeCurso = lblModalidade.Text;
                    dados.UsuarioId = User.Identity.Name;
                    dados.SerieReferencia = !hdnSerieReferencia.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnSerieReferencia.Value) : -1;
                    dados.CursoReferencia = !hdnCursoReferencia.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCursoReferencia.Value : null;

                    lsCurso = new List<Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOfertaCurso>();

                    foreach (RepeaterItem item in rpVagas.Items)
                    {
                        oferta = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOfertaCurso();

                        Label lblCurso = (Label)item.FindControl("lblCurso");
                        Label lblSerie = (Label)item.FindControl("lblSerie");

                        TextBox txtManha = (TextBox)item.FindControl("txtManha");
                        TextBox txtTarde = (TextBox)item.FindControl("txtTarde");
                        TextBox txtNoite = (TextBox)item.FindControl("txtNoite");
                        TextBox txtIntegral = (TextBox)item.FindControl("txtIntegral");
                        TextBox txtOptantes = (TextBox)item.FindControl("txtOptantes");

                        oferta.Curso = !lblCurso.Text.IsNullOrEmptyOrWhiteSpace() ? lblCurso.Text : null;
                        oferta.Serie = !lblSerie.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(lblSerie.Text) : -1;
                        oferta.VagasManha = !txtManha.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtManha.Text) : -1;
                        oferta.VagasTarde = !txtTarde.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtTarde.Text) : -1;
                        oferta.VagasNoite = !txtNoite.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtNoite.Text) : -1;
                        oferta.VagasIntegral = !txtIntegral.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtIntegral.Text) : -1;
                        oferta.QuantidadeOptantes = !txtOptantes.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtOptantes.Text) : -1;

                        oferta.HabilitaManha = txtManha.Enabled;
                        oferta.HabilitaTarde = txtTarde.Enabled;
                        oferta.HabilitaNoite = txtNoite.Enabled;
                        oferta.HabilitaIntegral = txtIntegral.Enabled;

                        lsCurso.Add(oferta);

                    }
                    dados.Ofertas = lsCurso;
                    lsVagas.Add(dados);
                }

                validacao = rnConfirmacaoVagasOferta.Valida(lsVagas, finaliza);

                if (validacao.Valido)
                {
                    rnConfirmacaoVagasOferta.Salva(lsVagas);

                    ManterTurma();
                    lblMensagem.Text = "Confirmação " + (finaliza ? "finalizada" : "salva") + " com sucesso. ";
                    return true;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return false;
            }
        }


        protected void btnFinalizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Salva(true))
                {
                    btnFinalizar.Visible = false;
                    btnSalvar.Visible = false;
                    pnGrid.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
