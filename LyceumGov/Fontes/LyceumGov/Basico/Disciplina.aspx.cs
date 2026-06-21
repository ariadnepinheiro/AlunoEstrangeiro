using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.RN;
using System.Data;
using Techne.Controls;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [
           NavUrl("~/Basico/Disciplina.aspx"),
            ControlText("Componente Curricular"),
            Title("Componente Curricular"),
          ]
    public partial class Disciplina : TPage
    {
        #region Propriedades e Enum
        public enum Operacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Confirmar
        }

        private Operacao _tipoOperacao
        {
            get { return (Operacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
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

        public object ListarDisciplinaMultipla(object disciplina)
        {
            RN.Disciplina rnDisciplina = new RN.Disciplina();
            var disc = disciplina.ToString();
            return rnDisciplina.ListaDisciplinaParaMultiplaPor(disc);
        }

        public object Listar(object disciplina)
        {
            DisciplinaMultipla rnDisciplinaMultipla = new DisciplinaMultipla();
            var disc = disciplina.ToString();
            return rnDisciplinaMultipla.ListaDisciplinaMultiplaPor(disc);
        }

        public object ListarGrupohabilitacaoDisciplina(object disciplina)
        {
            RN.GrupoHabilitacao rnGrupoHabilitacao = new Techne.Lyceum.RN.GrupoHabilitacao();
            var disc = disciplina.ToString();
            return rnGrupoHabilitacao.ListaGrupoHabilitacaoPor(disc);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdGrupoHabilitacoes, "Grupos de Habilitações");
            TituloGrid(grdDisciplinaMultipla, "Disciplinas Múltiplas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    // para a primeira vez que a página é carregada o tipo de operação será inicial
                    _tipoOperacao = Operacao.Inicial;
                    ControlarTipoOperacao();
                }

                if (_tipoOperacao.Equals(Operacao.Novo))
                {
                    tseDisciplina.Mode = ControlMode.View;
                    tseDisciplina.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        private void ControlarTipoOperacao()
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Disciplina rnDisciplina = new Techne.Lyceum.RN.Disciplina();
                switch (_tipoOperacao)
                {
                    case Operacao.Inicial:
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            pcDisciplina.Visible = false;
                            tseDisciplina.ResetValue();
                            tseDisciplina.Enabled = true;
                            tseDisciplina.Mode = Techne.Controls.ControlMode.Edit;
                            ControlarVisibilidadeControle(controles);
                            break;
                        }
                    case Operacao.Consultar:
                        {
                            LimparTela();
                            ImageButton[] controles = new ImageButton[] { btnExcluir, btnNovo, btnEditar };

                            carregaDadosDisciplina(tseDisciplina.DBValue.ToString());

                            DesabilitaCampos();
                            ControlarVisibilidadeControle(controles);

                            break;
                        }
                    case Operacao.Novo:
                        {
                            ImageButton[] controles = new ImageButton[] { btnSalvar, btnCancel };
                            tseDisciplina.ResetValue();
                            pcDisciplina.Visible = true;
                            pcDisciplina.ActiveTabIndex = 0; //abrir sempre na primeira aba
                            pcDisciplina.TabPages[2].ClientEnabled = false;
                            pcDisciplina.TabPages[3].ClientEnabled = false;
                            tseDisciplina.Enabled = false;
                            tseDisciplina.Mode = ControlMode.View;
                            LimparTela();
                            if (string.IsNullOrEmpty(lblMensagem.Text.ToString()))
                            {
                                txHoras_Aula.Text = "0";
                                txHoras_Estagio.Text = "0";
                                txtHorasAtividade.Text = "0";
                                txNota_Max.Text = "10";
                                txN_Casas_Dec.Text = "2";
                            }
                            ControlarVisibilidadeControle(controles);
                            break;
                        }
                    case Operacao.Excluir:
                        {
                            validacao = rnDisciplina.ValidaRemocao(tseDisciplina.DBValue.ToString());
                            if (validacao.Valido)
                            {
                                rnDisciplina.RemoveDisciplina(tseDisciplina.DBValue.ToString());
                            }
                            else
                            {
                                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                                return;
                            }

                            lblMensagem.Text = "Disciplina excluída com sucesso.";

                            _tipoOperacao = Operacao.Inicial;
                            ControlarTipoOperacao();
                            break;
                        }
                    case Operacao.Alterar:
                        {
                            ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                            tseDisciplina.Enabled = false;
                            ControlarVisibilidadeControle(controles);
                            CarregarDadosDrop("DDLGRUPONOTA");
                            CarregarDadosDrop("DDLCOMPONENTE");
                            CarregarDadosDrop("DDLAREACONHECIMENTO");
                            CarregarDadosDrop("DDLCONCEITONOTA");

                            carregaDadosDisciplina(tseDisciplina.DBValue.ToString());
                            HabilitaCampos();
                            this.txtDisciplina.ReadOnly = true;

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
        }

        /// <summary>
        /// Retira a visibilidade de todos botões
        /// </summary>
        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        private void LimparTela()
        {
            //Preencher dropdowns

            CarregarDadosDrop("DDLDEPTO");
            //GRUPONOTA
            ddlGrupo_Nota.Items.Clear();
            CarregarDadosDrop("DDLGRUPONOTA");

            ddlConceitoNota.Items.Clear();
            CarregarDadosDrop("DDLCONCEITONOTA");

            ddlComponente.Items.Clear();
            CarregarDadosDrop("DDLCOMPONENTE");

            ddlAreaConhecimento.Items.Clear();
            CarregarDadosDrop("DDLAREACONHECIMENTO");

            //Textboxes
            this.txtDisciplina.Text = string.Empty;
            this.txNome.Text = string.Empty;
            this.txNome_Fantasia.Text = string.Empty;
            this.txHorasSemanais.Text = string.Empty;
            this.txHoras_Aula.Text = string.Empty;
            this.txtHorasAtividade.Text = string.Empty;
            this.txHoras_Estagio.Text = string.Empty;
            this.txNota_Max.Text = string.Empty;
            this.txN_Casas_Dec.Text = string.Empty;

            //Check boxes
            ckAtiva.Checked = false;
            chkMultipla.Checked = false;
            ckVerifica_Horario.Checked = false;
            ckAtividadeCompl.Checked = false;
            ckEstagio.Checked = false;
            ckTem_Nota.Checked = false;
            ckTem_Freq.Checked = false;
            chkRelatorio.Checked = false;
            ckPrioriza_Freq.Checked = false;
            chkEletiva.Checked = false;
            rblGrupoEletiva.ClearSelection();
            HabilitaCampos();
        }

        private void DesabilitaCampos()
        {
            //Desabilita campos
            this.txtDisciplina.ReadOnly = true;
            this.txNome.ReadOnly = true;
            this.txNome_Fantasia.ReadOnly = true;
            ddlComponente.Enabled = false;
            ddlAreaConhecimento.Enabled = false;
            this.txHoras_Aula.ReadOnly = true;
            this.txtHorasAtividade.ReadOnly = true;
            this.txHoras_Estagio.ReadOnly = true;
            this.txNota_Max.ReadOnly = true;
            this.txN_Casas_Dec.ReadOnly = true;
            this.txHorasSemanais.ReadOnly = true;

            //Desabilita check boxes (11)
            ckAtiva.Enabled = false;
            chkMultipla.Enabled = false;
            ckVerifica_Horario.Enabled = false;
            ckAtividadeCompl.Enabled = false;
            ckEstagio.Enabled = false;
            ckTem_Nota.Enabled = false;
            ckTem_Freq.Enabled = false;
            chkRelatorio.Enabled = false;
            ckPrioriza_Freq.Enabled = false;

            //Desabilita dropdowns
            ddlGrupo_Nota.Enabled = false;
            ddlConceitoNota.Enabled = false;
            chkEletiva.Enabled = false;
            rblGrupoEletiva.Enabled = false;
        }

        private void HabilitaCampos()
        {
            //Habilita textboxes
            this.txtDisciplina.ReadOnly = false;
            this.txNome.ReadOnly = false;
            this.txNome_Fantasia.ReadOnly = false;
            ddlComponente.Enabled = true;
            ddlAreaConhecimento.Enabled = true;
            this.txHoras_Aula.ReadOnly = false;
            this.txtHorasAtividade.ReadOnly = false;
            this.txHoras_Estagio.ReadOnly = false;
            this.txNota_Max.ReadOnly = false;
            this.txN_Casas_Dec.ReadOnly = false;
            this.txHorasSemanais.ReadOnly = false;

            //Habilita check boxes (11)
            ckAtiva.Enabled = true;
            chkMultipla.Enabled = true;
            ckVerifica_Horario.Enabled = true;
            ckAtividadeCompl.Enabled = true;
            ckEstagio.Enabled = true;
            ckTem_Nota.Enabled = true;
            ckTem_Freq.Enabled = true;
            chkRelatorio.Enabled = true;
            ckPrioriza_Freq.Enabled = true;

            //Habilita dropdowns
            ddlGrupo_Nota.Enabled = true;
            ddlConceitoNota.Enabled = true;
            chkEletiva.Enabled = true;
            rblGrupoEletiva.Enabled = true;
        }

        private void CarregarDropDownList(DropDownList drop, DataTable data, string defaultValue)
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
                        drop.SelectedValue = Convert.ToString(itemNulo);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    drop.ClearSelection();
                }
            }
            if (_tipoOperacao.Equals(Operacao.Novo))
            {
                drop.SelectedValue = "";
            }
        }

        private DataTable CarregarDadosDrop(string idDrop)
        {
            DataTable dadosDrop = null;
            Conceito rnConceito = new Conceito();

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLGRUPONOTA":
                        {
                            dadosDrop = rnConceito.ListaGrupoNotas();
                            CarregarDropDownList(ddlGrupo_Nota, dadosDrop, null);
                            break;
                        }
                    case "DDLCONCEITONOTA":
                        {
                            if (!string.IsNullOrEmpty(ddlGrupo_Nota.SelectedValue))
                            {
                                dadosDrop = rnConceito.ListaConceitosPor(ddlGrupo_Nota.SelectedValue);
                                CarregarDropDownList(ddlConceitoNota, dadosDrop, null);
                            }
                            break;
                        }
                    case "DDLCOMPONENTE":
                        {
                            dadosDrop = RN.TabelaGeral.ConsultaItemTabelaValDescr("ComponenteDisciplina");
                            CarregarDropDownList(ddlComponente, dadosDrop, "");
                            break;
                        }
                    case "DDLAREACONHECIMENTO":
                        {
                            dadosDrop = RN.TabelaGeral.ConsultaItemTabelaValDescr("AreaConhecimentoDisc");
                            CarregarDropDownList(ddlAreaConhecimento, dadosDrop, "");
                            break;
                        }
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }

            return dadosDrop;
        }

        protected void chkEletiva_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                pnlGrupoEletiva.Enabled = false;
                rblGrupoEletiva.ClearSelection();

                if (chkEletiva.Checked)
                {
                    pnlGrupoEletiva.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseDisciplina_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(tseDisciplina.DBValue.ToString()) && tseDisciplina.IsValidDBValue)
                {
                    lblMensagem.Text = "";
                    _tipoOperacao = Operacao.Consultar;
                    grdGrupoHabilitacoes.Enabled = true;
                    //carregarGridGrupoHabilitacao(tseDisciplina.DBValue.ToString());
                }
                else
                {
                    lblMensagem.Text = "Componente Curricular não cadastrada.";
                    _tipoOperacao = Operacao.Inicial;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void carregarGridGrupoHabilitacao(string disciplina)
        {
            RN.GrupoHabilitacao rnGrupoHabilitacao = new Techne.Lyceum.RN.GrupoHabilitacao();
            grdGrupoHabilitacoes.DataSource = rnGrupoHabilitacao.ListaGrupoHabilitacaoPor(disciplina);
            grdGrupoHabilitacoes.DataBind();
        }

        private void carregaDadosDisciplina(string disciplina)
        {
            RN.Disciplina rnDisciplina = new Techne.Lyceum.RN.Disciplina();
            RN.Entidades.LyDisciplina dadosDisciplina = new Techne.Lyceum.RN.Entidades.LyDisciplina();

            try
            {
                dadosDisciplina = rnDisciplina.ObtemDisciplinaPor(disciplina);

                if (dadosDisciplina != null)
                {
                    tseDisciplina.Enabled = true;
                    pcDisciplina.ActiveTabIndex = 0; //abrir sempre na primeira aba
                    pcDisciplina.Visible = true;
                    pcDisciplina.TabPages[2].ClientEnabled = true;
                    if (chkMultipla.Checked)
                    {
                        pcDisciplina.TabPages[3].ClientEnabled = true;
                    }
                    else
                    {
                        pcDisciplina.TabPages[3].ClientEnabled = false;
                    }

                    txHorasSemanais.Text = Convert.ToString(dadosDisciplina.AulasSemanais);

                    //Preencher dropdowns
                    PreencherDadoCombo(ddlGrupo_Nota, Convert.ToString(dadosDisciplina.GrupoNota));
                    PreencherDadoCombo(ddlComponente, dadosDisciplina.Componente);
                    PreencherDadoCombo(ddlAreaConhecimento, dadosDisciplina.AreaConhecimento);

                    if (!string.IsNullOrEmpty(ddlGrupo_Nota.SelectedValue))
                    {
                        lblCasasDecNota.Visible = false;
                        txN_Casas_Dec.ReadOnly = true;
                        txN_Casas_Dec.Visible = false;

                        txNota_Max.Visible = false;
                        rfvNotaMax.Enabled = false;
                        rfvNotaMax.Visible = false;

                        ddlConceitoNota.Visible = true;
                        rfvConceitoNota.Enabled = true;

                        lblCasasDecNota.Visible = false;
                        txN_Casas_Dec.ReadOnly = true;
                        txN_Casas_Dec.Visible = false;

                        lblNotaMaxima.Text = "Nota Máxima: ";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txNota_Max.Attributes["onkeypress"]))
                        {
                            txNota_Max.Attributes.Add("onkeypress", "return NumeroComVirgula(event);");
                            txNota_Max.Attributes.Add("onChange", "validaDecimais(this);");
                        }

                        lblCasasDecNota.Visible = true;
                        txN_Casas_Dec.ReadOnly = false;
                        txN_Casas_Dec.Visible = true;

                        txNota_Max.Visible = true;
                        rfvNotaMax.Enabled = true;
                        rfvNotaMax.Visible = true;

                        ddlConceitoNota.Visible = false;
                        rfvConceitoNota.Enabled = false;

                        lblCasasDecNota.Visible = true;
                        txN_Casas_Dec.ReadOnly = false;
                        txN_Casas_Dec.Visible = true;

                        lblNotaMaxima.Text = "Nota Máxima: ";
                    }

                    if (!string.IsNullOrEmpty(dadosDisciplina.GrupoNota))
                    {
                        PreencherDadoCombo(ddlConceitoNota, Convert.ToString(dadosDisciplina.NotaMax));
                    }

                    //Preencher text boxes
                    this.txtDisciplina.Text = dadosDisciplina.Disciplina;
                    this.txNome.Text = dadosDisciplina.Nome;
                    this.txNome_Fantasia.Text = dadosDisciplina.NomeFantasia;
                    this.txHoras_Aula.Text = Convert.ToString(dadosDisciplina.HorasAula);
                    this.txtHorasAtividade.Text = dadosDisciplina.HorasAtiv.HasValue ? Convert.ToString(dadosDisciplina.HorasAtiv) : "0";
                    this.txHoras_Estagio.Text = Convert.ToString(dadosDisciplina.HorasEstagio);
                    this.txNota_Max.Text = dadosDisciplina.NotaMax;
                    this.txN_Casas_Dec.Text = Convert.ToString(dadosDisciplina.NCasasDec);

                    //Preencher check boxes
                    if (dadosDisciplina.Ativa == "S")
                    {
                        ckAtiva.Checked = true;
                    }
                    else
                    {
                        ckAtiva.Checked = false;
                    }

                    if (dadosDisciplina.Multipla == "S")
                    {
                        chkMultipla.Checked = true;
                        pcDisciplina.TabPages[3].ClientEnabled = true;
                    }
                    else
                    {
                        chkMultipla.Checked = false;
                        pcDisciplina.TabPages[3].ClientEnabled = false;
                    }

                    if (dadosDisciplina.VerificaHorario == "S")
                    {
                        ckVerifica_Horario.Checked = true;
                    }
                    else
                    {
                        ckVerifica_Horario.Checked = false;
                    }

                    if (dadosDisciplina.Campo01 == "Atividade")
                    {
                        ckAtividadeCompl.Checked = true;
                    }
                    else
                    {
                        ckAtividadeCompl.Checked = false;
                    }

                    if (dadosDisciplina.Estagio == "S")
                    {
                        ckEstagio.Checked = true;
                    }
                    else
                    {
                        ckEstagio.Checked = false;
                    }

                    if (dadosDisciplina.TemNota == "S")
                    {
                        ckTem_Nota.Checked = true;
                    }
                    else
                    {
                        ckTem_Nota.Checked = false;
                    }

                    if (dadosDisciplina.TemFreq == "S")
                    {
                        ckTem_Freq.Checked = true;
                    }
                    else
                    {
                        ckTem_Freq.Checked = false;
                    }

                    if (dadosDisciplina.TemAvalDescritiva == "S")
                    {
                        chkRelatorio.Checked = true;
                    }
                    else
                    {
                        chkRelatorio.Checked = false;
                    }

                    if (dadosDisciplina.PriorizaFreq == "S")
                    {
                        ckPrioriza_Freq.Checked = true;
                    }
                    else
                    {
                        ckPrioriza_Freq.Checked = false;
                    }

                    chkEletiva.Checked = dadosDisciplina.Eletiva == "S" ? true : false;

                    if (chkEletiva.Checked)
                    {
                        if (!dadosDisciplina.Grupo.IsNullOrEmptyOrWhiteSpace())
                        {
                            rblGrupoEletiva.SelectedValue = dadosDisciplina.Grupo;
                            pnlGrupoEletiva.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        //Eventos dos botões
        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = "";
            _tipoOperacao = Operacao.Novo;
            ControlarTipoOperacao();
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = "";
            _tipoOperacao = Operacao.Excluir;
            ControlarTipoOperacao();
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = "";
            _tipoOperacao = Operacao.Alterar;
            ControlarTipoOperacao();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = "";
            _tipoOperacao = Operacao.Inicial;
            ControlarTipoOperacao();
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.Disciplina rnDisciplina = new Techne.Lyceum.RN.Disciplina();
                LyDisciplina disciplina = new LyDisciplina();
                ValidacaoDados validaDisciplina = new ValidacaoDados();
                string mensagem = string.Empty;

                if (!cvHoras.IsValid)
                {
                    lblMensagem.Text = cvHoras.ToolTip;
                    return;
                }

                disciplina = ObterDados();

                if (this._tipoOperacao.Equals(Operacao.Novo))
                {
                    validaDisciplina = rnDisciplina.ValidaInsercao(disciplina);
                    if (validaDisciplina.Valido)
                    {
                        rnDisciplina.InsereDisciplina(disciplina);
                    }
                    else
                    {
                        lblMensagem.Text = validaDisciplina.Mensagem.Replace(Environment.NewLine, "<br />");

                        return;
                    }
                    tseDisciplina.DBValue = txtDisciplina.Text;
                    mensagem = "Componente Curricular incluída com sucesso.";
                }
                else if (this._tipoOperacao.Equals(Operacao.Alterar))
                {
                    validaDisciplina = rnDisciplina.ValidaAlteracao(disciplina, tseDisciplina["tem_nota"].ToString(), tseDisciplina["tem_freq"].ToString());
                    if (validaDisciplina.Valido)
                    {
                        rnDisciplina.AlteraDisciplina(disciplina, tseDisciplina["tem_nota"].ToString(), tseDisciplina["tem_freq"].ToString());
                    }
                    else
                    {
                        lblMensagem.Text = validaDisciplina.Mensagem.Replace(Environment.NewLine, "<br />");

                        return;
                    }

                    mensagem = "Componente Curricular alterada com sucesso.";
                }
                lblMensagem.Text = mensagem;
                _tipoOperacao = Operacao.Consultar;
                grdGrupoHabilitacoes.DataBind();
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private LyDisciplina ObterDados()
        {
            LyDisciplina dadosDisciplina = new LyDisciplina();

            try
            {
                //CAMPOS DEFAULT
                dadosDisciplina.Disciplina = txtDisciplina.Text;
                dadosDisciplina.Nome = dadosDisciplina.NomeCompl = txNome.Text;
                dadosDisciplina.NomeFantasia = txNome_Fantasia.Text;
                dadosDisciplina.Faculdade = "99999999";
                dadosDisciplina.NotaMaxMedia = "10";
                dadosDisciplina.NCasasDecMedia = 1;
                dadosDisciplina.AvalCompetencia = "N";
                dadosDisciplina.Pim = "N";
                dadosDisciplina.CopiaNotaSubturma = "S";
                dadosDisciplina.TruncaMedia = "N";
                dadosDisciplina.PercPresmin = 0;
                dadosDisciplina.PrazoRevisao = 0;
                dadosDisciplina.PrazoDivulgacao = null;
                dadosDisciplina.Depto = "GERAL";
                dadosDisciplina.Creditos = 0;
                dadosDisciplina.HorasAtiv = 0;
                dadosDisciplina.HorasLab = 0;

                dadosDisciplina.Componente = ddlComponente.SelectedValue;
                if (!string.IsNullOrEmpty(ddlAreaConhecimento.SelectedValue))
                {
                    dadosDisciplina.AreaConhecimento = ddlAreaConhecimento.SelectedValue;
                }
                else
                {
                    dadosDisciplina.AreaConhecimento = null;
                }

                int aula_sem = 0;
                if (!string.IsNullOrEmpty(txHorasSemanais.Text.ToString()))
                {
                    dadosDisciplina.AulasSemAula = Convert.ToInt32(txHorasSemanais.Text);
                    aula_sem = aula_sem + Convert.ToInt32(dadosDisciplina.AulasSemAula);
                }
                else
                {
                    dadosDisciplina.AulasSemAula = null;
                }

                dadosDisciplina.AulasSemAtiv = null;
                dadosDisciplina.AulasSemLab = null;
                dadosDisciplina.AulasSemanais = aula_sem;

                if (!string.IsNullOrEmpty(txHoras_Aula.Text.ToString()))
                {
                    dadosDisciplina.HorasAula = Convert.ToInt32(txHoras_Aula.Text);
                }
                else
                {
                    dadosDisciplina.HorasAula = 0;
                }

                if (!string.IsNullOrEmpty(txHoras_Estagio.Text.ToString()))
                    dadosDisciplina.HorasEstagio = Convert.ToInt32(txHoras_Estagio.Text);
                else
                {
                    dadosDisciplina.HorasEstagio = 0;
                }

                if (!string.IsNullOrEmpty(txtHorasAtividade.Text.ToString()))
                {
                    dadosDisciplina.HorasAtiv = Convert.ToInt32(txtHorasAtividade.Text);
                }
                else
                {
                    dadosDisciplina.HorasAtiv = 0;
                }

                if (string.IsNullOrEmpty(ddlGrupo_Nota.SelectedValue))
                {
                    dadosDisciplina.GrupoNota = null;
                    dadosDisciplina.NotaMax = txNota_Max.Text;
                }
                else
                {
                    dadosDisciplina.GrupoNota = ddlGrupo_Nota.SelectedValue;
                    dadosDisciplina.NotaMax = ddlConceitoNota.SelectedValue;
                }
                if (!string.IsNullOrEmpty(txN_Casas_Dec.Text.ToString()))
                {
                    dadosDisciplina.NCasasDec = Convert.ToDecimal(txN_Casas_Dec.Text);
                }
                else
                {
                    dadosDisciplina.NCasasDec = null;
                }

                //Check boxes

                dadosDisciplina.Ativa = (ckAtiva.Checked) ? "S" : "N";
                dadosDisciplina.Multipla = (chkMultipla.Checked) ? "S" : "N";
                dadosDisciplina.VerificaHorario = (ckVerifica_Horario.Checked) ? "S" : "N";
                dadosDisciplina.Campo01 = (ckAtividadeCompl.Checked) ? "Atividade" : "Disciplina";
                dadosDisciplina.Estagio = (ckEstagio.Checked) ? "S" : "N";
                dadosDisciplina.TemNota = (ckTem_Nota.Checked) ? "S" : "N";
                dadosDisciplina.TemFreq = (ckTem_Freq.Checked) ? "S" : "N";
                dadosDisciplina.TemAvalDescritiva = (chkRelatorio.Checked) ? "S" : "N";

                dadosDisciplina.PriorizaFreq = "S";

                dadosDisciplina.Eletiva = chkEletiva.Checked ? "S" : "N";

                dadosDisciplina.Grupo = !rblGrupoEletiva.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblGrupoEletiva.SelectedValue : null;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            return dadosDisciplina;
        }

        protected void ddlGrupo_Nota_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (ddlGrupo_Nota.SelectedValue != "")
                {
                    lblCasasDecNota.Visible = false;
                    txN_Casas_Dec.ReadOnly = true;
                    txN_Casas_Dec.Visible = false;

                    txNota_Max.Visible = false;
                    rfvNotaMax.Enabled = false;
                    rfvNotaMax.Visible = false;

                    ddlConceitoNota.Visible = true;
                    rfvConceitoNota.Enabled = true;

                    lblCasasDecNota.Visible = false;
                    txN_Casas_Dec.ReadOnly = true;
                    txN_Casas_Dec.Visible = false;

                    lblNotaMaxima.Text = "Nota Máxima: ";
                }
                else
                {
                    if (string.IsNullOrEmpty(txNota_Max.Attributes["onkeypress"]))
                    {
                        txNota_Max.Attributes.Add("onkeypress", "return NumeroComVirgula(event);");
                        txNota_Max.Attributes.Add("onChange", "validaDecimais(this);");
                    }

                    txNota_Max.Text = string.Empty;
                    lblCasasDecNota.Visible = true;
                    txN_Casas_Dec.ReadOnly = false;
                    txN_Casas_Dec.Visible = true;

                    txNota_Max.Visible = true;
                    rfvNotaMax.Enabled = true;
                    rfvNotaMax.Visible = true;

                    ddlConceitoNota.Visible = false;
                    rfvConceitoNota.Enabled = false;

                    lblCasasDecNota.Visible = true;
                    txN_Casas_Dec.ReadOnly = false;
                    txN_Casas_Dec.Visible = true;

                    lblNotaMaxima.Text = "Nota Máxima: ";
                }

                CarregarDadosDrop("DDLCONCEITONOTA");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlComponente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlComponente.SelectedValue.ToUpper() == "BASE NACIONAL COMUM")
            {
                rfvAreaConhecimento.Enabled = true;
                lblAreaConhecimento.Text = "Área de Conhecimento*:";
                lblAreaConhecimento.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            }
            else
            {
                rfvAreaConhecimento.Enabled = false;
                lblAreaConhecimento.Text = "Área de Conhecimento:";
                lblAreaConhecimento.Style.Add(HtmlTextWriterStyle.FontWeight, "normal");
            }
        }

        protected void ValidaHoras_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = new String[] { txHoras_Aula.Text, txtHorasAtividade.Text, txHoras_Estagio.Text }
                .Where(str => !String.IsNullOrEmpty(str) && decimal.Parse(str) > 0)
                .Count() > 0;
        }

        protected void grdDisciplinaMultipla_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs asPxGridViewAfterPerformCallbackEventArgs)
        {
            this.ControlaAcesso(this.grdDisciplinaMultipla);
        }

        protected void grdDisciplinaMultipla_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var disciplina = Convert.ToString(e.GetListSourceFieldValue("disciplina"));
                var disciplina_multipla = Convert.ToString(e.GetListSourceFieldValue("disciplina_multipla"));
                e.Value = disciplina + ";" + disciplina_multipla;
            }
        }

        protected void grdDisciplinaMultipla_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            if (tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull)
            {
                e.NewValues["disciplina"] = tseDisciplina.DBValue.ToString();
            }

            grdDisciplinaMultipla.Settings.ShowFilterRow = false;
        }

        protected void grdDisciplinaMultipla_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            RN.DisciplinaMultipla rnDisciplinaMultipla = new DisciplinaMultipla();
            LyDisciplinaMultipla discMult = new LyDisciplinaMultipla();

            discMult = rnDisciplinaMultipla.Bind(null, e.NewValues);
            discMult.Disciplina = tseDisciplina.DBValue.ToString();
            rnDisciplinaMultipla.Insere(discMult);

            e.Cancel = true;
            this.grdDisciplinaMultipla.CancelEdit();
        }
        public void DeleteGrupoHabilitacao(object CompositeKey) { }

        protected void grdGrupoHabilitacoes_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var disciplina = Convert.ToString(tseDisciplina.DBValue);
                var agrupamento = Convert.ToString(e.GetListSourceFieldValue("agrupamento"));
                e.Value = disciplina + ";" + agrupamento;
            }
        }

        protected void grdGrupoHabilitacoes_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            if (tseDisciplina.IsValidDBValue && !tseDisciplina.DBValue.IsNull)
            {
                e.NewValues["disciplina"] = tseDisciplina.DBValue.ToString();
            }

            grdDisciplinaMultipla.Settings.ShowFilterRow = false;
        }

        protected void grdGrupoHabilitacoes_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.GrupoHabilitacao rnGrupoHabilitacao = new Techne.Lyceum.RN.GrupoHabilitacao();
            string agrupamento, disciplina;

            disciplina = Convert.ToString(e.Values["CompositeKey"]).Split(';')[0];
            agrupamento = Convert.ToString(e.Values["CompositeKey"]).Split(';')[1];


            rnGrupoHabilitacao.RemoveGrupoDisciplina(tseDisciplina.DBValue.ToString(), agrupamento);
            grdGrupoHabilitacoes.DataBind();

        }

        protected void grdDisciplinaMultipla_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            RN.DisciplinaMultipla rnDisciplinaMultipla = new DisciplinaMultipla();
            LyDisciplinaMultipla discMult = new LyDisciplinaMultipla();
            ValidacaoDados validacao = new ValidacaoDados();

            if (tseDisciplina.DBValue.IsNull)
            {
                e.RowError = "O campo Disciplina é obrigatório.";
                return;
            }

            //discMult = rnDisciplinaMultipla.Bind(e.Keys, e.NewValues);

            discMult.Disciplina = tseDisciplina.DBValue.ToString();
            discMult.DisciplinaMultipla = e.NewValues["disciplina_multipla"].ToString();

            validacao = rnDisciplinaMultipla.Valida(discMult);

            if (!validacao.Valido)
            {
                e.RowError = validacao.Mensagem;
            }
        }

        protected void grdDisciplinaMultipla_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            RN.DisciplinaMultipla rnDisciplinaMultipla = new DisciplinaMultipla();
            ValidacaoDados validacao = new ValidacaoDados();
            LyDisciplinaMultipla discMult = new LyDisciplinaMultipla();

            discMult.Disciplina = Convert.ToString(e.Values["CompositeKey"]).Split(';')[0];
            discMult.DisciplinaMultipla = Convert.ToString(e.Values["CompositeKey"]).Split(';')[1];

            validacao = rnDisciplinaMultipla.ValidaRemocao(discMult);

            if (!validacao.Valido)
            {
                throw new ApplicationException(validacao.Mensagem);
            }
            else
            {
                rnDisciplinaMultipla.Remove(discMult);
            }

            e.Cancel = true;
            this.grdDisciplinaMultipla.CancelEdit();
        }
    }
}
