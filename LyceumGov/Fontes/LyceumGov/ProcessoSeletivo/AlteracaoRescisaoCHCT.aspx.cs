using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Techne.Data;
using System.Collections.Generic;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.CR;
using Techne.Controls;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/AlteracaoRescisaoCHCT.aspx"),
    ControlText("AlteracaoRescisaoCHCT"),
    Title("Alteração CH/Rescisão Contrato"),]
    public partial class AlteracaoRescisaoCHCT : TPage
    {
        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DesabilitaTsearchs();
                divOpcao.Visible = false;
                lblMensagem.Text = string.Empty;
                ddlSelecionarOpcao.Enabled = false;

                bool bolEhContratoTempo = RN.PadroesDeAcessos.ConsultarPadacesContratoTempoPorUsuario(User.Identity.Name);

                if (!bolEhContratoTempo)
                {
                    int intRegional = RN.Usuarios.RetornarRegionalUsuario(User.Identity.Name);
                    tseCandidatoBusca.SqlWhere = tseCandidatoBusca.SqlWhere + " AND REGIONALID = " + intRegional;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCandidatoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                LimparCampoCandidato();
                if (!this.tseCandidatoBusca.DBValue.IsNull && !tseConcursoBusca.DBValue.IsNull)
                {
                    if (this.tseCandidatoBusca.IsValidDBValue && tseConcursoBusca.IsValidDBValue)
                    {
                        ddlSelecionarOpcao.Enabled = true;
                    }
                }
                else
                {
                    lblMensagem.Text = "Candidato não encontrado.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSelecionarOpcao_SelectedIndexChanged(object sender, EventArgs args)
        {
            try
            {
                pnCamposDocente.Visible = false;
                divOpcao.Visible = false;
                if (tseCandidatoBusca.DBValue.IsNull || tseConcursoBusca.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor informar processo seletivo e ID/Vínculo ou matrícula.";
                    return;
                }

                if (ddlSelecionarOpcao.SelectedValue == "0")
                {
                    ddlSelecionarOpcao.Enabled = true;
                    lblMensagem.Text = "Selecionar: Escolha uma das opções.";
                    return;
                }
                else if (ddlSelecionarOpcao.SelectedValue == "1")
                {
                    CarregarDadosRescisao(tseConcursoBusca.Value.ToString(), tseCandidatoBusca["candidato"].ToString());
                }
                else
                {
                    CarregarDadosAlteracao(tseConcursoBusca.Value.ToString(), tseCandidatoBusca["candidato"].ToString());
                }

                MontarTela();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseConcursoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                LimparCampoCandidato();
                if (this.tseConcursoBusca.DBValue.IsNull || !this.tseConcursoBusca.IsValidDBValue)
                {
                    lblMensagem.Text = "Processo Seletivo não encontrado.";
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
                LimparCampoSolicitacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAprovar_Click(object sender, EventArgs e)
        {
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
            LyLotacao lotacao = new LyLotacao();
            LyDocente docente = new LyDocente();
            List<string> mensagens = new List<string>();

            try
            {
                if (dtExercicio.Date == DateTime.MinValue)
                {
                    mensagens.Add("Data do último exercício: Preenchimento obrigatório.");
                }
                else if (dtExercicio.Date > DateTime.Now || dtExercicio.Date < dtdataProposta.Date)
                {
                    mensagens.Add("Data do último exercício: Data inválida! Escolha uma data entre a data de admissão e a atual.");
                }

                if (txtJustificativa.Text.Trim() == string.Empty)
                {
                    mensagens.Add("Justificativa: Preenchimento obrigatório.");
                }
                if (txtSituacao.Text.Equals(RN.ProcessoSeletivo.Status.ContratoRescindido.GetStringValue()))
                {
                    mensagens.Add("Contrato já foi rescindido.");
                }

                Int64 aulasAlocadas = -1;
                Int64.TryParse(txtAulasAlocadas.Text, out aulasAlocadas);
                if (aulasAlocadas > 0)
                {
                    mensagens.Add("Rescisão não permitida: existem aulas alocadas para o docente.");
                }

                if (mensagens.Count == 0)
                {
                    docente = RN.Docentes.Carregar(Convert.ToInt32(tseCandidatoBusca["num_func"].ToString()));

                    lotacao.DataDesativacao = dtExercicio.Date;
                    lotacao.Matricula = txtMatricula.Text;
                    lotacao.Pessoa = Convert.ToInt32(tseCandidatoBusca["pessoa"].ToString());
                    lotacao.Usuario = User.Identity.Name;

                    RN.ProcessoSeletivo.AprovarRescisao(lotacao, docente, tseCandidatoBusca["num_func"].ToString(), txtJustificativa.Text);

                    lblMensagem.Text = "Rescisão efetuada com sucesso.";
                    btnAprovar.Enabled = false;
                    dtExercicio.ReadOnly = true;
                    txtJustificativa.Enabled = false;
                    btnAprovar.Enabled = false;
                    Imprimir("Rescisao");
                }
                else
                {
                    lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAprovarAlteracao_Click(object sender, EventArgs e)
        {
            string strTipo = string.Empty;
            List<string> mensagens = new List<string>();

            try
            {
                if (ddlNovaCargaHoraria.SelectedIndex == 0)
                {
                    mensagens.Add("Nova Carga Horária: Preenchimento obrigatório.");
                }
                else if (Convert.ToInt32(ddlNovaCargaHoraria.SelectedValue) < Convert.ToInt32(txtAulasAlocadas.Text))
                {
                    mensagens.Add("Nova Carga Horária: A nova carga horária não pode ser menor que a carga alocada para o docente.");
                }

                if (mensagens.Count == 0)
                {
                    if (Convert.ToInt32(ddlNovaCargaHoraria.SelectedItem.Text) > Convert.ToInt32(txtCargaHoraria.Text))
                        strTipo = "Proposta de Ampliação de Carga Horária Reprovada";
                    else
                        strTipo = "Solicitação de Redução de Carga Horária Aprovada";

                    RN.ProcessoSeletivo.AprovarAlteracaoCargaHoraria(tseConcursoBusca.DBValue.ToString(), tseCandidatoBusca["candidato"].ToString(), txtCargaHoraria.Text, ddlNovaCargaHoraria.SelectedItem.Text, strTipo);

                    lblMensagem.Text = "Carga Horária alterada com sucesso.";
                    btnAprovarAlteracao.Enabled = false;
                    ddlNovaCargaHoraria.Enabled = false;
                    Imprimir("Alteracao");
                }
                else
                {
                    lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Métodos

        private void CarregarDadosRescisao(string concurso, string candidato)
        {
            DataTable dtCandidato = new DataTable();
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
            if (!string.IsNullOrEmpty(concurso) && !string.IsNullOrEmpty(candidato))
            {
                dtCandidato = rnProcessoSeletivo.ObtemCandidatoAvaliacaoRescisaoPor(concurso, candidato);

                if (dtCandidato.Rows.Count > 0)
                {
                    PreencherDadosTela(dtCandidato);
                }
                else
                {
                    lblMensagem.Text = "Nenhum resultado encontrado.";
                    divOpcao.Visible = false;
                    pnCamposDocente.Visible = false;
                }
            }
        }

        private void DesabilitaTsearchs()
        {
            tseCoordenadoria.Mode = ControlMode.View;
            tseRegional.Mode = ControlMode.View;
            tseDisciplina.Mode = ControlMode.View;
            tseMunicipio.Mode = ControlMode.View;
            tseUnidadeResponsavel.Mode = ControlMode.View;
            tseCoordenadoria.Visible = !tseRegional.IsValidDBValue;
            tseRegional.Visible = !tseCoordenadoria.IsValidDBValue;
        }

        private void CarregarDadosAlteracao(string concurso, string candidato)
        {
            DataTable dtCandidato = new DataTable();
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
            divOpcao.Visible = false;
            pnCamposDocente.Visible = false;

            if (!string.IsNullOrEmpty(concurso) && !string.IsNullOrEmpty(candidato))
            {
                dtCandidato = rnProcessoSeletivo.ObtemCandidatoAvaliacaoRHPor(concurso, candidato);
                if (dtCandidato.Rows.Count > 0)
                {
                    PreencherDadosTela(dtCandidato);
                    divOpcao.Visible = true;
                    pnCamposDocente.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Nenhum resultado encontrado";
                }
            }
        }

        private void PreencherDadosTela(DataTable qt)
        {
            txtDocente.Text = Convert.ToString(qt.Rows[0]["nome"]);
            txtCPF.Text = Convert.ToString(qt.Rows[0]["cpf"]);
            txtMatricula.Text = Convert.ToString(qt.Rows[0]["matricula"]);
            txtIdFuncional.Text = qt.Rows[0]["IDVINCULO"].ToString();
            txtCotas.Text = qt.Rows[0]["cota"].ToString();

            if (qt.Rows[0]["DATAADMISSAO"] != null)
                dtdataProposta.Date = Convert.ToDateTime(qt.Rows[0]["DATAADMISSAO"]);

            txtSituacao.Text = ((RN.ProcessoSeletivo.Status)Enum.Parse(typeof(RN.ProcessoSeletivo.Status), Convert.ToString(qt.Rows[0]["situacao"]))).GetStringValue();

            if (qt.Rows[0]["regionalid"] != DBNull.Value)
            {
                lblCoordenadoria.Text = "Regional: ";
                tseRegional.Visible = true;
                tseRegional.Value = Convert.ToString(qt.Rows[0]["regionalid"]);
                tseCoordenadoria.Visible = false;
            }
            else
            {
                tseRegional.Visible = false;
                lblCoordenadoria.Text = "Coordenadoria: ";
                tseCoordenadoria.Value = Convert.ToString(qt.Rows[0]["coordenadoriaid"]);
                tseCoordenadoria.Visible = true;
            }

            if (qt.Rows[0]["municipio_proc"] != DBNull.Value)
                tseMunicipio.Value = Convert.ToString(qt.Rows[0]["municipio_proc"]);

            if (qt.Rows[0]["unidade_ens"] != DBNull.Value)
                tseUnidadeResponsavel.Value = Convert.ToString(qt.Rows[0]["unidade_ens"]);

            if (qt.Rows[0]["disciplina"] != DBNull.Value)
                tseDisciplina.Value = Convert.ToString(qt.Rows[0]["disciplina"]);

            txtCargo.Text = qt.Rows[0]["cargo"].ToString();
            txtCargaHoraria.Text = !String.IsNullOrEmpty(Convert.ToString(qt.Rows[0]["carga_horaria"])) ? Convert.ToString(qt.Rows[0]["carga_horaria"]) : "0";
            txtAulasAlocadas.Text = RN.ProcessoSeletivo.ConsultarAulasAlocadasRH(
                tseConcursoBusca.DBValue.ToString(), tseCandidatoBusca["candidato"].ToString()).ToString();

            if (ddlSelecionarOpcao.SelectedValue == "1")
            {
                if (qt.Rows[0]["DATAULTIMOEXERCICIO"] != DBNull.Value)
                    dtExercicio.Date = Convert.ToDateTime(qt.Rows[0]["DATAULTIMOEXERCICIO"]);
                txtJustificativa.Text = "Rescisão aprovada pela COSEP e pela CDGP";
            }
            else if (ddlSelecionarOpcao.SelectedValue == "2")
            {
                CarregarCargaHoraria();
            }

            DesabilitaTsearchs();
        }

        private void LimparCampoSolicitacao()
        {
            tseConcursoBusca.ResetValue();
            tseCandidatoBusca.ResetValue();
            LimparCampoCandidato();
        }

        private void LimparCampoCandidato()
        {
            ddlSelecionarOpcao.SelectedIndex = 0;
            LimparCampoSelecionar();
        }

        private void LimparCampoSelecionar()
        {
            txtDocente.Text = string.Empty;
            tseDisciplina.ResetValue();
            txtCPF.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            dtdataProposta.Text = string.Empty;
            tseCoordenadoria.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            txtCargaHoraria.Text = string.Empty;
            txtAulasAlocadas.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            pnCamposDocente.Visible = false;
            lblMensagem.Text = string.Empty;
            divOpcao.Visible = false;
            dtExercicio.Text = string.Empty;
            dtExercicio.ReadOnly = false;
        }

        private void MontarTela()
        {
            pnCamposDocente.Visible = true;
            divOpcao.Visible = true;
            btnImprimir.Visible = false;
            btnCancelar.Enabled = true;
            ddlSelecionarOpcao.Enabled = true;

            if (ddlSelecionarOpcao.SelectedValue == "1")
            {
                lblBlocoSolicitacao.Text = "Solicitação de Rescisão";
                lblJustificativa.Visible = true;
                txtJustificativa.Visible = true;
                lblDtExercicio.Visible = true;
                dtExercicio.Visible = true;
                dtExercicio.ReadOnly = false;
                lblNovaCargaHoraria.Visible = false;
                ddlNovaCargaHoraria.Visible = false;
                btnAprovar.Visible = true;
                btnAprovar.Enabled = true;
                btnAprovarAlteracao.Visible = false;
            }
            else if (ddlSelecionarOpcao.SelectedValue == "2")
            {
                lblBlocoSolicitacao.Text = "Alteração da carga horária";
                lblJustificativa.Visible = false;
                txtJustificativa.Visible = false;
                lblDtExercicio.Visible = false;
                dtExercicio.Visible = false;
                lblNovaCargaHoraria.Visible = true;
                ddlNovaCargaHoraria.Visible = true;
                ddlNovaCargaHoraria.Enabled = true;
                btnAprovar.Visible = false;
                btnAprovarAlteracao.Visible = true;
                btnAprovarAlteracao.Enabled = true;
            }
        }

        private void CarregarCargaHoraria()
        {
            DataTable dtCarga = RN.ProcessoSeletivo.ConsultarCargaHorariaPor(tseConcursoBusca.DBValue.ToString(), txtCargo.Text);
            int intMenorValor = Convert.ToInt32(dtCarga.Rows[0].ItemArray[1]);
            int intCargaEfetiva = Convert.ToInt32(dtCarga.Rows[0].ItemArray[0]);

            ddlNovaCargaHoraria.Items.Clear();

            for (int i = intMenorValor; i <= intCargaEfetiva; i++)
            {
                ddlNovaCargaHoraria.Items.Add(i.ToString());
            }

            ddlNovaCargaHoraria.DataBind();
            ddlNovaCargaHoraria.Items.Insert(0, new ListItem("Selecione", string.Empty));
            ddlNovaCargaHoraria.SelectedIndex = 0;
        }

        private void Imprimir(string strTipo)
        {
            btnCancelar.Enabled = false;
            divOpcao.Visible = true;
            btnImprimir.Visible = true;

            IDictionary<string, string> pares = new Dictionary<string, string>();
            pares.Add("tipo", strTipo);
            pares.Add("candidato", tseCandidatoBusca["candidato"].ToString());
            pares.Add("concurso", tseConcursoBusca.DBValue.ToString());
            btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=rsrescisaoealteracao&grp=processoseletivo&" + TPage.CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
        }

        #endregion

    }
}
