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

namespace Techne.Lyceum.Net.Academico
{

    [NavUrl("~/Academico/HistoricoNotasTurma.aspx"), ControlText("HistoricoNotasTurma"), Title("Notas e Frequências por Turma - Histórico")]
    public partial class HistoricoNotasTurma : TPage
    {
        // private const string SitMatriculado = "Matriculado";

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

        private decimal Subperiodo
        {
            get
            {
                var superiodo = this.ViewState["subperiodo"];

                return superiodo == null ? -1.0m : (decimal)superiodo;
            }

            set
            {
                this.ViewState["subperiodo"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdMatriculas, "Lançamento de Notas e Frequências");
            TituloGrid(this.grdConsolidado, "Notas e frequências consolidadas da turma");

            var mp = (LyceumMaster)this.Master;

            if (mp != null)
            {
                mp.habilitaLoading = true;
            }

            this.Page.MaintainScrollPositionOnPostBack = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                var grade_id = this.ObtemGradeId();

                if (grade_id.HasValue)
                {
                    this.CarregarDadosIniciais(grade_id.Value);
                }
                else
                {
                    this.pnlDadosTurma.Visible = false;
                    this.pnlDisciplina.Visible = false;
                    this.pnlGridMatriculas.Visible = false;
                    this.Response.Redirect("~/Academico/ListarNotasTurma.aspx");
                }

                pnMensagemHdn.Visible = false;
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                btnVoltar.Visible = true;
                btnImprimirComp.Visible = false;
                this.grdMatriculas.Visible = false;

                this.lblAulasDadas.Visible = false;
                this.txtAulasDadas.Visible = false;
                this.lblAulasPrevistas.Visible = false;
                this.txtAulasPrevistas.Visible = false;
            }

            if (this.cmbdisciplina.SelectedValue != "Selecione")
            {
                this.grdMatriculas.Visible = true;
            }
            else
            {
                this.grdMatriculas.Visible = false;
            }

            this.lblMensagem.Visible = false;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (this.cmbdisciplina.SelectedValue == "Selecione")
            {
                return;
            }

            var disciplina = this.cmbdisciplina.SelectedValue;
            var turma = this.tbTurma.Text;
            var ano = string.IsNullOrEmpty(this.tbAno.Text) ? 0 : Convert.ToDecimal(this.tbAno.Text);
            var periodo = string.IsNullOrEmpty(this.tbPeriodo.Text) ? 0 : Convert.ToDecimal(this.tbPeriodo.Text);
            //var subperiodo = this.Subperiodo;
            var provaRow = ProvaTurma.ConsultarProva(disciplina, turma, ano, periodo, Subperiodo);

            if (provaRow != null)
            {
                if (this.grdMatriculas.Columns["MÉDIA"] != null)
                {
                    this.grdMatriculas.Columns["MÉDIA"].Visible = !string.IsNullOrEmpty(provaRow.Prova);
                }
            }

            var freq = Nota.ObterFrequencia(ano.ToString(), periodo.ToString(), disciplina, turma, Subperiodo);
            var dadosFrequencia = freq.Split('|');

            this.hdnFreq.Value = freq == string.Empty ? freq : dadosFrequencia[0];

            if (string.IsNullOrEmpty(this.hdnFreq.Value))
            {
                this.lblAulasDadas.Visible = false;
                this.txtAulasDadas.Visible = false;
                this.lblAulasPrevistas.Visible = false;
                this.txtAulasPrevistas.Visible = false;
            }
            else
            {
                this.lblAulasDadas.Visible = true;

                this.txtAulasDadas.Visible = true;
                if (string.IsNullOrEmpty(txtAulasDadas.Text.Trim()))
                {
                    this.txtAulasDadas.Text = dadosFrequencia[2].Trim();
                }

                this.lblAulasPrevistas.Visible = true;

                this.txtAulasPrevistas.Visible = true;
                if (string.IsNullOrEmpty(txtAulasPrevistas.Text.Trim()))
                {
                    this.txtAulasPrevistas.Text = dadosFrequencia[3].Trim();
                }
            }

            //if (string.IsNullOrEmpty(this.hdnFreq.Value)
            //    && this.grdMatriculas.Columns["faltas"] != null)
            //{
            //    this.grdMatriculas.Columns["faltas"].Visible = false;
            //}
            //else if (this.grdMatriculas.Columns["faltas"] != null)
            //{
            //    this.grdMatriculas.Columns["faltas"].Visible = true;
            //}

            var pares = new Dictionary<string, string>
                        {
                            { "matricula", this.User.Identity.Name }, 
                            { "idvinculo", this.User.Identity.Name },
                            { "disciplina", disciplina }, 
                            { "turma", turma }, 
                            { "ano", ano.ToString() }, 
                            { "periodo", periodo.ToString() }, 
                            { "subperiodo", this.Subperiodo.ToString() }, 
                            { "semestre", periodo.ToString() }, 
                            { "nome", Usuarios.BuscaNome(User.Identity.Name) }, 
                            { "escola", tbUnidadeEnsino.Text}, 
                            { "nomedisciplina", this.cmbdisciplina.SelectedValue }
                        };

            if (!string.IsNullOrEmpty(Convert.ToString(this.ViewState["protocolo"])))
            {
                pares.Add("protocolo", Convert.ToString(this.ViewState["protocolo"]));

                this.ViewState["protocolo"] = null;
            }
            else
            {
                pares.Add("protocolo", "(sem protocolo)");
            }

            this.btnImprimirComp.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=Filipeta&grp=dol&" + CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
            this.btnImprimirConsolidado.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=FilipetaConsolidadaHistorico&grp=dol&" + CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.CarregarDadosGrid();
            if (tcSubperiodo.ActiveTab != null)
            {
                if (tcSubperiodo.ActiveTab.Name == "5")
                {
                    pnlAulas.Visible = false;
                    pnMensagemHdn.Visible = false;
                    pnlGridMatriculas.Visible = false;
                    pnlConsolidado.Visible = true;
                    pnlNotasConsolidada.Visible = true;
                    btnCancelar.Visible = false;
                    btnSalvar.Visible = false;
                    btnImprimirComp.Visible = false;

                    if (grdConsolidado.VisibleRowCount > 0)
                    {
                        btnImprimirConsolidado.Visible = true;
                        pnlMaterialEstudo.Visible = false;
                    }
                }
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

        protected void btnCancelarVoltar_Click(object sender, ImageClickEventArgs e)
        {
            this.RetirarVisibilidadeGradeNotas();

            var queryString = this.MontarQueryString();
            var bytesToEncode = Encoding.UTF8.GetBytes(queryString);

            this.Server.Transfer("ListarNotasTurmaHistorico.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            decimal aulasDadas = 0m;
            int quantidadeSemAvaliacao = 0;
            int quantidadeTotalAlunos = 0;
            decimal frequenciaMinima = Convert.ToDecimal(System.Configuration.ConfigurationSettings.AppSettings["FrequenciaMinimaLancamentoNotas"]);
            decimal porcentagemMaximaSemAvaliacao = Convert.ToDecimal(System.Configuration.ConfigurationSettings.AppSettings["PorcentagemMaximaSemAvaliacaoLancamentoNotas"]);
            RN.Disciplina rnDisciplina = new Disciplina();
            NotaHistmatr rnNotaHistmatr = new NotaHistmatr();
            FaltaHistmatr rnFaltaHistmatr = new FaltaHistmatr();
            RN.LancamentoNotas.TurmaMaterialEstudo rnTurmaMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.TurmaMaterialEstudo();

            string freq = Nota.ObterFrequencia(this.tbAno.Text, this.tbPeriodo.Text, this.cmbdisciplina.SelectedValue, this.tbTurma.Text, decimal.Parse(this.tcSubperiodo.ActiveTab.Name));
            this.hdnFreq.Value = freq == string.Empty ? freq : freq.Split('|')[0];

            if ((this.hdnFreq.Value != string.Empty) && hdnTemFreq.Value == "S")
            {
                if (this.txtAulasPrevistas.Text == string.Empty)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Por favor, informe as aulas previstas no período letivo.');", true);
                    return;
                }

                decimal aulasPrevistas;

                if (!decimal.TryParse(this.txtAulasPrevistas.Text, out aulasPrevistas))
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Valor inválido para aulas previstas. O valor deve ser um número inteiro e positivo.');", true);
                    return;
                }

                if (aulasPrevistas <= 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não é permitido informar \"0\" para as aulas previstas no período. O número deve ser positivo e diferente de \"0\".');", true);
                    return;
                }

                if (this.txtAulasDadas.Text == string.Empty)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Por favor, informe as aulas dadas no período letivo.');", true);
                    return;
                }

                if (!decimal.TryParse(this.txtAulasDadas.Text, out aulasDadas))
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Valor inválido para aulas dadas. O valor deve ser um número inteiro e positivo.');", true);
                    return;
                }

