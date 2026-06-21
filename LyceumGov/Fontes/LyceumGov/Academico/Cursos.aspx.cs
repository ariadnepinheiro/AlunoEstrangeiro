using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.ASPxGridView;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/Cursos.aspx"),
    ControlText("Cursos"),
    Title("Escolaridades")]
    public partial class Cursos : TPage
    {
        #region Propriedades e Enumeradores
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        public object Listar(DbObject tseCurso)
        {
            RN.CursoDuracao rnCursoDuracao = new Techne.Lyceum.RN.CursoDuracao();
            DataTable dt = new DataTable();

            if (!tseCurso.IsNull)
            {
                dt = rnCursoDuracao.ListaPor(tseCurso.ToString());
            }

            return dt;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtDOU.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                lblMensagem.Text = string.Empty;


                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDuracao, "Duração das aulas por ano");
        }

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

        #region Métodos

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        tseCurso.ResetValue();
                        pnAbas.Visible = false;
                        pcCursoDuracao.TabPages[1].Enabled = false;
                        pnlComposicaoItinerarioFormativo.Visible = false;
                        pnlUnidadeCurricular.Visible = false;
                        pnlAreaItinerarioFormativo.Visible = false;
                        pnlTipoCursoItinerario.Visible = false;
                        chkUnidadeCurricular.ClearSelection();
                        chkAreaItinerarioFormativo.ClearSelection();
                        chkComposicaoItinerario.ClearSelection();
                        rblTipoCursoItinerario.ClearSelection();
                        pnlTrilha.Visible = false;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                        ControlarVisibilidadeControle(controles);
                        pnAbas.Visible = true;
                        if (!string.IsNullOrEmpty(txtCurso.Text))
                        {
                            tseCurso.DBValue = txtCurso.Text;
                        }
                        this.odsDuracao.Select();
                        this.odsDuracao.DataBind();
                        this.grdDuracao.DataBind();
                        pcCursoDuracao.TabPages[1].Enabled = true;
                        DesabilitaCampos();
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        LimparTela();
                        tseCurso.ResetValue();
                        tseCurso.Enabled = false;
                        pnlComposicaoItinerarioFormativo.Visible = false;
                        pnlUnidadeCurricular.Visible = false;
                        pnlAreaItinerarioFormativo.Visible = false;
                        pnlTipoCursoItinerario.Visible = false;
                        pnlTrilha.Visible = false;
                        CarregaUnidadeCurricular();
                        CarregaAreaItinerarioFormativo();

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        lblMensagem.Text = string.Empty;

                        tseCurso.ResetValue();
                        tseCurso.Enabled = false;

                        this.odsDuracao.Select();
                        this.odsDuracao.DataBind();
                        this.grdDuracao.DataBind();

                        pnAbas.Visible = true;
                        pcCursoDuracao.TabPages[1].Enabled = false;

                        this.HabilitaCampos();

                        this.CarregarDadosDrop(ddlModalidade.ID);
                        this.CarregarDadosDrop(ddlNivel.ID);

                        chkSalaExterna.Checked = false;
                        chkParticipaCalculoNovasTurmasTurnosVagas.Checked = true;
                        chkParticipaFechamentoAutomatico.Checked = true;
                        chkPermiteTransferenciaTurmaTotal.Checked = false;
                        chkChoqueHorarioIntegral.Checked = false;

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        ValidacaoDados validacao = new ValidacaoDados();
                        RN.Curso rnCurso = new Techne.Lyceum.RN.Curso();

                        validacao = rnCurso.ValidaRemocao(txtCurso.Text);

                        if (validacao.Valido)
                        {
                            rnCurso.Remove(txtCurso.Text);

                            LimparTela();
                            lblMensagem.Text = "Curso excluído com sucesso.";
                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseCurso.Enabled = false;

                        HabilitaCampos();
                        txtCurso.ReadOnly = true;
                        this.grdDuracao.CancelEdit();
                        pcCursoDuracao.ActiveTabIndex = 0;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        if (txtCurso.Text == "9999.91" || txtCurso.Text == "9999.92")
                        {
                            chkConcomitante.Enabled = false;
                        }
                        else
                        {
                            chkConcomitante.Enabled = true;
                        }

                        if (chkSalaExterna.Checked)
                            hdnSalaExterna.Value = "S";
                        else
                            hdnSalaExterna.Value = "N";

                        if (chkParticipaCalculoNovasTurmasTurnosVagas.Checked)
                            hdnParticipaCalculoNovasTurmasTurnosVagas.Value = "S";
                        else
                            hdnParticipaCalculoNovasTurmasTurnosVagas.Value = "N";

                        if (chkParticipaFechamentoAutomatico.Checked)
                            hdnParticipaFechamentoAutomatico.Value = "S";
                        else
                            hdnParticipaFechamentoAutomatico.Value = "N";

                        if (chkPermiteTransferenciaTurmaTotal.Checked)
                            hdnPermiteTransferenciaTurmaTotal.Value = "S";
                        else
                            hdnPermiteTransferenciaTurmaTotal.Value = "N";

                        if (chkChoqueHorarioIntegral.Checked)
                            hdnChoqueHorarioIntegral.Value = "S";
                        else
                            hdnChoqueHorarioIntegral.Value = "N";
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        lblMensagem.Text = string.Empty;
                        pnlComposicaoItinerarioFormativo.Visible = false;
                        pnlUnidadeCurricular.Visible = false;
                        pnlAreaItinerarioFormativo.Visible = false;
                        CarregaUnidadeCurricular();
                        CarregaAreaItinerarioFormativo();

                        tseCurso.Enabled = true;

                        RN.DTOs.DadosCurso dadosCurso = new Techne.Lyceum.RN.DTOs.DadosCurso();
                        RN.Curso rnCurso = new Techne.Lyceum.RN.Curso();
                        string curso = tseCurso.DBValue.ToString();

                        dadosCurso = rnCurso.ObtemDadosCursoPor(curso);


                        if (dadosCurso != null)
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnExcluir };
                            this.ControlarVisibilidadeControle(controles);
                            pnAbas.Visible = true;
                            pcCursoDuracao.TabPages[1].Enabled = true;

                            this.CarregarDadosDrop(ddlModalidade.ID);
                            this.CarregarDadosDrop(ddlNivel.ID);
                            this.PreencherDadosTela(dadosCurso);

                            this.DesabilitaCampos();
                        }
                        else
                        {
                            LimparTela();
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            pcCursoDuracao.TabPages[1].Enabled = false;
                            pnAbas.Visible = false;
                            lblMensagem.Text = "Escolaridade não cadastrada.";
                        }
                        break;
                    }
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        /// <summary>
        /// Armazena uma nova linha com os dados da tela no datatable passado como parâmetro
        /// </summary>
        /// <param name="dtDocente">DataTable do docente que será adicionado uma nova linha</param>
        private RN.DTOs.DadosCurso ObterDados()
        {
            RN.DTOs.DadosCurso dadosCurso = new Techne.Lyceum.RN.DTOs.DadosCurso();

            if (!string.IsNullOrEmpty(txtCurso.Text))
                dadosCurso.Curso = txtCurso.Text;

            if (!string.IsNullOrEmpty(txtMnemonico.Text))
                dadosCurso.Mnemonico = txtMnemonico.Text;

            if (!string.IsNullOrEmpty(txtNome.Text))
                dadosCurso.Nome = txtNome.Text.Trim().ToUpper();

            if (!string.IsNullOrEmpty(txtTitulo.Text))
                dadosCurso.Titulo = txtTitulo.Text.Trim().ToUpper();

            if (!string.IsNullOrEmpty(txtHabilitacao.Text))
                dadosCurso.Habilitacao = txtHabilitacao.Text;

            if (!string.IsNullOrEmpty(txtDecreto.Text))
                dadosCurso.Decreto = txtDecreto.Text;

            if (!string.IsNullOrEmpty(txtVagas.Text))
                dadosCurso.Vagas = Convert.ToInt32(txtVagas.Text);

            if (!string.IsNullOrEmpty(ddlNivel.SelectedValue))
                dadosCurso.Tipo = ddlNivel.SelectedValue;

            if (!string.IsNullOrEmpty(ddlModalidade.SelectedValue))
                dadosCurso.Modalidade = ddlModalidade.SelectedValue;

            if (!string.IsNullOrEmpty(dtDOU.Text))
                dadosCurso.Dt_dou = dtDOU.Date;

            if (chkAtivo.Checked)
                dadosCurso.Ativo = "S";
            else
                dadosCurso.Ativo = "N";

            if (chkReclassificacao.Checked)
                dadosCurso.Tem_reclassificacao = "S";
            else
                dadosCurso.Tem_reclassificacao = "N";

            if (chkFormatura.Checked)
                dadosCurso.Formatura = "S";
            else
                dadosCurso.Formatura = "N";

            if (chkSalaExterna.Checked)
            {
                dadosCurso.Salaexterna = "S";
            }
            else
            {
                dadosCurso.Salaexterna = "N";
            }

            if (chkParticipaCalculoNovasTurmasTurnosVagas.Checked)
            {
                dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas = "S";
            }
            else
            {
                dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas = "N";
            }

            if (chkParticipaFechamentoAutomatico.Checked)
            {
                dadosCurso.ParticipaFechamentoAutomatico = "S";
            }
            else
            {
                dadosCurso.ParticipaFechamentoAutomatico = "N";
            }

            if (chkPermiteTransferenciaTurmaTotal.Checked)
            {
                dadosCurso.PermiteTransferenciaTurmaTotal = "S";
            }
            else
            {
                dadosCurso.PermiteTransferenciaTurmaTotal = "N";
            }



            if (chkChoqueHorarioIntegral.Checked)
            {
                dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas = "S";
            }
            else
            {
                dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas = "N";
            }
            if (!string.IsNullOrEmpty(ddlTipoCurso.SelectedValue))
                dadosCurso.Tipo_curso = ddlTipoCurso.SelectedValue;

            if (chkConcomitante.Checked)
                dadosCurso.Concomitante = "S";
            else
                dadosCurso.Concomitante = "N";

            //FIXOS:
            dadosCurso.Faculdade = "99999999";

            string depto = RN.Curso.ConsultarTopDepto("99999999");

            if (!string.IsNullOrEmpty(depto))
                dadosCurso.Depto = depto;

            if (chkOfertaEletiva.Checked)
            {
                dadosCurso.OfertaEletiva = "S";
            }
            else
            {
                dadosCurso.OfertaEletiva = "N";
            }

            dadosCurso.NaoSeAplica = "N";
            dadosCurso.FormacaoBasica = "N";
            dadosCurso.ItinerarioFormativo = "N";

            if (rblEstruturaCurricular.SelectedValue == "Nao")
            {
                dadosCurso.NaoSeAplica = "S";
            }
            else if (rblEstruturaCurricular.SelectedValue == "FormacaoBasica")
            {
                dadosCurso.FormacaoBasica = "S";
            }
            else if (rblEstruturaCurricular.SelectedValue == "Itinerario")
            {
                dadosCurso.ItinerarioFormativo = "S";
            }

            List<int> unidade = new List<int>();

            foreach (ListItem item in chkUnidadeCurricular.Items)
            {
                if (item.Selected)
                {
                    unidade.Add(Convert.ToInt32(item.Value));
                }
            }
            dadosCurso.UnidadesCurricular = unidade;

            List<int> area = new List<int>();

            foreach (ListItem item in chkAreaItinerarioFormativo.Items)
            {
                if (item.Selected)
                {
                    area.Add(Convert.ToInt32(item.Value));
                }
            }

            List<int> composicao = new List<int>();

            foreach (ListItem item in chkComposicaoItinerario.Items)
            {
                if (item.Selected)
                {
                    composicao.Add(Convert.ToInt32(item.Value));
                }
            }


            dadosCurso.ComposicaoItinerarioFormativoIntegrado = composicao;
            dadosCurso.AreaItinerarioFormativo = area;

            List<int> tipo = new List<int>();

            foreach (ListItem item in rblTipoCursoItinerario.Items)
            {
                if (item.Selected)
                {
                    tipo.Add(Convert.ToInt32(item.Value));
                }
            }

            dadosCurso.TipoItinerarioFormacaoTecnicaProfissional = tipo;

            dadosCurso.UsuarioId = User.Identity.Name;
            dadosCurso.ItinerarioFormativoId = !ddlItinerario.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlItinerario.SelectedValue) : (int?)null;

            dadosCurso.TrilhaAprendizagem = !ddlTrilha.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTrilha.SelectedValue) : (int?)null;
            dadosCurso.MaximoComponentes = !txtMaxComponente.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtMaxComponente.Text) : (int?)null;


            return dadosCurso;
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosTela(RN.DTOs.DadosCurso dadosCurso)
        {
            txtCurso.Text = Convert.ToString(dadosCurso.Curso);
            txtMnemonico.Text = Convert.ToString(dadosCurso.Mnemonico);
            txtNome.Text = Convert.ToString(dadosCurso.Nome);
            txtTitulo.Text = Convert.ToString(dadosCurso.Titulo);
            txtHabilitacao.Text = Convert.ToString(dadosCurso.Habilitacao);
            txtDecreto.Text = Convert.ToString(dadosCurso.Decreto);
            txtVagas.Text = Convert.ToString(dadosCurso.Vagas);
            dtDOU.Date = Convert.ToDateTime(dadosCurso.Dt_dou);
            PreencherDadoCombo(ddlNivel, Convert.ToString(dadosCurso.Tipo));
            PreencherDadoCombo(ddlModalidade, Convert.ToString(dadosCurso.Modalidade));
            PreencherDadoCombo(ddlTipoCurso, Convert.ToString(dadosCurso.Tipo_curso));

            if (dadosCurso.Ativo == "S")
            {
                chkAtivo.Checked = true;
            }
            else
            {
                chkAtivo.Checked = false;
            }

            if (dadosCurso.Tem_reclassificacao == "S")
            {
                chkReclassificacao.Checked = true;
            }
            else
            {
                chkReclassificacao.Checked = false;
            }

            if (dadosCurso.Formatura == "S")
            {
                chkFormatura.Checked = true;
            }
            else
            {
                chkFormatura.Checked = false;
            }

            if (dadosCurso.Curso == "9999.91" || dadosCurso.Curso == "9999.92")
            {
                chkConcomitante.Enabled = false;
            }
            else
            {
                chkConcomitante.Enabled = true;
                chkConcomitante.Checked = dadosCurso.Concomitante == "S";
            }

            if (dadosCurso.ParticipaCalculoNovasTurmasTurnosVagas == "S")
            {
                chkParticipaCalculoNovasTurmasTurnosVagas.Checked = true;
                hdnParticipaCalculoNovasTurmasTurnosVagas.Value = "S";
            }
            else
            {
                chkParticipaCalculoNovasTurmasTurnosVagas.Checked = false;
                hdnParticipaCalculoNovasTurmasTurnosVagas.Value = "N";
            }

            if (dadosCurso.ParticipaFechamentoAutomatico == "S")
            {
                chkParticipaFechamentoAutomatico.Checked = true;
                hdnParticipaFechamentoAutomatico.Value = "S";
            }
            else
            {
                chkParticipaFechamentoAutomatico.Checked = false;
                hdnParticipaFechamentoAutomatico.Value = "N";
            }

            if (dadosCurso.PermiteTransferenciaTurmaTotal == "S")
            {
                chkPermiteTransferenciaTurmaTotal.Checked = true;
                hdnPermiteTransferenciaTurmaTotal.Value = "S";
            }
            else
            {
                chkPermiteTransferenciaTurmaTotal.Checked = false;
                hdnPermiteTransferenciaTurmaTotal.Value = "N";
            }


            if (dadosCurso.PermiteChoqueTurnoIntegralTurnosVagas == "S")
            {
                chkChoqueHorarioIntegral.Checked = true;
                hdnChoqueHorarioIntegral.Value = "S";
            }
            else
            {
                chkChoqueHorarioIntegral.Checked = false;
                hdnChoqueHorarioIntegral.Value = "N";
            }
            if (dadosCurso.Salaexterna == "S")
            {
                chkSalaExterna.Checked = true;
                hdnSalaExterna.Value = "S";
            }
            else
            {
                chkSalaExterna.Checked = false;
                hdnSalaExterna.Value = "N";
            }

            chkOfertaEletiva.Checked = dadosCurso.OfertaEletiva == "S" ? true : false;

            if (dadosCurso.ItinerarioFormativo == "N" && dadosCurso.FormacaoBasica == "N")
            {
                rblEstruturaCurricular.SelectedValue = "Nao";
            }
            else if (dadosCurso.ItinerarioFormativo == "S")
            {
                rblEstruturaCurricular.SelectedValue = "Itinerario";
            }
            else if (dadosCurso.FormacaoBasica == "S")
            {
                rblEstruturaCurricular.SelectedValue = "FormacaoBasica";
            }

            rblEstruturaCurricular_SelectedIndexChanged(null, null);

            foreach (var item in dadosCurso.AreaItinerarioFormativo)
            {
                if (chkAreaItinerarioFormativo.Items.FindByValue(item.ToString()) != null)
                {
                    chkAreaItinerarioFormativo.Items.FindByValue(item.ToString()).Selected = true;
                }
            }

            chkAreaItinerarioFormativo_SelectedIndexChanged(null, null);

            foreach (var item in dadosCurso.TipoItinerarioFormacaoTecnicaProfissional)
            {
                if (rblTipoCursoItinerario.Items.FindByValue(item.ToString()) != null)
                {
                    rblTipoCursoItinerario.Items.FindByValue(item.ToString()).Selected = true;
                }
            }

            foreach (var item in dadosCurso.ComposicaoItinerarioFormativoIntegrado)
            {
                if (chkComposicaoItinerario.Items.FindByValue(item.ToString()) != null)
                {
                    chkComposicaoItinerario.Items.FindByValue(item.ToString()).Selected = true;
                }
            }

            foreach (var item in dadosCurso.UnidadesCurricular)
            {
                chkUnidadeCurricular.Items.FindByValue(item.ToString()).Selected = true;
            }

            if (dadosCurso.ItinerarioFormativoId.HasValue)
            {
                ddlItinerario.SelectedValue = Convert.ToString(dadosCurso.ItinerarioFormativoId);
            }

            if (!ddlItinerario.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                CarregaTrilha(Convert.ToInt32(ddlItinerario.SelectedValue));

                if (dadosCurso.TrilhaAprendizagem.HasValue)
                {
                    ddlTrilha.SelectedValue = Convert.ToString(dadosCurso.TrilhaAprendizagem);
                }
            }

            txtMaxComponente.Text = dadosCurso.MaximoComponentes.ToString();
        }

        /// <summary>
        /// Limpa todas as textbox e combobox.
        /// </summary>
        protected void LimparTela()
        {
            txtCurso.Text = string.Empty;
            txtMnemonico.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtHabilitacao.Text = string.Empty;
            txtDecreto.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            txtVagas.Text = string.Empty;
            dtDOU.Text = string.Empty;
            ddlNivel.Items.Clear();
            ddlModalidade.Items.Clear();
            ddlTipoCurso.ClearSelection();
            chkAtivo.Checked = false;
            chkReclassificacao.Checked = false;
            chkFormatura.Checked = false;
            chkConcomitante.Checked = false;
            chkSalaExterna.Checked = false;
            this.grdDuracao.CancelEdit();
            grdDuracao.FilterExpression = string.Empty;
            chkOfertaEletiva.Checked = false;
            chkUnidadeCurricular.ClearSelection();
            chkAreaItinerarioFormativo.ClearSelection();
            chkComposicaoItinerario.ClearSelection();
            rblTipoCursoItinerario.ClearSelection();
            rblEstruturaCurricular.ClearSelection();
            txtMaxComponente.Text = string.Empty;
            ddlTrilha.Items.Clear();
            ddlItinerario.Items.Clear();
        }

        /// <summary>
        /// Habilita todos os campos para edição
        /// </summary>
        protected void HabilitaCampos()
        {
            txtCurso.ReadOnly = false;
            txtMnemonico.ReadOnly = false;
            txtNome.ReadOnly = false;
            txtHabilitacao.ReadOnly = false;
            txtDecreto.ReadOnly = false;
            txtVagas.ReadOnly = false;
            txtTitulo.ReadOnly = false;
            dtDOU.Enabled = true;
            ddlNivel.Enabled = true;
            ddlModalidade.Enabled = true;
            ddlTipoCurso.Enabled = true;
            chkFormatura.Enabled = true;
            chkAtivo.Enabled = true;
            chkReclassificacao.Enabled = true;
            chkConcomitante.Enabled = true;
            chkSalaExterna.Enabled = true;
            chkParticipaCalculoNovasTurmasTurnosVagas.Enabled = true;
            chkParticipaFechamentoAutomatico.Enabled = true;
            chkPermiteTransferenciaTurmaTotal.Enabled = true;
            chkChoqueHorarioIntegral.Enabled = true;
            chkOfertaEletiva.Enabled = true;
            pnlAreaItinerarioFormativo.Enabled = true;
            pnlComposicaoItinerarioFormativo.Enabled = true;
            pnlTipoCursoItinerario.Enabled = true;
            rblEstruturaCurricular.Enabled = true;
            pnlUnidadeCurricular.Enabled = true;
            ddlTrilha.Enabled = true;
            ddlItinerario.Enabled = true;
            txtMaxComponente.Enabled = true;
        }

        /// <summary>
        /// Desabilita todos os campos para edição.
        /// </summary>
        protected void DesabilitaCampos()
        {
            txtCurso.ReadOnly = true;
            txtMnemonico.ReadOnly = true;
            txtNome.ReadOnly = true;
            txtHabilitacao.ReadOnly = true;
            txtDecreto.ReadOnly = true;
            txtVagas.ReadOnly = true;
            txtTitulo.ReadOnly = true;
            dtDOU.Enabled = false;
            ddlNivel.Enabled = false;
            ddlModalidade.Enabled = false;
            ddlTipoCurso.Enabled = false;
            chkFormatura.Enabled = false;
            chkAtivo.Enabled = false;
            chkReclassificacao.Enabled = false;
            chkConcomitante.Enabled = false;
            chkSalaExterna.Enabled = false;
            chkParticipaCalculoNovasTurmasTurnosVagas.Enabled = false;
            chkParticipaFechamentoAutomatico.Enabled = false;
            chkPermiteTransferenciaTurmaTotal.Enabled = false;
            chkChoqueHorarioIntegral.Enabled = false;
            this.grdDuracao.CancelEdit();
            chkOfertaEletiva.Enabled = false;
            pnlAreaItinerarioFormativo.Enabled = false;
            pnlComposicaoItinerarioFormativo.Enabled = false;
            pnlTipoCursoItinerario.Enabled = false;
            rblEstruturaCurricular.Enabled = false;
            pnlUnidadeCurricular.Enabled = false;
            ddlTrilha.Enabled = false;
            ddlItinerario.Enabled = false;
            txtMaxComponente.Enabled = false;
        }

        /// <summary>
        /// Carrega os dados do banco na dropdownlist
        /// </summary>
        /// <param name="drop">DropDownList que será carregado</param>
        /// <param name="data">Dados que serão preenchidos</param>
        /// <param name="defaultValue">Valor padrão</param>
        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("<Nenhum>", "");
            drop.Items.Add(itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                drop.SelectedValue = "";
            }
        }

        /// <summary>
        /// Faz a consulta ao banco para cada DropDownList e chama o método para carregá-los
        /// </summary>
        /// <param name="idDrop">ID da DropDownList</param>
        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop)
                {
                    case "ddlNivel":
                        {
                            dadosDrop = RN.Curso.ConsultarTipoCurso();
                            CarregarDropDownList(ddlNivel, dadosDrop, "");
                            break;
                        }
                    case "ddlModalidade":
                        {
                            dadosDrop = RN.Curso.ConsultarModalidadeCurso();

                            CarregarDropDownList(ddlModalidade, dadosDrop, "");
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
        #endregion

        #region Eventos
        protected void pcCursoDuracao_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                RN.Curso rnCurso = new Techne.Lyceum.RN.Curso();

                RN.DTOs.DadosCurso dadosCurso = new Techne.Lyceum.RN.DTOs.DadosCurso();
                dadosCurso = ObterDados();

                validacao = rnCurso.Valida(dadosCurso, _tipoOperacao.Equals(TipoOperacao.Novo));

                if (validacao.Valido)
                {
                    if (_tipoOperacao.Equals(TipoOperacao.Novo))
                    {
                        rnCurso.Insere(dadosCurso);
                        lblMensagem.Text = "Curso inserido com sucesso.";
                    }
                    else
                    {

                        rnCurso.Atualiza(dadosCurso);
                        lblMensagem.Text = "Curso atualizado com sucesso.";
                    }

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

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

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Excluir;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tab_TabClick(object source, DevExpress.Web.ASPxTabControl.TabControlCancelEventArgs e)
        {

        }
        protected void tseCurso_Changed(object sender, EventArgs e)
        {
            try
            {
                RN.CursoDuracao rnCursoDuracao = new Techne.Lyceum.RN.CursoDuracao();

                if (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
                {
                    LimparTela();
                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                    lblMensagem.Text = string.Empty;
                }
                else if (!tseCurso.DBValue.IsNull)
                {
                    lblMensagem.Text = "Escolaridade não cadastrada.";
                }
                else
                {
                    lblMensagem.Text = "Favor consultar uma escolaridade.";
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDuracao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDuracao.Settings.ShowFilterRow = false;
        }

        protected void grdDuracao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

        }

        protected void grdDuracao_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (this.grdDuracao.IsNewRowEditing)
            {
                if (e.Column.FieldName == "ID_CURSO_DURACAO")
                {
                    e.Editor.ReadOnly = true;
                }
                if (e.Column.FieldName == "ANO")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "TURNO")
                {
                    e.Editor.ReadOnly = false;
                }
                if (e.Column.FieldName == "DURACAO")
                {
                    e.Editor.ReadOnly = false;
                }
            }
            else if (this.grdDuracao.IsEditing)
            {
                if (e.Column.FieldName == "ID_CURSO_DURACAO")
                {
                    e.Editor.Enabled = false;
                }
                if (e.Column.FieldName == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }
                if (e.Column.FieldName == "TURNO")
                {
                    e.Editor.ReadOnly = true;
                }
                if (e.Column.FieldName == "DURACAO")
                {
                    e.Editor.Enabled = true;
                }
            }

        }

        protected void grdDuracao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDuracao);
        }

        public void Insert(object ID_CURSO_DURACAO, object ANO, object TURNO, object DURACAO)
        {
        }

        public void Update(object ID_CURSO_DURACAO, object ANO, object TURNO, object DURACAO)
        {
        }

        public void Delete(object ID_CURSO_DURACAO, object ANO, object TURNO, object DURACAO)
        {
        }

        protected void odsDuracao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.CursoDuracao rnCursoDuracao = new Techne.Lyceum.RN.CursoDuracao();
            ValidacaoDados validacao = new ValidacaoDados();
            int idCursoDuracao;

            idCursoDuracao = string.IsNullOrEmpty(Convert.ToString(e.InputParameters["ID_CURSO_DURACAO"])) ? -1 : Convert.ToInt32(e.InputParameters["ID_CURSO_DURACAO"]);

            validacao = rnCursoDuracao.ValidaRemocao(idCursoDuracao);

            if (validacao.Valido)
            {
                rnCursoDuracao.Remove(idCursoDuracao);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

            e.Cancel = true;
            this.grdDuracao.CancelEdit();
        }

        protected void odsDuracao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.CursoDuracao rnCursoDuracao = new Techne.Lyceum.RN.CursoDuracao();
            ValidacaoDados validacao = new ValidacaoDados();
            TceCursoDuracao cursoDuracao;

            cursoDuracao = new TceCursoDuracao
            {
                IdCursoDuracao = string.IsNullOrEmpty(Convert.ToString(e.InputParameters["ID_CURSO_DURACAO"])) ? -1 : Convert.ToInt32(e.InputParameters["ID_CURSO_DURACAO"]),
                Ano = string.IsNullOrEmpty(Convert.ToString(e.InputParameters["ANO"])) ? -1 : Convert.ToInt32(e.InputParameters["ANO"]),
                Turno = Convert.ToString(e.InputParameters["TURNO"]),
                Curso = Convert.ToString(tseCurso.DBValue),
                Duracao = string.IsNullOrEmpty(Convert.ToString(e.InputParameters["DURACAO"])) ? -1 : Convert.ToInt32(e.InputParameters["DURACAO"]),
                Matricula = User.Identity.Name
            };

            validacao = rnCursoDuracao.Valida(cursoDuracao);

            if (validacao.Valido)
            {
                rnCursoDuracao.Altera(cursoDuracao);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

            e.Cancel = true;
            this.grdDuracao.CancelEdit();
        }

        protected void odsDuracao_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.CursoDuracao rnCursoDuracao = new Techne.Lyceum.RN.CursoDuracao();
            ValidacaoDados validacao = new ValidacaoDados();
            TceCursoDuracao cursoDuracao;

            cursoDuracao = new TceCursoDuracao
            {
                Ano = string.IsNullOrEmpty(Convert.ToString(e.InputParameters["ANO"])) ? -1 : Convert.ToInt32(e.InputParameters["ANO"]),
                Turno = Convert.ToString(e.InputParameters["TURNO"]),
                Curso = txtCurso.Text,
                Duracao = string.IsNullOrEmpty(Convert.ToString(e.InputParameters["DURACAO"])) ? -1 : Convert.ToInt32(e.InputParameters["DURACAO"]),
                Matricula = User.Identity.Name
            };

            validacao = rnCursoDuracao.Valida(cursoDuracao);

            if (validacao.Valido)
            {
                rnCursoDuracao.Insere(cursoDuracao);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

            e.Cancel = true;
            this.grdDuracao.CancelEdit();
        }

        #endregion

        private void CarregaUnidadeCurricular()
        {
            try
            {
                RN.Pedagogico.UnidadeCurricular rnUnidadeCurricular = new Techne.Lyceum.RN.Pedagogico.UnidadeCurricular();

                chkUnidadeCurricular.Items.Clear();
                chkUnidadeCurricular.DataSource = rnUnidadeCurricular.ListaUnidadeCurricularAtivo();
                chkUnidadeCurricular.DataTextField = "DESCRICAO";
                chkUnidadeCurricular.DataValueField = "UNIDADECURRICULARID";
                chkUnidadeCurricular.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAreaItinerarioFormativo()
        {
            try
            {
                RN.Pedagogico.AreaItinerarioFormativo rnAreaItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.AreaItinerarioFormativo();

                chkAreaItinerarioFormativo.Items.Clear();
                chkAreaItinerarioFormativo.DataSource = rnAreaItinerarioFormativo.ListaAreaItinerarioFormativoAtivo();
                chkAreaItinerarioFormativo.DataTextField = "DESCRICAO";
                chkAreaItinerarioFormativo.DataValueField = "AREAITINERARIOFORMATIVOID";
                chkAreaItinerarioFormativo.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaComposicaoItinerarioFormativo(int areaItinerario)
        {
            try
            {
                RN.Pedagogico.ComposicaoItinerarioFormativo rnComposicaoItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ComposicaoItinerarioFormativo();

                chkComposicaoItinerario.Items.Clear();
                chkComposicaoItinerario.DataSource = rnComposicaoItinerarioFormativo.ListaComposicaoItinerarioFormativoAtivo(areaItinerario);
                chkComposicaoItinerario.DataTextField = "DESCRICAO";
                chkComposicaoItinerario.DataValueField = "COMPOSICAOITINERARIOFORMATIVOID";
                chkComposicaoItinerario.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTipoCursoItinerarioFormativo(int areaItinerario)
        {
            try
            {
                RN.Pedagogico.ComposicaoItinerarioFormativo rnComposicaoItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ComposicaoItinerarioFormativo();

                rblTipoCursoItinerario.Items.Clear();
                rblTipoCursoItinerario.DataSource = rnComposicaoItinerarioFormativo.ListaComposicaoItinerarioFormativoAtivo(areaItinerario);
                rblTipoCursoItinerario.DataTextField = "DESCRICAO";
                rblTipoCursoItinerario.DataValueField = "COMPOSICAOITINERARIOFORMATIVOID";
                rblTipoCursoItinerario.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaItinerarioFormativo()
        {
            try
            {
                RN.Pedagogico.ItinerarioFormativo rnItinerarioFormativo = new Techne.Lyceum.RN.Pedagogico.ItinerarioFormativo();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlItinerario.Items.Clear();
                ddlItinerario.DataSource = rnItinerarioFormativo.ListaItinerarioFormativoAtivo();
                ddlItinerario.DataTextField = "DESCRICAO";
                ddlItinerario.DataValueField = "ITINERARIOFORMATIVOID";
                ddlItinerario.DataBind();
                ddlItinerario.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTrilha(int itinerario)
        {
            try
            {
                RN.Pedagogico.TrilhaAprendizagem rnTrilhaAprendizagem = new Techne.Lyceum.RN.Pedagogico.TrilhaAprendizagem();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlTrilha.Items.Clear();
                ddlTrilha.DataSource = rnTrilhaAprendizagem.ListaTrilhaAprendizagemAtivoPor(itinerario);
                ddlTrilha.DataTextField = "DESCRICAO";
                ddlTrilha.DataValueField = "TRILHAAPRENDIZAGEMID";
                ddlTrilha.DataBind();
                ddlTrilha.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblEstruturaCurricular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlComposicaoItinerarioFormativo.Visible = false;
                pnlUnidadeCurricular.Visible = false;
                pnlAreaItinerarioFormativo.Visible = false;
                pnlTipoCursoItinerario.Visible = false;
                chkUnidadeCurricular.ClearSelection();
                chkAreaItinerarioFormativo.ClearSelection();
                chkComposicaoItinerario.ClearSelection();
                rblTipoCursoItinerario.ClearSelection();
                ddlItinerario.Items.Clear();
                ddlTrilha.ClearSelection();
                txtMaxComponente.Text = string.Empty;
                pnlTrilha.Visible = false;

                if (rblEstruturaCurricular.SelectedValue == "Itinerario")
                {
                    CarregaItinerarioFormativo();
                    pnlUnidadeCurricular.Visible = true;
                    pnlAreaItinerarioFormativo.Visible = true;
                    pnlTrilha.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void chkAreaItinerarioFormativo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                foreach (ListItem item in chkAreaItinerarioFormativo.Items)
                {
                    if (item.Value == "6")//Itinerário formativo integrado
                    {
                        if (item.Selected)
                        {
                            if (!pnlComposicaoItinerarioFormativo.Visible)
                            {
                                CarregaComposicaoItinerarioFormativo(Convert.ToInt32(item.Value));
                                pnlComposicaoItinerarioFormativo.Visible = true;
                            }
                        }
                        else
                        {
                            chkComposicaoItinerario.ClearSelection();
                            pnlComposicaoItinerarioFormativo.Visible = false;
                        }
                    }

                    if (item.Value == "5")//Formação técnica e profissional
                    {
                        if (item.Selected)
                        {
                            if (!pnlTipoCursoItinerario.Visible)
                            {
                                CarregaTipoCursoItinerarioFormativo(Convert.ToInt32(item.Value));
                                pnlTipoCursoItinerario.Visible = true;
                            }
                        }
                        else
                        {
                            rblTipoCursoItinerario.ClearSelection();
                            pnlTipoCursoItinerario.Visible = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void ddlItinerario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlItinerario.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaTrilha(Convert.ToInt32(ddlItinerario.SelectedValue));
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
    }
}
