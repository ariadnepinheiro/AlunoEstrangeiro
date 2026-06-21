using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Helpers;
using System.Data;

namespace Techne.Lyceum.Net.Academico
{
    [
    NavUrl("~/Academico/FrequenciaEmergencialCorrecao.aspx"),
    ControlText("FrequenciaEmergencialCorrecao"),
    Title("Controle Frequência - Correção"),
]
    public partial class FrequenciaEmergencialCorrecao : TPage
    {
        public object Lista(object turma, object ano, object periodo, object mes)
        {
            RN.PeDeMeiaCorrecao.AlunoPeDeMeiaCorrecao rnAlunoPeDeMeiaCorrecao = new Techne.Lyceum.RN.PeDeMeiaCorrecao.AlunoPeDeMeiaCorrecao();

            var anofiltro = Convert.ToString(ano);
            var periodofiltro = Convert.ToString(periodo);
            var turmafiltro = Convert.ToString(turma);
            var mesfiltro = Convert.ToString(mes);

            if (!turmafiltro.IsNullOrEmptyOrWhiteSpace() && !anofiltro.IsNullOrEmptyOrWhiteSpace() && !periodofiltro.IsNullOrEmptyOrWhiteSpace() && !mesfiltro.IsNullOrEmptyOrWhiteSpace())
            {
                return rnAlunoPeDeMeiaCorrecao.ListaMatriculasElegiveisCorrecaoPor(turmafiltro, Convert.ToInt32(anofiltro), Convert.ToInt32(periodofiltro), Convert.ToInt32(mesfiltro));
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.PeDeMeiaCorrecao.PeriodoFrequenciaAluno rnPeriodoFrequenciaAluno = new Techne.Lyceum.RN.PeDeMeiaCorrecao.PeriodoFrequenciaAluno();
                this.lblMensagem.Text = string.Empty;
                lblMensagemFixa.Text = @"Com o objetivo de otimizar o registro das informações, todos os dias letivos estão preenchidos com a marcação de frequência.<br/>
                                         A ausência deverá ser informada desmarcando o(s) dia(s) letivo(s) em que o estudante não compareceu à unidade escolar.";

                if (!IsPostBack)
                {
                    LimparCampos();
                    pnlGridMatriculas.Visible = false;
                    btnSalvar.Visible = false;

                    if (!rnPeriodoFrequenciaAluno.PossuiPeriodoLancamentoAbertoPor(DateTime.Now.Date.Year, DateTime.Now.Date))
                    {
                        lblMensagem.Text = "Não existe período de lançamento vigente.";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdFrequencia, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnBuscar, AcaoControle.novo);
            ControlaAcesso(grdFrequencia);
        }


        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void CarregaTurma()
        {
            RN.PeDeMeiaCorrecao.AlunoPeDeMeiaCorrecao rnAlunoPeDeMeiaCorrecao = new Techne.Lyceum.RN.PeDeMeiaCorrecao.AlunoPeDeMeiaCorrecao();
            ddlTurma.Items.Clear();

            if (ddlAno.SelectedValue != "Selecione" && ddlPeriodo.SelectedValue != "Selecione" && !tseUnidadeResponsavel.DBValue.IsNull)
            {
                ddlTurma.DataSource = rnAlunoPeDeMeiaCorrecao.ListaTurmaPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), tseUnidadeResponsavel.DBValue.ToString());
                ddlTurma.Items.Insert(0, "Selecione");
                ddlTurma.DataBind();
            }
        }


        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ddlPeriodo.Items.Clear();
                this.ddlMes.ClearSelection();
                ddlTurma.Items.Clear();
                this.pnlGridMatriculas.Visible = false;
                btnSalvar.Visible = false;

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    ListItem item = new ListItem("Selecione", string.Empty);
                    this.ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodoFrequencia(this.ddlAno.SelectedValue);
                    this.ddlPeriodo.DataBind();
                    this.ddlPeriodo.Items.Insert(0, item);
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
                this.ddlTurma.Items.Clear();
                this.ddlMes.ClearSelection();
                ddlTurma.Items.Clear();
                this.pnlGridMatriculas.Visible = false;
                btnSalvar.Visible = false;

