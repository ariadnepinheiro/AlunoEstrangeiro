using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/AvaliacaoNecessidadeEspecial.aspx"), ControlText("AvaliacaoNecessidadeEspecial"), Title("Avaliação Necessidade Especial")]

    public partial class AvaliacaoNecessidadeEspecial : TPage
    {
        public object ListaAvaliacao(object unidade_ens, object situacao)
        {
            RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();

            if (!string.IsNullOrEmpty(unidade_ens.ToString()))
            {
                bool? avaliado = null;

                if (Convert.ToString(situacao) == "Avaliado")
                {
                    avaliado = true;
                }
                else if (Convert.ToString(situacao) == "Pendente")
                {
                    avaliado = false;
                }

                return rnAvaliacaoNapes.ListaAlunoPor(unidade_ens.ToString(), avaliado);
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            lblMensagemPopup.Text = string.Empty;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAvaliacao, "Avaliação Necessidade Especial");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnConfirma, AcaoControle.novo);
            ControlaAcesso(grdAvaliacao, AcaoControle.novo, "btnAvaliar");
                   
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                pnlAvaliacao.Visible = false;
                PnlTiposAvaliacao.Visible = false;

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
                            tseUnidadeResponsavel.ResetValue();
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
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseRegionalSalaRecurso_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                /*      pnlAvaliacao.Visible = false;

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
                                tseUnidadeResponsavel.ResetValue();
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
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }*/
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
                if (this.Page.IsCallback)
                {
                    return;
                }

                pnlAvaliacao.Visible = false;
                PnlTiposAvaliacao.Visible = false;

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
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                pnlAvaliacao.Visible = false;
                PnlTiposAvaliacao.Visible = false;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        pnlAvaliacao.Visible = true;
                        grdAvaliacao.DataBind();
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
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
                        lblMensagem.Text = "Unidade de ensino não encontrada.";
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                    lblMensagem.Text = "Unidade de Ensino da Sala de Recursos não preenchida.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

   
        protected void grdAvaliacao_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            var status = (string)grdAvaliacao.GetRowValues(e.VisibleIndex, "AVALIACAONAPES");
            if (!string.IsNullOrEmpty(status))
            {
                e.Text = status;
            }
        }

        protected void grdAvaliacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAvaliacao);
        }

        protected void grdAvaliacao_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                if (e.ButtonID == "btnAvaliar")
                {
                    LimpaCampos();
                    RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial rnAlunoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.AlunoRecursoNecessidadeEspecial();
                    RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
                    List<int> tipoRecursoAluno = new List<int>();
                    RN.NecessidadeEspecial.Entidades.AvaliacaoNapes avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
                    PnlTiposAvaliacao.Visible = true;
                    HabilitaCamposAvaliacao();
                    hdnAvaliacao.Value = Convert.ToString(grdAvaliacao.GetRowValues(e.VisibleIndex, "AVALIACAONAPES"));
                    lblAluno.Text = Convert.ToString(grdAvaliacao.GetRowValues(e.VisibleIndex, "ALUNO"));
                    lblNome.Text = Convert.ToString(grdAvaliacao.GetRowValues(e.VisibleIndex, "NOME_COMPL"));

                    if (hdnAvaliacao.Value == "Pendente")
                    {
                        tipoRecursoAluno = rnAlunoRecursoNecessidadeEspecial.ListaRecursoAlunoPor(lblAluno.Text);

                        foreach (var tipoRecurso in tipoRecursoAluno)
                        {
                            if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador)
                            {
                                rblNecessitaCuidador.SelectedValue = "1";
                            }

                            if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete)
                            {
                                rblNecessitaInterprete.SelectedValue = "1";
                            }

                            if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor)
                            {
                                rblNecessitaLedor.SelectedValue = "1";
                            }

                            if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.SalaRecurso)
                            {
                                rblNecessitaSalaRecurso.SelectedValue = "1";
                            }

                            if (tipoRecurso == (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.PAPEE)
                            {
                                rblNecessitaPAPEE.SelectedValue = "1";
                            }
                        }
                    }
                    else
                    {
                        avaliacao = rnAvaliacaoNapes.ObtemPor(lblAluno.Text, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador);

                        if (avaliacao.AvaliacaoNapesId > 0)
                        {
                            hdnCodigoCuidador.Value = avaliacao.AvaliacaoNapesId.ToString();
                            rblNecessitaCuidador.SelectedValue = Convert.ToBoolean(avaliacao.NecessitaRecurso) ? "1" : "0";
                            rblNecessitaCuidador_SelectedIndexChanged(null, null);
                            rblTipoCuidador.SelectedValue = Convert.ToBoolean(avaliacao.Transitorio) ? "1" : "0";
                            rblTipoCuidador_SelectedIndexChanged(null, null);
                            if (avaliacao.DataInicio.HasValue)
                            {
                                dtInicioCuidador.Value = avaliacao.DataInicio;
                            }
                            if (avaliacao.DataFim.HasValue)
                            {
                                dtFimCuidador.Value = avaliacao.DataFim;
                            }
                            txtJustificativaCuidador.Text = avaliacao.Justificativa;

                        }

                        avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();

                        avaliacao = rnAvaliacaoNapes.ObtemPor(lblAluno.Text, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor);

                        if (avaliacao.AvaliacaoNapesId > 0)
                        {
                            hdnCodigoLedor.Value = avaliacao.AvaliacaoNapesId.ToString();
                            rblNecessitaLedor.SelectedValue = Convert.ToBoolean(avaliacao.NecessitaRecurso) ? "1" : "0";
                            rblNecessitaLedor_SelectedIndexChanged(null, null);
                            rblTipoLedor.SelectedValue = Convert.ToBoolean(avaliacao.Transitorio) ? "1" : "0";
                            rblTipoLedor_SelectedIndexChanged(null, null);
                            if (avaliacao.DataInicio.HasValue)
                            {
                                dtInicioLedor.Value = avaliacao.DataInicio;
                            }
                            if (avaliacao.DataFim.HasValue)
                            {
                                dtFimLedor.Value = avaliacao.DataFim;
                            }
                            txtJustificativaLedor.Text = avaliacao.Justificativa;

                        }
                        avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();

                        avaliacao = rnAvaliacaoNapes.ObtemPor(lblAluno.Text, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete);

                        if (avaliacao.AvaliacaoNapesId > 0)
                        {
                            hdnCodigoInterprete.Value = avaliacao.AvaliacaoNapesId.ToString();
                            rblNecessitaInterprete.SelectedValue = Convert.ToBoolean(avaliacao.NecessitaRecurso) ? "1" : "0";
                            rblNecessitaInterprete_SelectedIndexChanged(null, null);
                            rblTipoInterprete.SelectedValue = Convert.ToBoolean(avaliacao.Transitorio) ? "1" : "0";
                            rblTipoInterprete_SelectedIndexChanged(null, null);
                            if (avaliacao.DataInicio.HasValue)
                            {
                                dtInicioInterprete.Value = avaliacao.DataInicio;
                            }
                            if (avaliacao.DataFim.HasValue)
                            {
                                dtFimInterprete.Value = avaliacao.DataFim;
                            }
                            txtJustificativaInterprete.Text = avaliacao.Justificativa;
                        }


                        avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();

                        avaliacao = rnAvaliacaoNapes.ObtemPor(lblAluno.Text, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.SalaRecurso);

                        if (avaliacao.AvaliacaoNapesId > 0)
                        {
                            hdnCodigoSalaRecurso.Value = avaliacao.AvaliacaoNapesId.ToString();
                            rblNecessitaSalaRecurso.SelectedValue = Convert.ToBoolean(avaliacao.NecessitaRecurso) ? "1" : "0";
                            rblNecessitaSalaRecurso_SelectedIndexChanged(null, null);
                            rblTipoSalaRecurso.SelectedValue = Convert.ToBoolean(avaliacao.Transitorio) ? "1" : "0";
                            rblTipoSalaRecurso_SelectedIndexChanged(null, null);
                            if (avaliacao.DataInicio.HasValue)
                            {
                                dtInicioSalaRecurso.Value = avaliacao.DataInicio;
                            }
                            if (avaliacao.DataFim.HasValue)
                            {
                                dtFimSalaRecurso.Value = avaliacao.DataFim;
                            }
                            txtJustificativaSalaRecurso.Text = avaliacao.Justificativa;
                           
                            
                        }

                        avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();

                        avaliacao = rnAvaliacaoNapes.ObtemPor(lblAluno.Text, (int)RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.PAPEE);

                        if (avaliacao.AvaliacaoNapesId > 0)
                        {
                            hdnCodigoPAPEE.Value = avaliacao.AvaliacaoNapesId.ToString();
                            rblNecessitaPAPEE.SelectedValue = Convert.ToBoolean(avaliacao.NecessitaRecurso) ? "1" : "0";
                            rblNecessitaPAPEE_SelectedIndexChanged(null, null);
                            rblTipoPAPEE.SelectedValue = Convert.ToBoolean(avaliacao.Transitorio) ? "1" : "0";
                            rblTipoPAPEE_SelectedIndexChanged(null, null);
                            if (avaliacao.DataInicio.HasValue)
                            {
                                dtInicioPAPEE.Value = avaliacao.DataInicio;
                            }
                            if (avaliacao.DataFim.HasValue)
                            {
                                dtFimPAPEE.Value = avaliacao.DataFim;
                            }
                            txtJustificativaPAPEE.Text = avaliacao.Justificativa;
                        }

                    }

                    PnlTiposAvaliacao.Visible = true;
                    pnlAvaliacao.Visible = false;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                PnlTiposAvaliacao.Visible = false;
                pnlAvaliacao.Visible = true;
                LimpaCampos();
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnConfirma_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();
                List<RN.NecessidadeEspecial.Entidades.AvaliacaoNapes> ListaAvaliacao = new List<Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes>();
                RN.NecessidadeEspecial.Entidades.AvaliacaoNapes avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();

                if (!rblNecessitaCuidador.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    avaliacao.AvaliacaoNapesId = hdnCodigoCuidador.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnCodigoCuidador.Value);
                    avaliacao.AlunoId = lblAluno.Text.Trim();
                    avaliacao.TipoRecursoNecessidadeEspecialId = (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Cuidador;
                    avaliacao.NecessitaRecurso = rblNecessitaCuidador.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblNecessitaCuidador.SelectedValue == "1" ? true : false;
                    avaliacao.Transitorio = rblTipoCuidador.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblTipoCuidador.SelectedValue == "1" ? true : false;
                    avaliacao.DataInicio = !string.IsNullOrEmpty(dtInicioCuidador.Text.Trim())
                                               ? dtInicioCuidador.Date
                                               : (DateTime?)null;
                    avaliacao.DataFim = !string.IsNullOrEmpty(dtFimCuidador.Text.Trim())
                                               ? dtFimCuidador.Date
                                               : (DateTime?)null;
                    avaliacao.Justificativa = txtJustificativaCuidador.Text == string.Empty ? null : txtJustificativaCuidador.Text.Trim();
                    avaliacao.UsuarioId = User.Identity.Name;
                    ListaAvaliacao.Add(avaliacao);
                }
                if (!rblNecessitaLedor.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
                    avaliacao.AvaliacaoNapesId = hdnCodigoLedor.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnCodigoLedor.Value);
                    avaliacao.AlunoId = lblAluno.Text.Trim();
                    avaliacao.TipoRecursoNecessidadeEspecialId = (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Ledor;
                    avaliacao.NecessitaRecurso = rblNecessitaLedor.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblNecessitaLedor.SelectedValue == "1" ? true : false;
                    avaliacao.Transitorio = rblTipoLedor.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblTipoLedor.SelectedValue == "1" ? true : false;

                    avaliacao.DataInicio = !string.IsNullOrEmpty(dtInicioLedor.Text.Trim())
                                               ? dtInicioLedor.Date
                                               : (DateTime?)null;
                    avaliacao.DataFim = !string.IsNullOrEmpty(dtFimLedor.Text.Trim())
                                               ? dtFimLedor.Date
                                               : (DateTime?)null;
                    avaliacao.Justificativa = txtJustificativaLedor.Text == string.Empty ? null : txtJustificativaLedor.Text.Trim();
                    avaliacao.UsuarioId = User.Identity.Name;
                    ListaAvaliacao.Add(avaliacao);
                }
                if (!rblNecessitaInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
                    avaliacao.AvaliacaoNapesId = hdnCodigoInterprete.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnCodigoInterprete.Value);
                    avaliacao.AlunoId = lblAluno.Text.Trim();
                    avaliacao.TipoRecursoNecessidadeEspecialId = (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.Interprete;
                    avaliacao.NecessitaRecurso = rblNecessitaInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblNecessitaInterprete.SelectedValue == "1" ? true : false;
                    avaliacao.Transitorio = rblTipoInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblTipoInterprete.SelectedValue == "1" ? true : false;
                    avaliacao.DataInicio = !string.IsNullOrEmpty(dtInicioInterprete.Text.Trim())
                                               ? dtInicioInterprete.Date
                                               : (DateTime?)null;
                    avaliacao.DataFim = !string.IsNullOrEmpty(dtFimInterprete.Text.Trim())
                                               ? dtFimInterprete.Date
                                               : (DateTime?)null;
                    avaliacao.Justificativa = txtJustificativaInterprete.Text == string.Empty ? null : txtJustificativaInterprete.Text.Trim();
                    avaliacao.UsuarioId = User.Identity.Name;
                    ListaAvaliacao.Add(avaliacao);
                }

                if (!rblNecessitaSalaRecurso.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
                    avaliacao.AvaliacaoNapesId = hdnCodigoSalaRecurso.Value.IsNullOrEmptyOrWhiteSpace() ? 99999 : Convert.ToInt32(hdnCodigoSalaRecurso.Value);
                    avaliacao.AlunoId = lblAluno.Text.Trim();
                    avaliacao.TipoRecursoNecessidadeEspecialId = (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.SalaRecurso;
                    avaliacao.NecessitaRecurso = rblNecessitaSalaRecurso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblNecessitaSalaRecurso.SelectedValue == "1" ? true : false;
                    avaliacao.Transitorio = rblTipoSalaRecurso.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblTipoSalaRecurso.SelectedValue == "1" ? true : false;
                    avaliacao.DataInicio = !string.IsNullOrEmpty(dtInicioSalaRecurso.Text.Trim())
                                               ? dtInicioSalaRecurso.Date
                                               : (DateTime?)null;
                    avaliacao.DataFim = !string.IsNullOrEmpty(dtFimSalaRecurso.Text.Trim())
                                               ? dtFimSalaRecurso.Date
                                               : (DateTime?)null;
                    avaliacao.Justificativa = txtJustificativaSalaRecurso.Text == string.Empty ? null : txtJustificativaSalaRecurso.Text.Trim();
                    avaliacao.UsuarioId = User.Identity.Name;
                    
                    ListaAvaliacao.Add(avaliacao);
                }

                if (!rblNecessitaPAPEE.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    avaliacao = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.AvaliacaoNapes();
                    avaliacao.AvaliacaoNapesId = hdnCodigoPAPEE.Value.IsNullOrEmptyOrWhiteSpace() ? 99999 : Convert.ToInt32(hdnCodigoPAPEE.Value);
                    avaliacao.AlunoId = lblAluno.Text.Trim();
                    avaliacao.TipoRecursoNecessidadeEspecialId = (int)Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial.TipoRecurso.PAPEE;
                    avaliacao.NecessitaRecurso = rblNecessitaPAPEE.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblNecessitaPAPEE.SelectedValue == "1" ? true : false;
                    avaliacao.Transitorio = rblTipoPAPEE.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (Boolean?)null : rblTipoPAPEE.SelectedValue == "1" ? true : false;
                    avaliacao.DataInicio = !string.IsNullOrEmpty(dtInicioPAPEE.Text.Trim())
                                               ? dtInicioPAPEE.Date
                                               : (DateTime?)null;
                    avaliacao.DataFim = !string.IsNullOrEmpty(dtFimPAPEE.Text.Trim())
                                               ? dtFimPAPEE.Date
                                               : (DateTime?)null;
                    avaliacao.Justificativa = txtJustificativaPAPEE.Text == string.Empty ? null : txtJustificativaPAPEE.Text.Trim();
                    avaliacao.UsuarioId = User.Identity.Name;
                    ListaAvaliacao.Add(avaliacao);
                }
                validacao = rnAvaliacaoNapes.Valida(ListaAvaliacao, hdnAvaliacao.Value == "Pendente" ? true : false);

                if (validacao.Valido)
                {
                    if (hdnAvaliacao.Value == "Pendente")
                    {
                        rnAvaliacaoNapes.Insere(ListaAvaliacao);
                        odsAvaliacao.DataBind();
                        grdAvaliacao.DataBind();
                    }
                    else
                    {
                        rnAvaliacaoNapes.Atualiza(ListaAvaliacao);
                    }
                    lblMensagem.Text = "Avaliação " + ((hdnAvaliacao.Value == "Pendente") ? "incluída" : "alterada") + " com sucesso.";
                    PnlTiposAvaliacao.Visible = false;
                    pnlAvaliacao.Visible = true;
                    LimpaCampos();
                }
                else
                {
                    if (validacao.Mensagem != null)
                    {
                        lblMensagemPopup.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemPopup.Text = ex.Message;
            }
        }

        protected void rblNecessitaCuidador_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoCuidador.ClearSelection();
            dtInicioCuidador.Text = string.Empty;
            dtFimCuidador.Text = string.Empty;
            txtJustificativaCuidador.Text = string.Empty;
            trVigenciaCuidador.Visible = true;
            trTipoCuidador.Visible = true;

            if (rblNecessitaCuidador.SelectedValue == "0")
            {
                trTipoCuidador.Visible = false;
                trVigenciaCuidador.Visible = false;
            }
        }

        protected void rblNecessitaLedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoLedor.ClearSelection();
            dtInicioLedor.Text = string.Empty;
            dtFimLedor.Text = string.Empty;
            txtJustificativaLedor.Text = string.Empty;
            trVigenciaLedor.Visible = true;
            trTipoLedor.Visible = true;

            if (rblNecessitaLedor.SelectedValue == "0")
            {
                trTipoLedor.Visible = false;
                trVigenciaLedor.Visible = false;
            }
        }

        protected void rblNecessitaInterprete_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoInterprete.ClearSelection();
            dtInicioInterprete.Text = string.Empty;
            dtFimInterprete.Text = string.Empty;
            txtJustificativaInterprete.Text = string.Empty;
            trVigenciaInterprete.Visible = true;
            trTipoInterprete.Visible = true;

            if (rblNecessitaInterprete.SelectedValue == "0")
            {
                trTipoInterprete.Visible = false;
                trVigenciaInterprete.Visible = false;
            }
        }

        protected void rblNecessitaSalaRecurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoSalaRecurso.ClearSelection();
            dtInicioSalaRecurso.Text = string.Empty;
            dtFimSalaRecurso.Text = string.Empty;
            txtJustificativaSalaRecurso.Text = string.Empty;
            trVigenciaSalaRecurso.Visible = true;
            trTipoSalaRecurso.Visible = true;
           
            if (rblNecessitaSalaRecurso.SelectedValue == "0")
            {
                trTipoSalaRecurso.Visible = false;
                trVigenciaSalaRecurso.Visible = false;
                
            }
        }

        protected void rblNecessitaPAPEE_SelectedIndexChanged(object sender, EventArgs e)
        {
            rblTipoPAPEE.ClearSelection();
            dtInicioPAPEE.Text = string.Empty;
            dtFimPAPEE.Text = string.Empty;
            txtJustificativaPAPEE.Text = string.Empty;
            trVigenciaPAPEE.Visible = true;
            trTipoPAPEE.Visible = true;

            if (rblNecessitaPAPEE.SelectedValue == "0")
            {
                trTipoPAPEE.Visible = false;
                trVigenciaPAPEE.Visible = false;
            }
        }

        protected void rblTipoCuidador_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtInicioCuidador.Text = string.Empty;
            dtFimCuidador.Text = string.Empty;
            trVigenciaCuidador.Visible = true;

            if (rblTipoCuidador.SelectedValue == "0")
            {
                trVigenciaCuidador.Visible = false;
            }
        }

        protected void rblTipoLedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtInicioLedor.Text = string.Empty;
            dtFimLedor.Text = string.Empty;
            trVigenciaLedor.Visible = true;

            if (rblTipoLedor.SelectedValue == "0")
            {
                trVigenciaLedor.Visible = false;
            }
        }

        protected void rblTipoInterprete_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtInicioInterprete.Text = string.Empty;
            dtFimInterprete.Text = string.Empty;
            trVigenciaInterprete.Visible = true;

            if (rblTipoInterprete.SelectedValue == "0")
            {
                trVigenciaInterprete.Visible = false;
            }
        }

        protected void rblTipoSalaRecurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtInicioSalaRecurso.Text = string.Empty;
            dtFimSalaRecurso.Text = string.Empty;
            trVigenciaSalaRecurso.Visible = true;

            if (rblTipoSalaRecurso.SelectedValue == "0")
            {
                trVigenciaSalaRecurso.Visible = false;
            }
        }

        protected void rblTipoPAPEE_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtInicioPAPEE.Text = string.Empty;
            dtFimPAPEE.Text = string.Empty;
            trVigenciaPAPEE.Visible = true;

            if (rblTipoPAPEE.SelectedValue == "0")
            {
                trVigenciaPAPEE.Visible = false;
            }
        }

        private void HabilitaCamposAvaliacao()
        {
            trTipoCuidador.Visible = true;
            trTipoInterprete.Visible = true;
            trTipoLedor.Visible = true;
            trVigenciaCuidador.Visible = true;
            trVigenciaInterprete.Visible = true;
            trVigenciaLedor.Visible = true;
        }

        private void LimpaCampos()
        {
            hdnCodigoCuidador.Value = string.Empty;
            hdnCodigoInterprete.Value = string.Empty;
            hdnCodigoLedor.Value = string.Empty;
            hdnCodigoSalaRecurso.Value = string.Empty;
            hdnCodigoPAPEE.Value = string.Empty;
            rblNecessitaCuidador.ClearSelection();
            rblTipoCuidador.ClearSelection();
            dtInicioCuidador.Text = string.Empty;
            dtFimCuidador.Text = string.Empty;
            txtJustificativaCuidador.Text = string.Empty;
            rblNecessitaLedor.ClearSelection();
            rblTipoLedor.ClearSelection();
            dtInicioLedor.Text = string.Empty;
            dtFimLedor.Text = string.Empty;
            txtJustificativaLedor.Text = string.Empty;
            rblNecessitaInterprete.ClearSelection();
            rblTipoInterprete.ClearSelection();
            dtInicioInterprete.Text = string.Empty;
            dtFimInterprete.Text = string.Empty;
            txtJustificativaInterprete.Text = string.Empty;
            rblNecessitaSalaRecurso.ClearSelection();
            rblTipoSalaRecurso.ClearSelection();
            dtInicioSalaRecurso.Text = string.Empty;
            dtFimSalaRecurso.Text = string.Empty;
            txtJustificativaSalaRecurso.Text = string.Empty;           
            rblNecessitaPAPEE.ClearSelection();
            rblTipoPAPEE.ClearSelection();
            dtInicioPAPEE.Text = string.Empty;
            dtFimPAPEE.Text = string.Empty;
            txtJustificativaPAPEE.Text = string.Empty;
        }
    }
}
