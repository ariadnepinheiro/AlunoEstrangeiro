using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.Net.Academico
{
    [
        NavUrl("~/Academico/ModalidadeSerieMatricula.aspx"),
         ControlText("ModalidadeSerieMatricula"),
         Title("Modalidade/Série - Matrícula"),
        ]

    public partial class ModalidadeSerieMatricula : TPage
    {
        public object Listar(object unidade_ens, object ano, object periodo)
        {
            var ue = unidade_ens.ToString();

            if (ue != null
                && (ano != null && ano.ToString() != "Selecione")
                && (periodo != null && periodo.ToString() != "Selecione"))
            {
                return CtvConfTurnoInicial.Listar(ue, Convert.ToInt32(ano), Convert.ToInt32(periodo));
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdModalidade, "Modalidade/Série - Matrícula");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                ddlAno.DataSource = RN.CtvAgendaConfTurnoVaga.ListarAnos();
                ddlAno.Items.Insert(0, "Selecione");
                ddlAno.DataBind();
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }

            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            LimparCampos();

            if (!tseUnidadeResponsavel.DBValue.IsNull)
            {
                if (tseUnidadeResponsavel.IsValidDBValue)
                {
                    if (string.IsNullOrEmpty(ddlAno.SelectedValue) || ddlAno.SelectedValue == "Selecione")
                    {
                        lblMensagem.Text = "Favor selecionar o Ano.";
                        return;
                    }
                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        cmbModalidade.Items.Clear();
                        cmbModalidade.DataSource = RN.Curso.ObtemModalidadeAgendaPor(Convert.ToString(tseUnidadeResponsavel.DBValue), int.Parse(ddlAno.SelectedItem.Text), int.Parse(ddlPeriodo.SelectedValue));
                        cmbModalidade.Items.Insert(0, "Selecione");
                        cmbModalidade.DataBind();
                    }
                    grdModalidade.Visible = true;
                    lblMensagem.Text = string.Empty;
                    pnGrid.Visible = true;
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }
                    grdModalidade.Visible = false;
                    lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    pnGrid.Visible = false;
                }
            }
            else
            {
                if (sessao != null)
                {
                    sessao.Escola = string.Empty;
                    sessao.Municipio = string.Empty;
                    sessao.Coordenadoria = string.Empty;
                }

                grdModalidade.Visible = false;
                lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                pnGrid.Visible = false;
            }

            updatePanel3.Update();
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Clear();
            cmbModalidade.Items.Clear();
            cmbNivel.Items.Clear();
            cmbEscolaridade.Items.Clear();
            cmbSerie.Items.Clear();
            txtPropostaVagasContinuidade.Text = string.Empty;
            txtPropostaVagasNovas.Text = string.Empty;
            tseUnidadeResponsavel.ResetValue();
            LimparCheckbox();
            grdModalidade.Visible = false;

            if (ddlAno.SelectedValue != "Selecione")
            {
                this.ddlPeriodo.DataSource = RN.CtvAgendaConfTurnoVaga.ListarPeriodo(Convert.ToInt32(this.ddlAno.SelectedValue));
                this.ddlPeriodo.Items.Insert(0, "Selecione");
                this.ddlPeriodo.DataBind();
            }

            updatePanel3.Update();
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbModalidade.Items.Clear();
            cmbNivel.Items.Clear();
            cmbEscolaridade.Items.Clear();
            cmbSerie.Items.Clear();
            txtPropostaVagasContinuidade.Text = string.Empty;
            txtPropostaVagasNovas.Text = string.Empty;
            tseUnidadeResponsavel.ResetValue();
            LimparCheckbox();
            grdModalidade.Visible = false;

            updatePanel3.Update();
        }

        protected void cmbModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbNivel.Items.Clear();
            cmbEscolaridade.Items.Clear();
            cmbSerie.Items.Clear();
            txtPropostaVagasContinuidade.Text = string.Empty;
            txtPropostaVagasNovas.Text = string.Empty;
            LimparCheckbox();

            if (cmbModalidade.SelectedValue != "Selecione")
            {
                cmbNivel.DataSource = RN.Curso.ObtemNivelAgendaPor(Convert.ToString(tseUnidadeResponsavel.DBValue), int.Parse(ddlAno.SelectedItem.Text), int.Parse(ddlPeriodo.SelectedValue));
                cmbNivel.Items.Insert(0, "Selecione");
                cmbNivel.DataBind();
            }
            updatePanel3.Update();
        }

        protected void cmbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbEscolaridade.Items.Clear();
            cmbSerie.Items.Clear();
            txtPropostaVagasContinuidade.Text = string.Empty;
            txtPropostaVagasNovas.Text = string.Empty;
            LimparCheckbox();

            if ((cmbNivel.SelectedValue != "Selecione") && (cmbModalidade.SelectedValue != "Selecione"))
            {
                cmbEscolaridade.DataSource = RN.Curso.ObtemEscolaridadeAgendaPor(Convert.ToString(tseUnidadeResponsavel.DBValue), int.Parse(ddlAno.SelectedItem.Text), int.Parse(ddlPeriodo.SelectedValue), cmbNivel.SelectedValue, cmbModalidade.SelectedValue);
                cmbEscolaridade.Items.Insert(0, "Selecione");
                cmbEscolaridade.DataBind();
            }
            updatePanel3.Update();
        }

        protected void cmbEscolaridade_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSerie.Items.Clear();
            txtPropostaVagasContinuidade.Text = string.Empty;
            txtPropostaVagasNovas.Text = string.Empty;
            LimparCheckbox();

            if (cmbEscolaridade.SelectedValue != "Selecione")
            {
                cmbSerie.DataSource = RN.Curso.ObtemSerieAgendaPor(Convert.ToString(tseUnidadeResponsavel.DBValue), int.Parse(ddlAno.SelectedItem.Text), int.Parse(ddlPeriodo.SelectedValue), cmbEscolaridade.SelectedValue);
                cmbSerie.Items.Insert(0, "Selecione");
                cmbSerie.DataBind();
            }
            updatePanel3.Update();
        }

        protected void cmbSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            CtvPropostaSeeduc rnCtvPropostaSeeduc = new CtvPropostaSeeduc();
            TceCtvPropostaSeeduc proposta = new TceCtvPropostaSeeduc();
            txtPropostaVagasContinuidade.Text = string.Empty;
            txtPropostaVagasNovas.Text = string.Empty;
            LimparCheckbox();

            if (cmbSerie.SelectedValue != "Selecione")
            {
                DataTable dt = CtvConfTurnoInicial.ListarTurnosPor(Convert.ToString(tseUnidadeResponsavel.DBValue), int.Parse(ddlAno.SelectedItem.Text), int.Parse(ddlPeriodo.SelectedValue), cmbEscolaridade.SelectedValue, int.Parse(cmbSerie.SelectedValue));

                if (dt.Rows != null)
                {
                    foreach (DataRow linha in dt.Rows)
                    {
                        switch (linha["TURNO"].ToString())
                        {
                            case "M":
                                chkManha.Checked = true;
                                chkManha.Enabled = false;
                                chkManhaVC.Enabled = false;
                                chkManhaVN.Enabled = false;

                                if (linha["CONTINUIDADE"].ToString() == "1")
                                {
                                    chkManhaVC.Checked = true;
                                }

                                if (linha["NOVO"].ToString() == "1")
                                {
                                    chkManhaVN.Checked = true;
                                }

                                break;
                            case "T":
                                chkTarde.Checked = true;
                                chkTarde.Enabled = false;
                                chkTardeVC.Enabled = false;
                                chkTardeVN.Enabled = false;

                                if (linha["CONTINUIDADE"].ToString() == "1")
                                {
                                    chkTardeVC.Checked = true;
                                }

                                if (linha["NOVO"].ToString() == "1")
                                {
                                    chkTardeVN.Checked = true;
                                }
                                break;
                            case "N":
                                chkNoite.Checked = true;
                                chkNoite.Enabled = false;
                                chkNoiteVN.Enabled = false;
                                chkNoiteVC.Enabled = false;

                                if (linha["CONTINUIDADE"].ToString() == "1")
                                {
                                    chkNoiteVC.Checked = true;
                                }

                                if (linha["NOVO"].ToString() == "1")
                                {
                                    chkNoiteVN.Checked = true;
                                }
                                break;
                            case "I":
                                chkIntegral.Checked = true;
                                chkIntegral.Enabled = false;
                                chkIntegralVC.Enabled = false;
                                chkIntegralVN.Enabled = false;

                                if (linha["CONTINUIDADE"].ToString() == "1")
                                {
                                    chkIntegralVC.Checked = true;
                                }

                                if (linha["NOVO"].ToString() == "1")
                                {
                                    chkIntegralVN.Checked = true;
                                }
                                break;
                            case "A":
                                chkAmpliado.Checked = true;
                                chkAmpliado.Enabled = false;
                                chkAmpliadoVN.Enabled = false;
                                chkAmpliadoVC.Enabled = false;

                                if (linha["CONTINUIDADE"].ToString() == "1")
                                {
                                    chkAmpliadoVC.Checked = true;
                                }

                                if (linha["NOVO"].ToString() == "1")
                                {
                                    chkAmpliadoVN.Checked = true;
                                }
                                break;
                        }
                    }
                }

                //Busca proposta
                proposta = rnCtvPropostaSeeduc.ObtemPor(Convert.ToString(tseUnidadeResponsavel.DBValue), int.Parse(ddlAno.SelectedItem.Text), int.Parse(ddlPeriodo.SelectedValue), cmbEscolaridade.SelectedValue, int.Parse(cmbSerie.SelectedValue));
                if (proposta.IdAgendaConfTurnoVaga > 0)
                {
                    txtPropostaVagasContinuidade.Text = Convert.ToString(proposta.VagasContinuidade);
                    txtPropostaVagasNovas.Text = Convert.ToString(proposta.VagasNovas);
                }

                if (chkManha.Checked && chkTarde.Checked && chkNoite.Checked && chkIntegral.Checked && chkAmpliado.Checked)
                {
                    btnSalvar.Visible = false;
                    lblMensagem.Text = "Todos os turnos já constam disponíveis para confirmação de turnos/vagas.";
                }
                else
                {
                    btnSalvar.Visible = true;
                }
            }

            updatePanel3.Update();
        }

        private void LimparCheckbox()
        {
            chkManha.Checked = false;
            chkManhaVC.Checked = false;
            chkManhaVN.Checked = false;
            chkTarde.Checked = false;
            chkTardeVC.Checked = false;
            chkTardeVN.Checked = false;
            chkNoite.Checked = false;
            chkNoiteVC.Checked = false;
            chkNoiteVN.Checked = false;
            chkAmpliado.Checked = false;
            chkAmpliadoVC.Checked = false;
            chkAmpliadoVN.Checked = false;
            chkIntegral.Checked = false;
            chkIntegralVC.Checked = false;
            chkIntegralVN.Checked = false;

            chkManha.Enabled = true;
            chkManhaVC.Enabled = false;
            chkManhaVN.Enabled = false;
            chkTarde.Enabled = true;
            chkTardeVC.Enabled = false;
            chkTardeVN.Enabled = false;
            chkNoite.Enabled = true;
            chkNoiteVC.Enabled = false;
            chkNoiteVN.Enabled = false;
            chkAmpliado.Enabled = true;
            chkAmpliadoVC.Enabled = false;
            chkAmpliadoVN.Enabled = false;
            chkIntegral.Enabled = true;
            chkIntegralVC.Enabled = false;
            chkIntegralVN.Enabled = false;
        }

        private void LimparCampos()
        {
            lblMensagem.Text = string.Empty;
            cmbModalidade.ClearSelection();
            cmbNivel.Items.Clear();
            cmbEscolaridade.Items.Clear();
            cmbSerie.Items.Clear();
            txtPropostaVagasContinuidade.Text = string.Empty;
            txtPropostaVagasNovas.Text = string.Empty;
            LimparCheckbox();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
                DadosInclusaoModalidadeSerieTurnosVagas modalidadeSerie = new DadosInclusaoModalidadeSerieTurnosVagas();
                DadosTurnoInclusaoModalidadeSerie turno = new DadosTurnoInclusaoModalidadeSerie();
                List<DadosTurnoInclusaoModalidadeSerie> listaTurnos = new List<DadosTurnoInclusaoModalidadeSerie>();

                if (string.IsNullOrEmpty(ddlAno.SelectedValue) || ddlAno.SelectedValue == "Selecione")
                {
                    modalidadeSerie.Ano = -1;
                }
                else
                {
                    modalidadeSerie.Ano = Convert.ToInt32(ddlAno.SelectedValue);
                }

                if (string.IsNullOrEmpty(ddlPeriodo.SelectedValue) || ddlPeriodo.SelectedValue == "Selecione")
                {
                    modalidadeSerie.Periodo = -1;
                }
                else
                {
                    modalidadeSerie.Periodo = Convert.ToInt32(ddlPeriodo.SelectedValue);
                }

                if (string.IsNullOrEmpty(cmbSerie.SelectedValue) || cmbSerie.SelectedValue == "Selecione")
                {
                    modalidadeSerie.Serie = -1;
                }
                else
                {
                    modalidadeSerie.Serie = Convert.ToInt32(cmbSerie.SelectedValue);
                }

                modalidadeSerie.Censo = Convert.ToString(tseUnidadeResponsavel.DBValue);
                modalidadeSerie.Curso = Convert.ToString(cmbEscolaridade.SelectedValue);
                modalidadeSerie.UsuarioResponsavel = User.Identity.Name;
                modalidadeSerie.PropostaVagaContinuidade = !string.IsNullOrEmpty(txtPropostaVagasContinuidade.Text) ? int.Parse(txtPropostaVagasContinuidade.Text) : -1;
                modalidadeSerie.PropostaVagaNova = !string.IsNullOrEmpty(txtPropostaVagasNovas.Text) ? int.Parse(txtPropostaVagasNovas.Text) : -1;

                if (chkManha.Checked && chkManha.Enabled)
                {
                    turno = new DadosTurnoInclusaoModalidadeSerie();
                    turno.Turno = "M";
                    turno.Continuidade = chkManhaVC.Checked;
                    turno.Novo = chkManhaVN.Checked;
                    listaTurnos.Add(turno);
                }

                if (chkTarde.Checked && chkTarde.Enabled)
                {
                    turno = new DadosTurnoInclusaoModalidadeSerie();
                    turno.Turno = "T";
                    turno.Continuidade = chkTardeVC.Checked;
                    turno.Novo = chkTardeVN.Checked;
                    listaTurnos.Add(turno);
                }

                if (chkNoite.Checked && chkNoite.Enabled)
                {
                    turno = new DadosTurnoInclusaoModalidadeSerie();
                    turno.Turno = "N";
                    turno.Continuidade = chkNoiteVC.Checked;
                    turno.Novo = chkNoiteVN.Checked;
                    listaTurnos.Add(turno);
                }

                if (chkAmpliado.Checked && chkAmpliado.Enabled)
                {
                    turno = new DadosTurnoInclusaoModalidadeSerie();
                    turno.Turno = "A";
                    turno.Continuidade = chkAmpliadoVC.Checked;
                    turno.Novo = chkAmpliadoVN.Checked;
                    listaTurnos.Add(turno);
                }

                if (chkIntegral.Checked && chkIntegral.Enabled)
                {
                    turno = new DadosTurnoInclusaoModalidadeSerie();
                    turno.Turno = "I";
                    turno.Continuidade = chkIntegralVC.Checked;
                    turno.Novo = chkIntegralVN.Checked;
                    listaTurnos.Add(turno);
                }

                modalidadeSerie.ListaTurnos = listaTurnos;

                validacao = rnCtvConfTurno.ValidaInclusaoModalidadeSerie(modalidadeSerie);

                if (validacao.Valido)
                {
                    rnCtvConfTurno.SalvaInclusaoModalidadeSerie(modalidadeSerie);

                    LimparCampos();
                    odsModalidade.Select();
                    odsModalidade.DataBind();
                    grdModalidade.DataBind();

                    lblMensagem.Text = "Modalidade/Série incluída com sucesso.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('Modalidade/Série incluída com sucesso.');", true);
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

        protected void chkManha_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManha.Checked)
            {
                chkManhaVC.Enabled = true;
                chkManhaVN.Enabled = true;
            }
            else
            {
                chkManhaVC.Enabled = false;
                chkManhaVN.Enabled = false;
                chkManhaVC.Checked = false;
                chkManhaVN.Checked = false;
            }
        }

        protected void chkTarde_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTarde.Checked)
            {
                chkTardeVC.Enabled = true;
                chkTardeVN.Enabled = true;
            }
            else
            {
                chkTardeVC.Enabled = false;
                chkTardeVN.Enabled = false;
                chkTardeVC.Checked = false;
                chkTardeVN.Checked = false;
            }
        }

        protected void chkNoite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoite.Checked)
            {
                chkNoiteVC.Enabled = true;
                chkNoiteVN.Enabled = true;
            }
            else
            {
                chkNoiteVC.Enabled = false;
                chkNoiteVN.Enabled = false;
                chkNoiteVC.Checked = false;
                chkNoiteVN.Checked = false;
            }
        }

        protected void chkAmpliado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAmpliado.Checked)
            {
                chkAmpliadoVC.Enabled = true;
                chkAmpliadoVN.Enabled = true;
            }
            else
            {
                chkAmpliadoVC.Enabled = false;
                chkAmpliadoVN.Enabled = false;
                chkAmpliadoVC.Checked = false;
                chkAmpliadoVN.Checked = false;
            }
        }

        protected void chkIntegral_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIntegral.Checked)
            {
                chkIntegralVC.Enabled = true;
                chkIntegralVN.Enabled = true;
            }
            else
            {
                chkIntegralVC.Enabled = false;
                chkIntegralVN.Enabled = false;
                chkIntegralVC.Checked = false;
                chkIntegralVN.Checked = false;
            }
        }
    }
}
