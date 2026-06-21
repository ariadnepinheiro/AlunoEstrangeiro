using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Data;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Library;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using System.Collections.Generic;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.DTOs.Agenda;

namespace Techne.Lyceum.Net.Academico
{
    [
     NavUrl("~/Academico/EncerramentoAluno.aspx"),
      ControlText("EncerramentoAluno"),
      Title("Encerramento de Aluno"),
    ]

    public partial class EncerramentoAluno : TPage
    {
        #region Propriedades e Enumeradores

        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            EncerrarNovo,
            Encerrar,
            Reabrir
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        [Serializable]
        private class DadosEncerramento
        {
            public DadosEncerramento()
            {

            }

            private string _aluno;
            private string _turno;
            private string _nome_turno;
            private string _curso;
            private string _nome_curso;
            private string _curriculo;
            private string _ano_ingresso;
            private string _periodo_ingresso;
            private string _dt_encerramento;
            private string _dt_reabertura;
            private string _motivo;
            private string _nome_motivo;
            private string _instituicao;
            private string _nome_instituicao;
            private string _dt_colacao;
            private string _dt_diploma;
            private string _ano_encerramento;
            private string _periodo_encerramento;
            private string _causa;
            private string _nome_causa;
            private string _motivoreabertura;
            private string _serie;


            public string Aluno
            {
                get { return _aluno; }
                set { _aluno = value; }
            }

            public string Turno
            {
                get { return _turno; }
                set { _turno = value; }
            }

            public string Nome_Turno
            {
                get { return _nome_turno; }
                set { _nome_turno = value; }
            }

            public string Curso
            {
                get { return _curso; }
                set { _curso = value; }
            }

            public string Nome_Curso
            {
                get { return _nome_curso; }
                set { _nome_curso = value; }
            }

            public string Curriculo
            {
                get { return _curriculo; }
                set { _curriculo = value; }
            }

            public string Ano_ingresso
            {
                get { return _ano_ingresso; }
                set { _ano_ingresso = value; }
            }

            public string Periodo_ingresso
            {
                get { return _periodo_ingresso; }
                set { _periodo_ingresso = value; }
            }

            public string Dt_encerramento
            {
                get { return _dt_encerramento; }
                set { _dt_encerramento = value; }
            }

            public string Dt_reabertura
            {
                get { return _dt_reabertura; }
                set { _dt_reabertura = value; }
            }

            public string Motivo
            {
                get { return _motivo; }
                set { _motivo = value; }
            }

            public string Nome_Motivo
            {
                get { return _nome_motivo; }
                set { _nome_motivo = value; }
            }

            public string Instituicao
            {
                get { return _instituicao; }
                set { _instituicao = value; }
            }

            public string Nome_Instituicao
            {
                get { return _nome_instituicao; }
                set { _nome_instituicao = value; }
            }

            public string Dt_colacao
            {
                get { return _dt_colacao; }
                set { _dt_colacao = value; }
            }

            public string Dt_diploma
            {
                get { return _dt_diploma; }
                set { _dt_diploma = value; }
            }

            public string Ano_encerramento
            {
                get { return _ano_encerramento; }
                set { _ano_encerramento = value; }
            }

            public string Periodo_encerramento
            {
                get { return _periodo_encerramento; }
                set { _periodo_encerramento = value; }
            }

            public string Causa
            {
                get { return _causa; }
                set { _causa = value; }
            }

            public string Nome_Causa
            {
                get { return _nome_causa; }
                set { _nome_causa = value; }
            }
            public string Motivoreabertura
            {
                get { return _motivoreabertura; }
                set { _motivoreabertura = value; }
            }
            public string Serie
            {
                get { return _serie; }
                set { _serie = value; }
            }
        }

        private DadosEncerramento ObjetoEncerramento
        {
            get { return (DadosEncerramento)ViewState["ObjetoEncerramento"]; }
            set { ViewState["ObjetoEncerramento"] = value; }
        }



        private RN.EncerramentoAluno.DadosExecucao ObjetoExecucao
        {
            get { return (RN.EncerramentoAluno.DadosExecucao)ViewState["ObjetoExecucao"]; }
            set { ViewState["ObjetoExecucao"] = value; }
        }

        #endregion

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        private void InitializeComponent()
        {

        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDisciplinasAtivas, "Disciplinas Ativas");

            dtConfDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtConfDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtEncDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtEncDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtEncDataEncerramento.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtEncDataReabertura.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            tseAluno.ReadOnly = true;

