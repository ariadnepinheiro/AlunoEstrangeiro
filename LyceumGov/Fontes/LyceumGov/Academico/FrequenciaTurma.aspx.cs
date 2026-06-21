namespace Techne.Lyceum.Net.Academico
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DevExpress.Web.ASPxGridView;
    using DevExpress.Web.ASPxTabControl;
    using Seeduc.Infra.Helpers;
    using Techne.Lyceum.Net.Modulos;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Web;
    using System.Web.UI.HtmlControls;
    using Seeduc.Infra.Data;
    using Techne.Library;
    using DevExpress.Data;
    using Techne.Lyceum.RN.Util;
    using System.Linq;


    [NavUrl("~/Academico/FrequenciaTurma.aspx"), ControlText("FrequenciaTurma"), Title("Frequência Diária")]
    public partial class FrequenciaTurma : TPage
    {
        private Turma.DadosTurma ObjetoTurma
        {
            get
            {
                return (Turma.DadosTurma)this.ViewState["ObjetoTurma"];
            }

            set
            {
                this.ViewState["ObjetoTurma"] = value;
            }
        }

        public object Listar(object disciplina, object turma, object ano, object periodo, object dataFrequencia)
        {
            RN.Turmas.FrequenciaDiaria rnFrequenciaDiaria = new Techne.Lyceum.RN.Turmas.FrequenciaDiaria();

            if (disciplina != null && turma != null && ano != null && periodo != null && dataFrequencia != null)
            {

                return rnFrequenciaDiaria.ConsultarMatriculas(disciplina.ToString(), turma.ToString(), Convert.ToDecimal(ano), Convert.ToDecimal(periodo), Convert.ToDateTime(dataFrequencia));
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdMatriculas, "Lançamento de Frequências");


            var mp = (LyceumMaster)this.Master;

            if (mp != null)
            {
                mp.habilitaLoading = true;
            }

            this.Page.MaintainScrollPositionOnPostBack = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                
                if (!this.Page.IsPostBack)
                {
                    lblMensagemFinalizacao.Text = string.Empty;
                    lblMensagemReposicao.Text = string.Empty;

                    this.btnSalvar.Visible = this.btnCancelar.Visible = btnBuscar.Visible= false;
                    btnVoltar.Visible = true;
                    btnImprimirComp.Visible = false;
                    grdMatriculas.Visible = false;
                    pnlGridMatriculas.Visible = false;
                    pnlCurriculo.Visible = false;
                    rpCurriculoBasico.DataSource = null;
                    rpCurriculoEssencial.DataSource = null;
                    rpCurriculoRecomposicao.DataSource = null;
                    pnlPlanoAula.Visible = false;
                    txtPlanoAula.Text = string.Empty;
                    lblProfessorAlocado.Text = string.Empty;

                    ddlDataFrequencia.Items.Clear();
                    pnlDadosFrequencia.Visible = false;
                    pnlBasico.Visible = false;
                    pnlEssencial.Visible = false;
                    pnlRecomposicao.Visible = false;
                    lblProfessor.Visible = false;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                        ObterQueryString(decodedText);

                        if (!tbAno.Text.IsNullOrEmptyOrWhiteSpace() && !tbPeriodo.Text.IsNullOrEmptyOrWhiteSpace() && !tbTurma.Text.IsNullOrEmptyOrWhiteSpace())
                        {
                            CarregaDisciplina(Convert.ToInt32(tbAno.Text), Convert.ToInt32(this.tbPeriodo.Text), this.tbTurma.Text);
                            pnlDadosFrequencia.Visible = true;
                        }
                    }
                    else
                    {

                        this.Response.Redirect("~/Academico/ListarFrequenciaTurma.aspx");
                    }

                }

                if (!tbAno.Text.IsNullOrEmptyOrWhiteSpace() && !tbPeriodo.Text.IsNullOrEmptyOrWhiteSpace() && !tbTurma.Text.IsNullOrEmptyOrWhiteSpace() && !ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlDataFrequencia.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    this.CarregarDadosGrid(Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbPeriodo.Text), tbTurma.Text, ddlDisciplina.SelectedValue, ((int)(Convert.ToDateTime(ddlDataFrequencia.SelectedValue)).DayOfWeek + 1), Convert.ToDateTime(ddlDataFrequencia.SelectedValue));
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }


        private void ObterQueryString(string queryString)
        {
            LimparCampo();
            ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();
            lblMensagem.Text = string.Empty;
            string[] listaDados = queryString.Split('&');
            RN.Turno rnTurno = new Turno();

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("ano") >= 0)
                    this.tbAno.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("semestre") >= 0)
                    this.tbPeriodo.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("regional") >= 0)
                    tbRegional.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("municipio") >= 0)
                    tbMunicipio.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("turma") >= 0)
                    this.tbTurma.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("censo") >= 0)
                {
                    tbCenso.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    ObjetoTurma.Faculdade = tbCenso.Text;
                }
                else if (dados.IndexOf("unidade") >= 0)
                {
                    this.tbUnidadeEnsino.Text = dados.Substring(dados.LastIndexOf('=') + 1);

                }
                else if (dados.IndexOf("curriculo") >= 0)
                    this.tbMatrizCurricular.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("turno") >= 0)
                {
                    tbTurno.Text = rnTurno.RetornaDescricaoTurno(dados.Substring(dados.LastIndexOf('=') + 1));
                    hdnTurno.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("curso") >= 0)
                    hdnCurso.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("escolaridade") >= 0)
                    tbEscolaridade.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("serie") >= 0)
                    tbSerie.Text = dados.Substring(dados.LastIndexOf('=') + 1);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected bool VerificarCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "S";
            }

            return false;
        }

        private void ManterCurriculo()
        {
            try
            {
                RN.CompetenciaHabilidadeDocente rnCompetenciaHabilidadeDocente = new CompetenciaHabilidadeDocente();
                List<int> listaItens = new List<int>();
                bool possuiBasico = false;
                bool possuiEssencial = false;
                bool possuiRecomposicao = false;


                listaItens = rnCompetenciaHabilidadeDocente.ObtemItensPor(tbTurma.Text, ddlDisciplina.SelectedValue, Convert.ToDateTime(ddlDataFrequencia.SelectedValue));

                foreach (RepeaterItem item in rpCurriculoBasico.Items)
                {
                    HiddenField hdnGrupoIdBasico = (HiddenField)item.FindControl("hdnGrupoIdBasico");

                    CheckBoxList chk = (CheckBoxList)item.FindControl("chkCompetenciaItemBasico");

                    foreach (var linha in listaItens)
                    {
                        if (chk.Items.FindByValue(linha.ToString()) != null)
                        {
                            chk.Items.FindByValue(linha.ToString()).Selected = true;
                            possuiBasico = true;
                        }
                    }
                }

                foreach (RepeaterItem item in rpCurriculoEssencial.Items)
                {
                    CheckBoxList chk = (CheckBoxList)item.FindControl("chkCompetenciaItemEssencial");

                    foreach (var linha in listaItens)
                    {
                        if (chk.Items.FindByValue(linha.ToString()) != null)
                        {
                            chk.Items.FindByValue(linha.ToString()).Selected = true;
                            possuiEssencial = true;
                        }
                    }
                }

                foreach (RepeaterItem item in rpCurriculoRecomposicao.Items)
                {
                    CheckBoxList chk = (CheckBoxList)item.FindControl("chkCompetenciaItemRecomposicao");

                    foreach (var linha in listaItens)
                    {
                        if (chk.Items.FindByValue(linha.ToString()) != null)
                        {
                            chk.Items.FindByValue(linha.ToString()).Selected = true;
                            possuiRecomposicao = true;
                        }
                    }
                }

                hdnPossuiBasico.Value = possuiBasico ? "S" : string.Empty;
                hdnPossuiEssencial.Value = possuiEssencial ? "S" : string.Empty;
                hdnPossuiRecomposicao.Value = possuiRecomposicao ? "S" : string.Empty;


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.CompetenciaHabilidadeGrupo rnCompetenciaHabilidadeGrupo = new CompetenciaHabilidadeGrupo();
                List<RN.DTOs.DadosCompetenciaCurriculo> lsGrupoBasico = new List<Techne.Lyceum.RN.DTOs.DadosCompetenciaCurriculo>();
                List<RN.DTOs.DadosCompetenciaCurriculo> lsGrupoEssencial = new List<Techne.Lyceum.RN.DTOs.DadosCompetenciaCurriculo>();
                List<RN.DTOs.DadosCompetenciaCurriculo> lsGrupoRecomposicao = new List<Techne.Lyceum.RN.DTOs.DadosCompetenciaCurriculo>();
                RN.Turmas.FrequenciaPlanoDeAula rnFrequenciaPlanoDeAula = new Techne.Lyceum.RN.Turmas.FrequenciaPlanoDeAula();
                RN.Turmas.DiaSemAula rnDiaSemAula = new Techne.Lyceum.RN.Turmas.DiaSemAula();
                
                pnlPlanoAula.Visible = false;
                pnlCurriculo.Visible = false;
                pnlGridMatriculas.Visible = false;
                lblMensagemFinalizacao.Text = string.Empty;
                lblMensagemReposicao.Text = string.Empty;

                if (!ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlDataFrequencia.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {

                    pnlCurriculo.Visible = true;
                    pnlGridMatriculas.Visible = true;
                    grdMatriculas.Visible = true;

                    lsGrupoBasico = rnCompetenciaHabilidadeGrupo.ListaDadosCompetenciaCurriculoPor(ddlDisciplina.SelectedValue, Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbSerie.Text), Convert.ToInt32(tbPeriodo.Text), hdnCurso.Value, Convert.ToDateTime(ddlDataFrequencia.SelectedValue), "BÁSICO");

                    lsGrupoEssencial = rnCompetenciaHabilidadeGrupo.ListaDadosCompetenciaCurriculoPor(ddlDisciplina.SelectedValue, Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbSerie.Text), Convert.ToInt32(tbPeriodo.Text), hdnCurso.Value, Convert.ToDateTime(ddlDataFrequencia.SelectedValue), "ESSENCIALIZADO");

                    lsGrupoRecomposicao = rnCompetenciaHabilidadeGrupo.ListaDadosCompetenciaCurriculoPor(ddlDisciplina.SelectedValue, Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbSerie.Text), Convert.ToInt32(tbPeriodo.Text), hdnCurso.Value, Convert.ToDateTime(ddlDataFrequencia.SelectedValue), "RECOMPOSIÇÃO");

                    rpCurriculoBasico.DataSource = lsGrupoBasico;
                    rpCurriculoBasico.DataBind();

                    rpCurriculoEssencial.DataSource = lsGrupoEssencial;
                    rpCurriculoEssencial.DataBind();

                    rpCurriculoRecomposicao.DataSource = lsGrupoRecomposicao;
                    rpCurriculoRecomposicao.DataBind();

                    ManterCurriculo();

                    pnlBasico.Visible = hdnPossuiBasico.Value == "S";
                    pnlEssencial.Visible = hdnPossuiEssencial.Value == "S";
                    pnlRecomposicao.Visible = hdnPossuiRecomposicao.Value == "S";

                    if (hdnPossuiBasico.Value.IsNullOrEmptyOrWhiteSpace() && hdnPossuiEssencial.Value.IsNullOrEmptyOrWhiteSpace() && hdnPossuiRecomposicao.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        pnlBasico.Visible = true;
                    }

                    pnlPlanoAula.Visible = true;
                    
                    bool  aberto;

                    string mensagem = rnDiaSemAula.VerificaDiaSemAulaPor(Convert.ToDateTime(ddlDataFrequencia.SelectedValue),tbCenso.Text,out aberto);
                    
                    this.btnSalvar.Visible = this.btnCancelar.Visible = aberto;

                    lblMensagemReposicao.Text = mensagem;

                    var plano = rnFrequenciaPlanoDeAula.ObtemPor(Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbPeriodo.Text), tbTurma.Text, ddlDisciplina.SelectedValue, Convert.ToDateTime(ddlDataFrequencia.SelectedValue));

                    if (!plano.PlanoAula.IsNullOrEmptyOrWhiteSpace())
                    {
                        txtPlanoAula.Text = plano.PlanoAula;
                        lblMensagemFinalizacao.Text = "Lançamento de Frequência realizado por " + plano.NomeUsuario + " em " + plano.DataAlteracao.ToShortDateString();
                    }

                    CarregarDadosGrid(Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbPeriodo.Text), tbTurma.Text, ddlDisciplina.SelectedValue, ((int)(Convert.ToDateTime(ddlDataFrequencia.SelectedValue)).DayOfWeek + 1), Convert.ToDateTime(ddlDataFrequencia.SelectedValue));
                }
                else
                {
                    lblMensagem.Text = "Para efetuar a busca é necessário selecionar a Disciplina, Data da Frequência e os Horários.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelarVoltar_Click(object sender, ImageClickEventArgs e)
        {
            this.RetirarVisibilidadeGradeNotas();

            var queryString = this.MontarQueryString();
            var bytesToEncode = Encoding.UTF8.GetBytes(queryString);

            this.Server.Transfer("ListarFrequenciaTurma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.DTOs.DadosFrequenciaDiaria dadosFrequencia = new Techne.Lyceum.RN.DTOs.DadosFrequenciaDiaria();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.FrequenciaDiaria rnFrequencia = new Techne.Lyceum.RN.Turmas.FrequenciaDiaria();
                List<int> listaHorarios = new List<int>();
                List<int> listaCurriculo = new List<int>();
                List<string> listaAlunosSuspensos = new List<string>();
                List<Techne.Lyceum.RN.DTOs.DadosFrequenciaDiaria.DadosAlunoFalta> listaDadosAlunoFalta = new List<Techne.Lyceum.RN.DTOs.DadosFrequenciaDiaria.DadosAlunoFalta>();
                RN.DTOs.DadosFrequenciaDiaria.DadosAlunoFalta dadosAlunoFalta = new Techne.Lyceum.RN.DTOs.DadosFrequenciaDiaria.DadosAlunoFalta();
                int semCurriculo = 0;

                foreach (RepeaterItem itemCurriculo in rpCurriculoBasico.Items)
                {
                    CheckBoxList chkCompetenciaItemBasico = (CheckBoxList)itemCurriculo.FindControl("chkCompetenciaItemBasico");

                    foreach (ListItem itemCompetencia in chkCompetenciaItemBasico.Items)
                    {
                        if (itemCompetencia.Selected)
                        {
                            listaCurriculo.Add(Convert.ToInt32(itemCompetencia.Value));
                        }
                    }
                }

                foreach (RepeaterItem itemCurriculo in rpCurriculoEssencial.Items)
                {
                    CheckBoxList chkCompetenciaItemEssencial = (CheckBoxList)itemCurriculo.FindControl("chkCompetenciaItemEssencial");

                    foreach (ListItem itemCompetencia in chkCompetenciaItemEssencial.Items)
                    {
                        if (itemCompetencia.Selected)
                        {
                            listaCurriculo.Add(Convert.ToInt32(itemCompetencia.Value));
                        }
                    }
                }

                foreach (RepeaterItem itemCurriculo in rpCurriculoRecomposicao.Items)
                {
                    CheckBoxList chkCompetenciaItemRecomposicao = (CheckBoxList)itemCurriculo.FindControl("chkCompetenciaItemRecomposicao");

                    foreach (ListItem itemCompetencia in chkCompetenciaItemRecomposicao.Items)
                    {
                        if (itemCompetencia.Selected)
                        {
                            listaCurriculo.Add(Convert.ToInt32(itemCompetencia.Value));
                        }
                    }
                }

                listaHorarios = grdMatriculas.VisibleColumns
                       .Where(a => a.Caption.Contains(":"))
                       .Select(s => Convert.ToInt32(s.Name))
                       .ToList();


                for (var rowIndex = 0; rowIndex < this.grdMatriculas.VisibleRowCount; rowIndex++)
                {
                    var aluno = grdMatriculas.GetRowValues(rowIndex, "Matricula").ToString();
                    var sitMatricula = grdMatriculas.GetRowValues(rowIndex, "Situacao").ToString();
                    var justificativa = grdMatriculas.GetRowValues(rowIndex, "Justificativa").ToString();
                    
                    foreach (int aula in listaHorarios)
                    {
                        CheckBox chk = grdMatriculas.FindRowCellTemplateControl(rowIndex, grdMatriculas.Columns[aula.ToString()] as GridViewDataColumn, aula.ToString()) as CheckBox;

                        if (!chk.Checked && (sitMatricula.Equals("Matriculado") || sitMatricula.Equals("Matrícula em Suspensão")) && justificativa.IsNullOrEmptyOrWhiteSpace())
                        {
                            dadosAlunoFalta = new Techne.Lyceum.RN.DTOs.DadosFrequenciaDiaria.DadosAlunoFalta();

                            dadosAlunoFalta.Aluno = aluno;
                            dadosAlunoFalta.Aula = aula;                           

                            listaDadosAlunoFalta.Add(dadosAlunoFalta);
                        }

                        if (chk.Checked && sitMatricula.Equals("Matrícula em Suspensão"))
                        {
                            listaAlunosSuspensos.Add(aluno);
                        }
                    }
                }

                if (rpCurriculoRecomposicao.Items.Count == 0 && rpCurriculoEssencial.Items.Count == 0 && rpCurriculoBasico.Items.Count == 0)
                {
                    semCurriculo = -1;
                }

                dadosFrequencia.Ano = !tbAno.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(tbAno.Text) : -1;
                dadosFrequencia.Semestre = !tbPeriodo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(tbPeriodo.Text) : -1;
                dadosFrequencia.Aulas = listaHorarios;
                dadosFrequencia.DataFrequencia = !ddlDataFrequencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(ddlDataFrequencia.SelectedValue) : DateTime.MinValue;
                dadosFrequencia.Disciplina = !ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlDisciplina.SelectedValue : null;
                dadosFrequencia.Faculdade = !tbCenso.Text.IsNullOrEmptyOrWhiteSpace() ? tbCenso.Text : null;
                dadosFrequencia.PlanoAula = !txtPlanoAula.Text.IsNullOrEmptyOrWhiteSpace() ? txtPlanoAula.Text.Trim() : null;
                dadosFrequencia.Turma = !tbTurma.Text.IsNullOrEmptyOrWhiteSpace() ? tbTurma.Text.Trim() : null;
                dadosFrequencia.Turno = !hdnTurno.Value.IsNullOrEmptyOrWhiteSpace() ? hdnTurno.Value : null;
                dadosFrequencia.UsuarioResponsavel = User.Identity.Name;
                dadosFrequencia.AlunosComFalta = listaDadosAlunoFalta;
                dadosFrequencia.CurriculosItemId = semCurriculo == -1 ? null : listaCurriculo;
                dadosFrequencia.AlunosSuspensos = listaAlunosSuspensos;

                validacao = rnFrequencia.Valida(dadosFrequencia);

                if (validacao.Valido)
                {
                    rnFrequencia.Salva(dadosFrequencia);

                    lblMensagem.Text = "Lançamento de frequência realizado com sucesso.";
                    lblMensagemFinalizacao.Text = "Lançamento de Frequência realizado por " + RN.Usuarios.BuscaNome(dadosFrequencia.UsuarioResponsavel) + " em " + DateTime.Now.ToShortDateString();
                    CarregarDadosGrid(Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbPeriodo.Text), tbTurma.Text, ddlDisciplina.SelectedValue, ((int)(Convert.ToDateTime(ddlDataFrequencia.SelectedValue)).DayOfWeek + 1), Convert.ToDateTime(ddlDataFrequencia.SelectedValue));
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

        protected void ddlDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagemFinalizacao.Text = string.Empty;
                lblMensagemReposicao.Text = string.Empty;

                RN.AulaDocente rnAulaDocente = new AulaDocente();
                List<string> listaDocentes = new List<string>();
                RN.HorariosDocente rnHorariosDocentes = new HorariosDocente();

                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                btnVoltar.Visible = true;
                btnImprimirComp.Visible = false;
                this.grdMatriculas.Visible = false;
                pnlGridMatriculas.Visible = false;
                pnlCurriculo.Visible = false;
                rpCurriculoBasico.DataSource = null;
                rpCurriculoEssencial.DataSource = null;
                rpCurriculoRecomposicao.DataSource = null;
                pnlPlanoAula.Visible = false;
                txtPlanoAula.Text = string.Empty;
                lblProfessorAlocado.Text = string.Empty;

                ddlMes.ClearSelection();
                ddlDataFrequencia.Items.Clear();
                lblProfessor.Visible = false;

                btnBuscar.Visible = false;

                if (!ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblProfessor.Visible = true;

                    listaDocentes = rnAulaDocente.ObtemDocentesEmAulaPor(Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbPeriodo.Text), tbTurma.Text, ddlDisciplina.SelectedValue);

                    if (listaDocentes.Count > 0)
                    {
                        lblProfessorAlocado.Text = listaDocentes.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
                    }
                    else
                    {
                        lblProfessorAlocado.Text = "Sem Professor alocado";
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagemFinalizacao.Text = string.Empty;
                lblMensagemReposicao.Text = string.Empty;

                pnlGridMatriculas.Visible = false;
                pnlPlanoAula.Visible = false;
                txtPlanoAula.Text = string.Empty;
                pnlBasico.Visible = false;
                pnlEssencial.Visible = false;
                pnlRecomposicao.Visible = false;
                btnBuscar.Visible = false;
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                btnVoltar.Visible = true;
                btnImprimirComp.Visible = false;
                this.grdMatriculas.Visible = false;
                pnlCurriculo.Visible = false;
                rpCurriculoBasico.DataSource = null;
                rpCurriculoEssencial.DataSource = null;
                rpCurriculoRecomposicao.DataSource = null;
                ddlDataFrequencia.Items.Clear();

                if (!ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaDatas(Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbPeriodo.Text), tbTurma.Text, ddlDisciplina.SelectedValue, Convert.ToInt32(ddlMes.SelectedValue));

                    if (ddlDataFrequencia.Items.Count <= 0)
                    {
                        lblMensagem.Text = "Para esta disciplina não tem dia disponível";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlDataFrequencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagemFinalizacao.Text = string.Empty;
                lblMensagemReposicao.Text = string.Empty;

                pnlGridMatriculas.Visible = false;
                pnlPlanoAula.Visible = false;
                txtPlanoAula.Text = string.Empty;
                pnlBasico.Visible = false;
                pnlEssencial.Visible = false;
                pnlRecomposicao.Visible = false;
                btnBuscar.Visible = false;
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                btnVoltar.Visible = true;
                btnImprimirComp.Visible = false;
                this.grdMatriculas.Visible = false;
                pnlCurriculo.Visible = false;
                rpCurriculoBasico.DataSource = null;
                rpCurriculoEssencial.DataSource = null;
                rpCurriculoRecomposicao.DataSource = null;

                if (!ddlDataFrequencia.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    btnBuscar.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        public class TextBoxFreqAcumuladaTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                TextBox tb = new TextBox();
                container.Controls.Add(tb);
            }
        }

        private void CarregarDadosGrid()
        {
            if (this.ddlDisciplina.SelectedValue == "Selecione")
            {
                return;
            }

            var disciplina = this.ddlDisciplina.SelectedValue;

            if (this.grdMatriculas.VisibleRowCount > 0)
            {
                this.grdMatriculas.Visible = true;
                this.btnSalvar.Visible = this.btnCancelar.Visible = true;
                btnImprimirComp.Visible = true;
            }
            else
            {

                this.grdMatriculas.Visible = false;
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                btnImprimirComp.Visible = false;
            }
        }

        private void CarregarDadosIniciais()
        {

            this.LimparCampo();

        }

        private void LimparCampo()
        {
            hdnTurno.Value = string.Empty;
            this.tbUnidadeEnsino.Text = string.Empty;
            this.tbTurma.Text = string.Empty;
            this.tbAno.Text = string.Empty;
            this.tbPeriodo.Text = string.Empty;
            this.tbEscolaridade.Text = string.Empty;
            this.tbMatrizCurricular.Text = string.Empty;
            this.tbSerie.Text = string.Empty;
            this.tbTurno.Text = string.Empty;
            hdnTurno.Value = string.Empty;
            tbRegional.Text = string.Empty;
            tbMunicipio.Text = string.Empty;
            tbCenso.Text = string.Empty;
            lblMensagemFinalizacao.Text = string.Empty;
            lblMensagemReposicao.Text = string.Empty;

            ddlMes.ClearSelection();
            ddlDisciplina.Items.Clear();
            ddlDataFrequencia.Items.Clear();

            txtPlanoAula.Text = string.Empty;
            lblProfessorAlocado.Text = string.Empty;
            rpCurriculoBasico.DataSource = null;
            rpCurriculoEssencial.DataSource = null;
            rpCurriculoRecomposicao.DataSource = null;
        }

        private string MontarQueryString()
        {
            var queryString = new StringBuilder();

            queryString.Append("ano=" + this.tbAno.Text);
            queryString.Append("&semestre=" + this.tbPeriodo.Text);
            queryString.Append("&turno=" + this.tbTurno.Text);
            queryString.Append("&curso=" + this.tbEscolaridade.Text);
            queryString.Append("&regional=" + this.ObjetoTurma.Regional);
            queryString.Append("&municipio=" + this.ObjetoTurma.Municipio);
            queryString.Append("&serie=" + this.ObjetoTurma.Serie);
            queryString.Append("&grade=" + this.ObjetoTurma.Grade);
            queryString.Append("&faculdade=" + this.ObjetoTurma.Faculdade);
            queryString.Append("&unidadeResponsavel=" + this.ObjetoTurma.UnidadeResponsavel);

            return queryString.ToString();
        }

        private void ObterDadosQueryString(string queryString)
        {
            this.ObjetoTurma = new Turma.DadosTurma();

            var gradeID = string.Empty;
            var listaDados = queryString.Split('&');

            foreach (var dados in listaDados)
            {
                if (dados.IndexOf("ano") >= 0)
                {
                    this.ObjetoTurma.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("semestre") >= 0)
                {
                    this.ObjetoTurma.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("turno") >= 0)
                {
                    this.ObjetoTurma.Turno = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("curso") >= 0)
                {
                    this.ObjetoTurma.Curso = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("unidadeResponsavel") >= 0)
                {
                    this.ObjetoTurma.UnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("prefixoUnidadeResponsavel") >= 0)
                {
                    this.ObjetoTurma.MnemonicoUnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("grade_id") >= 0)
                {
                    gradeID = dados.Substring(dados.LastIndexOf('=') + 1);

                    this.ObjetoTurma.Grade_ID = gradeID;
                }
                else if (dados.IndexOf("gradeId") >= 0)
                {
                    this.ObjetoTurma.Grade_ID = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("grade") >= 0)
                {
                    this.ObjetoTurma.Grade = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("serie") >= 0)
                {
                    this.ObjetoTurma.Serie = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("curriculo") >= 0)
                {
                    this.ObjetoTurma.Curriculo = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("faculdade") >= 0)
                {
                    this.ObjetoTurma.Faculdade = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("nucleo") >= 0)
                {
                    this.ObjetoTurma.Regional = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("municipio") >= 0)
                {
                    this.ObjetoTurma.Municipio = dados.Substring(dados.LastIndexOf('=') + 1);
                }
            }

            if (!string.IsNullOrEmpty(gradeID))
            {
                decimal gradeId;

                decimal.TryParse(gradeID, out gradeId);

                var qtTurma = Turma.ConsultarTurmaPorGradeSerieNota(gradeId);

                if (qtTurma != null)
                {
                    if (qtTurma.Rows.Count > 0)
                    {
                        this.ObjetoTurma.Ano = Convert.ToString(qtTurma.Rows[0]["ANO"]);
                        this.ObjetoTurma.Periodo = Convert.ToString(qtTurma.Rows[0]["SEMESTRE"]);
                        this.ObjetoTurma.Turno = Convert.ToString(qtTurma.Rows[0]["TURNO"]);
                        this.ObjetoTurma.Curso = Convert.ToString(qtTurma.Rows[0]["CURSO"]);
                        this.ObjetoTurma.UnidadeResponsavel = Convert.ToString(qtTurma.Rows[0]["UNIDADE_RESPONSAVEL"]);
                        this.ObjetoTurma.Grade = Convert.ToString(qtTurma.Rows[0]["TURMA"]);
                        this.ObjetoTurma.Serie = Convert.ToString(qtTurma.Rows[0]["SERIE"]);
                        this.ObjetoTurma.Curriculo = Convert.ToString(qtTurma.Rows[0]["CURRICULO"]);
                        this.ObjetoTurma.Faculdade = Convert.ToString(qtTurma.Rows[0]["FACULDADE"]);
                    }
                }
            }
        }

        private void RetirarVisibilidadeGradeNotas()
        {

            this.btnSalvar.Visible = this.btnCancelar.Visible = false;
            btnImprimirComp.Visible = false;
        }


        private void CarregaDisciplina(int ano, int semestre, string turma)
        {
            RN.Disciplina rnDisciplina = new Disciplina();

            ddlDisciplina.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlDisciplina.DataSource = rnDisciplina.ListaDisciplinasTurmaComFrequenciaPor(ano, semestre, turma);
            ddlDisciplina.DataBind();
            ddlDisciplina.Items.Insert(0, item);
        }

        private void CarregaDatas(int ano, int semestre, string turma, string disciplina, int mes)
        {
            RN.HorariosDocente rnHorariosDocente = new HorariosDocente();

            ddlDataFrequencia.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlDataFrequencia.DataSource = rnHorariosDocente.ListaDataFrequenciaPor(ano, semestre, turma, disciplina, mes);
            ddlDataFrequencia.DataBind();
            ddlDataFrequencia.Items.Insert(0, item);
        }

        protected void lnkBasico_Click(object sender, EventArgs e)
        {
            try
            {
                pnlBasico.Visible = !pnlBasico.Visible;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkEssencial_Click(object sender, EventArgs e)
        {
            try
            {

                pnlEssencial.Visible = !pnlEssencial.Visible;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkRecomposicao_Click(object sender, EventArgs e)
        {
            try
            {

                pnlRecomposicao.Visible = !pnlRecomposicao.Visible;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        //protected void grdMatriculas_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        //{

        //    var situacao = grdMatriculas.GetRowValues(e.VisibleIndex, "sit_matricula");

        //    if (Convert.ToString(situacao) != "Matriculado")
        //    {
        //        e.Cell.BackColor = Color.FromName("#ffddcc");

        //    }
        //}

        protected void grdMatriculas_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            var situacao = grdMatriculas.GetRowValues(e.VisibleIndex, "Situacao");

            if (Convert.ToString(situacao) != "Matriculado" && Convert.ToString(situacao) != "Matrícula em Suspensão")
            {
                e.Cell.BackColor = Color.FromName("#ffddcc");
            }
            else if (Convert.ToString(situacao) == "Matrícula em Suspensão")
            {
                e.Cell.BackColor = Color.FromName("#FFFACD"); // AMARELO PASTEL
            }
        }

        public class CheckBoxTemplate : ITemplate
        {
            private readonly string id;

            public CheckBoxTemplate(string id)
            {
                this.id = id;
            }
            public void InstantiateIn(Control container)
            {
                CheckBox tb = new CheckBox();
                tb.ID = id;
                container.Controls.Add(tb);
            }
        }

        private void CarregarDadosGrid(int ano, int semestre, string turma, string disciplina, int diaSemana, DateTime dataLancamento)
        {
            DataTable dt = new DataTable();
            DataTable dtHorarios = new DataTable();
            RN.Turmas.FrequenciaDiaria rnFrequenciaDiaria = new Techne.Lyceum.RN.Turmas.FrequenciaDiaria();

            RN.HorariosDocente rnHorariosDocente = new HorariosDocente();

            dtHorarios = rnHorariosDocente.ListaHorariosPor(ano, semestre, turma, disciplina, diaSemana, dataLancamento);

            //Limpa grid
            grdMatriculas.Columns.Clear();
            grdMatriculas.DataSource = null;

            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Nº", FieldName = "NumChamada", Name = "num_chamada", UnboundType = UnboundColumnType.String });
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Aluno", FieldName = "Matricula", Name = "Aluno", UnboundType = UnboundColumnType.String });
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Nome Social", FieldName = "NomeSocial", Name = "nome", UnboundType = UnboundColumnType.String });
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Nome", FieldName = "Nome", Name = "nome", UnboundType = UnboundColumnType.String });
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Sit. Aluno", FieldName = "Situacao", Name = "sit_matricula", UnboundType = UnboundColumnType.String });


            foreach (DataRow row in dtHorarios.Rows)
            {
                GridViewDataColumn colHorario = new GridViewDataColumn();
                colHorario.Caption = row["HORARIO"].ToString();
                colHorario.FieldName = row["AULA"].ToString();
                colHorario.Name = row["AULA"].ToString();
                colHorario.UnboundType = UnboundColumnType.Boolean;
                colHorario.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colHorario.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                colHorario.CellStyle.HorizontalAlign = HorizontalAlign.Center;


                colHorario.DataItemTemplate = new CheckBoxTemplate("chk_" + row["AULA"].ToString());

                grdMatriculas.Columns.Add(colHorario);
            }

            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Faltas", FieldName = "FaltasTempos", Name = "faltas_tempos", UnboundType = UnboundColumnType.String });
            grdMatriculas.Columns.Add(new GridViewDataTextColumn { Caption = "Justificativa de ausência", FieldName = "Justificativa", Name = "justificativa", UnboundType = UnboundColumnType.String });

            dt = rnFrequenciaDiaria.ListaMatriculaPor(Convert.ToInt32(ano), Convert.ToInt32(semestre), turma.ToString(), disciplina.ToString(), Convert.ToDateTime(ddlDataFrequencia.SelectedValue));


            if (dt.Rows.Count > 0)
            {
                grdMatriculas.Visible = true;
                grdMatriculas.DataSource = dt;
                grdMatriculas.DataBind();
            }
            else
            {
                grdMatriculas.Visible = false;
            }
        }

        protected void grdMatriculas_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (this.grdMatriculas.VisibleRowCount == 0)
            {
                return;
            }

            foreach (GridViewColumn column_tmp in grdMatriculas.Columns)
            {
                GridViewDataColumn column = null;
                if (column_tmp is GridViewDataColumn)
                    column = (GridViewDataColumn)column_tmp;
                else
                    continue;

                GridViewDataItemTemplateContainer container = null;
                foreach (Control o in e.Row.Cells[column.Index].Controls)
                    if (o is GridViewDataItemTemplateContainer)
                        container = (GridViewDataItemTemplateContainer)o;

                if (container == null) continue;


                String controlID = column.FieldName;
                int rowIndex = e.VisibleIndex;
                int columnIndex = column.Index;

                var sitMatricula = (string)this.grdMatriculas.GetRowValues(e.VisibleIndex, "Situacao");
                var justificativa = Convert.ToString(this.grdMatriculas.GetRowValues(e.VisibleIndex, "Justificativa"));

                CheckBox chk = grdMatriculas.FindRowCellTemplateControl(e.VisibleIndex, column, column_tmp.Name) as CheckBox;


                var somenteLeitura = (sitMatricula != "Matriculado" && sitMatricula != "Matrícula em Suspensão");

                //Verifica o tipo do controle no Template da célula
                if (column.DataItemTemplate is CheckBoxTemplate)
                {
                    CheckBoxTemplate template = (CheckBoxTemplate)column.DataItemTemplate;
                    if (container.Controls[0] is CheckBox)
                    {
                        CheckBox tb = (CheckBox)container.Controls[0];
                        tb.ID = controlID;                

                        var frequencia = Convert.ToString(grdMatriculas.GetRowValues(e.VisibleIndex, tb.ID));

                        if (frequencia == "1")
                        {
                            tb.Checked = true;
                        }

                        if (frequencia == "0")
                        {
                            tb.Checked = false;
                        }

                        tb.Visible = !somenteLeitura;

                        if (!justificativa.IsNullOrEmptyOrWhiteSpace())
                        {
                            tb.Checked = false;
                            tb.Enabled = false;
                        }
                    }
                }

            }
        }

    }
}