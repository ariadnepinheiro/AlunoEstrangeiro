using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
using Techne.Data;
using Techne.Lyceum.RN.Agenda.Entidades;
using Techne.Lyceum.RN.Util;
using System.Drawing;
using System.Collections;
using DevExpress.Web.ASPxEditors;
using System.Text.RegularExpressions;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/ConfirmacaoTurnosVagas.aspx"), ControlText("ConfirmacaoTurnosVagas"), Title("Confirmação de Turnos e Vagas")]

    public partial class ConfirmacaoTurnosVagas : TPage
    {
        #region Variaveis de Sessao

        public string PerfilUsuario
        {
            get { return Page.Session["perfil"] as string; }
            set { (Page.Session["perfil"]) = value; }
        }

        // Itens do DropDownList (fixo)
        public List<DadosTurmaConfVaga> Turmas
        {
            get { return Page.Session["turmas"] as List<DadosTurmaConfVaga>; }
            set { (Page.Session["turmas"]) = value; }
        }

        // Itens do da turma utilizados
        public List<int> IndicesUtilizados
        {
            get { return Page.Session["indices"] as List<int>; }
            set { (Page.Session["indices"]) = value; }
        }

        // String: ddl_cities_[column]_[row]
        // List<int>: todos os índices de Cidades que ele deve mostrar
        public Dictionary<String, List<int>> Valores
        {
            get { return Page.Session["valores"] as Dictionary<String, List<int>>; }
            set { (Page.Session["valores"]) = value; }
        }

        // String: ddl_cities_[column]_[row]
        // int[0]: indice da cidade antes da alteração de seleção do item na combo
        // int[1]: valor da textBoxVC da respectiva [column] e [row] antes da alteração de valor da mesma
        // int[2]: valor da textBoxVN da respectiva [column] e [row] antes da alteração de valor da mesma
        public Dictionary<String, int[]> Controle
        {
            get { return Page.Session["controle"] as Dictionary<String, int[]>; }
            set { (Page.Session["controle"]) = value; }
        }

        //Recebe uma lista de Confirmação de Vagas. Atualizada de acordo com as mudanças na grid de vagas
        public List<DadosAgendaVagas> Confirmacao
        {
            get { return Page.Session["confirmacao"] as List<DadosAgendaVagas>; }
            set { (Page.Session["confirmacao"]) = value; }
        }

        //Recebe uma lista de DadosConfVaga.
        public List<DadosConfVaga> ListagemSalas
        {
            get { return Page.Session["listagem"] as List<DadosConfVaga>; }
            set { (Page.Session["listagem"]) = value; }
        }

        #endregion

        #region Variáveis ReadOnly

        public readonly string comboTurmaNome = "ddlTurmas";
        public readonly string textBoxVCNome = "txtVC";
        public readonly string textBoxVNNome = "txtVN";
        public readonly string labelVCNome = "lblVCM";
        public readonly string labelVNNome = "lblVN";
        public static int contador = 0;

        #endregion

        #region Enums

        public enum TipoVaga
        {
            Continuidade = 1,
            Nova = 2
        }

        public enum TipoItem
        {
            Selecione = -1,
            Novo = -2
        }

        public enum ValidacaoGravacao
        {
            Validacao = 1,
            Gravacao = 2
        }

        public enum Turnos
        {
            Manha = 1,
            Tarde = 2,
            Noite = 3,
            Ampliado = 4,
            Integral = 5
        }

        #endregion

        public object Listar(object unidade_ens, object ano)
        {
            string ue = unidade_ens.ToString();
            int codPerfil = 0;

            if (!string.IsNullOrEmpty(ano.ToString()) && !string.IsNullOrEmpty(ue) && Session["perfil"].ToString() != string.Empty)
            {
                codPerfil = Convert.ToInt32(Session["codPerfil"]);
                return CtvConfTurno.Listar(ue, Convert.ToInt32(ano), codPerfil);
            }

            return null;
        }

        public void grdConfTurnos_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data
                || !this.grdConfTurnos.Visible
                || this.grdConfTurnos.VisibleRowCount == 0)
            {
                return;
            }

            if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !this.tseUnidadeResponsavel.DBValue.IsNull && this.grdConfTurnos.VisibleRowCount == 0 && e.RowType != GridViewRowType.Data)
            {
                pnlAnaliseTurnos.Visible = false;
                pnGridTurnos.Visible = false;
                pnTurnos.Visible = false;
                btnConfirmarTurnos.Visible = false;
                btnSalvarParcialTurnos.Visible = false;
                btnFinalizarTurnos.Visible = false;
                btnSalvarAnaliseTurnos.Visible = false;
                lblMensagem.Text = "Não existem séries cadastradas para o ano/periodo referência";
                return;
            }

            var perfil_resp = (string)this.grdConfTurnos.GetRowValues(e.VisibleIndex, "PerfilResponsavel");
            var finalizado = (bool)this.grdConfTurnos.GetRowValues(e.VisibleIndex, "Finalizado") == false ? "0" : "1";
            var encerrado = (bool)this.grdConfTurnos.GetRowValues(e.VisibleIndex, "Encerrado") == false ? "0" : "1";
            var chkManha = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "Manha", "chkManha");
            var chkTarde = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "Tarde", "chkTarde");
            var chkNoite = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "Noite", "chkNoite");
            var chkIntegral = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "Integral", "chkIntegral");
            var chkAmpliado = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "Ampliado", "chkAmpliado");
            var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, e.VisibleIndex, "Justificativa", "txtJustificativa");
            var hdnValorAntigo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, e.VisibleIndex, "Justificativa", "hdnValorAntigo");
            var hdnValorNovo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, e.VisibleIndex, "Justificativa", "hdnValorNovo");
            var hdnPerfilResponsavel = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, e.VisibleIndex, "Justificativa", "hdnPerfilResponsavel");
            var hdnFinalizado = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, e.VisibleIndex, "Justificativa", "hdnFinalizado");
            var hdnEncerrado = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, e.VisibleIndex, "Justificativa", "hdnEncerrado");

            var chkManhaNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "ManhaNovo", "chkManhaNovo");
            var chkTardeNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "TardeNovo", "chkTardeNovo");
            var chkNoiteNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "NoiteNovo", "chkNoiteNovo");
            var chkIntegralNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "IntegralNovo", "chkIntegralNovo");
            var chkAmpliadoNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, e.VisibleIndex, "AmpliadoNovo", "chkAmpliadoNovo");
            var txtJustificativaNovo = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, e.VisibleIndex, "JustificativaNovo", "txtJustificativaNovo");
            var hdnValorAntigoNovo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, e.VisibleIndex, "JustificativaNovo", "hdnValorAntigoNovo");
            var hdnValorNovoNovo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, e.VisibleIndex, "JustificativaNovo", "hdnValorNovoNovo");

            if (chkManha == null
                || chkTarde == null
                || chkNoite == null
                || chkIntegral == null
                || chkAmpliado == null
                || txtJustificativa == null
                || hdnValorAntigo == null
                || hdnValorNovo == null
                || chkManhaNovo == null
                || chkTardeNovo == null
                || chkNoiteNovo == null
                || chkIntegralNovo == null
                || chkAmpliadoNovo == null
                || txtJustificativaNovo == null
                || hdnValorAntigoNovo == null
                || hdnValorNovoNovo == null)
            {
                return;
            }

            if (encerrado == "1")
            {
                e.Row.Enabled = false;
                txtJustificativa.Enabled = false;
                txtJustificativaNovo.Enabled = false;
            }
            else
            {
                if (Session["perfil"].ToString() == "DIRETOR_UE")
                {
                    if (finalizado == "1")
                    {
                        e.Row.Enabled = false;
                        txtJustificativa.Enabled = false;
                        txtJustificativaNovo.Enabled = false;
                    }
                }

                if (Session["perfil"].ToString() == "SUPED")
                {
                    if (perfil_resp != "SUPED")
                    {
                        e.Row.Enabled = false;
                        txtJustificativa.Enabled = false;
                        txtJustificativaNovo.Enabled = false;
                    }
                }

                if (Session["perfil"].ToString() == "SUPLAN")
                {
                    if (perfil_resp != "SUPLAN")
                    {
                        e.Row.Enabled = false;
                        txtJustificativa.Enabled = false;
                        txtJustificativaNovo.Enabled = false;
                    }
                }
            }
            chkManha.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkTarde.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkNoite.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkIntegral.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkAmpliado.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);

            chkManha.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkTarde.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkNoite.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkIntegral.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkAmpliado.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);

            chkManha.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkTarde.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkNoite.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkIntegral.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkAmpliado.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);

            chkManha.InputAttributes.Add("valorAntigo", hdnValorAntigo.ClientID);
            chkTarde.InputAttributes.Add("valorAntigo", hdnValorAntigo.ClientID);
            chkNoite.InputAttributes.Add("valorAntigo", hdnValorAntigo.ClientID);
            chkIntegral.InputAttributes.Add("valorAntigo", hdnValorAntigo.ClientID);
            chkAmpliado.InputAttributes.Add("valorAntigo", hdnValorAntigo.ClientID);

            chkManha.InputAttributes.Add("valorNovo", hdnValorNovo.ClientID);
            chkTarde.InputAttributes.Add("valorNovo", hdnValorNovo.ClientID);
            chkNoite.InputAttributes.Add("valorNovo", hdnValorNovo.ClientID);
            chkIntegral.InputAttributes.Add("valorNovo", hdnValorNovo.ClientID);
            chkAmpliado.InputAttributes.Add("valorNovo", hdnValorNovo.ClientID);

            chkManha.InputAttributes.Add("justificativa", txtJustificativa.ClientID);
            chkTarde.InputAttributes.Add("justificativa", txtJustificativa.ClientID);
            chkNoite.InputAttributes.Add("justificativa", txtJustificativa.ClientID);
            chkIntegral.InputAttributes.Add("justificativa", txtJustificativa.ClientID);
            chkAmpliado.InputAttributes.Add("justificativa", txtJustificativa.ClientID);

            chkIntegral.InputAttributes.Add("integral", chkIntegral.ClientID);
            chkAmpliado.InputAttributes.Add("ampliado", chkAmpliado.ClientID);

            chkManhaNovo.InputAttributes.Add("valorAntigoNovo", hdnValorAntigoNovo.ClientID);
            chkTardeNovo.InputAttributes.Add("valorAntigoNovo", hdnValorAntigoNovo.ClientID);
            chkNoiteNovo.InputAttributes.Add("valorAntigoNovo", hdnValorAntigoNovo.ClientID);
            chkIntegralNovo.InputAttributes.Add("valorAntigoNovo", hdnValorAntigoNovo.ClientID);
            chkAmpliadoNovo.InputAttributes.Add("valorAntigoNovo", hdnValorAntigoNovo.ClientID);

            chkManhaNovo.InputAttributes.Add("valorNovoNovo", hdnValorNovoNovo.ClientID);
            chkTardeNovo.InputAttributes.Add("valorNovoNovo", hdnValorNovoNovo.ClientID);
            chkNoiteNovo.InputAttributes.Add("valorNovoNovo", hdnValorNovoNovo.ClientID);
            chkIntegralNovo.InputAttributes.Add("valorNovoNovo", hdnValorNovoNovo.ClientID);
            chkAmpliadoNovo.InputAttributes.Add("valorNovoNovo", hdnValorNovoNovo.ClientID);

            chkManhaNovo.InputAttributes.Add("justificativaNovo", txtJustificativaNovo.ClientID);
            chkTardeNovo.InputAttributes.Add("justificativaNovo", txtJustificativaNovo.ClientID);
            chkNoiteNovo.InputAttributes.Add("justificativaNovo", txtJustificativaNovo.ClientID);
            chkIntegralNovo.InputAttributes.Add("justificativaNovo", txtJustificativaNovo.ClientID);
            chkAmpliadoNovo.InputAttributes.Add("justificativaNovo", txtJustificativaNovo.ClientID);

            chkManhaNovo.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkTardeNovo.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkNoiteNovo.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkIntegralNovo.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);
            chkAmpliadoNovo.InputAttributes.Add("perfilResponsavel", hdnPerfilResponsavel.ClientID);

            chkManhaNovo.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkTardeNovo.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkNoiteNovo.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkIntegralNovo.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);
            chkAmpliadoNovo.InputAttributes.Add("finalizado", hdnFinalizado.ClientID);

            chkManhaNovo.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkTardeNovo.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkNoiteNovo.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkIntegralNovo.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);
            chkAmpliadoNovo.InputAttributes.Add("encerrado", hdnEncerrado.ClientID);

            txtJustificativa.ToolTip = txtJustificativa.Text;
            txtJustificativaNovo.ToolTip = txtJustificativaNovo.Text;

            updatePanel3.Update();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdConfTurnos, "Confirmação de Turnos");
            TituloGrid(this.gridVagas, "Vagas");
            TituloGrid(this.gridSalas, "Salas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                this.lblMensagemVagas.Text = string.Empty;
                this.lblMensagemVagasBottom.Text = string.Empty;
                this.lblInconsistenciaSala.Text = string.Empty;

                if (!this.Page.IsPostBack)
                {
                    Session["TurmaNovaComDados"] = null;
                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);
                    RN.Agenda.PeriodoLetivoAgenda rnPeriodoLetivoAgenda = new RN.Agenda.PeriodoLetivoAgenda();

                    if (RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name) || (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("SUPLAN") + "'").Length > 0))
                    {
                        dtPerfil.Rows.Add(string.Empty, "privilegiado", "privilegiado", 0);
                    }

                    Session["perfis"] = dtPerfil;

                    this.VerificaPerfil();

                    ddlAno.Items.Clear();
                    ddlAno.DataSource = rnPeriodoLetivoAgenda.ListaAnosPeriodoLetivoAgenda((int)RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoTurnosVagas);
                    ddlAno.DataBind();
                    ListItem ls = new ListItem("Selecione", string.Empty);
                    ddlAno.Items.Insert(0, ls);

                    //VERIFICA A VISIBILIDADE DO PAINEL DE ANALISE
                    controlaVisibilidadeFiltroAnalise();

                    if (!CtvAgendaConfTurnoVaga.ConsultarPeriodoAtivoTurnos())
                    {
                        var dtAnt = CtvAgendaConfTurnoVaga.ConsultarPeriodoAnteriorTurnos();

                        if (dtAnt.Rows.Count > 0)
                        {
                            this.lblMensagem.Text =
                                string.Format("O período de confirmação de turnos iniciará em {0} e se encerrará em {1}. ",
                                              Convert.ToDateTime(dtAnt.Rows[0]["DT_INICIO_CONF_TURNO"])
                                                  .ToShortDateString(),
                                              Convert.ToDateTime(dtAnt.Rows[0]["DT_FIM_CONF_TURNO"])
                                                  .ToShortDateString());
                            return;
                        }

                        var dtPost = CtvAgendaConfTurnoVaga.ConsultarPeriodoPosteriorTurnos();

                        if (dtPost.Rows.Count > 0)
                        {
                            if (dtPost.Rows[0]["DT_FIM_CONF_TURNO"] != DBNull.Value)
                            {
                                this.lblMensagem.Text =
                                    string.Format("O prazo para confirmação de turnos está encerrado desde {0}. ",
                                                  Convert.ToDateTime(dtPost.Rows[0]["DT_FIM_CONF_TURNO"])
                                                      .ToShortDateString());
                                return;
                            }
                        }
                    }

                    this.pnGridVagas.Visible = false;
                    this.pnTurmas.Visible = false;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                        preencheDadosQueryString(decodedText);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void preencheDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("Ano=") >= 0)
                {
                    ddlAno.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                    ddlAno_SelectedIndexChanged(null, null);
                }

                if (pnlAnalise.Visible)
                {
                    if (dados.IndexOf("Perfil=") >= 0)
                    {
                        ddlPerfil.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        ddlPerfil_SelectedIndexChanged(null, null);
                    }
                    else if (dados.IndexOf("TurnosAnalisados=") >= 0)
                    {
                        rblTurnosAnalisados.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        rblTurnosAnalisados_SelectedIndexChanged(null, null);

                    }
                    else if (dados.IndexOf("ResultadoTurnos=") >= 0)
                    {
                        if (rblTurnosAnalisados.SelectedValue == "Sim")
                        {
                            ddlResultadoTurnos.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                            ddlResultadoTurnos_SelectedIndexChanged(null, null);
                        }
                    }
                    else if (dados.IndexOf("VagasAnalisadas=") >= 0)
                    {
                        rblVagasAnalisadas.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        rblVagasAnalisadas_SelectedIndexChanged(null, null);

                    }
                    else if (dados.IndexOf("ResultadoVagas=") >= 0)
                    {
                        if (rblVagasAnalisadas.SelectedValue == "Sim")
                        {
                            ddlResultadoVagas.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                            ddlResultadoVagas_SelectedIndexChanged(null, null);
                        }
                    }
                    else if (dados.IndexOf("FaixaInicial=") >= 0)
                    {
                        txtFaixaInicial.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                        txtFaixaInicial_TextChanged(null, null);
                    }
                    else if (dados.IndexOf("FaixaFinal=") >= 0)
                    {
                        txtFaixaFinal.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                        txtFaixaFinal_TextChanged(null, null);
                    }
                    else if (dados.IndexOf("TipoVariacao=") >= 0)
                    {
                        ddlFaixaVariacao.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        ddlFaixaVariacao_SelectedIndexChanged(null, null);
                    }
                    else if (dados.IndexOf("Modalidade=") >= 0)
                    {
                        ddlModSegCurso.DataBind();
                        ddlModSegCurso.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                        ddlModSegCurso_SelectedIndexChanged(null, null);
                    }
                    else if (dados.IndexOf("Serie=") >= 0)
                    {
                        ddlSerie.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        ddlSerie_SelectedIndexChanged(null, null);
                    }
                    else if (dados.IndexOf("Turno=") >= 0)
                    {
                        ddlTurno.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        ddlTurno_SelectedIndexChanged(null, null);
                    }
                }
                if (dados.IndexOf("UnidadeEnsino=") >= 0)
                {
                    tseUnidadeResponsavel.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                    //tseUnidadeResponsavel_Changed(null, null);
                    Session["carregaUnidadeTurmaProvisora"] = "1";
                }
            }
        }

        protected void VerificaPerfil()
        {
            try
            {
                var dt = (DataTable)Session["perfis"];

                if (dt.Select("perfil ='" + RN.RNBase.MudarAspas("privilegiado") + "'").Length > 0
                    || (dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPLAN") + "'").Length > 0))
                {
                    Session["perfil"] = "privilegiado";
                    Session["codPerfil"] = "0";
                }
                else if (dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPED") + "'").Length > 0)
                {
                    Session["perfil"] = "SUPED";
                    Session["codPerfil"] = Convert.ToString(dt.Select("perfil ='" + RN.RNBase.MudarAspas("SUPED") + "'")[0].ItemArray[3]);
                }
                else if (dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIESP") + "'").Length > 0)
                {
                    Session["perfil"] = "DIESP";
                    Session["codPerfil"] = Convert.ToString(dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIESP") + "'")[0].ItemArray[3]);
                }
                else if (dt.Select("perfil ='" + RN.RNBase.MudarAspas("REGIONAL") + "'").Length > 0)
                {
                    Session["perfil"] = "REGIONAL";
                    Session["codPerfil"] = Convert.ToString(dt.Select("perfil ='" + RN.RNBase.MudarAspas("REGIONAL") + "'")[0].ItemArray[3]);
                }
                else if (dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIRETOR_UE") + "'").Length > 0)
                {
                    Session["perfil"] = "DIRETOR_UE";
                    Session["codPerfil"] = Convert.ToString(dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIRETOR_UE") + "'")[0].ItemArray[3]);
                }
                else
                {
                    Session["perfil"] = string.Empty;
                    Session["codPerfil"] = "-1";
                    lblMensagem.Text = "Não é permitido alterar turnos para seu perfil de usuário";
                }

                hdnPerfil.Value = Session["perfil"].ToString();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void HabilitaLinkHistorico()
        {
            RN.TurnosVagas.HistoricoTurnoVaga rnHistoricoTurnoVaga = new Techne.Lyceum.RN.TurnosVagas.HistoricoTurnoVaga();
            int tipoHistoricoDiretor = (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Diretor;

            try
            {
                divLinkHistorico.Visible = false;

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    //Verifica se existe historico para aquele ano / escola
                    if (rnHistoricoTurnoVaga.PossuiHistoricoTurnoPor(tipoHistoricoDiretor, Convert.ToInt32(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString()))
                    {
                        divLinkHistorico.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                this.ControlaAcesso(this.grdConfTurnos);
                //var dt = (DataTable)Session["perfis"];
                btnSalvarParcialTurnos.Visible = false;
                btnFinalizarTurnos.Visible = false;
                btnSalvarAnaliseTurnos.Visible = false;
                btnConfirmarTurnos.Visible = false;
                lblMensagemFinalizarTurno.Text = string.Empty;
                lblMensagemFinalizarVagas.Text = string.Empty;
                CtvFinalizado nrCtvFinalizado = new CtvFinalizado();
                RN.Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new Techne.Lyceum.RN.Agenda.ParametroTurnoVaga();
                RN.Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new RN.Agenda.Entidades.ParametroTurnoVaga();
                int codPerfil = 0;
                int idAgenda = 0;
                bool encerrado;

                //Verifica se é para remontar tsearch de escola
                if (Session["carregaUnidadeTurmaProvisora"] != null && Session["carregaUnidadeTurmaProvisora"].ToString() == "1")
                {
                    tseUnidadeResponsavel_Changed(null, null);
                    Session["carregaUnidadeTurmaProvisora"] = "0";
                }

                if (!this.tseUnidadeResponsavel.DBValue.IsNull && !string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    string dataTurno = nrCtvFinalizado.RetornaDadosFinalizacao(int.Parse(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString(), true, false);

                    if (!string.IsNullOrEmpty(dataTurno))
                    {
                        lblMensagemFinalizarTurno.Text = string.Format(@"A confirmação de turnos foi finalizada pela unidade escolar em:<br />{0}",
                                dataTurno.Replace(Environment.NewLine, "<br />"));
                    }

                    string dataVagas = nrCtvFinalizado.RetornaDadosFinalizacao(int.Parse(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString(), false, true);

                    if (!string.IsNullOrEmpty(dataVagas))
                    {
                        lblMensagemFinalizarVagas.Text = string.Format(@"A confirmação de vagas foi finalizada pela unidade escolar em:<br />{0}",
                                dataVagas.Replace(Environment.NewLine, "<br />"));
                    }

                    int Finalizado = 0;
                    int Encerrado = 0;
                    var perfil = Session["perfil"].ToString();

                    for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
                    {
                        var Ehfinalizado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Finalizado");
                        var EhEncerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado");

                        if (Ehfinalizado)
                        {
                            Finalizado = Finalizado + 1;
                        }

                        if (EhEncerrado)
                        {
                            Encerrado = Encerrado + 1;
                        }
                    }

                    if (this.grdConfTurnos.VisibleRowCount != Finalizado)
                    {
                        btnFinalizarTurnos.Visible = true;
                        btnConfirmarTurnos.Visible = true;

                        if (string.IsNullOrEmpty(hdnPodeAnalisarTurno.Value))
                        {
                            btnSalvarParcialTurnos.Visible = true;
                            btnSalvarAnaliseTurnos.Visible = false;
                        }
                        else
                        {
                            btnSalvarParcialTurnos.Visible = false;
                            btnSalvarAnaliseTurnos.Visible = true;
                        }
                    }
                    else
                    {
                        codPerfil = Convert.ToInt32(Session["codPerfil"]);

                        //Verifica se é privilegiado
                        if (codPerfil == 0)
                        {
                            if (string.IsNullOrEmpty(hdnPodeAnalisarTurno.Value))
                            {
                                btnSalvarParcialTurnos.Visible = true;
                                btnConfirmarTurnos.Visible = true;
                                btnSalvarAnaliseTurnos.Visible = false;
                            }
                            else
                            {
                                btnSalvarParcialTurnos.Visible = false;
                                btnConfirmarTurnos.Visible = false;
                                btnSalvarAnaliseTurnos.Visible = true;
                            }
                        }
                        else
                        {
                            for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
                            {
                                encerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado");
                                if (!encerrado)
                                {
                                    idAgenda = Convert.ToInt32(this.grdConfTurnos.GetRowValues(rowIndex, "AgendaId").ToString());
                                    break;
                                }
                            }

                            parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, idAgenda);
                            if (parametroTurnoVaga.ParametroTurnoVagaId > 0)
                            {
                                if (parametroTurnoVaga.EditarTurnoFinalizado)
                                {
                                    if (string.IsNullOrEmpty(hdnPodeAnalisarTurno.Value))
                                    {
                                        btnSalvarParcialTurnos.Visible = true;
                                        btnConfirmarTurnos.Visible = true;
                                        btnSalvarAnaliseTurnos.Visible = false;
                                    }
                                    else
                                    {
                                        btnSalvarParcialTurnos.Visible = false;
                                        btnConfirmarTurnos.Visible = false;
                                        btnSalvarAnaliseTurnos.Visible = true;
                                    }
                                }
                            }
                        }
                    }

                    if (this.grdConfTurnos.VisibleRowCount == Encerrado)
                    {
                        btnSalvarParcialTurnos.Visible = false;
                        btnFinalizarTurnos.Visible = false;
                        btnConfirmarTurnos.Visible = true;
                        if (string.IsNullOrEmpty(hdnPodeAnalisarTurno.Value))
                        {
                            btnSalvarAnaliseTurnos.Visible = false;
                        }
                        else
                        {
                            btnSalvarAnaliseTurnos.Visible = true;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    if (CtvAgendaConfTurnoVaga.PossuiApenasAgendasEncerradas(int.Parse(ddlAno.SelectedValue)))
                    {
                        btnSalvarParcialTurnos.Visible = false;
                        btnFinalizarTurnos.Visible = false;
                        btnConfirmarTurnos.Visible = false;
                        btnSalvarAnaliseTurnos.Visible = false;
                        pnTurmas.Visible = true;
                        btnSalvarParcialVagas.Visible = false;
                        btnSalvarDefinitivoVagas.Visible = false;
                        btnExcluirTurmasProvisorias.Visible = false;
                        btnSalvarAnaliseVagas.Visible = false;
                    }
                }
                //TRATAMENTO DE CORES DAS CELULAS DE SALAS 
                string[] turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();
                for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
                {
                    for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                    {
                        var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna]);
                        var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                        var nome = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna]);
                        var currentTextBoxVC = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;

                        nome = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna]);
                        var currentTextBoxVN = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;
                        if (currentDropDown.Enabled)
                        {
                            currentDropDown.BackColor = Color.FromName("#FFFF80");
                            currentTextBoxVC.BackColor = Color.FromName("#FFFF80");
                            currentTextBoxVN.BackColor = Color.FromName("#FFFF80");
                        }
                        else
                        {
                            currentDropDown.BackColor = Color.FromName("Gainsboro");
                            currentTextBoxVC.BackColor = Color.FromName("Gainsboro");
                            currentTextBoxVN.BackColor = Color.FromName("Gainsboro");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web
                         .Navigation
                         .GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod())
                         .GetUrl(new object[] { });
            #endregion
        }

        #region Filtros

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                Turmas = null;
                Controle = null;
                Valores = null;
                Session["CursosNaoParticipamVagas"] = null;
                divLinkHistorico.Visible = false;

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

                            Confirmacao = null;

                            gridSalas.DataSource = null;
                            gridSalas.DataBind();
                            gridVagas.DataSource = null;
                            gridVagas.DataBind();
                            lblAnaliseVagasGeralSUPED.Text = string.Empty;
                            lblAnaliseVagasGeralSUPLAN.Text = string.Empty;
                            lblAnaliseVagasGeralDIESP.Text = string.Empty;
                            lblAnaliseTurnosGeralSUPED.Text = string.Empty;
                            lblAnaliseTurnosGeralSUPLAN.Text = string.Empty;
                            lblAnaliseTurnosGeralDIESP.Text = string.Empty;
                            pnGridVagas.Visible = false;
                            pnTurnos.Visible = false;
                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            grdConfTurnos.Visible = false;
                            btnSalvarParcialTurnos.Visible = false;
                            btnFinalizarTurnos.Visible = false;
                            btnConfirmarTurnos.Visible = false;
                            btnSalvarAnaliseTurnos.Visible = false;
                            pnGridTurnos.Visible = false;
                            pnTurnos.Visible = false;
                            pnTurmas.Visible = false;
                            pnlAnaliseTurnos.Visible = false;
                            pnlAnaliseVagas.Visible = false;
                            lblMensagemFinalizarTurno.Text = string.Empty;
                            lblMensagemFinalizarVagas.Text = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();
                        grdConfTurnos.Visible = false;
                        btnSalvarParcialTurnos.Visible = false;
                        btnSalvarAnaliseTurnos.Visible = false;
                        btnFinalizarTurnos.Visible = false;
                        btnConfirmarTurnos.Visible = false;
                        pnGridTurnos.Visible = false;
                        pnTurnos.Visible = false;
                        pnGridVagas.Visible = false;
                        pnTurmas.Visible = false;
                        pnlAnaliseTurnos.Visible = false;
                        pnlAnaliseVagas.Visible = false;
                        lblMensagemFinalizarTurno.Text = string.Empty;
                        lblMensagemFinalizarVagas.Text = string.Empty;
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
                Turmas = null;
                Controle = null;
                Valores = null;
                Session["CursosNaoParticipamVagas"] = null;
                divLinkHistorico.Visible = false;

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
                            Confirmacao = null;

                            gridSalas.DataSource = null;
                            gridSalas.DataBind();
                            gridVagas.DataSource = null;
                            gridVagas.DataBind();
                            lblAnaliseVagasGeralSUPED.Text = string.Empty;
                            lblAnaliseVagasGeralSUPLAN.Text = string.Empty;
                            lblAnaliseVagasGeralDIESP.Text = string.Empty;
                            lblAnaliseTurnosGeralSUPED.Text = string.Empty;
                            lblAnaliseTurnosGeralSUPLAN.Text = string.Empty;
                            lblAnaliseTurnosGeralDIESP.Text = string.Empty;
                            pnGridVagas.Visible = false;
                            pnTurnos.Visible = false;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            grdConfTurnos.Visible = false;
                            btnSalvarParcialTurnos.Visible = false;
                            btnSalvarAnaliseTurnos.Visible = false;
                            btnFinalizarTurnos.Visible = false;
                            btnConfirmarTurnos.Visible = false;
                            pnGridTurnos.Visible = false;
                            pnTurnos.Visible = false;
                            pnGridVagas.Visible = false;
                            pnTurmas.Visible = false;
                            pnlAnaliseTurnos.Visible = false;
                            pnlAnaliseVagas.Visible = false;
                            lblMensagemFinalizarTurno.Text = string.Empty;
                            lblMensagemFinalizarVagas.Text = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        grdConfTurnos.Visible = false;
                        btnSalvarParcialTurnos.Visible = false;
                        btnSalvarAnaliseTurnos.Visible = false;
                        btnFinalizarTurnos.Visible = false;
                        btnConfirmarTurnos.Visible = false;
                        pnGridTurnos.Visible = false;
                        pnTurnos.Visible = false;
                        pnGridVagas.Visible = false;
                        pnTurmas.Visible = false;
                        pnlAnaliseTurnos.Visible = false;
                        pnlAnaliseVagas.Visible = false;
                        lblMensagemFinalizarTurno.Text = string.Empty;
                        lblMensagemFinalizarVagas.Text = string.Empty;
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
                DataTable dtTurma = new DataTable();
                RN.CtvConfVaga rnCtvConfVaga = new CtvConfVaga();
                Turmas = null;
                Controle = null;
                Valores = null;
                var dt = (DataTable)Session["perfis"];
                Session["TurmaNovaComDados"] = null;
                Session["CursosNaoParticipamVagas"] = null;
                Session["podeTurmaProvisoria"] = null;
                divLinkHistorico.Visible = false;
               
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

                            if (Session["perfil"].ToString() != "DIRETOR_UE")
                            {
                                if (tseRegional.Value.ToString() == "5"
                                    && ((dt.Select("perfil ='" + RN.RNBase.MudarAspas("DIESP") + "'").Length == 0)
                                    && dt.Select("perfil ='" + RN.RNBase.MudarAspas("privilegiado") + "'").Length == 0))
                                {
                                    lblMensagem.Text = "Seu perfil não permite acesso a escolas da Regional Especial de Unidades Socioeducativas e Prisionais.";
                                    pnGridTurnos.Visible = false;
                                    pnTurnos.Visible = false;
                                    pnlAnaliseTurnos.Visible = false;
                                    pnlAnaliseVagas.Visible = false;
                                    pnGridVagas.Visible = false;
                                    return;
                                }
                            }
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }

                        if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                        {
                            grdConfTurnos.Visible = true;
                            lblMensagem.Text = string.Empty;
                            lblMensagemFinalizarTurno.Text = string.Empty;
                            lblMensagemFinalizarVagas.Text = string.Empty;
                            lblMensagemVagas.Text = string.Empty;
                            lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
                            pnGridTurnos.Visible = true;
                            pnTurnos.Visible = true;
                            pnlAnaliseTurnos.Visible = true;
                            pnlAnaliseVagas.Visible = true;
                            pnGridVagas.Visible = true;

                            this.PreencheDadosAnaliseTurnos(Convert.ToInt32(ddlAno.SelectedValue),
                                                 tseUnidadeResponsavel.DBValue.ToString());

                            this.VerificaPerfil();

                            this.HabilitaLinkHistorico();

                            VerificaInconsistenciaSalaVaga(Convert.ToInt32(ddlAno.SelectedValue),
                                                 tseUnidadeResponsavel.DBValue.ToString());
                            
                            gridSalas.DataSource = null;
                            gridSalas.DataBind();
                            this.InicializaVagas();
                        }
                        else
                        {
                            grdConfTurnos.Visible = false;
                            lblMensagem.Text = "Favor selecionar Ano.";
                            lblMensagemFinalizarTurno.Text = string.Empty;
                            lblMensagemFinalizarVagas.Text = string.Empty;
                            pnGridTurnos.Visible = false;
                            pnTurnos.Visible = false;
                            pnGridVagas.Visible = false;
                            pnTurmas.Visible = false;
                            pnlAnaliseTurnos.Visible = false;
                            pnlAnaliseVagas.Visible = false;
                            btnConfirmarTurnos.Visible = false;
                            btnSalvarParcialTurnos.Visible = false;
                            btnSalvarAnaliseTurnos.Visible = false;
                            btnFinalizarTurnos.Visible = false;
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

                        grdConfTurnos.Visible = false;
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                        pnGridTurnos.Visible = false;
                        lblMensagemFinalizarTurno.Text = string.Empty;
                        lblMensagemFinalizarVagas.Text = string.Empty;
                        pnTurnos.Visible = false;
                        pnGridVagas.Visible = false;
                        pnTurmas.Visible = false;
                        pnlAnaliseTurnos.Visible = false;
                        pnlAnaliseVagas.Visible = false;
                        btnConfirmarTurnos.Visible = false;
                        btnSalvarParcialTurnos.Visible = false;
                        btnSalvarAnaliseTurnos.Visible = false;
                        btnFinalizarTurnos.Visible = false;
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

                    grdConfTurnos.Visible = false;
                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                    lblMensagemFinalizarTurno.Text = string.Empty;
                    lblMensagemFinalizarVagas.Text = string.Empty;
                    pnGridTurnos.Visible = false;
                    pnTurnos.Visible = false;
                    pnGridVagas.Visible = false;
                    pnTurmas.Visible = false;
                    pnlAnaliseTurnos.Visible = false;
                    pnlAnaliseVagas.Visible = false;
                    btnConfirmarTurnos.Visible = false;
                    btnSalvarParcialTurnos.Visible = false;
                    btnSalvarAnaliseTurnos.Visible = false;
                    btnFinalizarTurnos.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
                grdConfTurnos.Visible = false;
                pnGridTurnos.Visible = false;
                pnTurnos.Visible = false;
                pnGridVagas.Visible = false;
                pnTurmas.Visible = false;
                pnlAnaliseTurnos.Visible = false;
                pnlAnaliseVagas.Visible = false;
                btnConfirmarTurnos.Visible = false;
                btnSalvarParcialTurnos.Visible = false;
                btnSalvarAnaliseTurnos.Visible = false;
                btnFinalizarTurnos.Visible = false;
            }

            updatePanel2.Update();
            updatePanel3.Update();

        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Turmas = null;
                Controle = null;
                Valores = null;
                Session["CursosNaoParticipamVagas"] = null;

                limpaCamposFiltroAnalise();
                grdConfTurnos.Visible = false;
                pnGridTurnos.Visible = false;
                pnTurnos.Visible = false;
                pnGridVagas.Visible = false;
                pnTurmas.Visible = false;
                pnlAnaliseTurnos.Visible = false;
                pnlAnaliseVagas.Visible = false;
                btnConfirmarTurnos.Visible = false;
                btnSalvarParcialTurnos.Visible = false;
                btnSalvarAnaliseTurnos.Visible = false;
                btnFinalizarTurnos.Visible = false;
                tseMunicipio.ResetValue();
                tseRegional.ResetValue();
                lblMensagemFinalizarTurno.Text = string.Empty;
                lblMensagemFinalizarVagas.Text = string.Empty;
                hdnPodeAnalisarTurno.Value = string.Empty;
                tseUnidadeResponsavel.ResetValue();
                divLinkHistorico.Visible = false;

                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        private void controlaVisibilidadeFiltroAnalise()
        {
            bool visivelAnaliseTurno = false;
            bool visivelAnaliseVaga = false;
            bool privilegiado = false;
            RN.CtvAnalise rnCtvAnalise = new CtvAnalise();
            RN.Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new Techne.Lyceum.RN.Agenda.ParametroTurnoVaga();

            //Verifica se o usuario logado é privilegiado
            if (Session["perfil"].ToString() == "privilegiado")
            {
                privilegiado = true;
            }

            visivelAnaliseTurno = rnCtvAnalise.EhModoAnaliseTurnoPor(User.Identity.Name, privilegiado);
            visivelAnaliseVaga = rnCtvAnalise.EhModoAnaliseVagaPor(User.Identity.Name, privilegiado);

            if (visivelAnaliseTurno || visivelAnaliseVaga)
            {
                pnlAnalise.Visible = (visivelAnaliseTurno || visivelAnaliseVaga);
                ddlPerfil.Items.Clear();
                ddlPerfil.DataSource = rnParametroTurnoVaga.ListaPerfilPodeAnalisarTurnosVagasPor(User.Identity.Name, privilegiado);
                ddlPerfil.DataBind();

                pnlFaixaVariacao.Visible = visivelAnaliseVaga;
            }
        }

        protected void ddlPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                limpaEscolaFiltroAnalise();
                limpaCamposFiltroAnalise();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void ddlModSegCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerie.Items.Clear();
                ddlTurno.Items.Clear();
                ddlSerie.Enabled = false;
                ddlTurno.Enabled = false;
                RN.Serie rnSerie = new Serie();
                limpaEscolaFiltroAnalise();

                if ((ddlModSegCurso.Value != null && ddlModSegCurso.Value.ToString() != "Curso") && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPerfil.SelectedValue))
                {
                    ddlSerie.DataSource = rnSerie.listaSerieAgendaTurnoVagaPor(int.Parse(ddlAno.SelectedValue), int.Parse(ddlPerfil.SelectedValue), ddlModSegCurso.Value.ToString());
                    ddlSerie.DataBind();
                    ListItem ls = new ListItem("Todas", string.Empty);
                    ddlSerie.Items.Insert(0, ls);
                    ddlSerie.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTurno.Items.Clear();
                ddlTurno.Enabled = false;
                RN.Turno rnTurno = new Turno();
                limpaEscolaFiltroAnalise();

                if (!string.IsNullOrEmpty(ddlSerie.SelectedValue) && (ddlModSegCurso.Value != null && ddlModSegCurso.Value.ToString() != "Curso") && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPerfil.SelectedValue))
                {
                    ddlTurno.DataSource = rnTurno.listaTurnoAgendaTurnoVagaPor(int.Parse(ddlAno.SelectedValue), int.Parse(ddlPerfil.SelectedValue), ddlModSegCurso.Value.ToString(), int.Parse(ddlSerie.SelectedValue));
                    ddlTurno.DataBind();
                    ListItem ls = new ListItem("Todos", string.Empty);
                    ddlTurno.Items.Insert(0, ls);
                    ddlTurno.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpaEscolaFiltroAnalise();
        }

        protected void ddlResultadoTurnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpaEscolaFiltroAnalise();
        }

        protected void ddlResultadoVagas_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpaEscolaFiltroAnalise();
        }

        protected void ddlFaixaVariacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpaEscolaFiltroAnalise();
        }

        protected void rblTurnosAnalisados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlResultadoTurnos.Items.Clear();
                ddlResultadoTurnos.Visible = false;
                lblFiltroResultadoTurnos.Visible = false;
                limpaEscolaFiltroAnalise();

                if (rblTurnosAnalisados.SelectedValue == "Sim")
                {
                    RN.TurnosVagas.ResultadoAnalise rnResultadoAnalise = new RN.TurnosVagas.ResultadoAnalise();

                    ddlResultadoTurnos.Visible = true;
                    lblFiltroResultadoTurnos.Visible = true;
                    ddlResultadoTurnos.DataSource = rnResultadoAnalise.ListaResultadosAnaliseAtivos();
                    ddlResultadoTurnos.DataBind();
                    ListItem ls = new ListItem("Todos", "Todos");
                    ddlResultadoTurnos.Items.Insert(0, ls);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void rblVagasAnalisadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlResultadoVagas.Items.Clear();
                ddlResultadoVagas.Visible = false;
                lblFiltroResultadoVagas.Visible = false;
                limpaEscolaFiltroAnalise();

                if (rblVagasAnalisadas.SelectedValue == "Sim")
                {
                    RN.TurnosVagas.ResultadoAnalise rnResultadoAnalise = new RN.TurnosVagas.ResultadoAnalise();

                    ddlResultadoVagas.Visible = true;
                    lblFiltroResultadoVagas.Visible = true;
                    ddlResultadoVagas.DataSource = rnResultadoAnalise.ListaResultadosAnaliseAtivos();
                    ddlResultadoVagas.DataBind();
                    ListItem ls = new ListItem("Todos", "Todos");
                    ddlResultadoVagas.Items.Insert(0, ls);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void txtFaixaInicial_TextChanged(object sender, EventArgs e)
        {
            limpaEscolaFiltroAnalise();
        }

        protected void txtFaixaFinal_TextChanged(object sender, EventArgs e)
        {
            limpaEscolaFiltroAnalise();
        }

        private void limpaEscolaFiltroAnalise()
        {
            Turmas = null;
            Controle = null;
            Valores = null;
            Session["CursosNaoParticipamVagas"] = null;

            divLinkHistorico.Visible = false;
            grdConfTurnos.Visible = false;
            pnGridTurnos.Visible = false;
            pnTurnos.Visible = false;
            pnGridVagas.Visible = false;
            pnTurmas.Visible = false;
            pnlAnaliseTurnos.Visible = false;
            pnlAnaliseVagas.Visible = false;
            btnConfirmarTurnos.Visible = false;
            btnSalvarParcialTurnos.Visible = false;
            btnFinalizarTurnos.Visible = false;
            btnSalvarAnaliseTurnos.Visible = false;
            tseMunicipio.ResetValue();
            tseRegional.ResetValue();
            lblMensagemFinalizarTurno.Text = string.Empty;
            lblMensagemFinalizarVagas.Text = string.Empty;
            hdnPodeAnalisarTurno.Value = string.Empty;
            tseUnidadeResponsavel.ResetValue();
        }

        #endregion

        #region Eventos Botao Turno

        private void GerarPopupTurnosOfertados()
        {
            int AgendaId = 0;
            int AgendaVinculadaId = 0;
            DataTable cursosMatriculaFacil = new DataTable();
            RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
            bool matFacil = false;
            List<DadosConfTurno> lista = new List<DadosConfTurno>();
            string periodo = string.Empty;
            string id = string.Empty;
            string serie = string.Empty;
            string descricaoSerie = string.Empty;
            string modalidade = string.Empty;
            string codModalidade = string.Empty;
            string codTipo = string.Empty;
            string curso = string.Empty;
            string nomeCurso = string.Empty;
            CheckBox chkManha;
            CheckBox chkTarde;
            CheckBox chkNoite;
            CheckBox chkIntegral;
            CheckBox chkAmpliado;
            TextBox txtJustificativa;
            HiddenField hdnValorAntigo;
            string encerrado = string.Empty;
            CheckBox chkManhaNovo;
            CheckBox chkTardeNovo;
            CheckBox chkNoiteNovo;
            CheckBox chkIntegralNovo;
            CheckBox chkAmpliadoNovo;
            TextBox txtJustificativaNovo;
            DadosConfTurno confTurnos;
            HtmlTableRow trTurnos;
            HtmlTableCell tdPeriodo;
            HtmlTableCell tdCurso;
            HtmlTableCell tdManhaMF;
            HtmlTableCell tdTardeMF;
            HtmlTableCell tdNoiteMF;
            HtmlTableCell tdAmpliadoMF;
            HtmlTableCell tdIntegralMF;
            HtmlTableCell tdManhaAbsorcao;
            HtmlTableCell tdTardeAbsorcao;
            HtmlTableCell tdNoiteAbsorcao;
            HtmlTableCell tdAmpliadoAbsorcao;
            HtmlTableCell tdIntegralAbsorcao;

            AgendaId = ObtemAgendaId();
            AgendaVinculadaId = rnAgenda.ObtemAgendaVinculadaPor(AgendaId, Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.MatriculaFacil));

            //Verfica se eh Curso / series do matricula facil
            cursosMatriculaFacil = rnAgenda.ListaCursosMatriculaFacilPor(AgendaVinculadaId);

            for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
            {
                matFacil = false;
                periodo = this.grdConfTurnos.GetRowValues(rowIndex, "Periodo").ToString();
                id = this.grdConfTurnos.GetRowValues(rowIndex, "IdAgendaConfTurnoVaga").ToString();
                serie = this.grdConfTurnos.GetRowValues(rowIndex, "Serie").ToString();
                descricaoSerie = this.grdConfTurnos.GetRowValues(rowIndex, "DescricaoSerie").ToString();
                modalidade = this.grdConfTurnos.GetRowValues(rowIndex, "Modalidade").ToString();
                codModalidade = this.grdConfTurnos.GetRowValues(rowIndex, "CodigoModalidade").ToString();
                codTipo = this.grdConfTurnos.GetRowValues(rowIndex, "CodigoTipo").ToString();
                curso = this.grdConfTurnos.GetRowValues(rowIndex, "Curso").ToString();
                nomeCurso = this.grdConfTurnos.GetRowValues(rowIndex, "NomeCurso").ToString();
                chkManha = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Manha", "chkManha");
                chkTarde = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Tarde", "chkTarde");
                chkNoite = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Noite", "chkNoite");
                chkIntegral = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Integral", "chkIntegral");
                chkAmpliado = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Ampliado", "chkAmpliado");
                txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "Justificativa", "txtJustificativa");
                hdnValorAntigo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, rowIndex, "Justificativa", "hdnValorAntigo");
                encerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado") == false ? "0" : "1";

                chkManhaNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "ManhaNovo", "chkManhaNovo");
                chkTardeNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "TardeNovo", "chkTardeNovo");
                chkNoiteNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "NoiteNovo", "chkNoiteNovo");
                chkIntegralNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "IntegralNovo", "chkIntegralNovo");
                chkAmpliadoNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "AmpliadoNovo", "chkAmpliadoNovo");
                txtJustificativaNovo = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "JustificativaNovo", "txtJustificativaNovo");

                if (encerrado == "0")
                {
                    confTurnos = new DadosConfTurno
                    {
                        Periodo = Convert.ToInt32(periodo),
                        IdAgendaConfTurnoVaga = int.Parse(id),
                        CodigoModalidade = codModalidade,
                        CodigoTipo = codTipo,
                        Censo = this.tseUnidadeResponsavel.DBValue.ToString(),
                        Manha = chkManha.Checked,
                        ManhaCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "ManhaCodigo")),
                        Tarde = chkTarde.Checked,
                        TardeCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TardeCodigo")),
                        Noite = chkNoite.Checked,
                        NoiteCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "NoiteCodigo")),
                        Integral = chkIntegral.Checked,
                        IntegralCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "IntegralCodigo")),
                        Ampliado = chkAmpliado.Checked,
                        AmpliadoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "AmpliadoCodigo")),
                        TurnosIniciais = hdnValorAntigo.Value,
                        Justificativa = txtJustificativa.Text.Trim(),
                        ManhaNovo = chkManhaNovo.Checked,
                        ManhaNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "ManhaNovoCodigo")),
                        TardeNovo = chkTardeNovo.Checked,
                        TardeNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TardeNovoCodigo")),
                        NoiteNovo = chkNoiteNovo.Checked,
                        NoiteNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "NoiteNovoCodigo")),
                        IntegralNovo = chkIntegralNovo.Checked,
                        IntegralNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "IntegralNovoCodigo")),
                        AmpliadoNovo = chkAmpliadoNovo.Checked,
                        AmpliadoNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "AmpliadoNovoCodigo")),
                        JustificativaNovo = txtJustificativaNovo.Text.Trim(),
                        PerfilResponsavel = Session["perfil"].ToString(),
                        Serie = Convert.ToInt32(serie),
                        NomeCurso = nomeCurso,
                        Curso = curso,
                        Modalidade = modalidade,
                        DescricaoSerie = descricaoSerie,
                        Matricula = this.User.Identity.Name,
                        TurnosListaInicial = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TurnosListaInicial"))
                    };

                    if (cursosMatriculaFacil != null)
                    {
                        foreach (DataRow item in cursosMatriculaFacil.Rows)
                        {
                            if (Convert.ToString(item["CURSO"]) == curso && Convert.ToString(item["SERIE"]) == serie)
                            {
                                matFacil = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        matFacil = false;
                    }

                    trTurnos = new HtmlTableRow();
                    tdPeriodo = new HtmlTableCell();
                    tdCurso = new HtmlTableCell();
                    tdManhaMF = new HtmlTableCell();
                    tdTardeMF = new HtmlTableCell();
                    tdNoiteMF = new HtmlTableCell();
                    tdAmpliadoMF = new HtmlTableCell();
                    tdIntegralMF = new HtmlTableCell();
                    tdManhaAbsorcao = new HtmlTableCell();
                    tdTardeAbsorcao = new HtmlTableCell();
                    tdNoiteAbsorcao = new HtmlTableCell();
                    tdAmpliadoAbsorcao = new HtmlTableCell();
                    tdIntegralAbsorcao = new HtmlTableCell();

                    tdPeriodo.Align = "center";
                    tdPeriodo.InnerText = periodo;

                    tdCurso.InnerText = modalidade + "/ " + nomeCurso + "/ " + serie.ToString();
                    tdCurso.Align = "center";

                    if (chkManha.Checked || (chkManhaNovo.Checked && !matFacil) || (chkManhaNovo.Checked && (Session["perfil"].ToString() == "DIESP" || (Convert.ToString(tseRegional.DBValue) == "5"))))
                    {
                        tdManhaAbsorcao.InnerText = "X";
                        tdManhaAbsorcao.Align = "center";
                    }
                    if (chkManhaNovo.Checked && matFacil && (Session["perfil"].ToString() != "DIESP" && (Convert.ToString(tseRegional.DBValue) != "5")))
                    {
                        tdManhaMF.InnerText = "X";
                        tdManhaMF.Align = "center";
                    }

                    if (chkTarde.Checked || (chkTardeNovo.Checked && !matFacil) || (chkTardeNovo.Checked && (Session["perfil"].ToString() == "DIESP" || (Convert.ToString(tseRegional.DBValue) == "5"))))
                    {

                        tdTardeAbsorcao.InnerText = "X";
                        tdTardeAbsorcao.Align = "center";
                    }
                    if (chkTardeNovo.Checked && matFacil && (Session["perfil"].ToString() != "DIESP" && (Convert.ToString(tseRegional.DBValue) != "5")))
                    {
                        tdTardeMF.InnerText = "X";
                        tdTardeMF.Align = "center";
                    }

                    if (chkNoite.Checked || (chkNoiteNovo.Checked && !matFacil) || (chkNoiteNovo.Checked && (Session["perfil"].ToString() == "DIESP" || (Convert.ToString(tseRegional.DBValue) == "5"))))
                    {
                        tdNoiteAbsorcao.InnerText = "X";
                        tdNoiteAbsorcao.Align = "center";
                    }
                    if (chkNoiteNovo.Checked && matFacil && (Session["perfil"].ToString() != "DIESP" && (Convert.ToString(tseRegional.DBValue) != "5")))
                    {
                        tdNoiteMF.InnerText = "X";
                        tdNoiteMF.Align = "center";
                    }

                    if (chkAmpliado.Checked || (chkAmpliadoNovo.Checked && !matFacil) || (chkAmpliadoNovo.Checked && (Session["perfil"].ToString() == "DIESP" || (Convert.ToString(tseRegional.DBValue) == "5"))))
                    {
                        tdAmpliadoAbsorcao.InnerText = "X";
                        tdAmpliadoAbsorcao.Align = "center";
                    }
                    if (chkAmpliadoNovo.Checked && matFacil && (Session["perfil"].ToString() != "DIESP" && (Convert.ToString(tseRegional.DBValue) != "5")))
                    {
                        tdAmpliadoMF.InnerText = "X";
                        tdAmpliadoMF.Align = "center";
                    }

                    if (chkIntegral.Checked || (chkIntegralNovo.Checked && !matFacil) || (chkIntegralNovo.Checked && (Session["perfil"].ToString() == "DIESP" || (Convert.ToString(tseRegional.DBValue) == "5"))))
                    {
                        tdIntegralAbsorcao.InnerText = "X";
                        tdIntegralAbsorcao.Align = "center";
                    }
                    if (chkIntegralNovo.Checked && matFacil && (Session["perfil"].ToString() != "DIESP" && (Convert.ToString(tseRegional.DBValue) != "5")))
                    {
                        tdIntegralMF.InnerText = "X";
                        tdIntegralMF.Align = "center";
                    }

                    trTurnos.Cells.Add(tdPeriodo);
                    trTurnos.Cells.Add(tdCurso);
                    trTurnos.Cells.Add(tdManhaMF);
                    trTurnos.Cells.Add(tdTardeMF);
                    trTurnos.Cells.Add(tdNoiteMF);
                    trTurnos.Cells.Add(tdAmpliadoMF);
                    trTurnos.Cells.Add(tdIntegralMF);
                    trTurnos.Cells.Add(tdManhaAbsorcao);
                    trTurnos.Cells.Add(tdTardeAbsorcao);
                    trTurnos.Cells.Add(tdNoiteAbsorcao);
                    trTurnos.Cells.Add(tdAmpliadoAbsorcao);
                    trTurnos.Cells.Add(tdIntegralAbsorcao);

                    tbTurnos.Rows.Add(trTurnos);

                    lista.Add(confTurnos);

                }
            }
        }

        private List<DadosTurnoHabilitado> ObtemTurnosNovosHabilitados()
        {
            List<DadosTurnoHabilitado> listaTurnosHabilitados = new List<DadosTurnoHabilitado>();
            DadosTurnoHabilitado turnoHabilitado;
            string periodo = string.Empty;
            string serie = string.Empty;
            string descricaoCurso = string.Empty;
            string curso = string.Empty;
            string encerrado = string.Empty;
            CheckBox chkManhaNovo;
            CheckBox chkTardeNovo;
            CheckBox chkNoiteNovo;
            CheckBox chkIntegralNovo;
            CheckBox chkAmpliadoNovo;


            for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
            {
                periodo = this.grdConfTurnos.GetRowValues(rowIndex, "Periodo").ToString();
                serie = this.grdConfTurnos.GetRowValues(rowIndex, "Serie").ToString();
                curso = this.grdConfTurnos.GetRowValues(rowIndex, "Curso").ToString();
                descricaoCurso = this.grdConfTurnos.GetRowValues(rowIndex, "NomeCurso").ToString();
                encerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado") == false ? "0" : "1";
                chkManhaNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "ManhaNovo", "chkManhaNovo");
                chkTardeNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "TardeNovo", "chkTardeNovo");
                chkNoiteNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "NoiteNovo", "chkNoiteNovo");
                chkIntegralNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "IntegralNovo", "chkIntegralNovo");
                chkAmpliadoNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "AmpliadoNovo", "chkAmpliadoNovo");

                if (encerrado == "0")
                {
                    if (chkManhaNovo.Checked)
                    {
                        turnoHabilitado = new DadosTurnoHabilitado
                        {
                            Periodo = Convert.ToInt32(periodo),
                            Serie = Convert.ToInt32(serie),
                            DescricaoCurso = descricaoCurso,
                            Curso = curso,
                            Turno = "M"
                        };
                        listaTurnosHabilitados.Add(turnoHabilitado);
                    }

                    if (chkTardeNovo.Checked)
                    {
                        turnoHabilitado = new DadosTurnoHabilitado
                        {
                            Periodo = Convert.ToInt32(periodo),
                            Serie = Convert.ToInt32(serie),
                            DescricaoCurso = descricaoCurso,
                            Curso = curso,
                            Turno = "T"
                        };
                        listaTurnosHabilitados.Add(turnoHabilitado);
                    }

                    if (chkNoiteNovo.Checked)
                    {
                        turnoHabilitado = new DadosTurnoHabilitado
                        {
                            Periodo = Convert.ToInt32(periodo),
                            Serie = Convert.ToInt32(serie),
                            DescricaoCurso = descricaoCurso,
                            Curso = curso,
                            Turno = "N"
                        };
                        listaTurnosHabilitados.Add(turnoHabilitado);
                    }

                    if (chkIntegralNovo.Checked)
                    {
                        turnoHabilitado = new DadosTurnoHabilitado
                        {
                            Periodo = Convert.ToInt32(periodo),
                            Serie = Convert.ToInt32(serie),
                            DescricaoCurso = descricaoCurso,
                            Curso = curso,
                            Turno = "I"
                        };
                        listaTurnosHabilitados.Add(turnoHabilitado);
                    }

                    if (chkAmpliadoNovo.Checked)
                    {
                        turnoHabilitado = new DadosTurnoHabilitado
                        {
                            Periodo = Convert.ToInt32(periodo),
                            Serie = Convert.ToInt32(serie),
                            DescricaoCurso = descricaoCurso,
                            Curso = curso,
                            Turno = "A"
                        };
                        listaTurnosHabilitados.Add(turnoHabilitado);
                    }
                }
            }

            return listaTurnosHabilitados;
        }

        protected void btnAbrirPopupTurnosFinalizar_Click(object sender, EventArgs e)
        {
            GerarPopupTurnosOfertados();

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupTurnosFinalizar();", true);
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupFinalizar();", true);

            updatePanel9.Update();
        }

        private void SalvaTurnos(bool salvaAnalise)
        {
            try
            {
                CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
                CtvAnalise rnCtvAnalise = new CtvAnalise();
                List<TceCtvAnalise> analises = new List<TceCtvAnalise>();
                ValidacaoDados validacaoRemocaoTurno = new ValidacaoDados();
                int totalTurnos = 0;
                Turno rnTurno = new Turno();
                int contFinalizado = 0;
                string perfilAnalise = string.Empty;

                if (this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                    return;
                }

                var valido = true;
                var lista = new List<DadosConfTurno>();
                var erros = new List<string>();

                totalTurnos = rnTurno.ObtemTotalTurnos();

                for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
                {
                    var linha = rowIndex + 1;
                    var id = this.grdConfTurnos.GetRowValues(rowIndex, "IdAgendaConfTurnoVaga").ToString();
                    var agendaId = this.grdConfTurnos.GetRowValues(rowIndex, "AgendaId").ToString();
                    var serie = this.grdConfTurnos.GetRowValues(rowIndex, "Serie").ToString();
                    var periodo = this.grdConfTurnos.GetRowValues(rowIndex, "Periodo").ToString();
                    var curso = this.grdConfTurnos.GetRowValues(rowIndex, "Curso").ToString();
                    var nomeCurso = this.grdConfTurnos.GetRowValues(rowIndex, "NomeCurso").ToString();
                    var encerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado") == false ? "0" : "1";
                    var finalizado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Finalizado");

                    var chkManha = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Manha", "chkManha");
                    var chkTarde = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Tarde", "chkTarde");
                    var chkNoite = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Noite", "chkNoite");
                    var chkIntegral = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Integral", "chkIntegral");
                    var chkAmpliado = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Ampliado", "chkAmpliado");
                    var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "Justificativa", "txtJustificativa");
                    var hdnValorAntigo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, rowIndex, "Justificativa", "hdnValorAntigo");

                    var chkManhaNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "ManhaNovo", "chkManhaNovo");
                    var chkTardeNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "TardeNovo", "chkTardeNovo");
                    var chkNoiteNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "NoiteNovo", "chkNoiteNovo");
                    var chkIntegralNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "IntegralNovo", "chkIntegralNovo");
                    var chkAmpliadoNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "AmpliadoNovo", "chkAmpliadoNovo");
                    var txtJustificativaNovo = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "JustificativaNovo", "txtJustificativaNovo");
                    var hdnValorAntigoNovo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, rowIndex, "JustificativaNovo", "hdnValorAntigoNovo");

                    if (encerrado == "0")
                    {
                        var confTurnos = new DadosConfTurno
                        {
                            IdAgendaConfTurnoVaga = int.Parse(id),
                            AgendaId = Convert.ToInt32(agendaId),
                            Censo = Convert.ToString(this.tseUnidadeResponsavel.DBValue),
                            Ano = Convert.ToInt32(ddlAno.SelectedValue),
                            Periodo = Convert.ToInt32(periodo),
                            CodigoTipo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "CodigoTipo")),
                            CodigoModalidade = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "CodigoModalidade")),
                            Manha = chkManha.Checked,
                            ManhaCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "ManhaCodigo")),
                            Tarde = chkTarde.Checked,
                            TardeCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TardeCodigo")),
                            Noite = chkNoite.Checked,
                            NoiteCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "NoiteCodigo")),
                            Integral = chkIntegral.Checked,
                            IntegralCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "IntegralCodigo")),
                            Ampliado = chkAmpliado.Checked,
                            AmpliadoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "AmpliadoCodigo")),
                            TurnosIniciais = hdnValorAntigo.Value,
                            Justificativa = txtJustificativa.Text.Trim(),
                            ManhaNovo = chkManhaNovo.Checked,
                            ManhaNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "ManhaNovoCodigo")),
                            TardeNovo = chkTardeNovo.Checked,
                            TardeNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TardeNovoCodigo")),
                            NoiteNovo = chkNoiteNovo.Checked,
                            NoiteNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "NoiteNovoCodigo")),
                            IntegralNovo = chkIntegralNovo.Checked,
                            IntegralNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "IntegralNovoCodigo")),
                            AmpliadoNovo = chkAmpliadoNovo.Checked,
                            AmpliadoNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "AmpliadoNovoCodigo")),
                            JustificativaNovo = txtJustificativaNovo.Text.Trim(),
                            PerfilResponsavel = Session["perfil"].ToString(),
                            Serie = Convert.ToInt32(serie),
                            NomeCurso = nomeCurso,
                            Curso = curso,
                            Finalizado = Convert.ToBoolean(finalizado),
                            Matricula = this.User.Identity.Name,
                            TurnosListaInicial = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TurnosListaInicial"))
                        };

                        var validacao = CtvConfTurno.ValidarParcial(confTurnos, Convert.ToInt32(Session["codPerfil"]), totalTurnos);

                        if (validacao.Valido)
                        {
                            lista.Add(confTurnos);
                        }
                        else
                        {
                            erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, validacao.Mensagem));

                            valido = false;
                        }

                        if (finalizado)
                        {
                            contFinalizado = contFinalizado + 1;
                        }
                    }
                }

                if (contFinalizado > 0)
                {
                    if (lista.Count() > 0)
                    {
                        validacaoRemocaoTurno = rnCtvConfTurno.VerificaRemocaoTurno(Convert.ToInt32(Session["codPerfil"]), lista);

                        if (!validacaoRemocaoTurno.Valido)
                        {
                            erros.Add(validacaoRemocaoTurno.Mensagem);
                            valido = false;
                        }
                    }
                }

                if (valido)
                {
                    if (!salvaAnalise)
                    {
                        CtvConfTurno.Salvar(lista);

                        lblMensagem.Text = "Salvo parcialmente com sucesso.";

                        this.grdConfTurnos.DataBind();

                        Valores.Clear();
                        this.ReMontaDataSources();

                        this.InicializaVagas();

                        updatePanel3.Update();
                        updatePanel9.Update();
                    }
                    else
                    {
                        //Caso esteja em modo de analise
                        perfilAnalise = ddlPerfil.SelectedItem.Text;

                        //Monta lista de analises de turnos
                        analises = this.MontaAnalisesTurnosPor(perfilAnalise);

                        rnCtvConfTurno.SalvaComAnalise(lista, analises, perfilAnalise);

                        lblMensagem.Text = "Salvo com sucesso.";

                        this.grdConfTurnos.DataBind();

                        Valores.Clear();
                        this.ReMontaDataSources();

                        this.InicializaVagas();

                        updatePanel3.Update();
                        updatePanel9.Update();
                    }
                }
                else
                {
                    this.lblMensagem.Text = erros.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvarParcialTurnos_Click(object sender, EventArgs e)
        {
            SalvaTurnos(false);
        }

        protected void btnAbrirPopupTurnosParcial_Click(object sender, EventArgs e)
        {
            try
            {
                int Finalizado = 0;
                int idAgenda = 0;
                RN.Agenda.Evento rnEvento = new Techne.Lyceum.RN.Agenda.Evento();
                DateTime dataFimAgenda = DateTime.MinValue;

                //btnSalvarParcialTurnos_Click(sender, e);
                SalvaTurnos(false);

                //Conta quantas linhas estão finalizadas
                for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
                {
                    var Ehfinalizado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Finalizado");

                    if (Ehfinalizado)
                    {
                        Finalizado = Finalizado + 1;
                    }
                }

                //Verifica se existem linhas sem finalização
                if (this.grdConfTurnos.VisibleRowCount != Finalizado)
                {
                    if (!lblMensagem.Text.Contains("Linha"))
                    {
                        idAgenda = ObtemAgendaId();

                        dataFimAgenda = rnEvento.ObtemDataFimPor(Convert.ToInt32(ddlAno.SelectedValue), idAgenda, Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoTurnos), Convert.ToInt32(Session["codPerfil"]));

                        if (dataFimAgenda != DateTime.MinValue && dataFimAgenda != null)
                        {
                            lblPopupMensagemParcial.Text = dataFimAgenda.ToString("dd/MM/yyyy");
                        }

                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupTurnosParcial();",
                                                            true);

                        updatePanel8.Update();
                        updatePanel2.Update();
                        //updatePanel7.Update();
                        updatePanel3.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnFinalizarTurnos_Click(object sender, EventArgs e)
        {
            try
            {
                CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
                ValidacaoDados validacaoRemocaoTurno = new ValidacaoDados();
                int totalTurnos = 0;
                Turno rnTurno = new Turno();
                string mensagem;

                this.pucConfirmarTurnos.ShowOnPageLoad = false;
                lblMensagem.Text = string.Empty;
                if (this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                    return;
                }

                if (this.grdConfTurnos.VisibleRowCount == 0)
                {
                    lblMensagem.Text = "Não existem turnos a serem finalizados.";
                    return;
                }

                if (!ValidaJustificativaFinalizarTurno(out mensagem))
                {
                    lblMensagem.Text = mensagem;
                    return;
                }

                var valido = true;
                //var validoFinalizacao = true;
                var lista = new List<DadosConfTurno>();
                var listaAbertos = new List<DadosConfTurno>();
                var erros = new List<string>();

                totalTurnos = rnTurno.ObtemTotalTurnos();

                for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
                {
                    var linha = rowIndex + 1;

                    var id = this.grdConfTurnos.GetRowValues(rowIndex, "IdAgendaConfTurnoVaga").ToString();
                    var agendaId = this.grdConfTurnos.GetRowValues(rowIndex, "AgendaId").ToString();
                    var serie = this.grdConfTurnos.GetRowValues(rowIndex, "Serie").ToString();
                    var curso = this.grdConfTurnos.GetRowValues(rowIndex, "Curso").ToString();
                    var periodo = this.grdConfTurnos.GetRowValues(rowIndex, "Periodo").ToString();
                    var nomeCurso = this.grdConfTurnos.GetRowValues(rowIndex, "NomeCurso").ToString();
                    var encerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado") == false ? "0" : "1";

                    var chkManha = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Manha", "chkManha");
                    var chkTarde = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Tarde", "chkTarde");
                    var chkNoite = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Noite", "chkNoite");
                    var chkIntegral = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Integral", "chkIntegral");
                    var chkAmpliado = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Ampliado", "chkAmpliado");
                    var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "Justificativa", "txtJustificativa");
                    var hdnValorAntigo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, rowIndex, "Justificativa", "hdnValorAntigo");

                    var chkManhaNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "ManhaNovo", "chkManhaNovo");
                    var chkTardeNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "TardeNovo", "chkTardeNovo");
                    var chkNoiteNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "NoiteNovo", "chkNoiteNovo");
                    var chkIntegralNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "IntegralNovo", "chkIntegralNovo");
                    var chkAmpliadoNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "AmpliadoNovo", "chkAmpliadoNovo");
                    var txtJustificativaNovo = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "JustificativaNovo", "txtJustificativaNovo");

                    if (encerrado == "0")
                    {
                        var ctvTurnos = new DadosConfTurno()
                        {
                            IdAgendaConfTurnoVaga = int.Parse(id),
                            AgendaId = Convert.ToInt32(agendaId),
                            Censo = Convert.ToString(this.tseUnidadeResponsavel.DBValue),
                            Ano = Convert.ToInt32(ddlAno.SelectedValue),
                            Periodo = Convert.ToInt32(periodo),
                            CodigoTipo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "CodigoTipo")),
                            CodigoModalidade = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "CodigoModalidade")),
                            Manha = chkManha.Checked,
                            ManhaCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "ManhaCodigo")),
                            Tarde = chkTarde.Checked,
                            TardeCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TardeCodigo")),
                            Noite = chkNoite.Checked,
                            NoiteCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "NoiteCodigo")),
                            Integral = chkIntegral.Checked,
                            IntegralCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "IntegralCodigo")),
                            Ampliado = chkAmpliado.Checked,
                            AmpliadoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "AmpliadoCodigo")),
                            TurnosIniciais = hdnValorAntigo.Value,
                            Justificativa = txtJustificativa.Text.Trim(),
                            ManhaNovo = chkManhaNovo.Checked,
                            ManhaNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "ManhaNovoCodigo")),
                            TardeNovo = chkTardeNovo.Checked,
                            TardeNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TardeNovoCodigo")),
                            NoiteNovo = chkNoiteNovo.Checked,
                            NoiteNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "NoiteNovoCodigo")),
                            IntegralNovo = chkIntegralNovo.Checked,
                            IntegralNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "IntegralNovoCodigo")),
                            AmpliadoNovo = chkAmpliadoNovo.Checked,
                            AmpliadoNovoCodigo = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "AmpliadoNovoCodigo")),
                            JustificativaNovo = txtJustificativaNovo.Text.Trim(),
                            PerfilResponsavel = Session["perfil"].ToString(),
                            Serie = Convert.ToInt32(serie),
                            NomeCurso = nomeCurso,
                            Curso = curso,
                            Matricula = this.User.Identity.Name,
                            TurnosListaInicial = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TurnosListaInicial"))
                        };

                        var validacao = CtvConfTurno.Validar(ctvTurnos, Convert.ToInt32(Session["codPerfil"]), totalTurnos);

                        if (validacao.Valido)
                        {
                            lista.Add(ctvTurnos);

                            var validacaoFin = CtvFinalizado.Validar(ctvTurnos);

                            if (validacaoFin.Valido)
                            {
                                listaAbertos.Add(ctvTurnos);
                            }
                        }
                        else
                        {
                            erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, validacao.Mensagem));

                            valido = false;
                        }
                    }
                }
                if (lista.Count() > 0)
                {
                    validacaoRemocaoTurno = rnCtvConfTurno.VerificaRemocaoTurno(Convert.ToInt32(Session["codPerfil"]), lista);

                    if (!validacaoRemocaoTurno.Valido)
                    {
                        erros.Add(validacaoRemocaoTurno.Mensagem);
                        valido = false;
                    }
                }

                if (valido)
                {
                    CtvConfTurno.Salvar(listaAbertos);

                    CtvFinalizado.InserirTurno(listaAbertos);

                    lblMensagem.Text = "Finalizado com sucesso.";

                    this.grdConfTurnos.DataBind();

                    btnFinalizarTurnos.Visible = false;
                    if (Session["perfil"].ToString() != "DIRETOR_UE")
                    {
                        btnConfirmarTurnos.Visible = true;
                    }
                    else
                    {
                        btnSalvarParcialTurnos.Visible = false;
                    }

                    this.InicializaVagas();

                    updatePanel3.Update();
                    updatePanel9.Update();
                }
                else
                {
                    this.lblMensagem.Text = erros.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvarAnaliseTurnos_Click(object sender, EventArgs e)
        {
            CtvAnalise rnCtvAnalise = new CtvAnalise();
            List<TceCtvAnalise> analises = new List<TceCtvAnalise>();
            ValidacaoDados validacaoAnalises = new ValidacaoDados();
            var erros = new List<string>();
            string perfilAnalise = string.Empty;

            try
            {
                perfilAnalise = ddlPerfil.SelectedItem.Text;

                //Monta lista de analises de turnos
                analises = this.MontaAnalisesTurnosPor(perfilAnalise);

                validacaoAnalises = rnCtvAnalise.ValidaListaAnalises(analises, perfilAnalise);

                if (validacaoAnalises.Valido)
                {
                    SalvaTurnos(true);
                }
                else
                {
                    erros.Add(validacaoAnalises.Mensagem);
                    this.lblMensagem.Text = erros.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Private/protected Methods Turno

        protected void PreencheDadosAnaliseTurnos(int ano, string censo)
        {
            CtvAnalise rnCtvAnalise = new CtvAnalise();
            List<DadosAnalise> listaAnalises = new List<DadosAnalise>();
            hdnPodeAnalisarTurno.Value = string.Empty;

            if (!pnlAnalise.Visible)
            {
                DadosAnalise dados = rnCtvAnalise.ObtemAnalisesPor(ano, censo, true, false);

                if (dados != null)
                {
                    lblAnaliseTurnosGeralSUPED.Text = dados.AnaliseSuped.Replace(Environment.NewLine, "<br />");
                    lblAnaliseTurnosGeralSUPLAN.Text = dados.AnaliseSuplan.Replace(Environment.NewLine, "<br />");
                    lblAnaliseTurnosGeralDIESP.Text = dados.AnaliseDiesp.Replace(Environment.NewLine, "<br />");
                    lblAnaliseTurnosGeralSUPED.Visible = true;
                    lblAnaliseTurnosGeralSUPLAN.Visible = true;
                    lblAnaliseTurnosGeralDIESP.Visible = true;
                }
            }
            else
            {
                LimparCamposAnaliseTurno();

                MasterPage ctl00 = FindControl("ctl00") as MasterPage;

                ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;

                listaAnalises = rnCtvAnalise.ListaDadosAnalisesPor(ano, censo, true, false);

                if (listaAnalises.FindAll(x => x.AnaliseDiespEditavel == true || x.AnaliseSupedEditavel == true || x.AnaliseSuplanEditavel == true).Count() > 0)
                {
                    hdnPodeAnalisarTurno.Value = "true";
                }

                RN.TurnosVagas.ResultadoAnalise rnResultadoAnalise = new RN.TurnosVagas.ResultadoAnalise();
                List<RN.TurnosVagas.Entidades.ResultadoAnalise> resultados = new List<RN.TurnosVagas.Entidades.ResultadoAnalise>();
                resultados = rnResultadoAnalise.ListaResultadosAnaliseAtivos();

                bool ehPeriodoJunto = true;

                if (listaAnalises.FindAll(x => x.Periodo == 2).Count > 0)
                {
                    ehPeriodoJunto = false;
                }

                foreach (var item in listaAnalises)
                {
                    DropDownList ddlResultadoSUPED = new DropDownList();
                    DropDownList ddlResultadoSUPLAN = new DropDownList();
                    DropDownList ddlResultadoDIESP = new DropDownList();
                    Label lblAnaliseSuped = new Label();
                    Label lblAnaliseSuplan = new Label();
                    Label lblAnaliseDiesp = new Label();

                    lblAnaliseSuped = MainContent.FindControl("lblAnaliseTurnosSUPED" + item.Periodo) as Label;

                    Label lblDescAnaliseTurnosSUPED = MainContent.FindControl("lblDescAnaliseTurnosSUPEDPeriodo" + item.Periodo) as Label;


                    if (item.AnaliseSupedEditavel && ddlPerfil.SelectedItem.Text == "SUPED")
                    {
                        if (item.Periodo == 0 || item.Periodo == 1)
                        {
                            ddlResultadoSUPED = MainContent.FindControl("ddlResultadoTurnosSUPEDPeriodo0") as DropDownList;
                            lblAnaliseSuped = MainContent.FindControl("lblAnaliseTurnosSUPED0") as Label;

                        }
                        else
                        {
                            ddlResultadoSUPED = MainContent.FindControl("ddlResultadoTurnosSUPEDPeriodo" + item.Periodo) as DropDownList;

                        }
                        if (ehPeriodoJunto)
                        {
                            lblAnaliseSuped.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: 0 e 1 - ";
                        }
                        else
                        {
                            lblAnaliseSuped.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                        }
                        ddlResultadoSUPED.Items.Clear();
                        ddlResultadoSUPED.Visible = true;
                        lblAnaliseSuped.Visible = true;
                        ddlResultadoSUPED.DataSource = resultados;
                        ddlResultadoSUPED.DataBind();
                        ListItem ls = new ListItem("Selecione", string.Empty);
                        ddlResultadoSUPED.Items.Insert(0, ls);
                        if (!string.IsNullOrEmpty(item.AnaliseSuped))
                        {
                            ddlResultadoSUPED.SelectedValue = item.AnaliseSuped;
                        }
                        lblDescAnaliseTurnosSUPED.Visible = false;
                    }
                    else
                    {
                        ddlResultadoSUPED.Visible = false;
                        if (!string.IsNullOrEmpty(item.AnaliseSuped))
                        {
                            lblDescAnaliseTurnosSUPED.Visible = true;
                            lblAnaliseSuped.Visible = true;
                            lblAnaliseSuped.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                            lblDescAnaliseTurnosSUPED.Text = item.AnaliseSuped + " - em " + item.DataAnaliseSuped.ToString("dd/MM/yyyy") + " às " + item.DataAnaliseSuped.ToString("HH:mm") + " por " + item.ResponsavelAnaliseSuped;
                        }
                    }


                    lblAnaliseSuplan = MainContent.FindControl("lblAnaliseTurnosSUPLAN" + item.Periodo) as Label;


                    Label lblDescAnaliseTurnosSUPLAN = MainContent.FindControl("lblDescAnaliseTurnosSUPLANPeriodo" + item.Periodo) as Label;

                    if (item.AnaliseSuplanEditavel && ddlPerfil.SelectedItem.Text == "SUPLAN")
                    {
                        if (item.Periodo == 0 || item.Periodo == 1)
                        {
                            lblAnaliseSuplan = MainContent.FindControl("lblAnaliseTurnosSUPLAN0") as Label;
                            ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoTurnosSUPLANPeriodo0") as DropDownList;
                        }
                        else
                        {
                            ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoTurnosSUPLANPeriodo" + item.Periodo) as DropDownList;
                        }
                        if (ehPeriodoJunto)
                        {
                            lblAnaliseSuplan.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: 0 e 1 - ";
                        }
                        else
                        {
                            lblAnaliseSuplan.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                        }
                        ddlResultadoSUPLAN.Items.Clear();
                        ddlResultadoSUPLAN.Visible = true;
                        lblAnaliseSuplan.Visible = true;
                        ddlResultadoSUPLAN.DataSource = resultados;
                        ddlResultadoSUPLAN.DataBind();
                        ListItem ls = new ListItem("Selecione", string.Empty);
                        ddlResultadoSUPLAN.Items.Insert(0, ls);
                        if (!string.IsNullOrEmpty(item.AnaliseSuplan))
                        {
                            ddlResultadoSUPLAN.SelectedValue = item.AnaliseSuplan;
                        }
                        lblDescAnaliseTurnosSUPLAN.Visible = false;
                    }
                    else
                    {
                        ddlResultadoSUPLAN.Visible = false;
                        if (!string.IsNullOrEmpty(item.AnaliseSuplan))
                        {
                            lblDescAnaliseTurnosSUPLAN.Visible = true;
                            lblAnaliseSuplan.Visible = true;
                            lblAnaliseSuplan.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                            lblDescAnaliseTurnosSUPLAN.Text = item.AnaliseSuplan + " - em " + item.DataAnaliseSuplan.ToString("dd/MM/yyyy") + " às " + item.DataAnaliseSuplan.ToString("HH:mm") + " por " + item.ResponsavelAnaliseSuplan;
                        }
                    }

                    lblAnaliseDiesp = MainContent.FindControl("lblAnaliseTurnosDIESP" + item.Periodo) as Label;

                    Label lblDescAnaliseTurnosDIESP = MainContent.FindControl("lblDescAnaliseTurnosDIESPPeriodo" + item.Periodo) as Label;

                    if (item.AnaliseDiespEditavel && ddlPerfil.SelectedItem.Text == "DIESP")
                    {
                        if (item.Periodo == 0 || item.Periodo == 1)
                        {
                            lblAnaliseDiesp = MainContent.FindControl("lblAnaliseTurnosDIESP0") as Label;
                            ddlResultadoDIESP = MainContent.FindControl("ddlResultadoTurnosDIESPPeriodo0") as DropDownList;
                        }
                        else
                        {
                            ddlResultadoDIESP = MainContent.FindControl("ddlResultadoTurnosDIESPPeriodo" + item.Periodo) as DropDownList;
                        }
                        if (ehPeriodoJunto)
                        {
                            lblAnaliseDiesp.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: 0 e 1 - ";
                        }
                        else
                        {
                            lblAnaliseDiesp.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                        }
                        ddlResultadoDIESP.Items.Clear();
                        ddlResultadoDIESP.Visible = true;
                        lblAnaliseDiesp.Visible = true;
                        ddlResultadoDIESP.DataSource = resultados;
                        ddlResultadoDIESP.DataBind();
                        ListItem ls = new ListItem("Selecione", string.Empty);
                        ddlResultadoDIESP.Items.Insert(0, ls);
                        if (!string.IsNullOrEmpty(item.AnaliseDiesp))
                        {
                            ddlResultadoDIESP.SelectedValue = item.AnaliseDiesp;
                        }
                        lblDescAnaliseTurnosDIESP.Visible = false;
                    }
                    else
                    {
                        ddlResultadoDIESP.Visible = false;
                        if (!string.IsNullOrEmpty(item.AnaliseDiesp))
                        {
                            lblDescAnaliseTurnosDIESP.Visible = true;
                            lblAnaliseDiesp.Visible = true;
                            lblAnaliseDiesp.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                            lblDescAnaliseTurnosDIESP.Text = item.AnaliseDiesp + " - em " + item.DataAnaliseDiesp.ToString("dd/MM/yyyy") + " às " + item.DataAnaliseDiesp.ToString("HH:mm") + " por " + item.ResponsavelAnaliseDiesp;
                        }
                    }

                }
            }
        }

        private int ObtemAgendaId()
        {
            int idAgenda = 0;
            bool encerrado;

            for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
            {
                encerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado");
                if (!encerrado)
                {
                    idAgenda = Convert.ToInt32(this.grdConfTurnos.GetRowValues(rowIndex, "AgendaId").ToString());
                    break;
                }
            }

            return idAgenda;
        }

        private int ObtemAgendaIdVagas()
        {
            int idAgenda = 0;
            bool encerrado;

            for (var rowIndex = 0; rowIndex < this.gridVagas.VisibleRowCount; rowIndex++)
            {
                encerrado = (bool)this.gridVagas.GetRowValues(rowIndex, "Encerrado");
                if (!encerrado)
                {
                    idAgenda = Convert.ToInt32(this.gridVagas.GetRowValues(rowIndex, "AgendaID").ToString());
                    break;
                }
            }

            return idAgenda;
        }

        private List<int> ObtemPeriodosVigentes()
        {
            var listaPeriodos = new List<int>();
            int periodo;
            bool encerrado;

            for (var rowIndex = 0; rowIndex < this.gridVagas.VisibleRowCount; rowIndex++)
            {
                encerrado = (bool)this.gridVagas.GetRowValues(rowIndex, "Encerrado");
                periodo = (int)this.gridVagas.GetRowValues(rowIndex, "Periodo");

                if (!encerrado)
                {
                    listaPeriodos.Add(periodo);
                }
            }

            return listaPeriodos;
        }

        protected void LimparCamposAnaliseTurno()
        {
            lblAnaliseTurnosGeralSUPED.Text = string.Empty;
            lblAnaliseTurnosSUPED0.Text = string.Empty;
            lblDescAnaliseTurnosSUPEDPeriodo0.Text = string.Empty;
            ddlResultadoTurnosSUPEDPeriodo0.Items.Clear();
            ddlResultadoTurnosSUPEDPeriodo0.Visible = false;

            lblAnaliseTurnosSUPED1.Text = string.Empty;
            lblDescAnaliseTurnosSUPEDPeriodo1.Text = string.Empty;


            lblAnaliseTurnosSUPED2.Text = string.Empty;
            lblDescAnaliseTurnosSUPEDPeriodo2.Text = string.Empty;
            ddlResultadoTurnosSUPEDPeriodo2.Items.Clear();
            ddlResultadoTurnosSUPEDPeriodo2.Visible = false;

            lblAnaliseTurnosGeralSUPLAN.Text = string.Empty;
            lblAnaliseTurnosSUPLAN0.Text = string.Empty;
            lblDescAnaliseTurnosSUPLANPeriodo0.Text = string.Empty;
            ddlResultadoTurnosSUPLANPeriodo0.Items.Clear();
            ddlResultadoTurnosSUPLANPeriodo0.Visible = false;

            lblAnaliseTurnosSUPLAN1.Text = string.Empty;
            lblDescAnaliseTurnosSUPLANPeriodo1.Text = string.Empty;

            lblAnaliseTurnosSUPLAN2.Text = string.Empty;
            lblDescAnaliseTurnosSUPLANPeriodo2.Text = string.Empty;
            ddlResultadoTurnosSUPLANPeriodo2.Items.Clear();
            ddlResultadoTurnosSUPLANPeriodo2.Visible = false;

            lblAnaliseTurnosGeralDIESP.Text = string.Empty;
            lblAnaliseTurnosDIESP0.Text = string.Empty;
            lblDescAnaliseTurnosDIESPPeriodo0.Text = string.Empty;
            ddlResultadoTurnosDIESPPeriodo0.Items.Clear();
            ddlResultadoTurnosDIESPPeriodo0.Visible = false;

            lblAnaliseTurnosDIESP1.Text = string.Empty;
            lblDescAnaliseTurnosDIESPPeriodo1.Text = string.Empty;

            lblAnaliseTurnosDIESP2.Text = string.Empty;
            lblDescAnaliseTurnosDIESPPeriodo2.Text = string.Empty;
            ddlResultadoTurnosDIESPPeriodo2.Items.Clear();
            ddlResultadoTurnosDIESPPeriodo2.Visible = false;

        }

        protected void limpaCamposFiltroAnalise()
        {
            rblTurnosAnalisados.ClearSelection();
            rblTurnosAnalisados.Items[0].Selected = true;
            lblFiltroResultadoTurnos.Visible = false;
            ddlResultadoTurnos.Visible = false;
            ddlResultadoTurnos.Items.Clear();

            rblVagasAnalisadas.ClearSelection();
            rblVagasAnalisadas.Items[0].Selected = true;
            lblFiltroResultadoVagas.Visible = false;
            ddlResultadoVagas.Visible = false;
            ddlResultadoVagas.Items.Clear();

            txtFaixaInicial.Text = string.Empty;
            txtFaixaFinal.Text = string.Empty;
            ddlModSegCurso.Items.Clear();
            ddlModSegCurso.SelectedIndex = 0;
            ddlSerie.Items.Clear();
            ddlSerie.DataSource = null;
            ddlSerie.DataBind();
            ListItem ls = new ListItem("Todas", string.Empty);
            ddlSerie.Items.Insert(0, ls);
            ddlTurno.Items.Clear();
            ddlTurno.DataSource = null;
            ddlTurno.DataBind();
            ListItem lsT = new ListItem("Todos", string.Empty);
            ddlTurno.Items.Insert(0, lsT);
            ddlSerie.Enabled = false;
            ddlTurno.Enabled = false;

        }
        #endregion


        #region Inicialização Vagas

        private void InicializaVagas()
        {
            try
            {
                int codPerfil = -1;
                int agendaId = -1;
                int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);
                RN.CtvConfVaga rnCtvConfVaga = new CtvConfVaga();
                RN.CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
                RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
                RN.Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new Techne.Lyceum.RN.Agenda.ParametroTurnoVaga();
                pnGridVagas.Enabled = true;
                Session["podeTurmaProvisoria"] = null;

                this.VerificaPerfil();

                if (!string.IsNullOrEmpty(Session["codPerfil"].ToString()))
                {
                    codPerfil = Convert.ToInt32(Session["codPerfil"]);
                }             

                var censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                int ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;

                Valores = new Dictionary<String, List<int>>();
                Controle = new Dictionary<String, int[]>();
                Confirmacao = new List<DadosAgendaVagas>();
                IndicesUtilizados = new List<int>();

                var conf = rnCtvConfVaga.ListaQuadroPropostaPor(ano, censo, codPerfil, Session["perfil"].ToString(), tipoEventoConfirmacaoVagas);

                foreach (DataRow item in conf.Rows)
                {
                    var dado = new DadosAgendaVagas
                    {
                        ID = item["ID_JUSTIFICATIVA"] != DBNull.Value ? Convert.ToInt32(item["ID_JUSTIFICATIVA"]) : 0,
                        IDAgenda = Convert.ToInt32(item["id_agenda_conf_turno_vaga"]),
                        AgendaID = Convert.ToInt32(item["AGENDAID"]),
                        SerieEntrada = Convert.ToBoolean(item["ENTRADA"]),
                        Periodo = Convert.ToInt32(item["PERIODO"]),
                        Modalidade = item["DESCRICAO_MODALIDADE"].ToString(),
                        Curso = item["CURSO"].ToString(),
                        DescricaoSerie = item["DESCRICAO_SERIE"].ToString(),
                        Serie = Convert.ToString(item["SERIE"]),
                        JustificativaNova = item["JUSTIFICATIVA_NOVO"].ToString(),
                        QuantCont = item["VAGAS_CONTINUIDADE"] != DBNull.Value ? Convert.ToInt32(item["VAGAS_CONTINUIDADE"]) : 0,
                        QuantNovas = item["VAGAS_NOVAS"] != DBNull.Value ? Convert.ToInt32(item["VAGAS_NOVAS"]) : 0,
                        Editavel = Convert.ToBoolean(item["EDITAVEL"]),
                        QuantContVagasUtilizadas = Convert.ToInt32(item["VAGAS_UTILIZADAS_CONT"]),
                        QuantNovasVagasUtilizadas = Convert.ToInt32(item["VAGAS_UTILIZADAS_NOVA"]),
                        QuantContSeeduc = Convert.ToInt32(item["PROPOSTA_SEEDUC_CONT"]),
                        QuantNovaSeeduc = Convert.ToInt32(item["PROPOSTA_SEEDUC_NOVA"]),
                        NomeCurso = item["NOME_CURSO"].ToString(),
                        TaxaAprovacao = Convert.ToString(item["TAXAAPROVACAO"]),
                        TaxaReprovacao = Convert.ToString(item["TAXAREPROVACAO"]),
                        Finalizado = Convert.ToBoolean(item["FINALIZADO"]),
                        Encerrado = Convert.ToBoolean(item["ENCERRADO"])
                    };

                    Confirmacao.Add(dado);
                }

                this.ListarVagas();
                gridVagas.DataBind();

                if (Session["CursosNaoParticipamVagas"] == null)
                {
                    Session["CursosNaoParticipamVagas"] = rnAgenda.ObtemListaCursosNaoParticipantesPor(ObtemAgendaIdVagas());
                }

                agendaId = ObtemAgendaIdVagas();

                if (codPerfil > 0 && agendaId > 0)
                {
                    Session["podeTurmaProvisoria"] = rnParametroTurnoVaga.RetornaPodeTurmaProvisoriaPor(agendaId, codPerfil);
                }
                
                List<int> periodos = ObtemPeriodosVigentes();

                var listagem = rnCtvConfVaga.ListaQuadroDeSalasPor(censo, ano, codPerfil, Session["perfil"].ToString(), tipoEventoConfirmacaoVagas, periodos, (List<string>)Session["CursosNaoParticipamVagas"])
                     .OrderBy(x => x.SalaCapacidade).ToList();

                var listagemAgrupadaPorSala = listagem.GroupBy(x => x.SalaCapacidade).ToList()
                        .Select(x => x.First()).ToList();

                ListagemSalas = listagem;

                var turmas = rnCtvConfVaga.ListaTurmasParaLancamentoPor(censo, ano, periodos, (List<string>)Session["CursosNaoParticipamVagas"]);

                Turmas = turmas;

                var finalizado = rnCtvConfTurno.EhTurnoFinalizadoPor(ano, censo, tipoEventoConfirmacaoVagas);
                if (finalizado)
                {
                    this.gridSalas.DataSource = listagemAgrupadaPorSala;
                    this.gridSalas.DataBind();
                }
                else
                {
                    Turmas = null;
                }
              
                this.pnGridVagas.Visible = true;
                this.preencheDadosAnaliseVagas(int.Parse(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString());
                this.ControlaVisibilidadeVagas();
                this.gridVagas.DataBind();

                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
                throw ex;
            }
        }

        protected void LimparCamposAnaliseVaga()
        {
            lblAnaliseVagasGeralSUPED.Text = string.Empty;
            lblAnaliseVagasSUPED0.Text = string.Empty;
            lblDescAnaliseVagasSUPEDPeriodo0.Text = string.Empty;
            ddlResultadoVagasSUPEDPeriodo0.Items.Clear();
            ddlResultadoVagasSUPEDPeriodo0.Visible = false;

            lblAnaliseVagasSUPED1.Text = string.Empty;
            lblDescAnaliseVagasSUPEDPeriodo1.Text = string.Empty;

            lblAnaliseVagasSUPED2.Text = string.Empty;
            lblDescAnaliseVagasSUPEDPeriodo2.Text = string.Empty;
            ddlResultadoVagasSUPEDPeriodo2.Items.Clear();
            ddlResultadoVagasSUPEDPeriodo2.Visible = false;

            lblAnaliseVagasGeralSUPLAN.Text = string.Empty;
            lblAnaliseVagasSUPLAN0.Text = string.Empty;
            lblDescAnaliseVagasSUPLANPeriodo0.Text = string.Empty;
            ddlResultadoVagasSUPLANPeriodo0.Items.Clear();
            ddlResultadoVagasSUPLANPeriodo0.Visible = false;

            lblAnaliseVagasSUPLAN1.Text = string.Empty;
            lblDescAnaliseVagasSUPLANPeriodo1.Text = string.Empty;

            lblAnaliseVagasSUPLAN2.Text = string.Empty;
            lblDescAnaliseVagasSUPLANPeriodo2.Text = string.Empty;
            ddlResultadoVagasSUPLANPeriodo2.Items.Clear();
            ddlResultadoVagasSUPLANPeriodo2.Visible = false;

            lblAnaliseVagasGeralDIESP.Text = string.Empty;
            lblAnaliseVagasDIESP0.Text = string.Empty;
            lblDescAnaliseVagasDIESPPeriodo0.Text = string.Empty;
            ddlResultadoVagasDIESPPeriodo0.Items.Clear();
            ddlResultadoVagasDIESPPeriodo0.Visible = false;

            lblAnaliseVagasDIESP1.Text = string.Empty;
            lblDescAnaliseVagasDIESPPeriodo1.Text = string.Empty;

            lblAnaliseVagasDIESP2.Text = string.Empty;
            lblDescAnaliseVagasDIESPPeriodo2.Text = string.Empty;
            ddlResultadoVagasDIESPPeriodo2.Items.Clear();
            ddlResultadoVagasDIESPPeriodo2.Visible = false;

        }

        private void preencheDadosAnaliseVagas(int ano, string censo)
        {
            try
            {
                CtvAnalise rnCtvAnalise = new CtvAnalise();
                List<DadosAnalise> listaAnalises = new List<DadosAnalise>();

                if (!pnlAnalise.Visible)
                {

                    DadosAnalise analise = rnCtvAnalise.ObtemAnalisesPor(ano, censo, false, true);

                    if (analise != null)
                    {
                        lblAnaliseVagasGeralSUPED.Text = analise.AnaliseSuped.Replace(Environment.NewLine, "<br />");
                        lblAnaliseVagasGeralSUPLAN.Text = analise.AnaliseSuplan.Replace(Environment.NewLine, "<br />");
                        lblAnaliseVagasGeralDIESP.Text = analise.AnaliseDiesp.Replace(Environment.NewLine, "<br />");
                        lblAnaliseVagasGeralSUPED.Visible = true;
                        lblAnaliseVagasGeralSUPLAN.Visible = true;
                        lblAnaliseVagasGeralDIESP.Visible = true;
                    }
                }
                else
                {
                    LimparCamposAnaliseVaga();

                    MasterPage ctl00 = FindControl("ctl00") as MasterPage;

                    ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;

                    listaAnalises = rnCtvAnalise.ListaDadosAnalisesPor(ano, censo, false, true);

                    RN.TurnosVagas.ResultadoAnalise rnResultadoAnalise = new RN.TurnosVagas.ResultadoAnalise();
                    List<RN.TurnosVagas.Entidades.ResultadoAnalise> resultados = new List<RN.TurnosVagas.Entidades.ResultadoAnalise>();
                    resultados = rnResultadoAnalise.ListaResultadosAnaliseAtivos();

                    bool ehPeriodoJunto = true;

                    if (listaAnalises.FindAll(x => x.Periodo == 2).Count > 0)
                    {
                        ehPeriodoJunto = false;
                    }

                    foreach (var item in listaAnalises)
                    {
                        DropDownList ddlResultadoSUPED = new DropDownList();
                        DropDownList ddlResultadoSUPLAN = new DropDownList();
                        DropDownList ddlResultadoDIESP = new DropDownList();
                        Label lblAnaliseSuped = new Label();
                        Label lblAnaliseSuplan = new Label();
                        Label lblAnaliseDiesp = new Label();

                        lblAnaliseSuped = MainContent.FindControl("lblAnaliseVagasSUPED" + item.Periodo) as Label;

                        Label lblDescAnaliseVagasSUPED = MainContent.FindControl("lblDescAnaliseVagasSUPEDPeriodo" + item.Periodo) as Label;


                        if (item.AnaliseSupedEditavel && ddlPerfil.SelectedItem.Text == "SUPED")
                        {
                            if (item.Periodo == 0 || item.Periodo == 1)
                            {
                                lblAnaliseSuped = MainContent.FindControl("lblAnaliseVagasSUPED0") as Label;
                                ddlResultadoSUPED = MainContent.FindControl("ddlResultadoVagasSUPEDPeriodo0") as DropDownList;
                            }
                            else
                            {
                                ddlResultadoSUPED = MainContent.FindControl("ddlResultadoVagasSUPEDPeriodo" + item.Periodo) as DropDownList;
                            }
                            if (ehPeriodoJunto)
                            {
                                lblAnaliseSuped.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: 0 e 1 - ";
                            }
                            else
                            {
                                lblAnaliseSuped.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                            }
                            ddlResultadoSUPED.Items.Clear();
                            ddlResultadoSUPED.Visible = true;
                            lblAnaliseSuped.Visible = true;
                            ddlResultadoSUPED.DataSource = resultados;
                            ddlResultadoSUPED.DataBind();
                            ListItem ls = new ListItem("Selecione", string.Empty);
                            ddlResultadoSUPED.Items.Insert(0, ls);
                            if (!string.IsNullOrEmpty(item.AnaliseSuped))
                            {
                                ddlResultadoSUPED.SelectedValue = item.AnaliseSuped;
                            }
                            lblDescAnaliseVagasSUPED.Visible = false;
                        }
                        else
                        {
                            ddlResultadoSUPED.Visible = false;
                            if (!string.IsNullOrEmpty(item.AnaliseSuped))
                            {
                                lblDescAnaliseVagasSUPED.Visible = true;
                                lblAnaliseSuped.Visible = true;
                                lblAnaliseSuped.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                                lblDescAnaliseVagasSUPED.Text = item.AnaliseSuped + " - em " + item.DataAnaliseSuped.ToString("dd/MM/yyyy") + " às " + item.DataAnaliseSuped.ToString("HH:mm") + " por " + item.ResponsavelAnaliseSuped;
                            }
                        }

                        lblAnaliseSuplan = MainContent.FindControl("lblAnaliseVagasSUPLAN" + item.Periodo) as Label;

                        Label lblDescAnaliseVagasSUPLAN = MainContent.FindControl("lblDescAnaliseVagasSUPLANPeriodo" + item.Periodo) as Label;

                        if (item.AnaliseSuplanEditavel && ddlPerfil.SelectedItem.Text == "SUPLAN")
                        {
                            if (item.Periodo == 0 || item.Periodo == 1)
                            {
                                lblAnaliseSuplan = MainContent.FindControl("lblAnaliseVagasSUPLAN0") as Label;
                                ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoVagasSUPLANPeriodo0") as DropDownList;
                            }
                            else
                            {
                                ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoVagasSUPLANPeriodo" + item.Periodo) as DropDownList;
                            }
                            if (ehPeriodoJunto)
                            {
                                lblAnaliseSuplan.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: 0 e 1 - ";
                            }
                            else
                            {
                                lblAnaliseSuplan.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                            }
                            ddlResultadoSUPLAN.Items.Clear();
                            ddlResultadoSUPLAN.Visible = true;
                            lblAnaliseSuplan.Visible = true;
                            ddlResultadoSUPLAN.DataSource = resultados;
                            ddlResultadoSUPLAN.DataBind();
                            ListItem ls = new ListItem("Selecione", string.Empty);
                            ddlResultadoSUPLAN.Items.Insert(0, ls);
                            if (!string.IsNullOrEmpty(item.AnaliseSuplan))
                            {
                                ddlResultadoSUPLAN.SelectedValue = item.AnaliseSuplan;
                            }
                            lblDescAnaliseVagasSUPLAN.Visible = false;
                        }
                        else
                        {
                            ddlResultadoSUPLAN.Visible = false;
                            if (!string.IsNullOrEmpty(item.AnaliseSuplan))
                            {
                                lblDescAnaliseVagasSUPLAN.Visible = true;
                                lblAnaliseSuplan.Visible = true;
                                lblAnaliseSuplan.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                                lblDescAnaliseVagasSUPLAN.Text = item.AnaliseSuplan + " - em " + item.DataAnaliseSuplan.ToString("dd/MM/yyyy") + " às " + item.DataAnaliseSuplan.ToString("HH:mm") + " por " + item.ResponsavelAnaliseSuplan;
                            }
                        }

                        lblAnaliseDiesp = MainContent.FindControl("lblAnaliseVagasDIESP" + item.Periodo) as Label;

                        Label lblDescAnaliseVagasDIESP = MainContent.FindControl("lblDescAnaliseVagasDIESPPeriodo" + item.Periodo) as Label;

                        if (item.AnaliseDiespEditavel && ddlPerfil.SelectedItem.Text == "DIESP")
                        {
                            if (item.Periodo == 0 || item.Periodo == 1)
                            {
                                lblAnaliseDiesp = MainContent.FindControl("lblAnaliseVagasDIESP0") as Label;
                                ddlResultadoDIESP = MainContent.FindControl("ddlResultadoVagasDIESPPeriodo0") as DropDownList;
                            }
                            else
                            {
                                ddlResultadoDIESP = MainContent.FindControl("ddlResultadoVagasDIESPPeriodo" + item.Periodo) as DropDownList;
                            }

                            if (ehPeriodoJunto)
                            {
                                lblAnaliseDiesp.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: 0 e 1 - ";
                            }
                            else
                            {
                                lblAnaliseDiesp.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                            }
                            ddlResultadoDIESP.Items.Clear();
                            ddlResultadoDIESP.Visible = true;
                            lblAnaliseDiesp.Visible = true;
                            ddlResultadoDIESP.DataSource = resultados;
                            ddlResultadoDIESP.DataBind();
                            ListItem ls = new ListItem("Selecione", string.Empty);
                            ddlResultadoDIESP.Items.Insert(0, ls);
                            if (!string.IsNullOrEmpty(item.AnaliseDiesp))
                            {
                                ddlResultadoDIESP.SelectedValue = item.AnaliseDiesp;
                            }
                            lblDescAnaliseVagasDIESP.Visible = false;
                        }
                        else
                        {
                            ddlResultadoDIESP.Visible = false;
                            if (!string.IsNullOrEmpty(item.AnaliseDiesp))
                            {
                                lblDescAnaliseVagasDIESP.Visible = true;
                                lblAnaliseDiesp.Visible = true;
                                lblAnaliseDiesp.Text = "Ano:" + ddlAno.SelectedValue + " Periodo: " + item.Periodo + " - ";
                                lblDescAnaliseVagasDIESP.Text = item.AnaliseDiesp + " - em " + item.DataAnaliseDiesp.ToString("dd/MM/yyyy") + " às " + item.DataAnaliseDiesp.ToString("HH:mm") + " por " + item.ResponsavelAnaliseDiesp;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
            }
        }

        private void ControlaVisibilidadeVagas()
        {
            RN.Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new RN.Agenda.ParametroTurnoVaga();
            RN.Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new RN.Agenda.Entidades.ParametroTurnoVaga();
            bool ehfinalizado = true;
            bool ehEncerrado = true;
            bool ehEditavel = false;
            int finalizado = 0;
            int encerrado = 0;
            int editavel = 0;
            int codPerfil = -1;
            bool permiteSalvarParcial = false;
            bool permiteFinalizar = false;
            int idAgenda = 0;

            try
            {
                if (this.gridVagas.VisibleRowCount > 0 && this.gridSalas.VisibleRowCount > 0)
                {
                    for (var rowIndex = 0; rowIndex < this.gridVagas.VisibleRowCount; rowIndex++)
                    {
                        ehfinalizado = (bool)this.gridVagas.GetRowValues(rowIndex, "Finalizado");
                        ehEncerrado = (bool)this.gridVagas.GetRowValues(rowIndex, "Encerrado");
                        ehEditavel = (bool)this.gridVagas.GetRowValues(rowIndex, "Editavel");

                        if (ehfinalizado)
                        {
                            finalizado = finalizado + 1;
                        }

                        if (ehEncerrado)
                        {
                            encerrado = encerrado + 1;
                        }

                        if (ehEditavel)
                        {
                            editavel = editavel + 1;
                        }
                    }

                    //Verifica se falta linhas para finalizar
                    if (this.gridVagas.VisibleRowCount != finalizado)
                    {
                        permiteSalvarParcial = true;
                        permiteFinalizar = true;
                    }
                    else
                    {
                        VerificaPerfil();
                        codPerfil = Convert.ToInt32(Session["codPerfil"]);

                        //Verifica se é privilegiado
                        if (codPerfil == 0)
                        {
                            permiteSalvarParcial = true;
                        }
                        else
                        {
                            for (var rowIndex = 0; rowIndex < this.gridVagas.VisibleRowCount; rowIndex++)
                            {
                                bool linhaEncerrada = (bool)this.gridVagas.GetRowValues(rowIndex, "Encerrado");
                                if (!linhaEncerrada)
                                {
                                    idAgenda = Convert.ToInt32(this.gridVagas.GetRowValues(rowIndex, "AgendaID").ToString());
                                    break;
                                }
                            }

                            //VerificaPerfil se perfil pode salvar itens finalizados
                            parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, idAgenda);
                            if (parametroTurnoVaga.ParametroTurnoVagaId > 0)
                            {
                                if (parametroTurnoVaga.EditarVagaFinalizada)
                                {
                                    permiteSalvarParcial = true;
                                }
                            }
                        }
                    }

                    //Verifica se todas as linhas são encerradas
                    if (this.gridVagas.VisibleRowCount == encerrado)
                    {
                        permiteSalvarParcial = false;
                        permiteFinalizar = false;
                    }

                    //Verifica se não existe linha editavel
                    if (editavel == 0)
                    {
                        permiteSalvarParcial = false;
                        permiteFinalizar = false;
                    }
                }

                btnExcluirTurmasProvisorias.Visible = permiteSalvarParcial;
                pnGridVagas.Enabled = permiteSalvarParcial;
                pnTurmas.Visible = permiteSalvarParcial;
                btnSalvarParcialVagas.Visible = permiteSalvarParcial;
                btnSalvarDefinitivoVagas.Visible = permiteFinalizar;
                pnTurmas.Visible = (permiteSalvarParcial || permiteFinalizar);

                if ((txtFaixaInicial.Visible) && (this.gridVagas.VisibleRowCount != encerrado))
                {
                    btnExcluirTurmasProvisorias.Visible = true;
                    btnSalvarAnaliseVagas.Visible = true;
                    btnSalvarParcialVagas.Visible = false;
                }
                else
                {
                    btnSalvarAnaliseVagas.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
            }
        }

        #endregion

        #region Eventos DropDown

        protected void ddlTurmas_OnDataBound(object sender, EventArgs e)
        {
            var currentDropDown = ((DropDownList)sender);

            if (!currentDropDown.Items.Contains(new ListItem(TipoItem.Selecione.ToString(),
                                                ((int)TipoItem.Selecione).ToString())))
            {
                currentDropDown.Items.Insert(0, new ListItem(TipoItem.Selecione.ToString(),
                                                ((int)TipoItem.Selecione).ToString()));
            }

            var quantItensDropDown = currentDropDown.Items.Count;
            if (!currentDropDown.Items.Contains(new ListItem(TipoItem.Novo.ToString(),
                                                                              ((int)TipoItem.Novo).ToString())))
            {
                var podeTurmaProvisoria = Session["podeTurmaProvisoria"] == null ? true : Convert.ToBoolean(Session["podeTurmaProvisoria"]);
                if (podeTurmaProvisoria)
                {
                    currentDropDown.Items.Insert(quantItensDropDown, new ListItem(TipoItem.Novo.ToString(),
                                                                                  ((int)TipoItem.Novo).ToString()));
                }
            }
        }

        protected void txtJFNova_OnTextChanged(object sender, EventArgs e)
        {
            var currentTextBox = ((TextBox)sender);
            var indiceLinhaTabela = (((GridViewTableDataCell)(currentTextBox.Parent.Parent))).VisibleIndex;

            var idAgenda = (int)this.gridVagas.GetRowValues(indiceLinhaTabela, "IDAgenda");

            Confirmacao.Where(x => x.IDAgenda == idAgenda)
                .FirstOrDefault()
                    .JustificativaNova = currentTextBox.Text;

        }

        protected void ddlTurmas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var currentDropDown = ((DropDownList)sender);
                var selectItemText = currentDropDown.SelectedItem.Text;

                var indiceLinhaTabela = (((GridViewTableDataCell)(currentDropDown.Parent.Parent))).VisibleIndex;
                var nomeCompleto = String.Format("{0}_{1}", currentDropDown.ID, indiceLinhaTabela);//currentDropDown.ID + "_" + indiceLinhaTabela;
                var indexOfSelectedValue = GetIndexOfSelectedValue(currentDropDown.SelectedValue);

                var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();
                var turno = currentDropDown.ID.Split('_').Last();

                var indiceColunaTabela = turnos
                                            .Select((x, i) => i)
                                                .Where(i => turnos[i] == turno).FirstOrDefault() + 1;

                var nomeTextBox = String.Format("{0}_{1}", textBoxVCNome, turno);// "txtVC_" + turno;
                var currentTextBoxVC = (GetControleDaGridPorNome(indiceLinhaTabela, indiceColunaTabela, nomeTextBox) as TextBox);

                nomeTextBox = String.Format("{0}_{1}", textBoxVNNome, turno);// "txtVN_" + turno;
                var currentTextBoxVN = (GetControleDaGridPorNome(indiceLinhaTabela, indiceColunaTabela, nomeTextBox) as TextBox);

                var item = -1;

                if (this.Controle.Count > 0)
                {
                    item = this.Controle[nomeCompleto][0];
                }

                // se for diferente de -1 é porque está alterando o item de uma combo
                // portanto é necessário atualizar a grid de confirmação
                if (item != -1)
                {
                    MantemGridConfirmacao(indiceLinhaTabela, currentTextBoxVC.ID, 0, TipoVaga.Continuidade, Turmas[item].Turma);
                    MantemGridConfirmacao(indiceLinhaTabela, currentTextBoxVN.ID, 0, TipoVaga.Nova, Turmas[item].Turma);
                }

                // atualizando a troca de contexto de um item da combo por outro
                // atualiza as variáveis de controle e de valor do grid de Salas
                if (this.Controle.Count > 0)
                {
                    AtualizaDadosGridSalas(nomeCompleto, indexOfSelectedValue);
                    AtualizaCombosTurmas();
                }

                // caso esteja selecionado o "select" desabilito os textboxes correntes
                currentTextBoxVC.Enabled = currentTextBoxVN.Enabled = !(indexOfSelectedValue == -1);

                // em qualquer mudança de contexto zero os valores das textboxes
                currentTextBoxVC.Text = currentTextBoxVN.Text = "0";

                nomeCompleto = String.Format("hdnEditavel{0}", ((Turnos)(indiceColunaTabela)).ToString());
                var currentHiddenEditavel = GetControleDaGridPorNome(indiceLinhaTabela, indiceColunaTabela, nomeCompleto) as HiddenField;
                currentHiddenEditavel.Value = "true";

                if (selectItemText == "Novo")
                {
                    var turmaNovaComDados = (TceCtvTurmaProvisoria)Session["TurmaNovaComDados"];

                    if (turmaNovaComDados == null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupNovaTurma();", true);

                        Session["pcTurmaNova"] = new string[] { indiceLinhaTabela.ToString(), indiceColunaTabela.ToString(), currentDropDown.ID, turno, "false" };

                        this.LimparCamposTurmaNovaPopup();

                        ddlTurmaNovaPeriodo.DataSource = CarregaPeriodoTurmaNova(int.Parse(ddlAno.SelectedValue));
                        ddlTurmaNovaPeriodo.DataBind();
                    }
                    else
                    {
                        Session["currentDropDown"] = currentDropDown;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupNovaTurmaComDados();", true);
                    }

                }
                updatePanel12.Update();
                updatePanel2.Update();
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
            }
        }

        protected void ddlTurmaNovaPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTurmaNovaMensagem.Text = string.Empty;
            ddlTurmaNovaModalidade.Items.Clear();
            lblTurmaNovaSeriePrefixo.Text = string.Empty;
            lblTurmaNovaSetor.Text = string.Empty;
            ddlTurmaNovaSerie.Items.Clear();
            txtTurmaNova.Text = string.Empty;
            ddlTurmaNovaCurso.Items.Clear();

            if (!string.IsNullOrEmpty(ddlTurmaNovaPeriodo.SelectedValue) && ddlTurmaNovaPeriodo.SelectedValue != "-1")
            {
                ddlTurmaNovaModalidade.DataSource = CarregaModalidadesTurmaNova(tseUnidadeResponsavel.DBValue.ToString(), int.Parse(ddlAno.SelectedValue), int.Parse(ddlTurmaNovaPeriodo.SelectedValue), Convert.ToInt32(Session["codPerfil"]));
                ddlTurmaNovaModalidade.DataBind();
            }
        }

        protected void ddlTurmaNovaModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTurmaNovaMensagem.Text = string.Empty;
            lblTurmaNovaSeriePrefixo.Text = string.Empty;
            lblTurmaNovaSetor.Text = string.Empty;
            ddlTurmaNovaSerie.Items.Clear();
            txtTurmaNova.Text = string.Empty;
            ddlTurmaNovaCurso.Items.Clear();

            ddlTurmaNovaCurso.DataSource = CarregaCursosTurmaNova(tseUnidadeResponsavel.DBValue.ToString(), ddlTurmaNovaModalidade.SelectedValue, int.Parse(ddlAno.SelectedValue), int.Parse(ddlTurmaNovaPeriodo.SelectedValue), Convert.ToInt32(Session["codPerfil"]));
            ddlTurmaNovaCurso.DataBind();
        }

        protected void ddlTurmaNovaCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblTurmaNovaMensagem.Text = string.Empty;
                lblTurmaNovaSeriePrefixo.Text = string.Empty;
                lblTurmaNovaSetor.Text = string.Empty;
                ddlTurmaNovaSerie.Items.Clear();
                txtTurmaNova.Text = string.Empty;


                string ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? ddlAno.SelectedValue : null;

                string curso = ddlTurmaNovaCurso.SelectedValue != null
                                ? ddlTurmaNovaCurso.SelectedValue.ToString() != "Selecione"
                                      ? ddlTurmaNovaCurso.SelectedValue.ToString()
                                      : null
                                : null;
                string periodo = ddlTurmaNovaPeriodo.SelectedValue != "Selecione" ? ddlTurmaNovaPeriodo.SelectedValue : null;

                ddlTurmaNovaSerie.DataSource = CarregaSeriesTurmaNova(ano, curso, periodo);
                ddlTurmaNovaSerie.DataBind();

                if (tseUnidadeResponsavel != null)
                {
                    if (!tseUnidadeResponsavel.DBValue.IsNull)
                        lblTurmaNovaSetor.Text = "-" + tseUnidadeResponsavel["setor"].ToString();
                }

                lblTurmaNovaMensagem.Text = ddlTurmaNovaSerie.Items.Count == 1 ? "Não existe Matriz Curricular para este ano/periodo/curso/turno" : string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
            }
        }

        protected void ddlTurmaNovaSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTurmaNovaMensagem.Text = string.Empty;
            lblTurmaNovaSeriePrefixo.Text = string.Empty;

            if (!string.IsNullOrEmpty(ddlTurmaNovaSerie.SelectedValue))
            {
                if (ddlTurmaNovaSerie.SelectedValue != "Selecione" && ddlTurmaNovaSerie.SelectedValue != "-1")
                {
                    //recupera a serie
                    var serie = !string.IsNullOrEmpty(ddlTurmaNovaSerie.SelectedValue.Split('-')[0]) ? ddlTurmaNovaSerie.SelectedValue.ToString().Split('-')[0] : string.Empty;

                    int tamanhoRetirado = serie.Length + 1;
                    lblTurmaNovaSeriePrefixo.Text = ddlTurmaNovaSerie.SelectedValue.Substring(tamanhoRetirado, ddlTurmaNovaSerie.SelectedValue.Length - tamanhoRetirado);
                }
                else
                {
                    lblTurmaNovaSeriePrefixo.Text = string.Empty;
                    txtTurmaNova.Text = string.Empty;
                }
            }
        }

        protected void ddlTurmaNovaPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlTurmaNovaPeriodo.Items.Insert(0, new ListItem("Selecione", "-1"));
            ddlTurmaNovaPeriodo.SelectedIndex = 0;
        }

        protected void ddlTurmaNovaModalidade_DataBound(object sender, EventArgs e)
        {
            ddlTurmaNovaModalidade.Items.Insert(0, new ListItem("Selecione", "-1"));
            ddlTurmaNovaModalidade.SelectedIndex = 0;
        }

        protected void ddlTurmaNovaCurso_DataBound(object sender, EventArgs e)
        {
            ddlTurmaNovaCurso.Items.Insert(0, new ListItem("Selecione", "-1"));
            ddlTurmaNovaCurso.SelectedIndex = 0;
        }

        protected void ddlTurmaNovaSerie_DataBound(object sender, EventArgs e)
        {
            ddlTurmaNovaSerie.Items.Insert(0, new ListItem("Selecione", "-1"));
            ddlTurmaNovaSerie.SelectedIndex = 0;
        }

        #endregion

        #region Eventos Grid

        protected void gridVagas_onHtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var quantN = (int)this.gridVagas.GetRowValues(e.VisibleIndex, "QuantNovas");
                var quantNSeeduc = (int)this.gridVagas.GetRowValues(e.VisibleIndex, "QuantNovaSeeduc");
                var editavel = (bool)this.gridVagas.GetRowValues(e.VisibleIndex, "Editavel");
                var txtJustificativaNova = DevExpressHelper.GetControl<TextBox>(this.gridVagas, e.VisibleIndex, "JustificativaNova", "txtJFNova");
                bool ehIgualProposta = false;

                //Habilita / desabilita linha caso esteja editavel
                e.Row.Enabled = editavel;

                //VerificaPerfil se as linha é editavel
                if (editavel)
                {
                    //Verificar se quantidade lançada pela escola é igual a proposta SEEDUC
                    ehIgualProposta = (quantN == quantNSeeduc);

                    if (ehIgualProposta)
                    {
                        //Desabilita Coluna de justificativa Nova
                        txtJustificativaNova.BackColor = Color.FromName("Gainsboro");
                        txtJustificativaNova.Enabled = false;
                    }
                    else
                    {
                        //Habilita Coluna de justificativa Nova
                        txtJustificativaNova.BackColor = Color.FromName(string.Empty);
                        txtJustificativaNova.Enabled = true;
                    }
                }
                else
                {
                    //Desabilita Coluna de justificativa Nova
                    txtJustificativaNova.BackColor = Color.FromName("Gainsboro");
                }

                txtJustificativaNova.ToolTip = txtJustificativaNova.Text;
            }

            if (e.RowType == GridViewRowType.Header)
            {
                ASPxGridView HeaderGrid = (ASPxGridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Proposta SEEDUC";
                HeaderCell.ColumnSpan = 3;
                HeaderCell.BorderColor = Color.Red;
                HeaderGridRow.Cells.Add(HeaderCell);



                HeaderCell = new TableCell();
                HeaderCell.Text = "Proposta UE";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

            }
        }

        protected void gridSalas_onHtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                if ((IsPostBack && (Turmas != null) && (Turmas.Count > 0) && (Valores != null) && !(Valores.Count > 0))
                || (Request.QueryString["Chave"] != null) || (Session["TurmaProvisoriaNova"] != null && Convert.ToString(Session["TurmaProvisoriaNova"]) == "S"))
                {
                    if (e.RowType == GridViewRowType.Data)
                    {
                        var linhaIndice = e.VisibleIndex;

                        // caso turma esteja vazio: representa que aquela sala ainda nao teve associação de dados,
                        // então adiciono como vazio a célula para todos os turnos
                        if (String.IsNullOrEmpty(ListagemSalas[contador].Turma))
                        {
                            foreach (var turno in Enum.GetNames(typeof(Turnos)))
                            {
                                AdicionaCelulaSemDado(linhaIndice, turno);
                            }

                            contador++;
                        }
                        else
                        {
                            //confere para aquela turma, quais turnos tem associação de dados, os outros turnos também ficam vazios 
                            var turnosExistentes = ListagemSalas.Where(y => y.SalaCapacidade == ListagemSalas[contador].SalaCapacidade)
                                                                .Select(x => x.Turno)
                                                                    .ToList();

                            for (int i = 0; i < turnosExistentes.Count; i++)
                            {
                                //se M = Manha; T = Tarde; N = Noite; A = Ampliado; I = Integral
                                turnosExistentes[i] = GetNameFromTurno(turnosExistentes[i]);
                            }


                            var turnosNaoExistentes = GetDemaisTurnos(turnosExistentes);

                            foreach (var item in turnosExistentes)
                            {
                                AdicionaCelulaComDado(linhaIndice, ListagemSalas[contador], GetNameFromTurno(ListagemSalas[contador].Turno));
                                contador++;
                            }

                            foreach (var item in turnosNaoExistentes)
                            {
                                AdicionaCelulaSemDado(linhaIndice, item);
                            }
                        }
                    }

                    if (ListagemSalas != null && contador == ListagemSalas.Count && (Turmas.Count > 0))
                    {
                        contador = 0;
                        Valores.Clear();
                        ReMontaDataSources();

                        (this.Master.FindControl("scrMng") as ScriptManager)
                          .RegisterAsyncPostBackControl(btnSalvarTurmaNova);

                        (this.Master.FindControl("scrMng") as ScriptManager)
                        .RegisterAsyncPostBackControl(btnConfirmarVagasOfertadas);
                    }
                }

                if (IsPostBack && Turmas != null && (Turmas.Count == 0))
                {
                    if (e.RowType == GridViewRowType.Data)
                    {
                        var linhaIndice = e.VisibleIndex;

                        if (String.IsNullOrEmpty(ListagemSalas[contador].Turma))
                        {
                            foreach (var turno in Enum.GetNames(typeof(Turnos)))
                            {
                                AdicionaCelulaSemDado(linhaIndice, turno);
                            }

                            contador++;
                        }

                        if (ListagemSalas != null && contador == ListagemSalas.Count)
                        {
                            contador = 0;
                            Valores.Clear();
                            ReMontaDataSources();

                            (this.Master.FindControl("scrMng") as ScriptManager)
                              .RegisterAsyncPostBackControl(btnSalvarTurmaNova);

                            (this.Master.FindControl("scrMng") as ScriptManager)
                            .RegisterAsyncPostBackControl(btnConfirmarVagasOfertadas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ReMontaDataSources()
        {
            try
            {
                RN.CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
                int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);
                string censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                int ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
                string[] turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();
                IList<string> colunasHabilitadas;

                //confere se turno deve ser habilitada
                colunasHabilitadas = rnCtvConfTurno.ListaTurnosParaLancamentoVagasPor(censo, ano, tipoEventoConfirmacaoVagas);

                for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                {
                    for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
                    {
                        var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna][0]);
                        var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                        nomeControle = String.Format("{0}_{1}", nomeControle, indiceLinha);
                        var indicesTurmasPermitidas = GetIndicesTurmas(nomeControle);

                        Valores.Add(nomeControle, indicesTurmasPermitidas);

                        var nomeTurmasPermitidas = GetTurmasPorIndices(indicesTurmasPermitidas);
                        currentDropDown.DataSource = nomeTurmasPermitidas;
                        currentDropDown.DataBind();

                        var nome = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna][0]);
                        var currentTextBoxVC = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;

                        nome = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna][0]);
                        var currentTextBoxVN = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;

                        nome = String.Format("hdnEditavel{0}", ((Turnos)(indiceColuna + 1)).ToString());
                        var currentHiddenEditavel = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as HiddenField;

                        if (Controle[nomeControle][0] != -1)
                        {
                            currentDropDown.SelectedIndex = GetNewIndexSelected(nomeTurmasPermitidas, nomeControle);
                            currentDropDown.DataBind();
                            currentDropDown.SelectedValue = GetTurmasPorIndices(new List<int> { Controle[nomeControle][0] })[0].Turma;
                            currentTextBoxVC.Text = Controle[nomeControle][1].ToString();
                            currentTextBoxVN.Text = Controle[nomeControle][2].ToString();
                        }
                        else
                        {
                            currentTextBoxVC.Text = currentTextBoxVN.Text = "0";
                        }

                        if (!String.IsNullOrEmpty(currentHiddenEditavel.Value))
                        {
                            currentTextBoxVC.Enabled = currentTextBoxVN.Enabled = Convert.ToBoolean(currentHiddenEditavel.Value);
                        }
                        else
                        {
                            if (currentDropDown.SelectedItem.Value != "-1" && currentDropDown.SelectedItem.Value != "-2")
                                //if (Controle[nomeControle][0] != -1)
                                currentTextBoxVC.Enabled = currentTextBoxVN.Enabled = true;
                        }

                        //confere se turno deve ser habilitada
                        if (!colunasHabilitadas.Any(x1 => x1 == turnos[indiceColuna][0].ToString()))
                        {
                            currentDropDown.Enabled = false;
                            currentTextBoxVC.Enabled = false;
                            currentTextBoxVN.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int GetNewIndexSelected(List<DadosTurmaConfVaga> nomes, string nomeControle)
        {
            for (int i = 0; i < nomes.Count; i++)
                if (Turmas[Controle[nomeControle][0]].Turma == nomes[i].Turma)
                    return i;
            return -1;
        }

        private List<DadosTurmaConfVaga> GetTurmasPorIndices(List<int> indicesTurmasPermitidas)
        {
            var listaTurmas = new List<DadosTurmaConfVaga>();

            for (int i = 0; i < indicesTurmasPermitidas.Count; i++)
            {
                var turma = new DadosTurmaConfVaga
                {
                    Turma = Turmas[indicesTurmasPermitidas[i]].Turma,
                    DescricaoTurma = Turmas[indicesTurmasPermitidas[i]].DescricaoTurma
                };

                listaTurmas.Add(turma);
            }

            return listaTurmas;
        }

        private void AdicionaCelulaSemDado(int linhaIndice, string turno)
        {
            var nome = String.Format("{0}_{1}", comboTurmaNome, turno[0]);
            var currentDropDown = DevExpressHelper.GetControl<DropDownList>(this.gridSalas, linhaIndice, turno, nome);

            if (currentDropDown != null)
            {

                //currentDropDown.DataSource = Turmas;

                nome = String.Format("{0}_{1}", nome, linhaIndice);

                if (Valores.Count(x => x.Key == nome) == 0)
                {
                    Controle.Add(nome, new int[] { -1, 0, 0 });

                    nome = String.Format("hdnEditavel{0}", turno);
                    var currentHiddenEditavel = DevExpressHelper.GetControl<HiddenField>(this.gridSalas, linhaIndice, turno, nome);
                    currentHiddenEditavel.Value = "false";

                    (this.Master.FindControl("scrMng") as ScriptManager)
                        .RegisterAsyncPostBackControl(currentDropDown);
                }
            }
        }

        private void AdicionaCelulaComDado(int linhaIndice, DadosConfVaga listagem, string turno)
        {
            var nome = String.Format("{0}_{1}", textBoxVCNome, turno[0]);
            var currentTextBoxVC = DevExpressHelper.GetControl<TextBox>(this.gridSalas, linhaIndice, turno, nome);
            currentTextBoxVC.Text = listagem.VagasContinuidade.ToString();
            currentTextBoxVC.Enabled = listagem.Editavel;

            nome = String.Format("{0}_{1}", textBoxVNNome, turno[0]);
            var currentTextBoxVN = DevExpressHelper.GetControl<TextBox>(this.gridSalas, linhaIndice, turno, nome);
            currentTextBoxVN.Text = listagem.VagasNova.ToString();
            currentTextBoxVN.Enabled = listagem.Editavel;

            if (!listagem.Editavel)
            {
                var nomeLabel = String.Format("{0}_{1}", labelVCNome, turno[0]);
                var currentLabelVC = DevExpressHelper.GetControl<Label>(this.gridSalas, linhaIndice, turno, nomeLabel);
                nomeLabel = String.Format("{0}_{1}", labelVNNome, turno[0]);
                var currentLabelVN = DevExpressHelper.GetControl<Label>(this.gridSalas, linhaIndice, turno, nomeLabel);

                currentLabelVC.Text = "Vagas";
                currentTextBoxVC.Text = listagem.NumAlunos.ToString();
                currentLabelVN.Visible = false;
                currentTextBoxVN.Visible = false;
            }

            nome = String.Format("hdnID{0}", turno);
            var currentHiddenID = DevExpressHelper.GetControl<HiddenField>(this.gridSalas, linhaIndice, turno, nome);
            currentHiddenID.Value = listagem.IdCtvConfVaga.ToString();

            nome = String.Format("hdnEditavel{0}", turno);
            var currentHiddenEditavel = DevExpressHelper.GetControl<HiddenField>(this.gridSalas, linhaIndice, turno, nome);
            currentHiddenEditavel.Value = listagem.Editavel.ToString();

            nome = String.Format("hdnTurmaFilha{0}", turno);
            var currentHiddenTurmaFilha = DevExpressHelper.GetControl<HiddenField>(this.gridSalas, linhaIndice, turno, nome);
            currentHiddenTurmaFilha.Value = listagem.Turma;

            nome = String.Format("{0}_{1}", comboTurmaNome, turno[0]);
            var currentDropDown = DevExpressHelper.GetControl<DropDownList>(this.gridSalas, linhaIndice, turno, nome);
            currentDropDown.Enabled = listagem.Editavel;


            var index = GetIndexOfSelectedValue(listagem.Turma);
            IndicesUtilizados.Add(index);

            nome = String.Format("{0}_{1}", nome, linhaIndice);

            if (Valores.Count(x => x.Key == nome) == 0)
            {

                //Verifica se aquele controle já exisite
                if (!Controle.ContainsKey(nome))
                {
                    Controle.Add(nome, new int[] { index, Convert.ToInt32(currentTextBoxVC.Text), Convert.ToInt32(currentTextBoxVN.Text) });
                }
                (this.Master.FindControl("scrMng") as ScriptManager).RegisterAsyncPostBackControl(currentDropDown);
            }
        }

        #endregion

        #region Eventos TextBox

        protected void txtVN_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                var currentTextBox = ((TextBox)sender);
                Int32 result;

                //confiro se ele apagou o valor e atribuo zero ao campo
                if (string.IsNullOrEmpty(currentTextBox.Text))
                    currentTextBox.Text = "0";

                if (!Int32.TryParse(currentTextBox.Text, out result))
                    currentTextBox.Text = "0";

                var currentTextBoxID = currentTextBox.ID;
                var quantidadeAlunos = Convert.ToInt32(currentTextBox.Text);
                var tipoVaga = TipoVaga.Nova;
                var indiceLinhaTabela = (((GridViewTableDataCell)(currentTextBox.Parent.Parent))).VisibleIndex;

                if (quantidadeAlunos < 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('O valor de vagas novas não pode ser menor do que 0.');", true);
                }

                // necessário atualizar a grid de confirmação
                MantemGridConfirmacao(indiceLinhaTabela, currentTextBoxID, quantidadeAlunos, tipoVaga, null);
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
            }
        }

        protected void txtVC_OnTextChanged(object sender, EventArgs e)
        {
            try
            {

                var currentTextBox = ((TextBox)sender);
                Int32 result;

                //confiro se ele apagou qualquer valor e atribuo zero ao campo
                if (string.IsNullOrEmpty(currentTextBox.Text))
                {
                    currentTextBox.Text = "0";
                }

                if (!Int32.TryParse(currentTextBox.Text, out result))
                    currentTextBox.Text = "0";

                var currentTextBoxID = currentTextBox.ID;
                var quantidadeAlunos = Convert.ToInt32(currentTextBox.Text);
                var tipoVaga = TipoVaga.Continuidade;
                var indiceLinhaTabela = (((GridViewTableDataCell)(currentTextBox.Parent.Parent))).VisibleIndex;

                if (quantidadeAlunos < 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('O valor de vagas de continuidade não pode ser menor do que 0.');", true);
                }

                // necessário atualizar a grid de confirmação
                this.MantemGridConfirmacao(indiceLinhaTabela, currentTextBoxID, quantidadeAlunos, tipoVaga, null);
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
            }
        }

        #endregion

        #region Eventos Botao
        protected void btnConfirmaTurmaNovaComDados_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Curso rnCurso = new Curso();
                var currentDropDown = ((DropDownList)Session["currentDropDown"]);
                var selectItemText = currentDropDown.SelectedItem.Text;

                var indiceLinhaTabela = (((GridViewTableDataCell)(currentDropDown.Parent.Parent))).VisibleIndex;
                var nomeCompleto = String.Format("{0}_{1}", currentDropDown.ID, indiceLinhaTabela);//currentDropDown.ID + "_" + indiceLinhaTabela;

                var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();
                var turno = currentDropDown.ID.Split('_').Last();

                var indiceColunaTabela = turnos
                                            .Select((x, i) => i)
                                                .Where(i => turnos[i] == turno).FirstOrDefault() + 1;

                var novaTurmaComDados = (TceCtvTurmaProvisoria)Session["TurmaNovaComDados"];

                ddlTurmaNovaPeriodo.DataSource = CarregaPeriodoTurmaNova(int.Parse(ddlAno.SelectedValue));
                ddlTurmaNovaPeriodo.DataBind();
                ddlTurmaNovaPeriodo.SelectedValue = novaTurmaComDados.Periodo.ToString();

                ddlTurmaNovaModalidade.DataSource = CarregaModalidadesTurmaNova(tseUnidadeResponsavel.DBValue.ToString(), int.Parse(ddlAno.SelectedValue), novaTurmaComDados.Periodo, Convert.ToInt32(Session["codPerfil"]));
                ddlTurmaNovaModalidade.DataBind();
                ddlTurmaNovaModalidade.SelectedValue = rnCurso.ObtemModalidadePor(int.Parse(ddlAno.SelectedValue), novaTurmaComDados.Periodo, tseUnidadeResponsavel.DBValue.ToString(), novaTurmaComDados.Curso);

                ddlTurmaNovaCurso.DataSource = CarregaCursosTurmaNova(tseUnidadeResponsavel.DBValue.ToString(), ddlTurmaNovaModalidade.SelectedValue, int.Parse(ddlAno.SelectedValue), novaTurmaComDados.Periodo, Convert.ToInt32(Session["codPerfil"]));
                ddlTurmaNovaCurso.DataBind();
                ddlTurmaNovaCurso.SelectedValue = novaTurmaComDados.Curso;

                ddlTurmaNovaSerie.DataSource = CarregaSeriesTurmaNova(ddlAno.SelectedValue, novaTurmaComDados.Curso, novaTurmaComDados.Periodo.ToString());
                ddlTurmaNovaSerie.DataBind();

                if (tseUnidadeResponsavel != null)
                {
                    if (!tseUnidadeResponsavel.DBValue.IsNull)
                        lblTurmaNovaSetor.Text = "-" + tseUnidadeResponsavel["setor"].ToString();
                }

                lblTurmaNovaMensagem.Text = ddlTurmaNovaSerie.Items.Count == 1 ? "Não existe Matriz Curricular para este ano/periodo/curso/turno" : string.Empty;


                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupNovaTurma();", true);

                Session["pcTurmaNova"] = new string[] { indiceLinhaTabela.ToString(), indiceColunaTabela.ToString(), currentDropDown.ID, turno, "false" };

                updatePanel12.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNaoConfirmaTurmaNovaComDados_Click(object sender, EventArgs e)
        {
            try
            {
                var currentDropDown = ((DropDownList)Session["currentDropDown"]);
                var selectItemText = currentDropDown.SelectedItem.Text;

                var indiceLinhaTabela = (((GridViewTableDataCell)(currentDropDown.Parent.Parent))).VisibleIndex;
                var nomeCompleto = String.Format("{0}_{1}", currentDropDown.ID, indiceLinhaTabela);//currentDropDown.ID + "_" + indiceLinhaTabela;

                var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();
                var turno = currentDropDown.ID.Split('_').Last();

                var indiceColunaTabela = turnos
                                            .Select((x, i) => i)
                                                .Where(i => turnos[i] == turno).FirstOrDefault() + 1;


                LimparCamposTurmaNovaPopup();

                ddlTurmaNovaPeriodo.DataSource = CarregaPeriodoTurmaNova(int.Parse(ddlAno.SelectedValue));
                ddlTurmaNovaPeriodo.DataBind();

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupNovaTurmaComDados();abrirPopupNovaTurma();", true);

                Session["pcTurmaNova"] = new string[] { indiceLinhaTabela.ToString(), indiceColunaTabela.ToString(), currentDropDown.ID, turno, "false" };

                updatePanel12.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnSalvarTurmaNova_OnClick(object sender, EventArgs e)
        {
            try
            {
                int codPerfil = -1;
                int totalSalasDisponiveis = 0;
                int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);
                RN.CtvConfVaga rnCtvConfVaga = new CtvConfVaga();
                CtvTurmaProvisoria rnCtvTurmaProvisoria = new CtvTurmaProvisoria();
                CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
                Int32 result;

                this.VerificaPerfil();

                if (!string.IsNullOrEmpty(Session["codPerfil"].ToString()))
                {
                    codPerfil = Convert.ToInt32(Session["codPerfil"]);
                }

                var ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
                var curso = ddlTurmaNovaCurso.SelectedValue != null && ddlTurmaNovaCurso.SelectedValue.ToString() != "Selecione" && ddlTurmaNovaCurso.SelectedValue.ToString() != "-1" ? ddlTurmaNovaCurso.SelectedValue.ToString() : null;
                var serie = !string.IsNullOrEmpty(ddlTurmaNovaSerie.SelectedValue) && ddlTurmaNovaSerie.SelectedValue != null && ddlTurmaNovaSerie.SelectedValue.ToString() != "Selecione" && ddlTurmaNovaSerie.SelectedValue.ToString() != "-1" ? int.Parse(ddlTurmaNovaSerie.SelectedValue.ToString().Split('-')[0]) : -1;
                int periodo = !string.IsNullOrEmpty(ddlTurmaNovaPeriodo.SelectedValue) && ddlTurmaNovaPeriodo.SelectedValue != "Selecione" && ddlTurmaNovaPeriodo.SelectedValue != "-1" ? int.Parse(ddlTurmaNovaPeriodo.SelectedValue) : -1;

                var idAgenda = RN.CtvAgendaConfTurnoVaga.RetornaIdAgenda(ano, periodo, curso, serie);

                var turmaProvisoria = new TceCtvTurmaProvisoria
                {
                    Ano = ano,
                    Periodo = periodo,
                    Curso = curso,
                    Serie = serie,
                    Censo = Convert.ToString(tseUnidadeResponsavel.DBValue),
                    Turma = !string.IsNullOrEmpty(txtTurmaNova.Text) ? lblTurmaNovaSeriePrefixo.Text + txtTurmaNova.Text.Trim() + lblTurmaNovaSetor.Text : null,
                    Matricula = User.Identity.Name
                };

                if (!string.IsNullOrEmpty(txtTurmaNova.Text.Trim()))
                {
                    if (lblTurmaNovaSetor.Text == "-")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('A UA da escola não foi encontrada.');", true);
                        return;
                    }

                    if (!Int32.TryParse(txtTurmaNova.Text, out result))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('O campo turma só pode ter números.');", true);
                        return;
                    }

                    if (int.Parse(txtTurmaNova.Text) < 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "alert('O valor da turma não pode ser menor do que 0.');", true);
                        return;
                    }
                }

                totalSalasDisponiveis = gridSalas.VisibleRowCount * rnCtvConfTurno.ListaTurnosParaLancamentoVagasPor(Convert.ToString(tseUnidadeResponsavel.DBValue), ano, tipoEventoConfirmacaoVagas).Count();

                var validacao = rnCtvTurmaProvisoria.ValidaTurmaProvisoria(turmaProvisoria, idAgenda, codPerfil, totalSalasDisponiveis);

                if (validacao.Valido)
                {
                    RN.CtvTurmaProvisoria.Inserir(turmaProvisoria);

                    this.LimparCamposTurmaNovaPopup();

                    List<int> periodos = ObtemPeriodosVigentes();

                    this.Turmas = rnCtvConfVaga.ListaTurmasParaLancamentoPor(turmaProvisoria.Censo, ano, periodos, (List<string>)Session["CursosNaoParticipamVagas"]);

                    var sessao = Session["pcTurmaNova"] as string[];

                    var linha = Convert.ToInt32(sessao[0]);
                    var coluna = Convert.ToInt32(sessao[1]);
                    var nomeControle = String.Format("{0}_{1}", sessao[2], sessao[0]);
                    var turno = sessao[3];

                    var listagem = rnCtvConfVaga.ListaQuadroDeSalasPor(turmaProvisoria.Censo, ano, codPerfil, Session["perfil"].ToString(), tipoEventoConfirmacaoVagas, periodos, (List<string>)Session["CursosNaoParticipamVagas"])
                         .OrderBy(x => x.SalaCapacidade).ToList();

                    var listagemAgrupadaPorSala = listagem.GroupBy(x => x.SalaCapacidade).ToList()
                            .Select(x => x.First()).ToList();

                    var index = GetIndexOfSelectedValue(turmaProvisoria.Turma);
                    AtualizaControleValores(index, nomeControle);

                    gridSalas.DataSource = listagemAgrupadaPorSala;

                    var dataSource = ListTurmasPorCombo(nomeControle);

                    nomeControle = sessao[2];
                    var currentDropDown = GetControleDaGridPorNome(linha, coluna, nomeControle) as DropDownList;
                    currentDropDown.DataSource = dataSource;
                    currentDropDown.SelectedIndex = GetIndiceItemSelecionadoCombo(dataSource, turmaProvisoria.Turma);
                    currentDropDown.DataBind();
                    Session["TurmaProvisoriaNova"] = "S";

                    gridSalas.DataBind();

                    Session["TurmaProvisoriaNova"] = "N";

                    Valores.Clear();
                    this.ReMontaDataSources();

                    sessao = null;

                    Session["TurmaNovaComDados"] = turmaProvisoria;
                    lblMensagemVagas.Text = "Turma incluída com sucesso.";


                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupNovaTurma();", true);
                }
                else
                {
                    lblTurmaNovaMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;

                updatePanel4.Update();
                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblTurmaNovaMensagem.Text = ex.Message;
            }
        }

        protected void hplLink_Click(object sender, EventArgs e)
        {
            string queryString = string.Empty;

            if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !this.tseUnidadeResponsavel.DBValue.IsNull)
            {
                queryString = string.Format("ano={0}&unidadeEnsino={1}", ddlAno.SelectedValue, tseUnidadeResponsavel.DBValue.ToString());
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                Response.Redirect("HistoricoConfirmacaoTurnosVagas.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            else
            {
                Response.Redirect("HistoricoConfirmacaoTurnosVagas.aspx");
            }
        }

        private void SalvaVagas()
        {
            this.lblMensagemVagas.Text = string.Empty;
            bool finalizado;
            bool encerrado;

            try
            {
                int contFinalizado = 0;
                for (var rowIndex = 0; rowIndex < this.gridVagas.VisibleRowCount; rowIndex++)
                {
                    finalizado = (bool)this.gridVagas.GetRowValues(rowIndex, "Finalizado");
                    encerrado = (bool)this.gridVagas.GetRowValues(rowIndex, "Encerrado");

                    //Verifica se existe lançamentos ativos já finalizados
                    if (finalizado && !encerrado)
                    {
                        contFinalizado = contFinalizado + 1;
                    }
                }

                if (contFinalizado > 0)
                {
                    SalvaVagasFinalizadas();
                }
                else
                {
                    SalvaVagasParcialmente();
                }

                VerificaInconsistenciaSalaVaga(Convert.ToInt32(ddlAno.SelectedValue),
                                               tseUnidadeResponsavel.DBValue.ToString());
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvarAnaliseVagas_Click(object sender, EventArgs e)
        {
            CtvAnalise rnCtvAnalise = new CtvAnalise();
            ValidacaoDados validacaoAnalises = new ValidacaoDados();
            List<TceCtvAnalise> analises = new List<TceCtvAnalise>();
            string perfilAnalise = string.Empty;

            try
            {
                perfilAnalise = ddlPerfil.SelectedItem.Text;

                //Monta lista de analises de vagas
                analises = this.MontaAnalisesVagaPor(perfilAnalise);

                validacaoAnalises = rnCtvAnalise.ValidaListaAnalises(analises, perfilAnalise);

                if (validacaoAnalises.Valido)
                {
                    SalvaVagas();
                }
                else
                {
                    lblMensagemVagas.Text = "Erro na validação dos dados: " + validacaoAnalises.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvarParcialVagas_OnClick(object sender, EventArgs e)
        {
            try
            {
                SalvaVagas();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private bool SalvaVagasParcialmente()
        {
            bool finalizar = false; //Salvar parcial
            bool podeSalvar = false;
            int idAgenda = 0;
            RN.Agenda.Evento rnEvento = new Techne.Lyceum.RN.Agenda.Evento();
            DateTime dataFimAgenda = DateTime.MinValue;
            RN.CtvConfVaga rnCtvConfVaga = new CtvConfVaga();
            hdnOrigem.Value = "Parcial";
            RN.TurnosVagas.HistoricoVaga rnHistoricoVaga = new RN.TurnosVagas.HistoricoVaga();
            CtvAnalise rnCtvAnalise = new CtvAnalise();
            List<TceCtvAnalise> analises = new List<TceCtvAnalise>();
            string perfilAnalise = string.Empty;
            string censo = Convert.ToString(tseUnidadeResponsavel.DBValue);
            var ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
            bool salvaAnalise = btnSalvarAnaliseVagas.Visible;
            List<int> periodos = new List<int>();
            try
            {
                podeSalvar = this.ValidaChoqueTurnoIntegral();

                if (podeSalvar)
                {
                    //Verifica se esta em modo de analise
                    if (salvaAnalise)
                    {
                        periodos = ObtemPeriodosVigentes().Distinct().ToList();

                        rnHistoricoVaga.SalvaHistoricoVagaDiretor(ano, periodos, censo);
                    }

                    podeSalvar = this.SalvarConfirmacaoVagasDaGrid(finalizar);

                    if (podeSalvar)
                    {
                        this.SalvarJustificativasVagas();

                        if (btnSalvarParcialVagas.Visible)
                        {
                            idAgenda = ObtemAgendaIdVagas();
                            dataFimAgenda = rnEvento.ObtemDataFimPor(Convert.ToInt32(ddlAno.SelectedValue), idAgenda, Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas), Convert.ToInt32(Session["codPerfil"]));

                            if (dataFimAgenda != DateTime.MinValue && dataFimAgenda != null)
                            {
                                lblMensagemDataFimVagas.Text = dataFimAgenda.ToString("dd/MM/yyyy");
                            }

                            gridVagas.DataBind();
                            lblMensagemVagas.Text = "Salvo parcialmente com sucesso.";

                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupVagas();", true);
                        }
                        if (!VerificaConsistenciaAlocacao())
                        {
                            rnCtvConfVaga.RemoveDuplicidadeLancamentoTurmaPor(int.Parse(ddlAno.SelectedValue), Convert.ToString(tseUnidadeResponsavel.DBValue));
                            //throw new Exception("DuplicidadeLancamentoTurma");
                        }

                        updatePanel5.Update();
                        updatePanel3.Update();
                    }
                    else
                    {
                        if (!VerificaConsistenciaAlocacao())
                        {
                            //                            throw new Exception("DuplicidadeLancamentoTurma");
                            rnCtvConfVaga.RemoveDuplicidadeLancamentoTurmaPor(int.Parse(ddlAno.SelectedValue), Convert.ToString(tseUnidadeResponsavel.DBValue));
                        }
                    }
                }

                //Verifica se pode salvar e se esta em modo de analise
                if (podeSalvar && salvaAnalise)
                {
                    perfilAnalise = ddlPerfil.SelectedItem.Text;

                    //Monta lista de analises de vagas
                    analises = this.MontaAnalisesVagaPor(perfilAnalise);

                    periodos = ObtemPeriodosVigentes().Distinct().ToList();
                    rnHistoricoVaga.SalvaHistoricoVagaSeeduc(ano, periodos, censo);

                    //Salva analises
                    rnCtvAnalise.Salva(analises, perfilAnalise);

                    gridVagas.DataBind();
                    lblMensagemVagas.Text = "Salvo com sucesso.";
                    lblMensagemVagasBottom.Text = lblMensagemVagas.Text;

                }

                return podeSalvar;
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = ex.Message;
                lblMensagem.Text = ex.Message;
                return podeSalvar;
            }
        }

        public List<TceCtvAnalise> MontaAnalisesVagaPor(string perfilAnalise)
        {
            List<TceCtvAnalise> analises = new List<TceCtvAnalise>();
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;

            ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;

            if (perfilAnalise == "SUPED")
            {
                for (int periodo = 0; periodo <= 2; periodo++)
                {
                    DropDownList ddlResultadoSUPED = MainContent.FindControl("ddlResultadoVagasSUPEDPeriodo" + periodo) as DropDownList;
                    TceCtvAnalise analise = new TceCtvAnalise();
                    Label lblAnaliseSuped = MainContent.FindControl("lblAnaliseVagasSUPED" + periodo) as Label;

                    if (periodo == 1)
                    {
                        ddlResultadoSUPED = MainContent.FindControl("ddlResultadoVagasSUPEDPeriodo0") as DropDownList;
                        lblAnaliseSuped = MainContent.FindControl("lblAnaliseVagasSUPED0") as Label;
                    }


                    if (ddlResultadoSUPED.Visible && lblAnaliseSuped.Text.Contains(" " + periodo.ToString()))
                    {
                        analise.Censo = tseUnidadeResponsavel.DBValue.ToString();
                        analise.Periodo = periodo;
                        analise.Ano = int.Parse(ddlAno.SelectedValue);
                        analise.Vaga = true;
                        analise.MatriculaSuped = User.Identity.Name;
                        analise.DtAnaliseSuped = DateTime.Now;
                        analise.AnaliseSuped = ddlResultadoSUPED.SelectedValue;

                        analises.Add(analise);
                    }
                }
            }

            if (perfilAnalise == "SUPLAN")
            {
                for (int periodo = 0; periodo <= 2; periodo++)
                {
                    DropDownList ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoVagasSUPLANPeriodo" + periodo) as DropDownList;
                    TceCtvAnalise analise = new TceCtvAnalise();

                    Label lblAnaliseSuplan = MainContent.FindControl("lblAnaliseVagasSUPLAN" + periodo) as Label;
                    if (periodo == 1)
                    {
                        lblAnaliseSuplan = MainContent.FindControl("lblAnaliseVagasSUPLAN0") as Label;
                        ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoVagasSUPLANPeriodo0") as DropDownList;
                    }

                    if (ddlResultadoSUPLAN.Visible && lblAnaliseSuplan.Text.Contains(" " + periodo.ToString()))
                    {
                        analise.Censo = tseUnidadeResponsavel.DBValue.ToString();
                        analise.Periodo = periodo;
                        analise.Ano = int.Parse(ddlAno.SelectedValue);
                        analise.Vaga = true;
                        analise.MatriculaSuplan = User.Identity.Name;
                        analise.DtAnaliseSuplan = DateTime.Now;
                        analise.AnaliseSuplan = ddlResultadoSUPLAN.SelectedValue;

                        analises.Add(analise);
                    }
                }

            }
            if (perfilAnalise == "DIESP")
            {
                for (int periodo = 0; periodo <= 2; periodo++)
                {
                    DropDownList ddlResultadoDIESP = MainContent.FindControl("ddlResultadoVagasDIESPPeriodo" + periodo) as DropDownList;
                    TceCtvAnalise analise = new TceCtvAnalise();
                    Label lblAnaliseDiesp = MainContent.FindControl("lblAnaliseVagasDIESP" + periodo) as Label;

                    if (periodo == 1)
                    {
                        ddlResultadoDIESP = MainContent.FindControl("ddlResultadoVagasDIESPPeriodo0") as DropDownList;
                        lblAnaliseDiesp = MainContent.FindControl("lblAnaliseVagasDIESP0") as Label;
                    }

                    if (ddlResultadoDIESP.Visible && lblAnaliseDiesp.Text.Contains(" " + periodo.ToString()))
                    {
                        analise.Censo = tseUnidadeResponsavel.DBValue.ToString();
                        analise.Periodo = periodo;
                        analise.Ano = int.Parse(ddlAno.SelectedValue);
                        analise.Vaga = true;
                        analise.MatriculaDiesp = User.Identity.Name;
                        analise.DataAnaliseDiesp = DateTime.Now;
                        analise.AnaliseDiesp = ddlResultadoDIESP.SelectedValue;

                        analises.Add(analise);
                    }
                }
            }

            return analises;
        }

        public List<TceCtvAnalise> MontaAnalisesTurnosPor(string perfilAnalise)
        {
            List<TceCtvAnalise> analises = new List<TceCtvAnalise>();
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("cphFormulario") as ContentPlaceHolder;

            try
            {
                if (perfilAnalise == "SUPED")
                {
                    for (int periodo = 0; periodo <= 2; periodo++)
                    {
                        DropDownList ddlResultadoSUPED = MainContent.FindControl("ddlResultadoTurnosSUPEDPeriodo" + periodo) as DropDownList;
                        TceCtvAnalise analise = new TceCtvAnalise();
                        Label lblAnaliseSuped = MainContent.FindControl("lblAnaliseTurnosSUPED" + periodo) as Label;
                        if (periodo == 1)
                        {
                            ddlResultadoSUPED = MainContent.FindControl("ddlResultadoTurnosSUPEDPeriodo0") as DropDownList;
                            lblAnaliseSuped = MainContent.FindControl("lblAnaliseTurnosSUPED0") as Label;
                        }

                        if (ddlResultadoSUPED.Visible && lblAnaliseSuped.Text.Contains(" " + periodo.ToString()))
                        {
                            analise.Censo = tseUnidadeResponsavel.DBValue.ToString();
                            analise.Periodo = periodo;
                            analise.Ano = int.Parse(ddlAno.SelectedValue);
                            analise.Turno = true;
                            analise.MatriculaSuped = User.Identity.Name;
                            analise.DtAnaliseSuped = DateTime.Now;
                            analise.AnaliseSuped = ddlResultadoSUPED.SelectedValue;

                            analises.Add(analise);
                        }

                    }
                }

                if (perfilAnalise == "SUPLAN")
                {
                    for (int periodo = 0; periodo <= 2; periodo++)
                    {
                        TceCtvAnalise analise = new TceCtvAnalise();
                        DropDownList ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoTurnosSUPLANPeriodo" + periodo) as DropDownList;
                        Label lblAnaliseSuplan = MainContent.FindControl("lblAnaliseTurnosSUPLAN" + periodo) as Label;

                        if (periodo == 1)
                        {
                            ddlResultadoSUPLAN = MainContent.FindControl("ddlResultadoTurnosSUPLANPeriodo0") as DropDownList;
                            lblAnaliseSuplan = MainContent.FindControl("lblAnaliseTurnosSUPLAN0") as Label;
                        }

                        if (ddlResultadoSUPLAN.Visible && lblAnaliseSuplan.Text.Contains(" " + periodo.ToString()))
                        {
                            analise.Censo = tseUnidadeResponsavel.DBValue.ToString();
                            analise.Periodo = periodo;
                            analise.Ano = int.Parse(ddlAno.SelectedValue);
                            analise.Turno = true;
                            analise.MatriculaSuplan = User.Identity.Name;
                            analise.DtAnaliseSuplan = DateTime.Now;
                            analise.AnaliseSuplan = ddlResultadoSUPLAN.SelectedValue;

                            analises.Add(analise);

                        }
                    }

                }
                if (perfilAnalise == "DIESP")
                {
                    for (int periodo = 0; periodo <= 2; periodo++)
                    {
                        TceCtvAnalise analise = new TceCtvAnalise();
                        DropDownList ddlResultadoDIESP = MainContent.FindControl("ddlResultadoTurnosDIESPPeriodo" + periodo) as DropDownList;
                        Label lblAnaliseDiesp = MainContent.FindControl("lblAnaliseTurnosDIESP" + periodo) as Label;

                        if (periodo == 1)
                        {
                            ddlResultadoDIESP = MainContent.FindControl("ddlResultadoTurnosDIESPPeriodo0") as DropDownList;
                            lblAnaliseDiesp = MainContent.FindControl("lblAnaliseTurnosDIESP0") as Label;
                        }
                        if (ddlResultadoDIESP.Visible && lblAnaliseDiesp.Text.Contains(" " + periodo.ToString()))
                        {
                            analise.Censo = tseUnidadeResponsavel.DBValue.ToString();
                            analise.Periodo = periodo;
                            analise.Ano = int.Parse(ddlAno.SelectedValue);
                            analise.Turno = true;
                            analise.MatriculaDiesp = User.Identity.Name;
                            analise.DataAnaliseDiesp = DateTime.Now;
                            analise.AnaliseDiesp = ddlResultadoDIESP.SelectedValue;

                            analises.Add(analise);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return analises;
        }

        protected void btnConfirmarVagasPopup_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupVagas(); fecharPopupConfirmacaoValidacaoVagas();", true);

            if (string.IsNullOrEmpty(lblMensagemVagas.Text))
            {
                lblMensagemVagas.Text = Convert.ToString(Session["MensagemInconsistencia"]);
            }

            lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
            lblMensagemVagas.Text = lblMensagemVagas.Text;
            updatePanel3.Update();
            updatePanel5.Update();
        }

        protected void btnSalvarDefinitivoVagas_OnClick(object sender, EventArgs e)
        {
            this.lblMensagemVagas.Text = string.Empty;
            SalvaVagasFinalizadas();
        }

        private bool SalvaVagasFinalizadas()
        {
            bool finalizar = true; //Finalizar
            bool podeSalvar = false;
            hdnOrigem.Value = "Definitivo";

            try
            {
                //Verifica preenchimento da justificativa
                podeSalvar = ValidarJustificativa();

                if (podeSalvar)
                {
                    podeSalvar = ValidarPropostaSeeduc(finalizar);

                    if (podeSalvar)
                    {
                        this.GerarPopupVagasOfertadas();
                    }
                }

                return podeSalvar;
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                return podeSalvar;
            }
        }

        protected void btnConfirmarVagasOfertadas_OnClick(object sender, EventArgs e)
        {
            try
            {
                bool finalizar = true; //Botao Finalizar
                RN.CtvConfVaga rnCtvConfVaga = new RN.CtvConfVaga();
                int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);
                bool podeSalvar = false;
                RN.TurnosVagas.HistoricoVaga rnHistoricoVaga = new RN.TurnosVagas.HistoricoVaga();
                CtvAnalise rnCtvAnalise = new CtvAnalise();
                List<TceCtvAnalise> analises = new List<TceCtvAnalise>();
                string perfilAnalise = string.Empty;
                string censo = Convert.ToString(tseUnidadeResponsavel.DBValue);
                var ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
                bool salvaAnalise = btnSalvarAnaliseVagas.Visible;
                List<int> periodos = new List<int>();

                //Verifica se esta em modo de analise
                if (salvaAnalise)
                {
                    periodos = ObtemPeriodosVigentes().Distinct().ToList();
                    rnHistoricoVaga.SalvaHistoricoVagaDiretor(ano, periodos, censo);
                }

                //Valida e caso passe pela validação grava
                podeSalvar = this.ValidaChoqueTurnoIntegral();

                if (podeSalvar)
                {
                    podeSalvar = SalvarConfirmacaoVagasDaGrid(finalizar);

                    if (podeSalvar)
                    {
                        SalvarJustificativasVagas();
                        lblMensagemVagas.Text = "Finalizado com sucesso. ";

                        FinalizarLancamento();
                        AtualizaGridSalasPosSalvamento();

                        string periodosAbertos = rnCtvConfVaga.ObtemPeriodosAbertosPor(tseUnidadeResponsavel.DBValue.ToString(), int.Parse(ddlAno.SelectedValue), tipoEventoConfirmacaoVagas);

                        if (!string.IsNullOrEmpty(periodosAbertos))
                        {
                            string MensagemPeriodoEmAberto = " Lançamento do(s) Período(s) " + periodosAbertos + " ainda em aberto. Favor efetuar o lançamento.";
                            lblMensagemVagas.Text = lblMensagemVagas.Text + "</br>" + MensagemPeriodoEmAberto;
                        }

                        rnCtvConfVaga.RemoveDuplicidadeLancamentoTurmaPor(int.Parse(ddlAno.SelectedValue), Convert.ToString(tseUnidadeResponsavel.DBValue));
                    }
                    else
                    {
                        lblMensagemVagas.Text = "Erro na validação dos dados: " + lblMensagemVagas.Text;

                        if (!VerificaConsistenciaAlocacao())
                        {
                            //   throw new Exception("DuplicidadeLancamentoTurma");
                            rnCtvConfVaga.RemoveDuplicidadeLancamentoTurmaPor(int.Parse(ddlAno.SelectedValue), Convert.ToString(tseUnidadeResponsavel.DBValue));
                        }
                    }
                }

                //Verifica se pode salvar e se esta em modo de analise
                if (podeSalvar && salvaAnalise)
                {
                    perfilAnalise = ddlPerfil.SelectedItem.Text;

                    //Monta lista de analises de vagas
                    analises = this.MontaAnalisesVagaPor(perfilAnalise);

                    periodos = ObtemPeriodosVigentes().Distinct().ToList();
                    rnHistoricoVaga.SalvaHistoricoVagaSeeduc(ano, periodos, censo);

                    //Salva analises
                    rnCtvAnalise.Salva(analises, perfilAnalise);
                }

                if (podeSalvar)
                {
                    InicializaVagas();
                    updatePanel2.Update();
                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupVagasOfertadas();", true);
                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
                updatePanel2.Update();
                updatePanel3.Update();

            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = ex.Message;
                lblMensagem.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupVagasOfertadas();", true);
                updatePanel2.Update();
                updatePanel3.Update();
            }
        }

        protected void btnRetornarVagasOfertadas_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupConfirmacaoValidacaoVagas();fecharPopupVagasOfertadas();", true);
        }

        protected void btnConfirmarValidacaoVagas_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.GerarPopupVagasOfertadas();
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = ex.Message;
                lblMensagem.Text = ex.Message;

            }
        }

        protected void btnCancelarValidacaoVagas_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupConfirmacaoValidacaoVagas();", true);
        }

        protected void btnExcluirTurmasProvisorias_Click(object sender, EventArgs e)
        {
            try
            {
                string Censo = tseUnidadeResponsavel.Value.ToString();
                string Ano = ddlAno.SelectedValue;
                int TipoEventoId = (int)RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas;
                string queryString = string.Empty;

                bool blnRetorno = SalvaVagasParcialmente();

                if (blnRetorno)
                {
                    queryString += "Ano=" + Ano;
                    queryString += "&Perfil=" + ddlPerfil.SelectedValue;
                    queryString += "&TurnosAnalisados=" + rblTurnosAnalisados.SelectedValue;
                    queryString += "&ResultadoTurnos=" + ddlResultadoTurnos.SelectedValue;
                    queryString += "&VagasAnalisadas=" + rblVagasAnalisadas.SelectedValue;
                    queryString += "&ResultadoVagas=" + ddlResultadoVagas.SelectedValue;
                    queryString += "&FaixaInicial=" + txtFaixaInicial.Text;
                    queryString += "&FaixaFinal=" + txtFaixaFinal.Text;
                    queryString += "&TipoVariacao=" + ddlFaixaVariacao.SelectedValue;
                    queryString += "&Modalidade=" + ddlModSegCurso.Value;
                    queryString += "&Serie=" + ddlSerie.SelectedValue;
                    queryString += "&Turno=" + ddlTurno.SelectedValue;
                    queryString += "&UnidadeEnsino=" + Censo;
                    queryString += "&TipoEvento=" + TipoEventoId.ToString();


                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                    Response.Redirect("~/Academico/TurmasProvisorias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        #endregion

        #region ObjectDataSouce gvVagas

        public object ListarVagas()
        {
            var confirmacao = Confirmacao;
            if (confirmacao != null)
            {
                confirmacao = Confirmacao
                .Where(x => !x.Delecao)
                    .ToList();
            }

            return confirmacao;
        }

        public DataTable CarregaPeriodoTurmaNova(int ano)
        {
            try
            {
                DataTable dt = new DataTable();
                RN.Agenda.Evento rnEvento = new Techne.Lyceum.RN.Agenda.Evento();
                int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);

                if (ano != -1)
                {
                    dt = rnEvento.ListaPeriodoEventoPorTipoEventoEPorAno(tipoEventoConfirmacaoVagas, ano);
                }

                return dt;
            }
            catch (Exception ex)
            {
                lblTurmaNovaMensagem.Text = ex.Message;
                return null;
            }
        }

        public DataTable CarregaModalidadesTurmaNova(string unidade_ens, int ano, int periodo, int codPerfil)
        {
            try
            {
                DataTable dt = new DataTable();

                if (!string.IsNullOrEmpty(unidade_ens))
                {
                    dt = RN.Curso.ObtemModalidadeAgendaSemRestricaoComTurnoPor(unidade_ens, ano, periodo, codPerfil);
                }

                return dt;
            }
            catch (Exception ex)
            {
                lblTurmaNovaMensagem.Text = ex.Message;
                return null;
            }
        }

        public DataTable CarregaCursosTurmaNova(string unidade_ens, string modalidade, int ano, int periodo, int codperfil)
        {
            try
            {
                DataTable dt = new DataTable();

                if (!string.IsNullOrEmpty(unidade_ens) && !string.IsNullOrEmpty(modalidade))
                    dt = RN.Curso.ObtemEscolaridadeAgendaSemRestricaoComTurnoPor(unidade_ens, ano, periodo, modalidade, codperfil);

                return dt;
            }
            catch (Exception ex)
            {
                lblTurmaNovaMensagem.Text = ex.Message;
                return null;
            }
        }

        public DataTable CarregaSeriesTurmaNova(string ano, string curso, string periodo)
        {
            try
            {
                var sessao = Session["pcTurmaNova"] as string[];
                var turno = sessao != null ? sessao[3].ToString() : null;

                DataTable dt = new DataTable();
                Serie rnSerie = new Serie();

                if (!string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(curso) && !string.IsNullOrEmpty(turno))
                {
                    if (!ano.Equals("Selecione"))
                        dt = rnSerie.ObtemSeriesNovaTurmaTurnosVagasPor(ano.ToString(), periodo, turno, curso.ToString(), tseUnidadeResponsavel.DBValue.ToString());
                }

                return dt;
            }
            catch (Exception ex)
            {
                lblTurmaNovaMensagem.Text = ex.Message;
                return null;
            }
        }

        #endregion

        #region Private Methods

        private IList<int> GetIdAgendasGridVagas()
        {
            var listaIds = new List<int>();

            for (var rowIndex = 0; rowIndex < this.gridVagas.VisibleRowCount; rowIndex++)
            {
                var idAgenda = this.gridVagas.GetRowValues(rowIndex, "IDAgenda").ToString();

                listaIds.Add(Convert.ToInt32(idAgenda));
            }

            return listaIds;
        }

        private void AtualizaControleValores(int index, string nomeControle)
        {
            foreach (var item in Controle)
            {
                if (item.Value[0] != -1 && item.Value[0] >= index)
                {
                    item.Value[0] += 1;
                }
            }

            foreach (var item in Valores)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (item.Value[i] >= index)
                    {
                        item.Value[i] += 1;
                    }
                }
            }

            Controle[nomeControle][0] = index;
            Valores[nomeControle].Add(index);
        }

        private bool ValidarPropostaSeeduc(bool finalizar)
        {
            try
            {
                StringBuilder mensagemFormatada = new StringBuilder();
                RN.Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new RN.Agenda.ParametroTurnoVaga();
                RN.Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new RN.Agenda.Entidades.ParametroTurnoVaga();
                RN.Agenda.Entidades.ParametroTurnoVaga parametroPadrao = new RN.Agenda.Entidades.ParametroTurnoVaga();
                bool ehUsuarioPadrao = false;
                int codPerfil = -1;
                int agendaId = 0;
                int somaVagas = 0;
                int somaVagasProposta = 0;

                //Carrega perfil do usuario logado
                VerificaPerfil();
                codPerfil = Convert.ToInt32(Session["codPerfil"]);

                //Busca agendaId não encerrado
                agendaId = this.Confirmacao.Where(x => x.Editavel).Max(y => y.AgendaID);

                ////Verifica a parametrização daquele perfil
                parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, agendaId);

                //Verifica se o usuario possui configuração padrao como parametro
                if (parametroTurnoVaga.ConfiguracaoPadrao)
                {
                    ehUsuarioPadrao = true;
                }
                else
                {
                    parametroPadrao = rnParametroTurnoVaga.ObtemPadraoPor(agendaId);
                    ehUsuarioPadrao = false;
                }

                for (int i = 0; i < this.Confirmacao.Count; i++)
                {
                    //Verifica se foram digitados vagas de continuidade e novas
                    if (this.Confirmacao[i].QuantCont > -1 && this.Confirmacao[i].QuantNovas > -1)
                    {
                        agendaId = this.Confirmacao[i].AgendaID;
                        var IdAgendaConfTurnoVaga = this.Confirmacao[i].IDAgenda;
                        var editavel = this.Confirmacao[i].Editavel;
                        bool serieEntrada = Convert.ToBoolean(this.Confirmacao[i].SerieEntrada);

                        if (editavel)
                        {
                            //Soma os valores de VC e VN digitados pelo usuario
                            somaVagas = this.Confirmacao[i].QuantCont + this.Confirmacao[i].QuantNovas;

                            //Soma os valores de VC e VN da proposta SEEDUC
                            somaVagasProposta = this.Confirmacao[i].QuantContSeeduc + this.Confirmacao[i].QuantNovaSeeduc;

                            //Verifica se carregou os dados da agenda
                            if (IdAgendaConfTurnoVaga > 0)
                            {
                                //Verifica se existe percentual de aumento de vaga para o perfil
                                if (parametroTurnoVaga.PercentualAumentoVaga > 0)
                                {
                                    //Verifica se a soma de VC e VN supera o percentual de aumento da proposta SEEDUC
                                    if (somaVagas > ((somaVagasProposta) * (1 + (parametroTurnoVaga.PercentualAumentoVaga / 100))))
                                    {
                                        mensagemFormatada.Append(string.Format("- A quantidade de vagas é superior a {0}% permitido da proposta SEEDUC para o curso: {1}, ano escolaridade: {2}.\\r\\n",
                                               parametroTurnoVaga.PercentualAumentoVaga.ToString(),
                                               this.Confirmacao[i].NomeCurso,
                                               this.Confirmacao[i].Serie));
                                    }
                                }
                                else
                                {
                                    //Verifica o usuario não possui o parametro padrao
                                    if (!ehUsuarioPadrao)
                                    {
                                        //Verifica se existe percentual de aumento de vaga padrao
                                        if (parametroPadrao.PercentualAumentoVaga > 0)
                                        {
                                            //Verifica se a soma de VC e VN supera o percentual de aumento da proposta SEEDUC
                                            if (somaVagas > ((somaVagasProposta) * (1 + (parametroPadrao.PercentualAumentoVaga / 100))))
                                            {
                                                mensagemFormatada.Append(string.Format("- A quantidade de vagas é superior a {0}% permitido da proposta SEEDUC para o curso: {1}, ano escolaridade: {2}.\\r\\n",
                                                       parametroPadrao.PercentualAumentoVaga.ToString(),
                                                       this.Confirmacao[i].NomeCurso,
                                                       this.Confirmacao[i].Serie));
                                            }
                                        }
                                    }
                                }

                                //Verifica se existe percentual de diminuição de vaga para o perfil
                                if (parametroTurnoVaga.PercentualDiminuicaoVaga > 0)
                                {
                                    //Verifica se a soma de VC e VN é inferior ao percentual de diminuição da proposta SEEDUC
                                    if ((somaVagas) < ((somaVagasProposta) * (parametroTurnoVaga.PercentualDiminuicaoVaga / 100)))
                                    {
                                        mensagemFormatada.Append(string.Format("- A quantidade de vagas é inferior a {0}% permitido da proposta SEEDUC para o curso: {1}, ano escolaridade: {2}.\\r\\n",
                                               parametroTurnoVaga.PercentualDiminuicaoVaga.ToString(),
                                               this.Confirmacao[i].NomeCurso,
                                               this.Confirmacao[i].Serie));
                                    }
                                }

                                //Verifica o usuario não possui o parametro padrao
                                if (!ehUsuarioPadrao)
                                {
                                    //Verifica se existe percentual de diminuição de vaga padrao
                                    if (parametroPadrao.PercentualDiminuicaoVaga > 0)
                                    {
                                        //Verifica se a soma de VC e VN é inferior ao percentual de diminuição da proposta SEEDUC
                                        if ((somaVagas) < ((somaVagasProposta) * ((parametroPadrao.PercentualDiminuicaoVaga / 100))))
                                        {
                                            mensagemFormatada.Append(string.Format("- A quantidade de vagas é inferior a {0}% permitido da proposta SEEDUC para o curso: {1}, ano escolaridade: {2}.\\r\\n",
                                                   parametroPadrao.PercentualDiminuicaoVaga.ToString(),
                                                   this.Confirmacao[i].NomeCurso,
                                                   this.Confirmacao[i].Serie));
                                        }
                                    }
                                }

                                //Validação de Lançamento de Vagas com valores maior do que 0 caso exista Proposta Seeduc
                                if (this.Confirmacao[i].QuantContSeeduc >= 1)
                                {
                                    if (this.Confirmacao[i].QuantCont < 1)
                                    {
                                        mensagemFormatada.Append(string.Format("Modalidade: {0} - Curso: {1} - Série: {2} - Erro: A quantidade de vagas de continuidade informada ({3}) não pode ser zero quando existe proposta seeduc para vagas de continuidade.\\r\\n",
                                            this.Confirmacao[i].Modalidade,
                                            this.Confirmacao[i].NomeCurso,
                                            this.Confirmacao[i].Serie,
                                            this.Confirmacao[i].QuantCont
                                            ));
                                    }
                                }

                                if (this.Confirmacao[i].QuantNovaSeeduc >= 1)
                                {
                                    if (this.Confirmacao[i].QuantNovas < 1)
                                    {
                                        mensagemFormatada.Append(string.Format("Modalidade: {0} - Curso: {1} - Série: {2} - Erro: A quantidade de vagas novas informada ({3}) não pode ser zero quando existe proposta seeduc para vagas de continuidade.\\r\\n",
                                           this.Confirmacao[i].Modalidade,
                                           this.Confirmacao[i].NomeCurso,
                                           this.Confirmacao[i].Serie,
                                           this.Confirmacao[i].QuantNovas
                                           ));
                                    }
                                }
                            }
                            else
                            {
                                lblMensagemVagas.Text = "Erro: Não foi possivel encontrar o valor do IdAgendaConfTurnoVaga.\\r\\n";
                                return false;
                            }
                        }
                    }
                }

                if (finalizar)
                {
                    //Caso esteja finalizando verificar minimo de vagas por turno
                    var errosMinimoVagas = ValidaMinimoVagas();
                    if (!string.IsNullOrEmpty(errosMinimoVagas))
                    {
                        mensagemFormatada.Append(errosMinimoVagas);
                    }
                }

                if (!string.IsNullOrEmpty(mensagemFormatada.ToString()))
                {
                    if (ehUsuarioPadrao)
                    {
                        //Usuarios que possuam o parametro padrao, devem ser impedidas de gravar alteraçoes caso os parametros não sejam respeitados
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", @"alert('" + mensagemFormatada.ToString() + "')", true);
                    }
                    else
                    {
                        //Para demais usuarios apenas emitir mensagem de aviso possibilitando confirmação
                        string mensagemLabel = mensagemFormatada.ToString().Replace("\\r\\n", "</br>");

                        lblMensagemValidacaoVagas.Text = mensagemLabel;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupConfirmacaoValidacaoVagas();", true);
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                lblMensagem.Text = ex.Message;
                lblMensagemVagasBottom.Text = ex.Message;
                return false;
            }
        }

        private bool ValidarJustificativa()
        {
            try
            {
                StringBuilder mensagemFormatada = new StringBuilder();

                //Validação de justificativa no Lançamento de Vagas quando vagas lançadas form diferentes de Proposta Seeduc
                for (int i = 0; i < this.Confirmacao.Count; i++)
                {
                    if (this.Confirmacao[i].QuantNovaSeeduc > -1 && this.Confirmacao[i].Editavel)
                    {
                        if (this.Confirmacao[i].QuantNovas != this.Confirmacao[i].QuantNovaSeeduc && string.IsNullOrEmpty(this.Confirmacao[i].JustificativaNova.Trim()))
                        {
                            mensagemFormatada.Append("- Modalidade: " + this.Confirmacao[i].Modalidade + " - Curso: " + this.Confirmacao[i].NomeCurso + " - Série: " + this.Confirmacao[i].Serie + " - Erro: O campo JUSTIFICATIVA NOVA é obrigatorio quando o valor da proposta UE for diferente da proposta SEEDUC.\\r\\n");
                        }
                    }

                    if (!string.IsNullOrEmpty(this.Confirmacao[i].JustificativaNova.Trim()) && this.Confirmacao[i].JustificativaNova.Length > 500 && this.Confirmacao[i].Editavel)
                    {
                        mensagemFormatada.Append("- Modalidade: " + this.Confirmacao[i].Modalidade + " - Curso: " + this.Confirmacao[i].NomeCurso + " - Série: " + this.Confirmacao[i].Serie + " - Erro: O campo JUSTIFICATIVA NOVA deve ter no máximo 500 caracteres.\\r\\n");
                    }

                    if (!string.IsNullOrEmpty(this.Confirmacao[i].JustificativaNova.Trim()) && this.Confirmacao[i].JustificativaNova.Length < 10 && this.Confirmacao[i].Editavel)
                    {
                        mensagemFormatada.Append("- Modalidade: " + this.Confirmacao[i].Modalidade + " - Curso: " + this.Confirmacao[i].NomeCurso + " - Série: " + this.Confirmacao[i].Serie + " - Erro: O campo JUSTIFICATIVA NOVA deve ter mais 10 caracteres.\\r\\n");
                    }
                }

                if (!string.IsNullOrEmpty(mensagemFormatada.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", @"alert('" + mensagemFormatada.ToString() + "')", true);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                return false;
            }
        }
        private bool ValidaJustificativaFinalizarTurno(out string mensagem)
        {
            try
            {
                var erros = new List<string>();

                for (var rowIndex = 0; rowIndex < this.grdConfTurnos.VisibleRowCount; rowIndex++)
                {
                    var linha = rowIndex + 1;

                    var encerrado = (bool)this.grdConfTurnos.GetRowValues(rowIndex, "Encerrado") == false ? "0" : "1";

                    var chkManha = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Manha", "chkManha");
                    var chkTarde = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Tarde", "chkTarde");
                    var chkNoite = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Noite", "chkNoite");
                    var chkIntegral = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Integral", "chkIntegral");
                    var chkAmpliado = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "Ampliado", "chkAmpliado");
                    var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "Justificativa", "txtJustificativa");
                    var hdnValorAntigo = DevExpressHelper.GetControl<HiddenField>(this.grdConfTurnos, rowIndex, "Justificativa", "hdnValorAntigo");

                    var chkManhaNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "ManhaNovo", "chkManhaNovo");
                    var chkTardeNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "TardeNovo", "chkTardeNovo");
                    var chkNoiteNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "NoiteNovo", "chkNoiteNovo");
                    var chkIntegralNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "IntegralNovo", "chkIntegralNovo");
                    var chkAmpliadoNovo = DevExpressHelper.GetControl<CheckBox>(this.grdConfTurnos, rowIndex, "AmpliadoNovo", "chkAmpliadoNovo");
                    var txtJustificativaNovo = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, rowIndex, "JustificativaNovo", "txtJustificativaNovo");

                    if (encerrado == "0")
                    {
                        var ctvTurnos = new DadosConfTurno()
                        {
                            Manha = chkManha.Checked,
                            Tarde = chkTarde.Checked,
                            Noite = chkNoite.Checked,
                            Integral = chkIntegral.Checked,
                            Ampliado = chkAmpliado.Checked,
                            TurnosIniciais = hdnValorAntigo.Value,
                            Justificativa = txtJustificativa.Text.Trim(),
                            ManhaNovo = chkManhaNovo.Checked,
                            TardeNovo = chkTardeNovo.Checked,
                            NoiteNovo = chkNoiteNovo.Checked,
                            IntegralNovo = chkIntegralNovo.Checked,
                            AmpliadoNovo = chkAmpliadoNovo.Checked,
                            JustificativaNovo = txtJustificativaNovo.Text.Trim(),
                            TurnosListaInicial = Convert.ToString(this.grdConfTurnos.GetRowValues(rowIndex, "TurnosListaInicial"))
                        };

                        if (!ctvTurnos.TurnosIniciais.Equals(ctvTurnos.Turnos)
                        && string.IsNullOrEmpty(ctvTurnos.Justificativa))
                        {
                            erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA CONTINUIDADE é de preenchimento obrigatório."));
                        }

                        if (!ctvTurnos.TurnosIniciais.Equals(ctvTurnos.TurnosNovo)
                            && string.IsNullOrEmpty(ctvTurnos.JustificativaNovo))
                        {
                            erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA NOVO é de preenchimento obrigatório."));
                        }

                        if (!string.IsNullOrEmpty(ctvTurnos.Justificativa))
                        {
                            if (ctvTurnos.Justificativa.Length < 10)
                            {
                                erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA CONTINUIDADE deve ter mais 10 caracteres."));
                            }

                            if (ctvTurnos.Justificativa.Length > 500)
                            {
                                erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA CONTINUIDADE é obrigatório com o máximo de 500 caracteres!"));
                            }

                            var regex = new Regex(@"(\w)\1\1+");

                            if (regex.IsMatch(ctvTurnos.Justificativa))
                            {
                                erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA CONTINUIDADE não deve ter mais de 2 letras consecutivas repetidas."));
                            }
                        }

                        if (!string.IsNullOrEmpty(ctvTurnos.JustificativaNovo))
                        {
                            if (ctvTurnos.JustificativaNovo.Length < 10)
                            {
                                erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA NOVO deve ter mais 10 caracteres."));
                            }

                            if (ctvTurnos.JustificativaNovo.Length > 500)
                            {
                                erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA NOVO é obrigatório com o máximo de 500 caracteres!"));
                            }

                            var regex = new Regex(@"(\w)\1\1+");

                            if (regex.IsMatch(ctvTurnos.JustificativaNovo))
                            {
                                erros.Add(string.Format("Linha {0} gerou o erro: {1}", linha, "O campo JUSTIFICATIVA NOVO não deve ter mais de 2 letras consecutivas repetidas."));
                            }
                        }
                    }
                }

                if (erros.Count > 0)
                {
                    mensagem = erros.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                    return false;
                }
                mensagem = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                mensagem = ex.Message;
                return false;
            }
        }
        private String GetNameFromTurno(String primeiraLetra)
        {
            switch (primeiraLetra)
            {
                case "M": return Turnos.Manha.ToString();
                case "T": return Turnos.Tarde.ToString();
                case "N": return Turnos.Noite.ToString();
                case "A": return Turnos.Ampliado.ToString();
                case "I": return Turnos.Integral.ToString();
                default:
                    return String.Empty;
            }
        }

        private List<String> GetDemaisTurnos(List<string> turnosExistentes)
        {
            var lista = new List<String>();

            foreach (var item in Enum.GetNames(typeof(Turnos)))
            {
                if (!turnosExistentes.Contains(item))
                {
                    lista.Add(item);
                }
            }

            return lista;
        }

        private Control GetControleDaGridPorNome(int indiceLinhaTabela, int indiceColunaTabela, string nomeControle)
        {
            return DevExpressHelper.GetControl<Control>(this.gridSalas, indiceLinhaTabela, ((Turnos)indiceColunaTabela).ToString(), nomeControle);
        }

        private void MantemGridConfirmacao(int indiceLinhaTabela, string IDComponente, int quantidadeAlunos, TipoVaga tipoVaga, string nomeTurma)
        {
            try
            {
                var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();
                var turno = IDComponente.Split('_').Last();
                var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turno);

                var indiceColunaTabela = turnos.Select((x, i) => i)
                                                    .Where(x => turnos[x] == turno)
                                                        .FirstOrDefault() + 1;

                if (String.IsNullOrEmpty(nomeTurma))
                    nomeTurma = (GetControleDaGridPorNome(indiceLinhaTabela, indiceColunaTabela, nomeControle) as DropDownList)
                                        .SelectedItem.Value;

                nomeControle = String.Format("{0}_{1}", nomeControle, indiceLinhaTabela);

                // Lógica para manter integridade dos dados da grid de confirmação
                AtualizaGridConfirmacao(nomeTurma, nomeControle, quantidadeAlunos, tipoVaga);

                if (this.Controle != null)
                {
                    //Atualiza controle para eu saber qual o valor anterior
                    Controle.Where(x => x.Key == nomeControle).Select(x => x.Value).FirstOrDefault()[(int)tipoVaga] = quantidadeAlunos;
                }
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = ex.Message;
            }
        }

        private void AtualizaGridConfirmacao(string turma, string nomeControle, int quantidadeAlunos, TipoVaga tipoVaga)
        {
            try
            {
                int quantidadeAntiga = 0;

                if (!String.IsNullOrEmpty(turma))
                {
                    var ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
                    var censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                    int periodo = -1;

                    //lógica para identificar a série, curso e modalidade
                    var dadosTurmaConfVaga = this.Turmas.Where(x => x.Turma == turma).Single();
                    periodo = dadosTurmaConfVaga.Periodo;

                    var dadosTurma = CtvAgendaConfTurnoVaga.RetornaDadosTurma(ano, periodo, censo, turma);
                    if (dadosTurma == null || dadosTurma.IdAgenda <= 0)
                    {
                        //Se não encontrar turma no ano/periodo referencia, ver turma provisoria
                        dadosTurma = CtvAgendaConfTurnoVaga.RetornaDadosTurmaProvisoria(ano, periodo, censo, turma);
                    }

                    var AgendaID = dadosTurma.IdAgenda;

                    var confirmacaoVaga = Confirmacao
                                           .Where(x => (x.IDAgenda == AgendaID))
                                               .FirstOrDefault();


                    if (confirmacaoVaga != null)
                    {
                        confirmacaoVaga.Delecao = false;
                        //se for atualização, precisa retirar o valor anterior e somar o valor atual

                        quantidadeAntiga = Controle.Where(x => x.Key == nomeControle)
                                   .Select(x => x.Value).FirstOrDefault()[(int)tipoVaga];

                        switch (tipoVaga)
                        {
                            case TipoVaga.Continuidade:
                                confirmacaoVaga.QuantCont += quantidadeAlunos - quantidadeAntiga;
                                break;
                            case TipoVaga.Nova:
                                confirmacaoVaga.QuantNovas += quantidadeAlunos - quantidadeAntiga;
                                //Verifica se a quantidade de vagas lançadas pela escola é igual a quantidade proposta pela SEEDUC
                                if (confirmacaoVaga.QuantNovaSeeduc == confirmacaoVaga.QuantNovas)
                                {
                                    //Se for igual não é necessario justificativa
                                    confirmacaoVaga.JustificativaNova = string.Empty;
                                }
                                break;
                        }
                    }

                    this.ListarVagas();
                    gridVagas.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
            }
        }

        private void ConfereDelecaoSerie(int AgendaID)
        {
            var quantNovasVagas = Confirmacao
                                      .Where(x => (x.IDAgenda == AgendaID))
                                        .FirstOrDefault()
                                            .QuantNovas;

            var quantContVagas = Confirmacao
                                    .Where(x => (x.IDAgenda == AgendaID))
                                        .FirstOrDefault()
                                            .QuantCont;

            if (quantContVagas == 0 && quantNovasVagas == 0)
            {
                var pos = Confirmacao.Select((x, i) => i)
                            .Where(x => Confirmacao[x].IDAgenda == AgendaID)
                                .FirstOrDefault();

                if (Confirmacao[pos].ID > 0)
                    Confirmacao[pos].Delecao = true;
                else
                    Confirmacao.RemoveAt(pos);
            }
        }

        private List<int> GetIndicesTurmas(string nomeControle)
        {
            var listaIndices = new List<int>();

            if (this.Turmas != null)
            {
                for (int i = 0; i < Turmas.Count; i++)
                    listaIndices.Add(i);
            }

            var listaIndicesIndisponiveis = Controle.Where(x => (x.Value[0] != -1) && (x.Key != nomeControle))
                        .Select(x => x.Value[0]).ToList();

            return listaIndices.Where(x => !listaIndicesIndisponiveis.Any(x1 => x1 == x)).ToList();
        }

        private int GetIndexOfSelectedValue(string turma)
        {
            for (int i = 0; i < Turmas.Count; i++)
            {
                if (Turmas[i].Turma == turma)
                    return i;
            }

            // se não encontrar é porque é um item de demonstração
            return -1;
        }

        private void AtualizaDadosGridSalas(string name, int indexOfSelectedValue)
        {
            try
            {
                //retorna todos nomes dos componentes que devem remover um item selecionado por uma determinada combo
                var nomeComponentesParaAtualizar = Valores.Where(x => x.Key != name)
                                                              .Select(x => x.Key)
                                                                .ToArray();

                foreach (var item in nomeComponentesParaAtualizar)
                {
                    //remove o item
                    Valores[item].Remove(indexOfSelectedValue);

                    //o index anterio deve voltar a aparecer nas combos
                    var indexControle = Controle[name];

                    //se for -1 é porque era o item de "selecione"
                    if (indexControle[0] != -1)
                    {
                        Valores[item].Add(indexControle[0]);
                        Valores[item] = Valores[item].OrderBy(x => x).ToList();
                    }
                }

                Controle[name][0] = indexOfSelectedValue;
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = ex.Message;
            }
        }

        private void AtualizaCombosTurmas()
        {
            try
            {
                var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();

                for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
                {
                    for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                    {
                        var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna]);
                        var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                        var nome = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna]);
                        var currentTextBoxVC = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;

                        nome = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna]);
                        var currentTextBoxVN = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;

                        //guarda qual o valor selecionado
                        var selectedValue = currentDropDown.SelectedValue;

                        //atualiza o datasource da dropdown atual
                        nomeControle = String.Format("{0}_{1}", nomeControle, indiceLinha);
                        var dataSource = ListTurmasPorCombo(nomeControle);

                        currentDropDown.ClearSelection();
                        currentDropDown.DataSource = dataSource;

                        //recupera qual era o valor selecionado
                        var indice = GetIndiceItemSelecionadoCombo(dataSource, selectedValue);
                        if (indice == -1)
                        {
                            currentDropDown.SelectedIndex = indice;
                            currentDropDown.DataBind();
                            currentDropDown.SelectedIndex = indice;
                        }
                        else
                        {
                            currentDropDown.SelectedValue = selectedValue;
                            currentDropDown.SelectedIndex = indice;
                            currentDropDown.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = ex.Message;
            }
        }

        private int GetIndiceItemSelecionadoCombo(List<DadosTurmaConfVaga> dataSource, string selectedValue)
        {
            for (int i = 0; i < dataSource.Count; i++)
            {
                if (dataSource[i].Turma == selectedValue)
                {
                    return i;
                }
            }
            return -1;
        }

        private List<DadosTurmaConfVaga> ListTurmasPorCombo(string nome)
        {
            var lista = new List<DadosTurmaConfVaga>();

            lista = Valores[nome]
                        .Select(x => Turmas[x])
                            .ToList();
            return lista;
        }

        private void AtualizaGridSalasPosSalvamento()
        {
            var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x.ToString()).ToArray();

            for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
            {
                for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                {
                    var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna][0]);
                    var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                    if (currentDropDown.SelectedIndex != 0)
                    {
                        nomeControle = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna][0]);
                        var currentTextBoxVC = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        nomeControle = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna][0]);
                        var currentTextBoxVN = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        if (PerfilUsuario.Equals("DIRETOR"))
                            currentDropDown.Enabled = currentTextBoxVC.Enabled = currentTextBoxVN.Enabled = false;
                    }
                }
            }
        }

        private void FinalizarLancamento()
        {
            try
            {
                var censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                CtvConfVaga rnCtvConfVaga = new CtvConfVaga();

                if (!VerificaConsistenciaAlocacao())
                {
                    rnCtvConfVaga.RemoveDuplicidadeLancamentoTurmaPor(int.Parse(ddlAno.SelectedValue), censo);
                    //throw new Exception("DuplicidadeLancamentoTurma");
                }

                RN.CtvFinalizado.InserirVaga(Confirmacao, censo, User.Identity.Name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool VerificaConsistenciaAlocacao()
        {
            CtvConfVaga rnCtvConfVaga = new CtvConfVaga();
            DataTable dtTurmas = null;
            string salas = string.Empty;
            try
            {
                var censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                dtTurmas = rnCtvConfVaga.ListaTurmasDuplicidadeVagasPor(int.Parse(ddlAno.SelectedValue), censo);

                if (dtTurmas.Rows.Count == 0)
                {
                    Session["MensagemInconsistencia"] = string.Empty;
                    return true;
                }
                else
                {
                    lblMensagemVagas.Text = lblMensagemVagas.Text + "</br>Foram encontradas inconsistências para a turma " + dtTurmas.Rows[0]["TURMA"] + " , pois a mesma está vinculada a sala " + dtTurmas.Rows[0]["SALA"] + " e agora foi informada também na sala " + dtTurmas.Rows[1]["SALA"] + ". Dessa forma, a turma ficará registrada na sala " + dtTurmas.Rows[1]["SALA"] + ". Favor confirmar novamente a alocação da turma";
                    Session["MensagemInconsistencia"] = lblMensagemVagas.Text;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SalvarJustificativasVagas()
        {
            try
            {
                var censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;

                var itensParaRemocao = new List<int>();

                for (int i = 0; i < Confirmacao.Count; i++)
                {
                    var justificativa = new TceCtvJustificativa
                    {
                        IdJustificativa = Confirmacao[i].ID,
                        IdAgendaConfTurnoVaga = Confirmacao[i].IDAgenda,
                        Censo = censo,
                        VagasContinuidade = Confirmacao[i].QuantCont,
                        VagasNovo = Confirmacao[i].QuantNovas,
                        Matricula = User.Identity.Name,
                        Vaga = true,
                        Turno = false,
                        JustificativaNovo = Confirmacao[i].JustificativaNova
                    };

                    if (Confirmacao[i].Delecao)
                    {
                        RN.CtvJustificativa.Remover(justificativa.IdJustificativa);
                        itensParaRemocao.Add(i);
                    }
                    else
                        Confirmacao[i].ID = RN.CtvJustificativa.Salvar(justificativa);
                }

                foreach (var item in itensParaRemocao.OrderByDescending(x => x))
                {
                    Confirmacao.RemoveAt(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GerarPopupVagasOfertadas()
        {
            try
            {
                int AgendaId = 0;
                int AgendaVinculadaId = 0;
                DataTable cursosMatriculaFacil = new DataTable();
                RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
                bool matFacil = false;
                string nome;
                string nomeControle;
                DropDownList currentDropDown;
                TextBox currentTextBoxVC;
                TextBox currentTextBoxVN;
                string turno;
                int ano;
                string censo;
                string turma;
                string currentIdAgenda;
                int periodo;
                DadosTurmaConfVaga dadosTurmaConfVaga;
                DadosTurmaAgenda dadosTurma;
                string[] vagaTurno;
                string[] turnos = Enum.GetNames(typeof(Turnos)).Select(x => x[0].ToString()).ToArray();
                List<string[]> listaVagasTurno = new List<string[]>();
                string id;
                string idAgenda;
                string modalidade;
                string curso;
                string serie;
                string descricaoSerie;
                bool editavel;
                string periodoGridVagas;
                HtmlTableRow trVagas;
                HtmlTableCell tdPeriodo;
                HtmlTableCell tdCursoVagas;
                HtmlTableCell tdManhaMF;
                HtmlTableCell tdTardeMF;
                HtmlTableCell tdNoiteMF;
                HtmlTableCell tdAmpliadoMF;
                HtmlTableCell tdIntegralMF;
                HtmlTableCell tdManhaAbsorcao;
                HtmlTableCell tdTardeAbsorcao;
                HtmlTableCell tdNoiteAbsorcao;
                HtmlTableCell tdAmpliadoAbsorcao;
                HtmlTableCell tdIntegralAbsorcao;
                int listaManhaMF;
                int listaTardeMF;
                int listaNoiteMF;
                int listaAmpliadoMF;
                int listaIntegralMF;
                int listaManhaAbsorcao;
                int listaTardeAbsorcao;
                int listaNoiteAbsorcao;
                int listaAmpliadoAbsorcao;
                int listaIntegralAbsorcao;
                AgendaId = ObtemAgendaIdVagas();
                AgendaVinculadaId = rnAgenda.ObtemAgendaVinculadaPor(AgendaId, Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.MatriculaFacil));

                //Verfica se eh Curso / series do matricula facil
                cursosMatriculaFacil = rnAgenda.ListaCursosMatriculaFacilPor(AgendaVinculadaId);

                //Retorna Lista de turmas escolhidas na grid de Salas de aula
                for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                {
                    for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
                    {
                        nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna][0]);
                        currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                        nome = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna][0]);
                        currentTextBoxVC = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;

                        nome = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna][0]);
                        currentTextBoxVN = GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nome) as TextBox;

                        turno = ((Turnos)(indiceColuna + 1)).ToString();

                        ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
                        censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                        turma = currentDropDown.SelectedValue.ToString();
                        currentIdAgenda = string.Empty;
                        periodo = -1;

                        if (turma != "-1")
                        {
                            dadosTurmaConfVaga = this.Turmas.Where(x => x.Turma == turma).Single();
                            periodo = dadosTurmaConfVaga.Periodo;

                            dadosTurma = CtvAgendaConfTurnoVaga.RetornaDadosTurma(ano, periodo, censo, turma);
                            if (dadosTurma == null || dadosTurma.IdAgenda <= 0)
                            {
                                //Se não encontrar turma no ano/periodo referencia, ver turma provisoria
                                dadosTurma = CtvAgendaConfTurnoVaga.RetornaDadosTurmaProvisoria(ano, periodo, censo, turma);
                            }

                            currentIdAgenda = dadosTurma.IdAgenda.ToString();
                        }

                        vagaTurno = new string[] { currentIdAgenda.ToString(), currentTextBoxVC.Text, currentTextBoxVN.Text, turno };

                        listaVagasTurno.Add(vagaTurno);
                    }
                }

                //Retorna Lista de turmas escolhidas na grid de Resumo das vagas
                for (var rowIndex = 0; rowIndex < this.gridVagas.VisibleRowCount; rowIndex++)
                {
                    id = this.gridVagas.GetRowValues(rowIndex, "ID").ToString();
                    idAgenda = this.gridVagas.GetRowValues(rowIndex, "IDAgenda").ToString();
                    modalidade = this.gridVagas.GetRowValues(rowIndex, "Modalidade").ToString();
                    curso = this.gridVagas.GetRowValues(rowIndex, "Curso").ToString();
                    serie = this.gridVagas.GetRowValues(rowIndex, "Serie").ToString();
                    descricaoSerie = this.gridVagas.GetRowValues(rowIndex, "DescricaoSerie").ToString();
                    editavel = (bool)this.gridVagas.GetRowValues(rowIndex, "Editavel");
                    periodoGridVagas = this.gridVagas.GetRowValues(rowIndex, "Periodo").ToString();

                    if (editavel)
                    {
                        trVagas = new HtmlTableRow();
                        tdPeriodo = new HtmlTableCell();
                        tdCursoVagas = new HtmlTableCell();
                        tdManhaMF = new HtmlTableCell();
                        tdTardeMF = new HtmlTableCell();
                        tdNoiteMF = new HtmlTableCell();
                        tdAmpliadoMF = new HtmlTableCell();
                        tdIntegralMF = new HtmlTableCell();
                        tdManhaAbsorcao = new HtmlTableCell();
                        tdTardeAbsorcao = new HtmlTableCell();
                        tdNoiteAbsorcao = new HtmlTableCell();
                        tdAmpliadoAbsorcao = new HtmlTableCell();
                        tdIntegralAbsorcao = new HtmlTableCell();

                        if (cursosMatriculaFacil != null)
                        {
                            matFacil = cursosMatriculaFacil.Select("curso = '" + curso + "' and serie= " + serie + "").Length > 0;
                        }
                        else
                        {
                            matFacil = false;
                        }

                        listaManhaMF = 0;
                        listaTardeMF = 0;
                        listaNoiteMF = 0;
                        listaAmpliadoMF = 0;
                        listaIntegralMF = 0;
                        listaManhaAbsorcao = 0;
                        listaTardeAbsorcao = 0;
                        listaNoiteAbsorcao = 0;
                        listaAmpliadoAbsorcao = 0;
                        listaIntegralAbsorcao = 0;

                        if (matFacil && (Session["perfil"].ToString() != "DIESP" && (Convert.ToString(tseRegional.DBValue) != "5")))
                        {
                            listaManhaMF =
                            listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Manha").Select(
                                y => int.Parse(y[2])).Sum();
                            listaTardeMF =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Tarde").Select(
                                    y => int.Parse(y[2])).Sum();
                            listaNoiteMF =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Noite").Select(
                                    y => int.Parse(y[2])).Sum();
                            listaAmpliadoMF =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Ampliado").Select(
                                    y => int.Parse(y[2])).Sum();
                            listaIntegralMF =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Integral").Select(
                                    y => int.Parse(y[2])).Sum();

                            listaManhaAbsorcao =
                           listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Manha").Select(
                               y => int.Parse(y[1])).Sum();
                            listaTardeAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Tarde").Select(
                                    y => int.Parse(y[1])).Sum();
                            listaNoiteAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Noite").Select(
                                    y => int.Parse(y[1])).Sum();
                            listaAmpliadoAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Ampliado").Select(
                                    y => int.Parse(y[1])).Sum();
                            listaIntegralAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Integral").Select(
                                    y => int.Parse(y[1])).Sum();
                        }
                        else
                        {
                            listaManhaAbsorcao =
                            listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Manha").Select(
                                y => int.Parse(y[1]) + int.Parse(y[2])).Sum();
                            listaTardeAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Tarde").Select(
                                    y => int.Parse(y[1]) + int.Parse(y[2])).Sum();
                            listaNoiteAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Noite").Select(
                                    y => int.Parse(y[1]) + int.Parse(y[2])).Sum();
                            listaAmpliadoAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Ampliado").Select(
                                    y => int.Parse(y[1]) + int.Parse(y[2])).Sum();
                            listaIntegralAbsorcao =
                                listaVagasTurno.Where(x => x[0] == idAgenda && x[3] == "Integral").Select(
                                    y => int.Parse(y[1]) + int.Parse(y[2])).Sum();
                        }

                        tdPeriodo.Align = "center";
                        tdPeriodo.InnerText = periodoGridVagas;

                        tdCursoVagas.Align = "center";
                        tdCursoVagas.InnerText = modalidade + " / " + curso + " / " + descricaoSerie;

                        tdManhaMF.Align = "center";
                        tdManhaMF.InnerText = Convert.ToString(listaManhaMF);
                        tdTardeMF.Align = "center";
                        tdTardeMF.InnerText = Convert.ToString(listaTardeMF);
                        tdNoiteMF.Align = "center";
                        tdNoiteMF.InnerText = Convert.ToString(listaNoiteMF);
                        tdAmpliadoMF.Align = "center";
                        tdAmpliadoMF.InnerText = Convert.ToString(listaAmpliadoMF);
                        tdIntegralMF.Align = "center";
                        tdIntegralMF.InnerText = Convert.ToString(listaIntegralMF);

                        tdManhaAbsorcao.Align = "center";
                        tdManhaAbsorcao.InnerText = Convert.ToString(listaManhaAbsorcao);
                        tdTardeAbsorcao.Align = "center";
                        tdTardeAbsorcao.InnerText = Convert.ToString(listaTardeAbsorcao);
                        tdNoiteAbsorcao.Align = "center";
                        tdNoiteAbsorcao.InnerText = Convert.ToString(listaNoiteAbsorcao);
                        tdAmpliadoAbsorcao.Align = "center";
                        tdAmpliadoAbsorcao.InnerText = Convert.ToString(listaAmpliadoAbsorcao);
                        tdIntegralAbsorcao.Align = "center";
                        tdIntegralAbsorcao.InnerText = Convert.ToString(listaIntegralAbsorcao);

                        trVagas.Cells.Add(tdPeriodo);
                        trVagas.Cells.Add(tdCursoVagas);
                        trVagas.Cells.Add(tdManhaMF);
                        trVagas.Cells.Add(tdTardeMF);
                        trVagas.Cells.Add(tdNoiteMF);
                        trVagas.Cells.Add(tdAmpliadoMF);
                        trVagas.Cells.Add(tdIntegralMF);
                        trVagas.Cells.Add(tdManhaAbsorcao);
                        trVagas.Cells.Add(tdTardeAbsorcao);
                        trVagas.Cells.Add(tdNoiteAbsorcao);
                        trVagas.Cells.Add(tdAmpliadoAbsorcao);
                        trVagas.Cells.Add(tdIntegralAbsorcao);

                        tbVagas.Rows.Add(trVagas);

                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupVagasOfertadas();", true);

                updatePanel6.Update();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupConfirmacaoValidacaoVagas();fecharPopupVagasOfertadas();", true);
                throw ex;
            }
        }

        private Boolean SalvarConfirmacaoVagasDaGrid(bool finalizar)
        {
            List<DadosConfVagaEncadeado> listaTotalVagas = new List<DadosConfVagaEncadeado>();
            List<DadosConfVagaEncadeado> listaPaisVagas = new List<DadosConfVagaEncadeado>();
            DadosTurmaConfVaga dadosTurma = new DadosTurmaConfVaga();
            RN.CtvConfVaga rnCtvConfVaga = new CtvConfVaga();
            List<DadosVagaSalva> listaVagasSalvas = new List<DadosVagaSalva>();
            int periodo = -1;
            var mensagensParcial = new List<string>();
            var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x.ToString()).ToArray();
            var ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
            List<string> turmas = new List<string>();
            lblMensagemVagas.Text = String.Empty;
            int codPerfil = -1;

            try
            {
                this.VerificaPerfil();

                if (!string.IsNullOrEmpty(Session["codPerfil"].ToString()))
                {
                    codPerfil = Convert.ToInt32(Session["codPerfil"]);
                }

                for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
                {
                    for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                    {
                        var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna][0]);
                        var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                        var currentHiddenID = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                            String.Format("hdnID{0}", turnos[indiceColuna])) as HiddenField);

                        var currentHiddenEditavel = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                            String.Format("hdnEditavel{0}", turnos[indiceColuna])) as HiddenField);

                        var currentHiddenTurmaFilha = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                           String.Format("hdnTurmaFilha{0}", turnos[indiceColuna])) as HiddenField);

                        nomeControle = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna][0]);
                        var currentTextBoxVC = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        nomeControle = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna][0]);
                        var currentTextBoxVN = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        if (currentDropDown.SelectedIndex != 0 || !currentHiddenTurmaFilha.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            var editavel = !String.IsNullOrEmpty(currentHiddenEditavel.Value) ?
                                    Convert.ToBoolean(currentHiddenEditavel.Value) : true;

                            if (editavel)
                            {
                                var sala = gridSalas.GetRowValues(indiceLinha, "SalaCapacidade").ToString().Split('[').First();
                                var salaCapacidade = gridSalas.GetRowValues(indiceLinha, "SalaCapacidade").ToString().Split('[').Last();

                                if (currentDropDown.SelectedIndex != 0)
                                {
                                    dadosTurma = this.Turmas.Where(x => x.Turma == currentDropDown.SelectedValue && x.DescricaoTurma == currentDropDown.SelectedItem.Text).Single();

                                    periodo = dadosTurma.Periodo;

                                    var vaga = new DadosConfVagaEncadeado
                                    {
                                        Turno = turnos[indiceColuna][0].ToString(),
                                        Censo = Convert.ToString(tseUnidadeResponsavel.DBValue),
                                        Turma = currentDropDown.SelectedValue,
                                        VagasNovas = !String.IsNullOrEmpty(currentTextBoxVN.Text) ? Convert.ToInt32(currentTextBoxVN.Text) : 0,
                                        VagasContinuidade = !String.IsNullOrEmpty(currentTextBoxVC.Text) ? Convert.ToInt32(currentTextBoxVC.Text) : 0,
                                        Matricula = User.Identity.Name,
                                        Sala = sala,
                                        Periodo = periodo,
                                        Ano = ano,
                                        SalaCapacidade = int.Parse(salaCapacidade.TrimEnd(']')),
                                        TurmaReferenciada = currentHiddenTurmaFilha.Value,
                                        Validado = false,
                                        Processado = false
                                    };

                                    listaTotalVagas.Add(vaga);
                                }
                                else
                                {
                                    var vaga = new DadosConfVagaEncadeado
                                    {
                                        IdConfVaga = Convert.ToInt32(currentHiddenID.Value),
                                        Turno = turnos[indiceColuna][0].ToString(),
                                        Censo = Convert.ToString(tseUnidadeResponsavel.DBValue),
                                        Turma = string.Empty,
                                        VagasNovas = 0,
                                        VagasContinuidade = 0,
                                        Matricula = User.Identity.Name,
                                        Sala = sala,
                                        Periodo = -1,
                                        Ano = 0,
                                        SalaCapacidade = int.Parse(salaCapacidade.TrimEnd(']')),
                                        TurmaReferenciada = currentHiddenTurmaFilha.Value,
                                        Validado = false,
                                        Processado = false
                                    };
                                    listaTotalVagas.Add(vaga);
                                }
                            }
                        }
                    }
                }

                //Busca a lista de turmas Pais (Serão consideradas Pais turmas que não existam como Filha (Turma Referenciada) em nenhuma outra sala OU que estejam vazias e com isso não podem ser filha de outra)
                listaPaisVagas = listaTotalVagas.Where(a => (!listaTotalVagas.Any(b => a != b && a.Turma == b.TurmaReferenciada)) || string.IsNullOrEmpty(a.Turma)).ToList();


                rnCtvConfVaga.ProcessaListaEncadeada(listaPaisVagas, listaTotalVagas, mensagensParcial, finalizar, turmas, PerfilUsuario, codPerfil, listaVagasSalvas);


                var listaDeadLock = listaTotalVagas.Where(a => !a.Validado).ToList();
                rnCtvConfVaga.ProcessaListaEncadeada(listaDeadLock, listaTotalVagas, mensagensParcial, finalizar, turmas, PerfilUsuario, codPerfil, listaVagasSalvas);

                //Atualiza hiddens das turmas que foram salvas
                this.AtualizaHiddensQuadroSalas(listaVagasSalvas);

                //Verifica se foram encontrados erros
                if (mensagensParcial.Count > 0)
                {
                    string mensagens = mensagensParcial.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
                    lblMensagemVagas.Text = mensagens.Replace(Environment.NewLine, "<br />");
                }

                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
                lblMensagemVagas.Text = lblMensagemVagasBottom.Text;

                return (String.IsNullOrEmpty(lblMensagemVagas.Text));
            }
            catch (Exception ex)
            {
                lblMensagemVagas.Text = ex.Message;
                //Atualiza hiddens das turmas que foram salvas
                this.AtualizaHiddensQuadroSalas(listaVagasSalvas);
                return false;
            }
        }

        protected void AtualizaHiddensQuadroSalas(List<DadosVagaSalva> listaTotalVagas)
        {
            foreach (var vaga in listaTotalVagas)
            {
                AtualizaHidden(vaga);
            }
        }

        protected void AtualizaHidden(DadosVagaSalva vaga)
        {
            //Atualiza hiddens: hdnTurmaFilha e hdnID da celula que foi salva
            var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x.ToString()).ToArray();
            for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
            {
                for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                {
                    var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna][0]);
                    var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                    var currentHiddenID = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                        String.Format("hdnID{0}", turnos[indiceColuna])) as HiddenField);

                    var currentHiddenTurmaFilha = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                      String.Format("hdnTurmaFilha{0}", turnos[indiceColuna])) as HiddenField);

                    var sala = gridSalas.GetRowValues(indiceLinha, "SalaCapacidade").ToString().Split('[').First();

                    var turno = turnos[indiceColuna][0].ToString();

                    if (vaga.Sala == sala && vaga.Turno == turno &&
                        (vaga.Turma == currentDropDown.SelectedValue || (vaga.Turma.IsNullOrEmptyOrWhiteSpace() && currentDropDown.SelectedIndex == 0)))
                    {
                        currentHiddenID.Value = vaga.IdSalvo;
                        currentHiddenTurmaFilha.Value = vaga.Turma;
                        return;
                    }
                }
            }

        }

        protected string ValidaMinimoVagas()
        {
            DadosTurmaConfVaga dadosTurma = new DadosTurmaConfVaga();
            DadosVagasPorTurma vagasTurma = new DadosVagasPorTurma();
            List<DadosVagasPorTurma> listaVagasTurma = new List<DadosVagasPorTurma>();
            int periodo = -1;
            var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x.ToString()).ToArray();
            var ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
            RN.CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
            StringBuilder mensagemFormatada = new StringBuilder();
            RN.Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new RN.Agenda.ParametroTurnoVaga();
            RN.Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new RN.Agenda.Entidades.ParametroTurnoVaga();
            RN.Agenda.Entidades.ParametroTurnoVaga parametroPadrao = new RN.Agenda.Entidades.ParametroTurnoVaga();
            bool ehUsuarioPadrao = false;
            RN.Agenda.ParametroMinimoVaga rnParametroMinimoVaga = new RN.Agenda.ParametroMinimoVaga();
            List<RN.Agenda.Entidades.ParametroMinimoVaga> listaParametroMinimoVaga = new List<RN.Agenda.Entidades.ParametroMinimoVaga>();
            List<RN.Agenda.Entidades.ParametroMinimoVaga> listaParametroMinimoPadrao = new List<RN.Agenda.Entidades.ParametroMinimoVaga>();
            int codPerfil = -1;
            int agendaId = 0;
            int tipoSerie = 1; //0-Série de Entrada, 1-Demais Séries - começa como demais series
            int VN = Convert.ToInt32(RN.Agenda.ParametroMinimoVaga.TipoVaga.VN);
            int minimoNova = 0;
            bool possuiTurnoNovo = false;
            string censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
            bool serieEntrada = true;

            //Carrega perfil do usuario logado
            VerificaPerfil();
            codPerfil = Convert.ToInt32(Session["codPerfil"]);

            //Busca agendaId não encerrado
            agendaId = this.Confirmacao.Where(x => x.Editavel).Max(y => y.AgendaID);

            if (agendaId > 0)
            {
                //Verifica a parametrização daquele perfil
                parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, agendaId);

                //Verifica Minimos de vagas para a agenda e perfil
                listaParametroMinimoVaga = rnParametroMinimoVaga.ObtemPor(agendaId, codPerfil);

                //Verifica se o usuario possui configuração padrao como parametro
                if (parametroTurnoVaga.ConfiguracaoPadrao)
                {
                    ehUsuarioPadrao = true;
                }
                else
                {
                    listaParametroMinimoPadrao = rnParametroMinimoVaga.ObtemPadraoPor(agendaId);
                    parametroPadrao = rnParametroTurnoVaga.ObtemPadraoPor(agendaId);
                    ehUsuarioPadrao = false;
                }

                //Percorre grid de Salas
                for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
                {
                    for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                    {
                        var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna][0]);
                        var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                        var currentHiddenID = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                            String.Format("hdnID{0}", turnos[indiceColuna])) as HiddenField);

                        var currentHiddenEditavel = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                            String.Format("hdnEditavel{0}", turnos[indiceColuna])) as HiddenField);

                        nomeControle = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna][0]);
                        var currentTextBoxVC = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        nomeControle = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna][0]);
                        var currentTextBoxVN = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        if (currentDropDown.SelectedIndex != 0)
                        {
                            var editavel = !String.IsNullOrEmpty(currentHiddenEditavel.Value) ?
                                    Convert.ToBoolean(currentHiddenEditavel.Value) : true;

                            if (editavel)
                            {
                                var turma = currentDropDown.SelectedValue;
                                dadosTurma = this.Turmas.Where(x => x.Turma == turma && x.DescricaoTurma == currentDropDown.SelectedItem.Text).Single();
                                periodo = dadosTurma.Periodo;
                                var sala = gridSalas.GetRowValues(indiceLinha, "SalaCapacidade").ToString().Split('[').First();

                                var dadosTurmaAgenda = CtvAgendaConfTurnoVaga.RetornaDadosTurma(ano, periodo, censo, dadosTurma.Turma);
                                if (dadosTurmaAgenda == null || dadosTurmaAgenda.IdAgenda <= 0)
                                {
                                    //Se não encontrar turma no ano/periodo referencia, ver turma provisoria
                                    dadosTurmaAgenda = CtvAgendaConfTurnoVaga.RetornaDadosTurmaProvisoria(ano, periodo, censo, turma);
                                }

                                vagasTurma = new DadosVagasPorTurma();
                                vagasTurma.Periodo = periodo;
                                vagasTurma.Turma = turma;
                                vagasTurma.Sala = sala;
                                vagasTurma.VagasNovas = !String.IsNullOrEmpty(currentTextBoxVN.Text) ? Convert.ToInt32(currentTextBoxVN.Text) : 0;
                                vagasTurma.VagasContinuidade = !String.IsNullOrEmpty(currentTextBoxVC.Text) ? Convert.ToInt32(currentTextBoxVC.Text) : 0;
                                vagasTurma.Turno = turnos[indiceColuna][0].ToString();
                                vagasTurma.Curso = dadosTurmaAgenda.Curso;
                                vagasTurma.DescricaoCurso = dadosTurmaAgenda.NomeCurso;
                                vagasTurma.Serie = dadosTurmaAgenda.Serie;

                                listaVagasTurma.Add(vagasTurma);
                            }
                        }
                    }
                }

                //Monta Lista por turnos para validação de minimo de vagas
                List<DadosTurnoHabilitado> listaPorTurno = this.ObtemTurnosNovosHabilitados();

                //Para cada turno
                foreach (var item in listaPorTurno)
                {
                    serieEntrada = Convert.ToBoolean(this.Confirmacao.Where(x => x.Periodo == item.Periodo && x.Curso == item.Curso && x.Serie == item.Serie.ToString()).Select(x => x.SerieEntrada).First());

                    int vagasNovas = 0;
                    //Verifica se o curso / serie são de entrada
                    if (serieEntrada)
                    {
                        tipoSerie = 0; // 0-Série de Entrada, 1-Demais Séries
                    }
                    else
                    {
                        tipoSerie = 1;
                    }

                    //Busca agendaId não encerrado
                    int IdAgendaConfTurnoVaga = this.Confirmacao.Where(x => x.Editavel && x.Periodo == item.Periodo && x.Curso == item.Curso && x.Serie == item.Serie.ToString()).Max(y => y.IDAgenda);

                    //Verifica se existe algum turno lançado para o curso / serie
                    possuiTurnoNovo = rnCtvConfTurno.ExisteTurnoPor(IdAgendaConfTurnoVaga, censo, true, false);

                    //Verificar se existe algum turno lançado como novo para validação do minimo
                    if (possuiTurnoNovo)
                    {
                        //Busca vagas novas
                        vagasNovas = listaVagasTurma.Where(x => x.Periodo == item.Periodo && x.Curso == item.Curso && x.Turno == item.Turno && x.Serie == item.Serie).Sum(x => x.VagasNovas);

                        //Verifica se existe parametrização para o perfil
                        if (listaParametroMinimoVaga.Count > 0)
                        {
                            //Verificar o numero minimo de nova
                            minimoNova = listaParametroMinimoVaga.Where(x => x.TipoSerieId == tipoSerie && x.TipoVagaId == VN).Min(y => y.QuantidadeVagas);

                            //Verifica se a quantidade escolhida é menor que o minimo para vagas novas
                            if (vagasNovas < minimoNova)
                            {
                                mensagemFormatada.Append(string.Format("- A quantidade de vagas novas do curso: {1}, ano escolaridade: {2}, turno: {3}, é inferior ao mínimo {0}.\\r\\n",
                                           minimoNova.ToString(),
                                           item.DescricaoCurso,
                                           item.Serie,
                                           GetNameFromTurno(item.Turno)));
                            }
                        }

                        //Verifica o usuario não possui o parametro padrao
                        if (!ehUsuarioPadrao)
                        {
                            //Verifica se existe percentual de diminuição de vaga padrao
                            if (listaParametroMinimoPadrao.Count > 0)
                            {
                                //Verificar o numero minimo de nova para o padrao
                                minimoNova = listaParametroMinimoPadrao.Where(x => x.TipoSerieId == tipoSerie && x.TipoVagaId == VN).Min(y => y.QuantidadeVagas);

                                //Verifica se a quantidade escolhida é menor que o minimo para vagas novas do padrao
                                if (vagasNovas < minimoNova)
                                {
                                    mensagemFormatada.Append(string.Format("- A quantidade de vagas novas do curso: {1}, ano escolaridade: {2}, turno: {3}, é inferior ao mínimo {0}.\\r\\n",
                                               minimoNova.ToString(),
                                               item.DescricaoCurso,
                                               item.Serie,
                                               GetNameFromTurno(item.Turno)));
                                }
                            }

                        }
                    }
                }

                if (!string.IsNullOrEmpty(mensagemFormatada.ToString()))
                {
                    return mensagemFormatada.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                lblMensagemVagas.Text = "Erro: Não foi possivel encontrar o valor do IdAgendaConfTurnoVaga.\\r\\n";
                return lblMensagemVagas.Text;
            }
        }

        protected bool ValidaChoqueTurnoIntegral()
        {
            DadosTurmaConfVaga dadosTurma = new DadosTurmaConfVaga();
            DadosVagasPorTurma vagasTurma = new DadosVagasPorTurma();
            List<DadosVagasPorTurma> listaVagasTurma = new List<DadosVagasPorTurma>();
            int periodo = -1;
            var turnos = Enum.GetNames(typeof(Turnos)).Select(x => x.ToString()).ToArray();
            var ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;
            StringBuilder mensagemFormatada = new StringBuilder();
            int agendaId = 0;
            string censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
            bool choqueTurnoIntegral = false;
            RN.Curso rnCurso = new Curso();
            string cursoTurnoIntegral = string.Empty;

            //Busca agendaId não encerrado
            agendaId = this.Confirmacao.Where(x => x.Editavel).Max(y => y.AgendaID);

            if (agendaId > 0)
            {
                //Percorre grid de Salas
                for (int indiceLinha = 0; indiceLinha < gridSalas.VisibleRowCount; indiceLinha++)
                {
                    for (int indiceColuna = 0; indiceColuna < gridSalas.Columns.Count - 1; indiceColuna++)
                    {
                        var nomeControle = String.Format("{0}_{1}", comboTurmaNome, turnos[indiceColuna][0]);
                        var currentDropDown = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as DropDownList);

                        var currentHiddenID = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                            String.Format("hdnID{0}", turnos[indiceColuna])) as HiddenField);

                        var currentHiddenEditavel = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1,
                            String.Format("hdnEditavel{0}", turnos[indiceColuna])) as HiddenField);

                        nomeControle = String.Format("{0}_{1}", textBoxVCNome, turnos[indiceColuna][0]);
                        var currentTextBoxVC = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        nomeControle = String.Format("{0}_{1}", textBoxVNNome, turnos[indiceColuna][0]);
                        var currentTextBoxVN = (GetControleDaGridPorNome(indiceLinha, indiceColuna + 1, nomeControle) as TextBox);

                        if (currentDropDown.SelectedIndex != 0)
                        {
                            var editavel = !String.IsNullOrEmpty(currentHiddenEditavel.Value) ?
                                    Convert.ToBoolean(currentHiddenEditavel.Value) : true;

                            if (editavel)
                            {
                                var turma = currentDropDown.SelectedValue;
                                dadosTurma = this.Turmas.Where(x => x.Turma == turma && x.DescricaoTurma == currentDropDown.SelectedItem.Text).Single();
                                periodo = dadosTurma.Periodo;
                                var sala = gridSalas.GetRowValues(indiceLinha, "SalaCapacidade").ToString().Split('[').First();

                                var dadosTurmaAgenda = CtvAgendaConfTurnoVaga.RetornaDadosTurma(ano, periodo, censo, dadosTurma.Turma);
                                if (dadosTurmaAgenda == null || dadosTurmaAgenda.IdAgenda <= 0)
                                {
                                    //Se não encontrar turma no ano/periodo referencia, ver turma provisoria
                                    dadosTurmaAgenda = CtvAgendaConfTurnoVaga.RetornaDadosTurmaProvisoria(ano, periodo, censo, turma);
                                }

                                vagasTurma = new DadosVagasPorTurma();
                                vagasTurma.Periodo = periodo;
                                vagasTurma.Turma = turma;
                                vagasTurma.Sala = sala;
                                vagasTurma.VagasNovas = !String.IsNullOrEmpty(currentTextBoxVN.Text) ? Convert.ToInt32(currentTextBoxVN.Text) : 0;
                                vagasTurma.VagasContinuidade = !String.IsNullOrEmpty(currentTextBoxVC.Text) ? Convert.ToInt32(currentTextBoxVC.Text) : 0;
                                vagasTurma.Turno = turnos[indiceColuna][0].ToString();
                                vagasTurma.Curso = dadosTurmaAgenda.Curso;
                                vagasTurma.DescricaoCurso = dadosTurmaAgenda.NomeCurso;
                                vagasTurma.Serie = dadosTurmaAgenda.Serie;

                                listaVagasTurma.Add(vagasTurma);
                            }
                        }
                    }
                }

                //Monta lista por sala para validação de turno Integral
                var listaPorSala = listaVagasTurma.Select(x => new { x.Sala }).Distinct();

                //Para cada Sala
                foreach (var item in listaPorSala)
                {
                    //Busca curso que está no turno integral nesta sala
                    var cursosIntegral = listaVagasTurma.Where(x => x.Sala == item.Sala && x.Turno == "I").Select(x => x.Curso).ToList();

                    if (cursosIntegral.Count > 0)
                    {
                        cursoTurnoIntegral = cursosIntegral.First();

                        //Verifica se o curso do turno integral permitem choque com outros turnos                         
                        choqueTurnoIntegral = rnCurso.PermiteChoqueTurnoIntegralPor(cursoTurnoIntegral);

                        if (!choqueTurnoIntegral)
                        {
                            //Caso não permita choque verifica se existe turnos Manha e/ou Tarde
                            //Busca curso que está no turno integral nesta sala
                            if (listaVagasTurma.Where(x => x.Sala == item.Sala && (x.Turno == "M" || x.Turno == "T")).ToList().Count() > 0)
                            {
                                mensagemFormatada.Append(string.Format("- O Curso do turno Integral da sala {0} não permite choque com turnos Manha e/ou Tarde.\\r\\n",
                                    item.Sala));
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(mensagemFormatada.ToString()))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "fecharPopupConfirmacaoValidacaoVagas();fecharPopupVagasOfertadas();", true);
                    lblMensagemVagas.Text = mensagemFormatada.ToString().Replace("\\r\\n", "<br />");
                    lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
                    return false;

                }
                else
                {
                    return true;
                }
            }
            else
            {
                lblMensagemVagas.Text = "Erro: Não foi possivel encontrar o valor do IdAgendaConfTurnoVaga.\\r\\n";
                return false;
            }
        }

        protected void LimparCamposTurmaNovaPopup()
        {
            ddlTurmaNovaPeriodo.Items.Clear();
            ddlTurmaNovaModalidade.Items.Clear();
            ddlTurmaNovaSerie.Items.Clear();
            ddlTurmaNovaCurso.Items.Clear();
            txtTurmaNova.Text = string.Empty;
            lblTurmaNovaSetor.Text = string.Empty;
            lblTurmaNovaSeriePrefixo.Text = string.Empty;
            lblTurmaNovaMensagem.Text = string.Empty;
        }

        #endregion

        protected void VerificaInconsistenciaSalaVaga(int ano, string censo)
        {
            try
            { 
                lblInconsistenciaSala.Visible = false;
                var turmas = new List<string>();
                RN.CtvConfVaga rnCtvConfVaga = new CtvConfVaga();

                turmas = rnCtvConfVaga.ListaTurmaEmSalaInativaPor(ano,censo);

                if (turmas.Count() > 0)
                {
                    lblInconsistenciaSala.Text = "AS TURMAS LISTADAS ABAIXO, FORAM PLANEJADAS EM SALAS DE AULA QUE ATUALMENTE ESTÃO INATIVAS. FAVOR, AVALIAR A NECESSIDADE DE AJUSTES. <br />" + turmas.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                    lblInconsistenciaSala.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}