            if (!IsPostBack)
            {
                ObjetoExecucao = new RN.EncerramentoAluno.DadosExecucao();
                _tipoOperacao = TipoOperacao.Inicial;
                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                    ObterDadosQueryString(decodedText);

                }
                else
                    Response.Redirect("ListarEncerramentoAluno.aspx");
                ControlarTipoOperacao();

            }
            odsDisciplinasAtivas.Select();
            grdDisciplinasAtivas.DataBind();
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDisciplinasAtivas);
            ControlaAcesso(btnEncerrar, AcaoControle.novo);
            ControlaAcesso(btnReabrir, AcaoControle.novo);
        }

        #region Métodos Tela
        private void ObterDadosQueryString(string queryString)
        {
             string ano = null;
            string periodo = null;
            string curso = null;
            string serie = null;
            hdnPessoa.Value = string.Empty;
            
            hdnCompartilhada.Value = string.Empty;
            ObjetoEncerramento = new DadosEncerramento();
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("aluno") >= 0)
                    ObjetoEncerramento.Aluno = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("turno") >= 0)
                    ObjetoEncerramento.Turno = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nome_turno") >= 0)
                    ObjetoEncerramento.Nome_Turno = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("curso") >= 0)
                    ObjetoEncerramento.Curso = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nome_curso") >= 0)
                    ObjetoEncerramento.Nome_Curso = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("curriculo") >= 0)
                    ObjetoEncerramento.Curriculo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("ano_ingresso") >= 0)
                    ObjetoEncerramento.Ano_ingresso = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("periodo_ingresso") >= 0)
                    ObjetoEncerramento.Periodo_ingresso = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("dt_encerramento") >= 0)
                    ObjetoEncerramento.Dt_encerramento = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("dt_reabertura") >= 0)
                    ObjetoEncerramento.Dt_reabertura = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("motivo") >= 0)
                    ObjetoEncerramento.Motivo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nome_motivo") >= 0)
                    ObjetoEncerramento.Nome_Motivo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("instituicao") >= 0)
                    ObjetoEncerramento.Instituicao = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nome_instituicao") >= 0)
                    ObjetoEncerramento.Nome_Instituicao = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("dt_colacao") >= 0)
                    ObjetoEncerramento.Dt_colacao = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("dt_diploma") >= 0)
                    ObjetoEncerramento.Dt_diploma = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("ano_encerramento") >= 0)
                    ObjetoEncerramento.Ano_encerramento = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("periodo_encerramento") >= 0)
                    ObjetoEncerramento.Periodo_encerramento = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("causa") >= 0)
                    ObjetoEncerramento.Causa = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nome_causa") >= 0)
                    ObjetoEncerramento.Nome_Causa = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("reabertura") >= 0)
                    ObjetoEncerramento.Motivoreabertura = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("pessoa") >= 0)
                    hdnPessoa.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("serie") >= 0)
                    ObjetoEncerramento.Serie = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("comp") >= 0)
                    hdnCompartilhada.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("anocp") >= 0)
                    ano = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("periodocp") >= 0)
                    periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("cursocp") >= 0)
                    curso = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("seriecp") >= 0)
                    serie = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("Operacao") >= 0)
                {
                    string tipoOperacao = dados.Substring(dados.LastIndexOf('=') + 1);

                    if (tipoOperacao == "ENCERRARNOVO")
                        _tipoOperacao = TipoOperacao.EncerrarNovo;
                    else if (tipoOperacao == "ENCERRAR")
                        _tipoOperacao = TipoOperacao.Encerrar;
                    else if (tipoOperacao == "VISUALIZAR")
                        _tipoOperacao = TipoOperacao.Consultar;
                }
            }

            if (hdnCompartilhada.Value == "S")
            {
                hdnAnoComp.Value = ano;              
                hdnPeriodoComp.Value = periodo;
                hdnCursoComp.Value = curso;
                hdnSerieComp.Value = serie;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.EncerrarNovo:
                    {
                        lblMensagem.Text = string.Empty;

                        ControlaBotoes();
                        tseAluno.DBValue = ObjetoEncerramento.Aluno;
                        tseAluno.ReadOnly = true;
                        pnEncerramentos.Visible = true;

                        dtEncDataEncerramento.Value = DateTime.Now;
                        CarregarDadosDrop(ddlEncAnoEncerramento.ID);
                        CarregarDadosDrop(ddlEncCausa.ID);
                        CarregarDadosDrop(ddlEncMotivo.ID);

                        //pnDados
                        Ly_aluno dtAluno = new Ly_aluno();
                        Ly_aluno.Row dadosAluno = dtAluno.NewRow();
                        dadosAluno.Aluno = ObjetoEncerramento.Aluno;

                        dadosAluno = RN.Aluno.ConsultarAluno(dadosAluno.Aluno);

                        if (dadosAluno != null)
                        {
                            DateTime dtUltEncerramento = RN.EncerramentoAluno.UltimaDataEncerramento(dadosAluno.Aluno);
                            if (dtUltEncerramento != DateTime.MinValue)
                                dtEncDataEncerramento.MinDate = dtUltEncerramento.Date;
                            PreencherDadosAluno(dadosAluno);
                            pnDados.Visible = true;
                            HabilitaCampos();
                        }
                        else
                        {
                            LimparTela();
                            pnDados.Visible = false;
                            pnEncerramentos.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        dtConfDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtConfDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataEncerramento.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataReabertura.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        break;
                    }
                case TipoOperacao.Encerrar:
                    {
                        ControlaBotoes();
                        pnEncerramentos.Visible = true;
                        tseAluno.DBValue = ObjetoEncerramento.Aluno;
                        tseAluno.ReadOnly = true;

                        //pnDados
                        Ly_aluno dtAluno = new Ly_aluno();
                        Ly_aluno.Row dadosAluno = dtAluno.NewRow();
                        dadosAluno.Aluno = ObjetoEncerramento.Aluno;

                        dadosAluno = RN.Aluno.ConsultarAluno(dadosAluno.Aluno);

                        if (dadosAluno != null)
                        {
                            PreencherDadosAluno(dadosAluno);
                            pnDados.Visible = true;
                            PreencherDadosEncerramento();
                            DesabilitaCampos();

                            if (hdnCompartilhada.Value == "S")
                            {                                
                                ddlAnoReabertura.SelectedValue = hdnAnoComp.Value;
                                ddlAnoReabertura_SelectedIndexChanged(null, null);
                                ddlPeriodoReabertura.SelectedValue = hdnPeriodoComp.Value;
                                ddlPeriodoReabertura_SelectedIndexChanged(null, null);
                                tseCurso.DBValue = hdnCursoComp.Value;
                                tseCurso_Changed(null, null);
                                
                                ddlAnoReabertura.Enabled = false;
                                ddlPeriodoReabertura.Enabled = false;
                                tseCurso.Enabled = false;
                                tseCurso.Mode = ControlMode.View;
                                tseCurso.ReadOnly = true;
                                cmbSerie.Enabled = false;
                            }
                        }
                        else
                        {
                            LimparTela();
                            pnDados.Visible = false;
                            pnEncerramentos.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        dtConfDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtConfDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataEncerramento.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataReabertura.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        //ControlaBotoes();
                        ddlMotivoReabertura.Enabled = false;
                        pnEncerramentos.Visible = true;
                        tseAluno.DBValue = ObjetoEncerramento.Aluno;
                        tseAluno.ReadOnly = true;

                        //pnDados
                        Ly_aluno dtAluno = new Ly_aluno();
                        Ly_aluno.Row dadosAluno = dtAluno.NewRow();
                        dadosAluno.Aluno = ObjetoEncerramento.Aluno;

                        dadosAluno = RN.Aluno.ConsultarAluno(dadosAluno.Aluno);

                        if (dadosAluno != null)
                        {
                            PreencherDadosAluno(dadosAluno);
                            pnDados.Visible = true;
                            PreencherDadosEncerramento();
                            DesabilitaCampos();
                            dtEncDataEncerramento.Enabled = false;
                            dtEncDataReabertura.Enabled = false;
                            btnReabrir.Visible = false;
                            btnEncerrar.Visible = false;
                        }
                        else
                        {
                            LimparTela();
                            pnDados.Visible = false;
                            pnEncerramentos.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        dtConfDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtConfDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataColacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataDiploma.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataEncerramento.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dtEncDataReabertura.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        break;
                    }
            }
        }

        private void ControlaBotoes()
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Agenda.PeriodoLetivoAgenda rnPeriodoLetivoAgenda = new Techne.Lyceum.RN.Agenda.PeriodoLetivoAgenda();
            int idEventoReaberturaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ReaberturaMatricula);


            if (RN.EncerramentoAluno.HabilitaEncerrar(ObjetoEncerramento.Aluno))
            {
                pnlReabertura.Visible = false;
                btnEncerrar.Visible = true;
                lblEncDataReabertura.Visible = false;
                dtEncDataReabertura.Visible = false;
                dtEncDataReabertura.ReadOnly = true;
                lblAnoReabertura.Visible = false;
                ddlAnoReabertura.Visible = false;
                lblPeriodoReabertura.Visible = false;
                ddlPeriodoReabertura.Visible = false;
            }
            else
            {
                btnEncerrar.Visible = false;
                dtEncDataEncerramento.Enabled = false;
            }

            if (RN.EncerramentoAluno.HabilitaReabrir(ObjetoEncerramento.Aluno))
            {
                DataTable dtAlunoAtivo = new DataTable();

                dtAlunoAtivo = RN.Aluno.ListaAlunoAtivoReabertura(ObjetoEncerramento.Aluno);

                if (dtAlunoAtivo.Rows.Count > 0)
                {
                    lblMensagem.Text = "Já existe um aluno ativo na rede com a matrícula " + dtAlunoAtivo.Rows[0]["ALUNO"].ToString() + ".";
                    btnReabrir.Visible = false;
                    return;
                }

                if (RN.EncerramentoAluno.PossuiEncerramentoPorDuplicidade(ObjetoEncerramento.Aluno))
                {
                    lblMensagem.Text = "Não é permitido a reabertura da matrícula do aluno devido estar encerrado por ”Duplicidade de Matrícula”.";
                    btnReabrir.Visible = false;
                    return;
                }

                pnlReabertura.Visible = true;
                btnReabrir.Visible = true;
                dtEncDataEncerramento.Enabled = false;
                lblAnoReabertura.Visible = true;
                ddlAnoReabertura.Visible = true;
                lblPeriodoReabertura.Visible = true;
                ddlPeriodoReabertura.Visible = true;
                if (ddlEncMotivo.SelectedValue == "PROV_DES")
                {
                    ddlMotivoReabertura.Enabled = true;
                }

               
                ControlaAcesso(btnReabrir, AcaoControle.novo);
            }
            else
            {
                btnReabrir.Visible = false;
                lblAnoReabertura.Visible = false;
                ddlAnoReabertura.Visible = false;
                lblPeriodoReabertura.Visible = false;
                ddlPeriodoReabertura.Visible = false;
                lblEncDataReabertura.Visible = false;
                dtEncDataReabertura.Visible = false;
                dtEncDataReabertura.ReadOnly = true;
                pnlReabertura.Visible = false;
                if (ddlEncMotivo.SelectedValue == "PROV_DES")
                {
                    ddlMotivoReabertura.Enabled = false;
                }
            }

        }

        /// <summary>
        /// Limpa todas as textbox e combobox.
        /// </summary>
        protected void LimparTela()
        {
            txtCurriculo.Text = string.Empty;
            txtCurso.Text = string.Empty;
            txtEncAnoIngresso.Text = string.Empty;
            txtEncCurriculo.Text = string.Empty;
            txtEncCurso.Text = string.Empty;
            txtEncPeriodoIngresso.Text = string.Empty;
            txtEncTurno.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtCodigoSerie.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtUniEnsino.Text = string.Empty;

            ddlEncAnoEncerramento.Items.Clear();
            ddlEncCausa.Items.Clear();
            ddlEncMotivo.Items.Clear();
            ddlEncPeriodoEncerramento.Items.Clear();
            CarregarDadosDrop(ddlEncAnoEncerramento.ID);
            CarregarDadosDrop(ddlEncCausa.ID);
            CarregarDadosDrop(ddlEncMotivo.ID);
            CarregarDadosDrop(ddlEncPeriodoEncerramento.ID);

            dtEncDataColacao.Text = string.Empty;
            dtEncDataDiploma.Text = string.Empty;
            dtEncDataEncerramento.Text = string.Empty;
            dtEncDataReabertura.Text = string.Empty;
        }

        /// <summary>
        /// Habilita todos os campos para edição
        /// </summary>
        protected void HabilitaCampos()
        {
            ddlEncAnoEncerramento.Enabled = true;
            ddlEncCausa.Enabled = true;
            ddlEncMotivo.Enabled = true;
            ddlEncPeriodoEncerramento.Enabled = true;

            dtEncDataColacao.ReadOnly = false;
            dtEncDataDiploma.ReadOnly = false;
        }

        /// <summary>
        /// Desabilita todos os campos para edição.
        /// </summary>
        protected void DesabilitaCampos()
        {
            ddlEncAnoEncerramento.Enabled = false;
            ddlEncCausa.Enabled = false;
            ddlEncMotivo.Enabled = false;
            ddlEncPeriodoEncerramento.Enabled = false;

            dtEncDataColacao.ReadOnly = true;
            dtEncDataDiploma.ReadOnly = true;

            dtEncDataColacao.ReadOnly = true;
            dtEncDataDiploma.ReadOnly = true;
            //dtEncDataEncerramento.ReadOnly = true;
            tseEncInstituicao.Mode = ControlMode.View;

        }

        protected void ddlConfMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlConfMotivo.Text == "TRANSFERENCIA")
            {
                tseConfInstituicao.Enabled = true;
                dtConfDataColacao.Text = string.Empty;
                dtConfDataDiploma.Text = string.Empty;
                dtConfDataColacao.Enabled = false;
                dtConfDataDiploma.Enabled = false;
            }
            else if (ddlConfMotivo.Text == "CONCLUSAO")
            {
                tseConfInstituicao.ResetValue();
                dtConfDataColacao.Enabled = true;
                dtConfDataDiploma.Enabled = true;
            }
            else
            {
                dtConfDataColacao.Value = null;
                dtConfDataDiploma.Value = null;
                dtConfDataColacao.Enabled = false;
                dtConfDataDiploma.Enabled = false;
                tseConfInstituicao.ResetValue();
            }
        }

        protected void ddlEncMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEncMotivo.Text == "TRANSFERENCIA")
            {
                tseEncInstituicao.Enabled = true;
                dtEncDataColacao.Text = string.Empty;
                dtEncDataDiploma.Text = string.Empty;
                dtEncDataColacao.Enabled = false;
                dtEncDataDiploma.Enabled = false;
            }
            else if (ddlEncMotivo.Text == "CONCLUSAO")
            {
                tseEncInstituicao.ResetValue();
                dtEncDataColacao.Enabled = true;
                dtEncDataDiploma.Enabled = true;
            }
            else
            {
                dtEncDataColacao.Value = null;
                dtEncDataDiploma.Value = null;
                dtEncDataColacao.Enabled = false;
                dtEncDataDiploma.Enabled = false;
                tseEncInstituicao.ResetValue();
            }
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosEncerramento()
        {
            RN.Agenda.PeriodoLetivoAgenda rnPeriodoLetivoAgenda = new Techne.Lyceum.RN.Agenda.PeriodoLetivoAgenda();
            int idEventoReaberturaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ReaberturaMatricula);
            lblMotivoReabertura.Visible = false;
            ddlMotivoReabertura.Visible = false;

            CarregarDadosDrop(ddlEncAnoEncerramento.ID);
            PreencherDadoCombo(ddlEncAnoEncerramento, ObjetoEncerramento.Ano_encerramento);
            CarregarDadosDrop(ddlEncCausa.ID);
            PreencherDadoCombo(ddlEncCausa, ObjetoEncerramento.Causa);
            if (ObjetoEncerramento.Motivo == "PROV_DES" || ObjetoEncerramento.Motivo == "DUPLIC_SIS")
            {
                var nomeMotivo = ObjetoEncerramento.Motivo == "PROV_DES" ? "Provável Desistência" : "Duplicidade de Matrícula";
                ddlEncMotivo.Items.Clear();
                ddlEncMotivo.Items.Add(new ListItem(nomeMotivo, ObjetoEncerramento.Motivo));
                ddlEncMotivo.SelectedIndex = 0;

                if (ObjetoEncerramento.Motivo == "PROV_DES")
                {
                    lblMotivoReabertura.Visible = true;
                    ddlMotivoReabertura.Visible = true;
                    CarregarDadosDrop(ddlMotivoReabertura.ID);
                    PreencherDadoCombo(ddlMotivoReabertura, ObjetoEncerramento.Motivoreabertura);
                }
            }
            else
            {
                CarregarDadosDrop(ddlEncMotivo.ID);
                PreencherDadoCombo(ddlEncMotivo, ObjetoEncerramento.Motivo);
            }
            CarregarDadosDrop(ddlEncPeriodoEncerramento.ID);
            PreencherDadoCombo(ddlEncPeriodoEncerramento, ObjetoEncerramento.Periodo_encerramento);
            if (!string.IsNullOrEmpty(ObjetoEncerramento.Dt_encerramento))
            {
                dtEncDataEncerramento.Date = Convert.ToDateTime(ObjetoEncerramento.Dt_encerramento);
            }
            if (!string.IsNullOrEmpty(ObjetoEncerramento.Dt_reabertura))
            {
                dtEncDataReabertura.Date = Convert.ToDateTime(ObjetoEncerramento.Dt_reabertura);
            }
            else if (string.IsNullOrEmpty(ObjetoEncerramento.Dt_reabertura))
            {
                dtEncDataReabertura.Date = DateTime.Now;
            }
            if (!string.IsNullOrEmpty(ObjetoEncerramento.Dt_colacao))
            {
                dtEncDataColacao.Date = Convert.ToDateTime(ObjetoEncerramento.Dt_colacao);
            }
            if (!string.IsNullOrEmpty(ObjetoEncerramento.Dt_diploma))
            {
                dtEncDataDiploma.Date = Convert.ToDateTime(ObjetoEncerramento.Dt_diploma);
            }
            tseEncInstituicao.DBValue = ObjetoEncerramento.Instituicao;

            ddlAnoReabertura.Items.Clear();
            this.ddlAnoReabertura.DataSource = rnPeriodoLetivoAgenda.ListaAnoAbertoPor(idEventoReaberturaMatricula);
            this.ddlAnoReabertura.Items.Insert(0, "Selecione");
            this.ddlAnoReabertura.DataBind();
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosAluno(Ly_aluno.Row dadosAluno)
        {
            RN.ControleVaga rnControleVaga = new ControleVaga();
            pnlConfirmacao.Visible = false;
            pnlPermuta.Visible = false;


            txtCurriculo.Text = Convert.ToString(dadosAluno.Curriculo);
            txtCurso.Text = Convert.ToString(dadosAluno.Curso);
            txtNomeCurso.Text = Convert.ToString(dadosAluno.Nome_curso);
            txtSerie.Text = Convert.ToString(dadosAluno.Descricao_serie);
            txtCodigoSerie.Text = Convert.ToString(dadosAluno.Serie);
            txtSituacao.Text = Convert.ToString(dadosAluno.Sit_aluno);
            txtTurno.Text = Convert.ToString(dadosAluno.Turno);
            txtNomeTurno.Text = Convert.ToString(dadosAluno.Descricao_turno);
            txtUniEnsino.Text = Convert.ToString(dadosAluno.Nome_comp04);
            hdnCenso.Value = Convert.ToString(dadosAluno.Unidade_ensino);


            //pnEncerramentos
            txtEncCurso.Text = Convert.ToString(dadosAluno.Nome_curso);
            txtEncCurriculo.Text = Convert.ToString(dadosAluno.Curriculo);
            txtEncTurno.Text = Convert.ToString(dadosAluno.Descricao_turno);
            txtEncPeriodoIngresso.Text = Convert.ToString(dadosAluno.Sem_ingresso);
            txtEncAnoIngresso.Text = Convert.ToString(dadosAluno.Ano_ingresso);


        }

        protected void grdDisciplinasAtivas_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string disciplina = Convert.ToString(e.GetListSourceFieldValue("disciplina"));
                string turma = Convert.ToString(e.GetListSourceFieldValue("turma"));
                string ano = Convert.ToString(e.GetListSourceFieldValue("ano"));
                string semestre = Convert.ToString(e.GetListSourceFieldValue("semestre"));

                e.Value = disciplina + "|" + turma + "|" + ano + "|" + semestre;
            }

        }

        #region COMBO

        protected void ddlEncAnoEncerramento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEncAnoEncerramento.SelectedValue.ToString() == "" || ddlEncAnoEncerramento.SelectedValue.ToString() == "Nulo")
            {
                ddlEncPeriodoEncerramento.Items.Clear();
                //CarregarDropDownList(ddlPeriodo, null, ObjetoOcorrencia.Periodo);
            }
            else
            {
                CarregarDadosDrop(ddlEncPeriodoEncerramento.ID);
            }
        }

        protected void ddlConfAnoEncerramento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlConfirmacao.Visible = false;
                pnlPermuta.Visible = false;
                LimparTelaPermuta();

                if (ddlConfAnoEncerramento.SelectedValue.ToString() == "" || ddlConfAnoEncerramento.SelectedValue.ToString() == "Nulo")
                {
                    ddlConfPeriodoEncerramento.Items.Clear();
                }
                else
                {
                    CarregarDadosDrop(ddlConfPeriodoEncerramento.ID);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                ListItem itemVazio = new ListItem("<Lista Vazia>", "");
                drop.Items.Add(itemVazio);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(defaultValue))
                        drop.SelectedValue = defaultValue;
                    else
                    {
                        ListItem itemNulo = new ListItem("<Nenhum>", "");
                        drop.Items.Add(itemNulo);
                        drop.SelectedValue = "";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    drop.ClearSelection();
                }
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLENCANOENCERRAMENTO":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            CarregarDropDownList(ddlEncAnoEncerramento, dadosDrop, null);
                            break;
                        }
                    case "DDLENCPERIODOENCERRAMENTO":
                        {
                            if (ddlEncAnoEncerramento.SelectedValue != "" && ddlEncAnoEncerramento.SelectedValue != null)
                            {
                                string ano = ddlEncAnoEncerramento.SelectedValue.ToString();
                                dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);

                                CarregarDropDownList(ddlEncPeriodoEncerramento, dadosDrop, null);
                            }

                            break;
                        }
                    case "DDLENCCAUSA":
                        {
                            dadosDrop = RN.EncerramentoAluno.ConsultarCausaEncerr();

                            CarregarDropDownList(ddlEncCausa, dadosDrop, null);

                            break;
                        }
                    case "DDLENCMOTIVO":
                        {
                            dadosDrop = RN.EncerramentoAluno.ConsultarMotivoEncerr("N");

                            CarregarDropDownList(ddlEncMotivo, dadosDrop, null);

                            break;
                        }
                    case "DDLCONFANOENCERRAMENTO":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            CarregarDropDownList(ddlConfAnoEncerramento, dadosDrop, null);
                            break;
                        }
                    case "DDLCONFPERIODOENCERRAMENTO":
                        {
                            if (ddlConfAnoEncerramento.SelectedValue != "" && ddlConfAnoEncerramento.SelectedValue != null)
                            {
                                string ano = ddlConfAnoEncerramento.SelectedValue.ToString();
                                dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);

                                CarregarDropDownList(ddlConfPeriodoEncerramento, dadosDrop, null);
                            }

                            break;
                        }
                    case "DDLCONFMOTIVO":
                        {
                            dadosDrop = RN.EncerramentoAluno.ConsultarMotivoEncerr("N");

                            CarregarDropDownList(ddlConfMotivo, dadosDrop, null);

                            break;
                        }
                    case "DDLCONFCAUSA":
                        {
                            dadosDrop = RN.EncerramentoAluno.ConsultarCausaEncerr();

                            CarregarDropDownList(ddlConfCausa, dadosDrop, null);

                            break;
                        }
                    case "DDLMOTIVOREABERTURA":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("MotivoReabertura");
                            CarregarDropDownList(ddlMotivoReabertura, dadosDrop, "");
                            break;
                        }
                    case "DDLMOTIVOPERMUTA":
                        {
                            RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
                            ddlMotivoPermuta.DataSource = rnTransferenciaTurma.ListaMotivoTransferencia();
                            ddlMotivoPermuta.DataBind();
                            ddlMotivoPermuta.Items.Insert(0, new ListItem("Selecione", string.Empty));

                            break;
                        }
                }
            }
            catch
            {
                throw;
            }

            return dadosDrop;
        }

        #endregion //COMBOS


        #endregion //METODOS

        #region Botões Encerrar e Reabrir

        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            try
            {
                string aluno = tseAluno.DBValue.ToString();

                if (dtEncDataEncerramento.Date != DateTime.MinValue)
                {
                    if (RN.EncerramentoAluno.ValidaDataEncerramento(aluno, dtEncDataEncerramento.Date))
                    {
                        lblMensagem.Text = "Não é possível selecionar uma data de encerramento menor que a última data de reabertura.";
                        return;
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor verificar a data de encerramento.";
                    return;
                }

                VerificaMatriculaFechamento();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnReabrir_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(dtEncDataEncerramento.Text.Trim()) && !string.IsNullOrEmpty(dtEncDataReabertura.Text.Trim()))
                {
                    if (dtEncDataReabertura.Date.Date <= dtEncDataEncerramento.Date.Date)
                    {
                        lblMensagem.Text = "A data de reabertura não pode ser menor e igual que a data de encerramento.";
                        return;
                    }
                }
                if (string.IsNullOrEmpty(ddlMotivoReabertura.SelectedValue) && ddlEncMotivo.SelectedValue == "PROV_DES")
                {
                    lblMensagem.Text = "O campo Motivo de Reabertura é de preenchimento obrigatório.";
                    return;
                }
                if (ddlAnoReabertura.SelectedValue == "Selecione" || ddlPeriodoReabertura.SelectedValue == "Selecione")
                {
                    lblMensagem.Text = "O campo Ano/Período de Reabertura é de preenchimento obrigatório.";
                    return;
                }

                //verificar se campos novos (curso / turno / serie) foram preenchidos
                if (tseCurso.DBValue.IsNull || !tseCurso.IsValidDBValue)
                {
                    lblMensagem.Text = "O campo Curso de Reabertura é de preenchimento obrigatório.";
                    return;
                }

                if (string.IsNullOrEmpty(cmbTurno.SelectedValue) || cmbTurno.SelectedValue == "Selecione")
                {
                    lblMensagem.Text = "O campo Turno de Reabertura é de preenchimento obrigatório.";
                    return;
                }

                if (string.IsNullOrEmpty(cmbSerie.SelectedValue) || cmbSerie.SelectedValue == "Selecione")
                {
                    lblMensagem.Text = "O campo Série de Reabertura é de preenchimento obrigatório.";
                    return;
                }

                string aluno = tseAluno.DBValue.ToString();


                if (string.IsNullOrEmpty(hdnCurriculoReabertura.Value))
                {
                    lblMensagem.Text = "Matriz Curricular não identificada.";
                    return;
                }

                ReabrirAluno();

                string mensagem = lblMensagem.Text;
                if (ddlMotivoReabertura.SelectedValue == "AjusteLancAnterior" && ddlEncMotivo.SelectedValue == "PROV_DES" && string.IsNullOrEmpty(mensagem))
                {
                    mensagem = "Reabertura realizada com sucesso. Por favor, verifique a necessidade de realizar correções/lançamentos de notas.";
                }
                string queryString = MontarQueryString(aluno, mensagem);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                Response.Redirect("ListarEncerramentoAluno.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            string aluno = tseAluno.DBValue.ToString();
            string mensagem = string.Empty;
            string queryString = MontarQueryString(aluno, mensagem);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            Response.Redirect("ListarEncerramentoAluno.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        #endregion

        #region Botões Pop Ups

        protected void btnRemoveDisciplinasMatricula_Click(object sender, EventArgs e)
        {
            ObjetoExecucao.Remove_Disciplinas = true;
            AbreConfirmacaoEncerramento();
        }

        protected void btnCancelaRemovaDisciplinasMatricula_Click(object sender, EventArgs e)
        {
            pcPreMatricula.ShowOnPageLoad = false;
            pcDisciplinasMatricula.ShowOnPageLoad = false;
            return;
        }

        protected void btnRemovePreMatricula_Click(object sender, EventArgs e)
        {
            ObjetoExecucao.Remove_PreMatricula = true;
            VerificaMatriculaFechamento();
        }

        protected void btnCancelaRemovePreMatricula_Click(object sender, EventArgs e)
        {
            pcPreMatricula.ShowOnPageLoad = false;
            return;
        }

        protected void btnConfSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Bloqueio", "Bloqueio();", true);
                EncerrarAluno();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfCancelar_Click(object sender, EventArgs e)
        {
            pcConfirmaDados.ShowOnPageLoad = false;
            pcDisciplinasMatricula.ShowOnPageLoad = false;
            pcPreMatricula.ShowOnPageLoad = false;
            return;
        }
        #endregion

        #region Métodos RN

        private void AbreConfirmacaoEncerramento()
        {

            ObterDadosEncerramento();
            pcConfirmaDados.ShowOnPageLoad = true;
        }

        private void AlunoFormandoSerie()
        {
            string curso = txtCurso.Text;
            string curriculo = txtCurriculo.Text;
            string turno = txtTurno.Text;
            string aluno = tseAluno.DBValue.ToString();

            QueryTable serie = RN.EncerramentoAluno.AlunoFormandoSerie(curso, turno, curriculo);
            if (serie.Rows.Count > 0)
            {
                QueryTable disciplinas = RN.EncerramentoAluno.DisciplinasUltimaSerie(curso, turno, curriculo, serie.Rows[0]["ultima_serie"].ToString());
                string nome_serie = serie.Rows[0]["ultima_serie"].ToString();
                if (disciplinas.Rows.Count > 0)
                {
                    ObjetoExecucao.Aluno_Formando = true;
                    for (int i = 0; i < disciplinas.Rows.Count; i++)
                    {
                        string disciplina = disciplinas.Rows[i]["DISCIPLINA"].ToString();
                        QueryTable sit_hist = RN.EncerramentoAluno.SituacaoDisciplinasHistorico(aluno, nome_serie, disciplina);
                        if (sit_hist.Rows.Count > 0)
                        {
                            if (sit_hist.Rows[0]["SITUACAO_HIST"].ToString() != "Aprovado")
                            {
                                ObjetoExecucao.Aluno_Formando = false;
                            }
                        }
                        else
                        {
                            ObjetoExecucao.Aluno_Formando = false;
                        }
                    }
                }
            }
        }

        private void VerificaCurriculoSubSeq()
        {
            if (RN.EncerramentoAluno.VerificaCurriculoSubSeq(txtCurso.Text, txtTurno.Text, txtCurriculo.Text))
            {
                ObjetoExecucao.Situacao_Aluno = "Ativo";
            }
        }

        private void AtualizaSituacaoPorMotivo()
        {
            string motivo = ddlConfMotivo.SelectedValue;
            if (motivo.ToUpper() == "TRANSFERENCIA")
            {
                ObjetoExecucao.Situacao_Aluno = "Transferido";
                ObjetoExecucao.Situacao_MatGrade = "Transf.Externamente";
            }
            else if (motivo.ToUpper() == "CONCLUSAO")
            {
                ObjetoExecucao.Situacao_Aluno = "Concluido";
                ObjetoExecucao.Situacao_MatGrade = "Concluido";
            }
            else if (motivo.ToUpper() == "EVASAO")
            {
                ObjetoExecucao.Situacao_Aluno = "Evadido";
                ObjetoExecucao.Situacao_MatGrade = "Desistente";
            }
            else //(motivo.ToUpper() == "CANCELAMENTO" || motivo.ToUpper() == "ABANDONO" || motivo.ToUpper() == "JUBILAMENTO" || motivo.ToUpper() == "OBITO")
            {
                ObjetoExecucao.Situacao_Aluno = "Cancelado";
                ObjetoExecucao.Situacao_MatGrade = "Cancelado";
            }
        }

        private void VerificaMatriculaFechamento()
        {
            if (RN.EncerramentoAluno.VerificaMatriculaAberta(ObjetoEncerramento.Aluno))
            {
                lblMensagem.Text = "Aluno não pode ser encerrado pois possui matrícula(s) fechadas com status 'Aprovado', 'Rep Freq' ou 'Rep Nota'.";
                return;
            }
            VerificaMatricula();
        }

        private void VerificaMatricula()
        {
            if (RN.EncerramentoAluno.VerificaMatricula(ObjetoEncerramento.Aluno))
            {
                ObjetoExecucao.Matricula = true;
                AbreDisciplinasMatriculadas();
            }
            else
            {
                AbreConfirmacaoEncerramento();
            }
        }

        private void AbreDisciplinasMatriculadas()
        {
            pcDisciplinasMatricula.ShowOnPageLoad = true;
        }

        private void AbrePreMatricula()
        {
            pcPreMatricula.ShowOnPageLoad = true;
        }


        private string MontarQueryString(string aluno, string mensagem)
        {
            string queryString = string.Empty;
            queryString += "aluno=" + aluno;
            queryString += "&mensagem=" + mensagem;
            return queryString;
        }


        private void ObterDadosEncerramento()
        {
            CarregarDadosDrop(ddlConfAnoEncerramento.ID);
            PreencherDadoCombo(ddlConfAnoEncerramento, ddlEncAnoEncerramento.SelectedValue);
            CarregarDadosDrop(ddlConfPeriodoEncerramento.ID);
            PreencherDadoCombo(ddlConfPeriodoEncerramento, ddlEncPeriodoEncerramento.SelectedValue);
            CarregarDadosDrop(ddlConfMotivo.ID);
            PreencherDadoCombo(ddlConfMotivo, ddlEncMotivo.SelectedValue);
            tseConfInstituicao.DBValue = tseEncInstituicao.DBValue;
            CarregarDadosDrop(ddlConfCausa.ID);
            PreencherDadoCombo(ddlConfCausa, ddlEncCausa.SelectedValue);
            dtConfDataColacao.Value = dtEncDataColacao.Value;
            dtConfDataDiploma.Value = dtEncDataDiploma.Value;
        }


        private void MontarDadosEncerramento(Ly_h_cursos_concl dtEncerramento)
        {
            Techne.Lyceum.CR.Ly_h_cursos_concl.Row dadosEncerramento = dtEncerramento.NewRow();

            dadosEncerramento.Curso = txtCurso.Text;
            dadosEncerramento.Turno = txtTurno.Text;
            dadosEncerramento.Curriculo = txtCurriculo.Text;
            dadosEncerramento.Aluno = tseAluno.DBValue.ToString();
            dadosEncerramento.Dt_encerramento = Convert.ToDateTime(ObjetoEncerramento.Dt_encerramento);
            dadosEncerramento.Ano_encerramento = Convert.ToDecimal(ddlConfAnoEncerramento.SelectedValue);
            dadosEncerramento.Ano_ingresso = Convert.ToDecimal(txtEncAnoIngresso.Text);
            dadosEncerramento.Causa_encerr = ddlConfCausa.SelectedValue;
            if (!string.IsNullOrEmpty(dtConfDataColacao.Text))
            {
                dadosEncerramento.Dt_colacao = Convert.ToDateTime(dtConfDataColacao.Value);
            }
            else
            {
                dadosEncerramento.Dt_colacao = null;
            }
            if (!string.IsNullOrEmpty(dtConfDataDiploma.Text))
            {
                dadosEncerramento.Dt_diploma = Convert.ToDateTime(dtConfDataDiploma.Value);
            }
            else
            {
                dadosEncerramento.Dt_diploma = null;
            }
            dadosEncerramento.Motivo = ddlConfMotivo.SelectedValue;
            dadosEncerramento.Outra_faculdade = (!tseConfInstituicao.DBValue.IsNull && tseConfInstituicao.IsValidDBValue) ? tseConfInstituicao.DBValue.ToString() : null;
            dadosEncerramento.Sem_encerramento = Convert.ToInt16(ddlConfPeriodoEncerramento.SelectedValue);
            dadosEncerramento.Sem_ingresso = Convert.ToInt16(txtEncPeriodoIngresso.Text);
            dadosEncerramento.Dt_insercao = DateTime.Now;
            dadosEncerramento.Dt_ultalt = DateTime.Now;
            dtEncerramento.Rows.Add(dadosEncerramento);

        }

        #endregion

        public object Listar(DbObject tseAluno)
        {
            QueryTable qt = null;

            if (!tseAluno.IsNull)
            {
                qt = RN.EncerramentoAluno.ConsultarMatriculaDisciplinas(tseAluno.ToString());
            }
            return qt;
        }

        protected void grdDisciplinasAtivas_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDisciplinasAtivas);
        }

        private void EncerrarAluno()
        {
            RN.EncerramentoAluno rnEncerramentoAluno = new Techne.Lyceum.RN.EncerramentoAluno();
            ValidacaoDados validacao = new ValidacaoDados();
            string aviso = string.Empty;
            DateTime dt = Convert.ToDateTime(dtEncDataEncerramento.Value);
            DateTime dt2 = new DateTime(dt.Year, dt.Month, dt.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            ObjetoEncerramento.Dt_encerramento = dt2.ToString();

            if (!string.IsNullOrEmpty(tseConfInstituicao.DBValue.ToString()) && !tseConfInstituicao.IsValidDBValue)
            {
                lblConfMensagem.Text = "Atenção! Instituição não cadastrada.";
                return;
            }

            if (ddlConfMotivo.SelectedValue.ToUpper() == "CANCELAMENTO" || ddlConfMotivo.SelectedValue.ToUpper() == "TRANSFERENCIA")
            {
                if (string.IsNullOrEmpty(ddlConfCausa.SelectedValue))
                {
                    lblConfMensagem.Text = "Atenção! Causa do encerramento deve ser informada.";
                    return;
                }
            }

            // Verifica Historico Existente            
            AtualizaSituacaoPorMotivo();

            ObjetoExecucao.Curso = txtCurso.Text;
            ObjetoExecucao.Turno = txtTurno.Text;
            ObjetoExecucao.Serie = !txtCodigoSerie.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtCodigoSerie.Text) : -1;

            //ObjetoExecucao.Num_Chamada = 0;
            ObjetoExecucao.RealizaPermuta = rblConfirmacao.SelectedValue == "Sim" ? true : false;
            ObjetoExecucao.AlunoPermuta = (!tseAlunoPermuta.DBValue.IsNull && tseAlunoPermuta.IsValidDBValue) ? tseAlunoPermuta.DBValue.ToString() : null;
            ObjetoExecucao.EnsinoReligioso = chkEnsReligiosoPermuta.Checked;
            ObjetoExecucao.LinguaEstrangeira = chkLinEstrangeiraPermuta.Checked;
            ObjetoExecucao.MotivoPermuta = !ddlMotivoPermuta.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlMotivoPermuta.SelectedValue : null;
            ObjetoExecucao.UsuarioResponsavel = User.Identity.Name;
            ObjetoExecucao.Censo = !hdnCenso.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCenso.Value : null;
            ObjetoExecucao.NecessidadeEspecialAlunoPermuta = (!tseAlunoPermuta.DBValue.IsNull && tseAlunoPermuta.IsValidDBValue) ? tseAlunoPermuta["necessidade_especial"].ToString() : null;
            ObjetoExecucao.DataNascimentoAlunoPermuta = (!tseAlunoPermuta.DBValue.IsNull && tseAlunoPermuta.IsValidDBValue) ? Convert.ToDateTime(tseAlunoPermuta["dt_nascimento"].ToString()) : DateTime.MinValue;


            string motivo = ddlConfMotivo.SelectedValue;

            if (ObjetoExecucao.Matricula == true)
            {
                if (motivo.ToUpper() != "CONCLUSAO")
                {
                    ObjetoExecucao.Cancela_Matricula = true;
                    ObjetoExecucao.Busca_Carteirinha = true;
                    ObjetoExecucao.Registra_EncMatGrade = true;
                }
                else if (motivo.ToUpper() == "CONCLUSAO")
                {
                    lblMensagem.Text = "O aluno não pode ser encerrado com motivo de conclusão pois ainda possui disciplina(s) Matriculada(s). É necessário fazer o fechamento antes.";
                    pcDisciplinasMatricula.ShowOnPageLoad = false;
                    pcPreMatricula.ShowOnPageLoad = false;
                    pcConfirmaDados.ShowOnPageLoad = false;
                    return;
                }
            }
            if (motivo.ToUpper() == "CONCLUSAO")
            {
                AlunoFormandoSerie();
                VerificaCurriculoSubSeq();
            }

            LyHCursosConcl lyHCursosConcl = new LyHCursosConcl();

            lyHCursosConcl.Curso = txtCurso.Text;
            lyHCursosConcl.Turno = txtTurno.Text;
            lyHCursosConcl.Curriculo = txtCurriculo.Text;
            lyHCursosConcl.Aluno = tseAluno.DBValue.ToString();
            lyHCursosConcl.DtEncerramento = Convert.ToDateTime(ObjetoEncerramento.Dt_encerramento);
            lyHCursosConcl.AnoEncerramento = !string.IsNullOrEmpty(ddlConfAnoEncerramento.SelectedValue) ? Convert.ToDecimal(ddlConfAnoEncerramento.SelectedValue) : -1;
            lyHCursosConcl.AnoIngresso = !string.IsNullOrEmpty(txtEncAnoIngresso.Text) ? Convert.ToDecimal(txtEncAnoIngresso.Text) : -1;
            lyHCursosConcl.CausaEncerr = ddlConfCausa.SelectedValue;
            lyHCursosConcl.DtColacao = !string.IsNullOrEmpty(dtConfDataColacao.Text) ? Convert.ToDateTime(dtConfDataColacao.Value) : (DateTime?)null;
            lyHCursosConcl.DtDiploma = !string.IsNullOrEmpty(dtConfDataDiploma.Text) ? Convert.ToDateTime(dtConfDataDiploma.Value) : (DateTime?)null;
            lyHCursosConcl.Motivo = ddlConfMotivo.SelectedValue;
            lyHCursosConcl.OutraFaculdade = (!tseConfInstituicao.DBValue.IsNull && tseConfInstituicao.IsValidDBValue) ? tseConfInstituicao.DBValue.ToString() : null;
            lyHCursosConcl.SemEncerramento = !string.IsNullOrEmpty(ddlConfPeriodoEncerramento.SelectedValue) ? Convert.ToInt16(ddlConfPeriodoEncerramento.SelectedValue) : -1;
            lyHCursosConcl.SemIngresso = Convert.ToInt16(txtEncPeriodoIngresso.Text);
            lyHCursosConcl.DtInsercao = DateTime.Now;
            lyHCursosConcl.DtUltalt = DateTime.Now;
            lyHCursosConcl.Pessoa = Convert.ToDecimal(hdnPessoa.Value);

            validacao = rnEncerramentoAluno.ValidaEncerramento(lyHCursosConcl, ObjetoExecucao);

            if (validacao.Valido)
            {
                CR.Ly_h_cursos_concl dtEncerramento = new Techne.Lyceum.CR.Ly_h_cursos_concl();
                MontarDadosEncerramento(dtEncerramento);

                rnEncerramentoAluno.EncerrarAluno(dtEncerramento, ObjetoExecucao, out aviso);

                pcDisciplinasMatricula.ShowOnPageLoad = false;
                pcPreMatricula.ShowOnPageLoad = false;
                pcConfirmaDados.ShowOnPageLoad = false;
                string aluno = tseAluno.DBValue.ToString();
                string mensagem = lblMensagem.Text;
                string queryString = MontarQueryString(aluno, mensagem);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                if (aviso.IsNullOrEmptyOrWhiteSpace())
                {
                    lblConfMensagem.Text = "Encerramento realizado com sucesso.";
                    Response.Redirect("ListarEncerramentoAluno.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
                else
                {
                    lblConfMensagem.Text = "Encerramento realizado com sucesso. " + "<br/>" + aviso;
                }
            }
            else
            {
                lblConfMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                pcDisciplinasMatricula.ShowOnPageLoad = false;
                pcPreMatricula.ShowOnPageLoad = false;
                return;
            }

        }

        private void ReabrirAluno()
        {
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new RN.ConfirmacaoMatricula();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            Compartilhada rnCompartilhada = new Compartilhada();
            GradeSerie rnGradeSerie = new GradeSerie();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            DadosParticipacao eventoReabertura = new DadosParticipacao();
            Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            RN.RestricaoIdadeSerie rnRestricaoIdadeSerie = new RestricaoIdadeSerie();
            TceRestricaoIdadeSerie restricao = new TceRestricaoIdadeSerie();
            RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
            List<RN.Agenda.Entidades.Agenda> agendas = new List<Techne.Lyceum.RN.Agenda.Entidades.Agenda>();
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            RN.Aluno rnAluno = new RN.Aluno();
            RN.Perfil rnPerfil = new Perfil();
            RN.Matriculas.PessoaAluno rnPessoaAluno = new Techne.Lyceum.RN.Matriculas.PessoaAluno();

            try
            {
                string tipoVaga = string.Empty;
                string aluno = tseAluno.DBValue.ToString();
                string curso = txtCurso.Text;
                string turno = txtTurno.Text;
                string curriculo = txtCurriculo.Text;
                string motivo = string.IsNullOrEmpty(ddlMotivoReabertura.SelectedValue) ? string.Empty : ddlMotivoReabertura.SelectedItem.Text;
                string ano = ddlAnoReabertura.SelectedValue;
                string semestre = ddlPeriodoReabertura.SelectedValue;
                bool utilizaTransporte = false;

                //Criação do aluno com dados novos:             
                var alunoReabertura = Aluno.Carregar(tseAluno.DBValue.ToString());
                string cursoReabertura = tseCurso.DBValue.ToString();
                string turnoReabertura = cmbTurno.SelectedValue;
                int serieReabertura = Convert.ToInt32(cmbSerie.SelectedValue);
                int idEventoReaberturaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ReaberturaMatricula);
                bool anoPeriodoAtual = false;
                int idade = 0;
                DateTime dtNascimento;

                bool matriculaAtivada = true;

                //Atualiza dados do aluno novo pelos novos combos escolhidos na tela
                alunoReabertura.Curso = cursoReabertura;
                alunoReabertura.Turno = turnoReabertura;
                alunoReabertura.Serie = serieReabertura;
                alunoReabertura.Curriculo = hdnCurriculoReabertura.Value;

                bool compartilhada = false;

                if (hdnCompartilhada.Value == "S")
                    compartilhada = true;

                //Verifica se é reabertura de aluno de compartilhada
                if (compartilhada)
                {
                    //Verifica se existe compartilhada
                    if (!rnCompartilhada.PossuiUnidadeCompartilhadaDestino(alunoReabertura.UnidadeEnsino))
                    {
                        lblMensagem.Text = "Não foi encontrado registro de unidade compartilhada para a unidade do aluno";
                        return;
                    }
                }
                else
                {
                    //Verifica se a reabertura será para ano e periodo atual 
                    anoPeriodoAtual = rnPeriodoLetivo.EhAnoPeriodoAtivoPor(Convert.ToInt32(ano), Convert.ToInt32(semestre), DateTime.Now);
                    if (anoPeriodoAtual)
                    {
                        //Verifica se existe evento aberto de Reabertura de matricula ativo
                        eventoReabertura = rnAgenda.VerificaEventoPor(idEventoReaberturaMatricula, alunoReabertura.UnidadeEnsino, alunoReabertura.Curso, alunoReabertura.Turno, Convert.ToInt32(alunoReabertura.Serie));

                        if (!rnPerfil.PossuiPerfilReaberturaPeriodoBloqueioPor(User.Identity.Name))
                        {
                            if (!(eventoReabertura.ParticipaTotal || eventoReabertura.ParticipaCurso || eventoReabertura.ParticipaUnidade))
                            {
                                lblMensagem.Text = "Não será possível realizar a reabertura, pois a Agenda de Reabertura não está aberta para este ano / período.";
                                return;
                            }
                        }
                    }

                    //Verifica se unidade/ano/periodo/curso/serie/turno está participando caso nao seja reabertura de compartilhada
                    if (rnControleVaga.PartipaMatriculaFacilPor(alunoReabertura.UnidadeEnsino, Convert.ToInt32(ddlAnoReabertura.SelectedValue), Convert.ToInt32(ddlPeriodoReabertura.SelectedValue), alunoReabertura.Curso, Convert.ToInt32(alunoReabertura.Serie), alunoReabertura.Turno))
                    {
                        lblMensagem.Text = "Não será possível realizar a reabertura, pois o curso/série está participando do Matrícula Fácil.";
                        return;
                    }
                }

                string matricula = rnPessoaAluno.ObtemOutraPessoaAlunoPor(alunoReabertura.Pessoa.Value, alunoReabertura.Aluno);

                if (!matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Não será possível realizar a reabertura, pois a matrícula de número " + matricula + " consta como correta do aluno.";
                    return;
                }

                //Busca dados do aluno
                dadosAluno = rnAluno.ObtemDadosAluno(aluno);

                //Verifica a data de nascimento do aluno
                if (dadosAluno.DataNascimento == null || dadosAluno.DataNascimento == DateTime.MinValue)
                {
                    lblMensagem.Text = "Aluno sem data de nascimento para validação.";
                    return;
                }

                //Verifica se existe outro aluno ativo com mesmo nome / mae / data Nascimento
                if (rnAluno.PossuiOutroAlunoAtivoPor(dadosAluno.Nome_compl, dadosAluno.NomeMae, Convert.ToDateTime(dadosAluno.DataNascimento), aluno))
                {
                    lblMensagem.Text = "Não será possível realizar a reabertura, pois já existe outro aluno ativo com mesmo nome/data de nascimento/nome da mãe.";
                    return;
                }

                if (!dadosAluno.Cpf.IsNullOrEmptyOrWhiteSpace())
                {
                    //Verifica se existe outro aluno ativo com mesmo cpf
                    if (rnAluno.PossuiOutroCPFAtivoPor(dadosAluno.Cpf, aluno))
                    {
                        lblMensagem.Text = "Não será possível realizar a reabertura, pois já existe outro aluno ativo com mesmo CPF.";
                        return;
                    }
                }

                //Calcula idade
                idade = Utils.CalcularIdade(Convert.ToDateTime(dadosAluno.DataNascimento));
                restricao = rnRestricaoIdadeSerie.CarregaRestricaoPor(alunoReabertura.Curso, Convert.ToInt32(alunoReabertura.Serie));

                //Verifica se o aluno possui necessidade especial
                if (!rnAluno.PossuiNecessidadeEspecialPor(alunoReabertura.Aluno))
                {
                    //Para Alunos sem necessidades Especiais Verificar restrição de idade minima e maxima
                    if (idade < restricao.IdadeMinima || idade > restricao.IdadeMaxima)
                    {
                        lblMensagem.Text = string.Format("Para o curso selecionado é permitido cadastrar alunos entre {0} e {1} anos. Favor verificar a DATA DE NASCIMENTO!",
                            restricao.IdadeMinima,
                            restricao.IdadeMaxima);
                        return;
                    }
                }
                else
                {
                    //Para Alunos com necessidades Especiais Verificar restrição de idade minima
                    if (idade < restricao.IdadeMinima)
                    {
                        lblMensagem.Text = string.Format("Para o curso selecionado não é permitido cadastrar alunos com necessidade especial com menos de {0} anos. Favor verificar a DATA DE NASCIMENTO!",
                            restricao.IdadeMinima);
                        return;
                    }
                }

                //Verificação do Tipo de Vaga através do Ano/Perido do Encerramento e Ano/Período da Reabertura
                if (ddlEncAnoEncerramento.SelectedValue == ddlAnoReabertura.SelectedValue
                    && ddlEncPeriodoEncerramento.SelectedValue == ddlPeriodoReabertura.SelectedValue)
                {
                    tipoVaga = "VC";
                }
                else
                {
                    tipoVaga = "VN";
                }

                TConnectionWritable connection = Config.CreateWritableConnection();
                try
                {
                    connection.Open(true);
                    RetValue ret = null;
                    DateTime dt = Convert.ToDateTime(dtEncDataReabertura.Value);
                    DateTime dt_reabr = new DateTime(dt.Year, dt.Month, dt.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                    int vagasLiberadas = 0;
                    int vagasUtilizadas = 0;

                    //Verificar se tem vaga no curso / serie / turno / ano / semestre
                    vagasLiberadas = rnControleVaga.ObtemVagasLiberadasTotalPor(
                        alunoReabertura.UnidadeEnsino,
                        Convert.ToInt32(ano),
                        Convert.ToInt32(semestre),
                        Convert.ToInt32(alunoReabertura.Serie),
                        alunoReabertura.Curso,
                        alunoReabertura.Turno);

                    vagasUtilizadas = rnControleVaga.ObtemVagasUtilizadasTotalPor(
                        alunoReabertura.UnidadeEnsino,
                        Convert.ToInt32(ano),
                        Convert.ToInt32(semestre),
                        Convert.ToInt32(alunoReabertura.Serie),
                        alunoReabertura.Curso,
                        alunoReabertura.Turno);

                    if (vagasLiberadas <= vagasUtilizadas)
                    {
                        lblMensagem.Text = "Não será possível realizar a reabertura, pois não existem vagas disponíveis para o curso/série e turno!";
                        return;
                    }

                    //Verificar se existe linha com os dados para ser reaberta
                    RN.EncerramentoAluno rnEncerramentoAluno = new RN.EncerramentoAluno();
                    if (!rnEncerramentoAluno.PossuiEncerramentoParaReabertura(aluno))
                    {
                        lblMensagem.Text = "Não será possível realizar a reabertura, pois não foi encontrado um registro de encerramento com os dados necessários.";
                        return;
                    }

                    //Atualizar dados do aluno, curso / turno / serie / curriculo / situação
                    RN.EncerramentoAluno.AtualizaDadosReaberturaAluno(connection, alunoReabertura);
                    ret = VerificarErro(connection.GetErrors());
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        lblMensagem.Text = ret.Errors.ToString();
                        return;
                    }

                    //Fernanda Reina: não dá pra usar o curso de reabertura... pq ele não vai encontrar uma vez que ele grava o curso que foi encerrado
                    RN.EncerramentoAluno.AtualizaDataReaberturaHistorico(connection, aluno, curso, turno, curriculo, dt_reabr, motivo, ddlAnoReabertura.SelectedValue, ddlPeriodoReabertura.SelectedValue);
                    ret = VerificarErro(connection.GetErrors());
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        lblMensagem.Text = ret.Errors.ToString();
                        return;
                    }

                    //Verifica se possui marcação no campo gratuidade
                    utilizaTransporte = Aluno.ExisteUtilizaTransporte(aluno);

                    //Caso exista marcação no campo gratuidade desmarcar
                    if (utilizaTransporte)
                    {
                        RN.EncerramentoAluno.RetiraUsoGratuidade(connection, aluno);
                        ret = VerificarErro(connection.GetErrors());
                        if (ret != null && !ret.Ok)
                        {
                            connection.Rollback();
                            lblMensagem.Text = ret.Errors.ToString();
                            return;
                        }
                    }

                    RN.EncerramentoAluno.VerificaCarteirinha(connection, aluno, dt_reabr);

                    //Fernanda Reina: o restante já tem que validar no curso novo
                    ObjetoExecucao.Id_Grade = RN.EncerramentoAluno.ConsultaMatGradePor(connection, aluno, ddlAnoReabertura.SelectedValue, ddlPeriodoReabertura.SelectedValue, cursoReabertura, turnoReabertura, Convert.ToString(serieReabertura), hdnCurriculoReabertura.Value, alunoReabertura.UnidadeEnsino);

                    if (ObjetoExecucao.Id_Grade > 0)
                    {
                        var vagas = RN.Turma.RetornaVagas(Convert.ToInt32(ddlAnoReabertura.SelectedValue), Convert.ToInt32(ddlPeriodoReabertura.SelectedValue), rnGradeSerie.ObtemTurmaPor(ObjetoExecucao.Id_Grade));
                        if (vagas > 0)
                        {
                            if (RN.EncerramentoAluno.VerificaMatriculas(aluno))
                            {
                                RN.EncerramentoAluno.AtivaMatriculas(connection, aluno, dt_reabr, ObjetoExecucao.Id_Grade);
                                ret = VerificarErro(connection.GetErrors());
                                if (ret != null && !ret.Ok)
                                {
                                    connection.Rollback();
                                    lblMensagem.Text = ret.Errors.ToString();
                                    return;
                                }
                                matriculaAtivada = true;
                            }

                            if (!RN.EncerramentoAluno.PossuiMatGrade(aluno, ObjetoExecucao.Id_Grade))
                            {
                                RN.EncerramentoAluno.InsereMatGrade(connection, aluno, ObjetoExecucao.Id_Grade, dt_reabr);
                                ret = VerificarErro(connection.GetErrors());
                                if (ret != null && !ret.Ok)
                                {
                                    connection.Rollback();
                                    lblMensagem.Text = ret.Errors.ToString();
                                    return;
                                }
                            }

                            RN.EncerramentoAluno.GeraNumChamada(connection, aluno, ObjetoExecucao.Id_Grade);
                            ret = VerificarErro(connection.GetErrors());
                            if (ret != null && !ret.Ok)
                            {
                                connection.Rollback();
                                lblMensagem.Text = ret.Errors.ToString();
                                return;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    connection.Rollback();
                    lblMensagem.Text = e.Message;
                    return;
                }
                finally
                {
                    connection.Close();
                }

                //Inclusao/Alteração da Confirmação de Matricula
                bool ensinoReligioso = chkEnsReligioso.Checked;
                bool linguaEstrangeira = chkLinguaEstrangeira.Checked;

                var confirmacao = new TceConfirmacaoMatricula
                {
                    Aluno = aluno,
                    Censo = alunoReabertura.UnidadeEnsino,
                    Ano = Convert.ToDecimal(ddlAnoReabertura.SelectedValue),
                    Periodo = Convert.ToDecimal(ddlPeriodoReabertura.SelectedValue),
                    Curso = alunoReabertura.Curso,
                    Turno = alunoReabertura.Turno,
                    Curriculo = alunoReabertura.Curriculo,
                    Serie = alunoReabertura.Serie,
                    EnsinoReligioso = ensinoReligioso,
                    LinguaEstrangeiraFacultativa = linguaEstrangeira,
                    ProjetoAutonomia = false,
                    Matricula = User.Identity.Name,
                    TipoVagaOcupada = tipoVaga,
                    Status = matriculaAtivada ? ConfirmacaoMatricula.Confirmado : ConfirmacaoMatricula.Pendente
                };

                rnConfirmacaoMatricula.InserirOuAtualizar(confirmacao);

                lblMensagem.Text = "Reabertura realizada com sucesso.<br />A MATRÍCULA REABERTA SOMENTE TERÁ DIREITO À GRATUIDADE APÓS A ATUALIZAÇÃO CADASTRAL DOS CAMPOS: ENDEREÇO/E-MAIL/GRATUIDADE/ MODAL. CASO O ALUNO JÁ TENHA UTILIZADO TRANSPORTE E POSSUA O CARTÃO, O MESMO SERÁ REATIVADO, CASO NÃO POSSUA, ORIENTE QUE ACESSE O SITE DA RIOCARD PARA SOLICITAR O CARTÃO.";
            }
            catch (Exception e)
            {
                lblMensagem.Text = e.Message;
            }
        }

        protected static RetValue VerificarErro(ErrorList erro)
        {
            if (erro != null)
            {
                if (erro.Count > 0)
                    return new RetValue(false, string.Empty, erro);
            }

            return null;
        }

        protected void ddlAnoReabertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Agenda.PeriodoLetivoAgenda rnPeriodoLetivoAgenda = new Techne.Lyceum.RN.Agenda.PeriodoLetivoAgenda();
            int idEventoReaberturaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ReaberturaMatricula);

            ddlPeriodoReabertura.Items.Clear();
            cmbSerie.Items.Clear();
            cmbTurno.Items.Clear();
            tseCurso.ResetValue();
            hdnCurriculoReabertura.Value = string.Empty;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (ddlAnoReabertura.SelectedValue != "Selecione" || !ddlAnoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                this.ddlPeriodoReabertura.DataSource = rnPeriodoLetivoAgenda.ListaPeriodoAbertoPor(idEventoReaberturaMatricula, Convert.ToInt32(ddlAnoReabertura.SelectedValue));
                this.ddlPeriodoReabertura.Items.Insert(0, "Selecione");
                this.ddlPeriodoReabertura.DataBind();
            }
        }

        protected void ddlPeriodoReabertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbTurno.Items.Clear();
            cmbSerie.Items.Clear();
            tseCurso.ResetValue();
            hdnCurriculoReabertura.Value = string.Empty;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (!string.IsNullOrEmpty(ddlPeriodoReabertura.SelectedValue) && ddlPeriodoReabertura.SelectedValue != "Selecione")
            {
                SimpleRow sr = RN.Aluno.ConsultarDadosAluno(tseAluno.Value.ToString());
                hdnCenso.Value = sr["unidade_ensino"].ToString();

                tseCurso.SqlWhere = " Unidade_ens = '" + hdnCenso.Value + "'";
            }
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            if (Page.IsCallback)
                return;

            cmbSerie.Items.Clear();
            cmbTurno.Items.Clear();
            hdnCurriculoReabertura.Value = string.Empty;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (!tseCurso.DBValue.IsNull || tseCurso.IsValidDBValue)
            {
                cmbTurno.DataSource = RN.Curriculo.ConsultarTurno(tseCurso.DBValue.ToString());
                cmbTurno.Items.Insert(0, "Selecione");
                cmbTurno.DataBind();
            }
        }

        protected void cmbTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSerie.Items.Clear();
            hdnCurriculoReabertura.Value = string.Empty;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (!string.IsNullOrEmpty(ddlAnoReabertura.SelectedValue) && ddlAnoReabertura.SelectedValue != "Selecione"
                && !string.IsNullOrEmpty(ddlPeriodoReabertura.SelectedValue) && ddlPeriodoReabertura.SelectedValue != "Selecione"
                && !string.IsNullOrEmpty(cmbTurno.SelectedValue) && cmbTurno.SelectedValue != "Selecione"
                && (!tseCurso.DBValue.IsNull || tseCurso.IsValidDBValue))
            {
                cmbSerie.DataSource = Serie.ListarSerie(hdnCenso.Value, Convert.ToDecimal(this.ddlAnoReabertura.SelectedValue), Convert.ToDecimal(this.ddlPeriodoReabertura.SelectedValue), this.cmbTurno.SelectedValue, this.tseCurso.DBValue.ToString());
                cmbSerie.Items.Insert(0, "Selecione");
                cmbSerie.DataBind();
            }

            if (hdnCompartilhada.Value == "S")
            {
                if (cmbSerie.Items.FindByValue(hdnSerieComp.Value) != null)
                {
                    cmbSerie.SelectedValue = hdnSerieComp.Value;
                    cmbSerie_SelectedIndexChanged(null, null);
                }
                else
                {
                    lblMensagem.Text = "A série escolhida na Inscriçao de Compartilhadas não possui turma vigente para este ano/período. Verifique.";
                }
            }
        }

        protected void cmbSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            LyCurriculo curriculo = new LyCurriculo();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            hdnCurriculoReabertura.Value = string.Empty;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (!string.IsNullOrEmpty(ddlAnoReabertura.SelectedValue) && ddlAnoReabertura.SelectedValue != "Selecione"
                && !string.IsNullOrEmpty(ddlPeriodoReabertura.SelectedValue) && ddlPeriodoReabertura.SelectedValue != "Selecione"
                && !string.IsNullOrEmpty(cmbTurno.SelectedValue) && cmbTurno.SelectedValue != "Selecione"
                && !string.IsNullOrEmpty(cmbSerie.SelectedValue) && cmbSerie.SelectedValue != "Selecione"
                && (!tseCurso.DBValue.IsNull || tseCurso.IsValidDBValue))
            {
                //Busca Curriculo
                curriculo = rnCurriculo.ObtemPrimeiroAtivoPor(tseCurso.DBValue.ToString(), cmbTurno.SelectedValue, Convert.ToInt32(cmbSerie.SelectedValue), Convert.ToInt32(ddlAnoReabertura.SelectedValue), Convert.ToInt32(ddlPeriodoReabertura.SelectedValue));
                hdnCurriculoReabertura.Value = curriculo.Curriculo;

                //Verifica se o curriculo permite ensino religioso
                if (curriculo.EnsinoReligioso == "S")
                {
                    chkEnsReligioso.Enabled = true;
                }

                //Verifica se o curriculo permite lingua estrangeira
                if (curriculo.LinguaEstrangeira == "S")
                {
                    chkLinguaEstrangeira.Enabled = true;
                }
            }
        }

        protected void rblConfirmacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlPermuta.Visible = false;
                tseAlunoPermuta.ResetValue();
                chkEnsReligiosoPermuta.Checked = false;
                chkLinEstrangeiraPermuta.Checked = false;
                ddlMotivoPermuta.ClearSelection();

                if (!rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblConfirmacao.SelectedValue == "Sim")
                    {
                        pnlPermuta.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAlunoPermuta_Changed(object sender, EventArgs args)
        {
            try
            {
                DataTable podeERLE = null;
                RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();

                if (!tseAlunoPermuta.DBValue.IsNull)
                {
                    if (tseAlunoPermuta.IsValidDBValue)
                    {

                        this.CarregarDadosDrop(ddlMotivoPermuta.ID);

                        podeERLE = rnCurriculo.ObtemPodeEnsinoReligiosoLinguaEstrangPor(txtCurriculo.Text, txtCurso.Text, txtTurno.Text, Convert.ToInt32(ddlEncAnoEncerramento.SelectedValue), Convert.ToInt32(ddlEncPeriodoEncerramento.SelectedValue));

                        if (podeERLE.Rows.Count > 0)
                        {
                            chkEnsReligiosoPermuta.Enabled = Convert.ToBoolean(podeERLE.Rows[0]["PODE_ENSINO_RELIGIOSO"]);
                            chkLinEstrangeiraPermuta.Enabled = Convert.ToBoolean(podeERLE.Rows[0]["PODE_LINGUA_ESTRANGEIRA"]);
                        }

                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        var mensagem = lblMensagem.Text;
                        var script = @"alert('" + mensagem + @"');";
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);


                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    var mensagem = lblMensagem.Text;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);


                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimparTelaPermuta()
        {
            rblConfirmacao.ClearSelection();
            tseAlunoPermuta.ResetValue();
            chkEnsReligiosoPermuta.Checked = false;
            chkLinEstrangeiraPermuta.Checked = false;
            ddlMotivoPermuta.ClearSelection();

        }

        protected void ddlEncPeriodoEncerramento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.ControleVaga rnControleVaga = new ControleVaga();
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

                pnlConfirmacao.Visible = false;
                pnlPermuta.Visible = false;
                LimparTelaPermuta();

                if (!ddlEncPeriodoEncerramento.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rnMatricula.EhMatriculaAtiva(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlEncAnoEncerramento.SelectedValue), Convert.ToInt32(ddlEncPeriodoEncerramento.SelectedValue)))
                    {
                        if (rnControleVaga.PartipaMatriculaFacilPor(hdnCenso.Value, Convert.ToInt32(ddlEncAnoEncerramento.SelectedValue), Convert.ToInt32(ddlEncPeriodoEncerramento.SelectedValue), txtCurso.Text, Convert.ToInt32(txtCodigoSerie.Text), txtTurno.Text))
                        {
                            pnlConfirmacao.Visible = true;
                        }
                    }
                    else
                    {
                        if (rnConfirmacaoMatricula.PossuiConfirmacaoConfirmadaPor(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlEncAnoEncerramento.SelectedValue), Convert.ToInt32(ddlEncPeriodoEncerramento.SelectedValue)))
                        {
                            ObjetoExecucao.ReservaVaga = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
