using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Data;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/AssociacaoRecursoEducacaoEspecial.aspx")]
    [ControlText("Associação Recurso Educação Especial")]
    [Title("Associação Recurso Educação Especial")]

    public partial class AssociacaoRecursoEducacaoEspecial : TPage
    {
        public object ListaAssociacaoCuidador(object recursoId)
        {
            RN.NecessidadeEspecial.CuidadorAluno rnCuidadorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.CuidadorAluno();

            if (!string.IsNullOrEmpty(recursoId.ToString()))
            {
                return rnCuidadorAluno.ListaPorCpf(recursoId.ToString());
            }
            return null;
        }

        public object ListaAssociacaoLedor(object recursoId)
        {
            RN.NecessidadeEspecial.LedorAluno rnLedorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.LedorAluno();

            if (!string.IsNullOrEmpty(recursoId.ToString()))
            {
                return rnLedorAluno.ListaPorCpf(recursoId.ToString());
            }
            return null;
        }

        public object ListaAssociacaoInterprete(object recursoId)
        {
            RN.NecessidadeEspecial.InterpreteTurma rnInterpreteTurmaAluno = new Techne.Lyceum.RN.NecessidadeEspecial.InterpreteTurma();

            if (!string.IsNullOrEmpty(recursoId.ToString()))
            {
                return rnInterpreteTurmaAluno.ListaPorCpf(recursoId.ToString());
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!Page.IsPostBack)
                {
                    LimparTela();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAssociacaoCuidador, "Associação");
            TituloGrid(grdAssociacaoLedor, "Associação");
            TituloGrid(grdAssociacaoInterprete, "Associação");
        }

        private void LimparTela()
        {
            tseAlunoCuidador.ResetValue();
            dtInicioCuidador.Text = string.Empty;
            dtFimCuidador.Text = string.Empty;
            tseUnidadeEnsinoLedor.ResetValue();
            ddlAno.ClearSelection();
            ddlSemestre.Items.Clear();
            tseCurso.ResetValue();
            ddlSerie.Items.Clear();
            ddlTurma.Items.Clear();
            ddlTurno.Items.Clear();
            tseAlunoLedor.ResetValue();
            dtInicioLedor.Text = string.Empty;
            dtFimLedor.Text = string.Empty;
            tseUnidadeEnsinoInterprete.ResetValue();
            ddlAnoInterprete.ClearSelection();
            ddlSemestreInterprete.Items.Clear();
            tseCursoInterprete.ResetValue();
            ddlTurnoInterprete.Items.Clear();
            ddlSerieInterprete.Items.Clear();
            ddlTurmaInterprete.Items.Clear();
            dtInicioInterprete.Text = string.Empty;
            dtFimInterprete.Text = string.Empty;
        }


        protected void tseRecurso_Changed(object sender, EventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            try
            {
                RN.DTOs.DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial = new Techne.Lyceum.RN.DTOs.DadosRecursoNecessidadeEspecial();
                RN.NecessidadeEspecial.RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.RecursoNecessidadeEspecial();

                LimparTela();
                pcAssociacao.Visible = false;
                if (!this.tseRecurso.DBValue.IsNull)
                {
                    if (this.tseRecurso.IsValidDBValue)
                    {
                        dadosRecursoNecessidadeEspecial = rnRecursoNecessidadeEspecial.ObtemDadosRecursoNecessidadeEspecialPor(Convert.ToInt32(tseRecurso["codigo"].ToString()));
                        pcAssociacao.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Recurso não cadastrado.";

                    }
                }
                else
                {
                    lblMensagem.Text = "Recurso não cadastrado.";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAlunoCuidador_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                if (!tseAlunoCuidador.DBValue.IsNull)
                {
                    if (tseAlunoCuidador.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {

                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAssociacaoCuidador_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAssociacaoCuidador);
        }

        protected void grdAssociacaoCuidador_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAssociacaoCuidador.Settings.ShowFilterRow = false;
        }

        protected void grdAssociacaoCuidador_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string Id = e.Keys["CUIDADORALUNOID"].ToString();

        }

        public void Update(object ALUNOID, object NOME_COMPL, object TURNO, object DATAINICIO, object DATAFIM, object CUIDADORALUNOID)
        {
        }

        protected void odsAssociacaoCuidador_Updating(object sender, System.Web.UI.WebControls.ObjectDataSourceMethodEventArgs e)
        {
            RN.NecessidadeEspecial.CuidadorAluno rnCuidadorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.CuidadorAluno();
            RN.NecessidadeEspecial.Entidades.CuidadorAluno cuidador = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.CuidadorAluno();
            ValidacaoDados validacao = new ValidacaoDados();

            cuidador.AlunoId = e.InputParameters["ALUNOID"].ToString();
            cuidador.CuidadorAlunoId = Convert.ToInt32(e.InputParameters["CUIDADORALUNOID"]);
            cuidador.RecursoNecessidadeEspecialId = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToInt32(tseRecurso["codigo"]) : -1;
            cuidador.DataInicio = e.InputParameters["DATAINICIO"] != null ? Convert.ToDateTime(e.InputParameters["DATAINICIO"].ToString()) : DateTime.MinValue;
            cuidador.DataFim = e.InputParameters["DATAFIM"] != null ? Convert.ToDateTime(e.InputParameters["DATAFIM"].ToString()) : DateTime.MinValue;
            cuidador.UsuarioId = User.Identity.Name;

            string cpf = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToString(tseRecurso.DBValue) : null;

            validacao = rnCuidadorAluno.Valida(cuidador, false, cpf);

            if (validacao.Valido)
            {
                rnCuidadorAluno.Atualiza(cuidador);

                e.Cancel = true;
                this.grdAssociacaoCuidador.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void btnAssociarCuidador_Click(object sender, EventArgs e)
        {
            try
            {
                RN.NecessidadeEspecial.CuidadorAluno rnCuidadorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.CuidadorAluno();
                RN.NecessidadeEspecial.Entidades.CuidadorAluno cuidador = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.CuidadorAluno();
                ValidacaoDados validacao = new ValidacaoDados();

                cuidador.AlunoId = !tseAlunoCuidador.DBValue.IsNull && tseAlunoCuidador.IsValidDBValue ? tseAlunoCuidador.DBValue.ToString() : null;
                cuidador.CuidadorAlunoId = 0;
                cuidador.RecursoNecessidadeEspecialId = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToInt32(tseRecurso["codigo"]) : -1;
                cuidador.DataInicio = !string.IsNullOrEmpty(dtInicioCuidador.Text.Trim()) ? dtInicioCuidador.Date : DateTime.MinValue;
                cuidador.DataFim = !string.IsNullOrEmpty(dtFimCuidador.Text.Trim()) ? dtFimCuidador.Date : DateTime.MinValue;
                cuidador.UsuarioId = User.Identity.Name;

                string cpf = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToString(tseRecurso.DBValue) : null;

                validacao = rnCuidadorAluno.Valida(cuidador, true, cpf);
                if (validacao.Valido)
                {
                    rnCuidadorAluno.Insere(cuidador);
                    grdAssociacaoCuidador.DataBind();

                    lblMensagem.Text = "Associação ao Cuidador incluída com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAlunoLedor_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                dtInicioLedor.Text = string.Empty;
                dtFimLedor.Text = string.Empty;

                if (!tseAlunoLedor.DBValue.IsNull)
                {
                    if (!tseAlunoLedor.IsValidDBValue)
                    {

                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsinoLedor_Changed(object sender, EventArgs args)
        {
            try
            {
                ddlAno.ClearSelection();
                ddlSemestre.ClearSelection();
                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                ddlTurma.Items.Clear();
                tseAlunoLedor.ResetValue();
                dtInicioLedor.Text = string.Empty;
                dtFimLedor.Text = string.Empty;

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeEnsinoLedor.DBValue.IsNull)
                {
                    if (this.tseUnidadeEnsinoLedor.IsValidDBValue)
                    {

                        sessao.Escola = Convert.ToString(this.tseUnidadeEnsinoLedor.DBValue);
                        CarregaAno(ddlAno);
                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }

                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
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

                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void grdAssociacaoLedor_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAssociacaoLedor);
        }

        protected void grdAssociacaoLedor_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAssociacaoLedor.Settings.ShowFilterRow = false;
        }

        protected void grdAssociacaoLedor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string Id = e.Keys["LEDORALUNOID"].ToString();

        }

        public void UpdateLedor(object ALUNOID, object NOME_COMPL, object TURNO, object DATAINICIO, object DATAFIM, object LEDORALUNOID)
        {
        }

        protected void odsAssociacaoLedor_Updating(object sender, System.Web.UI.WebControls.ObjectDataSourceMethodEventArgs e)
        {
            RN.NecessidadeEspecial.LedorAluno rnLedorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.LedorAluno();
            RN.NecessidadeEspecial.Entidades.LedorAluno Ledor = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.LedorAluno();
            ValidacaoDados validacao = new ValidacaoDados();

            Ledor.AlunoId = e.InputParameters["ALUNOID"].ToString();
            Ledor.LedorAlunoId = Convert.ToInt32(e.InputParameters["LEDORALUNOID"]);
            Ledor.RecursoNecessidadeEspecialId = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToInt32(tseRecurso["codigo"]) : -1;
            Ledor.DataInicio = e.InputParameters["DATAINICIO"] != null ? Convert.ToDateTime(e.InputParameters["DATAINICIO"].ToString()) : DateTime.MinValue;
            Ledor.DataFim = e.InputParameters["DATAFIM"] != null ? Convert.ToDateTime(e.InputParameters["DATAFIM"].ToString()) : DateTime.MinValue;
            Ledor.Ano = Convert.ToInt32(e.InputParameters["ANO"]);
            Ledor.Semestre = Convert.ToInt32(e.InputParameters["SEMESTRE"]);
            Ledor.Turma = e.InputParameters["TURMA"].ToString();
            Ledor.UsuarioId = User.Identity.Name;

            string cpf = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToString(tseRecurso.DBValue) : null;

            validacao = rnLedorAluno.Valida(Ledor, false, (!tseUnidadeEnsinoLedor.DBValue.IsNull && tseUnidadeEnsinoLedor.IsValidDBValue) ? tseUnidadeEnsinoLedor.DBValue.ToString() : null, (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) ? tseCurso.DBValue.ToString() : null, !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null, !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSerie.SelectedValue) : -1, cpf);

            if (validacao.Valido)
            {
                rnLedorAluno.Atualiza(Ledor);

                e.Cancel = true;
                this.grdAssociacaoLedor.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void btnAssociarLedor_Click(object sender, EventArgs e)
        {
            try
            {
                RN.NecessidadeEspecial.LedorAluno rnLedorAluno = new Techne.Lyceum.RN.NecessidadeEspecial.LedorAluno();
                RN.NecessidadeEspecial.Entidades.LedorAluno Ledor = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.LedorAluno();
                ValidacaoDados validacao = new ValidacaoDados();

                Ledor.AlunoId = !tseAlunoLedor.DBValue.IsNull && tseAlunoLedor.IsValidDBValue ? tseAlunoLedor.DBValue.ToString() : null;
                Ledor.LedorAlunoId = 0;
                Ledor.RecursoNecessidadeEspecialId = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToInt32(tseRecurso["codigo"]) : -1;
                Ledor.DataInicio = !string.IsNullOrEmpty(dtInicioLedor.Text.Trim()) ? dtInicioLedor.Date : DateTime.MinValue;
                Ledor.DataFim = !string.IsNullOrEmpty(dtFimLedor.Text.Trim()) ? dtFimLedor.Date : DateTime.MinValue;
                Ledor.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                Ledor.Semestre = !ddlSemestre.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSemestre.SelectedValue) : -1;
                Ledor.Turma = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurma.SelectedValue : null;
                Ledor.UsuarioId = User.Identity.Name;

                string cpf = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToString(tseRecurso.DBValue) : null;

                validacao = rnLedorAluno.Valida(Ledor, true, (!tseUnidadeEnsinoLedor.DBValue.IsNull && tseUnidadeEnsinoLedor.IsValidDBValue) ? tseUnidadeEnsinoLedor.DBValue.ToString() : null, (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) ? tseCurso.DBValue.ToString() : null, !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null, !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSerie.SelectedValue) : -1, cpf);
                if (validacao.Valido)
                {
                    rnLedorAluno.Insere(Ledor);
                    grdAssociacaoLedor.DataBind();

                    lblMensagem.Text = "Associação ao Ledor incluída com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAno(DropDownList controleAno)
        {
            controleAno.Items.Clear();
            controleAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            controleAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
            controleAno.DataBind();
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSemestre.Items.Clear();
                ddlSerie.Items.Clear();
                ddlTurno.Items.Clear();
                tseCurso.ResetValue();
                ddlTurma.Items.Clear();
                tseAlunoLedor.ResetValue();

                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    this.ddlSemestre.DataSource = RN.PeriodoLetivo.ListarPeriodo(this.ddlAno.SelectedValue);
                    this.ddlSemestre.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    this.ddlSemestre.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSemestre_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerie.Items.Clear();
                ddlTurno.Items.Clear();
                tseCurso.ResetValue();
                ddlTurma.Items.Clear();
                tseAlunoLedor.ResetValue();

                if (!ddlSemestre.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !tseUnidadeEnsinoLedor.DBValue.IsNull && tseUnidadeEnsinoLedor.IsValidDBValue)
                {
                    this.tseCurso.SqlWhere = " t.faculdade = '" + Convert.ToString(tseUnidadeEnsinoLedor.DBValue) + "' and t.ano = " + int.Parse(this.ddlAno.SelectedValue) + " and t.semestre = " + int.Parse(this.ddlSemestre.SelectedValue);

                }
                else
                {
                    lblMensagem.Text = "Campo Periodo é de preenchimento obrigatório.";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCurso_Changed(object sender, EventArgs args)
        {
            try
            {
                this.ddlSerie.Items.Clear();
                this.ddlTurno.Items.Clear();
                this.ddlTurma.Items.Clear();
                tseAlunoLedor.ResetValue();

                if (ddlSemestre.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() || this.tseUnidadeEnsinoLedor.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor selecionar o Ano/Período/Unidade de Ensino.";
                    return;
                }

                if (!this.tseCurso.DBValue.IsNull && !this.tseUnidadeEnsinoLedor.DBValue.IsNull)
                {
                    if (this.tseCurso.IsValidDBValue)
                    {
                        this.ddlTurno.DataSource = RN.Turno.ListarTurnosPorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoLedor.DBValue), this.tseCurso.DBValue.ToString(), int.Parse(this.ddlAno.SelectedValue), int.Parse(this.ddlSemestre.SelectedValue));
                        this.ddlTurno.Items.Insert(0, new ListItem("Selecione", string.Empty));
                        this.ddlTurno.DataBind();
                    }
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
                this.ddlTurma.Items.Clear();

                if (!ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    this.ddlTurma.DataSource = Turma.ListarPorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoLedor.DBValue), this.tseCurso.DBValue.ToString(), int.Parse(this.ddlSerie.SelectedValue), int.Parse(this.ddlAno.SelectedValue), int.Parse(this.ddlSemestre.SelectedValue), this.ddlTurno.SelectedValue);
                    this.ddlTurma.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    this.ddlTurma.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ddlSerie.Items.Clear();
                this.ddlTurma.Items.Clear();
                tseAlunoLedor.ResetValue();
                dtInicioLedor.Text = string.Empty;
                dtFimLedor.Text = string.Empty;

                if (this.ddlTurno.SelectedValue != "Selecione")
                {
                    this.ddlSerie.DataSource = Serie.ListarSeriePorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoLedor.DBValue), this.tseCurso.DBValue.ToString(), int.Parse(this.ddlAno.SelectedValue), int.Parse(this.ddlSemestre.SelectedValue), this.ddlTurno.SelectedValue);
                    this.ddlSerie.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    this.ddlSerie.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseAlunoLedor.ResetValue();
                dtInicioLedor.Text = string.Empty;
                dtFimLedor.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        protected void ddlAnoInterprete_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSemestreInterprete.Items.Clear();
                ddlSerieInterprete.Items.Clear();
                ddlTurnoInterprete.Items.Clear();
                tseCursoInterprete.ResetValue();
                ddlTurmaInterprete.Items.Clear();


                if (!ddlAnoInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    this.ddlSemestreInterprete.DataSource = RN.PeriodoLetivo.ListarPeriodo(this.ddlAnoInterprete.SelectedValue);
                    this.ddlSemestreInterprete.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    this.ddlSemestreInterprete.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSemestreInterprete_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerieInterprete.Items.Clear();
                ddlTurnoInterprete.Items.Clear();
                tseCursoInterprete.ResetValue();
                ddlTurmaInterprete.Items.Clear();


                if (!ddlSemestreInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlAnoInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !tseUnidadeEnsinoInterprete.DBValue.IsNull && tseUnidadeEnsinoInterprete.IsValidDBValue)
                {
                    this.tseCursoInterprete.SqlWhere = " t.faculdade = '" + Convert.ToString(tseUnidadeEnsinoInterprete.DBValue) + "' and t.ano = " + int.Parse(this.ddlAnoInterprete.SelectedValue) + " and t.semestre = " + int.Parse(this.ddlSemestreInterprete.SelectedValue);

                }
                else
                {
                    lblMensagem.Text = "Campo Ano/Periodo/Unidade de Ensino é de preenchimento obrigatório.";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCursoInterprete_Changed(object sender, EventArgs args)
        {
            try
            {
                this.ddlSerieInterprete.Items.Clear();
                this.ddlTurnoInterprete.Items.Clear();
                this.ddlTurmaInterprete.Items.Clear();

                if (ddlSemestreInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlAnoInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() || this.tseUnidadeEnsinoInterprete.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor selecionar o Ano/Período/Unidade de Ensino.";
                    return;
                }

                if (!this.tseCursoInterprete.DBValue.IsNull && !this.tseUnidadeEnsinoInterprete.DBValue.IsNull)
                {
                    if (this.tseCursoInterprete.IsValidDBValue)
                    {
                        this.ddlTurnoInterprete.DataSource = RN.Turno.ListarTurnosPorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoInterprete.DBValue), this.tseCursoInterprete.DBValue.ToString(), int.Parse(this.ddlAnoInterprete.SelectedValue), int.Parse(this.ddlSemestreInterprete.SelectedValue));
                        this.ddlTurnoInterprete.Items.Insert(0, new ListItem("Selecione", string.Empty));
                        this.ddlTurnoInterprete.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSerieInterprete_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ddlTurmaInterprete.Items.Clear();

                if (!ddlSerieInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    this.ddlTurmaInterprete.DataSource = Turma.ListarPorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoInterprete.DBValue), this.tseCursoInterprete.DBValue.ToString(), int.Parse(this.ddlSerieInterprete.SelectedValue), int.Parse(this.ddlAnoInterprete.SelectedValue), int.Parse(this.ddlSemestreInterprete.SelectedValue), this.ddlTurnoInterprete.SelectedValue);
                    this.ddlTurmaInterprete.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    this.ddlTurmaInterprete.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurnoInterprete_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ddlSerieInterprete.Items.Clear();
                this.ddlTurmaInterprete.Items.Clear();
                dtInicioInterprete.Text = string.Empty;
                dtFimInterprete.Text = string.Empty;

                if (this.ddlTurnoInterprete.SelectedValue != "Selecione")
                {
                    this.ddlSerieInterprete.DataSource = Serie.ListarSeriePorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoInterprete.DBValue), this.tseCursoInterprete.DBValue.ToString(), int.Parse(this.ddlAnoInterprete.SelectedValue), int.Parse(this.ddlSemestreInterprete.SelectedValue), this.ddlTurnoInterprete.SelectedValue);
                    this.ddlSerieInterprete.Items.Insert(0, new ListItem("Selecione", string.Empty));
                    this.ddlSerieInterprete.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurmaInterprete_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dtInicioInterprete.Text = string.Empty;
                dtFimInterprete.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsinoInterprete_Changed(object sender, EventArgs args)
        {
            try
            {
                ddlAnoInterprete.ClearSelection();
                ddlSemestreInterprete.ClearSelection();
                tseCursoInterprete.ResetValue();
                ddlTurnoInterprete.Items.Clear();
                ddlSerieInterprete.Items.Clear();
                ddlTurmaInterprete.Items.Clear();
                dtInicioInterprete.Text = string.Empty;
                dtFimInterprete.Text = string.Empty;

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeEnsinoInterprete.DBValue.IsNull)
                {
                    if (this.tseUnidadeEnsinoInterprete.IsValidDBValue)
                    {

                        sessao.Escola = Convert.ToString(this.tseUnidadeEnsinoInterprete.DBValue);
                        CarregaAno(ddlAnoInterprete);
                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }

                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
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

                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void grdAssociacaoInterprete_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAssociacaoInterprete);
        }

        protected void grdAssociacaoInterprete_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAssociacaoInterprete.Settings.ShowFilterRow = false;
        }

        protected void grdAssociacaoInterprete_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string Id = e.Keys["INTERPRETETURMAID"].ToString();

        }

        public void UpdateInterprete(object INTERPRETETURMAID, object TURNO, object DATAINICIO, object DATAFIM, object TURMA)
        {
        }

        protected void odsAssociacaoInterprete_Updating(object sender, System.Web.UI.WebControls.ObjectDataSourceMethodEventArgs e)
        {
            RN.NecessidadeEspecial.InterpreteTurma rnInterpreteTurma = new Techne.Lyceum.RN.NecessidadeEspecial.InterpreteTurma();
            RN.NecessidadeEspecial.Entidades.InterpreteTurma Interprete = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.InterpreteTurma();
            ValidacaoDados validacao = new ValidacaoDados();

            Interprete.InterpreteTurmaId = Convert.ToInt32(e.InputParameters["INTERPRETETURMAID"]);
            Interprete.RecursoNecessidadeEspecialId = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToInt32(tseRecurso["codigo"]) : -1;
            Interprete.DataInicio = e.InputParameters["DATAINICIO"] != null ? Convert.ToDateTime(e.InputParameters["DATAINICIO"].ToString()) : DateTime.MinValue;
            Interprete.DataFim = e.InputParameters["DATAFIM"] != null ? Convert.ToDateTime(e.InputParameters["DATAFIM"].ToString()) : DateTime.MinValue;
            Interprete.Ano = Convert.ToInt32(e.InputParameters["ANO"]);
            Interprete.Semestre = Convert.ToInt32(e.InputParameters["SEMESTRE"]);
            Interprete.Turma = e.InputParameters["TURMA"].ToString();
            Interprete.UsuarioId = User.Identity.Name;

            string cpf = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToString(tseRecurso.DBValue) : null;

            validacao = rnInterpreteTurma.Valida(Interprete, false, (!tseUnidadeEnsinoInterprete.DBValue.IsNull && tseUnidadeEnsinoInterprete.IsValidDBValue) ? tseUnidadeEnsinoInterprete.DBValue.ToString() : null, (!tseCursoInterprete.DBValue.IsNull && tseCursoInterprete.IsValidDBValue) ? tseCursoInterprete.DBValue.ToString() : null, !ddlTurnoInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurnoInterprete.SelectedValue : null, !ddlSerieInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSerieInterprete.SelectedValue) : -1, cpf);

            if (validacao.Valido)
            {
                rnInterpreteTurma.Atualiza(Interprete);

                e.Cancel = true;
                this.grdAssociacaoInterprete.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void btnAssociarInterprete_Click(object sender, EventArgs e)
        {
            try
            {
                RN.NecessidadeEspecial.InterpreteTurma rnInterpreteTurma = new Techne.Lyceum.RN.NecessidadeEspecial.InterpreteTurma();
                RN.NecessidadeEspecial.Entidades.InterpreteTurma Interprete = new Techne.Lyceum.RN.NecessidadeEspecial.Entidades.InterpreteTurma();
                ValidacaoDados validacao = new ValidacaoDados();

                Interprete.InterpreteTurmaId = 0;
                Interprete.RecursoNecessidadeEspecialId = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToInt32(tseRecurso["codigo"]) : -1;
                Interprete.DataInicio = !string.IsNullOrEmpty(dtInicioInterprete.Text.Trim()) ? dtInicioInterprete.Date : DateTime.MinValue;
                Interprete.DataFim = !string.IsNullOrEmpty(dtFimInterprete.Text.Trim()) ? dtFimInterprete.Date : DateTime.MinValue;
                Interprete.Ano = !ddlAnoInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAnoInterprete.SelectedValue) : -1;
                Interprete.Semestre = !ddlSemestreInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSemestreInterprete.SelectedValue) : -1;
                Interprete.Turma = !ddlTurmaInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurmaInterprete.SelectedValue : null;
                Interprete.UsuarioId = User.Identity.Name;

                string cpf = !tseRecurso.DBValue.IsNull && tseRecurso.IsValidDBValue ? Convert.ToString(tseRecurso.DBValue) : null;

                validacao = rnInterpreteTurma.Valida(Interprete, true, (!tseUnidadeEnsinoInterprete.DBValue.IsNull && tseUnidadeEnsinoInterprete.IsValidDBValue) ? tseUnidadeEnsinoInterprete.DBValue.ToString() : null, (!tseCursoInterprete.DBValue.IsNull && tseCursoInterprete.IsValidDBValue) ? tseCursoInterprete.DBValue.ToString() : null, !ddlTurnoInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurnoInterprete.SelectedValue : null, !ddlSerieInterprete.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSerieInterprete.SelectedValue) : -1, cpf);
                if (validacao.Valido)
                {
                    rnInterpreteTurma.Insere(Interprete);
                    grdAssociacaoInterprete.DataBind();

                    lblMensagem.Text = "Associação ao Interprete incluída com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