                if (!string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {
                    CarregaTurma();
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
                lblMensagemLancamento.Text = string.Empty;
                this.pnlGridMatriculas.Visible = false;
                btnSalvar.Visible = false;

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && !string.IsNullOrEmpty(ddlTurma.SelectedValue) && !string.IsNullOrEmpty(ddlMes.SelectedValue))
                {                  
                   CarregaDiasLetivos();    
                }
                else
                {
                    lblMensagem.Text = "Para selecionar o mês de referência é necessário ter escolhido Unidade de Ensino/Ano/Período/Turma.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void VerificaDadosLancamento()
        {
            RN.PeDeMeiaCorrecao.FrequenciaTurma rnFrequenciaTurma = new Techne.Lyceum.RN.PeDeMeiaCorrecao.FrequenciaTurma();
            string dadosLancamento = rnFrequenciaTurma.RetornaDadosLancamento(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlTurma.SelectedValue, Convert.ToInt32(ddlMes.SelectedValue));
            lblMensagemLancamento.Text = string.Empty;

            if (!dadosLancamento.IsNullOrEmptyOrWhiteSpace())
            {
                lblMensagemLancamento.Text = string.Format(@"O lançamento da frequência ocorreu em {0}.",
                        dadosLancamento);
            }
        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                this.pnlGridMatriculas.Visible = false;
                btnSalvar.Visible = false;

                if (!string.IsNullOrEmpty(ddlTurma.SelectedValue))
                {
                    
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void CarregaDiasLetivos()
        {
            RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();
            List<DateTime> diasLetivos = new List<DateTime>();

            if (!string.IsNullOrEmpty(ddlMes.SelectedValue))
            {
                diasLetivos = rnDiasNaoLetivos.RetornaDiasLetivosPor(tseMunicipio.DBValue.ToString(), Convert.ToInt32(ddlMes.SelectedValue), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue));

                Session["dias"] = diasLetivos;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = Techne.Lyceum.RN.SessaoUsuario.GetSessaoUsuario();
                tseUnidadeResponsavel.ResetValue();
                this.ddlAno.Items.Clear();
                this.ddlPeriodo.Items.Clear();
                this.ddlMes.ClearSelection();
                ddlTurma.Items.Clear();
                this.pnlGridMatriculas.Visible = false;
                btnSalvar.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);

                            sessao.Escola = string.Empty;
                            this.tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
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
            try
            {
                var sessao = SessaoUsuario.GetSessaoUsuario();
                this.ddlAno.Items.Clear();
                this.ddlPeriodo.Items.Clear();
                this.ddlMes.ClearSelection();
                ddlTurma.Items.Clear();
                btnSalvar.Visible = false;
                this.pnlGridMatriculas.Visible = false;


                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            CarregaAno();
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
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

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            this.tseMunicipio.ResetValue();
            this.tseUnidadeResponsavel.ResetValue();
            this.ddlAno.SelectedIndex = 0;
            this.ddlPeriodo.Items.Clear();
            this.ddlMes.ClearSelection();
            this.ddlTurma.Items.Clear();
            lblMensagemLancamento.Text = string.Empty;
            this.pnlGridMatriculas.Visible = false;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PeDeMeiaCorrecao.PeriodoFrequenciaAluno rnPeriodoFrequenciaAluno = new Techne.Lyceum.RN.PeDeMeiaCorrecao.PeriodoFrequenciaAluno();               
                
                this.pnlGridMatriculas.Visible = false;
                btnSalvar.Visible = false;
                if ((tseUnidadeResponsavel.DBValue.IsNull && !tseUnidadeResponsavel.IsValidDBValue) || ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Para efetuar a busca da lista dos alunos elegíveis é necessário preencher todos os campos de filtro.";
                    return;
                }
                else
                {
                    List<DateTime> diasLetivos = new List<DateTime>();

                    diasLetivos = (List<DateTime>)Session["dias"];

                    if (diasLetivos == null)
                    {
                        CarregaDiasLetivos();
                        diasLetivos = (List<DateTime>)Session["dias"];
                    }

                    grdFrequencia.DataBind();                    

                    if (grdFrequencia.VisibleRowCount > 0)
                    {
                        this.pnlGridMatriculas.Visible = true;

                        if (!rnPeriodoFrequenciaAluno.PossuiPeriodoLancamentoAbertoPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), DateTime.Now.Date))
                        {
                            lblMensagem.Text = "Não existe período de lançamento vigente para este Ano/Mês.";
                          
                        }
                        else
                        {
                            btnSalvar.Visible = true;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Não existe aluno elegível para esta Escola/Turma/Mês de referência.";
                        this.pnlGridMatriculas.Visible = false;
                    }
                    VerificaDadosLancamento();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
     
        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                RN.PeDeMeiaCorrecao.FrequenciaTurma rnFrequenciaTurma = new Techne.Lyceum.RN.PeDeMeiaCorrecao.FrequenciaTurma();
                List<RN.PeDeMeiaCorrecao.Entidades.FrequenciaAluno> lsAlunos = new List<RN.PeDeMeiaCorrecao.Entidades.FrequenciaAluno>();
                RN.PeDeMeiaCorrecao.Entidades.FrequenciaAluno dados = new Techne.Lyceum.RN.PeDeMeiaCorrecao.Entidades.FrequenciaAluno();
                RN.PeDeMeiaCorrecao.Entidades.FrequenciaTurma dadosTurma = new Techne.Lyceum.RN.PeDeMeiaCorrecao.Entidades.FrequenciaTurma();
                List<string> alunos = new List<string>();

                for (var rowIndex = 0; rowIndex < this.grdFrequencia.VisibleRowCount; rowIndex++)
                {
                    dadosTurma.Censo = tseUnidadeResponsavel.DBValue.ToString();
                    dadosTurma.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                    dadosTurma.Periodo = !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodo.SelectedValue) : -1;
                    dadosTurma.Turma = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurma.SelectedValue : null;
                    dadosTurma.MesReferencia = !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMes.SelectedValue) : -1;
                    dadosTurma.UsuarioID = User.Identity.Name;                  

                    for (int i = 1; i < 31; i++)
                    {
                        List<DateTime> diasLetivos = new List<DateTime>();

                        diasLetivos = (List<DateTime>)Session["dias"];

                        if (diasLetivos == null)
                        {
                            CarregaDiasLetivos();
                            diasLetivos = (List<DateTime>)Session["dias"];
                        }

                        var valor = i.ToString() + "/" + ddlMes.SelectedValue + "/" + ddlAno.SelectedValue;

                        DateTime resultado;
                        if (DateTime.TryParse(valor, out resultado))
                        {
                            var data = Convert.ToDateTime(valor);

                            if (diasLetivos.Contains(data))
                            {
                                var descColuna = grdFrequencia.Columns[i.ToString()] as GridViewDataColumn;

                                var coluna = ("chkFrequencia" + i).ToString();

                                var chkFrequencia = DevExpressHelper.GetControl<CheckBox>(this.grdFrequencia, rowIndex, descColuna.Caption, coluna);

                                var aluno = this.grdFrequencia.GetRowValues(rowIndex, "ALUNO").ToString();

                                alunos.Add(aluno);

                                if (!chkFrequencia.Checked)
                                {
                                    dados = new Techne.Lyceum.RN.PeDeMeiaCorrecao.Entidades.FrequenciaAluno();

                                    dados.Aluno = aluno;
                                    dados.UsuarioID = User.Identity.Name;
                                    dados.DataAusencia = data;

                                    lsAlunos.Add(dados);
                                }
                            }
                        }
                    }
                }

                List<string> listaAlunos = alunos.Distinct().ToList();
                validacao = rnFrequenciaTurma.Valida(dadosTurma, lsAlunos, listaAlunos, false);

                if (validacao.Valido)
                {
                    rnFrequenciaTurma.Salva(dadosTurma, lsAlunos, listaAlunos);
                    lblMensagem.Text = "Frequência realizada com sucesso.";

                    VerificaDadosLancamento();
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

        protected void grdFrequencia_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name.ToString() != "aluno" && e.DataColumn.Name.ToString() != "nome_compl")
            {
                List<DateTime> diasLetivos = new List<DateTime>();

                diasLetivos = (List<DateTime>)Session["dias"];

                var valor = e.DataColumn.Name.ToString() + "/" + ddlMes.SelectedValue + "/" + ddlAno.SelectedValue;

                DateTime resultado;
                if (!DateTime.TryParse(valor, out resultado))
                {                  
                    e.Cell.Text = string.Empty;
                }
                else
                {
                    var data = Convert.ToDateTime(valor);

                    if (diasLetivos == null)
                    {
                        CarregaDiasLetivos();
                        diasLetivos = (List<DateTime>)Session["dias"];
                    }

                    if (diasLetivos != null)
                    {
                        if (!diasLetivos.Contains(data))
                        {
                            if (data.DayOfWeek == DayOfWeek.Saturday)
                            {
                                e.Cell.BackColor = System.Drawing.Color.FromArgb(212, 229, 187);
                                e.Cell.Text = "S";

                            }
                            else if (data.DayOfWeek == DayOfWeek.Sunday)
                            {
                                e.Cell.BackColor = System.Drawing.Color.FromArgb(212, 229, 187);
                                e.Cell.Text = "D";
                            }
                            else
                            {
                                e.Cell.BackColor = System.Drawing.Color.FromArgb(247, 175, 132);
                                e.Cell.Text = "DNL";
                                //e.Cell.Text = Convert.ToString(valor) + "/" + diasLetivos.Count.ToString();
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Favor efetuar a busca novamente.";
                        pnlGridMatriculas.Visible = false;
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

            if (Convert.ToBoolean(valor))
            {
                return true;
            }

            return false;
        }
    }
}