                if (aulasDadas < 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('O número deve ser positivo e diferente de \"0\".');", true);
                    return;
                }

                if (aulasDadas > 999)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('As aulas dadas deve ter no máximo 3 dígitos.');", true);
                    return;
                }
            }

            string disciplina = string.Empty;
            string nomeDisciplina = string.Empty;

            if (this.cmbdisciplina.SelectedValue != "Selecione")
            {
                disciplina = this.cmbdisciplina.SelectedValue;
                nomeDisciplina = this.cmbdisciplina.SelectedItem.Text.Replace(disciplina, string.Empty).Trim();
            }

            string turma = this.tbTurma.Text;
            var ano = Convert.ToDecimal(this.tbAno.Text);
            var semestre = Convert.ToDecimal(this.tbPeriodo.Text);
            var notas = new List<LyNotaHistmatr>();
            var faltas = new List<LyFaltaHistmatr>();
            var periodo = Convert.ToInt16(semestre);
            var subperiodo = this.Subperiodo;
            var prova = ProvaTurma.ConsultarProva(disciplina, turma, ano, periodo, subperiodo);
            var listTurmaMaterialEstudo = new List<int>();

            // Percorre as provas da disciplina, e obtém os dados da coluna da grid referente à prova
            if (prova != null || this.hdnFreq.Value != null)
            {
                for (var rowIndex = 0; rowIndex < this.grdMatriculas.VisibleRowCount; rowIndex++)
                {
                    var aluno = this.grdMatriculas.GetRowValues(rowIndex, "aluno").ToString();
                    var ordem = this.grdMatriculas.GetRowValues(rowIndex, "ORDEM").ToString();
                    var sitMatricula = this.grdMatriculas.GetRowValues(rowIndex, "sit_matricula").ToString();
                    var nota = new LyNotaHistmatr();

                    if (aluno != null && sitMatricula != "Cancelado" && sitMatricula != "Dispensado" && sitMatricula != "Inconcluido" && sitMatricula != "Trancado")
                    {
                        quantidadeTotalAlunos = quantidadeTotalAlunos + 1;

                        var txtConceito = DevExpressHelper.GetControl<TextBox>(this.grdMatriculas, rowIndex, "MÉDIA", "txtConceito");
                        var txtFrequencia = DevExpressHelper.GetControl<TextBox>(this.grdMatriculas, rowIndex, "faltas", "txtFrequencia");
                        var txtNotaRecuperacao = DevExpressHelper.GetControl<TextBox>(this.grdMatriculas, rowIndex, "NotaRecuperacao", "txtNotaRecuperacao");
                        var txtNotaFinal = DevExpressHelper.GetControl<TextBox>(this.grdMatriculas, rowIndex, "NotaFinal", "txtNotaFinal");
                        var chkRecuperacao = DevExpressHelper.GetControl<CheckBox>(this.grdMatriculas, rowIndex, "recuperacao_paralela", "chkRecuperacao");
                        var chkSemAvaliacao = DevExpressHelper.GetControl<CheckBox>(this.grdMatriculas, rowIndex, "sem_avaliacao", "chkSemAvaliacao");
                        var cmbJustificativa = DevExpressHelper.GetControl<DropDownList>(this.grdMatriculas, rowIndex, "justificativa", "cmbJustificativa");

                        nota.Aluno = aluno;
                        nota.Disciplina = disciplina;//prova.Disciplina;
                        nota.Turma = turma;//prova.Turma;
                        nota.Ano = ano; //prova.Ano;
                        nota.Semestre = semestre; //prova.Semestre;
                        nota.Ordem = Convert.ToDecimal(ordem);

                        if (prova != null)
                        {
                            nota.NotaId = prova.Prova;
                        }

                        if (chkRecuperacao.Checked
                            && string.IsNullOrEmpty(txtNotaRecuperacao.Text.Replace(".", ",")))
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('O campo Nota de Recuperação de estudos é de preenchimento obrigatório quando o campo Rec. Paralela estiver marcado.');", true);
                            return;
                        }

                        if (hdnTemNota.Value == "S")
                        {
                            if (!(txtConceito.Text.Replace(".", ",")).Contains(",") && chkSemAvaliacao.Checked == false)
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Valor inválido para nota do(a) aluno(a): " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + ". A nota deve ter uma casa decimal (Exemplo: 10,0).');", true);
                                return;
                            }
                            if (!string.IsNullOrEmpty(txtConceito.Text))
                            {
                                if ((Convert.ToDecimal(txtConceito.Text.Replace(".", ",")) < 0 || Convert.ToDecimal(txtConceito.Text.Replace(".", ",")) > 10 && chkSemAvaliacao.Checked == false) && chkSemAvaliacao.Checked == false)
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Valor inválido para nota do(a) aluno(a): " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + ". A nota deve estar entre 0,0 e 10,0');", true);
                                    return;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(txtNotaRecuperacao.Text) && chkSemAvaliacao.Checked == false)
                        {
                            if (!(txtNotaRecuperacao.Text.Replace(".", ",")).Contains(","))
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Valor inválido para nota de recuperação de estudos do(a) aluno(a): " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + ". A nota de recuperação de estudos deve ter uma casa decimal (Exemplo: 10,0).');", true);
                                return;
                            }
                        }

                        if (!string.IsNullOrEmpty(txtNotaRecuperacao.Text))
                        {
                            if ((Convert.ToDecimal(txtNotaRecuperacao.Text.Replace(".", ",")) < 0 || Convert.ToDecimal(txtNotaRecuperacao.Text.Replace(".", ",")) > 10 && chkSemAvaliacao.Checked == false) && chkSemAvaliacao.Checked == false)
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Valor inválido para nota de recuperação de estudos do(a) aluno(a): " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + ". A nota de recuperação de estudos deve estar entre 0,0 e 10,0');", true);
                                return;
                            }
                        }

                        if (hdnTemFreq.Value == "S" && (this.hdnFreq.Value != string.Empty))
                        {
                            if ((!string.IsNullOrEmpty(txtConceito.Text) || !string.IsNullOrEmpty(txtNotaRecuperacao.Text)) && aulasDadas == 0)
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não é permitido lançamento de notas e faltas para disciplinas que não possuem aulas dadas. Por favor, verifique o valor informado para o item \"Aulas Dadas\".');", true);
                                return;
                            }
                        }

                        if (chkRecuperacao.Checked
                            && string.IsNullOrEmpty(txtNotaRecuperacao.Text))
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('O campo Nota de Recuperação de estudos é de preenchimento obrigatório quando o campo Rec. Paralela estiver marcado.');", true);
                            return;
                        }

                        if (chkSemAvaliacao.Checked
                            && cmbJustificativa.SelectedIndex == 0)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('O campo Justificativa é de preenchimento obrigatório quando o campo Sem Avaliação estiver marcado.');", true);
                            return;
                        }

                        if ((string.IsNullOrEmpty(txtConceito.Text)) && (string.IsNullOrEmpty(txtNotaRecuperacao.Text)) && (chkSemAvaliacao.Checked == false) && (hdnTemNota.Value == "S"))
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('É necessário lançar as notas de todos os alunos matriculados.');", true);
                            return;
                        }

                        if (chkSemAvaliacao.Checked)
                        {
                            nota.NotaProva = null;
                            nota.Conceito = null;
                            nota.SemAvaliacao = "S";
                            quantidadeSemAvaliacao = quantidadeSemAvaliacao + 1;
                            if (cmbJustificativa.SelectedIndex == 0)
                            {
                                nota.MotivoSemNotaId = null;
                            }
                            else
                            {
                                nota.MotivoSemNotaId = Convert.ToInt32(cmbJustificativa.SelectedValue);
                            }
                        }
                        else
                        {
                            if (hdnTemNota.Value == "S")
                            {
                                nota.NotaProva = Convert.ToDecimal(txtConceito.Text.Replace(".", ","));
                                nota.Conceito = nota.NotaProva.ToString(); //Inicia com nota da prova
                                nota.SemAvaliacao = "N";
                                nota.MotivoSemNotaId = null;
                            }
                        }

                        if (chkRecuperacao.Checked && !chkSemAvaliacao.Checked)
                        {
                            nota.RecuperacaoParalela = "S";
                            nota.NotaRecuperacao = Convert.ToDecimal(txtNotaRecuperacao.Text.Replace(".", ","));
                            nota.Conceito = nota.NotaProva > nota.NotaRecuperacao ? nota.NotaProva.ToString() : nota.NotaRecuperacao.ToString(); //Se existir nota de recuperacao pega sempre o maior valor
                        }
                        else
                        {
                            nota.RecuperacaoParalela = "N";
                            nota.NotaRecuperacao = null;
                        }

                        if (chkRecuperacao.Checked
                           && (nota.NotaProva >= (string.IsNullOrEmpty(prova.NotaMax) ? 0m : decimal.Parse(prova.NotaMax.Replace(".", ","))) / 2m))
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('O Aluno " + aluno + " - " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + " possui nota acima da média e por isso não é permitido informar recuperação.');", true);
                            return;
                        }

                        if (this.hdnFreq.Value != string.Empty && txtFrequencia != null && hdnTemFreq.Value == "S")
                        {
                            decimal? decFalta;

                            if (!string.IsNullOrEmpty(txtFrequencia.Text))
                            {
                                decimal faltaTmp;

                                if (!decimal.TryParse(txtFrequencia.Text, out faltaTmp))
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Valor inválido para falta do aluno: " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + ". O valor deve ser um número inteiro e positivo.');", true);
                                    return;
                                }

                                decFalta = faltaTmp;
                            }
                            else
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('É necessário lançar as faltas de todos os alunos matriculados.');", true);
                                return;
                            }

                            if (decFalta > aulasDadas)
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não é permitido lançar \"Faltas\" superior à \"Aulas Dadas\". Aluno: " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + ".');", true);
                                return;
                            }

                            if (decFalta < 0)
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não é permitido lançar faltas inferior à \"0\". Aluno: " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + ".');", true);
                                return;
                            }

                            if (chkSemAvaliacao.Checked && decFalta == 0
                                && aulasDadas > 0)
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('O Aluno " + aluno + " - " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + " possui 100% de frequência e por isso não é permitido informar que o mesmo não possui avaliação.');", true);
                                return;
                            }

                            if (aulasDadas == decFalta
                                && (!string.IsNullOrEmpty(txtConceito.Text) || !string.IsNullOrEmpty(txtNotaRecuperacao.Text)))
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('O Aluno " + aluno + " - " + this.grdMatriculas.GetRowValues(rowIndex, "nome_compl") + " possui 0% de frequência e por isso não é permitido informar a nota. No caso de ausência de avaliação, marcar o item  \"Sem Avaliação\" e sua respectiva justificativa.');", true);
                                return;
                            }

                            if (hdnTemFreq.Value == "S" && hdnFreq.Value != string.Empty)
                            {
                                faltas.Add(
                                    new LyFaltaHistmatr
                                    {
                                        Aluno = aluno,
                                        Disciplina = disciplina,//prova.Disciplina,
                                        Turma = turma, //prova.Turma,
                                        Ano = ano, //prova.Ano,
                                        Semestre = semestre, //prova.Semestre,
                                        Faltas = decFalta,
                                        Ordem = Convert.ToDecimal(ordem),
                                        FreqId = this.hdnFreq.Value
                                    });
                            }
                        }

                        if (hdnTemNota.Value == "S")
                        {
                            notas.Add(nota);
                        }

                        // Corrige coloração do TextBox (para evitar chamada de HtmlRowCreated)
                        decimal notaValor;

                        if (hdnTemNota.Value == "S")
                        {
                            if (prova != null)
                            {
                                if (decimal.TryParse(txtConceito.Text.Replace(".", ","), out notaValor))
                                {
                                    txtConceito.Attributes.CssStyle["color"] = (notaValor >= (string.IsNullOrEmpty(prova.NotaMax) ? 0m : decimal.Parse(prova.NotaMax.Replace(".", ","))) / 2m) ? "Blue" : "Red";
                                }

                                if (decimal.TryParse(txtNotaRecuperacao.Text.Replace(".", ","), out notaValor))
                                {
                                    txtNotaRecuperacao.Attributes.CssStyle["color"] = (notaValor >= (string.IsNullOrEmpty(prova.NotaMax) ? 0m : decimal.Parse(prova.NotaMax.Replace(".", ","))) / 2m) ? "Blue" : "Red";
                                }

                                if (decimal.TryParse(txtNotaFinal.Text.Replace(".", ","), out notaValor))
                                {
                                    txtNotaFinal.Attributes.CssStyle["color"] = (notaValor >= (string.IsNullOrEmpty(prova.NotaMax) ? 0m : decimal.Parse(prova.NotaMax.Replace(".", ","))) / 2m) ? "Blue" : "Red";
                                }
                            }
                        }
                    }
                }

                var ctx = DataContextBuilder.FromLyceum.UsingLock();
                var contextQueries = new List<ContextQuery>();

                try
                {
                    ctx.BeginBulkModifications();

                    if (hdnTemNota.Value == "S")
                    {
                        rnNotaHistmatr.AtualizaNotas(
                        notas,
                        HttpContext.Current.User.Identity.Name,
                        subperiodo,
                        contextQueries,
                        ctx);
                    }

                    if (hdnTemFreq.Value == "S" && (this.hdnFreq.Value != string.Empty))
                    {
                        rnFaltaHistmatr.AtualizaFrequencias(
                        HttpContext.Current.User.Identity.Name,
                        string.IsNullOrEmpty(this.hdnFreq.Value) ? null : faltas,
                        this.txtAulasDadas.Text.Trim(),
                        this.txtAulasPrevistas.Text.Trim(),
                        subperiodo,
                        contextQueries,
                        ctx);
                    }

                    foreach (ListItem item in cblMaterialEstudo.Items)
                    {
                        if (item.Selected)
                        {
                            listTurmaMaterialEstudo.Add(Convert.ToInt32(item.Value));
                        }
                    }
                    if (listTurmaMaterialEstudo.Count > 0)
                    {
                        rnTurmaMaterialEstudo.InsereLista(ctx, listTurmaMaterialEstudo, Convert.ToInt32(ano), Convert.ToInt32(periodo), Convert.ToInt32(subperiodo), turma, disciplina, User.Identity.Name);
                    }
                    else
                    {
                        if (Convert.ToInt32(ano) > 2020)
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('É necessário escolher pelo menos um Material de Estudo Proposto.');", true);
                            return;
                        }
                    }

                    // Executar todas as alterações
                    foreach (var contextQuery in contextQueries)
                    {
                        ctx.ApplyModifications(contextQuery);
                    }

                    ctx.EndBulkModifications();

                    var script = @"alert('Lançamento realizado com sucesso!');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    this.CarregarDadosGrid();
                    this.grdMatriculas.DataBind();

                }
                catch (Exception erro)
                {
                    ctx.Abandon();
                    this.ExibirMensagem("Erro durante atualização de notas: " + erro.Message);
                }
                finally
                {
                    ctx.Dispose();
                }

            }
        }

        protected void cmbdisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnVoltar.Visible = false;
            pnlAviso.Visible = true;
            cblMaterialEstudo.ClearSelection();

            if (this.cmbdisciplina.SelectedValue == "Selecione")
            {
                this.tcSubperiodo.Tabs.Clear();                
                pnMensagemHdn.Visible = false;
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                btnVoltar.Visible = true;
                btnImprimirComp.Visible = false;
                this.grdMatriculas.Visible = false;

                this.lblAulasDadas.Visible = false;
                this.txtAulasDadas.Visible = false;
                this.lblAulasPrevistas.Visible = false;
                this.txtAulasPrevistas.Visible = false;

                pnlAulas.Visible = false;
                pnlGridMatriculas.Visible = false;
                pnlConsolidado.Visible = false;
                pnlNotasConsolidada.Visible = false;
                grdConsolidado.Visible = false;
                grdConsolidado.DataSource = null;
                grdMatriculas.DataSource = null;
                grdConsolidado.DataBind();
                grdMatriculas.DataBind();
                return;
            }

            CarregaDados("1");
        }
        private void CarregaMaterialEstudo()
        {
            RN.LancamentoNotas.MaterialEstudo rnMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.MaterialEstudo();

            cblMaterialEstudo.Items.Clear();
            cblMaterialEstudo.DataSource = rnMaterialEstudo.ListaAtivoPor();
            cblMaterialEstudo.DataTextField = "DESCRICAO";
            cblMaterialEstudo.DataValueField = "MATERIALESTUDOID";
            cblMaterialEstudo.DataBind();
        }

        private void CarregaDados(string subPeriodoTab)
        {
            RN.LancamentoNotas.TurmaMaterialEstudo rnTurmaMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.TurmaMaterialEstudo();
            List<int> turmaMaterialEstudo = new List<int>();


            // limpa os campos aulas previstas e aulas dadas
            // --------------------------------------------------
            txtAulasPrevistas.Text = "";
            txtAulasDadas.Text = "";
            // --------------------------------------------------
            pnlConsolidado.Visible = false;
            pnlNotasConsolidada.Visible = false;
            grdConsolidado.DataSource = null;
            grdConsolidado.DataBind();

            if (this.cmbdisciplina.SelectedValue != "Selecione")
            {
                string tem_nota = string.Empty;
                string tem_freq = string.Empty;

                DataTable dt = Lyceum.RN.Disciplina.ConsultarPorDisciplina(cmbdisciplina.SelectedValue);

                tem_nota = dt.Rows[0]["tem_nota"].ToString();
                tem_freq = dt.Rows[0]["tem_freq"].ToString();

                hdnTemNota.Value = tem_nota;
                hdnTemFreq.Value = tem_freq;

                // mensagens da tela 
                // ----------------------
                if (tem_nota == "N" && tem_freq == "S")
                    msg.InnerHtml = "<br />Esta disciplina não possui lançamento de notas.";
                else if (tem_nota == "S" && tem_freq == "N")
                    msg.InnerHtml = "<br />Esta disciplina não possui lançamento de faltas, aulas previstas e aulas dadas.";
                else if (tem_nota == "N" && tem_freq == "N")
                    msg.InnerHtml = "<br />Esta disciplina não possui lançamento de notas, faltas, aulas previstas e aulas dadas.";
                else
                    msg.InnerHtml = "";

                // ativacao de controles 
                // ----------------------
                if (tem_nota == "N" && tem_freq == "N")
                {
                    txtAulasPrevistas.Style.Add("background-color", "#E0E0E0");
                    txtAulasDadas.Style.Add("background-color", "#E0E0E0");
                    // --------------------------------------------------------------
                    txtAulasPrevistas.Enabled = false;
                    txtAulasDadas.Enabled = false;
                    // --------------------------------------------------------------
                    btnSalvar.Enabled = false;
                }
                else if (tem_nota == "N" && tem_freq == "S")
                {
                    txtAulasPrevistas.Style.Add("background-color", "#FFFFFF");
                    txtAulasDadas.Style.Add("background-color", "#FFFFFF");
                    // --------------------------------------------------------------
                    txtAulasPrevistas.Enabled = true;
                    txtAulasDadas.Enabled = true;
                    // --------------------------------------------------------------
                    btnSalvar.Enabled = true;
                }
                else if (tem_nota == "S" && tem_freq == "N")
                {
                    txtAulasPrevistas.Style.Add("background-color", "#E0E0E0");
                    txtAulasDadas.Style.Add("background-color", "#E0E0E0");
                    // --------------------------------------------------------------
                    txtAulasPrevistas.Enabled = false;
                    txtAulasDadas.Enabled = false;
                    // --------------------------------------------------------------
                    btnSalvar.Enabled = true;
                }
                else
                {
                    txtAulasPrevistas.Style.Add("background-color", "#FFFFFF");
                    txtAulasDadas.Style.Add("background-color", "#FFFFFF");
                    // --------------------------------------------------------------
                    txtAulasPrevistas.Enabled = true;
                    txtAulasDadas.Enabled = true;
                    // --------------------------------------------------------------
                    btnSalvar.Enabled = true;
                }

                ///


                this.RetirarVisibilidadeGradeNotas();
                this.CarregarAbas();

                var subPeriodo = subPeriodoTab; //PeriodoLetivo.ObterSubPeriodoAtual(this.tbAno.Text, subPeriodoTab);

                this.SelecionarAba(subPeriodo.ToString());

                //this.upnlMatriculas.Visible = true;
                this.btnSalvar.Visible = this.btnCancelar.Visible = true;
                pnMensagemHdn.Visible = true;
                pnlGridMatriculas.Visible = true;
                pnlAulas.Visible = true;

                this.grdMatriculas.DataBind();

                var freq = Nota.ObterFrequencia(this.tbAno.Text, this.tbPeriodo.Text, this.cmbdisciplina.SelectedValue, this.tbTurma.Text, Convert.ToDecimal(subPeriodoTab));

                this.hdnFreq.Value = freq == string.Empty ? freq : freq.Split('|')[0];

                if (this.hdnFreq.Value == string.Empty)
                {
                    this.lblAulasDadas.Visible = false;
                    this.txtAulasDadas.Visible = false;
                    this.lblAulasPrevistas.Visible = false;
                    this.txtAulasPrevistas.Visible = false;
                }
                else
                {
                    this.lblAulasDadas.Visible = true;
                    this.txtAulasDadas.Visible = true;
                    this.txtAulasDadas.Text = freq.Split('|')[2].Trim();
                    this.lblAulasPrevistas.Visible = true;
                    this.txtAulasPrevistas.Visible = true;
                    this.txtAulasPrevistas.Text = freq.Split('|')[3].Trim();
                }

                if (Convert.ToInt32(tbAno.Text) > 2020)
                {
                    pnlMaterialEstudo.Visible = true;

                    turmaMaterialEstudo = rnTurmaMaterialEstudo.ListaTurmaMaterialEstudoPor(Convert.ToInt32(tbAno.Text), Convert.ToInt32(tbPeriodo.Text), tbTurma.Text, cmbdisciplina.SelectedValue, Convert.ToInt32(subPeriodo));

                    cblMaterialEstudo.ClearSelection();
                    foreach (var materialEstudo in turmaMaterialEstudo)
                    {
                        cblMaterialEstudo.Items.FindByValue(materialEstudo.ToString()).Selected = true;
                    }
                }
            }
            else
            {
                //this.upnlMatriculas.Visible = true;
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                pnMensagemHdn.Visible = false;
            }

            if (this.tcSubperiodo.ActiveTab != null)
            {
                this.Subperiodo = Convert.ToDecimal(subPeriodoTab);
            }
        }

        private void ControlaAtivacao(int VisibleIndex)
        {
            if (!this.grdMatriculas.Visible
               || this.grdMatriculas.VisibleRowCount == 0
               || this.cmbdisciplina.SelectedValue == "Selecione")
            {
                return;
            }

            var sitMatricula = (string)this.grdMatriculas.GetRowValues(VisibleIndex, "sit_matricula");
            var nomeAluno = (string)this.grdMatriculas.GetRowValues(VisibleIndex, "nome_compl");
            var nomeProva = Convert.ToString(this.grdMatriculas.GetRowValues(VisibleIndex, "nome_prova"));
            var formula = Convert.ToString(this.grdMatriculas.GetRowValues(VisibleIndex, "formula"));
            var contemFormula = !string.IsNullOrEmpty(formula);
            var notaMax = Convert.ToString(this.grdMatriculas.GetRowValues(VisibleIndex, "nota_max"));
            var colMedia = this.grdMatriculas.Columns["MÉDIA"] as GridViewDataColumn;
            var colNotaRecuperacao = this.grdMatriculas.Columns["NotaRecuperacao"] as GridViewDataColumn;
            var colNotaFinal = this.grdMatriculas.Columns["NotaFinal"] as GridViewDataColumn;

            decimal notaValor;
            decimal conceito = 0;
            decimal notarecuperacao = 0;
            decimal resultado = 0;

            if (colMedia == null || colNotaRecuperacao == null)
            {
                return;
            }

            var txtConceito = (TextBox)this.grdMatriculas.FindRowCellTemplateControl(VisibleIndex, colMedia, "txtConceito");
            var txtNotaRecuperacao = (TextBox)this.grdMatriculas.FindRowCellTemplateControl(VisibleIndex, colNotaRecuperacao, "txtNotaRecuperacao");
            var txtNotaFinal = (TextBox)this.grdMatriculas.FindRowCellTemplateControl(VisibleIndex, colNotaFinal, "txtNotaFinal");

            if (txtConceito == null || txtNotaRecuperacao == null)
            //                || string.IsNullOrEmpty(nomeProva)) Removido na demanda Somente Frequencia
            {
                return;
            }

            if (decimal.TryParse(txtConceito.Text.Replace(".", ","), out notaValor))
            {
                conceito = notaValor;

                if (decimal.TryParse(txtNotaRecuperacao.Text.Replace(".", ","), out notaValor))
                {
                    notarecuperacao = notaValor;
                }

                resultado = (conceito > notarecuperacao ? conceito : notarecuperacao);
            }

            txtNotaFinal.Text = resultado > 0 ? resultado.ToString() : string.Empty;

            var somenteLeitura = sitMatricula == "Cancelado" || sitMatricula == "Dispensado" || sitMatricula == "Inconcluido" || sitMatricula == "Trancado" || contemFormula;
            var rowIndex = VisibleIndex;
            var columnIndex = colMedia.Index;

            txtConceito.ReadOnly = somenteLeitura;
            txtNotaRecuperacao.ReadOnly = somenteLeitura;
            txtNotaFinal.ReadOnly = somenteLeitura;
            // txtNotaFinal.Enabled = false;

            if (somenteLeitura)
            {
                txtConceito.BackColor = Color.Gainsboro;
                txtConceito.TabIndex = -1;
                txtConceito.Text = string.Empty;

                txtNotaRecuperacao.BackColor = Color.Gainsboro;
                txtNotaRecuperacao.TabIndex = -1;
                txtNotaRecuperacao.Text = string.Empty;

                txtNotaFinal.BackColor = Color.Gainsboro;
                txtNotaFinal.TabIndex = -1;
                txtNotaFinal.Text = string.Empty;
            }

            if (decimal.TryParse(txtConceito.Text.Replace(".", ","), out notaValor))
            {
                txtConceito.Attributes.CssStyle["color"] = (notaValor >= (string.IsNullOrEmpty(notaMax) ? 0M : decimal.Parse(notaMax.Replace(".", ","))) / 2M) ? "Blue" : "Red";
            }

            if (decimal.TryParse(txtNotaRecuperacao.Text.Replace(".", ","), out notaValor))
            {
                txtNotaRecuperacao.Attributes.CssStyle["color"] = (notaValor >= (string.IsNullOrEmpty(notaMax) ? 0M : decimal.Parse(notaMax.Replace(".", ","))) / 2M) ? "Blue" : "Red";
            }

            //if (decimal.TryParse(txtNotaFinal.Text.Replace(".", ","), out notaValor))
            //{
            //    txtNotaFinal.Attributes.CssStyle["color"] = (notaValor >= (string.IsNullOrEmpty(notaMax) ? 0M : decimal.Parse(notaMax.Replace(".", ","))) / 2M) ? "Blue" : "Red";
            //}

            var colSemAvaliacao = (GridViewDataColumn)this.grdMatriculas.Columns["sem_avaliacao"];
            var chkSemAvaliacao = (CheckBox)this.grdMatriculas.FindRowCellTemplateControl(VisibleIndex, colSemAvaliacao, "chkSemAvaliacao");

            var chkRecuperacao = DevExpressHelper.GetControl<CheckBox>(this.grdMatriculas, VisibleIndex, "recuperacao_paralela", "chkRecuperacao");

            txtConceito.Attributes.Add("rowIndex", rowIndex.ToString());
            txtConceito.Attributes.Add("columnIndex", columnIndex.ToString());
            txtConceito.Attributes.Add("navegar", "true");
            txtConceito.Attributes.Add("validar", somenteLeitura ? "false" : "true");
            txtConceito.Attributes.Add("notaMax", notaMax);
            txtConceito.Attributes.Add("semAvaliacao", chkSemAvaliacao.ClientID);
            txtConceito.Attributes.Add("sitMatricula", sitMatricula);
            txtConceito.Attributes.Add("nomeAluno", nomeAluno);

            txtNotaRecuperacao.Attributes.Add("rowIndex", rowIndex.ToString());
            txtNotaRecuperacao.Attributes.Add("columnIndex", columnIndex.ToString());
            txtNotaRecuperacao.Attributes.Add("navegar", "true");
            txtNotaRecuperacao.Attributes.Add("validarRecuperacao", "true");
            txtNotaRecuperacao.Attributes.Add("semAvaliacao", chkSemAvaliacao.ClientID);
            txtNotaRecuperacao.Attributes.Add("sitMatricula", sitMatricula);
            txtNotaRecuperacao.Attributes.Add("notaMax", notaMax);
            txtNotaRecuperacao.Attributes.Add("recuperacao", chkRecuperacao.ClientID);
            txtNotaRecuperacao.Attributes.Add("nomeAluno", nomeAluno);

            txtNotaFinal.Attributes.Add("rowIndex", rowIndex.ToString());
            txtNotaFinal.Attributes.Add("columnIndex", columnIndex.ToString());
            txtNotaFinal.Attributes.Add("navegar", "false");
            txtNotaFinal.Attributes.Add("semAvaliacao", chkSemAvaliacao.ClientID);
            txtNotaFinal.Attributes.Add("sitMatricula", sitMatricula);
            txtNotaFinal.Attributes.Add("notaMax", notaMax);

            var colFaltas = (GridViewDataColumn)this.grdMatriculas.Columns["faltas"];
            var txtFaltas = (TextBox)this.grdMatriculas.FindRowCellTemplateControl(VisibleIndex, colFaltas, "txtFrequencia");

            if (txtFaltas == null)
            {
                return;
            }

            txtFaltas.Attributes.Add("navegar", "true");
            txtFaltas.Attributes.Add("rowIndex", rowIndex.ToString());
            txtFaltas.Attributes.Add("columnIndex", colFaltas.Index.ToString());
            txtFaltas.Attributes.Add("falta", "true");
            txtFaltas.Attributes.Add("sitMatricula", sitMatricula);
            txtFaltas.ReadOnly = string.IsNullOrEmpty(hdnFreq.Value) ? false : somenteLeitura;
            txtFaltas.Attributes.Add("nomeAluno", nomeAluno);

            if (somenteLeitura)
            {
                txtFaltas.BackColor = Color.Gainsboro;
                txtFaltas.TabIndex = -1;
                txtFaltas.Text = string.Empty;
            }

            var hfJustificativa = DevExpressHelper.GetControl<HiddenField>(this.grdMatriculas, VisibleIndex, "justificativa", "hfJustificativa");
            var cmbJustificativa = DevExpressHelper.GetControl<DropDownList>(this.grdMatriculas, VisibleIndex, "justificativa", "cmbJustificativa");

            cmbJustificativa.ClearSelection();

            var justificativa = string.IsNullOrEmpty(hfJustificativa.Value) ? "Selecione" : hfJustificativa.Value;

            cmbJustificativa.SelectedValue = justificativa;

            chkSemAvaliacao.InputAttributes.Add("conceito", txtConceito.ClientID);
            chkSemAvaliacao.InputAttributes.Add("NotaRecuperacao", txtNotaRecuperacao.ClientID);
            chkSemAvaliacao.InputAttributes.Add("NotaFinal", txtNotaFinal.ClientID);
            chkSemAvaliacao.InputAttributes.Add("sitMatricula", sitMatricula);
            chkSemAvaliacao.InputAttributes.Add("justificativa", cmbJustificativa.ClientID);
            chkSemAvaliacao.InputAttributes.Add("falta", txtFaltas.ClientID);
            chkSemAvaliacao.InputAttributes.Add("semAvaliacao", chkSemAvaliacao.ClientID);
            chkSemAvaliacao.InputAttributes.Add("recuperacao", chkRecuperacao.ClientID);

            chkRecuperacao.InputAttributes.Add("sitMatricula", sitMatricula);
            chkRecuperacao.InputAttributes.Add("NotaRecuperacao", txtNotaRecuperacao.ClientID);
            chkRecuperacao.InputAttributes.Add("NotaFinal", txtNotaFinal.ClientID);
            chkRecuperacao.InputAttributes.Add("notaMax", notaMax);
            chkRecuperacao.InputAttributes.Add("recuperacao", chkRecuperacao.ClientID);
            chkRecuperacao.InputAttributes.Add("conceito", txtConceito.ClientID);

            chkSemAvaliacao.Enabled = !somenteLeitura && chkSemAvaliacao.Checked;
            chkRecuperacao.Enabled = !somenteLeitura;
            cmbJustificativa.Enabled = false;
        }

        protected void grdMatriculas_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ControlaAtivacao(e.VisibleIndex);
        }

        protected void tcSubperiodo_TabClick(object source, TabControlCancelEventArgs e)
        {
            RN.Turma rnTurma = new Turma();
            Techne.Lyceum.RN.DTOs.DadosConsolidadoBimestralTurma consolidadoBimestral = new Techne.Lyceum.RN.DTOs.DadosConsolidadoBimestralTurma();
            pnlConsolidado.Visible = false;
            pnlNotasConsolidada.Visible = false;
            btnVoltar.Visible = false;
            btnImprimirConsolidado.Visible = false;
            pnlAviso.Visible = true;

            if (!string.IsNullOrEmpty(e.Tab.Name))
            {
                if (e.Tab.Name != "5")
                {
                    var subperiodo = Convert.ToDecimal(e.Tab.Name);
                    ProvaTurma rnProvaTurma = new ProvaTurma();

                    LyProva provaRow = rnProvaTurma.ConsultaProvaPor(cmbdisciplina.SelectedValue, tbTurma.Text, Convert.ToDecimal(tbAno.Text), Convert.ToDecimal(tbPeriodo.Text), subperiodo);

                    var freq = Nota.ObterFrequencia(tbAno.Text, tbPeriodo.Text, cmbdisciplina.SelectedValue, tbTurma.Text, subperiodo);

                    if (hdnTemNota.Value == "S" && provaRow == null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não existem provas para este período letivo.');", true);
                        e.Cancel = true;
                        return;
                    }

                    if (hdnTemFreq.Value == "S" && string.IsNullOrEmpty(freq))
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não existem frequencia para este período letivo.');", true);
                        e.Cancel = true;
                        return;
                    }

                    if ((hdnTemNota.Value == "S" && provaRow != null) || (hdnTemFreq.Value == "S" && freq != ""))
                    {
                        this.Subperiodo = subperiodo;
                        CarregaDados(this.Subperiodo.ToString());
                    }
                }
                else
                {
                    pnlAviso.Visible = false;
                    btnVoltar.Visible = true;
                    btnImprimirConsolidado.Visible = true;
                    txtTotalAulasDadas.Text = string.Empty;
                    txtTotalAulasPrevistas.Text = string.Empty;
                    txtPrimeiroBimestre.Text = string.Empty;
                    txtSegundoBimestre.Text = string.Empty;
                    txtTerceiroBimestre.Text = string.Empty;
                    //txtQuartoBimestre.Text = string.Empty;

                    if (!string.IsNullOrEmpty(tbAno.Text) && !string.IsNullOrEmpty(tbPeriodo.Text) && !string.IsNullOrEmpty(tbTurma.Text) && !string.IsNullOrEmpty(cmbdisciplina.SelectedValue))
                    {
                        consolidadoBimestral = rnTurma.ObtemHistoricoConsolidadoBimestralPor(int.Parse(tbAno.Text), int.Parse(tbPeriodo.Text), tbTurma.Text, cmbdisciplina.SelectedValue);
                        txtTotalAulasDadas.Text = consolidadoBimestral.TotalAulasDadas.ToString();
                        txtTotalAulasPrevistas.Text = consolidadoBimestral.TotalAulasPrevistas.ToString();
                        txtPrimeiroBimestre.Text = consolidadoBimestral.MediaTurmaBimestre1.ToString();
                        txtSegundoBimestre.Text = consolidadoBimestral.MediaTurmaBimestre2.ToString();

                        if (consolidadoBimestral.QuantidadeSubPeriodos > 2)
                        {
                            lblTerceiroBimestre.Visible = true;
                            //lblQuartoBimestre.Visible = true;
                            txtTerceiroBimestre.Visible = true;
                            //txtQuartoBimestre.Visible = true;
                            txtTerceiroBimestre.Text = consolidadoBimestral.MediaTurmaBimestre3.ToString();
                            //txtQuartoBimestre.Text = consolidadoBimestral.MediaTurmaBimestre4.ToString();
                        }
                        else
                        {
                            lblTerceiroBimestre.Visible = false;
                            //lblQuartoBimestre.Visible = false;
                            txtTerceiroBimestre.Visible = false;
                            //txtQuartoBimestre.Visible = false;
                        }

                        hdnTotalSubPeriodo.Value = consolidadoBimestral.QuantidadeSubPeriodos.ToString();
                        CarregarDadosGrid(consolidadoBimestral.QuantidadeSubPeriodos);

                    }
                }
            }
        }

        public class TextBoxNotaTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                TextBox tb = new TextBox();
                container.Controls.Add(tb);
            }
        }
        public class TextBoxNotaAcumuladaTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                TextBox tb = new TextBox();
                container.Controls.Add(tb);
            }
        }
        public class TextBoxFaltaTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                TextBox tb = new TextBox();
                container.Controls.Add(tb);
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
        protected void grdConsolidado_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (this.grdConsolidado.VisibleRowCount == 0)
            {
                return;
            }
            decimal notaMinimaBimestre = 5.0m;
            decimal nota = 0m;
            decimal freqAcumulada = 75.0m;
            foreach (GridViewColumn column_tmp in grdConsolidado.Columns)
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

                String controlID = "gridControl_" + column.FieldName;
                int rowIndex = e.VisibleIndex;
                int columnIndex = column.Index;

                //Verifica o tipo do controle no Template da célula
                if (column.DataItemTemplate is TextBoxNotaTemplate)
                {
                    TextBoxNotaTemplate template = (TextBoxNotaTemplate)column.DataItemTemplate;
                    if (container.Controls[0] is TextBox)
                    {
                        TextBox tb = (TextBox)container.Controls[0];
                        tb.ID = controlID;
                        tb.Text = HttpUtility.HtmlDecode(container.Text).Trim();
                        tb.Width = Unit.Pixel(35);
                        tb.ReadOnly = true;
                        tb.Attributes.Add("style", "text-align:center");
                        tb.Attributes.Add("rowIndex", rowIndex.ToString());
                        tb.Attributes.Add("columnIndex", columnIndex.ToString());
                        tb.Style.Add("background-color", "#E0E0E0");

                        if (!string.IsNullOrEmpty(tb.Text))
                        {
                            nota = Convert.ToDecimal(tb.Text);
                            tb.Attributes.CssStyle["color"] = (nota >= notaMinimaBimestre ? "Blue" : "Red");
                        }
                    }
                }
                if (column.DataItemTemplate is TextBoxNotaAcumuladaTemplate)
                {
                    TextBoxNotaAcumuladaTemplate template = (TextBoxNotaAcumuladaTemplate)column.DataItemTemplate;
                    if (container.Controls[0] is TextBox)
                    {
                        TextBox tb = (TextBox)container.Controls[0];
                        tb.ID = controlID;
                        tb.Text = HttpUtility.HtmlDecode(container.Text).Trim();
                        tb.Width = Unit.Pixel(35);
                        tb.ReadOnly = true;
                        tb.Attributes.Add("style", "text-align:center");
                        tb.Attributes.Add("rowIndex", rowIndex.ToString());
                        tb.Attributes.Add("columnIndex", columnIndex.ToString());
                        tb.Style.Add("background-color", "#E0E0E0");

                        if (!string.IsNullOrEmpty(tb.Text))
                        {
                            nota = Convert.ToDecimal(tb.Text);
                            tb.Attributes.CssStyle["color"] = (nota >= (notaMinimaBimestre * int.Parse(hdnTotalSubPeriodo.Value)) ? "Blue" : "Red");
                        }
                    }
                }
                if (column.DataItemTemplate is TextBoxFaltaTemplate)
                {
                    TextBoxFaltaTemplate template = (TextBoxFaltaTemplate)column.DataItemTemplate;
                    if (container.Controls[0] is TextBox)
                    {
                        TextBox tb = (TextBox)container.Controls[0];
                        tb.ID = controlID;
                        tb.Text = HttpUtility.HtmlDecode(container.Text).Trim();
                        tb.Width = Unit.Pixel(35);
                        tb.ReadOnly = true;
                        tb.Attributes.Add("style", "text-align:center");
                        tb.Attributes.Add("rowIndex", rowIndex.ToString());
                        tb.Attributes.Add("columnIndex", columnIndex.ToString());
                        tb.Style.Add("background-color", "#E0E0E0");
                    }
                }
                if (column.DataItemTemplate is TextBoxFreqAcumuladaTemplate)
                {
                    TextBoxFreqAcumuladaTemplate template = (TextBoxFreqAcumuladaTemplate)column.DataItemTemplate;
                    if (container.Controls[0] is TextBox)
                    {
                        TextBox tb = (TextBox)container.Controls[0];
                        tb.ID = controlID;
                        tb.Text = HttpUtility.HtmlDecode(container.Text).Trim();
                        tb.Width = Unit.Pixel(50);
                        tb.ReadOnly = true;
                        tb.Attributes.Add("style", "text-align:center");
                        tb.Attributes.Add("rowIndex", rowIndex.ToString());
                        tb.Attributes.Add("columnIndex", columnIndex.ToString());
                        tb.Style.Add("background-color", "#E0E0E0");

                        if (!string.IsNullOrEmpty(tb.Text))
                        {
                            tb.Attributes.CssStyle["color"] = (Convert.ToDecimal(tb.Text) >= (freqAcumulada) ? "Blue" : "Red");
                            tb.Text = tb.Text + " %";
                        }
                    }
                }
            }
        }

        private void CarregarDadosGrid(int totalSubPeriodo)
        {
            RN.HistMatricula rnHistMatricula = new Techne.Lyceum.RN.HistMatricula();
            DataTable dtConsolidado = new DataTable();

            //Limpa grid
            grdConsolidado.Columns.Clear();
            grdConsolidado.DataSource = null;

            grdConsolidado.Columns.Add(new GridViewDataTextColumn { Caption = "Nome", FieldName = "NOME_COMPL", Name = "nome", UnboundType = UnboundColumnType.String });
            grdConsolidado.Columns.Add(new GridViewDataTextColumn { Caption = "Situação", FieldName = "SITUACAO_HIST", Name = "sit_matricula", UnboundType = UnboundColumnType.String });

            for (int i = 1; i <= totalSubPeriodo; i++)
            {
                GridViewDataColumn colNota = new GridViewDataColumn();
                colNota.Caption = "Nota Final " + i + " º Bim";
                colNota.FieldName = "NOTA" + i;
                colNota.Name = "nota" + i;
                colNota.Width = Unit.Pixel(30);
                colNota.UnboundType = UnboundColumnType.String;
                colNota.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colNota.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                colNota.CellStyle.HorizontalAlign = HorizontalAlign.Center;

                colNota.DataItemTemplate = new TextBoxNotaTemplate();

                grdConsolidado.Columns.Add(colNota);

                GridViewDataColumn colFalta = new GridViewDataColumn();
                colFalta.Caption = "Faltas " + i + "º Bim";
                colFalta.FieldName = "FALTAS" + i;
                colFalta.Name = "falta" + i;
                colFalta.UnboundType = UnboundColumnType.String;
                colFalta.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colFalta.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                colFalta.CellStyle.HorizontalAlign = HorizontalAlign.Center;

                colFalta.DataItemTemplate = new TextBoxFaltaTemplate();

                grdConsolidado.Columns.Add(colFalta);
            }

            GridViewDataColumn colNotaAcum = new GridViewDataColumn();
            GridViewDataColumn colFaltaAcum = new GridViewDataColumn();
            GridViewDataColumn colFreqAcum = new GridViewDataColumn();

            colNotaAcum.Caption = "Notas Acum.";
            colNotaAcum.FieldName = "NOTASACUMULADAS";
            colNotaAcum.Name = "notasacumuladas";
            colNotaAcum.UnboundType = UnboundColumnType.String;
            colNotaAcum.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colNotaAcum.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            colNotaAcum.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colNotaAcum.DataItemTemplate = new TextBoxNotaAcumuladaTemplate();

            colFaltaAcum.Caption = "Faltas Acum.";
            colFaltaAcum.FieldName = "FALTASACUMULADAS";
            colFaltaAcum.Name = "Faltasacumuladas";
            colFaltaAcum.UnboundType = UnboundColumnType.String;
            colFaltaAcum.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colFaltaAcum.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            colFaltaAcum.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colFaltaAcum.DataItemTemplate = new TextBoxFaltaTemplate();

            colFreqAcum.Caption = "% Freq. Acum.";
            colFreqAcum.FieldName = "PERCENTUALFREQUENCIAACUMULADA";
            colFreqAcum.Name = "percentualfrequenciaacumulada";
            colFreqAcum.UnboundType = UnboundColumnType.String;
            colFreqAcum.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colFreqAcum.HeaderStyle.Wrap = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            colFreqAcum.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colFreqAcum.DataItemTemplate = new TextBoxFreqAcumuladaTemplate();

            grdConsolidado.Columns.Add(colNotaAcum);
            grdConsolidado.Columns.Add(colFaltaAcum);
            grdConsolidado.Columns.Add(colFreqAcum);

            dtConsolidado = rnHistMatricula.ListaHistoricoMatriculasConsolidadoBimestralPor(int.Parse(tbAno.Text), int.Parse(tbPeriodo.Text), tbTurma.Text, cmbdisciplina.SelectedValue);

            if (dtConsolidado.Rows.Count > 0)
            {
                grdConsolidado.Visible = true;
                grdConsolidado.DataSource = dtConsolidado;
                grdConsolidado.DataBind();
            }
            else
            {
                ExibirMensagem("Não há alunos matriculados nesta turma.");
                grdConsolidado.Visible = false;
            }
        }

        protected void tcSubperiodo_OnActiveTabChanging(object source, TabControlCancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Tab.Name))
            {
                if (e.Tab.Name != "5")
                {
                    var subperiodo = Convert.ToDecimal(e.Tab.Name);
                    ProvaTurma rnProvaTurma = new ProvaTurma();

                    LyProva provaRow = rnProvaTurma.ConsultaProvaPor(cmbdisciplina.SelectedValue, tbTurma.Text, Convert.ToDecimal(tbAno.Text), Convert.ToDecimal(tbPeriodo.Text), subperiodo);

                    var freq = Nota.ObterFrequencia(tbAno.Text, tbPeriodo.Text, cmbdisciplina.SelectedValue, tbTurma.Text, subperiodo);

                    if ((hdnTemNota.Value == "S" && provaRow == null) || (hdnTemFreq.Value == "S" && freq == ""))
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não existem provas para este período letivo.');", true);
                        e.Cancel = true;
                        CarregaDados(Subperiodo.ToString());
                    }
                }
            }
        }

        private void CarregarAbas()
        {
            var subPeriodos = SubperiodoLetivo.ConsultarSubPeriodosLetivos(Convert.ToDecimal(this.tbAno.Text), this.tbPeriodo.Text);

            this.tcSubperiodo.Tabs.Clear();

            foreach (DataRow row in subPeriodos.Rows)
            {
                this.tcSubperiodo.Tabs.Add(row["descricao"].ToString(), row["subperiodo"].ToString());
            }
            if (this.tbPeriodo.Text == "0" && Convert.ToInt32(this.tbAno.Text) >= 2025)
            {
                this.tcSubperiodo.Tabs.Add("CONSOLIDADO TRIMESTRAL", "5");
            }
            else
            { 
                this.tcSubperiodo.Tabs.Add("CONSOLIDADO BIMESTRAL", "5"); 
            }
            
        }

        private void CarregarDadosGrid()
        {
            if (this.cmbdisciplina.SelectedValue == "Selecione")
            {
                return;
            }

            var disciplina = this.cmbdisciplina.SelectedValue;
            var dadosNotas = Disciplina.ConsultarDisciplinaConceitos(disciplina);

            this.hdnGrupoNota.Value = dadosNotas.GrupoNota;
            this.hdnNCasasDec.Value = dadosNotas.CasasDecimais.ToString();

            if (this.grdMatriculas.VisibleRowCount > 0 || this.grdConsolidado.VisibleRowCount > 0)
            {
                this.grdMatriculas.Visible = true;
                this.btnSalvar.Visible = this.btnCancelar.Visible = true;
                btnImprimirComp.Visible = true;
            }
            else
            {
                this.ExibirMensagem("Selecione uma disciplina que tenha alunos.");
                this.grdMatriculas.Visible = false;
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;
                btnImprimirComp.Visible = false;
            }
        }

        private void CarregarDadosIniciais(decimal gradeId)
        {
            RN.Disciplina rnDisciplina = new Disciplina();

            var dt = GradeSerie.ConsultarGrade(gradeId);

            if (dt.Rows.Count > 0)
            {
                this.hdnGradeID.Value = gradeId.ToString();

                this.tbUnidadeEnsino.Text = dt.Rows[0]["NOME_COMP"].ToString();
                this.tbTurma.Text = dt.Rows[0]["GRADE"].ToString();
                this.tbAno.Text = dt.Rows[0]["ANO"].ToString();
                this.tbPeriodo.Text = dt.Rows[0]["SEMESTRE"].ToString();
                this.tbEscolaridade.Text = dt.Rows[0]["TITULO"].ToString();
                this.tbMatrizCurricular.Text = dt.Rows[0]["CURRICULO"].ToString();
                this.tbAnoEscolar.Text = dt.Rows[0]["SERIE"].ToString();
                this.tbTurno.Text = dt.Rows[0]["NOME_TURNO"].ToString();

                var subPeriodo = PeriodoLetivo.ObterSubPeriodoAtual(this.tbAno.Text, this.tbPeriodo.Text).ToString();

                this.Subperiodo = Convert.ToDecimal(subPeriodo);

                this.cmbdisciplina.DataSource = rnDisciplina.ObtemDisciplinaNotasHistoricoPor(this.hdnGradeID.Value, this.tbTurma.Text, this.tbAno.Text, this.tbPeriodo.Text);
                this.cmbdisciplina.Items.Insert(0, "Selecione");
                this.cmbdisciplina.DataBind();

                CarregaMaterialEstudo();
            }
            else
            {
                this.LimparCampo();
            }
        }

        private void ExibirMensagem(string mensagem)
        {
            this.lblMensagem.Visible = !string.IsNullOrEmpty(mensagem);
            this.lblMensagem.Text = mensagem + "<br/>";
        }

        private void LimparCampo()
        {
            this.tbUnidadeEnsino.Text = string.Empty;
            this.tbTurma.Text = string.Empty;
            this.tbAno.Text = string.Empty;
            this.tbPeriodo.Text = string.Empty;
            this.tbEscolaridade.Text = string.Empty;
            this.tbMatrizCurricular.Text = string.Empty;
            this.tbAnoEscolar.Text = string.Empty;
            this.tbTurno.Text = string.Empty;
        }

        private string MontarQueryString()
        {
            var queryString = new StringBuilder();

            queryString.Append("ano=" + this.tbAno.Text);
            queryString.Append("&semestre=" + this.tbPeriodo.Text);
            queryString.Append("&turno=" + this.tbTurno.Text);
            queryString.Append("&curso=" + this.tbEscolaridade.Text);
            queryString.Append("&nucleo=" + this.ObjetoTurma.Nucleo);
            queryString.Append("&municipio=" + this.ObjetoTurma.Municipio);
            queryString.Append("&serie=" + this.ObjetoTurma.Serie);
            queryString.Append("&grade=" + this.ObjetoTurma.Grade);
            queryString.Append("&faculdade=" + this.ObjetoTurma.Faculdade);
            queryString.Append("&unidadeResponsavel=" + this.ObjetoTurma.UnidadeResponsavel);

            return queryString.ToString();
        }

        private decimal? ObtemGradeId()
        {
            try
            {
                var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                var decodedText = Encoding.UTF8.GetString(decodedBytes);
                this.ObterDadosQueryString(decodedText);

                decimal grade_id;

                if (decimal.TryParse(this.ObjetoTurma.Grade_ID.Replace("grade_id=", string.Empty), out grade_id))
                {
                    return grade_id;
                }

                return null;
            }
            catch
            {
                return null;
            }
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
                    this.ObjetoTurma.Nucleo = dados.Substring(dados.LastIndexOf('=') + 1);
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
            //this.upnlMatriculas.Visible = true;
            this.btnSalvar.Visible = this.btnCancelar.Visible = false;
            pnMensagemHdn.Visible = false;
            btnImprimirComp.Visible = false;
        }


        private void SelecionarAba(string subPeriodo)
        {
            if (this.tcSubperiodo.Tabs.Count == 0)
            {
                this.lblMensagem.Text = "Não existe Período Letivo ativo.";
                return;
            }

            if (this.tcSubperiodo.Tabs.FindByName(subPeriodo) != null)
            {
                this.tcSubperiodo.ActiveTab = this.tcSubperiodo.Tabs.FindByName(subPeriodo);
            }
            else
            {
                this.tcSubperiodo.ActiveTab = this.tcSubperiodo.Tabs[0];
            }

            this.Subperiodo = Convert.ToDecimal(this.tcSubperiodo.ActiveTab.Name);
        }

    }
}
