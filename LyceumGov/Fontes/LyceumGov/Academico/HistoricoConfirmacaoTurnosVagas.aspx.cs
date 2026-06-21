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


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/HistoricoConfirmacaoTurnosVagas.aspx"), ControlText("HistoricoConfirmacaoTurnosVagas"), Title("Histórico de Confirmação de Turnos e Vagas")]

    public partial class HistoricoConfirmacaoTurnosVagas : TPage
    {
        #region Variaveis de Sessao

        // Itens do DropDownList (fixo)
        public List<DadosTurmaConfVaga> Turmas
        {
            get { return Page.Session["turmasHistorico"] as List<DadosTurmaConfVaga>; }
            set { (Page.Session["turmasHistorico"]) = value; }
        }

        // Itens do da turma utilizados
        public List<int> IndicesUtilizados
        {
            get { return Page.Session["indicesHistorico"] as List<int>; }
            set { (Page.Session["indicesHistorico"]) = value; }
        }

        //Recebe uma lista de Confirmação de Vagas. Atualizada de acordo com as mudanças na grid de vagas
        public List<DadosAgendaVagas> Confirmacao
        {
            get { return Page.Session["confirmacaoHistorico"] as List<DadosAgendaVagas>; }
            set { (Page.Session["confirmacaoHistorico"]) = value; }
        }

        //Recebe uma lista de DadosConfVaga.
        public List<DadosConfVaga> ListagemSalas
        {
            get { return Page.Session["listagemHistorico"] as List<DadosConfVaga>; }
            set { (Page.Session["listagemHistorico"]) = value; }
        }

        #endregion

        #region Variáveis ReadOnly

        public readonly string textBoxTurmaNome = "txtHistTurmas";
        public readonly string textBoxVCNome = "txtVC";
        public readonly string textBoxVNNome = "txtVN";
        public readonly string labelVCNome = "lblVCM";
        public readonly string labelVNNome = "lblVN";
        public static int contador = 0;

        #endregion

        #region Enums

        public enum Turnos
        {
            Manha = 1,
            Tarde = 2,
            Noite = 3,
            Ampliado = 4,
            Integral = 5
        }

        #endregion

        public object ListarTurnos(object unidadeEns, object ano, object tipoHistorico)
        {
            string ue = unidadeEns.ToString();

            RN.TurnosVagas.HistoricoTurno rnHistoricoTurno = new Techne.Lyceum.RN.TurnosVagas.HistoricoTurno();

            if (!string.IsNullOrEmpty(ano.ToString()) && !string.IsNullOrEmpty(ue) && tipoHistorico != null)
            {
                return rnHistoricoTurno.ListaQuadroHistoricoTurnos(ue, int.Parse(ano.ToString()), int.Parse(tipoHistorico.ToString()));
            }

            return null;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdConfTurnos, "Turnos");
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

                if (!this.Page.IsPostBack)
                {
                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);
                    RN.Agenda.PeriodoLetivoAgenda rnPeriodoLetivoAgenda = new RN.Agenda.PeriodoLetivoAgenda();
                    RN.TurnosVagas.TipoHistorico rnTipoHistorico = new Techne.Lyceum.RN.TurnosVagas.TipoHistorico();

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

                    ddlTipoHistorico.Items.Clear();
                    ddlTipoHistorico.DataSource = rnTipoHistorico.ListaTiposHistoricoAtivos();
                    ddlTipoHistorico.DataBind();
                    ListItem lsT = new ListItem("Selecione", string.Empty);
                    ddlTipoHistorico.Items.Insert(0, lsT);

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("DIESP") + "'").Length > 0 && Session["perfil"].ToString() != "privilegiado")
                    {
                        tseRegional.SqlWhere = " s.id_regional = 5";
                        tseMunicipio.SqlWhere = " id_regional = 5";
                        tseUnidadeResponsavel.SqlWhere = " id_regional = 5 AND municipio = #tseMunicipio# and situacao = 'ESTADUAL'";
                    }
                    else if (Session["perfil"].ToString() != "privilegiado")
                    {
                        tseRegional.SqlWhere = " s.id_regional <> 5";
                    }

                    this.pnGridVagas.Visible = false;
                    this.pnTurmas.Visible = false;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                        PreencheDadosQueryString(decodedText);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void PreencheDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            int tipoHistorico = (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Diretor;

            try
            {
                foreach (string dados in listaDados)
                {
                    if (dados.IndexOf("ano=") >= 0)
                    {
                        ddlAno.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        ddlAno_SelectedIndexChanged(null, null);

                        //Verificar se existe confirmação do periodo 2 para aquele ano
                        if (ddlPeriodo.Items.Count > 2)
                        {
                            //Caso exista selecionar periodo 2
                            ddlPeriodo.SelectedValue = "2";
                        }
                        else
                        {
                            ddlPeriodo.SelectedValue = "0, 1";
                        }

                        //Seleciona tipo historico diretor
                        ddlTipoHistorico.SelectedValue = tipoHistorico.ToString();
                    }

                    if (dados.IndexOf("unidadeEnsino=") >= 0)
                    {
                        tseUnidadeResponsavel.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                        tseUnidadeResponsavel_Changed(null, null);
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
                lblMensagemFinalizarTurno.Text = string.Empty;
                lblMensagemFinalizarVagas.Text = string.Empty;
                CtvFinalizado nrCtvFinalizado = new CtvFinalizado();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlTipoHistorico.SelectedValue))
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
                    RN.TurnosVagas.HistoricoTurnoVaga rnHistoricoTurnoVaga = new Techne.Lyceum.RN.TurnosVagas.HistoricoTurnoVaga();

                    if (!rnHistoricoTurnoVaga.PossuiHistoricoTurnoPor(int.Parse(ddlTipoHistorico.SelectedValue), int.Parse(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString()))
                    {
                        lblMensagem.Text = "Não existe histórico para o tipo de lançamento selecionado.";
                        pnTurnos.Visible = false;
                        pnGridVagas.Visible = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        #region Filtros

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                Turmas = null;
                ListagemSalas = null;
                contador = 0;
                Session["CursosNaoParticipamVagasHistorico"] = null;

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
                ListagemSalas = null;
                contador = 0;
                Session["CursosNaoParticipamVagasHistorico"] = null;

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
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            RN.TurnosVagas.HistoricoVaga rnHistoricoVaga = new Techne.Lyceum.RN.TurnosVagas.HistoricoVaga();
            RN.TurnosVagas.HistoricoTurnoVaga rnHistoricoTurnoVaga = new RN.TurnosVagas.HistoricoTurnoVaga();

            try
            {
                Turmas = null;
                ListagemSalas = null;
                contador = 0;
                var dt = (DataTable)Session["perfis"];
                Session["CursosNaoParticipamVagasHistorico"] = null;

                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
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

                        if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && !string.IsNullOrEmpty(ddlTipoHistorico.SelectedValue))
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

                            this.PreencheDadosAnaliseTurnos(Convert.ToInt32(ddlAno.SelectedValue),
                                                 tseUnidadeResponsavel.DBValue.ToString());

                            this.VerificaPerfil();

                            gridSalas.DataSource = null;
                            gridSalas.DataBind();

                            //Verifica se possui vagas lançadas
                            if (rnHistoricoTurnoVaga.PossuiHistoricoVagaDiretorPor(int.Parse(ddlTipoHistorico.SelectedValue), int.Parse(ddlAno.SelectedValue), ddlPeriodo.SelectedValue, tseUnidadeResponsavel.DBValue.ToString()))
                            {
                                this.InicializaVagas();
                            }
                            else
                            {
                                lblMensagem.Text = "Não existe histórico de vagas para o tipo de lançamento selecionado.";
                            }
                        }
                        else
                        {
                            grdConfTurnos.Visible = false;
                            lblMensagem.Text = "Os campos Ano/Período/Tipo de Lançamento são de preenchimento obrigatório.";
                            lblMensagemFinalizarTurno.Text = string.Empty;
                            lblMensagemFinalizarVagas.Text = string.Empty;
                            pnGridTurnos.Visible = false;
                            pnTurnos.Visible = false;
                            pnGridVagas.Visible = false;
                            pnTurmas.Visible = false;
                            pnlAnaliseTurnos.Visible = false;
                            pnlAnaliseVagas.Visible = false;
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
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Turmas = null;
                ListagemSalas = null;
                contador = 0;
                Session["CursosNaoParticipamVagasHistorico"] = null;

                CtvAgendaConfTurnoVaga rnCtvAgendaConfTurnoVaga = new CtvAgendaConfTurnoVaga();
                ddlPeriodo.Items.Clear();
                ddlTipoHistorico.ClearSelection();
                tseRegional.ResetValue();
                tseMunicipio.ResetValue();
                tseUnidadeResponsavel.ResetValue();
                pnTurnos.Visible = false;
                pnGridVagas.Visible = false;

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    ddlPeriodo.DataSource = rnCtvAgendaConfTurnoVaga.ListaPeriodosParaHistoricoPor(int.Parse(ddlAno.SelectedValue));
                    ddlPeriodo.DataBind();
                    ListItem ls = new ListItem("Selecione", string.Empty);
                    ddlPeriodo.Items.Insert(0, ls);
                    CarregaUnidadeEnsino();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Turmas = null;
                ListagemSalas = null;
                contador = 0;
                Session["CursosNaoParticipamVagasHistorico"] = null;

                ddlTipoHistorico.ClearSelection();
                tseRegional.ResetValue();
                tseMunicipio.ResetValue();
                tseUnidadeResponsavel.ResetValue();
                pnTurnos.Visible = false;
                pnGridVagas.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTipoHistorico_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Turmas = null;
                ListagemSalas = null;
                contador = 0;
                Session["CursosNaoParticipamVagasHistorico"] = null;

                tseRegional.ResetValue();
                tseMunicipio.ResetValue();
                tseUnidadeResponsavel.ResetValue();
                pnTurnos.Visible = false;
                pnGridVagas.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        private void CarregaUnidadeEnsino()
        {
            //Cria listagem da tseUnidadeEns
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;
            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
            tseUnidadeResponsavel.SqlSelect = sqlSelect;
            tseUnidadeResponsavel.ResetValue();

            if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                //Esta view lista todos as escolas considerando as permissoes usuario
                table = @" VW_UNIDADE_ENSINO_SITUACAO S
                                INNER JOIN dbo.TCE_CTV_CONF_TURNO_INICIAL TI ON S.UNIDADE_ENS=TI.CENSO
                                INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON TI.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                ";

                if (Session["perfil"].ToString() == "privilegiado" || Session["perfil"].ToString() == "DIRETOR_UE" || Session["perfil"].ToString() == "REGIONAL")
                {
                    tseUnidadeResponsavel.SqlWhere = " situacao = 'ESTADUAL' AND s.id_regional =  #tseRegional# AND municipio = #tseMunicipio# AND ANO = " + ddlAno.SelectedValue + " ";
                }
                else
                {
                    if (Session["perfil"].ToString() == "DIESP" && Session["perfil"].ToString() != "privilegiado")
                    {
                        tseRegional.SqlWhere = " s.id_regional = 5";
                        tseMunicipio.SqlWhere = " id_regional = 5";
                        tseUnidadeResponsavel.SqlWhere = " s.id_regional = 5 AND municipio = #tseMunicipio# and situacao = 'ESTADUAL' AND ANO = " + ddlAno.SelectedValue + " ";
                    }
                    if (Session["perfil"].ToString() != "privilegiado" && Session["perfil"].ToString() != "DIESP")
                    {
                        tseRegional.SqlWhere = " s.id_regional <> 5";
                        tseUnidadeResponsavel.SqlWhere = " situacao = 'ESTADUAL' AND s.id_regional <> 5 AND id_regional =  #tseRegional# AND municipio = #tseMunicipio# AND ANO = " + ddlAno.SelectedValue + " ";
                    }
                }


                coluna.Add("unidade_ens");
                coluna.Add("nome_comp");
                coluna.Add("setor");
                coluna.Add("id_regional");
                coluna.Add("municipio");
                coluna.Add("ua_atual");
                coluna.Add("ua_antiga");

                sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseUnidadeResponsavel.SqlSelect = sqlSelect;
                tseUnidadeResponsavel.DataBind();
            }
        }

        #endregion

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


        public void grdConfTurnos_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data
                || !this.grdConfTurnos.Visible
                || this.grdConfTurnos.VisibleRowCount == 0)
            {
                return;
            }
            var txtJustificativa = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, e.VisibleIndex, "Justificativa", "txtJustificativa");
            var txtJustificativaNovo = DevExpressHelper.GetControl<TextBox>(this.grdConfTurnos, e.VisibleIndex, "JustificativaNovo", "txtJustificativaNovo");

            if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !this.tseUnidadeResponsavel.DBValue.IsNull && this.grdConfTurnos.VisibleRowCount == 0 && e.RowType != GridViewRowType.Data)
            {
                pnlAnaliseTurnos.Visible = false;
                pnGridTurnos.Visible = false;
                pnTurnos.Visible = false;
                lblMensagem.Text = "Não existem séries cadastradas para o ano/periodo referência";
                return;
            }

            e.Row.Enabled = false;
            txtJustificativa.ToolTip = txtJustificativa.Text;
            txtJustificativaNovo.ToolTip = txtJustificativaNovo.Text;


        }

        protected void gridSalas_onHtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (IsPostBack || (!IsPostBack && (Request.QueryString["Chave"] != null)))
                {
                    if (ListagemSalas != null && contador < ListagemSalas.Count && e.RowType == GridViewRowType.Data)
                    {
                        var linhaIndice = e.VisibleIndex;

                        // caso turma esteja vazio: representa que aquela sala ainda nao teve associação de dados,
                        // então adiciono como vazio a célula para todos os turnos
                        if (String.IsNullOrEmpty(ListagemSalas[contador].Turma))
                        {
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
                                if ((ListagemSalas != null && contador < ListagemSalas.Count))
                                {
                                    AdicionaCelulaComDado(linhaIndice, ListagemSalas[contador], GetNameFromTurno(ListagemSalas[contador].Turno));
                                    contador++;
                                }
                            }
                        }
                    }

                    if (ListagemSalas != null && contador == ListagemSalas.Count && (Turmas.Count > 0))
                    {
                        contador = 0;
                    }
                }

                if (IsPostBack && Turmas != null && (Turmas.Count == 0))
                {
                    if (e.RowType == GridViewRowType.Data)
                    {
                        var linhaIndice = e.VisibleIndex;

                        if (ListagemSalas != null && contador < ListagemSalas.Count)
                        {
                            if (String.IsNullOrEmpty(ListagemSalas[contador].Turma))
                            {
                                contador++;
                            }

                            if (ListagemSalas != null && contador == ListagemSalas.Count)
                            {
                                contador = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void AdicionaCelulaComDado(int linhaIndice, DadosConfVaga listagem, string turno)
        {
            try
            {
                var nome = String.Format("{0}_{1}", textBoxVCNome, turno[0]);
                var currentTextBoxVC = DevExpressHelper.GetControl<TextBox>(this.gridSalas, linhaIndice, turno, nome);
                currentTextBoxVC.Text = listagem.VagasContinuidade.ToString();

                nome = String.Format("{0}_{1}", textBoxVNNome, turno[0]);
                var currentTextBoxVN = DevExpressHelper.GetControl<TextBox>(this.gridSalas, linhaIndice, turno, nome);
                currentTextBoxVN.Text = listagem.VagasNova.ToString();

                nome = String.Format("{0}_{1}", textBoxTurmaNome, turno[0]);
                var currentTextBoxTurmas = DevExpressHelper.GetControl<TextBox>(this.gridSalas, linhaIndice, turno, nome);
                currentTextBoxTurmas.Text = listagem.Turma.ToString();
                currentTextBoxTurmas.ToolTip = listagem.Turma.ToString();

                nome = String.Format("hdnID{0}", turno);
                var currentHiddenID = DevExpressHelper.GetControl<HiddenField>(this.gridSalas, linhaIndice, turno, nome);
                currentHiddenID.Value = listagem.IdCtvConfVaga.ToString();

                nome = String.Format("hdnEditavel{0}", turno);
                var currentHiddenEditavel = DevExpressHelper.GetControl<HiddenField>(this.gridSalas, linhaIndice, turno, nome);
                currentHiddenEditavel.Value = listagem.Editavel.ToString();

                var index = GetIndexOfSelectedValue(listagem.Turma);
                IndicesUtilizados.Add(index);

                nome = String.Format("{0}_{1}", nome, linhaIndice);

                //if (Valores.Count(x => x.Key == nome) == 0)
                //{
                //    //Verifica se aquele controle já exisite
                //    if (!Controle.ContainsKey(nome))
                //    {
                //        Controle.Add(nome, new int[] { index, Convert.ToInt32(currentTextBoxVC.Text), Convert.ToInt32(currentTextBoxVN.Text) });
                //    }
                //}
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
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

        protected void gridVagas_onHtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data
               || !this.grdConfTurnos.Visible
               || this.grdConfTurnos.VisibleRowCount == 0)
            {
                return;
            }

            var txtJustificativaNova = DevExpressHelper.GetControl<TextBox>(this.gridVagas, e.VisibleIndex, "JustificativaNova", "txtJFNova");

            e.Row.Enabled = false;
            txtJustificativaNova.ToolTip = txtJustificativaNova.Text;

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

        protected void PreencheDadosAnaliseTurnos(int ano, string censo)
        {
            try
            {
                CtvAnalise rnCtvAnalise = new CtvAnalise();
                List<DadosAnalise> listaAnalises = new List<DadosAnalise>();
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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
            }
        }

        private void preencheDadosAnaliseVagas(int ano, string censo)
        {
            try
            {
                CtvAnalise rnCtvAnalise = new CtvAnalise();
                List<DadosAnalise> listaAnalises = new List<DadosAnalise>();

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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemVagas.Text = ex.Message;
                lblMensagemVagasBottom.Text = lblMensagemVagas.Text;
            }
        }

        private void InicializaVagas()
        {
            try
            {
                int codPerfil = -1;
                contador = 0;
                int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);
                RN.CtvConfVaga rnCtvConfVaga = new CtvConfVaga();
                RN.CtvConfTurno rnCtvConfTurno = new CtvConfTurno();
                RN.TurnosVagas.HistoricoVaga rnHistoricoVaga = new Techne.Lyceum.RN.TurnosVagas.HistoricoVaga();
                RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
                pnGridVagas.Enabled = true;

                this.VerificaPerfil();

                if (!string.IsNullOrEmpty(Session["codPerfil"].ToString()))
                {
                    codPerfil = Convert.ToInt32(Session["codPerfil"]);
                }

                var censo = !tseUnidadeResponsavel.DBValue.IsNull ? Convert.ToString(tseUnidadeResponsavel.DBValue) : null;
                int ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0;

                Confirmacao = new List<DadosAgendaVagas>();
                IndicesUtilizados = new List<int>();

                if (ano != 0 && !string.IsNullOrEmpty(censo) && !string.IsNullOrEmpty(ddlTipoHistorico.SelectedValue))
                {
                    var conf = rnHistoricoVaga.ListaQuadroHistoricoPropostaPor(ano, censo, int.Parse(ddlTipoHistorico.SelectedValue));

                    foreach (DataRow item in conf.Rows)
                    {
                        var dado = new DadosAgendaVagas
                        {
                            IDAgenda = Convert.ToInt32(item["id_agenda_conf_turno_vaga"]),
                            AgendaID = Convert.ToInt32(item["AGENDAID"]),
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
                            TaxaReprovacao = Convert.ToString(item["TAXAREPROVACAO"])
                        };

                        Confirmacao.Add(dado);
                    }

                    this.ListarVagas();
                    gridVagas.DataBind();

                    if (Session["CursosNaoParticipamVagasHistorico"] == null)
                    {
                        Session["CursosNaoParticipamVagasHistorico"] = rnAgenda.ObtemListaCursosNaoParticipantesPor(ObtemAgendaIdVagas());
                    }

                    List<int> periodos = ObtemPeriodosVigentes();

                    var listagem = rnHistoricoVaga.ListaQuadroHistoricoSalasPor(censo, ano, ddlPeriodo.SelectedValue, int.Parse(ddlTipoHistorico.SelectedValue))
                         .OrderBy(x => x.SalaCapacidade).ToList();

                    var listagemAgrupadaPorSala = listagem.GroupBy(x => x.SalaCapacidade).ToList()
                            .Select(x => x.First()).ToList();

                    ListagemSalas = listagem;

                    var turmas = rnCtvConfVaga.ListaTurmasParaLancamentoPor(censo, ano, periodos, (List<string>)Session["CursosNaoParticipamVagasHistorico"]);

                    Turmas = turmas;

                    if (listagemAgrupadaPorSala.Count > 0)
                    {
                        this.gridSalas.DataSource = listagemAgrupadaPorSala;
                        this.gridSalas.DataBind();
                    }
                    else
                    {
                        Turmas = null;
                    }

                    this.pnGridVagas.Visible = true;
                    pnTurmas.Visible = true;
                    this.preencheDadosAnaliseVagas(int.Parse(ddlAno.SelectedValue), tseUnidadeResponsavel.DBValue.ToString());
                }
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


        #endregion
    }
}
